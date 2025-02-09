// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.EngineCore.DependencyGraphBuilder
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
using System.Threading.Tasks;

#nullable disable
namespace Elli.CalculationEngine.Core.EngineCore
{
  public class DependencyGraphBuilder
  {
    private EntityDescriptor rootEntityType;
    private string rootRelationship;
    private IEnumerable<FieldExpressionCalculation> calculationList;

    private Dictionary<FieldDescriptor, short> fieldIndexDictionary { get; set; }

    public DependencyGraph DependencyGraph { get; private set; }

    public DependencyGraphBuilder(
      Dictionary<FieldDescriptor, short> fieldIndexDictionary,
      IEnumerable<FieldExpressionCalculation> calculationList,
      EntityDescriptor rootEntityType,
      string rootRelationship)
    {
      this.fieldIndexDictionary = fieldIndexDictionary;
      this.calculationList = calculationList;
      this.rootEntityType = rootEntityType;
      this.rootRelationship = rootRelationship;
    }

    public DependencyGraph BuildGraph()
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      this.DependencyGraph = new DependencyGraph(this.calculationList, this.rootEntityType, this.rootRelationship);
      Parallel.ForEach<DependencyNode>((IEnumerable<DependencyNode>) this.DependencyGraph.nodeDictionary.Values, (Action<DependencyNode>) (node => this.SortDependencies(node)));
      stopwatch.Stop();
      Tracing.Log(TraceLevel.Verbose, nameof (DependencyGraphBuilder), string.Format("BuildGraph duration: {0}", (object) stopwatch.Elapsed));
      return this.DependencyGraph;
    }

    private void SortDependencies(DependencyNode node)
    {
      Dictionary<ReferencedElement, DependencyNode> dependentRelationships = node.DependentRelationships;
      if (!dependentRelationships.Any<KeyValuePair<ReferencedElement, DependencyNode>>())
        return;
      Dictionary<ReferencedElement, DependencyNode> dictionary = new Dictionary<ReferencedElement, DependencyNode>();
      foreach (FieldDescriptor fieldDescriptor in this.fieldIndexDictionary.Keys.Intersect<FieldDescriptor>(dependentRelationships.Keys.Select<ReferencedElement, FieldDescriptor>((Func<ReferencedElement, FieldDescriptor>) (item => new FieldDescriptor((object) item.GetFieldParentType() == null ? this.rootEntityType : item.GetFieldParentType(), item.GetFieldName())))))
      {
        FieldDescriptor item = fieldDescriptor;
        ReferencedElement key = dependentRelationships.Keys.First<ReferencedElement>((Func<ReferencedElement, bool>) (p =>
        {
          if (!(p.GetFieldName() == item.FieldId))
            return false;
          return (object) p.GetFieldParentType() != null ? p.GetFieldParentType() == item.ParentEntityType : item.ParentEntityType == this.rootEntityType;
        }));
        dictionary.Add(key, dependentRelationships[key]);
      }
      node.DependentRelationships = dictionary;
    }
  }
}
