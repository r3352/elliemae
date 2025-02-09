// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ImportShippingStatusFactory
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
  public static class ImportShippingStatusFactory
  {
    private static string className = nameof (ImportShippingStatusFactory);
    private static readonly string sw = Tracing.SwOutsideLoan;
    private static LoanDataMgr loanDataMgr = (LoanDataMgr) null;
    private static string lockId = string.Empty;
    private static bool success;
    private static double height = 0.7;
    private static double width = 0.7;
    private static string efsAppModuleURL = string.Empty;

    public static bool Success => ImportShippingStatusFactory.success;

    static ImportShippingStatusFactory()
    {
      ImportShippingStatusFactory.lockId = Session.SessionObjects != null ? Session.SessionObjects.SessionID : string.Empty;
      ImportShippingStatusFactory.efsAppModuleURL = Session.DefaultInstance.StartupInfo.InvestorConnectAppUrl + "/lender/import/shipping";
    }

    private static bool CustomImportShippingStatusTestMode
    {
      get
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return registryKey != null && !string.IsNullOrEmpty((string) registryKey.GetValue("ImportShippingDetailsURL"));
      }
    }

    private static string CustomImportShippingStatusTestModeURL
    {
      get
      {
        string statusTestModeUrl = "https://encompass.elliemae.com/homepage/em-ssf/demo/modules/appmodule.html";
        if (!ImportShippingStatusFactory.CustomImportShippingStatusTestMode)
          return statusTestModeUrl;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return (string) (registryKey.GetValue("ImportShippingDetailsURL") ?? (object) statusTestModeUrl);
      }
    }

    public static Form GetImportShippingStatusForm(LoanDataMgr loandataMgr)
    {
      ImportShippingStatusFactory.loanDataMgr = loandataMgr;
      if (ImportShippingStatusFactory.loanDataMgr.Dirty && !Session.Application.GetService<ILoanConsole>().SaveLoan())
        return (Form) null;
      ImportShippingStatusFactory.success = false;
      string webPageURL = ImportShippingStatusFactory.CustomImportShippingStatusTestMode ? ImportShippingStatusFactory.CustomImportShippingStatusTestModeURL : ImportShippingStatusFactory.efsAppModuleURL;
      Tracing.Log(ImportShippingStatusFactory.sw, TraceLevel.Info, ImportShippingStatusFactory.className, string.Format("CustomImportShippingStatus TestMode: {0}, Investors Page URL: {1}", (object) ImportShippingStatusFactory.CustomImportShippingStatusTestMode, (object) webPageURL));
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      var data = new
      {
        clientId = Session.DefaultInstance.CompanyInfo.ClientID,
        version = Session.DefaultInstance.UserInfo.EncompassVersion,
        loans = new LoanDetail[1]
        {
          new LoanDetail(ImportShippingStatusFactory.loanDataMgr.LoanData.GUID.ToString().Replace("{", "").Replace("}", ""), ImportShippingStatusFactory.loanDataMgr.LoanData.LoanNumber, ImportShippingStatusFactory.lockId)
        },
        user = new
        {
          id = string.Format("encompass\\{0}\\{1}", (object) Session.DefaultInstance.ServerIdentity.InstanceName, (object) Session.DefaultInstance.UserInfo.Userid)
        }
      };
      string scope = "apiplatform";
      webPageParams.Add("encompass", (object) data);
      webPageParams.Add("errorMessages", (object) new List<string>());
      LoadWebPageForm shippingStatusForm = new LoadWebPageForm(webPageURL, webPageParams, scope, "Import Shipping Details");
      shippingStatusForm.executeComplete += new LoadWebPageForm.ExecuteComplete(ImportShippingStatusFactory.form_executeComplete);
      ImportShippingStatusFactory.setWindowSize((Form) shippingStatusForm);
      return (Form) shippingStatusForm;
    }

    private static void form_executeComplete(object sender, ExecuteCompleteEventArgs e)
    {
      try
      {
        if (e.jDoc == null || e.jDoc.Element((XName) "root").Element((XName) "command") == null || !e.jDoc.Element((XName) "root").Element((XName) "command").Value.Trim().ToLower().Equals("refresh_loan"))
          return;
        ImportShippingStatusFactory.loanDataMgr.Refresh(false);
        ImportShippingStatusFactory.success = true;
      }
      catch (Exception ex)
      {
        Tracing.Log(ImportShippingStatusFactory.sw, TraceLevel.Error, ImportShippingStatusFactory.className, "CustomImportShippingStatus failed to refresh loan. Error: " + ex.Message);
        ImportShippingStatusFactory.success = false;
      }
    }

    private static void setWindowSize(Form form)
    {
      if (Form.ActiveForm != null && Form.ActiveForm.Text != "NotificationForm")
      {
        Form form1 = Form.ActiveForm;
        while (form1.Owner != null)
          form1 = form1.Owner;
        form.Width = Convert.ToInt32((double) form1.Width * ImportShippingStatusFactory.width);
        form.Height = Convert.ToInt32((double) form1.Height * ImportShippingStatusFactory.height);
      }
      else
      {
        form.Width = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Width * ImportShippingStatusFactory.width);
        form.Height = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Height * ImportShippingStatusFactory.height);
        if (Form.ActiveForm == null || !(Form.ActiveForm.Text == "NotificationForm"))
          return;
        form.StartPosition = FormStartPosition.CenterScreen;
      }
    }
  }
}
