// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Cache.RequestCache
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Cache;
using System;
using System.Collections.Concurrent;

#nullable disable
namespace EllieMae.EMLite.Common.Cache
{
  public class RequestCache : IDataCache
  {
    private readonly ConcurrentDictionary<string, object> cacheValues = new ConcurrentDictionary<string, object>();

    public T Get<T>(string key)
    {
      object obj;
      return this.cacheValues.TryGetValue(key, out obj) ? (T) obj : default (T);
    }

    public T Get<T>(string key, Func<T> loaderFunc)
    {
      return (T) this.cacheValues.GetOrAdd(key, (Func<string, object>) (v => (object) loaderFunc()));
    }

    public void Remove(string key) => this.cacheValues.TryRemove(key, out object _);

    public void Set<T>(string key, T value)
    {
      this.cacheValues.AddOrUpdate(key, (object) value, (Func<string, object, object>) ((k, v) => (object) (T) value));
    }
  }
}
