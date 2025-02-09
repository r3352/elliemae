// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Dashboard.DashboardReportInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Dashboard
{
  [Serializable]
  public class DashboardReportInfo
  {
    public int ReportId;
    public int ViewId;
    public int LayoutBlockNumber;
    public string DashboardTemplatePath;
    public string ParameterString;

    public DashboardReportInfo()
    {
    }

    public DashboardReportInfo(
      int reportId,
      int viewId,
      int layoutBlockNumber,
      string dashboardTemplatePath,
      string parameterString)
    {
      this.ReportId = reportId;
      this.ViewId = viewId;
      this.LayoutBlockNumber = layoutBlockNumber;
      this.DashboardTemplatePath = dashboardTemplatePath;
      this.ParameterString = parameterString;
    }
  }
}
