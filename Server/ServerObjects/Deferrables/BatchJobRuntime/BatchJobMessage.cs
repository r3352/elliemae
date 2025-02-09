// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.BatchJobRuntime.BatchJobMessage
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Service.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.BatchJobRuntime
{
  [Serializable]
  public class BatchJobMessage(
    string applicationId,
    string serviceId,
    string instanceId,
    string siteId,
    string eventId,
    string userId,
    string auditUserId = null) : StandardMessage(applicationId, serviceId, instanceId, siteId, eventId, userId, auditUserId), ICloneable
  {
    public DateTime PublishTime { get; set; }

    public EncompassServerMode ServerMode { get; set; }

    public int BatchJobId { get; set; }

    public string BatchJobEntityId { get; set; }

    public int BatchJobItemId { get; set; }

    public string BatchJobItemEntityId { get; set; }

    public object Clone()
    {
      return (object) new BatchJobMessage(this.ApplicationId, this.ServiceId, this.InstanceId, this.SiteId, this.EventId, this.UserId)
      {
        ServerMode = this.ServerMode,
        PublishTime = this.PublishTime
      };
    }
  }
}
