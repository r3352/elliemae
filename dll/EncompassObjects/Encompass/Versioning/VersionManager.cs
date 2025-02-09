// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Versioning.VersionManager
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.Encompass.Licensing;
using System;
using System.Runtime.InteropServices;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.Encompass.Versioning
{
  [ComVisible(false)]
  public class VersionManager
  {
    public string CurrentVersion => VersionInformation.CurrentVersion.Version.ToString();

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
