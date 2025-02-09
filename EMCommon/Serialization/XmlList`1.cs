// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Serialization.XmlList`1
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Serialization
{
  [Serializable]
  public class XmlList<T> : List<T>, IXmlSerializable
  {
    public XmlList()
    {
    }

    public XmlList(IEnumerable<T> items)
      : base(items)
    {
    }

    public XmlList(XmlSerializationInfo info)
    {
      foreach (string str in info)
      {
        if (XmlList<T>.isNumeric(str))
          this.Add((T) info.GetValue(str, typeof (T)));
      }
    }

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
      for (int index = 0; index < this.Count; ++index)
        info.AddValue(index.ToString(), (object) this[index]);
    }

    public static XmlList<T> Parse(string xml)
    {
      return (xml ?? "") == "" ? (XmlList<T>) null : (XmlList<T>) new XmlSerializer().Deserialize(xml, typeof (XmlList<T>));
    }

    public string ToXml() => new XmlSerializer().Serialize((object) this);

    private static bool isNumeric(string text)
    {
      for (int index = 0; index < text.Length; ++index)
      {
        if (!char.IsDigit(text, index))
          return false;
      }
      return true;
    }
  }
}
