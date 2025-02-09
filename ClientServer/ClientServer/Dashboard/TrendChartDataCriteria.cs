// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Dashboard.TrendChartDataCriteria
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Dashboard
{
  [Serializable]
  public class TrendChartDataCriteria : DashboardDataCriteria
  {
    private int maxLines = 10;
    private DashboardSubsetType subsetType;
    private string xAxisField = "Loan.DateFileStarted";
    private string yAxisField = "Dashboard.LoanCount";
    private ColumnSummaryType yAxisSummaryType = ColumnSummaryType.Count;
    private string groupByField = "Dashboard.NoGroupBy";
    private DashboardTimePeriodType timePeriodType = DashboardTimePeriodType.None;
    private DashboardTimeUnitType timeUnitType = DashboardTimeUnitType.None;
    private int timePeriodCount;
    private TrendChartDataCriteria.TimePeriodRange[] timePeriods = new TrendChartDataCriteria.TimePeriodRange[0];

    public int MaxLines
    {
      get => this.maxLines;
      set => this.maxLines = value;
    }

    public DashboardSubsetType SubsetType
    {
      get => this.subsetType;
      set => this.subsetType = value;
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

    public string GroupByField
    {
      get => this.groupByField;
      set => this.groupByField = value;
    }

    public DashboardTimePeriodType TimePeriodType
    {
      get => this.timePeriodType;
      set => this.timePeriodType = value;
    }

    public DashboardTimeUnitType TimeUnitType
    {
      get => this.timeUnitType;
      set => this.timeUnitType = value;
    }

    public int TimePeriodCount
    {
      get => this.timePeriodCount;
      set => this.timePeriodCount = value;
    }

    public TrendChartDataCriteria.TimePeriodRange[] TimePeriods
    {
      get => this.timePeriods;
      set => this.timePeriods = value;
    }

    [Serializable]
    public class TimePeriodRange
    {
      private DateTime startDate = DateTime.MinValue;
      private DateTime endDate = DateTime.MaxValue;

      public DateTime StartDate => this.startDate;

      public DateTime EndDate => this.endDate;

      public TimePeriodRange(DateTime startDate, DateTime endDate)
      {
        this.startDate = startDate;
        this.endDate = endDate;
      }
    }
  }
}
