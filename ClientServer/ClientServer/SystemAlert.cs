// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAlert
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class SystemAlert
  {
    private DateTime eventTime = DateTime.Now;
    private string description;
    private string source;
    private int eventId;
    private SystemAlertCategory category;

    public SystemAlert(
      int eventId,
      SystemAlertCategory category,
      string source,
      string description)
    {
      this.eventId = eventId;
      this.category = category;
      this.source = source;
      this.description = description;
    }

    public DateTime EventTime => this.eventTime;

    public int EventID => this.eventId;

    public SystemAlertCategory Category => this.category;

    public string Source => this.source;

    public string Description => this.description;
  }
}
