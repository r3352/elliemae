// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.ImportCustomForms
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class ImportCustomForms : Form
  {
    private RadioButton pointRadioBtn;
    private Button importBtn;
    private Label label1;
    private Button cancelBtn;
    private GroupBox groupBox1;
    private RadioButton otherRadioBtn;
    private System.ComponentModel.Container components;

    public ImportCustomForms() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pointRadioBtn = new RadioButton();
      this.importBtn = new Button();
      this.label1 = new Label();
      this.cancelBtn = new Button();
      this.otherRadioBtn = new RadioButton();
      this.groupBox1 = new GroupBox();
      this.SuspendLayout();
      this.pointRadioBtn.Checked = true;
      this.pointRadioBtn.ForeColor = SystemColors.ControlText;
      this.pointRadioBtn.Location = new Point(12, 41);
      this.pointRadioBtn.Name = "pointRadioBtn";
      this.pointRadioBtn.Size = new Size(120, 24);
      this.pointRadioBtn.TabIndex = 16;
      this.pointRadioBtn.TabStop = true;
      this.pointRadioBtn.Text = "Calyx Point";
      this.importBtn.BackColor = SystemColors.Control;
      this.importBtn.DialogResult = DialogResult.OK;
      this.importBtn.Location = new Point(162, 120);
      this.importBtn.Name = "importBtn";
      this.importBtn.Size = new Size(75, 24);
      this.importBtn.TabIndex = 17;
      this.importBtn.Text = "C&ontinue";
      this.importBtn.UseVisualStyleBackColor = false;
      this.importBtn.Click += new EventHandler(this.importBtn_Click);
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = SystemColors.ControlText;
      this.label1.Location = new Point(10, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(160, 16);
      this.label1.TabIndex = 14;
      this.label1.Text = "Import Custom Forms From:";
      this.cancelBtn.BackColor = SystemColors.Control;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(242, 120);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 15;
      this.cancelBtn.Text = "&Cancel";
      this.cancelBtn.UseVisualStyleBackColor = false;
      this.otherRadioBtn.ForeColor = SystemColors.ControlText;
      this.otherRadioBtn.Location = new Point(12, 68);
      this.otherRadioBtn.Name = "otherRadioBtn";
      this.otherRadioBtn.Size = new Size(216, 24);
      this.otherRadioBtn.TabIndex = 18;
      this.otherRadioBtn.Text = "Other Custom Forms (DOC and RTF)";
      this.groupBox1.Location = new Point(15, 101);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(302, 10);
      this.groupBox1.TabIndex = 50;
      this.groupBox1.TabStop = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(329, 155);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.otherRadioBtn);
      this.Controls.Add((Control) this.pointRadioBtn);
      this.Controls.Add((Control) this.importBtn);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cancelBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = nameof (ImportCustomForms);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import Custom Forms";
      this.ResumeLayout(false);
    }

    private void importBtn_Click(object sender, EventArgs e)
    {
      if (this.pointRadioBtn.Checked)
      {
        int num1 = (int) new ImportPointTemplates("CF").ShowDialog((IWin32Window) this);
      }
      else
      {
        if (!this.otherRadioBtn.Checked)
          return;
        int num2 = (int) new ImportOtherCustomForms().ShowDialog((IWin32Window) this);
      }
    }
  }
}
