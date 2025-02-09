// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddEditUserCEDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.Setup.SecurityGroup;
using EllieMae.EMLite.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddEditUserCEDialog : Form, IHelp
  {
    private IWin32Window owner;
    private const string className = "AddEditUserCEDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private TextBox useridTxt;
    private TextBox passwTxt;
    private TextBox lnameTxt;
    private TextBox fnameTxt;
    private Button okBtn;
    private Button cancelBtn;
    private ComboBox folderComboBox;
    private TextBox verifyPwdTxt;
    private CheckBox disableCheckBox;
    private Label emailLabel;
    private TextBox emailTextBox;
    private TextBox phoneTextBox;
    private Label phoneLabel;
    private bool intermidiateData;
    private bool deleteBackKey;
    private UserInfo.UserStatusEnum initialStatus;
    private UserInfo currentUser;
    private AclGroup[] assignedGroup;
    private WorkflowManager workflowMgr;
    private int currentOrgID;
    private Persona[] personaList;
    private IContainer components;
    private UserProfileInfo currentuserProfile;
    private string userid;
    private string password;
    private string lname;
    private string jobtitle;
    private string suffixName;
    private string fname;
    private string middleName;
    private string employeeID = string.Empty;
    private string workingFolder;
    private UserInfo.AccessModeEnum accessMode;
    private UserInfo.UserStatusEnum status;
    private UserInfo.UserPeerView peerView;
    private string email;
    private bool locked;
    private bool requirePwdChange;
    private Label userIDLabel;
    private Label pwdLabel;
    private Label lnameLabel;
    private Label fnameLabel;
    private Label folderLabel;
    private Label verifyPwdLabel;
    private string phone;
    private string cellPhone;
    private string fax;
    private string chumID = string.Empty;
    private string nmlsOriginatorID = string.Empty;
    private DateTime nmlsExpirationDate = DateTime.MaxValue;
    private LoanCompHistoryList loanCompHistoryList;
    private bool editUser;
    private int _OrgId = -1;
    private Button btnViewPersonaRights;
    private LOLicenseInfo[] loLicInfo;
    private CheckBox lockedCheckBox;
    private CheckBox newPasswordCheckBox;
    private TextBox faxTextBox;
    private Label label7;
    private Label label2;
    private TextBox txtCellPhone;
    private Label label3;
    private TextBox chumTextBox;
    private GroupContainer grpAcctInfo;
    private Label label1;
    private Label label9;
    private Label label8;
    private Label label6;
    private Label label5;
    private Label label4;
    private GroupContainer grpPersonas;
    private Label label10;
    private FlowLayoutPanel flowLayoutPanel1;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnRemovePersona;
    private StandardIconButton btnAssignPersona;
    private GridView gvPersonas;
    private ToolTip toolTip1;
    private Label label11;
    private TextBox nmlsTextBox;
    private Persona[] personas;
    private Label label12;
    private DatePicker dpNMLSExpiration;
    private Label label13;
    private TextBox employeeIDTxt;
    private Sessions.Session session;
    private LOCompCurrentControl loCompCurrentControl;
    private Panel panelTop;
    private Panel panelBottom;
    private Label label15;
    private TextBox suffixTxt;
    private Label label14;
    private TextBox middleNameTxt;
    private GroupContainer grpLoanAccess;
    private CheckBox chbEditSubordinate;
    private CheckBox chbPeerAccess;
    private RadioButton rdbEdit;
    private RadioButton rdbView;
    private Panel panelLOCompHistory;
    private Panel panelLOCompLatest;
    private Panel panelMiddle;
    private GroupContainer grpLicenses;
    private GridView gvLicenses;
    private StandardIconButton loBtn;
    private GroupContainer grpComments;
    private RichTextBox PersonaCommentsTextBox;
    private GroupContainer grpGroups;
    private FlowLayoutPanel flowLayoutPanel2;
    private Button btnViewGroupRights;
    private GridView gvGroups;
    private Button btnPublicProfile;
    private LOCompHistoryControl loCompHistoryControl;
    private CCSiteControl ccSiteControl;
    private const long minSizeInByte = 3072;
    private Panel panelCCSite;
    private Panel panel2;
    private Label label16;
    private TextBox oAuthIDtext;
    private CheckBox allowImpersonationCheckBox;
    private Label oAuthIDLabel;
    private Panel panel1;
    private CheckBox apiUserCheckBox;
    private Label label17;
    private TextBox jobtitleTxt;
    private CheckBox ssoAccessCheckBox;
    private ImageList imgListTv;
    private Label lblOrgLink;
    private ContextMenu contMenu;
    private MenuItem miLinkTo;
    private MenuItem miDisconTo;
    private const long maxSizeInByte = 512000;
    private OrgInfo orgInfo;
    private Label lblOrgUnlink;
    private OrgInfo currentOrgInfo;
    private bool dataSaved;

    public string Userid => this.userid;

    public string Password => this.password;

    public string LastName => this.lname;

    public string JobTitle => this.jobtitle;

    public string SuffixName => this.suffixName;

    public string FirstName => this.fname;

    public string MiddleName => this.middleName;

    public string EmployeeID => this.employeeID;

    public string WorkingFolder => this.workingFolder;

    public UserInfo.AccessModeEnum AccessMode => this.accessMode;

    public UserInfo.UserStatusEnum Status => this.status;

    public UserInfo.UserPeerView PeerView => this.peerView;

    public bool Locked => this.locked;

    public bool RequirePasswordChange => this.requirePwdChange;

    public string Email => this.email;

    public string Phone => this.phone;

    public string CellPhone => this.cellPhone;

    public string Fax => this.fax;

    public string ChumID => this.chumID;

    public string NMLSOriginatorID => this.nmlsOriginatorID;

    public DateTime NMLSExpirationDate => this.nmlsExpirationDate;

    public LoanCompHistoryList LOCompHistoryList => this.loanCompHistoryList;

    public AddEditUserCEDialog(
      Sessions.Session session,
      IWin32Window owner,
      string userid,
      int orgId,
      bool readOnly)
    {
      this.owner = owner;
      this.session = session;
      this.orgInfo = this.session.OrganizationManager.GetOrganization(this.session.UserInfo.OrgId);
      this.workflowMgr = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
      this.currentOrgID = orgId;
      this.currentOrgInfo = this.session.OrganizationManager.GetOrganization(this.currentOrgID);
      this.personaList = this.session.PersonaManager.GetAllPersonas();
      this.InitializeComponent();
      Rectangle bounds = Screen.FromControl((Control) this).Bounds;
      if (this.Height > bounds.Height)
      {
        this.AutoScroll = true;
        this.Height = bounds.Height - 50;
        this.Width += 10;
      }
      this.editUser = userid != null;
      this.userid = userid;
      this._OrgId = orgId;
      this.currentUser = (UserInfo) null;
      if (this.editUser)
      {
        this.currentUser = this.session.OrganizationManager.GetUser(userid, true);
        if (this.currentUser.ApiUser)
        {
          this.apiUserCheckBox.Visible = true;
          this.apiUserCheckBox.Checked = true;
          this.apiUserCheckBox.Enabled = false;
        }
        else
          this.apiUserCheckBox.Visible = false;
      }
      this.initFolderComboBox(this.currentUser == (UserInfo) null ? (string) null : this.currentUser.WorkingFolder);
      if (this.editUser)
      {
        this.loLicInfo = this.session.OrganizationManager.GetLOLicenses(userid);
        if (this.session.EncompassEdition == EncompassEdition.Banker)
          this.loanCompHistoryList = this.session.ConfigurationManager.GetComPlanHistoryforUser(userid);
      }
      else
      {
        this.loLicInfo = this.createDefaultLOLicenses();
        if (this.session.EncompassEdition == EncompassEdition.Banker)
          this.loanCompHistoryList = new LoanCompHistoryList(userid);
      }
      if (this.loanCompHistoryList != null)
      {
        this.loanCompHistoryList.UseParentInfo = this.currentUser != (UserInfo) null && this.currentUser.InheritParentCompPlan;
        this.loCompCurrentControl = new LOCompCurrentControl(this.session, true, false);
        this.panelLOCompLatest.Controls.Add((Control) this.loCompCurrentControl);
        this.loCompHistoryControl = new LOCompHistoryControl(this.session, true, false);
        this.panelLOCompHistory.Controls.Add((Control) this.loCompHistoryControl);
        if (this.loanCompHistoryList == null)
          this.loanCompHistoryList = new LoanCompHistoryList(userid);
        this.loCompCurrentControl.RefreshDate(this.loanCompHistoryList, orgId.ToString(), userid);
        this.loCompHistoryControl.RefreshData(this.loanCompHistoryList, orgId.ToString(), userid);
        this.loCompHistoryControl.HistorySelectedIndexChanged += new EventHandler(this.loCompHistoryControl_HistorySelectedIndexChanged);
        this.loCompHistoryControl.UseParentInfoClicked += new EventHandler(this.loCompCurrentControl_UseParentInfoClicked);
        this.loCompHistoryControl.AssignPlanButtonClicked += new EventHandler(this.loCompHistoryControl_AssignPlanButtonClicked);
        this.loCompCurrentControl.StartDateChanged += new EventHandler(this.loCompCurrentControl_StartDateChanged);
      }
      else
      {
        this.panelMiddle.Visible = false;
        this.Height = this.panelTop.Height + this.panelBottom.Height * 2;
      }
      this.ccSiteControl = new CCSiteControl(this.session, this.currentUser, orgId);
      this.panelCCSite.Controls.Add((Control) this.ccSiteControl);
      this.ccSiteControl.Dock = DockStyle.Fill;
      this.setAccessToSSOControls();
      if (!this.editUser)
      {
        this.useridTxt.TabStop = true;
        this.btnViewPersonaRights.Enabled = false;
        this.rdbEdit.Enabled = false;
        this.rdbView.Enabled = false;
        this.loadDefaultAssignedGroup();
        this.refreshLicenses();
        this.setCheckBoxEventHandlers();
      }
      else
      {
        if (UserInfo.IsSuperAdministrator(this.currentUser.Userid, this.currentUser.UserPersonas))
          this.disableCheckBox.Enabled = false;
        if (UserInfo.IsSuperAdministrator(userid, this.currentUser.UserPersonas))
        {
          this.btnViewPersonaRights.Enabled = false;
          this.btnViewGroupRights.Enabled = false;
        }
        if (this.currentUser.Userid == "admin")
        {
          this.disableCheckBox.Enabled = false;
          this.lockedCheckBox.Enabled = false;
          this.newPasswordCheckBox.Enabled = false;
        }
        else
        {
          this.disableCheckBox.Enabled = true;
          this.lockedCheckBox.Enabled = true;
          if (!this.ssoAccessCheckBox.Checked)
            this.newPasswordCheckBox.Enabled = true;
        }
        if (readOnly)
        {
          foreach (Control control in (ArrangedElementCollection) this.Controls)
          {
            switch (control)
            {
              case TextBox _:
                ((TextBoxBase) control).ReadOnly = true;
                continue;
              case Button _:
              case RadioButton _:
              case ComboBox _:
              case CheckBox _:
              case GroupBox _:
                control.Enabled = false;
                continue;
              default:
                continue;
            }
          }
          this.cancelBtn.Enabled = true;
        }
        this.reLoad();
        if (this.currentUser.Userid == "tpowcadmin")
        {
          this.newPasswordCheckBox.Enabled = this.newPasswordCheckBox.Checked = false;
          if (this.validateTPOWebCenterAdminUserPersona())
            this.btnAssignPersona.Enabled = this.btnRemovePersona.Enabled = false;
        }
        this.setCheckBoxEventHandlers();
        this.btnPublicProfile.Enabled = ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_Personal_MyProfilePhoto);
      }
    }

    private void setAccessToSSOControls()
    {
      bool enableSso = this.session.StartupInfo.EnableSSO;
      this.ssoAccessCheckBox.Visible = enableSso;
      this.passwTxt.Enabled = !enableSso;
      this.verifyPwdTxt.Enabled = !enableSso;
      this.newPasswordCheckBox.Enabled = !enableSso;
      this.lblOrgLink.Visible = this.lblOrgUnlink.Visible = enableSso;
      this.lblOrgLink.Enabled = this.lblOrgUnlink.Enabled = this.ssoAccessCheckBox.Enabled = this.IsRestrictedAccessEnabled();
      if (!enableSso)
        return;
      this.lblOrgLink.ContextMenu = this.contMenu;
      this.lblOrgUnlink.ContextMenu = this.contMenu;
      ToolTip toolTip = new ToolTip();
      if (this.editUser)
      {
        this.ssoAccessCheckBox.Checked = this.currentUser.SSOOnly;
        this.lblOrgLink.Visible = !this.currentUser.SSODisconnectedFromOrg;
        this.lblOrgUnlink.Visible = this.currentUser.SSODisconnectedFromOrg;
        this.passwTxt.Enabled = !this.currentUser.SSOOnly;
        this.verifyPwdTxt.Enabled = !this.currentUser.SSOOnly;
        this.newPasswordCheckBox.Enabled = !this.currentUser.SSOOnly;
      }
      else
      {
        this.ssoAccessCheckBox.Checked = this.currentOrgInfo.SSOSettings.LoginAccess;
        this.passwTxt.Enabled = !this.ssoAccessCheckBox.Checked;
        this.verifyPwdTxt.Enabled = !this.ssoAccessCheckBox.Checked;
        this.newPasswordCheckBox.Enabled = !this.ssoAccessCheckBox.Checked;
        this.lblOrgLink.Visible = true;
        this.lblOrgUnlink.Visible = false;
      }
    }

    private bool IsRestrictedAccessEnabled()
    {
      return this.session.UserInfo.IsAdministrator() && this.session.StartupInfo.EnableSSO && this.session.StartupInfo.IsWebLoginEnabled;
    }

    private void setCheckBoxEventHandlers()
    {
      this.disableCheckBox.CheckedChanged += new EventHandler(this.disableCheckBox_CheckedChanged);
      this.lockedCheckBox.CheckedChanged += new EventHandler(this.lockedCheckBox_CheckedChanged);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddEditUserCEDialog));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.btnRemovePersona = new StandardIconButton();
      this.btnAssignPersona = new StandardIconButton();
      this.loBtn = new StandardIconButton();
      this.apiUserCheckBox = new CheckBox();
      this.oAuthIDLabel = new Label();
      this.lblOrgLink = new Label();
      this.imgListTv = new ImageList(this.components);
      this.lblOrgUnlink = new Label();
      this.panelTop = new Panel();
      this.grpComments = new GroupContainer();
      this.PersonaCommentsTextBox = new RichTextBox();
      this.grpAcctInfo = new GroupContainer();
      this.label17 = new Label();
      this.jobtitleTxt = new TextBox();
      this.panel2 = new Panel();
      this.label16 = new Label();
      this.oAuthIDtext = new TextBox();
      this.allowImpersonationCheckBox = new CheckBox();
      this.panel1 = new Panel();
      this.ssoAccessCheckBox = new CheckBox();
      this.label4 = new Label();
      this.passwTxt = new TextBox();
      this.newPasswordCheckBox = new CheckBox();
      this.verifyPwdTxt = new TextBox();
      this.label5 = new Label();
      this.verifyPwdLabel = new Label();
      this.pwdLabel = new Label();
      this.label15 = new Label();
      this.suffixTxt = new TextBox();
      this.label14 = new Label();
      this.middleNameTxt = new TextBox();
      this.btnPublicProfile = new Button();
      this.label13 = new Label();
      this.employeeIDTxt = new TextBox();
      this.dpNMLSExpiration = new DatePicker();
      this.label12 = new Label();
      this.nmlsTextBox = new TextBox();
      this.label11 = new Label();
      this.label3 = new Label();
      this.folderLabel = new Label();
      this.userIDLabel = new Label();
      this.emailLabel = new Label();
      this.label7 = new Label();
      this.lnameLabel = new Label();
      this.label2 = new Label();
      this.fnameLabel = new Label();
      this.phoneLabel = new Label();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label6 = new Label();
      this.label1 = new Label();
      this.chumTextBox = new TextBox();
      this.disableCheckBox = new CheckBox();
      this.folderComboBox = new ComboBox();
      this.emailTextBox = new TextBox();
      this.faxTextBox = new TextBox();
      this.txtCellPhone = new TextBox();
      this.phoneTextBox = new TextBox();
      this.fnameTxt = new TextBox();
      this.lnameTxt = new TextBox();
      this.useridTxt = new TextBox();
      this.lockedCheckBox = new CheckBox();
      this.grpGroups = new GroupContainer();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnViewGroupRights = new Button();
      this.gvGroups = new GridView();
      this.grpLicenses = new GroupContainer();
      this.gvLicenses = new GridView();
      this.grpLoanAccess = new GroupContainer();
      this.chbEditSubordinate = new CheckBox();
      this.chbPeerAccess = new CheckBox();
      this.rdbEdit = new RadioButton();
      this.rdbView = new RadioButton();
      this.grpPersonas = new GroupContainer();
      this.label10 = new Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnViewPersonaRights = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.gvPersonas = new GridView();
      this.panelBottom = new Panel();
      this.panelCCSite = new Panel();
      this.panelLOCompHistory = new Panel();
      this.panelLOCompLatest = new Panel();
      this.panelMiddle = new Panel();
      this.contMenu = new ContextMenu();
      this.miLinkTo = new MenuItem();
      this.miDisconTo = new MenuItem();
      ((ISupportInitialize) this.btnRemovePersona).BeginInit();
      ((ISupportInitialize) this.btnAssignPersona).BeginInit();
      ((ISupportInitialize) this.loBtn).BeginInit();
      this.panelTop.SuspendLayout();
      this.grpComments.SuspendLayout();
      this.grpAcctInfo.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.grpGroups.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.grpLicenses.SuspendLayout();
      this.grpLoanAccess.SuspendLayout();
      this.grpPersonas.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.panelBottom.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.BackColor = SystemColors.Control;
      this.okBtn.Location = new Point(722, 88);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(72, 22);
      this.okBtn.TabIndex = 6;
      this.okBtn.Text = "&Save";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.BackColor = SystemColors.Control;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(796, 88);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(72, 22);
      this.cancelBtn.TabIndex = 7;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.btnRemovePersona.BackColor = Color.Transparent;
      this.btnRemovePersona.Enabled = false;
      this.btnRemovePersona.Location = new Point(36, 3);
      this.btnRemovePersona.Margin = new Padding(2, 3, 3, 3);
      this.btnRemovePersona.MouseDownImage = (Image) null;
      this.btnRemovePersona.Name = "btnRemovePersona";
      this.btnRemovePersona.Size = new Size(16, 16);
      this.btnRemovePersona.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemovePersona.TabIndex = 22;
      this.btnRemovePersona.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemovePersona, "Remove Persona");
      this.btnRemovePersona.Click += new EventHandler(this.btnRemovePersona_Click);
      this.btnAssignPersona.BackColor = Color.Transparent;
      this.btnAssignPersona.Location = new Point(15, 3);
      this.btnAssignPersona.Margin = new Padding(2, 3, 3, 3);
      this.btnAssignPersona.MouseDownImage = (Image) null;
      this.btnAssignPersona.Name = "btnAssignPersona";
      this.btnAssignPersona.Size = new Size(16, 16);
      this.btnAssignPersona.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAssignPersona.TabIndex = 23;
      this.btnAssignPersona.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAssignPersona, "Add Persona");
      this.btnAssignPersona.Click += new EventHandler(this.btnAssignPersona_Click);
      this.loBtn.BackColor = Color.Transparent;
      this.loBtn.Location = new Point(343, 5);
      this.loBtn.MouseDownImage = (Image) null;
      this.loBtn.Name = "loBtn";
      this.loBtn.Size = new Size(16, 16);
      this.loBtn.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.loBtn.TabIndex = 0;
      this.loBtn.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.loBtn, "Edit Licenses");
      this.loBtn.Click += new EventHandler(this.loBtn_Click);
      this.apiUserCheckBox.Location = new Point(358, 28);
      this.apiUserCheckBox.Name = "apiUserCheckBox";
      this.apiUserCheckBox.Size = new Size(94, 18);
      this.apiUserCheckBox.TabIndex = 1;
      this.apiUserCheckBox.Text = "API User";
      this.toolTip1.SetToolTip((Control) this.apiUserCheckBox, "This option applies exclusively to Consulting Partner access. \r\nPlease refer to the documentation for more details.");
      this.apiUserCheckBox.CheckedChanged += new EventHandler(this.showOAuthPanelOrNot);
      this.oAuthIDLabel.AutoSize = true;
      this.oAuthIDLabel.BackColor = Color.Transparent;
      this.oAuthIDLabel.Location = new Point(7, 4);
      this.oAuthIDLabel.Name = "oAuthIDLabel";
      this.oAuthIDLabel.Size = new Size(67, 13);
      this.oAuthIDLabel.TabIndex = 1;
      this.oAuthIDLabel.Text = "API Client ID";
      this.toolTip1.SetToolTip((Control) this.oAuthIDLabel, "Please enter the API Client ID provided by the Consulting partner.");
      this.lblOrgLink.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblOrgLink.ImageIndex = 1;
      this.lblOrgLink.ImageList = this.imgListTv;
      this.lblOrgLink.Location = new Point(489, 1);
      this.lblOrgLink.Name = "lblOrgLink";
      this.lblOrgLink.Size = new Size(25, 20);
      this.lblOrgLink.TabIndex = 8;
      this.lblOrgLink.TextAlign = ContentAlignment.MiddleLeft;
      this.toolTip1.SetToolTip((Control) this.lblOrgLink, "Connected to Organization SSO Setting.");
      this.lblOrgLink.Visible = false;
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.lblOrgUnlink.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblOrgUnlink.ImageIndex = 0;
      this.lblOrgUnlink.ImageList = this.imgListTv;
      this.lblOrgUnlink.Location = new Point(489, 1);
      this.lblOrgUnlink.Name = "lblOrgUnlink";
      this.lblOrgUnlink.Size = new Size(25, 20);
      this.lblOrgUnlink.TabIndex = 129;
      this.lblOrgUnlink.TextAlign = ContentAlignment.MiddleLeft;
      this.toolTip1.SetToolTip((Control) this.lblOrgUnlink, "Disconnected from Organization SSO Setting.");
      this.lblOrgUnlink.Visible = false;
      this.panelTop.Controls.Add((Control) this.grpComments);
      this.panelTop.Controls.Add((Control) this.grpAcctInfo);
      this.panelTop.Controls.Add((Control) this.grpGroups);
      this.panelTop.Controls.Add((Control) this.grpLicenses);
      this.panelTop.Controls.Add((Control) this.grpLoanAccess);
      this.panelTop.Controls.Add((Control) this.grpPersonas);
      this.panelTop.Dock = DockStyle.Top;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(881, 560);
      this.panelTop.TabIndex = 0;
      this.grpComments.Controls.Add((Control) this.PersonaCommentsTextBox);
      this.grpComments.HeaderForeColor = SystemColors.ControlText;
      this.grpComments.Location = new Point(553, 322);
      this.grpComments.Name = "grpComments";
      this.grpComments.Size = new Size(320, 116);
      this.grpComments.TabIndex = 4;
      this.grpComments.Text = "Comments";
      this.PersonaCommentsTextBox.Dock = DockStyle.Fill;
      this.PersonaCommentsTextBox.Location = new Point(1, 26);
      this.PersonaCommentsTextBox.MaxLength = 4096;
      this.PersonaCommentsTextBox.Name = "PersonaCommentsTextBox";
      this.PersonaCommentsTextBox.Size = new Size(318, 89);
      this.PersonaCommentsTextBox.TabIndex = 0;
      this.PersonaCommentsTextBox.Text = "";
      this.grpAcctInfo.Controls.Add((Control) this.label17);
      this.grpAcctInfo.Controls.Add((Control) this.jobtitleTxt);
      this.grpAcctInfo.Controls.Add((Control) this.apiUserCheckBox);
      this.grpAcctInfo.Controls.Add((Control) this.panel2);
      this.grpAcctInfo.Controls.Add((Control) this.panel1);
      this.grpAcctInfo.Controls.Add((Control) this.label15);
      this.grpAcctInfo.Controls.Add((Control) this.suffixTxt);
      this.grpAcctInfo.Controls.Add((Control) this.label14);
      this.grpAcctInfo.Controls.Add((Control) this.middleNameTxt);
      this.grpAcctInfo.Controls.Add((Control) this.btnPublicProfile);
      this.grpAcctInfo.Controls.Add((Control) this.label13);
      this.grpAcctInfo.Controls.Add((Control) this.employeeIDTxt);
      this.grpAcctInfo.Controls.Add((Control) this.dpNMLSExpiration);
      this.grpAcctInfo.Controls.Add((Control) this.label12);
      this.grpAcctInfo.Controls.Add((Control) this.nmlsTextBox);
      this.grpAcctInfo.Controls.Add((Control) this.label11);
      this.grpAcctInfo.Controls.Add((Control) this.label3);
      this.grpAcctInfo.Controls.Add((Control) this.folderLabel);
      this.grpAcctInfo.Controls.Add((Control) this.userIDLabel);
      this.grpAcctInfo.Controls.Add((Control) this.emailLabel);
      this.grpAcctInfo.Controls.Add((Control) this.label7);
      this.grpAcctInfo.Controls.Add((Control) this.lnameLabel);
      this.grpAcctInfo.Controls.Add((Control) this.label2);
      this.grpAcctInfo.Controls.Add((Control) this.fnameLabel);
      this.grpAcctInfo.Controls.Add((Control) this.phoneLabel);
      this.grpAcctInfo.Controls.Add((Control) this.label9);
      this.grpAcctInfo.Controls.Add((Control) this.label8);
      this.grpAcctInfo.Controls.Add((Control) this.label6);
      this.grpAcctInfo.Controls.Add((Control) this.label1);
      this.grpAcctInfo.Controls.Add((Control) this.chumTextBox);
      this.grpAcctInfo.Controls.Add((Control) this.disableCheckBox);
      this.grpAcctInfo.Controls.Add((Control) this.folderComboBox);
      this.grpAcctInfo.Controls.Add((Control) this.emailTextBox);
      this.grpAcctInfo.Controls.Add((Control) this.faxTextBox);
      this.grpAcctInfo.Controls.Add((Control) this.txtCellPhone);
      this.grpAcctInfo.Controls.Add((Control) this.phoneTextBox);
      this.grpAcctInfo.Controls.Add((Control) this.fnameTxt);
      this.grpAcctInfo.Controls.Add((Control) this.lnameTxt);
      this.grpAcctInfo.Controls.Add((Control) this.useridTxt);
      this.grpAcctInfo.Controls.Add((Control) this.lockedCheckBox);
      this.grpAcctInfo.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.grpAcctInfo.HeaderForeColor = SystemColors.ControlText;
      this.grpAcctInfo.Location = new Point(9, 5);
      this.grpAcctInfo.Name = "grpAcctInfo";
      this.grpAcctInfo.Size = new Size(539, 433);
      this.grpAcctInfo.TabIndex = 1;
      this.grpAcctInfo.Text = "Account Information";
      this.label17.AutoSize = true;
      this.label17.BackColor = Color.Transparent;
      this.label17.Location = new Point(20, 207);
      this.label17.Name = "label17";
      this.label17.Size = new Size(47, 13);
      this.label17.TabIndex = 128;
      this.label17.Text = "Job Title";
      this.jobtitleTxt.Location = new Point(148, 203);
      this.jobtitleTxt.MaxLength = 64;
      this.jobtitleTxt.Name = "jobtitleTxt";
      this.jobtitleTxt.Size = new Size(203, 20);
      this.jobtitleTxt.TabIndex = 9;
      this.panel2.Controls.Add((Control) this.label16);
      this.panel2.Controls.Add((Control) this.oAuthIDtext);
      this.panel2.Controls.Add((Control) this.allowImpersonationCheckBox);
      this.panel2.Controls.Add((Control) this.oAuthIDLabel);
      this.panel2.Location = new Point(12, 48);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(435, 61);
      this.panel2.TabIndex = 2;
      this.panel2.Visible = false;
      this.label16.AutoSize = true;
      this.label16.BackColor = Color.Transparent;
      this.label16.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label16.ForeColor = Color.FromArgb(238, 0, 0);
      this.label16.Location = new Point(-3, 4);
      this.label16.Name = "label16";
      this.label16.Size = new Size(13, 16);
      this.label16.TabIndex = 0;
      this.label16.Text = "*";
      this.oAuthIDtext.Location = new Point(136, 1);
      this.oAuthIDtext.MaxLength = 100;
      this.oAuthIDtext.Name = "oAuthIDtext";
      this.oAuthIDtext.Size = new Size(203, 20);
      this.oAuthIDtext.TabIndex = 2;
      this.allowImpersonationCheckBox.Location = new Point(136, 23);
      this.allowImpersonationCheckBox.Name = "allowImpersonationCheckBox";
      this.allowImpersonationCheckBox.Size = new Size(184, 29);
      this.allowImpersonationCheckBox.TabIndex = 4;
      this.allowImpersonationCheckBox.Text = "Allow Impersonation";
      this.allowImpersonationCheckBox.Visible = true;
      this.panel1.Controls.Add((Control) this.lblOrgUnlink);
      this.panel1.Controls.Add((Control) this.lblOrgLink);
      this.panel1.Controls.Add((Control) this.ssoAccessCheckBox);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.passwTxt);
      this.panel1.Controls.Add((Control) this.newPasswordCheckBox);
      this.panel1.Controls.Add((Control) this.verifyPwdTxt);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.verifyPwdLabel);
      this.panel1.Controls.Add((Control) this.pwdLabel);
      this.panel1.Location = new Point(12, 48);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(520, 63);
      this.panel1.TabIndex = 1;
      this.ssoAccessCheckBox.Location = new Point(346, 3);
      this.ssoAccessCheckBox.Name = "ssoAccessCheckBox";
      this.ssoAccessCheckBox.Size = new Size(151, 18);
      this.ssoAccessCheckBox.TabIndex = 129;
      this.ssoAccessCheckBox.Text = "Restricted to SSO Access";
      this.ssoAccessCheckBox.CheckStateChanged += new EventHandler(this.ssoAccessCheckBox_CheckedChanged);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.ForeColor = Color.FromArgb(238, 0, 0);
      this.label4.Location = new Point(-4, 7);
      this.label4.Name = "label4";
      this.label4.Size = new Size(13, 16);
      this.label4.TabIndex = 0;
      this.label4.Text = "*";
      this.passwTxt.Location = new Point(136, 1);
      this.passwTxt.MaxLength = 50;
      this.passwTxt.Name = "passwTxt";
      this.passwTxt.PasswordChar = '*';
      this.passwTxt.Size = new Size(203, 20);
      this.passwTxt.TabIndex = 0;
      this.newPasswordCheckBox.Location = new Point(136, 46);
      this.newPasswordCheckBox.Name = "newPasswordCheckBox";
      this.newPasswordCheckBox.Size = new Size(184, 15);
      this.newPasswordCheckBox.TabIndex = 2;
      this.newPasswordCheckBox.Text = "Force user to change password";
      this.verifyPwdTxt.Location = new Point(136, 23);
      this.verifyPwdTxt.MaxLength = 50;
      this.verifyPwdTxt.Name = "verifyPwdTxt";
      this.verifyPwdTxt.PasswordChar = '*';
      this.verifyPwdTxt.Size = new Size(203, 20);
      this.verifyPwdTxt.TabIndex = 1;
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.ForeColor = Color.FromArgb(238, 0, 0);
      this.label5.Location = new Point(-4, 29);
      this.label5.Name = "label5";
      this.label5.Size = new Size(13, 16);
      this.label5.TabIndex = 2;
      this.label5.Text = "*";
      this.verifyPwdLabel.AutoSize = true;
      this.verifyPwdLabel.BackColor = Color.Transparent;
      this.verifyPwdLabel.Location = new Point(7, 27);
      this.verifyPwdLabel.Name = "verifyPwdLabel";
      this.verifyPwdLabel.Size = new Size(93, 13);
      this.verifyPwdLabel.TabIndex = 6;
      this.verifyPwdLabel.Text = "Re-type Password";
      this.pwdLabel.AutoSize = true;
      this.pwdLabel.BackColor = Color.Transparent;
      this.pwdLabel.Location = new Point(7, 5);
      this.pwdLabel.Name = "pwdLabel";
      this.pwdLabel.Size = new Size(53, 13);
      this.pwdLabel.TabIndex = 1;
      this.pwdLabel.Text = "Password";
      this.label15.AutoSize = true;
      this.label15.BackColor = Color.Transparent;
      this.label15.Location = new Point(20, 185);
      this.label15.Name = "label15";
      this.label15.Size = new Size(36, 13);
      this.label15.TabIndex = 126;
      this.label15.Text = "Suffix ";
      this.suffixTxt.Location = new Point(148, 181);
      this.suffixTxt.MaxLength = 64;
      this.suffixTxt.Name = "suffixTxt";
      this.suffixTxt.Size = new Size(203, 20);
      this.suffixTxt.TabIndex = 8;
      this.label14.AutoSize = true;
      this.label14.BackColor = Color.Transparent;
      this.label14.Location = new Point(19, 141);
      this.label14.Name = "label14";
      this.label14.Size = new Size(69, 13);
      this.label14.TabIndex = 124;
      this.label14.Text = "Middle Name";
      this.middleNameTxt.Location = new Point(148, 137);
      this.middleNameTxt.MaxLength = 64;
      this.middleNameTxt.Name = "middleNameTxt";
      this.middleNameTxt.Size = new Size(203, 20);
      this.middleNameTxt.TabIndex = 6;
      this.btnPublicProfile.Location = new Point(365, 2);
      this.btnPublicProfile.Margin = new Padding(0);
      this.btnPublicProfile.Name = "btnPublicProfile";
      this.btnPublicProfile.Size = new Size(77, 22);
      this.btnPublicProfile.TabIndex = 20;
      this.btnPublicProfile.Text = "Public Profile";
      this.btnPublicProfile.Click += new EventHandler(this.btnPublicProfile_Click);
      this.label13.AutoSize = true;
      this.label13.BackColor = Color.Transparent;
      this.label13.Location = new Point(19, 229);
      this.label13.Name = "label13";
      this.label13.Size = new Size(107, 13);
      this.label13.TabIndex = 122;
      this.label13.Text = "Employee ID Number";
      this.employeeIDTxt.Location = new Point(148, 225);
      this.employeeIDTxt.MaxLength = 10;
      this.employeeIDTxt.Name = "employeeIDTxt";
      this.employeeIDTxt.Size = new Size(203, 20);
      this.employeeIDTxt.TabIndex = 10;
      this.dpNMLSExpiration.BackColor = SystemColors.Window;
      this.dpNMLSExpiration.Location = new Point(148, 403);
      this.dpNMLSExpiration.Margin = new Padding(4, 5, 4, 5);
      this.dpNMLSExpiration.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpNMLSExpiration.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpNMLSExpiration.Name = "dpNMLSExpiration";
      this.dpNMLSExpiration.Size = new Size(203, 21);
      this.dpNMLSExpiration.TabIndex = 18;
      this.dpNMLSExpiration.Tag = (object) "";
      this.dpNMLSExpiration.ToolTip = "";
      this.dpNMLSExpiration.Value = new DateTime(0L);
      this.label12.AutoSize = true;
      this.label12.BackColor = Color.Transparent;
      this.label12.Location = new Point(19, 405);
      this.label12.Name = "label12";
      this.label12.Size = new Size(112, 13);
      this.label12.TabIndex = 120;
      this.label12.Text = "NMLS Expiration Date";
      this.nmlsTextBox.Location = new Point(148, 381);
      this.nmlsTextBox.MaxLength = 12;
      this.nmlsTextBox.Name = "nmlsTextBox";
      this.nmlsTextBox.Size = new Size(203, 20);
      this.nmlsTextBox.TabIndex = 17;
      this.nmlsTextBox.TextChanged += new EventHandler(this.nmlsTextBox_TextChanged);
      this.nmlsTextBox.KeyDown += new KeyEventHandler(this.nmlsTextBox_KeyDown);
      this.label11.AutoSize = true;
      this.label11.BackColor = Color.Transparent;
      this.label11.Location = new Point(19, 383);
      this.label11.Name = "label11";
      this.label11.Size = new Size(126, 13);
      this.label11.TabIndex = 118;
      this.label11.Text = "NMLS Loan Originator ID";
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(19, 339);
      this.label3.Name = "label3";
      this.label3.Size = new Size(39, 13);
      this.label3.TabIndex = 111;
      this.label3.Text = "CHUM";
      this.folderLabel.AutoSize = true;
      this.folderLabel.BackColor = Color.Transparent;
      this.folderLabel.Location = new Point(19, 361);
      this.folderLabel.Name = "folderLabel";
      this.folderLabel.Size = new Size(79, 13);
      this.folderLabel.TabIndex = 24;
      this.folderLabel.Text = "Working Folder";
      this.userIDLabel.AutoSize = true;
      this.userIDLabel.BackColor = Color.Transparent;
      this.userIDLabel.Location = new Point(19, 31);
      this.userIDLabel.Name = "userIDLabel";
      this.userIDLabel.Size = new Size(43, 13);
      this.userIDLabel.TabIndex = 0;
      this.userIDLabel.Text = "User ID";
      this.emailLabel.AutoSize = true;
      this.emailLabel.BackColor = Color.Transparent;
      this.emailLabel.Location = new Point(19, 317);
      this.emailLabel.Name = "emailLabel";
      this.emailLabel.Size = new Size(32, 13);
      this.emailLabel.TabIndex = 16;
      this.emailLabel.Text = "Email";
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Location = new Point(19, 295);
      this.label7.Name = "label7";
      this.label7.Size = new Size(64, 13);
      this.label7.TabIndex = 108;
      this.label7.Text = "Fax Number";
      this.lnameLabel.AutoSize = true;
      this.lnameLabel.BackColor = Color.Transparent;
      this.lnameLabel.Location = new Point(19, 163);
      this.lnameLabel.Name = "lnameLabel";
      this.lnameLabel.Size = new Size(58, 13);
      this.lnameLabel.TabIndex = 10;
      this.lnameLabel.Text = "Last Name";
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(19, 273);
      this.label2.Name = "label2";
      this.label2.Size = new Size(98, 13);
      this.label2.TabIndex = 110;
      this.label2.Text = "Cell Phone Number";
      this.fnameLabel.AutoSize = true;
      this.fnameLabel.BackColor = Color.Transparent;
      this.fnameLabel.Location = new Point(19, 119);
      this.fnameLabel.Name = "fnameLabel";
      this.fnameLabel.Size = new Size(57, 13);
      this.fnameLabel.TabIndex = 8;
      this.fnameLabel.Text = "First Name";
      this.phoneLabel.AutoSize = true;
      this.phoneLabel.BackColor = Color.Transparent;
      this.phoneLabel.Location = new Point(19, 251);
      this.phoneLabel.Name = "phoneLabel";
      this.phoneLabel.Size = new Size(78, 13);
      this.phoneLabel.TabIndex = 14;
      this.phoneLabel.Text = "Phone Number";
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.ForeColor = Color.FromArgb(238, 0, 0);
      this.label9.Location = new Point(8, 120);
      this.label9.Name = "label9";
      this.label9.Size = new Size(13, 16);
      this.label9.TabIndex = 116;
      this.label9.Text = "*";
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Transparent;
      this.label8.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label8.ForeColor = Color.FromArgb(238, 0, 0);
      this.label8.Location = new Point(8, 163);
      this.label8.Name = "label8";
      this.label8.Size = new Size(13, 16);
      this.label8.TabIndex = 115;
      this.label8.Text = "*";
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.ForeColor = Color.FromArgb(238, 0, 0);
      this.label6.Location = new Point(8, 317);
      this.label6.Name = "label6";
      this.label6.Size = new Size(13, 16);
      this.label6.TabIndex = 114;
      this.label6.Text = "*";
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.FromArgb(238, 0, 0);
      this.label1.Location = new Point(8, 31);
      this.label1.Name = "label1";
      this.label1.Size = new Size(13, 16);
      this.label1.TabIndex = 17;
      this.label1.Text = "*";
      this.chumTextBox.Location = new Point(148, 335);
      this.chumTextBox.MaxLength = 10;
      this.chumTextBox.Name = "chumTextBox";
      this.chumTextBox.Size = new Size(203, 20);
      this.chumTextBox.TabIndex = 15;
      this.disableCheckBox.BackColor = Color.Transparent;
      this.disableCheckBox.Location = new Point(226, 4);
      this.disableCheckBox.Name = "disableCheckBox";
      this.disableCheckBox.Size = new Size(106, 18);
      this.disableCheckBox.TabIndex = 19;
      this.disableCheckBox.Text = "Disable Account";
      this.disableCheckBox.UseVisualStyleBackColor = false;
      this.folderComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.folderComboBox.Location = new Point(148, 357);
      this.folderComboBox.Name = "folderComboBox";
      this.folderComboBox.Size = new Size(203, 21);
      this.folderComboBox.TabIndex = 16;
      this.emailTextBox.Location = new Point(148, 313);
      this.emailTextBox.MaxLength = 64;
      this.emailTextBox.Name = "emailTextBox";
      this.emailTextBox.Size = new Size(203, 20);
      this.emailTextBox.TabIndex = 14;
      this.faxTextBox.Location = new Point(148, 291);
      this.faxTextBox.MaxLength = 25;
      this.faxTextBox.Name = "faxTextBox";
      this.faxTextBox.Size = new Size(203, 20);
      this.faxTextBox.TabIndex = 13;
      this.faxTextBox.TextChanged += new EventHandler(this.phoneTextBox_TextChanged);
      this.faxTextBox.KeyDown += new KeyEventHandler(this.phoneTextBox_KeyDown);
      this.txtCellPhone.Location = new Point(148, 269);
      this.txtCellPhone.MaxLength = 25;
      this.txtCellPhone.Name = "txtCellPhone";
      this.txtCellPhone.Size = new Size(203, 20);
      this.txtCellPhone.TabIndex = 12;
      this.txtCellPhone.TextChanged += new EventHandler(this.txtCellPhone_TextChanged);
      this.txtCellPhone.KeyDown += new KeyEventHandler(this.txtCellPhone_KeyDown);
      this.phoneTextBox.Location = new Point(148, 247);
      this.phoneTextBox.MaxLength = 25;
      this.phoneTextBox.Name = "phoneTextBox";
      this.phoneTextBox.Size = new Size(203, 20);
      this.phoneTextBox.TabIndex = 11;
      this.phoneTextBox.TextChanged += new EventHandler(this.phoneTextBox_TextChanged);
      this.phoneTextBox.KeyDown += new KeyEventHandler(this.phoneTextBox_KeyDown);
      this.fnameTxt.Location = new Point(148, 115);
      this.fnameTxt.MaxLength = 64;
      this.fnameTxt.Name = "fnameTxt";
      this.fnameTxt.Size = new Size(203, 20);
      this.fnameTxt.TabIndex = 5;
      this.lnameTxt.Location = new Point(148, 159);
      this.lnameTxt.MaxLength = 64;
      this.lnameTxt.Name = "lnameTxt";
      this.lnameTxt.Size = new Size(203, 20);
      this.lnameTxt.TabIndex = 7;
      this.useridTxt.CharacterCasing = CharacterCasing.Lower;
      this.useridTxt.Location = new Point(148, 27);
      this.useridTxt.MaxLength = 16;
      this.useridTxt.Name = "useridTxt";
      this.useridTxt.Size = new Size(203, 20);
      this.useridTxt.TabIndex = 0;
      this.useridTxt.TabStop = false;
      this.useridTxt.TextChanged += new EventHandler(this.useridTxt_TextChanged);
      this.useridTxt.KeyPress += new KeyPressEventHandler(this.useridTxt_KeyPress);
      this.useridTxt.Leave += new EventHandler(this.useridTxt_Leave);
      this.lockedCheckBox.BackColor = Color.Transparent;
      this.lockedCheckBox.Location = new Point(133, 4);
      this.lockedCheckBox.Name = "lockedCheckBox";
      this.lockedCheckBox.Size = new Size(91, 18);
      this.lockedCheckBox.TabIndex = 18;
      this.lockedCheckBox.Text = "Disable Login";
      this.lockedCheckBox.UseVisualStyleBackColor = false;
      this.grpGroups.Controls.Add((Control) this.flowLayoutPanel2);
      this.grpGroups.Controls.Add((Control) this.gvGroups);
      this.grpGroups.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.grpGroups.HeaderForeColor = SystemColors.ControlText;
      this.grpGroups.Location = new Point(553, 167);
      this.grpGroups.Name = "grpGroups";
      this.grpGroups.Size = new Size(320, 149);
      this.grpGroups.TabIndex = 3;
      this.grpGroups.Text = "Group Membership";
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnViewGroupRights);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(197, 2);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(117, 22);
      this.flowLayoutPanel2.TabIndex = 0;
      this.btnViewGroupRights.Location = new Point(17, 0);
      this.btnViewGroupRights.Margin = new Padding(0);
      this.btnViewGroupRights.Name = "btnViewGroupRights";
      this.btnViewGroupRights.Size = new Size(100, 22);
      this.btnViewGroupRights.TabIndex = 0;
      this.btnViewGroupRights.Text = "View Rights";
      this.btnViewGroupRights.Click += new EventHandler(this.btnViewGroupRights_Click);
      this.gvGroups.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "State";
      gvColumn1.Width = 318;
      this.gvGroups.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvGroups.Dock = DockStyle.Fill;
      this.gvGroups.HeaderHeight = 0;
      this.gvGroups.HeaderVisible = false;
      this.gvGroups.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvGroups.Location = new Point(1, 26);
      this.gvGroups.Name = "gvGroups";
      this.gvGroups.Selectable = false;
      this.gvGroups.Size = new Size(318, 122);
      this.gvGroups.TabIndex = 1;
      this.gvGroups.TabStop = false;
      this.grpLicenses.Controls.Add((Control) this.gvLicenses);
      this.grpLicenses.Controls.Add((Control) this.loBtn);
      this.grpLicenses.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.grpLicenses.HeaderForeColor = SystemColors.ControlText;
      this.grpLicenses.Location = new Point(9, 448);
      this.grpLicenses.Name = "grpLicenses";
      this.grpLicenses.Size = new Size(452, 102);
      this.grpLicenses.TabIndex = 1;
      this.grpLicenses.Text = "Loan Officer Active Licenses";
      this.gvLicenses.BorderStyle = BorderStyle.None;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.Text = "State";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "License Number";
      gvColumn3.Width = 264;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.Text = "End Date";
      gvColumn4.Width = 86;
      this.gvLicenses.Columns.AddRange(new GVColumn[3]
      {
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gvLicenses.Dock = DockStyle.Fill;
      this.gvLicenses.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLicenses.Location = new Point(1, 26);
      this.gvLicenses.Name = "gvLicenses";
      this.gvLicenses.Selectable = false;
      this.gvLicenses.Size = new Size(450, 75);
      this.gvLicenses.TabIndex = 0;
      this.gvLicenses.TabStop = false;
      this.grpLoanAccess.Controls.Add((Control) this.chbEditSubordinate);
      this.grpLoanAccess.Controls.Add((Control) this.chbPeerAccess);
      this.grpLoanAccess.Controls.Add((Control) this.rdbEdit);
      this.grpLoanAccess.Controls.Add((Control) this.rdbView);
      this.grpLoanAccess.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.grpLoanAccess.HeaderForeColor = SystemColors.ControlText;
      this.grpLoanAccess.Location = new Point(467, 448);
      this.grpLoanAccess.Name = "grpLoanAccess";
      this.grpLoanAccess.Size = new Size(406, 102);
      this.grpLoanAccess.TabIndex = 5;
      this.grpLoanAccess.Text = "Access to Subordinates' Loans";
      this.chbEditSubordinate.Location = new Point(10, 77);
      this.chbEditSubordinate.Name = "chbEditSubordinate";
      this.chbEditSubordinate.Size = new Size(184, 19);
      this.chbEditSubordinate.TabIndex = 4;
      this.chbEditSubordinate.Text = "Edit Subordinates' Loans";
      this.chbPeerAccess.CheckAlign = ContentAlignment.TopLeft;
      this.chbPeerAccess.Location = new Point(10, 35);
      this.chbPeerAccess.Name = "chbPeerAccess";
      this.chbPeerAccess.Size = new Size(204, 19);
      this.chbPeerAccess.TabIndex = 1;
      this.chbPeerAccess.Text = "Access to all loans in the same level in the organization hierarchy";
      this.chbPeerAccess.CheckedChanged += new EventHandler(this.chbPeerAccess_CheckedChanged);
      this.rdbEdit.Location = new Point(109, 57);
      this.rdbEdit.Name = "rdbEdit";
      this.rdbEdit.Size = new Size(44, 17);
      this.rdbEdit.TabIndex = 3;
      this.rdbEdit.Text = "Edit";
      this.rdbView.Location = new Point(27, 57);
      this.rdbView.Name = "rdbView";
      this.rdbView.Size = new Size(76, 17);
      this.rdbView.TabIndex = 2;
      this.rdbView.Text = "View Only";
      this.grpPersonas.Controls.Add((Control) this.label10);
      this.grpPersonas.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpPersonas.Controls.Add((Control) this.gvPersonas);
      this.grpPersonas.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.grpPersonas.HeaderForeColor = SystemColors.ControlText;
      this.grpPersonas.Location = new Point(554, 5);
      this.grpPersonas.Name = "grpPersonas";
      this.grpPersonas.Size = new Size(319, 157);
      this.grpPersonas.TabIndex = 2;
      this.grpPersonas.Text = "Personas";
      this.label10.AutoSize = true;
      this.label10.BackColor = Color.Transparent;
      this.label10.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.ForeColor = Color.FromArgb(238, 0, 0);
      this.label10.Location = new Point(1, 6);
      this.label10.Name = "label10";
      this.label10.Size = new Size(13, 16);
      this.label10.TabIndex = 0;
      this.label10.Text = "*";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnViewPersonaRights);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemovePersona);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAssignPersona);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(153, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(161, 22);
      this.flowLayoutPanel1.TabIndex = 0;
      this.btnViewPersonaRights.Location = new Point(61, 0);
      this.btnViewPersonaRights.Margin = new Padding(0);
      this.btnViewPersonaRights.Name = "btnViewPersonaRights";
      this.btnViewPersonaRights.Size = new Size(100, 22);
      this.btnViewPersonaRights.TabIndex = 0;
      this.btnViewPersonaRights.Text = "View/Edit Rights";
      this.btnViewPersonaRights.Click += new EventHandler(this.btnViewPersonaRights_Click);
      this.verticalSeparator1.Location = new Point(56, 3);
      this.verticalSeparator1.Margin = new Padding(1, 3, 3, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 21;
      this.verticalSeparator1.TabStop = false;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.gvPersonas.BorderStyle = BorderStyle.None;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column1";
      gvColumn5.SpringToFit = true;
      gvColumn5.Text = "State";
      gvColumn5.Width = 317;
      this.gvPersonas.Columns.AddRange(new GVColumn[1]
      {
        gvColumn5
      });
      this.gvPersonas.Dock = DockStyle.Fill;
      this.gvPersonas.HeaderHeight = 0;
      this.gvPersonas.HeaderVisible = false;
      this.gvPersonas.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPersonas.Location = new Point(1, 26);
      this.gvPersonas.Name = "gvPersonas";
      this.gvPersonas.Size = new Size(317, 130);
      this.gvPersonas.TabIndex = 2;
      this.gvPersonas.TabStop = false;
      this.gvPersonas.SelectedIndexChanged += new EventHandler(this.gvPersonas_SelectedIndexChanged);
      this.panelBottom.Controls.Add((Control) this.panelCCSite);
      this.panelBottom.Controls.Add((Control) this.okBtn);
      this.panelBottom.Controls.Add((Control) this.cancelBtn);
      this.panelBottom.Dock = DockStyle.Top;
      this.panelBottom.Location = new Point(0, 766);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(881, 118);
      this.panelBottom.TabIndex = 2;
      this.panelCCSite.Location = new Point(9, 5);
      this.panelCCSite.Name = "panelCCSite";
      this.panelCCSite.Size = new Size(863, 77);
      this.panelCCSite.TabIndex = 0;
      this.panelLOCompHistory.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.panelLOCompHistory.Location = new Point(466, 6);
      this.panelLOCompHistory.Name = "panelLOCompHistory";
      this.panelLOCompHistory.Size = new Size(406, 187);
      this.panelLOCompHistory.TabIndex = 1;
      this.panelLOCompLatest.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.panelLOCompLatest.Location = new Point(10, 5);
      this.panelLOCompLatest.Name = "panelLOCompLatest";
      this.panelLOCompLatest.Size = new Size(403, 187);
      this.panelLOCompLatest.TabIndex = 0;
      this.panelMiddle.Controls.Add((Control) this.panelLOCompLatest);
      this.panelMiddle.Controls.Add((Control) this.panelLOCompHistory);
      this.panelMiddle.Dock = DockStyle.Top;
      this.panelMiddle.Location = new Point(0, 560);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new Size(881, 206);
      this.panelMiddle.TabIndex = 1;
      this.contMenu.MenuItems.AddRange(new MenuItem[2]
      {
        this.miLinkTo,
        this.miDisconTo
      });
      this.miLinkTo.Index = 0;
      this.miLinkTo.Text = "Link with Organization SSO Settings";
      this.miLinkTo.Click += new EventHandler(this.miLinkTo_Click);
      this.miDisconTo.Index = 1;
      this.miDisconTo.Text = "Disconnect from Organization SSO Settings";
      this.miDisconTo.Click += new EventHandler(this.miDisconTo_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(881, 882);
      this.Controls.Add((Control) this.panelBottom);
      this.Controls.Add((Control) this.panelMiddle);
      this.Controls.Add((Control) this.panelTop);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddEditUserCEDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "User Details";
      this.Shown += new EventHandler(this.AddEditDialog_Warning);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      ((ISupportInitialize) this.btnRemovePersona).EndInit();
      ((ISupportInitialize) this.btnAssignPersona).EndInit();
      ((ISupportInitialize) this.loBtn).EndInit();
      this.panelTop.ResumeLayout(false);
      this.grpComments.ResumeLayout(false);
      this.grpAcctInfo.ResumeLayout(false);
      this.grpAcctInfo.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.grpGroups.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.grpLicenses.ResumeLayout(false);
      this.grpLoanAccess.ResumeLayout(false);
      this.grpPersonas.ResumeLayout(false);
      this.grpPersonas.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.panelBottom.ResumeLayout(false);
      this.panelMiddle.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void showOAuthPanelOrNot(object sender, EventArgs e)
    {
      if (this.apiUserCheckBox.Checked)
      {
        this.panel2.Visible = true;
        this.panel1.Visible = false;
        this.passwTxt.Text = "";
        this.verifyPwdTxt.Text = "";
        this.ssoAccessCheckBox.Checked = false;
      }
      else
      {
        this.panel2.Visible = false;
        this.panel1.Visible = true;
        this.oAuthIDtext.Text = "";
        this.allowImpersonationCheckBox.Checked = false;
        this.ssoAccessCheckBox.Checked = this.currentOrgInfo.SSOSettings.LoginAccess;
      }
    }

    private LOLicenseInfo[] createDefaultLOLicenses() => new LOLicenseInfo[0];

    private void loadCurrentAssignedGroup()
    {
      this.assignedGroup = this.session.AclGroupManager.GetGroupsOfUser(this.Userid);
      foreach (object itemValue in this.assignedGroup)
        this.gvGroups.Items.Add(itemValue);
      if (this.assignedGroup.Length != 0)
        return;
      this.btnViewGroupRights.Enabled = false;
    }

    private void loadDefaultAssignedGroup()
    {
      this.assignedGroup = this.session.AclGroupManager.GetGroupsOfOrganization(this._OrgId);
      foreach (object itemValue in this.assignedGroup)
        this.gvGroups.Items.Add(itemValue);
      if (this.assignedGroup.Length != 0)
        return;
      this.btnViewGroupRights.Enabled = false;
    }

    private void setDefaultStateLicenses()
    {
      if (this.userid == "" || this.userid == null)
        return;
      string[] states = Utils.GetStates();
      this.loLicInfo = new LOLicenseInfo[states.Length];
      for (int index = 0; index < states.Length; ++index)
      {
        LOLicenseInfo loLicenseInfo = new LOLicenseInfo(this.userid, states[index], true, "", DateTime.MaxValue);
        this.loLicInfo[index] = loLicenseInfo;
      }
    }

    private void initFolderComboBox(string workingFolder)
    {
      this.folderComboBox.Items.Clear();
      LoanFolderInfo[] allLoanFolderInfos = this.session.LoanManager.GetAllLoanFolderInfos(false, false);
      if (allLoanFolderInfos == null)
        return;
      this.folderComboBox.Items.AddRange((object[]) allLoanFolderInfos);
      if (workingFolder != null)
      {
        for (int index = 0; index < this.folderComboBox.Items.Count; ++index)
        {
          if (((LoanFolderInfo) this.folderComboBox.Items[index]).Name == workingFolder)
          {
            this.folderComboBox.SelectedIndex = index;
            return;
          }
        }
      }
      if (allLoanFolderInfos.Length == 0)
        return;
      this.folderComboBox.SelectedIndex = 0;
      if (!SystemSettings.ArchiveFolder.Equals(allLoanFolderInfos[0].Name) || allLoanFolderInfos.Length <= 1)
        return;
      this.folderComboBox.SelectedIndex = 1;
      for (int index = 0; index < this.folderComboBox.Items.Count; ++index)
      {
        if (((LoanFolderInfo) this.folderComboBox.Items[index]).Name == "My Pipeline")
        {
          this.folderComboBox.SelectedIndex = index;
          break;
        }
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (!this.saveUser())
        return;
      this.DialogResult = DialogResult.OK;
    }

    private bool verifyInput()
    {
      if (this.useridTxt.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a user ID.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.useridTxt.Focus();
        return false;
      }
      if (this.lnameTxt.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter both a user last name and first name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.lnameTxt.Focus();
        return false;
      }
      if (this.fnameTxt.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter both a user last name and first name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.fnameTxt.Focus();
        return false;
      }
      if (this.editUser && (this.passwTxt.Text.Trim() != string.Empty || this.verifyPwdTxt.Text.Trim() != string.Empty) && string.Compare(this.useridTxt.Text, "tpowcadmin", true) == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You cannot change the password for the tpowcadmin user account here. Your TPO WebCenter website relies on this tpowcadmin account to communicate with your Encompass system. Modifying this account's password will cause your TPO WebCenter to stop working and prevent loan data from being passed between your website and your Encompass system. To safely update this account's password, use the Loan Settings page in TPO WebCenter Administration.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!this.apiUserCheckBox.Checked && !this.verifyPwdTxt.Text.Equals(this.passwTxt.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The passwords do not match. The password is case-sensitive. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.passwTxt.Focus();
        return false;
      }
      if (this.gvPersonas.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a persona for the user.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (string.Compare(this.useridTxt.Text, "tpowcadmin", true) == 0 && !this.validateTPOWebCenterAdminUserPersona())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Your TPO WebCenter website relies on this tpowcadmin account to communicate with your Encompass system. Please set Super Administrator persona to this user!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.apiUserCheckBox.Checked && this.oAuthIDtext.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter an OAuth Client Id for an API User.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.oAuthIDtext.Focus();
        return false;
      }
      if (!this.apiUserCheckBox.Checked && (!this.editUser || this.editUser && this.passwTxt.Text.Length != 0) && !this.ssoAccessCheckBox.Checked && !this.validatePassword(this.passwTxt.Text))
      {
        this.passwTxt.Focus();
        return false;
      }
      if (!this.editUser && this.session.OrganizationManager.UserExists(this.useridTxt.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The User ID that you entered is already in use.  Please try a different ID.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.useridTxt.Focus();
        return false;
      }
      if (this.apiUserCheckBox.Checked && (this.currentUser == (UserInfo) null || (this.currentUser.OAuthClientId ?? "") != this.oAuthIDtext.Text.Trim()) && this.session.OrganizationManager.OAuthClientIdExists(this.oAuthIDtext.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The OAuth Client ID that you entered is already in use.  Please try a different ID.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.oAuthIDtext.Focus();
        return false;
      }
      if (!this.editUser && this.apiUserCheckBox.Checked && Utils.Dialog((IWin32Window) this, "Once created, an API user cannot be converted back to regular user. Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
        return false;
      if (!this.disableCheckBox.Checked)
      {
        if (this.emailTextBox.Text.Trim() == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Many Encompass features require an email address. Please enter one at this time.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.emailTextBox.Focus();
          return false;
        }
        if (!Utils.ValidateEmail(this.emailTextBox.Text.Trim()))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The email address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.emailTextBox.Focus();
          return false;
        }
      }
      if (this.ccSiteControl.userChangedURLText && !this.ccSiteControl.doSearch())
        return false;
      if (this.editUser && !this.ssoAccessCheckBox.Checked && !this.currentUser.PasswordExists && string.IsNullOrEmpty(this.passwTxt.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Password cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.passwTxt.Focus();
        return false;
      }
      if (this.folderComboBox.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "No loan folders found in Working Folder. Please contact the administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.folderComboBox.Focus();
        return false;
      }
      this.userid = this.useridTxt.Text;
      for (int index = 0; index < this.loLicInfo.Length; ++index)
        this.loLicInfo[index].UserID = this.userid;
      this.lname = this.lnameTxt.Text;
      this.suffixName = this.suffixTxt.Text.Trim();
      this.fname = this.fnameTxt.Text;
      this.jobtitle = this.jobtitleTxt.Text.Trim();
      this.middleName = this.middleNameTxt.Text.Trim();
      this.employeeID = this.employeeIDTxt.Text.Trim();
      this.phone = this.phoneTextBox.Text;
      this.cellPhone = this.txtCellPhone.Text;
      this.fax = this.faxTextBox.Text;
      this.email = this.emailTextBox.Text.Trim();
      this.locked = this.lockedCheckBox.Checked;
      this.requirePwdChange = this.newPasswordCheckBox.Checked;
      this.chumID = this.chumTextBox.Text.Trim();
      this.nmlsOriginatorID = this.nmlsTextBox.Text.Trim();
      this.nmlsExpirationDate = this.dpNMLSExpiration.Value == DateTime.MinValue ? DateTime.MaxValue : this.dpNMLSExpiration.Value;
      this.status = !this.disableCheckBox.Checked ? UserInfo.UserStatusEnum.Enabled : UserInfo.UserStatusEnum.Disabled;
      this.password = !(this.passwTxt.Text == string.Empty) || !this.editUser ? this.passwTxt.Text : (string) null;
      this.workingFolder = ((LoanFolderInfo) this.folderComboBox.SelectedItem).Name;
      this.peerView = !this.chbPeerAccess.Checked ? UserInfo.UserPeerView.Disabled : (!this.rdbView.Checked ? UserInfo.UserPeerView.Edit : UserInfo.UserPeerView.ViewOnly);
      this.accessMode = this.apiUserCheckBox.Checked || this.chbEditSubordinate.Checked ? UserInfo.AccessModeEnum.ReadWrite : UserInfo.AccessModeEnum.ReadOnly;
      return this.status != UserInfo.UserStatusEnum.Enabled || this.validateLicense();
    }

    private bool validateLicense()
    {
      if (this.editUser && this.initialStatus == UserInfo.UserStatusEnum.Enabled)
        return true;
      LicenseInfo serverLicense = this.session.ConfigurationManager.GetServerLicense();
      if (serverLicense.UserLimit <= 0)
        return true;
      int enabledUserCount = this.session.OrganizationManager.GetEnabledUserCount();
      if (enabledUserCount < serverLicense.UserLimitWithFlex)
      {
        DialogResult res = DialogResult.OK;
        if (enabledUserCount > 5 && enabledUserCount <= 14)
        {
          if (serverLicense.UserLimit - enabledUserCount == 2)
            res = this.DisplayWarningAlert(res, serverLicense.UserLimit, enabledUserCount);
          else if (serverLicense.UserLimit - enabledUserCount == 1)
            res = this.DisplayFinalWarningAlert(res, serverLicense.UserLimit, enabledUserCount);
        }
        else if (enabledUserCount > 14 && enabledUserCount <= 999)
        {
          if (serverLicense.UserLimit - enabledUserCount == 1)
            res = this.DisplayFinalWarningAlert(res, serverLicense.UserLimit, enabledUserCount);
          else if (enabledUserCount >= serverLicense.UserLimitWith90Percent)
            res = this.DisplayWarningAlert(res, serverLicense.UserLimit, enabledUserCount);
        }
        else if (enabledUserCount > 999)
        {
          if (serverLicense.UserLimit - enabledUserCount == 1)
            res = this.DisplayFinalWarningAlert(res, serverLicense.UserLimit, enabledUserCount);
          else if (serverLicense.UserLimit - enabledUserCount <= 100)
            res = this.DisplayWarningAlert(res, serverLicense.UserLimit, enabledUserCount);
        }
        return res != DialogResult.Cancel;
      }
      this.DisplayHardStopAlert(serverLicense.UserLimit);
      return false;
    }

    private DialogResult DisplayWarningAlert(DialogResult res, int userLimit, int userCount)
    {
      if (this.editUser && this.initialStatus == UserInfo.UserStatusEnum.Disabled)
        res = Utils.Dialog((IWin32Window) this, "Your Encompass software is currently licensed for a maximum of " + (object) userLimit + " enabled users. By enabling this user you will have " + (object) (userCount + 1) + " enabled users. You are approaching your licensing limit and may need additional licenses soon to enable or create more users." + Environment.NewLine + Environment.NewLine + "Please consider contacting your ICE Mortgage Technology Account Representative to purchase additional Encompass licenses.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
      else if (!this.editUser)
        res = Utils.Dialog((IWin32Window) this, "Your Encompass software is currently licensed for a maximum of " + (object) userLimit + " enabled users. By creating this user you will have " + (object) (userCount + 1) + " active users. You are approaching your licensing limit and may need additional licenses soon to create or enable more users." + Environment.NewLine + Environment.NewLine + "Please consider contacting your ICE Mortgage Technology Account Representative to purchase additional Encompass licenses.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
      RemoteLogger.Write(TraceLevel.Warning, "License warning message displayed.");
      return res;
    }

    private DialogResult DisplayFinalWarningAlert(DialogResult res, int userLimit, int userCount)
    {
      if (this.editUser && this.initialStatus == UserInfo.UserStatusEnum.Disabled)
        res = Utils.Dialog((IWin32Window) this, "Your Encompass software is currently licensed for a maximum of " + (object) userLimit + " enabled users. By enabling this user, you will reach that limit and will no longer be able to enable or add more users." + Environment.NewLine + Environment.NewLine + "To enable or create more users in future, please contact your ICE Mortgage Technology Account Representative to purchase additional Encompass licenses.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
      else if (!this.editUser)
        res = Utils.Dialog((IWin32Window) this, "Your Encompass software is currently licensed for a maximum of " + (object) userLimit + " users. By creating this user, you will reach that limit and will no longer be able to add or enable more users." + Environment.NewLine + Environment.NewLine + "To create or enable more users in future, please contact your ICE Mortgage Technology Account Representative to purchase additional Encompass licenses.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
      RemoteLogger.Write(TraceLevel.Warning, "License hard stop message displayed.");
      return res;
    }

    private void DisplayHardStopAlert(int userLimit)
    {
      if (this.editUser && this.initialStatus == UserInfo.UserStatusEnum.Disabled)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This user cannot be enabled because you have already exceeded the maximum number of active accounts (" + (object) userLimit + ") permitted by your license. You have " + (object) userLimit + " active users in the system." + Environment.NewLine + Environment.NewLine + "To enable this user, contact your ICE Mortgage Technology Account Representative to purchase additional Encompass licenses, or delete/disable unused user accounts.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (!this.editUser)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "This user cannot be created because you have already reached the maximum number of active accounts (" + (object) userLimit + ") permitted by your license. You have " + (object) userLimit + " active users in the system." + Environment.NewLine + Environment.NewLine + "To create this new user, contact your ICE Mortgage Technology Account Representative to purchase additional Encompass licenses, delete/disable unused user accounts, or mark this user's account as disabled.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      RemoteLogger.Write(TraceLevel.Warning, "License hard stop message displayed.");
    }

    private void useridTxt_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar) || (char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '_' || e.KeyChar == '@' || e.KeyChar == '-' || e.KeyChar == '.') && this.useridTxt.Text.Length <= 16)
        return;
      e.Handled = true;
    }

    private void phoneTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void phoneTextBox_TextChanged(object sender, EventArgs e)
    {
      if (this.intermidiateData)
        this.intermidiateData = false;
      else if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        FieldFormat fieldFormat;
        FieldFormat dataFormat = fieldFormat = FieldFormat.PHONE;
        TextBox textBox = (TextBox) sender;
        bool needsUpdate = false;
        string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
        if (!needsUpdate)
          return;
        this.intermidiateData = true;
        textBox.Text = str;
        textBox.SelectionStart = str.Length;
      }
    }

    private void loBtn_Click(object sender, EventArgs e)
    {
      using (LoanOfficerLicenseDialog officerLicenseDialog = new LoanOfficerLicenseDialog(this.loLicInfo, this.useridTxt.Text.Trim(), false))
      {
        if (officerLicenseDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.loLicInfo = officerLicenseDialog.LoLicInfo;
        this.refreshLicenses();
      }
    }

    private void refreshLicenses()
    {
      this.gvLicenses.Items.Clear();
      foreach (LOLicenseInfo loLicenseInfo in this.loLicInfo)
      {
        if (loLicenseInfo.Enabled)
          this.gvLicenses.Items.Add(new GVItem(Utils.GetFullStateName(loLicenseInfo.StateAbbr))
          {
            SubItems = {
              [1] = {
                Text = loLicenseInfo.License
              },
              [2] = {
                Text = loLicenseInfo.ExpirationDate == DateTime.MaxValue ? "" : loLicenseInfo.ExpirationDate.ToString("MM/dd/yyyy")
              }
            }
          });
      }
      this.gvLicenses.ReSort();
      if (this.gvLicenses.Items.Count == LoanOfficerLicenseDialog.StateList.Length)
        this.grpLicenses.Text = "Loan Officer Active Licenses (All " + (object) LoanOfficerLicenseDialog.StateList.Length + ")";
      else
        this.grpLicenses.Text = "Loan Officer Active Licenses (" + (object) this.gvLicenses.Items.Count + ")";
    }

    private bool saveUser()
    {
      if (!this.verifyInput())
        return false;
      bool flag = true;
      string str1 = string.Empty;
      if (this.currentUser != (UserInfo) null && this.currentUser.Userid == this.session.UserID)
        str1 = this.session.Password;
      UserInfo userInfo = this.getUserInfo();
      userInfo.PersonaAccessComments = this.PersonaCommentsTextBox.Text;
      userInfo.JobTitle = this.jobtitle;
      if (!this.editUser)
      {
        if (this.apiUserCheckBox.Checked)
          userInfo.Password = Guid.NewGuid().ToString();
        this.session.OrganizationManager.CreateNewUser(userInfo);
        this.session.OrganizationManager.SetLOLicenses(userInfo.Userid, this.loLicInfo);
        if (this.ccSiteControl.ccSiteSettings != null)
        {
          this.session.OrganizationManager.CreateUserCCSiteInfo(this.ccSiteControl.ccSiteSettings, userInfo.Userid);
          userInfo.InheritParentCCSite = this.ccSiteControl.ccSiteSettings.UseParentInfo;
        }
        if (this.loanCompHistoryList != null)
          this.session.ConfigurationManager.CreateHistoryCompPlansForUser(this.loanCompHistoryList, userInfo.Userid);
        flag = false;
        this.apiUserCheckBox.Enabled = false;
        this.editUser = true;
      }
      else
      {
        if (!this.session.OrganizationManager.UserExists(userInfo.Userid))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "User does not exist.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        this.session.OrganizationManager.SetLOLicenses(userInfo.Userid, this.loLicInfo);
        if (this.loanCompHistoryList != null)
          this.session.ConfigurationManager.CreateHistoryCompPlansForUser(this.loanCompHistoryList, userInfo.Userid);
        if (this.ccSiteControl.ccSiteSettings != null)
          this.session.OrganizationManager.UpdateUserCCSiteInfo(this.ccSiteControl.ccSiteSettings, userInfo.Userid);
        userInfo.InheritParentCCSite = this.ccSiteControl.ccSiteSettings.UseParentInfo;
        this.session.OrganizationManager.UpdateUser(userInfo);
        if (userInfo.Userid == this.session.UserID)
          this.session.RecacheUserInfo();
      }
      this.currentUser = userInfo;
      string empty = string.Empty;
      string str2 = string.Empty;
      if (this.currentUser.Userid != this.session.UserID && !flag)
        str2 = EMNetworkPwdMgr.AddUser(this.passwTxt.Text, this.session, userInfo);
      this.CleanUpPersonalizedSetting(this.currentUser);
      this.cancelBtn.DialogResult = DialogResult.OK;
      this.dataSaved = true;
      this.btnViewGroupRights.Enabled = true;
      if (this.personas != null && this.personas.Length != 0)
        this.btnViewPersonaRights.Enabled = true;
      return true;
    }

    public bool DataSaved => this.dataSaved;

    public UserInfo getUserInfo()
    {
      bool personalStatusOnline = false;
      if (this.currentUser != (UserInfo) null)
        personalStatusOnline = this.currentUser.PersonalStatusOnline;
      return new UserInfo(this.Userid, this.Password, this.LastName, this.SuffixName, this.FirstName, this.MiddleName, this.EmployeeID, "", this.personas, this.WorkingFolder, this._OrgId, false, this.AccessMode, this.Status, this.Email, this.Phone, this.CellPhone, this.Fax, false, this.RequirePasswordChange, this.Locked, this.peerView, "", false, DateTime.MinValue, this.ChumID, this.NMLSOriginatorID, this.nmlsExpirationDate, "", "", personalStatusOnline, this.loanCompHistoryList != null && this.loanCompHistoryList.UseParentInfo, this.apiUserCheckBox.Checked, this.oAuthIDtext.Text.Trim() == "" ? (string) null : this.oAuthIDtext.Text.Trim(), this.allowImpersonationCheckBox.Checked, this.ccSiteControl.ccSiteSettings == null || this.ccSiteControl.ccSiteSettings.UseParentInfo, ssoOnly: this.ssoAccessCheckBox.Checked, ssoDisconnectedFromOrg: this.lblOrgUnlink.Visible);
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (AddEditUserCEDialog));
    }

    private void btnViewPersonaRights_Click(object sender, EventArgs e)
    {
      if (!this.editUser)
      {
        if (Utils.Dialog((IWin32Window) this, "Before you can view/edit persona rights, you need to save this new account first.  Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes || !this.saveUser())
          return;
      }
      else
      {
        bool flag1 = true;
        Persona[] userPersonas = this.currentUser.UserPersonas;
        if (userPersonas != null && userPersonas.Length != 0)
        {
          flag1 = false;
          if (this.personas.Length != userPersonas.Length)
          {
            flag1 = true;
          }
          else
          {
            foreach (Persona persona1 in this.personas)
            {
              bool flag2 = false;
              foreach (Persona persona2 in userPersonas)
              {
                if (persona1.ID == persona2.ID)
                {
                  flag2 = true;
                  break;
                }
              }
              if (!flag2)
              {
                flag1 = true;
                break;
              }
            }
          }
        }
        if (flag1 && (Utils.Dialog((IWin32Window) this, "Before you can view/edit persona rights, you need to save your new settings first.  Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes || !this.saveUser()))
          return;
      }
      using (PersonaSettingsMainForm settingsMainForm = new PersonaSettingsMainForm((IWin32Window) this, this.session, this.userid, this.personas))
      {
        int num = (int) settingsMainForm.ShowDialog((IWin32Window) this);
      }
    }

    private void btnAssignPersona_Click(object sender, EventArgs e)
    {
      using (PersonaSelectionForm personaSelectionForm = new PersonaSelectionForm(this.session, this.personas, this.session.UserInfo, this.currentUser, this.currentOrgID))
      {
        if (DialogResult.Cancel == personaSelectionForm.ShowDialog((IWin32Window) this))
          return;
        this.personas = personaSelectionForm.SelectedPersonas;
        this.refreshPersonas();
      }
    }

    private void refreshPersonas()
    {
      this.gvPersonas.Items.Clear();
      if (this.personas != null && this.personas.Length != 0)
      {
        foreach (object persona in this.personas)
          this.gvPersonas.Items.Add(persona);
        this.btnViewPersonaRights.Enabled = this.currentUser == (UserInfo) null || !this.currentUser.IsSuperAdministrator();
        this.btnViewGroupRights.Enabled = this.currentUser == (UserInfo) null || !this.currentUser.IsSuperAdministrator();
      }
      else
        this.btnViewPersonaRights.Enabled = false;
    }

    private void btnAssignGroup_Click(object sender, EventArgs e)
    {
      using (SecurityGroupSelectionForm groupSelectionForm = new SecurityGroupSelectionForm(this.Userid))
      {
        int num = (int) groupSelectionForm.ShowDialog((IWin32Window) this);
      }
    }

    private void btnViewGroupRights_Click(object sender, EventArgs e)
    {
      using (SecurityGroupSettingsMainForm settingsMainForm = new SecurityGroupSettingsMainForm(this.session, this.owner, this.Userid, this.assignedGroup))
      {
        int num = (int) settingsMainForm.ShowDialog((IWin32Window) this);
      }
    }

    private bool validatePassword(string password)
    {
      PwdRuleValidator passwordValidator = this.session.OrganizationManager.GetPasswordValidator();
      if (!passwordValidator.CheckMinLength(password))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The password must be at least " + (object) passwordValidator.MinimumLength + " characters long.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (passwordValidator.CheckCompositionRule(password))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The password must contain the following:" + Environment.NewLine + Environment.NewLine + passwordValidator.GetCompositionRuleDescription(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void chbPeerAccess_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chbPeerAccess.Checked)
      {
        this.rdbView.Checked = true;
        this.rdbView.Enabled = true;
        this.rdbEdit.Enabled = true;
      }
      else
      {
        this.rdbView.Checked = false;
        this.rdbView.Enabled = false;
        this.rdbEdit.Checked = false;
        this.rdbEdit.Enabled = false;
      }
    }

    private void reLoad()
    {
      this.useridTxt.Text = this.userid;
      this.disableCheckBox.Checked = this.currentUser.Status == UserInfo.UserStatusEnum.Disabled;
      this.passwTxt.Text = "";
      this.verifyPwdTxt.Text = this.passwTxt.Text;
      this.lnameTxt.Text = this.currentUser.LastName;
      this.suffixTxt.Text = this.currentUser.SuffixName;
      this.fnameTxt.Text = this.currentUser.FirstName;
      this.jobtitleTxt.Text = this.currentUser.JobTitle;
      this.middleNameTxt.Text = this.currentUser.MiddleName;
      this.employeeIDTxt.Text = this.currentUser.EmployeeID;
      this.newPasswordCheckBox.Checked = this.currentUser.RequirePasswordChange;
      this.lockedCheckBox.Checked = this.currentUser.Locked;
      this.apiUserCheckBox.Checked = this.currentUser.ApiUser;
      if (this.apiUserCheckBox.Checked)
      {
        this.oAuthIDtext.Text = this.currentUser.OAuthClientId == null ? "" : this.currentUser.OAuthClientId;
        this.allowImpersonationCheckBox.Checked = this.currentUser.AllowImpersonation;
      }
      else
      {
        this.oAuthIDtext.Text = "";
        this.allowImpersonationCheckBox.Checked = false;
      }
      this.intermidiateData = true;
      this.phoneTextBox.Text = this.currentUser.Phone;
      this.txtCellPhone.Text = this.currentUser.CellPhone;
      this.faxTextBox.Text = this.currentUser.Fax;
      this.emailTextBox.Text = this.currentUser.Email;
      this.initialStatus = this.currentUser.Status;
      this.chumTextBox.Text = this.currentUser.CHUMId;
      this.nmlsTextBox.Text = this.currentUser.NMLSOriginatorID;
      this.dpNMLSExpiration.Value = this.currentUser.NMLSExpirationDate == DateTime.MaxValue ? DateTime.MinValue : this.currentUser.NMLSExpirationDate;
      this.dpNMLSExpiration.Enabled = this.currentUser.NMLSOriginatorID != string.Empty;
      this.PersonaCommentsTextBox.Text = this.currentUser.PersonaAccessComments;
      for (int index = 0; index < this.folderComboBox.Items.Count; ++index)
      {
        if (((LoanFolderInfo) this.folderComboBox.Items[index]).Name == this.currentUser.WorkingFolder)
        {
          this.folderComboBox.SelectedItem = (object) index;
          break;
        }
      }
      this.chbEditSubordinate.Checked = this.currentUser.AccessMode == UserInfo.AccessModeEnum.ReadWrite;
      if (this.currentUser.PeerView == UserInfo.UserPeerView.ViewOnly)
      {
        this.chbPeerAccess.Checked = true;
        this.rdbView.Checked = true;
      }
      else if (this.currentUser.PeerView == UserInfo.UserPeerView.Edit)
      {
        this.chbPeerAccess.Checked = true;
        this.rdbEdit.Checked = true;
      }
      else
      {
        this.rdbEdit.Enabled = false;
        this.rdbView.Enabled = false;
      }
      this.useridTxt.ReadOnly = true;
      this.personas = this.currentUser.UserPersonas;
      if (this.currentUser.UserPersonas != null && this.currentUser.UserPersonas.Length != 0)
      {
        ArrayList arrayList = new ArrayList();
        foreach (Persona userPersona in this.currentUser.UserPersonas)
        {
          foreach (Persona persona in this.personaList)
          {
            if (persona.ID == userPersona.ID)
              arrayList.Add((object) persona);
          }
        }
        this.personas = (Persona[]) arrayList.ToArray(typeof (Persona));
        this.refreshPersonas();
      }
      else
        this.btnViewPersonaRights.Enabled = false;
      this.loadCurrentAssignedGroup();
      this.refreshLicenses();
    }

    private void cancelBtn_Click(object sender, EventArgs e)
    {
      if (!this.editUser)
        return;
      this.reLoad();
    }

    private void CleanUpPersonalizedSetting(UserInfo userObj)
    {
      if (userObj.UserPersonas == null || userObj.UserPersonas.Length == 0)
        return;
      bool flag = false;
      foreach (Persona userPersona in userObj.UserPersonas)
      {
        if (userPersona.ID == 1 || userPersona.ID == 0)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      this.RemovePersonalization(userObj);
    }

    private void RemovePersonalization(UserInfo userObj)
    {
      FeaturesAclManager aclManager1 = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      ArrayList arrayList = new ArrayList();
      arrayList.AddRange((ICollection) FeatureSets.BizContacts);
      arrayList.AddRange((ICollection) FeatureSets.BorContacts);
      arrayList.AddRange((ICollection) FeatureSets.Contacts);
      arrayList.AddRange((ICollection) FeatureSets.Features);
      arrayList.AddRange((ICollection) FeatureSets.PipelineGlobalTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoanMgmtFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoansPrintFeatures);
      arrayList.AddRange((ICollection) FeatureSets.SettingsTabPersonalFeatures);
      arrayList.AddRange((ICollection) FeatureSets.SettingsTabCompanyFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ToolsFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoanEFolderFeatures);
      arrayList.AddRange((ICollection) FeatureSets.LoanOtherFeatures);
      arrayList.AddRange((ICollection) FeatureSets.DashboardFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ReportFeatures);
      arrayList.AddRange((ICollection) FeatureSets.TradeFeatures);
      arrayList.AddRange((ICollection) FeatureSets.HomeFeatures);
      arrayList.AddRange((ICollection) FeatureSets.EMClosingDocsFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ExternalSettingTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.TPOAdministrationTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.ConsumerConnectTabFeatures);
      arrayList.AddRange((ICollection) FeatureSets.TPOSiteSettingsTabFeatures);
      IEnumerator enumerator = aclManager1.GetPermissions((AclFeature[]) arrayList.ToArray(typeof (AclFeature)), userObj.Userid).Keys.GetEnumerator();
      Hashtable featureAccesses = new Hashtable();
      while (enumerator.MoveNext())
        featureAccesses.Add(enumerator.Current, (object) AclTriState.Unspecified);
      if (featureAccesses.Count > 0)
        aclManager1.SetPermissions(featureAccesses, userObj.Userid);
      InputFormsAclManager aclManager2 = (InputFormsAclManager) this.session.ACL.GetAclManager(AclCategory.InputForms);
      Hashtable permissionsForAllForms = aclManager2.GetPermissionsForAllForms(userObj.Userid);
      if (permissionsForAllForms != null && permissionsForAllForms.Count > 0)
      {
        foreach (object key in (IEnumerable) permissionsForAllForms.Keys)
          aclManager2.SetPermission(string.Concat(key), userObj.Userid, (object) null);
      }
      ((MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones)).DeleteUserSpecificSetting(userObj.Userid);
      ((ToolsAclManager) this.session.ACL.GetAclManager(AclCategory.ToolsGrantWriteAccess)).SetPermissions((ToolsAclInfo[]) null, userObj.Userid);
      LoanFoldersAclManager aclManager3 = (LoanFoldersAclManager) this.session.ACL.GetAclManager(AclCategory.LoanFolderMove);
      aclManager3.SetPermissions(AclFeature.LoanMgmt_Move, (LoanFolderAclInfo[]) null, userObj.Userid);
      aclManager3.SetPermissions(AclFeature.LoanMgmt_Import, (LoanFolderAclInfo[]) null, userObj.Userid);
    }

    private void txtCellPhone_TextChanged(object sender, EventArgs e)
    {
      if (this.intermidiateData)
        this.intermidiateData = false;
      else if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        FieldFormat fieldFormat;
        FieldFormat dataFormat = fieldFormat = FieldFormat.PHONE;
        TextBox textBox = (TextBox) sender;
        bool needsUpdate = false;
        string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
        if (!needsUpdate)
          return;
        this.intermidiateData = true;
        textBox.Text = str;
        textBox.SelectionStart = str.Length;
      }
    }

    private void txtCellPhone_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void gvPersonas_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.userid == "admin" || string.Compare(this.userid, "tpowcadmin", true) == 0)
      {
        foreach (GVItem selectedItem in this.gvPersonas.SelectedItems)
        {
          if (selectedItem.Value.Equals((object) Persona.SuperAdministrator))
          {
            this.btnRemovePersona.Enabled = false;
            return;
          }
        }
      }
      this.btnRemovePersona.Enabled = this.gvPersonas.SelectedItems.Count > 0;
    }

    private void btnRemovePersona_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList((ICollection) this.personas);
      foreach (GVItem selectedItem in this.gvPersonas.SelectedItems)
        arrayList.Remove(selectedItem.Value);
      this.personas = (Persona[]) arrayList.ToArray(typeof (Persona));
      this.refreshPersonas();
    }

    private void nmlsTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void nmlsTextBox_TextChanged(object sender, EventArgs e)
    {
      this.dpNMLSExpiration.Enabled = this.nmlsTextBox.Text.Trim() != string.Empty;
      if (this.nmlsTextBox.Text.Trim() == string.Empty)
        this.dpNMLSExpiration.Text = string.Empty;
      if (this.intermidiateData)
        this.intermidiateData = false;
      else if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        Regex regex = new Regex("^\\d+$");
        if (this.nmlsTextBox.Text.Length <= 0 || regex.IsMatch(this.nmlsTextBox.Text))
          return;
        this.nmlsTextBox.Text = this.nmlsTextBox.Text.Substring(0, this.nmlsTextBox.Text.Length - 1);
        this.nmlsTextBox.SelectionStart = this.nmlsTextBox.Text.Length;
      }
    }

    private void useridTxt_TextChanged(object sender, EventArgs e)
    {
      bool flag = false;
      if (this.useridTxt.Text.Length > 16)
        flag = true;
      else if (this.useridTxt.Text.IndexOf('.') == 0)
      {
        flag = true;
      }
      else
      {
        foreach (char c in this.useridTxt.Text)
        {
          if (!char.IsLetterOrDigit(c) && c != '_' && c != '@' && c != '-' && c != '.')
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "The provided UserID is not in valid format.");
      this.useridTxt.Text = "";
    }

    private void loCompHistoryControl_HistorySelectedIndexChanged(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
    }

    private void loCompCurrentControl_UseParentInfoClicked(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
    }

    private void loCompHistoryControl_AssignPlanButtonClicked(object sender, EventArgs e)
    {
      this.loCompCurrentControl.RefreshPlanDetails((LoanCompHistory) sender);
    }

    private void loCompCurrentControl_StartDateChanged(object sender, EventArgs e)
    {
      this.loCompHistoryControl.RefreshHistoryList((LoanCompHistory) sender);
    }

    private void disableCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (string.Compare(this.useridTxt.Text.Trim(), "tpowcadmin", true) != 0 || !this.disableCheckBox.Checked)
        return;
      this.disableCheckBox.CheckedChanged -= new EventHandler(this.disableCheckBox_CheckedChanged);
      int num = (int) Utils.Dialog((IWin32Window) this, "You cannot delete or disable the tpowcadmin  user account. Your TPO WebCenter website relies on this tpowcadmin  account to communicate with your Encompass system. Disabling or deleting this account will cause your TPO WebCenter to stop working and prevent loan data from being passed between your website and your Encompass system.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.disableCheckBox.Checked = false;
      this.disableCheckBox.CheckedChanged += new EventHandler(this.disableCheckBox_CheckedChanged);
    }

    private void lockedCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (string.Compare(this.useridTxt.Text.Trim(), "tpowcadmin", true) != 0 || !this.lockedCheckBox.Checked)
        return;
      this.lockedCheckBox.CheckedChanged -= new EventHandler(this.lockedCheckBox_CheckedChanged);
      int num = (int) Utils.Dialog((IWin32Window) this, "You cannot delete or disable the tpowcadmin  user account. Your TPO WebCenter website relies on this tpowcadmin  account to communicate with your Encompass system. Disabling or deleting this account will cause your TPO WebCenter to stop working and prevent loan data from being passed between your website and your Encompass system.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.lockedCheckBox.Checked = false;
      this.lockedCheckBox.CheckedChanged += new EventHandler(this.lockedCheckBox_CheckedChanged);
    }

    private void useridTxt_Leave(object sender, EventArgs e)
    {
      if (this.useridTxt.Text.EndsWith("."))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The provided UserID is not in valid format.");
        this.useridTxt.Text = "";
        this.useridTxt.Focus();
      }
      else
      {
        if (string.Compare(this.useridTxt.Text.Trim(), "tpowcadmin", true) != 0)
          return;
        this.newPasswordCheckBox.Enabled = this.newPasswordCheckBox.Checked = false;
      }
    }

    private bool validateTPOWebCenterAdminUserPersona()
    {
      if (this.gvPersonas.Items.Count == 0)
        return false;
      for (int nItemIndex = 0; nItemIndex < this.gvPersonas.Items.Count; ++nItemIndex)
      {
        if (string.Compare(this.gvPersonas.Items[nItemIndex].Text, "Super Administrator", true) == 0)
          return true;
      }
      return false;
    }

    private void btnPublicProfile_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.userid))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must save the user to access the public profile.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.okBtn.Focus();
      }
      else
      {
        using (PublicProfileDialog publicProfileDialog = new PublicProfileDialog(this.userid))
        {
          if (publicProfileDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
            return;
          this.currentuserProfile = publicProfileDialog.getUserProfileInfo();
        }
      }
    }

    private void AddEditDialog_Warning(object sender, EventArgs e)
    {
      if (!this.ccSiteControl.invalidCCSiteURL)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Consumer Connect Site URL is not valid or not available. If you have linked this site from a parent organization, please change the linked site URL from that level.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.ccSiteControl.invalidCCSiteURL = false;
    }

    private void miLinkTo_Click(object sender, EventArgs e) => this.connectUserSSOToOrg();

    private void miDisconTo_Click(object sender, EventArgs e)
    {
      this.disconnectUserSSOFromOrg(true);
    }

    private void connectUserSSOToOrg()
    {
      this.disconnectUserSSOFromOrg(false);
      this.ssoAccessCheckBox.Checked = (this.currentOrgInfo.SSOSettings.UseParentInfo ? this.session.OrganizationManager.GetFirstOrganizationForSSO(this.currentOrgInfo.Oid) : this.currentOrgInfo).SSOSettings.LoginAccess;
    }

    private void disconnectUserSSOFromOrg(bool disconnect)
    {
      this.lblOrgUnlink.Visible = disconnect;
      this.lblOrgLink.Visible = !disconnect;
    }

    private void ssoAccessCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      bool flag = this.ssoAccessCheckBox.Checked;
      if (flag)
      {
        this.passwTxt.Text = string.Empty;
        this.verifyPwdTxt.Text = string.Empty;
      }
      if (flag != this.currentOrgInfo.SSOSettings.LoginAccess && !this.apiUserCheckBox.Checked)
        this.disconnectUserSSOFromOrg(true);
      this.passwTxt.Enabled = !this.ssoAccessCheckBox.Checked;
      this.verifyPwdTxt.Enabled = !this.ssoAccessCheckBox.Checked;
      this.newPasswordCheckBox.Enabled = !this.ssoAccessCheckBox.Checked;
    }

    private bool IsRestrictedSSOEnabled()
    {
      string ssoEnabledResponse = new OAuth2(this.session.StartupInfo.OAPIGatewayBaseUri).GetRestrictedSSOEnabledResponse(this.session.ServerIdentity.InstanceName, this.session.SessionID, "sc");
      if (string.IsNullOrWhiteSpace(ssoEnabledResponse))
        return false;
      object obj = JsonConvert.DeserializeObject<object>(ssoEnabledResponse);
      return obj != null && ((JToken) obj).HasValues;
    }
  }
}
