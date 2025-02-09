// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.AuditReportLabel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.UI;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class AuditReportLabel : Element
  {
    public AuditMessageStyle Style { get; private set; }

    public string Text { get; private set; }

    public Rectangle ImageRect { get; private set; }

    public Rectangle Bounds => this.ImageRect;

    public AuditReportLabel(AuditMessageStyle style, string text)
    {
      this.Style = style;
      this.Text = text;
    }

    public override Rectangle Draw(ItemDrawArgs e)
    {
      this.ImageRect = ControlDraw.DrawImage(this.GetCurrentDisplayImage(), e);
      Rectangle imageRect1 = this.ImageRect;
      int x1 = imageRect1.X;
      imageRect1 = this.ImageRect;
      int y1 = imageRect1.Y;
      imageRect1 = this.ImageRect;
      int width1 = imageRect1.Width * 2;
      imageRect1 = this.ImageRect;
      int height1 = imageRect1.Height;
      this.ImageRect = new Rectangle(x1, y1, width1, height1);
      Rectangle rect;
      ref Rectangle local = ref rect;
      Rectangle imageRect2 = this.ImageRect;
      int x2 = imageRect2.X;
      imageRect2 = this.ImageRect;
      int num = imageRect2.Width / 2;
      int x3 = x2 + num;
      imageRect2 = this.ImageRect;
      int y2 = imageRect2.Y;
      imageRect2 = this.ImageRect;
      int width2 = imageRect2.Width / 2;
      imageRect2 = this.ImageRect;
      int height2 = imageRect2.Height;
      local = new Rectangle(x3, y2, width2, height2);
      StringFormat defaultStringFormat = ControlDraw.CreateDefaultStringFormat(ContentAlignment.MiddleCenter);
      defaultStringFormat.FormatFlags |= StringFormatFlags.NoWrap;
      ControlDraw.DrawText(this.Text, new ItemDrawArgs(e.Graphics, EncompassFonts.Normal1.Font, Color.Black, Color.Transparent, ControlDraw.GetRectangleInterior(rect, new Padding(1, 0, 0, 0)), defaultStringFormat));
      return this.ImageRect;
    }

    public override Size Measure(ItemDrawArgs drawArgs) => this.ImageRect.Size;

    public override string ToString() => this.Text;

    protected virtual Image GetCurrentDisplayImage()
    {
      switch (this.Style)
      {
        case AuditMessageStyle.Red:
          return (Image) Resources.audit_red_icon;
        case AuditMessageStyle.Yellow:
          return (Image) Resources.audit_orange_icon;
        case AuditMessageStyle.Green:
          return (Image) Resources.audit_green_icon;
        default:
          return (Image) null;
      }
    }
  }
}
