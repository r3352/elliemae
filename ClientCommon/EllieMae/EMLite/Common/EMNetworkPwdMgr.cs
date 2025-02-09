// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.EMNetworkPwdMgr
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class EMNetworkPwdMgr
  {
    private const string className = "EMNetworkPwdMgr";
    private static readonly string sw = Tracing.SwEpass;
    private const string CHANGEUSERURL = "https://www.epassbusinesscenter.com/epassutils/jedchangeuserpwd.asp";
    private const string CHECKUSERURL = "https://www.epassbusinesscenter.com/epassutils/jedcheckuserpwd.asp";
    private const string EMAILUSERURL = "https://www.epassbusinesscenter.com/epassutils/jedemailuserpwd.asp";
    private const string MANAGEUSERURL = "https://www.epassbusinesscenter.com/epassutils/jedchangeuserpwd.asp";
    private const string SECONDARYCHANGEUSERURL = "https://core.elliemaeservices.com/epassutils/jedchangeuserpwd.asp";
    private const string SECONDARYCHECKUSERURL = "https://core.elliemaeservices.com/epassutils/jedcheckuserpwd.asp";
    private const string SECONDARYEMAILUSERURL = "https://core.elliemaeservices.com/epassutils/jedemailuserpwd.asp";
    private const string SECONDARYMANAGEUSERURL = "https://core.elliemaeservices.com/epassutils/jedchangeuserpwd.asp";
    private const string CHECKCOMPANYURL = "https://www.epassbusinesscenter.com/epassutils/jedcheckcompanypwd.asp";
    private const string EMAILCOMPANYURL = "https://www.epassbusinesscenter.com/epassutils/jedemailcompanypwd.asp";
    private const string SECONDARYCHECKCOMPANYURL = "https://core.elliemaeservices.com/epassutils/jedcheckcompanypwd.asp";
    private const string SECONDARYEMAILCOMPANYURL = "https://core.elliemaeservices.com/epassutils/jedemailcompanypwd.asp";
    private const string LOGINURL = "https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp";
    private const string DEFAULTURL = "https://www.epassbusinesscenter.com/epassutils/jedlogindefault.asp";
    private const string SECONDARYLOGINURL = "https://core.elliemaeservices.com/epassutils/ePASSLoginHA.asp";
    private const string SECONDARYDEFAULTURL = "https://core.elliemaeservices.com/epassutils/ePASSLoginDefaultHA.asp";

    [DllImport("wininet.dll")]
    private static extern bool InternetSetCookieEx(
      string url,
      string name,
      string data,
      int flags,
      string p3p);

    public static string ValidateUserPassword(
      bool remember,
      string password,
      string newPassword,
      out string errorMsg,
      Sessions.Session session)
    {
      string str1 = (string) null;
      errorMsg = string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        string postUrl = remember ? "https://www.epassbusinesscenter.com/epassutils/jedchangeuserpwd.asp" : "https://www.epassbusinesscenter.com/epassutils/jedcheckuserpwd.asp";
        string str2 = (string) null;
        try
        {
          if (session.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
            str2 = session.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
            {
              "Elli.Emn"
            }, 5);
        }
        catch (Exception ex)
        {
          Tracing.Log(EMNetworkPwdMgr.sw, nameof (EMNetworkPwdMgr), TraceLevel.Error, "Failed to retrieve the security token: " + (object) ex);
          return (string) null;
        }
        stringBuilder.Append("clientid=" + HttpUtility.UrlEncode(session.CompanyInfo.ClientID));
        stringBuilder.Append("&userid=" + HttpUtility.UrlEncode(session.UserID));
        stringBuilder.Append("&oldpassword=" + HttpUtility.UrlEncode(password));
        stringBuilder.Append("&newpassword=" + HttpUtility.UrlEncode(newPassword));
        str1 = EMNetworkPwdMgr.sendRequest(stringBuilder.ToString(), postUrl);
      }
      catch (Exception ex1)
      {
        Tracing.Log(EMNetworkPwdMgr.sw, TraceLevel.Error, nameof (EMNetworkPwdMgr), ex1.Message);
        try
        {
          string postUrl = remember ? "https://core.elliemaeservices.com/epassutils/jedchangeuserpwd.asp" : "https://core.elliemaeservices.com/epassutils/jedcheckuserpwd.asp";
          str1 = EMNetworkPwdMgr.sendRequest(stringBuilder.ToString(), postUrl);
        }
        catch (Exception ex2)
        {
          Tracing.Log(EMNetworkPwdMgr.sw, TraceLevel.Error, nameof (EMNetworkPwdMgr), ex2.Message);
          errorMsg = "The following error occurred when trying to check the user password:\n\n" + ex1.ToString();
        }
      }
      return str1;
    }

    public static string SendUserEmail(
      CompanyInfo companyInfo,
      UserInfo userInfo,
      out string errorMsg,
      Sessions.Session session)
    {
      string str1 = (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      errorMsg = string.Empty;
      string str2 = (string) null;
      try
      {
        if (session.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
          str2 = session.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
          {
            "Elli.Emn"
          }, 5);
      }
      catch (Exception ex)
      {
        Tracing.Log(EMNetworkPwdMgr.sw, nameof (EMNetworkPwdMgr), TraceLevel.Error, "Failed to retrieve the security token: " + (object) ex);
        return (string) null;
      }
      try
      {
        stringBuilder.Append("clientid=" + HttpUtility.UrlEncode(companyInfo.ClientID));
        stringBuilder.Append("&userid=" + HttpUtility.UrlEncode(userInfo.Userid));
        stringBuilder.Append("&userfirstname=" + HttpUtility.UrlEncode(userInfo.FirstName));
        stringBuilder.Append("&userlastname=" + HttpUtility.UrlEncode(userInfo.LastName));
        stringBuilder.Append("&userpersona=" + HttpUtility.UrlEncode(session.GetEPassPersonaDescriptor()));
        stringBuilder.Append("&useremail=" + HttpUtility.UrlEncode(userInfo.Email));
        stringBuilder.Append("&userphone=" + HttpUtility.UrlEncode(userInfo.Phone));
        str1 = EMNetworkPwdMgr.sendRequest(stringBuilder.ToString(), "https://www.epassbusinesscenter.com/epassutils/jedemailuserpwd.asp");
      }
      catch (Exception ex1)
      {
        Tracing.Log(EMNetworkPwdMgr.sw, TraceLevel.Error, nameof (EMNetworkPwdMgr), ex1.Message);
        try
        {
          str1 = EMNetworkPwdMgr.sendRequest(stringBuilder.ToString(), "https://core.elliemaeservices.com/epassutils/jedemailuserpwd.asp");
        }
        catch (Exception ex2)
        {
          Tracing.Log(EMNetworkPwdMgr.sw, TraceLevel.Error, nameof (EMNetworkPwdMgr), ex2.Message);
          errorMsg = "The following error occurred when trying to email the previous password:\r\n\r\n" + ex1.ToString();
        }
      }
      return str1;
    }

    public static string ChangeUserPassword(
      string password,
      string newPassword,
      out string errorMsg,
      Sessions.Session session)
    {
      string str1 = "";
      if (newPassword == "")
      {
        errorMsg = "";
        return str1;
      }
      EncompassUserClient encompassUserClient = new EncompassUserClient((System.ServiceModel.Channels.Binding) new BasicHttpBinding(BasicHttpSecurityMode.Transport), new EndpointAddress(new Uri("https://epass.elliemaeservices.com/EMNservices/EncompassUser.svc"), Array.Empty<AddressHeader>()));
      string str2;
      try
      {
        encompassUserClient.UserChangePwd(session.CompanyInfo.ClientID, session.UserID, password, newPassword, session.CompanyInfo.Password);
        str2 = "success";
        errorMsg = "";
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Password Synchronization Failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        errorMsg = "The following error occurred when trying to check the user password:\n\n" + ex.ToString();
        str2 = ex.Message;
      }
      return str2;
    }

    public static string ManageUserPassword(
      CompanyInfo companyInfo,
      UserInfo userInfo,
      string newPassword,
      out string errorMsg,
      Sessions.Session session)
    {
      string str = "";
      errorMsg = string.Empty;
      if (newPassword == "")
        return str;
      bool applicationRight = session.FeaturesAclManager.GetUserApplicationRight(AclFeature.SettingsTab_OrganizationsUser);
      if (!session.UserInfo.IsAdministrator() && !applicationRight)
      {
        errorMsg = "This function can only be used by users with administrator persona.";
        return str;
      }
      string AdminPwdEncrypted = "0x" + BitConverter.ToString(session.OrganizationManager.GetUserPasswordHash("admin", applicationRight, true)).Replace("-", "");
      EncompassUserClient encompassUserClient = new EncompassUserClient((System.ServiceModel.Channels.Binding) new BasicHttpBinding(BasicHttpSecurityMode.Transport), new EndpointAddress(new Uri("https://epass.elliemaeservices.com/EMNservices/EncompassUser.svc"), Array.Empty<AddressHeader>()));
      try
      {
        encompassUserClient.AdminChangeUserPwd(session.CompanyInfo.ClientID, userInfo.Userid, newPassword, session.CompanyInfo.Password, session.UserID, session.Password, AdminPwdEncrypted);
        return "success";
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Password Synchronization Failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return ex.Message;
      }
    }

    public static string AddUser(string password, Sessions.Session session, UserInfo newUser)
    {
      return EMNetworkPwdMgr.addUser("https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp", password, session, newUser);
    }

    private static string addUser(
      string url,
      string password,
      Sessions.Session session,
      UserInfo userInfo)
    {
      bool flag1 = true;
      CompanyInfo companyInfo = session.CompanyInfo;
      OrgInfo branchInfo = session.StartupInfo.BranchInfo;
      if (branchInfo != null)
        companyInfo = new CompanyInfo(companyInfo.ClientID, branchInfo.CompanyName, branchInfo.CompanyAddress.Street1, branchInfo.CompanyAddress.City, branchInfo.CompanyAddress.State, branchInfo.CompanyAddress.Zip, branchInfo.CompanyPhone, branchInfo.CompanyFax, companyInfo.Password, new string[4]
        {
          branchInfo.DBAName1,
          branchInfo.DBAName2,
          branchInfo.DBAName3,
          branchInfo.DBAName4
        }, (BranchExtLicensing) branchInfo.OrgBranchLicensing.Clone());
      string str1 = string.Empty;
      switch (session.StartupInfo.RuntimeEnvironment)
      {
        case RuntimeEnvironment.Default:
          str1 = "Default";
          break;
        case RuntimeEnvironment.Hosted:
          str1 = "Hosted";
          break;
      }
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting = ((ServicesAclManager) session.ACL.GetAclManager(AclCategory.Services)).GetServicesDefaultSetting();
      string str2 = string.Empty;
      string str3 = (string) null;
      switch (servicesDefaultSetting)
      {
        case ServiceAclInfo.ServicesDefaultSetting.None:
          str3 = "n";
          break;
        case ServiceAclInfo.ServicesDefaultSetting.Custom:
          str3 = "c";
          str2 = EMNetworkPwdMgr.getCompanyServices(session);
          break;
        case ServiceAclInfo.ServicesDefaultSetting.All:
          str3 = "y";
          break;
      }
      string str4 = (string) null;
      try
      {
        if (session.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
          str4 = session.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
          {
            "Elli.Emn"
          }, 5);
      }
      catch (Exception ex)
      {
        Tracing.Log(EMNetworkPwdMgr.sw, nameof (EMNetworkPwdMgr), TraceLevel.Error, "User creation failed in ePass. Failed to retrieve the security token: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) null, "Failed to generate security token for access to ICE Mortgage Technology Network. User account creation in ICE Mortgage Technology Network failed.");
        return (string) null;
      }
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("clientid=" + HttpUtility.UrlEncode(companyInfo.ClientID));
      stringBuilder.Append("&clientname=" + HttpUtility.UrlEncode(companyInfo.Name));
      stringBuilder.Append("&clientaddress=" + HttpUtility.UrlEncode(companyInfo.Address));
      stringBuilder.Append("&clientcity=" + HttpUtility.UrlEncode(companyInfo.City));
      stringBuilder.Append("&clientstate=" + HttpUtility.UrlEncode(companyInfo.State));
      stringBuilder.Append("&clientzip=" + HttpUtility.UrlEncode(companyInfo.Zip));
      stringBuilder.Append("&clientphone=" + HttpUtility.UrlEncode(companyInfo.Phone));
      stringBuilder.Append("&clientfax=" + HttpUtility.UrlEncode(companyInfo.Fax));
      stringBuilder.Append("&clientpassword=" + HttpUtility.UrlEncode(companyInfo.Password));
      stringBuilder.Append("&userid=" + HttpUtility.UrlEncode(userInfo.Userid));
      stringBuilder.Append("&userfirstname=" + HttpUtility.UrlEncode(userInfo.FirstName));
      stringBuilder.Append("&userlastname=" + HttpUtility.UrlEncode(userInfo.LastName));
      stringBuilder.Append("&userpersona=" + HttpUtility.UrlEncode(session.GetEPassPersonaDescriptor()));
      stringBuilder.Append("&useremail=" + HttpUtility.UrlEncode(userInfo.Email));
      stringBuilder.Append("&userphone=" + HttpUtility.UrlEncode(userInfo.Phone));
      stringBuilder.Append("&userpassword=" + HttpUtility.UrlEncode(str4));
      stringBuilder.Append("&personal=" + HttpUtility.UrlEncode(str3));
      stringBuilder.Append("&version=" + HttpUtility.UrlEncode(VersionInformation.CurrentVersion.GetExtendedVersion(session.EncompassEdition)));
      stringBuilder.Append("&environment=" + HttpUtility.UrlEncode(str1));
      stringBuilder.Append("&companyservices=" + HttpUtility.UrlEncode(str2));
      stringBuilder.Append("&NMLSOriginatorID=" + HttpUtility.UrlEncode(userInfo.NMLSOriginatorID));
      stringBuilder.Append("&NMLSExpirationDate=" + HttpUtility.UrlEncode(userInfo.NMLSExpirationDate == DateTime.MaxValue ? "" : userInfo.NMLSExpirationDate.ToString("MM/dd/yyyy")));
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml("<LOLICENSES></LOLICENSES>");
        XmlElement documentElement = xmlDocument.DocumentElement;
        foreach (LOLicenseInfo loLicense in session.OrganizationManager.GetLOLicenses(session.UserInfo.Userid))
        {
          if (loLicense.Enabled && loLicense.License != string.Empty)
          {
            XmlElement element = xmlDocument.CreateElement("LIC");
            documentElement.AppendChild((XmlNode) element);
            element.SetAttribute("ST", loLicense.StateAbbr.Trim());
            element.SetAttribute("LN", loLicense.License.Trim());
            element.SetAttribute("ED", loLicense.ExpirationDate == DateTime.MaxValue ? "" : loLicense.ExpirationDate.ToString("MM/dd/yyyy"));
          }
        }
        stringBuilder.Append("&lolicensesxml=" + HttpUtility.UrlEncode(xmlDocument.OuterXml));
      }
      catch (Exception ex)
      {
        Tracing.Log(EMNetworkPwdMgr.sw, TraceLevel.Error, nameof (EMNetworkPwdMgr), ex.Message);
        return (string) null;
      }
      string str5 = (string) null;
      string cookieHeader = (string) null;
      string p3p = (string) null;
      try
      {
        using (Tracing.StartTimer(EMNetworkPwdMgr.sw, nameof (EMNetworkPwdMgr), TraceLevel.Info, "Performing ePASS Login..."))
        {
          HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
          httpWebRequest.KeepAlive = false;
          httpWebRequest.Method = "POST";
          httpWebRequest.ContentType = "application/x-www-form-urlencoded";
          httpWebRequest.ContentLength = (long) stringBuilder.Length;
          httpWebRequest.Timeout = 5000;
          Tracing.Log(EMNetworkPwdMgr.sw, nameof (EMNetworkPwdMgr), TraceLevel.Info, "Sending ePASS login request...");
          StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
          streamWriter.Write(stringBuilder.ToString());
          streamWriter.Close();
          Tracing.Log(EMNetworkPwdMgr.sw, nameof (EMNetworkPwdMgr), TraceLevel.Info, "Reading ePASS login response...");
          HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
          cookieHeader = response.Headers["Set-Cookie"];
          p3p = response.Headers["P3P"];
          StreamReader streamReader = new StreamReader(response.GetResponseStream());
          str5 = streamReader.ReadToEnd();
          streamReader.Close();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(EMNetworkPwdMgr.sw, TraceLevel.Error, nameof (EMNetworkPwdMgr), ex.Message);
        switch (url)
        {
          case "https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp":
            return EMNetworkPwdMgr.addUser("https://core.elliemaeservices.com/epassutils/ePASSLoginHA.asp", password, session, userInfo);
          case "https://www.epassbusinesscenter.com/epassutils/jedlogindefault.asp":
            return EMNetworkPwdMgr.addUser("https://core.elliemaeservices.com/epassutils/ePASSLoginDefaultHA.asp", password, session, userInfo);
          default:
            if (flag1)
            {
              int num = (int) Utils.Dialog((IWin32Window) null, "We have detected consecutive attempts with invalid password and your access has been restricted for 15 minutes. Please check your internet connection before attempting again.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return (string) null;
        }
      }
      if (cookieHeader == null)
      {
        if (flag1)
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "There was a problem establishing your Session. This might be due to cookies being disabled.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        return (string) null;
      }
      Uri uri = new Uri(url);
      CookieContainer cookieContainer = new CookieContainer();
      cookieContainer.SetCookies(uri, cookieHeader);
      foreach (Cookie cookie in cookieContainer.GetCookies(uri))
      {
        string str6 = cookie.ToString();
        if (cookie.Expires != DateTime.MinValue)
          str6 = str6 + "; expires=" + cookie.Expires.ToString("r");
        string data = str6 + "; path=" + cookie.Path;
        bool flag2 = EMNetworkPwdMgr.InternetSetCookieEx(uri.AbsoluteUri, (string) null, data, 64, p3p);
        Tracing.Log(EMNetworkPwdMgr.sw, TraceLevel.Verbose, nameof (EMNetworkPwdMgr), "SetCookie=" + flag2.ToString() + " " + data);
      }
      return str5;
    }

    private static string getCompanyServices(Sessions.Session session)
    {
      string[] services = ((ServicesAclManager) session.ACL.GetAclManager(AclCategory.Services)).GetServices(false);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<SERVICES/>");
      foreach (string str in services)
      {
        XmlElement element = xmlDocument.CreateElement("CATEGORY");
        element.SetAttribute("Title", str);
        xmlDocument.DocumentElement.AppendChild((XmlNode) element);
      }
      return xmlDocument.OuterXml;
    }

    public static string ValidateCompanyPassword(
      string clientID,
      string password,
      out string errorMsg)
    {
      string str = (string) null;
      errorMsg = string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("clientid=" + HttpUtility.UrlEncode(clientID));
      stringBuilder.Append("&clientpassword=" + HttpUtility.UrlEncode(password));
      try
      {
        str = EMNetworkPwdMgr.sendRequest(stringBuilder.ToString(), "https://www.epassbusinesscenter.com/epassutils/jedcheckcompanypwd.asp");
      }
      catch (Exception ex1)
      {
        Tracing.Log(EMNetworkPwdMgr.sw, TraceLevel.Error, nameof (EMNetworkPwdMgr), ex1.Message);
        try
        {
          str = EMNetworkPwdMgr.sendRequest(stringBuilder.ToString(), "https://core.elliemaeservices.com/epassutils/jedcheckcompanypwd.asp");
        }
        catch (Exception ex2)
        {
          Tracing.Log(EMNetworkPwdMgr.sw, TraceLevel.Error, nameof (EMNetworkPwdMgr), ex2.Message);
          errorMsg = "The following error occurred when trying to check the company password:\r\n\r\n";
        }
      }
      return str;
    }

    public static string SendCompanyEmail(
      CompanyInfo companyInfo,
      UserInfo userInfo,
      out string errorMsg,
      Sessions.Session session)
    {
      string str = (string) null;
      errorMsg = string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        stringBuilder.Append("clientid=" + HttpUtility.UrlEncode(companyInfo.ClientID));
        stringBuilder.Append("&userid=" + HttpUtility.UrlEncode(userInfo.Userid));
        stringBuilder.Append("&userfirstname=" + HttpUtility.UrlEncode(userInfo.FirstName));
        stringBuilder.Append("&userlastname=" + HttpUtility.UrlEncode(userInfo.LastName));
        stringBuilder.Append("&userpersona=" + HttpUtility.UrlEncode(session.GetEPassPersonaDescriptor()));
        stringBuilder.Append("&useremail=" + HttpUtility.UrlEncode(userInfo.Email));
        stringBuilder.Append("&userphone=" + HttpUtility.UrlEncode(userInfo.Phone));
        str = EMNetworkPwdMgr.sendRequest(stringBuilder.ToString(), "https://www.epassbusinesscenter.com/epassutils/jedemailcompanypwd.asp");
      }
      catch (Exception ex1)
      {
        Tracing.Log(EMNetworkPwdMgr.sw, TraceLevel.Error, nameof (EMNetworkPwdMgr), ex1.Message);
        try
        {
          str = EMNetworkPwdMgr.sendRequest(stringBuilder.ToString(), "https://core.elliemaeservices.com/epassutils/jedemailcompanypwd.asp");
        }
        catch (Exception ex2)
        {
          Tracing.Log(EMNetworkPwdMgr.sw, TraceLevel.Error, nameof (EMNetworkPwdMgr), ex2.Message);
          errorMsg = "The following error occurred when trying to email the company password:\r\n\r\n" + ex1.ToString();
        }
      }
      return str;
    }

    private static string sendRequest(string postData, string postUrl)
    {
      string empty = string.Empty;
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(postUrl);
      httpWebRequest.KeepAlive = false;
      httpWebRequest.Method = "POST";
      httpWebRequest.ContentType = "application/x-www-form-urlencoded";
      httpWebRequest.ContentLength = (long) postData.Length;
      using (Stream requestStream = httpWebRequest.GetRequestStream())
      {
        using (StreamWriter streamWriter = new StreamWriter(requestStream))
          streamWriter.Write(postData);
      }
      using (WebResponse response = httpWebRequest.GetResponse())
      {
        using (Stream responseStream = response.GetResponseStream())
        {
          using (StreamReader streamReader = new StreamReader(responseStream))
            return streamReader.ReadToEnd();
        }
      }
    }
  }
}
