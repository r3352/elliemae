// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.DeferredAuditTrailProcessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Service.Common;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  internal sealed class DeferredAuditTrailProcessor(DeferrableLoanTransaction transaction) : 
    ProcessorBase(transaction),
    IAuditTrailProcessor,
    IDeferrableProcessor
  {
    public override void InternalExecute()
    {
      this.CurrentTransaction.DeliveryTaskList.Add(new DeferrableMessageDeliveryTask((IMessage) AsyncProcessMessage.CreateBlank(MessageActionType.AuditTrail), "AuditTrail"));
    }

    public override string GetKey() => this.GetType().ToString();

    public override DeferrableType GetDeferrableType() => DeferrableType.Deferred;
  }
}
