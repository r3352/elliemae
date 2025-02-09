// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.ReceivedResponses
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Service.Common
{
  public class ReceivedResponses
  {
    private readonly Response[] _responses;
    private readonly Dictionary<string, int> _keyToResultPositions;

    public ReceivedResponses(Response[] responses)
      : this(responses, new Dictionary<string, int>())
    {
    }

    public ReceivedResponses(Response[] responses, Dictionary<string, int> keyToResultPositions)
    {
      this._responses = responses;
      this._keyToResultPositions = keyToResultPositions;
    }

    public IEnumerable<Response> Responses => (IEnumerable<Response>) this._responses;

    public virtual TResponse Get<TResponse>() where TResponse : Response
    {
      Type responseType = typeof (TResponse);
      return (TResponse) ((IEnumerable<Response>) this._responses).Single<Response>((Func<Response, bool>) (r => r.GetType().Equals(responseType)));
    }

    public virtual TResponse Get<TResponse>(string key) where TResponse : Response
    {
      return (TResponse) this._responses[this._keyToResultPositions[key]];
    }

    public virtual bool HasResponse<TResponse>() where TResponse : Response
    {
      return Enumerable.OfType<TResponse>(this._responses).Any<TResponse>();
    }
  }
}
