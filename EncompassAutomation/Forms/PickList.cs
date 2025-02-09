// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.PickList
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Provides a dynamic list of options for the user to choose from in a modal pop-up.
  /// </summary>
  /// <remarks>To make the PickList control useful you must add an event handler to the
  /// <see cref="E:EllieMae.Encompass.Forms.PickList.ItemSelected" /> event. Use the selected item to populate a loan field or
  /// whatever other behavior you need to achieve.</remarks>
  [ToolboxControl("Pick List")]
  [PropertyTab(typeof (EventsTab), PropertyTabScope.Component)]
  public class PickList : RuntimeControl, ISupportsPopupEvent, ISupportsEvents
  {
    private static readonly string[] supportedEvents = new string[2]
    {
      "Popup",
      "ItemSelected"
    };
    private ScopedEventHandler<EventArgs> popup = new ScopedEventHandler<EventArgs>(nameof (PickList), "Popup");
    private ScopedEventHandler<ItemSelectedEventArgs> itemSelected = new ScopedEventHandler<ItemSelectedEventArgs>(nameof (PickList), "ItemSelected");
    private DropdownOptionCollection options;
    private FieldControl boundControl;
    /// <summary>
    /// A flag to set the picklist size that is matched with an editable dropdown field.
    /// </summary>
    private bool useForEditableDropdownField;

    /// <summary>
    /// The Popup event is fired when the user clicks the control.
    /// </summary>
    /// <remarks>Use this event to dynamically generate the list of options that are available
    /// to the user (if not defined at design time).</remarks>
    [Category("Behavior")]
    public event EventHandler Popup
    {
      add
      {
        if (value == null)
          return;
        this.popup.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.popup.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// The ItemSelected event is fired when the user selects an item from the list.
    /// </summary>
    /// <remarks>Use this event to make the PickList functional, e.g. to copy the selected value
    /// to a loan field or use it as the input for another operation.</remarks>
    [Category("Behavior")]
    public event ItemSelectedEventHandler ItemSelected
    {
      add
      {
        if (value == null)
          return;
        this.itemSelected.Add(new ScopedEventHandler<ItemSelectedEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.itemSelected.Remove(new ScopedEventHandler<ItemSelectedEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>Constructor for a new PickList control.</summary>
    public PickList()
    {
      this.options = new DropdownOptionCollection((IDropdownOptionContainer) new PickList.PickListOptionContainer(this));
    }

    internal PickList(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      this.options = new DropdownOptionCollection((IDropdownOptionContainer) new PickList.PickListOptionContainer(this));
    }

    /// <summary>
    /// Gets the collection of options to be displayed in the dropdown box.
    /// </summary>
    [Category("Data")]
    [Editor(typeof (DropdownOptionsEditor), typeof (UITypeEditor))]
    public DropdownOptionCollection Options => this.options;

    /// <summary>
    /// Gets or sets the title that will appear on the Pick List box when displayed.
    /// </summary>
    [Category("Appearance")]
    public string Title
    {
      get => this.GetAttribute("emtitle");
      set => this.SetAttribute("emtitle", value ?? "");
    }

    /// <summary>
    /// Gets or sets the Control to which the PickLis is bounds
    /// </summary>
    [Browsable(false)]
    public FieldControl BoundControl
    {
      get => this.boundControl;
      set => this.boundControl = value;
    }

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      this.getImageElement().src = Control.ResolveInternalImagePath("dropdown.png");
    }

    internal void BindToControl(FieldControl control) => this.boundControl = control;

    private DropdownOption[] ParseOptionsList()
    {
      ArrayList arrayList = new ArrayList();
      IHTMLElementCollection children = (IHTMLElementCollection) this.HTMLElement.children;
      for (int name = 0; name < children.length; ++name)
      {
        IHTMLElement htmlElement = (IHTMLElement) children.item((object) name);
        if (string.Compare(htmlElement.tagName, "emoption", true) == 0)
          arrayList.Add((object) new DropdownOption(string.Concat(htmlElement.getAttribute("text", 2)), string.Concat(htmlElement.getAttribute("value", 2))));
      }
      return (DropdownOption[]) arrayList.ToArray(typeof (DropdownOption));
    }

    private void RenderOptionsList(DropdownOption[] options)
    {
      foreach (IHTMLDOMNode oldChild in (IEnumerable) HTMLHelper.GetChildrenWithTagName(this.HTMLElement, "emoption"))
        oldChild.parentNode.removeChild(oldChild);
      foreach (DropdownOption option in options)
      {
        IHTMLElement element = this.GetHTMLDocument().createElement("emoption");
        element.setAttribute("text", (object) option.Text);
        element.setAttribute("value", (object) option.Value);
        ((IHTMLDOMNode) this.HTMLElement).appendChild((IHTMLDOMNode) element);
      }
    }

    internal bool UseForEditableDropdownField
    {
      set => this.useForEditableDropdownField = value;
    }

    internal override string RenderHTML()
    {
      return this.useForEditableDropdownField ? "<button" + this.GetBaseAttributes(false) + "tabIndex=\"-1\" hideFocus=\"true\" class=\"inputButtonImage\"><img src=\"" + Control.ResolveInternalImagePath("dropdown.png") + "\" width=\"20\" height=\"22\" emimport=\"0\"></button>" : "<button" + this.GetBaseAttributes(false) + "tabIndex=\"-1\" hideFocus=\"true\" class=\"inputButtonImage\"><img src=\"" + Control.ResolveInternalImagePath("dropdown.png") + "\" width=\"17\" height=\"17\" emimport=\"0\"></button>";
    }

    private IHTMLImgElement getImageElement()
    {
      return (IHTMLImgElement) (HTMLHelper.GetChildWithTagName(this.HTMLElement, "img") ?? throw new Exception("The interior image element could not be found"));
    }

    /// <summary>Invokes the Popup event on the object.</summary>
    /// <remarks> This method is meant for internal use within Encompass.</remarks>
    /// <exclude />
    public void InvokePopup() => this.OnPopup(EventArgs.Empty);

    /// <summary>
    /// Raises the <see cref="E:EllieMae.Encompass.Forms.PickList.Popup" /> event.
    /// </summary>
    /// <param name="e">The parameters passed to the event handlers.</param>
    protected void OnPopup(EventArgs e) => this.popup.Invoke((object) this, e);

    /// <summary>Invokes the ItemSelected event on the object.</summary>
    /// <remarks>This method is meant for internal use within Encompass.</remarks>
    /// <exclude />
    public void InvokeItemSelected(DropdownOption option)
    {
      this.OnItemSelected(new ItemSelectedEventArgs(option));
    }

    /// <summary>
    /// Raises the <see cref="E:EllieMae.Encompass.Forms.PickList.ItemSelected" /> event.
    /// </summary>
    /// <param name="e">The parameters passed to the event handlers.</param>
    protected void OnItemSelected(ItemSelectedEventArgs e)
    {
      this.itemSelected.Invoke((object) this, e);
    }

    /// <summary>Gets the list of events supported by this class.</summary>
    [Browsable(false)]
    public virtual string[] SupportedEvents => PickList.supportedEvents;

    private class PickListOptionContainer : IDropdownOptionContainer
    {
      private PickList parentControl;

      public PickListOptionContainer(PickList parentControl) => this.parentControl = parentControl;

      public DropdownOption[] ParseOptionsList() => this.parentControl.ParseOptionsList();

      public void RenderOptionsList(DropdownOption[] options)
      {
        this.parentControl.RenderOptionsList(options);
      }

      public bool AllowEditValues => true;

      public bool AllowRearrangeValues => true;
    }
  }
}
