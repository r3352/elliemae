// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.BorrowerStatusItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class BorrowerStatusItem : IComparable, IXmlSerializable
  {
    public string name = "";
    public int index = -1;

    public BorrowerStatusItem()
    {
    }

    public BorrowerStatusItem(string name, int index)
    {
      this.name = name;
      this.index = index;
    }

    public BorrowerStatusItem(XmlSerializationInfo info)
    {
      this.name = info.GetString(nameof (name));
      this.index = info.GetInteger(nameof (index));
    }

    public override string ToString() => this.name != null ? this.name : string.Empty;

    public override int GetHashCode() => this.name.GetHashCode();

    public static bool operator ==(BorrowerStatusItem o1, BorrowerStatusItem o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(BorrowerStatusItem o1, BorrowerStatusItem o2) => !(o1 == o2);

    public override bool Equals(object obj)
    {
      return obj != null && !(this.GetType() != obj.GetType()) && object.Equals((object) this.name, (object) ((BorrowerStatusItem) obj).name);
    }

    public int CompareTo(object obj)
    {
      BorrowerStatusItem borrowerStatusItem = (BorrowerStatusItem) obj;
      if (this.index > borrowerStatusItem.index)
        return 1;
      return this.index < borrowerStatusItem.index ? -1 : 0;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("name", (object) this.name);
      info.AddValue("index", (object) this.index);
    }
  }
}
