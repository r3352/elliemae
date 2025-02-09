// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DocumentReceivedMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.MessageServices.Message.Common;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Document;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class DocumentReceivedMessage : QueueMessage
  {
    public string UserId { get; set; }

    public Resource Resource { get; set; }

    public Client Client { get; set; }

    public DocumentReceivedEvent Event { get; set; }

    public static DocumentReceivedMessage CreateDocumentReceivedMessage(
      string loanId,
      string correlationId,
      string userId,
      string clientId,
      string instanceId,
      DocumentReceivedEvent documentReceived,
      bool isSourceEncompass)
    {
      string str = "/encompass/v3/loans/" + loanId + "/documents/" + documentReceived.documentReceived.Id + "/attachments";
      DocumentReceivedMessage documentReceivedMessage = new DocumentReceivedMessage();
      documentReceivedMessage.EnvelopeVersion = "2.0.0";
      documentReceivedMessage.PayloadVersion = "2.0.0";
      documentReceivedMessage.CorrelationId = correlationId;
      documentReceivedMessage.UserId = userId;
      documentReceivedMessage.Type = string.Format("urn:elli:service:{0}:loan:document:receive", isSourceEncompass ? (object) "encompass" : (object) "ebs").ToLower();
      documentReceivedMessage.PublishTime = DateTime.Now.ToString("O");
      documentReceivedMessage.Resource = new Resource()
      {
        Type = "Loan",
        Id = loanId,
        Reference = str
      };
      documentReceivedMessage.Client = new Client()
      {
        ClientId = clientId,
        InstanceId = instanceId
      };
      documentReceivedMessage.Event = documentReceived;
      return documentReceivedMessage;
    }
  }
}
