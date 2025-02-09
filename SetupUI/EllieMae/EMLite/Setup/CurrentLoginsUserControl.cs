// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CurrentLoginsUserControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
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
  public class CurrentLoginsUserControl : UserControl
  {
    private static readonly string className = nameof (CurrentLoginsUserControl);
    private static readonly string sw = Tracing.SwOutsideLoan;
    private FeaturesAclManager aclMgr;
    private GridView loginListView;
    private IContainer components;
    private Button emailBtn;
    private Button logoutBtn;
    private IServerManager serverManager;
    private IOrganizationManager orgManager;
    private Button sendMsgBtn;
    private SessionInfo[] sessions;
    private ToolTip toolTip1;
    private GroupContainer gcCurrentLogins;
    private StandardIconButton stdIconBtnRefresh;
    private Button btnEnableLogin;
    private Button btnDisableLogin;
    private ContextMenuStrip contextMenuStripListView;
    private ToolStripMenuItem selectAllToolStripMenuItem;
    private ToolStripMenuItem emailAllToolStripMenuItem;
    private ToolStripMenuItem sendAllToolStripMenuItem;
    private ToolStripMenuItem logoutAllToolStripMenuItem;
    private Dictionary<string, UserInfo> currentList = new Dictionary<string, UserInfo>();
    private VerticalSeparator verticalSeparator1;
    private FlowLayoutPanel flowLayoutPanel1;
    private VerticalSeparator verticalSeparator2;
    private Hashtable userTypes = new Hashtable();
    private SetUpContainer setupContainer;
    private ServerSessionEventHandler serverSessionEventHandler;
    private static bool? _serverSessionEventHandlerEnabled = new bool?();

    public event EventHandler SessionTerminated;

    private static bool serverSessionEventHandlerEnabled
    {
      get
      {
        try
        {
          if (!CurrentLoginsUserControl._serverSessionEventHandlerEnabled.HasValue)
            CurrentLoginsUserControl._serverSessionEventHandlerEnabled = new bool?(((int) Session.SessionObjects.ServerManager.GetServerSetting("Internal.MultiServer") & 4) == 4);
        }
        catch (Exception ex)
        {
          Tracing.Log(CurrentLoginsUserControl.sw, CurrentLoginsUserControl.className, TraceLevel.Error, "Error getting setting MultiServer: " + ex.Message);
        }
        bool? eventHandlerEnabled = CurrentLoginsUserControl._serverSessionEventHandlerEnabled;
        bool flag = true;
        return eventHandlerEnabled.GetValueOrDefault() == flag & eventHandlerEnabled.HasValue;
      }
    }

    public CurrentLoginsUserControl()
      : this((SetUpContainer) null)
    {
    }

    public CurrentLoginsUserControl(SetUpContainer setupContainer)
    {
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.setupContainer = setupContainer;
      this.InitializeComponent();
      if (!this.DesignMode)
      {
        this.serverManager = Session.ServerManager;
        this.orgManager = Session.OrganizationManager;
        this.refresh();
      }
      this.loginListView.Sort(0, SortOrder.Ascending);
      this.loginListView_SelectedIndexChanged((object) this, (EventArgs) null);
      try
      {
        if (Session.DefaultInstance == null || !CurrentLoginsUserControl.serverSessionEventHandlerEnabled)
          return;
        this.serverSessionEventHandler = new ServerSessionEventHandler(this.handlerServerSessionEvent);
        Session.DefaultInstance.ServerSessionEvent += this.serverSessionEventHandler;
        Session.ISession.RegisterForEvents(typeof (SessionEvent));
      }
      catch (Exception ex)
      {
        Tracing.Log(CurrentLoginsUserControl.sw, CurrentLoginsUserControl.className, TraceLevel.Error, "Error registering server session event handler: " + ex.Message);
      }
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (Session.DefaultInstance != null)
        {
          if (this.serverSessionEventHandler != null)
          {
            Session.DefaultInstance.ServerSessionEvent -= this.serverSessionEventHandler;
            this.serverSessionEventHandler = (ServerSessionEventHandler) null;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CurrentLoginsUserControl.sw, CurrentLoginsUserControl.className, TraceLevel.Error, "Error unregistering server session event handler: " + ex.Message);
      }
      try
      {
        Session.ISession.UnregisterForEvents(typeof (SessionEvent));
      }
      catch (Exception ex)
      {
        Tracing.Log(CurrentLoginsUserControl.sw, CurrentLoginsUserControl.className, TraceLevel.Error, "Error unregistering session event: " + ex.Message);
      }
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
      this.loginListView = new GridView();
      this.contextMenuStripListView = new ContextMenuStrip(this.components);
      this.selectAllToolStripMenuItem = new ToolStripMenuItem();
      this.emailAllToolStripMenuItem = new ToolStripMenuItem();
      this.sendAllToolStripMenuItem = new ToolStripMenuItem();
      this.logoutAllToolStripMenuItem = new ToolStripMenuItem();
      this.emailBtn = new Button();
      this.logoutBtn = new Button();
      this.sendMsgBtn = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.stdIconBtnRefresh = new StandardIconButton();
      this.gcCurrentLogins = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.verticalSeparator2 = new VerticalSeparator();
      this.btnDisableLogin = new Button();
      this.btnEnableLogin = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.contextMenuStripListView.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnRefresh).BeginInit();
      this.gcCurrentLogins.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.loginListView.AllowColumnReorder = true;
      this.loginListView.AllowDrop = true;
      this.loginListView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "User ID";
      gvColumn1.Width = 69;
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
      gvColumn4.Text = "Login Time";
      gvColumn4.Width = 122;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Email";
      gvColumn5.Width = 124;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Phone";
      gvColumn6.Width = 74;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "Client Machine";
      gvColumn7.Width = 90;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column10";
      gvColumn8.Text = "Version";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column8";
      gvColumn9.Text = "Server";
      gvColumn9.Width = 93;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column9";
      gvColumn10.Text = "Current Loan";
      gvColumn10.Width = 113;
      this.loginListView.Columns.AddRange(new GVColumn[10]
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
        gvColumn10
      });
      this.loginListView.ContextMenuStrip = this.contextMenuStripListView;
      this.loginListView.Dock = DockStyle.Fill;
      this.loginListView.Location = new Point(1, 26);
      this.loginListView.Name = "loginListView";
      this.loginListView.Size = new Size(893, 493);
      this.loginListView.TabIndex = 0;
      this.loginListView.SelectedIndexChanged += new EventHandler(this.loginListView_SelectedIndexChanged);
      this.contextMenuStripListView.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.selectAllToolStripMenuItem,
        (ToolStripItem) this.emailAllToolStripMenuItem,
        (ToolStripItem) this.sendAllToolStripMenuItem,
        (ToolStripItem) this.logoutAllToolStripMenuItem
      });
      this.contextMenuStripListView.Name = "contextMenuStripListView";
      this.contextMenuStripListView.Size = new Size(133, 92);
      this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
      this.selectAllToolStripMenuItem.Size = new Size(132, 22);
      this.selectAllToolStripMenuItem.Text = "Select All";
      this.selectAllToolStripMenuItem.Click += new EventHandler(this.selectAllBtn_Click);
      this.emailAllToolStripMenuItem.Name = "emailAllToolStripMenuItem";
      this.emailAllToolStripMenuItem.Size = new Size(132, 22);
      this.emailAllToolStripMenuItem.Text = "Email All";
      this.emailAllToolStripMenuItem.Click += new EventHandler(this.emailAllBtn_Click);
      this.sendAllToolStripMenuItem.Name = "sendAllToolStripMenuItem";
      this.sendAllToolStripMenuItem.Size = new Size(132, 22);
      this.sendAllToolStripMenuItem.Text = "Send All";
      this.sendAllToolStripMenuItem.Click += new EventHandler(this.sendAllBtn_Click);
      this.logoutAllToolStripMenuItem.Name = "logoutAllToolStripMenuItem";
      this.logoutAllToolStripMenuItem.Size = new Size(132, 22);
      this.logoutAllToolStripMenuItem.Text = "Logout All";
      this.logoutAllToolStripMenuItem.Click += new EventHandler(this.logoutAllBtn_Click);
      this.emailBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emailBtn.BackColor = SystemColors.Control;
      this.emailBtn.Font = new Font("Arial", 8.25f);
      this.emailBtn.Location = new Point(61, 0);
      this.emailBtn.Margin = new Padding(0);
      this.emailBtn.Name = "emailBtn";
      this.emailBtn.Padding = new Padding(2, 0, 0, 0);
      this.emailBtn.Size = new Size(63, 22);
      this.emailBtn.TabIndex = 4;
      this.emailBtn.Text = "Email";
      this.emailBtn.UseVisualStyleBackColor = true;
      this.emailBtn.Click += new EventHandler(this.emailBtn_Click);
      this.logoutBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.logoutBtn.BackColor = SystemColors.Control;
      this.logoutBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.logoutBtn.Location = new Point(124, 0);
      this.logoutBtn.Margin = new Padding(0);
      this.logoutBtn.Name = "logoutBtn";
      this.logoutBtn.Padding = new Padding(2, 0, 0, 0);
      this.logoutBtn.Size = new Size(64, 22);
      this.logoutBtn.TabIndex = 6;
      this.logoutBtn.Text = "Logout";
      this.logoutBtn.UseVisualStyleBackColor = true;
      this.logoutBtn.Click += new EventHandler(this.logoutBtn_Click);
      this.sendMsgBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.sendMsgBtn.BackColor = SystemColors.Control;
      this.sendMsgBtn.Font = new Font("Arial", 8.25f);
      this.sendMsgBtn.Location = new Point(196, 0);
      this.sendMsgBtn.Margin = new Padding(0);
      this.sendMsgBtn.Name = "sendMsgBtn";
      this.sendMsgBtn.Padding = new Padding(2, 0, 0, 0);
      this.sendMsgBtn.Size = new Size(115, 22);
      this.sendMsgBtn.TabIndex = 8;
      this.sendMsgBtn.Text = "Broadcast Messa&ge";
      this.sendMsgBtn.UseVisualStyleBackColor = true;
      this.sendMsgBtn.Click += new EventHandler(this.sendMsgBtn_Click);
      this.stdIconBtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnRefresh.BackColor = Color.Transparent;
      this.stdIconBtnRefresh.Location = new Point(34, 3);
      this.stdIconBtnRefresh.Name = "stdIconBtnRefresh";
      this.stdIconBtnRefresh.Size = new Size(16, 16);
      this.stdIconBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.stdIconBtnRefresh.TabIndex = 1;
      this.stdIconBtnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnRefresh, "Refresh");
      this.stdIconBtnRefresh.Click += new EventHandler(this.refreshBtn_Click);
      this.gcCurrentLogins.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcCurrentLogins.Controls.Add((Control) this.loginListView);
      this.gcCurrentLogins.Dock = DockStyle.Fill;
      this.gcCurrentLogins.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gcCurrentLogins.HeaderForeColor = SystemColors.ControlText;
      this.gcCurrentLogins.Location = new Point(0, 0);
      this.gcCurrentLogins.Name = "gcCurrentLogins";
      this.gcCurrentLogins.Size = new Size(895, 520);
      this.gcCurrentLogins.TabIndex = 11;
      this.gcCurrentLogins.Text = "Currently Logged in Users";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDisableLogin);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnEnableLogin);
      this.flowLayoutPanel1.Controls.Add((Control) this.sendMsgBtn);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator2);
      this.flowLayoutPanel1.Controls.Add((Control) this.logoutBtn);
      this.flowLayoutPanel1.Controls.Add((Control) this.emailBtn);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnRefresh);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(402, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Padding = new Padding(0, 0, 5, 0);
      this.flowLayoutPanel1.Size = new Size(492, 22);
      this.flowLayoutPanel1.TabIndex = 15;
      this.verticalSeparator2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator2.Location = new Point(191, 3);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 15;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.btnDisableLogin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDisableLogin.BackColor = SystemColors.Control;
      this.btnDisableLogin.Font = new Font("Arial", 8.25f);
      this.btnDisableLogin.Location = new Point(397, 0);
      this.btnDisableLogin.Margin = new Padding(0);
      this.btnDisableLogin.Name = "btnDisableLogin";
      this.btnDisableLogin.Padding = new Padding(2, 0, 0, 0);
      this.btnDisableLogin.Size = new Size(90, 22);
      this.btnDisableLogin.TabIndex = 13;
      this.btnDisableLogin.Text = "Disable Logins";
      this.btnDisableLogin.UseVisualStyleBackColor = true;
      this.btnDisableLogin.Click += new EventHandler(this.btnEnableDisableLogin_Click);
      this.btnEnableLogin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEnableLogin.BackColor = SystemColors.Control;
      this.btnEnableLogin.Font = new Font("Arial", 8.25f);
      this.btnEnableLogin.Location = new Point(311, 0);
      this.btnEnableLogin.Margin = new Padding(0);
      this.btnEnableLogin.Name = "btnEnableLogin";
      this.btnEnableLogin.Padding = new Padding(2, 0, 0, 0);
      this.btnEnableLogin.Size = new Size(86, 22);
      this.btnEnableLogin.TabIndex = 12;
      this.btnEnableLogin.Text = "Enable Logins";
      this.btnEnableLogin.UseVisualStyleBackColor = true;
      this.btnEnableLogin.Click += new EventHandler(this.btnEnableDisableLogin_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(56, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 14;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.Controls.Add((Control) this.gcCurrentLogins);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (CurrentLoginsUserControl);
      this.Size = new Size(895, 520);
      this.contextMenuStripListView.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnRefresh).EndInit();
      this.gcCurrentLogins.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void refresh()
    {
      try
      {
        if (Session.UserInfo.IsSuperAdministrator() || this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_DisableLogins))
        {
          if (this.serverManager.LoginsEnabled)
          {
            this.btnDisableLogin.Enabled = true;
            this.btnEnableLogin.Enabled = false;
          }
          else
          {
            this.btnEnableLogin.Enabled = true;
            this.btnDisableLogin.Enabled = false;
          }
        }
        else
        {
          this.btnEnableLogin.Enabled = false;
          this.btnDisableLogin.Enabled = false;
          this.logoutAllToolStripMenuItem.Enabled = false;
        }
        this.sessions = !Session.Connection.IsServerInProcess ? this.serverManager.GetAllSessionInfo(true) : this.serverManager.GetAllSessionInfo(false);
      }
      catch (Exception ex)
      {
        this.OnSessionTerminated();
        return;
      }
      Cursor.Current = Cursors.WaitCursor;
      this.loginListView.Items.Clear();
      this.loginListView.BeginUpdate();
      Dictionary<string, UserLoginInfo> dictionary = (Dictionary<string, UserLoginInfo>) null;
      this.userTypes.Clear();
      bool isTPOMVP = Session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      try
      {
        string[] userIds = new string[this.sessions.Length];
        for (int index = 0; index < this.sessions.Length; ++index)
          userIds[index] = this.sessions[index].UserID;
        if (userIds.Length != 0)
          dictionary = this.orgManager.GetUserLoginInfos(userIds, isTPOMVP);
      }
      catch (Exception ex)
      {
        this.loadUsers();
        dictionary = (Dictionary<string, UserLoginInfo>) null;
      }
      if (this.sessions != null)
      {
        for (int index = 0; index < this.sessions.Length; ++index)
        {
          if (dictionary != null && dictionary.ContainsKey(this.sessions[index].UserID))
          {
            UserLoginInfo userLoginInfo = dictionary[this.sessions[index].UserID];
            if (userLoginInfo.UserType.ToLower() != "external" || userLoginInfo.UserType.ToLower() == "external" & isTPOMVP)
            {
              string str = "";
              if (this.sessions[index].CurrentLoanInfo != null)
                str = this.sessions[index].CurrentLoanInfo.BorrowerFirstName + " " + this.sessions[index].CurrentLoanInfo.BorrowerLastName + " (" + this.sessions[index].CurrentLoanInfo.LoanNumber + ")";
              this.loginListView.Items.Add(new GVItem(this.sessions[index].UserID)
              {
                SubItems = {
                  (object) userLoginInfo.LastName,
                  (object) userLoginInfo.FirstName,
                  (object) this.sessions[index].LoginTime.ToString(),
                  (object) userLoginInfo.Email,
                  (object) userLoginInfo.Phone,
                  (object) (this.sessions[index].Hostname + (this.sessions[index].HostAddress != null ? " (" + this.sessions[index].HostAddress.ToString() + ")" : "")),
                  (object) this.sessions[index].DisplayVersion,
                  this.sessions[index].Server == null ? (object) "(local)" : (object) this.sessions[index].Server.ToString(),
                  (object) str
                },
                Tag = (object) this.sessions[index]
              });
              if (!this.userTypes.ContainsKey((object) userLoginInfo.UserID))
                this.userTypes.Add((object) userLoginInfo.UserID, (object) userLoginInfo.UserType);
            }
          }
        }
      }
      this.loginListView.EndUpdate();
      this.gcCurrentLogins.Text = "Currently Logged in Users (" + (object) this.loginListView.Items.Count + ")";
      this.loginListView_SelectedIndexChanged((object) this, (EventArgs) null);
      Cursor.Current = Cursors.Default;
    }

    private void loadUsers()
    {
      this.currentList.Clear();
      foreach (RoleSummaryInfo allRoleFunction in ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions())
      {
        foreach (UserInfo userInfo in Session.OrganizationManager.GetScopedUsersWithRole(allRoleFunction.RoleID))
        {
          if (!this.currentList.ContainsKey(userInfo.Userid))
            this.currentList.Add(userInfo.Userid, userInfo);
        }
      }
    }

    private void loginListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.logoutBtn.Enabled = this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_LogUsersOut) && this.loginListView.SelectedItems.Count > 0;
      this.sendMsgBtn.Enabled = this.loginListView.SelectedItems.Count > 0;
      this.emailBtn.Enabled = true;
      foreach (GVItem selectedItem in this.loginListView.SelectedItems)
      {
        if (this.userTypes != null && this.userTypes.Count > 0 && this.userTypes[(object) selectedItem.SubItems[0].Text].ToString().ToLower() == "external")
        {
          this.emailBtn.Enabled = false;
          this.logoutBtn.Enabled = false;
          this.sendMsgBtn.Enabled = false;
        }
      }
    }

    private void refreshBtn_Click(object sender, EventArgs e)
    {
      this.refresh();
      SaveConfirmScreen.Show(this.setupContainer == null ? (IWin32Window) this : (IWin32Window) this.setupContainer, "Data refreshed.");
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
      string str1 = "";
      for (int index = 0; index < count; ++index)
      {
        string str2 = !emailAll ? this.loginListView.SelectedItems[index].SubItems[4].Text : this.loginListView.Items[index].SubItems[4].Text;
        if (str2 != null && str2 != string.Empty)
        {
          if (str1 != string.Empty)
            str1 += ";";
          str1 += str2;
        }
      }
      SystemUtil.ShellExecute("mailto:" + str1);
    }

    private void emailBtn_Click(object sender, EventArgs e) => this.emailUsers(false);

    private void emailAllBtn_Click(object sender, EventArgs e) => this.emailUsers(true);

    private string[] getSelectedSessionIDs()
    {
      int count = this.loginListView.SelectedItems.Count;
      if (count == 0)
        return (string[]) null;
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < count; ++index)
        arrayList.Add((object) ((SessionInfo) this.loginListView.SelectedItems[index].Tag).SessionID);
      return (string[]) arrayList.ToArray(typeof (string));
    }

    private void logoutAllBtn_Click(object sender, EventArgs e)
    {
      bool forceDisconnect = false;
      using (TerminateSessionDialog terminateSessionDialog = new TerminateSessionDialog())
      {
        if (terminateSessionDialog.ShowDialog(Form.ActiveForm, "All users will be logged out of Encompass, including yourself. Are you sure you want to continue?") != DialogResult.Yes)
          return;
        forceDisconnect = terminateSessionDialog.ForceLogout;
      }
      this.serverManager.TerminateAllSessions(forceDisconnect, true);
      this.refresh();
    }

    private void logoutBtn_Click(object sender, EventArgs e)
    {
      string[] selectedSessionIds = this.getSelectedSessionIDs();
      if (selectedSessionIds == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a user.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        bool forceDisconnect = false;
        using (TerminateSessionDialog terminateSessionDialog = new TerminateSessionDialog())
        {
          if (terminateSessionDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.Yes)
            return;
          forceDisconnect = terminateSessionDialog.ForceLogout;
        }
        if (Session.UserInfo.IsSuperAdministrator() || this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_LogUsersOut))
        {
          this.serverManager.TerminateSessions(selectedSessionIds, forceDisconnect, true);
        }
        else
        {
          int orgId = Session.UserInfo.OrgId;
          bool flag1 = false;
          for (int index = 0; index < this.loginListView.SelectedItems.Count; ++index)
          {
            if (selectedSessionIds[index] == Session.SessionObjects.SessionID)
            {
              flag1 = true;
            }
            else
            {
              GVItem selectedItem = this.loginListView.SelectedItems[index];
              string text = selectedItem.SubItems[0].Text;
              bool flag2 = false;
              UserInfo user = Session.OrganizationManager.GetUser(text);
              if (user != (UserInfo) null)
              {
                OrgInfo organization = Session.OrganizationManager.GetOrganization(user.OrgId);
                flag2 = true;
                for (; organization != null; organization = Session.OrganizationManager.GetOrganization(organization.Parent))
                {
                  if (organization.Oid == orgId)
                  {
                    flag2 = false;
                    break;
                  }
                  if (organization.Oid == organization.Parent)
                    break;
                }
              }
              if (flag2)
              {
                int num2 = (int) Utils.Dialog((IWin32Window) this, "User " + text + " (" + selectedItem.SubItems[2].Text + " " + selectedItem.SubItems[1].Text + ") is not in or under your organization. You cannot log that person out.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              }
              else
                this.serverManager.TerminateSession(selectedSessionIds[index], forceDisconnect, true);
            }
          }
          if (flag1)
            this.serverManager.TerminateSession(Session.SessionObjects.SessionID, forceDisconnect, false);
        }
        this.refresh();
      }
    }

    private void sendMsg(bool allUsers)
    {
      string[] sessionIds = (string[]) null;
      if (!allUsers)
      {
        sessionIds = this.getSelectedSessionIDs();
        if (sessionIds == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must select at least one user.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      BroadcastMessageDialog broadcastMessageDialog = new BroadcastMessageDialog();
      if (broadcastMessageDialog.ShowDialog() != DialogResult.OK)
        return;
      if (allUsers)
        this.serverManager.BroadcastMessage(new EllieMae.EMLite.ClientServer.Message(broadcastMessageDialog.Message), true);
      else
        this.serverManager.SendMessage(new EllieMae.EMLite.ClientServer.Message(broadcastMessageDialog.Message), sessionIds, true);
    }

    private void sendMsgBtn_Click(object sender, EventArgs e) => this.sendMsg(false);

    private void sendAllBtn_Click(object sender, EventArgs e)
    {
      this.refresh();
      if (Session.UserInfo.IsSuperAdministrator())
      {
        this.sendMsg(true);
      }
      else
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.loginListView.Items)
          gvItem.Selected = true;
        this.sendMsg(false);
      }
    }

    protected void OnSessionTerminated()
    {
      if (this.SessionTerminated == null)
        return;
      this.SessionTerminated((object) this, new EventArgs());
    }

    private void btnEnableDisableLogin_Click(object sender, EventArgs e)
    {
      bool flag = true;
      if (sender == this.btnDisableLogin)
      {
        if (Utils.Dialog((IWin32Window) this, "Disabling logins will prevent all users from logging in except the 'admin' user. Users who are currently logged into Encompass will not be affected." + Environment.NewLine + Environment.NewLine + "Are you sure you want to disable logins?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        flag = false;
      }
      if (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_DisableLogins))
        this.serverManager.LoginsEnabledWithForPersonna = flag;
      else
        this.serverManager.LoginsEnabled = flag;
      this.refresh();
    }

    private void handlerServerSessionEvent(object sender, ServerSessionEventArgs args)
    {
      if (args.SessionEvent.EventType != SessionEventType.Login && args.SessionEvent.EventType != SessionEventType.Logout && args.SessionEvent.EventType != SessionEventType.Terminated)
        return;
      this.refresh();
    }
  }
}
