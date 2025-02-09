// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.AIQCapsilon.AIQButtonHelper
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.CapsilonAIQ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientCommon.AIQCapsilon
{
  public class AIQButtonHelper
  {
    private const string className = "AIQButtonHelper";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private IWin32Window windowOwner;
    private Button btn_LaunchAIQ;
    private string AIQFolderID;
    private static readonly string AIQLaunchTypeWEB = "WEB";
    private static readonly string AIQLaunchTypeDesktop = "DESKTOP";
    private static readonly string AIQAnalyzerLaunchTypeWEB = "Analyzer_WEB";
    private static readonly string Pipeline_AIQButton_Name = nameof (btn_LaunchAIQ);
    private static readonly string EFolder_AIQButton_Name = "btnAIQ";

    public AIQButtonHelper(IWin32Window owner, Button btn)
    {
      this.windowOwner = owner;
      this.btn_LaunchAIQ = btn;
    }

    public void EnableAIQLaunchButton(string loanGUID)
    {
      try
      {
        if (this.CheckAIQLaunchAccess(false))
        {
          string folderId = Session.SessionObjects.LoanExternalFieldManager.GetFolderID(loanGUID);
          if (!string.IsNullOrWhiteSpace(folderId))
          {
            this.btn_LaunchAIQ.Visible = true;
            this.AIQFolderID = folderId;
            Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "Data & Document Automation DB field for the selected loan has value and so enabling the Launch button.");
          }
          else
          {
            EBSServiceClient ebsServiceClient = new EBSServiceClient(Session.StartupInfo.ServerInstanceName, Session.StartupInfo.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects));
            string key1 = "CX.DV.FOLDERID";
            string key2 = "AIQ.FOLDERID";
            Task<Dictionary<string, string>> laonFieldIdValue = ebsServiceClient.GetLaonFieldIDValue(loanGUID, new string[2]
            {
              key1,
              key2
            });
            if (laonFieldIdValue.Result != null && !string.IsNullOrWhiteSpace(laonFieldIdValue.Result[key1]))
            {
              this.btn_LaunchAIQ.Visible = true;
              this.AIQFolderID = laonFieldIdValue.Result[key1];
              Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "Custom field for the selected has value and so enabling the Launch button.");
            }
            else if (laonFieldIdValue.Result != null && !string.IsNullOrWhiteSpace(laonFieldIdValue.Result[key2]))
            {
              this.btn_LaunchAIQ.Visible = true;
              this.AIQFolderID = laonFieldIdValue.Result[key2];
              Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "Standard field for the selected loan has value and so enabling the Launch button.");
            }
            else
            {
              this.btn_LaunchAIQ.Visible = false;
              Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "Custom or Standard Field for the selected loan does not exist or does not have value so disabling the launch button. ");
            }
          }
        }
        else
          this.btn_LaunchAIQ.Visible = false;
      }
      catch (Exception ex)
      {
        Tracing.Log(AIQButtonHelper.sw, TraceLevel.Error, nameof (AIQButtonHelper), string.Format("Error in loading Launching Data & Document Automation button. Exception: {0}", (object) ex.Message));
      }
    }

    public void EnableAIQLaunchButton(LoanDataMgr loanDataMgr, bool forAnalyzer = false)
    {
      try
      {
        if (this.CheckAIQLaunchAccess(forAnalyzer))
        {
          string folderId = Session.SessionObjects.LoanExternalFieldManager.GetFolderID(loanDataMgr.LoanData.GUID);
          string str1 = loanDataMgr.LoanData.GetField("CX.DV.FOLDERID") ?? "";
          string str2 = loanDataMgr.LoanData.GetField("AIQ.FOLDERID") ?? "";
          if (!string.IsNullOrEmpty(folderId))
          {
            this.btn_LaunchAIQ.Visible = true;
            this.AIQFolderID = folderId;
            Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "Data & Document Automation DB field for the selected has value and so enabling the Launch button.");
          }
          else if (str1.Length != 0)
          {
            this.btn_LaunchAIQ.Visible = true;
            this.AIQFolderID = str1;
            Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "Custom field for the selected has value and so enabling the Launch button.");
          }
          else if (str2.Length != 0)
          {
            this.btn_LaunchAIQ.Visible = true;
            this.AIQFolderID = str2;
            Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "Standard field for the selected loan has value and so enabling the Launch button.");
          }
          else
          {
            this.btn_LaunchAIQ.Visible = false;
            Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "Custom or Standard Field for the selected loan does not exist or does not have value so disabling the launch button. ");
          }
        }
        else
          this.btn_LaunchAIQ.Visible = false;
      }
      catch (Exception ex)
      {
        Tracing.Log(AIQButtonHelper.sw, TraceLevel.Error, nameof (AIQButtonHelper), string.Format("Error in loading Launching Data & Document Automation and Mortgage Analyzers button. Exception: {0}", (object) ex.Message));
      }
    }

    private bool CheckAIQLaunchAccess(bool forAnalyzer)
    {
      bool flag = false;
      if (Session.StartupInfo.HasAIQLicense)
      {
        if (Session.StartupInfo.UserInfo.IsSuperAdministrator())
        {
          flag = true;
          Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "This logged in user is admin and hence making Mortgage Analyzers launch button visible.");
        }
        else if (!forAnalyzer && Session.StartupInfo.UserAclFeaturConfigRights.ContainsKey(AclFeature.SettingsTab_EncompassAIQAccess))
        {
          if (((IEnumerable<int>) new int[2]
          {
            1,
            int.MaxValue
          }).Contains<int>(Session.StartupInfo.UserAclFeaturConfigRights[AclFeature.SettingsTab_EncompassAIQAccess]))
          {
            flag = true;
            Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "This logged in user has EncompassAIQAccess hence making Mortgage Analyzers launch button visible.");
          }
          else
            Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "User does have EncompassAIQAccess but it is not enabled.");
        }
        else if (forAnalyzer && Session.StartupInfo.UserAclFeaturConfigRights.ContainsKey(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers))
        {
          if (((IEnumerable<int>) new int[2]
          {
            1,
            int.MaxValue
          }).Contains<int>(Session.StartupInfo.UserAclFeaturConfigRights[AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers]))
          {
            flag = true;
            Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "This logged in user has EncompassAIQAccess_AIQAnalyzers access hence making Mortgage Analyzers launch button visible.");
          }
          else
            Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "User does have EncompassAIQAccess_AIQAnalyzers but it is not enabled.");
        }
        else
          Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "User does not have EncompassAIQAccess.");
      }
      else
        Tracing.Log(AIQButtonHelper.sw, TraceLevel.Info, nameof (AIQButtonHelper), "User does not have Data & Document Automation and Mortgage Analyzers License.");
      return flag;
    }

    public void btnClick_action(string loanGUID)
    {
      string LaunchType = (string) null;
      string url = (string) null;
      try
      {
        if (this.btn_LaunchAIQ.Name == AIQButtonHelper.Pipeline_AIQButton_Name || this.btn_LaunchAIQ.Name == AIQButtonHelper.EFolder_AIQButton_Name)
        {
          if (Session.StartupInfo.UserAclFeaturConfigRights.ContainsKey(AclFeature.SettingsTab_AIQLaunchType))
            LaunchType = !((IEnumerable<int>) new int[2]
            {
              1,
              int.MaxValue
            }).Contains<int>(Session.StartupInfo.UserAclFeaturConfigRights[AclFeature.SettingsTab_AIQLaunchType]) ? AIQButtonHelper.AIQLaunchTypeWEB : AIQButtonHelper.AIQLaunchTypeDesktop;
        }
        else
          LaunchType = AIQButtonHelper.AIQAnalyzerLaunchTypeWEB;
        if (!string.IsNullOrWhiteSpace(LaunchType))
          url = this.GetLaunchConfiguration(LaunchType, loanGUID);
        if (string.IsNullOrWhiteSpace(url))
          return;
        this.ShowAIQPage(url);
      }
      catch (Exception ex)
      {
        Tracing.Log(AIQButtonHelper.sw, TraceLevel.Error, nameof (AIQButtonHelper), string.Format("Error in Launch Data & Document Automation and Mortgage Analyzers button click. Exception: {0}", (object) ex.Message));
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Unable to launch Data & Document Automation and Mortgage Analyzers. Please retry. If error persists, please contact Customer Support.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void ShowAIQPage(string url)
    {
      try
      {
        Process.Start(new ProcessStartInfo()
        {
          FileName = url,
          UseShellExecute = true
        });
      }
      catch (Exception ex)
      {
        Tracing.Log(AIQButtonHelper.sw, TraceLevel.Error, nameof (AIQButtonHelper), string.Format("Error in Launching Data & Document Automation URL WEB/Desktop. Exception: {0}", (object) ("URL:" + url + " " + ex.Message)));
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Unable to launch Data & Document Automation and Mortgage Analyzers. Please retry. If error persists, please contact Customer Support.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private Dictionary<string, string> GetAPIConfigDetails()
    {
      return new Dictionary<string, string>()
      {
        {
          "AppName",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.AppName")
        },
        {
          "AppSecret",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.AppSecret")
        },
        {
          "AIQcredential",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.credential")
        },
        {
          "AIQ.ApplicationDataMap",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.ApplicationDataMap")
        },
        {
          "AIQ.ApplicationFlags",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.ApplicationFlags")
        },
        {
          "AIQ.GetSiteID.URL",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.GetSiteID.URL")
        },
        {
          "AIQ.LOS.URL",
          Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.LOS.URL")
        },
        {
          "secretKeyId",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "secretKeyId")
        },
        {
          "secretKey",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "secretKey")
        },
        {
          "regionName",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "regionName")
        },
        {
          "serviceName_LOS",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "serviceName_LOS")
        },
        {
          "serviceName_SiteID",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "serviceName_SiteID")
        },
        {
          "TERMINATOR",
          Session.ConfigurationManager.GetCompanySetting("AIQAuth", "TERMINATOR")
        }
      };
    }

    private string GetLaunchConfiguration(string LaunchType, string loanGUID)
    {
      AIQServerClient aiqServerClient = new AIQServerClient(this.GetAPIConfigDetails());
      string DecrptedLaunchCode = (string) null;
      AIQSoftwareInstall aiqSoftwareInstall = new AIQSoftwareInstall();
      string launchConfiguration = (string) null;
      UserInfo userInfo = Session.User.GetUserInfo();
      AIQDecryptor aiqDecryptor = new AIQDecryptor();
      List<string> rIDs = new List<string>();
      List<string> appFlags = new List<string>();
      string settingFromCache = Session.SessionObjects.GetCompanySettingFromCache("AIQSetup", "AIQAddress");
      List<AIQServerClient.ApplicationDataKeyValuePair> dataKeyValuePairList = new List<AIQServerClient.ApplicationDataKeyValuePair>();
      dataKeyValuePairList.Add(new AIQServerClient.ApplicationDataKeyValuePair("userId", Session.UserID));
      dataKeyValuePairList.Add(new AIQServerClient.ApplicationDataKeyValuePair("encompassSiteId", Session.StartupInfo.ServerInstanceName));
      dataKeyValuePairList.Add(new AIQServerClient.ApplicationDataKeyValuePair("encompassFolderId", loanGUID));
      if (LaunchType == AIQButtonHelper.AIQAnalyzerLaunchTypeWEB)
      {
        if (this.btn_LaunchAIQ?.Tag?.ToString() == "aus")
          dataKeyValuePairList.Add(new AIQServerClient.ApplicationDataKeyValuePair("dms-app-name", "WEB_AUS_ANALYZER"));
        else if (this.btn_LaunchAIQ?.Tag?.ToString() == "asset")
          dataKeyValuePairList.Add(new AIQServerClient.ApplicationDataKeyValuePair("dms-app-name", "WEB_ASSET_ANALYZER"));
        else if (this.btn_LaunchAIQ?.Tag?.ToString() == "audit")
          dataKeyValuePairList.Add(new AIQServerClient.ApplicationDataKeyValuePair("dms-app-name", "WEB_PC_ANALYZER"));
      }
      string pNames = string.Join(",", (IEnumerable<string>) ((IEnumerable<Persona>) userInfo.UserPersonas).Select<Persona, string>((Func<Persona, string>) (personaName => personaName.Name)).ToList<string>());
      AIQServerClient.AIQSessionDetails inputSessionAPI = new AIQServerClient.AIQSessionDetails(new AIQServerClient.AuthRequest("", "", "", "", new AIQServerClient.UserDetail(Session.UserID, userInfo.FirstName, userInfo.LastName, userInfo.Email, pNames)));
      AIQServerClient.LaunchConfigurations LaunchConfigAPI_input = new AIQServerClient.LaunchConfigurations(new AIQServerClient.LaunchConfigurationRequest(this.AIQFolderID, rIDs, new AIQServerClient.ApplicationDataMap(dataKeyValuePairList), appFlags));
      try
      {
        string aiqSiteId = Session.StartupInfo.AIQSiteID;
        string aiqBaseAddress = Session.StartupInfo.AIQBaseAddress;
        AIQServerClient.LaunchResponce launchResponce = aiqServerClient.CallingAIQLaunchAPI(ref aiqSiteId, ref aiqBaseAddress, settingFromCache, inputSessionAPI, LaunchConfigAPI_input);
        if (!(launchResponce.status.ToLower() == "success"))
          throw new Exception(launchResponce.errorMessage);
        Session.StartupInfo.AIQSiteID = aiqSiteId;
        Session.StartupInfo.AIQBaseAddress = aiqBaseAddress;
        string configurationCode = launchResponce.launchConfigurationCode;
        string launchAiqSession = launchResponce.LaunchAIQSession;
        if (!string.IsNullOrWhiteSpace(configurationCode))
          DecrptedLaunchCode = aiqDecryptor.RsaDecryptWithPrivate(configurationCode);
        launchConfiguration = string.Empty;
        if (LaunchType.ToUpper() == AIQButtonHelper.AIQLaunchTypeDesktop)
        {
          if (aiqSoftwareInstall.IsSoftwareInstalled())
          {
            launchConfiguration = aiqServerClient.CreateLaunchURL(LaunchType, settingFromCache, DecrptedLaunchCode, launchAiqSession);
          }
          else
          {
            int num = (int) Utils.Dialog(this.windowOwner, "Unable to find Data & Document Automation desktop app. Launching Data & Document Automation Web.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            launchConfiguration = aiqServerClient.CreateLaunchURL(AIQButtonHelper.AIQLaunchTypeWEB, settingFromCache, DecrptedLaunchCode, launchAiqSession);
          }
        }
        else
        {
          if (LaunchType == AIQButtonHelper.AIQAnalyzerLaunchTypeWEB)
            settingFromCache = Session.SessionObjects.GetCompanySettingFromCache("AIQSetup", "AIQAnalyzerAddress");
          launchConfiguration = aiqServerClient.CreateLaunchURL(LaunchType, settingFromCache, DecrptedLaunchCode, launchAiqSession);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(AIQButtonHelper.sw, TraceLevel.Error, nameof (AIQButtonHelper), string.Format("Error in get Launch Configuration. Exception: {0}", (object) ex.Message));
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Unable to launch Data & Document Automation and Mortgage Analyzers. Please retry. If error persists, please contact Customer Support.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return launchConfiguration;
    }
  }
}
