// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MaventFeeListForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MaventFeeListForm : Form
  {
    private IContainer components;
    private Button btnCancel;
    private GridView gridFees;
    private Button btnSelect;

    public MaventFeeListForm(string preselected)
    {
      this.InitializeComponent();
      this.initForm(preselected);
    }

    private void initForm(string preselected)
    {
    }

    private void gridFees_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.gridFees.SelectedItems.Count == 1;
    }

    private void btnSelect_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    public string MaventFeeSelected => this.gridFees.SelectedItems[0].Text;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.btnCancel = new Button();
      this.gridFees = new GridView();
      this.btnSelect = new Button();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(555, 323);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.gridFees.AllowMultiselect = false;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Fee Names";
      gvColumn.Width = 616;
      this.gridFees.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gridFees.Location = new Point(12, 12);
      this.gridFees.Name = "gridFees";
      this.gridFees.Size = new Size(618, 305);
      this.gridFees.TabIndex = 2;
      this.gridFees.SelectedIndexChanged += new EventHandler(this.gridFees_SelectedIndexChanged);
      this.btnSelect.DialogResult = DialogResult.Cancel;
      this.btnSelect.Location = new Point(474, 323);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 3;
      this.btnSelect.Text = "&Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(642, 358);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.gridFees);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MaventFeeListForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Mavent Fees";
      this.ResumeLayout(false);
    }
  }
}
