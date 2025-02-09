// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ExpressionParser.TransientDataObjectExpression
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.ExpressionParser
{
  [DataContract]
  public class TransientDataObjectExpression : Expression
  {
    public TransientDataObjectExpression()
    {
    }

    public TransientDataObjectExpression(string text, ValueType returnType = ValueType.None)
    {
      this.parentEntityType = CalculationUtility.GetRootEntityType();
      this.ReturnType = returnType;
      this.Text = text == null ? string.Empty : text;
      this.Type = ExpressionType.TransientDataObject;
    }

    public string ParentEntityType
    {
      get => this.parentEntityType;
      set => this.parentEntityType = value;
    }
  }
}
