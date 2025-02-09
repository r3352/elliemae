// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.LockDeskSettings.LockDeskSettingsService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System;
using System.Collections;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.LockDeskSettings
{
  public class LockDeskSettingsService : ILockDeskSettings
  {
    private IConfigurationManager mngr;
    private Session session;

    public LockDeskSettingsService(Session session)
    {
      this.session = session;
      this.mngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
    }

    public LockDeskSetting GetLockDeskSetting()
    {
      throw new Exception("This API is obsolete, use GetRetailLockDeskHours(), GetWholesaleLockDeskHours(), GeCorrespondentLockDeskHours() APIs instead.");
    }

    public BranchGlobalSettings GetRetailGlobalSetting()
    {
      throw new Exception("This API is obsolete, use BranchGlobalSettings GetGlobalRetailSetting() API instead.");
    }

    public BranchGlobalSettings GetWholesaleGlobalSetting()
    {
      throw new Exception("This API is obsolete, use BranchGlobalSettings GetGlobalWholesaleSetting() API instead.");
    }

    public BranchGlobalSettings GetCorrespondentGlobalSetting()
    {
      throw new Exception("This API is obsolete, use BranchGlobalSettings GetGlobalCorrespondentSetting() API instead.");
    }

    public BranchGlobalSettings GetGlobalRetailSetting()
    {
      BranchGlobalSettings globalRetailSetting = new BranchGlobalSettings();
      IDictionary serverSettings = ((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies");
      LockDeskGlobalSettings deskGlobalSettings = new LockDeskGlobalSettings(serverSettings, (LoanChannel) 1);
      globalRetailSetting.ONRPEnabled = serverSettings[(object) "Policies.ENABLEONRPRET"].ToString();
      globalRetailSetting.ONRPSatEnabled = serverSettings[(object) "Policies.ENABLEONRPRETSAT"].ToString();
      globalRetailSetting.ONRPSunEnabled = serverSettings[(object) "Policies.ENABLEONRPRETSUN"].ToString();
      bool flag1 = globalRetailSetting.ONRPEnabled.ToString().ToUpper() == "TRUE" && deskGlobalSettings.LockDeskStartTime == deskGlobalSettings.LockDeskEndTime;
      bool flag2 = globalRetailSetting.ONRPSatEnabled.ToString().ToUpper() == "TRUE" && deskGlobalSettings.LockDeskStartTimeSat == deskGlobalSettings.LockDeskEndTimeSat;
      bool flag3 = globalRetailSetting.ONRPSunEnabled.ToString().ToUpper() == "TRUE" && deskGlobalSettings.LockDeskStartTimeSun == deskGlobalSettings.LockDeskEndTimeSun;
      globalRetailSetting.ContinuousCoverage = serverSettings[(object) "Policies.ONRPRETCVRG"].ToString();
      int num1 = globalRetailSetting.ONRPEnabled.ToString().ToUpper() == "FALSE" ? 1 : (globalRetailSetting.ContinuousCoverage.ToString().ToUpper() == "CONTINUOUS" ? 1 : 0);
      int num2 = flag1 ? 1 : 0;
      globalRetailSetting.StartTime = (num1 | num2) == 0 ? serverSettings[(object) "Policies.RETLDENDTIME"].ToString() : "";
      int num3 = globalRetailSetting.ONRPSatEnabled.ToString().ToUpper() == "FALSE" ? 1 : (globalRetailSetting.ContinuousCoverage.ToString().ToUpper() == "CONTINUOUS" ? 1 : 0);
      int num4 = flag2 ? 1 : 0;
      globalRetailSetting.SatStartTime = (num3 | num4) == 0 ? serverSettings[(object) "Policies.RETLDSATENDTIME"].ToString() : "";
      int num5 = globalRetailSetting.ONRPSunEnabled.ToString().ToUpper() == "FALSE" ? 1 : (globalRetailSetting.ContinuousCoverage.ToString().ToUpper() == "CONTINUOUS" ? 1 : 0);
      int num6 = flag3 ? 1 : 0;
      globalRetailSetting.SunStartTime = (num5 | num6) == 0 ? serverSettings[(object) "Policies.RETLDSUNENDTIME"].ToString() : "";
      globalRetailSetting.EndTime = serverSettings[(object) "Policies.ONRPRETENDTIME"].ToString();
      globalRetailSetting.SatEndTime = serverSettings[(object) "Policies.ONRPRETSATENDTIME"].ToString();
      globalRetailSetting.SunEndTime = serverSettings[(object) "Policies.ONRPRETSUNENDTIME"].ToString();
      globalRetailSetting.NoMaxLimit = serverSettings[(object) "Policies.ONRPNOMAXLIMITRET"].ToString();
      globalRetailSetting.WeekendHoliday = serverSettings[(object) "Policies.ENABLEONRPWHRETCVRG"].ToString();
      if (globalRetailSetting.NoMaxLimit.ToString().ToUpper() == "FALSE")
      {
        globalRetailSetting.Tolerance = serverSettings[(object) "Policies.ONRPRETDOLTOL"].ToString();
        globalRetailSetting.DollarLimit = serverSettings[(object) "Policies.ONRPRETDOLLIMIT"].ToString();
      }
      else
      {
        globalRetailSetting.Tolerance = "";
        globalRetailSetting.DollarLimit = "";
      }
      return globalRetailSetting;
    }

    public BranchGlobalSettings GetGlobalWholesaleSetting()
    {
      BranchGlobalSettings wholesaleSetting = new BranchGlobalSettings();
      IDictionary serverSettings = ((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies");
      LockDeskGlobalSettings deskGlobalSettings = new LockDeskGlobalSettings(serverSettings, (LoanChannel) 2);
      wholesaleSetting.ONRPEnabled = serverSettings[(object) "Policies.ENABLEONRPBROKER"].ToString();
      wholesaleSetting.ONRPSatEnabled = serverSettings[(object) "Policies.ENABLEONRPBROKERSAT"].ToString();
      wholesaleSetting.ONRPSunEnabled = serverSettings[(object) "Policies.ENABLEONRPBROKERSUN"].ToString();
      bool flag1 = wholesaleSetting.ONRPEnabled.ToString().ToUpper() == "TRUE" && deskGlobalSettings.LockDeskStartTime == deskGlobalSettings.LockDeskEndTime;
      bool flag2 = wholesaleSetting.ONRPSatEnabled.ToString().ToUpper() == "TRUE" && deskGlobalSettings.LockDeskStartTimeSat == deskGlobalSettings.LockDeskEndTimeSat;
      bool flag3 = wholesaleSetting.ONRPSunEnabled.ToString().ToUpper() == "TRUE" && deskGlobalSettings.LockDeskStartTimeSun == deskGlobalSettings.LockDeskEndTimeSun;
      wholesaleSetting.ContinuousCoverage = serverSettings[(object) "Policies.ONRPBROKERCVRG"].ToString();
      int num1 = wholesaleSetting.ONRPEnabled.ToString().ToUpper() == "FALSE" ? 1 : (wholesaleSetting.ContinuousCoverage.ToString().ToUpper() == "CONTINUOUS" ? 1 : 0);
      int num2 = flag1 ? 1 : 0;
      wholesaleSetting.StartTime = (num1 | num2) == 0 ? serverSettings[(object) "Policies.BROKERLDENDTIME"].ToString() : "";
      int num3 = wholesaleSetting.ONRPSatEnabled.ToString().ToUpper() == "FALSE" ? 1 : (wholesaleSetting.ContinuousCoverage.ToString().ToUpper() == "CONTINUOUS" ? 1 : 0);
      int num4 = flag2 ? 1 : 0;
      wholesaleSetting.SatStartTime = (num3 | num4) == 0 ? serverSettings[(object) "Policies.BROKERLDSATENDTIME"].ToString() : "";
      int num5 = wholesaleSetting.ONRPSunEnabled.ToString().ToUpper() == "FALSE" ? 1 : (wholesaleSetting.ContinuousCoverage.ToString().ToUpper() == "CONTINUOUS" ? 1 : 0);
      int num6 = flag3 ? 1 : 0;
      wholesaleSetting.SunStartTime = (num5 | num6) == 0 ? serverSettings[(object) "Policies.BROKERLDSUNENDTIME"].ToString() : "";
      wholesaleSetting.EndTime = serverSettings[(object) "Policies.ONRPBROKERENDTIME"].ToString();
      wholesaleSetting.SatEndTime = serverSettings[(object) "Policies.ONRPBROKERSATENDTIME"].ToString();
      wholesaleSetting.SunEndTime = serverSettings[(object) "Policies.ONRPBROKERSUNENDTIME"].ToString();
      wholesaleSetting.NoMaxLimit = serverSettings[(object) "Policies.ONRPNOMAXLIMITBROKER"].ToString();
      wholesaleSetting.WeekendHoliday = serverSettings[(object) "Policies.ENABLEONRPWHBROKERCVRG"].ToString();
      if (wholesaleSetting.NoMaxLimit.ToString().ToUpper() == "FALSE")
      {
        wholesaleSetting.Tolerance = serverSettings[(object) "Policies.ONRPBROKERDOLTOL"].ToString();
        wholesaleSetting.DollarLimit = serverSettings[(object) "Policies.ONRPBROKERDOLLIMIT"].ToString();
      }
      else
      {
        wholesaleSetting.Tolerance = "";
        wholesaleSetting.DollarLimit = "";
      }
      return wholesaleSetting;
    }

    public BranchGlobalSettings GetGlobalCorrespondentSetting()
    {
      BranchGlobalSettings correspondentSetting = new BranchGlobalSettings();
      IDictionary serverSettings = ((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies");
      LockDeskGlobalSettings deskGlobalSettings = new LockDeskGlobalSettings(serverSettings, (LoanChannel) 4);
      correspondentSetting.ONRPEnabled = serverSettings[(object) "Policies.ENABLEONRPCOR"].ToString();
      correspondentSetting.ONRPSatEnabled = serverSettings[(object) "Policies.ENABLEONRPCORSAT"].ToString();
      correspondentSetting.ONRPSunEnabled = serverSettings[(object) "Policies.ENABLEONRPCORSUN"].ToString();
      bool flag1 = correspondentSetting.ONRPEnabled.ToString().ToUpper() == "TRUE" && deskGlobalSettings.LockDeskStartTime == deskGlobalSettings.LockDeskEndTime;
      bool flag2 = correspondentSetting.ONRPSatEnabled.ToString().ToUpper() == "TRUE" && deskGlobalSettings.LockDeskStartTimeSat == deskGlobalSettings.LockDeskEndTimeSat;
      bool flag3 = correspondentSetting.ONRPSunEnabled.ToString().ToLower() == "TRUE" && deskGlobalSettings.LockDeskStartTimeSun == deskGlobalSettings.LockDeskEndTimeSun;
      correspondentSetting.ContinuousCoverage = serverSettings[(object) "Policies.ONRPCORCVRG"].ToString();
      int num1 = correspondentSetting.ONRPEnabled.ToString().ToUpper() == "FALSE" ? 1 : (correspondentSetting.ContinuousCoverage.ToString().ToUpper() == "CONTINUOUS" ? 1 : 0);
      int num2 = flag1 ? 1 : 0;
      correspondentSetting.StartTime = (num1 | num2) == 0 ? serverSettings[(object) "Policies.CORLDENDTIME"].ToString() : "";
      int num3 = correspondentSetting.ONRPSatEnabled.ToString().ToUpper() == "FALSE" ? 1 : (correspondentSetting.ContinuousCoverage.ToString().ToUpper() == "CONTINUOUS" ? 1 : 0);
      int num4 = flag2 ? 1 : 0;
      correspondentSetting.SatStartTime = (num3 | num4) == 0 ? serverSettings[(object) "Policies.CORLDSATENDTIME"].ToString() : "";
      int num5 = correspondentSetting.ONRPSunEnabled.ToString().ToUpper() == "FALSE" ? 1 : (correspondentSetting.ContinuousCoverage.ToString().ToUpper() == "CONTINUOUS" ? 1 : 0);
      int num6 = flag3 ? 1 : 0;
      correspondentSetting.SunStartTime = (num5 | num6) == 0 ? serverSettings[(object) "Policies.CORLDSUNENDTIME"].ToString() : "";
      correspondentSetting.EndTime = serverSettings[(object) "Policies.ONRPCORENDTIME"].ToString();
      correspondentSetting.SatEndTime = serverSettings[(object) "Policies.ONRPCORSATENDTIME"].ToString();
      correspondentSetting.SunEndTime = serverSettings[(object) "Policies.ONRPCORSUNENDTIME"].ToString();
      correspondentSetting.NoMaxLimit = serverSettings[(object) "Policies.ONRPNOMAXLIMITCOR"].ToString();
      correspondentSetting.WeekendHoliday = serverSettings[(object) "Policies.ENABLEONRPWHCORCVRG"].ToString();
      if (correspondentSetting.NoMaxLimit.ToString().ToUpper() == "FALSE")
      {
        correspondentSetting.Tolerance = serverSettings[(object) "Policies.ONRPCORDOLTOL"].ToString();
        correspondentSetting.DollarLimit = serverSettings[(object) "Policies.ONRPCORDOLLIMIT"].ToString();
      }
      else
      {
        correspondentSetting.Tolerance = "";
        correspondentSetting.DollarLimit = "";
      }
      return correspondentSetting;
    }

    public void SetBrokerONRPSettingForTPO(string tpoId, ONRPBranchSettings setting)
    {
      throw new Exception("This API is obsolete, use void SetOnrpSettingForBrokerChannel(int orgId, ONRPBrokerBranchSetting setting) API instead.");
    }

    public void SetCorrespondentONRPSettingForTPO(string tpoId, ONRPBranchSettings setting)
    {
      throw new Exception("This API is obsolete, use void SetOnrpSettingForCorrespondentChannel(int orgId, ONRPCorrespondentBranchSetting setting) API instead.");
    }

    public void SetOnrpSettingForRetailChannelFromSDK(int orgId, ONRPRetailBranchSettings settings)
    {
      throw new Exception("This API is obsolete, use void SetOnrpSettingForRetailChannel(int orgId, ONRPRetailBranchSetting setting) API instead.");
    }

    public ONRPRetailBranchSettings GetOnrpSettingForRetailChannelFromSDK(int orgId)
    {
      throw new Exception("This API is obsolete, use void GetOnrpSettingForRetailChannel(int orgId) API instead.");
    }

    public ONRPBranchSettings GetBrokerONRPSettingForTPO(string tpoId)
    {
      throw new Exception("This API is obsolete, use void GetOnrpSettingForBrokerChannel(int tpoId) API instead.");
    }

    public ONRPBranchSettings GetCorrespondentONRPSettingForTPO(string tpoId)
    {
      throw new Exception("This API is obsolete, use void GetOnrpSettingForCorrespontChannel(int tpoId) API instead.");
    }

    private ONRPBranchSettings SetSettingsFromEncompass(ONRPEntitySettings sourceSetting)
    {
      ONRPBranchSettings onrpBranchSettings = new ONRPBranchSettings();
      onrpBranchSettings.EnableONRP = new bool?(sourceSetting.EnableONRP);
      onrpBranchSettings.ChannelDefaultOrCustomize = new bool?(sourceSetting.UseChannelDefault);
      onrpBranchSettings.ContinousCoverageOrSpecifyTime = new bool?(sourceSetting.ContinuousCoverage);
      if (!string.IsNullOrEmpty(sourceSetting.ONRPEndTime))
        onrpBranchSettings.EndTime = new DateTime?(DateTime.Parse(sourceSetting.ONRPEndTime));
      if (!string.IsNullOrEmpty(sourceSetting.ONRPStartTime))
        onrpBranchSettings.StartTime = new DateTime?(DateTime.Parse(sourceSetting.ONRPStartTime));
      onrpBranchSettings.WeekendHolidayCoverage = new bool?(sourceSetting.WeekendHolidayCoverage);
      onrpBranchSettings.NoMaxLimit = new bool?(sourceSetting.MaximumLimit);
      onrpBranchSettings.DollarLimit = new double?(sourceSetting.DollarLimit);
      onrpBranchSettings.Tolerance = new int?(sourceSetting.Tolerance);
      return onrpBranchSettings;
    }

    public void SetOnrpSettingForBrokerChannel(string tpoId, ONRPTPOBranchSetting setting)
    {
      string str = "Channel: Brokered, Tpo: Tpo Id " + tpoId;
      ExternalOriginatorManagementData originatorManagementData = this.mngr.GetExternalOrganizationByTPOID(tpoId).FirstOrDefault<ExternalOriginatorManagementData>();
      if (originatorManagementData == null)
        throw new Exception(str + " from SDK no match.");
      if (originatorManagementData.entityType != 1 && originatorManagementData.entityType != 3)
        throw new Exception(str + ": \"Broker\" Channel Type not enabled on Basic Info Tab. ONRP may not be enabled for this TPO Client/Channel.");
      Hashtable hashtable = new Hashtable();
      hashtable.Add((object) "[HEADER]", (object) ("Channel: Brokered, TPO: " + tpoId));
      hashtable.Add((object) "[BRANCH]", (object) "Broker");
      ONRPEntitySettings wholesaleSdkSetting = ONRPSettingFactory.GetWholesaleSDKSetting(((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies"));
      ExternalOrgOnrpSettings externalOrgOnrpSettings1 = this.mngr.GetExternalOrgOnrpSettings(originatorManagementData.oid);
      ExternalOrgOnrpSettings externalOrgOnrpSettings2 = externalOrgOnrpSettings1.Clone();
      wholesaleSdkSetting.MessageHandler = (IONRPRuleHandler) new SDKMessageHandler(hashtable);
      externalOrgOnrpSettings1.Broker = this.SetTPOSettingsFromSDK(setting, wholesaleSdkSetting);
      bool flag;
      ONRPUtils.ValidateSettings(externalOrgOnrpSettings1.Broker.MessageHandler, externalOrgOnrpSettings1.Broker, ref flag);
      this.mngr.UpdateOnrpSettings(externalOrgOnrpSettings1, originatorManagementData.oid, externalOrgOnrpSettings2);
    }

    public ONRPTPOBranchSetting GetOnrpSettingForBrokerChannel(string tpoId)
    {
      string str = "Channel: Broker, TPO: TPO Id " + tpoId;
      ExternalOrgOnrpSettings externalOrgOnrpSettings = this.mngr.GetExternalOrgOnrpSettings((this.mngr.GetExternalOrganizationByTPOID(tpoId).FirstOrDefault<ExternalOriginatorManagementData>() ?? throw new Exception(str + " from SDK no match. ")).oid);
      if (externalOrgOnrpSettings.ONRPID == -1)
        externalOrgOnrpSettings.Broker.UseChannelDefault = true;
      IDictionary serverSettings = ((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies");
      externalOrgOnrpSettings.Broker.SetRules((IONRPRuleHandler) null, new ONRPBaseRule(), new LockDeskGlobalSettings(serverSettings, (LoanChannel) 2), externalOrgOnrpSettings.Broker.UseChannelDefault);
      externalOrgOnrpSettings.Broker.ONRPStartTime = this.GetGlobalWholesaleSetting().StartTime;
      return new ONRPTPOBranchSetting().Convert(this.SetSettingsFromEncompassObject(externalOrgOnrpSettings.Broker));
    }

    public void SetOnrpSettingForCorrespondentChannel(string tpoId, ONRPTPOBranchSetting setting)
    {
      string str = "Channel: Correspondent, Tpo: Tpo Id " + tpoId;
      ExternalOriginatorManagementData originatorManagementData = this.mngr.GetExternalOrganizationByTPOID(tpoId).FirstOrDefault<ExternalOriginatorManagementData>();
      if (originatorManagementData == null)
        throw new Exception(str + " from SDK no match.");
      if (originatorManagementData.entityType != 2 && originatorManagementData.entityType != 3)
        throw new Exception(str + ": \"Correspondent\" Channel Type not enabled on Basic Info Tab. ONRP may not be enabled for this TPO Client/Channel.");
      Hashtable hashtable = new Hashtable();
      hashtable.Add((object) "[HEADER]", (object) ("Channel: Correspondent, TPO: " + tpoId));
      hashtable.Add((object) "[BRANCH]", (object) "Correspondent");
      ONRPEntitySettings correspondentSdkSetting = ONRPSettingFactory.GetCorrespondentSDKSetting(((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies"));
      ExternalOrgOnrpSettings externalOrgOnrpSettings1 = this.mngr.GetExternalOrgOnrpSettings(originatorManagementData.oid);
      ExternalOrgOnrpSettings externalOrgOnrpSettings2 = externalOrgOnrpSettings1.Clone();
      correspondentSdkSetting.MessageHandler = (IONRPRuleHandler) new SDKMessageHandler(hashtable);
      externalOrgOnrpSettings1.Correspondent = this.SetTPOSettingsFromSDK(setting, correspondentSdkSetting);
      bool flag;
      ONRPUtils.ValidateSettings(externalOrgOnrpSettings1.Correspondent.MessageHandler, externalOrgOnrpSettings1.Correspondent, ref flag);
      this.mngr.UpdateOnrpSettings(externalOrgOnrpSettings1, originatorManagementData.oid, externalOrgOnrpSettings2);
    }

    public ONRPTPOBranchSetting GetOnrpSettingForCorrespontChannel(string tpoId)
    {
      string str = "Channel: Correspondent, Tpo: Tpo Id " + tpoId;
      ExternalOrgOnrpSettings externalOrgOnrpSettings = this.mngr.GetExternalOrgOnrpSettings((this.mngr.GetExternalOrganizationByTPOID(tpoId).FirstOrDefault<ExternalOriginatorManagementData>() ?? throw new Exception(str + " from SDK no match.")).oid);
      if (externalOrgOnrpSettings.ONRPID == -1)
        externalOrgOnrpSettings.Correspondent.UseChannelDefault = true;
      IDictionary serverSettings = ((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies");
      externalOrgOnrpSettings.Correspondent.SetRules((IONRPRuleHandler) null, new ONRPBaseRule(), new LockDeskGlobalSettings(serverSettings, (LoanChannel) 4), externalOrgOnrpSettings.Correspondent.UseChannelDefault);
      externalOrgOnrpSettings.Correspondent.ONRPStartTime = this.GetGlobalCorrespondentSetting().StartTime;
      return new ONRPTPOBranchSetting().Convert(this.SetSettingsFromEncompassObject(externalOrgOnrpSettings.Correspondent));
    }

    public void SetOnrpSettingForRetailChannel(int orgId, ONRPRetailBranchSetting setting)
    {
      string str = "Channel: Retail, Organization: Org Id " + (object) orgId;
      if (orgId == 0)
        throw new Exception(str + " : invalid. Access to Administration Folder is not allowed in SDK for ONRP Settings.");
      if (!this.mngr.IsRetailBranchExist(orgId))
        throw new Exception(str + " from SDK no match. ");
      Hashtable hashtable = new Hashtable();
      hashtable.Add((object) "[HEADER]", (object) ("Channel: Retail, Organization: " + (object) orgId));
      hashtable.Add((object) "[BRANCH]", (object) "Retail");
      ONRPEntitySettings branchSdkSetting = ONRPSettingFactory.GetRetailBranchSDKSetting(((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies"));
      OrgInfo organization = this.session.SessionObjects.OrganizationManager.GetOrganization(orgId);
      ONRPEntitySettings onrpEntitySettings = LockDeskSettingsService.OrgInfoToOrgOnrpSettings(organization);
      if (organization.Parent == 0 && setting.UseParentInfo)
        throw new Exception(str + " Use Parent Info not selectable on second level Folders (under Administration Folder). There is no Parent entity that contains ONRP Settings.");
      branchSdkSetting.MessageHandler = (IONRPRuleHandler) new SDKMessageHandler(hashtable);
      onrpEntitySettings.UseParentInfo = setting.UseParentInfo;
      if (this.DetectONRPSettingChange(setting) && setting.UseParentInfo)
        throw new Exception(str + " set to 'Use Parent Info'. ONRP Settings may not be edited.");
      if (!setting.UseParentInfo)
      {
        onrpEntitySettings = this.SetSettingsFromSDK(setting, branchSdkSetting);
        bool flag;
        ONRPUtils.ValidateSettings(onrpEntitySettings.MessageHandler, onrpEntitySettings, ref flag);
      }
      organization.ONRPRetailBranchSettings = onrpEntitySettings;
      this.session.SessionObjects.OrganizationManager.UpdateOrganization(organization);
    }

    public ONRPRetailBranchSetting GetOnrpSettingForRetailChannel(int orgId)
    {
      string str = "Channel: Retail, Organization: Org Id " + (object) orgId;
      if (orgId == 0)
        throw new Exception(str + " : invalid. Access to Administration Folder is not allowed in SDK for ONRP Settings.");
      if (!this.mngr.IsRetailBranchExist(orgId))
        throw new Exception(str + " from SDK no match. ");
      IDictionary serverSettings = ((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies");
      ONRPSettingFactory.GetRetailBranchSDKSetting(serverSettings);
      OrgInfo info = this.session.SessionObjects.OrganizationManager.GetOrganization(orgId);
      if (info.ONRPRetailBranchSettings.UseParentInfo)
      {
        info = this.session.SessionObjects.OrganizationManager.GetFirstOrganizationWithONRP(orgId);
        info.ONRPRetailBranchSettings.UseParentInfo = true;
      }
      ONRPEntitySettings orgOnrpSettings = LockDeskSettingsService.OrgInfoToOrgOnrpSettings(info);
      orgOnrpSettings.ONRPStartTime = serverSettings[(object) "Policies.RETLDSTRTIME"].ToString();
      orgOnrpSettings.SetRules((IONRPRuleHandler) null, (ONRPBaseRule) new ONRPSettingRules(), new LockDeskGlobalSettings(serverSettings, (LoanChannel) 1), orgOnrpSettings.UseChannelDefault);
      ONRPRetailBranchSetting forRetailChannel = new ONRPRetailBranchSetting();
      forRetailChannel.Convert(this.SetSettingsFromEncompassObject(orgOnrpSettings));
      forRetailChannel.UseParentInfo = info.ONRPRetailBranchSettings.UseParentInfo;
      return forRetailChannel;
    }

    private bool DetectONRPSettingChange(ONRPRetailBranchSetting setting)
    {
      return setting.ChannelDefaultOrCustomize.HasValue || setting.ContinousCoverageOrSpecifyTime.HasValue || setting.DollarLimit.HasValue || setting.EnableONRP.HasValue || setting.WeekDayEndTime != null || setting.NoMaxLimit.HasValue || setting.Tolerance.HasValue || setting.WeekendHolidayCoverage.HasValue || setting.EnableSaturday.HasValue || setting.SaturdayEndTime != null || setting.EnableSunday.HasValue || setting.SundayEndTime != null;
    }

    private ONRPEntitySettings SetSettingsFromSDK(
      ONRPRetailBranchSetting sourceSetting,
      ONRPEntitySettings destSetting)
    {
      destSetting.UseParentInfo = sourceSetting.UseParentInfo;
      if (sourceSetting.EnableONRP.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        bool? enableOnrp = sourceSetting.EnableONRP;
        bool flag = true;
        int num = enableOnrp.GetValueOrDefault() == flag & enableOnrp.HasValue ? 1 : 0;
        onrpEntitySettings.EnableONRP = num != 0;
      }
      if (sourceSetting.ChannelDefaultOrCustomize.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        bool? defaultOrCustomize = sourceSetting.ChannelDefaultOrCustomize;
        bool flag = true;
        int num = defaultOrCustomize.GetValueOrDefault() == flag & defaultOrCustomize.HasValue ? 1 : 0;
        onrpEntitySettings.UseChannelDefault = num != 0;
      }
      if (sourceSetting.ContinousCoverageOrSpecifyTime.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        bool? coverageOrSpecifyTime = sourceSetting.ContinousCoverageOrSpecifyTime;
        bool flag = true;
        int num = coverageOrSpecifyTime.GetValueOrDefault() == flag & coverageOrSpecifyTime.HasValue ? 1 : 0;
        onrpEntitySettings.ContinuousCoverage = num != 0;
      }
      if (sourceSetting.WeekDayEndTime != null)
        destSetting.ONRPEndTime = string.IsNullOrEmpty(sourceSetting.WeekDayEndTime) ? "" : ONRPEntitySettings.ConverToDateTime(sourceSetting.WeekDayEndTime).ToString("HH\\:mm");
      if (sourceSetting.WeekendHolidayCoverage.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        bool? weekendHolidayCoverage = sourceSetting.WeekendHolidayCoverage;
        bool flag = true;
        int num = weekendHolidayCoverage.GetValueOrDefault() == flag & weekendHolidayCoverage.HasValue ? 1 : 0;
        onrpEntitySettings.WeekendHolidayCoverage = num != 0;
      }
      if (sourceSetting.NoMaxLimit.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        bool? noMaxLimit = sourceSetting.NoMaxLimit;
        bool flag = true;
        int num = noMaxLimit.GetValueOrDefault() == flag & noMaxLimit.HasValue ? 1 : 0;
        onrpEntitySettings.MaximumLimit = num != 0;
      }
      if (sourceSetting.DollarLimit.HasValue)
        destSetting.DollarLimit = double.Parse(sourceSetting.DollarLimit.ToString());
      if (sourceSetting.Tolerance.HasValue)
        destSetting.Tolerance = int.Parse(sourceSetting.Tolerance.ToString());
      if (sourceSetting.EnableSaturday.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        bool? enableSaturday = sourceSetting.EnableSaturday;
        bool flag = true;
        int num = enableSaturday.GetValueOrDefault() == flag & enableSaturday.HasValue ? 1 : 0;
        onrpEntitySettings.EnableSatONRP = num != 0;
      }
      if (sourceSetting.SaturdayStartTime != null)
        destSetting.ONRPSatStartTime = string.IsNullOrEmpty(sourceSetting.SaturdayStartTime) ? "" : ONRPEntitySettings.ConverToDateTime(sourceSetting.SaturdayStartTime).ToString("HH\\:mm");
      if (sourceSetting.SaturdayEndTime != null)
        destSetting.ONRPSatEndTime = string.IsNullOrEmpty(sourceSetting.SaturdayEndTime) ? "" : ONRPEntitySettings.ConverToDateTime(sourceSetting.SaturdayEndTime).ToString("HH\\:mm");
      if (sourceSetting.EnableSunday.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        bool? enableSunday = sourceSetting.EnableSunday;
        bool flag = true;
        int num = enableSunday.GetValueOrDefault() == flag & enableSunday.HasValue ? 1 : 0;
        onrpEntitySettings.EnableSunONRP = num != 0;
      }
      if (sourceSetting.SundayStartTime != null)
        destSetting.ONRPSunStartTime = string.IsNullOrEmpty(sourceSetting.SundayStartTime) ? "" : ONRPEntitySettings.ConverToDateTime(sourceSetting.SundayStartTime).ToString("HH\\:mm");
      if (sourceSetting.SundayEndTime != null)
        destSetting.ONRPSunEndTime = string.IsNullOrEmpty(sourceSetting.SundayEndTime) ? "" : ONRPEntitySettings.ConverToDateTime(sourceSetting.SundayEndTime).ToString("HH\\:mm");
      return destSetting;
    }

    private ONRPBranchSetting SetSettingsFromEncompassObject(ONRPEntitySettings sourceSetting)
    {
      return new ONRPBranchSetting()
      {
        EnableONRP = new bool?(sourceSetting.EnableONRP),
        ChannelDefaultOrCustomize = new bool?(sourceSetting.UseChannelDefault),
        ContinousCoverageOrSpecifyTime = new bool?(sourceSetting.ContinuousCoverage),
        WeekDayStartTime = sourceSetting.ONRPStartTime,
        WeekDayEndTime = sourceSetting.ONRPEndTime,
        WeekendHolidayCoverage = new bool?(sourceSetting.WeekendHolidayCoverage),
        NoMaxLimit = new bool?(sourceSetting.MaximumLimit),
        DollarLimit = new double?(sourceSetting.DollarLimit),
        Tolerance = new int?(sourceSetting.Tolerance),
        EnableSaturday = new bool?(sourceSetting.EnableSatONRP),
        SaturdayStartTime = sourceSetting.ONRPSatStartTime,
        SaturdayEndTime = sourceSetting.ONRPSatEndTime,
        EnableSunday = new bool?(sourceSetting.EnableSunONRP),
        SundayStartTime = sourceSetting.ONRPSunStartTime,
        SundayEndTime = sourceSetting.ONRPSunEndTime
      };
    }

    private static ONRPEntitySettings OrgInfoToOrgOnrpSettings(OrgInfo info)
    {
      return new ONRPEntitySettings()
      {
        ContinuousCoverage = info.ONRPRetailBranchSettings.ContinuousCoverage,
        EnableONRP = info.ONRPRetailBranchSettings.EnableONRP,
        ONRPEndTime = info.ONRPRetailBranchSettings.ONRPEndTime,
        ONRPStartTime = info.ONRPRetailBranchSettings.ONRPStartTime,
        UseChannelDefault = info.ONRPRetailBranchSettings.UseChannelDefault,
        WeekendHolidayCoverage = info.ONRPRetailBranchSettings.WeekendHolidayCoverage,
        MaximumLimit = info.ONRPRetailBranchSettings.MaximumLimit,
        Tolerance = info.ONRPRetailBranchSettings.Tolerance,
        DollarLimit = info.ONRPRetailBranchSettings.DollarLimit,
        EnableSatONRP = info.ONRPRetailBranchSettings.EnableSatONRP,
        ONRPSatStartTime = info.ONRPRetailBranchSettings.ONRPSatStartTime,
        ONRPSatEndTime = info.ONRPRetailBranchSettings.ONRPSatEndTime,
        EnableSunONRP = info.ONRPRetailBranchSettings.EnableSunONRP,
        ONRPSunStartTime = info.ONRPRetailBranchSettings.ONRPSunStartTime,
        ONRPSunEndTime = info.ONRPRetailBranchSettings.ONRPSunEndTime
      };
    }

    private ONRPEntitySettings SetTPOSettingsFromSDK(
      ONRPTPOBranchSetting sourceSetting,
      ONRPEntitySettings destSetting)
    {
      bool? nullable;
      if (sourceSetting.EnableONRP.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        nullable = sourceSetting.EnableONRP;
        bool flag = true;
        int num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
        onrpEntitySettings.EnableONRP = num != 0;
      }
      if (sourceSetting.ChannelDefaultOrCustomize.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        nullable = sourceSetting.ChannelDefaultOrCustomize;
        bool flag = true;
        int num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
        onrpEntitySettings.UseChannelDefault = num != 0;
      }
      if (sourceSetting.ContinousCoverageOrSpecifyTime.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        nullable = sourceSetting.ContinousCoverageOrSpecifyTime;
        bool flag = true;
        int num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
        onrpEntitySettings.ContinuousCoverage = num != 0;
      }
      DateTime dateTime;
      if (sourceSetting.WeekDayEndTime != null)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        string str;
        if (!string.IsNullOrEmpty(sourceSetting.WeekDayEndTime))
        {
          dateTime = ONRPEntitySettings.ConverToDateTime(sourceSetting.WeekDayEndTime);
          str = dateTime.ToString("HH\\:mm");
        }
        else
          str = "";
        onrpEntitySettings.ONRPEndTime = str;
      }
      if (sourceSetting.WeekendHolidayCoverage.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        nullable = sourceSetting.WeekendHolidayCoverage;
        bool flag = true;
        int num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
        onrpEntitySettings.WeekendHolidayCoverage = num != 0;
      }
      if (sourceSetting.NoMaxLimit.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        nullable = sourceSetting.NoMaxLimit;
        bool flag = true;
        int num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
        onrpEntitySettings.MaximumLimit = num != 0;
      }
      if (sourceSetting.DollarLimit.HasValue)
        destSetting.DollarLimit = double.Parse(sourceSetting.DollarLimit.ToString());
      if (sourceSetting.Tolerance.HasValue)
        destSetting.Tolerance = int.Parse(sourceSetting.Tolerance.ToString());
      if (sourceSetting.EnableSaturday.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        nullable = sourceSetting.EnableSaturday;
        bool flag = true;
        int num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
        onrpEntitySettings.EnableSatONRP = num != 0;
      }
      if (sourceSetting.SaturdayStartTime != null)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        string str;
        if (!string.IsNullOrEmpty(sourceSetting.SaturdayStartTime))
        {
          dateTime = ONRPEntitySettings.ConverToDateTime(sourceSetting.SaturdayStartTime);
          str = dateTime.ToString("HH\\:mm");
        }
        else
          str = "";
        onrpEntitySettings.ONRPSatStartTime = str;
      }
      if (sourceSetting.SaturdayEndTime != null)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        string str;
        if (!string.IsNullOrEmpty(sourceSetting.SaturdayEndTime))
        {
          dateTime = ONRPEntitySettings.ConverToDateTime(sourceSetting.SaturdayEndTime);
          str = dateTime.ToString("HH\\:mm");
        }
        else
          str = "";
        onrpEntitySettings.ONRPSatEndTime = str;
      }
      if (sourceSetting.EnableSunday.HasValue)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        nullable = sourceSetting.EnableSunday;
        bool flag = true;
        int num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
        onrpEntitySettings.EnableSunONRP = num != 0;
      }
      if (sourceSetting.SundayStartTime != null)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        string str;
        if (!string.IsNullOrEmpty(sourceSetting.SundayStartTime))
        {
          dateTime = ONRPEntitySettings.ConverToDateTime(sourceSetting.SundayStartTime);
          str = dateTime.ToString("HH\\:mm");
        }
        else
          str = "";
        onrpEntitySettings.ONRPSunStartTime = str;
      }
      if (sourceSetting.SundayEndTime != null)
      {
        ONRPEntitySettings onrpEntitySettings = destSetting;
        string str;
        if (!string.IsNullOrEmpty(sourceSetting.SundayEndTime))
        {
          dateTime = ONRPEntitySettings.ConverToDateTime(sourceSetting.SundayEndTime);
          str = dateTime.ToString("HH\\:mm");
        }
        else
          str = "";
        onrpEntitySettings.ONRPSunEndTime = str;
      }
      return destSetting;
    }

    public LockDeskChannelSetting GetRetailLockDeskHours()
    {
      IDictionary serverSettings = ((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies");
      return new LockDeskChannelSetting()
      {
        IsAllChannelsEnabled = serverSettings[(object) "Policies.ENABLEALLCHANNEL"].ToString(),
        LockDeskHoursMessage = serverSettings[(object) "Policies.RETLDHRMSG"].ToString(),
        LockDeskShutDownMessage = serverSettings[(object) "Policies.RETLDSHUTDOWNMSG"].ToString(),
        SaturdayEndDateTime = serverSettings[(object) "Policies.RETLDSATENDTIME"].ToString(),
        IsSaturdayHoursEnabled = serverSettings[(object) "Policies.ENABLELDRETSAT"].ToString(),
        SaturdayStartDateTime = serverSettings[(object) "Policies.RETLDSATSTRTIME"].ToString(),
        IsLockDeskShutDown = serverSettings[(object) "Policies.RETLDSHUTDOWN"].ToString(),
        AllowActiveRelockRequests = serverSettings[(object) "Policies.RETLDALLOWACTIVERELOCK"].ToString(),
        SundayEndDateTime = serverSettings[(object) "Policies.RETLDSUNENDTIME"].ToString(),
        IsSundayHoursEnabled = serverSettings[(object) "Policies.ENABLELDRETSUN"].ToString(),
        SundayStartDateTime = serverSettings[(object) "Policies.RETLDSUNSTRTIME"].ToString(),
        WeekdayEndDateTime = serverSettings[(object) "Policies.RETLDENDTIME"].ToString(),
        WeekdayStartDateTime = serverSettings[(object) "Policies.RETLDSTRTIME"].ToString()
      };
    }

    public LockDeskChannelSetting GetWholesaleLockDeskHours()
    {
      IDictionary serverSettings = ((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies");
      return new LockDeskChannelSetting()
      {
        IsAllChannelsEnabled = serverSettings[(object) "Policies.ENABLEALLCHANNEL"].ToString(),
        LockDeskHoursMessage = serverSettings[(object) "Policies.BROKERLDHRMSG"].ToString(),
        LockDeskShutDownMessage = serverSettings[(object) "Policies.BROKERLDSHUTDOWNMSG"].ToString(),
        SaturdayEndDateTime = serverSettings[(object) "Policies.BROKERLDSATENDTIME"].ToString(),
        IsSaturdayHoursEnabled = serverSettings[(object) "Policies.ENABLELDBROKERSAT"].ToString(),
        SaturdayStartDateTime = serverSettings[(object) "Policies.BROKERLDSATSTRTIME"].ToString(),
        IsLockDeskShutDown = serverSettings[(object) "Policies.BROKERLDSHUTDOWN"].ToString(),
        AllowActiveRelockRequests = serverSettings[(object) "Policies.BROKERLDALLOWACTIVERELOCK"].ToString(),
        SundayEndDateTime = serverSettings[(object) "Policies.BROKERLDSUNENDTIME"].ToString(),
        IsSundayHoursEnabled = serverSettings[(object) "Policies.ENABLELDBROKERSUN"].ToString(),
        SundayStartDateTime = serverSettings[(object) "Policies.BROKERLDSUNSTRTIME"].ToString(),
        WeekdayEndDateTime = serverSettings[(object) "Policies.BROKERLDENDTIME"].ToString(),
        WeekdayStartDateTime = serverSettings[(object) "Policies.BROKERLDSTRTIME"].ToString()
      };
    }

    public LockDeskChannelSetting GeCorrespondentLockDeskHours()
    {
      IDictionary serverSettings = ((IServerManager) this.session.GetObject("ServerManager")).GetServerSettings("Policies");
      return new LockDeskChannelSetting()
      {
        IsAllChannelsEnabled = serverSettings[(object) "Policies.ENABLEALLCHANNEL"].ToString(),
        LockDeskHoursMessage = serverSettings[(object) "Policies.CORLDHRMSG"].ToString(),
        LockDeskShutDownMessage = serverSettings[(object) "Policies.CORLDSHUTDOWNMSG"].ToString(),
        SaturdayEndDateTime = serverSettings[(object) "Policies.CORLDSATENDTIME"].ToString(),
        IsSaturdayHoursEnabled = serverSettings[(object) "Policies.ENABLELDCORSAT"].ToString(),
        SaturdayStartDateTime = serverSettings[(object) "Policies.CORLDSATSTRTIME"].ToString(),
        IsLockDeskShutDown = serverSettings[(object) "Policies.CORLDSHUTDOWN"].ToString(),
        AllowActiveRelockRequests = serverSettings[(object) "Policies.CORLDALLOWACTIVERELOCK"].ToString(),
        SundayEndDateTime = serverSettings[(object) "Policies.CORLDSUNENDTIME"].ToString(),
        IsSundayHoursEnabled = serverSettings[(object) "Policies.ENABLELDCORSUN"].ToString(),
        SundayStartDateTime = serverSettings[(object) "Policies.CORLDSUNSTRTIME"].ToString(),
        WeekdayEndDateTime = serverSettings[(object) "Policies.CORLDENDTIME"].ToString(),
        WeekdayStartDateTime = serverSettings[(object) "Policies.CORLDSTRTIME"].ToString()
      };
    }
  }
}
