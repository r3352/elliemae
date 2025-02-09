// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SelectCurrentRoleForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SelectCurrentRoleForm : Form
  {
    private Sessions.Session session;
    private string milestoneID = string.Empty;
    private int roleID = -1;
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private GridView lsvMilestone;

    public SelectCurrentRoleForm(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.initForm();
    }

    public string MilestoneID => this.milestoneID;

    public int RoleID => this.roleID;

    private void initForm()
    {
      this.lsvMilestone.Items.Clear();
      foreach (EllieMae.EMLite.Workflow.Milestone activeMilestones in ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllActiveMilestonesList())
      {
        if (activeMilestones.Name != null && !(activeMilestones.Name == "Completion") && activeMilestones.RoleID > 0)
        {
          MilestoneLabel milestoneLabel = new MilestoneLabel(activeMilestones);
          this.lsvMilestone.Items.Add(new GVItem((object) this.session.SessionObjects.BpmManager.GetRoleFunction(activeMilestones.RoleID))
          {
            SubItems = {
              [1] = {
                Value = (object) milestoneLabel
              }
            },
            Tag = (object) activeMilestones
          });
        }
      }
    }

    private void listViewMS_DoubleClick(object sender, EventArgs e)
    {
      this.btnOK_Click((object) null, (EventArgs) null);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.lsvMilestone.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a role.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        EllieMae.EMLite.Workflow.Milestone tag = (EllieMae.EMLite.Workflow.Milestone) this.lsvMilestone.SelectedItems[0].Tag;
        this.milestoneID = tag.MilestoneID;
        this.roleID = tag.RoleID;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void SelectCurrentRoleForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }

    private void lsvMilestone_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnOK_Click((object) null, (EventArgs) null);
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
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.lsvMilestone = new GridView();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(237, 320);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(64, 24);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnOK.Location = new Point(167, 320);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(64, 24);
      this.btnOK.TabIndex = 9;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.lsvMilestone.AllowMultiselect = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column2";
      gvColumn1.Text = "Role";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.Text = "Milestones";
      gvColumn2.Width = 180;
      this.lsvMilestone.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.lsvMilestone.Location = new Point(12, 12);
      this.lsvMilestone.Name = "lsvMilestone";
      this.lsvMilestone.Size = new Size(289, 302);
      this.lsvMilestone.SortOption = GVSortOption.None;
      this.lsvMilestone.TabIndex = 11;
      this.lsvMilestone.ItemDoubleClick += new GVItemEventHandler(this.lsvMilestone_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(313, 355);
      this.Controls.Add((Control) this.lsvMilestone);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectCurrentRoleForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Current Role";
      this.KeyDown += new KeyEventHandler(this.SelectCurrentRoleForm_KeyDown);
      this.ResumeLayout(false);
    }
  }
}
