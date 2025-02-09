// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.IEscrowInterest
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>Interface for EscrowInterest class.</summary>
  /// <exclude />
  [Guid("6443AD54-1253-455b-B8C9-3EF0186C9AF9")]
  public interface IEscrowInterest
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

    string Comments { get; set; }
  }
}
