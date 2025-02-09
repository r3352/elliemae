// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.IProtoTypeSerializer
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Compiler;
using ProtoBuf.Meta;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal interface IProtoTypeSerializer : IProtoSerializer
  {
    bool HasCallbacks(TypeModel.CallbackType callbackType);

    bool CanCreateInstance();

    object CreateInstance(ProtoReader source);

    void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context);

    void EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType);

    void EmitCreateInstance(CompilerContext ctx);
  }
}
