// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Calendar.CSControlMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Calendar
{
  [Serializable]
  public class CSControlMessage : CSMessage
  {
    private string fromUser;
    private string toUser;
    private CSMessage.MessageType msgType;

    public CSControlMessage(
      string fromUser,
      string toUser,
      CSMessage.MessageType msgType,
      string msgText)
      : base(msgText)
    {
      this.fromUser = fromUser;
      this.toUser = toUser;
      this.msgType = msgType;
      if (this.fromUser == null || this.toUser == null)
        throw new Exception("CSControlMessage: fromUser and toUser cannot be null");
    }

    public string FromUser => this.fromUser;

    public string ToUser => this.toUser;

    public CSMessage.MessageType MsgType => this.msgType;
  }
}
