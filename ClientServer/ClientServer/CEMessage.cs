// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CEMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class CEMessage : Message
  {
    public readonly CEMessageType MessageType;
    public readonly UserInfo Sender;

    public CEMessage(UserInfo sender, CEMessageType messageType)
      : base(CEMessage.getDefaultText(sender, messageType))
    {
      this.Sender = sender;
      this.MessageType = messageType;
    }

    public override Message Clone(SessionInfo info)
    {
      return (Message) new CEMessage(this.Sender, this.MessageType);
    }

    private static string getDefaultText(UserInfo sender, CEMessageType msgType)
    {
      switch (msgType)
      {
        case CEMessageType.UserOpenLoan:
          return string.Format("The following user has opened this loan:\r\n{0} {1}", (object) sender.FirstName, (object) sender.LastName);
        case CEMessageType.UserExitLoan:
          return string.Format("The following user has exited this loan:\r\n{0} {1}", (object) sender.FirstName, (object) sender.LastName);
        case CEMessageType.LoanFileSaved:
          return "";
        default:
          return (string) null;
      }
    }
  }
}
