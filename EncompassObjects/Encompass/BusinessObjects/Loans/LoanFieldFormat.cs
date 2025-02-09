// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanFieldFormat
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// A enumeration that defines the possible field formats for a loan field.
  /// </summary>
  [Guid("F1C71EA2-877E-3714-8318-FC9EEC80BB89")]
  public enum LoanFieldFormat
  {
    /// <summary>Unformatted</summary>
    NONE = 0,
    /// <summary>Generic string format (i.e. unformatted)</summary>
    STRING = 101, // 0x00000065
    /// <summary>Yes/No boolean value</summary>
    YN = 102, // 0x00000066
    /// <summary>X/(blank) boolean value</summary>
    X = 103, // 0x00000067
    /// <summary>Zip code (XXXXX-XXXX)</summary>
    ZIPCODE = 104, // 0x00000068
    /// <summary>Two-character state value</summary>
    STATE = 105, // 0x00000069
    /// <summary>Phone number format</summary>
    PHONE = 106, // 0x0000006A
    /// <summary>Social Security number format (XXX-XX-XXXX)</summary>
    SSN = 107, // 0x0000006B
    /// <summary>Tracking number format</summary>
    TRACKING = 108, // 0x0000006C
    /// <summary>Integer value format (X,XXX)</summary>
    INTEGER = 201, // 0x000000C9
    /// <summary>Decimal value format with 1 decimal place (X,XXX.0)</summary>
    DECIMAL_1 = 202, // 0x000000CA
    /// <summary>Decimal value format with 2 decimal places (X,XXX.00)</summary>
    DECIMAL_2 = 203, // 0x000000CB
    /// <summary>Decimal value format with 3 decimal places (X,XXX.000)</summary>
    DECIMAL_3 = 204, // 0x000000CC
    /// <summary>Decimal value format with 4 decimal places (X,XXX.0000)</summary>
    DECIMAL_4 = 205, // 0x000000CD
    /// <summary>Decimal value format with 6 decimal places (X,XXX.000000)</summary>
    DECIMAL_6 = 207, // 0x000000CF
    /// <summary>Decimal value format with 5 decimal places (X,XXX.000000)</summary>
    DECIMAL_5 = 208, // 0x000000D0
    /// <summary>Decimal value format with 7 decimal places (X,XXX.0000000)</summary>
    DECIMAL_7 = 209, // 0x000000D1
    /// <summary>Unformatted decimal value format</summary>
    DECIMAL = 210, // 0x000000D2
    /// <summary>Decimal value format with 10 decimal places (X,XXX.0000000000)</summary>
    DECIMAL_10 = 211, // 0x000000D3
    /// <summary>Date value (no time)</summary>
    DATE = 301, // 0x0000012D
    /// <summary>Time value</summary>
    TIME = 302, // 0x0000012E
    /// <summary>Month and day value only (MM/dd)</summary>
    MONTHDAY = 303, // 0x0000012F
    /// <summary>Text value which must be selected selected from a list of predefined values.</summary>
    DROPDOWNLIST = 998, // 0x000003E6
    /// <summary>Text value which is selected from a dropdown or entered manually.</summary>
    DROPDOWN = 999, // 0x000003E7
    /// <summary>An audit field for tracing the changes to another field.</summary>
    AUDIT = 1001, // 0x000003E9
  }
}
