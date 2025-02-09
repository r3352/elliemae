// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.IScheduledPayment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>Interface for ScheduledPayment class.</summary>
  /// <exclude />
  [Guid("6C67F9FE-FCB1-4c77-BB01-72EE4690EFA7")]
  public interface IScheduledPayment
  {
    int Index { get; }

    DateTime DueDate { get; }

    Decimal InterestRate { get; }

    Decimal Principal { get; }

    Decimal Interest { get; }

    Decimal MortgageInsurance { get; }

    Decimal Balance { get; }

    Decimal Total { get; }
  }
}
