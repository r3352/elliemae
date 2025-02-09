// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.ConcurrentUpdateAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Common;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class ConcurrentUpdateAccessor
  {
    private const string ClassName = "DocumentBatchUpdateAccessor�";
    private const string ConcurrentUpdateTable = "LoanConcurrentUpdate�";
    private const string ConcurrentStageUpdateTable = "LoanConcurrentUpdateProperties�";
    private const string LoanXRefTable = "LoanXRef�";
    private const string DocumentLogElementName = "Record�";
    private static readonly string[] ColumnNames = new string[8]
    {
      "RecordId",
      "LoanXRefId",
      "LoanGuid",
      "ActionType",
      "Data",
      "CreatedOn",
      "SequenceNumber",
      "LoanHistoryData"
    };
    public const string LoanXRefIdNotFoundError = "The LoanXRefId was not found for loanId = {0}�";
    public const string DocumentBatchNotFoundError = "No records found for recordId = {0}�";

    public static long InsertDataToStaging(
      ConcurrentUpdateModel concurrentUpdateModel,
      double lockTimeout = 60.0)
    {
      int loanXrefId = ConcurrentUpdateAccessor.GetLoanXRefId(concurrentUpdateModel.LoanGuid);
      if (loanXrefId.Equals(-1))
      {
        string message = string.Format("The LoanXRefId was not found for loanId = {0}", (object) concurrentUpdateModel.LoanGuid);
        TraceLog.WriteVerbose("DocumentBatchUpdateAccessor", message);
        throw new Exception(message);
      }
      using (SafeMutex safeMutex = new SafeMutex((IClientContext) ClientContext.GetCurrent(), "DocumentBatchUpdateAccessor", MutexAccess.Write))
      {
        if (!safeMutex.WaitOne(TimeSpan.FromSeconds(lockTimeout)))
          throw new LockException("Timeout while waiting to acquire lock on Document Batch Update. You must resubmit this document at a later time.");
        long nextSequenceNumber = ConcurrentUpdateAccessor.GetNextSequenceNumber(concurrentUpdateModel.LoanGuid);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("LoanConcurrentUpdateProperties");
        DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("LoanConcurrentUpdate");
        concurrentUpdateModel.LoanXRefId = loanXrefId;
        concurrentUpdateModel.SequenceNumber = nextSequenceNumber;
        DbValueList updateDbValueList = ConcurrentUpdateAccessor.CreateConcurrentUpdateDbValueList(concurrentUpdateModel);
        dbQueryBuilder.Declare("@RecordId", "bigint");
        dbQueryBuilder.InsertInto(table2, updateDbValueList, true, false);
        dbQueryBuilder.SelectIdentity("@RecordId");
        dbQueryBuilder.Select("@RecordId");
        DbValueList values = new DbValueList()
        {
          {
            "recordId",
            (object) "@RecordId",
            (IDbEncoder) DbEncoding.None
          },
          {
            "attribute",
            (object) concurrentUpdateModel.MergeParamKeyValues?.Keys
          },
          {
            "value",
            (object) concurrentUpdateModel.MergeParamKeyValues?.Values
          }
        };
        dbQueryBuilder.InsertInto(table1, values, true, false);
        return (long) SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
      }
    }

    public static void DeleteDocumentBatchUpdate(long concurrentUpdateId)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("LoanConcurrentUpdate");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.DeleteFrom(table, new DbValueList()
      {
        {
          "RecordId",
          (object) concurrentUpdateId
        }
      });
      dbQueryBuilder.Execute();
    }

    public static void DeleteAllConcurrentUpdates(string loanGuid, List<long> sequenceNumberList)
    {
      if (string.IsNullOrWhiteSpace(loanGuid) || sequenceNumberList == null || sequenceNumberList.Count.Equals(0))
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLineFormat("DELETE FROM {0} where LoanGuid = '{1}' AND SequenceNumber IN ({2})", (object) "LoanConcurrentUpdate", (object) loanGuid, (object) string.Join<long>(",", (IEnumerable<long>) sequenceNumberList));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteConcurrentUpdates(string loanGuid, long sequenceNumber)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLineFormat("DELETE FROM {0} where LoanGuid = '{1}' AND SequenceNumber = '{2}'", (object) "LoanConcurrentUpdate", (object) loanGuid, (object) sequenceNumber);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteAllConcurrentUpdates(List<long> recordIdList)
    {
      if (recordIdList == null || recordIdList.Count.Equals(0))
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLineFormat("DELETE FROM {0} where RecordId IN ({1})", (object) "LoanConcurrentUpdate", (object) string.Join<long>(",", (IEnumerable<long>) recordIdList));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static ConcurrentUpdateModel GetDocumentBatchUpdateById(
      LogList logList,
      long concurrentUpdateId)
    {
      try
      {
        DbValueList keys = new DbValueList()
        {
          {
            "RecordId",
            (object) concurrentUpdateId
          }
        };
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("LoanConcurrentUpdate"), ConcurrentUpdateAccessor.ColumnNames, keys);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection == null || dataRowCollection.Count != 1 ? (ConcurrentUpdateModel) null : ConcurrentUpdateAccessor.ToConcurrentUpdate(dataRowCollection[0], logList);
      }
      catch (Exception ex)
      {
        TraceLog.WriteException("DocumentBatchUpdateAccessor", ex);
        return (ConcurrentUpdateModel) null;
      }
    }

    public static List<ConcurrentUpdateModel> GetConcurrentUpdatesByLoanId(
      LogList logList,
      string loanGuid)
    {
      try
      {
        DbValueList keys = new DbValueList()
        {
          {
            "LoanGuid",
            (object) loanGuid
          }
        };
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("LoanConcurrentUpdate"), ConcurrentUpdateAccessor.ColumnNames, keys);
        DataRowCollection source = dbQueryBuilder.Execute();
        return source == null ? (List<ConcurrentUpdateModel>) null : (source.Count < 1 ? (List<ConcurrentUpdateModel>) new EmList<ConcurrentUpdateModel>() : source.Cast<DataRow>().Select<DataRow, ConcurrentUpdateModel>((System.Func<DataRow, ConcurrentUpdateModel>) (row => ConcurrentUpdateAccessor.ToConcurrentUpdate(row, logList))).ToList<ConcurrentUpdateModel>());
      }
      catch (Exception ex)
      {
        TraceLog.WriteException("DocumentBatchUpdateAccessor", ex);
        return (List<ConcurrentUpdateModel>) null;
      }
    }

    public static List<ConcurrentUpdateModel> GetConcurrentUpdatesByLoanId(string loanGuid)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select lcu.*,lcup.* from LoanConcurrentUpdate lcu LEFT JOIN LoanConcurrentUpdateProperties lcup ON lcu.RecordId = lcup.recordId where ([LoanGuid] = " + SQL.Encode((object) loanGuid) + ")");
        DataRowCollection source = dbQueryBuilder.Execute();
        return source == null ? (List<ConcurrentUpdateModel>) null : (source.Count < 1 ? (List<ConcurrentUpdateModel>) new EmList<ConcurrentUpdateModel>() : source.Cast<DataRow>().GroupBy<DataRow, string>((System.Func<DataRow, string>) (row => row["recordId"].ToString())).Select<IGrouping<string, DataRow>, ConcurrentUpdateModel>((System.Func<IGrouping<string, DataRow>, ConcurrentUpdateModel>) (egroup => ConcurrentUpdateAccessor.ToDocumentBatchUpdate(egroup.ToList<DataRow>()))).ToList<ConcurrentUpdateModel>());
      }
      catch (Exception ex)
      {
        TraceLog.WriteException("DocumentBatchUpdateAccessor", ex);
        return (List<ConcurrentUpdateModel>) null;
      }
    }

    public static List<ConcurrentUpdateModel> GetConcurrentUpdatesByLoanIdAndActionid(
      LogList logList,
      string loanGuid,
      ConcurrentUpdateActionType actionType)
    {
      try
      {
        DbValueList keys = new DbValueList()
        {
          {
            "LoanGuid",
            (object) loanGuid
          },
          {
            "ActionType",
            (object) (byte) actionType.GetHashCode()
          }
        };
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("LoanConcurrentUpdate"), ConcurrentUpdateAccessor.ColumnNames, keys);
        DataRowCollection source = dbQueryBuilder.Execute();
        return source == null ? (List<ConcurrentUpdateModel>) null : (source.Count < 1 ? (List<ConcurrentUpdateModel>) new EmList<ConcurrentUpdateModel>() : source.Cast<DataRow>().Select<DataRow, ConcurrentUpdateModel>((System.Func<DataRow, ConcurrentUpdateModel>) (row => ConcurrentUpdateAccessor.ToConcurrentUpdate(row, logList))).ToList<ConcurrentUpdateModel>());
      }
      catch (Exception ex)
      {
        TraceLog.WriteException("DocumentBatchUpdateAccessor", ex);
        return (List<ConcurrentUpdateModel>) null;
      }
    }

    public static bool IsLoanLocked(string loanGuid)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Append("select 1 from LoanLock where Guid = " + SQL.Encode((object) loanGuid) + " and lockedfor <> 0");
        return dbQueryBuilder.Execute().Count != 0;
      }
      catch (Exception ex)
      {
        TraceLog.WriteException("DocumentBatchUpdateAccessor", ex);
        return true;
      }
    }

    public static ConcurrentUpdateModel GetConcurrentchUpdateByLoandIdAndSequenceId(
      LogList logList,
      string loanGuid,
      long sequenceId)
    {
      try
      {
        DbValueList keys = new DbValueList()
        {
          {
            "LoanGuid",
            (object) loanGuid
          },
          {
            "SequenceNumber",
            (object) sequenceId
          }
        };
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("LoanConcurrentUpdate"), ConcurrentUpdateAccessor.ColumnNames, keys);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection == null || dataRowCollection.Count != 1 ? (ConcurrentUpdateModel) null : ConcurrentUpdateAccessor.ToConcurrentUpdate(dataRowCollection[0], logList);
      }
      catch (Exception ex)
      {
        TraceLog.WriteException("DocumentBatchUpdateAccessor", ex);
        return (ConcurrentUpdateModel) null;
      }
    }

    public static ConcurrentUpdateModel GetConcurrentUpdateByLoandIdAndSequenceId(
      string loanGuid,
      long sequenceId)
    {
      try
      {
        DbValueList keys = new DbValueList()
        {
          {
            "LoanGuid",
            (object) loanGuid
          },
          {
            "SequenceNumber",
            (object) sequenceId
          }
        };
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("LoanConcurrentUpdate"), ConcurrentUpdateAccessor.ColumnNames, keys);
        DataRowCollection source = dbQueryBuilder.Execute();
        return source == null || source.Count != 1 ? (ConcurrentUpdateModel) null : ConcurrentUpdateAccessor.ToDocumentBatchUpdate((List<DataRow>) source.Cast<DataRow>());
      }
      catch (Exception ex)
      {
        TraceLog.WriteException("DocumentBatchUpdateAccessor", ex);
        return (ConcurrentUpdateModel) null;
      }
    }

    public static bool IsConcurrentUpdateDataExists(string loanGuid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT top(1) 1 FROM LoanConcurrentUpdate WHERE LoanGuid =" + SQL.Encode((object) loanGuid));
      return Convert.ToInt16(dbQueryBuilder.ExecuteScalar()) > (short) 0;
    }

    private static ConcurrentUpdateModel ToConcurrentUpdate(DataRow row, LogList logList)
    {
      long num1 = (long) row["RecordId"];
      int num2 = (int) row["LoanXRefId"];
      DateTime dateTime = (DateTime) row["CreatedOn"];
      long num3 = (long) row["SequenceNumber"];
      string str1 = row["LoanGuid"] as string;
      string str2 = row["Data"] as string;
      ConcurrentUpdateActionType updateActionType = (ConcurrentUpdateActionType) (byte) row["ActionType"];
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml((string) row["data"]);
      XmlElement documentElement = xmlDocument.DocumentElement;
      return new ConcurrentUpdateModel()
      {
        RecordId = num1,
        LoanXRefId = num2,
        CreatedOn = dateTime,
        SequenceNumber = num3,
        XmlStr = str2,
        LoanGuid = str1,
        ActionType = updateActionType
      };
    }

    private static ConcurrentUpdateModel ToDocumentBatchUpdate(List<DataRow> concurrentData)
    {
      DataRow dataRow1 = concurrentData.First<DataRow>();
      long num1 = (long) dataRow1["RecordId"];
      int num2 = (int) dataRow1["LoanXRefId"];
      DateTime dateTime = (DateTime) dataRow1["CreatedOn"];
      long num3 = (long) dataRow1["SequenceNumber"];
      string str1 = dataRow1["LoanGuid"] as string;
      ConcurrentUpdateActionType updateActionType = (ConcurrentUpdateActionType) (byte) dataRow1["ActionType"];
      string str2 = (string) dataRow1["data"];
      string str3 = Convert.ToString(dataRow1["LoanHistoryData"]);
      Dictionary<string, object> dictionary = (Dictionary<string, object>) null;
      foreach (DataRow dataRow2 in concurrentData)
      {
        dictionary = dictionary ?? new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
        string key = Convert.ToString(dataRow2["attribute"]);
        if (!string.IsNullOrEmpty(key) && !dictionary.ContainsKey(key))
        {
          string str4 = Convert.ToString(dataRow1["value"]);
          dictionary.Add(key, (object) str4);
        }
      }
      return new ConcurrentUpdateModel()
      {
        RecordId = num1,
        LoanXRefId = num2,
        CreatedOn = dateTime,
        SequenceNumber = num3,
        XmlStr = str2,
        LoanGuid = str1,
        ActionType = updateActionType,
        XmlHistoryStr = str3,
        MergeParamKeyValues = dictionary
      };
    }

    private static int GetLoanXRefId(string loanGuid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("LoanXRef"), new DbValueList()
      {
        {
          "LoanGuid",
          (object) loanGuid
        }
      });
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return !dataRowCollection.Count.Equals(0) ? (int) SQL.Decode(dataRowCollection[0]["XrefID"], (object) -1) : -1;
    }

    private static long GetNextSequenceNumber(string loanGuid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("DECLARE @LoanId AS NVARCHAR(38)");
      dbQueryBuilder.AppendLineFormat("SET @LoanId = '{0}'", (object) loanGuid);
      dbQueryBuilder.AppendLineFormat("SELECT MAX(SequenceNumber) AS SequenceNumber FROM {0}", (object) "LoanConcurrentUpdate");
      dbQueryBuilder.AppendLine(" WHERE LoanGuid =  @LoanId");
      long result;
      return !long.TryParse(dbQueryBuilder.ExecuteScalar().ToString(), out result) ? DateTime.Now.Ticks : result + 1L;
    }

    private static DbValueList CreateConcurrentUpdateDbValueList(
      ConcurrentUpdateModel concurrentBatchUpdate)
    {
      return new DbValueList()
      {
        {
          "LoanXRefId",
          (object) concurrentBatchUpdate.LoanXRefId
        },
        {
          "LoanGuid",
          (object) concurrentBatchUpdate.LoanGuid
        },
        {
          "ActionType",
          (object) (int) concurrentBatchUpdate.ActionType
        },
        {
          "Data",
          (object) concurrentBatchUpdate.XmlStr
        },
        {
          "CreatedOn",
          (object) DateTime.UtcNow,
          (IDbEncoder) DbEncoding.DateTime
        },
        {
          "SequenceNumber",
          (object) concurrentBatchUpdate.SequenceNumber
        },
        {
          "LoanHistoryData",
          (object) concurrentBatchUpdate.XmlHistoryStr
        }
      };
    }
  }
}
