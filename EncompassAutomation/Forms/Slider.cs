// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Slider
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using mshtml;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// A Slider is a <see cref="T:EllieMae.Encompass.Forms.DesignerControl" /> which allows the form designer to easily shift
  /// form content left-and-right or up-and-down.
  /// </summary>
  /// <remarks>Because this control is a DesignerControl, it will not be visible at runtime if
  /// left on a Form.</remarks>
  public abstract class Slider : DesignerControl
  {
    /// <summary>
    /// Represents the starting position of a slide operation.
    /// </summary>
    protected Point SlideStart = Point.Empty;

    /// <summary>Constructor for a new Slider control.</summary>
    public Slider()
    {
    }

    internal Slider(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
    }

    /// <summary>Indicates of the slider is active or not.</summary>
    /// <remarks>This property can only be modified at design time. Attempts to modify this
    /// property at runtime will result in an exception.</remarks>
    [Description("Activates the slider control so movement of the control will move other form controls.")]
    [Category("Behavior")]
    public bool Active
    {
      get => this.GetAttribute("emactive") == "1";
      set
      {
        this.Form.EnsureEditing();
        this.SetAttribute("emactive", value ? "1" : "0");
        this.ApplyActiveStateBackgroundColor();
      }
    }

    internal void PrepareForSlide() => this.SlideStart = this.AbsolutePosition;

    internal abstract void SlideControls();

    internal void ApplyActiveStateBackgroundColor()
    {
      this.HTMLElement.style.backgroundColor = (object) HTMLHelper.StyleFromColor(this.Active ? Color.Green : Color.Red);
    }
  }
}
