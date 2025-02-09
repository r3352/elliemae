// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.BusinessObjects.Users;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanAssociate : ILoanAssociate
  {
    private Loan loan;
    private LoanAssociateLog logItem;
    private Role role;
    private User user;
    private UserGroup userGroup;
    private MilestoneEvent msEvent;

    internal LoanAssociate(Loan loan, LoanAssociateLog logItem, Role role)
    {
      this.loan = loan;
      this.logItem = logItem;
      this.role = role;
    }

    public LoanAssociateType AssociateType => (LoanAssociateType) this.logItem.LoanAssociateType;

    public User User
    {
      get
      {
        if (this.AssociateType != LoanAssociateType.User || (this.logItem.LoanAssociateID ?? "") == "")
          return (User) null;
        if (this.user == null || this.user.ID != this.logItem.LoanAssociateID)
          this.user = this.loan.Session.Users.GetUser(this.logItem.LoanAssociateID);
        return this.user;
      }
      set
      {
        if (value == null && this.AssociateType == LoanAssociateType.User)
        {
          this.loan.Unwrap().ClearLoanAssociate(this.logItem);
        }
        else
        {
          if (value == null)
            return;
          this.loan.Unwrap().AssignLoanAssociate(this.logItem, value.Unwrap());
        }
      }
    }

    public UserGroup UserGroup
    {
      get
      {
        if (this.AssociateType != LoanAssociateType.UserGroup || (this.logItem.LoanAssociateID ?? "") == "")
          return (UserGroup) null;
        if (this.userGroup == null || this.userGroup.ID.ToString() != this.logItem.LoanAssociateID)
          this.userGroup = this.loan.Session.Users.Groups.GetGroupByID(Utils.ParseInt((object) this.logItem.LoanAssociateID, false, -1));
        return this.userGroup;
      }
      set
      {
        if (value == null && this.AssociateType == LoanAssociateType.UserGroup)
        {
          this.loan.Unwrap().ClearLoanAssociate(this.logItem);
        }
        else
        {
          if (value == null)
            return;
          this.loan.Unwrap().AssignLoanAssociate(this.logItem, value.Unwrap());
        }
      }
    }

    public string ContactName => this.logItem.LoanAssociateName ?? "";

    public string ContactEmail
    {
      get => this.logItem.LoanAssociateEmail ?? "";
      set => this.logItem.LoanAssociateEmail = value ?? "";
    }

    public string ContactPhone
    {
      get => this.logItem.LoanAssociatePhone ?? "";
      set => this.logItem.LoanAssociatePhone = value ?? "";
    }

    public string ContactFax
    {
      get => this.logItem.LoanAssociateFax ?? "";
      set => this.logItem.LoanAssociateFax = value ?? "";
    }

    public string ContactCellPhone
    {
      get => this.logItem.LoanAssociateCellPhone ?? "";
      set => this.logItem.LoanAssociateCellPhone = value ?? "";
    }

    public MilestoneEvent MilestoneEvent
    {
      get
      {
        if (this.msEvent == null && this.logItem is MilestoneLog)
          this.msEvent = (MilestoneEvent) this.loan.Log.MilestoneEvents.Find((LogRecordBase) this.logItem, false);
        return this.msEvent;
      }
    }

    public Role WorkflowRole => this.role;

    public bool AllowWriteAccess
    {
      get => this.logItem.LoanAssociateAccess;
      set => this.logItem.LoanAssociateAccess = value;
    }

    public void AssignUser(User associateUser) => this.User = associateUser;

    public void AssignUserGroup(UserGroup associateGroup) => this.UserGroup = associateGroup;

    public void Unassign()
    {
      if (this.logItem.LoanAssociateType == null)
        return;
      this.loan.Unwrap().ClearLoanAssociate(this.logItem);
    }

    public bool Assigned => this.logItem.LoanAssociateType > 0;

    public override bool Equals(object obj)
    {
      return obj is LoanAssociate loanAssociate && loanAssociate.logItem.LoanAssociateID == this.logItem.LoanAssociateID && this.role.ID == loanAssociate.role.ID;
    }

    public override int GetHashCode() => this.logItem.LoanAssociateID.GetHashCode();
  }
}
