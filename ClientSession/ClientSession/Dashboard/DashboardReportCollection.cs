// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientSession.Dashboard.DashboardReportCollection
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BizLayer;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientSession.Dashboard
{
  [Serializable]
  public class DashboardReportCollection : BusinessCollectionBase
  {
    public DashboardReport this[int index]
    {
      get => (DashboardReport) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(DashboardReport dashboardReport) => this.List.Add((object) dashboardReport);

    public void Remove(DashboardReport dashboardReport)
    {
      this.List.Remove((object) dashboardReport);
    }

    public DashboardReport Find(int reportId)
    {
      foreach (object obj in (IEnumerable) this.List)
      {
        if (reportId == ((DashboardReport) obj).ReportId)
          return (DashboardReport) obj;
      }
      return (DashboardReport) null;
    }

    public bool Contains(int reportId) => this.Find(reportId) != null;

    public static DashboardReportCollection NewDashboardReportCollection()
    {
      return new DashboardReportCollection();
    }

    private DashboardReportCollection() => this.MarkAsChild();
  }
}
