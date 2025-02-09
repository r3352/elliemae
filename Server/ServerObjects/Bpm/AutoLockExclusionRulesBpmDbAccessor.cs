// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.AutoLockExclusionRulesBpmDbAccessor
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
  public sealed class AutoLockExclusionRulesBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "AutoLockExclusionRulesBpmDbAccessor�";

    public AutoLockExclusionRulesBpmDbAccessor()
      : base(ClientSessionCacheID.BpmFieldRules)
    {
    }

    protected override string RuleTableName => "[BR_AutoLockExclusionRule]";

    protected override Type RuleType => typeof (AutoLockExclusionRuleInfo);

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue ruleIdValue)
    {
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue ruleIdValue)
    {
    }

    private static DbValueList getDbValueList(BizRuleInfo rule)
    {
      return new DbValueList()
      {
        {
          "ruleName",
          (object) rule.RuleName
        },
        {
          "condition",
          (object) (int) rule.Condition
        },
        {
          "condition2",
          (object) rule.Condition2
        },
        {
          "conditionState",
          rule.Condition == BizRule.Condition.AdvancedCoding ? (object) "" : (object) rule.ConditionState
        },
        {
          "conditionState2",
          (object) rule.ConditionState2
        },
        {
          "advancedCode",
          rule.Condition == BizRule.Condition.AdvancedCoding ? (object) rule.ConditionState : (object) (string) null
        },
        {
          "advancedCodeXml",
          rule.Condition == BizRule.Condition.AdvancedCoding ? (object) rule.AdvancedCodeXML : (object) (string) null
        },
        {
          "status",
          (object) (int) rule.Status
        },
        {
          "lastModTime",
          (object) "GetDate()",
          (IDbEncoder) DbEncoding.None
        },
        {
          "lastModifiedByFullName",
          (object) rule.LastModifiedByFullName
        },
        {
          "lastModifiedByUserId",
          (object) rule.LastModifiedByUserId
        }
      };
    }

    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.Append("select * from [BR_AutoLockExclusionRule] rules where " + filter);
      dbQueryBuilder.AppendLine("select fields.* from [BR_AutoLockExclusionRule] fields");
      dbQueryBuilder.AppendLine("\tinner join [BR_AutoLockExclusionRule] rules on fields.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      return (BizRuleInfo[]) AutoLockExclusionRulesBpmDbAccessor.dataSetToRuleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    private static AutoLockExclusionRuleInfo[] dataSetToRuleInfos(DataSet data)
    {
      data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      AutoLockExclusionRuleInfo[] ruleInfos = new AutoLockExclusionRuleInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        ruleInfos[index] = new AutoLockExclusionRuleInfo(row);
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

    private static Hashtable dataRowsToFieldRulesHT(ICollection rows)
    {
      if (rows.Count == 0)
        return new Hashtable();
      Hashtable fieldRulesHt = new Hashtable();
      foreach (DataRow row in (IEnumerable) rows)
      {
        string key = (string) row["fieldID"];
        BizRule.FieldRuleType fieldRuleType = (BizRule.FieldRuleType) (byte) row["ruleType"];
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
