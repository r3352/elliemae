// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Kafka.Event.UnlockAllEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Kafka.Event
{
  public class UnlockAllEvent(
    string serviceId,
    string instanceId,
    string siteId,
    string eventId,
    string userId,
    Enums.Source source,
    DateTime loanModifiedTime) : WebHooksEvent(serviceId, instanceId, siteId, eventId, userId, source, loanModifiedTime)
  {
    private static readonly string Topic = string.Format("{0}.{1}.{2}", (object) KafkaUtils.DeploymentProfile, (object) KafkaUtils.Region, (object) "loan.unlockall");

    public override string GetTopic(string messageType) => UnlockAllEvent.Topic;
  }
}
