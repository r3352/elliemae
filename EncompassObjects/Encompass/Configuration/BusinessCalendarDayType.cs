// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.BusinessCalendarDayType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>
  /// Enumerates the different types of days in a Business Calendar
  /// </summary>
  [Guid("CAE8697A-53DF-445e-B450-A8A53755277A")]
  public enum BusinessCalendarDayType
  {
    /// <summary>No day type specified</summary>
    None,
    /// <summary>The day represents a day the business is open</summary>
    BusinessDay,
    /// <summary>The day represents a recurring weekend non-work day</summary>
    Weekend,
    /// <summary>No day represents a non-recurring non-work day</summary>
    Holiday,
  }
}
