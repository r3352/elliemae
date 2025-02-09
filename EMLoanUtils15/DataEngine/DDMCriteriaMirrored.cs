// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMCriteriaMirrored
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public enum DDMCriteriaMirrored
  {
    none = -1, // 0xFFFFFFFF
    Equals = 0,
    LessThan = 1,
    LessThanOrEqual = 2,
    GreaterThan = 3,
    GreaterThanOrEqual = 4,
    NotEqual = 5,
    Range = 6,
    CheckBox = 7,
    SSN_SpecificValue = 8,
    SSN_ListofValues = 9,
    strEquals = 10, // 0x0000000A
    strNotEqual = 11, // 0x0000000B
    strContains = 12, // 0x0000000C
    strNotContains = 13, // 0x0000000D
    strBegins = 14, // 0x0000000E
    strEnds = 15, // 0x0000000F
    OP_SpecificValue = 16, // 0x00000010
    OP_AdvancedCoding = 17, // 0x00000011
    ListOfValues = 18, // 0x00000012
    zip_SpecificValue = 19, // 0x00000013
    zip_FindByZip = 20, // 0x00000014
    county_SpecificValue = 21, // 0x00000015
    county_FindByCounty = 22, // 0x00000016
    st_ListOfValues = 23, // 0x00000017
    IgnoreValueInLoanFile = 24, // 0x00000018
    NoValueInLoanFile = 25, // 0x00000019
    UseCalculatedValue = 26, // 0x0000001A
    OP_ClearValueInLoanFile = 27, // 0x0000001B
  }
}
