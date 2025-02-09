// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Browser.EncompassWebBrowser
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using Elli.Web.Host;
using Elli.Web.Host.EventObjects;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass.Browser
{
  public class EncompassWebBrowser : UserControl, IEncompassBrowser
  {
    private const string className = "EncompassWebBrowser";
    private static readonly string sw = Tracing.SwThinThick;
    internal WebHost hostBrowser;
    internal Func<string, string> encPostMessageHandler;
    private IContainer components;
    private Panel panel1;

    public static bool IsGlobalDebugEnabled => EnConfigurationSettings.GlobalSettings.Debug;

    private EncompassWebBrowser()
    {
      this.InitializeComponent();
      this.hostBrowser = new WebHost();
      this.hostBrowser.Dock = DockStyle.Fill;
      this.SubscribeEventHandler();
      this.panel1.Controls.Add((Control) this.hostBrowser);
      this.panel1.Dock = DockStyle.Fill;
    }

    private EncompassWebBrowser(BrowserParams parameter)
    {
      this.InitializeComponent();
      this.hostBrowser = new WebHost(this.BrowserParamMapper((IBrowserParams) parameter));
      this.encPostMessageHandler = parameter.EncPostMessageHandler;
      this.hostBrowser.Dock = DockStyle.Fill;
      this.SubscribeEventHandler();
      this.panel1.Controls.Add((Control) this.hostBrowser);
      this.panel1.Dock = DockStyle.Fill;
    }

    private string EncPostMesssageHandler(string payload) => this.encPostMessageHandler(payload);

    private void SubscribeEventHandler()
    {
      this.hostBrowser.FrameComplete += new WebHost.FrameCompleteEventHandler(this.HostBrowser_OnCompletingFrameEvent);
      this.hostBrowser.LoadingFrame += new WebHost.LoadingFrameEventHandler(this.HostBrowser_OnLoadingFrameEvent);
      this.hostBrowser.PrintComplete += new WebHost.PrintCompleteEventHandler(this.HostBrowser_onPrintJobEvent);
      this.hostBrowser.ConsoleMessage += new WebHost.ConsoleMessageEventHandler(this.HostBrowser_OnConsoleOutputEvent);
      this.hostBrowser.BeforeNavigation += new EventHandler<BeforeNavigationEventArgs>(this.HostBrowser_OnBeforeNavigationEvent);
    }

    private void UnsubscribeEventHandler()
    {
      this.hostBrowser.FrameComplete -= new WebHost.FrameCompleteEventHandler(this.HostBrowser_OnCompletingFrameEvent);
      this.hostBrowser.LoadingFrame -= new WebHost.LoadingFrameEventHandler(this.HostBrowser_OnLoadingFrameEvent);
      this.hostBrowser.PrintComplete -= new WebHost.PrintCompleteEventHandler(this.HostBrowser_onPrintJobEvent);
      this.hostBrowser.ConsoleMessage -= new WebHost.ConsoleMessageEventHandler(this.HostBrowser_OnConsoleOutputEvent);
      this.hostBrowser.BeforeNavigation -= new EventHandler<BeforeNavigationEventArgs>(this.HostBrowser_OnBeforeNavigationEvent);
    }

    private static string[] GetSwitchPreferences(List<string> browserPreferences)
    {
      if (browserPreferences == null)
        browserPreferences = new List<string>();
      browserPreferences.Add(SwitchPreferences.AggressiveCacheDiscard.ToDescriptionString());
      if (EncompassWebBrowser.IsGlobalDebugEnabled)
      {
        browserPreferences.Add(SwitchPreferences.RemoteDebuggingPort.ToDescriptionString());
        browserPreferences.Add(SwitchPreferences.ForceRendererAccessibility.ToDescriptionString());
      }
      else
        browserPreferences.Add(SwitchPreferences.CrashDumpDir.ToDescriptionString());
      return browserPreferences.ToArray();
    }

    private BrowserPreferences BrowserParamMapper(IBrowserParams browserParams)
    {
      BrowserPreferences browserPreferences = (BrowserPreferences) null;
      if (browserParams != null)
      {
        this.encPostMessageHandler = browserParams.EncPostMessageHandler;
        browserPreferences = new BrowserPreferences()
        {
          PostMessageHandler = new PostMessageHandler(this.EncPostMesssageHandler),
          Scope = browserParams.Scope,
          UnloadHandler = browserParams.UnloadHandler
        };
      }
      return browserPreferences;
    }

    public static EncompassWebBrowser Create(IBrowserParams parameters = null)
    {
      return parameters != null ? new EncompassWebBrowser(parameters as BrowserParams) : new EncompassWebBrowser();
    }

    public void Navigate(string url, string postData, Dictionary<string, string> headerCollection)
    {
      if (postData == null || headerCollection == null)
        return;
      this.hostBrowser?.Navigate(url, postData, headerCollection);
    }

    public void Navigate(string url)
    {
      if (string.IsNullOrWhiteSpace(url))
        return;
      this.hostBrowser?.Navigate(url);
    }

    public void Close() => this.hostBrowser?.Dispose();

    public bool ShowDebugPopUp()
    {
      WebHost hostBrowser = this.hostBrowser;
      return hostBrowser != null && hostBrowser.ShowDebugPopUp();
    }

    public void Reload(bool ignoreCache = false) => this.hostBrowser?.Reload(ignoreCache);

    public void ExecuteJavascript(string javascriptCode, [Optional] long? frameId)
    {
      this.hostBrowser?.ExecuteJavascript(javascriptCode, (object) frameId);
    }

    public void LoadModule(
      string hostUrl,
      string guestUrl,
      EncModuleParameters parameters,
      bool allowDragDrop = false)
    {
      this.hostBrowser?.LoadModule(hostUrl, guestUrl, parameters != null ? parameters.CustomModuleParametersMapper() : (ModuleParameters) null, allowDragDrop);
    }

    public void LoadModule(string guestUrl, EncModuleParameters parameters)
    {
      this.hostBrowser?.LoadModule(guestUrl, parameters != null ? parameters.CustomModuleParametersMapper() : (ModuleParameters) null);
    }

    public void LoadHtml(string html) => this.hostBrowser?.LoadHtml(html);

    public T ExecuteAndReturnValue<T>(string functionName, object[] eventParams = null)
    {
      if (this.hostBrowser == null)
        throw new NullReferenceException("Browser object is either dispose and not available");
      if (string.IsNullOrWhiteSpace(functionName))
        throw new ArgumentNullException("FunctionName is mandatory");
      return this.hostBrowser.ExecuteAndReturnValue<T>(functionName, eventParams);
    }

    public T RaiseEvent<T>(string eventType, object eventParams, int millisecondsTimeout)
    {
      return this.hostBrowser.RaiseEvent<T>(eventType, eventParams, millisecondsTimeout);
    }

    public bool SetJsObjectProperty(string sourceName, string propertyName, object propertyValue)
    {
      try
      {
        this.hostBrowser.SetBrowserProperty(sourceName, propertyName, propertyValue);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(EncompassWebBrowser.sw, TraceLevel.Error, nameof (EncompassWebBrowser), "Exception while setting value for the property - " + propertyName + ", sourceName - " + sourceName + ", exception-" + ex.Message);
        throw ex;
      }
    }

    public void BeginPrintWebPage(EncPrintSettings printSettings, [Optional] long? frameId, bool forcePrint = false)
    {
      try
      {
        this.hostBrowser?.BeginPrint(printSettings != null ? printSettings.CustomPrintSettingsMapper<PrintSettings>() : (PrintSettings) null, (object) frameId, forcePrint);
      }
      catch (Exception ex)
      {
        Tracing.Log(EncompassWebBrowser.sw, TraceLevel.Error, nameof (EncompassWebBrowser), string.Format("Error while printing. Handler not initialized for the frameId - {0}", (object) frameId));
        throw ex;
      }
    }

    public void InitCustomPopupHandler(Form parentWindowForm)
    {
      this.hostBrowser?.InitCustomPopUpHandler(parentWindowForm);
    }

    public string GetBrowserHtmlContent([Optional] long? frameId)
    {
      try
      {
        return this.hostBrowser?.GetBrowserHtml((object) frameId);
      }
      catch (Exception ex)
      {
        Tracing.Log(EncompassWebBrowser.sw, TraceLevel.Error, nameof (EncompassWebBrowser), string.Format("Error while reading the browser content for the frameId - {0}", (object) frameId));
        throw ex;
      }
    }

    public bool DOMElementExists([Optional] object frameName)
    {
      try
      {
        return this.hostBrowser?.DOMElementExists(frameName).Value;
      }
      catch (Exception ex)
      {
        Tracing.Log(EncompassWebBrowser.sw, TraceLevel.Error, nameof (EncompassWebBrowser), string.Format("Error while checking the browser DOMElement for the frameId - {0}", frameName));
        throw ex;
      }
    }

    public event EncFinishLoadingFrameEvent FinishLoadingFrameEvent;

    public event EncStartLoadingFrameEvent StartLoadingFrameEvent;

    public event EncPrintJobEvent PrintJobStatusChangeEvent;

    public event EncConsoleMessageEvent ConsoleMessageEvent;

    public event EncBeforeNavigationEvent BeforeNavigationEvent;

    private void HostBrowser_OnCompletingFrameEvent(object sender, FinishedLoadingEventArgs e)
    {
      if (this.FinishLoadingFrameEvent == null)
        return;
      this.BeginInvoke((Delegate) (() => this.FinishLoadingFrameEvent(sender, new EncFinishLoadingEventArgs(e.FrameName, e.IsMainFrame, e.ValidatedURL))));
    }

    private void HostBrowser_OnLoadingFrameEvent(object sender, StartLoadingEventArgs e)
    {
      if (this.StartLoadingFrameEvent == null)
        return;
      this.BeginInvoke((Delegate) (() => this.StartLoadingFrameEvent(sender, new EncStartLoadingEventArgs(e.IsMainFrame, e.ValidatedURL, e.ParentFrameId, e.IsErrorPage, e.IsMainFrame))));
    }

    private void HostBrowser_onPrintJobEvent(object sender, PrintJobEventArgs arg)
    {
      if (this.PrintJobStatusChangeEvent == null)
        return;
      this.BeginInvoke((Delegate) (() => this.PrintJobStatusChangeEvent(sender, new EncPrintJobEventArgs(arg.Success))));
    }

    private void HostBrowser_OnConsoleOutputEvent(object sender, ConsoleMessageEventArgs e)
    {
      if (this.ConsoleMessageEvent == null)
        return;
      this.BeginInvoke((Delegate) (() => this.ConsoleMessageEvent(sender, new EncConsoleMessageEventArgs((int) e.Level, e.LineNumber, e.Message, e.Source))));
    }

    private void HostBrowser_OnBeforeNavigationEvent(object sender, BeforeNavigationEventArgs e)
    {
      if (this.BeforeNavigationEvent == null)
        return;
      EncBeforeNavigationEventArgs args = new EncBeforeNavigationEventArgs(e.HasUserGesture, e.IsExternalProtocol, e.IsMainFrame, e.IsPost, e.IsRedirect, e.Url, e.Cancel);
      this.BeginInvoke((Delegate) (() => this.BeforeNavigationEvent(sender, args)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.components?.Dispose();
      this.UnsubscribeEventHandler();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.SuspendLayout();
      this.panel1.AutoSize = true;
      this.panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Location = new Point(21, 23);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(2, 2);
      this.panel1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (EncompassWebBrowser);
      this.Size = new Size(1000, 1000);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
