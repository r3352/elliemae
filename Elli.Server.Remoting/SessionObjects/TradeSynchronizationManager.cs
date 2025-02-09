// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.TradeSynchronizationManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class TradeSynchronizationManager : SessionBoundObject, ITradeSynchronizationManager
  {
    private const string className = "TradeSynchronizationManager";
    protected static string sw = Tracing.SwOutsideLoan;

    public TradeSynchronizationManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (TradeSynchronizationManager).ToLower());
      return this;
    }

    public virtual int Assign(
      TradeInfoObj tradeInfo,
      List<TradeAssignmentItem> items,
      List<string> skipFieldList,
      string siteId,
      BatchJobApplicationChannel applicationChannel,
      string tradeExtensionInfo = null,
      string lockLoanSyncOption = "syncLockToLoan")
    {
      this.onApiCalled(nameof (TradeSynchronizationManager), nameof (Assign), new object[1]
      {
        (object) tradeInfo
      });
      try
      {
        return TradeSynchronizationHelper.Assign(tradeInfo, items, skipFieldList, this.Session.GetUserInfo(), "elli", "encompass", ClientContext.GetCurrent().InstanceName, siteId, "TradeSynchronization", applicationChannel, lockLoanSyncOption, tradeInfo.status.ToDescription(), tradeExtensionInfo, this.Session.SessionID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeSynchronizationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual BatchJobSummaryInfo[] GetAllJobStatus()
    {
      this.onApiCalled(nameof (TradeSynchronizationManager), nameof (GetAllJobStatus), (object[]) null);
      try
      {
        return BatchJobsAndItemsAccessor.GetAllBatchJobs();
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeSynchronizationManager.sw, nameof (TradeSynchronizationManager), TraceLevel.Error, "GetAllJobStatus: " + ex.Message + " | Stack Trace - " + ex.StackTrace);
        throw ex;
      }
    }

    public virtual bool Cancel(int jobId)
    {
      this.onApiCalled(nameof (TradeSynchronizationManager), nameof (Cancel), (object[]) null);
      try
      {
        BatchJobInfo batchJob = BatchJobsAndItemsAccessor.GetBatchJob(jobId);
        if (batchJob == null)
          throw new Exception("Job with JobId - " + (object) jobId + " not found.");
        if (batchJob.Status == BatchJobStatus.Cancelled || batchJob.Status == BatchJobStatus.Completed || batchJob.Status == BatchJobStatus.CompletedWithError)
          throw new Exception("Can not cancel job which is in Cancelled/Completed/CompletedWithError status.");
        if (!this.Session.GetUserInfo().IsAdministrator() && !this.Session.GetUserInfo().IsSuperAdministrator() || batchJob.CreatedBy != this.Session.GetUserInfo().Userid)
          throw new Exception(jobId.ToString() + " - Only Administrator or owner of job can cancel this job.");
        batchJob.Status = BatchJobStatus.Cancelled;
        BatchJobsAndItemsAccessor.UpdateBatchJob(batchJob);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeSynchronizationManager.sw, nameof (TradeSynchronizationManager), TraceLevel.Error, "Cancel: " + ex.Message + " | Stack Trace - " + ex.StackTrace);
        throw ex;
      }
    }

    public virtual bool Remove(int jobId)
    {
      this.onApiCalled(nameof (TradeSynchronizationManager), nameof (Remove), (object[]) null);
      try
      {
        BatchJobInfo batchJob = BatchJobsAndItemsAccessor.GetBatchJob(jobId);
        if (batchJob == null)
          throw new Exception("Job with JobId - " + (object) jobId + " not found.");
        if (!this.Session.GetUserInfo().IsAdministrator() && !this.Session.GetUserInfo().IsSuperAdministrator())
          throw new Exception(jobId.ToString() + " - Only Administrator can remove job.");
        CorrespondentTradeInfo info = batchJob.Status == BatchJobStatus.Cancelled || batchJob.Status == BatchJobStatus.Completed || batchJob.Status == BatchJobStatus.CompletedWithError || batchJob.Status == BatchJobStatus.Error ? CorrespondentTrades.GetTrade(int.Parse(batchJob.EntityId)) : throw new Exception(jobId.ToString() + " - Job can be removed only if status is in Cancelled/Completed/Error.");
        if (info == null)
          throw new Exception("Trade with TradeId = " + batchJob.EntityId + " not found.");
        info.Status = TradeStatus.Open;
        CorrespondentTrades.UpdateTrade(info, this.Session.GetUserInfo(), false, false);
        BatchJobsAndItemsAccessor.DeleteBatchJob(jobId, "SDK", this.Session.UserID, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeSynchronizationManager.sw, nameof (TradeSynchronizationManager), TraceLevel.Error, "Remove: " + ex.Message + " | Stack Trace - " + ex.StackTrace);
        throw ex;
      }
    }

    public virtual BatchJobInfo GetJob(int jobId)
    {
      this.onApiCalled(nameof (TradeSynchronizationManager), nameof (GetJob), (object[]) null);
      try
      {
        return BatchJobsAndItemsAccessor.GetBatchJob(jobId);
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeSynchronizationManager.sw, nameof (TradeSynchronizationManager), TraceLevel.Error, "GetJob: " + ex.Message + " | Stack Trace - " + ex.StackTrace);
        throw ex;
      }
    }
  }
}
