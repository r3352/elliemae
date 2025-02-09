// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Cache.SinglePropertyETagsProvider`2
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Cache
{
  public class SinglePropertyETagsProvider<T, TProp> : ETagsProviderBase<T>
  {
    private readonly ETagsProvider<TProp> _innerETagProvider;
    private readonly Func<T, TProp> _get;
    private readonly Action<T, TProp> _set;

    public SinglePropertyETagsProvider(string keyFormat, Func<T, TProp> get, Action<T, TProp> set)
    {
      this._innerETagProvider = new ETagsProvider<TProp>(keyFormat);
      this._get = get;
      this._set = set;
    }

    public override void GetClientETags(
      IClientCacheStore cacheStore,
      Dictionary<string, string> clientETags,
      params object[] methodArgs)
    {
      this._innerETagProvider.GetClientETags(cacheStore, clientETags, methodArgs);
    }

    public override T GetServerValueAndETags(
      T originalServerValue,
      Dictionary<string, string> clientETags,
      Dictionary<string, string> serverETags,
      params object[] methodArgs)
    {
      TProp serverValueAndEtags = this._innerETagProvider.GetServerValueAndETags(this._get(originalServerValue), clientETags, serverETags, methodArgs);
      this._set(originalServerValue, serverValueAndEtags);
      return originalServerValue;
    }

    public override T MergeWithCache(
      IClientCacheStore cacheStore,
      T serverValue,
      Dictionary<string, string> serverETags)
    {
      TProp serverValue1 = this._get(serverValue);
      TProp prop = this._innerETagProvider.MergeWithCache(cacheStore, serverValue1, serverETags);
      this._set(serverValue, prop);
      return serverValue;
    }
  }
}
