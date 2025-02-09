// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.ConditionSetsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class ConditionSetsDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private ConditionTrackingSetup condSetup;
    private ConditionType condType;
    private EllieMae.EMLite.ClientServer.TemplateSettingsType templateType;
    private GridViewDataManager gvAvailableMgr;
    private GridViewDataManager gvSelectedMgr;
    private List<ConditionLog> condList = new List<ConditionLog>();
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

    public ConditionSetsDialog(LoanDataMgr loanDataMgr, ConditionType condType)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.condType = condType;
      this.initAvailableList();
      this.initSelectedList();
      this.initSetList();
      this.loadSetList();
      this.loadBorrowerList();
    }

    public ConditionLog[] Conditions => this.condList.ToArray();

    private void initAvailableList()
    {
      this.gvAvailableMgr = new GridViewDataManager(this.gvAvailable, this.loanDataMgr);
      this.gvAvailableMgr.CreateLayout(new TableLayout.Column[2]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DescriptionColumn
      });
      this.gvAvailable.Sort(0, SortOrder.Ascending);
    }

    private void initSelectedList()
    {
      this.gvSelectedMgr = new GridViewDataManager(this.gvSelected, this.loanDataMgr);
      this.gvSelectedMgr.CreateLayout(new TableLayout.Column[2]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DescriptionColumn
      });
      this.gvSelected.Sort(0, SortOrder.Ascending);
    }

    private void initSetList()
    {
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          this.condSetup = (ConditionTrackingSetup) this.loanDataMgr.SystemConfiguration.UnderwritingConditionTrackingSetup;
          this.templateType = EllieMae.EMLite.ClientServer.TemplateSettingsType.UnderwritingConditionSet;
          break;
        case ConditionType.PostClosing:
          this.condSetup = (ConditionTrackingSetup) this.loanDataMgr.SystemConfiguration.PostClosingConditionTrackingSetup;
          this.templateType = EllieMae.EMLite.ClientServer.TemplateSettingsType.PostClosingConditionSet;
          break;
        case ConditionType.Preliminary:
          this.condSetup = (ConditionTrackingSetup) this.loanDataMgr.SystemConfiguration.UnderwritingConditionTrackingSetup;
          this.templateType = EllieMae.EMLite.ClientServer.TemplateSettingsType.UnderwritingConditionSet;
          break;
      }
    }

    private void loadSetList()
    {
      this.cboSet.Items.AddRange((object[]) Session.ConfigurationManager.GetTemplateDirEntries(this.templateType, FileSystemEntry.PublicRoot));
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
      FileSystemEntry selectedItem = (FileSystemEntry) this.cboSet.SelectedItem;
      if (selectedItem == null)
        return;
      ConditionSetTemplate templateSettings = (ConditionSetTemplate) Session.ConfigurationManager.GetTemplateSettings(this.templateType, selectedItem);
      if (templateSettings == null)
        return;
      ArrayList arrayList = new ArrayList();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvSelected.Items)
        arrayList.Add(gvItem.Tag);
      foreach (string condition in templateSettings.Conditions)
      {
        ConditionTemplate byId = this.condSetup.GetByID(condition);
        if (byId != null && !arrayList.Contains((object) byId))
          this.gvAvailableMgr.AddItem(byId);
      }
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
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      BorrowerPair selectedItem = (BorrowerPair) this.cboBorrower.SelectedItem;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvSelected.Items)
      {
        ConditionTemplate tag = (ConditionTemplate) gvItem.Tag;
        ConditionLog rec = (ConditionLog) null;
        switch (this.condType)
        {
          case ConditionType.Underwriting:
            rec = (ConditionLog) new UnderwritingConditionLog((UnderwritingConditionTemplate) tag, Session.UserID, selectedItem.Id);
            break;
          case ConditionType.PostClosing:
            rec = (ConditionLog) new PostClosingConditionLog((PostClosingConditionTemplate) tag, Session.UserID, selectedItem.Id);
            break;
          case ConditionType.Preliminary:
            rec = (ConditionLog) new PreliminaryConditionLog((UnderwritingConditionTemplate) tag, Session.UserID, selectedItem.Id);
            break;
        }
        rec.Source = "condition set";
        logList.AddRecord((LogRecordBase) rec);
        this.condList.Add(rec);
      }
      this.DialogResult = DialogResult.OK;
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
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.btnRemove = new Button();
      this.btnAdd = new Button();
      this.gvSelected = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.cboBorrower = new ComboBox();
      this.lblPair = new Label();
      this.gvAvailable = new GridView();
      this.lblSet = new Label();
      this.cboSet = new ComboBox();
      this.SuspendLayout();
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(518, 396);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 8;
      this.btnOK.Text = "Add";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(594, 396);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnRemove.Enabled = false;
      this.btnRemove.Location = new Point(308, 212);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(64, 22);
      this.btnRemove.TabIndex = 4;
      this.btnRemove.Text = "< Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(308, 188);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(64, 22);
      this.btnAdd.TabIndex = 3;
      this.btnAdd.Text = "Add >";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gvSelected.ClearSelectionsOnEmptyRowClick = false;
      this.gvSelected.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSelected.Location = new Point(380, 36);
      this.gvSelected.Name = "gvSelected";
      this.gvSelected.Size = new Size(288, 352);
      this.gvSelected.TabIndex = 7;
      this.gvSelected.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvSelected.SelectedIndexChanged += new EventHandler(this.gvSelected_SelectedIndexChanged);
      this.cboBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrower.FormattingEnabled = true;
      this.cboBorrower.Location = new Point(474, 8);
      this.cboBorrower.Name = "cboBorrower";
      this.cboBorrower.Size = new Size(195, 22);
      this.cboBorrower.TabIndex = 6;
      this.lblPair.AutoSize = true;
      this.lblPair.Location = new Point(380, 12);
      this.lblPair.Margin = new Padding(0);
      this.lblPair.Name = "lblPair";
      this.lblPair.Size = new Size(94, 14);
      this.lblPair.TabIndex = 5;
      this.lblPair.Text = "For Borrower Pair";
      this.gvAvailable.ClearSelectionsOnEmptyRowClick = false;
      this.gvAvailable.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAvailable.Location = new Point(12, 36);
      this.gvAvailable.Name = "gvAvailable";
      this.gvAvailable.Size = new Size(288, 352);
      this.gvAvailable.TabIndex = 2;
      this.gvAvailable.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvAvailable.SelectedIndexChanged += new EventHandler(this.gvAvailable_SelectedIndexChanged);
      this.lblSet.AutoSize = true;
      this.lblSet.Location = new Point(12, 12);
      this.lblSet.Name = "lblSet";
      this.lblSet.Size = new Size(54, 14);
      this.lblSet.TabIndex = 0;
      this.lblSet.Text = "Cond. Set";
      this.cboSet.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSet.DropDownWidth = 450;
      this.cboSet.FormattingEnabled = true;
      this.cboSet.Location = new Point(68, 8);
      this.cboSet.Name = "cboSet";
      this.cboSet.Size = new Size(232, 22);
      this.cboSet.TabIndex = 10;
      this.cboSet.SelectedIndexChanged += new EventHandler(this.cboSet_SelectedIndexChanged);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(679, 427);
      this.Controls.Add((Control) this.cboSet);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.gvSelected);
      this.Controls.Add((Control) this.cboBorrower);
      this.Controls.Add((Control) this.lblPair);
      this.Controls.Add((Control) this.gvAvailable);
      this.Controls.Add((Control) this.lblSet);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConditionSetsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Condition Sets";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
