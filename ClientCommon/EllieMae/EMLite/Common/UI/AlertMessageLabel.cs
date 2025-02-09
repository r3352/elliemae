// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.AlertMessageLabel
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.UI;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class AlertMessageLabel : Element
  {
    private AlertMessageLabel.AlertMessageStyle style;
    private string text;
    private Rectangle imageRect = Rectangle.Empty;

    public AlertMessageLabel(AlertMessageLabel.AlertMessageStyle style, string text)
    {
      this.style = style;
      this.text = text;
    }

    public AlertMessageLabel.AlertMessageStyle Style
    {
      get => this.style;
      set => this.style = value;
    }

    public string Text
    {
      get => this.text;
      set => this.text = value;
    }

    protected Rectangle Bounds => this.imageRect;

    public override Rectangle Draw(ItemDrawArgs e)
    {
      this.imageRect = ControlDraw.DrawImage(this.GetCurrentDisplayImage(), e);
      StringFormat defaultStringFormat = ControlDraw.CreateDefaultStringFormat(ContentAlignment.MiddleCenter);
      defaultStringFormat.FormatFlags |= StringFormatFlags.NoWrap;
      ControlDraw.DrawText(this.text, new ItemDrawArgs(e.Graphics, EncompassFonts.Normal2.Font, Color.White, Color.Transparent, ControlDraw.GetRectangleInterior(this.imageRect, new Padding(1, 0, 0, 0)), defaultStringFormat));
      return this.imageRect;
    }

    protected virtual Image GetCurrentDisplayImage()
    {
      return this.style == AlertMessageLabel.AlertMessageStyle.Alert ? (Image) Resources.alert : (Image) Resources.new_message;
    }

    public override Size Measure(ItemDrawArgs drawArgs) => Resources.alert.Size;

    public override string ToString() => this.text;

    public enum AlertMessageStyle
    {
      Alert = 1,
      Message = 2,
    }
  }
}
