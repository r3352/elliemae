// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.Caching.CacheManager
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Service.Common.Caching
{
  public class CacheManager : ICacheManager
  {
    private const string DefaultRegionName = "_defaultRegion";
    private readonly CacheConfiguration _configuration;
    private readonly ICacheProvider _cacheProvider;
    private Dictionary<string, ICache> _caches;

    public CacheManager(CacheConfiguration configuration, ICacheProvider cacheProvider)
    {
      this._configuration = configuration;
      this._cacheProvider = cacheProvider;
      this.BuildCaches();
    }

    private void BuildCaches()
    {
      IEnumerable<string> regionNames = this._configuration.GetRegionNames();
      this._caches = new Dictionary<string, ICache>(regionNames.Count<string>() + 1);
      this._caches.Add("_defaultRegion", this._cacheProvider.BuildCache("_defaultRegion"));
      foreach (string str in regionNames)
        this._caches.Add(str, this._cacheProvider.BuildCache(str));
    }

    public virtual bool IsCachingEnabledFor(Type requestType)
    {
      return this._configuration.IsCachingEnabledFor(requestType);
    }

    public virtual Response GetCachedResponseFor(Request request)
    {
      return this.GetCachedResponseFor(request, this._configuration.GetRegionNameFor(request.GetType()) ?? "_defaultRegion");
    }

    protected virtual Response GetCachedResponseFor(Request request, string region)
    {
      return this._caches[region].GetCachedResponseFor(request);
    }

    public virtual void StoreInCache(Request request, Response response)
    {
      RequestCacheConfiguration configurationFor = this._configuration.GetConfigurationFor(request.GetType());
      this.StoreInCache(request, response, configurationFor.Expiration, configurationFor.Region ?? "_defaultRegion");
    }

    protected virtual void StoreInCache(
      Request request,
      Response response,
      TimeSpan expiration,
      string region)
    {
      Response shallowCopy = response.GetShallowCopy();
      shallowCopy.IsCached = true;
      this._caches[region].Store(request, shallowCopy, expiration);
    }

    public virtual void Clear(string region) => this._caches[region].Clear();
  }
}
