// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.AsyncProcessMessageDeliveryTaskHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.ElliEnum;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka.Event;
using EllieMae.EMLite.ClientServer.MessageServices.Utils;
using EllieMae.EMLite.Common.RemotingServices;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  public class AsyncProcessMessageDeliveryTaskHandler : IDeferrableMessageTaskHandler
  {
    public const string BaseRoutingKey = "Elli.LoanMessage.DeferredLoanProcess�";

    public void Handle(DeferrableMessageDeliveryTask task, ClientContext context)
    {
      throw new NotImplementedException();
    }

    public void Handle<T>(
      DeferrableMessageDeliveryTask[] tasks,
      ClientContext context,
      DeferrableDataBag<T> threadSafeDataBag = null)
    {
      DeferrableDataBag<AsyncProcessMessage> dataBag = threadSafeDataBag != null ? threadSafeDataBag as DeferrableDataBag<AsyncProcessMessage> : DeferrableDataBag<AsyncProcessMessage>.GetInstance();
      List<Tuple<AsyncProcessMessage, string>> list = ((IEnumerable<DeferrableMessageDeliveryTask>) tasks).Select<DeferrableMessageDeliveryTask, Tuple<AsyncProcessMessage, string>>((Func<DeferrableMessageDeliveryTask, Tuple<AsyncProcessMessage, string>>) (task => new Tuple<AsyncProcessMessage, string>(AsyncProcessMessageDeliveryTaskHandler.Setup((AsyncProcessMessage) task.Message, dataBag), AsyncProcessMessageDeliveryTaskHandler.FormatRoutingKey(task.RoutingKey)))).ToList<Tuple<AsyncProcessMessage, string>>();
      if (dataBag == null)
        return;
      string region = KafkaUtils.Region;
      string topicExtension = "loan.deferred";
      if (dataBag.Get<bool>("BatchUpdateRequest"))
      {
        topicExtension = "loan.batchDeferred";
        region = KafkaUtils.Region;
      }
      bool isSourceEncompass = EncompassServer.ServerMode != EncompassServerMode.Service;
      DeferredLoanEvent queueEvent = new DeferredLoanEvent(dataBag.Get<string>("EventId"), dataBag.Get<string>("InstanceId"), dataBag.Get<string>("ServiceId"), dataBag.Get<string>("SiteId"), dataBag.Get<string>("UserId"), region, topicExtension, isSourceEncompass);
      string correlationId1 = ClientContext.CurrentRequest.CorrelationId;
      foreach (Tuple<AsyncProcessMessage, string> tuple in list)
      {
        AsyncProcessMessage asyncMessage = tuple.Item1;
        MessageActionType actionType1;
        if (asyncMessage != null)
        {
          if (asyncMessage.ActionType == MessageActionType.MilestoneEmailNotification || asyncMessage.ActionType == MessageActionType.EmailTrigger)
          {
            List<EmailNotificationInfo> source = dataBag.Get<List<EmailNotificationInfo>>("EmailNotificationInfoList");
            DeferredLoanEvent deferredLoanEvent = queueEvent;
            string correlationId2 = correlationId1 ?? asyncMessage.CorrelationId;
            actionType1 = asyncMessage.ActionType;
            string actionType2 = actionType1.ToString();
            string loanId = dataBag.Get<string>("LoanId");
            string loanPath = dataBag.Get<string>("LoanPath");
            IEnumerable<EmailNotificationInfo> emailNotificationList = source.Where<EmailNotificationInfo>((Func<EmailNotificationInfo, bool>) (item => item.EmailNotificationType == (asyncMessage.ActionType == MessageActionType.EmailTrigger ? EmailType.EmailTriggerNofication.ToString() : EmailType.MileStoneEmailNotification.ToString())));
            deferredLoanEvent.CreateEmailKafkaMessage(correlationId2, actionType2, loanId, loanPath, emailNotificationList);
          }
          else
          {
            DeferredLoanEvent deferredLoanEvent = queueEvent;
            string correlationId3 = correlationId1 ?? asyncMessage.CorrelationId;
            actionType1 = asyncMessage.ActionType;
            string actionType3 = actionType1.ToString();
            string loanId = dataBag.Get<string>("LoanId");
            string loanPath = dataBag.Get<string>("LoanPath");
            LoanActionType? loanActionType = dataBag.Get<LoanActionType?>("LoanActionType");
            DateTime auditModifiedTime = dataBag.Get<DateTime>("AuditModifiedTime");
            DateTime auditCurrentTime = dataBag.Get<DateTime>("AuditCurrentTime");
            DateTime xdbModifiedTime = dataBag.Get<DateTime>("XDBModifiedTime");
            string priorLoanFileName = dataBag.Get<string>("PriorLoanFileName");
            string afterLoanFileName = dataBag.Get<string>("AfterLoanFileName");
            string auditUserId = asyncMessage.AuditUserId;
            string clientId = context.ClientID;
            deferredLoanEvent.AddKafkaMessage(correlationId3, actionType3, loanId, loanPath, loanActionType, auditModifiedTime, auditCurrentTime, xdbModifiedTime, priorLoanFileName, afterLoanFileName, auditUserId, clientId);
          }
        }
      }
      if (queueEvent.QueueMessages.Count <= 0)
        return;
      IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
      IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
      queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
    }

    private static AsyncProcessMessage Setup(
      AsyncProcessMessage orgMessage,
      DeferrableDataBag<AsyncProcessMessage> dataBag)
    {
      return new AsyncProcessMessage(dataBag.Get<string>("ApplicationId"), dataBag.Get<string>("ServiceId"), dataBag.Get<string>("InstanceId"), dataBag.Get<string>("SiteId"), dataBag.Get<string>("EventId"), dataBag.Get<string>("UserId"), dataBag.Get<string>("AuditUserId"))
      {
        ActionType = orgMessage.ActionType,
        ServerMode = EncompassServer.ServerMode,
        IsRelayed = false,
        NotifyUserIds = dataBag.Get<string>("NotifyUserIds"),
        NotifyGroupIds = dataBag.Get<string>("NotifyGroupIds"),
        LoanId = dataBag.Get<string>("LoanId"),
        LoanFolder = dataBag.Get<string>("LoanFolder"),
        LoanPath = dataBag.Get<string>("LoanPath"),
        PriorLoanFileName = dataBag.Get<string>("PriorLoanFileName"),
        AfterLoanFileName = dataBag.Get<string>("AfterLoanFileName"),
        LoanActionType = dataBag.Get<LoanActionType?>("LoanActionType"),
        AuditModifiedTime = dataBag.Get<DateTime>("AuditModifiedTime"),
        AuditCurrentTime = dataBag.Get<DateTime>("AuditCurrentTime"),
        XDBModifiedTime = dataBag.Get<DateTime>("XDBModifiedTime"),
        UpdateForceRebuild = dataBag.Get<bool>("UpdateForceRebuild"),
        NewLogRecordGuid = dataBag.Get<string>("NewLogRecordGuid"),
        PublishTime = DateTime.UtcNow
      };
    }

    public static string FormatRoutingKey(string subRoutingKey)
    {
      return string.Format("{0}.{1}", (object) "Elli.LoanMessage.DeferredLoanProcess", (object) subRoutingKey);
    }
  }
}
