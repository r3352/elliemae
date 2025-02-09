// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.ConcurrentUpdateNotificationHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Confluent.Kafka;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  public class ConcurrentUpdateNotificationHandler : IConcurrentUpdateNotificationHandler
  {
    private const string className = "ConcurrentUpdateNotificationHandler�";
    private ClientContext context;
    private bool processStarted;
    private Thread subscriberThread;
    private IConsumer<string, string> consumer;
    private ILogger concurrentUpdateTraceLog;
    private CancellationTokenSource cts;
    private NotificationStatus status = new NotificationStatus();

    public ConcurrentUpdateNotificationHandler(ClientContext context)
    {
      this.context = context != null ? context : throw new ArgumentNullException(nameof (context), "cannot be null");
      this.concurrentUpdateTraceLog = DiagUtility.LogManager.GetLogger("ConcurrentUpdates");
    }

    public string GetNotificationStatus() => this.status.ToString();

    private IClientSession GetSession(string sessionId)
    {
      return this.context.Sessions.GetSession(sessionId);
    }

    public IConcurrentUpdateNotificationResult SendNotification(
      string sessionId,
      string loanGuid,
      DateTime notificationTime,
      string correlationId = "�")
    {
      NotificationResult notificationResult = new NotificationResult();
      string str = string.Format("LoanGuid: {0}, SessionId: {1}, NotificationTime: {2}, CorrelationId: {3}", (object) loanGuid, (object) sessionId, (object) notificationTime, (object) correlationId);
      try
      {
        IClientSession clientSession = !string.IsNullOrEmpty(sessionId) && !string.IsNullOrEmpty(loanGuid) ? this.GetSession(sessionId) : throw new ArgumentException("sessionId and loanGuid are required parameters. They cannot be null or empty.");
        if (clientSession == null)
        {
          notificationResult.IsSessionNotFound = true;
          return (IConcurrentUpdateNotificationResult) notificationResult;
        }
        ConcurrentDictionary<string, string> notificationCache = clientSession.ConcurrentUpdatesNotificationCache;
        if (notificationCache.ContainsKey(loanGuid))
        {
          notificationResult.IsNotificationSuppressed = true;
          this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Notification suppressed. Params: " + str);
          return (IConcurrentUpdateNotificationResult) notificationResult;
        }
        clientSession.RaiseEvent((ServerEvent) new NotificationEvent(clientSession.SessionInfo, (UserNotification) new ConcurrentUpdateNotification(clientSession.UserID, loanGuid, correlationId, notificationTime)));
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Notification sent. Params: " + str);
        if (!notificationCache.TryAdd(loanGuid, loanGuid))
          this.Log(Encompass.Diagnostics.Logging.LogLevel.WARN, "Error encountered while saving the notification to cache");
      }
      catch (Exception ex)
      {
        notificationResult.NotificationFailed = true;
        notificationResult.ErrorMessage = ex.Message;
        this.Log(Encompass.Diagnostics.Logging.LogLevel.ERROR, "Error encountered while sending notification: " + ex.Message + " " + Environment.NewLine + " Params: " + str + " ");
      }
      return (IConcurrentUpdateNotificationResult) notificationResult;
    }

    public void StartProcess()
    {
      this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Starting notification subscriber.");
      if (this.processStarted)
        return;
      lock (this)
      {
        if (this.processStarted)
          return;
        this.cts = new CancellationTokenSource();
        this.subscriberThread = new Thread(new ThreadStart(this.pollForConcurrentUpdateNotifications));
        this.subscriberThread.IsBackground = true;
        this.subscriberThread.Priority = ThreadPriority.Lowest;
        this.subscriberThread.Start();
        this.processStarted = true;
      }
    }

    public void StopProcess()
    {
      if (this.subscriberThread != null && this.subscriberThread.IsAlive && this.cts != null)
      {
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Stopping the current notification subscriber...");
        this.cts.Cancel();
        this.subscriberThread.Join();
        this.subscriberThread = (Thread) null;
        this.consumer = (IConsumer<string, string>) null;
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Stopped/Unregistered notification subscriber.");
      }
      else
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "No active subscription found, to be stopped.");
    }

    private ConcurrentNotificationPayload ParseMessage(string message)
    {
      ConcurrentNotificationPayload message1 = new ConcurrentNotificationPayload();
      try
      {
        object obj1 = (object) JObject.Parse(message);
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__0, obj1);
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "client", typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__1.Target((CallSite) ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__1, obj2);
        ConcurrentNotificationPayload notificationPayload1 = message1;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConcurrentUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target1 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p4 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__4;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target2 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p3 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "correlationId", typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__2.Target((CallSite) ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__2, obj1);
        object obj5 = target2((CallSite) p3, obj4);
        string str1 = target1((CallSite) p4, obj5);
        notificationPayload1.CorrealtionId = str1;
        ConcurrentNotificationPayload notificationPayload2 = message1;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConcurrentUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target3 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__7.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p7 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__7;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target4 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p6 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__6;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "userId", typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__5.Target((CallSite) ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__5, obj2);
        object obj7 = target4((CallSite) p6, obj6);
        string str2 = target3((CallSite) p7, obj7);
        notificationPayload2.UserId = str2;
        ConcurrentNotificationPayload notificationPayload3 = message1;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConcurrentUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target5 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__10.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p10 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__10;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target6 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p9 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__9;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "sessionId", typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj8 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__8.Target((CallSite) ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__8, obj2);
        object obj9 = target6((CallSite) p9, obj8);
        string str3 = target5((CallSite) p10, obj9);
        notificationPayload3.SessionId = str3;
        ConcurrentNotificationPayload notificationPayload4 = message1;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConcurrentUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target7 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__13.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p13 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__13;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target8 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__12.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p12 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__12;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "loanId", typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj10 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__11.Target((CallSite) ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__11, obj2);
        object obj11 = target8((CallSite) p12, obj10);
        string str4 = target7((CallSite) p13, obj11);
        notificationPayload4.LoanId = str4;
        DateTime now = DateTime.Now;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__16 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__16 = CallSite<\u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, DateTime>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "TryParse", (IEnumerable<Type>) null, typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        \u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, DateTime> target9 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__16.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<\u003C\u003EA\u007B00000008\u007D<CallSite, Type, object, DateTime>> p16 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__16;
        Type type = typeof (DateTime);
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target10 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__15.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p15 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__15;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "publishTime", typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj12 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__14.Target((CallSite) ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__14, obj2);
        object obj13 = target10((CallSite) p15, obj12);
        ref DateTime local = ref now;
        target9((CallSite) p16, type, obj13, ref local);
        message1.NotificationTime = now.ToUniversalTime();
        ConcurrentNotificationPayload notificationPayload5 = message1;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__19 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ConcurrentUpdateNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target11 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__19.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p19 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__19;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__18 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target12 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__18.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p18 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__18;
        // ISSUE: reference to a compiler-generated field
        if (ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__17 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "instanceId", typeof (ConcurrentUpdateNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj14 = ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__17.Target((CallSite) ConcurrentUpdateNotificationHandler.\u003C\u003Eo__14.\u003C\u003Ep__17, obj3);
        object obj15 = target12((CallSite) p18, obj14);
        string str5 = target11((CallSite) p19, obj15);
        notificationPayload5.InstanceName = str5;
        return message1;
      }
      catch (Exception ex)
      {
        this.Log(Encompass.Diagnostics.Logging.LogLevel.WARN, "Parsing Error - Message in incorrect format: " + ex.Message + ", Payload: " + message);
        return (ConcurrentNotificationPayload) null;
      }
    }

    private string getTopic()
    {
      return KafkaUtils.DeploymentProfile + "." + KafkaUtils.Region + ".loan.smartClientSync";
    }

    public void Log(Encompass.Diagnostics.Logging.LogLevel LogLevel, string message)
    {
      using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        this.concurrentUpdateTraceLog.Write(LogLevel, nameof (ConcurrentUpdateNotificationHandler), message);
    }

    private void pollForConcurrentUpdateNotifications()
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
              string message1 = consumeResult.Message.Value;
              try
              {
                using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
                {
                  ConcurrentNotificationPayload message2 = this.ParseMessage(message1);
                  if (message2 != null)
                  {
                    if (message2.InstanceName.Trim().Equals(this.context.InstanceName.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                      this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Received message: " + Environment.NewLine + " " + message1);
                      this.status.LastReceivedMessage = message1;
                      this.SendNotification(message2.SessionId, message2.LoanId, message2.NotificationTime, message2.CorrealtionId);
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
            this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Notifications polling canceled. Closing consumer...");
            this.consumer.Close();
            this.processStarted = false;
            this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Closed consumer.");
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
