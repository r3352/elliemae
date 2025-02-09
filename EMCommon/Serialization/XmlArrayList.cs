// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Serialization.XmlArrayList
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Serialization
{
  public class XmlArrayList : ArrayList, IXmlSerializable
  {
    public XmlArrayList()
    {
    }

    public XmlArrayList(ICollection items)
      : base(items)
    {
    }

    public XmlArrayList(XmlSerializationInfo info, Type valueType)
    {
      foreach (string name in info)
        this.Add(info.GetValue(name, valueType));
    }

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
      for (int index = 0; index < this.Count; ++index)
        info.AddValue(index.ToString(), this[index]);
    }
  }
}
