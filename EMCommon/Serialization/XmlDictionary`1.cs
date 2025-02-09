// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Serialization.XmlDictionary`1
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.Serialization
{
  [Serializable]
  public class XmlDictionary<T> : Dictionary<string, T>, IXmlSerializable
  {
    public XmlDictionary()
    {
    }

    public XmlDictionary(IDictionary<string, T> dictionary)
      : base(dictionary)
    {
    }

    public XmlDictionary(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public XmlDictionary(XmlSerializationInfo info)
    {
      foreach (string str in info)
        this.Add(str, (T) info.GetValue(str, typeof (T)));
    }

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
      foreach (KeyValuePair<string, T> keyValuePair in (Dictionary<string, T>) this)
        info.AddValue(keyValuePair.Key, (object) keyValuePair.Value);
    }

    public static XmlDictionary<T> Parse(string xml)
    {
      return (xml ?? "") == "" ? (XmlDictionary<T>) null : (XmlDictionary<T>) new XmlSerializer().Deserialize(xml, typeof (XmlDictionary<T>));
    }

    public string ToXml() => new XmlSerializer().Serialize((object) this);
  }
}
