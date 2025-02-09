// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationLibrary.Function
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
  public class Function : CalculationSetElement
  {
    public Function()
    {
    }

    public Function(
      Guid id,
      string descriptiveName,
      string description,
      string calc,
      Guid parentId,
      bool enabled,
      Elli.CalculationEngine.Common.ValueType returnType = Elli.CalculationEngine.Common.ValueType.None,
      List<CalculationTest> calculationTests = null)
    {
      this.Identity = new ElementIdentity();
      this.Identity.Id = id;
      this.Identity.ParentId = parentId;
      this.Identity.Type = LibraryElementType.Function;
      this.Identity.Description = description;
      this.Name = id.ToString();
      this.DescriptiveName = descriptiveName;
      this.Enabled = enabled;
      this.CalculationTests = calculationTests;
      this.Expression = new FunctionExpression(calc, returnType);
    }

    [DataMember]
    public FunctionExpression Expression { get; set; }
  }
}
