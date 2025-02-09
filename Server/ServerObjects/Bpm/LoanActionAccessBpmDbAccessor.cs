// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.LoanActionAccessBpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public sealed class LoanActionAccessBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "LoanActionAccessBpmDbAccessor�";

    public LoanActionAccessBpmDbAccessor()
      : base(ClientSessionCacheID.BpmLoanActionAccess)
    {
    }

    protected override string RuleTableName => "[BR_PersonaAccessToActions]";

    protected override Type RuleType => typeof (LoanActionAccessRuleInfo);

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue keyValue)
    {
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[LAAR_LoanActions]"), keyValue);
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue ruleIdValue)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("[LAAR_LoanActions]");
      foreach (LoanActionAccessRights actionAccessRight in ((LoanActionAccessRuleInfo) rule).LoanActionAccessRights)
      {
        foreach (int key in (IEnumerable) actionAccessRight.AccessRights.Keys)
          sql.InsertInto(table, new DbValueList()
          {
            ruleIdValue,
            {
              "loanActionID",
              (object) actionAccessRight.LoanActionID
            },
            {
              "personaId",
              (object) key
            },
            {
              "accessRight",
              (object) (int) actionAccessRight.AccessRights[(object) key]
            }
          }, true, false);
      }
    }

    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.Append("select * from [BR_PersonaAccessToActions] rules where " + filter);
      dbQueryBuilder.AppendLine("select loanActions.* from [LAAR_LoanActions] loanActions");
      dbQueryBuilder.AppendLine("\tinner join [BR_PersonaAccessToActions] rules on loanActions.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("\torder by loanActions.loanActionID");
      return (BizRuleInfo[]) LoanActionAccessBpmDbAccessor.dataSetToRuleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    public LoanActionAccessRights[] GetActiveRights(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      LoanActionAccessRuleInfo activeRule = (LoanActionAccessRuleInfo) this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      return activeRule == null ? new LoanActionAccessRights[0] : activeRule.LoanActionAccessRights;
    }

    private static LoanActionAccessRuleInfo[] dataSetToRuleInfos(DataSet data)
    {
      DataRelation relation = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      LoanActionAccessRuleInfo[] ruleInfos = new LoanActionAccessRuleInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        LoanActionAccessRights[] accessRights = LoanActionAccessBpmDbAccessor.dataRowsToAccessRights((ICollection) row.GetChildRows(relation));
        ruleInfos[index] = new LoanActionAccessRuleInfo(row, accessRights);
      }
      return ruleInfos;
    }

    private static LoanActionAccessRights[] dataRowsToAccessRights(ICollection rows)
    {
      if (rows.Count == 0)
        return new LoanActionAccessRights[0];
      Dictionary<string, List<PersonaLoanActionAccessRight>> dictionary = new Dictionary<string, List<PersonaLoanActionAccessRight>>();
      foreach (DataRow row in (IEnumerable) rows)
      {
        string key = (string) row["loanActionID"];
        int personaID = (int) row["personaID"];
        BizRule.LoanActionAccessRight accessRight = (BizRule.LoanActionAccessRight) (byte) row["accessRight"];
        if (!dictionary.ContainsKey(key))
          dictionary[key] = new List<PersonaLoanActionAccessRight>();
        dictionary[key].Add(new PersonaLoanActionAccessRight(personaID, accessRight));
      }
      List<LoanActionAccessRights> actionAccessRightsList = new List<LoanActionAccessRights>();
      foreach (string key in dictionary.Keys)
        actionAccessRightsList.Add(new LoanActionAccessRights(key, dictionary[key].ToArray()));
      return actionAccessRightsList.ToArray();
    }
  }
}
