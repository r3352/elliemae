// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LogLockPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  internal class LogLockPanel : SettingsUserControl
  {
    private string logName = string.Empty;
    private string logLockPath = string.Empty;
    private const string printingLogPath = "Policies.ShowPrintingLog";
    private GroupContainer gcLog;
    private Splitter splitter1;
    private GroupContainer gcSecurity;
    private CheckBox chkBoxShowPrintingLogs;
    private CheckBox chkLockLog;
    private System.ComponentModel.Container components;

    internal LogLockPanel(SetUpContainer setupContainer, string logName)
      : base(setupContainer)
    {
      this.logName = logName;
      this.logLockPath = "Conversation Log" == logName ? "Policies.ConversationLogLock" : "Policies.GeneralLogLock";
      this.InitializeComponent();
      this.chkLockLog.Text = "Lock " + logName;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void reset()
    {
      this.chkLockLog.Checked = (bool) Session.ServerManager.GetServerSetting(this.logLockPath);
      this.chkBoxShowPrintingLogs.Checked = (bool) Session.ServerManager.GetServerSetting("Policies.ShowPrintingLog");
    }

    private void save()
    {
      Session.ServerManager.UpdateServerSetting(this.logLockPath, (object) this.chkLockLog.Checked);
      Session.ServerManager.UpdateServerSetting("Policies.ShowPrintingLog", (object) this.chkBoxShowPrintingLogs.Checked);
    }

    public override void Save()
    {
      this.save();
      this.setDirtyFlag(false);
    }

    public override void Reset()
    {
      this.reset();
      this.setDirtyFlag(false);
    }

    private void InitializeComponent()
    {
      this.chkLockLog = new CheckBox();
      this.gcLog = new GroupContainer();
      this.chkBoxShowPrintingLogs = new CheckBox();
      this.splitter1 = new Splitter();
      this.gcSecurity = new GroupContainer();
      this.gcLog.SuspendLayout();
      this.gcSecurity.SuspendLayout();
      this.SuspendLayout();
      this.chkLockLog.AutoSize = true;
      this.chkLockLog.Location = new Point(10, 32);
      this.chkLockLog.Name = "chkLockLog";
      this.chkLockLog.Size = new Size(136, 17);
      this.chkLockLog.TabIndex = 1;
      this.chkLockLog.Text = "Lock Conversation Log";
      this.chkLockLog.CheckedChanged += new EventHandler(this.chkLockLog_CheckedChanged);
      this.gcLog.Controls.Add((Control) this.chkBoxShowPrintingLogs);
      this.gcLog.Dock = DockStyle.Top;
      this.gcLog.Location = new Point(0, 0);
      this.gcLog.Name = "gcLog";
      this.gcLog.Size = new Size(736, 158);
      this.gcLog.TabIndex = 2;
      this.gcLog.Text = "Printing Log";
      this.chkBoxShowPrintingLogs.AutoSize = true;
      this.chkBoxShowPrintingLogs.Location = new Point(10, 32);
      this.chkBoxShowPrintingLogs.Name = "chkBoxShowPrintingLogs";
      this.chkBoxShowPrintingLogs.Size = new Size(166, 17);
      this.chkBoxShowPrintingLogs.TabIndex = 2;
      this.chkBoxShowPrintingLogs.Text = "Show Printing Records in Log";
      this.chkBoxShowPrintingLogs.CheckedChanged += new EventHandler(this.chkBoxShowPrintingLogs_CheckedChanged);
      this.splitter1.Dock = DockStyle.Top;
      this.splitter1.Location = new Point(0, 158);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(736, 3);
      this.splitter1.TabIndex = 3;
      this.splitter1.TabStop = false;
      this.gcSecurity.Controls.Add((Control) this.chkLockLog);
      this.gcSecurity.Dock = DockStyle.Fill;
      this.gcSecurity.Location = new Point(0, 161);
      this.gcSecurity.Name = "gcSecurity";
      this.gcSecurity.Size = new Size(736, 319);
      this.gcSecurity.TabIndex = 4;
      this.gcSecurity.Text = "Log Data Security";
      this.Controls.Add((Control) this.gcSecurity);
      this.Controls.Add((Control) this.splitter1);
      this.Controls.Add((Control) this.gcLog);
      this.Name = nameof (LogLockPanel);
      this.Size = new Size(736, 480);
      this.Load += new EventHandler(this.LogLockPanel_Load);
      this.gcLog.ResumeLayout(false);
      this.gcLog.PerformLayout();
      this.gcSecurity.ResumeLayout(false);
      this.gcSecurity.PerformLayout();
      this.ResumeLayout(false);
    }

    private void chkLockLog_CheckedChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void chkBoxShowPrintingLogs_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void LogLockPanel_Load(object sender, EventArgs e) => this.Reset();
  }
}
