// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.Login.LoginThinThickForm
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Elli.Web.Host.BrowserControls;
using Elli.Web.Host.EventObjects;
using EllieMae.EMLite.Common.ThinThick;
using EllieMae.EMLite.Common.UI.Controls.ThinThick;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host.Login
{
  public class LoginThinThickForm : Form
  {
    private UserControl _webBrowser;
    private CommandContext commandContext;
    private ThinInThickCommand thinInThickCommand;
    private IContainer components;

    public event LoginThinThickForm.TitleChangedEventDelegate OnBrowserTitleChange;

    public event LoginThinThickForm.BrowserLoadComplete OnBrowserLoadComplete;

    public event LoginThinThickForm.BeforeNavigation OnStartNavigation;

    public Sessions.Session _session { get; private set; }

    public IWin32Window SourceWindow { get; private set; }

    public CommandContext CommandContext => this.commandContext;

    public LoginThinThickForm(string title = "")
    {
      this.InitializeComponent();
      if (!string.IsNullOrEmpty(title))
        this.Text = title;
      this.AddBrowserControl();
    }

    public void HideForm()
    {
      if (this.InvokeRequired)
        this.BeginInvoke((Delegate) (() => this.Hide()));
      else
        this.Hide();
    }

    private void AddBrowserControl()
    {
      for (int index = 0; index < this.Controls.Count; ++index)
        this.Controls.RemoveAt(index);
      WebHost webHost = new WebHost();
      this.Controls.Add((Control) webHost);
      webHost.Dock = DockStyle.Fill;
      this._webBrowser = (UserControl) webHost;
      this._session = Session.DefaultInstance;
      this.InitObjectForScripting();
    }

    private void InitObjectForScripting()
    {
      if (this._webBrowser == null)
        return;
      this.commandContext = new CommandContext()
      {
        Session = this._session,
        SourceWindow = (IWin32Window) this._webBrowser
      };
      this.thinInThickCommand = new ThinInThickCommand(this.commandContext);
      ((WebHost) this._webBrowser)?.SetBrowserProperty("window", "external", (object) this.thinInThickCommand);
    }

    private void LoginThinThickForm_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void LoginThinThickForm_Load(object sender, EventArgs e)
    {
      Control control = this.Controls[0];
      if (!(control is WebHost))
        return;
      if (string.IsNullOrWhiteSpace(this.Text) || string.Compare("Login Form", this.Text, true) == 0)
        this.Text = "Encompass Log In";
      ((EncWebFormBrowserControl) control).BrowserTitleChanged += new EventHandler<TitleChangeEventArgs>(this.LoginThinThickForm_BrowserTitleChanged);
      ((WebHost) control).BrowserNavigationStarted += new EventHandler<StartLoadingEventArgs>(this.OnBeforeNavigation);
      ((WebHost) control).BrowserNavigationCompleted += new EventHandler<NavigationCompletedEventArgs>(this.OnBrowserNavigateCompleted);
      this.MaximizeBox = false;
    }

    private void ChromeThinThickForm_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    public void InitObjectForScripting(Sessions.Session session, IWin32Window sourceWindow)
    {
    }

    private void OnBrowserNavigateCompleted(object sender, NavigationCompletedEventArgs e)
    {
      if (this.OnBrowserLoadComplete == null)
        return;
      this.OnBrowserLoadComplete(sender, e);
    }

    private void OnBeforeNavigation(object sender, StartLoadingEventArgs e)
    {
      if (this.OnStartNavigation == null)
        return;
      this.OnStartNavigation(sender, e);
    }

    private void LoginThinThickForm_BrowserTitleChanged(object sender, TitleChangeEventArgs e)
    {
      if (this.OnBrowserTitleChange == null)
        return;
      this.OnBrowserTitleChange(sender, e);
    }

    public void Navigate(string url)
    {
      if (!(this._webBrowser is WebHost))
        return;
      ((EncWebFormBrowserControl) this._webBrowser).Navigate(url);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoginThinThickForm));
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1042, 732);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MinimizeBox = false;
      this.Name = nameof (LoginThinThickForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Login Form";
      this.FormClosing += new FormClosingEventHandler(this.LoginThinThickForm_FormClosing);
      this.Load += new EventHandler(this.LoginThinThickForm_Load);
      this.ResumeLayout(false);
    }

    public delegate void TitleChangedEventDelegate(object sender, TitleChangeEventArgs e);

    public delegate void BrowserLoadComplete(object sender, NavigationCompletedEventArgs e);

    public delegate void BeforeNavigation(object sender, StartLoadingEventArgs e);
  }
}
