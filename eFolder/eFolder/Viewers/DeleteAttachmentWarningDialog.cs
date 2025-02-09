// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.DeleteAttachmentWarningDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class DeleteAttachmentWarningDialog : Form
  {
    private IContainer components;
    private PictureBox pictureBoxWarning;
    private Label lblDeleteWarning;
    private Label lblDeleteWarning1;
    private CheckBox chkDoNotShow;
    private Button btnOk;

    public DeleteAttachmentWarningDialog() => this.InitializeComponent();

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.saveDoNotShowSetting();
      this.DialogResult = DialogResult.OK;
    }

    private void saveDoNotShowSetting()
    {
      Session.WritePrivateProfileString("Dialog.eFolderDeleteAttachment", this.chkDoNotShow.Checked ? "OFF" : "ON");
    }

    public static void ShowDeleteWarning(IWin32Window parent)
    {
      if (!(Session.GetPrivateProfileString("Dialog", "eFolderDeleteAttachment") != "OFF"))
        return;
      using (DeleteAttachmentWarningDialog attachmentWarningDialog = new DeleteAttachmentWarningDialog())
      {
        int num = (int) attachmentWarningDialog.ShowDialog(parent);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pictureBoxWarning = new PictureBox();
      this.lblDeleteWarning = new Label();
      this.lblDeleteWarning1 = new Label();
      this.chkDoNotShow = new CheckBox();
      this.btnOk = new Button();
      ((ISupportInitialize) this.pictureBoxWarning).BeginInit();
      this.SuspendLayout();
      this.pictureBoxWarning.Image = (Image) Resources.warning_32x32;
      this.pictureBoxWarning.Location = new Point(12, 12);
      this.pictureBoxWarning.Name = "pictureBoxWarning";
      this.pictureBoxWarning.Size = new Size(32, 32);
      this.pictureBoxWarning.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBoxWarning.TabIndex = 0;
      this.pictureBoxWarning.TabStop = false;
      this.lblDeleteWarning.AutoSize = true;
      this.lblDeleteWarning.Location = new Point(67, 12);
      this.lblDeleteWarning.Name = "lblDeleteWarning";
      this.lblDeleteWarning.Size = new Size(292, 14);
      this.lblDeleteWarning.TabIndex = 1;
      this.lblDeleteWarning.Text = "All the attachment’s pages have been assigned or deleted, ";
      this.lblDeleteWarning1.AutoSize = true;
      this.lblDeleteWarning1.Location = new Point(67, 32);
      this.lblDeleteWarning1.Name = "lblDeleteWarning1";
      this.lblDeleteWarning1.Size = new Size(174, 14);
      this.lblDeleteWarning1.TabIndex = 2;
      this.lblDeleteWarning1.Text = "and the attachment will be deleted.";
      this.chkDoNotShow.AutoSize = true;
      this.chkDoNotShow.Location = new Point(12, 69);
      this.chkDoNotShow.Name = "chkDoNotShow";
      this.chkDoNotShow.Size = new Size(187, 18);
      this.chkDoNotShow.TabIndex = 3;
      this.chkDoNotShow.Text = "Do not show this message again.";
      this.chkDoNotShow.UseVisualStyleBackColor = true;
      this.btnOk.Location = new Point(141, 94);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 4;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.AcceptButton = (IButtonControl) this.btnOk;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(368, 130);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.chkDoNotShow);
      this.Controls.Add((Control) this.lblDeleteWarning1);
      this.Controls.Add((Control) this.lblDeleteWarning);
      this.Controls.Add((Control) this.pictureBoxWarning);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DeleteAttachmentWarningDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass";
      ((ISupportInitialize) this.pictureBoxWarning).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
