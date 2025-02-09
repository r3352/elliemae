// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard.CampaignStepsPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.Campaign;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard
{
  public class CampaignStepsPanel : CampaignWizardItem
  {
    private int stepNumber;
    private CampaignData campaignData;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private CampaignStep campaignStep;
    private ContextMenuStrip ctxSteps;
    private ToolStripMenuItem mnuAddStep;
    private ToolStripMenuItem mnuEditStep;
    private ToolStripMenuItem mnuRemoveStep;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem mnuMoveUp;
    private ToolStripMenuItem mnuMoveDown;
    private GroupContainer groupContainer1;
    private StandardIconButton btnRemoveStep;
    private StandardIconButton btnEditStep;
    private StandardIconButton btnAddStep;
    private StandardIconButton btnMoveDown;
    private StandardIconButton btnMoveUp;
    private ToolTip toolTip1;
    private ActivityTypeNameProvider activityNames = new ActivityTypeNameProvider();
    private IContainer components;
    private Label lblSeparator1;
    private FormattedLabel lblCampaignDuration;
    private FormattedLabel lblTotalSteps;
    private GridView gvSteps;

    public CampaignStepsPanel(int stepNumber)
    {
      this.stepNumber = stepNumber;
      this.InitializeComponent();
      this.campaignData = CampaignData.GetCampaignData();
      this.campaign = this.campaignData.WizardCampaign;
      this.populateControls();
    }

    protected void populateControls()
    {
      this.lblStep.Text = this.stepNumber.ToString() + ". Create Campaign Steps";
      this.populateListView(0);
      this.populateSummaryView();
    }

    private void populateListView(int selectedIndex)
    {
      this.gvSteps.BeginUpdate();
      this.gvSteps.Items.Clear();
      if (this.campaign.CampaignSteps != null && this.campaign.CampaignSteps.Count != 0)
      {
        for (int index = 0; index < this.campaign.CampaignSteps.Count; ++index)
          this.gvSteps.Items.Add(new GVItem(this.campaign.CampaignSteps[index].StepNumber.ToString())
          {
            SubItems = {
              (object) this.campaign.CampaignSteps[index].StepNumber.ToString(),
              (object) this.campaign.CampaignSteps[index].StepName,
              (object) this.campaign.CampaignSteps[index].StepDesc,
              (object) this.activityNames.GetName((object) this.campaign.CampaignSteps[index].ActivityType),
              (object) this.campaign.CampaignSteps[index].StepOffset.ToString()
            },
            Tag = (object) this.campaign.CampaignSteps[index]
          });
        if (0 < this.gvSteps.Items.Count)
        {
          if (selectedIndex < 0)
            selectedIndex = 0;
          if (selectedIndex >= this.gvSteps.Items.Count)
            selectedIndex = this.gvSteps.Items.Count - 1;
          this.gvSteps.Items[selectedIndex].Selected = true;
          this.gvSteps.EnsureVisible(selectedIndex);
        }
      }
      this.gvSteps.EndUpdate();
    }

    private void populateSummaryView()
    {
      this.lblTotalSteps.Text = "Total number of steps:  <b>" + this.campaign.CampaignStepCount.ToString() + "</b>";
      int num = 0;
      if (0 < this.campaign.CampaignStepCount)
      {
        foreach (CampaignStep campaignStep in (CollectionBase) this.campaign.CampaignSteps)
          num += campaignStep.StepOffset;
      }
      this.lblCampaignDuration.Text = "Campaign Duration:  <b>" + num.ToString() + " days</b>";
    }

    private bool processUserEntry()
    {
      if (this.campaign.IsValid)
        return true;
      this.DisplayBrokenRules((BusinessBase) this.campaign);
      return false;
    }

    private void moveUp()
    {
      int index = this.gvSteps.SelectedItems[0].Index;
      --((CampaignStep) this.gvSteps.Items[index].Tag).StepNumber;
      ++((CampaignStep) this.gvSteps.Items[index - 1].Tag).StepNumber;
      this.campaign.CampaignSteps.Sort("StepNumber", true);
      this.populateListView(index - 1);
    }

    private void moveDown()
    {
      int index = this.gvSteps.SelectedItems[0].Index;
      ++((CampaignStep) this.gvSteps.Items[index].Tag).StepNumber;
      --((CampaignStep) this.gvSteps.Items[index + 1].Tag).StepNumber;
      this.campaign.CampaignSteps.Sort("StepNumber", true);
      this.populateListView(index + 1);
    }

    private void addStep()
    {
      this.campaignStep = CampaignStep.NewCampaignStep(this.campaign, Session.SessionObjects);
      this.campaignStep.StepNumber = this.gvSteps.Items.Count + 1;
      if (DialogResult.OK == new CampaignStepDialog(CampaignStepsPanel.EditMode.AddMode, this.campaign, this.campaignStep).ShowDialog())
      {
        this.campaignStep.ApplyEdit();
        this.campaign.CampaignSteps.Add(this.campaignStep);
        this.populateListView(this.campaignStep.StepNumber - 1);
        this.populateSummaryView();
      }
      else
      {
        this.campaignStep.CancelEdit();
        this.campaignStep = this.gvSteps.Items.Count == 0 || this.gvSteps.SelectedItems.Count == 0 ? (CampaignStep) null : (CampaignStep) this.gvSteps.SelectedItems[0].Tag;
      }
    }

    private void editStep()
    {
      CampaignStep tag = (CampaignStep) this.gvSteps.Items[this.gvSteps.SelectedItems[0].Index].Tag;
      if (DialogResult.OK == new CampaignStepDialog(CampaignStepsPanel.EditMode.EditMode, this.campaign, tag).ShowDialog())
      {
        tag.ApplyEdit();
        this.populateListView(tag.StepNumber - 1);
        this.populateSummaryView();
      }
      else
        tag.CancelEdit();
    }

    private void removeStep()
    {
      if (DialogResult.Yes != Utils.Dialog((IWin32Window) this, "Are you sure you want to remove this step?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
        return;
      int index1 = this.gvSteps.SelectedItems[0].Index;
      CampaignStep tag = (CampaignStep) this.gvSteps.Items[index1].Tag;
      this.campaign.CampaignSteps.Remove(tag);
      for (int index2 = tag.StepNumber - 1; index2 < this.campaign.CampaignSteps.Count; ++index2)
        --this.campaign.CampaignSteps[index2].StepNumber;
      this.populateListView(index1);
      this.populateSummaryView();
      if (this.gvSteps.Items.Count != 0)
        return;
      this.gvSteps_SelectedIndexChanged((object) this, EventArgs.Empty);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public override bool NextEnabled => true;

    public override bool BackEnabled => true;

    public override bool CancelEnabled => true;

    public override WizardItem Next()
    {
      return !this.processUserEntry() ? (WizardItem) null : (WizardItem) new ManageContactsPanel(this.stepNumber + 1);
    }

    public override WizardItem Back()
    {
      if (this.campaign.CampaignStepCount == 0)
        this.campaign.CampaignSteps = (CampaignStepCollection) null;
      return !this.processUserEntry() ? (WizardItem) null : (WizardItem) new CampaignDetailsPanel(this.stepNumber - 1);
    }

    private void gvSteps_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvSteps.Items.Count == 0 || this.gvSteps.SelectedItems.Count == 0)
      {
        this.campaignStep = (CampaignStep) null;
        this.btnEditStep.Enabled = false;
        this.btnRemoveStep.Enabled = false;
        this.btnMoveUp.Enabled = false;
        this.btnMoveDown.Enabled = false;
      }
      else
      {
        this.campaignStep = (CampaignStep) this.gvSteps.SelectedItems[0].Tag;
        this.btnEditStep.Enabled = true;
        this.btnRemoveStep.Enabled = true;
        this.btnMoveUp.Enabled = this.gvSteps.SelectedItems[0].Index != 0;
        this.btnMoveDown.Enabled = this.gvSteps.Items.Count - 1 != this.gvSteps.SelectedItems[0].Index;
      }
    }

    private void gvSteps_DoubleClick(object sender, EventArgs e)
    {
      Point client = this.gvSteps.PointToClient(Control.MousePosition);
      GVItem itemAt = this.gvSteps.GetItemAt(client.X, client.Y);
      if (itemAt == null)
        return;
      itemAt.Selected = true;
      this.editStep();
    }

    private void btnMoveUp_Click(object sender, EventArgs e) => this.moveUp();

    private void btnMoveDown_Click(object sender, EventArgs e) => this.moveDown();

    private void btnMoveDown_EnabledChanged(object sender, EventArgs e)
    {
      if (this.btnMoveDown.Enabled || 1 >= this.gvSteps.Items.Count)
        return;
      this.btnMoveUp.Focus();
    }

    private void btnAddStep_Click(object sender, EventArgs e) => this.addStep();

    private void btnEditStep_Click(object sender, EventArgs e) => this.editStep();

    private void btnRemoveStep_Click(object sender, EventArgs e) => this.removeStep();

    private void ctxSteps_Opening(object sender, CancelEventArgs e)
    {
      this.mnuAddStep.Enabled = this.btnAddStep.Enabled;
      this.mnuEditStep.Enabled = this.btnEditStep.Enabled;
      this.mnuRemoveStep.Enabled = this.btnRemoveStep.Enabled;
      this.mnuMoveUp.Enabled = this.btnMoveUp.Enabled;
      this.mnuMoveDown.Enabled = this.btnMoveDown.Enabled;
    }

    private void mnuAddStep_Click(object sender, EventArgs e) => this.addStep();

    private void mnuEditStep_Click(object sender, EventArgs e) => this.editStep();

    private void mnuRemoveStep_Click(object sender, EventArgs e) => this.removeStep();

    private void mnuMoveUp_Click(object sender, EventArgs e) => this.moveUp();

    private void mnuMoveDown_Click(object sender, EventArgs e) => this.moveDown();

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.lblSeparator1 = new Label();
      this.lblCampaignDuration = new FormattedLabel();
      this.lblTotalSteps = new FormattedLabel();
      this.gvSteps = new GridView();
      this.ctxSteps = new ContextMenuStrip(this.components);
      this.mnuAddStep = new ToolStripMenuItem();
      this.mnuEditStep = new ToolStripMenuItem();
      this.mnuRemoveStep = new ToolStripMenuItem();
      this.toolStripMenuItem2 = new ToolStripSeparator();
      this.mnuMoveUp = new ToolStripMenuItem();
      this.mnuMoveDown = new ToolStripMenuItem();
      this.groupContainer1 = new GroupContainer();
      this.btnMoveDown = new StandardIconButton();
      this.btnMoveUp = new StandardIconButton();
      this.btnRemoveStep = new StandardIconButton();
      this.btnEditStep = new StandardIconButton();
      this.btnAddStep = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.pnlStepTitle.SuspendLayout();
      this.pnlMainContent.SuspendLayout();
      this.ctxSteps.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnMoveDown).BeginInit();
      ((ISupportInitialize) this.btnMoveUp).BeginInit();
      ((ISupportInitialize) this.btnRemoveStep).BeginInit();
      ((ISupportInitialize) this.btnEditStep).BeginInit();
      ((ISupportInitialize) this.btnAddStep).BeginInit();
      this.SuspendLayout();
      this.pnlStepTitle.TabIndex = 0;
      this.pnlMainContent.Controls.Add((Control) this.groupContainer1);
      this.pnlMainContent.Controls.Add((Control) this.lblCampaignDuration);
      this.pnlMainContent.Controls.Add((Control) this.lblTotalSteps);
      this.pnlMainContent.TabIndex = 0;
      this.lblStep.Text = "2. Create Campaign Steps";
      this.lblSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSeparator1.BorderStyle = BorderStyle.FixedSingle;
      this.lblSeparator1.Location = new Point(8, 528);
      this.lblSeparator1.Name = "lblSeparator1";
      this.lblSeparator1.Size = new Size(657, 1);
      this.lblSeparator1.TabIndex = 2;
      this.lblSeparator1.Text = "label1";
      this.lblCampaignDuration.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblCampaignDuration.AutoSize = false;
      this.lblCampaignDuration.Location = new Point(174, 395);
      this.lblCampaignDuration.Name = "lblCampaignDuration";
      this.lblCampaignDuration.Size = new Size(149, 16);
      this.lblCampaignDuration.TabIndex = 5;
      this.lblCampaignDuration.Text = "Campaign Duration:  <b>99 Days</b>";
      this.lblTotalSteps.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblTotalSteps.AutoSize = false;
      this.lblTotalSteps.Location = new Point(25, 395);
      this.lblTotalSteps.Name = "lblTotalSteps";
      this.lblTotalSteps.Size = new Size(143, 16);
      this.lblTotalSteps.TabIndex = 4;
      this.lblTotalSteps.Text = "Total number of steps:  <b>999</b>";
      this.gvSteps.AllowMultiselect = false;
      this.gvSteps.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "gvcStepId";
      gvColumn1.Text = "StepId";
      gvColumn1.Width = 0;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "gvcStepNumber";
      gvColumn2.Text = "Step";
      gvColumn2.Width = 37;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "gvcName";
      gvColumn3.Text = "Name";
      gvColumn3.Width = 174;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "gvcDescription";
      gvColumn4.Text = "Description";
      gvColumn4.Width = 175;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "gvcActivityType";
      gvColumn5.Text = "Task Type";
      gvColumn5.Width = 76;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "gvcInterval";
      gvColumn6.Text = "Interval";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 58;
      this.gvSteps.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvSteps.ContextMenuStrip = this.ctxSteps;
      this.gvSteps.Dock = DockStyle.Fill;
      this.gvSteps.Location = new Point(1, 26);
      this.gvSteps.Name = "gvSteps";
      this.gvSteps.Size = new Size(652, 275);
      this.gvSteps.SortOption = GVSortOption.None;
      this.gvSteps.TabIndex = 6;
      this.gvSteps.SelectedIndexChanged += new EventHandler(this.gvSteps_SelectedIndexChanged);
      this.gvSteps.DoubleClick += new EventHandler(this.gvSteps_DoubleClick);
      this.ctxSteps.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.mnuAddStep,
        (ToolStripItem) this.mnuEditStep,
        (ToolStripItem) this.mnuRemoveStep,
        (ToolStripItem) this.toolStripMenuItem2,
        (ToolStripItem) this.mnuMoveUp,
        (ToolStripItem) this.mnuMoveDown
      });
      this.ctxSteps.Name = "ctxSteps";
      this.ctxSteps.Size = new Size(142, 120);
      this.ctxSteps.Opening += new CancelEventHandler(this.ctxSteps_Opening);
      this.mnuAddStep.Name = "mnuAddStep";
      this.mnuAddStep.Size = new Size(141, 22);
      this.mnuAddStep.Text = "Add Step";
      this.mnuAddStep.Click += new EventHandler(this.mnuAddStep_Click);
      this.mnuEditStep.Name = "mnuEditStep";
      this.mnuEditStep.Size = new Size(141, 22);
      this.mnuEditStep.Text = "Edit";
      this.mnuEditStep.Click += new EventHandler(this.mnuEditStep_Click);
      this.mnuRemoveStep.Name = "mnuRemoveStep";
      this.mnuRemoveStep.Size = new Size(141, 22);
      this.mnuRemoveStep.Text = "Remove";
      this.mnuRemoveStep.Click += new EventHandler(this.mnuRemoveStep_Click);
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new Size(138, 6);
      this.mnuMoveUp.Name = "mnuMoveUp";
      this.mnuMoveUp.Size = new Size(141, 22);
      this.mnuMoveUp.Text = "Move Up";
      this.mnuMoveUp.Click += new EventHandler(this.mnuMoveUp_Click);
      this.mnuMoveDown.Name = "mnuMoveDown";
      this.mnuMoveDown.Size = new Size(141, 22);
      this.mnuMoveDown.Text = "Move Down";
      this.mnuMoveDown.Click += new EventHandler(this.mnuMoveDown_Click);
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.btnMoveDown);
      this.groupContainer1.Controls.Add((Control) this.btnMoveUp);
      this.groupContainer1.Controls.Add((Control) this.btnRemoveStep);
      this.groupContainer1.Controls.Add((Control) this.btnEditStep);
      this.groupContainer1.Controls.Add((Control) this.btnAddStep);
      this.groupContainer1.Controls.Add((Control) this.gvSteps);
      this.groupContainer1.Location = new Point(7, 22);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(654, 302);
      this.groupContainer1.TabIndex = 7;
      this.groupContainer1.Text = "A minimum of one step is required";
      this.btnMoveDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveDown.BackColor = Color.Transparent;
      this.btnMoveDown.Enabled = false;
      this.btnMoveDown.Location = new Point(633, 4);
      this.btnMoveDown.Name = "btnMoveDown";
      this.btnMoveDown.Size = new Size(16, 16);
      this.btnMoveDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveDown.TabIndex = 11;
      this.btnMoveDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveDown, "Move Down");
      this.btnMoveDown.Click += new EventHandler(this.btnMoveDown_Click);
      this.btnMoveUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveUp.BackColor = Color.Transparent;
      this.btnMoveUp.Enabled = false;
      this.btnMoveUp.Location = new Point(612, 4);
      this.btnMoveUp.Name = "btnMoveUp";
      this.btnMoveUp.Size = new Size(16, 16);
      this.btnMoveUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveUp.TabIndex = 10;
      this.btnMoveUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveUp, "Move Up");
      this.btnMoveUp.Click += new EventHandler(this.btnMoveUp_Click);
      this.btnRemoveStep.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemoveStep.BackColor = Color.Transparent;
      this.btnRemoveStep.Enabled = false;
      this.btnRemoveStep.Location = new Point(591, 4);
      this.btnRemoveStep.Name = "btnRemoveStep";
      this.btnRemoveStep.Size = new Size(16, 16);
      this.btnRemoveStep.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveStep.TabIndex = 9;
      this.btnRemoveStep.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveStep, "Delete Step");
      this.btnRemoveStep.Click += new EventHandler(this.btnRemoveStep_Click);
      this.btnEditStep.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditStep.BackColor = Color.Transparent;
      this.btnEditStep.Enabled = false;
      this.btnEditStep.Location = new Point(570, 4);
      this.btnEditStep.Name = "btnEditStep";
      this.btnEditStep.Size = new Size(16, 16);
      this.btnEditStep.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditStep.TabIndex = 8;
      this.btnEditStep.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditStep, "Edit Step");
      this.btnEditStep.Click += new EventHandler(this.btnEditStep_Click);
      this.btnAddStep.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddStep.BackColor = Color.Transparent;
      this.btnAddStep.Location = new Point(549, 4);
      this.btnAddStep.Name = "btnAddStep";
      this.btnAddStep.Size = new Size(16, 16);
      this.btnAddStep.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddStep.TabIndex = 7;
      this.btnAddStep.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddStep, "New Step");
      this.btnAddStep.Click += new EventHandler(this.btnAddStep_Click);
      this.Controls.Add((Control) this.lblSeparator1);
      this.Location = new Point(0, 0);
      this.Name = nameof (CampaignStepsPanel);
      this.Controls.SetChildIndex((Control) this.pnlStepTitle, 0);
      this.Controls.SetChildIndex((Control) this.pnlMainContent, 0);
      this.Controls.SetChildIndex((Control) this.lblSeparator1, 0);
      this.pnlStepTitle.ResumeLayout(false);
      this.pnlMainContent.ResumeLayout(false);
      this.ctxSteps.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btnMoveDown).EndInit();
      ((ISupportInitialize) this.btnMoveUp).EndInit();
      ((ISupportInitialize) this.btnRemoveStep).EndInit();
      ((ISupportInitialize) this.btnEditStep).EndInit();
      ((ISupportInitialize) this.btnAddStep).EndInit();
      this.ResumeLayout(false);
    }

    public enum EditMode
    {
      None,
      AddMode,
      EditMode,
    }
  }
}
