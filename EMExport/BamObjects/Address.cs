// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.BamObjects.Address
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

#nullable disable
namespace EllieMae.EMLite.Export.BamObjects
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
