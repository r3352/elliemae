// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Event.LoanBillingEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.MessageServices.Message;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Event
{
  public class LoanBillingEvent : MessageQueueEvent
  {
    public const string tenant = "urn:elli:encompass:{0}:user:{1}�";
    private string _instanceId;
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.Region, (object) "loan.billing");

    public LoanBillingEvent(
      string instanceId,
      string loanId,
      string userId,
      Enums.Source source,
      DateTime loanModifiedTime)
    {
      this._instanceId = instanceId == null ? string.Empty : instanceId.Trim();
      this.StandardMessage = this.CreateMessageHeader(loanId, userId, source, loanModifiedTime);
      this.QueueMessages = new List<QueueMessage>();
    }

    private StandardMessage CreateMessageHeader(
      string loanId,
      string userId,
      Enums.Source source,
      DateTime loanModifiedTime)
    {
      return new StandardMessage()
      {
        EntityId = loanId,
        InstanceId = this._instanceId,
        UserId = userId,
        Tenant = string.Format("urn:elli:encompass:{0}:user:{1}", (object) this._instanceId, (object) userId),
        Category = Enums.Category.EVENT,
        Source = EnumUtils.StringValueOf((Enum) source),
        CreateAt = loanModifiedTime
      };
    }

    public void AddKafkaMessage(
      string correlationId,
      string loanEventType,
      TransactionLog transactionLog)
    {
      this.QueueMessages.Add((QueueMessage) LoanBillingMessage.CreateLoanBillingMessage(correlationId, loanEventType, transactionLog));
    }

    public override string GetTopic(string messageType) => LoanBillingEvent.Topic;
  }
}
