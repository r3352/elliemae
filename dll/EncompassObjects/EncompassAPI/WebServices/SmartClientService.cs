// Decompiled with JetBrains decompiler
// Type: EllieMae.EncompassAPI.WebServices.SmartClientService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EncompassAPI.WebServices
{
  [GeneratedCode("System.Web.Services", "4.8.3752.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "SmartClientServiceSoap", Namespace = "http://hosted.elliemae.com/")]
  [XmlInclude(typeof (NameValuePair[]))]
  public class SmartClientService : SoapHttpClientProtocol
  {
    private SendOrPostCallback GetSCPackageInfoOperationCompleted;
    private SendOrPostCallback GetClientInfoOperationCompleted;
    private SendOrPostCallback LetEMDoUupdatesOperationCompleted;
    private SendOrPostCallback DoUpdatesByClientOperationCompleted;
    private SendOrPostCallback InstallDotNetOperationCompleted;
    private SendOrPostCallback UpdateSelfHostedClientInfoOperationCompleted;
    private SendOrPostCallback UpdateClientInfoOperationCompleted;
    private SendOrPostCallback UpdateClientInfo2OperationCompleted;
    private SendOrPostCallback InstallSmartClientOperationCompleted;
    private SendOrPostCallback GetAttributesOperationCompleted;
    private SendOrPostCallback GetAttributeOperationCompleted;
    private ServiceHeader serviceHeaderValueField;
    private bool useDefaultCredentialsSetExplicitly;

    public SmartClientService(string smartClientServiceUrl)
    {
      this.Url = string.IsNullOrWhiteSpace(smartClientServiceUrl) || !Uri.IsWellFormedUriString(smartClientServiceUrl, UriKind.Absolute) ? "https://hosted.elliemae.com/EncompassSCWS/SmartClientService.asmx" : smartClientServiceUrl;
      if (this.IsLocalFileSystemWebService(this.Url))
      {
        this.UseDefaultCredentials = true;
        this.useDefaultCredentialsSetExplicitly = false;
      }
      else
        this.useDefaultCredentialsSetExplicitly = true;
    }

    public SmartClientService()
    {
      this.Url = "https://hosted.elliemae.com/EncompassSCWS/SmartClientService.asmx";
      if (this.IsLocalFileSystemWebService(this.Url))
      {
        this.UseDefaultCredentials = true;
        this.useDefaultCredentialsSetExplicitly = false;
      }
      else
        this.useDefaultCredentialsSetExplicitly = true;
    }

    public ServiceHeader ServiceHeaderValue
    {
      get => this.serviceHeaderValueField;
      set => this.serviceHeaderValueField = value;
    }

    public new string Url
    {
      get => base.Url;
      set
      {
        if (this.IsLocalFileSystemWebService(base.Url) && !this.useDefaultCredentialsSetExplicitly && !this.IsLocalFileSystemWebService(value))
          base.UseDefaultCredentials = false;
        base.Url = value;
      }
    }

    public new bool UseDefaultCredentials
    {
      get => base.UseDefaultCredentials;
      set
      {
        base.UseDefaultCredentials = value;
        this.useDefaultCredentialsSetExplicitly = true;
      }
    }

    public event GetSCPackageInfoCompletedEventHandler GetSCPackageInfoCompleted;

    public event GetClientInfoCompletedEventHandler GetClientInfoCompleted;

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public event LetEMDoUupdatesCompletedEventHandler LetEMDoUupdatesCompleted;

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public event DoUpdatesByClientCompletedEventHandler DoUpdatesByClientCompleted;

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public event InstallDotNetCompletedEventHandler InstallDotNetCompleted;

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public event UpdateSelfHostedClientInfoCompletedEventHandler UpdateSelfHostedClientInfoCompleted;

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public event UpdateClientInfoCompletedEventHandler UpdateClientInfoCompleted;

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public event UpdateClientInfo2CompletedEventHandler UpdateClientInfo2Completed;

    public event InstallSmartClientCompletedEventHandler InstallSmartClientCompleted;

    public event GetAttributesCompletedEventHandler GetAttributesCompleted;

    public event GetAttributeCompletedEventHandler GetAttributeCompleted;

    [SoapDocumentMethod("http://hosted.elliemae.com/GetSCPackageInfo", RequestNamespace = "http://hosted.elliemae.com/", ResponseNamespace = "http://hosted.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public SCPackageInfo[] GetSCPackageInfo(string scCID, string encMajorVersion)
    {
      return (SCPackageInfo[]) this.Invoke(nameof (GetSCPackageInfo), new object[2]
      {
        (object) scCID,
        (object) encMajorVersion
      })[0];
    }

    public void GetSCPackageInfoAsync(string scCID, string encMajorVersion)
    {
      this.GetSCPackageInfoAsync(scCID, encMajorVersion, (object) null);
    }

    public void GetSCPackageInfoAsync(string scCID, string encMajorVersion, object userState)
    {
      if (this.GetSCPackageInfoOperationCompleted == null)
        this.GetSCPackageInfoOperationCompleted = new SendOrPostCallback(this.OnGetSCPackageInfoOperationCompleted);
      this.InvokeAsync("GetSCPackageInfo", new object[2]
      {
        (object) scCID,
        (object) encMajorVersion
      }, this.GetSCPackageInfoOperationCompleted, userState);
    }

    private void OnGetSCPackageInfoOperationCompleted(object arg)
    {
      if (this.GetSCPackageInfoCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetSCPackageInfoCompleted((object) this, new GetSCPackageInfoCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://hosted.elliemae.com/GetClientInfo", RequestNamespace = "http://hosted.elliemae.com/", ResponseNamespace = "http://hosted.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public ReturnResult GetClientInfo(string scCID, string password)
    {
      return (ReturnResult) this.Invoke(nameof (GetClientInfo), new object[2]
      {
        (object) scCID,
        (object) password
      })[0];
    }

    public void GetClientInfoAsync(string scCID, string password)
    {
      this.GetClientInfoAsync(scCID, password, (object) null);
    }

    public void GetClientInfoAsync(string scCID, string password, object userState)
    {
      if (this.GetClientInfoOperationCompleted == null)
        this.GetClientInfoOperationCompleted = new SendOrPostCallback(this.OnGetClientInfoOperationCompleted);
      this.InvokeAsync("GetClientInfo", new object[2]
      {
        (object) scCID,
        (object) password
      }, this.GetClientInfoOperationCompleted, userState);
    }

    private void OnGetClientInfoOperationCompleted(object arg)
    {
      if (this.GetClientInfoCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetClientInfoCompleted((object) this, new GetClientInfoCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://hosted.elliemae.com/LetEMDoUupdates", RequestNamespace = "http://hosted.elliemae.com/", ResponseNamespace = "http://hosted.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [Obsolete("Method will be deprecated in an upcoming release.")]
    public ReturnResult LetEMDoUupdates(string scCID, string password, string encMajorVersion)
    {
      return (ReturnResult) this.Invoke(nameof (LetEMDoUupdates), new object[3]
      {
        (object) scCID,
        (object) password,
        (object) encMajorVersion
      })[0];
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void LetEMDoUupdatesAsync(string scCID, string password, string encMajorVersion)
    {
      this.LetEMDoUupdatesAsync(scCID, password, encMajorVersion, (object) null);
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void LetEMDoUupdatesAsync(
      string scCID,
      string password,
      string encMajorVersion,
      object userState)
    {
      if (this.LetEMDoUupdatesOperationCompleted == null)
        this.LetEMDoUupdatesOperationCompleted = new SendOrPostCallback(this.OnLetEMDoUupdatesOperationCompleted);
      this.InvokeAsync("LetEMDoUupdates", new object[3]
      {
        (object) scCID,
        (object) password,
        (object) encMajorVersion
      }, this.LetEMDoUupdatesOperationCompleted, userState);
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    private void OnLetEMDoUupdatesOperationCompleted(object arg)
    {
      if (this.LetEMDoUupdatesCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.LetEMDoUupdatesCompleted((object) this, new LetEMDoUupdatesCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://hosted.elliemae.com/DoUpdatesByClient", RequestNamespace = "http://hosted.elliemae.com/", ResponseNamespace = "http://hosted.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [Obsolete("Method will be deprecated in an upcoming release.")]
    public ReturnResult DoUpdatesByClient(string scCID, string password, string installUrlID)
    {
      return (ReturnResult) this.Invoke(nameof (DoUpdatesByClient), new object[3]
      {
        (object) scCID,
        (object) password,
        (object) installUrlID
      })[0];
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void DoUpdatesByClientAsync(string scCID, string password, string installUrlID)
    {
      this.DoUpdatesByClientAsync(scCID, password, installUrlID, (object) null);
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void DoUpdatesByClientAsync(
      string scCID,
      string password,
      string installUrlID,
      object userState)
    {
      if (this.DoUpdatesByClientOperationCompleted == null)
        this.DoUpdatesByClientOperationCompleted = new SendOrPostCallback(this.OnDoUpdatesByClientOperationCompleted);
      this.InvokeAsync("DoUpdatesByClient", new object[3]
      {
        (object) scCID,
        (object) password,
        (object) installUrlID
      }, this.DoUpdatesByClientOperationCompleted, userState);
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    private void OnDoUpdatesByClientOperationCompleted(object arg)
    {
      if (this.DoUpdatesByClientCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.DoUpdatesByClientCompleted((object) this, new DoUpdatesByClientCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://hosted.elliemae.com/InstallDotNet", RequestNamespace = "http://hosted.elliemae.com/", ResponseNamespace = "http://hosted.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [Obsolete("Method will be deprecated in an upcoming release.")]
    public string InstallDotNet(
      string scCID,
      string password,
      string dotNetVersion,
      bool toInstall)
    {
      return (string) this.Invoke(nameof (InstallDotNet), new object[4]
      {
        (object) scCID,
        (object) password,
        (object) dotNetVersion,
        (object) toInstall
      })[0];
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void InstallDotNetAsync(
      string scCID,
      string password,
      string dotNetVersion,
      bool toInstall)
    {
      this.InstallDotNetAsync(scCID, password, dotNetVersion, toInstall, (object) null);
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void InstallDotNetAsync(
      string scCID,
      string password,
      string dotNetVersion,
      bool toInstall,
      object userState)
    {
      if (this.InstallDotNetOperationCompleted == null)
        this.InstallDotNetOperationCompleted = new SendOrPostCallback(this.OnInstallDotNetOperationCompleted);
      this.InvokeAsync("InstallDotNet", new object[4]
      {
        (object) scCID,
        (object) password,
        (object) dotNetVersion,
        (object) toInstall
      }, this.InstallDotNetOperationCompleted, userState);
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    private void OnInstallDotNetOperationCompleted(object arg)
    {
      if (this.InstallDotNetCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.InstallDotNetCompleted((object) this, new InstallDotNetCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://hosted.elliemae.com/UpdateSelfHostedClientInfo", RequestNamespace = "http://hosted.elliemae.com/", ResponseNamespace = "http://hosted.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [Obsolete("Method will be deprecated in an upcoming release.")]
    public ReturnResult UpdateSelfHostedClientInfo(
      string clientID,
      string password,
      string encSystemID,
      string sqlDbID,
      string version,
      char edition,
      string server,
      bool legacySettingAutoUpdate)
    {
      return (ReturnResult) this.Invoke(nameof (UpdateSelfHostedClientInfo), new object[8]
      {
        (object) clientID,
        (object) password,
        (object) encSystemID,
        (object) sqlDbID,
        (object) version,
        (object) edition,
        (object) server,
        (object) legacySettingAutoUpdate
      })[0];
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void UpdateSelfHostedClientInfoAsync(
      string clientID,
      string password,
      string encSystemID,
      string sqlDbID,
      string version,
      char edition,
      string server,
      bool legacySettingAutoUpdate)
    {
      this.UpdateSelfHostedClientInfoAsync(clientID, password, encSystemID, sqlDbID, version, edition, server, legacySettingAutoUpdate, (object) null);
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void UpdateSelfHostedClientInfoAsync(
      string clientID,
      string password,
      string encSystemID,
      string sqlDbID,
      string version,
      char edition,
      string server,
      bool legacySettingAutoUpdate,
      object userState)
    {
      if (this.UpdateSelfHostedClientInfoOperationCompleted == null)
        this.UpdateSelfHostedClientInfoOperationCompleted = new SendOrPostCallback(this.OnUpdateSelfHostedClientInfoOperationCompleted);
      this.InvokeAsync("UpdateSelfHostedClientInfo", new object[8]
      {
        (object) clientID,
        (object) password,
        (object) encSystemID,
        (object) sqlDbID,
        (object) version,
        (object) edition,
        (object) server,
        (object) legacySettingAutoUpdate
      }, this.UpdateSelfHostedClientInfoOperationCompleted, userState);
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    private void OnUpdateSelfHostedClientInfoOperationCompleted(object arg)
    {
      if (this.UpdateSelfHostedClientInfoCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.UpdateSelfHostedClientInfoCompleted((object) this, new UpdateSelfHostedClientInfoCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://hosted.elliemae.com/UpdateClientInfo", RequestNamespace = "http://hosted.elliemae.com/", ResponseNamespace = "http://hosted.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [Obsolete("Method will be deprecated in an upcoming release.")]
    public ReturnResult UpdateClientInfo(
      string clientID,
      string password,
      string encSystemID,
      string sqlDbID,
      string version,
      char edition,
      string server,
      bool legacySettingAutoUpdate,
      bool isSelfHosted,
      NameValuePair[] info)
    {
      return (ReturnResult) this.Invoke(nameof (UpdateClientInfo), new object[10]
      {
        (object) clientID,
        (object) password,
        (object) encSystemID,
        (object) sqlDbID,
        (object) version,
        (object) edition,
        (object) server,
        (object) legacySettingAutoUpdate,
        (object) isSelfHosted,
        (object) info
      })[0];
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void UpdateClientInfoAsync(
      string clientID,
      string password,
      string encSystemID,
      string sqlDbID,
      string version,
      char edition,
      string server,
      bool legacySettingAutoUpdate,
      bool isSelfHosted,
      NameValuePair[] info)
    {
      this.UpdateClientInfoAsync(clientID, password, encSystemID, sqlDbID, version, edition, server, legacySettingAutoUpdate, isSelfHosted, info, (object) null);
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void UpdateClientInfoAsync(
      string clientID,
      string password,
      string encSystemID,
      string sqlDbID,
      string version,
      char edition,
      string server,
      bool legacySettingAutoUpdate,
      bool isSelfHosted,
      NameValuePair[] info,
      object userState)
    {
      if (this.UpdateClientInfoOperationCompleted == null)
        this.UpdateClientInfoOperationCompleted = new SendOrPostCallback(this.OnUpdateClientInfoOperationCompleted);
      this.InvokeAsync("UpdateClientInfo", new object[10]
      {
        (object) clientID,
        (object) password,
        (object) encSystemID,
        (object) sqlDbID,
        (object) version,
        (object) edition,
        (object) server,
        (object) legacySettingAutoUpdate,
        (object) isSelfHosted,
        (object) info
      }, this.UpdateClientInfoOperationCompleted, userState);
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    private void OnUpdateClientInfoOperationCompleted(object arg)
    {
      if (this.UpdateClientInfoCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.UpdateClientInfoCompleted((object) this, new UpdateClientInfoCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://hosted.elliemae.com/UpdateClientInfo2", RequestNamespace = "http://hosted.elliemae.com/", ResponseNamespace = "http://hosted.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [Obsolete("Method will be deprecated in an upcoming release.")]
    public ReturnResult UpdateClientInfo2(
      string clientID,
      string password,
      string encSystemID,
      string sqlDbID,
      string version,
      char edition,
      string server,
      bool legacySettingAutoUpdate,
      bool isSelfHosted,
      NameValuePair[] info)
    {
      return (ReturnResult) this.Invoke(nameof (UpdateClientInfo2), new object[10]
      {
        (object) clientID,
        (object) password,
        (object) encSystemID,
        (object) sqlDbID,
        (object) version,
        (object) edition,
        (object) server,
        (object) legacySettingAutoUpdate,
        (object) isSelfHosted,
        (object) info
      })[0];
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void UpdateClientInfo2Async(
      string clientID,
      string password,
      string encSystemID,
      string sqlDbID,
      string version,
      char edition,
      string server,
      bool legacySettingAutoUpdate,
      bool isSelfHosted,
      NameValuePair[] info)
    {
      this.UpdateClientInfo2Async(clientID, password, encSystemID, sqlDbID, version, edition, server, legacySettingAutoUpdate, isSelfHosted, info, (object) null);
    }

    [Obsolete("Method will be deprecated in an upcoming release.")]
    public void UpdateClientInfo2Async(
      string clientID,
      string password,
      string encSystemID,
      string sqlDbID,
      string version,
      char edition,
      string server,
      bool legacySettingAutoUpdate,
      bool isSelfHosted,
      NameValuePair[] info,
      object userState)
    {
      if (this.UpdateClientInfo2OperationCompleted == null)
        this.UpdateClientInfo2OperationCompleted = new SendOrPostCallback(this.OnUpdateClientInfo2OperationCompleted);
      this.InvokeAsync("UpdateClientInfo2", new object[10]
      {
        (object) clientID,
        (object) password,
        (object) encSystemID,
        (object) sqlDbID,
        (object) version,
        (object) edition,
        (object) server,
        (object) legacySettingAutoUpdate,
        (object) isSelfHosted,
        (object) info
      }, this.UpdateClientInfo2OperationCompleted, userState);
    }

    private void OnUpdateClientInfo2OperationCompleted(object arg)
    {
      if (this.UpdateClientInfo2Completed == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.UpdateClientInfo2Completed((object) this, new UpdateClientInfo2CompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://hosted.elliemae.com/InstallSmartClient", RequestNamespace = "http://hosted.elliemae.com/", ResponseNamespace = "http://hosted.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool InstallSmartClient(string clientID)
    {
      return (bool) this.Invoke(nameof (InstallSmartClient), new object[1]
      {
        (object) clientID
      })[0];
    }

    public void InstallSmartClientAsync(string clientID)
    {
      this.InstallSmartClientAsync(clientID, (object) null);
    }

    public void InstallSmartClientAsync(string clientID, object userState)
    {
      if (this.InstallSmartClientOperationCompleted == null)
        this.InstallSmartClientOperationCompleted = new SendOrPostCallback(this.OnInstallSmartClientOperationCompleted);
      this.InvokeAsync("InstallSmartClient", new object[1]
      {
        (object) clientID
      }, this.InstallSmartClientOperationCompleted, userState);
    }

    private void OnInstallSmartClientOperationCompleted(object arg)
    {
      if (this.InstallSmartClientCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.InstallSmartClientCompleted((object) this, new InstallSmartClientCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://hosted.elliemae.com/GetAttributes", RequestNamespace = "http://hosted.elliemae.com/", ResponseNamespace = "http://hosted.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string[] GetAttributes(string clientID, string appSuiteName, string appName)
    {
      return (string[]) this.Invoke(nameof (GetAttributes), new object[3]
      {
        (object) clientID,
        (object) appSuiteName,
        (object) appName
      })[0];
    }

    public void GetAttributesAsync(string clientID, string appSuiteName, string appName)
    {
      this.GetAttributesAsync(clientID, appSuiteName, appName, (object) null);
    }

    public void GetAttributesAsync(
      string clientID,
      string appSuiteName,
      string appName,
      object userState)
    {
      if (this.GetAttributesOperationCompleted == null)
        this.GetAttributesOperationCompleted = new SendOrPostCallback(this.OnGetAttributesOperationCompleted);
      this.InvokeAsync("GetAttributes", new object[3]
      {
        (object) clientID,
        (object) appSuiteName,
        (object) appName
      }, this.GetAttributesOperationCompleted, userState);
    }

    private void OnGetAttributesOperationCompleted(object arg)
    {
      if (this.GetAttributesCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetAttributesCompleted((object) this, new GetAttributesCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://hosted.elliemae.com/GetAttribute", RequestNamespace = "http://hosted.elliemae.com/", ResponseNamespace = "http://hosted.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetAttribute(
      string clientID,
      string appSuiteName,
      string appName,
      string attrName)
    {
      return (string) this.Invoke(nameof (GetAttribute), new object[4]
      {
        (object) clientID,
        (object) appSuiteName,
        (object) appName,
        (object) attrName
      })[0];
    }

    public void GetAttributeAsync(
      string clientID,
      string appSuiteName,
      string appName,
      string attrName)
    {
      this.GetAttributeAsync(clientID, appSuiteName, appName, attrName, (object) null);
    }

    public void GetAttributeAsync(
      string clientID,
      string appSuiteName,
      string appName,
      string attrName,
      object userState)
    {
      if (this.GetAttributeOperationCompleted == null)
        this.GetAttributeOperationCompleted = new SendOrPostCallback(this.OnGetAttributeOperationCompleted);
      this.InvokeAsync("GetAttribute", new object[4]
      {
        (object) clientID,
        (object) appSuiteName,
        (object) appName,
        (object) attrName
      }, this.GetAttributeOperationCompleted, userState);
    }

    private void OnGetAttributeOperationCompleted(object arg)
    {
      if (this.GetAttributeCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetAttributeCompleted((object) this, new GetAttributeCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);

    private bool IsLocalFileSystemWebService(string url)
    {
      if (url == null || url == string.Empty)
        return false;
      Uri uri = new Uri(url);
      return uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0;
    }
  }
}
