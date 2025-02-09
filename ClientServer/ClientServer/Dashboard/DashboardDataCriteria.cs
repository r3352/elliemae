// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Dashboard.DashboardDataCriteria
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Dashboard
{
  [Serializable]
  public class DashboardDataCriteria
  {
    protected UserInfo currentUser;
    protected DashboardViewFilterType viewFilterType = DashboardViewFilterType.None;
    protected int viewFilterRoleId;
    protected string viewFilterUserInRole = string.Empty;
    protected int viewFilterOrganizationId;
    protected bool viewFilterIncludeChildren;
    protected bool viewTPOFilterIncludeChildren;
    protected int viewFilterUserGroupId;
    protected string viewFilterTPOOrgId = "0";
    protected DashboardDataSourceType dataSourceType = DashboardDataSourceType.None;
    protected DashboardChartType chartType = DashboardChartType.None;
    protected List<string> folderNames = new List<string>();
    protected FieldFilterList fieldFilters = new FieldFilterList();
    protected QueryCriterion queryCriteria;

    [CLSCompliant(false)]
    public UserInfo CurrentUser
    {
      get => this.currentUser;
      set => this.currentUser = value;
    }

    [CLSCompliant(false)]
    public DashboardViewFilterType ViewFilterType
    {
      get => this.viewFilterType;
      set => this.viewFilterType = value;
    }

    [CLSCompliant(false)]
    public int ViewFilterRoleId
    {
      get => this.viewFilterRoleId;
      set => this.viewFilterRoleId = value;
    }

    [CLSCompliant(false)]
    public string ViewFilterUserInRole
    {
      get => this.viewFilterUserInRole;
      set => this.viewFilterUserInRole = value;
    }

    [CLSCompliant(false)]
    public int ViewFilterOrganizationId
    {
      get => this.viewFilterOrganizationId;
      set => this.viewFilterOrganizationId = value;
    }

    [CLSCompliant(false)]
    public bool ViewFilterIncludeChildren
    {
      get => this.viewFilterIncludeChildren;
      set => this.viewFilterIncludeChildren = value;
    }

    [CLSCompliant(false)]
    public bool ViewTPOFilterIncludeChildren
    {
      get => this.viewTPOFilterIncludeChildren;
      set => this.viewTPOFilterIncludeChildren = value;
    }

    [CLSCompliant(false)]
    public int ViewFilterUserGroupId
    {
      get => this.viewFilterUserGroupId;
      set => this.viewFilterUserGroupId = value;
    }

    [CLSCompliant(false)]
    public string ViewFilterTPOOrgId
    {
      get => this.viewFilterTPOOrgId;
      set => this.viewFilterTPOOrgId = value;
    }

    [CLSCompliant(false)]
    public DashboardDataSourceType DataSourceType
    {
      get => this.dataSourceType;
      set => this.dataSourceType = value;
    }

    [CLSCompliant(false)]
    public DashboardChartType ChartType
    {
      get => this.chartType;
      set => this.chartType = value;
    }

    [CLSCompliant(false)]
    public List<string> FolderNames
    {
      get => this.folderNames;
      set => this.folderNames = value;
    }

    [CLSCompliant(false)]
    public FieldFilterList FieldFilters
    {
      get => this.fieldFilters;
      set => this.fieldFilters = value;
    }

    public void ClearQueryCriteria() => this.queryCriteria = (QueryCriterion) null;

    public void AddQueryCriteria(QueryCriterion newQueryCriteria)
    {
      if (newQueryCriteria == null)
        return;
      if (this.queryCriteria != null)
        this.queryCriteria = this.queryCriteria.And(newQueryCriteria);
      else
        this.queryCriteria = newQueryCriteria;
    }

    public void ClearViewCriteria()
    {
      this.viewFilterType = DashboardViewFilterType.None;
      this.viewFilterRoleId = 0;
      this.viewFilterUserInRole = string.Empty;
      this.viewFilterOrganizationId = 0;
      this.viewFilterIncludeChildren = false;
      this.viewTPOFilterIncludeChildren = false;
      this.viewFilterUserGroupId = 0;
      this.viewFilterTPOOrgId = "0";
    }

    public QueryCriterion GetReportCritera()
    {
      QueryCriterion queryCriteria = this.queryCriteria;
      this.addTemplateCriteria(ref queryCriteria);
      this.addFolderCriteria(ref queryCriteria);
      this.addFieldFilterCriteria(ref queryCriteria);
      return queryCriteria;
    }

    protected virtual void addTemplateCriteria(ref QueryCriterion queryCriteria)
    {
    }

    private void addFolderCriteria(ref QueryCriterion queryCriteria)
    {
      if (this.folderNames == null || this.folderNames.Count == 0)
        return;
      QueryCriterion criterion = (QueryCriterion) new ListValueCriterion("Loan.LoanFolder", (Array) this.folderNames.ToArray());
      if (queryCriteria != null)
        queryCriteria = queryCriteria.And(criterion);
      else
        queryCriteria = criterion;
    }

    private void addFieldFilterCriteria(ref QueryCriterion queryCriteria)
    {
      QueryCriterion queryCriterion = this.fieldFilters.CreateEvaluator().ToQueryCriterion();
      if (queryCriterion == null)
        return;
      if (queryCriteria != null)
        queryCriteria = queryCriteria.And(queryCriterion);
      else
        queryCriteria = queryCriterion;
    }
  }
}
