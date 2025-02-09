// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MilestoneHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MilestoneHelper
  {
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private MilestonesAclManager aclMgr;
    private Color backColor = Color.White;

    public MilestoneHelper(Sessions.Session session)
    {
      this.session = session;
      this.aclMgr = (MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones);
    }

    public bool GetPermission(AclMilestone feature, int personaId, EllieMae.EMLite.Workflow.Milestone milestone)
    {
      return this.aclMgr.GetPermission(feature, milestone.MilestoneID, personaId);
    }

    public bool GetPermission(AclMilestone feature, string userId, EllieMae.EMLite.Workflow.Milestone milestone)
    {
      bool permission = false;
      switch ((AclTriState) this.aclMgr.GetPermission(feature, milestone.MilestoneID, userId))
      {
        case AclTriState.False:
          permission = false;
          break;
        case AclTriState.True:
          permission = true;
          break;
        default:
          foreach (Persona userPersona in this.session.OrganizationManager.GetUser(userId).UserPersonas)
          {
            permission = this.aclMgr.GetPermission(feature, milestone.MilestoneID, userPersona.ID);
            if (permission)
              break;
          }
          break;
      }
      return permission;
    }

    public Hashtable GetPermissionFromUser(
      AclMilestone feature,
      string userId,
      EllieMae.EMLite.Workflow.Milestone milestone)
    {
      Hashtable permissionFromUser = new Hashtable();
      switch ((AclTriState) this.aclMgr.GetPermission(feature, milestone.MilestoneID, userId))
      {
        case AclTriState.False:
          permissionFromUser.Add((object) "Source", (object) "USER");
          permissionFromUser.Add((object) "Result", (object) false);
          break;
        case AclTriState.True:
          permissionFromUser.Add((object) "Source", (object) "USER");
          permissionFromUser.Add((object) "Result", (object) true);
          break;
        default:
          Persona[] userPersonas = this.session.OrganizationManager.GetUser(userId).UserPersonas;
          permissionFromUser.Add((object) "Source", (object) "PERSONA");
          permissionFromUser.Add((object) "Result", (object) false);
          for (int index = 0; index < userPersonas.Length; ++index)
          {
            if (this.aclMgr.GetPermission(feature, milestone.MilestoneID, userPersonas[index].ID))
            {
              permissionFromUser[(object) "Result"] = (object) true;
              break;
            }
          }
          break;
      }
      return permissionFromUser;
    }

    public bool GetPermissionFromPersonas(AclMilestone feature, string userId, EllieMae.EMLite.Workflow.Milestone milestone)
    {
      bool permissionFromPersonas = false;
      foreach (Persona userPersona in this.session.OrganizationManager.GetUser(userId).UserPersonas)
      {
        if (this.aclMgr.GetPermission(feature, milestone.MilestoneID, userPersona.ID))
        {
          permissionFromPersonas = true;
          break;
        }
      }
      return permissionFromPersonas;
    }

    public Color GetForeColor(bool isLoanAssociate) => isLoanAssociate ? Color.Black : Color.White;

    public Color GetBackColor(bool isLoanAssociate, EllieMae.EMLite.Workflow.Milestone milestone)
    {
      return isLoanAssociate ? Color.White : this.backColor;
    }

    public void SetPermission(
      AclMilestone feature,
      int personaId,
      bool access,
      EllieMae.EMLite.Workflow.Milestone milestone)
    {
      this.aclMgr.SetPermission(feature, milestone.MilestoneID, personaId, access);
    }

    public void SetPermission(
      AclMilestone feature,
      string userId,
      object access,
      EllieMae.EMLite.Workflow.Milestone milestone)
    {
      this.aclMgr.SetPermission(feature, milestone.MilestoneID, userId, access);
    }
  }
}
