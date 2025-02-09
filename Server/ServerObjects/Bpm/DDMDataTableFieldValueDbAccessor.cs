// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.DDMDataTableFieldValueDbAccessor
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
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class DDMDataTableFieldValueDbAccessor
  {
    private static readonly DbTableInfo DataTableFieldValue = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTableFields");

    public static int CreateDDMDataTableField(DDMDataTableFieldValue ddmDataTableField)
    {
      DbValueList values = new DbValueList()
      {
        {
          "criteria",
          (object) ddmDataTableField.Criteria
        },
        {
          "columnId",
          (object) ddmDataTableField.ColumnId
        },
        {
          "dataTableID",
          (object) ddmDataTableField.DataTableId
        },
        {
          "fieldId",
          (object) ddmDataTableField.FieldId
        },
        {
          "isOutput",
          (object) SQL.EncodeFlag(ddmDataTableField.IsOutput)
        },
        {
          "rowId",
          (object) ddmDataTableField.RowId
        },
        {
          "values",
          (object) ddmDataTableField.Values
        }
      };
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@dataTableFieldID", "int");
      dbQueryBuilder.InsertInto(DDMDataTableFieldValueDbAccessor.DataTableFieldValue, values, true, false);
      dbQueryBuilder.SelectIdentity("@dataTableFieldID");
      dbQueryBuilder.Select("@dataTableFieldID");
      object obj = dbQueryBuilder.ExecuteScalar();
      ddmDataTableField.Id = Convert.ToInt32(obj);
      return ddmDataTableField.Id;
    }

    public static void CreateDDMDataTableFields(
      int dataTableId,
      Dictionary<int, List<DDMDataTableFieldValue>> fieldValues)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DataTable dataTable = new DataTable(DDMDataTableFieldValueDbAccessor.DataTableFieldValue.Name);
      DDMDataTableFieldValueDbAccessor.createTableSchema(dataTable);
      foreach (KeyValuePair<int, List<DDMDataTableFieldValue>> fieldValue in fieldValues)
      {
        List<DDMDataTableFieldValue> dataTableFieldValueList = fieldValue.Value;
        int count = dataTableFieldValueList.Count;
        for (int index = 0; index < count; ++index)
        {
          DDMDataTableFieldValue dataTableFieldValue = dataTableFieldValueList[index];
          dataTableFieldValue.DataTableId = dataTableId;
          DataRow row = dataTable.NewRow();
          row["criteria"] = (object) dataTableFieldValue.Criteria;
          row["columnId"] = (object) dataTableFieldValue.ColumnId;
          row["dataTableID"] = (object) dataTableFieldValue.DataTableId;
          row["fieldId"] = (object) dataTableFieldValue.FieldId;
          row["isOutput"] = (object) dataTableFieldValue.IsOutput;
          row["rowId"] = (object) dataTableFieldValue.RowId;
          row["values"] = (object) dataTableFieldValue.Values;
          dataTable.Rows.Add(row);
        }
      }
      dataTable.AcceptChanges();
      dbQueryBuilder.DoBulkCopy(DDMDataTableFieldValueDbAccessor.DataTableFieldValue.Name, dataTable);
    }

    private static void createTableSchema(DataTable dataTable)
    {
      dataTable.Columns.Add(new DataColumn("dataTableFieldID", typeof (int))
      {
        AutoIncrement = true
      });
      dataTable.Columns.Add(new DataColumn("dataTableID", typeof (int)));
      dataTable.Columns.Add(new DataColumn("rowId", typeof (int)));
      dataTable.Columns.Add(new DataColumn("columnId", typeof (int)));
      dataTable.Columns.Add(new DataColumn("fieldId", typeof (string)));
      dataTable.Columns.Add(new DataColumn("values", typeof (string)));
      dataTable.Columns.Add(new DataColumn("criteria", typeof (int)));
      dataTable.Columns.Add(new DataColumn("isOutput", typeof (bool)));
    }

    public static int UpdateDDMDataTableField(DDMDataTableFieldValue ddmDataTableField)
    {
      try
      {
        DbValueList values = new DbValueList()
        {
          {
            "criteria",
            (object) ddmDataTableField.Criteria
          },
          {
            "columnId",
            (object) ddmDataTableField.ColumnId
          },
          {
            "dataTableID",
            (object) ddmDataTableField.DataTableId
          },
          {
            "fieldId",
            (object) ddmDataTableField.FieldId
          },
          {
            "isOutput",
            (object) SQL.EncodeFlag(ddmDataTableField.IsOutput)
          },
          {
            "rowId",
            (object) ddmDataTableField.RowId
          },
          {
            "values",
            (object) ddmDataTableField.Values
          }
        };
        DbValue key = new DbValue("dataTableFieldID", (object) ddmDataTableField.Id, (IDbEncoder) DbEncoding.None);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Update(DDMDataTableFieldValueDbAccessor.DataTableFieldValue, values, key);
        dbQueryBuilder.Execute();
      }
      catch
      {
        return 0;
      }
      return ddmDataTableField.Id;
    }

    public static void DeleteDDMDataTableField(DDMDataTableFieldValue ddmDataTableField)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue("dataTableFieldID", (object) ddmDataTableField.Id, (IDbEncoder) DbEncoding.None);
      dbQueryBuilder.DeleteFrom(DDMDataTableFieldValueDbAccessor.DataTableFieldValue, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static int[] AtomicDataTableChange(
      List<DDMDataTableFieldValue> batchCreationList,
      List<DDMDataTableFieldValue> batchUpdateList,
      List<DDMDataTableFieldValue> batchDeletionList)
    {
      List<string> tmpTables = new List<string>();
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      sql.AppendLine("begin transaction ");
      if (batchDeletionList.Count > 0)
        DDMDataTableFieldValueDbAccessor.BatchDeleteDDMDataTableFields(sql, batchDeletionList);
      sql.AppendLine("");
      List<string> lsGuids = new List<string>();
      if (batchUpdateList.Count > 0)
        lsGuids = DDMDataTableFieldValueDbAccessor.BatchUpdateDDMDataTableFields(sql, batchUpdateList, tmpTables);
      sql.AppendLine("");
      try
      {
        if (batchCreationList.Count > 0)
          DDMDataTableFieldValueDbAccessor.BatchCreateDDMDataTableFields(sql, batchCreationList);
        sql.AppendLine("  commit");
        sql.ToString();
        sql.Execute();
      }
      catch (Exception ex)
      {
        if (lsGuids != null && lsGuids.Count > 0)
          DDMDataTableFieldValueDbAccessor.CleanTempTables(lsGuids);
        throw ex;
      }
      return batchCreationList.Count > 0 ? DDMDataTableFieldValueDbAccessor.FetchDataTableFieldIdsForCreatedCells(batchCreationList) : new int[0];
    }

    public static void BatchCreateDDMDataTableFields(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      List<DDMDataTableFieldValue> ddmDataTableFieldValues)
    {
      DataTable dataTable = new DataTable(DDMDataTableFieldValueDbAccessor.DataTableFieldValue.Name);
      DDMDataTableFieldValueDbAccessor.createTableSchema(dataTable);
      List<DDMDataTableFieldValue> dataTableFieldValueList = ddmDataTableFieldValues;
      int count = dataTableFieldValueList.Count;
      for (int index = 0; index < count; ++index)
      {
        DDMDataTableFieldValue dataTableFieldValue = dataTableFieldValueList[index];
        DataRow row = dataTable.NewRow();
        row["criteria"] = (object) dataTableFieldValue.Criteria;
        row["columnId"] = (object) dataTableFieldValue.ColumnId;
        row["dataTableID"] = (object) dataTableFieldValue.DataTableId;
        row["fieldId"] = (object) dataTableFieldValue.FieldId;
        row["isOutput"] = (object) dataTableFieldValue.IsOutput;
        row["rowId"] = (object) dataTableFieldValue.RowId;
        row["values"] = (object) dataTableFieldValue.Values;
        dataTable.Rows.Add(row);
      }
      dataTable.AcceptChanges();
      sql.DoBulkCopy(DDMDataTableFieldValueDbAccessor.DataTableFieldValue.Name, dataTable);
    }

    private static void CreateMiniBatchHelper(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      List<DDMDataTableFieldValue> ddmDataTableFieldValues)
    {
      List<DbValueList> values = new List<DbValueList>(ddmDataTableFieldValues.Count);
      foreach (DDMDataTableFieldValue dataTableFieldValue in ddmDataTableFieldValues)
      {
        DbValueList dbValueList = new DbValueList()
        {
          {
            "criteria",
            (object) dataTableFieldValue.Criteria
          },
          {
            "columnId",
            (object) dataTableFieldValue.ColumnId
          },
          {
            "dataTableID",
            (object) dataTableFieldValue.DataTableId
          },
          {
            "fieldId",
            (object) dataTableFieldValue.FieldId
          },
          {
            "isOutput",
            (object) SQL.EncodeFlag(dataTableFieldValue.IsOutput)
          },
          {
            "rowId",
            (object) dataTableFieldValue.RowId
          },
          {
            "values",
            (object) dataTableFieldValue.Values
          }
        };
        values.Add(dbValueList);
      }
      DbVersion dbVersion;
      using (EllieMae.EMLite.Server.DbAccessManager dbAccessManager = new EllieMae.EMLite.Server.DbAccessManager())
        dbVersion = dbAccessManager.GetDbVersion();
      sql.InsertInto(DDMDataTableFieldValueDbAccessor.DataTableFieldValue, values, dbVersion);
    }

    private static int[] FetchDataTableFieldIdsForCreatedCells(
      List<DDMDataTableFieldValue> ddmDataTableFieldValues)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      List<int> values1 = new List<int>(ddmDataTableFieldValues.Count);
      List<int> values2 = new List<int>(ddmDataTableFieldValues.Count);
      List<int> source = new List<int>();
      int num = 0;
      for (int index1 = 0; index1 < ddmDataTableFieldValues.Count; ++index1)
      {
        DDMDataTableFieldValue dataTableFieldValue = ddmDataTableFieldValues[index1];
        values1.Add(dataTableFieldValue.RowId);
        values2.Add(dataTableFieldValue.ColumnId);
        if (num == 1000 || index1 == ddmDataTableFieldValues.Count - 1)
        {
          dbQueryBuilder.AppendLine("select max(dataTableFieldId) as fieldIdForNewCell from DDM_DataTableFields");
          dbQueryBuilder.AppendLine("where dataTableID = " + (object) ddmDataTableFieldValues[0].DataTableId);
          dbQueryBuilder.AppendLine("and rowId in (" + string.Join<int>(", ", (IEnumerable<int>) values1) + ")");
          dbQueryBuilder.AppendLine("and columnId in (" + string.Join<int>(", ", (IEnumerable<int>) values2) + ")");
          dbQueryBuilder.AppendLine("group by dataTableID, rowId, columnId");
          dbQueryBuilder.AppendLine("order by fieldIdForNewCell");
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          int[] numArray = new int[ddmDataTableFieldValues.Count];
          for (int index2 = 0; index2 < dataRowCollection.Count; ++index2)
            source.Add((int) dataRowCollection[index2]["fieldIdForNewCell"]);
          values1.Clear();
          values2.Clear();
          dbQueryBuilder.Reset();
          num = 0;
        }
        ++num;
      }
      source.Distinct<int>();
      return source.ToArray();
    }

    public static List<string> BatchUpdateDDMDataTableFields(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      List<DDMDataTableFieldValue> ddmDataTableFieldValues,
      List<string> tmpTables)
    {
      List<string> stringList = new List<string>();
      List<DDMDataTableFieldValue> list = ddmDataTableFieldValues.GroupBy<DDMDataTableFieldValue, int>((System.Func<DDMDataTableFieldValue, int>) (r => r.Id)).Where<IGrouping<int, DDMDataTableFieldValue>>((System.Func<IGrouping<int, DDMDataTableFieldValue>, bool>) (g => g.Count<DDMDataTableFieldValue>() == 1)).SelectMany<IGrouping<int, DDMDataTableFieldValue>, DDMDataTableFieldValue>((System.Func<IGrouping<int, DDMDataTableFieldValue>, IEnumerable<DDMDataTableFieldValue>>) (r => (IEnumerable<DDMDataTableFieldValue>) r)).ToList<DDMDataTableFieldValue>();
      int count = list.Count;
      int num = 0;
      DataTable dataTable = new DataTable("TmpTable");
      DDMDataTableFieldValueDbAccessor.createTableSchema(dataTable);
      for (int index = 0; index < count; ++index)
      {
        ++num;
        DDMDataTableFieldValue dataTableFieldValue = list[index];
        DataRow row = dataTable.NewRow();
        row["criteria"] = (object) dataTableFieldValue.Criteria;
        row["rowId"] = (object) dataTableFieldValue.RowId;
        row["columnId"] = (object) dataTableFieldValue.ColumnId;
        row["dataTableID"] = (object) dataTableFieldValue.DataTableId;
        row["fieldId"] = (object) dataTableFieldValue.FieldId;
        row["isOutput"] = (object) dataTableFieldValue.IsOutput;
        row["values"] = (object) dataTableFieldValue.Values;
        row["dataTableFieldID"] = (object) dataTableFieldValue.Id;
        dataTable.Rows.Add(row);
        if (num == 10000 || index == count - 1)
        {
          Guid guid = Guid.NewGuid();
          dataTable.AcceptChanges();
          string tmpTable = "[$tmp" + (object) guid + "]";
          string pkName = "PK_tmp" + (object) guid;
          DDMDataTableFieldValueDbAccessor.createTmpTable(tmpTable, pkName, dataTable);
          stringList.Add(tmpTable);
          sql.AppendLine("UPDATE T SET T.rowId = Temp.rowId, T.columnId = Temp.columnId, T.fieldId = Temp.fieldId, T.[values] = Temp.[values], T.criteria = Temp.criteria FROM DDM_DataTableFields T INNER JOIN " + tmpTable + " Temp ON T.dataTableFieldID = Temp.dataTableFieldID;Drop table " + tmpTable + ";");
          num = 0;
          dataTable.Clear();
        }
      }
      return stringList;
    }

    private static void CleanTempTables(List<string> lsGuids)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (string lsGuid in lsGuids)
      {
        dbQueryBuilder.AppendLine("IF EXISTS(SELECT * FROM sysobjects WHERE id = object_id(N'" + lsGuid + "') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) ");
        dbQueryBuilder.AppendLine(" begin ");
        dbQueryBuilder.AppendLine(" drop table " + lsGuid);
        dbQueryBuilder.AppendLine(" end ");
      }
      dbQueryBuilder.Execute();
    }

    private static void createTmpTable(string tmpTable, string pkName, DataTable dt)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("CREATE TABLE " + tmpTable + "( [dataTableFieldID] [int], [dataTableID] [int], [rowId] [int], [columnId] [int], [fieldId] [varchar] (100), [values] [varchar] (MAX), [criteria] [int] ,[isOutput] [bit],CONSTRAINT [" + pkName + "] PRIMARY KEY CLUSTERED ( [dataTableFieldID], [dataTableID]))");
      dbQueryBuilder.Execute();
      dbQueryBuilder.DoBulkCopy(tmpTable, dt);
    }

    public static void BatchDeleteDDMDataTableFields(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      List<DDMDataTableFieldValue> ddmDataTableFieldValues)
    {
      List<int> values = new List<int>(ddmDataTableFieldValues.Count);
      int num = 10000;
      foreach (DDMDataTableFieldValue dataTableFieldValue in ddmDataTableFieldValues)
      {
        values.Add(dataTableFieldValue.Id);
        if (num == 10000)
        {
          sql.DeleteFrom(DDMDataTableFieldValueDbAccessor.DataTableFieldValue);
          sql.AppendLine(" where dataTableFieldID in (" + string.Join<int>(", ", (IEnumerable<int>) values) + ")");
          num = 0;
          values.Clear();
        }
        ++num;
      }
      sql.DeleteFrom(DDMDataTableFieldValueDbAccessor.DataTableFieldValue);
      sql.AppendLine(" where dataTableFieldID in (" + string.Join<int>(", ", (IEnumerable<int>) values) + ")");
    }

    public static void DeleteDDMDataTableFieldByTableID(DDMDataTable ddmDataTable)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue("dataTableID", (object) ddmDataTable.Id, (IDbEncoder) DbEncoding.None);
      dbQueryBuilder.DeleteFrom(DDMDataTableFieldValueDbAccessor.DataTableFieldValue, key);
      dbQueryBuilder.Execute();
    }
  }
}
