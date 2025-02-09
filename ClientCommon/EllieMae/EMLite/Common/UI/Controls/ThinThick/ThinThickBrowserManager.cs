// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Controls.ThinThick.ThinThickBrowserManager
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.ThinThick;
using EllieMae.EMLite.RemotingServices;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Controls.ThinThick
{
  public class ThinThickBrowserManager : IDisposable
  {
    private const string className = "ThinThickBrowserManager";
    private const string rootRes = "EllieMae.EMLite.Common.UI.Controls.ThinThick";
    private CommandContext commandContext = new CommandContext();
    private ThinInThickCommand thinInThickCommand;

    public event ThinThickBrowserManager.WebBrowserTitleChangedHandler WebBrowserDocumentTitleChanged;

    public event ThinThickBrowserManager.WebBrowserBeforeNavigateHandler BeforeNavigate;

    public event ThinThickBrowserManager.WebBrowserNavigatedHandler Navigated;

    public ThinThickBrowserManager(
      Sessions.Session session,
      ThinThickBrowser browser,
      IWin32Window sourceWindow)
    {
      this.Session = session;
      this.Browser = browser;
      this.SourceWindow = sourceWindow;
      this.InitObjectForScripting();
      this.InitializeEvents();
    }

    private void InitializeEvents()
    {
      this.Browser.WebBrowserDocumentTitleChanged += new ThinThickBrowser.WebBrowserTitleChangedHandler(this.ThinThickBrowserManager_WebBrowserDocumentTitleChanged);
      this.Browser.BeforeNavigate += new ThinThickBrowser.WebBrowserBeforeNavigateHandler(this.Browser_BeforeNavigate);
      this.Browser.Navigated += new ThinThickBrowser.WebBrowserNavigatedHandler(this.Browser_Navigated);
    }

    private void Browser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
    {
      if (this.Navigated == null)
        return;
      this.Navigated(sender, e);
    }

    private void Browser_BeforeNavigate(object sender, WebBrowserNavigatingEventArgs e)
    {
      if (this.BeforeNavigate == null)
        return;
      this.BeforeNavigate(sender, e);
    }

    private void ThinThickBrowserManager_WebBrowserDocumentTitleChanged(object sender, EventArgs e)
    {
      if (this.WebBrowserDocumentTitleChanged == null)
        return;
      this.WebBrowserDocumentTitleChanged(sender, e);
    }

    public ThinThickBrowserManager(Sessions.Session session, ThinThickBrowser browser)
      : this(session, browser, (IWin32Window) null)
    {
    }

    public Sessions.Session Session { get; private set; }

    public ThinThickBrowser Browser { get; private set; }

    public IWin32Window SourceWindow { get; private set; }

    internal ThinThickBrowserManager Manager { get; private set; }

    private void InitObjectForScripting()
    {
      if (this.Browser == null)
        return;
      this.commandContext = new CommandContext()
      {
        Session = this.Session,
        SourceWindow = this.SourceWindow
      };
      this.thinInThickCommand = new ThinInThickCommand(this.commandContext);
      this.Browser.SetObjectForScripting((object) this.thinInThickCommand);
      this.Browser.SetManager(this);
    }

    internal void SupressScriptErrors() => this.Browser.SupressScriptError();

    public CommandContext CommandContext => this.commandContext;

    public void NavigatePage(string pageName)
    {
      string instanceName = this.Session.ServerIdentity == null ? string.Empty : this.Session.ServerIdentity.InstanceName;
      if (string.IsNullOrEmpty(instanceName))
      {
        Tracing.Log(true, "Warning", nameof (ThinThickBrowserManager), "Null or empty instance name");
        this.HandleUnexpectedConfiguration();
      }
      else
      {
        string empty = string.Empty;
        string drysdaleHostForInstance;
        try
        {
          drysdaleHostForInstance = this.Session.SessionObjects.ConfigurationManager.GetDrysdaleHostForInstance(instanceName);
        }
        catch (Exception ex)
        {
          Tracing.Log(true, "Error", nameof (ThinThickBrowserManager), "Error getting Drysdale host for instance " + instanceName + ": " + ex.Message);
          this.HandleUnexpectedConfiguration();
          return;
        }
        this.Browser.Navigate(string.Format("https://{0}/{1}", (object) drysdaleHostForInstance, (object) pageName));
      }
    }

    public void NavigateToThinClient(string pageName)
    {
      string str = this.Session.ServerIdentity == null ? string.Empty : this.Session.ServerIdentity.InstanceName;
      if (string.IsNullOrEmpty(str))
      {
        Tracing.Log(true, "Warning", nameof (ThinThickBrowserManager), "Null or empty instance name");
        this.HandleUnexpectedConfiguration();
      }
      else
      {
        string empty = string.Empty;
        string thinClientUrl;
        try
        {
          thinClientUrl = this.Session.SessionObjects.ConfigurationManager.GetThinClientURL();
        }
        catch (Exception ex)
        {
          Tracing.Log(true, "Error", nameof (ThinThickBrowserManager), "Error getting Encompass NG URL " + str + ": " + ex.Message);
          this.HandleUnexpectedConfiguration();
          return;
        }
        this.Browser.Navigate(string.Format("{0}/{1}", (object) thinClientUrl, (object) pageName));
      }
    }

    public void NavigatePageWithHeaders(string url, byte[] postData, string additionalHttpHeaders)
    {
      this.Browser.Navigate(url, postData, additionalHttpHeaders);
    }

    public void Navigate(string url) => this.Browser.Navigate(url);

    public void LoadHtml(string html) => this.Browser.SetDocumentText(html);

    private void HandleUnexpectedConfiguration() => this.NavigateErrorPage();

    public void NavigateSample()
    {
      string resourceName1 = "EllieMae.EMLite.Common.UI.Controls.ThinThick" + ".SamplePages." + (!(this.SourceWindow is IBrowser sourceWindow) ? "encompass.interaction.html" : sourceWindow.GetSamplePageName());
      string resourceName2 = "EllieMae.EMLite.Common.UI.Controls.ThinThick" + "." + "encompass.interaction.js";
      this.Browser.SetDocumentText(this.GetSampleResource(resourceName1).Replace("// $encompass.interaction.js$", this.GetSampleResource(resourceName2)));
    }

    public void NavigateErrorPage()
    {
      this.Browser.SetDocumentText(this.GetSampleResource("EllieMae.EMLite.Common.UI.Controls.ThinThick" + "." + "encompass.error.html"));
    }

    public bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (keyData != (Keys.S | Keys.Shift | Keys.Control | Keys.Alt))
        return false;
      this.NavigateSample();
      return true;
    }

    private string GetSampleResource(string resourceName)
    {
      string sampleResource = string.Empty;
      using (Stream manifestResourceStream = this.GetType().Assembly.GetManifestResourceStream(resourceName))
      {
        if (manifestResourceStream != null)
        {
          using (StreamReader streamReader = new StreamReader(manifestResourceStream, Encoding.ASCII))
            sampleResource = streamReader.ReadToEnd();
        }
      }
      return sampleResource;
    }

    public bool IsDirty() => this.thinInThickCommand.IsSomethingChanged();

    public void Reset() => this.thinInThickCommand.ResetTheChange();

    public virtual void Dispose() => this.Browser = (ThinThickBrowser) null;

    public delegate void WebBrowserTitleChangedHandler(object sender, EventArgs e);

    public delegate void WebBrowserBeforeNavigateHandler(
      object sender,
      WebBrowserNavigatingEventArgs e);

    public delegate void WebBrowserNavigatedHandler(object sender, WebBrowserNavigatedEventArgs e);
  }
}
