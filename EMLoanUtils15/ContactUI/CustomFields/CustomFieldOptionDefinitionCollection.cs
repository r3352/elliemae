// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CustomFields.CustomFieldOptionDefinitionCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CustomFields
{
  [Serializable]
  public class CustomFieldOptionDefinitionCollection : BusinessCollectionBase
  {
    public CustomFieldOptionDefinition this[int index]
    {
      get => (CustomFieldOptionDefinition) this.List[index];
    }

    public void Add(
      CustomFieldOptionDefinition customFieldOptionDefinition)
    {
      this.List.Add((object) customFieldOptionDefinition);
    }

    public void Remove(
      CustomFieldOptionDefinition customFieldOptionDefinition)
    {
      this.List.Remove((object) customFieldOptionDefinition);
    }

    public string[] GetOptionValues()
    {
      string[] optionValues = new string[this.Count];
      int num = 0;
      foreach (CustomFieldOptionDefinition optionDefinition in (IEnumerable) this.List)
        optionValues[num++] = optionDefinition.OptionValue;
      return optionValues;
    }

    public void SetOptionValues(string[] optionValues)
    {
      bool flag = true;
      if (this.Count == optionValues.Length)
      {
        flag = false;
        for (int index = 0; index < this.Count; ++index)
        {
          if (this[index].OptionValue != optionValues[index])
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        return;
      for (int index = this.Count - 1; index >= 0; --index)
        this.Remove(this[index]);
      int num = 1;
      foreach (string optionValue in optionValues)
      {
        CustomFieldOptionDefinition customFieldOptionDefinition = CustomFieldOptionDefinition.NewCustomFieldOptionDefinition();
        customFieldOptionDefinition.OptionNumber = num++;
        customFieldOptionDefinition.OptionValue = optionValue;
        this.Add(customFieldOptionDefinition);
      }
    }

    public bool Contains(
      CustomFieldOptionDefinition customFieldOptionDefinition)
    {
      foreach (CustomFieldOptionDefinition optionDefinition in (IEnumerable) this.List)
      {
        if (optionDefinition.Equals(customFieldOptionDefinition))
          return true;
      }
      return false;
    }

    public bool Contains(string optionValue)
    {
      foreach (CustomFieldOptionDefinition optionDefinition in (IEnumerable) this.List)
      {
        if (optionDefinition.OptionValue.Equals(optionValue))
          return true;
      }
      return false;
    }

    public bool ContainsDeleted(
      CustomFieldOptionDefinition customFieldOptionDefinition)
    {
      foreach (CustomFieldOptionDefinition deleted in (CollectionBase) this.deletedList)
      {
        if (deleted.Equals(customFieldOptionDefinition))
          return true;
      }
      return false;
    }

    public static CustomFieldOptionDefinitionCollection NewCustomFieldOptionDefinitionCollection()
    {
      return new CustomFieldOptionDefinitionCollection();
    }

    private CustomFieldOptionDefinitionCollection()
    {
    }

    [Serializable]
    public class Criteria
    {
      public int FieldId;

      public Criteria(int fieldId) => this.FieldId = fieldId;
    }
  }
}
