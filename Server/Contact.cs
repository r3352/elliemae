// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Contact
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public abstract class Contact : IContact, IDisposable
  {
    private const string className = "Contact�";
    protected ContactStore.ContactIdentity id;
    private ICacheLock<bool?> innerLock;
    private bool exists;

    public Contact(ICacheLock<bool?> innerLock)
    {
      this.innerLock = innerLock;
      this.id = (ContactStore.ContactIdentity) innerLock.Identifier;
      if (innerLock == null)
        this.initializeContact();
      if (this.existsInDatabase())
        this.exists = true;
      else
        this.exists = false;
    }

    public Contact(ContactStore.ContactIdentity id)
    {
      this.id = id;
      if (this.existsInDatabase())
        this.exists = true;
      else
        this.exists = false;
    }

    public bool Exists => this.exists;

    public int ContactID => this.id.ContactID;

    public ContactType ContactType => this.id.ContactType;

    public abstract string FullName { get; }

    public void Delete()
    {
      this.validateInstance();
      this.deleteFromDatabase();
      this.innerLock.UndoCheckout();
      this.Dispose();
    }

    public void UndoCheckout()
    {
      if (this.innerLock == null)
        return;
      this.innerLock.UndoCheckout();
      this.Dispose();
    }

    public void Dispose()
    {
      if (this.innerLock == null)
        return;
      this.innerLock.Dispose();
      this.innerLock = (ICacheLock<bool?>) null;
    }

    private void initializeContact() => this.innerLock.UndoCheckout();

    protected void validateInstance() => this.validateInstance(true);

    protected void validateInstance(bool requireExists)
    {
      if (this.innerLock == null)
        Err.Raise(TraceLevel.Error, nameof (Contact), new ServerException("Attempt to access disposed Contact object"));
      if (!requireExists)
        return;
      this.validateExists();
    }

    protected void validateExists()
    {
      if (this.Exists)
        return;
      Err.Raise(TraceLevel.Error, nameof (Contact), (ServerException) new ObjectNotFoundException("Object does not exist", ObjectType.Contact, (object) this.id.ToString()));
    }

    protected abstract string ContactTable { get; }

    protected abstract string HistoryTable { get; }

    protected abstract string NotesTable { get; }

    protected abstract string CustomFieldTable { get; }

    protected abstract string CampaignActivityTable { get; }

    protected string CreditScoreTable => "ContactCreditScores";

    private bool existsInDatabase()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable(this.ContactTable), new string[1]
      {
        "ContactID"
      }, new DbValue("ContactID", (object) this.id.ContactID));
      return dbQueryBuilder.ExecuteScalar() != null;
    }

    private void deleteFromDatabase()
    {
      DbValue key = new DbValue("ContactID", (object) this.id.ContactID);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable(this.CampaignActivityTable), key);
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable(this.CustomFieldTable), key);
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable(this.NotesTable), key);
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable(this.HistoryTable), key);
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable(this.ContactTable), key);
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("AppointmentsXref"), new DbValueList()
      {
        key,
        {
          "ContactType",
          (object) (int) this.ContactType
        }
      });
      dbQueryBuilder.ExecuteNonQuery();
    }

    public ContactHistoryItem[] GetHistory() => this.GetHistory((string) null);

    public ContactHistoryItem[] GetHistory(DateTime startDate, DateTime endDate)
    {
      return this.GetHistory((string) null, startDate, endDate);
    }

    public ContactHistoryItem[] GetHistory(string eventType)
    {
      return this.GetHistory(eventType, DateTime.MinValue, DateTime.MaxValue);
    }

    public ContactHistoryItem[] GetHistory(string eventType, DateTime startDate, DateTime endDate)
    {
      this.validateExists();
      return this.getHistoryItemsFromDatabase(eventType, startDate, endDate);
    }

    public ContactHistoryItem GetHistoryItem(int itemId)
    {
      this.validateExists();
      return this.getHistoryItemFromDatabase(itemId);
    }

    public int AddHistoryItem(ContactHistoryItem item)
    {
      this.validateInstance();
      return this.addHistoryItemToDatabase(item);
    }

    public void DeleteHistoryItem(int historyItemId)
    {
      this.validateInstance();
      this.deleteHistoryItemFromDatabase(historyItemId);
    }

    public static ContactHistoryNoteInfo GetContactHistoryNote(int noteId)
    {
      if (0 >= noteId)
        return (ContactHistoryNoteInfo) null;
      ContactHistoryNoteInfo[] contactHistoryNotes = EllieMae.EMLite.Server.Contact.GetContactHistoryNotes(new int[1]
      {
        noteId
      });
      return contactHistoryNotes.Length == 0 ? (ContactHistoryNoteInfo) null : contactHistoryNotes[0];
    }

    public static ContactHistoryNoteInfo[] GetContactHistoryNotes(int[] noteIds)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT h.* FROM ContactHistoryNote h WHERE NoteId IN " + EllieMae.EMLite.Server.Contact.encodeIntArray((Array) noteIds));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int count = dataRowCollection.Count;
      if (count == 0)
        return (ContactHistoryNoteInfo[]) null;
      ContactHistoryNoteInfo[] contactHistoryNotes = new ContactHistoryNoteInfo[count];
      for (int index = 0; index < count; ++index)
      {
        int noteId = (int) SQL.Decode(dataRowCollection[index]["NoteId"], (object) 0);
        string note = (string) SQL.Decode(dataRowCollection[index]["Note"], (object) string.Empty);
        contactHistoryNotes[index] = new ContactHistoryNoteInfo(noteId, note);
      }
      return contactHistoryNotes;
    }

    private static string encodeIntArray(Array intArray)
    {
      StringBuilder stringBuilder = new StringBuilder("(");
      foreach (int num in intArray)
        stringBuilder.Append(num.ToString() + ",");
      stringBuilder.Replace(",", ")", stringBuilder.Length - 1, 1);
      return stringBuilder.ToString();
    }

    public static ContactCustomFieldInfoCollection GetCustomFieldInfo(ContactType contactType)
    {
      ContactCustomFieldInfoCollection customFieldInfo;
      switch (contactType)
      {
        case ContactType.Borrower:
          customFieldInfo = BorrowerCustomFields.Get();
          break;
        case ContactType.BizPartner:
          customFieldInfo = BizPartnerCustomFields.Get();
          break;
        case ContactType.PublicBiz:
          customFieldInfo = BizPartnerCustomFields.Get();
          break;
        default:
          customFieldInfo = (ContactCustomFieldInfoCollection) null;
          break;
      }
      return customFieldInfo;
    }

    public static void UpdateCustomFieldInfo(
      ContactType contactType,
      ContactCustomFieldInfoCollection customFields)
    {
      if (contactType == ContactType.Borrower)
      {
        BorrowerCustomFields.Set(customFields);
      }
      else
      {
        if (contactType != ContactType.BizPartner)
          return;
        BizPartnerCustomFields.Set(customFields);
      }
    }

    public static CustomFieldsMappingInfo GetCustomFieldsMapping(
      CustomFieldsType customFieldsType,
      int categoryId,
      bool twoWayTransfersOnly)
    {
      List<CustomFieldMappingInfo> fieldMappingInfoList = new List<CustomFieldMappingInfo>();
      if (CustomFieldsType.Borrower == (CustomFieldsType.Borrower & customFieldsType))
      {
        ContactCustomFieldInfoCollection customFieldInfo = EllieMae.EMLite.Server.Contact.GetCustomFieldInfo(ContactType.Borrower);
        if (customFieldInfo.Items != null)
        {
          foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
          {
            if (string.Empty != contactCustomFieldInfo.LoanFieldId && (!twoWayTransfersOnly || contactCustomFieldInfo.TwoWayTransfer))
              fieldMappingInfoList.Add(new CustomFieldMappingInfo(CustomFieldsType.Borrower, 0, contactCustomFieldInfo.LabelID, 0, contactCustomFieldInfo.FieldType, contactCustomFieldInfo.LoanFieldId, contactCustomFieldInfo.TwoWayTransfer));
          }
        }
      }
      if (CustomFieldsType.BizPartner == (CustomFieldsType.BizPartner & customFieldsType))
      {
        ContactCustomFieldInfoCollection customFieldInfo = EllieMae.EMLite.Server.Contact.GetCustomFieldInfo(ContactType.BizPartner);
        if (customFieldInfo.Items != null)
        {
          foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
          {
            if (string.Empty != contactCustomFieldInfo.LoanFieldId && (!twoWayTransfersOnly || contactCustomFieldInfo.TwoWayTransfer))
              fieldMappingInfoList.Add(new CustomFieldMappingInfo(CustomFieldsType.BizPartner, 0, contactCustomFieldInfo.LabelID, 0, contactCustomFieldInfo.FieldType, contactCustomFieldInfo.LoanFieldId, contactCustomFieldInfo.TwoWayTransfer));
          }
        }
      }
      if (CustomFieldsType.BizCategoryCustom == (CustomFieldsType.BizCategoryCustom & customFieldsType))
      {
        CustomFieldsMappingInfo customFieldsMapping = BizPartnerContact.GetCategoryCustomFieldsMapping(CustomFieldsType.BizCategoryCustom, categoryId, twoWayTransfersOnly);
        fieldMappingInfoList.AddRange((IEnumerable<CustomFieldMappingInfo>) customFieldsMapping.CustomFieldMappings);
      }
      return new CustomFieldsMappingInfo(customFieldsType, fieldMappingInfoList.ToArray());
    }

    public ContactCustomField[] GetCustomFields()
    {
      this.validateExists();
      return this.getCustomFieldsFromDatabase();
    }

    public static void DeleteContactCustomFieldValues(ContactType contactType, int[] fieldIds)
    {
      if (fieldIds == null || fieldIds.Length == 0)
        return;
      string str = contactType == ContactType.Borrower ? "BorCustomField" : "BizCustomField";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE FROM " + str + " WHERE FieldId IN " + EllieMae.EMLite.Server.Contact.encodeIntArray((Array) fieldIds));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public void UpdateCustomFields(ContactCustomField[] fields)
    {
      this.validateInstance();
      this.updateCustomFieldsInDatabase(fields);
    }

    private void updateCustomFieldsInDatabase(ContactCustomField[] fields)
    {
      if (fields.Length == 0)
        return;
      DbTableInfo table1 = DbAccessManager.GetTable(this.CustomFieldTable);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      for (int index = 0; index < fields.Length; ++index)
      {
        DbValueList dbValueList = new DbValueList();
        dbValueList.Add("ContactID", (object) this.id.ContactID);
        dbValueList.Add("FieldID", (object) fields[index].FieldID);
        dbQueryBuilder.DeleteFrom(table1, dbValueList);
        dbValueList.Add("FieldValue", (object) fields[index].FieldValue);
        dbValueList.Add("OwnerID", (object) fields[index].OwnerID);
        dbQueryBuilder.InsertInto(table1, dbValueList, true, false);
      }
      DbTableInfo table2 = DbAccessManager.GetTable(this.ContactTable);
      DbValueList values = new DbValueList();
      values.Add("LastModified", (object) DateTime.Now);
      DbValue key = new DbValue("ContactID", (object) this.id.ContactID);
      dbQueryBuilder.Update(table2, values, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private ContactCustomField[] getCustomFieldsFromDatabase()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable(this.CustomFieldTable), new DbValue("ContactID", (object) this.id.ContactID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ContactCustomField[] fieldsFromDatabase = new ContactCustomField[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        fieldsFromDatabase[index] = this.dataRowToCustomField(dataRowCollection[index]);
      return fieldsFromDatabase;
    }

    private ContactCustomField dataRowToCustomField(DataRow row)
    {
      return new ContactCustomField((int) row["ContactID"], (int) row["FieldID"], SQL.Decode(row["OwnerID"], (object) "").ToString(), SQL.Decode(row["FieldValue"], (object) "").ToString());
    }

    public ContactNote[] GetNotes() => this.GetNotes(DateTime.MinValue, DateTime.MaxValue);

    public ContactNote[] GetNotes(DateTime startDate, DateTime endDate)
    {
      this.validateExists();
      return this.getNotesFromDatabase(startDate, endDate);
    }

    public ContactNote GetNote(int noteId) => this.getNoteFromDatabase(noteId);

    public static ContactNote GetContactNote(int noteId, ContactType contactType)
    {
      string tableName = contactType == ContactType.Borrower ? "BorrowerNotes" : "BizPartnerNotes";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable(tableName), new DbValue("NoteID", (object) noteId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (ContactNote) null : EllieMae.EMLite.Server.Contact.dataRowToNote(dataRowCollection[0]);
    }

    public int AddNote(ContactNote note)
    {
      this.validateExists();
      return this.addNoteToDatabase(note);
    }

    public void UpdateNote(ContactNote note)
    {
      this.validateExists();
      this.updateNoteInDatabase(note);
    }

    public void DeleteNote(int noteId)
    {
      this.validateExists();
      this.deleteNoteFromDatabase(noteId);
    }

    public void AddCreditScoresForHistoryItem(
      int historyId,
      ContactCreditScores[] contactScoresList)
    {
      for (int index1 = 0; index1 < contactScoresList.Length; ++index1)
      {
        ContactCreditScores contactScores = contactScoresList[index1];
        DbValueList values = new DbValueList();
        values.Add("HistoryID", (object) historyId);
        values.Add("FirstName", (object) contactScores.FirstName);
        values.Add("LastName", (object) contactScores.LastName);
        values.Add("SSN", (object) contactScores.SSN);
        for (int index2 = 0; index2 < contactScores.Scores.Length && index2 < 5; ++index2)
        {
          string columnName1 = "Source" + (object) (index2 + 1);
          string columnName2 = "Score" + (object) (index2 + 1);
          values.Add(columnName1, (object) contactScores.Scores[index2].Source);
          values.Add(columnName2, (object) contactScores.Scores[index2].Score);
        }
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable(this.CreditScoreTable), values, true, false);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public ContactCreditScores[] GetCreditScoresForHistoryItem(int historyId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from " + this.CreditScoreTable);
      dbQueryBuilder.AppendLine("where (HistoryID = " + (object) historyId + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ContactCreditScores[] scoresForHistoryItem = new ContactCreditScores[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        scoresForHistoryItem[index] = this.dataRowToCreditScoresItem(dataRowCollection[index]);
      return scoresForHistoryItem;
    }

    private ContactCreditScores dataRowToCreditScoresItem(DataRow row)
    {
      CreditScore[] scores = new CreditScore[5]
      {
        new CreditScore(SQL.Decode(row["Source1"], (object) "").ToString().Trim(), SQL.Decode(row["Score1"], (object) "").ToString().Trim()),
        new CreditScore(SQL.Decode(row["Source2"], (object) "").ToString().Trim(), SQL.Decode(row["Score2"], (object) "").ToString().Trim()),
        new CreditScore(SQL.Decode(row["Source3"], (object) "").ToString().Trim(), SQL.Decode(row["Score3"], (object) "").ToString().Trim()),
        new CreditScore(SQL.Decode(row["Source4"], (object) "").ToString().Trim(), SQL.Decode(row["Score4"], (object) "").ToString().Trim()),
        new CreditScore(SQL.Decode(row["Source5"], (object) "").ToString().Trim(), SQL.Decode(row["Score5"], (object) "").ToString().Trim())
      };
      return new ContactCreditScores("-1", SQL.Decode(row["FirstName"], (object) "").ToString().Trim(), SQL.Decode(row["LastName"], (object) "").ToString().Trim(), SQL.Decode(row["SSN"], (object) "").ToString().Trim(), scores);
    }

    private ContactHistoryItem[] getHistoryItemsFromDatabase(
      string eventType,
      DateTime startDate,
      DateTime endDate)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from " + this.HistoryTable);
      dbQueryBuilder.AppendLine("where (ContactID = " + (object) this.id.ContactID + ")");
      if (eventType != null)
        dbQueryBuilder.AppendLine("      and (EventType = " + SQL.Encode((object) eventType) + ")");
      if (startDate != DateTime.MinValue)
        dbQueryBuilder.AppendLine("      and (TimeOfHistory >= " + SQL.Encode((object) startDate) + ")");
      if (endDate != DateTime.MaxValue)
        dbQueryBuilder.AppendLine("      and (TimeOfHistory <= " + SQL.Encode((object) endDate) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ContactHistoryItem[] itemsFromDatabase = new ContactHistoryItem[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        itemsFromDatabase[index] = this.dataRowToHistoryItem(dataRowCollection[index]);
      return itemsFromDatabase;
    }

    private ContactHistoryItem getHistoryItemFromDatabase(int itemId)
    {
      DbValueList keys = new DbValueList();
      keys.Add("ContactID", (object) this.id.ContactID);
      keys.Add("HistoryID", (object) itemId);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable(this.HistoryTable), keys);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (ContactHistoryItem) null : this.dataRowToHistoryItem(dataRowCollection[0]);
    }

    private int addHistoryItemToDatabase(ContactHistoryItem item)
    {
      DbValueList values = new DbValueList();
      values.Add("ContactID", (object) this.id.ContactID);
      values.Add("TimeOfHistory", (object) item.Timestamp);
      values.Add("EventType", (object) item.EventType);
      values.Add("LoanID", (object) item.LoanID);
      values.Add("LetterName", (object) item.LetterName);
      values.Add("Sender", (object) item.Sender);
      values.Add("Subject", (object) item.Subject);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.InsertInto(DbAccessManager.GetTable(this.HistoryTable), values, true, false);
      dbQueryBuilder.SelectIdentity();
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    private void deleteHistoryItemFromDatabase(int itemId)
    {
      DbValueList keys = new DbValueList();
      keys.Add("ContactID", (object) this.id.ContactID);
      keys.Add("HistoryID", (object) itemId);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable(this.HistoryTable), keys);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private ContactHistoryItem dataRowToHistoryItem(DataRow row)
    {
      return new ContactHistoryItem((int) row["HistoryID"], SQL.Decode(row["EventType"], (object) "").ToString().Trim(), (DateTime) SQL.Decode(row["TimeOfHistory"], (object) DateTime.MinValue), (int) SQL.Decode(row["LoanID"], (object) -1), SQL.Decode(row["LetterName"], (object) "").ToString().Trim(), SQL.Decode(row["Sender"], (object) "").ToString().Trim(), SQL.Decode(row["Subject"], (object) "").ToString().Trim(), ContactSourceEnumUtil.ValueStringToValue(SQL.Decode(row["ContactSource"], (object) "").ToString().Trim()), SQL.Decode(row["CampaignName"], (object) "").ToString().Trim(), SQL.Decode(row["CampaignStepName"], (object) "").ToString().Trim(), SQL.Decode(row["CampaignActivityStatus"], (object) "").ToString().Trim(), (DateTime) SQL.Decode(row["CampaignScheduledDate"], (object) DateTime.MinValue), (int) SQL.Decode(row["CampaignStepNumber"], (object) 0));
    }

    private ContactNote[] getNotesFromDatabase(DateTime startDate, DateTime endDate)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from " + this.NotesTable);
      dbQueryBuilder.AppendLine("where (ContactID = " + (object) this.id.ContactID + ")");
      if (startDate != DateTime.MinValue)
        dbQueryBuilder.AppendLine("      and (TimeOfNote >= " + SQL.EncodeDateTime(startDate, true) + ")");
      if (endDate != DateTime.MaxValue)
        dbQueryBuilder.AppendLine("      and (TimeOfNote <= " + SQL.EncodeDateTime(endDate, true) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ContactNote[] notesFromDatabase = new ContactNote[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        notesFromDatabase[index] = EllieMae.EMLite.Server.Contact.dataRowToNote(dataRowCollection[index]);
      return notesFromDatabase;
    }

    private ContactNote getNoteFromDatabase(int noteId)
    {
      DbValueList keys = new DbValueList();
      keys.Add("ContactID", (object) this.id.ContactID);
      keys.Add("NoteID", (object) noteId);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable(this.NotesTable), keys);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (ContactNote) null : EllieMae.EMLite.Server.Contact.dataRowToNote(dataRowCollection[0]);
    }

    private int addNoteToDatabase(ContactNote note)
    {
      DbValueList values = new DbValueList();
      values.Add("ContactID", (object) this.id.ContactID);
      values.Add("TimeOfNote", (object) note.Timestamp, (IDbEncoder) DbEncoding.ShortDateTime);
      values.Add("Subject", (object) note.Subject);
      values.Add("Details", (object) note.Details);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.InsertInto(DbAccessManager.GetTable(this.NotesTable), values, true, false);
      dbQueryBuilder.SelectIdentity();
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    private void updateNoteInDatabase(ContactNote note)
    {
      DbValueList values = new DbValueList();
      values.Add("TimeOfNote", (object) note.Timestamp, (IDbEncoder) DbEncoding.ShortDateTime);
      values.Add("Subject", (object) note.Subject);
      values.Add("Details", (object) note.Details);
      DbValueList keys = new DbValueList();
      keys.Add("ContactID", (object) this.id.ContactID);
      keys.Add("NoteID", (object) note.NoteID);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable(this.NotesTable);
      dbQueryBuilder.Update(table, values, keys);
      dbQueryBuilder.SelectFrom(table, new string[1]
      {
        "NoteID"
      }, keys);
      if (dbQueryBuilder.ExecuteScalar() == null)
        throw new ObjectNotFoundException("Specified note does not exist for the current contact", ObjectType.ContactNote, (object) note.NoteID);
    }

    private void deleteNoteFromDatabase(int noteId)
    {
      DbValueList keys = new DbValueList();
      keys.Add("ContactID", (object) this.id.ContactID);
      keys.Add("NoteID", (object) noteId);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable(this.NotesTable), keys);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static ContactNote dataRowToNote(DataRow row)
    {
      return new ContactNote((int) row["NoteID"], SQL.Decode(row["Subject"], (object) "").ToString(), (DateTime) SQL.Decode(row["TimeOfNote"], (object) DateTime.MinValue), SQL.Decode(row["Details"], (object) "").ToString());
    }

    public static ContactLoanPair[] GetRelatedLoansForContact(
      int contactId,
      ContactType contactType)
    {
      return contactType == ContactType.BizPartner ? BizPartnerContact.GetRelatedLoansForBizPartner(contactId) : BorrowerContact.GetRelatedLoansForBorrower(contactId);
    }

    public static ContactLoanPair[] GetRelatedLoansForContact(string contactGuid)
    {
      ContactLoanPair[] loansForBorrower = BorrowerContact.GetRelatedLoansForBorrower(contactGuid);
      return loansForBorrower.Length != 0 ? loansForBorrower : BizPartnerContact.GetRelatedLoansForBizPartner(contactGuid);
    }

    public static ContactLoanPair[] GetRelatedContactsForLoan(string loanGuid)
    {
      List<ContactLoanPair> contactLoanPairList = new List<ContactLoanPair>();
      contactLoanPairList.AddRange((IEnumerable<ContactLoanPair>) BorrowerContact.GetBorrowersForLoan(loanGuid));
      contactLoanPairList.AddRange((IEnumerable<ContactLoanPair>) BizPartnerContact.GetBizPartnersForLoan(loanGuid));
      return contactLoanPairList.ToArray();
    }
  }
}
