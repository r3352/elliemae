// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.LogoutDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class LogoutDialog : Form
  {
    private IContainer components;
    private Label label1;
    private CheckBox chkDoNotShow;
    private Button btnNo;
    private Button btnYes;
    private PictureBox pictureBox1;

    public LogoutDialog() => this.InitializeComponent();

    private void btnYes_Click(object sender, EventArgs e)
    {
      this.saveDoNotShowSetting();
      this.DialogResult = DialogResult.Yes;
    }

    private void btnNo_Click(object sender, EventArgs e)
    {
      this.saveDoNotShowSetting();
      this.DialogResult = DialogResult.No;
    }

    private void saveDoNotShowSetting()
    {
      Session.WritePrivateProfileString("Dialog.Logout", this.chkDoNotShow.Checked ? "OFF" : "ON");
    }

    public static bool Display(IWin32Window parentForm)
    {
      string privateProfileString = Session.GetPrivateProfileString("Dialog.Logout");
      if (privateProfileString != "" && privateProfileString == "OFF")
        return true;
      using (LogoutDialog logoutDialog = new LogoutDialog())
        return logoutDialog.ShowDialog(parentForm) == DialogResult.Yes;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.chkDoNotShow = new CheckBox();
      this.btnNo = new Button();
      this.btnYes = new Button();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(54, 19);
      this.label1.Name = "label1";
      this.label1.Size = new Size(235, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Are you sure you want to exit Encompass?";
      this.chkDoNotShow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkDoNotShow.AutoSize = true;
      this.chkDoNotShow.Location = new Point(10, 66);
      this.chkDoNotShow.Name = "chkDoNotShow";
      this.chkDoNotShow.Size = new Size(171, 18);
      this.chkDoNotShow.TabIndex = 3;
      this.chkDoNotShow.Text = "Do not show this dialog again.";
      this.chkDoNotShow.UseVisualStyleBackColor = true;
      this.btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNo.DialogResult = DialogResult.Cancel;
      this.btnNo.Location = new Point(270, 63);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 22);
      this.btnNo.TabIndex = 2;
      this.btnNo.Text = "&No";
      this.btnNo.UseVisualStyleBackColor = true;
      this.btnNo.Click += new EventHandler(this.btnNo_Click);
      this.btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnYes.Location = new Point(187, 63);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 22);
      this.btnYes.TabIndex = 1;
      this.btnYes.Text = "&Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.pictureBox1.Image = (Image) Resources.question;
      this.pictureBox1.Location = new Point(10, 10);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 4;
      this.pictureBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnYes;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnNo;
      this.ClientSize = new Size(354, 94);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.chkDoNotShow);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LogoutDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
