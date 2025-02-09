// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ContactsSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ContactsSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode contactsNode;
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
    protected TreeNode borrowerPersonalCustomLetterNode;
    protected TreeNode borrowerLoanNode;
    protected TreeNode accessToBizCntNode;
    protected TreeNode newBizCntNode;
    protected TreeNode copyBizCntNode;
    protected TreeNode deleteBizCntNode;
    protected TreeNode mailMergeBizCntNode;
    protected TreeNode importBizCntNode;
    protected TreeNode exportBizCntNode;
    protected TreeNode printBizCntNode;
    protected TreeNode businessPersonalCustomLetterNode;
    protected TreeNode businessLoanNode;
    protected TreeNode campaignAccessNode;
    protected TreeNode campaingAssignTaskToOtherNode;
    protected TreeNode campaignPersonalTemplateNode;
    protected TreeNode synchronizationNode;
    protected TreeNode contactUpdateNode;

    public ContactsSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.All);
    }

    public ContactsSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.All);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.contactsNode = new TreeNode("Access to Contacts Tab");
      this.accessToBorCntNode = new TreeNode("Borrower Contacts");
      this.newBorCntNode = new TreeNode("Create New Contacts");
      this.copyBorCntNode = new TreeNode("Duplicate Contacts");
      this.deleteBorCntNode = new TreeNode("Delete Contacts");
      this.reassignBorCntNode = new TreeNode("Reassign Contacts");
      this.mailMergeBorCntNode = new TreeNode("Mail Merge");
      this.importBorCntNode = new TreeNode("Import Contacts");
      this.exportBorCntNode = new TreeNode("Export Contacts");
      this.borrowerLoanNode = new TreeNode("Access to Loans tab");
      this.printBorCntNode = new TreeNode("Print");
      this.createLoanNode = new TreeNode("Originate Loan/Order Credit/Product and Pricing");
      this.createLoanNode.Tag = (object) AclFeature.Cnt_Borrower_OriginateLoan;
      this.createLoanNode.ForeColor = Color.Blue;
      this.borrowerPersonalCustomLetterNode = new TreeNode("Manage Personal Custom Letter");
      this.accessToBizCntNode = new TreeNode("Business Contacts");
      this.newBizCntNode = new TreeNode("Create New Contacts");
      this.copyBizCntNode = new TreeNode("Duplicate Contacts");
      this.deleteBizCntNode = new TreeNode("Delete Contacts");
      this.mailMergeBizCntNode = new TreeNode("Mail Merge");
      this.importBizCntNode = new TreeNode("Import Contacts");
      this.exportBizCntNode = new TreeNode("Export Contacts");
      this.printBizCntNode = new TreeNode("Print");
      this.businessPersonalCustomLetterNode = new TreeNode("Manage Personal Custom Letter");
      this.businessLoanNode = new TreeNode("Access to Loans tab");
      this.campaignAccessNode = new TreeNode("Campaign Management");
      this.campaingAssignTaskToOtherNode = new TreeNode("Assign Tasks to Others");
      this.campaignPersonalTemplateNode = new TreeNode("Manage Personal Campaign Templates");
      this.synchronizationNode = new TreeNode("Contact Synchronization");
      this.contactUpdateNode = new TreeNode("Contact Update Opt Out");
      this.dependentNodes.Add((object) this.createLoanNode);
      this.accessToBorCntNode.Nodes.AddRange(new TreeNode[11]
      {
        this.newBorCntNode,
        this.copyBorCntNode,
        this.deleteBorCntNode,
        this.reassignBorCntNode,
        this.mailMergeBorCntNode,
        this.importBorCntNode,
        this.exportBorCntNode,
        this.printBorCntNode,
        this.createLoanNode,
        this.borrowerLoanNode,
        this.borrowerPersonalCustomLetterNode
      });
      this.accessToBizCntNode.Nodes.AddRange(new TreeNode[9]
      {
        this.newBizCntNode,
        this.copyBizCntNode,
        this.deleteBizCntNode,
        this.mailMergeBizCntNode,
        this.importBizCntNode,
        this.exportBizCntNode,
        this.printBizCntNode,
        this.businessLoanNode,
        this.businessPersonalCustomLetterNode
      });
      this.campaignAccessNode.Nodes.AddRange(new TreeNode[2]
      {
        this.campaingAssignTaskToOtherNode,
        this.campaignPersonalTemplateNode
      });
      this.contactsNode.Nodes.AddRange(new TreeNode[5]
      {
        this.accessToBorCntNode,
        this.accessToBizCntNode,
        this.campaignAccessNode,
        this.synchronizationNode,
        this.contactUpdateNode
      });
      treeView.Nodes.Add(this.contactsNode);
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
      this.nodeToFeature.Add((object) this.createLoanNode, (object) AclFeature.Cnt_Borrower_OriginateLoan);
      this.nodeToFeature.Add((object) this.borrowerPersonalCustomLetterNode, (object) AclFeature.Borrower_Contacts_Personal_CustomLetters);
      this.nodeToFeature.Add((object) this.borrowerLoanNode, (object) AclFeature.Cnt_Borrower_LoansTab);
      this.nodeToFeature.Add((object) this.accessToBizCntNode, (object) AclFeature.Cnt_Biz_Access);
      this.nodeToFeature.Add((object) this.newBizCntNode, (object) AclFeature.Cnt_Biz_CreateNew);
      this.nodeToFeature.Add((object) this.copyBizCntNode, (object) AclFeature.Cnt_Biz_Copy);
      this.nodeToFeature.Add((object) this.deleteBizCntNode, (object) AclFeature.Cnt_Biz_Delete);
      this.nodeToFeature.Add((object) this.mailMergeBizCntNode, (object) AclFeature.Cnt_Biz_MailMerge);
      this.nodeToFeature.Add((object) this.importBizCntNode, (object) AclFeature.Cnt_Biz_Import);
      this.nodeToFeature.Add((object) this.exportBizCntNode, (object) AclFeature.Cnt_Biz_Export);
      this.nodeToFeature.Add((object) this.printBizCntNode, (object) AclFeature.Cnt_Biz_Print);
      this.nodeToFeature.Add((object) this.businessPersonalCustomLetterNode, (object) AclFeature.Business_Contacts_Personal_CustomLetters);
      this.nodeToFeature.Add((object) this.businessLoanNode, (object) AclFeature.Cnt_Biz_LoansTab);
      this.nodeToFeature.Add((object) this.contactsNode, (object) AclFeature.GlobalTab_Contacts);
      this.nodeToFeature.Add((object) this.campaignAccessNode, (object) AclFeature.Cnt_Campaign_Access);
      this.nodeToFeature.Add((object) this.campaingAssignTaskToOtherNode, (object) AclFeature.Cnt_Campaign_AssignTaskToOther);
      this.nodeToFeature.Add((object) this.campaignPersonalTemplateNode, (object) AclFeature.Cnt_Campaign_PersonalTemplates);
      this.nodeToFeature.Add((object) this.synchronizationNode, (object) AclFeature.Cnt_Synchronization);
      this.nodeToFeature.Add((object) this.contactUpdateNode, (object) AclFeature.Cnt_Contacts_Update);
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
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Borrower_LoansTab, (object) this.borrowerLoanNode);
      this.featureToNodeTbl.Add((object) AclFeature.Borrower_Contacts_Personal_CustomLetters, (object) this.borrowerPersonalCustomLetterNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Access, (object) this.accessToBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_CreateNew, (object) this.newBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Copy, (object) this.copyBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Delete, (object) this.deleteBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_MailMerge, (object) this.mailMergeBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Import, (object) this.importBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Export, (object) this.exportBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_Print, (object) this.printBizCntNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Biz_LoansTab, (object) this.businessLoanNode);
      this.featureToNodeTbl.Add((object) AclFeature.Business_Contacts_Personal_CustomLetters, (object) this.businessPersonalCustomLetterNode);
      this.featureToNodeTbl.Add((object) AclFeature.GlobalTab_Contacts, (object) this.contactsNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Campaign_Access, (object) this.campaignAccessNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Campaign_AssignTaskToOther, (object) this.campaingAssignTaskToOtherNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Campaign_PersonalTemplates, (object) this.campaignPersonalTemplateNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Synchronization, (object) this.synchronizationNode);
      this.featureToNodeTbl.Add((object) AclFeature.Cnt_Contacts_Update, (object) this.contactUpdateNode);
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
      this.nodeToUpdateStatus.Add((object) this.borrowerLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.borrowerPersonalCustomLetterNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessToBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.newBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.copyBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.mailMergeBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.importBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.printBizCntNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.businessLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.businessPersonalCustomLetterNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.contactsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.campaignAccessNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.campaingAssignTaskToOtherNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.campaignPersonalTemplateNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.synchronizationNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.contactUpdateNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
