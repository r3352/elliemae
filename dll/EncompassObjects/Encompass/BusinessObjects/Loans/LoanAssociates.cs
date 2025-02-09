// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanAssociates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanAssociates : ILoanAssociates, IEnumerable
  {
    private Loan loan;

    internal LoanAssociates(Loan loan) => this.loan = loan;

    public LoanAssociateList GetAssociatesByRole(Role selectedRole)
    {
      if (selectedRole == null)
        throw new ArgumentNullException("Specified role cannot be null");
      LoanAssociateList loanAssociates = this.getLoanAssociates();
      LoanAssociateList associatesByRole = new LoanAssociateList();
      foreach (LoanAssociate loanAssociate in (CollectionBase) loanAssociates)
      {
        if (loanAssociate.WorkflowRole.Equals((object) selectedRole))
          associatesByRole.Add(loanAssociate);
      }
      return associatesByRole;
    }

    public LoanAssociateList GetAssociatesByRole(FixedRole selectedRole)
    {
      Role fixedRole = this.loan.Session.Loans.Roles.GetFixedRole(selectedRole);
      return fixedRole == null ? new LoanAssociateList() : this.GetAssociatesByRole(fixedRole);
    }

    public LoanAssociateList GetAssociatesByUser(User associate)
    {
      return this.GetAssociatesByUser(associate, false);
    }

    public LoanAssociateList GetAssociatesByUser(User associate, bool includeGroups)
    {
      LoanAssociateList loanAssociates = this.getLoanAssociates();
      LoanAssociateList associatesByUser = new LoanAssociateList();
      UserGroupList userGroupList = (UserGroupList) null;
      if (includeGroups)
        userGroupList = new UserGroupList((IList) associate.GetUserGroups());
      foreach (LoanAssociate loanAssociate in (CollectionBase) loanAssociates)
      {
        if (loanAssociate.User != null && loanAssociate.User.Equals((object) associate))
          associatesByUser.Add(loanAssociate);
        else if (includeGroups && loanAssociate.UserGroup != null && userGroupList.Contains(loanAssociate.UserGroup))
          associatesByUser.Add(loanAssociate);
      }
      return associatesByUser;
    }

    public LoanAssociateList GetAssociatesByUserGroup(UserGroup associate)
    {
      LoanAssociateList loanAssociates = this.getLoanAssociates();
      LoanAssociateList associatesByUserGroup = new LoanAssociateList();
      foreach (LoanAssociate loanAssociate in (CollectionBase) loanAssociates)
      {
        if (loanAssociate.UserGroup != null && loanAssociate.UserGroup.Equals((object) associate))
          associatesByUserGroup.Add(loanAssociate);
      }
      return associatesByUserGroup;
    }

    public LoanAssociateList GetMilestoneAssociates()
    {
      LoanAssociateList loanAssociates = this.getLoanAssociates();
      LoanAssociateList milestoneAssociates = new LoanAssociateList();
      foreach (LoanAssociate loanAssociate in (CollectionBase) loanAssociates)
      {
        if (loanAssociate.MilestoneEvent != null)
          milestoneAssociates.Add(loanAssociate);
      }
      return milestoneAssociates;
    }

    public LoanAssociateList AssignUser(Role roleToAssign, User assignedUser)
    {
      LoanAssociateList associatesByRole = this.GetAssociatesByRole(roleToAssign);
      foreach (LoanAssociate loanAssociate in (CollectionBase) associatesByRole)
        loanAssociate.AssignUser(assignedUser);
      return associatesByRole;
    }

    public LoanAssociateList AssignUser(FixedRole roleToAssign, User assignedUser)
    {
      Role fixedRole = this.loan.Session.Loans.Roles.GetFixedRole(roleToAssign);
      return fixedRole == null ? new LoanAssociateList() : this.AssignUser(fixedRole, assignedUser);
    }

    public LoanAssociateList AssignUserGroup(Role roleToAssign, UserGroup assignedGroup)
    {
      LoanAssociateList associatesByRole = this.GetAssociatesByRole(roleToAssign);
      foreach (LoanAssociate loanAssociate in (CollectionBase) associatesByRole)
        loanAssociate.AssignUserGroup(assignedGroup);
      return associatesByRole;
    }

    public LoanAssociateList AssignUserGroup(FixedRole roleToAssign, UserGroup assignedGroup)
    {
      Role fixedRole = this.loan.Session.Loans.Roles.GetFixedRole(roleToAssign);
      return fixedRole == null ? new LoanAssociateList() : this.AssignUserGroup(fixedRole, assignedGroup);
    }

    public IEnumerator GetEnumerator() => this.getLoanAssociates().GetEnumerator();

    private LoanAssociateList getLoanAssociates()
    {
      LoanAssociateList loanAssociates = new LoanAssociateList();
      Hashtable hashtable = new Hashtable();
      LogList logList = this.loan.Unwrap().LoanData.GetLogList();
      foreach (MilestoneEvent milestoneEvent in (LoanLogEntryCollection) this.loan.Log.MilestoneEvents)
      {
        LoanAssociate loanAssociate = milestoneEvent.LoanAssociate;
        if (loanAssociate != null)
        {
          loanAssociates.Add(loanAssociate);
          hashtable[(object) loanAssociate.WorkflowRole.ID] = (object) true;
        }
      }
      foreach (MilestoneFreeRoleLog milestoneFreeRole in logList.GetAllMilestoneFreeRoles())
      {
        Role roleById = this.loan.Session.Loans.Roles.GetRoleByID(((LoanAssociateLog) milestoneFreeRole).RoleID);
        if (roleById != null && !hashtable.Contains((object) roleById.ID))
        {
          loanAssociates.Add(new LoanAssociate(this.loan, (LoanAssociateLog) milestoneFreeRole, roleById));
          hashtable[(object) roleById.ID] = (object) true;
        }
      }
      foreach (Role role in this.loan.Session.Loans.Roles)
      {
        if (!hashtable.Contains((object) role.ID))
        {
          MilestoneFreeRoleLog logItem = new MilestoneFreeRoleLog();
          ((LoanAssociateLog) logItem).RoleID = role.ID;
          ((LoanAssociateLog) logItem).RoleName = role.Name;
          loanAssociates.Add(new LoanAssociate(this.loan, (LoanAssociateLog) logItem, role));
          hashtable[(object) role.ID] = (object) true;
          logList.AddRecord((LogRecordBase) logItem);
        }
      }
      return loanAssociates;
    }
  }
}
