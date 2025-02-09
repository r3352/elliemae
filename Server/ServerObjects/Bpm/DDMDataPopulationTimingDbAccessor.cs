// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.DDMDataPopulationTimingDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class DDMDataPopulationTimingDbAccessor
  {
    private static string ddmDataPopTimingTable = "DDM_DataPopTimingSetting";
    private static string ddmDataPopTimingFieldTable = "DDM_DataPopTimingField";

    public static void UpdateDDMDataPopulationTiming(DDMDataPopulationTiming ddmDataPopTiming)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable(DDMDataPopulationTimingDbAccessor.ddmDataPopTimingTable);
      EllieMae.EMLite.Server.DbQueryBuilder DDMFieldSql = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable(DDMDataPopulationTimingDbAccessor.ddmDataPopTimingFieldTable);
      if (DDMDataPopulationTimingDbAccessor.GetCurrentSetting())
      {
        dbQueryBuilder.Update(table1, DDMDataPopulationTimingDbAccessor.GetDDMSettingDbValueList(ddmDataPopTiming), new DbValueList());
        dbQueryBuilder.Execute();
      }
      else
      {
        dbQueryBuilder.InsertInto(table1, DDMDataPopulationTimingDbAccessor.GetDDMSettingDbValueList(ddmDataPopTiming), true, false);
        dbQueryBuilder.Execute();
      }
      DDMDataPopulationTimingDbAccessor.UpdateDDMFieldTable(DDMFieldSql, table2, ddmDataPopTiming.FieldList);
    }

    public static DDMDataPopulationTiming GetDDMDataPopulationTiming()
    {
      DDMDataPopulationTiming popTimingSetting = DDMDataPopulationTimingDbAccessor.GetDDMDataPopTimingSetting();
      List<DDMDataPopTimingField> popTimeFieldList = DDMDataPopulationTimingDbAccessor.GetDDMDataPopTimeFieldList();
      popTimingSetting.FieldList = popTimeFieldList;
      return popTimingSetting;
    }

    [PgReady]
    private static DDMDataPopulationTiming GetDDMDataPopTimingSetting()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(DDMDataPopulationTimingDbAccessor.ddmDataPopTimingTable);
        pgDbQueryBuilder.SelectFrom(table);
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        DDMDataPopulationTiming popTimingSetting = new DDMDataPopulationTiming();
        if (dataRowCollection == null || dataRowCollection.Count == 0)
        {
          popTimingSetting.LoanSave = true;
        }
        else
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          {
            popTimingSetting.LoanSave = true;
            popTimingSetting.FieldChanges = dataRow["FieldChanges"] != DBNull.Value && (string) dataRow["FieldChanges"] == "1";
            popTimingSetting.AfterLoanInitEst = dataRow["AfterLoanInitEst"] != DBNull.Value && (string) dataRow["AfterLoanInitEst"] == "1";
            popTimingSetting.LoanCondMet = dataRow["LoanCondMet"] != DBNull.Value && (string) dataRow["LoanCondMet"] == "1";
            popTimingSetting.LoanCondMetCond = dataRow["LoanCondMetCond"] == DBNull.Value ? string.Empty : (string) dataRow["LoanCondMetCond"];
            popTimingSetting.LoanCondMetCondXml = dataRow["LoanCondMetCondXml"] == DBNull.Value ? string.Empty : (string) dataRow["LoanCondMetCondXml"];
          }
        }
        return popTimingSetting;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable(DDMDataPopulationTimingDbAccessor.ddmDataPopTimingTable);
      dbQueryBuilder.SelectFrom(table1);
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      DDMDataPopulationTiming popTimingSetting1 = new DDMDataPopulationTiming();
      if (dataRowCollection1 == null || dataRowCollection1.Count == 0)
      {
        popTimingSetting1.LoanSave = true;
      }
      else
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection1)
        {
          popTimingSetting1.LoanSave = true;
          popTimingSetting1.FieldChanges = dataRow["FieldChanges"] != DBNull.Value && (string) dataRow["FieldChanges"] == "1";
          popTimingSetting1.AfterLoanInitEst = dataRow["AfterLoanInitEst"] != DBNull.Value && (string) dataRow["AfterLoanInitEst"] == "1";
          popTimingSetting1.LoanCondMet = dataRow["LoanCondMet"] != DBNull.Value && (string) dataRow["LoanCondMet"] == "1";
          popTimingSetting1.LoanCondMetCond = dataRow["LoanCondMetCond"] == DBNull.Value ? string.Empty : (string) dataRow["LoanCondMetCond"];
          popTimingSetting1.LoanCondMetCondXml = dataRow["LoanCondMetCondXml"] == DBNull.Value ? string.Empty : (string) dataRow["LoanCondMetCondXml"];
        }
      }
      return popTimingSetting1;
    }

    [PgReady]
    private static List<DDMDataPopTimingField> GetDDMDataPopTimeFieldList()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(DDMDataPopulationTimingDbAccessor.ddmDataPopTimingFieldTable);
        pgDbQueryBuilder.SelectFrom(table);
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        List<DDMDataPopTimingField> popTimeFieldList = new List<DDMDataPopTimingField>();
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          popTimeFieldList.Add(new DDMDataPopTimingField()
          {
            FieldID = (string) dataRow["FieldID"],
            FieldDescription = (string) dataRow["Description"],
            NumReferenced = Convert.ToInt32(dataRow["RuleNumRef"])
          });
        return popTimeFieldList;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable(DDMDataPopulationTimingDbAccessor.ddmDataPopTimingFieldTable);
      dbQueryBuilder.SelectFrom(table1);
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      List<DDMDataPopTimingField> popTimeFieldList1 = new List<DDMDataPopTimingField>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection1)
        popTimeFieldList1.Add(new DDMDataPopTimingField()
        {
          FieldID = (string) dataRow["FieldID"],
          FieldDescription = (string) dataRow["Description"],
          NumReferenced = Convert.ToInt32(dataRow["RuleNumRef"])
        });
      return popTimeFieldList1;
    }

    private static bool GetCurrentSetting()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      string text = "SELECT COUNT(*) FROM DDM_DataPopTimingSetting";
      dbQueryBuilder.AppendLine(text);
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) != 0;
    }

    private static DbValueList GetDDMSettingDbValueList(DDMDataPopulationTiming ddmDataPopTiming)
    {
      return new DbValueList()
      {
        {
          "LoanSave",
          (object) ddmDataPopTiming.LoanSave,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "FieldChanges",
          (object) ddmDataPopTiming.FieldChanges,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "AfterLoanInitEst",
          (object) ddmDataPopTiming.AfterLoanInitEst,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "LoanCondMet",
          (object) ddmDataPopTiming.LoanCondMet,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "LoanCondMetCond",
          (object) ddmDataPopTiming.LoanCondMetCond
        },
        {
          "LoanCondMetCondXml",
          (object) ddmDataPopTiming.LoanCondMetCondXml
        },
        {
          "lastmodByUserID",
          (object) ddmDataPopTiming.UserID
        },
        {
          "lastModDate",
          (object) DateTime.Now
        }
      };
    }

    private static void UpdateDDMFieldTable(
      EllieMae.EMLite.Server.DbQueryBuilder DDMFieldSql,
      DbTableInfo DDMDataFieldTable,
      List<DDMDataPopTimingField> fieldList)
    {
      DDMFieldSql.DeleteFrom(DDMDataFieldTable);
      DDMFieldSql.Execute();
      if (fieldList == null || fieldList.Count <= 0)
        return;
      foreach (DDMDataPopTimingField field in fieldList)
      {
        DDMFieldSql.InsertInto(DDMDataFieldTable, DDMDataPopulationTimingDbAccessor.GetDDMFieldDbValueList(field), true, false);
        DDMFieldSql.Execute();
      }
    }

    private static DbValueList GetDDMFieldDbValueList(DDMDataPopTimingField field)
    {
      return new DbValueList()
      {
        {
          "FieldID",
          (object) field.FieldID
        },
        {
          "Description",
          (object) field.FieldDescription
        },
        {
          "RuleNumRef",
          (object) field.NumReferenced
        },
        {
          "lastmodByUserID",
          (object) field.UserID
        },
        {
          "lastModDate",
          (object) DateTime.Now
        }
      };
    }

    public static List<DDMDataPopTimingField> UpdateDDMDataPopulationTimingNumberOfReferences(
      string userID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text1 = "select count(fieldId) as count , fieldId from (\r\n                            select fieldID,feeRuleScenarioId from (\r\n\r\n                            select pop.fieldID , val.feeRuleScenarioId from DDM_FeeRuleValue as val \r\n                            right outer  join DDM_DataPopTimingField as pop on (pop.FieldID = val.fieldID And  val.fieldValueType > 1)\r\n\t                        group by  pop.fieldID , val.feeRuleScenarioId\r\n                            union all\r\n                            \r\n\t\t\t\t\t\t\tselect pop.fieldid,val.feerulescenarioid from DDM_DataPopTimingField as pop \r\n\t\t\t\t\t\t\tleft  join  DDM_FeeRuleValue as val on (  val.fieldValueType = 4 And (fieldValue  like '%\\[' + pop.FieldId + '\\]%' escape '\\' Or fieldValue  like '%\\[_' + pop.FieldId + '\\]%' escape '\\'  )) \r\n\t\t\t\t\t\t\tgroup by pop.fieldid,val.feerulescenarioid\r\n\t\t\t\t\t\t\tunion all\r\n\r\n\t\t\t\t\t\t\tselect pop.fieldid,val.fieldrulescenarioid from DDM_DataPopTimingField as pop \r\n\t\t\t\t\t\t\tleft  join  DDM_FieldRuleValue as val on (  val.fieldValueType = 4 And (fieldValue  like '%\\[' + pop.FieldId + '\\]%' escape '\\' Or fieldValue  like '%\\[_' + pop.FieldId + '\\]%' escape '\\'  )) \r\n\t\t\t\t\t\t\tgroup by pop.fieldid,val.fieldrulescenarioid\r\n\t\t\t\t\t\t\tunion all\r\n\r\n                            select dtf.fieldid , FRV.feeRuleScenarioId from DDM_DataTableFields as DTF \r\n                            join DDM_DataTables as DT on DTF.dataTableID = DT.dataTableID\r\n                            right outer join DDM_DataPopTimingField as pop on (pop.FieldID = DTF.fieldID And DTF.criteria <> -1)\r\n                            right outer join DDM_FeeRuleValue as FRV on (fieldValueType = 3 And fieldValue like '%|' + dataTableName + '|%')\r\n                            group by dataTableName, dtf.fieldid ,FRV.feeRuleScenarioId\r\n\r\n\t\t\t\t\t\t\tunion all\r\n\r\n\t\t\t\t\t\t\t select pop.fieldID , val.fieldRuleScenarioId from DDM_FieldRuleValue as val \r\n                            right outer join DDM_DataPopTimingField as pop on (pop.FieldID = val.fieldID And  val.fieldValueType > 1)\r\n                            group by  pop.fieldID , val.fieldRuleScenarioId\r\n                            union all\r\n\r\n\t\t\t\t\t\t\t select dtf.fieldid , FRV.fieldRuleScenarioId from DDM_DataTableFields as DTF \r\n                            join DDM_DataTables as DT on DTF.dataTableID = DT.dataTableID\r\n                            right outer join DDM_DataPopTimingField as pop on (pop.FieldID = DTF.fieldID And DTF.criteria <> -1)\r\n                            right outer join DDM_FieldRuleValue as FRV on (fieldValueType = 3 And fieldValue like '%|' + dataTableName + '|%')\r\n                            group by dataTableName, dtf.fieldid ,FRV.fieldRuleScenarioId\r\n\r\n\t\t\t                ) as result where feeRuleScenarioId Is Not Null And fieldID is Not Null group by fieldID , feeRuleScenarioId ) as total\r\n\r\n\t\t\t\t\t\t\tgroup by fieldId";
      dbQueryBuilder1.AppendLine(text1);
      List<DDMDataPopTimingField> list = dbQueryBuilder1.Execute().Cast<DataRow>().Select<DataRow, DDMDataPopTimingField>((System.Func<DataRow, DDMDataPopTimingField>) (row => new DDMDataPopTimingField(userID, row["FieldID"].ToString(), "", Convert.ToInt32(row["count"])))).ToList<DDMDataPopTimingField>();
      List<DDMDataPopTimingField> dataPopTimingFieldList = new List<DDMDataPopTimingField>();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(DDMDataPopulationTimingDbAccessor.ddmDataPopTimingFieldTable);
      foreach (DDMDataPopTimingField dataPopTimingField in list)
      {
        string text2 = string.Format("update {0} set {1} = {2} output INSERTED.* where {3} = '{4}'", (object) table.Name, (object) "RuleNumRef", (object) dataPopTimingField.NumReferenced, (object) "FieldID", (object) dataPopTimingField.FieldID);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder2.AppendLine(text2);
        DataRowCollection source = dbQueryBuilder2.Execute();
        dataPopTimingFieldList.AddRange((IEnumerable<DDMDataPopTimingField>) source.Cast<DataRow>().Select<DataRow, DDMDataPopTimingField>((System.Func<DataRow, DDMDataPopTimingField>) (row => new DDMDataPopTimingField(userID, row["FieldID"].ToString(), row["Description"].ToString(), Convert.ToInt32(row["RuleNumRef"])))).ToList<DDMDataPopTimingField>());
      }
      return dataPopTimingFieldList;
    }

    public static int GetNumberReferences(string fieldId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      string text = "select count(feeRuleScenarioId) as count , feeRuleScenarioId from (\r\n                            select fieldID,feeRuleScenarioId from (\r\n\r\n                            select fieldID , feeRuleScenarioId from DDM_FeeRuleValue \r\n                            where fieldId ='" + fieldId + "' And  fieldValueType > 1\r\n\t                        group by  fieldID , feeRuleScenarioId\r\n                            union all\r\n                            \r\n\t\t\t\t\t\t\tselect fieldid,feerulescenarioid from   DDM_FeeRuleValue \r\n\t\t\t\t\t\t\twhere (fieldValueType = 4 And (fieldValue  like '%\\[' + '" + fieldId + "' + '\\]%' escape '\\' Or fieldValue  like '%\\[_' + '" + fieldId + "' + '\\]%' escape '\\'  )) \r\n\t\t\t\t\t\t\tgroup by fieldid,feerulescenarioid\r\n\t\t\t\t\t\t\tunion all\r\n\r\n                            select dtf.fieldid , FRV.feeRuleScenarioId from DDM_DataTableFields as DTF \r\n                            join DDM_DataTables as DT on DTF.dataTableID = DT.dataTableID\r\n                            right outer join DDM_FeeRuleValue as FRV on (fieldValueType = 3 And fieldValue like '%|' + dataTableName + '|%')\r\n\t\t\t\t\t\t\twhere dtf.fieldId = '" + fieldId + "' and criteria <> -1\r\n                            group by dataTableName, dtf.fieldid ,FRV.feeRuleScenarioId\r\n\t\t\t\t\t\t\tunion all\r\n\r\n\t\t\t\t\t\t\tselect fieldid,fieldrulescenarioid from DDM_FieldRuleValue \r\n\t\t\t\t\t\t\twhere (fieldValueType = 4 And (fieldValue  like '%\\[' + '" + fieldId + "' + '\\]%' escape '\\' Or fieldValue  like '%\\[_' + '" + fieldId + "' + '\\]%' escape '\\'  )) \r\n\t\t\t\t\t\t\tgroup by fieldid,fieldrulescenarioid\r\n\t\t\t\t\t\t\tunion all\r\n\r\n\t\t\t\t\t\t\tselect fieldID , val.fieldRuleScenarioId from DDM_FieldRuleValue as val \r\n\t\t\t\t\t\t\twhere fieldid = '" + fieldId + "' and fieldValueType > 1\r\n                            group by  fieldID , val.fieldRuleScenarioId\r\n                            union all\r\n\r\n\t\t\t\t\t\t\tselect dtf.fieldid , FRV.fieldRuleScenarioId from DDM_DataTableFields as DTF \r\n                            join DDM_DataTables as DT on DTF.dataTableID = DT.dataTableID\r\n                            right outer join DDM_FieldRuleValue as FRV on (fieldValueType = 3 And fieldValue like '%|' + dataTableName + '|%')\r\n\t\t\t\t\t\t\twhere dtf.fieldid = '" + fieldId + "' and DTF.criteria <> -1\r\n                            group by dataTableName, dtf.fieldid ,FRV.fieldRuleScenarioId\r\n\r\n\r\n\t\t\t                ) as result where feeRuleScenarioId Is Not Null And fieldID is Not Null group by fieldID , feeRuleScenarioId ) as total\r\n\r\n\t\t\t\t\t\t\tgroup by feeRuleScenarioId";
      dbQueryBuilder.AppendLine(text);
      return dbQueryBuilder.Execute().Count;
    }
  }
}
