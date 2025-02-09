// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.ContextBoundCodeImplBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections.Concurrent;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public class ContextBoundCodeImplBase : CompiledCodeImplBase
  {
    private ConcurrentDictionary<int, IExecutionContext> threadContexts = new ConcurrentDictionary<int, IExecutionContext>();

    protected virtual void EstablishContext(IExecutionContext context)
    {
      this.threadContexts[Thread.CurrentThread.ManagedThreadId] = context;
    }

    protected virtual void ReleaseContext()
    {
      this.threadContexts[Thread.CurrentThread.ManagedThreadId] = (IExecutionContext) null;
    }

    protected IExecutionContext Context
    {
      get => this.threadContexts[Thread.CurrentThread.ManagedThreadId];
    }

    protected IFieldSource Fields => this.Context.Fields;

    protected ITaskDataSource Tasks => this.Context.Tasks;

    protected IDocumentDataSource Documents => this.Context.Documents;

    protected IMilestoneDataSource Milestones => this.Context.Milestones;

    protected ILoanActionDataSource LoanActions => this.Context.LoanActions;

    protected ICalendarDataSource Calendar => this.Context.Calendar;

    protected IUserDataSource CurrentUser => this.Context.CurrentUser;

    public string UserName => this.Context.UserName;

    public string UserID => this.Context.UserID;

    public object XType(object value, string fieldId) => this.Fields.XType(value, fieldId);
  }
}
