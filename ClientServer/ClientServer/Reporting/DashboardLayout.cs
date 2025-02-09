// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.DashboardLayout
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class DashboardLayout
  {
    private int id;
    private string name;
    private RectangleF[] blocks;
    private int minResolution;

    public DashboardLayout(int id, string name, RectangleF[] blocks, int minResolution)
    {
      this.id = id;
      this.name = name;
      this.blocks = blocks;
      this.minResolution = minResolution;
    }

    public DashboardLayout(string name, RectangleF[] blocks, int minResolution)
      : this(-1, name, blocks, minResolution)
    {
    }

    public int LayoutID => this.id;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public RectangleF[] Blocks => this.blocks;

    public int MinResolution
    {
      get => this.minResolution;
      set => this.minResolution = value;
    }
  }
}
