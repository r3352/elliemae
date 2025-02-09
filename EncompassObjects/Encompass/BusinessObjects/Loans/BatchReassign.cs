// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.BatchReassign
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Provides access to reassigning loans.</summary>
  public class BatchReassign : SessionBoundObject
  {
    private ILoanManager mngr;
    private IOrganizationManager orgMngr;
    private IFeaturesAclManager aclMngr;

    internal BatchReassign(Session session)
      : base(session)
    {
      this.mngr = (ILoanManager) session.GetObject("LoanManager");
      this.orgMngr = (IOrganizationManager) session.GetObject("OrganizationManager");
      this.aclMngr = (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features);
    }

    private void CheckParams(User user, Role role)
    {
      if (user == null)
        throw new ArgumentException("User cannot be null.");
      if (role == null)
        throw new ArgumentException("Role cannot be null.");
      if (!((IEnumerable<UserInfo>) this.orgMngr.GetScopedUsersWithRole(role.ID)).Any<UserInfo>((Func<UserInfo, bool>) (u => u.Userid == user.ID)))
        throw new ArgumentException(string.Format("The user {0} does not belong to the role {1}.", (object) user.ID, (object) role.Name));
    }

    /// <summary>
    /// Reassigns a collection of loans to a new User for a given Role
    /// </summary>
    /// <param name="guids">Array of Loan GUIDs to be reassinged.</param>
    /// <param name="user">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User">User</see> that the loans well be assigned to.</param>
    /// <param name="role">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role">Role</see> for which the User will be updated.</param>
    /// <returns>Array of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.ReassignResult">ReassignResult</see> with the results of the reassign process.</returns>
    /// <remarks>This call is done synchronously and will not return until all loans have finished being processed.</remarks>
    public ReassignResult[] Reassign(string[] guids, User user, Role role)
    {
      this.CheckParams(user, role);
      string[] strArray = new string[10]
      {
        "LoanNumber",
        "BorrowerFirstName",
        "BorrowerLastName",
        "Address1",
        "LoanAmount",
        "LoanType",
        "LoanPurpose",
        "CoBorrowerFirstName",
        "CoBorrowerLastName",
        "LoanName"
      };
      PipelineInfo[] pipeline = this.mngr.GetPipeline(guids, false);
      SDKProgressFeedback feedback = new SDKProgressFeedback(guids.Length);
      for (int index = 0; index < pipeline.Length; ++index)
      {
        feedback.CurrentGUID = pipeline[index].GUID;
        feedback.CurrentLoanNumber = pipeline[index].LoanNumber;
        this.mngr.LoanReassign(index, pipeline[index], user.ID, role.ID, (IServerProgressFeedback) feedback);
      }
      return feedback.Results;
    }
  }
}
