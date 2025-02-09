// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.DDMFeeRuleDbAccessor
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
  public class DDMFeeRuleDbAccessor
  {
    public static bool DDMFeeRuleExist(string ruleName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.AppendLine(string.Format("SELECT count(*) FROM DDM_FeeRules WHERE ruleName={0}", (object) SQL.EncodeString(ruleName)));
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) != 0;
    }

    public static bool DDMFeeLineExist(string feeLine)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.AppendLine("SELECT count(*) FROM DDM_FeeRules WHERE feeLine='" + feeLine.TrimStart('0') + "'");
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) != 0;
    }

    public static DDMFeeRule[] GetAllDDMFeeRule()
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRules");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.SelectFrom(table);
      return dbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, DDMFeeRule>((System.Func<DataRow, DDMFeeRule>) (row => new DDMFeeRule(row))).ToArray<DDMFeeRule>();
    }

    [PgReady]
    public static DDMFeeRule[] GetAllDDMFeeRulesAndScenarios(bool activeOnly)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRules");
        DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRuleScenario");
        DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRuleValue");
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.SelectFrom(table1);
        if (activeOnly)
          pgDbQueryBuilder.Append(" where status = 1");
        List<DDMFeeRule> list1 = pgDbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, DDMFeeRule>((System.Func<DataRow, DDMFeeRule>) (row => new DDMFeeRule(row))).ToList<DDMFeeRule>();
        pgDbQueryBuilder.Reset();
        pgDbQueryBuilder.SelectFrom(table2);
        if (activeOnly)
          pgDbQueryBuilder.Append(" where status = 1");
        pgDbQueryBuilder.Append(" order by sortOrder asc");
        List<DDMFeeRuleScenario> list2 = pgDbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, DDMFeeRuleScenario>((System.Func<DataRow, DDMFeeRuleScenario>) (row => new DDMFeeRuleScenario(row))).ToList<DDMFeeRuleScenario>();
        pgDbQueryBuilder.Reset();
        pgDbQueryBuilder.SelectFrom(table3);
        List<DDMFeeRuleValue> list3 = pgDbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, DDMFeeRuleValue>((System.Func<DataRow, DDMFeeRuleValue>) (row => new DDMFeeRuleValue(row))).ToList<DDMFeeRuleValue>();
        Dictionary<int, List<DDMFeeRuleValue>> dictionary1 = new Dictionary<int, List<DDMFeeRuleValue>>();
        foreach (DDMFeeRuleValue ddmFeeRuleValue in list3)
        {
          if (dictionary1.ContainsKey(ddmFeeRuleValue.RuleScenarioID))
            dictionary1[ddmFeeRuleValue.RuleScenarioID].Add(ddmFeeRuleValue);
          else
            dictionary1.Add(ddmFeeRuleValue.RuleScenarioID, new List<DDMFeeRuleValue>()
            {
              ddmFeeRuleValue
            });
        }
        Dictionary<int, List<DDMFeeRuleScenario>> dictionary2 = new Dictionary<int, List<DDMFeeRuleScenario>>();
        foreach (DDMFeeRuleScenario ddmFeeRuleScenario in list2)
        {
          if (dictionary2.ContainsKey(ddmFeeRuleScenario.FeeRuleID))
            dictionary2[ddmFeeRuleScenario.FeeRuleID].Add(ddmFeeRuleScenario);
          else
            dictionary2.Add(ddmFeeRuleScenario.FeeRuleID, new List<DDMFeeRuleScenario>()
            {
              ddmFeeRuleScenario
            });
          if (dictionary1.ContainsKey(ddmFeeRuleScenario.RuleID))
            ddmFeeRuleScenario.FeeRuleValues = dictionary1[ddmFeeRuleScenario.RuleID];
        }
        foreach (DDMFeeRule ddmFeeRule in list1)
        {
          if (dictionary2.ContainsKey(ddmFeeRule.RuleID))
            ddmFeeRule.Scenarios = dictionary2[ddmFeeRule.RuleID];
        }
        return list1.ToArray();
      }
      DbTableInfo table4 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRules");
      DbTableInfo table5 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRuleScenario");
      DbTableInfo table6 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRuleValue");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder1.SelectFrom(table4);
      if (activeOnly)
        dbQueryBuilder1.Append(" where status = 1");
      List<DDMFeeRule> list4 = dbQueryBuilder1.Execute().Cast<DataRow>().Select<DataRow, DDMFeeRule>((System.Func<DataRow, DDMFeeRule>) (row => new DDMFeeRule(row))).ToList<DDMFeeRule>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder2.SelectFrom(table5);
      if (activeOnly)
        dbQueryBuilder2.Append(" where status = 1");
      dbQueryBuilder2.Append(" order by sortOrder asc");
      List<DDMFeeRuleScenario> list5 = dbQueryBuilder2.Execute().Cast<DataRow>().Select<DataRow, DDMFeeRuleScenario>((System.Func<DataRow, DDMFeeRuleScenario>) (row => new DDMFeeRuleScenario(row))).ToList<DDMFeeRuleScenario>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder3 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder3.SelectFrom(table6);
      List<DDMFeeRuleValue> list6 = dbQueryBuilder3.Execute().Cast<DataRow>().Select<DataRow, DDMFeeRuleValue>((System.Func<DataRow, DDMFeeRuleValue>) (row => new DDMFeeRuleValue(row))).ToList<DDMFeeRuleValue>();
      Dictionary<int, List<DDMFeeRuleValue>> dictionary3 = new Dictionary<int, List<DDMFeeRuleValue>>();
      foreach (DDMFeeRuleValue ddmFeeRuleValue in list6)
      {
        if (dictionary3.ContainsKey(ddmFeeRuleValue.RuleScenarioID))
          dictionary3[ddmFeeRuleValue.RuleScenarioID].Add(ddmFeeRuleValue);
        else
          dictionary3.Add(ddmFeeRuleValue.RuleScenarioID, new List<DDMFeeRuleValue>()
          {
            ddmFeeRuleValue
          });
      }
      Dictionary<int, List<DDMFeeRuleScenario>> dictionary4 = new Dictionary<int, List<DDMFeeRuleScenario>>();
      foreach (DDMFeeRuleScenario ddmFeeRuleScenario in list5)
      {
        if (dictionary4.ContainsKey(ddmFeeRuleScenario.FeeRuleID))
          dictionary4[ddmFeeRuleScenario.FeeRuleID].Add(ddmFeeRuleScenario);
        else
          dictionary4.Add(ddmFeeRuleScenario.FeeRuleID, new List<DDMFeeRuleScenario>()
          {
            ddmFeeRuleScenario
          });
        if (dictionary3.ContainsKey(ddmFeeRuleScenario.RuleID))
          ddmFeeRuleScenario.FeeRuleValues = dictionary3[ddmFeeRuleScenario.RuleID];
      }
      foreach (DDMFeeRule ddmFeeRule in list4)
      {
        if (dictionary4.ContainsKey(ddmFeeRule.RuleID))
          ddmFeeRule.Scenarios = dictionary4[ddmFeeRule.RuleID];
      }
      return list4.ToArray();
    }

    public static int CreateDDMFeeRule(DDMFeeRule DDMfeerule)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRules");
      if (DDMFeeRuleDbAccessor.DDMFeeRuleExist(DDMfeerule.RuleName))
        return 0;
      DateTime now = DateTime.Now;
      DbValueList values = new DbValueList();
      values.Add("ruleName", (object) DDMfeerule.RuleName);
      values.Add("feeLine", (object) DDMfeerule.FeeLine.TrimStart('0'));
      values.Add("feeType", (object) (int) DDMfeerule.FeeType);
      values.Add("initLESent", (object) Convert.ToInt32(DDMfeerule.InitLESent));
      values.Add("condition", (object) Convert.ToInt32(DDMfeerule.Condition));
      values.Add("conditionState", (object) DDMfeerule.ConditionState);
      values.Add("lastModifiedByFullName", (object) DDMfeerule.LastModByFullName);
      values.Add("lastModByUserID", (object) DDMfeerule.LastModByUserID);
      values.Add("createDT", (object) now);
      values.Add("LastModTime", (object) now);
      values.Add("status", (object) (int) DDMfeerule.Status);
      values.Add("advCodeCondXML", (object) DDMfeerule.AdvCodeConditionXML);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@ruleId", "int");
      DbValue dbValue = new DbValue("ruleId", (object) "@ruleId", (IDbEncoder) DbEncoding.None);
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.SelectIdentity("@ruleId");
      dbQueryBuilder.Select("@ruleId");
      object obj = dbQueryBuilder.ExecuteScalar();
      DDMfeerule.RuleID = Convert.ToInt32(obj);
      return DDMfeerule.RuleID;
    }

    public static DDMFeeRule GetDDMFeeRuleAndScenarioByID(int ruleID)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRules");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      DbValue key = new DbValue(nameof (ruleID), (object) ruleID);
      dbQueryBuilder.SelectFrom(table, key);
      return new DDMFeeRule(dbQueryBuilder.Execute()[0]);
    }

    public static DDMFeeRule GetDDMFeeRule(int ruleID)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRules");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      DbValue key = new DbValue(nameof (ruleID), (object) ruleID);
      dbQueryBuilder.SelectFrom(table, key);
      return new DDMFeeRule(dbQueryBuilder.Execute()[0]);
    }

    public static void UpdateDDMFeeRuleByID(int ruleID, DDMFeeRule feeRule)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRules");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue(nameof (ruleID), (object) ruleID);
      dbQueryBuilder.Update(table, new DbValueList()
      {
        {
          "ruleName",
          (object) feeRule.RuleName
        },
        {
          "feeLine",
          (object) feeRule.FeeLine.TrimStart('0')
        },
        {
          "feeType",
          (object) (int) feeRule.FeeType
        },
        {
          "initLESent",
          (object) Convert.ToInt32(feeRule.InitLESent)
        },
        {
          "condition",
          (object) Convert.ToInt32(feeRule.Condition)
        },
        {
          "conditionState",
          (object) feeRule.ConditionState
        },
        {
          "lastModifiedByFullName",
          (object) feeRule.LastModByFullName
        },
        {
          "lastModByUserID",
          (object) feeRule.LastModByUserID
        },
        {
          "createDT",
          (object) feeRule.CreateDt
        },
        {
          "LastModTime",
          (object) DateTime.Now
        },
        {
          "status",
          (object) (int) feeRule.Status
        },
        {
          "advCodeCondXML",
          (object) feeRule.AdvCodeConditionXML
        }
      }, key);
      dbQueryBuilder.Execute();
    }

    public static DDMFeeRule[] GetFeeRulesByDataTable(string dataTableName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      string text = "SELECT A.* FROM DDM_FeeRuleScenario A inner join DDM_FeeRuleValue B on A.ruleID = B.feeRuleScenarioID and B.fieldValue like '%DDM|" + dataTableName + "%' and B.fieldValueType=3";
      dbQueryBuilder.AppendLine(text);
      List<DDMFeeRuleScenario> list1 = dbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, DDMFeeRuleScenario>((System.Func<DataRow, DDMFeeRuleScenario>) (row => new DDMFeeRuleScenario(row))).ToList<DDMFeeRuleScenario>();
      List<DDMFeeRule> source = new List<DDMFeeRule>();
      foreach (DDMFeeRuleScenario ddmFeeRuleScenario in list1)
      {
        DDMFeeRuleScenario feescenariorule = ddmFeeRuleScenario;
        DDMFeeRule ddmFeeRule1 = (DDMFeeRule) null;
        if (source.Count > 0)
          ddmFeeRule1 = source.Where<DDMFeeRule>((System.Func<DDMFeeRule, bool>) (x => x.RuleID == feescenariorule.FeeRuleID)).FirstOrDefault<DDMFeeRule>();
        if (ddmFeeRule1 == null)
        {
          DDMFeeRule ddmFeeRule2 = DDMFeeRuleDbAccessor.GetDDMFeeRule(feescenariorule.FeeRuleID);
          source.Add(ddmFeeRule2);
        }
      }
      foreach (DDMFeeRule ddmFeeRule in source)
      {
        DDMFeeRule feerule = ddmFeeRule;
        List<DDMFeeRuleScenario> list2 = list1.Where<DDMFeeRuleScenario>((System.Func<DDMFeeRuleScenario, bool>) (x => x.FeeRuleID == feerule.RuleID)).ToList<DDMFeeRuleScenario>();
        feerule.Scenarios = list2;
      }
      return source.ToArray();
    }

    public static void DeleteDDMFeeRuleByID(int ruleID)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRuleScenario");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue("feeRuleID", (object) ruleID);
      string[] columnNames = new string[1]
      {
        nameof (ruleID)
      };
      dbQueryBuilder.SelectFrom(table, columnNames, key);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<int> intList = new List<int>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        intList.Add((int) dataRow[nameof (ruleID)]);
      int[] array = intList.ToArray();
      if (((IEnumerable<int>) array).Count<int>() > 0)
      {
        DDMFeeRuleDbAccessor.DeleteDDMFeeRuleValueByIDs(array);
        DDMFeeRuleDbAccessor.DeleteDDMFeeRuleScenarioByID(ruleID);
      }
      DDMFeeRuleDbAccessor.DeleteDDMFeeRuleFromRuleTableByID(ruleID);
    }

    private static void DeleteDDMFeeRuleValueByIDs(int[] ruleIDs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from DDM_FeeRuleValue where feeRuleScenarioID in (" + string.Join<int>(",", (IEnumerable<int>) ruleIDs) + ")");
      dbQueryBuilder.Execute();
    }

    private static void DeleteDDMFeeRuleScenarioByID(int feeRuleID)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRuleScenario");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue(nameof (feeRuleID), (object) feeRuleID);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.Execute();
    }

    private static void DeleteDDMFeeRuleFromRuleTableByID(int ruleID)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FeeRules");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue(nameof (ruleID), (object) ruleID);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.Execute();
    }

    public static bool IsTableUsedByFeeRule(DDMDataTable dt)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      string text = "select top 1 1\r\n                                from DDM_FeeRuleValue frv\r\n                                inner join DDM_FeeRuleScenario frs\r\n                                on frv.feeRuleScenarioID = frs.ruleID\r\n                                where frv.fieldValueType = 3\r\n                                    and frv.fieldValue like '%DDM|" + dt.Name + "|%'";
      dbQueryBuilder.Append(text);
      return dbQueryBuilder.Execute().Count > 0;
    }

    public static void ResetDataTableFeeRuleValue(DDMDataTable dt)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = "update DDM_FeeRuleValue\r\n                                set fieldValue = '',\r\n                                    fieldValueType = 1\r\n                                where fieldValueType = 3\r\n                                    and fieldValue like '%DDM|" + dt.Name + "|%'";
      dbQueryBuilder.Append(text);
      dbQueryBuilder.Execute();
    }

    public static void UpdateDataTableReference(string oldName, string newName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = "WITH\r\n                  Decomposed AS (\r\n                    SELECT\r\n                      ruleValueId,\r\n                      fieldValue,\r\n                      SUBSTRING(SUBSTRING(fieldValue, 5, LEN(fieldValue)), --chop off ddm\r\n                                0,\r\n                                CHARINDEX('|', SUBSTRING(fieldValue, 5, LEN(fieldValue)))\r\n                                ) AS TableName,\r\n                      REPLACE(fieldValue, 'DDM|{{OldDataTableName}}|', '') AS Remnant\r\n                    FROM DDM_FeeRuleValue\r\n                    WHERE fieldValue like 'DDM|{{OldDataTableName}}|%' AND fieldValueType = 3\r\n                  )\r\n                UPDATE frv\r\n                  SET frv.fieldValue = 'DDM|' + '{{NewDataTableName}}' + '|' + d.Remnant\r\n                FROM DDM_FeeRuleValue AS frv\r\n                  INNER JOIN Decomposed AS d ON (frv.ruleValueID = d.ruleValueID)".Replace("{{NewDataTableName}}", newName).Replace("{{OldDataTableName}}", oldName);
      dbQueryBuilder.AppendLine(text);
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}
