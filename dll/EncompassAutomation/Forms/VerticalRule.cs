// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.VerticalRule
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
  [ToolboxControl("Vertical Rule")]
  public class VerticalRule : RuntimeControl
  {
    public VerticalRule()
    {
    }

    internal VerticalRule(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      string str = Control.SpanContentEditable();
      if (string.IsNullOrWhiteSpace(str))
        return;
      controlElement.setAttribute("contentEditable", (object) str, 1);
    }

    [Browsable(false)]
    public override bool Enabled
    {
      get => base.Enabled;
      set => base.Enabled = value;
    }

    internal override Size AdjustSize(Size size) => new Size(3, size.Height);

    internal override string RenderHTML()
    {
      return "<span" + this.GetBaseAttributes() + " class=\"verticalRule\" style=\"WIDTH: 3px; HEIGHT: 150px\"/>";
    }
  }
}
