// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Kafka.Message.AuditTrailMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using System;
using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Kafka.Message
{
  public class AuditTrailMessage : QueueMessage
  {
    public string PriorLoanFileName { get; set; }

    public string AfterLoanFileName { get; set; }

    public string AuditModifiedTime { get; set; }

    public string AuditCurrentTime { get; set; }

    public string XdbModifiedTime { get; set; }

    public string AuditUserId { get; set; }

    public string ClientId { get; set; }

    [CLSCompliant(false)]
    public static AuditTrailMessage CreateAuditTrailMessage(
      string correlationId,
      Enums.Type actionType,
      string loanId,
      string loanPath,
      string targetPriorLoanFileName,
      string targetAfterLoanFileName,
      DateTime auditModifiedTime,
      DateTime auditCurrentTime,
      DateTime xdbModifiedTime,
      string auditUserId,
      string clientId)
    {
      AuditTrailMessage auditTrailMessage = new AuditTrailMessage();
      auditTrailMessage.Type = EnumUtils.StringValueOf((Enum) actionType);
      auditTrailMessage.CorrelationId = correlationId;
      auditTrailMessage.LoanId = loanId;
      auditTrailMessage.LoanPath = loanPath;
      auditTrailMessage.AuditCurrentTime = auditCurrentTime.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      auditTrailMessage.AuditModifiedTime = auditModifiedTime.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      auditTrailMessage.XdbModifiedTime = xdbModifiedTime.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      auditTrailMessage.PriorLoanFileName = targetPriorLoanFileName;
      auditTrailMessage.AfterLoanFileName = targetAfterLoanFileName;
      auditTrailMessage.PublishTime = DateTime.Now.ToString("O");
      auditTrailMessage.AuditUserId = auditUserId;
      auditTrailMessage.ClientId = clientId;
      auditTrailMessage.EnvelopeVersion = "1.0.2";
      return auditTrailMessage;
    }
  }
}
