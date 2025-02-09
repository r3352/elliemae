// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PipelineSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public static class PipelineSettings
  {
    private static int minRefreshInterval = -1;

    public static event EventHandler SettingsChange;

    public static int MinimumRefreshInterval
    {
      get
      {
        if (PipelineSettings.minRefreshInterval == -1)
          PipelineSettings.minRefreshInterval = (int) Session.GetComponentSetting("RateLockRefreshInterval");
        return PipelineSettings.minRefreshInterval;
      }
    }

    public static int RefreshInterval
    {
      get
      {
        if (!Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_PipelineAutoRefresh))
          return -1;
        int val1 = -1;
        int result;
        if (int.TryParse(Session.GetPrivateProfileString("Pipeline.RefreshInterval"), out result))
          val1 = result;
        return val1 <= 0 ? -1 : Math.Max(val1, PipelineSettings.MinimumRefreshInterval);
      }
      set
      {
        Session.WritePrivateProfileString("Pipeline.RefreshInterval", value.ToString());
        PipelineSettings.onSettingsChange();
      }
    }

    public static bool AutoRefreshEnabled => PipelineSettings.RefreshInterval > 0;

    private static void onSettingsChange()
    {
      if (PipelineSettings.SettingsChange == null)
        return;
      PipelineSettings.SettingsChange((object) null, EventArgs.Empty);
    }
  }
}
