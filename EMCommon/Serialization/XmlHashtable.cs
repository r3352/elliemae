// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Serialization.XmlHashtable
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Serialization
{
  public abstract class XmlHashtable : Dictionary<string, object>, IXmlSerializable
  {
    private Dictionary<string, object> sortedList = new Dictionary<string, object>();

    public XmlHashtable()
    {
    }

    public XmlHashtable(IDictionary dataElements)
    {
      foreach (DictionaryEntry dataElement in dataElements)
        this[dataElement.Key.ToString()] = dataElement.Value;
    }

    public XmlHashtable(XmlSerializationInfo info, Type valueType)
    {
      foreach (string str in info)
        this.Add(str, info.GetValue(str, valueType));
    }

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
      foreach (string key in this.Keys)
        info.AddValue(key, this[key]);
    }

    public new object this[string key]
    {
      get => this.ContainsKey(key) ? base[key] : (object) null;
      set
      {
        if (this.ContainsKey(key))
          base[key] = value;
        else
          this.Add(key, value);
      }
    }
  }
}
