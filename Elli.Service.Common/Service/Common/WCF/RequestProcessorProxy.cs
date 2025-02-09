// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.WCF.RequestProcessorProxy
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace Elli.Service.Common.WCF
{
  public class RequestProcessorProxy : 
    ClientBase<IWcfRequestProcessor>,
    IRequestProcessor,
    IDisposable
  {
    public RequestProcessorProxy()
    {
    }

    public RequestProcessorProxy(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public RequestProcessorProxy(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public RequestProcessorProxy(string endpointConfigurationName, EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public RequestProcessorProxy(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public RequestProcessorProxy(InstanceContext callbackInstance)
      : base(callbackInstance)
    {
    }

    public RequestProcessorProxy(InstanceContext callbackInstance, string endpointConfigurationName)
      : base(callbackInstance, endpointConfigurationName)
    {
    }

    public RequestProcessorProxy(
      InstanceContext callbackInstance,
      string endpointConfigurationName,
      string remoteAddress)
      : base(callbackInstance, endpointConfigurationName, remoteAddress)
    {
    }

    public RequestProcessorProxy(
      InstanceContext callbackInstance,
      string endpointConfigurationName,
      EndpointAddress remoteAddress)
      : base(callbackInstance, endpointConfigurationName, remoteAddress)
    {
    }

    public RequestProcessorProxy(
      InstanceContext callbackInstance,
      Binding binding,
      EndpointAddress remoteAddress)
      : base(callbackInstance, binding, remoteAddress)
    {
    }

    public Response[] Process(params Request[] requests) => this.Channel.Process(requests);

    public void ProcessOneWayRequests(params OneWayRequest[] requests)
    {
      this.Channel.ProcessOneWayRequests(requests);
    }

    public void Dispose()
    {
      try
      {
        this.Close();
      }
      catch (Exception ex)
      {
        this.Abort();
      }
    }
  }
}
