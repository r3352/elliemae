// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.AutomatedEnhancedConditionBpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public sealed class AutomatedEnhancedConditionBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "AutomatedEnhancedConditionBpmDbAccessor�";
    private const string AUTOMATED_ENHANCED_CONDITION_RULE_FIELD_FILTERS = "BR_AutomatedEnhancedConditionRuleFieldFilters�";

    public AutomatedEnhancedConditionBpmDbAccessor()
      : base(ClientSessionCacheID.BpmAutomatedEnhancedConditions)
    {
    }

    protected override string RuleTableName => "[BR_AutomatedEnhancedConditionRules]";

    protected override Type RuleType => typeof (AutomatedEnhancedConditionRuleInfo);

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue ruleIdValue)
    {
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[AECR_EnhancedConditions]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_AutomatedEnhancedConditionRuleFieldFilters"), ruleIdValue);
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue ruleIdValue)
    {
      AutomatedEnhancedConditionRuleInfo conditionRuleInfo = (AutomatedEnhancedConditionRuleInfo) rule;
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("[AECR_EnhancedConditions]");
      if (!sql.ToString().Contains("declare @ruleId"))
        sql.Declare("@ruleId", "int");
      if (rule.RuleID > 0)
        sql.SelectVar("@ruleId", (object) rule.RuleID);
      foreach (AutomatedEnhancedCondition condition in conditionRuleInfo.Conditions)
        sql.InsertInto(table, new DbValueList()
        {
          ruleIdValue,
          {
            "conditionType",
            (object) condition.ConditionType
          },
          {
            "conditionName",
            (object) condition.ConditionName
          }
        }, true, false);
      AutomatedEnhancedConditionBpmDbAccessor.generateInsertDataQueryForFieldFilters(sql, rule.AdvancedCodeXML);
    }

    private static void generateInsertDataQueryForFieldFilters(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      string advancedCodeXML)
    {
      if (string.IsNullOrEmpty(advancedCodeXML))
        return;
      FieldFilterList fieldFilterList = (FieldFilterList) new XmlSerializer().Deserialize(advancedCodeXML, typeof (FieldFilterList));
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_AutomatedEnhancedConditionRuleFieldFilters");
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) fieldFilterList)
      {
        DbValueList dbValueList = new DbValueList();
        dbValueList.Add("RuleID", (object) "@ruleId", (IDbEncoder) DbEncoding.None);
        dbValueList.Add("FieldID", (object) fieldFilter.FieldID);
        sql.IfNotExists(table, dbValueList);
        sql.Begin();
        sql.InsertInto(table, dbValueList, true, false);
        sql.End();
      }
    }

    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder1.Append("select * from [BR_AutomatedEnhancedConditionRules] rules where " + filter);
      dbQueryBuilder1.AppendLine("select EnhancedConditions.* from [AECR_EnhancedConditions] EnhancedConditions");
      dbQueryBuilder1.AppendLine("\tinner join [BR_AutomatedEnhancedConditionRules] rules on EnhancedConditions.ruleID = rules.ruleID");
      dbQueryBuilder1.AppendLine("\twhere " + filter);
      dbQueryBuilder1.AppendLine("\torder by EnhancedConditions.conditionType");
      DataSet data = dbQueryBuilder1.ExecuteSetQuery();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_AutomatedEnhancedConditionRuleFieldFilters");
      dbQueryBuilder2.SelectFrom(table);
      DataRowCollection dataRowCollection = dbQueryBuilder2.Execute();
      if ((dataRowCollection == null || dataRowCollection.Count == 0) && data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
      {
        EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
        sql.Declare("@ruleId", "int");
        foreach (DataRow row in (InternalDataCollectionBase) data.Tables[0].Rows)
        {
          string advancedCodeXML = SQL.DecodeString(row["advancedCodeXml"]);
          if (!string.IsNullOrEmpty(advancedCodeXML))
          {
            int num = SQL.DecodeInt(row["ruleID"]);
            sql.SelectVar("@ruleId", (object) num);
            AutomatedEnhancedConditionBpmDbAccessor.generateInsertDataQueryForFieldFilters(sql, advancedCodeXML);
          }
        }
        if (!string.IsNullOrEmpty(sql.ToString()))
          sql.ExecuteNonQuery(DbTransactionType.Default);
      }
      return (BizRuleInfo[]) AutomatedEnhancedConditionBpmDbAccessor.dataSetToRuleInfos(data);
    }

    private static AutomatedEnhancedConditionRuleInfo[] dataSetToRuleInfos(DataSet data)
    {
      DataRelation relation = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      AutomatedEnhancedConditionRuleInfo[] ruleInfos = new AutomatedEnhancedConditionRuleInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        DataRow[] childRows = row.GetChildRows(relation);
        ruleInfos[index] = new AutomatedEnhancedConditionRuleInfo(row, childRows);
      }
      return ruleInfos;
    }

    public static string[] GetMilestoneNamesByRuleIds(int[] ruleIDs)
    {
      List<string> stringList = new List<string>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("SELECT DISTINCT (M.Name + ',' + M.MilestoneID) AS Name FROM BR_AutomatedEnhancedConditionRules R ");
      dbQueryBuilder.Append("INNER JOIN Milestones M ON R.conditionState = M.MilestoneID AND (R.condition = 4 OR R.condition = 8) ");
      dbQueryBuilder.Append(string.Format("WHERE R.RuleID in ({0})", (object) string.Join<int>(",", (IEnumerable<int>) ruleIDs)));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          stringList.Add(dataRow["Name"].ToString());
      }
      return stringList.ToArray();
    }
  }
}
