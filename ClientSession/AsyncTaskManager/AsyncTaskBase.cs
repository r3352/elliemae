// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AsyncTaskManager.AsyncTaskBase
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using System;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.AsyncTaskManager
{
  public abstract class AsyncTaskBase
  {
    private Thread taskThread;
    private StatusState status;
    private Guid taskId = Guid.NewGuid();
    private bool requestCancel;
    private TimeSpan cancelWaitTime = TimeSpan.Zero;
    private int cancelCheckInterval = 5;
    private bool supportsProgress;
    protected int progress;

    public StatusState Status => this.status;

    public Guid TaskId => this.taskId;

    protected bool RequestCancel => this.requestCancel;

    protected TimeSpan CancelWaitTime
    {
      get => this.cancelWaitTime;
      set => this.cancelWaitTime = value;
    }

    protected int CancelCheckInterval
    {
      get => this.cancelCheckInterval;
      set => this.cancelCheckInterval = value;
    }

    protected bool SupportsProgress
    {
      get => this.supportsProgress;
      set => this.supportsProgress = value;
    }

    public int Progress
    {
      get
      {
        if (!this.supportsProgress)
          throw new InvalidOperationException("This task does not report progess.");
        return this.progress;
      }
    }

    public void Start()
    {
      this.status = StatusState.InProgress != this.status ? StatusState.InProgress : throw new InvalidOperationException("Already in progress.");
      this.taskThread = new Thread(new ThreadStart(this.StartAsyncTask));
      this.taskThread.Name = this.GetType().Name;
      this.taskThread.IsBackground = true;
      this.taskThread.Start();
    }

    private void StartAsyncTask()
    {
      this.OnStarted();
      this.DoTask();
      this.status = StatusState.Completed;
      this.OnCompleted();
    }

    protected abstract void DoTask();

    protected abstract void OnStarted();

    protected abstract void OnCompleted();

    public void StopTask()
    {
      if (this.status != StatusState.InProgress)
        return;
      if (this.cancelWaitTime != TimeSpan.Zero)
      {
        DateTime now = DateTime.Now;
        while (DateTime.Now.Subtract(now).TotalSeconds > 0.0)
          Thread.Sleep(TimeSpan.FromSeconds((double) this.cancelCheckInterval));
      }
      this.taskThread.Abort();
    }
  }
}
