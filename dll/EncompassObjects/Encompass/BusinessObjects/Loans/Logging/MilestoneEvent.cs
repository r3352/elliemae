// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessEnums;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class MilestoneEvent : LogEntry, IMilestoneEvent
  {
    private Milestone ms;
    private MilestoneLog logItem;
    private LoanAssociate loanAssociate;

    internal MilestoneEvent(Loan loan, MilestoneLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
      this.ms = loan.Session.Loans.Milestones.GetItemByName(logItem.Stage);
    }

    public override LogEntryType EntryType => LogEntryType.MilestoneEvent;

    public string MilestoneID => this.logItem.MilestoneID;

    public string TPOConnectStatus => this.logItem.TPOConnectStatus;

    public string MilestoneName
    {
      get
      {
        this.EnsureValid();
        return (EnumItem) this.ms != (EnumItem) null ? this.ms.Name : this.logItem.Stage;
      }
    }

    public bool Completed
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Done;
      }
      set
      {
        if (this.logItem.Done == value)
          return;
        this.EnsureEditable();
        if (this.ms.Name == "Started")
          throw new InvalidOperationException("Completed property cannot be modified for this milestone.");
        this.Loan.Unwrap().SetMilestoneStatus(this.logItem.MilestoneID, value);
      }
    }

    public new object Date
    {
      get
      {
        this.EnsureValid();
        return base.Date;
      }
      set
      {
        this.EnsureEditable();
        ((LogRecordBase) this.logItem).Date = value == null ? DateTime.MaxValue : Convert.ToDateTime(value);
      }
    }

    void IMilestoneEvent.SetDate(object newDate) => this.Date = newDate;

    public void AdjustDate(
      object newDate,
      bool allowAdjustPastMilestones,
      bool allowAdjustFutureMilestones)
    {
      this.EnsureEditable();
      this.logItem.AdjustDate(newDate == null ? DateTime.MaxValue : Convert.ToDateTime(newDate), allowAdjustPastMilestones, allowAdjustFutureMilestones);
    }

    public override bool IsAlert
    {
      get
      {
        return this.Date != null && !this.Completed && Convert.ToDateTime(this.Date).Date <= DateTime.Today;
      }
    }

    public LoanAssociate LoanAssociate
    {
      get
      {
        this.EnsureValid();
        if (this.loanAssociate == null)
          this.loanAssociate = this.createLoanAssociate();
        return this.loanAssociate;
      }
    }

    public bool LoanAssociateAccess
    {
      get
      {
        this.EnsureValid();
        return ((LoanAssociateLog) this.logItem).LoanAssociateAccess;
      }
      set
      {
        this.EnsureEditable();
        ((LoanAssociateLog) this.logItem).LoanAssociateAccess = value;
      }
    }

    internal Milestone GetMilestone()
    {
      this.EnsureValid();
      return this.ms;
    }

    private LoanAssociate createLoanAssociate()
    {
      if (((LoanAssociateLog) this.logItem).RoleID < 0)
        return (LoanAssociate) null;
      Role roleById = this.Loan.Session.Loans.Roles.GetRoleByID(((LoanAssociateLog) this.logItem).RoleID);
      return roleById == null ? (LoanAssociate) null : new LoanAssociate(this.Loan, (LoanAssociateLog) this.logItem, roleById);
    }

    internal string InternalName
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Stage;
      }
    }
  }
}
