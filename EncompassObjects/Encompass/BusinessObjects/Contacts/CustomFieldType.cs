// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.CustomFieldType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Defines the possible formats for a custom field associated with a contact.
  /// </summary>
  [Guid("7DF8D173-F829-41b3-A096-F981BA1EA07D")]
  public enum CustomFieldType
  {
    /// <summary>No specific field type.</summary>
    NONE = 0,
    /// <summary>A generic string field.</summary>
    STRING = 101, // 0x00000065
    /// <summary>A Yes/No field which can take on the value "Y" or "N".</summary>
    YN = 102, // 0x00000066
    /// <summary>A boolean field which can take on the value "X" or "".</summary>
    X = 103, // 0x00000067
    /// <summary>A string field formatted as a zip code (XXXXX-XXXX).</summary>
    ZIPCODE = 104, // 0x00000068
    /// <summary>A 2-character state abbreviation.</summary>
    STATE = 105, // 0x00000069
    /// <summary>A string field formatted as a phone number with optional extension (XXX-XXX-XXXX XXXX).</summary>
    PHONE = 106, // 0x0000006A
    /// <summary>A string field formatted as a Social Security Number (XXX-XX-XXXX).</summary>
    SSN = 107, // 0x0000006B
    /// <summary>An integer field.</summary>
    INTEGER = 201, // 0x000000C9
    /// <summary>An decimal field with one decimal place of precision.</summary>
    DECIMAL_1 = 202, // 0x000000CA
    /// <summary>An decimal field with two decimal places of precision.</summary>
    DECIMAL_2 = 203, // 0x000000CB
    /// <summary>An decimal field with three decimal places of precision.</summary>
    DECIMAL_3 = 204, // 0x000000CC
    /// <summary>An decimal field with four decimal places of precision.</summary>
    DECIMAL_4 = 205, // 0x000000CD
    /// <summary>An decimal field with four decimal places of precision.</summary>
    DECIMAL_6 = 207, // 0x000000CF
    /// <summary>An decimal field with five decimal places of precision.</summary>
    DECIMAL_5 = 208, // 0x000000D0
    /// <summary>An decimal field with seven decimal places of precision.</summary>
    DECIMAL_7 = 209, // 0x000000D1
    /// <summary>An decimal field without a specified precision.</summary>
    DECIMAL = 210, // 0x000000D2
    /// <summary>An decimal field with ten decimal places of precision.</summary>
    DECIMAL_10 = 211, // 0x000000D3
    /// <summary>An date value (no time included).</summary>
    DATE = 301, // 0x0000012D
    /// <summary>An date and time value.</summary>
    TIME = 302, // 0x0000012E
    /// <summary>An recurring date value (month and day only).</summary>
    MONTHDAY = 303, // 0x0000012F
    /// <summary>A field which provides a list of possible values from which the user must select.</summary>
    DROPDOWN = 998, // 0x000003E6
    /// <summary>A field which provides a list of possible values from which the user may select or enter a custom value.</summary>
    DROPDOWN_EDITABLE = 999, // 0x000003E7
  }
}
