// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Label
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represents a simple lable containing static text on the Form.
  /// </summary>
  [ToolboxControl("Label")]
  public class Label : ContentControl
  {
    /// <summary>Constructor for a new Label control.</summary>
    public Label()
    {
    }

    internal Label(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      string AttributeValue = Control.SpanContentEditable();
      if (string.IsNullOrWhiteSpace(AttributeValue))
        return;
      controlElement.setAttribute("contentEditable", (object) AttributeValue);
    }

    /// <summary>Gets or sets the text displayed in the label.</summary>
    [Category("Appearance")]
    public string Text
    {
      get => this.HTMLElement.innerText ?? "";
      set
      {
        this.HTMLElement.innerText = value ?? "";
        this.NotifyPropertyChange();
      }
    }

    /// <summary>
    /// Determines how text in the element is clipped if it overflows the bounds of the control.
    /// </summary>
    [Category("Appearance")]
    [System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All)]
    public Overflow Overflow
    {
      get
      {
        if (((IHTMLStyle4) this.HTMLElement.style).textOverflow == "ellipsis")
          return Overflow.Ellipses;
        return (this.HTMLElement.style.overflow ?? "") == "hidden" ? Overflow.Clip : Overflow.Auto;
      }
      set
      {
        switch (value)
        {
          case Overflow.Auto:
            ((IHTMLStyle4) this.HTMLElement.style).textOverflow = "clip";
            this.HTMLElement.style.overflow = "visible";
            break;
          case Overflow.Clip:
            ((IHTMLStyle4) this.HTMLElement.style).textOverflow = "clip";
            this.HTMLElement.style.overflow = "hidden";
            break;
          case Overflow.Ellipses:
            this.HTMLElement.style.whiteSpace = "nowrap";
            ((IHTMLStyle4) this.HTMLElement.style).textOverflow = "ellipsis";
            this.HTMLElement.style.overflow = "hidden";
            break;
        }
        string text = this.Text;
        this.Text = "";
        this.Text = text;
      }
    }

    /// <summary>
    /// Gets or sets whether word-wrap is enabled in the label.
    /// </summary>
    /// <remarks>This property must be set to <c>false</c> in order for the <see cref="P:EllieMae.Encompass.Forms.Label.Overflow" />
    /// property to be set to <c>Ellipses</c>.</remarks>
    [Category("Appearance")]
    [System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All)]
    public bool WordWrap
    {
      get => (this.HTMLElement.style.whiteSpace ?? "") != "nowrap";
      set
      {
        this.HTMLElement.style.whiteSpace = value ? "normal" : "nowrap";
        if (!value || this.Overflow != Overflow.Ellipses)
          return;
        this.Overflow = Overflow.Auto;
      }
    }

    /// <summary>
    /// Gets or sets whether the label will automatically size itself to it contents.
    /// </summary>
    [Category("Appearance")]
    [Description("Controls whether the label will automatically size to fit its contents.")]
    public bool AutoSize
    {
      get => string.Concat(this.HTMLElement.style.width) == "";
      set
      {
        if (value)
        {
          this.HTMLElement.style.width = (object) "";
          this.HTMLElement.style.height = (object) "";
        }
        else
        {
          this.HTMLElement.style.width = this.HTMLElement2.currentStyle.width;
          this.HTMLElement.style.height = this.HTMLElement2.currentStyle.height;
        }
        this.NotifyPropertyChange();
        this.Form.OnResize(new FormEventArgs((Control) this));
      }
    }

    internal override string RenderHTML()
    {
      return "<span" + this.GetBaseAttributes() + ">Enter your text here.</span>";
    }
  }
}
