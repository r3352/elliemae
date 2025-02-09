// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Hyperlink
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
  [ToolboxControl("Hyperlink")]
  public class Hyperlink : Label, ISupportsClickEvent, ISupportsEvents
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Click"
    };
    private ScopedEventHandler<EventArgs> click = new ScopedEventHandler<EventArgs>(nameof (Hyperlink), "Click");

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

    [Browsable(false)]
    public string[] SupportedEvents => Hyperlink.supportedEvents;

    public bool InvokeClick()
    {
      this.OnClick(EventArgs.Empty);
      return this.URL != "";
    }

    protected virtual void OnClick(EventArgs e) => this.click((object) this, e);
  }
}
