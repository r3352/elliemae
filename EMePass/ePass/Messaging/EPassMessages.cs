// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Messaging.EPassMessages
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.ePass.Messaging
{
  public static class EPassMessages
  {
    private const string className = "EPassMessages";
    private static string sw = Tracing.SwEpass;
    private static readonly TimeSpan requestTimeout = TimeSpan.FromSeconds(5.0);
    private static bool listening = false;
    private static bool isSynchronizing = false;

    public static event EPassMessageEventHandler MessageActivity;

    public static void StartListening()
    {
      if (EPassMessages.listening)
        return;
      Session.Connection.ServerEvent += new ServerEventHandler(EPassMessages.onServerEvent);
      EPassMessages.listening = true;
    }

    public static void SyncReadMessages(bool async)
    {
      if (async)
        ThreadPool.QueueUserWorkItem(new WaitCallback(EPassMessages.executeMessageSync));
      else
        EPassMessages.executeMessageSync((object) null);
    }

    private static void onServerEvent(IConnection sender, ServerEvent e)
    {
      if (!(e is NotificationEvent))
        return;
      NotificationEvent notificationEvent = (NotificationEvent) e;
      if (!(notificationEvent.Notification is EPassMessageNotification))
        return;
      EPassMessages.onMessageActivity(new EPassMessageEventArgs(EPassMessageEventType.MessageArrived, ((EPassMessageNotification) notificationEvent.Notification).Message));
    }

    private static void onMessageActivity(EPassMessageEventArgs eventArgs)
    {
      EPassMessageEventHandler messageEventHandler = (EPassMessageEventHandler) null;
      lock (typeof (EPassMessages))
      {
        if (EPassMessages.MessageActivity != null)
          messageEventHandler = (EPassMessageEventHandler) EPassMessages.MessageActivity.Clone();
      }
      if (messageEventHandler == null)
        return;
      messageEventHandler((object) null, eventArgs);
    }

    private static void executeMessageSync(object state)
    {
      Tracing.Log(EPassMessages.sw, nameof (EPassMessages), TraceLevel.Info, "Starting ePASS message sync processing.");
      try
      {
        EPassMessageServerResponse messageServerResponse = EPassMessages.queryEPassMessageServer();
        if (messageServerResponse == null)
          return;
        string[] readMessageIds = messageServerResponse.GetReadMessageIDs();
        if (readMessageIds.Length != 0)
          Session.ConfigurationManager.DeleteEPassMessages(readMessageIds);
        EPassMessages.onMessageActivity(new EPassMessageEventArgs(EPassMessageEventType.MessagesSynced));
      }
      catch (Exception ex)
      {
        Tracing.Log(EPassMessages.sw, nameof (EPassMessages), TraceLevel.Error, "ePASS Message Sync failed: " + (object) ex);
      }
    }

    private static EPassMessageServerResponse queryEPassMessageServer()
    {
      lock (typeof (EPassMessages))
      {
        if (EPassMessages.isSynchronizing)
          return (EPassMessageServerResponse) null;
        EPassMessages.isSynchronizing = true;
      }
      try
      {
        return EPassMessageServerResponse.QueryServer(Session.CompanyInfo.ClientID, "", Session.ConfigurationManager.GetCompanySetting("EPASSMSGS", "LastReadMsgDate"), true, EPassMessages.requestTimeout, Session.SessionObjects?.StartupInfo?.ServiceUrls?.ePassGetMessageAlertsUrl);
      }
      finally
      {
        lock (typeof (EPassMessages))
          EPassMessages.isSynchronizing = false;
      }
    }
  }
}
