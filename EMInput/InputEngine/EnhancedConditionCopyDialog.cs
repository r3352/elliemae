// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.EnhancedConditionCopyDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class EnhancedConditionCopyDialog : Form
  {
    private LoanData loan;
    private string condList = string.Empty;
    private List<EnhancedConditionLog> selected = new List<EnhancedConditionLog>();
    private IContainer components;
    private Panel panel2;
    private RadioButton rdoCopyAll;
    private RadioButton rdoAddFromSets;
    private Panel panel1;
    private GridView gridView1;
    private Panel panel3;
    private CheckBox chkAppend;
    private Button btnCancel;
    private Button btnOK;

    public EnhancedConditionCopyDialog(LoanData loan)
    {
      this.InitializeComponent();
      this.loan = loan;
      this.rdoAddFromSets.Checked = true;
      this.rdoCopyAll.Enabled = !this.loan.IsTemplate && this.loan != null;
      if (Session.IsBrokerEdition())
      {
        this.rdoAddFromSets.Visible = false;
        this.rdoCopyAll.Checked = true;
      }
      this.panel1.Visible = false;
    }

    public bool IsListAppended => this.chkAppend.Checked;

    public string CondList => this.condList;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.rdoAddFromSets.Checked)
      {
        using (EnhancedConditionAddDialog conditionAddDialog = new EnhancedConditionAddDialog())
        {
          if (conditionAddDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.condList = conditionAddDialog.CondList;
        }
      }
      else
      {
        foreach (EnhancedConditionLog enhancedConditionLog in this.selected)
          this.condList = this.condList + enhancedConditionLog.Title + ":" + enhancedConditionLog.ExternalDescription + "\r\n\r\n";
      }
      this.DialogResult = DialogResult.OK;
    }

    private void loadEnhancedConditions()
    {
      EnhancedConditionLog[] enhancedConditions = Session.LoanDataMgr.LoanData.GetLogList().GetAllEnhancedConditions();
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(Session.LoanDataMgr);
      foreach (EnhancedConditionLog enhancedConditionLog in enhancedConditions)
      {
        if (folderAccessRights.CanAccessEnhancedCondition(enhancedConditionLog.EnhancedConditionType) && enhancedConditionLog.StatusOpen)
        {
          GVItem gvItem = new GVItem();
          gvItem.SubItems[1].Text = enhancedConditionLog.ExternalId;
          gvItem.SubItems[2].Text = enhancedConditionLog.InternalId;
          gvItem.SubItems[3].Text = enhancedConditionLog.Title;
          this.gridView1.Items.Add(gvItem);
          gvItem.Tag = (object) enhancedConditionLog;
        }
      }
      this.gridView1.SubItemCheck += new GVSubItemEventHandler(this.closedConditionCheckChanged);
    }

    private void btnSelectFromConditionTabChanged(object sender, EventArgs e)
    {
      if (this.rdoCopyAll.Checked)
      {
        this.gridView1.Enabled = true;
        this.loadEnhancedConditions();
        this.btnOK.Enabled = false;
        this.panel1.Visible = true;
      }
      else
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridView1.Items)
          gvItem.Checked = false;
        this.gridView1.Enabled = false;
        this.gridView1.Items.Clear();
        this.btnOK.Enabled = true;
        this.panel1.Visible = false;
      }
    }

    private void closedConditionCheckChanged(object sender, GVSubItemEventArgs e)
    {
      GVSubItem subItem = e.SubItem;
      if (subItem.Checked)
        this.selected.Add((EnhancedConditionLog) subItem.Parent.Tag);
      else
        this.selected.Remove((EnhancedConditionLog) subItem.Parent.Tag);
      this.btnOK.Enabled = this.selected.Count > 0;
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
      this.panel2 = new Panel();
      this.rdoCopyAll = new RadioButton();
      this.rdoAddFromSets = new RadioButton();
      this.panel1 = new Panel();
      this.gridView1 = new GridView();
      this.panel3 = new Panel();
      this.chkAppend = new CheckBox();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.panel2.AutoSize = true;
      this.panel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.panel2.Controls.Add((Control) this.rdoCopyAll);
      this.panel2.Controls.Add((Control) this.rdoAddFromSets);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(587, 51);
      this.panel2.TabIndex = 23;
      this.rdoCopyAll.Location = new Point(10, 28);
      this.rdoCopyAll.Name = "rdoCopyAll";
      this.rdoCopyAll.Size = new Size(264, 20);
      this.rdoCopyAll.TabIndex = 22;
      this.rdoCopyAll.Text = "Copy Open Conditions from Conditions Tab";
      this.rdoCopyAll.CheckedChanged += new EventHandler(this.btnSelectFromConditionTabChanged);
      this.rdoAddFromSets.Checked = true;
      this.rdoAddFromSets.Location = new Point(10, 9);
      this.rdoAddFromSets.Name = "rdoAddFromSets";
      this.rdoAddFromSets.Size = new Size(232, 20);
      this.rdoAddFromSets.TabIndex = 21;
      this.rdoAddFromSets.TabStop = true;
      this.rdoAddFromSets.Text = "Add from Condition Sets";
      this.panel1.Controls.Add((Control) this.gridView1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 51);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(587, 370);
      this.panel1.TabIndex = 24;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ColumnHeaderCheckbox = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Checkbox;
      gvColumn1.Text = "";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "External ID";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Internal ID";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Condition Name";
      gvColumn4.Width = 200;
      this.gridView1.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gridView1.DropTarget = GVDropTarget.Item;
      this.gridView1.Enabled = false;
      this.gridView1.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridView1.Location = new Point(13, 9);
      this.gridView1.Name = "gridView1";
      this.gridView1.Size = new Size(561, 348);
      this.gridView1.TabIndex = 23;
      this.panel3.Controls.Add((Control) this.chkAppend);
      this.panel3.Controls.Add((Control) this.btnCancel);
      this.panel3.Controls.Add((Control) this.btnOK);
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(0, 421);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(587, 40);
      this.panel3.TabIndex = 26;
      this.chkAppend.Checked = true;
      this.chkAppend.CheckState = CheckState.Checked;
      this.chkAppend.Location = new Point(283, 12);
      this.chkAppend.Name = "chkAppend";
      this.chkAppend.Size = new Size(128, 16);
      this.chkAppend.TabIndex = 28;
      this.chkAppend.Text = "Append to current list";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(501, 7);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 26;
      this.btnCancel.Text = "&Cancel";
      this.btnOK.Location = new Point(422, 7);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 27;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.ClientSize = new Size(587, 460);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.panel2);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(603, 39);
      this.Name = nameof (EnhancedConditionCopyDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Condition";
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
