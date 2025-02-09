// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.NewResidenceDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class NewResidenceDialog : Form
  {
    private GroupBox groupBox1;
    private RadioButton brwBtn;
    private RadioButton radioButton1;
    private GroupBox groupBox2;
    private RadioButton priorBtn;
    private RadioButton currentBtn;
    private Button okBtn;
    private Button cancelBtn;
    private System.ComponentModel.Container components;
    private bool borrower;
    private bool current;

    public bool Borrower => this.borrower;

    public bool Current => this.current;

    public NewResidenceDialog(string caption, string status)
    {
      this.InitializeComponent();
      this.Text = caption;
      this.groupBox2.Text = status;
      this.StartPosition = FormStartPosition.CenterParent;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBox1 = new GroupBox();
      this.radioButton1 = new RadioButton();
      this.brwBtn = new RadioButton();
      this.groupBox2 = new GroupBox();
      this.priorBtn = new RadioButton();
      this.currentBtn = new RadioButton();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.radioButton1);
      this.groupBox1.Controls.Add((Control) this.brwBtn);
      this.groupBox1.Location = new Point(15, 23);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(138, 83);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Borrower Type";
      this.radioButton1.Location = new Point(22, 53);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new Size(95, 23);
      this.radioButton1.TabIndex = 1;
      this.radioButton1.Text = "CoBorrower";
      this.brwBtn.Checked = true;
      this.brwBtn.Location = new Point(22, 23);
      this.brwBtn.Name = "brwBtn";
      this.brwBtn.Size = new Size(95, 22);
      this.brwBtn.TabIndex = 0;
      this.brwBtn.TabStop = true;
      this.brwBtn.Text = "Borrower";
      this.groupBox2.Controls.Add((Control) this.priorBtn);
      this.groupBox2.Controls.Add((Control) this.currentBtn);
      this.groupBox2.Location = new Point(15, 136);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(138, 84);
      this.groupBox2.TabIndex = 2;
      this.groupBox2.TabStop = false;
      this.priorBtn.Location = new Point(22, 53);
      this.priorBtn.Name = "priorBtn";
      this.priorBtn.Size = new Size(95, 23);
      this.priorBtn.TabIndex = 1;
      this.priorBtn.Text = "Prior";
      this.currentBtn.Checked = true;
      this.currentBtn.Location = new Point(22, 23);
      this.currentBtn.Name = "currentBtn";
      this.currentBtn.Size = new Size(95, 22);
      this.currentBtn.TabIndex = 0;
      this.currentBtn.TabStop = true;
      this.currentBtn.Text = "Current";
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(168, 28);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 3;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(168, 60);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 4;
      this.cancelBtn.Text = "&Cancel";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(250, 236);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.groupBox2);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (NewResidenceDialog);
      this.ShowInTaskbar = false;
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.borrower = this.brwBtn.Checked;
      this.current = this.currentBtn.Checked;
      this.Close();
    }
  }
}
