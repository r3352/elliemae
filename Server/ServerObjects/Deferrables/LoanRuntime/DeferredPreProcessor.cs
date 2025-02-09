// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.DeferredPreProcessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataEngine;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  internal sealed class DeferredPreProcessor(DeferrableLoanTransaction transaction) : 
    ProcessorBase(transaction),
    IPreProcessor,
    IDeferrableProcessor
  {
    public override void InternalExecute()
    {
      if (EncompassServerMode.Service == EncompassServer.ServerMode)
        return;
      this.CurrentTransaction.DeliveryTaskList.RegisterTaskHandler<AsyncProcessMessage>((IDeferrableMessageTaskHandler) new AsyncProcessMessageDeliveryTaskHandler());
      LoanIdentity identity = this.CurrentTransaction.CurrentContext.CurrentLoan.Identity;
      this.CurrentTransaction.CurrentContext.SetServerMode(EncompassServer.ServerMode);
      this.CurrentTransaction.CurrentContext.SetLoanFolder(identity.LoanFolder);
      this.CurrentTransaction.CurrentContext.SetLoanId(identity.Guid);
      if (!(this.CurrentTransaction.CurrentContext.DataBag.Get<string>("EventId") != "CreateLoan"))
        return;
      try
      {
        string path2 = string.IsNullOrEmpty(this.CurrentTransaction.CurrentContext.CurrentLoan.Identity.LoanFolder) ? string.Empty : this.CurrentTransaction.CurrentContext.CurrentLoan.Identity.LoanFolder;
        if (!string.IsNullOrEmpty(path2))
          this.CurrentTransaction.CurrentContext.DataBag.Set("LoanPath", (object) Path.Combine("Loans", path2, this.CurrentTransaction.CurrentContext.CurrentLoan.Identity.Guid, "Versions"));
        else
          TraceLog.WriteWarning(nameof (DeferredPreProcessor), "loanFolder is null or empty");
      }
      catch
      {
        this.CurrentTransaction.CurrentContext.SetPriorLoanData(this.CurrentTransaction.CurrentContext.GetPriorLoanDataPerRole());
        throw;
      }
    }

    private void TakeSnapshot()
    {
      string targetLoanFolderPath = this.CurrentTransaction.CurrentContext.DataBag.Get<string>("LoanPath");
      if (string.IsNullOrWhiteSpace(targetLoanFolderPath))
      {
        targetLoanFolderPath = this.CurrentTransaction.CurrentContext.GetTargetLoanFolderPath();
        this.CurrentTransaction.CurrentContext.DataBag.Set("LoanPath", (object) targetLoanFolderPath);
      }
      string encompassDataDir = ClientContext.GetCurrent().Settings.EncompassDataDir;
      string loanFilePath = this.CurrentTransaction.CurrentContext.GetLoanFilePath();
      if (string.IsNullOrWhiteSpace(loanFilePath) || LoanFileSnapshotUtils.TakeSnapshot(encompassDataDir, loanFilePath, "_^_Before_loan.em", targetLoanFolderPath))
        return;
      this.CurrentTransaction.SwitchedToRealTimeMode = true;
    }

    public override string GetKey() => this.GetType().ToString();

    public override DeferrableType GetDeferrableType() => DeferrableType.Deferred;
  }
}
