// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.StringFieldMatchType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Query
{
  /// <summary>
  /// Identifies the type of match to performed between a string criterion and the field values.
  /// </summary>
  [Guid("ABDEF381-41F5-3396-B1FD-C2EAA482232D")]
  public enum StringFieldMatchType
  {
    /// <summary>An exact match (including case) is required.</summary>
    Exact,
    /// <summary>An exact match (excluding case) is required.</summary>
    CaseInsensitive,
    /// <summary>The field value must start with the specified substring (case insensitive).</summary>
    StartsWith,
    /// <summary>The field value must contain the specified substring (case insensitive).</summary>
    Contains,
  }
}
