// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ImageButton
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
  /// Provides a button which can be represented using an arbitary image.
  /// </summary>
  [PropertyTab(typeof (EventsTab), PropertyTabScope.Component)]
  [ToolboxControl("Image Button")]
  public class ImageButton : RuntimeControl, ISupportsClickEvent, ISupportsEvents, IActionable
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Click"
    };
    private ScopedEventHandler<EventArgs> click = new ScopedEventHandler<EventArgs>(nameof (ImageButton), "Click");

    /// <summary>
    /// The Click event is fired when the user clicks the ImageButton control.
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

    /// <summary>Constructor for a new ImageButton control.</summary>
    public ImageButton()
    {
    }

    internal ImageButton(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    /// <summary>Gets ot sets the source location of the image.</summary>
    [Category("Appearance")]
    [Editor(typeof (FileOpenTypeEditor), typeof (UITypeEditor))]
    public string Source
    {
      get => string.Concat(((IHTMLElement) this.getImageElement()).getAttribute("src", 2));
      set
      {
        this.getImageElement().src = value;
        this.NotifyPropertyChange();
      }
    }

    /// <summary>Sets the action associated with the button.</summary>
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

    /// <summary>
    /// Gets or sets the tooltip text displayed when the mouse hovers over the control.
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

    internal override string RenderHTML()
    {
      return "<button class=\"inputButtonImage\" " + this.GetBaseAttributes(false) + " hideFocus=\"true\" tabIndex=\"-1\" style=\"width: 16px; height: 16px\"><img/></button>";
    }

    /// <summary>Returns the list of events supported by this control.</summary>
    [Browsable(false)]
    public string[] SupportedEvents => ImageButton.supportedEvents;

    /// <summary>Invokes the Click method on the control.</summary>
    /// <returns></returns>
    /// <remarks>This method is intended for use by the Encompass application only.</remarks>
    /// <exclude />
    public bool InvokeClick()
    {
      this.OnClick(EventArgs.Empty);
      return true;
    }

    /// <summary>
    /// Raises the <see cref="E:EllieMae.Encompass.Forms.ImageButton.Click" /> event.
    /// </summary>
    /// <param name="e">The parameters passed to the event handlers.</param>
    protected virtual void OnClick(EventArgs e) => this.click.Invoke((object) this, e);

    private IHTMLImgElement getImageElement()
    {
      return (IHTMLImgElement) (HTMLHelper.GetChildWithTagName(this.HTMLElement, "img") ?? throw new Exception("The interior image element could not be found"));
    }
  }
}
