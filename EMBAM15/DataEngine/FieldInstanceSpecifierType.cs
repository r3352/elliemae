// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldInstanceSpecifierType
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public enum FieldInstanceSpecifierType
  {
    None = 0,
    Index = 1,
    Role = 2,
    Milestone = 3,
    Document = 4,
    UnderwritingCondition = 5,
    PostClosingCondition = 6,
    MilestoneTask = 8,
    CustomAlert = 10, // 0x0000000A
    PreliminaryCondition = 11, // 0x0000000B
    MilestoneTemplate = 12, // 0x0000000C
    EnhancedCondition = 13, // 0x0000000D
    EnhancedConditionSingleAttribute = 14, // 0x0000000E
    EnhancedConditionByOption = 15, // 0x0000000F
  }
}
