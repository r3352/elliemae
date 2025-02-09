// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.SettingsRptAction
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Reporting.UserGroups;
using EllieMae.EMLite.Setup;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite
{
  public class SettingsRptAction : UserControl
  {
    private string reportID;
    private string userID;
    private string reportName;
    private DateTime requestDT;
    private SettingsRptJobInfo.jobType jobType;
    private IContainer components;
    private EllieMae.EMLite.UI.LinkLabel download_lbl;
    private EllieMae.EMLite.UI.LinkLabel rerun_lbl;
    private EllieMae.EMLite.UI.LinkLabel viewLog;

    public SettingsRptAction(
      SettingsRptJobInfo.jobType jobType,
      string reportId,
      string reportName,
      string userId,
      DateTime requestDt,
      SettingsRptJobInfo.jobStatus status)
    {
      this.InitializeComponent();
      if (!((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ViewNewSettings_Report))
        this.rerun_lbl.Enabled = false;
      if (status.Equals((object) SettingsRptJobInfo.jobStatus.Failed) || status.Equals((object) SettingsRptJobInfo.jobStatus.Canceled))
        this.download_lbl.Enabled = false;
      this.reportID = reportId;
      this.reportName = reportName;
      this.userID = userId;
      this.requestDT = requestDt;
      this.jobType = jobType;
    }

    private void download_lbl_Click(object sender, EventArgs e)
    {
      string path = Path.Combine(EnConfigurationSettings.GlobalSettings.AppSettingsReportDownloadsDirectory, (string.IsNullOrEmpty(this.reportName) || this.reportName.Length <= 20 ? this.reportName : this.reportName.Substring(0, 20)) + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx");
      if (this.jobType == SettingsRptJobInfo.jobType.Organization)
        new SettingsReportXLS(this.reportID).Save(path, true);
      else if (this.jobType == SettingsRptJobInfo.jobType.Persona)
      {
        new PersonaReportXLS(this.reportID).Save(path, true);
      }
      else
      {
        if (this.jobType != SettingsRptJobInfo.jobType.UserGroups)
          return;
        new UserGroupsReportXLS(this.reportID).Save(path, true);
      }
    }

    private void rerun_lbl_Click(object sender, EventArgs e)
    {
      SettingsRptJobQueue settingsRptJobQueue = (SettingsRptJobQueue) this.TopLevelControl.Controls.Find("SettingsRptJobQueue", true)[0];
      using (GenSettingsRptDlg genSettingsRptDlg = new GenSettingsRptDlg(settingsRptJobQueue.curSession, this.userID, this.reportID))
      {
        int num1 = (int) genSettingsRptDlg.ShowDialog((IWin32Window) this);
        if (genSettingsRptDlg.DialogResult == DialogResult.OK)
        {
          try
          {
            settingsRptJobQueue.refreshJobQueue();
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "A new report was not created. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        genSettingsRptDlg.Dispose();
      }
    }

    private void viewLog_Click(object sender, EventArgs e)
    {
      using (SettingsRptViewLog settingsRptViewLog = new SettingsRptViewLog(this.reportID))
      {
        int num = (int) settingsRptViewLog.ShowDialog((IWin32Window) this);
        settingsRptViewLog.Dispose();
      }
    }

    private void SettingsRptAction_Load(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.download_lbl = new EllieMae.EMLite.UI.LinkLabel();
      this.rerun_lbl = new EllieMae.EMLite.UI.LinkLabel();
      this.viewLog = new EllieMae.EMLite.UI.LinkLabel();
      this.SuspendLayout();
      this.download_lbl.AutoSize = true;
      this.download_lbl.Font = new Font("Microsoft Sans Serif", 7f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.download_lbl.Location = new Point(3, 0);
      this.download_lbl.Name = "download_lbl";
      this.download_lbl.Size = new Size(103, 15);
      this.download_lbl.TabIndex = 0;
      this.download_lbl.Text = "Download Report";
      this.download_lbl.Click += new EventHandler(this.download_lbl_Click);
      this.rerun_lbl.AutoSize = true;
      this.rerun_lbl.Font = new Font("Microsoft Sans Serif", 7f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rerun_lbl.Location = new Point(153, 0);
      this.rerun_lbl.Name = "rerun_lbl";
      this.rerun_lbl.Size = new Size(45, 15);
      this.rerun_lbl.TabIndex = 1;
      this.rerun_lbl.Text = "Re-run";
      this.rerun_lbl.Click += new EventHandler(this.rerun_lbl_Click);
      this.viewLog.AutoSize = true;
      this.viewLog.Font = new Font("Microsoft Sans Serif", 7f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.viewLog.Location = new Point(235, 0);
      this.viewLog.Name = "viewLog";
      this.viewLog.Size = new Size(57, 15);
      this.viewLog.TabIndex = 2;
      this.viewLog.Text = "View Log";
      this.viewLog.Click += new EventHandler(this.viewLog_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.viewLog);
      this.Controls.Add((Control) this.rerun_lbl);
      this.Controls.Add((Control) this.download_lbl);
      this.Name = nameof (SettingsRptAction);
      this.Size = new Size(304, 22);
      this.Load += new EventHandler(this.SettingsRptAction_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
