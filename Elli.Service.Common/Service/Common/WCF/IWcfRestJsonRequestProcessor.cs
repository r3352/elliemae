// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.WCF.IWcfRestJsonRequestProcessor
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System.ServiceModel;
using System.ServiceModel.Web;

#nullable disable
namespace Elli.Service.Common.WCF
{
  [ServiceContract]
  public interface IWcfRestJsonRequestProcessor
  {
    [OperationContract(Name = "ProcessJsonRequestsGet")]
    [ServiceKnownType("GetKnownTypes", typeof (KnownTypeProvider))]
    [TransactionFlow(TransactionFlowOption.Allowed)]
    [WebGet(UriTemplate = "/", BodyStyle = WebMessageBodyStyle.WrappedResponse, ResponseFormat = WebMessageFormat.Json)]
    Response[] Process();

    [OperationContract(Name = "ProcessJsonRequestsPost")]
    [ServiceKnownType("GetKnownTypes", typeof (KnownTypeProvider))]
    [TransactionFlow(TransactionFlowOption.Allowed)]
    [WebInvoke(UriTemplate = "/post", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
    Response[] Process(Request[] requests);

    [OperationContract(Name = "ProcessOneWayJsonRequestsPost", IsOneWay = true)]
    [ServiceKnownType("GetKnownTypes", typeof (KnownTypeProvider))]
    [WebInvoke(UriTemplate = "/post/oneway", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
    void ProcessOneWayRequests(OneWayRequest[] requests);
  }
}
