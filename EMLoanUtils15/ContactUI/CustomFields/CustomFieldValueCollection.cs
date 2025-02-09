// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CustomFields.CustomFieldValueCollection
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
  public class CustomFieldValueCollection : BusinessCollectionBase
  {
    private CustomFieldValueCollection.Criteria criteria;
    private SessionObjects sessionObjects;

    public CustomFieldValue this[int index] => (CustomFieldValue) this.List[index];

    public void Add(CustomFieldValue customFieldValue) => this.List.Add((object) customFieldValue);

    public void Remove(CustomFieldValue customFieldValue)
    {
      this.List.Remove((object) customFieldValue);
    }

    public CustomFieldValue Find(int fieldId)
    {
      foreach (CustomFieldValue customFieldValue in (IEnumerable) this.List)
      {
        if (customFieldValue.FieldId.Equals(fieldId))
          return customFieldValue;
      }
      return (CustomFieldValue) null;
    }

    public bool Contains(CustomFieldValue customFieldValue)
    {
      foreach (CustomFieldValue customFieldValue1 in (IEnumerable) this.List)
      {
        if (customFieldValue1.Equals(customFieldValue))
          return true;
      }
      return false;
    }

    public bool Contains(int fieldId)
    {
      foreach (CustomFieldValue customFieldValue in (IEnumerable) this.List)
      {
        if (customFieldValue.FieldId.Equals(fieldId))
          return true;
      }
      return false;
    }

    public CustomFieldValueCollection Save()
    {
      CustomFieldsValueInfo customFieldsValues = new CustomFieldsValueInfo(this.criteria.ContactId, this.criteria.CategoryId, new CustomFieldValueInfo[this.AddedCount + this.UpdatedCount]);
      int num = 0;
      foreach (CustomFieldValue customFieldValue in (IEnumerable) this.List)
      {
        if (customFieldValue.IsDirty)
          customFieldsValues.CustomFieldValues[num++] = customFieldValue.GetInfo();
      }
      CustomFieldsValueInfo customFieldsValueInfo = this.sessionObjects.ContactManager.UpdateCustomFieldsValues(customFieldsValues);
      CustomFieldValueCollection fieldValueCollection = CustomFieldValueCollection.NewCustomFieldValueCollection(this.sessionObjects, this.criteria);
      foreach (CustomFieldValueInfo customFieldValue in customFieldsValueInfo.CustomFieldValues)
        fieldValueCollection.Add(CustomFieldValue.NewCustomFieldValue(customFieldValue));
      return fieldValueCollection;
    }

    public static CustomFieldValueCollection NewCustomFieldValueCollection(
      SessionObjects sessionObjects,
      CustomFieldValueCollection.Criteria criteria)
    {
      return new CustomFieldValueCollection(sessionObjects)
      {
        criteria = criteria
      };
    }

    public static CustomFieldValueCollection GetCustomFieldValueCollection(
      SessionObjects sessionObjects,
      CustomFieldValueCollection.Criteria criteria)
    {
      CustomFieldsValueInfo customFieldsValues = sessionObjects.ContactManager.GetCustomFieldsValues(criteria.ContactId, criteria.CategoryId);
      CustomFieldValueCollection fieldValueCollection = new CustomFieldValueCollection(sessionObjects);
      fieldValueCollection.criteria = criteria;
      foreach (CustomFieldValueInfo customFieldValue in customFieldsValues.CustomFieldValues)
        fieldValueCollection.Add(CustomFieldValue.NewCustomFieldValue(customFieldValue));
      return fieldValueCollection;
    }

    private CustomFieldValueCollection(SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
    }

    [Serializable]
    public class Criteria
    {
      public int ContactId;
      public int CategoryId;

      public Criteria(int contactId, int categoryId)
      {
        this.ContactId = contactId;
        this.CategoryId = categoryId;
      }
    }
  }
}
