// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.WebLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class WebLink
  {
    public static string GetJumpURL(string pageName, Sessions.Session session)
    {
      if (!pageName.ToLower().StartsWith("http"))
        pageName = "https://encompass.elliemae.com/Go/" + pageName;
      pageName = pageName.IndexOf("?") >= 0 ? pageName + "&" : pageName + "?";
      pageName = pageName + "cid=" + session.CompanyInfo.ClientID + "&uid=" + session.UserID + "&ver=" + VersionInformation.CurrentVersion.OriginalVersion.NormalizedVersion + "&ed=" + (object) session.EncompassEdition;
      if (EnConfigurationSettings.GlobalSettings.RuntimeEnvironment != RuntimeEnvironment.Default)
        pageName = pageName + "&env=" + (object) EnConfigurationSettings.GlobalSettings.RuntimeEnvironment;
      return pageName;
    }
  }
}
