// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.HideeFolderWarningMessageForm
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.eFolder.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder
{
  public class HideeFolderWarningMessageForm : Form
  {
    private IContainer components;
    private Button button1;
    private Label label1;
    private CheckBox chkHide;
    private PictureBox pictureBox1;

    public HideeFolderWarningMessageForm() => this.InitializeComponent();

    public bool HideMessage => this.chkHide.Checked;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.button1 = new Button();
      this.label1 = new Label();
      this.chkHide = new CheckBox();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(141, 88);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 0;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.label1.Location = new Point(49, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(281, 42);
      this.label1.TabIndex = 1;
      this.label1.Text = "The service you are running has updated loan file data. Encompass will close and re-open eFolder to prevent data loss.";
      this.chkHide.AutoSize = true;
      this.chkHide.Location = new Point(52, 57);
      this.chkHide.Name = "chkHide";
      this.chkHide.Size = new Size(188, 17);
      this.chkHide.TabIndex = 2;
      this.chkHide.Text = "Do not display the message again.";
      this.chkHide.UseVisualStyleBackColor = true;
      this.pictureBox1.Image = (Image) Resources.warning_32x32;
      this.pictureBox1.Location = new Point(10, 11);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.TabIndex = 3;
      this.pictureBox1.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(342, 122);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.chkHide);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.button1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (HideeFolderWarningMessageForm);
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
