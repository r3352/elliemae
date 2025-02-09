// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.CMInterface
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using Microsoft.Web.Services2;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

#nullable disable
namespace ClosingMarket.WebServices
{
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "CMInterfaceSoap", Namespace = "http://www.closingmarket.com")]
  [SoapInclude(typeof (SerializableObject))]
  public class CMInterface : WebServicesClientProtocol
  {
    public event ChunkHandler ChunkSent;

    public event ChunkHandler ChunkReceived;

    public event EventHandler RequestComplete;

    public event EventHandler ResponseComplete;

    protected override WebRequest GetWebRequest(Uri uri)
    {
      SoapWebRequest webRequest = (SoapWebRequest) base.GetWebRequest(uri);
      FieldInfo field = webRequest.GetType().GetField("_request", BindingFlags.Instance | BindingFlags.NonPublic);
      CustomWebRequest customWebRequest = new CustomWebRequest(uri);
      customWebRequest.ChunkSent += new ChunkHandler(this.webRequest_ChunkSent);
      customWebRequest.ChunkReceived += new ChunkHandler(this.webRequest_ChunkReceived);
      customWebRequest.RequestComplete += new EventHandler(this.webRequest_RequestComplete);
      customWebRequest.ResponseComplete += new EventHandler(this.webRequest_ResponseComplete);
      field.SetValue((object) webRequest, (object) customWebRequest);
      return (WebRequest) webRequest;
    }

    private void webRequest_ChunkSent(object sender, ChunkHandlerEventArgs e)
    {
      if (this.ChunkSent == null)
        return;
      this.ChunkSent((object) this, e);
    }

    private void webRequest_ChunkReceived(object sender, ChunkHandlerEventArgs e)
    {
      if (this.ChunkReceived == null)
        return;
      this.ChunkReceived((object) this, e);
    }

    private void webRequest_RequestComplete(object sender, EventArgs e)
    {
      if (this.RequestComplete == null)
        return;
      this.RequestComplete((object) this, e);
    }

    private void webRequest_ResponseComplete(object sender, EventArgs e)
    {
      if (this.ResponseComplete == null)
        return;
      this.ResponseComplete((object) this, e);
    }

    public CMInterface()
    {
      this.Url = "https://ws1.closingmarket.com/ClosingMarketService/CMInterface.asmx";
    }

    [SoapRpcMethod("http://www.closingmarket.com/Login", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public string Login(string userName, string password)
    {
      return (string) this.Invoke(nameof (Login), new object[2]
      {
        (object) userName,
        (object) password
      })[0];
    }

    public IAsyncResult BeginLogin(
      string userName,
      string password,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("Login", new object[2]
      {
        (object) userName,
        (object) password
      }, callback, asyncState);
    }

    public string EndLogin(IAsyncResult asyncResult) => (string) this.EndInvoke(asyncResult)[0];

    [SoapRpcMethod("http://www.closingmarket.com/GetClosingMarketConstants", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public ClosingMarketConstants GetClosingMarketConstants()
    {
      return (ClosingMarketConstants) this.Invoke(nameof (GetClosingMarketConstants), new object[0])[0];
    }

    public IAsyncResult BeginGetClosingMarketConstants(AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("GetClosingMarketConstants", new object[0], callback, asyncState);
    }

    public ClosingMarketConstants EndGetClosingMarketConstants(IAsyncResult asyncResult)
    {
      return (ClosingMarketConstants) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetTradingPartnerServices", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public EnterpriseDetailData[] GetTradingPartnerServices(string token, int enterpriseDetailFlag)
    {
      return (EnterpriseDetailData[]) this.Invoke(nameof (GetTradingPartnerServices), new object[2]
      {
        (object) token,
        (object) enterpriseDetailFlag
      })[0];
    }

    public IAsyncResult BeginGetTradingPartnerServices(
      string token,
      int enterpriseDetailFlag,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetTradingPartnerServices", new object[2]
      {
        (object) token,
        (object) enterpriseDetailFlag
      }, callback, asyncState);
    }

    public EnterpriseDetailData[] EndGetTradingPartnerServices(IAsyncResult asyncResult)
    {
      return (EnterpriseDetailData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetTradingPartnerServicesByEnterpriseType", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public EnterpriseDetailData[] GetTradingPartnerServicesByEnterpriseType(
      string token,
      int enterpriseDetailFlag,
      EnterpriseType enterpriseType,
      bool includeEnterprisesWithNoOrders)
    {
      return (EnterpriseDetailData[]) this.Invoke(nameof (GetTradingPartnerServicesByEnterpriseType), new object[4]
      {
        (object) token,
        (object) enterpriseDetailFlag,
        (object) enterpriseType,
        (object) includeEnterprisesWithNoOrders
      })[0];
    }

    public IAsyncResult BeginGetTradingPartnerServicesByEnterpriseType(
      string token,
      int enterpriseDetailFlag,
      EnterpriseType enterpriseType,
      bool includeEnterprisesWithNoOrders,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetTradingPartnerServicesByEnterpriseType", new object[4]
      {
        (object) token,
        (object) enterpriseDetailFlag,
        (object) enterpriseType,
        (object) includeEnterprisesWithNoOrders
      }, callback, asyncState);
    }

    public EnterpriseDetailData[] EndGetTradingPartnerServicesByEnterpriseType(
      IAsyncResult asyncResult)
    {
      return (EnterpriseDetailData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetAvailableServices", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public EnterpriseDetailData[] GetAvailableServices(string token, int enterpriseDetailFlag)
    {
      return (EnterpriseDetailData[]) this.Invoke(nameof (GetAvailableServices), new object[2]
      {
        (object) token,
        (object) enterpriseDetailFlag
      })[0];
    }

    public IAsyncResult BeginGetAvailableServices(
      string token,
      int enterpriseDetailFlag,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetAvailableServices", new object[2]
      {
        (object) token,
        (object) enterpriseDetailFlag
      }, callback, asyncState);
    }

    public EnterpriseDetailData[] EndGetAvailableServices(IAsyncResult asyncResult)
    {
      return (EnterpriseDetailData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetAvailableServicesByEnterpriseType", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public EnterpriseDetailData[] GetAvailableServicesByEnterpriseType(
      string token,
      int enterpriseDetailFlag,
      EnterpriseType enterpriseType)
    {
      return (EnterpriseDetailData[]) this.Invoke(nameof (GetAvailableServicesByEnterpriseType), new object[3]
      {
        (object) token,
        (object) enterpriseDetailFlag,
        (object) enterpriseType
      })[0];
    }

    public IAsyncResult BeginGetAvailableServicesByEnterpriseType(
      string token,
      int enterpriseDetailFlag,
      EnterpriseType enterpriseType,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetAvailableServicesByEnterpriseType", new object[3]
      {
        (object) token,
        (object) enterpriseDetailFlag,
        (object) enterpriseType
      }, callback, asyncState);
    }

    public EnterpriseDetailData[] EndGetAvailableServicesByEnterpriseType(IAsyncResult asyncResult)
    {
      return (EnterpriseDetailData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetEnterpriseServiceUIDefinitionData", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public EnterpriseServiceUIDefinitionChildData GetEnterpriseServiceUIDefinitionData(
      string token,
      int enterpriseServiceID)
    {
      return (EnterpriseServiceUIDefinitionChildData) this.Invoke(nameof (GetEnterpriseServiceUIDefinitionData), new object[2]
      {
        (object) token,
        (object) enterpriseServiceID
      })[0];
    }

    public IAsyncResult BeginGetEnterpriseServiceUIDefinitionData(
      string token,
      int enterpriseServiceID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetEnterpriseServiceUIDefinitionData", new object[2]
      {
        (object) token,
        (object) enterpriseServiceID
      }, callback, asyncState);
    }

    public EnterpriseServiceUIDefinitionChildData EndGetEnterpriseServiceUIDefinitionData(
      IAsyncResult asyncResult)
    {
      return (EnterpriseServiceUIDefinitionChildData) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/RequestTradingPartnerRelationship", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public void RequestTradingPartnerRelationship(
      string token,
      int originatorEnterpriseServiceID,
      int providerEnterpriseID,
      int enterpriseServiceID)
    {
      this.Invoke(nameof (RequestTradingPartnerRelationship), new object[4]
      {
        (object) token,
        (object) originatorEnterpriseServiceID,
        (object) providerEnterpriseID,
        (object) enterpriseServiceID
      });
    }

    public IAsyncResult BeginRequestTradingPartnerRelationship(
      string token,
      int originatorEnterpriseServiceID,
      int providerEnterpriseID,
      int enterpriseServiceID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("RequestTradingPartnerRelationship", new object[4]
      {
        (object) token,
        (object) originatorEnterpriseServiceID,
        (object) providerEnterpriseID,
        (object) enterpriseServiceID
      }, callback, asyncState);
    }

    public void EndRequestTradingPartnerRelationship(IAsyncResult asyncResult)
    {
      this.EndInvoke(asyncResult);
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetEnterpriseServiceProductList", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public ProductData[] GetEnterpriseServiceProductList(
      string token,
      int enterpriseServiceID,
      ProductCategory category,
      string state,
      int county)
    {
      return (ProductData[]) this.Invoke(nameof (GetEnterpriseServiceProductList), new object[5]
      {
        (object) token,
        (object) enterpriseServiceID,
        (object) category,
        (object) state,
        (object) county
      })[0];
    }

    public IAsyncResult BeginGetEnterpriseServiceProductList(
      string token,
      int enterpriseServiceID,
      ProductCategory category,
      string state,
      int county,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetEnterpriseServiceProductList", new object[5]
      {
        (object) token,
        (object) enterpriseServiceID,
        (object) category,
        (object) state,
        (object) county
      }, callback, asyncState);
    }

    public ProductData[] EndGetEnterpriseServiceProductList(IAsyncResult asyncResult)
    {
      return (ProductData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetClosingMarketStates", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public StateData[] GetClosingMarketStates(string token)
    {
      return (StateData[]) this.Invoke(nameof (GetClosingMarketStates), new object[1]
      {
        (object) token
      })[0];
    }

    public IAsyncResult BeginGetClosingMarketStates(
      string token,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetClosingMarketStates", new object[1]
      {
        (object) token
      }, callback, asyncState);
    }

    public StateData[] EndGetClosingMarketStates(IAsyncResult asyncResult)
    {
      return (StateData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/SubmitOrder", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public SubmitOrderResult SubmitOrder(
      string token,
      int enterpriseServiceID,
      OrderData orderData)
    {
      return (SubmitOrderResult) this.Invoke(nameof (SubmitOrder), new object[3]
      {
        (object) token,
        (object) enterpriseServiceID,
        (object) orderData
      })[0];
    }

    public IAsyncResult BeginSubmitOrder(
      string token,
      int enterpriseServiceID,
      OrderData orderData,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SubmitOrder", new object[3]
      {
        (object) token,
        (object) enterpriseServiceID,
        (object) orderData
      }, callback, asyncState);
    }

    public SubmitOrderResult EndSubmitOrder(IAsyncResult asyncResult)
    {
      return (SubmitOrderResult) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/SubmitGenericOrder", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public SubmitOrderResult SubmitGenericOrder(
      string token,
      int enterpriseServiceID,
      OrderDataFormat dataFormat,
      string orderData)
    {
      return (SubmitOrderResult) this.Invoke(nameof (SubmitGenericOrder), new object[4]
      {
        (object) token,
        (object) enterpriseServiceID,
        (object) dataFormat,
        (object) orderData
      })[0];
    }

    public IAsyncResult BeginSubmitGenericOrder(
      string token,
      int enterpriseServiceID,
      OrderDataFormat dataFormat,
      string orderData,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SubmitGenericOrder", new object[4]
      {
        (object) token,
        (object) enterpriseServiceID,
        (object) dataFormat,
        (object) orderData
      }, callback, asyncState);
    }

    public SubmitOrderResult EndSubmitGenericOrder(IAsyncResult asyncResult)
    {
      return (SubmitOrderResult) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/ConfirmOrder", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public ClosingMarketResult ConfirmOrder(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      string providerOrderID)
    {
      return (ClosingMarketResult) this.Invoke(nameof (ConfirmOrder), new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) providerOrderID
      })[0];
    }

    public IAsyncResult BeginConfirmOrder(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      string providerOrderID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("ConfirmOrder", new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) providerOrderID
      }, callback, asyncState);
    }

    public ClosingMarketResult EndConfirmOrder(IAsyncResult asyncResult)
    {
      return (ClosingMarketResult) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/UpdateOrder", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public ClosingMarketResult UpdateOrder(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      OrderData orderData)
    {
      return (ClosingMarketResult) this.Invoke(nameof (UpdateOrder), new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) orderData
      })[0];
    }

    public IAsyncResult BeginUpdateOrder(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      OrderData orderData,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UpdateOrder", new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) orderData
      }, callback, asyncState);
    }

    public ClosingMarketResult EndUpdateOrder(IAsyncResult asyncResult)
    {
      return (ClosingMarketResult) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/UpdateGenericOrder", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public ClosingMarketResult UpdateGenericOrder(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      OrderDataFormat dataFormat,
      string orderData)
    {
      return (ClosingMarketResult) this.Invoke(nameof (UpdateGenericOrder), new object[6]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) dataFormat,
        (object) orderData
      })[0];
    }

    public IAsyncResult BeginUpdateGenericOrder(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      OrderDataFormat dataFormat,
      string orderData,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UpdateGenericOrder", new object[6]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) dataFormat,
        (object) orderData
      }, callback, asyncState);
    }

    public ClosingMarketResult EndUpdateGenericOrder(IAsyncResult asyncResult)
    {
      return (ClosingMarketResult) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetOrderList", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public OrderSummaryData[] GetOrderList(
      string token,
      OrderStatus orderStatus,
      int enterpriseServiceID,
      DateTime startDate,
      DateTime endDate)
    {
      return (OrderSummaryData[]) this.Invoke(nameof (GetOrderList), new object[5]
      {
        (object) token,
        (object) orderStatus,
        (object) enterpriseServiceID,
        (object) startDate,
        (object) endDate
      })[0];
    }

    public IAsyncResult BeginGetOrderList(
      string token,
      OrderStatus orderStatus,
      int enterpriseServiceID,
      DateTime startDate,
      DateTime endDate,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetOrderList", new object[5]
      {
        (object) token,
        (object) orderStatus,
        (object) enterpriseServiceID,
        (object) startDate,
        (object) endDate
      }, callback, asyncState);
    }

    public OrderSummaryData[] EndGetOrderList(IAsyncResult asyncResult)
    {
      return (OrderSummaryData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetUnprocessedOrders", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public OrderSummaryData[] GetUnprocessedOrders(string token)
    {
      return (OrderSummaryData[]) this.Invoke(nameof (GetUnprocessedOrders), new object[1]
      {
        (object) token
      })[0];
    }

    public IAsyncResult BeginGetUnprocessedOrders(
      string token,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetUnprocessedOrders", new object[1]
      {
        (object) token
      }, callback, asyncState);
    }

    public OrderSummaryData[] EndGetUnprocessedOrders(IAsyncResult asyncResult)
    {
      return (OrderSummaryData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetOrder", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public OrderData GetOrder(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      bool includeDocumentBody)
    {
      return (OrderData) this.Invoke(nameof (GetOrder), new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) includeDocumentBody
      })[0];
    }

    public IAsyncResult BeginGetOrder(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      bool includeDocumentBody,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetOrder", new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) includeDocumentBody
      }, callback, asyncState);
    }

    public OrderData EndGetOrder(IAsyncResult asyncResult)
    {
      return (OrderData) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetOrderDocuments", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public DocumentData[] GetOrderDocuments(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      bool includeDocumentBody)
    {
      return (DocumentData[]) this.Invoke(nameof (GetOrderDocuments), new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) includeDocumentBody
      })[0];
    }

    public IAsyncResult BeginGetOrderDocuments(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      bool includeDocumentBody,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetOrderDocuments", new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) includeDocumentBody
      }, callback, asyncState);
    }

    public DocumentData[] EndGetOrderDocuments(IAsyncResult asyncResult)
    {
      return (DocumentData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetOrderNotes", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public NoteData[] GetOrderNotes(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType)
    {
      return (NoteData[]) this.Invoke(nameof (GetOrderNotes), new object[4]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType
      })[0];
    }

    public IAsyncResult BeginGetOrderNotes(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetOrderNotes", new object[4]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType
      }, callback, asyncState);
    }

    public NoteData[] EndGetOrderNotes(IAsyncResult asyncResult)
    {
      return (NoteData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetOrderHistory", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public OrderHistoryData[] GetOrderHistory(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType)
    {
      return (OrderHistoryData[]) this.Invoke(nameof (GetOrderHistory), new object[4]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType
      })[0];
    }

    public IAsyncResult BeginGetOrderHistory(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetOrderHistory", new object[4]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType
      }, callback, asyncState);
    }

    public OrderHistoryData[] EndGetOrderHistory(IAsyncResult asyncResult)
    {
      return (OrderHistoryData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetOrderTasks", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public OrderTaskData[] GetOrderTasks(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType)
    {
      return (OrderTaskData[]) this.Invoke(nameof (GetOrderTasks), new object[4]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType
      })[0];
    }

    public IAsyncResult BeginGetOrderTasks(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetOrderTasks", new object[4]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType
      }, callback, asyncState);
    }

    public OrderTaskData[] EndGetOrderTasks(IAsyncResult asyncResult)
    {
      return (OrderTaskData[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/AddOrderNote", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public ClosingMarketResult AddOrderNote(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      string note)
    {
      return (ClosingMarketResult) this.Invoke(nameof (AddOrderNote), new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) note
      })[0];
    }

    public IAsyncResult BeginAddOrderNote(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      string note,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("AddOrderNote", new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) note
      }, callback, asyncState);
    }

    public ClosingMarketResult EndAddOrderNote(IAsyncResult asyncResult)
    {
      return (ClosingMarketResult) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/UpdateOrderTaskStatus", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public ClosingMarketResult UpdateOrderTaskStatus(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      OrderTaskUpdateData taskUpdateData)
    {
      return (ClosingMarketResult) this.Invoke(nameof (UpdateOrderTaskStatus), new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) taskUpdateData
      })[0];
    }

    public IAsyncResult BeginUpdateOrderTaskStatus(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      OrderTaskUpdateData taskUpdateData,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UpdateOrderTaskStatus", new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) taskUpdateData
      }, callback, asyncState);
    }

    public ClosingMarketResult EndUpdateOrderTaskStatus(IAsyncResult asyncResult)
    {
      return (ClosingMarketResult) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/AddOrderDocument", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public ClosingMarketResult AddOrderDocument(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      DocumentData documentData)
    {
      return (ClosingMarketResult) this.Invoke(nameof (AddOrderDocument), new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) documentData
      })[0];
    }

    public IAsyncResult BeginAddOrderDocument(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      DocumentData documentData,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("AddOrderDocument", new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) documentData
      }, callback, asyncState);
    }

    public ClosingMarketResult EndAddOrderDocument(IAsyncResult asyncResult)
    {
      return (ClosingMarketResult) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/ModifyOrderDocumentDescription", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public ClosingMarketResult ModifyOrderDocumentDescription(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      int documentID,
      string newDescription)
    {
      return (ClosingMarketResult) this.Invoke(nameof (ModifyOrderDocumentDescription), new object[6]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) documentID,
        (object) newDescription
      })[0];
    }

    public IAsyncResult BeginModifyOrderDocumentDescription(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      int documentID,
      string newDescription,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("ModifyOrderDocumentDescription", new object[6]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) documentID,
        (object) newDescription
      }, callback, asyncState);
    }

    public ClosingMarketResult EndModifyOrderDocumentDescription(IAsyncResult asyncResult)
    {
      return (ClosingMarketResult) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/GetOrderDocument", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public DocumentData GetOrderDocument(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      int documentID)
    {
      return (DocumentData) this.Invoke(nameof (GetOrderDocument), new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) documentID
      })[0];
    }

    public IAsyncResult BeginGetOrderDocument(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      int documentID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetOrderDocument", new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) documentID
      }, callback, asyncState);
    }

    public DocumentData EndGetOrderDocument(IAsyncResult asyncResult)
    {
      return (DocumentData) this.EndInvoke(asyncResult)[0];
    }

    [SoapRpcMethod("http://www.closingmarket.com/DeleteOrderDocument", RequestNamespace = "http://www.closingmarket.com", ResponseNamespace = "http://www.closingmarket.com")]
    public ClosingMarketResult DeleteOrderDocument(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      int documentID)
    {
      return (ClosingMarketResult) this.Invoke(nameof (DeleteOrderDocument), new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) documentID
      })[0];
    }

    public IAsyncResult BeginDeleteOrderDocument(
      string token,
      string orderID,
      int tradingPartnerID,
      OrderIDType orderIDType,
      int documentID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("DeleteOrderDocument", new object[5]
      {
        (object) token,
        (object) orderID,
        (object) tradingPartnerID,
        (object) orderIDType,
        (object) documentID
      }, callback, asyncState);
    }

    public ClosingMarketResult EndDeleteOrderDocument(IAsyncResult asyncResult)
    {
      return (ClosingMarketResult) this.EndInvoke(asyncResult)[0];
    }
  }
}
