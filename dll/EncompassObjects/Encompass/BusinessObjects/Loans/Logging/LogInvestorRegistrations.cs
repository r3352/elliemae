// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogInvestorRegistrations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogInvestorRegistrations : 
    LoanLogEntryCollection,
    ILogInvestorRegistrations,
    IEnumerable
  {
    internal LogInvestorRegistrations(Loan loan)
      : base(loan, typeof (RegistrationLog))
    {
    }

    public InvestorRegistration this[int index] => (InvestorRegistration) this.LogEntries[index];

    public InvestorRegistration Add(DateTime registrationDate)
    {
      return (InvestorRegistration) this.CreateEntry((LogRecordBase) new RegistrationLog()
      {
        RegisteredByID = this.Loan.Session.UserID,
        RegisteredByName = this.Loan.Session.GetCurrentUser().FullName,
        RegisteredDate = registrationDate
      });
    }

    public InvestorRegistration GetCurrent()
    {
      foreach (InvestorRegistration current in (LoanLogEntryCollection) this)
      {
        if (current.Current)
          return current;
      }
      return (InvestorRegistration) null;
    }

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new InvestorRegistration(this.Loan, logRecord);
    }
  }
}
