// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ServicingTransactionType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  [Guid("E9F83C8F-6011-4b81-A8B2-C199873BBD1D")]
  public enum ServicingTransactionType
  {
    None,
    Payment,
    PaymentReversal,
    EscrowDisbursement,
    EscrowInterest,
    EscrowDisbursementReversal,
    Other,
  }
}
