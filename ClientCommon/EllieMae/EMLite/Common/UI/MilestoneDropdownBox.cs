// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.MilestoneDropdownBox
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class MilestoneDropdownBox : UserControl
  {
    private Size customSize;
    private bool blank;
    private IContainer components;
    private ComboBoxEx cboMilestones;
    private ComboBox cboAutoHeight;

    public event EventHandler SelectedIndexChanged;

    public MilestoneDropdownBox() => this.InitializeComponent();

    public void PopulateAllMilestones(
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> allActiveMilestones,
      bool includeFileStarted,
      bool includeBlank)
    {
      this.PopulateAllMilestones(allActiveMilestones, includeFileStarted, true, includeBlank);
    }

    public void PopulateAllMilestones(
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> allActiveMilestones,
      bool includeFileStarted,
      bool includeArchived,
      bool includeBlank)
    {
      this.blank = includeBlank;
      this.cboMilestones.Items.Clear();
      if (includeBlank)
        this.cboMilestones.Items.Add((object) "");
      List<EllieMae.EMLite.Workflow.Milestone> milestoneList = new List<EllieMae.EMLite.Workflow.Milestone>();
      foreach (EllieMae.EMLite.Workflow.Milestone allActiveMilestone in allActiveMilestones)
      {
        if (allActiveMilestone != null)
        {
          if (allActiveMilestone.Archived)
          {
            milestoneList.Add(allActiveMilestone);
          }
          else
          {
            MilestoneLabel milestoneLabel = new MilestoneLabel(allActiveMilestone);
            milestoneLabel.Tag = (object) allActiveMilestone.MilestoneID;
            if (!allActiveMilestone.Name.Equals("Started") | includeFileStarted)
              this.cboMilestones.Items.Add((object) milestoneLabel);
          }
        }
      }
      if (!includeArchived)
        return;
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestoneList)
      {
        MilestoneLabel milestoneLabel = new MilestoneLabel(milestone);
        milestoneLabel.Tag = (object) milestone.MilestoneID;
        this.cboMilestones.Items.Add((object) milestoneLabel);
      }
    }

    public void SetDefaultMilestoneID(string defaultMilestoneID)
    {
      if (defaultMilestoneID == "1")
      {
        if (this.blank)
        {
          MilestoneLabel milestoneLabel = (MilestoneLabel) this.cboMilestones.Items[1];
          milestoneLabel.DisplayName = milestoneLabel.MilestoneName + " (System Default)";
          this.cboMilestones.Items[1] = (object) milestoneLabel;
        }
        else
        {
          MilestoneLabel milestoneLabel = (MilestoneLabel) this.cboMilestones.Items[0];
          milestoneLabel.DisplayName = milestoneLabel.MilestoneName + " (System Default)";
          this.cboMilestones.Items[0] = (object) milestoneLabel;
        }
      }
      if (!(defaultMilestoneID == "7"))
        return;
      MilestoneLabel milestoneLabel1 = (MilestoneLabel) this.cboMilestones.Items[this.cboMilestones.Items.Count - 1];
      milestoneLabel1.DisplayName = milestoneLabel1.MilestoneName + " (System Default)";
      this.cboMilestones.Items[this.cboMilestones.Items.Count - 1] = (object) milestoneLabel1;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string MilestoneName
    {
      get
      {
        return this.cboMilestones.SelectedIndex < 1 ? (string) null : ((MilestoneLabel) this.cboMilestones.SelectedItem).MilestoneName;
      }
      set
      {
        if ((value ?? "") == "")
        {
          this.cboMilestones.SelectedIndex = 0;
        }
        else
        {
          foreach (object obj in this.cboMilestones.Items)
          {
            if (obj is MilestoneLabel && ((MilestoneLabel) obj).MilestoneName.Equals(value))
            {
              this.cboMilestones.SelectedItem = obj;
              break;
            }
          }
        }
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string MilestoneID
    {
      get
      {
        if (this.blank && this.cboMilestones.SelectedIndex < 1)
          return (string) null;
        return this.cboMilestones.SelectedItem != null ? ((Element) this.cboMilestones.SelectedItem).Tag.ToString() : (string) null;
      }
      set
      {
        if ((value ?? "") == "")
        {
          this.cboMilestones.SelectedIndex = 0;
        }
        else
        {
          foreach (object obj in this.cboMilestones.Items)
          {
            if (obj is MilestoneLabel && ((Element) obj).Tag.Equals((object) value))
            {
              this.cboMilestones.SelectedItem = obj;
              break;
            }
          }
        }
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string MilestoneDisplayName
    {
      get
      {
        return this.cboMilestones.SelectedIndex < 1 ? (string) null : ((MilestoneLabel) this.cboMilestones.SelectedItem).DisplayName;
      }
      set
      {
        if ((value ?? "") == "")
        {
          this.cboMilestones.SelectedIndex = 0;
        }
        else
        {
          foreach (object obj in this.cboMilestones.Items)
          {
            if (obj is MilestoneLabel && string.Compare(((MilestoneLabel) obj).DisplayName, value, true) == 0)
            {
              this.cboMilestones.SelectedItem = obj;
              break;
            }
          }
        }
      }
    }

    private void cboMilestones_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.OnSelectedIndexChanged(e);
    }

    protected void OnSelectedIndexChanged(EventArgs e)
    {
      if (this.SelectedIndexChanged == null)
        return;
      this.SelectedIndexChanged((object) this, e);
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
      return new Size(proposedSize.Width, this.cboAutoHeight.Height);
    }

    public void SetPreferredSize(Size proposedSize) => this.customSize = proposedSize;

    protected override void OnResize(EventArgs e)
    {
      Size customSize = this.customSize;
      if (this.customSize.Height > 0)
        this.Height = this.customSize.Height;
      else if (this.Height != this.cboAutoHeight.Height)
        this.Height = this.cboAutoHeight.Height;
      this.cboMilestones.ItemHeight = this.Height - 6;
      base.OnResize(e);
    }

    private void MilestoneDropdownBox_Load(object sender, EventArgs e)
    {
      Size customSize = this.customSize;
      this.Height = this.customSize.Height;
      this.cboAutoHeight.Visible = false;
    }

    public void ResizeWidth(int Width) => this.Width = Width;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cboMilestones = new ComboBoxEx();
      this.cboAutoHeight = new ComboBox();
      this.SuspendLayout();
      this.cboMilestones.Dock = DockStyle.Top;
      this.cboMilestones.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboMilestones.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboMilestones.FormattingEnabled = true;
      this.cboMilestones.ItemHeight = 16;
      this.cboMilestones.Location = new Point(0, 0);
      this.cboMilestones.Name = "cboMilestones";
      this.cboMilestones.SelectedBGColor = SystemColors.Highlight;
      this.cboMilestones.Size = new Size(146, 22);
      this.cboMilestones.TabIndex = 0;
      this.cboMilestones.SelectedIndexChanged += new EventHandler(this.cboMilestones_SelectedIndexChanged);
      this.cboAutoHeight.FormattingEnabled = true;
      this.cboAutoHeight.Location = new Point(98, 26);
      this.cboAutoHeight.Name = "cboAutoHeight";
      this.cboAutoHeight.Size = new Size(10, 22);
      this.cboAutoHeight.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.cboAutoHeight);
      this.Controls.Add((Control) this.cboMilestones);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (MilestoneDropdownBox);
      this.Size = new Size(146, 27);
      this.Load += new EventHandler(this.MilestoneDropdownBox_Load);
      this.ResumeLayout(false);
    }
  }
}
