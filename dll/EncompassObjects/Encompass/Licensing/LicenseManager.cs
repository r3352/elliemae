// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Licensing.LicenseManager
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.JedLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.Encompass.Licensing
{
  public sealed class LicenseManager : ILicenseManager
  {
    private const string className = "LicenseManager�";
    private static string sw = Tracing.SwCommon;
    private static LicenseManager.LicenseFormatter licenseFormatter;
    private static Dictionary<string, string> clientAuthorizationTokens = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    static LicenseManager()
    {
      a.a("abc0xyx8jfiwkcpo");
      LicenseManager.licenseFormatter = new LicenseManager.LicenseFormatter();
      RSACryptoServiceProvider.UseMachineKeyStore = true;
    }

    public string GetEncompassVersion(bool includeHotfixLevel)
    {
      return includeHotfixLevel ? VersionInformation.CurrentVersion.DisplayVersion.ToString() : VersionInformation.CurrentVersion.Version.ToString();
    }

    public void GenerateLicense(string licenseKey)
    {
      using (LicenseService licenseService = new LicenseService(EnConfigurationSettings.AppSettings["JedServicesUrl"]?.ToString()))
      {
        try
        {
          string[] macAddresses = new string[0];
          ApiRegistrationInfo registrationInfo = licenseService.RegisterInstall2(licenseKey, Environment.MachineName, macAddresses);
          if (registrationInfo == null)
            throw new LicenseException("Registration Service returned invalid response.");
          EnConfigurationSettings.GlobalSettings.SDKLicense = LicenseManager.licenseFormatter.Serialize(new LicenseFile()
          {
            ClientID = registrationInfo.ClientID,
            LicenseKey = licenseKey,
            MacAddresses = macAddresses,
            Version = VersionInformation.CurrentVersion.Version,
            AuthorizeAllSessions = registrationInfo.AutoAuthorizeSessions
          });
        }
        catch (SoapException ex)
        {
          if (ex.Message.IndexOf("--> ") > 0)
            throw new LicenseException(ex.Message.Substring(ex.Message.IndexOf("--> ") + 4), (Exception) ex);
          throw new LicenseException(ex.Message, (Exception) ex);
        }
      }
    }

    public void RefreshLicense()
    {
      this.GenerateLicense((this.GetCurrentLicense() ?? throw new InvalidOperationException("The current machine is not licensed to use this product.")).LicenseKey);
    }

    public bool LicenseKeyExists()
    {
      try
      {
        return this.GetCurrentLicense() != null;
      }
      catch
      {
        return false;
      }
    }

    public bool ValidateLicense(bool autoRefresh)
    {
      try
      {
        return this.GetCurrentLicense() != null;
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "ERROR", nameof (LicenseManager), "Error validating license: " + ex.Message);
        return false;
      }
    }

    internal LicenseFile GetCurrentLicense()
    {
      byte[] sdkLicense = EnConfigurationSettings.GlobalSettings.SDKLicense;
      return sdkLicense == null ? (LicenseFile) null : LicenseManager.licenseFormatter.Deserialize(sdkLicense);
    }

    internal void AuthorizeSession(ISessionStartupInfo sessionInfo)
    {
      if (sessionInfo == null)
        throw new ArgumentNullException(nameof (sessionInfo));
      if (!this.ValidateLicense(false))
        throw new LicenseException("This machine is not licensed to use the Encompass API.");
      LicenseFile currentLicense = this.GetCurrentLicense();
      if (currentLicense.AuthorizeAllSessions)
      {
        Tracing.Log(LicenseManager.sw, nameof (LicenseManager), TraceLevel.Info, "Skipping session authorization due to license setting.");
      }
      else
      {
        Process currentProcess = Process.GetCurrentProcess();
        string hostName = Dns.GetHostName();
        string userName = Environment.UserName;
        string clientId = sessionInfo.CompanyInfo.ClientID;
        string passPhrase = clientId + ";" + string.Join(";", currentLicense.MacAddresses) + ";" + currentProcess.ProcessName;
        lock (LicenseManager.clientAuthorizationTokens)
        {
          if (LicenseManager.clientAuthorizationTokens.ContainsKey(clientId) && AuthenticationUtils.CompareAuthorizationKey(LicenseManager.clientAuthorizationTokens[clientId], passPhrase))
            return;
          string[] commandLineArgs = Environment.GetCommandLineArgs();
          string processName = currentProcess.ProcessName;
          string appCRC = "";
          if (commandLineArgs.Length != 0)
          {
            processName = commandLineArgs[0];
            appCRC = AuthenticationUtils.ComputeCRC(processName);
          }
          try
          {
            using (LicenseService licenseService = new LicenseService(sessionInfo?.ServiceUrls?.JedServicesUrl))
            {
              licenseService.Timeout = 20000;
              LicenseManager.clientAuthorizationTokens[clientId] = licenseService.AuthorizeSession(clientId, currentLicense.LicenseKey, hostName, userName, processName, appCRC, passPhrase);
            }
            if (!LicenseManager.clientAuthorizationTokens.ContainsKey(clientId))
              return;
            AuthenticationUtils.CompareAuthorizationKey(LicenseManager.clientAuthorizationTokens[clientId], passPhrase);
          }
          catch (SoapException ex)
          {
            Tracing.Log(LicenseManager.sw, nameof (LicenseManager), TraceLevel.Error, "Failed to authorize session: " + (object) ex);
            if (ex.Message.ToString().IndexOf("The current application is not authorized") >= 0)
            {
              if (ex.Message.IndexOf("--> ") > 0)
                throw new LicenseException(ex.Message.Substring(ex.Message.IndexOf("--> ") + 4), (Exception) ex);
              throw new LicenseException(ex.Message, (Exception) ex);
            }
            if (ex.Message.ToString().IndexOf("--> System.Security.Cryptography.CryptographicException: CryptoAPI cryptographic service provider (CSP) for this implementation could not be acquired.") < 0)
              return;
            Tracing.Log(LicenseManager.sw, nameof (LicenseManager), TraceLevel.Warning, "Cryptographic exception: " + ex.Message);
          }
          catch (Exception ex)
          {
            Tracing.Log(LicenseManager.sw, nameof (LicenseManager), TraceLevel.Error, "Failed to authorize session: " + (object) ex);
          }
        }
      }
    }

    [StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100ED611F91C2EEA49D628904356176F8405967CF8FD01EE7F2914038B5BA5F70C9EB8BE8489CD4F8E6DC00CE61D5127EE2B7D97AC08EA11B7D33829FFE313EDB7408F67A09F6226F942F86B01311DC7EE0088AA29491178EFEFF12B2826097CE5CAEE7B0DF8E69EE50D07034E8F79FA95CCD98BF9D9615E9EF53D5647217C330B9")]
    private sealed class LicenseFormatter
    {
      private b jed;

      public LicenseFormatter() => this.jed = a.b("abc8yzl5dj3bc9");

      public LicenseFile Deserialize(byte[] data)
      {
        try
        {
          lock (this.jed)
          {
            this.jed.b();
            return LicenseFile.Parse(this.jed.a(data, 0, data.Length));
          }
        }
        catch (Exception ex)
        {
          throw new LicenseException("Error reading machine SDK license", ex);
        }
      }

      public byte[] Serialize(LicenseFile file)
      {
        lock (this.jed)
        {
          this.jed.b();
          return this.jed.b(file.ToString());
        }
      }
    }
  }
}
