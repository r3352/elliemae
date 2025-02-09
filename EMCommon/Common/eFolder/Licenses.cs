// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.eFolder.Licenses
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using GdPicture14;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Common.eFolder
{
  public static class Licenses
  {
    private const string className = "Licenses";
    private static readonly string sw = Tracing.SwEFolder;

    public static void RegisterGDPicture()
    {
      Tracing.Log(Licenses.sw, TraceLevel.Verbose, nameof (Licenses), "Creating GdPicture14.LicenseManager");
      LicenseManager licenseManager = new LicenseManager();
      Tracing.Log(Licenses.sw, TraceLevel.Verbose, nameof (Licenses), "Calling LicenseManager.RegisterKEY");
      licenseManager.RegisterKEY("132910996703047601621131550736870");
      licenseManager.RegisterKEY("726363989913125671618131565946732");
      licenseManager.RegisterKEY("903227975943349671221111282831158");
      licenseManager.RegisterKEY("806389477733398771111121289599653");
      licenseManager.RegisterKEY("13294279592918777161411631614724895290");
      licenseManager.RegisterKEY("72633989752704974121816499075229569721");
      licenseManager.RegisterKEY("13294279592918777161411631614724895290");
    }
  }
}
