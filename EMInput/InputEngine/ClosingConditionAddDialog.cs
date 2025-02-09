// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ClosingConditionAddDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.eFolder;
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
  public class ClosingConditionAddDialog : Form
  {
    private GridViewDataManager gvAvailableMgr;
    private ConditionTrackingSetup condSetup;
    private GridViewDataManager gvSelectedMgr;
    private string condList = string.Empty;
    private IContainer components;
    private ComboBox cboSet;
    private Button btnOK;
    private Button btnCancel;
    private Button btnRemove;
    private Button btnAdd;
    private GridView gvSelected;
    private GridView gvAvailable;
    private Label lblSet;

    public ClosingConditionAddDialog()
    {
      this.InitializeComponent();
      this.condSetup = Session.ConfigurationManager.GetConditionTrackingSetup(ConditionType.Underwriting);
      this.initAvailableList();
      this.initSelectedList();
      this.loadSetList();
    }

    private void initAvailableList()
    {
      this.gvAvailableMgr = new GridViewDataManager(this.gvAvailable, Session.LoanDataMgr);
      this.gvAvailableMgr.CreateLayout(new TableLayout.Column[2]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DescriptionColumn
      });
      this.gvAvailable.Sort(0, SortOrder.Ascending);
    }

    private void loadSetList()
    {
      this.cboSet.Items.AddRange((object[]) Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.UnderwritingConditionSet, FileSystemEntry.PublicRoot));
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
      FileSystemEntry selectedItem = (FileSystemEntry) this.cboSet.SelectedItem;
      if (selectedItem == null)
        return;
      ConditionSetTemplate templateSettings = (ConditionSetTemplate) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.UnderwritingConditionSet, selectedItem);
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

    private void initSelectedList()
    {
      this.gvSelectedMgr = new GridViewDataManager(this.gvSelected, Session.LoanDataMgr);
      this.gvSelectedMgr.CreateLayout(new TableLayout.Column[2]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DescriptionColumn
      });
      this.gvSelected.Sort(0, SortOrder.Ascending);
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
          ConditionTemplate tag = (ConditionTemplate) gvItem.Tag;
          if (this.condList != string.Empty)
            this.condList += "\r\n\r\n";
          this.condList = this.condList + tag.Name + ":" + tag.Description;
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cboSet = new ComboBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.btnRemove = new Button();
      this.btnAdd = new Button();
      this.gvSelected = new GridView();
      this.gvAvailable = new GridView();
      this.lblSet = new Label();
      this.SuspendLayout();
      this.cboSet.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSet.FormattingEnabled = true;
      this.cboSet.Location = new Point(66, 7);
      this.cboSet.Name = "cboSet";
      this.cboSet.Size = new Size(232, 21);
      this.cboSet.TabIndex = 20;
      this.cboSet.SelectedIndexChanged += new EventHandler(this.cboSet_SelectedIndexChanged);
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(516, 395);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 18;
      this.btnOK.Text = "Add";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(592, 395);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 19;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnRemove.Enabled = false;
      this.btnRemove.Location = new Point(306, 211);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(64, 22);
      this.btnRemove.TabIndex = 14;
      this.btnRemove.Text = "< Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(306, 187);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(64, 22);
      this.btnAdd.TabIndex = 13;
      this.btnAdd.Text = "Add >";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gvSelected.ClearSelectionsOnEmptyRowClick = false;
      this.gvSelected.Location = new Point(378, 35);
      this.gvSelected.Name = "gvSelected";
      this.gvSelected.Size = new Size(288, 352);
      this.gvSelected.TabIndex = 17;
      this.gvSelected.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvSelected.SelectedIndexChanged += new EventHandler(this.gvSelected_SelectedIndexChanged);
      this.gvAvailable.ClearSelectionsOnEmptyRowClick = false;
      this.gvAvailable.Location = new Point(10, 35);
      this.gvAvailable.Name = "gvAvailable";
      this.gvAvailable.Size = new Size(288, 352);
      this.gvAvailable.TabIndex = 12;
      this.gvAvailable.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvAvailable.SelectedIndexChanged += new EventHandler(this.gvAvailable_SelectedIndexChanged);
      this.lblSet.AutoSize = true;
      this.lblSet.Location = new Point(10, 11);
      this.lblSet.Name = "lblSet";
      this.lblSet.Size = new Size(54, 13);
      this.lblSet.TabIndex = 11;
      this.lblSet.Text = "Cond. Set";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(677, 425);
      this.Controls.Add((Control) this.cboSet);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.gvSelected);
      this.Controls.Add((Control) this.gvAvailable);
      this.Controls.Add((Control) this.lblSet);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ClosingConditionAddDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Condition Sets";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
