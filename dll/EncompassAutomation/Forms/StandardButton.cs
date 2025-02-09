// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.StandardButton
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [PropertyTab(typeof (EventsTab), PropertyTabScope.Component)]
  [ToolboxControl("Standard Button")]
  public class StandardButton : RuntimeControl, ISupportsClickEvent, ISupportsEvents, IActionable
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Click"
    };
    private ScopedEventHandler<EventArgs> click = new ScopedEventHandler<EventArgs>(nameof (StandardButton), "Click");

    [Category("Behavior")]
    public event EventHandler Click
    {
      add
      {
        if (value == null)
          return;
        this.click.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.click.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public StandardButton()
    {
    }

    internal StandardButton(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    [Category("Appearance")]
    public StandardButtonType ButtonType
    {
      get => StandardButton.stringToButtonType(this.GetAttribute("buttonType"));
      set
      {
        this.SetAttribute("buttonType", StandardButton.buttonTypeToString(value));
        this.getImageElement().src = this.getIconImageSource(value, this.Enabled);
        this.HoverText = StandardButton.getDefaultHoverText(value);
        this.NotifyPropertyChange();
      }
    }

    [Category("Behavior")]
    public string Action
    {
      get => this.GetAttribute("emid");
      set
      {
        this.SetAttribute("emid", value);
        this.NotifyPropertyChange();
      }
    }

    [Category("Appearance")]
    [Description("The text that appears when the user hovers the mouse over the control.")]
    public virtual string HoverText
    {
      get
      {
        string hoverText = this.HTMLElement.title ?? "";
        if (hoverText == "")
          hoverText = StandardButton.getDefaultHoverText(this.ButtonType);
        return hoverText;
      }
      set
      {
        this.HTMLElement.title = value;
        this.NotifyPropertyChange();
      }
    }

    internal override string RenderHTML()
    {
      return "<button class=\"inputButtonImage\" " + this.GetBaseAttributes(false) + " hideFocus=\"true\" tabIndex=\"-1\" style=\"width: 16px; height: 16px\"><img emimport=\"0\" src=\"" + this.getIconImageSource(StandardButtonType.Edit, true) + " width=\"16\" height=\"16\" onmouseover=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('.png', '-over.png'); }\" onmouseout=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('-over.png', '.png'); }\"/></button>";
    }

    [Browsable(false)]
    public string[] SupportedEvents => StandardButton.supportedEvents;

    public bool InvokeClick()
    {
      this.OnClick(EventArgs.Empty);
      return true;
    }

    protected virtual void OnClick(EventArgs e) => this.click((object) this, e);

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      this.getImageElement().src = this.getIconImageSource(this.ButtonType, this.Enabled);
      if (!((this.HTMLElement.title ?? "") == ""))
        return;
      this.HTMLElement.title = StandardButton.getDefaultHoverText(this.ButtonType);
    }

    internal override void ChangeControlInteractiveState(bool interactive)
    {
      base.ChangeControlInteractiveState(interactive);
      this.getImageElement().src = this.getIconImageSource(this.ButtonType, this.Enabled);
    }

    private IHTMLImgElement getImageElement()
    {
      return (IHTMLImgElement) (HTMLHelper.GetChildWithTagName(this.HTMLElement, "img") ?? throw new Exception("The interior image element could not be found"));
    }

    private static string buttonTypeToString(StandardButtonType buttonType)
    {
      switch (buttonType)
      {
        case StandardButtonType.Edit:
          return "edit";
        case StandardButtonType.Lookup:
          return "zoom";
        case StandardButtonType.Clear:
          return "delete";
        case StandardButtonType.Refresh:
          return "refresh";
        case StandardButtonType.Help:
          return "help";
        case StandardButtonType.Calendar:
          return "calendar";
        case StandardButtonType.Alert:
          return "alert";
        default:
          return buttonType.ToString().ToLower();
      }
    }

    private static StandardButtonType stringToButtonType(string name)
    {
      switch (name.ToLower())
      {
        case "alert":
          return StandardButtonType.Alert;
        case "calendar":
          return StandardButtonType.Calendar;
        case "delete":
          return StandardButtonType.Clear;
        case "edit":
          return StandardButtonType.Edit;
        case "help":
          return StandardButtonType.Help;
        case "refresh":
          return StandardButtonType.Refresh;
        case "zoom":
          return StandardButtonType.Lookup;
        default:
          try
          {
            return (StandardButtonType) Enum.Parse(typeof (StandardButtonType), name, true);
          }
          catch
          {
            return StandardButtonType.Edit;
          }
      }
    }

    private string getIconImageSource(StandardButtonType buttonType, bool enabled)
    {
      string imageName;
      switch (buttonType)
      {
        case StandardButtonType.Edit:
          imageName = enabled ? "edit.png" : "edit-disabled.png";
          break;
        case StandardButtonType.Lookup:
          imageName = enabled ? "search.png" : "search-disabled.png";
          break;
        case StandardButtonType.Clear:
          imageName = enabled ? "delete.png" : "delete-disabled.png";
          break;
        case StandardButtonType.Refresh:
          imageName = enabled ? "refresh.png" : "refresh-disabled.png";
          break;
        case StandardButtonType.Help:
          imageName = enabled ? "help.png" : "help-disabled.png";
          break;
        case StandardButtonType.Calendar:
          imageName = enabled ? "calendar.png" : "calendar-disabled.png";
          break;
        case StandardButtonType.Alert:
          imageName = enabled ? "alert.png" : "alert.png";
          break;
        default:
          throw new ArgumentException("Invalid button type specified");
      }
      return Control.ResolveInternalImagePath(imageName);
    }

    private static string getDefaultHoverText(StandardButtonType buttonType)
    {
      switch (buttonType)
      {
        case StandardButtonType.Edit:
          return "Edit Field Value";
        case StandardButtonType.Lookup:
          return "Lookup Value";
        case StandardButtonType.Clear:
          return "Clear Values";
        case StandardButtonType.Refresh:
          return "Refresh";
        case StandardButtonType.Help:
          return "Help";
        case StandardButtonType.Calendar:
          return "Select a Date";
        default:
          return "";
      }
    }
  }
}
