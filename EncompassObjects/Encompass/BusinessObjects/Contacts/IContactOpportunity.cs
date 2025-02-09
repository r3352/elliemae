// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IContactOpportunity
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Interface for ContactOpportunity class.</summary>
  /// <exclude />
  [Guid("87E74AD0-29AD-4003-8B68-F0AEA9CE8953")]
  public interface IContactOpportunity
  {
    int ID { get; }

    Decimal LoanAmount { [return: MarshalAs(UnmanagedType.Currency)] get; [param: MarshalAs(UnmanagedType.Currency)] set; }

    LoanPurpose Purpose { get; set; }

    string PurposeOther { get; set; }

    int Term { get; set; }

    AmortizationType Amortization { get; set; }

    Decimal DownPayment { [return: MarshalAs(UnmanagedType.Currency)] get; [param: MarshalAs(UnmanagedType.Currency)] set; }

    Address PropertyAddress { get; }

    PropertyUse PropertyUse { get; set; }

    PropertyType PropertyType { get; set; }

    Decimal PropertyValue { [return: MarshalAs(UnmanagedType.Currency)] get; [param: MarshalAs(UnmanagedType.Currency)] set; }

    Decimal MortgageBalance { [return: MarshalAs(UnmanagedType.Currency)] get; [param: MarshalAs(UnmanagedType.Currency)] set; }

    float MortgageRate { get; set; }

    Decimal HousingPayment { [return: MarshalAs(UnmanagedType.Currency)] get; [param: MarshalAs(UnmanagedType.Currency)] set; }

    Decimal NonHousingPayment { [return: MarshalAs(UnmanagedType.Currency)] get; [param: MarshalAs(UnmanagedType.Currency)] set; }

    DateTime PurchaseDate { get; set; }

    string CreditRating { get; set; }

    bool InBankruptcy { get; set; }

    EmploymentStatus EmploymentStatus { get; set; }

    void Commit();

    void Refresh();
  }
}
