// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.IRequestDispatcher
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Service.Common
{
  public interface IRequestDispatcher : IDisposable
  {
    IEnumerable<Response> Responses { get; }

    void Add(Request request);

    void Add(params Request[] requestsToAdd);

    void Add(string key, Request request);

    void Add<TRequest>(Action<TRequest> action) where TRequest : Request, new();

    void Send(params OneWayRequest[] oneWayRequests);

    bool HasResponse<TResponse>() where TResponse : Response;

    TResponse Get<TResponse>() where TResponse : Response;

    TResponse Get<TResponse>(string key) where TResponse : Response;

    TResponse Get<TResponse>(Request request) where TResponse : Response;

    void Clear();
  }
}
