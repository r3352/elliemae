// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.UpdateService
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "UpdateServiceSoap", Namespace = "http://www.elliemae.com/encompass")]
  [XmlInclude(typeof (object[]))]
  public class UpdateService : SoapHttpClientProtocol
  {
    private SendOrPostCallback GetUpdateVersionInfoOperationCompleted;
    private SendOrPostCallback GetUpdateVersionInfoExOperationCompleted;
    private SendOrPostCallback ValidateUpdateOperationCompleted;
    private SendOrPostCallback GetVersionHotfixesOperationCompleted;
    private SendOrPostCallback GetReleaseInfoOperationCompleted;

    public UpdateService()
    {
      this.Url = "https://encompass.elliemae.com/VersionUpdate/UpdateService.asmx";
    }

    public UpdateService(string serviceUrl)
    {
      if (string.IsNullOrWhiteSpace(serviceUrl) || !Uri.IsWellFormedUriString(serviceUrl, UriKind.Absolute))
        this.Url = "https://encompass.elliemae.com/VersionUpdate/UpdateService.asmx";
      else
        this.Url = serviceUrl;
    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
      HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(uri);
      webRequest.KeepAlive = false;
      return (WebRequest) webRequest;
    }

    public event GetUpdateVersionInfoCompletedEventHandler GetUpdateVersionInfoCompleted;

    public event GetUpdateVersionInfoExCompletedEventHandler GetUpdateVersionInfoExCompleted;

    public event ValidateUpdateCompletedEventHandler ValidateUpdateCompleted;

    public event GetVersionHotfixesCompletedEventHandler GetVersionHotfixesCompleted;

    public event GetReleaseInfoCompletedEventHandler GetReleaseInfoCompleted;

    [SoapDocumentMethod("http://www.elliemae.com/encompass/GetUpdateVersionInfo", RequestNamespace = "http://www.elliemae.com/encompass", ResponseNamespace = "http://www.elliemae.com/encompass", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public object[] GetUpdateVersionInfo(string version, string clientId)
    {
      return (object[]) this.Invoke(nameof (GetUpdateVersionInfo), new object[2]
      {
        (object) version,
        (object) clientId
      })[0];
    }

    public IAsyncResult BeginGetUpdateVersionInfo(
      string version,
      string clientId,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetUpdateVersionInfo", new object[2]
      {
        (object) version,
        (object) clientId
      }, callback, asyncState);
    }

    public object[] EndGetUpdateVersionInfo(IAsyncResult asyncResult)
    {
      return (object[]) this.EndInvoke(asyncResult)[0];
    }

    public void GetUpdateVersionInfoAsync(string version, string clientId)
    {
      this.GetUpdateVersionInfoAsync(version, clientId, (object) null);
    }

    public void GetUpdateVersionInfoAsync(string version, string clientId, object userState)
    {
      if (this.GetUpdateVersionInfoOperationCompleted == null)
        this.GetUpdateVersionInfoOperationCompleted = new SendOrPostCallback(this.OnGetUpdateVersionInfoOperationCompleted);
      this.InvokeAsync("GetUpdateVersionInfo", new object[2]
      {
        (object) version,
        (object) clientId
      }, this.GetUpdateVersionInfoOperationCompleted, userState);
    }

    private void OnGetUpdateVersionInfoOperationCompleted(object arg)
    {
      if (this.GetUpdateVersionInfoCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetUpdateVersionInfoCompleted((object) this, new GetUpdateVersionInfoCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/encompass/GetUpdateVersionInfoEx", RequestNamespace = "http://www.elliemae.com/encompass", ResponseNamespace = "http://www.elliemae.com/encompass", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public object[] GetUpdateVersionInfoEx(string version, string clientId, int affectedSystems)
    {
      return (object[]) this.Invoke(nameof (GetUpdateVersionInfoEx), new object[3]
      {
        (object) version,
        (object) clientId,
        (object) affectedSystems
      })[0];
    }

    public IAsyncResult BeginGetUpdateVersionInfoEx(
      string version,
      string clientId,
      int affectedSystems,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetUpdateVersionInfoEx", new object[3]
      {
        (object) version,
        (object) clientId,
        (object) affectedSystems
      }, callback, asyncState);
    }

    public object[] EndGetUpdateVersionInfoEx(IAsyncResult asyncResult)
    {
      return (object[]) this.EndInvoke(asyncResult)[0];
    }

    public void GetUpdateVersionInfoExAsync(string version, string clientId, int affectedSystems)
    {
      this.GetUpdateVersionInfoExAsync(version, clientId, affectedSystems, (object) null);
    }

    public void GetUpdateVersionInfoExAsync(
      string version,
      string clientId,
      int affectedSystems,
      object userState)
    {
      if (this.GetUpdateVersionInfoExOperationCompleted == null)
        this.GetUpdateVersionInfoExOperationCompleted = new SendOrPostCallback(this.OnGetUpdateVersionInfoExOperationCompleted);
      this.InvokeAsync("GetUpdateVersionInfoEx", new object[3]
      {
        (object) version,
        (object) clientId,
        (object) affectedSystems
      }, this.GetUpdateVersionInfoExOperationCompleted, userState);
    }

    private void OnGetUpdateVersionInfoExOperationCompleted(object arg)
    {
      if (this.GetUpdateVersionInfoExCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetUpdateVersionInfoExCompleted((object) this, new GetUpdateVersionInfoExCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/encompass/ValidateUpdate", RequestNamespace = "http://www.elliemae.com/encompass", ResponseNamespace = "http://www.elliemae.com/encompass", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void ValidateUpdate(
      string clientID,
      string productType,
      string updateVersion,
      string updateSource)
    {
      this.Invoke(nameof (ValidateUpdate), new object[4]
      {
        (object) clientID,
        (object) productType,
        (object) updateVersion,
        (object) updateSource
      });
    }

    public IAsyncResult BeginValidateUpdate(
      string clientID,
      string productType,
      string updateVersion,
      string updateSource,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("ValidateUpdate", new object[4]
      {
        (object) clientID,
        (object) productType,
        (object) updateVersion,
        (object) updateSource
      }, callback, asyncState);
    }

    public void EndValidateUpdate(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void ValidateUpdateAsync(
      string clientID,
      string productType,
      string updateVersion,
      string updateSource)
    {
      this.ValidateUpdateAsync(clientID, productType, updateVersion, updateSource, (object) null);
    }

    public void ValidateUpdateAsync(
      string clientID,
      string productType,
      string updateVersion,
      string updateSource,
      object userState)
    {
      if (this.ValidateUpdateOperationCompleted == null)
        this.ValidateUpdateOperationCompleted = new SendOrPostCallback(this.OnValidateUpdateOperationCompleted);
      this.InvokeAsync("ValidateUpdate", new object[4]
      {
        (object) clientID,
        (object) productType,
        (object) updateVersion,
        (object) updateSource
      }, this.ValidateUpdateOperationCompleted, userState);
    }

    private void OnValidateUpdateOperationCompleted(object arg)
    {
      if (this.ValidateUpdateCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.ValidateUpdateCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/encompass/GetVersionHotfixes", RequestNamespace = "http://www.elliemae.com/encompass", ResponseNamespace = "http://www.elliemae.com/encompass", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public ReleaseInfo[] GetVersionHotfixes(string majorVersion, string clientId)
    {
      return (ReleaseInfo[]) this.Invoke(nameof (GetVersionHotfixes), new object[2]
      {
        (object) majorVersion,
        (object) clientId
      })[0];
    }

    public IAsyncResult BeginGetVersionHotfixes(
      string majorVersion,
      string clientId,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetVersionHotfixes", new object[2]
      {
        (object) majorVersion,
        (object) clientId
      }, callback, asyncState);
    }

    public ReleaseInfo[] EndGetVersionHotfixes(IAsyncResult asyncResult)
    {
      return (ReleaseInfo[]) this.EndInvoke(asyncResult)[0];
    }

    public void GetVersionHotfixesAsync(string majorVersion, string clientId)
    {
      this.GetVersionHotfixesAsync(majorVersion, clientId, (object) null);
    }

    public void GetVersionHotfixesAsync(string majorVersion, string clientId, object userState)
    {
      if (this.GetVersionHotfixesOperationCompleted == null)
        this.GetVersionHotfixesOperationCompleted = new SendOrPostCallback(this.OnGetVersionHotfixesOperationCompleted);
      this.InvokeAsync("GetVersionHotfixes", new object[2]
      {
        (object) majorVersion,
        (object) clientId
      }, this.GetVersionHotfixesOperationCompleted, userState);
    }

    private void OnGetVersionHotfixesOperationCompleted(object arg)
    {
      if (this.GetVersionHotfixesCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetVersionHotfixesCompleted((object) this, new GetVersionHotfixesCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/encompass/GetReleaseInfo", RequestNamespace = "http://www.elliemae.com/encompass", ResponseNamespace = "http://www.elliemae.com/encompass", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public ReleaseInfo GetReleaseInfo(string majorVersion, int sequenceNumber)
    {
      return (ReleaseInfo) this.Invoke(nameof (GetReleaseInfo), new object[2]
      {
        (object) majorVersion,
        (object) sequenceNumber
      })[0];
    }

    public IAsyncResult BeginGetReleaseInfo(
      string majorVersion,
      int sequenceNumber,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetReleaseInfo", new object[2]
      {
        (object) majorVersion,
        (object) sequenceNumber
      }, callback, asyncState);
    }

    public ReleaseInfo EndGetReleaseInfo(IAsyncResult asyncResult)
    {
      return (ReleaseInfo) this.EndInvoke(asyncResult)[0];
    }

    public void GetReleaseInfoAsync(string majorVersion, int sequenceNumber)
    {
      this.GetReleaseInfoAsync(majorVersion, sequenceNumber, (object) null);
    }

    public void GetReleaseInfoAsync(string majorVersion, int sequenceNumber, object userState)
    {
      if (this.GetReleaseInfoOperationCompleted == null)
        this.GetReleaseInfoOperationCompleted = new SendOrPostCallback(this.OnGetReleaseInfoOperationCompleted);
      this.InvokeAsync("GetReleaseInfo", new object[2]
      {
        (object) majorVersion,
        (object) sequenceNumber
      }, this.GetReleaseInfoOperationCompleted, userState);
    }

    private void OnGetReleaseInfoOperationCompleted(object arg)
    {
      if (this.GetReleaseInfoCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetReleaseInfoCompleted((object) this, new GetReleaseInfoCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
