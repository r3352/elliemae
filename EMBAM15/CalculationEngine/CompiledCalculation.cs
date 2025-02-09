// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CompiledCalculation
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class CompiledCalculation : ICustomCalculationImpl
  {
    private CustomCalculation calc;
    private ICustomCalculationImpl impl;

    public CompiledCalculation(CustomCalculation calc, ICustomCalculationImpl impl)
    {
      this.calc = calc;
      this.impl = impl;
    }

    public CompiledCalculation(string userCode, ICustomCalculationImpl impl)
      : this(new CustomCalculation(userCode), impl)
    {
    }

    public CustomCalculation Calculation => this.calc;

    public object Calculate(ICalculationContext context) => this.impl.Calculate(context);
  }
}
