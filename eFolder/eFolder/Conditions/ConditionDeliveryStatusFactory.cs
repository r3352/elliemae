// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.ConditionDeliveryStatusFactory
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
  public static class ConditionDeliveryStatusFactory
  {
    private static string className = nameof (ConditionDeliveryStatusFactory);
    private static readonly string sw = Tracing.SwOutsideLoan;
    private static double height = 0.8;
    private static double width = 0.8;
    private static string efsAppModuleURL = string.Empty;
    private static string oApiBaseUrl = string.Empty;
    private static string lockId = string.Empty;

    static ConditionDeliveryStatusFactory()
    {
      ConditionDeliveryStatusFactory.lockId = Session.SessionObjects != null ? Session.SessionObjects.SessionID : string.Empty;
      ConditionDeliveryStatusFactory.efsAppModuleURL = Session.DefaultInstance.StartupInfo.InvestorConnectAppUrl + "/lender/delivery/condition/status";
      ConditionDeliveryStatusFactory.oApiBaseUrl = string.IsNullOrEmpty(Session.StartupInfo.OAPIGatewayBaseUri) ? EnConfigurationSettings.AppSettings["oAuth.Url"] : Session.StartupInfo.OAPIGatewayBaseUri;
      new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects)).Execute("apiplatform", (Action<AccessToken>) (accessToken => new DataDocsServiceHelper().GetDimension(accessToken.TypeAndToken, "ui.conditionstatuses.dimensions", ref ConditionDeliveryStatusFactory.width, ref ConditionDeliveryStatusFactory.height)));
    }

    private static bool CustomConditionDeliveryStatusTestMode
    {
      get
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return registryKey != null && !string.IsNullOrEmpty((string) registryKey.GetValue("ConditionDeliveryStatusURL"));
      }
    }

    private static string CustomConditionDeliveryStatusTestModeURL
    {
      get
      {
        string statusTestModeUrl = "https://encompass.elliemae.com/homepage/em-ssf/demo/modules/appmodule.html";
        if (!ConditionDeliveryStatusFactory.CustomConditionDeliveryStatusTestMode)
          return statusTestModeUrl;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return (string) (registryKey.GetValue("ConditionDeliveryStatusURL") ?? (object) statusTestModeUrl);
      }
    }

    public static Form GetConditionDeliveryStatusForm(LoanDataMgr loanDataMgr)
    {
      string webPageURL = ConditionDeliveryStatusFactory.CustomConditionDeliveryStatusTestMode ? ConditionDeliveryStatusFactory.CustomConditionDeliveryStatusTestModeURL : ConditionDeliveryStatusFactory.efsAppModuleURL;
      Tracing.Log(ConditionDeliveryStatusFactory.sw, TraceLevel.Info, ConditionDeliveryStatusFactory.className, string.Format("CustomConditionDeliveryStatus TestMode: {0}, Investors Page URL: {1}", (object) ConditionDeliveryStatusFactory.CustomConditionDeliveryStatusTestMode, (object) webPageURL));
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      var data = new
      {
        clientId = Session.DefaultInstance.CompanyInfo.ClientID,
        version = Session.DefaultInstance.UserInfo.EncompassVersion,
        loans = new LoanDetailWithLock[1]
        {
          new LoanDetailWithLock(loanDataMgr.LoanData.GUID.ToString().Replace("{", "").Replace("}", ""), loanDataMgr.LoanData.LoanNumber, ConditionDeliveryStatusFactory.lockId)
        },
        user = new
        {
          id = string.Format("encompass\\{0}\\{1}", (object) Session.DefaultInstance.ServerIdentity.InstanceName, (object) Session.DefaultInstance.UserInfo.Userid)
        }
      };
      string scope = "apiplatform";
      webPageParams.Add("encompass", (object) data);
      webPageParams.Add("errorMessages", (object) new List<string>());
      LoadWebPageForm deliveryStatusForm = new LoadWebPageForm(webPageURL, webPageParams, scope, "Condition Delivery Status for Loan# " + loanDataMgr.LoanData.LoanNumber);
      ConditionDeliveryStatusFactory.setWindowSize((Form) deliveryStatusForm);
      return (Form) deliveryStatusForm;
    }

    private static void setWindowSize(Form form)
    {
      if (Form.ActiveForm != null && Form.ActiveForm.Text != "NotificationForm")
      {
        Form form1 = Form.ActiveForm;
        while (form1.Owner != null)
          form1 = form1.Owner;
        form.Width = Convert.ToInt32((double) form1.Width * ConditionDeliveryStatusFactory.width);
        form.Height = Convert.ToInt32((double) form1.Height * ConditionDeliveryStatusFactory.height);
      }
      else
      {
        form.Width = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Width * ConditionDeliveryStatusFactory.width);
        form.Height = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Height * ConditionDeliveryStatusFactory.height);
        if (Form.ActiveForm == null || !(Form.ActiveForm.Text == "NotificationForm"))
          return;
        form.StartPosition = FormStartPosition.CenterScreen;
      }
    }
  }
}
