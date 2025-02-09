// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.IOtherTransaction
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
  [Guid("7D0F561E-CF33-444c-80A6-068B97037C43")]
  public interface IOtherTransaction
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

    string InstitutionName { get; set; }

    string InstitutionRouting { get; set; }

    string AccountNumber { get; set; }

    string Reference { get; set; }

    string Comments { get; set; }
  }
}
