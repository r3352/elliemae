// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoan
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.BusinessObjects.Loans.Servicing;
using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [System.Runtime.InteropServices.Guid("D39CAE4D-3C0B-43e3-B59E-6B65740E9347")]
  public interface ILoan
  {
    string LoanFolder { get; set; }

    string LoanName { get; set; }

    string Guid { get; }

    string LoanNumber { get; set; }

    string EncompassVersion { get; set; }

    string MersNumber { get; set; }

    DateTime LastModified { get; }

    LoanFields Fields { get; }

    LoanBorrowerPairs BorrowerPairs { get; }

    LoanLiabilities Liabilities { get; }

    LoanMortgages Mortgages { get; }

    LoanDeposits Deposits { get; }

    LoanResidences BorrowerResidences { get; }

    LoanResidences CoBorrowerResidences { get; }

    LoanEmployers BorrowerEmployers { get; }

    LoanEmployers CoBorrowerEmployers { get; }

    LoanURLAAdditionalLoans URLAAdditionalLoans { get; }

    LoanURLAGiftsGrants URLAGiftsGrants { get; }

    LoanURLAOtherAssets URLAOtherAssets { get; }

    LoanURLAOtherIncome URLAOtherIncome { get; }

    LoanURLAOtherLiabilities URLAOtherLiabilities { get; }

    void SendToLoanOfficer(User loanOfficer);

    void SendToProcessing(User loanProcessor);

    void Lock();

    void Unlock();

    void Recalculate();

    void Export(string filePath, string exportKey, LoanExportFormat exportFormat);

    void Import(string filePath, LoanImportFormat importFormat);

    bool IsNew { get; }

    void Commit();

    void Refresh();

    void Close();

    void Delete();

    bool Equals(object obj);

    LoanLog Log { get; }

    void ImportFromBytes(ref byte[] importData, LoanImportFormat format);

    void Move(EllieMae.Encompass.BusinessObjects.Loans.LoanFolder newFolder, string newLoanName);

    string LoanOfficerID { get; }

    string LoanProcessorID { get; }

    string LoanCloserID { get; }

    LoanAttachments Attachments { get; }

    void ForceLock();

    void ForceUnlock();

    LoanAccessRights GetAccessRights();

    LoanAccessRights GetAssignedAccessRights(User user);

    LoanAccessRights GetEffectiveAccessRights(User user);

    void AssignRights(User user, LoanAccessRights rights);

    UserList GetUsersWithAssignedRights();

    void ImportWithTemplate(string filePath, LoanImportFormat format, LoanTemplate template);

    bool CalculationsEnabled { get; set; }

    bool BusinessRulesEnabled { get; set; }

    LoanLock GetCurrentLock();

    void ImportWithLoanOfficer(
      string filePath,
      LoanImportFormat format,
      LoanTemplate template,
      User user);

    LoanAssociates Associates { get; }

    LoanAuditTrail AuditTrail { get; }

    Loan LinkedLoan { get; }

    void LinkTo(Loan linkLoan);

    void Unlink();

    LoanServicing Servicing { get; }

    void MoveToFolder(EllieMae.Encompass.BusinessObjects.Loans.LoanFolder folder);

    DataObject GetCustomDataObject(string name);

    void SaveCustomDataObject(string name, DataObject data);

    void DeleteCustomDataObject(string name);

    void ExecuteCalculation(string calcName);

    LoanContacts Contacts { get; }

    void ApplyTemplate(Template tmpl, bool appendData);

    bool Locked { get; }

    string ExportAsText(string exportKey, LoanExportFormat exportFormat);

    bool Modified { get; }

    void AppendToCustomDataObject(string name, DataObject data);

    FundingFeeList GetFundingFees(bool hideZero);

    MilestoneTemplate MilestoneTemplate { get; }

    void ApplyManualMilestoneTemplate(
      MilestoneTemplate milestoneTemplate,
      bool forceApplyMilestoneTemplate);

    void ApplyBestMatchingMilestoneTemplate(bool forceApplyMilestoneTemplate);

    bool MSLock { get; set; }

    bool MSDateLock { get; set; }

    string GetUCDForLoanEstimate(bool setTotalFields);

    string GetUCDForClosingDisclosure(bool setTotalFields);
  }
}
