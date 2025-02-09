// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.InvestorContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The InvestorContact class represents an Investor Contact on an Investor Template defined in Encompass settings.
  /// </summary>
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

    /// <summary>Gets the type of the InvestorContact</summary>
    public InvestorContactType ContactType => this.contactType;

    /// <summary>Gets the EntityName of the InvestorContact</summary>
    public string EntityName => this.contactInfo.EntityName;

    /// <summary>Gets the ContactName of the InvestorContact</summary>
    public string ContactName => this.contactInfo.ContactName;

    /// <summary>Gets the Street1 of the InvestorContact</summary>
    public string Street1 => this.contactInfo.Address.Street1;

    /// <summary>Gets the Street2 of the InvestorContact</summary>
    public string Street2 => this.contactInfo.Address.Street2;

    /// <summary>Gets the City of the InvestorContact</summary>
    public string City => this.contactInfo.Address.City;

    /// <summary>Gets the State of the InvestorContact</summary>
    public string State => this.contactInfo.Address.State;

    /// <summary>Gets the Zip Code</summary>
    public string Zip => this.contactInfo.Address.Zip;

    /// <summary>Gets the PhoneNumber of the InvestorContact</summary>
    public string PhoneNumber => this.contactInfo.PhoneNumber;

    /// <summary>Gets the FaxNumber of the InvestorContact</summary>
    public string FaxNumber => this.contactInfo.FaxNumber;

    /// <summary>Gets the EmailAddress of the InvestorContact</summary>
    public string EmailAddress => this.contactInfo.EmailAddress;

    /// <summary>Gets the WebSite of the InvestorContact</summary>
    public string WebSite => this.contactInfo.WebSite;
  }
}
