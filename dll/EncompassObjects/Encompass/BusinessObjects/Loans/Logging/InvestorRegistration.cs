// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.InvestorRegistration
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class InvestorRegistration : LogEntry, IInvestorRegistration
  {
    private RegistrationLog regLog;

    internal InvestorRegistration(Loan loan, LogRecordBase logItem)
      : base(loan, logItem)
    {
      this.regLog = (RegistrationLog) logItem;
    }

    public override LogEntryType EntryType => LogEntryType.InvestorRegistration;

    public string RegisteredBy => this.regLog.RegisteredByID;

    public string InvestorName
    {
      get => this.regLog.InvestorName;
      set => this.regLog.InvestorName = value ?? "";
    }

    public DateTime DateRegistered
    {
      get => this.regLog.RegisteredDate;
      set => this.regLog.RegisteredDate = value;
    }

    public object ExpirationDate
    {
      get
      {
        return !(this.regLog.ExpiredDate == DateTime.MinValue) ? (object) this.regLog.ExpiredDate : (object) null;
      }
      set => this.regLog.ExpiredDate = value == null ? DateTime.MinValue : (DateTime) value;
    }

    public bool Expired => this.regLog.IsExpired;

    public bool Current => this.regLog.IsCurrent;

    public string ReferenceNumber
    {
      get => this.regLog.Reference;
      set => this.regLog.Reference = value ?? "";
    }
  }
}
