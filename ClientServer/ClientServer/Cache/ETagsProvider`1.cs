// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Cache.ETagsProvider`1
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Cache
{
  public class ETagsProvider<T> : ETagsProviderBase<T>
  {
    private readonly string _keyFormat;
    private string _cacheKey;
    private string _clientETag;

    public ETagsProvider(string keyFormat) => this._keyFormat = keyFormat;

    public override void GetClientETags(
      IClientCacheStore cacheStore,
      Dictionary<string, string> clientETags,
      params object[] methodArgs)
    {
      this._cacheKey = string.Format(this._keyFormat, methodArgs);
      this._clientETag = cacheStore.GetETag<T>(this._cacheKey);
      if (string.IsNullOrEmpty(this._cacheKey) || string.IsNullOrEmpty(this._clientETag))
        return;
      clientETags[this._cacheKey] = this._clientETag;
    }

    public override T GetServerValueAndETags(
      T originalServerValue,
      Dictionary<string, string> clientETags,
      Dictionary<string, string> serverETags,
      params object[] methodArgs)
    {
      string key = string.Format(this._keyFormat, methodArgs);
      string a = ObjectArrayHelpers.GetAggregateHash((object) key, (object) originalServerValue).ToString("X");
      if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(a))
        return originalServerValue;
      serverETags[key] = a;
      string b;
      if (!clientETags.TryGetValue(key, out b))
        b = (string) null;
      return string.Equals(a, b) ? default (T) : originalServerValue;
    }

    public override T MergeWithCache(
      IClientCacheStore cacheStore,
      T serverValue,
      Dictionary<string, string> serverETags)
    {
      string str;
      if (!serverETags.TryGetValue(this._cacheKey, out str))
      {
        cacheStore.Reset(this._cacheKey);
        return serverValue;
      }
      if (string.Equals(this._clientETag, str))
        return cacheStore.GetValue<T>(this._cacheKey);
      cacheStore.Store<T>(this._cacheKey, str, serverValue);
      return serverValue;
    }
  }
}
