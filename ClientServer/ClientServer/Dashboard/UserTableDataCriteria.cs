// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Dashboard.UserTableDataCriteria
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Dashboard
{
  [Serializable]
  public class UserTableDataCriteria : DashboardDataCriteria
  {
    private string timeFrameField = string.Empty;
    private int roleId;
    private int organizationId;
    private bool includeChildren;
    private int userGroupId;
    private string groupByField = string.Empty;
    private string summaryField1 = string.Empty;
    private string summaryField2 = string.Empty;
    private string summaryField3 = string.Empty;
    private ColumnSummaryType summaryType1;
    private ColumnSummaryType summaryType2;
    private ColumnSummaryType summaryType3;
    private DashboardTimeFrameType timeFrameType = DashboardTimeFrameType.None;

    public string TimeFrameField
    {
      get => this.timeFrameField;
      set => this.timeFrameField = value;
    }

    public int RoleId
    {
      get => this.roleId;
      set => this.roleId = value;
    }

    public int OrganizationId
    {
      get => this.organizationId;
      set => this.organizationId = value;
    }

    public bool IncludeChildren
    {
      get => this.includeChildren;
      set => this.includeChildren = value;
    }

    public int UserGroupId
    {
      get => this.userGroupId;
      set => this.userGroupId = value;
    }

    public string GroupByField
    {
      get => this.groupByField;
      set => this.groupByField = value;
    }

    public string SummaryField1
    {
      get => this.summaryField1;
      set => this.summaryField1 = value;
    }

    public string SummaryField2
    {
      get => this.summaryField2;
      set => this.summaryField2 = value;
    }

    public string SummaryField3
    {
      get => this.summaryField3;
      set => this.summaryField3 = value;
    }

    public ColumnSummaryType SummaryType1
    {
      get => this.summaryType1;
      set => this.summaryType1 = value;
    }

    public ColumnSummaryType SummaryType2
    {
      get => this.summaryType2;
      set => this.summaryType2 = value;
    }

    public ColumnSummaryType SummaryType3
    {
      get => this.summaryType3;
      set => this.summaryType3 = value;
    }

    public DashboardTimeFrameType TimeFrameType
    {
      get => this.timeFrameType;
      set => this.timeFrameType = value;
    }
  }
}
