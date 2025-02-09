// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientSession.Dashboard.DashboardLayoutCollection
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BizLayer;
using System;
using System.Collections;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.ClientSession.Dashboard
{
  [Serializable]
  public class DashboardLayoutCollection : BusinessCollectionBase
  {
    public DashboardLayout this[int index] => (DashboardLayout) this.List[index];

    public DashboardLayout Find(int layoutId)
    {
      foreach (DashboardLayout dashboardLayout in (IEnumerable) this.List)
      {
        if (dashboardLayout.LayoutId.Equals(layoutId))
          return dashboardLayout;
      }
      return (DashboardLayout) null;
    }

    public DashboardLayout Find(string layoutName)
    {
      foreach (DashboardLayout dashboardLayout in (IEnumerable) this.List)
      {
        if (dashboardLayout.LayoutName.Equals(layoutName))
          return dashboardLayout;
      }
      return (DashboardLayout) null;
    }

    public bool Contains(int layoutId) => this.Find(layoutId) != null;

    public bool Contains(string layoutName) => this.Find(layoutName) != null;

    public static DashboardLayoutCollection GetDashboardLayoutCollection()
    {
      DashboardLayoutCollection layoutCollection = new DashboardLayoutCollection();
      foreach (DashboardLayout dashboardLayout in DashboardLayoutCollection.getDashboardLayouts())
        layoutCollection.List.Add((object) dashboardLayout);
      return layoutCollection;
    }

    private static DashboardLayout[] getDashboardLayouts()
    {
      return new DashboardLayout[19]
      {
        DashboardLayout.NewDashboardLayout(0, string.Empty, new RectangleF[0]),
        DashboardLayout.NewDashboardLayout(1, "C1R1", new RectangleF[1]
        {
          new RectangleF(0.0f, 0.0f, 1f, 1f)
        }),
        DashboardLayout.NewDashboardLayout(2, "C1R2", new RectangleF[2]
        {
          new RectangleF(0.0f, 0.0f, 1f, 0.5f),
          new RectangleF(0.0f, 0.5f, 1f, 0.5f)
        }),
        DashboardLayout.NewDashboardLayout(3, "C2R1R1", new RectangleF[2]
        {
          new RectangleF(0.0f, 0.0f, 0.5f, 1f),
          new RectangleF(0.5f, 0.0f, 0.5f, 1f)
        }),
        DashboardLayout.NewDashboardLayout(4, "C2R2R2", new RectangleF[4]
        {
          new RectangleF(0.0f, 0.0f, 0.5f, 0.5f),
          new RectangleF(0.0f, 0.5f, 0.5f, 0.5f),
          new RectangleF(0.5f, 0.0f, 0.5f, 0.5f),
          new RectangleF(0.5f, 0.5f, 0.5f, 0.5f)
        }),
        DashboardLayout.NewDashboardLayout(5, "C2R2R2W4", new RectangleF[4]
        {
          new RectangleF(0.0f, 0.0f, 0.4f, 0.5f),
          new RectangleF(0.0f, 0.5f, 0.4f, 0.5f),
          new RectangleF(0.4f, 0.0f, 0.6f, 0.5f),
          new RectangleF(0.4f, 0.5f, 0.6f, 0.5f)
        }),
        DashboardLayout.NewDashboardLayout(6, "C2R2R2W6", new RectangleF[4]
        {
          new RectangleF(0.0f, 0.0f, 0.6f, 0.5f),
          new RectangleF(0.0f, 0.5f, 0.6f, 0.5f),
          new RectangleF(0.6f, 0.0f, 0.4f, 0.5f),
          new RectangleF(0.6f, 0.5f, 0.4f, 0.5f)
        }),
        DashboardLayout.NewDashboardLayout(7, "C2R3R2", new RectangleF[5]
        {
          new RectangleF(0.0f, 0.0f, 0.5f, 0.333f),
          new RectangleF(0.0f, 0.333f, 0.5f, 0.333f),
          new RectangleF(0.0f, 0.666f, 0.5f, 0.333f),
          new RectangleF(0.5f, 0.0f, 0.5f, 0.5f),
          new RectangleF(0.5f, 0.5f, 0.5f, 0.5f)
        }),
        DashboardLayout.NewDashboardLayout(8, "C2R3R2W4", new RectangleF[5]
        {
          new RectangleF(0.0f, 0.0f, 0.4f, 0.333f),
          new RectangleF(0.0f, 0.333f, 0.4f, 0.333f),
          new RectangleF(0.0f, 0.666f, 0.4f, 0.333f),
          new RectangleF(0.4f, 0.0f, 0.6f, 0.5f),
          new RectangleF(0.4f, 0.5f, 0.6f, 0.5f)
        }),
        DashboardLayout.NewDashboardLayout(9, "C2R3R2W6", new RectangleF[5]
        {
          new RectangleF(0.0f, 0.0f, 0.6f, 0.333f),
          new RectangleF(0.0f, 0.333f, 0.6f, 0.333f),
          new RectangleF(0.0f, 0.666f, 0.6f, 0.333f),
          new RectangleF(0.6f, 0.0f, 0.4f, 0.5f),
          new RectangleF(0.6f, 0.5f, 0.4f, 0.5f)
        }),
        DashboardLayout.NewDashboardLayout(10, "C2R2R3", new RectangleF[5]
        {
          new RectangleF(0.0f, 0.0f, 0.5f, 0.5f),
          new RectangleF(0.0f, 0.5f, 0.5f, 0.5f),
          new RectangleF(0.5f, 0.0f, 0.5f, 0.333f),
          new RectangleF(0.5f, 0.333f, 0.5f, 0.333f),
          new RectangleF(0.5f, 0.666f, 0.5f, 0.333f)
        }),
        DashboardLayout.NewDashboardLayout(11, "C2R2R3W4", new RectangleF[5]
        {
          new RectangleF(0.0f, 0.0f, 0.4f, 0.5f),
          new RectangleF(0.0f, 0.5f, 0.4f, 0.5f),
          new RectangleF(0.4f, 0.0f, 0.6f, 0.333f),
          new RectangleF(0.4f, 0.333f, 0.6f, 0.333f),
          new RectangleF(0.4f, 0.666f, 0.6f, 0.333f)
        }),
        DashboardLayout.NewDashboardLayout(12, "C2R2R3W6", new RectangleF[5]
        {
          new RectangleF(0.0f, 0.0f, 0.6f, 0.5f),
          new RectangleF(0.0f, 0.5f, 0.6f, 0.5f),
          new RectangleF(0.6f, 0.0f, 0.4f, 0.333f),
          new RectangleF(0.6f, 0.333f, 0.4f, 0.333f),
          new RectangleF(0.6f, 0.666f, 0.4f, 0.333f)
        }),
        DashboardLayout.NewDashboardLayout(13, "C2R3R3", new RectangleF[6]
        {
          new RectangleF(0.0f, 0.0f, 0.5f, 0.333f),
          new RectangleF(0.0f, 0.333f, 0.5f, 0.333f),
          new RectangleF(0.0f, 0.666f, 0.5f, 0.333f),
          new RectangleF(0.5f, 0.0f, 0.5f, 0.333f),
          new RectangleF(0.5f, 0.333f, 0.5f, 0.333f),
          new RectangleF(0.5f, 0.666f, 0.5f, 0.333f)
        }),
        DashboardLayout.NewDashboardLayout(14, "C2R3R3W4", new RectangleF[6]
        {
          new RectangleF(0.0f, 0.0f, 0.4f, 0.333f),
          new RectangleF(0.0f, 0.333f, 0.4f, 0.333f),
          new RectangleF(0.0f, 0.666f, 0.4f, 0.333f),
          new RectangleF(0.4f, 0.0f, 0.6f, 0.333f),
          new RectangleF(0.4f, 0.333f, 0.6f, 0.333f),
          new RectangleF(0.4f, 0.666f, 0.6f, 0.333f)
        }),
        DashboardLayout.NewDashboardLayout(15, "C2R3R3W6", new RectangleF[6]
        {
          new RectangleF(0.0f, 0.0f, 0.6f, 0.333f),
          new RectangleF(0.0f, 0.333f, 0.6f, 0.333f),
          new RectangleF(0.0f, 0.666f, 0.6f, 0.333f),
          new RectangleF(0.6f, 0.0f, 0.4f, 0.333f),
          new RectangleF(0.6f, 0.333f, 0.4f, 0.333f),
          new RectangleF(0.6f, 0.666f, 0.4f, 0.333f)
        }),
        DashboardLayout.NewDashboardLayout(16, "C3R2R2R2", new RectangleF[6]
        {
          new RectangleF(0.0f, 0.0f, 0.333f, 0.5f),
          new RectangleF(0.0f, 0.5f, 0.333f, 0.5f),
          new RectangleF(0.333f, 0.0f, 0.333f, 0.5f),
          new RectangleF(0.333f, 0.5f, 0.333f, 0.5f),
          new RectangleF(0.667f, 0.0f, 0.333f, 0.5f),
          new RectangleF(0.667f, 0.5f, 0.333f, 0.5f)
        }),
        DashboardLayout.NewDashboardLayout(17, "R2C2C3", new RectangleF[5]
        {
          new RectangleF(0.0f, 0.0f, 0.5f, 0.5f),
          new RectangleF(0.0f, 0.5f, 0.333f, 0.5f),
          new RectangleF(0.5f, 0.0f, 0.5f, 0.5f),
          new RectangleF(0.333f, 0.5f, 0.333f, 0.5f),
          new RectangleF(0.667f, 0.5f, 0.333f, 0.5f)
        }),
        DashboardLayout.NewDashboardLayout(18, "C3R3R3R3", new RectangleF[9]
        {
          new RectangleF(0.0f, 0.0f, 0.333f, 0.333f),
          new RectangleF(0.0f, 0.333f, 0.333f, 0.333f),
          new RectangleF(0.0f, 0.666f, 0.333f, 0.333f),
          new RectangleF(0.333f, 0.0f, 0.333f, 0.333f),
          new RectangleF(0.333f, 0.333f, 0.333f, 0.333f),
          new RectangleF(0.333f, 0.666f, 0.333f, 0.333f),
          new RectangleF(0.666f, 0.0f, 0.333f, 0.333f),
          new RectangleF(0.666f, 0.333f, 0.333f, 0.333f),
          new RectangleF(0.666f, 0.666f, 0.333f, 0.333f)
        })
      };
    }

    private DashboardLayoutCollection()
    {
    }
  }
}
