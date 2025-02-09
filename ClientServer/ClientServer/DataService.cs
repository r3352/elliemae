// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DataService
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "DataServiceSoap", Namespace = "http://encompass.elliemae.com/")]
  public class DataService : SoapHttpClientProtocol
  {
    public DataService() => this.Url = "https://encompass.elliemae.com/homepageWS/DataService.asmx";

    public DataService(string dataServicesUrl)
    {
      if (string.IsNullOrWhiteSpace(dataServicesUrl) || !Uri.IsWellFormedUriString(dataServicesUrl, UriKind.Absolute))
        this.Url = "https://encompass.elliemae.com/homepageWS/DataService.asmx";
      else
        this.Url = dataServicesUrl;
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/CheckServer", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string CheckServer() => (string) this.Invoke(nameof (CheckServer), new object[0])[0];

    public IAsyncResult BeginCheckServer(AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("CheckServer", new object[0], callback, asyncState);
    }

    public string EndCheckServer(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/QueryService", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string QueryService(string serviceXML)
    {
      return (string) this.Invoke(nameof (QueryService), new object[1]
      {
        (object) serviceXML
      })[0];
    }

    public IAsyncResult BeginQueryService(
      string serviceXML,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("QueryService", new object[1]
      {
        (object) serviceXML
      }, callback, asyncState);
    }

    public string EndQueryService(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/GetModuleList", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetModuleList(string UserUID)
    {
      return (string) this.Invoke(nameof (GetModuleList), new object[1]
      {
        (object) UserUID
      })[0];
    }

    public IAsyncResult BeginGetModuleList(
      string UserUID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetModuleList", new object[1]
      {
        (object) UserUID
      }, callback, asyncState);
    }

    public string EndGetModuleList(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/GetModuleData", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetModuleData(string UserUID, string ModuleID)
    {
      return (string) this.Invoke(nameof (GetModuleData), new object[2]
      {
        (object) UserUID,
        (object) ModuleID
      })[0];
    }

    public IAsyncResult BeginGetModuleData(
      string UserUID,
      string ModuleID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetModuleData", new object[2]
      {
        (object) UserUID,
        (object) ModuleID
      }, callback, asyncState);
    }

    public string EndGetModuleData(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/GetClientModulePreference", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetClientModulePreference(string UserUID, string ModuleID)
    {
      return (string) this.Invoke(nameof (GetClientModulePreference), new object[2]
      {
        (object) UserUID,
        (object) ModuleID
      })[0];
    }

    public IAsyncResult BeginGetClientModulePreference(
      string UserUID,
      string ModuleID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetClientModulePreference", new object[2]
      {
        (object) UserUID,
        (object) ModuleID
      }, callback, asyncState);
    }

    public string EndGetClientModulePreference(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/GetCustomLinks", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetCustomLinks(string UserUID, string ModuleID)
    {
      return (string) this.Invoke(nameof (GetCustomLinks), new object[2]
      {
        (object) UserUID,
        (object) ModuleID
      })[0];
    }

    public IAsyncResult BeginGetCustomLinks(
      string UserUID,
      string ModuleID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetCustomLinks", new object[2]
      {
        (object) UserUID,
        (object) ModuleID
      }, callback, asyncState);
    }

    public string EndGetCustomLinks(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/GetLoggedInUsers", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetLoggedInUsers(string UserUID, string ModuleID)
    {
      return (string) this.Invoke(nameof (GetLoggedInUsers), new object[2]
      {
        (object) UserUID,
        (object) ModuleID
      })[0];
    }

    public IAsyncResult BeginGetLoggedInUsers(
      string UserUID,
      string ModuleID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetLoggedInUsers", new object[2]
      {
        (object) UserUID,
        (object) ModuleID
      }, callback, asyncState);
    }

    public string EndGetLoggedInUsers(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/GetEllieMaeMessage", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetEllieMaeMessage(string ClientID, string UserUID, string ModuleID)
    {
      return (string) this.Invoke(nameof (GetEllieMaeMessage), new object[3]
      {
        (object) ClientID,
        (object) UserUID,
        (object) ModuleID
      })[0];
    }

    public IAsyncResult BeginGetEllieMaeMessage(
      string ClientID,
      string UserUID,
      string ModuleID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetEllieMaeMessage", new object[3]
      {
        (object) ClientID,
        (object) UserUID,
        (object) ModuleID
      }, callback, asyncState);
    }

    public string EndGetEllieMaeMessage(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/GetDefaultModulesSettings", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetDefaultModulesSettings(
      string ClientID,
      string UserUID,
      string Persona,
      string PersonaID)
    {
      return (string) this.Invoke(nameof (GetDefaultModulesSettings), new object[4]
      {
        (object) ClientID,
        (object) UserUID,
        (object) Persona,
        (object) PersonaID
      })[0];
    }

    public IAsyncResult BeginGetDefaultModulesSettings(
      string ClientID,
      string UserUID,
      string Persona,
      string PersonaID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetDefaultModulesSettings", new object[4]
      {
        (object) ClientID,
        (object) UserUID,
        (object) Persona,
        (object) PersonaID
      }, callback, asyncState);
    }

    public string EndGetDefaultModulesSettings(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/SaveClientModulePreference", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string SaveClientModulePreference(string UserUID, string ModuleID, string Data)
    {
      return (string) this.Invoke(nameof (SaveClientModulePreference), new object[3]
      {
        (object) UserUID,
        (object) ModuleID,
        (object) Data
      })[0];
    }

    public IAsyncResult BeginSaveClientModulePreference(
      string UserUID,
      string ModuleID,
      string Data,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SaveClientModulePreference", new object[3]
      {
        (object) UserUID,
        (object) ModuleID,
        (object) Data
      }, callback, asyncState);
    }

    public string EndSaveClientModulePreference(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/SaveUserModulePreference", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string SaveUserModulePreference(
      string UserUID,
      string ModuleID,
      string Preference,
      string Value)
    {
      return (string) this.Invoke(nameof (SaveUserModulePreference), new object[4]
      {
        (object) UserUID,
        (object) ModuleID,
        (object) Preference,
        (object) Value
      })[0];
    }

    public IAsyncResult BeginSaveUserModulePreference(
      string UserUID,
      string ModuleID,
      string Preference,
      string Value,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SaveUserModulePreference", new object[4]
      {
        (object) UserUID,
        (object) ModuleID,
        (object) Preference,
        (object) Value
      }, callback, asyncState);
    }

    public string EndSaveUserModulePreference(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/SetCustomLink", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string SetCustomLink(
      string UserUID,
      string ModuleID,
      string Action,
      int ActionID,
      string Name,
      string Value)
    {
      return (string) this.Invoke(nameof (SetCustomLink), new object[6]
      {
        (object) UserUID,
        (object) ModuleID,
        (object) Action,
        (object) ActionID,
        (object) Name,
        (object) Value
      })[0];
    }

    public IAsyncResult BeginSetCustomLink(
      string UserUID,
      string ModuleID,
      string Action,
      int ActionID,
      string Name,
      string Value,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SetCustomLink", new object[6]
      {
        (object) UserUID,
        (object) ModuleID,
        (object) Action,
        (object) ActionID,
        (object) Name,
        (object) Value
      }, callback, asyncState);
    }

    public string EndSetCustomLink(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/SetDefaultModulesSettings", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string SetDefaultModulesSettings(
      string ClientID,
      string UserUID,
      string Persona,
      string PersonaID,
      string Settings)
    {
      return (string) this.Invoke(nameof (SetDefaultModulesSettings), new object[5]
      {
        (object) ClientID,
        (object) UserUID,
        (object) Persona,
        (object) PersonaID,
        (object) Settings
      })[0];
    }

    public IAsyncResult BeginSetDefaultModulesSettings(
      string ClientID,
      string UserUID,
      string Persona,
      string PersonaID,
      string Settings,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SetDefaultModulesSettings", new object[5]
      {
        (object) ClientID,
        (object) UserUID,
        (object) Persona,
        (object) PersonaID,
        (object) Settings
      }, callback, asyncState);
    }

    public string EndSetDefaultModulesSettings(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/GetUserModulesAccessByPersona", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetUserModulesAccessByPersona(string ClientID, string UserUID, string Persona)
    {
      return (string) this.Invoke(nameof (GetUserModulesAccessByPersona), new object[3]
      {
        (object) ClientID,
        (object) UserUID,
        (object) Persona
      })[0];
    }

    public IAsyncResult BeginGetUserModulesAccessByPersona(
      string ClientID,
      string UserUID,
      string Persona,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetUserModulesAccessByPersona", new object[3]
      {
        (object) ClientID,
        (object) UserUID,
        (object) Persona
      }, callback, asyncState);
    }

    public string EndGetUserModulesAccessByPersona(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }
  }
}
