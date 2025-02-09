// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity.EVaultSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity
{
  public class EVaultSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode eVaultPortal;
    protected TreeNode transferServicing;
    protected TreeNode reverseRegistration;
    protected TreeNode deactivateENote;
    protected TreeNode reverseDeactivationENote;

    public EVaultSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.eVaultFeatures);
    }

    public EVaultSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.eVaultFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      if (this.session.EncompassEdition != EncompassEdition.Banker)
        return;
      this.eVaultPortal = new TreeNode("eVault Portal");
      this.eVaultPortal.Tag = (object) AclFeature.eVaultTab_eVaultPortal;
      this.transferServicing = new TreeNode("Transfer Servicing");
      this.reverseRegistration = new TreeNode("Reverse Registration");
      this.deactivateENote = new TreeNode("Deactivate an eNote");
      this.reverseDeactivationENote = new TreeNode("Reverse Deactivation of eNote");
      this.eVaultPortal.Nodes.AddRange(new TreeNode[4]
      {
        this.transferServicing,
        this.reverseRegistration,
        this.deactivateENote,
        this.reverseDeactivationENote
      });
      treeView.Nodes.AddRange(new TreeNode[1]
      {
        this.eVaultPortal
      });
      treeView.ExpandAll();
      this.nodeToFeature = new Hashtable(FeatureSets.eVaultFeatures.Length);
      this.nodeToFeature.Add((object) this.eVaultPortal, (object) AclFeature.eVaultTab_eVaultPortal);
      this.nodeToFeature.Add((object) this.transferServicing, (object) AclFeature.eVaultTab_TransferServicing);
      this.nodeToFeature.Add((object) this.reverseRegistration, (object) AclFeature.eVaultTab_ReverseRegistration);
      this.nodeToFeature.Add((object) this.deactivateENote, (object) AclFeature.eVaultTab_DeactivateAnENote);
      this.nodeToFeature.Add((object) this.reverseDeactivationENote, (object) AclFeature.eVaultTab_ReverseDeactivationOfENote);
      this.featureToNodeTbl = new Hashtable(FeatureSets.eVaultFeatures.Length);
      this.featureToNodeTbl.Add((object) AclFeature.eVaultTab_eVaultPortal, (object) this.eVaultPortal);
      this.featureToNodeTbl.Add((object) AclFeature.eVaultTab_TransferServicing, (object) this.transferServicing);
      this.featureToNodeTbl.Add((object) AclFeature.eVaultTab_ReverseRegistration, (object) this.reverseRegistration);
      this.featureToNodeTbl.Add((object) AclFeature.eVaultTab_DeactivateAnENote, (object) this.deactivateENote);
      this.featureToNodeTbl.Add((object) AclFeature.eVaultTab_ReverseDeactivationOfENote, (object) this.reverseDeactivationENote);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.eVaultPortal, (object) false);
      this.nodeToUpdateStatus.Add((object) this.transferServicing, (object) false);
      this.nodeToUpdateStatus.Add((object) this.reverseRegistration, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deactivateENote, (object) false);
      this.nodeToUpdateStatus.Add((object) this.reverseDeactivationENote, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
    }
  }
}
