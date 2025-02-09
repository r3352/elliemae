// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.BizPartnerReportFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class BizPartnerReportFieldDefs : ReportFieldDefs
  {
    public BizPartnerReportFieldDefs(Sessions.Session session)
      : base(session)
    {
    }

    internal override ReportFieldDef CreateReportFieldDef(string category, XmlElement fieldElement)
    {
      return (ReportFieldDef) new BizPartnerReportFieldDef(category, fieldElement);
    }

    internal override ReportFieldDef CreateReportFieldDef(FieldDefinition fieldDef)
    {
      return (ReportFieldDef) new BizPartnerReportFieldDef(fieldDef);
    }

    private BizPartnerReportFieldDefs(Sessions.Session session, string fileName)
      : base(session, fileName)
    {
    }

    public BizPartnerReportFieldDef this[int index]
    {
      get => (BizPartnerReportFieldDef) this.fieldDefs[index];
    }

    public BizPartnerReportFieldDef GetFieldByID(string fieldId)
    {
      return this.fieldIdLookup.ContainsKey(fieldId) ? (BizPartnerReportFieldDef) this.fieldIdLookup[fieldId] : (BizPartnerReportFieldDef) null;
    }

    public BizPartnerReportFieldDef GetFieldByCriterionName(string dbname)
    {
      return this.dbnameLookup.ContainsKey(dbname) ? (BizPartnerReportFieldDef) this.dbnameLookup[dbname] : (BizPartnerReportFieldDef) null;
    }

    public static BizPartnerReportFieldDefs GetFieldDefs(
      bool excludeHistoryFields,
      ContactType type)
    {
      return BizPartnerReportFieldDefs.GetFieldDefs(Session.DefaultInstance, Session.UserInfo, excludeHistoryFields, type);
    }

    public static BizPartnerReportFieldDefs GetFieldDefs(
      SessionObjects sessionObjects,
      UserInfo userObj,
      bool excludeHistoryFields,
      ContactType type)
    {
      return BizPartnerReportFieldDefs.getFieldDefs(Session.DefaultInstance, userObj, excludeHistoryFields, type);
    }

    public static BizPartnerReportFieldDefs GetFieldDefs(
      Sessions.Session session,
      bool excludeHistoryFields,
      ContactType type)
    {
      return BizPartnerReportFieldDefs.GetFieldDefs(session, session.UserInfo, excludeHistoryFields, type);
    }

    public static BizPartnerReportFieldDefs GetFieldDefs(
      Sessions.Session session,
      UserInfo userObj,
      bool excludeHistoryFields,
      ContactType type)
    {
      return BizPartnerReportFieldDefs.getFieldDefs(session, userObj, excludeHistoryFields, type);
    }

    public static BizPartnerReportFieldDefs GetFieldDefs(
      BizPartnerReportFieldFlags flags,
      ContactType type)
    {
      BizPartnerReportFieldDefs partnerReportFieldDefs = new BizPartnerReportFieldDefs(Session.DefaultInstance);
      foreach (BizPartnerReportFieldDef fieldDef in (ReportFieldDefContainer) BizPartnerReportFieldDefs.GetFieldDefs(false, type))
        partnerReportFieldDefs.Add((ReportFieldDef) fieldDef);
      return partnerReportFieldDefs.ExtractFields(flags);
    }

    public BizPartnerReportFieldDefs ExtractFields(BizPartnerReportFieldFlags flags)
    {
      BizPartnerReportFieldDefs fields = new BizPartnerReportFieldDefs(Session.DefaultInstance);
      if ((flags & BizPartnerReportFieldFlags.AllFields) == (BizPartnerReportFieldFlags) 0)
        flags |= BizPartnerReportFieldFlags.AllFields;
      Bitmask bitmask = new Bitmask((object) flags);
      foreach (ReportFieldDef fieldDef in this.fieldDefs)
      {
        bool flag = false;
        if (fieldDef.Category == "ContactHistory")
          flag = bitmask.Contains((object) BizPartnerReportFieldFlags.IncludeHistoryFields);
        else if (!fieldDef.Selectable)
          flag = bitmask.Contains((object) BizPartnerReportFieldFlags.IncludeNonSelectableFields);
        else if (fieldDef.Selectable)
          flag = true;
        if (flag)
          fields.Add(fieldDef);
      }
      return fields;
    }

    public static BizPartnerReportFieldDefs GetSelectableFieldDefs(
      Sessions.Session session,
      bool excludeHistoryFields,
      ContactType type)
    {
      BizPartnerReportFieldDefs selectableFieldDefs = new BizPartnerReportFieldDefs(session);
      foreach (BizPartnerReportFieldDef fieldDef in (ReportFieldDefContainer) BizPartnerReportFieldDefs.GetFieldDefs(session, excludeHistoryFields, type))
      {
        if (fieldDef.Selectable)
          selectableFieldDefs.Add((ReportFieldDef) fieldDef);
      }
      return selectableFieldDefs;
    }

    public override string GetFieldPrefix() => "";

    private static BizPartnerReportFieldDefs getFieldDefs(
      Sessions.Session session,
      UserInfo userObj,
      bool excludeHistoryFields,
      ContactType type)
    {
      IContactManager contactManager = session.ContactManager;
      IContactGroup contactGroupManager = session.ContactGroupManager;
      IAclGroupManager aclGroupManager = session.AclGroupManager;
      BizPartnerReportFieldDefs fieldDefs = new BizPartnerReportFieldDefs(session);
      foreach (BizPartnerReportFieldDef fieldDef in (ReportFieldDefContainer) new BizPartnerReportFieldDefs(session, "BizPartnerMap.xml"))
      {
        if ((!excludeHistoryFields || !(fieldDef.Category == "ContactHistory")) && (type != ContactType.PublicBiz || !(fieldDef.FieldID == "BizContactGroups") && !(fieldDef.FieldID == "BizGroupName")) && (type != ContactType.BizPartner || !(fieldDef.FieldID == "PubBizContactGroups") && !(fieldDef.FieldID == "PublicContactGroupName")) && fieldDef.FieldDefinition.AppliesToEdition(session.ServerLicense.Edition))
          fieldDefs.Add((ReportFieldDef) fieldDef);
      }
      ContactCustomFieldInfoCollection customFieldInfo = contactManager.GetCustomFieldInfo(ContactType.BizPartner);
      if (customFieldInfo != null && customFieldInfo.Items.Length != 0)
      {
        foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
        {
          CustomField field = contactCustomFieldInfo.FieldType == FieldFormat.DROPDOWN || contactCustomFieldInfo.FieldType == FieldFormat.DROPDOWNLIST ? new CustomField(new CustomFieldInfo("custom." + contactCustomFieldInfo.Label, contactCustomFieldInfo.Label, contactCustomFieldInfo.FieldType, contactCustomFieldInfo.FieldOptions)) : new CustomField(new CustomFieldInfo("custom." + contactCustomFieldInfo.Label, contactCustomFieldInfo.Label, contactCustomFieldInfo.FieldType, (string[]) null));
          fieldDefs.Add((ReportFieldDef) new BizPartnerReportFieldDef(field));
        }
      }
      BizCategory[] bizCategories = contactManager.GetBizCategories();
      Dictionary<int, string> dictionary = new Dictionary<int, string>();
      if (bizCategories != null && bizCategories.Length != 0)
      {
        foreach (BizCategory bizCategory in bizCategories)
          dictionary.Add(bizCategory.CategoryID, bizCategory.Name);
      }
      CustomFieldsDefinitionInfo[] fieldsDefinitions1 = contactManager.GetCustomFieldsDefinitions(CustomFieldsType.BizCategoryCustom);
      if (fieldsDefinitions1 != null && fieldsDefinitions1.Length != 0)
      {
        foreach (CustomFieldsDefinitionInfo fieldsDefinitionInfo in fieldsDefinitions1)
        {
          foreach (CustomFieldDefinitionInfo customFieldDefinition in fieldsDefinitionInfo.CustomFieldDefinitions)
          {
            string str = "None";
            if (dictionary.ContainsKey(customFieldDefinition.CategoryId))
              str = dictionary[customFieldDefinition.CategoryId];
            CustomField field;
            if (customFieldDefinition.FieldFormat == FieldFormat.DROPDOWN || customFieldDefinition.FieldFormat == FieldFormat.DROPDOWNLIST)
            {
              List<string> stringList = new List<string>();
              if (customFieldDefinition.CustomFieldOptionDefinitions != null)
              {
                foreach (CustomFieldOptionDefinitionInfo optionDefinition in customFieldDefinition.CustomFieldOptionDefinitions)
                  stringList.Add(optionDefinition.OptionValue);
              }
              field = new CustomField(new CustomFieldInfo("customcategory." + str + "." + customFieldDefinition.FieldDescription, customFieldDefinition.FieldDescription + " for " + str, customFieldDefinition.FieldFormat, stringList.ToArray()));
            }
            else
              field = new CustomField(new CustomFieldInfo("customcategory." + str + "." + customFieldDefinition.FieldDescription, customFieldDefinition.FieldDescription + " for " + str, customFieldDefinition.FieldFormat, (string[]) null));
            fieldDefs.Add((ReportFieldDef) new BizPartnerReportFieldDef(field));
          }
        }
      }
      CustomFieldsDefinitionInfo[] fieldsDefinitions2 = contactManager.GetCustomFieldsDefinitions(CustomFieldsType.BizCategoryStandard);
      if (fieldsDefinitions2 != null && fieldsDefinitions2.Length != 0)
      {
        foreach (CustomFieldsDefinitionInfo fieldsDefinitionInfo in fieldsDefinitions2)
        {
          foreach (CustomFieldDefinitionInfo customFieldDefinition in fieldsDefinitionInfo.CustomFieldDefinitions)
          {
            string str = "None";
            if (dictionary.ContainsKey(customFieldDefinition.CategoryId))
              str = dictionary[customFieldDefinition.CategoryId];
            CustomField field;
            if (customFieldDefinition.FieldFormat == FieldFormat.DROPDOWN || customFieldDefinition.FieldFormat == FieldFormat.DROPDOWNLIST)
            {
              List<string> stringList = new List<string>();
              if (customFieldDefinition.CustomFieldOptionDefinitions != null)
              {
                foreach (CustomFieldOptionDefinitionInfo optionDefinition in customFieldDefinition.CustomFieldOptionDefinitions)
                  stringList.Add(optionDefinition.OptionValue);
              }
              field = new CustomField(new CustomFieldInfo("standardcategory." + str + "." + customFieldDefinition.FieldDescription, customFieldDefinition.FieldDescription + " for " + str, customFieldDefinition.FieldFormat, stringList.ToArray()));
            }
            else
              field = new CustomField(new CustomFieldInfo("standardcategory." + str + "." + customFieldDefinition.FieldDescription, customFieldDefinition.FieldDescription + " for " + str, customFieldDefinition.FieldFormat, (string[]) null));
            fieldDefs.Add((ReportFieldDef) new BizPartnerReportFieldDef(field));
          }
        }
      }
      return BizPartnerReportFieldDefs.insertDynamicFieldValue(fieldDefs, contactManager, contactGroupManager, aclGroupManager, userObj);
    }

    private static BizPartnerReportFieldDefs insertDynamicFieldValue(
      BizPartnerReportFieldDefs fieldDefs,
      IContactManager contactMgr,
      IContactGroup contactGroupMgr,
      IAclGroupManager aclGroupMgr,
      UserInfo userObj)
    {
      try
      {
        foreach (BizPartnerReportFieldDef fieldDef in (ReportFieldDefContainer) fieldDefs)
        {
          if (fieldDef.FieldID == "CategoryID")
          {
            foreach (BizCategory bizCategory in contactMgr.GetBizCategories())
              fieldDef.FieldDefinition.Options.AddOption(bizCategory.Name, bizCategory.Name ?? "");
          }
          else if (fieldDef.FieldID == "BizState")
          {
            foreach (string state in Utils.GetStates())
              fieldDef.FieldDefinition.Options.AddOption(state, state ?? "");
          }
          else if (fieldDef.FieldID == "BizGroupName")
          {
            ContactGroupCollectionCriteria criteria = new ContactGroupCollectionCriteria(userObj.Userid, ContactType.BizPartner, new ContactGroupType[1]);
            foreach (ContactGroupInfo contactGroupInfo in contactGroupMgr.GetContactGroupsForUser(criteria))
              fieldDef.FieldDefinition.Options.AddOption(contactGroupInfo.GroupName, contactGroupInfo.GroupName);
          }
          else if (fieldDef.FieldID == "PublicContactGroupName")
          {
            ContactGroupInfo[] contactGroupInfoArray = contactGroupMgr.GetPublicBizContactGroups();
            if (!userObj.IsSuperAdministrator())
            {
              BizGroupRef[] contactGroupRefs = aclGroupMgr.GetBizContactGroupRefs(userObj.Userid, true);
              ArrayList arrayList = new ArrayList();
              foreach (ContactGroupInfo contactGroupInfo in contactGroupInfoArray)
              {
                foreach (BizGroupRef bizGroupRef in contactGroupRefs)
                {
                  if (bizGroupRef.BizGroupID == contactGroupInfo.GroupId && !arrayList.Contains((object) contactGroupInfo))
                  {
                    arrayList.Add((object) contactGroupInfo);
                    break;
                  }
                }
              }
              contactGroupInfoArray = (ContactGroupInfo[]) arrayList.ToArray(typeof (ContactGroupInfo));
            }
            foreach (ContactGroupInfo contactGroupInfo in contactGroupInfoArray)
              fieldDef.FieldDefinition.Options.AddOption(contactGroupInfo.GroupName, contactGroupInfo.GroupName);
          }
        }
        return fieldDefs;
      }
      catch (Exception ex)
      {
        return fieldDefs;
      }
    }
  }
}
