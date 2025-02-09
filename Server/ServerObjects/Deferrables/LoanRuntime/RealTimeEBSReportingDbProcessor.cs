// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.RealTimeEBSReportingDbProcessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Service.Common;
using Encompass.Diagnostics;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  internal sealed class RealTimeEBSReportingDbProcessor(DeferrableLoanTransaction transaction) : 
    ProcessorBase(transaction),
    IReportingDbProcessor,
    IDeferrableProcessor
  {
    private bool syncUpdateFailed;

    public override void InternalExecute()
    {
      DeferrableLoanTransactionContext currentContext = this.CurrentTransaction.CurrentContext;
      DateTime lastModified = currentContext.DataBag.Get<DateTime>("XDBModifiedTime");
      try
      {
        string message = currentContext.CurrentLoan.InternalUpdateLoanXDBTablesForEBS(lastModified);
        if (!string.IsNullOrEmpty(message))
          throw new Exception(message);
      }
      catch (Exception ex)
      {
        this.syncUpdateFailed = true;
        DiagUtility.DefaultLogger.Write(Encompass.Diagnostics.Logging.LogLevel.WARN, "RealTimeEBSReportingDbProcessor.InternalExecute", "Exception while writing to RDB synchronously. Attempting to write asynchronously.\r\n", ex);
        this.CurrentTransaction.DeliveryTaskList.Add(new DeferrableMessageDeliveryTask((IMessage) AsyncProcessMessage.CreateBlank(MessageActionType.ReportingDatabase), "ReportingDatabase"));
      }
    }

    public override string GetKey() => this.GetType().ToString();

    public override DeferrableType GetDeferrableType()
    {
      return !this.syncUpdateFailed ? DeferrableType.RealTime : DeferrableType.Deferred;
    }
  }
}
