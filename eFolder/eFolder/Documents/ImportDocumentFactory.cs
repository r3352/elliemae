// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.ImportDocumentFactory
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Elli.Web.Host;
using Elli.Web.Host.Adapter;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public static class ImportDocumentFactory
  {
    private static string className = nameof (ImportDocumentFactory);
    private static readonly string sw = Tracing.SwOutsideLoan;
    private static string lockId = string.Empty;
    private static bool success;
    private static double height = 0.9;
    private static double width = 0.9;
    private static string efsAppModuleURL = string.Empty;
    private static string oApiBaseUrl = string.Empty;
    private static LoanDataMgr loanDataMgr = (LoanDataMgr) null;

    public static bool Success => ImportDocumentFactory.success;

    static ImportDocumentFactory()
    {
      ImportDocumentFactory.lockId = Session.SessionObjects != null ? Session.SessionObjects.SessionID : string.Empty;
      ImportDocumentFactory.efsAppModuleURL = Session.DefaultInstance.StartupInfo.InvestorConnectAppUrl + "/lender/import/documents";
      ImportDocumentFactory.oApiBaseUrl = string.IsNullOrEmpty(Session.StartupInfo.OAPIGatewayBaseUri) ? EnConfigurationSettings.AppSettings["oAuth.Url"] : Session.StartupInfo.OAPIGatewayBaseUri;
      new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects)).Execute("apiplatform", (Action<AccessToken>) (accessToken => new DataDocsServiceHelper().GetDimension(accessToken.TypeAndToken, "ui.documents.dimensions", ref ImportDocumentFactory.width, ref ImportDocumentFactory.height)));
    }

    private static bool CustomImportDocumentTestMode
    {
      get
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return registryKey != null && !string.IsNullOrEmpty((string) registryKey.GetValue("ImportDocumentURL"));
      }
    }

    private static string CustomImportDocumentTestModeURL
    {
      get
      {
        string documentTestModeUrl = "https://encompass.elliemae.com/homepage/em-ssf/demo/modules/appmodule.html";
        if (!ImportDocumentFactory.CustomImportDocumentTestMode)
          return documentTestModeUrl;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return (string) (registryKey.GetValue("ImportDocumentURL") ?? (object) documentTestModeUrl);
      }
    }

    public static Form GetImportDocumentForm(LoanDataMgr loandataMgr)
    {
      ImportDocumentFactory.loanDataMgr = loandataMgr;
      if (ImportDocumentFactory.loanDataMgr.Dirty && !Session.Application.GetService<ILoanConsole>().SaveLoan())
        return (Form) null;
      ImportDocumentFactory.success = false;
      string webPageURL = ImportDocumentFactory.CustomImportDocumentTestMode ? ImportDocumentFactory.CustomImportDocumentTestModeURL : ImportDocumentFactory.efsAppModuleURL;
      Tracing.Log(ImportDocumentFactory.sw, TraceLevel.Info, ImportDocumentFactory.className, string.Format("CustomImportDocument TestMode: {0}, Investors Page URL: {1}", (object) ImportDocumentFactory.CustomImportDocumentTestMode, (object) webPageURL));
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      var data = new
      {
        clientId = Session.DefaultInstance.CompanyInfo.ClientID,
        version = Session.DefaultInstance.UserInfo.EncompassVersion,
        loans = new LoanDetail[1]
        {
          new LoanDetail(loandataMgr.LoanData.GUID.ToString().Replace("{", "").Replace("}", ""), loandataMgr.LoanData.LoanNumber, ImportDocumentFactory.lockId)
        },
        user = new
        {
          id = string.Format("encompass\\{0}\\{1}", (object) Session.DefaultInstance.ServerIdentity.InstanceName, (object) Session.DefaultInstance.UserInfo.Userid)
        }
      };
      string scope = "apiplatform";
      webPageParams.Add("encompass", (object) data);
      webPageParams.Add("errorMessages", (object) new List<string>());
      LoadWebPageForm importDocumentForm = new LoadWebPageForm(webPageURL, webPageParams, scope, "Import Document Details");
      importDocumentForm.executeComplete += new LoadWebPageForm.ExecuteComplete(ImportDocumentFactory.form_executeComplete);
      ImportDocumentFactory.setWindowSize((Form) importDocumentForm);
      return (Form) importDocumentForm;
    }

    private static void form_executeComplete(object sender, ExecuteCompleteEventArgs e)
    {
      try
      {
        if (e.jDoc == null || e.jDoc.Element((XName) "root").Element((XName) "command") == null || !e.jDoc.Element((XName) "root").Element((XName) "command").Value.Trim().ToLower().Equals("refresh_loan"))
          return;
        ImportDocumentFactory.loanDataMgr.Refresh(false);
        ImportDocumentFactory.success = true;
      }
      catch (Exception ex)
      {
        Tracing.Log(ImportDocumentFactory.sw, TraceLevel.Error, ImportDocumentFactory.className, "CustomImportDocumentDetails failed to refresh loan. Error: " + ex.Message);
        ImportDocumentFactory.success = false;
      }
    }

    private static void setWindowSize(Form form)
    {
      if (Form.ActiveForm != null && Form.ActiveForm.Text != "NotificationForm")
      {
        Form form1 = Form.ActiveForm;
        while (form1.Owner != null)
          form1 = form1.Owner;
        form.Width = Convert.ToInt32((double) form1.Width * ImportDocumentFactory.width);
        form.Height = Convert.ToInt32((double) form1.Height * ImportDocumentFactory.height);
      }
      else
      {
        form.Width = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Width * ImportDocumentFactory.width);
        form.Height = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Height * ImportDocumentFactory.height);
        if (Form.ActiveForm == null || !(Form.ActiveForm.Text == "NotificationForm"))
          return;
        form.StartPosition = FormStartPosition.CenterScreen;
      }
    }
  }
}
