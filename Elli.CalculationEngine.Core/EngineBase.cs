// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.EngineBase
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.CalculationLibrary;
using Elli.CalculationEngine.Core.DataSource;
using Elli.CalculationEngine.Core.EngineCore;
using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public class EngineBase : IDisposable
  {
    private Elli.CalculationEngine.Core.EngineCore.EngineCore engineCore;
    private const string calculationSetFilename = "CalculationSet.xml";
    private static int indentation = 0;
    private static StringBuilder sb = new StringBuilder();
    private static int depCount = 0;
    private static string loadedCalculationSetDllPath = string.Empty;
    private static ConcurrentDictionary<Type, CalculationSet> calcDictionary = new ConcurrentDictionary<Type, CalculationSet>();
    private static ConcurrentDictionary<Type, DependencyGraph> graphDictionary = new ConcurrentDictionary<Type, DependencyGraph>();
    private static ConcurrentDictionary<Type, ExecutionDataTable> tableDictionary = new ConcurrentDictionary<Type, ExecutionDataTable>();
    protected Assembly callingAssembly;

    public IDataSource DataSource
    {
      get => this.engineCore.DataSource;
      private set
      {
      }
    }

    public EngineBase()
    {
    }

    public EngineBase(string fullAssemblyPath)
    {
    }

    public void Initialize<T>(
      IDataSource dataSource,
      T model,
      bool initializeTransients = true,
      List<Tuple<string, string>> skipCalcList = null)
    {
      this.engineCore.Initialize<T>(dataSource, model, initializeTransients, skipCalcList, this.callingAssembly);
    }

    public object GetCalculatedValue<T>(IDataField field, T model)
    {
      return this.engineCore.GetCalculatedValue<T>(DataFieldWrapper.Create(field, (DataSourceWrapper) this.DataSource), model);
    }

    public IEnumerable<string> CalculateFieldAndDependencies<T>(
      IDataField field,
      T model,
      bool forceAll = false)
    {
      this.engineCore.ClearEntityCache();
      return this.engineCore.CalculateFieldAndDependencies<T>(DataFieldWrapper.Create(field, (DataSourceWrapper) this.DataSource), model);
    }

    public IEnumerable<string> CalculateDependenciesForModifiedFields<T>(IDataField field, T model)
    {
      this.engineCore.ClearEntityCache();
      List<DataFieldWrapper> modifiedFields = new List<DataFieldWrapper>();
      DataFieldWrapper dataFieldWrapper = DataFieldWrapper.Create(field, (DataSourceWrapper) this.DataSource);
      dataFieldWrapper.WrappedParentEntity.SetFieldValue(dataFieldWrapper.Id, field.GetValue());
      modifiedFields.Add(dataFieldWrapper);
      List<DataFieldWrapper> lockedFields = new List<DataFieldWrapper>();
      List<DataFieldWrapper> unlockedFields = new List<DataFieldWrapper>();
      return this.engineCore.CalculateDependenciesForModifiedFields<T>(modifiedFields, lockedFields, unlockedFields, (IEnumerable<Type>) null, model);
    }

    public IEnumerable<string> CalculateDependenciesForModifiedFields<T>(
      IEnumerable<IDataField> modifiedFields,
      T model)
    {
      this.engineCore.ClearEntityCache();
      return this.engineCore.CalculateDependenciesForModifiedFields<T>(this.WrapFieldList(modifiedFields), new List<DataFieldWrapper>(), new List<DataFieldWrapper>(), (IEnumerable<Type>) null, model);
    }

    public IEnumerable<string> CalculateDependenciesForModifiedFields<T>(
      IEnumerable<IDataField> modifiedFields,
      IEnumerable<Type> modifiedEntityTypes,
      T model)
    {
      this.engineCore.ClearEntityCache();
      return this.engineCore.CalculateDependenciesForModifiedFields<T>(this.WrapFieldList(modifiedFields), new List<DataFieldWrapper>(), new List<DataFieldWrapper>(), modifiedEntityTypes, model);
    }

    public IEnumerable<string> CalculateDependenciesForModifiedFields<T>(
      IEnumerable<IDataField> modifiedFields,
      IEnumerable<IDataField> lockedFields,
      IEnumerable<IDataField> unlockedFields,
      T model)
    {
      this.engineCore.ClearEntityCache();
      return this.engineCore.CalculateDependenciesForModifiedFields<T>(this.WrapFieldList(modifiedFields), this.WrapFieldList(lockedFields), this.WrapFieldList(unlockedFields), (IEnumerable<Type>) null, model);
    }

    public IEnumerable<string> CalculateDependenciesForModifiedFields<T>(
      IEnumerable<IDataField> modifiedFields,
      IEnumerable<IDataField> lockedFields,
      IEnumerable<IDataField> unlockedFields,
      IEnumerable<Type> modifiedEntityTypes,
      T model,
      StringBuilder sbTrace = null)
    {
      this.engineCore.ClearEntityCache();
      return this.engineCore.CalculateDependenciesForModifiedFields<T>(this.WrapFieldList(modifiedFields), this.WrapFieldList(lockedFields), this.WrapFieldList(unlockedFields), modifiedEntityTypes, model, sbTrace);
    }

    public object CalculateTargetFieldValue<T>(
      IDataField targetField,
      IEnumerable<IDataField> modifiedFields,
      T model)
    {
      this.engineCore.ClearEntityCache();
      List<DataFieldWrapper> modifiedFields1 = this.WrapFieldList(modifiedFields);
      List<DataFieldWrapper> lockedFields = new List<DataFieldWrapper>();
      List<DataFieldWrapper> unlockedFields = new List<DataFieldWrapper>();
      return this.engineCore.CalculateTargetFieldValue<T>(new List<DataFieldWrapper>()
      {
        DataFieldWrapper.Create(targetField, (DataSourceWrapper) this.DataSource)
      }, modifiedFields1, lockedFields, unlockedFields, model);
    }

    public object CalculateTargetFieldValue<T>(
      IDataField targetField,
      IEnumerable<IDataField> modifiedFields,
      IEnumerable<IDataField> lockedFields,
      IEnumerable<IDataField> unlockedFields,
      T model)
    {
      this.engineCore.ClearEntityCache();
      List<DataFieldWrapper> modifiedFields1 = this.WrapFieldList(modifiedFields);
      List<DataFieldWrapper> lockedFields1 = this.WrapFieldList(lockedFields);
      List<DataFieldWrapper> unlockedFields1 = this.WrapFieldList(unlockedFields);
      return this.engineCore.CalculateTargetFieldValue<T>(new List<DataFieldWrapper>()
      {
        DataFieldWrapper.Create(targetField, (DataSourceWrapper) this.DataSource)
      }, modifiedFields1, lockedFields1, unlockedFields1, model);
    }

    public object CalculateTargetFieldValue<T>(
      IEnumerable<IDataField> targetFields,
      IEnumerable<IDataField> modifiedFields,
      IEnumerable<IDataField> lockedFields,
      IEnumerable<IDataField> unlockedFields,
      T model)
    {
      this.engineCore.ClearEntityCache();
      List<DataFieldWrapper> modifiedFields1 = this.WrapFieldList(modifiedFields);
      List<DataFieldWrapper> lockedFields1 = this.WrapFieldList(lockedFields);
      List<DataFieldWrapper> unlockedFields1 = this.WrapFieldList(unlockedFields);
      return this.engineCore.CalculateTargetFieldValue<T>(this.WrapFieldList(targetFields), modifiedFields1, lockedFields1, unlockedFields1, model);
    }

    private List<DataFieldWrapper> WrapFieldList(IEnumerable<IDataField> fields)
    {
      List<DataFieldWrapper> dataFieldWrapperList = new List<DataFieldWrapper>();
      if (fields != null)
      {
        foreach (IDataField field in fields)
        {
          DataFieldWrapper dataFieldWrapper = DataFieldWrapper.Create(field, (DataSourceWrapper) this.DataSource);
          dataFieldWrapperList.Add(dataFieldWrapper);
        }
      }
      return dataFieldWrapperList;
    }

    public string CalculateAllFields<T>(T model)
    {
      this.engineCore.ClearEntityCache();
      return this.engineCore.CalculateAllFields<T>(model);
    }

    public IEnumerable<string> GetExecutionPlan<T>(IDataField field, T model)
    {
      Elli.CalculationEngine.Core.EngineCore.EngineCore engineCore = this.engineCore;
      List<DataFieldWrapper> modifiedFields = new List<DataFieldWrapper>();
      modifiedFields.Add(DataFieldWrapper.Create(field, (DataSourceWrapper) this.DataSource));
      T model1 = model;
      return engineCore.GetExecutionPlan<T>((IEnumerable<DataFieldWrapper>) modifiedFields, model1);
    }

    public IEnumerable<string> GetExecutionPlan<T>(IEnumerable<IDataField> modifiedFields, T model)
    {
      return this.engineCore.GetExecutionPlan<T>((IEnumerable<DataFieldWrapper>) this.WrapFieldList(modifiedFields), model);
    }

    public string GetDependencyDiagram(params string[] fieldIds) => string.Empty;

    public string GetElementDescriptor(string fieldId)
    {
      return this.engineCore.GetElementDescriptor(fieldId);
    }

    public string[] ListElements() => this.engineCore.ListElements();

    public void ExportDataSource(string exportFile) => this.engineCore.ExportDataSource(exportFile);

    public void ImportDataSource<T>(string importFile, T model)
    {
      this.engineCore.ImportDataSource<T>(importFile, model);
    }

    public IEnumerable<string> ValidateDataSource(bool includeFieldValidation = true)
    {
      return this.engineCore.ValidateDataSource(includeFieldValidation);
    }

    protected static ExecutionDataTable LoadExecutionDataTable(Assembly assembly, Type type)
    {
      ExecutionDataTable table;
      if (!EngineBase.tableDictionary.TryGetValue(type, out table))
      {
        table = (ExecutionDataTable) null;
        table = ExecutionDataTable.Deserialize(assembly.GetManifestResourceStream("ExecutionPlanTable.xml"));
        EngineBase.tableDictionary.AddOrUpdate(type, table, (Func<Type, ExecutionDataTable, ExecutionDataTable>) ((key, oldTable) => table));
      }
      return table;
    }

    protected static DependencyGraph LoadDependencyGraph(
      Assembly assembly,
      Type type,
      CalculationSet calcSet,
      ExecutionDataTable table)
    {
      DependencyGraph graph;
      if (!EngineBase.graphDictionary.TryGetValue(type, out graph))
      {
        DependencyGraphBuilder dependencyGraphBuilder = new DependencyGraphBuilder(table.FieldIndexDictionary, calcSet.GetAllFieldExpressionCalculations(), calcSet.RootEntityType, calcSet.RootRelationship);
        dependencyGraphBuilder.BuildGraph();
        graph = dependencyGraphBuilder.DependencyGraph;
        EngineBase.graphDictionary.AddOrUpdate(type, graph, (Func<Type, DependencyGraph, DependencyGraph>) ((key, oldGraph) => graph));
      }
      return graph;
    }

    protected static CalculationSet LoadCalculationSet(Assembly assembly, Type type)
    {
      EngineBase.LoadExecutionDataTable(assembly, type);
      Tracing.Log(TraceLevel.Info, type.Name, string.Format("EngineBase path: {0}; {1} path: {2}", (object) string.Empty, (object) assembly.FullName, (object) assembly.Location));
      CalculationSet calcSet;
      if (!EngineBase.calcDictionary.TryGetValue(type, out calcSet))
      {
        string name = "CalculationSet.xml.gz";
        GZipStream input = new GZipStream(assembly.GetManifestResourceStream(name), CompressionMode.Decompress);
        using (input)
        {
          calcSet = (CalculationSet) new DataContractSerializer(typeof (CalculationSet)).ReadObject(XmlReader.Create((Stream) input));
          CalculationUtility.SetEntityTypeList(calcSet.EntityTypes);
          calcSet.AddReadOnlyCalcs();
          foreach (LibraryElement element in calcSet.Elements)
            element.Identity.ParentId = calcSet.Identity.Id;
          EngineBase.calcDictionary.AddOrUpdate(type, calcSet, (Func<Type, CalculationSet, CalculationSet>) ((key, oldSet) => calcSet));
        }
      }
      return calcSet;
    }

    protected void SetUpEngine(string fullAssemblyPath = "")
    {
      Assembly callingAssembly = this.callingAssembly;
      Version version1 = callingAssembly.GetName().Version;
      Type type = this.GetType();
      if (string.IsNullOrEmpty(fullAssemblyPath))
        fullAssemblyPath = Assembly.GetAssembly(typeof (EngineBase)).Location;
      string directoryName = Path.GetDirectoryName(fullAssemblyPath);
      ExecutionDataTable executionDataTable = EngineBase.LoadExecutionDataTable(callingAssembly, type);
      CalculationSet calculationSet = EngineBase.LoadCalculationSet(callingAssembly, type);
      DependencyGraph dependencyGraph = EngineBase.LoadDependencyGraph(callingAssembly, type, calculationSet, executionDataTable);
      string calculationSetDllName = calculationSet.CalculationSetDllName;
      string str = Path.Combine(directoryName, calculationSetDllName);
      if (string.Compare(str, EngineBase.loadedCalculationSetDllPath, true) != 0)
      {
        Utility.AddAssemblyNameToAssemblyList(calculationSet.IdentityString);
        Version version2 = RuntimeContext.Current.LoadAssembly(calculationSet.IdentityString, str);
        if (version1 != version2)
        {
          string empty = string.Empty;
          throw new Exception(string.Format("Calculation Engine and Calculation Set versions do not match! {0} : Version {1}, {2} : Version {3}", (object) calculationSet.CalculationEngineDllName, (object) version1, (object) calculationSet.CalculationSetDllName, (object) version2));
        }
        EngineBase.loadedCalculationSetDllPath = str;
      }
      this.engineCore = new Elli.CalculationEngine.Core.EngineCore.EngineCore(calculationSet, dependencyGraph, executionDataTable);
    }

    protected static bool IsFieldCalculated(
      Type type,
      EntityDescriptor entity,
      string fieldId,
      bool includeWeakReferenceCalcs)
    {
      Assembly assembly = Assembly.GetAssembly(type);
      ExecutionDataTable table = EngineBase.LoadExecutionDataTable(assembly, type);
      CalculationSet calcSet = EngineBase.LoadCalculationSet(assembly, type);
      DependencyNode node = EngineBase.LoadDependencyGraph(assembly, type, calcSet, table).GetNode(fieldId, entity);
      return node != null && (includeWeakReferenceCalcs || !node.HasWeakReferences) && !node.ReadOnly;
    }

    protected static bool IsFieldCalculated(
      Type type,
      IDataField field,
      bool includeWeakReferenceCalcs)
    {
      return EngineBase.IsFieldCalculated(type, field.ParentEntity.GetEntityType(), field.Id, includeWeakReferenceCalcs);
    }

    protected static bool IsFieldUsedInCalculation(
      Type type,
      EntityDescriptor entity,
      string fieldId,
      bool ignoreEntityParameters)
    {
      bool flag = false;
      Assembly assembly = Assembly.GetAssembly(type);
      ExecutionDataTable table = EngineBase.LoadExecutionDataTable(assembly, type);
      CalculationSet calcSet = EngineBase.LoadCalculationSet(assembly, type);
      DependencyGraph dependencyGraph = EngineBase.LoadDependencyGraph(assembly, type, calcSet, table);
      if (dependencyGraph != null)
        flag = dependencyGraph.CalculationExists(entity, fieldId, ignoreEntityParameters);
      return flag;
    }

    public static bool IsFieldUsedInCalculation(
      Type type,
      IDataField field,
      bool ignoreEntityParameters)
    {
      return EngineBase.IsFieldUsedInCalculation(type, field.ParentEntity.GetEntityType(), field.Id, ignoreEntityParameters);
    }

    protected static bool IsFieldWeakReference(Type type, EntityDescriptor entity, string fieldId)
    {
      Assembly assembly = Assembly.GetAssembly(type);
      ExecutionDataTable table = EngineBase.LoadExecutionDataTable(assembly, type);
      CalculationSet calcSet = EngineBase.LoadCalculationSet(assembly, type);
      EngineBase.LoadDependencyGraph(assembly, type, calcSet, table);
      return ((IEnumerable<short>) table.GetFieldExecutionData(FieldDescriptor.Create(entity, fieldId)).WeakDependencies).Any<short>();
    }

    protected static bool HasWeakReference(Type type, EntityDescriptor entity, string fieldId)
    {
      return ((IEnumerable<short>) EngineBase.LoadExecutionDataTable(Assembly.GetAssembly(type), type).GetFieldExecutionData(FieldDescriptor.Create(entity, fieldId)).WeakReferences).Any<short>();
    }

    protected static List<FieldDescriptor> GetFieldReferences(
      Type type,
      EntityDescriptor entity,
      string fieldId)
    {
      List<FieldDescriptor> list = new List<FieldDescriptor>();
      Assembly assembly = Assembly.GetAssembly(type);
      ExecutionDataTable table = EngineBase.LoadExecutionDataTable(assembly, type);
      CalculationSet calcSet = EngineBase.LoadCalculationSet(assembly, type);
      DependencyGraph dependencyGraph = EngineBase.LoadDependencyGraph(assembly, type, calcSet, table);
      FieldDescriptor fieldDescriptor = new FieldDescriptor(entity, fieldId);
      string fieldId1 = fieldId;
      EntityDescriptor parentType = entity;
      DependencyNode node = dependencyGraph.GetNode(fieldId1, parentType);
      EngineBase.AddFieldReferences(ref list, calcSet, node);
      list.Remove(fieldDescriptor);
      EngineBase.sb.ToString();
      return list;
    }

    private static void AddFieldReferences(
      ref List<FieldDescriptor> list,
      CalculationSet calcSet,
      DependencyNode node)
    {
      ++EngineBase.indentation;
      foreach (KeyValuePair<ReferencedElement, DependencyNode> referencedRelationship in node.ReferencedRelationships)
      {
        EngineBase.sb.Append(string.Format("{0}{1}", (object) "".PadLeft(EngineBase.indentation * 4), (object) referencedRelationship.Value.FieldDescriptor));
        if (!list.Contains(referencedRelationship.Value.FieldDescriptor))
        {
          list.Add(referencedRelationship.Value.FieldDescriptor);
          EngineBase.AddFieldReferences(ref list, calcSet, referencedRelationship.Value);
        }
      }
      --EngineBase.indentation;
    }

    protected static List<FieldDescriptor> GetFieldDependencies(
      Type type,
      EntityDescriptor entity,
      string fieldId)
    {
      List<FieldDescriptor> list = new List<FieldDescriptor>();
      Assembly assembly = Assembly.GetAssembly(type);
      ExecutionDataTable table = EngineBase.LoadExecutionDataTable(assembly, type);
      CalculationSet calcSet = EngineBase.LoadCalculationSet(assembly, type);
      DependencyGraph dependencyGraph = EngineBase.LoadDependencyGraph(assembly, type, calcSet, table);
      FieldDescriptor fieldDescriptor = new FieldDescriptor(entity, fieldId);
      string fieldId1 = fieldId;
      EntityDescriptor parentType = entity;
      DependencyNode node = dependencyGraph.GetNode(fieldId1, parentType);
      EngineBase.AddFieldDependencies(ref list, calcSet, node);
      list.Remove(fieldDescriptor);
      EngineBase.sb.ToString();
      return list;
    }

    private static void AddFieldDependencies(
      ref List<FieldDescriptor> list,
      CalculationSet calcSet,
      DependencyNode node)
    {
      ++EngineBase.indentation;
      EngineBase.depCount = node.DependentRelationships.Count;
      foreach (KeyValuePair<ReferencedElement, DependencyNode> dependentRelationship in node.DependentRelationships)
      {
        EngineBase.sb.AppendLine(string.Format("{0}{1} - Parent:{2} - DepCount = {3}", (object) "".PadLeft(EngineBase.indentation * 4), (object) dependentRelationship.Value.FieldDescriptor, (object) node.FieldDescriptor, (object) dependentRelationship.Value.DependentRelationships.Count));
        FieldDescriptor fieldDescriptor = dependentRelationship.Value.FieldDescriptor;
        if (!list.Contains(fieldDescriptor))
        {
          list.Add(fieldDescriptor);
          EngineBase.AddFieldDependencies(ref list, calcSet, dependentRelationship.Value);
        }
      }
      --EngineBase.indentation;
    }

    protected static List<FieldDescriptor> GetAllFieldsDescriptors(Type type)
    {
      return EngineBase.LoadExecutionDataTable(Assembly.GetAssembly(type), type).FieldIndexDictionary.Keys.ToList<FieldDescriptor>();
    }

    protected static List<FieldDescriptor> GetMergedExecutionPlan(
      Type type,
      List<FieldDescriptor> modifiedFields,
      FieldDescriptor targetField)
    {
      return EngineBase.GetMergedPlan(type, targetField, modifiedFields, (IEnumerable<FieldDescriptor>) null, (IEnumerable<FieldDescriptor>) null, PlanType.MergedExecutionPlan);
    }

    protected static List<FieldDescriptor> GetMergedInitializationPlan(
      Type type,
      List<FieldDescriptor> modifiedFields,
      FieldDescriptor targetField)
    {
      return EngineBase.GetMergedPlan(type, targetField, modifiedFields, (IEnumerable<FieldDescriptor>) null, (IEnumerable<FieldDescriptor>) null, PlanType.MergedInitializationPlan);
    }

    protected static List<FieldDescriptor> GetFullInitializationPlan(
      Type type,
      List<FieldDescriptor> modifiedFields,
      FieldDescriptor targetField)
    {
      return EngineBase.GetMergedPlan(type, targetField, modifiedFields, (IEnumerable<FieldDescriptor>) null, (IEnumerable<FieldDescriptor>) null, PlanType.FullInitializationPlan);
    }

    private static List<FieldDescriptor> GetMergedPlan(
      Type type,
      FieldDescriptor targetField,
      List<FieldDescriptor> modifiedFields,
      IEnumerable<FieldDescriptor> lockedFields,
      IEnumerable<FieldDescriptor> unlockedFields,
      PlanType planType)
    {
      ExecutionDataTable table = EngineBase.LoadExecutionDataTable(Assembly.GetAssembly(type), type);
      short num1;
      if (targetField == (FieldDescriptor) null || !table.FieldIndexDictionary.TryGetValue(targetField, out num1))
        num1 = (short) -1;
      List<short> modifiedIndexes = new List<short>();
      if (modifiedFields != null)
      {
        foreach (FieldDescriptor modifiedField in modifiedFields)
        {
          short num2;
          if (table.FieldIndexDictionary.TryGetValue(modifiedField, out num2))
            modifiedIndexes.Add(num2);
        }
      }
      List<short> first = new List<short>();
      if (lockedFields != null)
      {
        foreach (FieldDescriptor lockedField in lockedFields)
        {
          short num3;
          if (table.FieldIndexDictionary.TryGetValue(lockedField, out num3))
            first.Add(num3);
        }
      }
      List<short> second = new List<short>();
      if (unlockedFields != null)
      {
        foreach (FieldDescriptor unlockedField in unlockedFields)
        {
          short num4;
          if (table.FieldIndexDictionary.TryGetValue(unlockedField, out num4))
            second.Add(num4);
        }
      }
      List<short> mergedExecutionList = new List<short>();
      List<short> mergedInitializationList = new List<short>();
      List<short> fullInitializationList = new List<short>();
      List<short> targetIndexes = new List<short>();
      if (num1 >= (short) 0)
        targetIndexes.Add(num1);
      List<short> list = first.Union<short>((IEnumerable<short>) second).ToList<short>();
      list.Sort();
      Elli.CalculationEngine.Core.EngineCore.EngineCore.BuildMergedExecutionPlan(table, targetIndexes, modifiedIndexes, list, out mergedExecutionList, out mergedInitializationList, out fullInitializationList);
      List<FieldDescriptor> mergedPlan = new List<FieldDescriptor>();
      switch (planType)
      {
        case PlanType.MergedExecutionPlan:
          using (List<short>.Enumerator enumerator = mergedExecutionList.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              short item = enumerator.Current;
              mergedPlan.Add(table.FieldIndexDictionary.FirstOrDefault<KeyValuePair<FieldDescriptor, short>>((Func<KeyValuePair<FieldDescriptor, short>, bool>) (p => (int) p.Value == (int) item)).Key);
            }
            break;
          }
        case PlanType.MergedInitializationPlan:
          using (List<short>.Enumerator enumerator = mergedInitializationList.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              short item = enumerator.Current;
              mergedPlan.Add(table.FieldIndexDictionary.FirstOrDefault<KeyValuePair<FieldDescriptor, short>>((Func<KeyValuePair<FieldDescriptor, short>, bool>) (p => (int) p.Value == (int) item)).Key);
            }
            break;
          }
        case PlanType.FullInitializationPlan:
          using (List<short>.Enumerator enumerator = fullInitializationList.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              short item = enumerator.Current;
              mergedPlan.Add(table.FieldIndexDictionary.FirstOrDefault<KeyValuePair<FieldDescriptor, short>>((Func<KeyValuePair<FieldDescriptor, short>, bool>) (p => (int) p.Value == (int) item)).Key);
            }
            break;
          }
      }
      return mergedPlan;
    }

    public static List<FieldDescriptor> GetReferencedCalculationDescriptors(
      Type type,
      FieldDescriptor fieldDescriptor)
    {
      return EngineBase.LoadExecutionDataTable(Assembly.GetAssembly(type), type).GetExecutionPlan(fieldDescriptor, PlanType.DirectlyReferencedCalculations);
    }

    public static List<FieldDescriptor> GetDependentCalculationDescriptors(
      Type type,
      FieldDescriptor fieldDescriptor)
    {
      return EngineBase.LoadExecutionDataTable(Assembly.GetAssembly(type), type).GetExecutionPlan(fieldDescriptor, PlanType.DirectlyDependentCalculations);
    }

    protected static List<FieldDescriptor> GetExecutionPlan(
      Type type,
      FieldDescriptor fieldDescriptor)
    {
      return EngineBase.LoadExecutionDataTable(Assembly.GetAssembly(type), type).GetExecutionPlan(fieldDescriptor, PlanType.SingleFieldExecutionPlan);
    }

    protected static List<FieldDescriptor> GetInitializationPlan(
      Type type,
      FieldDescriptor fieldDescriptor)
    {
      return EngineBase.LoadExecutionDataTable(Assembly.GetAssembly(type), type).GetExecutionPlan(fieldDescriptor, PlanType.SingleFieldInitializationPlan);
    }

    protected static List<FieldDescriptor> GetReferencePlan(
      Type type,
      FieldDescriptor fieldDescriptor)
    {
      return EngineBase.LoadExecutionDataTable(Assembly.GetAssembly(type), type).GetExecutionPlan(fieldDescriptor, PlanType.SingleFieldReferencePlan);
    }

    protected static List<string> VerifyParameterizedCalculationExists(Type type)
    {
      List<string> stringList = new List<string>();
      Assembly assembly = Assembly.GetAssembly(type);
      ExecutionDataTable table = EngineBase.LoadExecutionDataTable(assembly, type);
      CalculationSet calcSet = EngineBase.LoadCalculationSet(assembly, type);
      DependencyGraph dependencyGraph = EngineBase.LoadDependencyGraph(assembly, type, calcSet, table);
      if (dependencyGraph != null)
      {
        foreach (KeyValuePair<FieldDescriptor, DependencyNode> node1 in dependencyGraph.nodeDictionary)
        {
          foreach (KeyValuePair<ReferencedElement, DependencyNode> keyValuePair in node1.Value.ReferencedRelationships.ToList<KeyValuePair<ReferencedElement, DependencyNode>>())
          {
            if (!keyValuePair.Value.ParentEntityType.IsBaseType() && keyValuePair.Value.ReadOnly)
            {
              DependencyNode node2 = dependencyGraph.GetNode(keyValuePair.Value.FieldId, new EntityDescriptor()
              {
                EntityType = keyValuePair.Value.ParentEntityType.EntityType
              });
              if (node2 != null && !node2.ReadOnly && node2.ParentEntityType.IsBaseType())
              {
                string str = string.Format("{0}.{1}", (object) keyValuePair.Value.ParentEntityType, (object) keyValuePair.Value.FieldId);
                if (!stringList.Contains(str))
                  stringList.Add(str);
              }
            }
          }
        }
      }
      return stringList;
    }

    public void Dispose() => this.Dispose(true);

    protected void Dispose(bool disposing) => this.engineCore.Dispose();
  }
}
