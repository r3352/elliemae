// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.Configuration.RequestHandlerRegistry
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Service.Common.Configuration
{
  public class RequestHandlerRegistry : IRequestHandlerRegistry
  {
    private readonly List<Type> _requestHandlerTypes = new List<Type>();

    public IEnumerable<Type> GetTypedRequestHandlers()
    {
      return (IEnumerable<Type>) this._requestHandlerTypes;
    }

    public void Register(Type handlerType) => this._requestHandlerTypes.Add(handlerType);
  }
}
