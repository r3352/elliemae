// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.EngineCore.ExecutionDataTableBuilder
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.DataSource;
using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Elli.CalculationEngine.Core.EngineCore
{
  public class ExecutionDataTableBuilder
  {
    private HashSet<FieldDescriptor> calcQueue = new HashSet<FieldDescriptor>();
    private HashSet<FieldDescriptor> initQueue = new HashSet<FieldDescriptor>();
    private HashSet<FieldDescriptor> refQueue = new HashSet<FieldDescriptor>();
    private List<short> planList = new List<short>();
    private List<short> initList = new List<short>();
    private Dictionary<FieldDescriptor, short> fieldIndexDictionary;
    private DependencyGraph dependencyGraph;
    private FieldExecutionData[] lookupTable;

    public ExecutionDataTableBuilder(
      Dictionary<FieldDescriptor, short> fieldIndexDictionary,
      DependencyGraph dependencyGraph)
    {
      this.fieldIndexDictionary = fieldIndexDictionary;
      this.dependencyGraph = dependencyGraph;
    }

    public ExecutionDataTable ExecutionDataTable { get; set; }

    public void BuildExecutionPlanTable()
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      this.BuildLookupTable(this.fieldIndexDictionary, this.dependencyGraph);
      this.ExecutionDataTable = new ExecutionDataTable(this.fieldIndexDictionary, this.lookupTable);
      stopwatch.Stop();
      Tracing.Log(TraceLevel.Info, "CalculationSet", string.Format("BuildExecutionPlanTable duration: {0}", (object) stopwatch.Elapsed));
    }

    private void BuildLookupTable(
      Dictionary<FieldDescriptor, short> fieldIndexDictionary,
      DependencyGraph dependencyGraph)
    {
      List<FieldExecutionData> fieldExecutionDataList = new List<FieldExecutionData>();
      foreach (FieldDescriptor key in fieldIndexDictionary.Keys)
      {
        FieldExecutionData fieldExecutionData = new FieldExecutionData();
        fieldExecutionData.Descriptor = key;
        DependencyNode node = dependencyGraph.GetNode(key);
        fieldExecutionData.HasCalculation = !node.ReadOnly;
        List<FieldDescriptor> fieldDescriptorList1 = new List<FieldDescriptor>();
        List<short> shortList1 = new List<short>();
        List<short> collection1 = new List<short>();
        foreach (KeyValuePair<ReferencedElement, DependencyNode> referencedRelationship in node.ReferencedRelationships)
        {
          FieldDescriptor desc = FieldDescriptor.Create(referencedRelationship.Key.FieldElement.GetFieldParentType(), referencedRelationship.Key.FieldElement.Name);
          fieldDescriptorList1.Add(desc);
          short descriptorIndex = this.GetDescriptorIndex(desc);
          if (!collection1.Contains(descriptorIndex))
            collection1.Add(descriptorIndex);
          if (referencedRelationship.Key.FieldElement.IsWeak)
            shortList1.Add(descriptorIndex);
        }
        fieldExecutionData.ReferencedCalculations = new HashSet<short>((IEnumerable<short>) collection1);
        fieldExecutionData.WeakReferences = shortList1.ToArray();
        List<FieldDescriptor> fieldDescriptorList2 = new List<FieldDescriptor>();
        List<short> shortList2 = new List<short>();
        List<short> collection2 = new List<short>();
        foreach (KeyValuePair<ReferencedElement, DependencyNode> dependentRelationship in node.DependentRelationships)
        {
          FieldDescriptor desc = FieldDescriptor.Create(dependentRelationship.Key.FieldElement.GetFieldParentType(), dependentRelationship.Key.FieldElement.Name);
          fieldDescriptorList2.Add(desc);
          short descriptorIndex = this.GetDescriptorIndex(desc);
          if (!collection2.Contains(descriptorIndex))
            collection2.Add(descriptorIndex);
          if (dependentRelationship.Value.HasWeakReferences)
          {
            foreach (KeyValuePair<ReferencedElement, DependencyNode> referencedRelationship in dependentRelationship.Value.ReferencedRelationships)
            {
              if (referencedRelationship.Key.FieldElement.IsWeak && FieldDescriptor.Create(referencedRelationship.Key.FieldElement.GetFieldParentType(), referencedRelationship.Key.FieldElement.Name) == key)
                shortList2.Add(descriptorIndex);
            }
          }
        }
        fieldExecutionData.DependentCalculations = new HashSet<short>((IEnumerable<short>) collection2);
        fieldExecutionData.WeakDependencies = shortList2.ToArray();
        fieldExecutionData.ExecutionPlan = this.BuildExecutionPlan(node);
        fieldExecutionData.InitializationPlan = this.BuildInitializationPlan(node);
        fieldExecutionData.ReferencePlan = this.BuildReferencePlan(node);
        fieldExecutionDataList.Add(fieldExecutionData);
      }
      this.lookupTable = fieldExecutionDataList.ToArray();
    }

    private short GetDescriptorIndex(FieldDescriptor desc)
    {
      short fieldIndex;
      if (!this.fieldIndexDictionary.TryGetValue(desc, out fieldIndex))
      {
        try
        {
          fieldIndex = this.fieldIndexDictionary[desc.GetBaseDescriptor()];
        }
        catch (Exception ex)
        {
          throw new Exception(string.Format("Missing Base Descriptor {0} from field lookup dictionary.", (object) desc.GetBaseDescriptor()), ex);
        }
      }
      return fieldIndex;
    }

    private short[] BuildAndSortFieldIndexArray(HashSet<FieldDescriptor> descriptorList)
    {
      List<short> shortList = new List<short>();
      foreach (FieldDescriptor descriptor in descriptorList)
      {
        short fieldIndex;
        if (!this.fieldIndexDictionary.TryGetValue(descriptor, out fieldIndex))
          fieldIndex = this.fieldIndexDictionary[descriptor.GetBaseDescriptor()];
        shortList.Add(fieldIndex);
      }
      shortList.Sort();
      return shortList.ToArray();
    }

    private short[] BuildExecutionPlan(DependencyNode changedNode)
    {
      this.calcQueue.Clear();
      if (changedNode != null)
      {
        this.calcQueue.Add(changedNode.FieldDescriptor);
        foreach (KeyValuePair<ReferencedElement, DependencyNode> dependentRelationship in changedNode.DependentRelationships)
          this.CalculateDependencyNode(dependentRelationship.Value, changedNode.FieldDescriptor);
      }
      return this.BuildAndSortFieldIndexArray(this.calcQueue);
    }

    private void CalculateDependencyNode(DependencyNode node, FieldDescriptor currentDescriptor)
    {
      if (node == null || this.calcQueue.Contains(node.FieldDescriptor) || node.HasWeakReferences && !node.ReferencedRelationships.Any<KeyValuePair<ReferencedElement, DependencyNode>>((Func<KeyValuePair<ReferencedElement, DependencyNode>, bool>) (p => p.Key.FieldElement.IsWeak && p.Value.FieldDescriptor == currentDescriptor)))
        return;
      this.calcQueue.Add(node.FieldDescriptor);
      foreach (KeyValuePair<ReferencedElement, DependencyNode> dependentRelationship in node.DependentRelationships)
        this.CalculateDependencyNode(dependentRelationship.Value, currentDescriptor);
    }

    private short[] BuildInitializationPlan(DependencyNode changedNode)
    {
      this.initQueue.Clear();
      foreach (FieldDescriptor descriptor in new List<FieldDescriptor>((IEnumerable<FieldDescriptor>) this.calcQueue)
      {
        changedNode.FieldDescriptor
      })
      {
        foreach (KeyValuePair<ReferencedElement, DependencyNode> referencedRelationship in this.dependencyGraph.GetNode(descriptor).ReferencedRelationships)
        {
          string fieldId = referencedRelationship.Value.FieldId;
          if (EngineUtility.IsTransientField(fieldId) || descriptor.FieldId == fieldId && descriptor.ParentEntityType.IsBaseType() && descriptor.ParentEntityType.EntityType == referencedRelationship.Value.ParentEntityType.EntityType)
            this.CalculateReferenceNode(referencedRelationship.Key, referencedRelationship.Value);
        }
      }
      List<short> shortList = new List<short>();
      foreach (FieldDescriptor init in this.initQueue)
      {
        short fieldIndex;
        if (!this.fieldIndexDictionary.TryGetValue(init, out fieldIndex))
          fieldIndex = this.fieldIndexDictionary[init.GetBaseDescriptor()];
        shortList.Add(fieldIndex);
      }
      shortList.Sort();
      return shortList.ToArray();
    }

    private short[] BuildReferencePlan(DependencyNode changedNode)
    {
      this.refQueue.Clear();
      if (changedNode != null)
      {
        this.refQueue.Add(changedNode.FieldDescriptor);
        foreach (KeyValuePair<ReferencedElement, DependencyNode> referencedRelationship in changedNode.ReferencedRelationships)
          this.CalculateReferencePlanNode(referencedRelationship.Key, referencedRelationship.Value);
      }
      List<short> shortList = new List<short>();
      foreach (FieldDescriptor key in this.refQueue)
      {
        short fieldIndex;
        if (!this.fieldIndexDictionary.TryGetValue(key, out fieldIndex))
          fieldIndex = this.fieldIndexDictionary[key.GetBaseDescriptor()];
        shortList.Add(fieldIndex);
      }
      shortList.Sort();
      return shortList.ToArray();
    }

    private void CalculateReferenceNode(ReferencedElement relationship, DependencyNode node)
    {
      if (node == null)
        return;
      DependencyNode node1 = this.dependencyGraph.GetNode(relationship.FieldElement.Name, relationship.FieldElement.ParentElement.EntityType);
      if (EngineUtility.IsTransientField(node1.FieldId) && !this.initQueue.Contains(node1.FieldDescriptor))
        this.initQueue.Add(node1.FieldDescriptor);
      foreach (KeyValuePair<ReferencedElement, DependencyNode> referencedRelationship in node1.ReferencedRelationships)
      {
        if (EngineUtility.IsTransientField(referencedRelationship.Value.FieldId) && !this.initQueue.Contains(referencedRelationship.Value.FieldDescriptor))
        {
          this.initQueue.Add(referencedRelationship.Value.FieldDescriptor);
          this.CalculateReferenceNode(referencedRelationship.Key, referencedRelationship.Value);
        }
      }
    }

    private void CalculateReferencePlanNode(ReferencedElement relationship, DependencyNode node)
    {
      if (node == null)
        return;
      DependencyNode node1 = this.dependencyGraph.GetNode(relationship.FieldElement.Name, relationship.FieldElement.ParentElement.EntityType);
      if (node1 == null || this.refQueue.Contains(node1.FieldDescriptor))
        return;
      this.refQueue.Add(node1.FieldDescriptor);
      foreach (KeyValuePair<ReferencedElement, DependencyNode> referencedRelationship in node1.ReferencedRelationships)
        this.CalculateReferencePlanNode(referencedRelationship.Key, referencedRelationship.Value);
    }
  }
}
