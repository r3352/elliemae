// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.BackgroundActionRunner
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class BackgroundActionRunner
  {
    private static readonly string ClassName = nameof (BackgroundActionRunner);
    private static readonly BackgroundActionRunner Instance = new BackgroundActionRunner();
    private readonly int _maxThreads;
    private readonly int _maxCountPerQueue;
    private readonly ILogger _logger;
    private int _currentThreadCount;
    private BackgroundActionRunner.BackgroundActionRunnerThread _currentThread;

    public BackgroundActionRunner(int maxThreads, int maxCountPerQueue, ILogger globalTraceLog)
    {
      this._maxThreads = maxThreads;
      this._maxCountPerQueue = maxCountPerQueue;
      this._currentThreadCount = 1;
      this._logger = globalTraceLog;
      this._currentThread = new BackgroundActionRunner.BackgroundActionRunnerThread(this);
    }

    public BackgroundActionRunner()
      : this(25, 1000, DiagUtility.GlobalLogger)
    {
    }

    public void Enqueue(
      string name,
      IClientContext context,
      bool throttleByName,
      Action<IClientContext> action)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException(nameof (name));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      do
      {
        bool flag = false;
        BackgroundActionRunner.BackgroundActionRunnerThread currentThread = this._currentThread;
        if (currentThread.CountInQueue >= this._maxCountPerQueue)
        {
          lock (this)
          {
            if (currentThread == this._currentThread)
            {
              if (this._currentThreadCount < this._maxThreads)
              {
                int num = ++this._currentThreadCount;
                this._currentThread.Terminate();
                this._currentThread = new BackgroundActionRunner.BackgroundActionRunnerThread(this);
                this._logger.Write(Encompass.Diagnostics.Logging.LogLevel.WARN, BackgroundActionRunner.ClassName, string.Format("Initializing new BackgroundActionRunner Thread. Updated Thread Count: ", (object) num));
              }
              else
                flag = true;
            }
          }
        }
        if (flag)
        {
          using (context.MakeCurrent())
          {
            action(context);
            break;
          }
        }
      }
      while (this._currentThread.Enqueue(name, context, throttleByName, action) == null);
    }

    public int GetMaxThreads() => this._maxThreads;

    public int GetMaxCountPerQueue() => this._maxCountPerQueue;

    public int GetThreadCount() => this._currentThreadCount;

    public int GetThreadQueueCount() => this._currentThread.CountInQueue;

    public static void EnqueueAction(
      string name,
      IClientContext context,
      bool throttleByName,
      Action<IClientContext> action)
    {
      BackgroundActionRunner.Instance.Enqueue(name, context, throttleByName, action);
    }

    public static int GetCurrentThreadCount() => BackgroundActionRunner.Instance.GetThreadCount();

    public static int GetCurrentThreadQueueCount()
    {
      return BackgroundActionRunner.Instance.GetThreadQueueCount();
    }

    private class AsyncAction
    {
      public string Name;
      public IClientContext Context;
      public IDataCache DataCache;
      public Action<IClientContext> Act;
      public string CorrelationId;
    }

    private class BackgroundActionRunnerThread
    {
      private readonly Queue<BackgroundActionRunner.AsyncAction> _actionQueue = new Queue<BackgroundActionRunner.AsyncAction>();
      private readonly ConcurrentDictionary<string, BackgroundActionRunner.AsyncAction> _actionDictionary = new ConcurrentDictionary<string, BackgroundActionRunner.AsyncAction>();
      private Thread _runnerThread;
      private readonly BackgroundActionRunner _parent;
      private bool keepGoing;

      public BackgroundActionRunnerThread(BackgroundActionRunner parent)
      {
        using (ExecutionContext.SuppressFlow())
        {
          this._runnerThread = new Thread(new ThreadStart(this.Runner));
          this._runnerThread.Priority = ThreadPriority.Lowest;
          this._runnerThread.IsBackground = true;
          this._runnerThread.Start();
        }
        this._parent = parent;
        this.keepGoing = true;
      }

      public int CountInQueue
      {
        get
        {
          lock (this._actionQueue)
            return this._actionQueue.Count;
        }
      }

      private BackgroundActionRunner.AsyncAction Enqueue(
        string name,
        IClientContext context,
        Action<IClientContext> action)
      {
        lock (this._actionQueue)
        {
          if (!this.keepGoing)
            return (BackgroundActionRunner.AsyncAction) null;
          string str = (string) null;
          if (ClientContext.CurrentRequest != null && !string.IsNullOrEmpty(ClientContext.CurrentRequest.CorrelationId))
            str = ClientContext.CurrentRequest.CorrelationId;
          IDataCache requestCache = ClientContext.CurrentRequest?.RequestCache;
          BackgroundActionRunner.AsyncAction asyncAction = new BackgroundActionRunner.AsyncAction()
          {
            Name = name,
            Context = context,
            DataCache = requestCache,
            Act = action,
            CorrelationId = str
          };
          this._actionQueue.Enqueue(asyncAction);
          Monitor.Pulse((object) this._actionQueue);
          return asyncAction;
        }
      }

      public BackgroundActionRunner.AsyncAction Enqueue(
        string name,
        IClientContext context,
        bool throttleByName,
        Action<IClientContext> action)
      {
        return throttleByName ? this._actionDictionary.GetOrAdd(context.InstanceName + "-" + name, (Func<string, BackgroundActionRunner.AsyncAction>) (k => this.Enqueue(name, context, action))) : this.Enqueue(name, context, action);
      }

      public void Terminate()
      {
        lock (this._actionQueue)
          this.keepGoing = false;
      }

      private void Runner()
      {
        try
        {
          while (this.keepGoing || this.CountInQueue > 0)
          {
            try
            {
              bool flag = true;
              lock (this._actionQueue)
              {
                while (this._actionQueue.Count == 0)
                  Monitor.Wait((object) this._actionQueue);
                if (this._actionQueue.Count >= 100)
                  flag = false;
              }
              if (flag)
                Thread.Sleep(100);
              Queue<BackgroundActionRunner.AsyncAction> asyncActionQueue = (Queue<BackgroundActionRunner.AsyncAction>) null;
              lock (this._actionQueue)
              {
                asyncActionQueue = new Queue<BackgroundActionRunner.AsyncAction>((IEnumerable<BackgroundActionRunner.AsyncAction>) this._actionQueue);
                this._actionQueue.Clear();
              }
              while (asyncActionQueue.Count > 0)
              {
                BackgroundActionRunner.AsyncAction asyncAction = asyncActionQueue.Dequeue();
                Stopwatch stopwatch = Stopwatch.StartNew();
                try
                {
                  using (asyncAction.Context.MakeCurrent(asyncAction.DataCache, asyncAction.CorrelationId))
                  {
                    try
                    {
                      asyncAction.Act(asyncAction.Context);
                    }
                    catch (Exception ex)
                    {
                      asyncAction.Context.TraceLog.WriteError(BackgroundActionRunner.ClassName, string.Format("Error while performing async action [{0}]. Exception: {1}", (object) asyncAction.Name, (object) ex.GetFullStackTrace()));
                    }
                  }
                }
                finally
                {
                  this._actionDictionary.TryRemove(asyncAction.Context.InstanceName + "-" + asyncAction.Name, out BackgroundActionRunner.AsyncAction _);
                }
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds > 600L)
                  asyncAction.Context.TraceLog.WriteWarning(BackgroundActionRunner.ClassName, string.Format("Long running background action detected. Action name: {0}. Time taken: {1}ms", (object) asyncAction.Name, (object) stopwatch.ElapsedMilliseconds));
              }
            }
            catch (Exception ex)
            {
              if (ex is ThreadAbortException)
                throw;
              else
                this._parent._logger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, BackgroundActionRunner.ClassName, "Error while running async actions.", ex);
            }
          }
        }
        finally
        {
          lock (this._parent)
          {
            int num = --this._parent._currentThreadCount;
            this._parent._logger.Write(Encompass.Diagnostics.Logging.LogLevel.WARN, BackgroundActionRunner.ClassName, string.Format("Terminating BackgroundActionRunner Thread. Updated Thread Count: ", (object) num));
          }
          this._runnerThread = (Thread) null;
        }
      }
    }
  }
}
