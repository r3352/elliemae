// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.DataConversion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Query
{
  /// <summary>
  /// Defines how field values should be converted from one type to another.
  /// </summary>
  [Guid("10FC41AC-1E85-4fb5-B792-17B6BD45E5DE")]
  public enum DataConversion
  {
    /// <summary>No conversion is applied -- the field's native type is used.</summary>
    None,
    /// <summary>The field's value is converted to its numeric equivalent.</summary>
    Numeric,
    /// <summary>The field's value is converted to a date/time value.</summary>
    DateTime,
    /// <summary>The field's value is converted to a string.</summary>
    Text,
  }
}
