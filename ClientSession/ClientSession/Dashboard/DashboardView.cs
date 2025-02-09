// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientSession.Dashboard.DashboardView
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
  public class DashboardView : BusinessBase, IDisposable
  {
    private DashboardViewInfo viewInfo;
    private DashboardLayout dashboardLayout;
    private bool isReadOnly = true;
    private DashboardReportCollection reportCollection = DashboardReportCollection.NewDashboardReportCollection();
    private Sessions.Session session;

    public int ViewId => this.viewInfo.ViewId;

    public string UserId
    {
      get => this.viewInfo.UserId;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.viewInfo.UserId != value))
          return;
        this.viewInfo.UserId = value;
        this.MarkDirty();
      }
    }

    public string Guid
    {
      get => this.viewInfo.Guid.ToString();
      set => this.viewInfo.Guid = new System.Guid(value);
    }

    public bool IsReadOnly
    {
      get => this.isReadOnly;
      set => this.isReadOnly = value;
    }

    public string ViewName
    {
      get => this.viewInfo.ViewName;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.viewInfo.ViewName != value))
          return;
        this.viewInfo.ViewName = value;
        this.BrokenRules.Assert("ViewNameRequired", "View Name is a required field", this.viewInfo.ViewName.Length < 1);
        this.BrokenRules.Assert("ViewNameLength", "View Name exceeds 50 characters", this.viewInfo.ViewName.Length > 50);
        this.MarkDirty();
      }
    }

    public bool IsViewReadWrite => !this.isReadOnly;

    public bool IsViewReadOnly => false;

    public DashboardViewFilterType ViewFilterType
    {
      get => this.viewInfo.ViewFilterType;
      set
      {
        if (this.viewInfo.ViewFilterType == value)
          return;
        this.viewInfo.ViewFilterType = value;
        this.MarkDirty();
      }
    }

    public int ViewFilterRoleId
    {
      get => this.viewInfo.ViewFilterRoleId;
      set
      {
        if (this.viewInfo.ViewFilterRoleId == value)
          return;
        this.viewInfo.ViewFilterRoleId = value;
        this.MarkDirty();
      }
    }

    public string ViewFilterUserInRole
    {
      get => this.viewInfo.ViewFilterUserInRole;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.viewInfo.ViewFilterUserInRole != value))
          return;
        this.viewInfo.ViewFilterUserInRole = value;
        this.MarkDirty();
      }
    }

    public int ViewFilterOrganizationId
    {
      get => this.viewInfo.ViewFilterOrganizationId;
      set
      {
        if (this.viewInfo.ViewFilterOrganizationId == value)
          return;
        this.viewInfo.ViewFilterOrganizationId = value;
        this.MarkDirty();
      }
    }

    public bool ViewFilterIncludeChildren
    {
      get => this.viewInfo.ViewFilterIncludeChildren;
      set
      {
        if (this.viewInfo.ViewFilterIncludeChildren == value)
          return;
        this.viewInfo.ViewFilterIncludeChildren = value;
        this.MarkDirty();
      }
    }

    public int ViewFilterUserGroupId
    {
      get => this.viewInfo.ViewFilterUserGroupId;
      set
      {
        if (this.viewInfo.ViewFilterUserGroupId == value)
          return;
        this.viewInfo.ViewFilterUserGroupId = value;
        this.MarkDirty();
      }
    }

    public string ViewFilterTPOOrgId
    {
      get => this.viewInfo.ViewFilterTPOOrgId;
      set
      {
        if (!(this.viewInfo.ViewFilterTPOOrgId != value))
          return;
        this.viewInfo.ViewFilterTPOOrgId = value;
        this.MarkDirty();
      }
    }

    public bool ViewTPOFilterIncludeChildren
    {
      get => this.viewInfo.ViewTPOFilterIncludeChildren;
      set
      {
        if (this.viewInfo.ViewTPOFilterIncludeChildren == value)
          return;
        this.viewInfo.ViewTPOFilterIncludeChildren = value;
        this.MarkDirty();
      }
    }

    public int LayoutId => this.dashboardLayout.LayoutId;

    public int LayoutBlockCount => this.dashboardLayout.LayoutBlocks.Length;

    public DashboardLayout DashboardLayout
    {
      get => this.dashboardLayout;
      set
      {
        if (this.viewInfo.LayoutId == value.LayoutId)
          return;
        this.dashboardLayout = value;
        this.viewInfo.LayoutId = this.dashboardLayout.LayoutId;
        this.MarkDirty();
      }
    }

    public DashboardReportCollection ReportCollection => this.reportCollection;

    public string TemplatePath
    {
      get => this.viewInfo.TemplatePath;
      set
      {
        if (!(this.viewInfo.TemplatePath != value))
          return;
        this.viewInfo.TemplatePath = value;
        this.MarkDirty();
      }
    }

    public bool IsFolder
    {
      get => this.viewInfo.IsFolder;
      set
      {
        if (this.viewInfo.IsFolder == value)
          return;
        this.viewInfo.IsFolder = value;
        this.MarkDirty();
      }
    }

    public bool ReplaceDashboardReport(int blockNumber, DashboardReport report)
    {
      bool flag = true;
      if (this.reportCollection.Count < blockNumber)
        flag = false;
      else
        this.reportCollection[blockNumber] = report;
      if (flag)
        this.MarkDirty();
      return flag;
    }

    internal DashboardViewInfo GetInfo()
    {
      this.viewInfo.IsNew = this.IsNew;
      this.viewInfo.IsDirty = this.IsDirty;
      this.viewInfo.IsDeleted = this.IsDeleted;
      return this.viewInfo;
    }

    internal void SetInfo(DashboardViewInfo viewInfo)
    {
      if (viewInfo == null)
        return;
      this.viewInfo = viewInfo;
      this.dashboardLayout = DashboardLayoutCollection.GetDashboardLayoutCollection().Find(viewInfo.LayoutId);
      this.reportCollection = DashboardReportCollection.NewDashboardReportCollection();
      if (viewInfo.DashboardReportInfos == null)
        return;
      foreach (DashboardReportInfo dashboardReportInfo in viewInfo.DashboardReportInfos)
        this.reportCollection.Add(DashboardReport.NewDashboardReport(dashboardReportInfo));
    }

    public override string ToString() => string.Format("DashboardView[{0}]", (object) this.ViewId);

    public Sessions.Session SessionObject
    {
      get => this.session;
      set => this.session = value;
    }

    public bool Equals(DashboardView dashboardView) => this.ViewId.Equals(dashboardView.ViewId);

    public new static bool Equals(object objA, object objB)
    {
      return objA is DashboardView && objB is DashboardView && ((DashboardView) objA).Equals((DashboardView) objB);
    }

    public override bool Equals(object obj)
    {
      return obj is DashboardView && this.Equals((DashboardView) obj);
    }

    public override int GetHashCode() => this.ViewId.GetHashCode();

    public override bool IsValid
    {
      get
      {
        this.BrokenRules.Assert("ReportCount", "Report count must match layout block count", this.LayoutBlockCount != this.reportCollection.Count);
        if (!base.IsValid)
          return false;
        return this.reportCollection == null || this.reportCollection.IsValid;
      }
    }

    public override bool IsDirty
    {
      get
      {
        if (base.IsDirty)
          return true;
        return this.reportCollection != null && this.reportCollection.IsDirty;
      }
    }

    public override BusinessBase Save()
    {
      if (this.IsDeleted)
      {
        if (!this.IsNew)
          this.session.ReportManager.DeleteDashboardView(this.ViewId);
        this.MarkNew();
      }
      else if (this.IsDirty)
      {
        this.viewInfo.IsNew = this.IsNew;
        this.viewInfo.IsDirty = this.IsDirty;
        this.viewInfo.IsDeleted = this.IsDeleted;
        int num = 0;
        this.viewInfo.DashboardReportInfos = new DashboardReportInfo[this.ReportCollection.Count];
        foreach (DashboardReport report in (CollectionBase) this.ReportCollection)
          this.viewInfo.DashboardReportInfos[num++] = report.GetInfo();
        this.SetInfo(this.session.ReportManager.SaveDashboardView(this.viewInfo));
        this.MarkOld();
      }
      return (BusinessBase) this;
    }

    public BusinessBase Restore()
    {
      this.SetInfo(this.session.ReportManager.GetDashboardView(this.ViewId));
      this.MarkOld();
      return (BusinessBase) this;
    }

    public static DashboardView NewDashboardView()
    {
      return DashboardView.NewDashboardView(Session.DefaultInstance);
    }

    public static DashboardView NewDashboardView(Sessions.Session session)
    {
      return new DashboardView(session);
    }

    public static DashboardView NewDashboardView(DashboardViewInfo viewInfo)
    {
      return new DashboardView(viewInfo);
    }

    public static DashboardView GetDashboardView(int viewId)
    {
      return DashboardView.GetDashboardView(viewId, Session.DefaultInstance);
    }

    public static DashboardView GetDashboardView(int viewId, Sessions.Session session)
    {
      DashboardViewInfo dashboardView = session.ReportManager.GetDashboardView(viewId);
      return dashboardView == null ? (DashboardView) null : new DashboardView(dashboardView);
    }

    public static DashboardView GetDashboardView(string viewGuid)
    {
      return DashboardView.GetDashboardView(viewGuid, Session.DefaultInstance);
    }

    public static DashboardView GetDashboardView(string viewGuid, Sessions.Session session)
    {
      DashboardViewInfo dashboardView = session.ReportManager.GetDashboardView(viewGuid);
      return dashboardView == null ? (DashboardView) null : new DashboardView(dashboardView);
    }

    public static void DeleteDashboardView(int viewId, Sessions.Session session)
    {
      if (0 >= viewId)
        return;
      session.ReportManager.DeleteDashboardView(viewId);
    }

    public static DashboardView DuplicateDashboardView(
      string newViewName,
      DashboardView viewToDuplicate)
    {
      return new DashboardView(newViewName, viewToDuplicate);
    }

    private DashboardView(Sessions.Session session)
    {
      this.session = session;
      this.MarkNew();
      this.dashboardLayout = DashboardLayout.GetDefaultDashboardLayout();
      this.viewInfo = new DashboardViewInfo(0, string.Empty, string.Empty, DashboardViewFilterType.None, 0, string.Empty, 0, false, 0, "0", false, this.dashboardLayout.LayoutId, string.Empty, false, (DashboardReportInfo[]) null, System.Guid.NewGuid());
      this.BrokenRules.Assert("ViewNameRequired", "View Name is a required field", this.viewInfo.ViewName.Length < 1);
    }

    private DashboardView(string newViewName, DashboardView viewToDuplicate)
    {
      this.MarkNew();
      DashboardViewInfo info = viewToDuplicate.GetInfo();
      int length = info.DashboardReportInfos.Length;
      DashboardReportInfo[] dashboardReportInfoArray = new DashboardReportInfo[length];
      Array.Copy((Array) info.DashboardReportInfos, (Array) dashboardReportInfoArray, length);
      this.SetInfo(new DashboardViewInfo(0, viewToDuplicate.UserId, newViewName, viewToDuplicate.ViewFilterType, viewToDuplicate.ViewFilterRoleId, viewToDuplicate.ViewFilterUserInRole, viewToDuplicate.ViewFilterOrganizationId, viewToDuplicate.ViewFilterIncludeChildren, viewToDuplicate.ViewFilterUserGroupId, viewToDuplicate.ViewFilterTPOOrgId, viewToDuplicate.ViewTPOFilterIncludeChildren, viewToDuplicate.LayoutId, viewToDuplicate.TemplatePath, viewToDuplicate.IsFolder, dashboardReportInfoArray, System.Guid.NewGuid()));
    }

    private DashboardView(DashboardViewInfo viewInfo)
    {
      this.MarkOld();
      this.SetInfo(viewInfo);
    }

    public void Dispose()
    {
    }
  }
}
