// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanEventQueueMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class LoanEventQueueMessage : QueueMessage
  {
    [CLSCompliant(false)]
    public static LoanEventQueueMessage CreateLoanEventKafkaMessage(
      string correlationId,
      Enums.Type actionType,
      string loanId,
      string loanPath)
    {
      LoanEventQueueMessage eventKafkaMessage = new LoanEventQueueMessage();
      eventKafkaMessage.Type = EnumUtils.StringValueOf((Enum) actionType);
      eventKafkaMessage.CorrelationId = correlationId;
      eventKafkaMessage.LoanPath = loanPath;
      eventKafkaMessage.LoanId = loanId;
      eventKafkaMessage.PublishTime = DateTime.Now.ToString("O");
      return eventKafkaMessage;
    }
  }
}
