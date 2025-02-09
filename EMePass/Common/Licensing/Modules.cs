// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Licensing.Modules
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using Elli.Server.Remoting;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.Licensing
{
  [ComVisible(false)]
  public class Modules
  {
    private const string className = "Modules";
    private static readonly string sw = Tracing.SwEpass;
    private static ReaderWriterLock licenseDownloadLock = new ReaderWriterLock();
    private static ReaderWriterLockSlim licenseDownloadLockSlim = (ReaderWriterLockSlim) null;
    private static string SECONDARYURL = "https://modules.elliemaeservices.com/jedservices/modules.asmx";

    static Modules()
    {
      if (!SmartClientUtils.UseReaderWriterLockSlim)
        return;
      if (SmartClientUtils.LockSlimNoRecursion)
        Modules.licenseDownloadLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
      else
        Modules.licenseDownloadLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
    }

    private Modules()
    {
    }

    public static void ClearUserLicense()
    {
      Modules.saveUserLicense(new UserLicenseInfo(Session.CompanyInfo.ClientID, Session.UserID));
    }

    public static void BeginDownloadUserLicense()
    {
      ThreadPool.QueueUserWorkItem(new WaitCallback(Modules.downloadUserLicenseAsync), (object) null);
    }

    private static void downloadUserLicenseAsync(object notUsed) => Modules.DownloadUserLicense();

    public static void DownloadUserLicense()
    {
      Modules.DownloadUserLicense(Session.DefaultInstance);
    }

    public static void DownloadUserLicense(Sessions.Session session)
    {
      using (Tracing.StartTimer(Modules.sw, nameof (Modules), TraceLevel.Info, "Downloading user module licenses..."))
      {
        Tracing.Log(Modules.sw, nameof (Modules), TraceLevel.Info, "Obtaining exclusive lock for module download...");
        bool readerWriterLockSlim = SmartClientUtils.UseReaderWriterLockSlim;
        if (readerWriterLockSlim)
        {
          if (!Modules.licenseDownloadLockSlim.TryEnterWriteLock(TimeSpan.FromSeconds(60.0)))
            throw new ApplicationException("Timeout expires before the write lock request is granted (002).");
        }
        else
          Modules.licenseDownloadLock.AcquireWriterLock(TimeSpan.FromSeconds(60.0));
        try
        {
          string clientId = session.CompanyInfo.ClientID;
          string userId = session.UserID;
          EllieMae.EMLite.WebServices.ModuleInfo[] enabledModules = (EllieMae.EMLite.WebServices.ModuleInfo[]) null;
          try
          {
            Tracing.Log(Modules.sw, nameof (Modules), TraceLevel.Info, "Creating ModuleService object...");
            using (ModuleService moduleService = new ModuleService(session?.SessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl))
            {
              moduleService.Timeout = 5000;
              Tracing.Log(Modules.sw, nameof (Modules), TraceLevel.Info, "Executing ModuleService request...");
              enabledModules = moduleService.GetUserModules(clientId, userId);
            }
          }
          catch (Exception ex1)
          {
            try
            {
              Tracing.Log(Modules.sw, TraceLevel.Error, nameof (Modules), ex1.Message);
              using (ModuleService moduleService = new ModuleService(session?.SessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl))
              {
                moduleService.Url = Modules.SECONDARYURL;
                moduleService.Timeout = 5000;
                Tracing.Log(Modules.sw, nameof (Modules), TraceLevel.Info, "Using HA Site: Executing ModuleService request...");
                moduleService.GetUserModules(clientId, userId);
                return;
              }
            }
            catch (Exception ex2)
            {
              Tracing.Log(Modules.sw, TraceLevel.Error, nameof (Modules), "Unable to get module info from HA sites." + ex2.Message);
              return;
            }
          }
          Tracing.Log(Modules.sw, nameof (Modules), TraceLevel.Info, "Saving user license information...");
          Modules.saveUserLicense(Modules.createUserLicense(userId, enabledModules, session));
        }
        finally
        {
          if (readerWriterLockSlim)
          {
            if (Modules.licenseDownloadLockSlim.IsWriteLockHeld)
              Modules.licenseDownloadLockSlim.ExitWriteLock();
          }
          else
            Modules.licenseDownloadLock.ReleaseWriterLock();
        }
      }
    }

    public static EncompassModule[] GetClientModules()
    {
      string clientId = Session.CompanyInfo.ClientID;
      using (ModuleService moduleService = new ModuleService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl))
      {
        EllieMae.EMLite.WebServices.ModuleInfo[] clientModules1 = moduleService.GetClientModules(clientId);
        if (clientModules1 == null)
          return (EncompassModule[]) null;
        ArrayList arrayList = new ArrayList();
        foreach (EllieMae.EMLite.WebServices.ModuleInfo moduleInfo in clientModules1)
        {
          try
          {
            EncompassModule encompassModule = (EncompassModule) Enum.Parse(typeof (EncompassModule), moduleInfo.ModuleID, true);
            arrayList.Add((object) encompassModule);
          }
          catch
          {
          }
        }
        EncompassModule[] clientModules2 = (EncompassModule[]) null;
        if (arrayList.Count > 0)
          clientModules2 = (EncompassModule[]) arrayList.ToArray(typeof (EncompassModule));
        return clientModules2;
      }
    }

    public static EncompassModule[] GetUserModules(string userID)
    {
      string clientId = Session.CompanyInfo.ClientID;
      using (ModuleService moduleService = new ModuleService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl))
      {
        EllieMae.EMLite.WebServices.ModuleInfo[] userModules1 = moduleService.GetUserModules(clientId, userID);
        if (userModules1 == null)
          return (EncompassModule[]) null;
        UserLicenseInfo userLicense = Modules.createUserLicense(userID, userModules1, Session.DefaultInstance);
        if (userLicense.LicensedModules.Count == 0)
          return (EncompassModule[]) null;
        EncompassModule[] userModules2 = new EncompassModule[userLicense.LicensedModules.Count];
        for (int index = 0; index < userModules2.Length; ++index)
          userModules2[index] = (EncompassModule) userLicense.LicensedModules[index];
        return userModules2;
      }
    }

    public static ModuleLicense GetModuleLicense(EncompassModule module, Sessions.Session session)
    {
      string clientId = session.CompanyInfo.ClientID;
      using (ModuleService moduleService = new ModuleService(session?.SessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl))
        return moduleService.GetModuleLicense2(clientId, module.ToString());
    }

    public static ModuleLicense GetModuleLicense(EncompassModule module)
    {
      return Modules.GetModuleLicense(module, Session.DefaultInstance);
    }

    public static void EnableModuleUser(EncompassModule module, string userID)
    {
      string clientId = Session.CompanyInfo.ClientID;
      using (ModuleService moduleService = new ModuleService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl))
      {
        EllieMae.EMLite.WebServices.ModuleInfo[] enabledModules = moduleService.EnableModuleUser(clientId, module.ToString(), userID);
        Modules.saveUserLicense(Modules.createUserLicense(userID, enabledModules, Session.DefaultInstance));
      }
    }

    public static void DisableModuleUser(EncompassModule module, string userID)
    {
      string clientId = Session.CompanyInfo.ClientID;
      using (ModuleService moduleService = new ModuleService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl))
      {
        EllieMae.EMLite.WebServices.ModuleInfo[] enabledModules = moduleService.DisableModuleUser(clientId, module.ToString(), userID);
        Modules.saveUserLicense(Modules.createUserLicense(userID, enabledModules, Session.DefaultInstance));
      }
    }

    public static void UpdateModuleUsers(
      EncompassModule module,
      string[] enabledList,
      string[] disabledList)
    {
      string clientId = Session.CompanyInfo.ClientID;
      using (ModuleService moduleService = new ModuleService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl))
      {
        List<ModuleUser> moduleUserList = new List<ModuleUser>();
        foreach (string enabled in enabledList)
          moduleUserList.Add(new ModuleUser()
          {
            UserID = enabled,
            Disabled = false
          });
        foreach (string disabled in disabledList)
          moduleUserList.Add(new ModuleUser()
          {
            UserID = disabled,
            Disabled = true
          });
        moduleService.UpdateModuleUsers(clientId, module.ToString(), moduleUserList.ToArray());
        foreach (ModuleUser moduleUser in moduleUserList)
        {
          UserLicenseInfo license = Modules.updateUserLicense(moduleUser.UserID, module, !moduleUser.Disabled);
          if (license != null)
            Modules.saveUserLicense(license);
        }
      }
    }

    public static bool IsModuleAvailableForUser(EncompassModule module, bool showDialogs)
    {
      bool readerWriterLockSlim = SmartClientUtils.UseReaderWriterLockSlim;
      if (readerWriterLockSlim)
      {
        if (!Modules.licenseDownloadLockSlim.TryEnterReadLock(TimeSpan.FromSeconds(60.0)))
          throw new ApplicationException("Timeout expires before the read lock request is granted (002).");
      }
      else
        Modules.licenseDownloadLock.AcquireReaderLock(TimeSpan.FromSeconds(60.0));
      try
      {
        if (!showDialogs)
        {
          switch (module)
          {
            case EncompassModule.EDM:
            case EncompassModule.Mavent:
            case EncompassModule.Fulfillment:
            case EncompassModule.ClosingDotCom:
            case EncompassModule.IRS4506T:
              if (EpassLogin.LoginID == null || EpassLogin.LoginID == "0")
                return false;
              break;
          }
          return Session.GetUserLicense().LicensedModules.Contains((int) module);
        }
      }
      finally
      {
        if (readerWriterLockSlim)
        {
          if (Modules.licenseDownloadLockSlim.IsReadLockHeld)
            Modules.licenseDownloadLockSlim.ExitReadLock();
        }
        else
          Modules.licenseDownloadLock.ReleaseReaderLock();
      }
      switch (module)
      {
        case EncompassModule.EDM:
        case EncompassModule.Mavent:
        case EncompassModule.Fulfillment:
        case EncompassModule.ClosingDotCom:
        case EncompassModule.IRS4506T:
          if (!EpassLogin.LoginRequired(true))
            return false;
          if (Session.GetUserLicense().LicensedModules.Contains((int) module))
            return true;
          break;
        default:
          if (Session.GetUserLicense().LicensedModules.Contains((int) module))
            return true;
          if (!EpassLogin.LoginUser(true))
            return false;
          if (Session.GetUserLicense().LicensedModules.Contains((int) module))
            return true;
          break;
      }
      string text;
      switch (module)
      {
        case EncompassModule.EDM:
          text = "To access the Electronic Document Management add-on feature, please contact your system administrator.";
          break;
        case EncompassModule.StatusOnline:
          text = "To access the Status Online add-on feature, please contact your system administrator.";
          break;
        case EncompassModule.Mavent:
          text = "The Compliance Review service is currently not available. Please contact your system administrator.";
          break;
        default:
          text = "To access this add-on feature, please contact your system administrator.";
          break;
      }
      int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    public static EncompassModule IsAnyModuleAvailableForUser(
      EncompassModule[] modules,
      bool showDialogs)
    {
      for (int index = 0; index < modules.Length - 1; ++index)
      {
        if (Modules.IsModuleAvailableForUser(modules[index], false))
          return modules[index];
      }
      return Modules.IsModuleAvailableForUser(modules[modules.Length - 1], showDialogs) ? modules[modules.Length - 1] : EncompassModule.None;
    }

    private static UserLicenseInfo createUserLicense(
      string userID,
      EllieMae.EMLite.WebServices.ModuleInfo[] enabledModules,
      Sessions.Session session)
    {
      UserLicenseInfo userLicense = new UserLicenseInfo(session.CompanyInfo.ClientID, userID);
      if (enabledModules != null)
      {
        foreach (EllieMae.EMLite.WebServices.ModuleInfo enabledModule in enabledModules)
        {
          EncompassModule result;
          if (Enum.TryParse<EncompassModule>(enabledModule.ModuleID, true, out result))
            userLicense.LicensedModules.Add((int) result);
        }
      }
      return userLicense;
    }

    private static UserLicenseInfo updateUserLicense(
      string userID,
      EncompassModule module,
      bool enabled)
    {
      UserLicenseInfo userLicenseInfo;
      try
      {
        userLicenseInfo = !(userID == Session.UserID) ? Session.ConfigurationManager.GetUserLicense(userID) : Session.GetUserLicense();
      }
      catch (Exception ex)
      {
        Tracing.Log(Modules.sw, TraceLevel.Error, nameof (Modules), ex.Message);
        return (UserLicenseInfo) null;
      }
      if (enabled)
        userLicenseInfo.LicensedModules.Add((int) module);
      else
        userLicenseInfo.LicensedModules.Remove((int) module);
      return userLicenseInfo;
    }

    private static void saveUserLicense(UserLicenseInfo license)
    {
      try
      {
        if (license.UserID == Session.UserID)
          Session.UpdateUserLicense(license);
        else
          Session.ConfigurationManager.UpdateUserLicense(license);
      }
      catch (Exception ex)
      {
        Tracing.Log(Modules.sw, TraceLevel.Error, nameof (Modules), ex.Message);
        return;
      }
      if (EnConfigurationSettings.GlobalSettings.InstallationMode != InstallationMode.Local)
        return;
      if (Session.Connection.IsServerInProcess)
        return;
      try
      {
        CompanyInfo companyInfo = Session.CompanyInfo;
        using (InProcConnection inProcConnection = new InProcConnection())
        {
          inProcConnection.OpenTrusted();
          IConfigurationManager configurationManager = (IConfigurationManager) inProcConnection.Session.GetObject("ConfigurationManager");
          if (configurationManager.GetCompanyInfo().ClientID == companyInfo.ClientID)
          {
            try
            {
              configurationManager.UpdateUserLicense(license);
            }
            catch
            {
            }
          }
          inProcConnection.Close();
        }
      }
      catch
      {
      }
    }
  }
}
