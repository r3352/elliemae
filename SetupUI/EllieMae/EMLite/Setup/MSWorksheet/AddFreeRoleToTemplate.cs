// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MSWorksheet.AddFreeRoleToTemplate
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.MSWorksheet
{
  public class AddFreeRoleToTemplate : Form
  {
    private List<RoleInfo> roles;
    private List<RoleInfo> rolesAdded = new List<RoleInfo>();
    private Sessions.Session session;
    private IContainer components;
    private Panel panel1;
    private GroupContainer grpAvailableMilestones;
    private GridView gvAvailableRoles;
    private Label label1;
    private EMHelpLink emHelpLink1;
    private Button btnCancel;
    private Button btnSubmit;

    public AddFreeRoleToTemplate(List<RoleInfo> roles)
      : this(Session.DefaultInstance, roles)
    {
    }

    public AddFreeRoleToTemplate(Sessions.Session session, List<RoleInfo> roles)
    {
      this.InitializeComponent();
      this.roles = roles;
      this.roles.ForEach((Action<RoleInfo>) (item => this.gvAvailableRoles.Items.Add(this.createGVItemFormAvailableRoles(item))));
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
    }

    private GVItem createGVItemFormAvailableRoles(RoleInfo role)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Text = role.RoleName
          }
        },
        Tag = (object) role
      };
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAvailableRoles.Items)
      {
        if (gvItem.Checked)
          this.rolesAdded.Add((RoleInfo) gvItem.Tag);
      }
      this.DialogResult = DialogResult.OK;
    }

    public List<RoleInfo> RolesAdded => this.rolesAdded;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddFreeRoleToTemplate));
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
      this.panel1.TabIndex = 15;
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
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(343, 13);
      this.label1.TabIndex = 19;
      this.label1.Text = "Select the roles to assign to the milestone template, and then click Add.";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = nameof (AddFreeRoleToTemplate);
      this.emHelpLink1.Location = new Point(10, 571);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 18;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(431, 564);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 17;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSubmit.Location = new Point(349, 564);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new Size(76, 23);
      this.btnSubmit.TabIndex = 16;
      this.btnSubmit.Text = "Add";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
      this.AcceptButton = (IButtonControl) this.btnSubmit;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(522, 599);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSubmit);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddFreeRoleToTemplate);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Role Selection";
      this.panel1.ResumeLayout(false);
      this.grpAvailableMilestones.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
