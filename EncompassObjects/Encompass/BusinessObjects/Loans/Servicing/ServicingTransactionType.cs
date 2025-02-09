// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ServicingTransactionType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Enumeration for the support interim servicing transaction types.
  /// </summary>
  [Guid("E9F83C8F-6011-4b81-A8B2-C199873BBD1D")]
  public enum ServicingTransactionType
  {
    /// <summary>No transaction type specified.</summary>
    None,
    /// <summary>A payment made by the borrower.</summary>
    Payment,
    /// <summary>Reversal of a payment made by the borrower.</summary>
    PaymentReversal,
    /// <summary>A disbursement from the escrow account.</summary>
    EscrowDisbursement,
    /// <summary>Interest paid on the escrow balance.</summary>
    EscrowInterest,
    /// <summary>Reversal of a disbursement paid from escrow.</summary>
    EscrowDisbursementReversal,
    /// <summary>A custom or user-defined transaction.</summary>
    Other,
  }
}
