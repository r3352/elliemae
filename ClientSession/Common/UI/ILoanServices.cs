// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ILoanServices
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DocEngine;
using System.Collections;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public interface ILoanServices
  {
    string GetUserPaymentToken(string settingType);

    string GetUserPaymentType(string settingType);

    void OrderComplianceReport();

    bool AllowClearComplianceAlerts();

    bool IsTaxServiceInstalled { get; }

    void OrderTaxReturns();

    void OrderTaxReturns(int recordIndex);

    void CheckTaxReturnStatus();

    void CheckTaxReturnStatus(int recordIndex);

    void OrderFraud();

    void OrderFlood();

    bool IsSSNServiceInstalled();

    void OrderSSNService();

    void CheckSSNServiceStatus();

    void OrderDataTrac();

    bool IsDocServiceInstalled();

    void SelectPlanCode();

    void SelectAltLender();

    void AuditClosingDocs();

    void OrderClosingDocs();

    void OrderDisclosures();

    void ViewClosingDocs(string logRecordID);

    DocEngineFieldData GetClosingDocumentFieldData(LoanData loan);

    string GetComplianceRequestXML();

    string CompliancePreAudit();

    string ComplianceLinkedLoanPreAudit();

    Hashtable GetClosingDocSettingsFromECS();

    void SetClosingDocSettingsToECS(Hashtable settingsToSave);

    DocumentOrderType GetAllowedDocumentOrderTypes();

    bool IsEncompassDocServiceAvailable(DocumentOrderType orderType);

    Task<bool> IsEClosingAllowed(SessionObjects sessionObjects);

    bool VerifyDocServiceSetup(DocumentOrderType orderType, bool showWarning = true);

    bool IsExportServiceAccessible(LoanDataMgr loanDataMgr, ServiceSetting serviceSetting);

    bool ExportServiceProcessLoans(
      LoanDataMgr loanDataMgr,
      ServiceSetting serviceSetting,
      string[] loanGuids);

    bool ExportServiceValidateLoan(
      LoanDataMgr loanDataMgr,
      ServiceSetting serviceSetting,
      string loanGuid);

    void ExportServiceProcessLoan(
      LoanDataMgr loanDataMgr,
      ServiceSetting serviceSetting,
      string loanGuid);

    bool ExportServiceExportData(
      LoanDataMgr loanDataMgr,
      ServiceSetting serviceSetting,
      string[] loanGuids);

    Hashtable GetClientServicesWarningSettings();

    bool IsInvestorMissingRequiredReports(LoanDataMgr loanDataMgr, int investorID);

    string GetComplianceRequestXML(string ReportRunType);

    string GetLinkedLoanComplianceRequestXML(string ReportRunType);
  }
}
