// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeParallelLoanUpdateJob
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CorrespondentTradeParallelLoanUpdateJob : ITradeJob
  {
    private const string className = "CorrespondentTradeParallelLoanUpdateJob";
    private static string sw = Tracing.SwOutsideLoan;
    private bool hadErrors;
    private List<TradeLoanUpdateError> errors = new List<TradeLoanUpdateError>();
    private object loansSkippedLockObject = new object();
    private int loansSkipped;
    private bool cancellationRequested;
    private object loansCompletedLockObject = new object();
    private int loansCompleted;
    private CorrespondentTradeAssignmentItem assignmentItm;

    public event TradeUpdateEventHandler Started;

    public event TradeUpdateEventHandler Completed;

    public event TradeUpdateEventHandler Cancelled;

    public event ProgressChangedEventHandler ProgressChanged;

    public string JobGuid { get; set; }

    public void StartAsync()
    {
      Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Verbose, "In StartAsync. AssignmentItems.Count: " + (object) this.AssignmentItems.Count);
      this.State = this.State == TradeJobProcessorState.NotStarted ? TradeJobProcessorState.Started : throw new InvalidOperationException("The job must be in the NotStarted state to call StartAsync().");
      this.onStarted(new TradeUpdateEventArgs("", (object) null));
      for (int state = 0; state < this.AssignmentItems.Count; ++state)
      {
        Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Verbose, "Calling Task.Factory.StartNew for index " + (object) state);
        Task.Factory.StartNew(new Action<object>(this.processLoan), (object) state);
      }
    }

    private void processLoan(object assignmentIndexParam)
    {
      try
      {
        Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Verbose, "In processLoan for index " + (object) (int) assignmentIndexParam);
        if (this.cancellationRequested)
        {
          Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Verbose, "In processLoan, cancellation was requested for index " + (object) (int) assignmentIndexParam);
          this.incrementLoansSkipped();
          this.incrementLoansCompleted();
        }
        else
        {
          if (!(assignmentIndexParam is int index))
            throw new ArgumentException("assignmentIndexParam must be an integer object");
          this.updateLoan(this.AssignmentItems[index]);
        }
        this.onProgressChanged(new ProgressChangedEventArgs((int) (100.0 * (double) this.LoansCompleted / (double) this.AssignmentItems.Count), (object) null));
        if (this.LoansCompleted != this.AssignmentItems.Count)
          return;
        if (this.cancellationRequested)
          this.onCancelled(new TradeUpdateEventArgs("", (object) null));
        this.onCompleted(new TradeUpdateEventArgs("", (object) null));
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Error, "Error in processLoan for index " + (object) (int) assignmentIndexParam + ": " + ex.ToString());
      }
    }

    public void StartAsyncBackground()
    {
      Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Verbose, "In StartAsync. AssignmentItems.Count: " + (object) this.AssignmentItems.Count);
      if (this.State != TradeJobProcessorState.NotStarted)
        throw new InvalidOperationException("The job must be in the NotStarted state to call StartAsyncBackground().");
      Task[] taskArray = new Task[this.AssignmentItems.Count];
      this.onStarted(new TradeUpdateEventArgs("", (object) null));
      Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Verbose, "Using Parallel ForEach");
      List<CorrespondentTradeAssignmentItem> assignmentItems = this.AssignmentItems;
      ParallelOptions parallelOptions = new ParallelOptions();
      parallelOptions.MaxDegreeOfParallelism = 10;
      Action<CorrespondentTradeAssignmentItem> body = (Action<CorrespondentTradeAssignmentItem>) (item =>
      {
        Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Verbose, "Processing loan guid = " + item.Assignment.Guid);
        this.processLoanBackground(item);
      });
      Parallel.ForEach<CorrespondentTradeAssignmentItem>((IEnumerable<CorrespondentTradeAssignmentItem>) assignmentItems, parallelOptions, body);
      if (this.cancellationRequested)
        this.onCancelled(new TradeUpdateEventArgs("", (object) this.JobGuid));
      this.onCompleted(new TradeUpdateEventArgs("", (object) this.JobGuid));
    }

    private void processLoanBackground(object assignmentIndexParam)
    {
      try
      {
        Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Verbose, "In processLoan for index " + (object) (int) assignmentIndexParam);
        if (this.cancellationRequested)
        {
          Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Verbose, "In processLoan, cancellation was requested for index " + (object) (int) assignmentIndexParam);
          this.incrementLoansSkipped();
          this.incrementLoansCompleted();
        }
        else
        {
          this.assignmentItm = assignmentIndexParam is int index ? this.AssignmentItems[index] : throw new ArgumentException("assignmentIndexParam must be an integer object");
          this.updateLoan(this.assignmentItm);
        }
        this.onProgressChanged(new ProgressChangedEventArgs((int) (100.0 * (double) this.LoansCompleted / (double) this.AssignmentItems.Count), (object) this.JobGuid));
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Error, "Error in processLoan for index " + (object) (int) assignmentIndexParam + ": " + ex.ToString());
      }
    }

    private void processLoanBackground(CorrespondentTradeAssignmentItem assignmentParam)
    {
      if (this.cancellationRequested)
      {
        this.incrementLoansSkipped();
        this.incrementLoansCompleted();
      }
      else
      {
        this.assignmentItm = assignmentParam;
        this.updateLoan(this.assignmentItm);
      }
      this.onProgressChanged(new ProgressChangedEventArgs((int) (100.0 * (double) this.LoansCompleted / (double) this.AssignmentItems.Count), (object) this.JobGuid));
    }

    private void updateLoan(CorrespondentTradeAssignmentItem assignmentItm)
    {
      try
      {
        if (Session.LoanData != null && assignmentItm.Assignment.Guid == Session.LoanData.GUID)
        {
          if (assignmentItm.Assignment.PendingStatus == CorrespondentTradeLoanStatus.Unassigned)
            Session.CorrespondentTradeManager.CommitPendingTradeStatus(assignmentItm.Trade.TradeID, assignmentItm.Assignment.Guid, assignmentItm.Assignment.PendingStatus, assignmentItm.Assignment.Rejected);
          throw new TradeLoanUpdateException(new TradeLoanUpdateError(assignmentItm.Assignment.Guid, assignmentItm.Assignment.PipelineInfo, "Loan file is currently opened.  Please close this loan file first before it can be updated."));
        }
        if (!assignmentItm.Assignment.Rejected)
          assignmentItm.Assignment.Rejected = Session.CorrespondentTradeManager.GetTradeForRejectedLoan(assignmentItm.Assignment.Guid) != null;
        if (!new CorrespondentTradeProcesses().WorkOneLoan(assignmentItm.Assignment, assignmentItm.Trade, this.ForceUpdateOfAllLoans, this.SkipFieldList, assignmentItm.SecurityPrice))
          this.incrementLoansSkipped();
        this.incrementLoansCompleted();
      }
      catch (TradeLoanUpdateException ex)
      {
        this.hadErrors = true;
        this.errors.Add(ex.Error);
        this.incrementLoansCompleted();
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentTradeParallelLoanUpdateJob.sw, nameof (CorrespondentTradeParallelLoanUpdateJob), TraceLevel.Error, "Error in updateLoan for " + assignmentItm.Assignment.Guid + ": " + ex.ToString());
        this.hadErrors = true;
        this.errors.Add(new TradeLoanUpdateError(assignmentItm.Assignment.Guid, assignmentItm.Assignment.PipelineInfo, "An error occurred during update."));
        this.incrementLoansCompleted();
      }
    }

    public void CancelAsync() => this.cancellationRequested = true;

    public TradeJobProcessorState State { get; set; }

    public List<CorrespondentTradeAssignmentItem> AssignmentItems { get; set; }

    public bool ForceUpdateOfAllLoans { get; set; }

    public List<string> SkipFieldList { get; set; }

    public bool HadErrors => this.hadErrors;

    public List<TradeLoanUpdateError> Errors => this.errors;

    public int LoansCompleted
    {
      get
      {
        lock (this.loansCompletedLockObject)
          return this.loansCompleted;
      }
    }

    public int LoansSkipped
    {
      get
      {
        lock (this.loansSkippedLockObject)
          return this.loansSkipped;
      }
    }

    private void incrementLoansCompleted()
    {
      lock (this.loansCompletedLockObject)
        ++this.loansCompleted;
    }

    private void incrementLoansSkipped()
    {
      lock (this.loansSkippedLockObject)
        ++this.loansSkipped;
    }

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
