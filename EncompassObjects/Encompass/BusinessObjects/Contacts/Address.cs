// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.Address
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Users;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Represents an Address.</summary>
  public class Address : IAddress
  {
    private EllieMae.EMLite.ClientServer.Address addr;
    private EllieMae.EMLite.ClientServer.Address topAddr;
    private Organization linkedOrg;

    internal Address() => this.addr = new EllieMae.EMLite.ClientServer.Address();

    internal Address(EllieMae.EMLite.ClientServer.Address addr) => this.addr = addr;

    internal Address(EllieMae.EMLite.ClientServer.Address addr, EllieMae.EMLite.ClientServer.Address topAddr, Organization org)
      : this(addr)
    {
      this.linkedOrg = org;
      this.topAddr = topAddr;
    }

    /// <summary>
    /// Gets or sets the first line of the street portion of the address.
    /// </summary>
    public string Street1
    {
      get
      {
        return this.linkedOrg != null && this.linkedOrg.UseParentInfo ? this.topAddr.Street1 : this.addr.Street1;
      }
      set
      {
        if (this.linkedOrg != null && this.linkedOrg.UseParentInfo)
          this.linkedOrg.UseParentInfo = false;
        this.addr.Street1 = value ?? "";
      }
    }

    /// <summary>
    /// Gets or sets the second line of the street portion of the address.
    /// </summary>
    public string Street2
    {
      get
      {
        return this.linkedOrg != null && this.linkedOrg.UseParentInfo ? this.topAddr.Street2 : this.addr.Street2;
      }
      set
      {
        if (this.linkedOrg != null && this.linkedOrg.UseParentInfo)
          this.linkedOrg.UseParentInfo = false;
        this.addr.Street2 = value ?? "";
      }
    }

    /// <summary>Gets or sets the city portion of the address.</summary>
    public string City
    {
      get
      {
        return this.linkedOrg != null && this.linkedOrg.UseParentInfo ? this.topAddr.City : this.addr.City;
      }
      set
      {
        if (this.linkedOrg != null && this.linkedOrg.UseParentInfo)
          this.linkedOrg.UseParentInfo = false;
        this.addr.City = value ?? "";
      }
    }

    /// <summary>
    /// Gets or sets the two characters state code for the address.
    /// </summary>
    public string State
    {
      get
      {
        return this.linkedOrg != null && this.linkedOrg.UseParentInfo ? this.topAddr.State : this.addr.State;
      }
      set
      {
        if (this.linkedOrg != null && this.linkedOrg.UseParentInfo)
          this.linkedOrg.UseParentInfo = false;
        this.addr.State = value ?? "";
      }
    }

    /// <summary>Gets or sets the zip/postal code for the address.</summary>
    public string Zip
    {
      get
      {
        return this.linkedOrg != null && this.linkedOrg.UseParentInfo ? this.topAddr.Zip : this.addr.Zip;
      }
      set
      {
        if (this.linkedOrg != null && this.linkedOrg.UseParentInfo)
          this.linkedOrg.UseParentInfo = false;
        this.addr.Zip = value ?? "";
      }
    }

    /// <summary>Gets or sets the Unit Type for the address</summary>
    public string UnitType
    {
      get
      {
        return this.linkedOrg != null && this.linkedOrg.UseParentInfo ? this.topAddr.UnitType : this.addr.UnitType;
      }
      set
      {
        if (this.linkedOrg != null && this.linkedOrg.UseParentInfo)
          this.linkedOrg.UseParentInfo = false;
        this.addr.UnitType = value ?? "";
      }
    }
  }
}
