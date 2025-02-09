// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DropdownBox
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

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
  /// <summary>
  /// Rerpresents a Dropdown box on an Encompass input form.
  /// </summary>
  [ToolboxControl("Dropdown Box")]
  public class DropdownBox : FieldControl, ISupportsChangeEvent, ISupportsEvents
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Change"
    };
    /// <summary>
    /// Represents the "restricted" option which is automatically displayed when the user
    /// does not have access to view the value of the underlying field.
    /// </summary>
    protected static readonly DropdownOption RestrictedOption = new DropdownOption("*");
    private ScopedEventHandler<EventArgs> change = new ScopedEventHandler<EventArgs>(nameof (DropdownBox), "Change");
    private DropdownOptionCollection options;
    private int restrictedOptionIndex = -1;

    /// <summary>
    /// Event which gets fired when the user modifies the selected value in the DropdownBox.
    /// </summary>
    [Category("Behavior")]
    public event EventHandler Change
    {
      add
      {
        if (value == null)
          return;
        this.change.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.change.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>Constructor for a new DropdownBox control.</summary>
    public DropdownBox() => this.options = this.CreateOptionsCollection();

    internal DropdownBox(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      this.options = this.CreateOptionsCollection();
    }

    /// <summary>
    /// Gets or sets the loan field associated with the control.
    /// </summary>
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

    /// <summary>
    /// Gets the collection of options to be displayed in the dropdown box.
    /// </summary>
    [Category("Data")]
    [Editor(typeof (DropdownOptionsEditor), typeof (UITypeEditor))]
    public DropdownOptionCollection Options => this.options;

    /// <summary>
    /// Overrides the HoverText property because Hover Text is not available on this
    /// object.
    /// </summary>
    /// <remarks>Attempting to set this property will result in a NotSupportedException.</remarks>
    [Browsable(false)]
    public override string HoverText
    {
      get => "";
      set => throw new NotSupportedException("Hover text is not supported on this object");
    }

    /// <summary>Gets the border color for the control.</summary>
    /// <remarks>The border color for a DropdownEditBox cannot be modified. Attempting to set
    /// this property will result in an InvalidOperationException.</remarks>
    [Browsable(false)]
    public override Color BorderColor
    {
      get => base.BorderColor;
      set
      {
        throw new InvalidOperationException("The BorderColor property for this control type is read-only.");
      }
    }

    /// <summary>Gets the border color for the control.</summary>
    /// <remarks>The border color for a DropdownEditBox cannot be modified.</remarks>
    [Browsable(false)]
    public override BorderStyle BorderStyle
    {
      get => base.BorderStyle;
      set
      {
        throw new InvalidOperationException("The BorderStyle property for this control type is read-only.");
      }
    }

    /// <summary>Gets the border color for the control.</summary>
    /// <remarks>The border color for a DropdownEditBox cannot be modified.</remarks>
    [Browsable(false)]
    public override int BorderWidth
    {
      get => base.BorderWidth;
      set
      {
        throw new InvalidOperationException("The BorderWidth property for this control type is read-only.");
      }
    }

    /// <summary>Gets or sets the selected index in the dropdown box.</summary>
    [Browsable(false)]
    public virtual int SelectedIndex
    {
      get => ((IHTMLSelectElement) this.FieldElement).selectedIndex;
      set => this.ApplyValueToControl(value >= 0 ? this.Options[value].Value : "");
    }

    /// <summary>
    /// Gets the current value of the checkbox based on the checked state of the object.
    /// </summary>
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

    /// <summary>
    /// This is a special handling for field 2626, user might need to hide some options from user
    /// This is nothing to do with access mode, just want to prevent user from selecting some options
    /// </summary>
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

    /// <summary>Gets the list of supported event names.</summary>
    public override string[] SupportedEvents
    {
      get => Control.MergeEvents(base.SupportedEvents, DropdownBox.supportedEvents);
    }

    /// <summary>Invokes the Change event on the control.</summary>
    /// <remarks>This method is intended for use by Encompass only.</remarks>
    /// <exclude />
    public void InvokeChange() => this.OnChange(EventArgs.Empty);

    /// <summary>Refreshes the properties of the Dropdown box.</summary>
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

    /// <summary>
    /// Raises the <see cref="E:EllieMae.Encompass.Forms.DropdownBox.Change" /> event.
    /// </summary>
    /// <param name="e">The event arguments to be passed to the event handlers.</param>
    protected virtual void OnChange(EventArgs e) => this.change.Invoke((object) this, e);

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
      for (int name = 0; name < fieldElement.length; ++name)
      {
        IHTMLOptionElement htmlOptionElement = (IHTMLOptionElement) fieldElement.item((object) name);
        arrayList.Add((object) new DropdownOption(htmlOptionElement.text, htmlOptionElement.value));
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
        IHTMLElement element = htmlDocument.createElement("option");
        fieldElement.add(element);
        element.innerText = option.Text;
        element.setAttribute("value", (object) option.Value);
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

    /// <summary>
    /// Method for creating the collection of options for the control.
    /// </summary>
    /// <returns></returns>
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
