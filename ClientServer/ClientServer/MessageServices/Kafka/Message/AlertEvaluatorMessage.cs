// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Kafka.Message.AlertEvaluatorMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Kafka.Message
{
  public class AlertEvaluatorMessage : QueueMessage
  {
    public string PriorLoanFileName { get; set; }

    public string AfterLoanFileName { get; set; }

    [CLSCompliant(false)]
    public static AlertEvaluatorMessage CreateAlertMessage(
      string correlationId,
      Enums.Type actionType,
      string loanId,
      string loanPath,
      string targetPriorLoanFileName,
      string targetAfterLoanFileName)
    {
      AlertEvaluatorMessage alertMessage = new AlertEvaluatorMessage();
      alertMessage.Type = EnumUtils.StringValueOf((Enum) actionType);
      alertMessage.CorrelationId = correlationId;
      alertMessage.AfterLoanFileName = targetAfterLoanFileName;
      alertMessage.PriorLoanFileName = targetPriorLoanFileName;
      alertMessage.LoanPath = loanPath;
      alertMessage.LoanId = loanId;
      alertMessage.PublishTime = DateTime.Now.ToString("O");
      return alertMessage;
    }
  }
}
