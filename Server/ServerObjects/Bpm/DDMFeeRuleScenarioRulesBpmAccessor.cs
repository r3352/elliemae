// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.DDMFeeRuleScenarioRulesBpmAccessor
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
  public class DDMFeeRuleScenarioRulesBpmAccessor : BpmDbAccessor
  {
    private const string className = "DDMFeeRuleScenarioRulesBpmAccessor�";

    public DDMFeeRuleScenarioRulesBpmAccessor()
      : base(ClientSessionCacheID.BpmDDMFeeRulesScenarios)
    {
    }

    protected override Type RuleType => typeof (DDMFeeRuleScenario);

    protected override string RuleTableName => "DDM_FeeRuleScenario";

    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.AppendLine(string.Format("select rules.*, fr.rulename as ParentRuleName from DDM_FeeRuleScenario rules  left join [DDM_FeeRules] fr on rules.feeruleid = fr.ruleid  where {0} order by rules.sortOrder", (object) filter));
      return filter.Equals("(1 = 1)") ? this.MapDataSetToAllDDMSFeeScenarios(dbQueryBuilder.ExecuteSetQuery()) : this.MapDataSetToDDMSFeeScenarios(dbQueryBuilder.ExecuteSetQuery());
    }

    protected override DbValueList getDbValueList(BizRuleInfo rule)
    {
      DbValueList dbValueList = base.getDbValueList(rule);
      DDMFeeRuleScenario ddmFeeRuleScenario = rule as DDMFeeRuleScenario;
      dbValueList.Add("feeRuleID", (object) ddmFeeRuleScenario.FeeRuleID);
      dbValueList.Add("sortOrder", (object) ddmFeeRuleScenario.Order);
      dbValueList.Add("feeNotAllowed", (object) (ddmFeeRuleScenario.FeeNotAllowed ? 1 : 0));
      dbValueList.Add("effectiveDateInfo", (object) ddmFeeRuleScenario.EffectiveDateInfo);
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
      List<DDMFeeRuleScenario> ddmFeeRuleScenarioList = new List<DDMFeeRuleScenario>();
      string[] strArray = additionalFilter.Split('=');
      strArray[0].Trim();
      string str = strArray[1].Trim();
      foreach (DDMFeeRuleScenario rule in rules)
      {
        if (rule.FeeRuleID == Convert.ToInt32(str))
          ddmFeeRuleScenarioList.Add(rule);
      }
      return (BizRuleInfo[]) ddmFeeRuleScenarioList.ToArray();
    }

    private BizRuleInfo[] MapDataSetToDDMSFeeScenarios(DataSet dataSet)
    {
      DataTable table = dataSet.Tables[0];
      List<DDMFeeRuleScenario> ddmFeeRuleScenarioList = new List<DDMFeeRuleScenario>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        DDMFeeRuleScenario ddmFeeRuleScenario = new DDMFeeRuleScenario(row, (TriggerEvent[]) null);
        List<DDMFeeRuleValue> valueForScenario = this.GetFeeRuleValueForScenario(ddmFeeRuleScenario.RuleID);
        if (valueForScenario != null)
          ddmFeeRuleScenario.FeeRuleValues = valueForScenario;
        ddmFeeRuleScenarioList.Add(ddmFeeRuleScenario);
      }
      return (BizRuleInfo[]) ddmFeeRuleScenarioList.ToArray();
    }

    private BizRuleInfo[] MapDataSetToAllDDMSFeeScenarios(DataSet dataSet)
    {
      DataTable table = dataSet.Tables[0];
      List<DDMFeeRuleScenario> feeRuleScenarios = new List<DDMFeeRuleScenario>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        DDMFeeRuleScenario ddmFeeRuleScenario = new DDMFeeRuleScenario(row, (TriggerEvent[]) null);
        feeRuleScenarios.Add(ddmFeeRuleScenario);
      }
      return (BizRuleInfo[]) this.GetFeeRuleValuesForScenarios(feeRuleScenarios).ToArray();
    }

    private List<DDMFeeRuleScenario> GetFeeRuleValuesForScenarios(
      List<DDMFeeRuleScenario> feeRuleScenarios)
    {
      if (!feeRuleScenarios.Any<DDMFeeRuleScenario>())
        return new List<DDMFeeRuleScenario>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.AppendLine("select * from DDM_FeeRuleValue");
      List<DDMFeeRuleValue> ddmFeeRuleValue = this.MapDataSetToDDMFeeRuleValue(dbQueryBuilder.ExecuteSetQuery());
      List<DDMFeeRuleScenario> valuesForScenarios = new List<DDMFeeRuleScenario>();
      foreach (DDMFeeRuleScenario feeRuleScenario in feeRuleScenarios)
      {
        DDMFeeRuleScenario scenario = feeRuleScenario;
        List<DDMFeeRuleValue> list = ddmFeeRuleValue.Where<DDMFeeRuleValue>((System.Func<DDMFeeRuleValue, bool>) (x => x.RuleScenarioID == scenario.RuleID)).ToList<DDMFeeRuleValue>();
        if (list != null)
          scenario.FeeRuleValues = list;
        valuesForScenarios.Add(scenario);
      }
      return valuesForScenarios;
    }

    private List<DDMFeeRuleValue> GetFeeRuleValueForScenario(int feeRuleScenarioID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.DDM);
      dbQueryBuilder.AppendLine(string.Format("select * from DDM_FeeRuleValue rules where feeRuleScenarioID = {0}", (object) feeRuleScenarioID));
      return this.MapDataSetToDDMFeeRuleValue(dbQueryBuilder.ExecuteSetQuery());
    }

    private List<DDMFeeRuleValue> MapDataSetToDDMFeeRuleValue(DataSet dataSet)
    {
      DataTable table = dataSet.Tables[0];
      List<DDMFeeRuleValue> ddmFeeRuleValue1 = new List<DDMFeeRuleValue>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        DDMFeeRuleValue ddmFeeRuleValue2 = new DDMFeeRuleValue(row);
        ddmFeeRuleValue1.Add(ddmFeeRuleValue2);
      }
      return ddmFeeRuleValue1;
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue keyValue)
    {
      DbValue key = new DbValue("feeRuleScenarioID", keyValue.Value, (IDbEncoder) DbEncoding.None);
      DDMFeeRuleScenario ddmFeeRuleScenario = (DDMFeeRuleScenario) rule;
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("[DDM_FeeRuleValue]");
      sql.DeleteFrom(table, key);
      if (ddmFeeRuleScenario.FeeRuleValues == null)
        return;
      for (int index = 0; index < ddmFeeRuleScenario.FeeRuleValues.Count; ++index)
        sql.InsertInto(table, new DbValueList()
        {
          key,
          {
            "fieldType",
            (object) ddmFeeRuleScenario.FeeRuleValues[index].Field_Type
          },
          {
            "fieldID",
            (object) ddmFeeRuleScenario.FeeRuleValues[index].FieldID
          },
          {
            "fieldName",
            (object) ddmFeeRuleScenario.FeeRuleValues[index].Field_Name
          },
          {
            "fieldValue",
            (object) ddmFeeRuleScenario.FeeRuleValues[index].Field_Value
          },
          {
            "fieldValueType",
            (object) ddmFeeRuleScenario.FeeRuleValues[index].Field_Value_Type
          }
        }, true, false);
    }

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue keyValue)
    {
      DbValue key = new DbValue("feeRuleScenarioID", keyValue.Value, (IDbEncoder) DbEncoding.None);
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("[DDM_FeeRuleValue]");
      sql.DeleteFrom(table, key);
    }
  }
}
