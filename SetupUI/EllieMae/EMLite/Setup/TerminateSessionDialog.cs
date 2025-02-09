// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TerminateSessionDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TerminateSessionDialog : Form
  {
    private Button btnYes;
    private Button btnNo;
    private CheckBox chkForce;
    private Label lblMessage;
    private PictureBox pictureBox1;
    private System.ComponentModel.Container components;

    public TerminateSessionDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TerminateSessionDialog));
      this.btnYes = new Button();
      this.btnNo = new Button();
      this.chkForce = new CheckBox();
      this.lblMessage = new Label();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.btnYes.DialogResult = DialogResult.Yes;
      this.btnYes.Location = new Point(156, 92);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 23);
      this.btnYes.TabIndex = 0;
      this.btnYes.Text = "&Yes";
      this.btnNo.DialogResult = DialogResult.No;
      this.btnNo.Location = new Point(250, 92);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 23);
      this.btnNo.TabIndex = 1;
      this.btnNo.Text = "&No";
      this.chkForce.Location = new Point(86, 56);
      this.chkForce.Name = "chkForce";
      this.chkForce.Size = new Size(344, 24);
      this.chkForce.TabIndex = 2;
      this.chkForce.Text = "Force immediate logout without opportunity to save";
      this.lblMessage.Location = new Point(86, 16);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(344, 34);
      this.lblMessage.TabIndex = 3;
      this.lblMessage.Text = "The selected user(s) will be logged out of Encompass. Are you sure you want to continue?";
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(22, 16);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 4;
      this.pictureBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnYes;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnNo;
      this.ClientSize = new Size(448, 126);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.lblMessage);
      this.Controls.Add((Control) this.chkForce);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnYes);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TerminateSessionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public DialogResult ShowDialog(Form parent, string text)
    {
      this.lblMessage.Text = text;
      return this.ShowDialog((IWin32Window) parent);
    }

    public bool ForceLogout => this.chkForce.Checked;
  }
}
