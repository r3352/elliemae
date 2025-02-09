// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BamObjects.Address
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

#nullable disable
namespace EllieMae.EMLite.ePass.BamObjects
{
  public class Address
  {
    public string City { get; set; }

    public Address(string city, string state, string street1, string street2, string zip)
    {
      this.City = city;
      this.State = state;
      this.Street1 = street1;
      this.Street2 = street2;
      this.Zip = zip;
    }

    public string State { get; set; }

    public string Street1 { get; set; }

    public string Street2 { get; set; }

    public string Zip { get; set; }

    public string AddressFullName
    {
      get
      {
        return this.Street1 + " " + this.Street2 + " " + this.City + " " + this.State + " " + this.Zip;
      }
    }
  }
}
