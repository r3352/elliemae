// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.IIsServerManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.IIs;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class IIsServerManager : Form
  {
    private Label label1;
    private Label lblServerMode;
    private Label label2;
    private Button btnCancel;
    private Button btnOK;
    private System.ComponentModel.Container components;
    private Label label3;
    private Button btnConfigWizard;
    private Label lblRootName;
    private Label lblWizard;
    private Panel panel2;
    private RadioButton radCompressionDisabled;
    private RadioButton radCompressionEnabled;
    private Label label7;
    private Button btnApply;
    private Label lblIIsStatus;
    private Button btnStartStop;
    private Label lblServiceName;
    private IWebRoot iisApp;

    public IIsServerManager()
    {
      this.InitializeComponent();
      this.BackColor = EllieMae.EMLite.AdminTools.AdminTools.FormBackgroundColor;
      if (EllieMae.EMLite.Common.IIs.IIs.GetVersion() >= IIsVersion.IIs6)
        this.lblServiceName.Text = "Status:";
      try
      {
        this.iisApp = EllieMae.EMLite.Common.IIs.IIs.GetWebServer().GetWebSiteByID(EnConfigurationSettings.GlobalSettings.IIsWebSiteID).GetWebRoot(EnConfigurationSettings.GlobalSettings.IIsVirtualRootName);
        this.lblRootName.Text = this.iisApp.Name;
        this.refreshStatus();
      }
      catch
      {
        this.lblRootName.Text = "Unknown";
        this.lblIIsStatus.Text = "Unknown";
        this.btnStartStop.Enabled = false;
        this.lblWizard.Text = "To repair the Encompass Server settings, run the Server Configuration Wizard.";
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to locate the IIS Virtual Directory used by Encompass. Run the Configuration Wizard in Repair mode to resolve this problem.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.disableControls();
      }
      if (EnConfigurationSettings.GlobalSettings.ServerCompression)
        this.radCompressionEnabled.Checked = true;
      else
        this.radCompressionDisabled.Checked = true;
      this.btnApply.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IIsServerManager));
      this.label1 = new Label();
      this.lblServerMode = new Label();
      this.label2 = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.lblWizard = new Label();
      this.btnConfigWizard = new Button();
      this.label3 = new Label();
      this.lblRootName = new Label();
      this.panel2 = new Panel();
      this.radCompressionDisabled = new RadioButton();
      this.radCompressionEnabled = new RadioButton();
      this.label7 = new Label();
      this.btnApply = new Button();
      this.lblIIsStatus = new Label();
      this.lblServiceName = new Label();
      this.btnStartStop = new Button();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(26, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(100, 22);
      this.label1.TabIndex = 0;
      this.label1.Text = "Server Mode:";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.lblServerMode.Location = new Point(134, 18);
      this.lblServerMode.Name = "lblServerMode";
      this.lblServerMode.Size = new Size(166, 22);
      this.lblServerMode.TabIndex = 1;
      this.lblServerMode.Text = "IIS/Hosted";
      this.lblServerMode.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(26, 42);
      this.label2.Name = "label2";
      this.label2.Size = new Size(100, 22);
      this.label2.TabIndex = 2;
      this.label2.Text = "Virtual Root:";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(227, 216);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(72, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Location = new Point(153, 216);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(72, 23);
      this.btnOK.TabIndex = 10;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.lblWizard.Location = new Point(27, 122);
      this.lblWizard.Name = "lblWizard";
      this.lblWizard.Size = new Size(272, 50);
      this.lblWizard.TabIndex = 11;
      this.lblWizard.Text = "To uninstall the server, switch to Windows Service mode, or modify your registration information, run the Server Configuration Wizard.";
      this.lblWizard.TextAlign = ContentAlignment.MiddleLeft;
      this.btnConfigWizard.Location = new Point(151, 176);
      this.btnConfigWizard.Name = "btnConfigWizard";
      this.btnConfigWizard.Size = new Size(148, 23);
      this.btnConfigWizard.TabIndex = 12;
      this.btnConfigWizard.Text = "Configuration Wizard";
      this.btnConfigWizard.Click += new EventHandler(this.btnConfigWizard_Click);
      this.label3.BackColor = Color.Black;
      this.label3.Location = new Point(7, 204);
      this.label3.Name = "label3";
      this.label3.Size = new Size(304, 2);
      this.label3.TabIndex = 25;
      this.lblRootName.Location = new Point(134, 42);
      this.lblRootName.Name = "lblRootName";
      this.lblRootName.Size = new Size(166, 22);
      this.lblRootName.TabIndex = 26;
      this.lblRootName.Text = "(Root Name)";
      this.lblRootName.TextAlign = ContentAlignment.MiddleLeft;
      this.panel2.Controls.Add((Control) this.radCompressionDisabled);
      this.panel2.Controls.Add((Control) this.radCompressionEnabled);
      this.panel2.Location = new Point(133, 90);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(176, 24);
      this.panel2.TabIndex = 32;
      this.radCompressionDisabled.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radCompressionDisabled.Location = new Point(87, 2);
      this.radCompressionDisabled.Name = "radCompressionDisabled";
      this.radCompressionDisabled.Size = new Size(86, 22);
      this.radCompressionDisabled.TabIndex = 1;
      this.radCompressionDisabled.Text = "Disabled";
      this.radCompressionDisabled.CheckedChanged += new EventHandler(this.controlChanged);
      this.radCompressionDisabled.TextChanged += new EventHandler(this.controlChanged);
      this.radCompressionEnabled.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radCompressionEnabled.Location = new Point(2, 2);
      this.radCompressionEnabled.Name = "radCompressionEnabled";
      this.radCompressionEnabled.Size = new Size(84, 22);
      this.radCompressionEnabled.TabIndex = 0;
      this.radCompressionEnabled.Text = "Enabled";
      this.radCompressionEnabled.CheckedChanged += new EventHandler(this.controlChanged);
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(25, 90);
      this.label7.Name = "label7";
      this.label7.Size = new Size(100, 22);
      this.label7.TabIndex = 33;
      this.label7.Text = "Compression:";
      this.label7.TextAlign = ContentAlignment.MiddleLeft;
      this.btnApply.Location = new Point(79, 216);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new Size(72, 23);
      this.btnApply.TabIndex = 34;
      this.btnApply.Text = "&Apply";
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.lblIIsStatus.Location = new Point(134, 66);
      this.lblIIsStatus.Name = "lblIIsStatus";
      this.lblIIsStatus.Size = new Size(78, 22);
      this.lblIIsStatus.TabIndex = 36;
      this.lblIIsStatus.Text = "(IIS Status)";
      this.lblIIsStatus.TextAlign = ContentAlignment.MiddleLeft;
      this.lblServiceName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblServiceName.Location = new Point(26, 66);
      this.lblServiceName.Name = "lblServiceName";
      this.lblServiceName.Size = new Size(100, 22);
      this.lblServiceName.TabIndex = 35;
      this.lblServiceName.Text = "IIS Service:";
      this.lblServiceName.TextAlign = ContentAlignment.MiddleLeft;
      this.btnStartStop.Location = new Point(214, 66);
      this.btnStartStop.Name = "btnStartStop";
      this.btnStartStop.Size = new Size(86, 23);
      this.btnStartStop.TabIndex = 37;
      this.btnStartStop.Text = "Start";
      this.btnStartStop.Click += new EventHandler(this.btnStartStop_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(316, 248);
      this.Controls.Add((Control) this.btnStartStop);
      this.Controls.Add((Control) this.lblIIsStatus);
      this.Controls.Add((Control) this.lblServiceName);
      this.Controls.Add((Control) this.btnApply);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.lblRootName);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.btnConfigWizard);
      this.Controls.Add((Control) this.lblWizard);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.lblServerMode);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (IIsServerManager);
      this.Text = "Encompass Server Manager";
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private bool saveConfiguration()
    {
      EnConfigurationSettings.GlobalSettings.ServerCompression = this.radCompressionEnabled.Checked;
      int num = (int) Utils.Dialog((IWin32Window) this, "If the Encompass Server application has been loaded by IIS, your changes will not take effect until the web server has been stopped and restarted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return true;
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

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void refreshStatus()
    {
      try
      {
        if (this.IsServerRunning())
        {
          this.lblIIsStatus.Text = "Running";
          this.btnStartStop.Text = "Stop";
        }
        else
        {
          this.lblIIsStatus.Text = "Stopped";
          this.btnStartStop.Text = "Start";
        }
        this.btnStartStop.Enabled = true;
      }
      catch (Exception ex)
      {
        this.lblIIsStatus.Text = "Unknown";
        this.btnStartStop.Enabled = false;
      }
    }

    public bool IsServerRunning()
    {
      return EllieMae.EMLite.Common.IIs.IIs.GetVersion() == IIsVersion.IIs5 ? EllieMae.EMLite.Common.IIs.IIs.IsRunning() : this.iisApp.GetApplicationPool().IsRunning();
    }

    private void disableControls()
    {
      this.radCompressionEnabled.Enabled = false;
      this.radCompressionDisabled.Enabled = false;
      this.btnOK.Enabled = false;
      this.btnApply.Enabled = false;
    }

    private void btnConfigWizard_Click(object sender, EventArgs e)
    {
      if (!EllieMae.EMLite.AdminTools.AdminTools.StartServerConfig())
        return;
      this.Close();
    }

    private void controlChanged(object sender, EventArgs e) => this.btnApply.Enabled = true;

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

    private void btnStartStop_Click(object sender, EventArgs e)
    {
      if (this.btnStartStop.Text == "Start")
        this.start();
      else
        this.stop();
      this.refreshStatus();
    }

    public void StopServer()
    {
      if (EllieMae.EMLite.Common.IIs.IIs.GetVersion() == IIsVersion.IIs5)
        EllieMae.EMLite.Common.IIs.IIs.StopService();
      else
        this.iisApp.GetApplicationPool().Stop();
    }

    public void StartServer()
    {
      if (EllieMae.EMLite.Common.IIs.IIs.GetVersion() == IIsVersion.IIs5)
        EllieMae.EMLite.Common.IIs.IIs.StartService();
      else
        this.iisApp.GetApplicationPool().Start();
    }

    private void start()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        this.StartServer();
      }
      catch (Exception ex)
      {
        Cursor.Current = Cursors.Default;
        int num = (int) Utils.Dialog((IWin32Window) this, "Error starting service: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void stop()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        this.StopServer();
      }
      catch (Exception ex)
      {
        Cursor.Current = Cursors.Default;
        int num = (int) Utils.Dialog((IWin32Window) this, "Error stopping service: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }
  }
}
