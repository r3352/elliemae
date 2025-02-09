// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.PipelineImageLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public abstract class PipelineImageLink : PipelineElement, IMouseListener
  {
    private ImageLink link;

    public PipelineImageLink(
      Control parentControl,
      PipelineElementData data,
      Element displayValue,
      Image normalImage,
      Image hotImage)
      : base(parentControl, data)
    {
      if (!(string.Concat((object) displayValue) != ""))
        return;
      this.link = new ImageLink(displayValue, normalImage, hotImage, 3, new EventHandler(this.OnLinkClicked));
    }

    public PipelineImageLink(Control parentControl, Image normalImage, Image hotImage)
      : base(parentControl, (PipelineElementData) null)
    {
      this.link = new ImageLink((Element) null, normalImage, hotImage, 3, new EventHandler(this.OnLinkClicked));
    }

    protected abstract void OnLinkClicked(object sender, EventArgs e);

    public override Rectangle Draw(ItemDrawArgs e)
    {
      return this.link != null ? this.link.Draw(e) : Rectangle.Empty;
    }

    public override Size Measure(ItemDrawArgs drawArgs)
    {
      return this.link != null ? this.link.Measure(drawArgs) : Size.Empty;
    }

    bool IMouseListener.OnMouseEnter() => this.link != null && this.link.OnMouseEnter();

    bool IMouseListener.OnMouseLeave() => this.link != null && this.link.OnMouseLeave();

    bool IMouseListener.OnMouseMove(Point pt) => this.link != null && this.link.OnMouseMove(pt);

    bool IMouseListener.OnClick(Point pt) => this.link != null && this.link.OnClick(pt);
  }
}
