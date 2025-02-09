// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.HorizontalRule
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
  /// <summary>Represents a horizontal line on the Form.</summary>
  [ToolboxControl("Horizontal Rule")]
  public class HorizontalRule : RuntimeControl
  {
    /// <summary>Constructor for a new HorizontalRule control.</summary>
    public HorizontalRule()
    {
    }

    internal HorizontalRule(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    /// <summary>Gets or sets the enabled state of the control.</summary>
    /// <remarks>This property has no effect on this control.</remarks>
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
