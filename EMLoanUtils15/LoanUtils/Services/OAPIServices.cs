// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.OAPIServices
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.WebServices.LoanUtils;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Services
{
  public class OAPIServices
  {
    public static string className = nameof (OAPIServices);
    private static readonly string sw = Tracing.SwEFolder;

    public static OAPIServices.AccessToken GetAccessToken(
      string instanceName,
      SessionObjects sessionObjects)
    {
      return OAPIServices.GetAccessToken(instanceName, sessionObjects.SessionID, string.Empty, sessionObjects?.StartupInfo?.ServiceUrls?.EPackageServiceUrl);
    }

    public static OAPIServices.AccessToken GetAccessToken(
      string instanceName,
      string sessionID,
      string scope,
      string ePackageServiceUrl)
    {
      try
      {
        using (ePackage ePackage = new ePackage((string) null, (string) null, ePackageServiceUrl))
          return JsonConvert.DeserializeObject<OAPIServices.AccessToken>(ePackage.GetOAPIAccessToken(instanceName + "_" + sessionID, scope));
      }
      catch (Exception ex)
      {
        string className = nameof (OAPIServices);
        string msg = "Error in ePackage service all to get Access Token. Ex: " + (object) ex;
        Tracing.Log(OAPIServices.sw, TraceLevel.Error, className, msg);
        throw ex;
      }
    }

    public class AccessToken
    {
      public string access_token { get; set; }

      public string token_type { get; set; }

      public string host_name { get; set; }
    }

    public class OpenAPIServiceError
    {
      public string error_description { get; set; }

      public string error { get; set; }
    }
  }
}
