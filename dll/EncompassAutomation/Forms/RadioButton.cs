// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.RadioButton
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Radio Button")]
  public class RadioButton : FieldControl, ISupportsClickEvent, ISupportsEvents
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Click"
    };
    private ScopedEventHandler<EventArgs> click = new ScopedEventHandler<EventArgs>(nameof (RadioButton), "Click");
    private IHTMLElement textElement;

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

    public RadioButton()
    {
    }

    internal RadioButton(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    public override FieldDescriptor Field
    {
      get => base.Field;
      set
      {
        if (this.GroupName == this.Field.FieldID || this.GroupName == "")
          this.GroupName = value.FieldID;
        if (!value.Options.IsValueAllowed(this.CheckedValue))
          this.CheckedValue = "";
        base.Field = value;
      }
    }

    [Category("Appearance")]
    public string Text
    {
      get => this.TextElement == null ? "(Not Available)" : this.TextElement.innerText ?? "";
      set
      {
        if (this.TextElement == null)
          return;
        this.TextElement.innerText = value;
        this.NotifyPropertyChange();
      }
    }

    [Category("Data")]
    public string GroupName
    {
      get => string.Concat(this.FieldElement.getAttribute("emgroup", 1));
      set
      {
        if (value.IndexOf("\"") >= 0)
          throw new ArgumentException("The group name cannot contain quotes");
        this.FieldElement.setAttribute("emgroup", (object) value, 1);
        this.NotifyPropertyChange();
      }
    }

    [Category("Data")]
    [Editor(typeof (CheckedValueEditor), typeof (UITypeEditor))]
    public string CheckedValue
    {
      get => string.Concat(this.FieldElement.getAttribute("value", 0));
      set
      {
        if (!this.Field.Options.IsValueAllowed(value))
          throw new ArgumentException("The only valid values for this property are: " + string.Join(", ", this.Field.Options.GetValues().ToArray()));
        this.FieldElement.setAttribute(nameof (value), (object) value, 1);
        this.NotifyPropertyChange();
      }
    }

    public override string Value
    {
      get
      {
        string groupName = this.GroupName;
        foreach (RadioButton radioButton in this.Form.FindControlsByType(typeof (RadioButton)))
        {
          if (radioButton.Checked && radioButton.GroupName == groupName)
            return radioButton.CheckedValue;
        }
        return "";
      }
    }

    [Browsable(false)]
    public bool Checked
    {
      get => ((IHTMLInputElement) this.FieldElement).@checked;
      set => this.ApplyValueToControl(value ? this.CheckedValue : "");
    }

    public override string[] SupportedEvents
    {
      get => Control.MergeEvents(base.SupportedEvents, RadioButton.supportedEvents);
    }

    internal override void DisplayValue(string value)
    {
      ((IHTMLInputElement) this.FieldElement).@checked = value == this.CheckedValue;
    }

    internal override void HideValue() => ((IHTMLInputElement) this.FieldElement).@checked = false;

    public bool InvokeClick()
    {
      this.OnClick(EventArgs.Empty);
      return true;
    }

    protected virtual void OnClick(EventArgs e) => this.click((object) this, e);

    internal IHTMLElement TextElement
    {
      get
      {
        this.EnsureAttached();
        return this.textElement;
      }
    }

    internal override bool ReattachRequired()
    {
      return base.ReattachRequired() || Control.IsDetached(this.textElement);
    }

    internal virtual IHTMLElement FindTextElement(IHTMLElement controlElement)
    {
      return HTMLHelper.FindElementWithAttribute(controlElement, "labelId", this.ControlID);
    }

    internal override void ChangeControlID(string oldValue, string newValue)
    {
      if (this.TextElement != null)
        this.textElement.setAttribute("labelId", (object) newValue, 1);
      base.ChangeControlID(oldValue, newValue);
      if (this.TextElement == null)
        return;
      this.FieldElement.id = this.FormElementID;
      this.textElement.setAttribute("for", (object) this.FormElementID, 1);
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      this.textElement = this.FindTextElement(controlElement);
      if (this.textElement == null)
        return;
      this.FieldElement.id = this.FormElementID;
      this.textElement.setAttribute("for", (object) this.FormElementID, 1);
    }

    internal override string RenderHTML()
    {
      return "<span" + this.GetBaseAttributes() + "><input id=\"" + this.FormElementID + "\" type=\"radio\" value=\"1\"" + this.GetBaseFieldAttributes() + "/><label for=\"" + this.FormElementID + "\" labelId=\"" + this.ControlID + "\">Your label here</span></span>";
    }

    private string FormElementID => Control.MangleControlID(this.ControlID, "Ctrl");

    internal static string PerformGroupNameReplacements(string html)
    {
      return new Regex("<input([^>]+)type=\"?radio\"?[^>]*>", RegexOptions.IgnoreCase).Replace(html, new MatchEvaluator(RadioButton.evalRegexRadioMatch));
    }

    private static string evalRegexRadioMatch(Match m)
    {
      return Regex.Replace(Regex.Replace(m.Value, "\\sname=((\".*?\")|([^>\\s]*))", "", RegexOptions.IgnoreCase | RegexOptions.Singleline), "emgroup=\"(.*?)\"", "emgroup=\"$1\" name=\"$1\"", RegexOptions.IgnoreCase);
    }
  }
}
