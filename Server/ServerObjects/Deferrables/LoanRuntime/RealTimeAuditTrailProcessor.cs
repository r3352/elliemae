// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.RealTimeAuditTrailProcessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.ElliEnum;
using Elli.Service.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  internal sealed class RealTimeAuditTrailProcessor(DeferrableLoanTransaction transaction) : 
    ProcessorBase(transaction),
    IAuditTrailProcessor,
    IDeferrableProcessor
  {
    private const string className = "RealTimeAuditTrailProcessor�";

    public override void InternalExecute()
    {
      DeferrableLoanTransactionContext currentContext = this.CurrentTransaction.CurrentContext;
      string typeUrnPart;
      bool updateForceRebuild;
      DateTime auditModifiedTime;
      DateTime auditCurrentTime;
      if (currentContext.Role == DeferrableProcessorRole.Subscriber)
      {
        if (currentContext.GetMessage().ActionType != MessageActionType.AuditTrail)
          return;
        typeUrnPart = currentContext.GetMessage().TryGetTypeUrnPart(StandardMessage.TypeUrnPart.EventId, "");
        updateForceRebuild = currentContext.GetMessage().UpdateForceRebuild;
        auditModifiedTime = currentContext.GetMessage().AuditModifiedTime;
        auditCurrentTime = currentContext.GetMessage().AuditCurrentTime;
      }
      else
      {
        typeUrnPart = currentContext.DataBag.Get<string>("EventId");
        updateForceRebuild = currentContext.DataBag.Get<bool>("UpdateForceRebuild");
        auditModifiedTime = currentContext.DataBag.Get<DateTime>("AuditModifiedTime");
        auditCurrentTime = currentContext.DataBag.Get<DateTime>("AuditCurrentTime");
      }
      if (updateForceRebuild)
        return;
      currentContext.CurrentLoan.InternalUpdateAuditTrail(currentContext.CurrentUser, auditModifiedTime, auditCurrentTime, currentContext.GetPriorLoanDataPerRole(), currentContext.GetAfterLoanDataPerRole(), currentContext.AuditUserId);
      this.CurrentTransaction.AddCheckpoint(typeUrnPart == "CreateLoan" ? "CreateNew: Added Audit Trail records ..." : "CheckIn: Added audit trail records to database ...");
    }

    public override string GetKey() => this.GetType().ToString();

    public override DeferrableType GetDeferrableType() => DeferrableType.RealTime;
  }
}
