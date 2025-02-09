// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.BizPartnerQuery
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.Query
{
  internal class BizPartnerQuery : QueryEngine
  {
    private LoanQuery loanQuery;
    private Dictionary<string, FieldFormat> cusFieldIdMap = new Dictionary<string, FieldFormat>();
    private Dictionary<string, FieldFormat> cusCategoryIdMap = new Dictionary<string, FieldFormat>();
    private Dictionary<string, FieldFormat> stdCategoryIdMap = new Dictionary<string, FieldFormat>();
    private RelatedLoanMatchType loanMatchType;

    public BizPartnerQuery(UserInfo currentUser, RelatedLoanMatchType loanMatchType)
      : base(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType, currentUser, (ICriterionTranslator) new ContactFieldTranslator("BizCustomField", BizPartnerCustomFields.Get()))
    {
      this.loanMatchType = loanMatchType;
      this.loanQuery = new LoanQuery(currentUser);
      ContactCustomFieldInfoCollection fieldInfoCollection = BizPartnerCustomFields.Get();
      for (int index = 0; index < fieldInfoCollection.Items.Length; ++index)
      {
        if (!this.cusFieldIdMap.ContainsKey(string.Concat((object) fieldInfoCollection.Items[index].LabelID)))
          this.cusFieldIdMap.Add(string.Concat((object) fieldInfoCollection.Items[index].LabelID), fieldInfoCollection.Items[index].FieldType);
      }
      CustomFieldsDefinitionInfo[] fieldsDefinitions1 = BizPartnerContact.GetCustomFieldsDefinitions(CustomFieldsType.BizCategoryCustom);
      if (fieldsDefinitions1 != null && fieldsDefinitions1.Length != 0)
      {
        foreach (CustomFieldsDefinitionInfo fieldsDefinitionInfo in fieldsDefinitions1)
        {
          foreach (CustomFieldDefinitionInfo customFieldDefinition in fieldsDefinitionInfo.CustomFieldDefinitions)
          {
            if (!this.cusCategoryIdMap.ContainsKey(string.Concat((object) customFieldDefinition.FieldId)))
              this.cusCategoryIdMap.Add(string.Concat((object) customFieldDefinition.FieldId), customFieldDefinition.FieldFormat);
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
          if (!this.stdCategoryIdMap.ContainsKey(string.Concat((object) customFieldDefinition.FieldId)))
            this.stdCategoryIdMap.Add(string.Concat((object) customFieldDefinition.FieldId), customFieldDefinition.FieldFormat);
        }
      }
    }

    public override string PrimaryKeyTableIdentifier => "BizPartner Contact";

    public override string PrimaryKeyIdentifier => "Contact.ContactID";

    public override string UserAccessQueryKeyColumnName => "ContactID";

    public override FieldSource GetFieldSource(string name)
    {
      switch (name.ToLower())
      {
        case "bizcategory":
          return new FieldSource("BizCategory", "left outer join BizCategory BizCategory on BizCategory.CategoryID = Contact.CategoryID", new string[1]
          {
            "Contact"
          });
        case "call":
          return new FieldSource("Call", "left outer join BizPartnerHistory Call on Contact.ContactID = Call.ContactID and Call.EventType = 'Called'", new string[1]
          {
            "Contact"
          });
        case "callnotes":
          return new FieldSource("CallNotes", "left outer join BizPartnerNotes CallNotes on Call.LoanID = CallNotes.NoteID", new string[2]
          {
            "Contact",
            "Call"
          });
        case "campaigncall":
          return new FieldSource("CampaignCall", "left outer join BizPartnerHistory CampaignCall on Contact.ContactID = CampaignCall.ContactID and CampaignCall.EventType = 'Campaign Phone Call'", new string[1]
          {
            "Contact"
          });
        case "campaignemail":
          return new FieldSource("CampaignEmail", "left outer join BizPartnerHistory CampaignEmail on Contact.ContactID = CampaignEmail.ContactID and CampaignEmail.EventType = 'Campaign Email'", new string[1]
          {
            "Contact"
          });
        case "campaignfax":
          return new FieldSource("CampaignFax", "left outer join BizPartnerHistory CampaignFax on Contact.ContactID = CampaignFax.ContactID and CampaignFax.EventType = 'Campaign Fax'", new string[1]
          {
            "Contact"
          });
        case "campaignletter":
          return new FieldSource("CampaignLetter", "left outer join BizPartnerHistory CampaignLetter on Contact.ContactID = CampaignLetter.ContactID and CampaignLetter.EventType = 'Campaign Letter'", new string[1]
          {
            "Contact"
          });
        case "campaignreminder":
          return new FieldSource("CampaignReminder", "left outer join BizPartnerHistory CampaignReminder on Contact.ContactID = CampaignReminder.ContactID and CampaignReminder.EventType = 'Campaign Reminder'", new string[1]
          {
            "Contact"
          });
        case "contactcategory":
          return new FieldSource("ContactCategory", "left outer join BizCategory ContactCategory on (ContactCategory.CategoryID = Contact.CategoryID) ", new string[1]
          {
            "Contact"
          });
        case "contactcompleteloancount":
          return new FieldSource("ContactCompleteLoanCount", "left outer join FN_GetBizPartnerLoansCount('Y') ContactCompleteLoanCount on Contact.ContactID = ContactCompleteLoanCount.ContactID", new string[1]
          {
            "Contact"
          });
        case "contactgroup":
          string str1 = "left outer join AclGroupPublicBizGroupRef GroupAccess on (GroupAccess.bizGroupID = ContactGroup.GroupID) ";
          if (this.CurrentUser != (UserInfo) null)
            str1 = str1 + "left outer join FN_GetUsersAclGroups(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.RunAsUserId) + ") UserGroup on (UserGroup.GroupID = GroupAccess.aclGroupID)";
          return new FieldSource("ContactGroup", "left outer join ContactGroup on ContactGroup.GroupID = ContactGroupXref.GroupID " + str1, new string[2]
          {
            "Contact",
            "ContactGroupXref"
          });
        case "contactgroupcount":
          return new FieldSource("ContactGroupCount", "left outer join FN_GetBizContactGroupCount(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.RunAsUserId) + ", 1) ContactGroupCount on Contact.ContactID = ContactGroupCount.ContactID", new string[1]
          {
            "Contact"
          });
        case "contactgroupxref":
          return new FieldSource("ContactGroupXref", "left outer join ContactGroupPartners ContactGroupXref on ContactGroupXref.ContactID = Contact.ContactID", new string[1]
          {
            "Contact"
          });
        case "contactgroupxrefmngr":
          return new FieldSource("ContactGroupXrefMngr", "left outer join ContactGroupPartners ContactGroupXrefMngr on ContactGroupXrefMngr.ContactID = Contact.ContactID", new string[1]
          {
            "Contact"
          });
        case "contactloancount":
          return new FieldSource("ContactLoanCount", "left outer join FN_GetBizPartnerLoansCount('') ContactLoanCount on Contact.ContactID = ContactLoanCount.ContactID", new string[1]
          {
            "Contact"
          });
        case "contactloans":
          return this.loanMatchType == RelatedLoanMatchType.LastOriginated ? new FieldSource("ContactLoans", "left outer join BizPartnerLastStartedLoans ContactLoans on Contact.Guid = ContactLoans.ContactGuid", new string[1]
          {
            "Contact"
          }) : (this.loanMatchType == RelatedLoanMatchType.LastClosed ? new FieldSource("ContactLoans", "left outer join BizPartnerLastCompletedLoans ContactLoans on Contact.Guid = ContactLoans.ContactGuid", new string[1]
          {
            "Contact"
          }) : new FieldSource("ContactLoans", "left outer join BizPartnerLoans ContactLoans on Contact.Guid = ContactLoans.ContactGuid", new string[1]
          {
            "Contact"
          }));
        case "contactowner":
          return new FieldSource("ContactOwner", "left outer join Users ContactOwner on Contact.OwnerID = ContactOwner.UserID", new string[1]
          {
            "Contact"
          });
        case "email":
          return new FieldSource("Email", "left outer join BizPartnerHistory Email on Contact.ContactID = Email.ContactID and Email.EventType = 'Emailed'", new string[1]
          {
            "Contact"
          });
        case "emailmerge":
          return new FieldSource("EmailMerge", "left outer join BizPartnerHistory EmailMerge on Contact.ContactID = EmailMerge.ContactID and EmailMerge.EventType = 'Email Merge'", new string[1]
          {
            "Contact"
          });
        case "emailnotes":
          return new FieldSource("EmailNotes", "left outer join BizPartnerNotes EmailNotes on Email.LoanID = EmailNotes.NoteID", new string[2]
          {
            "Contact",
            "Email"
          });
        case "fax":
          return new FieldSource("Fax", "left outer join BizPartnerHistory Fax on Contact.ContactID = Fax.ContactID and Fax.EventType = 'Faxed'", new string[1]
          {
            "Contact"
          });
        case "faxnotes":
          return new FieldSource("FaxNotes", "left outer join BizPartnerNotes FaxNotes on Fax.LoanID = FaxNotes.NoteID", new string[2]
          {
            "Contact",
            "Fax"
          });
        case "firstcontact":
          return new FieldSource("FirstContact", "left outer join FN_GetLastBizPartnerHistoryEvents('First Contact') FirstContact on Contact.ContactID = FirstContact.ContactID", new string[1]
          {
            "Contact"
          });
        case "history":
          return new FieldSource("History", "left outer join BizPartnerHistory History on History.ContactID = Contact.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastappointment":
          return new FieldSource("LastAppointment", "left outer join FN_GetContactsLastAppointments(1, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) DateTime.Now) + ") LastAppointment on LastAppointment.ContactID = Contact.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcall":
          return new FieldSource("LastCall", "left outer join FN_GetLastBizPartnerHistoryEvents('Called') LastCall on Contact.ContactID = LastCall.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcallnotes":
          return new FieldSource("LastCallNotes", "left outer join BizPartnerNotes LastCallNotes on LastCall.LoanID = LastCallNotes.NoteID", new string[2]
          {
            "Contact",
            "LastCall"
          });
        case "lastcampaigncall":
          return new FieldSource("LastCampaignCall", "left outer join FN_GetLastBizPartnerHistoryEvents('Campaign Phone Call') LastCampaignCall on Contact.ContactID = LastCampaignCall.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcampaignemail":
          return new FieldSource("LastCampaignEmail", "left outer join FN_GetLastBizPartnerHistoryEvents('Campaign Email') LastCampaignEmail on Contact.ContactID = LastCampaignEmail.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcampaignfax":
          return new FieldSource("LastCampaignFax", "left outer join FN_GetLastBizPartnerHistoryEvents('Campaign Fax') LastCampaignFax on Contact.ContactID = LastCampaignFax.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcampaignletter":
          return new FieldSource("LastCampaignLetter", "left outer join FN_GetLastBizPartnerHistoryEvents('Campaign Letter') LastCampaignLetter on Contact.ContactID = LastCampaignLetter.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcampaignreminder":
          return new FieldSource("LastCampaignReminder", "left outer join FN_GetLastBizPartnerHistoryEvents('Campaign Reminder') LastCampaignReminder on Contact.ContactID = LastCampaignReminder.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastemail":
          return new FieldSource("LastEmail", "left outer join FN_GetLastBizPartnerHistoryEvents('Emailed') LastEmail on Contact.ContactID = LastEmail.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastemailmerge":
          return new FieldSource("LastEMailMerge", "left outer join FN_GetLastBizPartnerHistoryEvents('Email Merge') LastEMailMerge on Contact.ContactID = LastEMailMerge.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastemailnotes":
          return new FieldSource("LastEmailNotes", "left outer join BizPartnerNotes LastEmailNotes on LastEmail.LoanID = LastEmailNotes.NoteID", new string[2]
          {
            "Contact",
            "LastEmail"
          });
        case "lastfax":
          return new FieldSource("LastFax", "left outer join FN_GetLastBizPartnerHistoryEvents('Faxed') LastFax on Contact.ContactID = LastFax.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastfaxnotes":
          return new FieldSource("LastFaxNotes", "left outer join BizPartnerNotes LastFaxNotes on LastFax.LoanID = LastFaxNotes.NoteID", new string[2]
          {
            "Contact",
            "LastFax"
          });
        case "lastmailmerge":
          return new FieldSource("LastMailMerge", "left outer join FN_GetLastBizPartnerHistoryEvents('Mail Merge') LastMailMerge on Contact.ContactID = LastMailMerge.ContactID", new string[1]
          {
            "Contact"
          });
        case "loan":
          return this.loanMatchType == RelatedLoanMatchType.AnyClosed ? new FieldSource("Loan", "left outer join LoanSummary Loan on (ContactLoans.LoanRefID = Loan.XRefID) and (Loan.DateCompleted is not NULL)", new string[2]
          {
            "Contact",
            "ContactLoans"
          }) : new FieldSource("Loan", "left outer join LoanSummary Loan on ContactLoans.LoanRefID = Loan.XRefID", new string[2]
          {
            "Contact",
            "ContactLoans"
          });
        case "mailmerge":
          return new FieldSource("MailMerge", "left outer join BizPartnerHistory MailMerge on Contact.ContactID = MailMerge.ContactID and MailMerge.EventType = 'Mail Merge'", new string[1]
          {
            "Contact"
          });
        case "nextappointment":
          return new FieldSource("NextAppointment", "left outer join FN_GetContactsNextAppointments(1, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) DateTime.Now) + ") NextAppointment on NextAppointment.ContactID = Contact.ContactID", new string[1]
          {
            "Contact"
          });
        case "opportunity":
          return new FieldSource("Opportunity", "left outer join Opportunity on Contact.ContactID = Opportunity.ContactID", new string[1]
          {
            "Contact"
          });
        case "owner":
          return new FieldSource("Owner", "inner join Users Owner on Contact.OwnerID = Owner.UserID", new string[1]
          {
            "Contact"
          });
        case "publiccontactgroup":
          string str2 = "left outer join AclGroupPublicBizGroupRef GroupAccess on (GroupAccess.bizGroupID = PublicContactGroup.GroupID) ";
          if (this.CurrentUser != (UserInfo) null)
            str2 = str2 + "left outer join FN_GetUsersAclGroups(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.RunAsUserId) + ") UserGroup on (UserGroup.GroupID = GroupAccess.aclGroupID)";
          return new FieldSource("PublicContactGroup", "left outer join ContactGroup PublicContactGroup on PublicContactGroup.GroupID = ContactGroupXref.GroupID  and PublicContactGroup.ContactType = 2 " + str2, new string[2]
          {
            "Contact",
            "ContactGroupXref"
          });
        case "publiccontactgroupmngr":
          return new FieldSource("PublicContactGroupMngr", "left outer join ContactGroup PublicContactGroupMngr on PublicContactGroupMngr.GroupID = ContactGroupXrefMngr.GroupID  and PublicContactGroupMngr.ContactType = 2 " + "left outer join AclGroupPublicBizGroupRef GroupAccessMngr on (GroupAccessMngr.bizGroupID = PublicContactGroupMngr.GroupID) ", new string[2]
          {
            "Contact",
            "ContactGroupXrefMngr"
          });
        case "publicgroupcount":
          return new FieldSource("PublicGroupCount", "left outer join FN_GetBizContactGroupCount('', 2) PublicGroupCount on Contact.ContactID = PublicGroupCount.ContactID", new string[1]
          {
            "Contact"
          });
        case "relatednote":
          return new FieldSource("RelatedNote", "left outer join BizPartnerNotes RelatedNote on RelatedNote.ContactID = Contact.ContactID", new string[1]
          {
            "Contact"
          });
        default:
          if (name.ToLower().StartsWith("custom_"))
          {
            string key = name.ToLower().Replace("custom_", "");
            FieldFormat fieldFormat = FieldFormat.STRING;
            if (this.cusFieldIdMap.ContainsKey(key))
              fieldFormat = this.cusFieldIdMap[key];
            switch (fieldFormat)
            {
              case FieldFormat.YN:
                return new FieldSource(name, "left outer join (select ContactID, (case when (FieldValue = 'Y') then 'Y' else 'N' end) as FieldValue from BizCustomField where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              case FieldFormat.X:
                return new FieldSource(name, "left outer join (select ContactID, (case when (FieldValue = 'X') then 'X' else '' end) as FieldValue from BizCustomField where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              case FieldFormat.DATE:
              case FieldFormat.DATETIME:
                return new FieldSource(name, "left outer join (select ContactID, (case when (ISDATE(FieldValue) <>0) then Convert(datetime, FieldValue) else Null end) as FieldValue from BizCustomField where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              case FieldFormat.MONTHDAY:
                return new FieldSource(name, "left outer join (select ContactID, (case when (ISDATE('2004/' + FieldValue) <>0) then Convert(datetime, '2004/' + FieldValue) else Null end) as FieldValue from BizCustomField where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              default:
                return new FieldSource(name, "left outer join (select ContactID, FieldValue from BizCustomField where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
            }
          }
          else if (name.ToLower().StartsWith("customcategory_"))
          {
            string key = name.ToLower().Replace("customcategory_", "");
            FieldFormat fieldFormat = FieldFormat.STRING;
            if (this.cusCategoryIdMap.ContainsKey(key))
              fieldFormat = this.cusCategoryIdMap[key];
            switch (fieldFormat)
            {
              case FieldFormat.YN:
                return new FieldSource(name, "left outer join (select ContactID, (case when (FieldValue = 'Y') then 'Y' else 'N' end) as FieldValue from BizCategoryCustomFieldValue where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              case FieldFormat.X:
                return new FieldSource(name, "left outer join (select ContactID, (case when (FieldValue = 'X') then 'X' else '' end) as FieldValue from BizCategoryCustomFieldValue where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              case FieldFormat.DATE:
              case FieldFormat.DATETIME:
                return new FieldSource(name, "left outer join (select ContactID, (case when (ISDATE(FieldValue) <>0) then Convert(datetime, FieldValue) else Null end) as FieldValue from BizCategoryCustomFieldValue where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              case FieldFormat.MONTHDAY:
                return new FieldSource(name, "left outer join (select ContactID, (case when (ISDATE('2004/' + FieldValue) <>0) then Convert(datetime, '2004/' + FieldValue) else Null end) as FieldValue from BizCategoryCustomFieldValue where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              default:
                return new FieldSource(name, "left outer join (select ContactID, FieldValue from BizCategoryCustomFieldValue where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
            }
          }
          else
          {
            if (!name.ToLower().StartsWith("standardcategory_"))
              return this.loanQuery.GetFieldSource(name);
            string key = name.ToLower().Replace("standardcategory_", "");
            FieldFormat fieldFormat = FieldFormat.STRING;
            if (this.stdCategoryIdMap.ContainsKey(key))
              fieldFormat = this.stdCategoryIdMap[key];
            switch (fieldFormat)
            {
              case FieldFormat.YN:
                return new FieldSource(name, "left outer join (select ContactID, (case when (FieldValue = 'Y') then 'Y' else 'N' end) as FieldValue from BizCategoryCustomFieldValue where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              case FieldFormat.X:
                return new FieldSource(name, "left outer join (select ContactID, (case when (FieldValue = 'X') then 'X' else '' end) as FieldValue from BizCategoryCustomFieldValue where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              case FieldFormat.DATE:
              case FieldFormat.DATETIME:
                return new FieldSource(name, "left outer join (select ContactID, (case when (ISDATE(FieldValue) <>0) then Convert(datetime, FieldValue) else Null end) as FieldValue from BizCategoryCustomFieldValue where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              case FieldFormat.MONTHDAY:
                return new FieldSource(name, "left outer join (select ContactID, (case when (ISDATE('2004/' + FieldValue) <>0) then Convert(datetime, '2004/' + FieldValue) else Null end) as FieldValue from BizCategoryCustomFieldValue where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
              default:
                return new FieldSource(name, "left outer join (select ContactID, FieldValue from BizCategoryCustomFieldValue where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
                {
                  "Contact"
                });
            }
          }
      }
    }

    public override string GetUserAccessFilterJoinClause(bool isExternalOrganization)
    {
      return this.CurrentUser == (UserInfo) null ? "" : "inner join FN_GetUsersAccessibleBizPartners(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.RunAsUserId) + ") _bpq_ids on _bpq_ids.ContactID = Contact.ContactID";
    }

    public override string GetUserAccessFilterJoinClause(
      bool isExternalOrganization,
      QueryCriterion filter)
    {
      return this.CurrentUser == (UserInfo) null ? "" : "inner join FN_GetUsersAccessibleBizPartners(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.RunAsUserId) + ") _bpq_ids on _bpq_ids.ContactID = Contact.ContactID";
    }

    public override List<string> ParentTables => throw new NotImplementedException();

    public override void SplitFiltersByReportsFor(FieldFilterList filterList)
    {
      throw new NotImplementedException();
    }

    public override void GetCategories(List<ColumnInfo> fields)
    {
      this.Categories.Add(ReportsFor.BusinessContact);
    }
  }
}
