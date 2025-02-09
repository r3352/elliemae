// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CloseLoanQueryForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class CloseLoanQueryForm : Form
  {
    private Label label1;
    private Button button1;
    private Button button2;
    private RadioButton rbtnCloseSave;
    private RadioButton rbtnCloseNotSave;
    private Panel panel1;
    private Panel panel2;
    private Panel panel3;
    private System.ComponentModel.Container components;
    private bool _boolSaveLoan;

    public CloseLoanQueryForm()
    {
      this.InitializeComponent();
      this.Init();
    }

    private void Init()
    {
      this.label1.Text = "A loan is currently open in the Loan tab. Would you like to exit this file and originate a new loan?";
      if (Session.LoanDataMgr.Writable)
        return;
      this.label1.Text = "A read-only loan is currently open in the Loan tab. Do you want to: ";
      this.rbtnCloseSave.Enabled = false;
      this.rbtnCloseNotSave.Checked = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rbtnCloseSave = new RadioButton();
      this.rbtnCloseNotSave = new RadioButton();
      this.label1 = new Label();
      this.button1 = new Button();
      this.button2 = new Button();
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.panel3 = new Panel();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.rbtnCloseSave.Checked = true;
      this.rbtnCloseSave.Location = new Point(28, 12);
      this.rbtnCloseSave.Name = "rbtnCloseSave";
      this.rbtnCloseSave.Size = new Size(212, 28);
      this.rbtnCloseSave.TabIndex = 0;
      this.rbtnCloseSave.TabStop = true;
      this.rbtnCloseSave.Text = "Save and exit the open loan";
      this.rbtnCloseNotSave.Location = new Point(28, 44);
      this.rbtnCloseNotSave.Name = "rbtnCloseNotSave";
      this.rbtnCloseNotSave.Size = new Size(212, 28);
      this.rbtnCloseNotSave.TabIndex = 1;
      this.rbtnCloseNotSave.Text = "Exit the open loan without saving";
      this.label1.Location = new Point(12, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(248, 36);
      this.label1.TabIndex = 3;
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(88, 12);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 4;
      this.button1.Text = "&OK";
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(168, 12);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 5;
      this.button2.Text = "&Cancel";
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(270, 52);
      this.panel1.TabIndex = 6;
      this.panel2.Controls.Add((Control) this.button2);
      this.panel2.Controls.Add((Control) this.button1);
      this.panel2.Dock = DockStyle.Bottom;
      this.panel2.Location = new Point(0, 139);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(270, 44);
      this.panel2.TabIndex = 7;
      this.panel3.BackColor = Color.White;
      this.panel3.BorderStyle = BorderStyle.Fixed3D;
      this.panel3.Controls.Add((Control) this.rbtnCloseSave);
      this.panel3.Controls.Add((Control) this.rbtnCloseNotSave);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 52);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(270, 87);
      this.panel3.TabIndex = 8;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(270, 183);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CloseLoanQueryForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Originate a New Loan";
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public bool BoolSaveLoan => this._boolSaveLoan;

    private void button1_Click(object sender, EventArgs e)
    {
      this._boolSaveLoan = this.rbtnCloseSave.Checked;
    }
  }
}
