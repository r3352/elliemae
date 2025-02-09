// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.Caching.RequestCacheConfiguration
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;

#nullable disable
namespace Elli.Service.Common.Caching
{
  public class RequestCacheConfiguration
  {
    public Type RequestType { get; private set; }

    public TimeSpan Expiration { get; private set; }

    public string Region { get; private set; }

    public RequestCacheConfiguration(Type requestType, TimeSpan expiration, string region)
    {
      this.RequestType = requestType;
      this.Expiration = expiration;
      this.Region = region;
    }
  }
}
