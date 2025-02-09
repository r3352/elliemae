// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.DiagnosticsService
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using Microsoft.Web.Services2;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "DiagnosticsServiceSoap", Namespace = "http://encompass.elliemae.com/jedservices/")]
  public class DiagnosticsService : WebServicesClientProtocol
  {
    private SendOrPostCallback PostDiagnosticsOperationCompleted;
    private SendOrPostCallback PostPerformanceDataOperationCompleted;
    private SendOrPostCallback GetPerformanceSettingsOperationCompleted;

    public event ChunkHandler ChunkSent;

    protected override WebRequest GetWebRequest(Uri uri)
    {
      SoapWebRequest webRequest = (SoapWebRequest) base.GetWebRequest(uri);
      FieldInfo field = webRequest.GetType().GetField("_request", BindingFlags.Instance | BindingFlags.NonPublic);
      CustomWebRequest customWebRequest = new CustomWebRequest(uri);
      customWebRequest.ChunkSent += new ChunkHandler(this.webRequest_ChunkSent);
      field.SetValue((object) webRequest, (object) customWebRequest);
      return (WebRequest) webRequest;
    }

    private void webRequest_ChunkSent(object sender, ChunkHandlerEventArgs e)
    {
      if (this.ChunkSent == null)
        return;
      this.ChunkSent((object) this, e);
    }

    public DiagnosticsService()
    {
      this.Url = "https://encompass.elliemae.com/jedservices/Diagnostics.asmx";
    }

    public DiagnosticsService(string jedServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(jedServiceUrl) || !Uri.IsWellFormedUriString(jedServiceUrl, UriKind.Absolute))
        this.Url = "https://encompass.elliemae.com/jedservices/Diagnostics.asmx";
      else
        this.Url = jedServiceUrl + "Diagnostics.asmx";
    }

    public event PostDiagnosticsCompletedEventHandler PostDiagnosticsCompleted;

    public event PostPerformanceDataCompletedEventHandler PostPerformanceDataCompleted;

    public event GetPerformanceSettingsCompletedEventHandler GetPerformanceSettingsCompleted;

    [SoapDocumentMethod("http://encompass.elliemae.com/jedservices/PostDiagnostics", RequestNamespace = "http://encompass.elliemae.com/jedservices/", ResponseNamespace = "http://encompass.elliemae.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string PostDiagnostics(
      string clientID,
      string userID,
      string caseNumber,
      string message)
    {
      return (string) this.Invoke(nameof (PostDiagnostics), new object[4]
      {
        (object) clientID,
        (object) userID,
        (object) caseNumber,
        (object) message
      })[0];
    }

    public IAsyncResult BeginPostDiagnostics(
      string clientID,
      string userID,
      string caseNumber,
      string message,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("PostDiagnostics", new object[4]
      {
        (object) clientID,
        (object) userID,
        (object) caseNumber,
        (object) message
      }, callback, asyncState);
    }

    public string EndPostDiagnostics(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void PostDiagnosticsAsync(
      string clientID,
      string userID,
      string caseNumber,
      string message)
    {
      this.PostDiagnosticsAsync(clientID, userID, caseNumber, message, (object) null);
    }

    public void PostDiagnosticsAsync(
      string clientID,
      string userID,
      string caseNumber,
      string message,
      object userState)
    {
      if (this.PostDiagnosticsOperationCompleted == null)
        this.PostDiagnosticsOperationCompleted = new SendOrPostCallback(this.OnPostDiagnosticsOperationCompleted);
      this.InvokeAsync("PostDiagnostics", new object[4]
      {
        (object) clientID,
        (object) userID,
        (object) caseNumber,
        (object) message
      }, this.PostDiagnosticsOperationCompleted, userState);
    }

    private void OnPostDiagnosticsOperationCompleted(object arg)
    {
      if (this.PostDiagnosticsCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.PostDiagnosticsCompleted((object) this, new PostDiagnosticsCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/jedservices/PostPerformanceData", RequestNamespace = "http://encompass.elliemae.com/jedservices/", ResponseNamespace = "http://encompass.elliemae.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void PostPerformanceData(
      string clientID,
      string userID,
      string server,
      bool hosted,
      string activity,
      int duration,
      string xmlData)
    {
      this.Invoke(nameof (PostPerformanceData), new object[7]
      {
        (object) clientID,
        (object) userID,
        (object) server,
        (object) hosted,
        (object) activity,
        (object) duration,
        (object) xmlData
      });
    }

    public IAsyncResult BeginPostPerformanceData(
      string clientID,
      string userID,
      string server,
      bool hosted,
      string activity,
      int duration,
      string xmlData,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("PostPerformanceData", new object[7]
      {
        (object) clientID,
        (object) userID,
        (object) server,
        (object) hosted,
        (object) activity,
        (object) duration,
        (object) xmlData
      }, callback, asyncState);
    }

    public void EndPostPerformanceData(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void PostPerformanceDataAsync(
      string clientID,
      string userID,
      string server,
      bool hosted,
      string activity,
      int duration,
      string xmlData)
    {
      this.PostPerformanceDataAsync(clientID, userID, server, hosted, activity, duration, xmlData, (object) null);
    }

    public void PostPerformanceDataAsync(
      string clientID,
      string userID,
      string server,
      bool hosted,
      string activity,
      int duration,
      string xmlData,
      object userState)
    {
      if (this.PostPerformanceDataOperationCompleted == null)
        this.PostPerformanceDataOperationCompleted = new SendOrPostCallback(this.OnPostPerformanceDataOperationCompleted);
      this.InvokeAsync("PostPerformanceData", new object[7]
      {
        (object) clientID,
        (object) userID,
        (object) server,
        (object) hosted,
        (object) activity,
        (object) duration,
        (object) xmlData
      }, this.PostPerformanceDataOperationCompleted, userState);
    }

    private void OnPostPerformanceDataOperationCompleted(object arg)
    {
      if (this.PostPerformanceDataCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.PostPerformanceDataCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/jedservices/GetPerformanceSettings", RequestNamespace = "http://encompass.elliemae.com/jedservices/", ResponseNamespace = "http://encompass.elliemae.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public PerformanceSettings GetPerformanceSettings(string clientId)
    {
      return (PerformanceSettings) this.Invoke(nameof (GetPerformanceSettings), new object[1]
      {
        (object) clientId
      })[0];
    }

    public IAsyncResult BeginGetPerformanceSettings(
      string clientId,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetPerformanceSettings", new object[1]
      {
        (object) clientId
      }, callback, asyncState);
    }

    public PerformanceSettings EndGetPerformanceSettings(IAsyncResult asyncResult)
    {
      return (PerformanceSettings) this.EndInvoke(asyncResult)[0];
    }

    public void GetPerformanceSettingsAsync(string clientId)
    {
      this.GetPerformanceSettingsAsync(clientId, (object) null);
    }

    public void GetPerformanceSettingsAsync(string clientId, object userState)
    {
      if (this.GetPerformanceSettingsOperationCompleted == null)
        this.GetPerformanceSettingsOperationCompleted = new SendOrPostCallback(this.OnGetPerformanceSettingsOperationCompleted);
      this.InvokeAsync("GetPerformanceSettings", new object[1]
      {
        (object) clientId
      }, this.GetPerformanceSettingsOperationCompleted, userState);
    }

    private void OnGetPerformanceSettingsOperationCompleted(object arg)
    {
      if (this.GetPerformanceSettingsCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetPerformanceSettingsCompleted((object) this, new GetPerformanceSettingsCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
