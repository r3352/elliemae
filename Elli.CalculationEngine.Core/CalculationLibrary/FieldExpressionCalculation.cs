// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationLibrary.FieldExpressionCalculation
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.CalculationLibrary
{
  [DataContract]
  public class FieldExpressionCalculation : CalculationSetElement
  {
    [DataMember]
    public bool IsTransient { get; set; }

    [DataMember(Order = 1)]
    public bool IsMultiLineCalculation { get; set; }

    [DataMember(Order = 2)]
    public bool IsReadOnly { get; set; }

    [DataMember]
    public CalculationExpression Expression { get; set; }

    public FieldExpressionCalculation()
    {
      this.Identity = new ElementIdentity();
      this.Identity.Type = LibraryElementType.FieldExpressionCalculation;
    }

    public FieldExpressionCalculation(
      Guid id,
      string fieldName,
      string descriptiveName,
      string description,
      bool transient,
      string calc,
      Guid parentId,
      bool enabled,
      string parentEntityType,
      Elli.CalculationEngine.Common.ValueType returnType = Elli.CalculationEngine.Common.ValueType.None,
      bool multiLine = false,
      List<CalculationTest> calculationTests = null)
      : this()
    {
      this.Identity.Id = id;
      this.Identity.ParentId = parentId;
      this.Identity.Description = description;
      this.Name = fieldName;
      this.DescriptiveName = descriptiveName;
      this.IsMultiLineCalculation = multiLine;
      this.IsTransient = transient;
      this.Enabled = enabled;
      this.CalculationTests = calculationTests;
      this.IsReadOnly = false;
      string parentEntityType1 = string.IsNullOrEmpty(parentEntityType) ? CalculationUtility.GetRootEntityType() : parentEntityType;
      this.Expression = new CalculationExpression(calc, returnType, parentEntityType1);
    }

    public IEnumerable<ReferencedElement> GetReferencedElements()
    {
      return (IEnumerable<ReferencedElement>) this.Expression.ReferenceRoot.ReferencedElements;
    }
  }
}
