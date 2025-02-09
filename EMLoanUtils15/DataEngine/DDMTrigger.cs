// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMTrigger
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using Elli.AdvCode.Parser.Scripts;
using Elli.AdvCode.Runtime;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DDMTrigger
  {
    private const string className = "DDMTrigger�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    private string loanGUID;
    private IFieldProvider fieldProvider;
    private Compiler compiler;
    private CompiledScript compiledScriptForFeeRule;
    private string theAdvanceCodeForFeeRule;
    private CompiledScript compiledScriptForFieldRule;
    private string theAdvanceCodeForFieldRule;
    private DDMDataPopulationTiming ddmPopulationTiming;
    private HashSet<string> ddmPopulationFieldList;
    private CompiledScript compiledDdmStopCode;
    private string[] virtualFields;
    private bool allowDDMExecution;
    private IDataCache ebsV3DdmCache;
    private string ddmOnDemandTriggerFields;
    private Dictionary<string, string> ddmOnDemandVirtualFields;
    private List<string> ddmOnDemandDataTableNames;
    private DateTime ddmLastModifiedDateTime = DateTime.MinValue;
    private DDMHost ddmHost;
    private LoanData newlyOpenedLoan;
    private ILoanConfigurationInfo newlyOpenedLoanConfigInfo;
    private string errorMessage = "";
    private DDMTrigger.EbsPerformanceAttributes _ebsPerformanceAttributes;

    public string LoanGUID => this.loanGUID;

    public IFieldProvider FieldProvider
    {
      get => this.fieldProvider;
      set => this.fieldProvider = value;
    }

    public string DDMOnDemandTriggerFields
    {
      set => this.ddmOnDemandTriggerFields = value;
      get => this.ddmOnDemandTriggerFields;
    }

    public Dictionary<string, string> DDMOnDemandVirtualFields => this.ddmOnDemandVirtualFields;

    public List<string> DDMOnDemandDataTableNames => this.ddmOnDemandDataTableNames;

    public DateTime DDMLastModifiedDateTime
    {
      set => this.ddmLastModifiedDateTime = value;
      get => this.ddmLastModifiedDateTime;
    }

    public Task DdmInitializationTask { get; private set; }

    public DDMTrigger(
      SessionObjects sessionObjects,
      string loanGUID,
      IFieldProvider fieldProvider,
      bool loadActivationField,
      DDMTrigger.EbsPerformanceAttributes ebsAttributes = null,
      IDataCache ebsV3DdmCache = null)
    {
      this.sessionObjects = sessionObjects;
      this.fieldProvider = fieldProvider;
      this.loanGUID = loanGUID;
      if (ebsAttributes != null)
        this.EbsAttributes = ebsAttributes;
      this.ebsV3DdmCache = ebsV3DdmCache == null ? (IDataCache) new DisabledEbsV3Cache() : ebsV3DdmCache;
      this.allowDDMExecution = this.sessionObjects.StartupInfo.AllowDDM;
      this.ddmHost = new DDMHost(this.sessionObjects);
      Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Info, "DDM CLIENT.ALLOWDDM Setting = [" + (this.allowDDMExecution ? "True" : "False") + "]");
      if (!this.allowDDMExecution)
        return;
      Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Info, "DDM Performance Enable Setting = [" + (this.sessionObjects.StartupInfo.EnableDDMPerformance ? "True" : "False") + "]");
      try
      {
        if (loadActivationField)
        {
          DDMAffectedFieldsandDataTableNames fieldsandDataTableNames1;
          if (!this.sessionObjects.StartupInfo.EnableDDMPerformance)
            fieldsandDataTableNames1 = (DDMAffectedFieldsandDataTableNames) null;
          else
            fieldsandDataTableNames1 = this.sessionObjects.ConfigurationManager.GetFieldsAndDataTableNamesUsedInRules(new FieldSearchRuleType[3]
            {
              FieldSearchRuleType.DDMFeeScenarios,
              FieldSearchRuleType.DDMFieldScenarios,
              FieldSearchRuleType.DDMDataTables
            });
          DDMAffectedFieldsandDataTableNames fieldsandDataTableNames2 = fieldsandDataTableNames1;
          if (fieldsandDataTableNames2 != null)
          {
            this.ddmOnDemandTriggerFields = fieldsandDataTableNames2.fields;
            this.ddmOnDemandDataTableNames = fieldsandDataTableNames2.dataTableNames;
          }
          if (this.ddmOnDemandTriggerFields != null && this.ddmOnDemandTriggerFields.IndexOf("VF|") > -1)
            this.virtualFields = this.ddmOnDemandTriggerFields.Substring(this.ddmOnDemandTriggerFields.IndexOf("VF|") + 2).Split('|');
          this.BuildVirtualFieldComparisonTable();
          Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Info, "DDM Number of Activation Fields = [" + (this.ddmOnDemandTriggerFields != null ? string.Concat((object) this.ddmOnDemandTriggerFields.Length) : "0") + "]");
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Error, "DDM - DDMTrigger Error: cannot retrieve field IDs used in DDM setting. " + ex.Message);
      }
      this.DdmInitializationTask = Task.Run((Action) (() => this.FetchAndCompileDDMRules()));
    }

    public void FetchAndCompileDDMRules()
    {
      this.compiler = new Compiler();
      this.ddmPopulationFieldList = new HashSet<string>();
      try
      {
        DDMRuleCodeGenerator instance = DDMRuleCodeGenerator.Instance;
        DDMFieldExecContext fieldExecContext = DDMFieldExecContext.StartTimer();
        this.getDdmPopulationTimingSetting();
        fieldExecContext.LogPerf("Retrieving DDM Population Timing Setting");
        this.compiledScriptForFeeRule = this.ebsV3DdmCache.Get<CompiledScript>("CompiledScriptForFeeRule", (Func<CompiledScript>) (() => this.compileAdvancedCodeForFeeRule()));
        this.compiledScriptForFieldRule = this.ebsV3DdmCache.Get<CompiledScript>("CompiledScriptForFieldRule", (Func<CompiledScript>) (() => this.compileAdvancedCodeForFieldRule()));
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Error, "DDM - Cannot build advance code from DDM setting. Error: " + ex.Message);
        this.errorMessage = ex.Message;
      }
    }

    public void Reset(string loanGUID, IFieldProvider fieldProvider)
    {
      this.loanGUID = loanGUID;
      this.fieldProvider = fieldProvider;
      this.BuildVirtualFieldComparisonTable();
    }

    public void BuildVirtualFieldComparisonTable()
    {
      if (this.virtualFields == null || this.virtualFields.Length == 0)
        return;
      this.ddmOnDemandVirtualFields = new Dictionary<string, string>();
      if (this.virtualFields == null || this.virtualFields.Length == 0)
        return;
      for (int index = 0; index < this.virtualFields.Length; ++index)
      {
        if (this.virtualFields[index] != "")
          this.ddmOnDemandVirtualFields.Add(this.virtualFields[index], this.fieldProvider.GetFieldValue(this.virtualFields[index]).ToString());
      }
    }

    public bool Execute(DDMStartPopulationTrigger startPopulationType = DDMStartPopulationTrigger.All, string fieldId = null)
    {
      if (!this.allowDDMExecution)
      {
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Info, "CLIENT.ALLOWDDM setting is turned off in company_settings.");
        return false;
      }
      try
      {
        this.DdmInitializationTask.Wait();
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Error, "DDM - Cannot initialize DDM Trigger. Error: " + ex.Message);
        return false;
      }
      this.errorMessage = "";
      bool flag = false;
      if (!this.canPopulateDDMTrigger(startPopulationType, fieldId))
        return false;
      DDMFieldExecContext fieldExecContext1 = DDMFieldExecContext.StartTimer();
      try
      {
        if (this.fieldProvider != null)
        {
          this.fieldProvider.SetFieldValue("DDM:EVENT:", (object) "OnExecute");
          if (this.fieldProvider is EllieMae.EMLite.DataEngine.FieldProvider)
          {
            try
            {
              ((EllieMae.EMLite.DataEngine.FieldProvider) this.fieldProvider).InitializedBeforeExecution();
            }
            catch (Exception ex)
            {
            }
          }
        }
        DDMFieldExecContext fieldExecContext2 = DDMFieldExecContext.StartTimer();
        DDMFieldExecContext.LogHeader(this.loanGUID, this.sessionObjects.UserID);
        if (this.EbsAttributes.EbsPerformanceOptimization)
          this.UpdateFieldRuleAdvCode();
        if (this.compiledScriptForFieldRule != null)
        {
          DDMFieldExecContext.LogHeader("EXECUTING FIELD RULES");
          DDMFieldExecContext fieldExecContext3 = DDMFieldExecContext.StartTimer();
          RuntimeContext context = new RuntimeContext((object) this.ddmHost, this.fieldProvider);
          context.Configuration.AllowedTypes.Allow(typeof (CalendarDataSource));
          flag = this.compiledScriptForFieldRule.Execute(context);
          flag = flag && context.RuntimeException == null;
          fieldExecContext3.LogPerf("Executing Field Rule Advanced Code");
        }
        if (this.compiledScriptForFeeRule != null)
        {
          DDMFieldExecContext.LogHeader("EXECUTING FEE RULES");
          DDMFieldExecContext fieldExecContext4 = DDMFieldExecContext.StartTimer();
          RuntimeContext context = new RuntimeContext((object) this.ddmHost, this.fieldProvider);
          context.Configuration.AllowedTypes.Allow(typeof (CalendarDataSource));
          flag = this.compiledScriptForFeeRule.Execute(context);
          flag = flag && context.RuntimeException == null;
          fieldExecContext4.LogPerf("Executing Fee Rule Advanced Code");
        }
        fieldExecContext2.LogPerf("Executing All Rules Advanced Code");
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Error, "DDM - Cannot trigger DDM Rules. Error: " + ex.Message);
        this.errorMessage = ex.Message;
      }
      fieldExecContext1.LogPerf("Total DDM Execution Time");
      this.fieldProvider.SetFieldValue("DMDDM.X1", (object) this.sessionObjects.Session.ServerTime.ToString("MM/dd/yyyy hh:mm:ss tt"));
      return flag;
    }

    private CompiledScript compileAdvancedCodeForFeeRule()
    {
      try
      {
        CompiledScript compiledScript = (CompiledScript) null;
        DDMFieldExecContext fieldExecContext = DDMFieldExecContext.StartTimer();
        this.buildAdvanceCodeForFeeRule();
        fieldExecContext.LogPerf("Building Advanced Code for FeeRule");
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Info, "DDM Fee Rule Advanced Code - " + this.theAdvanceCodeForFeeRule);
        if (!string.IsNullOrEmpty(this.theAdvanceCodeForFeeRule))
        {
          compiledScript = this.compiler.Compile(this.theAdvanceCodeForFeeRule);
          this.theAdvanceCodeForFeeRule = (string) null;
        }
        fieldExecContext.LogPerf("Compiling Advanced Code for FeeRule");
        return compiledScript;
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Error, "DDM - Cannot compile DDM Advance code for FeeRule. Error: " + ex.Message);
        this.errorMessage = ex.Message;
        return (CompiledScript) null;
      }
    }

    private CompiledScript compileAdvancedCodeForFieldRule()
    {
      try
      {
        CompiledScript compiledScript = (CompiledScript) null;
        DDMFieldExecContext fieldExecContext = DDMFieldExecContext.StartTimer();
        this.buildAdvanceCodeForFieldRule();
        fieldExecContext.LogPerf("Building Advanced Code for FieldRule");
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Info, "DDM Field Rule Advanced Code - " + this.theAdvanceCodeForFieldRule);
        if (!string.IsNullOrEmpty(this.theAdvanceCodeForFieldRule))
        {
          compiledScript = this.compiler.Compile(this.theAdvanceCodeForFieldRule);
          this.theAdvanceCodeForFieldRule = (string) null;
        }
        fieldExecContext.LogPerf("Compiling Advanced Code for FieldRule");
        return compiledScript;
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Error, "DDM - Cannot compile DDM Advance code for FieldRule. Error: " + ex.Message);
        this.errorMessage = ex.Message;
        return (CompiledScript) null;
      }
    }

    private void getDdmPopulationTimingSetting()
    {
      try
      {
        this.ddmPopulationTiming = (DDMDataPopulationTiming) null;
        this.ddmPopulationTiming = this.sessionObjects.BpmManager.GetDDMDataPopulationTiming();
        if (this.ddmPopulationTiming == null)
          return;
        if (this.ddmPopulationTiming.FieldList.Count > 0)
        {
          foreach (DDMDataPopTimingField field in this.ddmPopulationTiming.FieldList)
          {
            if (!this.ddmPopulationFieldList.Contains(field.FieldID))
              this.ddmPopulationFieldList.Add(field.FieldID);
          }
        }
        if (string.IsNullOrEmpty(this.ddmPopulationTiming.LoanCondMetCond))
          return;
        this.compiledDdmStopCode = this.ebsV3DdmCache.Get<CompiledScript>("CompiledDdmStopCode", (Func<CompiledScript>) (() => this.compiler.CompileExpression(new ScriptBuilder().ParseExpression(this.ddmPopulationTiming.LoanCondMetCond))));
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Error, "DDM - Cannot load DDM Data Population Timing Setting. Error: " + ex.Message);
        this.errorMessage = ex.Message;
      }
    }

    private void buildAdvanceCodeForFeeRule()
    {
      this.theAdvanceCodeForFeeRule = "";
      this.compiledScriptForFeeRule = (CompiledScript) null;
      try
      {
        List<string> colVmFeeRules = new List<string>();
        DDMFeeRule[] ddmFeeRuleArray = !this.sessionObjects.FastLoanLoad || this.sessionObjects.DDMFeeRules == null || ((IEnumerable<DDMFeeRule>) this.sessionObjects.DDMFeeRules).Count<DDMFeeRule>() <= 0 ? this.sessionObjects.BpmManager.GetAllDDMFeeRulesAndScenarios(true) : this.sessionObjects.DDMFeeRules;
        if (ddmFeeRuleArray != null)
        {
          if (ddmFeeRuleArray.Length != 0)
          {
            for (int index = 0; index < ddmFeeRuleArray.Length; ++index)
            {
              DDMVmFeeRule ddmVmFeeRule = DDMViewModelConverter.ConvertDDMFeeRule(ddmFeeRuleArray[index]);
              colVmFeeRules.Add(DDMRuleCodeGenerator.Instance.GetAdvancedCode(ddmVmFeeRule));
            }
            this.theAdvanceCodeForFeeRule = DDMRuleCodeGenerator.Instance.GetAdvancedCode(colVmFeeRules);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Error, "DDM - Cannot load DDM Rules. Error: " + ex.Message);
        this.errorMessage = ex.Message;
      }
      if (!DDMFieldExecContext.IsDDMDiagnosticOn())
        return;
      Tracing.Log(true, "DDM_DIAGNOTICS_DETAIL", nameof (DDMTrigger), "Generated AdvCode " + (this.theAdvanceCodeForFeeRule == null ? "empty code" : this.theAdvanceCodeForFeeRule));
    }

    private void buildAdvanceCodeForFieldRule()
    {
      this.theAdvanceCodeForFieldRule = "";
      this.compiledScriptForFieldRule = (CompiledScript) null;
      try
      {
        List<string> colVmFeeRules = new List<string>();
        DDMFieldRule[] ddmFieldRuleArray = this.EbsAttributes.EbsPerformanceOptimization ? this.sessionObjects.BpmManager.GetAllDDMFieldRulesAndScenarios(true, this.EbsAttributes.GetFieldRulesIdsToBuildAdvancedCode()) : (!this.sessionObjects.FastLoanLoad || this.sessionObjects.DDMFieldRules == null || ((IEnumerable<DDMFieldRule>) this.sessionObjects.DDMFieldRules).Count<DDMFieldRule>() <= 0 ? this.sessionObjects.BpmManager.GetAllDDMFieldRulesAndScenarios(true) : this.sessionObjects.DDMFieldRules);
        if (ddmFieldRuleArray != null)
        {
          if (ddmFieldRuleArray.Length != 0)
          {
            for (int index = 0; index < ddmFieldRuleArray.Length; ++index)
            {
              string advancedCode = DDMRuleCodeGenerator.Instance.GetAdvancedCode((DDMVmFeeRule) DDMViewModelConverter.ConvertDDMFieldRule(ddmFieldRuleArray[index]));
              colVmFeeRules.Add(advancedCode);
              if (this.EbsAttributes.EbsPerformanceOptimization)
                this.EbsAttributes.AddFieldRuleAdvCode(ddmFieldRuleArray[index].RuleID, advancedCode);
            }
            this.theAdvanceCodeForFieldRule = this.EbsAttributes.EbsPerformanceOptimization ? DDMRuleCodeGenerator.Instance.GetAdvancedCode(this.EbsAttributes.FieldRulesAdvCode.Values.ToList<string>()) : DDMRuleCodeGenerator.Instance.GetAdvancedCode(colVmFeeRules);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Error, "DDM - Cannot load DDM Rules. Error: " + ex.Message);
        this.errorMessage = ex.Message;
      }
      if (!DDMFieldExecContext.IsDDMDiagnosticOn())
        return;
      Tracing.Log(true, "DDM_DIAGNOTICS_DETAIL", nameof (DDMTrigger), "Generated AdvCode " + (this.theAdvanceCodeForFieldRule == null ? "empty code" : this.theAdvanceCodeForFieldRule));
    }

    public void ReassignUnsavedLoanBacktoDDM(
      LoanData unsavedLoan,
      ILoanConfigurationInfo unsavedLoanConfig)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 601, nameof (ReassignUnsavedLoanBacktoDDM), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DDM\\DDMTrigger.cs");
      EllieMae.EMLite.DataEngine.FieldProvider fieldProvider = (EllieMae.EMLite.DataEngine.FieldProvider) this.fieldProvider;
      this.newlyOpenedLoan = fieldProvider._Loan;
      this.newlyOpenedLoanConfigInfo = fieldProvider._LoanConfigInfo;
      fieldProvider.Reset(unsavedLoan, unsavedLoanConfig);
      this.Reset(unsavedLoan.GUID, this.fieldProvider);
      this.DDMLastModifiedDateTime = unsavedLoan.Settings.DDMLastModifiedDateTime;
      PerformanceMeter.Current.AddCheckpoint("END", 612, nameof (ReassignUnsavedLoanBacktoDDM), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DDM\\DDMTrigger.cs");
    }

    public void ReassignNewlyCreatedLoanBacktoDDM()
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 623, nameof (ReassignNewlyCreatedLoanBacktoDDM), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DDM\\DDMTrigger.cs");
      if (this.newlyOpenedLoan == null || this.newlyOpenedLoanConfigInfo == null)
      {
        PerformanceMeter.Current.AddCheckpoint("EXIT - null", 627, nameof (ReassignNewlyCreatedLoanBacktoDDM), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DDM\\DDMTrigger.cs");
      }
      else
      {
        EllieMae.EMLite.DataEngine.FieldProvider fieldProvider = (EllieMae.EMLite.DataEngine.FieldProvider) this.fieldProvider;
        fieldProvider.Reset(this.newlyOpenedLoan, this.newlyOpenedLoanConfigInfo);
        this.Reset(this.newlyOpenedLoan.GUID, (IFieldProvider) fieldProvider);
        this.DDMLastModifiedDateTime = this.newlyOpenedLoan.Settings.DDMLastModifiedDateTime;
        PerformanceMeter.Current.AddCheckpoint("END", 636, nameof (ReassignNewlyCreatedLoanBacktoDDM), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DDM\\DDMTrigger.cs");
      }
    }

    public string ErrorMessage => this.errorMessage;

    private bool canPopulateDDMTrigger(DDMStartPopulationTrigger triggerType, string fieldId = null)
    {
      try
      {
        if (this.willStopPopulationByAfterLEInit() || this.willStopPopulationByLoanCondition())
          return false;
        switch (triggerType)
        {
          case DDMStartPopulationTrigger.All:
            return this.willStartPopulationByFieldChange(fieldId) || this.checkPopStartByLoanSave();
          case DDMStartPopulationTrigger.LoanSave:
            return this.checkPopStartByLoanSave();
          case DDMStartPopulationTrigger.FieldChange:
            return this.willStartPopulationByFieldChange(fieldId);
          case DDMStartPopulationTrigger.UserRequest:
            return true;
          default:
            return false;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Error, "DDM - While checking if we CanPopuplateDDMTrigger(). Error: " + ex.Message);
        this.errorMessage = ex.Message;
        return false;
      }
    }

    private bool checkPopStartByLoanSave()
    {
      if (Tracing.Debug)
      {
        string msg = string.Format("DDM Start Timing Setting - Population Timing Object = [{0}], LoanSave = [{1}]", this.ddmPopulationTiming != null ? (object) "True" : (object) "Null", this.ddmPopulationTiming != null ? (object) this.ddmPopulationTiming.LoanSave.ToString() : (object) "Null");
        Tracing.Log(DDMTrigger.sw, TraceLevel.Verbose, nameof (DDMTrigger), msg);
      }
      return this.ddmPopulationTiming != null && this.ddmPopulationTiming.LoanSave;
    }

    private bool willStartPopulationByFieldChange(string fieldId)
    {
      if (Tracing.Debug)
      {
        string msg = string.Format("DDM Start Timing Setting - Population Timing Object = [{0}], FieldChanges = [{1}], FieldList#=[{2}], FieldId=[{3}]", this.ddmPopulationTiming != null ? (object) "True" : (object) "Null", this.ddmPopulationTiming != null ? (object) this.ddmPopulationTiming.FieldChanges.ToString() : (object) "Null", this.ddmPopulationTiming == null || this.ddmPopulationTiming.FieldList == null ? (object) "Null" : (object) this.ddmPopulationTiming.FieldList.Count.ToString(), (object) fieldId);
        Tracing.Log(DDMTrigger.sw, TraceLevel.Verbose, nameof (DDMTrigger), msg);
      }
      if (this.ddmPopulationTiming == null || !this.ddmPopulationTiming.FieldChanges || this.ddmPopulationTiming.FieldList.Count <= 0 || fieldId == null)
        return false;
      bool flag = this.ddmPopulationFieldList.Contains(fieldId);
      if (Tracing.Debug)
        Tracing.Log(DDMTrigger.sw, TraceLevel.Verbose, nameof (DDMTrigger), string.Format("DDM Start Timing Setting does not contain the field [{0}]", (object) fieldId));
      return flag;
    }

    private bool willStopPopulationByAfterLEInit()
    {
      if (Tracing.Debug)
      {
        string msg = string.Format("DDM Stop Timing Setting - Population Timing Object = [{0}], After Loan Init = [{1}]", this.ddmPopulationTiming != null ? (object) "True" : (object) "Null", this.ddmPopulationTiming != null ? (object) this.ddmPopulationTiming.AfterLoanInitEst.ToString() : (object) "Null");
        Tracing.Log(DDMTrigger.sw, TraceLevel.Verbose, nameof (DDMTrigger), msg);
      }
      if (this.ddmPopulationTiming == null || !this.ddmPopulationTiming.AfterLoanInitEst)
        return false;
      bool fieldValue = (bool) this.fieldProvider.GetFieldValue("LOANDATAREQUEST:GetInitialDisclosureTrackingLogsByType");
      if (Tracing.Debug)
        Tracing.Log(DDMTrigger.sw, TraceLevel.Verbose, nameof (DDMTrigger), "DDM - Will stop population by After LE Init = [" + fieldValue.ToString() + "]");
      return fieldValue;
    }

    private bool willStopPopulationByLoanCondition()
    {
      if (Tracing.Debug)
      {
        string msg = string.Format("DDM Stop Timing Setting - Population Timing Object = [{0}], Adv Code Check = [{1}], Adv Code = [{2}]", this.ddmPopulationTiming != null ? (object) "True" : (object) "Null", this.ddmPopulationTiming != null ? (object) this.ddmPopulationTiming.LoanCondMet.ToString() : (object) "Null", this.compiledDdmStopCode != null ? (object) this.compiledDdmStopCode.ToString() : (object) "Null");
        Tracing.Log(DDMTrigger.sw, TraceLevel.Verbose, nameof (DDMTrigger), msg);
      }
      if (this.ddmPopulationTiming == null || !this.ddmPopulationTiming.LoanCondMet || this.compiledDdmStopCode == null)
        return false;
      RuntimeContext context = new RuntimeContext((object) this.ddmHost, this.fieldProvider);
      context.Configuration.AllowedTypes.Allow(typeof (CalendarDataSource));
      this.compiledDdmStopCode.Execute(context);
      bool returnValue = (bool) context.ReturnValue;
      if (Tracing.Debug)
        Tracing.Log(DDMTrigger.sw, TraceLevel.Verbose, nameof (DDMTrigger), "DDM - Will stop population by Advanced Code = [" + returnValue.ToString() + "]");
      return returnValue;
    }

    private bool anyDdmPopulationStartTimingSet(DDMDataPopulationTiming ddmPopTiming)
    {
      return ddmPopTiming.FieldChanges && ddmPopTiming.FieldList.Count > 0 || ddmPopTiming.LoanSave;
    }

    private void UpdateFieldRuleAdvCode()
    {
      if (this.EbsAttributes.GetFieldRulesIdsToBuildAdvancedCode().Count < 1)
        return;
      this.buildAdvanceCodeForFieldRule();
      if (string.IsNullOrEmpty(this.theAdvanceCodeForFieldRule))
        return;
      Tracing.Log(DDMTrigger.sw, nameof (DDMTrigger), TraceLevel.Info, "DDM Field Rule Advanced Code - " + this.theAdvanceCodeForFieldRule);
      if (string.IsNullOrEmpty(this.theAdvanceCodeForFieldRule))
        return;
      this.compiledScriptForFieldRule = this.compiler.Compile(this.theAdvanceCodeForFieldRule);
      this.theAdvanceCodeForFieldRule = (string) null;
    }

    public DDMTrigger.EbsPerformanceAttributes EbsAttributes
    {
      get
      {
        if (this._ebsPerformanceAttributes != null)
          return this._ebsPerformanceAttributes;
        this._ebsPerformanceAttributes = new DDMTrigger.EbsPerformanceAttributes();
        return this._ebsPerformanceAttributes;
      }
      set => this._ebsPerformanceAttributes = new DDMTrigger.EbsPerformanceAttributes(value);
    }

    [Flags]
    private enum enforceDDMRules
    {
      None = 0,
      ValidateLE = 1,
      ValidateAdvCode = 2,
    }

    public class EbsPerformanceAttributes
    {
      public bool FromPlatform;
      public bool EbsPerformanceOptimization;
      public List<int> RequiredFieldRuleIds;
      private Dictionary<int, string> _fieldRulesAdvCode;

      public EbsPerformanceAttributes()
      {
      }

      public EbsPerformanceAttributes(DDMTrigger.EbsPerformanceAttributes ebsAttributes)
      {
        if (ebsAttributes == null || ebsAttributes.RequiredFieldRuleIds == null || ebsAttributes.RequiredFieldRuleIds.Count < 1)
          return;
        this.EbsPerformanceOptimization = ebsAttributes.EbsPerformanceOptimization;
        this.RequiredFieldRuleIds = ebsAttributes.RequiredFieldRuleIds;
        this.FromPlatform = true;
      }

      public Dictionary<int, string> FieldRulesAdvCode
      {
        get => this._fieldRulesAdvCode ?? (this._fieldRulesAdvCode = new Dictionary<int, string>());
      }

      public List<int> GetFieldRulesIdsToBuildAdvancedCode()
      {
        return this._fieldRulesAdvCode == null || this._fieldRulesAdvCode.Count < 1 ? this.RequiredFieldRuleIds : this.RequiredFieldRuleIds.Except<int>((IEnumerable<int>) this.FieldRulesAdvCode.Keys.ToList<int>()).ToList<int>();
      }

      public void AddFieldRuleAdvCode(int ruleId, string advCode)
      {
        if (ruleId < 1 || string.IsNullOrEmpty(advCode))
          return;
        if (this.FieldRulesAdvCode.ContainsKey(ruleId))
          this.FieldRulesAdvCode[ruleId] = advCode;
        else
          this.FieldRulesAdvCode.Add(ruleId, advCode);
      }

      public void AddRequiredfieldRuleIds(List<int> ruleIds)
      {
        ruleIds?.ForEach((Action<int>) (x =>
        {
          if (this.RequiredFieldRuleIds.Contains(x))
            return;
          this.RequiredFieldRuleIds.Add(x);
        }));
      }
    }
  }
}
