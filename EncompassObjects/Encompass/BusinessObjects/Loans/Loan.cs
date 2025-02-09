// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Loan
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Export;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
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
  /// <summary>
  /// The Loan class represents a single loan file within Encompass. All loan-related data
  /// for this this file is accessible from this object's properties and methods.
  /// </summary>
  /// <example>
  /// The following code creates a new loan, sets the value of several fields
  /// and then commits the loan to the database.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Create the empty shell for the new loan. At this point,
  ///       // the loan has not been saved to the Encompass server.
  ///       Loan loan = session.Loans.CreateNew();
  /// 
  ///       // Set the loan folder and loan name for the loan
  ///       loan.LoanFolder = "My Pipeline";
  ///       loan.LoanName = "Harrison";
  /// 
  ///       // Set the borrower's name and property address
  ///       loan.Fields["36"].Value = "Howard";        // First name
  ///       loan.Fields["37"].Value = "Harrison";      // Last name
  ///       loan.Fields["11"].Value = "235 Main St.";  // Street Address
  ///       loan.Fields["12"].Value = "Anycity";       // City
  ///       loan.Fields["13"].Value = "Anycounty";     // County
  ///       loan.Fields["14"].Value = "CA";            // State
  ///       loan.Fields["15"].Value = "94432";         // Zip code
  /// 
  ///       // Save the loan to the server
  ///       loan.Commit();
  /// 
  ///       // Write out the GUID of the newly created loan
  ///       Console.WriteLine(loan.Guid);
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
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

    /// <summary>
    /// The BeforeCommit event is raised just before a the loan is saved.
    /// </summary>
    /// <remarks>Use this event to run custom validation and, if necessary cancel the save operation.</remarks>
    public event CancelableEventHandler BeforeCommit
    {
      add
      {
        if (value == null)
          return;
        this.beforeCommit.Add(new ScopedEventHandler<CancelableEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.beforeCommit.Remove(new ScopedEventHandler<CancelableEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// The Committed event is raised whenever changes to the current loan are committed
    /// to the server.
    /// </summary>
    public event PersistentObjectEventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// The FieldChange event is fired any time a change is made to field within the loan.
    /// </summary>
    public event FieldChangeEventHandler FieldChange
    {
      add
      {
        if (value == null)
          return;
        this.fieldChange.Add(new ScopedEventHandler<FieldChangeEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.fieldChange.Remove(new ScopedEventHandler<FieldChangeEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// The OnLoanRefreshedFromServer is fired when loan is reloaded from server.
    /// </summary>
    public event EventHandler OnLoanRefreshedFromServer
    {
      add
      {
        if (value == null)
          return;
        this.loanRefreshedFromServer.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.loanRefreshedFromServer.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// The BeforeLoanRefreshedFromServer is fired when loan is reloaded from server.
    /// </summary>
    public event EventHandler OnBeforeLoanRefreshedFromServer
    {
      add
      {
        this.beforeLoanRefreshedFromServer.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        this.beforeLoanRefreshedFromServer.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// The LogEntryAdded event is fired when a new LogEntry is added to the loan's log.
    /// </summary>
    public event LogEntryEventHandler LogEntryAdded
    {
      add
      {
        if (value == null)
          return;
        this.logEntryAdded.Add(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.logEntryAdded.Remove(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// The LogEntryRemoved event is fired when a LogEntry is removed from the loan's log.
    /// </summary>
    /// <remarks>This event is fired just prior to the removal of the event from the log.
    /// The event cannot be used to prevent the log entry from being removed.</remarks>
    public event LogEntryEventHandler LogEntryRemoved
    {
      add
      {
        if (value == null)
          return;
        this.logEntryRemoved.Add(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.logEntryRemoved.Remove(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// When a user extension aka plugin is about to be invoked.
    /// </summary>
    public event ExtensionInvocationEventHandler BeforeExtensionInvoked
    {
      add
      {
        if (value == null)
          return;
        this.beforeExtensionInvoked.Add(new ScopedEventHandler<ExtensionInvocationEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.beforeExtensionInvoked.Remove(new ScopedEventHandler<ExtensionInvocationEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>When a user extension (plugin) is completed.</summary>
    public event ExtensionInvocationEventHandler AfterExtensionInvoked
    {
      add
      {
        if (value == null)
          return;
        this.afterExtensionInvoked.Add(new ScopedEventHandler<ExtensionInvocationEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.afterExtensionInvoked.Remove(new ScopedEventHandler<ExtensionInvocationEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// The LogEntryChange event is fired when a field on a LogEntry object is modified.
    /// </summary>
    /// <remarks>This event only fires for LogEntry objects that are part of the Loan object.
    /// </remarks>
    public event LogEntryEventHandler LogEntryChange
    {
      add
      {
        if (value == null)
          return;
        this.logEntryChange.Add(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.logEntryChange.Remove(new ScopedEventHandler<LogEntryEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// The BeforeMilestoneCompleted event is raised just before a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" />
    /// is marked as completed.
    /// </summary>
    /// <remarks>Use this event to run custom validation and, if necessary cancel the completion of the
    /// milestone.</remarks>
    public event CancelableMilestoneEventHandler BeforeMilestoneCompleted
    {
      add
      {
        if (value == null)
          return;
        this.beforeMilestoneCompleted.Add(new ScopedEventHandler<CancelableMilestoneEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.beforeMilestoneCompleted.Remove(new ScopedEventHandler<CancelableMilestoneEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// The MilestoneCompleted event is raised when a milestone is marked as complete.
    /// </summary>
    /// <remarks>This event occurs after the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" /> is set to
    /// completed. To run custom validation logic prior to the completion of the milestone
    /// event, use the <see cref="E:EllieMae.Encompass.BusinessObjects.Loans.Loan.BeforeMilestoneCompleted" /> event.</remarks>
    public event MilestoneEventHandler MilestoneCompleted
    {
      add
      {
        if (value == null)
          return;
        this.milestoneCompleted.Add(new ScopedEventHandler<MilestoneEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.milestoneCompleted.Remove(new ScopedEventHandler<MilestoneEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    internal Loan(Session session, LoanDataMgr dataMgr)
      : base(session)
    {
      this.beforeCommit = new ScopedEventHandler<CancelableEventArgs>(nameof (Loan), "BeforeCommit", (ScopedEventHandler<CancelableEventArgs>.BeforeDelegateExecution) ((type, start) => this.beforeExtensionInvoked.Invoke((object) this, new ExtensionInvocationEventArgs()
      {
        InvocationType = ExtensionInvocationType.OnBeforeCommit,
        Target = type
      })), (ScopedEventHandler<CancelableEventArgs>.AfterDelegateExecution) ((type, start, end) => this.afterExtensionInvoked.Invoke((object) this, new ExtensionInvocationEventArgs()
      {
        InvocationType = ExtensionInvocationType.OnBeforeCommit,
        Target = type,
        Elapsed = new TimeSpan?(end - start)
      })));
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (Loan), "Committed", (ScopedEventHandler<PersistentObjectEventArgs>.BeforeDelegateExecution) ((type, start) => this.beforeExtensionInvoked.Invoke((object) this, new ExtensionInvocationEventArgs()
      {
        InvocationType = ExtensionInvocationType.OnCommited,
        Target = type
      })), (ScopedEventHandler<PersistentObjectEventArgs>.AfterDelegateExecution) ((type, start, end) => this.afterExtensionInvoked.Invoke((object) this, new ExtensionInvocationEventArgs()
      {
        InvocationType = ExtensionInvocationType.OnCommited,
        Target = type,
        Elapsed = new TimeSpan?(end - start)
      })));
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
      this.dataMgr.BeforeSavingLoanFiles += new EllieMae.EMLite.Common.CancelableEventHandler(this.onBeforeLoanSaved);
      this.dataMgr.BeforeLoanRefreshedFromServer += new EventHandler(this.onBeforeLoanRefreshedFromServer);
      this.dataMgr.OnLoanRefreshedFromServer += new EventHandler(this.onLoanRefreshedFromServer);
      this.initializeLoanDataListeners();
      this.initializeSubobjects();
    }

    /// <summary>
    /// Finalizer to ensure server side LoanProxy objects are released after client side loans go away
    /// </summary>
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

    /// <summary>
    /// Applies the investor from the provided <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorTemplate" /> to the current loan.
    /// </summary>
    /// <param name="investorTemplate">The <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorTemplate" /> to apply.</param>
    /// <remarks><para>The <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorTemplate" /> is obtained from the secondary settings.</para><para>Passing a null <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorTemplate" /> will throw an exception.</para></remarks>
    /// <example>
    ///     The following code manually applies a given investor to the current loan.
    ///     <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Templates;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan from the My Pipeline folder and retrieve the information on the current lock
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    ///       loan.Lock();
    /// 
    ///       // Get investor name from arguments
    ///       string investorName = args[0];
    /// 
    ///       // Get investor template for investor
    ///       InvestorTemplate investorTemplate = session.SystemSettings.Secondary.InvestorTemplates[investorName];
    /// 
    ///       // Check to make sure an investor template exists for the give name
    ///       if(investorTemplate == null)
    ///       {
    ///           Console.WriteLine("InvestorTemplate not found.");
    ///           return;
    ///       }
    /// 
    ///       // Applies the MilestoneTemplate to the loan.
    ///       loan.ApplyInvestorToLoan(investorTemplate);
    /// 
    ///       // Save and close the loan file
    ///       loan.Commit();
    ///       loan.Close();
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///     </code>
    ///   </example>
    public void ApplyInvestorToLoan(EllieMae.Encompass.BusinessEnums.InvestorTemplate investorTemplate)
    {
      if ((EnumItem) investorTemplate == (EnumItem) null)
        throw new NullReferenceException("The InvestorTemplate cannot be null.");
      this.dataMgr.ApplyInvestorToLoan(investorTemplate.Unwrap().CompanyInformation, (ContactInformation) null, true);
    }

    /// <summary>
    /// Applies a given <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.MilestoneTemplate" /> to the loan.
    /// </summary>
    /// <param name="milestoneTemplate">The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.MilestoneTemplate" /> to apply to the loan.</param>
    /// <param name="forceApplyMilestoneTemplate">Used to force apply the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.MilestoneTemplate" /> even when Manual mode is disabled.
    /// This parameter is optional. The default value is false.</param>
    /// <remarks><para>If Manual Mode is disabled, the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.MilestoneTemplate" /> conditions don't match the loan data while the "Allow non-matching templates to be applied to loans" is disabled or the given <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.MilestoneTemplate" /> is not active this will throw and exception.</para>
    /// <para>If the Milestone list on the loan is locked the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.MilestoneTemplate" /> will not be applied.</para></remarks>
    /// <example>
    ///       The following code manually applies a specific MilestoneTemplate to an existing loan file.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Templates;
    /// using EllieMae.Encompass.BusinessEnums;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan from the My Pipeline folder and retrieve the information on the current lock
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    ///       loan.Lock();
    /// 
    ///       // Retrieve the desired MilestoneTemplate
    ///       MilestoneTemplate mt = session.Loans.MilestoneTemplates.GetItemByName("Test Template");
    /// 
    ///       // Applies the MilestoneTemplate to the loan.
    ///       loan.ApplyManualMilestoneTemplate(mt);
    /// 
    ///       // Save and close the loan file
    ///       loan.Commit();
    ///       loan.Close();
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void ApplyManualMilestoneTemplate(
      EllieMae.Encompass.BusinessEnums.MilestoneTemplate milestoneTemplate,
      bool forceApplyMilestoneTemplate = false)
    {
      if (!forceApplyMilestoneTemplate)
      {
        switch ((MilestoneTemplatesSetting) this.dataMgr.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.MilestoneTemplateSettings"])
        {
          case MilestoneTemplatesSetting.Manual:
          case MilestoneTemplatesSetting.Both:
            break;
          default:
            throw new InvalidOperationException("Cannot manually apply a MilestoneTemplate when Manual mode is disabled.");
        }
      }
      List<EllieMae.EMLite.Workflow.MilestoneTemplate> milestoneTemplateList = new List<EllieMae.EMLite.Workflow.MilestoneTemplate>(this.dataMgr.SessionObjects.BpmManager.GetMilestoneTemplates(false));
      EllieMae.EMLite.Workflow.MilestoneTemplate template = (EllieMae.EMLite.Workflow.MilestoneTemplate) null;
      foreach (EllieMae.EMLite.Workflow.MilestoneTemplate milestoneTemplate1 in milestoneTemplateList)
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
      bool? nullable = new MilestoneTemplatesManager().ApplyMilestoneTemplate(this.dataMgr.SessionObjects, this.dataMgr.LoanData, (ILoanMilestoneTemplateOrchestrator) new MilestoneTemplateApply(this.dataMgr, template, true), (EllieMae.EMLite.Workflow.MilestoneTemplate) null, string.Empty);
      if (nullable.HasValue && !nullable.Value && !forceApplyMilestoneTemplate)
        throw new InvalidOperationException("MilestoneTemplate did not match loan conditions. Allow non-matching templates is disabled.");
    }

    /// <summary>
    /// Applies the best matching <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.MilestoneTemplate" /> to the loan.
    /// </summary>
    /// <param name="forceApplyMilestoneTemplate">Used to force apply the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.MilestoneTemplate" /> even when Manual mode is disabled.
    /// This parameter is optional. The default value is false.</param>
    public void ApplyBestMatchingMilestoneTemplate(bool forceApplyMilestoneTemplate = false)
    {
      if (!forceApplyMilestoneTemplate)
      {
        switch ((MilestoneTemplatesSetting) this.dataMgr.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.MilestoneTemplateSettings"])
        {
          case MilestoneTemplatesSetting.Manual:
          case MilestoneTemplatesSetting.Both:
            break;
          default:
            throw new InvalidOperationException("Cannot manually apply a MilestoneTemplate when Manual mode is disabled.");
        }
      }
      new MilestoneTemplatesManager().ApplyMilestoneTemplate(this.dataMgr.SessionObjects, this.dataMgr.LoanData, (ILoanMilestoneTemplateOrchestrator) new MilestoneTemplateApply(this.dataMgr, (EllieMae.EMLite.Workflow.MilestoneTemplate) null, true), (EllieMae.EMLite.Workflow.MilestoneTemplate) null, string.Empty);
    }

    /// <summary>
    /// Gets or, in the case of a new loan, sets the loan folder in which the loan resides.
    /// </summary>
    /// <example>
    /// The following code creates a new loan, sets the value of several fields
    /// and then commits the loan to the database.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the empty shell for the new loan. At this point,
    ///       // the loan has not been saved to the Encompass server.
    ///       Loan loan = session.Loans.CreateNew();
    /// 
    ///       // Set the loan folder and loan name for the loan
    ///       loan.LoanFolder = "My Pipeline";
    ///       loan.LoanName = "Harrison";
    /// 
    ///       // Set the borrower's name and property address
    ///       loan.Fields["36"].Value = "Howard";        // First name
    ///       loan.Fields["37"].Value = "Harrison";      // Last name
    ///       loan.Fields["11"].Value = "235 Main St.";  // Street Address
    ///       loan.Fields["12"].Value = "Anycity";       // City
    ///       loan.Fields["13"].Value = "Anycounty";     // County
    ///       loan.Fields["14"].Value = "CA";            // State
    ///       loan.Fields["15"].Value = "94432";         // Zip code
    /// 
    ///       // Save the loan to the server
    ///       loan.Commit();
    /// 
    ///       // Write out the GUID of the newly created loan
    ///       Console.WriteLine(loan.Guid);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
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

    /// <summary>
    /// Gets or, in the case of a new loan, sets the name of the loan.
    /// </summary>
    /// <remarks>The loan name must be unique within its loan folder.</remarks>
    /// <example>
    /// The following code creates a new loan, sets the value of several fields
    /// and then commits the loan to the database.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the empty shell for the new loan. At this point,
    ///       // the loan has not been saved to the Encompass server.
    ///       Loan loan = session.Loans.CreateNew();
    /// 
    ///       // Set the loan folder and loan name for the loan
    ///       loan.LoanFolder = "My Pipeline";
    ///       loan.LoanName = "Harrison";
    /// 
    ///       // Set the borrower's name and property address
    ///       loan.Fields["36"].Value = "Howard";        // First name
    ///       loan.Fields["37"].Value = "Harrison";      // Last name
    ///       loan.Fields["11"].Value = "235 Main St.";  // Street Address
    ///       loan.Fields["12"].Value = "Anycity";       // City
    ///       loan.Fields["13"].Value = "Anycounty";     // County
    ///       loan.Fields["14"].Value = "CA";            // State
    ///       loan.Fields["15"].Value = "94432";         // Zip code
    /// 
    ///       // Save the loan to the server
    ///       loan.Commit();
    /// 
    ///       // Write out the GUID of the newly created loan
    ///       Console.WriteLine(loan.Guid);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string LoanName
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.LoanName;
      }
      set => this.dataMgr.LoanName = this.Guid;
    }

    /// <summary>Gets the unique global identifier for the loan.</summary>
    /// <example>
    /// The following code creates a new loan, sets the value of several fields
    /// and then commits the loan to the database.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the empty shell for the new loan. At this point,
    ///       // the loan has not been saved to the Encompass server.
    ///       Loan loan = session.Loans.CreateNew();
    /// 
    ///       // Set the loan folder and loan name for the loan
    ///       loan.LoanFolder = "My Pipeline";
    ///       loan.LoanName = "Harrison";
    /// 
    ///       // Set the borrower's name and property address
    ///       loan.Fields["36"].Value = "Howard";        // First name
    ///       loan.Fields["37"].Value = "Harrison";      // Last name
    ///       loan.Fields["11"].Value = "235 Main St.";  // Street Address
    ///       loan.Fields["12"].Value = "Anycity";       // City
    ///       loan.Fields["13"].Value = "Anycounty";     // County
    ///       loan.Fields["14"].Value = "CA";            // State
    ///       loan.Fields["15"].Value = "94432";         // Zip code
    /// 
    ///       // Save the loan to the server
    ///       loan.Commit();
    /// 
    ///       // Write out the GUID of the newly created loan
    ///       Console.WriteLine(loan.Guid);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string Guid
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.LoanData.GUID;
      }
    }

    /// <summary>Gets or sets the internal loan number for the loan.</summary>
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

    /// <summary>Gets the Version Number of the loan</summary>
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

    /// <summary>Gets or sets the internal loan number for the loan.</summary>
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

    /// <summary>Gets or sets the MERS number for the loan.</summary>
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

    /// <summary>
    /// Enables or disables the calculation engine in Encompass.
    /// </summary>
    /// <remarks>By default, calculations within Encompass are always enabled. Setting this property
    /// to <c>false</c> will temporarily disable the calculations, which can have unexpected results
    /// when setting the values of fields which are used in calcaultions. Only modify this property if
    /// you have verified that your code works correctly when it is disabled.
    /// </remarks>
    public bool CalculationsEnabled
    {
      get => this.calculationsEnabled;
      set => this.calculationsEnabled = value;
    }

    /// <summary>
    /// Enables or disables the business rule validation for the loan.
    /// </summary>
    /// <remarks>If this property is <c>false</c> (the default), the business rules
    /// defined for loans will not be enforced. When <c>true</c>, all business rules will
    /// be evaluated. As a result, exceptions may occur when setting field values,
    /// moving the loan between milestones, etc.
    /// </remarks>
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

    /// <summary>
    /// Enables or disables exceptions when setting a field would violate a Loan Access Rule.
    /// </summary>
    public bool LoanAccessExceptionsEnabled
    {
      get => this.loanAccessExceptionsEnabled;
      set => this.loanAccessExceptionsEnabled = value;
    }

    /// <summary>Gets the last modification date for the loan.</summary>
    public DateTime LastModified
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.LastModified;
      }
    }

    /// <summary>
    /// Gets the Login ID of the Loan Officer for the current loan.
    /// </summary>
    public string LoanOfficerID => string.Concat(this.fields["LOID"].Value);

    /// <summary>
    /// Gets the Login ID of the Loan Processor for the current loan.
    /// </summary>
    public string LoanProcessorID => string.Concat(this.fields["LPID"].Value);

    /// <summary>
    /// Gets the Login ID of the Loan Processor for the current loan.
    /// </summary>
    public string LoanCloserID => string.Concat(this.fields["CLID"].Value);

    /// <summary>Provides access to all of the fields on the loan.</summary>
    /// <example>
    /// The following code creates a new loan, sets the value of several fields
    /// and then commits the loan to the database.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the empty shell for the new loan. At this point,
    ///       // the loan has not been saved to the Encompass server.
    ///       Loan loan = session.Loans.CreateNew();
    /// 
    ///       // Set the loan folder and loan name for the loan
    ///       loan.LoanFolder = "My Pipeline";
    ///       loan.LoanName = "Harrison";
    /// 
    ///       // Set the borrower's name and property address
    ///       loan.Fields["36"].Value = "Howard";        // First name
    ///       loan.Fields["37"].Value = "Harrison";      // Last name
    ///       loan.Fields["11"].Value = "235 Main St.";  // Street Address
    ///       loan.Fields["12"].Value = "Anycity";       // City
    ///       loan.Fields["13"].Value = "Anycounty";     // County
    ///       loan.Fields["14"].Value = "CA";            // State
    ///       loan.Fields["15"].Value = "94432";         // Zip code
    /// 
    ///       // Save the loan to the server
    ///       loan.Commit();
    /// 
    ///       // Write out the GUID of the newly created loan
    ///       Console.WriteLine(loan.Guid);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public LoanFields Fields
    {
      get
      {
        this.ensureOpen();
        return this.fields;
      }
    }

    /// <summary>
    /// Provides access to defined sets of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair">BorrowerPairs</see> as well as providing methods
    /// and proeprties for adding or removing BorrowerPairs.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to add a second Borrower Pair to a loan
    ///       and then manipulate the two pairs independently.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.StartOffline("mary", "maryspwd");
    /// 
    ///          // Create the empty shell for the new loan.
    ///          Loan loan = session.Loans.CreateNew();
    /// 
    ///          // Set the loan folder and loan name for the loan
    ///          loan.LoanFolder = "My Pipeline";
    ///          loan.LoanName = "BorrowerPairExample";
    /// 
    ///          // The loan is created with one Borrower Pair already, so set
    ///          // the borrower and coborrower names
    ///          loan.Fields["36"].Value = "Howard";        // Borrower First name
    ///          loan.Fields["37"].Value = "Harrison";      // Borrower Last name
    ///          loan.Fields["68"].Value = "Martha";        // CoBorrower First name
    ///          loan.Fields["69"].Value = "Harrison";      // CoBorrower Last name
    /// 
    ///          // Add a new borrower pair to the loan
    ///          BorrowerPair newPair = loan.BorrowerPairs.Add();
    /// 
    ///          // Set the borrower and coborrower information for this pair
    ///          loan.Fields["36"].SetValueForBorrowerPair(newPair, "Caroline");
    ///          loan.Fields["37"].SetValueForBorrowerPair(newPair, "Irving");
    ///          loan.Fields["68"].SetValueForBorrowerPair(newPair, "Thomas");
    ///          loan.Fields["69"].SetValueForBorrowerPair(newPair, "Irving");
    /// 
    ///          // Set the newly created pair as the current (primary) pair
    ///          loan.BorrowerPairs.Current = newPair;
    /// 
    ///          // Set the mailing address for the "current" pair
    ///          loan.Fields["1519"].Value = "20221 Highway 99";
    ///          loan.Fields["1520"].Value = "Maynorsville";
    ///          loan.Fields["1521"].Value = "IA";
    ///          loan.Fields["1522"].Value = "51223";
    /// 
    ///          // Commit the changes to the server
    ///          loan.Commit();
    /// 
    ///          // End the session gracefully
    ///          session.End();
    ///    }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
    public LoanBorrowerPairs BorrowerPairs
    {
      get
      {
        this.ensureOpen();
        return this.pairs;
      }
    }

    /// <summary>
    /// Provides access to the methods for adding to and removing from the defined
    /// set of liabilities for the loan.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to add a new liability to an existing
    ///       loan and then set its field values.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          // Lock the loan so we can modify it safely
    ///          loan.Lock();
    /// 
    ///          // Add a new liability and save off it index in the liabilities list
    ///          int newIndex = loan.Liabilities.Add();
    /// 
    ///          // Set the value of some of theliability fields
    ///          loan.Fields.GetFieldAt("FL02", newIndex).Value = "Bank of Havasu";  // Liability Holder
    ///          loan.Fields.GetFieldAt("FL10", newIndex).Value = "2220001-003";     // Account #
    ///          loan.Fields.GetFieldAt("FL20", newIndex).Value = "(555) 555-0233";  // Holder Phone
    /// 
    ///          // Commit the changes to the server
    ///          loan.Commit();
    /// 
    ///          // Release the lock on the loan
    ///          loan.Unlock();
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
    public LoanLiabilities Liabilities
    {
      get
      {
        this.ensureOpen();
        return this.liabilities;
      }
    }

    /// <summary>
    /// Provides access to the methods for adding to and removing from the defined
    /// set of mortgages for the loan.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to add an existing mortgage to a loan file
    ///       and then set a subset of its field values.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          // Lock the loan so we can modify it safely
    ///          loan.Lock();
    /// 
    ///          // Add a new liability and save off it index in the liabilities list
    ///          int newIndex = loan.Liabilities.Add();
    /// 
    ///          // Set the value of some of theliability fields
    ///          loan.Fields.GetFieldAt("FL02", newIndex).Value = "Thomas Olden";    // Asset Holder
    ///          loan.Fields.GetFieldAt("FL10", newIndex).Value = "2220001-003";     // Account #
    ///          loan.Fields.GetFieldAt("FL20", newIndex).Value = "(555) 555-0233";  // Holder Phone
    /// 
    ///          // Create an IntegerList to hold the ID of the liability
    ///          IntegerList liabIds = new IntegerList();
    ///          liabIds.Add(newIndex);
    /// 
    ///           // Create the new Mortgage, attaching the liability to it
    ///          int newMort = loan.Mortgages.Add(liabIds);
    /// 
    ///          // Set some Mortgage-related fields
    ///          loan.Fields.GetFieldAt("FM04", newMort).Value = "2056 Blue Hollow Lane";  // Street Addr
    ///          loan.Fields.GetFieldAt("FM06", newMort).Value = "Lake Mary";              // City
    ///          loan.Fields.GetFieldAt("FM07", newMort).Value = "FL";                     // State
    /// 
    ///          // Commit the changes to the server
    ///          loan.Commit();
    /// 
    ///          // Release the lock on the loan
    ///          loan.Unlock();
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
    public LoanMortgages Mortgages
    {
      get
      {
        this.ensureOpen();
        return this.mortgages;
      }
    }

    /// <summary>
    /// Provides access to the methods for adding to and removing from the defined
    /// set of assets/deposits for the loan.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to add a new asset/deposit to an existing
    ///       loan and then set its field values.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          // Lock the loan so we can modify it safely
    ///          loan.Lock();
    /// 
    ///          // Add a new deposit and save off it index in the deposits list
    ///          int newIndex = loan.Deposits.Add();
    /// 
    ///          // Set the value of some of theliability fields
    ///          loan.Fields.GetFieldAt("FD02", newIndex).Value = "Thomas Olden";    // Asset Holder
    ///          loan.Fields.GetFieldAt("FD10", newIndex).Value = "2220001-003";     // Account #
    ///          loan.Fields.GetFieldAt("FD26", newIndex).Value = "(555) 555-0233";  // Holder Phone
    /// 
    ///          // Commit the changes to the server
    ///          loan.Commit();
    /// 
    ///          // Release the lock on the loan
    ///          loan.Unlock();
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
    public LoanDeposits Deposits
    {
      get
      {
        this.ensureOpen();
        return this.deposits;
      }
    }

    /// <summary>
    /// Provides access to the methods for adding to and removing from the defined
    /// set of borrower residences (past and current) for the loan.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to print the addresses of all of the
    ///       prior residences of both the primary borrower and the coborrower.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open an existing loan using the GUID from the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Loop over the set of residences, printing the addresses
    ///       for (int i = 1; i <= loan.BorrowerResidences.Count; i++)
    ///       {
    ///          Console.WriteLine("Borrower Residence " + i + ":");
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BR04", i));   // Street Addr
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BR06", i));   // City
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BR07", i));   // State
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BR08", i));   // Zip
    ///       }
    /// 
    ///       // Now the CoBorrower residences
    ///       for (int i = 1; i <= loan.CoBorrowerResidences.Count; i++)
    ///       {
    ///          Console.WriteLine("CoBorrower Residence " + i + ":");
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CR04", i));   // Street Addr
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CR06", i));   // City
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CR07", i));   // State
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CR08", i));   // Zip
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanResidences BorrowerResidences
    {
      get
      {
        this.ensureOpen();
        return this.borResidences;
      }
    }

    /// <summary>
    /// Provides access to the methods for adding to and removing from the defined
    /// set of coborrower residences (past and current) for the loan.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to print the addresses of all of the
    ///       prior residences of both the primary borrower and the coborrower.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open an existing loan using the GUID from the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Loop over the set of residences, printing the addresses
    ///       for (int i = 1; i <= loan.BorrowerResidences.Count; i++)
    ///       {
    ///          Console.WriteLine("Borrower Residence " + i + ":");
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BR04", i));   // Street Addr
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BR06", i));   // City
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BR07", i));   // State
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BR08", i));   // Zip
    ///       }
    /// 
    ///       // Now the CoBorrower residences
    ///       for (int i = 1; i <= loan.CoBorrowerResidences.Count; i++)
    ///       {
    ///          Console.WriteLine("CoBorrower Residence " + i + ":");
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CR04", i));   // Street Addr
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CR06", i));   // City
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CR07", i));   // State
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CR08", i));   // Zip
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanResidences CoBorrowerResidences
    {
      get
      {
        this.ensureOpen();
        return this.cobResidences;
      }
    }

    /// <summary>
    /// Provides access to the methods for adding to and removing from the defined
    /// set of borrower employers (past and current) for the loan.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to print the names and locations of all of the
    ///       prior employers for both the primary borrower and the coborrower.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open an existing loan using the GUID from the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Loop over the set of residences, printing the addresses
    ///       for (int i = 1; i <= loan.BorrowerEmployers.Count; i++)
    ///       {
    ///          Console.WriteLine("Borrower Employer " + i + ":");
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BE02", i));   // Employer Name
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BE05", i));   // City
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BE06", i));   // State
    ///       }
    /// 
    ///       // Now the CoBorrower residences
    ///       for (int i = 1; i <= loan.CoBorrowerEmployers.Count; i++)
    ///       {
    ///          Console.WriteLine("CoBorrower Employer " + i + ":");
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CE02", i));   // Employer Name
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CE05", i));   // City
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CE06", i));   // State
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanEmployers BorrowerEmployers
    {
      get
      {
        this.ensureOpen();
        return this.borEmployers;
      }
    }

    /// <summary>
    /// Provides access to the methods for adding to and removing from the defined
    /// set of coborrower employers (past and current) for the loan.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to print the names and locations of all of the
    ///       prior employers for both the primary borrower and the coborrower.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open an existing loan using the GUID from the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Loop over the set of residences, printing the addresses
    ///       for (int i = 1; i <= loan.BorrowerEmployers.Count; i++)
    ///       {
    ///          Console.WriteLine("Borrower Employer " + i + ":");
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BE02", i));   // Employer Name
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BE05", i));   // City
    ///          Console.WriteLine(loan.Fields.GetFieldAt("BE06", i));   // State
    ///       }
    /// 
    ///       // Now the CoBorrower residences
    ///       for (int i = 1; i <= loan.CoBorrowerEmployers.Count; i++)
    ///       {
    ///          Console.WriteLine("CoBorrower Employer " + i + ":");
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CE02", i));   // Employer Name
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CE05", i));   // City
    ///          Console.WriteLine(loan.Fields.GetFieldAt("CE06", i));   // State
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanEmployers CoBorrowerEmployers
    {
      get
      {
        this.ensureOpen();
        return this.cobEmployers;
      }
    }

    /// <summary>
    /// Provides access to the methods for adding to and removing from the defined
    /// set of Additiona Vesting Parties for the loan.
    /// </summary>
    public LoanVestingParties AdditionalVestingParties
    {
      get
      {
        this.ensureOpen();
        return this.vestingParties;
      }
    }

    /// <summary>Returns non borrowing owner contacts</summary>
    public NonBorrowingOwnerContacts NBOContacts
    {
      get
      {
        this.ensureOpen();
        return this.nonBorrowingOwnerContacts;
      }
    }

    /// <summary>Returns Change Of Circumstance Entries</summary>
    public ChangeOfCircumstanceEntries CoCEntries
    {
      get
      {
        this.ensureOpen();
        return this.changeOfCircumstanceEntries;
      }
    }

    /// <summary>
    /// Provides access to the loan's log, which provides a record of all
    /// events, past and future, in the timeline of the current loan.
    /// </summary>
    /// <include file="Loan.xml" path="Examples/Example[@name=&quot;Loan.Log&quot;]/*" />
    public LoanLog Log
    {
      get
      {
        this.ensureOpen();
        return this.log;
      }
    }

    /// <summary>
    /// Provides access to the loan's current MilestoneTemplate, which provides information of the
    /// template applied to the loan.
    /// </summary>
    public EllieMae.Encompass.BusinessEnums.MilestoneTemplate MilestoneTemplate
    {
      get
      {
        this.ensureOpen();
        return new EllieMae.Encompass.BusinessEnums.MilestoneTemplate(this);
      }
    }

    /// <summary>
    /// Provides access to the set of documents attached to the loan through the
    /// loan's eFolder.
    /// </summary>
    /// <example>
    /// The following code demonstrates how to iterate over each attachment associated
    /// with a loan and extract it to disk.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Iterate over the list of attachments, saving them to the C:\Temp folder
    ///       foreach (Attachment att in loan.Attachments)
    ///          att.SaveToDisk("C:\\Temp\\" + att.Name);
    /// 
    ///       // Close the loan, discarding all of our changes
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public LoanAttachments Attachments
    {
      get
      {
        this.ensureOpen();
        this.ensureAttachmentsCompletedInitialization();
        return this.attachments;
      }
    }

    /// <summary>Gets the collection of Loan Associates for the loan.</summary>
    /// <example>
    ///       The following code assigns a users to the fixed Loan Officer role in the loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan from the My Pipeline folder and retrieve the information on the current lock
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    ///       loan.Lock();
    /// 
    ///       User u = session.Users.GetUser("officer");
    ///       loan.Associates.AssignUser(FixedRole.LoanOfficer, u);
    /// 
    ///       // Save and close the loan file
    ///       loan.Commit();
    ///       loan.Close();
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanAssociates Associates
    {
      get
      {
        this.ensureOpen();
        return this.associates;
      }
    }

    /// <summary>
    /// Gets the collection of Contact Relationships for the loan.
    /// </summary>
    /// <example>
    ///       The following code assigns a users to the fixed Loan Officer role in the loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan from the My Pipeline folder and retrieve the information on the current lock
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    /// 
    ///       // Iterate over the contact relationships that are part of the loan
    ///       // and identify the appraiser, if one exists.
    ///       foreach (LoanContactRelationship rel in loan.Contacts)
    ///         if (rel.RelationshipType == LoanContactRelationshipType.Appraiser)
    ///         {
    ///           BizContact contact = (BizContact)rel.OpenContact();
    ///           Console.WriteLine("The appraiser is " + contact.FullName +
    ///             " and has an email address of " + contact.BizEmail);
    ///           break;
    ///         }
    /// 
    ///       // Close the loan file
    ///       loan.Close();
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanContacts Contacts
    {
      get
      {
        this.ensureOpen();
        return this.contacts;
      }
    }

    /// <summary>
    /// Provides access to audit trail information for the loan.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to retrieve the list of all fields
    ///       included in the Audit Trail and, for each field, display the history of changes
    ///       to its value.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class SampleApp
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Iterate over the list of fields that are included in the audit trail
    ///       foreach (string fieldId in loan.AuditTrail.GetAuditFieldList())
    ///       {
    ///         Console.WriteLine("Audit trail for field " + fieldId);
    /// 
    ///         // Retrieve the history for the current field
    ///         AuditTrailEntryList entries = loan.AuditTrail.GetHistory(fieldId);
    /// 
    ///         // Iterate over the historical changes and print the time of the change and
    ///         // the user's identity that made the change.
    ///         foreach (AuditTrailEntry e in entries)
    ///           Console.WriteLine("   -> " + e.Timestamp + " by " + e.UserName + " (" + e.UserID + ") -> " + e.Field.FormattedValue);
    /// 
    ///         Console.WriteLine();
    ///       }
    /// 
    ///       // Close the loan, releasing its resources
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanAuditTrail AuditTrail
    {
      get
      {
        this.ensureOpen();
        return this.auditTrail;
      }
    }

    /// <summary>
    /// Provides access to interim servicing data for the loan.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to activate the servicing activities
    ///       for a loan and display the payment schedule for the loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class SampleApp
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Set the first payment date on the loan. This is a required field before
    ///       // starting servicing since it determines the payment schedule for the loan
    ///       loan.Fields["682"].Value = "6/1/2008";
    /// 
    ///       // First, activate the servicing if not already started. This will calculate
    ///       // the initial payment schedule for the loan.
    ///       if (!loan.Servicing.IsStarted())
    ///         loan.Servicing.Start();
    /// 
    ///       // Display the payment schedule on the screen, showing the date of each
    ///       // payment along with the amount of principal and interest due.
    ///       PaymentSchedule schedule = loan.Servicing.GetPaymentSchedule();
    /// 
    ///       foreach (ScheduledPayment payment in schedule.Payments)
    ///         Console.WriteLine(payment.DueDate + ": P = " + payment.Principal + ", I = " + payment.Interest);
    /// 
    ///       // Close the loan, releasing its resources
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanServicing Servicing
    {
      get
      {
        this.ensureOpen();
        return this.servicing;
      }
    }

    /// <summary>
    /// Gets the collection of URLA Additional Loans for the loan.
    /// </summary>
    public LoanURLAAdditionalLoans URLAAdditionalLoans
    {
      get
      {
        this.ensureOpen();
        return this.urlaAdditionalLoans;
      }
    }

    /// <summary>
    /// Gets the collection of URLA Gifts Grants for the loan.
    /// </summary>
    public LoanURLAGiftsGrants URLAGiftsGrants
    {
      get
      {
        this.ensureOpen();
        return this.urlaGiftsGrants;
      }
    }

    /// <summary>
    /// Gets the collection of URLA Other Assets for the loan.
    /// </summary>
    public LoanURLAOtherAssets URLAOtherAssets
    {
      get
      {
        this.ensureOpen();
        return this.urlaOtherAssets;
      }
    }

    /// <summary>
    /// Gets the collection of URLA Other Income for the loan.
    /// </summary>
    public LoanURLAOtherIncome URLAOtherIncome
    {
      get
      {
        this.ensureOpen();
        return this.urlaOtherIncome;
      }
    }

    /// <summary>
    /// Gets the collection of URLA Other Liabilities for the loan.
    /// </summary>
    public LoanURLAOtherLiabilities URLAOtherLiabilities
    {
      get
      {
        this.ensureOpen();
        return this.urlaOtherLiabilities;
      }
    }

    /// <summary>
    /// Returns the loan which is linked to the current loan or <c>null</c> if
    /// no loan is currently linked.
    /// </summary>
    /// <remarks>Linked loans are typically used in a piggy-back loan scenario
    /// in which one loan is the primary and the other the secondary. The relative positions
    /// of the loans is determined by the lien positions set on the loans themselves.
    /// </remarks>
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

    /// <summary>Gets or sets if the Milestone list is locked.</summary>
    /// <remarks>
    /// A true value means the Milestone list is locked in manual mode and MilestoneTemplate will not be applied automatically on save.
    /// A false value means the Milestone list is not in manual mode and MillstoneTemplate will be applied automatically on save.
    /// </remarks>
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

    /// <summary>Gets or sets if the Milestone dates are locked.</summary>
    /// <remarks>
    /// A true value means future expected milestone dates will not but updated when one is changed.
    /// A false value means future expected milestone dates will be upated when one is changed.
    /// </remarks>
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

    /// <summary>Links the current loan to another loan.</summary>
    /// <param name="loan">The loan to which to link the current loan.</param>
    public void LinkTo(Loan loan)
    {
      this.EnsureExclusive();
      loan.EnsureExclusive();
      this.dataMgr.LinkTo(loan.dataMgr);
      this.linkedLoan = loan;
      loan.linkedLoan = this;
    }

    /// <summary>
    /// Breaks the link between the current loan and its linked loan, if one is present.
    /// </summary>
    public void Unlink()
    {
      this.linkedLoan.EnsureExclusive();
      this.EnsureExclusive();
      if (this.linkedLoan != null)
        this.linkedLoan.linkedLoan = (Loan) null;
      this.dataMgr.Unlink();
      this.linkedLoan = (Loan) null;
    }

    /// <summary>
    /// Sets the Loan Officer (LO) currently responsible for this loan file.
    /// </summary>
    /// <param name="loanOfficer">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User">User</see> object
    /// representing the new Loan Officer.</param>
    /// <remarks>This method is equivalent to calling <c>Loan.Associates.AssignUser(FixedRole.LoanOfficer, loanOfficer)</c>.
    /// The system determines which <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> is mapped to the LoanOfficer <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FixedRole" />
    /// and then assigns the user to all LoanAssociate records for that role.
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to set the Loan Officer currently
    ///       responsible for a loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open an existing loan using the GUID from the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Get the "Loan Officer" Persona
    ///       Persona lo = session.Users.Personas.GetPersonaByName("Loan Officer");
    /// 
    ///       // Fetch the list of Loan Officers from the server
    ///       UserList los = session.Users.GetUsersWithPersona(lo, false);
    /// 
    ///       if (los.Count > 0)
    ///       {
    ///       	// Lock the loan so we can edit it
    ///       	loan.Lock();
    /// 
    ///       	// Assign the first LO returned to the loan. This will automatically
    ///       	// grant that user read/write access rights to the current loan.
    ///       	loan.SendToLoanOfficer(los[0]);
    /// 
    ///       	// Commit the change and unlock the loan
    ///       	loan.Commit();
    ///       	loan.Unlock();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
    public void SendToLoanOfficer(User loanOfficer)
    {
      this.Associates.AssignUser(FixedRole.LoanOfficer, loanOfficer);
    }

    /// <summary>
    /// Sends the current loan to processing and sets the Loan Processor (LP)
    /// responsible for handling the loan.
    /// </summary>
    /// <param name="loanProcessor">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User">User</see> object
    /// representing the new Loan Processor.</param>
    /// <remarks>This method is equivalent to calling <c>Loan.Associates.AssignUser(FixedRole.LoanProcessor, loanProcessor)</c>.
    /// The system determines which <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> is mapped to the LoanProcessor <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FixedRole" />
    /// and then assigns the user to all LoanAssociate records for that role.
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to send a loan to processing by specifying
    ///       the Loan Processor who will handle this loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open an existing loan using the GUID from the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Get the "Loan Officer" Persona
    ///       Persona lp = session.Users.Personas.GetPersonaByName("Loan Processor");
    /// 
    ///       // Fetch the list of Loan Officers from the server
    ///       UserList los = session.Users.GetUsersWithPersona(lp, false);
    /// 
    ///       if (lps.Count > 0)
    ///       {
    ///       	// Lock the loan so we can edit it
    ///       	loan.Lock();
    /// 
    ///       	// Assign the first LP returned to the loan. This user will automatically
    ///       	// be granted Read/Write rights to the loan when the loan is committed.
    ///       	loan.SendToProcessing(los[0]);
    /// 
    ///       	// Commit and unlock the loan
    ///       	loan.Commit();
    ///       	loan.Unlock();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
    public void SendToProcessing(User loanProcessor)
    {
      this.Associates.AssignUser(FixedRole.LoanProcessor, loanProcessor);
    }

    /// <summary>
    /// Locks the loan for exclusive editing. Once locked, you may make changes to the loan and
    /// invoke the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Commit">Commit</see> method to save those changes to the
    /// server.
    /// </summary>
    /// <remarks>This method obtains an exclusive lock on the loan.</remarks>
    /// <example>
    ///       The following code demonstrates how to safely lock and unlock a loan in order
    ///       to make changes to its data.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          // Lock the loan
    ///          loan.Lock();
    /// 
    ///          // Modify some of the loan data
    ///          loan.Fields["11"].Value = "3094 Underwood Lane";    // Property Address
    ///          loan.Fields["12"].Value = "Westchester";            // Property City
    ///          loan.Fields["14"].Value = "PA";                     // Property State
    /// 
    ///          // Commit the changes
    ///          loan.Commit();
    /// 
    ///          // Unlock the loan, allowing other clients to obtain a lock
    ///          loan.Unlock();
    /// 
    ///          // We can still safely read data from the loan
    ///          Console.WriteLine(loan.Fields["1335"].Value);       // Down Payment
    ///          Console.WriteLine(loan.Fields["LOANSUB.X3"].Value); // Appraisal Fee
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
    public void Lock() => this.Lock(true);

    /// <summary>
    /// Locks the loan for editing. Once locked, you may make changes to the loan and
    /// invoke the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Commit">Commit</see> method to save those changes to the
    /// server.
    /// </summary>
    /// <param name="exclusive">Indicates if an exclusive lock is required.</param>
    /// <remarks><p>A loan can be locked for either exclusive or shared editing. An exclusive lock
    /// will prevent other users from editing the loan, but obtaining an exclusive lock requires
    /// that no other user has an exclusive or shared lock on the loan.</p>
    /// <p>A shared lock will allow other users to open and edit the loan as long as they are
    /// also using a shared lock. If an exclusive lock is on the loan, you cannot obtain a shared lock
    /// and an exception will occur.</p>
    /// <p>If you have previously obtained a shared lock and you need to upgrade it to an exclusive
    /// lock, you can invoke the Lock() method a second time, passing <c>true</c> for the parameter.
    /// If you have an exclusive lock, you cannot downgrade the lock to a shared lock without
    /// unlocking the loan completely (via the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Unlock" /> method).</p>
    /// </remarks>
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
      LockInfo.ExclusiveLock exclusive1 = exclusive ? LockInfo.ExclusiveLock.Exclusive : LockInfo.ExclusiveLock.Nonexclusive;
      try
      {
        this.dataMgr.Lock(LoanInfo.LockReason.OpenForWork, exclusive1);
        this.currentLock = (LoanLock) null;
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.LockException ex)
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
        this.Session.SessionObjects.ServerManager.SendMessage((Message) new CEMessage(this.Session.GetUserInfo(), CEMessageType.UserOpenLoan), workingOnTheLoan.SessionIDs, true);
      }
      catch
      {
      }
    }

    /// <summary>
    /// Unlocks the loan once editing has been finished. Any pending changes should be
    /// Committed prior to unlocking the loan.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to safely lock and unlock a loan in order
    ///       to make changes to its data.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          // Lock the loan
    ///          loan.Lock();
    /// 
    ///          // Modify some of the loan data
    ///          loan.Fields["11"].Value = "3094 Underwood Lane";    // Property Address
    ///          loan.Fields["12"].Value = "Westchester";            // Property City
    ///          loan.Fields["14"].Value = "PA";                     // Property State
    /// 
    ///          // Commit the changes
    ///          loan.Commit();
    /// 
    ///          // Unlock the loan, allowing other clients to obtain a lock
    ///          loan.Unlock();
    /// 
    ///          // We can still safely read data from the loan
    ///          Console.WriteLine(loan.Fields["1335"].Value);       // Down Payment
    ///          Console.WriteLine(loan.Fields["LOANSUB.X3"].Value); // Appraisal Fee
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
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
        this.Session.SessionObjects.ServerManager.SendMessage((Message) new CEMessage(this.Session.GetUserInfo(), CEMessageType.UserExitLoan), workingOnTheLoan.SessionIDs, true);
      }
      catch
      {
      }
    }

    /// <summary>
    /// Returns the current <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanLock" /> on the loan.
    /// </summary>
    /// <returns>The lock information for the current loan, or <c>null</c> if no lock is currently
    /// held on this loan. If concurrent editing is enabled, you should consider calling
    /// <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.GetCurrentLocks" /> instead to retrieve all locks held on the current loan.
    /// If multiple locks are held on this loan, this method will return only one of them.</returns>
    /// <example>
    ///       The following code opens a loan and retrieves the information about the
    ///       current lock holder, if any.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open a loan from the My Pipeline folder and retrieve the information on the current lock
    ///          Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    ///          LoanLock lockInfo = loan.GetCurrentLock();
    /// 
    ///          if (lockInfo == null)
    ///          Console.WriteLine("Loan is not locked");
    ///          else
    ///          Console.WriteLine("Loan is locked by " + lockInfo.LockedBy + ", since " + lockInfo.LockedSince +
    ///          ", for " + lockInfo.LockType);
    /// 
    ///          // Close the loan and end the session
    ///          loan.Close();
    ///          session.End();
    ///    }
    /// }
    ///               ]]>
    ///       </code>
    ///     </example>
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

    /// <summary>
    /// Returns all locks currently held <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanLock" /> on the loan.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.LoanLockList" /> containing all locks held on the current loan.
    /// If concurrent editing is enabled and the loan is being edited by multiple users, a LockLock
    /// object will be returned for each user who has a lock on the loan.</returns>
    /// <example>
    ///       The following code opens a loan and retrieves the information about the
    ///       current locks, if any.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open a loan from the My Pipeline folder and retrieve the information on the current lock
    ///          Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    ///          LoanLockList lockInfoList = loan.GetCurrentLocks();
    /// 
    ///          foreach (LoanLock lockInfo in lockInfoList)
    ///          {
    ///             Console.WriteLine("Loan is locked by " + lockInfo.LockedBy + ", since " + lockInfo.LockedSince +
    ///             ", for " + lockInfo.LockType);
    ///          }
    /// 
    ///          // Close the loan and end the session
    ///          loan.Close();
    ///          session.End();
    ///    }
    /// }
    ///               ]]>
    ///       </code>
    ///     </example>
    public LoanLockList GetCurrentLocks()
    {
      LoanLockList currentLocks = new LoanLockList();
      foreach (LockInfo currentLock in this.dataMgr.GetCurrentLocks())
      {
        if (currentLock.LockedFor != LoanInfo.LockReason.NotLocked)
          currentLocks.Add(new LoanLock(currentLock));
      }
      return currentLocks;
    }

    /// <summary>
    /// Removes an existing locks on the current loan and then locks it for the current user.
    /// </summary>
    /// <remarks>If a lock already exists on the loan, this function will remove it before
    /// applying a lock for the current user. This action may cause a user editing the document in Encompass
    /// to lose their changes as they will not be able to save the loan. Only invoke
    /// this function if you know that existing lock is no longer valid.
    /// <p>When you are done with the lock, call the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Unlock" /> method. It is
    /// not necessary to call <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.ForceUnlock" /> to remove the lock applied by
    /// this method.</p>
    /// <p>This function requires Administrator rights to invoke if a lock already exists
    /// on the loan.</p>
    /// </remarks>
    /// <example>
    ///       The following code opens a loan and forces a lock onto it. If another user was
    ///       modifying the loan, their changes will be lost.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///           // Open a loan from the My Pipeline folder
    ///          Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    /// 
    ///          // Force a lock on the loan and then modify the loan amount
    ///          loan.ForceLock();
    ///          loan.Fields["1109"].Value = "167000";
    /// 
    ///          // Commit the changes and unlock the loan
    ///          loan.Commit();
    ///          loan.Unlock();
    /// 
    ///          // Close the loan and end the session
    ///          loan.Close();
    ///          session.End();
    ///    }
    /// }
    ///               ]]>
    ///       </code>
    ///     </example>
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

    /// <summary>Removes any lock currently held on the loan.</summary>
    /// <remarks>This function may cause a user editing the document in Encompass
    /// to lose their changes as they will not be able to save the loan. Only invoke
    /// this function if you know that existing lock is no longer valid. To remove
    /// a lock on the loan which was created using the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Lock" /> method,
    /// call <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Unlock" /> instead.
    /// <p>This function requires Administrator rights to invoke.</p>
    /// </remarks>
    /// <example>
    ///       The following code attempts to open a loan and, if it finds the loan is already locked,
    ///       forcibly removes the lock. This is logically equivalent to the functionality of the
    ///       <see href="ForceLock" /> method.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open a loan from the My Pipeline folder
    ///          Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    /// 
    ///          try
    ///          {
    ///             // Attempt the lock on the loan
    ///             loan.Lock();
    ///          }
    ///          catch (LockException)
    ///          {
    ///             // The LockException means the loan is already locked, so we forcibly unlock it.
    ///             // This will cause the user who has the loan locked to be unable to save their
    ///             // changes (if any).
    ///             loan.ForceUnlock();
    ///             loan.Lock();
    ///          }
    /// 
    ///          // Modify the loan amount now that the loan is locked
    ///          loan.Fields["1109"].Value = "156000";
    /// 
    ///          // Commit the changes and unlock the loan
    ///          loan.Commit();
    ///          loan.Unlock();
    /// 
    ///          // Close the loan and end the session
    ///          loan.Close();
    ///          session.End();
    ///    }
    /// }
    ///               ]]>
    ///       </code>
    ///     </example>
    public void ForceUnlock()
    {
      this.ensureExists();
      this.dataMgr.Unlock(true);
    }

    /// <summary>
    /// Indicates if the Loan object has been locked for editing by the current session.
    /// </summary>
    /// <remarks>This method only indicates if the loan is locked by the current session (i.e.
    /// the Lock() method has been invoked successfully). It does not indicate whether or not
    /// a lock is held on the loan by another user. For information on what lock, if any, is held
    /// on the loan, use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.GetCurrentLock" /> method.</remarks>
    public bool Locked
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.Writable;
      }
    }

    /// <summary>
    /// Indicates if an exclusive lock is held on the loan by the current session.
    /// </summary>
    /// <remarks>This method only indicates if the loan is locked by the current session (i.e.
    /// the Lock() method has been invoked successfully). It does not indicate whether or not
    /// a lock is held on the loan by another user. For information on what lock, if any, is held
    /// on the loan, use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.GetCurrentLock" /> method.</remarks>
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

    /// <summary>
    /// Forces a full recalculation for the loan's calculated fields.
    /// </summary>
    public void Recalculate()
    {
      this.ensureOpen();
      this.dataMgr.Calculator.CalculateAll();
    }

    /// <summary>
    /// Executes a specific calculation for a form or process.
    /// </summary>
    /// <param name="calcName">The name of the calculation to be run.</param>
    /// <remarks>This method is for internal use by Encompass only.</remarks>
    public void ExecuteCalculation(string calcName)
    {
      this.ensureOpen();
      this.dataMgr.Calculator.FormCalculation(calcName, (string) null, (string) null);
    }

    /// <summary>Execute calculation using trigger option</summary>
    /// <param name="triggerOption"></param>
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

    /// <summary>
    /// Exports the loan information to one of serveral supported formats.
    /// </summary>
    /// <param name="format">The target format to which the loan data will
    /// be converted.</param>
    /// <param name="exportKey">Your Encompass API Export Key (see Remarks below for more information).</param>
    /// <param name="filePath">The path to which to export the file. Any existing
    /// file will be overwritten.</param>
    /// <returns>A string containing the formatted loan data.</returns>
    /// <remarks>
    /// <p>Exporting loans using the API requires the use of an Encompass API Export Key.
    /// This key is the same as the CD Key used to register the Encompass API SDK/Runtime.</p>
    /// <p>If you are a licensed Encompass Partner and your application is being distributed to Encompass Customers,
    /// you should use your own CD Key when calling this method, not the CD Key of of the individual
    /// customers who use your software. In general, simply hard-coding this key to your Encompass API
    /// CD Key is the preferred technique.</p>
    /// <p>For customer doing their own in-house development, simply use your own Encompass API Key
    /// for this purpose.</p>
    /// <p>Before Encompass will perform the export, it must first validate your Export Key by communicating
    /// with the Encompass Licensing Servers. To do this, your application must be running on a computer
    /// which has Internet access and which permits anonymous requests to the host <c>encompass.elliemae.com</c>.
    /// If you are running behind a proxy which requires authentication for outbound connections, an exception
    /// will need to be made for this host in order to export data with this method.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to export the data of an existing loan
    ///       to Fannie Mae 3.2 format.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          string exportPath = "C:\\Export\\" + loan.LoanName;
    /// 
    ///          // Export the loan to Fannie Mae 3.2 format. You would need to provide
    ///          // your own Export Key to perform the export.
    ///          loan.Export(exportPath, "AAAAAAAAAA", LoanExportFormat.FNMA32);
    /// 
    ///          // Read the file in and display it
    ///          using (StreamReader reader = new StreamReader(exportPath))
    ///          Console.Write(reader.ReadToEnd());
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void Export(string filePath, string exportKey, LoanExportFormat format)
    {
      string str = this.ExportAsText(exportKey, format);
      StreamWriter streamWriter = new StreamWriter(filePath ?? "", false, Encoding.Default);
      streamWriter.Write(str);
      streamWriter.Close();
    }

    /// <summary>
    /// Exports the loan information to one of serveral supported formats.
    /// </summary>
    /// <param name="format">The target format to which the loan data will
    /// be converted.</param>
    /// <param name="exportKey">Your Encompass API Export Key (see Remarks below for more information).</param>
    /// <remarks>
    /// <p>Exporting loans using the API requires the use of an Encompass API Export Key.
    /// This key is the same as the CD Key used to register the Encompass API SDK/Runtime.</p>
    /// <p>If you are a licensed Encompass Partner and your application is being distributed to Encompass Customers,
    /// you should use your own CD Key when calling this method, not the CD Key of of the individual
    /// customers who use your software. In general, simply hard-coding this key to your Encompass API
    /// CD Key is the preferred technique.</p>
    /// <p>For customer doing their own in-house development, simply use your own Encompass API Key
    /// for this purpose.</p>
    /// <p>Before Encompass will perform the export, it must first validate your Export Key by communicating
    /// with the Encompass Licensing Servers. To do this, your application must be running on a computer
    /// which has Internet access and which permits anonymous requests to the host <c>encompass.elliemae.com</c>.
    /// If you are running behind a proxy which requires authentication for outbound connections, an exception
    /// will need to be made for this host in order to export data with this method.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to export the data of an existing loan
    ///       to Fannie Mae 3.2 format.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          string exportPath = "C:\\Export\\" + loan.LoanName;
    /// 
    ///          // Export the loan to Fannie Mae 3.2 format. You would need to provide
    ///          // your own Export Key to perform the export.
    ///          loan.Export(exportPath, "AAAAAAAAAA", LoanExportFormat.FNMA32);
    /// 
    ///          // Read the file in and display it
    ///          using (StreamReader reader = new StreamReader(exportPath))
    ///          Console.Write(reader.ReadToEnd());
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public string ExportAsText(string exportKey, LoanExportFormat format)
    {
      return this.ExportAsText(exportKey, format, false);
    }

    /// <summary>
    /// Exports the loan information to one of serveral supported formats.
    /// </summary>
    /// <param name="exportKey">Your Encompass API Export Key (see Remarks below for more information).</param>
    /// <param name="format">The target format to which the loan data will
    /// be converted.</param>
    /// <param name="currentBorPairOnly">Indicates whether only the "current" borrower pair
    /// should be exported.</param>
    /// <remarks>
    /// <p>Exporting loans using the API requires the use of an Encompass API Export Key.
    /// This key is the same as the CD Key used to register the Encompass API SDK/Runtime.</p>
    /// <p>If you are a licensed Encompass Partner and your application is being distributed to Encompass Customers,
    /// you should use your own CD Key when calling this method, not the CD Key of of the individual
    /// customers who use your software. In general, simply hard-coding this key to your Encompass API
    /// CD Key is the preferred technique.</p>
    /// <p>For customer doing their own in-house development, simply use your own Encompass API Key
    /// for this purpose.</p>
    /// <p>Before Encompass will perform the export, it must first validate your Export Key by communicating
    /// with the Encompass Licensing Servers. To do this, your application must be running on a computer
    /// which has Internet access and which permits anonymous requests to the host <c>encompass.elliemae.com</c>.
    /// If you are running behind a proxy which requires authentication for outbound connections, an exception
    /// will need to be made for this host in order to export data with this method.</p>
    /// <p>Not all export formats support the <c>currentBorPairOnly</c> option. If you pass a value of
    /// <c>true</c> but this option is not supported by your selected export format, a
    /// <c>NotSupportedException</c> will be raised. Pass a <c>false</c> value to ensure the export will
    /// be successful.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to export the data of an existing loan
    ///       to Fannie Mae 3.2 format.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          string exportPath = "C:\\Export\\" + loan.LoanName;
    /// 
    ///          // Export the loan to Fannie Mae 3.2 format. You would need to provide
    ///          // your own Export Key to perform the export.
    ///          loan.Export(exportPath, "AAAAAAAAAA", LoanExportFormat.FNMA32);
    /// 
    ///          // Read the file in and display it
    ///          using (StreamReader reader = new StreamReader(exportPath))
    ///          Console.Write(reader.ReadToEnd());
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public string ExportAsText(string exportKey, LoanExportFormat format, bool currentBorPairOnly)
    {
      this.ensureOpen();
      this.validateExportKey(exportKey, format);
      lock (typeof (Loan))
        Loan.ensureExportAssemblyExists(format);
      return new ExportData(this.dataMgr, this.dataMgr.LoanData).Export(format.ToString(), currentBorPairOnly);
    }

    /// <summary>
    /// Imports loan data from a disk file into the current loan.
    /// </summary>
    /// <param name="filePath">The path to the file from which data will be imported.</param>
    /// <param name="format">The format of the data within the import file.</param>
    /// <remarks>This method imports data into the current loan without changing the
    /// identifying information of this loan (i.e. GUID and Loan Number).
    /// If you wish for the identifying information from the imported loan to be
    /// preserved, use the <c>ImportLoan</c>
    /// method on the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loans">Loans</see> object.</remarks>
    /// <example>
    ///       The following code demonstrates how to import Fannie Mae 3.x data into
    ///       an existing Loan.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          // Lock the loan since we will be modifying its contents
    ///          loan.Lock();
    /// 
    ///          // Import the loan from Fannie Mae 3.x format
    ///          string importPath = "C:\\Import\\" + loan.LoanName;
    /// 
    ///          loan.Import(importPath, LoanImportFormat.FNMA3X);
    /// 
    ///          // Commit the changes and unlock the loan
    ///          loan.Commit();
    ///          loan.Unlock();
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    ///              ]]>
    ///       </code>
    ///     </example>
    public void Import(string filePath, LoanImportFormat format)
    {
      this.ImportWithTemplate(filePath, format, (EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate) null);
    }

    /// <summary>
    /// Imports loan data from a disk file into the current loan using a specified template.
    /// </summary>
    /// <param name="filePath">The path to the file from which data will be imported.</param>
    /// <param name="format">The format of the data within the import file.</param>
    /// <param name="template">The template to use for the import.</param>
    /// <remarks>This method imports data into the current loan without changing the
    /// identifying information of this loan (i.e. GUID and Loan Number).
    /// If you wish for the identifying information from the imported loan to be
    /// preserved, use the <c>ImportLoan</c>
    /// method on the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loans">Loans</see> object.</remarks>
    public void ImportWithTemplate(string filePath, LoanImportFormat format, EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate template)
    {
      this.ImportWithLoanOfficer(filePath, format, template, (User) null);
    }

    /// <summary>
    /// Imports loan data from a disk file into the current loan using a specified template.
    /// Assigns the specified user as the loan officer when there is no match between the
    /// loan officer name in the loan data and loan officer names in the organization.
    /// </summary>
    /// <param name="filePath">The path to the file from which data will be imported.</param>
    /// <param name="format">The format of the data within the import file.</param>
    /// <param name="template">The template to use for the import.</param>
    /// <param name="user">The user to assign as the loan officer.</param>
    /// <remarks>This method imports data into the current loan without changing the
    /// identifying information of this loan (i.e. GUID and Loan Number).
    /// If you wish for the identifying information from the imported loan to be
    /// preserved, use the <c>ImportLoan</c>
    /// method on the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loans">Loans</see> object.</remarks>
    public void ImportWithLoanOfficer(
      string filePath,
      LoanImportFormat format,
      EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate template,
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

    /// <summary>
    /// Imports loan data from a byte array into the current loan.
    /// </summary>
    /// <param name="importData">A byte array containing the data to be imported. This parameter
    /// is passed by reference solely for compatibility with Visual Basic 6.0 clients.
    /// The array passed to this function will not be modified.</param>
    /// <param name="format">The format of the data within the import file.</param>
    /// <remarks>This method imports data into the current loan without changing the
    /// identifying information of this loan (i.e. GUID and Loan Number).
    /// If you wish for the identifying information from the imported loan to be
    /// preserved, use the <c>ImportLoan</c>
    /// method on the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loans">Loans</see> object.</remarks>
    /// <remarks><note type="implementnotes">Because of language restrictions, this method
    /// cannot be used from using a weakly-typed language such as VBScript or
    /// JScript. Use the Import method instead.</note></remarks>
    public void ImportFromBytes(ref byte[] importData, LoanImportFormat format)
    {
      lock (this)
      {
        this.ensureExists();
        using (LoanDataMgr loanDataMgr = Loan.Import(importData, format, (EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate) null, this.Session, string.Empty))
          this.Attach(loanDataMgr.LoanData);
      }
    }

    /// <summary>
    /// Gets a flag indicating if the loan has yet to be saved to the server.
    /// </summary>
    /// <remarks>A new loan will not be saved to the server until Commit is called.</remarks>
    /// <example>
    ///       The following code demonstrates how to use the IsNew property.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Create the empty shell for the new loan.
    ///          Loan loan = session.Loans.CreateNew();
    /// 
    ///          // Print the value of the IsNew flag
    ///          Console.WriteLine("IsNew = " + loan.IsNew.ToString());
    /// 
    ///          // Set the loan folder and name at a minimum
    ///          loan.LoanFolder = "My Pipeline";
    ///          loan.LoanName = "IsNewExample";
    /// 
    ///          // Now commit the loan to the server
    ///          loan.Commit();
    /// 
    ///          // Print the value of the IsNew flag
    ///          Console.WriteLine("IsNew = " + loan.IsNew.ToString());
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    ///              ]]>
    ///       </code>
    ///     </example>
    public bool IsNew
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.IsNew();
      }
    }

    /// <summary>
    /// Indicates if the loan has been modified since it was opened or last saved.
    /// </summary>
    public bool Modified
    {
      get
      {
        this.ensureOpen();
        return this.dataMgr.LoanData.Dirty;
      }
    }

    /// <summary>Saves any pending changes to the loan to the server.</summary>
    /// <remarks>This method is used either to update an existing loan
    /// or to save a new loan to the server. In the latter case, the loan's
    /// Guid property will be populated once the loan has been saved.</remarks>
    /// <example>
    ///       The following code demonstrates how to safely lock and unlock a loan in order
    ///       to make changes to its data.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          // Lock the loan
    ///          loan.Lock();
    /// 
    ///          // Modify some of the loan data
    ///          loan.Fields["11"].Value = "3094 Underwood Lane";    // Property Address
    ///          loan.Fields["12"].Value = "Westchester";            // Property City
    ///          loan.Fields["14"].Value = "PA";                     // Property State
    /// 
    ///          // Commit the changes
    ///          loan.Commit();
    /// 
    ///          // Unlock the loan, allowing other clients to obtain a lock
    ///          loan.Unlock();
    /// 
    ///          // We can still safely read data from the loan
    ///          Console.WriteLine(loan.Fields["1335"].Value);       // Down Payment
    ///          Console.WriteLine(loan.Fields["LOANSUB.X3"].Value); // Appraisal Fee
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    ///         ]]>
    ///       </code>
    ///     </example>
    public void Commit()
    {
      this.ensureLocked();
      using (ClientMetricsProviderFactory.GetIncrementalTimer("LoanSaveIncTimer", (SFxTag) new SFxSdkTag()))
        this.dataMgr.SaveLoan(false, false, true, (ILoanMilestoneTemplateOrchestrator) null, false, out bool _);
    }

    /// <summary>
    /// Refreshes the loan from the server. Any changes made by other users will becomes
    /// visible after the refresh. Any pending changes will be lost.
    /// </summary>
    /// <example>
    ///       The following code demonstrates how to use the Refresh method to reload the
    ///       loan's data from the server.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          // Lock the loan since we will be modifying its contents
    ///          loan.Lock();
    /// 
    ///          // Set the property address fields an dcommit those changes
    ///          loan.Fields["11"].Value = "202 Howard St., SW";
    ///          loan.Fields["12"].Value = "Baltimore";
    ///          loan.Fields["14"].Value = "MD";
    ///          loan.Commit();
    /// 
    ///          // Set new address fields, but then call Refresh instead of Commit
    ///          loan.Fields["11"].Value = "202 Nowhere Street";
    ///          loan.Fields["12"].Value = "Noplace";
    ///          loan.Fields["14"].Value = "XX";
    ///          loan.Refresh();
    /// 
    ///          // Dump the values of the field. The values printed will reflect those
    ///          // we saved when we commited to loan to the server.
    ///          Console.WriteLine(loan.Fields["11"].Value);
    ///          Console.WriteLine(loan.Fields["12"].Value);
    ///          Console.WriteLine(loan.Fields["14"].Value);
    /// 
    ///          // Unlock the loan
    ///          loan.Unlock();
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    ///               ]]>
    ///       </code>
    ///     </example>
    public void Refresh()
    {
      this.ensureExists();
      this.dataMgr.Refresh(false);
      this.refreshInternal();
    }

    /// <summary>Closes the loan object, releasing its resources.</summary>
    /// <remarks>If the loan had been previously locked, it will be unlocked
    /// when closed. Any pending changes to the loan will not be saved.
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to use the Close method to release the
    ///       resources held by a loan and discard any changes made to it.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          // Lock the loan since we will be modifying its contents
    ///          loan.Lock();
    /// 
    ///          // Set the property address fields an dcommit those changes
    ///          loan.Fields["11"].Value = "202 Howard St., SW";
    ///          loan.Fields["12"].Value = "Baltimore";
    ///          loan.Fields["14"].Value = "MD";
    /// 
    ///          // Close the loan. This will discard any changes that have not
    ///          // been committed and will unlock the loan if Lock() was
    ///          // previously called.
    ///          loan.Close();
    /// 
    ///          // The following line will cause an exception because the loan's
    ///          // resources have been released.
    ///          try
    ///          {
    ///             Console.WriteLine(loan.Fields["11"].Value);
    ///          }
    ///          catch (Exception ex)
    ///          {
    ///             Console.WriteLine("Error accessing loan: " + ex.Message);
    ///          }
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    ///               ]]>
    ///       </code>
    ///     </example>
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

    /// <summary>
    /// Moves an existing loan to a new loan folder and/or assigns it a new name.
    /// </summary>
    /// <param name="newFolder">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanFolder" />
    /// to which to move the loan.</param>
    /// <param name="newLoanName">The new name for the loan.</param>
    /// <remarks>This method allows existing loans to be moved between loan folders
    /// or to be renamed within the current loan folder. To move a loan without changing
    /// its name, pass the loan's current <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.LoanName" /> for the second
    /// parameter. To change the loan's name but keep it in the current folder,
    /// pass the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanFolder" /> object
    /// corresponding to the loan's current <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.LoanFolder" /> property value.
    /// <p>If a loan with the name provided already exists in the specified loan folder,
    /// an exception will be raised. To have a new name assigned automatically, use the
    /// <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.MoveToFolder(EllieMae.Encompass.BusinessObjects.Loans.LoanFolder)" /> method instead.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to move an existing loan from one
    ///       loan folder to another.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open a loan from the My Pipeline folder
    ///          Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    /// 
    ///          // Move the loan to the (Archive) folder without changing its name
    ///          loan.Move(session.Loans.Folders["(Archive)"], loan.LoanName);
    /// 
    ///          // Display the loan's new loan folder information
    ///          Console.WriteLine(loan.LoanFolder);
    /// 
    ///          // Close the loan and end the session
    ///          loan.Close();
    ///          session.End();
    ///    }
    /// }
    ///               ]]>
    ///       </code>
    ///     </example>
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
      this.dataMgr.Move(newFolder.Name, newLoanName, DuplicateLoanAction.Fail);
      if (!flag)
        return;
      this.Unlock();
    }

    /// <summary>Moves the loan to the specified loan folder.</summary>
    /// <param name="newFolder">The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.LoanFolder" /> to which the loan will be moved.</param>
    /// <remarks>Calling this method may result in the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.LoanName" /> of the loan changing
    /// since a loan's LoanName is unique within its folder. If a loan with the same LoanName already
    /// exists in the target folder, a new loan name will be generated automatically and assigned
    /// to the loan. You can determine the new loan name by reading the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Loan.LoanName" />
    /// property after calling this method.
    /// <p>To specify the a new loan name for the loan, use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Move(EllieMae.Encompass.BusinessObjects.Loans.LoanFolder,System.String)" /> method instead.</p>
    /// </remarks>
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
      this.dataMgr.Move(newFolder.Name, "", DuplicateLoanAction.Rename);
      if (!flag)
        return;
      this.Unlock();
    }

    /// <summary>
    /// Closes the loan and releases any resources held by it.
    /// </summary>
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

    /// <summary>Deletes the current loan from the server.</summary>
    /// <remarks>This operation is irrevocable so the loan, once deleted, cannot be recovered.
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to delete an existing loan.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open an existing loan using the GUID from the command line
    ///          Loan loan = session.Loans.Open(args[0]);
    /// 
    ///          try
    ///          {
    ///             if (loan != null)
    ///               loan.Delete();
    ///          }
    ///          catch (Exception ex)
    ///          {
    ///             // Loan deletion could fail if the loan is locked by another user
    ///             Console.WriteLine("Unable to delete loan due to error: " + ex.Message);
    ///          }
    /// 
    ///          // End the session to gracefully disconnect from the server
    ///          session.End();
    ///    }
    /// }
    ///              ]]>
    ///       </code>
    ///     </example>
    public void Delete()
    {
      this.ensureOpen();
      this.dataMgr.Delete();
    }

    /// <summary>Retrieves a custom object for a loan.</summary>
    /// <param name="name">The name of the object to retrieve.</param>
    /// <returns>Returns the specified custom object.</returns>
    public DataObject GetCustomDataObject(string name)
    {
      if ((name ?? "") == "")
        throw new ArgumentException(nameof (name), "Object name cannot be blank or null");
      BinaryObject supportingData = this.dataMgr.GetSupportingData(this.customDataPrefix + name);
      return supportingData == null ? (DataObject) null : new DataObject(supportingData);
    }

    /// <summary>Saves a custom data object for the loan.</summary>
    /// <param name="name">The name to assign to the Data Object</param>
    /// <param name="data">The object to be written</param>
    /// <remarks></remarks>
    public void SaveCustomDataObject(string name, DataObject data)
    {
      this.ensureLocked();
      if ((name ?? "") == "")
        throw new ArgumentException(nameof (name), "Object name cannot be blank or null");
      this.dataMgr.SaveSupportingData(this.customDataPrefix + name, data == null ? (BinaryObject) null : data.Unwrap());
    }

    /// <summary>
    /// Appends data to an existing custom data object or creates a new one.
    /// </summary>
    /// <param name="name">The name to of the data object</param>
    /// <param name="data">The data to be appended</param>
    /// <remarks>If the specified data object does not exist, an empty object will first be created
    /// and then the specified data appended to it.</remarks>
    public void AppendToCustomDataObject(string name, DataObject data)
    {
      this.ensureLocked();
      if ((name ?? "") == "")
        throw new ArgumentException(nameof (name), "Object name cannot be blank or null");
      if (data == null)
        throw new ArgumentNullException(nameof (data), "Data object cannot be null");
      this.dataMgr.AppendSupportingData(this.customDataPrefix + name, data.Unwrap());
    }

    /// <summary>
    /// Deletes a custom data object associated with the loan.
    /// </summary>
    /// <param name="name">The name of the data object</param>
    public void DeleteCustomDataObject(string name)
    {
      this.SaveCustomDataObject(this.customDataPrefix + name, (DataObject) null);
    }

    /// <summary>
    /// Retrieves an object associated with an ePASS Transaction.
    /// </summary>
    /// <param name="name">The name of the object to retrieve.</param>
    /// <returns>Returns the specified custom object.</returns>
    /// <remarks>This method is intended for internal use only by the Encompass application.
    /// To retrieve a custom data object stored using the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.SaveCustomDataObject(System.String,EllieMae.Encompass.BusinessObjects.DataObject)" /> method,
    /// use <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.GetCustomDataObject(System.String)" />.</remarks>
    public DataObject GetEPassTransactionDataObject(string name)
    {
      BinaryObject data = !((name ?? "") == "") ? this.dataMgr.GetSupportingData(name) : throw new ArgumentException(nameof (name), "Object name cannot be blank or null");
      return data == null ? (DataObject) null : new DataObject(data);
    }

    /// <summary>Saves an object associated with an ePASS transaction.</summary>
    /// <param name="name">The name to assign to the Data Object</param>
    /// <param name="data">The object to be written</param>
    /// <remarks>This method is intended for internal use only by the Encompass application. Use of this
    /// method by other applications can cause data corruption or loss of data for the current loan.
    /// To save a custom data object in a safe manner, you should use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.SaveCustomDataObject(System.String,EllieMae.Encompass.BusinessObjects.DataObject)" />
    /// method. Objects saved in that manner can be retrieved using the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.GetCustomDataObject(System.String)" />
    /// method.
    /// </remarks>
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

    /// <summary>
    /// Returns the effective access rights the currently logged in user
    /// has to the current loan.
    /// </summary>
    /// <returns>The rights for the current user.</returns>
    /// <remarks>For information about effective versus assigned access rights, see
    /// the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.GetEffectiveAccessRights(EllieMae.Encompass.BusinessObjects.Users.User)" /> method.</remarks>
    /// <example>
    ///       The following code iterates throught each loan in the "My Pipeline" folder,
    ///       verifies that the user has at least Read/Write access and, if so, adds a new
    ///       GeneralEntry to the loan's log.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Query for all loans in the "My Pipeline" folder
    ///          LoanIdentityList ids = session.Loans.Folders["My Pipeline"].GetContents();
    /// 
    ///          foreach (LoanIdentity id in ids)
    ///          {
    ///             // Open the loan with the specified GUID and check if the current user has
    ///             // write access. If so, create a new General log entry.
    ///             Loan loan = session.Loans.Open(id.Guid);
    /// 
    ///             // If the user has at least read/write rights, modify the loan
    ///             if (loan.GetAccessRights() >= LoanAccessRights.ReadWrite)
    ///             {
    ///                 loan.Lock();
    ///                 loan.Fields["4000"].Value = "John";
    ///                 loan.Fields["4002"].Value = "Doe";
    ///                 loan.Commit();
    ///                 loan.Unlock();
    ///                 loan.Close();
    ///             }
    ///          }
    /// 
    ///          // Close the loan and end the session
    ///          session.End();
    ///    }
    /// }
    ///              ]]>
    ///       </code>
    ///     </example>
    public LoanAccessRights GetAccessRights()
    {
      this.ensureOpen();
      switch (this.dataMgr.GetEffectiveRight(this.Session.UserID))
      {
        case LoanInfo.Right.NoRight:
          return LoanAccessRights.None;
        case LoanInfo.Right.Access:
          return LoanAccessRights.ReadWrite;
        case LoanInfo.Right.FullRight:
          return LoanAccessRights.Full;
        case LoanInfo.Right.Read:
          return LoanAccessRights.ReadOnly;
        default:
          throw new Exception("Invalid value returned for loan rights");
      }
    }

    /// <summary>
    /// Returns the access rights the specified user has to the current loan.
    /// </summary>
    /// <param name="user">The user for which to query the rights.</param>
    /// <returns>The rights for the specified user.</returns>
    /// <remarks>This method may only be called by a user with Full rights on the current
    /// loan. Additionally, it may only be called on a commited loan.</remarks>
    /// <example>
    ///       The following code demonstrates how to display the assigned and effective rights
    ///       of every named user to a particular loan.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open a loan from the My Pipeline folder
    ///          Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    /// 
    ///          // Iterate over all users in the system, printing their assigned and effective rights
    ///          foreach (User user in session.Users.GetAllUsers())
    ///             Console.WriteLine(user.ID + ": " + loan.GetAssignedAccessRights(user) + " "
    ///             + loan.GetEffectiveAccessRights(user));
    /// 
    ///          // Close the loan and end the session
    ///          loan.Close();
    ///          session.End();
    ///    }
    /// }
    ///              ]]>
    ///       </code>
    ///     </example>
    public LoanAccessRights GetAssignedAccessRights(User user)
    {
      this.ensureFullRights();
      this.ensureExists();
      switch (this.dataMgr.GetRight(user.ID))
      {
        case LoanInfo.Right.NoRight:
          return LoanAccessRights.None;
        case LoanInfo.Right.Access:
          return LoanAccessRights.ReadWrite;
        case LoanInfo.Right.FullRight:
          return LoanAccessRights.Full;
        default:
          throw new Exception("Invalid value returned for loan rights");
      }
    }

    /// <summary>
    /// Returns the effective rights the specified user has to the current loan.
    /// </summary>
    /// <param name="user">The user for which to query the rights.</param>
    /// <returns>The user effective rights for the specified user.</returns>
    /// <remarks>A user's effective rights are determined by a combination of their
    /// assigned rights, their persona and their position within the organization
    /// hierarchy as follows:
    /// <list type="bullet">
    /// <item>An administrator at the top level of the organization hierarchy will
    /// always have <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.LoanAccessRights.Full" /> rights to all loans.</item>
    /// <item>An administrator at any other level of the organization hierarchy will
    /// have rights equal to the greatest of their assigned rights and the assigned rights
    /// of all users user below them in the hierarchy.</item>
    /// <item>A non-administrative user's effective rights will always be equal to their
    /// assigned rights, if any rights are assigned.</item>
    /// <item>A non-administrative user with no assigned rights will have either read or read/write
    /// rights to the loan (based on their user profile) if at least one subordinate has read
    /// access to the loan.
    /// </item>
    /// </list>
    /// <p>This method may only be called by a user with Full rights on the current
    /// loan. Additionally, it may only be called on a commited loan.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to display the assigned and effective rights
    ///       of every named user to a particular loan.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open a loan from the My Pipeline folder
    ///          Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    /// 
    ///          // Iterate over all users in the system, printing their assigned and effective rights
    ///          foreach (User user in session.Users.GetAllUsers())
    ///             Console.WriteLine(user.ID + ": " + loan.GetAssignedAccessRights(user) + " "
    ///             + loan.GetEffectiveAccessRights(user));
    /// 
    ///          // Close the loan and end the session
    ///          loan.Close();
    ///          session.End();
    ///    }
    /// }
    ///              ]]>
    ///       </code>
    ///     </example>
    public LoanAccessRights GetEffectiveAccessRights(User user)
    {
      this.ensureFullRights();
      this.ensureExists();
      switch (this.dataMgr.GetEffectiveRight(user.ID))
      {
        case LoanInfo.Right.NoRight:
          return LoanAccessRights.None;
        case LoanInfo.Right.Access:
          return LoanAccessRights.ReadWrite;
        case LoanInfo.Right.FullRight:
          return LoanAccessRights.Full;
        case LoanInfo.Right.Read:
          return LoanAccessRights.ReadOnly;
        default:
          throw new Exception("Invalid value returned for loan rights");
      }
    }

    /// <summary>
    /// Generates a list of all users who have assigned rights for the loan.
    /// </summary>
    /// <returns>A <see cref="T:EllieMae.Encompass.Collections.UserList" /> containing all Users that
    /// have assigned rights for the loan.</returns>
    /// <remarks>This method may only be called by a user with Full rights on the current
    /// loan. Additionally, it may only be called on a commited loan.</remarks>
    /// <example>
    ///       The following code demonstrates how to revoke the assigned rights of every
    ///       user that has assigned rights for a particular loan.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open a loan from the My Pipeline folder
    ///          Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    /// 
    ///          // Iterate over each user with assigned rights and revoke them
    ///          foreach (User user in loan.GetUsersWithAssignedRights())
    ///             loan.AssignRights(user, LoanAccessRights.None);
    /// 
    ///          // Close the loan and end the session
    ///          loan.Close();
    ///          session.End();
    ///    }
    /// }
    ///               ]]>
    ///       </code>
    ///     </example>
    public UserList GetUsersWithAssignedRights()
    {
      this.ensureFullRights();
      this.ensureExists();
      UserList withAssignedRights = new UserList();
      foreach (DictionaryEntry right in this.dataMgr.GetRights())
      {
        if ((LoanInfo.Right) right.Value != LoanInfo.Right.NoRight)
        {
          User user = this.Session.Users.GetUser(right.Key.ToString());
          if (user != null)
            withAssignedRights.Add(user);
        }
      }
      return withAssignedRights;
    }

    /// <summary>
    /// Modifies the assigned rights of a user to the current loan.
    /// </summary>
    /// <param name="user">The user for whom the rights will be assigned.</param>
    /// <param name="rights">The rights to assign to this user.</param>
    /// <remarks>This function replaces the previous rights of the specified user,
    /// if any, with the new rights provided. To revoke all rights from the user,
    /// use <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.LoanAccessRights.None" />.
    /// <p>Changes made with this method take affect immediately and do not require you
    /// to invoke the Commit() method on the loan. Additionally, the loan does not
    /// need to be locked to invoke this method.</p>
    /// <p>This method may only be called by a user with Full rights on the current
    /// loan. Additionally, it may only be called on a commited loan.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to revoke the assigned rights of every
    ///       user that has assigned rights for a particular loan.
    ///       <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///          // Open the session to the remote server
    ///          Session session = new Session();
    ///          session.Start("myserver", "mary", "maryspwd");
    /// 
    ///          // Open a loan from the My Pipeline folder
    ///          Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    /// 
    ///          // Iterate over each user with assigned rights and revoke them
    ///          foreach (User user in loan.GetUsersWithAssignedRights())
    ///             loan.AssignRights(user, LoanAccessRights.None);
    /// 
    ///          // Close the loan and end the session
    ///          loan.Close();
    ///          session.End();
    ///    }
    /// }
    ///               ]]>
    ///       </code>
    ///     </example>
    public void AssignRights(User user, LoanAccessRights rights)
    {
      this.ensureFullRights();
      this.ensureExists();
      LoanInfo.Right right;
      switch (rights)
      {
        case LoanAccessRights.None:
          right = LoanInfo.Right.NoRight;
          break;
        case LoanAccessRights.ReadWrite:
          right = LoanInfo.Right.Access;
          break;
        case LoanAccessRights.Full:
          right = LoanInfo.Right.FullRight;
          break;
        default:
          throw new ArgumentException("Invalid access right specified");
      }
      this.dataMgr.SetRight(user.ID, right);
    }

    /// <summary>Applies a template to the current loan.</summary>
    /// <param name="tmpl">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.Template" /> to be applied.</param>
    /// <param name="appendData">Indicates if the data in the template should be appended to the
    /// existing data in the loan or overwrite the data in the loan.</param>
    /// <remarks>If the <c>appendData</c> parameter is <c>true</c>, only non-empty values from the
    /// template will be applied to the loan. Thus, if a field is populated in the loan and unpopulated
    /// in the template, the field's value will be preserved. If the parameter is <c>false</c>,
    /// all fields from the template, including blank fields values, will overwrite the data in the loan.
    /// </remarks>
    /// <example>
    ///       The following code applies a loan template to an existing loan file,
    ///       appending the templates data to the loan to ensure only non-empty template
    ///       data is applied.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Templates;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan from the My Pipeline folder and retrieve the information on the current lock
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    ///       loan.Lock();
    /// 
    ///       // Retrieve the desired loan template
    ///       LoanTemplate template = (LoanTemplate)session.Loans.Templates.GetTemplate(TemplateType.LoanTemplate,
    ///         @"public:\Companywide\Example Purchase Loan Template");
    /// 
    ///       // Apply the template to the loan in "append" mode. This will ensure that blank values
    ///       // from the template do not overwrite data in the loan file.
    ///       loan.ApplyTemplate(template, true);
    /// 
    ///       // Save and close the loan file
    ///       loan.Commit();
    ///       loan.Close();
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void ApplyTemplate(Template tmpl, bool appendData)
    {
      if (tmpl == null)
        throw new ArgumentNullException("Specified template must be non-null.");
      switch (tmpl.TemplateType)
      {
        case EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateType.LoanProgram:
          this.dataMgr.ApplyLoanProgram(tmpl.FileSystemEntry, (EllieMae.EMLite.DataEngine.LoanProgram) tmpl.Unwrap(), appendData);
          break;
        case EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateType.ClosingCost:
          this.dataMgr.ApplyClosingCost(tmpl.FileSystemEntry, (EllieMae.EMLite.DataEngine.ClosingCost) tmpl.Unwrap(), appendData);
          break;
        case EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateType.DataTemplate:
          this.dataMgr.ApplyDataTemplate(tmpl.FileSystemEntry, (EllieMae.EMLite.DataEngine.DataTemplate) tmpl.Unwrap(), appendData);
          break;
        case EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateType.InputFormSet:
          this.dataMgr.ApplyInputFormSet(tmpl.FileSystemEntry, (FormTemplate) tmpl.Unwrap());
          break;
        case EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateType.DocumentSet:
          this.dataMgr.ApplyDocumentSet(tmpl.FileSystemEntry, (DocumentSetTemplate) tmpl.Unwrap());
          break;
        case EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateType.LoanTemplate:
          this.dataMgr.ApplyLoanTemplate(tmpl.FileSystemEntry, appendData, false, false);
          break;
        case EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateType.TaskSet:
          this.dataMgr.ApplyTaskSet(tmpl.FileSystemEntry, (TaskSetTemplate) tmpl.Unwrap());
          break;
        default:
          throw new Exception("The specified template type is not supported");
      }
    }

    /// <summary>
    /// Sets a flag indicating if the specified field should be overridden with the loan
    /// field value when orderino Closing Documents thru the Ellie Mae Docs Service.
    /// </summary>
    /// <param name="fieldId">The ID of the field to be overridden.</param>
    /// <param name="ovrrde">Indicates the Doc Service setting should be overriden
    /// by the loan's data if <c>true</c>, otherwise the Doc Service value will be used.</param>
    /// <remarks>
    /// <p>The Ellie Mae Closing Doc Service provides customer configuration that is applied to
    /// all loans that are run thru the system. It is possible to override the values of certain
    /// fields on a per-loan basis by setting the value of the loan field and enabling the override
    /// flag for the field.</p>
    /// <p>The only fields that are appropriate to be used for this call are those that start with
    /// the "Closing" prefix, e.g. "Closing.LndNm". The fields map to settings within the Doc
    /// Service that are configured with the customer's data.</p>
    /// <p>Note that if you set the value of a "Closing" field to a non-empty value, you do
    /// not need to explicitly set the override for this field. A non-empty value will always be
    /// treated as an override when closing docs are generated. Use this method only to override
    /// Doc Service field values with a blank value.</p>
    /// <p>Additionally, if this method is called and the <c>ovrrde</c> parameter is <c>false</c>,
    /// any value stored in the field will be wiped out.</p>
    /// </remarks>
    public void SetClosingDocumentFieldOverride(string fieldId, bool ovrrde)
    {
      if (!fieldId.StartsWith("Closing.", StringComparison.CurrentCultureIgnoreCase))
        throw new ArgumentException("The specified field is invalid in this context. Only fields that begin with the 'Closing' prefix are valid.");
      if (!ovrrde)
        this.Fields[fieldId].Value = (object) "";
      this.dataMgr.LoanData.SetDocFieldUserOverride(fieldId, ovrrde);
    }

    /// <summary>
    /// Indicates if a value from the loan will override the Ellie Mae Doc Service setting
    /// for the specified field.
    /// </summary>
    /// <param name="fieldId">The Field ID to be checked. This field ID must start with the
    /// "Closing" prefix.</param>
    /// <returns>Returns <c>true</c> if the field either contains a non-empty value or has been
    /// overridden explicitly using the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.SetClosingDocumentFieldOverride(System.String,System.Boolean)" /> method;
    /// otherwise, it returns <c>false</c>.</returns>
    public bool IsClosingDocumentFieldOverridden(string fieldId)
    {
      if (!fieldId.StartsWith("Closing.", StringComparison.CurrentCultureIgnoreCase))
        throw new ArgumentException("The specified field is invalid in this context. Only fields that begin with the 'Closing' prefix are valid.");
      return this.dataMgr.LoanData.IsDocFieldOverridden(fieldId);
    }

    /// <summary>
    /// Provides an API to get Funding Fees from Funding Worksheet.
    /// </summary>
    /// <returns>A List with FundingFee class.</returns>
    public FundingFeeList GetFundingFees(bool hideZero)
    {
      List<EllieMae.EMLite.DataEngine.FundingFee> fundingFees = this.dataMgr.LoanData.Calculator.GetFundingFees(hideZero);
      if (fundingFees == null)
        return new FundingFeeList();
      List<FundingFee> source = new List<FundingFee>();
      for (int index = 0; index < fundingFees.Count; ++index)
        source.Add(new FundingFee(fundingFees[index]));
      return new FundingFeeList((IList) source);
    }

    /// <summary>
    /// Provides a hash code implementation for the Loan object.
    /// </summary>
    /// <returns>A hash code usable in a Hashtable object.</returns>
    public override int GetHashCode() => this.Guid.GetHashCode();

    /// <summary>
    /// Determines if two Loan objects represent the same persistent loan.
    /// </summary>
    /// <param name="obj">The loan to which to compare this object.</param>
    /// <returns>Returns <c>true</c> if the two objects represent the same
    /// loan and come from the same <see cref="T:EllieMae.Encompass.Client.Session">Session</see>,
    /// <c>false</c> otherwise.</returns>
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
      this.dataMgr.LoanData.BeforeMilestoneCompleted -= new EllieMae.EMLite.DataEngine.CancelableMilestoneEventHandler(this.onBeforeMilestoneCompleted);
      this.dataMgr.LoanData.MilestoneCompleted -= new EllieMae.EMLite.DataEngine.MilestoneEventHandler(this.onMilestoneCompleted);
      this.dataMgr.LoanData.FieldChanged += new FieldChangedEventHandler(this.onFieldChanged);
      this.dataMgr.LoanData.LogRecordAdded += new LogRecordEventHandler(this.onLogRecordAdded);
      this.dataMgr.LoanData.LogRecordRemoved += new LogRecordEventHandler(this.onLogRecordRemoved);
      this.dataMgr.LoanData.LogRecordChanged += new LogRecordEventHandler(this.onLogRecordChanged);
      this.dataMgr.LoanData.BeforeMilestoneCompleted += new EllieMae.EMLite.DataEngine.CancelableMilestoneEventHandler(this.onBeforeMilestoneCompleted);
      this.dataMgr.LoanData.MilestoneCompleted += new EllieMae.EMLite.DataEngine.MilestoneEventHandler(this.onMilestoneCompleted);
    }

    private void disposeLoanDataListeners()
    {
      this.LoanData.FieldChanged -= new FieldChangedEventHandler(this.onFieldChanged);
      this.LoanData.LogRecordAdded -= new LogRecordEventHandler(this.onLogRecordAdded);
      this.LoanData.LogRecordRemoved -= new LogRecordEventHandler(this.onLogRecordRemoved);
      this.LoanData.LogRecordChanged -= new LogRecordEventHandler(this.onLogRecordChanged);
      this.LoanData.BeforeMilestoneCompleted -= new EllieMae.EMLite.DataEngine.CancelableMilestoneEventHandler(this.onBeforeMilestoneCompleted);
      this.LoanData.MilestoneCompleted -= new EllieMae.EMLite.DataEngine.MilestoneEventHandler(this.onMilestoneCompleted);
    }

    private void ensureAttachmentsCompletedInitialization()
    {
      if (this.attachments != null)
        return;
      this.attachments = new LoanAttachments(this);
    }

    /// <summary>Raises the Committed event.</summary>
    protected void OnCommitted()
    {
      this.committed.Invoke((object) this, new PersistentObjectEventArgs(this.Guid));
    }

    /// <summary>Raises the BeforeCommit event.</summary>
    /// <param name="e"></param>
    protected void OnBeforeCommit(CancelableEventArgs e)
    {
      this.beforeCommit.Invoke((object) this, e);
    }

    /// <summary>
    /// Retrieves the plugin extension information for all delegates registered to
    /// the OnBeforeCommit event.
    /// </summary>
    /// <returns></returns>
    public IList<Type> GetOnBeforeCommitExtensions() => this.beforeCommit.GetTargetTypes();

    /// <summary>
    /// Retrieves the plugin extension information for all delegates registered to
    /// the OnCommitted event.
    /// </summary>
    /// <returns></returns>
    public IList<Type> GetOnCommittedExtensions() => this.committed.GetTargetTypes();

    private void onLoanRefreshedFromServer(object source, EventArgs e)
    {
      this.initializeSubobjects();
      this.initializeLoanDataListeners();
      this.loanRefreshedFromServer.Invoke((object) this, new EventArgs());
    }

    private void onBeforeLoanRefreshedFromServer(object source, EventArgs e)
    {
      if (this.beforeLoanRefreshedFromServer != null)
        this.beforeLoanRefreshedFromServer.Invoke((object) this, new EventArgs());
      this.disposeLoanDataListeners();
    }

    private void onFieldChanged(object source, FieldChangedEventArgs e)
    {
      this.fieldChange.Invoke((object) this, new FieldChangeEventArgs(this, e));
    }

    private void onLogRecordRemoved(object source, LogRecordEventArgs e)
    {
      LogEntry logEntry = this.log.Wrap(e.LogRecord);
      if (logEntry == null)
        return;
      this.log.PurgeEntry(logEntry);
      this.logEntryRemoved.Invoke((object) this, new LogEntryEventArgs(this, logEntry));
    }

    private void onLogRecordAdded(object source, LogRecordEventArgs e)
    {
      LogEntry logEntry = this.log.Find(e.LogRecord, true);
      if (logEntry == null)
        return;
      this.logEntryAdded.Invoke((object) this, new LogEntryEventArgs(this, logEntry));
    }

    private void onLogRecordChanged(object source, LogRecordEventArgs e)
    {
      if (this.logEntryChange.IsNull())
        return;
      LogEntry logEntry = this.log.Find(e.LogRecord, false);
      if (logEntry == null)
        return;
      this.logEntryChange.Invoke((object) this, new LogEntryEventArgs(this, logEntry));
    }

    private void onMilestoneCompleted(object source, EllieMae.EMLite.DataEngine.MilestoneEventArgs e)
    {
      if (!(this.log.Wrap((LogRecordBase) e.MilestoneLog) is MilestoneEvent msEvent))
        return;
      this.milestoneCompleted.Invoke((object) this, new MilestoneEventArgs(this, msEvent));
    }

    private void onBeforeMilestoneCompleted(object source, EllieMae.EMLite.DataEngine.CancelableMilestoneEventArgs e)
    {
      if (!(this.log.Wrap((LogRecordBase) e.MilestoneLog) is MilestoneEvent msEvent))
        return;
      CancelableMilestoneEventArgs e1 = new CancelableMilestoneEventArgs(this, msEvent);
      this.beforeMilestoneCompleted.Invoke((object) this, e1);
      e.Cancel = e1.Cancel;
    }

    private void onLoanSaved(object obj, SavingLoanFilesEventArgs e)
    {
      this.currentLock = (LoanLock) null;
      this.OnCommitted();
    }

    private void onBeforeLoanSaved(object sender, EllieMae.EMLite.Common.CancelableEventArgs e)
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
      EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate template,
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
      EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate template,
      Session session,
      string loanOfficerId)
    {
      return Loan.Import(data, format, template, session, loanOfficerId, false, false);
    }

    internal static LoanDataMgr Import(
      byte[] data,
      LoanImportFormat format,
      EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate template,
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
      EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate template,
      Session session,
      string loanOfficerId,
      bool suppressCalcs,
      bool isTPO,
      LoanImportFormat format)
    {
      LoanTemplateSelection loanTemplate = (LoanTemplateSelection) null;
      if (template != null)
        loanTemplate = new LoanTemplateSelection(template.FileSystemEntry);
      IServerManager serverManager = (IServerManager) session.GetObject("ServerManager");
      EnableDisableSetting serverSetting = (EnableDisableSetting) serverManager.GetServerSetting("Import.LoanNumbering");
      ApplicationDateImportSetting dateImportSetting = !isTPO ? (ApplicationDateImportSetting) serverManager.GetServerSetting("Import.EnforceApplicationDate") : (ApplicationDateImportSetting) serverManager.GetServerSetting("Import.TPODateImport");
      return new FannieImport(loanTemplate, format == LoanImportFormat.FNMA34)
      {
        UseEMLoanNumbering = (serverSetting == EnableDisableSetting.Enabled),
        EnforceApplicationDate = (dateImportSetting == ApplicationDateImportSetting.UseCurrentDateIfBlank),
        SuppressCalcs = suppressCalcs,
        TPOImport = isTPO
      }.Convert(importData, session.SessionObjects, loanOfficerId);
    }

    private void ensureFullRights()
    {
      if (this.dataMgr.GetEffectiveRight(this.Session.UserID) != LoanInfo.Right.FullRight)
        throw new InvalidOperationException("Access denied");
    }

    internal IBpmManager WorkflowManager => (IBpmManager) this.Session.GetObject("BpmManager");

    internal LoanDataMgr Unwrap() => this.dataMgr;

    /// <summary>
    /// This method is for internal Encompass use only and should not be called from
    /// your code.
    /// </summary>
    /// <exclude />
    public static Loan Wrap(Session session, LoanDataMgr dataMgr) => new Loan(session, dataMgr);

    /// <summary>Validates underwriting advanced conditions.</summary>
    /// <param name="contactID"></param>
    /// <returns></returns>
    public bool ValidateUnderwritingAdvancedConditions(string contactID)
    {
      return !(contactID == "") ? this.dataMgr.ValidateUnderwritingAdvancedConditions(contactID) : throw new Exception("Contact ID cannot be blank.");
    }

    /// <summary>
    /// Performs export key validation to allow the user to export loans using the API.
    /// </summary>
    private void validateExportKey(string exportKey, LoanExportFormat format)
    {
      if ((exportKey ?? "") == "")
        throw new EllieMae.Encompass.Licensing.LicenseException("A valid Export Key is required to perform this action.");
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
        message = ex.Message.IndexOf("--> ") > 0 ? ex.Message.Substring(ex.Message.IndexOf("--> ") + 4) : throw new EllieMae.Encompass.Licensing.LicenseException(message);
      }
      catch (Exception ex)
      {
        throw new Exception("Error attempting to validate Export Key.", ex);
      }
    }

    /// <summary>
    /// Ensures that the export assembly for the specified format is downloaded and available
    /// </summary>
    private static void ensureExportAssemblyExists(LoanExportFormat format)
    {
      string exportAssemblyPath = ExportData.GetExportAssemblyPath(format.ToString());
      if (exportAssemblyPath == null || System.IO.File.Exists(exportAssemblyPath))
        return;
      string address = "https://www.epassbusinesscenter.com/epassutils/data/" + (Path.GetFileNameWithoutExtension(Path.GetFileName(exportAssemblyPath)) + ".zip");
      try
      {
        byte[] buffer1 = (byte[]) null;
        using (WebClient webClient = new WebClient())
        {
          webClient.Proxy.Credentials = CredentialCache.DefaultCredentials;
          buffer1 = webClient.DownloadData(address);
        }
        byte[] buffer2 = FileCompressor.Instance.UnzipBuffer(buffer1);
        Directory.CreateDirectory(Path.GetDirectoryName(exportAssemblyPath));
        using (FileStream fileStream = System.IO.File.Create(exportAssemblyPath))
          fileStream.Write(buffer2, 0, buffer2.Length);
      }
      catch (Exception ex)
      {
        throw new Exception("The export assembly for format '" + (object) format + "' could not be loaded", ex);
      }
    }

    /// <summary>
    /// Gets UCD XML for Loan estimate .Input parameter should be true to set total fields.
    /// </summary>
    /// <param name="setTotalFields"></param>
    /// <returns></returns>
    public string GetUCDForLoanEstimate(bool setTotalFields)
    {
      return this.LoanData.Calculator.GetUCD(true, setTotalFields);
    }

    /// <summary>
    /// Gets UCD XML for Closing Disclosure .Input parameter should be true to set total fields.
    /// </summary>
    /// <param name="setTotalFields"></param>
    /// <returns></returns>
    public string GetUCDForClosingDisclosure(bool setTotalFields)
    {
      return this.LoanData.Calculator.GetUCD(false, setTotalFields);
    }
  }
}
