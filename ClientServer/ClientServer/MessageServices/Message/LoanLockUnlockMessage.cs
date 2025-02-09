// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.LoanLockUnlockMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.MessageServices.Message.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message
{
  public class LoanLockUnlockMessage : QueueMessage
  {
    public string EventType { get; set; }

    public string UserId { get; set; }

    public Resource Resource { get; set; }

    public Client Client { get; set; }

    internal static QueueMessage CreateLockUnlockMessage(
      string loanId,
      string correlationId,
      string userId,
      string clientId,
      string instanceId,
      bool isSourceEncompass,
      string type)
    {
      string str = "/encompass/v3/Loans/" + loanId + "/resourceLocks";
      LoanLockUnlockMessage lockUnlockMessage = new LoanLockUnlockMessage();
      lockUnlockMessage.EnvelopeVersion = "2.0.0";
      lockUnlockMessage.PayloadVersion = "2.0.0";
      lockUnlockMessage.CorrelationId = correlationId;
      lockUnlockMessage.UserId = userId;
      lockUnlockMessage.Type = string.Format("urn:elli:service:{0}:loan:{1}", isSourceEncompass ? (object) "encompass" : (object) "ebs", (object) type).ToLower();
      lockUnlockMessage.PublishTime = DateTime.Now.ToString("O");
      lockUnlockMessage.EventType = type.ToLower();
      lockUnlockMessage.Resource = new Resource()
      {
        Type = "Loan",
        Id = loanId.TrimStart('{').TrimEnd('}'),
        Reference = str
      };
      lockUnlockMessage.Client = new Client()
      {
        ClientId = clientId,
        InstanceId = instanceId
      };
      return (QueueMessage) lockUnlockMessage;
    }
  }
}
