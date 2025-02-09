// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.DbServerManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.ServiceProcess;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class DbServerManager : Form
  {
    private ServiceController service;
    private Button startBtn;
    private Button stopBtn;
    private Button closeBtn;
    private Label statusLabel;
    private TextBox serviceTextBox;
    private Label serviceLabel;
    private TextBox statusValueLabel;
    private StandardIconButton refreshStatusBtn;
    private System.ComponentModel.Container components;

    public DbServerManager()
    {
      this.InitializeComponent();
      this.BackColor = EllieMae.EMLite.AdminTools.AdminTools.FormBackgroundColor;
      string databaseServer = EnConfigurationSettings.GlobalSettings.DatabaseServer;
      int num = databaseServer.LastIndexOf("\\");
      if (num >= 0)
        this.serviceTextBox.Text = "MSSQL$" + databaseServer.Substring(num + 1);
      else
        this.serviceTextBox.Text = "MSSQL";
      this.service = new ServiceController(this.serviceTextBox.Text);
      this.statusValueLabel.Text = this.getStatus();
    }

    protected override void Dispose(bool disposing)
    {
      if (this.service != null)
        this.service.Close();
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DbServerManager));
      this.serviceTextBox = new TextBox();
      this.startBtn = new Button();
      this.stopBtn = new Button();
      this.serviceLabel = new Label();
      this.closeBtn = new Button();
      this.statusLabel = new Label();
      this.statusValueLabel = new TextBox();
      this.refreshStatusBtn = new StandardIconButton();
      ((ISupportInitialize) this.refreshStatusBtn).BeginInit();
      this.SuspendLayout();
      this.serviceTextBox.Location = new Point(93, 10);
      this.serviceTextBox.Name = "serviceTextBox";
      this.serviceTextBox.ReadOnly = true;
      this.serviceTextBox.Size = new Size(196, 20);
      this.serviceTextBox.TabIndex = 0;
      this.serviceTextBox.TabStop = false;
      this.serviceTextBox.Text = "MSSQL$EMMSDE";
      this.startBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.startBtn.Location = new Point(142, 74);
      this.startBtn.Name = "startBtn";
      this.startBtn.Size = new Size(75, 22);
      this.startBtn.TabIndex = 1;
      this.startBtn.Text = "Start";
      this.startBtn.Click += new EventHandler(this.startBtn_Click);
      this.stopBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.stopBtn.Location = new Point(67, 74);
      this.stopBtn.Name = "stopBtn";
      this.stopBtn.Size = new Size(75, 22);
      this.stopBtn.TabIndex = 2;
      this.stopBtn.Text = "Stop";
      this.stopBtn.Click += new EventHandler(this.stopBtn_Click);
      this.serviceLabel.AutoSize = true;
      this.serviceLabel.Location = new Point(12, 11);
      this.serviceLabel.Name = "serviceLabel";
      this.serviceLabel.Size = new Size(74, 14);
      this.serviceLabel.TabIndex = 3;
      this.serviceLabel.Text = "Service Name";
      this.closeBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.closeBtn.Location = new Point(217, 74);
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new Size(75, 22);
      this.closeBtn.TabIndex = 6;
      this.closeBtn.Text = "Close";
      this.closeBtn.Click += new EventHandler(this.closeBtn_Click);
      this.statusLabel.AutoSize = true;
      this.statusLabel.Location = new Point(12, 35);
      this.statusLabel.Name = "statusLabel";
      this.statusLabel.Size = new Size(38, 14);
      this.statusLabel.TabIndex = 8;
      this.statusLabel.Text = "Status";
      this.statusValueLabel.Location = new Point(93, 32);
      this.statusValueLabel.Name = "statusValueLabel";
      this.statusValueLabel.ReadOnly = true;
      this.statusValueLabel.Size = new Size(175, 20);
      this.statusValueLabel.TabIndex = 9;
      this.statusValueLabel.TabStop = false;
      this.refreshStatusBtn.BackColor = Color.Transparent;
      this.refreshStatusBtn.Location = new Point(272, 33);
      this.refreshStatusBtn.Name = "refreshStatusBtn";
      this.refreshStatusBtn.Size = new Size(16, 16);
      this.refreshStatusBtn.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.refreshStatusBtn.TabIndex = 10;
      this.refreshStatusBtn.TabStop = false;
      this.refreshStatusBtn.Click += new EventHandler(this.refreshStatusBtn_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(301, 105);
      this.Controls.Add((Control) this.refreshStatusBtn);
      this.Controls.Add((Control) this.statusValueLabel);
      this.Controls.Add((Control) this.statusLabel);
      this.Controls.Add((Control) this.closeBtn);
      this.Controls.Add((Control) this.serviceLabel);
      this.Controls.Add((Control) this.stopBtn);
      this.Controls.Add((Control) this.startBtn);
      this.Controls.Add((Control) this.serviceTextBox);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (DbServerManager);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "SQL Service Manager";
      ((ISupportInitialize) this.refreshStatusBtn).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private string getStatus()
    {
      try
      {
        return SystemUtil.GetServiceStatusAsString(this.service);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error retrieving service status: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return "Unknown";
      }
    }

    private void startBtn_Click(object sender, EventArgs e)
    {
      this.statusValueLabel.Text = "Starting...";
      try
      {
        if (this.service.Status == ServiceControllerStatus.Stopped)
          SystemUtil.StartService(this.service);
        if (this.service.Status != ServiceControllerStatus.Running)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The server could not be started or stopped immediately.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.statusValueLabel.Text = this.getStatus();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.statusValueLabel.Text = "Unknown";
      }
    }

    private void stopBtn_Click(object sender, EventArgs e)
    {
      this.statusValueLabel.Text = "Stopping...";
      try
      {
        if (this.service.Status == ServiceControllerStatus.Running)
          SystemUtil.StopService(this.service);
        if (this.service.Status != ServiceControllerStatus.Stopped)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The server could not be stopped.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.statusValueLabel.Text = this.getStatus();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.statusValueLabel.Text = "Unknown";
      }
    }

    private void closeBtn_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void refreshStatusBtn_Click(object sender, EventArgs e)
    {
      this.statusValueLabel.Text = this.getStatus();
      SaveConfirmScreen.Show((IWin32Window) this, "Data refreshed.");
    }

    private void statusValueLabel_TextChanged(object sender, EventArgs e)
    {
      if ("Stopped".Equals(this.statusValueLabel.Text))
      {
        this.startBtn.Enabled = true;
        this.stopBtn.Enabled = false;
      }
      else if ("Running".Equals(this.statusValueLabel.Text))
      {
        this.startBtn.Enabled = false;
        this.stopBtn.Enabled = true;
      }
      else
      {
        this.startBtn.Enabled = false;
        this.stopBtn.Enabled = false;
      }
    }
  }
}
