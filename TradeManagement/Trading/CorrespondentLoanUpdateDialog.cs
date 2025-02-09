// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentLoanUpdateDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CorrespondentLoanUpdateDialog : Form
  {
    private CorrespondentTradeProcesses.ActionType action;
    private CorrespondentTradeLoanAssignmentManager assignmentMgr;
    private int[] tradeIds;
    private CorrespondentTradeInfo correspondentTrade;
    private bool forceUpdateOfAllLoans;
    private Decimal securityPrice;
    private Queue<CorrespondentTradeAssignmentItem> assignmentQueue = new Queue<CorrespondentTradeAssignmentItem>();
    private List<string> skipFieldList = new List<string>();
    private CorrespondentTradeParallelLoanUpdateJob parallelLoanJob;
    private bool skipSyncDialog = true;
    private int loanCount;
    private bool cancelRequested;
    private List<TradeLoanUpdateError> errors = new List<TradeLoanUpdateError>();
    private int sequentialCompletedCount;
    private int parallelCompletedCount;
    private int completedSuccessfullyCount;
    private int completedWithErrorsCount;
    private IContainer components;
    private ProgressBar progressBar1;
    private Button buttonCancel;
    private Label labelMessage;

    public CorrespondentLoanUpdateDialog(
      CorrespondentTradeProcesses.ActionType action,
      CorrespondentTradeInfo correspondentTrade,
      CorrespondentTradeLoanAssignmentManager assignmentMgr,
      bool forceUpdateOfAllLoans,
      Decimal securityPrice,
      bool skipSyncDialog)
    {
      this.InitializeComponent();
      this.correspondentTrade = correspondentTrade;
      this.assignmentMgr = assignmentMgr;
      this.action = action;
      this.forceUpdateOfAllLoans = forceUpdateOfAllLoans;
      this.securityPrice = securityPrice;
      this.skipSyncDialog = skipSyncDialog;
    }

    public CorrespondentLoanUpdateDialog(
      CorrespondentTradeProcesses.ActionType action,
      int[] tradeIds,
      bool forceUpdateOfAllLoans,
      bool skipSyncDialog)
    {
      this.InitializeComponent();
      this.tradeIds = tradeIds;
      this.action = action;
      this.forceUpdateOfAllLoans = forceUpdateOfAllLoans;
      this.skipSyncDialog = skipSyncDialog;
    }

    private void TradeLoanUpdateDialog_Shown(object sender, EventArgs e)
    {
      this.startParallelProcess();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.labelMessage.Text = "Cancelling, waiting for current updates to complete...";
      this.Cursor = Cursors.WaitCursor;
      this.buttonCancel.Enabled = false;
      this.progressBar1.Visible = false;
      this.cancelRequested = true;
      if (this.parallelLoanJob == null)
        return;
      this.parallelLoanJob.CancelAsync();
    }

    public void updateMessageTextForSingleItem(CorrespondentTradeAssignmentItem assignmentItm)
    {
      if (assignmentItm.Assignment.PendingStatus == CorrespondentTradeLoanStatus.Assigned)
        this.labelMessage.Text = "Assigning loan " + assignmentItm.Assignment.PipelineInfo.LoanNumber + " to trade...";
      else if (assignmentItm.Assignment.PendingStatus == CorrespondentTradeLoanStatus.Unassigned)
        this.labelMessage.Text = "Removing loan " + assignmentItm.Assignment.PipelineInfo.LoanNumber + " from trade...";
      else
        this.labelMessage.Text = "Updating trade status for loan " + assignmentItm.Assignment.PipelineInfo.LoanNumber + "...";
    }

    private void startParallelProcess()
    {
      if (this.assignmentMgr != null)
      {
        foreach (CorrespondentTradeLoanAssignment assignment in !this.forceUpdateOfAllLoans ? this.assignmentMgr.GetPendingLoans() : this.assignmentMgr.GetPendingAndAssignedLoans())
          this.assignmentQueue.Enqueue(new CorrespondentTradeAssignmentItem(this.correspondentTrade, assignment, this.securityPrice));
      }
      else
      {
        if (this.tradeIds == null)
          throw new InvalidOperationException("Either assignmentMgr or tradeIds must have a value.");
        foreach (int tradeId in this.tradeIds)
        {
          CorrespondentTradeInfo trade = Session.CorrespondentTradeManager.GetTrade(tradeId);
          if (trade != null)
          {
            foreach (CorrespondentTradeLoanAssignment assignment in new CorrespondentTradeLoanAssignmentManager(Session.SessionObjects, trade.TradeID, false))
              this.assignmentQueue.Enqueue(new CorrespondentTradeAssignmentItem(trade, assignment, this.securityPrice));
          }
        }
      }
      if (this.assignmentQueue.Count == 0)
      {
        this.updateComplete();
      }
      else
      {
        this.loanCount = this.assignmentQueue.Count;
        this.labelMessage.Text = "Waiting to update loans...";
        this.progressBar1.Increment(5);
        if (this.skipSyncDialog)
        {
          this.skipFieldList = new List<string>();
        }
        else
        {
          CorrespondentLoanSyncDlg correspondentLoanSyncDlg = new CorrespondentLoanSyncDlg();
          int num = (int) correspondentLoanSyncDlg.ShowDialog((IWin32Window) Form.ActiveForm);
          this.skipFieldList = correspondentLoanSyncDlg.SkipFieldList;
        }
        this.labelMessage.Text = "Updating loans...";
        this.parallelLoanJob = new CorrespondentTradeParallelLoanUpdateJob();
        this.parallelLoanJob.AssignmentItems = this.assignmentQueue.ToList<CorrespondentTradeAssignmentItem>();
        this.parallelLoanJob.ForceUpdateOfAllLoans = this.forceUpdateOfAllLoans;
        this.parallelLoanJob.SkipFieldList = this.skipFieldList;
        this.parallelLoanJob.Completed += new TradeUpdateEventHandler(this.ParallelLoanUpdateJob_Completed);
        this.parallelLoanJob.ProgressChanged += new ProgressChangedEventHandler(this.ParallelLoanJob_ProgressChanged);
        this.parallelLoanJob.StartAsync();
      }
    }

    private void ParallelLoanJob_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      ProgressChangedEventHandler method = new ProgressChangedEventHandler(this.ParallelLoanJob_ProgressChanged);
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) method, sender, (object) e);
      }
      else
      {
        this.parallelCompletedCount = this.parallelLoanJob.LoansCompleted;
        this.progressBar1.Value = (int) (100.0 * (double) this.TotalCompletedCount / (this.loanCount == 0 ? 1.0 : (double) this.loanCount));
      }
    }

    private void ParallelLoanUpdateJob_Completed(object sender, TradeUpdateEventArgs eventArgs)
    {
      TradeUpdateEventHandler method = new TradeUpdateEventHandler(this.ParallelLoanUpdateJob_Completed);
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) method, sender, (object) eventArgs);
      }
      else
      {
        this.parallelLoanJob.Completed -= new TradeUpdateEventHandler(this.ParallelLoanUpdateJob_Completed);
        this.parallelLoanJob.ProgressChanged -= new ProgressChangedEventHandler(this.ParallelLoanJob_ProgressChanged);
        this.parallelCompletedCount = this.parallelLoanJob.LoansCompleted;
        if (this.parallelLoanJob.HadErrors)
        {
          this.errors.AddRange((IEnumerable<TradeLoanUpdateError>) this.parallelLoanJob.Errors);
          this.completedWithErrorsCount += this.parallelLoanJob.Errors.Count;
        }
        this.completedSuccessfullyCount += this.parallelLoanJob.LoansCompleted - this.parallelLoanJob.LoansSkipped - this.parallelLoanJob.Errors.Count;
        this.labelMessage.Text = eventArgs.Message;
        this.progressBar1.Value = (int) (100.0 * (double) this.TotalCompletedCount / (this.loanCount == 0 ? 1.0 : (double) this.loanCount));
        if (this.cancelRequested)
        {
          this.Cursor = Cursors.Default;
          this.updateComplete();
        }
        else
          this.updateComplete();
      }
    }

    private void updateComplete()
    {
      this.progressBar1.Value = 100;
      this.Close();
    }

    public int LoanCount => this.loanCount;

    public bool CancelRequested => this.cancelRequested;

    public List<TradeLoanUpdateError> Errors => this.errors;

    public int TotalCompletedCount => this.sequentialCompletedCount + this.parallelCompletedCount;

    public int CompletedSuccessfullyCount => this.completedSuccessfullyCount;

    public int CompletedWithErrorsCount => this.completedWithErrorsCount;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.progressBar1 = new ProgressBar();
      this.buttonCancel = new Button();
      this.labelMessage = new Label();
      this.SuspendLayout();
      this.progressBar1.Location = new Point(12, 40);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(389, 23);
      this.progressBar1.TabIndex = 0;
      this.buttonCancel.Location = new Point(326, 79);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 23);
      this.buttonCancel.TabIndex = 1;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
      this.labelMessage.AutoSize = true;
      this.labelMessage.Location = new Point(12, 24);
      this.labelMessage.Name = "labelMessage";
      this.labelMessage.Size = new Size(50, 13);
      this.labelMessage.TabIndex = 2;
      this.labelMessage.Text = "Message";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(413, 114);
      this.ControlBox = false;
      this.Controls.Add((Control) this.labelMessage);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.progressBar1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = "TradeLoanUpdateDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Trade Management";
      this.Shown += new EventHandler(this.TradeLoanUpdateDialog_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
