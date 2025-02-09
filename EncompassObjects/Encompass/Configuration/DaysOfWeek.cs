// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.DaysOfWeek
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>Rerpesents one or more days of the week.</summary>
  [Flags]
  [Guid("202E1D2A-537D-447f-83FF-75572D482FFE")]
  public enum DaysOfWeek
  {
    /// <summary>No Day specifed</summary>
    None = 0,
    /// <summary>Sunday</summary>
    Sunday = 1,
    /// <summary>Monday</summary>
    Monday = 2,
    /// <summary>Tuesday</summary>
    Tuesday = 4,
    /// <summary>Wednesday</summary>
    Wednesday = 8,
    /// <summary>Thursday</summary>
    Thursday = 16, // 0x00000010
    /// <summary>Friday</summary>
    Friday = 32, // 0x00000020
    /// <summary>Saturday</summary>
    Saturday = 64, // 0x00000040
    /// <summary>All days of the week</summary>
    All = Saturday | Friday | Thursday | Wednesday | Tuesday | Monday | Sunday, // 0x0000007F
    /// <summary>Weekdays</summary>
    Weekdays = Friday | Thursday | Wednesday | Tuesday | Monday, // 0x0000003E
    /// <summary>Weekends</summary>
    Weekends = Saturday | Sunday, // 0x00000041
  }
}
