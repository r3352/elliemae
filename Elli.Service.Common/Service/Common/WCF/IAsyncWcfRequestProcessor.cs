// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.WCF.IAsyncWcfRequestProcessor
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;
using System.ComponentModel;
using System.ServiceModel;

#nullable disable
namespace Elli.Service.Common.WCF
{
  [ServiceContract(Name = "IWcfRequestProcessor", ConfigurationName = "Elli.Service.Common.WCF.IWcfRequestProcessor")]
  public interface IAsyncWcfRequestProcessor : IDisposable
  {
    [OperationContract(AsyncPattern = true, Name = "ProcessRequests")]
    [ServiceKnownType("GetKnownTypes", typeof (KnownTypeProvider))]
    IAsyncResult BeginProcessRequests(
      Request[] requests,
      AsyncCallback callback,
      object asyncState);

    Response[] EndProcessRequests(IAsyncResult result);

    void ProcessRequestsAsync(
      Request[] requests,
      Action<ProcessRequestsAsyncCompletedArgs> processCompleted);

    [OperationContract(AsyncPattern = true, Name = "ProcessOneWayRequests", IsOneWay = true)]
    [ServiceKnownType("GetKnownTypes", typeof (KnownTypeProvider))]
    IAsyncResult BeginProcessOneWayRequests(
      OneWayRequest[] requests,
      AsyncCallback callback,
      object asyncState);

    void EndProcessOneWayRequests(IAsyncResult result);

    void ProcessOneWayRequestsAsync(
      OneWayRequest[] requests,
      Action<AsyncCompletedEventArgs> processCompleted);
  }
}
