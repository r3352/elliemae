// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.HorizontalRule
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Horizontal Rule")]
  public class HorizontalRule : RuntimeControl
  {
    public HorizontalRule()
    {
    }

    internal HorizontalRule(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    [Browsable(false)]
    public override bool Enabled
    {
      get => base.Enabled;
      set => base.Enabled = value;
    }

    internal override Size AdjustSize(Size size) => new Size(size.Width, 3);

    internal override string RenderHTML()
    {
      return "<hr" + this.GetBaseAttributes(false) + " class=\"horizontalRule\" style=\"WIDTH: 100px; HEIGHT: 3px;\"/>";
    }
  }
}
