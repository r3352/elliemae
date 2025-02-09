// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.SystemTableCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class SystemTableCalculation : CalculationBase
  {
    private const string ClassName = "SystemTableCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    private ILoanConfigurationInfo configInfo;
    private Hashtable systemTables;

    public SystemTableCalculation(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.configInfo = configInfo;
    }

    internal void ExecuteTable_MI(bool showMessage)
    {
      Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_MI is triggered.");
      string loanType = this.Val("1172");
      LoanTypeEnum loanTypeEnum = LoanTypeEnum.Other;
      switch (loanType)
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
      if (this.systemTables == null)
        this.systemTables = CollectionsUtil.CreateCaseInsensitiveHashtable();
      MIRecord[] miRecordArray;
      if (this.systemTables.ContainsKey((object) loanTypeEnum))
      {
        miRecordArray = (MIRecord[]) this.systemTables[(object) loanTypeEnum];
      }
      else
      {
        miRecordArray = this.sessionObjects.ConfigurationManager.GetMIRecords(loanTypeEnum, "");
        this.systemTables.Add((object) loanTypeEnum, (object) miRecordArray);
      }
      if (miRecordArray == null || miRecordArray.Length == 0)
      {
        if (showMessage)
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "The current loan information does not meet any criteria in MI tables.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_MI: No MI record from setting");
      }
      else
      {
        bool isStreamLine = this.loan.GetField("MORNET.X40") == "StreamlineWithAppraisal" || this.loan.GetField("MORNET.X40") == "StreamlineWithoutAppraisal";
        ArrayList arrayList = new ArrayList();
        for (int index = 0; index < miRecordArray.Length; ++index)
        {
          if (miRecordArray[index].Scenarios != null && miRecordArray[index].Scenarios.Length != 0 && this.recordMatched(miRecordArray[index].Scenarios, loanType, isStreamLine, this.loan))
            arrayList.Add((object) miRecordArray[index]);
        }
        if (arrayList.Count == 0)
        {
          if (showMessage)
          {
            int num = (int) Utils.Dialog((IWin32Window) null, "The current loan information does not meet any criteria in MI tables.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_MI: No MI record matched");
        }
        else
        {
          MIRecord miRecord = ((MIRecord[]) arrayList.ToArray(typeof (MIRecord)))[0];
          if (loanTypeEnum == LoanTypeEnum.FHA || loanTypeEnum == LoanTypeEnum.Conventional || loanTypeEnum == LoanTypeEnum.VA && this.Val("VASUMM.X49") == "Y")
            this.SetVal("1107", miRecord.Premium1st == 0.0 ? "" : miRecord.Premium1st.ToString("N6"));
          else if (loanTypeEnum == LoanTypeEnum.VA)
            this.SetVal("1107", miRecord.Premium1st == 0.0 ? "" : miRecord.SubsequentPremium.ToString("N6"));
          this.SetVal("1199", miRecord.Monthly1st == 0.0 ? "" : miRecord.Monthly1st.ToString("N3"));
          this.SetVal("1201", miRecord.Monthly2st == 0.0 ? "" : miRecord.Monthly2st.ToString("N3"));
          int num1 = this.IntVal("4");
          if (miRecord.Months1st > 0)
          {
            if (miRecord.Months1st > num1 && num1 > 0)
              this.SetVal("1198", num1.ToString());
            else if (miRecord.Months1st >= 999 && num1 <= 0)
              this.SetVal("1198", "0");
            else
              this.SetVal("1198", miRecord.Months1st.ToString());
          }
          else
            this.SetVal("1198", "");
          if (miRecord.Months2st > 0)
          {
            if (miRecord.Months2st > num1 && num1 > 0)
              this.SetVal("1200", num1.ToString());
            else if (miRecord.Months2st >= 999 && num1 <= 0)
              this.SetVal("1200", "0");
            else
              this.SetVal("1200", miRecord.Months2st.ToString());
          }
          else
            this.SetVal("1200", "");
          this.SetVal("1205", miRecord.Cutoff == 0.0 ? "" : miRecord.Cutoff.ToString("N3"));
          double num2 = 0.0;
          if (this.loan.GetField("1757") == "")
          {
            switch (loanType)
            {
              case "VA":
              case "Conventional":
                this.SetVal("1757", "Loan Amount");
                break;
              case "FHA":
                this.SetVal("1757", "Base Loan Amount");
                break;
            }
          }
          switch (this.loan.GetField("1757"))
          {
            case "Loan Amount":
              num2 = Utils.ParseDouble((object) this.loan.GetField("2"));
              break;
            case "Purchase Price":
              num2 = Utils.ParseDouble((object) this.loan.GetField("136"));
              break;
            case "Appraisal Value":
              num2 = Utils.ParseDouble((object) this.loan.GetField("356"));
              break;
            case "Base Loan Amount":
              num2 = Utils.ParseDouble((object) this.loan.GetField("1109"));
              break;
          }
          double num3 = Utils.ArithmeticRounding(miRecord.Monthly1st / 100.0 * num2 / 12.0, 2);
          if (loanType == "FarmersHomeAdministration")
            this.SetVal("1766", num3 == 0.0 ? "" : num3.ToString("N3"));
          if (miRecord.Monthly1st == 0.0 || num2 == 0.0)
          {
            this.SetVal("232", "");
            this.SetVal("NEWHUD.X1707", "");
            this.SetVal("3971", "");
          }
          num3 = Utils.ArithmeticRounding(miRecord.Monthly2st / 100.0 * num2 / 12.0, 2);
          if (loanType == "FarmersHomeAdministration")
            this.SetVal("1770", num3 == 0.0 ? "" : num3.ToString("N3"));
          if (miRecord.Monthly1st == 0.0 || num2 == 0.0)
          {
            if (loanType == "FarmersHomeAdministration")
            {
              this.SetCurrentNum("NEWHUD.X1707", Utils.ParseDouble((object) this.loan.GetField("1766")));
              if (this.Val("232") != string.Empty)
                this.calObjs.USDACal.CopyPOCPTCAPRFromLine1003ToLine1010((string) null, (string) null);
              this.SetVal("232", "");
              this.calObjs.ULDDExpCal.CalcFannieMaeExportFields("232", this.Val("232"));
            }
            else
            {
              this.SetVal("NEWHUD.X1707", "");
              this.calObjs.ULDDExpCal.CalcFannieMaeExportFields("232", this.Val("232"));
            }
          }
          this.SetVal("1765", "");
          this.SetVal("3625", "");
          this.SetVal("3044", this.Val("1107"));
          this.SetVal("3047", this.Val("1760"));
        }
      }
    }

    private bool recordMatched(
      FieldFilter[] filters,
      string loanType,
      bool isStreamLine,
      LoanData tempLoan)
    {
      string str1 = "";
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      bool flag = tempLoan.GetField("3000") == "Y";
      for (int index = 0; index < filters.Length; ++index)
      {
        string id = filters[index].FieldID;
        if (((loanType == "FHA" ? 1 : (loanType == "VA" ? 1 : 0)) & (flag ? 1 : 0)) != 0 && id == "353")
          id = "MAX23K.X104";
        string simpleField = tempLoan.GetSimpleField(id);
        if ((loanType == "FHA" && id == "3042" && filters[index].OperatorType != OperatorTypes.EmptyDate || loanType == "FHA" & isStreamLine && id == "3432") && (simpleField == string.Empty || simpleField == "//"))
          simpleField = DateTime.Today.ToString("MM/dd/yyyy");
        string scriptCommands = filters[index].GetScriptCommands(simpleField);
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

    public void ExecuteTable_TitleEscrow(string tableName, string itemizationLineNumber)
    {
      Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_TitleEscrow is triggered. tableName: " + tableName + "; itemizationLineNumber: " + itemizationLineNumber);
      if (!this.UseNew2015GFEHUD && !this.UseNewGFEHUD)
        return;
      string str1 = this.Val("19");
      if (str1 == string.Empty || tableName == "" || itemizationLineNumber == "" || string.Compare("1102c", itemizationLineNumber, true) != 0 && itemizationLineNumber != "1103" && itemizationLineNumber != "1104")
        return;
      if (this.systemTables == null)
        this.systemTables = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (!this.systemTables.ContainsKey((object) "EscrowTitle"))
        this.systemTables.Add((object) "EscrowTitle", (object) this.sessionObjects.ConfigurationManager.GetSystemSettings(new string[4]
        {
          "TblEscrowPurList",
          "TblEscrowRefiList",
          "TblTitlePurList",
          "TblTitleRefiList"
        }));
      Hashtable systemTable = (Hashtable) this.systemTables[(object) "EscrowTitle"];
      if (systemTable == null || systemTable.Count == 0)
      {
        Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_TitleEscrow: No Escrow or Title record from setting");
      }
      else
      {
        itemizationLineNumber = itemizationLineNumber.ToLower();
        int key = str1.IndexOf("Refinance", StringComparison.Ordinal) <= -1 ? (itemizationLineNumber == "1102c" ? 0 : 2) : (itemizationLineNumber == "1102c" ? 1 : 3);
        if (!systemTable.ContainsKey((object) key))
        {
          Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_TitleEscrow: Cannot find table name " + tableName + " from setting");
        }
        else
        {
          TableFeeListBase tableFeeListBase = (TableFeeListBase) (BinaryObject) systemTable[(object) key];
          if (tableFeeListBase == null || tableFeeListBase.Count == 0)
          {
            Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_TitleEscrow: Cannot find fee list from Title or Escrow Table setting");
          }
          else
          {
            TableFeeListBase.FeeTable table = tableFeeListBase.GetTable(tableName);
            if (table == null)
              Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_TitleEscrow: Cannot find table name " + tableName + " from setting");
            else if (itemizationLineNumber == "1103" && table.FeeType != "Owner" || itemizationLineNumber == "1104" && table.FeeType != "Lender")
            {
              Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_TitleEscrow: Cannot find table name " + tableName + " with \"" + (itemizationLineNumber == "1103" ? "Owner" : "Lender") + "\" Type from setting");
            }
            else
            {
              switch (itemizationLineNumber)
              {
                case "1102c":
                  this.SetVal("ESCROW_TABLE", tableName);
                  break;
                case "1103":
                  this.SetVal("2010TITLE_TABLE", tableName);
                  break;
                case "1104":
                  this.SetVal("TITLE_TABLE", tableName);
                  break;
                default:
                  return;
              }
              double num = this.calObjs.ToolCal.CalcTitleEscrowRate(table);
              string str2;
              switch (itemizationLineNumber)
              {
                case "1102c":
                  str2 = "NEWHUD.X808";
                  break;
                case "1103":
                  str2 = "NEWHUD.X572";
                  break;
                default:
                  str2 = "NEWHUD.X639";
                  break;
              }
              string str3 = str2;
              if (Math.Abs(this.FltVal(str3) - num) >= 0.01)
              {
                this.SetVal(str3, num > 0.0 ? num.ToString("N2") : "");
                if (this.UseNew2015GFEHUD || this.UseNewGFEHUD)
                {
                  if (key == 0)
                  {
                    this.TriggerCalculation("NEWHUD.X808", num.ToString("N2"));
                    this.CalculationObjects.NewHudCalCopyHud2010ToGfe2010("NEWHUD.X645", (string) null, false);
                  }
                  else
                    this.CalculationObjects.NewHudCalCopyHud2010ToGfe2010(str3, (string) null, false);
                }
              }
              if (!this.UseNew2015GFEHUD)
                return;
              this.CalculationObjects.Calc_2015TitleFees(itemizationLineNumber, itemizationLineNumber);
            }
          }
        }
      }
    }

    public void ExecuteTable_CityStateTax(string tableName, string itemizationLineNumber)
    {
      Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_CityStateTax is triggered. tableName: " + tableName + "; itemizationLineNumber: " + itemizationLineNumber);
      if (itemizationLineNumber != "1204" && itemizationLineNumber != "1205" && itemizationLineNumber != "1206" && itemizationLineNumber != "1207" && itemizationLineNumber != "1208")
        Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_CityStateTax: Line Number is invalid. tableName: " + tableName + "; itemizationLineNumber: " + itemizationLineNumber);
      else if (tableName == "")
      {
        Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_CityStateTax: Table name cannot be blank");
      }
      else
      {
        if (this.systemTables == null)
          this.systemTables = CollectionsUtil.CreateCaseInsensitiveHashtable();
        if (!this.systemTables.ContainsKey((object) "CityStateTax"))
          this.systemTables.Add((object) "CityStateTax", (object) this.sessionObjects.ConfigurationManager.GetSystemSettings(new string[3]
          {
            "FeeCityList",
            "FeeStateList",
            "FeeUserList"
          }));
        Hashtable systemTable = (Hashtable) this.systemTables[(object) "CityStateTax"];
        if (systemTable == null || systemTable.Count == 0)
        {
          Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_CityStateTax: Cannot find setting from City/State/User tables.");
        }
        else
        {
          int num;
          switch (itemizationLineNumber)
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
          if (systemTable.ContainsKey((object) key))
            feeListBase = (FeeListBase) (BinaryObject) systemTable[(object) key];
          if (feeListBase == null || !feeListBase.TableNameExists(tableName))
          {
            Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_CityStateTax: Cannot find setting from City/State/User tables based on line number: " + itemizationLineNumber);
          }
          else
          {
            FeeListBase.FeeTable nonCaseSensitive = feeListBase.GetTableNonCaseSensitive(tableName);
            switch (key)
            {
              case 0:
                this.updateFee("1637", "647", "RE88395.X94", "593", nonCaseSensitive);
                this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1204((string) null, (string) null);
                break;
              case 1:
                this.updateFee("1638", "648", "RE88395.X89", "594", nonCaseSensitive);
                this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1205((string) null, (string) null);
                break;
              default:
                switch (itemizationLineNumber)
                {
                  case "1206":
                    this.updateFee("373", "374", "RE88395.X99", "576", nonCaseSensitive);
                    this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1206((string) null, (string) null);
                    break;
                  case "1207":
                    this.updateFee("1640", "1641", "RE88395.X100", "1642", nonCaseSensitive);
                    this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1207((string) null, (string) null);
                    break;
                  default:
                    this.updateFee("1643", "1644", "RE88395.X169", "1645", nonCaseSensitive);
                    this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1208((string) null, (string) null);
                    break;
                }
                break;
            }
            Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_CityStateTax: table value is applied.");
          }
        }
      }
    }

    private void updateFee(
      string feeNameId,
      string borFieldId,
      string caFieldId,
      string sellerId,
      FeeListBase.FeeTable fee)
    {
      double num1 = this.FltVal("2");
      double num2 = this.FltVal("136");
      double num3 = (fee.CalcBasedOn == "Loan Amount" ? num1 : num2) * (Utils.ParseDouble((object) fee.Rate) / 100.0) + Utils.ParseDouble((object) fee.Additional);
      this.SetCurrentNum(borFieldId, num3 - this.FltVal(sellerId));
      this.SetVal(caFieldId, num3.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.SetVal(feeNameId, fee.FeeName);
    }

    internal void ExecuteTable_AMI(bool showMessage, bool useYearUsed = false)
    {
      Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_AMI is triggered.");
      if (this.Val("5027") == "Y")
        return;
      AMILimit[] amiLimitRecords = this.GetAMILimitRecords(showMessage);
      if (amiLimitRecords == null || amiLimitRecords.Length == 0)
        return;
      AMILimit amiLimit = amiLimitRecords[0];
      int num = Utils.ParseInt((object) this.Val("4970"));
      bool flag = true;
      if (useYearUsed && num > 0 && num != amiLimit.LimitYear)
      {
        flag = false;
        for (int index = 1; index < amiLimitRecords.Length; ++index)
        {
          if (num == amiLimitRecords[index].LimitYear)
          {
            amiLimit = amiLimitRecords[index];
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        return;
      this.SetVal("MORNET.X30", amiLimit.AmiLimit100);
      this.SetVal("4971", amiLimit.AmiLimit80);
      this.SetVal("4972", amiLimit.AmiLimit50);
      this.SetVal("4970", amiLimit.LimitYear.ToString());
      this.calObjs.Cal.CalcAMILimit((string) null, (string) null);
    }

    internal AMILimit[] GetAMILimitRecords(bool showMessage)
    {
      Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "GetAMILimitRecords is triggered.");
      string str = this.Val("HMDA.X111");
      if (string.IsNullOrEmpty(str))
      {
        if (showMessage)
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "To retrieve the AMI data, you must first complete the zip code for the subject property.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "GetAMILimitRecords: No FIPS Code assigned.");
        return (AMILimit[]) null;
      }
      if (this.systemTables == null)
        this.systemTables = CollectionsUtil.CreateCaseInsensitiveHashtable();
      AMILimit[] amiLimitRecords;
      if (this.systemTables.ContainsKey((object) str))
      {
        amiLimitRecords = (AMILimit[]) this.systemTables[(object) str];
      }
      else
      {
        amiLimitRecords = this.sessionObjects.ConfigurationManager.GetAMILimits(str);
        this.systemTables.Add((object) str, (object) amiLimitRecords);
      }
      if (amiLimitRecords != null && amiLimitRecords.Length != 0)
        return amiLimitRecords;
      if (showMessage)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "The current loan information does not meet any criteria in AMI tables.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "GetAMILimitRecords: No AMI record from setting");
      return (AMILimit[]) null;
    }

    internal MFILimit[] GetMFILimitRecords(bool showMessage)
    {
      Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "GetMFILimitRecords is triggered.");
      string msaCode = this.Val("699");
      string subjectState = this.Val("14");
      if (string.IsNullOrEmpty(msaCode))
      {
        msaCode = "99999";
        if (string.IsNullOrEmpty(subjectState))
        {
          if (showMessage)
          {
            int num = (int) Utils.Dialog((IWin32Window) null, "To retrieve the MFI data, you must at least enter the State for the subject property.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_MFI: No MSA Code or subject property state assigned.");
          return (MFILimit[]) null;
        }
      }
      if (this.systemTables == null)
        this.systemTables = CollectionsUtil.CreateCaseInsensitiveHashtable();
      string key = "MFI" + msaCode + subjectState;
      MFILimit[] mfiLimitRecords;
      if (this.systemTables.ContainsKey((object) key))
      {
        mfiLimitRecords = (MFILimit[]) this.systemTables[(object) key];
      }
      else
      {
        mfiLimitRecords = this.sessionObjects.ConfigurationManager.GetMFILimits(msaCode, subjectState);
        this.systemTables.Add((object) key, (object) mfiLimitRecords);
      }
      if (mfiLimitRecords != null && mfiLimitRecords.Length != 0)
        return mfiLimitRecords;
      if (showMessage)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "No match is found in MFI table based on the subject property county and state.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_MFI: No MFI record matched from setting");
      return (MFILimit[]) null;
    }

    internal void ExecuteTable_MFI(bool showMessage, bool useYearUsed = false)
    {
      Tracing.Log(SystemTableCalculation.sw, TraceLevel.Info, nameof (SystemTableCalculation), "ExecuteTable_MFI is triggered.");
      if (this.Val("5028") == "Y")
        return;
      MFILimit[] mfiLimitRecords = this.GetMFILimitRecords(showMessage);
      if (mfiLimitRecords == null || mfiLimitRecords.Length == 0)
        return;
      MFILimit mfiLimit = mfiLimitRecords[0];
      int num1 = Utils.ParseInt((object) this.Val("5019"));
      bool flag = true;
      if (useYearUsed && num1 > 0 && num1 != mfiLimit.EstimatedMFIYear)
      {
        flag = false;
        for (int index = 1; index < mfiLimitRecords.Length; ++index)
        {
          if (num1 == mfiLimitRecords[index].EstimatedMFIYear)
          {
            mfiLimit = mfiLimitRecords[index];
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        return;
      this.SetVal("5017", mfiLimit.MSAMDName);
      int num2 = mfiLimit.ActualMFIYear;
      this.SetVal("5018", num2.ToString());
      num2 = mfiLimit.EstimatedMFIYear;
      this.SetVal("5019", num2.ToString());
      this.SetVal("5020", mfiLimit.ActualMFIAmount);
      this.SetVal("5021", mfiLimit.EstimatedMFIAmount);
    }
  }
}
