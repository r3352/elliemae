// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.InputFormsBpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public sealed class InputFormsBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "InputFormsBpmDbAccessor�";

    public InputFormsBpmDbAccessor()
      : base(ClientSessionCacheID.BpmInputForms)
    {
    }

    protected override string RuleTableName => "[BR_InputFormRules]";

    protected override Type RuleType => typeof (InputFormRuleInfo);

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue ruleIdValue)
    {
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[IFR_InputForms]"), ruleIdValue);
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue ruleIdValue)
    {
      InputFormRuleInfo inputFormRuleInfo = (InputFormRuleInfo) rule;
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("[IFR_InputForms]");
      foreach (string form in inputFormRuleInfo.Forms)
        sql.InsertInto(table, new DbValueList()
        {
          ruleIdValue,
          {
            "formID",
            (object) form
          }
        }, true, false);
    }

    [PgReady]
    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from [BR_InputFormRules] rules where " + filter + ";");
        pgDbQueryBuilder.AppendLine("select forms.* from [IFR_InputForms] forms");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_InputFormRules] rules on forms.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter);
        pgDbQueryBuilder.AppendLine("\torder by forms.formID;");
        return (BizRuleInfo[]) InputFormsBpmDbAccessor.dataSetToRuleInfos(pgDbQueryBuilder.ExecuteSetQuery());
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.Append("select * from [BR_InputFormRules] rules where " + filter);
      dbQueryBuilder.AppendLine("select forms.* from [IFR_InputForms] forms");
      dbQueryBuilder.AppendLine("\tinner join [BR_InputFormRules] rules on forms.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("\torder by forms.formID");
      return (BizRuleInfo[]) InputFormsBpmDbAccessor.dataSetToRuleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    private static InputFormRuleInfo[] dataSetToRuleInfos(DataSet data)
    {
      DataRelation relation = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      InputFormRuleInfo[] ruleInfos = new InputFormRuleInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        DataRow[] childRows = row.GetChildRows(relation);
        ruleInfos[index] = new InputFormRuleInfo(row, childRows);
      }
      return ruleInfos;
    }
  }
}
