// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.LoanSaveMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message
{
  internal class LoanSaveMessage : QueueMessage
  {
    public string LoanVersionNumber { get; set; }

    public string LoanEventType { get; set; }

    public bool DataLakeEnabled { get; set; }

    public bool DataLakeUseGenericIngestEndPoint { get; set; }

    public string ClientId { get; set; }

    public string LoanFolder { get; set; }

    public string EventSequenceNumber { get; set; }

    public bool BatchApplied { get; set; }

    public string Source { get; set; }

    public string EventId { get; set; }

    public string ApplicationId { get; set; }

    public string AuditUserId { get; set; }

    public string MessageType { get; set; }

    public static LoanSaveMessage createLoanSaveMessage(
      string loanId,
      string loanVersionNumber,
      string loanEventType,
      string loanFileLocation,
      bool dataLakeEnabled,
      bool dataLakeUseGenericIngestEndPoint,
      string clientId,
      string loanFolder,
      string eventSequenceNumber,
      bool batchApplied,
      string auditUserId,
      string instanceId,
      string correlationId)
    {
      string str = Guid.NewGuid().ToString();
      LoanSaveMessage loanSaveMessage = new LoanSaveMessage();
      loanSaveMessage.LoanId = loanId;
      loanSaveMessage.LoanVersionNumber = loanVersionNumber;
      loanSaveMessage.LoanEventType = loanEventType;
      loanSaveMessage.LoanPath = loanFileLocation;
      loanSaveMessage.DataLakeEnabled = dataLakeEnabled;
      loanSaveMessage.DataLakeUseGenericIngestEndPoint = dataLakeUseGenericIngestEndPoint;
      loanSaveMessage.ClientId = clientId;
      loanSaveMessage.LoanFolder = loanFolder;
      loanSaveMessage.EventSequenceNumber = eventSequenceNumber;
      loanSaveMessage.BatchApplied = batchApplied;
      loanSaveMessage.EventId = str;
      loanSaveMessage.Source = string.Format("urn:Encompass:serviceId:{0}:siteId", (object) instanceId);
      loanSaveMessage.Type = "urn:elli:service:ebs:loan:save";
      loanSaveMessage.ApplicationId = "Encompass";
      loanSaveMessage.AuditUserId = auditUserId;
      loanSaveMessage.PublishTime = DateTime.Now.ToString("o");
      loanSaveMessage.MessageType = string.Format("urn:Encompass:serviceId:{0}", (object) str);
      loanSaveMessage.CorrelationId = correlationId;
      return loanSaveMessage;
    }
  }
}
