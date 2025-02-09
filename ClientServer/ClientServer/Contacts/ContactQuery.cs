// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.ContactQuery
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class ContactQuery : IXmlSerializable
  {
    public string Name;
    public string OrderID;
    public RelatedLoanMatchType LoanMatchType;
    public ContactQueryItem[] Items = new ContactQueryItem[0];

    public ContactQuery()
    {
    }

    public ContactQuery(XmlSerializationInfo info)
    {
      this.Name = info.GetString(nameof (Name));
      this.OrderID = info.GetString(nameof (OrderID));
      this.LoanMatchType = (RelatedLoanMatchType) info.GetValue(nameof (LoanMatchType), typeof (RelatedLoanMatchType));
      this.Items = (ContactQueryItem[]) info.GetValue(nameof (Items), typeof (ContactQueryItem[]));
    }

    public override string ToString() => this.Name != null ? this.Name : string.Empty;

    public override int GetHashCode() => this.Name.GetHashCode();

    public static bool operator ==(ContactQuery o1, ContactQuery o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(ContactQuery o1, ContactQuery o2) => !(o1 == o2);

    public override bool Equals(object obj)
    {
      return obj != null && !(this.GetType() != obj.GetType()) && object.Equals((object) this.Name, (object) ((ContactQuery) obj).Name);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Name", (object) this.Name);
      info.AddValue("OrderID", (object) this.OrderID);
      info.AddValue("LoanMatchType", (object) this.LoanMatchType);
      info.AddValue("Items", (object) this.Items);
    }
  }
}
