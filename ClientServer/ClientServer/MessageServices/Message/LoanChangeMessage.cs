// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.LoanChangeMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.MessageServices.Message.Common;
using EllieMae.EMLite.ClientServer.MessageServices.Message.HOI;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message
{
  internal class LoanChangeMessage : QueueMessage
  {
    public Resource Resource { get; set; }

    public Client Client { get; set; }

    public string UserId { get; set; }

    public string EventType { get; set; }

    public HoiEligibleEvent Event { get; set; }

    public static LoanChangeMessage CreateLoanChangeMessage(
      string loanId,
      string instanceId,
      string clientId,
      string userId,
      bool isSourceEncompass)
    {
      LoanChangeMessage loanChangeMessage = new LoanChangeMessage();
      loanChangeMessage.EnvelopeVersion = "2.0.0";
      loanChangeMessage.PayloadVersion = "2.0.0";
      loanChangeMessage.PublishTime = DateTime.Now.ToString("O");
      loanChangeMessage.UserId = userId;
      loanChangeMessage.EventType = KafkaEventType.Consumer.ToString().ToLower();
      loanChangeMessage.Type = string.Format("urn:elli:service:{0}:loan:consumer:hoi:eligibility", (object) (SourceEnum) (isSourceEncompass ? 0 : 1)).ToLower();
      loanChangeMessage.Resource = new Resource()
      {
        Type = "Loan",
        Id = loanId,
        Reference = string.Format("/encompass/v3/loans/{0}", (object) loanId)
      };
      loanChangeMessage.Client = new Client()
      {
        ClientId = clientId,
        InstanceId = instanceId
      };
      loanChangeMessage.Event = new HoiEligibleEvent()
      {
        HOIEligible = new HoiEligible()
        {
          Eligibility = true
        }
      };
      return loanChangeMessage;
    }
  }
}
