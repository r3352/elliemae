// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.ProcessorBase
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  internal abstract class ProcessorBase(DeferrableLoanTransaction transaction) : 
    TransactionAwareBase(transaction),
    IDeferrableProcessor
  {
    public abstract DeferrableType GetDeferrableType();

    public abstract string GetKey();

    public void Execute()
    {
      if (this.CurrentTransaction != null)
      {
        try
        {
          this.InternalExecute();
          this.CurrentTransaction.SetProcessStatus((IDeferrableProcessor) this);
        }
        catch (Exception ex)
        {
          throw new DeferredProcessException("Error executing deferrable process:", ex);
        }
      }
      this.ExecuteSuccessorProcessor();
    }

    public abstract void InternalExecute();

    private void ExecuteSuccessorProcessor()
    {
      IDeferrableProcessor successorProcessor = this.CurrentTransaction.GetInjectedSuccessorProcessor((IDeferrableProcessor) this);
      if (successorProcessor == null || !this.CurrentTransaction.NeedToRunRealTimeProcessor(successorProcessor))
        return;
      successorProcessor.Execute();
      this.CurrentTransaction.SetProcessStatus(successorProcessor);
    }
  }
}
