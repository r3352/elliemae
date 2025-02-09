// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.AsyncTargetWrapperFactory
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public class AsyncTargetWrapperFactory : IAsyncTargetWrapperFactory, IDisposable
  {
    private readonly RestrictedQueue<AsyncTargetWrapperFactory.QueueItem> _logQueue;
    private readonly Thread _writerThread;
    private readonly IApplicationEventHandler _eventHandler;
    private readonly int _payloadSize;
    private readonly int _writeInterval;

    public AsyncTargetWrapperFactory(
      IApplicationEventHandler eventHandler,
      int payloadSize,
      int maxQueueSize,
      int writeInterval)
    {
      this._eventHandler = ArgumentChecks.IsNotNull<IApplicationEventHandler>(eventHandler, nameof (eventHandler));
      this._logQueue = new RestrictedQueue<AsyncTargetWrapperFactory.QueueItem>(maxQueueSize);
      this._payloadSize = payloadSize;
      this._writeInterval = writeInterval;
      using (ExecutionContext.SuppressFlow())
      {
        this._writerThread = new Thread(new ThreadStart(this.logWriterProc));
        this._writerThread.Priority = ThreadPriority.Lowest;
        this._writerThread.IsBackground = true;
        this._writerThread.Start();
      }
    }

    private void Enqueue(AsyncTargetWrapperFactory.QueueItem item)
    {
      lock (this._logQueue)
      {
        if (this._writerThread.IsAlive)
        {
          this._logQueue.Enqueue(item);
          if (this._logQueue.Count < this._payloadSize && item.Action != AsyncTargetWrapperFactory.QueueItemAction.Dispose)
            return;
          Monitor.Pulse((object) this._logQueue);
        }
        else
          item.DisposeTaskCompletion.SetResult(false);
      }
    }

    private void logWriterProc()
    {
      bool flag1 = false;
      while (!flag1)
      {
        bool flag2;
        lock (this._logQueue)
        {
          flag2 = this._writeInterval < 0 ? Monitor.Wait((object) this._logQueue) : Monitor.Wait((object) this._logQueue, this._writeInterval);
          if (!this._logQueue.Any<AsyncTargetWrapperFactory.QueueItem>())
            continue;
        }
        if (flag2)
          Thread.Sleep(500);
        Queue<AsyncTargetWrapperFactory.QueueItem> queueItemQueue = (Queue<AsyncTargetWrapperFactory.QueueItem>) null;
        lock (this._logQueue)
        {
          queueItemQueue = new Queue<AsyncTargetWrapperFactory.QueueItem>((IEnumerable<AsyncTargetWrapperFactory.QueueItem>) this._logQueue);
          this._logQueue.Clear();
        }
        HashSet<ILogTarget> logTargetSet = new HashSet<ILogTarget>();
        HashSet<AsyncTargetWrapperFactory.QueueItem> queueItemSet = new HashSet<AsyncTargetWrapperFactory.QueueItem>();
        while (queueItemQueue.Count > 0)
        {
          AsyncTargetWrapperFactory.QueueItem queueItem = queueItemQueue.Dequeue();
          switch (queueItem.Action)
          {
            case AsyncTargetWrapperFactory.QueueItemAction.Terminate:
              flag1 = true;
              break;
            case AsyncTargetWrapperFactory.QueueItemAction.Write:
              try
              {
                queueItem.Target.Write(queueItem.Log);
                break;
              }
              catch (Exception ex)
              {
                this._eventHandler.WriteApplicationEvent("Exception while writing log message: " + ex.GetFullStackTrace(), EventLogEntryType.Error, 1100);
                break;
              }
            case AsyncTargetWrapperFactory.QueueItemAction.Flush:
              logTargetSet.Add(queueItem.Target);
              break;
            case AsyncTargetWrapperFactory.QueueItemAction.Dispose:
              queueItemSet.Add(queueItem);
              break;
          }
        }
        foreach (ILogTarget logTarget in logTargetSet)
        {
          try
          {
            logTarget.Flush();
          }
          catch (Exception ex)
          {
            this._eventHandler.WriteApplicationEvent("Exception while flushing log target: " + ex.GetFullStackTrace(), EventLogEntryType.Error, 1100);
          }
        }
        foreach (AsyncTargetWrapperFactory.QueueItem queueItem in queueItemSet)
        {
          try
          {
            queueItem.Target.Dispose();
            queueItem.DisposeTaskCompletion.SetResult(true);
          }
          catch (Exception ex)
          {
            queueItem.DisposeTaskCompletion.SetException(ex);
            this._eventHandler.WriteApplicationEvent("Exception while disposing log target: " + ex.GetFullStackTrace(), EventLogEntryType.Error, 1103);
          }
        }
      }
    }

    public void Dispose()
    {
      lock (this._logQueue)
      {
        this._logQueue.Enqueue(AsyncTargetWrapperFactory.QueueItem.ForTerminate());
        Monitor.Pulse((object) this._logQueue);
      }
      this._writerThread.Join();
    }

    public ILogTarget WrapTarget(ILogTarget innerTarget)
    {
      return (ILogTarget) new AsyncTargetWrapperFactory.AsyncTargetWrapper(this, innerTarget);
    }

    private enum QueueItemAction
    {
      Terminate,
      Write,
      Flush,
      Dispose,
    }

    private class QueueItem
    {
      public Encompass.Diagnostics.Logging.Schema.Log Log { get; }

      public ILogTarget Target { get; }

      public AsyncTargetWrapperFactory.QueueItemAction Action { get; }

      public TaskCompletionSource<bool> DisposeTaskCompletion { get; }

      private QueueItem(
        AsyncTargetWrapperFactory.QueueItemAction action,
        ILogTarget target,
        Encompass.Diagnostics.Logging.Schema.Log log,
        TaskCompletionSource<bool> disposeTaskCompletion)
      {
        this.Log = log;
        this.Target = target;
        this.Action = action;
        this.DisposeTaskCompletion = disposeTaskCompletion;
      }

      public static AsyncTargetWrapperFactory.QueueItem ForWrite(ILogTarget target, Encompass.Diagnostics.Logging.Schema.Log log)
      {
        return new AsyncTargetWrapperFactory.QueueItem(AsyncTargetWrapperFactory.QueueItemAction.Write, target, log, (TaskCompletionSource<bool>) null);
      }

      public static AsyncTargetWrapperFactory.QueueItem ForFlush(ILogTarget target)
      {
        return new AsyncTargetWrapperFactory.QueueItem(AsyncTargetWrapperFactory.QueueItemAction.Flush, target, (Encompass.Diagnostics.Logging.Schema.Log) null, (TaskCompletionSource<bool>) null);
      }

      public static AsyncTargetWrapperFactory.QueueItem ForTerminate()
      {
        return new AsyncTargetWrapperFactory.QueueItem(AsyncTargetWrapperFactory.QueueItemAction.Terminate, (ILogTarget) null, (Encompass.Diagnostics.Logging.Schema.Log) null, (TaskCompletionSource<bool>) null);
      }

      public static AsyncTargetWrapperFactory.QueueItem ForDispose(
        ILogTarget target,
        TaskCompletionSource<bool> disposeTaskCompletion)
      {
        return new AsyncTargetWrapperFactory.QueueItem(AsyncTargetWrapperFactory.QueueItemAction.Dispose, target, (Encompass.Diagnostics.Logging.Schema.Log) null, disposeTaskCompletion);
      }

      public override int GetHashCode()
      {
        ILogTarget target = this.Target;
        return (target != null ? target.GetHashCode() : 0) * 17 + this.Action.GetHashCode();
      }
    }

    private class AsyncTargetWrapper : ILogTarget, IDisposable
    {
      private AsyncTargetWrapperFactory _parent;
      private readonly ILogTarget _innerLogTarget;

      public AsyncTargetWrapper(AsyncTargetWrapperFactory parent, ILogTarget innerLogTarget)
      {
        this._parent = parent;
        this._innerLogTarget = innerLogTarget;
      }

      public void Dispose()
      {
        AsyncTargetWrapperFactory parent = this._parent;
        this._parent = (AsyncTargetWrapperFactory) null;
        Thread.Sleep(500);
        TaskCompletionSource<bool> disposeTaskCompletion = new TaskCompletionSource<bool>();
        parent.Enqueue(AsyncTargetWrapperFactory.QueueItem.ForDispose(this._innerLogTarget, disposeTaskCompletion));
        disposeTaskCompletion.Task.Wait();
      }

      public void Flush()
      {
        (this._parent ?? throw new InvalidOperationException("Target already disposed")).Enqueue(AsyncTargetWrapperFactory.QueueItem.ForFlush(this._innerLogTarget));
      }

      public void Write(Encompass.Diagnostics.Logging.Schema.Log log)
      {
        (this._parent ?? throw new InvalidOperationException("Target already disposed")).Enqueue(AsyncTargetWrapperFactory.QueueItem.ForWrite(this._innerLogTarget, log));
      }
    }
  }
}
