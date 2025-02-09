// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Serialization.XmlSerializer
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Serialization
{
  public class XmlSerializer
  {
    public void Serialize(Stream serializationStream, object graph)
    {
      StreamWriter streamWriter = new StreamWriter(serializationStream, Encoding.Default);
      streamWriter.Write(this.Serialize(graph));
      streamWriter.Flush();
    }

    public string Serialize(object graph)
    {
      XmlSerializationInfo serializationInfo = new XmlSerializationInfo();
      serializationInfo.AddValue("root", graph);
      return serializationInfo.ToString();
    }

    public string Serialize(XmlElement containerElement, string name, object graph)
    {
      XmlSerializationInfo serializationInfo = new XmlSerializationInfo(containerElement);
      serializationInfo.AddValue(name, graph);
      return serializationInfo.ToString();
    }

    public object Deserialize(string xmlData, Type valueType)
    {
      return new XmlSerializationInfo(xmlData).GetValue("root", valueType);
    }

    public T Deserialize<T>(string xmlData)
    {
      return new XmlSerializationInfo(xmlData).GetValue<T>("root");
    }

    public object Deserialize(Stream serializationStream, Type valueType)
    {
      return new XmlSerializationInfo(serializationStream).GetValue("root", valueType);
    }

    public T Deserialize<T>(Stream serializationStream)
    {
      return new XmlSerializationInfo(serializationStream).GetValue<T>("root");
    }

    public object Deserialize(XmlElement containerElement, string name, Type valueType)
    {
      return new XmlSerializationInfo(containerElement).GetValue(name, valueType);
    }
  }
}
