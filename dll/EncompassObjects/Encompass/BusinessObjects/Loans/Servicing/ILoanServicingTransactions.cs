// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ILoanServicingTransactions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  [Guid("BAEB4681-17D0-45e5-AD15-4E13AD8E42A0")]
  public interface ILoanServicingTransactions
  {
    Payment AddPayment(DateTime paymentDate);

    EscrowDisbursement AddDisbursement(
      DateTime disDate,
      Decimal disAmount,
      ServicingDisbursementType disType);

    EscrowInterest AddEscrowInterest(DateTime transDate, Decimal transAmount);

    OtherTransaction AddOtherTransaction(DateTime transDate, Decimal transAmount);

    ServicingTransaction GetTransactionByID(string transId);

    ServicingTransactionList GetTransactions(ServicingTransactionType transType);

    void Remove(ServicingTransaction trans);

    IEnumerator GetEnumerator();
  }
}
