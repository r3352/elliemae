// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LockRequestProcessDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LockRequestProcessDialog : Form
  {
    private Label label1;
    private Button keepBtn;
    protected Button exitBtn;
    private PictureBox pictureBox1;
    private System.ComponentModel.Container components;
    private bool closeLoanFile;

    public LockRequestProcessDialog()
    {
      this.init(LockRequestProcessDialog.MessageSubjectType.LockRequest);
    }

    public LockRequestProcessDialog(
      LockRequestProcessDialog.MessageSubjectType messageSubject)
    {
      this.init(messageSubject);
    }

    private void init(
      LockRequestProcessDialog.MessageSubjectType messageSubject)
    {
      this.InitializeComponent();
      switch (messageSubject)
      {
        case LockRequestProcessDialog.MessageSubjectType.LockCancellation:
          this.label1.Text = this.label1.Text.Replace("MESSAGE_SUBJECT", "lock cancellation");
          break;
        case LockRequestProcessDialog.MessageSubjectType.Extention:
          this.label1.Text = this.label1.Text.Replace("MESSAGE_SUBJECT", "extension request");
          break;
        default:
          this.label1.Text = this.label1.Text.Replace("MESSAGE_SUBJECT", "lock request");
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public bool CloseLoanFile => this.closeLoanFile;

    private void InitializeComponent()
    {
      this.keepBtn = new Button();
      this.exitBtn = new Button();
      this.label1 = new Label();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.keepBtn.DialogResult = DialogResult.Cancel;
      this.keepBtn.Location = new Point(227, 63);
      this.keepBtn.Name = "keepBtn";
      this.keepBtn.Size = new Size(116, 24);
      this.keepBtn.TabIndex = 9;
      this.keepBtn.Text = "&Keep Loan Open";
      this.keepBtn.Click += new EventHandler(this.keepBtn_Click);
      this.exitBtn.Location = new Point(104, 63);
      this.exitBtn.Name = "exitBtn";
      this.exitBtn.Size = new Size(116, 24);
      this.exitBtn.TabIndex = 8;
      this.exitBtn.Text = "&Exit Loan";
      this.exitBtn.Click += new EventHandler(this.exitBtn_Click);
      this.label1.Location = new Point(48, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(380, 36);
      this.label1.TabIndex = 10;
      this.label1.Text = "The MESSAGE_SUBJECT has been submitted. The lock desk cannot process this request until you exit the loan. Do you want to exit the loan now?";
      this.pictureBox1.Image = (Image) Resources.warning_32x32;
      this.pictureBox1.Location = new Point(10, 10);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 11;
      this.pictureBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.exitBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(440, 97);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.keepBtn);
      this.Controls.Add((Control) this.exitBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LockRequestProcessDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void exitBtn_Click(object sender, EventArgs e)
    {
      this.closeLoanFile = true;
      this.DialogResult = DialogResult.OK;
    }

    private void keepBtn_Click(object sender, EventArgs e)
    {
      this.closeLoanFile = false;
      this.DialogResult = DialogResult.OK;
    }

    public enum MessageSubjectType
    {
      LockRequest,
      LockCancellation,
      Extention,
    }
  }
}
