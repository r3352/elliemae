// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.eCloseSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.LoanUtils.Authentication;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class eCloseSetupControl : UserControl
  {
    private Sessions.Session session;
    internal eCloseRestClient _client;
    private WebHost webHost;
    private IContainer components;
    internal GroupContainer gcFulfillment;
    internal Button btnGetSetup;

    public eCloseSetupControl(Sessions.Session session)
    {
      this.session = session;
      this.webHost = new WebHost("sc");
      this.webHost.Dock = DockStyle.Fill;
      this.InitializeComponent();
      this.gcFulfillment.Controls.Add((Control) this.webHost);
      this.buildScreen();
    }

    private void buildScreen()
    {
      string str = Session.SessionObjects?.StartupInfo?.ServiceUrls?.EcloseSetupUrl;
      if (string.IsNullOrWhiteSpace(str) || !Uri.IsWellFormedUriString(str, UriKind.Absolute))
        str = "https://www.icemortgagetechnology.com/resources/eclosing?ad_zone=az103&ad_uid=blt541c9fe931d5130f";
      this.btnGetSetup.Visible = false;
      SetupPage setupPage = SetupPage.GetSetupPage("Encompass eClose Setup");
      ModuleLicense moduleLic = this.getModuleLic();
      if (moduleLic != null && !moduleLic.Disabled)
      {
        this.webHost.Navigate(this.getNavigationURLWithToken());
        setupPage.setSubtitleText("Learn more about getting started with Encompass eClose, check out the information, guides and tools on the Resource Center.");
        this._client = new eCloseRestClient();
        string lenderOrganizationId = this._client.GetLenderOrganizationId();
        if (string.IsNullOrWhiteSpace(lenderOrganizationId))
        {
          this.gcFulfillment.Text = "Encompass eClose - Setup Required";
          this.btnGetSetup.Visible = true;
        }
        else if (!string.IsNullOrWhiteSpace(lenderOrganizationId) && !lenderOrganizationId.Contains("Invalid"))
          this.gcFulfillment.Text = "Encompass eClose - Setup Complete (" + lenderOrganizationId + ")";
        else
          this.gcFulfillment.Text = "Encompass eClose - Setup Error (" + lenderOrganizationId + ")";
      }
      else
      {
        this.webHost.Navigate(str);
        setupPage.setSubtitleText("Want to experience faster closing times, improve investor delivery and enhance borrower experience? Learn more about Encompass eClose, and join the eClose community");
        this.gcFulfillment.Text = "Encompass eClose - Signup Required";
      }
    }

    private ModuleLicense getModuleLic()
    {
      try
      {
        return Modules.GetModuleLicense(EncompassModule.eClose, this.session);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(this.GetType().Name, "Exception while getting getModuleLic: " + (object) ex);
        return (ModuleLicense) null;
      }
    }

    private void btnGetSetup_Click(object sender, EventArgs e)
    {
      using (ECloseOrgSetupDialog ecloseOrgSetupDialog = new ECloseOrgSetupDialog(this.session, this))
      {
        int num = (int) ecloseOrgSetupDialog.ShowDialog((IWin32Window) this);
      }
    }

    private string getNavigationURLWithToken()
    {
      string navigationUrlWithToken = this.session.StartupInfo.ResourceCenterUrl + "/resourcecenter/default.aspx";
      try
      {
        AccessToken accessToken = new OAuth2Utils(this.session.SessionObjects.Session, this.session.StartupInfo).GetAccessToken("rc", true);
        if (accessToken != null)
          navigationUrlWithToken = navigationUrlWithToken + "?authToken=" + HttpUtility.UrlEncode(accessToken.Token) + "&target=encompasseclose.aspx";
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(this.GetType().Name, "Exception while getting getNavigationURLWithToken: " + (object) ex);
      }
      return navigationUrlWithToken;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcFulfillment = new GroupContainer();
      this.btnGetSetup = new Button();
      this.gcFulfillment.SuspendLayout();
      this.SuspendLayout();
      this.gcFulfillment.Controls.Add((Control) this.btnGetSetup);
      this.gcFulfillment.Dock = DockStyle.Fill;
      this.gcFulfillment.HeaderForeColor = SystemColors.ControlText;
      this.gcFulfillment.Location = new Point(0, 0);
      this.gcFulfillment.Name = "gcFulfillment";
      this.gcFulfillment.Size = new Size(676, 295);
      this.gcFulfillment.TabIndex = 0;
      this.gcFulfillment.Text = "eClose";
      this.btnGetSetup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnGetSetup.Location = new Point(590, 2);
      this.btnGetSetup.Margin = new Padding(0);
      this.btnGetSetup.Name = "btnGetSetup";
      this.btnGetSetup.Size = new Size(77, 22);
      this.btnGetSetup.TabIndex = 27;
      this.btnGetSetup.Text = "Get Setup";
      this.btnGetSetup.Click += new EventHandler(this.btnGetSetup_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcFulfillment);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (eCloseSetupControl);
      this.Size = new Size(676, 295);
      this.gcFulfillment.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
