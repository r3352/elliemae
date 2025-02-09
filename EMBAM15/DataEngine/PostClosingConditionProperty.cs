// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PostClosingConditionProperty
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public enum PostClosingConditionProperty
  {
    None = 0,
    Title = 1,
    Source = 2,
    Description = 4,
    AddedBy = 5,
    DateAdded = 6,
    IsCleared = 7,
    ClearedBy = 8,
    DateCleared = 9,
    IsReceived = 10, // 0x0000000A
    ReceivedBy = 11, // 0x0000000B
    DateReceived = 12, // 0x0000000C
    IsRequested = 13, // 0x0000000D
    RequestedBy = 14, // 0x0000000E
    DateRequested = 15, // 0x0000000F
    IsRerequested = 16, // 0x00000010
    RerequestedBy = 17, // 0x00000011
    DateRerequested = 18, // 0x00000012
    IsSent = 19, // 0x00000013
    SentBy = 20, // 0x00000014
    DateSent = 21, // 0x00000015
    Recipient = 22, // 0x00000016
    DateExpected = 23, // 0x00000017
    Status = 24, // 0x00000018
    Comments = 25, // 0x00000019
    RequestedFrom = 26, // 0x0000001A
    PrintInternally = 27, // 0x0000001B
    PrintExternally = 28, // 0x0000001C
  }
}
