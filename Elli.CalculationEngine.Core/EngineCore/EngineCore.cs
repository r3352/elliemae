// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.EngineCore.EngineCore
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.CalculationLibrary;
using Elli.CalculationEngine.Core.CoreFunctions;
using Elli.CalculationEngine.Core.DataSource;
using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

#nullable disable
namespace Elli.CalculationEngine.Core.EngineCore
{
  public class EngineCore : IDisposable
  {
    private const string className = "EngineCore";
    private static readonly object _logSyncObject = new object();
    private static TraceSwitch traceSwitch = new TraceSwitch("CalculationEngine", "CalculationEngine");
    private int indentation;
    private bool buildDependencyDiagramString;
    private StringWriter dependencyDiagramWriter = new StringWriter();
    private Stopwatch sw = new Stopwatch();
    private Stopwatch swCalc = new Stopwatch();
    private TimeSpan transTotal = TimeSpan.Zero;
    private TimeSpan fieldTotal = TimeSpan.Zero;
    private TimeSpan longestTransientTime = TimeSpan.Zero;
    private TimeSpan longestFieldTime = TimeSpan.Zero;
    private string longestTransientCalc = string.Empty;
    private string longestFieldCalc = string.Empty;
    private int transCount;
    private int fieldCount;
    private bool _debugCalculationPerformance;
    private bool _debugLogCalculatedResults;
    private bool _debugLogExecutionPlan;
    private bool _debugLogCalcForMod;
    private bool _debugLogDataSourceValues;
    private bool _enableShortCircuiting;
    private ICalculationContext calculationContext;
    private DataSourceWrapper dataSource;
    private CalculationSet calculationSet;
    private DependencyGraph dependencyGraph;
    private List<Tuple<string, string>> skipCalcList;
    private Dictionary<string, HashSet<string>> visitedDictionary = new Dictionary<string, HashSet<string>>();
    private Dictionary<long, FieldState> fieldStateDictionary = new Dictionary<long, FieldState>();
    private List<DataFieldWrapper> calcQueue = new List<DataFieldWrapper>();
    private List<short> _valueChangedList = new List<short>();
    private List<short> _modifiedIndexes = new List<short>();
    private readonly ExecutionDataTable executionPlanTable;

    public IDataSource DataSource
    {
      get => (IDataSource) this.dataSource;
      set
      {
      }
    }

    public EngineCore(
      CalculationSet calculationSet,
      DependencyGraph dependencyGraph,
      ExecutionDataTable executionPlanTable)
    {
      if (CalculationEngineConfiguration.CurrentConfiguration != null)
      {
        this._debugCalculationPerformance = CalculationEngineConfiguration.CurrentConfiguration.DebugCalculationPerformance;
        this._debugLogCalcForMod = CalculationEngineConfiguration.CurrentConfiguration.DebugLogCalcForMod;
        this._debugLogCalculatedResults = CalculationEngineConfiguration.CurrentConfiguration.DebugLogCalculatedResults;
        this._debugLogExecutionPlan = CalculationEngineConfiguration.CurrentConfiguration.DebugLogExecutionPlan;
        this._debugLogDataSourceValues = CalculationEngineConfiguration.CurrentConfiguration.DebugLogDataSourceValues;
        this._enableShortCircuiting = CalculationEngineConfiguration.CurrentConfiguration.EnableShortCircuiting;
      }
      if (this.calculationSet == null)
        this.calculationSet = calculationSet;
      if (this.dependencyGraph == null)
        this.dependencyGraph = dependencyGraph;
      if (this.executionPlanTable != null)
        return;
      this.executionPlanTable = executionPlanTable;
    }

    public void Initialize<T>(
      IDataSource initialDataSource,
      T model,
      bool initializeTransients = true,
      List<Tuple<string, string>> skipList = null,
      Assembly callingAssembly = null)
    {
      Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("---Initialize()---"));
      this.skipCalcList = skipList;
      this.fieldStateDictionary.Clear();
      EntityDescriptor entityType = initialDataSource.GetEntityType();
      if (entityType != this.calculationSet.RootEntityType)
        throw new Exception(string.Format("Root Entity Type {{{0}}} in DataSource does not match CalculationSet Root Entity Type {{{1}}}.", (object) entityType, (object) this.calculationSet.RootEntityType));
      DataSourceWrapperFactory sourceWrapperFactory = new DataSourceWrapperFactory(callingAssembly);
      this.dataSource = initializeTransients || this.dataSource == null ? sourceWrapperFactory.CreateDataSourceWrapper(initialDataSource, this.calculationSet) : sourceWrapperFactory.CreateDataSourceWrapper(initialDataSource, this.calculationSet, this.dataSource.TransientFields);
      this.calculationContext = (ICalculationContext) new CalculationContext((IDataEntity) this.dataSource);
    }

    public void ClearEntityCache()
    {
      CoreFunctionsBase.EnumerableDictionary.Clear();
      this.dataSource.ClearEntityCache();
      CalculationUtility._queryDictionary.Clear();
    }

    public object GetCalculatedValue<T>(DataFieldWrapper field, T model, StringBuilder sbTrace = null)
    {
      sbTrace?.AppendLine("EngineCore | -- GetCalculatedValue() Begin --");
      object calculatedValue = (object) null;
      if (field != null && !field.IsNull())
      {
        if (field.CalcImpl == null)
          this.dataSource.InitializeDataField((IDataField) field);
        if (field.CalcImpl != null)
        {
          try
          {
            if (this._debugCalculationPerformance)
              this.swCalc.Restart();
            this.calculationContext.RootDataSource = this.dataSource;
            this.calculationContext.ContextRootEntity = field.WrappedParentEntity;
            this.calculationContext.LockAfterSetValue = false;
            this.calculationContext.UnlockBeforeSetValue = false;
            calculatedValue = field.CalcImpl.Calculate<T>(model, ref this.calculationContext);
            if ((field.Calculation.ReturnType == Elli.CalculationEngine.Common.ValueType.Date || field.Calculation.ReturnType == Elli.CalculationEngine.Common.ValueType.DateTime || field.Calculation.ReturnType == Elli.CalculationEngine.Common.ValueType.NullableDate || field.Calculation.ReturnType == Elli.CalculationEngine.Common.ValueType.NullableDateTime) && Convert.ToDateTime(calculatedValue) == DateTime.MinValue)
              calculatedValue = (object) null;
            this.calculationContext.RootDataSource = (DataSourceWrapper) null;
            this.calculationContext.ContextRootEntity = (DataEntityWrapper) null;
          }
          catch (Exception ex)
          {
            Tracing.Log(TraceLevel.Error, nameof (EngineCore), string.Format("Error Calculating field [{0}]; CalcId = {1}: {2}", (object) field.GetQualifiedName(), (object) field.CalcId, (object) ex.Message));
            sbTrace?.AppendLine("EngineCore | " + string.Format("Error Calculating field [{0}]; CalcId = {1}: {2}", (object) field.GetQualifiedName(), (object) field.CalcId, (object) ex.Message));
            throw;
          }
          finally
          {
            if (this._debugCalculationPerformance)
            {
              this.swCalc.Stop();
              if (EngineUtility.IsTransientField(field.Id))
              {
                this.transTotal += this.swCalc.Elapsed;
                ++this.transCount;
                if (this.longestTransientTime < this.swCalc.Elapsed)
                {
                  this.longestTransientTime = this.swCalc.Elapsed;
                  this.longestTransientCalc = field.GetQualifiedName();
                }
              }
              else
              {
                this.fieldTotal += this.swCalc.Elapsed;
                ++this.fieldCount;
                if (this.longestFieldTime < this.swCalc.Elapsed)
                {
                  this.longestFieldTime = this.swCalc.Elapsed;
                  this.longestFieldCalc = field.GetQualifiedName();
                }
              }
            }
          }
        }
      }
      sbTrace?.AppendLine("EngineCore | -- GetCalculatedValue() End --");
      return calculatedValue;
    }

    public IEnumerable<string> CalculateFieldAndDependencies<T>(DataFieldWrapper field, T model)
    {
      Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("CalculateFieldAndDependencies()"));
      List<string> fieldAndDependencies = new List<string>();
      this.LogDataSourceValues();
      try
      {
        new List<DataFieldWrapper>() { field };
        bool enableShortCircuiting = this._enableShortCircuiting;
        this._enableShortCircuiting = false;
        fieldAndDependencies = this.RunDependentCalculations<T>((List<DataFieldWrapper>) null, new List<DataFieldWrapper>()
        {
          field
        }, new List<DataFieldWrapper>() { field }, new List<DataFieldWrapper>(), (IEnumerable<Type>) null, model);
        this._enableShortCircuiting = enableShortCircuiting;
      }
      finally
      {
        this.LogDependencyDiagram();
      }
      return (IEnumerable<string>) fieldAndDependencies;
    }

    public IEnumerable<string> CalculateDependenciesForModifiedFields<T>(
      List<DataFieldWrapper> modifiedFields,
      List<DataFieldWrapper> lockedFields,
      List<DataFieldWrapper> unlockedFields,
      IEnumerable<Type> modifiedEntityTypes,
      T model,
      StringBuilder sbTrace = null)
    {
      Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("---CalculateDependenciesForModifiedFields()---"));
      List<string> stringList = new List<string>();
      this._valueChangedList.Clear();
      this.indentation = 0;
      this.LogDataSourceValues();
      this.StartPerformanceLogging();
      List<string> forModifiedFields = this.RunDependentCalculations<T>((List<DataFieldWrapper>) null, modifiedFields, lockedFields, unlockedFields, modifiedEntityTypes, model, sbTrace);
      this.EndPerformanceLogging(nameof (CalculateDependenciesForModifiedFields));
      return (IEnumerable<string>) forModifiedFields;
    }

    public object CalculateTargetFieldValue<T>(
      List<DataFieldWrapper> targetFields,
      List<DataFieldWrapper> modifiedFields,
      List<DataFieldWrapper> lockedFields,
      List<DataFieldWrapper> unlockedFields,
      T model)
    {
      Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("---CalculateDependenciesForModifiedFields()---"));
      this._valueChangedList.Clear();
      this.indentation = 0;
      this.LogDataSourceValues();
      this.StartPerformanceLogging();
      bool enableShortCircuiting = this._enableShortCircuiting;
      this._enableShortCircuiting = false;
      List<string> targetFieldValue = this.RunDependentCalculations<T>(targetFields, modifiedFields, lockedFields, unlockedFields, (IEnumerable<Type>) null, model);
      this.EndPerformanceLogging(nameof (CalculateTargetFieldValue));
      this._enableShortCircuiting = enableShortCircuiting;
      return (object) targetFieldValue;
    }

    public string CalculateAllFields<T>(T model, bool transientsOnly = false)
    {
      StringBuilder sbTrace = new StringBuilder();
      Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("---CalculateAllFields()---"));
      sbTrace.AppendLine("EngineCore | -- CalculateAllFields() Begin --");
      sbTrace.Append(this.LogDataSourceValues());
      this.StartPerformanceLogging();
      sbTrace.Append(this.InitializeParameterizedEntities());
      List<DataFieldWrapper> dataFieldWrapperList = new List<DataFieldWrapper>();
      Dictionary<DataFieldWrapper, bool> dictionary = new Dictionary<DataFieldWrapper, bool>();
      foreach (FieldExecutionData allExecutionPlan in this.executionPlanTable.GetAllExecutionPlans())
      {
        FieldDescriptor descriptor = allExecutionPlan.Descriptor;
        string fieldName = descriptor.FieldId;
        EntityDescriptor parentType = descriptor.ParentEntityType;
        try
        {
          if (transientsOnly)
          {
            if (!EngineUtility.IsTransientField(descriptor.FieldId))
              continue;
          }
          if (parentType == (EntityDescriptor) null)
            sbTrace.AppendLine("item.Descriptor.ParentEntityType is null.");
          if (this.skipCalcList != null)
          {
            if (this.skipCalcList.Any<Tuple<string, string>>((Func<Tuple<string, string>, bool>) (p => p.Item1 == parentType.ToString() && p.Item2 == fieldName)))
              continue;
          }
          if (allExecutionPlan.HasCalculation)
          {
            if (!((IEnumerable<short>) allExecutionPlan.WeakReferences).Any<short>())
            {
              foreach (DataEntityWrapper dataEntityWrapper in this.dataSource.GetAllWrappedEntitiesOfType(parentType, sbTrace))
              {
                if (dataEntityWrapper != null)
                {
                  DataFieldWrapper wrappedField = dataEntityWrapper.GetWrappedField(fieldName);
                  if (wrappedField != null && !wrappedField.IsNull() && !dictionary.TryGetValue(wrappedField, out bool _))
                  {
                    dataFieldWrapperList.Add(wrappedField);
                    dictionary.Add(wrappedField, true);
                  }
                }
                else
                  sbTrace.AppendLine(string.Format("DataEntityWrapper from the entityList is null for field {{{0}}}.[{1}]", (object) parentType.ToString(), (object) fieldName));
              }
            }
          }
        }
        catch (Exception ex)
        {
          throw new Exception(this.GenerateExceptionMessage(string.Format("Error getting field {0}.{1}", (object) parentType, (object) fieldName), sbTrace), ex);
        }
      }
      foreach (DataFieldWrapper dataFieldWrapper in dataFieldWrapperList)
      {
        try
        {
          object oldVal = (object) null;
          if (this._debugLogCalculatedResults)
            oldVal = dataFieldWrapper.GetValue();
          object calculatedVal;
          bool isFieldChanged = this.ExecuteCalculation<T>(DataFieldWrapper.Create((IDataField) dataFieldWrapper, (DataSourceWrapper) this.DataSource), oldVal, this._debugLogCalculatedResults, out calculatedVal, model);
          if (this._debugLogCalculatedResults & isFieldChanged)
            sbTrace.AppendLine(this.LogCalculatedResults(dataFieldWrapper, oldVal, calculatedVal, isFieldChanged));
        }
        catch (Exception ex)
        {
          throw new Exception(this.GenerateExceptionMessage(string.Format("Error calculating field {0}", (object) dataFieldWrapper.Id), sbTrace), ex);
        }
      }
      if (transientsOnly)
        this.EndPerformanceLogging("Initialize");
      else
        this.EndPerformanceLogging(nameof (CalculateAllFields));
      sbTrace.Append(this.dataSource.CalculationLog);
      sbTrace.AppendLine("EngineCore | -- CalculateAllFields() End --");
      return sbTrace.ToString();
    }

    public IEnumerable<string> GetExecutionPlan<T>(
      IEnumerable<DataFieldWrapper> modifiedFields,
      T model)
    {
      List<short> mergedExecutionList = new List<short>();
      List<short> mergedInitializationList = new List<short>();
      List<short> fullInitializationList = new List<short>();
      List<short> modifiedLockIndexes = new List<short>();
      Elli.CalculationEngine.Core.EngineCore.EngineCore.BuildMergedExecutionPlan(this.executionPlanTable, new List<short>(), this.BuildIndexListFromFieldWrappers(modifiedFields), modifiedLockIndexes, out mergedExecutionList, out mergedInitializationList, out fullInitializationList);
      List<string> executionPlan = new List<string>();
      foreach (short num in mergedExecutionList)
      {
        short item = num;
        executionPlan.Add(this.executionPlanTable.FieldIndexDictionary.FirstOrDefault<KeyValuePair<FieldDescriptor, short>>((Func<KeyValuePair<FieldDescriptor, short>, bool>) (p => (int) p.Value == (int) item)).Key.ToString());
      }
      return (IEnumerable<string>) executionPlan;
    }

    public string GetElementDescriptor(string fieldId)
    {
      return this.calculationSet.GetElementDescriptor(fieldId);
    }

    public string[] ListElements() => this.calculationSet.ListElements();

    public void ExportDataSource(string exportFile) => this.dataSource.ExportDataSource(exportFile);

    public void ImportDataSource<T>(string importFile, T model)
    {
      this.dataSource.ImportDataSource(importFile);
      this.Initialize<T>((IDataSource) this.dataSource, model);
    }

    public IEnumerable<string> ValidateDataSource(bool includeFieldValidation = true)
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<FieldDescriptor, DependencyNode> node in this.dependencyGraph.nodeDictionary)
      {
        foreach (KeyValuePair<ReferencedElement, DependencyNode> referencedRelationship in node.Value.ReferencedRelationships)
        {
          EntityDescriptor parentEntityType = referencedRelationship.Value.ParentEntityType;
          string fieldId = referencedRelationship.Value.FieldId;
          IEnumerable<DataEntityWrapper> wrappedEntitiesOfType = this.dataSource.GetAllWrappedEntitiesOfType(node.Key.ParentEntityType);
          DataEntityWrapper rootEntity = wrappedEntitiesOfType.Any<DataEntityWrapper>() ? wrappedEntitiesOfType.FirstOrDefault<DataEntityWrapper>() : (DataEntityWrapper) null;
          ReferencedElement key = referencedRelationship.Key;
          IEnumerable<DataEntityWrapper> entitiesFromDataSource = this.GetRelatedEntitiesFromDataSource(rootEntity, key);
          if (rootEntity == null || !entitiesFromDataSource.Any<DataEntityWrapper>())
          {
            string message = string.Format("Missing reference in Calculation for {0}. Missing entity in reference path: {1}.", (object) node.Value.FieldDescriptor.ToString(), (object) key.QualifiedName);
            if (!stringList.Contains(message))
            {
              stringList.Add(message);
              Tracing.Log(TraceLevel.Info, nameof (EngineCore), message);
            }
          }
          else if (includeFieldValidation)
          {
            try
            {
              if (entitiesFromDataSource.FirstOrDefault<DataEntityWrapper>().GetField(fieldId) == null)
              {
                string message = string.Format("Missing reference in Calculation for {0}. No Field of Type {1} in entity {2}.", (object) node.Value.FieldDescriptor.ToString(), (object) fieldId, (object) parentEntityType.ToString());
                if (!stringList.Contains(message))
                {
                  stringList.Add(message);
                  Tracing.Log(TraceLevel.Info, nameof (EngineCore), message);
                }
              }
            }
            catch
            {
              string message = string.Format("Missing reference in Calculation for {0}. No Field of Type {1} in entity {2}.", (object) node.Value.FieldDescriptor.ToString(), (object) fieldId, (object) parentEntityType.ToString());
              if (!stringList.Contains(message))
              {
                stringList.Add(message);
                Tracing.Log(TraceLevel.Info, nameof (EngineCore), message);
              }
            }
          }
        }
      }
      return (IEnumerable<string>) stringList;
    }

    public static void BuildMergedExecutionPlan(
      ExecutionDataTable table,
      List<short> targetIndexes,
      List<short> modifiedIndexes,
      List<short> modifiedLockIndexes,
      out List<short> mergedExecutionList,
      out List<short> mergedInitializationList,
      out List<short> fullInitializationList)
    {
      mergedExecutionList = new List<short>();
      mergedInitializationList = new List<short>();
      fullInitializationList = new List<short>();
      List<short> shortList = new List<short>();
      List<short> second = new List<short>();
      foreach (short modifiedIndex in modifiedIndexes)
      {
        if (modifiedIndex >= (short) 0)
        {
          FieldExecutionData fieldExecutionData1 = table.GetFieldExecutionData(modifiedIndex);
          if (((IEnumerable<short>) fieldExecutionData1.WeakDependencies).Any<short>())
          {
            second.Add(modifiedIndex);
            foreach (short weakDependency in fieldExecutionData1.WeakDependencies)
            {
              FieldExecutionData fieldExecutionData2 = table.GetFieldExecutionData(weakDependency);
              if (((IEnumerable<short>) fieldExecutionData2.WeakReferences).Intersect<short>((IEnumerable<short>) modifiedIndexes).Count<short>() != ((IEnumerable<short>) fieldExecutionData2.WeakReferences).Count<short>())
                second.Add(weakDependency);
            }
          }
          if (((IEnumerable<short>) fieldExecutionData1.WeakReferences).Any<short>() && !((IEnumerable<short>) fieldExecutionData1.WeakDependencies).Intersect<short>((IEnumerable<short>) modifiedIndexes).Any<short>())
            second.Add(modifiedIndex);
          mergedExecutionList = mergedExecutionList.Union<short>((IEnumerable<short>) table.GetFieldExecutionData(modifiedIndex).ExecutionPlan).ToList<short>();
          fullInitializationList = fullInitializationList.Union<short>((IEnumerable<short>) table.GetFieldExecutionData(modifiedIndex).InitializationPlan).ToList<short>();
        }
      }
      foreach (short modifiedLockIndex in modifiedLockIndexes)
      {
        if (modifiedLockIndex >= (short) 0)
        {
          if (!mergedExecutionList.Contains(modifiedLockIndex))
          {
            mergedExecutionList.Add(modifiedLockIndex);
            mergedExecutionList.Sort();
          }
          mergedExecutionList = mergedExecutionList.Union<short>((IEnumerable<short>) table.GetFieldExecutionData(modifiedLockIndex).ExecutionPlan).ToList<short>();
          fullInitializationList = fullInitializationList.Union<short>((IEnumerable<short>) table.GetFieldExecutionData(modifiedLockIndex).InitializationPlan).ToList<short>();
        }
      }
      if (targetIndexes != null && targetIndexes.Any<short>())
      {
        foreach (short targetIndex in targetIndexes)
          shortList = shortList.Union<short>((IEnumerable<short>) table.GetFieldExecutionData(targetIndex).ReferencePlan).ToList<short>();
        if (!modifiedIndexes.Any<short>())
        {
          foreach (short index in shortList)
          {
            if (!((IEnumerable<short>) table.GetFieldExecutionData(index).WeakReferences).Any<short>())
              mergedExecutionList.Add(index);
          }
        }
        else
        {
          fullInitializationList = fullInitializationList.Intersect<short>((IEnumerable<short>) shortList).ToList<short>();
          mergedInitializationList = fullInitializationList.Except<short>((IEnumerable<short>) mergedExecutionList).ToList<short>();
          mergedExecutionList = mergedExecutionList.Except<short>((IEnumerable<short>) second).ToList<short>();
          mergedExecutionList = mergedExecutionList.Intersect<short>((IEnumerable<short>) shortList).ToList<short>();
        }
      }
      else
      {
        mergedInitializationList = fullInitializationList.Except<short>((IEnumerable<short>) mergedExecutionList).ToList<short>();
        mergedExecutionList = mergedExecutionList.Except<short>((IEnumerable<short>) second).ToList<short>();
      }
      mergedExecutionList = mergedExecutionList.Intersect<short>((IEnumerable<short>) table.CalculatedFields).ToList<short>();
      mergedExecutionList = mergedExecutionList.OrderBy<short, short>((Func<short, short>) (i => i)).ToList<short>();
      mergedInitializationList = mergedInitializationList.OrderBy<short, short>((Func<short, short>) (i => i)).ToList<short>();
      fullInitializationList = fullInitializationList.OrderBy<short, short>((Func<short, short>) (i => i)).ToList<short>();
    }

    public void Dispose() => this.Dispose(true);

    protected void Dispose(bool disposing)
    {
      if (this.calculationContext != null)
        ((ExecutionContext) this.calculationContext).Dispose();
      if (this.dataSource != null)
        this.dataSource.Dispose();
      if (this.calcQueue != null)
        this.calcQueue.Clear();
      if (this.skipCalcList == null)
        return;
      this.skipCalcList.Clear();
    }

    private List<short> BuildIndexListFromFieldWrappers(IEnumerable<DataFieldWrapper> fields)
    {
      List<short> shortList = new List<short>();
      if (fields != null)
      {
        foreach (DataFieldWrapper field in fields)
        {
          short descriptorIndex = this.executionPlanTable.GetDescriptorIndex(FieldDescriptor.Create(field.WrappedParentEntity.GetEntityType(), field.Id));
          shortList.Add(descriptorIndex);
        }
      }
      return shortList;
    }

    private string InitializeParameterizedEntities()
    {
      StringBuilder sbTrace = new StringBuilder();
      sbTrace.AppendLine("EngineCore | -- InitializeParameterizedEntities() Begin --");
      foreach (EntityDescriptor entityType in this.calculationSet.EntityTypes)
      {
        if (!entityType.IsBaseType())
          this.dataSource.GetAllEntitiesOfType(entityType, sbTrace);
      }
      sbTrace.AppendLine("EngineCore | -- InitializeParameterizedEntities() End --");
      return sbTrace.ToString();
    }

    private List<string> RunDependentCalculations<T>(
      List<DataFieldWrapper> targetFields,
      List<DataFieldWrapper> modifiedFields,
      List<DataFieldWrapper> lockedFields,
      List<DataFieldWrapper> unlockedFields,
      IEnumerable<Type> modifiedEntityTypes,
      T model,
      StringBuilder sbTrace = null)
    {
      List<short> targetIndexes = new List<short>();
      this.calculationContext.ModifiedFields = (IEnumerable<IDataField>) modifiedFields;
      if (sbTrace != null)
      {
        sbTrace.AppendLine("EngineCore | -- RunDependentCalculations() Begin --");
        sbTrace.Append(this.LogDataSourceValues());
        sbTrace.Append(this.InitializeParameterizedEntities());
      }
      else
        this.InitializeParameterizedEntities();
      if (targetFields != null && targetFields.Any<DataFieldWrapper>())
      {
        foreach (DataFieldWrapper targetField in targetFields)
        {
          FieldDescriptor descriptor = FieldDescriptor.Create(targetField.WrappedParentEntity.GetEntityType(), targetField.Id);
          targetIndexes.Add(this.executionPlanTable.GetDescriptorIndex(descriptor));
        }
      }
      this._modifiedIndexes = this.BuildIndexListFromFieldWrappers((IEnumerable<DataFieldWrapper>) modifiedFields);
      if (modifiedEntityTypes != null)
      {
        foreach (Type modifiedEntityType in modifiedEntityTypes)
          this._modifiedIndexes.AddRange((IEnumerable<short>) this.executionPlanTable.GetAllMemberFieldIndexes(EntityDescriptor.Create(modifiedEntityType)));
      }
      this._valueChangedList.AddRange((IEnumerable<short>) this._modifiedIndexes);
      List<short> modifiedLockIndexes = this.BuildIndexListFromFieldWrappers(lockedFields.Union<DataFieldWrapper>((IEnumerable<DataFieldWrapper>) unlockedFields));
      List<short> mergedExecutionList;
      List<short> mergedInitializationList;
      List<short> fullInitializationList;
      Elli.CalculationEngine.Core.EngineCore.EngineCore.BuildMergedExecutionPlan(this.executionPlanTable, targetIndexes, this._modifiedIndexes, modifiedLockIndexes, out mergedExecutionList, out mergedInitializationList, out fullInitializationList);
      List<string> stringList1 = new List<string>();
      List<string> stringList2;
      try
      {
        stringList2 = this.InitializeTransients<T>(mergedInitializationList, model);
        stringList2.AddRange((IEnumerable<string>) this.ExecuteDependentCalculations<T>(mergedExecutionList, fullInitializationList, model));
      }
      catch (Exception ex)
      {
        throw new Exception(this.GenerateExceptionMessage(string.Format("Error in RunDependentCalculations. Model: {0} ", (object) model.GetHashCode()), sbTrace), ex);
      }
      if (sbTrace != null)
      {
        sbTrace.AppendLine(string.Format("RunDependentCalculations. Model: {0} ", (object) model.GetHashCode()));
        sbTrace.Append(this.dataSource.CalculationLog);
        sbTrace.AppendLine("EngineCore | -- RunDependentCalculations() End --");
      }
      return stringList2;
    }

    private List<string> ExecuteDependentCalculations<T>(
      List<short> calculationList,
      List<short> initializationList,
      T model)
    {
      List<string> stringList = new List<string>();
      foreach (short index in calculationList.OrderBy<short, short>((Func<short, short>) (i => i)).ToArray<short>())
      {
        bool flag1 = false;
        if (this._enableShortCircuiting && !this._modifiedIndexes.Contains(index))
          flag1 = !this._valueChangedList.Intersect<short>((IEnumerable<short>) this.executionPlanTable.GetFieldExecutionData(index).ReferencedCalculations).Any<short>() && !initializationList.Contains(index);
        if (!flag1)
        {
          FieldExecutionData fieldExecutionData = this.executionPlanTable.GetFieldExecutionData(index);
          if (fieldExecutionData != null)
          {
            stringList.Add(fieldExecutionData.Descriptor.ToString());
            bool flag2 = false;
            foreach (DataEntityWrapper dataEntityWrapper in this.dataSource.GetAllWrappedEntitiesOfType(fieldExecutionData.Descriptor.ParentEntityType))
            {
              DataFieldWrapper wrappedField = dataEntityWrapper.GetWrappedField(fieldExecutionData.Descriptor.FieldId);
              if (wrappedField != null)
              {
                if (!wrappedField.IsNull())
                {
                  try
                  {
                    object oldVal = wrappedField.GetValue();
                    bool flag3 = this.ExecuteCalculation<T>(wrappedField, oldVal, true, out object _, model);
                    flag2 |= flag3;
                  }
                  catch (Exception ex)
                  {
                    Tracing.Log(TraceLevel.Error, nameof (EngineCore), string.Format("Error Calculating field [{0}]; CalcId = {1}: {2}", (object) wrappedField.GetQualifiedName(), (object) wrappedField.CalcId, (object) ex.Message));
                    throw new Exception(string.Format("Error Calculating field [{0}]; CalcId = {1}: {2}", (object) wrappedField.GetQualifiedName(), (object) wrappedField.CalcId, (object) ex.Message));
                  }
                }
              }
            }
            if (flag2)
              this._valueChangedList.Add(index);
          }
        }
        else
          Tracing.Log(TraceLevel.Verbose, nameof (EngineCore), string.Format("Skipping calcs for {0} - No references have been modified.", (object) this.executionPlanTable.GetFieldExecutionData(index).Descriptor));
      }
      return stringList;
    }

    private List<string> InitializeTransients<T>(List<short> initializeTransientsList, T model)
    {
      List<string> stringList = new List<string>();
      foreach (short index in initializeTransientsList.OrderBy<short, short>((Func<short, short>) (i => i)).ToArray<short>())
      {
        FieldExecutionData fieldExecutionData = this.executionPlanTable.GetFieldExecutionData(index);
        if (fieldExecutionData != null)
        {
          if (!EngineUtility.IsTransientField(fieldExecutionData.Descriptor.FieldId))
            throw new Exception(string.Format("Non-transient field [{0}] found in transient initialization list", (object) fieldExecutionData.Descriptor.ToString()));
          bool flag1 = false;
          foreach (DataEntityWrapper dataEntityWrapper in this.dataSource.GetAllWrappedEntitiesOfType(fieldExecutionData.Descriptor.ParentEntityType))
          {
            DataFieldWrapper wrappedField = dataEntityWrapper.GetWrappedField(fieldExecutionData.Descriptor.FieldId);
            if (wrappedField != null && !wrappedField.IsNull())
            {
              object oldVal = wrappedField.GetValue();
              if (oldVal == TransientField.NotSet)
              {
                try
                {
                  bool flag2 = this.ExecuteCalculation<T>(wrappedField, oldVal, true, out object _, model);
                  flag1 |= flag2;
                }
                catch (Exception ex)
                {
                  Tracing.Log(TraceLevel.Error, nameof (EngineCore), string.Format("Error Calculating Transient field [{0}]; CalcId = {1}: {2}", (object) wrappedField.GetQualifiedName(), (object) wrappedField.CalcId, (object) ex.Message));
                  throw new Exception(string.Format("Error Calculating Transient field [{0}]; CalcId = {1}: {2}", (object) wrappedField.GetQualifiedName(), (object) wrappedField.CalcId, (object) ex.Message));
                }
              }
            }
          }
          if (flag1)
          {
            stringList.Add(fieldExecutionData.Descriptor.ToString());
            this._valueChangedList.Add(index);
          }
        }
      }
      return stringList;
    }

    private IEnumerable<DataEntityWrapper> GetRelatedEntitiesFromDataSource(
      DataEntityWrapper rootEntity,
      ReferencedElement baseRootElement)
    {
      if (this._debugLogCalcForMod)
        Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("Getting Related Entities from DataSource: {0} - {1}", (object) rootEntity.GetEntityType(), (object) baseRootElement.QualifiedName));
      List<DataEntityWrapper> dataEntityWrapperList1 = new List<DataEntityWrapper>();
      List<DataEntityWrapper> source = new List<DataEntityWrapper>();
      if (rootEntity == null)
        return (IEnumerable<DataEntityWrapper>) source;
      if (baseRootElement == null)
        rootEntity = (DataEntityWrapper) this.dataSource;
      try
      {
        if (baseRootElement != null)
        {
          ReferencedElement referencedElement = baseRootElement;
          if (referencedElement.Name == CalculationUtility.ALL_ENTITIES)
          {
            source.AddRange(this.dataSource.GetAllWrappedEntitiesOfType(referencedElement.EntityType));
          }
          else
          {
            if (rootEntity.GetEntityType() == baseRootElement.EntityType)
              dataEntityWrapperList1.Add(rootEntity);
            else if (referencedElement.DataElementType == DataElementType.DataEntityCollectionType)
            {
              dataEntityWrapperList1.AddRange(rootEntity.GetRelatedWrappedEntities(referencedElement.Name, referencedElement.EntityType));
            }
            else
            {
              DataEntityWrapper relatedWrappedEntity = rootEntity.GetRelatedWrappedEntity(referencedElement.Name);
              if (!relatedWrappedEntity.IsNull())
                dataEntityWrapperList1.Add(relatedWrappedEntity);
            }
            source = dataEntityWrapperList1;
          }
          ReferencedElement subElement = referencedElement;
          while (subElement != null)
          {
            if (subElement.DataElementType != DataElementType.DataFieldType)
            {
              if (subElement.ReferencedElements.Any<ReferencedElement>())
              {
                subElement = subElement.ReferencedElements.First<ReferencedElement>();
                if (subElement.DataElementType != DataElementType.DataFieldType)
                {
                  IEnumerable<DataEntityWrapper> collection = !(subElement.Name == CalculationUtility.ALL_ENTITIES) ? (subElement.DataElementType != DataElementType.DataEntityCollectionType ? source.Where<DataEntityWrapper>((Func<DataEntityWrapper, bool>) (p => !p.IsNull())).Select<DataEntityWrapper, DataEntityWrapper>((Func<DataEntityWrapper, DataEntityWrapper>) (p => p.GetRelatedWrappedEntity(subElement.Name))).Where<DataEntityWrapper>((Func<DataEntityWrapper, bool>) (p => !p.IsNull())) : source.Where<DataEntityWrapper>((Func<DataEntityWrapper, bool>) (p => !p.IsNull())).SelectMany<DataEntityWrapper, DataEntityWrapper>((Func<DataEntityWrapper, IEnumerable<DataEntityWrapper>>) (p => p.GetRelatedWrappedEntities(subElement.Name, subElement.EntityType))).Where<DataEntityWrapper>((Func<DataEntityWrapper, bool>) (p => !p.IsNull()))) : this.dataSource.GetAllWrappedEntitiesOfType(subElement.EntityType);
                  List<DataEntityWrapper> dataEntityWrapperList2 = new List<DataEntityWrapper>();
                  if (collection != null)
                    dataEntityWrapperList2.AddRange(collection);
                  source = dataEntityWrapperList2;
                }
              }
              else
                subElement = (ReferencedElement) null;
            }
            else
              break;
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("GetRelatedEntitiesFromDataSource - Root = {0}, QualifiedName = {1}.", (object) rootEntity.GetEntityType(), (object) baseRootElement.QualifiedName), ex);
      }
      return (IEnumerable<DataEntityWrapper>) source;
    }

    private bool ExecuteCalculation<T>(
      DataFieldWrapper field,
      object oldVal,
      bool checkFieldChanged,
      out object calculatedVal,
      T model)
    {
      bool flag1 = false;
      calculatedVal = this.GetCalculatedValue<T>(field, model);
      bool flag2 = checkFieldChanged && (this.calculationContext.NoOperation || object.Equals(calculatedVal, oldVal));
      if (this.calculationContext.UnlockBeforeSetValue)
      {
        field.Unlock();
        field.SetValue(calculatedVal);
      }
      else if (!flag2)
        field.SetValue(calculatedVal);
      if (this.calculationContext.LockAfterSetValue)
        field.Lock();
      if (!flag2 & checkFieldChanged)
      {
        object objB = field.GetValue();
        flag1 = !object.Equals(oldVal, objB);
      }
      return flag1;
    }

    public bool IsCalculated(IDataField field)
    {
      FieldState fieldState = FieldState.Unknown;
      if (field != null && field.ParentEntity != null && !this.fieldStateDictionary.TryGetValue(this.GetFieldCode(field), out fieldState))
        fieldState = FieldState.Unknown;
      return fieldState == FieldState.Calculated;
    }

    public FieldState GetFieldState(IDataField field)
    {
      FieldState fieldState = FieldState.Unknown;
      if (field != null && field.ParentEntity != null && !this.fieldStateDictionary.TryGetValue(this.GetFieldCode(field), out fieldState))
        fieldState = FieldState.Unknown;
      return fieldState;
    }

    public void SetFieldState(IDataField field, FieldState state)
    {
      if (field == null || field.ParentEntity == null)
        return;
      long fieldCode = this.GetFieldCode(field);
      if (this.fieldStateDictionary.ContainsKey(fieldCode))
        this.fieldStateDictionary[fieldCode] = state;
      else
        this.fieldStateDictionary.Add(fieldCode, state);
    }

    public void ResetAllFieldStates(FieldState state)
    {
      foreach (long key in this.fieldStateDictionary.Keys.ToList<long>())
        this.fieldStateDictionary[key] = state;
    }

    private long GetFieldCode(IDataField ef)
    {
      return ((long) ef.Id.GetHashCode() << 32) + (ef.ParentEntity == null ? 34671L : (long) ef.ParentEntity.GetHashCode());
    }

    public bool IsRelationshipVisited(DependencyNode node, string relationship)
    {
      bool flag = false;
      HashSet<string> stringSet;
      this.visitedDictionary.TryGetValue(node.GetFullName(), out stringSet);
      if (stringSet != null)
        flag = stringSet.Contains(relationship) || stringSet.Contains(CalculationUtility.ALL_ENTITIES);
      return flag;
    }

    public bool IsRelationshipVisited(DependencyNode node)
    {
      bool flag = false;
      HashSet<string> stringSet;
      this.visitedDictionary.TryGetValue(node.GetFullName(), out stringSet);
      if (stringSet != null)
        flag = stringSet.Contains(CalculationUtility.ALL_ENTITIES);
      return flag;
    }

    public void AddVisitedRelationship(DependencyNode node, string relationship)
    {
      string fullName = node.GetFullName();
      HashSet<string> stringSet1;
      this.visitedDictionary.TryGetValue(fullName, out stringSet1);
      if (stringSet1 == null)
      {
        HashSet<string> stringSet2 = new HashSet<string>()
        {
          relationship
        };
        this.visitedDictionary.Add(fullName, stringSet2);
      }
      else
      {
        if (stringSet1.Contains(relationship))
          return;
        stringSet1.Add(relationship);
      }
    }

    public void SetAllVisited(DependencyNode node)
    {
      this.AddVisitedRelationship(node, CalculationUtility.ALL_ENTITIES);
    }

    public void ClearAllVisited() => this.visitedDictionary.Clear();

    private string GenerateExceptionMessage(string message, StringBuilder sbTrace = null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(message);
      stringBuilder.AppendLine("-- Begin Trace Log --");
      if (sbTrace != null)
        stringBuilder.Append(sbTrace.ToString());
      stringBuilder.AppendLine("-- End Trace Log --");
      stringBuilder.AppendLine("-- Begin Calculation Log --");
      stringBuilder.Append(this.dataSource.CalculationLog);
      stringBuilder.AppendLine("-- End Calculation Log --");
      return stringBuilder.ToString();
    }

    private string LogCalculatedResults(
      FieldDescriptor desc,
      object oldVal,
      object newVal,
      bool isFieldChanged)
    {
      return this.LogCalculatedResults(desc.ToString(), oldVal, newVal, isFieldChanged);
    }

    private string LogCalculatedResults(
      DataFieldWrapper queuedField,
      object oldVal,
      object newVal,
      bool isFieldChanged)
    {
      return this.LogCalculatedResults(queuedField.GetQualifiedName(), oldVal, newVal, isFieldChanged);
    }

    private string LogCalculatedResults(
      string fieldName,
      object oldVal,
      object newVal,
      bool isFieldChanged)
    {
      string str1 = newVal != null ? newVal.GetType().Name : "Field";
      string str2 = isFieldChanged ? "changed" : "UNCHANGED";
      string str3 = string.Format("{0} value {1} for {2}", (object) str1, (object) str2, (object) fieldName);
      if ((newVal == null || !(newVal.GetType().Namespace != "System")) && (oldVal == null || !(oldVal.GetType().Namespace != "System")))
        str3 = string.Format("{0} value {1} for {2}", (object) str1, (object) str2, (object) fieldName) + string.Format(" from {0} to {1}", oldVal == null ? (object) "Null" : oldVal, newVal == null ? (object) "Null" : newVal);
      return str3;
    }

    private string LogSystemObjectProperties(object initial, object result)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (initial == null)
      {
        if (result != null)
          stringBuilder.Append("Initial = NULL ; Result = ").Append(result).AppendLine();
      }
      else if (initial.GetType().Namespace == "System")
        stringBuilder.Append("Initial = ").Append(initial).Append(" ; ").Append("Result = ").Append(result == null ? (object) "NULL" : result).AppendLine();
      return stringBuilder.ToString();
    }

    private string LogObjectArrayProperties(IList initList, IList resultList)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!Utility.IsObjectMatch((object) initList, (object) resultList))
      {
        int count1 = initList != null ? initList.Count : 0;
        int count2 = resultList != null ? resultList.Count : 0;
        stringBuilder.Append("Array Initial Count [").Append(count1).Append("] ; Result Count[").Append(count2).AppendLine("]");
        if (count1 == count2)
        {
          for (int index = 0; index < initList.Count; ++index)
          {
            object init = initList[index];
            object result = resultList[index];
            stringBuilder.Append(this.LogObjectFields(init, result));
          }
        }
      }
      return stringBuilder.ToString();
    }

    private string LogObjectListProperties(IEnumerable<object> initial, IEnumerable<object> result)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!Utility.IsObjectMatch((object) initial, (object) result))
      {
        int num1 = initial != null ? initial.Count<object>() : 0;
        int num2 = result != null ? result.Count<object>() : 0;
        stringBuilder.Append("IEnumerable<object> Initial Count [").Append(num1).Append("] ; Result Count[").Append(num2).AppendLine("]");
        if (num1 == num2)
        {
          for (int index = 0; index < num1; ++index)
          {
            object initial1 = initial != null ? initial.ElementAt<object>(index) : (object) null;
            object result1 = result != null ? result.ElementAt<object>(index) : (object) null;
            stringBuilder.Append(this.LogObjectFields(initial1, result1));
          }
        }
      }
      return stringBuilder.ToString();
    }

    private string LogObjectDictionaryListProperties(
      Dictionary<string, List<object>> initial,
      Dictionary<string, List<object>> result)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!Utility.IsObjectMatch((object) initial, (object) result))
      {
        Dictionary<string, List<object>>.KeyCollection keyCollection = initial != null ? initial.Keys : result.Keys;
        int num1 = initial != null ? initial.Count<KeyValuePair<string, List<object>>>() : 0;
        int num2 = result != null ? result.Count<KeyValuePair<string, List<object>>>() : 0;
        stringBuilder.Append("Dictionary<string, List<object>> Initial Count [").Append(num1).Append("] ; Result Count[").Append(num2).AppendLine("]");
        if (num1 == num2)
        {
          foreach (string key in keyCollection)
          {
            List<object> initial1 = (List<object>) null;
            List<object> result1 = (List<object>) null;
            initial?.TryGetValue(key, out initial1);
            result?.TryGetValue(key, out result1);
            stringBuilder.Append("[").Append(key).Append("]").Append(this.LogObjectFields((object) initial1, (object) result1));
          }
        }
      }
      return stringBuilder.ToString();
    }

    private string LogObjectDictionaryProperties(
      Dictionary<string, object> initial,
      Dictionary<string, object> result)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!Utility.IsObjectMatch((object) initial, (object) result))
      {
        if (initial == null)
        {
          Dictionary<string, object>.KeyCollection keys1 = result.Keys;
        }
        else
        {
          Dictionary<string, object>.KeyCollection keys2 = initial.Keys;
        }
        int num1 = initial != null ? initial.Count<KeyValuePair<string, object>>() : 0;
        int num2 = result != null ? result.Count<KeyValuePair<string, object>>() : 0;
        stringBuilder.Append("Dictionary<string, object> Initial Count [").Append(num1).Append("] ; Result Count[").Append(num2).AppendLine("]");
        if (num1 == num2)
        {
          foreach (string key in initial.Keys)
          {
            object initial1 = (object) null;
            object result1 = (object) null;
            initial?.TryGetValue(key, out initial1);
            result?.TryGetValue(key, out result1);
            stringBuilder.Append("[").Append(key).Append("]").Append(this.LogObjectFields(initial1, result1));
          }
        }
      }
      return stringBuilder.ToString();
    }

    private string LogObjectFields(object initial, object result)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (initial != null && initial.GetType().Namespace == "System" && result != null && result.GetType().Namespace == "System")
      {
        stringBuilder.Append(this.LogSystemObjectProperties(initial, result));
      }
      else
      {
        int num = 0;
        if (initial != null)
        {
          IEnumerable<\u003C\u003Ef__AnonymousType0<string, object>> source1 = ((IEnumerable<FieldInfo>) initial.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)).Select(field => new
          {
            Name = field.Name,
            Value = field.GetValue(initial)
          });
          IEnumerable<\u003C\u003Ef__AnonymousType0<string, object>> source2 = ((IEnumerable<FieldInfo>) result.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)).Select(field => new
          {
            Name = field.Name,
            Value = field.GetValue(result)
          });
          num = source1.Count();
          if (num > 0)
          {
            stringBuilder.AppendLine(string.Format("{0} - Count:{1}", (object) initial.GetType().ToString(), (object) num));
            for (int index = 0; index < source1.Count(); ++index)
            {
              var data1 = source1.ElementAt(index);
              var data2 = source2.ElementAt(index);
              object obj1 = data1.Value;
              object obj2 = data2.Value;
              if (!Utility.IsObjectMatch(obj1, obj2))
                stringBuilder.Append(data1.Name).Append(": ").Append(this.LogObject(obj1, obj2));
            }
          }
        }
        if (num == 0)
          stringBuilder.Append(this.LogObject(initial, result));
      }
      return stringBuilder.ToString();
    }

    public static string ObjectToString(object obj)
    {
      return Elli.CalculationEngine.Core.EngineCore.EngineCore.ByteArrayToString(Elli.CalculationEngine.Core.EngineCore.EngineCore.ObjectToByteArray(obj));
    }

    public static string ByteArrayToString(byte[] array)
    {
      string str = string.Empty;
      try
      {
        bool flag = false;
        foreach (byte num in array)
        {
          char c = (char) num;
          if (char.IsLetterOrDigit(c) || char.IsPunctuation(c))
          {
            if (!flag)
              str += "\n";
            str += c.ToString();
            flag = true;
          }
          else
          {
            if (flag)
              str += "\n";
            str = str + (object) (int) num + ";";
            flag = false;
          }
        }
      }
      catch
      {
      }
      return str;
    }

    public static byte[] ObjectToByteArray(object obj)
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      using (MemoryStream serializationStream = new MemoryStream())
      {
        byte[] byteArray = new byte[0];
        try
        {
          binaryFormatter.Serialize((Stream) serializationStream, obj);
          byteArray = serializationStream.ToArray();
        }
        catch
        {
        }
        return byteArray;
      }
    }

    private string LogObject(object initial, object result)
    {
      StringBuilder stringBuilder = new StringBuilder();
      object obj = result != null ? initial : result;
      if (obj != null)
      {
        Type type = obj.GetType();
        if (typeof (IEnumerable<object>).IsAssignableFrom(type))
          stringBuilder.Append(this.LogObjectListProperties((IEnumerable<object>) initial, (IEnumerable<object>) result));
        else if (type.IsArray)
        {
          stringBuilder.Append(this.LogObjectArrayProperties(initial as IList, result as IList));
        }
        else
        {
          switch (obj)
          {
            case Dictionary<string, List<object>> _:
              stringBuilder.Append(this.LogObjectDictionaryListProperties((Dictionary<string, List<object>>) initial, (Dictionary<string, List<object>>) result));
              break;
            case Dictionary<string, object> _:
              stringBuilder.Append(this.LogObjectDictionaryProperties((Dictionary<string, object>) initial, (Dictionary<string, object>) result));
              break;
            default:
              stringBuilder.Append(this.LogSystemObjectProperties((object) Elli.CalculationEngine.Core.EngineCore.EngineCore.ObjectToString(initial), (object) Elli.CalculationEngine.Core.EngineCore.EngineCore.ObjectToString(result)));
              break;
          }
        }
      }
      else
        stringBuilder.Append(this.LogSystemObjectProperties(initial, result));
      return stringBuilder.ToString();
    }

    private string LogDataSourceValues()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this._debugLogDataSourceValues)
      {
        stringBuilder.AppendLine("EngineCore | -- LogDataSourceValues() Begin --");
        foreach (string field in this.dataSource.GetFieldList())
        {
          object obj = (object) null;
          try
          {
            obj = this.dataSource.GetFieldValue(field);
          }
          catch
          {
            stringBuilder.AppendLine("EngineCore | " + string.Format("[{0}] not in DataSource {1}", (object) field, (object) this.dataSource.GetEntityName()));
          }
          finally
          {
            stringBuilder.AppendLine("EngineCore | " + string.Format("[{0}] Start Value:{1}", (object) field, obj != null ? (object) obj.ToString() : (object) string.Empty));
            stringBuilder.AppendLine(this.LogObjectFields(obj != null ? (object) obj.ToString() : (object) string.Empty, (object) "NA"));
          }
        }
        stringBuilder.AppendLine("EngineCore | -- LogDataSourceValues() End --");
      }
      return stringBuilder.ToString();
    }

    private void LogDependencyDiagram()
    {
      if (!Elli.CalculationEngine.Core.EngineCore.EngineCore.traceSwitch.TraceVerbose || !this.buildDependencyDiagramString)
        return;
      Tracing.Log(TraceLevel.Verbose, nameof (EngineCore), "--Execution Tree Begin--");
      Tracing.Log(TraceLevel.Verbose, nameof (EngineCore), this.dependencyDiagramWriter.ToString());
      this.dependencyDiagramWriter.Flush();
      Tracing.Log(TraceLevel.Verbose, nameof (EngineCore), "--Execution Tree End--");
    }

    private void WriteDependencyDiagramLine(string line)
    {
      if (!Elli.CalculationEngine.Core.EngineCore.EngineCore.traceSwitch.TraceVerbose || !this.buildDependencyDiagramString)
        return;
      this.dependencyDiagramWriter.WriteLine("{0}{1}", (object) "".PadLeft(this.indentation * 4), (object) line);
    }

    private void LogReferences(ReferencedElement reference)
    {
      if (reference.DataElementType == DataElementType.DataFieldType)
      {
        Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format(" - references {{{0}}}.[{1}]", (object) reference.ParentElement.EntityType.ToString(), (object) reference.Name));
      }
      else
      {
        foreach (ReferencedElement referencedElement in reference.ReferencedElements)
          this.LogReferences(referencedElement);
      }
    }

    private void StartPerformanceLogging()
    {
      if (!this._debugCalculationPerformance)
        return;
      this.transCount = 0;
      this.fieldCount = 0;
      this.transTotal = TimeSpan.Zero;
      this.fieldTotal = TimeSpan.Zero;
      this.longestTransientCalc = string.Empty;
      this.longestTransientTime = TimeSpan.Zero;
      this.longestFieldCalc = string.Empty;
      this.longestFieldTime = TimeSpan.Zero;
      this.sw.Restart();
    }

    private void EndPerformanceLogging(string methodName)
    {
      if (!this._debugCalculationPerformance)
        return;
      this.sw.Stop();
      TimeSpan timeSpan1 = this.transCount > 0 ? new TimeSpan(this.transTotal.Ticks / (long) this.transCount) : this.transTotal;
      TimeSpan timeSpan2 = this.fieldCount > 0 ? new TimeSpan(this.transTotal.Ticks / (long) this.fieldCount) : this.fieldTotal;
      Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("{0} Duration: {1}", (object) methodName, (object) this.sw.Elapsed));
      Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("  Total Calculation Time: {0}", (object) (this.transTotal + this.fieldTotal)));
      Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("  Total Transient Calculations: {0}; Duration: {1}; Average:{2}", (object) this.transCount, (object) this.transTotal, (object) timeSpan1));
      Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("  Total Field Calculations: {0}; Duration: {1}; Average:{2}", (object) this.fieldCount, (object) this.fieldTotal, (object) timeSpan2));
      Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("  Longest Transient Calculation = {0}; Duration: {1}", (object) this.longestTransientCalc, (object) this.longestTransientTime));
      Tracing.Log(TraceLevel.Info, nameof (EngineCore), string.Format("  Longest Field Calculation = {0}; Duration: {1}", (object) this.longestFieldCalc, (object) this.longestFieldTime));
      try
      {
        lock (Elli.CalculationEngine.Core.EngineCore.EngineCore._logSyncObject)
        {
          StreamWriter streamWriter1 = File.AppendText("CalcEnginePerformance.log");
          streamWriter1.WriteLine(string.Format("{0} Duration: {1}", (object) methodName, (object) this.sw.Elapsed));
          streamWriter1.WriteLine(string.Format("  Total Calculation Time: {0}", (object) (this.transTotal + this.fieldTotal)));
          streamWriter1.WriteLine(string.Format("  Total Transient Calculations: {0}; Duration: {1}; Average:{2}", (object) this.transCount, (object) this.transTotal, (object) timeSpan1));
          streamWriter1.WriteLine(string.Format("  Total Field Calculations: {0}; Duration: {1}; Average:{2}", (object) this.fieldCount, (object) this.fieldTotal, (object) timeSpan2));
          streamWriter1.WriteLine(string.Format("  Longest Transient Calculation = {0}; Duration: {1}", (object) this.longestTransientCalc, (object) this.longestTransientTime));
          streamWriter1.WriteLine(string.Format("  Longest Field Calculation = {0}; Duration: {1}", (object) this.longestFieldCalc, (object) this.longestFieldTime));
          streamWriter1.Close();
          StreamWriter streamWriter2 = File.AppendText("CalcEnginePerformance.csv");
          streamWriter2.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", (object) this.sw.Elapsed.TotalMilliseconds, (object) (this.transTotal.TotalMilliseconds + this.fieldTotal.TotalMilliseconds), (object) this.transCount, (object) this.transTotal.TotalMilliseconds, (object) timeSpan1.TotalMilliseconds, (object) this.fieldCount, (object) this.fieldTotal.TotalMilliseconds, (object) timeSpan2.TotalMilliseconds, (object) this.longestTransientCalc.Replace(",", ";"), (object) this.longestTransientTime.TotalMilliseconds, (object) this.longestFieldCalc.Replace(",", ";"), (object) this.longestFieldTime.TotalMilliseconds));
          streamWriter2.Close();
        }
      }
      catch
      {
      }
    }
  }
}
