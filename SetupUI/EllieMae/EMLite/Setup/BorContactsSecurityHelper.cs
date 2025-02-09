// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BorContactsSecurityHelper
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
  public class BorContactsSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode accessToBorCntNode;
    protected TreeNode newBorCntNode;
    protected TreeNode copyBorCntNode;
    protected TreeNode deleteBorCntNode;
    protected TreeNode reassignBorCntNode;
    protected TreeNode mailMergeBorCntNode;
    protected TreeNode importBorCntNode;
    protected TreeNode exportBorCntNode;
    protected TreeNode printBorCntNode;
    protected TreeNode createLoanNode;
    protected TreeNode createBlankLoanNode;
    protected TreeNode createLoanFromTempNode;

    public BorContactsSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.BorContacts);
    }

    public BorContactsSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.BorContacts);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.newBorCntNode = new TreeNode("Create New Contacts");
      this.copyBorCntNode = new TreeNode("Duplicate Contacts");
      this.deleteBorCntNode = new TreeNode("Delete Contacts");
      this.reassignBorCntNode = new TreeNode("Reassign Contacts");
      this.mailMergeBorCntNode = new TreeNode("Mail Merge");
      this.importBorCntNode = new TreeNode("Import Contacts");
      this.exportBorCntNode = new TreeNode("Export Contacts");
      this.printBorCntNode = new TreeNode("Print");
      this.createLoanNode = new TreeNode("Originate Loan/Order Credit/Product and Pricing");
      this.createBlankLoanNode = new TreeNode("New Blank Loan");
      this.createLoanFromTempNode = new TreeNode("New from Template");
      this.createLoanNode.Nodes.AddRange(new TreeNode[2]
      {
        this.createBlankLoanNode,
        this.createLoanFromTempNode
      });
      this.accessToBorCntNode = new TreeNode("Access to Borrower Contacts");
      this.accessToBorCntNode.Nodes.AddRange(new TreeNode[9]
      {
        this.newBorCntNode,
        this.copyBorCntNode,
        this.deleteBorCntNode,
        this.reassignBorCntNode,
        this.mailMergeBorCntNode,
        this.importBorCntNode,
        this.exportBorCntNode,
        this.printBorCntNode,
        this.createLoanNode
      });
      this.dependentNodes.Add((object) this.createLoanNode);
      treeView.Nodes.AddRange(new TreeNode[1]
      {
        this.accessToBorCntNode
      });
      treeView.ExpandAll();
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.accessToBorCntNode, (object) AclFeature.Cnt_Borrower_Access);
      this.nodeToFeature.Add((object) this.newBorCntNode, (object) AclFeature.Cnt_Borrower_CreateNew);
      this.nodeToFeature.Add((object) this.copyBorCntNode, (object) AclFeature.Cnt_Borrower_Copy);
      this.nodeToFeature.Add((object) this.deleteBorCntNode, (object) AclFeature.Cnt_Borrower_Delete);
      this.nodeToFeature.Add((object) this.reassignBorCntNode, (object) AclFeature.Cnt_Borrower_Reassign);
      this.nodeToFeature.Add((object) this.mailMergeBorCntNode, (object) AclFeature.Cnt_Borrower_MailMerge);
      this.nodeToFeature.Add((object) this.importBorCntNode, (object) AclFeature.Cnt_Borrower_Import);
      this.nodeToFeature.Add((object) this.exportBorCntNode, (object) AclFeature.Cnt_Borrower_Export);
      this.nodeToFeature.Add((object) this.printBorCntNode, (object) AclFeature.Cnt_Borrower_Print);
      this.nodeToFeature.Add((object) this.createBlankLoanNode, (object) AclFeature.Cnt_Borrower_CreatBlankLoan);
      this.nodeToFeature.Add((object) this.createLoanFromTempNode, (object) AclFeature.Cnt_Borrower_CreatLoanFrmTemplate);
      this.featureToNodeTbl = new Hashtable(FeatureSets.Contacts.Length);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_Access, (object) this.accessToBorCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_CreateNew, (object) this.newBorCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_Copy, (object) this.copyBorCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_Delete, (object) this.deleteBorCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_Reassign, (object) this.reassignBorCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_MailMerge, (object) this.mailMergeBorCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_Import, (object) this.importBorCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_Export, (object) this.exportBorCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_Print, (object) this.printBorCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_CreatBlankLoan, (object) this.createBlankLoanNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_CreatLoanFrmTemplate, (object) this.createLoanFromTempNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.accessToBorCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.newBorCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.copyBorCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteBorCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.reassignBorCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.mailMergeBorCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.importBorCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportBorCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.printBorCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.createLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.createBlankLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.createLoanFromTempNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
    }
  }
}
