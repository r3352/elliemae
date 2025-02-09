// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.ReportingDbMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message
{
  public class ReportingDbMessage : QueueMessage
  {
    public string PriorLoanFileName { get; set; }

    public string ClientId { get; set; }

    [CLSCompliant(false)]
    public static ReportingDbMessage CreateReportingDbMessage(
      string correlationId,
      Enums.Type actionType,
      string loanId,
      string loanPath,
      string targetPriorLoanFileName = null,
      string clientId = null)
    {
      ReportingDbMessage reportingDbMessage = new ReportingDbMessage();
      reportingDbMessage.Type = EnumUtils.StringValueOf((Enum) actionType);
      reportingDbMessage.CorrelationId = correlationId;
      reportingDbMessage.LoanId = loanId;
      reportingDbMessage.LoanPath = loanPath;
      reportingDbMessage.PriorLoanFileName = targetPriorLoanFileName;
      reportingDbMessage.PublishTime = DateTime.Now.ToString("O");
      reportingDbMessage.ClientId = clientId;
      return reportingDbMessage;
    }
  }
}
