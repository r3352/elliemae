// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.BusinessCalendarType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>
  /// Provides a list of the different business calendars provided by Encompass.
  /// </summary>
  [Guid("73291672-7E40-4ef6-932A-6F653516F0D0")]
  public enum BusinessCalendarType
  {
    /// <summary>No calendar type specified</summary>
    None = -1, // 0xFFFFFFFF
    /// <summary>Represents the business calendar of the US Postal Service</summary>
    Postal = 0,
    /// <summary>Represents your company's business calendar</summary>
    Company = 1,
    /// <summary>Represents the Reg-Z business calendar</summary>
    RegZ = 4,
  }
}
