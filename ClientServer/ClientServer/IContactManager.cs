// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IContactManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.TaskList;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IContactManager
  {
    int CreateBizPartner(BizPartnerInfo info);

    int CreateBizPartner(
      BizPartnerInfo info,
      DateTime firstContactDate,
      ContactSource contactSource);

    BizPartnerInfo GetBizPartner(int contactId);

    BizPartnerInfo GetBizPartner(string contactGuid);

    void UpdateBizPartner(BizPartnerInfo info);

    void DeleteBizPartner(int contactId);

    void DeleteBizPartners(int[] contactIds);

    void MakeBizPartnersPublic(int[] contactIds);

    BizPartnerInfo[] GetBizPartnersByOwner(string ownerId);

    BizPartnerInfo[] GetBizPartners(int[] ids);

    BizPartnerInfo[] GetBizPartners(string[] contactGuids);

    BizPartnerInfo[] QueryBizPartners(QueryCriterion[] criteria, RelatedLoanMatchType loanMatchType);

    BizPartnerSummaryInfo[] QueryBizPartnerSummaries(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType);

    BizPartnerInfo[] QueryBizPartnersForUser(
      string userID,
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType);

    BizPartnerInfo[] QueryAllBizPartners(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType);

    BizPartnerInfo[] QueryMaxAccessibleBizPartners(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType);

    int[] QueryBizPartnerIds(
      string userId,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType);

    ICursor OpenBizPartnerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summariesOnly);

    ICursor OpenAllBizPartnerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summariesOnly);

    ICursor OpenMaxAccessibleBizPartnerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summariesOnly);

    ICursor OpenBizPartnerCursor(
      ContactQueryInfo queryInfo,
      SortField[] sortOrder,
      string[] fields,
      bool summariesOnly);

    int QueryBizPartnerMortgageClause(int categoryID, string mortgageClauseCompany);

    int CreateBorrower(BorrowerInfo info);

    int CreateBorrower(BorrowerInfo info, DateTime firstContactDate, ContactSource contactSource);

    BorrowerInfo CreateBorrowerInfo(
      BorrowerInfo info,
      DateTime firstContactDate,
      ContactSource contactSource);

    BorrowerInfo GetBorrower(int contactId);

    BorrowerInfo GetBorrower(string contactGuid);

    void UpdateBorrower(BorrowerInfo info);

    void DeleteBorrower(int contactId);

    void DeleteBorrowers(int[] contactIds);

    BorrowerInfo[] GetBorrowers(int[] ids);

    BorrowerInfo[] GetBorrowersByOwner(string ownerId);

    BorrowerInfo[] QueryBorrowers(QueryCriterion[] criteria);

    BorrowerInfo[] QueryBorrowersForUser(string userID, QueryCriterion[] criteria);

    BorrowerInfo[] QueryBorrowersConflict(QueryCriterion[] criteria);

    BorrowerInfo[] QueryBorrowers(QueryCriterion[] criteria, RelatedLoanMatchType loanMatchType);

    BorrowerInfo[] QueryAllBorrowers(QueryCriterion[] criteria, RelatedLoanMatchType loanMatchType);

    BorrowerInfo[] QueryMaxAccessibleBorrowers(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType);

    int[] QueryBorrowerIds(
      string userId,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType);

    BorrowerSummaryInfo[] QueryBorrowerSummaries(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType);

    ICursor OpenBorrowerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly);

    ICursor OpenBorrowerCursorForUser(
      string userID,
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly);

    ICursor OpenBorrowerSummaryCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder);

    ICursor OpenAllBorrowerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly);

    ICursor OpenMaxAccessibleBorrowerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly);

    ICursor OpenBorrowerCursor(
      ContactQueryInfo queryInfo,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly);

    ICursor OpenPublicBizPartnerCursor(
      QueryCriterion[] criteria,
      RelatedLoanMatchType loanMatchType,
      SortField[] sortOrder,
      string[] fields,
      bool summariesOnly);

    ContactNote GetContactNote(int noteId, ContactType contactType);

    ContactNote[] GetNotesForContact(int contactId, ContactType contactType);

    ContactNote GetNoteForContact(int contactId, ContactType contactType, int noteId);

    int AddNoteForContact(int contactId, ContactType contactType, ContactNote note);

    void UpdateNoteForContact(int contactId, ContactType contactType, ContactNote note);

    void DeleteNoteForContact(int contactId, ContactType contactType, int noteId);

    ContactHistoryItem[] GetHistoryForContact(int contactId, ContactType contactType);

    ContactHistoryItem[] GetHistoryForContact(
      int contactId,
      ContactType contactType,
      DateTime startDate,
      DateTime endDate);

    ContactHistoryItem[] GetHistoryForContact(
      int contactId,
      ContactType contactType,
      string eventType);

    ContactHistoryItem[] GetHistoryForContact(
      int contactId,
      ContactType contactType,
      string eventType,
      DateTime startDate,
      DateTime endDate);

    ContactHistoryItem GetHistoryItemForContact(
      int contactId,
      ContactType contactType,
      int historyItemId);

    int AddHistoryItemForContact(int contactId, ContactType contactType, ContactHistoryItem item);

    void AddHistoryItemForContacts(
      string[] contactGuid,
      ContactType contactType,
      ContactHistoryItem item);

    void DeleteHistoryItemForContact(int contactId, ContactType contactType, int historyItemId);

    ContactHistoryNoteInfo GetContactHistoryNote(int noteId);

    ContactHistoryNoteInfo[] GetContactHistoryNotes(int[] noteIds);

    BizCategory[] GetBizCategories();

    void DeleteBizCategory(BizCategory cat);

    BizCategory AddBizCategory(string categoryName);

    void UpdateBizCategory(BizCategory cat);

    int AddContactLoan(ContactLoanInfo info);

    ContactLoanInfo GetContactLoan(int loanId);

    ContactLoanInfo[] GetBorrowerLoans(int borrowerId);

    ContactLoanInfo GetLastClosedLoanForContact(int contactId, ContactType contactType);

    ContactCustomFieldInfoCollection GetCustomFieldInfo(ContactType contactType);

    void UpdateCustomFieldInfo(
      ContactType contactType,
      ContactCustomFieldInfoCollection customFields);

    ContactCustomField[] GetCustomFieldsForContact(int contactId, ContactType contactType);

    void UpdateCustomFieldsForContact(
      int contactId,
      ContactType contactType,
      ContactCustomField[] fields);

    CustomFieldsDefinitionInfo[] GetCustomFieldsDefinitions(CustomFieldsType customFieldsType);

    CustomFieldsDefinitionInfo GetCustomFieldsDefinition(
      CustomFieldsType customFieldsType,
      int recordId);

    CustomFieldsDefinitionInfo UpdateCustomFieldsDefinition(
      CustomFieldsDefinitionInfo customFieldsDefinitionInfo);

    CustomFieldsValueInfo GetCustomFieldsValues(int contactId, int categoryId);

    CustomFieldsValueInfo UpdateCustomFieldsValues(CustomFieldsValueInfo customFieldsValues);

    void DeleteContactCustomFieldValues(ContactType contactType, int[] fieldIds);

    CustomFieldsMappingInfo GetCustomFieldsMapping(
      CustomFieldsType customFieldsType,
      int categoryId,
      bool twoWayTransfersOnly);

    ContactQueries GetBizPartnerQueries(string userid);

    void AddBorrowerQuery(string userid, ContactQuery query);

    void DeleteBorrowerQuery(string userid, ContactQuery query);

    void UpdateBorrowerQuery(string userid, ContactQuery query);

    void SetBorrowerQueries(string userid, ContactQueries queries);

    ContactQueries GetBorrowerQueries(string userid);

    void AddBizPartnerQuery(string userid, ContactQuery query);

    void DeleteBizPartnerQuery(string userid, ContactQuery query);

    void UpdateBizPartnerQuery(string userid, ContactQuery query);

    BorrowerStatus GetBorrowerStatus();

    void SetBorrowerStatus(BorrowerStatusItem item);

    void SetBorrowerStatus(BorrowerStatusItem[] items);

    void CreateBorrowerStatus(BorrowerStatusItem item);

    void UpdateBorrowerStatusItem(int index, BorrowerStatusItem item, string oldName);

    void RenameStatusInBorrowerTable(string oldStatus, string newStatus);

    void UpdateBorrowerOpportunity(Opportunity item);

    void DeleteBorrowerOpportunity(int itemId);

    void DeleteOpportunityByBorrowerId(int contactId);

    int CreateBorrowerOpportunity(Opportunity item);

    Opportunity GetBorrowerOpportunity(int itemId);

    Opportunity GetOpportunityByBorrowerId(int borrowerId);

    string[] GetSyncConfigurationNames();

    BinaryObject GetSyncConfiguration(string name);

    void SaveSyncConfiguration(string name, BinaryObject o);

    void DeleteSyncConfiguration(string name);

    void RenameSyncConfiguration(BinaryObject o, string oldName, string newName);

    BinaryObject GetSyncMap(string name);

    void SaveSyncMap(string name, BinaryObject o);

    void AddCreditScoresForHistoryItem(
      int contactId,
      int historyId,
      ContactCreditScores[] contactScoresList);

    ContactCreditScores[] GetCreditScoresForHistoryItem(int contactId, int historyId);

    string GetBizPartnerAdvQueryPath(string userID);

    string GetBorrowerAdvQueryPath(string userID);

    DataTable GetUnSyncBorrowerContacts(string loanFolder);

    ContactLoanPair[] GetRelatedLoansForContact(int contactId, ContactType contactType);

    ContactLoanPair[] GetRelatedLoansForContact(string contactGuid);

    ContactLoanPair[] GetRelatedContactsForLoan(string loanGuid);

    TaskInfo GetTask(int taskID);

    int InsertOrUpdateTask(TaskInfo task);

    void DeleteTask(int taskID);

    void DeleteTasks(int[] taskIDs);

    TaskInfo[] GetAllTasksForUser(string userid);

    TaskInfo[] QueryTasks(QueryCriterion criteria);

    TaskInfo[] QueryTasks(QueryCriterion criteria, bool fromHomePage);
  }
}
