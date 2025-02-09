// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.AlertMessageLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class AlertMessageLink(AlertMessageLabel.AlertMessageStyle style, string text) : 
    AlertMessageLabel(style, text),
    IMouseListener
  {
    private bool hot;

    public event EventHandler Click;

    public AlertMessageLink(
      AlertMessageLabel.AlertMessageStyle style,
      string text,
      EventHandler clickHandler)
      : this(style, text)
    {
      this.Click += clickHandler;
    }

    public bool Activate()
    {
      Cursor.Current = Cursors.Hand;
      if (this.hot)
        return false;
      this.hot = true;
      return true;
    }

    public bool Deactivate()
    {
      if (!this.hot)
        return false;
      this.hot = false;
      return true;
    }

    protected override Image GetCurrentDisplayImage()
    {
      return this.Style == AlertMessageLabel.AlertMessageStyle.Alert ? (!this.hot ? (Image) Resources.alert : (Image) Resources.alert_hover) : (!this.hot ? (Image) Resources.new_message : (Image) Resources.new_message_over);
    }

    public bool OnMouseEnter() => false;

    public bool OnMouseLeave() => this.Deactivate();

    public bool OnMouseMove(Point pt)
    {
      return this.Bounds.Contains(pt) ? this.Activate() : this.Deactivate();
    }

    public bool OnClick(Point pt)
    {
      if (this.Bounds.Contains(pt) && this.Click != null)
        this.Click((object) this, EventArgs.Empty);
      return false;
    }
  }
}
