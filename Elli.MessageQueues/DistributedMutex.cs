// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.DistributedMutex
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

#nullable disable
namespace Elli.MessageQueues
{
  [ExcludeFromCodeCoverage]
  public class DistributedMutex : IDistributedMutexObject, IDisposable
  {
    private static Dictionary<string, string> _mutexIds = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private static Dictionary<string, bool> _initializationFlags = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private static Dictionary<string, DateTime> _pendingMutexes = new Dictionary<string, DateTime>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private static readonly TimeSpan MutexCleanupInterval = TimeSpan.FromSeconds(20.0);
    private FileStream _mutexStream;
    private string _mutexPath;

    static DistributedMutex()
    {
      new Thread(new ThreadStart(DistributedMutex.PurgeMutexFiles))
      {
        IsBackground = true,
        Priority = ThreadPriority.Lowest
      }.Start();
    }

    public DistributedMutex(string name, string path = null)
    {
      if (!string.IsNullOrEmpty(path))
      {
        this._mutexPath = Path.Combine(path, name);
      }
      else
      {
        this._mutexPath = DistributedMutex.GenerateMutexPath(DistributedMutex.GetMutexId(name));
        lock (DistributedMutex._initializationFlags)
        {
          if (DistributedMutex._initializationFlags.ContainsKey(name))
            return;
          DistributedMutex.InitializeQueue(name);
          DistributedMutex._initializationFlags[name] = true;
        }
      }
    }

    public bool WaitOne(TimeSpan timeout)
    {
      DateTime dateTime = DateTime.MaxValue;
      if (timeout != TimeSpan.MaxValue)
        dateTime = DateTime.Now.Add(timeout);
      int num = 0;
      do
      {
        if (num > 0)
          Thread.Sleep(250);
        this._mutexStream = this.AcquireFileLock();
        if (this._mutexStream != null)
          return true;
        ++num;
      }
      while (DateTime.Now < dateTime);
      return false;
    }

    private static string GenerateMutexPath(string hash)
    {
      return Path.Combine(DistributedMutex.GenerateMutexFolderPath(""), "~" + hash);
    }

    private static string GenerateMutexFolderPath(string subFolder)
    {
      return string.IsNullOrWhiteSpace(subFolder) ? Path.Combine(Global.DistributedMutexPolicy.GetMutexRootFolderPath(), "_ElliServiceDistributeMutex") : Path.Combine(Global.DistributedMutexPolicy.GetMutexRootFolderPath(), subFolder, "_ElliServiceDistributeMutex");
    }

    private FileStream AcquireFileLock()
    {
      try
      {
        return File.Open(this._mutexPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
      }
      catch
      {
        return (FileStream) null;
      }
    }

    private static string GetMutexId(string name)
    {
      lock (DistributedMutex._mutexIds)
      {
        if (DistributedMutex._mutexIds.ContainsKey(name))
          return DistributedMutex._mutexIds[name];
      }
      string mutexId = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(name.ToLower()))).ToUpper().Replace("/", "_");
      lock (DistributedMutex._mutexIds)
        DistributedMutex._mutexIds[name] = mutexId;
      return mutexId;
    }

    public void Dispose()
    {
      if (this._mutexStream == null)
        return;
      try
      {
        this._mutexStream.Close();
      }
      catch
      {
      }
      this._mutexStream = (FileStream) null;
      lock (DistributedMutex._pendingMutexes)
        DistributedMutex._pendingMutexes[this._mutexPath] = DateTime.Now;
      GC.SuppressFinalize((object) this);
    }

    private static void PurgeMutexFiles()
    {
label_0:
      try
      {
        Thread.Sleep(DistributedMutex.MutexCleanupInterval);
        Dictionary<string, DateTime> dictionary = (Dictionary<string, DateTime>) null;
        lock (DistributedMutex._pendingMutexes)
          dictionary = new Dictionary<string, DateTime>((IDictionary<string, DateTime>) DistributedMutex._pendingMutexes);
        using (Dictionary<string, DateTime>.KeyCollection.Enumerator enumerator = dictionary.Keys.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            string current = enumerator.Current;
            DateTime mutexTimestamp = dictionary[current];
            if (DateTime.Now - mutexTimestamp > DistributedMutex.MutexCleanupInterval)
              DistributedMutex.PurgeMutex(current, mutexTimestamp);
          }
          goto label_0;
        }
      }
      catch
      {
      }
    }

    private static void PurgeMutex(string mutexPath, DateTime mutexTimestamp)
    {
      bool flag = false;
      lock (DistributedMutex._pendingMutexes)
      {
        DateTime dateTime;
        if (DistributedMutex._pendingMutexes.TryGetValue(mutexPath, out dateTime))
        {
          if (mutexTimestamp == dateTime)
          {
            flag = true;
            DistributedMutex._pendingMutexes.Remove(mutexPath);
          }
        }
      }
      if (!flag)
        return;
      try
      {
        File.Delete(mutexPath);
      }
      catch
      {
      }
    }

    private static void InitializeQueue(string name)
    {
      string mutexFolderPath = DistributedMutex.GenerateMutexFolderPath("");
      Directory.CreateDirectory(mutexFolderPath);
      foreach (string file in Directory.GetFiles(mutexFolderPath))
      {
        try
        {
          File.Delete(file);
        }
        catch
        {
        }
      }
    }
  }
}
