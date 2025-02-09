// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ServiceModel.XmlProtoSerializer
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Meta;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

#nullable disable
namespace ProtoBuf.ServiceModel
{
  /// <summary>
  /// An xml object serializer that can embed protobuf data in a base-64 hunk (looking like a byte[])
  /// </summary>
  public sealed class XmlProtoSerializer : XmlObjectSerializer
  {
    private readonly TypeModel model;
    private readonly int key;
    private readonly bool isList;
    private readonly bool isEnum;
    private readonly Type type;
    private const string PROTO_ELEMENT = "proto";

    internal XmlProtoSerializer(TypeModel model, int key, Type type, bool isList)
    {
      if (key < 0)
        throw new ArgumentOutOfRangeException(nameof (key));
      this.model = model ?? throw new ArgumentNullException(nameof (model));
      this.key = key;
      this.isList = isList;
      this.type = type ?? throw new ArgumentOutOfRangeException(nameof (type));
      this.isEnum = Helpers.IsEnum(type);
    }

    /// <summary>
    /// Attempt to create a new serializer for the given model and type
    /// </summary>
    /// <returns>A new serializer instance if the type is recognised by the model; null otherwise</returns>
    public static XmlProtoSerializer TryCreate(TypeModel model, Type type)
    {
      if (model == null)
        throw new ArgumentNullException(nameof (model));
      bool isList;
      int key = !(type == (Type) null) ? XmlProtoSerializer.GetKey(model, ref type, out isList) : throw new ArgumentNullException(nameof (type));
      return key >= 0 ? new XmlProtoSerializer(model, key, type, isList) : (XmlProtoSerializer) null;
    }

    /// <summary>Creates a new serializer for the given model and type</summary>
    public XmlProtoSerializer(TypeModel model, Type type)
    {
      if (model == null)
        throw new ArgumentNullException(nameof (model));
      this.key = !(type == (Type) null) ? XmlProtoSerializer.GetKey(model, ref type, out this.isList) : throw new ArgumentNullException(nameof (type));
      this.model = model;
      this.type = type;
      this.isEnum = Helpers.IsEnum(type);
      if (this.key < 0)
        throw new ArgumentOutOfRangeException(nameof (type), "Type not recognised by the model: " + type.FullName);
    }

    private static int GetKey(TypeModel model, ref Type type, out bool isList)
    {
      if (model != null && type != (Type) null)
      {
        int key1 = model.GetKey(ref type);
        if (key1 >= 0)
        {
          isList = false;
          return key1;
        }
        Type listItemType = TypeModel.GetListItemType(model, type);
        if (listItemType != (Type) null)
        {
          int key2 = model.GetKey(ref listItemType);
          if (key2 >= 0)
          {
            isList = true;
            return key2;
          }
        }
      }
      isList = false;
      return -1;
    }

    /// <summary>Ends an object in the output</summary>
    public override void WriteEndObject(XmlDictionaryWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.WriteEndElement();
    }

    /// <summary>Begins an object in the output</summary>
    public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.WriteStartElement("proto");
    }

    /// <summary>Writes the body of an object in the output</summary>
    public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (graph == null)
      {
        writer.WriteAttributeString("nil", "true");
      }
      else
      {
        using (MemoryStream dest1 = new MemoryStream())
        {
          if (this.isList)
          {
            this.model.Serialize((Stream) dest1, graph, (SerializationContext) null);
          }
          else
          {
            using (ProtoWriter dest2 = ProtoWriter.Create((Stream) dest1, this.model))
              this.model.Serialize(this.key, graph, dest2);
          }
          byte[] buffer = dest1.GetBuffer();
          writer.WriteBase64(buffer, 0, (int) dest1.Length);
        }
      }
    }

    /// <summary>
    /// Indicates whether this is the start of an object we are prepared to handle
    /// </summary>
    public override bool IsStartObject(XmlDictionaryReader reader)
    {
      int num = reader != null ? (int) reader.MoveToContent() : throw new ArgumentNullException(nameof (reader));
      return reader.NodeType == XmlNodeType.Element && reader.Name == "proto";
    }

    /// <summary>Reads the body of an object</summary>
    public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
    {
      int num = reader != null ? (int) reader.MoveToContent() : throw new ArgumentNullException(nameof (reader));
      bool isEmptyElement = reader.IsEmptyElement;
      bool flag = reader.GetAttribute("nil") == "true";
      reader.ReadStartElement("proto");
      if (flag)
      {
        if (!isEmptyElement)
          reader.ReadEndElement();
        return (object) null;
      }
      if (isEmptyElement)
      {
        if (this.isList || this.isEnum)
          return this.model.Deserialize(Stream.Null, (object) null, this.type, (SerializationContext) null);
        ProtoReader protoReader = (ProtoReader) null;
        try
        {
          protoReader = ProtoReader.Create(Stream.Null, this.model);
          return this.model.Deserialize(this.key, (object) null, protoReader);
        }
        finally
        {
          ProtoReader.Recycle(protoReader);
        }
      }
      else
      {
        object obj;
        using (MemoryStream source = new MemoryStream(reader.ReadContentAsBase64()))
        {
          if (this.isList || this.isEnum)
          {
            obj = this.model.Deserialize((Stream) source, (object) null, this.type, (SerializationContext) null);
          }
          else
          {
            ProtoReader protoReader = (ProtoReader) null;
            try
            {
              protoReader = ProtoReader.Create((Stream) source, this.model);
              obj = this.model.Deserialize(this.key, (object) null, protoReader);
            }
            finally
            {
              ProtoReader.Recycle(protoReader);
            }
          }
        }
        reader.ReadEndElement();
        return obj;
      }
    }
  }
}
