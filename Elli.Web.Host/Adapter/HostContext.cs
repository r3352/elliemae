// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.Adapter.HostContext
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.LoanUtils.Authentication;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Elli.Web.Host.Adapter
{
  public class HostContext
  {
    private static readonly string sw = Tracing.SwCommon;
    private static string className = nameof (HostContext);
    private string moduleUrl;
    private ModuleParameters parameters;
    private PostMessageHandler postMessageHandler;
    private AuditWindowHandler auditWindowHandler;
    private UpdateTemplateWindowHandler updateTemplateWindowHandler;
    private Action unloadHandler;
    private string scope;

    public event HostContext.ExecuteComplete executeComplete;

    public event HostContext.PageLoadComplete GuestPageLoadCompleted;

    public event HostContext.InvokeHelpBrowser helpBrowserInvoked;

    public HostContext(
      string moduleUrl,
      ModuleParameters parameters,
      PostMessageHandler postMessageHandler,
      Action unloadHandler,
      string scope,
      AuditWindowHandler auditWindowHandler = null,
      UpdateTemplateWindowHandler updateTemplateWindowHandler = null)
    {
      this.moduleUrl = moduleUrl;
      this.parameters = parameters;
      this.unloadHandler = unloadHandler;
      this.scope = scope;
      this.postMessageHandler = postMessageHandler;
      this.auditWindowHandler = auditWindowHandler;
      this.updateTemplateWindowHandler = updateTemplateWindowHandler;
    }

    public string GetModuleInfo()
    {
      JObject jobject = new JObject();
      jobject["moduleURL"] = (JToken) this.moduleUrl;
      jobject["version"] = (JToken) VersionInformation.CurrentVersion.GetExtendedVersion(Session.EncompassEdition);
      jobject["user"] = (JToken) JObject.FromObject((object) (this.parameters.User ?? new ModuleUser()));
      try
      {
        AccessToken accessToken = new OAuth2Utils(Session.ISession, Session.StartupInfo).GetAccessToken(this.scope);
        OAuth2.AuthToken authToken = new OAuth2.AuthToken()
        {
          token_type = accessToken.Type,
          access_token = accessToken.Token,
          host_name = accessToken.HostName
        };
        string key = "oauthToken";
        if (this.parameters.Parameters.ContainsKey(key))
          this.parameters.Parameters[key] = (object) authToken;
        else
          this.parameters.Parameters.Add(key, (object) authToken);
      }
      catch (Exception ex)
      {
        Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Error, "Error Getting AccessToken: " + (object) ex);
      }
      jobject["parameters"] = (JToken) JObject.FromObject((object) (this.parameters.Parameters ?? new Dictionary<string, object>()));
      return jobject.ToString();
    }

    public string GetOAuthCode(string guestClientID)
    {
      try
      {
        return new OAuth2Utils(Session.ISession, Session.StartupInfo).GetAuthCodeForGuest(guestClientID).Code;
      }
      catch (Exception ex)
      {
        return new JObject()
        {
          ["errorMessages"] = ((JToken) ex.Message)
        }.ToString();
      }
    }

    public string GetAccessToken()
    {
      string accessToken = string.Empty;
      try
      {
        accessToken = new OAuth2Utils(Session.ISession, Session.StartupInfo).GetAccessToken(this.scope, true).TypeAndToken;
      }
      catch (Exception ex)
      {
        Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Error, "GetAccessToken() : Error getting token" + (object) ex);
      }
      return accessToken;
    }

    public void Unload()
    {
      if (this.unloadHandler == null)
        return;
      this.unloadHandler();
    }

    public void Log(string message, string logLevel)
    {
      TraceLevel l = TraceLevel.Info;
      switch (logLevel.ToLower())
      {
        case "info":
          l = TraceLevel.Info;
          break;
        case "off":
          l = TraceLevel.Off;
          break;
        case "error":
          l = TraceLevel.Error;
          break;
        case "warning":
          l = TraceLevel.Warning;
          break;
        case "verbose":
          l = TraceLevel.Verbose;
          break;
      }
      Tracing.Log(HostContext.sw, HostContext.className, l, message);
    }

    public string PostMessage(string payload)
    {
      Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Info, "PostMessage: " + payload);
      JObject jobject = new JObject();
      try
      {
        if (this.postMessageHandler != null)
          return this.postMessageHandler(payload);
      }
      catch (Exception ex)
      {
        Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Error, "Error Getting PostMessage response: " + (object) ex);
      }
      return jobject.ToString();
    }

    public bool ShowAuditWindow(string payload)
    {
      Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Info, "ShowAuditWindow: " + payload);
      try
      {
        return this.auditWindowHandler != null && this.auditWindowHandler(payload);
      }
      catch (Exception ex)
      {
        Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Error, "ShowAuditWindow failed: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Error trying to Show Audit Window:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    public bool ShowUpdateTemplateWindow(string payload)
    {
      Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Info, "ShowUpdateTemplateWindow: " + payload);
      try
      {
        return this.updateTemplateWindowHandler != null && this.updateTemplateWindowHandler(payload);
      }
      catch (Exception ex)
      {
        Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Error, "ShowUpdateTemplateWindow failed: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Error trying to Show Update Template Window:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    public void Print(string url, string authorizationHeader, int currentPageNumber)
    {
      Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Info, "Print: " + url);
      try
      {
        Form.ActiveForm.Invoke((Delegate) (() => Session.Application.GetService<IEFolder>().Print(Session.LoanDataMgr, url, authorizationHeader, currentPageNumber)));
      }
      catch (Exception ex)
      {
        Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Error, "Print failed: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to print the document:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public void LoadGuestPageCompleted()
    {
      if (this.GuestPageLoadCompleted == null)
        return;
      this.GuestPageLoadCompleted((object) this, (EventArgs) null);
    }

    public void Execute(string jsonData)
    {
      try
      {
        XDocument xdocument = XDocument.Load((XmlReader) JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(jsonData), new XmlDictionaryReaderQuotas()));
        if (this.executeComplete == null)
          return;
        this.executeComplete((object) this, new ExecuteCompleteEventArgs()
        {
          jDoc = xdocument
        });
      }
      catch (Exception ex)
      {
        ex.Message.ToString();
      }
    }

    public bool OpenLoan(string loanGuid)
    {
      Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Info, "OpenLoan: " + loanGuid);
      try
      {
        return Session.Application.GetService<ILoanConsole>().OpenLoan(loanGuid);
      }
      catch (Exception ex)
      {
        Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Error, "OpenLoan failed: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to open the loan:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    public void IsDirty(bool isDirty, string formName)
    {
      Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Info, "IsDirty: " + isDirty.ToString());
      try
      {
        switch (formName)
        {
          case "LockComparisonSetting":
            Session.Application.GetService<ILockComparisonConsole>().SetIsDirty(isDirty);
            break;
          case "BidTapeTemplate":
            Session.Application.GetService<INormalizedBidTapeTemplate>().SetIsDirty(isDirty);
            break;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(HostContext.sw, HostContext.className, TraceLevel.Error, "IsDirty failed: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, string.Format("The following error occurred when set IsDirty flag for {0}:\n\n", (object) formName) + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public void invokeHelpBrowser(string helpUrl)
    {
      if (this.helpBrowserInvoked == null)
        return;
      this.helpBrowserInvoked((object) this, new InvokeHelpBrowserEventArgs()
      {
        helpBrowserUrl = helpUrl
      });
    }

    public delegate void ExecuteComplete(object sender, ExecuteCompleteEventArgs e);

    public delegate void PageLoadComplete(object sender, EventArgs args);

    public delegate void InvokeHelpBrowser(object sender, InvokeHelpBrowserEventArgs args);
  }
}
