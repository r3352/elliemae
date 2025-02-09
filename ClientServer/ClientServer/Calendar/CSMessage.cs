// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Calendar.CSMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Calendar
{
  [Serializable]
  public class CSMessage : Message
  {
    public CSMessage(string text)
      : base(text)
    {
    }

    protected CSMessage(CSMessage source, SessionInfo info)
      : base((Message) source, info)
    {
    }

    public enum MessageType
    {
      RequestReadOnlyCalendarAccess = 1,
      RequestPartialCalendarAccess = 2,
      RequestFullCalendarAccess = 3,
      AllowAddToList = 4,
      DenyAddToList = 5,
      DenyAck = 6,
      AcceptAck = 7,
      ModifyAccess = 8,
      DeleteAccess = 9,
      WithdrawAccess = 10, // 0x0000000A
      ModifyAck = 11, // 0x0000000B
    }

    public enum AccessLevel
    {
      ReadOnly,
      Partial,
      Full,
      Pending,
    }
  }
}
