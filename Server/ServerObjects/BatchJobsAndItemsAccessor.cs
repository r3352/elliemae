// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.BatchJobsAndItemsAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class BatchJobsAndItemsAccessor
  {
    private const string className = "BatchJobsAndItemsAccessor�";
    private const string batchJobTableName = "BatchJobs�";
    private const string batchJobItemsTableName = "BatchJobItems�";

    public static int CreateBatchJob(BatchJobInfo batchJob)
    {
      return BatchJobsAndItemsAccessor.createBatchJob(batchJob);
    }

    public static BatchJobInfo CreateBatchJobInfo(BatchJobInfo batchJob)
    {
      return BatchJobsAndItemsAccessor.getBatchJobInfoByJobId(BatchJobsAndItemsAccessor.createBatchJob(batchJob));
    }

    public static void CreateBatchJobItems(int batchJobId, List<BatchJobItemInfo> batchJobItems)
    {
      if (batchJobItems == null || batchJobItems.Count == 0)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("BatchJobItems");
      foreach (BatchJobItemInfo batchJobItem in batchJobItems)
      {
        DbValueList jobItemDbValueList = BatchJobsAndItemsAccessor.createBatchJobItemDbValueList(batchJobId, batchJobItem);
        dbQueryBuilder.InsertInto(table, jobItemDbValueList, true, false);
      }
      dbQueryBuilder.ExecuteScalar();
    }

    public static void CreateBatchJobItems(List<BatchJobItemInfo> batchJobItems)
    {
      if (batchJobItems == null || batchJobItems.Count == 0)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("BatchJobItems");
      foreach (BatchJobItemInfo batchJobItem in batchJobItems)
      {
        DbValueList jobItemDbValueList = BatchJobsAndItemsAccessor.createBatchJobItemDbValueList(batchJobItem.BatchJobId, batchJobItem);
        dbQueryBuilder.InsertInto(table, jobItemDbValueList, true, false);
      }
      dbQueryBuilder.ExecuteScalar();
    }

    public static List<BatchJobItemInfo> GetAllBatchJobItems(int batchJobId)
    {
      if (batchJobId <= 0)
        return (List<BatchJobItemInfo>) null;
      List<BatchJobItemInfo> allBatchJobItems = new List<BatchJobItemInfo>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select bji.* from BatchJobItems bji");
      dbQueryBuilder.AppendLine("   where bji.BatchJobId = " + (object) batchJobId);
      DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        BatchJobItemInfo batchJobItemInfo = BatchJobsAndItemsAccessor.dataRowToBatchJobItemInfo(table.Rows[index]);
        allBatchJobItems.Add(batchJobItemInfo);
      }
      return allBatchJobItems;
    }

    public static List<BatchJobInfo> GetStuckedBatchJobs()
    {
      List<BatchJobInfo> source = new List<BatchJobInfo>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select t.status as TradeStatus, b.BatchJobId, b.CreatedBy, b.CreatedDate, b.ApplicationChannel, b.Status as JobStatus, b.StatusDate as JobStatusDate, b.LastModifiedByUserId, b.EntityId as JobEntityId, b.EntityType as JobEntityType, b.MetaData as JobMetaData, ");
      dbQueryBuilder.AppendLine("i.BatchJobItemId, i.Status as ItemStatus, i.StatusDate as ItemStatusDate, i.Action, i.EntityId as ItemEntityId, i.EntityType as ItemEntityType, i.MetaData as ItemMetaData, i.Result from [BatchJobItems] i ");
      dbQueryBuilder.AppendLine("inner join [BatchJobs] b on i.BatchJobId = b.BatchJobId ");
      dbQueryBuilder.AppendLine("inner join [Trades] t on t.TradeId = b.EntityId ");
      dbQueryBuilder.AppendLine("where b.BatchJobId in ( select BatchJobId ");
      dbQueryBuilder.AppendLine("from BatchJobItems ");
      dbQueryBuilder.AppendLine("group by BatchJobId ");
      dbQueryBuilder.AppendLine("having datediff(HOUR, max(statusdate) , getUtcdate()) > 1 and min(status)= 1 and datediff(DAY, max(statusdate) , getUtcdate()) < 31)");
      dbQueryBuilder.AppendLine("order by b.batchJobId desc");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        DataRow r = dataRow;
        BatchJobInfo batchJobInfo;
        if (!source.Any<BatchJobInfo>((System.Func<BatchJobInfo, bool>) (j => j.BatchJobId == SQL.DecodeInt(r["BatchJobId"]))))
        {
          List<BatchJobItemInfo> batchJobItems = new List<BatchJobItemInfo>();
          batchJobInfo = new BatchJobInfo(SQL.DecodeInt(r["BatchJobId"]), SQL.DecodeString(r["CreatedBy"]), SQL.DecodeDateTime(r["CreatedDate"]), (BatchJobApplicationChannel) Enum.Parse(typeof (BatchJobApplicationChannel), SQL.DecodeString(r["ApplicationChannel"])), (BatchJobStatus) SQL.DecodeInt(r["JobStatus"]), SQL.DecodeDateTime(r["JobStatusDate"]), BatchJobType.TradeSynchronization, SQL.DecodeString(r["LastModifiedByUserId"]), SQL.DecodeString(r["JobEntityId"]), (BatchJobEntityType) SQL.DecodeInt(r["JobEntityType"]), SQL.DecodeString(r["JobMetaData"]), batchJobItems, SQL.DecodeString(r["TradeStatus"]));
        }
        else
          batchJobInfo = source.Where<BatchJobInfo>((System.Func<BatchJobInfo, bool>) (j => j.BatchJobId == SQL.DecodeInt(r["BatchJobId"]))).FirstOrDefault<BatchJobInfo>();
        if (!batchJobInfo.BatchJobItems.Any<BatchJobItemInfo>((System.Func<BatchJobItemInfo, bool>) (i => i.BatchJobItemId == SQL.DecodeInt(r["BatchJobItemId"]))))
        {
          BatchJobItemInfo batchJobItemInfo = new BatchJobItemInfo(SQL.DecodeInt(r["BatchJobItemId"]), SQL.DecodeInt(r["BatchJobId"]), (BatchJobItemStatus) SQL.DecodeInt(r["ItemStatus"]), SQL.DecodeDateTime(r["ItemStatusDate"]), SQL.DecodeString(r["Action"]), SQL.DecodeString(r["ItemEntityId"]), (BatchJobItemEntityType) SQL.DecodeInt(r["ItemEntityType"]), SQL.DecodeString(r["Result"]), SQL.DecodeString(r["ItemMetaData"]));
          batchJobInfo.BatchJobItems.Add(batchJobItemInfo);
        }
        if (!source.Any<BatchJobInfo>((System.Func<BatchJobInfo, bool>) (j => j.BatchJobId == SQL.DecodeInt(r["BatchJobId"]))))
          source.Add(batchJobInfo);
      }
      return source;
    }

    public static BatchJobItemInfo GetBatchJobItemByBatchJobItemId(int batchJobItemId)
    {
      if (batchJobItemId <= 0)
        return (BatchJobItemInfo) null;
      BatchJobItemInfo byBatchJobItemId = (BatchJobItemInfo) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select bji.* from BatchJobItems bji");
      dbQueryBuilder.AppendLine("   where bji.BatchJobItemId = " + (object) batchJobItemId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count > 0)
        byBatchJobItemId = BatchJobsAndItemsAccessor.dataRowToBatchJobItemInfo(dataRowCollection[0]);
      return byBatchJobItemId;
    }

    public static BatchJobInfo GetBatchJob(int batchJobId)
    {
      return batchJobId <= 0 ? (BatchJobInfo) null : BatchJobsAndItemsAccessor.getBatchJobInfoByJobId(batchJobId);
    }

    public static BatchJobSummaryInfo[] GetAllBatchJobs()
    {
      List<BatchJobSummaryInfo> batchJobSummaryInfoList = new List<BatchJobSummaryInfo>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select ");
      dbQueryBuilder.AppendLine("bj.BatchJobId,bj.CreatedBy,bj.CreatedDate,bj.ApplicationChannel,bj.[Status],bj.StatusDate,bj.[Type],bj.LastModifiedByUserId, bj.EntityId, bj.EntityType, t.Name,");
      dbQueryBuilder.AppendLine("COUNT(bji.BatchJobItemId) as TotalLoans,");
      dbQueryBuilder.AppendLine("sum(case when (bji.[Status] = 1) then 1 else 0 end) as UnProcessed,");
      dbQueryBuilder.AppendLine("sum(case when (bji.[Status] = 2) then 1 else 0 end) as InProgress,");
      dbQueryBuilder.AppendLine("sum(case when (bji.[Status] = 3) then 1 else 0 end) as Completed,");
      dbQueryBuilder.AppendLine("sum(case when (bji.[Status] = 4) then 1 else 0 end) as Cancelled,");
      dbQueryBuilder.AppendLine("sum(case when (bji.[Status] = 5 or bji.[Status] = 8) then 1 else 0 end) as Errored");
      dbQueryBuilder.AppendLine("from BatchJobs bj inner join BatchJobItems bji on bj.BatchJobId = bji.BatchJobId");
      dbQueryBuilder.AppendLine("left join Trades t on bj.EntityId = t.TradeID");
      dbQueryBuilder.AppendLine("group by bj.BatchJobId,bj.CreatedBy,bj.CreatedDate,bj.ApplicationChannel,bj.[Status],bj.StatusDate,bj.[Type],bj.LastModifiedByUserId, bj.EntityId, bj.EntityType, t.Name");
      foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        batchJobSummaryInfoList.Add(BatchJobsAndItemsAccessor.dataRowToBatchJobSummaryInfo(r));
      return batchJobSummaryInfoList.ToArray();
    }

    public static BatchJobSummaryInfo[] GetBatchJobsByIds(int[] batchJobIds)
    {
      List<BatchJobSummaryInfo> batchJobSummaryInfoList = new List<BatchJobSummaryInfo>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select ");
      dbQueryBuilder.AppendLine("bj.BatchJobId,bj.CreatedBy,bj.CreatedDate,bj.ApplicationChannel,bj.[Status],bj.StatusDate,bj.[Type],bj.LastModifiedByUserId, bj.EntityId, bj.EntityType, t.Name,");
      dbQueryBuilder.AppendLine("COUNT(bji.BatchJobItemId) as TotalLoans,");
      dbQueryBuilder.AppendLine("sum(case when (bji.[Status] = 1) then 1 else 0 end) as UnProcessed,");
      dbQueryBuilder.AppendLine("sum(case when (bji.[Status] = 2) then 1 else 0 end) as InProgress,");
      dbQueryBuilder.AppendLine("sum(case when (bji.[Status] = 3) then 1 else 0 end) as Completed,");
      dbQueryBuilder.AppendLine("sum(case when (bji.[Status] = 4) then 1 else 0 end) as Cancelled,");
      dbQueryBuilder.AppendLine("sum(case when (bji.[Status] = 5 or bji.[Status] = 8) then 1 else 0 end) as Errored");
      dbQueryBuilder.AppendLine("from BatchJobs bj inner join BatchJobItems bji on bj.BatchJobId = bji.BatchJobId");
      dbQueryBuilder.AppendLine("left join Trades t on bj.EntityId = t.TradeID");
      dbQueryBuilder.AppendLine("where bj.BatchJobId in (" + SQL.EncodeArray((Array) batchJobIds) + ")");
      dbQueryBuilder.AppendLine("group by bj.BatchJobId,bj.CreatedBy,bj.CreatedDate,bj.ApplicationChannel,bj.[Status],bj.StatusDate,bj.[Type],bj.LastModifiedByUserId, bj.EntityId, bj.EntityType, t.Name");
      foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        batchJobSummaryInfoList.Add(BatchJobsAndItemsAccessor.dataRowToBatchJobSummaryInfo(r));
      return batchJobSummaryInfoList.ToArray();
    }

    public static int[] GetExistingBatchJobIds(int[] batchJobIds)
    {
      List<int> intList = new List<int>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select BatchJobId from BatchJobs");
      dbQueryBuilder.AppendLine("where BatchJobId in (" + SQL.EncodeArray((Array) batchJobIds) + ")");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        intList.Add(SQL.DecodeInt(dataRow["BatchJobId"]));
      return intList.ToArray();
    }

    public static void UpdateBatchJob(BatchJobInfo batchJob)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("BatchJobs");
      DbValueList keyValues = new DbValueList();
      keyValues.Add("BatchJobId", (object) batchJob.BatchJobId);
      DbValueList dbValueList = new DbValueList();
      DbValueList batchJobDbValueList = BatchJobsAndItemsAccessor.createBatchJobDbValueList(batchJob);
      BatchJobsAndItemsAccessor.updateTable(table, batchJobDbValueList, keyValues);
    }

    public static void UpdateBatchJob(
      int batchJobId,
      string applicationChannel,
      BatchJobStatus status,
      DateTime statusDate,
      string lastModifiedByUserId)
    {
      BatchJobsAndItemsAccessor.updateBatchJobStatus(batchJobId, applicationChannel, status, statusDate, lastModifiedByUserId);
    }

    public static void UpdateBatchJobStatus(
      int batchJobId,
      string applicationChannel,
      BatchJobStatus status,
      DateTime statusDate,
      string lastModifiedByUserId)
    {
      BatchJobsAndItemsAccessor.updateBatchJobStatus(batchJobId, applicationChannel, status, statusDate, lastModifiedByUserId);
    }

    public static void UpdateBatchJobItem(BatchJobItemInfo batchJobItemInfo)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("BatchJobItems");
      DbValueList keyValues = new DbValueList();
      keyValues.Add("BatchJobItemId", (object) batchJobItemInfo.BatchJobItemId);
      DbValueList dbValueList = new DbValueList();
      DbValueList jobItemDbValueList = BatchJobsAndItemsAccessor.createBatchJobItemDbValueList(batchJobItemInfo.BatchJobId, batchJobItemInfo);
      BatchJobsAndItemsAccessor.updateTable(table, jobItemDbValueList, keyValues);
    }

    public static void UpdateBatchJobItem(
      int batchJobItemId,
      BatchJobItemStatus status,
      DateTime statusDate,
      string result)
    {
      BatchJobsAndItemsAccessor.updateTable(EllieMae.EMLite.Server.DbAccessManager.GetTable("BatchJobItems"), new DbValueList()
      {
        {
          "Status",
          (object) status
        },
        {
          "StatusDate",
          (object) statusDate
        },
        {
          "Result",
          (object) result
        }
      }, new DbValueList()
      {
        {
          "BatchJobItemId",
          (object) batchJobItemId
        }
      });
    }

    public static void UpdateBatchJobItem(
      int batchJobId,
      string entityId,
      BatchJobItemStatus status,
      DateTime statusDate,
      string result)
    {
      BatchJobsAndItemsAccessor.updateTable(EllieMae.EMLite.Server.DbAccessManager.GetTable("BatchJobItems"), new DbValueList()
      {
        {
          "Status",
          (object) status
        },
        {
          "StatusDate",
          (object) statusDate
        },
        {
          "Result",
          (object) result
        }
      }, new DbValueList()
      {
        {
          "BatchJobId",
          (object) batchJobId
        },
        {
          "EntityId",
          (object) entityId
        }
      });
    }

    public static void DeleteBatchJob(
      int batchJobId,
      string applicationChannel,
      string userId,
      bool removeFromTable = false)
    {
      if (removeFromTable)
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("BatchJobs");
        DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("BatchJobItems");
        dbQueryBuilder.DeleteFrom(table2, new DbValueList()
        {
          {
            "BatchJobId",
            (object) batchJobId
          }
        });
        dbQueryBuilder.DeleteFrom(table1, new DbValueList()
        {
          {
            "BatchJobId",
            (object) batchJobId
          }
        });
        dbQueryBuilder.Execute();
      }
      else
        BatchJobsAndItemsAccessor.UpdateBatchJob(batchJobId, applicationChannel, BatchJobStatus.Deleted, DateTime.Now, userId);
    }

    public static void DeleteBatchJobs(
      int[] batchJobIds,
      string applicationChannel,
      string userId,
      bool removeFromTable = false)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (removeFromTable)
      {
        dbQueryBuilder.AppendLine("Delete from BatchJobItems where batchJobId in (" + SQL.EncodeArray((Array) batchJobIds) + ")");
        dbQueryBuilder.AppendLine("Delete from BatchJobs where batchJobId in (" + SQL.EncodeArray((Array) batchJobIds) + ")");
        dbQueryBuilder.Execute();
      }
      else
      {
        dbQueryBuilder.AppendLine("update BatchJobs set status = 6, StatusDate = '" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "', LastModifiedByUserId = '" + userId + "' where batchJobId in (" + SQL.EncodeArray((Array) batchJobIds) + ")");
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static void CancelBatchJobs(
      int[] batchJobIds,
      string applicationChannel,
      string userId,
      DateTime cancelledDate)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("update BatchJobs set status = 4, StatusDate = '" + (object) cancelledDate + "', LastModifiedByUserId = '" + userId + "', applicationChannel = '" + applicationChannel + "' where type = 1 and batchJobId in (" + SQL.EncodeArray((Array) batchJobIds) + ")");
      dbQueryBuilder.AppendLine("update BatchJobItems set status = 4, StatusDate = '" + (object) cancelledDate + "' where status = 1 and batchJobId in (" + SQL.EncodeArray((Array) batchJobIds) + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void CancelBatchJobs(int[] batchJobIds, string userId, DateTime cancelledDate)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("update BatchJobs set status = 4, StatusDate = '" + cancelledDate.ToString("yyyy-MM-dd HH:mm:ss") + "', LastModifiedByUserId = '" + userId + "' where type = 1 and status in (1,2,7) and batchJobId in (" + SQL.EncodeArray((Array) batchJobIds) + ")");
      dbQueryBuilder.AppendLine("update BatchJobItems set status = 4, StatusDate = '" + cancelledDate.ToString("yyyy-MM-dd HH:mm:ss") + "' where status = 1 and batchJobId in (" + SQL.EncodeArray((Array) batchJobIds) + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static BatchJobSummaryInfo[] GetBatchJobsPipeline(
      BatchJobPipeline batchJobPipeline,
      out int totalBatchJobsCount,
      int start = 0,
      int limit = 1000)
    {
      List<BatchJobSummaryInfo> batchJobSummaryInfoList = new List<BatchJobSummaryInfo>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      BatchJobPipelineFilter filter = batchJobPipeline?.Filter;
      BatchJobPipelineSorting sortOrder = batchJobPipeline?.SortOrder;
      string str = "ORDER BY bj.batchJobId desc";
      bool flag = filter != null && filter.TradeIds != null && filter.TradeIds.Length != 0;
      if (!flag && sortOrder != null && !string.IsNullOrWhiteSpace(sortOrder.FieldId))
        str = "ORDER BY " + BatchJobsAndItemsAccessor.getColumnName(sortOrder.FieldId) + (string.IsNullOrWhiteSpace(sortOrder.Order) || !(sortOrder.Order.Trim().ToLower() == "descending") ? " asc" : " desc");
      dbQueryBuilder1.AppendLine("SELECT * FROM (");
      dbQueryBuilder1.AppendLine("SELECT ROW_NUMBER() OVER(" + str + ") AS RowNum, COUNT(*) OVER() AS TotalCount,");
      dbQueryBuilder1.AppendLine("bj.BatchJobId,bj.CreatedBy,bj.CreatedDate,bj.ApplicationChannel,bj.[Status],bj.StatusDate,bj.[Type],bj.LastModifiedByUserId, bj.EntityId, bj.EntityType, t.Name,");
      dbQueryBuilder1.AppendLine("COUNT(bji.BatchJobItemId) as TotalLoans,");
      dbQueryBuilder1.AppendLine("sum(case when (bji.[Status] = 1) then 1 else 0 end) as UnProcessed,");
      dbQueryBuilder1.AppendLine("sum(case when (bji.[Status] = 2) then 1 else 0 end) as InProgress,");
      dbQueryBuilder1.AppendLine("sum(case when (bji.[Status] = 3) then 1 else 0 end) as Completed,");
      dbQueryBuilder1.AppendLine("sum(case when (bji.[Status] = 4) then 1 else 0 end) as Cancelled,");
      dbQueryBuilder1.AppendLine("sum(case when (bji.[Status] = 5 or bji.[Status] = 8) then 1 else 0 end) as Errored");
      dbQueryBuilder1.AppendLine("from BatchJobs bj inner join BatchJobItems bji on bj.BatchJobId = bji.BatchJobId");
      dbQueryBuilder1.AppendLine("left join Trades t on bj.EntityId = t.TradeID");
      dbQueryBuilder1.AppendLine("where bj.[Status] != " + (object) (int) Enum.Parse(typeof (BatchJobStatus), "Deleted") + " ");
      dbQueryBuilder1.AppendLine("and t.Name IS NOT NULL");
      if (filter != null)
      {
        if (flag)
        {
          dbQueryBuilder1.AppendLine("and bj.BatchJobId in (" + SQL.EncodeArray((Array) filter.TradeIds) + ") ");
        }
        else
        {
          if (!string.IsNullOrWhiteSpace(filter.TradeName))
            dbQueryBuilder1.AppendLine("and t.Name like '%" + filter.TradeName.Trim() + "%' ");
          if (!string.IsNullOrWhiteSpace(filter.TradeType))
            dbQueryBuilder1.AppendLine("and bj.EntityType = " + (object) (int) Enum.Parse(typeof (BatchJobEntityType), filter.TradeType.Trim(), true) + " ");
          DateTime? nullable = filter.CreatedDateStart;
          if (nullable.HasValue)
          {
            EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = dbQueryBuilder1;
            nullable = filter.CreatedDateStart;
            ref DateTime? local = ref nullable;
            string text = "and bj.CreatedDate  >= '" + (local.HasValue ? local.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss.fff") : (string) null) + "' ";
            dbQueryBuilder2.AppendLine(text);
          }
          nullable = filter.CreatedDateEnd;
          if (nullable.HasValue)
          {
            EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder3 = dbQueryBuilder1;
            nullable = filter.CreatedDateEnd;
            ref DateTime? local = ref nullable;
            string text = "and bj.CreatedDate  <= '" + (local.HasValue ? local.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss.fff") : (string) null) + "' ";
            dbQueryBuilder3.AppendLine(text);
          }
          if (!string.IsNullOrWhiteSpace(filter.CreatedBy))
            dbQueryBuilder1.AppendLine("and bj.CreatedBy = '" + filter.CreatedBy.Trim() + "' ");
          if (filter.Status != null && filter.Status.Length != 0)
          {
            int[] data = new int[filter.Status.Length];
            for (int index = 0; index < filter.Status.Length; ++index)
              data[index] = (int) Enum.Parse(typeof (BatchJobStatus), filter.Status[index].Trim(), true);
            dbQueryBuilder1.AppendLine("and bj.Status in (" + SQL.EncodeArray((Array) data) + ") ");
          }
        }
      }
      dbQueryBuilder1.AppendLine("group by bj.BatchJobId,bj.CreatedBy,bj.CreatedDate,bj.ApplicationChannel,bj.[Status],bj.StatusDate,bj.[Type],bj.LastModifiedByUserId, bj.EntityId, bj.EntityType, t.Name ");
      dbQueryBuilder1.AppendLine(") AS t ");
      dbQueryBuilder1.AppendLine("ORDER BY t.RowNum ");
      if (!flag)
        dbQueryBuilder1.AppendLine(string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", (object) start, (object) limit));
      DataRowCollection dataRowCollection = dbQueryBuilder1.Execute();
      foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
        batchJobSummaryInfoList.Add(BatchJobsAndItemsAccessor.dataRowToBatchJobSummaryInfo(r));
      totalBatchJobsCount = 0;
      if (dataRowCollection.Count > 0)
        totalBatchJobsCount = SQL.DecodeInt(dataRowCollection[0]["TotalCount"]);
      return batchJobSummaryInfoList.ToArray();
    }

    private static int createBatchJob(BatchJobInfo batchJob)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("BatchJobs");
      DbValueList batchJobDbValueList = BatchJobsAndItemsAccessor.createBatchJobDbValueList(batchJob);
      dbQueryBuilder.Declare("@batchJobId", "int");
      dbQueryBuilder.InsertInto(table1, batchJobDbValueList, true, false);
      dbQueryBuilder.SelectIdentity("@batchJobId");
      if (batchJob.BatchJobItems != null && batchJob.BatchJobItems.Count > 0)
      {
        DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("BatchJobItems");
        foreach (BatchJobItemInfo batchJobItem in batchJob.BatchJobItems)
        {
          DbValueList jobItemDbValueList = BatchJobsAndItemsAccessor.createBatchJobItemDbValueList(batchJobItem);
          DbValue dbValue = new DbValue("BatchJobId", (object) "@batchJobId", (IDbEncoder) DbEncoding.None);
          jobItemDbValueList.Add(dbValue);
          dbQueryBuilder.InsertInto(table2, jobItemDbValueList, true, false);
        }
      }
      dbQueryBuilder.Select("@batchJobId");
      return SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
    }

    private static BatchJobInfo getBatchJobInfoByJobId(int batchJobId)
    {
      BatchJobInfo batchJobInfoByJobId = (BatchJobInfo) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select bj.* from BatchJobs bj");
      dbQueryBuilder.AppendLine("   where bj.BatchJobId = " + (object) batchJobId);
      dbQueryBuilder.AppendLine("select bji.* from BatchJobItems bji");
      dbQueryBuilder.AppendLine("   where bji.BatchJobId = " + (object) batchJobId);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      for (int index = 0; index < table1.Rows.Count; ++index)
        batchJobInfoByJobId = BatchJobsAndItemsAccessor.dataRowToBatchJobInfo(table1.Rows[index], table2);
      return batchJobInfoByJobId;
    }

    private static void updateTable(
      DbTableInfo tableInfo,
      DbValueList dataValues,
      DbValueList keyValues)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.IfExists(tableInfo, keyValues);
      dbQueryBuilder.Begin();
      dbQueryBuilder.Update(tableInfo, dataValues, keyValues);
      dbQueryBuilder.End();
      dbQueryBuilder.Execute(DbTransactionType.Serialized);
    }

    private static void updateBatchJobStatus(
      int batchJobId,
      string applicationChannel,
      BatchJobStatus status,
      DateTime statusDate,
      string lastModifiedByUserId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      BatchJobsAndItemsAccessor.updateTable(EllieMae.EMLite.Server.DbAccessManager.GetTable("BatchJobs"), new DbValueList()
      {
        {
          "ApplicationChannel",
          (object) applicationChannel
        },
        {
          "Status",
          (object) status
        },
        {
          "StatusDate",
          (object) statusDate
        },
        {
          "LastModifiedByUserId",
          (object) lastModifiedByUserId
        }
      }, new DbValueList()
      {
        {
          "BatchJobId",
          (object) batchJobId
        }
      });
    }

    private static DbValueList createBatchJobDbValueList(BatchJobInfo batchJob)
    {
      return new DbValueList()
      {
        {
          "CreatedBy",
          (object) batchJob.CreatedBy
        },
        {
          "CreatedDate",
          (object) batchJob.CreatedDate
        },
        {
          "ApplicationChannel",
          (object) string.Concat((object) batchJob.ApplicationChannel)
        },
        {
          "Status",
          (object) batchJob.Status
        },
        {
          "StatusDate",
          (object) batchJob.StatusDate
        },
        {
          "Type",
          (object) batchJob.Type
        },
        {
          "LastModifiedByUserId",
          (object) batchJob.LastModifiedByUserId
        },
        {
          "EntityId",
          (object) batchJob.EntityId
        },
        {
          "EntityType",
          (object) batchJob.EntityType
        },
        {
          "MetaData",
          (object) batchJob.MetaData
        },
        {
          "Result",
          (object) batchJob.Result
        }
      };
    }

    private static DbValueList createBatchJobItemDbValueList(BatchJobItemInfo batchJobItem)
    {
      return new DbValueList()
      {
        {
          "Status",
          (object) batchJobItem.Status
        },
        {
          "StatusDate",
          (object) batchJobItem.StatusDate
        },
        {
          "Action",
          (object) batchJobItem.Action
        },
        {
          "EntityId",
          (object) batchJobItem.EntityId
        },
        {
          "EntityType",
          (object) batchJobItem.EntityType
        },
        {
          "Result",
          (object) batchJobItem.Result
        },
        {
          "MetaData",
          (object) batchJobItem.MetaData
        }
      };
    }

    private static DbValueList createBatchJobItemDbValueList(
      int batchJobId,
      BatchJobItemInfo batchJobItem)
    {
      DbValueList jobItemDbValueList = BatchJobsAndItemsAccessor.createBatchJobItemDbValueList(batchJobItem);
      jobItemDbValueList.Add("BatchJobId", (object) batchJobId);
      return jobItemDbValueList;
    }

    private static BatchJobItemInfo dataRowToBatchJobItemInfo(DataRow r)
    {
      return new BatchJobItemInfo(SQL.DecodeInt(r["BatchJobItemId"]), SQL.DecodeInt(r["BatchJobId"]), (BatchJobItemStatus) SQL.DecodeInt(r["Status"]), SQL.DecodeDateTime(r["StatusDate"]), SQL.DecodeString(r["Action"]), SQL.DecodeString(r["EntityId"]), (BatchJobItemEntityType) SQL.DecodeInt(r["EntityType"]), SQL.DecodeString(r["Result"]), SQL.DecodeString(r["MetaData"]));
    }

    private static BatchJobSummaryInfo dataRowToBatchJobSummaryInfo(DataRow r)
    {
      return new BatchJobSummaryInfo()
      {
        ApplicationChannel = (BatchJobApplicationChannel) Enum.Parse(typeof (BatchJobApplicationChannel), SQL.DecodeString(r["ApplicationChannel"])),
        BatchJobId = SQL.DecodeInt(r["BatchJobId"]),
        CreatedBy = SQL.DecodeString(r["CreatedBy"]),
        CreatedDate = SQL.DecodeDateTime(r["CreatedDate"]),
        EntityId = SQL.DecodeString(r["EntityId"]),
        EntityType = (BatchJobEntityType) SQL.DecodeInt(r["EntityType"]),
        EntityName = SQL.DecodeString(r["Name"]),
        LastModifiedByUserId = SQL.DecodeString(r["LastModifiedByUserId"]),
        Status = (BatchJobStatus) SQL.DecodeInt(r["Status"]),
        StatusDate = SQL.DecodeDateTime(r["StatusDate"]),
        TotalJobItemsCount = SQL.DecodeInt(r["TotalLoans"]),
        TotalJobItemsUnProcessed = SQL.DecodeInt(r["UnProcessed"]),
        TotalJobItemsInProgress = SQL.DecodeInt(r["InProgress"]),
        TotalJobItemsCancelled = SQL.DecodeInt(r["Cancelled"]),
        TotalJobItemsErrored = SQL.DecodeInt(r["Errored"]),
        TotalJobItemsSucceeded = SQL.DecodeInt(r["Completed"])
      };
    }

    private static BatchJobInfo dataRowToBatchJobInfo(DataRow r, DataTable batchJobItemTable)
    {
      List<BatchJobItemInfo> batchJobItems = new List<BatchJobItemInfo>();
      if (batchJobItemTable != null)
      {
        foreach (DataRow r1 in batchJobItemTable.Select())
          batchJobItems.Add(BatchJobsAndItemsAccessor.dataRowToBatchJobItemInfo(r1));
      }
      return new BatchJobInfo(SQL.DecodeInt(r["BatchJobId"]), SQL.DecodeString(r["CreatedBy"]), SQL.DecodeDateTime(r["CreatedDate"]), (BatchJobApplicationChannel) Enum.Parse(typeof (BatchJobApplicationChannel), SQL.DecodeString(r["ApplicationChannel"])), (BatchJobStatus) SQL.DecodeInt(r["Status"]), SQL.DecodeDateTime(r["StatusDate"]), (BatchJobType) SQL.DecodeInt(r["Type"]), SQL.DecodeString(r["LastModifiedByUserId"]), SQL.DecodeString(r["EntityId"]), (BatchJobEntityType) SQL.DecodeInt(r["EntityType"]), SQL.DecodeString(r["MetaData"]), batchJobItems, SQL.DecodeString(r["Result"]));
    }

    private static string getColumnName(string fieldId)
    {
      string columnName;
      switch (fieldId.Trim().ToLower())
      {
        case "tradename":
          columnName = "t.Name";
          break;
        case "tradetype":
          columnName = "bj.EntityType";
          break;
        case "createddate":
          columnName = "bj.CreatedDate";
          break;
        case "createdby":
          columnName = "bj.CreatedBy";
          break;
        case "status":
          columnName = "bj.Status";
          break;
        default:
          columnName = string.Empty;
          break;
      }
      return columnName;
    }
  }
}
