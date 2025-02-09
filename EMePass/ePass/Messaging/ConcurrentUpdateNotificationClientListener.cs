// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Messaging.ConcurrentUpdateNotificationClientListener
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.ePass.Messaging
{
  public static class ConcurrentUpdateNotificationClientListener
  {
    private const string className = "StagedDocumentUpdatesClientListener";
    private static string sw = Tracing.SwConcurrentUpdates;
    private static readonly TimeSpan requestTimeout = TimeSpan.FromSeconds(5.0);
    private static bool listening = false;

    public static event ConcurrentUpdateNotificationEventHandler ConcurrentUpdateNotificationActivity;

    public static void StartListening()
    {
      if (ConcurrentUpdateNotificationClientListener.listening)
        return;
      Session.Connection.ServerEvent += new ServerEventHandler(ConcurrentUpdateNotificationClientListener.onServerEvent);
      ConcurrentUpdateNotificationClientListener.listening = true;
    }

    private static void onServerEvent(IConnection sender, ServerEvent e)
    {
      if (!(e is NotificationEvent))
        return;
      NotificationEvent notificationEvent = e as NotificationEvent;
      if (!(notificationEvent.Notification is ConcurrentUpdateNotification))
        return;
      ConcurrentUpdateNotification notification = notificationEvent.Notification as ConcurrentUpdateNotification;
      if (ConcurrentUpdateNotificationClientListener.ConcurrentUpdateNotificationActivity == null)
        return;
      ConcurrentUpdateNotificationClientListener.ConcurrentUpdateNotificationActivity((object) sender, new ConcurrentUpdateArgs(notification.LoanGuid, notification.Timestamp, notification.CorrelationId));
    }
  }
}
