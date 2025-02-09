// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Customization.MultiColumnLayout
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Customization
{
  public class MultiColumnLayout : SurfaceLayout
  {
    private List<IControlSurface> tiles = new List<IControlSurface>();
    private MultiColumnLayout.LayoutParameters parameters;
    private Size tileSize;

    public MultiColumnLayout(IControlSurface surface, MultiColumnLayout.LayoutParameters parameters)
      : base(surface)
    {
      this.parameters = parameters;
      this.computeLayout();
    }

    public override Size TileSize => this.tileSize;

    public override IControlSurface CreateTile()
    {
      IControlSurface surface = this.Surface.CreateSurface(new Rectangle(new Point(0, 0), this.tileSize));
      this.tiles.Add(surface);
      return surface;
    }

    public override void FinalizeLayout()
    {
      int num = 0;
      for (int index = 0; index < this.tiles.Count; ++index)
      {
        Point tilePosition = this.getTilePosition(index);
        this.tiles[index].Bounds = new Rectangle(tilePosition, this.tileSize);
        num = Math.Max(num, tilePosition.Y + this.tileSize.Height + this.parameters.Padding.Bottom);
      }
      if (!this.parameters.AutoResize)
        return;
      Rectangle bounds = this.Surface.Bounds;
      this.Surface.Bounds = new Rectangle(bounds.Location, new Size(bounds.Width, num));
    }

    private Point getTilePosition(int index)
    {
      int num1;
      int num2;
      if (this.parameters.LayoutDirection == LayoutDirection.Horizontal)
      {
        num1 = index / this.parameters.ColumnCount;
        num2 = index % this.parameters.ColumnCount;
      }
      else
      {
        int num3 = this.tiles.Count / this.parameters.ColumnCount;
        if (this.tiles.Count % this.parameters.ColumnCount != 0)
          ++num3;
        num1 = index % num3;
        num2 = index / num3;
      }
      return new Point(num2 * (this.tileSize.Width + this.parameters.ColumnSpacing) + this.parameters.Padding.Left, num1 * (this.tileSize.Height + this.parameters.RowSpacing) + this.parameters.Padding.Top);
    }

    private void computeLayout()
    {
      this.tileSize = new Size((this.Surface.Bounds.Size.Width - (this.parameters.ColumnCount - 1) * this.parameters.ColumnSpacing - this.parameters.Padding.Horizontal) / this.parameters.ColumnCount, this.parameters.RowHeight);
    }

    public class LayoutParameters
    {
      public int ColumnCount = 2;
      public int ColumnSpacing = 15;
      public int RowSpacing = 2;
      public int RowHeight = 22;
      public Padding Padding = new Padding(10);
      public LayoutDirection LayoutDirection = LayoutDirection.Vertical;
      public bool AutoResize = true;
    }
  }
}
