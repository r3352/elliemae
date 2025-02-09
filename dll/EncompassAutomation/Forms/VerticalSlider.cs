// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.VerticalSlider
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Vertical Slider")]
  public class VerticalSlider : Slider
  {
    public VerticalSlider()
    {
    }

    internal VerticalSlider(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
    }

    internal override void SlideControls()
    {
      Point absolutePosition1 = this.AbsolutePosition;
      int num = absolutePosition1.Y - this.SlideStart.Y;
      int width1 = this.Bounds.Width;
      if (num != 0)
      {
        foreach (Control control in this.GetContainer().Controls)
        {
          if (control != this)
          {
            Point absolutePosition2 = control.AbsolutePosition;
            int width2 = control.Bounds.Width;
            if (absolutePosition2.Y >= this.SlideStart.Y && absolutePosition2.X <= this.SlideStart.X + width1 && absolutePosition2.X + width2 >= this.SlideStart.X)
              control.AbsolutePosition = new Point(absolutePosition2.X, absolutePosition2.Y + num);
          }
        }
      }
      if (!(absolutePosition1 != this.SlideStart))
        return;
      this.SlideStart = new Point(this.SlideStart.X, absolutePosition1.Y);
      this.AbsolutePosition = this.SlideStart;
    }

    internal override string RenderHTML()
    {
      return "<hr" + this.GetBaseAttributes() + " style=\"border: dashed black 1px; background-color: red; HEIGHT: 5px; WIDTH: 150px;\" contentEditable=\"false\"/>";
    }
  }
}
