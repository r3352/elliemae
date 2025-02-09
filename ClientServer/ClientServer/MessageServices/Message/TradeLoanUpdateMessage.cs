// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.TradeLoanUpdateMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message
{
  internal class TradeLoanUpdateMessage : QueueMessage
  {
    public string instanceId { get; set; }

    public string batchJobId { get; set; }

    public string batchJobStatus { get; set; }

    public BatchJobResult batchJobResult { get; set; }

    public string tradeId { get; set; }

    public string tradeStatus { get; set; }

    public string sessionId { get; set; }

    public static TradeLoanUpdateMessage CreateTradeLoanUpdateMessage(
      string instanceId,
      string batchJobId,
      string batchJobStatus,
      BatchJobResult batchJobResult,
      string tradeId,
      string tradeStatus,
      string sessionId)
    {
      TradeLoanUpdateMessage loanUpdateMessage = new TradeLoanUpdateMessage();
      loanUpdateMessage.EnvelopeVersion = "2.0.0";
      loanUpdateMessage.PayloadVersion = "2.0.0";
      loanUpdateMessage.PublishTime = DateTime.Now.ToString("O");
      loanUpdateMessage.Type = "encompass:tradeloanupdate";
      loanUpdateMessage.instanceId = instanceId;
      loanUpdateMessage.batchJobId = batchJobId;
      loanUpdateMessage.batchJobStatus = batchJobStatus;
      loanUpdateMessage.batchJobResult = batchJobResult;
      loanUpdateMessage.tradeId = tradeId;
      loanUpdateMessage.tradeStatus = tradeStatus;
      loanUpdateMessage.sessionId = sessionId;
      return loanUpdateMessage;
    }
  }
}
