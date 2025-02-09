// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanActionMilestonesListPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanActionMilestonesListPanel : UserControl
  {
    private string[] selectedMilestones;
    private IContainer components;
    private GridView lvwMilestones;
    private GroupContainer gcMilestones;

    public LoanActionMilestonesListPanel(Dictionary<string, string> msList)
    {
      this.InitializeComponent();
      this.initForm(msList);
    }

    private void initForm(Dictionary<string, string> msList)
    {
      this.lvwMilestones.Items.Clear();
      Cursor.Current = Cursors.WaitCursor;
      this.lvwMilestones.BeginUpdate();
      foreach (KeyValuePair<string, string> ms in msList)
        this.lvwMilestones.Items.Add(new GVItem(ms.Value, (object) ms.Key));
      this.lvwMilestones.EndUpdate();
      this.setTitle();
      Cursor.Current = Cursors.Default;
    }

    private void setTitle()
    {
      this.gcMilestones.Text = "Milestones (" + (object) this.lvwMilestones.Items.Count + ")";
    }

    public int SelectedMilestoneCount => this.lvwMilestones.SelectedItems.Count;

    public string[] SelectedMilestones
    {
      get
      {
        if (this.selectedMilestones == null)
        {
          this.selectedMilestones = new string[this.lvwMilestones.SelectedItems.Count];
          for (int index = 0; index < this.lvwMilestones.SelectedItems.Count; ++index)
            this.selectedMilestones[index] = this.lvwMilestones.SelectedItems[index].Tag.ToString();
        }
        return this.selectedMilestones;
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
      GVColumn gvColumn = new GVColumn();
      this.lvwMilestones = new GridView();
      this.gcMilestones = new GroupContainer();
      this.gcMilestones.SuspendLayout();
      this.SuspendLayout();
      this.lvwMilestones.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "clName";
      gvColumn.Text = "Name";
      gvColumn.Width = 284;
      this.lvwMilestones.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.lvwMilestones.Dock = DockStyle.Fill;
      this.lvwMilestones.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.lvwMilestones.Location = new Point(1, 26);
      this.lvwMilestones.Name = "lvwMilestones";
      this.lvwMilestones.Size = new Size(299, 387);
      this.lvwMilestones.TabIndex = 9;
      this.lvwMilestones.TextTrimming = StringTrimming.EllipsisWord;
      this.gcMilestones.Controls.Add((Control) this.lvwMilestones);
      this.gcMilestones.Dock = DockStyle.Fill;
      this.gcMilestones.HeaderForeColor = SystemColors.ControlText;
      this.gcMilestones.Location = new Point(0, 0);
      this.gcMilestones.Name = "gcMilestones";
      this.gcMilestones.Size = new Size(301, 414);
      this.gcMilestones.TabIndex = 11;
      this.gcMilestones.Text = "Milestones (0)";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcMilestones);
      this.Name = nameof (LoanActionMilestonesListPanel);
      this.Size = new Size(301, 414);
      this.gcMilestones.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
