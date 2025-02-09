// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Event.LoanChangeEvent
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
  public class LoanChangeEvent : MessageQueueEvent
  {
    public const string tenant = "urn:elli:encompass:{0}:user:{1}�";
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.Region, (object) "loan.change");
    private string _instanceId;

    public LoanChangeEvent(string instanceId, string entityId, string userId, Enums.Source source)
    {
      this._instanceId = instanceId?.Trim() ?? string.Empty;
      this.StandardMessage = this.CreateMessageHeader(entityId, userId, source);
      this.QueueMessages = new List<QueueMessage>();
    }

    private StandardMessage CreateMessageHeader(
      string entityId,
      string userId,
      Enums.Source source)
    {
      return new StandardMessage()
      {
        EntityId = entityId,
        InstanceId = this._instanceId,
        UserId = userId,
        Tenant = string.Format("urn:elli:encompass:{0}:user:{1}", (object) this._instanceId, (object) userId),
        Category = Enums.Category.EVENT,
        Source = EnumUtils.StringValueOf((Enum) source),
        CreateAt = DateTime.UtcNow
      };
    }

    public void AddKafkaMessage(
      string loanId,
      string instanceId,
      string clientId,
      string userId,
      bool isSourceEncompass)
    {
      this.QueueMessages.Add((QueueMessage) LoanChangeMessage.CreateLoanChangeMessage(loanId, instanceId, clientId, userId, isSourceEncompass));
    }

    public override string GetTopic(string messageType) => LoanChangeEvent.Topic;
  }
}
