// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.LoanBillingMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Classes;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message
{
  internal class LoanBillingMessage : QueueMessage
  {
    public TransactionLog TransactionLog { get; set; }

    public string LoanEventType { get; set; }

    [CLSCompliant(false)]
    public static LoanBillingMessage CreateLoanBillingMessage(
      string correlationId,
      string loanEventType,
      TransactionLog transactionLog)
    {
      LoanBillingMessage loanBillingMessage = new LoanBillingMessage();
      loanBillingMessage.CorrelationId = correlationId;
      loanBillingMessage.Type = "urn:elli:service:ebs:loan:billing";
      loanBillingMessage.PublishTime = DateTime.Now.ToString("O");
      loanBillingMessage.LoanEventType = loanEventType;
      loanBillingMessage.TransactionLog = transactionLog;
      return loanBillingMessage;
    }
  }
}
