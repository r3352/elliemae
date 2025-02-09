// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.EmailInfoForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class EmailInfoForm : Form
  {
    private Label label1;
    private TextBox txtBoxSubject;
    private Button btnOK;
    private Button btnCancel;
    private System.ComponentModel.Container components;
    private string _EmailSubject = "";

    public EmailInfoForm() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.txtBoxSubject = new TextBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(148, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "Please enter email subject";
      this.txtBoxSubject.Location = new Point(12, 36);
      this.txtBoxSubject.Name = "txtBoxSubject";
      this.txtBoxSubject.Size = new Size(324, 20);
      this.txtBoxSubject.TabIndex = 1;
      this.txtBoxSubject.Text = "";
      this.btnOK.Location = new Point(176, 64);
      this.btnOK.Name = "btnOK";
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(260, 64);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(348, 93);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtBoxSubject);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EmailInfoForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Email Merge";
      this.ResumeLayout(false);
    }

    public string EmailSubject
    {
      get => this._EmailSubject;
      set => this._EmailSubject = value;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this._EmailSubject = this.txtBoxSubject.Text;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
  }
}
