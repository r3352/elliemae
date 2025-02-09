// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.VersionManagerNotification
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class VersionManagerNotification : Form
  {
    private IServerManager serverManager;
    private bool suspendEvent;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Label label1;
    private Label label8;
    private TextBox textBoxSmuSettingNotificationMinTime;
    private Label label7;
    private RichTextBox msgRichTextBox;
    private Label label2;
    private Label label3;
    private Button btnInstallMajorUpdates;
    private Button btnClose;
    private Label label4;
    private TextBox textBoxSmuSettingNotificationSecTime;

    public VersionManagerNotification() => this.InitializeComponent();

    private void VersionManagerNotification_Load(object sender, EventArgs e)
    {
      this.resetSmuSettings();
      this.serverManager = Session.ServerManager;
      this.btnInstallMajorUpdates.Enabled = true;
    }

    private void resetSmuSettings()
    {
      IDictionary serverSettings = Session.ServerManager.GetServerSettings("SMU");
      int num = (int) serverSettings[(object) "SMU.SettingNotificationTime"];
      this.suspendEvent = true;
      this.textBoxSmuSettingNotificationMinTime.Text = string.Concat((object) (num / 60));
      this.textBoxSmuSettingNotificationSecTime.Text = string.Concat((object) (num % 60));
      this.suspendEvent = false;
      this.textBoxSmuSettingNotificationSecTime_TextChanged((object) null, (EventArgs) null);
      this.msgRichTextBox.Text = serverSettings[(object) "SMU.SettingNotificationMessage"].ToString();
    }

    private void updateSmuSettings()
    {
      Session.ServerManager.UpdateServerSettings((IDictionary) new Dictionary<string, object>()
      {
        {
          "SMU.SettingNotificationTime",
          (object) (Utils.ParseInt((object) this.textBoxSmuSettingNotificationMinTime.Text, 0) * 60 + Utils.ParseInt((object) this.textBoxSmuSettingNotificationSecTime.Text, 0))
        },
        {
          "SMU.SettingNotificationMessage",
          (object) this.msgRichTextBox.Text
        }
      });
    }

    private void btnClose_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.No;

    private void textBoxSuSettingNotificationTime_TextChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.textBoxSmuSettingNotificationMinTime.Text.ToString() == "")
        this.textBoxSmuSettingNotificationMinTime.Text = "0";
      string text = this.textBoxSmuSettingNotificationMinTime.Text;
      if (!Utils.IsInt((object) text) || text.Substring(0, 1) == "0" && text.Length > 1 || Utils.ParseInt((object) text) < 0)
      {
        this.textBoxSmuSettingNotificationMinTime.Text = "0";
        int num = (int) Utils.Dialog((IWin32Window) this, "Minutes should be interger and greater than or equal to 0.");
      }
      else if (Utils.ParseInt((object) this.textBoxSmuSettingNotificationMinTime.Text, 0) * 60 + Utils.ParseInt((object) this.textBoxSmuSettingNotificationSecTime.Text) >= 0)
      {
        this.btnInstallMajorUpdates.Enabled = true;
      }
      else
      {
        int num = (int) MessageBox.Show("Please provide data greater than 0.");
        this.btnInstallMajorUpdates.Enabled = false;
      }
    }

    private void btnInstallMajorUpdates_Click(object sender, EventArgs e)
    {
      this.updateSmuSettings();
      this.DialogResult = DialogResult.OK;
    }

    private void textBoxSmuSettingNotificationSecTime_TextChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.textBoxSmuSettingNotificationSecTime.Text == "")
        this.textBoxSmuSettingNotificationSecTime.Text = "0";
      string text = this.textBoxSmuSettingNotificationSecTime.Text;
      if (!Utils.IsInt((object) text) || text.Substring(0, 1) == "0" && text.Length > 1 || Utils.ParseInt((object) text) < 0 || Utils.ParseInt((object) text) >= 60)
      {
        this.textBoxSmuSettingNotificationSecTime.Text = "0";
        int num = (int) Utils.Dialog((IWin32Window) this, "Seconds should be interger between 0 and 59");
      }
      else if (Utils.ParseInt((object) this.textBoxSmuSettingNotificationMinTime.Text, 0) * 60 + Utils.ParseInt((object) this.textBoxSmuSettingNotificationSecTime.Text) >= 0)
      {
        this.btnInstallMajorUpdates.Enabled = true;
      }
      else
      {
        int num = (int) MessageBox.Show("Please provide data greater than or eqaul to 0.");
        this.btnInstallMajorUpdates.Enabled = false;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (VersionManagerNotification));
      this.groupContainer1 = new GroupContainer();
      this.label4 = new Label();
      this.textBoxSmuSettingNotificationSecTime = new TextBox();
      this.msgRichTextBox = new RichTextBox();
      this.label2 = new Label();
      this.label8 = new Label();
      this.textBoxSmuSettingNotificationMinTime = new TextBox();
      this.label7 = new Label();
      this.label1 = new Label();
      this.label3 = new Label();
      this.btnInstallMajorUpdates = new Button();
      this.btnClose = new Button();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.textBoxSmuSettingNotificationSecTime);
      this.groupContainer1.Controls.Add((Control) this.msgRichTextBox);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.label8);
      this.groupContainer1.Controls.Add((Control) this.textBoxSmuSettingNotificationMinTime);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(17, 66);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(539, 232);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Notification";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(310, 36);
      this.label4.Name = "label4";
      this.label4.Size = new Size(197, 13);
      this.label4.TabIndex = 36;
      this.label4.Text = "seconds before the server update starts.";
      this.textBoxSmuSettingNotificationSecTime.Location = new Point(226, 29);
      this.textBoxSmuSettingNotificationSecTime.Name = "textBoxSmuSettingNotificationSecTime";
      this.textBoxSmuSettingNotificationSecTime.Size = new Size(77, 20);
      this.textBoxSmuSettingNotificationSecTime.TabIndex = 35;
      this.textBoxSmuSettingNotificationSecTime.TextChanged += new EventHandler(this.textBoxSmuSettingNotificationSecTime_TextChanged);
      this.msgRichTextBox.Location = new Point(17, 76);
      this.msgRichTextBox.Name = "msgRichTextBox";
      this.msgRichTextBox.Size = new Size(506, 141);
      this.msgRichTextBox.TabIndex = 34;
      this.msgRichTextBox.Text = "";
      this.label2.Location = new Point(17, 56);
      this.label2.Name = "label2";
      this.label2.Size = new Size(306, 17);
      this.label2.TabIndex = 33;
      this.label2.Text = "Announcement to online users:";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(173, 36);
      this.label8.Name = "label8";
      this.label8.Size = new Size(46, 13);
      this.label8.TabIndex = 32;
      this.label8.Text = "minutes ";
      this.textBoxSmuSettingNotificationMinTime.Location = new Point(89, 29);
      this.textBoxSmuSettingNotificationMinTime.Name = "textBoxSmuSettingNotificationMinTime";
      this.textBoxSmuSettingNotificationMinTime.Size = new Size(77, 20);
      this.textBoxSmuSettingNotificationMinTime.TabIndex = 31;
      this.textBoxSmuSettingNotificationMinTime.TextChanged += new EventHandler(this.textBoxSuSettingNotificationTime_TextChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(17, 36);
      this.label7.Name = "label7";
      this.label7.Size = new Size(65, 13);
      this.label7.TabIndex = 30;
      this.label7.Text = "Notify users ";
      this.label1.Location = new Point(17, 23);
      this.label1.Name = "label1";
      this.label1.Size = new Size(539, 40);
      this.label1.TabIndex = 0;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.label3.Location = new Point(14, 313);
      this.label3.Name = "label3";
      this.label3.Size = new Size(542, 40);
      this.label3.TabIndex = 1;
      this.label3.Text = "To install server updates, Encompass server will be terminated and restarted and all users including Admin Tools may be terminated as well. Do you want to install the updates?";
      this.btnInstallMajorUpdates.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnInstallMajorUpdates.Location = new Point(376, 362);
      this.btnInstallMajorUpdates.Margin = new Padding(2, 0, 2, 0);
      this.btnInstallMajorUpdates.Name = "btnInstallMajorUpdates";
      this.btnInstallMajorUpdates.Size = new Size(100, 22);
      this.btnInstallMajorUpdates.TabIndex = 2;
      this.btnInstallMajorUpdates.Text = "Install Updates";
      this.btnInstallMajorUpdates.UseVisualStyleBackColor = true;
      this.btnInstallMajorUpdates.Click += new EventHandler(this.btnInstallMajorUpdates_Click);
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(481, 362);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 10;
      this.btnClose.Text = "&Close";
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(575, 393);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnInstallMajorUpdates);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = nameof (VersionManagerNotification);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Version Management Notification";
      this.Load += new EventHandler(this.VersionManagerNotification_Load);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
