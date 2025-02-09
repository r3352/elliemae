// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientSession.Dashboard.DashboardViewCollection
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientSession.Dashboard
{
  [Serializable]
  public class DashboardViewCollection : BusinessCollectionBase
  {
    public DashboardView this[int index] => (DashboardView) this.List[index];

    public void Add(DashboardView dashboardView) => this.List.Add((object) dashboardView);

    public void Remove(DashboardView dashboardView) => this.List.Remove((object) dashboardView);

    public DashboardView Find(int viewId)
    {
      foreach (object obj in (IEnumerable) this.List)
      {
        if (viewId == ((DashboardView) obj).ViewId)
          return (DashboardView) obj;
      }
      return (DashboardView) null;
    }

    public bool Contains(int viewId) => this.Find(viewId) != null;

    public static DashboardViewCollection NewDashboardViewCollection()
    {
      return new DashboardViewCollection();
    }

    public static DashboardViewCollection GetDashboardViewCollection(
      DashboardViewCollectionCriteria criteria)
    {
      DashboardViewInfo[] dashboardViews = Session.ReportManager.GetDashboardViews(criteria);
      DashboardViewCollection dashboardViewCollection = new DashboardViewCollection();
      if (dashboardViews != null)
      {
        foreach (DashboardViewInfo viewInfo in dashboardViews)
          dashboardViewCollection.Add(DashboardView.NewDashboardView(viewInfo));
      }
      return dashboardViewCollection;
    }

    private DashboardViewCollection()
    {
    }
  }
}
