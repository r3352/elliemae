// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.WidgetSettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class WidgetSettings
  {
    public const int SettingsCount = 6;
    private int layoutBlockId;
    private string name;
    private string[] settings = new string[6];

    public WidgetSettings(int layoutBlockId, string name, string[] settings)
    {
      this.layoutBlockId = layoutBlockId;
      this.name = name;
      Array.Copy((Array) settings, 0, (Array) this.settings, 0, 6);
    }

    public string Name => this.name;

    public int LayoutBlockID => this.layoutBlockId;

    public string[] Settings => this.settings;
  }
}
