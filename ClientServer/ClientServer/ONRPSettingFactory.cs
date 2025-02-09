// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ONRPSettingFactory
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class ONRPSettingFactory
  {
    public static ONRPEntitySettings GetRetailBranchUISetting(IDictionary policySettings)
    {
      return new ONRPEntitySettings((ONRPBaseRule) new ONRPSettingRules(), new LockDeskGlobalSettings(policySettings, LoanChannel.BankedRetail));
    }

    public static ONRPEntitySettings GetRetailBranchSDKSetting(IDictionary policySettings)
    {
      return new ONRPEntitySettings((IONRPRuleHandler) new SDKMessageHandler(), (ONRPBaseRule) new ONRPSettingRules(), new LockDeskGlobalSettings(policySettings, LoanChannel.BankedRetail), false);
    }

    public static ONRPEntitySettings GetWholesaleUISetting(IDictionary policySettings)
    {
      return new ONRPEntitySettings((ONRPBaseRule) new ONRPSettingRules(), new LockDeskGlobalSettings(policySettings, LoanChannel.BankedWholesale));
    }

    public static ONRPEntitySettings GetWholesaleSDKSetting(IDictionary policySettings)
    {
      return new ONRPEntitySettings((IONRPRuleHandler) new SDKMessageHandler(), (ONRPBaseRule) new ONRPSettingRules(), new LockDeskGlobalSettings(policySettings, LoanChannel.BankedWholesale), false);
    }

    public static ONRPEntitySettings GetCorrespondentUISetting(IDictionary policySettings)
    {
      return new ONRPEntitySettings((ONRPBaseRule) new ONRPSettingRules(), new LockDeskGlobalSettings(policySettings, LoanChannel.Correspondent));
    }

    public static ONRPEntitySettings GetCorrespondentSDKSetting(IDictionary policySettings)
    {
      return new ONRPEntitySettings((IONRPRuleHandler) new SDKMessageHandler(), (ONRPBaseRule) new ONRPSettingRules(), new LockDeskGlobalSettings(policySettings, LoanChannel.Correspondent), false);
    }
  }
}
