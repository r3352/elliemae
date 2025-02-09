// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.ContactMessengerProfile
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class ContactMessengerProfile : Form
  {
    private Button okBtn;
    private GroupBox groupBox1;
    private Label label1;
    private Label lblName;
    private Label label3;
    private Label lblPhone;
    private Label lbluserID;
    private Label label4;
    private Label lblEmail;
    private Label label5;
    private PictureBox pictureBox1;
    private System.ComponentModel.Container components;

    public ContactMessengerProfile(UserInfo user)
    {
      this.InitializeComponent();
      this.lblName.Text = user.FullName;
      this.lblPhone.Text = user.Phone;
      this.lblEmail.Text = user.Email;
      this.lbluserID.Text = user.Userid;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ContactMessengerProfile));
      this.okBtn = new Button();
      this.groupBox1 = new GroupBox();
      this.pictureBox1 = new PictureBox();
      this.lblEmail = new Label();
      this.label5 = new Label();
      this.lbluserID = new Label();
      this.label4 = new Label();
      this.lblPhone = new Label();
      this.label3 = new Label();
      this.lblName = new Label();
      this.label1 = new Label();
      this.groupBox1.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(272, 160);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 3;
      this.okBtn.Text = "&Close";
      this.groupBox1.Controls.Add((Control) this.pictureBox1);
      this.groupBox1.Controls.Add((Control) this.lblEmail);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.lbluserID);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.lblPhone);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.lblName);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Location = new Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(336, 136);
      this.groupBox1.TabIndex = 5;
      this.groupBox1.TabStop = false;
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(16, 16);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 8;
      this.pictureBox1.TabStop = false;
      this.lblEmail.Location = new Point(136, 96);
      this.lblEmail.Name = "lblEmail";
      this.lblEmail.Size = new Size(184, 16);
      this.lblEmail.TabIndex = 7;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(60, 96);
      this.label5.Name = "label5";
      this.label5.Size = new Size(33, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "EMail";
      this.lbluserID.Location = new Point(136, 24);
      this.lbluserID.Name = "lbluserID";
      this.lbluserID.Size = new Size(184, 16);
      this.lbluserID.TabIndex = 5;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(60, 24);
      this.label4.Name = "label4";
      this.label4.Size = new Size(50, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "User's ID";
      this.lblPhone.Location = new Point(136, 72);
      this.lblPhone.Name = "lblPhone";
      this.lblPhone.Size = new Size(184, 16);
      this.lblPhone.TabIndex = 3;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(60, 72);
      this.label3.Name = "label3";
      this.label3.Size = new Size(38, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Phone";
      this.lblName.Location = new Point(136, 48);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(184, 16);
      this.lblName.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(60, 48);
      this.label1.Name = "label1";
      this.label1.Size = new Size(65, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "User's name";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(362, 201);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactMessengerProfile);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Contact Profile";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
