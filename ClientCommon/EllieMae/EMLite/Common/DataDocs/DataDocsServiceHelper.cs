// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DataDocs.DataDocsServiceHelper
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.RemotingServices;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common.DataDocs
{
  public class DataDocsServiceHelper
  {
    private object syncLock = new object();
    private readonly string sw = Tracing.SwOutsideLoan;
    private readonly string className = nameof (DataDocsServiceHelper);
    private List<SubmissionStatus> _submissions = new List<SubmissionStatus>();
    private List<SubmissionStatus> _filteredSubmissions = new List<SubmissionStatus>();
    private bool _isFiltered;
    private int _submissionCount;
    private int _filterSubmissionCount;
    private bool _isSuperAdmin;
    private string _lastSortOrder = "";
    private CacheItemRetentionPolicy _cacheItemRetentionPolicy;
    private Sessions.Session session;

    public string oApiBaseUrl { get; private set; }

    public string PartnerAPIPath { get; private set; }

    public string LoanSubmissionsAPIPath { get; private set; }

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public DataDocsServiceHelper()
      : this(Session.DefaultInstance)
    {
    }

    public DataDocsServiceHelper(Sessions.Session session)
    {
      this.session = session;
      this.oApiBaseUrl = string.IsNullOrEmpty(session.StartupInfo.OAPIGatewayBaseUri) ? EnConfigurationSettings.AppSettings["oAuth.Url"] : session.StartupInfo.OAPIGatewayBaseUri;
      this.PartnerAPIPath = "/services/v1/partners";
      this.LoanSubmissionsAPIPath = "/encompass/v1/efolderservices/loanSelector";
      this._isSuperAdmin = session.UserInfo.IsAdministrator() || session.UserInfo.IsSuperAdministrator();
      this._cacheItemRetentionPolicy = CacheItemRetentionPolicy.ExpireIn2Hours;
    }

    private List<PartnerResponseBody> GetProviderList(string accessToken)
    {
      ICache simpleCache = CacheManager.GetSimpleCache("PartnerResponseCache");
      string key = string.Format("{0}_{1}", (object) this.session.StartupInfo.ServerInstanceName, (object) this.session.StartupInfo.SessionID);
      List<PartnerResponseBody> providerList1 = (List<PartnerResponseBody>) simpleCache.Get(key) ?? (List<PartnerResponseBody>) null;
      if (providerList1 != null)
      {
        Tracing.Log(this.sw, TraceLevel.Info, this.className, "Reading partner response from cache. Key :  " + key + "Cached Investors Count : " + (object) providerList1.Count);
        return providerList1;
      }
      HttpWebRequest httpWebRequest = DataDocsServiceHelper.GetHttpWebRequest(this.oApiBaseUrl + "/investor/v1/products");
      httpWebRequest.Method = "GET";
      httpWebRequest.Headers.Add("Authorization", accessToken);
      string end;
      using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
        end = streamReader.ReadToEnd();
      object obj1 = JsonConvert.DeserializeObject(end);
      List<PartnerResponseBody> providerList2 = JsonConvert.DeserializeObject<List<PartnerResponseBody>>(end);
      for (int index = 0; index < providerList2.Count; ++index)
      {
        PartnerResponseBody partnerResponseBody = providerList2[index];
        // ISSUE: reference to a compiler-generated field
        if (DataDocsServiceHelper.\u003C\u003Eo__30.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          DataDocsServiceHelper.\u003C\u003Eo__30.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, JObject>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (JObject), typeof (DataDocsServiceHelper)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, JObject> target = DataDocsServiceHelper.\u003C\u003Eo__30.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, JObject>> p1 = DataDocsServiceHelper.\u003C\u003Eo__30.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (DataDocsServiceHelper.\u003C\u003Eo__30.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          DataDocsServiceHelper.\u003C\u003Eo__30.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (DataDocsServiceHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = DataDocsServiceHelper.\u003C\u003Eo__30.\u003C\u003Ep__0.Target((CallSite) DataDocsServiceHelper.\u003C\u003Eo__30.\u003C\u003Ep__0, obj1, index);
        JObject jobject = target((CallSite) p1, obj2);
        partnerResponseBody.RawResponse = jobject;
      }
      Tracing.Log(this.sw, TraceLevel.Info, this.className, "Fetched partners response from OAPI. Caching the Key :  " + key + "Investors Count: " + (object) providerList2.Count);
      simpleCache.Put(key, new CacheItem((object) providerList2, this._cacheItemRetentionPolicy, (ICacheItemExpirationBehavior) new SimpleCacheItemExpirationBehavior()));
      return providerList2;
    }

    public bool HasServiceAcccess(string investortServiceCategory)
    {
      return this.GetInvestorsList().Exists((Predicate<PartnerResponseBody>) (i => i.Category.Equals(investortServiceCategory, StringComparison.InvariantCultureIgnoreCase)));
    }

    public List<PartnerResponseBody> GetInvestorsList()
    {
      lock (this.syncLock)
      {
        if (!this.session.IsConnected)
          return new List<PartnerResponseBody>();
        try
        {
          ReauthenticateOnUnauthorised reauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(this.session.ServerIdentity.InstanceName, this.session.SessionObjects.SessionID, this.session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(this.session.SessionObjects));
          List<PartnerResponseBody> serviceProviders = new List<PartnerResponseBody>();
          reauthenticateOnUnauthorised.Execute("sc", (Action<AccessToken>) (accessToken => serviceProviders = this.GetProviderList(accessToken.TypeAndToken)));
          return serviceProviders;
        }
        catch (Exception ex)
        {
          Tracing.Log(this.sw, this.className, TraceLevel.Error, "Error while retrieving partners using OAPI: " + (object) ex);
          return new List<PartnerResponseBody>();
        }
      }
    }

    public PartnerPreferencesResponseBody GetPartnerPreferences(
      string partnerId,
      string productname,
      string activeVersion,
      string accessToken)
    {
      try
      {
        HttpWebRequest httpWebRequest = DataDocsServiceHelper.GetHttpWebRequest(this.oApiBaseUrl + this.PartnerAPIPath + "/" + partnerId + "/product/" + productname + "/preferences?key=eic_application&version=" + activeVersion);
        httpWebRequest.Method = "GET";
        httpWebRequest.Headers.Add("Authorization", accessToken);
        string end;
        using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
          end = streamReader.ReadToEnd();
        return JsonConvert.DeserializeObject<PartnerPreferencesResponseBody>(end);
      }
      catch (Exception ex)
      {
      }
      return (PartnerPreferencesResponseBody) null;
    }

    public List<StackingTemplatesResponseBody> GetStackingTemplates(string accessToken)
    {
      List<StackingTemplatesResponseBody> templatesResponseBodyList = new List<StackingTemplatesResponseBody>();
      try
      {
        HttpWebRequest httpWebRequest = DataDocsServiceHelper.GetHttpWebRequest(this.oApiBaseUrl + "/encompass/v1/settings/efolder/documentStackingTemplates");
        httpWebRequest.Method = "GET";
        httpWebRequest.Headers.Add("Authorization", accessToken);
        string end;
        using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
          end = streamReader.ReadToEnd();
        return JsonConvert.DeserializeObject<List<StackingTemplatesResponseBody>>(end);
      }
      catch (Exception ex)
      {
        return (List<StackingTemplatesResponseBody>) null;
      }
    }

    public List<CustomDataField> GetCustomDataFields(string jasonCustomFields)
    {
      if (string.IsNullOrWhiteSpace(jasonCustomFields))
        return (List<CustomDataField>) null;
      try
      {
        return JsonConvert.DeserializeObject<List<CustomDataField>>(jasonCustomFields);
      }
      catch (Exception ex)
      {
        throw new Exception("Error while parsing custom fields on Investors page");
      }
    }

    public WebResponse CreateInvestorPackage(
      string accessToken,
      CreateInvestorPackageRequestBody createInvestorPackageRequestBody)
    {
      HttpWebRequest httpWebRequest = DataDocsServiceHelper.GetHttpWebRequest(this.oApiBaseUrl + "/encompass/v1/efolderservices/packages");
      httpWebRequest.Method = "POST";
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Headers.Add("Authorization", accessToken);
      byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) createInvestorPackageRequestBody, Formatting.Indented));
      httpWebRequest.ContentLength = (long) bytes.Length;
      using (Stream requestStream = httpWebRequest.GetRequestStream())
        requestStream.Write(bytes, 0, bytes.Length);
      return httpWebRequest.GetResponse();
    }

    public string CreateTransaction(
      PartnerResponseBody partner,
      string loanGuid,
      string accessToken)
    {
      HttpWebRequest httpWebRequest = DataDocsServiceHelper.GetHttpWebRequest(this.oApiBaseUrl + this.PartnerAPIPath + "/" + partner.PartnerID + "/transactions");
      httpWebRequest.Method = "POST";
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Headers.Add("Authorization", accessToken);
      byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) new CreateTransactionRequestBody()
      {
        Product = new Product()
        {
          EntityRef = new EntityRef()
          {
            EntityId = loanGuid.Replace("{", "").Replace("}", ""),
            EntityType = "urn:elli:encompass:loan"
          },
          ClientEntity = new ClientEntity()
          {
            ConfigName = partner.ConfigurationName
          },
          Name = partner.ProductName
        }
      }, Formatting.Indented));
      httpWebRequest.ContentLength = (long) bytes.Length;
      using (Stream requestStream = httpWebRequest.GetRequestStream())
        requestStream.Write(bytes, 0, bytes.Length);
      return httpWebRequest.GetResponse().Headers["elli-uilocation"];
    }

    private static HttpWebRequest GetHttpWebRequest(string apiUrl)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(apiUrl);
      httpWebRequest.ServicePoint.Expect100Continue = false;
      return httpWebRequest;
    }

    public bool ValidateUserSettings(string category, string userId)
    {
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(this.session.ServerIdentity.InstanceName, this.session.SessionID, this.session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(this.session.SessionObjects));
      bool result = false;
      this.ReauthenticateOnUnauthorised.Execute("sc", (Action<AccessToken>) (accessToken =>
      {
        HttpWebRequest httpWebRequest = DataDocsServiceHelper.GetHttpWebRequest(this.oApiBaseUrl + "/encompass/v1/users/" + userId + "/systemOptionsValidator");
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Headers.Add("Authorization", accessToken.TypeAndToken);
        byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) new List<SystemOptionsValidator>()
        {
          new SystemOptionsValidator()
          {
            Category = category,
            Attribute = "username"
          },
          new SystemOptionsValidator()
          {
            Category = category,
            Attribute = "password"
          }
        }));
        httpWebRequest.ContentLength = (long) bytes.Length;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
          requestStream.Write(bytes, 0, bytes.Length);
        string end;
        using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
          end = streamReader.ReadToEnd();
        result = JsonConvert.DeserializeObject<List<SystemOptionsValidatorResponse>>(end).Any<SystemOptionsValidatorResponse>((Func<SystemOptionsValidatorResponse, bool>) (x => x.Exists));
      }));
      return result;
    }

    public string ConvertGenericObjectToJson(object postData)
    {
      return JsonConvert.SerializeObject(postData);
    }

    public List<SubmissionStatus> InitializeSubmissions(
      string range = null,
      FieldFilterList filters = null,
      string sortIndicator = "desc")
    {
      List<SubmissionStatus> submissions = (List<SubmissionStatus>) null;
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(this.session.ServerIdentity.InstanceName, this.session.SessionID, this.session.SessionObjects.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(this.session.SessionObjects));
      try
      {
        this.ReauthenticateOnUnauthorised.Execute("sc", (Action<AccessToken>) (accessToken =>
        {
          HttpWebRequest httpWebRequest = DataDocsServiceHelper.GetHttpWebRequest(this.oApiBaseUrl + this.LoanSubmissionsAPIPath + range);
          httpWebRequest.Method = "POST";
          httpWebRequest.ContentType = "application/json";
          httpWebRequest.Headers.Add("Authorization", accessToken.TypeAndToken);
          byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(filters == null ? (object) GetSubmissionsRequestBody.GetRequestBody(this.session.ServerIdentity.InstanceName, this.session.UserID, this._isSuperAdmin, sortIndicator, (FieldFilterList) null) : (object) GetSubmissionsRequestBody.GetRequestBody(this.session.ServerIdentity.InstanceName, this.session.UserID, this._isSuperAdmin, sortIndicator, filters), Formatting.Indented));
          httpWebRequest.ContentLength = (long) bytes.Length;
          using (Stream requestStream = httpWebRequest.GetRequestStream())
            requestStream.Write(bytes, 0, bytes.Length);
          WebResponse response = httpWebRequest.GetResponse();
          if (filters == null)
            this._submissionCount = Convert.ToInt32(response.Headers["X-Total-Count"]);
          else
            this._filterSubmissionCount = Convert.ToInt32(response.Headers["X-Total-Count"]);
          string end;
          using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            end = streamReader.ReadToEnd();
          submissions = JsonConvert.DeserializeObject<List<SubmissionStatus>>(end, new JsonSerializerSettings()
          {
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore
          });
        }));
        return submissions;
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, TraceLevel.Error, this.className, "Error occurred while retrieving submissions. Exception details: " + ex.Message + " " + ex.StackTrace);
        return new List<SubmissionStatus>();
      }
    }

    public int GetSubmissionsCount()
    {
      if (this._isFiltered)
        return this._filterSubmissionCount;
      this._submissions = this.InitializeSubmissions(string.Format("?start={0}&limit={1}", (object) 1, (object) DataDocsConstants.MAX_SUBMISSIONS_PER_PAGE));
      return this._submissionCount;
    }

    public SubmissionStatus[] GetSubmissions(int startIndex, int count, string sortOrder)
    {
      if (!string.IsNullOrEmpty(sortOrder))
        this._lastSortOrder = sortOrder;
      this._submissions = this.InitializeSubmissions(string.Format("?start={0}&limit={1}", (object) startIndex, (object) count), sortIndicator: this._lastSortOrder);
      return this._submissions.ToArray();
    }

    public SubmissionStatus[] GetFilteredSubmissions(
      int startIndex,
      int count,
      FieldFilterList filters,
      string sortOrder)
    {
      this._isFiltered = filters.Count != 0;
      if (!string.IsNullOrEmpty(sortOrder))
        this._lastSortOrder = sortOrder;
      this._filteredSubmissions = this.InitializeSubmissions(string.Format("?start={0}&limit={1}", (object) startIndex, (object) count), filters, this._lastSortOrder);
      return this._filteredSubmissions.ToArray();
    }

    public void ClearFilter()
    {
      this._isFiltered = false;
      this._filteredSubmissions = new List<SubmissionStatus>();
    }

    private IEnumerable<SubmissionStatus> Filter(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      IEnumerable<SubmissionStatus> submissionStatuses = intermediateResults;
      switch (filter.FieldDescription)
      {
        case "Created By":
          submissionStatuses = this.FilterByCreatedBy(intermediateResults, filter);
          break;
        case "Created Date":
          submissionStatuses = this.FilterByCreateDate(intermediateResults, filter);
          break;
        case "Failed":
          submissionStatuses = this.FilterByAuditRed(intermediateResults, filter);
          break;
        case "Inconclusive":
          submissionStatuses = this.FilterByAuditYellow(intermediateResults, filter);
          break;
        case "Loan Number":
          submissionStatuses = this.FilterByLoanNumber(intermediateResults, filter);
          break;
        case "Passed":
          submissionStatuses = this.FilterByAuditGreen(intermediateResults, filter);
          break;
        case "Recipient Transaction ID":
          submissionStatuses = this.FilterByRecipientTransactionID(intermediateResults, filter);
          break;
        case "Reference ID":
          submissionStatuses = this.FilterByReferenceId(intermediateResults, filter);
          break;
        case "Status":
          submissionStatuses = this.FilterByStatus(intermediateResults, filter);
          break;
        case "Status Date":
          submissionStatuses = this.FilterByStatusDate(intermediateResults, filter);
          break;
        case "Submission Date":
          submissionStatuses = this.FilterBySubmissionDate(intermediateResults, filter);
          break;
        case "Submitted To":
          submissionStatuses = this.FilterBySubmittedTo(intermediateResults, filter);
          break;
      }
      return submissionStatuses;
    }

    private IEnumerable<SubmissionStatus> FilterByStatusDate(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      IEnumerable<SubmissionStatus> submissionStatuses = intermediateResults;
      DateTime dateToCompare = Convert.ToDateTime(filter.ValueDescription);
      switch (filter.OperatorType)
      {
        case OperatorTypes.Equals:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.StatusDate.HasValue && sub.StatusDate.Value.Date == dateToCompare));
          break;
        case OperatorTypes.NotEqual:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.StatusDate.HasValue && sub.StatusDate.Value.Date != dateToCompare));
          break;
        case OperatorTypes.DateOnOrAfter:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.StatusDate.HasValue && sub.StatusDate.Value.Date >= dateToCompare));
          break;
        case OperatorTypes.DateOnOrBefore:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.StatusDate.HasValue && sub.StatusDate.Value.Date <= dateToCompare));
          break;
        case OperatorTypes.DateAfter:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.StatusDate.HasValue && sub.StatusDate.Value.Date > dateToCompare));
          break;
        case OperatorTypes.DateBefore:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.StatusDate.HasValue && sub.StatusDate.Value.Date < dateToCompare));
          break;
      }
      return submissionStatuses;
    }

    private IEnumerable<SubmissionStatus> FilterByAuditGreen(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      IEnumerable<SubmissionStatus> submissionStatuses = intermediateResults;
      int valueToCompare = Convert.ToInt32(filter.ValueDescription);
      switch (filter.OperatorType)
      {
        case OperatorTypes.Equals:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountGreen == valueToCompare));
          break;
        case OperatorTypes.NotEqual:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountGreen != valueToCompare));
          break;
        case OperatorTypes.GreaterThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountGreen > valueToCompare));
          break;
        case OperatorTypes.NotGreaterThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountGreen <= valueToCompare));
          break;
        case OperatorTypes.LessThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountGreen < valueToCompare));
          break;
        case OperatorTypes.NotLessThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountGreen >= valueToCompare));
          break;
      }
      return submissionStatuses;
    }

    private IEnumerable<SubmissionStatus> FilterByAuditYellow(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      IEnumerable<SubmissionStatus> submissionStatuses = intermediateResults;
      int valueToCompare = Convert.ToInt32(filter.ValueDescription);
      switch (filter.OperatorType)
      {
        case OperatorTypes.Equals:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountYellow == valueToCompare));
          break;
        case OperatorTypes.NotEqual:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountYellow != valueToCompare));
          break;
        case OperatorTypes.GreaterThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountYellow > valueToCompare));
          break;
        case OperatorTypes.NotGreaterThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountYellow <= valueToCompare));
          break;
        case OperatorTypes.LessThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountYellow < valueToCompare));
          break;
        case OperatorTypes.NotLessThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountYellow >= valueToCompare));
          break;
      }
      return submissionStatuses;
    }

    private IEnumerable<SubmissionStatus> FilterByAuditRed(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      IEnumerable<SubmissionStatus> submissionStatuses = intermediateResults;
      int valueToCompare = Convert.ToInt32(filter.ValueDescription);
      switch (filter.OperatorType)
      {
        case OperatorTypes.Equals:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountRed == valueToCompare));
          break;
        case OperatorTypes.NotEqual:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountRed != valueToCompare));
          break;
        case OperatorTypes.GreaterThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountRed > valueToCompare));
          break;
        case OperatorTypes.NotGreaterThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountRed <= valueToCompare));
          break;
        case OperatorTypes.LessThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountRed < valueToCompare));
          break;
        case OperatorTypes.NotLessThan:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.AuditCountRed >= valueToCompare));
          break;
      }
      return submissionStatuses;
    }

    private IEnumerable<SubmissionStatus> FilterByStatus(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      IEnumerable<SubmissionStatus> submissionStatuses = intermediateResults;
      switch (DataDocsConstants.StringToDeliveryStatusEnum(filter.ValueDescription))
      {
        case DeliveryStatus.InProgress:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.Status == DeliveryStatus.InProgress));
          break;
        case DeliveryStatus.Completed:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.Status == DeliveryStatus.Completed));
          break;
        case DeliveryStatus.Error:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.Status == DeliveryStatus.Error));
          break;
        case DeliveryStatus.Cancelled:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.Status == DeliveryStatus.Cancelled));
          break;
        case DeliveryStatus.Submitted:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.Status == DeliveryStatus.Submitted));
          break;
        case DeliveryStatus.Accepted:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.Status == DeliveryStatus.Accepted));
          break;
        case DeliveryStatus.Rejected:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.Status == DeliveryStatus.Rejected));
          break;
        case DeliveryStatus.Delivered:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.Status == DeliveryStatus.Delivered));
          break;
      }
      return submissionStatuses;
    }

    private IEnumerable<SubmissionStatus> FilterByLoanNumber(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      return intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.LoanNumber.ToLower().Contains(filter.ValueDescription.ToLower())));
    }

    private IEnumerable<SubmissionStatus> FilterByCreatedUserName(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      return intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => !string.IsNullOrWhiteSpace(sub.CreatedUserName) && sub.CreatedUserName.ToLower().Contains(filter.ValueDescription.ToLower())));
    }

    private IEnumerable<SubmissionStatus> FilterByCreatedBy(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      return intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => !string.IsNullOrWhiteSpace(sub.CreatedBy) && sub.CreatedBy.ToLower().Contains(filter.ValueDescription.ToLower())));
    }

    private IEnumerable<SubmissionStatus> FilterByCreateDate(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      IEnumerable<SubmissionStatus> date = intermediateResults;
      DateTime dateToCompare = Convert.ToDateTime(filter.ValueDescription);
      switch (filter.OperatorType)
      {
        case OperatorTypes.Equals:
          date = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.CreateDate.HasValue && sub.CreateDate.Value.Date == dateToCompare));
          break;
        case OperatorTypes.NotEqual:
          date = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.CreateDate.HasValue && sub.CreateDate.Value.Date != dateToCompare));
          break;
        case OperatorTypes.DateOnOrAfter:
          date = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.CreateDate.HasValue && sub.CreateDate.Value.Date >= dateToCompare));
          break;
        case OperatorTypes.DateOnOrBefore:
          date = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.CreateDate.HasValue && sub.CreateDate.Value.Date <= dateToCompare));
          break;
        case OperatorTypes.DateAfter:
          date = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.CreateDate.HasValue && sub.CreateDate.Value.Date > dateToCompare));
          break;
        case OperatorTypes.DateBefore:
          date = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.CreateDate.HasValue && sub.CreateDate.Value.Date < dateToCompare));
          break;
      }
      return date;
    }

    private IEnumerable<SubmissionStatus> FilterBySubmissionDate(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      IEnumerable<SubmissionStatus> submissionStatuses = intermediateResults;
      DateTime dateToCompare = Convert.ToDateTime(filter.ValueDescription);
      switch (filter.OperatorType)
      {
        case OperatorTypes.Equals:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.SubmissionDate.HasValue && sub.SubmissionDate.Value.Date == dateToCompare));
          break;
        case OperatorTypes.NotEqual:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.SubmissionDate.HasValue && sub.SubmissionDate.Value.Date != dateToCompare));
          break;
        case OperatorTypes.DateOnOrAfter:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.SubmissionDate.HasValue && sub.SubmissionDate.Value.Date >= dateToCompare));
          break;
        case OperatorTypes.DateOnOrBefore:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.SubmissionDate.HasValue && sub.SubmissionDate.Value.Date <= dateToCompare));
          break;
        case OperatorTypes.DateAfter:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.SubmissionDate.HasValue && sub.SubmissionDate.Value.Date > dateToCompare));
          break;
        case OperatorTypes.DateBefore:
          submissionStatuses = intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.SubmissionDate.HasValue && sub.SubmissionDate.Value.Date < dateToCompare));
          break;
      }
      return submissionStatuses;
    }

    private IEnumerable<SubmissionStatus> FilterBySubmittedTo(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      return intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.SubmittedTo.ToLower().Contains(filter.ValueDescription.ToLower())));
    }

    private IEnumerable<SubmissionStatus> FilterByRecipientTransactionID(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      return intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.RecipientTransactionID.ToLower().Contains(filter.ValueDescription.ToLower())));
    }

    private IEnumerable<SubmissionStatus> FilterByReferenceId(
      IEnumerable<SubmissionStatus> intermediateResults,
      FieldFilter filter)
    {
      return intermediateResults.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.ReferenceID.ToLower().Contains(filter.ValueDescription.ToLower())));
    }

    public void SubmitForDelivery(List<SubmissionStatus> submissions)
    {
      this._submissions = this._submissions.Except<SubmissionStatus>((IEnumerable<SubmissionStatus>) submissions).ToList<SubmissionStatus>();
    }

    public void RemoveFromDelivery(List<SubmissionStatus> submissions)
    {
      this._submissions = this._submissions.Except<SubmissionStatus>((IEnumerable<SubmissionStatus>) submissions).ToList<SubmissionStatus>();
    }

    public string GetAuditReport(SubmissionStatus submission) => (string) null;

    public void GetDimension(
      string accessToken,
      string key,
      ref double defaultWidth,
      ref double defaultHeight)
    {
      HttpWebRequest httpWebRequest = DataDocsServiceHelper.GetHttpWebRequest(this.oApiBaseUrl + "/investor/v1/settings?keys=" + key);
      httpWebRequest.Method = "GET";
      httpWebRequest.Headers.Add("Authorization", accessToken);
      httpWebRequest.ContentType = "application/json";
      double num1 = defaultWidth;
      double num2 = defaultHeight;
      try
      {
        string end;
        using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
          end = streamReader.ReadToEnd();
        List<DataDocsServiceHelper.Settings> settingsList = JsonConvert.DeserializeObject<List<DataDocsServiceHelper.Settings>>(end);
        if (settingsList != null && settingsList.Count > 0)
        {
          string str = settingsList[0].Value;
          if (!string.IsNullOrEmpty(str) && str.ToLower().Contains<char>('x'))
          {
            string[] strArray = str.ToLower().Split('x');
            if (strArray.Length != 0)
            {
              num1 = Convert.ToDouble(strArray[0]) / 100.0;
              num2 = Convert.ToDouble(strArray[1]) / 100.0;
              if (num1 == 0.0 || num2 == 0.0)
              {
                num1 = defaultWidth;
                num2 = defaultHeight;
              }
            }
          }
        }
        defaultHeight = num2;
        defaultWidth = num1;
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, TraceLevel.Error, this.className, string.Format("Error in GetDimension. Exception: {0}", (object) ex.Message));
      }
    }

    private class Settings
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
