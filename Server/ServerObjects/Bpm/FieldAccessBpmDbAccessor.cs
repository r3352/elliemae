// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.FieldAccessBpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public sealed class FieldAccessBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "FieldAccessBpmDbAccessor�";

    public FieldAccessBpmDbAccessor()
      : base(ClientSessionCacheID.BpmFieldAccess)
    {
    }

    protected override string RuleTableName => "[BR_FieldAccessRules]";

    protected override Type RuleType => typeof (FieldAccessRuleInfo);

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue keyValue)
    {
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[FAR_Fields]"), keyValue);
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue ruleIdValue)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("[FAR_Fields]");
      foreach (FieldAccessRights fieldAccessRight in ((FieldAccessRuleInfo) rule).FieldAccessRights)
      {
        foreach (int key in (IEnumerable) fieldAccessRight.AccessRights.Keys)
          sql.InsertInto(table, new DbValueList()
          {
            ruleIdValue,
            {
              "fieldId",
              (object) fieldAccessRight.FieldID
            },
            {
              "personaId",
              (object) key
            },
            {
              "accessRight",
              (object) (int) fieldAccessRight.AccessRights[(object) key]
            }
          }, true, false);
      }
    }

    [PgReady]
    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from [BR_FieldAccessRules] rules where " + filter + ";");
        pgDbQueryBuilder.AppendLine("select fields.* from [FAR_Fields] fields");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_FieldAccessRules] rules on fields.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter);
        pgDbQueryBuilder.AppendLine("\torder by fields.fieldID;");
        return (BizRuleInfo[]) FieldAccessBpmDbAccessor.dataSetToRuleInfos(pgDbQueryBuilder.ExecuteSetQuery());
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.Append("select * from [BR_FieldAccessRules] rules where " + filter);
      dbQueryBuilder.AppendLine("select fields.* from [FAR_Fields] fields");
      dbQueryBuilder.AppendLine("\tinner join [BR_FieldAccessRules] rules on fields.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("\torder by fields.fieldID");
      return (BizRuleInfo[]) FieldAccessBpmDbAccessor.dataSetToRuleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    public static FieldAccessRuleInfo[] GetActiveFieldAccessRulesByPersona(int[] personaIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str1 = "(rules.status = 1)";
      dbQueryBuilder.Append("select * from [BR_FieldAccessRules] rules where " + str1);
      string str2 = str1 + " and (fields.personaID IN (" + SQL.EncodeArray((Array) personaIds) + "))" + " and (fields.accessRight NOT IN (" + SQL.EncodeArray((Array) new int[1]
      {
        4
      }) + "))";
      dbQueryBuilder.AppendLine("select fields.* from [FAR_Fields] fields");
      dbQueryBuilder.AppendLine("\tinner join [BR_FieldAccessRules] rules on fields.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + str2);
      dbQueryBuilder.AppendLine("\torder by fields.fieldID");
      return FieldAccessBpmDbAccessor.dataSetToRuleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    public FieldAccessRights[] GetActiveRights(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      FieldAccessRuleInfo activeRule = (FieldAccessRuleInfo) this.GetActiveRule(condition, condition2, conditionState, milestoneID, conditionState2);
      return activeRule == null ? new FieldAccessRights[0] : activeRule.FieldAccessRights;
    }

    private static FieldAccessRuleInfo[] dataSetToRuleInfos(DataSet data)
    {
      DataRelation relation = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      FieldAccessRuleInfo[] ruleInfos = new FieldAccessRuleInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        FieldAccessRights[] accessRights = FieldAccessBpmDbAccessor.dataRowsToAccessRights((ICollection) row.GetChildRows(relation));
        ruleInfos[index] = new FieldAccessRuleInfo(row, accessRights);
      }
      return ruleInfos;
    }

    [PgReady]
    private static FieldAccessRights[] dataRowsToAccessRights(ICollection rows)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        if (rows.Count == 0)
          return new FieldAccessRights[0];
        Dictionary<string, List<PersonaFieldAccessRight>> dictionary = new Dictionary<string, List<PersonaFieldAccessRight>>();
        foreach (DataRow row in (IEnumerable) rows)
        {
          string key = (string) row["fieldID"];
          int personaID = (int) row["personaID"];
          BizRule.FieldAccessRight accessRight = (BizRule.FieldAccessRight) SQL.DecodeInt(row["accessRight"]);
          if (!dictionary.ContainsKey(key))
            dictionary[key] = new List<PersonaFieldAccessRight>();
          dictionary[key].Add(new PersonaFieldAccessRight(personaID, accessRight));
        }
        List<FieldAccessRights> fieldAccessRightsList = new List<FieldAccessRights>();
        foreach (string key in dictionary.Keys)
          fieldAccessRightsList.Add(new FieldAccessRights(key, dictionary[key].ToArray()));
        return fieldAccessRightsList.ToArray();
      }
      if (rows.Count == 0)
        return new FieldAccessRights[0];
      Dictionary<string, List<PersonaFieldAccessRight>> dictionary1 = new Dictionary<string, List<PersonaFieldAccessRight>>();
      foreach (DataRow row in (IEnumerable) rows)
      {
        string key = (string) row["fieldID"];
        int personaID = (int) row["personaID"];
        BizRule.FieldAccessRight accessRight = (BizRule.FieldAccessRight) (byte) row["accessRight"];
        if (!dictionary1.ContainsKey(key))
          dictionary1[key] = new List<PersonaFieldAccessRight>();
        dictionary1[key].Add(new PersonaFieldAccessRight(personaID, accessRight));
      }
      List<FieldAccessRights> fieldAccessRightsList1 = new List<FieldAccessRights>();
      foreach (string key in dictionary1.Keys)
        fieldAccessRightsList1.Add(new FieldAccessRights(key, dictionary1[key].ToArray()));
      return fieldAccessRightsList1.ToArray();
    }
  }
}
