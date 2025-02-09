// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.DeferredPostProcessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.ElliEnum;
using System;
using System.IO;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  internal sealed class DeferredPostProcessor(DeferrableLoanTransaction transaction) : 
    ProcessorBase(transaction),
    IPostProcessor,
    IDeferrableProcessor
  {
    private bool EnableWaitOnSnapshotAndPublish;

    public override void InternalExecute()
    {
      this.CurrentTransaction.CurrentContext.PostProcessorExecuted = true;
      if (EncompassServerMode.Service == EncompassServer.ServerMode)
        return;
      this.CurrentTransaction.AddCheckpoint("Before publishing");
      DeferrableDataBag<AsyncProcessMessage> dataBag = new DeferrableDataBag<AsyncProcessMessage>(this.CurrentTransaction.CurrentContext.DataBag.Data);
      string encompassDataDir = ClientContext.GetCurrent().Settings.EncompassDataDir;
      string loanFilePath = this.CurrentTransaction.CurrentContext.GetLoanFilePath();
      ClientContext current = ClientContext.GetCurrent();
      if (!this.CurrentTransaction.DeliveryTaskList.HasMessages)
      {
        TraceLog.WriteInfo(nameof (DeferredPostProcessor), string.Format("No message to publish, loan {0}, from {1}", (object) this.CurrentTransaction.CurrentContext.DataBag.Get<string>("LoanId"), (object) Environment.MachineName));
      }
      else
      {
        this.EnableWaitOnSnapshotAndPublish = true;
        if (this.EnableWaitOnSnapshotAndPublish)
          this.Run(encompassDataDir, loanFilePath, current, dataBag);
        else
          this.RunAsync(encompassDataDir, loanFilePath, current, dataBag);
        this.CurrentTransaction.AddCheckpoint("After publishing");
      }
    }

    private void Run(
      string encompassDataDir,
      string loanFilePath,
      ClientContext context,
      DeferrableDataBag<AsyncProcessMessage> dataBag = null)
    {
      string path2 = string.IsNullOrEmpty(this.CurrentTransaction.CurrentContext.CurrentLoan.Identity.LoanFolder) ? string.Empty : this.CurrentTransaction.CurrentContext.CurrentLoan.Identity.LoanFolder;
      if (!string.IsNullOrEmpty(path2))
      {
        string val = Path.Combine("Loans", path2, this.CurrentTransaction.CurrentContext.CurrentLoan.Identity.Guid, "Versions");
        this.CurrentTransaction.CurrentContext.DataBag.Set("LoanPath", (object) val);
        dataBag.Set("LoanPath", (object) val);
      }
      else
        TraceLog.WriteWarning(nameof (DeferredPostProcessor), "loanFolder is null or empty");
      if (this.CurrentTransaction.SwitchedToRealTimeMode)
        return;
      this.CurrentTransaction.DeliveryTaskList.RunBatch<AsyncProcessMessage>(context, dataBag);
      TraceLog.WriteInfo(nameof (DeferredPostProcessor), string.Format("Published deferrable messages for loan {0}-{1}, {2}. From {3}", (object) dataBag.Get<string>("LoanId"), (object) dataBag.Get<LoanActionType>("LoanActionType"), (object) dataBag.Get<string>("LoanPath"), (object) Environment.MachineName));
    }

    private void RunAsync(
      string encompassDataDir,
      string loanFilePath,
      ClientContext context,
      DeferrableDataBag<AsyncProcessMessage> dataBag)
    {
      Task.Run((Action) (() => this.Run(encompassDataDir, loanFilePath, context, dataBag)));
    }

    private void TakeSnapshot(
      string encompassDataDir,
      string loanFilePath,
      DeferrableDataBag<AsyncProcessMessage> threadSafeDataBag = null)
    {
      DeferrableDataBag<AsyncProcessMessage> deferrableDataBag = threadSafeDataBag != null ? threadSafeDataBag : this.CurrentTransaction.CurrentContext.DataBag;
      string targetLoanFolderPath = deferrableDataBag.Get<string>("LoanPath");
      if (string.IsNullOrWhiteSpace(targetLoanFolderPath))
      {
        targetLoanFolderPath = this.CurrentTransaction.CurrentContext.GetTargetLoanFolderPath();
        deferrableDataBag.Set("LoanPath", (object) targetLoanFolderPath);
      }
      if (LoanFileSnapshotUtils.TakeSnapshot(encompassDataDir, loanFilePath, "_^_After_loan.em", targetLoanFolderPath))
        return;
      this.CurrentTransaction.SwitchedToRealTimeMode = true;
    }

    public override string GetKey() => this.GetType().ToString();

    public override DeferrableType GetDeferrableType() => DeferrableType.Deferred;
  }
}
