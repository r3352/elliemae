// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.RightsAssignmentForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class RightsAssignmentForm : Form
  {
    private const string FullRights = "FullRights";
    private const string RWRights = "RWRights";
    private UserInfoSummary[] users;
    private Hashtable roleInfos = new Hashtable();
    private IOrganizationManager rOrg;
    private LoanReportFieldDefs fieldDefs;
    private IContainer components;
    private Label folderLabel;
    private RadioButton rwRadioButton;
    private ComboBox folderCombo;
    private RadioButton bothRadioButton;
    private RadioButton rvkGrantRadioButton;
    private RadioButton rvkBothRadioButton;
    private Button assignButton;
    private Button revokeButton;
    private ContextMenu contextMenu1;
    private MenuItem showDetailsMenuItem;
    private MenuItem showAllMenuItem;
    private GridView gvLoans;
    private ComboBox roleCombo;
    private Label label3;
    private Label label4;
    private ComboBox cmbRole;
    private TextBox txtUserName;
    private Button btnGo;
    private StandardIconButton btnReset;
    private GroupContainer gcRight;
    private GradientPanel gradPnlAssign;
    private Label lblAssign;
    private Panel pnlAssignRevoke;
    private GradientPanel gradPnlRevoke;
    private Label lblRevoke;
    private GradientPanel gradPnlRoleTop;
    private Label lblRole;
    private Label label5;
    private Panel pnlRole;
    private GroupContainer gcLeft;
    private Splitter pnlSplitter;
    private GradientPanel gradPnlTopLeft;
    private GroupContainer gcLoans;
    private StandardIconButton btnSelectUser;
    private Splitter splitterGC2;
    private GradientPanel pnlAssign;
    private Panel pnlRevoke;
    private GridView gvUserList;
    private ToolTip toolTip1;
    private StandardIconButton btnSelectTPO;
    private ComboBox cboCompany;
    private Label label1;
    private TextBox txtTPOName;
    private List<ExternalOriginatorManagementData> externalOrgsList;
    private Sessions.Session session;

    public RightsAssignmentForm(Sessions.Session session)
    {
      this.session = session;
      this.rOrg = this.session.OrganizationManager;
      this.InitializeComponent();
      this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllDatabaseFields);
      this.roleInfos = new Hashtable();
      this.initFolderCombo();
      this.initRoleCombo();
      this.loadCompanyDropDown();
      this.cboCompany_SelectedIndexChanged((object) null, (EventArgs) null);
      this.loadExternalOrgs();
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      this.contextMenu1 = new ContextMenu();
      this.showDetailsMenuItem = new MenuItem();
      this.showAllMenuItem = new MenuItem();
      this.pnlSplitter = new Splitter();
      this.toolTip1 = new ToolTip(this.components);
      this.btnSelectUser = new StandardIconButton();
      this.btnReset = new StandardIconButton();
      this.btnSelectTPO = new StandardIconButton();
      this.gcLeft = new GroupContainer();
      this.gcLoans = new GroupContainer();
      this.gvLoans = new GridView();
      this.gradPnlTopLeft = new GradientPanel();
      this.cboCompany = new ComboBox();
      this.label1 = new Label();
      this.txtTPOName = new TextBox();
      this.label5 = new Label();
      this.cmbRole = new ComboBox();
      this.btnGo = new Button();
      this.folderCombo = new ComboBox();
      this.folderLabel = new Label();
      this.label3 = new Label();
      this.txtUserName = new TextBox();
      this.label4 = new Label();
      this.gcRight = new GroupContainer();
      this.pnlRole = new Panel();
      this.gvUserList = new GridView();
      this.gradPnlRoleTop = new GradientPanel();
      this.lblRole = new Label();
      this.roleCombo = new ComboBox();
      this.pnlAssignRevoke = new Panel();
      this.pnlRevoke = new Panel();
      this.rvkGrantRadioButton = new RadioButton();
      this.gradPnlRevoke = new GradientPanel();
      this.revokeButton = new Button();
      this.lblRevoke = new Label();
      this.rvkBothRadioButton = new RadioButton();
      this.pnlAssign = new GradientPanel();
      this.bothRadioButton = new RadioButton();
      this.gradPnlAssign = new GradientPanel();
      this.assignButton = new Button();
      this.lblAssign = new Label();
      this.rwRadioButton = new RadioButton();
      this.splitterGC2 = new Splitter();
      ((ISupportInitialize) this.btnSelectUser).BeginInit();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSelectTPO).BeginInit();
      this.gcLeft.SuspendLayout();
      this.gcLoans.SuspendLayout();
      this.gradPnlTopLeft.SuspendLayout();
      this.gcRight.SuspendLayout();
      this.pnlRole.SuspendLayout();
      this.gradPnlRoleTop.SuspendLayout();
      this.pnlAssignRevoke.SuspendLayout();
      this.pnlRevoke.SuspendLayout();
      this.gradPnlRevoke.SuspendLayout();
      this.pnlAssign.SuspendLayout();
      this.gradPnlAssign.SuspendLayout();
      this.SuspendLayout();
      this.contextMenu1.MenuItems.AddRange(new MenuItem[2]
      {
        this.showDetailsMenuItem,
        this.showAllMenuItem
      });
      this.showDetailsMenuItem.Index = 0;
      this.showDetailsMenuItem.Text = "Show details of selected loans";
      this.showDetailsMenuItem.Click += new EventHandler(this.showDetailsMenuItem_Click);
      this.showAllMenuItem.Index = 1;
      this.showAllMenuItem.Text = "Show details of all loans";
      this.showAllMenuItem.Click += new EventHandler(this.showAllMenuItem_Click);
      this.pnlSplitter.BackColor = Color.WhiteSmoke;
      this.pnlSplitter.Dock = DockStyle.Right;
      this.pnlSplitter.Location = new Point(564, 0);
      this.pnlSplitter.Name = "pnlSplitter";
      this.pnlSplitter.Size = new Size(3, 544);
      this.pnlSplitter.TabIndex = 28;
      this.pnlSplitter.TabStop = false;
      this.btnSelectUser.BackColor = Color.Transparent;
      this.btnSelectUser.Location = new Point(486, 27);
      this.btnSelectUser.MouseDownImage = (Image) null;
      this.btnSelectUser.Name = "btnSelectUser";
      this.btnSelectUser.Size = new Size(16, 17);
      this.btnSelectUser.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelectUser.TabIndex = 4;
      this.btnSelectUser.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSelectUser, "Find");
      this.btnSelectUser.Click += new EventHandler(this.btnSelectUser_Click);
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(713, 27);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 17);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 9;
      this.btnReset.TabStop = false;
      this.btnReset.Text = "Reset";
      this.toolTip1.SetToolTip((Control) this.btnReset, "Reset");
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSelectTPO.BackColor = Color.Transparent;
      this.btnSelectTPO.Location = new Point(248, 27);
      this.btnSelectTPO.MouseDownImage = (Image) null;
      this.btnSelectTPO.Name = "btnSelectTPO";
      this.btnSelectTPO.Size = new Size(16, 17);
      this.btnSelectTPO.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelectTPO.TabIndex = 12;
      this.btnSelectTPO.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSelectTPO, "Find");
      this.btnSelectTPO.Click += new EventHandler(this.btnSelectTPO_Click);
      this.gcLeft.Controls.Add((Control) this.gcLoans);
      this.gcLeft.Controls.Add((Control) this.gradPnlTopLeft);
      this.gcLeft.Dock = DockStyle.Fill;
      this.gcLeft.HeaderForeColor = SystemColors.ControlText;
      this.gcLeft.Location = new Point(0, 0);
      this.gcLeft.Name = "gcLeft";
      this.gcLeft.Size = new Size(564, 544);
      this.gcLeft.TabIndex = 27;
      this.gcLeft.Text = "1. Select loans.";
      this.gcLoans.Borders = AnchorStyles.None;
      this.gcLoans.Controls.Add((Control) this.gvLoans);
      this.gcLoans.Dock = DockStyle.Fill;
      this.gcLoans.HeaderForeColor = SystemColors.ControlText;
      this.gcLoans.Location = new Point(1, 106);
      this.gcLoans.Name = "gcLoans";
      this.gcLoans.Size = new Size(562, 437);
      this.gcLoans.TabIndex = 4;
      this.gcLoans.Text = "Loans (0)";
      this.gvLoans.AllowColumnReorder = true;
      this.gvLoans.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Tag = (object) "Loan.BorrowerName";
      gvColumn1.Text = "Borrower Name";
      gvColumn1.Width = 99;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Tag = (object) "FullRights";
      gvColumn2.Text = "Full Right";
      gvColumn2.Width = 76;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Tag = (object) "RWRights";
      gvColumn3.Text = "R/W Right";
      gvColumn3.Width = 75;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Tag = (object) "Loan.LoanNumber";
      gvColumn4.Text = "Loan #";
      gvColumn4.Width = 77;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Tag = (object) "Loan.LockStatus";
      gvColumn5.Text = "Lock Status";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Tag = (object) "Loan.Address1";
      gvColumn6.Text = "Address";
      gvColumn6.Width = 150;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Tag = (object) "Loan.LoanAmount";
      gvColumn7.Text = "Amount";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 80;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.Tag = (object) "Loan.LoanType";
      gvColumn8.Text = "Type";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column9";
      gvColumn9.Tag = (object) "Loan.LoanPurpose";
      gvColumn9.Text = "Purpose";
      gvColumn9.Width = 100;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column10";
      gvColumn10.Tag = (object) "Loan.CoBorrowerName";
      gvColumn10.Text = "Co-Borrower";
      gvColumn10.Width = 120;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column11";
      gvColumn11.Tag = (object) "Loan.LoanName";
      gvColumn11.Text = "Loan Name";
      gvColumn11.Width = 80;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column12";
      gvColumn12.Tag = (object) "Loan.Active";
      gvColumn12.Text = "Loan Status";
      gvColumn12.Width = 100;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column13";
      gvColumn13.Tag = (object) "Loan.GUID";
      gvColumn13.Text = "GUID";
      gvColumn13.Width = 150;
      this.gvLoans.Columns.AddRange(new GVColumn[13]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.gvLoans.Dock = DockStyle.Fill;
      this.gvLoans.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLoans.Location = new Point(0, 25);
      this.gvLoans.Name = "gvLoans";
      this.gvLoans.Size = new Size(562, 412);
      this.gvLoans.SortOption = GVSortOption.Owner;
      this.gvLoans.TabIndex = 3;
      this.gvLoans.SortItems += new GVColumnSortEventHandler(this.gvLoans_SortItems);
      this.gradPnlTopLeft.Borders = AnchorStyles.Bottom;
      this.gradPnlTopLeft.Controls.Add((Control) this.btnSelectTPO);
      this.gradPnlTopLeft.Controls.Add((Control) this.cboCompany);
      this.gradPnlTopLeft.Controls.Add((Control) this.label1);
      this.gradPnlTopLeft.Controls.Add((Control) this.txtTPOName);
      this.gradPnlTopLeft.Controls.Add((Control) this.btnSelectUser);
      this.gradPnlTopLeft.Controls.Add((Control) this.btnReset);
      this.gradPnlTopLeft.Controls.Add((Control) this.label5);
      this.gradPnlTopLeft.Controls.Add((Control) this.cmbRole);
      this.gradPnlTopLeft.Controls.Add((Control) this.btnGo);
      this.gradPnlTopLeft.Controls.Add((Control) this.folderCombo);
      this.gradPnlTopLeft.Controls.Add((Control) this.folderLabel);
      this.gradPnlTopLeft.Controls.Add((Control) this.label3);
      this.gradPnlTopLeft.Controls.Add((Control) this.txtUserName);
      this.gradPnlTopLeft.Controls.Add((Control) this.label4);
      this.gradPnlTopLeft.Dock = DockStyle.Top;
      this.gradPnlTopLeft.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradPnlTopLeft.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradPnlTopLeft.Location = new Point(1, 26);
      this.gradPnlTopLeft.Name = "gradPnlTopLeft";
      this.gradPnlTopLeft.Size = new Size(562, 80);
      this.gradPnlTopLeft.TabIndex = 2;
      this.cboCompany.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCompany.Items.AddRange(new object[2]
      {
        (object) "Internal Organization",
        (object) "TPO"
      });
      this.cboCompany.Location = new Point(10, 26);
      this.cboCompany.Name = "cboCompany";
      this.cboCompany.Size = new Size(125, 22);
      this.cboCompany.TabIndex = 13;
      this.cboCompany.SelectedIndexChanged += new EventHandler(this.cboCompany_SelectedIndexChanged);
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(88, 17);
      this.label1.TabIndex = 11;
      this.label1.Text = "Company";
      this.txtTPOName.BackColor = SystemColors.Control;
      this.txtTPOName.Location = new Point(140, 26);
      this.txtTPOName.Name = "txtTPOName";
      this.txtTPOName.ReadOnly = true;
      this.txtTPOName.Size = new Size(105, 20);
      this.txtTPOName.TabIndex = 14;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(504, 29);
      this.label5.Name = "label5";
      this.label5.Size = new Size(15, 14);
      this.label5.TabIndex = 10;
      this.label5.Text = "In";
      this.cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRole.Location = new Point(270, 26);
      this.cmbRole.Name = "cmbRole";
      this.cmbRole.Size = new Size(105, 22);
      this.cmbRole.TabIndex = 5;
      this.cmbRole.SelectedIndexChanged += new EventHandler(this.cmbRole_SelectedIndexChanged);
      this.btnGo.BackColor = SystemColors.Control;
      this.btnGo.Location = new Point(647, 24);
      this.btnGo.Name = "btnGo";
      this.btnGo.Size = new Size(61, 22);
      this.btnGo.TabIndex = 8;
      this.btnGo.Text = "Search";
      this.btnGo.UseVisualStyleBackColor = true;
      this.btnGo.Click += new EventHandler(this.btnGo_Click);
      this.folderCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.folderCombo.Location = new Point(520, 26);
      this.folderCombo.Name = "folderCombo";
      this.folderCombo.Size = new Size(122, 22);
      this.folderCombo.TabIndex = 1;
      this.folderLabel.BackColor = Color.Transparent;
      this.folderLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.folderLabel.Location = new Point(520, 8);
      this.folderLabel.Name = "folderLabel";
      this.folderLabel.Size = new Size(108, 17);
      this.folderLabel.TabIndex = 2;
      this.folderLabel.Text = "Loan Folder";
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(270, 8);
      this.label3.Name = "label3";
      this.label3.Size = new Size(88, 17);
      this.label3.TabIndex = 3;
      this.label3.Text = "Role";
      this.txtUserName.BackColor = SystemColors.Window;
      this.txtUserName.Location = new Point(379, 26);
      this.txtUserName.Name = "txtUserName";
      this.txtUserName.ReadOnly = true;
      this.txtUserName.Size = new Size(105, 20);
      this.txtUserName.TabIndex = 6;
      this.label4.BackColor = Color.Transparent;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(379, 8);
      this.label4.Name = "label4";
      this.label4.Size = new Size(48, 17);
      this.label4.TabIndex = 4;
      this.label4.Text = "Name";
      this.gcRight.Controls.Add((Control) this.pnlRole);
      this.gcRight.Dock = DockStyle.Right;
      this.gcRight.HeaderForeColor = SystemColors.ControlText;
      this.gcRight.Location = new Point(567, 0);
      this.gcRight.Name = "gcRight";
      this.gcRight.Size = new Size(292, 544);
      this.gcRight.TabIndex = 26;
      this.gcRight.Text = "2. Select a user to grant access to or revoke from.";
      this.pnlRole.Controls.Add((Control) this.gvUserList);
      this.pnlRole.Controls.Add((Control) this.gradPnlRoleTop);
      this.pnlRole.Controls.Add((Control) this.pnlAssignRevoke);
      this.pnlRole.Dock = DockStyle.Fill;
      this.pnlRole.Location = new Point(1, 26);
      this.pnlRole.MinimumSize = new Size(50, 54);
      this.pnlRole.Name = "pnlRole";
      this.pnlRole.Size = new Size(290, 517);
      this.pnlRole.TabIndex = 1;
      this.gvUserList.BorderStyle = BorderStyle.None;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column1";
      gvColumn14.SpringToFit = true;
      gvColumn14.Text = "Column";
      gvColumn14.Width = 290;
      this.gvUserList.Columns.AddRange(new GVColumn[1]
      {
        gvColumn14
      });
      this.gvUserList.Dock = DockStyle.Fill;
      this.gvUserList.HeaderHeight = 0;
      this.gvUserList.HeaderVisible = false;
      this.gvUserList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvUserList.Location = new Point(0, 36);
      this.gvUserList.Name = "gvUserList";
      this.gvUserList.Size = new Size(290, 365);
      this.gvUserList.TabIndex = 7;
      this.gradPnlRoleTop.Borders = AnchorStyles.Bottom;
      this.gradPnlRoleTop.Controls.Add((Control) this.lblRole);
      this.gradPnlRoleTop.Controls.Add((Control) this.roleCombo);
      this.gradPnlRoleTop.Dock = DockStyle.Top;
      this.gradPnlRoleTop.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradPnlRoleTop.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradPnlRoleTop.Location = new Point(0, 0);
      this.gradPnlRoleTop.Name = "gradPnlRoleTop";
      this.gradPnlRoleTop.Size = new Size(290, 36);
      this.gradPnlRoleTop.TabIndex = 1;
      this.lblRole.AutoSize = true;
      this.lblRole.BackColor = Color.Transparent;
      this.lblRole.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRole.Location = new Point(3, 12);
      this.lblRole.Name = "lblRole";
      this.lblRole.Size = new Size(33, 13);
      this.lblRole.TabIndex = 15;
      this.lblRole.Text = "Role";
      this.roleCombo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.roleCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.roleCombo.Location = new Point(42, 7);
      this.roleCombo.Name = "roleCombo";
      this.roleCombo.Size = new Size(237, 22);
      this.roleCombo.TabIndex = 5;
      this.roleCombo.SelectedIndexChanged += new EventHandler(this.roleCombo_SelectedIndexChanged);
      this.pnlAssignRevoke.Controls.Add((Control) this.pnlRevoke);
      this.pnlAssignRevoke.Controls.Add((Control) this.pnlAssign);
      this.pnlAssignRevoke.Dock = DockStyle.Bottom;
      this.pnlAssignRevoke.Location = new Point(0, 401);
      this.pnlAssignRevoke.MinimumSize = new Size(50, 54);
      this.pnlAssignRevoke.Name = "pnlAssignRevoke";
      this.pnlAssignRevoke.Size = new Size(290, 116);
      this.pnlAssignRevoke.TabIndex = 0;
      this.pnlAssignRevoke.SizeChanged += new EventHandler(this.pnlAssignRevoke_SizeChanged);
      this.pnlRevoke.Controls.Add((Control) this.rvkGrantRadioButton);
      this.pnlRevoke.Controls.Add((Control) this.gradPnlRevoke);
      this.pnlRevoke.Controls.Add((Control) this.rvkBothRadioButton);
      this.pnlRevoke.Dock = DockStyle.Fill;
      this.pnlRevoke.Location = new Point(147, 0);
      this.pnlRevoke.Name = "pnlRevoke";
      this.pnlRevoke.Size = new Size(143, 116);
      this.pnlRevoke.TabIndex = 20;
      this.rvkGrantRadioButton.Checked = true;
      this.rvkGrantRadioButton.Location = new Point(8, 36);
      this.rvkGrantRadioButton.Name = "rvkGrantRadioButton";
      this.rvkGrantRadioButton.Size = new Size(128, 28);
      this.rvkGrantRadioButton.TabIndex = 17;
      this.rvkGrantRadioButton.TabStop = true;
      this.rvkGrantRadioButton.Text = "Assignment Right";
      this.gradPnlRevoke.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradPnlRevoke.Controls.Add((Control) this.revokeButton);
      this.gradPnlRevoke.Controls.Add((Control) this.lblRevoke);
      this.gradPnlRevoke.Dock = DockStyle.Top;
      this.gradPnlRevoke.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradPnlRevoke.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradPnlRevoke.Location = new Point(0, 0);
      this.gradPnlRevoke.Name = "gradPnlRevoke";
      this.gradPnlRevoke.Size = new Size(143, 32);
      this.gradPnlRevoke.TabIndex = 1;
      this.revokeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.revokeButton.BackColor = SystemColors.Control;
      this.revokeButton.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.revokeButton.Location = new Point(61, 5);
      this.revokeButton.Name = "revokeButton";
      this.revokeButton.Size = new Size(75, 22);
      this.revokeButton.TabIndex = 19;
      this.revokeButton.Text = "&Revoke";
      this.revokeButton.UseVisualStyleBackColor = true;
      this.revokeButton.Click += new EventHandler(this.revokeButton_Click);
      this.lblRevoke.AutoSize = true;
      this.lblRevoke.BackColor = Color.Transparent;
      this.lblRevoke.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRevoke.Location = new Point(5, 10);
      this.lblRevoke.Name = "lblRevoke";
      this.lblRevoke.Size = new Size(51, 13);
      this.lblRevoke.TabIndex = 15;
      this.lblRevoke.Text = "Revoke";
      this.rvkBothRadioButton.Location = new Point(8, 67);
      this.rvkBothRadioButton.Name = "rvkBothRadioButton";
      this.rvkBothRadioButton.Size = new Size(128, 36);
      this.rvkBothRadioButton.TabIndex = 18;
      this.rvkBothRadioButton.Text = "Full Right (R/W and assignment rights)";
      this.pnlAssign.Borders = AnchorStyles.Right;
      this.pnlAssign.Controls.Add((Control) this.bothRadioButton);
      this.pnlAssign.Controls.Add((Control) this.gradPnlAssign);
      this.pnlAssign.Controls.Add((Control) this.rwRadioButton);
      this.pnlAssign.Dock = DockStyle.Left;
      this.pnlAssign.GradientColor1 = Color.WhiteSmoke;
      this.pnlAssign.GradientColor2 = Color.WhiteSmoke;
      this.pnlAssign.Location = new Point(0, 0);
      this.pnlAssign.Name = "pnlAssign";
      this.pnlAssign.Size = new Size(147, 116);
      this.pnlAssign.TabIndex = 19;
      this.bothRadioButton.BackColor = Color.Transparent;
      this.bothRadioButton.Location = new Point(9, 67);
      this.bothRadioButton.Name = "bothRadioButton";
      this.bothRadioButton.Size = new Size(128, 34);
      this.bothRadioButton.TabIndex = 11;
      this.bothRadioButton.Text = "Full Right (R/W and assignment rights)";
      this.bothRadioButton.UseVisualStyleBackColor = false;
      this.gradPnlAssign.BackColor = Color.WhiteSmoke;
      this.gradPnlAssign.Borders = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.gradPnlAssign.Controls.Add((Control) this.assignButton);
      this.gradPnlAssign.Controls.Add((Control) this.lblAssign);
      this.gradPnlAssign.Dock = DockStyle.Top;
      this.gradPnlAssign.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradPnlAssign.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradPnlAssign.Location = new Point(0, 0);
      this.gradPnlAssign.Name = "gradPnlAssign";
      this.gradPnlAssign.Size = new Size(147, 32);
      this.gradPnlAssign.TabIndex = 0;
      this.assignButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.assignButton.BackColor = SystemColors.Control;
      this.assignButton.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.assignButton.Location = new Point(65, 5);
      this.assignButton.Name = "assignButton";
      this.assignButton.Size = new Size(75, 22);
      this.assignButton.TabIndex = 14;
      this.assignButton.Text = "&Assign";
      this.assignButton.UseVisualStyleBackColor = true;
      this.assignButton.Click += new EventHandler(this.assignButton_Click);
      this.lblAssign.AutoSize = true;
      this.lblAssign.BackColor = Color.Transparent;
      this.lblAssign.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblAssign.Location = new Point(6, 10);
      this.lblAssign.Name = "lblAssign";
      this.lblAssign.Size = new Size(44, 13);
      this.lblAssign.TabIndex = 15;
      this.lblAssign.Text = "Assign";
      this.rwRadioButton.BackColor = Color.Transparent;
      this.rwRadioButton.Checked = true;
      this.rwRadioButton.Location = new Point(9, 36);
      this.rwRadioButton.Name = "rwRadioButton";
      this.rwRadioButton.Size = new Size(128, 28);
      this.rwRadioButton.TabIndex = 8;
      this.rwRadioButton.TabStop = true;
      this.rwRadioButton.Text = "R/W Right";
      this.rwRadioButton.UseVisualStyleBackColor = false;
      this.splitterGC2.BackColor = Color.FromArgb(233, 242, (int) byte.MaxValue);
      this.splitterGC2.Dock = DockStyle.Right;
      this.splitterGC2.Location = new Point(289, 25);
      this.splitterGC2.Name = "splitterGC2";
      this.splitterGC2.Size = new Size(3, 453);
      this.splitterGC2.TabIndex = 2;
      this.splitterGC2.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(859, 544);
      this.ContextMenu = this.contextMenu1;
      this.Controls.Add((Control) this.gcLeft);
      this.Controls.Add((Control) this.pnlSplitter);
      this.Controls.Add((Control) this.gcRight);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (RightsAssignmentForm);
      this.Text = nameof (RightsAssignmentForm);
      ((ISupportInitialize) this.btnSelectUser).EndInit();
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSelectTPO).EndInit();
      this.gcLeft.ResumeLayout(false);
      this.gcLoans.ResumeLayout(false);
      this.gradPnlTopLeft.ResumeLayout(false);
      this.gradPnlTopLeft.PerformLayout();
      this.gcRight.ResumeLayout(false);
      this.pnlRole.ResumeLayout(false);
      this.gradPnlRoleTop.ResumeLayout(false);
      this.gradPnlRoleTop.PerformLayout();
      this.pnlAssignRevoke.ResumeLayout(false);
      this.pnlRevoke.ResumeLayout(false);
      this.gradPnlRevoke.ResumeLayout(false);
      this.gradPnlRevoke.PerformLayout();
      this.pnlAssign.ResumeLayout(false);
      this.gradPnlAssign.ResumeLayout(false);
      this.gradPnlAssign.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initFolderCombo()
    {
      this.folderCombo.Items.Clear();
      this.folderCombo.Items.Add((object) new LoanFolderInfo(SystemSettings.AllFolders));
      LoanFolderInfo[] allLoanFolderInfos = this.session.LoanManager.GetAllLoanFolderInfos(false);
      this.folderCombo.Items.AddRange((object[]) allLoanFolderInfos);
      this.folderCombo.SelectedIndex = -1;
      for (int index = 1; index < this.folderCombo.Items.Count; ++index)
      {
        if (string.Compare(((LoanFolderInfo) this.folderCombo.Items[index]).Name, this.session.UserInfo.WorkingFolder ?? "", StringComparison.OrdinalIgnoreCase) == 0)
        {
          this.folderCombo.SelectedIndex = index;
          break;
        }
      }
      if (this.folderCombo.SelectedIndex >= 0 || allLoanFolderInfos.Length == 0)
        return;
      this.folderCombo.SelectedIndex = 0;
      for (int index = 1; index < this.folderCombo.Items.Count; ++index)
      {
        if (((LoanFolderInfo) this.folderCombo.Items[index]).Type == LoanFolderInfo.LoanFolderType.Regular)
        {
          this.folderCombo.SelectedIndex = index;
          break;
        }
      }
    }

    private void initRoleCombo()
    {
      this.roleCombo.Items.Clear();
      this.cmbRole.Items.Clear();
      this.roleInfos.Clear();
      RoleInfo[] allRoleFunctions = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      this.roleCombo.Items.Add((object) "All Users");
      this.cmbRole.Items.Add((object) "<(All Roles)>");
      this.roleInfos.Add((object) "<(All Roles)>", (object) RoleInfo.All);
      foreach (RoleInfo roleInfo in allRoleFunctions)
      {
        this.roleCombo.Items.Add((object) roleInfo.RoleName);
        this.cmbRole.Items.Add((object) roleInfo.RoleName);
        this.roleInfos.Add((object) roleInfo.RoleName, (object) roleInfo);
      }
      this.roleCombo.SelectedIndex = 0;
      this.cmbRole.SelectedIndex = 0;
    }

    private void loadLoans() => this.loadLoans(this.gvLoans.Columns.GetSortOrder());

    private void loadLoans(GVColumnSort[] sortOrder)
    {
      this.gvLoans.DataProvider = (IGVDataProvider) new CursorGVDataProvider(this.session.LoanManager.OpenPipeline(LoanInfo.Right.FullRight, this.getDisplayFields(), PipelineData.Lock | PipelineData.AssignedRights | PipelineData.Milestones | PipelineData.LoanAssociates, this.getSortFields(sortOrder), this.getLoanFilter(), false), new PopulateGVItemEventHandler(this.onPopulateGVItem));
      this.gcLoans.Text = "Loans (" + (object) this.gvLoans.Items.Count + ")";
    }

    private void onPopulateGVItem(object sender, PopulateGVItemEventArgs e)
    {
      PipelineInfo dataItem = (PipelineInfo) e.DataItem;
      foreach (GVColumn column in this.gvLoans.Columns)
      {
        string name = column.Tag.ToString();
        ReportFieldDef fieldByCriterionName = (ReportFieldDef) this.fieldDefs.GetFieldByCriterionName(column.Tag.ToString());
        switch (name)
        {
          case "FullRights":
            e.ListItem.SubItems[column.Index].Text = this.getUserAccessList(dataItem.Rights, LoanInfo.Right.FullRight);
            break;
          case "RWRights":
            e.ListItem.SubItems[column.Index].Text = this.getUserAccessList(dataItem.Rights, LoanInfo.Right.Access);
            break;
          case "Loan.Active":
            e.ListItem.SubItems[column.Index].Text = string.Concat(dataItem.GetField(name)) == "Y" ? "Active" : "Inactive";
            break;
          default:
            e.ListItem.SubItems[column.Index].Text = fieldByCriterionName == null ? string.Concat(dataItem.GetField(name)) : fieldByCriterionName.ToDisplayValue(string.Concat(dataItem.GetField(name)));
            break;
        }
        e.ListItem.Tag = (object) dataItem;
      }
    }

    private string getUserAccessList(Hashtable rightsTbl, LoanInfo.Right rights)
    {
      List<string> stringList = new List<string>();
      foreach (DictionaryEntry dictionaryEntry in rightsTbl)
      {
        if (dictionaryEntry.Value.Equals((object) (int) rights))
          stringList.Add(dictionaryEntry.Key.ToString());
      }
      return string.Join(",", stringList.ToArray());
    }

    private QueryCriterion getLoanFilter()
    {
      QueryCriterion loanFilter = (QueryCriterion) new StringValueCriterion("Loan.Guid", "", StringMatchType.NotEquals);
      if (this.txtUserName.Text != "")
      {
        loanFilter = loanFilter.And((QueryCriterion) new StringValueCriterion("LoanAssociate.UserID", this.txtUserName.Tag.ToString()));
        if (this.cmbRole.SelectedIndex > 0)
          loanFilter = loanFilter.And((QueryCriterion) new OrdinalValueCriterion("LoanAssociate.RoleID", (object) ((RoleSummaryInfo) this.roleInfos[(object) this.cmbRole.Text]).RoleID));
      }
      if (this.cboCompany.SelectedItem.Equals((object) "TPO"))
      {
        QueryCriterion queryCriterion = new StringValueCriterion("Loan.TPOLOID", (string) null, StringMatchType.NotEquals).And((QueryCriterion) new StringValueCriterion("Loan.TPOLOID", "", StringMatchType.NotEquals));
        QueryCriterion criterion1 = new StringValueCriterion("Loan.TPOLPID", (string) null, StringMatchType.NotEquals).And((QueryCriterion) new StringValueCriterion("Loan.TPOLPID", "", StringMatchType.NotEquals));
        loanFilter = loanFilter.And(queryCriterion.Or(criterion1)).And(new StringValueCriterion("Loan.TPOCompanyID", (string) null, StringMatchType.NotEquals).And((QueryCriterion) new StringValueCriterion("Loan.TPOCompanyID", "", StringMatchType.NotEquals)));
        if (this.txtTPOName.Text != "" && this.txtTPOName.Text != "All")
        {
          QueryCriterion criterion2 = (QueryCriterion) new StringValueCriterion("Loan.TPOCompanyID", (string) this.txtTPOName.Tag, StringMatchType.Exact);
          loanFilter = loanFilter.And(criterion2);
        }
      }
      if (this.folderCombo.SelectedIndex > 0)
        loanFilter = loanFilter.And((QueryCriterion) new StringValueCriterion("Loan.LoanFolder", this.folderCombo.Text));
      return loanFilter;
    }

    private string[] getDisplayFields()
    {
      List<string> stringList = new List<string>();
      foreach (GVColumn column in this.gvLoans.Columns)
      {
        string str = column.Tag.ToString();
        if (str.StartsWith("Loan."))
          stringList.Add(str);
      }
      string[] strArray = new string[5]
      {
        "Loan.ActionTaken",
        "Loan.TotalLoanAmount",
        "Loan.LockRequestDate",
        "Loan.BorrowerLastName",
        "Loan.BorrowerFirstName"
      };
      foreach (string str in strArray)
      {
        if (!stringList.Contains(str))
          stringList.Add(str);
      }
      return stringList.ToArray();
    }

    private SortField[] getSortFields(GVColumnSort[] sortOrder)
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumnSort gvColumnSort in sortOrder)
      {
        GVColumn column = this.gvLoans.Columns[gvColumnSort.Column];
        sortFieldList.Add(new SortField(column.Tag.ToString(), gvColumnSort.SortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending));
      }
      return sortFieldList.ToArray();
    }

    private void gvLoans_SortItems(object source, GVColumnSortEventArgs e)
    {
      foreach (GVColumnSort columnSort in e.ColumnSorts)
      {
        if (!this.gvLoans.Columns[columnSort.Column].Tag.ToString().StartsWith("Loan."))
        {
          e.Cancel = true;
          return;
        }
      }
      this.loadLoans(e.ColumnSorts);
    }

    private void roleCombo_SelectedIndexChanged(object sender, EventArgs e) => this.loadUsers();

    private void loadUsers() => this.populateUserInfoSummary();

    private void populateUserInfoSummary()
    {
      this.gvUserList.Items.Clear();
      if (this.roleCombo.SelectedIndex > 0)
        this.users = this.session.OrganizationManager.GetScopedUsersWithRoles(new int[1]
        {
          ((RoleSummaryInfo) this.roleInfos[(object) this.roleCombo.SelectedItem.ToString()]).RoleID
        });
      else
        this.users = this.session.OrganizationManager.GetScopedUserInfos();
      if (this.users == null)
        return;
      for (int index = 0; index < this.users.Length; ++index)
        this.gvUserList.Items.Add(new GVItem(this.users[index].UserID + " (" + this.users[index].FullName + ")")
        {
          Tag = (object) this.users[index].UserID
        });
      this.gvUserList.Sort(0, SortOrder.Ascending);
    }

    private string[] getSelectedUserIds()
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < this.gvUserList.SelectedItems.Count; ++index)
        arrayList.Add((object) (string) this.gvUserList.SelectedItems[index].Tag);
      return (string[]) arrayList.ToArray(typeof (string));
    }

    private string[] getSelectedLoanIds()
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < this.gvLoans.SelectedItems.Count; ++index)
        arrayList.Add((object) ((PipelineInfo) this.gvLoans.SelectedItems[index].Tag).GUID);
      return (string[]) arrayList.ToArray(typeof (string));
    }

    private void assignButton_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select at least one loan.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.gvUserList.SelectedItems.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must select at least one user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        LoanInfo.Right rights = LoanInfo.Right.Access;
        if (this.bothRadioButton.Checked)
          rights = LoanInfo.Right.FullRight;
        try
        {
          this.session.LoanManager.AddLoanAccessRights(this.getSelectedLoanIds(), this.getSelectedUserIds(), rights);
        }
        catch (SecurityException ex)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.loadLoans();
        this.roleCombo_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    private void revokeButton_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select at least one loan.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.gvUserList.SelectedItems.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must select at least one user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        LoanInfo.Right rights = LoanInfo.Right.Assignment;
        if (this.rvkBothRadioButton.Checked)
          rights = LoanInfo.Right.FullRight;
        try
        {
          this.session.LoanManager.RemoveLoanAccessRights(this.getSelectedLoanIds(), this.getSelectedUserIds(), rights);
        }
        catch (SecurityException ex)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.loadLoans();
        this.roleCombo_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    private void showDetailsMenuItem_Click(object sender, EventArgs e)
    {
      PipelineInfo[] pipelineInfos = new PipelineInfo[this.gvLoans.SelectedItems.Count];
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select at least one loan.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        for (int index = 0; index < this.gvLoans.SelectedItems.Count; ++index)
          pipelineInfos[index] = (PipelineInfo) this.gvLoans.SelectedItems[index].Tag;
        using (LoanDetails loanDetails = new LoanDetails(pipelineInfos))
        {
          int num2 = (int) loanDetails.ShowDialog((IWin32Window) Session.MainForm);
        }
      }
    }

    private void showAllMenuItem_Click(object sender, EventArgs e)
    {
      PipelineInfo[] pipelineInfos = new PipelineInfo[this.gvLoans.Items.Count];
      if (this.gvLoans.Items.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "There are no loans to display.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        for (int nItemIndex = 0; nItemIndex < this.gvLoans.Items.Count; ++nItemIndex)
          pipelineInfos[nItemIndex] = (PipelineInfo) this.gvLoans.Items[nItemIndex].Tag;
        using (LoanDetails loanDetails = new LoanDetails(pipelineInfos))
        {
          int num2 = (int) loanDetails.ShowDialog((IWin32Window) Session.MainForm);
        }
      }
    }

    private void selectAllBtn_Click(object sender, EventArgs e)
    {
      int count = this.gvLoans.Items.Count;
      for (int nItemIndex = 0; nItemIndex < count; ++nItemIndex)
        this.gvLoans.Items[nItemIndex].Selected = true;
    }

    private void deselectAllBtn_Click(object sender, EventArgs e)
    {
      int count = this.gvLoans.Items.Count;
      for (int nItemIndex = 0; nItemIndex < count; ++nItemIndex)
        this.gvLoans.Items[nItemIndex].Selected = false;
    }

    private static int intValue(string strValue)
    {
      return Convert.ToInt32(strValue == string.Empty || strValue == null ? 0.0 : double.Parse(strValue));
    }

    private void btnSelectUser_Click(object sender, EventArgs e)
    {
      RoleInfo roleInfo = (RoleInfo) this.roleInfos[(object) this.cmbRole.SelectedItem.ToString()];
      if ("All" == roleInfo.RoleName || roleInfo.PersonaIDs != null && roleInfo.PersonaIDs.Length != 0)
      {
        ContactAssignment contactAssignment = new ContactAssignment(this.session, roleInfo, "");
        if (contactAssignment.ShowDialog() != DialogResult.OK)
          return;
        this.txtUserName.Text = contactAssignment.SelectedUser.FirstName + " " + contactAssignment.SelectedUser.LastName;
        this.txtUserName.Tag = (object) contactAssignment.SelectedUser.Userid;
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There is no persona associated with this role", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void btnGo_Click(object sender, EventArgs e)
    {
      using (CursorActivator.Wait())
        this.loadLoans();
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      using (CursorActivator.Wait())
      {
        this.txtUserName.Text = "";
        this.txtUserName.Tag = (object) null;
        this.txtTPOName.Text = "";
        this.txtTPOName.Tag = (object) null;
        this.cboCompany.SelectedIndex = 0;
        this.loadLoans();
      }
    }

    private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.txtUserName.Text = "";
      this.txtUserName.Tag = (object) null;
    }

    private void pnlAssignRevoke_SizeChanged(object sender, EventArgs e)
    {
      this.pnlAssign.Width = this.pnlAssignRevoke.Width / 2;
    }

    private void cboCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboCompany.SelectedIndex == 0)
      {
        this.btnSelectTPO.Enabled = false;
        this.txtTPOName.Enabled = false;
        this.txtTPOName.Text = string.Empty;
        this.txtTPOName.Tag = (object) null;
      }
      else
      {
        this.btnSelectTPO.Enabled = true;
        this.txtTPOName.Enabled = true;
        this.txtTPOName.Text = "All";
        this.txtTPOName.Tag = (object) "-1";
      }
    }

    private void btnSelectTPO_Click(object sender, EventArgs e)
    {
      if (this.externalOrgsList.Count > 0)
      {
        PipeLineExtOrgInfo pipeLineExtOrgInfo = new PipeLineExtOrgInfo(this.externalOrgsList);
        pipeLineExtOrgInfo.StartPosition = FormStartPosition.CenterParent;
        if (pipeLineExtOrgInfo.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          ExternalOriginatorManagementData selectedOrg = pipeLineExtOrgInfo.selectedOrg;
          if (selectedOrg != null)
          {
            this.txtTPOName.Text = selectedOrg.OrganizationName;
            this.txtTPOName.Tag = (object) selectedOrg.ExternalID;
          }
          pipeLineExtOrgInfo.Close();
        }
        else
          pipeLineExtOrgInfo.Close();
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "No External Org found!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void loadCompanyDropDown()
    {
      if (this.cboCompany.Items.Count == 0)
      {
        this.cboCompany.Items.Add((object) "Internal Organization");
        this.cboCompany.Items.Add((object) "TPO");
      }
      this.cboCompany.SelectedIndex = 0;
    }

    private void loadExternalOrgs()
    {
      bool flag1 = false;
      bool flag2 = false;
      if (Session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ExternalSettings_ContactSalesRep) && Session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ExternalSettings_OrganizationSettings))
        flag1 = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ExternalSettings_ContactSalesRep] && (bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ExternalSettings_OrganizationSettings];
      AclGroup[] groupsOfUser = Session.AclGroupManager.GetGroupsOfUser(Session.UserID);
      if (groupsOfUser != null && groupsOfUser.Length != 0)
        flag2 = ((IEnumerable<AclGroup>) groupsOfUser).Any<AclGroup>((Func<AclGroup, bool>) (group => group.Name.ToLower() == "TPO Admins".ToLower()));
      if (Session.UserInfo.IsAdministrator() | flag1 | flag2)
        this.externalOrgsList = Session.ConfigurationManager.GetAllExternalParentOrganizations(false);
      else
        this.externalOrgsList = (List<ExternalOriginatorManagementData>) Session.ConfigurationManager.GetExternalAndInternalUserAndOrgBySalesRep(Session.UserID, Session.UserInfo.OrgId)[1];
    }
  }
}
