// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Address
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class Address : IXmlSerializable, ICloneable
  {
    private string street1;
    private string street2;
    private string city;
    private string state;
    private string zip;
    private string unitType;

    public Address()
    {
      this.street1 = "";
      this.street2 = "";
      this.city = "";
      this.state = "";
      this.zip = "";
      this.unitType = "";
    }

    public Address(string street1, string street2, string city, string state, string zip)
    {
      this.street1 = street1;
      this.street2 = street2;
      this.city = city;
      this.state = state;
      this.zip = zip;
      this.unitType = "";
    }

    public Address(
      string street1,
      string street2,
      string city,
      string state,
      string zip,
      string unitType)
    {
      this.street1 = street1;
      this.street2 = street2;
      this.city = city;
      this.state = state;
      this.zip = zip;
      this.unitType = unitType;
    }

    public Address(Address source)
    {
      this.street1 = source.street1;
      this.street2 = source.street2;
      this.city = source.city;
      this.state = source.state;
      this.zip = source.zip;
      this.unitType = source.unitType;
    }

    public Address(XmlSerializationInfo info)
    {
      this.street1 = info.GetString(nameof (street1));
      this.street2 = info.GetString(nameof (street2));
      this.city = info.GetString(nameof (city));
      this.state = info.GetString(nameof (state));
      this.zip = info.GetString(nameof (zip));
      this.unitType = info.GetString("unittype");
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("street1", (object) this.street1);
      info.AddValue("street2", (object) this.street2);
      info.AddValue("city", (object) this.city);
      info.AddValue("state", (object) this.state);
      info.AddValue("zip", (object) this.zip);
      info.AddValue("unittype", (object) this.unitType);
    }

    public string UnitType
    {
      get => this.unitType;
      set => this.unitType = value ?? "";
    }

    public string Street1
    {
      get => this.street1;
      set => this.street1 = value ?? "";
    }

    public string Street2
    {
      get => this.street2;
      set => this.street2 = value ?? "";
    }

    public string City
    {
      get => this.city;
      set => this.city = value ?? "";
    }

    public string State
    {
      get => this.state;
      set => this.state = value ?? "";
    }

    public string Zip
    {
      get => this.zip;
      set => this.zip = value ?? "";
    }

    public object Clone() => (object) new Address(this);

    public override string ToString()
    {
      return this.street1 + " " + this.street2 + " " + this.city + ", " + this.state + " " + this.zip;
    }

    public string ToString(bool excludeStree2)
    {
      if (!excludeStree2)
        return this.ToString();
      return this.street1 + " " + this.city + ", " + this.state + " " + this.zip;
    }
  }
}
