// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IBatchJobsManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IBatchJobsManager
  {
    int CreateBatchJob(BatchJobInfo batchJob);

    BatchJobInfo CreateBatchJobInfo(BatchJobInfo batchJob);

    void CreateBatchJobItems(int batchJobId, List<BatchJobItemInfo> batchJobItems);

    void CreateBatchJobItems(List<BatchJobItemInfo> batchJobItems);

    BatchJobInfo GetBatchJob(int batchJobId);

    BatchJobSummaryInfo[] GetBatchJobs(int[] batchJobIds);

    BatchJobSummaryInfo[] GetAllBatchJobs();

    List<BatchJobItemInfo> GetAllBatchJobItems(int batchJobId);

    BatchJobItemInfo GetBatchJobItemByBatchJobItemId(int batchJobItemId);

    void UpdateBatchJob(BatchJobInfo batchJob);

    void UpdateBatchJob(
      int batchJobId,
      string applicationChannel,
      BatchJobStatus status,
      DateTime statusDate,
      string lastModifiedByUserId);

    void UpdateBatchJobStatus(
      int batchJobId,
      string applicationChannel,
      BatchJobStatus status,
      DateTime statusDate,
      string lastModifiedByUserId);

    void UpdateBatchJobItem(BatchJobItemInfo batchJobItemInfo);

    void UpdateBatchJobItem(
      int batchJobItemId,
      BatchJobItemStatus status,
      DateTime statusDate,
      string result);

    void UpdateBatchJobItem(
      int batchJobId,
      string entityId,
      BatchJobItemStatus status,
      DateTime statusDate,
      string result);

    void DeleteBatchJob(
      int batchJobId,
      string applicationChannel,
      string userId,
      bool removeFromTable = false);

    void DeleteBatchJobs(
      int[] batchJobId,
      string applicationChannel,
      string userId,
      bool removeFromTable = false);

    void CancelBatchJobs(
      int[] batchJobIds,
      string applicationChannel,
      string userId,
      DateTime cancelledDate);

    BatchJobSummaryInfo[] GetBatchJobPipeline(
      BatchJobPipeline batchJobPipeline,
      out int totalCount,
      int start = 0,
      int limit = 1000);

    int[] GetExistingBatchJobIds(int[] batchJobIds);
  }
}
