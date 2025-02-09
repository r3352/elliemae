// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BorrowerContactUpdateDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BorrowerContactUpdateDialog : SettingsUserControl
  {
    private GVItem currItem;
    private bool suspendItemCheckedEventHandler;
    private IContainer components;
    private GridView lsvMilestone;
    private GroupContainer groupContainer1;

    public BorrowerContactUpdateDialog(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.initPageSetting();
      this.setDirtyFlag(false);
    }

    private void initPageSetting()
    {
      this.suspendItemCheckedEventHandler = true;
      try
      {
        this.lsvMilestone.Items.Clear();
        IEnumerable<Milestone> activeMilestonesList = ((MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllActiveMilestonesList();
        string str = string.Concat(Session.ServerManager.GetServerSetting("Policies.ContactUpdateMilestone"));
        foreach (Milestone milestone in activeMilestonesList)
        {
          if (milestone.Name != null && !(milestone.Name == "Started"))
          {
            GVItem gvItem = new GVItem((object) new MilestoneLabel(milestone));
            if (milestone.MilestoneID == str)
            {
              gvItem.Checked = true;
              this.currItem = gvItem;
            }
            else
              gvItem.Checked = false;
            gvItem.Tag = (object) milestone;
            this.lsvMilestone.Items.Add(gvItem);
          }
        }
      }
      finally
      {
        this.suspendItemCheckedEventHandler = false;
      }
    }

    public override void Save()
    {
      GVItem gvItem1 = (GVItem) null;
      foreach (GVItem gvItem2 in (IEnumerable<GVItem>) this.lsvMilestone.Items)
      {
        if (gvItem2.Checked)
        {
          gvItem1 = gvItem2;
          break;
        }
      }
      if (gvItem1 != null)
      {
        this.currItem = gvItem1;
        Session.ServerManager.UpdateServerSetting("Policies.ContactUpdateMilestone", (object) ((Milestone) this.currItem.Tag).MilestoneID);
      }
      this.setDirtyFlag(false);
    }

    public override void Reset()
    {
      this.initPageSetting();
      this.setDirtyFlag(false);
    }

    private void lsvMilestone_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.suspendItemCheckedEventHandler)
        return;
      this.suspendItemCheckedEventHandler = true;
      try
      {
        if (e.SubItem.Checked)
        {
          if (e.SubItem.Item != this.currItem)
            this.setDirtyFlag(true);
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lsvMilestone.Items)
          {
            if (gvItem != e.SubItem.Item)
              gvItem.Checked = false;
          }
        }
        else
          e.SubItem.Checked = true;
      }
      finally
      {
        this.suspendItemCheckedEventHandler = false;
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
      this.lsvMilestone = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.lsvMilestone.AllowMultiselect = false;
      this.lsvMilestone.BorderStyle = BorderStyle.None;
      gvColumn.CheckBoxes = true;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Milestone";
      gvColumn.Width = 300;
      this.lsvMilestone.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.lsvMilestone.Dock = DockStyle.Fill;
      this.lsvMilestone.Location = new Point(1, 26);
      this.lsvMilestone.Name = "lsvMilestone";
      this.lsvMilestone.Size = new Size(457, 331);
      this.lsvMilestone.SortOption = GVSortOption.None;
      this.lsvMilestone.TabIndex = 3;
      this.lsvMilestone.SubItemCheck += new GVSubItemEventHandler(this.lsvMilestone_SubItemCheck);
      this.groupContainer1.Controls.Add((Control) this.lsvMilestone);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(459, 358);
      this.groupContainer1.TabIndex = 6;
      this.groupContainer1.Text = "Select a Milestone";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (BorrowerContactUpdateDialog);
      this.Size = new Size(459, 358);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
