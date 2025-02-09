// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Dashboard.BarChartDataCriteria
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Dashboard
{
  [Serializable]
  public class BarChartDataCriteria : DashboardDataCriteria
  {
    private int maxBars = 50;
    private DashboardSubsetType subsetType;
    private string timeFrameField = "Loan.DateFileOpened";
    private string xAxisField = "Loan.CurrentMilestone";
    private string yAxisField = "Dashboard.LoanCount";
    private ColumnSummaryType yAxisSummaryType = ColumnSummaryType.Count;
    private DashboardTimeFrameType timeFrameType = DashboardTimeFrameType.None;

    public int MaxBars
    {
      get => this.maxBars;
      set => this.maxBars = value;
    }

    public DashboardSubsetType SubsetType
    {
      get => this.subsetType;
      set => this.subsetType = value;
    }

    public string TimeFrameField
    {
      get => this.timeFrameField;
      set => this.timeFrameField = value;
    }

    public string XAxisField
    {
      get => this.xAxisField;
      set => this.xAxisField = value;
    }

    public string YAxisField
    {
      get => this.yAxisField;
      set => this.yAxisField = value;
    }

    public ColumnSummaryType YAxisSummaryType
    {
      get => this.yAxisSummaryType;
      set => this.yAxisSummaryType = value;
    }

    public DashboardTimeFrameType TimeFrameType
    {
      get => this.timeFrameType;
      set => this.timeFrameType = value;
    }
  }
}
