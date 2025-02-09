// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientSession.Dashboard.DashboardReport
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.Dashboard;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientSession.Dashboard
{
  [Serializable]
  public class DashboardReport : BusinessBase, IDisposable
  {
    private const int MINIMUM_LAYOUT_NUMBER = 1;
    private const int MAXIMUM_LAYOUT_NUMBER = 9;
    private const string PARAMETER_SEPARATOR = "|";
    private DashboardReportInfo reportInfo;
    private string[] reportParameters = new string[0];

    public int ReportId => this.reportInfo.ReportId;

    public string ReportName
    {
      get
      {
        return this.reportInfo.DashboardTemplatePath.Substring(this.reportInfo.DashboardTemplatePath.LastIndexOf("\\") + 1);
      }
    }

    public int LayoutBlockNumber
    {
      get => this.reportInfo.LayoutBlockNumber;
      set
      {
        if (this.reportInfo.LayoutBlockNumber == value)
          return;
        this.reportInfo.LayoutBlockNumber = value;
        this.BrokenRules.Assert(nameof (LayoutBlockNumber), "Layout Block Number is out of range", this.reportInfo.LayoutBlockNumber < 1 || this.reportInfo.LayoutBlockNumber > 9);
        this.MarkDirty();
      }
    }

    public string DashboardTemplatePath
    {
      get => this.reportInfo.DashboardTemplatePath;
      set
      {
        value = value == null ? string.Empty : value.Trim();
        if (!(this.reportInfo.DashboardTemplatePath != value))
          return;
        this.reportInfo.DashboardTemplatePath = value;
        this.BrokenRules.Assert("TemplatePathRequired", "Dashboard Template Path is a required field", this.reportInfo.DashboardTemplatePath.Length < 1);
        this.BrokenRules.Assert("TemplatePathLength", "Dashboard Template Path exceeds 255 characters", this.reportInfo.DashboardTemplatePath.Length > (int) byte.MaxValue);
        this.MarkDirty();
      }
    }

    public string[] ReportParameters
    {
      get => this.reportParameters;
      set
      {
        if (value == null)
          value = new string[0];
        this.reportParameters = value;
      }
    }

    internal DashboardReportInfo GetInfo()
    {
      this.reportInfo.ParameterString = this.parameterArrayToParameterString(this.reportParameters);
      return this.reportInfo;
    }

    internal void SetInfo(DashboardReportInfo reportInfo)
    {
      if (reportInfo == null)
        return;
      this.reportInfo = reportInfo;
      this.reportParameters = this.parameterStringToParameterArray(reportInfo.ParameterString);
    }

    public override string ToString()
    {
      return string.Format("DashboardReport[{0}]", (object) this.ReportId);
    }

    public bool Equals(DashboardReport dashboardReport)
    {
      return this.ReportId.Equals(dashboardReport.ReportId);
    }

    public new static bool Equals(object objA, object objB)
    {
      return objA is DashboardReport && objB is DashboardReport && ((DashboardReport) objA).Equals((DashboardReport) objB);
    }

    public override bool Equals(object obj)
    {
      return obj is DashboardReport && this.Equals((DashboardReport) obj);
    }

    public override int GetHashCode() => this.ReportId.GetHashCode();

    public override bool IsDirty
    {
      get
      {
        return base.IsDirty || this.reportInfo.ParameterString != this.parameterArrayToParameterString(this.reportParameters);
      }
    }

    public static DashboardReport NewDashboardReport() => new DashboardReport();

    public static DashboardReport NewDashboardReport(DashboardReportInfo reportInfo)
    {
      return new DashboardReport(reportInfo);
    }

    private DashboardReport()
    {
      this.MarkNew();
      this.MarkAsChild();
      this.reportInfo = new DashboardReportInfo(0, 0, 0, string.Empty, string.Empty);
      this.BrokenRules.Assert("TemplatePathRequired", "Dashboard Template Path is a required field", this.reportInfo.DashboardTemplatePath.Length < 1);
    }

    private DashboardReport(DashboardReportInfo reportInfo)
    {
      this.MarkOld();
      this.MarkAsChild();
      this.SetInfo(reportInfo);
    }

    public void Dispose()
    {
    }

    private string[] parameterStringToParameterArray(string parameterString)
    {
      string[] parameterArray = new string[0];
      if (string.Empty != parameterString)
        parameterArray = parameterString.Split(new string[1]
        {
          "|"
        }, StringSplitOptions.None);
      return parameterArray;
    }

    private string parameterArrayToParameterString(string[] parameterArray)
    {
      string parameterString = string.Empty;
      if (parameterArray.Length != 0)
        parameterString = string.Join("|", parameterArray);
      return parameterString;
    }
  }
}
