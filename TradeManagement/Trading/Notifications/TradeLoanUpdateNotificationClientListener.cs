// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.Notifications.TradeLoanUpdateNotificationClientListener
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading.Notifications
{
  public static class TradeLoanUpdateNotificationClientListener
  {
    private const string className = "TradeLoanUpdateNotificationClientListener";
    private static readonly TimeSpan requestTimeout = TimeSpan.FromSeconds(5.0);
    private static bool listening = false;

    public static event TradeLoanUpdateNotificationEventHandler TradeLoanUpdateNotificationActivity;

    public static void StartListening()
    {
      if (TradeLoanUpdateNotificationClientListener.listening)
        return;
      Session.Connection.ServerEvent += new ServerEventHandler(TradeLoanUpdateNotificationClientListener.onServerEvent);
      TradeLoanUpdateNotificationClientListener.listening = true;
    }

    private static void onServerEvent(IConnection sender, ServerEvent e)
    {
      if (!(e is NotificationEvent))
        return;
      NotificationEvent notificationEvent = e as NotificationEvent;
      if (!(notificationEvent.Notification is TradeLoanUpdateNotification))
        return;
      TradeLoanUpdateNotification notification = notificationEvent.Notification as TradeLoanUpdateNotification;
      if (TradeLoanUpdateNotificationClientListener.TradeLoanUpdateNotificationActivity == null)
        return;
      TradeLoanUpdateNotificationClientListener.TradeLoanUpdateNotificationActivity((object) sender, new TradeLoanUpdateArgs(notification.TradeId, notification.TradeStatus, notification.Timestamp, notification.CorrelationId));
    }
  }
}
