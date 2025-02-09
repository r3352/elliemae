// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.RediscloseTILAlertDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class RediscloseTILAlertDialog : Form
  {
    private IContainer components;
    private Button btnYes;
    private Button btnNo;
    private Label labelAlert;
    private Label label1;
    private PictureBox pictureBox1;

    public RediscloseTILAlertDialog(bool forFix)
    {
      this.InitializeComponent();
      if (forFix)
        return;
      this.labelAlert.Text = "The Current APR differs from the Disclosed APR by .125% or greater. You may need to redisclose the APR.";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnYes = new Button();
      this.btnNo = new Button();
      this.labelAlert = new Label();
      this.label1 = new Label();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.btnYes.DialogResult = DialogResult.OK;
      this.btnYes.Location = new Point(158, 78);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 23);
      this.btnYes.TabIndex = 0;
      this.btnYes.Text = "&Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnNo.DialogResult = DialogResult.Cancel;
      this.btnNo.Location = new Point(239, 78);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 23);
      this.btnNo.TabIndex = 1;
      this.btnNo.Text = "&No";
      this.btnNo.UseVisualStyleBackColor = true;
      this.labelAlert.Location = new Point(56, 13);
      this.labelAlert.Name = "labelAlert";
      this.labelAlert.Size = new Size(358, 30);
      this.labelAlert.TabIndex = 2;
      this.labelAlert.Text = "The Current APR differs from the Disclosed APR by .125% or greater. You must redisclose the APR.";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(56, 47);
      this.label1.Name = "label1";
      this.label1.Size = new Size(193, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Do you want to open REGZ - TIL now?";
      this.pictureBox1.Image = (Image) Resources.RediscloseTILAlert;
      this.pictureBox1.Location = new Point(19, 12);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(27, 20);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 4;
      this.pictureBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnYes;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(431, 113);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.labelAlert);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnYes);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RediscloseTILAlertDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Redisclose TIL";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
