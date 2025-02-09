// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewProgressBar
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ListViewProgressBar
  {
    private const int bufferX = 5;
    private const int bufferY = 3;
    private const int minProgressWidth = 20;

    public static void Draw(DrawListViewSubItemEventArgs e)
    {
      ListViewProgressBar.Draw(e, Decimal.Parse(e.SubItem.Text), true);
    }

    public static void Draw(DrawListViewSubItemEventArgs e, Decimal percent, bool showNumericValue)
    {
      e.DrawDefault = false;
      Brush brush1;
      Brush brush2;
      if (e.Item.Selected)
      {
        brush1 = (Brush) new SolidBrush(SystemColors.HighlightText);
        brush2 = (Brush) new SolidBrush(SystemColors.Highlight);
      }
      else if (e.Item.UseItemStyleForSubItems)
      {
        brush1 = (Brush) new SolidBrush(e.Item.ForeColor);
        brush2 = (Brush) new SolidBrush(e.Item.BackColor);
      }
      else
      {
        brush1 = (Brush) new SolidBrush(e.SubItem.ForeColor);
        brush2 = (Brush) new SolidBrush(e.SubItem.BackColor);
      }
      e.Graphics.FillRectangle(brush2, e.Bounds);
      string s = percent.ToString("0") + "%";
      float width1 = e.Graphics.MeasureString("100%", e.SubItem.Font).Width + 1f;
      int num = e.Bounds.Width - 10;
      if (showNumericValue)
        num -= (int) ((double) width1 + 5.0);
      StringFormat format = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.LineLimit);
      format.Alignment = StringAlignment.Far;
      format.LineAlignment = StringAlignment.Center;
      RectangleF rectangleF;
      ref RectangleF local1 = ref rectangleF;
      Rectangle bounds = e.Bounds;
      double x1 = (double) bounds.X;
      bounds = e.Bounds;
      double y1 = (double) bounds.Y;
      bounds = e.Bounds;
      double width2 = (double) bounds.Width;
      bounds = e.Bounds;
      double height1 = (double) bounds.Height;
      local1 = new RectangleF((float) x1, (float) y1, (float) width2, (float) height1);
      if (num <= 20 & showNumericValue)
      {
        RectangleF layoutRectangle = new RectangleF(rectangleF.X + (float) (((double) rectangleF.Width - (double) width1) / 2.0), rectangleF.Y, width1, rectangleF.Height);
        e.Graphics.DrawString(s, e.SubItem.Font, brush1, layoutRectangle, format);
      }
      else
      {
        if (showNumericValue)
        {
          RectangleF layoutRectangle = new RectangleF((float) ((double) rectangleF.X + (double) num + 5.0), rectangleF.Y, width1, rectangleF.Height);
          e.Graphics.DrawString(s, e.SubItem.Font, brush1, layoutRectangle, format);
        }
        Rectangle rect1;
        ref Rectangle local2 = ref rect1;
        bounds = e.Bounds;
        int x2 = bounds.X + 5;
        bounds = e.Bounds;
        int y2 = bounds.Y + 3;
        int width3 = num;
        bounds = e.Bounds;
        int height2 = bounds.Height - 6;
        local2 = new Rectangle(x2, y2, width3, height2);
        e.Graphics.DrawRectangle(Pens.Gray, rect1);
        int width4 = (int) Math.Floor((Decimal) (rect1.Width - 3) * percent / 100M);
        if (percent < 0M)
          width4 = 0;
        if (percent >= 100M)
          width4 = rect1.Width - 3;
        Rectangle rect2 = new Rectangle(rect1.X + 2, rect1.Y + 2, width4, rect1.Height - 3);
        e.Graphics.FillRectangle(Brushes.LimeGreen, rect2);
      }
      brush2.Dispose();
      brush1.Dispose();
    }
  }
}
