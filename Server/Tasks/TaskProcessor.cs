// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.TaskProcessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer.ServerTasks;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  internal class TaskProcessor
  {
    private const string className = "TaskProcessor�";
    private string processorID = Guid.NewGuid().ToString("D");
    private Thread thread;
    private AutoResetEvent contextChangeEvent = new AutoResetEvent(false);
    private ClientContext[] contexts;
    private ServerTaskProcessorStatus status;

    public TaskProcessor()
    {
      this.status = new ServerTaskProcessorStatus(this.processorID, TaskProcessorStatus.Starting);
      this.contexts = ClientContext.GetAll();
      this.thread = new Thread(new ThreadStart(this.execProcessingThread));
      this.thread.IsBackground = true;
      this.thread.Priority = ThreadPriority.Lowest;
      this.thread.Start();
    }

    public string ProcessorID => this.processorID;

    public ServerTaskProcessorStatus GetStatus()
    {
      lock (this)
        return this.status;
    }

    public void RefreshContextList()
    {
      lock (this)
        this.contexts = ClientContext.GetAll();
      this.contextChangeEvent.Set();
    }

    private void execProcessingThread()
    {
      try
      {
        while (true)
        {
          try
          {
            TaskQueue queue = (TaskQueue) null;
            while (queue == null)
              queue = this.waitForQueuedTask();
            ServerTask taskInfo;
            while ((taskInfo = queue.Dequeue()) != null)
              this.processTask(queue, taskInfo);
          }
          catch (ThreadAbortException ex)
          {
            TraceLog.WriteInfo(nameof (TaskProcessor), "TaskProcessor thread shutting down.");
            throw;
          }
          catch (Exception ex)
          {
            TraceLog.WriteError(nameof (TaskProcessor), "TaskProcessor thread encountered an unhandled exception: " + (object) ex);
          }
        }
      }
      finally
      {
        lock (this)
          this.status = new ServerTaskProcessorStatus(this.processorID, TaskProcessorStatus.Stopped);
      }
    }

    private void processTask(TaskQueue queue, ServerTask taskInfo)
    {
      try
      {
        lock (this)
          this.status = new ServerTaskProcessorStatus(this.processorID, queue.Context.InstanceName, taskInfo.TaskID, taskInfo.ScheduleID, taskInfo.TaskType);
        ITaskHandler taskHandler = TaskQueue.GetTaskHandler(taskInfo.TaskType);
        using (queue.Context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          DateTime now = DateTime.Now;
          string message = (string) null;
          ServerTaskOutcome outcome;
          try
          {
            taskHandler.ProcessTask(taskInfo);
            outcome = ServerTaskOutcome.Success;
          }
          catch (Exception ex)
          {
            outcome = ServerTaskOutcome.Failed;
            message = ex.Message;
          }
          TimeSpan timeSpan = DateTime.Now - now;
          queue.SetTaskOutcome(taskInfo, outcome, message);
          if (outcome == ServerTaskOutcome.Success)
            queue.Context.TraceLog.WriteInfo(nameof (TaskProcessor), "Successfully completed queued task " + (object) taskInfo.TaskType + "/" + (object) taskInfo.TaskID + " in " + timeSpan.TotalMilliseconds.ToString("#") + "ms");
          else
            queue.Context.TraceLog.WriteWarning(nameof (TaskProcessor), "Failed to complete queued task " + (object) taskInfo.TaskType + "/" + (object) taskInfo.TaskID + ". Time taken was " + timeSpan.TotalMilliseconds.ToString("#") + "ms");
        }
      }
      catch (Exception ex)
      {
        queue.Context.TraceLog.WriteWarning(nameof (TaskProcessor), "Failed to complete queued task " + (object) taskInfo.TaskType + "/" + (object) taskInfo.TaskID + " due to exception. Exception : " + ex.ToString());
      }
    }

    private TaskQueue waitForQueuedTask()
    {
      lock (this)
        this.status = new ServerTaskProcessorStatus(this.processorID, TaskProcessorStatus.Waiting);
      WaitHandle[] queueHandles = this.getQueueHandles();
      int index = WaitHandle.WaitAny(queueHandles);
      return queueHandles[index] == this.contextChangeEvent ? (TaskQueue) null : this.contexts[index].TaskQueue;
    }

    private WaitHandle[] getQueueHandles()
    {
      List<WaitHandle> waitHandleList = new List<WaitHandle>();
      lock (this.contexts)
      {
        for (int index = 0; index < this.contexts.Length; ++index)
          waitHandleList.Add(this.contexts[index].TaskQueue.WaitHandle);
      }
      waitHandleList.Add((WaitHandle) this.contextChangeEvent);
      return waitHandleList.ToArray();
    }
  }
}
