// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LicenseManager
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Services;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.VersionInterface15;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LicenseManager
  {
    private const string className = "LicenseManager�";
    private static readonly string hostName = Dns.GetHostName();
    private static readonly JedVersion assemblyVersion = LicenseManager.getAssemblyFileVersion();
    private const string LicenseErrMsg = "Your login has been denied because the Encompass software is not properly registered. If you are running offline, you should re-register Encompass using the Admin Tools, which can be found in the Encompass program group in the Start Menu. If you are connecting to an Encompass Server, please contact your server administrator.�";
    private static readonly string url;
    private static ConcurrentDictionary<string, Task<string>> previousAcquireLicenseResposes = new ConcurrentDictionary<string, Task<string>>();

    private LicenseManager()
    {
    }

    static LicenseManager()
    {
      WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
      LicenseManager.url = EnConfigurationSettings.AppSettings["LicenseUrl"];
      if (!string.IsNullOrWhiteSpace(LicenseManager.url) && Uri.IsWellFormedUriString(LicenseManager.url, UriKind.Absolute))
        return;
      LicenseManager.url = "https://encompass.elliemae.com/license/";
    }

    public static void ReleaseAllLicenses(ClientContext context)
    {
      try
      {
        CompanyInfo companyInfo = Company.GetCompanyInfo((IClientContext) context);
        if (companyInfo.ClientID == "")
          return;
        Hashtable postData = new Hashtable();
        postData.Add((object) "clientid", (object) companyInfo.ClientID);
        postData.Add((object) "server", (object) LicenseManager.hostName);
        LoginParameters loginParameters = new LoginParameters("", "", (ServerIdentity) null, "", "", false);
        postData.Add((object) "windows", (object) loginParameters.OSVersion.Version.ToString());
        postData.Add((object) "version", (object) VersionInformation.CurrentVersion.Version.NormalizedVersion);
        postData.Add((object) "memory", (object) loginParameters.TotalMemory.ToString());
        postData.Add((object) "processorid", (object) loginParameters.ProcessorID);
        postData.Add((object) "processorspeed", (object) loginParameters.ProcessorSpeed);
        string empty = string.Empty;
        context.TraceLog.WriteInfo(nameof (LicenseManager), "Releasing all licenses for server " + LicenseManager.hostName);
        new LicenseRequest(LicenseManager.url + "reset.asp", postData).Send();
      }
      catch (Exception ex)
      {
        context.TraceLog.WriteWarning(nameof (LicenseManager), "Error communicating with License Server in ReleaseAllLicenses(): " + ex.Message);
      }
    }

    public static void ValidateServerLicense(ClientContext context)
    {
      LicenseInfo serverLicense = Company.GetServerLicense((IClientContext) context);
      if (serverLicense == null)
        Err.Raise((IClientContext) context, nameof (LicenseManager), (ServerException) new LicenseException((LicenseInfo) null, LicenseExceptionType.InvalidLicense, "Your login has been denied because the Encompass software is not properly registered. If you are running offline, you should re-register Encompass using the Admin Tools, which can be found in the Encompass program group in the Start Menu. If you are connecting to an Encompass Server, please contact your server administrator."));
      if (serverLicense.Type == LicenseType.Demo)
        return;
      if (serverLicense.Version < LicenseManager.assemblyVersion)
        Err.Raise((IClientContext) context, nameof (LicenseManager), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.InvalidLicense, "Your login has been denied because the Encompass software is not properly registered. If you are running offline, you should re-register Encompass using the Admin Tools, which can be found in the Encompass program group in the Start Menu. If you are connecting to an Encompass Server, please contact your server administrator."));
      if (serverLicense.UserLimit >= 0)
        return;
      Err.Raise((IClientContext) context, nameof (LicenseManager), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.InvalidLicense, "Your login has been denied because the Encompass software is not properly registered. If you are running offline, you should re-register Encompass using the Admin Tools, which can be found in the Encompass program group in the Start Menu. If you are connecting to an Encompass Server, please contact your server administrator."));
    }

    public static void AcquireLicense(LoginParameters loginParams, IClientContext context)
    {
      string str1 = string.Empty;
      LicenseInfo serverLicense = Company.GetServerLicense(context);
      if (serverLicense.IsTrialVersion)
      {
        if ((DateTime.Now - loginParams.ClientTimestamp).Duration().TotalHours > 30.0)
          Err.Raise(context, nameof (LicenseManager), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.ClockDiscrepancy, "The clock on your computer and the clock of the Encompass Server differ by more than 24 hours. You must correct this problem in order to log in."));
        if (DateTime.Now.AddDays(-1.0) > serverLicense.Expires)
          Err.Raise(context, nameof (LicenseManager), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.TrialExpired, "Thank you for evaluating Encompass.  Your trial version has expired.  Please contact your ICE Mortgage Technology account representative at 1-888-955-9100 to upgrade your product."));
      }
      CompanyInfo companyInfo = Company.GetCompanyInfo(context);
      EncompassSystemInfo encompassSystemInfo = EncompassSystemDbAccessor.GetEncompassSystemInfo(context);
      if (companyInfo.ClientID == "" || serverLicense.ClientID != companyInfo.ClientID)
        Err.Raise(context, nameof (LicenseManager), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.InvalidLicense, "Your login has been denied because the Encompass software is not properly registered. If you are running offline, you should re-register Encompass using the Admin Tools, which can be found in the Encompass program group in the Start Menu. If you are connecting to an Encompass Server, please contact your server administrator."));
      UserInfo userInfo = (UserInfo) null;
      string str2 = "";
      bool flag = false;
      using (context.MakeCurrent())
      {
        userInfo = UserStore.GetLatestVersion(loginParams.UserID).UserInfo;
        str2 = LicenseManager.getPersonaDescription(userInfo);
      }
      try
      {
        Hashtable postData = new Hashtable();
        postData.Add((object) "clientid", (object) companyInfo.ClientID);
        postData.Add((object) "userid", (object) loginParams.UserID);
        postData.Add((object) "server", (object) LicenseManager.hostName);
        postData.Add((object) "license", (object) serverLicense.Type.ToString());
        postData.Add((object) "edition", (object) serverLicense.Edition.ToString());
        postData.Add((object) "cdkey", (object) serverLicense.CDKey);
        postData.Add((object) "windows", (object) loginParams.OSVersion.Version.ToString());
        postData.Add((object) "winarch", (object) loginParams.OSPlatform);
        postData.Add((object) "version", (object) VersionInformation.CurrentVersion.GetExtendedVersion(serverLicense.Edition));
        postData.Add((object) "memory", (object) loginParams.TotalMemory.ToString());
        postData.Add((object) "processorid", (object) loginParams.ProcessorID);
        postData.Add((object) "processorspeed", (object) loginParams.ProcessorSpeed);
        postData.Add((object) "serveruri", loginParams.Server.Uri == (Uri) null ? (object) "" : (object) loginParams.Server.Uri.AbsoluteUri);
        postData.Add((object) "systemid", (object) encompassSystemInfo.SystemID);
        postData.Add((object) "appname", (object) loginParams.AppName);
        postData.Add((object) "usercount", (object) User.GetEnabledUserCount(context));
        postData.Add((object) "environment", (object) ServerGlobals.RuntimeEnvironment);
        postData.Add((object) "clientenv", (object) loginParams.ClientEnvironment);
        postData.Add((object) "fname", (object) userInfo.FirstName);
        postData.Add((object) "lname", (object) userInfo.LastName);
        postData.Add((object) "email", (object) userInfo.Email);
        postData.Add((object) "phone", (object) userInfo.Phone);
        postData.Add((object) "persona", (object) str2);
        DbVersion dbVersion;
        using (DbAccessManager dbAccessManager = new DbAccessManager())
          dbVersion = dbAccessManager.GetDbVersion();
        postData.Add((object) "sqlversion", (object) dbVersion);
        postData.Add((object) "clientversion", (object) loginParams.ClientDisplayVersion);
        if (serverLicense.IsTrialVersion)
          postData.Add((object) "expires", (object) serverLicense.Expires.ToShortDateString());
        context.TraceLog.WriteInfo(nameof (LicenseManager), "Obtaining license for user " + loginParams.UserID);
        Task<string> task = (Task<string>) null;
        if (LicenseManager.previousAcquireLicenseResposes.ContainsKey(companyInfo.ClientID))
          task = LicenseManager.previousAcquireLicenseResposes[companyInfo.ClientID];
        LicenseManager.previousAcquireLicenseResposes[companyInfo.ClientID] = Task.Run<string>((Func<string>) (() => new LicenseRequest(LicenseManager.url + "login.asp", postData).Send()));
        if (task != null && task.IsCompleted)
        {
          flag = true;
          str1 = task.Result;
        }
        else
        {
          if (serverLicense.Enabled)
            return;
          throw new Exception();
        }
      }
      catch (Exception ex)
      {
        if (flag)
          context.TraceLog.WriteWarning(nameof (LicenseManager), "Error communicating with License Server acquiring license for user " + loginParams.UserID + ": " + ex.Message);
        if (!serverLicense.Enabled)
        {
          EncompassServer.RaiseEvent(context, (ServerEvent) new LicenseEvent(LicenseEventType.Denied, loginParams.UserID));
          Err.Raise(context, nameof (LicenseManager), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.AccountDisabled, "Your account has been disabled.  Please call 800-777-1718."));
        }
      }
      context.TraceLog.WriteDebug(nameof (LicenseManager), "License Server response for user " + loginParams.UserID + ": " + str1);
      if (str1 == "Expired")
        Err.Raise(context, nameof (LicenseManager), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.TrialExpired, "Thank you for evaluating Encompass.  Your trial version has expired.  Please contact your ICE Mortgage Technology account representative at 1-888-955-9100 to upgrade your product."));
      if (str1 == "Disabled")
      {
        LicenseManager.disableLicense(serverLicense);
        EncompassServer.RaiseEvent(context, (ServerEvent) new LicenseEvent(LicenseEventType.Denied, loginParams.UserID));
        Err.Raise(context, nameof (LicenseManager), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.AccountDisabled, "Your account has been disabled.  Please call 800-777-1718."));
      }
      if (str1 == "Exceeded License")
      {
        EncompassServer.RaiseEvent(context, (ServerEvent) new LicenseEvent(LicenseEventType.Denied, loginParams.UserID));
        Err.Raise(context, nameof (LicenseManager), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.LicensesExceeded, "All client licenses for your company are currently in use.  Please ask a colleague to log out of Encompass, or purchase additional licenses."));
      }
      if (str1 == "Invalid Protocol")
      {
        EncompassServer.RaiseEvent(context, (ServerEvent) new LicenseEvent(LicenseEventType.Denied, loginParams.UserID));
        Err.Raise(context, nameof (LicenseManager), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.InvalidProtocol, "You are not licensed to use the specified protocol."));
      }
      if (str1 == "Invalid ClientID")
      {
        LicenseManager.disableLicense(serverLicense);
        EncompassServer.RaiseEvent(context, (ServerEvent) new LicenseEvent(LicenseEventType.Denied, loginParams.UserID));
        Err.Raise(context, nameof (LicenseManager), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.InvalidClientID, "The Client ID registered with Encompass is invalid. Please re-register your version of Encompass using the Admin Tools application."));
      }
      LicenseManager.enableLicense(serverLicense);
      EncompassServer.RaiseEvent(context, (ServerEvent) new LicenseEvent(LicenseEventType.Granted, loginParams.UserID));
    }

    public static void ReleaseLicense(LoginParameters loginParams, IClientContext context)
    {
      if (loginParams.UserID == "(trusted)")
        return;
      if (!loginParams.LicenseRequired)
        return;
      try
      {
        CompanyInfo companyInfo = Company.GetCompanyInfo(context);
        if (companyInfo.ClientID == "")
          return;
        Hashtable postData = new Hashtable();
        postData.Add((object) "clientid", (object) companyInfo.ClientID);
        postData.Add((object) "userid", (object) loginParams.UserID);
        postData.Add((object) "server", (object) LicenseManager.hostName);
        postData.Add((object) "environment", (object) ServerGlobals.RuntimeEnvironment);
        string empty = string.Empty;
        context.TraceLog.WriteInfo(nameof (LicenseManager), "Releasing license for user " + loginParams.UserID);
        new LicenseRequest(LicenseManager.url + "logout.asp", postData).Send();
        EncompassServer.RaiseEvent(context, (ServerEvent) new LicenseEvent(LicenseEventType.Released, loginParams.UserID));
      }
      catch (Exception ex)
      {
        context.TraceLog.WriteWarning(nameof (LicenseManager), "Error communicating with License Server releasing license for user " + loginParams.UserID + ": " + ex.Message);
      }
    }

    public static void RefreshServerLicense(IClientContext context)
    {
      try
      {
        LicenseInfo serverLicense = Company.GetServerLicense(context);
        if (serverLicense == null)
          return;
        Hashtable postData = new Hashtable();
        postData.Add((object) "clientid", (object) serverLicense.ClientID);
        postData.Add((object) "license", (object) serverLicense.Type.ToString());
        postData.Add((object) "edition", (object) serverLicense.Edition.ToString());
        postData.Add((object) "environment", (object) ServerGlobals.RuntimeEnvironment);
        string empty = string.Empty;
        context.TraceLog.WriteInfo(nameof (LicenseManager), "Refreshing server license");
        try
        {
          string xml = new LicenseRequest(LicenseManager.url + "getlicenseinfo2.asp", postData).Send();
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml(xml);
          serverLicense.UserLimit = int.Parse(xmlDocument.DocumentElement.GetAttribute("UserLimit"));
          serverLicense.UserLimitFlexPercent = int.Parse(xmlDocument.DocumentElement.GetAttribute("Threshold"));
          Company.UpdateServerLicense(context, serverLicense);
        }
        catch (Exception ex)
        {
          context.TraceLog.WriteWarning(nameof (LicenseManager), "Error communicating with License Server while refreshing user limit data: " + ex.StackTrace);
          throw ex;
        }
      }
      catch (Exception ex)
      {
        context.TraceLog.WriteWarning(nameof (LicenseManager), "Error while refreshing license for the client : " + context.InstanceName + " Exception:" + ex.StackTrace);
        throw;
      }
    }

    private static JedVersion getAssemblyFileVersion()
    {
      try
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyFileVersionAttribute), false);
        return customAttributes.Length == 0 || !(customAttributes[0] is AssemblyFileVersionAttribute versionAttribute) ? JedVersion.Unknown : JedVersion.Parse(versionAttribute.Version);
      }
      catch
      {
        return JedVersion.Unknown;
      }
    }

    private static void disableLicense(LicenseInfo license)
    {
      if (!license.Enabled)
        return;
      license.Enabled = false;
      Company.UpdateServerLicense(license);
    }

    private static void enableLicense(LicenseInfo license)
    {
      if (license.Enabled)
        return;
      license.Enabled = true;
      Company.UpdateServerLicense(license);
    }

    public static string getPersonaDescription(UserInfo userInfo)
    {
      try
      {
        RealWorldRoleIDNameProvider roleIdNameProvider = new RealWorldRoleIDNameProvider();
        ArrayList arrayList = new ArrayList();
        if (userInfo.IsAdministrator())
          arrayList.Add((object) "Admin");
        foreach (RolesMappingInfo rolesMappingInfo in WorkflowBpmDbAccessor.GetUsersRoleMapping(userInfo.Userid))
        {
          string name = roleIdNameProvider.GetName((object) rolesMappingInfo.RealWorldRoleID);
          if (!arrayList.Contains((object) name))
            arrayList.Add((object) name);
        }
        if (arrayList.Count == 0)
          arrayList.Add((object) "Other");
        return string.Join(" + ", (string[]) arrayList.ToArray(typeof (string)));
      }
      catch
      {
      }
      return "";
    }

    public static void RefreshBillingCalculation(
      IClientContext context,
      out bool isBillingModelEnabled)
    {
      ClosedLoanBillingInfo closedLoanBillingInfo = (ClosedLoanBillingInfo) null;
      isBillingModelEnabled = EnumUtil.ParseEnum<BillingModel>(Company.GetCompanySetting(context, "License", "BillingModel")) == BillingModel.ClosedLoan;
      if (!isBillingModelEnabled)
        return;
      using (LicenseService licenseService = new LicenseService(EnConfigurationSettings.AppSettings["JedServicesUrl"]))
        closedLoanBillingInfo = licenseService.GetClosedLoanBillingInfo(context.ClientID);
      if (closedLoanBillingInfo != null)
      {
        Company.SetCompanySetting(context, "INTERNAL", new Dictionary<string, string>()
        {
          {
            "BillingCategoryCalculation",
            closedLoanBillingInfo.BillingCategoryCalculation
          },
          {
            "ClosingDateCalculation",
            closedLoanBillingInfo.ClosingDateCalculation
          }
        });
      }
      else
      {
        if (!string.IsNullOrEmpty(Company.GetCompanySetting(context, "INTERNAL", "BillingCategoryCalculation")))
          Company.DeleteCompanySetting("INTERNAL", "BillingCategoryCalculation");
        if (string.IsNullOrEmpty(Company.GetCompanySetting(context, "INTERNAL", "ClosingDateCalculation")))
          return;
        Company.DeleteCompanySetting("INTERNAL", "ClosingDateCalculation");
      }
    }
  }
}
