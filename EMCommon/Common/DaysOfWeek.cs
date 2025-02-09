// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DaysOfWeek
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Flags]
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
