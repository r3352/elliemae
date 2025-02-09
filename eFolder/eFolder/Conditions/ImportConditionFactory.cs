// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.ImportConditionFactory
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Elli.Web.Host;
using Elli.Web.Host.Adapter;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
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
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class ImportConditionFactory
  {
    private string className = nameof (ImportConditionFactory);
    private static readonly string sw = Tracing.SwOutsideLoan;
    private string conditionType;
    private XDocument xDoc;
    private LoanData loanData;
    private List<ConditionLog> lstConditionLog;
    private static string oApiBaseUrl = string.Empty;
    private double height = 0.7;
    private double width = 0.7;
    private string accessToken = string.Empty;
    private static readonly string[] categoryList = new string[6]
    {
      "Assets",
      "Credit",
      "Income",
      "Legal",
      "Misc",
      "Property"
    };
    private static List<string> arrResourceIDs = new List<string>();
    private bool bIsEnchancedCondition;
    private bool bIsEnchancedImportAll;
    private bool success;

    public bool Success => this.success;

    public double Height => this.height;

    public double Width => this.width;

    public ImportConditionFactory(LoanData loanData)
    {
      this.loanData = loanData;
      ImportConditionFactory.oApiBaseUrl = string.IsNullOrEmpty(Session.StartupInfo.OAPIGatewayBaseUri) ? EnConfigurationSettings.AppSettings["oAuth.Url"] : Session.StartupInfo.OAPIGatewayBaseUri;
    }

    public ImportConditionFactory(
      ConditionType conditionType,
      LoanData loanData,
      bool isEnhancedCondition = false,
      bool isEnhancedImportAll = false)
      : this(loanData)
    {
      this.conditionType = conditionType == ConditionType.Unknown ? string.Empty : conditionType.ToString();
      this.bIsEnchancedCondition = isEnhancedCondition;
      this.bIsEnchancedImportAll = isEnhancedImportAll;
      this.success = false;
      new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects)).Execute("apiplatform", (Action<AccessToken>) (accessToken => this.GetDimension(accessToken.TypeAndToken)));
    }

    private bool CustomImportConditionsTestMode
    {
      get
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return registryKey != null && !string.IsNullOrEmpty((string) registryKey.GetValue("ImportConditionURL"));
      }
    }

    public string CustomImportConditionTestModeURL
    {
      get
      {
        string conditionTestModeUrl = "https://encompass.elliemae.com/homepage/em-ssf/demo/modules/appmodule.html";
        if (!this.CustomImportConditionsTestMode)
          return conditionTestModeUrl;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\InvestorConnectSSFGuestTestPage"))
          return (string) (registryKey.GetValue("ImportConditionURL") ?? (object) conditionTestModeUrl);
      }
    }

    public Form GetImportConditionForm()
    {
      string title = "Import Conditions";
      string str = Session.DefaultInstance.StartupInfo.InvestorConnectAppUrl + "/lender/import/condition";
      if (this.bIsEnchancedImportAll)
      {
        str = str.ToLower().Replace("condition", "conditions");
        title = "Importing Conditions";
      }
      string webPageURL = this.CustomImportConditionsTestMode ? this.CustomImportConditionTestModeURL : str;
      Tracing.Log(ImportConditionFactory.sw, TraceLevel.Info, this.className, string.Format("IC_CustomImportCondition TestMode: {0}, Investors Page URL: {1}", (object) this.CustomImportConditionsTestMode, (object) webPageURL));
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      LoanDetail loanDetail = new LoanDetail()
      {
        id = this.loanData.GUID.ToString().Replace("{", "").Replace("}", ""),
        loanNumber = this.loanData.LoanNumber
      };
      if (this.bIsEnchancedCondition)
      {
        loanDetail.lockId = Session.SessionObjects != null ? Session.SessionObjects.SessionID : string.Empty;
        loanDetail.enhancedConditionEnabled = true;
      }
      var data1 = new
      {
        clientId = Session.DefaultInstance.CompanyInfo.ClientID,
        version = Session.DefaultInstance.UserInfo.EncompassVersion,
        loans = new LoanDetail[1]{ loanDetail },
        user = new
        {
          id = string.Format("encompass\\{0}\\{1}", (object) Session.DefaultInstance.ServerIdentity.InstanceName, (object) Session.DefaultInstance.UserInfo.Userid)
        }
      };
      string scope = "apiplatform";
      webPageParams.Add("encompass", (object) data1);
      if (!this.bIsEnchancedCondition)
      {
        var data2 = new
        {
          eFolder = new
          {
            conditionType = this.conditionType
          }
        };
        webPageParams.Add("condition", (object) data2);
      }
      webPageParams.Add("errorMessages", (object) new List<string>());
      LoadWebPageForm importConditionForm = new LoadWebPageForm(webPageURL, webPageParams, scope, title);
      importConditionForm.executeComplete += new LoadWebPageForm.ExecuteComplete(this.form_executeComplete);
      this.setWindowSize((Form) importConditionForm);
      return (Form) importConditionForm;
    }

    public List<ConditionLog> GetAllConditionsToImport()
    {
      string jsonStr = string.Empty;
      try
      {
        new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects)).Execute("apiplatform", (Action<AccessToken>) (accessToken =>
        {
          HttpWebRequest httpWebRequest = this.GetHttpWebRequest(ImportConditionFactory.oApiBaseUrl + string.Format("/investor/v1/loans/{0}/response?resource=conditions&applyRule=Import", (object) this.loanData.GUID.ToString().Replace("{", "").Replace("}", "")));
          httpWebRequest.Method = "GET";
          httpWebRequest.Headers.Add("Authorization", accessToken.TypeAndToken);
          httpWebRequest.ContentType = "application/json";
          using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
            jsonStr = streamReader.ReadToEnd();
        }));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Import failed due to system errors. Please contact ICE Mortgage Technology support for details.", "Import Conditions", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(ImportConditionFactory.sw, TraceLevel.Error, this.className, string.Format("Error in GetAllConditionsToImport. Exception: {0}", (object) ex.Message));
        return (List<ConditionLog>) null;
      }
      if (!string.IsNullOrEmpty(jsonStr))
      {
        this.lstConditionLog = new List<ConditionLog>();
        bool flag = false;
        BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
        JArray jarray1 = JArray.Parse(jsonStr);
        if (jarray1 != null && jarray1.Count > 0)
        {
          foreach (JObject jobject1 in jarray1)
          {
            JArray jarray2 = (JArray) jobject1.GetValue("conditions");
            JToken jtoken1 = jobject1.GetValue("provider");
            if (jarray2 != null && jarray2.Count > 0)
            {
              foreach (JObject jobject2 in jarray2)
              {
                string pairId = string.Empty;
                string empty1 = string.Empty;
                string empty2;
                string lastName1 = empty2 = string.Empty;
                string str1 = empty2;
                string str2 = empty2;
                string lastName2 = empty2;
                string str3 = empty2;
                string str4 = empty2;
                JArray jarray3 = (JArray) jobject2.GetValue("applications");
                if (jarray3 != null && jarray3.Count > 0)
                {
                  foreach (JObject jobject3 in jarray3)
                  {
                    if (jobject3.GetValue("borrower") != null)
                    {
                      JToken jtoken2 = jobject3.GetValue("borrower");
                      str4 = jtoken2[(object) "firstName"] != null ? jtoken2[(object) "firstName"].Value<string>() : string.Empty;
                      str3 = jtoken2[(object) "middleName"] != null ? jtoken2[(object) "middleName"].Value<string>() : string.Empty;
                      lastName2 = jtoken2[(object) "lastName"] != null ? jtoken2[(object) "lastName"].Value<string>() : string.Empty;
                    }
                    if (jobject3.GetValue("coborrower") != null)
                    {
                      JToken jtoken3 = jobject3.GetValue("coborrower");
                      str2 = jtoken3[(object) "firstName"] != null ? jtoken3[(object) "firstName"].Value<string>() : string.Empty;
                      str1 = jtoken3[(object) "middleName"] != null ? jtoken3[(object) "middleName"].Value<string>() : string.Empty;
                      lastName1 = jtoken3[(object) "lastName"] != null ? jtoken3[(object) "lastName"].Value<string>() : string.Empty;
                    }
                    Borrower borrower1 = new Borrower(str4 + (str3 != string.Empty ? " " : "") + str3, lastName2, string.Empty);
                    Borrower borrower2 = new Borrower(str2 + (str1 != string.Empty ? " " : "") + str1, lastName1, string.Empty);
                    if (borrowerPairs != null)
                    {
                      for (int index = 0; index < borrowerPairs.Length; ++index)
                      {
                        if (borrowerPairs[index].Borrower.ToString().Trim().ToLower().Contains(borrower1.ToString().Trim().ToLower()) && borrowerPairs[index].CoBorrower.ToString().Trim().ToLower().Contains(borrower2.ToString().Trim().ToLower()))
                        {
                          if (!flag && !string.IsNullOrEmpty(pairId))
                          {
                            flag = true;
                            break;
                          }
                          pairId = borrowerPairs[index].Id;
                        }
                      }
                    }
                  }
                }
                SellConditionLog sellConditionLog = new SellConditionLog(Session.UserID, pairId);
                if (sellConditionLog != null)
                {
                  sellConditionLog.Title = jobject2.GetValue("title") != null ? jobject2.GetValue("title").ToString() : string.Empty;
                  sellConditionLog.Description = jobject2.GetValue("description") != null ? jobject2.GetValue("description").ToString() : string.Empty;
                  sellConditionLog.Source = jtoken1[(object) "displayName"] != null ? jtoken1[(object) "displayName"].Value<string>() : string.Empty;
                  sellConditionLog.PairId = flag || string.IsNullOrEmpty(pairId) ? BorrowerPair.All.ToString().Trim() : pairId;
                  sellConditionLog.ProviderURN = jobject2.GetValue("providerUrn") != null ? jobject2.GetValue("providerUrn").ToString() : string.Empty;
                  sellConditionLog.Comments.Add(jobject2.GetValue("comments") != null ? jobject2.GetValue("comments").ToString() : string.Empty, "Received - " + sellConditionLog.Source, string.Empty);
                  string str5 = jobject2.GetValue("category") != null ? jobject2.GetValue("category").ToString() : string.Empty;
                  foreach (string category in ImportConditionFactory.categoryList)
                  {
                    if (category.ToLower() == str5.Trim().ToLower())
                    {
                      sellConditionLog.Category = category;
                      break;
                    }
                  }
                  if (sellConditionLog.GetType() == typeof (SellConditionLog))
                  {
                    string str6 = jobject2.GetValue("status") != null ? jobject2.GetValue("status").ToString() : string.Empty;
                    if (str6 == StandardConditionLog.GetStatusString(ConditionStatus.Fulfilled))
                      sellConditionLog.SetStatusFulfilled = true;
                    else if (str6 == StandardConditionLog.GetStatusString(ConditionStatus.Requested))
                      sellConditionLog.SetStatusRequested = true;
                    else if (str6 == StandardConditionLog.GetStatusString(ConditionStatus.Rerequested))
                      sellConditionLog.SetStatusRerequested = true;
                    else if (str6 == StandardConditionLog.GetStatusString(ConditionStatus.Received))
                      sellConditionLog.SetStatusReceived = true;
                    else if (str6 == StandardConditionLog.GetStatusString(ConditionStatus.Reviewed))
                      sellConditionLog.SetStatusReviewed = true;
                    else if (str6 == StandardConditionLog.GetStatusString(ConditionStatus.Rejected))
                      sellConditionLog.SetStatusRejected = true;
                    else if (str6 == StandardConditionLog.GetStatusString(ConditionStatus.Cleared))
                      sellConditionLog.SetStatusCleared = true;
                    else if (str6 == StandardConditionLog.GetStatusString(ConditionStatus.Waived))
                      sellConditionLog.SetStatusWaived = true;
                    if (jobject2.GetValue("priorTo") != null && !string.IsNullOrEmpty(jobject2.GetValue("priorTo").ToString()))
                      sellConditionLog.PriorTo = this.loadPriorToField(jobject2.GetValue("priorTo").ToString());
                    ImportConditionFactory.arrResourceIDs.Add(jobject2.GetValue("id") != null ? jobject2.GetValue("id").ToString() : string.Empty);
                    sellConditionLog.ConditionCode = jobject2.GetValue("code") != null ? jobject2.GetValue("code").ToString() : string.Empty;
                  }
                  this.lstConditionLog.Add((ConditionLog) sellConditionLog);
                }
              }
            }
            else
            {
              int num = (int) MessageBox.Show("There are no conditions to be imported", "Import Conditions", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
          }
        }
        else
        {
          int num1 = (int) MessageBox.Show("There are no conditions to be imported", "Import Conditions", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      return this.lstConditionLog;
    }

    public static void CallResourceImporterAPI(LoanData loandata)
    {
      try
      {
        if (ImportConditionFactory.arrResourceIDs.Count <= 0)
          return;
        JArray jPost = new JArray();
        foreach (string str in ImportConditionFactory.arrResourceIDs.Where<string>((Func<string, bool>) (s => !string.IsNullOrEmpty(s))).Distinct<string>())
        {
          if (!string.IsNullOrEmpty(str))
            jPost.Add((JToken) new JObject()
            {
              {
                "resourceId",
                (JToken) str
              },
              {
                "resourceType",
                (JToken) "condition"
              }
            });
        }
        new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects)).Execute("apiplatform", (Action<AccessToken>) (accessToken =>
        {
          HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(ImportConditionFactory.oApiBaseUrl + string.Format("/investor/v1/loans/{0}/resourceImporter", (object) loandata.GUID.ToString().Replace("{", "").Replace("}", "")));
          httpWebRequest.ServicePoint.Expect100Continue = false;
          httpWebRequest.Method = "POST";
          httpWebRequest.Headers.Add("Authorization", accessToken.TypeAndToken);
          httpWebRequest.ContentType = "application/json";
          byte[] bytes = Encoding.UTF8.GetBytes(jPost.ToString());
          httpWebRequest.ContentLength = (long) bytes.Length;
          using (Stream requestStream = httpWebRequest.GetRequestStream())
            requestStream.Write(bytes, 0, bytes.Length);
          httpWebRequest.GetResponse();
        }));
        ImportConditionFactory.arrResourceIDs.Clear();
      }
      catch (Exception ex)
      {
        Tracing.Log(ImportConditionFactory.sw, TraceLevel.Error, nameof (ImportConditionFactory), string.Format("Error in CallResourceImporterAPI. Exception: {0}", (object) ex.Message));
      }
    }

    public static void ClearImportedResourceIds() => ImportConditionFactory.arrResourceIDs.Clear();

    private void form_executeComplete(object sender, ExecuteCompleteEventArgs e)
    {
      if (this.bIsEnchancedCondition)
      {
        this.success = true;
      }
      else
      {
        this.xDoc = e.jDoc;
        StandardConditionLog standardConditionLog = (StandardConditionLog) null;
        this.lstConditionLog = new List<ConditionLog>();
        bool flag = false;
        BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
        if (this.xDoc.Element((XName) "root") == null || this.xDoc.Element((XName) "root").Element((XName) "conditions") == null || this.xDoc.Element((XName) "root").Element((XName) "conditions").Elements((XName) "item") == null)
          return;
        foreach (XElement element1 in this.xDoc.Element((XName) "root").Element((XName) "conditions").Elements((XName) "item"))
        {
          string str1 = element1.Element((XName) "conditionType").Value;
          string pairId = string.Empty;
          string empty1 = string.Empty;
          string empty2;
          string lastName1 = empty2 = string.Empty;
          string str2 = empty2;
          string str3 = empty2;
          string lastName2 = empty2;
          string str4 = empty2;
          string str5 = empty2;
          foreach (XElement element2 in element1.Element((XName) "borrowerPairs").Elements((XName) "item"))
          {
            if (element2.Element((XName) "borrower") != null)
            {
              str5 = element2.Element((XName) "borrower").Element((XName) "firstName") != null ? Convert.ToString(element2.Element((XName) "borrower").Element((XName) "firstName").Value) : string.Empty;
              str4 = element2.Element((XName) "borrower").Element((XName) "middleName") != null ? Convert.ToString(element2.Element((XName) "borrower").Element((XName) "middleName").Value) : string.Empty;
              lastName2 = element2.Element((XName) "borrower").Element((XName) "lastName") != null ? Convert.ToString(element2.Element((XName) "borrower").Element((XName) "lastName").Value) : string.Empty;
            }
            if (element2.Element((XName) "co-borrower") != null)
            {
              str3 = element2.Element((XName) "co-borrower").Element((XName) "firstName") != null ? Convert.ToString(element2.Element((XName) "co-borrower").Element((XName) "firstName").Value) : string.Empty;
              str2 = element2.Element((XName) "co-borrower").Element((XName) "middleName") != null ? Convert.ToString(element2.Element((XName) "co-borrower").Element((XName) "middleName").Value) : string.Empty;
              lastName1 = element2.Element((XName) "co-borrower").Element((XName) "lastName") != null ? Convert.ToString(element2.Element((XName) "co-borrower").Element((XName) "lastName").Value) : string.Empty;
            }
            Borrower borrower1 = new Borrower(str5 + (str4 != string.Empty ? " " : "") + str4, lastName2, string.Empty);
            Borrower borrower2 = new Borrower(str3 + (str2 != string.Empty ? " " : "") + str2, lastName1, string.Empty);
            if (borrowerPairs != null)
            {
              for (int index = 0; index < borrowerPairs.Length; ++index)
              {
                if (borrowerPairs[index].Borrower.ToString().Trim().ToLower().Contains(borrower1.ToString().Trim().ToLower()) && borrowerPairs[index].CoBorrower.ToString().Trim().ToLower().Contains(borrower2.ToString().Trim().ToLower()))
                {
                  if (!flag && !string.IsNullOrEmpty(pairId))
                  {
                    flag = true;
                    break;
                  }
                  pairId = borrowerPairs[index].Id;
                }
              }
            }
          }
          string lower1 = str1.ToString().ToLower();
          ConditionType conditionType = ConditionType.Underwriting;
          string lower2 = conditionType.ToString().ToLower();
          if (lower1 == lower2)
          {
            standardConditionLog = (StandardConditionLog) new UnderwritingConditionLog(Session.UserID, pairId);
          }
          else
          {
            string lower3 = str1.ToString().ToLower();
            conditionType = ConditionType.Preliminary;
            string lower4 = conditionType.ToString().ToLower();
            if (lower3 == lower4)
            {
              standardConditionLog = (StandardConditionLog) new PreliminaryConditionLog(Session.UserID, pairId);
            }
            else
            {
              string lower5 = str1.ToString().ToLower();
              conditionType = ConditionType.PostClosing;
              string lower6 = conditionType.ToString().ToLower();
              if (lower5 == lower6)
              {
                standardConditionLog = (StandardConditionLog) new PostClosingConditionLog(Session.UserID, pairId);
              }
              else
              {
                string lower7 = str1.ToString().ToLower();
                conditionType = ConditionType.Sell;
                string lower8 = conditionType.ToString().ToLower();
                if (lower7 == lower8)
                  standardConditionLog = (StandardConditionLog) new SellConditionLog(Session.UserID, pairId);
              }
            }
          }
          if (standardConditionLog != null)
          {
            standardConditionLog.Title = element1.Element((XName) "title") != null ? Convert.ToString(element1.Element((XName) "title").Value) : string.Empty;
            standardConditionLog.Description = element1.Element((XName) "description") != null ? Convert.ToString(element1.Element((XName) "description").Value) : string.Empty;
            standardConditionLog.Source = element1.Element((XName) "displayName") != null ? Convert.ToString(element1.Element((XName) "displayName").Value) : string.Empty;
            standardConditionLog.PairId = flag || string.IsNullOrEmpty(pairId) ? BorrowerPair.All.ToString().Trim() : pairId;
            standardConditionLog.ProviderURN = element1.Element((XName) "providerUrn") != null ? Convert.ToString(element1.Element((XName) "providerUrn").Value) : string.Empty;
            standardConditionLog.Comments.Add(element1.Element((XName) "comments") != null ? Convert.ToString(element1.Element((XName) "comments").Value) : string.Empty, "Received - " + standardConditionLog.Source, string.Empty);
            string str6 = element1.Element((XName) "category") != null ? Convert.ToString(element1.Element((XName) "category").Value) : string.Empty;
            foreach (string category in ImportConditionFactory.categoryList)
            {
              if (category.ToLower() == str6.Trim().ToLower())
                standardConditionLog.Category = category;
            }
            if (standardConditionLog.GetType() == typeof (SellConditionLog))
            {
              string str7 = element1.Element((XName) "status") != null ? Convert.ToString(element1.Element((XName) "status").Value) : string.Empty;
              if (str7 == StandardConditionLog.GetStatusString(ConditionStatus.Fulfilled))
                ((SellConditionLog) standardConditionLog).SetStatusFulfilled = true;
              else if (str7 == StandardConditionLog.GetStatusString(ConditionStatus.Requested))
                ((SellConditionLog) standardConditionLog).SetStatusRequested = true;
              else if (str7 == StandardConditionLog.GetStatusString(ConditionStatus.Rerequested))
                ((SellConditionLog) standardConditionLog).SetStatusRerequested = true;
              else if (str7 == StandardConditionLog.GetStatusString(ConditionStatus.Received))
                ((SellConditionLog) standardConditionLog).SetStatusReceived = true;
              else if (str7 == StandardConditionLog.GetStatusString(ConditionStatus.Reviewed))
                ((SellConditionLog) standardConditionLog).SetStatusReviewed = true;
              else if (str7 == StandardConditionLog.GetStatusString(ConditionStatus.Rejected))
                ((SellConditionLog) standardConditionLog).SetStatusRejected = true;
              else if (str7 == StandardConditionLog.GetStatusString(ConditionStatus.Cleared))
                ((SellConditionLog) standardConditionLog).SetStatusCleared = true;
              else if (str7 == StandardConditionLog.GetStatusString(ConditionStatus.Waived))
                ((SellConditionLog) standardConditionLog).SetStatusWaived = true;
              if (element1.Element((XName) "priorTo") != null && !string.IsNullOrEmpty(element1.Element((XName) "priorTo").Value))
                ((SellConditionLog) standardConditionLog).PriorTo = this.loadPriorToField(element1.Element((XName) "priorTo").Value);
              ((SellConditionLog) standardConditionLog).ConditionCode = element1.Element((XName) "code") != null ? element1.Element((XName) "code").Value.ToString() : string.Empty;
              ImportConditionFactory.arrResourceIDs.Add(element1.Element((XName) "id") != null ? element1.Element((XName) "id").Value.ToString() : string.Empty);
            }
            this.lstConditionLog.Add((ConditionLog) standardConditionLog);
          }
        }
      }
    }

    private void GetDimension(string accessToken)
    {
      string str1 = !this.bIsEnchancedImportAll ? "ui.conditions.dimensions" : "ui.conditions.importall.dimensions";
      HttpWebRequest httpWebRequest = this.GetHttpWebRequest(ImportConditionFactory.oApiBaseUrl + "/investor/v1/settings?keys=" + str1);
      httpWebRequest.Method = "GET";
      httpWebRequest.Headers.Add("Authorization", accessToken);
      httpWebRequest.ContentType = "application/json";
      try
      {
        string end;
        using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
          end = streamReader.ReadToEnd();
        List<FormDimension> formDimensionList = JsonConvert.DeserializeObject<List<FormDimension>>(end);
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
        this.width = 0.7;
        this.height = 0.7;
      }
      catch (Exception ex)
      {
        Tracing.Log(ImportConditionFactory.sw, TraceLevel.Error, this.className, string.Format("Error in GetDimension. Exception: {0}", (object) ex.Message));
      }
    }

    private HttpWebRequest GetHttpWebRequest(string apiUrl)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(apiUrl);
      httpWebRequest.ServicePoint.Expect100Continue = false;
      return httpWebRequest;
    }

    public List<ConditionLog> GetResult() => this.lstConditionLog;

    private void setWindowSize(Form form)
    {
      if (Form.ActiveForm != null && Form.ActiveForm.Owner != null)
      {
        Form form1 = Form.ActiveForm;
        while (form1.Owner != null)
          form1 = form1.Owner;
        form.Width = Convert.ToInt32((double) form1.Width * this.width);
        form.Height = Convert.ToInt32((double) form1.Height * this.height);
      }
      else
      {
        Form form2 = form;
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        int int32_1 = Convert.ToInt32((double) workingArea.Width * this.width);
        form2.Width = int32_1;
        Form form3 = form;
        workingArea = Screen.PrimaryScreen.WorkingArea;
        int int32_2 = Convert.ToInt32((double) workingArea.Height * this.height);
        form3.Height = int32_2;
        if (Form.ActiveForm == null || Form.ActiveForm.Owner != null)
          return;
        form.StartPosition = FormStartPosition.CenterScreen;
      }
    }

    private string loadPriorToField(string priorTo)
    {
      string field = string.Empty;
      if (priorTo != null)
      {
        switch (priorTo.ToLower())
        {
          case "approval":
            field = "PTA";
            break;
          case "docs":
            field = "PTD";
            break;
          case "funding":
            field = "PTF";
            break;
          case "closing":
            field = "AC";
            break;
          case "purchase":
            field = "PTP";
            break;
        }
      }
      return field;
    }
  }
}
