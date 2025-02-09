// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AllUserPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AllUserPanel : UserControl
  {
    private const string className = "AllUserPanel";
    private static string sw = Tracing.SwOutsideLoan;
    private SetUpContainer setupContainer;
    private GridView loginListView;
    private IContainer components;
    private Button emailBtn;
    private Label label2;
    private IOrganizationManager rOrg = Session.OrganizationManager;
    private Button btnFindInHierarchy;
    private ComboBox personaCombo;
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
    private Hashtable personaInfos;

    public AllUserPanel(SetUpContainer setupContainer)
    {
      this.setupContainer = setupContainer;
      this.InitializeComponent();
      this.personaInfos = new Hashtable();
      this.loadPersonaComboDDL();
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
      this.contextMenuStripListView = new ContextMenuStrip(this.components);
      this.selectAllToolStripMenuItem = new ToolStripMenuItem();
      this.emailAllToolStripMenuItem = new ToolStripMenuItem();
      this.toolTip1 = new ToolTip(this.components);
      this.stdIconBtnExport = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnRefresh = new StandardIconButton();
      this.gcAllUsers = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.emailBtn = new Button();
      this.btnFindInHierarchy = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.lblCount = new Label();
      this.personaCombo = new ComboBox();
      this.label2 = new Label();
      this.loginListView = new GridView();
      this.contextMenuStripListView.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnExport).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnRefresh).BeginInit();
      this.gcAllUsers.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.contextMenuStripListView.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.selectAllToolStripMenuItem,
        (ToolStripItem) this.emailAllToolStripMenuItem
      });
      this.contextMenuStripListView.Name = "contextMenuStripListView";
      this.contextMenuStripListView.Size = new Size(129, 48);
      this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
      this.selectAllToolStripMenuItem.Size = new Size(128, 22);
      this.selectAllToolStripMenuItem.Text = "Select All";
      this.selectAllToolStripMenuItem.Click += new EventHandler(this.selectAllBtn_Click);
      this.emailAllToolStripMenuItem.Name = "emailAllToolStripMenuItem";
      this.emailAllToolStripMenuItem.Size = new Size(128, 22);
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
      this.gcAllUsers.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcAllUsers.Controls.Add((Control) this.lblCount);
      this.gcAllUsers.Controls.Add((Control) this.personaCombo);
      this.gcAllUsers.Controls.Add((Control) this.label2);
      this.gcAllUsers.Controls.Add((Control) this.loginListView);
      this.gcAllUsers.Dock = DockStyle.Fill;
      this.gcAllUsers.HeaderForeColor = SystemColors.ControlText;
      this.gcAllUsers.Location = new Point(0, 0);
      this.gcAllUsers.Name = "gcAllUsers";
      this.gcAllUsers.Size = new Size(855, 357);
      this.gcAllUsers.TabIndex = 21;
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
      this.lblCount.Location = new Point(257, 4);
      this.lblCount.Name = "lblCount";
      this.lblCount.Size = new Size(83, 16);
      this.lblCount.TabIndex = 21;
      this.lblCount.Text = "(#)";
      this.lblCount.TextAlign = ContentAlignment.MiddleLeft;
      this.personaCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.personaCombo.Location = new Point(91, 2);
      this.personaCombo.Name = "personaCombo";
      this.personaCombo.Size = new Size(162, 22);
      this.personaCombo.TabIndex = 11;
      this.personaCombo.SelectedIndexChanged += new EventHandler(this.personaCombo_SelectedIndexChanged);
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
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "User ID";
      gvColumn1.Width = 68;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 94;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 84;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Persona";
      gvColumn4.Width = 71;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Organization group";
      gvColumn5.Width = 123;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.SortMethod = GVSortMethod.DateTime;
      gvColumn6.Text = "Last Login";
      gvColumn6.Width = 111;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column11";
      gvColumn7.Text = "Version";
      gvColumn7.Width = 70;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column7";
      gvColumn8.Text = "Login";
      gvColumn8.Width = 69;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column8";
      gvColumn9.Text = "Account";
      gvColumn9.Width = 66;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column9";
      gvColumn10.Tag = (object) "Email";
      gvColumn10.Text = "Email";
      gvColumn10.Width = 71;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column10";
      gvColumn11.Text = "Phone";
      gvColumn11.Width = 82;
      this.loginListView.Columns.AddRange(new GVColumn[11]
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
        gvColumn11
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
      this.Name = nameof (AllUserPanel);
      this.Size = new Size(855, 357);
      this.contextMenuStripListView.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnExport).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnRefresh).EndInit();
      this.gcAllUsers.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void refreshBtn_Click(object sender, EventArgs e)
    {
      this.personaCombo_SelectedIndexChanged((object) null, (EventArgs) null);
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

    private void emailBtn_Click(object sender, EventArgs e) => this.emailUsers(false);

    private void emailAllBtn_Click(object sender, EventArgs e) => this.emailUsers(true);

    private void refresh()
    {
      OrgInfo[] allOrganizations = this.rOrg.GetAllOrganizations();
      Hashtable hashtable = new Hashtable(allOrganizations.Length);
      for (int index = 0; index < allOrganizations.Length; ++index)
        hashtable.Add((object) allOrganizations[index].Oid, (object) allOrganizations[index].OrgName);
      this.loginListView.Items.Clear();
      UserInfo[] userInfoArray = this.personaCombo.SelectedIndex <= 0 ? Session.OrganizationManager.GetAllAccessibleUsers() : (!(this.personaCombo.SelectedItem.ToString() != "<All Users>") ? Session.OrganizationManager.GetAllAccessibleUsers() : Session.OrganizationManager.GetAccessibleUsersWithPersona(((Persona) this.personaInfos[(object) this.personaCombo.SelectedItem.ToString()]).ID, false));
      this.loginListView.BeginUpdate();
      if (userInfoArray != null && userInfoArray.Length != 0)
      {
        for (int nIndex = 0; nIndex < userInfoArray.Length; ++nIndex)
        {
          GVItem gvItem = new GVItem(userInfoArray[nIndex].Userid);
          gvItem.Tag = (object) userInfoArray[nIndex];
          gvItem.SubItems.Add((object) userInfoArray[nIndex].LastName);
          gvItem.SubItems.Add((object) userInfoArray[nIndex].FirstName);
          string str = "";
          if (userInfoArray[nIndex].UserPersonas != null)
          {
            for (int index = 0; index < userInfoArray[nIndex].UserPersonas.Length; ++index)
              str = index != 0 ? str + " + " + userInfoArray[nIndex].UserPersonas[index].Name : userInfoArray[nIndex].UserPersonas[index].Name;
          }
          gvItem.SubItems.Add((object) str);
          gvItem.SubItems.Add(hashtable.ContainsKey((object) userInfoArray[nIndex].OrgId) ? (object) hashtable[(object) userInfoArray[nIndex].OrgId].ToString() : (object) "");
          gvItem.SubItems.Add(userInfoArray[nIndex].LastLogin == DateTime.MinValue ? (object) "" : (object) userInfoArray[nIndex].LastLogin.ToString("M/d/yyyy h:mm:ss tt"));
          gvItem.SubItems.Add((object) userInfoArray[nIndex].EncompassVersion);
          gvItem.SubItems.Add(userInfoArray[nIndex].Locked ? (object) "Disabled" : (object) "Enabled");
          gvItem.SubItems.Add(userInfoArray[nIndex].Status == UserInfo.UserStatusEnum.Enabled ? (object) "Enabled" : (object) "Disabled");
          gvItem.SubItems.Add((object) userInfoArray[nIndex].Email);
          gvItem.SubItems.Add((object) userInfoArray[nIndex].Phone);
          this.loginListView.Items.Insert(nIndex, gvItem);
        }
      }
      this.loginListView.EndUpdate();
      this.lblCount.Text = "(" + (object) this.loginListView.Items.Count + ")";
    }

    private void editUserBtn_Click(object sender, EventArgs e) => this.editUser();

    private void editUser()
    {
      if (this.loginListView.SelectedItems != null && this.loginListView.SelectedItems.Count > 1)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You can only select one user to edit at a time.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.loginListView.SelectedItems == null || this.loginListView.SelectedItems.Count == 0 || this.loginListView.SelectedItems[0] == null)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must select a user.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string text1 = this.loginListView.SelectedItems[0].Text;
        int orgId = Session.UserInfo.OrgId;
        OrgInfo organization = Session.OrganizationManager.GetOrganization(Session.OrganizationManager.GetUser(text1).OrgId);
        bool bReadOnly = true;
        for (; organization != null; organization = Session.OrganizationManager.GetOrganization(organization.Parent))
        {
          if (organization.Oid == orgId)
          {
            bReadOnly = false;
            break;
          }
          if (organization.Oid == organization.Parent)
            break;
        }
        UserInfo tag = (UserInfo) this.loginListView.SelectedItems[0].Tag;
        UserInfo userInfoFromDialog = SetupUtil.GetNewUserInfoFromDialog(Session.DefaultInstance, (IWin32Window) this, true, text1, tag.OrgId, bReadOnly);
        if (userInfoFromDialog == (UserInfo) null)
          return;
        userInfoFromDialog.LastLogin = tag.LastLogin;
        userInfoFromDialog.EncompassVersion = tag.EncompassVersion;
        string text2 = this.loginListView.SelectedItems[0].SubItems[4].Text;
        GVItem selectedItem = this.loginListView.SelectedItems[0];
        selectedItem.Tag = (object) userInfoFromDialog;
        selectedItem.SubItems.Clear();
        selectedItem.Text = userInfoFromDialog.Userid;
        selectedItem.SubItems.Add((object) userInfoFromDialog.LastName);
        selectedItem.SubItems.Add((object) userInfoFromDialog.FirstName);
        selectedItem.SubItems.Add((object) Persona.ToString(userInfoFromDialog.UserPersonas));
        selectedItem.SubItems.Add((object) text2);
        selectedItem.SubItems.Add(userInfoFromDialog.LastLogin == DateTime.MinValue ? (object) "" : (object) userInfoFromDialog.LastLogin.ToString("M/d/yyyy h:mm:ss tt"));
        selectedItem.SubItems.Add((object) userInfoFromDialog.EncompassVersion);
        selectedItem.SubItems.Add(userInfoFromDialog.Locked ? (object) "Disabled" : (object) "Enabled");
        selectedItem.SubItems.Add(userInfoFromDialog.Status == UserInfo.UserStatusEnum.Enabled ? (object) "Enabled" : (object) "Disabled");
        selectedItem.SubItems.Add((object) userInfoFromDialog.Email);
        selectedItem.SubItems.Add((object) userInfoFromDialog.Phone);
      }
    }

    private void loginListView_DoubleClick(object sender, EventArgs e)
    {
      if (!UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas))
        return;
      this.editUser();
    }

    private void btnFindInHierarchy_Click(object sender, EventArgs e)
    {
      if (this.loginListView.SelectedItems == null || this.loginListView.SelectedItems.Count != 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select only one user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        Session.MainScreen.SwitchToOrgUserSetup(this.loginListView.SelectedItems[0].Text);
    }

    private void loadPersonaComboDDL()
    {
      this.personaCombo.Items.Add((object) "<All Users>");
      Persona[] allPersonas = Session.PersonaManager.GetAllPersonas();
      foreach (Persona persona in allPersonas != null ? (IEnumerable<Persona>) ((IEnumerable<Persona>) allPersonas).OrderBy<Persona, string>((Func<Persona, string>) (item => item.Name)) : (IEnumerable<Persona>) null)
      {
        this.personaCombo.Items.Add((object) persona.Name);
        this.personaInfos.Add((object) persona.Name, (object) persona);
      }
      this.personaCombo.SelectedIndex = 0;
    }

    private void personaCombo_SelectedIndexChanged(object sender, EventArgs e) => this.refresh();

    private void loginListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEdit.Enabled = this.btnFindInHierarchy.Enabled = this.loginListView.SelectedItems.Count == 1;
    }

    private void loginListView_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.loginListView_DoubleClick((object) this, (EventArgs) null);
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
        Tracing.Log(AllUserPanel.sw, nameof (AllUserPanel), TraceLevel.Error, "Error during export: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export the users information to Microsoft Excel. Ensure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }
  }
}
