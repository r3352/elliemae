// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DashboardSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DashboardSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode dashboardNode;
    protected TreeNode managePersonalViewNode;
    protected TreeNode managePersonalTemplateNode;
    protected TreeNode filterByOrgNode;
    protected TreeNode filterByUserGroupNode;

    public DashboardSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.DashboardFeatures);
    }

    public DashboardSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.DashboardFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.dashboardNode = new TreeNode("Access to Dashboard Tab");
      this.managePersonalViewNode = new TreeNode("Manage Personal View");
      this.managePersonalTemplateNode = new TreeNode("Manage Personal Snapshot Template");
      this.filterByOrgNode = new TreeNode("Filter Data by Organization*");
      this.filterByUserGroupNode = new TreeNode("Filter Data by User Group*");
      this.dashboardNode.Nodes.AddRange(new TreeNode[4]
      {
        this.managePersonalViewNode,
        this.managePersonalTemplateNode,
        this.filterByOrgNode,
        this.filterByUserGroupNode
      });
      treeView.Nodes.Add(this.dashboardNode);
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.dashboardNode, (object) AclFeature.DashboardTab_Dashboard);
      this.nodeToFeature.Add((object) this.managePersonalViewNode, (object) AclFeature.DashboardTab_ManagePersonalViewTemplate);
      this.nodeToFeature.Add((object) this.managePersonalTemplateNode, (object) AclFeature.DashboardTab_ManagePersonalTemplate);
      this.nodeToFeature.Add((object) this.filterByOrgNode, (object) AclFeature.DashboardTab_Organization);
      this.nodeToFeature.Add((object) this.filterByUserGroupNode, (object) AclFeature.DashboardTab_UserGroup);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclFeature.DashboardTab_Dashboard, (object) this.dashboardNode);
      this.featureToNodeTbl.Add((object) AclFeature.DashboardTab_ManagePersonalViewTemplate, (object) this.managePersonalViewNode);
      this.featureToNodeTbl.Add((object) AclFeature.DashboardTab_ManagePersonalTemplate, (object) this.managePersonalTemplateNode);
      this.featureToNodeTbl.Add((object) AclFeature.DashboardTab_Organization, (object) this.filterByOrgNode);
      this.featureToNodeTbl.Add((object) AclFeature.DashboardTab_UserGroup, (object) this.filterByUserGroupNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.dashboardNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.managePersonalViewNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.managePersonalTemplateNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.filterByOrgNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.filterByUserGroupNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
