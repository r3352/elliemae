// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.DeployUserSelectionDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class DeployUserSelectionDialog : Form
  {
    private ListViewSortManager lvwSortManager;
    private IContainer components;
    private ListView lvwUsers;
    private ColumnHeader chdrUserId;
    private ColumnHeader chdrLastName;
    private ColumnHeader chdrFirstName;
    private Button btnCancel;
    private Button btnOk;

    public List<string> SelectedUserIds
    {
      get
      {
        List<string> selectedUserIds = new List<string>();
        foreach (ListViewItem selectedItem in this.lvwUsers.SelectedItems)
          selectedUserIds.Add(selectedItem.Text);
        return selectedUserIds;
      }
    }

    public DeployUserSelectionDialog()
    {
      this.InitializeComponent();
      this.lvwSortManager = new ListViewSortManager(this.lvwUsers, new System.Type[3]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.populateUserList();
      this.lvwSortManager.Sort(0);
    }

    private void populateUserList()
    {
      this.lvwUsers.BeginUpdate();
      this.lvwSortManager.Disable();
      this.lvwUsers.Items.Clear();
      this.lvwUsers.Items.AddRange(this.getUserList());
      if (this.lvwUsers.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There are currently no Users to select from.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.lvwSortManager.Enable();
      this.lvwUsers.EndUpdate();
      this.btnOk.Enabled = false;
    }

    private ListViewItem[] getUserList()
    {
      UserInfo[] withCampaignAccess = this.getScopedUsersWithCampaignAccess();
      List<ListViewItem> listViewItemList = new List<ListViewItem>();
      foreach (UserInfo userInfo in withCampaignAccess)
        listViewItemList.Add(new ListViewItem(userInfo.Userid)
        {
          SubItems = {
            userInfo.LastName,
            userInfo.FirstName
          }
        });
      return listViewItemList.ToArray();
    }

    private UserInfo[] getScopedUsersWithCampaignAccess()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) aclManager.GetPersonaListByFeature(new AclFeature[1]
      {
        AclFeature.Cnt_Campaign_Access
      }, AclTriState.True));
      RoleInfo[] allRoleFunctions = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      Dictionary<string, UserInfo> dictionary1 = new Dictionary<string, UserInfo>();
      Dictionary<string, UserInfo> dictionary2 = new Dictionary<string, UserInfo>();
      foreach (RoleInfo roleInfo in allRoleFunctions)
      {
        bool flag = false;
        foreach (int personaId in roleInfo.PersonaIDs)
        {
          if (stringList.Contains(personaId.ToString()))
          {
            flag = true;
            break;
          }
        }
        foreach (UserInfo userInfo in Session.OrganizationManager.GetScopedUsersWithRole(roleInfo.RoleID))
        {
          if (flag && !dictionary1.ContainsKey(userInfo.Userid))
            dictionary1.Add(userInfo.Userid, userInfo);
          else if (!dictionary2.ContainsKey(userInfo.Userid))
            dictionary2.Add(userInfo.Userid, userInfo);
        }
      }
      FeaturesAclManager featuresAclManager1 = aclManager;
      AclFeature[] features1 = new AclFeature[1]
      {
        AclFeature.Cnt_Campaign_Access
      };
      foreach (string str in featuresAclManager1.GetUserListByFeature(features1, AclTriState.True))
      {
        if (dictionary2.ContainsKey(str) && !dictionary1.ContainsKey(str))
        {
          UserInfo user = Session.OrganizationManager.GetUser(str);
          dictionary1.Add(str, user);
        }
      }
      FeaturesAclManager featuresAclManager2 = aclManager;
      AclFeature[] features2 = new AclFeature[1]
      {
        AclFeature.Cnt_Campaign_Access
      };
      foreach (string key in featuresAclManager2.GetUserListByFeature(features2, AclTriState.False))
      {
        if (dictionary1.ContainsKey(key))
          dictionary1.Remove(key);
      }
      UserInfo[] array = new UserInfo[dictionary1.Count];
      dictionary1.Values.CopyTo(array, 0);
      return array;
    }

    private void lvwUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnOk.Enabled = 0 < this.lvwUsers.SelectedItems.Count;
    }

    private void chkShowAllUsers_CheckedChanged(object sender, EventArgs e)
    {
      this.populateUserList();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
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
      this.lvwUsers = new ListView();
      this.chdrUserId = new ColumnHeader();
      this.chdrLastName = new ColumnHeader();
      this.chdrFirstName = new ColumnHeader();
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.SuspendLayout();
      this.lvwUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwUsers.Columns.AddRange(new ColumnHeader[3]
      {
        this.chdrUserId,
        this.chdrLastName,
        this.chdrFirstName
      });
      this.lvwUsers.FullRowSelect = true;
      this.lvwUsers.Location = new Point(12, 12);
      this.lvwUsers.Name = "lvwUsers";
      this.lvwUsers.Size = new Size(410, 213);
      this.lvwUsers.TabIndex = 0;
      this.lvwUsers.UseCompatibleStateImageBehavior = false;
      this.lvwUsers.View = View.Details;
      this.lvwUsers.SelectedIndexChanged += new EventHandler(this.lvwUsers_SelectedIndexChanged);
      this.chdrUserId.Text = "User ID";
      this.chdrUserId.Width = 71;
      this.chdrLastName.Text = "Last Name";
      this.chdrLastName.Width = 134;
      this.chdrFirstName.Text = "First Name";
      this.chdrFirstName.Width = 184;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(347, 231);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Location = new Point(266, 231);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 2;
      this.btnOk.Text = "&OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(434, 266);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lvwUsers);
      this.MinimizeBox = false;
      this.Name = nameof (DeployUserSelectionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select a User";
      this.ResumeLayout(false);
    }
  }
}
