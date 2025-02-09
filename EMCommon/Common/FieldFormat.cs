// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.FieldFormat
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [CLSCompliant(true)]
  public enum FieldFormat
  {
    UNDEFINED = -1, // 0xFFFFFFFF
    NONE = 0,
    STRING = 101, // 0x00000065
    YN = 102, // 0x00000066
    X = 103, // 0x00000067
    ZIPCODE = 104, // 0x00000068
    STATE = 105, // 0x00000069
    PHONE = 106, // 0x0000006A
    SSN = 107, // 0x0000006B
    RA_STRING = 108, // 0x0000006C
    RA_INTEGER = 109, // 0x0000006D
    RA_DECIMAL_2 = 110, // 0x0000006E
    RA_DECIMAL_3 = 111, // 0x0000006F
    INTEGER = 201, // 0x000000C9
    DECIMAL_1 = 202, // 0x000000CA
    DECIMAL_2 = 203, // 0x000000CB
    DECIMAL_3 = 204, // 0x000000CC
    DECIMAL_4 = 205, // 0x000000CD
    DECIMAL_6 = 207, // 0x000000CF
    DECIMAL_5 = 208, // 0x000000D0
    DECIMAL_7 = 209, // 0x000000D1
    DECIMAL = 210, // 0x000000D2
    DECIMAL_10 = 211, // 0x000000D3
    DECIMAL_9 = 212, // 0x000000D4
    DATE = 301, // 0x0000012D
    MONTHDAY = 303, // 0x0000012F
    DATETIME = 304, // 0x00000130
    DROPDOWNLIST = 998, // 0x000003E6
    DROPDOWN = 999, // 0x000003E7
    AUDIT = 1001, // 0x000003E9
    COMMENT = 2001, // 0x000007D1
  }
}
