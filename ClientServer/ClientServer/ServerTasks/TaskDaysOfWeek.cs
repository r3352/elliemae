// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ServerTasks.TaskDaysOfWeek
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ServerTasks
{
  [Flags]
  public enum TaskDaysOfWeek
  {
    None = 0,
    Sunday = 1,
    Monday = 2,
    Tuesday = 4,
    Wednesday = 8,
    Thursday = 16, // 0x00000010
    Friday = 32, // 0x00000020
    Saturday = 64, // 0x00000040
    Weekdays = Friday | Thursday | Wednesday | Tuesday | Monday, // 0x0000003E
    All = Weekdays | Saturday | Sunday, // 0x0000007F
  }
}
