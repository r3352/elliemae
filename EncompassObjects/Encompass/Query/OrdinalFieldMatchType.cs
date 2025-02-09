// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.OrdinalFieldMatchType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Query
{
  /// <summary>
  /// Identifies the desired ordinal relationship between a numeric or date criterion and
  /// the field values against which the criterion applies.
  /// </summary>
  [Guid("45900E47-56CD-3A8B-BB58-700422AAB186")]
  public enum OrdinalFieldMatchType
  {
    /// <summary>An exact match is required.</summary>
    Equals,
    /// <summary>The field value must not match the specified criterion value.</summary>
    NotEquals,
    /// <summary>The field value must be greater than the specified criterion value.</summary>
    GreaterThan,
    /// <summary>The field value must be greater than or equal to the specified criterion value.</summary>
    GreaterThanOrEquals,
    /// <summary>The field value must be less than the specified criterion value.</summary>
    LessThan,
    /// <summary>The field value must be less than or equal to the specified criterion value.</summary>
    LessThanOrEquals,
  }
}
