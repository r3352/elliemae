// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CustomFields.CustomFieldDefinitionCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.CustomFields;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CustomFields
{
  [Serializable]
  public class CustomFieldDefinitionCollection : BusinessCollectionBase
  {
    public CustomFieldDefinition this[int index] => (CustomFieldDefinition) this.List[index];

    public void Add(CustomFieldDefinition customFieldDefinition)
    {
      this.List.Add((object) customFieldDefinition);
    }

    public void Remove(CustomFieldDefinition customFieldDefinition)
    {
      this.List.Remove((object) customFieldDefinition);
    }

    public CustomFieldDefinition FindFieldId(int fieldId)
    {
      foreach (CustomFieldDefinition fieldId1 in (IEnumerable) this.List)
      {
        if (fieldId1.FieldId.Equals(fieldId))
          return fieldId1;
      }
      return (CustomFieldDefinition) null;
    }

    public CustomFieldDefinition FindFieldNumber(int fieldNumber)
    {
      foreach (CustomFieldDefinition fieldNumber1 in (IEnumerable) this.List)
      {
        if (fieldNumber1.FieldNumber.Equals(fieldNumber))
          return fieldNumber1;
      }
      return (CustomFieldDefinition) null;
    }

    public CustomFieldDefinition Find(string fieldDescription)
    {
      foreach (CustomFieldDefinition customFieldDefinition in (IEnumerable) this.List)
      {
        if (customFieldDefinition.FieldDescription.Equals(fieldDescription))
          return customFieldDefinition;
      }
      return (CustomFieldDefinition) null;
    }

    public bool PossibleDataLoss()
    {
      foreach (CustomFieldDefinition customFieldDefinition in (IEnumerable) this.List)
      {
        if (customFieldDefinition.FieldFormatChanged())
          return true;
      }
      foreach (BusinessBase deleted in (CollectionBase) this.deletedList)
      {
        if (!deleted.IsNew)
          return true;
      }
      return false;
    }

    public CustomFieldDefinitionCollection GetDirtyMembers()
    {
      CustomFieldDefinitionCollection dirtyMembers = CustomFieldDefinitionCollection.NewCustomFieldDefinitionCollection();
      foreach (CustomFieldDefinition customFieldDefinition in (IEnumerable) this.List)
      {
        if (customFieldDefinition.IsDirty)
          dirtyMembers.Add(customFieldDefinition);
      }
      foreach (CustomFieldDefinition deleted in (CollectionBase) this.deletedList)
      {
        if (!deleted.IsNew)
          dirtyMembers.Add(deleted);
      }
      return dirtyMembers;
    }

    public bool Contains(CustomFieldDefinition customFieldDefinition)
    {
      foreach (CustomFieldDefinition customFieldDefinition1 in (IEnumerable) this.List)
      {
        if (customFieldDefinition1.Equals(customFieldDefinition))
          return true;
      }
      return false;
    }

    public bool ContainsFieldId(int fieldId)
    {
      foreach (CustomFieldDefinition customFieldDefinition in (IEnumerable) this.List)
      {
        if (customFieldDefinition.FieldId.Equals(fieldId))
          return true;
      }
      return false;
    }

    public bool ContainsFieldNumber(int fieldNumber)
    {
      foreach (CustomFieldDefinition customFieldDefinition in (IEnumerable) this.List)
      {
        if (customFieldDefinition.FieldNumber.Equals(fieldNumber))
          return true;
      }
      return false;
    }

    public bool Contains(string fieldDescription)
    {
      foreach (CustomFieldDefinition customFieldDefinition in (IEnumerable) this.List)
      {
        if (customFieldDefinition.FieldDescription.Equals(fieldDescription))
          return true;
      }
      return false;
    }

    public bool ContainsDuplicate(
      CustomFieldDefinition customFieldDefinition,
      string fieldDescription)
    {
      foreach (CustomFieldDefinition customFieldDefinition1 in (IEnumerable) this.List)
      {
        if (customFieldDefinition1.FieldDescription.Equals(fieldDescription, StringComparison.InvariantCultureIgnoreCase) && (customFieldDefinition == null || !customFieldDefinition1.FieldNumber.Equals(customFieldDefinition.FieldNumber)))
          return true;
      }
      return false;
    }

    public static CustomFieldDefinitionCollection NewCustomFieldDefinitionCollection()
    {
      return new CustomFieldDefinitionCollection();
    }

    private CustomFieldDefinitionCollection()
    {
    }

    [Serializable]
    public class Criteria
    {
      public CustomFieldsType CustomFieldsType;
      public int RecordId;

      public Criteria(CustomFieldsType customFieldsType, int recordId)
      {
        this.CustomFieldsType = customFieldsType;
        this.RecordId = recordId;
      }
    }
  }
}
