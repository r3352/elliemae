// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.LoanQuery
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.Query
{
  public class LoanQuery : EllieMae.EMLite.ReportingDbUtils.Query.LoanQuery
  {
    private LoanInfo.Right accessRight = LoanInfo.Right.Read;
    private bool optimizeForSmallResultSet;
    private string loanFolder;
    private string[] loanFolders;
    private string forcedAlertQueryMethod;
    private LoanQuery.UseAlertOptimization useAlertOptimization;

    public LoanQuery(UserInfo currentUser)
      : this(currentUser, false, LoanInfo.Right.Read)
    {
    }

    public LoanQuery(UserInfo currentUser, bool useReadReplica)
      : this(currentUser, useReadReplica, LoanInfo.Right.Read)
    {
    }

    public LoanQuery(bool useERDB, UserInfo currentUser)
      : this(useERDB, currentUser, LoanInfo.Right.Read)
    {
    }

    public LoanQuery(UserInfo user, bool useReadReplica, LoanInfo.Right accessRight)
      : this(user, useReadReplica, accessRight, (ICriterionTranslator) new LoanFieldTranslator())
    {
    }

    public LoanQuery(bool useERDB, UserInfo user, LoanInfo.Right accessRight)
      : this(useERDB, false, user, accessRight, (ICriterionTranslator) new LoanFieldTranslator(useERDB))
    {
    }

    public LoanQuery(
      UserInfo user,
      bool useReadReplica,
      LoanInfo.Right accessRight,
      ICriterionTranslator fieldTranslator)
      : this(false, useReadReplica, user, accessRight, fieldTranslator)
    {
    }

    public LoanQuery(
      UserInfo user,
      bool useReadReplica,
      LoanInfo.Right accessRight,
      ICriterionTranslator fieldTranslator,
      string[] loanFolders)
      : this(false, useReadReplica, user, accessRight, fieldTranslator)
    {
      this.loanFolders = loanFolders;
    }

    public LoanQuery(
      bool useERDB,
      bool useReadReplica,
      UserInfo user,
      LoanInfo.Right accessRight,
      ICriterionTranslator fieldTranslator)
      : base(useERDB, LoanQuery.GetDbConnectionString(useERDB, useReadReplica, ClientContext.GetCurrent()), ClientContext.GetCurrent().Settings.DbServerType, user, fieldTranslator)
    {
      this.accessRight = accessRight;
      this.forcedAlertQueryMethod = ClientContext.GetCurrent().Settings.AlertQueryMethod;
      string companySetting = Company.GetCompanySetting("POLICIES", "UseAlertOptimization");
      if (string.IsNullOrEmpty(companySetting))
        this.useAlertOptimization = LoanQuery.UseAlertOptimization.LSOPT;
      else
        this.useAlertOptimization = (LoanQuery.UseAlertOptimization) Enum.Parse(typeof (LoanQuery.UseAlertOptimization), companySetting, true);
    }

    public LoanQuery(
      UserInfo user,
      LoanInfo.Right accessRight,
      ICriterionTranslator fieldTranslator,
      string loanFolder,
      bool optimizeForSmallResultSet)
      : this(user, false, accessRight, fieldTranslator)
    {
      this.optimizeForSmallResultSet = optimizeForSmallResultSet;
      this.loanFolder = loanFolder;
    }

    public LoanQuery(
      UserInfo user,
      LoanInfo.Right accessRight,
      ICriterionTranslator fieldTranslator,
      string[] loanFolders,
      bool optimizeForSmallResultSet)
      : this(user, false, accessRight, fieldTranslator)
    {
      this.optimizeForSmallResultSet = optimizeForSmallResultSet;
      this.loanFolders = loanFolders;
      if (loanFolders == null || loanFolders.Length != 1)
        return;
      this.loanFolder = loanFolders[0];
    }

    public LoanQuery(UserInfo user, string[] _loanFolders)
      : this(user, false, LoanInfo.Right.Read)
    {
      this.loanFolders = _loanFolders;
      if (this.loanFolders == null || this.loanFolders.Length != 1)
        return;
      this.loanFolder = this.loanFolders[0];
    }

    public LoanQuery(
      UserInfo user,
      LoanInfo.Right accessRight,
      ICriterionTranslator fieldTranslator,
      string[] loanFolders,
      bool optimizeForSmallResultSet,
      string[] guidList,
      bool excludeArchivedLoans)
      : this(user, accessRight, fieldTranslator, loanFolders, optimizeForSmallResultSet)
    {
      this.GuidList = guidList;
      this.ExcludeArchivedLoans = excludeArchivedLoans;
    }

    private static string GetDbConnectionString(
      bool useERDB,
      bool useAgReadReplica,
      ClientContext context)
    {
      return useERDB && context.UseERDB ? ERDBSession.getERDBConnectionString(context) : context.Settings.GetSqlConnectionString(useAgReadReplica);
    }

    public override string PrimaryKeyTableIdentifier => "LoanSummary Loan";

    public override string PrimaryKeyIdentifier => "Loan.Guid";

    public override string UserAccessQueryKeyColumnName => "Guid";

    public override FieldSource GetFieldSource(string name)
    {
      int? nullable = new int?();
      if (this.ExcludeArchivedLoans)
        nullable = new int?(0);
      if (!(name.ToLower() == "alerts"))
        return base.GetFieldSource(name);
      if (this.CurrentUser == (UserInfo) null)
        throw new Exception("Cannot retrieve alert data without a valid user");
      return this.forcedAlertQueryMethod == "table" || this.forcedAlertQueryMethod == "table_optimize" && !this.optimizeForSmallResultSet ? new FieldSource("Alerts", "left outer join FN_GetAlertSummaryTable(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.Userid) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.Now) + ") Alerts on (Alerts.LoanXRefID = Loan.XRefID)", new string[1]
      {
        "Loan"
      }) : (this.useAlertOptimization == LoanQuery.UseAlertOptimization.FilterOPT && this.loanFolder != null ? new FieldSource("Alerts", "left outer join " + this.GetAlertSummaryInlineFunction() + "(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.Userid) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.Now) + ", null, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.loanFolder) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) nullable) + ") Alerts on (Alerts.LoanXRefID = Loan.XRefID)", new string[1]
      {
        "Loan"
      }) : (this.useAlertOptimization == LoanQuery.UseAlertOptimization.FilterOPT && this.GuidList != null ? new FieldSource("Alerts", "left outer join " + this.GetAlertSummaryInlineFunction() + "(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.Userid) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.Now) + ", null ,  @loanguids ) Alerts on (Alerts.LoanXRefID = Loan.XRefID)", new string[1]
      {
        "Loan"
      }) : new FieldSource("Alerts", "left outer join " + this.GetAlertSummaryInlineFunction() + "(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.CurrentUser.Userid) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.Now) + ", null) Alerts on (Alerts.LoanXRefID = Loan.XRefID)", new string[1]
      {
        "Loan"
      })));
    }

    public string GetAlertSummaryInlineFunction()
    {
      string empty = string.Empty;
      return this.useAlertOptimization != LoanQuery.UseAlertOptimization.FilterOPT ? (this.useAlertOptimization != LoanQuery.UseAlertOptimization.LSOPT ? "FN_GetAlertSummaryInline" : "FN_GetAlertSummaryInline_Opt") : (this.loanFolder != null ? "FN_GetAlertSummaryInline_LoanFolder_OPT" : (this.GuidList != null ? "FN_GetAlertSummaryInline_LoanGuids_OPT" : "FN_GetAlertSummaryInline_Opt"));
    }

    public bool UseOptionRecompileForAlerts
    {
      get => this.useAlertOptimization == LoanQuery.UseAlertOptimization.LSOPT;
    }

    public string[] GuidList { get; }

    public bool ExcludeArchivedLoans { get; }

    public override string GetUserAccessFilterJoinClause(bool isExternalOrganization)
    {
      return Pipeline.GetUserVisibleIDQueryFilterJoinClause(this.CurrentUser, this.loanFolders, this.accessRight, isExternalOrganization, (QueryCriterion) null);
    }

    public override string GetUserAccessFilterJoinClause(
      bool isExternalOrganization,
      QueryCriterion filter)
    {
      return Pipeline.GetUserVisibleIDQueryFilterJoinClause(this.CurrentUser, this.loanFolders, this.accessRight, isExternalOrganization, filter);
    }

    public override string GetUserAccessFilterJoinClause(
      bool isExternalOrganization,
      QueryCriterion filter,
      bool isGlobalSearch)
    {
      return Pipeline.GetUserVisibleIDQueryFilterJoinClause(this.CurrentUser, this.loanFolders, this.accessRight, isExternalOrganization, filter, isGlobalSearch);
    }

    public override string GetUserAccessFilterJoinClause(
      bool isExternalOrganization,
      QueryCriterion filter,
      bool isGlobalSearch,
      bool isOptFlow)
    {
      return Pipeline.GetUserVisibleIDQueryFilterJoinClause(this.CurrentUser, this.loanFolders, this.accessRight, isExternalOrganization, filter, isGlobalSearch, isOptFlow);
    }

    public enum UseAlertOptimization
    {
      None,
      LSOPT,
      FilterOPT,
    }
  }
}
