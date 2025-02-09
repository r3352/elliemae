// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CustomFields.CustomFieldMappingCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.CustomFields;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CustomFields
{
  [Serializable]
  public class CustomFieldMappingCollection : BusinessCollectionBase
  {
    private CustomFieldMappingCollection.Criteria criteria;

    public CustomFieldMapping this[int index] => (CustomFieldMapping) this.List[index];

    public void Add(CustomFieldMapping customFieldMapping)
    {
      this.List.Add((object) customFieldMapping);
    }

    public void Remove(CustomFieldMapping customFieldMapping)
    {
      this.List.Remove((object) customFieldMapping);
    }

    public CustomFieldMapping Find(
      CustomFieldsType customFieldsType,
      int categoryId,
      int fieldNumber)
    {
      foreach (CustomFieldMapping customFieldMapping in (IEnumerable) this.List)
      {
        if (customFieldMapping.CustomFieldsType.Equals((object) customFieldsType))
        {
          int num = customFieldMapping.CategoryId;
          if (num.Equals(categoryId))
          {
            num = customFieldMapping.FieldNumber;
            if (num.Equals(fieldNumber))
              return customFieldMapping;
          }
        }
      }
      return (CustomFieldMapping) null;
    }

    public CustomFieldMapping Find(string loanFieldId)
    {
      foreach (CustomFieldMapping customFieldMapping in (IEnumerable) this.List)
      {
        if (customFieldMapping.LoanFieldId.Equals(loanFieldId))
          return customFieldMapping;
      }
      return (CustomFieldMapping) null;
    }

    public bool Contains(CustomFieldMapping customFieldMapping)
    {
      foreach (CustomFieldMapping customFieldMapping1 in (IEnumerable) this.List)
      {
        if (customFieldMapping1.Equals(customFieldMapping))
          return true;
      }
      return false;
    }

    public bool Contains(CustomFieldsType customFieldsType, int categoryId, int fieldNumber)
    {
      foreach (CustomFieldMapping customFieldMapping in (IEnumerable) this.List)
      {
        if (customFieldMapping.CustomFieldsType.Equals((object) customFieldsType))
        {
          int num = customFieldMapping.CategoryId;
          if (num.Equals(categoryId))
          {
            num = customFieldMapping.FieldNumber;
            if (num.Equals(fieldNumber))
              return true;
          }
        }
      }
      return false;
    }

    public bool Contains(string loanFieldId)
    {
      foreach (CustomFieldMapping customFieldMapping in (IEnumerable) this.List)
      {
        if (customFieldMapping.LoanFieldId.Equals(loanFieldId))
          return true;
      }
      return false;
    }

    public bool ContainsDuplicate(CustomFieldMapping customFieldMapping, string loanFieldId)
    {
      foreach (CustomFieldMapping customFieldMapping1 in (IEnumerable) this.List)
      {
        if (customFieldMapping1.LoanFieldId.Equals(loanFieldId) && !customFieldMapping1.Equals(customFieldMapping))
          return true;
      }
      return false;
    }

    public static CustomFieldMappingCollection NewCustomFieldMappingCollection()
    {
      return new CustomFieldMappingCollection();
    }

    public static CustomFieldMappingCollection GetCustomFieldMappingCollection(
      SessionObjects sessionObjects,
      CustomFieldMappingCollection.Criteria criteria)
    {
      CustomFieldMappingCollection mappingCollection = new CustomFieldMappingCollection();
      mappingCollection.criteria = criteria;
      foreach (CustomFieldMappingInfo customFieldMapping in sessionObjects.ContactManager.GetCustomFieldsMapping(criteria.CustomFieldsType, criteria.CategoryId, criteria.TwoWayTransfersOnly).CustomFieldMappings)
        mappingCollection.Add(CustomFieldMapping.NewCustomFieldMapping(customFieldMapping));
      return mappingCollection;
    }

    public static CustomFieldMappingCollection GetCustomFieldMappingCollection(
      CustomFieldsMappingInfo fieldsInfo,
      CustomFieldMappingCollection.Criteria criteria)
    {
      CustomFieldMappingCollection mappingCollection = new CustomFieldMappingCollection();
      mappingCollection.criteria = criteria;
      foreach (CustomFieldMappingInfo customFieldMapping in fieldsInfo.CustomFieldMappings)
        mappingCollection.Add(CustomFieldMapping.NewCustomFieldMapping(customFieldMapping));
      return mappingCollection;
    }

    private CustomFieldMappingCollection()
    {
    }

    [Serializable]
    public class Criteria
    {
      public CustomFieldsType CustomFieldsType;
      public int CategoryId = -1;
      public bool TwoWayTransfersOnly;

      public Criteria(CustomFieldsType customFieldsType, bool twoWayTransfersOnly)
      {
        this.CustomFieldsType = customFieldsType;
        this.TwoWayTransfersOnly = twoWayTransfersOnly;
      }

      public Criteria(CustomFieldsType customFieldsType, int categoryId, bool twoWayTransfersOnly)
      {
        this.CustomFieldsType = customFieldsType;
        this.CategoryId = categoryId;
        this.TwoWayTransfersOnly = twoWayTransfersOnly;
      }
    }
  }
}
