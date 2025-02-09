// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CCAdminSecurityHelper
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
  public class CCAdminSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode accessCCAdmin;
    protected TreeNode accessCCContributor;
    protected TreeNode accessCCAssistant;

    public CCAdminSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.ConsumerConnectTabFeatures);
    }

    public CCAdminSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.ConsumerConnectTabFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.accessCCAdmin = new TreeNode("WebAdmin");
      this.accessCCContributor = new TreeNode("WebContent");
      this.accessCCAssistant = new TreeNode("WebAssistant");
      treeView.Nodes.AddRange(new TreeNode[3]
      {
        this.accessCCAdmin,
        this.accessCCContributor,
        this.accessCCAssistant
      });
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.accessCCAdmin, (object) AclFeature.ConsumerConnect_Admin);
      this.nodeToFeature.Add((object) this.accessCCContributor, (object) AclFeature.ConsumerConnect_Contributor);
      this.nodeToFeature.Add((object) this.accessCCAssistant, (object) AclFeature.ConsumerConnect_Assistant);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclFeature.ConsumerConnect_Admin, (object) this.accessCCAdmin);
      this.featureToNodeTbl.Add((object) AclFeature.ConsumerConnect_Contributor, (object) this.accessCCContributor);
      this.featureToNodeTbl.Add((object) AclFeature.ConsumerConnect_Assistant, (object) this.accessCCAssistant);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.accessCCAdmin, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessCCContributor, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessCCAssistant, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
