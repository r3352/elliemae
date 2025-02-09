// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.AsyncRequestDispatcher
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using Elli.Service.Common.Caching;
using Elli.Service.Common.InversionOfControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

#nullable disable
namespace Elli.Service.Common
{
  public class AsyncRequestDispatcher : Disposable, IAsyncRequestDispatcher, IDisposable
  {
    private readonly IAsyncRequestProcessor _requestProcessor;
    private readonly ICacheManager _cacheManager;
    private Dictionary<string, Type> _keyToTypes;
    private bool _oneWayRequestsAdded;
    private bool _twoWayRequestsAdded;
    private List<Request> _queuedRequests;
    private List<OneWayRequest> _queuedOneWayRequests;
    protected Dictionary<string, int> KeyToResultPositions;

    public AsyncRequestDispatcher(
      IAsyncRequestProcessor requestProcessor,
      ICacheManager cacheManager)
    {
      this._requestProcessor = requestProcessor;
      this._cacheManager = cacheManager;
      this.InitializeState();
    }

    public virtual Request[] QueuedRequests => this._queuedRequests.ToArray();

    public virtual void Add(params Request[] requestsToAdd)
    {
      foreach (Request request in requestsToAdd)
        this.Add(request);
    }

    public virtual void Add(string key, Request request)
    {
      this.AddRequest(request, true);
      this._keyToTypes[key] = request.GetType();
      this.KeyToResultPositions[key] = this._queuedRequests.Count - 1;
    }

    public virtual void Add<TRequest>(Action<TRequest> action) where TRequest : Request, new()
    {
      TRequest request = new TRequest();
      action(request);
      this.Add((Request) request);
    }

    public virtual void Add(Request request) => this.AddRequest(request, false);

    public virtual void Add(params OneWayRequest[] oneWayRequests)
    {
      this.EnsureWeOnlyHaveOneWayRequests();
      this._queuedOneWayRequests.AddRange((IEnumerable<OneWayRequest>) oneWayRequests);
    }

    public virtual void ProcessOneWayRequests()
    {
      OneWayRequest[] array = this._queuedOneWayRequests.ToArray();
      this.BeforeSendingRequests((IEnumerable<Request>) array);
      this._requestProcessor.ProcessOneWayRequestsAsync(array, new Action<AsyncCompletedEventArgs>(this.OnProcessOneWayRequestsCompleted));
      this.AfterSendingRequests((IEnumerable<Request>) array);
      this._queuedOneWayRequests.Clear();
    }

    private void OnProcessOneWayRequestsCompleted(AsyncCompletedEventArgs args)
    {
      this.Dispose();
      if (args.Error != null)
        throw new InvalidOperationException("Exception occurred during processing of one-way requests", args.Error);
    }

    public virtual void ProcessRequests(
      Action<ReceivedResponses> receivedResponsesDelegate,
      Action<ExceptionInfo> exceptionOccurredDelegate)
    {
      this.ProcessRequests(new ResponseReceiver(receivedResponsesDelegate, exceptionOccurredDelegate, this.KeyToResultPositions, this._cacheManager));
    }

    public virtual void ProcessRequests(
      Action<ReceivedResponses> receivedResponsesDelegate,
      Action<ExceptionInfo, ExceptionType> exceptionAndTypeOccurredDelegate)
    {
      this.ProcessRequests(new ResponseReceiver(receivedResponsesDelegate, exceptionAndTypeOccurredDelegate, this.KeyToResultPositions, this._cacheManager));
    }

    private void ProcessRequests(ResponseReceiver responseReciever)
    {
      Request[] array = this._queuedRequests.ToArray();
      this.BeforeSendingRequests((IEnumerable<Request>) array);
      Response[] tempResponseArray = new Response[array.Length];
      List<Request> requestsToSend = new List<Request>((IEnumerable<Request>) array);
      this.GetCachedResponsesAndRemoveThoseRequests(array, tempResponseArray, requestsToSend);
      Request[] requestsToSendAsArray = requestsToSend.ToArray();
      if (requestsToSendAsArray.Length != 0)
        this._requestProcessor.ProcessRequestsAsync(requestsToSendAsArray, (Action<ProcessRequestsAsyncCompletedArgs>) (a => this.OnProcessRequestsCompleted(a, responseReciever, tempResponseArray, requestsToSendAsArray)));
      else
        (SynchronizationContext.Current ?? new SynchronizationContext()).Post((SendOrPostCallback) (s => this.OnProcessRequestsCompleted((ProcessRequestsAsyncCompletedArgs) null, responseReciever, tempResponseArray, requestsToSendAsArray)), (object) null);
      this.AfterSendingRequests((IEnumerable<Request>) array);
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

    protected virtual void BeforeSendingRequests(IEnumerable<Request> requestsToProcess)
    {
    }

    protected virtual void AfterSendingRequests(IEnumerable<Request> sentRequests)
    {
    }

    public virtual void OnProcessRequestsCompleted(
      ProcessRequestsAsyncCompletedArgs args,
      ResponseReceiver responseReciever,
      Response[] tempResponseArray,
      Request[] requestsToSendAsArray)
    {
      this.Dispose();
      responseReciever.ReceiveResponses(args, tempResponseArray, requestsToSendAsArray);
    }

    protected override void DisposeManagedResources()
    {
      SimpleInjector.Container container = IoC.Container;
      if (this._requestProcessor == null)
        return;
      this._requestProcessor.Dispose();
    }

    private void AddRequest(Request request, bool wasAddedWithKey)
    {
      this.EnsureWeOnlyHaveTwoWayRequests();
      Type type = request.GetType();
      if (this.RequestTypeIsAlreadyPresent(type) && (this.RequestTypeIsNotAssociatedWithKey(type) || !wasAddedWithKey))
        throw new InvalidOperationException(string.Format("A request of type {0} has already been added. Please add requests of the same type with a different key.", (object) type.FullName));
      this._queuedRequests.Add(request);
    }

    private bool RequestTypeIsAlreadyPresent(Type requestType)
    {
      return ((IEnumerable<Request>) this.QueuedRequests).Any<Request>((Func<Request, bool>) (r => r.GetType().Equals(requestType)));
    }

    private bool RequestTypeIsNotAssociatedWithKey(Type requestType)
    {
      return !this._keyToTypes.Values.Contains<Type>(requestType);
    }

    private void InitializeState()
    {
      this._queuedRequests = new List<Request>();
      this._queuedOneWayRequests = new List<OneWayRequest>();
      this._keyToTypes = new Dictionary<string, Type>();
      this.KeyToResultPositions = new Dictionary<string, int>();
    }

    private void EnsureWeOnlyHaveOneWayRequests()
    {
      if (this._twoWayRequestsAdded)
        this.ThrowInvalidUsageException();
      this._oneWayRequestsAdded = true;
    }

    private void EnsureWeOnlyHaveTwoWayRequests()
    {
      if (this._oneWayRequestsAdded)
        this.ThrowInvalidUsageException();
      this._twoWayRequestsAdded = true;
    }

    private void ThrowInvalidUsageException()
    {
      throw new InvalidOperationException("You cannot combine one-way and two-way requests in the same AsyncRequestDispatcher.");
    }
  }
}
