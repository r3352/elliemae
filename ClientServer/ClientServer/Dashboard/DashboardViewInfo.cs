// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Dashboard.DashboardViewInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.BizLayer;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Dashboard
{
  [Serializable]
  public class DashboardViewInfo : BusinessInfoBase
  {
    public int ViewId;
    public string UserId = string.Empty;
    public string ViewName = string.Empty;
    public DashboardViewFilterType ViewFilterType = DashboardViewFilterType.None;
    public int ViewFilterRoleId;
    public string ViewFilterUserInRole = string.Empty;
    public int ViewFilterOrganizationId;
    public bool ViewFilterIncludeChildren;
    public int ViewFilterUserGroupId;
    public string ViewFilterTPOOrgId = "0";
    public bool ViewTPOFilterIncludeChildren;
    public string TemplatePath = string.Empty;
    public bool IsFolder;
    public int LayoutId;
    public Guid Guid = Guid.NewGuid();
    [NotUndoable]
    public DashboardReportInfo[] DashboardReportInfos;
    [NotUndoable]
    public bool IsNew;
    [NotUndoable]
    public bool IsDirty;
    [NotUndoable]
    public bool IsDeleted;

    public DashboardViewInfo()
    {
    }

    public DashboardViewInfo(
      int viewId,
      string userId,
      string viewName,
      DashboardViewFilterType viewFilterType,
      int viewFilterRoleId,
      string viewFilterUserInRole,
      int viewFilterOrganizationId,
      bool viewFilterIncludeChildren,
      int viewFilterUserGroupId,
      string viewFilterTPOOrgId,
      bool viewTPOFilterIncludeChildren,
      int layoutId,
      string templatePath,
      bool isFolder,
      DashboardReportInfo[] dashboardReportInfos,
      Guid guid)
    {
      this.ViewId = viewId;
      this.UserId = userId;
      this.ViewName = viewName;
      this.ViewFilterType = viewFilterType;
      this.ViewFilterRoleId = viewFilterRoleId;
      this.ViewFilterUserInRole = viewFilterUserInRole;
      this.ViewFilterOrganizationId = viewFilterOrganizationId;
      this.ViewFilterIncludeChildren = viewFilterIncludeChildren;
      this.ViewFilterUserGroupId = viewFilterUserGroupId;
      this.ViewFilterTPOOrgId = viewFilterTPOOrgId;
      this.ViewTPOFilterIncludeChildren = viewTPOFilterIncludeChildren;
      this.LayoutId = layoutId;
      this.DashboardReportInfos = dashboardReportInfos;
      this.Guid = guid;
      this.TemplatePath = templatePath;
      this.IsFolder = isFolder;
    }
  }
}
