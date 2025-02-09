// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.EMSiteWebService
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "4.0.30319.17929")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "EMSiteWebServiceSoap", Namespace = "http://www.elliemae.com/")]
  public class EMSiteWebService : SoapHttpClientProtocol
  {
    private SendOrPostCallback GetFeesForCDWLoanOperationCompleted;
    private SendOrPostCallback GetFeesForCDWMortgageRatesOperationCompleted;
    private SendOrPostCallback GetEMSiteIDByClientIDUserIDOperationCompleted;
    private SendOrPostCallback ValidateEMSiteIDOperationCompleted;
    private SendOrPostCallback GetDomainStringByEMSiteIDOperationCompleted;
    private SendOrPostCallback GetSiteTypeByEMSiteIDOperationCompleted;
    private SendOrPostCallback GetSSLDomainOperationCompleted;
    private SendOrPostCallback GetCompanyNameByEMSiteIDOperationCompleted;
    private SendOrPostCallback GetTrialStatusOperationCompleted;
    private SendOrPostCallback CreateCDWSiteOperationCompleted;
    private SendOrPostCallback CreateEMSiteOperationCompleted;
    private SendOrPostCallback CreateMirrorTPOSiteFromSiteIDOperationCompleted;
    private SendOrPostCallback CreateEMSiteReturnSiteIDsOperationCompleted;
    private SendOrPostCallback UploadFileOperationCompleted;
    private SendOrPostCallback AddShowDocumentOperationCompleted;
    private SendOrPostCallback DeleteShowDocumentOperationCompleted;
    private SendOrPostCallback DeleteFilesOperationCompleted;
    private SendOrPostCallback GetWatchCodeOperationCompleted;
    private SendOrPostCallback CheckClientUserIDOperationCompleted;
    private SendOrPostCallback HasAccoutingIDOperationCompleted;
    private SendOrPostCallback AddBundlePurchaseOperationCompleted;
    private SendOrPostCallback CenterWiseNotConfiguredOperationCompleted;
    private SendOrPostCallback HasUnconfiguredCenterWiseSiteOperationCompleted;
    private SendOrPostCallback CreateCenterWiseLOSiteOperationCompleted;
    private SendOrPostCallback CreateCenterWiseCorporateSiteOperationCompleted;
    private SendOrPostCallback GetPortablePageCustomUrlOperationCompleted;
    private SendOrPostCallback CenterWiseNotConfigured1OperationCompleted;
    private SendOrPostCallback CreateCenterWiseCorporateSite1OperationCompleted;
    private SendOrPostCallback CreateCenterWiseLOSite1OperationCompleted;
    private SendOrPostCallback GetEMSiteIDByClientIDUserID1OperationCompleted;
    private SendOrPostCallback GetPortablePageCustomUrl1OperationCompleted;
    private SendOrPostCallback GetTrialStatus1OperationCompleted;
    private SendOrPostCallback HasUnconfiguredCenterWiseSite1OperationCompleted;
    private SendOrPostCallback HasActiveWebCenterOperationCompleted;
    private SendOrPostCallback IsWebCenterAdminOperationCompleted;
    private SendOrPostCallback UnlockTPOLoansOperationCompleted;
    private SendOrPostCallback GetTPODocumentListOperationCompleted;
    private SendOrPostCallback GetTPOLoanNumberOperationCompleted;
    private SendOrPostCallback GetTPOLenderLoanGuidOperationCompleted;
    private SendOrPostCallback ValidateTPOLoginAndLoanNumberOperationCompleted;
    private SendOrPostCallback UploadTPODocumentsOperationCompleted;
    private SendOrPostCallback GetTPOLocationOperationCompleted;
    private SendOrPostCallback GetTPOInformationWithClientIDOperationCompleted;
    private SendOrPostCallback GetTPOInformationOperationCompleted;
    private SendOrPostCallback GetTPOInformationMenusOperationCompleted;
    private SendOrPostCallback SendLoanLockedEmailToLOLPOperationCompleted;
    private SendOrPostCallback GetOriginatorRegionsOperationCompleted;
    private SendOrPostCallback UpdateMirrorTPOSiteStatusOperationCompleted;
    private SendOrPostCallback ClearTPONotificationsOperationCompleted;

    public EMSiteWebService()
    {
      this.Url = "https://www.encompasswebcenter.com/EMSiteWebService.asmx";
    }

    public EMSiteWebService(string eMSiteWebServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(eMSiteWebServiceUrl) || !Uri.IsWellFormedUriString(eMSiteWebServiceUrl, UriKind.Absolute))
        this.Url = "https://www.encompasswebcenter.com/EMSiteWebService.asmx";
      else
        this.Url = eMSiteWebServiceUrl;
    }

    public event GetFeesForCDWLoanCompletedEventHandler GetFeesForCDWLoanCompleted;

    public event GetFeesForCDWMortgageRatesCompletedEventHandler GetFeesForCDWMortgageRatesCompleted;

    public event GetEMSiteIDByClientIDUserIDCompletedEventHandler GetEMSiteIDByClientIDUserIDCompleted;

    public event ValidateEMSiteIDCompletedEventHandler ValidateEMSiteIDCompleted;

    public event GetDomainStringByEMSiteIDCompletedEventHandler GetDomainStringByEMSiteIDCompleted;

    public event GetSiteTypeByEMSiteIDCompletedEventHandler GetSiteTypeByEMSiteIDCompleted;

    public event GetSSLDomainCompletedEventHandler GetSSLDomainCompleted;

    public event GetCompanyNameByEMSiteIDCompletedEventHandler GetCompanyNameByEMSiteIDCompleted;

    public event GetTrialStatusCompletedEventHandler GetTrialStatusCompleted;

    public event CreateCDWSiteCompletedEventHandler CreateCDWSiteCompleted;

    public event CreateEMSiteCompletedEventHandler CreateEMSiteCompleted;

    public event CreateMirrorTPOSiteFromSiteIDCompletedEventHandler CreateMirrorTPOSiteFromSiteIDCompleted;

    public event CreateEMSiteReturnSiteIDsCompletedEventHandler CreateEMSiteReturnSiteIDsCompleted;

    public event UploadFileCompletedEventHandler UploadFileCompleted;

    public event AddShowDocumentCompletedEventHandler AddShowDocumentCompleted;

    public event DeleteShowDocumentCompletedEventHandler DeleteShowDocumentCompleted;

    public event DeleteFilesCompletedEventHandler DeleteFilesCompleted;

    public event GetWatchCodeCompletedEventHandler GetWatchCodeCompleted;

    public event CheckClientUserIDCompletedEventHandler CheckClientUserIDCompleted;

    public event HasAccoutingIDCompletedEventHandler HasAccoutingIDCompleted;

    public event AddBundlePurchaseCompletedEventHandler AddBundlePurchaseCompleted;

    public event CenterWiseNotConfiguredCompletedEventHandler CenterWiseNotConfiguredCompleted;

    public event HasUnconfiguredCenterWiseSiteCompletedEventHandler HasUnconfiguredCenterWiseSiteCompleted;

    public event CreateCenterWiseLOSiteCompletedEventHandler CreateCenterWiseLOSiteCompleted;

    public event CreateCenterWiseCorporateSiteCompletedEventHandler CreateCenterWiseCorporateSiteCompleted;

    public event GetPortablePageCustomUrlCompletedEventHandler GetPortablePageCustomUrlCompleted;

    public event CenterWiseNotConfigured1CompletedEventHandler CenterWiseNotConfigured1Completed;

    public event CreateCenterWiseCorporateSite1CompletedEventHandler CreateCenterWiseCorporateSite1Completed;

    public event CreateCenterWiseLOSite1CompletedEventHandler CreateCenterWiseLOSite1Completed;

    public event GetEMSiteIDByClientIDUserID1CompletedEventHandler GetEMSiteIDByClientIDUserID1Completed;

    public event GetPortablePageCustomUrl1CompletedEventHandler GetPortablePageCustomUrl1Completed;

    public event GetTrialStatus1CompletedEventHandler GetTrialStatus1Completed;

    public event HasUnconfiguredCenterWiseSite1CompletedEventHandler HasUnconfiguredCenterWiseSite1Completed;

    public event HasActiveWebCenterCompletedEventHandler HasActiveWebCenterCompleted;

    public event IsWebCenterAdminCompletedEventHandler IsWebCenterAdminCompleted;

    public event UnlockTPOLoansCompletedEventHandler UnlockTPOLoansCompleted;

    public event GetTPODocumentListCompletedEventHandler GetTPODocumentListCompleted;

    public event GetTPOLoanNumberCompletedEventHandler GetTPOLoanNumberCompleted;

    public event GetTPOLenderLoanGuidCompletedEventHandler GetTPOLenderLoanGuidCompleted;

    public event ValidateTPOLoginAndLoanNumberCompletedEventHandler ValidateTPOLoginAndLoanNumberCompleted;

    public event UploadTPODocumentsCompletedEventHandler UploadTPODocumentsCompleted;

    public event GetTPOLocationCompletedEventHandler GetTPOLocationCompleted;

    public event GetTPOInformationWithClientIDCompletedEventHandler GetTPOInformationWithClientIDCompleted;

    public event GetTPOInformationCompletedEventHandler GetTPOInformationCompleted;

    public event GetTPOInformationMenusCompletedEventHandler GetTPOInformationMenusCompleted;

    public event SendLoanLockedEmailToLOLPCompletedEventHandler SendLoanLockedEmailToLOLPCompleted;

    public event GetOriginatorRegionsCompletedEventHandler GetOriginatorRegionsCompleted;

    public event UpdateMirrorTPOSiteStatusCompletedEventHandler UpdateMirrorTPOSiteStatusCompleted;

    public event ClearTPONotificationsCompletedEventHandler ClearTPONotificationsCompleted;

    [SoapDocumentMethod("http://www.elliemae.com/GetFeesForCDWLoan", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetFeesForCDWLoan(
      string passCode,
      string emsiteID,
      int loanID,
      string rate,
      string points)
    {
      return (string) this.Invoke(nameof (GetFeesForCDWLoan), new object[5]
      {
        (object) passCode,
        (object) emsiteID,
        (object) loanID,
        (object) rate,
        (object) points
      })[0];
    }

    public IAsyncResult BeginGetFeesForCDWLoan(
      string passCode,
      string emsiteID,
      int loanID,
      string rate,
      string points,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetFeesForCDWLoan", new object[5]
      {
        (object) passCode,
        (object) emsiteID,
        (object) loanID,
        (object) rate,
        (object) points
      }, callback, asyncState);
    }

    public string EndGetFeesForCDWLoan(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetFeesForCDWLoanAsync(
      string passCode,
      string emsiteID,
      int loanID,
      string rate,
      string points)
    {
      this.GetFeesForCDWLoanAsync(passCode, emsiteID, loanID, rate, points, (object) null);
    }

    public void GetFeesForCDWLoanAsync(
      string passCode,
      string emsiteID,
      int loanID,
      string rate,
      string points,
      object userState)
    {
      if (this.GetFeesForCDWLoanOperationCompleted == null)
        this.GetFeesForCDWLoanOperationCompleted = new SendOrPostCallback(this.OnGetFeesForCDWLoanOperationCompleted);
      this.InvokeAsync("GetFeesForCDWLoan", new object[5]
      {
        (object) passCode,
        (object) emsiteID,
        (object) loanID,
        (object) rate,
        (object) points
      }, this.GetFeesForCDWLoanOperationCompleted, userState);
    }

    private void OnGetFeesForCDWLoanOperationCompleted(object arg)
    {
      if (this.GetFeesForCDWLoanCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetFeesForCDWLoanCompleted((object) this, new GetFeesForCDWLoanCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/GetFeesForCDWMortgageRates", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetFeesForCDWMortgageRates(
      string passCode,
      string emsiteID,
      string rate,
      string points,
      string loanAmount,
      string loanType,
      string loanPurpose,
      string propertyUsage,
      string lienPosition,
      string state)
    {
      return (string) this.Invoke(nameof (GetFeesForCDWMortgageRates), new object[10]
      {
        (object) passCode,
        (object) emsiteID,
        (object) rate,
        (object) points,
        (object) loanAmount,
        (object) loanType,
        (object) loanPurpose,
        (object) propertyUsage,
        (object) lienPosition,
        (object) state
      })[0];
    }

    public IAsyncResult BeginGetFeesForCDWMortgageRates(
      string passCode,
      string emsiteID,
      string rate,
      string points,
      string loanAmount,
      string loanType,
      string loanPurpose,
      string propertyUsage,
      string lienPosition,
      string state,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetFeesForCDWMortgageRates", new object[10]
      {
        (object) passCode,
        (object) emsiteID,
        (object) rate,
        (object) points,
        (object) loanAmount,
        (object) loanType,
        (object) loanPurpose,
        (object) propertyUsage,
        (object) lienPosition,
        (object) state
      }, callback, asyncState);
    }

    public string EndGetFeesForCDWMortgageRates(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetFeesForCDWMortgageRatesAsync(
      string passCode,
      string emsiteID,
      string rate,
      string points,
      string loanAmount,
      string loanType,
      string loanPurpose,
      string propertyUsage,
      string lienPosition,
      string state)
    {
      this.GetFeesForCDWMortgageRatesAsync(passCode, emsiteID, rate, points, loanAmount, loanType, loanPurpose, propertyUsage, lienPosition, state, (object) null);
    }

    public void GetFeesForCDWMortgageRatesAsync(
      string passCode,
      string emsiteID,
      string rate,
      string points,
      string loanAmount,
      string loanType,
      string loanPurpose,
      string propertyUsage,
      string lienPosition,
      string state,
      object userState)
    {
      if (this.GetFeesForCDWMortgageRatesOperationCompleted == null)
        this.GetFeesForCDWMortgageRatesOperationCompleted = new SendOrPostCallback(this.OnGetFeesForCDWMortgageRatesOperationCompleted);
      this.InvokeAsync("GetFeesForCDWMortgageRates", new object[10]
      {
        (object) passCode,
        (object) emsiteID,
        (object) rate,
        (object) points,
        (object) loanAmount,
        (object) loanType,
        (object) loanPurpose,
        (object) propertyUsage,
        (object) lienPosition,
        (object) state
      }, this.GetFeesForCDWMortgageRatesOperationCompleted, userState);
    }

    private void OnGetFeesForCDWMortgageRatesOperationCompleted(object arg)
    {
      if (this.GetFeesForCDWMortgageRatesCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetFeesForCDWMortgageRatesCompleted((object) this, new GetFeesForCDWMortgageRatesCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/GetEMSiteIDByClientIDUserID", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetEMSiteIDByClientIDUserID(string clientID, string userID)
    {
      return (string) this.Invoke(nameof (GetEMSiteIDByClientIDUserID), new object[2]
      {
        (object) clientID,
        (object) userID
      })[0];
    }

    public IAsyncResult BeginGetEMSiteIDByClientIDUserID(
      string clientID,
      string userID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetEMSiteIDByClientIDUserID", new object[2]
      {
        (object) clientID,
        (object) userID
      }, callback, asyncState);
    }

    public string EndGetEMSiteIDByClientIDUserID(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetEMSiteIDByClientIDUserIDAsync(string clientID, string userID)
    {
      this.GetEMSiteIDByClientIDUserIDAsync(clientID, userID, (object) null);
    }

    public void GetEMSiteIDByClientIDUserIDAsync(string clientID, string userID, object userState)
    {
      if (this.GetEMSiteIDByClientIDUserIDOperationCompleted == null)
        this.GetEMSiteIDByClientIDUserIDOperationCompleted = new SendOrPostCallback(this.OnGetEMSiteIDByClientIDUserIDOperationCompleted);
      this.InvokeAsync("GetEMSiteIDByClientIDUserID", new object[2]
      {
        (object) clientID,
        (object) userID
      }, this.GetEMSiteIDByClientIDUserIDOperationCompleted, userState);
    }

    private void OnGetEMSiteIDByClientIDUserIDOperationCompleted(object arg)
    {
      if (this.GetEMSiteIDByClientIDUserIDCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetEMSiteIDByClientIDUserIDCompleted((object) this, new GetEMSiteIDByClientIDUserIDCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/ValidateEMSiteID", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool ValidateEMSiteID(string emsiteid)
    {
      return (bool) this.Invoke(nameof (ValidateEMSiteID), new object[1]
      {
        (object) emsiteid
      })[0];
    }

    public IAsyncResult BeginValidateEMSiteID(
      string emsiteid,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("ValidateEMSiteID", new object[1]
      {
        (object) emsiteid
      }, callback, asyncState);
    }

    public bool EndValidateEMSiteID(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void ValidateEMSiteIDAsync(string emsiteid)
    {
      this.ValidateEMSiteIDAsync(emsiteid, (object) null);
    }

    public void ValidateEMSiteIDAsync(string emsiteid, object userState)
    {
      if (this.ValidateEMSiteIDOperationCompleted == null)
        this.ValidateEMSiteIDOperationCompleted = new SendOrPostCallback(this.OnValidateEMSiteIDOperationCompleted);
      this.InvokeAsync("ValidateEMSiteID", new object[1]
      {
        (object) emsiteid
      }, this.ValidateEMSiteIDOperationCompleted, userState);
    }

    private void OnValidateEMSiteIDOperationCompleted(object arg)
    {
      if (this.ValidateEMSiteIDCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.ValidateEMSiteIDCompleted((object) this, new ValidateEMSiteIDCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/GetDomainStringByEMSiteID", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetDomainStringByEMSiteID(string emsiteid)
    {
      return (string) this.Invoke(nameof (GetDomainStringByEMSiteID), new object[1]
      {
        (object) emsiteid
      })[0];
    }

    public IAsyncResult BeginGetDomainStringByEMSiteID(
      string emsiteid,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetDomainStringByEMSiteID", new object[1]
      {
        (object) emsiteid
      }, callback, asyncState);
    }

    public string EndGetDomainStringByEMSiteID(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetDomainStringByEMSiteIDAsync(string emsiteid)
    {
      this.GetDomainStringByEMSiteIDAsync(emsiteid, (object) null);
    }

    public void GetDomainStringByEMSiteIDAsync(string emsiteid, object userState)
    {
      if (this.GetDomainStringByEMSiteIDOperationCompleted == null)
        this.GetDomainStringByEMSiteIDOperationCompleted = new SendOrPostCallback(this.OnGetDomainStringByEMSiteIDOperationCompleted);
      this.InvokeAsync("GetDomainStringByEMSiteID", new object[1]
      {
        (object) emsiteid
      }, this.GetDomainStringByEMSiteIDOperationCompleted, userState);
    }

    private void OnGetDomainStringByEMSiteIDOperationCompleted(object arg)
    {
      if (this.GetDomainStringByEMSiteIDCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetDomainStringByEMSiteIDCompleted((object) this, new GetDomainStringByEMSiteIDCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/GetSiteTypeByEMSiteID", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetSiteTypeByEMSiteID(string emsiteid)
    {
      return (string) this.Invoke(nameof (GetSiteTypeByEMSiteID), new object[1]
      {
        (object) emsiteid
      })[0];
    }

    public IAsyncResult BeginGetSiteTypeByEMSiteID(
      string emsiteid,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetSiteTypeByEMSiteID", new object[1]
      {
        (object) emsiteid
      }, callback, asyncState);
    }

    public string EndGetSiteTypeByEMSiteID(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetSiteTypeByEMSiteIDAsync(string emsiteid)
    {
      this.GetSiteTypeByEMSiteIDAsync(emsiteid, (object) null);
    }

    public void GetSiteTypeByEMSiteIDAsync(string emsiteid, object userState)
    {
      if (this.GetSiteTypeByEMSiteIDOperationCompleted == null)
        this.GetSiteTypeByEMSiteIDOperationCompleted = new SendOrPostCallback(this.OnGetSiteTypeByEMSiteIDOperationCompleted);
      this.InvokeAsync("GetSiteTypeByEMSiteID", new object[1]
      {
        (object) emsiteid
      }, this.GetSiteTypeByEMSiteIDOperationCompleted, userState);
    }

    private void OnGetSiteTypeByEMSiteIDOperationCompleted(object arg)
    {
      if (this.GetSiteTypeByEMSiteIDCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetSiteTypeByEMSiteIDCompleted((object) this, new GetSiteTypeByEMSiteIDCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/GetSSLDomain", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetSSLDomain(string emsiteid)
    {
      return (string) this.Invoke(nameof (GetSSLDomain), new object[1]
      {
        (object) emsiteid
      })[0];
    }

    public IAsyncResult BeginGetSSLDomain(
      string emsiteid,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetSSLDomain", new object[1]
      {
        (object) emsiteid
      }, callback, asyncState);
    }

    public string EndGetSSLDomain(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetSSLDomainAsync(string emsiteid)
    {
      this.GetSSLDomainAsync(emsiteid, (object) null);
    }

    public void GetSSLDomainAsync(string emsiteid, object userState)
    {
      if (this.GetSSLDomainOperationCompleted == null)
        this.GetSSLDomainOperationCompleted = new SendOrPostCallback(this.OnGetSSLDomainOperationCompleted);
      this.InvokeAsync("GetSSLDomain", new object[1]
      {
        (object) emsiteid
      }, this.GetSSLDomainOperationCompleted, userState);
    }

    private void OnGetSSLDomainOperationCompleted(object arg)
    {
      if (this.GetSSLDomainCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetSSLDomainCompleted((object) this, new GetSSLDomainCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/GetCompanyNameByEMSiteID", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetCompanyNameByEMSiteID(string emsiteid)
    {
      return (string) this.Invoke(nameof (GetCompanyNameByEMSiteID), new object[1]
      {
        (object) emsiteid
      })[0];
    }

    public IAsyncResult BeginGetCompanyNameByEMSiteID(
      string emsiteid,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetCompanyNameByEMSiteID", new object[1]
      {
        (object) emsiteid
      }, callback, asyncState);
    }

    public string EndGetCompanyNameByEMSiteID(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetCompanyNameByEMSiteIDAsync(string emsiteid)
    {
      this.GetCompanyNameByEMSiteIDAsync(emsiteid, (object) null);
    }

    public void GetCompanyNameByEMSiteIDAsync(string emsiteid, object userState)
    {
      if (this.GetCompanyNameByEMSiteIDOperationCompleted == null)
        this.GetCompanyNameByEMSiteIDOperationCompleted = new SendOrPostCallback(this.OnGetCompanyNameByEMSiteIDOperationCompleted);
      this.InvokeAsync("GetCompanyNameByEMSiteID", new object[1]
      {
        (object) emsiteid
      }, this.GetCompanyNameByEMSiteIDOperationCompleted, userState);
    }

    private void OnGetCompanyNameByEMSiteIDOperationCompleted(object arg)
    {
      if (this.GetCompanyNameByEMSiteIDCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetCompanyNameByEMSiteIDCompleted((object) this, new GetCompanyNameByEMSiteIDCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/GetTrialStatus", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public int GetTrialStatus(string clientID, string userID)
    {
      return (int) this.Invoke(nameof (GetTrialStatus), new object[2]
      {
        (object) clientID,
        (object) userID
      })[0];
    }

    public IAsyncResult BeginGetTrialStatus(
      string clientID,
      string userID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetTrialStatus", new object[2]
      {
        (object) clientID,
        (object) userID
      }, callback, asyncState);
    }

    public int EndGetTrialStatus(IAsyncResult asyncResult) => (int) this.EndInvoke(asyncResult)[0];

    public void GetTrialStatusAsync(string clientID, string userID)
    {
      this.GetTrialStatusAsync(clientID, userID, (object) null);
    }

    public void GetTrialStatusAsync(string clientID, string userID, object userState)
    {
      if (this.GetTrialStatusOperationCompleted == null)
        this.GetTrialStatusOperationCompleted = new SendOrPostCallback(this.OnGetTrialStatusOperationCompleted);
      this.InvokeAsync("GetTrialStatus", new object[2]
      {
        (object) clientID,
        (object) userID
      }, this.GetTrialStatusOperationCompleted, userState);
    }

    private void OnGetTrialStatusOperationCompleted(object arg)
    {
      if (this.GetTrialStatusCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetTrialStatusCompleted((object) this, new GetTrialStatusCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/CreateBundleSite", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public int CreateCDWSite(
      string passcode,
      string accountID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string sku)
    {
      return (int) this.Invoke(nameof (CreateCDWSite), new object[7]
      {
        (object) passcode,
        (object) accountID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) sku
      })[0];
    }

    public IAsyncResult BeginCreateCDWSite(
      string passcode,
      string accountID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string sku,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CreateCDWSite", new object[7]
      {
        (object) passcode,
        (object) accountID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) sku
      }, callback, asyncState);
    }

    public int EndCreateCDWSite(IAsyncResult asyncResult) => (int) this.EndInvoke(asyncResult)[0];

    public void CreateCDWSiteAsync(
      string passcode,
      string accountID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string sku)
    {
      this.CreateCDWSiteAsync(passcode, accountID, clientID, userID, ptd, isTrial, sku, (object) null);
    }

    public void CreateCDWSiteAsync(
      string passcode,
      string accountID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string sku,
      object userState)
    {
      if (this.CreateCDWSiteOperationCompleted == null)
        this.CreateCDWSiteOperationCompleted = new SendOrPostCallback(this.OnCreateCDWSiteOperationCompleted);
      this.InvokeAsync("CreateCDWSite", new object[7]
      {
        (object) passcode,
        (object) accountID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) sku
      }, this.CreateCDWSiteOperationCompleted, userState);
    }

    private void OnCreateCDWSiteOperationCompleted(object arg)
    {
      if (this.CreateCDWSiteCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.CreateCDWSiteCompleted((object) this, new CreateCDWSiteCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/CreateEMSite", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public int CreateEMSite(
      string passcode,
      string accouuntID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      int quantity)
    {
      return (int) this.Invoke(nameof (CreateEMSite), new object[8]
      {
        (object) passcode,
        (object) accouuntID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) accountType,
        (object) quantity
      })[0];
    }

    public IAsyncResult BeginCreateEMSite(
      string passcode,
      string accouuntID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      int quantity,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CreateEMSite", new object[8]
      {
        (object) passcode,
        (object) accouuntID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) accountType,
        (object) quantity
      }, callback, asyncState);
    }

    public int EndCreateEMSite(IAsyncResult asyncResult) => (int) this.EndInvoke(asyncResult)[0];

    public void CreateEMSiteAsync(
      string passcode,
      string accouuntID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      int quantity)
    {
      this.CreateEMSiteAsync(passcode, accouuntID, clientID, userID, ptd, isTrial, accountType, quantity, (object) null);
    }

    public void CreateEMSiteAsync(
      string passcode,
      string accouuntID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      int quantity,
      object userState)
    {
      if (this.CreateEMSiteOperationCompleted == null)
        this.CreateEMSiteOperationCompleted = new SendOrPostCallback(this.OnCreateEMSiteOperationCompleted);
      this.InvokeAsync("CreateEMSite", new object[8]
      {
        (object) passcode,
        (object) accouuntID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) accountType,
        (object) quantity
      }, this.CreateEMSiteOperationCompleted, userState);
    }

    private void OnCreateEMSiteOperationCompleted(object arg)
    {
      if (this.CreateEMSiteCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.CreateEMSiteCompleted((object) this, new CreateEMSiteCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/CreateMirrorTPOSiteFromSiteID", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public int CreateMirrorTPOSiteFromSiteID(
      string passcode,
      string accountID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      string emsiteID)
    {
      return (int) this.Invoke(nameof (CreateMirrorTPOSiteFromSiteID), new object[8]
      {
        (object) passcode,
        (object) accountID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) accountType,
        (object) emsiteID
      })[0];
    }

    public IAsyncResult BeginCreateMirrorTPOSiteFromSiteID(
      string passcode,
      string accountID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      string emsiteID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CreateMirrorTPOSiteFromSiteID", new object[8]
      {
        (object) passcode,
        (object) accountID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) accountType,
        (object) emsiteID
      }, callback, asyncState);
    }

    public int EndCreateMirrorTPOSiteFromSiteID(IAsyncResult asyncResult)
    {
      return (int) this.EndInvoke(asyncResult)[0];
    }

    public void CreateMirrorTPOSiteFromSiteIDAsync(
      string passcode,
      string accountID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      string emsiteID)
    {
      this.CreateMirrorTPOSiteFromSiteIDAsync(passcode, accountID, clientID, userID, ptd, isTrial, accountType, emsiteID, (object) null);
    }

    public void CreateMirrorTPOSiteFromSiteIDAsync(
      string passcode,
      string accountID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      string emsiteID,
      object userState)
    {
      if (this.CreateMirrorTPOSiteFromSiteIDOperationCompleted == null)
        this.CreateMirrorTPOSiteFromSiteIDOperationCompleted = new SendOrPostCallback(this.OnCreateMirrorTPOSiteFromSiteIDOperationCompleted);
      this.InvokeAsync("CreateMirrorTPOSiteFromSiteID", new object[8]
      {
        (object) passcode,
        (object) accountID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) accountType,
        (object) emsiteID
      }, this.CreateMirrorTPOSiteFromSiteIDOperationCompleted, userState);
    }

    private void OnCreateMirrorTPOSiteFromSiteIDOperationCompleted(object arg)
    {
      if (this.CreateMirrorTPOSiteFromSiteIDCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.CreateMirrorTPOSiteFromSiteIDCompleted((object) this, new CreateMirrorTPOSiteFromSiteIDCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/CreateEMSiteReturnSiteIDs", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public int CreateEMSiteReturnSiteIDs(
      string passcode,
      string accouuntID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      int quantity,
      out string[] siteIDs)
    {
      object[] objArray = this.Invoke(nameof (CreateEMSiteReturnSiteIDs), new object[8]
      {
        (object) passcode,
        (object) accouuntID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) accountType,
        (object) quantity
      });
      siteIDs = (string[]) objArray[1];
      return (int) objArray[0];
    }

    public IAsyncResult BeginCreateEMSiteReturnSiteIDs(
      string passcode,
      string accouuntID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      int quantity,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CreateEMSiteReturnSiteIDs", new object[8]
      {
        (object) passcode,
        (object) accouuntID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) accountType,
        (object) quantity
      }, callback, asyncState);
    }

    public int EndCreateEMSiteReturnSiteIDs(IAsyncResult asyncResult, out string[] siteIDs)
    {
      object[] objArray = this.EndInvoke(asyncResult);
      siteIDs = (string[]) objArray[1];
      return (int) objArray[0];
    }

    public void CreateEMSiteReturnSiteIDsAsync(
      string passcode,
      string accouuntID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      int quantity)
    {
      this.CreateEMSiteReturnSiteIDsAsync(passcode, accouuntID, clientID, userID, ptd, isTrial, accountType, quantity, (object) null);
    }

    public void CreateEMSiteReturnSiteIDsAsync(
      string passcode,
      string accouuntID,
      string clientID,
      string userID,
      DateTime ptd,
      bool isTrial,
      string accountType,
      int quantity,
      object userState)
    {
      if (this.CreateEMSiteReturnSiteIDsOperationCompleted == null)
        this.CreateEMSiteReturnSiteIDsOperationCompleted = new SendOrPostCallback(this.OnCreateEMSiteReturnSiteIDsOperationCompleted);
      this.InvokeAsync("CreateEMSiteReturnSiteIDs", new object[8]
      {
        (object) passcode,
        (object) accouuntID,
        (object) clientID,
        (object) userID,
        (object) ptd,
        (object) isTrial,
        (object) accountType,
        (object) quantity
      }, this.CreateEMSiteReturnSiteIDsOperationCompleted, userState);
    }

    private void OnCreateEMSiteReturnSiteIDsOperationCompleted(object arg)
    {
      if (this.CreateEMSiteReturnSiteIDsCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.CreateEMSiteReturnSiteIDsCompleted((object) this, new CreateEMSiteReturnSiteIDsCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/UploadFile", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool UploadFile(string passcode, string emsiteID, [XmlElement(DataType = "base64Binary")] byte[] fileData, string fileName)
    {
      return (bool) this.Invoke(nameof (UploadFile), new object[4]
      {
        (object) passcode,
        (object) emsiteID,
        (object) fileData,
        (object) fileName
      })[0];
    }

    public IAsyncResult BeginUploadFile(
      string passcode,
      string emsiteID,
      byte[] fileData,
      string fileName,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UploadFile", new object[4]
      {
        (object) passcode,
        (object) emsiteID,
        (object) fileData,
        (object) fileName
      }, callback, asyncState);
    }

    public bool EndUploadFile(IAsyncResult asyncResult) => (bool) this.EndInvoke(asyncResult)[0];

    public void UploadFileAsync(
      string passcode,
      string emsiteID,
      byte[] fileData,
      string fileName)
    {
      this.UploadFileAsync(passcode, emsiteID, fileData, fileName, (object) null);
    }

    public void UploadFileAsync(
      string passcode,
      string emsiteID,
      byte[] fileData,
      string fileName,
      object userState)
    {
      if (this.UploadFileOperationCompleted == null)
        this.UploadFileOperationCompleted = new SendOrPostCallback(this.OnUploadFileOperationCompleted);
      this.InvokeAsync("UploadFile", new object[4]
      {
        (object) passcode,
        (object) emsiteID,
        (object) fileData,
        (object) fileName
      }, this.UploadFileOperationCompleted, userState);
    }

    private void OnUploadFileOperationCompleted(object arg)
    {
      if (this.UploadFileCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.UploadFileCompleted((object) this, new UploadFileCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/AddShowDocument", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool AddShowDocument(
      string clientID,
      Guid guid,
      [XmlElement(DataType = "base64Binary")] byte[] fileData,
      string fileName,
      string key)
    {
      return (bool) this.Invoke(nameof (AddShowDocument), new object[5]
      {
        (object) clientID,
        (object) guid,
        (object) fileData,
        (object) fileName,
        (object) key
      })[0];
    }

    public IAsyncResult BeginAddShowDocument(
      string clientID,
      Guid guid,
      byte[] fileData,
      string fileName,
      string key,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("AddShowDocument", new object[5]
      {
        (object) clientID,
        (object) guid,
        (object) fileData,
        (object) fileName,
        (object) key
      }, callback, asyncState);
    }

    public bool EndAddShowDocument(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void AddShowDocumentAsync(
      string clientID,
      Guid guid,
      byte[] fileData,
      string fileName,
      string key)
    {
      this.AddShowDocumentAsync(clientID, guid, fileData, fileName, key, (object) null);
    }

    public void AddShowDocumentAsync(
      string clientID,
      Guid guid,
      byte[] fileData,
      string fileName,
      string key,
      object userState)
    {
      if (this.AddShowDocumentOperationCompleted == null)
        this.AddShowDocumentOperationCompleted = new SendOrPostCallback(this.OnAddShowDocumentOperationCompleted);
      this.InvokeAsync("AddShowDocument", new object[5]
      {
        (object) clientID,
        (object) guid,
        (object) fileData,
        (object) fileName,
        (object) key
      }, this.AddShowDocumentOperationCompleted, userState);
    }

    private void OnAddShowDocumentOperationCompleted(object arg)
    {
      if (this.AddShowDocumentCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.AddShowDocumentCompleted((object) this, new AddShowDocumentCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/DeleteShowDocument", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool DeleteShowDocument(string clientID, Guid guid, string fileName, string key)
    {
      return (bool) this.Invoke(nameof (DeleteShowDocument), new object[4]
      {
        (object) clientID,
        (object) guid,
        (object) fileName,
        (object) key
      })[0];
    }

    public IAsyncResult BeginDeleteShowDocument(
      string clientID,
      Guid guid,
      string fileName,
      string key,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("DeleteShowDocument", new object[4]
      {
        (object) clientID,
        (object) guid,
        (object) fileName,
        (object) key
      }, callback, asyncState);
    }

    public bool EndDeleteShowDocument(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void DeleteShowDocumentAsync(string clientID, Guid guid, string fileName, string key)
    {
      this.DeleteShowDocumentAsync(clientID, guid, fileName, key, (object) null);
    }

    public void DeleteShowDocumentAsync(
      string clientID,
      Guid guid,
      string fileName,
      string key,
      object userState)
    {
      if (this.DeleteShowDocumentOperationCompleted == null)
        this.DeleteShowDocumentOperationCompleted = new SendOrPostCallback(this.OnDeleteShowDocumentOperationCompleted);
      this.InvokeAsync("DeleteShowDocument", new object[4]
      {
        (object) clientID,
        (object) guid,
        (object) fileName,
        (object) key
      }, this.DeleteShowDocumentOperationCompleted, userState);
    }

    private void OnDeleteShowDocumentOperationCompleted(object arg)
    {
      if (this.DeleteShowDocumentCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.DeleteShowDocumentCompleted((object) this, new DeleteShowDocumentCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/DeleteFiles", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool DeleteFiles(string passcode, string emsiteID)
    {
      return (bool) this.Invoke(nameof (DeleteFiles), new object[2]
      {
        (object) passcode,
        (object) emsiteID
      })[0];
    }

    public IAsyncResult BeginDeleteFiles(
      string passcode,
      string emsiteID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("DeleteFiles", new object[2]
      {
        (object) passcode,
        (object) emsiteID
      }, callback, asyncState);
    }

    public bool EndDeleteFiles(IAsyncResult asyncResult) => (bool) this.EndInvoke(asyncResult)[0];

    public void DeleteFilesAsync(string passcode, string emsiteID)
    {
      this.DeleteFilesAsync(passcode, emsiteID, (object) null);
    }

    public void DeleteFilesAsync(string passcode, string emsiteID, object userState)
    {
      if (this.DeleteFilesOperationCompleted == null)
        this.DeleteFilesOperationCompleted = new SendOrPostCallback(this.OnDeleteFilesOperationCompleted);
      this.InvokeAsync("DeleteFiles", new object[2]
      {
        (object) passcode,
        (object) emsiteID
      }, this.DeleteFilesOperationCompleted, userState);
    }

    private void OnDeleteFilesOperationCompleted(object arg)
    {
      if (this.DeleteFilesCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.DeleteFilesCompleted((object) this, new DeleteFilesCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/GetWatchCode", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetWatchCode(string clientID, string userID, string passcode)
    {
      return (string) this.Invoke(nameof (GetWatchCode), new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passcode
      })[0];
    }

    public IAsyncResult BeginGetWatchCode(
      string clientID,
      string userID,
      string passcode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetWatchCode", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passcode
      }, callback, asyncState);
    }

    public string EndGetWatchCode(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetWatchCodeAsync(string clientID, string userID, string passcode)
    {
      this.GetWatchCodeAsync(clientID, userID, passcode, (object) null);
    }

    public void GetWatchCodeAsync(
      string clientID,
      string userID,
      string passcode,
      object userState)
    {
      if (this.GetWatchCodeOperationCompleted == null)
        this.GetWatchCodeOperationCompleted = new SendOrPostCallback(this.OnGetWatchCodeOperationCompleted);
      this.InvokeAsync("GetWatchCode", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passcode
      }, this.GetWatchCodeOperationCompleted, userState);
    }

    private void OnGetWatchCodeOperationCompleted(object arg)
    {
      if (this.GetWatchCodeCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetWatchCodeCompleted((object) this, new GetWatchCodeCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/CheckClientUserID", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool CheckClientUserID(string clientID, string UserID, string passCode)
    {
      return (bool) this.Invoke(nameof (CheckClientUserID), new object[3]
      {
        (object) clientID,
        (object) UserID,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginCheckClientUserID(
      string clientID,
      string UserID,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CheckClientUserID", new object[3]
      {
        (object) clientID,
        (object) UserID,
        (object) passCode
      }, callback, asyncState);
    }

    public bool EndCheckClientUserID(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void CheckClientUserIDAsync(string clientID, string UserID, string passCode)
    {
      this.CheckClientUserIDAsync(clientID, UserID, passCode, (object) null);
    }

    public void CheckClientUserIDAsync(
      string clientID,
      string UserID,
      string passCode,
      object userState)
    {
      if (this.CheckClientUserIDOperationCompleted == null)
        this.CheckClientUserIDOperationCompleted = new SendOrPostCallback(this.OnCheckClientUserIDOperationCompleted);
      this.InvokeAsync("CheckClientUserID", new object[3]
      {
        (object) clientID,
        (object) UserID,
        (object) passCode
      }, this.CheckClientUserIDOperationCompleted, userState);
    }

    private void OnCheckClientUserIDOperationCompleted(object arg)
    {
      if (this.CheckClientUserIDCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.CheckClientUserIDCompleted((object) this, new CheckClientUserIDCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/HasAccoutingID", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool HasAccoutingID(string accountID, string siteType, string passCode)
    {
      return (bool) this.Invoke(nameof (HasAccoutingID), new object[3]
      {
        (object) accountID,
        (object) siteType,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginHasAccoutingID(
      string accountID,
      string siteType,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("HasAccoutingID", new object[3]
      {
        (object) accountID,
        (object) siteType,
        (object) passCode
      }, callback, asyncState);
    }

    public bool EndHasAccoutingID(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void HasAccoutingIDAsync(string accountID, string siteType, string passCode)
    {
      this.HasAccoutingIDAsync(accountID, siteType, passCode, (object) null);
    }

    public void HasAccoutingIDAsync(
      string accountID,
      string siteType,
      string passCode,
      object userState)
    {
      if (this.HasAccoutingIDOperationCompleted == null)
        this.HasAccoutingIDOperationCompleted = new SendOrPostCallback(this.OnHasAccoutingIDOperationCompleted);
      this.InvokeAsync("HasAccoutingID", new object[3]
      {
        (object) accountID,
        (object) siteType,
        (object) passCode
      }, this.HasAccoutingIDOperationCompleted, userState);
    }

    private void OnHasAccoutingIDOperationCompleted(object arg)
    {
      if (this.HasAccoutingIDCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.HasAccoutingIDCompleted((object) this, new HasAccoutingIDCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/AddBundlePurchase", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public int AddBundlePurchase(string clientID, DateTime pTD, string passCode)
    {
      return (int) this.Invoke(nameof (AddBundlePurchase), new object[3]
      {
        (object) clientID,
        (object) pTD,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginAddBundlePurchase(
      string clientID,
      DateTime pTD,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("AddBundlePurchase", new object[3]
      {
        (object) clientID,
        (object) pTD,
        (object) passCode
      }, callback, asyncState);
    }

    public int EndAddBundlePurchase(IAsyncResult asyncResult)
    {
      return (int) this.EndInvoke(asyncResult)[0];
    }

    public void AddBundlePurchaseAsync(string clientID, DateTime pTD, string passCode)
    {
      this.AddBundlePurchaseAsync(clientID, pTD, passCode, (object) null);
    }

    public void AddBundlePurchaseAsync(
      string clientID,
      DateTime pTD,
      string passCode,
      object userState)
    {
      if (this.AddBundlePurchaseOperationCompleted == null)
        this.AddBundlePurchaseOperationCompleted = new SendOrPostCallback(this.OnAddBundlePurchaseOperationCompleted);
      this.InvokeAsync("AddBundlePurchase", new object[3]
      {
        (object) clientID,
        (object) pTD,
        (object) passCode
      }, this.AddBundlePurchaseOperationCompleted, userState);
    }

    private void OnAddBundlePurchaseOperationCompleted(object arg)
    {
      if (this.AddBundlePurchaseCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.AddBundlePurchaseCompleted((object) this, new AddBundlePurchaseCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/CenterWiseNotConfigured", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool CenterWiseNotConfigured(string clientID)
    {
      return (bool) this.Invoke(nameof (CenterWiseNotConfigured), new object[1]
      {
        (object) clientID
      })[0];
    }

    public IAsyncResult BeginCenterWiseNotConfigured(
      string clientID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CenterWiseNotConfigured", new object[1]
      {
        (object) clientID
      }, callback, asyncState);
    }

    public bool EndCenterWiseNotConfigured(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void CenterWiseNotConfiguredAsync(string clientID)
    {
      this.CenterWiseNotConfiguredAsync(clientID, (object) null);
    }

    public void CenterWiseNotConfiguredAsync(string clientID, object userState)
    {
      if (this.CenterWiseNotConfiguredOperationCompleted == null)
        this.CenterWiseNotConfiguredOperationCompleted = new SendOrPostCallback(this.OnCenterWiseNotConfiguredOperationCompleted);
      this.InvokeAsync("CenterWiseNotConfigured", new object[1]
      {
        (object) clientID
      }, this.CenterWiseNotConfiguredOperationCompleted, userState);
    }

    private void OnCenterWiseNotConfiguredOperationCompleted(object arg)
    {
      if (this.CenterWiseNotConfiguredCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.CenterWiseNotConfiguredCompleted((object) this, new CenterWiseNotConfiguredCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/HasUnconfiguredCenterWiseSite", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool HasUnconfiguredCenterWiseSite(string clientID, string userID)
    {
      return (bool) this.Invoke(nameof (HasUnconfiguredCenterWiseSite), new object[2]
      {
        (object) clientID,
        (object) userID
      })[0];
    }

    public IAsyncResult BeginHasUnconfiguredCenterWiseSite(
      string clientID,
      string userID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("HasUnconfiguredCenterWiseSite", new object[2]
      {
        (object) clientID,
        (object) userID
      }, callback, asyncState);
    }

    public bool EndHasUnconfiguredCenterWiseSite(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void HasUnconfiguredCenterWiseSiteAsync(string clientID, string userID)
    {
      this.HasUnconfiguredCenterWiseSiteAsync(clientID, userID, (object) null);
    }

    public void HasUnconfiguredCenterWiseSiteAsync(
      string clientID,
      string userID,
      object userState)
    {
      if (this.HasUnconfiguredCenterWiseSiteOperationCompleted == null)
        this.HasUnconfiguredCenterWiseSiteOperationCompleted = new SendOrPostCallback(this.OnHasUnconfiguredCenterWiseSiteOperationCompleted);
      this.InvokeAsync("HasUnconfiguredCenterWiseSite", new object[2]
      {
        (object) clientID,
        (object) userID
      }, this.HasUnconfiguredCenterWiseSiteOperationCompleted, userState);
    }

    private void OnHasUnconfiguredCenterWiseSiteOperationCompleted(object arg)
    {
      if (this.HasUnconfiguredCenterWiseSiteCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.HasUnconfiguredCenterWiseSiteCompleted((object) this, new HasUnconfiguredCenterWiseSiteCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/CreateCenterWiseLOSite", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool CreateCenterWiseLOSite(string clientID, string userID)
    {
      return (bool) this.Invoke(nameof (CreateCenterWiseLOSite), new object[2]
      {
        (object) clientID,
        (object) userID
      })[0];
    }

    public IAsyncResult BeginCreateCenterWiseLOSite(
      string clientID,
      string userID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CreateCenterWiseLOSite", new object[2]
      {
        (object) clientID,
        (object) userID
      }, callback, asyncState);
    }

    public bool EndCreateCenterWiseLOSite(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void CreateCenterWiseLOSiteAsync(string clientID, string userID)
    {
      this.CreateCenterWiseLOSiteAsync(clientID, userID, (object) null);
    }

    public void CreateCenterWiseLOSiteAsync(string clientID, string userID, object userState)
    {
      if (this.CreateCenterWiseLOSiteOperationCompleted == null)
        this.CreateCenterWiseLOSiteOperationCompleted = new SendOrPostCallback(this.OnCreateCenterWiseLOSiteOperationCompleted);
      this.InvokeAsync("CreateCenterWiseLOSite", new object[2]
      {
        (object) clientID,
        (object) userID
      }, this.CreateCenterWiseLOSiteOperationCompleted, userState);
    }

    private void OnCreateCenterWiseLOSiteOperationCompleted(object arg)
    {
      if (this.CreateCenterWiseLOSiteCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.CreateCenterWiseLOSiteCompleted((object) this, new CreateCenterWiseLOSiteCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/CreateCenterWiseCorporateSite", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool CreateCenterWiseCorporateSite(string clientID)
    {
      return (bool) this.Invoke(nameof (CreateCenterWiseCorporateSite), new object[1]
      {
        (object) clientID
      })[0];
    }

    public IAsyncResult BeginCreateCenterWiseCorporateSite(
      string clientID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CreateCenterWiseCorporateSite", new object[1]
      {
        (object) clientID
      }, callback, asyncState);
    }

    public bool EndCreateCenterWiseCorporateSite(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void CreateCenterWiseCorporateSiteAsync(string clientID)
    {
      this.CreateCenterWiseCorporateSiteAsync(clientID, (object) null);
    }

    public void CreateCenterWiseCorporateSiteAsync(string clientID, object userState)
    {
      if (this.CreateCenterWiseCorporateSiteOperationCompleted == null)
        this.CreateCenterWiseCorporateSiteOperationCompleted = new SendOrPostCallback(this.OnCreateCenterWiseCorporateSiteOperationCompleted);
      this.InvokeAsync("CreateCenterWiseCorporateSite", new object[1]
      {
        (object) clientID
      }, this.CreateCenterWiseCorporateSiteOperationCompleted, userState);
    }

    private void OnCreateCenterWiseCorporateSiteOperationCompleted(object arg)
    {
      if (this.CreateCenterWiseCorporateSiteCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.CreateCenterWiseCorporateSiteCompleted((object) this, new CreateCenterWiseCorporateSiteCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/GetPortablePageCustomUrl", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetPortablePageCustomUrl(string emsiteID)
    {
      return (string) this.Invoke(nameof (GetPortablePageCustomUrl), new object[1]
      {
        (object) emsiteID
      })[0];
    }

    public IAsyncResult BeginGetPortablePageCustomUrl(
      string emsiteID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetPortablePageCustomUrl", new object[1]
      {
        (object) emsiteID
      }, callback, asyncState);
    }

    public string EndGetPortablePageCustomUrl(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetPortablePageCustomUrlAsync(string emsiteID)
    {
      this.GetPortablePageCustomUrlAsync(emsiteID, (object) null);
    }

    public void GetPortablePageCustomUrlAsync(string emsiteID, object userState)
    {
      if (this.GetPortablePageCustomUrlOperationCompleted == null)
        this.GetPortablePageCustomUrlOperationCompleted = new SendOrPostCallback(this.OnGetPortablePageCustomUrlOperationCompleted);
      this.InvokeAsync("GetPortablePageCustomUrl", new object[1]
      {
        (object) emsiteID
      }, this.GetPortablePageCustomUrlOperationCompleted, userState);
    }

    private void OnGetPortablePageCustomUrlOperationCompleted(object arg)
    {
      if (this.GetPortablePageCustomUrlCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetPortablePageCustomUrlCompleted((object) this, new GetPortablePageCustomUrlCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [WebMethod(MessageName = "CenterWiseNotConfigured1")]
    [SoapDocumentMethod("https://www.elliemae.com/CenterWiseNotConfiguredSecure", RequestElementName = "CenterWiseNotConfiguredSecure", RequestNamespace = "https://www.elliemae.com/", ResponseElementName = "CenterWiseNotConfiguredSecureResponse", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [return: XmlElement("CenterWiseNotConfiguredSecureResult")]
    public bool CenterWiseNotConfigured(string clientID, string passCode)
    {
      return (bool) this.Invoke("CenterWiseNotConfigured1", new object[2]
      {
        (object) clientID,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginCenterWiseNotConfigured1(
      string clientID,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CenterWiseNotConfigured1", new object[2]
      {
        (object) clientID,
        (object) passCode
      }, callback, asyncState);
    }

    public bool EndCenterWiseNotConfigured1(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void CenterWiseNotConfigured1Async(string clientID, string passCode)
    {
      this.CenterWiseNotConfigured1Async(clientID, passCode, (object) null);
    }

    public void CenterWiseNotConfigured1Async(string clientID, string passCode, object userState)
    {
      if (this.CenterWiseNotConfigured1OperationCompleted == null)
        this.CenterWiseNotConfigured1OperationCompleted = new SendOrPostCallback(this.OnCenterWiseNotConfigured1OperationCompleted);
      this.InvokeAsync("CenterWiseNotConfigured1", new object[2]
      {
        (object) clientID,
        (object) passCode
      }, this.CenterWiseNotConfigured1OperationCompleted, userState);
    }

    private void OnCenterWiseNotConfigured1OperationCompleted(object arg)
    {
      if (this.CenterWiseNotConfigured1Completed == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.CenterWiseNotConfigured1Completed((object) this, new CenterWiseNotConfigured1CompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [WebMethod(MessageName = "CreateCenterWiseCorporateSite1")]
    [SoapDocumentMethod("https://www.elliemae.com/CreateCenterWiseCorporateSiteSecure", RequestElementName = "CreateCenterWiseCorporateSiteSecure", RequestNamespace = "https://www.elliemae.com/", ResponseElementName = "CreateCenterWiseCorporateSiteSecureResponse", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [return: XmlElement("CreateCenterWiseCorporateSiteSecureResult")]
    public bool CreateCenterWiseCorporateSite(string clientID, string passCode)
    {
      return (bool) this.Invoke("CreateCenterWiseCorporateSite1", new object[2]
      {
        (object) clientID,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginCreateCenterWiseCorporateSite1(
      string clientID,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CreateCenterWiseCorporateSite1", new object[2]
      {
        (object) clientID,
        (object) passCode
      }, callback, asyncState);
    }

    public bool EndCreateCenterWiseCorporateSite1(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void CreateCenterWiseCorporateSite1Async(string clientID, string passCode)
    {
      this.CreateCenterWiseCorporateSite1Async(clientID, passCode, (object) null);
    }

    public void CreateCenterWiseCorporateSite1Async(
      string clientID,
      string passCode,
      object userState)
    {
      if (this.CreateCenterWiseCorporateSite1OperationCompleted == null)
        this.CreateCenterWiseCorporateSite1OperationCompleted = new SendOrPostCallback(this.OnCreateCenterWiseCorporateSite1OperationCompleted);
      this.InvokeAsync("CreateCenterWiseCorporateSite1", new object[2]
      {
        (object) clientID,
        (object) passCode
      }, this.CreateCenterWiseCorporateSite1OperationCompleted, userState);
    }

    private void OnCreateCenterWiseCorporateSite1OperationCompleted(object arg)
    {
      if (this.CreateCenterWiseCorporateSite1Completed == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.CreateCenterWiseCorporateSite1Completed((object) this, new CreateCenterWiseCorporateSite1CompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [WebMethod(MessageName = "CreateCenterWiseLOSite1")]
    [SoapDocumentMethod("https://www.elliemae.com/CreateCenterWiseLOSiteSecure", RequestElementName = "CreateCenterWiseLOSiteSecure", RequestNamespace = "https://www.elliemae.com/", ResponseElementName = "CreateCenterWiseLOSiteSecureResponse", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [return: XmlElement("CreateCenterWiseLOSiteSecureResult")]
    public bool CreateCenterWiseLOSite(string clientID, string userID, string passCode)
    {
      return (bool) this.Invoke("CreateCenterWiseLOSite1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginCreateCenterWiseLOSite1(
      string clientID,
      string userID,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CreateCenterWiseLOSite1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      }, callback, asyncState);
    }

    public bool EndCreateCenterWiseLOSite1(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void CreateCenterWiseLOSite1Async(string clientID, string userID, string passCode)
    {
      this.CreateCenterWiseLOSite1Async(clientID, userID, passCode, (object) null);
    }

    public void CreateCenterWiseLOSite1Async(
      string clientID,
      string userID,
      string passCode,
      object userState)
    {
      if (this.CreateCenterWiseLOSite1OperationCompleted == null)
        this.CreateCenterWiseLOSite1OperationCompleted = new SendOrPostCallback(this.OnCreateCenterWiseLOSite1OperationCompleted);
      this.InvokeAsync("CreateCenterWiseLOSite1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      }, this.CreateCenterWiseLOSite1OperationCompleted, userState);
    }

    private void OnCreateCenterWiseLOSite1OperationCompleted(object arg)
    {
      if (this.CreateCenterWiseLOSite1Completed == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.CreateCenterWiseLOSite1Completed((object) this, new CreateCenterWiseLOSite1CompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [WebMethod(MessageName = "GetEMSiteIDByClientIDUserID1")]
    [SoapDocumentMethod("http://www.elliemae.com/GetEMSiteIDByClientIDUserIDSecure", RequestElementName = "GetEMSiteIDByClientIDUserIDSecure", RequestNamespace = "http://www.elliemae.com/", ResponseElementName = "GetEMSiteIDByClientIDUserIDSecureResponse", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [return: XmlElement("GetEMSiteIDByClientIDUserIDSecureResult")]
    public string GetEMSiteIDByClientIDUserID(string clientID, string userID, string passCode)
    {
      return (string) this.Invoke("GetEMSiteIDByClientIDUserID1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginGetEMSiteIDByClientIDUserID1(
      string clientID,
      string userID,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetEMSiteIDByClientIDUserID1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      }, callback, asyncState);
    }

    public string EndGetEMSiteIDByClientIDUserID1(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetEMSiteIDByClientIDUserID1Async(string clientID, string userID, string passCode)
    {
      this.GetEMSiteIDByClientIDUserID1Async(clientID, userID, passCode, (object) null);
    }

    public void GetEMSiteIDByClientIDUserID1Async(
      string clientID,
      string userID,
      string passCode,
      object userState)
    {
      if (this.GetEMSiteIDByClientIDUserID1OperationCompleted == null)
        this.GetEMSiteIDByClientIDUserID1OperationCompleted = new SendOrPostCallback(this.OnGetEMSiteIDByClientIDUserID1OperationCompleted);
      this.InvokeAsync("GetEMSiteIDByClientIDUserID1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      }, this.GetEMSiteIDByClientIDUserID1OperationCompleted, userState);
    }

    private void OnGetEMSiteIDByClientIDUserID1OperationCompleted(object arg)
    {
      if (this.GetEMSiteIDByClientIDUserID1Completed == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetEMSiteIDByClientIDUserID1Completed((object) this, new GetEMSiteIDByClientIDUserID1CompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [WebMethod(MessageName = "GetPortablePageCustomUrl1")]
    [SoapDocumentMethod("https://www.elliemae.com/GetPortablePageCustomUrlSecure", RequestElementName = "GetPortablePageCustomUrlSecure", RequestNamespace = "https://www.elliemae.com/", ResponseElementName = "GetPortablePageCustomUrlSecureResponse", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [return: XmlElement("GetPortablePageCustomUrlSecureResult")]
    public string GetPortablePageCustomUrl(string emsiteID, string passCode)
    {
      return (string) this.Invoke("GetPortablePageCustomUrl1", new object[2]
      {
        (object) emsiteID,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginGetPortablePageCustomUrl1(
      string emsiteID,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetPortablePageCustomUrl1", new object[2]
      {
        (object) emsiteID,
        (object) passCode
      }, callback, asyncState);
    }

    public string EndGetPortablePageCustomUrl1(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetPortablePageCustomUrl1Async(string emsiteID, string passCode)
    {
      this.GetPortablePageCustomUrl1Async(emsiteID, passCode, (object) null);
    }

    public void GetPortablePageCustomUrl1Async(string emsiteID, string passCode, object userState)
    {
      if (this.GetPortablePageCustomUrl1OperationCompleted == null)
        this.GetPortablePageCustomUrl1OperationCompleted = new SendOrPostCallback(this.OnGetPortablePageCustomUrl1OperationCompleted);
      this.InvokeAsync("GetPortablePageCustomUrl1", new object[2]
      {
        (object) emsiteID,
        (object) passCode
      }, this.GetPortablePageCustomUrl1OperationCompleted, userState);
    }

    private void OnGetPortablePageCustomUrl1OperationCompleted(object arg)
    {
      if (this.GetPortablePageCustomUrl1Completed == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetPortablePageCustomUrl1Completed((object) this, new GetPortablePageCustomUrl1CompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [WebMethod(MessageName = "GetTrialStatus1")]
    [SoapDocumentMethod("http://www.elliemae.com/GetTrialStatusSecure", RequestElementName = "GetTrialStatusSecure", RequestNamespace = "http://www.elliemae.com/", ResponseElementName = "GetTrialStatusSecureResponse", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [return: XmlElement("GetTrialStatusSecureResult")]
    public int GetTrialStatus(string clientID, string userID, string passCode)
    {
      return (int) this.Invoke("GetTrialStatus1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginGetTrialStatus1(
      string clientID,
      string userID,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetTrialStatus1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      }, callback, asyncState);
    }

    public int EndGetTrialStatus1(IAsyncResult asyncResult) => (int) this.EndInvoke(asyncResult)[0];

    public void GetTrialStatus1Async(string clientID, string userID, string passCode)
    {
      this.GetTrialStatus1Async(clientID, userID, passCode, (object) null);
    }

    public void GetTrialStatus1Async(
      string clientID,
      string userID,
      string passCode,
      object userState)
    {
      if (this.GetTrialStatus1OperationCompleted == null)
        this.GetTrialStatus1OperationCompleted = new SendOrPostCallback(this.OnGetTrialStatus1OperationCompleted);
      this.InvokeAsync("GetTrialStatus1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      }, this.GetTrialStatus1OperationCompleted, userState);
    }

    private void OnGetTrialStatus1OperationCompleted(object arg)
    {
      if (this.GetTrialStatus1Completed == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetTrialStatus1Completed((object) this, new GetTrialStatus1CompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [WebMethod(MessageName = "HasUnconfiguredCenterWiseSite1")]
    [SoapDocumentMethod("https://www.elliemae.com/HasUnconfiguredCenterWiseSiteSecure", RequestElementName = "HasUnconfiguredCenterWiseSiteSecure", RequestNamespace = "https://www.elliemae.com/", ResponseElementName = "HasUnconfiguredCenterWiseSiteSecureResponse", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [return: XmlElement("HasUnconfiguredCenterWiseSiteSecureResult")]
    public bool HasUnconfiguredCenterWiseSite(string clientID, string userID, string passCode)
    {
      return (bool) this.Invoke("HasUnconfiguredCenterWiseSite1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginHasUnconfiguredCenterWiseSite1(
      string clientID,
      string userID,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("HasUnconfiguredCenterWiseSite1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      }, callback, asyncState);
    }

    public bool EndHasUnconfiguredCenterWiseSite1(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void HasUnconfiguredCenterWiseSite1Async(
      string clientID,
      string userID,
      string passCode)
    {
      this.HasUnconfiguredCenterWiseSite1Async(clientID, userID, passCode, (object) null);
    }

    public void HasUnconfiguredCenterWiseSite1Async(
      string clientID,
      string userID,
      string passCode,
      object userState)
    {
      if (this.HasUnconfiguredCenterWiseSite1OperationCompleted == null)
        this.HasUnconfiguredCenterWiseSite1OperationCompleted = new SendOrPostCallback(this.OnHasUnconfiguredCenterWiseSite1OperationCompleted);
      this.InvokeAsync("HasUnconfiguredCenterWiseSite1", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      }, this.HasUnconfiguredCenterWiseSite1OperationCompleted, userState);
    }

    private void OnHasUnconfiguredCenterWiseSite1OperationCompleted(object arg)
    {
      if (this.HasUnconfiguredCenterWiseSite1Completed == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.HasUnconfiguredCenterWiseSite1Completed((object) this, new HasUnconfiguredCenterWiseSite1CompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/HasActiveWebCenter", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool HasActiveWebCenter(string clientID, string passCode)
    {
      return (bool) this.Invoke(nameof (HasActiveWebCenter), new object[2]
      {
        (object) clientID,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginHasActiveWebCenter(
      string clientID,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("HasActiveWebCenter", new object[2]
      {
        (object) clientID,
        (object) passCode
      }, callback, asyncState);
    }

    public bool EndHasActiveWebCenter(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void HasActiveWebCenterAsync(string clientID, string passCode)
    {
      this.HasActiveWebCenterAsync(clientID, passCode, (object) null);
    }

    public void HasActiveWebCenterAsync(string clientID, string passCode, object userState)
    {
      if (this.HasActiveWebCenterOperationCompleted == null)
        this.HasActiveWebCenterOperationCompleted = new SendOrPostCallback(this.OnHasActiveWebCenterOperationCompleted);
      this.InvokeAsync("HasActiveWebCenter", new object[2]
      {
        (object) clientID,
        (object) passCode
      }, this.HasActiveWebCenterOperationCompleted, userState);
    }

    private void OnHasActiveWebCenterOperationCompleted(object arg)
    {
      if (this.HasActiveWebCenterCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.HasActiveWebCenterCompleted((object) this, new HasActiveWebCenterCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/IsWebCenterAdmin", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool IsWebCenterAdmin(string clientID, string userID, string passCode)
    {
      return (bool) this.Invoke(nameof (IsWebCenterAdmin), new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginIsWebCenterAdmin(
      string clientID,
      string userID,
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("IsWebCenterAdmin", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      }, callback, asyncState);
    }

    public bool EndIsWebCenterAdmin(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void IsWebCenterAdminAsync(string clientID, string userID, string passCode)
    {
      this.IsWebCenterAdminAsync(clientID, userID, passCode, (object) null);
    }

    public void IsWebCenterAdminAsync(
      string clientID,
      string userID,
      string passCode,
      object userState)
    {
      if (this.IsWebCenterAdminOperationCompleted == null)
        this.IsWebCenterAdminOperationCompleted = new SendOrPostCallback(this.OnIsWebCenterAdminOperationCompleted);
      this.InvokeAsync("IsWebCenterAdmin", new object[3]
      {
        (object) clientID,
        (object) userID,
        (object) passCode
      }, this.IsWebCenterAdminOperationCompleted, userState);
    }

    private void OnIsWebCenterAdminOperationCompleted(object arg)
    {
      if (this.IsWebCenterAdminCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.IsWebCenterAdminCompleted((object) this, new IsWebCenterAdminCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/UnlockTPOLoans", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string UnlockTPOLoans(string passCode)
    {
      return (string) this.Invoke(nameof (UnlockTPOLoans), new object[1]
      {
        (object) passCode
      })[0];
    }

    public IAsyncResult BeginUnlockTPOLoans(
      string passCode,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UnlockTPOLoans", new object[1]
      {
        (object) passCode
      }, callback, asyncState);
    }

    public string EndUnlockTPOLoans(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void UnlockTPOLoansAsync(string passCode)
    {
      this.UnlockTPOLoansAsync(passCode, (object) null);
    }

    public void UnlockTPOLoansAsync(string passCode, object userState)
    {
      if (this.UnlockTPOLoansOperationCompleted == null)
        this.UnlockTPOLoansOperationCompleted = new SendOrPostCallback(this.OnUnlockTPOLoansOperationCompleted);
      this.InvokeAsync("UnlockTPOLoans", new object[1]
      {
        (object) passCode
      }, this.UnlockTPOLoansOperationCompleted, userState);
    }

    private void OnUnlockTPOLoansOperationCompleted(object arg)
    {
      if (this.UnlockTPOLoansCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.UnlockTPOLoansCompleted((object) this, new UnlockTPOLoansCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/GetTPODocumentList", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetTPODocumentList(string emsiteID)
    {
      return (string) this.Invoke(nameof (GetTPODocumentList), new object[1]
      {
        (object) emsiteID
      })[0];
    }

    public IAsyncResult BeginGetTPODocumentList(
      string emsiteID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetTPODocumentList", new object[1]
      {
        (object) emsiteID
      }, callback, asyncState);
    }

    public string EndGetTPODocumentList(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetTPODocumentListAsync(string emsiteID)
    {
      this.GetTPODocumentListAsync(emsiteID, (object) null);
    }

    public void GetTPODocumentListAsync(string emsiteID, object userState)
    {
      if (this.GetTPODocumentListOperationCompleted == null)
        this.GetTPODocumentListOperationCompleted = new SendOrPostCallback(this.OnGetTPODocumentListOperationCompleted);
      this.InvokeAsync("GetTPODocumentList", new object[1]
      {
        (object) emsiteID
      }, this.GetTPODocumentListOperationCompleted, userState);
    }

    private void OnGetTPODocumentListOperationCompleted(object arg)
    {
      if (this.GetTPODocumentListCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetTPODocumentListCompleted((object) this, new GetTPODocumentListCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/GetTPOLoanNumber", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetTPOLoanNumber(string emsiteID, string loanGuid)
    {
      return (string) this.Invoke(nameof (GetTPOLoanNumber), new object[2]
      {
        (object) emsiteID,
        (object) loanGuid
      })[0];
    }

    public IAsyncResult BeginGetTPOLoanNumber(
      string emsiteID,
      string loanGuid,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetTPOLoanNumber", new object[2]
      {
        (object) emsiteID,
        (object) loanGuid
      }, callback, asyncState);
    }

    public string EndGetTPOLoanNumber(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetTPOLoanNumberAsync(string emsiteID, string loanGuid)
    {
      this.GetTPOLoanNumberAsync(emsiteID, loanGuid, (object) null);
    }

    public void GetTPOLoanNumberAsync(string emsiteID, string loanGuid, object userState)
    {
      if (this.GetTPOLoanNumberOperationCompleted == null)
        this.GetTPOLoanNumberOperationCompleted = new SendOrPostCallback(this.OnGetTPOLoanNumberOperationCompleted);
      this.InvokeAsync("GetTPOLoanNumber", new object[2]
      {
        (object) emsiteID,
        (object) loanGuid
      }, this.GetTPOLoanNumberOperationCompleted, userState);
    }

    private void OnGetTPOLoanNumberOperationCompleted(object arg)
    {
      if (this.GetTPOLoanNumberCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetTPOLoanNumberCompleted((object) this, new GetTPOLoanNumberCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/GetTPOLenderLoanGuid", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetTPOLenderLoanGuid(string emsiteID, string loanGuid)
    {
      return (string) this.Invoke(nameof (GetTPOLenderLoanGuid), new object[2]
      {
        (object) emsiteID,
        (object) loanGuid
      })[0];
    }

    public IAsyncResult BeginGetTPOLenderLoanGuid(
      string emsiteID,
      string loanGuid,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetTPOLenderLoanGuid", new object[2]
      {
        (object) emsiteID,
        (object) loanGuid
      }, callback, asyncState);
    }

    public string EndGetTPOLenderLoanGuid(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetTPOLenderLoanGuidAsync(string emsiteID, string loanGuid)
    {
      this.GetTPOLenderLoanGuidAsync(emsiteID, loanGuid, (object) null);
    }

    public void GetTPOLenderLoanGuidAsync(string emsiteID, string loanGuid, object userState)
    {
      if (this.GetTPOLenderLoanGuidOperationCompleted == null)
        this.GetTPOLenderLoanGuidOperationCompleted = new SendOrPostCallback(this.OnGetTPOLenderLoanGuidOperationCompleted);
      this.InvokeAsync("GetTPOLenderLoanGuid", new object[2]
      {
        (object) emsiteID,
        (object) loanGuid
      }, this.GetTPOLenderLoanGuidOperationCompleted, userState);
    }

    private void OnGetTPOLenderLoanGuidOperationCompleted(object arg)
    {
      if (this.GetTPOLenderLoanGuidCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetTPOLenderLoanGuidCompleted((object) this, new GetTPOLenderLoanGuidCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/ValidateTPOLoginAndLoanNumber", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool ValidateTPOLoginAndLoanNumber(
      string emsiteID,
      string userID,
      string password,
      string loanNumber)
    {
      return (bool) this.Invoke(nameof (ValidateTPOLoginAndLoanNumber), new object[4]
      {
        (object) emsiteID,
        (object) userID,
        (object) password,
        (object) loanNumber
      })[0];
    }

    public IAsyncResult BeginValidateTPOLoginAndLoanNumber(
      string emsiteID,
      string userID,
      string password,
      string loanNumber,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("ValidateTPOLoginAndLoanNumber", new object[4]
      {
        (object) emsiteID,
        (object) userID,
        (object) password,
        (object) loanNumber
      }, callback, asyncState);
    }

    public bool EndValidateTPOLoginAndLoanNumber(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void ValidateTPOLoginAndLoanNumberAsync(
      string emsiteID,
      string userID,
      string password,
      string loanNumber)
    {
      this.ValidateTPOLoginAndLoanNumberAsync(emsiteID, userID, password, loanNumber, (object) null);
    }

    public void ValidateTPOLoginAndLoanNumberAsync(
      string emsiteID,
      string userID,
      string password,
      string loanNumber,
      object userState)
    {
      if (this.ValidateTPOLoginAndLoanNumberOperationCompleted == null)
        this.ValidateTPOLoginAndLoanNumberOperationCompleted = new SendOrPostCallback(this.OnValidateTPOLoginAndLoanNumberOperationCompleted);
      this.InvokeAsync("ValidateTPOLoginAndLoanNumber", new object[4]
      {
        (object) emsiteID,
        (object) userID,
        (object) password,
        (object) loanNumber
      }, this.ValidateTPOLoginAndLoanNumberOperationCompleted, userState);
    }

    private void OnValidateTPOLoginAndLoanNumberOperationCompleted(object arg)
    {
      if (this.ValidateTPOLoginAndLoanNumberCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.ValidateTPOLoginAndLoanNumberCompleted((object) this, new ValidateTPOLoginAndLoanNumberCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/UploadTPODocuments", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void UploadTPODocuments(string emsiteID, [XmlElement(DataType = "base64Binary")] byte[] data)
    {
      this.Invoke(nameof (UploadTPODocuments), new object[2]
      {
        (object) emsiteID,
        (object) data
      });
    }

    public IAsyncResult BeginUploadTPODocuments(
      string emsiteID,
      byte[] data,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UploadTPODocuments", new object[2]
      {
        (object) emsiteID,
        (object) data
      }, callback, asyncState);
    }

    public void EndUploadTPODocuments(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void UploadTPODocumentsAsync(string emsiteID, byte[] data)
    {
      this.UploadTPODocumentsAsync(emsiteID, data, (object) null);
    }

    public void UploadTPODocumentsAsync(string emsiteID, byte[] data, object userState)
    {
      if (this.UploadTPODocumentsOperationCompleted == null)
        this.UploadTPODocumentsOperationCompleted = new SendOrPostCallback(this.OnUploadTPODocumentsOperationCompleted);
      this.InvokeAsync("UploadTPODocuments", new object[2]
      {
        (object) emsiteID,
        (object) data
      }, this.UploadTPODocumentsOperationCompleted, userState);
    }

    private void OnUploadTPODocumentsOperationCompleted(object arg)
    {
      if (this.UploadTPODocumentsCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.UploadTPODocumentsCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/GetTPOLocation", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetTPOLocation(string clientID)
    {
      return (string) this.Invoke(nameof (GetTPOLocation), new object[1]
      {
        (object) clientID
      })[0];
    }

    public IAsyncResult BeginGetTPOLocation(
      string clientID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetTPOLocation", new object[1]
      {
        (object) clientID
      }, callback, asyncState);
    }

    public string EndGetTPOLocation(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetTPOLocationAsync(string clientID)
    {
      this.GetTPOLocationAsync(clientID, (object) null);
    }

    public void GetTPOLocationAsync(string clientID, object userState)
    {
      if (this.GetTPOLocationOperationCompleted == null)
        this.GetTPOLocationOperationCompleted = new SendOrPostCallback(this.OnGetTPOLocationOperationCompleted);
      this.InvokeAsync("GetTPOLocation", new object[1]
      {
        (object) clientID
      }, this.GetTPOLocationOperationCompleted, userState);
    }

    private void OnGetTPOLocationOperationCompleted(object arg)
    {
      if (this.GetTPOLocationCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetTPOLocationCompleted((object) this, new GetTPOLocationCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/GetTPOInformationWithClientID", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetTPOInformationWithClientID(
      string clientID,
      string userid,
      string password,
      string key,
      string companyID,
      string branchID,
      string loID,
      string lpID)
    {
      return (string) this.Invoke(nameof (GetTPOInformationWithClientID), new object[8]
      {
        (object) clientID,
        (object) userid,
        (object) password,
        (object) key,
        (object) companyID,
        (object) branchID,
        (object) loID,
        (object) lpID
      })[0];
    }

    public IAsyncResult BeginGetTPOInformationWithClientID(
      string clientID,
      string userid,
      string password,
      string key,
      string companyID,
      string branchID,
      string loID,
      string lpID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetTPOInformationWithClientID", new object[8]
      {
        (object) clientID,
        (object) userid,
        (object) password,
        (object) key,
        (object) companyID,
        (object) branchID,
        (object) loID,
        (object) lpID
      }, callback, asyncState);
    }

    public string EndGetTPOInformationWithClientID(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetTPOInformationWithClientIDAsync(
      string clientID,
      string userid,
      string password,
      string key,
      string companyID,
      string branchID,
      string loID,
      string lpID)
    {
      this.GetTPOInformationWithClientIDAsync(clientID, userid, password, key, companyID, branchID, loID, lpID, (object) null);
    }

    public void GetTPOInformationWithClientIDAsync(
      string clientID,
      string userid,
      string password,
      string key,
      string companyID,
      string branchID,
      string loID,
      string lpID,
      object userState)
    {
      if (this.GetTPOInformationWithClientIDOperationCompleted == null)
        this.GetTPOInformationWithClientIDOperationCompleted = new SendOrPostCallback(this.OnGetTPOInformationWithClientIDOperationCompleted);
      this.InvokeAsync("GetTPOInformationWithClientID", new object[8]
      {
        (object) clientID,
        (object) userid,
        (object) password,
        (object) key,
        (object) companyID,
        (object) branchID,
        (object) loID,
        (object) lpID
      }, this.GetTPOInformationWithClientIDOperationCompleted, userState);
    }

    private void OnGetTPOInformationWithClientIDOperationCompleted(object arg)
    {
      if (this.GetTPOInformationWithClientIDCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetTPOInformationWithClientIDCompleted((object) this, new GetTPOInformationWithClientIDCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/GetTPOInformation", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetTPOInformation(
      string clientID,
      string userid,
      string password,
      string key,
      string companyID,
      string branchID,
      string loID,
      string lpID)
    {
      return (string) this.Invoke(nameof (GetTPOInformation), new object[8]
      {
        (object) clientID,
        (object) userid,
        (object) password,
        (object) key,
        (object) companyID,
        (object) branchID,
        (object) loID,
        (object) lpID
      })[0];
    }

    public IAsyncResult BeginGetTPOInformation(
      string clientID,
      string userid,
      string password,
      string key,
      string companyID,
      string branchID,
      string loID,
      string lpID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetTPOInformation", new object[8]
      {
        (object) clientID,
        (object) userid,
        (object) password,
        (object) key,
        (object) companyID,
        (object) branchID,
        (object) loID,
        (object) lpID
      }, callback, asyncState);
    }

    public string EndGetTPOInformation(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetTPOInformationAsync(
      string clientID,
      string userid,
      string password,
      string key,
      string companyID,
      string branchID,
      string loID,
      string lpID)
    {
      this.GetTPOInformationAsync(clientID, userid, password, key, companyID, branchID, loID, lpID, (object) null);
    }

    public void GetTPOInformationAsync(
      string clientID,
      string userid,
      string password,
      string key,
      string companyID,
      string branchID,
      string loID,
      string lpID,
      object userState)
    {
      if (this.GetTPOInformationOperationCompleted == null)
        this.GetTPOInformationOperationCompleted = new SendOrPostCallback(this.OnGetTPOInformationOperationCompleted);
      this.InvokeAsync("GetTPOInformation", new object[8]
      {
        (object) clientID,
        (object) userid,
        (object) password,
        (object) key,
        (object) companyID,
        (object) branchID,
        (object) loID,
        (object) lpID
      }, this.GetTPOInformationOperationCompleted, userState);
    }

    private void OnGetTPOInformationOperationCompleted(object arg)
    {
      if (this.GetTPOInformationCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetTPOInformationCompleted((object) this, new GetTPOInformationCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/GetTPOInformationMenus", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetTPOInformationMenus(
      string clientID,
      string userid,
      string password,
      string key)
    {
      return (string) this.Invoke(nameof (GetTPOInformationMenus), new object[4]
      {
        (object) clientID,
        (object) userid,
        (object) password,
        (object) key
      })[0];
    }

    public IAsyncResult BeginGetTPOInformationMenus(
      string clientID,
      string userid,
      string password,
      string key,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetTPOInformationMenus", new object[4]
      {
        (object) clientID,
        (object) userid,
        (object) password,
        (object) key
      }, callback, asyncState);
    }

    public string EndGetTPOInformationMenus(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetTPOInformationMenusAsync(
      string clientID,
      string userid,
      string password,
      string key)
    {
      this.GetTPOInformationMenusAsync(clientID, userid, password, key, (object) null);
    }

    public void GetTPOInformationMenusAsync(
      string clientID,
      string userid,
      string password,
      string key,
      object userState)
    {
      if (this.GetTPOInformationMenusOperationCompleted == null)
        this.GetTPOInformationMenusOperationCompleted = new SendOrPostCallback(this.OnGetTPOInformationMenusOperationCompleted);
      this.InvokeAsync("GetTPOInformationMenus", new object[4]
      {
        (object) clientID,
        (object) userid,
        (object) password,
        (object) key
      }, this.GetTPOInformationMenusOperationCompleted, userState);
    }

    private void OnGetTPOInformationMenusOperationCompleted(object arg)
    {
      if (this.GetTPOInformationMenusCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetTPOInformationMenusCompleted((object) this, new GetTPOInformationMenusCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/SendLoanLockedEmail", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool SendLoanLockedEmailToLOLP(string guid, string passCode, string emsiteID)
    {
      return (bool) this.Invoke(nameof (SendLoanLockedEmailToLOLP), new object[3]
      {
        (object) guid,
        (object) passCode,
        (object) emsiteID
      })[0];
    }

    public IAsyncResult BeginSendLoanLockedEmailToLOLP(
      string guid,
      string passCode,
      string emsiteID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SendLoanLockedEmailToLOLP", new object[3]
      {
        (object) guid,
        (object) passCode,
        (object) emsiteID
      }, callback, asyncState);
    }

    public bool EndSendLoanLockedEmailToLOLP(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void SendLoanLockedEmailToLOLPAsync(string guid, string passCode, string emsiteID)
    {
      this.SendLoanLockedEmailToLOLPAsync(guid, passCode, emsiteID, (object) null);
    }

    public void SendLoanLockedEmailToLOLPAsync(
      string guid,
      string passCode,
      string emsiteID,
      object userState)
    {
      if (this.SendLoanLockedEmailToLOLPOperationCompleted == null)
        this.SendLoanLockedEmailToLOLPOperationCompleted = new SendOrPostCallback(this.OnSendLoanLockedEmailToLOLPOperationCompleted);
      this.InvokeAsync("SendLoanLockedEmailToLOLP", new object[3]
      {
        (object) guid,
        (object) passCode,
        (object) emsiteID
      }, this.SendLoanLockedEmailToLOLPOperationCompleted, userState);
    }

    private void OnSendLoanLockedEmailToLOLPOperationCompleted(object arg)
    {
      if (this.SendLoanLockedEmailToLOLPCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.SendLoanLockedEmailToLOLPCompleted((object) this, new SendLoanLockedEmailToLOLPCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/GetOriginatorRegions", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public XmlNode GetOriginatorRegions(string passcode, string emsiteID, string clientID)
    {
      return (XmlNode) this.Invoke(nameof (GetOriginatorRegions), new object[3]
      {
        (object) passcode,
        (object) emsiteID,
        (object) clientID
      })[0];
    }

    public IAsyncResult BeginGetOriginatorRegions(
      string passcode,
      string emsiteID,
      string clientID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetOriginatorRegions", new object[3]
      {
        (object) passcode,
        (object) emsiteID,
        (object) clientID
      }, callback, asyncState);
    }

    public XmlNode EndGetOriginatorRegions(IAsyncResult asyncResult)
    {
      return (XmlNode) this.EndInvoke(asyncResult)[0];
    }

    public void GetOriginatorRegionsAsync(string passcode, string emsiteID, string clientID)
    {
      this.GetOriginatorRegionsAsync(passcode, emsiteID, clientID, (object) null);
    }

    public void GetOriginatorRegionsAsync(
      string passcode,
      string emsiteID,
      string clientID,
      object userState)
    {
      if (this.GetOriginatorRegionsOperationCompleted == null)
        this.GetOriginatorRegionsOperationCompleted = new SendOrPostCallback(this.OnGetOriginatorRegionsOperationCompleted);
      this.InvokeAsync("GetOriginatorRegions", new object[3]
      {
        (object) passcode,
        (object) emsiteID,
        (object) clientID
      }, this.GetOriginatorRegionsOperationCompleted, userState);
    }

    private void OnGetOriginatorRegionsOperationCompleted(object arg)
    {
      if (this.GetOriginatorRegionsCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetOriginatorRegionsCompleted((object) this, new GetOriginatorRegionsCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.elliemae.com/UpdateMirrorTPOSiteStatus", RequestNamespace = "http://www.elliemae.com/", ResponseNamespace = "http://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public bool UpdateMirrorTPOSiteStatus(
      string accountingId,
      string accountingType,
      DateTime ptd,
      int status,
      string SKU)
    {
      return (bool) this.Invoke(nameof (UpdateMirrorTPOSiteStatus), new object[5]
      {
        (object) accountingId,
        (object) accountingType,
        (object) ptd,
        (object) status,
        (object) SKU
      })[0];
    }

    public IAsyncResult BeginUpdateMirrorTPOSiteStatus(
      string accountingId,
      string accountingType,
      DateTime ptd,
      int status,
      string SKU,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UpdateMirrorTPOSiteStatus", new object[5]
      {
        (object) accountingId,
        (object) accountingType,
        (object) ptd,
        (object) status,
        (object) SKU
      }, callback, asyncState);
    }

    public bool EndUpdateMirrorTPOSiteStatus(IAsyncResult asyncResult)
    {
      return (bool) this.EndInvoke(asyncResult)[0];
    }

    public void UpdateMirrorTPOSiteStatusAsync(
      string accountingId,
      string accountingType,
      DateTime ptd,
      int status,
      string SKU)
    {
      this.UpdateMirrorTPOSiteStatusAsync(accountingId, accountingType, ptd, status, SKU, (object) null);
    }

    public void UpdateMirrorTPOSiteStatusAsync(
      string accountingId,
      string accountingType,
      DateTime ptd,
      int status,
      string SKU,
      object userState)
    {
      if (this.UpdateMirrorTPOSiteStatusOperationCompleted == null)
        this.UpdateMirrorTPOSiteStatusOperationCompleted = new SendOrPostCallback(this.OnUpdateMirrorTPOSiteStatusOperationCompleted);
      this.InvokeAsync("UpdateMirrorTPOSiteStatus", new object[5]
      {
        (object) accountingId,
        (object) accountingType,
        (object) ptd,
        (object) status,
        (object) SKU
      }, this.UpdateMirrorTPOSiteStatusOperationCompleted, userState);
    }

    private void OnUpdateMirrorTPOSiteStatusOperationCompleted(object arg)
    {
      if (this.UpdateMirrorTPOSiteStatusCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.UpdateMirrorTPOSiteStatusCompleted((object) this, new UpdateMirrorTPOSiteStatusCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("https://www.elliemae.com/ClearTPONotifications", RequestNamespace = "https://www.elliemae.com/", ResponseNamespace = "https://www.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void ClearTPONotifications(string message)
    {
      this.Invoke(nameof (ClearTPONotifications), new object[1]
      {
        (object) message
      });
    }

    public IAsyncResult BeginClearTPONotifications(
      string message,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("ClearTPONotifications", new object[1]
      {
        (object) message
      }, callback, asyncState);
    }

    public void EndClearTPONotifications(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void ClearTPONotificationsAsync(string message)
    {
      this.ClearTPONotificationsAsync(message, (object) null);
    }

    public void ClearTPONotificationsAsync(string message, object userState)
    {
      if (this.ClearTPONotificationsOperationCompleted == null)
        this.ClearTPONotificationsOperationCompleted = new SendOrPostCallback(this.OnClearTPONotificationsOperationCompleted);
      this.InvokeAsync("ClearTPONotifications", new object[1]
      {
        (object) message
      }, this.ClearTPONotificationsOperationCompleted, userState);
    }

    private void OnClearTPONotificationsOperationCompleted(object arg)
    {
      if (this.ClearTPONotificationsCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.ClearTPONotificationsCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
