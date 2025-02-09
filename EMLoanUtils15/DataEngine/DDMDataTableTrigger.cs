// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMDataTableTrigger
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using Elli.AdvCode.Runtime;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DDMDataTableTrigger
  {
    private const string className = "DDMDataTableTrigger�";
    private static readonly string sw = Tracing.SwDataEngine;
    private Compiler Compiler;
    private SessionObjects sessionObjects;
    private List<string> dataTableNames;
    private readonly ConcurrentDictionary<string, DDMDataTableTrigger.AdvancedCodeAssets> DataTableAdvCodes;
    private Dictionary<string, DDMDataTable> ddmDataTableByTableNames = new Dictionary<string, DDMDataTable>();
    private HashSet<string> deletedTableNames;
    private IDataCache ebsV3DataCache;
    private DDMHost ddmHost;

    public DDMDataTableTrigger(
      SessionObjects sessionObjects,
      List<string> dataTableNames = null,
      IDataCache ebsV3DataCache = null)
    {
      this.sessionObjects = sessionObjects;
      this.ddmHost = new DDMHost(sessionObjects);
      this.ebsV3DataCache = ebsV3DataCache != null ? ebsV3DataCache : (IDataCache) new DisabledEbsV3Cache();
      if (sessionObjects.DDMBackgroundLoadingState == null)
        sessionObjects.DDMBackgroundLoadingState = (object) new DDMDataTableTrigger.DDMBackgroundLoadingState(this.ebsV3DataCache);
      this.DataTableAdvCodes = ((DDMDataTableTrigger.DDMBackgroundLoadingState) sessionObjects.DDMBackgroundLoadingState).DataTableAdvCodes;
      this.Compiler = new Compiler();
      this.dataTableNames = dataTableNames;
      this.deletedTableNames = new HashSet<string>();
    }

    public void Initialize()
    {
      if (((DDMDataTableTrigger.DDMBackgroundLoadingState) this.sessionObjects.DDMBackgroundLoadingState).dataTableBackgroundLoadingTask != null || this.DataTableAdvCodes.Count != 0)
        this.BackgroundCacheRenewal();
      else
        this.BackgroundLoading();
    }

    public Task OneTimeInitialize()
    {
      DDMDataTableTrigger.DDMBackgroundLoadingState backgroundLoadingState = (DDMDataTableTrigger.DDMBackgroundLoadingState) this.sessionObjects.DDMBackgroundLoadingState;
      if (backgroundLoadingState.DataTableAdvCodes.Count != 0)
        return (Task) null;
      this.BackgroundLoading();
      return backgroundLoadingState.dataTableBackgroundLoadingTask;
    }

    private void BackgroundLoading()
    {
      ((DDMDataTableTrigger.DDMBackgroundLoadingState) this.sessionObjects.DDMBackgroundLoadingState).dataTableBackgroundLoadingTask = Task.Run((Action) (() => this.populateCache(this.dataTableNames != null ? this.sessionObjects.BpmManager.GetDDMDataTablesAndFieldValuesForDataTableNames(this.dataTableNames) : this.sessionObjects.BpmManager.GetAllReferencedDDMDataTablesWithFieldValues())));
    }

    private void populateCache(DDMDataTable[] dataTables)
    {
      Parallel.ForEach<DDMDataTable>((IEnumerable<DDMDataTable>) dataTables, (Action<DDMDataTable, ParallelLoopState, long>) ((dataTable, _loopState, i) =>
      {
        try
        {
          this.DataTableAdvCodes.GetOrAdd(dataTable.Name, (Func<string, DDMDataTableTrigger.AdvancedCodeAssets>) (key =>
          {
            DDMDataTableTrigger.AdvancedCodeAssets advancedCodeAssets = this.buildAdvancedCodeAsset(dataTable);
            dataTables[i] = (DDMDataTable) null;
            return advancedCodeAssets;
          }));
        }
        catch (Exception ex)
        {
          Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Error, "Building the advanced code for dataTable " + dataTable.Name + " failed with error: " + ex.Message);
        }
      }));
      Task.Run((Action) (() => Parallel.ForEach<KeyValuePair<string, DDMDataTableTrigger.AdvancedCodeAssets>>((IEnumerable<KeyValuePair<string, DDMDataTableTrigger.AdvancedCodeAssets>>) this.DataTableAdvCodes, (Action<KeyValuePair<string, DDMDataTableTrigger.AdvancedCodeAssets>>) (kvPair =>
      {
        DDMDataTableTrigger.AdvancedCodeAssets advCodeAsset = kvPair.Value;
        Compiler compiler = new Compiler();
        this.compileAdvancedCodeAsset(kvPair.Key, advCodeAsset, compiler);
      }))));
    }

    private void BackgroundCacheRenewal()
    {
      DDMDataTableTrigger.DDMBackgroundLoadingState backgroundLoadingState = (DDMDataTableTrigger.DDMBackgroundLoadingState) this.sessionObjects.DDMBackgroundLoadingState;
      try
      {
        if (backgroundLoadingState.pendingRenewalTask != null)
          backgroundLoadingState.pendingRenewalTask.Wait();
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Error, "DDM Background cache renewal failed with error :" + ex.Message);
      }
      this.sessionObjects.ClearCachedFieldSettings();
      backgroundLoadingState.pendingRenewalTask = Task.Run((Action) (() =>
      {
        try
        {
          Dictionary<string, DateTime> dictionary = new Dictionary<string, DateTime>();
          List<string> dataTableNames = new List<string>();
          foreach (DDMDataTable allDdmDataTable in this.sessionObjects.BpmManager.GetAllDDMDataTables())
            dictionary[allDdmDataTable.Name] = Convert.ToDateTime(allDdmDataTable.LastModDt);
          try
          {
            backgroundLoadingState.dataTableBackgroundLoadingTask.Wait();
          }
          catch (Exception ex)
          {
            Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Error, "DDM Background data table loading failed with error :" + ex.Message);
          }
          foreach (KeyValuePair<string, DDMDataTableTrigger.AdvancedCodeAssets> dataTableAdvCode in this.DataTableAdvCodes)
          {
            if (!dictionary.ContainsKey(dataTableAdvCode.Key) || !DateTime.Equals(dictionary[dataTableAdvCode.Key], dataTableAdvCode.Value.LastModifiedDate))
              dataTableNames.Add(dataTableAdvCode.Key);
          }
          foreach (string key in dataTableNames)
            ((IDictionary<string, DDMDataTableTrigger.AdvancedCodeAssets>) this.DataTableAdvCodes).Remove(key);
          if (dataTableNames.Count <= 0)
            return;
          this.populateCache(this.sessionObjects.BpmManager.GetDDMDataTablesAndFieldValuesForDataTableNames(dataTableNames));
        }
        catch (Exception ex)
        {
          Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Error, "DDM Background cache renewal failed with error :" + ex.Message);
        }
      }));
    }

    public object GetValue(string ddmTableName, IFieldProvider fieldProvider, int outputIdx = 0)
    {
      try
      {
        DDMDataTableTrigger.DDMBackgroundLoadingState backgroundLoadingState = (DDMDataTableTrigger.DDMBackgroundLoadingState) this.sessionObjects.DDMBackgroundLoadingState;
        if (backgroundLoadingState.pendingRenewalTask != null)
          backgroundLoadingState.pendingRenewalTask.Wait();
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Error, "DDM Background cache renewal failed with error :" + ex.Message);
      }
      if (!this.DataTableAdvCodes.ContainsKey(ddmTableName) && !this.deletedTableNames.Contains(ddmTableName))
      {
        DDMDataTable[] forDataTableNames = this.sessionObjects.BpmManager.GetDDMDataTablesAndFieldValuesForDataTableNames(new List<string>()
        {
          ddmTableName
        });
        if (forDataTableNames.Length != 0)
        {
          DDMDataTable dataTable = forDataTableNames[0];
          DDMDataTableTrigger.AdvancedCodeAssets advancedCodeAssets = this.ebsV3DataCache.Get<DDMDataTableTrigger.AdvancedCodeAssets>("DataTableAdvCodes/" + ddmTableName, (Func<DDMDataTableTrigger.AdvancedCodeAssets>) (() => this.buildAdvancedCodeAsset(dataTable)));
          this.DataTableAdvCodes[dataTable.Name] = advancedCodeAssets;
        }
        else
        {
          this.deletedTableNames.Add(ddmTableName);
          ((IDictionary<string, DDMDataTableTrigger.AdvancedCodeAssets>) this.DataTableAdvCodes).Remove(ddmTableName);
        }
      }
      if (this.DataTableAdvCodes.ContainsKey(ddmTableName))
      {
        if (outputIdx < 0)
          outputIdx = 0;
        DDMDataTableTrigger.AdvancedCodeAssets dataTableAdvCode = this.DataTableAdvCodes[ddmTableName];
        try
        {
          this.compileAdvancedCodeAsset(ddmTableName, dataTableAdvCode, this.Compiler);
          RuntimeContext context = new RuntimeContext((object) this.ddmHost, fieldProvider);
          context.Configuration.AllowedTypes.Allow(typeof (CalendarDataSource));
          dataTableAdvCode.PrecompiledScript.Execute(context);
          if (Tracing.Debug)
            Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Verbose, string.Format("DataTable Execute Result : Name=[{0}], Result=[{1}]", (object) ddmTableName, context.ReturnValue == null ? (object) "Null" : (object) Convert.ToString(string.Join(", ", context.ReturnValue as object[]))));
          object returnValue = context.ReturnValue;
          if (returnValue == null)
            return (object) null;
          object[] objArray = returnValue as object[];
          return outputIdx < objArray.Length ? objArray[outputIdx] : (object) null;
        }
        catch (Exception ex)
        {
          Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Error, "Exception while executing DDM Datatable advanced code. Error: " + ex.Message + "\r\n" + dataTableAdvCode.AdvancedCode);
          return (object) null;
        }
      }
      else
      {
        Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Error, "DDM DataTable (" + ddmTableName + ") does not exist.");
        return (object) null;
      }
    }

    public object GetValue(
      string ddmTableName,
      IFieldProvider fieldProvider,
      bool useDataTableOptimization,
      int outputIdx = 0)
    {
      if (!useDataTableOptimization)
        return this.GetValue(ddmTableName, fieldProvider, outputIdx);
      DDMDataTable forDataTableName;
      if (!this.ddmDataTableByTableNames.TryGetValue(ddmTableName, out forDataTableName))
      {
        forDataTableName = this.sessionObjects.BpmManager.GetDDMDataTableAndFieldIdsForDataTableName(ddmTableName);
        this.ddmDataTableByTableNames[ddmTableName] = forDataTableName;
      }
      if (forDataTableName == null)
        return (object) null;
      string[] strArray = forDataTableName.FieldIdList.Split('|');
      Dictionary<string, string> fieldValues = new Dictionary<string, string>();
      foreach (string str in strArray)
      {
        object fieldValue = fieldProvider.GetFieldValue(str);
        fieldValues[str] = fieldValue != null ? fieldValue.ToString() : string.Empty;
      }
      this.sessionObjects.BpmManager.AddFieldValuesForDataTable(forDataTableName, fieldValues);
      DDMDataTableTrigger.AdvancedCodeAssets advCodeAsset = this.buildAdvancedCodeAsset(forDataTableName);
      try
      {
        this.compileAdvancedCodeAsset(ddmTableName, advCodeAsset, this.Compiler);
        RuntimeContext context = new RuntimeContext((object) this.ddmHost, fieldProvider);
        context.Configuration.AllowedTypes.Allow(typeof (CalendarDataSource));
        advCodeAsset.PrecompiledScript.Execute(context);
        if (Tracing.Debug)
          Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Verbose, string.Format("DataTable Execute Result : Name=[{0}], Result=[{1}]", (object) ddmTableName, context.ReturnValue == null ? (object) "Null" : (object) Convert.ToString(string.Join(", ", context.ReturnValue as object[]))));
        object returnValue = context.ReturnValue;
        if (returnValue == null)
          return (object) null;
        object[] objArray = returnValue as object[];
        return outputIdx < objArray.Length ? objArray[outputIdx] : (object) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Error, "Exception while executing DDM Datatable advanced code. Error: " + ex.Message + "\r\n" + advCodeAsset.AdvancedCode);
        return (object) null;
      }
    }

    private void compileAdvancedCodeAsset(
      string dataTableName,
      DDMDataTableTrigger.AdvancedCodeAssets advCodeAsset,
      Compiler compiler)
    {
      try
      {
        lock (advCodeAsset)
        {
          if (advCodeAsset.PrecompiledScript != null)
            return;
          advCodeAsset.PrecompiledScript = compiler.Compile(advCodeAsset.AdvancedCode);
          advCodeAsset.AdvancedCode = (string) null;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Error, "DDM DataTable compilation for " + dataTableName + " failed: " + ex.Message);
      }
    }

    private DDMDataTableTrigger.AdvancedCodeAssets buildAdvancedCodeAsset(DDMDataTable dataTable)
    {
      DDMDataTableTrigger.AdvancedCodeAssets advancedCodeAssets = new DDMDataTableTrigger.AdvancedCodeAssets();
      DDMVmDataTable vmDt = DDMViewModelConverter.ConvertDDMDataTable(dataTable, this.sessionObjects);
      advancedCodeAssets.AdvancedCode = this.buildSingleDataTableAdvCode(vmDt);
      advancedCodeAssets.LastModifiedDate = Convert.ToDateTime(dataTable.LastModDt);
      return advancedCodeAssets;
    }

    private string buildSingleDataTableAdvCode(DDMVmDataTable vmDt)
    {
      string advancedCode = DDMRuleCodeGenerator.Instance.GetAdvancedCode(vmDt);
      if (Tracing.Debug)
        Tracing.Log(DDMDataTableTrigger.sw, nameof (DDMDataTableTrigger), TraceLevel.Verbose, string.Format("DataTable AdvCode : {0} {1}", (object) vmDt.Name, (object) advancedCode));
      if (DDMFieldExecContext.IsDDMDiagnosticOn())
        Tracing.Log(true, "DDM_DIAGNOTICS_DETAIL", nameof (DDMDataTableTrigger), "Generated AdvCode for DataTable : " + vmDt.Name + " " + (advancedCode == null ? "empty code" : advancedCode));
      return advancedCode;
    }

    public void Initialize(DDMVmDataTable[] vmDataTables)
    {
      this.DataTableAdvCodes.Clear();
      foreach (DDMVmDataTable vmDataTable in vmDataTables)
        this.DataTableAdvCodes[vmDataTable.Name] = new DDMDataTableTrigger.AdvancedCodeAssets()
        {
          AdvancedCode = this.buildSingleDataTableAdvCode(vmDataTable)
        };
    }

    private class AdvancedCodeAssets
    {
      public DateTime LastModifiedDate;
      public string AdvancedCode;
      public CompiledScript PrecompiledScript;
    }

    private class DDMBackgroundLoadingState
    {
      public readonly ConcurrentDictionary<string, DDMDataTableTrigger.AdvancedCodeAssets> DataTableAdvCodes;
      public Task dataTableBackgroundLoadingTask;
      public Task pendingRenewalTask;

      public DDMBackgroundLoadingState(IDataCache ebsV3DataCache)
      {
        this.DataTableAdvCodes = ebsV3DataCache.Get<ConcurrentDictionary<string, DDMDataTableTrigger.AdvancedCodeAssets>>("DDMBackgroundLoadingState.DataTableAdvCodes", (Func<ConcurrentDictionary<string, DDMDataTableTrigger.AdvancedCodeAssets>>) (() => new ConcurrentDictionary<string, DDMDataTableTrigger.AdvancedCodeAssets>()));
        this.dataTableBackgroundLoadingTask = (Task) null;
        this.pendingRenewalTask = (Task) null;
      }
    }
  }
}
