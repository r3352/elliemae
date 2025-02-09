// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.DDMFieldRuleDbAccessor
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
  public class DDMFieldRuleDbAccessor
  {
    public static bool DDMFieldRuleExist(string ruleName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.AppendLine(string.Format("SELECT count(*) FROM DDM_FieldRules WHERE ruleName={0}", (object) SQL.EncodeString(ruleName)));
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) != 0;
    }

    public static bool DDMFieldsExistInFieldRules(string fields)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      if (string.IsNullOrEmpty(fields))
        return false;
      dbQueryBuilder.AppendLine("SELECT count(*) FROM DDM_FieldRuleValue WHERE fieldID in (" + fields + ")");
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) != 0;
    }

    public static DDMFieldRule[] GetAllDDMFieldRule()
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRules");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.SelectFrom(table);
      return dbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, DDMFieldRule>((System.Func<DataRow, DDMFieldRule>) (row => new DDMFieldRule(row))).ToArray<DDMFieldRule>();
    }

    [PgReady]
    public static DDMFieldRule[] GetAllDDMFieldRulesAndScenarios(
      bool activeOnly,
      List<int> fieldRuleIds = null)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRules");
        DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRuleScenario");
        DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRuleValue");
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.SelectFrom(table1);
        if (activeOnly)
          pgDbQueryBuilder.Append(" where status = 1");
        if (fieldRuleIds != null && fieldRuleIds.Count > 0)
        {
          if (!activeOnly)
            pgDbQueryBuilder.Append(" where status = 1");
          pgDbQueryBuilder.AppendLine("and ruleId in (" + string.Join<int>(", ", (IEnumerable<int>) fieldRuleIds) + ")");
        }
        List<DDMFieldRule> list1 = pgDbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, DDMFieldRule>((System.Func<DataRow, DDMFieldRule>) (row => new DDMFieldRule(row))).ToList<DDMFieldRule>();
        pgDbQueryBuilder.Reset();
        pgDbQueryBuilder.SelectFrom(table2);
        if (activeOnly)
          pgDbQueryBuilder.Append(" where status = 1");
        if (fieldRuleIds != null && fieldRuleIds.Count > 0)
        {
          if (!activeOnly)
            pgDbQueryBuilder.Append(" where status = 1");
          pgDbQueryBuilder.AppendLine("and fieldRuleID in (" + string.Join<int>(", ", (IEnumerable<int>) fieldRuleIds) + ")");
        }
        pgDbQueryBuilder.Append(" order by sortOrder asc");
        List<DDMFieldRuleScenario> list2 = pgDbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, DDMFieldRuleScenario>((System.Func<DataRow, DDMFieldRuleScenario>) (row => new DDMFieldRuleScenario(row))).ToList<DDMFieldRuleScenario>();
        pgDbQueryBuilder.Reset();
        pgDbQueryBuilder.SelectFrom(table3);
        List<DDMFeeRuleValue> list3 = pgDbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, DDMFeeRuleValue>((System.Func<DataRow, DDMFeeRuleValue>) (row => new DDMFeeRuleValue(row, false))).ToList<DDMFeeRuleValue>();
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
        Dictionary<int, List<DDMFieldRuleScenario>> dictionary2 = new Dictionary<int, List<DDMFieldRuleScenario>>();
        foreach (DDMFieldRuleScenario fieldRuleScenario in list2)
        {
          if (dictionary2.ContainsKey(fieldRuleScenario.FieldRuleID))
            dictionary2[fieldRuleScenario.FieldRuleID].Add(fieldRuleScenario);
          else
            dictionary2.Add(fieldRuleScenario.FieldRuleID, new List<DDMFieldRuleScenario>()
            {
              fieldRuleScenario
            });
          if (dictionary1.ContainsKey(fieldRuleScenario.RuleID))
            fieldRuleScenario.FieldRuleValues = dictionary1[fieldRuleScenario.RuleID];
        }
        foreach (DDMFieldRule ddmFieldRule in list1)
        {
          if (dictionary2.ContainsKey(ddmFieldRule.RuleID))
            ddmFieldRule.Scenarios = dictionary2[ddmFieldRule.RuleID];
        }
        return list1.ToArray();
      }
      DbTableInfo table4 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRules");
      DbTableInfo table5 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRuleScenario");
      DbTableInfo table6 = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRuleValue");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder1.SelectFrom(table4);
      if (activeOnly)
        dbQueryBuilder1.Append(" where status = 1");
      if (fieldRuleIds != null && fieldRuleIds.Count > 0)
      {
        if (!activeOnly)
          dbQueryBuilder1.Append(" where status = 1");
        dbQueryBuilder1.AppendLine("and ruleId in (" + string.Join<int>(", ", (IEnumerable<int>) fieldRuleIds) + ")");
      }
      List<DDMFieldRule> list4 = dbQueryBuilder1.Execute().Cast<DataRow>().Select<DataRow, DDMFieldRule>((System.Func<DataRow, DDMFieldRule>) (row => new DDMFieldRule(row))).ToList<DDMFieldRule>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder2.SelectFrom(table5);
      if (activeOnly)
        dbQueryBuilder2.Append(" where status = 1");
      if (fieldRuleIds != null && fieldRuleIds.Count > 0)
      {
        if (!activeOnly)
          dbQueryBuilder2.Append(" where status = 1");
        dbQueryBuilder2.AppendLine("and fieldRuleID in (" + string.Join<int>(", ", (IEnumerable<int>) fieldRuleIds) + ")");
      }
      dbQueryBuilder2.Append(" order by sortOrder asc");
      List<DDMFieldRuleScenario> list5 = dbQueryBuilder2.Execute().Cast<DataRow>().Select<DataRow, DDMFieldRuleScenario>((System.Func<DataRow, DDMFieldRuleScenario>) (row => new DDMFieldRuleScenario(row))).ToList<DDMFieldRuleScenario>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder3 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder3.SelectFrom(table6);
      List<DDMFeeRuleValue> list6 = dbQueryBuilder3.Execute().Cast<DataRow>().Select<DataRow, DDMFeeRuleValue>((System.Func<DataRow, DDMFeeRuleValue>) (row => new DDMFeeRuleValue(row, false))).ToList<DDMFeeRuleValue>();
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
      Dictionary<int, List<DDMFieldRuleScenario>> dictionary4 = new Dictionary<int, List<DDMFieldRuleScenario>>();
      foreach (DDMFieldRuleScenario fieldRuleScenario in list5)
      {
        if (dictionary4.ContainsKey(fieldRuleScenario.FieldRuleID))
          dictionary4[fieldRuleScenario.FieldRuleID].Add(fieldRuleScenario);
        else
          dictionary4.Add(fieldRuleScenario.FieldRuleID, new List<DDMFieldRuleScenario>()
          {
            fieldRuleScenario
          });
        if (dictionary3.ContainsKey(fieldRuleScenario.RuleID))
          fieldRuleScenario.FieldRuleValues = dictionary3[fieldRuleScenario.RuleID];
      }
      foreach (DDMFieldRule ddmFieldRule in list4)
      {
        if (dictionary4.ContainsKey(ddmFieldRule.RuleID))
          ddmFieldRule.Scenarios = dictionary4[ddmFieldRule.RuleID];
      }
      return list4.ToArray();
    }

    public static DDMFieldRule GetDDMFieldRule(int ruleID)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRules");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      DbValue key = new DbValue(nameof (ruleID), (object) ruleID);
      dbQueryBuilder.SelectFrom(table, key);
      return new DDMFieldRule(dbQueryBuilder.Execute()[0]);
    }

    public static DDMFieldRule[] GetFieldRulesByDataTable(string dataTableName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      string text = "SELECT A.* FROM DDM_FieldRuleScenario A inner join DDM_FieldRuleValue B on A.ruleID = B.fieldRuleScenarioID and B.fieldValue like '%DDM|" + dataTableName + "%' and B.fieldValueType=3";
      dbQueryBuilder.AppendLine(text);
      List<DDMFieldRuleScenario> list1 = dbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, DDMFieldRuleScenario>((System.Func<DataRow, DDMFieldRuleScenario>) (row => new DDMFieldRuleScenario(row))).ToList<DDMFieldRuleScenario>();
      List<DDMFieldRule> source = new List<DDMFieldRule>();
      foreach (DDMFieldRuleScenario fieldRuleScenario in list1)
      {
        DDMFieldRuleScenario fieldscenariorule = fieldRuleScenario;
        DDMFieldRule ddmFieldRule1 = (DDMFieldRule) null;
        if (source.Count > 0)
          ddmFieldRule1 = source.Where<DDMFieldRule>((System.Func<DDMFieldRule, bool>) (x => x.RuleID == fieldscenariorule.FieldRuleID)).FirstOrDefault<DDMFieldRule>();
        if (ddmFieldRule1 == null)
        {
          DDMFieldRule ddmFieldRule2 = DDMFieldRuleDbAccessor.GetDDMFieldRule(fieldscenariorule.FieldRuleID);
          source.Add(ddmFieldRule2);
        }
      }
      foreach (DDMFieldRule ddmFieldRule in source)
      {
        DDMFieldRule fieldrule = ddmFieldRule;
        List<DDMFieldRuleScenario> list2 = list1.Where<DDMFieldRuleScenario>((System.Func<DDMFieldRuleScenario, bool>) (x => x.FieldRuleID == fieldrule.RuleID)).ToList<DDMFieldRuleScenario>();
        fieldrule.Scenarios = list2;
      }
      return source.ToArray();
    }

    public static int CreateDDMFieldRule(DDMFieldRule DDMfieldrule)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRules");
      DateTime now = DateTime.Now;
      DbValueList values = new DbValueList();
      values.Add("ruleName", (object) DDMfieldrule.RuleName);
      values.Add("fields", (object) DDMfieldrule.Fields);
      values.Add("initLESent", (object) Convert.ToInt32(DDMfieldrule.InitLESent));
      values.Add("condition", (object) Convert.ToInt32(DDMfieldrule.Condition));
      values.Add("conditionState", (object) DDMfieldrule.ConditionState);
      values.Add("lastModifiedByFullName", (object) DDMfieldrule.LastModByFullName);
      values.Add("lastModByUserID", (object) DDMfieldrule.LastModByUserID);
      values.Add("createDT", (object) now);
      values.Add("LastModTime", (object) now);
      values.Add("status", (object) (int) DDMfieldrule.Status);
      values.Add("advCodeCondXML", (object) DDMfieldrule.AdvCodeConditionXML);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@ruleId", "int");
      DbValue dbValue = new DbValue("ruleId", (object) "@ruleId", (IDbEncoder) DbEncoding.None);
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.SelectIdentity("@ruleId");
      dbQueryBuilder.Select("@ruleId");
      object obj = dbQueryBuilder.ExecuteScalar();
      DDMfieldrule.RuleID = Convert.ToInt32(obj);
      return DDMfieldrule.RuleID;
    }

    public static int UpdateDDMFieldRule(DDMFieldRule DDMfieldrule)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRules");
      DbValueList values = new DbValueList();
      values.Add("ruleName", (object) DDMfieldrule.RuleName);
      values.Add("fields", (object) DDMfieldrule.Fields);
      values.Add("initLESent", (object) Convert.ToInt32(DDMfieldrule.InitLESent));
      values.Add("condition", (object) Convert.ToInt32(DDMfieldrule.Condition));
      values.Add("conditionState", (object) DDMfieldrule.ConditionState);
      values.Add("lastModifiedByFullName", (object) DDMfieldrule.LastModByFullName);
      values.Add("lastModByUserID", (object) DDMfieldrule.LastModByUserID);
      values.Add("createDT", (object) DDMfieldrule.CreateDt);
      values.Add("LastModTime", (object) DateTime.Now);
      values.Add("status", (object) (int) DDMfieldrule.Status);
      values.Add("advCodeCondXML", (object) DDMfieldrule.AdvCodeConditionXML);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue("ruleId", (object) DDMfieldrule.RuleID, (IDbEncoder) DbEncoding.None);
      dbQueryBuilder.Update(table, values, key);
      object obj = (object) dbQueryBuilder.Execute();
      return DDMfieldrule.RuleID;
    }

    public static void DeleteDDMFieldRuleByID(int ruleID)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRuleScenario");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue("fieldRuleID", (object) ruleID);
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
        DDMFieldRuleDbAccessor.DeleteDDMFieldRuleValueByIDs(array);
        DDMFieldRuleDbAccessor.DeleteDDMFieldRuleScenarioByID(ruleID);
      }
      DDMFieldRuleDbAccessor.DeleteDDMFieldRuleFromTableByID(ruleID);
    }

    private static void DeleteDDMFieldRuleValueByIDs(int[] ruleIDs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from DDM_FieldRuleValue where fieldRuleScenarioID in (" + string.Join<int>(",", (IEnumerable<int>) ruleIDs) + ")");
      dbQueryBuilder.Execute();
    }

    private static void DeleteDDMFieldRuleScenarioByID(int fieldRuleID)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRuleScenario");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue(nameof (fieldRuleID), (object) fieldRuleID);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.Execute();
    }

    private static void DeleteDDMFieldRuleFromTableByID(int ruleID)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("DDM_FieldRules");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue(nameof (ruleID), (object) ruleID);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.Execute();
    }

    public static bool IsTableUsedByFieldRules(DDMDataTable dt)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      string text = "select top 1 1\r\n                                from DDM_FieldRuleValue frv\r\n                                inner join DDM_FieldRuleScenario frs\r\n                                on frv.fieldRuleScenarioID = frs.ruleID\r\n                                where frv.fieldValueType = 3\r\n                                    and frv.fieldValue like '%DDM|" + dt.Name + "|%'";
      dbQueryBuilder.Append(text);
      return dbQueryBuilder.Execute().Count > 0;
    }

    public static void ResetDataTableFieldRuleValue(DDMDataTable dt)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = "update DDM_FieldRuleValue\r\n                                set fieldValue = '',\r\n                                    fieldValueType = 1\r\n                                where fieldValueType = 3\r\n                                    and fieldValue like '%DDM|" + dt.Name + "|%'";
      dbQueryBuilder.Append(text);
      dbQueryBuilder.Execute();
    }

    public static void UpdateDataTableReference(string oldName, string newName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = "WITH\r\n                  Decomposed AS (\r\n                    SELECT\r\n                      ruleValueId,\r\n                      fieldValue,\r\n                      SUBSTRING(SUBSTRING(fieldValue, 5, LEN(fieldValue)), --chop off ddm\r\n                                0,\r\n                                CHARINDEX('|', SUBSTRING(fieldValue, 5, LEN(fieldValue)))\r\n                                ) AS TableName,\r\n                      REPLACE(fieldValue, 'DDM|{{OldDataTableName}}|', '') AS Remnant\r\n                    FROM DDM_FieldRuleValue\r\n                    WHERE fieldValue like 'DDM|{{OldDataTableName}}|%' AND fieldValueType = 3\r\n                  )\r\n                UPDATE frv\r\n                  SET frv.fieldValue = 'DDM|' + '{{NewDataTableName}}' + '|' + d.Remnant\r\n                FROM DDM_FieldRuleValue AS frv\r\n                  INNER JOIN Decomposed AS d ON (frv.ruleValueID = d.ruleValueID)".Replace("{{NewDataTableName}}", newName).Replace("{{OldDataTableName}}", oldName);
      dbQueryBuilder.AppendLine(text);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static Dictionary<int, HashSet<string>> GetAllDdmFieldRulesAndFieldsList()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.Append("SELECT FRules.ruleid, FRules.rulename, Fsce.rulename, fsFields.fieldid, fsFields.relationshiptype \r\n                        FROM   DDM_FieldRules FRules \r\n                               INNER JOIN DDM_FieldRuleScenario Fsce ON Fsce.fieldruleid = FRules.ruleid \r\n                               INNER JOIN FS_Rules fsRules ON fsRules.ruleid = Fsce.ruleid \r\n                               INNER JOIN FS_Fields fsFields ON fsFields.fsruleid = fsRules.fsruleid \r\n                        WHERE  fsRules.ruletype = 25 \r\n                               AND FRules.status = 1 \r\n                               AND Fsce.status = 1");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return (Dictionary<int, HashSet<string>>) null;
      Dictionary<int, HashSet<string>> rulesAndFieldsList = new Dictionary<int, HashSet<string>>();
      Dictionary<string, HashSet<int>> dictionary = new Dictionary<string, HashSet<int>>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        int key = (int) dataRow["ruleID"];
        if (!rulesAndFieldsList.ContainsKey(key))
          rulesAndFieldsList.Add(key, new HashSet<string>());
        string str = dataRow["FieldId"] != DBNull.Value ? dataRow["FieldId"].ToString() : (string) null;
        if (str != null)
        {
          if ((dataRow["RelationshipType"] != DBNull.Value ? (int) Convert.ToInt16(dataRow["RelationshipType"]) : 0) == 11)
          {
            string[] strArray = str.Split('|');
            if (strArray.Length >= 3)
            {
              if (!dictionary.ContainsKey(strArray[2]))
                dictionary.Add(strArray[2], new HashSet<int>()
                {
                  key
                });
              else
                dictionary[strArray[2]].Add(key);
              rulesAndFieldsList[key].Add(strArray[0]);
            }
          }
          else
            rulesAndFieldsList[key].Add(str);
        }
      }
      if (dictionary.Count < 1)
        return rulesAndFieldsList;
      Dictionary<string, HashSet<string>> referredInDataTables = FieldSearchDbAccessor.GetFieldsReferredInDataTables(dictionary.Keys.ToList<string>());
      foreach (KeyValuePair<string, HashSet<int>> keyValuePair in dictionary)
      {
        if (referredInDataTables.ContainsKey(keyValuePair.Key))
        {
          HashSet<string> other = referredInDataTables[keyValuePair.Key];
          if (other.Count > 0)
          {
            foreach (int key in keyValuePair.Value)
              rulesAndFieldsList[key].UnionWith((IEnumerable<string>) other);
          }
        }
      }
      return rulesAndFieldsList;
    }
  }
}
