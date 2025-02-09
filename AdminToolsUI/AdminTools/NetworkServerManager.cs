// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.NetworkServerManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.ServiceProcess;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class NetworkServerManager : Form
  {
    private Label label1;
    private Label lblServerMode;
    private Label label2;
    private Label label5;
    private Button btnStartStop;
    private Button btnCancel;
    private Button btnOK;
    private System.ComponentModel.Container components;
    private Label lblStatus;
    private Label lblServiceName;
    private Label label3;
    private Button btnConfigWizard;
    private Label label4;
    private Panel panel2;
    private RadioButton radCompressionDisabled;
    private RadioButton radCompressionEnabled;
    private Button btnApply;
    private Label label9;
    private Button btnEditIP;
    private Label lblIpRestrictions;
    private ServiceController service = new ServiceController(EnConfigurationSettings.GlobalSettings.ServerServiceName);
    private Label lblWizard;
    private TextBox txtServerPort;
    private Label label7;
    private IPAddressRange[] ipAddressRestrictions;

    public NetworkServerManager()
    {
      this.InitializeComponent();
      this.BackColor = EllieMae.EMLite.AdminTools.AdminTools.FormBackgroundColor;
      this.lblServiceName.Text = EnConfigurationSettings.GlobalSettings.ServerServiceName;
      this.txtServerPort.Text = EnConfigurationSettings.GlobalSettings.ServerPortNumber.ToString();
      this.ipAddressRestrictions = EnConfigurationSettings.GlobalSettings.ServerIPAddressRestrictions;
      if (EnConfigurationSettings.GlobalSettings.ServerCompression)
        this.radCompressionEnabled.Checked = true;
      else
        this.radCompressionDisabled.Checked = true;
      if (this.ipAddressRestrictions.Length != 0)
        this.lblIpRestrictions.Text = "Enabled";
      else
        this.lblIpRestrictions.Text = "Disabled";
      this.btnApply.Enabled = false;
      try
      {
        this.refreshStatus();
      }
      catch
      {
        this.lblStatus.Text = "Unknown";
        this.lblWizard.Text = "To repair the Encompass Server settings, run the Server Configuration Wizard.";
        this.lblStatus.Text = "Unknown";
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to locate the Encompass Server service. Run the Configuration Wizard in Repair mode to resolve this problem.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.disableControls();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (NetworkServerManager));
      this.label1 = new Label();
      this.lblServerMode = new Label();
      this.label2 = new Label();
      this.lblServiceName = new Label();
      this.lblStatus = new Label();
      this.label5 = new Label();
      this.btnStartStop = new Button();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.lblWizard = new Label();
      this.btnConfigWizard = new Button();
      this.label3 = new Label();
      this.label4 = new Label();
      this.panel2 = new Panel();
      this.radCompressionDisabled = new RadioButton();
      this.radCompressionEnabled = new RadioButton();
      this.btnApply = new Button();
      this.label9 = new Label();
      this.btnEditIP = new Button();
      this.lblIpRestrictions = new Label();
      this.txtServerPort = new TextBox();
      this.label7 = new Label();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(21, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(100, 22);
      this.label1.TabIndex = 0;
      this.label1.Text = "Server Mode:";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.lblServerMode.Location = new Point(129, 18);
      this.lblServerMode.Name = "lblServerMode";
      this.lblServerMode.Size = new Size(166, 22);
      this.lblServerMode.TabIndex = 1;
      this.lblServerMode.Text = "Windows Service";
      this.lblServerMode.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(21, 42);
      this.label2.Name = "label2";
      this.label2.Size = new Size(100, 22);
      this.label2.TabIndex = 2;
      this.label2.Text = "Service Name:";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.lblServiceName.Location = new Point(129, 42);
      this.lblServiceName.Name = "lblServiceName";
      this.lblServiceName.Size = new Size(166, 22);
      this.lblServiceName.TabIndex = 3;
      this.lblServiceName.Text = "EncompassServer";
      this.lblServiceName.TextAlign = ContentAlignment.MiddleLeft;
      this.lblStatus.Location = new Point(129, 66);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(81, 22);
      this.lblStatus.TabIndex = 5;
      this.lblStatus.Text = "(Status)";
      this.lblStatus.TextAlign = ContentAlignment.MiddleLeft;
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(21, 66);
      this.label5.Name = "label5";
      this.label5.Size = new Size(100, 22);
      this.label5.TabIndex = 4;
      this.label5.Text = "Status:";
      this.label5.TextAlign = ContentAlignment.MiddleLeft;
      this.btnStartStop.Location = new Point(212, 66);
      this.btnStartStop.Name = "btnStartStop";
      this.btnStartStop.Size = new Size(86, 23);
      this.btnStartStop.TabIndex = 6;
      this.btnStartStop.Text = "Start";
      this.btnStartStop.Click += new EventHandler(this.btnStartStop_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(228, 256);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(72, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Location = new Point(152, 256);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(72, 23);
      this.btnOK.TabIndex = 10;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.lblWizard.Location = new Point(28, 164);
      this.lblWizard.Name = "lblWizard";
      this.lblWizard.Size = new Size(272, 48);
      this.lblWizard.TabIndex = 11;
      this.lblWizard.Text = "To uninstall the server, switch to IIS/Hosted mode, or modify your registration information, run the Server Configuration Wizard.";
      this.lblWizard.TextAlign = ContentAlignment.MiddleLeft;
      this.btnConfigWizard.Location = new Point(152, 216);
      this.btnConfigWizard.Name = "btnConfigWizard";
      this.btnConfigWizard.Size = new Size(148, 23);
      this.btnConfigWizard.TabIndex = 12;
      this.btnConfigWizard.Text = "Configuration Wizard";
      this.btnConfigWizard.Click += new EventHandler(this.btnConfigWizard_Click);
      this.label3.BackColor = Color.Black;
      this.label3.Location = new Point(8, 244);
      this.label3.Name = "label3";
      this.label3.Size = new Size(304, 2);
      this.label3.TabIndex = 25;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(22, 114);
      this.label4.Name = "label4";
      this.label4.Size = new Size(100, 22);
      this.label4.TabIndex = 37;
      this.label4.Text = "Compression:";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.panel2.Controls.Add((Control) this.radCompressionDisabled);
      this.panel2.Controls.Add((Control) this.radCompressionEnabled);
      this.panel2.Location = new Point(130, 114);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(176, 22);
      this.panel2.TabIndex = 36;
      this.radCompressionDisabled.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radCompressionDisabled.Location = new Point(88, 2);
      this.radCompressionDisabled.Name = "radCompressionDisabled";
      this.radCompressionDisabled.Size = new Size(86, 22);
      this.radCompressionDisabled.TabIndex = 1;
      this.radCompressionDisabled.Text = "Disabled";
      this.radCompressionDisabled.CheckedChanged += new EventHandler(this.controlChanged);
      this.radCompressionEnabled.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radCompressionEnabled.Location = new Point(2, 2);
      this.radCompressionEnabled.Name = "radCompressionEnabled";
      this.radCompressionEnabled.Size = new Size(84, 22);
      this.radCompressionEnabled.TabIndex = 0;
      this.radCompressionEnabled.Text = "Enabled";
      this.radCompressionEnabled.CheckedChanged += new EventHandler(this.controlChanged);
      this.btnApply.Location = new Point(78, 256);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new Size(72, 23);
      this.btnApply.TabIndex = 38;
      this.btnApply.Text = "&Apply";
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(22, 138);
      this.label9.Name = "label9";
      this.label9.Size = new Size(100, 22);
      this.label9.TabIndex = 39;
      this.label9.Text = "IP Restrictions:";
      this.label9.TextAlign = ContentAlignment.MiddleLeft;
      this.btnEditIP.Location = new Point(212, 138);
      this.btnEditIP.Name = "btnEditIP";
      this.btnEditIP.Size = new Size(86, 23);
      this.btnEditIP.TabIndex = 41;
      this.btnEditIP.Text = "Edit";
      this.btnEditIP.Click += new EventHandler(this.btnEditIP_Click);
      this.lblIpRestrictions.Location = new Point(130, 138);
      this.lblIpRestrictions.Name = "lblIpRestrictions";
      this.lblIpRestrictions.Size = new Size(81, 22);
      this.lblIpRestrictions.TabIndex = 40;
      this.lblIpRestrictions.Text = "(Status)";
      this.lblIpRestrictions.TextAlign = ContentAlignment.MiddleLeft;
      this.txtServerPort.Location = new Point(130, 90);
      this.txtServerPort.Name = "txtServerPort";
      this.txtServerPort.Size = new Size(64, 20);
      this.txtServerPort.TabIndex = 43;
      this.txtServerPort.KeyPress += new KeyPressEventHandler(this.txtPort_KeyPress);
      this.txtServerPort.TextChanged += new EventHandler(this.controlChanged);
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(22, 90);
      this.label7.Name = "label7";
      this.label7.Size = new Size(100, 22);
      this.label7.TabIndex = 42;
      this.label7.Text = "Server Port:";
      this.label7.TextAlign = ContentAlignment.MiddleLeft;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(314, 288);
      this.Controls.Add((Control) this.txtServerPort);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.btnEditIP);
      this.Controls.Add((Control) this.lblIpRestrictions);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.btnApply);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.btnConfigWizard);
      this.Controls.Add((Control) this.lblWizard);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnStartStop);
      this.Controls.Add((Control) this.lblStatus);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.lblServiceName);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.lblServerMode);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (NetworkServerManager);
      this.Text = "Encompass Server Manager";
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void refreshStatus()
    {
      this.lblStatus.Text = SystemUtil.GetServiceStatusAsString(this.service);
      if (this.lblStatus.Text == "Running")
        this.btnStartStop.Text = "Stop";
      else
        this.btnStartStop.Text = "Start";
      this.btnStartStop.Enabled = true;
    }

    private void btnStartStop_Click(object sender, EventArgs e)
    {
      if (this.btnStartStop.Text == "Start")
      {
        this.StartService();
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Stopping the Encompass Server service will cause all users to be disconnected.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
          return;
        this.StopService();
      }
    }

    public bool StartService()
    {
      this.lblStatus.Text = "Starting...";
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        SystemUtil.StartService(this.service);
        this.refreshStatus();
        return true;
      }
      catch (Exception ex)
      {
        Cursor.Current = Cursors.Default;
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.lblStatus.Text = "Unknown";
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    public bool StopService()
    {
      this.lblStatus.Text = "Stopping...";
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        SystemUtil.StopService(this.service);
        this.refreshStatus();
        return true;
      }
      catch (Exception ex)
      {
        Cursor.Current = Cursors.Default;
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.lblStatus.Text = "Unknown";
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    public bool RestartService() => this.StopService() && this.StartService();

    public bool IsServiceRunning()
    {
      try
      {
        this.service.Refresh();
        return this.service.Status == ServiceControllerStatus.Running;
      }
      catch
      {
        return false;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.btnApply.Enabled && !this.saveConfiguration())
        return;
      this.Close();
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
      if (!this.saveConfiguration())
        return;
      this.btnApply.Enabled = false;
    }

    private bool saveConfiguration()
    {
      if (this.txtServerPort.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please specify the Server Port to be used for client connections.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!this.isValidPortNumber(this.txtServerPort.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Server Port number is invalid. Please ensure this field contains a number between " + (object) 0 + " and " + (object) (int) ushort.MaxValue + " and try again.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      EnConfigurationSettings.GlobalSettings.ServerPortNumber = int.Parse(this.txtServerPort.Text);
      EnConfigurationSettings.GlobalSettings.ServerCompression = this.radCompressionEnabled.Checked;
      EnConfigurationSettings.GlobalSettings.ServerIPAddressRestrictions = this.ipAddressRestrictions;
      if (this.IsServiceRunning() && Utils.Dialog((IWin32Window) this, "Your changes will not take effect until the Encompass Server has been stopped and restarted.\r\n\r\nStop and restart the server now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        this.RestartService();
        this.refreshStatus();
      }
      return true;
    }

    private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void btnConfigWizard_Click(object sender, EventArgs e)
    {
      if (!EllieMae.EMLite.AdminTools.AdminTools.StartServerConfig())
        return;
      this.Close();
    }

    private void controlChanged(object sender, EventArgs e) => this.btnApply.Enabled = true;

    private void btnEditIP_Click(object sender, EventArgs e)
    {
      IpAddressForm ipAddressForm = new IpAddressForm(this.ipAddressRestrictions);
      if (ipAddressForm.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      this.ipAddressRestrictions = ipAddressForm.GetIpRestrictions();
      if (this.ipAddressRestrictions.Length != 0)
        this.lblIpRestrictions.Text = "Enabled";
      else
        this.lblIpRestrictions.Text = "Disabled";
      this.btnApply.Enabled = true;
    }

    private void disableControls()
    {
      this.btnStartStop.Enabled = false;
      this.txtServerPort.Enabled = false;
      this.radCompressionEnabled.Enabled = false;
      this.radCompressionDisabled.Enabled = false;
      this.btnEditIP.Enabled = false;
      this.btnOK.Enabled = false;
      this.btnApply.Enabled = false;
    }

    private bool isValidPortNumber(string value)
    {
      if (value.Length == 0)
        return false;
      for (int index = 0; index < value.Length; ++index)
      {
        if (!char.IsDigit(value, index))
          return false;
      }
      return int.Parse(value) <= (int) ushort.MaxValue;
    }
  }
}
