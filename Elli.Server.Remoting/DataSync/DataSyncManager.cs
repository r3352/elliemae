// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.DataSyncManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public class DataSyncManager : SessionBoundObject, IDataSyncManager
  {
    public const string className = "DataSyncManager";
    private TableManager tableMgr;
    private TimeSpan dbTimeout = new TimeSpan(600L);
    private const string ESCROW_FEE = "EscrowFee";
    private const string TITLE_FEE = "TitleFee";
    private const string DEFAULT_FEE_NAMES = "DefaultFeeNames";
    private const string MILESTONES = "Milestones";
    private const string MILESTONE_NAMES = "MilestoneNames";
    private const string EDISCLOSURE_STACKING_TEMPLATES = "DocEngineStackingOrders";
    private const string MILESTONE_DATA = "MilestoneData";

    public Dictionary<string, string> PostSqlParameterNameValues { get; set; }

    public DataSyncManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (DataSyncManager).ToLower());
      this.tableMgr = new TableManager(File.ReadAllText(Path.Combine(session.Context.Settings.ApplicationDir, "TableDefinition.xml")));
      this.dbTimeout = EnConfigurationSettings.GlobalSettings.SettingsSyncSQLTimeout;
      return this;
    }

    private void createOrgChartWithOrgPath()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("execute CreateOrgChartWithOrgPath");
      dbQueryBuilder.ExecuteNonQuery(this.dbTimeout, DbTransactionType.Default);
    }

    public virtual Dictionary<string, object> GetDataAsXmlString(
      string headTableName,
      string[] uiKeyValues,
      bool fetchFileSystemData)
    {
      this.onApiCalled(nameof (DataSyncManager), nameof (GetDataAsXmlString), (object[]) null);
      Dictionary<string, object> dataAsXmlString1 = new Dictionary<string, object>();
      List<DataSyncRecord> dbResult = new List<DataSyncRecord>();
      List<FileSystemSyncRecord> fileResult = new List<FileSystemSyncRecord>();
      this.Security.DemandRootAdministrator();
      this.createOrgChartWithOrgPath();
      string mappingDataAsXmlString = this.GetMappingDataAsXmlString(headTableName);
      HeadTableDef headTableDef1 = this.tableMgr.GetHeadTableDef(headTableName);
      HeadTableDef headTableDef2 = this.tableMgr.GetHeadTableDef(headTableName);
      if (headTableDef2 is HeadRelationTableDef)
        headTableDef2 = this.tableMgr.CreateVirtualHeadTable((HeadRelationTableDef) headTableDef2);
      Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
      if (headTableDef2.RecursiveSync)
      {
        List<string> stringList1 = new List<string>();
        foreach (string uiKeyValue in uiKeyValues)
        {
          List<string> stringList2 = new List<string>();
          char[] chArray = new char[1]{ '\\' };
          string[] strArray1 = uiKeyValue.Split(chArray);
          for (int index = 0; index < strArray1.Length; ++index)
          {
            if (index == 0)
              stringList2.Add(strArray1[index]);
            else
              stringList2.Add(stringList2[stringList2.Count - 1] + "\\" + strArray1[index]);
            if (!dictionary.ContainsKey(stringList2[stringList2.Count - 1]))
            {
              string[] strArray2 = new string[stringList2.Count];
              stringList2.CopyTo(0, strArray2, 0, stringList2.Count);
              dictionary.Add(stringList2[stringList2.Count - 1], new List<string>((IEnumerable<string>) strArray2));
            }
          }
          if (stringList2.Count > 0)
          {
            foreach (string str in stringList2.ToArray())
            {
              if (!stringList1.Contains(str))
                stringList1.Add(str);
            }
          }
        }
        uiKeyValues = stringList1.ToArray();
      }
      foreach (string uiKeyValue in uiKeyValues)
      {
        if (!dbResult.Contains(new DataSyncRecord(headTableDef2.TableName, uiKeyValue, "", mappingDataAsXmlString, true)))
        {
          string filter1 = DataSyncUtils.ConstructFilter(headTableDef2, new string[1]
          {
            uiKeyValue
          });
          string filter2 = filter1;
          if (headTableDef2.RecursiveSync)
            filter2 = DataSyncUtils.ConstructFilter(headTableDef2, dictionary[uiKeyValue].ToArray());
          string dataAsXmlString2 = this.getDataAsXmlString(headTableDef2, filter2);
          dbResult.Add(new DataSyncRecord(headTableDef1.TableName, uiKeyValue, dataAsXmlString2, mappingDataAsXmlString, true));
          if (headTableDef2.RequireFileSystemSync)
          {
            foreach (FileSystemSyncRecord associatedFileSystemEntry in new FileSystemManager("", dataAsXmlString2, this.Session).GetAssociatedFileSystemEntries((TableDef) headTableDef2, fetchFileSystemData))
            {
              if (!fileResult.Contains(associatedFileSystemEntry))
                fileResult.Add(associatedFileSystemEntry);
            }
          }
          this.getExtendedTablesDataAsXmlString(headTableDef2, filter1, dbResult, fileResult, fetchFileSystemData);
          if (!dataAsXmlString1.ContainsKey("DataSync"))
          {
            dataAsXmlString1.Add("DataSync", (object) dbResult);
          }
          else
          {
            List<DataSyncRecord> dataSyncRecordList = (List<DataSyncRecord>) dataAsXmlString1["DataSync"];
            foreach (DataSyncRecord dataSyncRecord in dbResult)
            {
              if (!dataSyncRecordList.Contains(dataSyncRecord))
                dataSyncRecordList.Add(dataSyncRecord);
            }
            dataAsXmlString1["DataSync"] = (object) dataSyncRecordList;
          }
          if (!dataAsXmlString1.ContainsKey("FileSync"))
          {
            dataAsXmlString1.Add("FileSync", (object) fileResult);
          }
          else
          {
            List<FileSystemSyncRecord> systemSyncRecordList = (List<FileSystemSyncRecord>) dataAsXmlString1["FileSync"];
            foreach (FileSystemSyncRecord systemSyncRecord in fileResult)
            {
              if (!systemSyncRecordList.Contains(systemSyncRecord))
                systemSyncRecordList.Add(systemSyncRecord);
            }
            dataAsXmlString1["FileSync"] = (object) systemSyncRecordList;
          }
        }
      }
      return dataAsXmlString1;
    }

    private string getDataAsXmlString(HeadTableDef headTable, string filter)
    {
      this.onApiCalled(nameof (DataSyncManager), nameof (getDataAsXmlString), new object[2]
      {
        (object) headTable,
        (object) filter
      });
      this.Security.DemandRootAdministrator();
      string query = (string) null;
      DataSet dataSet1 = this.getData(out query, headTable, filter);
      switch (dataSet1)
      {
        case null:
        case null:
          return dataSet1.GetXml();
        default:
          int? count = dataSet1.Tables?.Count;
          int num = 0;
          if (count.GetValueOrDefault() > num & count.HasValue)
          {
            DataSet dataSet2 = new DataSet()
            {
              Locale = CultureInfo.InvariantCulture
            };
            foreach (DataTable table1 in (InternalDataCollectionBase) dataSet1.Tables)
            {
              bool flag = false;
              DataTable table2 = table1.Clone();
              foreach (DataColumn column in (InternalDataCollectionBase) table2.Columns)
              {
                if (column.DataType == Type.GetType("System.DateTime"))
                {
                  column.DateTimeMode = DataSetDateTime.Unspecified;
                  flag = true;
                }
              }
              if (flag)
              {
                foreach (DataRow row in (InternalDataCollectionBase) table1.Rows)
                  table2.ImportRow(row);
                dataSet2.Tables.Add(table2);
              }
              else
                dataSet2.Tables.Add(table1.Copy());
            }
            dataSet1.Tables.Clear();
            dataSet1 = dataSet2;
            goto case null;
          }
          else
            goto case null;
      }
    }

    private void SetDateTimeMode(DataTable table, DataColumn col, DataSetDateTime mode)
    {
      object[] objArray = new object[table.Rows.Count];
      for (int index = 0; index < objArray.Length; ++index)
      {
        if (table.Rows[index].RowState != DataRowState.Deleted)
          objArray[index] = table.Rows[index][col];
      }
      table.Columns.Remove(col);
      col.DateTimeMode = mode;
      table.Columns.Add(col);
      for (int index = 0; index < objArray.Length; ++index)
      {
        if (table.Rows[index].RowState != DataRowState.Deleted)
        {
          int rowState = (int) table.Rows[index].RowState;
          table.Rows[index][col] = objArray[index];
          if (rowState == 2)
            table.Rows[index].AcceptChanges();
        }
      }
    }

    private void getExtendedTablesDataAsXmlString(
      HeadTableDef headTable,
      string filter,
      List<DataSyncRecord> dbResult,
      List<FileSystemSyncRecord> fileResult,
      bool fetchFileSystemData)
    {
      this.onApiCalled(nameof (DataSyncManager), nameof (getExtendedTablesDataAsXmlString), new object[4]
      {
        (object) headTable,
        (object) filter,
        (object) dbResult,
        (object) fileResult
      });
      this.Security.DemandRootAdministrator();
      Dictionary<string, object> dictionary1 = new Dictionary<string, object>();
      string query = (string) null;
      DataSet data = this.getData(out query, headTable, filter);
      foreach (HeadTableDef headTable1 in this.tableMgr.HeadTables)
      {
        if (!(headTable1.TableName == headTable.TableName))
        {
          string mappingXmlString = "";
          if (data.Tables.Contains(headTable1.TableName) && data.Tables[headTable1.TableName].Rows.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) data.Tables[headTable1.TableName].Rows)
            {
              string uiKeyValue = string.Concat(row[headTable1.UIKeyColumn]);
              if (!dbResult.Contains(new DataSyncRecord(headTable1.TableName, uiKeyValue, "", "", false)))
              {
                if (mappingXmlString == "")
                  mappingXmlString = this.GetMappingDataAsXmlString(headTable1.TableName);
                Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
                if (headTable1.RecursiveSync)
                {
                  List<string> stringList1 = new List<string>();
                  List<string> stringList2 = new List<string>();
                  string[] strArray1 = uiKeyValue.Split('\\');
                  for (int index = 0; index < strArray1.Length; ++index)
                  {
                    if (index == 0)
                      stringList2.Add(strArray1[index]);
                    else
                      stringList2.Add(stringList2[stringList2.Count - 1] + "\\" + strArray1[index]);
                    if (!dictionary2.ContainsKey(stringList2[stringList2.Count - 1]))
                    {
                      string[] strArray2 = new string[stringList2.Count];
                      stringList2.CopyTo(0, strArray2, 0, stringList2.Count);
                      dictionary2.Add(stringList2[stringList2.Count - 1], new List<string>((IEnumerable<string>) strArray2));
                    }
                  }
                  if (stringList2.Count > 0)
                  {
                    foreach (string str in stringList2.ToArray())
                    {
                      if (!stringList1.Contains(str))
                        stringList1.Add(str);
                    }
                  }
                  foreach (string str in stringList1.ToArray())
                  {
                    if (!dbResult.Contains(new DataSyncRecord(headTable1.TableName, str, "", "", false)))
                    {
                      string filter1 = DataSyncUtils.ConstructFilter(headTable1, new string[1]
                      {
                        str
                      });
                      string filter2 = filter1;
                      if (headTable1.RecursiveSync)
                        filter2 = DataSyncUtils.ConstructFilter(headTable1, dictionary2[str].ToArray());
                      string dataAsXmlString = this.getDataAsXmlString(headTable1, filter2);
                      dbResult.Add(new DataSyncRecord(headTable1.TableName, str, dataAsXmlString, mappingXmlString, false));
                      if (headTable1.RequireFileSystemSync)
                      {
                        foreach (FileSystemSyncRecord associatedFileSystemEntry in new FileSystemManager("", dataAsXmlString, this.Session).GetAssociatedFileSystemEntries((TableDef) headTable1, fetchFileSystemData))
                        {
                          if (!fileResult.Contains(associatedFileSystemEntry))
                            fileResult.Add(associatedFileSystemEntry);
                        }
                      }
                      this.getExtendedTablesDataAsXmlString(headTable1, filter1, dbResult, fileResult, fetchFileSystemData);
                    }
                  }
                }
                else
                {
                  string filter3 = DataSyncUtils.ConstructFilter(headTable1, new string[1]
                  {
                    uiKeyValue
                  });
                  string dataAsXmlString = this.getDataAsXmlString(headTable1, filter3);
                  dbResult.Add(new DataSyncRecord(headTable1.TableName, uiKeyValue, dataAsXmlString, mappingXmlString, false));
                  if (headTable1.RequireFileSystemSync)
                  {
                    foreach (FileSystemSyncRecord associatedFileSystemEntry in new FileSystemManager("", dataAsXmlString, this.Session).GetAssociatedFileSystemEntries((TableDef) headTable1, fetchFileSystemData))
                    {
                      if (!fileResult.Contains(associatedFileSystemEntry))
                        fileResult.Add(associatedFileSystemEntry);
                    }
                  }
                  this.getExtendedTablesDataAsXmlString(headTable1, filter3, dbResult, fileResult, fetchFileSystemData);
                }
              }
            }
          }
        }
      }
    }

    private TableDef[] getFileSystemSyncTables(string headTableName)
    {
      TableDef[] relatedTables = this.tableMgr.GetRelatedTables(this.tableMgr.GetHeadTableDef(headTableName), true);
      List<TableDef> tableDefList = new List<TableDef>();
      foreach (TableDef tableDef in relatedTables)
      {
        if (tableDef.RequireFileSystemSync)
          tableDefList.Add(tableDef);
      }
      return tableDefList.ToArray();
    }

    public virtual bool RequireFileSystemSynchronization(string headTableName)
    {
      return this.getFileSystemSyncTables(headTableName).Length != 0;
    }

    private DataSet getData(out string query, HeadTableDef headTable, string filter)
    {
      DataSet data1 = new DataSet();
      string[] tableNames = (string[]) null;
      query = this.constructSqlQueryForRetrievingData(headTable, filter, out tableNames);
      int num = Array.IndexOf<string>(tableNames, (string) null);
      string[] strArray = new string[tableNames.Length - 1];
      if (query == "")
        return data1;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(query);
      DataSet data2 = dbQueryBuilder.ExecuteSetQuery(this.dbTimeout, DbTransactionType.Default);
      if (num >= 0)
      {
        int index1 = 0;
        int index2 = 0;
        while (index1 < strArray.Length)
        {
          if (index1 == num)
            ++index2;
          strArray[index1] = tableNames[index2];
          ++index1;
          ++index2;
        }
        tableNames = strArray;
      }
      for (int index = 0; index < tableNames.Length; ++index)
        data2.Tables[index].TableName = tableNames[index];
      return data2;
    }

    private string constructSqlQueryForRetrievingData(
      HeadTableDef headTable,
      string filter,
      out string[] tableNames)
    {
      List<string> stringList = new List<string>();
      Dictionary<string, List<string>> dictionary1 = new Dictionary<string, List<string>>();
      Dictionary<string, List<string>> extendedForeignKeyQueryList = new Dictionary<string, List<string>>();
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder1.AppendLine("Select * from [" + headTable.TableName + "] " + filter);
      TableDef[] relatedTables = this.tableMgr.GetRelatedTables(headTable, false);
      foreach (TableDef tableDef1 in relatedTables)
      {
        switch (tableDef1)
        {
          case HeadTableDef _ when !(tableDef1 is HeadRelationTableDef):
            dictionary1.Add(tableDef1.TableName, new List<string>());
            string foreignKeyFilter1 = headTable.GetForeignKeyFilter(filter, tableDef1.TableName);
            string str = "Select * from [" + tableDef1.TableName + "] where ";
            string currentFilter = "";
            foreach (TableDef tableDef2 in relatedTables)
            {
              string foreignKeyFilter2 = ((RelationTableDef) tableDef2).GetForeignKeyFilter(headTable.TableName, filter, tableDef1.TableName);
              if ((!(tableDef2 is HeadTableDef) || !(foreignKeyFilter2 == "")) && foreignKeyFilter2 != "")
                currentFilter = !(currentFilter == "") ? currentFilter + " or " + foreignKeyFilter2 : foreignKeyFilter2;
            }
            if (foreignKeyFilter1 != "" && currentFilter != "")
              currentFilter = foreignKeyFilter1 + " or " + currentFilter;
            else if (currentFilter == "" && foreignKeyFilter1 != "")
              currentFilter = foreignKeyFilter1;
            dictionary1[tableDef1.TableName].Add(currentFilter);
            extendedForeignKeyQueryList = this.getExtendedForeignKeyQuery((HeadTableDef) tableDef1, extendedForeignKeyQueryList, currentFilter);
            break;
          case RelationTableDef _:
            if (headTable.RelationTableList.Contains(tableDef1.TableName))
              stringList.Add(tableDef1.TableName);
            RelationTableDef relationTableDef = (RelationTableDef) tableDef1;
            stringList.AddRange((IEnumerable<string>) relationTableDef.RelationTableList.ToArray());
            stringBuilder3.AppendLine(relationTableDef.GetSQLStatementForPrimaryKeyTable(headTable.TableName, filter, this.getTables(relatedTables, relationTableDef.RelationTableList)));
            string foreignKeyColumn = relationTableDef.GetSQLStatementForNonEntryForeignKeyColumn(headTable.TableName, filter);
            if (foreignKeyColumn != "")
            {
              stringBuilder1.AppendLine(" UNION " + foreignKeyColumn);
              break;
            }
            break;
        }
      }
      Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
      Dictionary<string, List<string>> dictionary3 = extendedForeignKeyQueryList;
      foreach (string key in dictionary1.Keys)
      {
        if (dictionary3.ContainsKey(key))
          dictionary3[key].AddRange((IEnumerable<string>) dictionary1[key].ToArray());
        else
          dictionary3.Add(key, dictionary1[key]);
      }
      tableNames = new string[dictionary3.Keys.Count + stringList.Count + 1];
      tableNames[0] = headTable.TableName;
      if (dictionary3.Keys.Contains<string>(headTable.TableName))
        dictionary3.Remove(headTable.TableName);
      dictionary3.Keys.CopyTo(tableNames, 1);
      stringList.CopyTo(tableNames, dictionary3.Count + 1);
      foreach (string key in dictionary3.Keys)
      {
        string str1 = "";
        HeadTableDef headTableDef = this.tableMgr.GetHeadTableDef(key);
        foreach (string str2 in dictionary3[key].ToArray())
          str1 = !(str1 == "") ? str1 + " or " + str2 : str2;
        if (headTableDef.RecursiveSync)
          stringBuilder2.AppendLine("execute " + headTableDef.StoredProcedureName + "'Select * from [" + key + "] where (" + str1.Replace("'", "''") + ")' , '" + headTableDef.Deliminator + "'");
        else
          stringBuilder2.AppendLine("Select * from [" + key + "] where (" + str1 + ")");
      }
      string str3 = stringBuilder1.ToString();
      string str4;
      if (headTable.RecursiveSync)
        str4 = "execute " + headTable.StoredProcedureName + " '" + str3.Replace("'", "''") + "', '" + headTable.Deliminator + "'";
      else
        str4 = "(" + stringBuilder1.ToString() + ") order by " + headTable.UIKeyColumn;
      return str4 + Environment.NewLine + stringBuilder2.ToString() + stringBuilder3.ToString();
    }

    private TableDef[] getTables(TableDef[] source, List<string> tableNames)
    {
      List<TableDef> tableDefList = new List<TableDef>();
      foreach (TableDef tableDef in source)
      {
        if (tableNames.Contains(tableDef.TableName))
          tableDefList.Add(tableDef);
      }
      return tableDefList.ToArray();
    }

    private Dictionary<string, List<string>> getExtendedForeignKeyQuery(
      HeadTableDef headTableDef,
      Dictionary<string, List<string>> extendedForeignKeyQueryList,
      string currentFilter)
    {
      if (headTableDef.ForeignKeys.Count == 0)
        return extendedForeignKeyQueryList;
      foreach (ForeignKey foreignKey in headTableDef.ForeignKeys)
      {
        if (!(foreignKey.PrimaryKeyTableName == ""))
        {
          HeadTableDef headTableDef1 = this.tableMgr.GetHeadTableDef(foreignKey.PrimaryKeyTableName);
          string foreignKeyFilter = headTableDef.GetExtendedForeignKeyFilter(currentFilter, headTableDef1.TableName);
          if (!extendedForeignKeyQueryList.ContainsKey(headTableDef1.TableName))
            extendedForeignKeyQueryList.Add(headTableDef1.TableName, new List<string>());
          extendedForeignKeyQueryList[headTableDef1.TableName].Add(foreignKeyFilter);
          extendedForeignKeyQueryList = this.getExtendedForeignKeyQuery(headTableDef1, extendedForeignKeyQueryList, foreignKeyFilter);
        }
      }
      return extendedForeignKeyQueryList;
    }

    public virtual string GetMappingDataAsXmlString(string headTableName)
    {
      HeadTableDef headTableDef = this.tableMgr.GetHeadTableDef(headTableName);
      if (headTableDef is HeadRelationTableDef)
        headTableDef = this.tableMgr.CreateVirtualHeadTable((HeadRelationTableDef) headTableDef);
      DataSet mappingData = this.getMappingData(headTableDef);
      if (mappingData.Tables[0] != null && mappingData.Tables[0].TableName == "DashboardTemplate")
        this.RemoveDuplicateRows(mappingData.Tables[0], "TemplatePath", "DashboardTemplateID");
      return mappingData.GetXml();
    }

    private void RemoveDuplicateRows(DataTable table, string distinctColumn, string keyColumn)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (table.Rows.Count <= 0)
        return;
      IEnumerable<DataRow> source = table.AsEnumerable().GroupBy<DataRow, string>((System.Func<DataRow, string>) (dr => dr.Field<string>(distinctColumn))).Where<IGrouping<string, DataRow>>((System.Func<IGrouping<string, DataRow>, bool>) (g => g.Count<DataRow>() > 1)).SelectMany<IGrouping<string, DataRow>, DataRow>((System.Func<IGrouping<string, DataRow>, IEnumerable<DataRow>>) (g => (IEnumerable<DataRow>) g)).OrderBy<DataRow, object>((System.Func<DataRow, object>) (dr => dr[keyColumn])).Skip<DataRow>(1);
      if (!source.Any<DataRow>())
        return;
      foreach (DataRow row in source)
      {
        dbQueryBuilder.AppendLine("Delete from DashboardTemplate where DashboardTemplateID=" + row[keyColumn].ToString() + ";");
        table.Rows.Remove(row);
      }
      dbQueryBuilder.Execute();
    }

    private DataSet getMappingData(HeadTableDef headTable)
    {
      DataSet dataSet = new DataSet();
      StringBuilder stringBuilder = new StringBuilder();
      List<string> stringList = new List<string>();
      stringBuilder.AppendLine(headTable.QueryForRetrievingUIKeyAndDBKeyColumns);
      stringList.Add(headTable.TableName);
      List<HeadTableDef> tableList = new List<HeadTableDef>();
      foreach (HeadTableDef headTableDef in this.getRecursiveRelatedHeadTable(headTable, false, tableList))
      {
        if (!stringList.Contains(headTableDef.TableName))
        {
          stringBuilder.AppendLine(headTableDef.QueryForRetrievingUIKeyAndDBKeyColumns);
          stringList.Add(headTableDef.TableName);
        }
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(stringBuilder.ToString());
      DataSet mappingData = dbQueryBuilder.ExecuteSetQuery(this.dbTimeout, DbTransactionType.Default);
      for (int index = 0; index < stringList.Count; ++index)
        mappingData.Tables[index].TableName = stringList[index];
      return mappingData;
    }

    private List<HeadTableDef> getRecursiveRelatedHeadTable(
      HeadTableDef headTable,
      bool foreignKeyOnly,
      List<HeadTableDef> tableList)
    {
      foreach (ForeignKey foreignKey in headTable.ForeignKeys.ToArray())
      {
        if (!(foreignKey.PrimaryKeyTableName == ""))
        {
          HeadTableDef tableDefinition = (HeadTableDef) this.tableMgr.TableDefinitions[foreignKey.PrimaryKeyTableName];
          if (!tableList.Contains(tableDefinition))
            tableList.Add(tableDefinition);
          tableList = this.getRecursiveRelatedHeadTable(tableDefinition, true, tableList);
        }
      }
      if (foreignKeyOnly)
        return tableList;
      foreach (string associateTable in headTable.AssociateTableList)
      {
        HeadTableDef tableDefinition = (HeadTableDef) this.tableMgr.TableDefinitions[associateTable];
        if (!tableList.Contains(tableDefinition))
          tableList.Add(tableDefinition);
        tableList = this.getRecursiveRelatedHeadTable(tableDefinition, true, tableList);
      }
      return tableList;
    }

    private Dictionary<string, Dictionary<string, string>> getKeyMapping(
      string sourceMappingDataString,
      HeadTableDef headTable)
    {
      DataSet mappingData = this.getMappingData(headTable);
      return DataSyncUtils.CreateMapping(sourceMappingDataString, this.tableMgr.GetMappingHeadTables(headTable, true), mappingData);
    }

    public virtual EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo GetImportOption(
      string uiKey,
      string srcDataXmlString,
      string srcMappingDataXmlString,
      string headTableName)
    {
      HeadTableDef headTableDef = this.tableMgr.GetHeadTableDef(headTableName);
      if (headTableDef is HeadRelationTableDef)
        headTableDef = this.tableMgr.CreateVirtualHeadTable((HeadRelationTableDef) headTableDef);
      string filter = DataSyncUtils.ConstructFilter(headTableDef, new string[1]
      {
        uiKey
      });
      return this.getImportOption(srcDataXmlString, srcMappingDataXmlString, headTableName, filter, uiKey);
    }

    private EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo getImportOption(
      string srcDataXmlString,
      string srcMappingDataXmlString,
      string headTableName,
      string filter,
      string uiKeyValue)
    {
      this.onApiCalled(nameof (DataSyncManager), "ImportData", (object[]) null);
      this.Security.DemandRootAdministrator();
      this.createOrgChartWithOrgPath();
      HeadTableDef headTableDef = this.tableMgr.GetHeadTableDef(headTableName);
      if (headTableDef == null)
        return new EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo(headTableName + " is not a head table", "", "", (Dictionary<string, Dictionary<string, string>>) null, "", "");
      if (headTableDef is HeadRelationTableDef)
        headTableDef = this.tableMgr.CreateVirtualHeadTable((HeadRelationTableDef) headTableDef);
      Dictionary<string, Dictionary<string, string>> keyMapping = this.getKeyMapping(srcMappingDataXmlString, headTableDef);
      this.tableMgr.GetRelatedHeadTables(headTableDef, true);
      HeadTableDef[] mappingHeadTables = this.tableMgr.GetMappingHeadTables(headTableDef, true);
      RelationTableDef[] relatedRelationTables = this.tableMgr.GetRelatedRelationTables((RelationTableDef) headTableDef);
      try
      {
        return new EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo("", DataSyncUtils.GetImportOption(srcDataXmlString, mappingHeadTables, relatedRelationTables, keyMapping, headTableDef), headTableDef.CategoryName, uiKeyValue, DataSyncUtils.GetFileSystemImportOption(srcDataXmlString, mappingHeadTables, headTableDef, this.Session));
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo ex)
      {
        return new EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo(ex.ErrorMessage, ex.NewRecordList, headTableDef.CategoryName, uiKeyValue, new List<FileSystemSyncRecord>());
      }
    }

    public virtual EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo ImportData(
      string uiKey,
      string srcDataXmlString,
      string srcMappingDataXmlString,
      string headTableName,
      Dictionary<string, int> newMilestoneMapping,
      List<FileSystemSyncRecord> filesToSync)
    {
      return this.ImportData(uiKey, srcDataXmlString, srcMappingDataXmlString, headTableName, 0, newMilestoneMapping, filesToSync);
    }

    public virtual EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo ImportData(
      string uiKey,
      string srcDataXmlString,
      string srcMappingDataXmlString,
      string headTableName,
      int dummy,
      Dictionary<string, int> newMilestoneMapping,
      List<FileSystemSyncRecord> filesToSync)
    {
      return this.ImportData(uiKey, srcDataXmlString, srcMappingDataXmlString, headTableName, true, newMilestoneMapping, filesToSync);
    }

    public virtual EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo ImportData(
      string uiKey,
      string srcDataXmlString,
      string srcMappingDataXmlString,
      string headTableName,
      bool execute,
      Dictionary<string, int> newMilestoneMapping,
      List<FileSystemSyncRecord> filesToSync)
    {
      HeadTableDef headTableDef = this.tableMgr.GetHeadTableDef(headTableName);
      Dictionary<string, DataSyncDebuggingInfo> dictionary = new Dictionary<string, DataSyncDebuggingInfo>();
      if (headTableDef is HeadRelationTableDef)
        headTableDef = this.tableMgr.CreateVirtualHeadTable((HeadRelationTableDef) headTableDef);
      string filter = DataSyncUtils.ConstructFilter(headTableDef, new string[1]
      {
        uiKey
      });
      return this.importData(srcDataXmlString, srcMappingDataXmlString, headTableName, filter, execute, newMilestoneMapping, filesToSync);
    }

    public virtual List<FileSystemSyncRecord> GetFileToSync(List<FileSystemSyncRecord> filesToSync)
    {
      this.onApiCalled(nameof (DataSyncManager), nameof (GetFileToSync), new object[1]
      {
        (object) filesToSync
      });
      List<FileSystemSyncRecord> systemSyncRecordList = new List<FileSystemSyncRecord>();
      this.Security.DemandRootAdministrator();
      return new FileSystemManager(this.Session).GetData(filesToSync);
    }

    private EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo importData(
      string srcDataXmlString,
      string srcMappingDataXmlString,
      string headTableName,
      string filter,
      bool execute,
      Dictionary<string, int> newMilestoneMapping,
      List<FileSystemSyncRecord> filesToSync)
    {
      this.onApiCalled(nameof (DataSyncManager), "ImportData", (object[]) null);
      this.Security.DemandRootAdministrator();
      this.createOrgChartWithOrgPath();
      HeadTableDef headTableDef1 = this.tableMgr.GetHeadTableDef(headTableName);
      if (headTableDef1 == null)
        return new EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo(headTableName + " is not a head table", "", "", (Dictionary<string, Dictionary<string, string>>) null, "", "");
      bool flag = headTableName == "MilestoneTemplates";
      if (headTableDef1 is HeadRelationTableDef)
        headTableDef1 = this.tableMgr.CreateVirtualHeadTable((HeadRelationTableDef) headTableDef1);
      if (headTableDef1.TableName == "MilestoneTemplates")
        flag = true;
      Dictionary<string, Dictionary<string, string>> keyMapping = this.getKeyMapping(srcMappingDataXmlString, headTableDef1);
      HeadTableDef[] relatedHeadTables = this.tableMgr.GetRelatedHeadTables(headTableDef1, true);
      HeadTableDef[] mappingHeadTables = this.tableMgr.GetMappingHeadTables(headTableDef1, true);
      RelationTableDef[] relatedRelationTables = this.tableMgr.GetRelatedRelationTables((RelationTableDef) headTableDef1);
      XmlDocument destinationXml = DataSyncUtils.TransformSourceToDestinationXml(srcDataXmlString, mappingHeadTables, relatedRelationTables, keyMapping, headTableDef1);
      foreach (TableDef table in relatedHeadTables)
      {
        if (table.TableName == "MilestoneTemplates")
          flag = true;
        if (table.CalculatedFields.Count != 0)
          DataSyncUtils.CopyCalculatedFieldToSource(table.CalculatedFields, destinationXml, table);
      }
      foreach (TableDef table in relatedRelationTables)
      {
        if (table.CalculatedFields.Count != 0)
          DataSyncUtils.CopyCalculatedFieldToSource(table.CalculatedFields, destinationXml, table);
      }
      HeadTableDef headTableDef2 = (HeadTableDef) null;
      RelationTableDef relationTableDef1 = (RelationTableDef) null;
      if (newMilestoneMapping.Count > 0)
      {
        foreach (HeadTableDef headTableDef3 in relatedHeadTables)
        {
          if (headTableDef3.TableName == "CustomMilestone")
          {
            headTableDef2 = headTableDef3;
            break;
          }
        }
        foreach (RelationTableDef relationTableDef2 in relatedRelationTables)
        {
          if (relationTableDef2.TableName == "MilestoneTemplate")
          {
            relationTableDef1 = relationTableDef2;
            break;
          }
        }
        if (headTableDef2 == null && headTableDef1.TableName == "CustomMilestones")
          headTableDef2 = headTableDef1;
        if (headTableDef2 != null && relationTableDef1 != null)
        {
          string xpathWplaceHolder1 = relationTableDef1.GetElementXPathWPlaceHolder("milestoneID");
          string xpathWplaceHolder2 = headTableDef2.GetElementXPathWPlaceHolder(headTableDef2.UIKeyColumn);
          foreach (string key in newMilestoneMapping.Keys)
          {
            XmlNode xmlNode = destinationXml.SelectSingleNode(xpathWplaceHolder2.Replace(DataSyncUtils.ParaPlaceHolder, XML.EncaseXpathString(key)));
            if (xmlNode != null)
            {
              string innerText = xmlNode.ParentNode.SelectSingleNode(headTableDef2.DBKeyColumn).InnerText;
              destinationXml.SelectSingleNode(xpathWplaceHolder1.Replace(DataSyncUtils.ParaPlaceHolder, XML.EncaseXpathString(innerText))).ParentNode.SelectSingleNode("order").InnerText = string.Concat((object) newMilestoneMapping[key]);
            }
          }
        }
      }
      string transformedData = DataSyncDebuggingInfo.FormatXmlDocument(destinationXml);
      Dictionary<string, DataColumnCollection> columnDefs = new Dictionary<string, DataColumnCollection>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      string query = (string) null;
      DataSet data = this.getData(out query, headTableDef1, filter);
      foreach (HeadTableDef headTableDef4 in relatedHeadTables)
        columnDefs.Add(headTableDef4.TableName, data.Tables[headTableDef4.TableName].Columns);
      foreach (RelationTableDef relationTableDef3 in relatedRelationTables)
        columnDefs.Add(relationTableDef3.TableName, data.Tables[relationTableDef3.TableName].Columns);
      string errMsg = (string) null;
      string sqlUpdateStatement = DataSyncUtils.GetSqlUpdateStatement(destinationXml, headTableDef1, relatedHeadTables, relatedRelationTables, columnDefs);
      StringBuilder stringBuilder = new StringBuilder();
      if (newMilestoneMapping.Count > 0 && relationTableDef1 != null)
      {
        StoredProcedureSignature procedureSignature1 = new StoredProcedureSignature("", (List<SqlDbType>) null);
        foreach (StoredProcedureSignature procedureSignature2 in ((List<StoredProcedureSignature>) relationTableDef1.PostSQLActionList["StoredProcedure"]).ToArray())
        {
          if (procedureSignature2.StoreProcedureName == "ResetMilestoneOrder")
          {
            procedureSignature1 = procedureSignature2;
            break;
          }
        }
        if (procedureSignature1.StoreProcedureName != "")
        {
          stringBuilder.AppendLine("-- Post Update Actions ----------------------------------\r\n");
          foreach (string key in newMilestoneMapping.Keys)
            stringBuilder.AppendLine(procedureSignature1.GetStoredProcedureCall(new string[2]
            {
              key,
              string.Concat((object) newMilestoneMapping[key])
            }));
          stringBuilder.AppendLine("-- Post Update Actions ----------------------------------\r\n");
        }
      }
      if (flag)
      {
        StoredProcedureSignature procedureSignature3 = new StoredProcedureSignature("", (List<SqlDbType>) null);
        List<StoredProcedureSignature> procedureSignatureList = new List<StoredProcedureSignature>();
        if (headTableDef1.TableName == "MilestoneTemplates")
        {
          procedureSignatureList = (List<StoredProcedureSignature>) headTableDef1.PostSQLActionList["StoredProcedure"];
        }
        else
        {
          foreach (HeadTableDef headTableDef5 in relatedHeadTables)
          {
            if (headTableDef5.TableName == "MilestoneTemplates")
            {
              procedureSignatureList = (List<StoredProcedureSignature>) headTableDef5.PostSQLActionList["StoredProcedure"];
              break;
            }
          }
        }
        foreach (StoredProcedureSignature procedureSignature4 in procedureSignatureList.ToArray())
        {
          if (procedureSignature4.StoreProcedureName == "ResetMilestoneTemplatesOrder")
          {
            procedureSignature3 = procedureSignature4;
            break;
          }
        }
        if (procedureSignature3.StoreProcedureName != "")
        {
          stringBuilder.AppendLine("-- Post Update Actions (Milestone Template Order Reset) ----------------------------------\r\n");
          stringBuilder.AppendLine(procedureSignature3.GetStoredProcedureCall(new string[0]));
          stringBuilder.AppendLine("-- Post Update Actions (Milestone Template Order Reset)----------------------------------\r\n");
        }
      }
      else
      {
        StoredProcedureSignature procedureSignature5 = new StoredProcedureSignature("", (List<SqlDbType>) null);
        List<StoredProcedureSignature> procedureSignatureList = new List<StoredProcedureSignature>();
        if (headTableDef1.TableName == "UnderwritingConditionList" || headTableDef1.TableName == "PostClosingConditionList" || headTableDef1.TableName == "StatusOnlineTriggers")
          procedureSignatureList = (List<StoredProcedureSignature>) headTableDef1.PostSQLActionList["StoredProcedure"];
        foreach (StoredProcedureSignature procedureSignature6 in procedureSignatureList.ToArray())
        {
          if (procedureSignature6.StoreProcedureName == "RebuildDocumentTemplates")
          {
            procedureSignature5 = procedureSignature6;
            break;
          }
        }
        if (procedureSignature5.StoreProcedureName != "")
        {
          stringBuilder.AppendLine("-- Post Update Actions (Rebuild DocumentTemplates table) ----------------------------------\r\n");
          stringBuilder.AppendLine(procedureSignature5.GetStoredProcedureCall(new string[0]));
          stringBuilder.AppendLine("-- Post Update Actions (Rebuild DocumentTemplates table)----------------------------------\r\n");
        }
        if (headTableDef1.TableName.Equals("eDisclosureChannels", StringComparison.OrdinalIgnoreCase))
        {
          StoredProcedureSignature procedureSignature7 = ((IEnumerable<StoredProcedureSignature>) headTableDef1.PostSQLActionList["StoredProcedure"]).Where<StoredProcedureSignature>((System.Func<StoredProcedureSignature, bool>) (sqlProcedure => sqlProcedure.StoreProcedureName.Equals("SetEdisclosureChannelElements", StringComparison.OrdinalIgnoreCase))).FirstOrDefault<StoredProcedureSignature>();
          if (procedureSignature7.StoreProcedureName != "")
          {
            stringBuilder.AppendLine("-- Post Update Actions (Update EdisclosureChannelElements table) ----------------------------------\r\n");
            stringBuilder.AppendLine(procedureSignature7.GetStoredProcedureCall(new string[7]
            {
              this.PostSqlParameterNameValues["DefaultChannel"],
              this.PostSqlParameterNameValues["AllowESigningConventional"],
              this.PostSqlParameterNameValues["AllowESigningFHA"],
              this.PostSqlParameterNameValues["AllowESigningVA"],
              this.PostSqlParameterNameValues["AllowESigningUSDA"],
              this.PostSqlParameterNameValues["AllowESigningOther"],
              this.PostSqlParameterNameValues["AllowESigningHELOC"]
            }));
            stringBuilder.AppendLine("-- Post Update Actions (Update EdisclosureChannelElements table)----------------------------------\r\n");
          }
        }
        if (headTableDef1.TableName.Equals("Milestones", StringComparison.OrdinalIgnoreCase) && this.PostSqlParameterNameValues != null && this.PostSqlParameterNameValues.ContainsKey("MilestoneNames"))
        {
          StoredProcedureSignature procedureSignature8 = ((IEnumerable<StoredProcedureSignature>) headTableDef1.PostSQLActionList["StoredProcedure"]).Where<StoredProcedureSignature>((System.Func<StoredProcedureSignature, bool>) (sqlProcedure => sqlProcedure.StoreProcedureName.Equals("SetNewMilestoneIDsForRelatedTables", StringComparison.OrdinalIgnoreCase))).FirstOrDefault<StoredProcedureSignature>();
          if (procedureSignature8.StoreProcedureName != "")
          {
            stringBuilder.AppendLine("-- Post Update Actions (Update Milestone IDs For Related Tables) ----------------------------------\r\n");
            string[] strArray = this.PostSqlParameterNameValues["MilestoneNames"].Split('^');
            foreach (string str1 in strArray)
            {
              string str2 = str1.Split(',')[0];
              string str3 = str1.Split(',')[1];
              stringBuilder.AppendLine(procedureSignature8.GetStoredProcedureCall(new string[2]
              {
                str2,
                str3
              }));
            }
            stringBuilder.AppendLine("-- Post Update Actions (Update Milestone IDs For Related Tables) ----------------------------------\r\n");
          }
        }
        if ((headTableDef1.TableName.Equals("EscrowFee", StringComparison.OrdinalIgnoreCase) || headTableDef1.TableName.Equals("TitleFee", StringComparison.OrdinalIgnoreCase)) && this.PostSqlParameterNameValues != null && this.PostSqlParameterNameValues.ContainsKey("DefaultFeeNames"))
        {
          StoredProcedureSignature procedureSignature9 = ((IEnumerable<StoredProcedureSignature>) headTableDef1.PostSQLActionList["StoredProcedure"]).Where<StoredProcedureSignature>((System.Func<StoredProcedureSignature, bool>) (sqlProcedure => sqlProcedure.StoreProcedureName.Equals("SetDefaultFeeName", StringComparison.OrdinalIgnoreCase))).FirstOrDefault<StoredProcedureSignature>();
          if (procedureSignature9.StoreProcedureName != "")
          {
            stringBuilder.AppendLine(string.Format("-- Post Update Actions (Update {0} table) ----------------------------------\r\n", (object) headTableDef1.TableName));
            string str4 = this.PostSqlParameterNameValues["DefaultFeeNames"].ToString();
            char[] chArray = new char[1]{ ',' };
            foreach (string str5 in str4.Split(chArray))
              stringBuilder.AppendLine(procedureSignature9.GetStoredProcedureCall(new string[2]
              {
                str5,
                headTableDef1.TableName.Substring(0, 1)
              }));
            stringBuilder.AppendLine(string.Format("-- Post Update Actions (Update {0} table)----------------------------------\r\n", (object) headTableDef1.TableName));
          }
        }
        if (headTableDef1.TableName.Equals("DocEngineStackingOrders", StringComparison.OrdinalIgnoreCase) && this.PostSqlParameterNameValues != null && this.PostSqlParameterNameValues.ContainsKey("DefaultStackingTemplateName") && this.PostSqlParameterNameValues.ContainsKey("DefaultStackingTemplateOrderType"))
        {
          StoredProcedureSignature procedureSignature10 = ((IEnumerable<StoredProcedureSignature>) headTableDef1.PostSQLActionList["StoredProcedure"]).Where<StoredProcedureSignature>((System.Func<StoredProcedureSignature, bool>) (sqlProcedure => sqlProcedure.StoreProcedureName.Equals("SetDefaultEDisclosureStackingTemplate", StringComparison.OrdinalIgnoreCase))).FirstOrDefault<StoredProcedureSignature>();
          if (procedureSignature10.StoreProcedureName != "")
          {
            stringBuilder.AppendLine(string.Format("-- Post Update Actions (Update {0} table) ----------------------------------\r\n", (object) "DocEngineStackingOrders"));
            stringBuilder.AppendLine(procedureSignature10.GetStoredProcedureCall(new string[2]
            {
              this.PostSqlParameterNameValues["DefaultStackingTemplateName"],
              this.PostSqlParameterNameValues["DefaultStackingTemplateOrderType"]
            }));
            stringBuilder.AppendLine(string.Format("-- Post Update Actions (Update {0} table)----------------------------------\r\n", (object) "DocEngineStackingOrders"));
          }
        }
        if (headTableDef1.TableName.Equals("BR_Triggers", StringComparison.OrdinalIgnoreCase) && this.PostSqlParameterNameValues != null && this.PostSqlParameterNameValues.ContainsKey("MilestoneData"))
        {
          StoredProcedureSignature procedureSignature11 = ((IEnumerable<StoredProcedureSignature>) headTableDef1.PostSQLActionList["StoredProcedure"]).Where<StoredProcedureSignature>((System.Func<StoredProcedureSignature, bool>) (sqlProcedure => sqlProcedure.StoreProcedureName.Equals("UpdateMilestoneGuidForBRTriggers", StringComparison.OrdinalIgnoreCase))).FirstOrDefault<StoredProcedureSignature>();
          if (procedureSignature11.StoreProcedureName != "")
          {
            stringBuilder.AppendLine("-- Post Update Actions (Update Milestone Guid for BR_Triggers) ----------------------------------\r\n");
            string parameterNameValue = this.PostSqlParameterNameValues["MilestoneData"];
            char[] chArray1 = new char[1]{ '^' };
            foreach (string str in parameterNameValue.Split(chArray1))
            {
              char[] chArray2 = new char[1]{ ',' };
              string[] strArray = str.Split(chArray2);
              stringBuilder.AppendLine(procedureSignature11.GetStoredProcedureCall(new string[4]
              {
                strArray[0],
                strArray[1],
                strArray[2],
                strArray[3]
              }));
            }
            stringBuilder.AppendLine("-- Post Update Actions (Update Milestone Guid for BR_Triggers)----------------------------------\r\n");
          }
        }
      }
      if (stringBuilder.Length > 0)
        sqlUpdateStatement += stringBuilder.ToString();
      if (execute)
      {
        try
        {
          EllieMae.EMLite.Server.ServerGlobals.TraceLog.WriteError("DataSyncManager.cs", "Execute sql: " + sqlUpdateStatement);
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.Append(sqlUpdateStatement);
          dbQueryBuilder.ExecuteNonQuery(this.dbTimeout, DbTransactionType.Default);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (DataSyncManager), ex.Message + Environment.NewLine + sqlUpdateStatement);
          Err.Reraise(nameof (DataSyncManager), ex, this.Session.SessionInfo);
        }
      }
      FileSystemManager fileSystemManager = new FileSystemManager(this.Session);
      foreach (FileSystemSyncRecord fileSystemRecord in filesToSync)
        fileSystemManager.Update(fileSystemRecord);
      this.Session.Context.RefreshCache(false);
      return new EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo(errMsg, query, srcDataXmlString, keyMapping, transformedData, sqlUpdateStatement);
    }

    public virtual Dictionary<string, string> GetHeadTablesUIKeys()
    {
      Dictionary<string, string> headTablesUiKeys = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (HeadTableDef headTable in this.tableMgr.HeadTables)
        headTablesUiKeys.Add(headTable.TableName, headTable.UIKeyColumn);
      return headTablesUiKeys;
    }

    public virtual Dictionary<string, string> GetHeadTablesCategoryNames()
    {
      Dictionary<string, string> tablesCategoryNames = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (HeadTableDef headTable in this.tableMgr.HeadTables)
        tablesCategoryNames.Add(headTable.TableName, headTable.CategoryName);
      return tablesCategoryNames;
    }
  }
}
