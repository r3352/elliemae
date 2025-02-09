// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.DDMFieldRuleScenarioRuleBpmAccessor
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
  public class DDMFieldRuleScenarioRuleBpmAccessor : BpmDbAccessor
  {
    private const string className = "DDMFieldRuleScenarioRuleBpmAccessor�";

    public DDMFieldRuleScenarioRuleBpmAccessor()
      : base(ClientSessionCacheID.BpmDDMFieldRulesScenarios)
    {
    }

    protected override Type RuleType => typeof (DDMFieldRuleScenario);

    protected override string RuleTableName => "DDM_FieldRuleScenario";

    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.AppendLine(string.Format("select rules.*, fr.rulename as ParentRuleName from DDM_FieldRuleScenario rules  left join [DDM_FieldRules] fr on rules.fieldruleid = fr.ruleid  where {0} order by rules.sortOrder", (object) filter));
      return filter.Equals("(1 = 1)") ? this.MapDataSetToAllDDMSFieldScenarios(dbQueryBuilder.ExecuteSetQuery()) : this.MapDataSetToDDMSFieldScenarios(dbQueryBuilder.ExecuteSetQuery());
    }

    protected override DbValueList getDbValueList(BizRuleInfo rule)
    {
      DbValueList dbValueList = base.getDbValueList(rule);
      DDMFieldRuleScenario fieldRuleScenario = rule as DDMFieldRuleScenario;
      dbValueList.Add("fieldRuleID", (object) fieldRuleScenario.FieldRuleID);
      dbValueList.Add("sortOrder", (object) fieldRuleScenario.Order);
      dbValueList.Add("effectiveDateInfo", (object) fieldRuleScenario.EffectiveDateInfo);
      dbValueList.Add("feeNotAllowed", (object) (fieldRuleScenario.FeeNotAllowed ? 1 : 0));
      return dbValueList;
    }

    protected override BizRuleInfo[] GetFilteredRulesFromCache(
      BizRuleInfo[] rules,
      string additionalFilter)
    {
      if (rules == null)
        return (BizRuleInfo[]) null;
      if (string.IsNullOrEmpty(additionalFilter))
        return base.GetFilteredRulesFromCache(rules, additionalFilter);
      List<DDMFieldRuleScenario> fieldRuleScenarioList = new List<DDMFieldRuleScenario>();
      string[] strArray = additionalFilter.Split('=');
      strArray[0].Trim();
      string str = strArray[1].Trim();
      foreach (DDMFieldRuleScenario rule in rules)
      {
        if (rule.FieldRuleID == Convert.ToInt32(str))
          fieldRuleScenarioList.Add(rule);
      }
      return (BizRuleInfo[]) fieldRuleScenarioList.ToArray();
    }

    private BizRuleInfo[] MapDataSetToDDMSFieldScenarios(DataSet dataSet)
    {
      DataTable table = dataSet.Tables[0];
      List<DDMFieldRuleScenario> fieldRuleScenarioList = new List<DDMFieldRuleScenario>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        DDMFieldRuleScenario fieldRuleScenario = new DDMFieldRuleScenario(row);
        List<DDMFeeRuleValue> valueForScenario = this.GetFieldRuleValueForScenario(fieldRuleScenario.RuleID);
        if (valueForScenario != null)
          fieldRuleScenario.FieldRuleValues = valueForScenario;
        fieldRuleScenarioList.Add(fieldRuleScenario);
      }
      return (BizRuleInfo[]) fieldRuleScenarioList.ToArray();
    }

    private BizRuleInfo[] MapDataSetToAllDDMSFieldScenarios(DataSet dataSet)
    {
      DataTable table = dataSet.Tables[0];
      List<DDMFieldRuleScenario> fieldRuleScenarios = new List<DDMFieldRuleScenario>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        DDMFieldRuleScenario fieldRuleScenario = new DDMFieldRuleScenario(row);
        fieldRuleScenarios.Add(fieldRuleScenario);
      }
      return (BizRuleInfo[]) this.GetFieldRuleValueForScenario(fieldRuleScenarios).ToArray();
    }

    private List<DDMFieldRuleScenario> GetFieldRuleValueForScenario(
      List<DDMFieldRuleScenario> fieldRuleScenarios)
    {
      if (!fieldRuleScenarios.Any<DDMFieldRuleScenario>())
        return new List<DDMFieldRuleScenario>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.AppendLine("select * from DDM_FieldRuleValue");
      List<DDMFeeRuleValue> ddmFieldRuleValue = this.MapDataSetToDDMFieldRuleValue(dbQueryBuilder.ExecuteSetQuery());
      List<DDMFieldRuleScenario> valueForScenario = new List<DDMFieldRuleScenario>();
      foreach (DDMFieldRuleScenario fieldRuleScenario in fieldRuleScenarios)
      {
        DDMFieldRuleScenario scenario = fieldRuleScenario;
        List<DDMFeeRuleValue> list = ddmFieldRuleValue.Where<DDMFeeRuleValue>((System.Func<DDMFeeRuleValue, bool>) (x => x.RuleScenarioID == scenario.RuleID)).ToList<DDMFeeRuleValue>();
        if (list != null)
          scenario.FieldRuleValues = list;
        valueForScenario.Add(scenario);
      }
      return valueForScenario;
    }

    private List<DDMFeeRuleValue> GetFieldRuleValueForScenario(int fieldRuleScenarioID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.AppendLine(string.Format("select * from DDM_FieldRuleValue rules where fieldRuleScenarioID = {0}", (object) fieldRuleScenarioID));
      return this.MapDataSetToDDMFieldRuleValue(dbQueryBuilder.ExecuteSetQuery());
    }

    private List<DDMFeeRuleValue> MapDataSetToDDMFieldRuleValue(DataSet dataSet)
    {
      DataTable table = dataSet.Tables[0];
      List<DDMFeeRuleValue> ddmFieldRuleValue = new List<DDMFeeRuleValue>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        DDMFeeRuleValue ddmFeeRuleValue = new DDMFeeRuleValue(row, false);
        ddmFieldRuleValue.Add(ddmFeeRuleValue);
      }
      return ddmFieldRuleValue;
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue keyValue)
    {
      DbValue key = new DbValue("fieldRuleScenarioID", keyValue.Value, (IDbEncoder) DbEncoding.None);
      DDMFieldRuleScenario fieldRuleScenario = (DDMFieldRuleScenario) rule;
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("[DDM_FieldRuleValue]");
      sql.DeleteFrom(table, key);
      if (fieldRuleScenario.FieldRuleValues == null)
        return;
      for (int index = 0; index < fieldRuleScenario.FieldRuleValues.Count; ++index)
        sql.InsertInto(table, new DbValueList()
        {
          key,
          {
            "fieldType",
            (object) fieldRuleScenario.FieldRuleValues[index].Field_Type
          },
          {
            "fieldID",
            (object) fieldRuleScenario.FieldRuleValues[index].FieldID
          },
          {
            "fieldName",
            (object) fieldRuleScenario.FieldRuleValues[index].Field_Name
          },
          {
            "fieldValue",
            (object) fieldRuleScenario.FieldRuleValues[index].Field_Value
          },
          {
            "fieldValueType",
            (object) fieldRuleScenario.FieldRuleValues[index].Field_Value_Type
          }
        }, true, false);
    }

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue keyValue)
    {
      DbValue key = new DbValue("fieldRuleScenarioID", keyValue.Value, (IDbEncoder) DbEncoding.None);
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("[DDM_FieldRuleValue]");
      sql.DeleteFrom(table, key);
    }
  }
}
