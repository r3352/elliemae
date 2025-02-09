// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CustomFields.CustomFieldsDefinitionInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.BizLayer;
using EllieMae.EMLite.FieldSearch;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.CustomFields
{
  [Serializable]
  public class CustomFieldsDefinitionInfo : BusinessInfoBase, IFieldSearchable
  {
    public CustomFieldsType CustomFieldsType;
    public int RecordId;
    [NotUndoable]
    public CustomFieldDefinitionInfo[] CustomFieldDefinitions;
    [NotUndoable]
    public bool IsNew;
    [NotUndoable]
    public bool IsDirty;
    [NotUndoable]
    public bool IsDeleted;

    public CustomFieldsDefinitionInfo()
    {
    }

    public CustomFieldsDefinitionInfo(
      CustomFieldsType customFieldsType,
      int recordId,
      CustomFieldDefinitionInfo[] customFieldDefinitions)
    {
      this.CustomFieldsType = customFieldsType;
      this.RecordId = recordId;
      this.CustomFieldDefinitions = customFieldDefinitions;
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      if (this.CustomFieldDefinitions != null)
      {
        CustomFieldDefinitionInfo[] fieldDefinitionInfoArray = this.CustomFieldDefinitions;
        for (int index = 0; index < fieldDefinitionInfoArray.Length; ++index)
        {
          CustomFieldDefinitionInfo fieldDef = fieldDefinitionInfoArray[index];
          if (!fieldDef.IsDeleted)
          {
            yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ContactCustomFields, fieldDef.FieldDescription);
            if (!string.IsNullOrEmpty(fieldDef.LoanFieldId))
              yield return new KeyValuePair<RelationshipType, string>(RelationshipType.AffectsValueOf, fieldDef.LoanFieldId);
          }
          fieldDef = (CustomFieldDefinitionInfo) null;
        }
        fieldDefinitionInfoArray = (CustomFieldDefinitionInfo[]) null;
      }
    }
  }
}
