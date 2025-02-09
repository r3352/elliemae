// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Kafka.Message.EmailNotificationMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.Common.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Kafka.Message
{
  public class EmailNotificationMessage : QueueMessage
  {
    public List<EmailNotificationInfo> EmailNotificationMessages { get; set; }

    [CLSCompliant(false)]
    public static EmailNotificationMessage CreateEmailNotificationMessage(
      string correlationId,
      Enums.Type actionType,
      string loanId,
      string loanPath,
      List<EmailNotificationInfo> emailNotificationMessages)
    {
      EmailNotificationMessage notificationMessage = new EmailNotificationMessage();
      notificationMessage.Type = EnumUtils.StringValueOf((Enum) actionType);
      notificationMessage.CorrelationId = correlationId;
      notificationMessage.LoanId = loanId;
      notificationMessage.LoanPath = loanPath;
      notificationMessage.EmailNotificationMessages = emailNotificationMessages;
      notificationMessage.PublishTime = DateTime.Now.ToString("O");
      return notificationMessage;
    }
  }
}
