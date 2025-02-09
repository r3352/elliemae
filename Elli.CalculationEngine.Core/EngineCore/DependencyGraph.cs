// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.EngineCore.DependencyGraph
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.CalculationLibrary;
using Elli.CalculationEngine.Core.DataSource;
using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace Elli.CalculationEngine.Core.EngineCore
{
  [DataContract(Namespace = "")]
  public class DependencyGraph : IDisposable
  {
    private HashSet<string> addedDependencyList = new HashSet<string>();
    [DataMember]
    public Dictionary<FieldDescriptor, DependencyNode> nodeDictionary;

    public DependencyGraph(
      IEnumerable<FieldExpressionCalculation> calculationList,
      EntityDescriptor rootEntityType,
      string rootRelationship)
    {
      Tracing.Log(TraceLevel.Info, this.GetType().Name, "DependencyGraph()");
      this.nodeDictionary = new Dictionary<FieldDescriptor, DependencyNode>();
      Dictionary<FieldDescriptor, FieldExpressionCalculation> dictionary = calculationList.ToDictionary<FieldExpressionCalculation, FieldDescriptor>((Func<FieldExpressionCalculation, FieldDescriptor>) (c => new FieldDescriptor(FieldReplacementRegex.ParseEntityDescriptor(c.ParentEntityType), c.Name)));
      foreach (KeyValuePair<FieldDescriptor, FieldExpressionCalculation> keyValuePair in dictionary)
      {
        FieldExpressionCalculation expressionCalculation1 = keyValuePair.Value;
        EntityDescriptor parentEntityType = keyValuePair.Key.ParentEntityType;
        DependencyNode dependencyNode1;
        if (!this.nodeDictionary.TryGetValue(keyValuePair.Key, out dependencyNode1))
        {
          dependencyNode1 = new DependencyNode(expressionCalculation1.Name, parentEntityType, expressionCalculation1.IsReadOnly);
          this.nodeDictionary.Add(dependencyNode1.FieldDescriptor, dependencyNode1);
        }
        for (int index = expressionCalculation1.Expression.ReferencedFields.Count<string>() - 1; index >= 0; --index)
        {
          string str = expressionCalculation1.Expression.ReferencedFields.ElementAt<string>(index);
          EntityDescriptor outParentType;
          string fieldId;
          FieldReplacementRegex.ParseFullyQualifiedFieldName(str, parentEntityType, rootEntityType, rootRelationship, out outParentType, out fieldId, out string _);
          ReferencedElement parentElement1 = new ReferencedElement(CalculationUtility.ALL_ENTITIES, outParentType, DataElementType.DataEntityType, (ReferencedElement) null, false);
          ReferencedElement complexId1 = FieldReplacementRegex.ParseComplexId(str, parentElement1, new List<ReferencedElement>(), rootEntityType, rootRelationship);
          if (!(fieldId == dependencyNode1.FieldId) || !(outParentType == dependencyNode1.ParentEntityType))
          {
            FieldDescriptor key1 = new FieldDescriptor(outParentType, fieldId);
            DependencyNode node1;
            if (!this.nodeDictionary.TryGetValue(key1, out node1))
            {
              bool readOnly = false;
              FieldExpressionCalculation expressionCalculation2 = (FieldExpressionCalculation) null;
              if (dictionary.TryGetValue(key1, out expressionCalculation2))
                readOnly = expressionCalculation2.IsReadOnly;
              node1 = new DependencyNode(fieldId, outParentType, readOnly);
              this.nodeDictionary.Add(node1.FieldDescriptor, node1);
            }
            dependencyNode1.AddReferencedRelationship(complexId1, node1);
            if (!complexId1.FieldElement.IsWeak)
            {
              ReferencedElement newParentElement = complexId1.FieldElement.ParentElement;
              if (newParentElement.EntityType.IsBaseType())
              {
                foreach (EntityDescriptor entityDescriptor in CalculationUtility.GetEntityTypeList().Where<EntityDescriptor>((Func<EntityDescriptor, bool>) (p => p.IsA(newParentElement.EntityType) && !p.IsBaseType())))
                {
                  FieldDescriptor key2 = new FieldDescriptor(entityDescriptor, node1.FieldId);
                  if (entityDescriptor != newParentElement.EntityType)
                  {
                    DependencyNode node2;
                    if (!this.nodeDictionary.TryGetValue(key2, out node2))
                    {
                      bool readOnly = false;
                      FieldExpressionCalculation expressionCalculation3 = (FieldExpressionCalculation) null;
                      if (dictionary.TryGetValue(key2, out expressionCalculation3))
                        readOnly = expressionCalculation3.IsReadOnly;
                      DependencyNode dependencyNode2 = new DependencyNode(key2.FieldId, key2.ParentEntityType, readOnly);
                      this.nodeDictionary.Add(key2, dependencyNode2);
                      this.nodeDictionary.TryGetValue(key2, out node2);
                    }
                    ReferencedElement referencedElement = new ReferencedElement(CalculationUtility.ALL_ENTITIES, entityDescriptor, DataElementType.DataEntityType, complexId1, false);
                    ReferencedElement element = new ReferencedElement(node1.FieldId, (EntityDescriptor) null, DataElementType.DataFieldType, referencedElement, false);
                    referencedElement.AddReferencedElement(element);
                    ReferencedElement parentElement2 = new ReferencedElement(CalculationUtility.ALL_ENTITIES, outParentType, DataElementType.DataEntityType, (ReferencedElement) null, false);
                    ReferencedElement complexId2 = FieldReplacementRegex.ParseComplexId(str, parentElement2, new List<ReferencedElement>(), rootEntityType, rootRelationship);
                    ReferencedElement parentElement3 = complexId2.FieldElement.ParentElement;
                    if (parentElement3.FormattedName != complexId2.FormattedName && parentElement3.ParentElement != null)
                    {
                      complexId2.FieldElement.ParentElement.EntityType = entityDescriptor;
                      referencedElement = complexId2;
                    }
                    dependencyNode1.AddReferencedRelationship(referencedElement, node2);
                  }
                }
              }
            }
          }
        }
      }
      foreach (DependencyNode dependentNode in this.nodeDictionary.Values)
      {
        if (dependentNode != null)
        {
          string str = CalculationUtility.BuildFullyQualifiedName((string) null, dependentNode.FieldId, CalculationUtility.ALL_ENTITIES, dependentNode.ParentEntityType.ToString());
          if (!this.addedDependencyList.Contains(str))
          {
            this.addedDependencyList.Add(str);
            this.AddDependentNodes(dependentNode, rootEntityType, rootRelationship);
          }
        }
      }
      this.ClearAllVisited();
      this.addedDependencyList.Clear();
    }

    public List<string> GetLogList()
    {
      List<string> logList = new List<string>();
      foreach (KeyValuePair<FieldDescriptor, DependencyNode> node in this.nodeDictionary)
      {
        logList.Add(node.Key.ToString());
        foreach (DependencyNode dependencyNode in node.Value.ReferencedRelationships.Values)
          logList.Add("\treference - " + dependencyNode.ParentEntityType.ToString() + "." + dependencyNode.FieldId);
      }
      return logList;
    }

    public void ClearAllVisited()
    {
      foreach (DependencyNode dependencyNode in this.nodeDictionary.Values)
        dependencyNode.ClearAllVisited();
    }

    public DependencyNode GetNode(
      string fieldId,
      EntityDescriptor parentType,
      StringBuilder sbTrace = null)
    {
      try
      {
        DependencyNode node;
        this.nodeDictionary.TryGetValue(FieldDescriptor.Create(parentType, fieldId), out node);
        if (node == null && !parentType.IsBaseType())
          this.nodeDictionary.TryGetValue(FieldDescriptor.Create(parentType.GetBaseDescriptor(), fieldId), out node);
        return node;
      }
      catch (Exception ex)
      {
        sbTrace.AppendLine("DependencyGraph.GetNode() Exception : " + ex.Message);
        sbTrace.AppendLine(ex.StackTrace);
        throw;
      }
    }

    public DependencyNode GetNode(FieldDescriptor descriptor)
    {
      return this.GetNode(descriptor.FieldId, descriptor.ParentEntityType);
    }

    public DependencyNode GetNode(IDataField field)
    {
      return field == null || string.IsNullOrEmpty(field.Id) ? (DependencyNode) null : this.GetNode(field.Id, field.ParentEntity.GetEntityType());
    }

    public bool CalculationExists(
      EntityDescriptor entity,
      string fieldId,
      bool ignoreEntityParameters,
      bool checkReadOnly = false)
    {
      bool flag;
      if (ignoreEntityParameters)
      {
        flag = this.nodeDictionary.Keys.Any<FieldDescriptor>((Func<FieldDescriptor, bool>) (p => p.ParentEntityType.EntityType == entity.EntityType && p.FieldId == fieldId));
      }
      else
      {
        DependencyNode node = this.GetNode(fieldId, entity);
        flag = !checkReadOnly ? node != null : node != null && !node.ReadOnly;
      }
      return flag;
    }

    public void SetFieldState(IDataField field, FieldState state)
    {
      if (field == null)
        return;
      this.GetNode(field)?.SetFieldState(field, state);
    }

    public void ResetAllFieldStates(FieldState state)
    {
      foreach (DependencyNode dependencyNode in this.nodeDictionary.Values)
        dependencyNode.ResetAllFieldStates(state);
    }

    public void Dispose() => this.Dispose(true);

    protected void Dispose(bool disposing)
    {
      if (this.addedDependencyList != null)
        this.addedDependencyList.Clear();
      if (this.nodeDictionary == null)
        return;
      foreach (DependencyNode dependencyNode in this.nodeDictionary.Values)
        dependencyNode.Dispose();
      this.nodeDictionary.Clear();
    }

    private void AddDependentNodes(
      DependencyNode dependentNode,
      EntityDescriptor rootEntityType,
      string rootRelationship)
    {
      foreach (KeyValuePair<ReferencedElement, DependencyNode> referencedRelationship in dependentNode.ReferencedRelationships)
      {
        ReferencedElement key = referencedRelationship.Key;
        DependencyNode dependentNode1 = referencedRelationship.Value;
        if (dependentNode1 != null)
        {
          if (!this.addedDependencyList.Contains(key.QualifiedName))
          {
            this.addedDependencyList.Add(key.QualifiedName);
            this.AddDependentNodes(dependentNode1, rootEntityType, rootRelationship);
          }
          ReferencedElement referencedElement = new ReferencedElement(CalculationUtility.ALL_ENTITIES, dependentNode.ParentEntityType, DataElementType.DataEntityCollectionType, (ReferencedElement) null, false);
          referencedElement.AddReferencedElement(new ReferencedElement(dependentNode.FieldId, (EntityDescriptor) null, DataElementType.DataFieldType, referencedElement, referencedElement.IsWeak));
          if (!(dependentNode.FieldId == dependentNode1.FieldId) || !(dependentNode.ParentEntityType == dependentNode1.ParentEntityType))
          {
            dependentNode1.AddDependentRelationship(referencedElement, dependentNode);
            if (!referencedElement.EntityType.IsBaseType())
            {
              ReferencedElement baseReference = dependentNode.GetBaseReference(referencedElement);
              DependencyNode node;
              if (this.nodeDictionary.TryGetValue(new FieldDescriptor(baseReference.GetFieldParentType(), dependentNode.FieldId), out node))
                dependentNode1.AddDependentRelationship(baseReference, node);
            }
          }
        }
        else
          Tracing.Log(TraceLevel.Warning, this.GetType().Name, "relationship.DependencyNode is null");
      }
    }
  }
}
