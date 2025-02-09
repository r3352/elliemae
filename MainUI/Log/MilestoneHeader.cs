// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.MilestoneHeader
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Properties;
using EllieMae.EMLite.UI;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class MilestoneHeader : Panel
  {
    public MilestoneHeader()
    {
      this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (this.Width == 0 || this.Height == 0)
        return;
      using (Brush brush = (Brush) new SolidBrush(this.BackColor))
        e.Graphics.FillRectangle(brush, this.ClientRectangle);
      Rectangle rect = ControlDraw.DrawBorder(this.ClientRectangle, e.Graphics, BorderStyle.Fixed3D);
      using (Pen pen = new Pen(EncompassColors.Gradient1PaddingColor, 1f))
        e.Graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
      Rectangle rectangleInterior = ControlDraw.GetRectangleInterior(rect, new Padding(1));
      using (Brush brush = (Brush) new TextureBrush((Image) Resources.milestone_header_gradient_overlay, WrapMode.TileFlipY))
        e.Graphics.FillRectangle(brush, rectangleInterior);
    }
  }
}
