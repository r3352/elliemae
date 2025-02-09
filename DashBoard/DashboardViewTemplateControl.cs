// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.DashboardViewTemplateControl
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientSession.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class DashboardViewTemplateControl : UserControl
  {
    private const string dashboardCategory = "Dashboard";
    private const string reservedViewName = "Dashboard.ReservedViewName";
    private const string SECTION_DASHBOARD = "Dashboard";
    private const string DEFAULT_VIEW_ID = "Dashboard.DefaultViewId";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private const int LAYOUT_COUNT = 18;
    private const int SNAPSHOT_COUNT = 9;
    private DashboardViewIFSExplorer ifsExplorer;
    private bool hasOrganizationRight;
    private bool hasUserGroupRight;
    private bool hasTPOContactRight;
    private const int VIEW_FILTER_NONE = -2;
    private const int VIEW_FILTER_ORGANIZATION = -3;
    private const int VIEW_FILTER_USER_GROUP = -4;
    private const int VIEW_FILTER_TPO = -5;
    private int currentViewFilterIndex = -1;
    private int viewFilterRoleId;
    private string viewFilterUserInRole = string.Empty;
    private OrgInfo viewFilterOrganization;
    private bool viewFilterIncludeChildren;
    private bool viewTPOFilterIncludeChildren;
    private AclGroup viewFilterUserGroup;
    private ExternalOriginatorManagementData viewFilterTPOOrg;
    private string viewFilterDescription = string.Empty;
    private bool hasEditRight;
    private WorkflowManager workflowManager;
    private PictureBox[] picLayouts;
    private Label[] lblSnapshots;
    private TextBox[] txtSnapshots;
    private PictureBox[] picSnapshots;
    private ComboBox[] cboTimeFrames;
    private bool hasPublicRight;
    private bool hasPrivateRight;
    private bool dataChanged;
    private PictureBox picSelectedLayout;
    private List<int> mostRecentViewIds = new List<int>();
    private DashboardLayoutCollection dashboardLayouts = DashboardLayoutCollection.GetDashboardLayoutCollection();
    private IDictionary dashboardSettings;
    private DashboardView dashboardView;
    private string currentDefaultViewPath = "";
    private DashboardViewTemplateControl.ProcessingMode processingMode;
    private FileSystemEntry fileSystemEntry;
    private DashboardViewTemplate dashboardViewTemplate;
    private DashboardViewFilterType viewFilterType = DashboardViewFilterType.None;
    private FileSystemEntry oriFileSystemEntry;
    private DashboardView oriDashboardView;
    private Sessions.Session session;
    private List<ExternalOriginatorManagementData> externalOrgsList;
    private IContainer components;
    private Panel pnlLeft;
    private FSExplorer fsExplorer;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel pnlRight;
    private Panel pnlSelectSnapshot;
    protected ComboBox cboTimeFrame9;
    protected ComboBox cboTimeFrame8;
    protected ComboBox cboTimeFrame7;
    protected ComboBox cboTimeFrame6;
    protected ComboBox cboTimeFrame5;
    protected ComboBox cboTimeFrame4;
    protected ComboBox cboTimeFrame3;
    protected ComboBox cboTimeFrame2;
    protected ComboBox cboTimeFrame1;
    private Label lblSelectSnapshot;
    private Label lblSnapshot1;
    private Label lblSnapshot2;
    private Label lblSnapshot3;
    private Label lblSnapshot4;
    private PictureBox picSnapshot9;
    private ToolTip tipToolTip;
    private Label lblSnapshot5;
    private PictureBox picSnapshot8;
    private Label lblSnapshot6;
    private PictureBox picSnapshot7;
    private Label lblSnapshot7;
    private PictureBox picSnapshot6;
    private Label lblSnapshot8;
    private PictureBox picSnapshot5;
    private Label lblSnapshot9;
    private PictureBox picSnapshot4;
    private TextBox txtSnapshot1;
    private PictureBox picSnapshot3;
    private TextBox txtSnapshot2;
    private PictureBox picSnapshot2;
    private TextBox txtSnapshot3;
    private PictureBox picSnapshot1;
    private TextBox txtSnapshot4;
    private TextBox txtSnapshot9;
    private TextBox txtSnapshot5;
    private TextBox txtSnapshot8;
    private TextBox txtSnapshot6;
    private TextBox txtSnapshot7;
    private Panel pnlSelectLayout;
    private Panel pnlLayout3;
    private PictureBox picLayout3;
    private Label lblSelectLayout;
    private Panel pnlLayout18;
    private PictureBox picLayout18;
    private Panel pnlLayout1;
    private PictureBox picLayout1;
    private Panel pnlLayout16;
    private PictureBox picLayout16;
    private Panel pnlLayout2;
    private PictureBox picLayout2;
    private Panel pnlLayout15;
    private PictureBox picLayout15;
    private Panel pnlLayout17;
    private PictureBox picLayout17;
    private Panel pnlLayout14;
    private PictureBox picLayout14;
    private Panel pnlLayout4;
    private PictureBox picLayout4;
    private Panel pnlLayout13;
    private PictureBox picLayout13;
    private Panel pnlLayout5;
    private PictureBox picLayout5;
    private Panel pnlLayout12;
    private PictureBox picLayout12;
    private Panel pnlLayout6;
    private PictureBox picLayout6;
    private Panel pnlLayout11;
    private PictureBox picLayout11;
    private Panel pnlLayout7;
    private PictureBox picLayout7;
    private Panel pnlLayout10;
    private PictureBox picLayout10;
    private Panel pnlLayout8;
    private PictureBox picLayout8;
    private Panel pnlLayout9;
    private PictureBox picLayout9;
    private Button btnSave;
    private Button btnCancel;
    private ImageList imgLstSearch;
    private ImageList imglstLayouts;
    private Button btnSelect;
    private Button btnSelectView;
    private GradientPanel gpDefaultView;
    private Label label1;
    private Button btnGoTo;
    private TextBox txtDefaultPath;
    private Panel pnlNavigate;
    private Panel pnlSelectView;
    private Button btnCancelSelect;
    private GroupContainer gcViewConfig;
    private GradientPanel gradientPanel1;
    private ComboBox cboViewFilter;
    private StandardIconButton btnViewFilter;
    private TextBox txtViewFilter;
    private Label lblRoleSelection;
    private GradientPanel gradientPanel2;

    public DashboardView DashboardView
    {
      get
      {
        if (this.dashboardView.SessionObject == null)
          this.dashboardView.SessionObject = this.session;
        return this.dashboardView;
      }
    }

    public FileSystemEntry DashboardViewFileSystemEntry => this.fileSystemEntry;

    public Size GetSelectTemplateSize
    {
      get => new Size(this.pnlLeft.Width + 15, this.pnlLeft.Height + 35);
    }

    public Size GetManageTemplateSize => new Size(this.Width + 15, this.Height + 45);

    protected override void OnCreateControl()
    {
      base.OnCreateControl();
      if (this.ParentForm == null)
        return;
      this.ParentForm.FormClosing += new FormClosingEventHandler(this.DashboardViewTemplateForm_FormClosing);
    }

    public DashboardViewTemplateControl(
      Sessions.Session session,
      DashboardView dashboardView,
      FileSystemEntry fileSystem,
      DashboardViewTemplateControl.ProcessingMode processingMode,
      List<ExternalOriginatorManagementData> externalOrgsList,
      bool isMultiSelect,
      bool saveOnly)
      : this(session, dashboardView, fileSystem, processingMode, externalOrgsList)
    {
      switch (processingMode)
      {
        case DashboardViewTemplateControl.ProcessingMode.EditTemplate:
        case DashboardViewTemplateControl.ProcessingMode.ManageTemplates:
          this.btnCancel.Visible = this.btnSelect.Visible = !saveOnly;
          this.fsExplorer.SingleSelection = !isMultiSelect;
          break;
      }
    }

    public DashboardViewTemplateControl(
      Sessions.Session session,
      DashboardView dashboardView,
      FileSystemEntry fileSystem,
      DashboardViewTemplateControl.ProcessingMode processingMode,
      List<ExternalOriginatorManagementData> externalOrgsList)
    {
      this.session = session;
      this.workflowManager = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
      this.processingMode = processingMode;
      this.dashboardView = dashboardView;
      this.fileSystemEntry = fileSystem;
      this.oriFileSystemEntry = fileSystem;
      this.oriDashboardView = dashboardView;
      this.externalOrgsList = externalOrgsList;
      this.fsExplorer = new FSExplorer(this.session);
      this.InitializeComponent();
      this.initialize();
    }

    private void DashboardViewTemplateForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.setLastFolderViewed();
    }

    private void populateFormWithView()
    {
      if (this.dashboardView == null)
      {
        this.clearUI();
        this.btnSelect.Enabled = false;
      }
      else
      {
        this.updateUIFromView(this.dashboardView);
        this.setViewModified(false);
      }
    }

    private void updateCurrentDefaultView()
    {
      this.currentDefaultViewPath = this.session.GetPrivateProfileString("Dashboard", "Dashboard.DefaultViewId");
      if (this.currentDefaultViewPath == null || this.currentDefaultViewPath == "")
      {
        this.txtDefaultPath.Text = "";
      }
      else
      {
        string[] strArray = this.currentDefaultViewPath.Split('\\');
        if (strArray == null || strArray.Length == 0)
          return;
        this.txtDefaultPath.Text = strArray[strArray.Length - 1];
      }
    }

    private void initialize()
    {
      this.getSecuritySettings();
      this.setViewFilterSelection();
      this.currentDefaultViewPath = "";
      try
      {
        this.updateCurrentDefaultView();
      }
      catch (Exception ex)
      {
      }
      this.dashboardLayouts = DashboardLayoutCollection.GetDashboardLayoutCollection();
      this.picLayouts = new PictureBox[18];
      this.picLayouts[0] = this.picLayout1;
      this.picLayouts[1] = this.picLayout2;
      this.picLayouts[2] = this.picLayout3;
      this.picLayouts[3] = this.picLayout4;
      this.picLayouts[4] = this.picLayout5;
      this.picLayouts[5] = this.picLayout6;
      this.picLayouts[6] = this.picLayout7;
      this.picLayouts[7] = this.picLayout8;
      this.picLayouts[8] = this.picLayout9;
      this.picLayouts[9] = this.picLayout10;
      this.picLayouts[10] = this.picLayout11;
      this.picLayouts[11] = this.picLayout12;
      this.picLayouts[12] = this.picLayout13;
      this.picLayouts[13] = this.picLayout14;
      this.picLayouts[14] = this.picLayout15;
      this.picLayouts[15] = this.picLayout16;
      this.picLayouts[16] = this.picLayout17;
      this.picLayouts[17] = this.picLayout18;
      this.lblSnapshots = new Label[9];
      this.lblSnapshots[0] = this.lblSnapshot1;
      this.lblSnapshots[1] = this.lblSnapshot2;
      this.lblSnapshots[2] = this.lblSnapshot3;
      this.lblSnapshots[3] = this.lblSnapshot4;
      this.lblSnapshots[4] = this.lblSnapshot5;
      this.lblSnapshots[5] = this.lblSnapshot6;
      this.lblSnapshots[6] = this.lblSnapshot7;
      this.lblSnapshots[7] = this.lblSnapshot8;
      this.lblSnapshots[8] = this.lblSnapshot9;
      this.txtSnapshots = new TextBox[9];
      this.txtSnapshots[0] = this.txtSnapshot1;
      this.txtSnapshots[1] = this.txtSnapshot2;
      this.txtSnapshots[2] = this.txtSnapshot3;
      this.txtSnapshots[3] = this.txtSnapshot4;
      this.txtSnapshots[4] = this.txtSnapshot5;
      this.txtSnapshots[5] = this.txtSnapshot6;
      this.txtSnapshots[6] = this.txtSnapshot7;
      this.txtSnapshots[7] = this.txtSnapshot8;
      this.txtSnapshots[8] = this.txtSnapshot9;
      this.picSnapshots = new PictureBox[9];
      this.picSnapshots[0] = this.picSnapshot1;
      this.picSnapshots[1] = this.picSnapshot2;
      this.picSnapshots[2] = this.picSnapshot3;
      this.picSnapshots[3] = this.picSnapshot4;
      this.picSnapshots[4] = this.picSnapshot5;
      this.picSnapshots[5] = this.picSnapshot6;
      this.picSnapshots[6] = this.picSnapshot7;
      this.picSnapshots[7] = this.picSnapshot8;
      this.picSnapshots[8] = this.picSnapshot9;
      this.cboTimeFrames = new ComboBox[9];
      this.cboTimeFrames[0] = this.cboTimeFrame1;
      this.cboTimeFrames[1] = this.cboTimeFrame2;
      this.cboTimeFrames[2] = this.cboTimeFrame3;
      this.cboTimeFrames[3] = this.cboTimeFrame4;
      this.cboTimeFrames[4] = this.cboTimeFrame5;
      this.cboTimeFrames[5] = this.cboTimeFrame6;
      this.cboTimeFrames[6] = this.cboTimeFrame7;
      this.cboTimeFrames[7] = this.cboTimeFrame8;
      this.cboTimeFrames[8] = this.cboTimeFrame9;
      if (DashboardViewTemplateControl.ProcessingMode.SelectTemplate == this.processingMode)
      {
        this.Text = "Select View";
        this.initializeDashboardViewExplorer(FSExplorer.DialogMode.SelectFiles);
        this.pnlRight.Visible = false;
        this.collapsibleSplitter1.Visible = false;
        if (this.fileSystemEntry == null)
          return;
        this.fsExplorer.SetFolder(this.fileSystemEntry);
      }
      else
      {
        this.Text = "Manage View";
        this.initializeDashboardViewExplorer(FSExplorer.DialogMode.ManageFiles);
        if (this.fileSystemEntry != null)
        {
          this.fsExplorer.SetFolder(this.fileSystemEntry);
          this.setViewModified(false);
        }
        else
        {
          this.clearUI();
          this.btnSelect.Enabled = false;
        }
        this.pnlSelectView.Visible = false;
      }
    }

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

    private void getSecuritySettings()
    {
      this.hasPublicRight = UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) || this.session.AclGroupManager.CheckPublicAccessPermission(AclFileType.DashboardViewTemplate);
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.hasPrivateRight = aclManager.GetUserApplicationRight(AclFeature.DashboardTab_ManagePersonalViewTemplate);
      this.hasOrganizationRight = aclManager.GetUserApplicationRight(AclFeature.DashboardTab_Organization);
      this.hasUserGroupRight = aclManager.GetUserApplicationRight(AclFeature.DashboardTab_UserGroup);
      this.hasTPOContactRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_Contacts);
    }

    private void initializeDashboardViewExplorer(FSExplorer.DialogMode fsExplorerMode)
    {
      this.fsExplorer.SetDashboardViewTemplateProperties(fsExplorerMode);
      this.ifsExplorer = new DashboardViewIFSExplorer(DashboardViewTemplateControl.ProcessingMode.SelectTemplate == this.processingMode, (DashboardViewList) null, this.session);
      FileSystemEntry defaultFolder = FileSystemEntry.PublicRoot;
      if (this.hasPrivateRight)
        defaultFolder = FileSystemEntry.PrivateRoot(this.session.UserID);
      FileSystemEntry lastFolderViewed = this.getLastFolderViewed();
      if (lastFolderViewed != null)
        defaultFolder = lastFolderViewed;
      this.fsExplorer.InitDashboardViewTemplate((IFSExplorerBase) this.ifsExplorer, defaultFolder);
      this.fsExplorer.SelectedEntryChanged += new EventHandler(this.fsExplorer_SelectedEntryChanged);
      this.fsExplorer.FolderChanged += new EventHandler(this.fsExplorer_FolderChanged);
      this.fsExplorer.BeforeFolderRenamed += new EventHandler(this.fsExplorer_BeforeFolderRenamed);
      this.fsExplorer.Leave += new EventHandler(this.fsExplorer_Leave);
      this.ifsExplorer.DeleteFileEvent += new DashboardViewIFSExplorer.DeleteFileEventHandler(this.ifsExplorer_DeleteFileEvent);
      this.ifsExplorer.MoveEntryEvent += new DashboardViewIFSExplorer.MoveEntryEventHandler(this.ifsExplorer_MoveEntryEvent);
      this.ifsExplorer.DuplicateFileEvent += new DashboardViewIFSExplorer.DuplicateFileEventHandler(this.ifsExplorer_DuplicateFileEvent);
      this.fsExplorer.SetAsDefaultButtonClick += new EventHandler(this.fsExplorer_SetAsDefaultButtonClick);
      if (DashboardViewTemplateControl.ProcessingMode.SelectTemplate != this.processingMode && this.processingMode != DashboardViewTemplateControl.ProcessingMode.ManageTemplates)
        return;
      this.ifsExplorer.OpenFileEvent += new DashboardViewIFSExplorer.OpenFileEventHandler(this.ifsExplorer_OpenFileEvent);
    }

    private void fsExplorer_SetAsDefaultButtonClick(object sender, EventArgs e)
    {
      if (this.fsExplorer.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a Dashboard View.");
      }
      else
      {
        FileSystemEntry tag = (FileSystemEntry) this.fsExplorer.SelectedItems[0].Tag;
        if (tag.Type != FileSystemEntry.Types.File)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select a Dashboard View template file.");
        }
        else
        {
          this.session.WritePrivateProfileString("Dashboard", "Dashboard.DefaultViewId", tag.ToString());
          this.currentDefaultViewPath = tag.ToString();
          this.updateCurrentDefaultView();
        }
      }
    }

    private void ifsExplorer_DuplicateFileEvent(
      object sender,
      SelectedFileEventArgs source,
      SelectedFileEventArgs target)
    {
      this.session.ReportManager.SyncDashboardView(source.FSEntry, target.FSEntry);
    }

    private FileSystemEntry getLastFolderViewed()
    {
      string privateProfileString = this.session.GetPrivateProfileString("DashboardViewTemplate", "LastFolderViewed");
      if (privateProfileString != null)
      {
        if (string.Empty != privateProfileString)
        {
          try
          {
            FileSystemEntry entry = FileSystemEntry.Parse(privateProfileString);
            if (this.ifsExplorer.EntryExists(entry))
            {
              if (!entry.IsPublic || !this.hasPublicRight)
              {
                if (!entry.IsPublic)
                {
                  if (!this.hasPrivateRight)
                    goto label_8;
                }
                else
                  goto label_8;
              }
              return entry;
            }
          }
          catch (Exception ex)
          {
          }
        }
      }
label_8:
      return (FileSystemEntry) null;
    }

    private void setLastFolderViewed()
    {
      try
      {
        if (this.fsExplorer.IsTopFolder)
          return;
        this.session.WritePrivateProfileString("DashboardViewTemplate", "LastFolderViewed", this.fsExplorer.CurrentFolder.ToString());
      }
      catch (Exception ex)
      {
      }
    }

    private void setReadOnlyMode()
    {
      this.pnlSelectLayout.Enabled = false;
      this.pnlSelectSnapshot.Enabled = false;
      foreach (PictureBox picSnapshot in this.picSnapshots)
        picSnapshot.Image = this.imgLstSearch.Images["picSearchDisabled"];
    }

    private void updateUIFromView(DashboardView startFromView)
    {
      if (string.Empty != this.dashboardView.ViewName)
        this.gcViewConfig.Text = this.dashboardView.ViewName;
      DashboardView dashboardView = startFromView == null ? this.dashboardView : startFromView;
      if (dashboardView.LayoutId < 1 || dashboardView.LayoutId > 18)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error encountered while loading selected view.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.enforceSecurity();
        Dictionary<ComboBox, DashboardViewTemplateControl.SnapshotTag> controlSnapshotList = new Dictionary<ComboBox, DashboardViewTemplateControl.SnapshotTag>();
        for (int index = 0; index < this.txtSnapshots.Length; ++index)
        {
          DashboardViewTemplateControl.SnapshotTag snapshotTag = (DashboardViewTemplateControl.SnapshotTag) null;
          if (index < dashboardView.ReportCollection.Count)
          {
            snapshotTag = new DashboardViewTemplateControl.SnapshotTag(dashboardView.ReportCollection[index].ReportName, dashboardView.ReportCollection[index].DashboardTemplatePath, dashboardView.ReportCollection[index].ReportParameters);
            this.txtSnapshots[index].Tag = (object) snapshotTag;
            this.txtSnapshots[index].Text = snapshotTag.ReportName;
          }
          else
          {
            this.txtSnapshots[index].Tag = (object) null;
            this.txtSnapshots[index].Text = string.Empty;
          }
          controlSnapshotList.Add(this.cboTimeFrames[index], snapshotTag);
        }
        this.updateCboFromView(controlSnapshotList);
        this.picLayout_Click((object) this.picLayouts[dashboardView.LayoutId - 1], EventArgs.Empty);
        if (dashboardView.ReportCollection.Count > 0)
          this.btnSelect.Enabled = true;
        else
          this.btnSelect.Enabled = false;
        this.cboViewFilter.Enabled = true;
        this.setViewFilterValues(this.hasEditRight, this.dashboardView.ViewFilterType, this.dashboardView.ViewFilterRoleId, this.dashboardView.ViewFilterUserInRole, this.dashboardView.ViewFilterOrganizationId, this.dashboardView.ViewFilterIncludeChildren, this.dashboardView.ViewFilterUserGroupId, this.dashboardView.ViewFilterTPOOrgId, this.dashboardView.ViewTPOFilterIncludeChildren);
      }
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

    private void setTextBoxText(TextBox textBox, string text)
    {
      if (string.Empty == text)
      {
        textBox.Text = string.Empty;
        this.tipToolTip.SetToolTip((Control) textBox, string.Empty);
      }
      else
      {
        using (Graphics graphics = textBox.CreateGraphics())
        {
          string empty = string.Empty;
          if (this.fitText(graphics, textBox.Font, (float) textBox.Width, text, ref empty))
          {
            textBox.Text = text;
            this.tipToolTip.SetToolTip((Control) textBox, string.Empty);
          }
          else
          {
            textBox.Text = empty;
            this.tipToolTip.SetToolTip((Control) textBox, text);
          }
        }
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
      if (!isReadWrite)
      {
        this.cboViewFilter.Enabled = isReadWrite;
        this.btnViewFilter.Enabled = isReadWrite;
      }
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

    private void cboViewFilter_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.cboViewFilter.SelectedIndex == this.currentViewFilterIndex)
        return;
      this.setViewModified(true);
      if (-3 == (int) this.cboViewFilter.SelectedValue && !this.hasOrganizationRight)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Filter Data by Organization' access right is required for this option.\n Contact your Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.cboViewFilter.SelectedIndex = this.currentViewFilterIndex;
      }
      else if (-5 == (int) this.cboViewFilter.SelectedValue && !this.hasTPOContactRight)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Filter Data by TPO' access right is required for this option.\n Contact your Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.cboViewFilter.SelectedIndex = this.currentViewFilterIndex;
      }
      else if (-4 == (int) this.cboViewFilter.SelectedValue && !this.hasUserGroupRight)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Filter Data by User Group' access right is required for this option.\n Contact your Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
            using (TPOOrgDialog tpoOrgDialog = new TPOOrgDialog(this.externalOrgsList, this.session))
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
            using (UserGroupDialog userGroupDialog = new UserGroupDialog(this.session))
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
            using (OrganizationDialog organizationDialog = new OrganizationDialog(this.session))
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
        this.setViewModified(true);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The " + str + "filter criteria for this view could not be created.\nSetup options may have changed which makes the criteria invalid.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.txtViewFilter.Text = text;
        return false;
      }
      return true;
    }

    private void enforceSecurity()
    {
      if (this.fileSystemEntry == null)
        this.setReadWriteAccess(false);
      else if (!this.fileSystemEntry.IsPublic)
        this.setReadWriteAccess(true);
      else if (this.session.AclGroupManager.GetUserFileFolderAccess(AclFileType.DashboardViewTemplate, this.fileSystemEntry) == AclResourceAccess.ReadWrite)
        this.setReadWriteAccess(true);
      else
        this.setReadWriteAccess(false);
    }

    private void setReadWriteAccess(bool writeAccess)
    {
      this.hasEditRight = writeAccess;
      this.pnlSelectLayout.Enabled = writeAccess;
      this.pnlSelectSnapshot.Enabled = writeAccess;
      foreach (PictureBox picSnapshot in this.picSnapshots)
        picSnapshot.Image = writeAccess ? this.imgLstSearch.Images["picSearch"] : this.imgLstSearch.Images["picSearchDisabled"];
      this.cboViewFilter.Enabled = false;
      this.btnViewFilter.Enabled = false;
    }

    private void updateCboFromView(DashboardViewTemplateControl.SnapshotTag tag, ComboBox control)
    {
      if (tag == null)
      {
        control.DataSource = (object) null;
        control.Items.Clear();
        control.Enabled = false;
      }
      else
      {
        FileSystemEntry fsFile = FileSystemEntry.Parse(tag.TemplatePath);
        DashboardTemplate dashboardTemplate = new DashboardIFSExplorer(this.session).LoadDashboardTemplate(fsFile);
        if (dashboardTemplate == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The associated snapshot '" + fsFile.Name + "' can not be found.  It may have been deleted.");
          control.DataSource = (object) null;
          control.Items.Clear();
          control.Enabled = false;
        }
        else
        {
          control.SelectedIndexChanged -= new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
          switch (dashboardTemplate.ChartType)
          {
            case DashboardChartType.BarChart:
            case DashboardChartType.LoanTable:
            case DashboardChartType.UserTable:
              control.DataSource = SelectionOptions.TimeFrameOptions.Clone();
              break;
            case DashboardChartType.TrendChart:
              control.DataSource = SelectionOptions.TimePeriodOptions.Clone();
              break;
          }
          control.DisplayMember = "Name";
          control.ValueMember = "Id";
          int num = 0;
          if (1 == tag.ReportParameters.Length && tag.ReportParameters[0] != "")
          {
            num = int.Parse(tag.ReportParameters[0]);
          }
          else
          {
            switch (dashboardTemplate.ChartType)
            {
              case DashboardChartType.BarChart:
              case DashboardChartType.LoanTable:
              case DashboardChartType.UserTable:
                num = 9;
                break;
              case DashboardChartType.TrendChart:
                num = 4;
                break;
            }
          }
          control.SelectedValue = (object) num;
          control.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
          this.cboTimeFrame_SelectedIndexChanged((object) control, (EventArgs) null);
        }
      }
    }

    private void updateCboFromView(
      Dictionary<ComboBox, DashboardViewTemplateControl.SnapshotTag> controlSnapshotList)
    {
      Dictionary<ComboBox, FileSystemEntry> dictionary1 = new Dictionary<ComboBox, FileSystemEntry>();
      foreach (ComboBox key in controlSnapshotList.Keys)
      {
        if (controlSnapshotList[key] == null)
        {
          key.DataSource = (object) null;
          key.Items.Clear();
          key.Enabled = false;
        }
        else
          dictionary1.Add(key, FileSystemEntry.Parse(controlSnapshotList[key].TemplatePath));
      }
      DashboardIFSExplorer dashboardIfsExplorer = new DashboardIFSExplorer(this.session);
      FileSystemEntry[] fileSystemEntryArray = new FileSystemEntry[dictionary1.Count];
      dictionary1.Values.CopyTo(fileSystemEntryArray, 0);
      Dictionary<FileSystemEntry, DashboardTemplate> dictionary2 = dashboardIfsExplorer.LoadDashboardTemplate(fileSystemEntryArray);
      foreach (ComboBox key1 in controlSnapshotList.Keys)
      {
        if (!dictionary1.ContainsKey(key1))
          break;
        FileSystemEntry key2 = dictionary1[key1];
        DashboardTemplate dashboardTemplate = (DashboardTemplate) null;
        if (dictionary2.ContainsKey(key2))
          dashboardTemplate = dictionary2[key2];
        if (dashboardTemplate == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The associated snapshot '" + key2.Name + "' can not be found.  It may have been deleted.");
          key1.DataSource = (object) null;
          key1.Items.Clear();
          key1.Enabled = false;
          break;
        }
        key1.SelectedIndexChanged -= new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
        switch (dashboardTemplate.ChartType)
        {
          case DashboardChartType.BarChart:
          case DashboardChartType.LoanTable:
          case DashboardChartType.UserTable:
            key1.DataSource = SelectionOptions.TimeFrameOptions.Clone();
            break;
          case DashboardChartType.TrendChart:
            key1.DataSource = SelectionOptions.TimePeriodOptions.Clone();
            break;
        }
        key1.DisplayMember = "Name";
        key1.ValueMember = "Id";
        int num1 = 0;
        if (1 == controlSnapshotList[key1].ReportParameters.Length && controlSnapshotList[key1].ReportParameters[0] != "")
        {
          num1 = int.Parse(controlSnapshotList[key1].ReportParameters[0]);
        }
        else
        {
          switch (dashboardTemplate.ChartType)
          {
            case DashboardChartType.BarChart:
            case DashboardChartType.LoanTable:
            case DashboardChartType.UserTable:
              num1 = 9;
              break;
            case DashboardChartType.TrendChart:
              num1 = 4;
              break;
          }
        }
        key1.SelectedValue = (object) num1;
        key1.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      }
    }

    private bool updateViewFromUI()
    {
      DashboardLayout dashboardLayout = this.dashboardLayouts.Find(this.picSelectedLayout.Tag.ToString());
      DashboardReport[] dashboardReportArray = new DashboardReport[dashboardLayout.LayoutBlocks.Length];
      for (int index = 0; index < dashboardLayout.LayoutBlocks.Length; ++index)
      {
        if (!(this.txtSnapshots[index].Tag is DashboardViewTemplateControl.SnapshotTag tag))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please specify a snapshot for each layout position.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.txtSnapshots[index].Focus();
          return false;
        }
        DashboardReport dashboardReport = DashboardReport.NewDashboardReport();
        dashboardReport.LayoutBlockNumber = index + 1;
        dashboardReport.DashboardTemplatePath = tag.TemplatePath;
        dashboardReport.ReportParameters = tag.ReportParameters;
        dashboardReportArray[index] = dashboardReport;
      }
      this.dashboardView.DashboardLayout = dashboardLayout;
      this.dashboardView.ReportCollection.Clear();
      foreach (DashboardReport dashboardReport in dashboardReportArray)
        this.dashboardView.ReportCollection.Add(dashboardReport);
      this.dashboardView.ViewFilterType = this.viewFilterType;
      this.dashboardView.ViewFilterRoleId = this.viewFilterRoleId;
      this.dashboardView.ViewFilterUserInRole = this.viewFilterUserInRole;
      this.dashboardView.ViewFilterOrganizationId = this.viewFilterOrganization == null ? 0 : this.viewFilterOrganization.Oid;
      this.dashboardView.ViewFilterIncludeChildren = this.viewFilterIncludeChildren;
      this.dashboardView.ViewFilterUserGroupId = (AclGroup) null == this.viewFilterUserGroup ? 0 : this.viewFilterUserGroup.ID;
      this.dashboardView.ViewFilterTPOOrgId = this.viewFilterTPOOrg == null ? "0" : this.viewFilterTPOOrg.ExternalID;
      this.dashboardView.ViewTPOFilterIncludeChildren = this.viewTPOFilterIncludeChildren;
      this.dashboardView.IsFolder = false;
      this.dashboardView.SessionObject = this.session;
      return true;
    }

    private void setViewModified(bool value)
    {
      this.dataChanged = value;
      this.btnSave.Enabled = value;
    }

    private bool isReservedName(string viewName)
    {
      if (this.dashboardSettings == null)
        this.dashboardSettings = this.session.ServerManager.GetServerSettings("Dashboard");
      foreach (object key in (IEnumerable) this.dashboardSettings.Keys)
      {
        if (key.ToString().StartsWith("Dashboard.ReservedViewName", StringComparison.OrdinalIgnoreCase) && string.Equals(this.dashboardSettings[key].ToString(), viewName, StringComparison.OrdinalIgnoreCase))
          return true;
      }
      return false;
    }

    private bool saveView()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        this.dashboardView = (DashboardView) this.dashboardView.Save();
        if (this.dashboardView.LayoutBlockCount > 0)
          this.btnSelect.Enabled = true;
        else
          this.btnSelect.Enabled = false;
      }
      catch (Exception ex)
      {
        if (0 <= ex.Message.IndexOf("Violation of UNIQUE KEY constraint 'UK_DashboardView_ViewName'"))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The name you specified already exists.\nPlease specify a different name.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        throw;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
      return true;
    }

    private void picLayout_Click(object sender, EventArgs e)
    {
      if (this.picSelectedLayout != null)
      {
        this.picSelectedLayout.Parent.BackColor = SystemColors.Control;
        this.picSelectedLayout.Image = this.imglstLayouts.Images[this.picSelectedLayout.Tag.ToString()];
      }
      foreach (Control picLayout in this.picLayouts)
        picLayout.Enabled = true;
      this.picSelectedLayout = (PictureBox) sender;
      int length = this.dashboardLayouts.Find(this.picSelectedLayout.Tag.ToString()).LayoutBlocks.Length;
      for (int index = 0; index < 9; ++index)
      {
        bool flag = index < length;
        this.lblSnapshots[index].Visible = flag;
        this.txtSnapshots[index].Visible = flag;
        this.picSnapshots[index].Visible = flag;
        this.cboTimeFrames[index].Visible = flag;
        if (flag)
        {
          if (this.txtSnapshots[index].Text == "" || this.cboTimeFrames[index].Items.Count == 0)
            this.cboTimeFrames[index].Enabled = false;
          else
            this.cboTimeFrames[index].Enabled = true;
        }
      }
      this.picSelectedLayout.Parent.BackColor = Color.FromArgb((int) byte.MaxValue, 156, 0);
      this.picSelectedLayout.Image = this.imglstLayouts.Images[this.picSelectedLayout.Tag.ToString() + "Ovr"];
      this.setViewModified(true);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.updateView())
        return;
      this.setViewModified(false);
    }

    private bool updateView() => !this.dataChanged || this.updateViewFromUI() && this.saveView();

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this.ParentForm == null)
        return;
      this.ParentForm.DialogResult = DialogResult.Cancel;
      this.ParentForm.Close();
    }

    private void picLayout_MouseEnter(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      string key = pictureBox.Tag.ToString() + "Ovr";
      pictureBox.Image = this.imglstLayouts.Images[key];
    }

    private void picLayout_MouseLeave(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      string key = pictureBox.Tag.ToString();
      if (this.picSelectedLayout == null || !(key != this.picSelectedLayout.Tag.ToString()))
        return;
      pictureBox.Image = this.imglstLayouts.Images[key];
    }

    private void picSnapshot_Click(object sender, EventArgs e)
    {
      using (DashboardTemplateFormDialog templateFormDialog = new DashboardTemplateFormDialog(DashboardTemplateFormDialog.ProcessingMode.SelectTemplate, this.fileSystemEntry.IsPublic, this.session))
      {
        if (DialogResult.OK != templateFormDialog.ShowDialog())
          return;
        DashboardTemplate selectedTemplate = templateFormDialog.SelectedTemplate;
        if (selectedTemplate == null)
          return;
        FileSystemEntry selectedFileSystemEntry = templateFormDialog.SelectedFileSystemEntry;
        DashboardViewTemplateControl.SnapshotTag tag = new DashboardViewTemplateControl.SnapshotTag(selectedTemplate.TemplateName, selectedFileSystemEntry.ToString(), new string[0]);
        int int32 = Convert.ToInt32(((Control) sender).Tag);
        this.txtSnapshots[int32].Tag = (object) tag;
        this.txtSnapshots[int32].Text = tag.ReportName;
        this.cboTimeFrames[int32].Enabled = true;
        this.updateCboFromView(tag, this.cboTimeFrames[int32]);
        this.setViewModified(true);
      }
    }

    private void picSnapshot_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = this.imgLstSearch.Images["picSearchMouseOver"];
    }

    private void picSnapshot_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = this.imgLstSearch.Images["picSearch"];
    }

    private void chkDefaultView_Click(object sender, EventArgs e) => this.btnSave.Enabled = true;

    private void cboTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
    {
      int int32 = Convert.ToInt32(((Control) sender).Tag);
      DashboardViewTemplateControl.SnapshotTag tag = (DashboardViewTemplateControl.SnapshotTag) this.txtSnapshots[int32].Tag;
      if (tag == null)
        return;
      tag.ReportParameters = new string[1]
      {
        string.Concat(((ListControl) sender).SelectedValue)
      };
      this.txtSnapshots[int32].Tag = (object) tag;
      this.setViewModified(true);
    }

    private void fsExplorer_SelectedEntryChanged(object sender, EventArgs e)
    {
      this.checkForDataChanged();
      if (this.fsExplorer.SelectedItems.Count == 0 || string.Empty == this.fsExplorer.SelectedItems[0].Tag.ToString() || FileSystemEntry.Types.Folder == ((FileSystemEntry) this.fsExplorer.SelectedItems[0].Tag).Type)
      {
        this.clearUI();
        this.btnSelect.Enabled = false;
        this.fsExplorer.EnableSetAsDefaultButton(false);
      }
      else
      {
        this.fsExplorer.EnableSetAsDefaultButton(true);
        this.getTemplate((FileSystemEntry) this.fsExplorer.SelectedItems[0].Tag);
        if (this.dashboardView.ReportCollection.Count == 0)
        {
          this.btnSelect.Enabled = false;
          this.btnSelectView.Enabled = false;
        }
        else
        {
          this.btnSelect.Enabled = true;
          this.btnSelectView.Enabled = true;
        }
      }
    }

    private void clearUI()
    {
      for (int index = 0; index < this.picLayouts.Length; ++index)
      {
        if (index < this.picSnapshots.Length)
          this.picSnapshots[index].Visible = false;
        if (index < this.cboTimeFrames.Length)
          this.cboTimeFrames[index].Visible = false;
        if (index < this.txtSnapshots.Length)
          this.txtSnapshots[index].Visible = false;
        if (index < this.lblSnapshots.Length)
          this.lblSnapshots[index].Visible = false;
        this.picLayouts[index].Enabled = false;
      }
      this.gcViewConfig.Text = "Dashboard View";
      this.cboViewFilter.SelectedIndex = 0;
      this.cboViewFilter.Enabled = false;
      this.txtViewFilter.Enabled = false;
      this.txtViewFilter.Text = "";
      this.btnViewFilter.Enabled = false;
      if (this.picSelectedLayout != null)
        this.picSelectedLayout.Parent.BackColor = SystemColors.Control;
      this.clearDataChanged();
    }

    private void fsExplorer_FolderChanged(object sender, EventArgs e)
    {
      this.checkForDataChanged();
      this.fileSystemEntry = (FileSystemEntry) null;
      this.clearUI();
    }

    private void fsExplorer_BeforeFolderRenamed(object sender, EventArgs e)
    {
      if (this.fileSystemEntry == null)
        return;
      this.checkForDataChanged();
    }

    private void ifsExplorer_OpenFileEvent(object sender, SelectedFileEventArgs e)
    {
      if (DashboardViewTemplateControl.ProcessingMode.SelectTemplate != this.processingMode && this.processingMode != DashboardViewTemplateControl.ProcessingMode.ManageTemplates)
        return;
      this.fileSystemEntry = e.FSEntry;
      if (this.btnSelect.Enabled)
      {
        this.btnExplorerOK_Click((object) null, (EventArgs) null);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please complete the configuration of this dashboard view before viewing it.");
      }
    }

    private void ifsExplorer_DeleteFileEvent(object sender, SelectedFileEventArgs e)
    {
      if (e.FSEntry.Type == FileSystemEntry.Types.File && this.dashboardView != null && this.dashboardView.ViewId > 0)
      {
        DashboardView.DeleteDashboardView(this.dashboardView.ViewId, this.session);
        this.dashboardView = (DashboardView) null;
      }
      this.clearUI();
      this.clearDataChanged();
    }

    private void ifsExplorer_MoveEntryEvent(object sender, SelectedFileEventArgs e)
    {
      if (this.fileSystemEntry == null || e.FSEntry.Type == FileSystemEntry.Types.Folder)
        return;
      this.getTemplate(e.FSEntry);
    }

    private void clearDataChanged()
    {
      this.dataChanged = false;
      this.btnSave.Enabled = false;
    }

    private void checkForDataChanged()
    {
      if (!this.dataChanged || DialogResult.Yes != Utils.Dialog((IWin32Window) this, "Do you want to save changes made to the current view?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
        return;
      this.updateView();
    }

    private void btnExplorerOK_Click(object sender, EventArgs e)
    {
      if (this.ParentForm == null)
        return;
      this.ParentForm.DialogResult = DialogResult.OK;
      this.ParentForm.Close();
    }

    private void btnExplorerCancel_Click(object sender, EventArgs e)
    {
      if (this.ParentForm == null)
        return;
      this.ParentForm.DialogResult = DialogResult.Cancel;
      this.ParentForm.Close();
    }

    private DashboardViewTemplate getSelectedTemplate()
    {
      DashboardViewTemplate selectedTemplate = (DashboardViewTemplate) null;
      FileSystemEntry selectedFileSystemEntry = this.getSelectedFileSystemEntry();
      if (selectedFileSystemEntry != null)
      {
        selectedTemplate = this.ifsExplorer.LoadDashboardViewTemplate(selectedFileSystemEntry);
        if (selectedTemplate == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The '" + this.fileSystemEntry.Name + "' template was not found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      return selectedTemplate;
    }

    private void setSelectedTemplate(FileSystemEntry fileSystemEntry)
    {
      if (fileSystemEntry == null)
        return;
      this.getTemplate(fileSystemEntry);
    }

    private void getTemplate(FileSystemEntry fileSystemEntry)
    {
      this.fileSystemEntry = fileSystemEntry;
      this.dashboardViewTemplate = this.ifsExplorer.LoadDashboardViewTemplate(fileSystemEntry);
      this.dashboardViewTemplate.TemplatePath = fileSystemEntry.ToString();
      if (this.dashboardViewTemplate == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The '" + this.fileSystemEntry.Name + "' snapshot was not found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.fileSystemEntry = (FileSystemEntry) null;
        this.clearUI();
      }
      else
      {
        if (this.oriDashboardView != null && this.dashboardViewTemplate.ViewGuid == this.oriDashboardView.Guid)
          this.oriFileSystemEntry = fileSystemEntry;
        DashboardView dashboardView = DashboardView.GetDashboardView(this.dashboardViewTemplate.ViewGuid, this.session);
        if (dashboardView == null)
        {
          dashboardView = DashboardView.NewDashboardView(this.session);
          dashboardView.Guid = this.dashboardViewTemplate.ViewGuid;
          dashboardView.ViewName = this.dashboardViewTemplate.Name;
          dashboardView.TemplatePath = this.dashboardViewTemplate.TemplatePath;
        }
        else
          dashboardView.ViewName = this.dashboardViewTemplate.Name;
        this.dashboardView = dashboardView;
        this.populateFormWithView();
      }
    }

    public FileSystemEntry OriFileSystemEntry => this.oriFileSystemEntry;

    private FileSystemEntry getSelectedFileSystemEntry()
    {
      if (this.fileSystemEntry == null)
      {
        GVSelectedItemCollection selectedItems = this.fsExplorer.SelectedItems;
        if (selectedItems.Count != 1 || FileSystemEntry.Types.File != ((FileSystemEntry) selectedItems[0].Tag).Type)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please select a DashboardView Template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
          this.fileSystemEntry = (FileSystemEntry) selectedItems[0].Tag;
      }
      return this.fileSystemEntry;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.fsExplorer.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a Dashboard View.");
      }
      else
      {
        if (this.dataChanged && DialogResult.Yes == Utils.Dialog((IWin32Window) this, "Do you want to save changes made to the current view?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
          this.btnSave.PerformClick();
        if (this.ParentForm == null)
          return;
        this.ParentForm.DialogResult = DialogResult.OK;
        this.ParentForm.Close();
      }
    }

    private void btnSelectView_Click(object sender, EventArgs e)
    {
      if (this.fsExplorer.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a Dashboard View.");
      }
      else
        this.btnSelect_Click((object) null, (EventArgs) null);
    }

    private void btnGoTo_Click(object sender, EventArgs e)
    {
      if (this.currentDefaultViewPath == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "There is no current view selected.");
      }
      else
      {
        this.fsExplorer.SetFolder(FileSystemEntry.Parse(this.currentDefaultViewPath));
        if (this.fsExplorer.SelectedItems.Count != 0)
          return;
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Default view cannot be found or no longer accessible to this user account.");
      }
    }

    private void btnCancelSelect_Click(object sender, EventArgs e)
    {
      if (this.ParentForm == null)
        return;
      this.ParentForm.DialogResult = DialogResult.Cancel;
    }

    private void DashboardViewTemplateForm_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      if (this.btnCancel.Visible)
        this.btnCancel.PerformClick();
      else
        this.btnCancelSelect.PerformClick();
    }

    private void fsExplorer_Leave(object sender, EventArgs e) => this.setLastFolderViewed();

    public FileSystemEntry SelectedFolder
    {
      get => this.fsExplorer.CurrentFolder;
      set => this.fsExplorer.SetFolder(value);
    }

    public string[] GetSelectedDashboardViewTemplates
    {
      get
      {
        return this.fsExplorer.SelectedItems.Count == 0 ? (string[]) null : this.fsExplorer.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (items => ((FileSystemEntry) items.Tag).ToString())).ToArray<string>();
      }
    }

    public string[] SelectedDashboardSnapshotTemplates
    {
      get
      {
        return this.fsExplorer.SelectedItems.Count == 0 ? (string[]) null : this.session.ReportManager.GetDashboardSnapshotPathsByViewTemplatePaths(this.fsExplorer.SelectedItems.Select<GVItem, FileSystemEntry>((Func<GVItem, FileSystemEntry>) (items => (FileSystemEntry) items.Tag)).ToArray<FileSystemEntry>());
      }
    }

    public string CheckForEmptyDashboardView()
    {
      foreach (GVItem selectedItem in this.fsExplorer.SelectedItems)
      {
        FileSystemEntry tag = (FileSystemEntry) selectedItem.Tag;
        if (tag.Type == FileSystemEntry.Types.File && !this.session.ReportManager.CheckDashboardViewExists(tag.ToString()))
          return tag.ToString();
      }
      return string.Empty;
    }

    public void HighlightDashboardViewTemplates(List<string> values)
    {
      if (values.Count == 0)
        return;
      for (int index = 0; index < values.Count; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.fsExplorer.GVItems.Count; ++nItemIndex)
        {
          if (values[index] == ((FileSystemEntry) this.fsExplorer.GVItems[nItemIndex].Tag).ToString())
          {
            this.fsExplorer.GVItems[nItemIndex].Selected = true;
            break;
          }
        }
      }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DashboardViewTemplateControl));
      this.pnlLeft = new Panel();
      this.pnlNavigate = new Panel();
      this.pnlSelectView = new Panel();
      this.btnCancelSelect = new Button();
      this.btnSelectView = new Button();
      this.gpDefaultView = new GradientPanel();
      this.btnGoTo = new Button();
      this.txtDefaultPath = new TextBox();
      this.label1 = new Label();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.pnlRight = new Panel();
      this.gcViewConfig = new GroupContainer();
      this.gradientPanel2 = new GradientPanel();
      this.gradientPanel1 = new GradientPanel();
      this.lblRoleSelection = new Label();
      this.cboViewFilter = new ComboBox();
      this.btnViewFilter = new StandardIconButton();
      this.txtViewFilter = new TextBox();
      this.pnlSelectLayout = new Panel();
      this.pnlLayout3 = new Panel();
      this.picLayout3 = new PictureBox();
      this.lblSelectLayout = new Label();
      this.pnlLayout18 = new Panel();
      this.picLayout18 = new PictureBox();
      this.pnlLayout1 = new Panel();
      this.picLayout1 = new PictureBox();
      this.pnlLayout16 = new Panel();
      this.picLayout16 = new PictureBox();
      this.pnlLayout2 = new Panel();
      this.picLayout2 = new PictureBox();
      this.pnlLayout15 = new Panel();
      this.picLayout15 = new PictureBox();
      this.pnlLayout17 = new Panel();
      this.picLayout17 = new PictureBox();
      this.pnlLayout14 = new Panel();
      this.picLayout14 = new PictureBox();
      this.pnlLayout4 = new Panel();
      this.picLayout4 = new PictureBox();
      this.pnlLayout13 = new Panel();
      this.picLayout13 = new PictureBox();
      this.pnlLayout5 = new Panel();
      this.picLayout5 = new PictureBox();
      this.pnlLayout12 = new Panel();
      this.picLayout12 = new PictureBox();
      this.pnlLayout6 = new Panel();
      this.picLayout6 = new PictureBox();
      this.pnlLayout11 = new Panel();
      this.picLayout11 = new PictureBox();
      this.pnlLayout7 = new Panel();
      this.picLayout7 = new PictureBox();
      this.pnlLayout10 = new Panel();
      this.picLayout10 = new PictureBox();
      this.pnlLayout8 = new Panel();
      this.picLayout8 = new PictureBox();
      this.pnlLayout9 = new Panel();
      this.picLayout9 = new PictureBox();
      this.pnlSelectSnapshot = new Panel();
      this.cboTimeFrame9 = new ComboBox();
      this.cboTimeFrame8 = new ComboBox();
      this.cboTimeFrame7 = new ComboBox();
      this.cboTimeFrame6 = new ComboBox();
      this.cboTimeFrame5 = new ComboBox();
      this.cboTimeFrame4 = new ComboBox();
      this.cboTimeFrame3 = new ComboBox();
      this.cboTimeFrame2 = new ComboBox();
      this.cboTimeFrame1 = new ComboBox();
      this.lblSelectSnapshot = new Label();
      this.lblSnapshot1 = new Label();
      this.lblSnapshot2 = new Label();
      this.lblSnapshot3 = new Label();
      this.lblSnapshot4 = new Label();
      this.picSnapshot9 = new PictureBox();
      this.lblSnapshot5 = new Label();
      this.picSnapshot8 = new PictureBox();
      this.lblSnapshot6 = new Label();
      this.picSnapshot7 = new PictureBox();
      this.lblSnapshot7 = new Label();
      this.picSnapshot6 = new PictureBox();
      this.lblSnapshot8 = new Label();
      this.picSnapshot5 = new PictureBox();
      this.lblSnapshot9 = new Label();
      this.picSnapshot4 = new PictureBox();
      this.txtSnapshot1 = new TextBox();
      this.picSnapshot3 = new PictureBox();
      this.txtSnapshot2 = new TextBox();
      this.picSnapshot2 = new PictureBox();
      this.txtSnapshot3 = new TextBox();
      this.picSnapshot1 = new PictureBox();
      this.txtSnapshot4 = new TextBox();
      this.txtSnapshot9 = new TextBox();
      this.txtSnapshot5 = new TextBox();
      this.txtSnapshot8 = new TextBox();
      this.txtSnapshot6 = new TextBox();
      this.txtSnapshot7 = new TextBox();
      this.btnSelect = new Button();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.imgLstSearch = new ImageList(this.components);
      this.tipToolTip = new ToolTip(this.components);
      this.imglstLayouts = new ImageList(this.components);
      this.pnlLeft.SuspendLayout();
      this.pnlNavigate.SuspendLayout();
      this.pnlSelectView.SuspendLayout();
      this.gpDefaultView.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.gcViewConfig.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnViewFilter).BeginInit();
      this.pnlSelectLayout.SuspendLayout();
      this.pnlLayout3.SuspendLayout();
      ((ISupportInitialize) this.picLayout3).BeginInit();
      this.pnlLayout18.SuspendLayout();
      ((ISupportInitialize) this.picLayout18).BeginInit();
      this.pnlLayout1.SuspendLayout();
      ((ISupportInitialize) this.picLayout1).BeginInit();
      this.pnlLayout16.SuspendLayout();
      ((ISupportInitialize) this.picLayout16).BeginInit();
      this.pnlLayout2.SuspendLayout();
      ((ISupportInitialize) this.picLayout2).BeginInit();
      this.pnlLayout15.SuspendLayout();
      ((ISupportInitialize) this.picLayout15).BeginInit();
      this.pnlLayout17.SuspendLayout();
      ((ISupportInitialize) this.picLayout17).BeginInit();
      this.pnlLayout14.SuspendLayout();
      ((ISupportInitialize) this.picLayout14).BeginInit();
      this.pnlLayout4.SuspendLayout();
      ((ISupportInitialize) this.picLayout4).BeginInit();
      this.pnlLayout13.SuspendLayout();
      ((ISupportInitialize) this.picLayout13).BeginInit();
      this.pnlLayout5.SuspendLayout();
      ((ISupportInitialize) this.picLayout5).BeginInit();
      this.pnlLayout12.SuspendLayout();
      ((ISupportInitialize) this.picLayout12).BeginInit();
      this.pnlLayout6.SuspendLayout();
      ((ISupportInitialize) this.picLayout6).BeginInit();
      this.pnlLayout11.SuspendLayout();
      ((ISupportInitialize) this.picLayout11).BeginInit();
      this.pnlLayout7.SuspendLayout();
      ((ISupportInitialize) this.picLayout7).BeginInit();
      this.pnlLayout10.SuspendLayout();
      ((ISupportInitialize) this.picLayout10).BeginInit();
      this.pnlLayout8.SuspendLayout();
      ((ISupportInitialize) this.picLayout8).BeginInit();
      this.pnlLayout9.SuspendLayout();
      ((ISupportInitialize) this.picLayout9).BeginInit();
      this.pnlSelectSnapshot.SuspendLayout();
      ((ISupportInitialize) this.picSnapshot9).BeginInit();
      ((ISupportInitialize) this.picSnapshot8).BeginInit();
      ((ISupportInitialize) this.picSnapshot7).BeginInit();
      ((ISupportInitialize) this.picSnapshot6).BeginInit();
      ((ISupportInitialize) this.picSnapshot5).BeginInit();
      ((ISupportInitialize) this.picSnapshot4).BeginInit();
      ((ISupportInitialize) this.picSnapshot3).BeginInit();
      ((ISupportInitialize) this.picSnapshot2).BeginInit();
      ((ISupportInitialize) this.picSnapshot1).BeginInit();
      this.SuspendLayout();
      this.pnlLeft.Controls.Add((Control) this.pnlNavigate);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(257, 608);
      this.pnlLeft.TabIndex = 0;
      this.pnlNavigate.Controls.Add((Control) this.fsExplorer);
      this.pnlNavigate.Controls.Add((Control) this.pnlSelectView);
      this.pnlNavigate.Controls.Add((Control) this.gpDefaultView);
      this.pnlNavigate.Dock = DockStyle.Fill;
      this.pnlNavigate.Location = new Point(0, 0);
      this.pnlNavigate.Name = "pnlNavigate";
      this.pnlNavigate.Size = new Size(257, 608);
      this.pnlNavigate.TabIndex = 446;
      this.fsExplorer.Dock = DockStyle.Fill;
      this.fsExplorer.FolderComboSelectedIndex = -1;
      this.fsExplorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fsExplorer.HasPublicRight = true;
      this.fsExplorer.Location = new Point(0, 31);
      this.fsExplorer.Name = "fsExplorer";
      this.fsExplorer.RenameButtonSize = new Size(62, 22);
      this.fsExplorer.RESPAMode = FSExplorer.RESPAFilter.All;
      this.fsExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.fsExplorer.Size = new Size(257, 537);
      this.fsExplorer.TabIndex = 0;
      this.pnlSelectView.Controls.Add((Control) this.btnCancelSelect);
      this.pnlSelectView.Controls.Add((Control) this.btnSelectView);
      this.pnlSelectView.Dock = DockStyle.Bottom;
      this.pnlSelectView.Location = new Point(0, 568);
      this.pnlSelectView.Name = "pnlSelectView";
      this.pnlSelectView.Size = new Size(257, 40);
      this.pnlSelectView.TabIndex = 446;
      this.btnCancelSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancelSelect.BackColor = SystemColors.Control;
      this.btnCancelSelect.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnCancelSelect.Location = new Point(180, 14);
      this.btnCancelSelect.Name = "btnCancelSelect";
      this.btnCancelSelect.Size = new Size(67, 20);
      this.btnCancelSelect.TabIndex = 445;
      this.btnCancelSelect.Text = "Cancel";
      this.btnCancelSelect.UseCompatibleTextRendering = true;
      this.btnCancelSelect.UseVisualStyleBackColor = true;
      this.btnCancelSelect.Click += new EventHandler(this.btnCancelSelect_Click);
      this.btnSelectView.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelectView.BackColor = SystemColors.Control;
      this.btnSelectView.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSelectView.Location = new Point(107, 14);
      this.btnSelectView.Name = "btnSelectView";
      this.btnSelectView.Size = new Size(67, 20);
      this.btnSelectView.TabIndex = 444;
      this.btnSelectView.Text = "Select";
      this.btnSelectView.UseCompatibleTextRendering = true;
      this.btnSelectView.UseVisualStyleBackColor = true;
      this.btnSelectView.Click += new EventHandler(this.btnSelectView_Click);
      this.gpDefaultView.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gpDefaultView.Controls.Add((Control) this.btnGoTo);
      this.gpDefaultView.Controls.Add((Control) this.txtDefaultPath);
      this.gpDefaultView.Controls.Add((Control) this.label1);
      this.gpDefaultView.Dock = DockStyle.Top;
      this.gpDefaultView.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gpDefaultView.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gpDefaultView.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gpDefaultView.Location = new Point(0, 0);
      this.gpDefaultView.Name = "gpDefaultView";
      this.gpDefaultView.Size = new Size(257, 31);
      this.gpDefaultView.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gpDefaultView.TabIndex = 445;
      this.btnGoTo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnGoTo.BackColor = SystemColors.Control;
      this.btnGoTo.Location = new Point(207, 5);
      this.btnGoTo.Name = "btnGoTo";
      this.btnGoTo.Size = new Size(46, 22);
      this.btnGoTo.TabIndex = 2;
      this.btnGoTo.Text = "Go To";
      this.btnGoTo.UseVisualStyleBackColor = true;
      this.btnGoTo.Click += new EventHandler(this.btnGoTo_Click);
      this.txtDefaultPath.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.txtDefaultPath.Location = new Point(52, 6);
      this.txtDefaultPath.Name = "txtDefaultPath";
      this.txtDefaultPath.ReadOnly = true;
      this.txtDefaultPath.Size = new Size(148, 20);
      this.txtDefaultPath.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(5, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(41, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Default";
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.pnlLeft;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(257, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 1;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.pnlRight.BackColor = Color.WhiteSmoke;
      this.pnlRight.Controls.Add((Control) this.gcViewConfig);
      this.pnlRight.Controls.Add((Control) this.btnSelect);
      this.pnlRight.Controls.Add((Control) this.btnSave);
      this.pnlRight.Controls.Add((Control) this.btnCancel);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(264, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(650, 608);
      this.pnlRight.TabIndex = 2;
      this.gcViewConfig.Controls.Add((Control) this.gradientPanel2);
      this.gcViewConfig.Controls.Add((Control) this.gradientPanel1);
      this.gcViewConfig.Controls.Add((Control) this.pnlSelectLayout);
      this.gcViewConfig.Controls.Add((Control) this.pnlSelectSnapshot);
      this.gcViewConfig.Dock = DockStyle.Top;
      this.gcViewConfig.HeaderForeColor = SystemColors.ControlText;
      this.gcViewConfig.Location = new Point(0, 0);
      this.gcViewConfig.Name = "gcViewConfig";
      this.gcViewConfig.Size = new Size(650, 583);
      this.gcViewConfig.TabIndex = 444;
      this.gcViewConfig.Text = "Dashboard View";
      this.gradientPanel2.BackColor = Color.Transparent;
      this.gradientPanel2.Location = new Point(306, 58);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(1, 555);
      this.gradientPanel2.TabIndex = 444;
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.lblRoleSelection);
      this.gradientPanel1.Controls.Add((Control) this.cboViewFilter);
      this.gradientPanel1.Controls.Add((Control) this.btnViewFilter);
      this.gradientPanel1.Controls.Add((Control) this.txtViewFilter);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(648, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 443;
      this.lblRoleSelection.AutoSize = true;
      this.lblRoleSelection.BackColor = Color.Transparent;
      this.lblRoleSelection.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.lblRoleSelection.Location = new Point(8, 8);
      this.lblRoleSelection.Name = "lblRoleSelection";
      this.lblRoleSelection.Size = new Size(80, 14);
      this.lblRoleSelection.TabIndex = 25;
      this.lblRoleSelection.Text = "Show Data For";
      this.lblRoleSelection.TextAlign = ContentAlignment.MiddleLeft;
      this.cboViewFilter.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboViewFilter.Enabled = false;
      this.cboViewFilter.Font = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboViewFilter.Items.AddRange(new object[3]
      {
        (object) "All",
        (object) "Loan Officer",
        (object) "Loan Processor"
      });
      this.cboViewFilter.Location = new Point(94, 4);
      this.cboViewFilter.Name = "cboViewFilter";
      this.cboViewFilter.Size = new Size(140, 22);
      this.cboViewFilter.TabIndex = 22;
      this.cboViewFilter.SelectionChangeCommitted += new EventHandler(this.cboViewFilter_SelectionChangeCommitted);
      this.btnViewFilter.BackColor = Color.Transparent;
      this.btnViewFilter.Location = new Point(407, 6);
      this.btnViewFilter.MouseDownImage = (Image) null;
      this.btnViewFilter.Name = "btnViewFilter";
      this.btnViewFilter.Size = new Size(16, 16);
      this.btnViewFilter.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnViewFilter.TabIndex = 24;
      this.btnViewFilter.TabStop = false;
      this.tipToolTip.SetToolTip((Control) this.btnViewFilter, "Lookup");
      this.btnViewFilter.Click += new EventHandler(this.picViewFilter_Click);
      this.txtViewFilter.Location = new Point(240, 5);
      this.txtViewFilter.Name = "txtViewFilter";
      this.txtViewFilter.ReadOnly = true;
      this.txtViewFilter.Size = new Size(161, 20);
      this.txtViewFilter.TabIndex = 23;
      this.pnlSelectLayout.BackColor = Color.Transparent;
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout3);
      this.pnlSelectLayout.Controls.Add((Control) this.lblSelectLayout);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout18);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout1);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout16);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout2);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout15);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout17);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout14);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout4);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout13);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout5);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout12);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout6);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout11);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout7);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout10);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout8);
      this.pnlSelectLayout.Controls.Add((Control) this.pnlLayout9);
      this.pnlSelectLayout.Location = new Point(20, 72);
      this.pnlSelectLayout.Name = "pnlSelectLayout";
      this.pnlSelectLayout.Size = new Size(276, 441);
      this.pnlSelectLayout.TabIndex = 441;
      this.pnlLayout3.BackColor = SystemColors.Control;
      this.pnlLayout3.Controls.Add((Control) this.picLayout3);
      this.pnlLayout3.Location = new Point(188, 27);
      this.pnlLayout3.Name = "pnlLayout3";
      this.pnlLayout3.Size = new Size(88, 64);
      this.pnlLayout3.TabIndex = 3;
      this.pnlLayout3.Tag = (object) "";
      this.picLayout3.BackColor = SystemColors.Control;
      this.picLayout3.Image = (Image) componentResourceManager.GetObject("picLayout3.Image");
      this.picLayout3.Location = new Point(2, 2);
      this.picLayout3.Name = "picLayout3";
      this.picLayout3.Size = new Size(84, 60);
      this.picLayout3.TabIndex = 38;
      this.picLayout3.TabStop = false;
      this.picLayout3.Tag = (object) "C2R1R1";
      this.picLayout3.Click += new EventHandler(this.picLayout_Click);
      this.picLayout3.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout3.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.lblSelectLayout.AutoSize = true;
      this.lblSelectLayout.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSelectLayout.Location = new Point(0, 0);
      this.lblSelectLayout.Name = "lblSelectLayout";
      this.lblSelectLayout.Size = new Size(115, 13);
      this.lblSelectLayout.TabIndex = 7;
      this.lblSelectLayout.Text = "1. Select a Layout.";
      this.lblSelectLayout.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlLayout18.BackColor = SystemColors.Control;
      this.pnlLayout18.Controls.Add((Control) this.picLayout18);
      this.pnlLayout18.Location = new Point(188, 377);
      this.pnlLayout18.Name = "pnlLayout18";
      this.pnlLayout18.Size = new Size(88, 64);
      this.pnlLayout18.TabIndex = 18;
      this.pnlLayout18.Tag = (object) "";
      this.picLayout18.BackColor = SystemColors.Control;
      this.picLayout18.Image = (Image) componentResourceManager.GetObject("picLayout18.Image");
      this.picLayout18.ImageLocation = "";
      this.picLayout18.Location = new Point(2, 2);
      this.picLayout18.Name = "picLayout18";
      this.picLayout18.Size = new Size(84, 60);
      this.picLayout18.TabIndex = 59;
      this.picLayout18.TabStop = false;
      this.picLayout18.Tag = (object) "C3R3R3R3";
      this.picLayout18.Click += new EventHandler(this.picLayout_Click);
      this.picLayout18.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout18.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout1.BackColor = SystemColors.Control;
      this.pnlLayout1.Controls.Add((Control) this.picLayout1);
      this.pnlLayout1.Location = new Point(0, 27);
      this.pnlLayout1.Name = "pnlLayout1";
      this.pnlLayout1.Size = new Size(88, 64);
      this.pnlLayout1.TabIndex = 1;
      this.pnlLayout1.Tag = (object) "";
      this.picLayout1.BackColor = SystemColors.Control;
      this.picLayout1.Image = (Image) componentResourceManager.GetObject("picLayout1.Image");
      this.picLayout1.Location = new Point(2, 2);
      this.picLayout1.Name = "picLayout1";
      this.picLayout1.Size = new Size(84, 60);
      this.picLayout1.TabIndex = 34;
      this.picLayout1.TabStop = false;
      this.picLayout1.Tag = (object) "C1R1";
      this.picLayout1.Click += new EventHandler(this.picLayout_Click);
      this.picLayout1.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout1.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout16.Controls.Add((Control) this.picLayout16);
      this.pnlLayout16.Location = new Point(0, 377);
      this.pnlLayout16.Name = "pnlLayout16";
      this.pnlLayout16.Size = new Size(88, 64);
      this.pnlLayout16.TabIndex = 16;
      this.picLayout16.BackColor = SystemColors.Control;
      this.picLayout16.Image = (Image) componentResourceManager.GetObject("picLayout16.Image");
      this.picLayout16.Location = new Point(2, 2);
      this.picLayout16.Name = "picLayout16";
      this.picLayout16.Size = new Size(84, 60);
      this.picLayout16.TabIndex = 425;
      this.picLayout16.TabStop = false;
      this.picLayout16.Tag = (object) "C3R2R2R2";
      this.picLayout16.Click += new EventHandler(this.picLayout_Click);
      this.picLayout16.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout16.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout2.BackColor = SystemColors.Control;
      this.pnlLayout2.Controls.Add((Control) this.picLayout2);
      this.pnlLayout2.Location = new Point(94, 27);
      this.pnlLayout2.Name = "pnlLayout2";
      this.pnlLayout2.Size = new Size(88, 64);
      this.pnlLayout2.TabIndex = 2;
      this.pnlLayout2.Tag = (object) "";
      this.picLayout2.BackColor = SystemColors.Control;
      this.picLayout2.Image = (Image) componentResourceManager.GetObject("picLayout2.Image");
      this.picLayout2.Location = new Point(2, 2);
      this.picLayout2.Name = "picLayout2";
      this.picLayout2.Size = new Size(84, 60);
      this.picLayout2.TabIndex = 35;
      this.picLayout2.TabStop = false;
      this.picLayout2.Tag = (object) "C1R2";
      this.picLayout2.Click += new EventHandler(this.picLayout_Click);
      this.picLayout2.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout2.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout15.Controls.Add((Control) this.picLayout15);
      this.pnlLayout15.Location = new Point(188, 307);
      this.pnlLayout15.Name = "pnlLayout15";
      this.pnlLayout15.Size = new Size(88, 64);
      this.pnlLayout15.TabIndex = 15;
      this.picLayout15.BackColor = SystemColors.Control;
      this.picLayout15.Image = (Image) componentResourceManager.GetObject("picLayout15.Image");
      this.picLayout15.Location = new Point(2, 2);
      this.picLayout15.Name = "picLayout15";
      this.picLayout15.Size = new Size(84, 60);
      this.picLayout15.TabIndex = 58;
      this.picLayout15.TabStop = false;
      this.picLayout15.Tag = (object) "C2R3R3W6";
      this.picLayout15.Click += new EventHandler(this.picLayout_Click);
      this.picLayout15.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout15.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout17.Controls.Add((Control) this.picLayout17);
      this.pnlLayout17.Location = new Point(94, 377);
      this.pnlLayout17.Name = "pnlLayout17";
      this.pnlLayout17.Size = new Size(88, 64);
      this.pnlLayout17.TabIndex = 17;
      this.picLayout17.BackColor = SystemColors.Control;
      this.picLayout17.Image = (Image) componentResourceManager.GetObject("picLayout17.Image");
      this.picLayout17.InitialImage = (Image) componentResourceManager.GetObject("picLayout17.InitialImage");
      this.picLayout17.Location = new Point(2, 2);
      this.picLayout17.Name = "picLayout17";
      this.picLayout17.Size = new Size(84, 60);
      this.picLayout17.TabIndex = 426;
      this.picLayout17.TabStop = false;
      this.picLayout17.Tag = (object) "R2C2C3";
      this.picLayout17.Click += new EventHandler(this.picLayout_Click);
      this.picLayout17.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout17.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout14.Controls.Add((Control) this.picLayout14);
      this.pnlLayout14.Location = new Point(94, 307);
      this.pnlLayout14.Name = "pnlLayout14";
      this.pnlLayout14.Size = new Size(88, 64);
      this.pnlLayout14.TabIndex = 14;
      this.picLayout14.BackColor = SystemColors.Control;
      this.picLayout14.Image = (Image) componentResourceManager.GetObject("picLayout14.Image");
      this.picLayout14.Location = new Point(2, 2);
      this.picLayout14.Name = "picLayout14";
      this.picLayout14.Size = new Size(84, 60);
      this.picLayout14.TabIndex = 57;
      this.picLayout14.TabStop = false;
      this.picLayout14.Tag = (object) "C2R3R3W4";
      this.picLayout14.Click += new EventHandler(this.picLayout_Click);
      this.picLayout14.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout14.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout4.Controls.Add((Control) this.picLayout4);
      this.pnlLayout4.Location = new Point(0, 97);
      this.pnlLayout4.Name = "pnlLayout4";
      this.pnlLayout4.Size = new Size(88, 64);
      this.pnlLayout4.TabIndex = 4;
      this.pnlLayout4.Tag = (object) "";
      this.picLayout4.BackColor = SystemColors.Control;
      this.picLayout4.Image = (Image) componentResourceManager.GetObject("picLayout4.Image");
      this.picLayout4.Location = new Point(2, 2);
      this.picLayout4.Name = "picLayout4";
      this.picLayout4.Size = new Size(84, 60);
      this.picLayout4.TabIndex = 39;
      this.picLayout4.TabStop = false;
      this.picLayout4.Tag = (object) "C2R2R2";
      this.picLayout4.Click += new EventHandler(this.picLayout_Click);
      this.picLayout4.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout4.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout13.Controls.Add((Control) this.picLayout13);
      this.pnlLayout13.Location = new Point(0, 307);
      this.pnlLayout13.Name = "pnlLayout13";
      this.pnlLayout13.Size = new Size(88, 64);
      this.pnlLayout13.TabIndex = 13;
      this.picLayout13.BackColor = SystemColors.Control;
      this.picLayout13.Image = (Image) componentResourceManager.GetObject("picLayout13.Image");
      this.picLayout13.Location = new Point(2, 2);
      this.picLayout13.Name = "picLayout13";
      this.picLayout13.Size = new Size(84, 60);
      this.picLayout13.TabIndex = 56;
      this.picLayout13.TabStop = false;
      this.picLayout13.Tag = (object) "C2R3R3";
      this.picLayout13.Click += new EventHandler(this.picLayout_Click);
      this.picLayout13.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout13.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout5.Controls.Add((Control) this.picLayout5);
      this.pnlLayout5.Location = new Point(94, 97);
      this.pnlLayout5.Name = "pnlLayout5";
      this.pnlLayout5.Size = new Size(88, 64);
      this.pnlLayout5.TabIndex = 5;
      this.pnlLayout5.Tag = (object) "";
      this.picLayout5.BackColor = SystemColors.Control;
      this.picLayout5.Image = (Image) componentResourceManager.GetObject("picLayout5.Image");
      this.picLayout5.Location = new Point(2, 2);
      this.picLayout5.Name = "picLayout5";
      this.picLayout5.Size = new Size(84, 60);
      this.picLayout5.TabIndex = 45;
      this.picLayout5.TabStop = false;
      this.picLayout5.Tag = (object) "C2R2R2W4";
      this.picLayout5.Click += new EventHandler(this.picLayout_Click);
      this.picLayout5.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout5.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout12.Controls.Add((Control) this.picLayout12);
      this.pnlLayout12.Location = new Point(188, 237);
      this.pnlLayout12.Name = "pnlLayout12";
      this.pnlLayout12.Size = new Size(88, 64);
      this.pnlLayout12.TabIndex = 12;
      this.picLayout12.BackColor = SystemColors.Control;
      this.picLayout12.Image = (Image) componentResourceManager.GetObject("picLayout12.Image");
      this.picLayout12.Location = new Point(2, 2);
      this.picLayout12.Name = "picLayout12";
      this.picLayout12.Size = new Size(84, 60);
      this.picLayout12.TabIndex = 54;
      this.picLayout12.TabStop = false;
      this.picLayout12.Tag = (object) "C2R2R3W6";
      this.picLayout12.Click += new EventHandler(this.picLayout_Click);
      this.picLayout12.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout12.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout6.Controls.Add((Control) this.picLayout6);
      this.pnlLayout6.Location = new Point(188, 97);
      this.pnlLayout6.Name = "pnlLayout6";
      this.pnlLayout6.Size = new Size(88, 64);
      this.pnlLayout6.TabIndex = 6;
      this.pnlLayout6.Tag = (object) "";
      this.picLayout6.BackColor = SystemColors.Control;
      this.picLayout6.Image = (Image) componentResourceManager.GetObject("picLayout6.Image");
      this.picLayout6.Location = new Point(2, 2);
      this.picLayout6.Name = "picLayout6";
      this.picLayout6.Size = new Size(84, 60);
      this.picLayout6.TabIndex = 46;
      this.picLayout6.TabStop = false;
      this.picLayout6.Tag = (object) "C2R2R2W6";
      this.picLayout6.Click += new EventHandler(this.picLayout_Click);
      this.picLayout6.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout6.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout11.Controls.Add((Control) this.picLayout11);
      this.pnlLayout11.Location = new Point(94, 237);
      this.pnlLayout11.Name = "pnlLayout11";
      this.pnlLayout11.Size = new Size(88, 64);
      this.pnlLayout11.TabIndex = 11;
      this.picLayout11.BackColor = SystemColors.Control;
      this.picLayout11.Image = (Image) componentResourceManager.GetObject("picLayout11.Image");
      this.picLayout11.Location = new Point(2, 2);
      this.picLayout11.Name = "picLayout11";
      this.picLayout11.Size = new Size(84, 60);
      this.picLayout11.TabIndex = 53;
      this.picLayout11.TabStop = false;
      this.picLayout11.Tag = (object) "C2R2R3W4";
      this.picLayout11.Click += new EventHandler(this.picLayout_Click);
      this.picLayout11.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout11.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout7.Controls.Add((Control) this.picLayout7);
      this.pnlLayout7.Location = new Point(0, 167);
      this.pnlLayout7.Name = "pnlLayout7";
      this.pnlLayout7.Size = new Size(88, 64);
      this.pnlLayout7.TabIndex = 7;
      this.picLayout7.BackColor = SystemColors.Control;
      this.picLayout7.Image = (Image) componentResourceManager.GetObject("picLayout7.Image");
      this.picLayout7.Location = new Point(2, 2);
      this.picLayout7.Name = "picLayout7";
      this.picLayout7.Size = new Size(84, 60);
      this.picLayout7.TabIndex = 52;
      this.picLayout7.TabStop = false;
      this.picLayout7.Tag = (object) "C2R3R2";
      this.picLayout7.Click += new EventHandler(this.picLayout_Click);
      this.picLayout7.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout7.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout10.Controls.Add((Control) this.picLayout10);
      this.pnlLayout10.Location = new Point(0, 237);
      this.pnlLayout10.Name = "pnlLayout10";
      this.pnlLayout10.Size = new Size(88, 64);
      this.pnlLayout10.TabIndex = 10;
      this.picLayout10.BackColor = SystemColors.Control;
      this.picLayout10.Image = (Image) componentResourceManager.GetObject("picLayout10.Image");
      this.picLayout10.Location = new Point(2, 2);
      this.picLayout10.Name = "picLayout10";
      this.picLayout10.Size = new Size(84, 60);
      this.picLayout10.TabIndex = 55;
      this.picLayout10.TabStop = false;
      this.picLayout10.Tag = (object) "C2R2R3";
      this.picLayout10.Click += new EventHandler(this.picLayout_Click);
      this.picLayout10.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout10.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout8.Controls.Add((Control) this.picLayout8);
      this.pnlLayout8.Location = new Point(94, 167);
      this.pnlLayout8.Name = "pnlLayout8";
      this.pnlLayout8.Size = new Size(88, 64);
      this.pnlLayout8.TabIndex = 8;
      this.picLayout8.BackColor = SystemColors.Control;
      this.picLayout8.Image = (Image) componentResourceManager.GetObject("picLayout8.Image");
      this.picLayout8.Location = new Point(2, 2);
      this.picLayout8.Name = "picLayout8";
      this.picLayout8.Size = new Size(84, 60);
      this.picLayout8.TabIndex = 50;
      this.picLayout8.TabStop = false;
      this.picLayout8.Tag = (object) "C2R3R2W4";
      this.picLayout8.Click += new EventHandler(this.picLayout_Click);
      this.picLayout8.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout8.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlLayout9.Controls.Add((Control) this.picLayout9);
      this.pnlLayout9.Location = new Point(188, 167);
      this.pnlLayout9.Name = "pnlLayout9";
      this.pnlLayout9.Size = new Size(88, 64);
      this.pnlLayout9.TabIndex = 9;
      this.picLayout9.BackColor = SystemColors.Control;
      this.picLayout9.Image = (Image) componentResourceManager.GetObject("picLayout9.Image");
      this.picLayout9.Location = new Point(2, 2);
      this.picLayout9.Name = "picLayout9";
      this.picLayout9.Size = new Size(84, 60);
      this.picLayout9.TabIndex = 51;
      this.picLayout9.TabStop = false;
      this.picLayout9.Tag = (object) "C2R3R2W6";
      this.picLayout9.Click += new EventHandler(this.picLayout_Click);
      this.picLayout9.MouseEnter += new EventHandler(this.picLayout_MouseEnter);
      this.picLayout9.MouseLeave += new EventHandler(this.picLayout_MouseLeave);
      this.pnlSelectSnapshot.BackColor = Color.Transparent;
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame9);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame8);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame7);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame6);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame5);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame4);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame3);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame2);
      this.pnlSelectSnapshot.Controls.Add((Control) this.cboTimeFrame1);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSelectSnapshot);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot1);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot2);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot3);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot4);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot9);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot5);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot8);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot6);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot7);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot7);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot6);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot8);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot5);
      this.pnlSelectSnapshot.Controls.Add((Control) this.lblSnapshot9);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot4);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot1);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot3);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot2);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot2);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot3);
      this.pnlSelectSnapshot.Controls.Add((Control) this.picSnapshot1);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot4);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot9);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot5);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot8);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot6);
      this.pnlSelectSnapshot.Controls.Add((Control) this.txtSnapshot7);
      this.pnlSelectSnapshot.Location = new Point(317, 72);
      this.pnlSelectSnapshot.Name = "pnlSelectSnapshot";
      this.pnlSelectSnapshot.Size = new Size(293, 503);
      this.pnlSelectSnapshot.TabIndex = 442;
      this.cboTimeFrame9.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame9.FormattingEnabled = true;
      this.cboTimeFrame9.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame9.Location = new Point(72, 477);
      this.cboTimeFrame9.Name = "cboTimeFrame9";
      this.cboTimeFrame9.Size = new Size(199, 21);
      this.cboTimeFrame9.TabIndex = 433;
      this.cboTimeFrame9.Tag = (object) "8";
      this.cboTimeFrame9.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame8.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame8.FormattingEnabled = true;
      this.cboTimeFrame8.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame8.Location = new Point(72, 424);
      this.cboTimeFrame8.Name = "cboTimeFrame8";
      this.cboTimeFrame8.Size = new Size(199, 21);
      this.cboTimeFrame8.TabIndex = 432;
      this.cboTimeFrame8.Tag = (object) "7";
      this.cboTimeFrame8.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame7.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame7.FormattingEnabled = true;
      this.cboTimeFrame7.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame7.Location = new Point(72, 371);
      this.cboTimeFrame7.Name = "cboTimeFrame7";
      this.cboTimeFrame7.Size = new Size(199, 21);
      this.cboTimeFrame7.TabIndex = 431;
      this.cboTimeFrame7.Tag = (object) "6";
      this.cboTimeFrame7.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame6.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame6.FormattingEnabled = true;
      this.cboTimeFrame6.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame6.Location = new Point(72, 318);
      this.cboTimeFrame6.Name = "cboTimeFrame6";
      this.cboTimeFrame6.Size = new Size(199, 21);
      this.cboTimeFrame6.TabIndex = 430;
      this.cboTimeFrame6.Tag = (object) "5";
      this.cboTimeFrame6.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame5.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame5.FormattingEnabled = true;
      this.cboTimeFrame5.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame5.Location = new Point(72, 265);
      this.cboTimeFrame5.Name = "cboTimeFrame5";
      this.cboTimeFrame5.Size = new Size(199, 21);
      this.cboTimeFrame5.TabIndex = 429;
      this.cboTimeFrame5.Tag = (object) "4";
      this.cboTimeFrame5.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame4.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame4.FormattingEnabled = true;
      this.cboTimeFrame4.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame4.Location = new Point(72, 212);
      this.cboTimeFrame4.Name = "cboTimeFrame4";
      this.cboTimeFrame4.Size = new Size(199, 21);
      this.cboTimeFrame4.TabIndex = 428;
      this.cboTimeFrame4.Tag = (object) "3";
      this.cboTimeFrame4.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame3.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame3.FormattingEnabled = true;
      this.cboTimeFrame3.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame3.Location = new Point(72, 159);
      this.cboTimeFrame3.Name = "cboTimeFrame3";
      this.cboTimeFrame3.Size = new Size(199, 21);
      this.cboTimeFrame3.TabIndex = 427;
      this.cboTimeFrame3.Tag = (object) "2";
      this.cboTimeFrame3.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame2.FormattingEnabled = true;
      this.cboTimeFrame2.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame2.Location = new Point(72, 106);
      this.cboTimeFrame2.Name = "cboTimeFrame2";
      this.cboTimeFrame2.Size = new Size(199, 21);
      this.cboTimeFrame2.TabIndex = 426;
      this.cboTimeFrame2.Tag = (object) "1";
      this.cboTimeFrame2.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.cboTimeFrame1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame1.FormattingEnabled = true;
      this.cboTimeFrame1.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame1.Location = new Point(72, 53);
      this.cboTimeFrame1.Name = "cboTimeFrame1";
      this.cboTimeFrame1.Size = new Size(199, 21);
      this.cboTimeFrame1.TabIndex = 425;
      this.cboTimeFrame1.Tag = (object) "0";
      this.cboTimeFrame1.SelectedIndexChanged += new EventHandler(this.cboTimeFrame_SelectedIndexChanged);
      this.lblSelectSnapshot.AutoSize = true;
      this.lblSelectSnapshot.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSelectSnapshot.Location = new Point(3, 0);
      this.lblSelectSnapshot.Name = "lblSelectSnapshot";
      this.lblSelectSnapshot.Size = new Size(225, 13);
      this.lblSelectSnapshot.TabIndex = 8;
      this.lblSelectSnapshot.Text = "2. Select a Snapshot for each module.";
      this.lblSelectSnapshot.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot1.AutoSize = true;
      this.lblSnapshot1.Location = new Point(1, 31);
      this.lblSnapshot1.Name = "lblSnapshot1";
      this.lblSnapshot1.Size = new Size(61, 13);
      this.lblSnapshot1.TabIndex = 19;
      this.lblSnapshot1.Text = "Snapshot 1";
      this.lblSnapshot1.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot2.AutoSize = true;
      this.lblSnapshot2.Location = new Point(1, 85);
      this.lblSnapshot2.Name = "lblSnapshot2";
      this.lblSnapshot2.Size = new Size(61, 13);
      this.lblSnapshot2.TabIndex = 20;
      this.lblSnapshot2.Text = "Snapshot 2";
      this.lblSnapshot2.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot3.AutoSize = true;
      this.lblSnapshot3.Location = new Point(1, 138);
      this.lblSnapshot3.Name = "lblSnapshot3";
      this.lblSnapshot3.Size = new Size(61, 13);
      this.lblSnapshot3.TabIndex = 21;
      this.lblSnapshot3.Text = "Snapshot 3";
      this.lblSnapshot3.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSnapshot4.AutoSize = true;
      this.lblSnapshot4.Location = new Point(1, 191);
      this.lblSnapshot4.Name = "lblSnapshot4";
      this.lblSnapshot4.Size = new Size(61, 13);
      this.lblSnapshot4.TabIndex = 22;
      this.lblSnapshot4.Text = "Snapshot 4";
      this.lblSnapshot4.TextAlign = ContentAlignment.MiddleLeft;
      this.picSnapshot9.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot9.Image = (Image) componentResourceManager.GetObject("picSnapshot9.Image");
      this.picSnapshot9.Location = new Point(277, 453);
      this.picSnapshot9.Name = "picSnapshot9";
      this.picSnapshot9.Size = new Size(16, 16);
      this.picSnapshot9.TabIndex = 424;
      this.picSnapshot9.TabStop = false;
      this.picSnapshot9.Tag = (object) "8";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot9, "Select...");
      this.picSnapshot9.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot9.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot9.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.lblSnapshot5.AutoSize = true;
      this.lblSnapshot5.Location = new Point(1, 244);
      this.lblSnapshot5.Name = "lblSnapshot5";
      this.lblSnapshot5.Size = new Size(61, 13);
      this.lblSnapshot5.TabIndex = 23;
      this.lblSnapshot5.Text = "Snapshot 5";
      this.lblSnapshot5.TextAlign = ContentAlignment.MiddleLeft;
      this.picSnapshot8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot8.Image = (Image) componentResourceManager.GetObject("picSnapshot8.Image");
      this.picSnapshot8.Location = new Point(277, 400);
      this.picSnapshot8.Name = "picSnapshot8";
      this.picSnapshot8.Size = new Size(16, 16);
      this.picSnapshot8.TabIndex = 423;
      this.picSnapshot8.TabStop = false;
      this.picSnapshot8.Tag = (object) "7";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot8, "Select...");
      this.picSnapshot8.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot8.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot8.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.lblSnapshot6.AutoSize = true;
      this.lblSnapshot6.Location = new Point(1, 297);
      this.lblSnapshot6.Name = "lblSnapshot6";
      this.lblSnapshot6.Size = new Size(61, 13);
      this.lblSnapshot6.TabIndex = 24;
      this.lblSnapshot6.Text = "Snapshot 6";
      this.lblSnapshot6.TextAlign = ContentAlignment.MiddleLeft;
      this.picSnapshot7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot7.Image = (Image) componentResourceManager.GetObject("picSnapshot7.Image");
      this.picSnapshot7.Location = new Point(277, 347);
      this.picSnapshot7.Name = "picSnapshot7";
      this.picSnapshot7.Size = new Size(16, 16);
      this.picSnapshot7.TabIndex = 422;
      this.picSnapshot7.TabStop = false;
      this.picSnapshot7.Tag = (object) "6";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot7, "Select...");
      this.picSnapshot7.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot7.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot7.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.lblSnapshot7.AutoSize = true;
      this.lblSnapshot7.Location = new Point(1, 350);
      this.lblSnapshot7.Name = "lblSnapshot7";
      this.lblSnapshot7.Size = new Size(61, 13);
      this.lblSnapshot7.TabIndex = 25;
      this.lblSnapshot7.Text = "Snapshot 7";
      this.lblSnapshot7.TextAlign = ContentAlignment.MiddleLeft;
      this.picSnapshot6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot6.Image = (Image) componentResourceManager.GetObject("picSnapshot6.Image");
      this.picSnapshot6.Location = new Point(277, 294);
      this.picSnapshot6.Name = "picSnapshot6";
      this.picSnapshot6.Size = new Size(16, 16);
      this.picSnapshot6.TabIndex = 421;
      this.picSnapshot6.TabStop = false;
      this.picSnapshot6.Tag = (object) "5";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot6, "Select...");
      this.picSnapshot6.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot6.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot6.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.lblSnapshot8.AutoSize = true;
      this.lblSnapshot8.Location = new Point(1, 403);
      this.lblSnapshot8.Name = "lblSnapshot8";
      this.lblSnapshot8.Size = new Size(61, 13);
      this.lblSnapshot8.TabIndex = 26;
      this.lblSnapshot8.Text = "Snapshot 8";
      this.lblSnapshot8.TextAlign = ContentAlignment.MiddleLeft;
      this.picSnapshot5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot5.Image = (Image) componentResourceManager.GetObject("picSnapshot5.Image");
      this.picSnapshot5.Location = new Point(277, 241);
      this.picSnapshot5.Name = "picSnapshot5";
      this.picSnapshot5.Size = new Size(16, 16);
      this.picSnapshot5.TabIndex = 420;
      this.picSnapshot5.TabStop = false;
      this.picSnapshot5.Tag = (object) "4";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot5, "Select...");
      this.picSnapshot5.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot5.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot5.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.lblSnapshot9.AutoSize = true;
      this.lblSnapshot9.Location = new Point(1, 456);
      this.lblSnapshot9.Name = "lblSnapshot9";
      this.lblSnapshot9.Size = new Size(61, 13);
      this.lblSnapshot9.TabIndex = 27;
      this.lblSnapshot9.Text = "Snapshot 9";
      this.lblSnapshot9.TextAlign = ContentAlignment.MiddleLeft;
      this.picSnapshot4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot4.Image = (Image) componentResourceManager.GetObject("picSnapshot4.Image");
      this.picSnapshot4.Location = new Point(277, 188);
      this.picSnapshot4.Name = "picSnapshot4";
      this.picSnapshot4.Size = new Size(16, 16);
      this.picSnapshot4.TabIndex = 419;
      this.picSnapshot4.TabStop = false;
      this.picSnapshot4.Tag = (object) "3";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot4, "Select...");
      this.picSnapshot4.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot4.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot4.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.txtSnapshot1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot1.Location = new Point(72, 27);
      this.txtSnapshot1.Name = "txtSnapshot1";
      this.txtSnapshot1.ReadOnly = true;
      this.txtSnapshot1.Size = new Size(199, 20);
      this.txtSnapshot1.TabIndex = 70;
      this.txtSnapshot1.Tag = (object) "";
      this.picSnapshot3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot3.Image = (Image) componentResourceManager.GetObject("picSnapshot3.Image");
      this.picSnapshot3.Location = new Point(277, 135);
      this.picSnapshot3.Name = "picSnapshot3";
      this.picSnapshot3.Size = new Size(16, 16);
      this.picSnapshot3.TabIndex = 418;
      this.picSnapshot3.TabStop = false;
      this.picSnapshot3.Tag = (object) "2";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot3, "Select...");
      this.picSnapshot3.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot3.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot3.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.txtSnapshot2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot2.Location = new Point(72, 80);
      this.txtSnapshot2.Name = "txtSnapshot2";
      this.txtSnapshot2.ReadOnly = true;
      this.txtSnapshot2.Size = new Size(199, 20);
      this.txtSnapshot2.TabIndex = 71;
      this.txtSnapshot2.Tag = (object) "";
      this.picSnapshot2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot2.Image = (Image) componentResourceManager.GetObject("picSnapshot2.Image");
      this.picSnapshot2.Location = new Point(277, 82);
      this.picSnapshot2.Name = "picSnapshot2";
      this.picSnapshot2.Size = new Size(16, 16);
      this.picSnapshot2.TabIndex = 417;
      this.picSnapshot2.TabStop = false;
      this.picSnapshot2.Tag = (object) "1";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot2, "Select...");
      this.picSnapshot2.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot2.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot2.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.txtSnapshot3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot3.Location = new Point(72, 133);
      this.txtSnapshot3.Name = "txtSnapshot3";
      this.txtSnapshot3.ReadOnly = true;
      this.txtSnapshot3.Size = new Size(199, 20);
      this.txtSnapshot3.TabIndex = 72;
      this.txtSnapshot3.Tag = (object) "";
      this.picSnapshot1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picSnapshot1.Image = (Image) componentResourceManager.GetObject("picSnapshot1.Image");
      this.picSnapshot1.Location = new Point(277, 29);
      this.picSnapshot1.Name = "picSnapshot1";
      this.picSnapshot1.Size = new Size(16, 16);
      this.picSnapshot1.TabIndex = 416;
      this.picSnapshot1.TabStop = false;
      this.picSnapshot1.Tag = (object) "0";
      this.tipToolTip.SetToolTip((Control) this.picSnapshot1, "Select...");
      this.picSnapshot1.Click += new EventHandler(this.picSnapshot_Click);
      this.picSnapshot1.MouseEnter += new EventHandler(this.picSnapshot_MouseEnter);
      this.picSnapshot1.MouseLeave += new EventHandler(this.picSnapshot_MouseLeave);
      this.txtSnapshot4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot4.Location = new Point(72, 186);
      this.txtSnapshot4.Name = "txtSnapshot4";
      this.txtSnapshot4.ReadOnly = true;
      this.txtSnapshot4.Size = new Size(199, 20);
      this.txtSnapshot4.TabIndex = 73;
      this.txtSnapshot4.Tag = (object) "";
      this.txtSnapshot9.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot9.Location = new Point(72, 451);
      this.txtSnapshot9.Name = "txtSnapshot9";
      this.txtSnapshot9.ReadOnly = true;
      this.txtSnapshot9.Size = new Size(199, 20);
      this.txtSnapshot9.TabIndex = 78;
      this.txtSnapshot9.Tag = (object) "";
      this.txtSnapshot5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot5.Location = new Point(72, 239);
      this.txtSnapshot5.Name = "txtSnapshot5";
      this.txtSnapshot5.ReadOnly = true;
      this.txtSnapshot5.Size = new Size(199, 20);
      this.txtSnapshot5.TabIndex = 74;
      this.txtSnapshot5.Tag = (object) "";
      this.txtSnapshot8.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot8.Location = new Point(72, 398);
      this.txtSnapshot8.Name = "txtSnapshot8";
      this.txtSnapshot8.ReadOnly = true;
      this.txtSnapshot8.Size = new Size(199, 20);
      this.txtSnapshot8.TabIndex = 77;
      this.txtSnapshot8.Tag = (object) "";
      this.txtSnapshot6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot6.Location = new Point(72, 292);
      this.txtSnapshot6.Name = "txtSnapshot6";
      this.txtSnapshot6.ReadOnly = true;
      this.txtSnapshot6.Size = new Size(199, 20);
      this.txtSnapshot6.TabIndex = 75;
      this.txtSnapshot6.Tag = (object) "";
      this.txtSnapshot7.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSnapshot7.Location = new Point(72, 345);
      this.txtSnapshot7.Name = "txtSnapshot7";
      this.txtSnapshot7.ReadOnly = true;
      this.txtSnapshot7.Size = new Size(199, 20);
      this.txtSnapshot7.TabIndex = 76;
      this.txtSnapshot7.Tag = (object) "";
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.BackColor = SystemColors.Control;
      this.btnSelect.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSelect.Location = new Point(379, 585);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(92, 21);
      this.btnSelect.TabIndex = 443;
      this.btnSelect.Text = "Select View";
      this.btnSelect.UseCompatibleTextRendering = true;
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.BackColor = SystemColors.Control;
      this.btnSave.Enabled = false;
      this.btnSave.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSave.Location = new Point(477, 585);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 21);
      this.btnSave.TabIndex = 436;
      this.btnSave.Text = "&Save";
      this.btnSave.UseCompatibleTextRendering = true;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnCancel.Location = new Point(558, 585);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 21);
      this.btnCancel.TabIndex = 435;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseCompatibleTextRendering = true;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.imgLstSearch.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgLstSearch.ImageStream");
      this.imgLstSearch.TransparentColor = Color.Transparent;
      this.imgLstSearch.Images.SetKeyName(0, "picSearch");
      this.imgLstSearch.Images.SetKeyName(1, "picSearchDisabled");
      this.imgLstSearch.Images.SetKeyName(2, "picSearchMouseOver");
      this.imglstLayouts.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imglstLayouts.ImageStream");
      this.imglstLayouts.TransparentColor = Color.Transparent;
      this.imglstLayouts.Images.SetKeyName(0, "C1R1");
      this.imglstLayouts.Images.SetKeyName(1, "C1R2");
      this.imglstLayouts.Images.SetKeyName(2, "C2R1R1");
      this.imglstLayouts.Images.SetKeyName(3, "C2R2R2");
      this.imglstLayouts.Images.SetKeyName(4, "C2R2R2W4");
      this.imglstLayouts.Images.SetKeyName(5, "C2R2R2W6");
      this.imglstLayouts.Images.SetKeyName(6, "C2R3R2");
      this.imglstLayouts.Images.SetKeyName(7, "C2R3R2W4");
      this.imglstLayouts.Images.SetKeyName(8, "C2R3R2W6");
      this.imglstLayouts.Images.SetKeyName(9, "C2R2R3");
      this.imglstLayouts.Images.SetKeyName(10, "C2R2R3W4");
      this.imglstLayouts.Images.SetKeyName(11, "C2R2R3W6");
      this.imglstLayouts.Images.SetKeyName(12, "C2R3R3");
      this.imglstLayouts.Images.SetKeyName(13, "C2R3R3W4");
      this.imglstLayouts.Images.SetKeyName(14, "C2R3R3W6");
      this.imglstLayouts.Images.SetKeyName(15, "C3R2R2R2");
      this.imglstLayouts.Images.SetKeyName(16, "R2C2C3");
      this.imglstLayouts.Images.SetKeyName(17, "C3R3R3R3");
      this.imglstLayouts.Images.SetKeyName(18, "C1R1Ovr");
      this.imglstLayouts.Images.SetKeyName(19, "C1R2Ovr");
      this.imglstLayouts.Images.SetKeyName(20, "C2R1R1Ovr");
      this.imglstLayouts.Images.SetKeyName(21, "C2R2R2Ovr");
      this.imglstLayouts.Images.SetKeyName(22, "C2R2R2W4Ovr");
      this.imglstLayouts.Images.SetKeyName(23, "C2R2R2W6Ovr");
      this.imglstLayouts.Images.SetKeyName(24, "C2R3R2Ovr");
      this.imglstLayouts.Images.SetKeyName(25, "C2R3R2W4Ovr");
      this.imglstLayouts.Images.SetKeyName(26, "C2R3R2W6Ovr");
      this.imglstLayouts.Images.SetKeyName(27, "C2R2R3Ovr");
      this.imglstLayouts.Images.SetKeyName(28, "C2R2R3W4Ovr");
      this.imglstLayouts.Images.SetKeyName(29, "C2R2R3W6Ovr");
      this.imglstLayouts.Images.SetKeyName(30, "C2R3R3Ovr");
      this.imglstLayouts.Images.SetKeyName(31, "C2R3R3W4Ovr");
      this.imglstLayouts.Images.SetKeyName(32, "C2R3R3W6Ovr");
      this.imglstLayouts.Images.SetKeyName(33, "C3R2R2R2Ovr");
      this.imglstLayouts.Images.SetKeyName(34, "R2C2C3Ovr");
      this.imglstLayouts.Images.SetKeyName(35, "C3R3R3R3Ovr");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = SystemColors.Window;
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.pnlLeft);
      this.Name = "DashboardViewTemplateForm";
      this.Size = new Size(914, 608);
      this.KeyUp += new KeyEventHandler(this.DashboardViewTemplateForm_KeyUp);
      this.pnlLeft.ResumeLayout(false);
      this.pnlNavigate.ResumeLayout(false);
      this.pnlSelectView.ResumeLayout(false);
      this.gpDefaultView.ResumeLayout(false);
      this.gpDefaultView.PerformLayout();
      this.pnlRight.ResumeLayout(false);
      this.gcViewConfig.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.btnViewFilter).EndInit();
      this.pnlSelectLayout.ResumeLayout(false);
      this.pnlSelectLayout.PerformLayout();
      this.pnlLayout3.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout3).EndInit();
      this.pnlLayout18.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout18).EndInit();
      this.pnlLayout1.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout1).EndInit();
      this.pnlLayout16.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout16).EndInit();
      this.pnlLayout2.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout2).EndInit();
      this.pnlLayout15.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout15).EndInit();
      this.pnlLayout17.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout17).EndInit();
      this.pnlLayout14.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout14).EndInit();
      this.pnlLayout4.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout4).EndInit();
      this.pnlLayout13.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout13).EndInit();
      this.pnlLayout5.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout5).EndInit();
      this.pnlLayout12.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout12).EndInit();
      this.pnlLayout6.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout6).EndInit();
      this.pnlLayout11.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout11).EndInit();
      this.pnlLayout7.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout7).EndInit();
      this.pnlLayout10.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout10).EndInit();
      this.pnlLayout8.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout8).EndInit();
      this.pnlLayout9.ResumeLayout(false);
      ((ISupportInitialize) this.picLayout9).EndInit();
      this.pnlSelectSnapshot.ResumeLayout(false);
      this.pnlSelectSnapshot.PerformLayout();
      ((ISupportInitialize) this.picSnapshot9).EndInit();
      ((ISupportInitialize) this.picSnapshot8).EndInit();
      ((ISupportInitialize) this.picSnapshot7).EndInit();
      ((ISupportInitialize) this.picSnapshot6).EndInit();
      ((ISupportInitialize) this.picSnapshot5).EndInit();
      ((ISupportInitialize) this.picSnapshot4).EndInit();
      ((ISupportInitialize) this.picSnapshot3).EndInit();
      ((ISupportInitialize) this.picSnapshot2).EndInit();
      ((ISupportInitialize) this.picSnapshot1).EndInit();
      this.ResumeLayout(false);
    }

    public enum ProcessingMode
    {
      Unspecified,
      SelectTemplate,
      EditTemplate,
      ManageTemplates,
    }

    private class SnapshotTag
    {
      public string ReportName = string.Empty;
      public string TemplatePath = string.Empty;
      public string[] ReportParameters = new string[0];

      private SnapshotTag()
      {
      }

      public SnapshotTag(string reportName, string templatePath, string[] reportParameters)
      {
        this.ReportName = reportName;
        this.TemplatePath = templatePath;
        this.ReportParameters = reportParameters;
      }
    }
  }
}
