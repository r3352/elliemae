// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.ExecutionContext
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public class ExecutionContext : MarshalByRefObject, IExecutionContext, IDisposable, ICloneable
  {
    private UserDataSource currentUser;
    private IServerDataProvider serverDataProvider;
    private LoanDataFieldSource fields;
    private TaskDataSource tasks;
    private DocumentDataSource documents;
    private MilestoneDataSource milestones;
    private LoanActionDataSource loanActions;
    private CalendarDataSource calendar;

    public ExecutionContext(
      UserInfo userInfo,
      LoanData loan,
      IServerDataProvider serverDataProvider)
      : this(userInfo, loan, serverDataProvider, true)
    {
    }

    public ExecutionContext(
      UserInfo userInfo,
      LoanData loan,
      IServerDataProvider serverDataProvider,
      bool readOnly)
    {
      this.currentUser = new UserDataSource(userInfo, loan);
      this.serverDataProvider = serverDataProvider;
      this.fields = new LoanDataFieldSource(loan, userInfo, readOnly);
      this.tasks = new TaskDataSource(loan, userInfo, readOnly);
      this.documents = new DocumentDataSource(loan, userInfo, readOnly);
      this.milestones = new MilestoneDataSource(loan, userInfo, readOnly);
      this.loanActions = new LoanActionDataSource(loan, userInfo, readOnly, serverDataProvider);
      this.calendar = new CalendarDataSource(serverDataProvider);
    }

    public LoanData Loan => this.fields.Loan;

    public UserInfo User => this.currentUser.UserInfo;

    public LoanDataFieldSource FieldSource => this.fields;

    public IServerDataProvider ServerDataProvider => this.serverDataProvider;

    public string UserID => this.currentUser.ID;

    public string UserName => this.currentUser.FullName;

    public IFieldSource Fields => (IFieldSource) this.fields;

    public ITaskDataSource Tasks => (ITaskDataSource) this.tasks;

    public IDocumentDataSource Documents => (IDocumentDataSource) this.documents;

    public IMilestoneDataSource Milestones => (IMilestoneDataSource) this.milestones;

    public ILoanActionDataSource LoanActions => (ILoanActionDataSource) this.loanActions;

    public ICalendarDataSource Calendar => (ICalendarDataSource) this.calendar;

    public DateTime Timestamp => this.serverDataProvider.Timestamp;

    public IUserDataSource CurrentUser => (IUserDataSource) this.currentUser;

    public virtual object Clone()
    {
      return (object) new ExecutionContext(this.currentUser.UserInfo, this.fields.Loan, this.serverDataProvider, this.fields.ReadOnly);
    }

    public virtual void Dispose()
    {
      try
      {
        System.Runtime.Remoting.RemotingServices.Disconnect((MarshalByRefObject) this);
      }
      catch
      {
      }
      this.serverDataProvider = (IServerDataProvider) null;
      if (this.fields != null)
      {
        this.fields.Dispose();
        this.fields = (LoanDataFieldSource) null;
      }
      if (this.tasks != null)
      {
        this.tasks.Dispose();
        this.tasks = (TaskDataSource) null;
      }
      if (this.documents != null)
      {
        this.documents.Dispose();
        this.documents = (DocumentDataSource) null;
      }
      if (this.milestones != null)
      {
        this.milestones.Dispose();
        this.milestones = (MilestoneDataSource) null;
      }
      if (this.loanActions != null)
      {
        this.loanActions.Dispose();
        this.loanActions = (LoanActionDataSource) null;
      }
      if (this.calendar != null)
      {
        this.calendar.Dispose();
        this.calendar = (CalendarDataSource) null;
      }
      if (this.currentUser == null)
        return;
      this.currentUser.Dispose();
      this.currentUser = (UserDataSource) null;
    }

    public override object InitializeLifetimeService() => (object) null;
  }
}
