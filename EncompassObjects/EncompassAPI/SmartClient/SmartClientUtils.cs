// Decompiled with JetBrains decompiler
// Type: EllieMae.EncompassAPI.SmartClient.SmartClientUtils
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EncompassAPI.SmartClient
{
  /// <summary>SmartClientUtils</summary>
  public class SmartClientUtils
  {
    private const string className = "SmartClientUtils�";
    private const string smartClientServiceUri = "/EncompassSCWS/SmartClientService.asmx�";
    /// <summary>DefaultSmartClientServiceServer</summary>
    public static readonly string DefaultSmartClientServiceServer;
    /// <summary>DefaultSmartClientServiceUrl</summary>
    public static string DefaultSmartClientServiceUrl;
    private static string regRoot = "Software\\Ellie Mae\\SmartClient\\" + Application.StartupPath.Replace("\\", "/");

    static SmartClientUtils()
    {
      WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
      SmartClientUtils.DefaultSmartClientServiceServer = AssemblyResolver.AuthServerURL;
      if (string.IsNullOrWhiteSpace(SmartClientUtils.DefaultSmartClientServiceServer) || !Uri.IsWellFormedUriString(SmartClientUtils.DefaultSmartClientServiceServer, UriKind.Absolute))
        SmartClientUtils.DefaultSmartClientServiceServer = "https://hosted.elliemae.com";
      SmartClientUtils.DefaultSmartClientServiceUrl = SmartClientUtils.GetSmartClientServiceUrl(SmartClientUtils.DefaultSmartClientServiceServer);
    }

    /// <summary>GetAttribute</summary>
    /// <param name="clientID"></param>
    /// <param name="appName"></param>
    /// <param name="attrName"></param>
    /// <returns></returns>
    public static string GetAttribute(string clientID, string appName, string attrName)
    {
      for (int index = 0; index < 2; ++index)
      {
        try
        {
          return new EllieMae.EncompassAPI.WebServices.SmartClientService(SmartClientUtils.DefaultSmartClientServiceUrl).GetAttribute(clientID, "", appName, attrName);
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
            break;
          }
        }
      }
      return (string) null;
    }

    /// <summary>GetSmartClientServiceUrl</summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetSmartClientServiceUrl(string url)
    {
      if ((url ?? "").Trim() == "")
        url = SmartClientUtils.DefaultSmartClientServiceServer;
      int length = url.IndexOf("/EncompassSCWS", StringComparison.CurrentCultureIgnoreCase);
      if (length > 0)
        url = url.Substring(0, length);
      return url + "/EncompassSCWS/SmartClientService.asmx";
    }
  }
}
