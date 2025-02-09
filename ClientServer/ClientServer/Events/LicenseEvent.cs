// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Events.LicenseEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Events
{
  [Serializable]
  public class LicenseEvent : ServerMonitorEvent
  {
    private string userId;
    private LicenseEventType eventType;

    public LicenseEvent(LicenseEventType eventType, string userId)
    {
      this.eventType = eventType;
      this.userId = userId;
    }

    public LicenseEventType EventType => this.eventType;

    public string UserID => this.userId;

    public override string ToString()
    {
      if (this.eventType == LicenseEventType.Granted)
        return "[License] License granted to user " + this.userId;
      if (this.eventType == LicenseEventType.Denied)
        return "[License] License denied to user " + this.userId;
      if (this.eventType == LicenseEventType.Released)
        return "[License] License released by user " + this.userId;
      return "[License] Unknown license event (" + (object) this.eventType + ") for user " + this.userId;
    }
  }
}
