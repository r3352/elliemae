// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ContentControl
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Represents a control which contains textual content.</summary>
  /// <remarks>Content controls provide properties that have to do with formatting and
  /// display text-based content, such as font information.</remarks>
  public abstract class ContentControl : BoundedControl
  {
    private IHTMLElement contentElement;

    /// <summary>Default constructor for the ContentControl.</summary>
    protected ContentControl()
    {
    }

    internal ContentControl(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    /// <summary>
    /// Gets or sets the <see cref="T:EllieMae.Encompass.Forms.HTMLFont" /> used to render the text of the control.
    /// </summary>
    [Category("Appearance")]
    [Editor(typeof (HTMLFontEditor), typeof (UITypeEditor))]
    public virtual HTMLFont Font
    {
      get => new HTMLFont(this.ContentElement2);
      set
      {
        value.ApplyToElement(this.ContentElement);
        this.NotifyPropertyChange();
      }
    }

    /// <summary>
    /// Indicates if the control supports setting the Font property.
    /// </summary>
    [Browsable(false)]
    public virtual bool SupportsFont => true;

    /// <summary>Gets or sets the background color of the control.</summary>
    [Category("Appearance")]
    public virtual Color BackColor
    {
      get
      {
        return HTMLHelper.ColorFromStyle(this.ContentElement2.currentStyle.backgroundColor.ToString());
      }
      set
      {
        try
        {
          this.ContentElement.style.backgroundColor = (object) HTMLHelper.StyleFromColor(value);
        }
        catch
        {
          this.ContentElement.style.backgroundColor = (object) HTMLHelper.StyleFromColor(value, true);
        }
        this.NotifyPropertyChange();
      }
    }

    /// <summary>
    /// Indicates if the control supports setting the BackColor property.
    /// </summary>
    [Browsable(false)]
    public virtual bool SupportsBackColor => true;

    /// <summary>Gets or sets the foreground color of the control.</summary>
    /// <remarks>The foreground color of a control determines the color of the font.</remarks>
    [Category("Appearance")]
    public virtual Color ForeColor
    {
      get => HTMLHelper.ColorFromStyle(this.ContentElement2.currentStyle.color.ToString());
      set
      {
        if (value == Color.Transparent)
          throw new ArgumentException("ForeColor cannot be set to transparent");
        try
        {
          this.ContentElement.style.color = (object) HTMLHelper.StyleFromColor(value);
        }
        catch
        {
          this.ContentElement.style.color = (object) HTMLHelper.StyleFromColor(value, true);
        }
        this.NotifyPropertyChange();
      }
    }

    /// <summary>
    /// Indicates if the control supports setting the ForeColor property.
    /// </summary>
    [Browsable(false)]
    public virtual bool SupportsForeColor => true;

    internal IHTMLElement ContentElement
    {
      get
      {
        this.EnsureAttached();
        return this.contentElement;
      }
    }

    internal IHTMLElement2 ContentElement2 => this.ContentElement as IHTMLElement2;

    internal override bool ReattachRequired()
    {
      return base.ReattachRequired() || Control.IsDetached(this.contentElement);
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      this.contentElement = this.FindContentElement(controlElement);
      if (this.contentElement == null)
        throw new InvalidOperationException("Content element not found for control " + this.ControlID);
    }

    internal virtual IHTMLElement FindContentElement(IHTMLElement controlElement)
    {
      return this.FindBoundedElement(controlElement);
    }
  }
}
