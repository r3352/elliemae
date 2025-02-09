// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SingleLoanUpdateJob
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class SingleLoanUpdateJob : ITradeJob
  {
    private bool hadErrors;
    private TradeLoanUpdateError error;
    private bool loanSkipped;

    public event TradeUpdateEventHandler Started;

    public event TradeUpdateEventHandler Completed;

    public event TradeUpdateEventHandler Cancelled;

    public event ProgressChangedEventHandler ProgressChanged;

    public TradeJobProcessorState State { get; set; }

    public LoanTradeAssignment Assignment { get; set; }

    public LoanTradeInfo Trade { get; set; }

    public bool ForceUpdateOfAllLoans { get; set; }

    public List<string> SkipFieldList { get; set; }

    public Decimal SecurityPrice { get; set; }

    public bool HadErrors => this.hadErrors;

    public TradeLoanUpdateError Error => this.error;

    public bool LoanSkipped => this.loanSkipped;

    public void StartAsync()
    {
      this.State = this.State == TradeJobProcessorState.NotStarted ? TradeJobProcessorState.Started : throw new InvalidOperationException("The job must be in the NotStarted state to call StartAsync().");
      this.onStarted(new TradeUpdateEventArgs("Loan " + this.Assignment.PipelineInfo.LoanNumber + " started.", (object) this.Assignment.Guid));
      Task.Factory.StartNew((Action) (() => this.updateLoan()));
    }

    private void updateLoan()
    {
      try
      {
        if (Session.LoanData != null && this.Assignment.Guid == Session.LoanData.GUID)
        {
          if (this.Assignment.PendingStatus == LoanTradeStatus.Unassigned)
            Session.LoanTradeManager.CommitPendingTradeStatus(this.Trade.TradeID, this.Assignment.Guid, this.Assignment.PendingStatus, this.Assignment.Rejected);
          throw new TradeLoanUpdateException(new TradeLoanUpdateError(this.Assignment.Guid, this.Assignment.PipelineInfo, "The loan is currently opened.  Please close this loan file first before it can be updated."));
        }
        if (!this.Assignment.Rejected)
          this.Assignment.Rejected = Session.LoanTradeManager.GetTradeForRejectedLoan(this.Assignment.Guid) != null;
        this.loanSkipped = !new TradeProcesses2().WorkOneLoan(this.Assignment, this.Trade, this.ForceUpdateOfAllLoans, this.SkipFieldList, this.SecurityPrice);
      }
      catch (TradeLoanUpdateException ex)
      {
        this.hadErrors = true;
        this.error = ex.Error;
      }
      this.onCompleted(new TradeUpdateEventArgs("Loan " + this.Assignment.PipelineInfo.LoanNumber + " completed.", (object) this.Assignment.Guid));
    }

    public int LoansSkipped => 0;

    public int LoansCompleted => 1;

    public List<TradeLoanUpdateError> Errors => (List<TradeLoanUpdateError>) null;

    public void StartAsyncBackground()
    {
    }

    public string JobGuid { get; set; }

    public List<object> AssignmentItems { get; set; }

    public void CancelAsync() => throw new NotImplementedException();

    private void onStarted(TradeUpdateEventArgs e)
    {
      if (this.Started == null)
        return;
      this.Started((object) this, e);
    }

    private void onCompleted(TradeUpdateEventArgs e)
    {
      if (this.Completed == null)
        return;
      this.Completed((object) this, e);
    }

    private void onCancelled(TradeUpdateEventArgs e)
    {
      if (this.Cancelled == null)
        return;
      this.Cancelled((object) this, e);
    }

    private void onProgressChanged(ProgressChangedEventArgs e)
    {
      if (this.ProgressChanged == null)
        return;
      this.ProgressChanged((object) this, e);
    }
  }
}
