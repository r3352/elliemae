// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SafeMutex
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class SafeMutex : IDisposable
  {
    private const string className = "SafeMutex�";
    public static readonly TimeSpan MaximumTimeout = TimeSpan.FromSeconds(30.0);
    private string name;
    private MutexAccess access;
    private IClientContext context;
    private SafeMutex.IMutexObject mutexObject;

    public SafeMutex(IClientContext context, string name)
      : this(context, name, MutexAccess.Write)
    {
    }

    public SafeMutex(IClientContext context, string name, MutexAccess access)
    {
      this.name = name;
      this.access = access;
      this.context = context;
      if (context.Cache.CacheStoreSource == CacheStoreSource.HazelCast)
        this.mutexObject = (SafeMutex.IMutexObject) new SafeMutex.HazelCastMutex(context, this.name, access);
      else if (context.Settings.MutexSetting == ServerMutexSetting.MultiServer)
        this.mutexObject = (SafeMutex.IMutexObject) new SafeMutex.CrossProcessMutex(context, this.name, access);
      else
        this.mutexObject = (SafeMutex.IMutexObject) new SafeMutex.SingleProcessMutex(context, this.name, access);
    }

    public SafeMutex(IClientContext context, string name, MutexAccess access, TimeSpan timeout)
      : this(context, name, access)
    {
      if (!this.WaitOne(timeout) && timeout > TimeSpan.Zero)
        throw new Exception("Timeout while waiting for " + (object) access + " lock on resource '" + name + "'");
    }

    public void Dispose()
    {
      this.ReleaseMutex();
      GC.SuppressFinalize((object) this);
    }

    ~SafeMutex() => this.Dispose();

    public void ReleaseMutex()
    {
      if (this.mutexObject == null)
        return;
      LockTracer.SetTracerStatus(nameof (SafeMutex), this.context.InstanceName, this.name, LockStatus.Completed);
      this.mutexObject.Dispose();
      this.mutexObject = (SafeMutex.IMutexObject) null;
      LockTracer.SetTracerStatus(nameof (SafeMutex), this.context.InstanceName, this.name, LockStatus.Released);
    }

    public bool WaitOne() => this.WaitOne(TimeSpan.MaxValue);

    public bool WaitOne(TimeSpan timeout)
    {
      try
      {
        LockTracer.Create(nameof (SafeMutex), this.context.InstanceName, this.name, this.access == MutexAccess.Read ? LockType.ReadOnly : LockType.ReaderWriter);
        if (timeout > SafeMutex.MaximumTimeout)
          timeout = SafeMutex.MaximumTimeout;
        bool flag = this.mutexObject.WaitOne(timeout);
        if (!flag && timeout > TimeSpan.Zero)
          TraceLog.WriteWarning(nameof (SafeMutex), "Timeout while acquiring mutex on '" + this.name + "'.");
        if (flag)
          LockTracer.SetTracerStatus(nameof (SafeMutex), this.context.InstanceName, this.name, LockStatus.Acquired);
        else
          LockTracer.SetTracerStatus(nameof (SafeMutex), this.context.InstanceName, this.name, LockStatus.Failed);
        return flag;
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (SafeMutex), "Error acquiring mutex on '" + this.name + "': " + (object) ex);
        LockTracer.SetTracerStatus(nameof (SafeMutex), this.context.InstanceName, this.name, LockStatus.Failed);
        return false;
      }
    }

    public bool WaitOne(int millisecondsTimeout)
    {
      return this.WaitOne(TimeSpan.FromMilliseconds((double) millisecondsTimeout));
    }

    private interface IMutexObject : IDisposable
    {
      bool WaitOne(TimeSpan timeout);
    }

    private class CrossProcessMutex : SafeMutex.IMutexObject, IDisposable
    {
      private static Dictionary<string, string> mutexIds = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      private static Dictionary<string, bool> contextInitializationFlags = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      private static Dictionary<string, DateTime> pendingMutexes = new Dictionary<string, DateTime>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      private static readonly TimeSpan MutexCleanupInterval = TimeSpan.FromSeconds(20.0);
      private MutexAccess access;
      private FileStream mutexStream;
      private SafeMutex.SingleProcessMutex inProcessMutex;
      private string mutexPath;

      static CrossProcessMutex()
      {
        new Thread(new ThreadStart(SafeMutex.CrossProcessMutex.purgeMutexFiles))
        {
          IsBackground = true,
          Priority = ThreadPriority.Lowest
        }.Start();
      }

      public CrossProcessMutex(IClientContext context, string name, MutexAccess access)
      {
        this.access = access;
        this.mutexPath = SafeMutex.CrossProcessMutex.generateMutexPath(context, SafeMutex.CrossProcessMutex.getMutexID(name));
        this.inProcessMutex = new SafeMutex.SingleProcessMutex(context, name, access);
        lock (SafeMutex.CrossProcessMutex.contextInitializationFlags)
        {
          if (SafeMutex.CrossProcessMutex.contextInitializationFlags.ContainsKey(context.InstanceName))
            return;
          SafeMutex.CrossProcessMutex.initializeContext(context);
          SafeMutex.CrossProcessMutex.contextInitializationFlags[context.InstanceName] = true;
        }
      }

      public bool WaitOne(TimeSpan timeout)
      {
        DateTime dateTime = DateTime.MaxValue;
        if (timeout != TimeSpan.MaxValue)
          dateTime = DateTime.Now.Add(timeout);
        if (!this.inProcessMutex.WaitOne(timeout))
          return false;
        int num = 0;
        do
        {
          if (num > 0)
            Thread.Sleep(250);
          this.mutexStream = this.acquireFileLock();
          if (this.mutexStream != null)
            return true;
          ++num;
        }
        while (DateTime.Now < dateTime);
        this.inProcessMutex.Dispose();
        this.inProcessMutex = (SafeMutex.SingleProcessMutex) null;
        return false;
      }

      private static string generateMutexPath(IClientContext context, string hash)
      {
        return Path.Combine(SafeMutex.CrossProcessMutex.generateMutexFolderPath(context), "~" + hash);
      }

      private static string generateMutexFolderPath(IClientContext context)
      {
        return context.Settings.GetDataFolderPath("Settings\\~Mutex");
      }

      private FileStream acquireFileLock()
      {
        try
        {
          return this.access == MutexAccess.Write ? File.Open(this.mutexPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None) : File.Open(this.mutexPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        }
        catch
        {
          return (FileStream) null;
        }
      }

      private static string getMutexID(string name)
      {
        lock (SafeMutex.CrossProcessMutex.mutexIds)
        {
          if (SafeMutex.CrossProcessMutex.mutexIds.ContainsKey(name))
            return SafeMutex.CrossProcessMutex.mutexIds[name];
        }
        string mutexId = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(name.ToLower()))).ToUpper().Replace("/", "_");
        lock (SafeMutex.CrossProcessMutex.mutexIds)
          SafeMutex.CrossProcessMutex.mutexIds[name] = mutexId;
        return mutexId;
      }

      public void Dispose()
      {
        if (this.mutexStream != null)
        {
          try
          {
            this.mutexStream.Close();
          }
          catch
          {
          }
          this.mutexStream = (FileStream) null;
          lock (SafeMutex.CrossProcessMutex.pendingMutexes)
            SafeMutex.CrossProcessMutex.pendingMutexes[this.mutexPath] = DateTime.Now;
        }
        if (this.inProcessMutex == null)
          return;
        this.inProcessMutex.Dispose();
        this.inProcessMutex = (SafeMutex.SingleProcessMutex) null;
      }

      private static void purgeMutexFiles()
      {
label_0:
        try
        {
          Thread.Sleep(SafeMutex.CrossProcessMutex.MutexCleanupInterval);
          Dictionary<string, DateTime> dictionary = (Dictionary<string, DateTime>) null;
          lock (SafeMutex.CrossProcessMutex.pendingMutexes)
            dictionary = new Dictionary<string, DateTime>((IDictionary<string, DateTime>) SafeMutex.CrossProcessMutex.pendingMutexes);
          using (Dictionary<string, DateTime>.KeyCollection.Enumerator enumerator = dictionary.Keys.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              DateTime mutexTimestamp = dictionary[current];
              if (DateTime.Now - mutexTimestamp > SafeMutex.CrossProcessMutex.MutexCleanupInterval)
                SafeMutex.CrossProcessMutex.purgeMutex(current, mutexTimestamp);
            }
            goto label_0;
          }
        }
        catch
        {
        }
      }

      private static void purgeMutex(string mutexPath, DateTime mutexTimestamp)
      {
        bool flag = false;
        lock (SafeMutex.CrossProcessMutex.pendingMutexes)
        {
          DateTime dateTime;
          if (SafeMutex.CrossProcessMutex.pendingMutexes.TryGetValue(mutexPath, out dateTime))
          {
            if (mutexTimestamp == dateTime)
            {
              flag = true;
              SafeMutex.CrossProcessMutex.pendingMutexes.Remove(mutexPath);
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

      private static void initializeContext(IClientContext context)
      {
        string mutexFolderPath = SafeMutex.CrossProcessMutex.generateMutexFolderPath(context);
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

    private class SingleProcessMutex : SafeMutex.IMutexObject, IDisposable
    {
      private string name;
      private MutexAccess access;
      private IClientContext context;
      private IDisposable mutexObject;

      public SingleProcessMutex(IClientContext context, string name, MutexAccess access)
      {
        this.name = name;
        this.access = access;
        this.context = context;
      }

      public bool WaitOne(TimeSpan timeout)
      {
        try
        {
          this.mutexObject = this.access != MutexAccess.Read ? this.context.Cache.Lock("Mutex_" + this.name, timeout: (int) timeout.TotalMilliseconds) : this.context.Cache.Lock("Mutex_" + this.name, LockType.ReadOnly, (int) timeout.TotalMilliseconds);
          return true;
        }
        catch
        {
          return false;
        }
      }

      public void Dispose()
      {
        if (this.mutexObject == null)
          return;
        this.mutexObject.Dispose();
        this.mutexObject = (IDisposable) null;
      }
    }

    private class HazelCastMutex : SafeMutex.IMutexObject, IDisposable
    {
      private static readonly Dictionary<string, SafeMutex.HazelCastMutex.ReaderWriterTracker> trackerMap = new Dictionary<string, SafeMutex.HazelCastMutex.ReaderWriterTracker>();
      private static TimeSpan lockRefreshInterval = TimeSpan.FromMinutes(1.0);
      private static TimeSpan lockExpirationInterval = TimeSpan.FromMinutes(2.0);
      private string name;
      private MutexAccess access;
      private IClientContext context;
      private bool locked;
      private IDisposable lockObject;
      private string identifier;
      private string trackerKey;
      private SafeMutex.HazelCastMutex.ReaderWriterTracker tracker;

      public HazelCastMutex(IClientContext context, string name, MutexAccess access)
      {
        this.name = "mutex_" + name;
        this.access = access;
        this.context = context;
        this.identifier = Guid.NewGuid().ToString();
        this.trackerKey = "SafeMutex\\->" + context.InstanceName + "\\->" + name;
        lock (SafeMutex.HazelCastMutex.trackerMap)
        {
          if (!SafeMutex.HazelCastMutex.trackerMap.TryGetValue(this.trackerKey, out this.tracker))
          {
            this.tracker = new SafeMutex.HazelCastMutex.ReaderWriterTracker();
            SafeMutex.HazelCastMutex.trackerMap.Add(this.trackerKey, this.tracker);
          }
          ++this.tracker.RefCount;
        }
      }

      private void writeHazelcastReaderLock(TimeSpan timeout)
      {
        SafeMutex.HazelCastMutex.HazelCastReaderLock hazelCastReaderLock = new SafeMutex.HazelCastMutex.HazelCastReaderLock(EncompassServer.ServerID);
        hazelCastReaderLock.ValidUntil = DateTime.Now + SafeMutex.HazelCastMutex.lockExpirationInterval;
        using (this.context.Cache.Lock(this.name, timeout: (int) timeout.TotalMilliseconds))
        {
          List<SafeMutex.HazelCastMutex.HazelCastReaderLock> o = this.context.Cache.Get<List<SafeMutex.HazelCastMutex.HazelCastReaderLock>>(this.name) ?? new List<SafeMutex.HazelCastMutex.HazelCastReaderLock>();
          o.Add(hazelCastReaderLock);
          this.context.Cache.Put(this.name, (object) o);
        }
      }

      private TimeSpan getRemainingTime(TimeSpan timeout, long elapsedMs)
      {
        if (timeout > SafeMutex.MaximumTimeout)
          return SafeMutex.MaximumTimeout;
        long num = (long) timeout.TotalMilliseconds - elapsedMs;
        return num <= 0L ? TimeSpan.Zero : TimeSpan.FromMilliseconds((double) num);
      }

      private bool acquireReaderLock(TimeSpan timeout)
      {
        Stopwatch stopwatch = Stopwatch.StartNew();
        if (!Monitor.TryEnter((object) this.tracker, timeout))
          return false;
        try
        {
          if (this.tracker.ReaderCount == 0 || DateTime.Now > this.tracker.ReaderLockRenewalTime)
          {
            TimeSpan remainingTime = this.getRemainingTime(timeout, stopwatch.ElapsedMilliseconds);
            if (remainingTime == TimeSpan.Zero)
              return false;
            this.writeHazelcastReaderLock(remainingTime);
            this.tracker.ReaderLockRenewalTime = DateTime.Now + SafeMutex.HazelCastMutex.lockRefreshInterval;
          }
          ++this.tracker.ReaderCount;
          this.locked = true;
          return true;
        }
        catch (TimeoutException ex)
        {
          return false;
        }
        finally
        {
          Monitor.Exit((object) this.tracker);
        }
      }

      private void releaseReaderLock()
      {
        lock (this.tracker)
        {
          if (this.tracker.ReaderCount > 0)
            --this.tracker.ReaderCount;
          this.locked = false;
          if (this.tracker.ReaderCount != 0)
            return;
          try
          {
            using (this.context.Cache.Lock(this.name))
            {
              List<SafeMutex.HazelCastMutex.HazelCastReaderLock> hazelCastReaderLockList = this.context.Cache.Get<List<SafeMutex.HazelCastMutex.HazelCastReaderLock>>(this.name);
              bool flag = SafeMutex.HazelCastMutex.purgeExpiredReaderLocks(hazelCastReaderLockList);
              int index = hazelCastReaderLockList.FindIndex((Predicate<SafeMutex.HazelCastMutex.HazelCastReaderLock>) (p => p.ServerID == EncompassServer.ServerID));
              if (index >= 0)
              {
                hazelCastReaderLockList.RemoveAt(index);
                flag = true;
              }
              if (!flag)
                return;
              this.context.Cache.Put(this.name, (object) hazelCastReaderLockList);
            }
          }
          catch (Exception ex)
          {
            TraceLog.WriteError(nameof (SafeMutex), "Failed to properly release reader lock on " + this.name + ": " + (object) ex);
          }
        }
      }

      private static bool purgeExpiredReaderLocks(
        List<SafeMutex.HazelCastMutex.HazelCastReaderLock> counters)
      {
        if (counters == null)
          return false;
        bool flag = false;
        for (int index = counters.Count - 1; index >= 0; --index)
        {
          if (DateTime.Now > counters[index].ValidUntil)
          {
            counters.RemoveAt(index);
            flag = true;
          }
        }
        return flag;
      }

      private bool acquireWriterLock(TimeSpan timeout)
      {
        Stopwatch stopwatch = Stopwatch.StartNew();
        do
        {
          if (Monitor.TryEnter((object) this.tracker, this.getRemainingTime(timeout, stopwatch.ElapsedMilliseconds)))
          {
            try
            {
              if (this.tracker.ReaderCount == 0)
              {
                try
                {
                  this.lockObject = this.context.Cache.Lock(this.name, timeout: (int) this.getRemainingTime(timeout, stopwatch.ElapsedMilliseconds).TotalMilliseconds);
                }
                catch (TimeoutException ex)
                {
                  return false;
                }
                List<SafeMutex.HazelCastMutex.HazelCastReaderLock> hazelCastReaderLockList = this.context.Cache.Get<List<SafeMutex.HazelCastMutex.HazelCastReaderLock>>(this.name);
                if (SafeMutex.HazelCastMutex.purgeExpiredReaderLocks(hazelCastReaderLockList))
                  this.context.Cache.Put(this.name, (object) hazelCastReaderLockList);
                if (hazelCastReaderLockList != null)
                {
                  if (hazelCastReaderLockList.Count != 0)
                    goto label_15;
                }
                ++this.tracker.WriterCount;
                this.locked = true;
                return true;
              }
            }
            finally
            {
              if (!this.locked)
              {
                if (this.lockObject != null)
                {
                  this.lockObject.Dispose();
                  this.lockObject = (IDisposable) null;
                }
                Monitor.Exit((object) this.tracker);
              }
            }
label_15:
            Thread.Sleep(200);
          }
        }
        while ((double) stopwatch.ElapsedMilliseconds < timeout.TotalMilliseconds);
        return false;
      }

      private void releaseWriterLock()
      {
        if (this.tracker.WriterCount > 0)
          --this.tracker.WriterCount;
        try
        {
          this.lockObject.Dispose();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (SafeMutex), "Failed to release writer lock on key '" + this.name + "': " + (object) ex);
        }
        Monitor.Exit((object) this.tracker);
        this.locked = false;
      }

      public bool WaitOne(TimeSpan timeout)
      {
        if (this.locked)
          return true;
        return this.access == MutexAccess.Read ? this.acquireReaderLock(timeout) : this.acquireWriterLock(timeout);
      }

      public void Dispose()
      {
        if (!this.locked)
          return;
        try
        {
          if (this.access == MutexAccess.Read)
            this.releaseReaderLock();
          else
            this.releaseWriterLock();
        }
        finally
        {
          lock (SafeMutex.HazelCastMutex.trackerMap)
          {
            --this.tracker.RefCount;
            if (this.tracker.RefCount == 0)
              SafeMutex.HazelCastMutex.trackerMap.Remove(this.trackerKey);
          }
          this.tracker = (SafeMutex.HazelCastMutex.ReaderWriterTracker) null;
        }
      }

      internal class ReaderWriterTracker
      {
        public int RefCount;
        public int ReaderCount;
        public int WriterCount;
        public DateTime ReaderLockRenewalTime = DateTime.MinValue;
      }

      [Serializable]
      internal class HazelCastReaderLock
      {
        public readonly string ServerID;

        public DateTime ValidUntil { get; set; }

        public HazelCastReaderLock(string serverId) => this.ServerID = EncompassServer.ServerID;
      }
    }
  }
}
