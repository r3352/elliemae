// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.PETrialUpgradeForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class PETrialUpgradeForm : Form
  {
    private const string className = "PETrialUpgradeForm";
    private Button btnCDKey;
    private Label label1;
    private Label lblButtonInfo;
    private Button btnRegister;
    private Button btnContinue;
    private PictureBox pictureBox1;
    private System.ComponentModel.Container components;
    private LicenseInfo license;

    public PETrialUpgradeForm(LicenseInfo license, bool expired)
    {
      this.InitializeComponent();
      this.license = license;
      if (!expired)
        return;
      this.btnContinue.Text = "E&xit";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (PETrialUpgradeForm));
      this.btnCDKey = new Button();
      this.label1 = new Label();
      this.lblButtonInfo = new Label();
      this.btnRegister = new Button();
      this.btnContinue = new Button();
      this.pictureBox1 = new PictureBox();
      this.SuspendLayout();
      this.btnCDKey.Location = new Point(21, 88);
      this.btnCDKey.Name = "btnCDKey";
      this.btnCDKey.Size = new Size(146, 23);
      this.btnCDKey.TabIndex = 13;
      this.btnCDKey.Text = "&Obtain Activation Key";
      this.btnCDKey.Click += new EventHandler(this.btnCDKey_Click);
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(74, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(358, 18);
      this.label1.TabIndex = 12;
      this.label1.Text = "Thank you for evaluating Encompass!";
      this.lblButtonInfo.Location = new Point(74, 42);
      this.lblButtonInfo.Name = "lblButtonInfo";
      this.lblButtonInfo.Size = new Size(392, 32);
      this.lblButtonInfo.TabIndex = 11;
      this.lblButtonInfo.Text = "You are currently in evaluation mode. To unlock the software for unlimited use, please obtain and enter a free Activation Key.";
      this.btnRegister.Location = new Point(171, 88);
      this.btnRegister.Name = "btnRegister";
      this.btnRegister.Size = new Size(146, 23);
      this.btnRegister.TabIndex = 10;
      this.btnRegister.Text = "&Enter Activiation Key";
      this.btnRegister.Click += new EventHandler(this.btnRegister_Click);
      this.btnContinue.DialogResult = DialogResult.Cancel;
      this.btnContinue.Location = new Point(321, 88);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(146, 23);
      this.btnContinue.TabIndex = 9;
      this.btnContinue.Text = "&Continue Trial";
      this.pictureBox1.Image = (Image) resourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(18, 16);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 8;
      this.pictureBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnContinue;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnContinue;
      this.ClientSize = new Size(488, 125);
      this.Controls.Add((Control) this.btnCDKey);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblButtonInfo);
      this.Controls.Add((Control) this.btnRegister);
      this.Controls.Add((Control) this.btnContinue);
      this.Controls.Add((Control) this.pictureBox1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = nameof (PETrialUpgradeForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass Evaluation Version";
      this.ResumeLayout(false);
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
      try
      {
        Process.Start(Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "ServerConfig.exe"), "-register");
        this.DialogResult = DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(Tracing.SwOutsideLoan, TraceLevel.Error, nameof (PETrialUpgradeForm), "Error launching server config: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Encompass was unable to launch the registration wizard for the following reason: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnCDKey_Click(object sender, EventArgs e)
    {
      try
      {
        string str = Session.ConfigurationManager.GetCompanySetting("CLIENT", "AFFILIATEID") ?? "";
        Process.Start("https://encompass.elliemae.com/license/purchase.asp?product=" + this.license.Edition.ToString() + "&clientid=" + this.license.ClientID + "&afid=" + str);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "An error occurred when attempting to launch your default web brower. Please open your web brower and enter the URL provided.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }
  }
}
