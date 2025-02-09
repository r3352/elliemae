// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ExpressionParser.CalculationExpression
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.ExpressionParser
{
  [DataContract]
  public class CalculationExpression : Expression
  {
    [IgnoreDataMember]
    private ReferencedElement referenceRoot;

    public CalculationExpression()
    {
      this.text = string.Empty;
      this.type = ExpressionType.Calculation;
      this.parentEntityType = CalculationUtility.GetRootEntityType();
    }

    public CalculationExpression(string text)
    {
      this.text = text == null ? string.Empty : text;
      this.type = ExpressionType.Calculation;
      this.parentEntityType = CalculationUtility.GetRootEntityType();
      this.referenceRoot = FieldReplacementRegex.ParseReferencedElements(text, CalculationUtility.GetRootEntityType());
    }

    public CalculationExpression(string text, string parentEntityType)
    {
      this.text = text == null ? string.Empty : text;
      this.type = ExpressionType.Calculation;
      this.parentEntityType = string.IsNullOrEmpty(parentEntityType) ? CalculationUtility.GetRootEntityType() : parentEntityType;
      this.referenceRoot = FieldReplacementRegex.ParseReferencedElements(text, this.parentEntityType);
    }

    public CalculationExpression(string text, ValueType returnType, string parentEntityType)
    {
      this.returnType = returnType;
      this.text = text == null ? string.Empty : text;
      this.type = ExpressionType.Calculation;
      this.parentEntityType = string.IsNullOrEmpty(parentEntityType) ? CalculationUtility.GetRootEntityType() : parentEntityType;
      this.referenceRoot = FieldReplacementRegex.ParseReferencedElements(text, this.parentEntityType);
    }

    public new string Text
    {
      get => this.text;
      set
      {
        this.text = value;
        this.referenceRoot = FieldReplacementRegex.ParseReferencedElements(this.text, this.parentEntityType);
      }
    }

    public string ParentEntityType
    {
      get => this.parentEntityType;
      set
      {
        this.parentEntityType = value;
        this.referenceRoot = FieldReplacementRegex.ParseReferencedElements(this.text, this.parentEntityType);
      }
    }

    public IEnumerable<string> ReferencedFields => this.referenceRoot.ReferencedFields;

    public IEnumerable<string> WeakReferencedFields => this.referenceRoot.WeakReferencedFields;

    public IEnumerable<string> StrongReferencedFields => this.referenceRoot.StrongReferencedFields;

    public ReferencedElement ReferenceRoot
    {
      get
      {
        if (this.referenceRoot == null)
          this.referenceRoot = FieldReplacementRegex.ParseReferencedElements(this.text, this.parentEntityType);
        return this.referenceRoot;
      }
      set
      {
      }
    }

    [OnDeserialized]
    private void ClearReferencedFields(StreamingContext c)
    {
      if (string.IsNullOrEmpty(this.parentEntityType))
        this.parentEntityType = CalculationUtility.GetRootEntityType();
      if (this.referenceRoot == null)
        return;
      this.referenceRoot = FieldReplacementRegex.ParseReferencedElements(this.text, this.parentEntityType);
    }
  }
}
