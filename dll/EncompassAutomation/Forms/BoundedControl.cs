// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.BoundedControl
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using mshtml;
using System;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public abstract class BoundedControl : RuntimeControl
  {
    private IHTMLElement boundedElement;

    protected BoundedControl()
    {
    }

    internal BoundedControl(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    [Category("Border")]
    public virtual BorderStyle BorderStyle
    {
      get
      {
        try
        {
          return (BorderStyle) Enum.Parse(typeof (BorderStyle), this.BoundedElement2.currentStyle.borderStyle, true);
        }
        catch
        {
          return BorderStyle.None;
        }
      }
      set
      {
        this.BorderWidth = this.BorderWidth;
        this.BoundedElement.style.borderStyle = value.ToString();
        this.NotifyPropertyChange();
      }
    }

    [Category("Border")]
    public virtual int BorderWidth
    {
      get
      {
        string borderWidth = this.BoundedElement2.currentStyle.borderWidth;
        return borderWidth.EndsWith("px") ? int.Parse(borderWidth.Substring(0, borderWidth.Length - 2)) : 0;
      }
      set
      {
        this.BoundedElement.style.borderWidth = value.ToString() + "px";
        this.NotifyPropertyChange();
      }
    }

    [Category("Border")]
    public virtual Color BorderColor
    {
      get => HTMLHelper.ColorFromStyle(this.BoundedElement2.currentStyle.borderColor);
      set
      {
        this.BorderWidth = this.BorderWidth;
        try
        {
          this.BoundedElement.style.borderColor = HTMLHelper.StyleFromColor(value);
        }
        catch
        {
          this.BoundedElement.style.borderColor = HTMLHelper.StyleFromColor(value, true);
        }
        this.NotifyPropertyChange();
      }
    }

    internal IHTMLElement BoundedElement
    {
      get
      {
        this.EnsureAttached();
        return this.boundedElement;
      }
    }

    internal IHTMLElement2 BoundedElement2
    {
      get
      {
        this.EnsureAttached();
        return this.boundedElement as IHTMLElement2;
      }
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      this.boundedElement = this.FindBoundedElement(controlElement);
    }

    internal virtual IHTMLElement FindBoundedElement(IHTMLElement controlElement) => controlElement;

    internal override bool ReattachRequired()
    {
      return base.ReattachRequired() || Control.IsDetached(this.boundedElement);
    }
  }
}
