// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.TrialUpgradeForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class TrialUpgradeForm : Form
  {
    private const string className = "TrialUpgradeForm";
    private LicenseInfo license;
    private PictureBox pictureBox1;
    private Button btnRegister;
    private Label lblTrialInfo;
    private Button btnContinue;
    private Label label1;
    private Button btnCDKey;
    private Label lblButtonInfo;
    private System.ComponentModel.Container components;

    public TrialUpgradeForm(LicenseInfo license)
    {
      this.InitializeComponent();
      this.license = license;
      this.lblTrialInfo.Text = this.lblTrialInfo.Text.Replace("%ENC_EDITION%", "Encompass");
      this.lblButtonInfo.Text = this.lblButtonInfo.Text.Replace("%ORDER_BTN%", "Order Now");
      this.btnRegister.Text = "&Order Now";
      this.lblTrialInfo.Text = this.lblTrialInfo.Text.Replace("%EXP_DATE%", license.Expires.ToShortDateString());
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (TrialUpgradeForm));
      this.lblTrialInfo = new Label();
      this.pictureBox1 = new PictureBox();
      this.btnContinue = new Button();
      this.btnRegister = new Button();
      this.lblButtonInfo = new Label();
      this.label1 = new Label();
      this.btnCDKey = new Button();
      this.SuspendLayout();
      this.lblTrialInfo.Location = new Point(80, 42);
      this.lblTrialInfo.Name = "lblTrialInfo";
      this.lblTrialInfo.Size = new Size(392, 24);
      this.lblTrialInfo.TabIndex = 0;
      this.lblTrialInfo.Text = "Your trial version of %ENC_EDITION% expires on %EXP_DATE%.";
      this.pictureBox1.Image = (Image) resourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(24, 18);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      this.btnContinue.DialogResult = DialogResult.Cancel;
      this.btnContinue.Location = new Point(326, 116);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(146, 23);
      this.btnContinue.TabIndex = 2;
      this.btnContinue.Text = "&Continue Trial";
      this.btnRegister.Location = new Point(208, 116);
      this.btnRegister.Name = "btnRegister";
      this.btnRegister.Size = new Size(111, 23);
      this.btnRegister.TabIndex = 3;
      this.btnRegister.Text = "&Order Now";
      this.btnRegister.Click += new EventHandler(this.btnRegister_Click);
      this.lblButtonInfo.Location = new Point(80, 66);
      this.lblButtonInfo.Name = "lblButtonInfo";
      this.lblButtonInfo.Size = new Size(392, 32);
      this.lblButtonInfo.TabIndex = 4;
      this.lblButtonInfo.Text = "To purchase a fully licensed version of Encompass, click the %ORDER_BTN% button.";
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(80, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(358, 18);
      this.label1.TabIndex = 5;
      this.label1.Text = "Thank you for trying Encompass!";
      this.btnCDKey.Location = new Point(80, 116);
      this.btnCDKey.Name = "btnCDKey";
      this.btnCDKey.Size = new Size(120, 23);
      this.btnCDKey.TabIndex = 6;
      this.btnCDKey.Text = "&Enter CD Key";
      this.btnCDKey.Click += new EventHandler(this.btnCDKey_Click);
      this.AcceptButton = (IButtonControl) this.btnContinue;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnContinue;
      this.ClientSize = new Size(488, 152);
      this.Controls.Add((Control) this.btnCDKey);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblButtonInfo);
      this.Controls.Add((Control) this.btnRegister);
      this.Controls.Add((Control) this.btnContinue);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.lblTrialInfo);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TrialUpgradeForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Trial Version";
      this.ResumeLayout(false);
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
      try
      {
        Process.Start(Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "ServerConfig.exe"), "-purchase");
        this.DialogResult = DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(Tracing.SwOutsideLoan, TraceLevel.Error, nameof (TrialUpgradeForm), "Error launching server config: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Encompass was unable to launch the registration wizard for the following reason: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnCDKey_Click(object sender, EventArgs e)
    {
      try
      {
        Process.Start(Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "ServerConfig.exe"), "-register");
        this.DialogResult = DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(Tracing.SwOutsideLoan, TraceLevel.Error, nameof (TrialUpgradeForm), "Error launching server config: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Encompass was unable to launch the registration wizard for the following reason: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }
  }
}
