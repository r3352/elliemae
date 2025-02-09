// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Versioning.VersionManager
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.Encompass.Licensing;
using System;
using System.Runtime.InteropServices;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.Encompass.Versioning
{
  /// <summary>
  /// Provides software versioning and update funtionality. This class is meant for
  /// internal use only.
  /// </summary>
  /// <exclude />
  [ComVisible(false)]
  public class VersionManager
  {
    /// <summary>
    /// Gets the current version of the SDK installed on the computer.
    /// </summary>
    public string CurrentVersion => VersionInformation.CurrentVersion.Version.ToString();

    /// <summary>
    /// Retrieves the URL of an SDK software update for the specified target version.
    /// </summary>
    /// <param name="targetVersion">The Encompass version to which the system
    /// is to be upgraded.</param>
    /// <returns>The URL of the installation package for the specified target
    /// version, if one exists. An empty return value indicates that no update
    /// exists for the specified version or the update is not version compatible
    /// with the currently installed version. An exception is thrown if the
    /// current computer's license does not permit upgrades.</returns>
    public string GetVersionUpgradeURL(string targetVersion)
    {
      LicenseFile currentLicense = new LicenseManager().GetCurrentLicense();
      if (currentLicense == null)
        throw new LicenseException("A valid license is required for this operation");
      using (LicenseService licenseService = new LicenseService(EnConfigurationSettings.AppSettings["JedServicesUrl"]?.ToString()))
      {
        try
        {
          return licenseService.GetUpdate(currentLicense.ClientID, targetVersion, VersionInformation.CurrentVersion.Version.ToString());
        }
        catch (SoapException ex)
        {
          if (ex.Message.IndexOf("--> ") > 0)
            throw new LicenseException(ex.Message.Substring(ex.Message.IndexOf("--> ") + 4), (Exception) ex);
          throw new ApplicationException(ex.Message, (Exception) ex);
        }
      }
    }
  }
}
