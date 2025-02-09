// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LoanTeamMemberConflictDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LoanTeamMemberConflictDialog : Form
  {
    private IContainer components;
    private Label lblTitle;
    private Panel panel1;
    private Button btnOk;
    private Label lblSubTitle;
    private GridView gvUsers;

    public LoanTeamMemberConflictDialog(
      RealWorldRoleID realWorldRoleID,
      UserInfo[] users,
      string currentUserID)
    {
      this.InitializeComponent();
      this.initialPageValue(realWorldRoleID);
      this.loadUserList(users, currentUserID);
    }

    public UserInfo GetSelectedUser()
    {
      GVItem[] checkedItems = this.gvUsers.GetCheckedItems(0);
      return checkedItems.Length != 0 ? checkedItems[0].Tag as UserInfo : (UserInfo) null;
    }

    private void initialPageValue(RealWorldRoleID roleType)
    {
      string newValue = "";
      if (roleType == RealWorldRoleID.LoanOfficer)
        newValue = "Loan Officer";
      else if (roleType == RealWorldRoleID.LoanProcessor)
        newValue = "Loan Processor";
      else if (roleType == RealWorldRoleID.Underwriter)
        newValue = "Underwriter";
      else if (roleType == RealWorldRoleID.LoanCloser)
        newValue = "Loan Closer";
      else if (roleType == RealWorldRoleID.LoanProcessor)
        newValue = "Loan Processor";
      this.Text = this.Text.Replace("%ROLE%", newValue);
      this.lblTitle.Text = this.lblTitle.Text.Replace("%ROLE%", newValue);
      this.lblSubTitle.Text = this.lblSubTitle.Text.Replace("%ROLE%", newValue);
    }

    private void loadUserList(UserInfo[] users, string currentUserID)
    {
      bool flag = false;
      foreach (UserInfo user in users)
      {
        GVItem gvItem = new GVItem(user.Userid);
        gvItem.SubItems[1].Text = user.FullName;
        gvItem.Tag = (object) user;
        this.gvUsers.Items.Add(gvItem);
        if (user.Userid == currentUserID)
        {
          flag = true;
          gvItem.Checked = true;
        }
      }
      if (flag)
        return;
      this.gvUsers.Items[0].Checked = true;
    }

    private void gvUsers_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.gvUsers.BeginUpdate();
      if (e.SubItem.Checked)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvUsers.Items)
        {
          if (gvItem != e.SubItem.Item && gvItem.Checked)
            gvItem.Checked = false;
        }
      }
      if (!e.SubItem.Checked && this.gvUsers.GetCheckedItems(0).Length == 0)
        e.SubItem.Checked = true;
      this.gvUsers.EndUpdate();
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
      this.lblTitle = new Label();
      this.panel1 = new Panel();
      this.btnOk = new Button();
      this.lblSubTitle = new Label();
      this.gvUsers = new GridView();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.lblTitle.Location = new Point(8, 8);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(309, 20);
      this.lblTitle.TabIndex = 1;
      this.lblTitle.Text = "More than one %ROLE% is associated with this loan.";
      this.panel1.Controls.Add((Control) this.btnOk);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 196);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(326, 35);
      this.panel1.TabIndex = 2;
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Location = new Point(242, 3);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 0;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.lblSubTitle.Location = new Point(8, 30);
      this.lblSubTitle.Name = "lblSubTitle";
      this.lblSubTitle.Size = new Size(309, 32);
      this.lblSubTitle.TabIndex = 3;
      this.lblSubTitle.Text = "For reporting purposes, please select the primary %ROLE% for this loan.";
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "User ID";
      gvColumn1.Width = 104;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Name";
      gvColumn2.Width = 200;
      this.gvUsers.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvUsers.Location = new Point(10, 63);
      this.gvUsers.Name = "gvUsers";
      this.gvUsers.Selectable = false;
      this.gvUsers.Size = new Size(306, 125);
      this.gvUsers.TabIndex = 4;
      this.gvUsers.SubItemCheck += new GVSubItemEventHandler(this.gvUsers_SubItemCheck);
      this.AcceptButton = (IButtonControl) this.btnOk;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(326, 231);
      this.Controls.Add((Control) this.gvUsers);
      this.Controls.Add((Control) this.lblSubTitle);
      this.Controls.Add((Control) this.lblTitle);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanTeamMemberConflictDialog);
      this.Text = "Select %ROLE%";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
