// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ContactsOriginateLoanSecurityHelper
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
  public class ContactsOriginateLoanSecurityHelper : FeatureSecurityHelperBase
  {
    private TreeNode createLoanNode;
    private TreeNode createBlankLoanNode;
    private TreeNode createLoanFromTempNode;
    private TreeNode orderCreditNode;
    private TreeNode productAndPricingNode;
    private const string CreateLoanNodePath = "Originate Loan";
    private const string CreateBlankLoanNodePath = "Originate Loan\\New Blank Loan";
    private const string CreateLoanFromTempNodePath = "Originate Loan\\New from Template";
    private const string OrderCreditNodePath = "Order Credit";
    private const string ProductAndPricingNodePath = "Product and Pricing";

    public ContactsOriginateLoanSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.BorContacts);
    }

    public ContactsOriginateLoanSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.BorContacts);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.createLoanNode = new TreeNode("Originate Loan");
      this.createBlankLoanNode = new TreeNode("New Blank Loan");
      this.createLoanFromTempNode = new TreeNode("New from Template");
      this.orderCreditNode = new TreeNode("Order Credit");
      this.productAndPricingNode = new TreeNode("Product and Pricing");
      this.createLoanNode.Nodes.AddRange(new TreeNode[2]
      {
        this.createBlankLoanNode,
        this.createLoanFromTempNode
      });
      treeView.Nodes.AddRange(new TreeNode[3]
      {
        this.createLoanNode,
        this.orderCreditNode,
        this.productAndPricingNode
      });
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.createBlankLoanNode, (object) AclFeature.Cnt_Borrower_CreatBlankLoan);
      this.nodeToFeature.Add((object) this.createLoanFromTempNode, (object) AclFeature.Cnt_Borrower_CreatLoanFrmTemplate);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_CreatBlankLoan, (object) this.createBlankLoanNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_CreatLoanFrmTemplate, (object) this.createLoanFromTempNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_OrderCredit, (object) this.orderCreditNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_ProductAndPricing, (object) this.productAndPricingNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.createBlankLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.createLoanFromTempNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orderCreditNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.productAndPricingNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }

    public override void SetNodeStates()
    {
      base.SetNodeStates();
      this.createLoanNode.Checked = this.createBlankLoanNode.Checked && this.createLoanFromTempNode.Checked;
    }

    public TreeNode GetCreateLoanNode(TreeView treeView)
    {
      return this.FindTreeNode(treeView, "Originate Loan");
    }

    public TreeNode GetCreateBlankLoanNode(TreeView treeView)
    {
      return this.FindTreeNode(treeView, "Originate Loan\\New Blank Loan");
    }

    public TreeNode GetCreateLoanFromTempNode(TreeView treeView)
    {
      return this.FindTreeNode(treeView, "Originate Loan\\New from Template");
    }

    public TreeNode GetOrderCreditNode(TreeView treeView)
    {
      return this.FindTreeNode(treeView, "Order Credit");
    }

    public TreeNode GetProductAndPricingNode(TreeView treeView)
    {
      return this.FindTreeNode(treeView, "Product and Pricing");
    }

    private TreeNode FindTreeNode(TreeView treeView, string treeNodePath)
    {
      foreach (TreeNode node in treeView.Nodes)
      {
        TreeNode treeNode = this.FindTreeNode(node, treeNodePath);
        if (treeNode != null)
          return treeNode;
      }
      return (TreeNode) null;
    }

    private TreeNode FindTreeNode(TreeNode node, string treeNodePath)
    {
      if (node.FullPath == treeNodePath)
        return node;
      foreach (TreeNode node1 in node.Nodes)
      {
        TreeNode treeNode = this.FindTreeNode(node1, treeNodePath);
        if (treeNode != null)
          return treeNode;
      }
      return (TreeNode) null;
    }
  }
}
