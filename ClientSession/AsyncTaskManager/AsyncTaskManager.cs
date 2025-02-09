// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AsyncTaskManager.AsyncTaskManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AsyncTaskManager
{
  public class AsyncTaskManager : Component
  {
    private int maxThreads = 2;
    private EllieMae.EMLite.AsyncTaskManager.AsyncTaskManager.AsyncTaskList tasksInProgress = new EllieMae.EMLite.AsyncTaskManager.AsyncTaskManager.AsyncTaskList();
    private EllieMae.EMLite.AsyncTaskManager.AsyncTaskManager.AsyncTaskList tasksQueued = new EllieMae.EMLite.AsyncTaskManager.AsyncTaskManager.AsyncTaskList();
    private EllieMae.EMLite.AsyncTaskManager.AsyncTaskManager.AsyncTaskList tasksCompleted = new EllieMae.EMLite.AsyncTaskManager.AsyncTaskManager.AsyncTaskList();
    private Thread taskManagerThread;
    private bool working;
    private int sleepTime = 2;
    private Control invokeContext;

    public int MaxThreads
    {
      get => this.maxThreads;
      set => this.maxThreads = value;
    }

    public int SleepTime
    {
      get => this.sleepTime;
      set => this.sleepTime = value;
    }

    public AsyncTaskBase[] CompletedTasks
    {
      get
      {
        AsyncTaskBase[] completedTasks;
        lock (this.tasksCompleted)
        {
          completedTasks = new AsyncTaskBase[this.tasksCompleted.Count];
          for (int index = 0; index < this.tasksCompleted.Count; ++index)
            completedTasks[index] = this.tasksCompleted[index];
        }
        return completedTasks;
      }
    }

    public void EnqueueTask(AsyncTaskBase task)
    {
      lock (this.tasksQueued)
        this.tasksQueued.Add(task);
    }

    public void StartAllocatingWork(Control invokeContext)
    {
      if (this.working)
        return;
      this.invokeContext = invokeContext;
      this.taskManagerThread = new Thread(new ThreadStart(this.manageTasks));
      this.taskManagerThread.Name = this.GetType().Name;
      this.taskManagerThread.IsBackground = true;
      this.taskManagerThread.Priority = ThreadPriority.BelowNormal;
      this.working = true;
      this.taskManagerThread.Start();
    }

    public void StopAllTasks()
    {
      this.taskManagerThread.Abort();
      this.taskManagerThread.Join();
      foreach (AsyncTaskBase asyncTaskBase in (CollectionBase) this.tasksInProgress)
        asyncTaskBase.StopTask();
    }

    public event ReportTaskProgressEventHandler ReportTaskProgress;

    private void OnReportTaskProgress(object sender, ReportTaskProgressEventArgs e)
    {
      if (this.ReportTaskProgress == null)
        return;
      this.ReportTaskProgress(sender, e);
    }

    public event TaskCompletedEventHandler TaskCompleted;

    private void OnTaskCompleted(object sender, TaskCompletedEventArgs e)
    {
      if (this.TaskCompleted == null)
        return;
      this.TaskCompleted(sender, e);
    }

    private void manageTasks()
    {
      while (true)
      {
        for (int index = this.tasksInProgress.Count - 1; index >= 0; --index)
        {
          if (this.tasksInProgress[index].Status == StatusState.Completed)
          {
            AsyncTaskBase task = this.tasksInProgress[index];
            lock (this.tasksCompleted)
              this.tasksCompleted.Add(task);
            lock (this.tasksInProgress)
              this.tasksInProgress.Remove(task);
            this.invokeContext.Invoke((Delegate) new TaskCompletedEventHandler(this.OnTaskCompleted), (object) this, (object) new TaskCompletedEventArgs(task.TaskId));
          }
        }
        while (this.tasksQueued.Count > 0 && this.tasksInProgress.Count < this.maxThreads)
        {
          AsyncTaskBase task = this.tasksQueued[0];
          lock (this.tasksInProgress)
            this.tasksInProgress.Add(task);
          lock (this.tasksQueued)
            this.tasksQueued.RemoveAt(0);
          task.Start();
        }
        for (int index = this.tasksInProgress.Count - 1; index >= 0; --index)
        {
          AsyncTaskBase asyncTaskBase = this.tasksInProgress[index];
          if (asyncTaskBase.Status == StatusState.InProgress && this.invokeContext.Created)
            this.invokeContext.Invoke((Delegate) new ReportTaskProgressEventHandler(this.OnReportTaskProgress), (object) this, (object) new ReportTaskProgressEventArgs(asyncTaskBase.TaskId, asyncTaskBase.Progress));
        }
        Thread.Sleep(TimeSpan.FromSeconds((double) this.sleepTime));
      }
    }

    public class AsyncTaskList : CollectionBase
    {
      public AsyncTaskBase this[int index]
      {
        get => (AsyncTaskBase) this.List[index];
        set => this.List[index] = (object) value;
      }

      public int Add(AsyncTaskBase task) => this.List.Add((object) task);

      public void Remove(AsyncTaskBase task) => this.List.Remove((object) task);
    }
  }
}
