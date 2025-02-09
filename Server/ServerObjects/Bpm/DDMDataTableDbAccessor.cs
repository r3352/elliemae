// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.DDMDataTableDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class DDMDataTableDbAccessor : BpmDbAccessor
  {
    private static readonly string className = nameof (DDMDataTableDbAccessor);
    private static readonly string FILTER_STR = "rules.ruleID = ";

    public DDMDataTableDbAccessor()
      : base(ClientSessionCacheID.BpmDDMDataTables)
    {
    }

    public static bool DDMDataTableExists(string tableName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.AppendLine(string.Format("SELECT count(*) FROM DDM_DataTables WHERE dataTableName = '{0}'", (object) tableName));
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar(DbTransactionType.None)) != 0;
    }

    public static DDMDataTable[] GetAllDDMDataTables()
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTables");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.SelectFrom(table);
      return dbQueryBuilder.Execute(DbTransactionType.None).Cast<DataRow>().Select<DataRow, DDMDataTable>((System.Func<DataRow, DDMDataTable>) (row => new DDMDataTable(row))).ToArray<DDMDataTable>();
    }

    public static DDMDataTable GetDDMDataTableAndFieldValues(int dataTableId)
    {
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTables");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTableFields");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder1.SelectFrom(table1);
      dbQueryBuilder1.Append(string.Format(" Where dataTableID = {0}", (object) dataTableId));
      DDMDataTable tableAndFieldValues = new DDMDataTable(dbQueryBuilder1.Execute(DbTransactionType.None)[0]);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder2.SelectFrom(table2);
      dbQueryBuilder2.Append(string.Format(" Where dataTableID = {0}", (object) dataTableId));
      dbQueryBuilder2.Append(string.Format(" Order By {0}, {1} ", (object) "rowId", (object) "columnId"));
      List<DDMDataTableFieldValue> list = dbQueryBuilder2.Execute(DbTransactionType.None).Cast<DataRow>().Select<DataRow, DDMDataTableFieldValue>((System.Func<DataRow, DDMDataTableFieldValue>) (row => new DDMDataTableFieldValue(row))).ToList<DDMDataTableFieldValue>();
      Dictionary<int, List<DDMDataTableFieldValue>> dictionary = new Dictionary<int, List<DDMDataTableFieldValue>>();
      foreach (DDMDataTableFieldValue dataTableFieldValue in list)
      {
        if (dictionary.ContainsKey(dataTableFieldValue.RowId))
          dictionary[dataTableFieldValue.RowId].Add(dataTableFieldValue);
        else
          dictionary.Add(dataTableFieldValue.RowId, new List<DDMDataTableFieldValue>()
          {
            dataTableFieldValue
          });
      }
      tableAndFieldValues.FieldValues = dictionary;
      return tableAndFieldValues;
    }

    public static DDMDataTable[] GetAllDDMDataTablesAndFieldValues()
    {
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTables");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTableFields");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder1.SelectFrom(table1);
      List<DDMDataTable> list = dbQueryBuilder1.Execute(DbTransactionType.None).Cast<DataRow>().Select<DataRow, DDMDataTable>((System.Func<DataRow, DDMDataTable>) (row => new DDMDataTable(row))).ToList<DDMDataTable>();
      if (list.Count == 0)
        return new DDMDataTable[0];
      Dictionary<int, Dictionary<int, List<DDMDataTableFieldValue>>> dictionary1 = new Dictionary<int, Dictionary<int, List<DDMDataTableFieldValue>>>();
      foreach (DDMDataTable ddmDataTable in list)
        dictionary1[ddmDataTable.Id] = ddmDataTable.FieldValues = new Dictionary<int, List<DDMDataTableFieldValue>>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder2.SelectFrom(table2);
      dbQueryBuilder2.Append(string.Format(" Order By {0}, {1}, {2} ", (object) "dataTableID", (object) "rowId", (object) "columnId"));
      foreach (DDMDataTableFieldValue dataTableFieldValue in dbQueryBuilder2.Execute(DbTransactionType.None).Cast<DataRow>().Select<DataRow, DDMDataTableFieldValue>((System.Func<DataRow, DDMDataTableFieldValue>) (row => new DDMDataTableFieldValue(row))).ToList<DDMDataTableFieldValue>())
      {
        Dictionary<int, List<DDMDataTableFieldValue>> dictionary2 = dictionary1[dataTableFieldValue.DataTableId];
        if (dictionary2.ContainsKey(dataTableFieldValue.RowId))
          dictionary2[dataTableFieldValue.RowId].Add(dataTableFieldValue);
        else
          dictionary2.Add(dataTableFieldValue.RowId, new List<DDMDataTableFieldValue>()
          {
            dataTableFieldValue
          });
      }
      return list.ToArray();
    }

    public static DDMDataTable[] GetDDMDataTablesAndFieldValuesForDataTableNames(
      List<string> dataTableNames)
    {
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTables");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTableFields");
      if (dataTableNames.Count == 0)
        return new DDMDataTable[0];
      List<string> values = new List<string>(dataTableNames.Count);
      foreach (string dataTableName in dataTableNames)
        values.Add(SQL.Encode((object) dataTableName));
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder1.SelectFrom(table1);
      dbQueryBuilder1.AppendLine(" where dataTableName in (" + string.Join(", ", (IEnumerable<string>) values) + ")");
      List<DDMDataTable> list = dbQueryBuilder1.Execute(DbTransactionType.None).Cast<DataRow>().Select<DataRow, DDMDataTable>((System.Func<DataRow, DDMDataTable>) (row => new DDMDataTable(row))).ToList<DDMDataTable>();
      if (list.Count == 0)
        return new DDMDataTable[0];
      Dictionary<int, Dictionary<int, List<DDMDataTableFieldValue>>> dictionary1 = new Dictionary<int, Dictionary<int, List<DDMDataTableFieldValue>>>();
      foreach (DDMDataTable ddmDataTable in list)
        dictionary1[ddmDataTable.Id] = ddmDataTable.FieldValues = new Dictionary<int, List<DDMDataTableFieldValue>>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder2.SelectFrom(table2);
      dbQueryBuilder2.AppendLine(" where dataTableID in (" + string.Join<int>(", ", (IEnumerable<int>) dictionary1.Keys) + ")");
      dbQueryBuilder2.Append(string.Format(" Order By {0}, {1}, {2} ", (object) "dataTableID", (object) "rowId", (object) "columnId"));
      foreach (DDMDataTableFieldValue dataTableFieldValue in dbQueryBuilder2.Execute(DbTransactionType.None).Cast<DataRow>().Select<DataRow, DDMDataTableFieldValue>((System.Func<DataRow, DDMDataTableFieldValue>) (row => new DDMDataTableFieldValue(row))).ToList<DDMDataTableFieldValue>())
      {
        Dictionary<int, List<DDMDataTableFieldValue>> dictionary2 = dictionary1[dataTableFieldValue.DataTableId];
        if (dictionary2.ContainsKey(dataTableFieldValue.RowId))
          dictionary2[dataTableFieldValue.RowId].Add(dataTableFieldValue);
        else
          dictionary2.Add(dataTableFieldValue.RowId, new List<DDMDataTableFieldValue>()
          {
            dataTableFieldValue
          });
      }
      return list.ToArray();
    }

    public static DDMDataTable GetDDMDataTableAndFieldIdsForDataTableName(string dataTableName)
    {
      if (string.IsNullOrEmpty(dataTableName))
        return (DDMDataTable) null;
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTables");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.SelectFrom(table);
      dbQueryBuilder.AppendLineFormat(" where dataTableName = {0}", (object) SQL.Encode((object) dataTableName));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      return dataRowCollection.Count > 0 ? new DDMDataTable(dataRowCollection[0]) : (DDMDataTable) null;
    }

    public static void AddDDMDataTableFieldValuesForDataTable(
      DDMDataTable dataTable,
      Dictionary<string, string> fieldValues)
    {
      if (dataTable == null || fieldValues == null)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      DataRowCollection source;
      try
      {
        dbQueryBuilder1.AppendLineFormat("DECLARE @MaxRowId int;\r\n                    DECLARE @RowCounter int = 0; \r\n                    DECLARE @NoOfCols int;\r\n                    DECLARE @Count int = 0;\r\n                    DECLARE @CurrentRowId int;\r\n                    DECLARE @fieldId nvarchar(30);\r\n                    DECLARE @Values nvarchar(max);\r\n                    DECLARE @criteria int;\r\n                    DECLARE @rowResult bit;\r\n\r\n                    DECLARE @TempRowIds AS TABLE (\r\n                        rowid int\r\n                    );\r\n\r\n                    SELECT @MaxRowId = MAX(rowid)\r\n                    FROM DDM_DataTableFields\r\n                    WHERE dataTableID = {0} -- DataTableId\r\n                    -- Max rowId, rowi start from 0 ..\r\n                    --print @MaxRowId\r\n\r\n                    --Columns count\r\n                    Select @NoOfCols = COUNT(1)\r\n                    From DDM_DataTableFields\r\n                    Where dataTableID = {0} -- DataTableId\r\n                    And rowId = 0\r\n                    And isOutput = 0\r\n                    --print @NoOfCols\r\n                    --RowId starts with 0, iterate till @MaxRowId\r\n                    WHILE @RowCounter <= @MaxRowId\r\n                    BEGIN\r\n                        SET @Count = 0;\r\n\t                    --ColumnId starts with 0 \r\n                        WHILE @Count < @NoOfCols\r\n                        BEGIN\r\n\t\t                    SELECT @fieldId = CASE fieldId ", (object) dataTable.Id);
        foreach (string key in fieldValues.Keys)
          dbQueryBuilder1.AppendLineFormat(" WHEN '{0}' THEN '{1}' ", (object) key, (object) fieldValues[key]);
        dbQueryBuilder1.AppendFormat(" END,\r\n\t\t                            @Values = [Values], @criteria = criteria\r\n\t\t                            FROM DDM_DataTableFields\r\n\t\t                            WHERE dataTableID = {0}  -- DataTableId\r\n\t\t                            AND rowid = @RowCounter\r\n\t\t                            AND columnId = @Count\r\n\r\n\t\t                            --print @count\r\n\t\t                            -- increment counter\r\n\t\t                            SET @Count += 1;\r\n\t\t                            --Print 'FieldId: ' + CAST(@fieldId  AS NVARCHAR(30)) + ' Values:' + @values + ' Criteria:' + CAST(@criteria AS NVARCHAR(30));\r\n\t\t                            BEGIN TRY\r\n\t\t\t                            --Criteria check\r\n\t\t\t                            SET @rowResult = CASE @criteria\r\n\t\t\t\t\t\t\t                WHEN 25 THEN --No value in loan file\r\n\t\t\t\t\t\t\t                CASE WHEN ISDATE(@values) = 1 THEN \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS datetime) = CAST('1/1/1900' AS datetime), 1, 0)\r\n\t\t\t\t\t\t\t                ELSE IIF(@fieldId = '', 1, 0)\r\n\t\t\t\t\t\t\t                END\r\n\t\t\t\t\t\t\t                WHEN 0 THEN \r\n\t\t\t\t\t\t\t                CASE WHEN ISDATE(@values) = 1 THEN \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS datetime) = CAST(@values AS datetime), 1, 0)\r\n\t\t\t\t\t\t\t                ELSE \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS decimal(38, 10)) = CAST(@values AS decimal(38, 10)), 1, 0)\r\n\t\t\t\t\t\t\t                END\r\n\t\t\t\t\t\t\t                WHEN 1 THEN \r\n\t\t\t\t\t\t\t                CASE WHEN ISDATE(@values) = 1 THEN \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS datetime) < CAST(@values AS datetime), 1, 0)\r\n\t\t\t\t\t\t\t                ELSE \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS decimal(38, 10)) < CAST(@values AS decimal(38, 10)), 1, 0)\r\n\t\t\t\t\t\t\t                END\r\n\t\t\t\t\t\t\t                WHEN 2 THEN \r\n\t\t\t\t\t\t\t                CASE WHEN ISDATE(@values) = 1 THEN \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS datetime) <= CAST(@values AS datetime), 1, 0)\r\n\t\t\t\t\t\t\t                ELSE \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS decimal(38, 10)) <= CAST(@values AS decimal(38, 10)), 1, 0)\r\n\t\t\t\t\t\t\t                END\r\n\t\t\t\t\t\t\t                WHEN 3 THEN \r\n\t\t\t\t\t\t\t                CASE WHEN ISDATE(@values) = 1 THEN \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS datetime) > CAST(@values AS datetime), 1, 0)\r\n\t\t\t\t\t\t\t                ELSE \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS decimal(38, 10)) > CAST(@values AS decimal(38, 10)), 1, 0)\r\n\t\t\t\t\t\t\t                END\r\n\t\t\t\t\t\t\t                WHEN 4 THEN \r\n\t\t\t\t\t\t\t                CASE WHEN ISDATE(@values) = 1 THEN \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS datetime) >= CAST(@values AS datetime), 1, 0)\r\n\t\t\t\t\t\t\t                ELSE \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS decimal(38, 10)) >= CAST(@values AS decimal(38, 10)), 1, 0)\r\n\t\t\t\t\t\t\t                END\r\n\t\t\t\t\t\t\t                WHEN 5 THEN \r\n\t\t\t\t\t\t\t                CASE WHEN ISDATE(@values) = 1 THEN \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS datetime) != CAST(@values AS datetime), 1, 0)\r\n\t\t\t\t\t\t\t                ELSE \r\n\t\t\t\t\t\t\t\t                IIF(CAST(@fieldId AS decimal(38, 10)) != CAST(@values AS decimal(38, 10)), 1, 0)\r\n\t\t\t\t\t\t\t                END\r\n\t\t\t\t\t\t\t                WHEN 6 THEN \r\n\t\t\t\t\t\t\t                CASE WHEN ISDATE(LEFT(@values, CHARINDEX('|', @values) - 1)) = 1 THEN \r\n\t\t\t\t\t\t\t\t                IIF(\r\n\t\t\t\t\t\t\t\t\t                (CAST(@fieldId AS datetime) >= CAST(LEFT(@values, CHARINDEX('|', @values) - 1) AS datetime)) AND\r\n\t\t\t\t\t\t\t\t\t                (CAST(@fieldId AS datetime) <= CAST(RIGHT(@values, LEN(@values) - CHARINDEX('|', @values)) AS datetime))\r\n\t\t\t\t\t\t\t\t                , 1, 0) -- range\r\n\t\t\t\t\t\t\t                ELSE \r\n\t\t\t\t\t\t\t\t                IIF(\r\n\t\t\t\t\t\t\t\t\t                (CAST(@fieldId AS decimal(38, 10)) >= CAST(LEFT(@values, CHARINDEX('|', @values) - 1) AS decimal(38, 10))) AND\r\n\t\t\t\t\t\t\t\t\t                (CAST(@fieldId AS decimal(38, 10)) <= CAST(RIGHT(@values, LEN(@values) - CHARINDEX('|', @values)) AS decimal(38, 10)))\r\n\t\t\t\t\t\t\t\t                , 1, 0) -- range\r\n\t\t\t\t\t\t\t                END\r\n\t\t\t\t\t\t\t                WHEN 10 THEN IIF(@fieldId = @values, 1, 0) -- Equals\r\n\t\t\t\t\t\t\t                WHEN 11 THEN IIF(@fieldId != @values, 1, 0) -- Not Equals\r\n\t\t\t\t\t\t\t                WHEN 12 THEN IIF(CHARINDEX(@values, @fieldId) > 0, 1, 0) -- Contains\r\n\t\t\t\t\t\t\t                WHEN 13 THEN IIF(CHARINDEX(@values, @fieldId) = 0, 1, 0) -- Does not Contains\r\n\t\t\t\t\t\t\t                WHEN 14 THEN IIF(CHARINDEX(@values, @fieldId) = 1, 1, 0) -- Starts With\r\n\t\t\t\t\t\t\t                WHEN 15 THEN IIF(RIGHT(@fieldId, LEN(@values)) = @values, 1, 0) -- Ends With\r\n\t\t\t\t\t\t\t                WHEN 8 THEN IIF(@fieldId = @values, 1, 0) -- SSN Equals\r\n\t\t\t\t\t\t\t                WHEN 19 THEN IIF(@fieldId = @values, 1, 0) -- Zip Equals\r\n\t\t\t\t\t\t\t                WHEN 21 THEN IIF(@fieldId = @values, 1, 0) -- County Equals\r\n\t\t\t\t\t\t\t                WHEN 18 THEN \r\n                                                CASE WHEN ISDATE(LEFT(@values, CHARINDEX('|', @values) - 1)) = 1 THEN \r\n\t\t\t\t\t\t\t\t                    1 -- return 1 if List of values are date field\r\n\t\t\t\t\t\t\t                    ELSE \r\n\t\t\t\t\t\t\t\t                    IIF(CHARINDEX(@fieldId, @values) > 0, 1, 0) --list of\r\n\t\t\t\t\t\t\t\t\t\t\t    END\r\n\t\t\t\t\t\t\t                WHEN 9 THEN IIF(CHARINDEX(@fieldId, @values) > 0, 1, 0) -- List of SSN\r\n\t\t\t\t\t\t\t                WHEN 20 THEN IIF(CHARINDEX(@fieldId, @values) > 0, 1, 0) -- List of Zip\r\n\t\t\t\t\t\t\t                WHEN 22 THEN IIF(CHARINDEX(@fieldId, @values) > 0, 1, 0) -- List of County\r\n\t\t\t\t\t\t\t                WHEN 23 THEN IIF(CHARINDEX(@fieldId, @values) > 0, 1, 0) -- List of State\r\n\t\t\t\t\t\t\t                ELSE 1 -- Return True\r\n\t\t\t\t\t\t                END\r\n\t\t\t                            --print @rowResult\r\n\t\t\t                            IF @rowResult = 0\r\n\t\t\t                            BEGIN  -- Skip the row\r\n\t\t\t\t                            --Print 'Not Matching'\r\n\t\t\t\t                            BREAK;\r\n\t\t\t                            END\r\n\t\t                            END TRY\r\n\t\t                            BEGIN CATCH\r\n\t\t\t                            --print 'error'\r\n\t\t\t                            -- Do nothing\r\n\t\t                            END CATCH\t\t\r\n                                END\r\n\t                            --print @rowResult\r\n\t                            IF @rowResult = 1\r\n\t                            BEGIN\r\n\t                            --print 'Match'\r\n\t\t                            INSERT INTO @TempRowIds (rowid) Values (@RowCounter)\r\n\t                            END\r\n                                SET @RowCounter += 1;\r\n                            END\r\n\r\n                            --Select * from @TempRowIds\r\n\r\n                            --Return result rows\r\n                            SELECT * FROM DDM_DataTableFields DTF\r\n                            JOIN @TempRowIds TR ON TR.rowid = DTF.Rowid\r\n                            WHERE dataTableID = {0} -- DataTableId\r\n                            ORDER BY DTF.rowId, DTF.columnId", (object) dataTable.Id);
        source = dbQueryBuilder1.Execute(DbTransactionType.None);
      }
      catch (Exception ex)
      {
        EllieMae.EMLite.Server.ServerGlobals.TraceLog.WriteException(TraceLevel.Error, DDMDataTableDbAccessor.className, ex);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTableFields");
        dbQueryBuilder2.SelectFrom(table);
        dbQueryBuilder2.AppendLineFormat(" where dataTableID = {0}", (object) dataTable.Id);
        dbQueryBuilder2.AppendLineFormat(string.Format(" Order By {0}, {1}", (object) "rowId", (object) "columnId"));
        source = dbQueryBuilder2.Execute(DbTransactionType.None);
      }
      List<DDMDataTableFieldValue> list = source.Cast<DataRow>().Select<DataRow, DDMDataTableFieldValue>((System.Func<DataRow, DDMDataTableFieldValue>) (row => new DDMDataTableFieldValue(row))).ToList<DDMDataTableFieldValue>();
      dataTable.FieldValues = new Dictionary<int, List<DDMDataTableFieldValue>>();
      foreach (DDMDataTableFieldValue dataTableFieldValue in list)
      {
        if (dataTable.FieldValues.ContainsKey(dataTableFieldValue.RowId))
          dataTable.FieldValues[dataTableFieldValue.RowId].Add(dataTableFieldValue);
        else
          dataTable.FieldValues.Add(dataTableFieldValue.RowId, new List<DDMDataTableFieldValue>()
          {
            dataTableFieldValue
          });
      }
    }

    public static int CreateDDMDataTable(DDMDataTable DDMdataTable)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTables");
      if (DDMDataTableDbAccessor.DDMDataTableExists(DDMdataTable.Name))
        return 0;
      DbValueList values = new DbValueList()
      {
        {
          "dataTableDesc",
          (object) DDMdataTable.Description
        },
        {
          "dataTableName",
          (object) DDMdataTable.Name
        },
        {
          "dataTableType",
          (object) DDMdataTable.DataTableType
        },
        {
          "lastModTime",
          (object) DDMdataTable.LastModDt
        },
        {
          "lastModByUserID",
          (object) DDMdataTable.LastModByUserID
        },
        {
          "lastModifiedByFullName",
          (object) DDMdataTable.LastModByFullName
        },
        {
          "fieldIdList",
          (object) DDMdataTable.FieldIdList
        },
        {
          "outputIdList",
          (object) DDMdataTable.OutputIdList
        }
      };
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@dataTableId", "int");
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.SelectIdentity("@dataTableId");
      dbQueryBuilder.Select("@dataTableId");
      object obj = dbQueryBuilder.ExecuteScalar();
      DDMdataTable.Id = Convert.ToInt32(obj);
      DDMdataTable.RuleID = DDMdataTable.Id;
      if (DDMdataTable.FieldValues != null && DDMdataTable.FieldValues.Count > 0)
        DDMDataTableFieldValueDbAccessor.CreateDDMDataTableFields(DDMdataTable.Id, DDMdataTable.FieldValues);
      try
      {
        DDMDataTableDbAccessor.CreateFieldSearchInfo(DDMdataTable);
      }
      catch (Exception ex)
      {
        EllieMae.EMLite.Server.ServerGlobals.TraceLog.WriteException(TraceLevel.Error, DDMDataTableDbAccessor.className, ex);
      }
      return DDMdataTable.Id;
    }

    public static int UpdateDDMDataTable(DDMDataTable DDMdataTable)
    {
      try
      {
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTables");
        DbValueList values = new DbValueList()
        {
          {
            "dataTableDesc",
            (object) DDMdataTable.Description
          },
          {
            "dataTableName",
            (object) DDMdataTable.Name
          },
          {
            "dataTableType",
            (object) DDMdataTable.DataTableType
          },
          {
            "lastModTime",
            (object) DDMdataTable.LastModDt
          },
          {
            "lastModByUserID",
            (object) DDMdataTable.LastModByUserID
          },
          {
            "lastModifiedByFullName",
            (object) DDMdataTable.LastModByFullName
          },
          {
            "fieldIdList",
            (object) DDMdataTable.FieldIdList
          },
          {
            "outputIdList",
            (object) DDMdataTable.OutputIdList
          }
        };
        DbValue key = new DbValue("dataTableID", (object) DDMdataTable.Id, (IDbEncoder) DbEncoding.None);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Update(table, values, key);
        dbQueryBuilder.Execute();
        if (DDMdataTable.FieldValues != null && DDMdataTable.FieldValues.Count > 0)
        {
          DDMDataTableFieldValueDbAccessor.DeleteDDMDataTableFieldByTableID(DDMdataTable);
          DDMDataTableFieldValueDbAccessor.CreateDDMDataTableFields(DDMdataTable.Id, DDMdataTable.FieldValues);
        }
        DDMDataTableDbAccessor.UpdateFieldSearchInfo(DDMdataTable, DDMdataTable.OriginalName);
      }
      catch (Exception ex)
      {
        EllieMae.EMLite.Server.ServerGlobals.TraceLog.WriteException(TraceLevel.Error, DDMDataTableDbAccessor.className, ex);
        return 0;
      }
      return DDMdataTable.Id;
    }

    public static void DeleteDDMDataTable(DDMDataTable ddmDataTable)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTables");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue("dataTableID", (object) ddmDataTable.Id, (IDbEncoder) DbEncoding.None);
      DDMDataTableFieldValueDbAccessor.DeleteDDMDataTableFieldByTableID(ddmDataTable);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
      try
      {
        DDMDataTableDbAccessor.DeleteFieldSearchInfo(ddmDataTable);
      }
      catch (Exception ex)
      {
        EllieMae.EMLite.Server.ServerGlobals.TraceLog.WriteException(TraceLevel.Error, DDMDataTableDbAccessor.className, ex);
      }
    }

    public static bool IsTableUsedByFeeOrFieldRules(DDMDataTable dt)
    {
      return DDMFeeRuleDbAccessor.IsTableUsedByFeeRule(dt) || DDMFieldRuleDbAccessor.IsTableUsedByFieldRules(dt);
    }

    public static void ResetDataTableFeeRuleFieldRuleValue(DDMDataTable dt)
    {
      DDMFeeRuleDbAccessor.ResetDataTableFeeRuleValue(dt);
      DDMFieldRuleDbAccessor.ResetDataTableFieldRuleValue(dt);
      if (!DDMDataTableDbAccessor.IsCachingEnabled())
        return;
      DDMDataTableDbAccessor.InvalidateFeeRuleAndFieldRuleCache();
    }

    private static bool IsCachingEnabled()
    {
      return ClientContext.GetCurrent().Settings.CacheSetting >= CacheSetting.Low;
    }

    private static void InvalidateFeeRuleAndFieldRuleCache()
    {
      string name1 = "DDM_FieldRuleScenario";
      string name2 = "DDM_FeeRuleScenario";
      ClientContext.GetCurrent().Cache.Remove(name1);
      ClientContext.GetCurrent().Cache.Remove(name2);
    }

    public static void SaveDataTableExportLog(DDMDataTableExportLog dataTableExportLog)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTableExportLog");
      DbValueList values = new DbValueList()
      {
        {
          "dataTableExportLogID",
          (object) dataTableExportLog.DataTableExportLogID
        },
        {
          "dataTableName",
          (object) dataTableExportLog.DataTableName
        },
        {
          "dataTableDesc",
          (object) dataTableExportLog.DataTableDescription
        },
        {
          "dataTableType",
          (object) dataTableExportLog.DataTableType
        },
        {
          "exportedTime",
          (object) dataTableExportLog.ExportedTime
        },
        {
          "exportedByUserID",
          (object) dataTableExportLog.ExportedByUserID
        },
        {
          "exportedByFullName",
          (object) dataTableExportLog.ExportedByFullName
        }
      };
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static DDMDataTableExportLog GetDataTableExportLog(string dataTableExportLogID)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTableExportLog");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      DbValue key = new DbValue(nameof (dataTableExportLogID), (object) dataTableExportLogID, (IDbEncoder) DbEncoding.Default);
      dbQueryBuilder.SelectFrom(table, key);
      return dbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, DDMDataTableExportLog>((System.Func<DataRow, DDMDataTableExportLog>) (row => new DDMDataTableExportLog(row))).FirstOrDefault<DDMDataTableExportLog>();
    }

    public static DDMDataTable GetDDMDataTableAndFieldValuesByName(string dataTableName)
    {
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTables");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTableFields");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder1.SelectFrom(table1);
      dbQueryBuilder1.Append(string.Format(" Where dataTableName = {0}", (object) SQL.Encode((object) dataTableName)));
      DDMDataTable fieldValuesByName = new DDMDataTable(dbQueryBuilder1.Execute(DbTransactionType.None)[0]);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder2.SelectFrom(table2);
      dbQueryBuilder2.Append(string.Format(" Where dataTableID = {0}", (object) fieldValuesByName.Id));
      dbQueryBuilder2.Append(string.Format(" Order By {0}, {1} ", (object) "rowId", (object) "columnId"));
      List<DDMDataTableFieldValue> list = dbQueryBuilder2.Execute(DbTransactionType.None).Cast<DataRow>().Select<DataRow, DDMDataTableFieldValue>((System.Func<DataRow, DDMDataTableFieldValue>) (row => new DDMDataTableFieldValue(row))).ToList<DDMDataTableFieldValue>();
      Dictionary<int, List<DDMDataTableFieldValue>> dictionary = new Dictionary<int, List<DDMDataTableFieldValue>>();
      foreach (DDMDataTableFieldValue dataTableFieldValue in list)
      {
        if (dictionary.ContainsKey(dataTableFieldValue.RowId))
          dictionary[dataTableFieldValue.RowId].Add(dataTableFieldValue);
        else
          dictionary.Add(dataTableFieldValue.RowId, new List<DDMDataTableFieldValue>()
          {
            dataTableFieldValue
          });
      }
      fieldValuesByName.FieldValues = dictionary;
      return fieldValuesByName;
    }

    public static List<DDMDataTableReference> GetDDMDataTableReferences(string dataTableName)
    {
      return FieldSearchDbAccessor.GetDdmDatatableReferences(dataTableName);
    }

    public static void CreateFieldSearchInfo(DDMDataTable dataTable)
    {
      FieldSearchDbAccessor.CreateFieldSearchInfo(new FieldSearchRule((BizRuleInfo) dataTable));
    }

    public static int UpdateFieldSearchInfo(DDMDataTable dataTable)
    {
      return DDMDataTableDbAccessor.UpdateFieldSearchInfo(dataTable, (string) null);
    }

    public static int UpdateFieldSearchInfo(DDMDataTable dataTable, string originalName)
    {
      dataTable = DDMDataTableDbAccessor.GetDDMDataTableAndFieldValues(dataTable.Id);
      int num = FieldSearchDbAccessor.UpdateFieldSearchInfo(new FieldSearchRule((BizRuleInfo) dataTable));
      if (!string.IsNullOrEmpty(originalName) && !dataTable.Name.Equals(originalName))
      {
        FieldSearchDbAccessor.UpdateDdmDataTableReference(originalName, dataTable.Name);
        DDMFeeRuleDbAccessor.UpdateDataTableReference(originalName, dataTable.Name);
        DDMFieldRuleDbAccessor.UpdateDataTableReference(originalName, dataTable.Name);
      }
      return num;
    }

    public static void DeleteFieldSearchInfo(DDMDataTable dataTable)
    {
      FieldSearchDbAccessor.DeleteFieldSearchInfo(dataTable.Id, FieldSearchRuleType.DDMDataTables);
      FieldSearchDbAccessor.ResetDataTableReference(dataTable.Name);
    }

    public static List<int> UpdateAllDataTableFieldSearchInfo()
    {
      EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_DataTables");
      List<int> intList = new List<int>();
      foreach (DDMDataTable tablesAndFieldValue in DDMDataTableDbAccessor.GetAllDDMDataTablesAndFieldValues())
        intList.Add(DDMDataTableDbAccessor.UpdateFieldSearchInfo(tablesAndFieldValue));
      return intList;
    }

    protected override string RuleTableName => "DDM_DataTables";

    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      string str = filter.Contains(DDMDataTableDbAccessor.FILTER_STR) ? filter.Replace(DDMDataTableDbAccessor.FILTER_STR, "") : "0";
      dbQueryBuilder.AppendLine(string.Format("select * from DDM_DataTables where dataTableID = {0}", (object) str));
      return this.MapDataSetToDDMDataTable(dbQueryBuilder.ExecuteSetQuery(DbTransactionType.None));
    }

    private BizRuleInfo[] MapDataSetToDDMDataTable(DataSet dataSet)
    {
      List<DDMDataTable> ddmDataTableList = new List<DDMDataTable>();
      if (dataSet == null || dataSet.Tables.Count == 0)
        return (BizRuleInfo[]) null;
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        DDMDataTable ddmDataTable = new DDMDataTable(row);
        ddmDataTableList.Add(ddmDataTable);
      }
      return (BizRuleInfo[]) ddmDataTableList.ToArray();
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue keyValue)
    {
      throw new NotImplementedException();
    }

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue keyValue)
    {
      throw new NotImplementedException();
    }

    protected override Type RuleType => typeof (DDMDataTable);
  }
}
