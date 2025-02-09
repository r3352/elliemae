// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMSystemTableEBSTrigger
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using Elli.AdvCode.Runtime;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DDMSystemTableEBSTrigger
  {
    private const string className = "DDMSystemTableEBSTrigger�";
    private static readonly string sw = Tracing.SwDataEngine;
    private const string TBL_TYPE_DDM = "DDM�";
    private const string TBL_TYPE_ESCROWTITLE = "ESCROWTITLE�";
    private const string TBL_TYPE_TAX = "TAX�";
    private const string TBL_TYPE_MI = "MI�";
    private SessionObjects sessionObjects;
    private Hashtable systemMITables;
    private IFieldProvider fieldProvider;
    private bool fromPlatform;
    private FeeManagementSetting _feeManagementSetting;
    private FeeManagementPersonaInfo _feeManagementPermission;
    private TableFeeListBase escrowPurchaseTable;
    private TableFeeListBase escrowRefiTable;
    private TableFeeListBase titlePurchaseTable;
    private TableFeeListBase titleRefiTable;
    private Hashtable cityStateUserTables;
    private string errorMessage = "";

    public DDMSystemTableEBSTrigger(
      SessionObjects sessionObjects,
      IFieldProvider fieldProvider,
      bool fromPlatform = false)
    {
      this.sessionObjects = sessionObjects;
      this.fieldProvider = fieldProvider;
      this.fromPlatform = fromPlatform;
    }

    public bool ApplyFeeManagement(string fieldID, object value)
    {
      Tracing.Log(DDMSystemTableEBSTrigger.sw, TraceLevel.Info, nameof (DDMSystemTableEBSTrigger), "DDMSystemTableEBSTrigger (ApplyFeeManagement) is triggered.");
      string[] strArray = Convert.ToString(fieldID).Split(':');
      if (strArray.Length <= 2)
      {
        this.errorMessage = "DDM - Invalid format detected in DDM:FEEMANAGMENT handling : " + fieldID;
        Tracing.Log(DDMSystemTableEBSTrigger.sw, TraceLevel.Error, nameof (DDMSystemTableEBSTrigger), this.errorMessage);
        return false;
      }
      string str = strArray.Length != 0 ? strArray[2] : (string) null;
      if (str == null)
      {
        this.errorMessage = "DDM - required field id is missing in DDM:FEEMANAGMENT: - " + str;
        Tracing.Log(DDMSystemTableEBSTrigger.sw, TraceLevel.Error, nameof (DDMSystemTableEBSTrigger), this.errorMessage);
        return false;
      }
      if (this._feeManagementSetting == null || this._feeManagementPermission == null)
      {
        ILoanConfigurationInfo configurationInfo = this.sessionObjects.LoanManager.GetLoanConfigurationInfo();
        this._feeManagementSetting = configurationInfo.FeeManagementList;
        this._feeManagementPermission = configurationInfo.FeeManagementPersonaPermission;
      }
      bool companyOptIn = this._feeManagementSetting.CompanyOptIn;
      FeeSectionEnum fieldSectionEnum = DDMItemizationFieldInfo.GetFieldSectionEnum(str);
      bool flag = this._feeManagementPermission != null && this._feeManagementPermission.IsSectionEditable(fieldSectionEnum);
      if (companyOptIn & flag)
        this.setField(str, Convert.ToString(value));
      else
        this.setField(str, Convert.ToString(value));
      return true;
    }

    public bool ApplySystemTable(string fieldID, object value)
    {
      string[] strArray = Convert.ToString(value).Split('|');
      string str1 = strArray.Length != 0 ? strArray[0] : (string) null;
      string tableName = strArray.Length > 1 ? strArray[1] : (string) null;
      string lineNumber = strArray.Length > 2 ? strArray[2] : (string) null;
      switch (str1)
      {
        case "DDM":
          this.errorMessage = "DDM table definition was found in the system table execution : " + str1;
          return false;
        case "ESCROWTITLE":
        case "TAX":
        case "MI":
          string str2 = "GET" + str1;
          if (string.IsNullOrEmpty(str2))
          {
            this.errorMessage = "Unknown system table calc-name : " + value;
            return false;
          }
          bool flag = false;
          switch (str2)
          {
            case "GETMI":
              flag = this.applyMortgageInsurance();
              break;
            case "GETTAX":
              flag = this.applyTax(tableName, lineNumber);
              break;
            case "GETESCROWTITLE":
              flag = !(lineNumber == "1102c") ? this.applyTitleFee(tableName, lineNumber) : this.applyEscrowFee(tableName);
              break;
          }
          return flag;
        default:
          this.errorMessage = "Unknown system table calc-name was found in the system table execution : " + str1;
          return false;
      }
    }

    public bool applyMortgageInsurance()
    {
      Tracing.Log(DDMSystemTableEBSTrigger.sw, TraceLevel.Info, nameof (DDMSystemTableEBSTrigger), "DDMSystemTableEBSTrigger (ApplyMortgageInsurance) is triggered.");
      this.errorMessage = "";
      string strField = this.getStrField("1172");
      LoanTypeEnum loanTypeEnum = LoanTypeEnum.Other;
      switch (strField)
      {
        case "Conventional":
          loanTypeEnum = LoanTypeEnum.Conventional;
          break;
        case "FHA":
          loanTypeEnum = LoanTypeEnum.FHA;
          break;
        case "VA":
          loanTypeEnum = LoanTypeEnum.VA;
          break;
      }
      if (this.systemMITables == null)
        this.systemMITables = CollectionsUtil.CreateCaseInsensitiveHashtable();
      MIRecord[] miRecordArray;
      if (this.systemMITables.ContainsKey((object) loanTypeEnum))
      {
        miRecordArray = (MIRecord[]) this.systemMITables[(object) loanTypeEnum];
      }
      else
      {
        miRecordArray = this.sessionObjects.ConfigurationManager.GetMIRecords(loanTypeEnum, "");
        this.systemMITables.Add((object) loanTypeEnum, (object) miRecordArray);
      }
      if (miRecordArray == null || miRecordArray.Length == 0)
      {
        this.errorMessage = "The current loan information does not meet any criteria in MI tables.";
        Tracing.Log(DDMSystemTableEBSTrigger.sw, TraceLevel.Info, nameof (DDMSystemTableEBSTrigger), "DDMSystemTableEBSTrigger (GetMI): No MI record from setting");
        return false;
      }
      bool isStreamLine = this.getStrField("MORNET.X40") == "StreamlineWithAppraisal" || this.getStrField("MORNET.X40") == "StreamlineWithoutAppraisal";
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < miRecordArray.Length; ++index)
      {
        if (miRecordArray[index].Scenarios != null && miRecordArray[index].Scenarios.Length != 0 && this.recordMatched(miRecordArray[index].Scenarios, strField, isStreamLine))
          arrayList.Add((object) miRecordArray[index]);
      }
      if (arrayList.Count == 0)
      {
        this.errorMessage = "The current loan information does not meet any criteria in MI tables.";
        Tracing.Log(DDMSystemTableEBSTrigger.sw, TraceLevel.Info, nameof (DDMSystemTableEBSTrigger), "ExecuteTable_MI: No MI record matched");
        return false;
      }
      MIRecord miRecord = ((MIRecord[]) arrayList.ToArray(typeof (MIRecord)))[0];
      if (loanTypeEnum == LoanTypeEnum.FHA || loanTypeEnum == LoanTypeEnum.Conventional || loanTypeEnum == LoanTypeEnum.VA && this.getStrField("VASUMM.X49") == "Y")
        this.setField("1107", miRecord.Premium1st == 0.0 ? "" : miRecord.Premium1st.ToString("N6"));
      else if (loanTypeEnum == LoanTypeEnum.VA)
        this.setField("1107", miRecord.Premium1st == 0.0 ? "" : miRecord.SubsequentPremium.ToString("N6"));
      this.setField("1199", miRecord.Monthly1st == 0.0 ? "" : miRecord.Monthly1st.ToString("N3"));
      this.setField("1201", miRecord.Monthly2st == 0.0 ? "" : miRecord.Monthly2st.ToString("N3"));
      int intField = this.getIntField("4");
      if (miRecord.Months1st > 0)
      {
        if (miRecord.Months1st > intField && intField > 0)
          this.setField("1198", intField.ToString());
        else if (miRecord.Months1st >= 999 && intField <= 0)
          this.setField("1198", "0");
        else
          this.setField("1198", miRecord.Months1st.ToString());
      }
      else
        this.setField("1198", "");
      if (miRecord.Months2st > 0)
      {
        if (miRecord.Months2st > intField && intField > 0)
          this.setField("1200", intField.ToString());
        else if (miRecord.Months2st >= 999 && intField <= 0)
          this.setField("1200", "0");
        else
          this.setField("1200", miRecord.Months2st.ToString());
      }
      else
        this.setField("1200", "");
      this.setField("1205", miRecord.Cutoff == 0.0 ? "" : miRecord.Cutoff.ToString("N3"));
      double num1 = 0.0;
      if (this.getStrField("1757") == "")
      {
        switch (strField)
        {
          case "VA":
          case "Conventional":
            this.setField("1757", "Loan Amount");
            break;
          case "FHA":
            this.setField("1757", "Base Loan Amount");
            break;
        }
      }
      switch (this.getStrField("1757"))
      {
        case "Loan Amount":
          num1 = Utils.ParseDouble((object) this.getStrField("2"));
          break;
        case "Purchase Price":
          num1 = Utils.ParseDouble((object) this.getStrField("136"));
          break;
        case "Appraisal Value":
          num1 = Utils.ParseDouble((object) this.getStrField("356"));
          break;
        case "Base Loan Amount":
          num1 = Utils.ParseDouble((object) this.getStrField("1109"));
          break;
      }
      double num2;
      if (strField == "FarmersHomeAdministration")
      {
        num2 = Utils.ArithmeticRounding(miRecord.Monthly1st / 100.0 * num1 / 12.0, 2);
        this.setField("1766", num2 == 0.0 ? "" : num2.ToString("N3"));
      }
      if (miRecord.Monthly1st == 0.0 || num1 == 0.0)
      {
        this.setField("232", "");
        this.setField("NEWHUD.X1707", "");
        this.setField("3971", "");
      }
      if (strField == "FarmersHomeAdministration")
      {
        num2 = Utils.ArithmeticRounding(miRecord.Monthly2st / 100.0 * num1 / 12.0, 2);
        this.setField("1770", num2 == 0.0 ? "" : num2.ToString("N3"));
      }
      if (miRecord.Monthly1st == 0.0 || num1 == 0.0)
      {
        double doubleField = this.getDoubleField("1766");
        if (strField == "FarmersHomeAdministration")
        {
          this.setField("NEWHUD.X1707", doubleField.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          if (this.getStrField("232") != string.Empty)
            this.copyPOCPTCAPRFromLine1003ToLine1010((string) null, (string) null);
          this.setField("232", "");
        }
        else
          this.setField("NEWHUD.X1707", "");
      }
      this.setField("1765", "");
      this.setField("3625", "");
      return true;
    }

    private bool recordMatched(FieldFilter[] filters, string loanType, bool isStreamLine)
    {
      string str1 = "";
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 0; index < filters.Length; ++index)
      {
        string strField = this.getStrField(filters[index].FieldID);
        if ((loanType == "FHA" && filters[index].FieldID == "3042" && filters[index].OperatorType != OperatorTypes.EmptyDate || loanType == "FHA" & isStreamLine && filters[index].FieldID == "3432") && (strField == string.Empty || strField == "//"))
          strField = DateTime.Today.ToString("MM/dd/yyyy");
        string scriptCommands = filters[index].GetScriptCommands(strField);
        str1 = !(str1 == "") ? str1 + " " + scriptCommands : scriptCommands;
      }
      string str2 = str1.Trim().ToLower();
      if (str2 == "")
        return true;
      if (str2.EndsWith("and"))
        str2 = str2.Substring(0, str2.Length - 3);
      if (str2.EndsWith("or"))
        str2 = str2.Substring(0, str2.Length - 2);
      return Utils.CheckFilter(str2.Trim()) == "true";
    }

    public bool applyEscrowFee(string tableName)
    {
      string strField = this.getStrField("19");
      TableFeeListBase tableFeeListBase = (TableFeeListBase) null;
      if (strField == "Purchase")
        tableFeeListBase = this.escrowPurchaseTable == null ? (TableFeeListBase) this.sessionObjects.ConfigurationManager.GetSystemSettings("TblEscrowPurList") : this.escrowPurchaseTable;
      else if (strField.EndsWith("Refinance"))
        tableFeeListBase = this.escrowRefiTable == null ? (TableFeeListBase) this.sessionObjects.ConfigurationManager.GetSystemSettings("TblEscrowRefiList") : this.escrowRefiTable;
      if (tableFeeListBase == null)
      {
        this.errorMessage = "DDM Error - cannot find system table name " + tableName + " based on loan purpose \"" + strField + "\".";
        return false;
      }
      TableFeeListBase.FeeTable table = tableFeeListBase.GetTable(tableName);
      if (table == null)
      {
        this.errorMessage = "DDM Error - cannot find system table name " + tableName;
        return false;
      }
      Decimal num = Convert.ToDecimal(this.calcTitleEscrowRate(table));
      this.fieldProvider.SetFieldValue("ESCROW_TABLE", (object) tableName);
      this.fieldProvider.SetFieldValue("NEWHUD.X808", (object) num);
      return true;
    }

    public bool applyTitleFee(string tableName, string lineNumber)
    {
      string strField = this.getStrField("19");
      TableFeeListBase tableFeeListBase = (TableFeeListBase) null;
      if (strField == "Purchase")
        tableFeeListBase = this.titlePurchaseTable == null ? (TableFeeListBase) this.sessionObjects.ConfigurationManager.GetSystemSettings("TblTitlePurList") : this.titlePurchaseTable;
      else if (strField.EndsWith("Refinance"))
        tableFeeListBase = this.titleRefiTable == null ? (TableFeeListBase) this.sessionObjects.ConfigurationManager.GetSystemSettings("TblTitleRefiList") : this.titleRefiTable;
      if (tableFeeListBase == null)
      {
        this.errorMessage = "DDM Error - cannot find system table name " + tableName + " based on loan purpose \"" + strField + "\".";
        return false;
      }
      TableFeeListBase.FeeTable table = tableFeeListBase.GetTable(tableName);
      if (table == null)
      {
        this.errorMessage = "DDM Error - cannot find system table name " + tableName;
        return false;
      }
      if (lineNumber == "1103" && table.FeeType != "Owner" || lineNumber == "1104" && table.FeeType != "Lender")
      {
        Tracing.Log(DDMSystemTableEBSTrigger.sw, TraceLevel.Info, nameof (DDMSystemTableEBSTrigger), "ExecuteTable_TitleEscrow: Cannot find table name " + tableName + " with \"" + (lineNumber == "1103" ? "Owner" : "Lender") + "\" Type from setting");
        return true;
      }
      Decimal num = Convert.ToDecimal(this.calcTitleEscrowRate(table));
      switch (lineNumber)
      {
        case "1103":
          this.fieldProvider.SetFieldValue("2010TITLE_TABLE", (object) tableName);
          this.fieldProvider.SetFieldValue("NEWHUD.X572", (object) num);
          break;
        case "1104":
          this.fieldProvider.SetFieldValue("TITLE_TABLE", (object) tableName);
          this.fieldProvider.SetFieldValue("NEWHUD.X639", (object) num);
          break;
        default:
          return false;
      }
      return true;
    }

    public bool applyTax(string tableName, string lineNumber)
    {
      if (this.cityStateUserTables == null || this.cityStateUserTables.Count == 0)
      {
        this.cityStateUserTables = this.sessionObjects.ConfigurationManager.GetSystemSettings(new string[3]
        {
          "FeeCityList",
          "FeeStateList",
          "FeeUserList"
        });
        if (this.cityStateUserTables == null || this.cityStateUserTables.Count == 0)
        {
          this.errorMessage = "DDM - Cannot find setting from City/State/User tables.";
          return false;
        }
      }
      int num;
      switch (lineNumber)
      {
        case "1204":
          num = 0;
          break;
        case "1205":
          num = 1;
          break;
        default:
          num = 2;
          break;
      }
      int key = num;
      FeeListBase feeListBase = (FeeListBase) null;
      if (this.cityStateUserTables.ContainsKey((object) key))
        feeListBase = (FeeListBase) (BinaryObject) this.cityStateUserTables[(object) key];
      if (feeListBase == null || !feeListBase.TableNameExists(tableName))
      {
        this.errorMessage = "DDM - Cannot find table '" + tableName + "' from City/State/User tables based on line number: " + lineNumber;
        return false;
      }
      FeeListBase.FeeTable nonCaseSensitive = feeListBase.GetTableNonCaseSensitive(tableName);
      switch (key)
      {
        case 0:
          this.updateTaxFee("1637", "647", "RE88395.X94", "593", nonCaseSensitive);
          break;
        case 1:
          this.updateTaxFee("1638", "648", "RE88395.X89", "594", nonCaseSensitive);
          break;
        default:
          switch (lineNumber)
          {
            case "1206":
              this.updateTaxFee("373", "374", "RE88395.X99", "576", nonCaseSensitive);
              break;
            case "1207":
              this.updateTaxFee("1640", "1641", "RE88395.X100", "1642", nonCaseSensitive);
              break;
            default:
              this.updateTaxFee("1643", "1644", "RE88395.X169", "1645", nonCaseSensitive);
              break;
          }
          break;
      }
      return true;
    }

    private void updateTaxFee(
      string feeNameId,
      string borFieldId,
      string caFieldId,
      string sellerId,
      FeeListBase.FeeTable fee)
    {
      double doubleField1 = this.getDoubleField("2");
      double doubleField2 = this.getDoubleField("136");
      double doubleField3 = this.getDoubleField(sellerId);
      double num1 = (fee.CalcBasedOn == "Loan Amount" ? doubleField1 : doubleField2) * (Utils.ParseDouble((object) fee.Rate) / 100.0) + Utils.ParseDouble((object) fee.Additional);
      double num2 = num1 - doubleField3;
      this.setField(borFieldId, num2.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.setField(caFieldId, num1.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.setField(feeNameId, fee.FeeName);
    }

    private double calcTitleEscrowRate(TableFeeListBase.FeeTable t)
    {
      if (t.RateList == string.Empty)
        return 0.0;
      double num1 = 0.0;
      switch (t.CalcBasedOn)
      {
        case "Sales Price":
          num1 = this.getDoubleField("136");
          break;
        case "Loan Amount":
          num1 = this.getDoubleField("2");
          break;
        case "Appraisal Value":
          num1 = this.getDoubleField("356");
          break;
        case "Base Loan Amount":
          num1 = this.getDoubleField("1109");
          break;
      }
      string empty = string.Empty;
      string[] strArray1 = t.RateList.Split('|');
      int length = strArray1.Length;
      double num2 = 0.0;
      double num3 = 0.0;
      for (int index = 0; index < length; ++index)
      {
        string[] strArray2 = strArray1[index].Split(':');
        if (strArray2.Length > 2)
        {
          double num4 = Utils.ParseDouble((object) strArray2[0]);
          if (num1 <= num4)
          {
            num2 = Utils.ParseDouble((object) strArray2[1]);
            num3 = Utils.ParseDouble((object) strArray2[2]) / 100.0;
            break;
          }
        }
      }
      double num5 = Utils.ParseDouble((object) t.Nearest);
      double num6 = Utils.ParseDouble((object) t.Offset);
      if (num5 != 0.0)
      {
        double num7 = num1 % num5;
        if (num7 != 0.0)
        {
          if (t.Rounding == "Up")
            num1 += num5 - num7;
          else
            num1 -= num7;
        }
        num1 -= num6;
      }
      return Utils.ArithmeticRounding(num1 * num3 + num2, 0);
    }

    private void copyPOCPTCAPRFromLine1003ToLine1010(string id, string val)
    {
      this.setField("NEWHUD.X1706", this.getIntField("1296").ToString());
      this.setField("NEWHUD.X1707", Convert.ToDecimal(this.getStrField("232")).ToString());
      this.setField("NEWHUD.X1711", this.getStrField("SYS.X43"));
      this.setField("NEWHUD.X1710", this.getStrField("SYS.X319"));
      this.setField("PTC.X346", this.getStrField("PTC.X40"));
      this.setField("PTC.X347", this.getDoubleField("PTC.X118").ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.setField("PTC.X348", this.getStrField("PTC.X196"));
      this.setField("POPT.X346", this.getStrField("POPT.X40"));
      this.setField("POPT.X347", this.getDoubleField("POPT.X118").ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.setField("POPT.X348", this.getStrField("POPT.X196"));
      this.setField("1296", "");
      this.setField("232", "");
      this.setField("SYS.X43", "");
      this.setField("SYS.X319", "");
      this.setField("PTC.X40", "");
      this.setField("PTC.X118", "");
      this.setField("PTC.X196", "");
      this.setField("POPT.X40", "");
      this.setField("POPT.X118", "");
      this.setField("POPT.X196", "");
    }

    private string getStrField(string id)
    {
      if ((id ?? "") == "")
        return "";
      object fieldValue = this.fieldProvider.GetFieldValue(id);
      return fieldValue == null ? "" : fieldValue.ToString();
    }

    private int getIntField(string id)
    {
      if ((id ?? "") == "")
        return 0;
      object fieldValue = this.fieldProvider.GetFieldValue(id);
      return fieldValue == null ? 0 : Utils.ParseInt(fieldValue, 0);
    }

    private double getDoubleField(string id)
    {
      if ((id ?? "") == "")
        return 0.0;
      object fieldValue = this.fieldProvider.GetFieldValue(id);
      return fieldValue == null ? 0.0 : Utils.ParseDouble(fieldValue, 0.0);
    }

    private void setField(string id, string val)
    {
      this.fieldProvider.SetFieldValue(id, (object) val);
    }

    public string ErrorMessage => this.errorMessage;
  }
}
