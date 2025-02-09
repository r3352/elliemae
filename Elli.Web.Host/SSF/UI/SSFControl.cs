// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.UI.SSFControl
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Events;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Handlers;
using DotNetBrowser.Input;
using DotNetBrowser.Input.Keyboard.Events;
using DotNetBrowser.Js;
using DotNetBrowser.Navigation.Events;
using DotNetBrowser.Navigation.Handlers;
using DotNetBrowser.WinForms;
using Elli.Web.Host.SSF.Context;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host.SSF.UI
{
  public class SSFControl : UserControl, IRaiseEventHandler
  {
    private const string className = "SSFControl";
    private static readonly string sw = Tracing.SwThinThick;
    private BrowserView browserView;
    private IBrowser browser;
    private SSFContext context;
    private string url;
    private bool finishedLoading;
    private List<ConsoleMessageEventArgs> consoleMessages;
    private IContainer components;

    public SSFControl()
    {
      this.InitializeComponent();
      if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
        return;
      this.createBrowser();
    }

    private void createBrowser()
    {
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Initializing Console Messages");
      this.consoleMessages = new List<ConsoleMessageEventArgs>();
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Creating Browser");
      this.browser = BrowserEngine.CreateBrowser();
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Creating BrowserView");
      this.browserView = new BrowserView();
      this.browserView.InitializeFrom(this.browser);
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Adding BrowserView to Controls");
      this.Controls.Add((Control) this.browserView);
      this.browserView.BackColor = Color.White;
      this.browserView.Dock = DockStyle.Fill;
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Subscribing to Browser Events");
      this.browser.ConsoleMessageReceived += new EventHandler<ConsoleMessageReceivedEventArgs>(this.browser_ConsoleMessage);
      this.browser.Disposed += new EventHandler(this.browser_Dispose);
      this.browser.Navigation.FrameLoadFinished += new EventHandler<FrameLoadFinishedEventArgs>(this.browser_FinishLoadingFrame);
      this.browser.Navigation.LoadFinished += new EventHandler<LoadFinishedEventArgs>(this.browser_LoadFinished);
      this.browser.Navigation.NavigationStarted += new EventHandler<NavigationStartedEventArgs>(this.browser_NavigationStarted);
      this.browser.InjectJsHandler = (IHandler<InjectJsParameters>) new Handler<InjectJsParameters>((Action<InjectJsParameters>) (e => this.browser_ScriptContextCreated(e)));
      this.browser.Keyboard.KeyReleased.Handler = (IHandler<IKeyReleasedEventArgs, InputEventResponse>) new Handler<IKeyReleasedEventArgs, InputEventResponse>((Func<IKeyReleasedEventArgs, InputEventResponse>) (e => this.browser_KeyReleased(e)));
      this.browser.Navigation.StartNavigationHandler = (IHandler<StartNavigationParameters, StartNavigationResponse>) new Handler<StartNavigationParameters, StartNavigationResponse>((Func<StartNavigationParameters, StartNavigationResponse>) (e => this.browser_StartNavigation(e)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.components != null)
          this.components.Dispose();
        this.disposeBrowser();
      }
      base.Dispose(disposing);
    }

    private void disposeBrowser()
    {
      if (this.IsBrowserDisposed)
        return;
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Unsubscribing from Browser Events");
      this.browser.ConsoleMessageReceived -= new EventHandler<ConsoleMessageReceivedEventArgs>(this.browser_ConsoleMessage);
      this.browser.Disposed -= new EventHandler(this.browser_Dispose);
      this.browser.Navigation.FrameLoadFinished -= new EventHandler<FrameLoadFinishedEventArgs>(this.browser_FinishLoadingFrame);
      this.browser.Navigation.NavigationStarted -= new EventHandler<NavigationStartedEventArgs>(this.browser_NavigationStarted);
      this.browser.Navigation.LoadFinished -= new EventHandler<LoadFinishedEventArgs>(this.browser_LoadFinished);
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Unsubscribing from Browser Handlers");
      this.browser.InjectJsHandler = (IHandler<InjectJsParameters>) null;
      this.browser.Keyboard.KeyReleased.Handler = (IHandler<IKeyReleasedEventArgs, InputEventResponse>) null;
      this.browser.Navigation.StartNavigationHandler = (IHandler<StartNavigationParameters, StartNavigationResponse>) null;
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Disposing BrowserView");
      this.browserView.Dispose();
      this.browserView = (BrowserView) null;
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Disposing Browser");
      this.browser.Dispose();
      this.browser = (IBrowser) null;
    }

    public bool IsBrowserDisposed
    {
      get
      {
        IBrowser browser = this.browser;
        return browser == null || browser.IsDisposed;
      }
    }

    public ConsoleMessageEventArgs[] ConsoleMessages
    {
      get
      {
        return this.consoleMessages != null ? this.consoleMessages.ToArray() : (ConsoleMessageEventArgs[]) null;
      }
    }

    private void browser_ConsoleMessage(object sender, ConsoleMessageReceivedEventArgs e)
    {
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Browser: ConsoleMessage: " + e.Message);
      ConsoleMessageEventArgs message = new ConsoleMessageEventArgs((int) e.Level, e.LineNumber, e.Message, e.Source);
      lock (this.consoleMessages)
        this.consoleMessages.Add(message);
      if (!this.IsHandleCreated)
        return;
      this.BeginInvoke((Delegate) (() => this.OnConsoleMessage(message)));
    }

    private void browser_Dispose(object sender, EventArgs e)
    {
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Browser: Dispose");
    }

    private void browser_FinishLoadingFrame(object sender, FrameLoadFinishedEventArgs e)
    {
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Browser: FinishLoadingFrame: " + e.ValidatedUrl + " [" + e.Frame.IsMain.ToString() + "]");
      if (!this.IsHandleCreated)
        return;
      FinishedLoadingEventArgs args = new FinishedLoadingEventArgs((object) e.Frame.Name, e.Frame?.IsMain.Value, e.ValidatedUrl);
      this.BeginInvoke((Delegate) (() => this.OnFrameComplete(args)));
    }

    private InputEventResponse browser_KeyReleased(IKeyReleasedEventArgs e)
    {
      if (e.VirtualKey == KeyCode.F12)
      {
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Showing Chromium Debugger");
        this.BeginInvoke((Delegate) (() => this.ShowDebugger()));
      }
      else if (e.VirtualKey == KeyCode.F11)
      {
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Showing Chromium Console");
        this.BeginInvoke((Delegate) (() => this.ShowConsole()));
      }
      return InputEventResponse.Proceed;
    }

    private void browser_LoadFinished(object sender, LoadFinishedEventArgs e)
    {
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Browser: LoadFinished: " + e.Browser.Url);
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Locking 'browserView'");
      lock (this.browserView)
      {
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Checking 'url': " + this.url);
        if (string.Equals(this.url, e.Browser.Url, StringComparison.OrdinalIgnoreCase))
        {
          Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Setting 'finishedLoading' = true");
          this.finishedLoading = true;
        }
      }
      if (!this.IsHandleCreated)
        return;
      FinishedLoadingEventArgs args = new FinishedLoadingEventArgs((object) e.Browser.MainFrame.Name, e.Browser.Url);
      this.BeginInvoke((Delegate) (() => this.OnNavigationComplete(args)));
    }

    private void browser_ScriptContextCreated(InjectJsParameters e)
    {
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Browser: ScriptContextCreated: IsMain=" + e.Frame.IsMain.ToString());
      if (e.Frame.IsMain)
      {
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling SetBrowserProperty: window, context");
        bool flag = this.SetBrowserProperty("window", "context", (object) this.context);
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Checking SetBrowserProperty Result: " + flag.ToString());
        if (!flag)
          return;
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling SetBrowserProperty: windows, context");
        this.context.RaiseEventHandler = (IRaiseEventHandler) this;
      }
      else
      {
        if (e.Frame.Parent == null || !e.Frame.Parent.IsMain)
          return;
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Initializing SSFGuestFrameHandler");
        SSFGuestFrameHandler.Initialize(e.Frame);
      }
    }

    private void browser_NavigationStarted(object sender, NavigationStartedEventArgs e)
    {
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Browser: NavigationStarted: " + e.Url + " [" + e.IsMainFrame.ToString() + "]");
      if (!this.IsHandleCreated)
        return;
      StartLoadingEventArgs args = new StartLoadingEventArgs(e.IsMainFrame, e.Url, e.IsSameDocument);
      this.BeginInvoke((Delegate) (() => this.OnNavigationStarted(args)));
    }

    private StartNavigationResponse browser_StartNavigation(StartNavigationParameters e)
    {
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Browser: StartNavigation: " + e.Url + " [" + e.IsMainFrame.ToString() + "]");
      BeforeNavigationEventArgs args = BeforeNavigationEventArgs.GetArgs(e);
      this.OnBeforeNavigation(args);
      return args.Cancel ? StartNavigationResponse.Ignore() : StartNavigationResponse.Start();
    }

    public event SSFControl.BeforeNavigationEventHandler BeforeNavigation;

    protected virtual void OnBeforeNavigation(BeforeNavigationEventArgs e)
    {
      if (this.BeforeNavigation == null)
        return;
      this.BeforeNavigation((object) this, e);
    }

    public event SSFControl.ConsoleMessageEventHandler ConsoleMessage;

    protected virtual void OnConsoleMessage(ConsoleMessageEventArgs e)
    {
      if (this.ConsoleMessage == null)
        return;
      this.ConsoleMessage((object) this, e);
    }

    public event SSFControl.FrameCompleteEventHandler FrameComplete;

    protected virtual void OnFrameComplete(FinishedLoadingEventArgs e)
    {
      if (this.FrameComplete == null)
        return;
      this.FrameComplete((object) this, e);
    }

    public event SSFControl.NavigationCompleteEventHandler NavigationComplete;

    protected virtual void OnNavigationComplete(FinishedLoadingEventArgs e)
    {
      if (this.NavigationComplete == null)
        return;
      this.NavigationComplete((object) this, e);
    }

    public event SSFControl.NavigationStartedEventHandler NavigationStarted;

    protected virtual void OnNavigationStarted(StartLoadingEventArgs e)
    {
      if (this.NavigationStarted == null)
        return;
      this.NavigationStarted((object) this, e);
    }

    public bool LoadApp(SSFContext context)
    {
      if (this.IsBrowserDisposed)
        return false;
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Storing Context");
      this.context = context;
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling Browser.LoadUrl");
      this.browser.Navigation.LoadUrl(context.hostUrl);
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Locking 'BrowserView'");
      lock (this.browserView)
      {
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Setting 'url' = " + context.hostUrl);
        this.url = context.hostUrl;
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Setting 'finishedLoading' = false");
        this.finishedLoading = false;
      }
      return true;
    }

    public bool RaiseEvent(SSFEventArgs eventArgs)
    {
      bool flag = false;
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Checking Browser.IsDisposed");
      if (!this.IsBrowserDisposed)
      {
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Locking 'BrowserView'");
        lock (this.browserView)
        {
          Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Getting 'finishedLoading': " + this.finishedLoading.ToString());
          if (!this.finishedLoading)
            return flag;
        }
        string javaScript = "raiseEvent";
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling Browser.ExecuteJavaScriptAndReturnValue: " + javaScript);
        IJsFunction result = this.browser.MainFrame?.ExecuteJavaScript<IJsFunction>(javaScript)?.Result;
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Checking Browser.ExecuteJavaScriptAndReturnValue Result");
        if (result != null)
        {
          Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling Function.Invoke: " + eventArgs.ToString());
          result.Invoke((IJsObject) null, null, (object) eventArgs.ObjectName, (object) eventArgs.EventName, (object) eventArgs.EventPayload, null);
          Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Completed Function.Invoke: " + eventArgs.ToString());
          flag = true;
        }
        else
          Tracing.Log(SSFControl.sw, TraceLevel.Warning, nameof (SSFControl), "Unable to find " + javaScript);
      }
      return flag;
    }

    public bool RaiseEvent<T>(SSFEventArgs<T> eventArgs, int millisecondsTimeout = 2000)
    {
      eventArgs.EventFeedback = default (T);
      bool flag = false;
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Checking Browser.IsDisposed");
      if (!this.IsBrowserDisposed)
      {
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Locking 'BrowserView'");
        lock (this.browserView)
        {
          Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Getting 'finishedLoading': " + this.finishedLoading.ToString());
          if (!this.finishedLoading)
            return flag;
        }
        string javaScript = "raiseEvent";
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling Browser.ExecuteJavaScriptAndReturnValue: " + javaScript);
        IJsFunction result = this.browser.MainFrame?.ExecuteJavaScript<IJsFunction>(javaScript)?.Result;
        Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Checking Browser.ExecuteJavaScriptAndReturnValue Result");
        if (result != null)
        {
          JavascriptFunctionCallback<T> callback = new JavascriptFunctionCallback<T>();
          JsFunctionCallback functionCallback = (JsFunctionCallback) (jsObject =>
          {
            Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling JavascriptFunctionCallback.Callback");
            return callback.Callback(jsObject);
          });
          Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling Function.Invoke: " + eventArgs.ToString());
          result.Invoke((IJsObject) null, (object) functionCallback, (object) eventArgs.ObjectName, (object) eventArgs.EventName, (object) eventArgs.EventPayload, (object) millisecondsTimeout);
          Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling JavascriptFunctionCallback.WaitForCallback: " + (object) millisecondsTimeout);
          eventArgs.EventFeedback = callback.WaitForCallback(millisecondsTimeout);
          Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Completed Function.Invoke: " + eventArgs.ToString());
          flag = true;
        }
        else
          Tracing.Log(SSFControl.sw, TraceLevel.Warning, nameof (SSFControl), "Unable to find " + javaScript);
      }
      return flag;
    }

    public bool SetBrowserProperty(string sourceName, string propertyName, object propertyValue)
    {
      if (string.IsNullOrEmpty(sourceName))
        throw new ArgumentException("SourceName - " + sourceName + " cannot be empty.");
      if (string.IsNullOrEmpty(propertyName))
        throw new ArgumentException("PropertyName - " + propertyName + " cannot be empty.");
      if (this.IsBrowserDisposed)
        return false;
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling ExecuteJavaScript: " + sourceName);
      IJsObject result = this.browser.MainFrame?.ExecuteJavaScript<IJsObject>(sourceName)?.Result;
      if (result == null)
      {
        Tracing.Log(SSFControl.sw, TraceLevel.Error, nameof (SSFControl), "ExecuteJavaScript Failed: " + sourceName);
        return false;
      }
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Setting " + propertyName + " Property: " + propertyValue);
      result.Properties[propertyName] = propertyValue;
      return true;
    }

    public bool ShowConsole()
    {
      if (this.IsBrowserDisposed)
        return false;
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Showing Console Window");
      SSFConsole.ShowConsole(this);
      return true;
    }

    public bool ShowDebugger()
    {
      if (this.IsBrowserDisposed)
        return false;
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling Browser.GetRemoteDebuggingURL");
      string remoteDebuggingUrl = this.browser.DevTools.RemoteDebuggingUrl;
      if (string.IsNullOrEmpty(remoteDebuggingUrl))
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Chromium Debugging is not enabled currently. To enable it, you will need to enable Diagnostic Mode from the Encompass 'Help' menu and then restart Encompass.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
      int int32_1 = Convert.ToInt32((double) workingArea.Width * 0.9);
      int int32_2 = Convert.ToInt32((double) workingArea.Height * 0.9);
      int int32_3 = Convert.ToInt32(workingArea.Left + (workingArea.Width - int32_1) / 2);
      int int32_4 = Convert.ToInt32(workingArea.Top + (workingArea.Height - int32_2) / 2);
      string javaScript = string.Format("window.open(\"{0}\", \"{1}\", \"left={2},top={3},width={4},height={5}\");", (object) remoteDebuggingUrl, (object) "_blank", (object) int32_3, (object) int32_4, (object) int32_1, (object) int32_2);
      Tracing.Log(SSFControl.sw, TraceLevel.Verbose, nameof (SSFControl), "Calling Browser.ExecuteJavaScript: " + javaScript);
      this.browser.MainFrame.ExecuteJavaScript(javaScript);
      return true;
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.DoubleBuffered = true;
      this.Name = nameof (SSFControl);
      this.ResumeLayout(false);
    }

    public delegate void BeforeNavigationEventHandler(object sender, BeforeNavigationEventArgs e);

    public delegate void ConsoleMessageEventHandler(object sender, ConsoleMessageEventArgs e);

    public delegate void FrameCompleteEventHandler(object sender, FinishedLoadingEventArgs e);

    public delegate void NavigationCompleteEventHandler(object sender, FinishedLoadingEventArgs e);

    public delegate void NavigationStartedEventHandler(object sender, StartLoadingEventArgs e);
  }
}
