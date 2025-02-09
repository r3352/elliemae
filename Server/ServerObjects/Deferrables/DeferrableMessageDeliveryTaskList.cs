// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.DeferrableMessageDeliveryTaskList
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables
{
  public sealed class DeferrableMessageDeliveryTaskList
  {
    private readonly Dictionary<Type, IDeferrableMessageTaskHandler> _taskHandlers = new Dictionary<Type, IDeferrableMessageTaskHandler>();
    private readonly List<DeferrableMessageDeliveryTask> _tasks = new List<DeferrableMessageDeliveryTask>();
    private object _syncLock = new object();

    public bool HasMessages => this._tasks.Count > 0;

    public static DeferrableMessageDeliveryTaskList GetInstance(string taskListInstanceName = null)
    {
      IRequestContext currentRequest = ClientContext.CurrentRequest;
      if (currentRequest == null)
        throw new InvalidOperationException("There is no current request context established");
      string key = string.IsNullOrWhiteSpace(taskListInstanceName) ? typeof (DeferrableMessageDeliveryTaskList).ToString() : string.Format("{0}-{1}", (object) taskListInstanceName, (object) typeof (DeferrableMessageDeliveryTaskList).ToString());
      DeferrableMessageDeliveryTaskList instance1 = currentRequest.RequestCache.Get<DeferrableMessageDeliveryTaskList>(key);
      if (instance1 != null)
        return instance1;
      DeferrableMessageDeliveryTaskList instance2 = new DeferrableMessageDeliveryTaskList();
      currentRequest.RequestCache.Set<DeferrableMessageDeliveryTaskList>(key, instance2);
      return instance2;
    }

    public void Run<T>(ClientContext context) where T : class
    {
      List<DeferrableMessageDeliveryTask> all = this._tasks.FindAll((Predicate<DeferrableMessageDeliveryTask>) (x => x.Message is T));
      IDeferrableMessageTaskHandler taskHandler = this.GetTaskHandler(typeof (T));
      foreach (DeferrableMessageDeliveryTask task in all)
      {
        try
        {
          taskHandler.Handle(task, context);
        }
        finally
        {
          this.Delete(task);
        }
      }
    }

    public void RunBatch<T>(ClientContext context, DeferrableDataBag<T> threadSafeDataBag = null) where T : class
    {
      DeferrableMessageDeliveryTask[] array = this._tasks.FindAll((Predicate<DeferrableMessageDeliveryTask>) (x => x.Message is T)).ToArray();
      IDeferrableMessageTaskHandler taskHandler = this.GetTaskHandler(typeof (T));
      try
      {
        taskHandler.Handle<T>(array, context, threadSafeDataBag);
      }
      finally
      {
        this.Delete(array);
      }
    }

    public void Add(DeferrableMessageDeliveryTask task) => this._tasks.Add(task);

    public void Delete(DeferrableMessageDeliveryTask task)
    {
      this._tasks.RemoveAll((Predicate<DeferrableMessageDeliveryTask>) (x => x.Id == task.Id));
    }

    public void Delete(DeferrableMessageDeliveryTask[] tasks)
    {
      foreach (DeferrableMessageDeliveryTask task in tasks)
        this.Delete(task);
    }

    public void Clear() => this._tasks.Clear();

    public void RegisterTaskHandler<messageType>(IDeferrableMessageTaskHandler handler) where messageType : class
    {
      lock (this._syncLock)
      {
        Type key = typeof (messageType);
        if (this._taskHandlers.ContainsKey(key))
          return;
        this._taskHandlers.Add(key, handler);
      }
    }

    public IDeferrableMessageTaskHandler GetTaskHandler(Type messageType)
    {
      return this._taskHandlers.ContainsKey(messageType) ? this._taskHandlers[messageType] : throw new ArgumentException("The message type '" + messageType.ToString() + "' has no associated handler");
    }
  }
}
