// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.ServiceWorkflowRulesBpmDbAccessor
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
  public sealed class ServiceWorkflowRulesBpmDbAccessor : BpmDbAccessor
  {
    private const string className = "ServiceWorkflowRulesBpmDbAccessor�";
    private const string ServiceWorkflowRuleTable = "BR_ServiceWorkflow�";
    private const string ServiceWorkflowActionTable = "WF_Actions�";
    private const string ActionColumnID = "ActionID�";
    private const string ActionColumnType = "ActionType�";
    private const string ActionColumnProfileID = "ProfileID�";

    public ServiceWorkflowRulesBpmDbAccessor()
      : base(ClientSessionCacheID.BpmServiceWorkflowRules)
    {
    }

    protected override string RuleTableName => "BR_ServiceWorkflow";

    protected override Type RuleType => typeof (ServiceWorkflowRuleInfo);

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue ruleIdValue)
    {
      sql.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("WF_Actions"), ruleIdValue);
    }

    protected override DbValueList getDbValueList(BizRuleInfo rule)
    {
      DbValueList dbValueList = base.getDbValueList(rule);
      ServiceWorkflowRuleInfo workflowRuleInfo = (ServiceWorkflowRuleInfo) rule;
      dbValueList.Add("description", (object) workflowRuleInfo.Description);
      dbValueList.Add("serviceType", (object) (int) workflowRuleInfo.ServiceType);
      dbValueList.Remove("lastModTime");
      dbValueList.Add("lastModTime", (object) DateTime.UtcNow);
      dbValueList.Add("LoanFieldId", (object) workflowRuleInfo.LoanFieldId);
      dbValueList.Add("EffectiveDate", (object) workflowRuleInfo.EffectiveDate);
      dbValueList.Add("OrderType", (object) (short) workflowRuleInfo.OrderType);
      return dbValueList;
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue ruleIdValue)
    {
      ServiceWorkflowRuleInfo workflowRuleInfo = (ServiceWorkflowRuleInfo) rule;
      if (workflowRuleInfo.Actions == null)
        return;
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("WF_Actions");
      foreach (ServiceWorkflowAction action in workflowRuleInfo.Actions)
        sql.InsertInto(table, new DbValueList()
        {
          ruleIdValue,
          {
            "ActionID",
            (object) action.ActionID.ToString()
          },
          {
            "ActionType",
            (object) action.ActionType.ToString()
          },
          {
            "ProfileID",
            (object) action.TargetID.ToString()
          }
        }, true, false);
    }

    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.Append(string.Format("select * from {0} rules where ", (object) "BR_ServiceWorkflow") + filter);
      dbQueryBuilder.AppendLine(string.Format("select actions.* from {0} actions", (object) "WF_Actions"));
      dbQueryBuilder.AppendLine(string.Format("\tinner join {0} rules on actions.ruleID = rules.ruleID", (object) "BR_ServiceWorkflow"));
      dbQueryBuilder.AppendLine("\twhere " + filter);
      return (BizRuleInfo[]) ServiceWorkflowRulesBpmDbAccessor.dataSetToRuleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    private static ServiceWorkflowRuleInfo[] dataSetToRuleInfos(DataSet data)
    {
      DataRelation relation = data.Relations.Add(data.Tables[0].Columns["ruleID"], data.Tables[1].Columns["ruleID"]);
      DataTable table = data.Tables[0];
      ServiceWorkflowRuleInfo[] ruleInfos = new ServiceWorkflowRuleInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        ServiceWorkflowAction[] actions = ServiceWorkflowRulesBpmDbAccessor.dataRowsToActions((ICollection) row.GetChildRows(relation));
        ruleInfos[index] = new ServiceWorkflowRuleInfo(row, actions);
      }
      return ruleInfos;
    }

    private static ServiceWorkflowAction[] dataRowsToActions(ICollection rows)
    {
      List<ServiceWorkflowAction> serviceWorkflowActionList = new List<ServiceWorkflowAction>();
      foreach (DataRow row in (IEnumerable) rows)
        serviceWorkflowActionList.Add(new ServiceWorkflowAction((Guid) row["ActionID"], (string) row["ActionType"], (Guid) row["ProfileID"]));
      return serviceWorkflowActionList.ToArray();
    }
  }
}
