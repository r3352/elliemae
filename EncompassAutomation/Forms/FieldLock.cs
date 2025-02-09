// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.FieldLock
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// The FieldLock control allows the user to lock and unlock Encompass calculated fields.
  /// </summary>
  [ToolboxControl("Field Lock")]
  [PropertyTab(typeof (EventsTab), PropertyTabScope.Component)]
  public class FieldLock : RuntimeControl, ISupportsClickEvent, ISupportsEvents
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Click"
    };
    private ScopedEventHandler<EventArgs> click = new ScopedEventHandler<EventArgs>(nameof (FieldLock), "Click");
    private bool locked = true;

    /// <summary>
    /// The Click event is fired whenever the user clicks this control.
    /// </summary>
    [Category("Behavior")]
    public event EventHandler Click
    {
      add
      {
        if (value == null)
          return;
        this.click.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.click.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>Constructor for a new FieldLock control.</summary>
    public FieldLock()
    {
    }

    internal FieldLock(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
    }

    /// <summary>
    /// Gets or sets the Control ID of the field control to be locked by this
    /// FieldLock.
    /// </summary>
    [Category("Behavior")]
    [TypeConverter(typeof (NonExpandableControlTypeConverter))]
    [Editor(typeof (LockableControlSelector), typeof (UITypeEditor))]
    public Control ControlToLock
    {
      get
      {
        string attribute = this.GetAttribute("emid");
        return attribute == "" ? (Control) null : this.Form.FindControl(attribute);
      }
      set
      {
        if (value == null)
        {
          this.SetAttribute("emid", "");
        }
        else
        {
          if (value.Form != this.Form)
            throw new ArgumentException("The specified control belong to a different form");
          this.SetAttribute("emid", value.ControlID);
        }
        this.NotifyPropertyChange();
      }
    }

    /// <summary>
    /// Gets of sets the tooltip text that appears when the user hovers the mouse over this control.
    /// </summary>
    [Category("Appearance")]
    [Description("The text that appears when the user hovers the mouse over the control.")]
    [Browsable(false)]
    public virtual string HoverText
    {
      get => this.HTMLElement.title ?? "";
      set
      {
      }
    }

    /// <summary>Indicates if the field lock is currently set.</summary>
    [Browsable(false)]
    public bool Locked => this.locked;

    /// <summary>Changes the lock image on the button.</summary>
    /// <param name="locked">Indicates whether the lock or unlock image
    /// should currently be displayed.</param>
    /// <remarks>This method is for use by the Encompass application framework
    /// and should not be invoked by external applications.</remarks>
    public void DisplayImage(bool locked)
    {
      this.locked = locked;
      this.getImageElement().src = this.getIconImageSource(locked, this.Interactive);
      this.HTMLElement.title = locked ? "Use the Calculated Value" : "Enter Data Manually";
    }

    /// <summary>
    /// Gets the names of the events supported by this control.
    /// </summary>
    [Browsable(false)]
    public string[] SupportedEvents => FieldLock.supportedEvents;

    /// <summary>Invokes the Click event on the control.</summary>
    /// <returns></returns>
    /// <remarks>This method is intended for use by the Encompass application only.</remarks>
    /// <exclude />
    public bool InvokeClick()
    {
      this.OnClick(EventArgs.Empty);
      return true;
    }

    /// <summary>
    /// Raises the <see cref="E:EllieMae.Encompass.Forms.FieldLock.Click" /> event.
    /// </summary>
    /// <param name="e">The event parameters to be passed to the event handlers.</param>
    protected virtual void OnClick(EventArgs e) => this.click.Invoke((object) this, e);

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      IHTMLElement imageElement = (IHTMLElement) this.getImageElement();
      imageElement.style.width = (object) "16px";
      imageElement.style.height = (object) "16px";
      this.SetAttribute("tabIndex", "-1");
      this.DisplayImage(this.locked);
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      form.ControlIDChanged -= new ControlIDChangedEventHandler(this.onFormControlIDChanged);
      form.ControlIDChanged += new ControlIDChangedEventHandler(this.onFormControlIDChanged);
    }

    internal override void ChangeControlInteractiveState(bool interactive)
    {
      base.ChangeControlInteractiveState(interactive);
      this.getImageElement().src = this.getIconImageSource(this.locked, interactive);
    }

    private void onFormControlIDChanged(object source, ControlIDChangedEventArgs e)
    {
      if (!(e.PriorControlID == this.GetAttribute("emid")))
        return;
      this.SetAttribute("emid", e.Control.ControlID);
    }

    private IHTMLImgElement getImageElement()
    {
      return (IHTMLImgElement) (HTMLHelper.GetChildWithTagName(this.HTMLElement, "img") ?? throw new Exception("The interior image element could not be found"));
    }

    internal override string RenderHTML()
    {
      return "<button" + this.GetBaseAttributes(false) + "class=\"inputButtonImage\"><img src=\"" + this.getIconImageSource(false, true) + "\" width=\"16\" height=\"16\" emimport=\"0\" onmouseover=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('.png', '-over.png'); }\" onmouseout=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('-over.png', '.png'); }\"></button>";
    }

    private string getIconImageSource(bool locked, bool enabled)
    {
      return locked ? Control.ResolveInternalImagePath(enabled ? "field-lock.png" : "field-lock-disabled.png") : Control.ResolveInternalImagePath(enabled ? "field-unlock.png" : "field-unlock-disabled.png");
    }
  }
}
