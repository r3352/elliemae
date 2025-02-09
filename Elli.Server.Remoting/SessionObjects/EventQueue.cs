// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.EventQueue
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server;
using System;
using System.Collections;
using System.Threading;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public static class EventQueue
  {
    private static readonly int MaxThreadCount = 5;
    private static Queue eventQueue = new Queue();
    private static ArrayList threads = new ArrayList();

    static EventQueue()
    {
      EventQueue.MaxThreadCount = Utils.ParseInt((object) EnConfigurationSettings.AppSettings["EventQueueThreadCount"], EventQueue.MaxThreadCount);
    }

    public static int Length => EventQueue.eventQueue.Count;

    public static AsyncEventResult Enqueue(DelegateInvoker invoker)
    {
      EventQueue.QueuedEvent queuedEvent = new EventQueue.QueuedEvent(invoker);
      lock (EventQueue.eventQueue)
      {
        EventQueue.eventQueue.Enqueue((object) queuedEvent);
        EventQueueCounters.EventQueue.Increment();
        Monitor.Pulse((object) EventQueue.eventQueue);
      }
      lock (EventQueue.eventQueue)
      {
        if (EventQueue.eventQueue.Count > 0)
        {
          lock (EventQueue.threads)
          {
            if (EventQueue.threads.Count < EventQueue.MaxThreadCount)
            {
              Thread thread = new Thread(new ThreadStart(EventQueue.processQueue));
              EventQueue.threads.Add((object) thread);
              thread.Name = "EventExecutionThread#" + (object) EventQueue.threads.Count;
              thread.IsBackground = true;
              thread.Priority = ThreadPriority.BelowNormal;
              thread.Start();
            }
          }
        }
      }
      return queuedEvent.Result;
    }

    private static void processQueue()
    {
      try
      {
        while (true)
        {
          EventQueue.QueuedEvent queuedEvent = (EventQueue.QueuedEvent) null;
          lock (EventQueue.eventQueue)
          {
            while (EventQueue.eventQueue.Count == 0)
              Monitor.Wait((object) EventQueue.eventQueue);
            queuedEvent = (EventQueue.QueuedEvent) EventQueue.eventQueue.Dequeue();
            EventQueueCounters.EventQueue.Decrement();
          }
          Exception ex1 = (Exception) null;
          try
          {
            queuedEvent.Invoker.Invoke();
          }
          catch (Exception ex2)
          {
            ex1 = ex2;
          }
          try
          {
            queuedEvent.Result.SetComplete(ex1);
          }
          catch
          {
          }
        }
      }
      catch
      {
      }
    }

    private class QueuedEvent
    {
      public readonly DelegateInvoker Invoker;
      public readonly AsyncEventResult Result;

      public QueuedEvent(DelegateInvoker invoker)
      {
        this.Invoker = invoker;
        this.Result = new AsyncEventResult(invoker);
      }
    }
  }
}
