// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.Address
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.BusinessObjects.Users;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  public class Address : IAddress
  {
    private Address addr;
    private Address topAddr;
    private Organization linkedOrg;

    internal Address() => this.addr = new Address();

    internal Address(Address addr) => this.addr = addr;

    internal Address(Address addr, Address topAddr, Organization org)
      : this(addr)
    {
      this.linkedOrg = org;
      this.topAddr = topAddr;
    }

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
