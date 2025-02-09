// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.BorrowerReportFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class BorrowerReportFieldDefs : ReportFieldDefs
  {
    public BorrowerReportFieldDefs()
      : base(Session.DefaultInstance)
    {
    }

    private BorrowerReportFieldDefs(string fileName)
      : base(Session.DefaultInstance, fileName)
    {
    }

    internal override ReportFieldDef CreateReportFieldDef(string category, XmlElement fieldElement)
    {
      return (ReportFieldDef) new BorrowerReportFieldDef(category, fieldElement);
    }

    internal override ReportFieldDef CreateReportFieldDef(FieldDefinition fieldDef)
    {
      return (ReportFieldDef) new BorrowerReportFieldDef(fieldDef);
    }

    public override string GetFieldPrefix() => "";

    public BorrowerReportFieldDef this[int index] => (BorrowerReportFieldDef) this.fieldDefs[index];

    public BorrowerReportFieldDef GetFieldByID(string fieldId)
    {
      return this.fieldIdLookup.ContainsKey(fieldId) ? (BorrowerReportFieldDef) this.fieldIdLookup[fieldId] : (BorrowerReportFieldDef) null;
    }

    public BorrowerReportFieldDef GetFieldByCriterionName(string dbname)
    {
      return this.dbnameLookup.ContainsKey(dbname) ? (BorrowerReportFieldDef) this.dbnameLookup[dbname] : (BorrowerReportFieldDef) null;
    }

    public static BorrowerReportFieldDefs GetFieldDefs(bool excludeHistoryFields)
    {
      return BorrowerReportFieldDefs.GetFieldDefs(Session.SessionObjects, Session.UserInfo, excludeHistoryFields);
    }

    public static BorrowerReportFieldDefs GetFieldDefs(
      Sessions.Session session,
      bool excludeHistoryFields)
    {
      return BorrowerReportFieldDefs.getFieldDefs(session.SessionObjects, session.UserInfo, excludeHistoryFields, session);
    }

    public static BorrowerReportFieldDefs GetFieldDefs(
      SessionObjects sessionObjects,
      UserInfo user,
      bool excludeHistoryFields)
    {
      return BorrowerReportFieldDefs.getFieldDefs(sessionObjects, user, excludeHistoryFields, (Sessions.Session) null);
    }

    public static BorrowerReportFieldDefs GetFieldDefs(BorrowerReportFieldFlags flags)
    {
      BorrowerReportFieldDefs borrowerReportFieldDefs = new BorrowerReportFieldDefs();
      foreach (BorrowerReportFieldDef fieldDef in (ReportFieldDefContainer) BorrowerReportFieldDefs.GetFieldDefs(Session.DefaultInstance, false))
        borrowerReportFieldDefs.Add((ReportFieldDef) fieldDef);
      return borrowerReportFieldDefs.ExtractFields(flags);
    }

    public static BorrowerReportFieldDefs GetFieldDefs(
      Sessions.Session session,
      BorrowerReportFieldFlags flags)
    {
      BorrowerReportFieldDefs borrowerReportFieldDefs = new BorrowerReportFieldDefs();
      foreach (BorrowerReportFieldDef fieldDef in (ReportFieldDefContainer) BorrowerReportFieldDefs.GetFieldDefs(session, false))
        borrowerReportFieldDefs.Add((ReportFieldDef) fieldDef);
      return borrowerReportFieldDefs.ExtractFields(flags);
    }

    public BorrowerReportFieldDefs ExtractFields(BorrowerReportFieldFlags flags)
    {
      BorrowerReportFieldDefs fields = new BorrowerReportFieldDefs();
      if ((flags & BorrowerReportFieldFlags.AllFields) == (BorrowerReportFieldFlags) 0)
        flags |= BorrowerReportFieldFlags.AllFields;
      Bitmask bitmask = new Bitmask((object) flags);
      foreach (ReportFieldDef fieldDef in this.fieldDefs)
      {
        bool flag = false;
        if (fieldDef.Category == "ContactHistory")
          flag = bitmask.Contains((object) BorrowerReportFieldFlags.IncludeHistoryFields);
        else if (!fieldDef.Selectable)
          flag = bitmask.Contains((object) BorrowerReportFieldFlags.IncludeNonSelectableFields);
        else if (fieldDef.Selectable)
          flag = true;
        if (flag)
          fields.Add(fieldDef);
      }
      return fields;
    }

    private static BorrowerReportFieldDefs insertDynamicFieldValue(
      BorrowerReportFieldDefs fieldDefs,
      IContactManager contactMgr,
      IContactGroup contactGroupMgr,
      string userID)
    {
      try
      {
        foreach (BorrowerReportFieldDef fieldDef in (ReportFieldDefContainer) fieldDefs)
        {
          if (fieldDef.FieldID == "Status")
          {
            BorrowerStatus borrowerStatus = contactMgr.GetBorrowerStatus();
            if (borrowerStatus != null && borrowerStatus.Items.Length != 0)
            {
              foreach (BorrowerStatusItem borrowerStatusItem in borrowerStatus.Items)
                fieldDef.FieldDefinition.Options.AddOption(borrowerStatusItem.name, borrowerStatusItem.name);
            }
          }
          else if (fieldDef.FieldID == "GroupName")
          {
            ContactGroupCollectionCriteria criteria = new ContactGroupCollectionCriteria(userID, ContactType.Borrower, new ContactGroupType[1]);
            foreach (ContactGroupInfo contactGroupInfo in contactGroupMgr.GetContactGroupsForUser(criteria))
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

    private static BorrowerReportFieldDefs getFieldDefs(
      SessionObjects sessionObjects,
      UserInfo user,
      bool excludeHistoryFields,
      Sessions.Session session)
    {
      IContactManager contactManager = sessionObjects.ContactManager;
      IContactGroup contactGroupManager = sessionObjects.ContactGroupManager;
      string userid = user.Userid;
      BorrowerReportFieldDefs fieldDefs = new BorrowerReportFieldDefs();
      foreach (BorrowerReportFieldDef fieldDef in (ReportFieldDefContainer) new BorrowerReportFieldDefs("BorrowerMap.xml"))
      {
        if ((!excludeHistoryFields || !(fieldDef.Category == "ContactHistory")) && fieldDef.FieldDefinition.AppliesToEdition(sessionObjects.ServerLicense.Edition))
          fieldDefs.Add((ReportFieldDef) fieldDef);
      }
      foreach (ContactCustomFieldInfo contactCustomFieldInfo in contactManager.GetCustomFieldInfo(ContactType.Borrower).Items)
      {
        CustomField field = contactCustomFieldInfo.FieldType == FieldFormat.DROPDOWN || contactCustomFieldInfo.FieldType == FieldFormat.DROPDOWNLIST ? new CustomField(new CustomFieldInfo("custom." + contactCustomFieldInfo.Label, contactCustomFieldInfo.Label, contactCustomFieldInfo.FieldType, contactCustomFieldInfo.FieldOptions)) : new CustomField(new CustomFieldInfo("custom." + contactCustomFieldInfo.Label, contactCustomFieldInfo.Label, contactCustomFieldInfo.FieldType, (string[]) null));
        if (session == null)
          fieldDefs.Add((ReportFieldDef) new BorrowerReportFieldDef(field));
        else
          fieldDefs.Add((ReportFieldDef) new BorrowerReportFieldDef(field));
      }
      return BorrowerReportFieldDefs.insertDynamicFieldValue(fieldDefs, contactManager, contactGroupManager, userid);
    }
  }
}
