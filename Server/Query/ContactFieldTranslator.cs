// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.ContactFieldTranslator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.Query
{
  internal class ContactFieldTranslator : ICriterionTranslator
  {
    private const string customFieldPrefix = "custom.�";
    private const string customCategoryFieldPrefix = "customcategory.�";
    private const string standardCategoryFieldPrefix = "standardcategory.�";
    private DbTableInfo customFieldTable;
    private Dictionary<string, int> fieldIdMap = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, FieldFormat> fieldLabelToType = new Dictionary<string, FieldFormat>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, CustomFieldDefinitionInfo> cusCategoryFieldList = new Dictionary<string, CustomFieldDefinitionInfo>();
    private Dictionary<string, CustomFieldDefinitionInfo> stdCategoryFieldList = new Dictionary<string, CustomFieldDefinitionInfo>();
    private LoanFieldTranslator loanTranslator;

    public ContactFieldTranslator(
      string customFieldTableName,
      ContactCustomFieldInfoCollection customFields)
    {
      this.loanTranslator = new LoanFieldTranslator();
      this.customFieldTable = EllieMae.EMLite.Server.DbAccessManager.GetTable(customFieldTableName);
      for (int index = 0; index < customFields.Items.Length; ++index)
      {
        if (!this.fieldIdMap.ContainsKey(customFields.Items[index].Label.ToLower()))
        {
          this.fieldIdMap.Add(customFields.Items[index].Label.ToLower().Replace(" ", ""), customFields.Items[index].LabelID);
          this.fieldLabelToType.Add(customFields.Items[index].Label.ToLower().Replace(" ", ""), customFields.Items[index].FieldType);
        }
      }
      BizCategory[] bizCategories = BizPartnerContact.GetBizCategories();
      Dictionary<int, string> dictionary = new Dictionary<int, string>();
      if (bizCategories != null && bizCategories.Length != 0)
      {
        foreach (BizCategory bizCategory in bizCategories)
          dictionary.Add(bizCategory.CategoryID, bizCategory.Name);
      }
      CustomFieldsDefinitionInfo[] fieldsDefinitions1 = BizPartnerContact.GetCustomFieldsDefinitions(CustomFieldsType.BizCategoryCustom);
      if (fieldsDefinitions1 != null && fieldsDefinitions1.Length != 0)
      {
        foreach (CustomFieldsDefinitionInfo fieldsDefinitionInfo in fieldsDefinitions1)
        {
          foreach (CustomFieldDefinitionInfo customFieldDefinition in fieldsDefinitionInfo.CustomFieldDefinitions)
          {
            string str = "";
            if (dictionary.ContainsKey(customFieldDefinition.CategoryId))
              str = dictionary[customFieldDefinition.CategoryId];
            if (str != "" && !this.cusCategoryFieldList.ContainsKey(str + "." + customFieldDefinition.FieldDescription.ToLower().Replace(" ", "")))
              this.cusCategoryFieldList.Add(str.ToLower().Replace(" ", "") + "." + customFieldDefinition.FieldDescription.ToLower().Replace(" ", ""), customFieldDefinition);
          }
        }
      }
      CustomFieldsDefinitionInfo[] fieldsDefinitions2 = BizPartnerContact.GetCustomFieldsDefinitions(CustomFieldsType.BizCategoryStandard);
      if (fieldsDefinitions2 == null || fieldsDefinitions2.Length == 0)
        return;
      foreach (CustomFieldsDefinitionInfo fieldsDefinitionInfo in fieldsDefinitions2)
      {
        foreach (CustomFieldDefinitionInfo customFieldDefinition in fieldsDefinitionInfo.CustomFieldDefinitions)
        {
          string str = "";
          if (dictionary.ContainsKey(customFieldDefinition.CategoryId))
            str = dictionary[customFieldDefinition.CategoryId];
          if (str != "" && !this.stdCategoryFieldList.ContainsKey(customFieldDefinition.CategoryId.ToString() + "." + customFieldDefinition.FieldDescription.ToLower().Replace(" ", "")))
            this.stdCategoryFieldList.Add(str.ToLower().Replace(" ", "") + "." + customFieldDefinition.FieldDescription.ToLower().Replace(" ", ""), customFieldDefinition);
        }
      }
    }

    public ICriterionNameFormatter CriterionNameFormatter { get; set; }

    public CriterionName TranslateName(string fieldName)
    {
      CriterionName criterionName = CriterionName.Parse(fieldName);
      if (criterionName.FieldSource == "")
        criterionName = new CriterionName("Contact", criterionName.FieldName);
      if (fieldName.ToLower().StartsWith("custom."))
        return this.translateCustomFieldCriterion(criterionName.FieldName);
      if (fieldName.ToLower().StartsWith("customcategory."))
        return this.translateCustomCategoryFieldCriterion(criterionName.FieldName);
      return fieldName.ToLower().StartsWith("standardcategory.") ? this.translateStandardCategoryFieldCriterion(criterionName.FieldName) : this.loanTranslator.TranslateName(criterionName.ToString());
    }

    private CriterionName translateCustomFieldCriterion(string custFieldName)
    {
      return !this.fieldIdMap.ContainsKey(custFieldName.ToLower().Replace(" ", "")) ? (CriterionName) null : new CriterionName("Custom_" + (object) this.fieldIdMap[custFieldName.ToLower().Replace(" ", "")], "FieldValue");
    }

    private CriterionName translateCustomCategoryFieldCriterion(string custFieldName)
    {
      return !this.cusCategoryFieldList.ContainsKey(custFieldName.ToLower().Replace(" ", "")) ? (CriterionName) null : new CriterionName("CustomCategory_" + (object) this.cusCategoryFieldList[custFieldName.ToLower().Replace(" ", "")].FieldId, "FieldValue");
    }

    private CriterionName translateStandardCategoryFieldCriterion(string custFieldName)
    {
      return !this.stdCategoryFieldList.ContainsKey(custFieldName.ToLower().Replace(" ", "")) ? (CriterionName) null : new CriterionName("StandardCategory_" + (object) this.stdCategoryFieldList[custFieldName.ToLower().Replace(" ", "")].FieldId, "FieldValue");
    }

    private CriterionName translateContactGroupFieldCriterion(string custFieldName)
    {
      return new CriterionName("ContactGroup", "GroupName");
    }

    public QueryCriterion TranslateCriterion(QueryCriterion cri)
    {
      if (cri is FieldValueCriterion fieldValueCriterion)
      {
        switch (fieldValueCriterion.FieldName.ToLower())
        {
          case "contactgroupcount.groupcount":
            switch (cri)
            {
              case StringValueCriterion _:
                return (QueryCriterion) new StringValueCriterion("ContactGroup.GroupName", ((StringValueCriterion) cri).Value, StringMatchType.Exact);
              case ListValueCriterion _:
                return (QueryCriterion) new ListValueCriterion("ContactGroup.GroupName", ((ListValueCriterion) cri).ValueList);
            }
            break;
          case "publicgroupcount.groupcount":
            switch (cri)
            {
              case StringValueCriterion _:
                return (QueryCriterion) new StringValueCriterion("PublicContactGroup.GroupName", ((StringValueCriterion) cri).Value, StringMatchType.Exact);
              case ListValueCriterion _:
                return (QueryCriterion) new ListValueCriterion("PublicContactGroup.GroupName", ((ListValueCriterion) cri).ValueList);
            }
            break;
        }
      }
      return this.loanTranslator.TranslateCriterion(cri);
    }
  }
}
