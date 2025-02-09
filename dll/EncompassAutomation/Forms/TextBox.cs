// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.TextBox
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Text Box")]
  public class TextBox : FieldControl, ISupportsChangeEvent, ISupportsEvents, ISupportsFormatEvent
  {
    private static readonly string[] supportedEvents = new string[2]
    {
      "Change",
      "Format"
    };
    private ScopedEventHandler<EventArgs> change = new ScopedEventHandler<EventArgs>(nameof (TextBox), "Change");
    private ScopedEventHandler<FormatEventArgs> format = new ScopedEventHandler<FormatEventArgs>(nameof (TextBox), "Format");
    private PickList boundList;
    private int maxLength = -1;
    private bool rolodexInitialized;

    [Category("Behavior")]
    public event EventHandler Change
    {
      add
      {
        if (value == null)
          return;
        this.change.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.change.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    [Category("Appearance")]
    public event FormatEventHandler Format
    {
      add
      {
        if (value == null)
          return;
        this.format.Add(new ScopedEventHandler<FormatEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.format.Remove(new ScopedEventHandler<FormatEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public TextBox()
    {
    }

    internal TextBox(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    public override FieldDescriptor Field
    {
      get => base.Field;
      set
      {
        RolodexGroup rolodex = this.Rolodex;
        RolodexField rolodexField = this.RolodexField;
        if (rolodex != null && rolodexField != RolodexField.None)
          rolodex[rolodexField] = value.FieldID;
        base.Field = value;
        if (this.SupportsMaxLength)
          this.MaxLength = value.MaxLength;
        if (!value.Options.RequireValueFromList)
          return;
        this.Enabled = false;
      }
    }

    [Browsable(false)]
    public virtual string Text
    {
      get => ((IHTMLInputElement) this.FieldElement).value ?? "";
      set => this.ApplyValueToControl(value);
    }

    public override string Value => this.Text;

    [Category("Behavior")]
    [Description("The maximum length of the text in the field.")]
    public virtual int MaxLength
    {
      get
      {
        if (this.maxLength < 0)
        {
          try
          {
            this.maxLength = int.Parse(string.Concat(this.FieldElement.getAttribute("maxLength", 1)));
          }
          catch
          {
            this.maxLength = 0;
          }
        }
        return this.maxLength;
      }
      set
      {
        FieldDescriptor field = this.Field;
        if (field.MaxLength > 0 && value > field.MaxLength)
          throw new ArgumentException("The MaxLength property must be set to the length of the underlying field.");
        if (field.IsCustomField && field.MaxLength > 0 && value != field.MaxLength)
          throw new ArgumentException("To adjust the length of a custom field, you should modify the custom field definition.");
        if (value == this.MaxLength)
          return;
        if (value <= 0)
        {
          this.FieldElement.removeAttribute("maxLength", 1);
          this.maxLength = 0;
        }
        else
        {
          this.FieldElement.setAttribute("maxLength", (object) string.Concat((object) value), 1);
          this.maxLength = value;
        }
        this.NotifyPropertyChange();
      }
    }

    public override bool Enabled
    {
      get => base.Enabled;
      set
      {
        if (value == this.Enabled)
          return;
        base.Enabled = value;
        this.Alignment = this.Alignment;
      }
    }

    [Category("Appearance")]
    public TextAlignment Alignment
    {
      get
      {
        string attribute = this.GetAttribute("emstyle");
        switch (attribute)
        {
          case "Numeric":
            return TextAlignment.Right;
          case "Text":
            return TextAlignment.Left;
          case "TextRightAligned":
            return TextAlignment.Right;
          default:
            string str1 = attribute;
            TextAlignment textAlignment = TextAlignment.Left;
            string str2 = textAlignment.ToString();
            if (str1 == str2)
              return TextAlignment.Left;
            string str3 = attribute;
            textAlignment = TextAlignment.Right;
            string str4 = textAlignment.ToString();
            return str3 == str4 ? TextAlignment.Right : TextAlignment.Auto;
        }
      }
      set
      {
        this.SetAttribute("emstyle", value.ToString());
        string str = TextBox.styleToClass(value, this.Interactive, this.HasFocus);
        if (str != this.HTMLElement.className)
          this.HTMLElement.className = str;
        this.NotifyPropertyChange();
      }
    }

    [Category("Rolodex")]
    [Editor(typeof (RolodexSelector), typeof (UITypeEditor))]
    public RolodexGroup Rolodex
    {
      get
      {
        this.initializeRolodex();
        return this.Form.RolodexGroups[this.RolodexID];
      }
      set
      {
        RolodexGroup rolodex = this.Rolodex;
        RolodexField rolodexField = this.RolodexField;
        if (rolodex != null && rolodexField != RolodexField.None && !rolodex.Equals((object) value))
          rolodex[rolodexField] = "";
        if (value == null || value.Equals((object) RolodexGroup.Empty))
        {
          this.RolodexID = "";
        }
        else
        {
          this.RolodexID = value.GroupID;
          if (rolodexField == RolodexField.None)
            return;
          value[rolodexField] = this.Field.FieldID;
        }
      }
    }

    [Category("Rolodex")]
    public RolodexField RolodexField
    {
      get
      {
        RolodexGroup rolodex = this.Rolodex;
        FieldDescriptor field = this.Field;
        return rolodex == null || field.FieldID == "" ? RolodexField.None : rolodex.GetFieldMap(field.FieldID);
      }
      set
      {
        FieldDescriptor field = this.Field;
        if (field.FieldID == "")
          throw new InvalidOperationException("A Field ID must be assigned to the TextBox before a Rolodex field can be mapped");
        RolodexGroup rolodex = this.Rolodex;
        if (rolodex == null && value != RolodexField.None)
          throw new InvalidOperationException("A rolodex must be assigned to this field before setting the mapping");
        if (rolodex == null)
          return;
        RolodexField fieldMap = rolodex.GetFieldMap(field.FieldID);
        if (fieldMap != RolodexField.None)
          rolodex[fieldMap] = "";
        if (value != RolodexField.None)
          rolodex[value] = field.FieldID;
        this.NotifyPropertyChange();
      }
    }

    public override bool Interactive => base.Interactive && !this.ReadOnly;

    internal virtual bool ReadOnly
    {
      get => ((IHTMLInputTextElement) this.HTMLElement).readOnly;
      set => ((IHTMLInputTextElement) this.HTMLElement).readOnly = value;
    }

    [Browsable(false)]
    public virtual bool SupportsMaxLength => true;

    public override string[] SupportedEvents
    {
      get => Control.MergeEvents(base.SupportedEvents, TextBox.supportedEvents);
    }

    internal override void DisplayValue(string value)
    {
      ((IHTMLInputElement) this.FieldElement).value = value;
    }

    internal override void HideValue() => this.DisplayValue("*");

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      if (!this.SupportsMaxLength || this.Field.MaxLength <= 0 || this.MaxLength != 0 && this.MaxLength <= this.Field.MaxLength && !this.Field.IsCustomField)
        return;
      this.MaxLength = this.Field.MaxLength;
    }

    public void InvokeChange() => this.OnChange(EventArgs.Empty);

    protected virtual void OnChange(EventArgs e) => this.change((object) this, e);

    public bool InvokeFormat(ref string value)
    {
      FormatEventArgs e = new FormatEventArgs(value);
      this.OnFormat(e);
      value = e.Value;
      return !e.Cancel;
    }

    protected virtual void OnFormat(FormatEventArgs e) => this.format((object) this, e);

    public void AttachedPickList(PickList pickList)
    {
      if (this.boundList != null)
        return;
      this.boundList = pickList;
      this.boundList.BindToControl((FieldControl) this);
      if (this.Field != FieldDescriptor.Empty)
        this.boundList.Title = "Field: " + this.Field.FieldID;
      Point absolutePosition = this.AbsolutePosition;
      Size size = this.Size;
      this.Size = new Size(Math.Max(size.Width - this.boundList.Size.Width - 1, 0), size.Height);
      this.boundList.AbsolutePosition = new Point(this.Bounds.Right + 1, absolutePosition.Y);
    }

    public PickList CreatePickList(string[] listItems)
    {
      if (this.boundList == null)
      {
        this.boundList = new PickList();
        this.GetContainer().Controls.Insert((Control) this.boundList);
        this.boundList.BindToControl((FieldControl) this);
        if (this.Field != FieldDescriptor.Empty)
          this.boundList.Title = "Field: " + this.Field.FieldID;
        Point absolutePosition = this.AbsolutePosition;
        Size size = this.Size;
        this.Size = new Size(Math.Max(size.Width - this.boundList.Size.Width - 1, 0), size.Height);
        this.boundList.AbsolutePosition = new Point(this.Bounds.Right + 1, absolutePosition.Y);
      }
      this.boundList.Options.Clear();
      DropdownOption[] optionList = new DropdownOption[listItems.Length];
      for (int index = 0; index < listItems.Length; ++index)
        optionList[index] = new DropdownOption(listItems[index]);
      this.boundList.Options.AddRange((ICollection) optionList);
      return this.boundList;
    }

    public PickList GetPickList() => this.boundList;

    public void RemovePickList()
    {
      if (this.boundList == null)
        return;
      Size size = this.Size;
      int width1 = size.Width;
      size = this.boundList.Size;
      int width2 = size.Width;
      int width3 = width1 + width2 + 1;
      size = this.Size;
      int height = size.Height;
      this.Size = new Size(width3, height);
      this.boundList.Delete();
      this.boundList = (PickList) null;
    }

    internal string RolodexID
    {
      get => this.GetAttribute("rolodexId");
      set => this.SetAttribute("rolodexId", value);
    }

    internal override string RenderHTML()
    {
      return "<input" + this.GetBaseAttributes() + this.GetBaseFieldAttributes() + " class=\"inputTextAlpha\" style=\"height: 20px\" type=\"text\"></input>";
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      if (!this.Form.IsOptionEnabled(FormOptions.ManageEvents))
        return;
      HTMLInputTextElementEvents2_Event fieldElement = (HTMLInputTextElementEvents2_Event) this.FieldElement;
      try
      {
        // ISSUE: method pointer
        fieldElement.onchange -= new HTMLInputTextElementEvents2_onchangeEventHandler((object) this, (UIntPtr) __methodptr(onTextChanged));
      }
      catch
      {
      }
      // ISSUE: method pointer
      fieldElement.onchange += new HTMLInputTextElementEvents2_onchangeEventHandler((object) this, (UIntPtr) __methodptr(onTextChanged));
    }

    public override void RefreshProperties()
    {
      base.RefreshProperties();
      using (PerformanceMeter.Current.BeginOperation("TextBox.RefreshProperties"))
      {
        if (!this.SupportsMaxLength || this.Field.MaxLength <= 0)
          return;
        if (this.Field.IsCustomField)
        {
          this.MaxLength = this.Field.MaxLength;
        }
        else
        {
          if (this.Field.MaxLength >= this.MaxLength)
            return;
          this.MaxLength = this.Field.MaxLength;
        }
      }
    }

    private bool onTextChanged(IHTMLEventObj e)
    {
      this.OnChange(EventArgs.Empty);
      return true;
    }

    internal override void ChangeControlInteractiveState(bool interactive)
    {
      this.ReadOnly = !interactive;
      string str = TextBox.styleToClass(this.Alignment, interactive, this.HasFocus);
      if (!(str != this.HTMLElement.className))
        return;
      this.HTMLElement.className = str;
    }

    private static string styleToClass(TextAlignment alignment, bool interactive, bool active)
    {
      if (alignment == TextAlignment.Right)
      {
        if (!interactive)
          return "inputTextNumDisabled";
        return active ? "inputTextNumFocus" : "inputTextNum";
      }
      if (!interactive)
        return "inputTextAlphaDisabled";
      return active ? "inputTextAlphaFocus" : "inputTextAlpha";
    }

    private void initializeRolodex()
    {
      if (this.rolodexInitialized)
        return;
      this.rolodexInitialized = true;
      using (PerformanceMeter.Current.BeginOperation("TextBox.initializeRolodex"))
      {
        string fieldId = this.Field.FieldID;
        if (!(this.RolodexID == "") || !(fieldId != ""))
          return;
        foreach (RolodexGroup rolodexGroup in (IEnumerable) this.Form.RolodexGroups)
        {
          foreach (RolodexField field in Enum.GetValues(typeof (RolodexField)))
          {
            if (rolodexGroup[field] == fieldId)
            {
              this.Rolodex = rolodexGroup;
              this.RolodexField = field;
            }
          }
        }
      }
    }
  }
}
