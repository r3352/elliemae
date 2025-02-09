// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanAssociates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Collections;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for ILoanAssociate class.</summary>
  /// <exclude />
  [Guid("38DF20EE-DFB9-43c5-B86A-2221BADF2AE6")]
  public interface ILoanAssociates
  {
    LoanAssociateList GetAssociatesByRole(Role selectedRole);

    LoanAssociateList GetAssociatesByUser(User associate, bool includeGroups);

    LoanAssociateList GetMilestoneAssociates();

    IEnumerator GetEnumerator();

    LoanAssociateList GetAssociatesByUserGroup(UserGroup associate);

    LoanAssociateList AssignUser(Role selectedRole, User assignedUser);

    LoanAssociateList AssignUserGroup(Role selectedRole, UserGroup assignedGroup);
  }
}
