// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.DashboardForm
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientSession.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class DashboardForm : Form, IOnlineHelpTarget
  {
    private const string className = "DashBoard";
    private const int MIN_SCREEN_RESOLUTION = 1024;
    private const string dashboardCategory = "Dashboard";
    private const string maxViewsSetting = "Dashboard.MaxRecentViews";
    private const string SECTION_DASHBOARD = "Dashboard";
    private const string DEFAULT_VIEW_ID = "Dashboard.DefaultViewId";
    private const int VIEW_FILTER_NONE = -2;
    private const int VIEW_FILTER_ORGANIZATION = -3;
    private const int VIEW_FILTER_USER_GROUP = -4;
    private const int VIEW_FILTER_TPO = -5;
    private int widthOffset = 16;
    private int heightOffset = 244;
    private int workingAreaWidth;
    private int workingAreaHeight;
    private IReportManager reportManager;
    private WorkflowManager workflowManager;
    private string defaultViewPath = "";
    private List<int> mostRecentViewIds = new List<int>();
    private DashboardLayoutCollection dashboardLayouts = DashboardLayoutCollection.GetDashboardLayoutCollection();
    private DashboardView currentView;
    private List<SnapshotBaseControl> snapshotList = new List<SnapshotBaseControl>();
    private SnapshotBaseControl zoomedSnapshot;
    private Point zoomedSnapshotLocation;
    private Size zoomedSnapshotSize;
    private bool hasPrivateRight;
    private bool hasPublicRight;
    private bool hasOrganizationRight;
    private bool hasUserGroupRight;
    private bool hasTPOContactRight;
    private DashboardViewFilterType viewFilterType = DashboardViewFilterType.None;
    private int viewFilterRoleId;
    private string viewFilterUserInRole = string.Empty;
    private OrgInfo viewFilterOrganization;
    private bool viewFilterIncludeChildren;
    private bool viewTPOFilterIncludeChildren;
    private AclGroup viewFilterUserGroup;
    private ExternalOriginatorManagementData viewFilterTPOOrg;
    private int currentViewFilterIndex = -1;
    private string viewFilterDescription = string.Empty;
    private DashboardDefaultView defaultViewMapping;
    protected DashboardTemplate defaultLoanTableTemplate;
    private FileSystemEntry currentViewFileSystemEntry;
    private bool viewModified;
    private Sessions.Session session;
    private List<ExternalOriginatorManagementData> externalOrgsList;
    private Dictionary<int, ExternalUserInfo[]> usersForExtensions = new Dictionary<int, ExternalUserInfo[]>();
    private IContainer components;
    private Panel pnlViewHeader;
    private TextBox txtViewFilter;
    private Label lblRoleSelection;
    private ComboBox cboViewFilter;
    private Button btnManageSnapshots;
    private ToolTip tipsIcons;
    private ToolTip tipDashboard;
    private ImageList imgsIcons;
    private TextBox txtCurrentViewName;
    private StandardIconButton btnViewMgr;
    private StandardIconButton btnViewFilter;
    private GradientPanel gradientPanel1;
    private Label label1;
    private GradientPanel gradientPanel2;
    private BorderPanel pnlSnapshots;
    private BorderPanel borderPanel1;
    private StandardIconButton siBtnSelectView;
    private StandardIconButton siBtnRefreshView;
    private StandardIconButton siBtnSaveView;

    public DashboardView CurrentView => this.currentView;

    public string ViewFilterDescription => this.viewFilterDescription;

    public DashboardForm(Sessions.Session session)
    {
      this.session = session;
      this.reportManager = this.session.ReportManager;
      this.workflowManager = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.externalOrgsList = Session.UserInfo.IsAdministrator() || !aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_ContactSalesRep) && aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_OrganizationSettings) ? this.session.ConfigurationManager.GetAllExternalParentOrganizations(false) : (List<ExternalOriginatorManagementData>) this.session.ConfigurationManager.GetExternalAndInternalUserAndOrgBySalesRep(Session.UserID, Session.UserInfo.OrgId)[1];
      this.InitializeComponent();
      this.initializeForm();
    }

    public bool IsMenuItemEnabled(string menuItem)
    {
      bool flag = false;
      switch (menuItem)
      {
        case "Manage Snapshots":
          flag = this.btnManageSnapshots.Enabled;
          break;
        case "Manage Dashboard Views":
          flag = this.btnViewMgr.Enabled;
          break;
      }
      return flag;
    }

    public bool IsMenuItemVisible(string menuItem)
    {
      bool flag = false;
      switch (menuItem)
      {
        case "Manage Snapshots":
          flag = this.btnManageSnapshots.Visible;
          break;
        case "Manage Dashboard Views":
          flag = this.btnViewMgr.Visible;
          break;
      }
      return flag;
    }

    public void MenuClicked(string menuItem)
    {
      switch (menuItem)
      {
        case "Manage Snapshots":
          this.btnManageSnapshots.PerformClick();
          break;
        case "Manage Dashboard Views":
          this.picEditView_Click((object) null, (EventArgs) null);
          break;
      }
    }

    public void ShowContent()
    {
      if (!this.checkSecuritySettings())
        return;
      this.setViewFilterSelection();
      if (this.currentView != null)
      {
        this.setViewFilterValues(this.IsCurrentViewEditable, this.viewFilterType, this.viewFilterRoleId, this.viewFilterUserInRole, this.viewFilterOrganization == null ? 0 : this.viewFilterOrganization.Oid, this.viewFilterIncludeChildren, (AclGroup) null == this.viewFilterUserGroup ? 0 : this.viewFilterUserGroup.ID, this.viewFilterTPOOrg == null ? "0" : this.viewFilterTPOOrg.ExternalID, this.viewTPOFilterIncludeChildren);
      }
      else
      {
        this.defaultViewPath = "";
        try
        {
          this.defaultViewPath = this.session.GetPrivateProfileString("Dashboard", "Dashboard.DefaultViewId");
        }
        catch (Exception ex)
        {
        }
        if (this.defaultViewPath != "")
        {
          string viewGuid = "";
          try
          {
            FileSystemEntry fileEntry = !this.defaultViewPath.StartsWith("Public:") ? new FileSystemEntry(this.defaultViewPath.Replace("Personal:\\" + this.session.UserID, ""), FileSystemEntry.Types.File, this.session.UserID) : new FileSystemEntry(this.defaultViewPath.Replace("Public:", ""), FileSystemEntry.Types.File, (string) null);
            if (fileEntry.IsPublic)
            {
              FileSystemEntry[] templateFileEntries = this.session.ConfigurationManager.GetFilteredTemplateFileEntries(TemplateSettingsType.DashboardViewTemplate, fileEntry.ParentFolder, false, true);
              if (templateFileEntries != null)
              {
                if (templateFileEntries.Length != 0)
                {
                  foreach (FileSystemEntry fileSystemEntry in templateFileEntries)
                  {
                    if (fileSystemEntry.ToString() == this.defaultViewPath)
                    {
                      viewGuid = string.Concat(fileSystemEntry.Properties[(object) "viewGuid"]);
                      this.currentViewFileSystemEntry = fileSystemEntry;
                      break;
                    }
                  }
                }
              }
            }
            else
            {
              this.currentViewFileSystemEntry = fileEntry;
              BinaryObject templateSettings = this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.DashboardViewTemplate, fileEntry);
              if (templateSettings != null)
                viewGuid = ((DashboardViewTemplate) templateSettings).ViewGuid;
            }
          }
          catch (Exception ex)
          {
          }
          if (viewGuid != "")
          {
            DashboardView dashboardView = DashboardView.GetDashboardView(viewGuid);
            if (dashboardView != null)
            {
              this.displayView(dashboardView);
              return;
            }
          }
        }
        this.displayView((DashboardView) null);
        int num = (int) Utils.Dialog((IWin32Window) this, "Default view cannot be found or no longer accessible to this user account.  Please select a dashboard view.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.siBtnSelectView_Click((object) null, (EventArgs) null);
      }
    }

    public bool IsViewModified
    {
      get => this.viewModified;
      set
      {
        this.viewModified = value;
        if (this.viewModified && this.IsCurrentViewEditable)
        {
          this.siBtnSaveView.Enabled = true;
          this.siBtnRefreshView.Enabled = true;
        }
        else
        {
          this.siBtnSaveView.Enabled = false;
          this.siBtnRefreshView.Enabled = false;
        }
      }
    }

    public bool IsCurrentViewEditable
    {
      get
      {
        return !this.currentViewFileSystemEntry.IsPublic || this.currentViewFileSystemEntry.Access == AclResourceAccess.ReadWrite;
      }
    }

    public void SetViewCriteria(DashboardDataCriteria reportCriteria)
    {
      reportCriteria.ClearViewCriteria();
      reportCriteria.ViewFilterType = this.viewFilterType;
      switch (this.viewFilterType)
      {
        case DashboardViewFilterType.Role:
          reportCriteria.ViewFilterRoleId = this.viewFilterRoleId;
          reportCriteria.ViewFilterUserInRole = this.viewFilterUserInRole;
          QueryCriterion newQueryCriteria1 = (QueryCriterion) new BinaryOperation(BinaryOperator.And, (QueryCriterion) new OrdinalValueCriterion("LoanAssociateUser.RoleID", (object) reportCriteria.ViewFilterRoleId), (QueryCriterion) new StringValueCriterion("LoanAssociateUser.UserID", reportCriteria.ViewFilterUserInRole));
          reportCriteria.AddQueryCriteria(newQueryCriteria1);
          break;
        case DashboardViewFilterType.Organization:
          reportCriteria.ViewFilterOrganizationId = this.viewFilterOrganization == null ? 0 : this.viewFilterOrganization.Oid;
          reportCriteria.ViewFilterIncludeChildren = this.viewFilterIncludeChildren;
          break;
        case DashboardViewFilterType.UserGroup:
          reportCriteria.ViewFilterUserGroupId = (AclGroup) null == this.viewFilterUserGroup ? 0 : this.viewFilterUserGroup.ID;
          break;
        case DashboardViewFilterType.TPO:
          reportCriteria.ViewFilterTPOOrgId = this.viewFilterTPOOrg == null || this.viewFilterTPOOrg.ExternalID == null ? "-1" : this.viewFilterTPOOrg.ExternalID;
          reportCriteria.ViewTPOFilterIncludeChildren = this.viewTPOFilterIncludeChildren;
          QueryCriterion newQueryCriteria2 = (QueryCriterion) null;
          if (!reportCriteria.ViewFilterTPOOrgId.Equals("-1"))
          {
            if (this.viewFilterTPOOrg.Parent == 0)
            {
              newQueryCriteria2 = (QueryCriterion) new StringValueCriterion("Loan.TPOCompanyID", reportCriteria.ViewFilterTPOOrgId);
            }
            else
            {
              if (this.viewFilterTPOOrg.OrganizationType == ExternalOriginatorOrgType.Branch || this.viewFilterTPOOrg.OrganizationType == ExternalOriginatorOrgType.BranchExtension)
                newQueryCriteria2 = (QueryCriterion) new StringValueCriterion("Loan.TPOBranchID", reportCriteria.ViewFilterTPOOrgId);
              else if (this.viewFilterTPOOrg.OrganizationType == ExternalOriginatorOrgType.CompanyExtension)
                newQueryCriteria2 = (QueryCriterion) new StringValueCriterion("Loan.TPOCompanyID", reportCriteria.ViewFilterTPOOrgId);
              if (this.viewFilterTPOOrg.OrganizationType == ExternalOriginatorOrgType.BranchExtension || this.viewFilterTPOOrg.OrganizationType == ExternalOriginatorOrgType.CompanyExtension)
              {
                string[] userList = this.getUserList(this.viewFilterTPOOrg.oid);
                QueryCriterion newQueryCriteria3 = new ListValueCriterion("Loan.TPOLOID", (Array) userList).Or((QueryCriterion) new ListValueCriterion("Loan.TPOLPID", (Array) userList));
                reportCriteria.AddQueryCriteria(newQueryCriteria3);
              }
            }
          }
          else
            newQueryCriteria2 = (QueryCriterion) new ListValueCriterion("Loan.TPOCompanyID", (Array) this.viewFilterTPOOrg.RootOrgBySalesRep.ToArray());
          reportCriteria.AddQueryCriteria(newQueryCriteria2);
          break;
      }
    }

    private string[] getUserList(int oid)
    {
      ExternalUserInfo[] externalUserInfoArray;
      if (this.usersForExtensions.ContainsKey(oid))
      {
        externalUserInfoArray = this.usersForExtensions[oid];
      }
      else
      {
        externalUserInfoArray = this.session.ConfigurationManager.GetAllExternalUserInfos(oid);
        this.usersForExtensions[oid] = externalUserInfoArray;
      }
      string[] userList;
      if (externalUserInfoArray.Length == 0)
      {
        userList = new string[1]{ "-1" };
      }
      else
      {
        userList = new string[externalUserInfoArray.Length];
        for (int index = 0; index < externalUserInfoArray.Length; ++index)
          userList[index] = externalUserInfoArray[index].ContactID;
      }
      return userList;
    }

    private bool checkSecuritySettings()
    {
      this.hasPublicRight = false;
      this.hasPublicRight = UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) || this.session.AclGroupManager.CheckPublicAccessPermission(AclFileType.DashboardTemplate);
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.hasPrivateRight = aclManager.GetUserApplicationRight(AclFeature.DashboardTab_ManagePersonalTemplate);
      if (this.session.LoanManager.GetLoanXDBTableList(false) == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Reporting Database is not currently available. Contact your Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      this.hasOrganizationRight = aclManager.GetUserApplicationRight(AclFeature.DashboardTab_Organization);
      this.hasUserGroupRight = aclManager.GetUserApplicationRight(AclFeature.DashboardTab_UserGroup);
      this.hasTPOContactRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_Contacts);
      return true;
    }

    private void displayView(DashboardView dashboardView)
    {
      this.zoomedSnapshot = (SnapshotBaseControl) null;
      if (dashboardView == null)
      {
        this.cboViewFilter.SelectedIndex = 0;
        this.txtCurrentViewName.Text = "";
        this.txtViewFilter.Text = "";
        this.btnViewFilter.Enabled = false;
        this.cboViewFilter.Enabled = false;
        this.pnlSnapshots.Visible = false;
        this.siBtnRefreshView.Enabled = false;
        this.siBtnSaveView.Enabled = false;
        this.currentView = (DashboardView) null;
        this.currentViewFileSystemEntry = (FileSystemEntry) null;
      }
      else
      {
        if (1024 > Screen.FromControl((Control) this).WorkingArea.Width)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, "The recommended screen resolution for the Dashboard is 1024 by 768 or higher.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.setViewFilterValues(this.IsCurrentViewEditable, dashboardView.ViewFilterType, dashboardView.ViewFilterRoleId, dashboardView.ViewFilterUserInRole, dashboardView.ViewFilterOrganizationId, dashboardView.ViewFilterIncludeChildren, dashboardView.ViewFilterUserGroupId, dashboardView.ViewFilterTPOOrgId, dashboardView.ViewTPOFilterIncludeChildren);
        this.pnlSnapshots.Visible = true;
        this.cboViewFilter.Enabled = true;
        this.setWorkingArea();
        this.currentView = dashboardView;
        this.defaultViewMapping = new DashboardDefaultView();
        try
        {
          BinaryObject dashboardSettings = this.session.User.GetUserDashboardSettings(this.currentView.Guid);
          if (dashboardSettings != null)
            this.defaultViewMapping = dashboardSettings.ToObject<DashboardDefaultView>();
        }
        catch
        {
        }
        this.displaySnapshots(this.currentView);
        this.txtCurrentViewName.Text = this.currentViewFileSystemEntry.Name;
        this.IsViewModified = false;
      }
    }

    private void setViewFilterValues(
      bool isReadWrite,
      DashboardViewFilterType filterType,
      int roleId,
      string userInRole,
      int organizationId,
      bool includeChildren,
      int userGroupId,
      string TPOOrgId,
      bool includeTPOChildren)
    {
      this.clearViewFilterValues();
      this.cboViewFilter.SelectedValue = (object) -2;
      string str = string.Empty;
      try
      {
        if (filterType == DashboardViewFilterType.Role)
        {
          str = "role and user ";
          this.cboViewFilter.SelectedValue = (object) roleId;
          if (this.cboViewFilter.SelectedItem == null)
            throw new Exception("");
          this.viewFilterType = DashboardViewFilterType.Role;
          this.viewFilterRoleId = roleId;
          this.viewFilterUserInRole = userInRole;
          UserInfo user = this.session.OrganizationManager.GetUser(userInRole);
          this.setTextBoxText(this.txtViewFilter, user.FullName);
          this.viewFilterDescription = "Role: " + this.cboViewFilter.Text + ", User: " + user.Userid + ", ";
          this.btnViewFilter.Enabled = isReadWrite;
        }
        else if (DashboardViewFilterType.Organization == filterType)
        {
          if (this.hasOrganizationRight)
          {
            str = "viewFilterOrganization ";
            this.cboViewFilter.SelectedValue = (object) -3;
            this.viewFilterType = DashboardViewFilterType.Organization;
            this.viewFilterOrganization = this.session.OrganizationManager.GetOrganization(organizationId);
            this.viewFilterIncludeChildren = includeChildren;
            string text = includeChildren ? this.viewFilterOrganization.Description + " and below" : this.viewFilterOrganization.Description;
            this.setTextBoxText(this.txtViewFilter, text);
            this.viewFilterDescription = "Organization: " + text + ", ";
            this.btnViewFilter.Enabled = isReadWrite;
          }
        }
        else if (DashboardViewFilterType.TPO == filterType)
        {
          if (this.hasTPOContactRight)
          {
            str = "viewFilterTPO ";
            this.cboViewFilter.SelectedValue = (object) -5;
            this.viewFilterType = DashboardViewFilterType.TPO;
            this.viewFilterTPOOrg = this.session.ConfigurationManager.GetExternalCompanyByTPOID(false, TPOOrgId);
            if (this.viewFilterTPOOrg == null)
            {
              this.viewFilterTPOOrg = new ExternalOriginatorManagementData();
              this.viewFilterTPOOrg.ExternalID = "-1";
              this.viewFilterTPOOrg.OrganizationName = "Third Party Originators";
              List<string> stringList = new List<string>();
              foreach (ExternalOriginatorManagementData externalOrgs in this.externalOrgsList)
                stringList.Add(externalOrgs.ExternalID);
              this.viewFilterTPOOrg.RootOrgBySalesRep = stringList;
            }
            this.viewTPOFilterIncludeChildren = includeTPOChildren;
            string text = includeTPOChildren ? this.viewFilterTPOOrg.OrganizationName + " and below" : this.viewFilterTPOOrg.OrganizationName;
            this.setTextBoxText(this.txtViewFilter, text);
            this.viewFilterDescription = "TPO Org: " + text + ", ";
            this.btnViewFilter.Enabled = isReadWrite;
          }
        }
        else if (DashboardViewFilterType.UserGroup == filterType)
        {
          if (this.hasUserGroupRight)
          {
            str = "user group ";
            this.cboViewFilter.SelectedValue = (object) -4;
            this.viewFilterType = DashboardViewFilterType.UserGroup;
            this.viewFilterUserGroup = this.session.AclGroupManager.GetGroupById(userGroupId);
            string name = this.viewFilterUserGroup.Name;
            this.setTextBoxText(this.txtViewFilter, name);
            this.viewFilterDescription = "User Group: " + name + ", ";
            this.btnViewFilter.Enabled = isReadWrite;
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The " + str + "filter criteria for this view could not be restored.\nSetup options may have changed which makes the criteria invalid.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.clearViewFilterValues();
        this.cboViewFilter.SelectedValue = (object) -2;
      }
      this.currentViewFilterIndex = this.cboViewFilter.SelectedIndex;
    }

    private void setTextBoxText(TextBox textBox, string text)
    {
      if (string.Empty == text)
      {
        textBox.Text = string.Empty;
        this.tipDashboard.SetToolTip((Control) textBox, string.Empty);
      }
      else
      {
        using (Graphics graphics = textBox.CreateGraphics())
        {
          string empty = string.Empty;
          if (this.fitText(graphics, textBox.Font, (float) textBox.Width, text, ref empty))
          {
            textBox.Text = text;
            this.tipDashboard.SetToolTip((Control) textBox, string.Empty);
          }
          else
          {
            textBox.Text = empty;
            this.tipDashboard.SetToolTip((Control) textBox, text);
          }
        }
      }
    }

    private bool fitText(
      Graphics graphics,
      Font font,
      float width,
      string textToFit,
      ref string textThatFits)
    {
      if ((double) width >= (double) graphics.MeasureString(textToFit, font).Width)
      {
        textThatFits = textToFit;
        return true;
      }
      StringBuilder stringBuilder = new StringBuilder(textToFit);
      do
      {
        --stringBuilder.Length;
      }
      while ((double) width < (double) graphics.MeasureString(stringBuilder.ToString(), font).Width);
      if (3 <= stringBuilder.Length)
      {
        stringBuilder.Length -= 3;
        stringBuilder.Append("...");
      }
      textThatFits = stringBuilder.ToString();
      return false;
    }

    private void initializeForm()
    {
      string path = string.Concat(this.session.SessionObjects.ServerManager.GetServerSetting("Dashboard.DefaultDrilldownView"));
      if (!(path != ""))
        return;
      FileSystemEntry fileSystemEntry = new FileSystemEntry(path, FileSystemEntry.Types.File, (string) null);
      try
      {
        FileSystemEntry[] templateFileEntries = this.session.ConfigurationManager.GetFilteredTemplateFileEntries(TemplateSettingsType.DashboardTemplate, fileSystemEntry.ParentFolder, false, false);
        if (templateFileEntries == null)
          return;
        foreach (FileSystemEntry fsFile in templateFileEntries)
        {
          if (fsFile.Path == path && (fsFile.Access == AclResourceAccess.ReadOnly || fsFile.Access == AclResourceAccess.ReadWrite))
            this.defaultLoanTableTemplate = new DashboardIFSExplorer().LoadDashboardTemplate(fsFile);
        }
      }
      catch (Exception ex)
      {
      }
    }

    public DashboardTemplate GetMappedSnapshot(string guid)
    {
      DashboardTemplate mappedSnapshot = (DashboardTemplate) null;
      if (this.defaultViewMapping.Contains(guid))
      {
        string mappedSnapshotPath = this.defaultViewMapping.GetMappedSnapshotPath(guid);
        if (!mappedSnapshotPath.StartsWith("Public:"))
        {
          BinaryObject templateSettings = this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.DashboardTemplate, new FileSystemEntry(mappedSnapshotPath.Replace("Personal:\\" + this.session.UserID, ""), FileSystemEntry.Types.File, this.session.UserID));
          if (templateSettings != null)
            mappedSnapshot = (DashboardTemplate) templateSettings;
        }
        else
        {
          string path = mappedSnapshotPath.Replace("Public:", "");
          FileSystemEntry fileEntry = new FileSystemEntry(path, FileSystemEntry.Types.File, (string) null);
          FileSystemEntry[] templateFileEntries = this.session.ConfigurationManager.GetFilteredTemplateFileEntries(TemplateSettingsType.DashboardTemplate, fileEntry.ParentFolder, false, true);
          if (templateFileEntries != null && templateFileEntries.Length != 0)
          {
            foreach (FileSystemEntry fileSystemEntry in templateFileEntries)
            {
              if (fileSystemEntry.Path == path)
              {
                BinaryObject templateSettings = this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.DashboardTemplate, fileEntry);
                if (templateSettings != null)
                {
                  mappedSnapshot = (DashboardTemplate) templateSettings;
                  break;
                }
                break;
              }
            }
          }
        }
      }
      if (mappedSnapshot == null)
        mappedSnapshot = this.defaultLoanTableTemplate;
      return mappedSnapshot;
    }

    public void SetSnapshotMap(string guid, string drilldownPath)
    {
      this.defaultViewMapping.AddMapping(guid, drilldownPath);
      using (BinaryObject data = new BinaryObject((IXmlSerializable) this.defaultViewMapping))
        this.session.User.SaveUserDashboardSettings(this.currentView.Guid, data);
    }

    public DashboardTemplate DefaultLoanTableTemplate => this.defaultLoanTableTemplate;

    private void refreshForm() => this.setViewFilterSelection();

    private void setViewFilterSelection()
    {
      List<RoleInfo> roleInfoList = new List<RoleInfo>();
      roleInfoList.Add(new RoleInfo(-2, "All", "AL", false, new int[0], new int[0]));
      if (this.hasOrganizationRight && this.session.EncompassEdition != EncompassEdition.Broker)
        roleInfoList.Add(new RoleInfo(-3, "Organizations", "OG", false, new int[0], new int[0]));
      if (this.hasTPOContactRight && this.session.EncompassEdition != EncompassEdition.Broker)
        roleInfoList.Add(new RoleInfo(-5, "TPO Organizations", "TPO", false, new int[0], new int[0]));
      if (this.hasUserGroupRight && this.session.EncompassEdition != EncompassEdition.Broker)
        roleInfoList.Add(new RoleInfo(-4, "User Group", "UG", false, new int[0], new int[0]));
      RoleInfo[] allRoleFunctions = this.workflowManager.GetAllRoleFunctions();
      RolesComparer rolesComparer = new RolesComparer();
      Array.Sort((Array) allRoleFunctions, (IComparer) rolesComparer);
      roleInfoList.AddRange((IEnumerable<RoleInfo>) allRoleFunctions);
      this.cboViewFilter.DataSource = (object) roleInfoList.ToArray();
      this.cboViewFilter.DisplayMember = "Name";
      this.cboViewFilter.ValueMember = "ID";
    }

    private void setWorkingArea()
    {
      Rectangle workingArea = Screen.FromControl((Control) this).WorkingArea;
      this.Size = new Size(workingArea.Width, workingArea.Height);
      this.workingAreaWidth = workingArea.Width - this.widthOffset;
      this.workingAreaHeight = workingArea.Height - this.heightOffset;
    }

    private void displaySnapshots(DashboardView dashboardView)
    {
      this.Cursor = Cursors.WaitCursor;
      this.SuspendLayout();
      foreach (Component snapshot in this.snapshotList)
        snapshot.Dispose();
      this.snapshotList.Clear();
      int index = 0;
      foreach (RectangleF layoutBlock in this.dashboardLayouts.Find(dashboardView.LayoutId).LayoutBlocks)
      {
        int x = (int) ((double) this.workingAreaWidth * (double) layoutBlock.X);
        int y = (int) ((double) this.workingAreaHeight * (double) layoutBlock.Y);
        int width = (int) ((double) this.workingAreaWidth * (double) layoutBlock.Width);
        int height = (int) ((double) this.workingAreaHeight * (double) layoutBlock.Height);
        DashboardReport report = dashboardView.ReportCollection[index];
        SnapshotBaseControl snapshot = this.createSnapshot(index++, report, new Point(x, y), new Size(width, height));
        this.snapshotList.Add(snapshot);
        this.pnlSnapshots.Controls.Add((Control) snapshot);
      }
      this.ResumeLayout();
      this.Cursor = Cursors.Default;
    }

    private SnapshotBaseControl createSnapshot(
      int snapshotHandle,
      DashboardReport dashboardReport,
      Point snapshotLocation,
      Size snapshotSize)
    {
      SnapshotBaseControl userDefinedSnapshot = this.createUserDefinedSnapshot(snapshotHandle, dashboardReport);
      if (userDefinedSnapshot != null)
      {
        userDefinedSnapshot.Location = new Point(snapshotLocation.X, snapshotLocation.Y);
        userDefinedSnapshot.Size = new Size(snapshotSize.Width, snapshotSize.Height);
        userDefinedSnapshot.AfterEditEvent += new AfterEditEventHandler(this.snapshot_AfterEditEvent);
        userDefinedSnapshot.AfterReplaceEvent += new AfterReplaceEventHandler(this.snapshot_AfterReplaceEvent);
        userDefinedSnapshot.RefreshAllEvent += new RefreshAllEventHandler(this.snapshot_RefreshAllEvent);
        userDefinedSnapshot.ZoomEvent += new ZoomEventHandler(this.snapshot_ZoomEvent);
      }
      return userDefinedSnapshot;
    }

    private SnapshotBaseControl createUserDefinedSnapshot(
      int snapshotHandle,
      DashboardReport dashboardReport)
    {
      SnapshotBaseControl userDefinedSnapshot = (SnapshotBaseControl) null;
      FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(dashboardReport.DashboardTemplatePath);
      fileSystemEntry.Access = !fileSystemEntry.IsPublic ? (this.hasPrivateRight ? AclResourceAccess.ReadWrite : AclResourceAccess.None) : this.session.AclGroupManager.GetUserFileFolderAccess(AclFileType.DashboardTemplate, fileSystemEntry);
      if (fileSystemEntry.Access == AclResourceAccess.None)
        return new SnapshotBaseControl(this, snapshotHandle, dashboardReport, "You are not authorized to view this snapshot.");
      DashboardTemplate dashboardTemplate = new DashboardIFSExplorer().LoadDashboardTemplate(fileSystemEntry);
      if (dashboardTemplate == null)
        return new SnapshotBaseControl(this, snapshotHandle, dashboardReport, "This snapshot is missing or has been deleted.");
      string errorMessage = "";
      if (!new DashboardTemplateValidator(DashboardSettings.FieldDefinitions).Validate(dashboardTemplate, out errorMessage))
        return new SnapshotBaseControl(this, snapshotHandle, dashboardReport, "This snapshot uses one or more fields which are no longer available.");
      switch (dashboardTemplate.ChartType)
      {
        case DashboardChartType.BarChart:
          userDefinedSnapshot = (SnapshotBaseControl) new UltraChartBase(this, snapshotHandle, dashboardReport, dashboardTemplate);
          break;
        case DashboardChartType.TrendChart:
          userDefinedSnapshot = (SnapshotBaseControl) new UltraChartBase(this, snapshotHandle, dashboardReport, dashboardTemplate);
          break;
        case DashboardChartType.LoanTable:
          userDefinedSnapshot = (SnapshotBaseControl) new UltraGridBase(this, snapshotHandle, dashboardReport, dashboardTemplate);
          break;
        case DashboardChartType.UserTable:
          userDefinedSnapshot = (SnapshotBaseControl) new UltraGridBase(this, snapshotHandle, dashboardReport, dashboardTemplate);
          break;
      }
      if (userDefinedSnapshot == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format("The Dashboard Snapshot '{0}' could not be created.", (object) dashboardTemplate.TemplateName), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        userDefinedSnapshot = new SnapshotBaseControl(this, snapshotHandle, dashboardReport, (DashboardTemplate) null, DashboardChartType.None);
      }
      return userDefinedSnapshot;
    }

    private bool processViewFilterSelection()
    {
      string text = this.txtViewFilter.Text;
      this.txtViewFilter.Text = string.Empty;
      if (this.cboViewFilter.SelectedItem == null)
        return false;
      string str = string.Empty;
      try
      {
        switch ((int) this.cboViewFilter.SelectedValue)
        {
          case -5:
            str = "viewFilterTPO ";
            using (TPOOrgDialog tpoOrgDialog = new TPOOrgDialog(this.externalOrgsList))
            {
              tpoOrgDialog.IncludeChildren = true;
              if (DashboardViewFilterType.TPO == this.viewFilterType && this.viewFilterTPOOrg != null)
              {
                tpoOrgDialog.SelectedTPOOrg = this.viewFilterTPOOrg;
                tpoOrgDialog.IncludeChildren = this.viewTPOFilterIncludeChildren;
              }
              if (DialogResult.OK != tpoOrgDialog.ShowDialog())
              {
                this.txtViewFilter.Text = text;
                return false;
              }
              this.clearViewFilterValues();
              this.viewFilterType = DashboardViewFilterType.TPO;
              this.viewFilterTPOOrg = tpoOrgDialog.SelectedTPOOrg;
              this.viewTPOFilterIncludeChildren = tpoOrgDialog.IncludeChildren;
              text = this.viewTPOFilterIncludeChildren ? this.viewFilterTPOOrg.OrganizationName + " and below" : this.viewFilterTPOOrg.OrganizationName;
              this.setTextBoxText(this.txtViewFilter, text);
              this.viewFilterDescription = "TPO Org: " + text + ", ";
              this.btnViewFilter.Enabled = true;
              break;
            }
          case -4:
            str = "user group ";
            using (UserGroupDialog userGroupDialog = new UserGroupDialog())
            {
              if (DashboardViewFilterType.UserGroup == this.viewFilterType && (AclGroup) null != this.viewFilterUserGroup)
                userGroupDialog.SelectedUserGroup = this.viewFilterUserGroup;
              if (DialogResult.OK != userGroupDialog.ShowDialog())
              {
                this.txtViewFilter.Text = text;
                return false;
              }
              this.clearViewFilterValues();
              this.viewFilterType = DashboardViewFilterType.UserGroup;
              this.viewFilterUserGroup = userGroupDialog.SelectedUserGroup;
              text = this.viewFilterUserGroup.Name;
              this.setTextBoxText(this.txtViewFilter, text);
              this.viewFilterDescription = "User Group: " + text + ", ";
              this.btnViewFilter.Enabled = true;
              break;
            }
          case -3:
            str = "viewFilterOrganization ";
            using (OrganizationDialog organizationDialog = new OrganizationDialog())
            {
              organizationDialog.IncludeChildren = true;
              if (DashboardViewFilterType.Organization == this.viewFilterType && this.viewFilterOrganization != null)
              {
                organizationDialog.SelectedOrganization = this.viewFilterOrganization;
                organizationDialog.IncludeChildren = this.viewFilterIncludeChildren;
              }
              if (DialogResult.OK != organizationDialog.ShowDialog())
              {
                this.txtViewFilter.Text = text;
                return false;
              }
              this.clearViewFilterValues();
              this.viewFilterType = DashboardViewFilterType.Organization;
              this.viewFilterOrganization = organizationDialog.SelectedOrganization;
              this.viewFilterIncludeChildren = organizationDialog.IncludeChildren;
              text = this.viewFilterIncludeChildren ? this.viewFilterOrganization.Description + " and below" : this.viewFilterOrganization.Description;
              this.setTextBoxText(this.txtViewFilter, text);
              this.viewFilterDescription = "Organization: " + text + ", ";
              this.btnViewFilter.Enabled = true;
              break;
            }
          case -2:
            this.clearViewFilterValues();
            break;
          default:
            str = "role and user ";
            using (ContactAssignment contactAssignment = new ContactAssignment(this.session, (RoleInfo) this.cboViewFilter.SelectedItem, string.Empty, true))
            {
              if (DialogResult.OK != contactAssignment.ShowDialog())
              {
                this.txtViewFilter.Text = text;
                return false;
              }
              this.clearViewFilterValues();
              this.viewFilterType = DashboardViewFilterType.Role;
              this.viewFilterRoleId = (int) this.cboViewFilter.SelectedValue;
              this.viewFilterUserInRole = contactAssignment.AssigneeID;
              text = contactAssignment.SelectedUser.FullName;
              this.setTextBoxText(this.txtViewFilter, text);
              this.viewFilterDescription = "Role: " + this.cboViewFilter.Text + ", User: " + contactAssignment.AssigneeID + ", ";
              this.btnViewFilter.Enabled = true;
              break;
            }
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The " + str + "filter criteria for this view could not be created.\nSetup options may have changed which makes the criteria invalid.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.txtViewFilter.Text = text;
        return false;
      }
      this.refreshAll();
      this.IsViewModified = true;
      return true;
    }

    private void clearViewFilterValues()
    {
      this.viewFilterType = DashboardViewFilterType.None;
      this.btnViewFilter.Enabled = false;
      this.setTextBoxText(this.txtViewFilter, string.Empty);
      this.viewFilterDescription = string.Empty;
      this.viewFilterRoleId = 0;
      this.viewFilterUserInRole = string.Empty;
      this.viewFilterOrganization = (OrgInfo) null;
      this.viewFilterIncludeChildren = false;
      this.viewFilterUserGroup = (AclGroup) null;
      this.viewFilterTPOOrg = (ExternalOriginatorManagementData) null;
      this.viewTPOFilterIncludeChildren = false;
    }

    private void refreshAll()
    {
      if (this.currentView != null)
        this.displaySnapshots(this.currentView);
      this.zoomedSnapshot = (SnapshotBaseControl) null;
    }

    private void saveDashboardView()
    {
      if (this.currentView == null)
        return;
      this.currentView.ViewFilterType = this.viewFilterType;
      this.currentView.ViewFilterRoleId = this.viewFilterRoleId;
      this.currentView.ViewFilterUserInRole = this.viewFilterUserInRole;
      this.currentView.ViewFilterOrganizationId = this.viewFilterOrganization == null ? 0 : this.viewFilterOrganization.Oid;
      this.currentView.ViewFilterIncludeChildren = this.viewFilterIncludeChildren;
      this.currentView.ViewFilterUserGroupId = (AclGroup) null == this.viewFilterUserGroup ? 0 : this.viewFilterUserGroup.ID;
      this.currentView.ViewFilterTPOOrgId = this.viewFilterTPOOrg == null ? "0" : this.viewFilterTPOOrg.ExternalID;
      this.currentView.ViewTPOFilterIncludeChildren = this.viewTPOFilterIncludeChildren;
      this.currentView.SessionObject = this.session;
      Cursor.Current = Cursors.WaitCursor;
      this.currentView = (DashboardView) this.currentView.Save();
      Cursor.Current = Cursors.Default;
      this.IsViewModified = false;
    }

    private void restoreDashboardView()
    {
      if (this.currentView == null)
        return;
      Cursor.Current = Cursors.WaitCursor;
      this.currentView.SessionObject = this.session;
      this.currentView = (DashboardView) this.currentView.Restore();
      Cursor.Current = Cursors.Default;
      this.displayView(this.currentView);
      this.IsViewModified = false;
    }

    private void setIconButton(PictureBox pictureBox, bool enable)
    {
      if (enable)
        pictureBox.Enabled = true;
      else
        pictureBox.Enabled = false;
    }

    public string GetHelpTargetName() => "DashBoard";

    private void snapshot_AfterEditEvent(object sender, SnapshotEventArgs e)
    {
      SnapshotBaseControl snapshot1 = this.snapshotList[e.SnapshotHandle];
      DashboardReport dashboardReport = snapshot1.DashboardReport;
      this.pnlSnapshots.Controls.Remove((Control) snapshot1);
      snapshot1.Dispose();
      RectangleF layoutBlock = this.dashboardLayouts.Find(this.currentView.LayoutId).LayoutBlocks[dashboardReport.LayoutBlockNumber - 1];
      int x = (int) ((double) this.workingAreaWidth * (double) layoutBlock.X);
      int y = (int) ((double) this.workingAreaHeight * (double) layoutBlock.Y);
      int width = (int) ((double) this.workingAreaWidth * (double) layoutBlock.Width);
      int height = (int) ((double) this.workingAreaHeight * (double) layoutBlock.Height);
      SnapshotBaseControl snapshot2 = this.createSnapshot(e.SnapshotHandle, dashboardReport, new Point(x, y), new Size(width, height));
      this.snapshotList[e.SnapshotHandle] = snapshot2;
      this.pnlSnapshots.Controls.Add((Control) snapshot2);
      if (this.zoomedSnapshot == null)
        return;
      this.zoomedSnapshot = (SnapshotBaseControl) null;
      this.snapshot_ZoomEvent(sender, e);
    }

    private void snapshot_AfterReplaceEvent(object sender, SnapshotEventArgs e)
    {
      SnapshotBaseControl snapshot1 = this.snapshotList[e.SnapshotHandle];
      DashboardReport dashboardReport = snapshot1.DashboardReport;
      dashboardReport.DashboardTemplatePath = e.EventString;
      dashboardReport.ReportParameters = new string[0];
      this.pnlSnapshots.Controls.Remove((Control) snapshot1);
      snapshot1.Dispose();
      RectangleF layoutBlock = this.dashboardLayouts.Find(this.currentView.LayoutId).LayoutBlocks[dashboardReport.LayoutBlockNumber - 1];
      int x = (int) ((double) this.workingAreaWidth * (double) layoutBlock.X);
      int y = (int) ((double) this.workingAreaHeight * (double) layoutBlock.Y);
      int width = (int) ((double) this.workingAreaWidth * (double) layoutBlock.Width);
      int height = (int) ((double) this.workingAreaHeight * (double) layoutBlock.Height);
      SnapshotBaseControl snapshot2 = this.createSnapshot(e.SnapshotHandle, dashboardReport, new Point(x, y), new Size(width, height));
      this.snapshotList[e.SnapshotHandle] = snapshot2;
      this.pnlSnapshots.Controls.Add((Control) snapshot2);
      if (this.zoomedSnapshot != null)
      {
        this.zoomedSnapshot = (SnapshotBaseControl) null;
        this.snapshot_ZoomEvent(sender, e);
      }
      this.currentView.ReplaceDashboardReport(e.SnapshotHandle, dashboardReport);
      this.IsViewModified = true;
    }

    private void snapshot_ZoomEvent(object sender, SnapshotEventArgs e)
    {
      if (1 == this.snapshotList.Count)
        return;
      this.SuspendLayout();
      SnapshotBaseControl snapshot1 = this.snapshotList[e.SnapshotHandle];
      if (this.zoomedSnapshot == null)
      {
        foreach (Control snapshot2 in this.snapshotList)
          snapshot2.Visible = false;
        this.zoomedSnapshot = snapshot1;
        Point location = snapshot1.Location;
        int x = location.X;
        location = snapshot1.Location;
        int y = location.Y;
        this.zoomedSnapshotLocation = new Point(x, y);
        Size size = snapshot1.Size;
        int width = size.Width;
        size = snapshot1.Size;
        int height = size.Height;
        this.zoomedSnapshotSize = new Size(width, height);
        snapshot1.Location = new Point(0, 0);
        snapshot1.Size = new Size(this.workingAreaWidth, this.workingAreaHeight + 60);
        snapshot1.Zoom(true);
        snapshot1.Visible = true;
      }
      else
      {
        this.pnlSnapshots.VerticalScroll.Value = 0;
        this.pnlSnapshots.HorizontalScroll.Value = 0;
        snapshot1.Location = new Point(this.zoomedSnapshotLocation.X, this.zoomedSnapshotLocation.Y);
        snapshot1.Size = new Size(this.zoomedSnapshotSize.Width, this.zoomedSnapshotSize.Height);
        snapshot1.Zoom(false);
        this.zoomedSnapshot = (SnapshotBaseControl) null;
        foreach (Control snapshot3 in this.snapshotList)
          snapshot3.Visible = true;
      }
      this.ResumeLayout();
    }

    private void snapshot_RefreshAllEvent(object sender, EventArgs e) => this.refreshAll();

    private void picEditView_Click(object sender, EventArgs e)
    {
      if (this.viewModified && this.IsCurrentViewEditable && Utils.Dialog((IWin32Window) this, "The Current Dashboard view contains changes which have not been saved.  Do you want to save these changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        this.siBtnSaveView_Click((object) null, (EventArgs) null);
      using (DashboardViewTemplateFormDialog templateFormDialog = new DashboardViewTemplateFormDialog(this.session, this.currentView, this.currentViewFileSystemEntry, DashboardViewTemplateFormDialog.ProcessingMode.ManageTemplates, this.externalOrgsList))
      {
        if (DialogResult.OK == templateFormDialog.ShowDialog())
        {
          int viewId = templateFormDialog.DashboardView.ViewId;
          this.currentViewFileSystemEntry = templateFormDialog.DashboardViewFileSystemEntry;
          this.refreshForm();
          this.displayView(DashboardView.GetDashboardView(viewId));
        }
        else
        {
          if (this.currentView == null)
            return;
          if (DashboardView.GetDashboardView(this.currentView.ViewId) == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Currently displayed view cannot be found or no longer accessible to this user account.  Please select another dashboard view.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.displayView((DashboardView) null);
            this.siBtnSelectView_Click((object) null, (EventArgs) null);
          }
          else
          {
            this.currentViewFileSystemEntry = templateFormDialog.OriFileSystemEntry;
            this.displayView(DashboardView.GetDashboardView(this.currentView.ViewId));
          }
        }
      }
    }

    private void cboViewFilter_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.cboViewFilter.SelectedIndex == this.currentViewFilterIndex)
        return;
      if (-3 == (int) this.cboViewFilter.SelectedValue && !this.hasOrganizationRight)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Filter Data by Organization' access right is required for this option.\n Contact your Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.cboViewFilter.SelectedIndex = this.currentViewFilterIndex;
      }
      else if (-4 == (int) this.cboViewFilter.SelectedValue && !this.hasUserGroupRight)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Filter Data by User Group' access right is required for this option.\n Contact your Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.cboViewFilter.SelectedIndex = this.currentViewFilterIndex;
      }
      else if (-5 == (int) this.cboViewFilter.SelectedValue && !this.hasTPOContactRight)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Filter Data by TPO' access right is required for this option.\n Contact your Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.cboViewFilter.SelectedIndex = this.currentViewFilterIndex;
      }
      else
      {
        int currentViewFilterIndex = this.currentViewFilterIndex;
        this.currentViewFilterIndex = this.cboViewFilter.SelectedIndex;
        if (!this.processViewFilterSelection())
          this.cboViewFilter.SelectedIndex = this.currentViewFilterIndex = currentViewFilterIndex;
        this.btnViewFilter.Enabled = this.cboViewFilter.SelectedIndex != 0;
      }
    }

    private void picViewFilter_Click(object sender, EventArgs e)
    {
      this.processViewFilterSelection();
    }

    private void picIcons_MouseEnter(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      pictureBox.Image = this.imgsIcons.Images[pictureBox.Name + "MouseOver"];
    }

    private void picIcons_MouseLeave(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      if (!pictureBox.Enabled)
        return;
      pictureBox.Image = this.imgsIcons.Images[pictureBox.Name];
    }

    private void btnManageSnapshots_Click(object sender, EventArgs e)
    {
      if (this.viewModified && this.IsCurrentViewEditable && Utils.Dialog((IWin32Window) this, "The Current Dashboard view contains changes which have not been saved.  Do you want to save these changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        this.siBtnSaveView_Click((object) null, (EventArgs) null);
      using (DashboardTemplateFormDialog templateFormDialog = new DashboardTemplateFormDialog(DashboardTemplateFormDialog.ProcessingMode.ManageTemplates))
      {
        int num = (int) templateFormDialog.ShowDialog();
      }
      this.refreshForm();
      if (this.currentView == null)
        return;
      this.displayView(DashboardView.GetDashboardView(this.currentView.ViewId));
    }

    private void siBtnSelectView_Click(object sender, EventArgs e)
    {
      if (this.viewModified && this.IsCurrentViewEditable && Utils.Dialog((IWin32Window) this, "The Current Dashboard view contains changes which have not been saved.  Do you want to save these changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        this.siBtnSaveView_Click((object) null, (EventArgs) null);
      using (DashboardViewTemplateFormDialog templateFormDialog = new DashboardViewTemplateFormDialog(this.session, this.currentView, this.currentViewFileSystemEntry, DashboardViewTemplateFormDialog.ProcessingMode.SelectTemplate, this.externalOrgsList))
      {
        if (DialogResult.OK != templateFormDialog.ShowDialog())
          return;
        int viewId = templateFormDialog.DashboardView.ViewId;
        this.currentViewFileSystemEntry = templateFormDialog.DashboardViewFileSystemEntry;
        this.refreshForm();
        this.displayView(DashboardView.GetDashboardView(viewId));
      }
    }

    private void siBtnRefreshView_Click(object sender, EventArgs e) => this.restoreDashboardView();

    private void siBtnSaveView_Click(object sender, EventArgs e)
    {
      if (DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "All the changes made will be saved to the current view.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
        return;
      this.saveDashboardView();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DashboardForm));
      this.pnlViewHeader = new Panel();
      this.gradientPanel2 = new GradientPanel();
      this.cboViewFilter = new ComboBox();
      this.txtViewFilter = new TextBox();
      this.lblRoleSelection = new Label();
      this.btnViewFilter = new StandardIconButton();
      this.gradientPanel1 = new GradientPanel();
      this.siBtnSaveView = new StandardIconButton();
      this.siBtnRefreshView = new StandardIconButton();
      this.siBtnSelectView = new StandardIconButton();
      this.label1 = new Label();
      this.txtCurrentViewName = new TextBox();
      this.btnManageSnapshots = new Button();
      this.btnViewMgr = new StandardIconButton();
      this.tipsIcons = new ToolTip(this.components);
      this.tipDashboard = new ToolTip(this.components);
      this.imgsIcons = new ImageList(this.components);
      this.pnlSnapshots = new BorderPanel();
      this.borderPanel1 = new BorderPanel();
      this.pnlViewHeader.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnViewFilter).BeginInit();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.siBtnSaveView).BeginInit();
      ((ISupportInitialize) this.siBtnRefreshView).BeginInit();
      ((ISupportInitialize) this.siBtnSelectView).BeginInit();
      ((ISupportInitialize) this.btnViewMgr).BeginInit();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlViewHeader.Controls.Add((Control) this.gradientPanel2);
      this.pnlViewHeader.Controls.Add((Control) this.gradientPanel1);
      this.pnlViewHeader.Dock = DockStyle.Top;
      this.pnlViewHeader.Location = new Point(1, 1);
      this.pnlViewHeader.Name = "pnlViewHeader";
      this.pnlViewHeader.Size = new Size(867, 63);
      this.pnlViewHeader.TabIndex = 0;
      this.gradientPanel2.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel2.Controls.Add((Control) this.cboViewFilter);
      this.gradientPanel2.Controls.Add((Control) this.txtViewFilter);
      this.gradientPanel2.Controls.Add((Control) this.lblRoleSelection);
      this.gradientPanel2.Controls.Add((Control) this.btnViewFilter);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(0, 30);
      this.gradientPanel2.Margin = new Padding(3, 3, 3, 0);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(867, 32);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 23;
      this.cboViewFilter.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboViewFilter.Enabled = false;
      this.cboViewFilter.Font = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboViewFilter.Items.AddRange(new object[3]
      {
        (object) "All",
        (object) "Loan Officer",
        (object) "Loan Processor"
      });
      this.cboViewFilter.Location = new Point(92, 4);
      this.cboViewFilter.Name = "cboViewFilter";
      this.cboViewFilter.Size = new Size(140, 22);
      this.cboViewFilter.TabIndex = 13;
      this.cboViewFilter.SelectionChangeCommitted += new EventHandler(this.cboViewFilter_SelectionChangeCommitted);
      this.txtViewFilter.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtViewFilter.Location = new Point(237, 5);
      this.txtViewFilter.Name = "txtViewFilter";
      this.txtViewFilter.ReadOnly = true;
      this.txtViewFilter.Size = new Size(161, 20);
      this.txtViewFilter.TabIndex = 15;
      this.lblRoleSelection.AutoSize = true;
      this.lblRoleSelection.BackColor = Color.Transparent;
      this.lblRoleSelection.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.lblRoleSelection.Location = new Point(7, 8);
      this.lblRoleSelection.Name = "lblRoleSelection";
      this.lblRoleSelection.Size = new Size(80, 14);
      this.lblRoleSelection.TabIndex = 14;
      this.lblRoleSelection.Text = "Show Data For";
      this.lblRoleSelection.TextAlign = ContentAlignment.MiddleLeft;
      this.btnViewFilter.BackColor = Color.Transparent;
      this.btnViewFilter.Location = new Point(403, 7);
      this.btnViewFilter.Name = "btnViewFilter";
      this.btnViewFilter.Size = new Size(16, 16);
      this.btnViewFilter.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnViewFilter.TabIndex = 21;
      this.btnViewFilter.TabStop = false;
      this.tipsIcons.SetToolTip((Control) this.btnViewFilter, "Lookup");
      this.btnViewFilter.Click += new EventHandler(this.picViewFilter_Click);
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.None;
      this.gradientPanel1.Controls.Add((Control) this.siBtnSaveView);
      this.gradientPanel1.Controls.Add((Control) this.siBtnRefreshView);
      this.gradientPanel1.Controls.Add((Control) this.siBtnSelectView);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Controls.Add((Control) this.txtCurrentViewName);
      this.gradientPanel1.Controls.Add((Control) this.btnManageSnapshots);
      this.gradientPanel1.Controls.Add((Control) this.btnViewMgr);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(867, 30);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 22;
      this.siBtnSaveView.BackColor = Color.Transparent;
      this.siBtnSaveView.Location = new Point(340, 8);
      this.siBtnSaveView.Name = "siBtnSaveView";
      this.siBtnSaveView.Size = new Size(16, 16);
      this.siBtnSaveView.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.siBtnSaveView.TabIndex = 23;
      this.siBtnSaveView.TabStop = false;
      this.tipsIcons.SetToolTip((Control) this.siBtnSaveView, "Save View");
      this.siBtnSaveView.Click += new EventHandler(this.siBtnSaveView_Click);
      this.siBtnRefreshView.BackColor = Color.Transparent;
      this.siBtnRefreshView.Location = new Point(361, 8);
      this.siBtnRefreshView.Name = "siBtnRefreshView";
      this.siBtnRefreshView.Size = new Size(16, 16);
      this.siBtnRefreshView.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.siBtnRefreshView.TabIndex = 22;
      this.siBtnRefreshView.TabStop = false;
      this.tipsIcons.SetToolTip((Control) this.siBtnRefreshView, "Reset View");
      this.siBtnRefreshView.Click += new EventHandler(this.siBtnRefreshView_Click);
      this.siBtnSelectView.BackColor = Color.Transparent;
      this.siBtnSelectView.Location = new Point(319, 8);
      this.siBtnSelectView.Name = "siBtnSelectView";
      this.siBtnSelectView.Size = new Size(16, 16);
      this.siBtnSelectView.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.siBtnSelectView.TabIndex = 21;
      this.siBtnSelectView.TabStop = false;
      this.tipsIcons.SetToolTip((Control) this.siBtnSelectView, "Select View");
      this.siBtnSelectView.Click += new EventHandler(this.siBtnSelectView_Click);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(7, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(112, 16);
      this.label1.TabIndex = 2;
      this.label1.Text = "Dashboard View";
      this.txtCurrentViewName.BackColor = Color.White;
      this.txtCurrentViewName.Location = new Point(124, 6);
      this.txtCurrentViewName.Name = "txtCurrentViewName";
      this.txtCurrentViewName.ReadOnly = true;
      this.txtCurrentViewName.Size = new Size(190, 20);
      this.txtCurrentViewName.TabIndex = 19;
      this.btnManageSnapshots.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnManageSnapshots.BackColor = SystemColors.Control;
      this.btnManageSnapshots.Location = new Point(749, 3);
      this.btnManageSnapshots.Name = "btnManageSnapshots";
      this.btnManageSnapshots.Padding = new Padding(1, 0, 0, 0);
      this.btnManageSnapshots.Size = new Size(113, 22);
      this.btnManageSnapshots.TabIndex = 17;
      this.btnManageSnapshots.Text = "Manage Snapshots";
      this.btnManageSnapshots.UseVisualStyleBackColor = true;
      this.btnManageSnapshots.Click += new EventHandler(this.btnManageSnapshots_Click);
      this.btnViewMgr.BackColor = Color.Transparent;
      this.btnViewMgr.Location = new Point(382, 8);
      this.btnViewMgr.Name = "btnViewMgr";
      this.btnViewMgr.Size = new Size(16, 16);
      this.btnViewMgr.StandardButtonType = StandardIconButton.ButtonType.ManageButton;
      this.btnViewMgr.TabIndex = 20;
      this.btnViewMgr.TabStop = false;
      this.tipsIcons.SetToolTip((Control) this.btnViewMgr, "Manage View");
      this.btnViewMgr.Click += new EventHandler(this.picEditView_Click);
      this.tipsIcons.ShowAlways = true;
      this.imgsIcons.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgsIcons.ImageStream");
      this.imgsIcons.TransparentColor = Color.Transparent;
      this.imgsIcons.Images.SetKeyName(0, "picSaveView");
      this.imgsIcons.Images.SetKeyName(1, "picSaveViewDisabled");
      this.imgsIcons.Images.SetKeyName(2, "picSaveViewMouseOver");
      this.imgsIcons.Images.SetKeyName(3, "picNewView");
      this.imgsIcons.Images.SetKeyName(4, "picNewViewDisabled");
      this.imgsIcons.Images.SetKeyName(5, "picNewViewMouseOver");
      this.imgsIcons.Images.SetKeyName(6, "picEditView");
      this.imgsIcons.Images.SetKeyName(7, "picEditViewDisabled");
      this.imgsIcons.Images.SetKeyName(8, "picEditViewMouseOver");
      this.imgsIcons.Images.SetKeyName(9, "picDeleteView");
      this.imgsIcons.Images.SetKeyName(10, "picDeleteViewDisabled");
      this.imgsIcons.Images.SetKeyName(11, "picDeleteViewMouseOver");
      this.imgsIcons.Images.SetKeyName(12, "picViewFilter");
      this.imgsIcons.Images.SetKeyName(13, "picViewFilterDisabled");
      this.imgsIcons.Images.SetKeyName(14, "picViewFilterMouseOver");
      this.pnlSnapshots.AutoScroll = true;
      this.pnlSnapshots.BackColor = Color.Transparent;
      this.pnlSnapshots.Borders = AnchorStyles.None;
      this.pnlSnapshots.Dock = DockStyle.Fill;
      this.pnlSnapshots.Location = new Point(1, 64);
      this.pnlSnapshots.Margin = new Padding(3, 0, 3, 3);
      this.pnlSnapshots.Name = "pnlSnapshots";
      this.pnlSnapshots.Size = new Size(867, 335);
      this.pnlSnapshots.TabIndex = 2;
      this.borderPanel1.Controls.Add((Control) this.pnlSnapshots);
      this.borderPanel1.Controls.Add((Control) this.pnlViewHeader);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(869, 400);
      this.borderPanel1.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(869, 400);
      this.Controls.Add((Control) this.borderPanel1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (DashboardForm);
      this.Text = "Dashboard";
      this.TransparencyKey = Color.Transparent;
      this.pnlViewHeader.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      ((ISupportInitialize) this.btnViewFilter).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.siBtnSaveView).EndInit();
      ((ISupportInitialize) this.siBtnRefreshView).EndInit();
      ((ISupportInitialize) this.siBtnSelectView).EndInit();
      ((ISupportInitialize) this.btnViewMgr).EndInit();
      this.borderPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
