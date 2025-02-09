// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.DateFieldMatchPrecision
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Query
{
  /// <summary>
  /// Identifies the precision with which a DateTime value must match the value of the field
  /// to which the criterion applies.
  /// </summary>
  [Guid("25EFE28F-1B67-355A-9EA4-241DC54C2D04")]
  public enum DateFieldMatchPrecision
  {
    /// <summary>Date and time must match exactly.</summary>
    Exact,
    /// <summary>Only the date portion must match exactly (i.e. time is ignored).</summary>
    Day,
    /// <summary>Only the month and year must match.</summary>
    Month,
    /// <summary>Only the year must match.</summary>
    Year,
    /// <summary>Only the day and month must match. This is useful for querying fields that
    /// represent recurring dates such as birthdays or anniversaries.</summary>
    Recurring,
  }
}
