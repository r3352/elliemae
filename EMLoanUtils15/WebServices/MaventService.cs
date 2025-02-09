// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.MaventService
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "MaventServiceSoap", Namespace = "http://epass.elliemaeservices.com/mavent/")]
  public class MaventService : SoapHttpClientProtocol
  {
    public MaventService(string maventServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(maventServiceUrl) || !Uri.IsWellFormedUriString(maventServiceUrl, UriKind.Absolute))
        this.Url = "https://epass.elliemaeservices.com/Mavent/mavent.asmx";
      else
        this.Url = maventServiceUrl;
      this.Timeout = 45000;
    }

    public MaventService()
      : this(false)
    {
    }

    public MaventService(bool useHAUrl)
    {
      if (!useHAUrl)
        this.Url = "https://epass.elliemaeservices.com/Mavent/mavent.asmx";
      else
        this.Url = "https://modules.elliemaeservices.com/Mavent/mavent.asmx";
      this.Timeout = 45000;
    }

    [SoapDocumentMethod("http://epass.elliemaeservices.com/mavent/SetupMaventAccount", RequestNamespace = "http://epass.elliemaeservices.com/mavent/", ResponseNamespace = "http://epass.elliemaeservices.com/mavent/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void SetupMaventAccount(string xmlRequest)
    {
      this.Invoke(nameof (SetupMaventAccount), new object[1]
      {
        (object) xmlRequest
      });
    }

    public IAsyncResult BeginSetupMaventAccount(
      string xmlRequest,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SetupMaventAccount", new object[1]
      {
        (object) xmlRequest
      }, callback, asyncState);
    }

    public void EndSetupMaventAccount(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    [SoapDocumentMethod("http://epass.elliemaeservices.com/mavent/GetOrderDate", RequestNamespace = "http://epass.elliemaeservices.com/mavent/", ResponseNamespace = "http://epass.elliemaeservices.com/mavent/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetOrderDate() => (string) this.Invoke(nameof (GetOrderDate), new object[0])[0];

    public IAsyncResult BeginGetOrderDate(AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("GetOrderDate", new object[0], callback, asyncState);
    }

    public string EndGetOrderDate(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://epass.elliemaeservices.com/mavent/GetARMIndexValue", RequestNamespace = "http://epass.elliemaeservices.com/mavent/", ResponseNamespace = "http://epass.elliemaeservices.com/mavent/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetARMIndexValue(string armIndexCode, string armIndexDate)
    {
      return (string) this.Invoke(nameof (GetARMIndexValue), new object[2]
      {
        (object) armIndexCode,
        (object) armIndexDate
      })[0];
    }

    public IAsyncResult BeginGetARMIndexValue(
      string armIndexCode,
      string armIndexDate,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetARMIndexValue", new object[2]
      {
        (object) armIndexCode,
        (object) armIndexDate
      }, callback, asyncState);
    }

    public string EndGetARMIndexValue(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://epass.elliemaeservices.com/mavent/GetResidualIncomeValue", RequestNamespace = "http://epass.elliemaeservices.com/mavent/", ResponseNamespace = "http://epass.elliemaeservices.com/mavent/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetResidualIncomeValue(
      string propertyState,
      string householdMembers,
      string loanAmount,
      string thresholdDate)
    {
      return (string) this.Invoke(nameof (GetResidualIncomeValue), new object[4]
      {
        (object) propertyState,
        (object) householdMembers,
        (object) loanAmount,
        (object) thresholdDate
      })[0];
    }

    public IAsyncResult BeginGetResidualIncomeValue(
      string propertyState,
      string householdMembers,
      string loanAmount,
      string thresholdDate,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetResidualIncomeValue", new object[4]
      {
        (object) propertyState,
        (object) householdMembers,
        (object) loanAmount,
        (object) thresholdDate
      }, callback, asyncState);
    }

    public string EndGetResidualIncomeValue(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://epass.elliemaeservices.com/mavent/SendMaventRequest", RequestNamespace = "http://epass.elliemaeservices.com/mavent/", ResponseNamespace = "http://epass.elliemaeservices.com/mavent/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string SendMaventRequest(
      string xmlRequest,
      string customerClientID,
      string customerCompanyName,
      string transactionID,
      string tqlReportType,
      string loanGuid,
      string eFolderReportFilename)
    {
      return (string) this.Invoke(nameof (SendMaventRequest), new object[7]
      {
        (object) xmlRequest,
        (object) customerClientID,
        (object) customerCompanyName,
        (object) transactionID,
        (object) tqlReportType,
        (object) loanGuid,
        (object) eFolderReportFilename
      })[0];
    }

    public IAsyncResult BeginSendMaventRequest(
      string xmlRequest,
      string customerClientID,
      string customerCompanyName,
      string transactionID,
      string tqlReportType,
      string loanGuid,
      string eFolderReportFilename,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SendMaventRequest", new object[7]
      {
        (object) xmlRequest,
        (object) customerClientID,
        (object) customerCompanyName,
        (object) transactionID,
        (object) tqlReportType,
        (object) loanGuid,
        (object) eFolderReportFilename
      }, callback, asyncState);
    }

    public string EndSendMaventRequest(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://epass.elliemaeservices.com/mavent/GetServiceStatus", RequestNamespace = "http://epass.elliemaeservices.com/mavent/", ResponseNamespace = "http://epass.elliemaeservices.com/mavent/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetServiceStatus(string clientID)
    {
      return (string) this.Invoke(nameof (GetServiceStatus), new object[1]
      {
        (object) clientID
      })[0];
    }

    public IAsyncResult BeginGetServiceStatus(
      string clientID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetServiceStatus", new object[1]
      {
        (object) clientID
      }, callback, asyncState);
    }

    public string EndGetServiceStatus(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://epass.elliemaeservices.com/mavent/CreateOrder", RequestNamespace = "http://epass.elliemaeservices.com/mavent/", ResponseNamespace = "http://epass.elliemaeservices.com/mavent/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void CreateOrder(string clientID, string contactEmail, string contactName)
    {
      this.Invoke(nameof (CreateOrder), new object[3]
      {
        (object) clientID,
        (object) contactEmail,
        (object) contactName
      });
    }

    public IAsyncResult BeginCreateOrder(
      string clientID,
      string contactEmail,
      string contactName,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CreateOrder", new object[3]
      {
        (object) clientID,
        (object) contactEmail,
        (object) contactName
      }, callback, asyncState);
    }

    public void EndCreateOrder(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    [SoapDocumentMethod("http://epass.elliemaeservices.com/mavent/SendNewFeesEmail", RequestNamespace = "http://epass.elliemaeservices.com/mavent/", ResponseNamespace = "http://epass.elliemaeservices.com/mavent/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void SendNewFeesEmail(
      string emailAddress,
      string[] feeList,
      string processorName,
      string borrowerName,
      string loanNumber,
      bool useEncompassFeeSetup)
    {
      this.Invoke(nameof (SendNewFeesEmail), new object[6]
      {
        (object) emailAddress,
        (object) feeList,
        (object) processorName,
        (object) borrowerName,
        (object) loanNumber,
        (object) useEncompassFeeSetup
      });
    }

    public IAsyncResult BeginSendNewFeesEmail(
      string emailAddress,
      string[] feeList,
      string processorName,
      string borrowerName,
      string loanNumber,
      bool useEncompassFeeSetup,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SendNewFeesEmail", new object[6]
      {
        (object) emailAddress,
        (object) feeList,
        (object) processorName,
        (object) borrowerName,
        (object) loanNumber,
        (object) useEncompassFeeSetup
      }, callback, asyncState);
    }

    public void EndSendNewFeesEmail(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);
  }
}
