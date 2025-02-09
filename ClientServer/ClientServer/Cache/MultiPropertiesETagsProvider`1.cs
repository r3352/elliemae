// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Cache.MultiPropertiesETagsProvider`1
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Cache
{
  public class MultiPropertiesETagsProvider<T> : ETagsProviderBase<T>
  {
    private readonly List<IETagsProvider> _providers = new List<IETagsProvider>();

    public void Add<TProp>(string keyFormat, Func<T, TProp> get, Action<T, TProp> set)
    {
      this._providers.Add((IETagsProvider) new SinglePropertyETagsProvider<T, TProp>(keyFormat, get, set));
    }

    public override void GetClientETags(
      IClientCacheStore cacheStore,
      Dictionary<string, string> clientETags,
      params object[] methodArgs)
    {
      foreach (IETagsProvider provider in this._providers)
        provider.GetClientETags(cacheStore, clientETags, methodArgs);
    }

    public override T GetServerValueAndETags(
      T originalServerValue,
      Dictionary<string, string> clientETags,
      Dictionary<string, string> serverETags,
      params object[] methodArgs)
    {
      foreach (IETagsProvider provider in this._providers)
        provider.GetServerValueAndETags((object) originalServerValue, clientETags, serverETags, methodArgs);
      return originalServerValue;
    }

    public override T MergeWithCache(
      IClientCacheStore cacheStore,
      T serverValue,
      Dictionary<string, string> serverETags)
    {
      foreach (IETagsProvider provider in this._providers)
        provider.MergeWithCache(cacheStore, (object) serverValue, serverETags);
      return serverValue;
    }
  }
}
