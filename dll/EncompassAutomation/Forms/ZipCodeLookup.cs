// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ZipCodeLookup
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

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
  [ToolboxControl("Zip Code Lookup")]
  public class ZipCodeLookup : DesignerControl
  {
    public ZipCodeLookup()
    {
    }

    internal ZipCodeLookup(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
    }

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

    [Category("Data")]
    [Description("The Field to which the City associated with the ZIP Code will be written.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor CityField
    {
      get => this.getFieldForAttribute("city");
      set => this.setFieldForAttribute("city", value);
    }

    [Category("Data")]
    [Description("The Field to which the County associated with the ZIP Code will be written.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor CountyField
    {
      get => this.getFieldForAttribute("cnty");
      set => this.setFieldForAttribute("cnty", value);
    }

    [Category("Data")]
    [Description("The Field to which the State associated with the ZIP Code will be written.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor StateField
    {
      get => this.getFieldForAttribute("st");
      set => this.setFieldForAttribute("st", value);
    }

    public override void Delete()
    {
      this.Form.ControlIDChanged -= new ControlIDChangedEventHandler(this.onFormControlIDChanged);
      base.Delete();
    }

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      ((DispHTMLImg) this.getImageElement()).src = Control.ResolveInternalImagePath("zipcode.jpg");
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
