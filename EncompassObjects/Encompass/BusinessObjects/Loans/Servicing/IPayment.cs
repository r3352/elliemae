// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.IPayment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>Interface for ServicingTransaction class.</summary>
  /// <exclude />
  [Guid("0F470A63-041B-42f1-A547-DAA50DD1BF15")]
  public interface IPayment
  {
    string ID { get; }

    ServicingTransactionType TransactionType { get; }

    DateTime TransactionDate { get; set; }

    Decimal TransactionAmount { get; set; }

    ServicingPaymentMethod PaymentMethod { get; set; }

    string CreatedBy { get; }

    DateTime CreationDate { get; }

    string LastModifiedBy { get; }

    DateTime LastModifiedDate { get; }

    int PaymentNumber { get; }

    DateTime ScheduledDueDate { get; }

    DateTime StatementDate { get; set; }

    DateTime PaymentDueDate { get; set; }

    DateTime LatePaymentDate { get; set; }

    DateTime PaymentDepositedDate { get; set; }

    Decimal IndexRate { get; }

    Decimal InterestRate { get; }

    Decimal AmountDue { get; }

    Decimal Principal { get; set; }

    Decimal Interest { get; set; }

    Decimal Escrow { get; set; }

    Decimal LateFee { get; set; }

    Decimal MiscFee { get; set; }

    Decimal AdditionalPrincipal { get; set; }

    Decimal AdditionalEscrow { get; set; }

    Decimal LateFeeIfLate { get; set; }

    string InstitutionName { get; set; }

    string InstitutionRouting { get; set; }

    string AccountNumber { get; set; }

    string AccountHolder { get; set; }

    string Reference { get; set; }

    string CheckNumber { get; set; }

    Decimal PaymentAmount { get; set; }

    DateTime PaymentDate { get; set; }

    string Comments { get; set; }

    bool IsReversed();

    PaymentReversal GetReversal();

    PaymentReversal Reverse(DateTime reversalDate);
  }
}
