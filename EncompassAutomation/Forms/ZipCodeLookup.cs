// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ZipCodeLookup
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// A <see cref="T:EllieMae.Encompass.Forms.DesignerControl" /> that adds ZIP code lookup functionality to a form.
  /// </summary>
  /// <remarks>Use this control to perform automatic ZIP code lookups when a user leaves a ZIP code
  /// field and have the matching city, county and/or state populated into other fields.</remarks>
  [ToolboxControl("Zip Code Lookup")]
  public class ZipCodeLookup : DesignerControl
  {
    /// <summary>Constructor for a new ZipCodeLookup control.</summary>
    public ZipCodeLookup()
    {
    }

    internal ZipCodeLookup(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
    }

    /// <summary>
    /// Gets or sets the Control which contains the ZIP Code information.
    /// </summary>
    [Category("Source")]
    [Description("The Control which will contain the ZIP Code value to be resolved.")]
    [TypeConverter(typeof (NonExpandableControlTypeConverter))]
    [Editor(typeof (ZipCodeTargetSelector), typeof (UITypeEditor))]
    public Control ZipControl
    {
      get
      {
        string attribute = this.GetAttribute("sourceControlId");
        return attribute == "" ? (Control) null : this.Form.FindControl(attribute);
      }
      set
      {
        if (string.Concat((object) value) == "")
        {
          this.SetAttribute("sourceControlId", "");
        }
        else
        {
          if (value.Form != this.Form)
            throw new ArgumentException("The specified control is from a different form");
          this.SetAttribute("sourceControlId", value.ControlID);
        }
      }
    }

    /// <summary>
    /// Gets or sets the Field which will be populated with the City that is resolved from the ZIP Code.
    /// </summary>
    [Category("Data")]
    [Description("The Field to which the City associated with the ZIP Code will be written.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor CityField
    {
      get => this.getFieldForAttribute("city");
      set => this.setFieldForAttribute("city", value);
    }

    /// <summary>
    /// Gets or sets the Field which will be populated with the County that is resolved from the ZIP Code.
    /// </summary>
    [Category("Data")]
    [Description("The Field to which the County associated with the ZIP Code will be written.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor CountyField
    {
      get => this.getFieldForAttribute("cnty");
      set => this.setFieldForAttribute("cnty", value);
    }

    /// <summary>
    /// Gets or sets the Field which will be populated with the State that is resolved from the ZIP Code.
    /// </summary>
    [Category("Data")]
    [Description("The Field to which the State associated with the ZIP Code will be written.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor StateField
    {
      get => this.getFieldForAttribute("st");
      set => this.setFieldForAttribute("st", value);
    }

    /// <summary>
    /// Overrides the base class Delete method to ensure that the control is properly disposed of.
    /// </summary>
    public override void Delete()
    {
      this.Form.ControlIDChanged -= new ControlIDChangedEventHandler(this.onFormControlIDChanged);
      base.Delete();
    }

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      this.getImageElement().src = Control.ResolveInternalImagePath("zipcode.jpg");
    }

    private FieldDescriptor getFieldForAttribute(string attributeName)
    {
      return this.Form.GetFieldDescriptor(this.GetAttribute(attributeName));
    }

    private void setFieldForAttribute(string attributeName, FieldDescriptor field)
    {
      if (field == null || field == FieldDescriptor.Empty)
        this.SetAttribute(attributeName, "");
      else
        this.SetAttribute(attributeName, field.FieldID);
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      form.ControlIDChanged -= new ControlIDChangedEventHandler(this.onFormControlIDChanged);
      form.ControlIDChanged += new ControlIDChangedEventHandler(this.onFormControlIDChanged);
    }

    private HTMLImg getImageElement() => (HTMLImg) this.HTMLElement;

    internal override string RenderHTML()
    {
      return "<img " + this.GetBaseAttributes(false) + " src=\"" + Control.ResolveInternalImagePath("zipcode.jpg") + "\" emimport=\"0\">";
    }

    private void onFormControlIDChanged(object source, ControlIDChangedEventArgs e)
    {
      if (!(e.PriorControlID == this.GetAttribute("sourceControlId")))
        return;
      this.SetAttribute("sourceControlId", e.Control.ControlID);
    }
  }
}
