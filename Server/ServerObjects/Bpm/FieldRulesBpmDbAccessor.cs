// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.FieldRulesBpmDbAccessor
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
  public sealed class FieldRulesBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "FieldRulesBpmDbAccessor�";

    public FieldRulesBpmDbAccessor()
      : base(ClientSessionCacheID.BpmFieldRules)
    {
    }

    protected override string RuleTableName => "[BR_FieldRules]";

    protected override Type RuleType => typeof (FieldRuleInfo);

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue ruleIdValue)
    {
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[FR_RequiredFields]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[FR_FieldRule]"), ruleIdValue);
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue ruleIdValue)
    {
      FieldRuleInfo fieldRuleInfo = (FieldRuleInfo) rule;
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[FR_RequiredFields]");
      if (fieldRuleInfo.RequiredFields != null)
      {
        foreach (string key in (IEnumerable) fieldRuleInfo.RequiredFields.Keys)
        {
          string[] requiredField = (string[]) fieldRuleInfo.RequiredFields[(object) key];
          if (requiredField != null)
          {
            foreach (string str in requiredField)
              sql.InsertInto(table1, new DbValueList()
              {
                ruleIdValue,
                {
                  "fieldId",
                  (object) key
                },
                {
                  "requiredFieldID",
                  (object) str
                }
              }, true, false);
          }
        }
      }
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[FR_FieldRule]");
      if (fieldRuleInfo.FieldRules == null)
        return;
      foreach (string key in (IEnumerable) fieldRuleInfo.FieldRules.Keys)
      {
        object fieldRule = fieldRuleInfo.FieldRules[(object) key];
        if (fieldRule != null)
          sql.InsertInto(table2, new DbValueList()
          {
            ruleIdValue,
            {
              "fieldId",
              (object) key
            },
            {
              "ruleType",
              (object) FieldRulesBpmDbAccessor.getFieldRuleType(fieldRule)
            },
            {
              "ruleValue",
              (object) fieldRule.ToString()
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
        pgDbQueryBuilder.AppendLine("select * from [BR_FieldRules] rules where " + filter + ";");
        pgDbQueryBuilder.AppendLine("select fields.* from [FR_RequiredFields] fields");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_FieldRules] rules on fields.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter + ";");
        pgDbQueryBuilder.AppendLine("select fields.* from [FR_FieldRule] fields");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_FieldRules] rules on fields.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter + ";");
        return (BizRuleInfo[]) FieldRulesBpmDbAccessor.dataSetToRuleInfos(pgDbQueryBuilder.ExecuteSetQuery());
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.Append("select * from [BR_FieldRules] rules where " + filter);
      dbQueryBuilder.AppendLine("select fields.* from [FR_RequiredFields] fields");
      dbQueryBuilder.AppendLine("\tinner join [BR_FieldRules] rules on fields.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("select fields.* from [FR_FieldRule] fields");
      dbQueryBuilder.AppendLine("\tinner join [BR_FieldRules] rules on fields.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      return (BizRuleInfo[]) FieldRulesBpmDbAccessor.dataSetToRuleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    private static FieldRuleInfo[] dataSetToRuleInfos(DataSet data)
    {
      DataRelation relation1 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataRelation relation2 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[2].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      FieldRuleInfo[] ruleInfos = new FieldRuleInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        Hashtable reqFieldsHt = FieldRulesBpmDbAccessor.dataRowsToReqFieldsHT((ICollection) row.GetChildRows(relation1));
        Hashtable fieldRulesHt = FieldRulesBpmDbAccessor.dataRowsToFieldRulesHT((ICollection) row.GetChildRows(relation2));
        ruleInfos[index] = new FieldRuleInfo(row, reqFieldsHt, fieldRulesHt);
      }
      return ruleInfos;
    }

    private static Hashtable dataRowsToReqFieldsHT(ICollection rows)
    {
      if (rows.Count == 0)
        return new Hashtable();
      Hashtable reqFieldsHt = new Hashtable();
      List<string> stringList = new List<string>();
      string key1 = (string) null;
      foreach (DataRow row in (IEnumerable) rows)
      {
        string key2 = (string) row["fieldID"];
        string str = (string) row["requiredFieldID"];
        if (key1 != key2)
        {
          if (key1 != null && stringList.Count > 0)
          {
            if (reqFieldsHt.ContainsKey((object) key1))
              reqFieldsHt[(object) key1] = (object) stringList.ToArray();
            else
              reqFieldsHt.Add((object) key1, (object) stringList.ToArray());
          }
          stringList.Clear();
          if (reqFieldsHt.ContainsKey((object) key2))
            stringList.AddRange((IEnumerable<string>) (string[]) reqFieldsHt[(object) key2]);
          key1 = key2;
        }
        stringList.Add(str);
      }
      if (key1 != null && stringList.Count > 0)
      {
        if (reqFieldsHt.ContainsKey((object) key1))
          reqFieldsHt[(object) key1] = (object) stringList.ToArray();
        else
          reqFieldsHt.Add((object) key1, (object) stringList.ToArray());
      }
      return reqFieldsHt;
    }

    [PgReady]
    private static Hashtable dataRowsToFieldRulesHT(ICollection rows)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        if (rows.Count == 0)
          return new Hashtable();
        Hashtable fieldRulesHt = new Hashtable();
        foreach (DataRow row in (IEnumerable) rows)
        {
          string key = (string) row["fieldID"];
          BizRule.FieldRuleType fieldRuleType = (BizRule.FieldRuleType) SQL.DecodeInt(row["ruleType"]);
          string text = (string) row["ruleValue"];
          switch (fieldRuleType)
          {
            case BizRule.FieldRuleType.Range:
              fieldRulesHt.Add((object) key, (object) FRRange.Parse(text));
              continue;
            case BizRule.FieldRuleType.ListLock:
            case BizRule.FieldRuleType.ListUnlock:
              fieldRulesHt.Add((object) key, (object) FRList.Parse(text, fieldRuleType));
              continue;
            case BizRule.FieldRuleType.Code:
              fieldRulesHt.Add((object) key, (object) text);
              continue;
            default:
              throw new Exception("Invalid File Rule Type");
          }
        }
        return fieldRulesHt;
      }
      if (rows.Count == 0)
        return new Hashtable();
      Hashtable fieldRulesHt1 = new Hashtable();
      foreach (DataRow row in (IEnumerable) rows)
      {
        string key = (string) row["fieldID"];
        BizRule.FieldRuleType fieldRuleType = (BizRule.FieldRuleType) (byte) row["ruleType"];
        string text = (string) row["ruleValue"];
        switch (fieldRuleType)
        {
          case BizRule.FieldRuleType.Range:
            fieldRulesHt1.Add((object) key, (object) FRRange.Parse(text));
            continue;
          case BizRule.FieldRuleType.ListLock:
          case BizRule.FieldRuleType.ListUnlock:
            fieldRulesHt1.Add((object) key, (object) FRList.Parse(text, fieldRuleType));
            continue;
          case BizRule.FieldRuleType.Code:
            fieldRulesHt1.Add((object) key, (object) text);
            continue;
          default:
            throw new Exception("Invalid File Rule Type");
        }
      }
      return fieldRulesHt1;
    }

    private static int getFieldRuleType(object ruleObj)
    {
      switch (ruleObj)
      {
        case IFieldRuleDefinition _:
          return (int) ((IFieldRuleDefinition) ruleObj).RuleType;
        case string _:
          return 3;
        default:
          throw new Exception("The field rule type " + ruleObj.GetType().Name + " is not recognized");
      }
    }

    public static FieldRuleFieldIDAndType[] GetInconsistentFields(int ruleID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select B.status, F.fieldID, F.ruleType from [FR_FieldRules] as F");
      dbQueryBuilder.AppendLine("\tinner join [BR_FieldRules] as B on F.ruleID = B.ruleID");
      dbQueryBuilder.AppendLine("\t\twhere B.ruleID = " + (object) ruleID);
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1.Count == 0)
        return new FieldRuleFieldIDAndType[0];
      if ((byte) dataRowCollection1[0]["status"] == (byte) 1)
        return new FieldRuleFieldIDAndType[0];
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("select B.ruleID, B.ruleName, F.fieldID, F.ruleType from [FR_FieldRules] as F");
      dbQueryBuilder.AppendLine("\tinner join [BR_FieldRules] as B on F.ruleID = B.ruleID");
      dbQueryBuilder.AppendLine("\t\twhere B.status = " + (object) 1);
      dbQueryBuilder.AppendLine("\t\tand (");
      for (int index = 0; index < dataRowCollection1.Count; ++index)
      {
        if (index > 0)
          dbQueryBuilder.Append("\t\t\tor ");
        dbQueryBuilder.AppendLine("\t\t\t(F.fieldID = '" + (string) dataRowCollection1[index]["fieldID"] + "' and F.ruleType <> " + (object) (byte) dataRowCollection1[index]["ruleType"] + ")");
      }
      dbQueryBuilder.AppendLine(")");
      DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index < dataRowCollection2.Count; ++index)
      {
        int num = (int) dataRowCollection2[index][nameof (ruleID)];
        string ruleName = (string) dataRowCollection2[index]["ruleName"];
        string fieldID = (string) dataRowCollection2[index]["fieldID"];
        BizRule.FieldRuleType ruleType = (BizRule.FieldRuleType) (byte) dataRowCollection2[index]["ruleType"];
        FieldRuleFieldIDAndType ruleFieldIdAndType = (FieldRuleFieldIDAndType) hashtable[(object) num];
        if (ruleFieldIdAndType == null)
        {
          ruleFieldIdAndType = new FieldRuleFieldIDAndType(num, ruleName);
          hashtable.Add((object) num, (object) ruleFieldIdAndType);
        }
        ruleFieldIdAndType.AddField(fieldID, ruleType);
      }
      ArrayList arrayList = new ArrayList();
      foreach (FieldRuleFieldIDAndType ruleFieldIdAndType in (IEnumerable) hashtable.Values)
        arrayList.Add((object) ruleFieldIdAndType);
      return (FieldRuleFieldIDAndType[]) arrayList.ToArray(typeof (FieldRuleFieldIDAndType));
    }
  }
}
