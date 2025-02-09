// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.PipelineEngine.MsPipelineEngine
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataAccess.Postgres;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.PipelineEngine
{
  internal class MsPipelineEngine(ClientContext context) : PiplineEngineBase(context)
  {
    private string className = nameof (MsPipelineEngine);
    private static readonly string sw = Tracing.SwSYSTEM;

    [PgReady]
    protected override PipelineInfo[] GeneratePipelineInternal(
      PipelineParameters parameters,
      out int totalCount)
    {
      totalCount = -1;
      if (this.CurrentContext.Settings.DbServerType == DbServerType.Postgres)
      {
        int result = int.MaxValue;
        int.TryParse(EnConfigurationSettings.AppSettings["PipelineQueryFieldCountLimit"], out result);
        if (parameters.Fields != null && ((IEnumerable<string>) parameters.Fields).Count<string>() > result)
          Tracing.Log(MsPipelineEngine.sw, TraceLevel.Warning, this.className, string.Format("Selected fields count ({0}), exceeded the configured limit ({1}).", (object) ((IEnumerable<string>) parameters.Fields).Count<string>(), (object) result));
        if (parameters.FieldTranslator == null)
          parameters.FieldTranslator = (ICriterionTranslator) new EllieMae.EMLite.Server.Query.LoanFieldTranslator();
        EllieMae.EMLite.Server.Query.LoanQuery loanQuery = new EllieMae.EMLite.Server.Query.LoanQuery(parameters.User, parameters.AccessRights, parameters.FieldTranslator, parameters.LoanFolders, parameters.LoanFolders != null || parameters.GuidList != null || Pipeline.IsExclusiveFilter(parameters.Filter));
        IQueryTerm[] fields = (IQueryTerm[]) DataField.CreateFields(parameters.Fields);
        EllieMae.EMLite.Server.PgDbQueryBuilder idbqb = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        bool flag1 = parameters.PaginationInfo != null && parameters.PaginationInfo.Start >= 0 && parameters.PaginationInfo.Limit > 0;
        bool flag2 = parameters.DataToInclude == PipelineData.Fields && parameters.GuidList == null && parameters.IdentitySelectionQuery == null && !flag1;
        int? nullable1;
        if (flag1)
        {
          nullable1 = parameters.MaxCount;
          if (nullable1.HasValue)
          {
            PipelinePagination paginationInfo = parameters.PaginationInfo;
            int limit1 = parameters.PaginationInfo.Limit;
            nullable1 = parameters.MaxCount;
            int valueOrDefault = nullable1.GetValueOrDefault();
            int limit2;
            if (!(limit1 > valueOrDefault & nullable1.HasValue))
            {
              limit2 = parameters.PaginationInfo.Limit;
            }
            else
            {
              nullable1 = parameters.MaxCount;
              limit2 = nullable1.Value;
            }
            paginationInfo.Limit = limit2;
            PipelineParameters pipelineParameters = parameters;
            nullable1 = new int?();
            int? nullable2 = nullable1;
            pipelineParameters.MaxCount = nullable2;
          }
        }
        if (parameters.LoanFolders != null)
        {
          string valuesString = string.Join(", ", ((IEnumerable<string>) new HashSet<string>((IEnumerable<string>) parameters.LoanFolders).ToArray<string>()).Select<string, string>((System.Func<string, string>) (f => "(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) f) + ")")));
          idbqb.AppendLine("-- Create a temp table for the loan folders for filtering");
          PgQueryHelpers.CreateTempTable((EllieMae.EMLite.DataAccess.PgDbQueryBuilder) idbqb, "v_tmpLoanFolders", (IEnumerable<DbVariable>) new DbVariable("name", DbType.AnsiString, 128).ToList(), (System.Func<DbVariable, string>) (v => valuesString));
        }
        if (!flag2)
        {
          idbqb.AppendLine("-- Create a temp table for the loan guid to return");
          idbqb.AppendLine("CREATE TEMP TABLE v_loan_guids ( guid varchar(38) primary key );");
          if (parameters.GuidList != null)
            idbqb.AppendLine("insert into v_loan_guids select Guid from FN_ParseGuids(" + Pipeline.EncodeParseableGuidList(parameters.GuidList) + ")");
          else if (parameters.IdentitySelectionQuery != null && parameters.User != (UserInfo) null)
            idbqb.AppendLine("insert into v_loan_guids select Guid from LoanSummary where Guid in (" + parameters.IdentitySelectionQuery + ") and Guid in (" + loanQuery.CreateIdentitySelectionQuery(parameters.IsExternalOrganization) + ")");
          else if (parameters.IdentitySelectionQuery != null)
            idbqb.AppendLine("insert into v_loan_guids " + parameters.IdentitySelectionQuery);
          else if (flag1)
          {
            string text = loanQuery.CreateIdentitySelectionQuery(parameters.Filter, parameters.SortFields, (IQueryTerm[]) null, parameters.ApplyUserAccessFiltering, parameters.IsExternalOrganization, excludeArchivedLoans: parameters.excludeArchivedLoans) + (parameters.SortFields == null || parameters.SortFields.Length == 0 ? " order by [Guid] " + Environment.NewLine : loanQuery.GetOrderByClause(parameters.SortFields)) + string.Format(" offset {0} rows fetch next {1} rows only ", (object) parameters.PaginationInfo.Start, (object) parameters.PaginationInfo.Limit);
            idbqb.AppendLine(";with paginationCte as (");
            idbqb.AppendLine(text);
            idbqb.AppendLine(")");
            idbqb.AppendLine("insert into @loan_guids ");
            idbqb.AppendLine("select [Guid] from paginationCte");
            idbqb.AppendLine("");
          }
          else
            idbqb.AppendLine("insert into v_loan_guids " + loanQuery.CreateIdentitySelectionQuery(parameters.Filter, parameters.IsExternalOrganization));
          idbqb.AppendLine(";");
        }
        if (parameters.Fields == null)
        {
          idbqb.Append("SELECT Loan.*");
        }
        else
        {
          idbqb.Append("SELECT Loan.Guid");
          if (((IEnumerable<string>) parameters.Fields).Any<string>())
            idbqb.Append(", " + loanQuery.GetFieldSelectionList(fields));
        }
        idbqb.AppendLine("");
        if (flag2)
        {
          if (parameters.User == (UserInfo) null || parameters.User.IsSuperAdministrator() || parameters.IsExternalOrganization || parameters.ApplyUserAccessFiltering || parameters.LoanFolders != null && !((IEnumerable<string>) parameters.LoanFolders).Contains<string>("<All Folders>"))
          {
            idbqb.Append(loanQuery.GetTableSelectionClause(fields, parameters.Filter, parameters.SortFields, false, true, parameters.IsExternalOrganization));
            idbqb.Append(loanQuery.GetFilterClause(parameters.Filter));
          }
          else
          {
            idbqb.Append(loanQuery.GetTableSelectionClause(fields, parameters.Filter, parameters.SortFields, false, false, parameters.IsExternalOrganization));
            idbqb.Append(loanQuery.GetFilterClause(parameters.Filter, parameters.ApplyUserAccessFiltering, parameters.UseGetLoansForMyLoans, parameters.User.Userid, parameters.LoanFolders));
          }
        }
        else
        {
          idbqb.Append(loanQuery.GetTableSelectionClause(fields, (QueryCriterion) null, parameters.SortFields, false, false, parameters.IsExternalOrganization));
          idbqb.AppendLine("    inner join v_loan_guids guids on Loan.Guid = guids.Guid");
        }
        if (parameters.SortFields != null && parameters.SortFields.Length != 0)
          idbqb.Append(loanQuery.GetOrderByClause(parameters.SortFields));
        nullable1 = parameters.MaxCount;
        if (nullable1.HasValue)
          idbqb.AppendLine("LIMIT " + (object) parameters.MaxCount + ";");
        idbqb.AppendLine(";");
        Bitmask bitmask = new Bitmask((object) parameters.DataToInclude);
        if (bitmask.Contains((object) PipelineData.Alerts))
        {
          idbqb.Append("select ls.Guid, LoanAlerts.* from LoanAlerts inner join LoanSummary ls on ls.XRefID = LoanAlerts.LoanXRefId inner join v_loan_guids guids on ls.Guid = guids.Guid ");
          if (parameters.User != (UserInfo) null)
          {
            idbqb.Append("left outer join AclGroupMembers agm on LoanAlerts.GroupID = agm.GroupID ");
            idbqb.Append("where (LoanAlerts.UserID is NULL And LoanAlerts.GroupID is NULL) or (LoanAlerts.UserID = @userid) Or (agm.UserID = @userid)");
          }
          idbqb.AppendLine(";");
        }
        if (bitmask.Contains((object) PipelineData.AlertSummary))
        {
          if (parameters.User == (UserInfo) null)
            throw new InvalidOperationException("Cannot specify AlertSummary if no User is specified");
          idbqb.AppendLine("select ls.Guid, Alerts.* from " + loanQuery.GetAlertSummaryInlineFunction() + "(@userid, " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.Now) + ", null) Alerts inner join LoanSummary ls on ls.XRefID = Alerts.LoanXRefID inner join v_loan_guids guids on ls.guid = guids.Guid;");
        }
        if (bitmask.Contains((object) PipelineData.Borrowers))
          idbqb.AppendLine("select LoanBorrowers.* from LoanBorrowers inner join v_loan_guids guids on LoanBorrowers.Guid = guids.Guid;");
        if (bitmask.Contains((object) PipelineData.Lock))
        {
          string str = "";
          if (Loan.IncludeCrashedSessionLoanLocks)
            str = " left";
          idbqb.AppendLine("select LoanLock.*, Sessions.SessionID, Sessions.ServerUri from LoanLock" + str + " join Sessions on LoanLock.loginSessionID = Sessions.SessionID inner join v_loan_guids guids on LoanLock.Guid = guids.Guid;");
        }
        if (bitmask.Contains((object) PipelineData.AccessRights))
          idbqb.AppendLine("select r.* from UserLoanAccessRights r inner join v_loan_guids guids on r.Guid = guids.Guid;");
        else if (bitmask.Contains((object) PipelineData.CurrentUserAccessRightsOnly))
          idbqb.AppendLine("select r.* from UserLoanAccessRights r inner join v_loan_guids guids on r.Guid = guids.Guid where r.userid = @userid;");
        else if (bitmask.Contains((object) PipelineData.AssignedRights))
          idbqb.AppendLine("select r.* from loan_rights r inner join v_loan_guids guids on r.Guid = guids.Guid;");
        if (bitmask.Contains((object) PipelineData.Milestones) || bitmask.Contains((object) PipelineData.LoanAssociates))
          idbqb.AppendLine("select LoanMilestones.* from LoanMilestones inner join v_loan_guids guids on LoanMilestones.Guid = guids.Guid;");
        if (bitmask.Contains((object) PipelineData.LoanAssociates))
          idbqb.AppendLine("select LoanAssociateDetails.* from LoanAssociateDetails inner join v_loan_guids guids on LoanAssociateDetails.Guid = guids.Guid;");
        if (bitmask.Contains((object) PipelineData.Trade))
        {
          if (parameters.TradeType != TradeType.None)
          {
            if (string.IsNullOrEmpty(parameters.IdentitySelectionQuery))
              idbqb.AppendLine("select TradeAssignment.*, Trades.TradeType from TradeAssignment inner join v_loan_guids guids on TradeAssignment.LoanGuid = guids.Guid inner join Trades on Trades.TradeID = TradeAssignment.TradeID;");
            else if (parameters.TradeType == TradeType.GSECommitment)
              idbqb.AppendLine("select ta.TradeID, ta.LoanGUID, ta.Profit, ta.AssignedStatus, ta.AssignedStatusDate, ta.PendingStatus, ta.PendingStatusDate, ta.Status, ta.StatusDate, ta.Rejected, ta.CommitmentContractNumber, ta.ProductName, g.TradeId as GseCommitmentId, ta.EPPSLoanProgramName, Trades.TradeType from TradeAssignment ta inner join v_loan_guids guids on ta.LoanGuid = guids.Guid inner join GseCommitments g on g.ContractNumber = ta.CommitmentContractNumber inner join Trades on Trades.TradeID = g.TradeID where Trades.TradeType = " + (object) (int) parameters.TradeType + ";");
            else
              idbqb.AppendLine("select TradeAssignment.*, Trades.TradeType from TradeAssignment inner join v_loan_guids guids on TradeAssignment.LoanGuid = guids.Guid inner join Trades on Trades.TradeID = TradeAssignment.TradeID where Trades.TradeType = " + (object) (int) parameters.TradeType + ";");
          }
          else
            idbqb.AppendLine("select TradeAssignment.*, Trades.TradeType from TradeAssignment inner join v_loan_guids guids on TradeAssignment.LoanGuid = guids.Guid inner join Trades on Trades.TradeID = TradeAssignment.TradeID;");
          idbqb.AppendLine("select TradeRejections.* from TradeRejections inner join v_loan_guids guids on TradeRejections.LoanGuid = guids.Guid;");
        }
        idbqb.Replace("#loanFolders", "v_tmpLoanFolders");
        idbqb.Replace("@loanFolders", "'v_tmpLoanFolders'");
        DbCommandParameter[] array = new DbCommandParameter("userid", (object) parameters.User.Userid, DbType.AnsiString).ToArray();
        DataSet dataSet = idbqb.ExecuteSetQuery(DbTransactionType.Snapshot, array);
        PerformanceMeter.Current.AddNote("Loan Summary Records = " + (object) dataSet.Tables[0].Rows.Count);
        int index1 = 1;
        if (bitmask.Contains((object) PipelineData.Alerts))
        {
          PerformanceMeter.Current.AddNote("Alert Records = " + (object) dataSet.Tables[index1].Rows.Count);
          dataSet.Relations.Add("Alerts", dataSet.Tables[0].Columns["Guid"], dataSet.Tables[index1].Columns["Guid"]);
          ++index1;
        }
        if (bitmask.Contains((object) PipelineData.AlertSummary))
        {
          PerformanceMeter.Current.AddNote("Alert Summary Records = " + (object) dataSet.Tables[index1].Rows.Count);
          dataSet.Relations.Add("AlertSummary", dataSet.Tables[0].Columns["Guid"], dataSet.Tables[index1].Columns["Guid"]);
          ++index1;
        }
        if (bitmask.Contains((object) PipelineData.Borrowers))
        {
          PerformanceMeter.Current.AddNote("LoanBorrowers Records = " + (object) dataSet.Tables[index1].Rows.Count);
          dataSet.Relations.Add("Borrowers", dataSet.Tables[0].Columns["Guid"], dataSet.Tables[index1].Columns["Guid"]);
          ++index1;
        }
        if (bitmask.Contains((object) PipelineData.Lock))
        {
          PerformanceMeter.Current.AddNote("Lock Records = " + (object) dataSet.Tables[index1].Rows.Count);
          dataSet.Relations.Add("Lock", dataSet.Tables[0].Columns["Guid"], dataSet.Tables[index1].Columns["guid"]);
          ++index1;
        }
        if (bitmask.Contains((object) PipelineData.AssignedRights) || bitmask.Contains((object) PipelineData.AccessRights) || bitmask.Contains((object) PipelineData.CurrentUserAccessRightsOnly))
        {
          PerformanceMeter.Current.AddNote("Rights Records = " + (object) dataSet.Tables[index1].Rows.Count);
          dataSet.Relations.Add("Rights", dataSet.Tables[0].Columns["Guid"], dataSet.Tables[index1].Columns["guid"]);
          ++index1;
        }
        if (bitmask.Contains((object) PipelineData.Milestones))
        {
          PerformanceMeter.Current.AddNote("Milestone Records = " + (object) dataSet.Tables[index1].Rows.Count);
          dataSet.Relations.Add("Milestones", dataSet.Tables[0].Columns["Guid"], dataSet.Tables[index1].Columns["guid"]);
          ++index1;
        }
        if (bitmask.Contains((object) PipelineData.LoanAssociates))
        {
          PerformanceMeter.Current.AddNote("Loan Assoc Records = " + (object) dataSet.Tables[index1].Rows.Count);
          dataSet.Relations.Add("LoanAssociates", dataSet.Tables[0].Columns["guid"], dataSet.Tables[index1].Columns["guid"]);
          ++index1;
        }
        if (bitmask.Contains((object) PipelineData.Trade))
        {
          Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>();
          List<DataRow> dataRowList = new List<DataRow>();
          if (dataSet.Tables[0].Columns.Contains("Trade__TradeType"))
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
            {
              string key = row["Guid"].ToString();
              int num = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["Trade__TradeType"]);
              if (num == 0)
              {
                if (!dictionary.ContainsKey(key))
                  dictionary.Add(key, row);
              }
              else if ((parameters.TradeType == TradeType.CorrespondentTrade && num == 4 || parameters.TradeType == TradeType.LoanTrade && (num == 2 || num == 3) || parameters.TradeType == TradeType.MbsPool && (num == 2 || EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["Trade__TradeType"]) == 3)) && !dictionary.ContainsKey(key))
                dictionary.Add(key, row);
            }
          }
          else
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
            {
              string key = row["Guid"].ToString();
              if (!dictionary.ContainsKey(key))
                dictionary.Add(key, row);
            }
          }
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            string key = row["Guid"].ToString();
            if (dictionary.ContainsKey(key))
            {
              if (dictionary[key] != row)
                dataRowList.Add(row);
            }
            else
              dictionary.Add(key, row);
          }
          for (int index2 = 0; index2 < dataSet.Tables[0].Rows.Count; ++index2)
          {
            DataRow row = dataSet.Tables[0].Rows[index2];
            if (dataRowList.Contains(row))
              row.Delete();
          }
          dataSet.Tables[0].AcceptChanges();
          int count = dataSet.Tables.Count;
          if (dataSet.Tables[count - 2] != null && dataSet.Tables[count - 2].Columns.Contains("LoanGuid") && dataSet.Tables[count - 2].Columns.Contains("TradeType"))
          {
            for (int index3 = 0; index3 < dataSet.Tables[count - 2].Rows.Count; ++index3)
            {
              DataRow row = dataSet.Tables[count - 2].Rows[index3];
              int num = EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TradeType"]);
              if (!dictionary.ContainsKey(row["LoanGuid"].ToString()))
                row.Delete();
              else if (parameters.TradeType == TradeType.CorrespondentTrade && num != 4 || parameters.TradeType == TradeType.LoanTrade && num != 2 && num != 3 || parameters.TradeType == TradeType.MbsPool && num != 2 && num != 3)
                row.Delete();
            }
          }
          dataSet.Tables[1].AcceptChanges();
          if (dataSet.Tables[count - 1] != null && dataSet.Tables[count - 1].Columns.Contains("LoanGuid"))
          {
            for (int index4 = 0; index4 < dataSet.Tables[count - 1].Rows.Count; ++index4)
            {
              DataRow row = dataSet.Tables[count - 1].Rows[index4];
              if (!dictionary.ContainsKey(row["LoanGuid"].ToString()))
                row.Delete();
            }
          }
          dataSet.Tables[1].AcceptChanges();
          PerformanceMeter.Current.AddNote("Trade Assignment Records = " + (object) dataSet.Tables[index1].Rows.Count);
          DataRelationCollection relations1 = dataSet.Relations;
          DataColumn column1 = dataSet.Tables[0].Columns["guid"];
          DataTableCollection tables1 = dataSet.Tables;
          int index5 = index1;
          int index6 = index5 + 1;
          DataColumn column2 = tables1[index5].Columns["LoanGuid"];
          relations1.Add("TradeAssignment", column1, column2);
          PerformanceMeter.Current.AddNote("Trade Rejection Records = " + (object) dataSet.Tables[index6].Rows.Count);
          DataRelationCollection relations2 = dataSet.Relations;
          DataColumn column3 = dataSet.Tables[0].Columns["guid"];
          DataTableCollection tables2 = dataSet.Tables;
          int index7 = index6;
          int num1 = index7 + 1;
          DataColumn column4 = tables2[index7].Columns["LoanGuid"];
          relations2.Add("TradeRejections", column3, column4);
        }
        int count1 = dataSet.Tables[0].Rows.Count;
        Dictionary<string, bool> dictionary1 = new Dictionary<string, bool>();
        ArrayList arrayList1 = new ArrayList();
        for (int index8 = 0; index8 < count1; ++index8)
        {
          Hashtable info = new Hashtable((IEqualityComparer) StringComparer.InvariantCultureIgnoreCase);
          DataRow row = dataSet.Tables[0].Rows[index8];
          string guid = row["Guid"].ToString();
          if (!dictionary1.ContainsKey(guid))
          {
            dictionary1[guid] = true;
            foreach (DataColumn column in (InternalDataCollectionBase) row.Table.Columns)
            {
              string criterionName = QueryEngine.ColumnNameToCriterionName(column.ColumnName);
              info[(object) criterionName] = EllieMae.EMLite.DataAccess.SQL.Decode(row[column.ColumnName]);
            }
            PipelineInfo.Alert[] alerts = (PipelineInfo.Alert[]) null;
            if (bitmask.Contains((object) PipelineData.Alerts))
            {
              DataRow[] childRows = dataSet.Tables[0].Rows[index8].GetChildRows("Alerts");
              alerts = new PipelineInfo.Alert[childRows.Length];
              for (int index9 = 0; index9 < childRows.Length; ++index9)
              {
                DataRow dataRow = childRows[index9];
                alerts[index9] = new PipelineInfo.Alert()
                {
                  AlertTargetID = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["UniqueID"]),
                  AlertID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["AlertType"]),
                  Event = string.Concat(dataRow["Event"]),
                  Status = string.Concat(dataRow["status"]),
                  Date = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["AlertDate"]),
                  GroupID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["GroupID"], -1),
                  DisplayStatus = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["DisplayStatus"], 1),
                  SnoozeStartDTTM = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["SnoozeStartDTTM"], DateTime.MinValue),
                  SnoozeDuration = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["SnoozeDuration"], 0),
                  LoanAlertID = string.Concat(dataRow["LoanAlertId"]),
                  UserID = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["UserID"])
                };
              }
            }
            PipelineInfo.AlertSummaryInfo alertSummary = (PipelineInfo.AlertSummaryInfo) null;
            if (bitmask.Contains((object) PipelineData.AlertSummary))
            {
              DataRow[] childRows = dataSet.Tables[0].Rows[index8].GetChildRows("AlertSummary");
              alertSummary = childRows.Length != 0 ? new PipelineInfo.AlertSummaryInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRows[0]["AlertCount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(childRows[0]["AlertDate"])) : new PipelineInfo.AlertSummaryInfo(0, DateTime.MaxValue);
            }
            PipelineInfo.Borrower[] borrowers = (PipelineInfo.Borrower[]) null;
            if (bitmask.Contains((object) PipelineData.Borrowers))
            {
              DataRow[] childRows = dataSet.Tables[0].Rows[index8].GetChildRows("Borrowers");
              borrowers = new PipelineInfo.Borrower[childRows.Length];
              for (int index10 = 0; index10 < childRows.Length; ++index10)
              {
                DataRow dataRow = childRows[index10];
                borrowers[index10] = new PipelineInfo.Borrower()
                {
                  PairIndex = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["PairIndex"]),
                  BorrowerType = EllieMae.EMLite.DataAccess.SQL.DecodeEnum<LoanBorrowerType>(dataRow["BorrowerType"]),
                  FirstName = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["FirstName"]),
                  LastName = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["LastName"]),
                  HomePhone = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["HomePhone"]),
                  WorkPhone = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["WorkPhone"]),
                  CellPhone = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["CellPhone"]),
                  Email = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Email"])
                };
              }
            }
            LockInfo lockInfo = (LockInfo) null;
            List<LockInfo> locks = (List<LockInfo>) null;
            if (bitmask.Contains((object) PipelineData.Lock))
            {
              DataRow[] childRows = dataSet.Tables[0].Rows[index8].GetChildRows("Lock");
              if (childRows.Length == 0)
              {
                lockInfo = new LockInfo(guid);
                locks = new List<LockInfo>();
              }
              else
              {
                locks = new List<LockInfo>();
                foreach (DataRow dataRow in childRows)
                {
                  DataRow[] source = dataRow.Table.Select();
                  locks.Add(new LockInfo(dataRow["guid"].ToString(), dataRow["lockedby"].ToString(), "", "", EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["SessionID"], (string) null), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["ServerUri"], (string) null), EllieMae.EMLite.DataAccess.SQL.DecodeEnum<LoanInfo.LockReason>(dataRow["lockedfor"]), (DateTime) dataRow["locktime"], EllieMae.EMLite.DataAccess.SQL.DecodeEnum<LockInfo.ExclusiveLock>(dataRow["exclusive"]), lockedByList: ((IEnumerable<DataRow>) source).Where<DataRow>((System.Func<DataRow, bool>) (rowElement => rowElement["guid"].ToString() == guid)).Select<DataRow, string>((System.Func<DataRow, string>) (resultRow => resultRow["lockedby"].ToString())).ToArray<string>()));
                }
                lockInfo = locks[0];
              }
            }
            Hashtable rights = (Hashtable) null;
            if (bitmask.Contains((object) PipelineData.AssignedRights) || bitmask.Contains((object) PipelineData.AccessRights) || bitmask.Contains((object) PipelineData.CurrentUserAccessRightsOnly))
            {
              rights = new Hashtable();
              foreach (DataRow childRow in dataSet.Tables[0].Rows[index8].GetChildRows("Rights"))
                rights.Add((object) childRow["userid"].ToString(), (object) (int) childRow["rights"]);
            }
            PipelineInfo.MilestoneInfo[] milestones = (PipelineInfo.MilestoneInfo[]) null;
            if (bitmask.Contains((object) PipelineData.Milestones))
            {
              DataRow[] childRows = dataSet.Tables[0].Rows[index8].GetChildRows("Milestones");
              milestones = new PipelineInfo.MilestoneInfo[childRows.Length];
              for (int index11 = 0; index11 < childRows.Length; ++index11)
              {
                DataRow dataRow = childRows[index11];
                string milestoneID = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["milestoneID"]);
                string milestoneName = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["milestoneName"]);
                int roleID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["RoleID"], -1);
                string associateGuid = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["associateGuid"], (string) null);
                bool finished = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["finished"]);
                bool reviewed = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["reviewed"]);
                DateTime dateStarted = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["dateStarted"]);
                DateTime dateCompleted = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["dateCompleted"]);
                int order = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["order"], 0);
                milestones[order] = new PipelineInfo.MilestoneInfo(milestoneID, milestoneName, roleID, associateGuid, finished, reviewed, order, dateStarted, dateCompleted);
              }
            }
            PipelineInfo.LoanAssociateInfo[] loanAssociates = (PipelineInfo.LoanAssociateInfo[]) null;
            if (bitmask.Contains((object) PipelineData.LoanAssociates))
            {
              ArrayList arrayList2 = new ArrayList();
              foreach (DataRow childRow in dataSet.Tables[0].Rows[index8].GetChildRows("LoanAssociates"))
              {
                int roleId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRow["RoleID"]);
                string roleName = string.Concat(childRow["RoleName"]);
                string roleAbbrev = string.Concat(childRow["RoleAbbr"]);
                string milestoneId = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRow["milestoneID"]);
                string milestoneName = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRow["milestoneName"]);
                string userId = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRow["UserID"]);
                int groupId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRow["GroupID"], -1);
                string fname = string.Concat(childRow["first_name"]);
                string lname = string.Concat(childRow["last_name"]);
                string fullname = string.Concat(childRow["FullName"]);
                string email = string.Concat(childRow["Email"]);
                string phone = string.Concat(childRow["Phone"]);
                string fax = string.Concat(childRow["Fax"]);
                bool writeAccess = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(childRow["AllowWrites"]);
                LoanAssociateType associateType = EllieMae.EMLite.DataAccess.SQL.DecodeEnum<LoanAssociateType>(childRow["AssociateType"]);
                string associateGuid = string.Concat(childRow["AssociateGuid"]);
                int order = 0;
                if (string.Concat(childRow["order"]) != "")
                  order = (int) childRow["order"];
                if (associateType != LoanAssociateType.None)
                  arrayList2.Add((object) new PipelineInfo.LoanAssociateInfo(associateGuid, associateType, userId, groupId, fname, lname, fullname, email, phone, fax, roleId, roleName, roleAbbrev, milestoneId, milestoneName, writeAccess, order));
              }
              loanAssociates = (PipelineInfo.LoanAssociateInfo[]) arrayList2.ToArray(typeof (PipelineInfo.LoanAssociateInfo));
            }
            PipelineInfo.TradeInfo assignedTrade = (PipelineInfo.TradeInfo) null;
            PipelineInfo.TradeInfo[] tradeAssignments = (PipelineInfo.TradeInfo[]) null;
            string[] rejectedInvestors = (string[]) null;
            if (bitmask.Contains((object) PipelineData.Trade))
            {
              DataRow[] childRows1 = dataSet.Tables[0].Rows[index8].GetChildRows("TradeAssignment");
              tradeAssignments = new PipelineInfo.TradeInfo[childRows1.Length];
              for (int index12 = 0; index12 < childRows1.Length; ++index12)
              {
                PipelineInfo.TradeInfo tradeInfo = new PipelineInfo.TradeInfo();
                tradeInfo.TradeID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRows1[index12]["TradeID"]);
                tradeInfo.AssignedStatus = EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRows1[index12]["AssignedStatus"]);
                tradeInfo.PendingStatus = EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRows1[index12]["PendingStatus"]);
                tradeInfo.Profit = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(childRows1[index12]["Profit"]);
                tradeInfo.CommitmentContractNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRows1[index12]["CommitmentContractNumber"], "");
                tradeInfo.ProductName = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRows1[index12]["ProductName"], "");
                tradeInfo.GseCommitmentId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRows1[index12]["GseCommitmentId"], -1);
                tradeInfo.EPPSLoanProgramName = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRows1[index12]["EPPSLoanProgramName"], "");
                tradeAssignments[index12] = tradeInfo;
                if (parameters.TradeType != TradeType.GSECommitment)
                  tradeInfo.TotalPrice = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(childRows1[index12]["TotalPrice"]);
                tradeAssignments[index12] = tradeInfo;
                if (tradeInfo.Status >= 2)
                  assignedTrade = tradeInfo;
              }
              DataRow[] childRows2 = dataSet.Tables[0].Rows[index8].GetChildRows("TradeRejections");
              rejectedInvestors = new string[childRows2.Length];
              for (int index13 = 0; index13 < childRows2.Length; ++index13)
                rejectedInvestors[index13] = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRows2[index13]["Investor"]);
            }
            arrayList1.Add((object) new PipelineInfo(info, borrowers, alerts, alertSummary, loanAssociates, lockInfo, locks, rights, milestones, assignedTrade, tradeAssignments, rejectedInvestors));
          }
        }
        return (PipelineInfo[]) arrayList1.ToArray(typeof (PipelineInfo));
      }
      if (parameters == null)
        return (PipelineInfo[]) null;
      int result1 = int.MaxValue;
      bool flag3 = parameters.GuidList != null && ((IEnumerable<string>) parameters.GuidList).Count<string>() == 1 && Utils.ParseBoolean((object) Company.GetCompanySetting("FEATURE", "EnableOptParseLoanGuids"));
      int.TryParse(EnConfigurationSettings.AppSettings["PipelineQueryFieldCountLimit"], out result1);
      if (parameters.Fields != null)
      {
        if (((IEnumerable<string>) parameters.Fields).Count<string>() > result1)
          Tracing.Log(MsPipelineEngine.sw, TraceLevel.Warning, this.className, string.Format("Selected fields count ({0}), exceeded the configured limit ({1}).", (object) ((IEnumerable<string>) parameters.Fields).Count<string>(), (object) result1));
        if (((IEnumerable<string>) parameters.Fields).Contains<string>("Loan.IsArchived"))
          parameters.Fields = ((IEnumerable<string>) parameters.Fields).Concat<string>((IEnumerable<string>) new string[1]
          {
            "LoanFolder.FolderName"
          }).ToArray<string>();
      }
      bool boolean = Utils.ParseBoolean((object) Company.GetCompanySetting("POLICIES", "UsePiplineOptimization"));
      bool excludeArchivedLoans = Utils.ParseBoolean((object) Company.GetCompanySetting("POLICIES", "EnableLoanSoftArchival")) && parameters.excludeArchivedLoans;
      if (parameters.FieldTranslator == null)
        parameters.FieldTranslator = (ICriterionTranslator) new EllieMae.EMLite.Server.Query.LoanFieldTranslator();
      EllieMae.EMLite.Server.Query.LoanQuery loanQuery1 = new EllieMae.EMLite.Server.Query.LoanQuery(parameters.User, parameters.AccessRights, parameters.FieldTranslator, parameters.LoanFolders, parameters.LoanFolders != null || parameters.GuidList != null || Pipeline.IsExclusiveFilter(parameters.Filter), parameters.GuidList, parameters.excludeArchivedLoans);
      IQueryTerm[] fields1 = (IQueryTerm[]) DataField.CreateFields(parameters.Fields);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      bool flag4 = parameters.PaginationInfo != null && parameters.PaginationInfo.Start >= 0 && parameters.PaginationInfo.Limit > 0;
      bool flag5 = parameters.DataToInclude == PipelineData.Fields && parameters.GuidList == null && parameters.IdentitySelectionQuery == null && !flag4 && parameters.CalculateCountOnly != CalculateTotalCountEnum.CountOnly;
      int? nullable3;
      if (flag4)
      {
        nullable3 = parameters.MaxCount;
        if (nullable3.HasValue)
        {
          PipelinePagination paginationInfo = parameters.PaginationInfo;
          int limit3 = parameters.PaginationInfo.Limit;
          nullable3 = parameters.MaxCount;
          int valueOrDefault = nullable3.GetValueOrDefault();
          int limit4;
          if (!(limit3 > valueOrDefault & nullable3.HasValue))
          {
            limit4 = parameters.PaginationInfo.Limit;
          }
          else
          {
            nullable3 = parameters.MaxCount;
            limit4 = nullable3.Value;
          }
          paginationInfo.Limit = limit4;
          PipelineParameters pipelineParameters = parameters;
          nullable3 = new int?();
          int? nullable4 = nullable3;
          pipelineParameters.MaxCount = nullable4;
        }
      }
      if (parameters.LoanFolders != null)
      {
        dbQueryBuilder.AppendLine("-- Create a temp table for the loan folders for filtering");
        dbQueryBuilder.AppendLine("\r\nif object_id('tempdb..#loanFolders', 'U') is not null\r\n\tdrop table #loanFolders\r\ncreate table #loanFolders(name varchar(250) primary key)");
        string[] array = new HashSet<string>((IEnumerable<string>) parameters.LoanFolders).ToArray<string>();
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < array.Length; ++index)
        {
          stringBuilder.Append("(").Append(EllieMae.EMLite.DataAccess.SQL.Encode((object) array[index])).Append(")");
          if (index != array.Length - 1)
            stringBuilder.Append(",");
        }
        dbQueryBuilder.AppendLine(string.Format("insert into #loanFolders values {0}", (object) stringBuilder.ToString()));
        dbQueryBuilder.AppendLine("declare @loanFolders StringTable");
        dbQueryBuilder.AppendLine(string.Format("insert into @loanFolders values {0}", (object) stringBuilder.ToString()));
      }
      else if ((parameters.IsGlobalSearch | boolean) & flag5 && !parameters.User.IsSuperAdministrator())
      {
        dbQueryBuilder.AppendLine("if object_id('tempdb..#usrloanFolders', 'U') is not null ");
        dbQueryBuilder.AppendLine("drop table #usrloanFolders ");
        dbQueryBuilder.AppendLine("create table #usrloanFolders(name varchar(100) primary key) ");
        dbQueryBuilder.AppendLine("insert into #usrloanFolders ");
        dbQueryBuilder.AppendLine("select distinct aglfa.FolderName ");
        dbQueryBuilder.AppendLine("from AclGroupLoanFolderAccess aglfa ");
        dbQueryBuilder.AppendLine("inner join AclGroupMembers agmu on agmu.GroupID = aglfa.GroupID ");
        dbQueryBuilder.AppendLine("where aglfa.Access = 1 and agmu.UserID = '" + parameters.User.Userid + "'");
        if (EncompassServer.ServerMode != EncompassServerMode.Service)
          dbQueryBuilder.AppendLine(" and aglfa.FolderName <> '(Trash)' ");
      }
      if (parameters.CalculateCountOnly == CalculateTotalCountEnum.CountOnly)
      {
        if (parameters.GuidList != null)
        {
          if (!flag3)
          {
            dbQueryBuilder.AppendLine("-- Create a temp table for the loan guid to return");
            dbQueryBuilder.AppendLine("declare @loan_guids table ( guid varchar(38) primary key )");
            dbQueryBuilder.AppendLine("insert into @loan_guids select Guid from FN_ParseGuids(" + Pipeline.EncodeParseableGuidList(parameters.GuidList) + ")");
            dbQueryBuilder.AppendLine("declare @loanguids GuidTable");
            dbQueryBuilder.AppendLine("insert into @loanguids select Guid from FN_ParseGuids(" + Pipeline.EncodeParseableGuidList(parameters.GuidList) + ")");
          }
          dbQueryBuilder.AppendLine("select COUNT(DISTINCT Loan.Guid) as totalCount");
          dbQueryBuilder.AppendLine(loanQuery1.GetTableSelectionClause(fields1, (QueryCriterion) null, (SortField[]) null, false, false, parameters.IsExternalOrganization));
          if (parameters.User != (UserInfo) null && !parameters.User.IsSuperAdministrator())
            dbQueryBuilder.AppendLine("inner join FN_GetUsersAccessibleLoans(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.User.Userid) + ", null) _vid_ual on _vid_ual.Guid = Loan.Guid");
          if (flag3)
            dbQueryBuilder.AppendLine("where Loan.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
          else
            dbQueryBuilder.AppendLine("inner join @loan_guids guids on Loan.Guid = guids.Guid");
          if (excludeArchivedLoans)
            dbQueryBuilder.AppendLine("and Loan.IsArchived = 0");
        }
        else
        {
          string identitySelectionQuery = loanQuery1.CreateIdentitySelectionQuery(parameters.Filter, (SortField[]) null, (IQueryTerm[]) null, parameters.ApplyUserAccessFiltering, parameters.IsExternalOrganization, excludeArchivedLoans: excludeArchivedLoans);
          dbQueryBuilder.AppendLine("select COUNT(1) as totalCount");
          dbQueryBuilder.AppendLine("from (");
          dbQueryBuilder.AppendLine(identitySelectionQuery);
          dbQueryBuilder.AppendLine(") guids");
        }
        DataRow dataRow = dbQueryBuilder.ExecuteRowQuery(DbTransactionType.None);
        totalCount = Convert.ToInt32(dataRow[nameof (totalCount)]);
        return new PipelineInfo[0];
      }
      if (parameters.GuidList != null)
      {
        dbQueryBuilder.AppendLine("-- Create a temp table for the loan guid to return");
        dbQueryBuilder.AppendLine("declare @loanguids GuidTable");
        dbQueryBuilder.AppendLine("insert into @loanguids select Guid from FN_ParseGuids(" + Pipeline.EncodeParseableGuidList(parameters.GuidList) + ")");
      }
      if (!flag5 && !flag3 | flag4)
      {
        dbQueryBuilder.AppendLine("-- Create a temp table for the loan guid to return");
        dbQueryBuilder.AppendLine("declare @loan_guids table ( guid varchar(38) primary key )");
        if (parameters.GuidList != null)
          dbQueryBuilder.AppendLine("insert into @loan_guids select Guid from FN_ParseGuids(" + Pipeline.EncodeParseableGuidList(parameters.GuidList) + ")");
        else if (parameters.IdentitySelectionQuery != null && parameters.User != (UserInfo) null)
          dbQueryBuilder.AppendLine("insert into @loan_guids select Guid from LoanSummary where Guid in (" + parameters.IdentitySelectionQuery + ") and Guid in (" + loanQuery1.CreateIdentitySelectionQuery((QueryCriterion) null, true, parameters.IsExternalOrganization, excludeArchivedLoans: excludeArchivedLoans) + ")");
        else if (parameters.IdentitySelectionQuery != null)
          dbQueryBuilder.AppendLine("insert into @loan_guids " + parameters.IdentitySelectionQuery);
        else if (flag4)
        {
          string identitySelectionQuery = loanQuery1.CreateIdentitySelectionQuery(parameters.Filter, parameters.SortFields, (IQueryTerm[]) null, parameters.ApplyUserAccessFiltering, parameters.IsExternalOrganization, excludeArchivedLoans: excludeArchivedLoans);
          if (Company.GetCompanySetting("PIPELINE", "CompatibilityMode") == "2008")
          {
            List<SortColumn> sortColumnList = new List<SortColumn>();
            if (parameters.SortFields != null && parameters.SortFields.Length != 0)
            {
              foreach (SortField sortField in parameters.SortFields)
              {
                string columnName = QueryEngine.CriterionNameToColumnName(sortField.Term.FieldName.Replace(" ", ""));
                sortColumnList.Add(new SortColumn(columnName, sortField.SortOrder == FieldSortOrder.Descending ? SortOrder.Descending : SortOrder.Ascending));
              }
              if (!((IEnumerable<SortField>) parameters.SortFields).Any<SortField>((System.Func<SortField, bool>) (t => t.Term.FieldName.Equals("Loan.Guid", StringComparison.OrdinalIgnoreCase))))
                sortColumnList.Add(new SortColumn("[Guid]", SortOrder.Ascending));
            }
            else
              sortColumnList.Add(new SortColumn("[Guid]", SortOrder.Ascending));
            string str = dbQueryBuilder.OrderBy(sortColumnList, true);
            int num2 = parameters.PaginationInfo.Start + 1;
            int num3 = parameters.PaginationInfo.Start + parameters.PaginationInfo.Limit;
            dbQueryBuilder.AppendLine(";with loanIdSelectionCte as (");
            dbQueryBuilder.AppendLine(identitySelectionQuery);
            dbQueryBuilder.AppendLine("),");
            dbQueryBuilder.AppendLine("rowNumberCte as (");
            dbQueryBuilder.AppendLine(string.Format("select *, row_number() over ({0}) as RowNum from loanIdSelectionCte", (object) str));
            dbQueryBuilder.AppendLine(")");
            dbQueryBuilder.AppendLine("insert into @loan_guids");
            dbQueryBuilder.AppendLine(string.Format("select [Guid] from rowNumberCte where RowNum between {0} and {1}", (object) num2, (object) num3));
            dbQueryBuilder.AppendLine("");
          }
          else
          {
            string str;
            if (parameters.SortFields != null && parameters.SortFields.Length != 0)
            {
              List<SortField> list = ((IEnumerable<SortField>) parameters.SortFields).ToList<SortField>();
              if (!((IEnumerable<SortField>) parameters.SortFields).Any<SortField>((System.Func<SortField, bool>) (t => t.Term.FieldName.Equals("Loan.Guid", StringComparison.OrdinalIgnoreCase))))
                list.Add(new SortField("Loan.Guid", FieldSortOrder.Ascending));
              str = identitySelectionQuery + loanQuery1.GetOrderByClause(list.ToArray());
            }
            else
              str = identitySelectionQuery + " order by [Guid] " + Environment.NewLine;
            string text = str + string.Format(" offset {0} rows fetch next {1} rows only ", (object) parameters.PaginationInfo.Start, (object) parameters.PaginationInfo.Limit);
            dbQueryBuilder.AppendLine(";with paginationCte as (");
            dbQueryBuilder.AppendLine(text);
            dbQueryBuilder.AppendLine(")");
            dbQueryBuilder.AppendLine("insert into @loan_guids");
            dbQueryBuilder.AppendLine("select [Guid] from paginationCte");
            dbQueryBuilder.AppendLine("");
          }
        }
        else
          dbQueryBuilder.AppendLine("insert into @loan_guids " + loanQuery1.CreateIdentitySelectionQuery(parameters.Filter, true, parameters.IsExternalOrganization, excludeArchivedLoans: excludeArchivedLoans));
      }
      if (parameters.Fields == null)
      {
        nullable3 = parameters.MaxCount;
        if (nullable3.HasValue)
          dbQueryBuilder.AppendLine("select top " + (object) parameters.MaxCount + " Loan.*");
        else
          dbQueryBuilder.AppendLine("select Loan.*");
      }
      else if (parameters.Fields.Length == 0)
      {
        nullable3 = parameters.MaxCount;
        if (nullable3.HasValue)
          dbQueryBuilder.AppendLine("select top " + (object) parameters.MaxCount + " Loan.Guid");
        else
          dbQueryBuilder.AppendLine("select Loan.Guid");
      }
      else
      {
        nullable3 = parameters.MaxCount;
        if (nullable3.HasValue)
          dbQueryBuilder.AppendLine("select top " + (object) parameters.MaxCount + " Loan.Guid, " + loanQuery1.GetFieldSelectionList(fields1));
        else
          dbQueryBuilder.AppendLine("select Loan.Guid, " + loanQuery1.GetFieldSelectionList(fields1));
      }
      bool isOptFlow = false;
      string empty = string.Empty;
      SortField[] sortFields = parameters.SortFields;
      string str1 = (sortFields != null ? (sortFields.Length != 0 ? 1 : 0) : 0) != 0 ? ", " + loanQuery1.GetNewOrderByClause(parameters.SortFields, isArchiveFieldExist: ((IEnumerable<SortField>) parameters.SortFields).Any<SortField>((System.Func<SortField, bool>) (x => x.Term.FieldName == "[Loan].[IsArchived]")) && loanQuery1.GetFieldSelectionList(fields1).Contains("Loan__IsArchived")) : " ";
      if (flag5)
      {
        if (parameters.User == (UserInfo) null || parameters.User.IsSuperAdministrator() || parameters.IsExternalOrganization || parameters.ApplyUserAccessFiltering || parameters.LoanFolders != null && !((IEnumerable<string>) parameters.LoanFolders).Contains<string>("<All Folders>"))
        {
          dbQueryBuilder.Append(loanQuery1.GetTableSelectionClause(fields1, parameters.Filter, parameters.SortFields, false, true, parameters.IsExternalOrganization, parameters.IsGlobalSearch, true, excludeArchivedLoans: excludeArchivedLoans));
          string str2 = dbQueryBuilder.ToString();
          if (str2.Contains("NewOPTFLOW"))
          {
            isOptFlow = true;
            dbQueryBuilder.Replace("-- NewOPTFLOW", " ");
            dbQueryBuilder.Replace("select Loan.Guid", "select Loan.Guid" + str1);
          }
          if (excludeArchivedLoans && str2.Contains("--newaccessibleflow"))
            dbQueryBuilder.Replace("--newaccessibleflow", " ");
          dbQueryBuilder.Append(loanQuery1.GetFilterClause(parameters.Filter, boolean, parameters.User.IsSuperAdministrator(), parameters.IsGlobalSearch, isOptFlow, parameters.User.Userid, excludeArchivedLoans));
          if (isOptFlow)
            dbQueryBuilder.Append("group by Loan.Guid " + str1);
          if (excludeArchivedLoans)
          {
            string query = dbQueryBuilder.ToString();
            dbQueryBuilder.Reset();
            dbQueryBuilder.Append(loanQuery1.GetArchivalClause(query));
          }
        }
        else
        {
          dbQueryBuilder.Append(loanQuery1.GetTableSelectionClause(fields1, parameters.Filter, parameters.SortFields, false, false, parameters.IsExternalOrganization));
          dbQueryBuilder.Append(loanQuery1.GetFilterClause(parameters.Filter, parameters.ApplyUserAccessFiltering, parameters.UseGetLoansForMyLoans, parameters.User.Userid, parameters.LoanFolders));
        }
      }
      else
      {
        dbQueryBuilder.AppendLine(loanQuery1.GetTableSelectionClause(fields1, flag4 ? parameters.Filter : (QueryCriterion) null, parameters.SortFields, false, false, parameters.IsExternalOrganization, excludeArchivedLoans: excludeArchivedLoans));
        if (parameters.GuidList != null && parameters.User != (UserInfo) null && !parameters.User.IsSuperAdministrator() && EncompassServer.ServerMode == EncompassServerMode.Service)
          dbQueryBuilder.AppendLine("inner join FN_GetUsersAccessibleLoans(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.User.Userid) + ", null) _vid_ual on _vid_ual.Guid = Loan.Guid");
        if (flag3 && !flag4)
          dbQueryBuilder.AppendLine("where Loan.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
        else
          dbQueryBuilder.AppendLine("inner join @loan_guids guids on Loan.Guid = guids.Guid");
        if (flag4)
          dbQueryBuilder.AppendLine(loanQuery1.GetFilterClause(parameters.Filter));
      }
      if (excludeArchivedLoans & !flag4)
      {
        string query = dbQueryBuilder.ToString();
        dbQueryBuilder.Reset();
        dbQueryBuilder.Append(loanQuery1.GetArchivalClause(query));
      }
      if (!isOptFlow && parameters.SortFields != null && parameters.SortFields.Length != 0)
      {
        bool isArchiveFieldExist = false;
        if (loanQuery1.GetFieldSelectionList(fields1).Contains("Loan__IsArchived"))
          isArchiveFieldExist = true;
        dbQueryBuilder.Append(loanQuery1.GetOrderByClause(parameters.SortFields, isArchiveFieldExist));
      }
      Bitmask bitmask1 = new Bitmask((object) parameters.DataToInclude);
      if (bitmask1.Contains((object) PipelineData.Alerts))
      {
        dbQueryBuilder.Append("select ls.Guid, LoanAlerts.* from LoanAlerts inner join LoanSummary ls on ls.XRefID = LoanAlerts.LoanXRefId ");
        if (!flag3)
        {
          dbQueryBuilder.Append("inner join @loan_guids guids on ls.Guid = guids.Guid ");
          if (parameters.User != (UserInfo) null)
          {
            dbQueryBuilder.Append("left outer join AclGroupMembers agm on LoanAlerts.GroupID = agm.GroupID ");
            dbQueryBuilder.Append("where ((LoanAlerts.UserID is NULL And LoanAlerts.GroupID is NULL) or (LoanAlerts.UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.User.Userid) + ") Or (agm.UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.User.Userid) + "))");
          }
        }
        else if (parameters.User != (UserInfo) null)
        {
          dbQueryBuilder.Append("left outer join AclGroupMembers agm on LoanAlerts.GroupID = agm.GroupID ");
          dbQueryBuilder.Append("where ((LoanAlerts.UserID is NULL And LoanAlerts.GroupID is NULL) or (LoanAlerts.UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.User.Userid) + ") Or (agm.UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.User.Userid) + "))");
          dbQueryBuilder.Append("and ls.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
        }
        else
          dbQueryBuilder.Append("where ls.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
        dbQueryBuilder.AppendLine("");
      }
      if (bitmask1.Contains((object) PipelineData.AlertSummary))
      {
        if (parameters.User == (UserInfo) null)
          throw new InvalidOperationException("Cannot specify AlertSummary if no User is specified");
        dbQueryBuilder.AppendLine("select ls.Guid, Alerts.* from " + loanQuery1.GetAlertSummaryInlineFunction() + "(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.User.Userid) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.Now) + ", null) Alerts inner join LoanSummary ls on ls.XRefID = Alerts.LoanXRefID ");
        if (flag3)
          dbQueryBuilder.Append(" where ls.Guid = '" + Convert.ToString(parameters.GuidList[0]) + "'");
        else
          dbQueryBuilder.Append(" inner join @loan_guids guids on ls.guid = guids.Guid");
        dbQueryBuilder.AppendLine("");
      }
      if (bitmask1.Contains((object) PipelineData.Borrowers))
      {
        dbQueryBuilder.AppendLine("select LoanBorrowers.* from LoanBorrowers ");
        if (flag3)
          dbQueryBuilder.Append(" where LoanBorrowers.Guid = '" + Convert.ToString(parameters.GuidList[0]) + "'");
        else
          dbQueryBuilder.Append(" inner join @loan_guids guids on LoanBorrowers.Guid = guids.Guid");
        dbQueryBuilder.AppendLine("");
      }
      if (bitmask1.Contains((object) PipelineData.Lock))
      {
        string str3 = "";
        if (Loan.IncludeCrashedSessionLoanLocks)
          str3 = " left";
        dbQueryBuilder.AppendLine("select LoanLock.*, Sessions.SessionID, Sessions.ServerUri from LoanLock" + str3 + " join Sessions on LoanLock.loginSessionID = Sessions.SessionID ");
        if (flag3)
          dbQueryBuilder.Append(" where LoanLock.Guid = '" + Convert.ToString(parameters.GuidList[0]) + "'");
        else
          dbQueryBuilder.Append(" inner join @loan_guids guids on LoanLock.Guid = guids.Guid");
        dbQueryBuilder.AppendLine("");
      }
      if (bitmask1.Contains((object) PipelineData.AccessRights))
      {
        dbQueryBuilder.AppendLine("select r.* from UserLoanAccessRights r ");
        if (flag3)
          dbQueryBuilder.Append(" where r.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
        else
          dbQueryBuilder.Append(" inner join @loan_guids guids on r.Guid = guids.Guid");
        dbQueryBuilder.AppendLine("");
      }
      else if (bitmask1.Contains((object) PipelineData.CurrentUserAccessRightsOnly))
      {
        dbQueryBuilder.AppendLine("select r.* from UserLoanAccessRights r ");
        if (!flag3)
          dbQueryBuilder.Append(" inner join @loan_guids guids on r.Guid = guids.Guid where r.userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.User.Userid));
        else
          dbQueryBuilder.Append(" where r.userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.User.Userid) + " and r.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
        dbQueryBuilder.AppendLine("");
      }
      else if (bitmask1.Contains((object) PipelineData.AssignedRights))
      {
        dbQueryBuilder.AppendLine("select r.* from loan_rights r ");
        if (flag3)
          dbQueryBuilder.Append(" where r.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
        else
          dbQueryBuilder.Append(" inner join @loan_guids guids on r.Guid = guids.Guid");
        dbQueryBuilder.AppendLine("");
      }
      if (bitmask1.Contains((object) PipelineData.Milestones) || bitmask1.Contains((object) PipelineData.LoanAssociates))
      {
        dbQueryBuilder.AppendLine("select LoanMilestones.* from LoanMilestones ");
        if (flag3)
          dbQueryBuilder.Append(" where LoanMilestones.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
        else
          dbQueryBuilder.Append(" inner join @loan_guids guids on LoanMilestones.Guid = guids.Guid");
        dbQueryBuilder.AppendLine("");
      }
      if (bitmask1.Contains((object) PipelineData.LoanAssociates))
      {
        dbQueryBuilder.AppendLine("select LoanAssociateDetails.* from LoanAssociateDetails ");
        if (flag3)
          dbQueryBuilder.Append(" where LoanAssociateDetails.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
        else
          dbQueryBuilder.Append(" inner join @loan_guids guids on LoanAssociateDetails.Guid = guids.Guid");
        dbQueryBuilder.AppendLine("");
      }
      if (bitmask1.Contains((object) PipelineData.Trade))
      {
        if (parameters.TradeType != TradeType.None)
        {
          if (string.IsNullOrEmpty(parameters.IdentitySelectionQuery))
          {
            dbQueryBuilder.AppendLine("select TradeAssignment.*, Trades.TradeType from TradeAssignment inner join Trades on Trades.TradeID = TradeAssignment.TradeID ");
            if (flag3)
              dbQueryBuilder.Append(" where TradeAssignment.LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
            else
              dbQueryBuilder.Append(" inner join @loan_guids guids on TradeAssignment.LoanGuid = guids.Guid");
            dbQueryBuilder.AppendLine("");
          }
          else if (parameters.TradeType == TradeType.GSECommitment)
          {
            dbQueryBuilder.AppendLine("select ta.TradeID, ta.LoanGUID, ta.Profit, ta.AssignedStatus, ta.AssignedStatusDate, ta.PendingStatus, ta.PendingStatusDate, ta.Status, ta.StatusDate, ta.Rejected, ta.CommitmentContractNumber, ta.ProductName, g.TradeId as GseCommitmentId, ta.EPPSLoanProgramName, Trades.TradeType from TradeAssignment ta inner join GseCommitments g on g.ContractNumber = ta.CommitmentContractNumber inner join Trades on Trades.TradeID = g.TradeID ");
            if (flag3)
              dbQueryBuilder.Append(" where ta.LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]) + " and Trades.TradeType = " + (object) (int) parameters.TradeType);
            else
              dbQueryBuilder.Append(" inner join @loan_guids guids on ta.LoanGuid = guids.Guid  where Trades.TradeType = " + (object) (int) parameters.TradeType);
            dbQueryBuilder.AppendLine("");
          }
          else
          {
            dbQueryBuilder.AppendLine("select TradeAssignment.*, Trades.TradeType from TradeAssignment inner join Trades on Trades.TradeID = TradeAssignment.TradeID ");
            if (flag3)
              dbQueryBuilder.Append(" where TradeAssignment.LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]) + " and Trades.TradeType = " + (object) (int) parameters.TradeType);
            else
              dbQueryBuilder.Append(" inner join @loan_guids guids on TradeAssignment.LoanGuid = guids.Guid where Trades.TradeType = " + (object) (int) parameters.TradeType);
            dbQueryBuilder.AppendLine("");
          }
        }
        else
        {
          dbQueryBuilder.AppendLine("select TradeAssignment.*, Trades.TradeType from TradeAssignment inner join Trades on Trades.TradeID = TradeAssignment.TradeID ");
          if (flag3)
            dbQueryBuilder.Append(" where TradeAssignment.LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
          else
            dbQueryBuilder.Append(" inner join @loan_guids guids on TradeAssignment.LoanGuid = guids.Guid");
          dbQueryBuilder.AppendLine("");
        }
        dbQueryBuilder.AppendLine("select TradeRejections.* from TradeRejections ");
        if (flag3)
          dbQueryBuilder.Append(" where TradeRejections.LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
        else
          dbQueryBuilder.Append(" inner join @loan_guids guids on TradeRejections.LoanGuid = guids.Guid");
        dbQueryBuilder.AppendLine("");
      }
      if (loanQuery1.UseOptionRecompileForAlerts & flag5 && this.isAlertsIncluded(dbQueryBuilder.ToString()))
        dbQueryBuilder.AppendLine(" OPTION (RECOMPILE)");
      DataSet dataSet1 = dbQueryBuilder.ExecuteSetQuery(DbTransactionType.Snapshot);
      PerformanceMeter.Current.AddNote("Loan Summary Records = " + (object) dataSet1.Tables[0].Rows.Count);
      if (isOptFlow && parameters.SortFields != null && parameters.SortFields.Length != 0)
      {
        DataView defaultView = dataSet1.Tables[0].DefaultView;
        defaultView.Sort = loanQuery1.GetNewOrderByClause(parameters.SortFields, true, loanQuery1.GetFieldSelectionList(fields1).Contains("Loan__IsArchived"));
        dataSet1.Tables.RemoveAt(0);
        dataSet1.Tables.Add(defaultView.ToTable());
      }
      int index14 = 1;
      if (bitmask1.Contains((object) PipelineData.Alerts))
      {
        PerformanceMeter.Current.AddNote("Alert Records = " + (object) dataSet1.Tables[index14].Rows.Count);
        if (dataSet1.Tables[0].Rows.Count <= 0 && dataSet1.Tables[index14].Rows.Count > 0)
          throw new DataAccessViolationException("User does not have access rights to the loan.");
        dataSet1.Relations.Add("Alerts", dataSet1.Tables[0].Columns["Guid"], dataSet1.Tables[index14].Columns["Guid"]);
        ++index14;
      }
      if (bitmask1.Contains((object) PipelineData.AlertSummary))
      {
        PerformanceMeter.Current.AddNote("Alert Summary Records = " + (object) dataSet1.Tables[index14].Rows.Count);
        if (dataSet1.Tables[0].Rows.Count <= 0 && dataSet1.Tables[index14].Rows.Count > 0)
          throw new DataAccessViolationException("User does not have access rights to the loan.");
        dataSet1.Relations.Add("AlertSummary", dataSet1.Tables[0].Columns["Guid"], dataSet1.Tables[index14].Columns["Guid"]);
        ++index14;
      }
      if (bitmask1.Contains((object) PipelineData.Borrowers))
      {
        PerformanceMeter.Current.AddNote("LoanBorrowers Records = " + (object) dataSet1.Tables[index14].Rows.Count);
        if (dataSet1.Tables[0].Rows.Count <= 0 && dataSet1.Tables[index14].Rows.Count > 0)
          throw new DataAccessViolationException("User does not have access rights to the loan.");
        dataSet1.Relations.Add("Borrowers", dataSet1.Tables[0].Columns["Guid"], dataSet1.Tables[index14].Columns["Guid"]);
        ++index14;
      }
      if (bitmask1.Contains((object) PipelineData.Lock))
      {
        PerformanceMeter.Current.AddNote("Lock Records = " + (object) dataSet1.Tables[index14].Rows.Count);
        if (dataSet1.Tables[0].Rows.Count <= 0 && dataSet1.Tables[index14].Rows.Count > 0)
          throw new DataAccessViolationException("User does not have access rights to the loan.");
        dataSet1.Relations.Add("Lock", dataSet1.Tables[0].Columns["Guid"], dataSet1.Tables[index14].Columns["guid"]);
        ++index14;
      }
      if (bitmask1.Contains((object) PipelineData.AssignedRights) || bitmask1.Contains((object) PipelineData.AccessRights) || bitmask1.Contains((object) PipelineData.CurrentUserAccessRightsOnly))
      {
        PerformanceMeter.Current.AddNote("Rights Records = " + (object) dataSet1.Tables[index14].Rows.Count);
        if (dataSet1.Tables[0].Rows.Count <= 0 && dataSet1.Tables[index14].Rows.Count > 0)
          throw new DataAccessViolationException("User does not have access rights to the loan.");
        dataSet1.Relations.Add("Rights", dataSet1.Tables[0].Columns["Guid"], dataSet1.Tables[index14].Columns["guid"]);
        ++index14;
      }
      if (bitmask1.Contains((object) PipelineData.Milestones))
      {
        PerformanceMeter.Current.AddNote("Milestone Records = " + (object) dataSet1.Tables[index14].Rows.Count);
        if (dataSet1.Tables[0].Rows.Count <= 0 && dataSet1.Tables[index14].Rows.Count > 0)
          throw new DataAccessViolationException("User does not have access rights to the loan.");
        dataSet1.Relations.Add("Milestones", dataSet1.Tables[0].Columns["Guid"], dataSet1.Tables[index14].Columns["guid"]);
        ++index14;
      }
      if (bitmask1.Contains((object) PipelineData.LoanAssociates))
      {
        PerformanceMeter.Current.AddNote("Loan Assoc Records = " + (object) dataSet1.Tables[index14].Rows.Count);
        if (dataSet1.Tables[0].Rows.Count <= 0 && dataSet1.Tables[index14].Rows.Count > 0)
          throw new DataAccessViolationException("User does not have access rights to the loan.");
        dataSet1.Relations.Add("LoanAssociates", dataSet1.Tables[0].Columns["guid"], dataSet1.Tables[index14].Columns["guid"]);
        ++index14;
      }
      if (bitmask1.Contains((object) PipelineData.Trade))
      {
        Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>();
        List<DataRow> dataRowList = new List<DataRow>();
        if (dataSet1.Tables[0].Columns.Contains("Trade__TradeType"))
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataSet1.Tables[0].Rows)
          {
            if (EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["Trade__TradeType"]) == 0)
            {
              if (!dictionary.ContainsKey(row["Guid"].ToString()))
                dictionary.Add(row["Guid"].ToString(), row);
            }
            else if ((parameters.TradeType == TradeType.CorrespondentTrade && EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["Trade__TradeType"]) == 4 || parameters.TradeType == TradeType.LoanTrade && (EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["Trade__TradeType"]) == 2 || EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["Trade__TradeType"]) == 3) || parameters.TradeType == TradeType.MbsPool && (EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["Trade__TradeType"]) == 2 || EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["Trade__TradeType"]) == 3)) && !dictionary.ContainsKey(row["Guid"].ToString()))
              dictionary.Add(row["Guid"].ToString(), row);
          }
        }
        else
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataSet1.Tables[0].Rows)
          {
            if (!dictionary.ContainsKey(row["Guid"].ToString()))
              dictionary.Add(row["Guid"].ToString(), row);
          }
        }
        foreach (DataRow row in (InternalDataCollectionBase) dataSet1.Tables[0].Rows)
        {
          if (dictionary.ContainsKey(row["Guid"].ToString()))
          {
            if (dictionary[row["Guid"].ToString()] != row)
              dataRowList.Add(row);
          }
          else
            dictionary.Add(row["Guid"].ToString(), row);
        }
        for (int index15 = 0; index15 < dataSet1.Tables[0].Rows.Count; ++index15)
        {
          DataRow row = dataSet1.Tables[0].Rows[index15];
          if (dataRowList.Contains(row))
            row.Delete();
        }
        dataSet1.Tables[0].AcceptChanges();
        int count = dataSet1.Tables.Count;
        if (dataSet1.Tables[count - 2] != null && dataSet1.Tables[count - 2].Columns.Contains("LoanGuid") && dataSet1.Tables[count - 2].Columns.Contains("TradeType"))
        {
          for (int index16 = 0; index16 < dataSet1.Tables[count - 2].Rows.Count; ++index16)
          {
            DataRow row = dataSet1.Tables[count - 2].Rows[index16];
            if (!dictionary.ContainsKey(row["LoanGuid"].ToString()))
              row.Delete();
            else if (parameters.TradeType == TradeType.CorrespondentTrade && EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TradeType"]) != 4 || parameters.TradeType == TradeType.LoanTrade && EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TradeType"]) != 2 && EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TradeType"]) != 3 || parameters.TradeType == TradeType.MbsPool && EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TradeType"]) != 2 && EllieMae.EMLite.DataAccess.SQL.DecodeInt(row["TradeType"]) != 3)
              row.Delete();
          }
        }
        dataSet1.Tables[1].AcceptChanges();
        if (dataSet1.Tables[count - 1] != null && dataSet1.Tables[count - 1].Columns.Contains("LoanGuid"))
        {
          for (int index17 = 0; index17 < dataSet1.Tables[count - 1].Rows.Count; ++index17)
          {
            DataRow row = dataSet1.Tables[count - 1].Rows[index17];
            if (!dictionary.ContainsKey(row["LoanGuid"].ToString()))
              row.Delete();
          }
        }
        dataSet1.Tables[1].AcceptChanges();
        PerformanceMeter.Current.AddNote("Trade Assignment Records = " + (object) dataSet1.Tables[index14].Rows.Count);
        if (dataSet1.Tables[0].Rows.Count <= 0 && dataSet1.Tables[index14].Rows.Count > 0)
          throw new DataAccessViolationException("User does not have access rights to the loan.");
        DataRelationCollection relations3 = dataSet1.Relations;
        DataColumn column5 = dataSet1.Tables[0].Columns["guid"];
        DataTableCollection tables3 = dataSet1.Tables;
        int index18 = index14;
        int index19 = index18 + 1;
        DataColumn column6 = tables3[index18].Columns["LoanGuid"];
        relations3.Add("TradeAssignment", column5, column6);
        PerformanceMeter.Current.AddNote("Trade Rejection Records = " + (object) dataSet1.Tables[index19].Rows.Count);
        if (dataSet1.Tables[0].Rows.Count <= 0 && dataSet1.Tables[index19].Rows.Count > 0)
          throw new DataAccessViolationException("User does not have access rights to the loan.");
        DataRelationCollection relations4 = dataSet1.Relations;
        DataColumn column7 = dataSet1.Tables[0].Columns["guid"];
        DataTableCollection tables4 = dataSet1.Tables;
        int index20 = index19;
        int num = index20 + 1;
        DataColumn column8 = tables4[index20].Columns["LoanGuid"];
        relations4.Add("TradeRejections", column7, column8);
      }
      int count2 = dataSet1.Tables[0].Rows.Count;
      Dictionary<string, bool> dictionary2 = new Dictionary<string, bool>();
      ArrayList arrayList3 = new ArrayList();
      for (int index21 = 0; index21 < count2; ++index21)
      {
        Hashtable info = new Hashtable();
        DataRow row = dataSet1.Tables[0].Rows[index21];
        string guid = row["Guid"].ToString();
        if (!dictionary2.ContainsKey(guid))
        {
          dictionary2[guid] = true;
          foreach (DataColumn column in (InternalDataCollectionBase) row.Table.Columns)
          {
            string criterionName = QueryEngine.ColumnNameToCriterionName(column.ColumnName);
            info[(object) criterionName] = EllieMae.EMLite.DataAccess.SQL.Decode(row[column.ColumnName]);
          }
          PipelineInfo.Alert[] alerts = (PipelineInfo.Alert[]) null;
          if (bitmask1.Contains((object) PipelineData.Alerts))
          {
            DataRow[] childRows = dataSet1.Tables[0].Rows[index21].GetChildRows("Alerts");
            alerts = new PipelineInfo.Alert[childRows.Length];
            for (int index22 = 0; index22 < childRows.Length; ++index22)
            {
              DataRow dataRow = childRows[index22];
              PipelineInfo.Alert alert = new PipelineInfo.Alert()
              {
                AlertTargetID = (string) EllieMae.EMLite.DataAccess.SQL.Decode(dataRow["UniqueID"]),
                AlertID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["AlertType"]),
                Event = string.Concat(dataRow["Event"]),
                Status = string.Concat(dataRow["status"]),
                Date = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["AlertDate"]),
                UserID = (string) EllieMae.EMLite.DataAccess.SQL.Decode(dataRow["UserID"]),
                GroupID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["GroupID"], -1),
                DisplayStatus = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["DisplayStatus"], 1),
                SnoozeStartDTTM = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["SnoozeStartDTTM"], DateTime.MinValue),
                SnoozeDuration = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["SnoozeDuration"], 0),
                LoanAlertID = string.Concat(dataRow["LoanAlertId"])
              };
              alert.UserID = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["UserID"]);
              alerts[index22] = alert;
            }
          }
          PipelineInfo.AlertSummaryInfo alertSummary = (PipelineInfo.AlertSummaryInfo) null;
          if (bitmask1.Contains((object) PipelineData.AlertSummary))
          {
            DataRow[] childRows = dataSet1.Tables[0].Rows[index21].GetChildRows("AlertSummary");
            alertSummary = childRows.Length != 0 ? new PipelineInfo.AlertSummaryInfo(EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRows[0]["AlertCount"]), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(childRows[0]["AlertDate"])) : new PipelineInfo.AlertSummaryInfo(0, DateTime.MaxValue);
          }
          PipelineInfo.Borrower[] borrowers = (PipelineInfo.Borrower[]) null;
          if (bitmask1.Contains((object) PipelineData.Borrowers))
          {
            DataRow[] childRows = dataSet1.Tables[0].Rows[index21].GetChildRows("Borrowers");
            borrowers = new PipelineInfo.Borrower[childRows.Length];
            for (int index23 = 0; index23 < childRows.Length; ++index23)
            {
              DataRow dataRow = childRows[index23];
              borrowers[index23] = new PipelineInfo.Borrower()
              {
                PairIndex = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["PairIndex"]),
                BorrowerType = EllieMae.EMLite.DataAccess.SQL.DecodeEnum<LoanBorrowerType>(dataRow["BorrowerType"]),
                FirstName = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["FirstName"]),
                LastName = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["LastName"]),
                HomePhone = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["HomePhone"]),
                WorkPhone = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["WorkPhone"]),
                CellPhone = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["CellPhone"]),
                Email = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Email"])
              };
            }
          }
          LockInfo lockInfo = (LockInfo) null;
          List<LockInfo> locks = (List<LockInfo>) null;
          if (bitmask1.Contains((object) PipelineData.Lock))
          {
            DataRow[] childRows = dataSet1.Tables[0].Rows[index21].GetChildRows("Lock");
            if (childRows.Length == 0)
            {
              lockInfo = new LockInfo(guid);
              locks = new List<LockInfo>();
            }
            else
            {
              locks = new List<LockInfo>();
              foreach (DataRow dataRow in childRows)
              {
                DataRow[] source = dataRow.Table.Select();
                locks.Add(new LockInfo(dataRow["guid"].ToString(), dataRow["lockedby"].ToString(), "", "", EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["SessionID"], (string) null), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["ServerUri"], (string) null), (LoanInfo.LockReason) dataRow["lockedfor"], (DateTime) dataRow["locktime"], (LockInfo.ExclusiveLock) (byte) dataRow["exclusive"], lockedByList: ((IEnumerable<DataRow>) source).Where<DataRow>((System.Func<DataRow, bool>) (rowElement => rowElement["guid"].ToString() == guid)).Select<DataRow, string>((System.Func<DataRow, string>) (resultRow => resultRow["lockedby"].ToString())).ToArray<string>()));
              }
              lockInfo = locks[0];
            }
          }
          Hashtable rights = (Hashtable) null;
          if (bitmask1.Contains((object) PipelineData.AssignedRights) || bitmask1.Contains((object) PipelineData.AccessRights) || bitmask1.Contains((object) PipelineData.CurrentUserAccessRightsOnly))
          {
            rights = new Hashtable();
            foreach (DataRow childRow in dataSet1.Tables[0].Rows[index21].GetChildRows("Rights"))
              rights.Add((object) childRow["userid"].ToString(), (object) (int) childRow["rights"]);
          }
          PipelineInfo.MilestoneInfo[] milestones = (PipelineInfo.MilestoneInfo[]) null;
          if (bitmask1.Contains((object) PipelineData.Milestones))
          {
            DataRow[] childRows = dataSet1.Tables[0].Rows[index21].GetChildRows("Milestones");
            milestones = new PipelineInfo.MilestoneInfo[childRows.Length];
            for (int index24 = 0; index24 < childRows.Length; ++index24)
            {
              DataRow dataRow = childRows[index24];
              string milestoneID = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["milestoneID"]);
              string milestoneName = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["milestoneName"]);
              int roleID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["RoleID"], -1);
              string associateGuid = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["associateGuid"], (string) null);
              bool finished = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["finished"]);
              bool reviewed = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["reviewed"]);
              DateTime dateStarted = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["dateStarted"]);
              DateTime dateCompleted = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["dateCompleted"]);
              int order = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["order"], 0);
              milestones[order] = new PipelineInfo.MilestoneInfo(milestoneID, milestoneName, roleID, associateGuid, finished, reviewed, order, dateStarted, dateCompleted);
            }
          }
          PipelineInfo.LoanAssociateInfo[] loanAssociates = (PipelineInfo.LoanAssociateInfo[]) null;
          if (bitmask1.Contains((object) PipelineData.LoanAssociates))
          {
            ArrayList arrayList4 = new ArrayList();
            foreach (DataRow childRow in dataSet1.Tables[0].Rows[index21].GetChildRows("LoanAssociates"))
            {
              int roleId = (int) childRow["RoleID"];
              string roleName = string.Concat(childRow["RoleName"]);
              string roleAbbrev = string.Concat(childRow["RoleAbbr"]);
              string milestoneId = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRow["milestoneID"]);
              string milestoneName = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRow["milestoneName"]);
              string userId = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRow["UserID"]);
              int groupId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRow["GroupID"], -1);
              string fname = string.Concat(childRow["first_name"]);
              string lname = string.Concat(childRow["last_name"]);
              string fullname = string.Concat(childRow["FullName"]);
              string email = string.Concat(childRow["Email"]);
              string phone = string.Concat(childRow["Phone"]);
              string fax = string.Concat(childRow["Fax"]);
              bool writeAccess = (bool) childRow["AllowWrites"];
              LoanAssociateType associateType = EllieMae.EMLite.DataAccess.SQL.DecodeEnum<LoanAssociateType>(childRow["AssociateType"]);
              string associateGuid = string.Concat(childRow["AssociateGuid"]);
              int order = 0;
              if (string.Concat(childRow["order"]) != "")
                order = (int) childRow["order"];
              if (associateType != LoanAssociateType.None)
                arrayList4.Add((object) new PipelineInfo.LoanAssociateInfo(associateGuid, associateType, userId, groupId, fname, lname, fullname, email, phone, fax, roleId, roleName, roleAbbrev, milestoneId, milestoneName, writeAccess, order));
            }
            loanAssociates = (PipelineInfo.LoanAssociateInfo[]) arrayList4.ToArray(typeof (PipelineInfo.LoanAssociateInfo));
          }
          PipelineInfo.TradeInfo assignedTrade = (PipelineInfo.TradeInfo) null;
          PipelineInfo.TradeInfo[] tradeAssignments = (PipelineInfo.TradeInfo[]) null;
          string[] rejectedInvestors = (string[]) null;
          if (bitmask1.Contains((object) PipelineData.Trade))
          {
            DataRow[] childRows3 = dataSet1.Tables[0].Rows[index21].GetChildRows("TradeAssignment");
            tradeAssignments = new PipelineInfo.TradeInfo[childRows3.Length];
            for (int index25 = 0; index25 < childRows3.Length; ++index25)
            {
              PipelineInfo.TradeInfo tradeInfo = new PipelineInfo.TradeInfo();
              tradeInfo.TradeID = EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRows3[index25]["TradeID"]);
              tradeInfo.AssignedStatus = EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRows3[index25]["AssignedStatus"]);
              tradeInfo.PendingStatus = EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRows3[index25]["PendingStatus"]);
              tradeInfo.Profit = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(childRows3[index25]["Profit"]);
              tradeInfo.CommitmentContractNumber = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRows3[index25]["CommitmentContractNumber"], "");
              tradeInfo.ProductName = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRows3[index25]["ProductName"], "");
              tradeInfo.GseCommitmentId = EllieMae.EMLite.DataAccess.SQL.DecodeInt(childRows3[index25]["GseCommitmentId"], -1);
              tradeInfo.EPPSLoanProgramName = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRows3[index25]["EPPSLoanProgramName"], "");
              tradeAssignments[index25] = tradeInfo;
              if (parameters.TradeType != TradeType.GSECommitment)
                tradeInfo.TotalPrice = EllieMae.EMLite.DataAccess.SQL.DecodeDecimal(childRows3[index25]["TotalPrice"]);
              tradeAssignments[index25] = tradeInfo;
              if (tradeInfo.Status >= 2)
                assignedTrade = tradeInfo;
            }
            DataRow[] childRows4 = dataSet1.Tables[0].Rows[index21].GetChildRows("TradeRejections");
            rejectedInvestors = new string[childRows4.Length];
            for (int index26 = 0; index26 < childRows4.Length; ++index26)
              rejectedInvestors[index26] = EllieMae.EMLite.DataAccess.SQL.DecodeString(childRows4[index26]["Investor"]);
          }
          arrayList3.Add((object) new PipelineInfo(info, borrowers, alerts, alertSummary, loanAssociates, lockInfo, locks, rights, milestones, assignedTrade, tradeAssignments, rejectedInvestors));
        }
      }
      return (PipelineInfo[]) arrayList3.ToArray(typeof (PipelineInfo));
    }

    private bool isAlertsIncluded(string query)
    {
      return query.ToLower().Contains("fn_getalertsummaryinline_opt");
    }
  }
}
