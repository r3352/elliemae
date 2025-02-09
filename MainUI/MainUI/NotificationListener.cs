// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.NotificationListener
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public static class NotificationListener
  {
    private const string className = "NotificationListener";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private static Queue<UserNotification> displayQueue = new Queue<UserNotification>();
    private static Thread displayThread = (Thread) null;
    private static AutoResetEvent dialogEvent = new AutoResetEvent(false);

    public static void Start()
    {
      if (NotificationListener.displayThread != null)
        return;
      NotificationListener.displayThread = new Thread(new ThreadStart(NotificationListener.waitForNotification));
      NotificationListener.displayThread.IsBackground = true;
      NotificationListener.displayThread.Priority = ThreadPriority.BelowNormal;
      NotificationListener.displayThread.Start();
      Session.Connection.ServerEvent += new ServerEventHandler(NotificationListener.onServerEvent);
    }

    private static void onServerEvent(IConnection sender, ServerEvent e)
    {
      if (!(e is NotificationEvent))
        return;
      lock (NotificationListener.displayQueue)
      {
        NotificationEvent notificationEvent = (NotificationEvent) e;
        Tracing.Log(NotificationListener.sw, nameof (NotificationListener), TraceLevel.Info, "Received Notification (" + (object) notificationEvent.Notification + ")");
        if (notificationEvent.Notification is ConcurrentUpdateNotification || notificationEvent.Notification is TradeLoanUpdateNotification)
          return;
        NotificationListener.displayQueue.Enqueue(notificationEvent.Notification);
        Monitor.Pulse((object) NotificationListener.displayQueue);
      }
    }

    private static string getUserSetting(string name)
    {
      return Session.StartupInfo.UserProfileSettings.Contains((object) name) ? string.Concat(Session.StartupInfo.UserProfileSettings[(object) name]) : (string) null;
    }

    private static void waitForNotification()
    {
      try
      {
        Tracing.Log(NotificationListener.sw, nameof (NotificationListener), TraceLevel.Info, "Waiting for notifications on thread " + (object) Thread.CurrentThread.GetHashCode());
        while (true)
        {
          UserNotification userNotification = (UserNotification) null;
          lock (NotificationListener.displayQueue)
          {
            while (NotificationListener.displayQueue.Count == 0)
              Monitor.Wait((object) NotificationListener.displayQueue);
            userNotification = NotificationListener.displayQueue.Dequeue();
          }
          Session.MainForm.Invoke((Delegate) new WaitCallback(NotificationListener.showNotificationDialog), (object) userNotification);
          NotificationListener.dialogEvent.WaitOne();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(NotificationListener.sw, nameof (NotificationListener), TraceLevel.Error, "Notification display thread terminated abnormally: " + (object) ex);
      }
    }

    private static void showNotificationDialog(object notificationObj)
    {
      try
      {
        Tracing.Log(NotificationListener.sw, nameof (NotificationListener), TraceLevel.Info, "Displaying Notification (" + notificationObj + ")");
        NotificationForm notificationForm = new NotificationForm(notificationObj as UserNotification);
        notificationForm.FormClosed += new FormClosedEventHandler(NotificationListener.onNotificationFormClosed);
        notificationForm.Show();
      }
      catch (Exception ex)
      {
        Tracing.Log(NotificationListener.sw, nameof (NotificationListener), TraceLevel.Error, "Error displaying notification dialog: " + (object) ex);
        NotificationListener.dialogEvent.Set();
      }
    }

    private static void onNotificationFormClosed(object sender, FormClosedEventArgs e)
    {
      NotificationListener.dialogEvent.Set();
    }
  }
}
