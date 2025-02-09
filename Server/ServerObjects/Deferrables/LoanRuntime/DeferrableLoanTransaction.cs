// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.DeferrableLoanTransaction
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Common;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  internal class DeferrableLoanTransaction(IDeferrableTransactionContext transactionContext) : 
    DeferrableTransactionBase(transactionContext),
    IDisposable
  {
    private DeferrableMessageDeliveryTaskList _deliveryTaskList;

    public DeferrableLoanTransactionContext CurrentContext
    {
      get => (DeferrableLoanTransactionContext) base.CurrentContext;
    }

    public DeferrableMessageDeliveryTaskList DeliveryTaskList
    {
      get
      {
        if (this._deliveryTaskList == null)
          this._deliveryTaskList = DeferrableMessageDeliveryTaskList.GetInstance("Elli.Workflow.LoanAlternation.Invoke");
        return this._deliveryTaskList;
      }
    }

    public override void Initialize(PerformanceMeter meter = null)
    {
      if (this.Initialized)
        return;
      if (this.CurrentContext == null)
        throw new Exception("DeferrableLoanTransactionContext cannot be null.");
      this.Meter = meter;
      this.InjectDefaultPreProcessor();
      this.InjectDefaultPostProcessor();
      this.InjectDefaultReportDbProcessor();
      this.InjectDefaultAuditTrailProcessor();
      this.Initialized = true;
    }

    public override void Complete(DeferrableType? deferrableType = null)
    {
      if (!this.Initialized)
        throw new Exception("DeferrableLoanTransaction has not been initialized.");
      if (deferrableType.HasValue)
        this.SetDeferrableType(deferrableType.Value);
      if (this.CurrentContext.CurrentUser == (UserInfo) null)
        this.SetDeferrableType(DeferrableType.RealTime);
      if (!DeferrableManager.IsEnvironmentDeferrableFriendly(this.DeferrableType))
        this.SetDeferrableType(DeferrableType.RealTime);
      new CompleteTransactionHandler(this).Handle();
    }

    public void AddCheckpoint(string verbiage)
    {
      if (this.Meter == null)
        return;
      this.Meter.AddCheckpoint(verbiage, 80, nameof (AddCheckpoint), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Deferrables\\LoanRuntime\\DeferrableLoanTransaction.cs");
    }

    private DeferrableLoanTransaction InjectDefaultPreProcessor()
    {
      return this.Inject<IPreProcessor>((IDeferrableProcessorFactory<IPreProcessor>) new PreProcessorFactory(this)) as DeferrableLoanTransaction;
    }

    private DeferrableLoanTransaction InjectDefaultPostProcessor()
    {
      return this.Inject<IPostProcessor>((IDeferrableProcessorFactory<IPostProcessor>) new PostProcessorFactory(this)) as DeferrableLoanTransaction;
    }

    private DeferrableLoanTransaction InjectDefaultReportDbProcessor()
    {
      return this.Inject<IReportingDbProcessor>((IDeferrableProcessorFactory<IReportingDbProcessor>) new ReportingDbProcessorFactory(this)) as DeferrableLoanTransaction;
    }

    private DeferrableLoanTransaction InjectDefaultAuditTrailProcessor()
    {
      return this.Inject<IAuditTrailProcessor>((IDeferrableProcessorFactory<IAuditTrailProcessor>) new AuditTrailProcessorFactory(this)) as DeferrableLoanTransaction;
    }

    private DeferrableLoanTransaction InjectDefaultAlertNotificationProcessor()
    {
      return this.Inject<IAlertNotificationProcessor>((IDeferrableProcessorFactory<IAlertNotificationProcessor>) new AlertNotificationProcessorFactory(this)) as DeferrableLoanTransaction;
    }

    private DeferrableLoanTransaction InjectDefaultServiceOrderProcessor()
    {
      return this.Inject<IServiceOrderProcessor>((IDeferrableProcessorFactory<IServiceOrderProcessor>) new ServiceOrderProcessorFactory(this)) as DeferrableLoanTransaction;
    }
  }
}
