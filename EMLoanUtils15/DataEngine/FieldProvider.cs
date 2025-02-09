// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldProvider
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using Elli.AdvCode.Runtime;
using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Specialized;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class FieldProvider : IFieldProvider
  {
    public const string TBL_TYPE_DDM = "DDM�";
    public const string TBL_TYPE_ESCROWTITLE = "ESCROWTITLE�";
    public const string TBL_TYPE_TAX = "TAX�";
    public const string TBL_TYPE_MI = "MI�";
    private const string className = "DDM_FieldProvider�";
    private static readonly string sw = Tracing.SwDataEngine;
    private DDMFieldExecContext _fieldExecContext;
    private DDMFieldExecBatch _fieldExecBatch;
    private LoanData _loan;
    private readonly SessionObjects _sessionObject;
    private DDMDataTableTrigger _ddmDataTableManager;
    private DDMSystemTableEBSTrigger _ddmSystemTableEBSTrigger;
    private FeeManagementSetting _feeManagementSetting;
    private FeeManagementPersonaInfo _feeManagementPermission;
    private ILoanConfigurationInfo _loanConfigInfo;
    private BorrowerPair[] _borrowerPairs;
    private int _currentBorrowerPairIndex;
    private string updatedFieldIDsByDDM = "";
    private IDisclosureTracking2015Log[] leInitialLogs;

    public LoanData _Loan => this._loan;

    public ILoanConfigurationInfo _LoanConfigInfo => this._loanConfigInfo;

    public FieldProvider(
      LoanData loan,
      SessionObjects sessionObject,
      ILoanConfigurationInfo _loanConfigInfo,
      DDMDataTableTrigger ddmDataTableTrigger = null)
    {
      this._loan = loan;
      this._sessionObject = sessionObject;
      this._loanConfigInfo = _loanConfigInfo;
      this._ddmDataTableManager = ddmDataTableTrigger;
      this._ddmDataTableManager.Initialize();
      this._ddmSystemTableEBSTrigger = new DDMSystemTableEBSTrigger(sessionObject, (IFieldProvider) this);
      this._fieldExecContext = new DDMFieldExecContext();
      this._fieldExecBatch = new DDMFieldExecBatch(loan);
      this._feeManagementSetting = this._loanConfigInfo.FeeManagementList;
      this._feeManagementPermission = this._loanConfigInfo.FeeManagementPersonaPermission;
      this._borrowerPairs = this._loan.GetBorrowerPairs();
      this._currentBorrowerPairIndex = -1;
      this._loan.Calculator.FeeLinePaidToTrigger = CollectionsUtil.CreateCaseInsensitiveHashtable();
    }

    public void Reset(LoanData loan, ILoanConfigurationInfo _loanConfigInfo)
    {
      this._loan = loan;
      this._loanConfigInfo = _loanConfigInfo;
      this._fieldExecBatch = new DDMFieldExecBatch(this._loan);
      this._feeManagementSetting = this._loanConfigInfo.FeeManagementList;
      this._feeManagementPermission = this._loanConfigInfo.FeeManagementPersonaPermission;
      this._borrowerPairs = this._loan.GetBorrowerPairs();
      this._currentBorrowerPairIndex = -1;
      this._loan.Calculator.FeeLinePaidToTrigger = CollectionsUtil.CreateCaseInsensitiveHashtable();
    }

    public void InitializedBeforeExecution()
    {
      this.updatedFieldIDsByDDM = "";
      this._borrowerPairs = this._loan.GetBorrowerPairs();
      for (int index = 0; index < this._borrowerPairs.Length; ++index)
      {
        if (this._loan.CurrentBorrowerPair.Id == this._borrowerPairs[index].Id)
        {
          this._currentBorrowerPairIndex = index;
          break;
        }
      }
    }

    public object GetFieldValue(string fieldId)
    {
      FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(fieldId);
      string str = fieldId;
      fieldId = fieldPairInfo.FieldID;
      int pairIndex = this._borrowerPairs != null && this._borrowerPairs.Length == 1 || str.IndexOf("#") == -1 || str.StartsWith("#") ? -1 : (fieldPairInfo.PairIndex > 0 ? fieldPairInfo.PairIndex - 1 : 0);
      FieldFormat format;
      fieldId = this.removeSpecialCharFromFieldId(fieldId, out format);
      object fieldValue;
      if (fieldId.StartsWith("DDM:BYPASS_LE"))
        fieldValue = this.readLoanInformation(fieldId);
      else if (fieldId.StartsWith("DDM:MILESTONELOG:"))
        fieldValue = (object) this.applyCurrentRole(fieldId);
      else if (fieldId.StartsWith("DDM:FINISHEDMILESTONE:"))
        fieldValue = (object) this.applyLastMilestone(fieldId);
      else if (fieldId.StartsWith("DDM:FIELDSTATE:"))
        fieldValue = (object) this.getFieldStateConidtion(fieldId);
      else if (fieldId.StartsWith("DDM:USERTABLE:"))
        fieldValue = this.getDdmDataTableValue(fieldId);
      else if (fieldId.StartsWith("DDM:EFFECTIVEDATE:"))
        fieldValue = this.getEffectiveDateOfMilestone(fieldId);
      else if (fieldId.StartsWith("LOANDATAREQUEST:"))
      {
        fieldValue = this.getSpecialLoanData(fieldId);
      }
      else
      {
        if (pairIndex >= this._borrowerPairs.Length || fieldPairInfo.PairIndex > 1 && str.IndexOf("#") > -1 && fieldPairInfo.PairIndex > this._borrowerPairs.Length)
        {
          fieldValue = (object) "";
          Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Warning, "DDM The borrower pair index value exceeds the max pairs in the system: " + (object) pairIndex + ". DDM uses the primary pair instead.");
        }
        else
        {
          if (this._currentBorrowerPairIndex != pairIndex && pairIndex != -1)
            this._loan.SetBorrowerPair(pairIndex);
          fieldValue = (object) this._loan.GetSimpleField(fieldId);
          if (this._currentBorrowerPairIndex != pairIndex && pairIndex != -1)
            this._loan.SetBorrowerPair(this._currentBorrowerPairIndex);
        }
        switch (format)
        {
          case FieldFormat.INTEGER:
            fieldValue = (object) Utils.ParseInt(fieldValue, 0);
            break;
          case FieldFormat.DECIMAL_1:
          case FieldFormat.DECIMAL_2:
          case FieldFormat.DECIMAL_3:
          case FieldFormat.DECIMAL_4:
          case FieldFormat.DECIMAL_6:
          case FieldFormat.DECIMAL_5:
          case FieldFormat.DECIMAL_7:
          case FieldFormat.DECIMAL:
          case FieldFormat.DECIMAL_10:
            fieldValue = (object) Utils.ParseDecimal(fieldValue, 0.0M);
            break;
          case FieldFormat.DATE:
            fieldValue = (object) Utils.ParseDate(fieldValue).Date;
            if ((DateTime) fieldValue == DateTime.MinValue && !str.StartsWith("@"))
            {
              fieldValue = (object) "";
              break;
            }
            break;
          case FieldFormat.MONTHDAY:
            fieldValue = (object) Utils.ParseMonthDay(fieldValue);
            break;
          case FieldFormat.DATETIME:
            fieldValue = (object) Utils.ParseDate(fieldValue);
            if ((DateTime) fieldValue == DateTime.MinValue)
            {
              fieldValue = (object) "";
              break;
            }
            break;
          default:
            if (DDM_FieldAccess_Utils.FieldDefinitionOverrides.ContainsKey(fieldId) && DDM_FieldAccess_Utils.FieldDefinitionOverrides[fieldId] == FieldFormat.DATE)
            {
              fieldValue = (object) Utils.ParseDate(fieldValue).ToString("MM/dd/yyyy");
              break;
            }
            break;
        }
      }
      if (Tracing.Debug)
        Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Verbose, string.Format("DDM Field Provider - GetFieldValue FieldId = [{0}], [{1}], Return = [{2}]", (object) str, (object) fieldId, fieldValue == null ? (object) "Null" : (object) Convert.ToString(fieldValue)));
      return fieldValue;
    }

    public void SetFieldValue(string fieldId, object value)
    {
      if (fieldId.StartsWith("DDM:CONTEXT:"))
      {
        if (!DDMFieldExecContext.IsDDMDiagnosticOn())
          return;
        this.changeContext(fieldId, Convert.ToString(value));
      }
      else if (fieldId.StartsWith("DDM:EVENT:"))
      {
        if (!value.Equals((object) "OnExecute"))
          return;
        this._borrowerPairs = this._loan.GetBorrowerPairs();
        this._currentBorrowerPairIndex = -1;
      }
      else
      {
        FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(fieldId);
        string str1 = fieldId;
        fieldId = fieldPairInfo.FieldID;
        int pairIndex = fieldPairInfo.PairIndex > 0 ? fieldPairInfo.PairIndex - 1 : 0;
        if (pairIndex >= this._borrowerPairs.Length)
        {
          Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM The borrower pair index value exceeds the max pairs in the system: " + (object) pairIndex + ". DDM uses the primary pair instead.");
          pairIndex = -1;
        }
        FieldFormat format;
        fieldId = this.removeSpecialCharFromFieldId(fieldId, out format);
        if (value != null && value.ToString() != "" && format == FieldFormat.DATE && Utils.ParseDate((object) value.ToString()) == DateTime.MinValue)
          value = (object) "";
        if (Tracing.Debug)
          Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Verbose, string.Format("DDM Field Provider - SetFieldValue FieldId = [{0}], [{1}], Value = [{2}]", (object) str1, (object) fieldId, value == null ? (object) "Null" : (object) Convert.ToString(value)));
        SetFieldResult setField = new SetFieldResult();
        setField.Result = false;
        if (fieldId.ToString().StartsWith("DDM:USERTABLE:", StringComparison.InvariantCultureIgnoreCase))
          setField = this.applyDdmUserTableToField(fieldId, value);
        else if (fieldId.ToString().StartsWith("DDM:SYSTEMTABLE:", StringComparison.InvariantCultureIgnoreCase))
        {
          this._fieldExecContext.CommandType = DDMFieldExecCmdType.Table;
          this._fieldExecContext.CommandDetail = fieldId + " " + Convert.ToString(value);
          setField = this.applySystemTableToFields(fieldId, value);
        }
        else if (fieldId.ToString().StartsWith("DDM:FEEMANAGEMENT:", StringComparison.InvariantCultureIgnoreCase))
        {
          this._fieldExecContext.CommandType = DDMFieldExecCmdType.FeeManagement;
          this._fieldExecContext.CommandDetail = fieldId + " " + Convert.ToString(value);
          setField = this.applyFeeManagementValue(fieldId, value);
        }
        else if (fieldId.ToString().StartsWith("DDM:USECALCVALUE:", StringComparison.InvariantCultureIgnoreCase))
          setField = this.applyUseCalculatedValue(fieldId, value);
        else if (value != null)
        {
          string simpleField = this._loan.GetSimpleField(fieldId);
          string str2 = Convert.ToString(value);
          setField.Init(fieldId, simpleField, str2);
          int num = -1;
          if (this._currentBorrowerPairIndex != pairIndex && pairIndex != -1)
          {
            num = this._loan.GetPairIndex(this._loan.CurrentBorrowerPair.Id);
            this._loan.SetBorrowerPair(pairIndex);
            this._currentBorrowerPairIndex = pairIndex;
          }
          if (pairIndex != -1 && !this._loan.Calculator.AddFeePaidToNameToCache(fieldId, simpleField, str2))
          {
            if (DDM_FieldAccess_Utils.IsFieldLockIconField(fieldId))
            {
              setField.LockIconActivated = true;
              this._loan.AddLock(fieldId);
            }
            if (DDM_FieldAccess_Utils.IsAmountFieldLockable(fieldId) && this._loan.IsLocked(DDM_FieldAccess_Utils.GetLockableAmountFieldID(fieldId)))
              this._loan.RemoveLock(DDM_FieldAccess_Utils.GetLockableAmountFieldID(fieldId));
            try
            {
              this._loan.SetCurrentField(fieldId, str2, false);
              this._fieldExecBatch.ExecBatchForField(fieldId);
              this.addUpdatedFieldID(fieldId);
            }
            catch (Exception ex)
            {
              Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM Rule cannot set value '" + str2 + "' to field '" + fieldId + "' due to error: " + ex.Message);
            }
          }
          if (num > -1)
          {
            this._currentBorrowerPairIndex = num;
            this._loan.SetBorrowerPair(this._currentBorrowerPairIndex);
          }
        }
        else
          setField.Init(fieldId, this._loan.GetSimpleField(fieldId), "NOT SET BY NULL");
        if (!setField.Result)
          return;
        this._fieldExecContext.LogDetail(setField);
      }
    }

    public string UpdatedFieldIDsByDDM
    {
      get => this.updatedFieldIDsByDDM;
      set => this.updatedFieldIDsByDDM = value;
    }

    private void addUpdatedFieldID(string fieldId)
    {
      if (this.updatedFieldIDsByDDM.IndexOf("|" + fieldId + "|") != -1)
        return;
      this.updatedFieldIDsByDDM = this.updatedFieldIDsByDDM + "|" + fieldId.ToUpper() + "|";
    }

    public bool IsField(string fieldId) => true;

    private void changeContext(string cntxtCmd, string cntxtValue)
    {
      string[] strArray1 = cntxtCmd.Split(new char[1]{ ':' }, StringSplitOptions.RemoveEmptyEntries);
      string[] strArray2 = cntxtValue.Split('|');
      if (strArray1.Length < 3)
      {
        this._fieldExecContext.RuleName = strArray2[1];
        this._fieldExecContext.ScenarioName = strArray2[2];
        this._fieldExecContext.RuleType = strArray2[0].Equals("FIELD") ? DDMFeildExecRuleType.FieldRule : DDMFeildExecRuleType.FeeRule;
      }
      else
      {
        switch (strArray1[2])
        {
          case "ON":
            this._fieldExecContext.IsLogOn = cntxtValue == "Y";
            break;
          case "CALCULATION":
            this._fieldExecContext.CommandType = DDMFieldExecCmdType.Calculation;
            break;
          case "TABLE":
            this._fieldExecContext.CommandType = DDMFieldExecCmdType.Table;
            break;
          default:
            this._fieldExecContext.CommandType = DDMFieldExecCmdType.Regular;
            break;
        }
        this._fieldExecContext.CommandDetail = cntxtValue;
      }
    }

    private object readLoanInformation(string fieldId)
    {
      if (!(fieldId == "DDM:BYPASS_LE"))
        return (object) "";
      DateTime result;
      return DateTime.TryParse(this._loan.GetField("3152"), out result) && DateTime.Now.Date >= result.Date ? (object) true : (object) false;
    }

    private object getVirtualFieldValue(string fieldId)
    {
      try
      {
        VirtualField field = VirtualFields.GetField(fieldId);
        if (field == null)
          return (object) null;
        string virtualFieldValue = field.GetValue(this._loan);
        if (Tracing.Debug)
          Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Verbose, string.Format("DDM Field Provider - Get VirtualField FieldId = [{0}], Return = [{1}]", (object) fieldId, virtualFieldValue == null ? (object) "Null" : (object) Convert.ToString(virtualFieldValue)));
        return (object) virtualFieldValue;
      }
      catch (Exception ex)
      {
        Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM - Exception while getting virtual field ID = " + fieldId + ". Error: " + ex.Message);
        return (object) null;
      }
    }

    private void setVirtualFieldValue(string fieldId, object value)
    {
      Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM - Cannot assign a value to virtual field = " + fieldId + ", Value = " + value != null ? Convert.ToString(value) : "Null");
    }

    private object getDdmDataTableValue(string fieldId)
    {
      string[] strArray1 = fieldId.Split(':');
      if (strArray1 != null)
      {
        if (strArray1.Length >= 3)
        {
          try
          {
            string[] strArray2 = strArray1[2].Split('|');
            string ddmTableName = strArray2[1];
            int int32 = strArray2.Length > 3 ? Convert.ToInt32(strArray2[3]) : 0;
            object ddmDataTableValue = this._ddmDataTableManager.GetValue(ddmTableName, (IFieldProvider) this, int32);
            if (Tracing.Debug)
              Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Verbose, string.Format("DDM Field Provider - Get DDM User Table value for table = [{0}], Return = [{1}]", (object) ddmTableName, ddmDataTableValue == null ? (object) "Null" : (object) Convert.ToString(ddmDataTableValue)));
            return ddmDataTableValue;
          }
          catch (Exception ex)
          {
            Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM - The DataTable information is not valid format : " + fieldId);
            return (object) null;
          }
        }
      }
      Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM - The DataTable information is invalid : " + fieldId);
      return (object) null;
    }

    private object getEffectiveDateOfMilestone(string fieldId)
    {
      string[] strArray = fieldId.Split(':');
      if (strArray != null && strArray.Length >= 3)
      {
        string str = strArray[2];
        return (object) this.checkMilestoneStatus(strArray[3]);
      }
      Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM - The DataTable information is invalid : " + fieldId);
      return (object) "N";
    }

    private SetFieldResult applySystemTableToFields(string fieldId, object nameOfSystemTable)
    {
      SetFieldResult fields = new SetFieldResult();
      string[] strArray = Convert.ToString(nameOfSystemTable).Split('|');
      string str1 = strArray.Length != 0 ? strArray[0] : (string) null;
      string str2 = strArray.Length > 1 ? strArray[1] : (string) null;
      string str3 = strArray.Length > 2 ? strArray[2] : (string) null;
      switch (str1)
      {
        case "DDM":
          throw new Exception("DDM table definition was found in the system table execution : " + str1);
        case "ESCROWTITLE":
        case "TAX":
        case "MI":
          string str4 = "GET" + str1;
          if (!string.IsNullOrEmpty(str4))
          {
            string calculationID = str4.Equals("GETMI") ? str4 : string.Format("{0};{1};{2}", (object) str4, (object) str2, (object) str3);
            this._loan.Calculator.FormCalculation(calculationID);
            fields.Init(fieldId, "[N/A]", "SYSTEM_TABLE:" + calculationID);
            this.addUpdatedFieldID(fieldId);
          }
          else
          {
            Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM - The system table information is invalid : " + nameOfSystemTable);
            fields.Result = false;
          }
          return fields;
        default:
          throw new Exception("Unknown system table calc-name was found in the system table execution : " + str1);
      }
    }

    private SetFieldResult applyUseCalculatedValue(string formattedFieldId, object value)
    {
      SetFieldResult setFieldResult = new SetFieldResult();
      string[] strArray = Convert.ToString(formattedFieldId).Split(':');
      if (strArray.Length > 2)
      {
        string str = strArray.Length != 0 ? strArray[2] : (string) null;
        if (str == null)
        {
          Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM - required field id is missing in DDM:USECALCVALUE: - " + formattedFieldId);
          setFieldResult.Result = false;
          return setFieldResult;
        }
        if (DDM_FieldAccess_Utils.IsFieldLockIconField(str))
        {
          setFieldResult.LockIconActivated = false;
          if (this._loan.IsLocked(str))
          {
            this._loan.RemoveLock(str);
            this._loan.SetCurrentField(str, "", false);
            this._loan.Calculator.FormCalculation(str);
          }
        }
        setFieldResult.Init(str, "[N/A]", "USE_CALCULATED_VALUE");
      }
      else
      {
        Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM - Invalid format detected in DDM:USECALCVALUE handling : " + formattedFieldId);
        setFieldResult.Result = false;
      }
      return setFieldResult;
    }

    private SetFieldResult applyFeeManagementValue(string formattedFieldId, object value)
    {
      SetFieldResult setFieldResult = new SetFieldResult();
      string[] strArray = Convert.ToString(formattedFieldId).Split(':');
      if (strArray.Length > 2)
      {
        string str = strArray.Length != 0 ? strArray[2] : (string) null;
        if (str == null)
        {
          Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM - required field id is missing in DDM:FEEMANAGMENT: - " + formattedFieldId);
          setFieldResult.Result = false;
          return setFieldResult;
        }
        bool companyOptIn = this._feeManagementSetting.CompanyOptIn;
        FeeSectionEnum fieldSectionEnum = DDM_FieldAccess_Utils.GetFeeManagementFieldSectionEnum(str);
        bool flag = this._feeManagementPermission != null && this._feeManagementPermission.IsSectionEditable(fieldSectionEnum);
        if (companyOptIn & flag)
          this._loan.SetCurrentField(str, Convert.ToString(value));
        else
          this._loan.SetCurrentField(str, Convert.ToString(value));
        setFieldResult.Init(str, this._loan.GetSimpleField(str), Convert.ToString(value));
      }
      else
      {
        Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Error, "DDM - Invalid format detected in DDM:FEEMANAGMENT handling : " + formattedFieldId);
        setFieldResult.Result = false;
      }
      return setFieldResult;
    }

    private SetFieldResult applyDdmUserTableToField(string fieldId, object valueWithDdmTableTag)
    {
      SetFieldResult field = new SetFieldResult();
      string[] strArray = ((string) valueWithDdmTableTag).Split(':');
      if (strArray != null && strArray.Length >= 3)
      {
        string ddmTableName = strArray[2];
        object obj = this._ddmDataTableManager.GetValue(ddmTableName, (IFieldProvider) this);
        if (Tracing.Debug)
          Tracing.Log(FieldProvider.sw, "DDM_FieldProvider", TraceLevel.Verbose, string.Format("DDM Field Provider - Set DDM User Table value [{0}] from [{1}]", obj == null ? (object) "Null" : (object) Convert.ToString(obj), (object) ddmTableName));
        string str = obj != null ? obj.ToString() : "";
        this._loan.SetCurrentField(fieldId, str);
        field.Init(fieldId, this._loan.GetSimpleField(fieldId), str);
      }
      else
        field.Result = false;
      return field;
    }

    private string applyCurrentRole(string fieldId)
    {
      MilestoneLog[] allMilestones = this._loan.GetLogList().GetAllMilestones();
      string[] strArray = fieldId.Split(':');
      if (strArray == null || strArray.Length < 4)
        return "N";
      string str1 = strArray[2];
      string str2 = strArray[3];
      for (int index = 0; index < allMilestones.Length - 1; ++index)
      {
        if (allMilestones[index].MilestoneID == str1)
        {
          if (index > 0 && allMilestones[index].Done && allMilestones[index].RoleID.ToString() == str2 && (allMilestones[index].LoanAssociateID ?? "") != "")
            return "Y";
          break;
        }
      }
      return "N";
    }

    private string applyLastMilestone(string fieldId)
    {
      string[] strArray = fieldId.Split(':');
      return strArray == null || strArray.Length < 3 ? "N" : this.checkMilestoneStatus(strArray[2]);
    }

    private string checkMilestoneStatus(string milestoneId)
    {
      MilestoneLog milestoneById = this._loan.GetLogList().GetMilestoneByID(milestoneId);
      string str = "Started";
      if (milestoneById == null)
        return "N";
      if (milestoneById.Done)
      {
        if (Utils.IsInt((object) milestoneById.MilestoneID))
        {
          switch (milestoneById.MilestoneID)
          {
            case "1":
              str = "File started";
              break;
            case "2":
              str = "Sent to processing";
              break;
            case "3":
              str = "Submitted";
              break;
            case "4":
              str = "Approved";
              break;
            case "5":
              str = "Doc signed";
              break;
            case "6":
              str = "Funded";
              break;
            case "7":
              str = "Completed";
              break;
          }
        }
        else
          str = milestoneById.DoneText;
      }
      return !(str == (string) this.GetFieldValue("MS.STATUS")) ? "N" : "Y";
    }

    public IDisclosureTracking2015Log[] LEInitialLogs
    {
      get
      {
        if (this.leInitialLogs == null)
          this.leInitialLogs = this._loan.GetLogList().GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.LE);
        return this.leInitialLogs;
      }
      set => this.leInitialLogs = value;
    }

    private string getFieldStateConidtion(string fieldId)
    {
      string[] strArray = fieldId.Split(':');
      if (strArray == null || strArray.Length < 4)
        return "N";
      string id = strArray[2];
      string str = strArray[3];
      string simpleField = this._loan.GetSimpleField(id);
      return !(Utils.UnformatValue(simpleField, this._loan.GetFormat(id)) == "") && !simpleField.Equals("N") ? "Y" : "N";
    }

    private string removeSpecialCharFromFieldId(string fieldId, out FieldFormat format)
    {
      format = FieldFormat.UNDEFINED;
      if (fieldId.StartsWith("#") && fieldId.EndsWith("#"))
      {
        format = FieldFormat.DECIMAL;
        return fieldId.Substring(1, fieldId.Length - 2);
      }
      if (fieldId.StartsWith("#"))
      {
        format = FieldFormat.DECIMAL;
        return fieldId.Substring(1);
      }
      if (fieldId.StartsWith("@"))
      {
        format = FieldFormat.DATE;
        return fieldId.Substring(1);
      }
      if (!fieldId.StartsWith("+"))
      {
        if (!fieldId.StartsWith("-"))
        {
          try
          {
            FieldDefinition fieldDefinition = this._loan.GetFieldDefinition(fieldId);
            if (fieldDefinition != null)
              format = fieldDefinition.Format;
          }
          catch (Exception ex)
          {
          }
          return fieldId;
        }
      }
      return fieldId.Substring(1);
    }

    private void initializeFeeManagementAssets()
    {
      this._loanConfigInfo = this._sessionObject.LoanManager.GetLoanConfigurationInfo();
      this._feeManagementSetting = this._loanConfigInfo.FeeManagementList;
      this._feeManagementPermission = this._loanConfigInfo.FeeManagementPersonaPermission;
    }

    private object getSpecialLoanData(string specialID)
    {
      if (specialID == "LOANDATAREQUEST:GetInitialDisclosureTrackingLogsByType")
      {
        IDisclosureTracking2015Log[] tracking2015LogsByType = this._loan.GetLogList().GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.LE);
        return (object) (bool) (tracking2015LogsByType == null ? 0 : (tracking2015LogsByType.Length != 0 ? 1 : 0));
      }
      if (!specialID.StartsWith("LOANDATAREQUEST:GetMilestoneStatus"))
        return (object) null;
      string[] strArray = specialID.Split(':');
      return strArray != null && strArray.Length == 3 ? (object) this.getMilestoneStatus(strArray[2]) : (object) false;
    }

    private bool getMilestoneStatus(string milestoneIds)
    {
      string str1 = milestoneIds;
      char[] chArray = new char[1]{ ';' };
      foreach (string stage in str1.Split(chArray))
      {
        MilestoneLog milestoneByName = this._loan.GetLogList().GetMilestoneByName(stage);
        string str2 = "Started";
        if (milestoneByName != null)
        {
          if (milestoneByName.Done)
          {
            if (Utils.IsInt((object) milestoneByName.MilestoneID))
            {
              switch (milestoneByName.MilestoneID)
              {
                case "1":
                  str2 = "File started";
                  break;
                case "2":
                  str2 = "Sent to processing";
                  break;
                case "3":
                  str2 = "Submitted";
                  break;
                case "4":
                  str2 = "Approved";
                  break;
                case "5":
                  str2 = "Doc signed";
                  break;
                case "6":
                  str2 = "Funded";
                  break;
                case "7":
                  str2 = "Completed";
                  break;
              }
            }
            else
              str2 = milestoneByName.DoneText;
          }
          if (str2 == this._loan.GetField("MS.STATUS"))
            return true;
        }
      }
      return false;
    }
  }
}
