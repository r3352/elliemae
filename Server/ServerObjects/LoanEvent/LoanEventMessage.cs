// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.LoanEvent.LoanEventMessage
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Service.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.LoanEvent
{
  [Serializable]
  public class LoanEventMessage(
    string applicationId,
    string serviceId,
    string instanceId,
    string siteId,
    string eventId,
    string userId,
    string auditUserId = null) : StandardMessage(applicationId, serviceId, instanceId, siteId, eventId, userId, auditUserId)
  {
    public Guid LoanId { get; set; }

    public int LoanVersionNumber { get; set; }

    public LoanEventType LoanEventType { get; set; }

    public string LoanFileLocation { get; set; }

    public bool DataLakeEnabled { get; set; }

    public bool DataLakeUseGenericIngestEndPoint { get; set; }

    public bool WebHookEnabled { get; set; }

    public string ClientId { get; set; }

    public string LoanFolder { get; set; }

    public bool SkipWebhookRmqMessageProcessing { get; set; }

    public string EventSequenceNumber { get; set; }

    public bool BatchApplied { get; set; }
  }
}
