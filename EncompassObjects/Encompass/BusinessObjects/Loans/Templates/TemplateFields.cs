// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents the collection of fields that are stored for a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.Template" />.
  /// </summary>
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

    /// <summary>
    /// Gets the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor" /> objects which define the fields
    /// available in the template.
    /// </summary>
    public FieldDescriptors Descriptors
    {
      get
      {
        if (this.descriptors == null)
          this.descriptors = TemplateFields.GetDescriptorsForTemplate(this.template.Session, this.fieldData);
        return this.descriptors;
      }
    }

    /// <summary>Generates a Field from the specified Field ID.</summary>
    internal override EllieMae.Encompass.BusinessObjects.Loans.Field CreateField(string fieldId)
    {
      FieldDescriptor descriptor = this.Descriptors[fieldId];
      return descriptor == null ? (EllieMae.Encompass.BusinessObjects.Loans.Field) null : (EllieMae.Encompass.BusinessObjects.Loans.Field) new ReadOnlyField(fieldId, this.fieldData.GetSimpleField(fieldId), descriptor);
    }

    /// <summary>
    /// Generates the FieldDescriptors object for a FormDataBase object
    /// </summary>
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
