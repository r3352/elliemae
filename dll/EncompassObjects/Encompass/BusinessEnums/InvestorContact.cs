// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.InvestorContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class InvestorContact : EnumItem, IInvestorContact
  {
    private ContactInformation contactInfo;
    private InvestorContactType contactType;

    internal InvestorContact(
      int id,
      ContactInformation contactInfo,
      InvestorContactType contactType)
      : base(id, contactType.ToString())
    {
      this.contactInfo = contactInfo;
      this.contactType = contactType;
    }

    public InvestorContactType ContactType => this.contactType;

    public string EntityName => this.contactInfo.EntityName;

    public string ContactName => this.contactInfo.ContactName;

    public string Street1 => this.contactInfo.Address.Street1;

    public string Street2 => this.contactInfo.Address.Street2;

    public string City => this.contactInfo.Address.City;

    public string State => this.contactInfo.Address.State;

    public string Zip => this.contactInfo.Address.Zip;

    public string PhoneNumber => this.contactInfo.PhoneNumber;

    public string FaxNumber => this.contactInfo.FaxNumber;

    public string EmailAddress => this.contactInfo.EmailAddress;

    public string WebSite => this.contactInfo.WebSite;
  }
}
