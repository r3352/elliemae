// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.LoanAccessBpmDbAccessor
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
  public sealed class LoanAccessBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "LoanAccessBpmDbAccessor�";

    public LoanAccessBpmDbAccessor()
      : base(ClientSessionCacheID.BpmLoanAccess)
    {
    }

    protected override string RuleTableName => "[BR_LoanAccessRules]";

    protected override Type RuleType => typeof (LoanAccessRuleInfo);

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue ruleIdValue)
    {
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[LAR_Fields]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[LAR_Loans]"), ruleIdValue);
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue ruleIdValue)
    {
      LoanAccessRuleInfo loanAccessRuleInfo = (LoanAccessRuleInfo) rule;
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[LAR_Fields]");
      foreach (PersonaLoanAccessRight loanAccessRight in loanAccessRuleInfo.LoanAccessRights)
      {
        if (loanAccessRight.editableFields != null)
        {
          for (int index = 0; index < loanAccessRight.editableFields.Length; ++index)
            sql.InsertInto(table1, new DbValueList()
            {
              ruleIdValue,
              {
                "personaId",
                (object) loanAccessRight.PersonaID
              },
              {
                "editableField",
                (object) loanAccessRight.editableFields[index]
              }
            }, true, false);
        }
      }
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[LAR_Loans]");
      foreach (PersonaLoanAccessRight loanAccessRight in loanAccessRuleInfo.LoanAccessRights)
        sql.InsertInto(table2, new DbValueList()
        {
          ruleIdValue,
          {
            "personaId",
            (object) loanAccessRight.PersonaID
          },
          {
            "accessRight",
            (object) loanAccessRight.AccessRight
          }
        }, true, false);
    }

    [PgReady]
    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from [BR_LoanAccessRules] rules where " + filter + ";");
        pgDbQueryBuilder.AppendLine("select fields.* from [LAR_Fields] fields");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_LoanAccessRules] rules on fields.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter + ";");
        Hashtable editableFields = LoanAccessBpmDbAccessor.dataSetToEditableFields(pgDbQueryBuilder.ExecuteSetQuery());
        pgDbQueryBuilder.Reset();
        pgDbQueryBuilder.AppendLine("select * from [BR_LoanAccessRules] rules where " + filter + ";");
        pgDbQueryBuilder.AppendLine("select rights.* from [LAR_Loans] rights");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_LoanAccessRules] rules on rights.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter + ";");
        return (BizRuleInfo[]) LoanAccessBpmDbAccessor.dataSetToRuleInfos(pgDbQueryBuilder.ExecuteSetQuery(), editableFields);
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Settings);
      dbQueryBuilder1.Append("select * from [BR_LoanAccessRules] rules where " + filter);
      dbQueryBuilder1.AppendLine("select fields.* from [LAR_Fields] fields");
      dbQueryBuilder1.AppendLine("\tinner join [BR_LoanAccessRules] rules on fields.ruleID = rules.ruleID");
      dbQueryBuilder1.AppendLine("\twhere " + filter);
      Hashtable editableFields1 = LoanAccessBpmDbAccessor.dataSetToEditableFields(dbQueryBuilder1.ExecuteSetQuery());
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Settings);
      dbQueryBuilder2.Append("select * from [BR_LoanAccessRules] rules where " + filter);
      dbQueryBuilder2.AppendLine("select rights.* from [LAR_Loans] rights");
      dbQueryBuilder2.AppendLine("\tinner join [BR_LoanAccessRules] rules on rights.ruleID = rules.ruleID");
      dbQueryBuilder2.AppendLine("\twhere " + filter);
      return (BizRuleInfo[]) LoanAccessBpmDbAccessor.dataSetToRuleInfos(dbQueryBuilder2.ExecuteSetQuery(), editableFields1);
    }

    private static Hashtable dataSetToEditableFields(DataSet data)
    {
      DataRelation relation = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      Hashtable editableFields = new Hashtable();
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        Hashtable fieldAccessRights = LoanAccessBpmDbAccessor.dataRowsToPersonaFieldAccessRights((ICollection) row.GetChildRows(relation));
        editableFields.Add((object) (int) row["ruleID"], (object) fieldAccessRights);
      }
      return editableFields;
    }

    private static LoanAccessRuleInfo[] dataSetToRuleInfos(DataSet data, Hashtable editableFields)
    {
      DataRelation relation = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      LoanAccessRuleInfo[] ruleInfos = new LoanAccessRuleInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        PersonaLoanAccessRight[] loanAccessRights = LoanAccessBpmDbAccessor.dataRowsToPersonaLoanAccessRights((ICollection) row.GetChildRows(relation), editableFields);
        ruleInfos[index] = new LoanAccessRuleInfo(row, loanAccessRights);
      }
      return ruleInfos;
    }

    private static Hashtable dataRowsToPersonaFieldAccessRights(ICollection rows)
    {
      Hashtable fieldAccessRights = new Hashtable();
      foreach (DataRow row in (IEnumerable) rows)
      {
        int key = (int) row["personaID"];
        if (fieldAccessRights.ContainsKey((object) key))
        {
          ArrayList arrayList = (ArrayList) fieldAccessRights[(object) key];
          arrayList.Add((object) (string) row["editableField"]);
          fieldAccessRights[(object) key] = (object) arrayList;
        }
        else
          fieldAccessRights.Add((object) key, (object) new ArrayList()
          {
            (object) (string) row["editableField"]
          });
      }
      return fieldAccessRights;
    }

    private static PersonaLoanAccessRight[] dataRowsToPersonaLoanAccessRights(
      ICollection rows,
      Hashtable editableFields)
    {
      List<PersonaLoanAccessRight> personaLoanAccessRightList = new List<PersonaLoanAccessRight>();
      foreach (DataRow row in (IEnumerable) rows)
      {
        int key1 = (int) row["ruleID"];
        int key2 = (int) row["personaID"];
        string[] editableFields1 = (string[]) null;
        if (editableFields.ContainsKey((object) key1))
        {
          Hashtable editableField = (Hashtable) editableFields[(object) key1];
          if (editableField.ContainsKey((object) key2))
            editableFields1 = (string[]) ((ArrayList) editableField[(object) key2]).ToArray(typeof (string));
        }
        int accessRight = (int) row["accessRight"];
        if (accessRight != 16777215 && (accessRight & 131072) != 0 && (accessRight & 1073741824) == 0)
          accessRight = accessRight | 1073741824 | 262144;
        personaLoanAccessRightList.Add(new PersonaLoanAccessRight((int) row["personaID"], accessRight, editableFields1));
      }
      return personaLoanAccessRightList.ToArray();
    }
  }
}
