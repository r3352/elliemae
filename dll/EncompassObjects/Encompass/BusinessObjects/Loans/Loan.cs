// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Loan
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Export;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Workflow;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.BusinessObjects.Loans.Servicing;
using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Licensing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [ComSourceInterfaces(typeof (IPersistentObjectEvents))]
  public class Loan : SessionBoundObject, ILoan, IDisposable
  {
    private readonly ScopedEventHandler<CancelableEventArgs> beforeCommit;
    private readonly ScopedEventHandler<PersistentObjectEventArgs> committed;
    private readonly ScopedEventHandler<FieldChangeEventArgs> fieldChange;
    private readonly ScopedEventHandler<EventArgs> loanRefreshedFromServer;
    private readonly ScopedEventHandler<EventArgs> beforeLoanRefreshedFromServer;
    private readonly ScopedEventHandler<LogEntryEventArgs> logEntryAdded;
    private readonly ScopedEventHandler<LogEntryEventArgs> logEntryRemoved;
    private readonly ScopedEventHandler<ExtensionInvocationEventArgs> beforeExtensionInvoked;
    private readonly ScopedEventHandler<ExtensionInvocationEventArgs> afterExtensionInvoked;
    private readonly ScopedEventHandler<LogEntryEventArgs> logEntryChange;
    private readonly ScopedEventHandler<CancelableMilestoneEventArgs> beforeMilestoneCompleted;
    private readonly ScopedEventHandler<MilestoneEventArgs> milestoneCompleted;
    private string customDataPrefix = "custom-";
    private LoanDataMgr dataMgr;
    private bool calculationsEnabled = true;
    private LoanLock currentLock;
    private bool loanAccessExceptionsEnabled;
    private LoanFields fields;
    private LoanBorrowerPairs pairs;
    private LoanLiabilities liabilities;
    private LoanMortgages mortgages;
    private LoanDeposits deposits;
    private LoanResidences borResidences;
    private LoanResidences cobResidences;
    private LoanEmployers borEmployers;
    private LoanEmployers cobEmployers;
    private LoanVestingParties vestingParties;
    private NonBorrowingOwnerContacts nonBorrowingOwnerContacts;
    private ChangeOfCircumstanceEntries changeOfCircumstanceEntries;
    private LoanLog log;
    private LoanAttachments attachments;
    private LoanAssociates associates;
    private LoanAuditTrail auditTrail;
    private LoanServicing servicing;
    private LoanContacts contacts;
    private Loan linkedLoan;
    private LoanURLAAdditionalLoans urlaAdditionalLoans;
    private LoanURLAGiftsGrants urlaGiftsGrants;
    private LoanURLAOtherAssets urlaOtherAssets;
    private LoanURLAOtherIncome urlaOtherIncome;
    private LoanURLAOtherLiabilities urlaOtherLiabilities;

    public event CancelableEventHandler BeforeCommit
    {
      add
      {
        if (value == null)
          return;
        this.beforeCommit.Add(new ScopedEventHandler<CancelableEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.beforeCommit.Remove(new ScopedEventHandler<CancelableEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event PersistentObjectEventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event FieldChangeEventHandler FieldChange
    {
      add
      {
        if (value == null)
          return;
        this.fieldChange.Add(new ScopedEventHandler<FieldChangeEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.fieldChange.Remove(new ScopedEventHandler<FieldChangeEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event EventHandler OnLoanRefreshedFromServer
    {
      add
      {
        if (value == null)
          return;
        this.loanRefreshedFromServer.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.loanRefreshedFromServer.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event EventHandler OnBeforeLoanRefreshedFromServer
    {
      add
      {
        this.beforeLoanRefreshedFromServer.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        this.beforeLoanRefreshedFromServer.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event LogEntryEventHandler LogEntryAdded
    {
      add
      {
        if (value == null)
          return;
        this.logEntryAdded.Add(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.logEntryAdded.Remove(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event LogEntryEventHandler LogEntryRemoved
    {
      add
      {
        if (value == null)
          return;
        this.logEntryRemoved.Add(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.logEntryRemoved.Remove(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event ExtensionInvocationEventHandler BeforeExtensionInvoked
    {
      add
      {
        if (value == null)
          return;
        this.beforeExtensionInvoked.Add(new ScopedEventHandler<ExtensionInvocationEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.beforeExtensionInvoked.Remove(new ScopedEventHandler<ExtensionInvocationEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event ExtensionInvocationEventHandler AfterExtensionInvoked
    {
      add
      {
        if (value == null)
          return;
        this.afterExtensionInvoked.Add(new ScopedEventHandler<ExtensionInvocationEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.afterExtensionInvoked.Remove(new ScopedEventHandler<ExtensionInvocationEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event LogEntryEventHandler LogEntryChange
    {
      add
      {
        if (value == null)
          return;
        this.logEntryChange.Add(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.logEntryChange.Remove(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event CancelableMilestoneEventHandler BeforeMilestoneCompleted
    {
      add
      {
        if (value == null)
          return;
        this.beforeMilestoneCompleted.Add(new ScopedEventHandler<CancelableMilestoneEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.beforeMilestoneCompleted.Remove(new ScopedEventHandler<CancelableMilestoneEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event MilestoneEventHandler MilestoneCompleted
    {
      add
      {
        if (value == null)
          return;
        this.milestoneCompleted.Add(new ScopedEventHandler<MilestoneEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.milestoneCompleted.Remove(new ScopedEventHandler<MilestoneEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    internal Loan(Session session, LoanDataMgr dataMgr)
      : base(session)
    {
      // ISSUE: method pointer
      // ISSUE: method pointer
      this.beforeCommit = new ScopedEventHandler<CancelableEventArgs>(nameof (Loan), "BeforeCommit", new ScopedEventHandler<CancelableEventArgs>.BeforeDelegateExecution((object) this, __methodptr(\u003C\u002Ector\u003Eb__77_0)), new ScopedEventHandler<CancelableEventArgs>.AfterDelegateExecution((object) this, __methodptr(\u003C\u002Ector\u003Eb__77_1)));
      // ISSUE: method pointer
      // ISSUE: method pointer
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (Loan), "Committed", new ScopedEventHandler<PersistentObjectEventArgs>.BeforeDelegateExecution((object) this, __methodptr(\u003C\u002Ector\u003Eb__77_2)), new ScopedEventHandler<PersistentObjectEventArgs>.AfterDelegateExecution((object) this, __methodptr(\u003C\u002Ector\u003Eb__77_3)));
      this.fieldChange = new ScopedEventHandler<FieldChangeEventArgs>(nameof (Loan), "FieldChange");
      this.loanRefreshedFromServer = new ScopedEventHandler<EventArgs>(nameof (Loan), nameof (loanRefreshedFromServer));
      this.beforeLoanRefreshedFromServer = new ScopedEventHandler<EventArgs>(nameof (Loan), nameof (beforeLoanRefreshedFromServer));
      this.logEntryAdded = new ScopedEventHandler<LogEntryEventArgs>(nameof (Loan), "LogEntryAdded");
      this.logEntryRemoved = new ScopedEventHandler<LogEntryEventArgs>(nameof (Loan), "LogEntryRemoved");
      this.beforeExtensionInvoked = new ScopedEventHandler<ExtensionInvocationEventArgs>(nameof (Loan), "BeforeExtensionInvoked");
      this.afterExtensionInvoked = new ScopedEventHandler<ExtensionInvocationEventArgs>(nameof (Loan), "AfterExtensionInvoked");
      this.logEntryChange = new ScopedEventHandler<LogEntryEventArgs>(nameof (Loan), "LogEntryChange");
      this.beforeMilestoneCompleted = new ScopedEventHandler<CancelableMilestoneEventArgs>(nameof (Loan), "BeforeMilestoneCompleted");
      this.milestoneCompleted = new ScopedEventHandler<MilestoneEventArgs>(nameof (Loan), "MilestoneCompleted");
      this.dataMgr = dataMgr;
      this.dataMgr.AfterSavingLoanFiles += new SavingLoanFilesEventHandler(this.onLoanSaved);
      this.dataMgr.BeforeSavingLoanFiles += new CancelableEventHandler(this.onBeforeLoanSaved);
      this.dataMgr.BeforeLoanRefreshedFromServer += new EventHandler(this.onBeforeLoanRefreshedFromServer);
      this.dataMgr.OnLoanRefreshedFromServer += new EventHandler(this.onLoanRefreshedFromServer);
      this.initializeLoanDataListeners();
      this.initializeSubobjects();
    }

    ~Loan()
    {
      try
      {
        if (!this.Session.IsConnected)
          return;
        this.Close();
      }
      catch (Exception ex)
      {
        try
        {
          new EventLog()
          {
            Log = "Application",
            Source = "Encompass SDK"
          }.WriteEntry("Encompass SDK Loan.Finalize() failed:" + Environment.NewLine + Environment.NewLine + ex.StackTrace, EventLogEntryType.Warning);
        }
        catch
        {
        }
      }
      finally
      {
        // ISSUE: explicit finalizer call
        base.Finalize();
      }
    }

    public void ApplyInvestorToLoan(InvestorTemplate investorTemplate)
    {
      if ((EnumItem) investorTemplate == (EnumItem) null)
        throw new NullReferenceException("The InvestorTemplate cannot be null.");
      this.dataMgr.ApplyInvestorToLoan(investorTemplate.Unwrap().CompanyInformation, (ContactInformation) null, true);
    }

    public void ApplyManualMilestoneTemplate(
      MilestoneTemplate milestoneTemplate,
      bool forceApplyMilestoneTemplate = false)
    {
      if (!forceApplyMilestoneTemplate)
      {
        MilestoneTemplatesSetting policySetting = (MilestoneTemplatesSetting) this.dataMgr.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.MilestoneTemplateSettings"];
        if (policySetting != 3 && policySetting != null)
          throw new InvalidOperationException("Cannot manually apply a MilestoneTemplate when Manual mode is disabled.");
      }
      List<MilestoneTemplate> milestoneTemplateList = new List<MilestoneTemplate>(this.dataMgr.SessionObjects.BpmManager.GetMilestoneTemplates(false));
      MilestoneTemplate template = (MilestoneTemplate) null;
      foreach (MilestoneTemplate milestoneTemplate1 in milestoneTemplateList)
      {
        if (milestoneTemplate1.Name == milestoneTemplate.Name)
        {
          template = milestoneTemplate1;
          break;
        }
      }
      if (!template.Active)
        throw new InvalidOperationException("Cannot apply an inactive MilestoneTemplate.");
      if (template == null)
        return;
      bool? nullable = new MilestoneTemplatesManager().ApplyMilestoneTemplate(this.dataMgr.SessionObjects, this.dataMgr.LoanData, (ILoanMilestoneTemplateOrchestrator) new MilestoneTemplateApply(this.dataMgr, template, true), (MilestoneTemplate) null, string.Empty);
      if (nullable.HasValue && !nullable.Value && !forceApplyMilestoneTemplate)
        throw new InvalidOperationException("MilestoneTemplate did not match loan conditions. Allow non-matching templates is disabled.");
    }

    public void ApplyBestMatchingMilestoneTemplate(bool forceApplyMilestoneTemplate = false)
    {
      if (!forceApplyMilestoneTemplate)
      {
        MilestoneTemplatesSetting policySetting = (MilestoneTemplatesSetting) this.dataMgr.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.MilestoneTemplateSettings"];
        if (policySetting != 3 && policySetting != null)
          throw new InvalidOperationException("Cannot manually apply a MilestoneTemplate when Manual mode is disabled.");
      }
      new MilestoneTemplatesManager().ApplyMilestoneTemplate(this.dataMgr.SessionObjects, this.dataMgr.LoanData, (ILoanMilestoneTemplateOrchestrator) new MilestoneTemplateApply(this.dataMgr, (MilestoneTemplate) null, true), (MilestoneTemplate) null, string.Empty);
    }

    public string LoanFolder
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.LoanFolder;
      }
      set
      {
        this.ensureNew();
        this.dataMgr.LoanFolder = value ?? "";
      }
    }

    public string LoanName
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.LoanName;
      }
      set => this.dataMgr.LoanName = this.Guid;
    }

    public string Guid
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.LoanData.GUID;
      }
    }

    public string LoanNumber
    {
      get
      {
        lock (this)
        {
          this.ensureOpen();
          return this.dataMgr.LoanData.LoanNumber;
        }
      }
      set
      {
        lock (this)
        {
          this.ensureOpen();
          this.dataMgr.LoanData.LoanNumber = value ?? "";
        }
      }
    }

    public int LoanVersionNumber
    {
      get
      {
        lock (this)
        {
          this.ensureOpen();
          return this.dataMgr.LoanData.LoanVersionNumber;
        }
      }
    }

    public string EncompassVersion
    {
      get
      {
        lock (this)
        {
          this.ensureOpen();
          return this.dataMgr.LoanData.EncompassVersion;
        }
      }
      set
      {
        lock (this)
        {
          this.ensureOpen();
          this.dataMgr.LoanData.EncompassVersion = value ?? "";
        }
      }
    }

    public string MersNumber
    {
      get
      {
        lock (this)
        {
          this.ensureOpen();
          return this.dataMgr.LoanData.MersNumber;
        }
      }
      set
      {
        lock (this)
        {
          this.ensureOpen();
          this.dataMgr.LoanData.MersNumber = value ?? "";
        }
      }
    }

    public bool CalculationsEnabled
    {
      get => this.calculationsEnabled;
      set => this.calculationsEnabled = value;
    }

    public bool BusinessRulesEnabled
    {
      get
      {
        lock (this)
        {
          this.ensureOpen();
          return this.dataMgr.ValidationsEnabled;
        }
      }
      set
      {
        lock (this)
        {
          this.ensureOpen();
          this.dataMgr.ValidationsEnabled = value;
        }
      }
    }

    public bool LoanAccessExceptionsEnabled
    {
      get => this.loanAccessExceptionsEnabled;
      set => this.loanAccessExceptionsEnabled = value;
    }

    public DateTime LastModified
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.LastModified;
      }
    }

    public string LoanOfficerID => string.Concat(this.fields["LOID"].Value);

    public string LoanProcessorID => string.Concat(this.fields["LPID"].Value);

    public string LoanCloserID => string.Concat(this.fields["CLID"].Value);

    public LoanFields Fields
    {
      get
      {
        this.ensureOpen();
        return this.fields;
      }
    }

    public LoanBorrowerPairs BorrowerPairs
    {
      get
      {
        this.ensureOpen();
        return this.pairs;
      }
    }

    public LoanLiabilities Liabilities
    {
      get
      {
        this.ensureOpen();
        return this.liabilities;
      }
    }

    public LoanMortgages Mortgages
    {
      get
      {
        this.ensureOpen();
        return this.mortgages;
      }
    }

    public LoanDeposits Deposits
    {
      get
      {
        this.ensureOpen();
        return this.deposits;
      }
    }

    public LoanResidences BorrowerResidences
    {
      get
      {
        this.ensureOpen();
        return this.borResidences;
      }
    }

    public LoanResidences CoBorrowerResidences
    {
      get
      {
        this.ensureOpen();
        return this.cobResidences;
      }
    }

    public LoanEmployers BorrowerEmployers
    {
      get
      {
        this.ensureOpen();
        return this.borEmployers;
      }
    }

    public LoanEmployers CoBorrowerEmployers
    {
      get
      {
        this.ensureOpen();
        return this.cobEmployers;
      }
    }

    public LoanVestingParties AdditionalVestingParties
    {
      get
      {
        this.ensureOpen();
        return this.vestingParties;
      }
    }

    public NonBorrowingOwnerContacts NBOContacts
    {
      get
      {
        this.ensureOpen();
        return this.nonBorrowingOwnerContacts;
      }
    }

    public ChangeOfCircumstanceEntries CoCEntries
    {
      get
      {
        this.ensureOpen();
        return this.changeOfCircumstanceEntries;
      }
    }

    public LoanLog Log
    {
      get
      {
        this.ensureOpen();
        return this.log;
      }
    }

    public MilestoneTemplate MilestoneTemplate
    {
      get
      {
        this.ensureOpen();
        return new MilestoneTemplate(this);
      }
    }

    public LoanAttachments Attachments
    {
      get
      {
        this.ensureOpen();
        this.ensureAttachmentsCompletedInitialization();
        return this.attachments;
      }
    }

    public LoanAssociates Associates
    {
      get
      {
        this.ensureOpen();
        return this.associates;
      }
    }

    public LoanContacts Contacts
    {
      get
      {
        this.ensureOpen();
        return this.contacts;
      }
    }

    public LoanAuditTrail AuditTrail
    {
      get
      {
        this.ensureOpen();
        return this.auditTrail;
      }
    }

    public LoanServicing Servicing
    {
      get
      {
        this.ensureOpen();
        return this.servicing;
      }
    }

    public LoanURLAAdditionalLoans URLAAdditionalLoans
    {
      get
      {
        this.ensureOpen();
        return this.urlaAdditionalLoans;
      }
    }

    public LoanURLAGiftsGrants URLAGiftsGrants
    {
      get
      {
        this.ensureOpen();
        return this.urlaGiftsGrants;
      }
    }

    public LoanURLAOtherAssets URLAOtherAssets
    {
      get
      {
        this.ensureOpen();
        return this.urlaOtherAssets;
      }
    }

    public LoanURLAOtherIncome URLAOtherIncome
    {
      get
      {
        this.ensureOpen();
        return this.urlaOtherIncome;
      }
    }

    public LoanURLAOtherLiabilities URLAOtherLiabilities
    {
      get
      {
        this.ensureOpen();
        return this.urlaOtherLiabilities;
      }
    }

    public Loan LinkedLoan
    {
      get
      {
        this.ensureOpen();
        if (this.linkedLoan == null)
        {
          if (this.dataMgr.LinkedLoan == null)
            return (Loan) null;
          this.linkedLoan = new Loan(this.Session, this.dataMgr.LinkedLoan);
          this.linkedLoan.linkedLoan = this;
        }
        return this.linkedLoan;
      }
    }

    public bool MSLock
    {
      get
      {
        this.ensureOpen();
        return this.LoanData.GetLogList().MSLock;
      }
      set
      {
        this.ensureOpen();
        this.LoanData.GetLogList().MSLock = value;
      }
    }

    public bool MSDateLock
    {
      get
      {
        this.ensureOpen();
        return this.LoanData.GetLogList().MSDateLock;
      }
      set
      {
        this.ensureOpen();
        this.LoanData.GetLogList().MSDateLock = value;
      }
    }

    public void LinkTo(Loan loan)
    {
      this.EnsureExclusive();
      loan.EnsureExclusive();
      this.dataMgr.LinkTo(loan.dataMgr);
      this.linkedLoan = loan;
      loan.linkedLoan = this;
    }

    public void Unlink()
    {
      this.linkedLoan.EnsureExclusive();
      this.EnsureExclusive();
      if (this.linkedLoan != null)
        this.linkedLoan.linkedLoan = (Loan) null;
      this.dataMgr.Unlink();
      this.linkedLoan = (Loan) null;
    }

    public void SendToLoanOfficer(User loanOfficer)
    {
      this.Associates.AssignUser(FixedRole.LoanOfficer, loanOfficer);
    }

    public void SendToProcessing(User loanProcessor)
    {
      this.Associates.AssignUser(FixedRole.LoanProcessor, loanProcessor);
    }

    public void Lock() => this.Lock(true);

    public void Lock(bool exclusive)
    {
      this.ensureExists();
      if (!exclusive)
      {
        LoanLock currentSessionLock = this.getCurrentSessionLock();
        if (currentSessionLock != null && currentSessionLock.Exclusive)
          throw new Exception("Your lock cannot be downgraded to concurrent once it is made exclusive. You must unlock and re-lock the loan.");
        if (!this.Session.SessionObjects.AllowSdkCE)
          throw new Exception("Concurrent loan locking is not permitted by system policy");
      }
      LockInfo.ExclusiveLock exclusiveLock = exclusive ? (LockInfo.ExclusiveLock) 1 : (LockInfo.ExclusiveLock) 0;
      try
      {
        this.dataMgr.Lock((LoanInfo.LockReason) 1, exclusiveLock);
        this.currentLock = (LoanLock) null;
      }
      catch (LockException ex)
      {
        throw new LockException(ex);
      }
      try
      {
        if (exclusive)
          return;
        UserShortInfoList workingOnTheLoan = this.dataMgr.GetUsersWorkingOnTheLoan(this.Session.SessionObjects.SessionID, true);
        if (workingOnTheLoan == null || workingOnTheLoan.Count <= 0)
          return;
        this.Session.SessionObjects.ServerManager.SendMessage((Message) new CEMessage(this.Session.GetUserInfo(), (CEMessageType) 0), workingOnTheLoan.SessionIDs, true);
      }
      catch
      {
      }
    }

    public void Unlock()
    {
      this.ensureExists();
      bool lockedExclusive = this.LockedExclusive;
      this.dataMgr.Unlock();
      try
      {
        if (lockedExclusive || !this.Session.SessionObjects.AllowSdkCE)
          return;
        UserShortInfoList workingOnTheLoan = this.dataMgr.GetUsersWorkingOnTheLoan(this.Session.SessionObjects.SessionID, true);
        if (workingOnTheLoan == null || workingOnTheLoan.Count <= 0)
          return;
        this.Session.SessionObjects.ServerManager.SendMessage((Message) new CEMessage(this.Session.GetUserInfo(), (CEMessageType) 1), workingOnTheLoan.SessionIDs, true);
      }
      catch
      {
      }
    }

    public LoanLock GetCurrentLock()
    {
      LoanLockList currentLocks = this.GetCurrentLocks();
      foreach (LoanLock currentLock in (CollectionBase) currentLocks)
      {
        if (currentLock.SessionID == this.Session.SessionObjects.SessionID)
          return currentLock;
      }
      return currentLocks.Count > 0 ? currentLocks[0] : (LoanLock) null;
    }

    public LoanLockList GetCurrentLocks()
    {
      LoanLockList currentLocks = new LoanLockList();
      foreach (LockInfo currentLock in this.dataMgr.GetCurrentLocks())
      {
        if (currentLock.LockedFor != null)
          currentLocks.Add(new LoanLock(currentLock));
      }
      return currentLocks;
    }

    public void ForceLock()
    {
      try
      {
        this.Lock();
      }
      catch (LockException ex)
      {
        this.ForceUnlock();
        this.Lock();
      }
    }

    public void ForceUnlock()
    {
      this.ensureExists();
      this.dataMgr.Unlock(true);
    }

    public bool Locked
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.Writable;
      }
    }

    public bool LockedExclusive
    {
      get
      {
        this.ensureOpen();
        if (this.IsNew)
          return true;
        if (!this.Locked)
          return false;
        if (this.currentLock == null)
          this.currentLock = this.getCurrentSessionLock();
        return this.currentLock != null && this.currentLock.Exclusive;
      }
    }

    public void Recalculate()
    {
      this.ensureOpen();
      this.dataMgr.Calculator.CalculateAll();
    }

    public void ExecuteCalculation(string calcName)
    {
      this.ensureOpen();
      this.dataMgr.Calculator.FormCalculation(calcName, (string) null, (string) null);
    }

    public void ExecuteCalculation(CalculationTriggerOptions triggerOption)
    {
      this.ensureOpen();
      string calcName;
      switch (triggerOption)
      {
        case CalculationTriggerOptions.ApplyDDM:
          calcName = "APPLYDDM";
          break;
        case CalculationTriggerOptions.Calculation_2015RESPA:
          calcName = "REGZGFEHUD";
          break;
        case CalculationTriggerOptions.Calculation_AggregateEscrow:
          calcName = "HUD1ES";
          break;
        case CalculationTriggerOptions.Calculation_CityCountyStateTax:
          calcName = "UPDATECITYSTATEUSERFEES";
          break;
        case CalculationTriggerOptions.Calculation_FHA203K:
          calcName = "FHA203K";
          break;
        case CalculationTriggerOptions.Calculation_MIP:
          calcName = "MIP";
          break;
        case CalculationTriggerOptions.Calculation_MLDS:
          calcName = "MLDS";
          break;
        case CalculationTriggerOptions.Calculation_PTCPOC:
          calcName = "VERIFYINGPOCPTCFIELDS";
          break;
        case CalculationTriggerOptions.Calculation_USDAMIP:
          calcName = "USDAMIP";
          break;
        case CalculationTriggerOptions.Calculation_TPO:
          calcName = "TPO";
          break;
        case CalculationTriggerOptions.Calculation_COPYLIABILIESTOCDPG3:
          calcName = "COPYLIABILIESTOCDPG3";
          break;
        case CalculationTriggerOptions.RECALCULATEHMDA:
          calcName = "RECALCULATEHMDA";
          break;
        default:
          return;
      }
      if (calcName == "")
        return;
      this.ExecuteCalculation(calcName);
    }

    public void Export(string filePath, string exportKey, LoanExportFormat format)
    {
      string str = this.ExportAsText(exportKey, format);
      StreamWriter streamWriter = new StreamWriter(filePath ?? "", false, Encoding.Default);
      streamWriter.Write(str);
      streamWriter.Close();
    }

    public string ExportAsText(string exportKey, LoanExportFormat format)
    {
      return this.ExportAsText(exportKey, format, false);
    }

    public string ExportAsText(string exportKey, LoanExportFormat format, bool currentBorPairOnly)
    {
      this.ensureOpen();
      this.validateExportKey(exportKey, format);
      lock (typeof (Loan))
        Loan.ensureExportAssemblyExists(format);
      return new ExportData(this.dataMgr, this.dataMgr.LoanData).Export(format.ToString(), currentBorPairOnly);
    }

    public void Import(string filePath, LoanImportFormat format)
    {
      this.ImportWithTemplate(filePath, format, (LoanTemplate) null);
    }

    public void ImportWithTemplate(string filePath, LoanImportFormat format, LoanTemplate template)
    {
      this.ImportWithLoanOfficer(filePath, format, template, (User) null);
    }

    public void ImportWithLoanOfficer(
      string filePath,
      LoanImportFormat format,
      LoanTemplate template,
      User user)
    {
      lock (this)
      {
        this.ensureExists();
        string loanOfficerId = user != null ? user.ID : string.Empty;
        using (LoanDataMgr loanDataMgr = Loan.ImportFile(filePath ?? "", format, template, this.Session, loanOfficerId))
          this.Attach(loanDataMgr.LoanData);
      }
    }

    public void ImportFromBytes(ref byte[] importData, LoanImportFormat format)
    {
      lock (this)
      {
        this.ensureExists();
        using (LoanDataMgr loanDataMgr = Loan.Import(importData, format, (LoanTemplate) null, this.Session, string.Empty))
          this.Attach(loanDataMgr.LoanData);
      }
    }

    public bool IsNew
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.IsNew();
      }
    }

    public bool Modified
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.LoanData.Dirty;
      }
    }

    public void Commit()
    {
      this.ensureLocked();
      using (ClientMetricsProviderFactory.GetIncrementalTimer("LoanSaveIncTimer", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      }))
      {
        bool flag;
        this.dataMgr.SaveLoan(false, false, true, (ILoanMilestoneTemplateOrchestrator) null, false, ref flag, false, false);
      }
    }

    public void Refresh()
    {
      this.ensureExists();
      this.dataMgr.Refresh(false);
      this.refreshInternal();
    }

    public void Close()
    {
      if (this.dataMgr != null)
      {
        this.dataMgr.Unlock();
        this.dataMgr.Close();
      }
      this.dataMgr = (LoanDataMgr) null;
      if (this.linkedLoan != null)
      {
        this.linkedLoan.linkedLoan = (Loan) null;
        this.linkedLoan = (Loan) null;
      }
      try
      {
        GC.SuppressFinalize((object) this);
      }
      catch
      {
      }
    }

    public void Move(EllieMae.Encompass.BusinessObjects.Loans.LoanFolder newFolder, string newLoanName)
    {
      this.ensureExists();
      bool flag = false;
      if (this.Locked)
      {
        this.EnsureExclusive();
      }
      else
      {
        this.Lock(true);
        flag = true;
      }
      this.dataMgr.Move(newFolder.Name, newLoanName, (DuplicateLoanAction) 3);
      if (!flag)
        return;
      this.Unlock();
    }

    public void MoveToFolder(EllieMae.Encompass.BusinessObjects.Loans.LoanFolder newFolder)
    {
      this.ensureExists();
      bool flag = false;
      if (this.Locked)
      {
        this.EnsureExclusive();
      }
      else
      {
        this.Lock(true);
        flag = true;
      }
      this.dataMgr.Move(newFolder.Name, "", (DuplicateLoanAction) 1);
      if (!flag)
        return;
      this.Unlock();
    }

    void IDisposable.Dispose()
    {
      try
      {
        this.Close();
      }
      catch
      {
      }
    }

    public void Delete()
    {
      this.ensureOpen();
      this.dataMgr.Delete();
    }

    public DataObject GetCustomDataObject(string name)
    {
      if ((name ?? "") == "")
        throw new ArgumentException(nameof (name), "Object name cannot be blank or null");
      BinaryObject supportingData = this.dataMgr.GetSupportingData(this.customDataPrefix + name);
      return supportingData == null ? (DataObject) null : new DataObject(supportingData);
    }

    public void SaveCustomDataObject(string name, DataObject data)
    {
      this.ensureLocked();
      if ((name ?? "") == "")
        throw new ArgumentException(nameof (name), "Object name cannot be blank or null");
      this.dataMgr.SaveSupportingData(this.customDataPrefix + name, data == null ? (BinaryObject) null : data.Unwrap());
    }

    public void AppendToCustomDataObject(string name, DataObject data)
    {
      this.ensureLocked();
      if ((name ?? "") == "")
        throw new ArgumentException(nameof (name), "Object name cannot be blank or null");
      if (data == null)
        throw new ArgumentNullException(nameof (data), "Data object cannot be null");
      this.dataMgr.AppendSupportingData(this.customDataPrefix + name, data.Unwrap());
    }

    public void DeleteCustomDataObject(string name)
    {
      this.SaveCustomDataObject(this.customDataPrefix + name, (DataObject) null);
    }

    public DataObject GetEPassTransactionDataObject(string name)
    {
      BinaryObject data = !((name ?? "") == "") ? this.dataMgr.GetSupportingData(name) : throw new ArgumentException(nameof (name), "Object name cannot be blank or null");
      return data == null ? (DataObject) null : new DataObject(data);
    }

    public void SaveEPassTransactionDataObject(string name, DataObject data)
    {
      this.ensureLocked();
      if ((name ?? "") == "")
        throw new ArgumentException(nameof (name), "Object name cannot be blank or null");
      if (string.Compare(name, "loan.em", true) == 0)
        throw new ArgumentException(nameof (name), "Invalid object name specified");
      if (string.Compare(name, "attachments.xml", true) == 0)
        throw new ArgumentException(nameof (name), "Invalid object name specified");
      this.dataMgr.SaveSupportingData(name, data == null ? (BinaryObject) null : data.Unwrap());
    }

    public LoanAccessRights GetAccessRights()
    {
      this.ensureOpen();
      LoanInfo.Right effectiveRight = this.dataMgr.GetEffectiveRight(this.Session.UserID);
      switch ((int) effectiveRight)
      {
        case 0:
          return LoanAccessRights.None;
        case 1:
          return LoanAccessRights.ReadWrite;
        case 2:
          throw new Exception("Invalid value returned for loan rights");
        case 3:
          return LoanAccessRights.Full;
        default:
          if (effectiveRight == 8)
            return LoanAccessRights.ReadOnly;
          goto case 2;
      }
    }

    public LoanAccessRights GetAssignedAccessRights(User user)
    {
      this.ensureFullRights();
      this.ensureExists();
      switch ((int) this.dataMgr.GetRight(user.ID))
      {
        case 0:
          return LoanAccessRights.None;
        case 1:
          return LoanAccessRights.ReadWrite;
        case 3:
          return LoanAccessRights.Full;
        default:
          throw new Exception("Invalid value returned for loan rights");
      }
    }

    public LoanAccessRights GetEffectiveAccessRights(User user)
    {
      this.ensureFullRights();
      this.ensureExists();
      LoanInfo.Right effectiveRight = this.dataMgr.GetEffectiveRight(user.ID);
      switch ((int) effectiveRight)
      {
        case 0:
          return LoanAccessRights.None;
        case 1:
          return LoanAccessRights.ReadWrite;
        case 2:
          throw new Exception("Invalid value returned for loan rights");
        case 3:
          return LoanAccessRights.Full;
        default:
          if (effectiveRight == 8)
            return LoanAccessRights.ReadOnly;
          goto case 2;
      }
    }

    public UserList GetUsersWithAssignedRights()
    {
      this.ensureFullRights();
      this.ensureExists();
      UserList withAssignedRights = new UserList();
      foreach (DictionaryEntry right in this.dataMgr.GetRights())
      {
        if ((LoanInfo.Right) right.Value != null)
        {
          User user = this.Session.Users.GetUser(right.Key.ToString());
          if (user != null)
            withAssignedRights.Add(user);
        }
      }
      return withAssignedRights;
    }

    public void AssignRights(User user, LoanAccessRights rights)
    {
      this.ensureFullRights();
      this.ensureExists();
      LoanInfo.Right right;
      switch (rights)
      {
        case LoanAccessRights.None:
          right = (LoanInfo.Right) 0;
          break;
        case LoanAccessRights.ReadWrite:
          right = (LoanInfo.Right) 1;
          break;
        case LoanAccessRights.Full:
          right = (LoanInfo.Right) 3;
          break;
        default:
          throw new ArgumentException("Invalid access right specified");
      }
      this.dataMgr.SetRight(user.ID, right);
    }

    public void ApplyTemplate(Template tmpl, bool appendData)
    {
      if (tmpl == null)
        throw new ArgumentNullException("Specified template must be non-null.");
      switch (tmpl.TemplateType)
      {
        case TemplateType.LoanProgram:
          this.dataMgr.ApplyLoanProgram(tmpl.FileSystemEntry, (LoanProgram) tmpl.Unwrap(), appendData);
          break;
        case TemplateType.ClosingCost:
          this.dataMgr.ApplyClosingCost(tmpl.FileSystemEntry, (ClosingCost) tmpl.Unwrap(), appendData);
          break;
        case TemplateType.DataTemplate:
          this.dataMgr.ApplyDataTemplate(tmpl.FileSystemEntry, (DataTemplate) tmpl.Unwrap(), appendData);
          break;
        case TemplateType.InputFormSet:
          this.dataMgr.ApplyInputFormSet(tmpl.FileSystemEntry, (FormTemplate) tmpl.Unwrap());
          break;
        case TemplateType.DocumentSet:
          this.dataMgr.ApplyDocumentSet(tmpl.FileSystemEntry, (DocumentSetTemplate) tmpl.Unwrap());
          break;
        case TemplateType.LoanTemplate:
          this.dataMgr.ApplyLoanTemplate(tmpl.FileSystemEntry, appendData, false, false);
          break;
        case TemplateType.TaskSet:
          this.dataMgr.ApplyTaskSet(tmpl.FileSystemEntry, (TaskSetTemplate) tmpl.Unwrap());
          break;
        default:
          throw new Exception("The specified template type is not supported");
      }
    }

    public void SetClosingDocumentFieldOverride(string fieldId, bool ovrrde)
    {
      if (!fieldId.StartsWith("Closing.", StringComparison.CurrentCultureIgnoreCase))
        throw new ArgumentException("The specified field is invalid in this context. Only fields that begin with the 'Closing' prefix are valid.");
      if (!ovrrde)
        this.Fields[fieldId].Value = (object) "";
      this.dataMgr.LoanData.SetDocFieldUserOverride(fieldId, ovrrde);
    }

    public bool IsClosingDocumentFieldOverridden(string fieldId)
    {
      if (!fieldId.StartsWith("Closing.", StringComparison.CurrentCultureIgnoreCase))
        throw new ArgumentException("The specified field is invalid in this context. Only fields that begin with the 'Closing' prefix are valid.");
      return this.dataMgr.LoanData.IsDocFieldOverridden(fieldId);
    }

    public FundingFeeList GetFundingFees(bool hideZero)
    {
      List<FundingFee> fundingFees = this.dataMgr.LoanData.Calculator.GetFundingFees(hideZero);
      if (fundingFees == null)
        return new FundingFeeList();
      List<FundingFee> source = new List<FundingFee>();
      for (int index = 0; index < fundingFees.Count; ++index)
        source.Add(new FundingFee(fundingFees[index]));
      return new FundingFeeList((IList) source);
    }

    public override int GetHashCode() => this.Guid.GetHashCode();

    public override bool Equals(object obj)
    {
      if (object.Equals((object) (obj as Loan), (object) null))
        return false;
      Loan loan = (Loan) obj;
      if (loan.Session != this.Session)
        return false;
      return loan.IsNew || this.IsNew ? this == obj : loan.Guid == this.Guid;
    }

    internal LoanData LoanData
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.LoanData;
      }
    }

    internal void Attach(LoanData newData)
    {
      this.ensureExists();
      newData.LoanNumber = this.LoanData.LoanNumber;
      newData.EncompassVersion = this.LoanData.EncompassVersion;
      newData.LoanVersionNumber = this.LoanData.LoanVersionNumber;
      this.disposeLoanDataListeners();
      this.dataMgr.Attach(newData);
      this.initializeLoanDataListeners();
      this.refreshInternal();
    }

    private void ensureOpen()
    {
      if (this.dataMgr == null)
        throw new ObjectDisposedException(nameof (Loan), "Attempt to access a disposed loan");
    }

    private void ensureNew()
    {
      this.ensureOpen();
      if (!this.dataMgr.IsNew())
        throw new InvalidOperationException("Operation only valid for a new loan");
    }

    private void ensureExists()
    {
      this.ensureOpen();
      if (this.dataMgr.IsNew())
        throw new InvalidOperationException("Operation only valid for an existing loan");
    }

    private void ensureLocked()
    {
      this.ensureOpen();
      if (!this.dataMgr.Writable)
        throw new InvalidOperationException("Loan must be locked for this operation");
    }

    internal void EnsureExclusive()
    {
      this.ensureOpen();
      if (!this.LockedExclusive)
        throw new InvalidOperationException("An exclusive lock is required for this operation");
    }

    private void initializeSubobjects()
    {
      this.fields = new LoanFields(this);
      this.pairs = new LoanBorrowerPairs(this);
      this.liabilities = new LoanLiabilities(this);
      this.mortgages = new LoanMortgages(this);
      this.deposits = new LoanDeposits(this);
      this.borResidences = new LoanResidences(this, true);
      this.cobResidences = new LoanResidences(this, false);
      this.borEmployers = new LoanEmployers(this, true);
      this.cobEmployers = new LoanEmployers(this, false);
      this.vestingParties = new LoanVestingParties(this);
      this.nonBorrowingOwnerContacts = new NonBorrowingOwnerContacts(this);
      this.changeOfCircumstanceEntries = new ChangeOfCircumstanceEntries(this);
      this.log = new LoanLog(this);
      this.associates = new LoanAssociates(this);
      this.auditTrail = new LoanAuditTrail(this);
      this.servicing = new LoanServicing(this);
      this.contacts = new LoanContacts(this);
      this.urlaAdditionalLoans = new LoanURLAAdditionalLoans(this);
      this.urlaGiftsGrants = new LoanURLAGiftsGrants(this);
      this.urlaOtherAssets = new LoanURLAOtherAssets(this);
      this.urlaOtherIncome = new LoanURLAOtherIncome(this);
      this.urlaOtherLiabilities = new LoanURLAOtherLiabilities(this);
    }

    private void initializeLoanDataListeners()
    {
      this.dataMgr.LoanData.FieldChanged -= new FieldChangedEventHandler(this.onFieldChanged);
      this.dataMgr.LoanData.LogRecordAdded -= new LogRecordEventHandler(this.onLogRecordAdded);
      this.dataMgr.LoanData.LogRecordRemoved -= new LogRecordEventHandler(this.onLogRecordRemoved);
      this.dataMgr.LoanData.LogRecordChanged -= new LogRecordEventHandler(this.onLogRecordChanged);
      this.dataMgr.LoanData.BeforeMilestoneCompleted -= new CancelableMilestoneEventHandler(this.onBeforeMilestoneCompleted);
      this.dataMgr.LoanData.MilestoneCompleted -= new MilestoneEventHandler(this.onMilestoneCompleted);
      this.dataMgr.LoanData.FieldChanged += new FieldChangedEventHandler(this.onFieldChanged);
      this.dataMgr.LoanData.LogRecordAdded += new LogRecordEventHandler(this.onLogRecordAdded);
      this.dataMgr.LoanData.LogRecordRemoved += new LogRecordEventHandler(this.onLogRecordRemoved);
      this.dataMgr.LoanData.LogRecordChanged += new LogRecordEventHandler(this.onLogRecordChanged);
      this.dataMgr.LoanData.BeforeMilestoneCompleted += new CancelableMilestoneEventHandler(this.onBeforeMilestoneCompleted);
      this.dataMgr.LoanData.MilestoneCompleted += new MilestoneEventHandler(this.onMilestoneCompleted);
    }

    private void disposeLoanDataListeners()
    {
      this.LoanData.FieldChanged -= new FieldChangedEventHandler(this.onFieldChanged);
      this.LoanData.LogRecordAdded -= new LogRecordEventHandler(this.onLogRecordAdded);
      this.LoanData.LogRecordRemoved -= new LogRecordEventHandler(this.onLogRecordRemoved);
      this.LoanData.LogRecordChanged -= new LogRecordEventHandler(this.onLogRecordChanged);
      this.LoanData.BeforeMilestoneCompleted -= new CancelableMilestoneEventHandler(this.onBeforeMilestoneCompleted);
      this.LoanData.MilestoneCompleted -= new MilestoneEventHandler(this.onMilestoneCompleted);
    }

    private void ensureAttachmentsCompletedInitialization()
    {
      if (this.attachments != null)
        return;
      this.attachments = new LoanAttachments(this);
    }

    protected void OnCommitted()
    {
      this.committed((object) this, new PersistentObjectEventArgs(this.Guid));
    }

    protected void OnBeforeCommit(CancelableEventArgs e) => this.beforeCommit((object) this, e);

    public IList<Type> GetOnBeforeCommitExtensions() => this.beforeCommit.GetTargetTypes();

    public IList<Type> GetOnCommittedExtensions() => this.committed.GetTargetTypes();

    private void onLoanRefreshedFromServer(object source, EventArgs e)
    {
      this.initializeSubobjects();
      this.initializeLoanDataListeners();
      this.loanRefreshedFromServer((object) this, new EventArgs());
    }

    private void onBeforeLoanRefreshedFromServer(object source, EventArgs e)
    {
      if (this.beforeLoanRefreshedFromServer != null)
        this.beforeLoanRefreshedFromServer((object) this, new EventArgs());
      this.disposeLoanDataListeners();
    }

    private void onFieldChanged(object source, FieldChangedEventArgs e)
    {
      this.fieldChange((object) this, new FieldChangeEventArgs(this, e));
    }

    private void onLogRecordRemoved(object source, LogRecordEventArgs e)
    {
      LogEntry logEntry = this.log.Wrap(e.LogRecord);
      if (logEntry == null)
        return;
      this.log.PurgeEntry(logEntry);
      this.logEntryRemoved((object) this, new LogEntryEventArgs(this, logEntry));
    }

    private void onLogRecordAdded(object source, LogRecordEventArgs e)
    {
      LogEntry logEntry = this.log.Find(e.LogRecord, true);
      if (logEntry == null)
        return;
      this.logEntryAdded((object) this, new LogEntryEventArgs(this, logEntry));
    }

    private void onLogRecordChanged(object source, LogRecordEventArgs e)
    {
      if (this.logEntryChange.IsNull())
        return;
      LogEntry logEntry = this.log.Find(e.LogRecord, false);
      if (logEntry == null)
        return;
      this.logEntryChange((object) this, new LogEntryEventArgs(this, logEntry));
    }

    private void onMilestoneCompleted(object source, MilestoneEventArgs e)
    {
      if (!(this.log.Wrap((LogRecordBase) e.MilestoneLog) is MilestoneEvent msEvent))
        return;
      this.milestoneCompleted((object) this, new MilestoneEventArgs(this, msEvent));
    }

    private void onBeforeMilestoneCompleted(object source, CancelableMilestoneEventArgs e)
    {
      if (!(this.log.Wrap((LogRecordBase) ((MilestoneEventArgs) e).MilestoneLog) is MilestoneEvent msEvent))
        return;
      CancelableMilestoneEventArgs milestoneEventArgs = new CancelableMilestoneEventArgs(this, msEvent);
      this.beforeMilestoneCompleted((object) this, milestoneEventArgs);
      e.Cancel = milestoneEventArgs.Cancel;
    }

    private void onLoanSaved(object obj, SavingLoanFilesEventArgs e)
    {
      this.currentLock = (LoanLock) null;
      this.OnCommitted();
    }

    private void onBeforeLoanSaved(object sender, CancelableEventArgs e)
    {
      this.currentLock = (LoanLock) null;
      CancelableEventArgs e1 = new CancelableEventArgs(this, e.Cancel);
      this.OnBeforeCommit(e1);
      e.Cancel = e1.Cancel;
    }

    private LoanLock getCurrentSessionLock()
    {
      foreach (LoanLock currentLock in (CollectionBase) this.GetCurrentLocks())
      {
        if (currentLock.SessionID == this.Session.SessionObjects.SessionID)
          return currentLock;
      }
      return (LoanLock) null;
    }

    private void refreshInternal()
    {
      this.currentLock = (LoanLock) null;
      this.pairs.RefreshPairs();
      this.ensureAttachmentsCompletedInitialization();
      this.attachments.Refresh();
    }

    internal static LoanDataMgr ImportFile(
      string filePath,
      LoanImportFormat format,
      LoanTemplate template,
      Session session,
      string loanOfficerId)
    {
      if (!System.IO.File.Exists(filePath))
        throw new ArgumentException("File \"" + filePath + "\" not found");
      byte[] numArray = (byte[]) null;
      using (FileStream fileStream = System.IO.File.OpenRead(filePath))
      {
        numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, numArray.Length);
      }
      return Loan.Import(numArray, format, template, session, loanOfficerId);
    }

    internal static LoanDataMgr Import(
      byte[] data,
      LoanImportFormat format,
      LoanTemplate template,
      Session session,
      string loanOfficerId)
    {
      return Loan.Import(data, format, template, session, loanOfficerId, false, false);
    }

    internal static LoanDataMgr Import(
      byte[] data,
      LoanImportFormat format,
      LoanTemplate template,
      Session session,
      string loanOfficerId,
      bool suppressCalcs,
      bool isTPO)
    {
      session.SessionObjects.Session.LoanImportInProgress = true;
      try
      {
        if (format == LoanImportFormat.FNMA3X || format == LoanImportFormat.FNMA34)
          return Loan.importFannie(data, template, session, loanOfficerId, suppressCalcs, isTPO, format);
        throw new ArgumentException("Invalid import format specified", nameof (format));
      }
      finally
      {
        session.SessionObjects.Session.LoanImportInProgress = false;
      }
    }

    private static LoanDataMgr importFannie(
      byte[] importData,
      LoanTemplate template,
      Session session,
      string loanOfficerId,
      bool suppressCalcs,
      bool isTPO,
      LoanImportFormat format)
    {
      LoanTemplateSelection templateSelection = (LoanTemplateSelection) null;
      if (template != null)
        templateSelection = new LoanTemplateSelection(template.FileSystemEntry);
      IServerManager iserverManager = (IServerManager) session.GetObject("ServerManager");
      EnableDisableSetting serverSetting = (EnableDisableSetting) iserverManager.GetServerSetting("Import.LoanNumbering");
      ApplicationDateImportSetting dateImportSetting = !isTPO ? (ApplicationDateImportSetting) iserverManager.GetServerSetting("Import.EnforceApplicationDate") : (ApplicationDateImportSetting) iserverManager.GetServerSetting("Import.TPODateImport");
      return new FannieImport(templateSelection, format == LoanImportFormat.FNMA34)
      {
        UseEMLoanNumbering = (serverSetting == 1),
        EnforceApplicationDate = (dateImportSetting == 1),
        SuppressCalcs = suppressCalcs,
        TPOImport = isTPO
      }.Convert(importData, session.SessionObjects, loanOfficerId);
    }

    private void ensureFullRights()
    {
      if (this.dataMgr.GetEffectiveRight(this.Session.UserID) != 3)
        throw new InvalidOperationException("Access denied");
    }

    internal IBpmManager WorkflowManager => (IBpmManager) this.Session.GetObject("BpmManager");

    internal LoanDataMgr Unwrap() => this.dataMgr;

    public static Loan Wrap(Session session, LoanDataMgr dataMgr) => new Loan(session, dataMgr);

    public bool ValidateUnderwritingAdvancedConditions(string contactID)
    {
      return !(contactID == "") ? this.dataMgr.ValidateUnderwritingAdvancedConditions(contactID) : throw new Exception("Contact ID cannot be blank.");
    }

    private void validateExportKey(string exportKey, LoanExportFormat format)
    {
      if ((exportKey ?? "") == "")
        throw new LicenseException("A valid Export Key is required to perform this action.");
      try
      {
        string appname = "";
        string[] commandLineArgs = Environment.GetCommandLineArgs();
        if (commandLineArgs.Length != 0)
          appname = Path.GetFileNameWithoutExtension(commandLineArgs[0]);
        string server = this.Session.ServerURI ?? "";
        try
        {
          server = ServerIdentity.Parse(server).Uri.Host;
        }
        catch
        {
        }
        using (LicenseService licenseService = new LicenseService(this.Session?.SessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl))
          licenseService.ValidateExport(exportKey, this.Session.ClientID, this.Guid, format.ToString(), Dns.GetHostName(), appname, server, this.Session.UserID);
      }
      catch (SoapException ex)
      {
        string message = ex.Message;
        message = ex.Message.IndexOf("--> ") > 0 ? ex.Message.Substring(ex.Message.IndexOf("--> ") + 4) : throw new LicenseException(message);
      }
      catch (Exception ex)
      {
        throw new Exception("Error attempting to validate Export Key.", ex);
      }
    }

    private static void ensureExportAssemblyExists(LoanExportFormat format)
    {
      string exportAssemblyPath = ExportData.GetExportAssemblyPath(format.ToString());
      if (exportAssemblyPath == null || System.IO.File.Exists(exportAssemblyPath))
        return;
      string address = "https://www.epassbusinesscenter.com/epassutils/data/" + (Path.GetFileNameWithoutExtension(Path.GetFileName(exportAssemblyPath)) + ".zip");
      try
      {
        byte[] numArray = (byte[]) null;
        using (WebClient webClient = new WebClient())
        {
          webClient.Proxy.Credentials = CredentialCache.DefaultCredentials;
          numArray = webClient.DownloadData(address);
        }
        byte[] buffer = FileCompressor.Instance.UnzipBuffer(numArray);
        Directory.CreateDirectory(Path.GetDirectoryName(exportAssemblyPath));
        using (FileStream fileStream = System.IO.File.Create(exportAssemblyPath))
          fileStream.Write(buffer, 0, buffer.Length);
      }
      catch (Exception ex)
      {
        throw new Exception("The export assembly for format '" + (object) format + "' could not be loaded", ex);
      }
    }

    public string GetUCDForLoanEstimate(bool setTotalFields)
    {
      return this.LoanData.Calculator.GetUCD(true, setTotalFields);
    }

    public string GetUCDForClosingDisclosure(bool setTotalFields)
    {
      return this.LoanData.Calculator.GetUCD(false, setTotalFields);
    }
  }
}
