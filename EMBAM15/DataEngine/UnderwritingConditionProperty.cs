// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.UnderwritingConditionProperty
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public enum UnderwritingConditionProperty
  {
    None = 0,
    Title = 1,
    Category = 2,
    Description = 4,
    AddedBy = 5,
    DateAdded = 6,
    IsCleared = 7,
    ClearedBy = 8,
    DateCleared = 9,
    IsReceived = 10, // 0x0000000A
    ReceivedBy = 11, // 0x0000000B
    DateReceived = 12, // 0x0000000C
    IsReviewed = 13, // 0x0000000D
    ReviewedBy = 14, // 0x0000000E
    DateReviewed = 15, // 0x0000000F
    IsWaived = 16, // 0x00000010
    WaivedBy = 17, // 0x00000011
    DateWaived = 18, // 0x00000012
    IsExpired = 19, // 0x00000013
    ExpirationDate = 20, // 0x00000014
    Status = 21, // 0x00000015
    PriorTo = 22, // 0x00000016
    Comments = 23, // 0x00000017
    AllowToClear = 24, // 0x00000018
    IsInternal = 25, // 0x00000019
  }
}
