// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AllTPOUserPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AllTPOUserPanel : UserControl
  {
    private const string className = "AllTPOUserPanel";
    private static string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private SetUpContainer setupContainer;
    private GridView loginListView;
    private IContainer components;
    private Button emailBtn;
    private Label label2;
    private IOrganizationManager rOrg = Session.OrganizationManager;
    private Button btnFindInHierarchy;
    private ComboBox extOrgCombo;
    private GroupContainer gcAllUsers;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnRefresh;
    private Label lblCount;
    private ContextMenuStrip contextMenuStripListView;
    private ToolStripMenuItem selectAllToolStripMenuItem;
    private ToolStripMenuItem emailAllToolStripMenuItem;
    private VerticalSeparator verticalSeparator1;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton stdIconBtnExport;
    private Dictionary<string, List<ExternalSettingValue>> settingsList;
    private List<ExternalOriginatorManagementData> orgs;
    private StandardIconButton popUpButton;
    private Hashtable orgLookup;
    private Hashtable allOrgLookup;
    private List<string> externalUsersIds;
    private List<ExternalOriginatorManagementData> externalOrgsList;
    private bool hasContactEditRight;

    public AllTPOUserPanel(Sessions.Session session, SetUpContainer setupContainer)
    {
      this.setupContainer = setupContainer;
      this.session = session;
      this.InitializeComponent();
      this.loadExternalOrgs();
      this.loginListView.Sort(0, SortOrder.Ascending);
      this.loginListView_SelectedIndexChanged((object) this, (EventArgs) null);
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
      this.contextMenuStripListView = new ContextMenuStrip(this.components);
      this.selectAllToolStripMenuItem = new ToolStripMenuItem();
      this.emailAllToolStripMenuItem = new ToolStripMenuItem();
      this.toolTip1 = new ToolTip(this.components);
      this.stdIconBtnExport = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnRefresh = new StandardIconButton();
      this.gcAllUsers = new GroupContainer();
      this.popUpButton = new StandardIconButton();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.emailBtn = new Button();
      this.btnFindInHierarchy = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.lblCount = new Label();
      this.extOrgCombo = new ComboBox();
      this.label2 = new Label();
      this.loginListView = new GridView();
      this.contextMenuStripListView.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnExport).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnRefresh).BeginInit();
      this.gcAllUsers.SuspendLayout();
      ((ISupportInitialize) this.popUpButton).BeginInit();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.contextMenuStripListView.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.selectAllToolStripMenuItem,
        (ToolStripItem) this.emailAllToolStripMenuItem
      });
      this.contextMenuStripListView.Name = "contextMenuStripListView";
      this.contextMenuStripListView.Size = new Size(123, 48);
      this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
      this.selectAllToolStripMenuItem.Size = new Size(122, 22);
      this.selectAllToolStripMenuItem.Text = "Select All";
      this.selectAllToolStripMenuItem.Click += new EventHandler(this.selectAllBtn_Click);
      this.emailAllToolStripMenuItem.Name = "emailAllToolStripMenuItem";
      this.emailAllToolStripMenuItem.Size = new Size(122, 22);
      this.emailAllToolStripMenuItem.Text = "Email All";
      this.emailAllToolStripMenuItem.Click += new EventHandler(this.emailAllBtn_Click);
      this.stdIconBtnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnExport.BackColor = Color.Transparent;
      this.stdIconBtnExport.Location = new Point(59, 3);
      this.stdIconBtnExport.Margin = new Padding(3, 3, 2, 3);
      this.stdIconBtnExport.MouseDownImage = (Image) null;
      this.stdIconBtnExport.Name = "stdIconBtnExport";
      this.stdIconBtnExport.Size = new Size(16, 16);
      this.stdIconBtnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.stdIconBtnExport.TabIndex = 23;
      this.stdIconBtnExport.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnExport, "Export");
      this.stdIconBtnExport.Click += new EventHandler(this.stdIconBtnExport_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(38, 3);
      this.stdIconBtnEdit.Margin = new Padding(3, 3, 2, 3);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 2;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit User");
      this.stdIconBtnEdit.Click += new EventHandler(this.editUserBtn_Click);
      this.stdIconBtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnRefresh.BackColor = Color.Transparent;
      this.stdIconBtnRefresh.Location = new Point(17, 3);
      this.stdIconBtnRefresh.Margin = new Padding(3, 3, 2, 3);
      this.stdIconBtnRefresh.MouseDownImage = (Image) null;
      this.stdIconBtnRefresh.Name = "stdIconBtnRefresh";
      this.stdIconBtnRefresh.Size = new Size(16, 16);
      this.stdIconBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.stdIconBtnRefresh.TabIndex = 1;
      this.stdIconBtnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnRefresh, "Refresh");
      this.stdIconBtnRefresh.Click += new EventHandler(this.refreshBtn_Click);
      this.gcAllUsers.Controls.Add((Control) this.popUpButton);
      this.gcAllUsers.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcAllUsers.Controls.Add((Control) this.lblCount);
      this.gcAllUsers.Controls.Add((Control) this.extOrgCombo);
      this.gcAllUsers.Controls.Add((Control) this.label2);
      this.gcAllUsers.Controls.Add((Control) this.loginListView);
      this.gcAllUsers.Dock = DockStyle.Fill;
      this.gcAllUsers.HeaderForeColor = SystemColors.ControlText;
      this.gcAllUsers.Location = new Point(0, 0);
      this.gcAllUsers.Name = "gcAllUsers";
      this.gcAllUsers.Size = new Size(855, 357);
      this.gcAllUsers.TabIndex = 21;
      this.popUpButton.BackColor = Color.Transparent;
      this.popUpButton.Location = new Point(259, 5);
      this.popUpButton.MouseDownImage = (Image) null;
      this.popUpButton.Name = "popUpButton";
      this.popUpButton.Size = new Size(16, 16);
      this.popUpButton.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.popUpButton.TabIndex = 24;
      this.popUpButton.TabStop = false;
      this.popUpButton.Click += new EventHandler(this.popUpButton_Click);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.emailBtn);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnFindInHierarchy);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnExport);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnEdit);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnRefresh);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(590, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Padding = new Padding(0, 0, 5, 0);
      this.flowLayoutPanel1.Size = new Size(265, 22);
      this.flowLayoutPanel1.TabIndex = 23;
      this.emailBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emailBtn.BackColor = SystemColors.Control;
      this.emailBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emailBtn.Location = new Point(197, 0);
      this.emailBtn.Margin = new Padding(0);
      this.emailBtn.Name = "emailBtn";
      this.emailBtn.Padding = new Padding(2, 0, 0, 0);
      this.emailBtn.Size = new Size(63, 22);
      this.emailBtn.TabIndex = 4;
      this.emailBtn.Text = "Email";
      this.emailBtn.UseVisualStyleBackColor = true;
      this.emailBtn.Click += new EventHandler(this.emailBtn_Click);
      this.btnFindInHierarchy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFindInHierarchy.BackColor = SystemColors.Control;
      this.btnFindInHierarchy.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnFindInHierarchy.Location = new Point(85, 0);
      this.btnFindInHierarchy.Margin = new Padding(0);
      this.btnFindInHierarchy.Name = "btnFindInHierarchy";
      this.btnFindInHierarchy.Padding = new Padding(2, 0, 0, 0);
      this.btnFindInHierarchy.Size = new Size(112, 22);
      this.btnFindInHierarchy.TabIndex = 20;
      this.btnFindInHierarchy.Text = "Find in Hierarchy";
      this.btnFindInHierarchy.UseVisualStyleBackColor = true;
      this.btnFindInHierarchy.Click += new EventHandler(this.btnFindInHierarchy_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(80, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 22;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.lblCount.BackColor = Color.Transparent;
      this.lblCount.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCount.Location = new Point(278, 4);
      this.lblCount.Name = "lblCount";
      this.lblCount.Size = new Size(83, 16);
      this.lblCount.TabIndex = 21;
      this.lblCount.Text = "(#)";
      this.lblCount.TextAlign = ContentAlignment.MiddleLeft;
      this.extOrgCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.extOrgCombo.Location = new Point(91, 2);
      this.extOrgCombo.Name = "extOrgCombo";
      this.extOrgCombo.Size = new Size(162, 22);
      this.extOrgCombo.TabIndex = 11;
      this.extOrgCombo.SelectedIndexChanged += new EventHandler(this.extOrgCombo_SelectedIndexChanged);
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(8, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size(83, 16);
      this.label2.TabIndex = 13;
      this.label2.Text = "Select Persona";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.loginListView.AllowColumnReorder = true;
      this.loginListView.AllowDrop = true;
      this.loginListView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnLastName";
      gvColumn1.Text = "Last Name";
      gvColumn1.Width = 94;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnFirstName";
      gvColumn2.Text = "First Name";
      gvColumn2.Width = 84;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnRoles";
      gvColumn3.Text = "Roles";
      gvColumn3.Width = 71;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnTPOOrg";
      gvColumn4.Text = "TPO Organization";
      gvColumn4.Width = 123;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnTitle";
      gvColumn5.Text = "Title";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnState";
      gvColumn6.Text = "State";
      gvColumn6.Width = 66;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnEmail";
      gvColumn7.Tag = (object) "Email";
      gvColumn7.Text = "Email";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnPhone";
      gvColumn8.Text = "Phone";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "ColumnStatus";
      gvColumn9.Text = "Status";
      gvColumn9.Width = 100;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "ColumnLoginID";
      gvColumn10.Text = "Login ID";
      gvColumn10.Width = 68;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "ColumnLastLogin";
      gvColumn11.Tag = (object) "";
      gvColumn11.Text = "Last Login";
      gvColumn11.Width = 71;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "ColumnLogin";
      gvColumn12.Text = "Login";
      gvColumn12.Width = 82;
      this.loginListView.Columns.AddRange(new GVColumn[12]
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
        gvColumn12
      });
      this.loginListView.ContextMenuStrip = this.contextMenuStripListView;
      this.loginListView.Dock = DockStyle.Fill;
      this.loginListView.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.loginListView.Location = new Point(1, 26);
      this.loginListView.Name = "loginListView";
      this.loginListView.Size = new Size(853, 330);
      this.loginListView.TabIndex = 0;
      this.loginListView.SelectedIndexChanged += new EventHandler(this.loginListView_SelectedIndexChanged);
      this.loginListView.ItemDoubleClick += new GVItemEventHandler(this.loginListView_ItemDoubleClick);
      this.Controls.Add((Control) this.gcAllUsers);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (AllTPOUserPanel);
      this.Size = new Size(855, 357);
      this.contextMenuStripListView.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnExport).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnRefresh).EndInit();
      this.gcAllUsers.ResumeLayout(false);
      ((ISupportInitialize) this.popUpButton).EndInit();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void refreshBtn_Click(object sender, EventArgs e)
    {
      this.extOrgCombo_SelectedIndexChanged((object) null, (EventArgs) null);
      SaveConfirmScreen.Show((IWin32Window) this.setupContainer, "Data refreshed.");
    }

    private void selectAllBtn_Click(object sender, EventArgs e)
    {
      int count = this.loginListView.Items.Count;
      for (int nItemIndex = 0; nItemIndex < count; ++nItemIndex)
        this.loginListView.Items[nItemIndex].Selected = true;
    }

    private void emailUsers(bool emailAll)
    {
      int count = this.loginListView.SelectedItems.Count;
      if (emailAll)
        count = this.loginListView.Items.Count;
      int nItemIndex = -1;
      foreach (GVColumn column in this.loginListView.Columns)
      {
        if (string.Concat(column.Tag) == "Email")
        {
          nItemIndex = column.Index;
          break;
        }
      }
      if (nItemIndex == -1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The column containing the Email addresses could not be located.");
      }
      else
      {
        string str1 = "";
        for (int index = 0; index < count; ++index)
        {
          string str2 = !emailAll ? this.loginListView.SelectedItems[index].SubItems[nItemIndex].Text : this.loginListView.Items[index].SubItems[nItemIndex].Text;
          if (str2 != null && str2 != string.Empty)
          {
            if (str1 != string.Empty)
              str1 += ";";
            str1 += str2;
          }
        }
        SystemUtil.ShellExecute("mailto:" + str1);
      }
    }

    private void emailBtn_Click(object sender, EventArgs e)
    {
      if (this.loginListView.SelectedItems.Count == 0)
        return;
      this.emailUsers(false);
    }

    private void emailAllBtn_Click(object sender, EventArgs e)
    {
      if (this.loginListView.SelectedItems.Count == 0)
        return;
      this.emailUsers(true);
    }

    public new void Refresh()
    {
      Hashtable hashtable = new Hashtable();
      this.settingsList = this.session.ConfigurationManager.GetExternalOrgSettings();
      if (this.settingsList != null && this.settingsList.Count > 0 && this.settingsList.ContainsKey("Current Contact Status") && this.settingsList["Current Contact Status"] != null)
      {
        foreach (ExternalSettingValue externalSettingValue in this.settingsList["Current Contact Status"])
          hashtable.Add((object) externalSettingValue.settingId, (object) externalSettingValue.settingValue);
      }
      this.loginListView.Items.Clear();
      ExternalUserInfo[] externalUserInfos = this.session.ConfigurationManager.GetAllExternalUserInfos(((ExternalOriginatorManagementData) this.extOrgCombo.SelectedItem).oid);
      this.loginListView.BeginUpdate();
      if (externalUserInfos != null && externalUserInfos.Length != 0)
      {
        for (int index = 0; index < externalUserInfos.Length; ++index)
        {
          if (this.externalUsersIds == null || this.externalUsersIds.Contains(externalUserInfos[index].ContactID))
          {
            GVItem gvItem = new GVItem(externalUserInfos[index].LastName);
            gvItem.Tag = (object) externalUserInfos[index];
            gvItem.SubItems.Add((object) externalUserInfos[index].FirstName);
            gvItem.SubItems.Add((object) TPOUtils.returnRoles(externalUserInfos[index].Roles));
            gvItem.SubItems.Add(this.allOrgLookup.ContainsKey((object) externalUserInfos[index].ExternalOrgID) ? (object) this.allOrgLookup[(object) externalUserInfos[index].ExternalOrgID].ToString() : (object) "");
            gvItem.SubItems.Add((object) externalUserInfos[index].Title);
            gvItem.SubItems.Add((object) externalUserInfos[index].State);
            gvItem.SubItems.Add((object) externalUserInfos[index].Email);
            gvItem.SubItems.Add((object) externalUserInfos[index].Phone);
            if (hashtable.Count > 0 && hashtable.ContainsKey((object) externalUserInfos[index].ApprovalCurrentStatus))
              gvItem.SubItems.Add((object) hashtable[(object) externalUserInfos[index].ApprovalCurrentStatus].ToString());
            else
              gvItem.SubItems.Add((object) "");
            gvItem.SubItems.Add((object) externalUserInfos[index].EmailForLogin);
            gvItem.SubItems.Add(externalUserInfos[index].LastLogin == DateTime.MinValue ? (object) "" : (object) externalUserInfos[index].LastLogin.ToString("M/d/yyyy h:mm:ss tt"));
            gvItem.SubItems.Add(externalUserInfos[index].DisabledLogin ? (object) "Disabled" : (object) "Enabled");
            this.loginListView.Items.Add(gvItem);
          }
        }
        this.stdIconBtnExport.Enabled = true;
      }
      else
        this.stdIconBtnExport.Enabled = false;
      this.loginListView.EndUpdate();
      this.lblCount.Text = "(" + (object) this.loginListView.Items.Count + ")";
    }

    private void editUserBtn_Click(object sender, EventArgs e)
    {
      if (this.loginListView.SelectedItems.Count == 0)
        return;
      ExternalUserInfo externalUserInfo = this.session.ConfigurationManager.GetExternalUserInfo(((ExternalUserInfo) this.loginListView.SelectedItems[0].Tag).ExternalUserID);
      using (TPOContactSetupForm contactSetupForm = new TPOContactSetupForm(externalUserInfo.ExternalOrgID, this.session, externalUserInfo))
      {
        if (contactSetupForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.Refresh();
      }
    }

    private void loginListView_DoubleClick(object sender, EventArgs e)
    {
      this.editUserBtn_Click(sender, e);
    }

    private void btnFindInHierarchy_Click(object sender, EventArgs e)
    {
      if (this.loginListView.SelectedItems == null || this.loginListView.SelectedItems.Count != 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select only one user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        Session.MainScreen.SwitchToExternalOrgUserSetup(((ExternalUserInfo) this.loginListView.SelectedItems[0].Tag).ExternalUserID);
    }

    private void loadExternalOrgs()
    {
      List<ExternalOriginatorManagementData> externalOrganizations = Session.ConfigurationManager.GetAllExternalOrganizations(false);
      this.allOrgLookup = new Hashtable(externalOrganizations.Count);
      for (int index = 0; index < externalOrganizations.Count; ++index)
        this.allOrgLookup.Add((object) externalOrganizations[index].oid, (object) externalOrganizations[index].OrganizationName);
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.hasContactEditRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditContacts);
      if (Session.UserInfo.IsAdministrator() || !aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_ContactSalesRep) && aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_OrganizationSettings))
      {
        this.externalOrgsList = Session.ConfigurationManager.GetAllExternalParentOrganizations(false);
      }
      else
      {
        ArrayList andOrgBySalesRep = Session.ConfigurationManager.GetExternalUserAndOrgBySalesRep(Session.UserID);
        this.externalOrgsList = (List<ExternalOriginatorManagementData>) andOrgBySalesRep[1];
        this.externalUsersIds = (List<string>) andOrgBySalesRep[4];
      }
      this.orgs = this.externalOrgsList;
      this.orgLookup = new Hashtable(this.orgs.Count);
      for (int index = 0; index < this.orgs.Count; ++index)
        this.orgLookup.Add((object) this.orgs[index].oid, (object) this.orgs[index].OrganizationName);
      List<ExternalOriginatorManagementData> originatorManagementDataList = new List<ExternalOriginatorManagementData>();
      originatorManagementDataList.Add(new ExternalOriginatorManagementData()
      {
        oid = -1,
        OrganizationName = "All TPOs"
      });
      originatorManagementDataList.AddRange((IEnumerable<ExternalOriginatorManagementData>) this.orgs.FindAll((Predicate<ExternalOriginatorManagementData>) (r => r.OrganizationType == ExternalOriginatorOrgType.Company)));
      this.extOrgCombo.Items.Clear();
      this.extOrgCombo.DataSource = (object) originatorManagementDataList;
      this.extOrgCombo.DisplayMember = "OrganizationName";
      this.extOrgCombo.ValueMember = "oid";
      this.extOrgCombo.SelectedIndex = 0;
    }

    private void loginListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEdit.Enabled = this.hasContactEditRight && this.loginListView.SelectedItems.Count == 1;
      this.btnFindInHierarchy.Enabled = this.loginListView.SelectedItems.Count == 1;
      this.emailBtn.Enabled = this.loginListView.SelectedItems.Count >= 1;
      this.contextMenuStripListView.Items[1].Enabled = this.loginListView.SelectedItems.Count >= 1;
      this.contextMenuStripListView.Items[0].Enabled = this.loginListView.SelectedItems.Count < this.loginListView.Items.Count;
    }

    private void loginListView_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editUserBtn_Click(source, (EventArgs) e);
    }

    private void stdIconBtnExport_Click(object sender, EventArgs e)
    {
      try
      {
        ExcelHandler excelHandler = new ExcelHandler();
        excelHandler.AddDataTable(this.loginListView, false);
        excelHandler.CreateExcel();
      }
      catch (Exception ex)
      {
        Tracing.Log(AllTPOUserPanel.sw, nameof (AllTPOUserPanel), TraceLevel.Error, "Error during export: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export the users information to Microsoft Excel. Ensure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void extOrgCombo_SelectedIndexChanged(object sender, EventArgs e) => this.Refresh();

    private void popUpButton_Click(object sender, EventArgs e)
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
            if (selectedOrg.oid == 0 || selectedOrg.OrganizationName.ToUpper() == "ALL")
              this.extOrgCombo.SelectedValue = (object) -1;
            else
              this.extOrgCombo.SelectedValue = (object) selectedOrg.oid;
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
  }
}
