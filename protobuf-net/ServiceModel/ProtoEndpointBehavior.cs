// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ServiceModel.ProtoEndpointBehavior
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

#nullable disable
namespace ProtoBuf.ServiceModel
{
  /// <summary>
  /// Behavior to swap out DatatContractSerilaizer with the XmlProtoSerializer for a given endpoint.
  ///  <example>
  /// Add the following to the server and client app.config in the system.serviceModel section:
  ///  <behaviors>
  ///    <endpointBehaviors>
  ///      <behavior name="ProtoBufBehaviorConfig">
  ///        <ProtoBufSerialization />
  ///      </behavior>
  ///    </endpointBehaviors>
  ///  </behaviors>
  ///  <extensions>
  ///    <behaviorExtensions>
  ///      <add name="ProtoBufSerialization" type="ProtoBuf.ServiceModel.ProtoBehaviorExtension, protobuf-net, Version=1.0.0.255, Culture=neutral, PublicKeyToken=257b51d87d2e4d67" />
  ///    </behaviorExtensions>
  ///  </extensions>
  /// 
  /// Configure your endpoints to have a behaviorConfiguration as follows:
  /// 
  ///  <service name="TK.Framework.Samples.ServiceModel.Contract.SampleService">
  ///    <endpoint address="http://myhost:9003/SampleService" binding="basicHttpBinding" behaviorConfiguration="ProtoBufBehaviorConfig" bindingConfiguration="basicHttpBindingConfig" name="basicHttpProtoBuf" contract="ISampleServiceContract" />
  ///  </service>
  ///  <client>
  ///      <endpoint address="http://myhost:9003/SampleService" binding="basicHttpBinding" bindingConfiguration="basicHttpBindingConfig" contract="ISampleServiceContract" name="BasicHttpProtoBufEndpoint" behaviorConfiguration="ProtoBufBehaviorConfig" />
  ///   </client>
  /// </example>
  /// </summary>
  public class ProtoEndpointBehavior : IEndpointBehavior
  {
    void IEndpointBehavior.AddBindingParameters(
      ServiceEndpoint endpoint,
      BindingParameterCollection bindingParameters)
    {
    }

    void IEndpointBehavior.ApplyClientBehavior(
      ServiceEndpoint endpoint,
      ClientRuntime clientRuntime)
    {
      ProtoEndpointBehavior.ReplaceDataContractSerializerOperationBehavior(endpoint);
    }

    void IEndpointBehavior.ApplyDispatchBehavior(
      ServiceEndpoint endpoint,
      EndpointDispatcher endpointDispatcher)
    {
      ProtoEndpointBehavior.ReplaceDataContractSerializerOperationBehavior(endpoint);
    }

    void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
    {
    }

    private static void ReplaceDataContractSerializerOperationBehavior(
      ServiceEndpoint serviceEndpoint)
    {
      foreach (OperationDescription operation in (Collection<OperationDescription>) serviceEndpoint.Contract.Operations)
        ProtoEndpointBehavior.ReplaceDataContractSerializerOperationBehavior(operation);
    }

    private static void ReplaceDataContractSerializerOperationBehavior(
      OperationDescription description)
    {
      DataContractSerializerOperationBehavior operationBehavior1 = description.Behaviors.Find<DataContractSerializerOperationBehavior>();
      if (operationBehavior1 == null)
        return;
      description.Behaviors.Remove((IOperationBehavior) operationBehavior1);
      ProtoOperationBehavior operationBehavior2 = new ProtoOperationBehavior(description);
      operationBehavior2.MaxItemsInObjectGraph = operationBehavior1.MaxItemsInObjectGraph;
      description.Behaviors.Add((IOperationBehavior) operationBehavior2);
    }
  }
}
