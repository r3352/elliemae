// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.EncompassLoginBrowser
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common.UI.Controls.ThinThick;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class EncompassLoginBrowser : UserControl
  {
    private readonly ThinThickBrowserManager _browserManager;
    private IContainer components;
    private ThinThickBrowser Browser;

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public string DocumentTitle => this._browserManager.Browser.DocumentTitle;

    public string Username { get; private set; }

    public string Password { get; private set; }

    public string InstanceID { get; private set; }

    public event EncompassLoginBrowser.WebBrowserTitleChangedHandler WebBrowserDocumentTitleChanged;

    public event EncompassLoginBrowser.WebBrowserBeforeNavigateHandler BeforeNavigate;

    public event EncompassLoginBrowser.WebBrowserNavigatedHandler Navigated;

    public EncompassLoginBrowser(string title = "")
    {
      this.InitializeComponent();
      this._browserManager = new ThinThickBrowserManager(Session.DefaultInstance, this.Browser, (IWin32Window) this);
      this._browserManager.WebBrowserDocumentTitleChanged += new ThinThickBrowserManager.WebBrowserTitleChangedHandler(this._browserManager_WebBrowserDocumentTitleChanged);
      this._browserManager.BeforeNavigate += new ThinThickBrowserManager.WebBrowserBeforeNavigateHandler(this._browserManager_BeforeNavigate);
      this._browserManager.Navigated += new ThinThickBrowserManager.WebBrowserNavigatedHandler(this._browserManager_Navigated);
      this._browserManager.SupressScriptErrors();
    }

    private void _browserManager_Navigated(object sender, WebBrowserNavigatedEventArgs e)
    {
      if (this.Navigated == null)
        return;
      this.Navigated(sender, e);
    }

    private void _browserManager_BeforeNavigate(object sender, WebBrowserNavigatingEventArgs e)
    {
      if (this.BeforeNavigate == null)
        return;
      this.BeforeNavigate(sender, e);
    }

    private void _browserManager_WebBrowserDocumentTitleChanged(object sender, EventArgs e)
    {
      if (this.WebBrowserDocumentTitleChanged == null)
        return;
      this.WebBrowserDocumentTitleChanged(sender, e);
    }

    public void Navigate(string url) => this._browserManager.Navigate(url);

    public void LoadHtml(string html) => this._browserManager.LoadHtml(html);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.Browser = new ThinThickBrowser();
      this.SuspendLayout();
      this.Browser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.Browser.AutoScroll = true;
      this.Browser.AutoSize = true;
      this.Browser.Location = new Point(0, 0);
      this.Browser.Name = "Browser";
      this.Browser.Size = new Size(0, 0);
      this.Browser.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.AutoSize = true;
      this.Controls.Add((Control) this.Browser);
      this.Name = "InvestorSubmissionStatusBrowser";
      this.Size = new Size(0, 0);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public delegate void WebBrowserTitleChangedHandler(object sender, EventArgs e);

    public delegate void WebBrowserBeforeNavigateHandler(
      object sender,
      WebBrowserNavigatingEventArgs e);

    public delegate void WebBrowserNavigatedHandler(object sender, WebBrowserNavigatedEventArgs e);
  }
}
