// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Button
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
  [ToolboxControl("Button")]
  public class Button : ContentControl, ISupportsClickEvent, ISupportsEvents, IActionable
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Click"
    };
    private ScopedEventHandler<EventArgs> click = new ScopedEventHandler<EventArgs>(nameof (Button), "Click");

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

    public Button()
    {
    }

    internal Button(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

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

    [Browsable(false)]
    public string[] SupportedEvents => Button.supportedEvents;

    public bool InvokeClick()
    {
      this.OnClick(EventArgs.Empty);
      return true;
    }

    protected virtual void OnClick(EventArgs e) => this.click((object) this, e);
  }
}
