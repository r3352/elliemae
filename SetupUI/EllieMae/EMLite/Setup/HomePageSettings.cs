// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.HomePageSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HomePage;
using EllieMae.EMLite.RemotingServices;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public static class HomePageSettings
  {
    private const string className = "HomePageSettings";
    private static readonly string sw = Tracing.SwEpass;

    public static Hashtable GetModuleSettings(
      Sessions.Session session,
      string personaName,
      int personaID,
      out int maxAllowed)
    {
      maxAllowed = 0;
      return HomePageService.GetModuleSettings(session.CompanyInfo.ClientID, session?.SessionObjects?.StartupInfo?.ServiceUrls?.DataServicesUrl, personaName, personaID, out maxAllowed, HomePageControl.AuthId);
    }

    public static void SaveModuleSettings(
      Sessions.Session session,
      string persona,
      int personaId,
      Hashtable htModuleList)
    {
      HomePageService.SaveModuleSettings(session.CompanyInfo.ClientID, session?.SessionObjects?.StartupInfo?.ServiceUrls?.DataServicesUrl, persona, personaId, htModuleList, HomePageControl.AuthId);
    }

    private static bool isInteger(string val)
    {
      try
      {
        int.Parse(val);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
