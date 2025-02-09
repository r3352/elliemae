// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Kafka.Event.DeferredLoanEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using Elli.ElliEnum;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka.Message;
using EllieMae.EMLite.ClientServer.MessageServices.Message;
using EllieMae.EMLite.Common.RemotingServices;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Kafka.Event
{
  public class DeferredLoanEvent : MessageQueueEvent
  {
    public const string eventActionTypePrefix = "DeferredLoan.�";
    public const string TargetPriorLoanFileName = "_^_Before_loan.em�";
    public const string TargetAfterLoanFileName = "_^_After_loan.em�";
    public const string deferredTenant = "urn:elli:encompass:{0}:user:{1}�";
    public string region = KafkaUtils.Region;
    public string topicExtension = "loan.deferred";

    public DeferredLoanEvent(
      string eventId,
      string instanceId,
      string serviceId,
      string siteId,
      string userId,
      string region,
      string topicExtension,
      bool isSourceEncompass)
    {
      this.StandardMessage = this.CreateMessageHeader(eventId, instanceId, serviceId, siteId, userId, isSourceEncompass);
      this.QueueMessages = new List<QueueMessage>();
      this.region = region;
      this.topicExtension = topicExtension;
    }

    private StandardMessage CreateMessageHeader(
      string eventId,
      string instanceId,
      string serviceId,
      string siteId,
      string userId,
      bool isSourceEncompass)
    {
      instanceId = instanceId == null ? string.Empty : instanceId.Trim();
      return new StandardMessage()
      {
        EntityId = eventId,
        InstanceId = instanceId,
        SiteId = siteId,
        UserId = userId,
        Tenant = string.Format("urn:elli:encompass:{0}:user:{1}", (object) instanceId, (object) userId),
        Category = Enums.Category.EVENT,
        Source = isSourceEncompass ? EnumUtils.StringValueOf((Enum) Enums.Source.URN_ELLI_SERVICE_ENCOMPASS) : EnumUtils.StringValueOf((Enum) Enums.Source.URN_ELLI_SERVICE_EBS),
        CreateAt = DateTime.Now
      };
    }

    public void AddKafkaMessage(
      string correlationId,
      string actionType,
      string loanId,
      string loanPath,
      LoanActionType? loanActionType,
      DateTime auditModifiedTime,
      DateTime auditCurrentTime,
      DateTime xdbModifiedTime,
      string priorLoanFileName,
      string afterLoanFileName,
      string auditUserId = null,
      string clientId = null)
    {
      Enums.Type actionType1 = (Enums.Type) EnumUtils.EnumValueOf("DeferredLoan." + actionType, typeof (Enums.Type));
      switch (actionType1)
      {
        case Enums.Type.DEFERRED_LOAN_STATUS_ONLINE:
          this.QueueMessages.Add((QueueMessage) LoanEventQueueMessage.CreateLoanEventKafkaMessage(correlationId, actionType1, loanId, loanPath));
          break;
        case Enums.Type.DEFERRED_LOAN_REPORTING_DATABASE:
          this.QueueMessages.Add((QueueMessage) ReportingDbMessage.CreateReportingDbMessage(correlationId, actionType1, loanId, loanPath, this.StandardMessage == null || string.IsNullOrWhiteSpace(this.StandardMessage.EntityId) || this.StandardMessage.EntityId.Equals("CreateLoan", StringComparison.OrdinalIgnoreCase) ? (string) null : priorLoanFileName, clientId));
          break;
        case Enums.Type.DEFERRED_LOAN_AUDIT_TRAIL:
          this.QueueMessages.Add((QueueMessage) AuditTrailMessage.CreateAuditTrailMessage(correlationId, actionType1, loanId, loanPath, priorLoanFileName, afterLoanFileName, auditModifiedTime, auditCurrentTime, xdbModifiedTime, auditUserId, clientId));
          break;
        case Enums.Type.DEFERRED_LOAN_ALERT_NOTIFICATION:
          break;
        case Enums.Type.DEFERRED_LOAN_SERVICE_ORDER:
          break;
        default:
          this.QueueMessages.Add((QueueMessage) LoanEventQueueMessage.CreateLoanEventKafkaMessage(correlationId, actionType1, loanId, loanPath));
          break;
      }
    }

    public void CreateEmailKafkaMessage(
      string correlationId,
      string actionType,
      string loanId,
      string loanPath,
      IEnumerable<EmailNotificationInfo> emailNotificationList)
    {
      Enums.Type actionType1 = (Enums.Type) EnumUtils.EnumValueOf("DeferredLoan." + actionType, typeof (Enums.Type));
      if (emailNotificationList == null)
        return;
      this.QueueMessages.Add((QueueMessage) EmailNotificationMessage.CreateEmailNotificationMessage(correlationId, actionType1, loanId, loanPath, emailNotificationList.ToList<EmailNotificationInfo>()));
    }

    public override string GetTopic(string messageType)
    {
      string str = this.topicExtension;
      switch ((Enums.Type) EnumUtils.EnumValueOf(messageType, typeof (Enums.Type)))
      {
        case Enums.Type.DEFERRED_LOAN_EMAIL_TRIGGER:
        case Enums.Type.DEFERRED_LOAN_MILESTONE_EMAIL_NOTIFICATION:
          this.region = KafkaUtils.AwsRegion;
          break;
        case Enums.Type.DEFERRED_LOAN_STATUS_ONLINE:
          str = "loan.statusonline";
          break;
        case Enums.Type.DEFERRED_LOAN_REPORTING_DATABASE:
          str = "loan.reportingdb";
          break;
        case Enums.Type.DEFERRED_LOAN_AUDIT_TRAIL:
          str = "loan.audittrail";
          break;
      }
      return string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) this.region, (object) str);
    }
  }
}
