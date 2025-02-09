// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.CustomFieldType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  [Guid("7DF8D173-F829-41b3-A096-F981BA1EA07D")]
  public enum CustomFieldType
  {
    NONE = 0,
    STRING = 101, // 0x00000065
    YN = 102, // 0x00000066
    X = 103, // 0x00000067
    ZIPCODE = 104, // 0x00000068
    STATE = 105, // 0x00000069
    PHONE = 106, // 0x0000006A
    SSN = 107, // 0x0000006B
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
    DATE = 301, // 0x0000012D
    TIME = 302, // 0x0000012E
    MONTHDAY = 303, // 0x0000012F
    DROPDOWN = 998, // 0x000003E6
    DROPDOWN_EDITABLE = 999, // 0x000003E7
  }
}
