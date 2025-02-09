// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExportMap
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  internal class ExportMap
  {
    private Dictionary<string, string> TPOMap = new Dictionary<string, string>();
    private Dictionary<string, string> TPOExportExpensesMap = new Dictionary<string, string>();

    public void InitializeTPOMap(bool isTPOMVP = false)
    {
      this.TPOMap.Add("First_name", "FirstName");
      this.TPOMap.Add("Middle_name", "MiddleName");
      this.TPOMap.Add("Last_name", "LastName");
      this.TPOMap.Add("Suffix_name", "Suffix");
      this.TPOMap.Add("Title", "Title");
      this.TPOMap.Add("UseCompanyAddress", "UseCompanyAddress");
      this.TPOMap.Add("Address1", "Address");
      this.TPOMap.Add("City", "City");
      this.TPOMap.Add("State", "State");
      this.TPOMap.Add("Zip", "Zipcode");
      this.TPOMap.Add("Email", "Email");
      this.TPOMap.Add("Phone", "Phone");
      this.TPOMap.Add("Fax", "Fax");
      this.TPOMap.Add("Cell_phone", "CellPhone");
      this.TPOMap.Add("SSN", "SSN");
      this.TPOMap.Add("DisabledLogin", "DisabledLogin");
      this.TPOMap.Add("Login_email", "EmailForLogin");
      this.TPOMap.Add("InheritParentRateSheet", "UseParentInfoForRateLock");
      this.TPOMap.Add("Rate_sheet_email", "EmailForRateSheet");
      this.TPOMap.Add("Rate_sheet_fax", "FaxForRateSheet");
      this.TPOMap.Add("Lock_info_email", "EmailForLockInfo");
      this.TPOMap.Add("Lock_info_fax", "FaxForLockInfo");
      this.TPOMap.Add("NMLSOriginatorID", "NmlsID");
      this.TPOMap.Add("NMLSCurrent", "NMLSCurrent");
      this.TPOMap.Add("Approval_status", "ApprovalCurrentStatus");
      this.TPOMap.Add("Approval_status_watchlist", "AddToWatchlist");
      this.TPOMap.Add("Approval_status_date", "ApprovalCurrentStatusDate");
      this.TPOMap.Add("Approval_date", "ApprovalDate");
      this.TPOMap.Add("Sales_rep_name", "SalesRepName");
      this.TPOMap.Add("Sales_rep_userid", "SalesRepID");
      if (isTPOMVP)
        this.TPOMap.Add("UserPersonas", "UserPersonas");
      else
        this.TPOMap.Add("Roles", "Roles");
      this.TPOMap.Add("Note", "Notes");
    }

    public Dictionary<string, string> GetTPOMap(bool isTPOMVP = false)
    {
      this.TPOMap.Clear();
      this.InitializeTPOMap(isTPOMVP);
      return this.TPOMap;
    }

    public void InitializeTPOExportExpensesMap()
    {
      this.TPOExportExpensesMap.Add("LicenseExempt", "Exempt");
      this.TPOExportExpensesMap.Add("State", "StateAbbrevation");
      this.TPOExportExpensesMap.Add("LicenseType", "LicenseType");
      this.TPOExportExpensesMap.Add("LicenseNumber", "LicenseNo");
      this.TPOExportExpensesMap.Add("IssueDate", "IssueDate");
      this.TPOExportExpensesMap.Add("StartDate", "StartDate");
      this.TPOExportExpensesMap.Add("EndDate", "EndDate");
      this.TPOExportExpensesMap.Add("Status", "LicenseStatus");
      this.TPOExportExpensesMap.Add("StatusDate", "StatusDate");
      this.TPOExportExpensesMap.Add("LastCheckedDate", "LastChecked");
    }

    public Dictionary<string, string> GetTPOExportExpensesMap()
    {
      this.TPOExportExpensesMap.Clear();
      this.InitializeTPOExportExpensesMap();
      return this.TPOExportExpensesMap;
    }
  }
}
