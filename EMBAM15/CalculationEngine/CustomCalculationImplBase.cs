// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CustomCalculationImplBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Customization;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public abstract class CustomCalculationImplBase : ContextBoundCodeImplBase, ICustomCalculationImpl
  {
    public object Calculate(ICalculationContext context)
    {
      this.EstablishContext((IExecutionContext) context);
      return this.ExecuteCalculation();
    }

    protected abstract object ExecuteCalculation();

    protected IReadOnlyFieldSource Fields => (IReadOnlyFieldSource) this.Context.Fields;

    protected string Audit(object value, int infoType)
    {
      switch (infoType)
      {
        case 0:
          return this.Context.UserID;
        case 1:
          return this.Context.UserName;
        case 2:
          return this.Context.Timestamp.ToString("MM/dd/yyyy hh:mm tt");
        default:
          return "";
      }
    }
  }
}
