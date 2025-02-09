// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ProcessorSelectionDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ProcessorSelectionDialog : Form
  {
    private System.ComponentModel.Container components;
    private Button okBtn;
    private Button cancelBtn;
    private GridView gvUsers;
    private GroupContainer grpUsers;
    private Panel panel1;
    private GroupContainer grpGroups;
    private GridView gvGroups;
    private Panel pnlDivider;
    private Button clearBtn;
    private LoanAssociateLog loanAssociate;
    private Label lblNoUsers;
    private RoleInfo currentRole;

    public ProcessorSelectionDialog(LoanAssociateLog laLog, bool clearUser)
    {
      this.loanAssociate = laLog;
      this.InitializeComponent();
      if (!clearUser)
      {
        this.clearBtn.Visible = false;
        this.okBtn.Left = this.clearBtn.Left;
      }
      this.currentRole = this.fetchCurrentRole(laLog.RoleID);
      this.loadUserList();
      this.loadGroupList();
      if (this.gvGroups.Items.Count == 0 && this.gvUsers.Items.Count == 0)
      {
        this.grpGroups.Visible = false;
        this.grpUsers.Visible = false;
        this.pnlDivider.Visible = false;
        this.lblNoUsers.Visible = true;
        this.lblNoUsers.Dock = DockStyle.Fill;
      }
      else if (this.gvGroups.Items.Count == 0)
      {
        this.pnlDivider.Visible = false;
        this.grpGroups.Visible = false;
        this.grpUsers.Dock = DockStyle.Fill;
      }
      else
      {
        if (this.gvUsers.Items.Count != 0)
          return;
        this.pnlDivider.Visible = false;
        this.grpUsers.Visible = false;
        this.grpGroups.Dock = DockStyle.Fill;
        this.grpGroups.Text = "Select a User Group";
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.gvUsers = new GridView();
      this.clearBtn = new Button();
      this.grpUsers = new GroupContainer();
      this.panel1 = new Panel();
      this.lblNoUsers = new Label();
      this.pnlDivider = new Panel();
      this.grpGroups = new GroupContainer();
      this.gvGroups = new GridView();
      this.grpUsers.SuspendLayout();
      this.panel1.SuspendLayout();
      this.grpGroups.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Location = new Point(121, 403);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 22);
      this.okBtn.TabIndex = 2;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(287, 403);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 3;
      this.cancelBtn.Text = "&Cancel";
      this.gvUsers.AllowMultiselect = false;
      this.gvUsers.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "User ID";
      gvColumn1.Width = 105;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 119;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 126;
      this.gvUsers.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvUsers.Dock = DockStyle.Fill;
      this.gvUsers.Location = new Point(1, 26);
      this.gvUsers.Name = "gvUsers";
      this.gvUsers.Size = new Size(350, 185);
      this.gvUsers.TabIndex = 4;
      this.gvUsers.SelectedIndexChanged += new EventHandler(this.gvUsers_SelectedIndexChanged);
      this.gvUsers.ItemDoubleClick += new GVItemEventHandler(this.gvUsers_ItemDoubleClick);
      this.clearBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.clearBtn.DialogResult = DialogResult.No;
      this.clearBtn.Location = new Point(204, 403);
      this.clearBtn.Name = "clearBtn";
      this.clearBtn.Size = new Size(75, 22);
      this.clearBtn.TabIndex = 5;
      this.clearBtn.Text = "Cl&ear User";
      this.clearBtn.Click += new EventHandler(this.clearBtn_Click);
      this.grpUsers.Controls.Add((Control) this.gvUsers);
      this.grpUsers.Dock = DockStyle.Fill;
      this.grpUsers.HeaderForeColor = SystemColors.ControlText;
      this.grpUsers.Location = new Point(0, 32);
      this.grpUsers.Name = "grpUsers";
      this.grpUsers.Size = new Size(352, 212);
      this.grpUsers.TabIndex = 6;
      this.grpUsers.Text = "Select a User";
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.grpUsers);
      this.panel1.Controls.Add((Control) this.lblNoUsers);
      this.panel1.Controls.Add((Control) this.pnlDivider);
      this.panel1.Controls.Add((Control) this.grpGroups);
      this.panel1.Location = new Point(10, 10);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(352, 382);
      this.panel1.TabIndex = 7;
      this.lblNoUsers.Dock = DockStyle.Top;
      this.lblNoUsers.Location = new Point(0, 0);
      this.lblNoUsers.Name = "lblNoUsers";
      this.lblNoUsers.Size = new Size(352, 32);
      this.lblNoUsers.TabIndex = 9;
      this.lblNoUsers.Text = "There are no users or User Groups eligible for assignment to the selected Role.";
      this.lblNoUsers.Visible = false;
      this.pnlDivider.Dock = DockStyle.Bottom;
      this.pnlDivider.Location = new Point(0, 244);
      this.pnlDivider.Name = "pnlDivider";
      this.pnlDivider.Size = new Size(352, 10);
      this.pnlDivider.TabIndex = 8;
      this.grpGroups.Controls.Add((Control) this.gvGroups);
      this.grpGroups.Dock = DockStyle.Bottom;
      this.grpGroups.HeaderForeColor = SystemColors.ControlText;
      this.grpGroups.Location = new Point(0, 254);
      this.grpGroups.Name = "grpGroups";
      this.grpGroups.Size = new Size(352, 128);
      this.grpGroups.TabIndex = 7;
      this.grpGroups.Text = "OR Select a User Group";
      this.gvGroups.AllowMultiselect = false;
      this.gvGroups.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "User Group Name";
      gvColumn4.Width = 350;
      this.gvGroups.Columns.AddRange(new GVColumn[1]
      {
        gvColumn4
      });
      this.gvGroups.Dock = DockStyle.Fill;
      this.gvGroups.Location = new Point(1, 26);
      this.gvGroups.Name = "gvGroups";
      this.gvGroups.Size = new Size(350, 101);
      this.gvGroups.TabIndex = 4;
      this.gvGroups.SelectedIndexChanged += new EventHandler(this.gvGroups_SelectedIndexChanged);
      this.gvGroups.ItemDoubleClick += new GVItemEventHandler(this.gvGroups_ItemDoubleClick);
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(372, 434);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.clearBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (ProcessorSelectionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Loan Team Member";
      this.grpUsers.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.grpGroups.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private RoleInfo fetchCurrentRole(int roleId)
    {
      foreach (RoleInfo allRole in Session.LoanDataMgr.SystemConfiguration.AllRoles)
      {
        if (allRole.RoleID == roleId)
          return allRole;
      }
      return (RoleInfo) null;
    }

    private void loadUserList()
    {
      UserInfo[] userInfoArray;
      if (this.currentRole != null)
      {
        userInfoArray = Session.OrganizationManager.GetScopedUsersWithRole(this.currentRole.RoleID);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The current role " + this.loanAssociate.RoleName + " is not in current Encompass setting. All accessible users can be assigned.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        userInfoArray = Session.OrganizationManager.GetAllAccessibleUsers();
      }
      this.gvUsers.Items.Clear();
      foreach (UserInfo userInfo in userInfoArray)
        this.gvUsers.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = userInfo.Userid
            },
            [1] = {
              Text = userInfo.LastName
            },
            [2] = {
              Text = userInfo.FirstName
            }
          },
          Tag = (object) userInfo
        });
      this.gvUsers.Sort(0, SortOrder.Ascending);
    }

    private void loadGroupList()
    {
      if (this.currentRole == null || this.currentRole.UserGroupIDs.Length == 0)
        return;
      foreach (AclGroup group in Session.AclGroupManager.GetGroups(this.currentRole.UserGroupIDs))
        this.gvGroups.Items.Add(new GVItem(group.Name)
        {
          Tag = (object) group
        });
      this.gvGroups.Sort(0, SortOrder.Ascending);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.gvUsers.Items.Count == 0 && this.gvGroups.Items.Count == 0)
        this.DialogResult = DialogResult.Cancel;
      else if (this.gvUsers.SelectedItems.Count == 0 && this.gvGroups.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a loan associate from the list provided.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!(this.gvUsers.SelectedItems.Count <= 0 ? this.applySelectedGroup((AclGroup) this.gvGroups.SelectedItems[0].Tag) : this.applySelectedUser((UserInfo) this.gvUsers.SelectedItems[0].Tag)))
          return;
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool applySelectedGroup(AclGroup group)
    {
      try
      {
        Session.LoanDataMgr.AssignLoanAssociate(this.loanAssociate, group);
        Session.LoanDataMgr.Calculator.UpdateLenderRepresentative(this.loanAssociate, "all");
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private bool applySelectedUser(UserInfo user)
    {
      if (Session.LoanDataMgr.GetRealWorldRoleID(this.loanAssociate.RoleID).Contains(RealWorldRoleID.LoanOfficer) && !Session.LoanDataMgr.ValidateLOLicense(user))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have selected a loan officer who does not have the current license(s) needed to originate a loan in the property state " + Session.LoanData.GetField("14") + ". Contact your system administrator for more details.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      try
      {
        Session.LoanDataMgr.AssignLoanAssociate(this.loanAssociate, user, new ResolveLoanAssociateHandler(this.resolveLoanAssociates));
        Session.LoanDataMgr.Calculator.UpdateLenderRepresentative(this.loanAssociate, "all");
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private UserInfo resolveLoanAssociates(
      RealWorldRoleID realWorldRole,
      UserInfo[] eligibleUsers,
      string currentUserID)
    {
      using (LoanTeamMemberConflictDialog memberConflictDialog = new LoanTeamMemberConflictDialog(realWorldRole, eligibleUsers, currentUserID))
      {
        if (memberConflictDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          return memberConflictDialog.GetSelectedUser();
      }
      return (UserInfo) null;
    }

    private void gvUsers_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (this.gvUsers.SelectedItems.Count <= 0)
        return;
      this.okBtn_Click((object) this, (EventArgs) null);
    }

    private void gvGroups_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (this.gvGroups.SelectedItems.Count <= 0)
        return;
      this.okBtn_Click((object) this, (EventArgs) null);
    }

    private void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvUsers.SelectedItems.Count <= 0)
        return;
      this.gvGroups.SelectedItems.Clear();
    }

    private void gvGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvGroups.SelectedItems.Count <= 0)
        return;
      this.gvUsers.SelectedItems.Clear();
    }

    private void clearBtn_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Remove the current loan associate from the selected role?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      Session.LoanDataMgr.ClearLoanAssociate(this.loanAssociate);
      Session.LoanDataMgr.Calculator.UpdateLenderRepresentative(this.loanAssociate, "all");
      this.DialogResult = DialogResult.OK;
    }
  }
}
