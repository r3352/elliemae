// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ExpressionParser.FunctionExpression
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.ExpressionParser
{
  [DataContract]
  public class FunctionExpression : Expression
  {
    private List<FunctionParameter> parameters { get; set; }

    public FunctionExpression()
    {
    }

    public FunctionExpression(string text, Elli.CalculationEngine.Common.ValueType returnType = Elli.CalculationEngine.Common.ValueType.None)
    {
      this.parentEntityType = CalculationUtility.GetRootEntityType();
      this.ReturnType = returnType;
      this.Text = string.IsNullOrWhiteSpace(text) ? string.Empty : text;
      this.Type = ExpressionType.Function;
    }

    public List<FunctionParameter> Parameters
    {
      get
      {
        if (this.parameters == null || this.parameters.Count == 0)
          this.parameters = FunctionReplacementRegex.ParseFunctionParameters(this.Text);
        return this.parameters;
      }
    }

    public string ParentEntityType
    {
      get => this.parentEntityType;
      set => this.parentEntityType = value;
    }

    public List<string> GetParameterNames()
    {
      return this.Parameters.Select<FunctionParameter, string>((Func<FunctionParameter, string>) (parameter => parameter.Name)).ToList<string>();
    }

    [OnDeserialized]
    private void ClearParameters(StreamingContext c)
    {
      if (this.parameters == null)
        return;
      this.parameters.Clear();
    }
  }
}
