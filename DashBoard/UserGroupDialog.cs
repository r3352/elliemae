// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.UserGroupDialog
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class UserGroupDialog : Form
  {
    private Sessions.Session session;
    private ListViewSortManager sortManager;
    private IContainer components;
    private ListView lvwUserGroups;
    private Button btnCancel;
    private Button btnOK;
    private ColumnHeader chdUserGroupName;

    public AclGroup SelectedUserGroup
    {
      get => this.getSelectedUserGroup();
      set => this.setSelectedUserGroup(value);
    }

    public UserGroupDialog()
      : this(Session.DefaultInstance)
    {
    }

    public UserGroupDialog(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.initializeDialog();
    }

    private void initializeDialog()
    {
      AclGroup[] aclGroupArray = !this.session.UserInfo.IsAdministrator() ? this.session.AclGroupManager.GetGroupsOfUser(this.session.UserID) : this.session.AclGroupManager.GetAllGroups();
      if (aclGroupArray != null && aclGroupArray.Length != 0)
      {
        foreach (AclGroup aclGroup in aclGroupArray)
          this.lvwUserGroups.Items.Add(new ListViewItem(aclGroup.Name)
          {
            Tag = (object) aclGroup
          });
      }
      this.sortManager = new ListViewSortManager(this.lvwUserGroups, new System.Type[1]
      {
        typeof (ListViewTextCaseInsensitiveSort)
      }, true);
      this.sortManager.Sort(0);
    }

    private AclGroup getSelectedUserGroup()
    {
      AclGroup selectedUserGroup = (AclGroup) null;
      if (0 < this.lvwUserGroups.SelectedItems.Count && this.lvwUserGroups.SelectedItems[0].Tag != null)
        selectedUserGroup = (AclGroup) this.lvwUserGroups.SelectedItems[0].Tag;
      return selectedUserGroup;
    }

    private void setSelectedUserGroup(AclGroup selectedUserGroup)
    {
      this.lvwUserGroups.SelectedItems.Clear();
      foreach (ListViewItem listViewItem in this.lvwUserGroups.Items)
      {
        if (listViewItem.Tag != null && ((AclGroup) listViewItem.Tag).ID == selectedUserGroup.ID)
        {
          listViewItem.Selected = true;
          break;
        }
      }
    }

    private void lvwUserGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = 0 < this.lvwUserGroups.SelectedItems.Count;
    }

    private void lvwUserGroups_Resize(object sender, EventArgs e)
    {
      this.chdUserGroupName.Width = this.lvwUserGroups.Width - 25;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = 0 < this.lvwUserGroups.SelectedItems.Count ? DialogResult.OK : DialogResult.Cancel;
      this.Close();
    }

    private void lvwUserGroups_DoubleClick(object sender, EventArgs e)
    {
      if (1 != this.lvwUserGroups.SelectedItems.Count)
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lvwUserGroups = new ListView();
      this.chdUserGroupName = new ColumnHeader();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.lvwUserGroups.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwUserGroups.Columns.AddRange(new ColumnHeader[1]
      {
        this.chdUserGroupName
      });
      this.lvwUserGroups.Location = new Point(12, 12);
      this.lvwUserGroups.MultiSelect = false;
      this.lvwUserGroups.Name = "lvwUserGroups";
      this.lvwUserGroups.Size = new Size(268, 400);
      this.lvwUserGroups.TabIndex = 0;
      this.lvwUserGroups.UseCompatibleStateImageBehavior = false;
      this.lvwUserGroups.View = View.Details;
      this.lvwUserGroups.Resize += new EventHandler(this.lvwUserGroups_Resize);
      this.lvwUserGroups.SelectedIndexChanged += new EventHandler(this.lvwUserGroups_SelectedIndexChanged);
      this.lvwUserGroups.DoubleClick += new EventHandler(this.lvwUserGroups_DoubleClick);
      this.chdUserGroupName.Text = "User Group Name";
      this.chdUserGroupName.Width = 243;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(205, 418);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(123, 418);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(292, 453);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lvwUserGroups);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (UserGroupDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select a User Group";
      this.ResumeLayout(false);
    }
  }
}
