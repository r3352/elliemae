// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.DeliverConditionResponseFactory
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Elli.Web.Host;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public static class DeliverConditionResponseFactory
  {
    private static string className = nameof (DeliverConditionResponseFactory);
    private static readonly string sw = Tracing.SwOutsideLoan;
    private static double height = 0.9;
    private static double width = 0.9;
    private static string efsAppModuleURL = string.Empty;
    private static string oApiBaseUrl = string.Empty;
    private static string lockId = string.Empty;

    static DeliverConditionResponseFactory()
    {
      DeliverConditionResponseFactory.lockId = Session.SessionObjects != null ? Session.SessionObjects.SessionID : string.Empty;
      DeliverConditionResponseFactory.efsAppModuleURL = Session.DefaultInstance.StartupInfo.InvestorConnectAppUrl + "/lender/delivery/condition";
      DeliverConditionResponseFactory.oApiBaseUrl = string.IsNullOrEmpty(Session.StartupInfo.OAPIGatewayBaseUri) ? EnConfigurationSettings.AppSettings["oAuth.Url"] : Session.StartupInfo.OAPIGatewayBaseUri;
      new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects)).Execute("apiplatform", (Action<AccessToken>) (accessToken => new DataDocsServiceHelper().GetDimension(accessToken.TypeAndToken, "ui.conditionresponses.dimensions", ref DeliverConditionResponseFactory.width, ref DeliverConditionResponseFactory.height)));
    }

    private static bool CustomDeliverCondResponseTestMode
    {
      get
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return registryKey != null && !string.IsNullOrEmpty((string) registryKey.GetValue("DeliverConditionResponseURL"));
      }
    }

    private static string CustomDeliverCondResponseTestModeURL
    {
      get
      {
        string responseTestModeUrl = "https://encompass.elliemae.com/homepage/em-ssf/demo/modules/appmodule.html";
        if (!DeliverConditionResponseFactory.CustomDeliverCondResponseTestMode)
          return responseTestModeUrl;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return (string) (registryKey.GetValue("DeliverConditionResponseURL") ?? (object) responseTestModeUrl);
      }
    }

    public static Form GetDeliverCondResponseForm(LoanDataMgr loanDataMgr)
    {
      string webPageURL = DeliverConditionResponseFactory.CustomDeliverCondResponseTestMode ? DeliverConditionResponseFactory.CustomDeliverCondResponseTestModeURL : DeliverConditionResponseFactory.efsAppModuleURL;
      Tracing.Log(DeliverConditionResponseFactory.sw, TraceLevel.Info, DeliverConditionResponseFactory.className, string.Format("CustomDeliverConditionResponse TestMode: {0}, Investors Page URL: {1}", (object) DeliverConditionResponseFactory.CustomDeliverCondResponseTestMode, (object) webPageURL));
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      var data = new
      {
        clientId = Session.DefaultInstance.CompanyInfo.ClientID,
        version = Session.DefaultInstance.UserInfo.EncompassVersion,
        loans = new LoanDetailWithLock[1]
        {
          new LoanDetailWithLock(loanDataMgr.LoanData.GUID.ToString().Replace("{", "").Replace("}", ""), loanDataMgr.LoanData.LoanNumber, DeliverConditionResponseFactory.lockId)
        },
        user = new
        {
          id = string.Format("encompass\\{0}\\{1}", (object) Session.DefaultInstance.ServerIdentity.InstanceName, (object) Session.DefaultInstance.UserInfo.Userid)
        }
      };
      string scope = "apiplatform";
      webPageParams.Add("encompass", (object) data);
      webPageParams.Add("errorMessages", (object) new List<string>());
      LoadWebPageForm condResponseForm = new LoadWebPageForm(webPageURL, webPageParams, scope, "Deliver Condition Responses");
      DeliverConditionResponseFactory.setWindowSize((Form) condResponseForm);
      return (Form) condResponseForm;
    }

    private static void setWindowSize(Form form)
    {
      if (Form.ActiveForm != null && Form.ActiveForm.Text != "NotificationForm")
      {
        Form form1 = Form.ActiveForm;
        while (form1.Owner != null)
          form1 = form1.Owner;
        form.Width = Convert.ToInt32((double) form1.Width * DeliverConditionResponseFactory.width);
        form.Height = Convert.ToInt32((double) form1.Height * DeliverConditionResponseFactory.height);
      }
      else
      {
        form.Width = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Width * DeliverConditionResponseFactory.width);
        form.Height = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Height * DeliverConditionResponseFactory.height);
        if (Form.ActiveForm == null || !(Form.ActiveForm.Text == "NotificationForm"))
          return;
        form.StartPosition = FormStartPosition.CenterScreen;
      }
    }
  }
}
