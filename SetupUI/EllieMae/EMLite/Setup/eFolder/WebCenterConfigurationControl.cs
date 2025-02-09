// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.WebCenterConfigurationControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class WebCenterConfigurationControl : SettingsUserControl
  {
    private Sessions.Session session;
    private WebCenterSettings webCenterSettings;
    private IContainer components;
    private GroupContainer gcWebCenter;
    private Label lblWebCenterNote;
    private CheckBox chkUseSameWebCenter;
    private Label lblWebCenter;
    private Label lblWebCenterNote2;
    private Label lblWebCenter1;

    public WebCenterConfigurationControl(Sessions.Session session, SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.webCenterSettings = this.session.ConfigurationManager.GetWebCenterSettings();
      this.initWebCenterSettings();
    }

    private void initWebCenterSettings()
    {
      this.chkUseSameWebCenter.Checked = this.webCenterSettings.UseSameWebcenter;
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      if (this.webCenterSettings.UseSameWebcenter != this.chkUseSameWebCenter.Checked)
      {
        this.webCenterSettings.UseSameWebcenter = this.chkUseSameWebCenter.Checked;
        this.session.ConfigurationManager.SaveWebCenterSettings(this.webCenterSettings);
      }
      this.setDirtyFlag(false);
    }

    public override void Reset() => this.initWebCenterSettings();

    private void chkUseSameWebCenter_CheckedChanged(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (WebCenterConfigurationControl));
      this.gcWebCenter = new GroupContainer();
      this.lblWebCenterNote2 = new Label();
      this.lblWebCenter1 = new Label();
      this.lblWebCenterNote = new Label();
      this.chkUseSameWebCenter = new CheckBox();
      this.lblWebCenter = new Label();
      this.gcWebCenter.SuspendLayout();
      this.SuspendLayout();
      this.gcWebCenter.Controls.Add((Control) this.lblWebCenterNote2);
      this.gcWebCenter.Controls.Add((Control) this.lblWebCenter1);
      this.gcWebCenter.Controls.Add((Control) this.lblWebCenterNote);
      this.gcWebCenter.Controls.Add((Control) this.chkUseSameWebCenter);
      this.gcWebCenter.Controls.Add((Control) this.lblWebCenter);
      this.gcWebCenter.Dock = DockStyle.Fill;
      this.gcWebCenter.HeaderForeColor = SystemColors.ControlText;
      this.gcWebCenter.Location = new Point(0, 0);
      this.gcWebCenter.Name = "gcWebCenter";
      this.gcWebCenter.Size = new Size(500, 380);
      this.gcWebCenter.TabIndex = 0;
      this.gcWebCenter.Text = "WebCenter Configuration";
      this.lblWebCenterNote2.AutoSize = true;
      this.lblWebCenterNote2.Location = new Point(13, 161);
      this.lblWebCenterNote2.Name = "lblWebCenterNote2";
      this.lblWebCenterNote2.Size = new Size(569, 13);
      this.lblWebCenterNote2.TabIndex = 4;
      this.lblWebCenterNote2.Text = "When using individual WebCenter sites in this manner, borrowers will be required to create log in accounts for each site.";
      this.lblWebCenter1.AutoSize = true;
      this.lblWebCenter1.Location = new Point(13, 66);
      this.lblWebCenter1.Name = "lblWebCenter1";
      this.lblWebCenter1.Size = new Size(593, 13);
      this.lblWebCenter1.TabIndex = 3;
      this.lblWebCenter1.Text = "The WebCenter site belonging to the user who sends the first update, document request, or disclosure package will be used.";
      this.lblWebCenterNote.AutoSize = true;
      this.lblWebCenterNote.Location = new Point(13, 143);
      this.lblWebCenterNote.Name = "lblWebCenterNote";
      this.lblWebCenterNote.Size = new Size(980, 13);
      this.lblWebCenterNote.TabIndex = 2;
      this.lblWebCenterNote.Text = componentResourceManager.GetString("lblWebCenterNote.Text");
      this.chkUseSameWebCenter.AutoSize = true;
      this.chkUseSameWebCenter.Location = new Point(13, 103);
      this.chkUseSameWebCenter.Name = "chkUseSameWebCenter";
      this.chkUseSameWebCenter.Size = new Size(452, 17);
      this.chkUseSameWebCenter.TabIndex = 1;
      this.chkUseSameWebCenter.Text = "Borrower will use one WebCenter site to retrieve, send, and return updates and documents";
      this.chkUseSameWebCenter.UseVisualStyleBackColor = true;
      this.chkUseSameWebCenter.CheckedChanged += new EventHandler(this.chkUseSameWebCenter_CheckedChanged);
      this.lblWebCenter.AutoSize = true;
      this.lblWebCenter.Location = new Point(13, 46);
      this.lblWebCenter.Name = "lblWebCenter";
      this.lblWebCenter.Size = new Size(1055, 13);
      this.lblWebCenter.TabIndex = 0;
      this.lblWebCenter.Text = componentResourceManager.GetString("lblWebCenter.Text");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcWebCenter);
      this.Name = nameof (WebCenterConfigurationControl);
      this.Size = new Size(500, 380);
      this.gcWebCenter.ResumeLayout(false);
      this.gcWebCenter.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
