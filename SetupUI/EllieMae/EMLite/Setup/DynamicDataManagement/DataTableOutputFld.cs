// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DataTableOutputFld
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DataTableOutputFld : Form
  {
    private IContainer components;
    private Panel panel1;
    private Panel panel2;
    private Label lblColumn;
    private Button btnOk;
    private Button btnCancel;
    private TextBox txtColumnName;
    private TextBox txtColumn;
    private Label lblColumnName;

    public string Column
    {
      get => this.txtColumn.Text;
      set => this.txtColumn.Text = value;
    }

    public string ColumnName
    {
      get => this.txtColumnName.Text;
      set => this.txtColumnName.Text = value;
    }

    public DataTableOutputFld() => this.InitializeComponent();

    public DataTableOutputFld(string column, string columnName)
      : this()
    {
      this.Column = column;
      this.ColumnName = columnName;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.panel2 = new Panel();
      this.txtColumnName = new TextBox();
      this.txtColumn = new TextBox();
      this.lblColumnName = new Label();
      this.lblColumn = new Label();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnOk);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 84);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(408, 36);
      this.panel1.TabIndex = 1;
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Location = new Point(236, 0);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(77, 30);
      this.btnOk.TabIndex = 0;
      this.btnOk.Text = "Ok";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(322, 0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 30);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel2.Controls.Add((Control) this.txtColumnName);
      this.panel2.Controls.Add((Control) this.txtColumn);
      this.panel2.Controls.Add((Control) this.lblColumnName);
      this.panel2.Controls.Add((Control) this.lblColumn);
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(408, 78);
      this.panel2.TabIndex = 0;
      this.txtColumnName.Location = new Point(113, 48);
      this.txtColumnName.Name = "txtColumnName";
      this.txtColumnName.Size = new Size(259, 20);
      this.txtColumnName.TabIndex = 0;
      this.txtColumn.Location = new Point(113, 21);
      this.txtColumn.Name = "txtColumn";
      this.txtColumn.ReadOnly = true;
      this.txtColumn.Size = new Size(259, 20);
      this.txtColumn.TabIndex = 2;
      this.txtColumn.TabStop = false;
      this.lblColumnName.AutoSize = true;
      this.lblColumnName.Location = new Point(26, 51);
      this.lblColumnName.Name = "lblColumnName";
      this.lblColumnName.Size = new Size(73, 13);
      this.lblColumnName.TabIndex = 1;
      this.lblColumnName.Text = "Column Name";
      this.lblColumn.AutoSize = true;
      this.lblColumn.Location = new Point(26, 24);
      this.lblColumn.Name = "lblColumn";
      this.lblColumn.Size = new Size(42, 13);
      this.lblColumn.TabIndex = 0;
      this.lblColumn.Text = "Column";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(408, 120);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DataTableOutputFld);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Rename Output Column";
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
