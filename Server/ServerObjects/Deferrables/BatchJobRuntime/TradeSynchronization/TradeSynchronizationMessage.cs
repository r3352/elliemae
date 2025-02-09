// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.BatchJobRuntime.TradeSynchronization.TradeSynchronizationMessage
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.BatchJobRuntime.TradeSynchronization
{
  [Serializable]
  public class TradeSynchronizationMessage(
    string applicationId,
    string serviceId,
    string instanceId,
    string siteId,
    string eventId,
    string userId) : BatchJobMessage(applicationId, serviceId, instanceId, siteId, eventId, userId)
  {
    public string BatchJobEntityType { get; set; }

    public string BatchJobItemAction { get; set; }

    public new object Clone()
    {
      TradeSynchronizationMessage synchronizationMessage = new TradeSynchronizationMessage(this.ApplicationId, this.ServiceId, this.InstanceId, this.SiteId, this.EventId, this.UserId);
      synchronizationMessage.ServerMode = this.ServerMode;
      synchronizationMessage.PublishTime = this.PublishTime;
      synchronizationMessage.BatchJobId = this.BatchJobId;
      synchronizationMessage.BatchJobEntityId = this.BatchJobEntityId;
      synchronizationMessage.BatchJobEntityType = this.BatchJobEntityType;
      synchronizationMessage.BatchJobItemId = this.BatchJobItemId;
      synchronizationMessage.BatchJobItemEntityId = this.BatchJobItemEntityId;
      synchronizationMessage.BatchJobItemAction = this.BatchJobItemAction;
      return (object) synchronizationMessage;
    }

    public static TradeSynchronizationMessage CreateBlank(
      string batchJobItemEntityId,
      string batchJobItemAction)
    {
      TradeSynchronizationMessage blank = new TradeSynchronizationMessage("", "", "", "", "", "");
      blank.BatchJobItemEntityId = batchJobItemEntityId;
      blank.BatchJobItemAction = batchJobItemAction;
      return blank;
    }
  }
}
