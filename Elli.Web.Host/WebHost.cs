// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.WebHost
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Events;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Dom;
using DotNetBrowser.Dom.Events;
using DotNetBrowser.Frames;
using DotNetBrowser.Handlers;
using DotNetBrowser.Input;
using DotNetBrowser.Input.Keyboard.Events;
using DotNetBrowser.Input.Mouse.Events;
using DotNetBrowser.Js;
using DotNetBrowser.Navigation;
using DotNetBrowser.Navigation.Events;
using DotNetBrowser.Navigation.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Passwords.Handlers;
using DotNetBrowser.Print.Handlers;
using DotNetBrowser.WinForms;
using Elli.Web.Host.Adapter;
using Elli.Web.Host.BrowserControls;
using Elli.Web.Host.EventObjects;
using Elli.Web.Host.Login;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host
{
  public class WebHost : EncWebFormBrowserControl
  {
    private const string className = "WebHost";
    private static readonly string sw = Tracing.SwThinThick;
    private const string mandatoryHeaderKey = "content-type";
    private BrowserView wbBrowserView;
    private IBrowser browser;
    private string scope;
    private Elli.Web.Host.PostMessageHandler postMessageHandler;
    private Elli.Web.Host.AuditWindowHandler auditWindowHandler;
    private Elli.Web.Host.UpdateTemplateWindowHandler updateTemplateWindowHandler;
    private System.Action unloadHandler;
    private EncCustomPrintHandler customPrintHandler;
    private HostContext hostContext;
    private string url;
    private bool finishedLoading;
    private IContainer components;
    private RichTextBox rtbConsole;
    private CollapsibleSplitter csBrowser;
    private BorderPanel pnlBrowser;
    private BorderPanel pnlConsole;

    public bool IsBrowserDisposed
    {
      get
      {
        IBrowser browser = this.browser;
        return browser == null || browser.IsDisposed;
      }
    }

    public WebHost()
    {
      this.InitializeComponent();
      this.createBrowser();
      this.HideConsole();
    }

    public WebHost(
      string scope,
      Elli.Web.Host.PostMessageHandler postMessageHandler = null,
      System.Action unloadHandler = null,
      Elli.Web.Host.AuditWindowHandler auditWindowHandler = null,
      Elli.Web.Host.UpdateTemplateWindowHandler updateTemplateWindowHandler = null)
      : this()
    {
      this.scope = scope;
      this.postMessageHandler = postMessageHandler;
      this.unloadHandler = unloadHandler;
      this.auditWindowHandler = auditWindowHandler;
      this.updateTemplateWindowHandler = updateTemplateWindowHandler;
    }

    public WebHost(BrowserPreferences preferenceParameter)
      : this()
    {
      if (preferenceParameter == null)
        return;
      this.scope = preferenceParameter.Scope;
      if (preferenceParameter.PostMessageHandler != null)
        this.postMessageHandler = preferenceParameter.PostMessageHandler;
      if (preferenceParameter.UnloadHandler == null)
        return;
      this.unloadHandler = preferenceParameter.UnloadHandler;
    }

    private void createBrowser()
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Creating Browser");
      this.browser = BrowserEngine.CreateBrowser();
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Creating WinFormsBrowserView");
      this.wbBrowserView = new BrowserView();
      this.wbBrowserView.InitializeFrom(this.browser);
      this.wbBrowserView.Focus();
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Adding WinFormsBrowserView to Controls");
      this.pnlBrowser.Controls.Add((Control) this.wbBrowserView);
      this.wbBrowserView.BackColor = Color.White;
      this.wbBrowserView.Bounds = this.pnlBrowser.DisplayRectangle;
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Subscribing to Browser Events");
      this.browser.ConsoleMessageReceived += new EventHandler<ConsoleMessageReceivedEventArgs>(this.browser_ConsoleMessage);
      this.browser.Disposed += new EventHandler(this.browser_Dispose);
      this.browser.Navigation.FrameLoadFinished += new EventHandler<FrameLoadFinishedEventArgs>(this.browser_FinishLoadingFrame);
      this.browser.Navigation.NavigationStarted += new EventHandler<NavigationStartedEventArgs>(this.browser_StartLoadingFrame);
      this.browser.Navigation.NavigationFinished += new EventHandler<NavigationFinishedEventArgs>(this.browser_NavigationFinished);
      this.browser.Navigation.LoadFinished += new EventHandler<LoadFinishedEventArgs>(this.browser_LoadFinished);
      this.browser.TitleChanged += new EventHandler<TitleChangedEventArgs>(this.browser_TitleChanged);
      this.browser.Passwords.SavePasswordHandler = (IHandler<SavePasswordParameters, SavePasswordResponse>) new Handler<SavePasswordParameters, SavePasswordResponse>((Func<SavePasswordParameters, SavePasswordResponse>) (p => SavePasswordResponse.Ignore));
      this.browser.InjectJsHandler = (IHandler<InjectJsParameters>) new Handler<InjectJsParameters>((Action<InjectJsParameters>) (args => this.browser_ScriptContextCreated(args)));
      this.browser.Navigation.StartNavigationHandler = (IHandler<StartNavigationParameters, StartNavigationResponse>) new Handler<StartNavigationParameters, StartNavigationResponse>(new Func<StartNavigationParameters, StartNavigationResponse>(this.OnBeforeNavigation));
      if (WebLoginUtil.IsEnableChromeDebugMode)
        this.browser.Keyboard.KeyReleased.Handler = (IHandler<IKeyReleasedEventArgs, InputEventResponse>) new Handler<IKeyReleasedEventArgs, InputEventResponse>((Func<IKeyReleasedEventArgs, InputEventResponse>) (e =>
        {
          if (e.VirtualKey == KeyCode.F12)
          {
            Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Showing Chromium Debugger");
            this.BeginInvoke((Delegate) (() => this.ShowDebugPopUp()));
          }
          return InputEventResponse.Proceed;
        }));
      this.browser.Mouse.Released.Handler = (IHandler<IMouseReleasedEventArgs, InputEventResponse>) new Handler<IMouseReleasedEventArgs, InputEventResponse>(new Func<IMouseReleasedEventArgs, InputEventResponse>(this.OnMouseReleasedHandler));
    }

    public void ExecuteJavascript(string scriptCode, [Optional] object frameName)
    {
      if (this.IsBrowserDisposed)
        return;
      IFrame frame = frameName == Missing.Value || frameName == null ? this.browser.MainFrame : this.browser.AllFrames.FirstOrDefault<IFrame>((Func<IFrame, bool>) (item => item.Name.Equals((string) frameName, StringComparison.InvariantCultureIgnoreCase)));
      if (frame == null)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "ExecuteJavaScript: Unable to locate frame");
      }
      else
      {
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Frame.ExecuteJavaScript: " + scriptCode);
        frame.ExecuteJavaScript(scriptCode);
      }
    }

    public T ExecuteJavascript<T>(string scriptCode, [Optional] object frameName)
    {
      if (this.IsBrowserDisposed)
        return default (T);
      IFrame frame = frameName == Missing.Value || frameName == null ? this.browser.MainFrame : this.browser.AllFrames.FirstOrDefault<IFrame>((Func<IFrame, bool>) (item => item.Name.Equals((string) frameName, StringComparison.InvariantCultureIgnoreCase)));
      if (frame == null)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "ExecuteJavaScript<T>: Unable to locate frame");
        return default (T);
      }
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Frame.ExecuteJavaScript: " + scriptCode);
      return frame.ExecuteJavaScript<T>(scriptCode).Result;
    }

    public void HideConsole()
    {
      this.csBrowser.IsCollapsed = true;
      this.csBrowser.Visible = false;
    }

    public override void LoadHtml(string html)
    {
      if (this.IsBrowserDisposed)
        return;
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.LoadHTML");
      this.browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(Encoding.UTF8.GetBytes(html)));
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Locking 'wbBrowser'");
      lock (this.wbBrowserView)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'url' = null");
        this.url = "about:blank";
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'finishedLoading' = false");
        this.finishedLoading = false;
      }
    }

    public override void LoadHtml(string html, bool documentReadonly)
    {
      if (this.IsBrowserDisposed)
        return;
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.LoadHTML");
      NavigationResult result = this.browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(Encoding.UTF8.GetBytes(html))).Result;
      if (result == null || (result != null ? (result.LoadResult != 0 ? 1 : 0) : 1) != 0)
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "LoadHtml: Failed to load the url. Load status-" + result.ToString());
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Locking 'wbBrowser'");
      lock (this.wbBrowserView)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'url' = null");
        this.url = "about:blank";
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'finishedLoading' = false");
        this.finishedLoading = false;
      }
      string propertyValue = documentReadonly ? "Off" : "On";
      this.ExecuteJavascript<string>("document.designMode = '" + propertyValue + "';", System.Type.Missing);
      try
      {
        this.SetBrowserProperty("document", "designMode", (object) propertyValue);
      }
      catch (Exception ex)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "LoadHtml: Error while setting design mode. Exception- " + ex.Message + Environment.NewLine + " Stack trace-" + ex.StackTrace);
      }
    }

    public void LoadModule(string guestUrl, ModuleParameters parameters)
    {
      if (this.IsBrowserDisposed)
        return;
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Creating HostContext");
      this.hostContext = new HostContext(guestUrl, parameters, new Elli.Web.Host.PostMessageHandler(this.PostMessageHandler), new System.Action(this.UnloadHandler), this.scope, new Elli.Web.Host.AuditWindowHandler(this.AuditWindowHandler));
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'executeComplete' event hook");
      this.hostContext.executeComplete += new HostContext.ExecuteComplete(this.hostContext_ExecuteComplete);
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'GuestPageLoadCompleted' event hook");
      this.hostContext.GuestPageLoadCompleted += new HostContext.PageLoadComplete(this.hostContext_GuestPageLoadCompleted);
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'helpBrowserInvoked' event hook");
      this.hostContext.helpBrowserInvoked += new HostContext.InvokeHelpBrowser(this.HostContext_helpBrowserInvoked);
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.LoadHTML");
      this.browser.Navigation.LoadUrl(Session.StartupInfo?.ServiceUrls?.SSFHostUrl + "HostAdapter.html");
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Locking 'wbBrowser'");
      lock (this.wbBrowserView)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'url' = null");
        this.url = "about:blank";
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'finishedLoading' = false");
        this.finishedLoading = false;
      }
    }

    public override bool ExecuteCommand(string command, object value = null)
    {
      bool flag = false;
      if (this.IsBrowserDisposed)
        return false;
      if (this.browser.MainFrame == null)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "ExecuteCommand: Unable to locate frame");
        return false;
      }
      switch (command.ToLower())
      {
        case "bold":
          flag = this.browser.MainFrame.Execute(EditorCommand.ToggleBold());
          break;
        case "copy":
          flag = this.browser.MainFrame.Execute(EditorCommand.Copy());
          break;
        case "createlink":
          if (value != null)
          {
            flag = this.browser.MainFrame.Execute(EditorCommand.InsertHtml("<a href='" + value.ToString() + "'>" + this.browser.MainFrame.SelectedText + "</a>"));
            break;
          }
          break;
        case "cut":
          flag = this.browser.MainFrame.Execute(EditorCommand.Cut());
          break;
        case "delete":
          flag = this.browser.MainFrame.Execute(EditorCommand.Delete());
          break;
        case "fontname":
          flag = this.browser.MainFrame.Execute(EditorCommand.FontName(value.ToString()));
          break;
        case "fontsize":
          flag = this.browser.MainFrame.Execute(EditorCommand.FontSize(Convert.ToInt32(value)));
          break;
        case "forecolor":
          flag = this.browser.MainFrame.Execute(EditorCommand.ForeColor(value.ToString()));
          break;
        case "inserthtml":
          flag = this.browser.MainFrame.Execute(EditorCommand.InsertHtml(value.ToString()));
          break;
        case "insertimage":
          if (value != null)
          {
            flag = this.browser.MainFrame.Execute(EditorCommand.InsertHtml("<div style='display:inline-block;resize:both;overflow:hidden;line-height:0;'><img src='" + value.ToString() + "' style='width:100%;height:100%;'></div > "));
            break;
          }
          break;
        case "italic":
          flag = this.browser.MainFrame.Execute(EditorCommand.ToggleItalic());
          break;
        case "paste":
          flag = this.browser.MainFrame.Execute(EditorCommand.Paste());
          break;
        case "redo":
          flag = this.browser.MainFrame.Execute(EditorCommand.Redo());
          break;
        case "underline":
          flag = this.browser.MainFrame.Execute(EditorCommand.ToggleUnderline());
          break;
        case "undo":
          flag = this.browser.MainFrame.Execute(EditorCommand.Undo());
          break;
      }
      return flag;
    }

    public override bool IsQueryCommandEnabled(string command)
    {
      bool flag = true;
      if (this.IsBrowserDisposed)
        return false;
      if (this.browser.MainFrame == null)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "IsQueryCommandEnabled: Unable to locate frame");
        return false;
      }
      switch (command.ToLower())
      {
        case "copy":
          flag = this.browser.MainFrame.IsCommandEnabled(EditorCommand.Copy());
          break;
        case "cut":
          flag = this.browser.MainFrame.IsCommandEnabled(EditorCommand.Cut());
          break;
        case "delete":
          flag = this.browser.MainFrame.IsCommandEnabled(EditorCommand.Delete());
          break;
        case "paste":
          flag = this.browser.MainFrame.IsCommandEnabled(EditorCommand.Paste());
          break;
        case "redo":
          flag = this.browser.MainFrame.IsCommandEnabled(EditorCommand.Redo());
          break;
        case "undo":
          flag = this.browser.MainFrame.IsCommandEnabled(EditorCommand.Undo());
          break;
      }
      return flag;
    }

    public override string GetQueryCommandValue(string command)
    {
      return this.ExecuteJavascript<string>("document.queryCommandValue('" + command + "');", System.Type.Missing);
    }

    public override void AddDomEvents(string eventName, object caller)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "AddDOMEvents called for event" + eventName);
      if (this.IsBrowserDisposed)
        return;
      if (this.browser.MainFrame == null)
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "AddDomEvents: Unable to locate frame");
      else if (this.browser.MainFrame.Document == null)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "AddDomEvents: Unable to find the document in the frame.");
      }
      else
      {
        EventType eventType = new EventType(eventName);
        this.browser.MainFrame.Document.CreateEvent(eventType, new EventParameters.Builder().Build());
        EventHandler<DomEventArgs> eventHandler = (EventHandler<DomEventArgs>) ((s, e) =>
        {
          switch (e.Event.Type.Value.ToLower())
          {
            case "selectionchange":
              this.OnProcessSelectionChange((object) this, EventArgs.Empty);
              break;
            case "keydown":
              IKeyEvent keyEvent = (IKeyEvent) e.Event;
              this.OnProcessKeyDown((object) this, new HtmlEditorKeyDownEventArgs()
              {
                KeyCode = Convert.ToInt32((object) keyEvent.KeyCode),
                IsChromiumBrowser = true
              });
              break;
            case "readystatechange":
              this.OnProcessReadyStateChange((object) this, EventArgs.Empty);
              break;
            default:
              this.OnContentChanged((object) this, EventArgs.Empty);
              break;
          }
        });
        this.browser.MainFrame.Document.Events[eventType] += eventHandler;
      }
    }

    public override string GetHtmlBodyText([Optional] object frameName)
    {
      if (this.IsBrowserDisposed)
        return (string) null;
      IFrame frame = frameName == Missing.Value || frameName == null ? this.browser.MainFrame : this.browser.AllFrames.FirstOrDefault<IFrame>((Func<IFrame, bool>) (item => item.Name.Equals((string) frameName, StringComparison.InvariantCultureIgnoreCase)));
      if (frame == null)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "GetBrowserHtml: Unable to locate frame");
        return (string) null;
      }
      return frame.Document == null ? (string) null : frame.Document.DocumentElement.GetElementByTagName("body").TextContent;
    }

    public string GetBrowserAttributeValue(string elementId)
    {
      if (this.IsBrowserDisposed)
        return (string) null;
      if (string.IsNullOrWhiteSpace(elementId))
        return (string) null;
      try
      {
        return ((IFormControlElement) this.browser.MainFrame.Document?.DocumentElement?.GetElementById(elementId))?.Value;
      }
      catch (Exception ex)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "Error accessing DOM element value - " + ex.Message);
        return (string) null;
      }
    }

    public override Color GetSelectedFontColor()
    {
      string[] strArray = this.GetQueryCommandValue("forecolor").Replace("rgb(", string.Empty).Replace(")", string.Empty).Split(',');
      return Color.FromArgb(1, Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
    }

    public override void InsertField(string fieldID, string fieldName)
    {
      if (this.IsBrowserDisposed)
        return;
      if (this.browser.MainFrame == null)
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "InsertField: Unable to locate frame");
      else if (this.browser.MainFrame.Document == null || this.browser.MainFrame.Document.IsDisposed)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "InsertField: HTML document is missing or disposed");
      }
      else
      {
        IDocument document = this.browser.MainFrame.Document;
        IElement element1 = document.CreateElement("label");
        element1.Attributes["style"] = "BACKGROUND-COLOR: " + ColorTranslator.ToHtml(Color.Gainsboro);
        element1.Attributes["emid"] = fieldID;
        if (fieldName != null)
          element1.InnerText = "<<" + fieldID + " " + fieldName + ">>";
        else
          element1.InnerText = "<<" + fieldID + ">>";
        element1.Attributes["contentEditable"] = "false";
        if (document.Frame.Document.Type.ToString() != "Control")
        {
          document.Frame.Execute(EditorCommand.InsertHtml(element1.OuterHtml));
          document.Frame.Execute(EditorCommand.SelectWord());
        }
        else
        {
          IElement parent = (IElement) this.ExecuteJavascript<ITextAreaElement>("document.createRange()", (object) document.Frame).Parent;
          IElement element2 = this.ExecuteJavascript<IElement>("document.body.innerText", System.Type.Missing);
          if (parent == null || parent == element2)
            return;
          parent.OuterHtml = element1.OuterHtml;
        }
      }
    }

    public override void ShowContextMenu()
    {
      this.browser.ShowContextMenuHandler = (IHandler<ShowContextMenuParameters, ShowContextMenuResponse>) new AsyncHandler<ShowContextMenuParameters, ShowContextMenuResponse>(new Func<ShowContextMenuParameters, Task<ShowContextMenuResponse>>(this.ShowMenu));
    }

    private Task<ShowContextMenuResponse> ShowMenu(ShowContextMenuParameters parameters)
    {
      if (this.ContextMenuStrip == null)
        return (Task<ShowContextMenuResponse>) null;
      TaskCompletionSource<ShowContextMenuResponse> tcs = new TaskCompletionSource<ShowContextMenuResponse>();
      if (parameters.SpellCheckMenu != null)
        this.BeginInvoke((Delegate) (() =>
        {
          EventHandler<FocusRequestedEventArgs> onFocusRequested = (EventHandler<FocusRequestedEventArgs>) null;
          onFocusRequested = (EventHandler<FocusRequestedEventArgs>) ((sender, args) =>
          {
            this.BeginInvoke((Delegate) (() => this.ContextMenuStrip.Close()));
            parameters.Browser.FocusRequested -= onFocusRequested;
          });
          parameters.Browser.FocusRequested += onFocusRequested;
          this.ContextMenuStrip.Show((Control) this, new Point(parameters.Location.X, parameters.Location.Y));
          tcs.TrySetResult(ShowContextMenuResponse.Close());
        }));
      else
        tcs.TrySetResult(ShowContextMenuResponse.Close());
      return tcs.Task;
    }

    public override bool IsDocumentExists
    {
      get => this.browser.MainFrame.Document != null && !this.browser.MainFrame.Document.IsDisposed;
    }

    public void LoadModule(
      string hostUrl,
      string guestUrl,
      ModuleParameters parameters,
      bool allowDragDrop = false)
    {
      if (this.IsBrowserDisposed)
        return;
      using (PerformanceMeter.StartNew(nameof (LoadModule), "WebHost.LoadModule with guestUrl", 767, nameof (LoadModule), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Web.Host\\WebHost.cs"))
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 769, nameof (LoadModule), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Web.Host\\WebHost.cs");
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting AllowDrop: " + allowDragDrop.ToString());
        this.wbBrowserView.AllowDrop = allowDragDrop;
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Creating HostContext");
        this.hostContext = new HostContext(guestUrl, parameters, new Elli.Web.Host.PostMessageHandler(this.PostMessageHandler), new System.Action(this.UnloadHandler), this.scope, new Elli.Web.Host.AuditWindowHandler(this.AuditWindowHandler), new Elli.Web.Host.UpdateTemplateWindowHandler(this.UpdateTemplateWindowHandler));
        PerformanceMeter.Current.AddCheckpoint("hostContext created.", 778, nameof (LoadModule), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Web.Host\\WebHost.cs");
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'executeComplete' event hook");
        this.hostContext.executeComplete += new HostContext.ExecuteComplete(this.hostContext_ExecuteComplete);
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'GuestPageLoadCompleted' event hook");
        this.hostContext.GuestPageLoadCompleted += new HostContext.PageLoadComplete(this.hostContext_GuestPageLoadCompleted);
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.LoadURL: " + hostUrl);
        this.browser.Navigation.LoadUrl(hostUrl);
        PerformanceMeter.Current.AddCheckpoint("Browser LoadUrl called.", 792, nameof (LoadModule), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Web.Host\\WebHost.cs");
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Locking 'wbBrowser'");
        lock (this.wbBrowserView)
        {
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'url' = " + hostUrl);
          this.url = hostUrl;
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'finishedLoading' = false");
          this.finishedLoading = false;
        }
        PerformanceMeter.Current.AddCheckpoint("END", 807, nameof (LoadModule), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Web.Host\\WebHost.cs");
      }
    }

    public override void Navigate(string url)
    {
      if (this.IsBrowserDisposed)
        return;
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.LoadURL: " + url);
      this.browser.Navigation.LoadUrl(url).Wait();
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Locking 'wbBrowser'");
      lock (this.wbBrowserView)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'url' = " + url);
        this.url = url;
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'finishedLoading' = false");
        this.finishedLoading = false;
      }
    }

    public override void Navigate(
      string url,
      string postData,
      Dictionary<string, string> headerCollection)
    {
      if (this.IsBrowserDisposed)
        return;
      bool hasPostData = !string.IsNullOrEmpty(postData);
      if (!this.validateMandatoryHeader(headerCollection, hasPostData))
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "content-type header is mandatory for POST data requests. Request will not be processed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        LoadUrlParameters parameters = new LoadUrlParameters(url);
        if (hasPostData)
          parameters.UploadData = (UploadData) new TextData(postData);
        parameters.HttpHeaders = this.getHeaders(headerCollection, hasPostData) ?? parameters.HttpHeaders;
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.LoadURL with header: " + url);
        this.browser.Navigation.LoadUrl(parameters);
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Locking 'wbBrowser'");
        lock (this.wbBrowserView)
        {
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'url' = " + url);
          this.url = url;
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'finishedLoading' = false");
          this.finishedLoading = false;
        }
      }
    }

    private IEnumerable<HttpHeader> getHeaders(
      Dictionary<string, string> headerCollection,
      bool hasPostData)
    {
      IEnumerable<HttpHeader> headers = (IEnumerable<HttpHeader>) null;
      // ISSUE: explicit non-virtual call
      if (headerCollection != null && __nonvirtual (headerCollection.Count) > 0)
        headers = headerCollection.Select<KeyValuePair<string, string>, HttpHeader>((Func<KeyValuePair<string, string>, HttpHeader>) (header => new HttpHeader(header.Key, new string[1]
        {
          header.Value
        }))).AsEnumerable<HttpHeader>();
      return headers;
    }

    private bool validateMandatoryHeader(
      Dictionary<string, string> headerCollection,
      bool hasPostData)
    {
      bool flag = false;
      if (hasPostData)
      {
        // ISSUE: explicit non-virtual call
        if (headerCollection != null && __nonvirtual (headerCollection.Count) > 0)
          flag = headerCollection.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (k => k.Key.Equals("content-type", StringComparison.InvariantCultureIgnoreCase)));
      }
      else
        flag = true;
      return flag;
    }

    public T RaiseEvent<T>(string eventType, object eventParams, int millisecondsTimeout)
    {
      T obj = default (T);
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Checking Browser.IsDisposed");
      if (!this.IsBrowserDisposed)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Locking 'wbBrowser'");
        lock (this.wbBrowserView)
        {
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Getting 'finishedLoading': " + this.finishedLoading.ToString());
          if (!this.finishedLoading)
            return obj;
        }
        string javaScript = nameof (RaiseEvent);
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.ExecuteJavaScriptAndReturnValue: " + javaScript);
        IJsFunction result = this.browser.MainFrame?.ExecuteJavaScript<IJsFunction>(javaScript)?.Result;
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Checking Browser.ExecuteJavaScriptAndReturnValue Result");
        if (result != null)
        {
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Serializing 'eventParams'");
          string str = JsonConvert.SerializeObject(eventParams);
          JavascriptFunctionCallback<T> callback = new JavascriptFunctionCallback<T>();
          JsFunctionCallback functionCallback = (JsFunctionCallback) (jsObject =>
          {
            Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling JavascriptFunctionCallback.Callback");
            return callback.Callback(jsObject);
          });
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Function.Invoke: eventType: " + eventType + ", jsonParams: " + str);
          result.Invoke((IJsObject) null, (object) functionCallback, (object) eventType, (object) str, (object) millisecondsTimeout);
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling JavascriptFunctionCallback.WaitForCallback: " + (object) millisecondsTimeout);
          obj = callback.WaitForCallback(millisecondsTimeout);
        }
        else
          Tracing.Log(WebHost.sw, TraceLevel.Warning, nameof (WebHost), "Unable to find " + javaScript);
      }
      return obj;
    }

    public bool ShowDebugPopUp()
    {
      if (this.IsBrowserDisposed)
        return false;
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.GetRemoteDebuggingURL");
      string remoteDebuggingUrl = this.browser.DevTools.RemoteDebuggingUrl;
      Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
      int int32_1 = Convert.ToInt32((double) workingArea.Width * 0.9);
      int int32_2 = Convert.ToInt32((double) workingArea.Height * 0.9);
      int int32_3 = Convert.ToInt32(workingArea.Left + (workingArea.Width - int32_1) / 2);
      int int32_4 = Convert.ToInt32(workingArea.Top + (workingArea.Height - int32_2) / 2);
      string javaScript = string.Format("window.open(\"{0}\", \"{1}\", \"left={2},top={3},width={4},height={5}\");", (object) remoteDebuggingUrl, (object) "_blank", (object) int32_3, (object) int32_4, (object) int32_1, (object) int32_2);
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.ExecuteJavaScript: " + javaScript);
      this.browser.MainFrame.ExecuteJavaScript(javaScript);
      return true;
    }

    public T ExecuteAndReturnValue<T>(string functionName, object[] eventParams = null)
    {
      T obj = default (T);
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Checking Browser.IsDisposed");
      if (!this.IsBrowserDisposed)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Locking 'wbBrowser'");
        lock (this.wbBrowserView)
        {
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Getting 'finishedLoading': " + this.finishedLoading.ToString());
          if (!this.finishedLoading)
            return obj;
        }
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.ExecuteJavaScriptAndReturnValue: " + functionName);
        IJsFunction result = this.browser.MainFrame?.ExecuteJavaScript<IJsFunction>(functionName)?.Result;
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Checking Browser.ExecuteJavaScriptAndReturnValue Result");
        if (result != null)
        {
          if (eventParams == null)
          {
            Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Initializing eventParams since it cannot be null");
            eventParams = new object[0];
          }
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Function.Invoke: " + functionName);
          IJsObject jsObject = result.Invoke<IJsObject>((IJsObject) null, eventParams);
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Checking Function.Invoke Result");
          if (jsObject != null)
          {
            Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling JSObject.ToJSONString");
            string jsonString = jsObject.ToJsonString();
            Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling JsonConvert.DeserializeObject: " + jsonString);
            obj = JsonConvert.DeserializeObject<T>(jsonString);
          }
          else
            Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Null Function.Invoke Result: functionName=" + functionName + ", jsonParams=" + JsonConvert.SerializeObject((object) eventParams));
        }
        else
          Tracing.Log(WebHost.sw, TraceLevel.Warning, nameof (WebHost), "Unable to find " + functionName);
      }
      return obj;
    }

    public override void Refresh()
    {
      if (this.IsBrowserDisposed)
        return;
      this.Reload(false);
    }

    public void Reload(bool ignoreCache)
    {
      if (this.IsBrowserDisposed)
        return;
      if (!ignoreCache)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.Reload");
        this.browser.Navigation.Reload();
      }
      else
      {
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling Browser.Reload Ignoring Cache");
        this.browser.Navigation.ReloadIgnoringCache();
      }
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Locking 'wbBrowser'");
      lock (this.wbBrowserView)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'finishedLoading' = false");
        this.finishedLoading = false;
      }
    }

    public void ShowConsole()
    {
      this.csBrowser.Visible = true;
      this.csBrowser.IsCollapsed = false;
    }

    public void SetBrowserProperty(string sourceName, string propertyName, object propertyValue)
    {
      if (string.IsNullOrEmpty(sourceName))
        throw new ArgumentException("SourceName - " + sourceName + " cannot be empty.");
      if (string.IsNullOrEmpty(propertyName))
        throw new ArgumentException("PropertyName - " + propertyName + " cannot be empty.");
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Calling ExecuteJavaScript: " + sourceName);
      IJsObject result = this.browser.MainFrame?.ExecuteJavaScript<IJsObject>(sourceName)?.Result;
      if (result == null)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "ExecuteJavaScript Failed: " + sourceName);
        throw new Exception("No value returned for the source " + sourceName);
      }
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting " + propertyName + " Property: " + propertyValue);
      result.Properties[propertyName] = propertyValue;
    }

    public override string GetBrowserHtml([Optional] object frameName)
    {
      if (this.IsBrowserDisposed)
        return (string) null;
      IFrame frame = frameName == Missing.Value || frameName == null ? this.browser.MainFrame : this.browser.AllFrames.FirstOrDefault<IFrame>((Func<IFrame, bool>) (item => item.Name.Equals((string) frameName, StringComparison.InvariantCultureIgnoreCase)));
      if (frame != null)
        return frame.Html;
      Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "GetBrowserHtml: Unable to locate frame");
      return (string) null;
    }

    public bool DOMElementExists([Optional] object frameName)
    {
      if (this.IsBrowserDisposed)
        return false;
      IFrame frame = frameName == Missing.Value || frameName == null ? this.browser.MainFrame : this.browser.AllFrames.FirstOrDefault<IFrame>((Func<IFrame, bool>) (item => item.Name.Equals((string) frameName, StringComparison.InvariantCultureIgnoreCase)));
      if (frame == null)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Error, nameof (WebHost), "DOMElementExists: Unable to locate frame");
        return false;
      }
      return frame.Document != null && frame.Document.DocumentElement != null;
    }

    public override void SetOpaqueBackground()
    {
      using (Graphics graphics = Graphics.FromHwnd(this.wbBrowserView.Handle))
        this.wbBrowserView.Width = Convert.ToInt32(7.5 * (double) graphics.DpiX);
    }

    public void InitCustomPopUpHandler(Form parentWindowsForm)
    {
      if (this.IsBrowserDisposed)
        throw new ObjectDisposedException("Browser view is no longer initialized");
      this.browser.OpenPopupHandler = (IHandler<OpenPopupParameters>) new EncCustomPopUpHandler(parentWindowsForm);
    }

    private void InitPrintHandler(PrintSettings printSettings)
    {
      if (this.IsBrowserDisposed)
        throw new ObjectDisposedException("Browser view is no longer initialized");
      if (printSettings != null && !printSettings.IsSettingsValid())
        throw new Exception(string.Join(",", (IEnumerable<string>) printSettings?.SettingValidationErrors));
      this.customPrintHandler = new EncCustomPrintHandler();
      this.browser.PrintHtmlContentHandler = (IHandler<PrintHtmlContentParameters, PrintHtmlContentResponse>) this.customPrintHandler.PrintHtmlHandler(printSettings);
      this.customPrintHandler.OnPrintJobEvent += new EncCustomPrintHandler.EncPrintJobEvent(this.customPrintHandler_PrintJobEvent);
    }

    public void BeginPrint(PrintSettings printSettings, [Optional] object frameName, bool forcePrint = false)
    {
      this.InitPrintHandler(printSettings);
      if (frameName != null)
      {
        IBrowser browser = this.browser;
        if (browser == null)
          return;
        IEnumerable<IFrame> allFrames = browser.AllFrames;
        if (allFrames == null)
          return;
        allFrames.SingleOrDefault<IFrame>((Func<IFrame, bool>) (item => item.Name.Equals((string) frameName, StringComparison.InvariantCultureIgnoreCase)))?.Print();
      }
      else
        this.browser?.MainFrame.Print();
    }

    public event EventHandler<NavigationCompletedEventArgs> BrowserNavigationCompleted;

    public event EventHandler<StartLoadingEventArgs> BrowserNavigationStarted;

    public event WebHost.ConsoleMessageEventHandler ConsoleMessage;

    public event WebHost.ExecuteCompleteEventHandler ExecuteComplete;

    public event WebHost.FrameCompleteEventHandler FrameComplete;

    public event WebHost.GuestFrameCompleteEventHandler GuestFrameComplete;

    public event WebHost.HelpInvokedEventHandler HelpInvoked;

    public event WebHost.LoadingFrameEventHandler LoadingFrame;

    public event WebHost.PrintCompleteEventHandler PrintComplete;

    private void pnlBrowser_ClientSizeChanged(object sender, EventArgs e)
    {
      if (this.wbBrowserView == null)
        return;
      Rectangle displayRectangle = this.pnlBrowser.DisplayRectangle;
      this.wbBrowserView.SetBounds(displayRectangle.X, displayRectangle.Y, displayRectangle.Width, displayRectangle.Height);
    }

    private void HostContext_helpBrowserInvoked(object sender, InvokeHelpBrowserEventArgs args)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "HostContext: HelpBrowserInvoked: " + args.helpBrowserUrl);
      if (this.HelpInvoked == null || !this.IsHandleCreated)
        return;
      this.BeginInvoke((Delegate) (() => this.HelpInvoked(sender, args)));
    }

    private void browser_ConsoleMessage(object sender, ConsoleMessageReceivedEventArgs e)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Browser: ConsoleMessage: " + e.Message);
      if (!this.IsHandleCreated)
        return;
      this.BeginInvoke((Delegate) (() =>
      {
        this.rtbConsole.AppendText(e.Source + "(" + (object) e.LineNumber + "): " + e.Message + Environment.NewLine);
        this.rtbConsole.ScrollToCaret();
        if (this.ConsoleMessage == null)
          return;
        this.ConsoleMessage(sender, new ConsoleMessageEventArgs((int) e.Level, e.LineNumber, e.Message, e.Source));
      }));
    }

    private void browser_Dispose(object sender, EventArgs e)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Browser: Dispose");
      this.UnloadHandler();
    }

    private void browser_FinishLoadingFrame(object sender, FrameLoadFinishedEventArgs e)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Browser: FinishLoadingFrame: " + e.ValidatedUrl + " [" + e.Frame.IsMain.ToString() + "]");
      if (this.FrameComplete == null || !this.IsHandleCreated)
        return;
      FinishedLoadingEventArgs args = new FinishedLoadingEventArgs((object) e.Frame.Name, e.Frame?.IsMain.Value, e.ValidatedUrl);
      this.BeginInvoke((Delegate) (() => this.FrameComplete(sender, args)));
    }

    private void browser_ScriptContextCreated(InjectJsParameters jsArgs)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Browser: ScriptContextCreated: " + jsArgs.Frame.IsMain.ToString());
      if (this.hostContext == null || !jsArgs.Frame.IsMain)
        return;
      this.SetBrowserProperty("window", "host", (object) this.hostContext);
    }

    private void browser_StartLoadingFrame(object sender, NavigationStartedEventArgs e)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Browser: StartLoadingFrame: " + e.Url + " [" + e.IsMainFrame.ToString() + "]");
      if (this.LoadingFrame != null && this.IsHandleCreated)
      {
        StartLoadingEventArgs args = new StartLoadingEventArgs(e.IsMainFrame, e.Url, e.IsSameDocument);
        this.BeginInvoke((Delegate) (() => this.LoadingFrame(sender, args)));
      }
      if (this.BrowserNavigationStarted == null || !this.IsHandleCreated)
        return;
      StartLoadingEventArgs args1 = new StartLoadingEventArgs(e.IsMainFrame, e.Url, e.IsSameDocument);
      this.BeginInvoke((Delegate) (() => this.BrowserNavigationStarted((object) this, args1)));
    }

    private void browser_NavigationFinished(object sender, NavigationFinishedEventArgs e)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Browser: StartLoadingFrame: " + e.Url);
      if (this.BrowserNavigationCompleted == null || !this.IsHandleCreated)
        return;
      this.BrowserNavigationCompleted((object) this, new NavigationCompletedEventArgs(e.Frame?.Name, e.ErrorCode.ToString(), e.HasCommitted, e.IsErrorPage, e.IsSameDocument, e.Url, e.WasServerRedirect));
    }

    protected StartNavigationResponse OnBeforeNavigation(StartNavigationParameters p)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Browser: StartNavigationHandler: " + p.Url + " [" + p.IsMainFrame.ToString() + "]");
      BeforeNavigationEventArgs args = BeforeNavigationEventArgs.GetArgs(p);
      this.OnBeforeNavigation((object) this, args);
      return args.Cancel ? StartNavigationResponse.Ignore() : StartNavigationResponse.Start();
    }

    private void browser_TitleChanged(object sender, TitleChangedEventArgs e)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Browser: TitleChanged: " + e.Title);
      if (!this.IsHandleCreated)
        return;
      this.OnTitleChanged(sender, new TitleChangeEventArgs(e.Title));
    }

    private void browser_LoadFinished(object sender, LoadFinishedEventArgs e)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Browser: LoadFinished: " + e.Browser.Url);
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Locking 'wbBrowser'");
      lock (this.wbBrowserView)
      {
        Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Checking 'url': " + this.url);
        if (string.Equals(this.url, e.Browser.Url, StringComparison.OrdinalIgnoreCase))
        {
          Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Setting 'finishedLoading' = true");
          this.finishedLoading = true;
        }
      }
      if (!this.IsHandleCreated)
        return;
      this.BeginInvoke((Delegate) (() =>
      {
        Point location = this.Location;
        int x = location.X;
        location = this.Location;
        int y = location.Y + 1;
        this.Location = new Point(x, y);
      }));
      this.BeginInvoke((Delegate) (() =>
      {
        Point location = this.Location;
        int x = location.X;
        location = this.Location;
        int y = location.Y - 1;
        this.Location = new Point(x, y);
      }));
      FinishedLoadingEventArgs args = new FinishedLoadingEventArgs((object) e.Browser.MainFrame.Name, e.Browser.Url);
      this.BeginInvoke((Delegate) (() => this.OnPageComplete(sender, args)));
    }

    private void customPrintHandler_PrintJobEvent(object sender, PrintJobEventArgs e)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Browser: PrintJobEvent: " + e.Success.ToString());
      if (this.PrintComplete == null || !this.IsHandleCreated)
        return;
      this.BeginInvoke((Delegate) (() => this.PrintComplete(sender, e)));
    }

    protected InputEventResponse OnMouseReleasedHandler(IMouseReleasedEventArgs e)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Browser: MouseReleasedHandler: " + (object) e.Button + " [" + (object) e.KeyModifiers + "]");
      this.browser.Focus();
      MouseReleasedEventArgs e1 = new MouseReleasedEventArgs(false);
      this.OnMouseReleased((object) this, e1);
      return e1.Cancel ? InputEventResponse.Suppress : InputEventResponse.Proceed;
    }

    private void hostContext_ExecuteComplete(object sender, ExecuteCompleteEventArgs e)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "HostContext: ExecuteComplete");
      if (this.ExecuteComplete == null || !this.IsHandleCreated)
        return;
      this.BeginInvoke((Delegate) (() => this.ExecuteComplete(sender, e)));
    }

    private void hostContext_GuestPageLoadCompleted(object sender, EventArgs e)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "HostContext: GuestPageLoadCompleted");
      if (this.GuestFrameComplete == null || !this.IsHandleCreated)
        return;
      this.BeginInvoke((Delegate) (() => this.GuestFrameComplete(sender, e)));
    }

    public bool AuditWindowHandler(string payload)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "AuditWindowHandler: " + payload);
      return this.auditWindowHandler != null && this.IsHandleCreated && (bool) this.Invoke((Delegate) (() => this.auditWindowHandler(payload)));
    }

    public string PostMessageHandler(string payload)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "PostMessageHandler: " + payload);
      return this.postMessageHandler != null && this.IsHandleCreated ? (string) this.Invoke((Delegate) (() => this.postMessageHandler(payload))) : string.Empty;
    }

    public void UnloadHandler()
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), nameof (UnloadHandler));
      if (this.unloadHandler == null || !this.IsHandleCreated)
        return;
      this.BeginInvoke((Delegate) (() => this.unloadHandler()));
    }

    public bool UpdateTemplateWindowHandler(string payload)
    {
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "UpdateTemplateWindowHandler: " + payload);
      return this.updateTemplateWindowHandler != null && this.IsHandleCreated && (bool) this.Invoke((Delegate) (() => this.updateTemplateWindowHandler(payload)));
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
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Unsubscribing from Browser Events");
      this.browser.ConsoleMessageReceived -= new EventHandler<ConsoleMessageReceivedEventArgs>(this.browser_ConsoleMessage);
      this.browser.Disposed -= new EventHandler(this.browser_Dispose);
      this.browser.Navigation.FrameLoadFinished -= new EventHandler<FrameLoadFinishedEventArgs>(this.browser_FinishLoadingFrame);
      this.browser.Navigation.NavigationStarted -= new EventHandler<NavigationStartedEventArgs>(this.browser_StartLoadingFrame);
      this.browser.Navigation.LoadFinished -= new EventHandler<LoadFinishedEventArgs>(this.browser_LoadFinished);
      this.browser.TitleChanged -= new EventHandler<TitleChangedEventArgs>(this.browser_TitleChanged);
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Unsubscribing from Browser Handlers");
      this.browser.InjectJsHandler = (IHandler<InjectJsParameters>) null;
      this.browser.Keyboard.KeyReleased.Handler = (IHandler<IKeyReleasedEventArgs, InputEventResponse>) null;
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Disposing BrowserView");
      this.wbBrowserView.Dispose();
      this.wbBrowserView = (BrowserView) null;
      Tracing.Log(WebHost.sw, TraceLevel.Verbose, nameof (WebHost), "Disposing Browser");
      this.browser.Dispose();
      this.browser = (IBrowser) null;
    }

    private void InitializeComponent()
    {
      this.rtbConsole = new RichTextBox();
      this.csBrowser = new CollapsibleSplitter();
      this.pnlConsole = new BorderPanel();
      this.pnlBrowser = new BorderPanel();
      this.pnlConsole.SuspendLayout();
      this.SuspendLayout();
      this.rtbConsole.BorderStyle = BorderStyle.None;
      this.rtbConsole.Dock = DockStyle.Fill;
      this.rtbConsole.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rtbConsole.Location = new Point(0, 1);
      this.rtbConsole.Name = "rtbConsole";
      this.rtbConsole.ReadOnly = true;
      this.rtbConsole.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
      this.rtbConsole.Size = new Size(1090, 105);
      this.rtbConsole.TabIndex = 3;
      this.rtbConsole.Text = "";
      this.csBrowser.AnimationDelay = 20;
      this.csBrowser.AnimationStep = 20;
      this.csBrowser.BorderStyle3D = Border3DStyle.Flat;
      this.csBrowser.ControlToHide = (Control) this.pnlConsole;
      this.csBrowser.Dock = DockStyle.Bottom;
      this.csBrowser.ExpandParentForm = false;
      this.csBrowser.Location = new Point(0, 258);
      this.csBrowser.Margin = new Padding(4, 5, 4, 5);
      this.csBrowser.Name = "csBrowser";
      this.csBrowser.TabIndex = 1;
      this.csBrowser.TabStop = false;
      this.csBrowser.UseAnimations = false;
      this.csBrowser.VisualStyle = VisualStyles.Encompass;
      this.pnlConsole.Borders = AnchorStyles.Top;
      this.pnlConsole.Controls.Add((Control) this.rtbConsole);
      this.pnlConsole.Dock = DockStyle.Bottom;
      this.pnlConsole.Location = new Point(0, 265);
      this.pnlConsole.Margin = new Padding(4, 5, 4, 5);
      this.pnlConsole.Name = "pnlConsole";
      this.pnlConsole.Size = new Size(1090, 106);
      this.pnlConsole.TabIndex = 2;
      this.pnlConsole.Visible = false;
      this.pnlBrowser.Borders = AnchorStyles.None;
      this.pnlBrowser.Dock = DockStyle.Fill;
      this.pnlBrowser.Location = new Point(0, 0);
      this.pnlBrowser.Margin = new Padding(4, 5, 4, 5);
      this.pnlBrowser.Name = "pnlBrowser";
      this.pnlBrowser.Size = new Size(1090, 258);
      this.pnlBrowser.TabIndex = 0;
      this.pnlBrowser.ClientSizeChanged += new EventHandler(this.pnlBrowser_ClientSizeChanged);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlBrowser);
      this.Controls.Add((Control) this.csBrowser);
      this.Controls.Add((Control) this.pnlConsole);
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = nameof (WebHost);
      this.Size = new Size(1090, 371);
      this.pnlConsole.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public delegate void ConsoleMessageEventHandler(object sender, ConsoleMessageEventArgs e);

    public delegate void ExecuteCompleteEventHandler(object sender, ExecuteCompleteEventArgs e);

    public delegate void FrameCompleteEventHandler(object sender, FinishedLoadingEventArgs e);

    public delegate void GuestFrameCompleteEventHandler(object sender, EventArgs args);

    public delegate void HelpInvokedEventHandler(object sender, InvokeHelpBrowserEventArgs args);

    public delegate void LoadingFrameEventHandler(object sender, StartLoadingEventArgs e);

    public delegate void PrintCompleteEventHandler(object sender, PrintJobEventArgs e);
  }
}
