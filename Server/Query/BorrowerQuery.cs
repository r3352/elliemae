// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.BorrowerQuery
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Contacts;
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
  internal class BorrowerQuery : QueryEngine
  {
    private LoanQuery loanQuery;
    private Dictionary<string, FieldFormat> fieldIdMap = new Dictionary<string, FieldFormat>();
    private RelatedLoanMatchType loanMatchType;

    public BorrowerQuery(UserInfo currentUser, RelatedLoanMatchType loanMatchType)
      : base(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType, currentUser, (ICriterionTranslator) new ContactFieldTranslator("BorCustomField", BorrowerCustomFields.Get()))
    {
      this.loanMatchType = loanMatchType;
      this.loanQuery = new LoanQuery(currentUser);
      ContactCustomFieldInfoCollection fieldInfoCollection = BorrowerCustomFields.Get();
      for (int index = 0; index < fieldInfoCollection.Items.Length; ++index)
      {
        if (!this.fieldIdMap.ContainsKey(string.Concat((object) fieldInfoCollection.Items[index].LabelID)))
          this.fieldIdMap.Add(string.Concat((object) fieldInfoCollection.Items[index].LabelID), fieldInfoCollection.Items[index].FieldType);
      }
    }

    public override string PrimaryKeyTableIdentifier => "Borrower Contact";

    public override string PrimaryKeyIdentifier => "Contact.ContactID";

    public override string UserAccessQueryKeyColumnName => "ContactID";

    public override FieldSource GetFieldSource(string name)
    {
      switch (name.ToLower())
      {
        case "call":
          return new FieldSource("Call", "left outer join BorrowerHistory Call on Contact.ContactID = Call.ContactID and Call.EventType = 'Called'", new string[1]
          {
            "Contact"
          });
        case "callnotes":
          return new FieldSource("CallNotes", "left outer join BorrowerNotes CallNotes on Call.LoanID = CallNotes.NoteID", new string[2]
          {
            "Contact",
            "Call"
          });
        case "campaigncall":
          return new FieldSource("CampaignCall", "left outer join BorrowerHistory CampaignCall on Contact.ContactID = CampaignCall.ContactID and CampaignCall.EventType = 'Campaign Phone Call'", new string[1]
          {
            "Contact"
          });
        case "campaignemail":
          return new FieldSource("CampaignEmail", "left outer join BorrowerHistory CampaignEmail on Contact.ContactID = CampaignEmail.ContactID and CampaignEmail.EventType = 'Campaign Email'", new string[1]
          {
            "Contact"
          });
        case "campaignfax":
          return new FieldSource("CampaignFax", "left outer join BorrowerHistory CampaignFax on Contact.ContactID = CampaignFax.ContactID and CampaignFax.EventType = 'Campaign Fax'", new string[1]
          {
            "Contact"
          });
        case "campaignletter":
          return new FieldSource("CampaignLetter", "left outer join BorrowerHistory CampaignLetter on Contact.ContactID = CampaignLetter.ContactID and CampaignLetter.EventType = 'Campaign Letter'", new string[1]
          {
            "Contact"
          });
        case "campaignreminder":
          return new FieldSource("CampaignReminder", "left outer join BorrowerHistory CampaignReminder on Contact.ContactID = CampaignReminder.ContactID and CampaignReminder.EventType = 'Campaign Reminder'", new string[1]
          {
            "Contact"
          });
        case "contactcompleteloancount":
          return new FieldSource("ContactCompleteLoanCount", "left outer join FN_GetBorrowerLoansCount('Y') ContactCompleteLoanCount on Contact.ContactID = ContactCompleteLoanCount.ContactID", new string[1]
          {
            "Contact"
          });
        case "contactgroup":
          return new FieldSource("ContactGroup", "left outer join ContactGroup on ContactGroup.GroupID = ContactGroupXref.GroupID", new string[2]
          {
            "Contact",
            "ContactGroupXref"
          });
        case "contactgroupcount":
          return new FieldSource("ContactGroupCount", "left outer join FN_GetBorrowerContactGroupCount(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.Userid) + ") ContactGroupCount on Contact.ContactID = ContactGroupCount.ContactID", new string[1]
          {
            "Contact"
          });
        case "contactgroupxref":
          return new FieldSource("ContactGroupXref", "left outer join ContactGroupBorrowers ContactGroupXref on ContactGroupXref.ContactID = Contact.ContactID", new string[1]
          {
            "Contact"
          });
        case "contactloancount":
          return new FieldSource("ContactLoanCount", "left outer join FN_GetBorrowerLoansCount('') ContactLoanCount on Contact.ContactID = ContactLoanCount.ContactID", new string[1]
          {
            "Contact"
          });
        case "contactloans":
          return this.loanMatchType == RelatedLoanMatchType.LastOriginated ? new FieldSource("ContactLoans", "left outer join BorrowerLastStartedLoans ContactLoans on Contact.Guid = ContactLoans.ContactGuid", new string[1]
          {
            "Contact"
          }) : (this.loanMatchType == RelatedLoanMatchType.LastClosed ? new FieldSource("ContactLoans", "left outer join BorrowerLastCompletedLoans ContactLoans on Contact.Guid = ContactLoans.ContactGuid", new string[1]
          {
            "Contact"
          }) : new FieldSource("ContactLoans", "left outer join BorrowerLoans ContactLoans on Contact.Guid = ContactLoans.ContactGuid", new string[1]
          {
            "Contact"
          }));
        case "contactowner":
          return new FieldSource("ContactOwner", "left outer join Users ContactOwner on Contact.OwnerID = ContactOwner.UserID", new string[1]
          {
            "Contact"
          });
        case "email":
          return new FieldSource("Email", "left outer join BorrowerHistory Email on Contact.ContactID = Email.ContactID and Email.EventType = 'Emailed'", new string[1]
          {
            "Contact"
          });
        case "emailmerge":
          return new FieldSource("EmailMerge", "left outer join BorrowerHistory EmailMerge on Contact.ContactID = EmailMerge.ContactID and EmailMerge.EventType = 'Email Merge'", new string[1]
          {
            "Contact"
          });
        case "emailnotes":
          return new FieldSource("EmailNotes", "left outer join BorrowerNotes EmailNotes on Email.LoanID = EmailNotes.NoteID", new string[2]
          {
            "Contact",
            "Email"
          });
        case "fax":
          return new FieldSource("Fax", "left outer join BorrowerHistory Fax on Contact.ContactID = Fax.ContactID and Fax.EventType = 'Faxed'", new string[1]
          {
            "Contact"
          });
        case "faxnotes":
          return new FieldSource("FaxNotes", "left outer join BorrowerNotes FaxNotes on Fax.LoanID = FaxNotes.NoteID", new string[2]
          {
            "Contact",
            "Fax"
          });
        case "firstcontact":
          return new FieldSource("FirstContact", "left outer join FN_GetLastBorrowerHistoryEvents('First Contact') FirstContact on Contact.ContactID = FirstContact.ContactID", new string[1]
          {
            "Contact"
          });
        case "history":
          return new FieldSource("History", "left outer join BorrowerHistory History on History.ContactID = Contact.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastappointment":
          return new FieldSource("LastAppointment", "left outer join FN_GetContactsLastAppointments(0, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) DateTime.Now) + ") LastAppointment on LastAppointment.ContactID = Contact.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcall":
          return new FieldSource("LastCall", "left outer join FN_GetLastBorrowerHistoryEvents('Called') LastCall on Contact.ContactID = LastCall.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcallnotes":
          return new FieldSource("LastCallNotes", "left outer join BorrowerNotes LastCallNotes on LastCall.LoanID = LastCallNotes.NoteID", new string[2]
          {
            "Contact",
            "LastCall"
          });
        case "lastcampaigncall":
          return new FieldSource("LastCampaignCall", "left outer join FN_GetLastBorrowerHistoryEvents('Campaign Phone Call') LastCampaignCall on Contact.ContactID = LastCampaignCall.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcampaignemail":
          return new FieldSource("LastCampaignEmail", "left outer join FN_GetLastBorrowerHistoryEvents('Campaign Email') LastCampaignEmail on Contact.ContactID = LastCampaignEmail.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcampaignfax":
          return new FieldSource("LastCampaignFax", "left outer join FN_GetLastBorrowerHistoryEvents('Campaign Fax') LastCampaignFax on Contact.ContactID = LastCampaignFax.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcampaignletter":
          return new FieldSource("LastCampaignLetter", "left outer join FN_GetLastBorrowerHistoryEvents('Campaign Letter') LastCampaignLetter on Contact.ContactID = LastCampaignLetter.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastcampaignreminder":
          return new FieldSource("LastCampaignReminder", "left outer join FN_GetLastBorrowerHistoryEvents('Campaign Reminder') LastCampaignReminder on Contact.ContactID = LastCampaignReminder.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastemail":
          return new FieldSource("LastEmail", "left outer join FN_GetLastBorrowerHistoryEvents('Emailed') LastEmail on Contact.ContactID = LastEmail.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastemailmerge":
          return new FieldSource("LastEMailMerge", "left outer join FN_GetLastBorrowerHistoryEvents('Email Merge') LastEMailMerge on Contact.ContactID = LastEMailMerge.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastemailnotes":
          return new FieldSource("LastEmailNotes", "left outer join BorrowerNotes LastEmailNotes on LastEmail.LoanID = LastEmailNotes.NoteID", new string[2]
          {
            "Contact",
            "LastEmail"
          });
        case "lastfax":
          return new FieldSource("LastFax", "left outer join FN_GetLastBorrowerHistoryEvents('Faxed') LastFax on Contact.ContactID = LastFax.ContactID", new string[1]
          {
            "Contact"
          });
        case "lastfaxnotes":
          return new FieldSource("LastFaxNotes", "left outer join BorrowerNotes LastFaxNotes on LastFax.LoanID = LastFaxNotes.NoteID", new string[2]
          {
            "Contact",
            "LastFax"
          });
        case "lastmailmerge":
          return new FieldSource("LastMailMerge", "left outer join FN_GetLastBorrowerHistoryEvents('Mail Merge') LastMailMerge on Contact.ContactID = LastMailMerge.ContactID", new string[1]
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
          return new FieldSource("MailMerge", "left outer join BorrowerHistory MailMerge on Contact.ContactID = MailMerge.ContactID and MailMerge.EventType = 'Mail Merge'", new string[1]
          {
            "Contact"
          });
        case "nextappointment":
          return new FieldSource("NextAppointment", "left outer join FN_GetContactsNextAppointments(0, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) DateTime.Now) + ") NextAppointment on NextAppointment.ContactID = Contact.ContactID", new string[1]
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
        case "relatednote":
          return new FieldSource("RelatedNote", "left outer join BorrowerNotes RelatedNote on RelatedNote.ContactID = Contact.ContactID", new string[1]
          {
            "Contact"
          });
        default:
          if (!name.ToLower().StartsWith("custom_"))
            return this.loanQuery.GetFieldSource(name);
          string key = name.ToLower().Replace("custom_", "");
          FieldFormat fieldFormat = FieldFormat.STRING;
          if (this.fieldIdMap.ContainsKey(key))
            fieldFormat = this.fieldIdMap[key];
          switch (fieldFormat)
          {
            case FieldFormat.YN:
              return new FieldSource(name, "left outer join (select ContactID, (case when (FieldValue = 'Y') then 'Y' else 'N' end) as FieldValue from BorCustomField where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
              {
                "Contact"
              });
            case FieldFormat.X:
              return new FieldSource(name, "left outer join (select ContactID, (case when (FieldValue = 'X') then 'X' else '' end) as FieldValue from BorCustomField where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
              {
                "Contact"
              });
            case FieldFormat.INTEGER:
              return new FieldSource(name, "left outer join (select ContactID, (case when (ISNUMERIC(FieldValue) <>0) then Convert(int, replace(FieldValue, ',', '')) else Null end) as FieldValue from BorCustomField where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
              {
                "Contact"
              });
            case FieldFormat.DECIMAL_1:
            case FieldFormat.DECIMAL_2:
            case FieldFormat.DECIMAL_3:
            case FieldFormat.DECIMAL_4:
            case FieldFormat.DECIMAL_6:
            case FieldFormat.DECIMAL_5:
            case FieldFormat.DECIMAL_7:
            case FieldFormat.DECIMAL:
            case FieldFormat.DECIMAL_10:
              return new FieldSource(name, "left outer join (select ContactID, (case when (ISNUMERIC(FieldValue) <>0) then Convert(decimal(18,5), replace(FieldValue, ',', '')) else Null end) as FieldValue from BorCustomField where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
              {
                "Contact"
              });
            case FieldFormat.DATE:
            case FieldFormat.DATETIME:
              return new FieldSource(name, "left outer join (select ContactID, (case when (ISDATE(FieldValue) <>0) then Convert(datetime, FieldValue) else Null end) as FieldValue from BorCustomField where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
              {
                "Contact"
              });
            case FieldFormat.MONTHDAY:
              return new FieldSource(name, "left outer join (select ContactID, (case when (ISDATE('2004/' + FieldValue) <>0) then Convert(datetime, '2004/' + FieldValue) else Null end) as FieldValue from BorCustomField where FieldID=" + key + " ) as " + name + " on " + name + ".ContactID = Contact.ContactID", new string[1]
              {
                "Contact"
              });
            default:
              return new FieldSource(name, "left outer join BorCustomField as " + name + " on " + name + ".FieldID=" + key + " and " + name + ".ContactID = Contact.ContactID", new string[1]
              {
                "Contact"
              });
          }
      }
    }

    public override string GetUserAccessFilterJoinClause(bool isExternalOrganization)
    {
      return this.CurrentUser == (UserInfo) null ? "" : "inner join FN_GetUsersAccessibleBorrowers(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.Userid) + ") _bq_ids on _bq_ids.ContactID = Contact.ContactID";
    }

    public override string GetUserAccessFilterJoinClause(
      bool isExternalOrganization,
      QueryCriterion filter)
    {
      return this.CurrentUser == (UserInfo) null ? "" : "inner join FN_GetUsersAccessibleBorrowers(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.Userid) + ") _bq_ids on _bq_ids.ContactID = Contact.ContactID";
    }

    public override List<string> ParentTables => throw new NotImplementedException();

    public override void SplitFiltersByReportsFor(FieldFilterList filterList)
    {
      throw new NotImplementedException();
    }

    public override void GetCategories(List<ColumnInfo> fields)
    {
      this.Categories.Add(ReportsFor.BorrowerContact);
    }
  }
}
