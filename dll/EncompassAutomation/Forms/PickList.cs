// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.PickList
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

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
    private bool useForEditableDropdownField;

    [Category("Behavior")]
    public event EventHandler Popup
    {
      add
      {
        if (value == null)
          return;
        this.popup.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.popup.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    [Category("Behavior")]
    public event ItemSelectedEventHandler ItemSelected
    {
      add
      {
        if (value == null)
          return;
        this.itemSelected.Add(new ScopedEventHandler<ItemSelectedEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.itemSelected.Remove(new ScopedEventHandler<ItemSelectedEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public PickList()
    {
      this.options = new DropdownOptionCollection((IDropdownOptionContainer) new PickList.PickListOptionContainer(this));
    }

    internal PickList(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      this.options = new DropdownOptionCollection((IDropdownOptionContainer) new PickList.PickListOptionContainer(this));
    }

    [Category("Data")]
    [Editor(typeof (DropdownOptionsEditor), typeof (UITypeEditor))]
    public DropdownOptionCollection Options => this.options;

    [Category("Appearance")]
    public string Title
    {
      get => this.GetAttribute("emtitle");
      set => this.SetAttribute("emtitle", value ?? "");
    }

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
      for (int index = 0; index < children.length; ++index)
      {
        IHTMLElement ihtmlElement = (IHTMLElement) children.item((object) index, (object) null);
        if (string.Compare(ihtmlElement.tagName, "emoption", true) == 0)
          arrayList.Add((object) new DropdownOption(string.Concat(ihtmlElement.getAttribute("text", 2)), string.Concat(ihtmlElement.getAttribute("value", 2))));
      }
      return (DropdownOption[]) arrayList.ToArray(typeof (DropdownOption));
    }

    private void RenderOptionsList(DropdownOption[] options)
    {
      foreach (IHTMLDOMNode ihtmldomNode in (IEnumerable) HTMLHelper.GetChildrenWithTagName(this.HTMLElement, "emoption"))
        ihtmldomNode.parentNode.removeChild(ihtmldomNode);
      foreach (DropdownOption option in options)
      {
        IHTMLElement element = ((DispHTMLDocument) this.GetHTMLDocument()).createElement("emoption");
        element.setAttribute("text", (object) option.Text, 1);
        element.setAttribute("value", (object) option.Value, 1);
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

    public void InvokePopup() => this.OnPopup(EventArgs.Empty);

    protected void OnPopup(EventArgs e) => this.popup((object) this, e);

    public void InvokeItemSelected(DropdownOption option)
    {
      this.OnItemSelected(new ItemSelectedEventArgs(option));
    }

    protected void OnItemSelected(ItemSelectedEventArgs e) => this.itemSelected((object) this, e);

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
