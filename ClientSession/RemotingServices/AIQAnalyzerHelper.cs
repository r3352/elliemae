// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.AIQAnalyzerHelper
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server.CapsilonAIQ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class AIQAnalyzerHelper
  {
    private const string className = "AIQAnalyzerHelper";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private LoanData loan;

    public AIQAnalyzerHelper(LoanData loanData) => this.loan = loanData;

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

    public AIQIncomeData GetIncomeAnalyzerResultJSON()
    {
      string str1 = Session.SessionObjects.LoanExternalFieldManager.GetFolderID(this.loan.GUID);
      if (string.IsNullOrEmpty(str1))
      {
        string str2 = this.loan.GetField("CX.DV.FOLDERID") ?? "";
        str1 = string.IsNullOrEmpty(str2) ? this.loan.GetField("AIQ.FOLDERID") : str2;
      }
      if (str1 == "")
        throw new Exception("Mortgage Analyzers Folder ID is not set for this loan (check fields CX.DV.FOLDERID and AIQ.FOLDERID).");
      AIQServerClient aiqServerClient = new AIQServerClient(this.GetAPIConfigDetails());
      UserInfo userInfo = Session.User.GetUserInfo();
      List<string> rIDs = new List<string>();
      List<string> appFlags = new List<string>();
      string settingFromCache = Session.SessionObjects.GetCompanySettingFromCache("AIQSetup", "AIQAddress");
      List<AIQServerClient.ApplicationDataKeyValuePair> dataKeyValuePairList = new List<AIQServerClient.ApplicationDataKeyValuePair>();
      dataKeyValuePairList.Add(new AIQServerClient.ApplicationDataKeyValuePair("userId", Session.UserID));
      dataKeyValuePairList.Add(new AIQServerClient.ApplicationDataKeyValuePair("encompassSiteId", Session.StartupInfo.ServerInstanceName));
      dataKeyValuePairList.Add(new AIQServerClient.ApplicationDataKeyValuePair("encompassFolderId", this.loan.GUID));
      string pNames = string.Join(",", (IEnumerable<string>) ((IEnumerable<Persona>) userInfo.UserPersonas).Select<Persona, string>((Func<Persona, string>) (personaName => personaName.Name)).ToList<string>());
      AIQServerClient.AIQSessionDetails inputSessionAPI = new AIQServerClient.AIQSessionDetails(new AIQServerClient.AuthRequest("", "", "", "", new AIQServerClient.UserDetail(Session.UserID, userInfo.FirstName, userInfo.LastName, userInfo.Email, pNames)));
      AIQServerClient.LaunchConfigurations launchConfigurations = new AIQServerClient.LaunchConfigurations(new AIQServerClient.LaunchConfigurationRequest(str1, rIDs, new AIQServerClient.ApplicationDataMap(dataKeyValuePairList), appFlags));
      AIQIncomeData analyzerResultJson;
      try
      {
        string aiqSiteId = Session.StartupInfo.AIQSiteID;
        string aiqBaseAddress = Session.StartupInfo.AIQBaseAddress;
        analyzerResultJson = new AIQIncomeData(this.loan, aiqServerClient.CallingAIQIncomeAnalyzerAPI(ref aiqSiteId, ref aiqBaseAddress, settingFromCache, inputSessionAPI, str1));
        Session.StartupInfo.AIQSiteID = aiqSiteId;
        Session.StartupInfo.AIQBaseAddress = aiqBaseAddress;
      }
      catch (Exception ex)
      {
        Tracing.Log(AIQAnalyzerHelper.sw, TraceLevel.Error, nameof (AIQAnalyzerHelper), string.Format("Error in GetIncomeAnalyzerResultJSON(). Exception: {0}", (object) ex.Message));
        throw new Exception("Error in getting Income Analyzer JSON payload: [" + ex.Message + "]");
      }
      return analyzerResultJson;
    }
  }
}
