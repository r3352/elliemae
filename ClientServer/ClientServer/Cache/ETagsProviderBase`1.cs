// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Cache.ETagsProviderBase`1
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Cache
{
  public abstract class ETagsProviderBase<T> : IETagsProvider
  {
    public abstract void GetClientETags(
      IClientCacheStore cacheStore,
      Dictionary<string, string> clientETags,
      params object[] methodArgs);

    public abstract T GetServerValueAndETags(
      T originalServerValue,
      Dictionary<string, string> clientETags,
      Dictionary<string, string> serverETags,
      params object[] methodArgs);

    public object GetServerValueAndETags(
      object originalServerValue,
      Dictionary<string, string> clientETags,
      Dictionary<string, string> serverETags,
      params object[] methodArgs)
    {
      return (object) this.GetServerValueAndETags((T) originalServerValue, clientETags, serverETags, methodArgs);
    }

    public abstract T MergeWithCache(
      IClientCacheStore cacheStore,
      T serverValue,
      Dictionary<string, string> serverETags);

    public object MergeWithCache(
      IClientCacheStore cacheStore,
      object serverValue,
      Dictionary<string, string> serverETags)
    {
      return (object) this.MergeWithCache(cacheStore, (T) serverValue, serverETags);
    }
  }
}
