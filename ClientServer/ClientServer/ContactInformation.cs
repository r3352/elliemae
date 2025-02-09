// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ContactInformation
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [CLSCompliant(true)]
  [Serializable]
  public class ContactInformation : BinaryConvertible<ContactInformation>
  {
    private string entityName = "";
    private string contactName = "";
    private Address address = new Address();
    private string phoneNumber = "";
    private string faxNumber = "";
    private string email = "";
    private string webSite = "";

    public ContactInformation()
    {
    }

    public ContactInformation(ContactInformation source)
    {
      this.entityName = source.EntityName;
      this.contactName = source.ContactName;
      this.address = new Address(source.Address);
      this.phoneNumber = source.PhoneNumber;
      this.faxNumber = source.FaxNumber;
      this.email = source.EmailAddress;
      this.webSite = source.WebSite;
    }

    public ContactInformation(XmlSerializationInfo info)
    {
      this.entityName = info.GetString("entity");
      this.contactName = info.GetString("contact");
      this.address = (Address) info.GetValue(nameof (address), typeof (Address));
      this.phoneNumber = info.GetString("phone");
      this.faxNumber = info.GetString("fax");
      this.email = info.GetString(nameof (email));
      this.webSite = info.GetString(nameof (webSite), "");
    }

    public string EntityName
    {
      get => this.entityName;
      set => this.entityName = value;
    }

    public string ContactName
    {
      get => this.contactName;
      set => this.contactName = value;
    }

    public Address Address
    {
      get => this.address == null ? new Address() : this.address;
      set => this.address = value;
    }

    public string PhoneNumber
    {
      get => this.phoneNumber;
      set => this.phoneNumber = value;
    }

    public string FaxNumber
    {
      get => this.faxNumber;
      set => this.faxNumber = value;
    }

    public string EmailAddress
    {
      get => this.email;
      set => this.email = value;
    }

    public string WebSite
    {
      get => this.webSite;
      set => this.webSite = value;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("entity", (object) this.entityName);
      info.AddValue("contact", (object) this.contactName);
      info.AddValue("address", (object) this.address);
      info.AddValue("phone", (object) this.phoneNumber);
      info.AddValue("fax", (object) this.faxNumber);
      info.AddValue("email", (object) this.email);
      info.AddValue("webSite", (object) this.webSite);
    }
  }
}
