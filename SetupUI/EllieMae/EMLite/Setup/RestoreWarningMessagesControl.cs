// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.RestoreWarningMessagesControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class RestoreWarningMessagesControl : SettingsUserControl
  {
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer gcHomeScreen;
    private GroupContainer gcEFolder;
    private CheckBox chkLogoutPrompt;
    private CheckBox chkDeleteAttachmentPrompt;

    public RestoreWarningMessagesControl(Sessions.Session session, SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.init();
    }

    private void init()
    {
      this.chkLogoutPrompt.Checked = Session.GetPrivateProfileString("Dialog", "Logout") != "OFF";
      this.chkDeleteAttachmentPrompt.Checked = Session.GetPrivateProfileString("Dialog", "eFolderDeleteAttachment") != "OFF";
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      Session.WritePrivateProfileString("Dialog", "Logout", this.chkLogoutPrompt.Checked ? "ON" : "OFF");
      Session.WritePrivateProfileString("Dialog", "eFolderDeleteAttachment", this.chkDeleteAttachmentPrompt.Checked ? "ON" : "OFF");
      this.setDirtyFlag(false);
    }

    public override void Reset() => this.init();

    private void RestoreWarningMessagesControl_Resize(object sender, EventArgs e)
    {
      this.gcEFolder.Height = this.ClientSize.Height * 9 / 10;
    }

    private void chkLogoutPrompt_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void chkDeleteAttachmentPrompt_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcHomeScreen = new GroupContainer();
      this.chkLogoutPrompt = new CheckBox();
      this.gcEFolder = new GroupContainer();
      this.chkDeleteAttachmentPrompt = new CheckBox();
      this.gcHomeScreen.SuspendLayout();
      this.gcEFolder.SuspendLayout();
      this.SuspendLayout();
      this.gcHomeScreen.Controls.Add((Control) this.chkLogoutPrompt);
      this.gcHomeScreen.Dock = DockStyle.Fill;
      this.gcHomeScreen.HeaderForeColor = SystemColors.ControlText;
      this.gcHomeScreen.Location = new Point(0, 0);
      this.gcHomeScreen.Name = "gcHomeScreen";
      this.gcHomeScreen.Size = new Size(468, 317);
      this.gcHomeScreen.TabIndex = 0;
      this.gcHomeScreen.Text = "Log Out";
      this.chkLogoutPrompt.AutoSize = true;
      this.chkLogoutPrompt.Location = new Point(22, 43);
      this.chkLogoutPrompt.Name = "chkLogoutPrompt";
      this.chkLogoutPrompt.Size = new Size(235, 17);
      this.chkLogoutPrompt.TabIndex = 0;
      this.chkLogoutPrompt.Text = "Logout prompt when exiting Encompass";
      this.chkLogoutPrompt.UseVisualStyleBackColor = true;
      this.chkLogoutPrompt.CheckedChanged += new EventHandler(this.chkLogoutPrompt_CheckedChanged);
      this.gcEFolder.Controls.Add((Control) this.chkDeleteAttachmentPrompt);
      this.gcEFolder.Dock = DockStyle.Bottom;
      this.gcEFolder.HeaderForeColor = SystemColors.ControlText;
      this.gcEFolder.Location = new Point(0, 91);
      this.gcEFolder.Name = "gcEFolder";
      this.gcEFolder.Size = new Size(468, 226);
      this.gcEFolder.TabIndex = 1;
      this.gcEFolder.Text = "eFolder File Manager";
      this.chkDeleteAttachmentPrompt.AutoSize = true;
      this.chkDeleteAttachmentPrompt.Location = new Point(22, 45);
      this.chkDeleteAttachmentPrompt.Name = "chkDeleteAttachmentPrompt";
      this.chkDeleteAttachmentPrompt.Size = new Size(422, 17);
      this.chkDeleteAttachmentPrompt.TabIndex = 1;
      this.chkDeleteAttachmentPrompt.Text = "Delete prompt for image attachments when all pages have been assigned or deleted";
      this.chkDeleteAttachmentPrompt.UseVisualStyleBackColor = true;
      this.chkDeleteAttachmentPrompt.CheckedChanged += new EventHandler(this.chkDeleteAttachmentPrompt_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcEFolder);
      this.Controls.Add((Control) this.gcHomeScreen);
      this.Name = nameof (RestoreWarningMessagesControl);
      this.Size = new Size(468, 317);
      this.Resize += new EventHandler(this.RestoreWarningMessagesControl_Resize);
      this.gcHomeScreen.ResumeLayout(false);
      this.gcHomeScreen.PerformLayout();
      this.gcEFolder.ResumeLayout(false);
      this.gcEFolder.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
