// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.GeneralFeaturesSecurityHelper
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
  public class GeneralFeaturesSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode imNode;

    public GeneralFeaturesSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.Features);
    }

    public GeneralFeaturesSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.Features);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.imNode = new TreeNode("Instant Messenger");
      treeView.Nodes.AddRange(new TreeNode[1]{ this.imNode });
      treeView.ExpandAll();
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.imNode, (object) AclFeature.InstantMessenger);
      this.featureToNodeTbl = new Hashtable(FeatureSets.PipelineGlobalTabFeatures.Length);
      this.featureToNodeTbl.Add((object) AclFeature.InstantMessenger, (object) this.imNode);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes);
    }
  }
}
