// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.RequestDispatcherStub
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
  public class RequestDispatcherStub : RequestDispatcher
  {
    private readonly List<OneWayRequest> _oneWayRequests = new List<OneWayRequest>();
    private readonly List<Response> _responsesToReturn = new List<Response>();
    private readonly Dictionary<string, Request> _keyToRequest = new Dictionary<string, Request>();

    public RequestDispatcherStub()
      : base((IRequestProcessor) null, (ICacheManager) null)
    {
    }

    public void AddResponsesToReturn(params Response[] responses)
    {
      this._responsesToReturn.AddRange((IEnumerable<Response>) responses);
    }

    public void AddResponsesToReturn(Dictionary<string, Response> keyedResponses)
    {
      this._responsesToReturn.AddRange((IEnumerable<Response>) keyedResponses.Values);
      for (int index = 0; index < keyedResponses.Keys.Count; ++index)
      {
        string key = keyedResponses.Keys.ElementAt<string>(index);
        if (key != null)
          this.KeyToResultPositions.Add(key, index);
      }
    }

    public void AddResponseToReturn(Response response) => this._responsesToReturn.Add(response);

    public void AddResponseToReturn(string key, Response response)
    {
      this._responsesToReturn.Add(response);
      this.KeyToResultPositions.Add(key, this._responsesToReturn.Count - 1);
    }

    public override void Clear()
    {
    }

    public override void Add(string key, Request request)
    {
      base.Add(key, request);
      this._keyToRequest[key] = request;
    }

    public TRequest GetRequest<TRequest>() where TRequest : Request
    {
      return (TRequest) this.SentRequests.First<Request>((Func<Request, bool>) (r => r.GetType().Equals(typeof (TRequest))));
    }

    public TRequest GetRequest<TRequest>(string key) where TRequest : Request
    {
      return (TRequest) this._keyToRequest[key];
    }

    public bool HasRequest<TRequest>() where TRequest : Request
    {
      return this.SentRequests.Any<Request>((Func<Request, bool>) (r => r.GetType().Equals(typeof (TRequest))));
    }

    public bool HasOneWayRequest<TOneWayRequest>() where TOneWayRequest : OneWayRequest
    {
      return this._oneWayRequests.Any<OneWayRequest>((Func<OneWayRequest, bool>) (r => r.GetType() == typeof (TOneWayRequest)));
    }

    public TOneWayRequest GetOneWayRequest<TOneWayRequest>() where TOneWayRequest : OneWayRequest
    {
      return !this._oneWayRequests.Any<OneWayRequest>((Func<OneWayRequest, bool>) (r => r.GetType() == typeof (TOneWayRequest))) ? this._oneWayRequests.FirstOrDefault<OneWayRequest>((Func<OneWayRequest, bool>) (r => r.GetType() == typeof (TOneWayRequest))) as TOneWayRequest : throw new InvalidOperationException(string.Format("Multiple OneWayRequests of type {0} have been added, use the GetOneWayRequests method instead to perform your inspection", (object) typeof (TOneWayRequest)));
    }

    public IEnumerable<OneWayRequest> GetOneWayRequests()
    {
      return (IEnumerable<OneWayRequest>) this._oneWayRequests;
    }

    protected override Response[] GetResponses(params Request[] requestsToProcess)
    {
      return this._responsesToReturn.ToArray();
    }

    public override void Send(params OneWayRequest[] oneWayRequests)
    {
      this._oneWayRequests.AddRange((IEnumerable<OneWayRequest>) oneWayRequests);
      base.Send(oneWayRequests);
    }
  }
}
