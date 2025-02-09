// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.UserPicker
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class UserPicker : Form
  {
    private bool multiSelect = true;
    private IContainer components;
    private Label lblTitle;
    private DialogButtons dlgButtons;
    private GridView gvUsers;

    public UserPicker(UserInfoSummary[] users, bool allowMultiSelect)
    {
      this.InitializeComponent();
      this.multiSelect = allowMultiSelect;
      this.loadUserInfoSummaries(users);
      this.configureUIForMultiSelect();
    }

    public UserPicker(UserInfo[] users, bool allowMultiSelect)
    {
      this.InitializeComponent();
      this.multiSelect = allowMultiSelect;
      this.loadUserInfos(users);
      this.configureUIForMultiSelect();
    }

    private void configureUIForMultiSelect()
    {
      this.gvUsers.Columns[0].CheckBoxes = this.multiSelect;
      this.gvUsers.Selectable = !this.multiSelect;
      if (this.multiSelect)
        return;
      this.lblTitle.Text = "Select a user from the list provided:";
    }

    public UserInfo[] GetSelectedUserInfos()
    {
      if (this.multiSelect)
        return (UserInfo[]) this.getCheckedUserObjects().ToArray(typeof (UserInfo));
      return new UserInfo[1]
      {
        this.gvUsers.SelectedItems[0].Tag as UserInfo
      };
    }

    public UserInfoSummary[] GetSelectedUserInfoSummaries()
    {
      if (this.multiSelect)
        return (UserInfoSummary[]) this.getCheckedUserObjects().ToArray(typeof (UserInfoSummary));
      return new UserInfoSummary[1]
      {
        this.gvUsers.SelectedItems[0].Tag as UserInfoSummary
      };
    }

    private void loadUserInfos(UserInfo[] users)
    {
      this.gvUsers.Items.Clear();
      foreach (UserInfo user in users)
        this.gvUsers.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = user.Userid
            },
            [1] = {
              Text = user.LastName
            },
            [2] = {
              Text = user.FirstName
            }
          },
          Tag = (object) user
        });
      this.gvUsers.Sort(0, SortOrder.Ascending);
    }

    private void loadUserInfoSummaries(UserInfoSummary[] users)
    {
      this.gvUsers.Items.Clear();
      foreach (UserInfoSummary user in users)
        this.gvUsers.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = user.UserID
            },
            [1] = {
              Text = user.LastName
            },
            [2] = {
              Text = user.FirstName
            }
          },
          Tag = (object) user
        });
      this.gvUsers.Sort(0, SortOrder.Ascending);
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (!(!this.multiSelect ? this.validateSingleUserSelection() : this.validateMultiUserSelection()))
        return;
      this.DialogResult = DialogResult.OK;
    }

    private bool validateMultiUserSelection()
    {
      if (this.getCheckedUserObjects().Count != 0)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must select one or more users from the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private bool validateSingleUserSelection()
    {
      if (this.gvUsers.SelectedItems.Count != 0)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a user from the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private ArrayList getCheckedUserObjects()
    {
      ArrayList checkedUserObjects = new ArrayList();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvUsers.Items)
      {
        if (gvItem.SubItems[0].Checked)
          checkedUserObjects.Add(gvItem.Tag);
      }
      return checkedUserObjects;
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
      this.lblTitle = new Label();
      this.dlgButtons = new DialogButtons();
      this.gvUsers = new GridView();
      this.SuspendLayout();
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(8, 9);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(236, 14);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Select one or more users from the list provided:";
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 283);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(332, 47);
      this.dlgButtons.TabIndex = 1;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.gvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "User ID";
      gvColumn1.Width = 90;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 118;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 100;
      this.gvUsers.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvUsers.Location = new Point(10, 25);
      this.gvUsers.Name = "gvUsers";
      this.gvUsers.Size = new Size(310, 258);
      this.gvUsers.TabIndex = 2;
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(332, 330);
      this.Controls.Add((Control) this.gvUsers);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.lblTitle);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (UserPicker);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select User(s)";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
