// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ConcurrentEditing.HiddenLoanFileExpoDialog
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ConcurrentEditing
{
  public class HiddenLoanFileExpoDialog : Form
  {
    private IContainer components;
    private Button button1;
    private TextBox txtPassword;
    private Label label1;
    private TextBox txtBaseDoc;
    private TextBox txtFirstDoc;
    private TextBox txtSecondDoc;

    public HiddenLoanFileExpoDialog(string baseLoanXml, string firstLoanXml, string secondLoanXml)
    {
      this.InitializeComponent();
      this.txtBaseDoc.Text = baseLoanXml;
      this.txtFirstDoc.Text = firstLoanXml;
      this.txtSecondDoc.Text = secondLoanXml;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (this.txtPassword.Text != "3010000024password")
        return;
      this.txtBaseDoc.Visible = true;
      this.txtFirstDoc.Visible = true;
      this.txtSecondDoc.Visible = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.button1 = new Button();
      this.txtPassword = new TextBox();
      this.label1 = new Label();
      this.txtBaseDoc = new TextBox();
      this.txtFirstDoc = new TextBox();
      this.txtSecondDoc = new TextBox();
      this.SuspendLayout();
      this.button1.Location = new Point(533, 9);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 0;
      this.button1.Text = "Retrieve";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.txtPassword.Location = new Point(89, 9);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new Size(438, 20);
      this.txtPassword.TabIndex = 1;
      this.txtPassword.UseSystemPasswordChar = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(70, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Access Code";
      this.txtBaseDoc.Location = new Point(16, 38);
      this.txtBaseDoc.Multiline = true;
      this.txtBaseDoc.Name = "txtBaseDoc";
      this.txtBaseDoc.Size = new Size(592, 128);
      this.txtBaseDoc.TabIndex = 3;
      this.txtBaseDoc.UseSystemPasswordChar = true;
      this.txtBaseDoc.Visible = false;
      this.txtFirstDoc.Location = new Point(16, 172);
      this.txtFirstDoc.Multiline = true;
      this.txtFirstDoc.Name = "txtFirstDoc";
      this.txtFirstDoc.Size = new Size(592, 126);
      this.txtFirstDoc.TabIndex = 4;
      this.txtFirstDoc.UseSystemPasswordChar = true;
      this.txtFirstDoc.Visible = false;
      this.txtSecondDoc.Location = new Point(16, 304);
      this.txtSecondDoc.Multiline = true;
      this.txtSecondDoc.Name = "txtSecondDoc";
      this.txtSecondDoc.Size = new Size(592, 123);
      this.txtSecondDoc.TabIndex = 5;
      this.txtSecondDoc.UseSystemPasswordChar = true;
      this.txtSecondDoc.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(620, 439);
      this.Controls.Add((Control) this.txtSecondDoc);
      this.Controls.Add((Control) this.txtFirstDoc);
      this.Controls.Add((Control) this.txtBaseDoc);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtPassword);
      this.Controls.Add((Control) this.button1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (HiddenLoanFileExpoDialog);
      this.Text = "Get Loan Files";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
