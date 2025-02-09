// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.EngineCore.FieldIndexDictionaryBuilder
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

#nullable disable
namespace Elli.CalculationEngine.Core.EngineCore
{
  public class FieldIndexDictionaryBuilder
  {
    private HashSet<string> addedReferenceList = new HashSet<string>();
    private bool addingReadOnlyCalcs = true;
    private bool buildingList = true;
    private CalculationSet calculationSet;

    public Dictionary<FieldDescriptor, short> FieldIndexDictionary { get; set; }

    public FieldIndexDictionaryBuilder(CalculationSet calculationSet)
    {
      this.calculationSet = calculationSet;
    }

    public void BuildFieldIndexDictionary(bool addingReadOnlyCalcs = false, bool buildingList = true)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      this.FieldIndexDictionary = new Dictionary<FieldDescriptor, short>();
      this.addedReferenceList.Clear();
      List<FieldExpressionCalculation> expressionCalculationList = new List<FieldExpressionCalculation>(this.calculationSet.GetAllFieldExpressionCalculations());
      foreach (FieldExpressionCalculation calc in expressionCalculationList)
      {
        Tracing.Log(TraceLevel.Info, "MasterListBuilder", string.Format("MasterListBuilder.BuildMasterList - Adding References for calc {0}.{1}", (object) calc.ParentEntityType, (object) calc.Name));
        this.AddReferencedFields(calc);
      }
      if (buildingList)
      {
        foreach (FieldExpressionCalculation expressionCalculation in expressionCalculationList)
        {
          if (!expressionCalculation.IsReadOnly)
          {
            FieldDescriptor key = new FieldDescriptor(expressionCalculation.ParentEntityType, expressionCalculation.Name);
            if (!this.FieldIndexDictionary.ContainsKey(key))
            {
              short int16 = Convert.ToInt16(this.FieldIndexDictionary.Count);
              this.FieldIndexDictionary.Add(key, int16);
            }
          }
        }
      }
      stopwatch.Stop();
      Tracing.Log(TraceLevel.Info, "CalculationSet", string.Format("BuildMasterList duration: {0}", (object) stopwatch.Elapsed));
    }

    private void AddReferencedFields(FieldExpressionCalculation calc)
    {
      if (calc == null)
        return;
      this.AddReferencedElements(calc.GetReferencedElements());
    }

    private void AddReferencedElements(IEnumerable<ReferencedElement> elements)
    {
      foreach (ReferencedElement element in elements)
      {
        if (element.DataElementType == DataElementType.DataFieldType)
          this.AddReferencedField(element);
        else
          this.AddReferencedElements((IEnumerable<ReferencedElement>) element.ReferencedElements);
      }
    }

    private void AddReferencedField(ReferencedElement element)
    {
      if (element.IsWeak)
        return;
      string typeFormattedName = element.TypeFormattedName;
      if (this.addedReferenceList.Contains(typeFormattedName))
        return;
      this.addedReferenceList.Add(typeFormattedName);
      EntityDescriptor entityDescriptor = element.ParentElement != null ? element.ParentElement.EntityType : this.calculationSet.RootEntityType;
      FieldExpressionCalculation calculation = this.calculationSet.GetCalculation(element.Name, entityDescriptor.ToString());
      if (calculation == null)
      {
        if (this.addingReadOnlyCalcs)
        {
          Tracing.Log(TraceLevel.Info, "MasterListBuilder", string.Format("MasterListBuilder.AddReferencedField AddElement to calc set {0}.{1}", (object) entityDescriptor, (object) element.Name));
          this.calculationSet.AddElement((CalculationSetElement) new FieldExpressionCalculation(Guid.NewGuid(), element.Name, "Referenced Field - No Calculation", string.Empty, false, string.Format("[{0}]", (object) element.Name), this.calculationSet.Identity.Id, true, entityDescriptor.ToString())
          {
            IsReadOnly = true
          });
        }
      }
      else
        this.AddReferencedFields(calculation);
      if (!this.buildingList)
        return;
      FieldDescriptor key = new FieldDescriptor(element.ParentElement.EntityType, element.Name);
      if (this.FieldIndexDictionary.ContainsKey(key))
        return;
      short int16 = Convert.ToInt16(this.FieldIndexDictionary.Count);
      this.FieldIndexDictionary.Add(key, int16);
    }
  }
}
