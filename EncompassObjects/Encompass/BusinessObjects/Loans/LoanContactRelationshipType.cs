// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanContactRelationshipType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Enumeration of the possible relationships between a Contact and a Loan.
  /// </summary>
  [Guid("16A123B2-7D87-4f16-884C-1E73FE8517B4")]
  public enum LoanContactRelationshipType
  {
    /// <summary>Indicates the contact is a Borrower</summary>
    Borrower,
    /// <summary>Indicates the contact is a Borrower</summary>
    Coborrower,
    /// <summary>Indicates the contact is a Lender</summary>
    Lender,
    /// <summary>Indicates the contact is a Appraiser</summary>
    Appraiser,
    /// <summary>Indicates the contact is a EscrowCompany</summary>
    EscrowCompany,
    /// <summary>Indicates the contact is a TitleInsuranceCompany</summary>
    TitleInsuranceCompany,
    /// <summary>Indicates the contact is a BuyerAttorney</summary>
    BuyerAttorney,
    /// <summary>Indicates the contact is a SellerAttorney</summary>
    SellerAttorney,
    /// <summary>Indicates the contact is a BuyerAgent</summary>
    BuyerAgent,
    /// <summary>Indicates the contact is a SellerAgent</summary>
    SellerAgent,
    /// <summary>Indicates the contact is a Seller1</summary>
    Seller1,
    /// <summary>Indicates the contact is a Seller2</summary>
    Seller2,
    /// <summary>Indicates the contact is a Notary</summary>
    Notary,
    /// <summary>Indicates the contact is a Builder</summary>
    Builder,
    /// <summary>Indicates the contact is a HazardInsurance</summary>
    HazardInsurance,
    /// <summary>Indicates the contact is a MortgageInsurance</summary>
    MortgageInsurance,
    /// <summary>Indicates the contact is a Surveyor</summary>
    Surveyor,
    /// <summary>Indicates the contact is a FloodInsurance</summary>
    FloodInsurance,
    /// <summary>Indicates the contact is a CreditCompany</summary>
    CreditCompany,
    /// <summary>Indicates the contact is a Underwriter</summary>
    Underwriter,
    /// <summary>Indicates the contact is a Servicing</summary>
    Servicing,
    /// <summary>Indicates the contact is a DocSigning</summary>
    DocSigning,
    /// <summary>Indicates the contact is a Warehouse</summary>
    Warehouse,
    /// <summary>Indicates the contact is a FinancialPlanner</summary>
    FinancialPlanner,
    /// <summary>Indicates the contact is a Investor</summary>
    Investor,
    /// <summary>Indicates the contact is a AssignTo</summary>
    AssignTo,
    /// <summary>Indicates the contact is a Broker</summary>
    Broker,
    /// <summary>Indicates the contact is a DocsPreparedBy</summary>
    DocsPreparedBy,
    /// <summary>Indicates the contact is a CustomCategory1</summary>
    CustomCategory1,
    /// <summary>Indicates the contact is a CustomCategory2</summary>
    CustomCategory2,
    /// <summary>Indicates the contact is a CustomCategory3</summary>
    CustomCategory3,
    /// <summary>Indicates the contact is a CustomCategory4</summary>
    CustomCategory4,
    /// <summary>Indicates the contact is a Referral</summary>
    Referral,
    /// <summary>Indicates the contact is a NotFound</summary>
    NotFound,
    /// <summary>Indicates the contact is a Employer</summary>
    Employer,
    /// <summary>Indicates the contact is a Depository</summary>
    Depository,
    /// <summary>Indicates the contact is a Liability</summary>
    Liability,
    /// <summary>Indicates the contact is a Residence</summary>
    Residence,
    /// <summary>Indicates the contact is a Seller3</summary>
    Seller3,
    /// <summary>Indicates the contact is a Seller4</summary>
    Seller4,
    /// <summary>Indicates the contact is a BoSettlementAgentrrower</summary>
    SettlementAgent,
    /// <summary>Indicates the contact is a SellerCorporationOfficer</summary>
    SellerCorporationOfficer,
  }
}
