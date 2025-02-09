// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TPOSiteSettingsSecurityHelper
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
  public class TPOSiteSettingsSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode accessToWholesaleNode;
    protected TreeNode registerWholesaleLoanNode;
    protected TreeNode accessToCorrespondentNode;
    protected TreeNode registerCorrespondentLoanNode;
    protected TreeNode accessToManageAccountNode;
    protected TreeNode accessCompanyInfoNode;
    protected TreeNode accessBranchesNode;
    protected TreeNode accessPersonalEditMyInformationNode;
    protected TreeNode accessPersonalEditMyLicenseNode;
    protected TreeNode accessPersonalEditMyNotificationNode;
    protected TreeNode generalNode;
    protected TreeNode accessToScenariosNode;
    protected TreeNode pipelinesAndLoansNode;
    protected TreeNode viewMessagesNode;
    protected TreeNode accessToFeesNode;
    protected TreeNode acceptFeesNode;
    protected TreeNode accessToProductAndPricingNode;
    protected TreeNode generateDisclosuresNode;
    protected TreeNode viewLockHistoryNode;
    protected TreeNode submitForPurchase;
    protected TreeNode viewPurchaseAdvice;
    protected TreeNode accessToTPOTradeMgmtNode;
    protected TreeNode viewCorrespondentTrades;
    protected TreeNode viewCorrespondentMasterCmts;
    protected TreeNode updateCorrespondentTrades;
    protected TreeNode viewCmtConfirmations;

    public TPOSiteSettingsSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.TPOSiteSettingsTabFeatures);
    }

    public TPOSiteSettingsSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.TPOSiteSettingsTabFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.accessToWholesaleNode = new TreeNode("Access Wholesale");
      this.registerWholesaleLoanNode = new TreeNode("Register Wholesale Loan");
      this.accessToCorrespondentNode = new TreeNode("Access Correspondent");
      this.registerCorrespondentLoanNode = new TreeNode("Register Correspondent Loan");
      this.accessToManageAccountNode = new TreeNode("Access to Manage Account Page");
      this.accessCompanyInfoNode = new TreeNode("Access Company Info Tab");
      this.accessBranchesNode = new TreeNode("Access Branches Tab");
      this.accessPersonalEditMyInformationNode = new TreeNode("Personal Account - Edit \"My Information\"");
      this.accessPersonalEditMyLicenseNode = new TreeNode("Personal Account - Edit \"My Licenses\"");
      this.accessPersonalEditMyNotificationNode = new TreeNode("Personal Account - Edit \"My Notifications\"");
      this.generalNode = new TreeNode("General");
      this.accessToScenariosNode = new TreeNode("Access to Scenarios");
      this.pipelinesAndLoansNode = new TreeNode("Pipeline and Loans");
      this.viewMessagesNode = new TreeNode("View Messages");
      this.accessToFeesNode = new TreeNode("Access to Fees");
      this.acceptFeesNode = new TreeNode("Accept Fees");
      this.generateDisclosuresNode = new TreeNode("Generate Disclosures");
      this.accessToProductAndPricingNode = new TreeNode("Access to Product & Pricing");
      this.viewLockHistoryNode = new TreeNode("View Lock History");
      this.submitForPurchase = new TreeNode("Submit For Purchase");
      this.viewPurchaseAdvice = new TreeNode("View Purchase Advice");
      this.accessToTPOTradeMgmtNode = new TreeNode("Access to TPO Trade Management");
      this.viewCorrespondentTrades = new TreeNode("View Correspondent Trades");
      this.viewCorrespondentMasterCmts = new TreeNode("View Correspondent Master Commitments");
      this.updateCorrespondentTrades = new TreeNode("Update Correspondent Trades");
      this.viewCmtConfirmations = new TreeNode("View Commitment Confirmations");
      this.accessToManageAccountNode.Nodes.AddRange(new TreeNode[5]
      {
        this.accessCompanyInfoNode,
        this.accessBranchesNode,
        this.accessPersonalEditMyInformationNode,
        this.accessPersonalEditMyLicenseNode,
        this.accessPersonalEditMyNotificationNode
      });
      this.generalNode.Nodes.AddRange(new TreeNode[4]
      {
        this.accessToWholesaleNode,
        this.accessToCorrespondentNode,
        this.accessToManageAccountNode,
        this.accessToScenariosNode
      });
      this.accessToWholesaleNode.Nodes.Add(this.registerWholesaleLoanNode);
      this.accessToCorrespondentNode.Nodes.Add(this.registerCorrespondentLoanNode);
      this.accessToFeesNode.Nodes.Add(this.acceptFeesNode);
      this.accessToProductAndPricingNode.Nodes.Add(this.viewLockHistoryNode);
      this.pipelinesAndLoansNode.Nodes.AddRange(new TreeNode[6]
      {
        this.viewMessagesNode,
        this.accessToFeesNode,
        this.generateDisclosuresNode,
        this.accessToProductAndPricingNode,
        this.submitForPurchase,
        this.viewPurchaseAdvice
      });
      this.accessToTPOTradeMgmtNode.Nodes.AddRange(new TreeNode[3]
      {
        this.viewCorrespondentTrades,
        this.viewCorrespondentMasterCmts,
        this.updateCorrespondentTrades
      });
      treeView.Nodes.AddRange(new TreeNode[3]
      {
        this.generalNode,
        this.pipelinesAndLoansNode,
        this.accessToTPOTradeMgmtNode
      });
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.accessToManageAccountNode, (object) AclFeature.TPOAdministrationTab_AccessToManageAccountPage);
      this.nodeToFeature.Add((object) this.accessCompanyInfoNode, (object) AclFeature.TPOAdministrationTab_AccessCompanyInfoTab);
      this.nodeToFeature.Add((object) this.accessBranchesNode, (object) AclFeature.TPOAdministrationTab_AccessBranchesTab);
      this.nodeToFeature.Add((object) this.accessPersonalEditMyInformationNode, (object) AclFeature.TPOAdministrationTab_AccessPersonalEditMyInformationTab);
      this.nodeToFeature.Add((object) this.accessPersonalEditMyLicenseNode, (object) AclFeature.TPOAdministrationTab_AccessPersonalEditMyLicenseTab);
      this.nodeToFeature.Add((object) this.accessPersonalEditMyNotificationNode, (object) AclFeature.TPOAdministrationTab_AccessPersonalEditMyNotificationTab);
      this.nodeToFeature.Add((object) this.generalNode, (object) AclFeature.TPOAdministrationTab_General);
      this.nodeToFeature.Add((object) this.accessToWholesaleNode, (object) AclFeature.TPOAdministrationTab_AccessToWholesale);
      this.nodeToFeature.Add((object) this.registerWholesaleLoanNode, (object) AclFeature.TPOAdministrationTab_RegisterWholesaleLoanNode);
      this.nodeToFeature.Add((object) this.accessToCorrespondentNode, (object) AclFeature.TPOAdministrationTab_AccessToCorrespondent);
      this.nodeToFeature.Add((object) this.registerCorrespondentLoanNode, (object) AclFeature.TPOAdministrationTab_RegisterCorrespondentLoanNode);
      this.nodeToFeature.Add((object) this.accessToScenariosNode, (object) AclFeature.TPOAdministrationTab_AccessToScenarios);
      this.nodeToFeature.Add((object) this.pipelinesAndLoansNode, (object) AclFeature.TPOAdministrationTab_PipelineAndLoans);
      this.nodeToFeature.Add((object) this.viewMessagesNode, (object) AclFeature.TPOAdministrationTab_ViewMessages);
      this.nodeToFeature.Add((object) this.accessToFeesNode, (object) AclFeature.TPOAdministrationTab_AccessToFees);
      this.nodeToFeature.Add((object) this.acceptFeesNode, (object) AclFeature.TPOAdministrationTab_AcceptFees);
      this.nodeToFeature.Add((object) this.generateDisclosuresNode, (object) AclFeature.TPOAdministrationTab_GenerateDisclosures);
      this.nodeToFeature.Add((object) this.accessToProductAndPricingNode, (object) AclFeature.TPOAdministrationTab_AccessToProductAndPricing);
      this.nodeToFeature.Add((object) this.viewLockHistoryNode, (object) AclFeature.TPOAdministrationTab_ViewLockHistory);
      this.nodeToFeature.Add((object) this.submitForPurchase, (object) AclFeature.TPOAdministrationTab_submitForPurchase);
      this.nodeToFeature.Add((object) this.viewPurchaseAdvice, (object) AclFeature.TPOAdministrationTab_ViewPurchaseAdvice);
      this.nodeToFeature.Add((object) this.accessToTPOTradeMgmtNode, (object) AclFeature.TPOAdministrationTab_AccessToTPOTradeMgmt);
      this.nodeToFeature.Add((object) this.viewCorrespondentTrades, (object) AclFeature.TPOAdministrationTab_ViewCorrespondentTrades);
      this.nodeToFeature.Add((object) this.viewCorrespondentMasterCmts, (object) AclFeature.TPOAdministrationTab_ViewCorrespondentMasterCmts);
      this.nodeToFeature.Add((object) this.updateCorrespondentTrades, (object) AclFeature.TPOAdministrationTab_UpdateCorrespondentTrades);
      this.nodeToFeature.Add((object) this.viewCmtConfirmations, (object) AclFeature.TPOAdministrationTab_ViewCmtConfirmations);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessToManageAccountPage, (object) this.accessToManageAccountNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessCompanyInfoTab, (object) this.accessCompanyInfoNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessBranchesTab, (object) this.accessBranchesNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessPersonalEditMyInformationTab, (object) this.accessPersonalEditMyInformationNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessPersonalEditMyLicenseTab, (object) this.accessPersonalEditMyLicenseNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessPersonalEditMyNotificationTab, (object) this.accessPersonalEditMyNotificationNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_General, (object) this.generalNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessToWholesale, (object) this.accessToWholesaleNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_RegisterWholesaleLoanNode, (object) this.registerWholesaleLoanNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessToCorrespondent, (object) this.accessToCorrespondentNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_RegisterCorrespondentLoanNode, (object) this.registerCorrespondentLoanNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessToScenarios, (object) this.accessToScenariosNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_PipelineAndLoans, (object) this.pipelinesAndLoansNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_ViewMessages, (object) this.viewMessagesNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessToFees, (object) this.accessToFeesNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AcceptFees, (object) this.acceptFeesNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_GenerateDisclosures, (object) this.generateDisclosuresNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessToProductAndPricing, (object) this.accessToProductAndPricingNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_ViewLockHistory, (object) this.viewLockHistoryNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_submitForPurchase, (object) this.submitForPurchase);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_ViewPurchaseAdvice, (object) this.viewPurchaseAdvice);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_AccessToTPOTradeMgmt, (object) this.accessToTPOTradeMgmtNode);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_ViewCorrespondentTrades, (object) this.viewCorrespondentTrades);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_ViewCorrespondentMasterCmts, (object) this.viewCorrespondentMasterCmts);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_UpdateCorrespondentTrades, (object) this.updateCorrespondentTrades);
      this.featureToNodeTbl.Add((object) AclFeature.TPOAdministrationTab_ViewCmtConfirmations, (object) this.viewCmtConfirmations);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.accessToManageAccountNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessCompanyInfoNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessBranchesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessPersonalEditMyInformationNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessPersonalEditMyLicenseNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessPersonalEditMyNotificationNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.generalNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessToWholesaleNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.registerWholesaleLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessToCorrespondentNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.registerCorrespondentLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessToScenariosNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.pipelinesAndLoansNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.viewMessagesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessToFeesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.acceptFeesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.generateDisclosuresNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessToProductAndPricingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.viewLockHistoryNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.submitForPurchase, (object) false);
      this.nodeToUpdateStatus.Add((object) this.viewPurchaseAdvice, (object) false);
      this.nodeToUpdateStatus.Add((object) this.accessToTPOTradeMgmtNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.viewCorrespondentTrades, (object) false);
      this.nodeToUpdateStatus.Add((object) this.viewCorrespondentMasterCmts, (object) false);
      this.nodeToUpdateStatus.Add((object) this.updateCorrespondentTrades, (object) false);
      this.nodeToUpdateStatus.Add((object) this.viewCmtConfirmations, (object) false);
      this.dependentNodes.Clear();
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
