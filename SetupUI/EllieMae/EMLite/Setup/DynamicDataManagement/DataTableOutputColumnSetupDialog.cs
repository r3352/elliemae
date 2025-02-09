// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DataTableOutputColumnSetupDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DataTableOutputColumnSetupDialog : Form
  {
    private bool throughSaveButton;
    private IContainer components;
    private Label label1;
    private ComboBoxEx numberOfOutputColumns;
    private GroupContainer groupContainer1;
    private GridView outputColumnsGrid;
    private ButtonEx buttonEx1;
    private ButtonEx btnSave;
    private ButtonEx buttonEx3;

    public GridView OutputGrid => this.outputColumnsGrid;

    public DataTableOutputColumnSetupDialog.METHOD Method { get; set; }

    public DataTableOutputColumnSetupDialog()
    {
      this.InitializeComponent();
      this.FormClosing += new FormClosingEventHandler(this.DataTableOutputColumnSetupDialog_FormClosing);
      this.Method = DataTableOutputColumnSetupDialog.METHOD.ADD;
      this.numberOfOutputColumns.SelectedItem = (object) "1";
      this.outputColumnsGrid.Items[0].Selected = true;
    }

    public DataTableOutputColumnSetupDialog(DDMDataTableInfo ddmDataTableInfo)
    {
      this.InitializeComponent();
      this.FormClosing += new FormClosingEventHandler(this.DataTableOutputColumnSetupDialog_FormClosing);
      this.Method = DataTableOutputColumnSetupDialog.METHOD.UPDATE;
      this.Text = "Modify Data Table";
      int num = 0;
      foreach (DDMDataTableFieldInfo field in ddmDataTableInfo.Fields)
      {
        if (field.IsOutput)
        {
          ++num;
          this.outputColumnsGrid.Items.Add(new GVItem(new string[2]
          {
            num.ToString(),
            field.FieldId
          }));
        }
      }
      this.numberOfOutputColumns.SelectedItem = (object) num.ToString();
      this.Method = DataTableOutputColumnSetupDialog.METHOD.ADD;
    }

    public DataTableOutputColumnSetupDialog(
      DDMDataTableInfo ddmDataTableInfo,
      Dictionary<string, List<string>> outputColumns)
    {
      this.InitializeComponent();
      this.FormClosing += new FormClosingEventHandler(this.DataTableOutputColumnSetupDialog_FormClosing);
      this.Method = DataTableOutputColumnSetupDialog.METHOD.IMPORT;
      if (outputColumns.Count > 8)
        this.numberOfOutputColumns.SelectedItem = (object) "8";
      else
        this.numberOfOutputColumns.SelectedItem = (object) outputColumns.Count.ToString();
      int num = 1;
      foreach (KeyValuePair<string, List<string>> outputColumn in outputColumns)
      {
        this.outputColumnsGrid.Items.Add(new GVItem(new string[3]
        {
          num.ToString(),
          outputColumn.Key.ToUpperInvariant(),
          outputColumn.Key.ToUpperInvariant()
        }));
        ++num;
      }
      this.outputColumnsGrid.Items[0].Selected = true;
      this.numberOfOutputColumns.Enabled = false;
      this.Method = DataTableOutputColumnSetupDialog.METHOD.ADD;
    }

    private void numberOfOutputColumns_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.Method != DataTableOutputColumnSetupDialog.METHOD.ADD)
        return;
      int result = 0;
      string[] items = new string[2];
      int num = this.outputColumnsGrid.Items.Count;
      if (num == 0)
        num = 1;
      if (!int.TryParse(this.numberOfOutputColumns.SelectedItem.ToString(), out result))
        return;
      if (result < num)
      {
        for (int index = num; index > result; --index)
          this.outputColumnsGrid.Items.RemoveAt(index - 1);
      }
      for (int index = num; index <= result; ++index)
      {
        if (index > this.outputColumnsGrid.Items.Count)
        {
          items[0] = index.ToString();
          items[1] = "OUTPUT COLUMN " + index.ToString();
          this.outputColumnsGrid.Items.Add(new GVItem(items));
        }
      }
    }

    private void DataTableOutputColumnSetupDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.throughSaveButton || !this.duplicateOutputColumnsExist())
        return;
      int num = (int) MessageBox.Show((IWin32Window) this, "Output column name already exists. Please provide a unique Output column name.", "Data Table Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      e.Cancel = true;
    }

    private bool duplicateOutputColumnsExist()
    {
      HashSet<string> stringSet = new HashSet<string>();
      for (int nItemIndex = 0; nItemIndex < this.outputColumnsGrid.Items.Count; ++nItemIndex)
      {
        string empty = string.Empty;
        string str = !(this.outputColumnsGrid.Items[nItemIndex].SubItems[1].Text == string.Empty) ? this.outputColumnsGrid.Items[nItemIndex].SubItems[1].Text : this.outputColumnsGrid.Items[nItemIndex].SubItems[0].Text;
        stringSet.Add(str.Trim());
      }
      return stringSet.Count != this.outputColumnsGrid.Items.Count;
    }

    private void buttonEx1_Click(object sender, EventArgs e) => this.throughSaveButton = false;

    private void btnSave_Click(object sender, EventArgs e) => this.throughSaveButton = true;

    private void buttonEx3_Click(object sender, EventArgs e) => this.buttonEx1_Click(sender, e);

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
      this.label1 = new Label();
      this.numberOfOutputColumns = new ComboBoxEx();
      this.groupContainer1 = new GroupContainer();
      this.outputColumnsGrid = new GridView();
      this.buttonEx1 = new ButtonEx();
      this.btnSave = new ButtonEx();
      this.buttonEx3 = new ButtonEx();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(28, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(214, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Number of Output Columns in the Import File";
      this.numberOfOutputColumns.DropDownStyle = ComboBoxStyle.DropDownList;
      this.numberOfOutputColumns.FormattingEnabled = true;
      this.numberOfOutputColumns.Items.AddRange(new object[8]
      {
        (object) "1",
        (object) "2",
        (object) "3",
        (object) "4",
        (object) "5",
        (object) "6",
        (object) "7",
        (object) "8"
      });
      this.numberOfOutputColumns.Location = new Point(270, 17);
      this.numberOfOutputColumns.MaxDropDownItems = 3;
      this.numberOfOutputColumns.Name = "numberOfOutputColumns";
      this.numberOfOutputColumns.SelectedBGColor = SystemColors.Highlight;
      this.numberOfOutputColumns.Size = new Size(235, 21);
      this.numberOfOutputColumns.TabIndex = 2;
      this.numberOfOutputColumns.SelectedIndexChanged += new EventHandler(this.numberOfOutputColumns_SelectedIndexChanged);
      this.groupContainer1.Controls.Add((Control) this.outputColumnsGrid);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(28, 41);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(478, 212);
      this.groupContainer1.TabIndex = 5;
      this.groupContainer1.Text = "Output Columns";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Number";
      gvColumn1.Text = "Number";
      gvColumn1.Width = 55;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Name";
      gvColumn2.Text = "Name";
      gvColumn2.Width = 420;
      this.outputColumnsGrid.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.outputColumnsGrid.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.outputColumnsGrid.Location = new Point(0, 25);
      this.outputColumnsGrid.Name = "outputColumnsGrid";
      this.outputColumnsGrid.Size = new Size(477, 183);
      this.outputColumnsGrid.TabIndex = 0;
      this.buttonEx1.DialogResult = DialogResult.Retry;
      this.buttonEx1.Location = new Point(270, 259);
      this.buttonEx1.Name = "buttonEx1";
      this.buttonEx1.Size = new Size(75, 23);
      this.buttonEx1.TabIndex = 6;
      this.buttonEx1.Text = "Back";
      this.buttonEx1.UseVisualStyleBackColor = true;
      this.buttonEx1.Click += new EventHandler(this.buttonEx1_Click);
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(351, 259);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 7;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.buttonEx3.DialogResult = DialogResult.Cancel;
      this.buttonEx3.Location = new Point(432, 259);
      this.buttonEx3.Name = "buttonEx3";
      this.buttonEx3.Size = new Size(75, 23);
      this.buttonEx3.TabIndex = 8;
      this.buttonEx3.Text = "Cancel";
      this.buttonEx3.UseVisualStyleBackColor = true;
      this.buttonEx3.Click += new EventHandler(this.buttonEx3_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(518, 288);
      this.Controls.Add((Control) this.buttonEx3);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.buttonEx1);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.numberOfOutputColumns);
      this.Controls.Add((Control) this.label1);
      this.MaximizeBox = false;
      this.MaximumSize = new Size(534, 327);
      this.MinimumSize = new Size(534, 327);
      this.Name = nameof (DataTableOutputColumnSetupDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Data Table";
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public enum METHOD
    {
      ADD,
      UPDATE,
      IMPORT,
    }
  }
}
