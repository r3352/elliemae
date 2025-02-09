// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.HELOCTableTypeSelectionForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class HELOCTableTypeSelectionForm : Form
  {
    private IContainer components;
    private Button btnCancel;
    private RadioButton rdoDynamic;
    private RadioButton rdoStatic;
    private Button btnOK;

    public HELOCTableTypeSelectionForm() => this.InitializeComponent();

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    public bool UseNewTable => this.rdoDynamic.Checked;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.rdoDynamic = new RadioButton();
      this.rdoStatic = new RadioButton();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(221, 112);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.rdoDynamic.AutoSize = true;
      this.rdoDynamic.Checked = true;
      this.rdoDynamic.Location = new Point(39, 24);
      this.rdoDynamic.Name = "rdoDynamic";
      this.rdoDynamic.Size = new Size(96, 17);
      this.rdoDynamic.TabIndex = 1;
      this.rdoDynamic.Text = "Dynamic Table";
      this.rdoDynamic.UseVisualStyleBackColor = true;
      this.rdoStatic.AutoSize = true;
      this.rdoStatic.Location = new Point(39, 47);
      this.rdoStatic.Name = "rdoStatic";
      this.rdoStatic.Size = new Size(82, 17);
      this.rdoStatic.TabIndex = 2;
      this.rdoStatic.Text = "Static Table";
      this.rdoStatic.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(140, 112);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(308, 147);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.rdoStatic);
      this.Controls.Add((Control) this.rdoDynamic);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (HELOCTableTypeSelectionForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "HELOC Table Type";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
