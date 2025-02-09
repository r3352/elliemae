// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.ModuleService
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "ModuleServiceSoap", Namespace = "http://modules.elliemaeservices.com/jedservices/")]
  public class ModuleService : SoapHttpClientProtocol
  {
    public ModuleService() => this.Url = "https://encompass.elliemae.com/jedservices/modules.asmx";

    public ModuleService(string jedServicesUrl)
    {
      if (string.IsNullOrWhiteSpace(jedServicesUrl) || !Uri.IsWellFormedUriString(jedServicesUrl, UriKind.Absolute))
        this.Url = "https://encompass.elliemae.com/jedservices/modules.asmx";
      else
        this.Url = jedServicesUrl + "modules.asmx";
    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
      HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(uri);
      webRequest.KeepAlive = false;
      return (WebRequest) webRequest;
    }

    [SoapDocumentMethod("http://modules.elliemaeservices.com/jedservices/GetModuleLicense", RequestNamespace = "http://modules.elliemaeservices.com/jedservices/", ResponseNamespace = "http://modules.elliemaeservices.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public ModuleLicense GetModuleLicense(string clientID, string moduleID)
    {
      return (ModuleLicense) this.Invoke(nameof (GetModuleLicense), new object[2]
      {
        (object) clientID,
        (object) moduleID
      })[0];
    }

    public IAsyncResult BeginGetModuleLicense(
      string clientID,
      string moduleID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetModuleLicense", new object[2]
      {
        (object) clientID,
        (object) moduleID
      }, callback, asyncState);
    }

    public ModuleLicense EndGetModuleLicense(IAsyncResult asyncResult)
    {
      return (ModuleLicense) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://modules.elliemaeservices.com/jedservices/GetModuleLicense2", RequestNamespace = "http://modules.elliemaeservices.com/jedservices/", ResponseNamespace = "http://modules.elliemaeservices.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public ModuleLicense GetModuleLicense2(string clientID, string moduleID)
    {
      return (ModuleLicense) this.Invoke(nameof (GetModuleLicense2), new object[2]
      {
        (object) clientID,
        (object) moduleID
      })[0];
    }

    public IAsyncResult BeginGetModuleLicense2(
      string clientID,
      string moduleID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetModuleLicense2", new object[2]
      {
        (object) clientID,
        (object) moduleID
      }, callback, asyncState);
    }

    public ModuleLicense EndGetModuleLicense2(IAsyncResult asyncResult)
    {
      return (ModuleLicense) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://modules.elliemaeservices.com/jedservices/GetClientModules", RequestNamespace = "http://modules.elliemaeservices.com/jedservices/", ResponseNamespace = "http://modules.elliemaeservices.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public ModuleInfo[] GetClientModules(string clientID)
    {
      return (ModuleInfo[]) this.Invoke(nameof (GetClientModules), new object[1]
      {
        (object) clientID
      })[0];
    }

    public IAsyncResult BeginGetClientModules(
      string clientID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetClientModules", new object[1]
      {
        (object) clientID
      }, callback, asyncState);
    }

    public ModuleInfo[] EndGetClientModules(IAsyncResult asyncResult)
    {
      return (ModuleInfo[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://modules.elliemaeservices.com/jedservices/GetUserModules", RequestNamespace = "http://modules.elliemaeservices.com/jedservices/", ResponseNamespace = "http://modules.elliemaeservices.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public ModuleInfo[] GetUserModules(string clientID, string userID)
    {
      return (ModuleInfo[]) this.Invoke(nameof (GetUserModules), new object[2]
      {
        (object) clientID,
        (object) userID
      })[0];
    }

    public IAsyncResult BeginGetUserModules(
      string clientID,
      string userID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetUserModules", new object[2]
      {
        (object) clientID,
        (object) userID
      }, callback, asyncState);
    }

    public ModuleInfo[] EndGetUserModules(IAsyncResult asyncResult)
    {
      return (ModuleInfo[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://modules.elliemaeservices.com/jedservices/EnableModuleUser", RequestNamespace = "http://modules.elliemaeservices.com/jedservices/", ResponseNamespace = "http://modules.elliemaeservices.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public ModuleInfo[] EnableModuleUser(string clientID, string moduleID, string userID)
    {
      return (ModuleInfo[]) this.Invoke(nameof (EnableModuleUser), new object[3]
      {
        (object) clientID,
        (object) moduleID,
        (object) userID
      })[0];
    }

    public IAsyncResult BeginEnableModuleUser(
      string clientID,
      string moduleID,
      string userID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("EnableModuleUser", new object[3]
      {
        (object) clientID,
        (object) moduleID,
        (object) userID
      }, callback, asyncState);
    }

    public ModuleInfo[] EndEnableModuleUser(IAsyncResult asyncResult)
    {
      return (ModuleInfo[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://modules.elliemaeservices.com/jedservices/DisableModuleUser", RequestNamespace = "http://modules.elliemaeservices.com/jedservices/", ResponseNamespace = "http://modules.elliemaeservices.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public ModuleInfo[] DisableModuleUser(string clientID, string moduleID, string userID)
    {
      return (ModuleInfo[]) this.Invoke(nameof (DisableModuleUser), new object[3]
      {
        (object) clientID,
        (object) moduleID,
        (object) userID
      })[0];
    }

    public IAsyncResult BeginDisableModuleUser(
      string clientID,
      string moduleID,
      string userID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("DisableModuleUser", new object[3]
      {
        (object) clientID,
        (object) moduleID,
        (object) userID
      }, callback, asyncState);
    }

    public ModuleInfo[] EndDisableModuleUser(IAsyncResult asyncResult)
    {
      return (ModuleInfo[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://modules.elliemaeservices.com/jedservices/UpdateModuleUsers", RequestNamespace = "http://modules.elliemaeservices.com/jedservices/", ResponseNamespace = "http://modules.elliemaeservices.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void UpdateModuleUsers(string clientID, string moduleID, ModuleUser[] userList)
    {
      this.Invoke(nameof (UpdateModuleUsers), new object[3]
      {
        (object) clientID,
        (object) moduleID,
        (object) userList
      });
    }

    public IAsyncResult BeginUpdateModuleUsers(
      string clientID,
      string moduleID,
      ModuleUser[] userList,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UpdateModuleUsers", new object[3]
      {
        (object) clientID,
        (object) moduleID,
        (object) userList
      }, callback, asyncState);
    }

    public void EndUpdateModuleUsers(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    [SoapDocumentMethod("http://modules.elliemaeservices.com/jedservices/HasPersonalLicense", RequestNamespace = "http://modules.elliemaeservices.com/jedservices/", ResponseNamespace = "http://modules.elliemaeservices.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool HasPersonalLicense(string clientID, string moduleID, string userID)
    {
      return (bool) this.Invoke(nameof (HasPersonalLicense), new object[3]
      {
        (object) clientID,
        (object) moduleID,
        (object) userID
      })[0];
    }

    public IAsyncResult BeginHasPersonalLicense(
      string clientID,
      string moduleID,
      string userID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("HasPersonalLicense", new object[3]
      {
        (object) clientID,
        (object) moduleID,
        (object) userID
      }, callback, asyncState);
    }

    public bool EndHasPersonalLicense(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }
  }
}
