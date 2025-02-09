// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CalculationBuilder
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Compiler;
using System;
using System.IO;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class CalculationBuilder
  {
    private CodeWriter assemblyCode;
    private string assemblyId;

    public CalculationBuilder() => this.Clear();

    public void Compile(RuntimeContext context)
    {
      context.CompileAssembly(this.assemblyId, this.assemblyCode.ToString(), CodeLanguage.VB, new string[1]
      {
        Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase)
      });
    }

    public ICustomCalculationImpl CreateImplementation(
      CustomCalculation calc,
      RuntimeContext context)
    {
      this.Clear();
      TypeIdentifier typeId = this.Add(calc);
      this.Compile(context);
      return (ICustomCalculationImpl) context.CreateInstance(typeId, typeof (ICustomCalculationImpl));
    }

    public void Clear()
    {
      this.assemblyId = Guid.NewGuid().ToString("N");
      this.assemblyCode = new CodeWriter(CodeLanguage.VB);
      this.assemblyCode.WriteLine("Imports System");
      this.assemblyCode.WriteLine("Imports Microsoft.VisualBasic");
      this.assemblyCode.WriteLine("Imports EllieMae.EMLite.CalculationEngine");
    }

    public TypeIdentifier Add(CustomCalculation calc)
    {
      string className = "Calc_" + calc.ID;
      this.assemblyCode.StartBlock("Public Class " + className);
      this.assemblyCode.WriteLine("Inherits CustomCalculationImplBase");
      this.assemblyCode.StartBlock("Protected Overrides Function ExecuteCalculation() as Object");
      this.assemblyCode.StartRegion(calc.ID, 7);
      this.assemblyCode.WriteLine("Return " + calc.ToSourceCode());
      this.assemblyCode.EndRegion(calc.ID);
      this.assemblyCode.EndBlock("End Function");
      this.assemblyCode.EndBlock("End Class");
      return new TypeIdentifier(this.assemblyId, className);
    }
  }
}
