// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Leads.ViewLeadsDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Leads
{
  public class ViewLeadsDialog : Form
  {
    private IContainer components;
    private Button btnYes;
    private CheckBox chkPrompt;
    private Button btnNo;
    private PictureBox pictureBox1;
    private Label label1;

    public ViewLeadsDialog() => this.InitializeComponent();

    public bool DoNotPrompt => this.chkPrompt.Checked;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ViewLeadsDialog));
      this.btnYes = new Button();
      this.chkPrompt = new CheckBox();
      this.btnNo = new Button();
      this.pictureBox1 = new PictureBox();
      this.label1 = new Label();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.btnYes.DialogResult = DialogResult.Yes;
      this.btnYes.Location = new Point(104, 68);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 22);
      this.btnYes.TabIndex = 6;
      this.btnYes.Text = "&Yes";
      this.chkPrompt.AutoSize = true;
      this.chkPrompt.Location = new Point(64, 36);
      this.chkPrompt.Name = "chkPrompt";
      this.chkPrompt.Size = new Size(205, 18);
      this.chkPrompt.TabIndex = 8;
      this.chkPrompt.Text = "Do not show this prompt in the future";
      this.btnNo.DialogResult = DialogResult.No;
      this.btnNo.Location = new Point(184, 68);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 22);
      this.btnNo.TabIndex = 7;
      this.btnNo.Text = "&No";
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(12, 12);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 5;
      this.pictureBox1.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(64, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(256, 14);
      this.label1.TabIndex = 4;
      this.label1.Text = "Do you want to view the leads you imported today?";
      this.AcceptButton = (IButtonControl) this.btnYes;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnNo;
      this.ClientSize = new Size(337, 107);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.chkPrompt);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ViewLeadsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "View Leads";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
