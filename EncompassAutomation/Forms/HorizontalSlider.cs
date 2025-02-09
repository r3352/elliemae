// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.HorizontalSlider
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// A <see cref="T:EllieMae.Encompass.Forms.Slider" /> control used to move controls horizontally in the form.
  /// </summary>
  /// <remarks>This control is a <see cref="T:EllieMae.Encompass.Forms.DesignerControl" /> and is not intended for use
  /// at runtime.</remarks>
  [ToolboxControl("Horizontal Slider")]
  public class HorizontalSlider : Slider
  {
    /// <summary>Constructor for a new Slider control.</summary>
    public HorizontalSlider()
    {
    }

    internal HorizontalSlider(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
      string AttributeValue = Control.SpanContentEditable();
      if (string.IsNullOrWhiteSpace(AttributeValue))
        return;
      controlElement.setAttribute("contentEditable", (object) AttributeValue);
    }

    internal override void SlideControls()
    {
      Point absolutePosition1 = this.AbsolutePosition;
      int num = absolutePosition1.X - this.SlideStart.X;
      int height1 = this.Bounds.Height;
      if (num != 0)
      {
        foreach (Control control in this.GetContainer().Controls)
        {
          if (control != this)
          {
            Point absolutePosition2 = control.AbsolutePosition;
            int height2 = control.Bounds.Height;
            if (absolutePosition2.X >= this.SlideStart.X && absolutePosition2.Y <= this.SlideStart.Y + height1 && absolutePosition2.Y + height2 >= this.SlideStart.Y)
              control.AbsolutePosition = new Point(absolutePosition2.X + num, absolutePosition2.Y);
          }
        }
      }
      if (!(absolutePosition1 != this.SlideStart))
        return;
      this.SlideStart = new Point(absolutePosition1.X, this.SlideStart.Y);
      this.AbsolutePosition = this.SlideStart;
    }

    internal override string RenderHTML()
    {
      return "<span" + this.GetBaseAttributes() + " style=\"border: dashed black 1px; background-color: red; WIDTH: 5px; HEIGHT: 150px;\"><span style=\"border: inset #bbbbbb 1px; width=100%; height=100%\"/></span>";
    }
  }
}
