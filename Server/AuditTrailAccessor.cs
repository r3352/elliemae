// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.AuditTrailAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class AuditTrailAccessor
  {
    private const string className = "AuditTrailAccessor�";
    private const string tableName = "AuditTrail_�";

    private AuditTrailAccessor()
    {
    }

    public static AuditRecord[] GetAuditRecords(string guid, string[] fieldIds)
    {
      if (fieldIds == null || fieldIds.Length == 0)
        return new AuditRecord[0];
      LoanXDBTableList loanXdbTableList = LoanXDBStore.GetLoanXDBTableList();
      Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
      string str = "";
      foreach (string fieldId in fieldIds)
      {
        LoanXDBField field = loanXdbTableList.GetField(fieldId);
        if (field == null)
          throw new ObjectNotFoundException("The field '" + fieldId + "' is not in the Audit Trail.", ObjectType.Field, (object) fieldId);
        if (str.Length > 0)
          str += ",";
        str += (string) (object) field.FieldXRefID;
        if (!dictionary.ContainsKey(field.FieldXRefID))
          dictionary[field.FieldXRefID] = new List<string>();
        dictionary[field.FieldXRefID].Add(fieldId);
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select atr.*, lxr.LoanGuid, u.first_name, u.last_name from AuditTrail atr");
      dbQueryBuilder.AppendLine("  inner join LoanXRef lxr on atr.LoanXRef = lxr.XRefID");
      dbQueryBuilder.AppendLine("  left outer join users u on atr.UserID = u.userid");
      dbQueryBuilder.AppendLine("where lxr.LoanGuid = " + SQL.Encode((object) guid));
      dbQueryBuilder.AppendLine("  and atr.FieldXRef in (" + str + ")");
      try
      {
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        List<AuditRecord> auditRecordList = new List<AuditRecord>();
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          int key = SQL.DecodeInt(row["FieldXRef"]);
          if (dictionary.ContainsKey(key))
          {
            foreach (string fieldId in dictionary[key])
              auditRecordList.Add(AuditTrailAccessor.getAuditRecord(row, fieldId));
          }
        }
        return auditRecordList.ToArray();
      }
      catch
      {
        return new AuditRecord[0];
      }
    }

    public static AuditRecord[] GetAuditRecords(string guid, string fieldId)
    {
      return AuditTrailAccessor.GetAuditRecords(guid, new List<string>()
      {
        fieldId
      }.ToArray());
    }

    public static void InsertLegacyAuditRecord(
      LoanXDBField field,
      LoanIdentity loanId,
      UserInfo currentUser,
      string priorValue,
      string newValue)
    {
      DbTableInfo dynamicTable = DbAccessManager.GetDynamicTable(field.LegacyAuditTableName);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValueList trailDbValueList = AuditTrailAccessor.createLegacyAuditTrailDbValueList(loanId.XrefId, loanId.Guid, newValue, priorValue, currentUser);
      dbQueryBuilder.AppendLine("Update " + field.LegacyAuditTableName + " set LastModified = 0 where Guid = " + SQL.Encode((object) loanId.Guid) + " and LastModified = 1");
      dbQueryBuilder.InsertInto(dynamicTable, trailDbValueList, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static DbValueList createLegacyAuditTrailDbValueList(
      int xrefId,
      string guid,
      string fieldValue,
      string originalValue,
      UserInfo currentUser)
    {
      return new DbValueList()
      {
        {
          "XrefId",
          (object) xrefId
        },
        {
          "Guid",
          (object) guid
        },
        {
          "UserID",
          (object) currentUser.Userid
        },
        {
          "FirstName",
          (object) currentUser.FirstName
        },
        {
          "LastName",
          (object) currentUser.LastName
        },
        {
          "Data",
          (object) fieldValue
        },
        {
          "ModifiedDTTM",
          (object) DateTime.Now
        },
        {
          "LastModified",
          (object) 1
        },
        {
          "PreviousData",
          (object) originalValue
        }
      };
    }

    public static void InsertAuditRecord(
      int loanXRef,
      int fieldXRef,
      UserInfo currentUser,
      object priorValue,
      object newValue,
      DateTime timestamp)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      AuditTrailAccessor.AppendAuditRecord(sql, loanXRef, fieldXRef, currentUser, priorValue, newValue, timestamp, (string) null);
      sql.ExecuteNonQuery();
    }

    internal static void AppendAuditRecord(
      DbQueryBuilder sql,
      int loanXRef,
      int fieldXRef,
      UserInfo currentUser,
      object priorValue,
      object newValue,
      DateTime timestamp,
      string auditUserId)
    {
      DbTableInfo table = DbAccessManager.GetTable("AuditTrail");
      DbValueList values = new DbValueList();
      values.Add("LoanXRef", (object) loanXRef);
      values.Add("FieldXRef", (object) fieldXRef);
      values.Add("UserID", auditUserId == null ? (object) currentUser.Userid : (object) auditUserId);
      values.Add("Data", newValue);
      values.Add("PreviousData", priorValue);
      values.Add("ModifiedDTTM", (object) timestamp);
      values.Add("IsCurrent", (object) 1);
      sql.AppendLine("Update AuditTrail set IsCurrent = 0 where LoanXRef = " + (object) loanXRef + " and FieldXRef = " + (object) fieldXRef + " and IsCurrent = 1");
      sql.InsertInto(table, values, true, false);
    }

    public static void AppendAuditRecord(
      Dictionary<int, DbValueList> auditTrailValueDictionary,
      LoanIdentity loanIdentity)
    {
      try
      {
        if (!(auditTrailValueDictionary != null & auditTrailValueDictionary.Any<KeyValuePair<int, DbValueList>>()))
          return;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        string str = string.Join(",", auditTrailValueDictionary.Keys.Select<int, string>((System.Func<int, string>) (c => SQL.Encode((object) c))));
        DbTableInfo table = DbAccessManager.GetTable("AuditTrail");
        dbQueryBuilder.AppendLine(string.Format("Update AuditTrail set IsCurrent = 0 where LoanXRef = {0} and FieldXRef IN ({1}) and IsCurrent = 1 ", (object) loanIdentity.XrefId, (object) str));
        bool flag = true;
        string[] strArray1 = (string[]) null;
        List<string> values = new List<string>();
        foreach (DbValueList dbValueList in auditTrailValueDictionary.Values)
        {
          int count = dbValueList.Count;
          string[] strArray2 = new string[count];
          strArray1 = strArray1 ?? new string[count];
          for (int index = 0; index < count; ++index)
          {
            string columnName = dbValueList[index].ColumnName;
            if (flag)
              strArray1[index] = columnName;
            strArray2[index] = dbValueList[index].Encode(table[columnName]);
          }
          flag = false;
          values.Add(string.Format("({0})", (object) string.Join(", ", strArray2)));
        }
        string text = " ([" + string.Join("], [", strArray1) + "])";
        dbQueryBuilder.AppendLine("INSERT into " + table.Name);
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.AppendLine(string.Format("SELECT * FROM (values {0} )tempValues{1}", (object) string.Join(",\n", (IEnumerable<string>) values), (object) text));
        using (PerformanceMeter.Current.BeginOperation("InsertAuditRecords"))
          dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (AuditTrailAccessor), "Error updating audit trail fields in loan " + loanIdentity.Guid + ": " + (object) ex);
      }
    }

    public static DbValueList GenerateAuditRecordDbValueList(
      int loanXRef,
      int fieldXRef,
      UserInfo currentUser,
      object priorValue,
      object newValue,
      DateTime timestamp,
      string auditUserId)
    {
      return new DbValueList()
      {
        {
          "LoanXRef",
          (object) loanXRef
        },
        {
          "FieldXRef",
          (object) fieldXRef
        },
        {
          "UserID",
          (object) (auditUserId ?? currentUser.Userid)
        },
        {
          "Data",
          newValue,
          (IDbEncoder) DbEncoding.SqlVariant
        },
        {
          "PreviousData",
          priorValue,
          (IDbEncoder) DbEncoding.SqlVariant
        },
        {
          "ModifiedDTTM",
          (object) timestamp
        },
        {
          "IsCurrent",
          (object) 1
        }
      };
    }

    private static AuditRecord getAuditRecord(DataRow row, string fieldId)
    {
      int recordId = SQL.DecodeInt(row["RecordID"]);
      DateTime modifiedDTTM = DateTime.Parse(string.Concat(row["ModifiedDTTM"]));
      string userID = string.Concat(row["UserID"]);
      string firstName = string.Concat(row["First_Name"]);
      string lastName = string.Concat(row["Last_Name"]);
      string loanGuid = string.Concat(row["LoanGuid"]);
      object dataValue = SQL.Decode(row["Data"]);
      object previousValue = SQL.Decode(row["PreviousData"]);
      return new AuditRecord(recordId, fieldId, loanGuid, userID, firstName, lastName, dataValue, previousValue, modifiedDTTM);
    }

    public static Dictionary<string, AuditRecord> GetLastAuditRecords(string guid)
    {
      Dictionary<string, AuditRecord> lastAuditRecords = new Dictionary<string, AuditRecord>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select atr.*, lxr.LoanGuid, fxr.FieldID, fxr.PairIndex, u.first_name, u.last_name from AuditTrail atr");
      dbQueryBuilder.AppendLine("  inner join LoanXRef lxr on atr.LoanXRef = lxr.XRefID");
      dbQueryBuilder.AppendLine("  inner join FieldXRef fxr on atr.FieldXRef = fxr.XRefID");
      dbQueryBuilder.AppendLine("  left outer join users u on atr.UserID = u.userid");
      dbQueryBuilder.AppendLine("where lxr.LoanGuid = " + SQL.Encode((object) guid));
      dbQueryBuilder.AppendLine("  and IsCurrent = 1");
      foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        string str = SQL.DecodeString(row["FieldID"]);
        int pairIndex = SQL.DecodeInt(row["PairIndex"], -1);
        FieldDefinition field = EncompassFields.GetField(str);
        if (field != null && field.RequiresBorrowerPredicate)
          str = FieldPairParser.GetFieldIDForBorrowerPair(str, pairIndex);
        lastAuditRecords[str] = AuditTrailAccessor.getAuditRecord(row, str);
      }
      return lastAuditRecords;
    }

    public static Dictionary<string, AuditRecord> GetLastAuditRecords(
      string guid,
      string[] fieldIds)
    {
      Dictionary<string, AuditRecord> lastAuditRecords = new Dictionary<string, AuditRecord>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      LoanXDBTableList loanXdbTableList = LoanXDBStore.GetLoanXDBTableList();
      List<int> intList = new List<int>();
      foreach (string fieldId in fieldIds)
      {
        LoanXDBField field = loanXdbTableList.GetField(fieldId);
        if (field != null)
          intList.Add(field.FieldXRefID);
      }
      if (intList.Count > 0)
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select atr.*, lxr.LoanGuid, fxr.FieldID, fxr.PairIndex, u.first_name, u.last_name from AuditTrail atr");
        dbQueryBuilder.AppendLine("  inner join LoanXRef lxr on atr.LoanXRef = lxr.XRefID");
        dbQueryBuilder.AppendLine("  inner join FieldXRef fxr on atr.FieldXRef = fxr.XRefID");
        dbQueryBuilder.AppendLine("  left outer join users u on atr.UserID = u.userid");
        dbQueryBuilder.AppendLine("where lxr.LoanGuid = " + SQL.Encode((object) guid));
        dbQueryBuilder.AppendLine("  and atr.FieldXRef in (" + SQL.Encode((object) intList.ToArray()) + ")");
        dbQueryBuilder.AppendLine("  and IsCurrent = 1");
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          string str = SQL.DecodeString(row["FieldID"]);
          int pairIndex = SQL.DecodeInt(row["PairIndex"], 0);
          FieldDefinition field = EncompassFields.GetField(str);
          if (field != null && field.RequiresBorrowerPredicate)
            str = FieldPairParser.GetFieldIDForBorrowerPair(str, pairIndex);
          lastAuditRecords.Add(str, AuditTrailAccessor.getAuditRecord(row, str));
        }
      }
      return lastAuditRecords;
    }

    public static List<AuditRecord> GetAuditTrailRecords(
      string guid,
      Dictionary<int, string> fieldXRefIDs,
      bool includeHistoricalData,
      out int totalCount,
      int start = 0,
      int limit = 10)
    {
      totalCount = 0;
      List<AuditRecord> auditTrailRecords = new List<AuditRecord>();
      if (fieldXRefIDs.Any<KeyValuePair<int, string>>())
      {
        bool flag = start >= 0 && limit > 0;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select atr.*, lxr.LoanGuid, ROW_NUMBER() OVER(PARTITION BY FieldXRef ORDER BY ModifiedDTTM DESC) AS RowNum,  u.first_name, u.last_name from AuditTrail atr");
        dbQueryBuilder.AppendLine("  inner join LoanXRef lxr on atr.LoanXRef = lxr.XRefID");
        dbQueryBuilder.AppendLine("  left outer join users u on atr.UserID = u.userid");
        dbQueryBuilder.AppendLine("where lxr.LoanGuid = " + SQL.Encode((object) guid));
        dbQueryBuilder.AppendLine("  and atr.FieldXRef in (" + SQL.Encode((object) fieldXRefIDs.Keys.ToArray<int>()) + ")");
        if (!includeHistoricalData)
          dbQueryBuilder.AppendLine("  and IsCurrent = 1");
        DataRowCollection dataRowCollection = (DataRowCollection) null;
        if (flag)
        {
          DataTable paginatedRecords = new DbQueryBuilder().GetPaginatedRecords(dbQueryBuilder.ToString(), start + 1, start + limit, (List<SortColumn>) null);
          if (paginatedRecords != null && paginatedRecords.Rows != null && paginatedRecords.Rows.Count > 0)
          {
            dataRowCollection = paginatedRecords.Rows;
            totalCount = SQL.DecodeInt(dataRowCollection[0]["TotalRowCount"]);
          }
        }
        else
        {
          dataRowCollection = dbQueryBuilder.Execute();
          totalCount = dataRowCollection.Count;
        }
        if (dataRowCollection != null)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
          {
            int key = SQL.DecodeInt(row["FieldXRef"]);
            string fieldXrefId = fieldXRefIDs[key];
            auditTrailRecords.Add(AuditTrailAccessor.getAuditRecord(row, fieldXrefId));
          }
        }
      }
      return auditTrailRecords;
    }
  }
}
