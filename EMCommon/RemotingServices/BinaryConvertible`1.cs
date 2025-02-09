// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.BinaryConvertible`1
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public abstract class BinaryConvertible<T> : BinaryConvertibleObject
  {
    public static T Parse(string xml)
    {
      return (xml ?? "") == "" ? default (T) : (T) new XmlSerializer().Deserialize(xml, typeof (T));
    }

    public static T Parse(BinaryObject o) => (T) BinaryConvertibleObject.Parse(o, typeof (T));
  }
}
