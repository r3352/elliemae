// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.DuplicateCampaignNameDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class DuplicateCampaignNameDialog : Form
  {
    private TextBox txtCampaignName;
    private Label lblAlreadyExists;
    private Label lblCampaignName;
    private PictureBox picWarning;
    private Button btnOK;
    private Button btnCancel;
    private System.ComponentModel.Container components;

    public string CampaignName
    {
      get => this.txtCampaignName.Text;
      set => this.txtCampaignName.Text = value;
    }

    public string UserName
    {
      set
      {
        if (!(string.Empty != value.Trim()))
          return;
        this.lblAlreadyExists.Text = "Campaign Name already exists for user " + value.Trim() + ". Please enter a different one.";
      }
    }

    public DuplicateCampaignNameDialog() => this.InitializeComponent();

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (string.Empty == this.txtCampaignName.Text)
        return;
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DuplicateCampaignNameDialog));
      this.lblAlreadyExists = new Label();
      this.lblCampaignName = new Label();
      this.txtCampaignName = new TextBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.picWarning = new PictureBox();
      ((ISupportInitialize) this.picWarning).BeginInit();
      this.SuspendLayout();
      this.lblAlreadyExists.Location = new Point(80, 24);
      this.lblAlreadyExists.Name = "lblAlreadyExists";
      this.lblAlreadyExists.Size = new Size(502, 23);
      this.lblAlreadyExists.TabIndex = 11;
      this.lblAlreadyExists.Text = "Campaign Name already exists. Please enter a different one.";
      this.lblAlreadyExists.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCampaignName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblCampaignName.Location = new Point(16, 72);
      this.lblCampaignName.Name = "lblCampaignName";
      this.lblCampaignName.Size = new Size(40, 23);
      this.lblCampaignName.TabIndex = 12;
      this.lblCampaignName.Text = "Name:";
      this.lblCampaignName.TextAlign = ContentAlignment.MiddleLeft;
      this.txtCampaignName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtCampaignName.Location = new Point(56, 73);
      this.txtCampaignName.MaxLength = 64;
      this.txtCampaignName.Name = "txtCampaignName";
      this.txtCampaignName.Size = new Size(528, 20);
      this.txtCampaignName.TabIndex = 13;
      this.btnOK.Location = new Point(218, 111);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 14;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(301, 111);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 15;
      this.btnCancel.Text = "Cancel";
      this.picWarning.Image = (Image) componentResourceManager.GetObject("picWarning.Image");
      this.picWarning.Location = new Point(16, 16);
      this.picWarning.Name = "picWarning";
      this.picWarning.Size = new Size(32, 32);
      this.picWarning.SizeMode = PictureBoxSizeMode.StretchImage;
      this.picWarning.TabIndex = 16;
      this.picWarning.TabStop = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(594, 148);
      this.Controls.Add((Control) this.picWarning);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.lblCampaignName);
      this.Controls.Add((Control) this.txtCampaignName);
      this.Controls.Add((Control) this.lblAlreadyExists);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DuplicateCampaignNameDialog);
      this.ShowInTaskbar = false;
      this.Text = "Encompass";
      this.KeyUp += new KeyEventHandler(this.DuplicateCampaignNameDialog_KeyUp);
      ((ISupportInitialize) this.picWarning).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void DuplicateCampaignNameDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }
  }
}
