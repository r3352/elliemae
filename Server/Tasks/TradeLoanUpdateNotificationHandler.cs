// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.TradeLoanUpdateNotificationHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Confluent.Kafka;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka;
using EllieMae.EMLite.ClientServer.MessageServices.Message;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  public class TradeLoanUpdateNotificationHandler : ITradeLoanUpdateNotificationHandler
  {
    private const string className = "TradeLoanUpdateNotificationHandler�";
    private ClientContext context;
    private bool processStarted;
    private Thread subscriberThread;
    private IConsumer<string, string> consumer;
    private readonly ILogger tradeLoanUpdateTraceLog;
    private CancellationTokenSource cts;
    private TradeLoanUpdateNotificationStatus status = new TradeLoanUpdateNotificationStatus();

    public TradeLoanUpdateNotificationHandler(ClientContext context)
    {
      this.context = context != null ? context : throw new ArgumentNullException(nameof (context), "cannot be null");
      this.tradeLoanUpdateTraceLog = DiagUtility.LogManager.GetLogger("TradeLoanUpdates");
    }

    public string GetTradeLoanUpdateNotificationStatus() => this.status.ToString();

    private IClientSession GetSession(string sessionId)
    {
      return this.context.Sessions.GetSession(sessionId);
    }

    public ITradeLoanUpdateNotificationResult NotifyEncompassClient(
      string sessionId,
      string tradeId,
      string tradeStatus,
      DateTime notificationTime,
      string correlationId = "�")
    {
      TradeLoanUpdateNotificationResult notificationResult = new TradeLoanUpdateNotificationResult();
      string str = string.Format("TradeId: {0}, SessionId: {1}, NotificationTime: {2}, CorrelationId: {3}", (object) tradeId, (object) sessionId, (object) notificationTime, (object) correlationId);
      try
      {
        IClientSession clientSession = !string.IsNullOrEmpty(sessionId) && !string.IsNullOrEmpty(tradeId) ? this.GetSession(sessionId) : throw new ArgumentException("sessionId and tradeId are required parameters. They cannot be null or empty.");
        if (clientSession == null)
        {
          notificationResult.IsSessionNotFound = true;
          return (ITradeLoanUpdateNotificationResult) notificationResult;
        }
        clientSession.RaiseEvent((ServerEvent) new NotificationEvent(clientSession.SessionInfo, (UserNotification) new TradeLoanUpdateNotification(clientSession.UserID, tradeId, tradeStatus, correlationId, notificationTime)));
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Trade Loan Update Notification sent. Params: " + str);
      }
      catch (Exception ex)
      {
        notificationResult.TradeLoanUpdateNotificationFailed = true;
        string message = "Params: " + str + " " + Environment.NewLine + " Error encountered while sending Trade Loan Update notification: " + ex.Message + " ";
        notificationResult.ErrorMessage = message;
        this.Log(Encompass.Diagnostics.Logging.LogLevel.ERROR, message);
      }
      return (ITradeLoanUpdateNotificationResult) notificationResult;
    }

    public void StartProcess()
    {
      this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Starting Trade Loan Update subscriber.");
      if (this.processStarted)
        return;
      lock (this)
      {
        if (this.processStarted)
          return;
        this.cts = new CancellationTokenSource();
        this.subscriberThread = new Thread(new ThreadStart(this.consumeKafkaMessagesFromEbs))
        {
          IsBackground = true,
          Priority = ThreadPriority.Lowest
        };
        this.subscriberThread.Start();
        this.processStarted = true;
      }
    }

    public void StopProcess()
    {
      if (this.subscriberThread != null && this.subscriberThread.IsAlive && this.cts != null)
      {
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Stopping the current Trade Loan Update subscriber...");
        this.cts.Cancel();
        this.subscriberThread.Join();
        this.subscriberThread = (Thread) null;
        this.consumer = (IConsumer<string, string>) null;
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Stopped/Unregistered Trade Loan Update subscriber.");
      }
      else
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "No active trade loan update subscription found, to be stopped.");
    }

    private TradeLoanUpdateNotificationPayload ParseKafkaMessage(string message)
    {
      TradeLoanUpdateNotificationPayload kafkaMessage = new TradeLoanUpdateNotificationPayload();
      try
      {
        object obj1 = (object) JObject.Parse(message);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__0, obj1);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "client", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__1.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__1, obj2);
        TradeLoanUpdateNotificationPayload notificationPayload1 = kafkaMessage;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (TradeLoanUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target1 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p4 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__4;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target2 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p3 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "correlationId", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__2.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__2, obj1);
        object obj5 = target2((CallSite) p3, obj4);
        string str1 = target1((CallSite) p4, obj5);
        notificationPayload1.CorrealtionId = str1;
        TradeLoanUpdateNotificationPayload notificationPayload2 = kafkaMessage;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (TradeLoanUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target3 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__7.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p7 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__7;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target4 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p6 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__6;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "instanceId", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__5.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__5, obj2);
        object obj7 = target4((CallSite) p6, obj6);
        string str2 = target3((CallSite) p7, obj7);
        notificationPayload2.InstanceId = str2;
        TradeLoanUpdateNotificationPayload notificationPayload3 = kafkaMessage;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (TradeLoanUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target5 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p9 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__9;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "batchJobId", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj8 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__8.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__8, obj2);
        string str3 = target5((CallSite) p9, obj8);
        notificationPayload3.BatchJobId = str3;
        TradeLoanUpdateNotificationPayload notificationPayload4 = kafkaMessage;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (TradeLoanUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target6 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__11.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p11 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__11;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "batchJobStatus", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj9 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__10.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__10, obj2);
        string str4 = target6((CallSite) p11, obj9);
        notificationPayload4.BatchJobStatus = str4;
        TradeLoanUpdateNotificationPayload notificationPayload5 = kafkaMessage;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, BatchJobResult>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (BatchJobResult), typeof (TradeLoanUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, BatchJobResult> target7 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__14.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, BatchJobResult>> p14 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__14;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "batchJobResult", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj10 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__12.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__12, obj2);
        object obj11;
        if (obj10 == null)
        {
          obj11 = (object) null;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToObject", (IEnumerable<Type>) new Type[1]
            {
              typeof (BatchJobResult)
            }, typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj11 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__13.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__13, obj10);
        }
        BatchJobResult batchJobResult = target7((CallSite) p14, obj11);
        notificationPayload5.BatchJobResult = batchJobResult;
        TradeLoanUpdateNotificationPayload notificationPayload6 = kafkaMessage;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__16 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (TradeLoanUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target8 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__16.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p16 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__16;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "tradeId", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj12 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__15.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__15, obj2);
        string str5 = target8((CallSite) p16, obj12);
        notificationPayload6.TradeId = str5;
        TradeLoanUpdateNotificationPayload notificationPayload7 = kafkaMessage;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__18 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (TradeLoanUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target9 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__18.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p18 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__18;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__17 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "tradeStatus", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj13 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__17.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__17, obj2);
        string str6 = target9((CallSite) p18, obj13);
        notificationPayload7.TradeStatus = str6;
        TradeLoanUpdateNotificationPayload notificationPayload8 = kafkaMessage;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__20 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (TradeLoanUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target10 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__20.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p20 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__20;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__19 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "sessionId", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj14 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__19.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__19, obj2);
        string str7 = target10((CallSite) p20, obj14);
        notificationPayload8.SessionId = str7;
        TradeLoanUpdateNotificationPayload notificationPayload9 = kafkaMessage;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__22 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (TradeLoanUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, DateTime> target11 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__22.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, DateTime>> p22 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__22;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__21 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "publishTime", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj15 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__21.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__21, obj2);
        DateTime dateTime = target11((CallSite) p22, obj15);
        notificationPayload9.PublishTime = dateTime;
        DateTime now = DateTime.Now;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__25 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__25 = CallSite<\u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, DateTime>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "TryParse", (IEnumerable<Type>) null, typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        \u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, DateTime> target12 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__25.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<\u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, DateTime>> p25 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__25;
        Type type = typeof (DateTime);
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__24 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target13 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__24.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p24 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__24;
        // ISSUE: reference to a compiler-generated field
        if (TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__23 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "publishTime", typeof (TradeLoanUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj16 = TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__23.Target((CallSite) TradeLoanUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__23, obj2);
        object obj17 = target13((CallSite) p24, obj16);
        ref DateTime local = ref now;
        target12((CallSite) p25, type, obj17, ref local);
        kafkaMessage.NotificationTime = now.ToUniversalTime();
        return kafkaMessage;
      }
      catch (Exception ex)
      {
        this.Log(Encompass.Diagnostics.Logging.LogLevel.ERROR, "Parsing Error - Message in incorrect format: " + ex.Message + ", Payload: " + message);
        return (TradeLoanUpdateNotificationPayload) null;
      }
    }

    private string getTopic()
    {
      return KafkaUtils.DeploymentProfile + "." + KafkaUtils.Region + ".trade.batchJob";
    }

    public void Log(Encompass.Diagnostics.Logging.LogLevel LogLevel, string message)
    {
      using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        this.tradeLoanUpdateTraceLog.Write(LogLevel, nameof (TradeLoanUpdateNotificationHandler), message);
    }

    private void consumeKafkaMessagesFromEbs()
    {
      string topic = this.getTopic();
      this.status.Topic = topic;
      this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Subscribing to the topic: " + topic + " ");
      try
      {
        string groupId = string.IsNullOrEmpty(this.context.InstanceName) ? "default" : this.context.InstanceName.ToUpper() + "-" + Environment.MachineName;
        this.status.ConsumerGroupId = groupId;
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "ConsumerGroup ID: " + groupId + " ");
        using (this.consumer = new KafkaConsumer(groupId).Consumer.Build())
        {
          this.consumer.Subscribe(topic);
          try
          {
            while (true)
            {
              ConsumeResult<string, string> consumeResult;
              do
              {
                consumeResult = this.consumer.Consume(this.cts.Token);
              }
              while (consumeResult.Message == null);
              string message = consumeResult.Message.Value;
              try
              {
                using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
                {
                  TradeLoanUpdateNotificationPayload kafkaMessage = this.ParseKafkaMessage(message);
                  if (kafkaMessage != null)
                  {
                    if (kafkaMessage.InstanceId.Trim().Equals(this.context.InstanceName.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                      this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Received message: " + Environment.NewLine + " " + message);
                      this.status.LastReceivedMessage = message;
                      this.NotifyEncompassClient(kafkaMessage.SessionId, kafkaMessage.TradeId, kafkaMessage.TradeStatus, kafkaMessage.PublishTime, kafkaMessage.CorrealtionId);
                    }
                  }
                }
              }
              catch (ConsumeException ex)
              {
                this.Log(Encompass.Diagnostics.Logging.LogLevel.ERROR, "Consume error: " + ex.Error.Reason);
              }
              catch (Exception ex)
              {
                this.Log(Encompass.Diagnostics.Logging.LogLevel.ERROR, string.Format("Error encountered while processing the message: {0}", (object) ex.Message));
              }
            }
          }
          catch (OperationCanceledException ex)
          {
            this.Log(Encompass.Diagnostics.Logging.LogLevel.ERROR, "Trade Loan Update Notifications polling canceled. Closing consumer...");
            this.consumer.Close();
            this.processStarted = false;
            this.Log(Encompass.Diagnostics.Logging.LogLevel.ERROR, "Closed consumer.");
          }
        }
      }
      catch (Exception ex)
      {
        this.Log(Encompass.Diagnostics.Logging.LogLevel.ERROR, string.Format("Exception occured while subscribing to the topic: {0}", (object) ex.Message));
        this.processStarted = false;
      }
    }
  }
}
