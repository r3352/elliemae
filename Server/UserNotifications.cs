// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.UserNotifications
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class UserNotifications
  {
    private const string className = "UserNotifications�";
    private static Queue<UserNotifications.QueuedNotification> notificationQueue = new Queue<UserNotifications.QueuedNotification>();
    private static Thread notificationThread = (Thread) null;

    static UserNotifications()
    {
      UserNotifications.notificationThread = new Thread(new ThreadStart(UserNotifications.processNotificationQueue));
      UserNotifications.notificationThread.IsBackground = true;
      UserNotifications.notificationThread.Priority = ThreadPriority.Lowest;
      UserNotifications.notificationThread.Start();
    }

    public static void Send(UserNotification notification)
    {
      lock (UserNotifications.notificationQueue)
      {
        UserNotifications.notificationQueue.Enqueue(new UserNotifications.QueuedNotification(ClientContext.GetCurrent(), notification));
        Monitor.Pulse((object) UserNotifications.notificationQueue);
        TraceLog.WriteInfo(nameof (UserNotifications), "Successfully queued UserNotification (" + (object) notification + ")");
      }
    }

    private static void processNotificationQueue()
    {
      try
      {
        while (true)
        {
          try
          {
            UserNotifications.processNextNotification();
          }
          catch (Exception ex)
          {
            TraceLog.WriteError(nameof (UserNotifications), "Error processing user notification: " + (object) ex);
          }
        }
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (UserNotifications), "User notification processing thread terminated abnormally: " + (object) ex);
      }
    }

    private static void processNextNotification()
    {
      UserNotifications.QueuedNotification queuedNotification = (UserNotifications.QueuedNotification) null;
      lock (UserNotifications.notificationQueue)
      {
        while (UserNotifications.notificationQueue.Count == 0)
          Monitor.Wait((object) UserNotifications.notificationQueue);
        queuedNotification = UserNotifications.notificationQueue.Dequeue();
      }
      foreach (IClientSession clientSession in queuedNotification.Context.Sessions.GetSessionsForUser(queuedNotification.Notification.UserID))
      {
        try
        {
          clientSession.RaiseEvent((ServerEvent) new NotificationEvent(clientSession.SessionInfo, queuedNotification.Notification));
          TraceLog.WriteInfo(nameof (UserNotifications), "Successfully dispatched user notification (" + (object) queuedNotification.Notification + ") to session " + (object) clientSession.SessionInfo);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (UserNotifications), "Failed to dispatch user notification (" + (object) queuedNotification.Notification + ") to session " + (object) clientSession.SessionInfo + ": " + (object) ex);
        }
      }
    }

    private class QueuedNotification
    {
      public readonly ClientContext Context;
      public readonly UserNotification Notification;

      public QueuedNotification(ClientContext context, UserNotification notification)
      {
        this.Context = context;
        this.Notification = notification;
      }
    }
  }
}
