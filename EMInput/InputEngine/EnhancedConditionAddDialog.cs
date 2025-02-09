// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.EnhancedConditionAddDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class EnhancedConditionAddDialog : Form
  {
    private eFolderAccessRights rights;
    private IEFolder eFolderMgr;
    private GridViewDataManager gvAvailableMgr;
    private GridViewDataManager gvSelectedMgr;
    private string condList = string.Empty;
    private IContainer components;
    private ComboBox cboSet;
    private Button btnOK;
    private Button btnCancel;
    private Button btnRemove;
    private Button btnAdd;
    private GridView gvAvailable;
    private Label lblSet;
    private GridView gvSelected;
    private SplitContainer splitContainer1;
    private Panel panel2;
    private Panel panel1;

    public EnhancedConditionAddDialog()
    {
      this.InitializeComponent();
      this.eFolderMgr = Session.Application.GetService<IEFolder>();
      this.rights = new eFolderAccessRights(Session.LoanDataMgr);
      this.initAvailableList();
      this.gvSelectedMgr = new GridViewDataManager(this.gvSelected, Session.LoanDataMgr);
      this.loadSetList();
    }

    private void initAvailableList()
    {
      this.gvAvailableMgr = new GridViewDataManager(this.gvAvailable, Session.LoanDataMgr);
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

    private void loadSetList()
    {
      foreach (object enhancedConditionSet in Session.ConfigurationManager.GetEnhancedConditionSets())
        this.cboSet.Items.Add(enhancedConditionSet);
      if (this.cboSet.Items.Count <= 0)
        return;
      this.cboSet.SelectedIndex = 0;
    }

    private void cboSet_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.loadAvailableList();
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
      if (conditionSetDetail.ConditionTemplates == null)
        return;
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

    private bool isTemplateActive(EnhancedConditionTemplate template)
    {
      return this.eFolderMgr.IsEnhancedConditionTemplateActive(template);
    }

    private bool allowedOnLoan(EnhancedConditionTemplate template)
    {
      return this.eFolderMgr.IsEnhancedConditionAllowedOnLoan(Session.LoanDataMgr, template);
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
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.gvSelected.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a condition first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.condList = string.Empty;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvSelected.Items)
        {
          EnhancedConditionTemplate tag = (EnhancedConditionTemplate) gvItem.Tag;
          if (this.condList != string.Empty)
            this.condList += "\r\n\r\n";
          this.condList = this.condList + tag.Title + ":" + tag.ExternalDescription;
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    public string CondList => this.condList;

    private void gvAvailable_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = this.gvAvailable.SelectedItems.Count > 0;
    }

    private void gvSelected_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemove.Enabled = this.gvSelected.SelectedItems.Count > 0;
    }

    private void EnhancedConditionAddDialog_SizeChanged(object sender, EventArgs e)
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.cboSet = new ComboBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.btnRemove = new Button();
      this.btnAdd = new Button();
      this.lblSet = new Label();
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
      this.cboSet.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSet.FormattingEnabled = true;
      this.cboSet.Location = new Point(91, 7);
      this.cboSet.Name = "cboSet";
      this.cboSet.Size = new Size(418, 21);
      this.cboSet.TabIndex = 20;
      this.cboSet.SelectedIndexChanged += new EventHandler(this.cboSet_SelectedIndexChanged);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(1546, 432);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 18;
      this.btnOK.Text = "Add";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(1622, 432);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 19;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnRemove.Enabled = false;
      this.btnRemove.Location = new Point(3, 161);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(64, 22);
      this.btnRemove.TabIndex = 14;
      this.btnRemove.Text = "< Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(3, 137);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(64, 22);
      this.btnAdd.TabIndex = 13;
      this.btnAdd.Text = "Add >";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.lblSet.AutoSize = true;
      this.lblSet.Location = new Point(10, 11);
      this.lblSet.Name = "lblSet";
      this.lblSet.Size = new Size(75, 13);
      this.lblSet.TabIndex = 11;
      this.lblSet.Text = "Condition Sets";
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.Location = new Point(12, 34);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.panel2);
      this.splitContainer1.Panel1.Controls.Add((Control) this.panel1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.gvSelected);
      this.splitContainer1.Size = new Size(1685, 390);
      this.splitContainer1.SplitterDistance = 874;
      this.splitContainer1.TabIndex = 22;
      this.panel2.Controls.Add((Control) this.gvAvailable);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(808, 390);
      this.panel2.TabIndex = 1;
      this.gvAvailable.ClearSelectionsOnEmptyRowClick = false;
      this.gvAvailable.Dock = DockStyle.Fill;
      this.gvAvailable.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAvailable.Location = new Point(0, 0);
      this.gvAvailable.Name = "gvAvailable";
      this.gvAvailable.Size = new Size(808, 390);
      this.gvAvailable.TabIndex = 12;
      this.gvAvailable.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvAvailable.SelectedIndexChanged += new EventHandler(this.gvAvailable_SelectedIndexChanged);
      this.panel1.Controls.Add((Control) this.btnRemove);
      this.panel1.Controls.Add((Control) this.btnAdd);
      this.panel1.Dock = DockStyle.Right;
      this.panel1.Location = new Point(808, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(66, 390);
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
      this.gvSelected.Size = new Size(807, 390);
      this.gvSelected.TabIndex = 21;
      this.gvSelected.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvSelected.SelectedIndexChanged += new EventHandler(this.gvSelected_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1709, 464);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.cboSet);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblSet);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EnhancedConditionAddDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Condition Sets";
      this.SizeChanged += new EventHandler(this.EnhancedConditionAddDialog_SizeChanged);
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
