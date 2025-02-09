// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Calendar
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.BusinessObjects.Loans;
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
  /// Provides a button which can be used to display a pop-up calendar from which the user can select a date.
  /// </summary>
  [PropertyTab(typeof (EventsTab), PropertyTabScope.Component)]
  [ToolboxControl("Calendar")]
  public class Calendar : RuntimeControl, ISupportsDateSelectedEvent, ISupportsEvents
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "DateSelected"
    };
    private FieldAccessMode accessMode;
    private ScopedEventHandler<DateSelectedEventArgs> dateSelected;

    /// <summary>
    /// The Click event is fired when the user clicks the icon for the ContactButton.
    /// </summary>
    [Category("Behavior")]
    public event DateSelectedEventHandler DateSelected
    {
      add
      {
        if (value == null)
          return;
        this.dateSelected.Add(new ScopedEventHandler<DateSelectedEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.dateSelected.Remove(new ScopedEventHandler<DateSelectedEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>Constructor for a new ContactButton control.</summary>
    public Calendar()
    {
      this.dateSelected = new ScopedEventHandler<DateSelectedEventArgs>(nameof (Calendar), "DateSelected");
    }

    internal Calendar(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
      this.dateSelected = new ScopedEventHandler<DateSelectedEventArgs>(nameof (Calendar), "DateSelected");
    }

    /// <summary>
    /// Gets or sets the field that will be used to populate the Last Name information of the contact record.
    /// </summary>
    [Category("Data")]
    [Description("The field that will be populated with the selected date.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor DateField
    {
      get => this.getAssociatedField("dateField");
      set => this.setAssociatedField("dateField", value);
    }

    /// <summary>Gets or sets the source of data for this loan field.</summary>
    /// <remarks>This property is currently not used and should not be modified.</remarks>
    [Category("Data")]
    [Description("Indicates the source of the loan data for this field")]
    public FieldSource FieldSource
    {
      get
      {
        return !(this.GetAttribute("emsrc") == "link") ? FieldSource.CurrentLoan : FieldSource.LinkedLoan;
      }
      set => this.SetAttribute("emsrc", value == FieldSource.LinkedLoan ? "link" : "");
    }

    /// <summary>
    /// Gets or sets the tooltip displayed when the mouse is hovered over the control.
    /// </summary>
    [Category("Appearance")]
    [Description("The text that appears when the user hovers the mouse over the control.")]
    public virtual string HoverText
    {
      get => this.HTMLElement.title ?? "";
      set
      {
        this.HTMLElement.title = value;
        this.NotifyPropertyChange();
      }
    }

    /// <summary>Gets/Sets the access mode for the field</summary>
    [Browsable(false)]
    public virtual FieldAccessMode AccessMode
    {
      get => this.accessMode;
      set
      {
        if (value == this.accessMode)
          return;
        this.accessMode = value;
        this.ApplyInteractiveState();
      }
    }

    /// <summary>Gets the list of events supported by this control.</summary>
    [Browsable(false)]
    public string[] SupportedEvents => Calendar.supportedEvents;

    /// <summary>Invokes the Click event on the control.</summary>
    /// <returns>This method is intended for use within the Encompass framework only.</returns>
    /// <exclude />
    public bool InvokeDateSelected(DateTime selectedDate)
    {
      this.OnDateSelected(new DateSelectedEventArgs(selectedDate));
      return true;
    }

    /// <summary>
    /// Overrides the function thate determines if the control should be displayed in interactive mode
    /// </summary>
    internal override bool AllowInteractivity
    {
      get => base.AllowInteractivity && this.accessMode == FieldAccessMode.NoRestrictions;
    }

    /// <summary>
    /// Raises the <see cref="E:EllieMae.Encompass.Forms.Calendar.DateSelected" /> event on the control.
    /// </summary>
    /// <param name="e">The event arguments passed to the event handlers.</param>
    protected virtual void OnDateSelected(DateSelectedEventArgs e)
    {
      this.dateSelected.Invoke((object) this, e);
    }

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      this.getImageElement().src = Calendar.getIconImageSource(this.Interactive);
      if (!((this.HTMLElement.title ?? "") == ""))
        return;
      this.HTMLElement.title = "Select a Date";
    }

    private FieldDescriptor getAssociatedField(string attrName)
    {
      return this.Form.GetFieldDescriptor(this.GetAttribute(attrName));
    }

    private void setAssociatedField(string attrName, FieldDescriptor field)
    {
      this.HTMLElement.setAttribute(attrName, (object) field.FieldID);
      this.NotifyPropertyChange();
    }

    internal override string RenderHTML()
    {
      return "<button" + this.GetBaseAttributes(false) + " tabIndex=\"-1\" hideFocus=\"true\" class=\"inputButtonImage\"><img src=\"" + Calendar.getIconImageSource(true) + "\" width=\"16\" height=\"16\" emimport=\"0\" onmouseover=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('.png', '-over.png'); }\" onmouseout=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('-over.png', '.png'); }\"></button>";
    }

    internal override void ChangeControlInteractiveState(bool interactive)
    {
      base.ChangeControlInteractiveState(interactive);
      this.getImageElement().src = Calendar.getIconImageSource(interactive);
    }

    private IHTMLImgElement getImageElement()
    {
      return (IHTMLImgElement) (HTMLHelper.GetChildWithTagName(this.HTMLElement, "img") ?? throw new Exception("The interior image element could not be found"));
    }

    private static string getIconImageSource(bool enabled)
    {
      return Control.ResolveInternalImagePath(enabled ? "calendar.png" : "calendar-disabled.png");
    }
  }
}
