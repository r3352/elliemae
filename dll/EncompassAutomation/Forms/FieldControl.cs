// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.FieldControl
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [PropertyTab(typeof (EventsTab), PropertyTabScope.Component)]
  public abstract class FieldControl : 
    ContentControl,
    ISupportsDataBindEvent,
    ISupportsEvents,
    ISupportsFocusEvent
  {
    private static readonly string[] supportedEvents = new string[4]
    {
      "DataBind",
      "DataCommit",
      "FocusIn",
      "FocusOut"
    };
    private ScopedEventHandler<DataBindEventArgs> dataBind = new ScopedEventHandler<DataBindEventArgs>(nameof (FieldControl), "DataBind");
    private ScopedEventHandler<DataCommitEventArgs> dataCommit = new ScopedEventHandler<DataCommitEventArgs>(nameof (FieldControl), "DataCommit");
    private ScopedEventHandler<EventArgs> focusIn = new ScopedEventHandler<EventArgs>(nameof (FieldControl), "FocusIn");
    private ScopedEventHandler<EventArgs> focusOut = new ScopedEventHandler<EventArgs>(nameof (FieldControl), "FocusOut");
    private ScopedEventHandler<EventArgs> valueChanged = new ScopedEventHandler<EventArgs>(nameof (FieldControl), "ValueChanged");
    private IHTMLElement fieldElement;
    private FieldDescriptor baseField;
    private bool hasFocus;
    private int tabIndex = int.MinValue;
    private FieldAccessMode accessMode;

    [Category("Data")]
    public event DataBindEventHandler DataBind
    {
      add
      {
        if (value == null)
          return;
        this.dataBind.Add(new ScopedEventHandler<DataBindEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.dataBind.Remove(new ScopedEventHandler<DataBindEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    [Category("Data")]
    public event DataCommitEventHandler DataCommit
    {
      add
      {
        if (value == null)
          return;
        this.dataCommit.Add(new ScopedEventHandler<DataCommitEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.dataCommit.Remove(new ScopedEventHandler<DataCommitEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    [Category("Focus")]
    public event EventHandler FocusIn
    {
      add
      {
        if (value == null)
          return;
        this.focusIn.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.focusIn.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    [Category("Focus")]
    public event EventHandler FocusOut
    {
      add
      {
        if (value == null)
          return;
        this.focusOut.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.focusOut.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event EventHandler ValueChanged
    {
      add
      {
        if (value == null)
          return;
        this.valueChanged.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.valueChanged.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    protected FieldControl()
    {
    }

    internal FieldControl(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    [Category("Data")]
    [Description("The Encompass Loan Field to be displayed in this field.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public virtual FieldDescriptor Field
    {
      get
      {
        if (!this.Form.EditingEnabled && this.baseField != null)
          return this.baseField;
        this.baseField = this.Form.GetFieldDescriptor(string.Concat(this.FieldElement.getAttribute("emid", 1)));
        return this.baseField;
      }
      set
      {
        this.FieldElement.setAttribute("emid", (object) value.FieldID, 1);
        this.baseField = (FieldDescriptor) null;
        if (value != FieldDescriptor.Empty && value.ReadOnly)
          this.Enabled = false;
        this.NotifyPropertyChange();
        this.Form.OnFieldBindingChanged(new FormEventArgs((Control) this));
      }
    }

    public override bool Enabled
    {
      get => base.Enabled;
      set
      {
        if (value && this.Field != FieldDescriptor.Empty && this.Field.ReadOnly)
        {
          string fieldId = this.Field.FieldID;
          if (((!fieldId.StartsWith("FE") || fieldId.StartsWith("FEMA")) && !fieldId.StartsWith("FR") || !fieldId.EndsWith("37")) && (!fieldId.StartsWith("FE") || !fieldId.EndsWith("56") && !fieldId.EndsWith("19") && !fieldId.EndsWith("79")) && (!fieldId.StartsWith("FR") || !fieldId.EndsWith("30")) && !fieldId.EndsWith("40"))
            throw new ArgumentException("A control cannot be Enabled when the underlying field value is read-only.");
        }
        base.Enabled = value;
      }
    }

    [Category("Data")]
    [Description("Indicates the source of the loan data for this field")]
    public FieldSource FieldSource
    {
      get
      {
        switch (this.GetAttribute("emsrc"))
        {
          case "link":
            return FieldSource.LinkedLoan;
          case "other":
            return FieldSource.Other;
          default:
            return FieldSource.CurrentLoan;
        }
      }
      set
      {
        if (value == FieldSource.LinkedLoan)
          this.SetAttribute("emsrc", "link");
        else if (value == FieldSource.Other)
          this.SetAttribute("emsrc", "other");
        else
          this.SetAttribute("emsrc", "");
      }
    }

    [Category("Appearance")]
    [Description("The text that appears when the user hovers the mouse over the field. Use this property to override the default hover text provided by the system based on the HelpKey property.")]
    public virtual string HoverText
    {
      get => this.FieldElement.title ?? "";
      set
      {
        this.FieldElement.title = value;
        this.NotifyPropertyChange();
      }
    }

    [Category("Appearance")]
    [Description("An index into the Help system to provide default hover text for the field, if the HoverText property is empty.")]
    public virtual string HelpKey
    {
      get
      {
        return !(this.FieldElement.getAttribute("helpKey", 2) is string attribute) ? this.Field.FieldID : attribute ?? "";
      }
      set
      {
        this.FieldElement.setAttribute("helpKey", (object) value, 1);
        this.NotifyPropertyChange();
      }
    }

    [Category("Layout")]
    [Description("Determines the tab order of the controls on the form")]
    public int TabIndex
    {
      get
      {
        if (this.tabIndex == int.MinValue)
          this.tabIndex = (int) this.FieldElement2.tabIndex;
        return this.tabIndex;
      }
      set
      {
        int tabIndex = this.TabIndex;
        this.FieldElement2.tabIndex = (short) value;
        this.tabIndex = value;
        if (this.Form.EditingEnabled && value > 0)
          this.reindexTabOrder();
        this.NotifyPropertyChange();
      }
    }

    [Browsable(false)]
    public virtual bool Lockable => false;

    [Browsable(false)]
    public abstract string Value { get; }

    [Browsable(false)]
    public virtual FieldAccessMode AccessMode
    {
      get => this.accessMode;
      set
      {
        if (value == this.accessMode)
          return;
        this.accessMode = value;
        this.ApplyInteractiveState();
        this.Rebind();
      }
    }

    public virtual void Focus() => ((IHTMLElement2) this.FieldElement).focus();

    public virtual void Blur() => ((IHTMLElement2) this.FieldElement).blur();

    [Browsable(false)]
    public bool HasFocus => this.hasFocus;

    public virtual void InvokeFocusIn()
    {
      this.hasFocus = true;
      this.ApplyInteractiveState();
      this.focusIn((object) this, EventArgs.Empty);
    }

    public virtual void InvokeFocusOut()
    {
      this.hasFocus = false;
      this.ApplyInteractiveState();
      this.focusOut((object) this, EventArgs.Empty);
    }

    public bool MoveNext()
    {
      if (this.TabIndex <= 0)
        return false;
      FieldControl fieldControl1 = (FieldControl) null;
      FieldControl fieldControl2 = (FieldControl) null;
      foreach (FieldControl allFieldControl in this.Form.GetAllFieldControls())
      {
        if (allFieldControl.Interactive)
        {
          if (!allFieldControl.Equals((object) this) && allFieldControl.TabIndex > 0 && (fieldControl1 == null || allFieldControl.TabIndex < fieldControl1.TabIndex))
            fieldControl1 = allFieldControl;
          if (allFieldControl.TabIndex > this.TabIndex && (fieldControl2 == null || allFieldControl.TabIndex < fieldControl2.TabIndex))
            fieldControl2 = allFieldControl;
        }
      }
      if (fieldControl2 == null)
        fieldControl2 = fieldControl1;
      if (fieldControl2 == null)
        return false;
      fieldControl2.Focus();
      return true;
    }

    public bool MovePrevious()
    {
      if (this.TabIndex <= 0)
        return false;
      FieldControl fieldControl1 = (FieldControl) null;
      FieldControl fieldControl2 = (FieldControl) null;
      foreach (FieldControl allFieldControl in this.Form.GetAllFieldControls())
      {
        if (allFieldControl.Interactive)
        {
          if (!allFieldControl.Equals((object) this) && allFieldControl.TabIndex > 0 && (fieldControl1 == null || allFieldControl.TabIndex > fieldControl1.TabIndex))
            fieldControl1 = allFieldControl;
          if (allFieldControl.TabIndex < this.TabIndex && (fieldControl2 == null || allFieldControl.TabIndex > fieldControl2.TabIndex))
            fieldControl2 = allFieldControl;
        }
      }
      if (fieldControl2 == null)
        fieldControl2 = fieldControl1;
      if (fieldControl2 == null)
        return false;
      fieldControl2.Focus();
      return true;
    }

    public bool BindTo(string value)
    {
      if (this.AccessMode == FieldAccessMode.Hidden)
      {
        this.HideValue();
        return false;
      }
      if (value != this.Value)
      {
        this.DisplayValue(value);
        return true;
      }
      this.DisplayValue(value);
      return false;
    }

    internal abstract void DisplayValue(string value);

    internal abstract void HideValue();

    internal virtual void Rebind() => this.BindTo(this.Value);

    public virtual bool InvokeDataBind(ref string value)
    {
      DataBindEventArgs e = new DataBindEventArgs(value);
      this.OnDataBind(e);
      value = e.Value;
      return !e.Cancel;
    }

    protected void OnDataBind(DataBindEventArgs e) => this.dataBind((object) this, e);

    public virtual bool InvokeDataCommit(ref string value)
    {
      DataCommitEventArgs e = new DataCommitEventArgs(value);
      this.OnDataCommit(e);
      value = e.Value;
      return !e.Cancel;
    }

    protected void OnDataCommit(DataCommitEventArgs e) => this.dataCommit((object) this, e);

    [Browsable(false)]
    public virtual string[] SupportedEvents => FieldControl.supportedEvents;

    public override void RefreshProperties()
    {
      base.RefreshProperties();
      this.baseField = (FieldDescriptor) null;
    }

    internal void NotifyValueChanged() => this.valueChanged((object) this, EventArgs.Empty);

    internal IHTMLElement FieldElement
    {
      get
      {
        this.EnsureAttached();
        return this.fieldElement;
      }
    }

    internal IHTMLElement2 FieldElement2 => this.FieldElement as IHTMLElement2;

    internal string GetBaseFieldAttributes() => " fieldId=\"" + this.ControlID + "\" emid=\"\" ";

    internal void ApplyValueToControl(string value)
    {
      this.EnsureWritable();
      this.BindTo(value);
      if (this.HasFocus)
        return;
      this.NotifyValueChanged();
    }

    internal void EnsureWritable()
    {
      if (this.accessMode != FieldAccessMode.NoRestrictions)
        throw new InvalidOperationException("Field access denied");
    }

    internal override bool ReattachRequired()
    {
      return base.ReattachRequired() || Control.IsDetached(this.fieldElement);
    }

    internal override void ChangeControlID(string oldValue, string newValue)
    {
      this.FieldElement.setAttribute("fieldId", (object) newValue, 1);
      base.ChangeControlID(oldValue, newValue);
    }

    internal override IHTMLElement GetEnablingTargetElement()
    {
      return this.resolveFieldElement(this.HTMLElement);
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      this.fieldElement = this.resolveFieldElement(controlElement);
      if (this.fieldElement == null)
        throw new InvalidOperationException("The field element for this field was not found");
      if (this.fieldElement == controlElement)
        return;
      this.fieldElement.removeAttribute("controlType", 1);
    }

    internal override bool AllowInteractivity
    {
      get => base.AllowInteractivity && this.accessMode == FieldAccessMode.NoRestrictions;
    }

    internal virtual IHTMLElement FindFieldElement(IHTMLElement controlElement)
    {
      return HTMLHelper.FindElementWithAttribute(controlElement, "fieldId", this.ControlID);
    }

    private IHTMLElement resolveFieldElement(IHTMLElement controlElement)
    {
      string lower = controlElement.tagName.ToLower();
      return lower == "input" || lower == "select" || lower == "textarea" || DropdownEditBox.IsDropdownEditBox(controlElement) ? controlElement : this.FindFieldElement(controlElement);
    }

    private void reindexTabOrder()
    {
      FieldControl[] fieldTabSequence = this.Form.GetFieldTabSequence();
      int tabIndex1 = this.TabIndex;
      bool flag = false;
      for (int index = 0; index < fieldTabSequence.Length; ++index)
      {
        if (fieldTabSequence[index] != this)
        {
          int tabIndex2 = fieldTabSequence[index].TabIndex;
          if (tabIndex2 == tabIndex1)
          {
            fieldTabSequence[index].FieldElement2.tabIndex = (short) (tabIndex1 + 1);
            flag = true;
          }
          else if (tabIndex2 == tabIndex1 + 1 & flag)
            fieldTabSequence[index].FieldElement2.tabIndex = (short) (++tabIndex1 + 1);
          else if (tabIndex2 > tabIndex1)
            break;
        }
      }
    }
  }
}
