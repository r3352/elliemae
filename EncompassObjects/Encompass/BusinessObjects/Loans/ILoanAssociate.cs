// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanAssociate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.BusinessObjects.Users;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for ILoanAssociate class.</summary>
  /// <exclude />
  [Guid("CD10DA42-DC90-432f-8D30-7B98A1D036A3")]
  public interface ILoanAssociate
  {
    User User { get; }

    string ContactEmail { get; set; }

    string ContactPhone { get; set; }

    MilestoneEvent MilestoneEvent { get; }

    Role WorkflowRole { get; }

    bool AllowWriteAccess { get; set; }

    string ContactFax { get; set; }

    void AssignUser(User associateUser);

    string ContactName { get; }

    string ContactCellPhone { get; set; }

    LoanAssociateType AssociateType { get; }

    UserGroup UserGroup { get; }

    void AssignUserGroup(UserGroup associateGroup);

    void Unassign();

    bool Assigned { get; }
  }
}
