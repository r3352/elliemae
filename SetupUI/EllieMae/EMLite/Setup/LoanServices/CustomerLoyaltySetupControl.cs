// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanServices.CustomerLoyaltySetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.LoanServices
{
  public class CustomerLoyaltySetupControl : UserControl
  {
    private IContainer components;
    private BrowserControl browser;

    public CustomerLoyaltySetupControl(bool clientSettings)
    {
      this.InitializeComponent();
      string uriString = Session.SessionObjects?.StartupInfo?.ServiceUrls?.CustomerLoyalty;
      if (string.IsNullOrWhiteSpace(uriString) || !Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
        uriString = "https://www.epassbusinesscenter.com/customerloyalty/";
      this.browser.ShowToolbar = false;
      this.browser.HomeUrl = clientSettings ? uriString + "clientsettings.asp" : uriString + "usersettings.asp";
      this.browser.OfflinePage = SystemSettings.EpassDir + "Epass.htm";
      this.browser.GoHome();
    }

    private void browser_BeforeNavigate(object sender, WebBrowserNavigatingEventArgs e)
    {
      string url1 = e.Url.ToString();
      if (!url1.Contains("_LOANSERVICE_SIGNATURE;"))
        return;
      string url2 = LoanServiceManager.ProcessUrl((LoanDataMgr) null, url1);
      if (url2 != null)
        this.browser.Navigate(url2);
      e.Cancel = true;
    }

    private void browser_LoginUser(object sender, LoginUserEventArgs e)
    {
      e.Response = EpassLogin.LoginRequired(e.ShowDialogs);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.browser = new BrowserControl();
      this.SuspendLayout();
      this.browser.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.browser.Dock = DockStyle.Fill;
      this.browser.Location = new Point(0, 0);
      this.browser.Name = "browser";
      this.browser.ShowToolbar = false;
      this.browser.Size = new Size(339, 224);
      this.browser.TabIndex = 0;
      this.browser.LoginUser += new LoginUserEventHandler(this.browser_LoginUser);
      this.browser.BeforeNavigate += new WebBrowserNavigatingEventHandler(this.browser_BeforeNavigate);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.browser);
      this.Name = nameof (CustomerLoyaltySetupControl);
      this.Size = new Size(339, 224);
      this.ResumeLayout(false);
    }
  }
}
