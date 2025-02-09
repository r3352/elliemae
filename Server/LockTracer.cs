// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LockTracer
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class LockTracer
  {
    private static Dictionary<string, LockTracer.LockTracerCollection> lockTracers = new Dictionary<string, LockTracer.LockTracerCollection>();
    private Stopwatch timer = new Stopwatch();
    private LockTracer.LockTracerCollection keyTracers;

    private LockTracer(
      string key,
      string className,
      LockType lockType,
      LockTracer parent,
      LockTracer.LockTracerCollection keyTracers)
    {
      this.Key = key;
      this.ClassName = className;
      this.ThreadId = Thread.CurrentThread.ManagedThreadId;
      this.LockType = lockType;
      this.Status = LockStatus.None;
      this.WaitTime = this.ProcessingTime = this.ReleaseTime = -1L;
      this.BlockedThreads = (int[]) null;
      this.keyTracers = keyTracers;
      this.NestLevel = parent == null ? 0 : parent.NestLevel + 1;
      this.Parent = parent;
      this.timer.Start();
    }

    public string Key { get; private set; }

    public string ClassName { get; private set; }

    public int ThreadId { get; private set; }

    public LockType LockType { get; private set; }

    public LockStatus Status { get; private set; }

    public long WaitTime { get; private set; }

    public long ProcessingTime { get; private set; }

    public long ReleaseTime { get; private set; }

    public int[] BlockedThreads { get; private set; }

    public int NestLevel { get; private set; }

    public LockTracer Parent { get; private set; }

    public void SetStatus(LockStatus status)
    {
      if (status == LockStatus.Acquired)
      {
        this.WaitTime = this.timer.ElapsedMilliseconds;
        this.ProcessingTime = -1L;
        this.ReleaseTime = -1L;
      }
      else if (status == LockStatus.Completed)
      {
        this.ProcessingTime = this.timer.ElapsedMilliseconds;
        this.ReleaseTime = -1L;
        LockTracer.getTracersForKey(this.ClassName, this.Key);
        lock (this.keyTracers)
          this.BlockedThreads = this.keyTracers.Values.Where<LockTracer>((Func<LockTracer, bool>) (info => info.Status == LockStatus.Waiting)).Select<LockTracer, int>((Func<LockTracer, int>) (info => info.ThreadId)).ToArray<int>();
      }
      else if (status == LockStatus.Released && this.WaitTime == -1L)
      {
        ILogger logger = DiagUtility.LogManager.GetLogger("LockDiagnostics");
        LockEventLog log = new LockEventLog();
        log.Level = Encompass.Diagnostics.Logging.LogLevel.WARN;
        log.Src = this.ClassName;
        log.Message = "Attempted phantom lock release on key '" + this.Key + "'";
        log.Set<LockStatus>(LockEventLog.Fields.Status, LockStatus.Unknown);
        log.Set<string>(LockEventLog.Fields.Key, this.Key);
        logger.Write<LockEventLog>(log);
      }
      else if (status == LockStatus.Released || status == LockStatus.ReleaseErrored || status == LockStatus.EvictionDetected)
      {
        this.ReleaseTime = this.timer.ElapsedMilliseconds;
        Encompass.Diagnostics.Logging.LogLevel level = this.BlockedThreads.Length != 0 || this.WaitTime >= 200L || this.ReleaseTime >= 100L || this.ProcessingTime >= 60000L ? Encompass.Diagnostics.Logging.LogLevel.WARN : Encompass.Diagnostics.Logging.LogLevel.DEBUG;
        ILogger logger = DiagUtility.LogManager.GetLogger("LockDiagnostics");
        logger.When(level, (Action) (() =>
        {
          LockEventLog log = new LockEventLog();
          log.Level = level;
          log.Src = this.ClassName;
          log.Message = "Exiting lock " + this.Key;
          log.Set<LockStatus>(LockEventLog.Fields.Status, status);
          log.Set<LockType>(LockEventLog.Fields.LockType, this.LockType);
          log.Set<string>(LockEventLog.Fields.Key, this.Key);
          log.Set<long>(LockEventLog.Fields.WaitTime, this.WaitTime);
          log.Set<long>(LockEventLog.Fields.ProcessingTime, this.ProcessingTime);
          log.Set<long>(LockEventLog.Fields.ReleaseTime, this.ReleaseTime);
          log.Set<long>(LockEventLog.Fields.TotalTime, this.WaitTime + this.ProcessingTime + this.ReleaseTime);
          if (((IEnumerable<int>) this.BlockedThreads).Any<int>())
            log.Set<int[]>(LockEventLog.Fields.ThreadsBlocked, this.BlockedThreads);
          log.Set<int>(LockEventLog.Fields.NestLevel, this.NestLevel);
          logger.Write<LockEventLog>(log);
        }));
      }
      else if (status == LockStatus.Failed)
      {
        ILogger logger = DiagUtility.LogManager.GetLogger("LockDiagnostics");
        logger.When(Encompass.Diagnostics.Logging.LogLevel.DEBUG, (Action) (() =>
        {
          LockEventLog log = new LockEventLog();
          log.Level = Encompass.Diagnostics.Logging.LogLevel.DEBUG;
          log.Src = this.ClassName;
          log.Message = string.Format("Failed {0} lock on key '{1}': WaitTime = {2}ms", (object) this.LockType, (object) this.Key, (object) this.WaitTime);
          log.Set<LockStatus>(LockEventLog.Fields.Status, status);
          log.Set<LockType>(LockEventLog.Fields.LockType, this.LockType);
          log.Set<string>(LockEventLog.Fields.Key, this.Key);
          log.Set<long>(LockEventLog.Fields.WaitTime, this.WaitTime);
          logger.Write<LockEventLog>(log);
        }));
      }
      this.Status = status;
      this.timer.Restart();
      if (this.Status != LockStatus.Released && this.Status != LockStatus.Failed && this.Status != LockStatus.ReleaseErrored && this.Status != LockStatus.EvictionDetected)
        return;
      lock (this.keyTracers)
      {
        if (this.NestLevel > 0)
          this.keyTracers[this.ThreadId] = this.Parent;
        else
          this.keyTracers.Remove(this.ThreadId);
      }
      lock (LockTracer.lockTracers)
      {
        --this.keyTracers.RefCount;
        if (this.keyTracers.RefCount == 0)
          LockTracer.lockTracers.Remove(this.keyTracers.TracerKey);
      }
      this.keyTracers = (LockTracer.LockTracerCollection) null;
    }

    private static string getTracerKey(string className, string key) => className + "\\" + key;

    private static LockTracer.LockTracerCollection getTracersForKey(
      string className,
      string key,
      bool createIfMissing = false)
    {
      string tracerKey = LockTracer.getTracerKey(className, key);
      LockTracer.LockTracerCollection tracersForKey = (LockTracer.LockTracerCollection) null;
      lock (LockTracer.lockTracers)
      {
        if (!LockTracer.lockTracers.TryGetValue(tracerKey, out tracersForKey) && createIfMissing)
        {
          tracersForKey = new LockTracer.LockTracerCollection()
          {
            TracerKey = tracerKey
          };
          LockTracer.lockTracers[tracerKey] = tracersForKey;
        }
        if (tracersForKey != null)
          ++tracersForKey.RefCount;
      }
      return tracersForKey;
    }

    public static LockTracer GetTracerForCurrentThread(string className, string key)
    {
      LockTracer.LockTracerCollection tracersForKey = LockTracer.getTracersForKey(className, key);
      LockTracer forCurrentThread = (LockTracer) null;
      lock (tracersForKey)
        tracersForKey.TryGetValue(Thread.CurrentThread.ManagedThreadId, out forCurrentThread);
      return forCurrentThread;
    }

    public static LockTracer Create(
      string className,
      string instanceName,
      string key,
      LockType lockType)
    {
      if (!DiagUtility.LogManager.GetLogger("LockDiagnostics").IsEnabled(Encompass.Diagnostics.Logging.LogLevel.WARN))
        return (LockTracer) null;
      return string.IsNullOrEmpty(instanceName) ? LockTracer.Create(className, "Default\\" + key, lockType) : LockTracer.Create(className, instanceName + "\\" + key, lockType);
    }

    public static LockTracer Create(string className, string key, LockType lockType)
    {
      if (!DiagUtility.LogManager.GetLogger("LockDiagnostics").IsEnabled(Encompass.Diagnostics.Logging.LogLevel.WARN))
        return (LockTracer) null;
      LockTracer.LockTracerCollection tracersForKey = LockTracer.getTracersForKey(className, key, true);
      LockTracer lockTracer = (LockTracer) null;
      lock (tracersForKey)
      {
        LockTracer parent = (LockTracer) null;
        lockTracer = tracersForKey.TryGetValue(Thread.CurrentThread.ManagedThreadId, out parent) ? new LockTracer(key, className, lockType, parent, tracersForKey) : new LockTracer(key, className, lockType, (LockTracer) null, tracersForKey);
        tracersForKey[lockTracer.ThreadId] = lockTracer;
      }
      lockTracer.Status = LockStatus.Waiting;
      return lockTracer;
    }

    public static void SetTracerStatus(
      string className,
      string instanceName,
      string key,
      LockStatus status)
    {
      if (!DiagUtility.LogManager.GetLogger("LockDiagnostics").IsEnabled(Encompass.Diagnostics.Logging.LogLevel.WARN))
        return;
      if (string.IsNullOrEmpty(instanceName))
        LockTracer.SetTracerStatus(className, "Default\\" + key, status);
      else
        LockTracer.SetTracerStatus(className, instanceName + "\\" + key, status);
    }

    public static void SetTracerStatus(string className, string key, LockStatus status)
    {
      if (!DiagUtility.LogManager.GetLogger("LockDiagnostics").IsEnabled(Encompass.Diagnostics.Logging.LogLevel.WARN))
        return;
      LockTracer.GetTracerForCurrentThread(className, key)?.SetStatus(status);
    }

    private class LockTracerCollection : Dictionary<int, LockTracer>
    {
      public string TracerKey;
      public int RefCount;
    }
  }
}
