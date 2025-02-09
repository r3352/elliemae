// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.VScrollBarDirector
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class VScrollBarDirector
  {
    private GridView GridView { get; set; }

    public VScrollBarDirector(GridView gridView)
    {
      this.GridView = gridView;
      gridView.ColumnClick += new GVColumnClickEventHandler(this.GridView_ColumnClick);
    }

    protected void GridView_ColumnClick(object sender, GVColumnClickEventArgs e)
    {
      this.EnsureVisible(this.GridView.Items.FirstOrDefault<GVItem>((Func<GVItem, bool>) (i => i.Selected))?.DisplayIndex ?? 0);
    }

    public void EnsureVisible(int index)
    {
      GridView gridView = this.GridView;
      int scrollVposition = gridView.ScrollVPosition;
      int scrollPosition = scrollVposition;
      Rectangle subItemBounds = gridView.GetSubItemBounds(index, 0);
      Rectangle scrollableRegion = gridView.ScrollableRegion;
      if (subItemBounds.Top < scrollableRegion.Top || subItemBounds.Bottom > scrollableRegion.Bottom)
      {
        int num = scrollableRegion.Height / subItemBounds.Height - 1;
        scrollPosition = scrollVposition >= index ? index : Math.Max(0, index - num);
      }
      gridView.SetVScroll(scrollPosition);
    }
  }
}
