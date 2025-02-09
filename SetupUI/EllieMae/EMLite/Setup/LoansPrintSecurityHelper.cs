// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoansPrintSecurityHelper
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
  public class LoansPrintSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode accessToPrintNode;
    protected TreeNode standardFormsNode;
    protected TreeNode customFormsNode;
    protected TreeNode previewNode;
    protected TreeNode toPDFNode;

    public LoansPrintSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.LoansPrintFeatures);
    }

    public LoansPrintSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.LoansPrintFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.accessToPrintNode = new TreeNode("Print Button");
      this.standardFormsNode = new TreeNode("Standard Forms Tab*");
      this.customFormsNode = new TreeNode("Custom Forms Tab*");
      this.previewNode = new TreeNode("Preview");
      this.toPDFNode = new TreeNode("Print to File");
      this.accessToPrintNode.Nodes.AddRange(new TreeNode[4]
      {
        this.standardFormsNode,
        this.customFormsNode,
        this.previewNode,
        this.toPDFNode
      });
      treeView.Nodes.AddRange(new TreeNode[1]
      {
        this.accessToPrintNode
      });
      treeView.ExpandAll();
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.accessToPrintNode, (object) AclFeature.LoansTab_Print_PrintButton);
      this.nodeToFeature.Add((object) this.standardFormsNode, (object) AclFeature.LoansTab_Print_StandardForms);
      this.nodeToFeature.Add((object) this.customFormsNode, (object) AclFeature.LoansTab_Print_CustomForms);
      this.nodeToFeature.Add((object) this.previewNode, (object) AclFeature.LoansTab_Print_Preview);
      this.nodeToFeature.Add((object) this.toPDFNode, (object) AclFeature.LoansTab_Print_ToPDF);
      this.featureToNodeTbl = new Hashtable(FeatureSets.LoansPrintFeatures.Length);
      this.featureToNodeTbl.Add((object) AclFeature.LoansTab_Print_PrintButton, (object) this.accessToPrintNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoansTab_Print_StandardForms, (object) this.standardFormsNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoansTab_Print_CustomForms, (object) this.customFormsNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoansTab_Print_Preview, (object) this.previewNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoansTab_Print_ToPDF, (object) this.toPDFNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.accessToPrintNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.standardFormsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.customFormsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.previewNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.toPDFNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
    }
  }
}
