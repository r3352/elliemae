// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ReportEngine
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ReportEngine
  {
    private const string className = "ReportEngine�";
    private string userId;

    public ReportEngine(string userId) => this.userId = userId;

    public LoanIdentity[] QueryLoans(
      QueryCriterion[] criteria,
      UserInfo userInfo,
      bool isExternalOrganization)
    {
      string[] fields = new string[4]
      {
        "guid",
        "LoanFolder",
        "LoanName",
        "XrefId"
      };
      QueryCriterion filter = QueryCriterion.Join(criteria, BinaryOperator.And);
      PipelineInfo[] pipelineInfoArray = Pipeline.Generate(userInfo, (string) null, LoanInfo.Right.Access, fields, PipelineData.Fields, filter, isExternalOrganization);
      LoanIdentity[] loanIdentityArray = new LoanIdentity[pipelineInfoArray.Length];
      for (int index = 0; index < pipelineInfoArray.Length; ++index)
      {
        string loanFolder = "";
        string loanName = "";
        string guid = "";
        int xrefId = -1;
        if (pipelineInfoArray[index].Info.ContainsKey((object) "LoanFolder"))
          loanFolder = pipelineInfoArray[index].Info[(object) "LoanFolder"].ToString();
        else if (pipelineInfoArray[index].Info.ContainsKey((object) "Loan.LoanFolder"))
          loanFolder = pipelineInfoArray[index].Info[(object) "Loan.LoanFolder"].ToString();
        if (pipelineInfoArray[index].Info.ContainsKey((object) "LoanName"))
          loanName = pipelineInfoArray[index].Info[(object) "LoanName"].ToString();
        else if (pipelineInfoArray[index].Info.ContainsKey((object) "Loan.LoanName"))
          loanName = pipelineInfoArray[index].Info[(object) "Loan.LoanName"].ToString();
        if (pipelineInfoArray[index].Info.ContainsKey((object) "Guid"))
          guid = pipelineInfoArray[index].Info[(object) "Guid"].ToString();
        else if (pipelineInfoArray[index].Info.ContainsKey((object) "Loan.Guid"))
          guid = pipelineInfoArray[index].Info[(object) "Loan.Guid"].ToString();
        if (pipelineInfoArray[index].Info.ContainsKey((object) "XrefId"))
          xrefId = (int) pipelineInfoArray[index].Info[(object) "XrefId"];
        else if (pipelineInfoArray[index].Info.ContainsKey((object) "Loan.XrefId"))
          xrefId = (int) pipelineInfoArray[index].Info[(object) "Loan.XrefId"];
        loanIdentityArray[index] = new LoanIdentity(loanFolder, loanName, guid, xrefId);
      }
      TraceLog.WriteVerbose(nameof (ReportEngine), "Loan query returned " + (object) loanIdentityArray.Length + " loans.");
      return loanIdentityArray;
    }

    public MilestoneStatistics[] GetMilestoneStatistics(
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      return this.GetMilestoneStatistics(filter, isExternalOrganization, false);
    }

    public MilestoneStatistics[] GetMilestoneStatistics(
      QueryCriterion filter,
      bool isExternalOrganization,
      bool fromHomePage)
    {
      string identitySelectionQuery = new LoanQuery(UserStore.GetLatestVersion(this.userId).UserInfo).CreateIdentitySelectionQuery(filter, isExternalOrganization);
      DbQueryBuilder dbQueryBuilder = !fromHomePage ? new DbQueryBuilder() : new DbQueryBuilder(DBReadReplicaFeature.HomePage);
      dbQueryBuilder.AppendLine("select MilestoneID, Count(*) as RecordCount");
      dbQueryBuilder.AppendLine("from LoanMilestones");
      if ((identitySelectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("where Guid in (" + identitySelectionQuery + ")");
      dbQueryBuilder.AppendLine("group by MilestoneID");
      dbQueryBuilder.AppendLine("select MilestoneID, Count(*) as CompletedCount, Avg(Duration) as AvgDuration, Max(Duration) as MaxDuration");
      dbQueryBuilder.AppendLine("from LoanMilestones");
      dbQueryBuilder.AppendLine("where finished = 1");
      if ((identitySelectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine(" and Guid in (" + identitySelectionQuery + ")");
      dbQueryBuilder.AppendLine("group by MilestoneID");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      List<MilestoneStatistics> milestoneStatisticsList = new List<MilestoneStatistics>();
      List<EllieMae.EMLite.Workflow.Milestone> list = WorkflowBpmDbAccessor.GetMilestones(false).ToList<EllieMae.EMLite.Workflow.Milestone>();
      int num = 0;
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in list)
      {
        MilestoneStatistics milestoneStatistics = new MilestoneStatistics(milestone.MilestoneID, milestone.Name, num++, milestone.DefaultDays);
        DataRow[] dataRowArray1 = dataSet.Tables[0].Select("MilestoneID = '" + milestone.MilestoneID + "'");
        DataRow[] dataRowArray2 = dataSet.Tables[1].Select("MilestoneID = '" + milestone.MilestoneID + "'");
        if (dataRowArray1.Length != 0)
          milestoneStatistics.LoanCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowArray1[0]["RecordCount"], 0);
        if (dataRowArray2.Length != 0)
        {
          milestoneStatistics.CompletedCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowArray2[0]["CompletedCount"], 0);
          milestoneStatistics.AvgDuration = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(dataRowArray2[0]["AvgDuration"], 0M);
          milestoneStatistics.MaxDuration = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowArray2[0]["MaxDuration"], 0);
        }
        if (milestoneStatistics.LoanCount > 0 || !milestone.Archived)
          milestoneStatisticsList.Add(milestoneStatistics);
      }
      return milestoneStatisticsList.ToArray();
    }

    public UserLoanStatistics[] GetLOStatistics(
      QueryCriterion filter,
      Range<DateTime> dateRange,
      bool isExternalOrganization)
    {
      return this.GetLOStatistics(filter, dateRange, isExternalOrganization, false);
    }

    public UserLoanStatistics[] GetLOStatistics(
      QueryCriterion filter,
      Range<DateTime> dateRange,
      bool isExternalOrganization,
      bool fromHomePage)
    {
      string identitySelectionQuery = new LoanQuery(UserStore.GetLatestVersion(this.userId).UserInfo).CreateIdentitySelectionQuery(filter, isExternalOrganization);
      DbQueryBuilder dbQueryBuilder = !fromHomePage ? new DbQueryBuilder() : new DbQueryBuilder(DBReadReplicaFeature.HomePage);
      if ((identitySelectionQuery ?? "") != "")
      {
        dbQueryBuilder.AppendLine("declare @loan_guids table ( guid varchar(38) primary key )");
        dbQueryBuilder.AppendLine("insert into @loan_guids " + identitySelectionQuery);
      }
      dbQueryBuilder.AppendLine("select distinct userId, first_name, last_name");
      dbQueryBuilder.AppendLine("from users inner join LoanSummary Loan on users.userId = Loan.LoanOfficerID");
      if ((identitySelectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("inner merge join @loan_guids guids on Loan.Guid = guids.Guid");
      dbQueryBuilder.AppendLine("select LoanOfficerID, Count(*) as RecordCount");
      dbQueryBuilder.AppendLine("from LoanSummary Loan");
      if ((identitySelectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("inner merge join @loan_guids guids on Loan.Guid = guids.Guid");
      dbQueryBuilder.AppendLine("group by LoanOfficerID");
      dbQueryBuilder.AppendLine("select LoanOfficerID, Count(*) as RecordCount");
      dbQueryBuilder.AppendLine("from LoanSummary Loan");
      if ((identitySelectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("inner merge join @loan_guids guids on Loan.Guid = guids.Guid");
      dbQueryBuilder.AppendLine("where DateOfFinalAction is NULL");
      dbQueryBuilder.AppendLine("group by LoanOfficerID");
      dbQueryBuilder.AppendLine("select LoanOfficerID, Count(*) as RecordCount");
      dbQueryBuilder.AppendLine("from LoanSummary Loan");
      if ((identitySelectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("inner merge join @loan_guids guids on Loan.Guid = guids.Guid");
      dbQueryBuilder.AppendLine("where DateFileOpened " + EllieMae.EMLite.DataAccess.SQL.EncodeDateRange(dateRange, true));
      dbQueryBuilder.AppendLine("group by LoanOfficerID");
      dbQueryBuilder.AppendLine("select LoanOfficerID, Adverse, Count(*) as RecordCount");
      dbQueryBuilder.AppendLine("from LoanSummary Loan");
      if ((identitySelectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("inner merge join @loan_guids guids on Loan.Guid = guids.Guid");
      dbQueryBuilder.AppendLine("where DateOfFinalAction " + EllieMae.EMLite.DataAccess.SQL.EncodeDateRange(dateRange, true));
      dbQueryBuilder.AppendLine("group by LoanOfficerID, Adverse");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      List<UserLoanStatistics> userLoanStatisticsList = new List<UserLoanStatistics>();
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        string userId = string.Concat(row["userid"]);
        UserLoanStatistics userLoanStatistics = new UserLoanStatistics(userId, string.Concat(row["first_name"]), string.Concat(row["last_name"]));
        DataRow[] dataRowArray1 = dataSet.Tables[1].Select("LoanOfficerID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
        DataRow[] dataRowArray2 = dataSet.Tables[2].Select("LoanOfficerID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
        DataRow[] dataRowArray3 = dataSet.Tables[3].Select("LoanOfficerID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
        DataRow[] dataRowArray4 = dataSet.Tables[4].Select("LoanOfficerID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
        if (dataRowArray1.Length != 0)
          userLoanStatistics.AssignedLoanCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowArray1[0]["RecordCount"], 0);
        if (dataRowArray2.Length != 0)
          userLoanStatistics.ActiveLoanCount = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowArray2[0]["RecordCount"], 0);
        if (dataRowArray3.Length != 0)
          userLoanStatistics.LoansStarted = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowArray3[0]["RecordCount"], 0);
        foreach (DataRow dataRow in dataRowArray4)
        {
          if (string.Concat(dataRow["Adverse"]) == "Y")
            userLoanStatistics.LoansAdverse = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["RecordCount"], 0);
          else
            userLoanStatistics.LoansCompleted = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["RecordCount"], 0);
        }
        userLoanStatisticsList.Add(userLoanStatistics);
      }
      return userLoanStatisticsList.ToArray();
    }

    public static string getXMLSettingsRpt(string reportID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT XMLReport FROM [SettingsRptQueue] WHERE reportID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) reportID);
      dbQueryBuilder.AppendLine(text);
      return dbQueryBuilder.ExecuteScalar().ToString();
    }

    public static SettingsRptJobInfo getSettingsRptInfo(string reportID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT ReportName, JobType, Status, CreatedBy, CreateDate FROM [SettingsRptQueue] WHERE reportID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) reportID);
      dbQueryBuilder.AppendLine(text);
      DataRow dataRow = dbQueryBuilder.ExecuteRowQuery();
      int type = (int) EllieMae.EMLite.DataAccess.SQL.Decode(dataRow["JobType"]);
      int status = (int) EllieMae.EMLite.DataAccess.SQL.Decode(dataRow["Status"]);
      DateTime dateTime = Convert.ToDateTime(dataRow["CreateDate"].ToString());
      return new SettingsRptJobInfo((SettingsRptJobInfo.jobType) type, dataRow["ReportName"].ToString(), (SettingsRptJobInfo.jobStatus) status, dataRow["CreatedBy"].ToString(), dateTime.ToString());
    }

    public static NMLSReportData[] GetNMLSReportData(int year, int quarter)
    {
      return ReportEngine.GetNMLSReportData(year, quarter, (string) null);
    }

    public static NMLSReportData[] GetNMLSReportData(int year, int quarter, string state)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("NMLSReportData");
      DbValueList keys = new DbValueList();
      keys.Add("ReportYear", (object) year);
      keys.Add("ReportQuarter", (object) quarter);
      if ((state ?? "") != "")
        keys.Add("StateCode", (object) state);
      dbQueryBuilder.SelectFrom(table, keys);
      return ReportEngine.dataTableToNMLSReportDataList(dbQueryBuilder.ExecuteTableQuery());
    }

    public static NMLSReportData GetNMLSReportData(
      int year,
      int quarter,
      string state,
      string appSource)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("NMLSReportData");
      dbQueryBuilder.SelectFrom(table, new DbValueList()
      {
        {
          "ReportYear",
          (object) year
        },
        {
          "ReportQuarter",
          (object) quarter
        },
        {
          "StateCode",
          (object) state
        },
        {
          "ApplicationSource",
          (object) appSource
        }
      });
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (NMLSReportData) null : ReportEngine.dataRowToNMLSReportData(dataRowCollection[0]);
    }

    public static NMLSReportData[] GetNMLSReportData(string state)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("NMLSReportData");
      DbValue key = new DbValue("StateCode", (object) state);
      dbQueryBuilder.SelectFrom(table, key);
      return ReportEngine.dataTableToNMLSReportDataList(dbQueryBuilder.ExecuteTableQuery());
    }

    public static void SaveNMLSReportData(NMLSReportData reportData)
    {
      ReportEngine.SaveNMLSReportData(new NMLSReportData[1]
      {
        reportData
      });
    }

    public static void SaveNMLSReportData(NMLSReportData[] reportData)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("NMLSReportData");
      foreach (NMLSReportData nmlsReportData in reportData)
      {
        DbValueList dbValueList = new DbValueList();
        dbValueList.Add("StateCode", (object) nmlsReportData.StateCode);
        dbValueList.Add("ReportYear", (object) nmlsReportData.ReportYear);
        dbValueList.Add("ReportQuarter", (object) nmlsReportData.ReportQuarter);
        dbValueList.Add("ApplicationSource", (object) nmlsReportData.ApplicationSource);
        dbQueryBuilder.DeleteFrom(table, dbValueList);
        dbValueList.Add("LoanAmount", (object) nmlsReportData.LoanAmount);
        dbValueList.Add("LoanCount", (object) nmlsReportData.LoanCount);
        dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static NMLSReportData[] dataTableToNMLSReportDataList(DataTable table)
    {
      List<NMLSReportData> nmlsReportDataList = new List<NMLSReportData>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        nmlsReportDataList.Add(ReportEngine.dataRowToNMLSReportData(row));
      return nmlsReportDataList.ToArray();
    }

    private static NMLSReportData dataRowToNMLSReportData(DataRow r)
    {
      return new NMLSReportData(string.Concat(r["StateCode"]), (int) r["ReportYear"], (int) r["ReportQuarter"], string.Concat(r["ApplicationSource"]), (Decimal) r["LoanAmount"], (int) r["LoanCount"]);
    }
  }
}
