// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.VerticalRule
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Used to add a vertical dividing line to the Form.</summary>
  [ToolboxControl("Vertical Rule")]
  public class VerticalRule : RuntimeControl
  {
    /// <summary>Constructor for a new VericialRule control.</summary>
    public VerticalRule()
    {
    }

    internal VerticalRule(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      string AttributeValue = Control.SpanContentEditable();
      if (string.IsNullOrWhiteSpace(AttributeValue))
        return;
      controlElement.setAttribute("contentEditable", (object) AttributeValue);
    }

    /// <summary>Gets or sets the enabled state of the control.</summary>
    /// <remarks>This property has no effect on this control.</remarks>
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
