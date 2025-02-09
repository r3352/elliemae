// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ExpressionParser.Expression
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.ExpressionParser
{
  [DataContract]
  public class Expression : IExtensibleDataObject
  {
    private const string className = "Expression";
    private ExtensionDataObject extensionData;

    [DataMember(Name = "ReturnType")]
    protected Elli.CalculationEngine.Common.ValueType returnType { get; set; }

    [DataMember(Name = "Text")]
    protected string text { get; set; }

    [DataMember(Name = "Type")]
    protected ExpressionType type { get; set; }

    [DataMember(Name = "ParentEntityType", Order = 1)]
    protected string parentEntityType { get; set; }

    public Elli.CalculationEngine.Common.ValueType ReturnType
    {
      get => this.returnType;
      set => this.returnType = value;
    }

    public ExpressionType Type
    {
      get => this.type;
      set => this.type = value;
    }

    public string Text
    {
      get => this.text;
      set => this.text = value;
    }

    public override string ToString() => this.text;

    public string ToSourceCode(string calculationFieldId, Assembly objectModelAssembly)
    {
      return FieldReplacementRegex.FormatSourceCode(this.Text, calculationFieldId, this.parentEntityType, objectModelAssembly).Replace("\r", " ").Replace("\n", " ");
    }

    public string[] ToSourceCodeArray(string calculationFieldId, Assembly objectModelAssembly)
    {
      return FieldReplacementRegex.FormatSourceCode(this.Text, calculationFieldId, this.parentEntityType, objectModelAssembly).Split(new char[2]
      {
        '\r',
        '\n'
      }, StringSplitOptions.RemoveEmptyEntries);
    }

    public void Invoke()
    {
      Tracing.Log(TraceLevel.Info, nameof (Expression), "Calculation: " + this.Text);
    }

    public virtual ExtensionDataObject ExtensionData
    {
      get => this.extensionData;
      set => this.extensionData = value;
    }
  }
}
