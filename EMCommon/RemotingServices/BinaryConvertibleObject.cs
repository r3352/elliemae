// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.BinaryConvertibleObject
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public abstract class BinaryConvertibleObject : IXmlSerializable, ICloneable
  {
    protected BinaryObject ToBinaryObject()
    {
      return new BinaryObject(new XmlSerializer().Serialize((object) this), Encoding.Default);
    }

    public static implicit operator BinaryObject(BinaryConvertibleObject obj)
    {
      return obj?.ToBinaryObject();
    }

    public static object Parse(BinaryObject o, Type objectType)
    {
      return o == null ? (object) null : new XmlSerializer().Deserialize(o.ToString(Encoding.Default), objectType);
    }

    public string ToXml() => new XmlSerializer().Serialize((object) this);

    public virtual object Clone()
    {
      return BinaryConvertibleObject.Parse(this.ToBinaryObject(), this.GetType());
    }

    public abstract void GetXmlObjectData(XmlSerializationInfo info);
  }
}
