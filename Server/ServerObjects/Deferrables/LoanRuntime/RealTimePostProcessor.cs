// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.RealTimePostProcessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  internal sealed class RealTimePostProcessor(DeferrableLoanTransaction transaction) : 
    ProcessorBase(transaction),
    IPostProcessor,
    IDeferrableProcessor
  {
    public override void InternalExecute()
    {
    }

    public override string GetKey() => this.GetType().ToString();

    public override DeferrableType GetDeferrableType() => DeferrableType.RealTime;
  }
}
