// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ServiceModel.ProtoOperationBehavior
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.Xml;

#nullable disable
namespace ProtoBuf.ServiceModel
{
  /// <summary>
  /// Describes a WCF operation behaviour that can perform protobuf serialization
  /// </summary>
  public sealed class ProtoOperationBehavior : DataContractSerializerOperationBehavior
  {
    private TypeModel model;

    /// <summary>Create a new ProtoOperationBehavior instance</summary>
    public ProtoOperationBehavior(OperationDescription operation)
      : base(operation)
    {
      this.model = (TypeModel) RuntimeTypeModel.Default;
    }

    /// <summary>
    /// The type-model that should be used with this behaviour
    /// </summary>
    public TypeModel Model
    {
      get => this.model;
      set => this.model = value ?? throw new ArgumentNullException(nameof (value));
    }

    /// <summary>
    /// Creates a protobuf serializer if possible (falling back to the default WCF serializer)
    /// </summary>
    public override XmlObjectSerializer CreateSerializer(
      Type type,
      XmlDictionaryString name,
      XmlDictionaryString ns,
      IList<Type> knownTypes)
    {
      if (this.model == null)
        throw new InvalidOperationException("No Model instance has been assigned to the ProtoOperationBehavior");
      return (XmlObjectSerializer) XmlProtoSerializer.TryCreate(this.model, type) ?? base.CreateSerializer(type, name, ns, knownTypes);
    }
  }
}
