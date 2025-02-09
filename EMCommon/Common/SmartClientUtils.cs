// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.SmartClientUtils
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.SmartClient;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.AsmResolver.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class SmartClientUtils
  {
    private const string className = "SmartClientUtils";
    private const string smartClientServiceUri = "/EncompassSCWS/SmartClientService.asmx";
    public static readonly string DefaultSmartClientServiceServer;
    public static string DefaultSmartClientServiceUrl;
    private static string regRoot = "Software\\Ellie Mae\\SmartClient\\" + Application.StartupPath.Replace("\\", "/");
    private const int DaysToSyncLoginDetails = 30;
    private const string LastLoginUpdatedDate = "LastLoginUpdatedDate";
    public static readonly string SCTestCIDSuffix = "-Test";
    private static int? _bypassProxy = new int?();
    private static string regKeyPath = "Software\\Ellie Mae\\SmartClient\\" + Application.StartupPath.Replace("\\", "/");
    private static Dictionary<string, int> settings = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private static int _useReaderWriterLockSlim = -1;

    static SmartClientUtils()
    {
      WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
      SmartClientUtils.DefaultSmartClientServiceServer = EnConfigurationSettings.AppSettings["SmartClientServiceUrl"];
      if (string.IsNullOrWhiteSpace(SmartClientUtils.DefaultSmartClientServiceServer) || !Uri.IsWellFormedUriString(SmartClientUtils.DefaultSmartClientServiceServer, UriKind.Absolute))
        SmartClientUtils.DefaultSmartClientServiceServer = AssemblyResolver.AuthServerURL;
      if (string.IsNullOrWhiteSpace(SmartClientUtils.DefaultSmartClientServiceServer) || !Uri.IsWellFormedUriString(SmartClientUtils.DefaultSmartClientServiceServer, UriKind.Absolute))
        SmartClientUtils.DefaultSmartClientServiceServer = "https://hosted.elliemae.com";
      SmartClientUtils.DefaultSmartClientServiceUrl = SmartClientUtils.GetSmartClientServiceUrl(SmartClientUtils.DefaultSmartClientServiceServer);
    }

    public static int GetBypassProxy(string cid, string appName)
    {
      if (!SmartClientUtils._bypassProxy.HasValue)
      {
        try
        {
          string attribute = SmartClientUtils.GetAttribute(cid, appName, "BypassProxy");
          SmartClientUtils._bypassProxy = !string.IsNullOrWhiteSpace(attribute) ? new int?(Convert.ToInt32(attribute)) : new int?(-2);
        }
        catch (Exception ex)
        {
          try
          {
            Tracing.Log(true, "Warning", nameof (SmartClientUtils), "Error getting the \"BypassProxy\" setting: " + ex.Message);
          }
          catch
          {
          }
        }
      }
      return SmartClientUtils._bypassProxy.HasValue ? SmartClientUtils._bypassProxy.Value : -2;
    }

    public static string GetAttribute(string clientID, string appName, string attrName)
    {
      return SmartClientUtils.GetAttribute(clientID, "", appName, attrName);
    }

    public static string GetAttribute(
      string clientID,
      string appSuiteName,
      string appName,
      string attrName)
    {
      for (int index = 0; index < 2; ++index)
      {
        try
        {
          return new SmartClientService(SmartClientUtils.DefaultSmartClientServiceUrl).GetAttribute(clientID, appSuiteName, appName, attrName);
        }
        catch (Exception ex)
        {
          if (ex is WebException && SmartClientUtils.DefaultSmartClientServiceUrl.StartsWith("https://"))
          {
            SmartClientUtils.DefaultSmartClientServiceUrl = SmartClientUtils.DefaultSmartClientServiceUrl.Replace("https://", "http://");
          }
          else
          {
            Tracing.Log(true, "Error", nameof (SmartClientUtils), "Error getting attribute '" + attrName + "': " + ex.Message);
            Tracing.Log(true, "Error", nameof (GetAttribute), index.ToString() + ": " + SmartClientUtils.DefaultSmartClientServiceUrl + " | " + clientID + " | " + appSuiteName + " | " + appName + " | " + attrName);
            break;
          }
        }
      }
      return (string) null;
    }

    public static string GetSmartClientServiceUrl(string url)
    {
      if (string.IsNullOrWhiteSpace(url))
        url = SmartClientUtils.DefaultSmartClientServiceServer;
      int length = url.IndexOf("/EncompassSCWS", StringComparison.CurrentCultureIgnoreCase);
      if (length > 0)
        url = url.Substring(0, length);
      return url + "/EncompassSCWS/SmartClientService.asmx";
    }

    public static bool UpsertAttribute(
      string instanceId,
      string attrName,
      string attrValue,
      string lastModifiedUserId)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(instanceId) || string.IsNullOrWhiteSpace(attrValue))
          return false;
        new SmartClientService(SmartClientUtils.DefaultSmartClientServiceUrl).UpsertScAttributes(instanceId, (string) null, (string) null, attrName, attrValue, (string) null, lastModifiedUserId);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "Error", nameof (SmartClientUtils), "Error while UpsertingAttribute : " + ex.StackTrace);
        throw ex;
      }
    }

    public static void HandleVersionMismatch(string clientVersion, string serverVersion)
    {
      bool flag = false;
      string encompassInfo = SmartClientUtils.getEncompassInfo(AssemblyResolver.SCClientID, "Version");
      if (encompassInfo != null && encompassInfo.StartsWith("@"))
        flag = true;
      if (!flag || clientVersion != encompassInfo)
      {
        if (new Version(serverVersion) <= new Version("6.8.0.0"))
        {
          int num1 = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "Encompass client version (" + clientVersion + ") does not match the server version (" + serverVersion + "). Encompass will terminate.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          SmartClientUtils.setEncompassInfo(AssemblyResolver.SCClientID, "Version", "@" + serverVersion);
          int num2 = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "Encompass client version (" + clientVersion + ") does not match the server version (" + serverVersion + "). Please restart Encompass to download the correct version.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      else
      {
        int num3 = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "Encompass client version (" + clientVersion + ") does not match the server version (" + serverVersion + "). Encompass will terminate. Please run RemoveUAC.exe first and then restart Encompass.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      SmartClientUtils.deleteRegistrySH2SC();
      Process.GetCurrentProcess().Kill();
    }

    public static string GetSmartClientAttribute(string attrName)
    {
      return SmartClientUtils.getSmartClientSetting("Attributes", attrName);
    }

    public static string GetSmartClientSetting(string valueName)
    {
      return SmartClientUtils.getSmartClientSetting((string) null, valueName);
    }

    public static bool IsUpdateClientInfo2Required()
    {
      try
      {
        DateTime dateTime1 = DateTime.MinValue;
        string loginUpdatedDate = SmartClientUtils.getLastLoginUpdatedDate();
        DateTime dateTime2;
        if (!string.IsNullOrWhiteSpace(loginUpdatedDate))
        {
          dateTime2 = Convert.ToDateTime(loginUpdatedDate);
          dateTime1 = dateTime2.Date;
        }
        if (dateTime1 != DateTime.MinValue && DateTime.Now > dateTime1.AddDays(30.0))
        {
          dateTime2 = DateTime.Now;
          dateTime2 = dateTime2.Date;
          SmartClientUtils.setLastLoginUpdatedDate(dateTime2.ToString("MM/dd/yyyy"));
          return true;
        }
        if (dateTime1 == DateTime.MinValue)
        {
          dateTime2 = DateTime.Now;
          dateTime2 = dateTime2.AddDays((double) (new Random().Next(1, 30) * -1)).Date;
          SmartClientUtils.setLastLoginUpdatedDate(dateTime2.ToString("MM/dd/yyyy"));
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "Warning", nameof (SmartClientUtils), "Error Fetching LastLoginUpdatedDate: " + ex.Message);
      }
      return false;
    }

    public static Tuple<string, string> getNetFxInfo()
    {
      Tuple<string, string> netFxInfo = (Tuple<string, string>) null;
      try
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full"))
        {
          if (registryKey != null)
          {
            int num = (int) registryKey.GetValue("Release");
            string str = registryKey.GetValue("Version") as string;
            netFxInfo = new Tuple<string, string>(string.Concat((object) num), str ?? "");
          }
        }
      }
      catch
      {
        netFxInfo = new Tuple<string, string>("", "");
      }
      return netFxInfo;
    }

    public static void UpdateClientInfo(
      ClientInfo sessionClientInfo,
      string scClientID,
      string serverVersion,
      string server,
      bool showError)
    {
      Task.Run((Action) (() =>
      {
        if ((server ?? "").Trim() == "")
          server = "(local)";
        bool isSelfHosted = true;
        string encClientId = SmartClientUtils.useridToEncClientID(scClientID);
        if (sessionClientInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        {
          isSelfHosted = false;
          if (encClientId.StartsWith("30") && encClientId.Length == 10)
          {
            string text = "You are trying to connect to an Encompass Anywhere (EA) server '" + server + "'. To connect to this server, you need to enter your SmartClient ID. Please check with your system administrator for your SmartClient ID.";
            if (scClientID != "3012345678")
              text = "Invalid SmartClient ID '" + scClientID + "': " + text;
            int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            SmartClientUtils.deleteRegistryCredentials();
            Process.GetCurrentProcess().Kill();
          }
        }
        string clientId = sessionClientInfo.ClientID;
        SmartClientUtils.getEncompassInfo(scClientID, "Version");
        SmartClientUtils.setEncompassInfo(scClientID, "Version", serverVersion);
        string password = sessionClientInfo.Password;
        string encompassSystemId = sessionClientInfo.EncompassSystemID;
        string sqlDbId = sessionClientInfo.SqlDbID;
        string encSystemID = XT.ESB64(encompassSystemId, KB.SC64);
        char edition = 'U';
        if (sessionClientInfo.EncompassEdition == EncompassEdition.Banker)
          edition = 'B';
        else if (sessionClientInfo.EncompassEdition == EncompassEdition.Broker)
          edition = 'O';
        else if (sessionClientInfo.EncompassEdition == EncompassEdition.None)
          edition = 'N';
        bool settingAutoUpdate = sessionClientInfo.IsLegacySettingAutoUpdate;
        SmartClientService smartClientService = new SmartClientService(SmartClientUtils.DefaultSmartClientServiceUrl);
        NameValuePair[] sessionInfoValuePair = sessionClientInfo.SessionInfoValuePair;
        ReturnResult returnResult = (ReturnResult) null;
        try
        {
          try
          {
            returnResult = smartClientService.UpdateClientInfo2(clientId, password, encSystemID, sqlDbId, serverVersion, edition, server, settingAutoUpdate, isSelfHosted, sessionInfoValuePair);
          }
          catch (Exception ex)
          {
            if (!(ex is SoapException) || !ex.Message.StartsWith("Server did not recognize the value of HTTP Header SOAPAction:"))
              throw ex;
            if (sessionClientInfo.UserId == "admin")
              returnResult = smartClientService.UpdateClientInfo(clientId, password, encSystemID, sqlDbId, serverVersion, edition, server, settingAutoUpdate, isSelfHosted, sessionInfoValuePair);
          }
          if (returnResult == null)
          {
            Tracing.Log(EnConfigurationSettings.GlobalSettings.Debug, "Warning", nameof (SmartClientUtils), "SmartClientService.UpdateClientInfo() returns null");
            return;
          }
          if (!isSelfHosted)
            return;
        }
        catch (Exception ex)
        {
          string str = "Error updating self-hosted client info: " + ex.Message;
          Tracing.Log(true, "Warning", nameof (SmartClientUtils), str);
          if (!showError)
            return;
          int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        if (returnResult.ReturnCode == ReturnCode.Success)
        {
          Tracing.Log(EnConfigurationSettings.GlobalSettings.Debug, "Info", nameof (SmartClientUtils), "Successfully updated client info.");
          string[] strArray = returnResult.Description.Split(new char[2]
          {
            ',',
            ' '
          }, StringSplitOptions.RemoveEmptyEntries);
          if (strArray == null || strArray.Length == 0)
            return;
          foreach (string strA in strArray)
          {
            if (string.Compare(strA, scClientID, true) == 0)
              return;
          }
          Tracing.Log(EnConfigurationSettings.GlobalSettings.Debug, "Info", nameof (SmartClientUtils), "SCID = " + scClientID + "; valid SCIDs are " + string.Join(", ", strArray));
          SmartClientUtils.deleteRegistryCredentials();
          SmartClientUtils.insertRegistrySmartClientIDs(strArray);
          SmartClientUtils.deleteRegistryLastSmartClientID();
          if (SmartClientUtils.GetSmartClientSetting("AutoSignOn") == "1" && SmartClientUtils.GetSmartClientAttribute("AutoCorrectSmartClientID") == "1")
          {
            SmartClientUtils.setRegistryCredentials(strArray[0], strArray[0]);
          }
          else
          {
            bool flag = false;
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\SmartClient\\Encompass"))
            {
              if (registryKey != null)
                flag = string.Concat(registryKey.GetValue("SH2SC")).Trim() == "1";
            }
            if (flag && scClientID == "3012345678")
              return;
            string text;
            if (strArray.Length > 1 && SmartClientUtils.useAllSmartClientIDs(strArray))
              text = "The valid Encompass SmartClient IDs for Encompass server '" + server + "' are '" + string.Join(", ", strArray) + "'. Please restart Encompass using a valid SmartClient ID.";
            else
              text = "The valid Encompass SmartClient ID for Encompass server '" + server + "' is '" + strArray[0] + "'. Please restart Encompass using the valid SmartClient ID.";
            if (scClientID != "3012345678")
              text = "Invalid SmartClient ID '" + scClientID + "': " + text;
            int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            Process.GetCurrentProcess().Kill();
          }
        }
        else
        {
          string str = "Error updating self-hosted client info. Please check your company password or call Ellie Mae support.\r\n\r\n" + returnResult.Description;
          Tracing.Log(true, "Warning", nameof (SmartClientUtils), str);
          if (!showError)
            return;
          int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }));
    }

    public static bool UpsertAttribute(
      string instanceId,
      string attrName,
      string attrValue,
      string lastModifiedUserId,
      string headerToken)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(instanceId) || string.IsNullOrWhiteSpace(attrValue))
          return false;
        SmartClientService smartClientService = new SmartClientService(SmartClientUtils.DefaultSmartClientServiceUrl)
        {
          ServiceHeaderValue = new ServiceHeader()
        };
        smartClientService.ServiceHeaderValue.AccessToken = headerToken;
        smartClientService.UpsertScAttributes(instanceId, (string) null, (string) null, attrName, attrValue, (string) null, lastModifiedUserId);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "Error", nameof (SmartClientUtils), "Error while UpsertingAttribute : " + ex.StackTrace);
        throw ex;
      }
    }

    private static void setRegistryCredentials(string scid, string password)
    {
      using (RegistryKey subKey = BasicUtils.GetRegistryHive((string) null).CreateSubKey(SmartClientUtils.regKeyPath))
        subKey.SetValue("Credentials", (object) XT.ESB64(scid + "\n" + password, KB.SC64));
    }

    private static string macAddress
    {
      get
      {
        string macAddress = "";
        try
        {
          if (SystemUtil.MacAddresses != null)
          {
            if (SystemUtil.MacAddresses.Length != 0)
              macAddress = (SystemUtil.MacAddresses[0] ?? "").Trim();
          }
        }
        catch
        {
        }
        return macAddress;
      }
    }

    public static List<ScAttribute> GetAttributesByName(string instanceId, string attrName)
    {
      if (string.IsNullOrWhiteSpace(instanceId) || string.IsNullOrWhiteSpace(attrName))
        return (List<ScAttribute>) null;
      ScAttribute[] attributesByName = new SmartClientService(SmartClientUtils.DefaultSmartClientServiceUrl).GetAttributesByName(instanceId, attrName);
      return attributesByName == null ? (List<ScAttribute>) null : ((IEnumerable<ScAttribute>) attributesByName).ToList<ScAttribute>();
    }

    private static int getSetting(
      string customerID,
      string settingName,
      int defaultValue,
      int unsetValue,
      int maxErrorValue)
    {
      string key = customerID + "|" + settingName;
      lock (SmartClientUtils.settings)
      {
        try
        {
          if (SmartClientUtils.settings.ContainsKey(key) && SmartClientUtils.settings[key] != unsetValue)
          {
            if (SmartClientUtils.settings[key] > maxErrorValue)
              goto label_6;
          }
          string attribute = SmartClientUtils.GetAttribute(customerID, "EncompassServer", settingName);
          SmartClientUtils.settings[key] = !string.IsNullOrWhiteSpace(attribute) ? Convert.ToInt32(attribute) : defaultValue;
        }
        catch
        {
          SmartClientUtils.settings[key] = maxErrorValue;
        }
label_6:
        return SmartClientUtils.settings[key];
      }
    }

    private static void removeSetting(string customerID, string settingName)
    {
      string key = customerID + "|" + settingName;
      lock (SmartClientUtils.settings)
      {
        try
        {
          if (!SmartClientUtils.settings.ContainsKey(key))
            return;
          SmartClientUtils.settings.Remove(key);
        }
        catch
        {
        }
      }
    }

    private static string getLastLoginUpdatedDate()
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(SmartClientUtils.regRoot))
      {
        if (registryKey != null)
          return (string) registryKey.GetValue("LastLoginUpdatedDate");
      }
      return (string) null;
    }

    private static void setLastLoginUpdatedDate(string attrvalue)
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(SmartClientUtils.regRoot, true))
        registryKey?.SetValue("LastLoginUpdatedDate", (object) attrvalue, RegistryValueKind.String);
    }

    private static string getEncompassInfo(string scClientID, string attrName)
    {
      string name = SmartClientUtils.regRoot + "\\Encompass\\" + scClientID;
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(name))
      {
        if (registryKey != null)
          return (string) registryKey.GetValue(attrName);
      }
      return (string) null;
    }

    private static void setEncompassInfo(string scClientID, string attrName, string value)
    {
      string subkey = SmartClientUtils.regRoot + "\\Encompass\\" + scClientID;
      using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey(subkey))
        subKey.SetValue(attrName, (object) value);
    }

    private static void deleteRegistrySH2SC()
    {
      try
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\SmartClient\\Encompass", true))
          registryKey?.DeleteValue("SH2SC", false);
      }
      catch (Exception ex)
      {
        Tracing.Log(EnConfigurationSettings.GlobalSettings.Debug, "Warning", nameof (SmartClientUtils), "Error deleting registry entry SH2SC: " + ex.Message);
      }
    }

    private static string getSmartClientSetting(string regKeySubPath, string valueName)
    {
      string name = SmartClientUtils.regRoot;
      if (regKeySubPath != null)
        name = SmartClientUtils.regRoot + "\\" + regKeySubPath;
      using (RegistryKey registryKey = BasicUtils.GetRegistryHive((string) null).OpenSubKey(name))
      {
        if (registryKey != null)
          return (string) registryKey.GetValue(valueName);
      }
      return (string) null;
    }

    private static void deleteRegistryCredentials()
    {
      using (RegistryKey registryKey = BasicUtils.GetRegistryHive((string) null).OpenSubKey(SmartClientUtils.regKeyPath, true))
        registryKey?.DeleteValue("Credentials", false);
    }

    private static bool useAllSmartClientIDs(string[] scIDs)
    {
      bool flag = false;
      foreach (string scId in scIDs)
      {
        if (scId.ToLower().IndexOf(scIDs[0].ToLower()) < 0)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private static void insertRegistrySmartClientIDs(string[] clientIDs)
    {
      if (clientIDs == null || clientIDs.Length == 0)
        return;
      List<string> stringList = new List<string>();
      if (SmartClientUtils.useAllSmartClientIDs(clientIDs))
        stringList.AddRange((IEnumerable<string>) clientIDs);
      else
        stringList.Add(clientIDs[0]);
      string[] registrySmartClientIds = SmartClientUtils.getRegistrySmartClientIDs();
      if (registrySmartClientIds != null && registrySmartClientIds.Length != 0)
      {
        foreach (string strB in registrySmartClientIds)
        {
          bool flag = false;
          foreach (string strA in stringList)
          {
            if (string.Compare(strA, strB, true) == 0)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            stringList.Add(strB);
        }
      }
      SmartClientUtils.setRegistrySmartClientIDs(stringList.ToArray());
    }

    private static void deleteRegistryLastSmartClientID()
    {
      try
      {
        using (RegistryKey registryKey = BasicUtils.GetRegistryHive((string) null).OpenSubKey(SmartClientUtils.regKeyPath, true))
          registryKey?.DeleteValue("LastSmartClientID", false);
      }
      catch (Exception ex)
      {
        Tracing.Log(EnConfigurationSettings.GlobalSettings.Debug, "Warning", nameof (SmartClientUtils), "Error deleting registry entry LastSmartClientID: " + ex.Message);
      }
    }

    private static string[] getRegistrySmartClientIDs()
    {
      List<string> stringList = new List<string>();
      try
      {
        using (RegistryKey registryKey = BasicUtils.GetRegistryHive((string) null).OpenSubKey(SmartClientUtils.regKeyPath))
        {
          if (registryKey == null)
            return (string[]) null;
          string str = (string) registryKey.GetValue("SmartClientIDs");
          if (str != null)
          {
            string[] collection = str.Split(new char[3]
            {
              ',',
              ';',
              ' '
            }, StringSplitOptions.RemoveEmptyEntries);
            stringList.AddRange((IEnumerable<string>) collection);
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error getting client IDs from registry: " + ex.Message, "Encompass SmartClient");
      }
      return stringList.ToArray();
    }

    private static void setRegistrySmartClientIDs(string[] clientIDs)
    {
      if (clientIDs == null)
        return;
      if (clientIDs.Length == 0)
        return;
      try
      {
        using (RegistryKey subKey = BasicUtils.GetRegistryHive((string) null).CreateSubKey(SmartClientUtils.regKeyPath))
        {
          string str = string.Join(", ", clientIDs);
          subKey.SetValue("SmartClientIDs", (object) str);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error setting client IDs in registry: " + ex.Message, "Encompass SmartClient");
      }
    }

    private static string useridToEncClientID(string userid)
    {
      userid = SmartClientUtils.removeUseridSuffixes(userid);
      string encClientId = "";
      string str = "";
      for (int index = 0; index < userid.Length; ++index)
      {
        if (char.IsDigit(userid[index]))
        {
          str += userid[index].ToString();
        }
        else
        {
          if (str.Length > encClientId.Length)
            encClientId = str;
          str = "";
        }
      }
      if (str.Length > encClientId.Length)
        encClientId = str;
      return encClientId;
    }

    private static string removeUseridSuffixes(string userid)
    {
      int length1 = userid.LastIndexOf("\\");
      if (length1 > 0)
        userid = userid.Substring(0, length1);
      int length2 = userid.LastIndexOf("-");
      if (length2 > 0)
        userid = userid.Substring(0, length2);
      return userid;
    }

    [CLSCompliant(false)]
    public static int _UseReaderWriterLockSlim => SmartClientUtils._useReaderWriterLockSlim;

    public static int UseReaderWriterLockSlim_
    {
      get
      {
        if (SmartClientUtils._useReaderWriterLockSlim < 0)
          SmartClientUtils._useReaderWriterLockSlim = SmartClientUtils.getSetting(SmartClientUtils.macAddress, "UseReaderWriterLockSlim", 0, -1, -2);
        return SmartClientUtils._useReaderWriterLockSlim;
      }
    }

    public static bool UseReaderWriterLockSlim
    {
      get
      {
        return CacheStoreConfiguration.CacheStoreType == CacheStoreSource.InProcess && SmartClientUtils.UseReaderWriterLockSlim_ > 0;
      }
    }

    public static bool LockSlimNoRecursion
    {
      get
      {
        return CacheStoreConfiguration.CacheStoreType == CacheStoreSource.InProcess && SmartClientUtils.UseReaderWriterLockSlim_ > 1;
      }
    }

    public static bool LockSlimNoRecursionStandardFields
    {
      get
      {
        return CacheStoreConfiguration.CacheStoreType == CacheStoreSource.InProcess && SmartClientUtils.UseReaderWriterLockSlim_ > 2;
      }
    }

    public static void ResetUseReaderWriterLockSlim()
    {
      SmartClientUtils._useReaderWriterLockSlim = -1;
      SmartClientUtils.removeSetting(SmartClientUtils.macAddress, "UseReaderWriterLockSlim");
    }
  }
}
