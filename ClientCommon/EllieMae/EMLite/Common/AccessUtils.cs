// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.AccessUtils
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class AccessUtils
  {
    public static Dictionary<Feature, CheckPoint> CheckPoints = new Dictionary<Feature, CheckPoint>()
    {
      {
        Feature.AnalysisTool,
        new CheckPoint("AllowAnalysisTool", "EnableAnalysisTool", string.Empty, new AclFeature?())
      },
      {
        Feature.RebuildFieldSearchData,
        new CheckPoint("AllowAnalysisTool", "EnableAnalysisTool", string.Empty, new AclFeature?())
      },
      {
        Feature.WebPipeline,
        new CheckPoint("AllowWebPipeline", "EnableWebPipeline", "EncompassWebPipelineViewByAdmin", new AclFeature?(AclFeature.ThinThick_PipelineTab_Access))
      },
      {
        Feature.WebTrading,
        new CheckPoint("AllowWebTrading", "EnableWebTrading", "EncompassWebTradingViewByAdmin", new AclFeature?(AclFeature.ThinThick_TradesTab_Access))
      },
      {
        Feature.Mobile,
        new CheckPoint("AllowMobile", string.Empty, string.Empty, new AclFeature?(AclFeature.PlatForm_Access))
      }
    };

    public static bool IsFeatureEnabled(Feature feature, Sessions.Session session)
    {
      string policyName = AccessUtils.CheckPoints[feature].PolicyName;
      if (!string.IsNullOrEmpty(policyName) && (bool) session.SettingsManager.GetServerSetting(string.Format("Policies.{0}", (object) policyName)))
        return true;
      string settingName = AccessUtils.CheckPoints[feature].SettingName;
      return !string.IsNullOrEmpty(settingName) && session.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted && AccessUtils.GetClientSetting(settingName, session);
    }

    public static bool CanAccessFeature(Feature feature)
    {
      if (!AccessUtils.IsFeatureEnabled(feature, Session.DefaultInstance))
        return false;
      if (Session.UserInfo.IsAdministrator() && !string.IsNullOrEmpty(AccessUtils.CheckPoints[feature].AdminPolicyName))
        return (bool) Session.SettingsManager.GetServerSetting(string.Format("Policies.{0}", (object) AccessUtils.CheckPoints[feature].AdminPolicyName));
      if (Session.UserInfo.IsAdministrator())
        return true;
      return AccessUtils.CheckPoints[feature].AclFeature.HasValue && ((FeatureConfigsAclManager) Session.ACL.GetAclManager(AclCategory.FeatureConfigs)).GetUserApplicationRight(AccessUtils.CheckPoints[feature].AclFeature.Value) > 0;
    }

    public static bool UserCanAccessLoanErrorInformation()
    {
      return Session.UserInfo.IsAdministrator() || ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_LoanErrorInformation);
    }

    public static bool GetClientSetting(string settingName, Sessions.Session session)
    {
      string str = (string) null;
      try
      {
        str = SmartClientUtils.GetAttribute(session.CompanyInfo.ClientID, "Encompass.exe", settingName);
      }
      catch
      {
      }
      if (str == null)
        return false;
      return str.Trim() == "1";
    }
  }
}
