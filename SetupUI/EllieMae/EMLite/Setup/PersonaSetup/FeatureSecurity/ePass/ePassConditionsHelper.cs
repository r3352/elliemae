// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity.ePass.ePassConditionsHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.FeatureSecurity.ePass
{
  public class ePassConditionsHelper : FeatureSecurityHelperBase
  {
    protected TreeNode preliminaryConditionsTabNode;
    protected TreeNode underwritingConditionsTabNode;
    protected TreeNode newEditDeleteConditionsNode;
    protected TreeNode changeSignoffNamesNode;
    protected TreeNode changePriorToNode;
    protected TreeNode changeSignOffDateNode;
    protected TreeNode markStatusCompletedNode;
    protected TreeNode addEditDeleteCommentsNode;
    protected TreeNode addSupportingDocumentsNode;
    protected TreeNode removeSupportingDocumentsNode;
    protected TreeNode postClosingConditionsTabNode;
    protected TreeNode editPostClosingConditionsNode;
    protected TreeNode statusFulfilledNode;
    protected TreeNode statusReceivedNode;
    protected TreeNode statusReviewedNode;
    protected TreeNode statusRejectedNode;
    protected TreeNode statusClearedNode;
    protected TreeNode statusWaivedNode;
    protected TreeNode historyTabNode;
    protected TreeNode addBusinessRulesPreliminaryNode;
    protected TreeNode addEditDeletePreliminaryConditionNode;
    protected TreeNode addBusinessRulesUnderwriteNode;
    protected TreeNode addBusinessRulesPostclosingNode;
    protected TreeNode sellConditionNode;
    protected TreeNode addEditDeleteSellConditionNode;
    protected TreeNode importInvestorConditionNode;
    protected TreeNode purchaseConditionNode;
    protected TreeNode pcNewEditDeleteConditionNode;
    protected TreeNode pcAddAutomatedPurchaseConditionNode;
    protected TreeNode pcChangePriorToNode;
    protected TreeNode pcMarkStatusCompletedNode;
    protected TreeNode pcAddEditDeleteCommentsNode;
    protected TreeNode pcAddSupportingDocNode;
    protected TreeNode pcRemoveSupportingDocNode;
    protected TreeNode pcChangeSignoffNamesNode;
    protected TreeNode pcChangeSignoffDatesNode;
    protected TreeNode deliverConditionResponseNode;
    protected TreeNode conditionDeliveryStatusNode;
    protected TreeNode importAllDeliveryConditionNode;

    public ePassConditionsHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.LoanEFolderFeatures);
    }

    public ePassConditionsHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.LoanEFolderFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      bool policySetting = this.session.SessionObjects.GetPolicySetting("EnableSellCondition");
      this.preliminaryConditionsTabNode = new TreeNode("Preliminary Conditions Tab");
      this.addEditDeletePreliminaryConditionNode = new TreeNode("Add/Edit/Delete Conditions");
      this.addBusinessRulesPreliminaryNode = new TreeNode("Add Automated Conditions");
      this.preliminaryConditionsTabNode.Nodes.AddRange(new TreeNode[2]
      {
        this.addEditDeletePreliminaryConditionNode,
        this.addBusinessRulesPreliminaryNode
      });
      this.underwritingConditionsTabNode = new TreeNode("Underwriting Conditions Tab");
      this.newEditDeleteConditionsNode = new TreeNode("New/Edit/Delete Conditions");
      this.changeSignoffNamesNode = new TreeNode("Change Signoff Names");
      this.changeSignOffDateNode = new TreeNode("Change Signoff Dates");
      this.newEditDeleteConditionsNode.Nodes.AddRange(new TreeNode[2]
      {
        this.changeSignoffNamesNode,
        this.changeSignOffDateNode
      });
      this.markStatusCompletedNode = new TreeNode("Mark Status Completed");
      this.statusFulfilledNode = new TreeNode("Fulfilled");
      this.statusReceivedNode = new TreeNode("Received");
      this.statusReviewedNode = new TreeNode("Reviewed");
      this.statusRejectedNode = new TreeNode("Rejected");
      this.statusClearedNode = new TreeNode("Cleared");
      this.statusWaivedNode = new TreeNode("Waived");
      this.markStatusCompletedNode.Nodes.AddRange(new TreeNode[6]
      {
        this.statusFulfilledNode,
        this.statusReceivedNode,
        this.statusReviewedNode,
        this.statusRejectedNode,
        this.statusClearedNode,
        this.statusWaivedNode
      });
      this.addEditDeleteCommentsNode = new TreeNode("Add/Edit/Delete Comments");
      this.addSupportingDocumentsNode = new TreeNode("Add Supporting Documents");
      this.removeSupportingDocumentsNode = new TreeNode("Remove Supporting Documents");
      this.changePriorToNode = new TreeNode("Change Prior To");
      this.addBusinessRulesUnderwriteNode = new TreeNode("Add Automated Conditions");
      this.underwritingConditionsTabNode.Nodes.AddRange(new TreeNode[7]
      {
        this.newEditDeleteConditionsNode,
        this.addBusinessRulesUnderwriteNode,
        this.changePriorToNode,
        this.markStatusCompletedNode,
        this.addEditDeleteCommentsNode,
        this.addSupportingDocumentsNode,
        this.removeSupportingDocumentsNode
      });
      this.postClosingConditionsTabNode = new TreeNode("Post-Closing Conditions Tab");
      this.editPostClosingConditionsNode = new TreeNode("Add/Edit/Delete Conditions");
      this.addBusinessRulesPostclosingNode = new TreeNode("Add Automated Conditions");
      this.postClosingConditionsTabNode.Nodes.AddRange(new TreeNode[2]
      {
        this.addBusinessRulesPostclosingNode,
        this.editPostClosingConditionsNode
      });
      this.historyTabNode = new TreeNode("History Tab");
      this.sellConditionNode = new TreeNode("Delivery Conditions Tab");
      this.addEditDeleteSellConditionNode = new TreeNode("Add/Edit/Delete Delivery Conditions");
      this.importInvestorConditionNode = new TreeNode("Review and Import Conditions");
      this.importAllDeliveryConditionNode = new TreeNode("Import All Conditions");
      this.deliverConditionResponseNode = new TreeNode("Deliver Condition Responses");
      this.conditionDeliveryStatusNode = new TreeNode("Condition Delivery Status");
      this.sellConditionNode.Nodes.AddRange(new TreeNode[5]
      {
        this.addEditDeleteSellConditionNode,
        this.importAllDeliveryConditionNode,
        this.importInvestorConditionNode,
        this.deliverConditionResponseNode,
        this.conditionDeliveryStatusNode
      });
      this.purchaseConditionNode = new TreeNode("Purchase Conditions");
      this.pcNewEditDeleteConditionNode = new TreeNode("New/Edit/Delete Conditions");
      this.pcChangeSignoffNamesNode = new TreeNode("Change Signoff Names");
      this.pcChangeSignoffDatesNode = new TreeNode("Change Signoff Dates");
      this.pcNewEditDeleteConditionNode.Nodes.AddRange(new TreeNode[2]
      {
        this.pcChangeSignoffNamesNode,
        this.pcChangeSignoffDatesNode
      });
      this.pcAddAutomatedPurchaseConditionNode = new TreeNode("Add Automated Conditions");
      this.pcChangePriorToNode = new TreeNode("Change Prior To");
      this.pcMarkStatusCompletedNode = new TreeNode("Mark Status Completed");
      this.pcAddEditDeleteCommentsNode = new TreeNode("Add/Edit/Delete Comments");
      this.pcAddSupportingDocNode = new TreeNode("Add Supporting Documents");
      this.pcRemoveSupportingDocNode = new TreeNode("Remove Supporting Documents");
      this.purchaseConditionNode.Nodes.AddRange(new TreeNode[7]
      {
        this.pcNewEditDeleteConditionNode,
        this.pcAddAutomatedPurchaseConditionNode,
        this.pcChangePriorToNode,
        this.pcMarkStatusCompletedNode,
        this.pcAddEditDeleteCommentsNode,
        this.pcAddSupportingDocNode,
        this.pcRemoveSupportingDocNode
      });
      this.dependentNodes.AddRange((ICollection) new TreeNode[1]
      {
        this.markStatusCompletedNode
      });
      treeView.Nodes.AddRange(new TreeNode[3]
      {
        this.preliminaryConditionsTabNode,
        this.underwritingConditionsTabNode,
        this.postClosingConditionsTabNode
      });
      if (policySetting)
        treeView.Nodes.Add(this.sellConditionNode);
      if ((bool) this.session.ServerManager.GetServerSetting("Policies.AllowPurchaseCondition"))
        treeView.Nodes.Add(this.purchaseConditionNode);
      treeView.Nodes.Add(this.historyTabNode);
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.preliminaryConditionsTabNode, (object) AclFeature.eFolder_Conditions_PreliminaryCondition);
      this.nodeToFeature.Add((object) this.addEditDeletePreliminaryConditionNode, (object) AclFeature.eFolder_Conditions_AddEditDeleteCondPreliminary);
      this.nodeToFeature.Add((object) this.underwritingConditionsTabNode, (object) AclFeature.eFolder_Conditions_UnderWritingCondTab);
      this.nodeToFeature.Add((object) this.newEditDeleteConditionsNode, (object) AclFeature.eFolder_Conditions_UW_NewEditImpDel);
      this.nodeToFeature.Add((object) this.changeSignoffNamesNode, (object) AclFeature.eFolder_Conditions_UW_EditUser);
      this.nodeToFeature.Add((object) this.statusClearedNode, (object) AclFeature.eFolder_Conditions_UW_Status_Cleared);
      this.nodeToFeature.Add((object) this.statusFulfilledNode, (object) AclFeature.eFolder_Conditions_UW_Status_Fulfilled);
      this.nodeToFeature.Add((object) this.statusReceivedNode, (object) AclFeature.eFolder_Conditions_UW_Status_Received);
      this.nodeToFeature.Add((object) this.statusRejectedNode, (object) AclFeature.eFolder_Conditions_UW_Status_Rejected);
      this.nodeToFeature.Add((object) this.statusReviewedNode, (object) AclFeature.eFolder_Conditions_UW_Status_Reviewed);
      this.nodeToFeature.Add((object) this.statusWaivedNode, (object) AclFeature.eFolder_Conditions_UW_Status_Waived);
      this.nodeToFeature.Add((object) this.addEditDeleteCommentsNode, (object) AclFeature.eFolder_Conditions_UW_EditComment);
      this.nodeToFeature.Add((object) this.addSupportingDocumentsNode, (object) AclFeature.eFolder_Conditions_UW_AddSupportDoc);
      this.nodeToFeature.Add((object) this.removeSupportingDocumentsNode, (object) AclFeature.eFolder_Conditions_UW_RemoveSupportDoc);
      this.nodeToFeature.Add((object) this.postClosingConditionsTabNode, (object) AclFeature.eFolder_Conditions_PostClosingCondTab);
      this.nodeToFeature.Add((object) this.editPostClosingConditionsNode, (object) AclFeature.eFolder_Conditions_PCCT_NewEditImpDel);
      this.nodeToFeature.Add((object) this.changeSignOffDateNode, (object) AclFeature.eFolder_Conditions_UW_EditDate);
      this.nodeToFeature.Add((object) this.changePriorToNode, (object) AclFeature.eFolder_Conditions_UW_PriorTo);
      this.nodeToFeature.Add((object) this.sellConditionNode, (object) AclFeature.eFolder_Conditions_SellCondTab);
      this.nodeToFeature.Add((object) this.addEditDeleteSellConditionNode, (object) AclFeature.eFolder_Conditions_SellCond_AddEditDel);
      this.nodeToFeature.Add((object) this.importInvestorConditionNode, (object) AclFeature.eFolder_Conditions_SellCond_ImportInvestorCond);
      this.nodeToFeature.Add((object) this.importAllDeliveryConditionNode, (object) AclFeature.eFolder_Conditions_SellCond_ImportAllDeliveryCond);
      this.nodeToFeature.Add((object) this.deliverConditionResponseNode, (object) AclFeature.eFolder_Conditions_SellCond_DeliverConditionResponse);
      this.nodeToFeature.Add((object) this.conditionDeliveryStatusNode, (object) AclFeature.eFolder_Conditions_SellCond_ConditionDeliveryStatus);
      this.nodeToFeature.Add((object) this.purchaseConditionNode, (object) AclFeature.eFolder_Conditions_PurchaseCondTab);
      this.nodeToFeature.Add((object) this.pcNewEditDeleteConditionNode, (object) AclFeature.eFolder_Conditions_PC_NewEditDelCond);
      this.nodeToFeature.Add((object) this.pcChangeSignoffNamesNode, (object) AclFeature.eFolder_Conditions_PC_EditName);
      this.nodeToFeature.Add((object) this.pcChangeSignoffDatesNode, (object) AclFeature.eFolder_Conditions_PC_EditDate);
      this.nodeToFeature.Add((object) this.pcAddAutomatedPurchaseConditionNode, (object) AclFeature.eFolder_Conditions_PC_AddAutoCond);
      this.nodeToFeature.Add((object) this.pcChangePriorToNode, (object) AclFeature.eFolder_Conditions_PC_PriorTo);
      this.nodeToFeature.Add((object) this.pcMarkStatusCompletedNode, (object) AclFeature.eFolder_Conditions_PC_Status_Completed);
      this.nodeToFeature.Add((object) this.pcAddEditDeleteCommentsNode, (object) AclFeature.eFolder_Conditions_PC_AddEditDelComment);
      this.nodeToFeature.Add((object) this.pcAddSupportingDocNode, (object) AclFeature.eFolder_Conditions_PC_AddSupportDoc);
      this.nodeToFeature.Add((object) this.pcRemoveSupportingDocNode, (object) AclFeature.eFolder_Conditions_PC_RemoveSupportDoc);
      this.nodeToFeature.Add((object) this.historyTabNode, (object) AclFeature.eFolder_Conditions_HistoryTab);
      this.nodeToFeature.Add((object) this.addBusinessRulesPreliminaryNode, (object) AclFeature.eFolder_Conditions_AddBusinessRulePreliminary);
      this.nodeToFeature.Add((object) this.addBusinessRulesUnderwriteNode, (object) AclFeature.eFolder_Conditions_AddBusinessRuleUnderwriting);
      this.nodeToFeature.Add((object) this.addBusinessRulesPostclosingNode, (object) AclFeature.eFolder_Conditions_AddBusinessRulePostClosing);
      this.featureToNodeTbl = new Hashtable(FeatureSets.Features.Length);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PreliminaryCondition, (object) this.preliminaryConditionsTabNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_AddEditDeleteCondPreliminary, (object) this.addEditDeletePreliminaryConditionNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UnderWritingCondTab, (object) this.underwritingConditionsTabNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_NewEditImpDel, (object) this.newEditDeleteConditionsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_EditUser, (object) this.changeSignoffNamesNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_Status_Cleared, (object) this.statusClearedNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_Status_Fulfilled, (object) this.statusFulfilledNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_Status_Received, (object) this.statusReceivedNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_Status_Rejected, (object) this.statusRejectedNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_Status_Reviewed, (object) this.statusReviewedNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_Status_Waived, (object) this.statusWaivedNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_EditComment, (object) this.addEditDeleteCommentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_AddSupportDoc, (object) this.addSupportingDocumentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_RemoveSupportDoc, (object) this.removeSupportingDocumentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PostClosingCondTab, (object) this.postClosingConditionsTabNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PCCT_NewEditImpDel, (object) this.editPostClosingConditionsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_EditDate, (object) this.changeSignOffDateNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_UW_PriorTo, (object) this.changePriorToNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_SellCondTab, (object) this.sellConditionNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_SellCond_AddEditDel, (object) this.addEditDeleteSellConditionNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_SellCond_ImportInvestorCond, (object) this.importInvestorConditionNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_SellCond_ImportAllDeliveryCond, (object) this.importAllDeliveryConditionNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_SellCond_DeliverConditionResponse, (object) this.deliverConditionResponseNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_SellCond_ConditionDeliveryStatus, (object) this.conditionDeliveryStatusNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PurchaseCondTab, (object) this.purchaseConditionNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PC_NewEditDelCond, (object) this.pcNewEditDeleteConditionNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PC_EditName, (object) this.pcChangeSignoffNamesNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PC_EditDate, (object) this.pcChangeSignoffDatesNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PC_AddAutoCond, (object) this.pcAddAutomatedPurchaseConditionNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PC_PriorTo, (object) this.pcChangePriorToNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PC_Status_Completed, (object) this.pcMarkStatusCompletedNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PC_AddEditDelComment, (object) this.pcAddEditDeleteCommentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PC_AddSupportDoc, (object) this.pcAddSupportingDocNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_PC_RemoveSupportDoc, (object) this.pcRemoveSupportingDocNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_HistoryTab, (object) this.historyTabNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_AddBusinessRulePreliminary, (object) this.addBusinessRulesPreliminaryNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_AddBusinessRuleUnderwriting, (object) this.addBusinessRulesUnderwriteNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Conditions_AddBusinessRulePostClosing, (object) this.addBusinessRulesPostclosingNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.preliminaryConditionsTabNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addEditDeletePreliminaryConditionNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.underwritingConditionsTabNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.newEditDeleteConditionsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.changeSignoffNamesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.statusClearedNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.statusFulfilledNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.statusReceivedNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.statusRejectedNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.statusReviewedNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.statusWaivedNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addEditDeleteCommentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addSupportingDocumentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.removeSupportingDocumentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.postClosingConditionsTabNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editPostClosingConditionsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.changeSignOffDateNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.changePriorToNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.historyTabNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addBusinessRulesPreliminaryNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addBusinessRulesUnderwriteNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addBusinessRulesPostclosingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.sellConditionNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addEditDeleteSellConditionNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.importInvestorConditionNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.importAllDeliveryConditionNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deliverConditionResponseNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.conditionDeliveryStatusNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.purchaseConditionNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.pcNewEditDeleteConditionNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.pcAddAutomatedPurchaseConditionNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.pcChangePriorToNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.pcMarkStatusCompletedNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.pcAddEditDeleteCommentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.pcAddSupportingDocNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.pcRemoveSupportingDocNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.pcChangeSignoffNamesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.pcChangeSignoffDatesNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
      treeView.ExpandAll();
    }
  }
}
