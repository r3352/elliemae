// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DropdownBox
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Dropdown Box")]
  public class DropdownBox : FieldControl, ISupportsChangeEvent, ISupportsEvents
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Change"
    };
    protected static readonly DropdownOption RestrictedOption = new DropdownOption("*");
    private ScopedEventHandler<EventArgs> change = new ScopedEventHandler<EventArgs>(nameof (DropdownBox), "Change");
    private DropdownOptionCollection options;
    private int restrictedOptionIndex = -1;

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

    public DropdownBox() => this.options = this.CreateOptionsCollection();

    internal DropdownBox(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      this.options = this.CreateOptionsCollection();
    }

    public override FieldDescriptor Field
    {
      get => base.Field;
      set
      {
        base.Field = value;
        if (value == null || value.Options.Count <= 0 && !value.Options.RequireValueFromList)
          return;
        this.prepopulateOptionsList(value);
      }
    }

    [Category("Data")]
    [Editor(typeof (DropdownOptionsEditor), typeof (UITypeEditor))]
    public DropdownOptionCollection Options => this.options;

    [Browsable(false)]
    public override string HoverText
    {
      get => "";
      set => throw new NotSupportedException("Hover text is not supported on this object");
    }

    [Browsable(false)]
    public override Color BorderColor
    {
      get => base.BorderColor;
      set
      {
        throw new InvalidOperationException("The BorderColor property for this control type is read-only.");
      }
    }

    [Browsable(false)]
    public override BorderStyle BorderStyle
    {
      get => base.BorderStyle;
      set
      {
        throw new InvalidOperationException("The BorderStyle property for this control type is read-only.");
      }
    }

    [Browsable(false)]
    public override int BorderWidth
    {
      get => base.BorderWidth;
      set
      {
        throw new InvalidOperationException("The BorderWidth property for this control type is read-only.");
      }
    }

    [Browsable(false)]
    public virtual int SelectedIndex
    {
      get => ((IHTMLSelectElement) this.FieldElement).selectedIndex;
      set => this.ApplyValueToControl(value >= 0 ? this.Options[value].Value : "");
    }

    [Browsable(false)]
    public override string Value => ((IHTMLSelectElement) this.FieldElement).value ?? "";

    internal override void DisplayValue(string value)
    {
      if (this.restrictedOptionIndex >= 0)
      {
        this.Options.RemoveAt(this.restrictedOptionIndex);
        this.restrictedOptionIndex = -1;
      }
      ((IHTMLSelectElement) this.FieldElement).value = value;
    }

    internal override void HideValue()
    {
      if (this.restrictedOptionIndex < 0)
        this.restrictedOptionIndex = this.Options.Add(DropdownBox.RestrictedOption);
      ((IHTMLSelectElement) this.FieldElement).selectedIndex = this.restrictedOptionIndex;
    }

    public void KeepOptions(int[] keepIndex)
    {
      if (keepIndex == null || keepIndex.Length == 0)
        return;
      ArrayList optionList = new ArrayList();
      for (int index1 = 0; index1 < this.options.Count; ++index1)
      {
        if (this.Options.AllowEmptyValues && index1 == 0)
        {
          optionList.Add((object) this.options[index1]);
        }
        else
        {
          bool flag = false;
          int num = !this.Options.AllowEmptyValues ? index1 : index1 - 1;
          for (int index2 = 0; index2 < keepIndex.Length; ++index2)
          {
            if (num == keepIndex[index2])
            {
              flag = true;
              break;
            }
          }
          if (flag)
            optionList.Add((object) this.options[index1]);
        }
      }
      this.Options.Clear();
      this.Options.AddRange((ICollection) optionList);
    }

    public override string[] SupportedEvents
    {
      get => Control.MergeEvents(base.SupportedEvents, DropdownBox.supportedEvents);
    }

    public void InvokeChange() => this.OnChange(EventArgs.Empty);

    public override void RefreshProperties()
    {
      base.RefreshProperties();
      using (PerformanceMeter.Current.BeginOperation("DropdownBox.RefreshProperties"))
      {
        FieldDescriptor field = this.Field;
        if (field == FieldDescriptor.Empty || !field.IsCustomField || !field.Options.RequireValueFromList)
          return;
        bool flag = false;
        Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
        foreach (DropdownOption option in this.Options)
        {
          insensitiveHashtable[(object) option.Value] = (object) option.Text;
          if (option.Value == "")
            flag = true;
        }
        ArrayList optionList = new ArrayList();
        if (flag)
          optionList.Add((object) new DropdownOption(string.Concat(insensitiveHashtable[(object) ""]), ""));
        foreach (FieldOption option in field.Options)
        {
          string text = insensitiveHashtable.Contains((object) option.Value) ? insensitiveHashtable[(object) option.Value].ToString() : option.Text;
          optionList.Add((object) new DropdownOption(text, option.Value));
        }
        this.Options.Clear();
        this.Options.AddRange((ICollection) optionList);
      }
    }

    protected virtual void OnChange(EventArgs e) => this.change((object) this, e);

    internal override void ChangeControlInteractiveState(bool interactive)
    {
      base.ChangeControlInteractiveState(interactive);
      if (!interactive)
        this.HTMLElement.className = "inputSelectDisabled";
      else if (this.HasFocus)
        this.HTMLElement.className = "inputSelectFocus";
      else
        this.HTMLElement.className = "inputSelect";
    }

    internal virtual DropdownOption[] ParseOptionsList()
    {
      IHTMLSelectElement fieldElement = this.FieldElement as IHTMLSelectElement;
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < fieldElement.length; ++index)
      {
        IHTMLOptionElement ihtmlOptionElement = (IHTMLOptionElement) fieldElement.item((object) index, (object) null);
        arrayList.Add((object) new DropdownOption(ihtmlOptionElement.text, ihtmlOptionElement.value));
      }
      return (DropdownOption[]) arrayList.ToArray(typeof (DropdownOption));
    }

    internal virtual void RenderOptionsList(DropdownOption[] options)
    {
      IHTMLSelectElement fieldElement = this.FieldElement as IHTMLSelectElement;
      HTMLDocument htmlDocument = this.GetHTMLDocument();
      fieldElement.length = 0;
      foreach (DropdownOption option in options)
      {
        IHTMLElement element = ((DispHTMLDocument) htmlDocument).createElement("option");
        fieldElement.add(element, (object) null);
        element.innerText = option.Text;
        element.setAttribute("value", (object) option.Value, 1);
      }
      this.NotifyPropertyChange();
    }

    internal override string RenderHTML()
    {
      return "<select size=\"1\" style=\"width: 200px\" class=\"inputSelect\"" + this.GetBaseAttributes() + this.GetBaseFieldAttributes() + ">" + Environment.NewLine + "</select>";
    }

    private void prepopulateOptionsList(FieldDescriptor field)
    {
      this.Options.Clear();
      bool flag = false;
      ArrayList optionList = new ArrayList();
      foreach (FieldOption option in field.Options)
      {
        if (option.Value == "")
          flag = true;
        optionList.Add((object) new DropdownOption(option.Text, option.Value));
      }
      if (!flag && this.Options.AllowEmptyValues)
        optionList.Insert(0, (object) new DropdownOption(""));
      this.Options.AddRange((ICollection) optionList);
    }

    internal virtual DropdownOptionCollection CreateOptionsCollection()
    {
      return new DropdownOptionCollection((IDropdownOptionContainer) new DropdownBox.DropdownBoxOptionContainer(this));
    }

    internal class DropdownBoxOptionContainer : IDropdownOptionContainer
    {
      private DropdownBox parentControl;

      public DropdownBoxOptionContainer(DropdownBox parentControl)
      {
        this.parentControl = parentControl;
      }

      public DropdownOption[] ParseOptionsList() => this.parentControl.ParseOptionsList();

      public void RenderOptionsList(DropdownOption[] options)
      {
        this.parentControl.RenderOptionsList(options);
      }

      public bool AllowEditValues => !this.parentControl.Field.Options.RequireValueFromList;

      public bool AllowRearrangeValues => !this.parentControl.Field.IsCustomField;
    }
  }
}
