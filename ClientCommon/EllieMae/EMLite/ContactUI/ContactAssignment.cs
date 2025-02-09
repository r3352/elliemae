// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactAssignment
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactAssignment : Form
  {
    private ListViewSortManager sortMgr;
    private System.ComponentModel.Container components;
    private ListView listView1;
    private ColumnHeader colHdrUserid;
    private ColumnHeader colHdrLastName;
    private ColumnHeader colHdrFirstName;
    private CheckBox cxbShowAll;
    private string _UserToExclude = string.Empty;
    private RoleInfo[] roles;
    private Button btnOK;
    private Button btnCancel;
    private AclFeature[] features;
    private Sessions.Session session;
    private string assigneeID = string.Empty;
    private UserInfo selectedUser;

    public ContactAssignment(Sessions.Session session, RoleInfo[] roles, string userToExclude)
    {
      this.session = session;
      this.InitializeComponent();
      this._UserToExclude = userToExclude;
      this.roles = roles;
      this.Initial(roles);
    }

    public ContactAssignment(
      Sessions.Session session,
      RoleInfo[] roles,
      string userToExclude,
      bool canShowAll)
    {
      this.session = session;
      this.InitializeComponent();
      this._UserToExclude = userToExclude;
      this.Initial(roles);
      this.roles = roles;
      if (!canShowAll)
        return;
      this.cxbShowAll.Enabled = true;
    }

    public ContactAssignment(Sessions.Session session, RoleInfo role, string userToExclude)
      : this(session, new RoleInfo[1]{ role }, userToExclude)
    {
    }

    public ContactAssignment(
      Sessions.Session session,
      RoleInfo role,
      string userToExclude,
      bool canShowAll)
      : this(session, new RoleInfo[1]{ role }, userToExclude, (canShowAll ? 1 : 0) != 0)
    {
    }

    public ContactAssignment(Sessions.Session session, AclFeature[] features, string userToExclude)
    {
      this.session = session;
      this.InitializeComponent();
      this._UserToExclude = userToExclude;
      this.features = features;
      this.Init(features);
    }

    public ContactAssignment(Sessions.Session session, AclFeature feature, string userToExclude)
      : this(session, new AclFeature[1]{ feature }, userToExclude)
    {
    }

    public ContactAssignment(Sessions.Session session, string userToExclude)
    {
      this.session = session;
      this.InitializeComponent();
      this._UserToExclude = userToExclude;
      this.Init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.listView1 = new ListView();
      this.colHdrUserid = new ColumnHeader();
      this.colHdrLastName = new ColumnHeader();
      this.colHdrFirstName = new ColumnHeader();
      this.cxbShowAll = new CheckBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listView1.Columns.AddRange(new ColumnHeader[3]
      {
        this.colHdrUserid,
        this.colHdrLastName,
        this.colHdrFirstName
      });
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.Location = new Point(0, 0);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.Size = new Size(323, 435);
      this.listView1.TabIndex = 4;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.listView1.DoubleClick += new EventHandler(this.listView1_DoubleClick);
      this.colHdrUserid.Text = "User ID";
      this.colHdrUserid.Width = 74;
      this.colHdrLastName.Text = "Last Name";
      this.colHdrLastName.Width = 125;
      this.colHdrFirstName.Text = "First Name";
      this.colHdrFirstName.Width = 107;
      this.cxbShowAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cxbShowAll.AutoSize = true;
      this.cxbShowAll.Enabled = false;
      this.cxbShowAll.Location = new Point(2, 442);
      this.cxbShowAll.Name = "cxbShowAll";
      this.cxbShowAll.Size = new Size(94, 17);
      this.cxbShowAll.TabIndex = 5;
      this.cxbShowAll.Text = "Show all users";
      this.cxbShowAll.UseVisualStyleBackColor = true;
      this.cxbShowAll.CheckedChanged += new EventHandler(this.cxbShowAll_CheckedChanged);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(154, 465);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(235, 465);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(321, 493);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.cxbShowAll);
      this.Controls.Add((Control) this.listView1);
      this.MinimizeBox = false;
      this.Name = nameof (ContactAssignment);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select a User";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void Init(RoleInfo[] roleInfos)
    {
      this.listView1.Items.Clear();
      this.sortMgr = new ListViewSortManager(this.listView1, new System.Type[3]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.sortMgr.Sort(0);
      if (roleInfos == null || roleInfos.Length == 0)
        return;
      List<UserInfoSummary> userInfoSummaryList = new List<UserInfoSummary>();
      foreach (RoleInfo roleInfo in roleInfos)
      {
        UserInfoSummary[] userInfoSummaryArray = (UserInfoSummary[]) null;
        if ("All" == roleInfo.RoleName)
          userInfoSummaryArray = this.session.OrganizationManager.GetScopedUserInfos();
        else if (roleInfo.PersonaIDs != null && roleInfo.PersonaIDs.Length != 0)
          userInfoSummaryArray = this.session.OrganizationManager.GetScopedUsersWithRoles(new int[1]
          {
            roleInfo.ID
          });
        if (userInfoSummaryArray != null)
        {
          foreach (UserInfoSummary userInfoSummary in userInfoSummaryArray)
          {
            if (!userInfoSummaryList.Contains(userInfoSummary))
              userInfoSummaryList.Add(userInfoSummary);
          }
        }
      }
      this.LoadPage(userInfoSummaryList.ToArray());
    }

    private void Initial(RoleInfo[] roleInfos)
    {
      try
      {
        this.listView1.Items.Clear();
        this.sortMgr = new ListViewSortManager(this.listView1, new System.Type[3]
        {
          typeof (ListViewTextCaseInsensitiveSort),
          typeof (ListViewTextCaseInsensitiveSort),
          typeof (ListViewTextCaseInsensitiveSort)
        });
        this.sortMgr.Sort(0);
        if (roleInfos == null || roleInfos.Length == 0)
          return;
        List<int> intList = new List<int>();
        bool flag = false;
        foreach (RoleInfo roleInfo in roleInfos)
        {
          if ("All" == roleInfo.RoleName)
            flag = true;
          intList.Add(roleInfo.RoleID);
        }
        this.LoadPage(!flag ? this.session.OrganizationManager.GetScopedUsersWithRoles(intList.ToArray()) : this.session.OrganizationManager.GetScopedUserInfos());
      }
      catch (Exception ex)
      {
        this.Init(roleInfos);
      }
    }

    private void Init()
    {
      this.listView1.Items.Clear();
      this.sortMgr = new ListViewSortManager(this.listView1, new System.Type[3]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.sortMgr.Sort(0);
      List<UserInfoSummary> userInfoSummaryList = new List<UserInfoSummary>();
      foreach (UserInfoSummary userInfoSummary in !this.session.UserInfo.IsSuperAdministrator() ? this.session.OrganizationManager.GetScopedUserInfos() : this.session.OrganizationManager.GetAllUserInfoSummary())
      {
        if (!userInfoSummaryList.Contains(userInfoSummary))
          userInfoSummaryList.Add(userInfoSummary);
      }
      if (userInfoSummaryList.Count <= 0)
        return;
      this.LoadPage(userInfoSummaryList.ToArray());
    }

    private void Init(AclFeature[] features)
    {
      if (features.Length == 1 && features[0] == AclFeature.Cnt_Borrower_Reassign)
      {
        this.InitForContactReassignment();
      }
      else
      {
        this.listView1.Items.Clear();
        this.sortMgr = new ListViewSortManager(this.listView1, new System.Type[3]
        {
          typeof (ListViewTextCaseInsensitiveSort),
          typeof (ListViewTextCaseInsensitiveSort),
          typeof (ListViewTextCaseInsensitiveSort)
        });
        this.sortMgr.Sort(0);
        if (features == null || features.Length == 0)
          return;
        List<UserInfoSummary> userInfoSummaryList = new List<UserInfoSummary>();
        FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
        foreach (string s in aclManager.GetPersonaListByFeature(features, AclTriState.True))
        {
          foreach (UserInfoSummary userInfoSummary in this.session.OrganizationManager.GetAccessibleUserInfoSummariesWithPersona(int.Parse(s), false))
          {
            if (!userInfoSummaryList.Contains(userInfoSummary))
              userInfoSummaryList.Add(userInfoSummary);
          }
        }
        Hashtable users = this.session.OrganizationManager.GetUsers(aclManager.GetUserListByFeature(features, AclTriState.True), true);
        foreach (string key in (IEnumerable) users.Keys)
        {
          UserInfoSummary userInfoSummary = (UserInfoSummary) users[(object) key];
          if (!userInfoSummaryList.Contains(userInfoSummary))
            userInfoSummaryList.Add(userInfoSummary);
        }
        if (userInfoSummaryList.Count <= 0)
          return;
        this.LoadPage(userInfoSummaryList.ToArray());
      }
    }

    private void InitForContactReassignment()
    {
      this.listView1.Items.Clear();
      this.sortMgr = new ListViewSortManager(this.listView1, new System.Type[3]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.sortMgr.Sort(0);
      List<UserInfoSummary> userInfoSummaryList = new List<UserInfoSummary>();
      WorkflowManager bpmManager = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
      RolesMappingInfo roleMappingInfo = bpmManager.GetRoleMappingInfo(RealWorldRoleID.LoanOfficer);
      if (roleMappingInfo != null && roleMappingInfo.RoleIDList.Length != 0)
      {
        RoleInfo roleFunction = bpmManager.GetRoleFunction(roleMappingInfo.RoleIDList[0]);
        if (roleFunction != null && roleFunction.PersonaIDs != null && roleFunction.PersonaIDs.Length != 0)
        {
          UserInfoSummary[] scopedUsersWithRoles = this.session.OrganizationManager.GetScopedUsersWithRoles(new int[1]
          {
            roleFunction.ID
          });
          if (scopedUsersWithRoles != null)
          {
            foreach (UserInfoSummary userInfoSummary in scopedUsersWithRoles)
            {
              if (!userInfoSummaryList.Contains(userInfoSummary))
                userInfoSummaryList.Add(userInfoSummary);
            }
          }
        }
      }
      UserInfoSummary[] summariesWithPersona1 = this.session.OrganizationManager.GetUserInfoSummariesWithPersona(0, false);
      if (summariesWithPersona1 != null)
      {
        foreach (UserInfoSummary userInfoSummary in summariesWithPersona1)
        {
          if (!userInfoSummaryList.Contains(userInfoSummary))
            userInfoSummaryList.Add(userInfoSummary);
        }
      }
      UserInfoSummary[] summariesWithPersona2 = this.session.OrganizationManager.GetUserInfoSummariesWithPersona(1, false);
      if (summariesWithPersona2 != null)
      {
        foreach (UserInfoSummary userInfoSummary in summariesWithPersona2)
        {
          if (!userInfoSummaryList.Contains(userInfoSummary))
            userInfoSummaryList.Add(userInfoSummary);
        }
      }
      UserInfo user = this.session.OrganizationManager.GetUser("admin");
      if (!userInfoSummaryList.Contains(new UserInfoSummary(user)))
        userInfoSummaryList.Add(new UserInfoSummary(user));
      if (userInfoSummaryList.Count <= 0)
        return;
      this.LoadPage(userInfoSummaryList.ToArray());
    }

    private void LoadPage(UserInfoSummary[] userList)
    {
      this.sortMgr.Disable();
      this.listView1.BeginUpdate();
      for (int index = 0; index < userList.Length; ++index)
      {
        if (this._UserToExclude == string.Empty || userList[index].UserID != this._UserToExclude)
          this.listView1.Items.Add(new ListViewItem(userList[index].UserID)
          {
            Tag = (object) userList[index].UserID,
            SubItems = {
              userList[index].LastName,
              userList[index].FirstName
            }
          });
      }
      this.sortMgr.Enable();
      this.listView1.EndUpdate();
    }

    public string AssigneeID
    {
      get => this.assigneeID;
      set => this.assigneeID = value;
    }

    public UserInfo SelectedUser => this.selectedUser;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.listView1.Items.Count == 0)
        this.DialogResult = DialogResult.Cancel;
      else if (this.listView1.SelectedItems == null || this.listView1.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a user in the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.selectedUser = this.session.OrganizationManager.GetUser((string) this.listView1.SelectedItems[0].Tag);
        this.assigneeID = this.selectedUser.Userid;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
      this.btnOK_Click((object) this, (EventArgs) null);
    }

    public bool SetAssignee(string UserID)
    {
      if (this.assigneeID == UserID)
        return true;
      bool flag = false;
      for (int index = 0; index < this.listView1.Items.Count; ++index)
      {
        if (this.listView1.Items[index].Text == UserID)
        {
          this.listView1.Items[index].Selected = true;
          flag = true;
          break;
        }
      }
      if (flag)
      {
        this.selectedUser = this.session.OrganizationManager.GetUser(UserID);
        this.assigneeID = this.selectedUser.Userid;
      }
      return flag;
    }

    private void cxbShowAll_CheckedChanged(object sender, EventArgs e)
    {
      if (this.cxbShowAll.Checked)
        this.Init();
      else if (this.roles != null)
        this.Init(this.roles);
      else
        this.Init(this.features);
    }
  }
}
