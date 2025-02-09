// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.BatchReassign
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

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
      this.aclMngr = (IFeaturesAclManager) this.Session.GetAclManager((AclCategory) 0);
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
      SDKProgressFeedback progressFeedback = new SDKProgressFeedback(guids.Length);
      for (int index = 0; index < pipeline.Length; ++index)
      {
        progressFeedback.CurrentGUID = pipeline[index].GUID;
        progressFeedback.CurrentLoanNumber = pipeline[index].LoanNumber;
        this.mngr.LoanReassign(index, pipeline[index], user.ID, role.ID, (IServerProgressFeedback) progressFeedback);
      }
      return progressFeedback.Results;
    }
  }
}
