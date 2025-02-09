// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.ILoanServicing
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>Interface for LoanServicing class.</summary>
  /// <exclude />
  [Guid("A6EC13F1-9C1F-4055-A85D-892B3CE0BDA9")]
  public interface ILoanServicing
  {
    bool IsStarted();

    void Start();

    LoanServicingTransactions Transactions { get; }

    void Recalculate();

    PaymentSchedule GetPaymentSchedule();
  }
}
