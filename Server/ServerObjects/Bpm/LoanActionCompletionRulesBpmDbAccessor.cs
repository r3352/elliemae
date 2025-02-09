// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.LoanActionCompletionRulesBpmDbAccessor
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
  public sealed class LoanActionCompletionRulesBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "LoanActionCompletionRulesBpmDbAccessor�";

    public LoanActionCompletionRulesBpmDbAccessor()
      : base(ClientSessionCacheID.BpmLoanActionCompletionRules)
    {
    }

    protected override string RuleTableName => "[BR_LoanActionCompletion]";

    protected override Type RuleType => typeof (LoanActionCompletionRuleInfo);

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue ruleIdValue)
    {
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[LACR_RequiredDocs]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[LACR_RequiredTasks]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[LACR_RequiredFields]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[LACR_RequiredMilestones]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[LACR_AdvancedCode]"), ruleIdValue);
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue ruleIdValue)
    {
      LoanActionCompletionRuleInfo completionRuleInfo = (LoanActionCompletionRuleInfo) rule;
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[LACR_RequiredFields]");
      if (completionRuleInfo.Fields != null)
      {
        foreach (FieldLoanActionPair field in completionRuleInfo.Fields)
          sql.InsertInto(table1, new DbValueList()
          {
            ruleIdValue,
            {
              "fieldId",
              (object) field.FieldID
            },
            {
              "loanActionID",
              (object) field.LoanActionID
            }
          }, true, false);
      }
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[LACR_RequiredDocs]");
      if (completionRuleInfo.Docs != null)
      {
        foreach (DocLoanActionPair doc in completionRuleInfo.Docs)
          sql.InsertInto(table2, new DbValueList()
          {
            ruleIdValue,
            {
              "docGuid",
              (object) doc.DocGuid
            },
            {
              "loanActionID",
              (object) doc.LoanActionID
            },
            {
              "attachedRequired",
              doc.AttachedRequired ? (object) "1" : (object) "0"
            }
          }, true, false);
      }
      DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[LACR_RequiredTasks]");
      if (completionRuleInfo.Tasks != null)
      {
        foreach (TaskLoanActionPair task in completionRuleInfo.Tasks)
          sql.InsertInto(table3, new DbValueList()
          {
            ruleIdValue,
            {
              "taskGuid",
              (object) task.TaskGuid
            },
            {
              "loanActionID",
              (object) task.LoanActionID
            }
          }, true, false);
      }
      if (completionRuleInfo.Milestones != null)
      {
        DbTableInfo table4 = EllieMae.EMLite.Server.DbAccessManager.GetTable("LACR_RequiredMilestones");
        foreach (MilestoneLoanActionPair milestone in completionRuleInfo.Milestones)
          sql.InsertInto(table4, new DbValueList()
          {
            ruleIdValue,
            {
              "milestoneID",
              (object) milestone.MilestoneID
            },
            {
              "loanActionID",
              (object) milestone.LoanActionID
            }
          }, true, false);
      }
      DbTableInfo table5 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[LACR_AdvancedCode]");
      if (completionRuleInfo.Tasks == null)
        return;
      foreach (AdvancedCodeLoanActionPair advancedCode in completionRuleInfo.AdvancedCodes)
        sql.InsertInto(table5, new DbValueList()
        {
          ruleIdValue,
          {
            "advancedCode",
            (object) advancedCode.SourceCode
          },
          {
            "loanActionID",
            (object) advancedCode.LoanActionID
          }
        }, true, false);
    }

    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.Append("select * from [BR_LoanActionCompletion] rules where " + filter);
      dbQueryBuilder.AppendLine("select fields.* from [LACR_RequiredFields] fields");
      dbQueryBuilder.AppendLine("\tinner join [BR_LoanActionCompletion] rules on fields.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("select docs.* from [LACR_RequiredDocs] docs");
      dbQueryBuilder.AppendLine("\tinner join [BR_LoanActionCompletion] rules on docs.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("select tasks.* from [LACR_RequiredTasks] tasks");
      dbQueryBuilder.AppendLine("\tinner join [BR_LoanActionCompletion] rules on tasks.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("SELECT milestones.* from [LACR_RequiredMilestones] milestones");
      dbQueryBuilder.AppendLine("\tINNER JOIN [BR_LoanActionCompletion] rules on milestones.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\tWHERE " + filter);
      dbQueryBuilder.AppendLine("select adv.* from [LACR_AdvancedCode] adv");
      dbQueryBuilder.AppendLine("\tinner join [BR_LoanActionCompletion] rules on adv.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      return (BizRuleInfo[]) LoanActionCompletionRulesBpmDbAccessor.dataSetToRuleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    public static string[] GetMilestoneTemplatesByChannelId(int channelId)
    {
      List<string> stringList = new List<string>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("SELECT DISTINCT M.Name FROM MilestoneTemplates M ");
      dbQueryBuilder.Append("INNER JOIN eDisclosureMSTemplateSettings AS MS ");
      dbQueryBuilder.Append("ON M.TemplateID = MS.TemplateID ");
      dbQueryBuilder.Append("INNER JOIN eDisclosureElementAttributes AS EL ");
      dbQueryBuilder.Append("ON EL.eDisclosureElementAttributeId = MS.eDisclosureElementAttributeId  ");
      dbQueryBuilder.Append("WHERE EL.ChannelID = " + (object) channelId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          stringList.Add(dataRow["Name"].ToString());
      }
      return stringList.ToArray();
    }

    public static string[] GetMilestoneNamesByChannelId(int channelId)
    {
      List<string> stringList = new List<string>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("SELECT DISTINCT (M.Name + ',' + M.MilestoneID) AS Name  FROM Milestones M ");
      dbQueryBuilder.Append("INNER JOIN eDisclosureElementAttributes AS EL ");
      dbQueryBuilder.Append("ON EL.RequiredMilestone = M.MilestoneID  ");
      dbQueryBuilder.Append("WHERE EL.ChannelID = " + (object) channelId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          stringList.Add(dataRow["Name"].ToString());
      }
      return stringList.ToArray();
    }

    private static LoanActionCompletionRuleInfo[] dataSetToRuleInfos(DataSet data)
    {
      DataRelation relation1 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataRelation relation2 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[2].Columns["ruleID"]);
      DataRelation relation3 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[3].Columns["ruleID"]);
      DataRelation relation4 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[4].Columns["ruleID"]);
      DataRelation relation5 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[5].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      LoanActionCompletionRuleInfo[] ruleInfos = new LoanActionCompletionRuleInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        FieldLoanActionPair[] fieldLoanActionPairs = LoanActionCompletionRulesBpmDbAccessor.dataRowsToFieldLoanActionPairs((ICollection) row.GetChildRows(relation1));
        DocLoanActionPair[] docLoanActionPairs = LoanActionCompletionRulesBpmDbAccessor.dataRowsToDocLoanActionPairs((ICollection) row.GetChildRows(relation2));
        TaskLoanActionPair[] taskLoanActionPair = LoanActionCompletionRulesBpmDbAccessor.dataRowsToTaskLoanActionPair((ICollection) row.GetChildRows(relation3));
        MilestoneLoanActionPair[] milestoneLoanActionPair = LoanActionCompletionRulesBpmDbAccessor.dataRowsToMilestoneLoanActionPair((ICollection) row.GetChildRows(relation4));
        AdvancedCodeLoanActionPair[] codeRequirements = LoanActionCompletionRulesBpmDbAccessor.dataRowsToAdvancedCodeRequirements((ICollection) row.GetChildRows(relation5));
        ruleInfos[index] = new LoanActionCompletionRuleInfo(row, docLoanActionPairs, fieldLoanActionPairs, taskLoanActionPair, milestoneLoanActionPair, codeRequirements);
      }
      return ruleInfos;
    }

    private static FieldLoanActionPair[] dataRowsToFieldLoanActionPairs(ICollection rows)
    {
      List<FieldLoanActionPair> fieldLoanActionPairList = new List<FieldLoanActionPair>();
      string empty = string.Empty;
      foreach (DataRow row in (IEnumerable) rows)
      {
        string fieldID = (string) row["fieldID"];
        if (fieldID.StartsWith("FD") && fieldID.Length == 6)
          fieldID = fieldID.Replace("FD", "DD");
        fieldLoanActionPairList.Add(new FieldLoanActionPair(fieldID, (string) row["loanActionID"]));
      }
      return fieldLoanActionPairList.ToArray();
    }

    private static DocLoanActionPair[] dataRowsToDocLoanActionPairs(ICollection rows)
    {
      List<DocLoanActionPair> docLoanActionPairList = new List<DocLoanActionPair>();
      foreach (DataRow row in (IEnumerable) rows)
        docLoanActionPairList.Add(new DocLoanActionPair((string) row["docGuid"], (string) row["loanActionID"], (bool) row["attachedRequired"]));
      return docLoanActionPairList.ToArray();
    }

    private static TaskLoanActionPair[] dataRowsToTaskLoanActionPair(ICollection rows)
    {
      List<TaskLoanActionPair> taskLoanActionPairList = new List<TaskLoanActionPair>();
      foreach (DataRow row in (IEnumerable) rows)
        taskLoanActionPairList.Add(new TaskLoanActionPair((string) row["taskGuid"], (string) row["loanActionID"]));
      return taskLoanActionPairList.ToArray();
    }

    private static MilestoneLoanActionPair[] dataRowsToMilestoneLoanActionPair(ICollection rows)
    {
      List<MilestoneLoanActionPair> milestoneLoanActionPairList = new List<MilestoneLoanActionPair>();
      foreach (DataRow row in (IEnumerable) rows)
        milestoneLoanActionPairList.Add(new MilestoneLoanActionPair((string) row["milestoneID"], (string) row["loanActionID"]));
      return milestoneLoanActionPairList.ToArray();
    }

    private static AdvancedCodeLoanActionPair[] dataRowsToAdvancedCodeRequirements(ICollection rows)
    {
      List<AdvancedCodeLoanActionPair> codeLoanActionPairList = new List<AdvancedCodeLoanActionPair>();
      foreach (DataRow row in (IEnumerable) rows)
        codeLoanActionPairList.Add(new AdvancedCodeLoanActionPair((string) row["loanActionID"], (string) row["advancedCode"]));
      return codeLoanActionPairList.ToArray();
    }
  }
}
