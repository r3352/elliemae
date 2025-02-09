// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.ConditionConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class ConditionConfiguration
  {
    private ConditionConfiguration()
    {
    }

    public static string GetCacheName(ConditionType conditionType)
    {
      return conditionType.ToString() + "ConditionList";
    }

    public static ConditionTrackingSetup GetConditionTrackingSetup(ConditionType conditionType)
    {
      return ClientContext.GetCurrent().Cache.Get<ConditionTrackingSetup>(ConditionConfiguration.GetCacheName(conditionType), (Func<ConditionTrackingSetup>) (() => ConditionConfiguration.GetConditionTrackingSetupFromXml(conditionType)), CacheSetting.Low);
    }

    public static ConditionTrackingSetup GetConditionTrackingSetupFromXml(
      ConditionType conditionType)
    {
      string name = conditionType.ToString() + "ConditionList";
      if (conditionType == ConditionType.Sell)
        name = ConditionType.Underwriting.ToString() + "ConditionList";
      ConditionTrackingSetup trackingSetupFromXml = (ConditionTrackingSetup) null;
      using (BinaryObject systemSettings = SystemConfiguration.GetSystemSettings(name))
      {
        if (systemSettings != null)
        {
          switch (conditionType)
          {
            case ConditionType.Underwriting:
              trackingSetupFromXml = (ConditionTrackingSetup) systemSettings.ToObject<UnderwritingConditionTrackingSetup>();
              break;
            case ConditionType.PostClosing:
              trackingSetupFromXml = (ConditionTrackingSetup) systemSettings.ToObject<PostClosingConditionTrackingSetup>();
              break;
            case ConditionType.Purchase:
              trackingSetupFromXml = (ConditionTrackingSetup) systemSettings.ToObject<PurchaseConditionTrackingSetup>();
              break;
            case ConditionType.Sell:
              trackingSetupFromXml = (ConditionTrackingSetup) systemSettings.ToObject<SellConditionTrackingSetup>();
              break;
          }
        }
        else
        {
          switch (conditionType)
          {
            case ConditionType.Underwriting:
              trackingSetupFromXml = (ConditionTrackingSetup) new UnderwritingConditionTrackingSetup();
              break;
            case ConditionType.PostClosing:
              trackingSetupFromXml = (ConditionTrackingSetup) new PostClosingConditionTrackingSetup();
              break;
            case ConditionType.Purchase:
              trackingSetupFromXml = (ConditionTrackingSetup) new PurchaseConditionTrackingSetup();
              break;
            case ConditionType.Sell:
              trackingSetupFromXml = (ConditionTrackingSetup) new SellConditionTrackingSetup();
              break;
          }
        }
      }
      return trackingSetupFromXml;
    }

    public static void ResetCacheForConditionTrackingSetup(ConditionType conditionType)
    {
      string name = conditionType.ToString() + "ConditionList";
      ClientContext.GetCurrent().Cache.Remove(name);
    }
  }
}
