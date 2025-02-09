// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Cache.ShadowedCache
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Cache
{
  public class ShadowedCache : IFileCache, IDisposable
  {
    private IFileCache baseCache;
    private IFileCache shadowCache;

    public ShadowedCache(string systemId, string cacheName)
      : this(systemId, cacheName, TimeSpan.FromSeconds(1.0))
    {
    }

    public ShadowedCache(string systemId, string cacheName, TimeSpan systemCacheTimeout)
    {
      this.shadowCache = (IFileCache) new ClientCache(systemId, cacheName, CacheScope.Session);
      try
      {
        this.baseCache = (IFileCache) new ClientCache(systemId, cacheName, CacheScope.System, systemCacheTimeout);
      }
      catch
      {
        this.baseCache = this.shadowCache;
      }
    }

    public string GetFilePath(string fileName)
    {
      this.ensureConsistent(fileName);
      return this.shadowCache.GetFilePath(fileName);
    }

    public bool Exists(string fileName) => this.baseCache.Exists(fileName);

    public BinaryObject Get(string fileName)
    {
      this.ensureConsistent(fileName);
      return this.shadowCache.Get(fileName);
    }

    public void Put(string fileName, BinaryObject data)
    {
      this.baseCache.Put(fileName, data);
      this.ensureConsistent(fileName);
    }

    public void Put(string fileName, BinaryObject data, DateTime lastModificationTime)
    {
      this.baseCache.Put(fileName, data, lastModificationTime);
      this.ensureConsistent(fileName);
    }

    public void Delete(string fileName)
    {
      this.baseCache.Delete(fileName);
      this.ensureConsistent(fileName);
    }

    public DateTime GetLastModificationDate(string fileName)
    {
      return this.baseCache.GetLastModificationDate(fileName);
    }

    public Version GetFileVersion(string fileName) => this.baseCache.GetFileVersion(fileName);

    public void CopyOut(string fileName, string targetFile)
    {
      this.baseCache.CopyOut(fileName, targetFile);
    }

    public void Dispose()
    {
      if (this.shadowCache != null)
      {
        this.shadowCache.Dispose();
        if (this.shadowCache == this.baseCache)
          this.baseCache = (IFileCache) null;
        this.shadowCache = (IFileCache) null;
      }
      if (this.baseCache == null)
        return;
      this.baseCache.Dispose();
      this.baseCache = (IFileCache) null;
    }

    private void ensureConsistent(string fileName)
    {
      if (this.baseCache == this.shadowCache)
        return;
      if (!this.baseCache.Exists(fileName))
        this.shadowCache.Delete(fileName);
      else if (!this.shadowCache.Exists(fileName))
      {
        this.baseCache.CopyOut(fileName, this.shadowCache.GetFilePath(fileName));
      }
      else
      {
        if (!(this.shadowCache.GetLastModificationDate(fileName) != this.baseCache.GetLastModificationDate(fileName)))
          return;
        this.baseCache.CopyOut(fileName, this.shadowCache.GetFilePath(fileName));
      }
    }
  }
}
