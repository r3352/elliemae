// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.AsyncRequestDispatcherStub
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
  public class AsyncRequestDispatcherStub : Disposable, IAsyncRequestDispatcher, IDisposable
  {
    private readonly Dictionary<Type, string> _unkeyedTypesToAutoKey;
    private readonly Dictionary<string, Request> _requests;
    private readonly Dictionary<string, int> _responseKeyToIndexPosition;
    private readonly List<Response> _responsesToReturn;
    private readonly List<OneWayRequest> _oneWayRequests;
    private ResponseReceiver _responseReceiver;
    private bool _twoWayRequestsAdded;
    private bool _oneWayRequestsAdded;

    public AsyncRequestDispatcherStub()
    {
      this._unkeyedTypesToAutoKey = new Dictionary<Type, string>();
      this._requests = new Dictionary<string, Request>();
      this._responseKeyToIndexPosition = new Dictionary<string, int>();
      this._responsesToReturn = new List<Response>();
      this._oneWayRequests = new List<OneWayRequest>();
    }

    public void SetResponsesToReturn(params Response[] responses)
    {
      this._responsesToReturn.Clear();
      this._responsesToReturn.AddRange((IEnumerable<Response>) responses);
    }

    public void AddResponseToReturn(Response response, string key)
    {
      this._responsesToReturn.Add(response);
      this._responseKeyToIndexPosition.Add(key, this._responsesToReturn.Count - 1);
    }

    public bool HasRequest<TRequest>() where TRequest : Request
    {
      return this._unkeyedTypesToAutoKey.ContainsKey(typeof (TRequest));
    }

    public bool HasRequest<TRequest>(string key) where TRequest : Request
    {
      return this._requests.ContainsKey(key) && this._requests[key] is TRequest;
    }

    public bool HasOneWayRequest<TOneWayRequest>() where TOneWayRequest : OneWayRequest
    {
      return this._oneWayRequests.Any<OneWayRequest>((Func<OneWayRequest, bool>) (r => r.GetType() == typeof (TOneWayRequest)));
    }

    public TRequest GetRequest<TRequest>() where TRequest : Request
    {
      return (TRequest) this._requests[this._unkeyedTypesToAutoKey[typeof (TRequest)]];
    }

    public TRequest GetRequest<TRequest>(string key) where TRequest : Request
    {
      return (TRequest) this._requests[key];
    }

    public TOneWayRequest GetOneWayRequest<TOneWayRequest>() where TOneWayRequest : OneWayRequest
    {
      return !this._oneWayRequests.Any<OneWayRequest>((Func<OneWayRequest, bool>) (r => r.GetType() == typeof (TOneWayRequest))) ? this._oneWayRequests.FirstOrDefault<OneWayRequest>((Func<OneWayRequest, bool>) (r => r.GetType() == typeof (TOneWayRequest))) as TOneWayRequest : throw new InvalidOperationException(string.Format("Multiple OneWayRequests of type {0} have been added, use the GetOneWayRequests method instead to perform your inspection", (object) typeof (TOneWayRequest)));
    }

    public IEnumerable<OneWayRequest> GetOneWayRequests()
    {
      return (IEnumerable<OneWayRequest>) this._oneWayRequests;
    }

    public void ClearRequests()
    {
      this._unkeyedTypesToAutoKey.Clear();
      this._requests.Clear();
    }

    public void Add(Request request)
    {
      this.EnsureWeOnlyHaveTwoWayRequests();
      string key = Guid.NewGuid().ToString();
      this._unkeyedTypesToAutoKey.Add(request.GetType(), key);
      this._requests.Add(key, request);
    }

    public void Add(params Request[] requestsToAdd)
    {
      if (requestsToAdd == null)
        return;
      foreach (Request request in requestsToAdd)
        this.Add(request);
    }

    public void Add<TRequest>(Action<TRequest> action) where TRequest : Request, new()
    {
      TRequest request = new TRequest();
      action(request);
      this.Add((Request) request);
    }

    public void Add(string key, Request request)
    {
      this.EnsureWeOnlyHaveTwoWayRequests();
      this._requests.Add(key, request);
    }

    public void Add(params OneWayRequest[] oneWayRequests)
    {
      this.EnsureWeOnlyHaveOneWayRequests();
      this._oneWayRequests.AddRange((IEnumerable<OneWayRequest>) oneWayRequests);
    }

    public void ProcessOneWayRequests()
    {
    }

    public void ProcessRequests(
      Action<ReceivedResponses> receivedResponsesDelegate,
      Action<ExceptionInfo> exceptionOccurredDelegate)
    {
      this.ProcessRequests(new ResponseReceiver(receivedResponsesDelegate, exceptionOccurredDelegate, this._responseKeyToIndexPosition, (ICacheManager) null));
    }

    public void ProcessRequests(
      Action<ReceivedResponses> receivedResponsesDelegate,
      Action<ExceptionInfo, ExceptionType> exceptionAndTypeOccurredDelegate)
    {
      this.ProcessRequests(new ResponseReceiver(receivedResponsesDelegate, exceptionAndTypeOccurredDelegate, this._responseKeyToIndexPosition, (ICacheManager) null));
    }

    private void ProcessRequests(ResponseReceiver responseReceiver)
    {
      this._responseReceiver = responseReceiver;
    }

    public void ReturnResponses()
    {
      this._responseReceiver.ReceiveResponses(new ProcessRequestsAsyncCompletedArgs((object[]) new Response[1][]
      {
        this._responsesToReturn.ToArray()
      }, (Exception) null, false, (object) null), new Response[this._responsesToReturn.Count], this._requests.Values.ToArray<Request>());
    }

    protected override void DisposeManagedResources()
    {
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
