// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.RadioButton
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
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represents a radio button control on an Encompass input form.
  /// </summary>
  [ToolboxControl("Radio Button")]
  public class RadioButton : FieldControl, ISupportsClickEvent, ISupportsEvents
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Click"
    };
    private ScopedEventHandler<EventArgs> click = new ScopedEventHandler<EventArgs>(nameof (RadioButton), "Click");
    private IHTMLElement textElement;

    /// <summary>
    /// The Click event is fired when the user selects the radio button.
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

    /// <summary>Constructor for a new RadioButton control.</summary>
    public RadioButton()
    {
    }

    internal RadioButton(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    /// <summary>Gets or sets the Field associated with this control.</summary>
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

    /// <summary>
    /// Gets or sets the text associated with the Radio Button.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the radio button group to which this control belongs.
    /// </summary>
    /// <remarks>The GroupName of a radio button determine how radio buttons work together.
    /// At most only one radio button can be selected within a given group.</remarks>
    [Category("Data")]
    public string GroupName
    {
      get => string.Concat(this.FieldElement.getAttribute("emgroup", 1));
      set
      {
        if (value.IndexOf("\"") >= 0)
          throw new ArgumentException("The group name cannot contain quotes");
        this.FieldElement.setAttribute("emgroup", (object) value);
        this.NotifyPropertyChange();
      }
    }

    /// <summary>
    /// The value the underlying field will take on if this radio button is selected.
    /// </summary>
    [Category("Data")]
    [Editor(typeof (CheckedValueEditor), typeof (UITypeEditor))]
    public string CheckedValue
    {
      get => string.Concat(this.FieldElement.getAttribute("value"));
      set
      {
        if (!this.Field.Options.IsValueAllowed(value))
          throw new ArgumentException("The only valid values for this property are: " + string.Join(", ", this.Field.Options.GetValues().ToArray()));
        this.FieldElement.setAttribute(nameof (value), (object) value);
        this.NotifyPropertyChange();
      }
    }

    /// <summary>
    /// Gets the current value of the radio button set based on the checked state
    /// of the object.
    /// </summary>
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

    /// <summary>Gets/Sets the checked state of the radio buttons</summary>
    [Browsable(false)]
    public bool Checked
    {
      get => ((IHTMLInputElement) this.FieldElement).@checked;
      set => this.ApplyValueToControl(value ? this.CheckedValue : "");
    }

    /// <summary>Gets the list of events supported by this control.</summary>
    public override string[] SupportedEvents
    {
      get => Control.MergeEvents(base.SupportedEvents, RadioButton.supportedEvents);
    }

    internal override void DisplayValue(string value)
    {
      ((IHTMLInputElement) this.FieldElement).@checked = value == this.CheckedValue;
    }

    internal override void HideValue() => ((IHTMLInputElement) this.FieldElement).@checked = false;

    /// <summary>Invokes the Click event on the control.</summary>
    /// <returns></returns>
    /// <remarks>This method is intended for use within Encompass only.</remarks>
    /// <exclude />
    public bool InvokeClick()
    {
      this.OnClick(EventArgs.Empty);
      return true;
    }

    /// <summary>
    /// Raises the <see cref="E:EllieMae.Encompass.Forms.RadioButton.Click" /> event.
    /// </summary>
    /// <param name="e">The event parameters passed to the event handlers.</param>
    protected virtual void OnClick(EventArgs e) => this.click.Invoke((object) this, e);

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
        this.textElement.setAttribute("labelId", (object) newValue);
      base.ChangeControlID(oldValue, newValue);
      if (this.TextElement == null)
        return;
      this.FieldElement.id = this.FormElementID;
      this.textElement.setAttribute("for", (object) this.FormElementID);
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      this.textElement = this.FindTextElement(controlElement);
      if (this.textElement == null)
        return;
      this.FieldElement.id = this.FormElementID;
      this.textElement.setAttribute("for", (object) this.FormElementID);
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
