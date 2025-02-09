// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.SyncProgressManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using System;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class SyncProgressManager : IDisposable
  {
    private CancellationTokenSource _tokenSource;
    private bool isDisposed;

    public IProgress<Ratio> Progress { get; private set; }

    public EstimatingProgressDialog Dialog { get; private set; }

    public CancellationToken CancellationToken { get; set; }

    public SyncProgressManager(int maxValue = 0)
    {
      EstimatingProgressDialog estimatingProgressDialog1 = new EstimatingProgressDialog();
      estimatingProgressDialog1.Title = "Enhanced Conditions Import";
      estimatingProgressDialog1.OverallDescription = "Importing Enhanced Conditions Templates...";
      estimatingProgressDialog1.MaxValue = maxValue;
      EstimatingProgressDialog estimatingProgressDialog2 = estimatingProgressDialog1;
      this.Dialog = estimatingProgressDialog1;
      EstimatingProgressDialog progressDialog = estimatingProgressDialog2;
      Action<int, int> updateCurrentDesc = (Action<int, int>) ((val, max) => progressDialog.CurrentDescription = string.Format("Completed: {0} of {1}", (object) val, (object) max));
      IProgress<Ratio> progress;
      this.Progress = progress = (IProgress<Ratio>) new System.Progress<Ratio>((Action<Ratio>) (value =>
      {
        EstimatingProgressDialog estimatingProgressDialog3 = progressDialog;
        if ((estimatingProgressDialog3 != null ? (estimatingProgressDialog3.DialogResult != 0 ? 1 : 0) : 1) != 0)
          return;
        progressDialog.MaxValue = value.Total;
        if (value.Total > 0)
          progressDialog.CurrentValue = value.Complete;
        updateCurrentDesc(value.Complete, value.Total);
      }));
      CancellationTokenSource tokenSource = this._tokenSource = new CancellationTokenSource();
      CancellationToken token;
      this.CancellationToken = token = tokenSource.Token;
      Action cancellationHandler = (Action) (() =>
      {
        if (tokenSource.IsCancellationRequested)
          return;
        tokenSource.Cancel();
      });
      progressDialog.FormClosed += (FormClosedEventHandler) ((vis, ce) => cancellationHandler());
      progressDialog.OnCancelRequested += (EventHandler) ((vis, ce) => cancellationHandler());
      progressDialog.StartEstimating(tokenSource.Token);
      updateCurrentDesc(0, maxValue);
      progressDialog.Show();
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.isDisposed)
        return;
      if (disposing)
        this._tokenSource.Dispose();
      this.isDisposed = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
