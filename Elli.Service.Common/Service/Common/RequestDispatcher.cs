// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.RequestDispatcher
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using Elli.Service.Common.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Service.Common
{
  public class RequestDispatcher : Disposable, IRequestDispatcher, IDisposable
  {
    private readonly IRequestProcessor _requestProcessor;
    private readonly ICacheManager _cacheManager;
    private Dictionary<string, Type> _keyToTypes;
    private List<Request> _requests;
    private Response[] _responses;
    protected Dictionary<string, int> KeyToResultPositions;

    public RequestDispatcher(IRequestProcessor requestProcessor, ICacheManager cacheManager)
    {
      this._requestProcessor = requestProcessor;
      this._cacheManager = cacheManager;
      this.InitializeState();
    }

    private void InitializeState()
    {
      this._requests = new List<Request>();
      this._responses = (Response[]) null;
      this._keyToTypes = new Dictionary<string, Type>();
      this.KeyToResultPositions = new Dictionary<string, int>();
    }

    public IEnumerable<Request> SentRequests => (IEnumerable<Request>) this._requests;

    public IEnumerable<Response> Responses
    {
      get
      {
        this.SendRequestsIfNecessary();
        return (IEnumerable<Response>) this._responses;
      }
    }

    public virtual void Add(params Request[] requestsToAdd)
    {
      foreach (Request request in requestsToAdd)
        this.Add(request);
    }

    public virtual void Add<TRequest>(Action<TRequest> action) where TRequest : Request, new()
    {
      TRequest request = new TRequest();
      action(request);
      this.Add((Request) request);
    }

    public virtual void Add(Request request) => this.AddRequest(request, false);

    public virtual void Add(string key, Request request)
    {
      if (this._keyToTypes.Keys.Contains<string>(key))
        throw new InvalidOperationException(string.Format("A request has already been added using the key '{0}'.", (object) key));
      this._keyToTypes[key] = request.GetType();
      this.AddRequest(request, true);
      this.KeyToResultPositions[key] = this._requests.Count - 1;
    }

    public virtual void Send(params OneWayRequest[] oneWayRequests)
    {
      this.BeforeSendingRequests((IEnumerable<Request>) oneWayRequests);
      this._requestProcessor.ProcessOneWayRequests(oneWayRequests);
      this.AfterSendingRequests((IEnumerable<Request>) oneWayRequests);
    }

    public virtual bool HasResponse<TResponse>() where TResponse : Response
    {
      this.SendRequestsIfNecessary();
      return Enumerable.OfType<TResponse>(this._responses).Any<TResponse>();
    }

    private bool HasResponse(string key)
    {
      this.SendRequestsIfNecessary();
      return this.KeyToResultPositions.ContainsKey(key);
    }

    private bool HasMoreThanOneResponse<TResponse>() where TResponse : Response
    {
      this.SendRequestsIfNecessary();
      return Enumerable.OfType<TResponse>(this._responses).Count<TResponse>() > 1;
    }

    public virtual TResponse Get<TResponse>() where TResponse : Response
    {
      this.SendRequestsIfNecessary();
      if (!this.HasResponse<TResponse>())
        throw new InvalidOperationException(string.Format("There is no response with type {0}. Maybe you called Clear before or forgot to add appropriate request first.", (object) typeof (TResponse).FullName));
      if (this.HasMoreThanOneResponse<TResponse>())
        throw new InvalidOperationException(string.Format("There is more than one response with type {0}. If two request handlers return responses with the same type, you need to add requests using Add(string key, Request request).", (object) typeof (TResponse).FullName));
      return Enumerable.OfType<TResponse>(this._responses).Single<TResponse>();
    }

    public virtual TResponse Get<TResponse>(string key) where TResponse : Response
    {
      this.SendRequestsIfNecessary();
      if (!this.HasResponse(key))
        throw new InvalidOperationException(string.Format("There is no response with key '{0}'. Maybe you called Clear before or forgot to add appropriate request first.", (object) key));
      return (TResponse) this._responses[this.KeyToResultPositions[key]];
    }

    public virtual TResponse Get<TResponse>(Request request) where TResponse : Response
    {
      this.Add(request);
      return this.Get<TResponse>();
    }

    public virtual void Clear() => this.InitializeState();

    protected override void DisposeManagedResources()
    {
      if (this._requestProcessor == null)
        return;
      this._requestProcessor.Dispose();
    }

    protected virtual Response[] GetResponses(params Request[] requestsToProcess)
    {
      this.BeforeSendingRequests((IEnumerable<Request>) requestsToProcess);
      Response[] responses = new Response[requestsToProcess.Length];
      List<Request> requestsToSend = new List<Request>((IEnumerable<Request>) requestsToProcess);
      this.GetCachedResponsesAndRemoveThoseRequests(requestsToProcess, responses, requestsToSend);
      Request[] array = requestsToSend.ToArray();
      if (requestsToSend.Count > 0)
      {
        Response[] receivedResponses = this._requestProcessor.Process(array);
        this.AddCacheableResponsesToCache(receivedResponses, array);
        this.PutReceivedResponsesInTempResponseArray(responses, receivedResponses);
      }
      this.AfterSendingRequests((IEnumerable<Request>) requestsToProcess);
      this.BeforeReturningResponses((IEnumerable<Response>) responses);
      return responses;
    }

    private void GetCachedResponsesAndRemoveThoseRequests(
      Request[] requestsToProcess,
      Response[] tempResponseArray,
      List<Request> requestsToSend)
    {
      for (int index = 0; index < requestsToProcess.Length; ++index)
      {
        Request request = requestsToProcess[index];
        if (this._cacheManager.IsCachingEnabledFor(request.GetType()))
        {
          Response cachedResponseFor = this._cacheManager.GetCachedResponseFor(request);
          if (cachedResponseFor != null)
          {
            tempResponseArray[index] = cachedResponseFor;
            requestsToSend.Remove(request);
          }
        }
      }
    }

    private void AddCacheableResponsesToCache(
      Response[] receivedResponses,
      Request[] requestsToSend)
    {
      for (int index = 0; index < receivedResponses.Length; ++index)
      {
        if (receivedResponses[index].ExceptionType == ExceptionType.None && this._cacheManager.IsCachingEnabledFor(requestsToSend[index].GetType()))
          this._cacheManager.StoreInCache(requestsToSend[index], receivedResponses[index]);
      }
    }

    private void PutReceivedResponsesInTempResponseArray(
      Response[] tempResponseArray,
      Response[] receivedResponses)
    {
      int num = 0;
      for (int index = 0; index < tempResponseArray.Length; ++index)
      {
        if (tempResponseArray[index] == null)
          tempResponseArray[index] = receivedResponses[num++];
      }
    }

    protected virtual void BeforeSendingRequests(IEnumerable<Request> requestsToProcess)
    {
    }

    protected virtual void AfterSendingRequests(IEnumerable<Request> sentRequests)
    {
    }

    protected virtual void BeforeReturningResponses(IEnumerable<Response> receivedResponses)
    {
    }

    private void SendRequestsIfNecessary()
    {
      if (this.RequestsSent())
        return;
      this._responses = this.GetResponses(this._requests.ToArray());
      this.DealWithPossibleExceptions((IEnumerable<Response>) this._responses);
    }

    private bool RequestsSent() => this._responses != null;

    private void DealWithPossibleExceptions(IEnumerable<Response> responsesToCheck)
    {
      foreach (Response response in responsesToCheck)
      {
        if (response.ExceptionType == ExceptionType.Security)
          this.DealWithSecurityException(response.Exception);
        if (response.ExceptionType == ExceptionType.Unknown)
          this.DealWithUnknownException(response.Exception);
      }
    }

    protected virtual void DealWithUnknownException(ExceptionInfo exception)
    {
    }

    protected virtual void DealWithSecurityException(ExceptionInfo exceptionDetail)
    {
    }

    private void AddRequest(Request request, bool wasAddedWithKey)
    {
      if (this.RequestsSent())
        throw new InvalidOperationException("Requests where already send. Either add request earlier or call Clear.");
      Type type = request.GetType();
      if (this.RequestTypeIsAlreadyPresent(type) && (this.RequestTypeIsNotAssociatedWithKey(type) || !wasAddedWithKey))
        throw new InvalidOperationException(string.Format("A request of type {0} has already been added. Please add requests of the same type with a different key.", (object) type.FullName));
      this._requests.Add(request);
    }

    private bool RequestTypeIsNotAssociatedWithKey(Type requestType)
    {
      return !this._keyToTypes.Values.Contains<Type>(requestType);
    }

    private bool RequestTypeIsAlreadyPresent(Type requestType)
    {
      return this._requests.Any<Request>((Func<Request, bool>) (r => r.GetType().Equals(requestType)));
    }
  }
}
