// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.LoadWebPageForm
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Elli.Web.Host.Adapter;
using Elli.Web.Host.EventObjects;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host
{
  public class LoadWebPageForm : Form
  {
    private WebHost webHost;
    private string webPageURL = string.Empty;
    private Dictionary<string, object> webPageParams;
    private IContainer components;
    private Panel pnlBrowser;
    private Button btnConsole;

    public event LoadWebPageForm.ExecuteComplete executeComplete;

    public event LoadWebPageForm.TitleChangedEventDelegate TitleChanged;

    public LoadWebPageForm(
      string webPageURL,
      Dictionary<string, object> webPageParams,
      string scope,
      string title = "")
    {
      this.InitializeComponent();
      if (!string.IsNullOrEmpty(title))
        this.Text = title;
      this.webHost = new WebHost(scope, unloadHandler: new Action(this.UnloadHandler));
      this.webHost.ExecuteComplete += new WebHost.ExecuteCompleteEventHandler(this.webHost_executeComplete);
      this.webHost.BrowserTitleChanged += new EventHandler<TitleChangeEventArgs>(this.WebHost_TitleChanged);
      this.webHost.GuestFrameComplete += new WebHost.GuestFrameCompleteEventHandler(this.webHost_GuestPageLoadCompleted);
      this.webHost.HelpInvoked += new WebHost.HelpInvokedEventHandler(this.WebHost_helpBrowserInvoked);
      this.pnlBrowser.Controls.Add((Control) this.webHost);
      this.webHost.Dock = DockStyle.Fill;
      this.webPageURL = webPageURL;
      this.webPageParams = webPageParams;
      this.ControlBox = false;
      this.LoadModule();
      this.btnConsole.Visible = false;
      this.MaximizeBox = true;
      this.FormBorderStyle = FormBorderStyle.Sizable;
    }

    public LoadWebPageForm(string url, string title)
    {
      this.InitializeComponent();
      this.Text = title;
      this.btnConsole.Visible = false;
      this.webHost = new WebHost();
      this.pnlBrowser.Controls.Add((Control) this.webHost);
      this.webHost.Dock = DockStyle.Fill;
      this.webHost.Navigate(url);
    }

    private void WebHost_helpBrowserInvoked(object sender, InvokeHelpBrowserEventArgs args)
    {
      if (args == null || string.IsNullOrEmpty(args.helpBrowserUrl))
        return;
      this.pnlBrowser.Focus();
      JedHelp.ShowHelpPage(args.helpBrowserUrl);
    }

    private void WebHost_TitleChanged(object sender, TitleChangeEventArgs e)
    {
      if (this.TitleChanged == null)
        return;
      this.TitleChanged(sender, e);
    }

    public LoadWebPageForm(string title = "")
    {
      this.InitializeComponent();
      if (!string.IsNullOrEmpty(title))
        this.Text = title;
      this.webHost = new WebHost();
      this.webHost.ExecuteComplete += new WebHost.ExecuteCompleteEventHandler(this.webHost_executeComplete);
      this.webHost.BrowserTitleChanged += new EventHandler<TitleChangeEventArgs>(this.WebHost_TitleChanged);
      this.pnlBrowser.Controls.Add((Control) this.webHost);
      this.webHost.Dock = DockStyle.Fill;
      this.btnConsole.Visible = false;
      if (!this.DebugMode)
        return;
      this.EnableConsoleLogging();
    }

    private void webHost_executeComplete(object sender, ExecuteCompleteEventArgs e)
    {
      if (this.executeComplete == null)
        return;
      this.executeComplete((object) this, e);
    }

    private void webHost_GuestPageLoadCompleted(object sender, EventArgs args)
    {
      this.Invoke((Delegate) (() => this.ControlBox = true));
    }

    private bool DebugMode
    {
      get
      {
        bool debugMode = false;
        if (debugMode)
          return debugMode;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\EnableWebHostDebugMode"))
          return registryKey != null;
      }
    }

    private void EnableConsoleLogging() => this.btnConsole.Visible = true;

    public void Navigate(string webPageUrl)
    {
      this.webPageURL = webPageUrl;
      this.webHost.Navigate(webPageUrl);
    }

    public void ReloadModule() => this.LoadModule();

    private ModuleUser GetModuleUser()
    {
      try
      {
        UserInfo userInfo = Session.DefaultInstance.UserInfo;
        return new ModuleUser()
        {
          ID = userInfo.Userid,
          LastName = userInfo.LastName,
          FirstName = userInfo.FirstName,
          Email = userInfo.Email
        };
      }
      catch
      {
        return new ModuleUser()
        {
          ID = string.Empty,
          LastName = string.Empty,
          FirstName = string.Empty,
          Email = string.Empty
        };
      }
    }

    private void LoadModule()
    {
      this.webHost.LoadModule(this.webPageURL, new ModuleParameters()
      {
        User = this.GetModuleUser(),
        Parameters = this.webPageParams
      });
    }

    private void UnloadHandler() => this.Invoke((Delegate) (() => this.Close()));

    private void btnConsole_Click(object sender, EventArgs e)
    {
      if (this.btnConsole.Text == "Show Console")
      {
        this.webHost.ShowConsole();
        this.btnConsole.Text = "Hide Console";
      }
      else
      {
        this.webHost.HideConsole();
        this.btnConsole.Text = "Show Console";
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.components != null)
          this.components.Dispose();
        this.UnsubscribeWebhostEvents();
      }
      base.Dispose(disposing);
    }

    private void UnsubscribeWebhostEvents()
    {
      try
      {
        if (this.webHost == null)
          return;
        this.webHost.ExecuteComplete -= new WebHost.ExecuteCompleteEventHandler(this.webHost_executeComplete);
        this.webHost.BrowserTitleChanged -= new EventHandler<TitleChangeEventArgs>(this.WebHost_TitleChanged);
        this.webHost.GuestFrameComplete -= new WebHost.GuestFrameCompleteEventHandler(this.webHost_GuestPageLoadCompleted);
        this.webHost.HelpInvoked -= new WebHost.HelpInvokedEventHandler(this.WebHost_helpBrowserInvoked);
      }
      catch (Exception ex)
      {
      }
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoadWebPageForm));
      this.pnlBrowser = new Panel();
      this.btnConsole = new Button();
      this.pnlBrowser.SuspendLayout();
      this.SuspendLayout();
      this.pnlBrowser.BackColor = SystemColors.Control;
      this.pnlBrowser.Controls.Add((Control) this.btnConsole);
      this.pnlBrowser.Dock = DockStyle.Fill;
      this.pnlBrowser.Location = new Point(0, 0);
      this.pnlBrowser.Margin = new Padding(2);
      this.pnlBrowser.Name = "pnlBrowser";
      this.pnlBrowser.Size = new Size(784, 562);
      this.pnlBrowser.TabIndex = 3;
      this.btnConsole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnConsole.AutoSize = true;
      this.btnConsole.Location = new Point(697, 2);
      this.btnConsole.Margin = new Padding(2);
      this.btnConsole.Name = "btnConsole";
      this.btnConsole.Size = new Size(85, 23);
      this.btnConsole.TabIndex = 2;
      this.btnConsole.Text = "Show Console";
      this.btnConsole.UseVisualStyleBackColor = true;
      this.btnConsole.Click += new EventHandler(this.btnConsole_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(784, 562);
      this.Controls.Add((Control) this.pnlBrowser);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoadWebPageForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (LoadWebPageForm);
      this.pnlBrowser.ResumeLayout(false);
      this.pnlBrowser.PerformLayout();
      this.ResumeLayout(false);
    }

    public delegate void ExecuteComplete(object sender, ExecuteCompleteEventArgs e);

    public delegate void TitleChangedEventDelegate(object sender, TitleChangeEventArgs e);
  }
}
