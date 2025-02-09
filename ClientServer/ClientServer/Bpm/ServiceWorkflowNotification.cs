// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Bpm.ServiceWorkflowNotification
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Bpm
{
  public class ServiceWorkflowNotification
  {
    public Guid NotificationID { get; set; }

    public int RuleID { get; set; }

    public WorkflowNotificationType NotificationType { get; set; }

    public string Subject { get; set; }

    public string Text { get; set; }

    public string LastModifiedByUserId { get; set; }

    public DateTime LastModified { get; set; }

    public List<WorkflowNotificationRecipient> Recipients { get; set; }
  }
}
