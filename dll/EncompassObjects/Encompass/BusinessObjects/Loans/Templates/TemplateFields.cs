// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class TemplateFields : Fields, ITemplateFields
  {
    private Template template;
    private FormDataBase fieldData;
    private FieldDescriptors descriptors;

    internal TemplateFields(Template template, FormDataBase fieldData)
    {
      this.template = template;
      this.fieldData = fieldData;
    }

    public FieldDescriptors Descriptors
    {
      get
      {
        if (this.descriptors == null)
          this.descriptors = TemplateFields.GetDescriptorsForTemplate(this.template.Session, this.fieldData);
        return this.descriptors;
      }
    }

    internal override Field CreateField(string fieldId)
    {
      FieldDescriptor descriptor = this.Descriptors[fieldId];
      return descriptor == null ? (Field) null : (Field) new ReadOnlyField(fieldId, this.fieldData.GetSimpleField(fieldId), descriptor);
    }

    internal static FieldDescriptors GetDescriptorsForTemplate(
      Session session,
      FormDataBase formDatabase)
    {
      string[] allowedFieldIds = formDatabase.GetAllowedFieldIDs();
      if (allowedFieldIds == null)
        return (FieldDescriptors) null;
      FieldDescriptors descriptorsForTemplate = new FieldDescriptors();
      foreach (string fieldId in allowedFieldIds)
      {
        FieldDescriptor fieldDescriptor = session.Loans.FieldDescriptors[fieldId];
        if (fieldDescriptor != null)
          descriptorsForTemplate.Add(fieldDescriptor);
      }
      return descriptorsForTemplate;
    }
  }
}
