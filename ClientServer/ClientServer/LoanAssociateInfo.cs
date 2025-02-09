// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanAssociateInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanAssociateInfo
  {
    private string associateGuid;
    private string loanGuid;
    private LoanAssociateType associateType;
    private string milestoneId;
    private string milestoneName;
    private int milestoneOrder;
    private int roleId;
    private string roleName;
    private string roleAbbr;
    private bool allowWrites;
    private string userId;
    private int groupId;
    private string associateName;
    private string userFirstName;
    private string userLastName;
    private string userEmail;
    private string userPhone;

    public LoanAssociateInfo(
      string associateGuid,
      string loanGuid,
      LoanAssociateType associateType,
      string milestoneId,
      string milestoneName,
      int milestoneOrder,
      int roleId,
      string roleName,
      string roleAbbr,
      bool allowWrites,
      string userId,
      int groupId,
      string associateName,
      string userFirstName,
      string userLastName,
      string userEmail,
      string userPhone)
    {
      this.associateGuid = associateGuid;
      this.loanGuid = loanGuid;
      this.associateType = associateType;
      this.milestoneId = milestoneId;
      this.milestoneName = milestoneName;
      this.milestoneOrder = milestoneOrder;
      this.roleId = roleId;
      this.roleName = roleName;
      this.roleAbbr = roleAbbr;
      this.allowWrites = allowWrites;
      this.groupId = groupId;
      this.userId = userId;
      this.associateName = associateName;
      this.userFirstName = userFirstName;
      this.userLastName = userLastName;
      this.userEmail = userEmail;
      this.userPhone = userPhone;
    }

    public string AssociateGuid => this.associateGuid;

    public string LoanGuid => this.loanGuid;

    public LoanAssociateType AssociateType => this.associateType;

    public string MilestoneID => this.milestoneId;

    public string MilestoneName => this.milestoneName;

    public int MilestoneOrder => this.milestoneOrder;

    public int RoleID => this.roleId;

    public string RoleName => this.roleName;

    public string RoleAbbr => this.roleAbbr;

    public bool AllowWrites => this.allowWrites;

    public string AssociateUserID => this.userId;

    public int AssociateGroupID => this.groupId;

    public string AssociateUserFirstName => this.userFirstName;

    public string AssociateUserLastName => this.userLastName;

    public string LoanAssociateName => this.associateName;

    public string AssociateUserEmail => this.userEmail;

    public string AssociateUserPhone => this.userPhone;

    public string AssociateUserFax => string.Empty;
  }
}
