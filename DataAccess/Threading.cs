// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.Threading
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public sealed class Threading
  {
    public static readonly TimeSpan DefaultTimeout = new TimeSpan(0, 0, 20);
    private const string className = "Threading";
    private static Hashtable currentLocks = new Hashtable();

    private Threading()
    {
    }

    public static void AquireReaderLock(ReaderWriterLock rwlock)
    {
      EllieMae.EMLite.DataAccess.Threading.AquireReaderLock(rwlock, EllieMae.EMLite.DataAccess.Threading.DefaultTimeout);
    }

    public static void AquireReaderLock(ReaderWriterLock rwlock, TimeSpan timeout)
    {
      try
      {
        rwlock.AcquireReaderLock(timeout);
      }
      catch (Exception ex)
      {
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteErrorI(nameof (Threading), "Error acquiring reader lock: " + ex.Message);
        throw;
      }
    }

    public static void AquireWriterLock(ReaderWriterLock rwlock)
    {
      EllieMae.EMLite.DataAccess.Threading.AquireWriterLock(rwlock, EllieMae.EMLite.DataAccess.Threading.DefaultTimeout);
    }

    public static void AquireWriterLock(ReaderWriterLock rwlock, TimeSpan timeout)
    {
      try
      {
        rwlock.AcquireWriterLock(timeout);
      }
      catch (Exception ex)
      {
        if (ServerGlobals.TraceLog != null && timeout > TimeSpan.Zero)
        {
          string message = "Error acquiring writer lock: " + ex.Message;
          if (timeout < TimeSpan.FromSeconds(1.0))
            ServerGlobals.TraceLog.WriteWarningI(nameof (Threading), message);
          else
            ServerGlobals.TraceLog.WriteErrorI(nameof (Threading), message);
        }
        throw;
      }
    }

    public static void ReleaseReaderLock(ReaderWriterLock rwlock) => rwlock.ReleaseReaderLock();

    public static void ReleaseWriterLock(ReaderWriterLock rwlock) => rwlock.ReleaseWriterLock();

    public static void AquireReaderLock(ReaderWriterLockSlim rwlockSlim)
    {
      EllieMae.EMLite.DataAccess.Threading.AquireReaderLock(rwlockSlim, EllieMae.EMLite.DataAccess.Threading.DefaultTimeout);
    }

    public static void AquireReaderLock(ReaderWriterLockSlim rwlockSlim, TimeSpan timeout)
    {
      if (!rwlockSlim.TryEnterReadLock(timeout))
        throw new ApplicationException("Timeout expires before the lock read request is granted.");
    }

    public static void AquireWriterLock(ReaderWriterLockSlim rwlockSlim)
    {
      EllieMae.EMLite.DataAccess.Threading.AquireWriterLock(rwlockSlim, EllieMae.EMLite.DataAccess.Threading.DefaultTimeout);
    }

    public static void AquireWriterLock(ReaderWriterLockSlim rwlockSlim, TimeSpan timeout)
    {
      if (!rwlockSlim.TryEnterWriteLock(timeout))
        throw new ApplicationException("Timeout expires before the write lock request is granted.");
    }

    public static void ReleaseReaderLock(ReaderWriterLockSlim rwlockSlim)
    {
      if (!rwlockSlim.IsReadLockHeld)
        return;
      rwlockSlim.ExitReadLock();
    }

    public static void ReleaseWriterLock(ReaderWriterLockSlim rwlockSlim)
    {
      if (!rwlockSlim.IsWriteLockHeld)
        return;
      rwlockSlim.ExitWriteLock();
    }

    public static void AcquireLock(string name, object lockObject, TimeSpan timeOut)
    {
      if (!Monitor.TryEnter(lockObject, timeOut))
      {
        ObjectLock objectLock = (ObjectLock) null;
        lock (EllieMae.EMLite.DataAccess.Threading.currentLocks)
          objectLock = (ObjectLock) EllieMae.EMLite.DataAccess.Threading.currentLocks[lockObject];
        if (objectLock != null)
        {
          string message = "Timeout while waiting to lock object '" + name + "' on thread " + (object) Thread.CurrentThread.GetHashCode() + ". " + (object) objectLock;
          if (ServerGlobals.CurrentContextTraceLog != null)
            ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Error, nameof (Threading), message);
          if (ServerGlobals.TraceLog != null)
            ServerGlobals.TraceLog.WriteErrorI(nameof (Threading), message);
        }
        else
        {
          string message = "Timeout while waiting to lock object '" + name + "' on thread " + (object) Thread.CurrentThread.GetHashCode() + ". No additional lock information available.";
          if (ServerGlobals.CurrentContextTraceLog != null)
            ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Error, nameof (Threading), message);
          if (ServerGlobals.TraceLog != null)
            ServerGlobals.TraceLog.WriteErrorI(nameof (Threading), message);
        }
        throw new Exception("Timeout while waiting to lock object '" + name + "' on thread " + (object) Thread.CurrentThread.GetHashCode());
      }
      try
      {
        lock (EllieMae.EMLite.DataAccess.Threading.currentLocks)
        {
          ObjectLock currentLock = (ObjectLock) EllieMae.EMLite.DataAccess.Threading.currentLocks[lockObject];
          if (currentLock == null || !currentLock.Thread.Equals((object) Thread.CurrentThread))
            EllieMae.EMLite.DataAccess.Threading.currentLocks[lockObject] = (object) new ObjectLock(name, lockObject);
          else
            ++currentLock.NestCount;
        }
      }
      catch
      {
      }
    }

    public static void ReleaseLock(object lockObject)
    {
      try
      {
        lock (EllieMae.EMLite.DataAccess.Threading.currentLocks)
        {
          ObjectLock currentLock = (ObjectLock) EllieMae.EMLite.DataAccess.Threading.currentLocks[lockObject];
          if (currentLock == null)
          {
            string message = "Attempt to release lock not held by current thread (" + (object) Thread.CurrentThread.GetHashCode() + ") at " + (object) new StackTrace();
            if (ServerGlobals.CurrentContextTraceLog != null)
              ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Error, nameof (Threading), message);
            if (ServerGlobals.TraceLog == null)
              return;
            ServerGlobals.TraceLog.WriteErrorI(nameof (Threading), message);
          }
          else if (!currentLock.Thread.Equals((object) Thread.CurrentThread))
          {
            string message = "Attempt to release lock not held by current thread (" + (object) Thread.CurrentThread.GetHashCode() + ") at " + (object) new StackTrace() + Environment.NewLine + (object) currentLock;
            if (ServerGlobals.CurrentContextTraceLog != null)
              ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Error, nameof (Threading), message);
            if (ServerGlobals.TraceLog == null)
              return;
            ServerGlobals.TraceLog.WriteErrorI(nameof (Threading), message);
          }
          else
          {
            --currentLock.NestCount;
            if (currentLock.NestCount != 0)
              return;
            EllieMae.EMLite.DataAccess.Threading.currentLocks.Remove(lockObject);
            if (!(DateTime.Now - currentLock.LockTime > EllieMae.EMLite.DataAccess.Threading.DefaultTimeout))
              return;
            string message = "Long duration lock detected. " + (object) currentLock;
            if (ServerGlobals.CurrentContextTraceLog != null)
              ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Warning, nameof (Threading), message);
            if (ServerGlobals.TraceLog == null)
              return;
            ServerGlobals.TraceLog.WriteWarningI(nameof (Threading), message);
          }
        }
      }
      finally
      {
        Monitor.Exit(lockObject);
      }
    }

    public static FileStream OpenFile(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share)
    {
      int num1 = 0;
      int num2 = 10;
      while (true)
      {
        try
        {
          return new FileStream(path, mode, access, share);
        }
        catch (IOException ex)
        {
          if (ex.Message.ToLower().IndexOf("is being used") < 0)
            throw;
          else if (++num1 > num2)
            throw;
          else
            Thread.Sleep(500);
        }
      }
    }

    public static ObjectLock GetCurrentLock(object lockObject)
    {
      lock (EllieMae.EMLite.DataAccess.Threading.currentLocks)
        return (ObjectLock) EllieMae.EMLite.DataAccess.Threading.currentLocks[lockObject];
    }
  }
}
