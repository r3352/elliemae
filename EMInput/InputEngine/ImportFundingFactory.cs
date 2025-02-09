// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ImportFundingFactory
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using Elli.Web.Host;
using Elli.Web.Host.Adapter;
using EllieMae.EMLite.Common;
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
namespace EllieMae.EMLite.InputEngine
{
  public static class ImportFundingFactory
  {
    private static string className = nameof (ImportFundingFactory);
    private static readonly string sw = Tracing.SwOutsideLoan;
    private static LoanDataMgr loanDataMgr = (LoanDataMgr) null;
    private static string lockId = string.Empty;
    private static bool success;
    private static double height = 0.7;
    private static double width = 0.7;
    private static string efsAppModuleURL = string.Empty;

    public static bool Success => ImportFundingFactory.success;

    static ImportFundingFactory()
    {
      ImportFundingFactory.lockId = Session.SessionObjects != null ? Session.SessionObjects.SessionID : string.Empty;
      ImportFundingFactory.efsAppModuleURL = Session.DefaultInstance.StartupInfo.InvestorConnectAppUrl + "/lender/import/funding";
    }

    private static bool CustomImportFundingTestMode
    {
      get
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return registryKey != null && !string.IsNullOrEmpty((string) registryKey.GetValue("ImportFundingDetailsURL"));
      }
    }

    private static string CustomImportFundingTestModeURL
    {
      get
      {
        string fundingTestModeUrl = "https://encompass.elliemae.com/homepage/em-ssf/demo/modules/appmodule.html";
        if (!ImportFundingFactory.CustomImportFundingTestMode)
          return fundingTestModeUrl;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return (string) (registryKey.GetValue("ImportFundingDetailsURL") ?? (object) fundingTestModeUrl);
      }
    }

    public static Form GetImportFundingForm(LoanDataMgr loandataMgr)
    {
      ImportFundingFactory.loanDataMgr = loandataMgr;
      if (ImportFundingFactory.loanDataMgr.Dirty && !Session.Application.GetService<ILoanConsole>().SaveLoan())
        return (Form) null;
      ImportFundingFactory.success = false;
      string webPageURL = ImportFundingFactory.CustomImportFundingTestMode ? ImportFundingFactory.CustomImportFundingTestModeURL : ImportFundingFactory.efsAppModuleURL;
      Tracing.Log(ImportFundingFactory.sw, TraceLevel.Info, ImportFundingFactory.className, string.Format("CustomImportFunding TestMode: {0}, Investors Page URL: {1}", (object) ImportFundingFactory.CustomImportFundingTestMode, (object) webPageURL));
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      var data = new
      {
        clientId = Session.DefaultInstance.CompanyInfo.ClientID,
        version = Session.DefaultInstance.UserInfo.EncompassVersion,
        loans = new LoanDetail[1]
        {
          new LoanDetail(ImportFundingFactory.loanDataMgr.LoanData.GUID.ToString().Replace("{", "").Replace("}", ""), ImportFundingFactory.loanDataMgr.LoanData.LoanNumber, ImportFundingFactory.lockId)
        },
        user = new
        {
          id = string.Format("encompass\\{0}\\{1}", (object) Session.DefaultInstance.ServerIdentity.InstanceName, (object) Session.DefaultInstance.UserInfo.Userid)
        }
      };
      string scope = "apiplatform";
      webPageParams.Add("encompass", (object) data);
      webPageParams.Add("errorMessages", (object) new List<string>());
      LoadWebPageForm importFundingForm = new LoadWebPageForm(webPageURL, webPageParams, scope, "Import Funding Details");
      importFundingForm.executeComplete += new LoadWebPageForm.ExecuteComplete(ImportFundingFactory.form_executeComplete);
      ImportFundingFactory.setWindowSize((Form) importFundingForm);
      return (Form) importFundingForm;
    }

    private static void form_executeComplete(object sender, ExecuteCompleteEventArgs e)
    {
      try
      {
        if (e.jDoc == null || e.jDoc.Element((XName) "root").Element((XName) "command") == null || !e.jDoc.Element((XName) "root").Element((XName) "command").Value.Trim().ToLower().Equals("refresh_loan"))
          return;
        ImportFundingFactory.loanDataMgr.Refresh(false);
        ImportFundingFactory.success = true;
      }
      catch (Exception ex)
      {
        Tracing.Log(ImportFundingFactory.sw, TraceLevel.Error, ImportFundingFactory.className, "CustomImportFundingDetails failed to refresh loan. Error: " + ex.Message);
        ImportFundingFactory.success = false;
      }
    }

    private static void setWindowSize(Form form)
    {
      if (Form.ActiveForm != null && Form.ActiveForm.Text != "NotificationForm")
      {
        Form form1 = Form.ActiveForm;
        while (form1.Owner != null)
          form1 = form1.Owner;
        form.Width = Convert.ToInt32((double) form1.Width * ImportFundingFactory.width);
        form.Height = Convert.ToInt32((double) form1.Height * ImportFundingFactory.height);
      }
      else
      {
        form.Width = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Width * ImportFundingFactory.width);
        form.Height = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Height * ImportFundingFactory.height);
        if (Form.ActiveForm == null || !(Form.ActiveForm.Text == "NotificationForm"))
          return;
        form.StartPosition = FormStartPosition.CenterScreen;
      }
    }
  }
}
