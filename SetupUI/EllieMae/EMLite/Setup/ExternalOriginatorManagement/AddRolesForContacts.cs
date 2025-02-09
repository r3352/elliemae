// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.AddRolesForContacts
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class AddRolesForContacts : Form
  {
    private List<ExternalUserInfo.ContactRoles> roles;
    private List<ExternalUserInfo.ContactRoles> rolesAdded = new List<ExternalUserInfo.ContactRoles>();
    private IContainer components;
    private Panel panel1;
    private GroupContainer grpAvailableMilestones;
    private GridView gvAvailableRoles;
    private Label label1;
    private EMHelpLink emHelpLink1;
    private Button btnCancel;
    private Button btnSubmit;

    public AddRolesForContacts(List<ExternalUserInfo.ContactRoles> roles)
    {
      this.InitializeComponent();
      this.roles = roles;
      this.roles.ForEach((Action<ExternalUserInfo.ContactRoles>) (item => this.gvAvailableRoles.Items.Add(this.createGVItemFormAvailableRoles(item))));
      this.btnSubmit.Enabled = false;
    }

    private GVItem createGVItemFormAvailableRoles(ExternalUserInfo.ContactRoles role)
    {
      GVItem formAvailableRoles = new GVItem();
      if (role == ExternalUserInfo.ContactRoles.LoanOfficer)
        formAvailableRoles.SubItems[0].Text = "Loan Officer";
      if (role == ExternalUserInfo.ContactRoles.LoanProcessor)
        formAvailableRoles.SubItems[0].Text = "Loan Processor";
      if (role == ExternalUserInfo.ContactRoles.Manager)
        formAvailableRoles.SubItems[0].Text = "Manager";
      if (role == ExternalUserInfo.ContactRoles.Administrator)
        formAvailableRoles.SubItems[0].Text = "Administrator";
      formAvailableRoles.Tag = (object) role;
      return formAvailableRoles;
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAvailableRoles.Items)
      {
        if (gvItem.Checked)
          this.rolesAdded.Add((ExternalUserInfo.ContactRoles) gvItem.Tag);
      }
      this.DialogResult = DialogResult.OK;
    }

    public List<ExternalUserInfo.ContactRoles> RolesAdded => this.rolesAdded;

    private void gvAvailableRoles_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.btnSubmit.Enabled = ((IEnumerable<GVItem>) this.gvAvailableRoles.GetCheckedItems(0)).Any<GVItem>();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddRolesForContacts));
      this.panel1 = new Panel();
      this.grpAvailableMilestones = new GroupContainer();
      this.gvAvailableRoles = new GridView();
      this.label1 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.btnCancel = new Button();
      this.btnSubmit = new Button();
      this.panel1.SuspendLayout();
      this.grpAvailableMilestones.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.grpAvailableMilestones);
      this.panel1.Location = new Point(8, 35);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(504, 522);
      this.panel1.TabIndex = 20;
      this.grpAvailableMilestones.AutoScroll = true;
      this.grpAvailableMilestones.Borders = AnchorStyles.Left;
      this.grpAvailableMilestones.Controls.Add((Control) this.gvAvailableRoles);
      this.grpAvailableMilestones.Dock = DockStyle.Fill;
      this.grpAvailableMilestones.HeaderForeColor = SystemColors.ControlText;
      this.grpAvailableMilestones.Location = new Point(0, 0);
      this.grpAvailableMilestones.Name = "grpAvailableMilestones";
      this.grpAvailableMilestones.Size = new Size(502, 520);
      this.grpAvailableMilestones.TabIndex = 3;
      this.grpAvailableMilestones.Text = "Available Roles";
      this.gvAvailableRoles.AllowDrop = true;
      this.gvAvailableRoles.BorderStyle = BorderStyle.None;
      gvColumn.CheckBoxes = true;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Roles";
      gvColumn.Width = 500;
      this.gvAvailableRoles.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvAvailableRoles.Dock = DockStyle.Fill;
      this.gvAvailableRoles.DropTarget = GVDropTarget.Item;
      this.gvAvailableRoles.Location = new Point(1, 25);
      this.gvAvailableRoles.Name = "gvAvailableRoles";
      this.gvAvailableRoles.Size = new Size(501, 495);
      this.gvAvailableRoles.TabIndex = 2;
      this.gvAvailableRoles.SubItemCheck += new GVSubItemEventHandler(this.gvAvailableRoles_SubItemCheck);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(292, 13);
      this.label1.TabIndex = 24;
      this.label1.Text = "Select the roles to assign to the contact, and then click Add.";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Company Details\\AddTPO Contact";
      this.emHelpLink1.Location = new Point(8, 566);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 23;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(437, 565);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 22;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSubmit.Location = new Point(355, 565);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new Size(76, 23);
      this.btnSubmit.TabIndex = 21;
      this.btnSubmit.Text = "Add";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
      this.AcceptButton = (IButtonControl) this.btnSubmit;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(518, 594);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSubmit);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddRolesForContacts);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add roles for the contact";
      this.panel1.ResumeLayout(false);
      this.grpAvailableMilestones.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
