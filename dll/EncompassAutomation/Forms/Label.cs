// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Label
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Label")]
  public class Label : ContentControl
  {
    public Label()
    {
    }

    internal Label(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      string str = Control.SpanContentEditable();
      if (string.IsNullOrWhiteSpace(str))
        return;
      controlElement.setAttribute("contentEditable", (object) str, 1);
    }

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
