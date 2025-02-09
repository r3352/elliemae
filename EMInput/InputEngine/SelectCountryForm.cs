// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SelectCountryForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SelectCountryForm : Form
  {
    private List<string> countryList;
    private IContainer components;
    private Button btnSelect;
    private Button btnCancel;
    private TableContainer tableContainer1;
    private GridView lvwCountryNames;
    private Label label1;
    private TextBox txtSearch;

    public SelectCountryForm()
    {
      this.InitializeComponent();
      this.loadCountryNames();
      this.populateCountryNames();
      this.lvwCountryNames_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void populateCountryNames()
    {
      string str = this.txtSearch.Text.Trim();
      this.lvwCountryNames.BeginUpdate();
      this.lvwCountryNames.Items.Clear();
      for (int index = 0; index < this.countryList.Count; ++index)
      {
        if (string.IsNullOrEmpty(str) || this.countryList[index].ToLower().IndexOf(str.ToLower()) >= 0)
          this.lvwCountryNames.Items.Add(new GVItem(this.countryList[index]));
      }
      this.lvwCountryNames.EndUpdate();
      if (this.lvwCountryNames.Items.Count <= 0)
        return;
      this.lvwCountryNames.Items[0].Selected = true;
    }

    private void loadCountryNames() => this.countryList = Utils.GetCountryNames(false, false);

    private void txtSearch_TextChanged(object sender, EventArgs e) => this.populateCountryNames();

    public string SelectedCountryName => this.lvwCountryNames.SelectedItems[0].Text;

    private void lvwCountryNames_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.lvwCountryNames.SelectedItems.Count == 1;
    }

    private void btnSelect_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void SelectCountryForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.DialogResult = DialogResult.Cancel;
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
      GVItem gvItem = new GVItem();
      GVSubItem gvSubItem1 = new GVSubItem();
      GVSubItem gvSubItem2 = new GVSubItem();
      GVSubItem gvSubItem3 = new GVSubItem();
      GVSubItem gvSubItem4 = new GVSubItem();
      GVSubItem gvSubItem5 = new GVSubItem();
      this.btnSelect = new Button();
      this.btnCancel = new Button();
      this.tableContainer1 = new TableContainer();
      this.lvwCountryNames = new GridView();
      this.label1 = new Label();
      this.txtSearch = new TextBox();
      this.tableContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.BackColor = SystemColors.Control;
      this.btnSelect.Location = new Point(553, 399);
      this.btnSelect.Margin = new Padding(4, 5, 4, 5);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(112, 37);
      this.btnSelect.TabIndex = 2;
      this.btnSelect.Text = "&Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(675, 399);
      this.btnCancel.Margin = new Padding(4, 5, 4, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(112, 37);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.tableContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableContainer1.Controls.Add((Control) this.lvwCountryNames);
      this.tableContainer1.Location = new Point(11, 52);
      this.tableContainer1.Margin = new Padding(4, 5, 4, 5);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(776, 337);
      this.tableContainer1.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.tableContainer1.TabIndex = 75;
      this.tableContainer1.Text = "Country Names";
      this.lvwCountryNames.AllowMultiselect = false;
      this.lvwCountryNames.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column2";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Country Name";
      gvColumn.Width = 774;
      this.lvwCountryNames.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.lvwCountryNames.Dock = DockStyle.Fill;
      this.lvwCountryNames.HeaderHeight = 0;
      this.lvwCountryNames.HeaderVisible = false;
      this.lvwCountryNames.HotTrackingColor = Color.FromArgb(250, 248, 188);
      gvItem.BackColor = Color.Empty;
      gvItem.ForeColor = Color.Empty;
      gvSubItem1.BackColor = Color.Empty;
      gvSubItem1.ForeColor = Color.Empty;
      gvSubItem2.BackColor = Color.Empty;
      gvSubItem2.ForeColor = Color.Empty;
      gvSubItem3.BackColor = Color.Empty;
      gvSubItem3.ForeColor = Color.Empty;
      gvSubItem4.BackColor = Color.Empty;
      gvSubItem4.ForeColor = Color.Empty;
      gvSubItem5.BackColor = Color.Empty;
      gvSubItem5.ForeColor = Color.Empty;
      gvItem.SubItems.AddRange(new GVSubItem[5]
      {
        gvSubItem1,
        gvSubItem2,
        gvSubItem3,
        gvSubItem4,
        gvSubItem5
      });
      this.lvwCountryNames.Items.AddRange(new GVItem[1]
      {
        gvItem
      });
      this.lvwCountryNames.Location = new Point(1, 26);
      this.lvwCountryNames.Margin = new Padding(4, 5, 4, 5);
      this.lvwCountryNames.Name = "lvwCountryNames";
      this.lvwCountryNames.ShowFocusRect = true;
      this.lvwCountryNames.Size = new Size(774, 310);
      this.lvwCountryNames.SortOption = GVSortOption.None;
      this.lvwCountryNames.TabIndex = 1;
      this.lvwCountryNames.SelectedIndexChanged += new EventHandler(this.lvwCountryNames_SelectedIndexChanged);
      this.lvwCountryNames.DoubleClick += new EventHandler(this.btnSelect_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 19);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(60, 20);
      this.label1.TabIndex = 77;
      this.label1.Text = "Search";
      this.txtSearch.Location = new Point(78, 16);
      this.txtSearch.Margin = new Padding(4, 5, 4, 5);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new Size(368, 26);
      this.txtSearch.TabIndex = 0;
      this.txtSearch.TextChanged += new EventHandler(this.txtSearch_TextChanged);
      this.AcceptButton = (IButtonControl) this.btnSelect;
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(800, 450);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtSearch);
      this.Controls.Add((Control) this.tableContainer1);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.btnCancel);
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectCountryForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Country";
      this.KeyDown += new KeyEventHandler(this.SelectCountryForm_KeyDown);
      this.tableContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
