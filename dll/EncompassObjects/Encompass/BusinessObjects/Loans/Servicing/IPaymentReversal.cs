// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.IPaymentReversal
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  [Guid("C46FE3DC-0F55-49aa-A3ED-323BB7FBDDFF")]
  public interface IPaymentReversal
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

    Payment GetPayment();
  }
}
