// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientSession.Dashboard.DashboardLayout
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.Dashboard;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.ClientSession.Dashboard
{
  [Serializable]
  public class DashboardLayout : BusinessBase, IDisposable
  {
    private DashboardLayoutInfo layoutInfo;

    public int LayoutId => this.layoutInfo.LayoutId;

    public string LayoutName => this.layoutInfo.LayoutName;

    public RectangleF[] LayoutBlocks => this.layoutInfo.LayoutBlocks;

    public override string ToString()
    {
      return string.Format("DashboardLayout[{0}]", (object) this.LayoutId);
    }

    public bool Equals(DashboardLayout obj) => this.LayoutId.Equals(obj.LayoutId);

    public new static bool Equals(object objA, object objB)
    {
      return objA is DashboardLayout && objB is DashboardLayout && ((DashboardLayout) objA).Equals((DashboardLayout) objB);
    }

    public override bool Equals(object obj)
    {
      return obj is DashboardLayout && this.Equals((DashboardLayout) obj);
    }

    public override int GetHashCode() => this.LayoutId.GetHashCode();

    public static DashboardLayout GetDefaultDashboardLayout()
    {
      return new DashboardLayout(1, "C1R1", new RectangleF[1]
      {
        new RectangleF(0.0f, 0.0f, 1f, 1f)
      });
    }

    public static DashboardLayout NewDashboardLayout(
      int layoutId,
      string layoutName,
      RectangleF[] layoutBlocks)
    {
      return new DashboardLayout(layoutId, layoutName, layoutBlocks);
    }

    private DashboardLayout(int layoutId, string layoutName, RectangleF[] layoutBlocks)
    {
      this.layoutInfo = new DashboardLayoutInfo(layoutId, layoutName, layoutBlocks);
    }

    public void Dispose()
    {
    }
  }
}
