// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.LoanServicing
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  public class LoanServicing : ILoanServicing
  {
    private Loan loan;
    private LoanServicingTransactions transactions;
    private bool isStarted;

    internal LoanServicing(Loan loan)
    {
      this.loan = loan;
      this.transactions = new LoanServicingTransactions(loan);
    }

    public bool IsStarted()
    {
      if (this.isStarted)
        return true;
      this.isStarted = this.loan.Unwrap().LoanData.GetPaymentScheduleSnapshot() != null;
      return this.isStarted;
    }

    public void Start()
    {
      if (this.IsStarted())
        return;
      if (!this.loan.Session.SessionObjects.ServerLicense.IsBankerEdition)
        throw new NotSupportedException("The specified operation is not supported by the current version of Encompass");
      string unformattedValue = this.loan.Fields["SERVICE.X8"].UnformattedValue;
      if (unformattedValue == "Foreclosure" || unformattedValue == "Servicing Released")
        throw new InvalidOperationException("Servicing cannot be started on a loan that has been foreclosure or released.");
      if (this.loan.Fields["682"].IsEmpty())
        throw new InvalidOperationException("First Payment Date (field 682) must be set prior to start of servicing.");
      this.loan.EnsureExclusive();
      this.loan.Unwrap().LoanData.StartInterimServicing();
      this.isStarted = true;
    }

    public LoanServicingTransactions Transactions
    {
      get
      {
        if (!this.IsStarted())
          throw new InvalidOperationException("Servicing has not been started for this loan. Call Start() method first.");
        return this.transactions;
      }
    }

    public void Recalculate()
    {
      this.loan.EnsureExclusive();
      this.loan.Unwrap().Calculator.CalculateInterimServicing(true);
    }

    public PaymentSchedule GetPaymentSchedule()
    {
      return new PaymentSchedule(this.loan.Unwrap().LoanData.GetPaymentScheduleSnapshot() ?? throw new InvalidOperationException("Servicing has not been started on this loan. Call the Start() method first."));
    }
  }
}
