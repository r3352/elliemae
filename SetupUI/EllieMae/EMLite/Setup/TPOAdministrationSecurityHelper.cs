// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TPOAdministrationSecurityHelper
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
  public class TPOAdministrationSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode accessTPOAdminSite;

    public TPOAdministrationSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.TPOAdministrationTabFeatures);
    }

    public TPOAdministrationSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.TPOAdministrationTabFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.accessTPOAdminSite = new TreeNode("Access TPO Connect Admin Site");
      treeView.Nodes.AddRange(new TreeNode[1]
      {
        this.accessTPOAdminSite
      });
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.accessTPOAdminSite, (object) AclFeature.TPOAdministrationTab_AccessTPOAdminSite);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessTPOAdminSite, (object) this.accessTPOAdminSite);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.accessTPOAdminSite, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
