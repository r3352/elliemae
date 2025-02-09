// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.InsightsSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host.BrowserControls;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class InsightsSetupControl : SettingsUserControl
  {
    private Sessions.Session session;
    private string url;
    private string authString = "";
    private IContainer components;
    private GroupContainer groupContainer1;
    private EncWebFormBrowserControl webBrowser1;
    private Button button1;

    public InsightsSetupControl(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.url = string.IsNullOrEmpty(session.StartupInfo.InsightsSetupUrl) ? EnConfigurationSettings.AppSettings["InsightsSetup.Url"] : session.StartupInfo.InsightsSetupUrl;
      try
      {
        this.authString = new OAuth2(session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(session.SessionObjects)).GetAccessToken(session.StartupInfo.ServerInstanceName, session.SessionID, "ia").access_token;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Unable to get access token: " + ex.Message, "Encompass");
        return;
      }
      this.webBrowser1.Navigate(this.url + "/settings/home?authorization=" + this.authString);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Process.Start(this.url + "/settings?authorization=" + this.authString);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.button1 = new Button();
      this.webBrowser1 = BrowserFactory.GetWebBrowserInstance();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.button1);
      this.groupContainer1.Controls.Add((Control) this.webBrowser1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(620, 440);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "ICE Mortgage Technology Insights - Setup Required";
      this.button1.AccessibleRole = AccessibleRole.None;
      this.button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.button1.ImageAlign = ContentAlignment.MiddleRight;
      this.button1.Location = new Point(532, 2);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 21);
      this.button1.TabIndex = 1;
      this.button1.Text = "Setup";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.webBrowser1.Dock = DockStyle.Fill;
      this.webBrowser1.Location = new Point(1, 26);
      this.webBrowser1.MinimumSize = new Size(20, 20);
      this.webBrowser1.Name = "webBrowser1";
      this.webBrowser1.Size = new Size(618, 413);
      this.webBrowser1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (InsightsSetupControl);
      this.Size = new Size(620, 440);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
