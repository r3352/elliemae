// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.BrowserControls.BrowserFactory
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using EllieMae.EMLite.Common;
using EllieMae.Encompass.AsmResolver;
using Microsoft.Win32;
using System.Diagnostics;

#nullable disable
namespace Elli.Web.Host.BrowserControls
{
  public static class BrowserFactory
  {
    private static EncWebFormBrowserControl _encWebFormBrowserControl;
    private const string className = "BrowserFactory";
    private const string ChromiumWebBrowserAttr = "UseChromiumForWebBrowser";
    private const string ChromiumGnetAttr = "UseChromiumForGnet";
    private const string ChromiumIFBAttr = "UseChromiumForIFB";
    private static readonly string sw = Tracing.SwThinThick;

    public static EncWebFormBrowserControl GetWebBrowserInstance()
    {
      BrowserFactory._encWebFormBrowserControl = !BrowserFactory.IsChromiumForWebbrowserEnabled ? (EncWebFormBrowserControl) new EncWebBrowser() : (EncWebFormBrowserControl) new WebHost();
      Tracing.Log(BrowserFactory.sw, TraceLevel.Verbose, nameof (BrowserFactory), "Type returned for GetWebBrowserInstance() - " + BrowserFactory._encWebFormBrowserControl.GetType().FullName);
      return BrowserFactory._encWebFormBrowserControl;
    }

    public static bool IsChromiumForWebbrowserEnabled
    {
      get
      {
        bool webbrowserEnabled = true;
        if ((string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "UseChromiumForWebBrowser") != null)
          webbrowserEnabled = (string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "UseChromiumForWebBrowser") == "1";
        else if (!string.IsNullOrEmpty(AssemblyResolver.GetScAttribute("UseChromiumForWebBrowser")))
          webbrowserEnabled = AssemblyResolver.GetScAttribute("UseChromiumForWebBrowser") == "1";
        return webbrowserEnabled;
      }
    }
  }
}
