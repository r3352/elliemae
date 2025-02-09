// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IMChatMessageException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class IMChatMessageException : ApplicationException
  {
    private static readonly string _cause = nameof (cause);
    private static readonly string _receiverUserID = "userID";
    private static readonly string _receiverSessionID = nameof (receiverSessionID);
    private IMChatMessageExceptionCause cause;
    private string receiverUserID;
    private string receiverSessionID;

    public IMChatMessageException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.receiverUserID = info.GetString(IMChatMessageException._receiverUserID);
      this.receiverSessionID = info.GetString(IMChatMessageException._receiverSessionID);
      this.cause = (IMChatMessageExceptionCause) info.GetValue(IMChatMessageException._cause, typeof (IMChatMessageExceptionCause));
    }

    public IMChatMessageException(
      IMChatMessageExceptionCause cause,
      string recvSessionID,
      string recvUserID,
      string message)
      : base(message)
    {
      this.cause = cause;
      this.receiverSessionID = recvSessionID;
      this.receiverUserID = recvUserID;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue(IMChatMessageException._cause, (object) this.cause, typeof (IMChatMessageExceptionCause));
      info.AddValue(IMChatMessageException._receiverUserID, (object) this.receiverUserID, typeof (string));
      info.AddValue(IMChatMessageException._receiverSessionID, (object) this.receiverSessionID, typeof (string));
    }

    public IMChatMessageExceptionCause Cause => this.cause;

    public string ReceiverSessionID => this.receiverSessionID;

    public string ReceiverUserID => this.receiverUserID;
  }
}
