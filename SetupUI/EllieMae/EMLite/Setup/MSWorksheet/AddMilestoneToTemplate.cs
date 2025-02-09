// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MSWorksheet.AddMilestoneToTemplate
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.MSWorksheet
{
  public class AddMilestoneToTemplate : Form
  {
    private RoleInfo[] roles;
    private List<EllieMae.EMLite.Workflow.Milestone> availableMilestones;
    private List<EllieMae.EMLite.Workflow.Milestone> milestonesAdded = new List<EllieMae.EMLite.Workflow.Milestone>();
    private Sessions.Session session;
    private IContainer components;
    private Panel panel1;
    private GroupContainer grpAvailableMilestones;
    private GridView gvAvailableMilestones;
    private EMHelpLink emHelpLink1;
    private Button btnCancel;
    private Button btnSubmit;
    private Label label1;

    public AddMilestoneToTemplate(List<EllieMae.EMLite.Workflow.Milestone> availableMilestones, RoleInfo[] roles)
      : this(Session.DefaultInstance, availableMilestones, roles)
    {
    }

    public AddMilestoneToTemplate(
      Sessions.Session session,
      List<EllieMae.EMLite.Workflow.Milestone> availableMilestones,
      RoleInfo[] roles)
    {
      this.InitializeComponent();
      this.roles = roles;
      this.availableMilestones = availableMilestones;
      availableMilestones.ForEach((Action<EllieMae.EMLite.Workflow.Milestone>) (item => this.gvAvailableMilestones.Items.Add(this.createGVItemFormAvailableMilestone(item))));
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
    }

    private GVItem createGVItemFormAvailableMilestone(EllieMae.EMLite.Workflow.Milestone ms)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) new MilestoneLabel(ms)
          },
          [1] = {
            Text = this.getRoleName(ms.RoleID)
          }
        },
        Tag = (object) ms
      };
    }

    private string getRoleName(int roleId)
    {
      RoleInfo roleInfo = ((IEnumerable<RoleInfo>) this.roles).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == roleId));
      return roleInfo != null ? roleInfo.RoleName : "";
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAvailableMilestones.Items)
      {
        if (gvItem.Checked)
          this.milestonesAdded.Add((EllieMae.EMLite.Workflow.Milestone) gvItem.Tag);
      }
      this.DialogResult = DialogResult.OK;
    }

    public List<EllieMae.EMLite.Workflow.Milestone> MilestonesAdded => this.milestonesAdded;

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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddMilestoneToTemplate));
      this.panel1 = new Panel();
      this.grpAvailableMilestones = new GroupContainer();
      this.gvAvailableMilestones = new GridView();
      this.btnCancel = new Button();
      this.btnSubmit = new Button();
      this.label1 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.panel1.SuspendLayout();
      this.grpAvailableMilestones.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.grpAvailableMilestones);
      this.panel1.Location = new Point(9, 39);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(504, 522);
      this.panel1.TabIndex = 3;
      this.grpAvailableMilestones.AutoScroll = true;
      this.grpAvailableMilestones.Borders = AnchorStyles.Left;
      this.grpAvailableMilestones.Controls.Add((Control) this.gvAvailableMilestones);
      this.grpAvailableMilestones.Dock = DockStyle.Fill;
      this.grpAvailableMilestones.HeaderForeColor = SystemColors.ControlText;
      this.grpAvailableMilestones.Location = new Point(0, 0);
      this.grpAvailableMilestones.Name = "grpAvailableMilestones";
      this.grpAvailableMilestones.Size = new Size(502, 520);
      this.grpAvailableMilestones.TabIndex = 3;
      this.grpAvailableMilestones.Text = "Available Milestones";
      this.gvAvailableMilestones.AllowDrop = true;
      this.gvAvailableMilestones.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Milestone";
      gvColumn1.Width = 300;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Role";
      gvColumn2.Width = 200;
      this.gvAvailableMilestones.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvAvailableMilestones.Dock = DockStyle.Fill;
      this.gvAvailableMilestones.DropTarget = GVDropTarget.Item;
      this.gvAvailableMilestones.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAvailableMilestones.Location = new Point(1, 25);
      this.gvAvailableMilestones.Name = "gvAvailableMilestones";
      this.gvAvailableMilestones.Size = new Size(501, 495);
      this.gvAvailableMilestones.TabIndex = 2;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(441, 567);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 12;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSubmit.Location = new Point(359, 567);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new Size(76, 23);
      this.btnSubmit.TabIndex = 11;
      this.btnSubmit.Text = "Add";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(284, 13);
      this.label1.TabIndex = 14;
      this.label1.Text = "Select the milestones to add to the template and click Add.";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = nameof (AddMilestoneToTemplate);
      this.emHelpLink1.Location = new Point(12, 574);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 13;
      this.AcceptButton = (IButtonControl) this.btnSubmit;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(525, 602);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSubmit);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddMilestoneToTemplate);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Milestone Selection";
      this.panel1.ResumeLayout(false);
      this.grpAvailableMilestones.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
