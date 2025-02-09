// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.BatchJobsManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class BatchJobsManager : SessionBoundObject, IBatchJobsManager
  {
    private const string className = "BatchJobsManager";

    public BatchJobsManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (BatchJobsManager).ToLower());
      return this;
    }

    public virtual int CreateBatchJob(BatchJobInfo batchJob)
    {
      this.onApiCalled(nameof (BatchJobsManager), "CreateBatchJobs", Array.Empty<object>());
      try
      {
        return BatchJobsAndItemsAccessor.CreateBatchJob(batchJob);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual BatchJobInfo CreateBatchJobInfo(BatchJobInfo batchJob)
    {
      this.onApiCalled(nameof (BatchJobsManager), nameof (CreateBatchJobInfo), Array.Empty<object>());
      try
      {
        return BatchJobsAndItemsAccessor.CreateBatchJobInfo(batchJob);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
        return (BatchJobInfo) null;
      }
    }

    public virtual void CreateBatchJobItems(int batchJobId, List<BatchJobItemInfo> batchJobItems)
    {
      this.onApiCalled(nameof (BatchJobsManager), nameof (CreateBatchJobItems), Array.Empty<object>());
      try
      {
        BatchJobsAndItemsAccessor.CreateBatchJobItems(batchJobId, batchJobItems);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CreateBatchJobItems(List<BatchJobItemInfo> batchJobItems)
    {
      this.onApiCalled(nameof (BatchJobsManager), nameof (CreateBatchJobItems), Array.Empty<object>());
      try
      {
        BatchJobsAndItemsAccessor.CreateBatchJobItems(batchJobItems);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual List<BatchJobItemInfo> GetAllBatchJobItems(int batchJobId)
    {
      List<BatchJobItemInfo> allBatchJobItems = new List<BatchJobItemInfo>();
      this.onApiCalled(nameof (BatchJobsManager), nameof (GetAllBatchJobItems), Array.Empty<object>());
      try
      {
        allBatchJobItems = BatchJobsAndItemsAccessor.GetAllBatchJobItems(batchJobId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
      return allBatchJobItems;
    }

    public virtual BatchJobItemInfo GetBatchJobItemByBatchJobItemId(int batchJobItemId)
    {
      BatchJobItemInfo byBatchJobItemId = (BatchJobItemInfo) null;
      this.onApiCalled(nameof (BatchJobsManager), nameof (GetBatchJobItemByBatchJobItemId), Array.Empty<object>());
      try
      {
        byBatchJobItemId = BatchJobsAndItemsAccessor.GetBatchJobItemByBatchJobItemId(batchJobItemId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
      return byBatchJobItemId;
    }

    public virtual BatchJobInfo GetBatchJob(int batchJobId)
    {
      BatchJobInfo batchJob = (BatchJobInfo) null;
      this.onApiCalled(nameof (BatchJobsManager), nameof (GetBatchJob), Array.Empty<object>());
      try
      {
        batchJob = BatchJobsAndItemsAccessor.GetBatchJob(batchJobId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
      return batchJob;
    }

    public virtual void UpdateBatchJob(BatchJobInfo batchJob)
    {
      this.onApiCalled(nameof (BatchJobsManager), "updateBatchJob", Array.Empty<object>());
      try
      {
        BatchJobsAndItemsAccessor.UpdateBatchJob(batchJob);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateBatchJob(
      int batchJobId,
      string applicationChannel,
      BatchJobStatus status,
      DateTime statusDate,
      string lastModifiedByUserId)
    {
      this.onApiCalled(nameof (BatchJobsManager), "updateBatchJob", Array.Empty<object>());
      try
      {
        BatchJobsAndItemsAccessor.UpdateBatchJob(batchJobId, applicationChannel, status, statusDate, lastModifiedByUserId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateBatchJobStatus(
      int batchJobId,
      string applicationChannel,
      BatchJobStatus status,
      DateTime statusDate,
      string lastModifiedByUserId)
    {
      this.onApiCalled(nameof (BatchJobsManager), "updateBatchJobStatus", Array.Empty<object>());
      try
      {
        BatchJobsAndItemsAccessor.UpdateBatchJob(batchJobId, applicationChannel, status, statusDate, lastModifiedByUserId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateBatchJobItem(BatchJobItemInfo batchJobItemInfo)
    {
      this.onApiCalled(nameof (BatchJobsManager), "updateBatchJobItem", Array.Empty<object>());
      try
      {
        BatchJobsAndItemsAccessor.UpdateBatchJobItem(batchJobItemInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateBatchJobItem(
      int batchJobItemId,
      BatchJobItemStatus status,
      DateTime statusDate,
      string result)
    {
      this.onApiCalled(nameof (BatchJobsManager), "updateBatchJobItem", Array.Empty<object>());
      try
      {
        BatchJobsAndItemsAccessor.UpdateBatchJobItem(batchJobItemId, status, statusDate, result);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateBatchJobItem(
      int batchJobId,
      string entityId,
      BatchJobItemStatus status,
      DateTime statusDate,
      string result)
    {
      this.onApiCalled(nameof (BatchJobsManager), "updateBatchJobItem", Array.Empty<object>());
      try
      {
        BatchJobsAndItemsAccessor.UpdateBatchJobItem(batchJobId, entityId, status, statusDate, result);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteBatchJob(
      int batchJobId,
      string applicationChannel,
      string userId,
      bool removeFromTable = false)
    {
      this.onApiCalled(nameof (BatchJobsManager), nameof (DeleteBatchJob), Array.Empty<object>());
      try
      {
        BatchJobsAndItemsAccessor.DeleteBatchJob(batchJobId, applicationChannel, userId, removeFromTable);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteBatchJobs(
      int[] batchJobIds,
      string applicationChannel,
      string userId,
      bool removeFromTable = false)
    {
      this.onApiCalled(nameof (BatchJobsManager), nameof (DeleteBatchJobs), Array.Empty<object>());
      try
      {
        BatchJobsAndItemsAccessor.DeleteBatchJobs(batchJobIds, applicationChannel, userId, removeFromTable);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BatchJobSummaryInfo[] GetAllBatchJobs()
    {
      BatchJobSummaryInfo[] allBatchJobs = new BatchJobSummaryInfo[0];
      this.onApiCalled(nameof (BatchJobsManager), "DeleteBatchJob", Array.Empty<object>());
      try
      {
        allBatchJobs = BatchJobsAndItemsAccessor.GetAllBatchJobs();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
      return allBatchJobs;
    }

    public virtual BatchJobSummaryInfo[] GetBatchJobs(int[] batchJobIds)
    {
      BatchJobSummaryInfo[] batchJobs = new BatchJobSummaryInfo[0];
      this.onApiCalled(nameof (BatchJobsManager), "GetBatchJobsByBatchJobIds", Array.Empty<object>());
      try
      {
        batchJobs = BatchJobsAndItemsAccessor.GetBatchJobsByIds(batchJobIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
      return batchJobs;
    }

    public virtual void CancelBatchJobs(
      int[] batchJobIds,
      string applicationChannel,
      string userId,
      DateTime cancelledDate)
    {
      this.onApiCalled(nameof (BatchJobsManager), nameof (CancelBatchJobs), Array.Empty<object>());
      try
      {
        BatchJobsAndItemsAccessor.CancelBatchJobs(batchJobIds, applicationChannel, userId, cancelledDate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
    }

    public BatchJobSummaryInfo[] GetBatchJobPipeline(
      BatchJobPipeline batchJobPipeline,
      out int totalCount,
      int start = 0,
      int limit = 1000)
    {
      BatchJobSummaryInfo[] batchJobPipeline1 = new BatchJobSummaryInfo[0];
      this.onApiCalled(nameof (BatchJobsManager), nameof (GetBatchJobPipeline), Array.Empty<object>());
      int totalBatchJobsCount = 0;
      try
      {
        batchJobPipeline1 = BatchJobsAndItemsAccessor.GetBatchJobsPipeline(batchJobPipeline, out totalBatchJobsCount, start, limit);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
      totalCount = totalBatchJobsCount;
      return batchJobPipeline1;
    }

    public virtual int[] GetExistingBatchJobIds(int[] batchJobIds)
    {
      int[] existingBatchJobIds = new int[0];
      this.onApiCalled(nameof (BatchJobsManager), "GetExistingBatchJobsByIds", Array.Empty<object>());
      try
      {
        existingBatchJobIds = BatchJobsAndItemsAccessor.GetExistingBatchJobIds(batchJobIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BatchJobsManager), ex, this.Session.SessionInfo);
      }
      return existingBatchJobIds;
    }
  }
}
