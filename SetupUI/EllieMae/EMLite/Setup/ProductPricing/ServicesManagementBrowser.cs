// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ProductPricing.ServicesManagementBrowser
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ProductPricing
{
  public class ServicesManagementBrowser : Form
  {
    private const string className = "ServicesManagementBrowser";
    private static readonly string sw = Tracing.SwEFolder;
    private LoadWebPageControl loadWebPageControl;
    private IContainer components;

    public ServicesManagementBrowser()
    {
      this.InitializeComponent();
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      webPageParams.Add("oapiBaseUrl", (object) Session.StartupInfo.OAPIGatewayBaseUri);
      webPageParams.Add("hostapplication", (object) "smartclient");
      webPageParams.Add("expandedcategory", (object) "PRODUCTPRICING");
      string appSetting1 = EnConfigurationSettings.AppSettings["ThinClientServicesManagement.Url"];
      string webPageURL = string.IsNullOrEmpty(appSetting1) ? Session.SessionObjects.ConfigurationManager.GetServicesManagementThinClientURL("ThinClientServicesManagement.Url") : appSetting1;
      Tracing.Log(ServicesManagementBrowser.sw, TraceLevel.Verbose, nameof (ServicesManagementBrowser), "showBrowser appUrl: " + webPageURL);
      string appSetting2 = EnConfigurationSettings.AppSettings["ThinClientServicesManagementHost.Url"];
      string hostPageUrl = string.IsNullOrEmpty(appSetting2) ? Session.SessionObjects.ConfigurationManager.GetServicesManagementThinClientURL("ThinClientServicesManagementHost.Url") : appSetting2;
      Tracing.Log(ServicesManagementBrowser.sw, TraceLevel.Verbose, nameof (ServicesManagementBrowser), "showBrowser hostUrl: " + hostPageUrl);
      this.loadWebPageControl = new LoadWebPageControl(webPageURL, webPageParams, "lp", hostPageUrl);
      this.Controls.Add((Control) this.loadWebPageControl);
      this.loadWebPageControl.Dock = DockStyle.Fill;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ServicesManagementBrowser));
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.ClientSize = new Size(1650, 1000);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ServicesManagementBrowser);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Services Management";
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.ResumeLayout(false);
    }
  }
}
