// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Cache.ClientCache
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.Common;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Cache
{
  public class ClientCache : IFileCache, IDisposable
  {
    private Mutex cacheLock;
    private string cachePath;

    public ClientCache(string systemID, string cacheName, CacheScope scope)
      : this(systemID, cacheName, scope, -1)
    {
    }

    public ClientCache(string systemID, string cacheName, CacheScope scope, TimeSpan timeout)
      : this(systemID, cacheName, scope, (int) timeout.TotalMilliseconds)
    {
    }

    public ClientCache(string systemID, string cacheName, CacheScope scope, int timeout)
    {
      if (scope == CacheScope.System)
      {
        this.cachePath = Path.Combine(EnConfigurationSettings.GlobalSettings.AppSettingsDirectory, "Cache\\" + systemID + "\\" + cacheName);
      }
      else
      {
        this.cachePath = Path.Combine(SystemSettings.TempFolderRoot, "Cache\\" + cacheName);
        this.AcquireMutex(cacheName, timeout);
      }
    }

    public ClientCache(string systemID, string cacheName, int timeout)
    {
      this.cachePath = Path.Combine(EnConfigurationSettings.GlobalSettings.AppSettingsDirectory, "Cache\\" + systemID + "\\" + cacheName);
      this.AcquireMutex(cacheName, timeout);
    }

    private void AcquireMutex(string cacheName, int timeout)
    {
      this.cacheLock = !string.IsNullOrEmpty(this.cachePath) ? new Mutex(false, "cache://EM/" + this.cachePath.Replace("\\", "/")) : throw new InvalidOperationException("Cache path must be set");
      if (!this.cacheLock.WaitOne(timeout, false))
        throw new TimeoutException("Failed to acquire mutex lock on system cache '" + cacheName + "'");
    }

    public string GetFilePath(string fileName) => Path.Combine(this.cachePath, fileName);

    public bool Exists(string fileName) => File.Exists(this.GetFilePath(fileName));

    public BinaryObject Get(string fileName)
    {
      string filePath = this.GetFilePath(fileName);
      return File.Exists(filePath) ? new BinaryObject(filePath) : (BinaryObject) null;
    }

    public void Put(string fileName, BinaryObject data)
    {
      this.Put(fileName, data, DateTime.MinValue);
    }

    public void Put(string fileName, BinaryObject data, DateTime lastModificationTime)
    {
      string filePath = this.GetFilePath(fileName);
      Directory.CreateDirectory(Path.GetDirectoryName(filePath));
      data.Write(filePath);
      if (!(lastModificationTime != DateTime.MinValue))
        return;
      File.SetLastWriteTime(filePath, lastModificationTime);
    }

    public void Delete(string fileName)
    {
      string filePath = this.GetFilePath(fileName);
      if (!File.Exists(filePath))
        return;
      File.Delete(filePath);
    }

    public DateTime GetLastModificationDate(string fileName)
    {
      string filePath = this.GetFilePath(fileName);
      return File.Exists(filePath) ? File.GetLastWriteTime(filePath) : DateTime.MinValue;
    }

    public Version GetFileVersion(string fileName)
    {
      string filePath = this.GetFilePath(fileName);
      if (!File.Exists(filePath))
        return (Version) null;
      int[] numArray = !filePath.Contains("Plugins") || new FileInfo(filePath).Length != 0L ? ValidationUtil.GetVersionInfo(FileVersionInfo.GetVersionInfo(filePath).FileVersion) : throw new Exception("Found plugin of 0 size; treat as if the plugin is disabled.");
      return new Version(numArray[0], numArray[1], numArray[2], numArray[3]);
    }

    public void CopyOut(string fileName, string targetFile)
    {
      string filePath = this.GetFilePath(fileName);
      Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
      File.Copy(filePath, targetFile, true);
    }

    public void Dispose()
    {
      if (this.cacheLock == null)
        return;
      this.cacheLock.ReleaseMutex();
      this.cacheLock = (Mutex) null;
    }
  }
}
