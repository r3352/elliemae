// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactOpportunity
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common.Contact;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  public class ContactOpportunity : SessionBoundObject, IContactOpportunity
  {
    private EllieMae.Encompass.BusinessObjects.Contacts.Contact contact;
    private Opportunity opp;
    private Address propertyAddress;

    internal ContactOpportunity(EllieMae.Encompass.BusinessObjects.Contacts.Contact contact, Opportunity opp)
      : base(contact.Session)
    {
      this.contact = contact;
      this.opp = opp;
    }

    public int ID => this.opp.OpportunityID;

    public Decimal LoanAmount
    {
      get => this.opp.LoanAmount;
      set => this.opp.LoanAmount = value;
    }

    public LoanPurpose Purpose
    {
      get => (LoanPurpose) this.opp.Purpose;
      set => this.opp.Purpose = (LoanPurpose) value;
    }

    public string PurposeOther
    {
      get => this.opp.PurposeOther;
      set => this.opp.PurposeOther = value;
    }

    public int Term
    {
      get => this.opp.Term;
      set => this.opp.Term = value;
    }

    public AmortizationType Amortization
    {
      get => (AmortizationType) this.opp.Amortization;
      set => this.opp.Amortization = (AmortizationType) value;
    }

    public Decimal DownPayment
    {
      get => this.opp.DownPayment;
      set => this.opp.DownPayment = value;
    }

    public Address PropertyAddress
    {
      get
      {
        if (this.propertyAddress == null)
          this.propertyAddress = new Address(this.opp.PropertyAddress);
        return this.propertyAddress;
      }
    }

    public PropertyUse PropertyUse
    {
      get => (PropertyUse) this.opp.PropUse;
      set => this.opp.PropUse = (PropertyUse) value;
    }

    public PropertyType PropertyType
    {
      get => (PropertyType) this.opp.PropType;
      set => this.opp.PropType = (PropertyType) value;
    }

    public Decimal PropertyValue
    {
      get => this.opp.PropertyValue;
      set => this.opp.PropertyValue = value;
    }

    public Decimal MortgageBalance
    {
      get => this.opp.MortgageBalance;
      set => this.opp.MortgageBalance = value;
    }

    public float MortgageRate
    {
      get => (float) this.opp.MortgageRate;
      set => this.opp.MortgageRate = (Decimal) value;
    }

    public Decimal HousingPayment
    {
      get => this.opp.HousingPayment;
      set => this.opp.HousingPayment = value;
    }

    public Decimal NonHousingPayment
    {
      get => this.opp.NonhousingPayment;
      set => this.opp.NonhousingPayment = value;
    }

    public DateTime PurchaseDate
    {
      get => this.opp.PurchaseDate;
      set => this.opp.PurchaseDate = value;
    }

    public string CreditRating
    {
      get => this.opp.CreditRating;
      set => this.opp.CreditRating = value;
    }

    public bool InBankruptcy
    {
      get => this.opp.IsBankruptcy;
      set => this.opp.IsBankruptcy = value;
    }

    public EmploymentStatus EmploymentStatus
    {
      get => (EmploymentStatus) this.opp.Employment;
      set => this.opp.Employment = (EmploymentStatus) value;
    }

    public void Commit()
    {
      this.Session.Contacts.ContactManager.UpdateBorrowerOpportunity(this.opp);
    }

    public void Refresh()
    {
      this.opp = this.Session.Contacts.ContactManager.GetBorrowerOpportunity(this.ID);
    }
  }
}
