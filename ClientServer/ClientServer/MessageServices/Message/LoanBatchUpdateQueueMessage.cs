// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.LoanBatchUpdateQueueMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message
{
  public class LoanBatchUpdateQueueMessage : QueueMessage
  {
    public string BatchId { get; set; }

    public IEnumerable<string> LoanIds { get; set; }

    public static LoanBatchUpdateQueueMessage CreateLoanBatchUpdateQueueMessage(
      string correlationId,
      string batchId,
      IEnumerable<string> loanIds)
    {
      LoanBatchUpdateQueueMessage updateQueueMessage = new LoanBatchUpdateQueueMessage();
      updateQueueMessage.BatchId = batchId;
      updateQueueMessage.LoanIds = loanIds;
      updateQueueMessage.PublishTime = DateTime.Now.ToString("O");
      updateQueueMessage.CorrelationId = correlationId;
      updateQueueMessage.Type = "urn:elli:service:ebs:loan:batchUpdate";
      return updateQueueMessage;
    }
  }
}
