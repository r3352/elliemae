// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CenterwiseLOSelectionDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite
{
  public class CenterwiseLOSelectionDialog : Form
  {
    private Dictionary<int, OrgInfo> orgLookup = new Dictionary<int, OrgInfo>();
    private UserInfo[] selectedUsers;
    private ListViewSortManager sortMngr;
    private IContainer components;
    private Label label1;
    private Panel panel1;
    private ListView lvwUsers;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private Label label2;
    private Button btnOK;
    private Button btnCancel;

    public CenterwiseLOSelectionDialog(string[] allowedUserIds)
    {
      this.InitializeComponent();
      this.loadOrgList();
      this.loadUserList(allowedUserIds);
      this.sortMngr = new ListViewSortManager(this.lvwUsers, new System.Type[4]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.sortMngr.Sort(0);
    }

    private void loadOrgList()
    {
      foreach (OrgInfo allOrganization in Session.SessionObjects.OrganizationManager.GetAllOrganizations())
        this.orgLookup[allOrganization.Oid] = allOrganization;
    }

    public UserInfo[] SelectedUsers => this.selectedUsers;

    private void loadUserList(string[] allowedUserIds)
    {
      foreach (UserInfo filteredLo in CenterwiseLOSelectionDialog.GetFilteredLOList(allowedUserIds))
      {
        ListViewItem listViewItem = new ListViewItem(filteredLo.Userid);
        listViewItem.SubItems.Add(filteredLo.LastName);
        listViewItem.SubItems.Add(filteredLo.FirstName);
        if (this.orgLookup.ContainsKey(filteredLo.OrgId))
          listViewItem.SubItems.Add(this.orgLookup[filteredLo.OrgId].OrgName);
        else
          listViewItem.SubItems.Add("");
        listViewItem.Tag = (object) filteredLo;
        listViewItem.Checked = true;
        this.lvwUsers.Items.Add(listViewItem);
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.selectedUsers = new UserInfo[this.lvwUsers.CheckedItems.Count];
      int num = 0;
      foreach (ListViewItem checkedItem in this.lvwUsers.CheckedItems)
        this.selectedUsers[num++] = (UserInfo) checkedItem.Tag;
      this.DialogResult = DialogResult.OK;
    }

    public static UserInfo[] GetFilteredLOList(string[] allowedUserIds)
    {
      Dictionary<string, string> stringLookupTable = CenterwiseLOSelectionDialog.createStringLookupTable(allowedUserIds);
      RolesMappingInfo officerRoleMapping = CenterwiseLOSelectionDialog.getLoanOfficerRoleMapping();
      if (officerRoleMapping == null || officerRoleMapping.RoleIDList.Length == 0)
        return new UserInfo[0];
      UserInfo[] usersWithRole = Session.SessionObjects.OrganizationManager.GetUsersWithRole(officerRoleMapping.RoleIDList[0]);
      List<UserInfo> userInfoList = new List<UserInfo>();
      foreach (UserInfo userInfo in usersWithRole)
      {
        if (stringLookupTable == null || stringLookupTable.ContainsKey(userInfo.Userid))
          userInfoList.Add(userInfo);
      }
      return userInfoList.ToArray();
    }

    private static RolesMappingInfo getLoanOfficerRoleMapping()
    {
      foreach (RolesMappingInfo roleMapping in Session.StartupInfo.RoleMappings)
      {
        if (roleMapping.RealWorldRoleID == RealWorldRoleID.LoanOfficer)
          return roleMapping;
      }
      return (RolesMappingInfo) null;
    }

    private static Dictionary<string, string> createStringLookupTable(string[] items)
    {
      if (items == null)
        return (Dictionary<string, string>) null;
      Dictionary<string, string> stringLookupTable = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (string key in items)
        stringLookupTable[key] = key;
      return stringLookupTable;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.panel1 = new Panel();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lvwUsers = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.label2 = new Label();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(0, 32);
      this.label1.Name = "label1";
      this.label1.Size = new Size(322, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Enable/Disable Loan Officer WebCenter for the following user(s):";
      this.panel1.Controls.Add((Control) this.btnOK);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.lvwUsers);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(10, 10);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(633, 437);
      this.panel1.TabIndex = 1;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(473, 414);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(556, 414);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lvwUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwUsers.CheckBoxes = true;
      this.lvwUsers.Columns.AddRange(new ColumnHeader[4]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4
      });
      this.lvwUsers.Location = new Point(0, 48);
      this.lvwUsers.MultiSelect = false;
      this.lvwUsers.Name = "lvwUsers";
      this.lvwUsers.Size = new Size(632, 354);
      this.lvwUsers.TabIndex = 2;
      this.lvwUsers.UseCompatibleStateImageBehavior = false;
      this.lvwUsers.View = View.Details;
      this.columnHeader1.Text = "User ID";
      this.columnHeader1.Width = 143;
      this.columnHeader2.Text = "Last Name";
      this.columnHeader2.Width = 140;
      this.columnHeader3.Text = "First Name";
      this.columnHeader3.Width = 140;
      this.columnHeader4.Text = "Organization";
      this.columnHeader4.Width = 180;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(0, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(142, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Loan Officer Web Center";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(653, 457);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CenterwiseLOSelectionDialog);
      this.Padding = new Padding(10);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Activation Setup Wizard";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
