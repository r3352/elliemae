// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ToolsSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ToolsSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode fileContactsNode;
    protected TreeNode grantWriteAccessNode;
    protected TreeNode showInvestorContactNode;
    protected TreeNode bizContactsNode;
    protected TreeNode prequalNode;
    protected TreeNode debtConsolidationNode;
    protected TreeNode loanComparisonNode;
    protected TreeNode cashToCloseNode;
    protected TreeNode rentOwnNode;
    protected TreeNode secureTransferNode;
    protected TreeNode profitMngtNode;
    protected TreeNode trustAccountNode;
    protected TreeNode fundingWSNode;
    protected TreeNode fundingImportWSNode;
    protected TreeNode brokerCheckCalNode;
    protected TreeNode fundingBalWSNode;
    protected TreeNode conversationLogNode;
    protected TreeNode taskNode;
    protected TreeNode taskAddNode;
    protected TreeNode taskEditNode;
    protected TreeNode taskDeleteNode;
    protected TreeNode piggybackLoanNode;
    protected TreeNode tqlToolNode;
    protected TreeNode tqlMiCenterToolNode;
    protected TreeNode tqlToolSelectInvestorNode;
    protected TreeNode tqlStartStopPublishingNode;
    protected TreeNode tqlAddCommentsNode;
    protected TreeNode tqlAddFraudCommentsNode;
    protected TreeNode tqlAddComplianceCommentsNode;
    protected TreeNode secondaryRegistrationNode;
    protected TreeNode purchaseAdviceFormNode;
    protected TreeNode correspondentPurchaseAdviceFormNode;
    protected TreeNode correspondentComplianceReviewDataNode;
    protected TreeNode tpoWebCenterFormNode;
    protected TreeNode correspondentLoanStatusNode;
    protected TreeNode companyInformation;
    protected TreeNode license;
    protected TreeNode dba;
    protected TreeNode warehouse;
    protected TreeNode loanType;
    protected TreeNode tpoContacts;
    protected TreeNode commitment;
    protected TreeNode fees;
    protected TreeNode LOComp;
    protected TreeNode notes;
    protected TreeNode webCenterSetup;
    protected TreeNode attachments;
    protected TreeNode salesReps;
    protected TreeNode lenderContacts;
    protected TreeNode tpoWebCenterDocs;
    protected TreeNode customFields;
    protected TreeNode customFieldsTab1;
    protected TreeNode customFieldsTab2;
    protected TreeNode customFieldsTab3;
    protected TreeNode customFieldsTab4;
    protected TreeNode customFieldsTab5;
    protected TreeNode shippingDetailNode;
    protected TreeNode documentTrackingNode;
    protected TreeNode importShippingDetails;
    protected TreeNode lockRequestFormNode;
    protected TreeNode lockComparisonToolNode;
    protected TreeNode validatePricingNode;
    protected TreeNode underwriterSummaryNode;
    protected TreeNode projectReview;
    protected TreeNode auditTrailNode;
    protected TreeNode ctEditUser;
    protected TreeNode interimServicingNode;
    protected TreeNode isCopyLoanNode;
    protected TreeNode isReCopyLoanNode;
    protected TreeNode isEnterTransactionNode;
    protected TreeNode isEditDeleteTransactionNode;
    protected TreeNode pafTemplateCreateEditNode;
    protected TreeNode disclosureTrackingNode;
    protected TreeNode changeReasonsNode;
    protected TreeNode excludeIncludeNode;
    protected TreeNode lESSPLSafeHarborNode;
    protected TreeNode cDNode;
    protected TreeNode feeToleranceWorksheetNode;
    protected TreeNode cureToleranceAlertNode;
    protected TreeNode safeHarborDisclosureNode;
    protected TreeNode ecsDataViewerNode;
    protected TreeNode netTangibleBenefitNode;
    protected TreeNode dtChangeSentDateNode;
    protected TreeNode amortizationScheduleNode;
    protected TreeNode dtManualFullfillmentNode;
    protected TreeNode dtCreateManualEntryNode;
    protected TreeNode dtWithdrawEConsent;
    protected TreeNode loCompToolNode;
    protected TreeNode loCompToolBrokerNode;
    protected TreeNode loCompToolOfficerNode;
    protected TreeNode ausTrackingToolNode;
    protected TreeNode ausTrackingToolManualNode;
    protected TreeNode RepAndWarrantTrackerNode;
    protected TreeNode ausVerificationToolNode;
    protected TreeNode ausVerificationToolNewNode;
    protected TreeNode ausVerificationToolEditNode;

    public ToolsSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.ToolsFeatures);
    }

    public ToolsSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.ToolsFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.fileContactsNode = new TreeNode("File Contacts");
      this.grantWriteAccessNode = new TreeNode("Grant Write Access to Loan Team Members");
      this.grantWriteAccessNode.Tag = (object) AclFeature.ToolsTab_GrantWriteAccess;
      this.grantWriteAccessNode.ForeColor = Color.Blue;
      this.showInvestorContactNode = new TreeNode("Show Investor Contact");
      this.bizContactsNode = new TreeNode("Business Contacts");
      this.conversationLogNode = new TreeNode("Conversation Log");
      this.taskNode = new TreeNode("Tasks");
      this.taskAddNode = new TreeNode("Add Tasks");
      this.taskEditNode = new TreeNode("Edit Tasks");
      this.taskDeleteNode = new TreeNode("Delete Tasks");
      this.taskNode.Nodes.AddRange(new TreeNode[3]
      {
        this.taskAddNode,
        this.taskEditNode,
        this.taskDeleteNode
      });
      this.amortizationScheduleNode = new TreeNode("Amortization Schedule");
      this.piggybackLoanNode = new TreeNode("Piggyback Loans");
      this.prequalNode = new TreeNode("Prequalification");
      this.debtConsolidationNode = new TreeNode("Debt Consolidation");
      this.loanComparisonNode = new TreeNode("Loan Comparison");
      this.cashToCloseNode = new TreeNode("Cash-to-Close");
      this.rentOwnNode = new TreeNode("Rent vs. Own");
      this.secureTransferNode = new TreeNode("Secure Form Transfer");
      this.profitMngtNode = new TreeNode("Profit Management");
      this.trustAccountNode = new TreeNode("Trust Account");
      this.fundingWSNode = new TreeNode("Funding Worksheet");
      this.fundingImportWSNode = new TreeNode("Import Funding Details");
      this.brokerCheckCalNode = new TreeNode("Broker Check Calculation");
      this.fundingBalWSNode = new TreeNode("Funding Balancing Worksheet");
      this.secondaryRegistrationNode = new TreeNode("Secondary Registration");
      this.purchaseAdviceFormNode = new TreeNode("Purchase Advice Form");
      this.correspondentPurchaseAdviceFormNode = new TreeNode("Correspondent Purchase Advice Form");
      bool boolean = Convert.ToBoolean(this.session.ServerManager.GetServerSetting("Policies.Correspondent", false));
      if (boolean)
        this.correspondentComplianceReviewDataNode = new TreeNode("Correspondent Compliance Review Data (Web only)");
      this.tpoWebCenterFormNode = new TreeNode("TPO Information");
      this.correspondentLoanStatusNode = new TreeNode("Correspondent Loan Status");
      this.companyInformation = new TreeNode("Basic Info tab");
      this.dba = new TreeNode("DBA tab");
      this.warehouse = new TreeNode("Warehouse tab");
      this.license = new TreeNode("License tab");
      this.loanType = new TreeNode("Loan Criteria tab");
      this.tpoContacts = new TreeNode("Key Contacts tab");
      this.commitment = new TreeNode("Commitments tab");
      this.fees = new TreeNode("Fees tab");
      this.LOComp = new TreeNode("LO Comp tab");
      this.notes = new TreeNode("Notes tab");
      this.webCenterSetup = new TreeNode("TPO WebCenter Setup tab");
      this.attachments = new TreeNode("Attachments tab");
      this.salesReps = new TreeNode("Sales Reps / AE tab");
      this.lenderContacts = new TreeNode("Lender Contacts");
      this.tpoWebCenterDocs = new TreeNode("TPO WebCenter Docs tab");
      this.customFields = new TreeNode("Custom Fields tab");
      this.customFieldsTab1 = new TreeNode("Custom Fields tab 1");
      this.customFieldsTab2 = new TreeNode("Custom Fields tab 2");
      this.customFieldsTab3 = new TreeNode("Custom Fields tab 3");
      this.customFieldsTab4 = new TreeNode("Custom Fields tab 4");
      this.customFieldsTab5 = new TreeNode("Custom Fields tab 5");
      this.pafTemplateCreateEditNode = new TreeNode("Create/Edit Template");
      this.purchaseAdviceFormNode.Nodes.Add(this.pafTemplateCreateEditNode);
      this.shippingDetailNode = new TreeNode("Shipping Detail");
      this.documentTrackingNode = new TreeNode("Collateral Tracking");
      this.importShippingDetails = new TreeNode("Import Shipping Details");
      this.lockRequestFormNode = new TreeNode("Lock Request Form");
      this.lockComparisonToolNode = new TreeNode("Lock Comparison Tool");
      this.validatePricingNode = new TreeNode("Validate Pricing");
      this.underwriterSummaryNode = new TreeNode("Underwriter Summary");
      this.projectReview = new TreeNode("Project Review");
      this.auditTrailNode = new TreeNode("Audit Trail");
      this.ausTrackingToolNode = new TreeNode("AUS Tracking");
      this.RepAndWarrantTrackerNode = new TreeNode("Rep and Warrant Tracker");
      this.ausVerificationToolNode = new TreeNode("Verification and Documentation Tracking");
      this.disclosureTrackingNode = new TreeNode("Disclosure Tracking");
      this.feeToleranceWorksheetNode = new TreeNode("Fee Variance Worksheet");
      this.cureToleranceAlertNode = new TreeNode("Cure Variance");
      this.safeHarborDisclosureNode = new TreeNode("Anti-Steering Safe Harbor Disclosure");
      this.ecsDataViewerNode = new TreeNode("ECS Data Viewer");
      this.netTangibleBenefitNode = new TreeNode("Net Tangible Benefit");
      this.dtChangeSentDateNode = new TreeNode("Change Disclosure Information");
      this.changeReasonsNode = new TreeNode("Change Reasons");
      this.excludeIncludeNode = new TreeNode("Exclude/Include Records");
      this.dtManualFullfillmentNode = new TreeNode("Manually Fulfill");
      this.dtWithdrawEConsent = new TreeNode("Withdraw eConsent");
      this.loCompToolNode = new TreeNode("LO Comp Tool");
      this.loCompToolBrokerNode = new TreeNode("Broker Comp");
      this.loCompToolOfficerNode = new TreeNode("Loan Officer Comp");
      this.loCompToolNode.Nodes.AddRange(new TreeNode[2]
      {
        this.loCompToolBrokerNode,
        this.loCompToolOfficerNode
      });
      bool serverSetting = (bool) this.session.ServerManager.GetServerSetting("Policies.DiscloseManually");
      if (serverSetting)
      {
        this.dtCreateManualEntryNode = new TreeNode("Create Manual Entry");
        this.lESSPLSafeHarborNode = new TreeNode("LE / SSPL / Safe Harbor");
        this.cDNode = new TreeNode("CD");
      }
      this.interimServicingNode = new TreeNode("Interim Servicing");
      this.isCopyLoanNode = new TreeNode("Start Servicing");
      this.isReCopyLoanNode = new TreeNode("Re-Start Servicing");
      this.isCopyLoanNode.Nodes.Add(this.isReCopyLoanNode);
      this.isEnterTransactionNode = new TreeNode("Enter Transaction");
      this.isEditDeleteTransactionNode = new TreeNode("Edit/Delete Transaction");
      this.tqlToolNode = new TreeNode("TQL Services");
      this.tqlMiCenterToolNode = new TreeNode("MI Center");
      this.tqlToolSelectInvestorNode = new TreeNode("Select Investor");
      this.tqlStartStopPublishingNode = new TreeNode("Start/Stop Publishing");
      this.tqlAddCommentsNode = new TreeNode("Add Comments");
      this.tqlAddFraudCommentsNode = new TreeNode("Add Fraud Report Comments");
      this.tqlAddComplianceCommentsNode = new TreeNode("Add Compliance Report Comments");
      this.ausTrackingToolManualNode = new TreeNode("Create Manual Entry");
      this.ausTrackingToolNode.Nodes.AddRange(new TreeNode[1]
      {
        this.ausTrackingToolManualNode
      });
      this.ausVerificationToolNewNode = new TreeNode("Create New Entry");
      this.ausVerificationToolEditNode = new TreeNode("Edit Entry");
      this.ausVerificationToolNode.Nodes.AddRange(new TreeNode[2]
      {
        this.ausVerificationToolNewNode,
        this.ausVerificationToolEditNode
      });
      this.tpoWebCenterFormNode.Nodes.AddRange(new TreeNode[16]
      {
        this.companyInformation,
        this.dba,
        this.license,
        this.loanType,
        this.tpoContacts,
        this.warehouse,
        this.fees,
        this.LOComp,
        this.commitment,
        this.notes,
        this.webCenterSetup,
        this.tpoWebCenterDocs,
        this.attachments,
        this.salesReps,
        this.lenderContacts,
        this.customFields
      });
      this.customFields.Nodes.AddRange(new TreeNode[5]
      {
        this.customFieldsTab1,
        this.customFieldsTab2,
        this.customFieldsTab3,
        this.customFieldsTab4,
        this.customFieldsTab5
      });
      this.dtChangeSentDateNode.Nodes.AddRange(new TreeNode[1]
      {
        this.changeReasonsNode
      });
      if (serverSetting)
      {
        this.dtCreateManualEntryNode.Nodes.AddRange(new TreeNode[2]
        {
          this.lESSPLSafeHarborNode,
          this.cDNode
        });
        this.disclosureTrackingNode.Nodes.AddRange(new TreeNode[5]
        {
          this.dtChangeSentDateNode,
          this.excludeIncludeNode,
          this.dtManualFullfillmentNode,
          this.dtCreateManualEntryNode,
          this.dtWithdrawEConsent
        });
      }
      else
        this.disclosureTrackingNode.Nodes.AddRange(new TreeNode[4]
        {
          this.dtChangeSentDateNode,
          this.excludeIncludeNode,
          this.dtManualFullfillmentNode,
          this.dtWithdrawEConsent
        });
      this.tqlToolNode.Nodes.AddRange(new TreeNode[5]
      {
        this.tqlToolSelectInvestorNode,
        this.tqlStartStopPublishingNode,
        this.tqlAddCommentsNode,
        this.tqlAddFraudCommentsNode,
        this.tqlAddComplianceCommentsNode
      });
      this.interimServicingNode.Nodes.AddRange(new TreeNode[3]
      {
        this.isCopyLoanNode,
        this.isEnterTransactionNode,
        this.isEditDeleteTransactionNode
      });
      this.fileContactsNode.Nodes.AddRange(new TreeNode[2]
      {
        this.grantWriteAccessNode,
        this.showInvestorContactNode
      });
      this.feeToleranceWorksheetNode.Nodes.AddRange(new TreeNode[1]
      {
        this.cureToleranceAlertNode
      });
      this.documentTrackingNode.Nodes.AddRange(new TreeNode[1]
      {
        this.importShippingDetails
      });
      if (this.session.EncompassEdition == EncompassEdition.Banker)
      {
        this.fundingWSNode.Nodes.AddRange(new TreeNode[1]
        {
          this.fundingImportWSNode
        });
        treeView.Nodes.AddRange(new TreeNode[17]
        {
          this.tpoWebCenterFormNode,
          this.correspondentLoanStatusNode,
          this.fileContactsNode,
          this.bizContactsNode,
          this.conversationLogNode,
          this.taskNode,
          this.ausTrackingToolNode,
          this.RepAndWarrantTrackerNode,
          this.ausVerificationToolNode,
          this.disclosureTrackingNode,
          this.feeToleranceWorksheetNode,
          this.safeHarborDisclosureNode,
          this.netTangibleBenefitNode,
          this.ecsDataViewerNode,
          this.tqlToolNode,
          this.tqlMiCenterToolNode,
          this.lockRequestFormNode
        });
        treeView.Nodes.Add(this.lockComparisonToolNode);
        this.lockComparisonToolNode.Nodes.Add(this.validatePricingNode);
        treeView.Nodes.AddRange(new TreeNode[16]
        {
          this.prequalNode,
          this.debtConsolidationNode,
          this.loanComparisonNode,
          this.cashToCloseNode,
          this.rentOwnNode,
          this.projectReview,
          this.underwriterSummaryNode,
          this.fundingWSNode,
          this.fundingBalWSNode,
          this.brokerCheckCalNode,
          this.secondaryRegistrationNode,
          this.interimServicingNode,
          this.shippingDetailNode,
          this.documentTrackingNode,
          this.purchaseAdviceFormNode,
          this.correspondentPurchaseAdviceFormNode
        });
        if (boolean)
          treeView.Nodes.Add(this.correspondentComplianceReviewDataNode);
        treeView.Nodes.AddRange(new TreeNode[7]
        {
          this.amortizationScheduleNode,
          this.piggybackLoanNode,
          this.secureTransferNode,
          this.auditTrailNode,
          this.profitMngtNode,
          this.loCompToolNode,
          this.trustAccountNode
        });
      }
      else
      {
        treeView.Nodes.AddRange(new TreeNode[26]
        {
          this.fileContactsNode,
          this.bizContactsNode,
          this.conversationLogNode,
          this.taskNode,
          this.ausTrackingToolNode,
          this.RepAndWarrantTrackerNode,
          this.ausVerificationToolNode,
          this.disclosureTrackingNode,
          this.feeToleranceWorksheetNode,
          this.safeHarborDisclosureNode,
          this.netTangibleBenefitNode,
          this.ecsDataViewerNode,
          this.tqlToolNode,
          this.tqlMiCenterToolNode,
          this.secureTransferNode,
          this.prequalNode,
          this.debtConsolidationNode,
          this.loanComparisonNode,
          this.cashToCloseNode,
          this.rentOwnNode,
          this.piggybackLoanNode,
          this.profitMngtNode,
          this.trustAccountNode,
          this.underwriterSummaryNode,
          this.auditTrailNode,
          this.correspondentComplianceReviewDataNode
        });
        if (boolean)
          treeView.Nodes.Add(this.correspondentComplianceReviewDataNode);
      }
      treeView.ExpandAll();
      this.dependentNodes.Add((object) this.grantWriteAccessNode);
      this.dependentNodes.Add((object) this.loCompToolNode);
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.fileContactsNode, (object) AclFeature.ToolsTab_FileContacts);
      this.nodeToFeature.Add((object) this.bizContactsNode, (object) AclFeature.ToolsTab_BizContacts);
      this.nodeToFeature.Add((object) this.conversationLogNode, (object) AclFeature.ToolsTab_ConversationLog);
      this.nodeToFeature.Add((object) this.taskNode, (object) AclFeature.ToolsTab_Task);
      this.nodeToFeature.Add((object) this.taskAddNode, (object) AclFeature.ToolsTab_Task_Add);
      this.nodeToFeature.Add((object) this.taskEditNode, (object) AclFeature.ToolsTab_Task_Edit);
      this.nodeToFeature.Add((object) this.taskDeleteNode, (object) AclFeature.ToolsTab_Task_Delete);
      this.nodeToFeature.Add((object) this.piggybackLoanNode, (object) AclFeature.ToolsTab_PiggybackLoans);
      this.nodeToFeature.Add((object) this.prequalNode, (object) AclFeature.ToolsTab_Prequal);
      this.nodeToFeature.Add((object) this.debtConsolidationNode, (object) AclFeature.ToolsTab_DebtConsolidation);
      this.nodeToFeature.Add((object) this.loanComparisonNode, (object) AclFeature.ToolsTab_LoanComparison);
      this.nodeToFeature.Add((object) this.cashToCloseNode, (object) AclFeature.ToolsTab_CashToClose);
      this.nodeToFeature.Add((object) this.rentOwnNode, (object) AclFeature.ToolsTab_RentOwn);
      this.nodeToFeature.Add((object) this.secureTransferNode, (object) AclFeature.ToolsTab_SecureFormTransfer);
      this.nodeToFeature.Add((object) this.profitMngtNode, (object) AclFeature.ToolsTab_ProfitMngt);
      this.nodeToFeature.Add((object) this.trustAccountNode, (object) AclFeature.ToolsTab_TrustAccount);
      this.nodeToFeature.Add((object) this.auditTrailNode, (object) AclFeature.ToolsTab_AuditTrail);
      this.nodeToFeature.Add((object) this.ausTrackingToolNode, (object) AclFeature.ToolsTab_AUSTracking);
      this.nodeToFeature.Add((object) this.ausTrackingToolManualNode, (object) AclFeature.ToolsTab_AUSTrackingManual);
      this.nodeToFeature.Add((object) this.RepAndWarrantTrackerNode, (object) AclFeature.ToolsTab_RepAndWarrantTracker);
      this.nodeToFeature.Add((object) this.ausVerificationToolNode, (object) AclFeature.ToolsTab_VerificationTracking);
      this.nodeToFeature.Add((object) this.ausVerificationToolNewNode, (object) AclFeature.ToolsTab_VerificationNew);
      this.nodeToFeature.Add((object) this.ausVerificationToolEditNode, (object) AclFeature.ToolsTab_VerificationEdit);
      this.nodeToFeature.Add((object) this.disclosureTrackingNode, (object) AclFeature.ToolsTab_DisclosureTracking);
      this.nodeToFeature.Add((object) this.feeToleranceWorksheetNode, (object) AclFeature.ToolsTab_FeeToleranceWorksheet);
      this.nodeToFeature.Add((object) this.cureToleranceAlertNode, (object) AclFeature.ToolsTab_CureToleranceAlert);
      this.nodeToFeature.Add((object) this.safeHarborDisclosureNode, (object) AclFeature.ToolsTab_SafeHarborDisclosure);
      this.nodeToFeature.Add((object) this.netTangibleBenefitNode, (object) AclFeature.ToolsTab_NetTangibleBenefit);
      this.nodeToFeature.Add((object) this.ecsDataViewerNode, (object) AclFeature.ToolsTab_ECSDataViewer);
      this.nodeToFeature.Add((object) this.dtChangeSentDateNode, (object) AclFeature.ToolsTab_DT_ChangeSentDate);
      this.nodeToFeature.Add((object) this.changeReasonsNode, (object) AclFeature.ToolsTab_DT_ChangeReasons);
      this.nodeToFeature.Add((object) this.excludeIncludeNode, (object) AclFeature.ToolsTab_DT_ExcludeIncludeRecords);
      this.nodeToFeature.Add((object) this.dtManualFullfillmentNode, (object) AclFeature.ToolsTab_DT_ManualFulfillment);
      this.nodeToFeature.Add((object) this.dtWithdrawEConsent, (object) AclFeature.ToolsTab_DT_WithdrawEConsent);
      if (this.dtCreateManualEntryNode != null)
      {
        this.nodeToFeature.Add((object) this.dtCreateManualEntryNode, (object) AclFeature.ToolsTab_DT_CreateManualEntry);
        this.nodeToFeature.Add((object) this.lESSPLSafeHarborNode, (object) AclFeature.ToolsTab_DT_LESSPLSafeHarborNode);
        this.nodeToFeature.Add((object) this.cDNode, (object) AclFeature.ToolsTab_DT_CD);
      }
      this.nodeToFeature.Add((object) this.loCompToolNode, (object) AclFeature.ToolsTab_LOCompTool);
      this.nodeToFeature.Add((object) this.loCompToolBrokerNode, (object) AclFeature.ToolsTab_LOCompBrokerTool);
      this.nodeToFeature.Add((object) this.loCompToolOfficerNode, (object) AclFeature.ToolsTab_LOCompOfficerTool);
      this.nodeToFeature.Add((object) this.showInvestorContactNode, (object) AclFeature.ToolsTab_ShowInvestorContact);
      this.nodeToFeature.Add((object) this.tqlToolNode, (object) AclFeature.ToolsTab_TQLTool);
      this.nodeToFeature.Add((object) this.tqlToolSelectInvestorNode, (object) AclFeature.ToolsTab_TQLTool_SelectInvestor);
      this.nodeToFeature.Add((object) this.tqlStartStopPublishingNode, (object) AclFeature.ToolsTab_TQLTool_StartStopPublishing);
      this.nodeToFeature.Add((object) this.tqlAddCommentsNode, (object) AclFeature.ToolsTab_TQLTool_AddComments);
      this.nodeToFeature.Add((object) this.tqlAddFraudCommentsNode, (object) AclFeature.ToolsTab_TQLTool_AddFraudComments);
      this.nodeToFeature.Add((object) this.tqlAddComplianceCommentsNode, (object) AclFeature.ToolsTab_TQLTool_AddComplianceComments);
      this.nodeToFeature.Add((object) this.tqlMiCenterToolNode, (object) AclFeature.ToolsTab_MiCenter);
      this.featureToNodeTbl = new Hashtable(FeatureSets.ToolsFeatures.Length);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_FileContacts, (object) this.fileContactsNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_BizContacts, (object) this.bizContactsNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_ConversationLog, (object) this.conversationLogNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_Task, (object) this.taskNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_Task_Add, (object) this.taskAddNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_Task_Edit, (object) this.taskEditNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_Task_Delete, (object) this.taskDeleteNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_PiggybackLoans, (object) this.piggybackLoanNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_Prequal, (object) this.prequalNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_DebtConsolidation, (object) this.debtConsolidationNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_LoanComparison, (object) this.loanComparisonNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_CashToClose, (object) this.cashToCloseNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_RentOwn, (object) this.rentOwnNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_SecureFormTransfer, (object) this.secureTransferNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_ProfitMngt, (object) this.profitMngtNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TrustAccount, (object) this.trustAccountNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_AuditTrail, (object) this.auditTrailNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_AUSTracking, (object) this.ausTrackingToolNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_AUSTrackingManual, (object) this.ausTrackingToolManualNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_RepAndWarrantTracker, (object) this.RepAndWarrantTrackerNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_VerificationTracking, (object) this.ausVerificationToolNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_VerificationNew, (object) this.ausVerificationToolNewNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_VerificationEdit, (object) this.ausVerificationToolEditNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_DisclosureTracking, (object) this.disclosureTrackingNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_FeeToleranceWorksheet, (object) this.feeToleranceWorksheetNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_CureToleranceAlert, (object) this.cureToleranceAlertNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_SafeHarborDisclosure, (object) this.safeHarborDisclosureNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_NetTangibleBenefit, (object) this.netTangibleBenefitNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_ECSDataViewer, (object) this.ecsDataViewerNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_DT_ChangeSentDate, (object) this.dtChangeSentDateNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_DT_ChangeReasons, (object) this.changeReasonsNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_DT_ExcludeIncludeRecords, (object) this.excludeIncludeNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_DT_ManualFulfillment, (object) this.dtManualFullfillmentNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_DT_WithdrawEConsent, (object) this.dtWithdrawEConsent);
      if (this.dtCreateManualEntryNode != null)
      {
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_DT_CreateManualEntry, (object) this.dtCreateManualEntryNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_DT_LESSPLSafeHarborNode, (object) this.lESSPLSafeHarborNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_DT_CD, (object) this.cDNode);
      }
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_LOCompTool, (object) this.loCompToolNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_LOCompBrokerTool, (object) this.loCompToolBrokerNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_LOCompOfficerTool, (object) this.loCompToolOfficerNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_ShowInvestorContact, (object) this.showInvestorContactNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TQLTool, (object) this.tqlToolNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TQLTool_SelectInvestor, (object) this.tqlToolSelectInvestorNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TQLTool_StartStopPublishing, (object) this.tqlStartStopPublishingNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TQLTool_AddComments, (object) this.tqlAddCommentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TQLTool_AddFraudComments, (object) this.tqlAddFraudCommentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TQLTool_AddComplianceComments, (object) this.tqlAddComplianceCommentsNode);
      this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_MiCenter, (object) this.tqlMiCenterToolNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.fileContactsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.showInvestorContactNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.bizContactsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.conversationLogNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.taskNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.taskAddNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.taskEditNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.taskDeleteNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.piggybackLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.prequalNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.debtConsolidationNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanComparisonNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.cashToCloseNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.rentOwnNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.secureTransferNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.profitMngtNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.trustAccountNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.auditTrailNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ausTrackingToolNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ausTrackingToolManualNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.RepAndWarrantTrackerNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ausVerificationToolNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ausVerificationToolNewNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ausVerificationToolEditNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.disclosureTrackingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.feeToleranceWorksheetNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.cureToleranceAlertNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.safeHarborDisclosureNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.netTangibleBenefitNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ecsDataViewerNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.dtChangeSentDateNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.changeReasonsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.excludeIncludeNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.dtManualFullfillmentNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.dtWithdrawEConsent, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loCompToolNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loCompToolBrokerNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loCompToolOfficerNode, (object) false);
      if (this.dtCreateManualEntryNode != null)
      {
        this.nodeToUpdateStatus.Add((object) this.dtCreateManualEntryNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.lESSPLSafeHarborNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.cDNode, (object) false);
      }
      this.nodeToUpdateStatus.Add((object) this.tqlToolNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tqlToolSelectInvestorNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tqlStartStopPublishingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tqlAddCommentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tqlAddFraudCommentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tqlAddComplianceCommentsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tqlMiCenterToolNode, (object) false);
      if (this.session.EncompassEdition == EncompassEdition.Banker)
      {
        this.nodeToFeature.Add((object) this.fundingWSNode, (object) AclFeature.ToolsTab_FundingWS);
        this.nodeToFeature.Add((object) this.fundingImportWSNode, (object) AclFeature.ToolsTab_FundingImportWS);
        this.nodeToFeature.Add((object) this.brokerCheckCalNode, (object) AclFeature.ToolsTab_BrokerCheckCal);
        this.nodeToFeature.Add((object) this.fundingBalWSNode, (object) AclFeature.ToolsTab_FundingBalWS);
        this.nodeToFeature.Add((object) this.secondaryRegistrationNode, (object) AclFeature.ToolsTab_SecondaryRegistration);
        this.nodeToFeature.Add((object) this.purchaseAdviceFormNode, (object) AclFeature.ToolsTab_PurchaseAdviceForm);
        this.nodeToFeature.Add((object) this.correspondentPurchaseAdviceFormNode, (object) AclFeature.ToolsTab_CorrespondentPurchaseAdviceForm);
        if (boolean)
          this.nodeToFeature.Add((object) this.correspondentComplianceReviewDataNode, (object) AclFeature.ToolsTab_CorrespondentComplianceReviewData);
        this.nodeToFeature.Add((object) this.tpoWebCenterFormNode, (object) AclFeature.ToolsTab_TPOWebCenterLoanInformation);
        this.nodeToFeature.Add((object) this.correspondentLoanStatusNode, (object) AclFeature.ToolsTab_CorrespondentLoanStatus);
        this.nodeToFeature.Add((object) this.companyInformation, (object) AclFeature.ToolsTab_TPOCompanyInformation);
        this.nodeToFeature.Add((object) this.dba, (object) AclFeature.ToolsTab_TPODBAInformation);
        this.nodeToFeature.Add((object) this.warehouse, (object) AclFeature.ToolsTab_TPOWarehouseInformation);
        this.nodeToFeature.Add((object) this.license, (object) AclFeature.ToolsTab_TPOLicenseInformation);
        this.nodeToFeature.Add((object) this.loanType, (object) AclFeature.ToolsTab_TPOLoanTypeInformation);
        this.nodeToFeature.Add((object) this.tpoContacts, (object) AclFeature.ToolsTab_TPOTPOContactsInformation);
        this.nodeToFeature.Add((object) this.LOComp, (object) AclFeature.ToolsTab_TPOLOCompInformation);
        this.nodeToFeature.Add((object) this.commitment, (object) AclFeature.ToolsTab_TPOCommitmentInformation);
        this.nodeToFeature.Add((object) this.fees, (object) AclFeature.ToolsTab_TPOFeesInformation);
        this.nodeToFeature.Add((object) this.notes, (object) AclFeature.ToolsTab_TPONotesInformation);
        this.nodeToFeature.Add((object) this.webCenterSetup, (object) AclFeature.ToolsTab_TPOWebCenterSetupInformationn);
        this.nodeToFeature.Add((object) this.tpoWebCenterDocs, (object) AclFeature.ToolsTab_TPODocsInformation);
        this.nodeToFeature.Add((object) this.attachments, (object) AclFeature.ToolsTab_TPOAttachmentsInformation);
        this.nodeToFeature.Add((object) this.salesReps, (object) AclFeature.ToolsTab_TPOSalesRepsInformation);
        this.nodeToFeature.Add((object) this.lenderContacts, (object) AclFeature.ToolsTab_TPOLenderContactsInformation);
        this.nodeToFeature.Add((object) this.customFields, (object) AclFeature.ToolsTab_TPOCustomFieldsInformation);
        this.nodeToFeature.Add((object) this.customFieldsTab1, (object) AclFeature.ToolsTab_TPOCustomFieldsTab1Information);
        this.nodeToFeature.Add((object) this.customFieldsTab2, (object) AclFeature.ToolsTab_TPOcustomFieldsTab2Information);
        this.nodeToFeature.Add((object) this.customFieldsTab3, (object) AclFeature.ToolsTab_TPOcustomFieldsTab3Information);
        this.nodeToFeature.Add((object) this.customFieldsTab4, (object) AclFeature.ToolsTab_TPOcustomFieldsTab4Information);
        this.nodeToFeature.Add((object) this.customFieldsTab5, (object) AclFeature.ToolsTab_TPOcustomFieldsTab5Information);
        this.nodeToFeature.Add((object) this.shippingDetailNode, (object) AclFeature.ToolsTab_ShippingDetail);
        this.nodeToFeature.Add((object) this.documentTrackingNode, (object) AclFeature.ToolsTab_DocumentTracking);
        this.nodeToFeature.Add((object) this.importShippingDetails, (object) AclFeature.ToolsTab_ImportShippingDetails);
        this.nodeToFeature.Add((object) this.lockRequestFormNode, (object) AclFeature.ToolsTab_LockRequestForm);
        this.nodeToFeature.Add((object) this.lockComparisonToolNode, (object) AclFeature.ToolsTab_LockComparisonTool);
        this.nodeToFeature.Add((object) this.validatePricingNode, (object) AclFeature.ToolsTab_ValidatePricing);
        this.nodeToFeature.Add((object) this.underwriterSummaryNode, (object) AclFeature.ToolsTab_UnderwriterSummary);
        this.nodeToFeature.Add((object) this.projectReview, (object) AclFeature.ToolsTab_ProjectReview);
        this.nodeToFeature.Add((object) this.pafTemplateCreateEditNode, (object) AclFeature.ToolsTab_PA_CreateEditTemplate);
        this.nodeToFeature.Add((object) this.interimServicingNode, (object) AclFeature.ToolsTab_InterimServicing);
        this.nodeToFeature.Add((object) this.isCopyLoanNode, (object) AclFeature.ToolsTab_IS_CopyLoan);
        this.nodeToFeature.Add((object) this.isEditDeleteTransactionNode, (object) AclFeature.ToolsTab_IS_EditDeleteTransaction);
        this.nodeToFeature.Add((object) this.isEnterTransactionNode, (object) AclFeature.ToolsTab_IS_EnterTransaction);
        this.nodeToFeature.Add((object) this.isReCopyLoanNode, (object) AclFeature.ToolsTab_IS_RecopyLoan);
        this.nodeToFeature.Add((object) this.amortizationScheduleNode, (object) AclFeature.ToolsTab_Amortization);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_FundingWS, (object) this.fundingWSNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_FundingImportWS, (object) this.fundingImportWSNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_BrokerCheckCal, (object) this.brokerCheckCalNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_FundingBalWS, (object) this.fundingBalWSNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_SecondaryRegistration, (object) this.secondaryRegistrationNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_PurchaseAdviceForm, (object) this.purchaseAdviceFormNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_CorrespondentPurchaseAdviceForm, (object) this.correspondentPurchaseAdviceFormNode);
        if (boolean)
          this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_CorrespondentComplianceReviewData, (object) this.correspondentComplianceReviewDataNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOWebCenterLoanInformation, (object) this.tpoWebCenterFormNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_CorrespondentLoanStatus, (object) this.correspondentLoanStatusNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOCompanyInformation, (object) this.companyInformation);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPODBAInformation, (object) this.dba);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOWarehouseInformation, (object) this.warehouse);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOLicenseInformation, (object) this.license);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOLoanTypeInformation, (object) this.loanType);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOTPOContactsInformation, (object) this.tpoContacts);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOCommitmentInformation, (object) this.commitment);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOFeesInformation, (object) this.fees);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOLOCompInformation, (object) this.LOComp);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPONotesInformation, (object) this.notes);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOWebCenterSetupInformationn, (object) this.webCenterSetup);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPODocsInformation, (object) this.tpoWebCenterDocs);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOAttachmentsInformation, (object) this.attachments);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOSalesRepsInformation, (object) this.salesReps);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOLenderContactsInformation, (object) this.lenderContacts);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOCustomFieldsInformation, (object) this.customFields);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOCustomFieldsTab1Information, (object) this.customFieldsTab1);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOcustomFieldsTab2Information, (object) this.customFieldsTab2);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOcustomFieldsTab3Information, (object) this.customFieldsTab3);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOcustomFieldsTab4Information, (object) this.customFieldsTab4);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_TPOcustomFieldsTab5Information, (object) this.customFieldsTab5);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_ShippingDetail, (object) this.shippingDetailNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_DocumentTracking, (object) this.documentTrackingNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_ImportShippingDetails, (object) this.importShippingDetails);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_LockRequestForm, (object) this.lockRequestFormNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_LockComparisonTool, (object) this.lockComparisonToolNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_ValidatePricing, (object) this.validatePricingNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_UnderwriterSummary, (object) this.underwriterSummaryNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_ProjectReview, (object) this.projectReview);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_PA_CreateEditTemplate, (object) this.pafTemplateCreateEditNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_InterimServicing, (object) this.interimServicingNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_IS_CopyLoan, (object) this.isCopyLoanNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_IS_EditDeleteTransaction, (object) this.isEditDeleteTransactionNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_IS_EnterTransaction, (object) this.isEnterTransactionNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_IS_RecopyLoan, (object) this.isReCopyLoanNode);
        this.featureToNodeTbl.Add((object) AclFeature.ToolsTab_Amortization, (object) this.amortizationScheduleNode);
        this.nodeToUpdateStatus.Add((object) this.fundingWSNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.fundingImportWSNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.brokerCheckCalNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.fundingBalWSNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.purchaseAdviceFormNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.correspondentPurchaseAdviceFormNode, (object) false);
        if (boolean)
          this.nodeToUpdateStatus.Add((object) this.correspondentComplianceReviewDataNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.tpoWebCenterFormNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.correspondentLoanStatusNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.companyInformation, (object) false);
        this.nodeToUpdateStatus.Add((object) this.dba, (object) false);
        this.nodeToUpdateStatus.Add((object) this.warehouse, (object) false);
        this.nodeToUpdateStatus.Add((object) this.license, (object) false);
        this.nodeToUpdateStatus.Add((object) this.loanType, (object) false);
        this.nodeToUpdateStatus.Add((object) this.commitment, (object) false);
        this.nodeToUpdateStatus.Add((object) this.fees, (object) false);
        this.nodeToUpdateStatus.Add((object) this.tpoContacts, (object) false);
        this.nodeToUpdateStatus.Add((object) this.LOComp, (object) false);
        this.nodeToUpdateStatus.Add((object) this.notes, (object) false);
        this.nodeToUpdateStatus.Add((object) this.webCenterSetup, (object) false);
        this.nodeToUpdateStatus.Add((object) this.tpoWebCenterDocs, (object) false);
        this.nodeToUpdateStatus.Add((object) this.attachments, (object) false);
        this.nodeToUpdateStatus.Add((object) this.salesReps, (object) false);
        this.nodeToUpdateStatus.Add((object) this.lenderContacts, (object) false);
        this.nodeToUpdateStatus.Add((object) this.customFields, (object) false);
        this.nodeToUpdateStatus.Add((object) this.customFieldsTab1, (object) false);
        this.nodeToUpdateStatus.Add((object) this.customFieldsTab2, (object) false);
        this.nodeToUpdateStatus.Add((object) this.customFieldsTab3, (object) false);
        this.nodeToUpdateStatus.Add((object) this.customFieldsTab4, (object) false);
        this.nodeToUpdateStatus.Add((object) this.customFieldsTab5, (object) false);
        this.nodeToUpdateStatus.Add((object) this.secondaryRegistrationNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.shippingDetailNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.documentTrackingNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.importShippingDetails, (object) false);
        this.nodeToUpdateStatus.Add((object) this.lockRequestFormNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.lockComparisonToolNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.validatePricingNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.underwriterSummaryNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.projectReview, (object) false);
        this.nodeToUpdateStatus.Add((object) this.pafTemplateCreateEditNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.interimServicingNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.isCopyLoanNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.isEditDeleteTransactionNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.isEnterTransactionNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.isReCopyLoanNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.amortizationScheduleNode, (object) false);
      }
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
    }
  }
}
