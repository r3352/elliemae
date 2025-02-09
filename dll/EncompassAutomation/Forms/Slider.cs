// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Slider
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using mshtml;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public abstract class Slider : DesignerControl
  {
    protected Point SlideStart = Point.Empty;

    public Slider()
    {
    }

    internal Slider(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
    }

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
