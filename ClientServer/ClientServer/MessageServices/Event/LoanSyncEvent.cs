// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Event.LoanSyncEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer.MessageServices.Message;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Event
{
  public class LoanSyncEvent : MessageQueueEvent
  {
    private readonly string _instanceId;
    private readonly string _loanId;
    public const string tenant = "urn:elli:encompass:{0}:user:{1}�";
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.Region, (object) "loan.smartClientSync");

    public LoanSyncEvent(
      string instanceId,
      string loanId,
      string userId,
      Enums.Source source,
      DateTime loanModifiedTime)
    {
      this._instanceId = instanceId?.Trim() ?? string.Empty;
      this._loanId = loanId;
      this.StandardMessage = this.CreateMessageHeader(this._loanId, userId, source, loanModifiedTime);
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

    public void AddKafkaMessage(string correlationId, string lockedByUser, string sessionId)
    {
      this.QueueMessages.Add((QueueMessage) LoanSyncMessage.CreateLoanSyncMessage(correlationId, lockedByUser, sessionId, this._loanId, this._instanceId));
    }

    public override string GetTopic(string messageType) => LoanSyncEvent.Topic;
  }
}
