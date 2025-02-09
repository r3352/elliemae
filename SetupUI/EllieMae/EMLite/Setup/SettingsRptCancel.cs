// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingsRptCancel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SettingsRptCancel : UserControl
  {
    private string reportID;
    private IContainer components;
    private EllieMae.EMLite.UI.LinkLabel cancel_lbl;

    public SettingsRptCancel(
      string reportID,
      SettingsRptJobInfo.jobStatus status,
      bool userCreatedReport)
    {
      this.InitializeComponent();
      this.reportID = reportID;
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (status.Equals((object) SettingsRptJobInfo.jobStatus.Canceling))
        this.cancel_lbl.Enabled = false;
      else if (userCreatedReport)
      {
        if (!aclManager.GetUserApplicationRight(AclFeature.CancelSettings_Report))
          this.cancel_lbl.Enabled = false;
        if (!aclManager.GetUserApplicationRight(AclFeature.CancelOtherSettings_Report))
          return;
        this.cancel_lbl.Enabled = true;
      }
      else
      {
        if (aclManager.GetUserApplicationRight(AclFeature.CancelOtherSettings_Report))
          return;
        this.cancel_lbl.Enabled = false;
      }
    }

    private void cancel_lbl_Click(object sender, EventArgs e)
    {
      bool flag = false;
      SettingsRptJobQueue settingsRptJobQueue = (SettingsRptJobQueue) this.TopLevelControl.Controls.Find("SettingsRptJobQueue", true)[0];
      SettingsRptJobInfo settingsRptInfo1 = settingsRptJobQueue.curSession.ReportManager.GetSettingsRptInfo(this.reportID);
      if (MessageBox.Show((IWin32Window) this, "Are you sure you want to cancel this report request?", "Cancel " + settingsRptInfo1.ReportName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      if (settingsRptInfo1.Status.Equals((object) SettingsRptJobInfo.jobStatus.Submitted))
        flag = settingsRptJobQueue.curSession.ReportManager.CancelSettingsRptJobs(this.reportID, settingsRptJobQueue.curSession.UserID, true);
      else if (settingsRptInfo1.Status.Equals((object) SettingsRptJobInfo.jobStatus.InProgress))
        flag = settingsRptJobQueue.curSession.ReportManager.CancelSettingsRptJobs(this.reportID, settingsRptJobQueue.curSession.UserID, false);
      if (!flag)
      {
        SettingsRptJobInfo settingsRptInfo2 = settingsRptJobQueue.curSession.ReportManager.GetSettingsRptInfo(this.reportID);
        if (settingsRptInfo2.Status.Equals((object) SettingsRptJobInfo.jobStatus.Completed))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Completed reports cannot be canceled.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        if (settingsRptInfo2.Status.Equals((object) SettingsRptJobInfo.jobStatus.Failed))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Report " + settingsRptInfo2.ReportName + " can not be cancelled.  It has returned with errors.  Please check log for more information.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
      }
      settingsRptJobQueue.refreshJobQueue();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cancel_lbl = new EllieMae.EMLite.UI.LinkLabel();
      this.SuspendLayout();
      this.cancel_lbl.AutoSize = true;
      this.cancel_lbl.Font = new Font("Microsoft Sans Serif", 7f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cancel_lbl.Location = new Point(3, 0);
      this.cancel_lbl.Name = "cancel_lbl";
      this.cancel_lbl.Size = new Size(45, 15);
      this.cancel_lbl.TabIndex = 1;
      this.cancel_lbl.Text = "Cancel";
      this.cancel_lbl.Click += new EventHandler(this.cancel_lbl_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.cancel_lbl);
      this.Name = nameof (SettingsRptCancel);
      this.Size = new Size(309, 20);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
