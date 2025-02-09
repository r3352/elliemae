// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.PasswordDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.DocumentConverter;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class PasswordDialog : Form
  {
    private string _password;
    private string _protectedFilePath;
    private string _unprotectedFilePath;
    private IContainer components;
    private Button btn_OK;
    private Button button2;
    private TextBox txtPassword;
    private Label label1;
    private Label lblMessage;

    public string Password => this._password;

    public string ProtectedFilePath => this._protectedFilePath;

    public string UnProtectedFilePath => this._unprotectedFilePath;

    public PasswordDialog(string filePath)
    {
      this.InitializeComponent();
      this.lblMessage.Text = Path.GetFileName(filePath);
      this._protectedFilePath = filePath;
    }

    private void OK_Click(object sender, EventArgs e)
    {
      this._password = this.txtPassword.Text;
      this._unprotectedFilePath = PasswordManager.RemovePasswordProtection(this._protectedFilePath, this._password);
      if (this._unprotectedFilePath == null)
      {
        this.lblMessage.Text = "Please enter a valid password";
      }
      else
      {
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void Cancel_Click(object sender, EventArgs e)
    {
      this._password = "";
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
      this.btn_OK = new Button();
      this.button2 = new Button();
      this.txtPassword = new TextBox();
      this.label1 = new Label();
      this.lblMessage = new Label();
      this.SuspendLayout();
      this.btn_OK.Location = new Point(80, 85);
      this.btn_OK.Name = "btn_OK";
      this.btn_OK.Size = new Size(75, 23);
      this.btn_OK.TabIndex = 0;
      this.btn_OK.Text = "OK";
      this.btn_OK.UseVisualStyleBackColor = true;
      this.btn_OK.Click += new EventHandler(this.OK_Click);
      this.button2.Location = new Point(177, 85);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 1;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.Cancel_Click);
      this.txtPassword.Location = new Point(147, 33);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.Size = new Size(161, 20);
      this.txtPassword.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(20, 36);
      this.label1.Name = "label1";
      this.label1.Size = new Size(120, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Enter the file password :";
      this.lblMessage.AutoSize = true;
      this.lblMessage.Location = new Point(20, 9);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(0, 13);
      this.lblMessage.TabIndex = 4;
      this.AcceptButton = (IButtonControl) this.btn_OK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(350, 133);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblMessage);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtPassword);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.btn_OK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PasswordDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Enter Password:";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
