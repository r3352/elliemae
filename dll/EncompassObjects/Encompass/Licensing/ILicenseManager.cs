// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Licensing.ILicenseManager
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Licensing
{
  [Guid("ACAD61CF-BF77-4647-879E-8BF858FB4BE4")]
  public interface ILicenseManager
  {
    void GenerateLicense(string licenseKey);

    void RefreshLicense();

    bool LicenseKeyExists();

    bool ValidateLicense(bool autoRefresh);

    string GetEncompassVersion(bool includeHotfixLevel);
  }
}
