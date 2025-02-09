// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineService
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.DocEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class DocEngineService : IDisposable
  {
    private const string className = "DocEngineService�";
    private static string sw = Tracing.SwEpass;
    private SessionObjects sessionObjects;
    private DocService docService;
    [Obsolete("Read from Web.config instead")]
    public const string EDS_DEFAULT_URL = "https://docsengine.elliemae.com/EMDocs/ws.asmx�";
    public const string EDS_SETTINGS_SECTION = "EDS�";
    public const string EDS_SETTINGS_KEY_WSURL = "DocsWsUrl�";
    public string[] EntityList;
    public const string DATEFORMAT = "yyyy-MM-dd HH:mm:ss�";
    public const string PLANS_UNCHANGED_MESSAGE = "Plans Unchanged�";
    public const string PLANS_UTC_PARAMNAME = "PlansLastDownloadedUtc�";
    private static object _lockObject = new object();
    private static Dictionary<string, StandardPlan[]> _edsPlansCache = new Dictionary<string, StandardPlan[]>();
    private static DateTime _edsPlansLastDownloadedUtc = DateTime.MinValue;

    public DocEngineService(SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      string ssoToken = (string) null;
      try
      {
        if (sessionObjects.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
          ssoToken = sessionObjects.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
          {
            "Elli.Eds"
          }, 5);
      }
      catch (Exception ex)
      {
        Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Failed to retrieve the security token: " + (object) ex);
      }
      try
      {
        string docServiceUrl = this.GetDocServiceUrl(sessionObjects?.StartupInfo?.ServiceUrls?.DocServiceUrl);
        this.docService = new DocService(ssoToken, docServiceUrl);
      }
      catch (Exception ex)
      {
        Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Failed to instantiate DocEngine web service: " + (object) ex);
        throw;
      }
    }

    private string GetDocServiceUrl(string docServiceUrlDefault)
    {
      string docServiceUrl = docServiceUrlDefault;
      string str1 = "default URL";
      try
      {
        string str2 = "user profile";
        string privateProfileString = this.sessionObjects.CurrentUser.GetPrivateProfileString("EDS", "DocsWsUrl");
        string str3 = privateProfileString == null ? string.Empty : privateProfileString.Trim();
        if (str3 == string.Empty)
        {
          str2 = "company settings";
          string companySetting = this.sessionObjects.ConfigurationManager.GetCompanySetting("EDS", "DocsWsUrl");
          str3 = companySetting == null ? string.Empty : companySetting.Trim();
        }
        if (str3 != string.Empty)
        {
          docServiceUrl = str3;
          str1 = str2;
        }
      }
      catch (Exception ex)
      {
        docServiceUrl = docServiceUrlDefault;
        Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Error executing GetDocServiceUrl (default URL returned): " + (object) ex);
      }
      string msg = string.Format("DocServiceUrl {{'source': '{0}', 'resultUrl': '{1}', 'defaultUrl': '{2}'}}", (object) str1, (object) docServiceUrl, (object) docServiceUrlDefault);
      Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Verbose, msg);
      return docServiceUrl;
    }

    public DocEngineClientAccountStatus GetClientAccountStatus(bool forceSendData = false)
    {
      DocEngineRequest req = new DocEngineRequest(this.sessionObjects, "GetClientClosingDocsStatus");
      try
      {
        this.finalizeRequest(req, forceSendData);
        return DocEngineClientAccountStatus.Extract(this.parseResponse(this.docService.GetClientClosingDocsStatus(req.ToString())));
      }
      catch (DocServiceLoginException ex)
      {
        return DocEngineClientAccountStatus.NoAccess;
      }
      catch (Exception ex)
      {
        Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Error executing GetClientAccountStatus request for DocEngineService: " + (object) ex);
        throw;
      }
    }

    public Investor[] GetInvestors(DocumentOrderType orderType, bool includeGenericInvestor)
    {
      DocEngineRequest req = new DocEngineRequest(this.sessionObjects, "GetInvestorList");
      if (orderType != DocumentOrderType.Both && orderType != DocumentOrderType.None)
        req.SetParameter("PlanCodeType", orderType.ToString());
      try
      {
        this.finalizeRequest(req);
        List<Investor> investorList = new List<Investor>((IEnumerable<Investor>) Investor.ExtractCollection(this.parseResponse(this.docService.GetInvestorList(req.ToString()))));
        if (includeGenericInvestor)
          investorList.Add(Investor.GenericInvestor);
        return investorList.ToArray();
      }
      catch (Exception ex)
      {
        Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Error executing GetInvestors request for DocEngineService: " + (object) ex);
        throw;
      }
    }

    public StandardPlan[] GetPlansForInvestor(string investorCode)
    {
      DocEngineRequest req = new DocEngineRequest(this.sessionObjects, "GetInvestorPlancodeList");
      req.SetParameter("InvestorCode", investorCode);
      try
      {
        this.finalizeRequest(req);
        return StandardPlan.ExtractCollection(this.parseResponse(this.docService.GetInvestorPlancodeList(req.ToString())));
      }
      catch (Exception ex)
      {
        Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Error executing GetInvestorPlancodeList request for DocEngineService: " + (object) ex);
        throw;
      }
    }

    public Dictionary<FormItemInfo, byte[]> MergeNative(
      LoanData loanData,
      List<FormItemInfo> edsFormsRequired,
      string printOption,
      string correlationID,
      out string edsRequestGuid,
      out string processingErrors)
    {
      Dictionary<FormItemInfo, byte[]> dictionary = new Dictionary<FormItemInfo, byte[]>();
      processingErrors = string.Empty;
      try
      {
        DocEngineRequest req = new DocEngineRequest(this.sessionObjects, nameof (MergeNative), correlationID);
        XmlDocument requestXml = req.RequestXml;
        edsRequestGuid = "";
        if (requestXml != null)
        {
          edsRequestGuid = requestXml.DocumentElement.GetAttribute("RequestGuid");
          XmlElement xmlElement1 = (XmlElement) requestXml.DocumentElement.SelectSingleNode("REQUEST_OPTIONS") ?? (XmlElement) requestXml.DocumentElement.AppendChild((XmlNode) requestXml.CreateElement("REQUEST_OPTIONS"));
          xmlElement1.SetAttribute("OrderType", "Preliminary");
          xmlElement1.SetAttribute("BorrowerPairIndex", loanData.GetPairIndex(loanData.PairId).ToString());
          switch (printOption)
          {
            case "BlankForm":
              xmlElement1.SetAttribute("MergeBlank", "true");
              xmlElement1.SetAttribute("DisplayFieldIds", "false");
              break;
            case "WithFieldIDs":
              xmlElement1.SetAttribute("DisplayFieldIds", "true");
              xmlElement1.SetAttribute("MergeBlank", "false");
              break;
            case "WithData":
            case "WithSignatures":
            case "Unknown":
              xmlElement1.SetAttribute("DisplayFieldIds", "false");
              xmlElement1.SetAttribute("MergeBlank", "false");
              break;
          }
          xmlElement1.SetAttribute("LoanIsPreCalculated", "true");
          XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) requestXml.CreateElement("MERGE_ON_DEMAND"));
          string str1 = this.EntityList == null || this.EntityList.Length == 0 ? string.Empty : string.Join(",", this.EntityList);
          for (int index = 0; index < edsFormsRequired.Count; ++index)
          {
            FormItemInfo formItemInfo = edsFormsRequired[index];
            string mergeParam = formItemInfo.MergeParams["TemplateName"];
            XmlElement xmlElement3 = (XmlElement) xmlElement2.AppendChild((XmlNode) requestXml.CreateElement("FORM"));
            xmlElement3.SetAttribute("Name", mergeParam);
            xmlElement3.SetAttribute("NumCopies", "1");
            xmlElement3.SetAttribute("Comment", "Merge Native Request from Encompass");
            if (!string.IsNullOrWhiteSpace(formItemInfo.SignatureType))
            {
              xmlElement3.SetAttribute("SigningMode", formItemInfo.SignatureType);
              formItemInfo.MergeParams["SigningMode"] = formItemInfo.SignatureType.ToString();
            }
            int? templatePageNumber = this.ParseEdsTemplatePageNumber(mergeParam);
            int num;
            if (templatePageNumber.HasValue)
            {
              MergeParamValues mergeParams = formItemInfo.MergeParams;
              num = templatePageNumber.Value;
              string str2 = num.ToString();
              mergeParams["PageNumber"] = str2;
            }
            MergeParamValues mergeParams1 = formItemInfo.MergeParams;
            num = index + 1;
            string str3 = num.ToString("000");
            mergeParams1["SerialNumber"] = str3;
            if (str1 != string.Empty)
              formItemInfo.MergeParams["EntityList"] = str1;
            XmlElement xml = formItemInfo.MergeParams.ToXml();
            if (xml != null)
              xmlElement3.AppendChild(xmlElement3.OwnerDocument.ImportNode((XmlNode) xml, true));
          }
          loanData.IncludeSnapshotInXML = true;
          loanData.GetCalculationDTSnapshotForLoan();
          ((XmlElement) requestXml.DocumentElement.AppendChild((XmlNode) requestXml.CreateElement("DATA"))).SetAttribute("LoanIdentifier", loanData.GUID);
          string base64String = Convert.ToBase64String(new FileCompressor().ZipString(loanData.EDSLoanXmlString, Encoding.UTF8));
          XmlElement elementToEncrypt = (XmlElement) requestXml.DocumentElement.AppendChild((XmlNode) requestXml.CreateElement("LOAN_APPLICATION"));
          elementToEncrypt.InnerText = base64String;
          req.EncryptLoanData(elementToEncrypt);
          this.finalizeRequest(req);
          string xml1 = this.docService.MergeNative(req.ToString());
          XmlDocument responseXml = new XmlDocument();
          responseXml.LoadXml(xml1);
          SyncedDataHelper.ReadItemsNeededByEds(responseXml);
          foreach (XmlElement selectNode in responseXml.SelectNodes("DOCSET_RESPONSE/DOCSET/DOCUMENT"))
          {
            string attribute1 = selectNode.GetAttribute("TemplateID");
            string attribute2 = selectNode.GetAttribute("Page");
            if (!string.IsNullOrWhiteSpace(attribute2))
            {
              string str4 = attribute1 + "," + attribute2;
            }
            byte[] numArray = Convert.FromBase64String(selectNode.SelectSingleNode("DATA").InnerText);
            MergeParamValues mergeParamValues = new MergeParamValues((XmlElement) selectNode.SelectSingleNode("MergeParams"));
            int result = -1;
            if (mergeParamValues.ContainsKey("SerialNumber") && int.TryParse(mergeParamValues["SerialNumber"], out result) && result > 0 && result <= edsFormsRequired.Count)
            {
              FormItemInfo key = edsFormsRequired[result - 1];
              dictionary[key] = numArray;
            }
          }
          if (responseXml.SelectSingleNode("DOCSET_RESPONSE/ProcessingErrors") != null)
            processingErrors = responseXml.SelectSingleNode("DOCSET_RESPONSE/ProcessingErrors").OuterXml;
          loanData.IncludeSnapshotInXML = false;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Error executing MergeNative request for DocEngineService: " + (object) ex);
        throw;
      }
      return dictionary;
    }

    public Dictionary<FormItemInfo, byte[]> MergeNativeWithCustomForms(
      LoanData loanData,
      List<FormItemInfo> edsFormsRequired,
      string printOption,
      string correlationID,
      string mergeOnDemandXml,
      out string edsRequestGuid,
      out string processingErrors)
    {
      Dictionary<FormItemInfo, byte[]> dictionary = new Dictionary<FormItemInfo, byte[]>();
      processingErrors = string.Empty;
      try
      {
        DocEngineRequest req = new DocEngineRequest(this.sessionObjects, "MergeNative", correlationID);
        XmlDocument requestXml = req.RequestXml;
        edsRequestGuid = "";
        if (requestXml != null)
        {
          edsRequestGuid = requestXml.DocumentElement.GetAttribute("RequestGuid");
          XmlElement xmlElement = (XmlElement) requestXml.DocumentElement.SelectSingleNode("REQUEST_OPTIONS") ?? (XmlElement) requestXml.DocumentElement.AppendChild((XmlNode) requestXml.CreateElement("REQUEST_OPTIONS"));
          xmlElement.SetAttribute("OrderType", "Preliminary");
          xmlElement.SetAttribute("BorrowerPairIndex", loanData.GetPairIndex(loanData.PairId).ToString());
          xmlElement.SetAttribute("InstanceID", this.sessionObjects.StartupInfo.ServerInstanceName);
          xmlElement.SetAttribute("DocumentOrderMethod", "OnDemand");
          switch (printOption)
          {
            case "BlankForm":
              xmlElement.SetAttribute("MergeBlank", "true");
              xmlElement.SetAttribute("DisplayFieldIds", "false");
              break;
            case "WithFieldIDs":
              xmlElement.SetAttribute("DisplayFieldIds", "true");
              xmlElement.SetAttribute("MergeBlank", "false");
              break;
            case "WithData":
            case "WithSignatures":
            case "Unknown":
              xmlElement.SetAttribute("DisplayFieldIds", "false");
              xmlElement.SetAttribute("MergeBlank", "false");
              break;
          }
          xmlElement.SetAttribute("LoanIsPreCalculated", "true");
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml(mergeOnDemandXml);
          XmlNode node = xmlDocument.SelectSingleNode("MERGE_ON_DEMAND");
          xmlElement.AppendChild(xmlElement.OwnerDocument.ImportNode(node, true));
          loanData.IncludeSnapshotInXML = true;
          loanData.GetCalculationDTSnapshotForLoan();
          ((XmlElement) requestXml.DocumentElement.AppendChild((XmlNode) requestXml.CreateElement("DATA"))).SetAttribute("LoanIdentifier", loanData.GUID);
          string base64String = Convert.ToBase64String(new FileCompressor().ZipString(loanData.EDSLoanXmlString, Encoding.UTF8));
          XmlElement elementToEncrypt = (XmlElement) requestXml.DocumentElement.AppendChild((XmlNode) requestXml.CreateElement("LOAN_APPLICATION"));
          elementToEncrypt.InnerText = base64String;
          req.EncryptLoanData(elementToEncrypt);
          this.finalizeRequest(req);
          string xml = this.docService.MergeNative(req.ToString());
          XmlDocument responseXml = new XmlDocument();
          responseXml.LoadXml(xml);
          SyncedDataHelper.ReadItemsNeededByEds(responseXml);
          foreach (XmlElement selectNode in responseXml.SelectNodes("DOCSET_RESPONSE/DOCSET/DOCUMENT"))
          {
            string attribute1 = selectNode.GetAttribute("TemplateID");
            string attribute2 = selectNode.GetAttribute("Page");
            if (!string.IsNullOrWhiteSpace(attribute2))
            {
              string str = attribute1 + "," + attribute2;
            }
            byte[] numArray = Convert.FromBase64String(selectNode.SelectSingleNode("DATA").InnerText);
            MergeParamValues mergeParamValues = new MergeParamValues((XmlElement) selectNode.SelectSingleNode("MergeParams"));
            int result = -1;
            if (mergeParamValues.ContainsKey("SerialNumber") && int.TryParse(mergeParamValues["SerialNumber"], out result) && result > 0 && result <= edsFormsRequired.Count)
            {
              FormItemInfo key = edsFormsRequired[result - 1];
              dictionary[key] = numArray;
            }
          }
          if (responseXml.SelectSingleNode("DOCSET_RESPONSE/ProcessingErrors") != null)
            processingErrors = responseXml.SelectSingleNode("DOCSET_RESPONSE/ProcessingErrors").OuterXml;
          loanData.IncludeSnapshotInXML = false;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Error executing MergeNative request for DocEngineService: " + (object) ex);
        throw;
      }
      return dictionary;
    }

    public string GenerateEDSStandardFormsRequest(
      LoanData loanData,
      List<FormItemInfo> edsFormsRequired,
      string printOption,
      string printMode,
      string correlationID,
      out string edsRequestGuid,
      out string processingErrors)
    {
      Dictionary<FormItemInfo, byte[]> dictionary = new Dictionary<FormItemInfo, byte[]>();
      XmlElement xmlElement1 = (XmlElement) null;
      processingErrors = string.Empty;
      try
      {
        XmlDocument requestXml = new DocEngineRequest(this.sessionObjects, "MergeNative", correlationID).RequestXml;
        edsRequestGuid = "";
        if (requestXml != null)
        {
          edsRequestGuid = requestXml.DocumentElement.GetAttribute("RequestGuid");
          xmlElement1 = (XmlElement) ((XmlElement) requestXml.DocumentElement.SelectSingleNode("REQUEST_OPTIONS") ?? (XmlElement) requestXml.DocumentElement.AppendChild((XmlNode) requestXml.CreateElement("REQUEST_OPTIONS"))).AppendChild((XmlNode) requestXml.CreateElement("MERGE_ON_DEMAND"));
          string str1 = this.EntityList == null || this.EntityList.Length == 0 ? string.Empty : string.Join(",", this.EntityList);
          for (int index = 0; index < edsFormsRequired.Count; ++index)
          {
            FormItemInfo formItemInfo = edsFormsRequired[index];
            string edsTemplateName = "";
            if (formItemInfo.FormName == "2010 GFE Page 1" && printMode == "RuleSelect")
            {
              string mergeParam = formItemInfo.MergeParams["TemplateName"];
              if (mergeParam.EndsWith(",1"))
                edsTemplateName = mergeParam.Replace(",1", "");
            }
            else
              edsTemplateName = formItemInfo.MergeParams["TemplateName"];
            XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) requestXml.CreateElement("FORM"));
            xmlElement2.SetAttribute("Name", edsTemplateName);
            xmlElement2.SetAttribute("NumCopies", "1");
            xmlElement2.SetAttribute("Comment", "Merge Native Request from Encompass");
            formItemInfo.MergeParams["SourceType"] = "Standard Form";
            formItemInfo.MergeParams["Name"] = formItemInfo.UIName;
            formItemInfo.MergeParams["SourceName"] = formItemInfo.GroupName;
            if (!string.IsNullOrWhiteSpace(formItemInfo.SignatureType))
            {
              xmlElement2.SetAttribute("SigningMode", formItemInfo.SignatureType);
              formItemInfo.MergeParams["SigningMode"] = formItemInfo.SignatureType.ToString();
            }
            else if (formItemInfo.FormName.Contains("4506"))
            {
              xmlElement2.SetAttribute("SigningMode", "TBD");
              formItemInfo.MergeParams["SigningMode"] = "TBD";
            }
            int? templatePageNumber = this.ParseEdsTemplatePageNumber(edsTemplateName);
            int num;
            if (templatePageNumber.HasValue)
            {
              MergeParamValues mergeParams = formItemInfo.MergeParams;
              num = templatePageNumber.Value;
              string str2 = num.ToString();
              mergeParams["PageNumber"] = str2;
            }
            MergeParamValues mergeParams1 = formItemInfo.MergeParams;
            num = index + 1;
            string str3 = num.ToString("000");
            mergeParams1["SerialNumber"] = str3;
            if (str1 != string.Empty)
              formItemInfo.MergeParams["EntityList"] = str1;
            XmlElement xml = formItemInfo.MergeParams.ToXml();
            if (xml != null)
              xmlElement2.AppendChild(xmlElement2.OwnerDocument.ImportNode((XmlNode) xml, true));
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Error executing GenerateEDSStandardFormsRequest/MergeNative request for DocEngineService: " + (object) ex);
        throw;
      }
      return xmlElement1.OuterXml;
    }

    private int? ParseEdsTemplatePageNumber(string edsTemplateName)
    {
      int? templatePageNumber = new int?();
      if (edsTemplateName.Contains(","))
      {
        string[] strArray = edsTemplateName.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        int result;
        if (strArray.Length > 1 && int.TryParse(strArray[1], out result) && result > 0)
          templatePageNumber = new int?(result);
      }
      return templatePageNumber;
    }

    public LateFees GetLateFees(Dictionary<string, string> inputParams)
    {
      DocEngineRequest req = new DocEngineRequest(this.sessionObjects, nameof (GetLateFees));
      foreach (KeyValuePair<string, string> inputParam in inputParams)
        req.SetExtendedParameter(inputParam.Key, inputParam.Value);
      try
      {
        this.finalizeRequest(req);
        return new LateFees(this.parseResponse(this.docService.GetLateFees(req.ToString())));
      }
      catch (Exception ex)
      {
        Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Error executing GetPlancodeDetails request for DocEngineService: " + (object) ex);
        throw;
      }
    }

    public StandardPlan[] GetPlans(string[] planIDs)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 649, nameof (GetPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\DocEngineService.cs");
      StandardPlan[] plans = (StandardPlan[]) null;
      DocEngineRequest req = new DocEngineRequest(this.sessionObjects, "GetPlancodeDetails");
      string key = string.Join(",", planIDs);
      req.SetParameter("PlancodeIdList", key);
      lock (DocEngineService._lockObject)
      {
        if (DocEngineService._edsPlansCache != null)
        {
          if (DocEngineService._edsPlansCache.ContainsKey(key))
          {
            if (DocEngineService._edsPlansCache[key] != null)
            {
              if (DocEngineService._edsPlansCache[key].Length != 0)
              {
                if (DocEngineService._edsPlansLastDownloadedUtc > DateTime.MinValue)
                {
                  string str = DocEngineService._edsPlansLastDownloadedUtc.ToString("yyyy-MM-dd HH:mm:ss");
                  req.SetParameter("PlansLastDownloadedUtc", str);
                  PerformanceMeter.Current.AddCheckpoint(string.Format("Posted to {0}:{1} to EDS for Short Response", (object) "PlansLastDownloadedUtc", (object) str), 665, nameof (GetPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\DocEngineService.cs");
                }
              }
            }
          }
        }
      }
      try
      {
        this.finalizeRequest(req);
        PerformanceMeter.Current.AddCheckpoint("BEFORE GetPlancodeDetails", 672, nameof (GetPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\DocEngineService.cs");
        string plancodeDetails = this.docService.GetPlancodeDetails(req.ToString());
        PerformanceMeter.Current.AddCheckpoint("AFTER GetPlancodeDetails", 674, nameof (GetPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\DocEngineService.cs");
        DocEngineResponse response = this.parseResponse(plancodeDetails);
        lock (DocEngineService._lockObject)
        {
          if (response.ResponseXml.DocumentElement.GetAttribute("Message") == "Plans Unchanged")
          {
            plans = DocEngineService._edsPlansCache[key];
            PerformanceMeter.Current.AddCheckpoint("Short response from EDS, Plans returned from Cache", 683, nameof (GetPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\DocEngineService.cs");
          }
          else
          {
            plans = StandardPlan.ExtractCollection(response);
            DocEngineService._edsPlansLastDownloadedUtc = DateTime.UtcNow;
            DocEngineService._edsPlansCache.Clear();
            DocEngineService._edsPlansCache[key] = plans;
            PerformanceMeter.Current.AddCheckpoint("Long response from EDS, Cache refreshed", 692, nameof (GetPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\DocEngineService.cs");
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Error executing GetPlancodeDetails request for DocEngineService: " + (object) ex);
        throw;
      }
      PerformanceMeter.Current.AddCheckpoint("END", 703, nameof (GetPlans), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\DocEngineService.cs");
      return plans;
    }

    public StandardPlan GetPlan(string planCode)
    {
      StandardPlan[] plans = this.GetPlans(new string[1]
      {
        planCode
      });
      return plans.Length != 0 ? plans[0] : (StandardPlan) null;
    }

    public void Dispose()
    {
      if (this.docService != null)
      {
        this.docService.Dispose();
        this.docService = (DocService) null;
      }
      this.sessionObjects = (SessionObjects) null;
    }

    private void finalizeRequest(DocEngineRequest req, bool forceSendSyncedData = false)
    {
      this.writeToTemp(req.ToString(), "EncompassDocs-LastRequest.xml");
      SyncedDataHelper.SendSyncedItems(req, this.sessionObjects, forceSendSyncedData);
    }

    private DocEngineResponse parseResponse(string responseXml)
    {
      this.writeToTemp(responseXml, "EncompassDocs-LastResponse.xml");
      DocEngineResponse response = new DocEngineResponse(responseXml);
      if (response.ResponseStatus == DocEngineResponseStatus.Exception)
        throw new DocServiceException("The Encompass Doc Service reported an internal exception: " + response.GetErrorMessage(), response);
      if (response.ResponseStatus == DocEngineResponseStatus.Failure && response.GetErrorMessage() == "Invalid credential")
        throw new DocServiceLoginException("Access to the Doc Service has been denied. Verify your Company Password and try again.", response);
      return !response.IsError() ? response : throw new DocServiceException("The Encompass Doc Service reported an error: " + response.GetErrorMessage(), response);
    }

    public void writeToTemp(string text, string fileName)
    {
      lock (typeof (DocEngineService))
      {
        string path = Path.Combine(SystemSettings.TempFolderRoot, fileName);
        try
        {
          File.WriteAllText(path, text, Encoding.Default);
        }
        catch (Exception ex)
        {
          Tracing.Log(DocEngineService.sw, nameof (DocEngineService), TraceLevel.Error, "Failed to write " + fileName + ": " + (object) ex);
        }
      }
    }
  }
}
