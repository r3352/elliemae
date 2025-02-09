// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ServiceModel.ProtoBehaviorAttribute
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

#nullable disable
namespace ProtoBuf.ServiceModel
{
  /// <summary>
  /// Uses protocol buffer serialization on the specified operation; note that this
  /// must be enabled on both the client and server.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public sealed class ProtoBehaviorAttribute : Attribute, IOperationBehavior
  {
    void IOperationBehavior.AddBindingParameters(
      OperationDescription operationDescription,
      BindingParameterCollection bindingParameters)
    {
    }

    void IOperationBehavior.ApplyClientBehavior(
      OperationDescription operationDescription,
      ClientOperation clientOperation)
    {
      ((IOperationBehavior) new ProtoOperationBehavior(operationDescription)).ApplyClientBehavior(operationDescription, clientOperation);
    }

    void IOperationBehavior.ApplyDispatchBehavior(
      OperationDescription operationDescription,
      DispatchOperation dispatchOperation)
    {
      ((IOperationBehavior) new ProtoOperationBehavior(operationDescription)).ApplyDispatchBehavior(operationDescription, dispatchOperation);
    }

    void IOperationBehavior.Validate(OperationDescription operationDescription)
    {
    }
  }
}
