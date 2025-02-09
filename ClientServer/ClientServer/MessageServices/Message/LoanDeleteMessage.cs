// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.LoanDeleteMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message
{
  internal class LoanDeleteMessage : QueueMessage
  {
    public string LoanEventType { get; set; }

    public string ClientId { get; set; }

    public string EventSequenceNumber { get; set; }

    public string Source { get; set; }

    public string EventId { get; set; }

    public string ApplicationId { get; set; }

    public string AuditUserId { get; set; }

    public static LoanDeleteMessage createLoanDeleteMessage(
      string loanId,
      string loanEventType,
      string clientId,
      string eventSequenceNumber,
      string auditUserId,
      string instanceId)
    {
      string str = Guid.NewGuid().ToString();
      LoanDeleteMessage loanDeleteMessage = new LoanDeleteMessage();
      loanDeleteMessage.LoanId = loanId;
      loanDeleteMessage.LoanEventType = loanEventType;
      loanDeleteMessage.ClientId = clientId;
      loanDeleteMessage.EventSequenceNumber = eventSequenceNumber;
      loanDeleteMessage.EventId = str;
      loanDeleteMessage.Source = string.Format("urn:Encompass:serviceId:{0}:siteId", (object) instanceId);
      loanDeleteMessage.Type = "urn:elli:service:ebs:loan:delete";
      loanDeleteMessage.ApplicationId = "Encompass";
      loanDeleteMessage.AuditUserId = auditUserId;
      loanDeleteMessage.PublishTime = DateTime.Now.ToString("O");
      return loanDeleteMessage;
    }
  }
}
