// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EPassMessageNotification
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class EPassMessageNotification : UserNotification
  {
    private EPassMessageInfo msg;

    public EPassMessageNotification(string userId, EPassMessageInfo msg)
      : base(userId, msg.Timestamp)
    {
      this.msg = msg;
    }

    public EPassMessageInfo Message => this.msg;

    public override string ToString()
    {
      return "ePASS Msg -> Recipient = " + this.UserID + ", MsgID = " + this.msg.MessageID + ", Type = " + this.msg.MessageType + ", Source = " + this.msg.Source + ", Loan = " + this.msg.LoanGuid + ", Desc = " + this.msg.Description;
    }
  }
}
