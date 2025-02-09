// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.LoanSyncMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.MessageServices.Message.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message
{
  public class LoanSyncMessage : QueueMessage
  {
    public string SessionId { get; set; }

    public Client Client { get; set; }

    public string UserId { get; private set; }

    [CLSCompliant(false)]
    public static LoanSyncMessage CreateLoanSyncMessage(
      string correlationId,
      string lockedBy,
      string sessionId,
      string loanId,
      string instanceId)
    {
      LoanSyncMessage loanSyncMessage = new LoanSyncMessage();
      loanSyncMessage.CorrelationId = correlationId;
      loanSyncMessage.UserId = lockedBy;
      loanSyncMessage.SessionId = sessionId;
      loanSyncMessage.LoanId = loanId;
      loanSyncMessage.Type = "urn:elli:service:ebs:loan:smartclientsync";
      loanSyncMessage.PublishTime = DateTime.Now.ToString("O");
      loanSyncMessage.Client = new Client()
      {
        InstanceId = instanceId
      };
      return loanSyncMessage;
    }
  }
}
