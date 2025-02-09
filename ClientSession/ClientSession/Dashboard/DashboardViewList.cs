// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientSession.Dashboard.DashboardViewList
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.ClientSession.Dashboard
{
  [Serializable]
  public class DashboardViewList : BindingList<DashboardView>
  {
    public DashboardView Find(int viewId)
    {
      foreach (DashboardView dashboardView in (IEnumerable<DashboardView>) this.Items)
      {
        if (viewId == dashboardView.ViewId)
          return dashboardView;
      }
      return (DashboardView) null;
    }

    public bool Contains(int viewId) => this.Find(viewId) != null;

    public bool IsTemplateReferenced(string templatePath)
    {
      foreach (DashboardView dashboardView in (IEnumerable<DashboardView>) this.Items)
      {
        foreach (DashboardReport report in (CollectionBase) dashboardView.ReportCollection)
        {
          if (string.Equals(report.DashboardTemplatePath, templatePath, StringComparison.OrdinalIgnoreCase))
            return true;
        }
      }
      return Session.ReportManager.IsTemplateReferenced(templatePath);
    }

    public static DashboardViewList NewDashboardViewList() => new DashboardViewList();

    public static DashboardViewList GetDashboardViewList(DashboardViewCollectionCriteria criteria)
    {
      DashboardViewInfo[] dashboardViews = Session.ReportManager.GetDashboardViews(criteria);
      DashboardViewList dashboardViewList = new DashboardViewList();
      if (dashboardViews != null)
      {
        foreach (DashboardViewInfo viewInfo in dashboardViews)
          dashboardViewList.Add(DashboardView.NewDashboardView(viewInfo));
      }
      return dashboardViewList;
    }

    private DashboardViewList()
    {
      this.AllowNew = true;
      this.AllowRemove = true;
    }
  }
}
