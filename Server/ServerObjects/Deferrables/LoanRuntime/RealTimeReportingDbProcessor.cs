// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.RealTimeReportingDbProcessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.ElliEnum;
using Elli.Service.Common;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Configuration;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  internal sealed class RealTimeReportingDbProcessor(DeferrableLoanTransaction transaction) : 
    ProcessorBase(transaction),
    IReportingDbProcessor,
    IDeferrableProcessor
  {
    private const string className = "RealTimeReportingDbProcessor�";

    public override void InternalExecute()
    {
      DeferrableLoanTransactionContext currentContext = this.CurrentTransaction.CurrentContext;
      DateTime now = DateTime.Now;
      LoanData currentLoanData = currentContext.GetAfterLoanDataPerRole();
      string appSetting1 = ConfigurationManager.AppSettings["ReportingDbTimeDiff"];
      int num = string.IsNullOrWhiteSpace(appSetting1) ? 1 : int.Parse(appSetting1);
      bool flag = false;
      string appSetting2 = ConfigurationManager.AppSettings["EnableAsyncReportingDB"];
      if (!string.IsNullOrWhiteSpace(appSetting2))
        flag = Utils.ParseBoolean((object) appSetting2);
      string typeUrnPart;
      DateTime xdbModifiedTime;
      if (currentContext.Role == DeferrableProcessorRole.Subscriber)
      {
        if (currentContext.GetMessage().ActionType != MessageActionType.ReportingDatabase)
          return;
        typeUrnPart = currentContext.GetMessage().TryGetTypeUrnPart(StandardMessage.TypeUrnPart.EventId, "");
        xdbModifiedTime = currentContext.GetMessage().XDBModifiedTime;
      }
      else
      {
        typeUrnPart = currentContext.DataBag.Get<string>("EventId");
        xdbModifiedTime = currentContext.DataBag.Get<DateTime>("XDBModifiedTime");
      }
      if (currentContext.Role == DeferrableProcessorRole.Subscriber || currentContext.Role == DeferrableProcessorRole.RelaySubscriber)
      {
        LoanServerInfo infoFromDatabase = Loan.InternalGetServerInfoFromDatabase(currentContext.CurrentLoan.Identity);
        if (infoFromDatabase != null && infoFromDatabase.LastModified > xdbModifiedTime && (int) (infoFromDatabase.LastModified - xdbModifiedTime).TotalSeconds > num)
        {
          if (flag)
          {
            TraceLog.WriteInfo(nameof (RealTimeReportingDbProcessor), string.Format("Loan {0} last modified at {1}. Acknowledging the async reporting db update at {2}", (object) currentContext.CurrentLoan.LoanData.GUID, (object) infoFromDatabase.LastModified.ToLongTimeString(), (object) xdbModifiedTime.ToLongTimeString()));
            currentLoanData = currentContext.CurrentLoan.LoanData;
          }
          else
            TraceLog.WriteVerbose(nameof (RealTimeReportingDbProcessor), string.Format("Loan last modified at {0}, skip last modification at {1}", (object) infoFromDatabase.LastModified.ToLongTimeString(), (object) xdbModifiedTime.ToLongTimeString()));
        }
      }
      string error = currentContext.CurrentLoan.InternalUpdateLoanXDBTables(xdbModifiedTime, currentContext.GetPriorLoanDataPerRole(), currentLoanData);
      if (error != null)
        this.CurrentTransaction.RecordError(this.GetKey(), error);
      this.CurrentTransaction.AddCheckpoint(typeUrnPart == "CreateLoan" ? "CreateNew: Updated Reporting Database tables ..." : "CheckIn: Updated Repoting Database tables...");
    }

    public override string GetKey() => this.GetType().ToString();

    public override DeferrableType GetDeferrableType() => DeferrableType.RealTime;
  }
}
