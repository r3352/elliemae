// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.ServiceWorkflowHistoryDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class ServiceWorkflowHistoryDbAccessor
  {
    private const string ServiceWorkflowHistoryTable = "WF_History�";
    private const string HistoryColumnLoanID = "LoanID�";
    private const string HistoryColumnRuleID = "RuleID�";
    private const string HistoryColumnLastTriggeredBy = "LastTriggeredByUserID�";
    private const string HistoryColumnLastTriggerTime = "LastTriggerTime�";
    private const string HistoryColumnProfiles = "Profiles�";

    public bool IsRuleTriggered(Guid loanId, int ruleId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = string.Format("SELECT TOP 1 RuleID FROM {0} WHERE {1} = {2} AND {3} = {4}", (object) "WF_History", (object) "LoanID", (object) SQL.Encode((object) loanId.ToString()), (object) "RuleID", (object) SQL.Encode((object) ruleId));
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection != null && dataRowCollection.Count > 0;
    }

    public List<ServiceWorkflowHistory> GetServiceWorkflowHistorys(Guid loanId, int ruleId)
    {
      List<ServiceWorkflowHistory> workflowHistorys = new List<ServiceWorkflowHistory>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = string.Format("SELECT * FROM {0} WHERE {1} = {2} AND {3} = {4}", (object) "WF_History", (object) "LoanID", (object) SQL.Encode((object) loanId.ToString()), (object) "RuleID", (object) SQL.Encode((object) ruleId));
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          ServiceWorkflowHistory history = this.ConvertDataRowToHistory(row);
          if (history != null)
            workflowHistorys.Add(history);
        }
      }
      return workflowHistorys;
    }

    private ServiceWorkflowHistory ConvertDataRowToHistory(DataRow row)
    {
      if (row == null)
        return (ServiceWorkflowHistory) null;
      ServiceWorkflowHistory serviceWorkflowHistory = new ServiceWorkflowHistory();
      serviceWorkflowHistory.LoanID = (Guid) row["LoanID"];
      serviceWorkflowHistory.RuleID = (int) row["RuleID"];
      serviceWorkflowHistory.LastTriggeredByUserID = row["LastTriggeredByUserID"] == DBNull.Value ? string.Empty : (string) row["LastTriggeredByUserID"];
      serviceWorkflowHistory.LastTriggerTime = (DateTime) row["LastTriggerTime"];
      string[] array = (row["Profiles"] == DBNull.Value ? string.Empty : (string) row["Profiles"]).Split(',');
      serviceWorkflowHistory.Profiles = new List<string>();
      Array.ForEach<string>(array, (Action<string>) (profile => serviceWorkflowHistory.Profiles.Add(profile)));
      return serviceWorkflowHistory;
    }

    public void CreateServiceWorkflowHistory(ServiceWorkflowHistory ServiceWorkflowHistory)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("WF_History");
      DbValueList dbValueList = this.GetDBValueList(ServiceWorkflowHistory);
      dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private DbValueList GetDBValueList(ServiceWorkflowHistory history)
    {
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("LoanID", (object) history.LoanID.ToString());
      dbValueList.Add("RuleID", (object) history.RuleID);
      dbValueList.Add("LastTriggeredByUserID", (object) history.LastTriggeredByUserID);
      string str = (string) null;
      if (history.Profiles != null && history.Profiles.Any<string>())
        str = string.Join(",", (IEnumerable<string>) history.Profiles);
      dbValueList.Add("Profiles", (object) str);
      return dbValueList;
    }

    public void DeleteHistory(Guid loanId, int ruleId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text = string.Format("DELETE from {0} where {1} = {2} and {3} = {4}", (object) "WF_History", (object) "LoanID", (object) SQL.Encode((object) loanId.ToString()), (object) "RuleID", (object) SQL.Encode((object) ruleId));
      dbQueryBuilder.AppendLine(text);
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}
