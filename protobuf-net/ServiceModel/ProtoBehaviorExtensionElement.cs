// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ServiceModel.ProtoBehaviorExtension
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.ServiceModel.Configuration;

#nullable disable
namespace ProtoBuf.ServiceModel
{
  /// <summary>
  /// Configuration element to swap out DatatContractSerilaizer with the XmlProtoSerializer for a given endpoint.
  /// </summary>
  /// <seealso cref="T:ProtoBuf.ServiceModel.ProtoEndpointBehavior" />
  public class ProtoBehaviorExtension : BehaviorExtensionElement
  {
    /// <summary>Gets the type of behavior.</summary>
    public override Type BehaviorType => typeof (ProtoEndpointBehavior);

    /// <summary>
    /// Creates a behavior extension based on the current configuration settings.
    /// </summary>
    /// <returns>The behavior extension.</returns>
    protected override object CreateBehavior() => (object) new ProtoEndpointBehavior();
  }
}
