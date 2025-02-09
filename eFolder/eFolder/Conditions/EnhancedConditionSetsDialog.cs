// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.EnhancedConditionSetsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class EnhancedConditionSetsDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private eFolderManager eFolderMgr;
    private eFolderAccessRights rights;
    private EnhancedConditionTemplate[] conditionTemplates;
    private GridViewDataManager gvAvailableMgr;
    private GridViewDataManager gvSelectedMgr;
    private List<EnhancedConditionTemplate> templatesToAddList = new List<EnhancedConditionTemplate>();
    private string windowTitle;
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private Button btnRemove;
    private Button btnAdd;
    private GridView gvSelected;
    private ComboBox cboBorrower;
    private Label lblPair;
    private GridView gvAvailable;
    private Label lblSet;
    private ComboBox cboSet;
    private ToolTip tooltip;
    private Button btnAddAll;
    private SplitContainer splitContainer1;
    private Panel panel2;
    private Panel panel1;

    public EnhancedConditionSetsDialog(
      LoanDataMgr loanDataMgr,
      List<EnhancedConditionSet> condSets,
      EnhancedConditionTemplate[] conditionTemplates)
    {
      this.InitializeComponent();
      this.windowTitle = this.Text;
      this.loanDataMgr = loanDataMgr;
      this.eFolderMgr = new eFolderManager();
      this.rights = new eFolderAccessRights(this.loanDataMgr);
      this.conditionTemplates = conditionTemplates;
      this.initAvailableList();
      this.gvSelectedMgr = new GridViewDataManager(this.gvSelected, this.loanDataMgr);
      this.loadBorrowerList();
      this.loadConditionSetList(condSets);
    }

    public EnhancedConditionTemplate[] TemplatesToAdd => this.templatesToAddList.ToArray();

    public string PairId => ((BorrowerPair) this.cboBorrower.SelectedItem).Id;

    private void initAvailableList()
    {
      this.gvAvailableMgr = new GridViewDataManager(this.gvAvailable, this.loanDataMgr);
      this.gvAvailableMgr.LayoutChanged += new EventHandler(this.gvAvailableMgr_LayoutChanged);
      this.gvAvailableMgr.CreateLayout(new TableLayout.Column[4]
      {
        GridViewDataManager.InternalIdColumn,
        GridViewDataManager.CondNameColumn,
        GridViewDataManager.InternalDescriptionColumn,
        GridViewDataManager.ExternalDescriptionColumn
      });
      this.gvAvailable.Columns[0].Width = 75;
      this.gvAvailable.Columns[0].Tag = (object) GridViewDataManager.InternalIdColumn.ColumnID;
      this.gvAvailable.Columns[1].Width = 300;
      this.gvAvailable.Columns[1].Tag = (object) GridViewDataManager.CondNameColumn.ColumnID;
      this.gvAvailable.Columns[2].Width = 300;
      this.gvAvailable.Columns[2].Tag = (object) GridViewDataManager.InternalDescriptionColumn.ColumnID;
      this.gvAvailable.Columns[3].Width = 300;
      this.gvAvailable.Columns[3].Tag = (object) GridViewDataManager.ExternalDescriptionColumn.ColumnID;
    }

    private void gvAvailableMgr_LayoutChanged(object sender, EventArgs e)
    {
      this.loadAvailableList();
    }

    private void loadConditionSetList(List<EnhancedConditionSet> condSets)
    {
      foreach (object condSet in condSets)
        this.cboSet.Items.Add(condSet);
      if (this.cboSet.Items.Count <= 0)
        return;
      this.cboSet.SelectedIndex = 0;
    }

    private void loadBorrowerList()
    {
      BorrowerPair[] borrowerPairs = this.loanDataMgr.LoanData.GetBorrowerPairs();
      this.cboBorrower.Items.AddRange((object[]) borrowerPairs);
      this.cboBorrower.Items.Add((object) BorrowerPair.All);
      this.cboBorrower.SelectedItem = (object) borrowerPairs[0];
    }

    private void loadAvailableList()
    {
      this.gvAvailableMgr.ClearItems();
      EnhancedConditionSet selectedItem = (EnhancedConditionSet) this.cboSet.SelectedItem;
      if (selectedItem == null)
        return;
      EnhancedConditionSet conditionSetDetail = Session.ConfigurationManager.GetEnhancedConditionSetDetail(selectedItem.Id, false, true);
      List<EnhancedConditionTemplate> conditionTemplateList = new List<EnhancedConditionTemplate>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvSelected.Items)
        conditionTemplateList.Add((EnhancedConditionTemplate) gvItem.Tag);
      if (conditionSetDetail.ConditionTemplates != null)
      {
        foreach (EnhancedConditionTemplate conditionTemplate1 in conditionSetDetail.ConditionTemplates)
        {
          bool flag = false;
          foreach (EnhancedConditionTemplate conditionTemplate2 in conditionTemplateList)
          {
            Guid? id1 = conditionTemplate1.Id;
            Guid? id2 = conditionTemplate2.Id;
            if ((id1.HasValue == id2.HasValue ? (id1.HasValue ? (id1.GetValueOrDefault() == id2.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
              flag = true;
          }
          if (!flag && this.rights.CanAddEnhancedCondition(conditionTemplate1.ConditionType) && this.isTemplateActive(conditionTemplate1) && this.allowedOnLoan(conditionTemplate1))
            this.gvAvailableMgr.AddItem(conditionTemplate1);
        }
      }
      this.gvAvailableMgr.ApplyEmptyFilters = false;
      this.gvAvailableMgr.ApplyFilter();
      this.adjustInternalIdFilter();
      this.btnAddAll.Enabled = this.gvAvailable.Items.Count > 0;
    }

    private bool isTemplateActive(EnhancedConditionTemplate template)
    {
      return this.eFolderMgr.IsEnhancedConditionTemplateActive(template);
    }

    private bool allowedOnLoan(EnhancedConditionTemplate template)
    {
      return this.eFolderMgr.IsEnhancedConditionAllowedOnLoan(this.loanDataMgr, template);
    }

    private void adjustInternalIdFilter()
    {
      GVColumn gvColumn = this.gvAvailable.Columns.Where<GVColumn>((Func<GVColumn, bool>) (x => x.Text == GridViewDataManager.InternalIdColumn.Title)).DefaultIfEmpty<GVColumn>((GVColumn) null).ToList<GVColumn>()[0];
      if (gvColumn == null)
        return;
      ((ComboBox) gvColumn.FilterControl).Items.Clear();
      ((ComboBox) gvColumn.FilterControl).Items.Add((object) string.Empty);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAvailable.Items)
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

    private void cboSet_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.loadAvailableList();
      this.tooltip.SetToolTip((Control) this.cboSet, string.Concat(this.cboSet.SelectedItem));
    }

    private void gvAvailable_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = this.gvAvailable.SelectedItems.Count > 0;
    }

    private void gvSelected_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemove.Enabled = this.gvSelected.SelectedItems.Count > 0;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gvAvailable.SelectedItems)
        arrayList.Add((object) selectedItem);
      foreach (GVItem gvItem in arrayList)
      {
        this.gvAvailable.Items.Remove(gvItem);
        gvItem.Selected = false;
        this.gvSelected.Items.Add(gvItem);
        gvItem.Selected = true;
      }
      this.gvSelected.ReSort();
      this.gvSelected.Focus();
      this.btnOK.Enabled = this.gvSelected.Items.Count > 0;
      this.btnAddAll.Enabled = this.gvAvailable.Items.Count > 0;
      this.Text = this.windowTitle + "(" + (object) this.gvSelected.Items.Count + " Selected)";
    }

    private void btnAddAll_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAvailable.Items)
        arrayList.Add((object) gvItem);
      foreach (GVItem gvItem in arrayList)
      {
        this.gvAvailable.Items.Remove(gvItem);
        gvItem.Selected = false;
        this.gvSelected.Items.Add(gvItem);
        gvItem.Selected = true;
      }
      this.gvSelected.ReSort();
      this.gvSelected.Focus();
      this.btnOK.Enabled = this.gvSelected.Items.Count > 0;
      this.btnAddAll.Enabled = this.gvAvailable.Items.Count > 0;
      this.Text = this.windowTitle + "(" + (object) this.gvSelected.Items.Count + " Selected)";
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gvSelected.SelectedItems)
        arrayList.Add((object) selectedItem);
      foreach (GVItem gvItem in arrayList)
      {
        this.gvSelected.Items.Remove(gvItem);
        gvItem.Selected = false;
        this.gvAvailable.Items.Add(gvItem);
        gvItem.Selected = true;
      }
      this.gvAvailable.ReSort();
      this.gvAvailable.Focus();
      this.btnOK.Enabled = this.gvSelected.Items.Count > 0;
      this.btnAddAll.Enabled = this.gvAvailable.Items.Count > 0;
      this.Text = this.windowTitle + "(" + (object) this.gvSelected.Items.Count + " Selected)";
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvSelected.Items)
      {
        EnhancedConditionTemplate tag = (EnhancedConditionTemplate) gvItem.Tag;
        foreach (EnhancedConditionTemplate conditionTemplate in this.conditionTemplates)
        {
          Guid? id1 = conditionTemplate.Id;
          Guid? id2 = tag.Id;
          if ((id1.HasValue == id2.HasValue ? (id1.HasValue ? (id1.GetValueOrDefault() == id2.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
          {
            this.templatesToAddList.Add(conditionTemplate);
            break;
          }
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    private void EnhancedConditionSetsDialog_SizeChanged(object sender, EventArgs e)
    {
      this.splitContainer1.SplitterDistance = this.splitContainer1.Width / 2;
      if (this.gvSelected.Width > 975)
      {
        this.gvSelected.Columns[0].Width = 75;
        this.gvSelected.Columns[1].Width = this.gvSelected.Width - 300 - 75 - 300;
        this.gvSelected.Columns[2].Width = 300;
        this.gvSelected.Columns[3].Width = 300;
      }
      if (this.gvAvailable.Width <= 975 || this.gvAvailable.Columns.Count < 4)
        return;
      this.gvAvailable.Columns[0].Width = 75;
      this.gvAvailable.Columns[1].Width = this.gvAvailable.Width - 300 - 75 - 300;
      this.gvAvailable.Columns[2].Width = 300;
      this.gvAvailable.Columns[3].Width = 300;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.btnRemove = new Button();
      this.btnAdd = new Button();
      this.tooltip = new ToolTip(this.components);
      this.cboBorrower = new ComboBox();
      this.lblPair = new Label();
      this.lblSet = new Label();
      this.cboSet = new ComboBox();
      this.btnAddAll = new Button();
      this.splitContainer1 = new SplitContainer();
      this.panel2 = new Panel();
      this.gvAvailable = new GridView();
      this.panel1 = new Panel();
      this.gvSelected = new GridView();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(1546, 432);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 8;
      this.btnOK.Text = "Add";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(1622, 432);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnRemove.Enabled = false;
      this.btnRemove.Location = new Point(3, 153);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(64, 22);
      this.btnRemove.TabIndex = 4;
      this.btnRemove.Text = "< Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(3, 129);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(64, 22);
      this.btnAdd.TabIndex = 3;
      this.btnAdd.Text = "Add >";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.cboBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrower.FormattingEnabled = true;
      this.cboBorrower.Location = new Point(108, 8);
      this.cboBorrower.Name = "cboBorrower";
      this.cboBorrower.Size = new Size(395, 22);
      this.cboBorrower.TabIndex = 6;
      this.lblPair.AutoSize = true;
      this.lblPair.Location = new Point(11, 12);
      this.lblPair.Margin = new Padding(0);
      this.lblPair.Name = "lblPair";
      this.lblPair.Size = new Size(94, 14);
      this.lblPair.TabIndex = 5;
      this.lblPair.Text = "For Borrower Pair";
      this.lblSet.AutoSize = true;
      this.lblSet.Location = new Point(11, 39);
      this.lblSet.Name = "lblSet";
      this.lblSet.Size = new Size(76, 14);
      this.lblSet.TabIndex = 0;
      this.lblSet.Text = "Condition Sets";
      this.cboSet.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSet.DropDownWidth = 450;
      this.cboSet.FormattingEnabled = true;
      this.cboSet.Location = new Point(108, 36);
      this.cboSet.Name = "cboSet";
      this.cboSet.Size = new Size(395, 22);
      this.cboSet.Sorted = true;
      this.cboSet.TabIndex = 10;
      this.cboSet.SelectedIndexChanged += new EventHandler(this.cboSet_SelectedIndexChanged);
      this.btnAddAll.Enabled = false;
      this.btnAddAll.Location = new Point(3, 104);
      this.btnAddAll.Name = "btnAddAll";
      this.btnAddAll.Size = new Size(64, 22);
      this.btnAddAll.TabIndex = 11;
      this.btnAddAll.Text = "Add All >";
      this.btnAddAll.UseVisualStyleBackColor = true;
      this.btnAddAll.Click += new EventHandler(this.btnAddAll_Click);
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.Location = new Point(12, 64);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.panel2);
      this.splitContainer1.Panel1.Controls.Add((Control) this.panel1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.gvSelected);
      this.splitContainer1.Size = new Size(1685, 359);
      this.splitContainer1.SplitterDistance = 866;
      this.splitContainer1.TabIndex = 12;
      this.panel2.Controls.Add((Control) this.gvAvailable);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(799, 359);
      this.panel2.TabIndex = 1;
      this.gvAvailable.ClearSelectionsOnEmptyRowClick = false;
      this.gvAvailable.Dock = DockStyle.Fill;
      this.gvAvailable.FilterVisible = true;
      this.gvAvailable.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAvailable.Location = new Point(0, 0);
      this.gvAvailable.Name = "gvAvailable";
      this.gvAvailable.Size = new Size(799, 359);
      this.gvAvailable.TabIndex = 2;
      this.gvAvailable.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvAvailable.SelectedIndexChanged += new EventHandler(this.gvAvailable_SelectedIndexChanged);
      this.panel1.Controls.Add((Control) this.btnRemove);
      this.panel1.Controls.Add((Control) this.btnAddAll);
      this.panel1.Controls.Add((Control) this.btnAdd);
      this.panel1.Dock = DockStyle.Right;
      this.panel1.Location = new Point(799, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(67, 359);
      this.panel1.TabIndex = 0;
      this.gvSelected.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "InternalId";
      gvColumn1.Tag = (object) "InternalId";
      gvColumn1.Text = "Internal Id";
      gvColumn1.Width = 75;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Name";
      gvColumn2.Tag = (object) "NAME";
      gvColumn2.Text = "Condition Name";
      gvColumn2.Width = 300;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "InternalDescription";
      gvColumn3.Tag = (object) "INTERNALDESCRIPTION";
      gvColumn3.Text = "Internal Description";
      gvColumn3.Width = 300;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ExternalDescription";
      gvColumn4.Tag = (object) "EXTERNALDESCRIPTION";
      gvColumn4.Text = "External Description";
      gvColumn4.Width = 300;
      this.gvSelected.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gvSelected.Dock = DockStyle.Fill;
      this.gvSelected.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSelected.Location = new Point(0, 0);
      this.gvSelected.Name = "gvSelected";
      this.gvSelected.Size = new Size(815, 359);
      this.gvSelected.TabIndex = 7;
      this.gvSelected.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvSelected.SelectedIndexChanged += new EventHandler(this.gvSelected_SelectedIndexChanged);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(1709, 464);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.cboSet);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.cboBorrower);
      this.Controls.Add((Control) this.lblPair);
      this.Controls.Add((Control) this.lblSet);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EnhancedConditionSetsDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Conditions From Condition Set";
      this.SizeChanged += new EventHandler(this.EnhancedConditionSetsDialog_SizeChanged);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
