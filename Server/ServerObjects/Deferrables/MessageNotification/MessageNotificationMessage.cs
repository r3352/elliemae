// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.MessageNotification.MessageNotificationMessage
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Service.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.MessageNotification
{
  [Serializable]
  public class MessageNotificationMessage(
    string applicationId,
    string serviceId,
    string instanceId,
    string siteId,
    string eventId,
    string userId,
    string auditUserId = null) : StandardMessage(applicationId, serviceId, instanceId, siteId, eventId, userId, auditUserId), ICloneable
  {
    public List<MessageNotificationMessageData> Messages = new List<MessageNotificationMessageData>();

    public static MessageNotificationMessage CreateBlank()
    {
      return new MessageNotificationMessage("", "", "", "", "", "")
      {
        Messages = new List<MessageNotificationMessageData>()
      };
    }

    public object Clone() => throw new NotImplementedException();
  }
}
