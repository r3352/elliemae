// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.HomePageService
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using Microsoft.Web.Services2;
using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public static class HomePageService
  {
    private const string className = "HomePageSettings�";
    private static readonly string sw = Tracing.SwEpass;
    private static bool isMigrated;
    private static bool isLocked;
    private static bool saveMigration;

    public static bool SaveMigration => HomePageService.saveMigration;

    public static Hashtable GetModuleSettings(
      string clientID,
      string dataServicesUrl,
      string personaName,
      int personaID,
      out int maxAllowed,
      string authId)
    {
      maxAllowed = 0;
      HomePageService.isMigrated = false;
      try
      {
        DataService dataService = new DataService(dataServicesUrl);
        string moduleListRequest = HomePageService.createGetPersonaModuleListRequest(clientID, personaName, personaID.ToString(), authId);
        if (moduleListRequest == null)
          return (Hashtable) null;
        dataService.Proxy = WebRequest.DefaultWebProxy;
        dataService.Proxy.Credentials = CredentialCache.DefaultCredentials;
        string xml = dataService.QueryService(moduleListRequest);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
        XmlElement xmlElement1 = (XmlElement) xmlDocument.SelectSingleNode("//ERROR");
        if (xmlElement1 != null)
          throw new Exception(xmlElement1.InnerText);
        HomePageService.isLocked = false;
        XmlElement xmlElement2 = (XmlElement) xmlDocument.SelectSingleNode("EMHOMEPAGEWS/PERSONA");
        if (xmlElement2 != null)
        {
          string attribute1 = xmlElement2.GetAttribute("IsLocked");
          if (!string.IsNullOrEmpty(attribute1))
            HomePageService.isLocked = attribute1 == "1";
          string attribute2 = xmlElement2.GetAttribute("IsMigrated");
          if (!string.IsNullOrEmpty(attribute2))
            HomePageService.isMigrated = attribute2 == "1";
          string attribute3 = xmlElement2.GetAttribute("MaxAllowed");
          if (!string.IsNullOrEmpty(attribute3) && HomePageService.isInteger(attribute3))
            maxAllowed = Convert.ToInt32(attribute3);
        }
        XmlNodeList xmlNodeList = xmlDocument.SelectNodes("EMHOMEPAGEWS/MODULES/MODULE");
        Hashtable moduleSettings = new Hashtable();
        foreach (XmlElement elm in xmlNodeList)
        {
          HomePageModuleSettings pageModuleSettings = new HomePageModuleSettings();
          if (!HomePageService.isMigrated)
          {
            if (!pageModuleSettings.MigrateSettings(elm, HomePageService.isLocked))
              throw new Exception(pageModuleSettings.Error);
          }
          else if (!pageModuleSettings.Load(elm))
            throw new Exception(pageModuleSettings.Error);
          moduleSettings.Add((object) pageModuleSettings.ModuleID, (object) pageModuleSettings);
        }
        if (!HomePageService.isMigrated)
        {
          if (HomePageService.isLocked)
            HomePageService.saveMigration = true;
          HomePageService.isMigrated = true;
        }
        return moduleSettings;
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePageService.sw, TraceLevel.Error, "HomePageSettings", "GetModuleSettings: " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to load the module settings:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (Hashtable) null;
      }
    }

    private static string createGetPersonaModuleListRequest(
      string clientID,
      string personaName,
      string personaID,
      string authId)
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml("<EMHOMEPAGEDATAWS />");
        XmlElement element1 = xmlDocument.CreateElement("REQUEST");
        element1.SetAttribute("DateTime", DateTime.Now.ToString("g"));
        element1.SetAttribute("Type", "Query");
        element1.SetAttribute("Action", "GetPersonaModuleList");
        XmlElement element2 = xmlDocument.CreateElement("PARAMETER");
        element2.SetAttribute("ClientID", clientID);
        element2.SetAttribute("UserUID", authId);
        element2.SetAttribute("Persona", personaName);
        element2.SetAttribute("PersonaID", personaID);
        element1.AppendChild((XmlNode) element2);
        xmlDocument.DocumentElement.AppendChild((XmlNode) element1);
        return xmlDocument.OuterXml;
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePageService.sw, TraceLevel.Error, "HomePageSettings", "createGetPersonaModuleListRequest: " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to create the module settings request:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string) null;
      }
    }

    public static void SaveModuleSettings(
      string clientID,
      string dataServicesUrl,
      string persona,
      int personaId,
      Hashtable htModuleList,
      string authId)
    {
      try
      {
        HomePageService.saveMigration = false;
        if (htModuleList == null || htModuleList.Count == 0)
          return;
        XmlDocument xml1 = new XmlDocument();
        xml1.LoadXml("<EMHOMEPAGEWS />");
        XmlElement element = xml1.CreateElement("MODULES");
        element.SetAttribute("IsLocked", HomePageService.isLocked ? "Y" : "N");
        if (HomePageService.isMigrated)
          element.SetAttribute("IsMigrated", "1");
        foreach (string key in (IEnumerable) htModuleList.Keys)
        {
          HomePageModuleSettings htModule = (HomePageModuleSettings) htModuleList[(object) key];
          if (!htModule.Save(xml1, element))
            throw new Exception("ModuleSettings exception: " + htModule.Error);
        }
        xml1.DocumentElement.AppendChild((XmlNode) element);
        DataService dataService = new DataService(dataServicesUrl);
        dataService.Proxy = WebRequest.DefaultWebProxy;
        dataService.Proxy.Credentials = CredentialCache.DefaultCredentials;
        string xml2 = dataService.SetDefaultModulesSettings(clientID, authId, persona, personaId.ToString(), xml1.OuterXml);
        xml1.LoadXml(xml2);
        XmlElement xmlElement = (XmlElement) xml1.SelectSingleNode("//ERROR");
        if (xmlElement != null)
          throw new Exception(xmlElement.InnerText);
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePageService.sw, TraceLevel.Error, "HomePageSettings", "SaveData: " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when saving module settings:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private static bool isInteger(string val)
    {
      try
      {
        int.Parse(val);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static string getSSO(
      string clientID,
      string loginServicesUrl,
      string userId,
      string version)
    {
      LoginServiceWse loginServiceWse = new LoginServiceWse(loginServicesUrl, (string) null);
      string loginRequest = HomePageService.createLoginRequest(clientID, userId, version);
      loginServiceWse.Proxy = WebRequest.DefaultWebProxy;
      loginServiceWse.Proxy.Credentials = CredentialCache.DefaultCredentials;
      loginServiceWse.Timeout = 30000;
      return HomePageService.parseLoginResponse(loginServiceWse.AutherticateUser(loginRequest), loginServiceWse.ResponseSoapContext);
    }

    public static string getModuleData(string authId, string dataServicesUrl, string moduleID)
    {
      try
      {
        DataService dataService = new DataService(dataServicesUrl);
        dataService.Proxy = WebRequest.DefaultWebProxy;
        dataService.Proxy.Credentials = CredentialCache.DefaultCredentials;
        return dataService.GetModuleData(authId, moduleID);
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePageService.sw, TraceLevel.Error, "HomePageSettings", "getModuleData: " + ex.Message);
        return "";
      }
    }

    public static void saveUserModulePreference(
      string authId,
      string dataServicesUrl,
      string setting)
    {
      try
      {
        DataService dataService = new DataService(dataServicesUrl);
        dataService.Proxy = WebRequest.DefaultWebProxy;
        dataService.Proxy.Credentials = CredentialCache.DefaultCredentials;
        dataService.SaveUserModulePreference(authId, "200", "ShowWelcome", setting);
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePageService.sw, TraceLevel.Error, "HomePageSettings", "saveUserModulePreference: " + ex.Message);
      }
    }

    public static string getWelcomePage(
      string clientID,
      string dataServicesUrl,
      string userId,
      string version,
      string authId)
    {
      try
      {
        string serviceXML = HomePageService.Create(authId, clientID, userId, "200", version);
        if (serviceXML == null || serviceXML.Length == 0)
          return (string) null;
        DataService dataService = new DataService(dataServicesUrl);
        dataService.Proxy = WebRequest.DefaultWebProxy;
        dataService.Proxy.Credentials = CredentialCache.DefaultCredentials;
        return dataService.QueryService(serviceXML).Replace("images/", "https://encompass.elliemae.com/homepage/images/");
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePageService.sw, TraceLevel.Error, "HomePageSettings", "getWelcomePage: " + ex.Message);
        return "";
      }
    }

    public static string getModuleList(
      string clientID,
      string dataServicesUrl,
      string userId,
      string version,
      string authId)
    {
      try
      {
        string serviceXML = HomePageService.Create(authId, clientID, userId, (string) null, version);
        if (serviceXML == null || serviceXML.Length == 0)
          return (string) null;
        DataService dataService = new DataService(dataServicesUrl);
        dataService.Proxy = WebRequest.DefaultWebProxy;
        dataService.Proxy.Credentials = CredentialCache.DefaultCredentials;
        return dataService.QueryService(serviceXML);
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePageService.sw, TraceLevel.Error, "HomePageSettings", "getModuleList: " + ex.Message);
        return "";
      }
    }

    private static string createLoginRequest(string clientID, string userId, string version)
    {
      try
      {
        Tracing.Log(HomePageService.sw, TraceLevel.Verbose, "HomePageSettings", nameof (createLoginRequest));
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml("<EMHOMEPAGELOGINWS />");
        XmlElement element1 = xmlDocument.CreateElement("REQUEST");
        DateTime now = DateTime.Now;
        element1.SetAttribute("DateTime", now.ToString());
        TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
        element1.SetAttribute("TimeZone", currentTimeZone.IsDaylightSavingTime(now) ? currentTimeZone.DaylightName : currentTimeZone.StandardName);
        element1.SetAttribute("Type", "Login");
        xmlDocument.DocumentElement.AppendChild((XmlNode) element1);
        XmlElement element2 = xmlDocument.CreateElement("LOGIN");
        element2.SetAttribute("ClientID", clientID);
        element2.SetAttribute("UserID", userId);
        element2.SetAttribute("UpdateUserInfo", "false");
        xmlDocument.DocumentElement.AppendChild((XmlNode) element2);
        XmlElement element3 = xmlDocument.CreateElement("LOS");
        element3.SetAttribute("Edition", version);
        xmlDocument.DocumentElement.AppendChild((XmlNode) element3);
        return xmlDocument.DocumentElement.OuterXml;
      }
      catch (Exception ex)
      {
        string str = nameof (createLoginRequest);
        Tracing.Log(HomePageService.sw, TraceLevel.Error, "HomePageSettings", str + ": " + ex.Message);
        throw new Exception("(" + str + ")" + ex.Message);
      }
    }

    private static string parseLoginResponse(string response, SoapContext context)
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(response);
        XmlElement xmlElement = (XmlElement) xmlDocument.SelectSingleNode("EMHOMEPAGEWS/ERROR");
        if (xmlElement != null)
          throw new Exception("LoginService Error: " + xmlElement.InnerText);
        return ((XmlElement) xmlDocument.SelectSingleNode("EMHOMEPAGEWS/USERINFO") ?? throw new Exception("Missing USERINFO element")).GetAttribute("UserUID");
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePageService.sw, TraceLevel.Error, "HomePageSettings", "parseLoginResponse: " + ex.Message);
        return "";
      }
    }

    public static string Create(
      string authId,
      string clientId,
      string userId,
      string moduleId,
      string version)
    {
      string str = moduleId == null ? "GetModuleList" : "GetModuleData";
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml("<EMHOMEPAGEDATAWS />");
        XmlElement element1 = xmlDocument.CreateElement("REQUEST");
        element1.SetAttribute("DateTime", DateTime.Now.ToString());
        element1.SetAttribute("Type", "Query");
        element1.SetAttribute("Action", str);
        XmlElement element2 = xmlDocument.CreateElement("PARAMETER");
        element2.SetAttribute("UserUID", "[AUTH_ID]");
        element2.SetAttribute("ClientID", "[CLIENT_ID]");
        element2.SetAttribute("UserID", "[USER_ID]");
        element2.SetAttribute("Version", version);
        element2.SetAttribute("IsAdministrator", "1");
        element2.SetAttribute("IsAccountAdmin", "1");
        element2.SetAttribute("IsEnableTPOLink", "1");
        if (moduleId != null)
          element2.SetAttribute("ModuleID", moduleId);
        element1.AppendChild((XmlNode) element2);
        xmlDocument.DocumentElement.AppendChild((XmlNode) element1);
        return xmlDocument.OuterXml.Replace("[AUTH_ID]", authId).Replace("[CLIENT_ID]", clientId).Replace("[USER_ID]", userId);
      }
      catch (Exception ex)
      {
        Tracing.Log(HomePageService.sw, TraceLevel.Error, "HomePageSettings", "ModuleRequest Create: " + ex.Message);
        return (string) null;
      }
    }
  }
}
