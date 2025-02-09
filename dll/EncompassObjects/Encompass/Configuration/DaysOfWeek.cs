// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.DaysOfWeek
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  [Flags]
  [Guid("202E1D2A-537D-447f-83FF-75572D482FFE")]
  public enum DaysOfWeek
  {
    None = 0,
    Sunday = 1,
    Monday = 2,
    Tuesday = 4,
    Wednesday = 8,
    Thursday = 16, // 0x00000010
    Friday = 32, // 0x00000020
    Saturday = 64, // 0x00000040
    All = Saturday | Friday | Thursday | Wednesday | Tuesday | Monday | Sunday, // 0x0000007F
    Weekdays = Friday | Thursday | Wednesday | Tuesday | Monday, // 0x0000003E
    Weekends = Saturday | Sunday, // 0x00000041
  }
}
