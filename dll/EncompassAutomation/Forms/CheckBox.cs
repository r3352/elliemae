// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.CheckBox
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
using System.Drawing;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Check Box")]
  public class CheckBox : FieldControl, ISupportsClickEvent, ISupportsEvents
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Click"
    };
    private ScopedEventHandler<EventArgs> click = new ScopedEventHandler<EventArgs>(nameof (CheckBox), "Click");
    private IHTMLElement textElement;
    private Image hiddenImage;

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

    public CheckBox()
    {
    }

    internal CheckBox(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    public override FieldDescriptor Field
    {
      get => base.Field;
      set
      {
        if (!value.Options.IsValueAllowed(this.CheckedValue))
          this.CheckedValue = "";
        if (!value.Options.IsValueAllowed(this.UncheckedValue))
          this.UncheckedValue = "";
        if (value.Options.Count > 2)
          this.BehaveAsRadio = true;
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
    [Editor(typeof (CheckedValueEditor), typeof (UITypeEditor))]
    public string CheckedValue
    {
      get => ((IHTMLInputElement) this.FieldElement).value ?? "";
      set
      {
        value = value ?? "";
        if (!this.Field.Options.IsValueAllowed(value))
          throw new ArgumentException("The only valid values for this property are: " + string.Join(", ", this.Field.Options.GetValues().ToArray()));
        if (value != "" && value == this.UncheckedValue)
          this.UncheckedValue = "";
        ((IHTMLInputElement) this.FieldElement).value = value;
        this.NotifyPropertyChange();
      }
    }

    [Category("Data")]
    [Editor(typeof (CheckedValueEditor), typeof (UITypeEditor))]
    public string UncheckedValue
    {
      get
      {
        if (this.BehaveAsRadio)
          return "";
        if (this.FieldElement.getAttribute("emunval", 1) is string attribute)
          return attribute;
        return this.CheckedValue == "Y" ? "N" : "";
      }
      set
      {
        value = value ?? "";
        if (this.BehaveAsRadio && value != "")
          throw new InvalidOperationException("A checkbox with the BehaveAsRadio property set to True cannot be assigned an UncheckedValue.");
        if (!this.Field.Options.IsValueAllowed(value))
          throw new ArgumentException("The only valid values for this property are: " + string.Join(", ", this.Field.Options.GetValues().ToArray()));
        if (value != "" && value == this.CheckedValue)
          this.CheckedValue = "";
        this.FieldElement.setAttribute("emunval", (object) value, 1);
        this.NotifyPropertyChange();
      }
    }

    [Category("Behavior")]
    public bool BehaveAsRadio
    {
      get => string.Concat(this.FieldElement.getAttribute("radio", 1)) == "Y";
      set
      {
        this.FieldElement.setAttribute("radio", value ? (object) "Y" : (object) "", 1);
        if (value)
          this.UncheckedValue = "";
        this.NotifyPropertyChange();
      }
    }

    [Browsable(false)]
    public bool Checked
    {
      get => ((IHTMLInputElement) this.FieldElement).@checked;
      set => this.ApplyValueToControl(value ? this.CheckedValue : this.UncheckedValue);
    }

    [Browsable(false)]
    public override string Value
    {
      get
      {
        if (this.Checked)
          return this.CheckedValue;
        return this.BehaveAsRadio ? this.getRadioButtonValue() : this.UncheckedValue;
      }
    }

    public override string[] SupportedEvents
    {
      get => Control.MergeEvents(base.SupportedEvents, CheckBox.supportedEvents);
    }

    public bool InvokeClick()
    {
      this.OnClick(EventArgs.Empty);
      return true;
    }

    protected virtual void OnClick(EventArgs e) => this.click((object) this, e);

    internal override void DisplayValue(string value)
    {
      this.removeHiddenImage();
      ((IHTMLInputElement) this.FieldElement).@checked = value == this.CheckedValue;
    }

    internal override void HideValue()
    {
      this.createHiddenImage();
      ((IHTMLInputElement) this.FieldElement).@checked = false;
    }

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
      try
      {
        if (((IHTMLLabelElement) this.textElement).htmlFor != this.FormElementID)
          ((IHTMLLabelElement) this.textElement).htmlFor = this.FormElementID;
      }
      catch (Exception ex)
      {
      }
      this.FieldElement.id = this.FormElementID;
      this.textElement.setAttribute("for", (object) this.FormElementID, 1);
    }

    internal override string RenderHTML()
    {
      return "<span" + this.GetBaseAttributes() + "><input id=\"" + this.FormElementID + "\" class=\"checkboxField\" type=\"checkbox\" value=\"Y\"" + this.GetBaseFieldAttributes() + "/><label for=\"" + this.FormElementID + "\" labelId=\"" + this.ControlID + "\">Your label here</label></span>";
    }

    private string FormElementID => Control.MangleControlID(this.ControlID, "Ctrl");

    private string getRadioButtonValue()
    {
      FieldDescriptor field = this.Field;
      if (field == FieldDescriptor.Empty)
        return "";
      foreach (CheckBox checkBox in this.Form.FindControlsByType(typeof (CheckBox)))
      {
        if (checkBox.Checked && checkBox.BehaveAsRadio && checkBox.Field.Equals((object) field))
          return checkBox.CheckedValue;
      }
      return "";
    }

    private void createHiddenImage()
    {
      if (this.hiddenImage != null)
        return;
      Point absolutePosition = this.AbsolutePosition;
      this.hiddenImage = new Image();
      this.GetContainer().Controls.Insert((Control) this.hiddenImage);
      this.hiddenImage.Source = Control.ResolveInternalImagePath("asterisk.gif");
      this.hiddenImage.Size = new Size(5, 4);
      this.hiddenImage.AbsolutePosition = new Point(absolutePosition.X + 8, absolutePosition.Y + 8);
      this.hiddenImage.ZIndex = this.ZIndex;
    }

    private void removeHiddenImage()
    {
      if (this.hiddenImage == null)
        return;
      this.hiddenImage.Delete();
      this.hiddenImage = (Image) null;
    }
  }
}
