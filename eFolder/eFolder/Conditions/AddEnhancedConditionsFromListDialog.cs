// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.AddEnhancedConditionsFromListDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
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
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class AddEnhancedConditionsFromListDialog : Form
  {
    private const string className = "AddEnhancedConditionsFromListDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private eFolderManager eFolderMgr;
    private eFolderAccessRights rights;
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
    private ComboBox cboBorrower;
    private Label lblBorrower;
    private GridView gvConditionTemplates;

    public AddEnhancedConditionsFromListDialog(
      LoanDataMgr loanDataMgr,
      EnhancedConditionTemplate[] conditionTemplates,
      EnhancedConditionType[] condTypes)
    {
      this.InitializeComponent();
      this.initForm(loanDataMgr, conditionTemplates, condTypes);
    }

    public EnhancedConditionTemplate[] TemplatesToAdd => this.templatesToAddList.ToArray();

    public string PairId => ((BorrowerPair) this.cboBorrower.SelectedItem).Id;

    private void initForm(
      LoanDataMgr loanDataMgr,
      EnhancedConditionTemplate[] conditionTemplates,
      EnhancedConditionType[] condTypes)
    {
      this.loanDataMgr = loanDataMgr;
      this.eFolderMgr = new eFolderManager();
      this.rights = new eFolderAccessRights(this.loanDataMgr);
      this.initAvailableList();
      this.initBorrowerField();
      this.loadConditionTemplatesList(conditionTemplates, condTypes);
      this.setButtons();
    }

    private void initAvailableList()
    {
      this.gvTemplatesMgr = new GridViewDataManager(this.gvConditionTemplates, this.loanDataMgr);
      this.gvTemplatesMgr.CreateLayout(new TableLayout.Column[7]
      {
        new TableLayout.Column("selected", "", "", "", HorizontalAlignment.Left, 25, true),
        GridViewDataManager.InternalIdColumn,
        GridViewDataManager.CondNameColumn,
        GridViewDataManager.InternalDescriptionColumn,
        GridViewDataManager.ExternalDescriptionColumn,
        GridViewDataManager.ConditionTypeColumn,
        GridViewDataManager.CategoryColumn
      });
      this.gvConditionTemplates.Columns[0].CheckBoxes = true;
      this.gvConditionTemplates.Columns[0].ColumnHeaderCheckbox = true;
      this.gvConditionTemplates.Columns[0].FilterControl.Enabled = false;
      this.gvConditionTemplates.Columns[0].SortMethod = GVSortMethod.Checkbox;
      this.gvConditionTemplates.Columns[0].Width = 50;
      this.gvConditionTemplates.Columns[1].Width = 100;
      this.gvConditionTemplates.Columns[1].Tag = (object) GridViewDataManager.InternalIdColumn.ColumnID;
      this.gvConditionTemplates.Columns[2].Width = 300;
      this.gvConditionTemplates.Columns[2].Tag = (object) GridViewDataManager.CondNameColumn.ColumnID;
      this.gvConditionTemplates.Columns[3].Width = 200;
      this.gvConditionTemplates.Columns[3].Tag = (object) GridViewDataManager.InternalDescriptionColumn.ColumnID;
      this.gvConditionTemplates.Columns[4].Width = 200;
      this.gvConditionTemplates.Columns[4].Tag = (object) GridViewDataManager.ExternalDescriptionColumn.ColumnID;
      this.gvConditionTemplates.Columns[5].Width = 150;
      this.gvConditionTemplates.Columns[5].Tag = (object) GridViewDataManager.ConditionTypeColumn.ColumnID;
      this.gvConditionTemplates.Columns[6].Width = 100;
      this.gvConditionTemplates.Columns[6].Tag = (object) GridViewDataManager.CategoryColumn.ColumnID;
    }

    private void initBorrowerField()
    {
      this.cboBorrower.Items.AddRange((object[]) this.loanDataMgr.LoanData.GetBorrowerPairs());
      this.cboBorrower.Items.Add((object) BorrowerPair.All);
      if (this.loanDataMgr.LoanData.CurrentBorrowerPair == null)
        return;
      this.cboBorrower.SelectedItem = (object) this.loanDataMgr.LoanData.CurrentBorrowerPair;
    }

    private void loadConditionTemplatesList(
      EnhancedConditionTemplate[] conditionTemplates,
      EnhancedConditionType[] condTypes)
    {
      this.gvTemplatesMgr.ClearItems();
      foreach (EnhancedConditionTemplate conditionTemplate in conditionTemplates)
      {
        if (this.rights.CanAddEnhancedCondition(conditionTemplate.ConditionType))
        {
          foreach (EnhancedConditionType condType in condTypes)
          {
            if (this.isTemplateActive(conditionTemplate) && condType.title == conditionTemplate.ConditionType && this.allowedOnLoan(conditionTemplate))
              this.gvTemplatesMgr.AddItem(conditionTemplate);
          }
        }
      }
      this.gvTemplatesMgr.ApplyEmptyFilters = false;
      this.gvTemplatesMgr.ApplyFilter();
      this.adjustInternalIdFilter();
      this.adjustConditionTypeFilter();
      this.adjustCategoryFilter();
      this.gvConditionTemplates.ReSort();
    }

    private void adjustInternalIdFilter()
    {
      GVColumn gvColumn = this.gvConditionTemplates.Columns.Where<GVColumn>((Func<GVColumn, bool>) (x => x.Text == GridViewDataManager.InternalIdColumn.Title)).DefaultIfEmpty<GVColumn>((GVColumn) null).ToList<GVColumn>()[0];
      if (gvColumn == null)
        return;
      ((ComboBox) gvColumn.FilterControl).Items.Clear();
      ((ComboBox) gvColumn.FilterControl).Items.Add((object) string.Empty);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvConditionTemplates.Items)
      {
        EnhancedConditionTemplate tag = (EnhancedConditionTemplate) gvItem.Tag;
        if (!string.IsNullOrEmpty(tag.InternalId))
        {
          FieldOption fieldOption = new FieldOption(tag.InternalId, tag.InternalId);
          if (!((ComboBox) gvColumn.FilterControl).Items.Contains((object) fieldOption))
            ((ComboBox) gvColumn.FilterControl).Items.Add((object) fieldOption);
        }
      }
      ((ComboBox) gvColumn.FilterControl).Sorted = true;
    }

    private void adjustConditionTypeFilter()
    {
      GVColumn gvColumn = this.gvConditionTemplates.Columns.Where<GVColumn>((Func<GVColumn, bool>) (x => x.Text == GridViewDataManager.ConditionTypeColumn.Title)).DefaultIfEmpty<GVColumn>((GVColumn) null).ToList<GVColumn>()[0];
      if (gvColumn == null)
        return;
      ((ComboBox) gvColumn.FilterControl).Items.Clear();
      ((ComboBox) gvColumn.FilterControl).Items.Add((object) string.Empty);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvConditionTemplates.Items)
      {
        EnhancedConditionTemplate tag = (EnhancedConditionTemplate) gvItem.Tag;
        if (!string.IsNullOrEmpty(tag.ConditionType))
        {
          FieldOption fieldOption = new FieldOption(tag.ConditionType, tag.ConditionType);
          if (!((ComboBox) gvColumn.FilterControl).Items.Contains((object) fieldOption))
            ((ComboBox) gvColumn.FilterControl).Items.Add((object) fieldOption);
        }
      }
      ((ComboBox) gvColumn.FilterControl).Sorted = true;
    }

    private void adjustCategoryFilter()
    {
      GVColumn gvColumn = this.gvConditionTemplates.Columns.Where<GVColumn>((Func<GVColumn, bool>) (x => x.Text == GridViewDataManager.CategoryColumn.Title)).DefaultIfEmpty<GVColumn>((GVColumn) null).ToList<GVColumn>()[0];
      if (gvColumn == null)
        return;
      ((ComboBox) gvColumn.FilterControl).Items.Clear();
      ((ComboBox) gvColumn.FilterControl).Items.Add((object) string.Empty);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvConditionTemplates.Items)
      {
        EnhancedConditionTemplate tag = (EnhancedConditionTemplate) gvItem.Tag;
        if (!string.IsNullOrEmpty(tag.Category))
        {
          FieldOption fieldOption = new FieldOption(tag.Category, tag.Category);
          if (!((ComboBox) gvColumn.FilterControl).Items.Contains((object) fieldOption))
            ((ComboBox) gvColumn.FilterControl).Items.Add((object) fieldOption);
        }
      }
      ((ComboBox) gvColumn.FilterControl).Sorted = true;
    }

    private bool isTemplateActive(EnhancedConditionTemplate template)
    {
      return this.eFolderMgr.IsEnhancedConditionTemplateActive(template);
    }

    private EnhancedConditionTemplate[] getSelectedTemplates()
    {
      List<EnhancedConditionTemplate> conditionTemplateList = new List<EnhancedConditionTemplate>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvConditionTemplates.Items)
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

    private bool allowedOnLoan(EnhancedConditionTemplate template)
    {
      return this.eFolderMgr.IsEnhancedConditionAllowedOnLoan(this.loanDataMgr, template);
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
      this.gradStackingOrder = new GradientPanel();
      this.lblBorrower = new Label();
      this.cboBorrower = new ComboBox();
      this.pnlClose = new Panel();
      this.btnCancel = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.btnAdd = new Button();
      this.gcConditions = new GroupContainer();
      this.gvConditionTemplates = new GridView();
      this.helpLink = new EMHelpLink();
      this.gradStackingOrder.SuspendLayout();
      this.pnlClose.SuspendLayout();
      this.gcConditions.SuspendLayout();
      this.SuspendLayout();
      this.gradStackingOrder.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradStackingOrder.Controls.Add((Control) this.lblBorrower);
      this.gradStackingOrder.Controls.Add((Control) this.cboBorrower);
      this.gradStackingOrder.Dock = DockStyle.Top;
      this.gradStackingOrder.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradStackingOrder.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradStackingOrder.Location = new Point(0, 0);
      this.gradStackingOrder.Name = "gradStackingOrder";
      this.gradStackingOrder.Padding = new Padding(8, 0, 0, 0);
      this.gradStackingOrder.Size = new Size(1150, 52);
      this.gradStackingOrder.TabIndex = 28;
      this.lblBorrower.AutoSize = true;
      this.lblBorrower.Location = new Point(12, 12);
      this.lblBorrower.Name = "lblBorrower";
      this.lblBorrower.Size = new Size(126, 14);
      this.lblBorrower.TabIndex = 15;
      this.lblBorrower.Text = "Assign To Borrower Pair";
      this.cboBorrower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrower.FormattingEnabled = true;
      this.cboBorrower.Location = new Point(144, 12);
      this.cboBorrower.Name = "cboBorrower";
      this.cboBorrower.Size = new Size(582, 22);
      this.cboBorrower.TabIndex = 12;
      this.pnlClose.Controls.Add((Control) this.btnCancel);
      this.pnlClose.Controls.Add((Control) this.emHelpLink1);
      this.pnlClose.Controls.Add((Control) this.btnAdd);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 441);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(1150, 52);
      this.pnlClose.TabIndex = 60;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(1063, 16);
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
      this.btnAdd.Location = new Point(984, 16);
      this.btnAdd.Margin = new Padding(1, 0, 0, 0);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 24);
      this.btnAdd.TabIndex = 25;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gcConditions.Controls.Add((Control) this.gvConditionTemplates);
      this.gcConditions.Dock = DockStyle.Fill;
      this.gcConditions.HeaderForeColor = SystemColors.ControlText;
      this.gcConditions.Location = new Point(0, 52);
      this.gcConditions.Name = "gcConditions";
      this.gcConditions.Size = new Size(1150, 389);
      this.gcConditions.TabIndex = 61;
      this.gcConditions.Text = "Conditions";
      this.gvConditionTemplates.BorderStyle = BorderStyle.None;
      this.gvConditionTemplates.ClearSelectionsOnEmptyRowClick = false;
      this.gvConditionTemplates.Dock = DockStyle.Fill;
      this.gvConditionTemplates.FilterVisible = true;
      this.gvConditionTemplates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvConditionTemplates.Location = new Point(1, 26);
      this.gvConditionTemplates.Name = "gvConditionTemplates";
      this.gvConditionTemplates.Size = new Size(1148, 362);
      this.gvConditionTemplates.TabIndex = 2;
      this.gvConditionTemplates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditionTemplates.SubItemCheck += new GVSubItemEventHandler(this.gvConditionTemplates_SubItemCheck);
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
      this.AcceptButton = (IButtonControl) this.btnAdd;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(1150, 493);
      this.Controls.Add((Control) this.gcConditions);
      this.Controls.Add((Control) this.pnlClose);
      this.Controls.Add((Control) this.gradStackingOrder);
      this.Controls.Add((Control) this.helpLink);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = Resources.icon_allsizes_bug;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddEnhancedConditionsFromListDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Conditions: Conditions List";
      this.KeyDown += new KeyEventHandler(this.AddEnhancedConditionsFromListDialog_KeyDown);
      this.gradStackingOrder.ResumeLayout(false);
      this.gradStackingOrder.PerformLayout();
      this.pnlClose.ResumeLayout(false);
      this.gcConditions.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
