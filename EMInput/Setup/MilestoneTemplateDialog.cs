// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MilestoneTemplateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MilestoneTemplateDialog : Form
  {
    private Sessions.Session session;
    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> ms;
    private RoleInfo[] roles;
    private IContainer components;
    private Panel panel1;
    private GroupContainer groupContainer1;
    private BorderPanel borderPanel1;
    private GridView gvMilestones;
    private Button btnOK;

    public MilestoneTemplateDialog(Sessions.Session session, MilestoneTemplate template)
    {
      this.session = session;
      this.ms = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllActiveMilestonesList();
      this.roles = this.session.SessionObjects.BpmManager.GetAllRoleFunctions();
      this.InitializeComponent();
      this.populateMilestones(template);
    }

    private void populateMilestones(MilestoneTemplate template)
    {
      foreach (TemplateMilestone sequentialMilestone in template.SequentialMilestones)
      {
        EllieMae.EMLite.Workflow.Milestone milestone = this.getMilestone(sequentialMilestone.MilestoneID);
        if (milestone != null)
          this.gvMilestones.Items.Add(new GVItem()
          {
            SubItems = {
              [0] = {
                Value = (object) new MilestoneLabel(milestone)
              },
              [1] = {
                Text = this.getRoleName(milestone.RoleID)
              }
            }
          });
      }
    }

    private EllieMae.EMLite.Workflow.Milestone getMilestone(string milestoneID)
    {
      return this.ms.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (item => item.MilestoneID == milestoneID));
    }

    private string getRoleName(int roleId)
    {
      RoleInfo roleInfo = ((IEnumerable<RoleInfo>) this.roles).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == roleId));
      return roleInfo != null ? roleInfo.RoleName : "";
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

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
      this.panel1 = new Panel();
      this.btnOK = new Button();
      this.groupContainer1 = new GroupContainer();
      this.borderPanel1 = new BorderPanel();
      this.gvMilestones = new GridView();
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.AutoSize = true;
      this.panel1.Controls.Add((Control) this.btnOK);
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(288, 362);
      this.panel1.TabIndex = 0;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.Cancel;
      this.btnOK.Location = new Point(209, 330);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.groupContainer1.AutoSize = true;
      this.groupContainer1.Controls.Add((Control) this.borderPanel1);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(3, 3);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(281, 321);
      this.groupContainer1.TabIndex = 4;
      this.groupContainer1.Text = "This template contains the following milestones:";
      this.borderPanel1.Borders = AnchorStyles.Bottom;
      this.borderPanel1.Controls.Add((Control) this.gvMilestones);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(1, 26);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(279, 294);
      this.borderPanel1.TabIndex = 1;
      this.gvMilestones.AllowDrop = true;
      this.gvMilestones.AllowMultiselect = false;
      this.gvMilestones.AutoHeight = true;
      this.gvMilestones.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Milestone";
      gvColumn1.Width = 130;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Role";
      gvColumn2.Width = 125;
      this.gvMilestones.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvMilestones.Dock = DockStyle.Fill;
      this.gvMilestones.DropTarget = GVDropTarget.BetweenItems;
      this.gvMilestones.Location = new Point(0, 0);
      this.gvMilestones.Name = "gvMilestones";
      this.gvMilestones.Size = new Size(279, 293);
      this.gvMilestones.SortOption = GVSortOption.None;
      this.gvMilestones.TabIndex = 1;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.CancelButton = (IButtonControl) this.btnOK;
      this.ClientSize = new Size(288, 362);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MilestoneTemplateDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Milestone Template ";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
