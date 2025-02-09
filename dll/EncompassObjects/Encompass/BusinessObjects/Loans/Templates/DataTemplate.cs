// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.DataTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class DataTemplate : Template, IDataTemplate
  {
    private DataTemplate template;
    private DataTemplate.DataTemplateFields fields;

    internal DataTemplate(Session session, TemplateEntry fsEntry, DataTemplate template)
      : base(session, fsEntry)
    {
      this.template = template;
      this.fields = new DataTemplate.DataTemplateFields(session, (FormDataBase) template);
    }

    public override TemplateType TemplateType => TemplateType.DataTemplate;

    public override string Description => ((FieldDataTemplate) this.template).Description;

    public Fields Fields => (Fields) this.fields;

    public Dictionary<string, object> UserModifiedDataTemplateFields
    {
      get => this.fields.UserModifiedFields;
    }

    internal override object Unwrap() => (object) this.template;

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

      internal override Field CreateField(string fieldId)
      {
        FieldDescriptor fieldDescriptor = this.session.Loans.FieldDescriptors[fieldId];
        return fieldDescriptor == null ? (Field) null : (Field) new ReadOnlyField(fieldId, this.fieldData.GetSimpleField(fieldId), fieldDescriptor);
      }
    }
  }
}
