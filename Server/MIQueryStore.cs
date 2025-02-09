// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.MIQueryStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class MIQueryStore
  {
    private const string className = "MIQueryStore�";

    public static int AddMIRecord(
      MIRecord miRecord,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      string tableId = MIQueryStore.getTableID(miType, isForDownload);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@scenarioID", "int");
      TraceLog.WriteInfo(nameof (MIQueryStore), "MIQueryStore.AddMIRecord: Creating SQL commands for table '" + tableId + "', Tab Name '" + tabName + ".");
      if (miType == LoanTypeEnum.Conventional)
      {
        if (tabName.ToLower() == "general")
          tabName = "";
        dbQueryBuilder.AppendLine("INSERT INTO " + tableId + " ([TabName], [ScenarioKey], [Premium1st], [Subsequent], [Monthly1st], [Months1st], [Monthly2st], [Months2st], [Cutoff]) VALUES (" + SQL.Encode((object) tabName) + "," + SQL.Encode((object) miRecord.ScenarioKey) + "," + (object) miRecord.Premium1st + "," + (object) miRecord.SubsequentPremium + "," + (object) miRecord.Monthly1st + "," + (object) miRecord.Months1st + "," + (object) miRecord.Monthly2st + "," + (object) miRecord.Months2st + "," + (object) miRecord.Cutoff + ")");
      }
      else
        dbQueryBuilder.AppendLine("INSERT INTO " + tableId + " ([ScenarioKey], [Premium1st], [Subsequent], [Monthly1st], [Months1st], [Monthly2st], [Months2st], [Cutoff]) VALUES (" + SQL.Encode((object) miRecord.ScenarioKey) + "," + (object) miRecord.Premium1st + "," + (object) miRecord.SubsequentPremium + "," + (object) miRecord.Monthly1st + "," + (object) miRecord.Months1st + "," + (object) miRecord.Monthly2st + "," + (object) miRecord.Months2st + "," + (object) miRecord.Cutoff + ")");
      dbQueryBuilder.SelectIdentity("@scenarioID");
      dbQueryBuilder.Select("@scenarioID");
      try
      {
        if (miRecord.Scenarios != null)
        {
          for (int index = 0; index < miRecord.Scenarios.Length; ++index)
            dbQueryBuilder.AppendLine("INSERT INTO " + tableId + "Scenarios ([id], [QueryField], [QueryDescription], [QueryType], [QueryOperators], [ValueFrom], [ValueTo], [JointToken], [LeftParentheses], [RightParentheses], [ScenarioOrder]) VALUES (@scenarioID," + SQL.Encode((object) miRecord.Scenarios[index].FieldID) + "," + SQL.Encode((object) miRecord.Scenarios[index].FieldDescription) + "," + ((int) miRecord.Scenarios[index].FieldType).ToString() + "," + SQL.Encode((object) miRecord.Scenarios[index].OperatorType.ToString()) + "," + SQL.Encode((object) miRecord.Scenarios[index].ValueFrom) + "," + SQL.Encode((object) miRecord.Scenarios[index].ValueTo) + "," + ((int) miRecord.Scenarios[index].JointToken).ToString() + "," + miRecord.Scenarios[index].LeftParentheses.ToString() + "," + miRecord.Scenarios[index].RightParentheses.ToString() + "," + (object) index + ")");
        }
        TraceLog.WriteInfo(nameof (MIQueryStore), dbQueryBuilder.ToString());
        return Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot update MI table '" + tableId + "' Due to the following problem:\r\n" + ex.Message);
      }
    }

    public static void DeleteMIRecord(
      int id,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      string tableId = MIQueryStore.getTableID(miType, isForDownload);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        string text = "DELETE FROM " + tableId + " Where [id] = " + (object) id;
        dbQueryBuilder.AppendLine(text);
        TraceLog.WriteInfo(nameof (MIQueryStore), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot delete MI record from table '" + tableId + "'.\r\n" + ex.Message);
      }
    }

    public static void UpdateMIOrder(
      List<int[]> ids,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      string tableId = MIQueryStore.getTableID(miType, isForDownload);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      for (int index = 0; index < ids.Count; ++index)
      {
        int[] id = ids[index];
        dbQueryBuilder.AppendLine("UPDATE " + tableId + " SET [GroupOrder] = " + (object) id[1] + " WHERE [Id] = " + (object) id[0]);
      }
      try
      {
        TraceLog.WriteInfo(nameof (MIQueryStore), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot update MI table '" + tableId + "' group order.\r\n" + ex.Message);
      }
    }

    public static bool HasDuplicateMIRecord(
      int id,
      string scenarioKey,
      LoanTypeEnum miType,
      string tabName)
    {
      string tableId = MIQueryStore.getTableID(miType, false);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        string text = "Select id from " + tableId + " Where [ScenarioKey] = " + SQL.Encode((object) scenarioKey);
        if (miType == LoanTypeEnum.Conventional)
        {
          if (tabName.ToLower() == "general")
            tabName = string.Empty;
          text = text + " And [TabName] = " + SQL.Encode((object) tabName);
        }
        if (id > -1)
          text = text + " And [id] <> " + (object) id;
        dbQueryBuilder.AppendLine(text);
        if (dbQueryBuilder.ExecuteScalar() is int num)
        {
          if (num > 0)
            return true;
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot access table '" + tableId + "'.\r\n" + ex.Message);
      }
      return false;
    }

    public static void UpdateMIRecord(
      MIRecord miRecord,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      string tableId = MIQueryStore.getTableID(miType, isForDownload);
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      try
      {
        string text1 = "DELETE FROM " + tableId + "Scenarios Where [id] = " + (object) miRecord.Id;
        dbQueryBuilder1.AppendLine(text1);
        string text2 = "UPDATE " + tableId + " SET [ScenarioKey] = " + SQL.Encode((object) miRecord.ScenarioKey) + ",[Premium1st] = " + (object) miRecord.Premium1st + ",[Subsequent] = " + (object) miRecord.SubsequentPremium + ",[Monthly1st] = " + (object) miRecord.Monthly1st + ",[Months1st] = " + (object) miRecord.Months1st + ",[Monthly2st] = " + (object) miRecord.Monthly2st + ",[Months2st] = " + (object) miRecord.Months2st + ",[Cutoff] = " + (object) miRecord.Cutoff + "WHERE id = " + (object) miRecord.Id;
        if (tabName.ToLower() == "general")
          tabName = string.Empty;
        if (miType == LoanTypeEnum.Conventional)
          text2 = text2 + " And [TabName] = " + SQL.Encode((object) tabName);
        dbQueryBuilder1.AppendLine(text2);
        if (miRecord.Scenarios != null)
        {
          for (int index = 0; index < miRecord.Scenarios.Length; ++index)
          {
            DbQueryBuilder dbQueryBuilder2 = dbQueryBuilder1;
            object[] objArray = new object[25];
            objArray[0] = (object) "INSERT INTO ";
            objArray[1] = (object) tableId;
            objArray[2] = (object) "Scenarios ([id], [QueryField], [QueryDescription], [QueryType], [QueryOperators], [ValueFrom], [ValueTo], [JointToken], [LeftParentheses], [RightParentheses], [ScenarioOrder]) VALUES (";
            objArray[3] = (object) miRecord.Id;
            objArray[4] = (object) ",";
            objArray[5] = (object) SQL.Encode((object) miRecord.Scenarios[index].FieldID);
            objArray[6] = (object) ",";
            objArray[7] = (object) SQL.Encode((object) miRecord.Scenarios[index].FieldDescription);
            objArray[8] = (object) ",";
            int num = (int) miRecord.Scenarios[index].FieldType;
            objArray[9] = (object) num.ToString();
            objArray[10] = (object) ",";
            objArray[11] = (object) SQL.Encode((object) miRecord.Scenarios[index].OperatorType.ToString());
            objArray[12] = (object) ",";
            objArray[13] = (object) SQL.Encode((object) miRecord.Scenarios[index].ValueFrom);
            objArray[14] = (object) ",";
            objArray[15] = (object) SQL.Encode((object) miRecord.Scenarios[index].ValueTo);
            objArray[16] = (object) ",";
            num = (int) miRecord.Scenarios[index].JointToken;
            objArray[17] = (object) num.ToString();
            objArray[18] = (object) ",";
            num = miRecord.Scenarios[index].LeftParentheses;
            objArray[19] = (object) num.ToString();
            objArray[20] = (object) ",";
            num = miRecord.Scenarios[index].RightParentheses;
            objArray[21] = (object) num.ToString();
            objArray[22] = (object) ",";
            objArray[23] = (object) index;
            objArray[24] = (object) ")";
            string text3 = string.Concat(objArray);
            dbQueryBuilder2.AppendLine(text3);
          }
        }
        dbQueryBuilder1.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot update MI record in table '" + tableId + "'.\r\n" + ex.Message);
      }
    }

    public static void UpdateMITable(
      MIRecord[] miRecords,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      string tableId = MIQueryStore.getTableID(miType, isForDownload);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE FROM " + tableId);
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot delete MI table '" + tableId + "'.\r\n" + ex.Message);
      }
      for (int index = 0; index < miRecords.Length; ++index)
        MIQueryStore.AddMIRecord(miRecords[index], miType, tabName, isForDownload);
    }

    public static MIRecord[] GetMIRecords(LoanTypeEnum miType, string tabName)
    {
      try
      {
        MIRecord[] miRecords1 = MIQueryStore.GetMIRecords(miType, tabName, false);
        MIRecord[] miRecords2 = (MIRecord[]) null;
        if (miType == LoanTypeEnum.FHA || miType == LoanTypeEnum.VA || miType == LoanTypeEnum.USDA)
          miRecords2 = MIQueryStore.GetMIRecords(miType, tabName, true);
        if (miRecords2 == null || miRecords2.Length == 0)
          return miRecords1;
        if (miRecords1 == null || miRecords2.Length == 0)
          return miRecords2;
        MIRecord[] miRecords3 = new MIRecord[miRecords1.Length + miRecords2.Length];
        int num = 0;
        for (int index = 0; index < miRecords1.Length; ++index)
          miRecords3[num++] = miRecords1[index];
        for (int index = 0; index < miRecords2.Length; ++index)
          miRecords3[num++] = miRecords2[index];
        return miRecords3;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot read records from MI table.\r\n" + ex.Message);
      }
    }

    public static MIRecord[] GetMIRecords(LoanTypeEnum miType, string tabName, bool isForDownload)
    {
      string tableId = MIQueryStore.getTableID(miType, isForDownload);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str = "Select queryTable.*, scenarioTable.* from " + tableId + " queryTable join " + tableId + "Scenarios scenarioTable on queryTable.id = scenarioTable.id";
      if (miType == LoanTypeEnum.Conventional && tabName != string.Empty)
        str = !(tabName.ToLower() == "general") ? str + " WHERE queryTable.TabName = " + SQL.Encode((object) tabName) : str + " WHERE (queryTable.TabName = '' Or queryTable.TabName is NULL)";
      string text = str + " order by queryTable.[id], queryTable.[GroupOrder], scenarioTable.[id], scenarioTable.[ScenarioOrder] ASC";
      dbQueryBuilder.AppendLine(text);
      MIRecord miRecord = (MIRecord) null;
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      try
      {
        TraceLog.WriteInfo(nameof (MIQueryStore), dbQueryBuilder.ToString());
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        int num1 = -1;
        for (int index = 0; index < dataTable.Rows.Count; ++index)
        {
          DataRow row = dataTable.Rows[index];
          int num2 = Utils.ParseInt((object) row["id"].ToString());
          if (num1 != num2)
          {
            if (miRecord != null)
            {
              miRecord.Scenarios = (FieldFilter[]) arrayList2.ToArray(typeof (FieldFilter));
              arrayList1.Add((object) miRecord);
            }
            miRecord = new MIRecord(row);
            arrayList2 = new ArrayList();
            num1 = num2;
          }
          arrayList2.Add((object) new FieldFilter(row));
        }
        if (arrayList2.Count > 0)
          miRecord.Scenarios = (FieldFilter[]) arrayList2.ToArray(typeof (FieldFilter));
        if (miRecord != null)
          arrayList1.Add((object) miRecord);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot read records from MI table '" + tableId + "'.\r\n" + ex.Message);
      }
      return arrayList1.Count == 0 ? (MIRecord[]) null : (MIRecord[]) arrayList1.ToArray(typeof (MIRecord));
    }

    public static MIRecordXML ExportMIRecord(
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload)
    {
      MIRecord[] miRecords = MIQueryStore.GetMIRecords(miType, tabName, isForDownload);
      if (miRecords == null || miRecords.Length == 0)
        return (MIRecordXML) null;
      MIRecordXML miRecordXml = new MIRecordXML();
      for (int index = 0; index < miRecords.Length; ++index)
        miRecordXml.InsertMIRecord(miRecords[index]);
      return miRecordXml;
    }

    private static string getTableID(LoanTypeEnum miType, bool isForDownload)
    {
      switch (miType)
      {
        case LoanTypeEnum.Conventional:
          return "MIConv";
        case LoanTypeEnum.USDA:
          return isForDownload ? "MIUSDADownload" : "MIUSDA";
        case LoanTypeEnum.FHA:
          return isForDownload ? "MIFHADownload" : "MIFHA";
        case LoanTypeEnum.VA:
          return isForDownload ? "MIVADownload" : "MIVA";
        case LoanTypeEnum.Other:
          return "MIOther";
        default:
          throw new Exception("Cannot get correct MI table name.");
      }
    }

    private static int convetToTableNo(string tableID)
    {
      switch (tableID)
      {
        case "MIConv":
          return 1;
        case "MIFHA":
          return 2;
        case "MIVA":
          return 3;
        case "MIOther":
          return 4;
        case "MIFHADownload":
          return 5;
        case "MIVADownload":
          return 6;
        default:
          throw new Exception("Cannot get correct loan type ID.");
      }
    }

    public static void AddMITab(string tabName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("INSERT INTO MITabNames ([TabName]) VALUES (" + SQL.Encode((object) tabName) + ")");
      try
      {
        TraceLog.WriteInfo(nameof (MIQueryStore), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot add MI Tab Name to table 'MITabNames'.\r\n" + ex.Message);
      }
    }

    public static void DeleteMITab(string tabName)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      dbQueryBuilder1.AppendLine("DELETE FROM MIConv Where [TabName] = " + SQL.Encode((object) tabName));
      try
      {
        TraceLog.WriteInfo(nameof (MIQueryStore), dbQueryBuilder1.ToString());
        dbQueryBuilder1.ExecuteNonQuery();
        DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
        dbQueryBuilder2.AppendLine("DELETE FROM MITabNames Where [TabName] = " + SQL.Encode((object) tabName));
        TraceLog.WriteInfo(nameof (MIQueryStore), dbQueryBuilder2.ToString());
        dbQueryBuilder2.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot delete MI Tab Name from table 'MITabNames'.\r\n" + ex.Message);
      }
    }

    public static void UpdateMITab(string oldTabName, string newTabName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("UPDATE MIConv SET [TabName] = " + SQL.Encode((object) newTabName) + " WHERE [TabName] = " + SQL.Encode((object) oldTabName));
        dbQueryBuilder.AppendLine("UPDATE MITabNames SET [TabName] = " + SQL.Encode((object) newTabName) + " WHERE [TabName] = " + SQL.Encode((object) oldTabName));
        TraceLog.WriteInfo(nameof (MIQueryStore), dbQueryBuilder.ToString());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot update MI Tab Name in table 'MITabNames'.\r\n" + ex.Message);
      }
    }

    public static bool HadDuplicateMITab(string tabName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("Select id from MITabNames Where [TabName] = " + SQL.Encode((object) tabName));
        if (dbQueryBuilder.ExecuteScalar() is int num)
        {
          if (num > 0)
            return true;
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot access table 'MITabNames'.\r\n" + ex.Message);
      }
      return false;
    }

    public static string[] GetMITabNames()
    {
      ArrayList arrayList = new ArrayList();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("Select * from MITabNames Order By [id]");
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable != null)
        {
          for (int index = 0; index < dataTable.Rows.Count; ++index)
          {
            DataRow row = dataTable.Rows[index];
            arrayList.Add((object) row["TabName"].ToString());
          }
          return (string[]) arrayList.ToArray(typeof (string));
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MIQueryStore), ex);
        throw new Exception("Cannot access record from 'MITabNames'.\r\n" + ex.Message);
      }
      return (string[]) null;
    }
  }
}
