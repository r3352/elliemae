// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.SectionControl
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public abstract class SectionControl : ContainerControl
  {
    private IHTMLElement headerElement;

    protected SectionControl()
    {
    }

    internal SectionControl(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    [Category("Header")]
    public string Title
    {
      get => this.HeaderElement.innerText;
      set
      {
        this.HeaderElement.innerText = value;
        this.NotifyPropertyChange();
      }
    }

    [Category("Header")]
    [Editor(typeof (HTMLFontEditor), typeof (UITypeEditor))]
    public virtual HTMLFont HeaderFont
    {
      get => new HTMLFont(this.HeaderElement2);
      set
      {
        value.ApplyToElement(this.HeaderElement);
        this.NotifyPropertyChange();
      }
    }

    [Category("Header")]
    public virtual Color HeaderBackColor
    {
      get => HTMLHelper.ColorFromStyle(this.HeaderElement2.currentStyle.backgroundColor.ToString());
      set
      {
        try
        {
          this.HeaderElement.style.backgroundColor = (object) HTMLHelper.StyleFromColor(value);
        }
        catch
        {
          this.HeaderElement.style.backgroundColor = (object) HTMLHelper.StyleFromColor(value, true);
        }
        this.NotifyPropertyChange();
      }
    }

    [Category("Header")]
    public virtual Color HeaderForeColor
    {
      get => HTMLHelper.ColorFromStyle(this.HeaderElement2.currentStyle.color.ToString());
      set
      {
        if (value == Color.Transparent)
          throw new ArgumentException("ForeColor cannot be set to transparent");
        try
        {
          this.HeaderElement.style.color = (object) HTMLHelper.StyleFromColor(value);
        }
        catch
        {
          this.HeaderElement.style.color = (object) HTMLHelper.StyleFromColor(value, true);
        }
        this.NotifyPropertyChange();
      }
    }

    internal IHTMLElement HeaderElement
    {
      get
      {
        this.EnsureAttached();
        return this.headerElement;
      }
    }

    internal IHTMLElement2 HeaderElement2 => this.HeaderElement as IHTMLElement2;

    internal virtual string GetBaseHeaderAttributes() => " headerId=\"" + this.ControlID + "\" ";

    internal override void ChangeControlID(string oldValue, string newValue)
    {
      this.HeaderElement.setAttribute("headerId", (object) newValue, 1);
      base.ChangeControlID(oldValue, newValue);
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      this.headerElement = this.FindHeaderElement(controlElement);
    }

    internal override bool ReattachRequired()
    {
      return base.ReattachRequired() || Control.IsDetached(this.headerElement);
    }

    internal virtual IHTMLElement FindHeaderElement(IHTMLElement controlElement)
    {
      return HTMLHelper.FindElementWithAttribute(controlElement, "headerId", this.ControlID);
    }
  }
}
