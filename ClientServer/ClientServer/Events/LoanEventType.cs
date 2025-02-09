// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Events.LoanEventType
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.Events
{
  public enum LoanEventType
  {
    Opened = 1,
    Locked = 2,
    Unlocked = 3,
    Saved = 4,
    Imported = 5,
    Exported = 6,
    PermissionsChanged = 7,
    Completed = 8,
    Created = 9,
    Moved = 10, // 0x0000000A
    Deleted = 11, // 0x0000000B
  }
}
