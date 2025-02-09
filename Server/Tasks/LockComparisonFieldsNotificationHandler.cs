// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.LockComparisonFieldsNotificationHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Confluent.Kafka;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.LockComparison;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka;
using EllieMae.EMLite.Server.ServerObjects;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  public class LockComparisonFieldsNotificationHandler : ILockComparisonFieldsNotificationHandler
  {
    private const string className = "LockComparisonFieldsNotificationHandler�";
    private ClientContext context;
    private bool processStarted;
    private Thread subscriberThread;
    private IConsumer<string, string> consumer;
    private readonly ILogger lockComparisonFieldsTraceLog;
    private CancellationTokenSource cts;
    private LockComparisonFieldsNotificationStatus status = new LockComparisonFieldsNotificationStatus();

    public LockComparisonFieldsNotificationHandler(ClientContext context)
    {
      this.context = context != null ? context : throw new ArgumentNullException(nameof (context), "ClienContex cannot be null in LockComparisonFieldsNotificationHandler");
      this.lockComparisonFieldsTraceLog = DiagUtility.LogManager.GetLogger("TradeLoanUpdates");
    }

    public string GetLockComparisonFieldsNotificationStatus() => this.status.ToString();

    public void StartProcess()
    {
      this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Starting Lock Comparison Fields subscriber.");
      if (this.processStarted)
        return;
      lock (this)
      {
        if (this.processStarted)
          return;
        this.cts = new CancellationTokenSource();
        this.subscriberThread = new Thread(new ThreadStart(this.ConsumeKafkaMessages))
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
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Stopping the current Lock Comparison Fields subscriber...");
        this.cts.Cancel();
        this.subscriberThread.Join();
        this.subscriberThread = (Thread) null;
        this.consumer = (IConsumer<string, string>) null;
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Stopped/Unregistered Lock Comparison Fields subscriber.");
      }
      else
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "No active Lock Comparison Fields subscription found, to be stopped.");
    }

    private LockComparisonFieldsNotificationPayload ParseKafkaMessage(string message)
    {
      LockComparisonFieldsNotificationPayload kafkaMessage = new LockComparisonFieldsNotificationPayload();
      try
      {
        object obj1 = (object) JObject.Parse(message);
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "payload", typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__0, obj1);
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__8.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p8 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__8;
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target2 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> p2 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__2;
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "fields", typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__1.Target((CallSite) LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__1, obj2);
        object obj4 = target2((CallSite) p2, obj3, (object) null);
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        object obj5;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__7.Target((CallSite) LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__7, obj4))
        {
          // ISSUE: reference to a compiler-generated field
          if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target3 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__6.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> p6 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__6;
          object obj6 = obj4;
          // ISSUE: reference to a compiler-generated field
          if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThanOrEqual, typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int, object> target4 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__5.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int, object>> p5 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__5;
          // ISSUE: reference to a compiler-generated field
          if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target5 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p4 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__4;
          // ISSUE: reference to a compiler-generated field
          if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "fields", typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__3.Target((CallSite) LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__3, obj2);
          object obj8 = target5((CallSite) p4, obj7);
          object obj9 = target4((CallSite) p5, obj8, 0);
          obj5 = target3((CallSite) p6, obj6, obj9);
        }
        else
          obj5 = obj4;
        if (target1((CallSite) p8, obj5))
          return (LockComparisonFieldsNotificationPayload) null;
        LockComparisonFieldsNotificationPayload notificationPayload1 = kafkaMessage;
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LockComparisonFieldsNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target6 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__11.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p11 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__11;
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target7 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__10.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p10 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__10;
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "correlationId", typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj10 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__9.Target((CallSite) LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__9, obj1);
        object obj11 = target7((CallSite) p10, obj10);
        string str1 = target6((CallSite) p11, obj11);
        notificationPayload1.CorrelationId = str1;
        LockComparisonFieldsNotificationPayload notificationPayload2 = kafkaMessage;
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (LockComparisonFieldsNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target8 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__14.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p14 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__14;
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target9 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__13.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p13 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__13;
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "instanceId", typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj12 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__12.Target((CallSite) LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__12, obj2);
        object obj13 = target9((CallSite) p13, obj12);
        string str2 = target8((CallSite) p14, obj13);
        notificationPayload2.InstanceId = str2;
        List<LockComparisonField> lockComparisonFieldList1 = new List<LockComparisonField>();
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__18 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (LockComparisonFieldsNotificationHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, IEnumerable> target10 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__18.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, IEnumerable>> p18 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__18;
        // ISSUE: reference to a compiler-generated field
        if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "fields", typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj14 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__15.Target((CallSite) LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__15, obj2);
        foreach (object obj15 in target10((CallSite) p18, obj14))
        {
          // ISSUE: reference to a compiler-generated field
          if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__17 = CallSite<Action<CallSite, List<LockComparisonField>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, List<LockComparisonField>, object> target11 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__17.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, List<LockComparisonField>, object>> p17 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__17;
          List<LockComparisonField> lockComparisonFieldList2 = lockComparisonFieldList1;
          // ISSUE: reference to a compiler-generated field
          if (LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToObject", (IEnumerable<Type>) null, typeof (LockComparisonFieldsNotificationHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj16 = LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__16.Target((CallSite) LockComparisonFieldsNotificationHandler.\u003C\u003Eo__12.\u003C\u003Ep__16, obj15, typeof (LockComparisonField));
          target11((CallSite) p17, lockComparisonFieldList2, obj16);
        }
        kafkaMessage.Fields = (IList<LockComparisonField>) lockComparisonFieldList1;
        kafkaMessage.NotificationTime = DateTime.Now.ToUniversalTime();
        return kafkaMessage;
      }
      catch (Exception ex)
      {
        this.Log(Encompass.Diagnostics.Logging.LogLevel.ERROR, "Parsing Error - Message in incorrect format: " + ex.Message + ", Payload: " + message);
        return (LockComparisonFieldsNotificationPayload) null;
      }
    }

    private string GetTopic()
    {
      return KafkaUtils.DeploymentProfile + "." + KafkaUtils.Region + ".setting.lockComparisonFields";
    }

    public void Log(Encompass.Diagnostics.Logging.LogLevel LogLevel, string message)
    {
      using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        this.lockComparisonFieldsTraceLog.Write(LogLevel, nameof (LockComparisonFieldsNotificationHandler), message);
    }

    private void ConsumeKafkaMessages()
    {
      string topic = this.GetTopic();
      this.status.Topic = topic;
      this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Subscribing to the topic: " + topic + " ");
      try
      {
        string groupId = string.IsNullOrEmpty(this.context.InstanceName) ? "default" : this.context.InstanceName.ToUpper();
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
                  LockComparisonFieldsNotificationPayload kafkaMessage = this.ParseKafkaMessage(message);
                  if (kafkaMessage != null)
                  {
                    if (kafkaMessage.InstanceId.Trim().Equals(this.context.InstanceName.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                      this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, "Received message: " + Environment.NewLine + " " + message);
                      LockComparisonFields.InsertLockComparisonFields(kafkaMessage.Fields);
                      this.status.LastReceivedMessage = message;
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
            this.Log(Encompass.Diagnostics.Logging.LogLevel.ERROR, "Lock Comparison Fields Notifications polling canceled. Closing consumer...");
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
