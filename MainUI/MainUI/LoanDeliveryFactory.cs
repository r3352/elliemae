// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.LoanDeliveryFactory
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Web.Host;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  internal class LoanDeliveryFactory
  {
    private string enableCustomLoanSubmissionStatusFlow;
    private string className = nameof (LoanDeliveryFactory);
    private static readonly string sw = Tracing.SwOutsideLoan;
    private string oApiBaseUrl = string.Empty;
    private double height = 0.95;
    private double width = 0.95;

    public LoanDeliveryFactory(LoanDeliveryFactory.FormType formType)
    {
      LoanDeliveryFactory loanDeliveryFactory = this;
      this.enableCustomLoanSubmissionStatusFlow = SmartClientUtils.GetAttribute(Session.CompanyInfo.ClientID, "Encompass.exe", "IC_CustomLoanSubmissionStatusFlow");
      this.oApiBaseUrl = string.IsNullOrEmpty(Session.StartupInfo.OAPIGatewayBaseUri) ? EnConfigurationSettings.AppSettings["oAuth.Url"] : Session.StartupInfo.OAPIGatewayBaseUri;
      new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects)).Execute("apiplatform", (Action<AccessToken>) (accessToken => loanDeliveryFactory.GetDimension(accessToken.TypeAndToken, formType)));
    }

    private bool EnableCustomLoanSubmissionStatusFlow
    {
      get
      {
        return !string.IsNullOrWhiteSpace(this.enableCustomLoanSubmissionStatusFlow) && this.enableCustomLoanSubmissionStatusFlow == "1";
      }
    }

    private bool LoanDeliveryTestMode
    {
      get
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\IC_CustomLoanDeliveryTestMode"))
          return registryKey != null;
      }
    }

    public string CustomLoanDeliveryTestModeURL
    {
      get
      {
        string deliveryTestModeUrl = "https://encompass.elliemae.com/homepage/em-ssf/demo/modules/appmodule.html";
        if (!this.LoanDeliveryTestMode)
          return deliveryTestModeUrl;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\IC_CustomLoanDeliveryTestMode"))
          return (string) (registryKey.GetValue("url") ?? (object) deliveryTestModeUrl);
      }
    }

    private bool CustomLoanSubmissionStatusTestMode
    {
      get
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\IC_CustomLoanSubmissionStatusTestMode"))
          return registryKey != null;
      }
    }

    public string CustomLoanSubmissionStatusTestModeURL
    {
      get
      {
        string statusTestModeUrl = "https://encompass.elliemae.com/homepage/em-ssf/demo/modules/appmodule.html";
        if (!this.CustomLoanSubmissionStatusTestMode)
          return statusTestModeUrl;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\IC_CustomLoanSubmissionStatusTestMode"))
          return (string) (registryKey.GetValue("url") ?? (object) statusTestModeUrl);
      }
    }

    public Form GetLoanDeliveryServicesForm(
      List<Loan> lstLoans,
      Size parentSize,
      JObject filterParam,
      SortField[] sortFields,
      int filteredLoanCounts)
    {
      string str = Session.DefaultInstance.StartupInfo.InvestorConnectAppUrl + "/lender/delivery/loan";
      string webPageURL = this.LoanDeliveryTestMode ? this.CustomLoanDeliveryTestModeURL : str;
      Tracing.Log(LoanDeliveryFactory.sw, TraceLevel.Info, this.className, string.Format("TestMode: {0}, Investors Page URL: {1}", (object) this.LoanDeliveryTestMode, (object) webPageURL));
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      IEnumerable<\u003C\u003Ef__AnonymousType3<string, string>> datas1 = lstLoans.Select(loan => new
      {
        id = loan.entityRef.EntityId,
        loanNumber = loan.entityRef.LoanNumber
      });
      IEnumerable<\u003C\u003Ef__AnonymousType4<string, string>> datas2 = sortFields != null ? ((IEnumerable<SortField>) sortFields).Select(sort => new
      {
        canonicalName = sort.Term.FieldName,
        order = sort.SortOrder.ToString()
      }) : null;
      var data = new
      {
        version = Session.DefaultInstance.UserInfo.EncompassVersion,
        edition = Session.EncompassEdition.ToString(),
        pipeline = new
        {
          filteredLoanCount = Convert.ToString(filteredLoanCounts),
          filter = filterParam,
          sortOrder = datas2
        },
        loans = datas1,
        user = new
        {
          id = string.Format("encompass\\{0}\\{1}", (object) Session.DefaultInstance.ServerIdentity.InstanceName, (object) Session.DefaultInstance.UserInfo.Userid)
        }
      };
      string scope = "sc";
      webPageParams.Add("encompass", (object) data);
      webPageParams.Add("errorMessages", (object) new List<string>());
      LoadWebPageForm deliveryServicesForm = new LoadWebPageForm(webPageURL, webPageParams, scope, "Loan Delivery Services");
      deliveryServicesForm.Height = Convert.ToInt32(this.height * (double) parentSize.Height);
      deliveryServicesForm.Width = Convert.ToInt32(this.width * (double) parentSize.Width);
      return (Form) deliveryServicesForm;
    }

    public Form GetSubmissionStatusForm(Size parentSize)
    {
      Tracing.Log(LoanDeliveryFactory.sw, TraceLevel.Info, this.className, string.Format("IC_CustomLoanSubmissionStatusFlow value: {0}", (object) this.enableCustomLoanSubmissionStatusFlow));
      if (!this.EnableCustomLoanSubmissionStatusFlow)
        return (Form) new DataDocsSubmissionStatus(Session.DefaultInstance);
      string str = Session.DefaultInstance.StartupInfo.InvestorConnectAppUrl + "/lender/delivery/loan/status";
      string webPageURL = this.CustomLoanSubmissionStatusTestMode ? this.CustomLoanSubmissionStatusTestModeURL : str;
      Tracing.Log(LoanDeliveryFactory.sw, TraceLevel.Info, this.className, string.Format("TestMode: {0}, Investors Page URL: {1}", (object) this.CustomLoanSubmissionStatusTestMode, (object) webPageURL));
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      string scope = "sc";
      var data = new
      {
        user = new
        {
          id = string.Format("encompass\\{0}\\{1}", (object) Session.DefaultInstance.ServerIdentity.InstanceName, (object) Session.DefaultInstance.UserInfo.Userid)
        },
        version = Session.DefaultInstance.UserInfo.EncompassVersion
      };
      webPageParams.Add("encompass", (object) data);
      webPageParams.Add("errorMessages", (object) new List<string>());
      LoadWebPageForm submissionStatusForm = new LoadWebPageForm(webPageURL, webPageParams, scope, "Loan Delivery Status");
      submissionStatusForm.Height = Convert.ToInt32(this.height * (double) parentSize.Height);
      submissionStatusForm.Width = Convert.ToInt32(this.width * (double) parentSize.Width);
      return (Form) submissionStatusForm;
    }

    private void GetDimension(string accessToken, LoanDeliveryFactory.FormType formType)
    {
      string empty = string.Empty;
      string str1;
      switch (formType)
      {
        case LoanDeliveryFactory.FormType.LoanDeliveryStatus:
          str1 = "/investor/v1/settings?keys=ui.loandeliverystatus.dimensions";
          break;
        case LoanDeliveryFactory.FormType.LoanDeliveryServices:
          str1 = "/investor/v1/settings?keys=ui.loandeliveryservices.dimensions";
          break;
        default:
          str1 = "/investor/v1/settings?keys=ui.loandeliveryservices.dimensions";
          break;
      }
      HttpWebRequest httpWebRequest = this.GetHttpWebRequest(this.oApiBaseUrl + str1);
      httpWebRequest.Method = "GET";
      httpWebRequest.Headers.Add("Authorization", accessToken);
      httpWebRequest.ContentType = "application/json";
      try
      {
        string end;
        using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
          end = streamReader.ReadToEnd();
        List<LoanDeliveryFactory.FormDimension> formDimensionList = JsonConvert.DeserializeObject<List<LoanDeliveryFactory.FormDimension>>(end);
        if (formDimensionList == null || formDimensionList.Count <= 0)
          return;
        string str2 = formDimensionList[0].Value;
        if (string.IsNullOrEmpty(str2) || !str2.ToLower().Contains<char>('x'))
          return;
        string[] strArray = str2.ToLower().Split('x');
        if (strArray.Length == 0)
          return;
        this.width = Convert.ToDouble(strArray[0]) / 100.0;
        this.height = Convert.ToDouble(strArray[1]) / 100.0;
        if (this.width != 0.0 && this.height != 0.0)
          return;
        this.width = 0.9;
        this.height = 0.9;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDeliveryFactory.sw, TraceLevel.Error, this.className, string.Format("Error in GetDimension. Exception: {0}", (object) ex.Message));
      }
    }

    private HttpWebRequest GetHttpWebRequest(string apiUrl)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(apiUrl);
      httpWebRequest.ServicePoint.Expect100Continue = false;
      return httpWebRequest;
    }

    public enum FormType
    {
      LoanDeliveryStatus = 1,
      LoanDeliveryServices = 2,
    }

    private class FormDimension
    {
      [JsonProperty("key")]
      public string Key { get; set; }

      [JsonProperty("value")]
      public string Value { get; set; }

      [JsonProperty("description")]
      public string Description { get; set; }
    }
  }
}
