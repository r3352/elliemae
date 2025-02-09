// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CustomFields.CustomFieldsDefinitionCollection
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
  public class CustomFieldsDefinitionCollection : BusinessCollectionBase
  {
    private Hashtable members = new Hashtable();

    public CustomFieldsDefinition this[int index] => (CustomFieldsDefinition) this.List[index];

    public void Add(CustomFieldsDefinition customFieldsDefinition)
    {
      this.List.Add((object) customFieldsDefinition);
      this.members.Add((object) (customFieldsDefinition.CustomFieldsType.ToString() + "." + customFieldsDefinition.RecordId.ToString()), (object) customFieldsDefinition);
    }

    public void Remove(CustomFieldsDefinition customFieldsDefinition)
    {
      this.List.Remove((object) customFieldsDefinition);
      this.members.Remove((object) (customFieldsDefinition.CustomFieldsType.ToString() + "." + customFieldsDefinition.RecordId.ToString()));
    }

    public bool Contains(CustomFieldsDefinition customFieldsDefinition)
    {
      return this.Contains(customFieldsDefinition.CustomFieldsType, customFieldsDefinition.RecordId);
    }

    public bool Contains(CustomFieldsType customFieldsType, int recordId)
    {
      return this.members.Contains((object) (customFieldsType.ToString() + "." + recordId.ToString()));
    }

    public CustomFieldsDefinition Find(CustomFieldsDefinition customFieldsDefinition)
    {
      return this.Find(customFieldsDefinition.CustomFieldsType, customFieldsDefinition.RecordId);
    }

    public CustomFieldsDefinition Find(CustomFieldsType customFieldsType, int recordId)
    {
      return (CustomFieldsDefinition) this.members[(object) (customFieldsType.ToString() + "." + recordId.ToString())];
    }

    public static CustomFieldsDefinitionCollection NewCustomFieldsDefinitionCollection()
    {
      return new CustomFieldsDefinitionCollection();
    }

    public static CustomFieldsDefinitionCollection GetCustomFieldsDefinitionCollection(
      SessionObjects sessionObjects,
      CustomFieldsDefinitionCollection.Criteria criteria)
    {
      CustomFieldsDefinitionCollection definitionCollection = new CustomFieldsDefinitionCollection();
      CustomFieldsDefinitionInfo[] fieldsDefinitions = sessionObjects.ContactManager.GetCustomFieldsDefinitions(criteria.CustomFieldsType);
      if (fieldsDefinitions != null)
      {
        foreach (CustomFieldsDefinitionInfo customFieldsDefinitionInfo in fieldsDefinitions)
          definitionCollection.Add(CustomFieldsDefinition.NewCustomFieldsDefinition(sessionObjects, customFieldsDefinitionInfo));
      }
      return definitionCollection;
    }

    private CustomFieldsDefinitionCollection()
    {
    }

    [Serializable]
    public class Criteria
    {
      public CustomFieldsType CustomFieldsType;

      public Criteria(CustomFieldsType customFieldsType)
      {
        this.CustomFieldsType = customFieldsType;
      }
    }
  }
}
