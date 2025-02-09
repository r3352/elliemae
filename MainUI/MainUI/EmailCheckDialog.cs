// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.EmailCheckDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class EmailCheckDialog : Form
  {
    private IContainer components;
    private Button okBtn;
    private Label emailLbl;
    private TextBox emailTxt;
    private Label infoLbl;
    private CheckBox hideChk;
    private LinkLabel infoLnk;

    public EmailCheckDialog() => this.InitializeComponent();

    public string emailAddress => this.emailTxt.Text.Trim();

    public bool DoNotShowAgain => this.hideChk.Checked;

    private void btnOK_Click(object sender, EventArgs e)
    {
      string emailAddresses = this.emailTxt.Text.Trim();
      if (emailAddresses != string.Empty && !Utils.ValidateEmail(emailAddresses))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Email Address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void infoLnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Session.MainScreen.OpenURL("http://help.icemortgagetechnology.com/encompass/secondary_use.htm", "Secondary Use", 500, 500);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.okBtn = new Button();
      this.emailLbl = new Label();
      this.emailTxt = new TextBox();
      this.infoLbl = new Label();
      this.hideChk = new CheckBox();
      this.infoLnk = new LinkLabel();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Location = new Point(272, 117);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 22);
      this.okBtn.TabIndex = 5;
      this.okBtn.Text = "&OK";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.btnOK_Click);
      this.emailLbl.AutoSize = true;
      this.emailLbl.Location = new Point(12, 60);
      this.emailLbl.Name = "emailLbl";
      this.emailLbl.Size = new Size(137, 14);
      this.emailLbl.TabIndex = 1;
      this.emailLbl.Text = "Borrower's Email Address:";
      this.emailTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.emailTxt.Location = new Point(12, 76);
      this.emailTxt.Name = "emailTxt";
      this.emailTxt.Size = new Size(336, 20);
      this.emailTxt.TabIndex = 2;
      this.infoLbl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.infoLbl.Location = new Point(12, 12);
      this.infoLbl.Name = "infoLbl";
      this.infoLbl.Size = new Size(336, 32);
      this.infoLbl.TabIndex = 0;
      this.infoLbl.Text = "In order to comply with credit bureau policies for secondary use, please enter the borrower’s email address.";
      this.hideChk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.hideChk.AutoSize = true;
      this.hideChk.Location = new Point(152, 120);
      this.hideChk.Name = "hideChk";
      this.hideChk.Size = new Size(117, 18);
      this.hideChk.TabIndex = 4;
      this.hideChk.Text = "Do not show again";
      this.hideChk.UseVisualStyleBackColor = true;
      this.infoLnk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.infoLnk.AutoSize = true;
      this.infoLnk.Location = new Point(12, 120);
      this.infoLnk.Name = "infoLnk";
      this.infoLnk.Size = new Size(52, 14);
      this.infoLnk.TabIndex = 3;
      this.infoLnk.TabStop = true;
      this.infoLnk.Text = "More Info";
      this.infoLnk.LinkClicked += new LinkLabelLinkClickedEventHandler(this.infoLnk_LinkClicked);
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(358, 152);
      this.Controls.Add((Control) this.infoLnk);
      this.Controls.Add((Control) this.hideChk);
      this.Controls.Add((Control) this.infoLbl);
      this.Controls.Add((Control) this.emailTxt);
      this.Controls.Add((Control) this.emailLbl);
      this.Controls.Add((Control) this.okBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EmailCheckDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Email Check";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
