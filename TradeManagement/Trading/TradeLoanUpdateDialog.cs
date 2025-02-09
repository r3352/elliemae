// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeLoanUpdateDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeLoanUpdateDialog : Form
  {
    private Queue<AssignmentItem> assignmentQueue = new Queue<AssignmentItem>();
    private SingleLoanUpdateJob singleLoanJob;
    private bool applySkipListToAllLoans;
    private List<string> skipFieldList = new List<string>();
    private bool forceUpdateOfAllLoans;
    private Decimal securityPrice;
    private ParallelLoanUpdateJob parallelLoanJob;
    private int loanCount;
    private bool cancelRequested;
    private List<TradeLoanUpdateError> errors = new List<TradeLoanUpdateError>();
    private int sequentialCompletedCount;
    private int parallelCompletedCount;
    private int completedSuccessfullyCount;
    private int completedWithErrorsCount;
    private IContainer components;
    private ProgressBar progressBar1;
    private Label labelMessage;

    public TradeLoanSyncDlg syncDlg { get; set; }

    public TradeLoanUpdateDialog(
      Queue<AssignmentItem> assignmentQueue,
      bool applySkipListToAllLoans,
      List<string> skipFieldList,
      Decimal securityPrice,
      bool forceUpdateOfAllLoans)
    {
      this.InitializeComponent();
      this.assignmentQueue = assignmentQueue;
      this.applySkipListToAllLoans = applySkipListToAllLoans;
      this.skipFieldList = skipFieldList;
      this.securityPrice = securityPrice;
      this.forceUpdateOfAllLoans = forceUpdateOfAllLoans;
    }

    private void TradeLoanUpdateDialog_Shown(object sender, EventArgs e)
    {
      this.startSequentialProcess();
    }

    private void startSequentialProcess()
    {
      if (this.assignmentQueue.Count == 0)
      {
        this.updateComplete();
      }
      else
      {
        this.loanCount = this.assignmentQueue.Count;
        this.progressBar1.Increment(5);
        this.updateMessageTextForSingleItem(this.assignmentQueue.Peek());
        if (this.applySkipListToAllLoans)
        {
          this.parallelProcess();
        }
        else
        {
          AssignmentItem assignmentItem = this.assignmentQueue.Dequeue();
          this.singleLoanJob = new SingleLoanUpdateJob();
          this.singleLoanJob.Assignment = assignmentItem.Assignment;
          this.singleLoanJob.Trade = assignmentItem.LoanTrade;
          this.singleLoanJob.ForceUpdateOfAllLoans = this.forceUpdateOfAllLoans;
          this.singleLoanJob.SkipFieldList = this.skipFieldList;
          this.singleLoanJob.SecurityPrice = assignmentItem.SecurityPrice;
          this.singleLoanJob.Completed += new TradeUpdateEventHandler(this.SingleLoanUpdateJob_Completed);
          this.singleLoanJob.StartAsync();
        }
      }
    }

    private void SingleLoanUpdateJob_Completed(object sender, TradeUpdateEventArgs eventArgs)
    {
      TradeUpdateEventHandler method = new TradeUpdateEventHandler(this.SingleLoanUpdateJob_Completed);
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) method, sender, (object) eventArgs);
      }
      else
      {
        this.singleLoanJob.Completed -= new TradeUpdateEventHandler(this.SingleLoanUpdateJob_Completed);
        ++this.sequentialCompletedCount;
        if (this.singleLoanJob.HadErrors)
        {
          this.errors.Add(this.singleLoanJob.Error);
          ++this.completedWithErrorsCount;
          string str = "Loan " + this.singleLoanJob.Error.LoanInfo.LoanNumber + ": " + this.singleLoanJob.Error.Message;
          int num = (int) Utils.Dialog((IWin32Window) this, this.singleLoanJob.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else if (!this.singleLoanJob.LoanSkipped)
          ++this.completedSuccessfullyCount;
        this.progressBar1.Value = (int) (100.0 * (double) this.sequentialCompletedCount / (this.loanCount == 0 ? 1.0 : (double) this.loanCount));
        if (this.cancelRequested)
        {
          this.Cursor = Cursors.Default;
          this.labelMessage.Text = "Cancelled.";
          this.updateComplete();
        }
        else if (this.assignmentQueue.Count == 0)
        {
          this.labelMessage.Text = "All Done.";
          this.updateComplete();
        }
        else
        {
          AssignmentItem assignmentItm = this.assignmentQueue.Peek();
          this.updateMessageTextForSingleItem(assignmentItm);
          if (this.syncDlg == null)
            this.syncDlg = new TradeLoanSyncDlg(assignmentItm.LoanTrade.PriceAdjustments, assignmentItm.LoanTrade);
          int num = (int) this.syncDlg.ShowDialog((IWin32Window) Form.ActiveForm);
          if (this.syncDlg.DialogResult == DialogResult.OK)
          {
            this.skipFieldList = this.syncDlg.SkipFieldList;
            this.applySkipListToAllLoans = this.syncDlg.ApplyToAll;
            if (this.applySkipListToAllLoans)
            {
              this.labelMessage.Text = "All Done.";
              this.updateComplete();
            }
            else
            {
              AssignmentItem assignmentItem = this.assignmentQueue.Dequeue();
              this.singleLoanJob = new SingleLoanUpdateJob();
              this.singleLoanJob.Assignment = assignmentItem.Assignment;
              this.singleLoanJob.Trade = assignmentItem.LoanTrade;
              this.singleLoanJob.ForceUpdateOfAllLoans = this.forceUpdateOfAllLoans;
              this.singleLoanJob.SkipFieldList = this.skipFieldList;
              this.singleLoanJob.SecurityPrice = this.securityPrice;
              this.singleLoanJob.Completed += new TradeUpdateEventHandler(this.SingleLoanUpdateJob_Completed);
              this.singleLoanJob.StartAsync();
            }
          }
          else
          {
            this.cancelRequested = true;
            this.Cursor = Cursors.Default;
            this.labelMessage.Text = "Cancelled.";
            this.updateComplete();
          }
        }
      }
    }

    public void updateMessageTextForSingleItem(AssignmentItem assignmentItm)
    {
      if (assignmentItm.Assignment.PendingStatus == LoanTradeStatus.Assigned)
        this.labelMessage.Text = "Assigning loan " + assignmentItm.Assignment.PipelineInfo.LoanNumber + " to trade...";
      else if (assignmentItm.Assignment.PendingStatus == LoanTradeStatus.Unassigned)
        this.labelMessage.Text = "Removing loan " + assignmentItm.Assignment.PipelineInfo.LoanNumber + " from trade...";
      else
        this.labelMessage.Text = "Updating trade status for loan " + assignmentItm.Assignment.PipelineInfo.LoanNumber + "...";
    }

    private void parallelProcess()
    {
      this.labelMessage.Text = "Updating loans...";
      this.parallelLoanJob = new ParallelLoanUpdateJob();
      this.parallelLoanJob.AssignmentItems = this.assignmentQueue.ToList<AssignmentItem>();
      this.parallelLoanJob.ForceUpdateOfAllLoans = this.forceUpdateOfAllLoans;
      this.parallelLoanJob.SkipFieldList = this.skipFieldList;
      this.parallelLoanJob.Completed += new TradeUpdateEventHandler(this.ParallelLoanUpdateJob_Completed);
      this.parallelLoanJob.ProgressChanged += new ProgressChangedEventHandler(this.ParallelLoanJob_ProgressChanged);
      this.parallelLoanJob.StartAsync();
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
      this.labelMessage = new Label();
      this.SuspendLayout();
      this.progressBar1.Location = new Point(12, 40);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(389, 23);
      this.progressBar1.TabIndex = 0;
      this.labelMessage.AutoSize = true;
      this.labelMessage.Location = new Point(12, 24);
      this.labelMessage.Name = "labelMessage";
      this.labelMessage.Size = new Size(50, 13);
      this.labelMessage.TabIndex = 2;
      this.labelMessage.Text = "Message";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(413, 105);
      this.ControlBox = false;
      this.Controls.Add((Control) this.labelMessage);
      this.Controls.Add((Control) this.progressBar1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (TradeLoanUpdateDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Trade Management";
      this.Shown += new EventHandler(this.TradeLoanUpdateDialog_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
