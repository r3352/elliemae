// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationContext
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.DataSource;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public class CalculationContext(IDataEntity dataSource) : 
    ExecutionContext(dataSource, true),
    ICalculationContext,
    IExecutionContext
  {
    public override object Clone()
    {
      return (object) new CalculationContext((IDataEntity) this.ContextRootEntity);
    }
  }
}
