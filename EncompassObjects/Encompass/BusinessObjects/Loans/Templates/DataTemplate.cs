// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.DataTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents a saved Data Template which can be applied to a loan.
  /// </summary>
  public class DataTemplate : Template, IDataTemplate
  {
    private EllieMae.EMLite.DataEngine.DataTemplate template;
    private DataTemplate.DataTemplateFields fields;

    internal DataTemplate(Session session, TemplateEntry fsEntry, EllieMae.EMLite.DataEngine.DataTemplate template)
      : base(session, fsEntry)
    {
      this.template = template;
      this.fields = new DataTemplate.DataTemplateFields(session, (FormDataBase) template);
    }

    /// <summary>
    /// Returns the type of template represented by the object.
    /// </summary>
    public override TemplateType TemplateType => TemplateType.DataTemplate;

    /// <summary>Gets the description of the template.</summary>
    public override string Description => this.template.Description;

    /// <summary>
    /// Gets the collection of fields associated with the template.
    /// </summary>
    public Fields Fields => (Fields) this.fields;

    /// <summary>
    /// Gets the collection of the Template Fields modified by the user
    /// </summary>
    public Dictionary<string, object> UserModifiedDataTemplateFields
    {
      get => this.fields.UserModifiedFields;
    }

    /// <summary>
    /// Unwraps the template object to return the internal data type.
    /// </summary>
    internal override object Unwrap() => (object) this.template;

    /// <summary>
    /// Provides an override of the Fields class for the DataTemplate
    /// </summary>
    private class DataTemplateFields : Fields
    {
      private Session session;
      private FormDataBase fieldData;
      private Dictionary<string, object> userModifiedFields = new Dictionary<string, object>();

      public DataTemplateFields(Session session, FormDataBase fieldData)
      {
        this.session = session;
        this.fieldData = fieldData;
      }

      public Dictionary<string, object> UserModifiedFields
      {
        get
        {
          this.userModifiedFields.Clear();
          foreach (KeyValuePair<string, object> fieldDataWithValue in (Dictionary<string, object>) this.fieldData.FieldDataWithValues)
            this.userModifiedFields.Add(fieldDataWithValue.Key, fieldDataWithValue.Value);
          return this.userModifiedFields;
        }
      }

      internal override EllieMae.Encompass.BusinessObjects.Loans.Field CreateField(string fieldId)
      {
        FieldDescriptor fieldDescriptor = this.session.Loans.FieldDescriptors[fieldId];
        return fieldDescriptor == null ? (EllieMae.Encompass.BusinessObjects.Loans.Field) null : (EllieMae.Encompass.BusinessObjects.Loans.Field) new ReadOnlyField(fieldId, this.fieldData.GetSimpleField(fieldId), fieldDescriptor);
      }
    }
  }
}
