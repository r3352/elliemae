// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.EpassLogin
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  [ComVisible(false)]
  public class EpassLogin
  {
    public static Task<bool> silentEpassLoginTask = (Task<bool>) null;
    private const string className = "EpassLogin";
    private static readonly string sw = Tracing.SwEpass;
    private const string LOGINURL = "https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp";
    private const string DEFAULTURL = "https://www.epassbusinesscenter.com/epassutils/jedlogindefault.asp";
    private const string SECONDARYLOGINURL = "https://core.elliemaeservices.com/epassutils/ePASSLoginHA.asp";
    private const string SECONDARYDEFAULTURL = "https://core.elliemaeservices.com/epassutils/ePASSLoginDefaultHA.asp";
    private static string loginID = (string) null;
    private static bool isEncompassSelfHosted = false;

    [DllImport("wininet.dll")]
    private static extern bool InternetSetCookieEx(
      string url,
      string name,
      string data,
      int flags,
      string p3p);

    public static string LoginID
    {
      get => EpassLogin.loginID;
      set => EpassLogin.loginID = value;
    }

    public static string LoginPassword => Session.Password;

    public static bool LoginUser(bool showDialogs)
    {
      if (EpassLogin.LoginID != null)
        return true;
      string str = EpassLogin.login("https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp", showDialogs, false, Session.DefaultInstance);
      if (str == null)
        return false;
      if (showDialogs)
        Modules.DownloadUserLicense();
      else
        Modules.BeginDownloadUserLicense();
      EpassLogin.LoginID = str;
      return true;
    }

    public static bool IsEncompassSelfHosted => EpassLogin.isEncompassSelfHosted;

    public static bool LoginRequired(bool showDialogs)
    {
      return EpassLogin.LoginRequired(showDialogs, Session.DefaultInstance);
    }

    public static bool LoginRequired(bool showDialogs, Sessions.Session session)
    {
      if (EpassLogin.LoginID != null && EpassLogin.LoginID != "0")
        return true;
      string str = EpassLogin.login("https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp", showDialogs, true, session);
      if (str == null)
        return false;
      Modules.DownloadUserLicense(session);
      EpassLogin.LoginID = str;
      return true;
    }

    private static string login(
      string url,
      bool showDialogs,
      bool required,
      Sessions.Session session)
    {
      CompanyInfo companyInfo = session.CompanyInfo;
      UserInfo userInfo = session.UserInfo;
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
        case EllieMae.EMLite.Common.RuntimeEnvironment.Default:
          str1 = "Default";
          break;
        case EllieMae.EMLite.Common.RuntimeEnvironment.Hosted:
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
          str2 = EpassLogin.getCompanyServices(session);
          break;
        case ServiceAclInfo.ServicesDefaultSetting.All:
          str3 = "y";
          break;
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
      stringBuilder.Append("&userpassword=" + HttpUtility.UrlEncode(EpassLogin.LoginPassword));
      stringBuilder.Append("&personal=" + HttpUtility.UrlEncode(str3));
      stringBuilder.Append("&version=" + HttpUtility.UrlEncode(EpassLogin.EpassVersion(session)));
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
        Tracing.Log(EpassLogin.sw, TraceLevel.Error, nameof (EpassLogin), ex.Message);
        return (string) null;
      }
      string str4 = (string) null;
      string cookieHeader = (string) null;
      string p3p = (string) null;
      try
      {
        using (Tracing.StartTimer(EpassLogin.sw, nameof (EpassLogin), TraceLevel.Info, "Performing ePASS Login..."))
        {
          HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
          httpWebRequest.KeepAlive = false;
          httpWebRequest.Method = "POST";
          httpWebRequest.ContentType = "application/x-www-form-urlencoded";
          httpWebRequest.ContentLength = (long) stringBuilder.Length;
          httpWebRequest.Timeout = 5000;
          if (EpassLogin.LoginPassword != null)
            httpWebRequest.Headers.Add("Authorization", "EMAuth " + EpassLogin.LoginPassword);
          Tracing.Log(EpassLogin.sw, nameof (EpassLogin), TraceLevel.Info, "Sending ePASS login request...");
          StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
          streamWriter.Write(stringBuilder.ToString());
          streamWriter.Close();
          Tracing.Log(EpassLogin.sw, nameof (EpassLogin), TraceLevel.Info, "Reading ePASS login response...");
          HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
          cookieHeader = response.Headers["Set-Cookie"];
          p3p = response.Headers["P3P"];
          StreamReader streamReader = new StreamReader(response.GetResponseStream());
          str4 = streamReader.ReadToEnd();
          streamReader.Close();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(EpassLogin.sw, TraceLevel.Error, nameof (EpassLogin), ex.Message);
        switch (url)
        {
          case "https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp":
            return EpassLogin.login("https://core.elliemaeservices.com/epassutils/ePASSLoginHA.asp", true, required, session);
          case "https://www.epassbusinesscenter.com/epassutils/jedlogindefault.asp":
            return EpassLogin.login("https://core.elliemaeservices.com/epassutils/ePASSLoginDefaultHA.asp", true, required, session);
          default:
            if (ex.Message.Contains("(403)"))
            {
              SelfHostedMessage selfHostedMessage = new SelfHostedMessage();
              if (selfHostedMessage.ShowDialog() == DialogResult.OK)
                selfHostedMessage.Close();
              EpassLogin.isEncompassSelfHosted = true;
            }
            else if (showDialogs)
            {
              int num = (int) Utils.Dialog((IWin32Window) null, "We have detected consecutive attempts with invalid password and your access has been restricted for 15 minutes. Please check your internet connection before attempting again.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return (string) null;
        }
      }
      if (cookieHeader == null)
      {
        if (showDialogs)
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "There was a problem establishing your Session. This might be due to cookies being disabled.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        return (string) null;
      }
      switch (str4)
      {
        case "Invalid Company Password":
          if (showDialogs && new CompanyPasswordDialog(session).ShowDialog() == DialogResult.OK)
            return EpassLogin.login(url, showDialogs, required, session);
          Modules.ClearUserLicense();
          return (string) null;
        case "Invalid User Password":
          if (showDialogs)
          {
            int num1 = (int) Utils.Dialog((IWin32Window) null, "There was an issue with your request to the ICE Mortgage Technology Network.  Please contact your admin.");
          }
          Tracing.Log(EpassLogin.sw, TraceLevel.Error, nameof (EpassLogin), "parseLoginResponse: Invalid SSO Token login to ePass.");
          return (string) null;
        default:
          Uri uri = new Uri(url);
          CookieContainer cookieContainer = new CookieContainer();
          cookieContainer.SetCookies(uri, cookieHeader);
          foreach (Cookie cookie in cookieContainer.GetCookies(uri))
          {
            string str5 = cookie.ToString();
            if (cookie.Expires != DateTime.MinValue)
              str5 = str5 + "; expires=" + cookie.Expires.ToString("r");
            string data = str5 + "; path=" + cookie.Path;
            bool flag = EpassLogin.InternetSetCookieEx(uri.AbsoluteUri, (string) null, data, 64, p3p);
            Tracing.Log(EpassLogin.sw, TraceLevel.Verbose, nameof (EpassLogin), "SetCookie=" + flag.ToString() + " " + data);
          }
          return str4;
      }
    }

    public static string EpassVersion(Sessions.Session session)
    {
      return VersionInformation.CurrentVersion.GetExtendedVersion(session.EncompassEdition);
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
  }
}
