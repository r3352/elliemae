// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.IPaymentSchedule
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>Interface for PaymentSchedule class.</summary>
  /// <exclude />
  [Guid("72624D1A-4AC6-4ca4-922D-4E1A3714EB91")]
  public interface IPaymentSchedule
  {
    ScheduledPayments Payments { get; }

    ScheduledDisbursements Disbursements { get; }

    int DaysPerYear { get; }

    int LoanTerm { get; }

    int ActualTerm { get; }

    bool IsARM { get; }

    bool IsBiweekly { get; }

    bool IsNegativeARM { get; }

    Decimal LoanAmount { get; }

    Decimal MarginRate { get; }

    Decimal MinLateFee { get; }

    Decimal MaxLateFee { get; }

    Decimal NoteRate { get; }

    int PaymentsPerYear { get; }

    int PaymentDueDays { get; }

    int StatementDueDays { get; }
  }
}
