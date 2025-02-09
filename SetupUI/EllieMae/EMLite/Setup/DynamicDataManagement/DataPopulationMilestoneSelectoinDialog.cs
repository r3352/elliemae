// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DataPopulationMilestoneSelectoinDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DataPopulationMilestoneSelectoinDialog : Form
  {
    private GroupContainer msDialog;
    private Sessions.Session session;
    private GridView lsvMilestone;
    private CheckBox chkAll;
    private Button btnOK;
    private Button btnCancel;
    private List<Milestone> msList;
    private List<string> milestoneNameList;

    public DataPopulationMilestoneSelectoinDialog(Sessions.Session session, HashSet<string> set)
    {
      this.session = session;
      this.InitializeComponent();
      this.PopulateMilestoneList(set);
      this.lsvMilestone_SubItemCheck((object) null, (GVSubItemEventArgs) null);
    }

    public List<string> GetSelectedMilestones()
    {
      if (this.milestoneNameList == null)
      {
        this.milestoneNameList = new List<string>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lsvMilestone.Items)
        {
          if (gvItem.SubItems[0].Checked)
            this.milestoneNameList.Add(((Milestone) gvItem.Tag).Name);
        }
      }
      return this.milestoneNameList;
    }

    private void PopulateMilestoneList(HashSet<string> set)
    {
      this.msList = this.session.StartupInfo.Milestones;
      foreach (Milestone ms in this.msList)
      {
        if (!(ms.Name == "Completion") && !ms.Archived)
          this.PopulateMisestone(ms, set);
      }
    }

    private void PopulateMisestone(Milestone ms, HashSet<string> set)
    {
      GVItem gvItem = new GVItem((object) new MilestoneLabel(ms));
      if (set.Contains(gvItem.Text))
        gvItem.SubItems[0].Checked = true;
      gvItem.Tag = (object) ms;
      this.lsvMilestone.Items.Add(gvItem);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.msDialog = new GroupContainer();
      this.chkAll = new CheckBox();
      this.lsvMilestone = new GridView();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.msDialog.SuspendLayout();
      this.SuspendLayout();
      this.msDialog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.msDialog.Controls.Add((Control) this.chkAll);
      this.msDialog.Controls.Add((Control) this.lsvMilestone);
      this.msDialog.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.msDialog.HeaderForeColor = SystemColors.ControlText;
      this.msDialog.Location = new Point(12, 12);
      this.msDialog.Name = "msDialog";
      this.msDialog.Size = new Size(449, 443);
      this.msDialog.TabIndex = 8;
      this.msDialog.Text = "Select milestones to time automated data population.";
      this.chkAll.AutoSize = true;
      this.chkAll.Location = new Point(4, 29);
      this.chkAll.Name = "chkAll";
      this.chkAll.Size = new Size(15, 14);
      this.chkAll.TabIndex = 5;
      this.chkAll.UseVisualStyleBackColor = true;
      this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
      this.lsvMilestone.AllowMultiselect = false;
      this.lsvMilestone.BorderStyle = BorderStyle.None;
      gvColumn.CheckBoxes = true;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "colMilestone";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "      Milestone";
      gvColumn.Width = 447;
      this.lsvMilestone.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.lsvMilestone.Dock = DockStyle.Fill;
      this.lsvMilestone.HotItemTracking = false;
      this.lsvMilestone.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.lsvMilestone.Location = new Point(1, 26);
      this.lsvMilestone.Name = "lsvMilestone";
      this.lsvMilestone.Size = new Size(447, 416);
      this.lsvMilestone.SortOption = GVSortOption.None;
      this.lsvMilestone.TabIndex = 4;
      this.lsvMilestone.SubItemCheck += new GVSubItemEventHandler(this.lsvMilestone_SubItemCheck);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(304, 466);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 9;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(385, 466);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.ClientSize = new Size(473, 502);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.msDialog);
      this.MinimizeBox = false;
      this.Name = nameof (DataPopulationMilestoneSelectoinDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Milestones";
      this.msDialog.ResumeLayout(false);
      this.msDialog.PerformLayout();
      this.ResumeLayout(false);
    }

    private void chkAll_CheckedChanged(object sender, EventArgs e)
    {
      this.lsvMilestone.SubItemCheck -= new GVSubItemEventHandler(this.lsvMilestone_SubItemCheck);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lsvMilestone.Items)
        gvItem.SubItems[0].Checked = this.chkAll.Checked;
      this.lsvMilestone.SubItemCheck += new GVSubItemEventHandler(this.lsvMilestone_SubItemCheck);
      this.btnOK.Enabled = this.lsvMilestone.GetCheckedItems(0).Length != 0;
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void lsvMilestone_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.btnOK.Enabled = this.lsvMilestone.GetCheckedItems(0).Length != 0;
      this.chkAll.CheckedChanged -= new EventHandler(this.chkAll_CheckedChanged);
      this.chkAll.Checked = this.lsvMilestone.GetCheckedItems(0).Length == this.lsvMilestone.Items.Count;
      this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
    }
  }
}
