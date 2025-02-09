// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ExpressionParser.TemplateExpression
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
  public class TemplateExpression : CalculationExpression
  {
    private List<string> parameters;

    public TemplateExpression()
    {
    }

    public TemplateExpression(string text, ValueType returnType = ValueType.None)
    {
      this.parentEntityType = CalculationUtility.GetRootEntityType();
      this.ReturnType = returnType;
      this.Text = text == null ? string.Empty : text;
      this.Type = ExpressionType.Template;
    }

    public List<string> Parameters
    {
      get
      {
        if (this.parameters == null || this.parameters.Count == 0)
          this.parameters = TemplateReplacementRegex.ParseTemplateParameters(this.Text);
        return this.parameters;
      }
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
