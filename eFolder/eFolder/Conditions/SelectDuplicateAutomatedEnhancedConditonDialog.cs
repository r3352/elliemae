// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.SelectDuplicateAutomatedEnhancedConditonDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class SelectDuplicateAutomatedEnhancedConditonDialog : Form
  {
    private const string className = "SelectDuplicateAutomatedEnhancedConditonDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private GridViewDataManager gvTemplatesMgr;
    private List<EnhancedConditionTemplate> templatesToAddList = new List<EnhancedConditionTemplate>();
    private IContainer components;
    private EMHelpLink helpLink;
    private GradientPanel gradStackingOrder;
    private Panel pnlClose;
    private Button btnCancel;
    private EMHelpLink emHelpLink1;
    private Button btnAdd;
    private GroupContainer gcConditions;
    private Label lblConditionsAddedSummary;
    private GridView gvAutomatedEnhancedConditions;
    private Label label1;

    public SelectDuplicateAutomatedEnhancedConditonDialog(
      LoanDataMgr loanDataMgr,
      EnhancedConditionTemplate[] conditionTemplates,
      int autoAddedConditionsCount)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.initForm(loanDataMgr, conditionTemplates, autoAddedConditionsCount);
    }

    public EnhancedConditionTemplate[] TemplatesToAdd => this.templatesToAddList.ToArray();

    public string PairId => "All";

    private void initForm(
      LoanDataMgr loanDataMgr,
      EnhancedConditionTemplate[] conditionTemplates,
      int autoAddedConditionsCount)
    {
      this.loanDataMgr = loanDataMgr;
      this.gvTemplatesMgr = new GridViewDataManager(this.gvAutomatedEnhancedConditions, this.loanDataMgr);
      this.initlblConditionsAddedSummary(autoAddedConditionsCount);
      this.loadEnhancedConditionTemplatesList(loanDataMgr, conditionTemplates);
      this.setButtons();
    }

    private void initlblConditionsAddedSummary(int autoAddedConditionsCount)
    {
      this.lblConditionsAddedSummary.Text = this.lblConditionsAddedSummary.Text.Replace("{}", autoAddedConditionsCount.ToString());
    }

    private void loadEnhancedConditionTemplatesList(
      LoanDataMgr loanDataMgr,
      EnhancedConditionTemplate[] conditionTemplates)
    {
      this.gvTemplatesMgr.ClearItems();
      foreach (EnhancedConditionTemplate conditionTemplate in conditionTemplates)
        this.gvTemplatesMgr.AddItem(conditionTemplate);
      this.gvAutomatedEnhancedConditions.ReSort();
    }

    private EnhancedConditionTemplate[] getSelectedTemplates()
    {
      List<EnhancedConditionTemplate> conditionTemplateList = new List<EnhancedConditionTemplate>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAutomatedEnhancedConditions.Items)
      {
        EnhancedConditionTemplate tag = (EnhancedConditionTemplate) gvItem.Tag;
        if (gvItem.Checked && tag != null)
          conditionTemplateList.Add(tag);
      }
      return conditionTemplateList.ToArray();
    }

    private void gvConditionTemplates_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.setButtons();
    }

    private void setButtons() => this.btnAdd.Enabled = this.getSelectedTemplates().Length > 0;

    private void btnAdd_Click(object sender, EventArgs e)
    {
      foreach (EnhancedConditionTemplate selectedTemplate in this.getSelectedTemplates())
        this.templatesToAddList.Add(selectedTemplate);
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void AddEnhancedConditionsFromListDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
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
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.helpLink = new EMHelpLink();
      this.gradStackingOrder = new GradientPanel();
      this.label1 = new Label();
      this.lblConditionsAddedSummary = new Label();
      this.pnlClose = new Panel();
      this.btnCancel = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.btnAdd = new Button();
      this.gcConditions = new GroupContainer();
      this.gvAutomatedEnhancedConditions = new GridView();
      this.gradStackingOrder.SuspendLayout();
      this.pnlClose.SuspendLayout();
      this.gcConditions.SuspendLayout();
      this.SuspendLayout();
      this.helpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Send Files";
      this.helpLink.Location = new Point(12, 501);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 19);
      this.helpLink.TabIndex = 17;
      this.helpLink.TabStop = false;
      this.gradStackingOrder.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradStackingOrder.Controls.Add((Control) this.label1);
      this.gradStackingOrder.Controls.Add((Control) this.lblConditionsAddedSummary);
      this.gradStackingOrder.Dock = DockStyle.Top;
      this.gradStackingOrder.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradStackingOrder.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradStackingOrder.Location = new Point(0, 0);
      this.gradStackingOrder.Name = "gradStackingOrder";
      this.gradStackingOrder.Padding = new Padding(8, 0, 0, 0);
      this.gradStackingOrder.Size = new Size(866, 66);
      this.gradStackingOrder.TabIndex = 28;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 49);
      this.label1.Name = "label1";
      this.label1.Size = new Size(495, 14);
      this.label1.TabIndex = 16;
      this.label1.Text = "These conditions were previously added to the loan. Select the conditions that you want to add again";
      this.lblConditionsAddedSummary.AutoSize = true;
      this.lblConditionsAddedSummary.Font = new Font("Arial", 8.5f);
      this.lblConditionsAddedSummary.Location = new Point(9, 9);
      this.lblConditionsAddedSummary.Name = "lblConditionsAddedSummary";
      this.lblConditionsAddedSummary.Size = new Size(226, 15);
      this.lblConditionsAddedSummary.TabIndex = 15;
      this.lblConditionsAddedSummary.Text = "{} condition(s) sucessfully added to loan";
      this.pnlClose.Controls.Add((Control) this.btnCancel);
      this.pnlClose.Controls.Add((Control) this.emHelpLink1);
      this.pnlClose.Controls.Add((Control) this.btnAdd);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 441);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(866, 52);
      this.pnlClose.TabIndex = 60;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(779, 16);
      this.btnCancel.Margin = new Padding(1, 0, 0, 0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 26;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Send Files";
      this.emHelpLink1.Location = new Point(12, 16);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 19);
      this.emHelpLink1.TabIndex = 27;
      this.emHelpLink1.TabStop = false;
      this.emHelpLink1.Visible = false;
      this.btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAdd.DialogResult = DialogResult.OK;
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(700, 16);
      this.btnAdd.Margin = new Padding(1, 0, 0, 0);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 24);
      this.btnAdd.TabIndex = 25;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gcConditions.Controls.Add((Control) this.gvAutomatedEnhancedConditions);
      this.gcConditions.Dock = DockStyle.Fill;
      this.gcConditions.HeaderForeColor = SystemColors.ControlText;
      this.gcConditions.Location = new Point(0, 66);
      this.gcConditions.Name = "gcConditions";
      this.gcConditions.Size = new Size(866, 375);
      this.gcConditions.TabIndex = 61;
      this.gcConditions.Text = "Previously Added Conditions";
      this.gvAutomatedEnhancedConditions.BorderStyle = BorderStyle.None;
      this.gvAutomatedEnhancedConditions.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ColumnHeaderCheckbox = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "SelectTemplate";
      gvColumn1.SortMethod = GVSortMethod.Checkbox;
      gvColumn1.Tag = (object) "SelectTemplate";
      gvColumn1.Text = "";
      gvColumn1.Width = 50;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "InternalId";
      gvColumn2.Tag = (object) "InternalId";
      gvColumn2.Text = "Internal ID";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Name";
      gvColumn3.Tag = (object) "NAME";
      gvColumn3.Text = "Condition Name";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ExternalDescription";
      gvColumn4.Tag = (object) "EXTERNALDESCRIPTION";
      gvColumn4.Text = "External Description";
      gvColumn4.Width = 200;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Type";
      gvColumn5.Tag = (object) "TYPE";
      gvColumn5.Text = "Condition Type";
      gvColumn5.Width = 150;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Category";
      gvColumn6.Tag = (object) "CATEGORY";
      gvColumn6.Text = "Category";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "PriorTo";
      gvColumn7.Tag = (object) "PRIORTO";
      gvColumn7.Text = "Prior To";
      gvColumn7.Width = 100;
      this.gvAutomatedEnhancedConditions.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gvAutomatedEnhancedConditions.Dock = DockStyle.Fill;
      this.gvAutomatedEnhancedConditions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAutomatedEnhancedConditions.Location = new Point(1, 26);
      this.gvAutomatedEnhancedConditions.Name = "gvAutomatedEnhancedConditions";
      this.gvAutomatedEnhancedConditions.Size = new Size(864, 348);
      this.gvAutomatedEnhancedConditions.TabIndex = 2;
      this.gvAutomatedEnhancedConditions.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvAutomatedEnhancedConditions.SubItemCheck += new GVSubItemEventHandler(this.gvConditionTemplates_SubItemCheck);
      this.AcceptButton = (IButtonControl) this.btnAdd;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(866, 493);
      this.Controls.Add((Control) this.gcConditions);
      this.Controls.Add((Control) this.pnlClose);
      this.Controls.Add((Control) this.gradStackingOrder);
      this.Controls.Add((Control) this.helpLink);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = Resources.icon_allsizes_bug;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectDuplicateAutomatedEnhancedConditonDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add from Automated Conditions";
      this.KeyDown += new KeyEventHandler(this.AddEnhancedConditionsFromListDialog_KeyDown);
      this.gradStackingOrder.ResumeLayout(false);
      this.gradStackingOrder.PerformLayout();
      this.pnlClose.ResumeLayout(false);
      this.gcConditions.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
