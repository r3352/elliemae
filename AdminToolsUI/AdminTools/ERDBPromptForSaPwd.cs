// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.ERDBPromptForSaPwd
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class ERDBPromptForSaPwd : Form
  {
    private IContainer components;
    private Label label1;
    private TextBox textBoxSaPwd;
    private Button btnCreate;
    private Button btnCancel;

    public ERDBPromptForSaPwd() => this.InitializeComponent();

    private void textBoxSaPwd_TextChanged(object sender, EventArgs e)
    {
      this.btnCreate.Enabled = this.textBoxSaPwd.Text.Trim() != "";
    }

    public string SAPwd => this.textBoxSaPwd.Text.Trim();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.textBoxSaPwd = new TextBox();
      this.btnCreate = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(152, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "ERDB Database SA Password";
      this.textBoxSaPwd.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxSaPwd.Location = new Point(168, 15);
      this.textBoxSaPwd.Name = "textBoxSaPwd";
      this.textBoxSaPwd.PasswordChar = '*';
      this.textBoxSaPwd.Size = new Size(211, 20);
      this.textBoxSaPwd.TabIndex = 1;
      this.textBoxSaPwd.TextChanged += new EventHandler(this.textBoxSaPwd_TextChanged);
      this.btnCreate.DialogResult = DialogResult.OK;
      this.btnCreate.Enabled = false;
      this.btnCreate.Location = new Point(168, 41);
      this.btnCreate.Name = "btnCreate";
      this.btnCreate.Size = new Size(75, 23);
      this.btnCreate.TabIndex = 2;
      this.btnCreate.Text = "OK";
      this.btnCreate.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(249, 41);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnCreate;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(391, 73);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnCreate);
      this.Controls.Add((Control) this.textBoxSaPwd);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ERDBPromptForSaPwd);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass External Reproting Database SA Password";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
