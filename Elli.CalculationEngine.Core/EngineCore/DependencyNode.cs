// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.EngineCore.DependencyNode
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.DataSource;
using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.EngineCore
{
  [DataContract(IsReference = true, Namespace = "")]
  public class DependencyNode : IDisposable
  {
    [DataMember]
    private List<string> visitedParentEntities { get; set; }

    [DataMember]
    private Dictionary<string, FieldState> calculatedParentEntities { get; set; }

    [DataMember]
    public EntityDescriptor ParentEntityType { get; private set; }

    [DataMember]
    public string FieldId { get; private set; }

    public FieldDescriptor FieldDescriptor { get; private set; }

    [DataMember]
    public bool ReadOnly { get; private set; }

    [DataMember]
    public Dictionary<ReferencedElement, DependencyNode> DependentRelationships { get; set; }

    [DataMember]
    public Dictionary<ReferencedElement, DependencyNode> ReferencedRelationships { get; private set; }

    [DataMember]
    public bool HasWeakReferences { get; private set; }

    public DependencyNode(string fieldId, EntityDescriptor parentEntityType, bool readOnly)
    {
      this.visitedParentEntities = new List<string>();
      this.calculatedParentEntities = new Dictionary<string, FieldState>();
      this.FieldId = fieldId;
      this.ParentEntityType = parentEntityType;
      this.FieldDescriptor = new FieldDescriptor(parentEntityType, fieldId);
      this.DependentRelationships = new Dictionary<ReferencedElement, DependencyNode>();
      this.ReferencedRelationships = new Dictionary<ReferencedElement, DependencyNode>();
      if (string.CompareOrdinal(parentEntityType.EntityType, CalculationUtility.SETTINGS) == 0)
        this.ReadOnly = true;
      else
        this.ReadOnly = readOnly;
    }

    public void AddDependentRelationship(ReferencedElement relationship, DependencyNode node)
    {
      DependencyNode dependencyNode;
      if (this.DependentRelationships.Keys.Where<ReferencedElement>((Func<ReferencedElement, bool>) (p => p.FieldElement.QualifiedName == relationship.FieldElement.QualifiedName)).Any<ReferencedElement>() || this.DependentRelationships.TryGetValue(relationship, out dependencyNode))
        return;
      foreach (ReferencedElement key in this.DependentRelationships.Keys)
      {
        if (key.FieldElement.QualifiedName == relationship.FieldElement.QualifiedName)
          throw new Exception(string.Format("Key '{0}' Already exists!!", (object) key.FieldElement.QualifiedName));
      }
      if (dependencyNode == node)
        return;
      this.DependentRelationships.Add(relationship, node);
    }

    public void AddReferencedRelationship(ReferencedElement relationship, DependencyNode node)
    {
      DependencyNode dependencyNode;
      if (this.ReferencedRelationships.Keys.Where<ReferencedElement>((Func<ReferencedElement, bool>) (p => p.FieldElement.QualifiedName == relationship.FieldElement.QualifiedName)).Any<ReferencedElement>() || this.ReferencedRelationships.TryGetValue(relationship, out dependencyNode))
        return;
      foreach (ReferencedElement key in this.DependentRelationships.Keys)
      {
        if (key.FieldElement.QualifiedName == relationship.FieldElement.QualifiedName)
          throw new Exception(string.Format("Key '{0}' Already exists!!", (object) key.FieldElement.QualifiedName));
      }
      if (dependencyNode == node)
        return;
      this.ReferencedRelationships.Add(relationship, node);
      this.HasWeakReferences = relationship.WeakReferencedFields.Any<string>() || this.HasWeakReferences;
    }

    public ReferencedElement GetBaseReference(ReferencedElement qualifiedName)
    {
      return new ReferencedElement(qualifiedName.Name, new EntityDescriptor(qualifiedName.EntityType.EntityType, (IEnumerable<EntityParameter>) null), qualifiedName.DataElementType, qualifiedName.ParentElement, qualifiedName.IsWeak);
    }

    public string GetFullName()
    {
      return string.Format("{{{0}}}.[{1}]", (object) this.ParentEntityType.ToString(), (object) this.FieldId);
    }

    public bool IsCalculated(IDataField field)
    {
      FieldState fieldState = FieldState.Unknown;
      if (field != null && field.ParentEntity != null && !this.calculatedParentEntities.TryGetValue(field.ParentEntity.GetEntityName(), out fieldState))
        fieldState = FieldState.Unknown;
      return fieldState == FieldState.Calculated;
    }

    public FieldState GetFieldState(IDataField field)
    {
      FieldState fieldState = FieldState.Unknown;
      if (field != null && field.ParentEntity != null && !this.calculatedParentEntities.TryGetValue(field.ParentEntity.GetEntityName(), out fieldState))
        fieldState = FieldState.Unknown;
      return fieldState;
    }

    public void SetFieldState(IDataField field, FieldState state)
    {
      if (field == null || field.ParentEntity == null)
        return;
      string entityName = field.ParentEntity.GetEntityName();
      if (this.calculatedParentEntities.ContainsKey(entityName))
        this.calculatedParentEntities[entityName] = state;
      else
        this.calculatedParentEntities.Add(entityName, state);
    }

    public void ResetAllFieldStates(FieldState state)
    {
      foreach (string key in this.calculatedParentEntities.Keys.ToList<string>())
        this.calculatedParentEntities[key] = state;
    }

    public bool IsRelationshipVisited(string relationship)
    {
      return this.visitedParentEntities.Contains(relationship) || this.visitedParentEntities.Contains(CalculationUtility.ALL_ENTITIES);
    }

    public bool IsRelationshipVisited()
    {
      return this.visitedParentEntities.Contains(CalculationUtility.ALL_ENTITIES);
    }

    public void AddVisitedRelationship(string relationship)
    {
      if (this.visitedParentEntities.Contains(relationship))
        return;
      this.visitedParentEntities.Add(relationship);
    }

    public void SetAllVisited()
    {
      if (this.visitedParentEntities.Contains(CalculationUtility.ALL_ENTITIES))
        return;
      this.visitedParentEntities.Add(CalculationUtility.ALL_ENTITIES);
    }

    public void ClearAllVisited() => this.visitedParentEntities.Clear();

    public void Dispose() => this.Dispose(true);

    protected void Dispose(bool disposing)
    {
      if (this.visitedParentEntities != null)
        this.visitedParentEntities.Clear();
      if (this.DependentRelationships != null)
        this.DependentRelationships.Clear();
      if (this.ReferencedRelationships != null)
        this.ReferencedRelationships.Clear();
      if (this.calculatedParentEntities == null)
        return;
      this.calculatedParentEntities.Clear();
    }

    private string GetQualifiedName(string relationship)
    {
      return CalculationUtility.BuildFullyQualifiedName(relationship, this.FieldId);
    }
  }
}
