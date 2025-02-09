// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Button
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
  /// <summary>Reprents a button control on an input form.</summary>
  /// <remarks>The button control is used to either trigger a default <see cref="P:EllieMae.Encompass.Forms.Button.Action" />
  /// when clicked or to invoke user-desfined code via the <see cref="E:EllieMae.Encompass.Forms.Button.Click" /> event.</remarks>
  [PropertyTab(typeof (EventsTab), PropertyTabScope.Component)]
  [ToolboxControl("Button")]
  public class Button : ContentControl, ISupportsClickEvent, ISupportsEvents, IActionable
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Click"
    };
    private ScopedEventHandler<EventArgs> click = new ScopedEventHandler<EventArgs>(nameof (Button), "Click");

    /// <summary>Event fired when the user clicks the button.</summary>
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

    /// <summary>Default cosntructor for a new Button control.</summary>
    public Button()
    {
    }

    internal Button(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    /// <summary>Gets or sets the text on the button.</summary>
    [Category("Appearance")]
    public string Text
    {
      get => ((IHTMLInputElement) this.HTMLElement).value ?? "";
      set
      {
        ((IHTMLInputElement) this.HTMLElement).value = value ?? "";
        this.NotifyPropertyChange();
      }
    }

    /// <summary>
    /// Gets or sets whether the button will automatically size itself to it contents.
    /// </summary>
    [Category("Appearance")]
    [Description("Controls whether the button will automatically size to fit its contents.")]
    public bool AutoSize
    {
      get => string.Concat(this.HTMLElement.style.width) == "";
      set
      {
        if (value)
        {
          this.HTMLElement.style.width = (object) "";
          this.HTMLElement.style.height = (object) "";
        }
        else
        {
          this.HTMLElement.style.width = this.HTMLElement2.currentStyle.width;
          this.HTMLElement.style.height = this.HTMLElement2.currentStyle.height;
        }
        this.NotifyPropertyChange();
        this.Form.OnResize(new FormEventArgs((Control) this));
      }
    }

    /// <summary>
    /// Gets or sets the pre-defined action associated with the button.
    /// </summary>
    /// <remarks>For a list of the possible pre-defined actions which can be associated with
    /// a button, see the Encompass Input Form Builder documentation.</remarks>
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
    /// Gets or sets the text that appears when the mouse is hovered over the button.
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
      return "<input type=\"button\" class=\"inputButton\" " + this.GetBaseAttributes(false) + " tabIndex=\"-1\" value=\"" + this.ControlID + "\"></input>";
    }

    /// <summary>
    /// Gets the list of events supported by the Button control.
    /// </summary>
    [Browsable(false)]
    public string[] SupportedEvents => Button.supportedEvents;

    /// <summary>
    /// This function is intended for use by the Encompass framework only.
    /// </summary>
    /// <returns></returns>
    /// <exclude />
    public bool InvokeClick()
    {
      this.OnClick(EventArgs.Empty);
      return true;
    }

    /// <summary>Raises the Click event on the Button control.</summary>
    /// <param name="e">The EventArgs for the event.</param>
    protected virtual void OnClick(EventArgs e) => this.click.Invoke((object) this, e);
  }
}
