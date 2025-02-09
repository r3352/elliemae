// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DeepLinking.DeepLinkLauncher
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.DeepLinking.Context.Contract;
using EllieMae.EMLite.Common.DeepLinking.Contract;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.DeepLinking
{
  public static class DeepLinkLauncher
  {
    private const string _className = "DeepLinkLauncher";

    public static async Task LaunchWebAppInBrowserAsync(
      DeepLinkType deepLinkType,
      IDeepLinkContext deepLinkApplicationContext)
    {
      PerformanceMeter performanceMeter = (PerformanceMeter) null;
      string traceMsg = string.Empty;
      try
      {
        IDeepLink deepLink = DeepLinkFactory.Create(deepLinkType, deepLinkApplicationContext);
        traceMsg = deepLink.GetLog();
        using (performanceMeter = PerformanceMeter.StartNew(deepLink.KPIName, deepLink.KPIDescription, true, false, true, 145, nameof (LaunchWebAppInBrowserAsync), "D:\\ws\\24.3.0.0\\EmLite\\ClientCommon\\DeepLinking\\DeepLinkLauncher.cs"))
        {
          Tracing.Log(Tracing.SwDeepLink, TraceLevel.Info, nameof (DeepLinkLauncher), traceMsg);
          bool flag = true;
          if (deepLink.PreDeepLinkActivity != null)
            flag = deepLink.PreDeepLinkActivity.Execute(deepLinkApplicationContext);
          if (flag)
            await Task.Run((Action) (() => DeepLinkLauncher.launch(DeepLinkLauncher.addAuthCodeToURL(deepLink.URL))));
        }
      }
      catch (Exception ex)
      {
        traceMsg = traceMsg + Environment.NewLine + ex.ToString();
        Tracing.Log(Tracing.SwDeepLink, TraceLevel.Error, nameof (DeepLinkLauncher), traceMsg);
        int num = (int) Utils.Dialog((IWin32Window) null, "DeepLink launcher exception. Please refer logs for more details.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private static void launch(string url)
    {
      if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
        throw new Exception("Invalid URL. Cannot launch web application.");
      Process.Start(url);
    }

    private static string addAuthCodeToURL(string url)
    {
      UriBuilder uriBuilder = new UriBuilder(new Uri(url));
      NameValueCollection queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
      queryString.Add("code", DeepLinkLauncher.getOAuthCode());
      uriBuilder.Query = queryString.ToString();
      return uriBuilder.ToString();
    }

    private static string getOAuthCode()
    {
      string sessionId = Guid.NewGuid().ToString();
      Session.ServerManager.AddServiceSession(sessionId, "loc");
      return new OAuth2(Session.StartupInfo?.OAPIGatewayBaseUri).GenerateGuestApplicationAuthCode(Session.DefaultInstance?.ServerIdentity?.InstanceName, sessionId, Session.StartupInfo?.OAuthEncWebClientId);
    }
  }
}
