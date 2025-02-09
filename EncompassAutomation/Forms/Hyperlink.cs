// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Hyperlink
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

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Represents a clickable hyperlink in the Form.</summary>
  [PropertyTab(typeof (EventsTab), PropertyTabScope.Component)]
  [ToolboxControl("Hyperlink")]
  public class Hyperlink : Label, ISupportsClickEvent, ISupportsEvents
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Click"
    };
    private ScopedEventHandler<EventArgs> click = new ScopedEventHandler<EventArgs>(nameof (Hyperlink), "Click");

    /// <summary>
    /// The Click event is fired when the user clicks the hyperlink.
    /// </summary>
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

    /// <summary>Constructor for a new Hyperlink control.</summary>
    public Hyperlink()
    {
    }

    internal Hyperlink(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    internal override string RenderHTML()
    {
      return "<a href=\"\" " + this.GetBaseAttributes() + " target=\"_blank\" tabIndex=\"-1\" hideFocus=\"true\">Enter your hyperlink here</a>";
    }

    /// <summary>Gets or sets the URL associated with the hyperlink.</summary>
    /// <remarks>This property can be left blank if you use the <see cref="E:EllieMae.Encompass.Forms.Hyperlink.Click" /> event to perform
    /// the desired action. If this property is set and you create a click event handler, the event
    /// handler will be triggered first and then the URL will be lanuched.</remarks>
    [Category("Behavior")]
    public string URL
    {
      get => string.Concat(this.HTMLElement.getAttribute("href", 2));
      set
      {
        if ((value ?? "") != "" && !value.ToLower().StartsWith("http://") && !value.ToLower().StartsWith("https://"))
          value = "http://" + value;
        ((DispHTMLAnchorElement) this.HTMLElement).href = value ?? "";
        this.NotifyPropertyChange();
      }
    }

    /// <summary>Gets the names of the events supported by this class.</summary>
    [Browsable(false)]
    public string[] SupportedEvents => Hyperlink.supportedEvents;

    /// <summary>Invokes the Click event on the object.</summary>
    /// <returns></returns>
    /// <remarks>This method is intended for use by the Encompass application only.</remarks>
    /// <exclude />
    public bool InvokeClick()
    {
      this.OnClick(EventArgs.Empty);
      return this.URL != "";
    }

    /// <summary>
    /// Raises the <see cref="E:EllieMae.Encompass.Forms.Hyperlink.Click" /> event.
    /// </summary>
    /// <param name="e">The event argument passed to the event handlers.</param>
    protected virtual void OnClick(EventArgs e) => this.click.Invoke((object) this, e);
  }
}
