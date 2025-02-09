// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.MilestoneRulesBpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.ElliEnum;
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
  public sealed class MilestoneRulesBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "MilestoneRulesBpmDbAccessor�";

    public MilestoneRulesBpmDbAccessor()
      : base(ClientSessionCacheID.BpmMilestoneRules)
    {
    }

    protected override string RuleTableName => "[BR_MilestoneRequirements]";

    protected override Type RuleType => typeof (MilestoneRuleInfo);

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue ruleIdValue)
    {
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[MRR_RequiredDocs]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[MRR_RequiredTasks]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[MRR_RequiredFields]"), ruleIdValue);
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("[MRR_AdvancedCode]"), ruleIdValue);
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue ruleIdValue)
    {
      MilestoneRuleInfo milestoneRuleInfo = (MilestoneRuleInfo) rule;
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[MRR_RequiredFields]");
      if (milestoneRuleInfo.Fields != null)
      {
        foreach (FieldMilestonePair field in milestoneRuleInfo.Fields)
          sql.InsertInto(table1, new DbValueList()
          {
            ruleIdValue,
            {
              "fieldId",
              (object) field.FieldID
            },
            {
              "milestoneID",
              (object) field.MilestoneID
            }
          }, true, false);
      }
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[MRR_RequiredDocs]");
      if (milestoneRuleInfo.Docs != null)
      {
        foreach (DocMilestonePair doc in milestoneRuleInfo.Docs)
          sql.InsertInto(table2, new DbValueList()
          {
            ruleIdValue,
            {
              "docGuid",
              (object) doc.DocGuid
            },
            {
              "milestoneID",
              (object) doc.MilestoneID
            },
            {
              "attachedRequired",
              doc.AttachedRequired ? (object) "1" : (object) "0"
            }
          }, true, false);
      }
      DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[MRR_RequiredTasks]");
      if (milestoneRuleInfo.Tasks != null)
      {
        foreach (TaskMilestonePair task in milestoneRuleInfo.Tasks)
          sql.InsertInto(table3, new DbValueList()
          {
            ruleIdValue,
            {
              "taskGuid",
              (object) task.TaskGuid
            },
            {
              "milestoneID",
              (object) task.MilestoneID
            },
            {
              "isRequired",
              task.isRequired ? (object) "1" : (object) "0"
            },
            {
              "taskType",
              (object) (int) task.TaskType
            },
            {
              "lastModified",
              (object) task.LastModifiedDate
            }
          }, true, false);
      }
      DbTableInfo table4 = EllieMae.EMLite.Server.DbAccessManager.GetTable("[MRR_AdvancedCode]");
      if (milestoneRuleInfo.Tasks == null)
        return;
      foreach (AdvancedCodeMilestonePair advancedCode in milestoneRuleInfo.AdvancedCodes)
        sql.InsertInto(table4, new DbValueList()
        {
          ruleIdValue,
          {
            "advancedCode",
            (object) advancedCode.SourceCode
          },
          {
            "milestoneID",
            (object) advancedCode.MilestoneID
          }
        }, true, false);
    }

    [PgReady]
    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from [BR_MilestoneRequirements] rules where " + filter + ";");
        pgDbQueryBuilder.AppendLine("select fields.* from [MRR_RequiredFields] fields");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_MilestoneRequirements] rules on fields.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter + ";");
        pgDbQueryBuilder.AppendLine("select docs.* from [MRR_RequiredDocs] docs");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_MilestoneRequirements] rules on docs.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter + ";");
        pgDbQueryBuilder.AppendLine("select tasks.*, mtasks.taskName,mtasks.taskDescription, mtasks.daysToComplete, mtasks.taskPriority from [MRR_RequiredTasks] tasks");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_MilestoneRequirements] rules on tasks.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("    left outer join [MilestoneTasks] mtasks on mtasks.taskGUID = tasks.taskGUID ");
        pgDbQueryBuilder.AppendLine("\twhere " + filter + ";");
        pgDbQueryBuilder.AppendLine("select adv.* from [MRR_AdvancedCode] adv");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_MilestoneRequirements] rules on adv.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter + ";");
        return (BizRuleInfo[]) MilestoneRulesBpmDbAccessor.dataSetToRuleInfos(pgDbQueryBuilder.ExecuteSetQuery());
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.Append("select * from [BR_MilestoneRequirements] rules where " + filter);
      dbQueryBuilder.AppendLine("select fields.* from [MRR_RequiredFields] fields");
      dbQueryBuilder.AppendLine("\tinner join [BR_MilestoneRequirements] rules on fields.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("select docs.* from [MRR_RequiredDocs] docs");
      dbQueryBuilder.AppendLine("\tinner join [BR_MilestoneRequirements] rules on docs.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("select tasks.*, mtasks.taskName,mtasks.taskDescription, mtasks.daysToComplete, mtasks.taskPriority from [MRR_RequiredTasks] tasks");
      dbQueryBuilder.AppendLine("\tinner join [BR_MilestoneRequirements] rules on tasks.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("    left outer join [MilestoneTasks] mtasks on mtasks.taskGUID = tasks.taskGUID ");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("select adv.* from [MRR_AdvancedCode] adv");
      dbQueryBuilder.AppendLine("\tinner join [BR_MilestoneRequirements] rules on adv.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      return (BizRuleInfo[]) MilestoneRulesBpmDbAccessor.dataSetToRuleInfos(dbQueryBuilder.ExecuteSetQuery());
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

    private static MilestoneRuleInfo[] dataSetToRuleInfos(DataSet data)
    {
      DataRelation relation1 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataRelation relation2 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[2].Columns["ruleID"]);
      DataRelation relation3 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[3].Columns["ruleID"]);
      DataRelation relation4 = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[4].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      MilestoneRuleInfo[] ruleInfos = new MilestoneRuleInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        FieldMilestonePair[] fieldMilestonePairs = MilestoneRulesBpmDbAccessor.dataRowsToFieldMilestonePairs((ICollection) row.GetChildRows(relation1));
        DocMilestonePair[] docMilestonePairs = MilestoneRulesBpmDbAccessor.dataRowsToDocMilestonePairs((ICollection) row.GetChildRows(relation2));
        TaskMilestonePair[] taskMilestonePairs = MilestoneRulesBpmDbAccessor.dataRowsToTaskMilestonePairs((ICollection) row.GetChildRows(relation3));
        AdvancedCodeMilestonePair[] codeRequirements = MilestoneRulesBpmDbAccessor.dataRowsToAdvancedCodeRequirements((ICollection) row.GetChildRows(relation4));
        ruleInfos[index] = new MilestoneRuleInfo(row, docMilestonePairs, fieldMilestonePairs, taskMilestonePairs, codeRequirements);
      }
      return ruleInfos;
    }

    private static FieldMilestonePair[] dataRowsToFieldMilestonePairs(ICollection rows)
    {
      List<FieldMilestonePair> fieldMilestonePairList = new List<FieldMilestonePair>();
      string empty = string.Empty;
      foreach (DataRow row in (IEnumerable) rows)
      {
        string fieldID = (string) row["fieldID"];
        if (fieldID.StartsWith("FD") && fieldID.Length == 6)
          fieldID = fieldID.Replace("FD", "DD");
        fieldMilestonePairList.Add(new FieldMilestonePair(fieldID, (string) row["milestoneID"]));
      }
      return fieldMilestonePairList.ToArray();
    }

    [PgReady]
    private static DocMilestonePair[] dataRowsToDocMilestonePairs(ICollection rows)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        List<DocMilestonePair> docMilestonePairList = new List<DocMilestonePair>();
        foreach (DataRow row in (IEnumerable) rows)
          docMilestonePairList.Add(new DocMilestonePair((string) row["docGuid"], (string) row["milestoneID"], SQL.DecodeBoolean(row["attachedRequired"])));
        return docMilestonePairList.ToArray();
      }
      List<DocMilestonePair> docMilestonePairList1 = new List<DocMilestonePair>();
      foreach (DataRow row in (IEnumerable) rows)
        docMilestonePairList1.Add(new DocMilestonePair((string) row["docGuid"], (string) row["milestoneID"], (bool) row["attachedRequired"]));
      return docMilestonePairList1.ToArray();
    }

    [PgReady]
    private static TaskMilestonePair[] dataRowsToTaskMilestonePairs(ICollection rows)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        List<TaskMilestonePair> taskMilestonePairList = new List<TaskMilestonePair>();
        foreach (DataRow row in (IEnumerable) rows)
          taskMilestonePairList.Add(new TaskMilestonePair(SQL.DecodeString(row["taskGuid"]), SQL.DecodeString(row["milestoneID"]), SQL.DecodeBoolean(row["isRequired"]), SQL.DecodeString(row["taskName"]), SQL.DecodeString(row["taskDescription"]), SQL.DecodeInt(row["daysToComplete"]), SQL.DecodeInt(row["taskPriority"]), SQL.DecodeEnum<MilestoneTaskType>(row["taskType"]), new DateTime?(SQL.DecodeDateTime(row["lastModified"]))));
        return taskMilestonePairList.ToArray();
      }
      List<TaskMilestonePair> taskMilestonePairList1 = new List<TaskMilestonePair>();
      foreach (DataRow row in (IEnumerable) rows)
        taskMilestonePairList1.Add(new TaskMilestonePair(SQL.DecodeString(row["taskGuid"]), SQL.DecodeString(row["milestoneID"]), SQL.DecodeBoolean(row["isRequired"]), SQL.DecodeString(row["taskName"]), SQL.DecodeString(row["taskDescription"]), SQL.DecodeInt(row["daysToComplete"]), SQL.DecodeInt(row["taskPriority"]), SQL.DecodeEnum<MilestoneTaskType>(row["taskType"]), new DateTime?(SQL.DecodeDateTime(row["lastModified"]))));
      return taskMilestonePairList1.ToArray();
    }

    private static AdvancedCodeMilestonePair[] dataRowsToAdvancedCodeRequirements(ICollection rows)
    {
      List<AdvancedCodeMilestonePair> codeMilestonePairList = new List<AdvancedCodeMilestonePair>();
      foreach (DataRow row in (IEnumerable) rows)
        codeMilestonePairList.Add(new AdvancedCodeMilestonePair((string) row["milestoneID"], (string) row["advancedCode"]));
      return codeMilestonePairList.ToArray();
    }
  }
}
