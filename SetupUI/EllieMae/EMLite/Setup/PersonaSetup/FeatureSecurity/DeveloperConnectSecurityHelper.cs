// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity.DeveloperConnectSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity
{
  public class DeveloperConnectSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode subscribetoWebhooks;
    protected TreeNode enhancedFieldChange;

    public DeveloperConnectSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.DeveloperConnectFeatures);
    }

    public DeveloperConnectSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.DeveloperConnectFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.subscribetoWebhooks = new TreeNode("Subscribe to Webhooks");
      this.subscribetoWebhooks.Tag = (object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks;
      this.enhancedFieldChange = new TreeNode("Enhanced Field Change");
      this.subscribetoWebhooks.Nodes.AddRange(new TreeNode[1]
      {
        this.enhancedFieldChange
      });
      treeView.Nodes.AddRange(new TreeNode[1]
      {
        this.subscribetoWebhooks
      });
      treeView.ExpandAll();
      this.nodeToFeature = new Hashtable(FeatureSets.DeveloperConnectFeatures.Length);
      this.nodeToFeature.Add((object) this.subscribetoWebhooks, (object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks);
      this.nodeToFeature.Add((object) this.enhancedFieldChange, (object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange);
      this.featureToNodeTbl = new Hashtable(FeatureSets.DeveloperConnectFeatures.Length);
      this.featureToNodeTbl.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks, (object) this.subscribetoWebhooks);
      this.featureToNodeTbl.Add((object) AclFeature.DeveloperConnectTab_SubscribetoWebhooks_EnhancedFieldChange, (object) this.enhancedFieldChange);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.subscribetoWebhooks, (object) false);
      this.nodeToUpdateStatus.Add((object) this.enhancedFieldChange, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
    }
  }
}
