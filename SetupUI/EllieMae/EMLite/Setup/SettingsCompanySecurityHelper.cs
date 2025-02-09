// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingsCompanySecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SettingsCompanySecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode companySettingNode;
    protected TreeNode userGroupsNode;
    protected TreeNode businessRulesNode;
    protected TreeNode companyUserSetupNode;
    protected TreeNode companyInformationSetupNode;
    protected TreeNode ellieMaeNetworkCompanySetupNode;
    protected TreeNode servicesPasswordManagerSetupNode;
    protected TreeNode personasSetupNode;
    protected TreeNode personasEditSetupNode;
    protected TreeNode organizationsUserSetupNode;
    protected TreeNode orgUserSetupAddLEINode;
    protected TreeNode orgUserSetupEditLEINode;
    protected TreeNode rolesSetupNode;
    protected TreeNode milestonesSetupNode;
    protected TreeNode userGroupsSetupNode;
    protected TreeNode investorConnectSetupNode;
    protected TreeNode deliverLoansSetupNode;
    protected TreeNode partnerSetupSetupNode;
    protected TreeNode loanSetupNode;
    protected TreeNode autoLoanNumberingSetupNode;
    protected TreeNode autoMERSMINNumberingSetupNode;
    protected TreeNode loanFoldersSetupNode;
    protected TreeNode loanDuplicationSetupNode;
    protected TreeNode alertsSetupNode;
    protected TreeNode logSetupNode;
    protected TreeNode tasksSetupNode;
    protected TreeNode defaultInputFormsSetupNode;
    protected TreeNode conditionFormsSetupNode;
    protected TreeNode loanCustomFieldsSetupNode;
    protected TreeNode configurableKeyDatesSetupNode;
    protected TreeNode channelOptionsSetupNode;
    protected TreeNode rESPASetupNode;
    protected TreeNode changedCircumstancesSetupSetupNode;
    protected TreeNode disclosureTrackingSettingsSetupNode;
    protected TreeNode complianceCalendarSetupNode;
    protected TreeNode gfe2009PrintSetupNode;
    protected TreeNode trusteeListSetupNode;
    protected TreeNode piggybackLoanSynchronizationSetupNode;
    protected TreeNode syncTemplateSetupNode;
    protected TreeNode privacyPolicySetupNode;
    protected TreeNode zipcodeSetupSetupNode;
    protected TreeNode hmdaSetupNode;
    protected TreeNode hmdaAddLEINode;
    protected TreeNode hmdaEditLEINode;
    protected TreeNode hmdaRemoveLEINode;
    protected TreeNode nMLSReportSetupSetupNode;
    protected TreeNode verificationContactSetupSetupNode;
    protected TreeNode eFolderSetupNode;
    protected TreeNode documentConversionSetupNode;
    protected TreeNode documentsSetupNode;
    protected TreeNode documentExportTemplatesSetupNode;
    protected TreeNode documentGroupsSetupNode;
    protected TreeNode documentStackingTemplatesSetupNode;
    protected TreeNode documentIdentificationSetupNode;
    protected TreeNode documentTrainingSetupNode;
    protected TreeNode enhancedConditionsSetupNode;
    protected TreeNode enhancedConditionSetsSetupNode;
    protected TreeNode conditionTypeSettingsSetupNode;
    protected TreeNode addEditCopyConditionsSetupNode;
    protected TreeNode deleteConditionsSetupNode;
    protected TreeNode activateDeativateConditionsSetupNode;
    protected TreeNode conditionsSetupNode;
    protected TreeNode conditionSetsSetupNode;
    protected TreeNode postClosingConditionsSetupNode;
    protected TreeNode postClosingConditionSetsSetupNode;
    protected TreeNode purchaseConditionOptionsSetupNode;
    protected TreeNode purchaseConditionsSetupNode;
    protected TreeNode purchaseConditionSetsSetupNode;
    protected TreeNode hTMLEmailTemplatesSetupNode;
    protected TreeNode webCenterConfigurationSetupNode;
    protected TreeNode notificationTemplatesNode;
    protected TreeNode docsSetupNode;
    protected TreeNode eDisclosurePackagesSetupNode;
    protected TreeNode eDisclosurePlanCodesSetupNode;
    protected TreeNode eDisclosureStackingTemplatesSetupNode;
    protected TreeNode closingPlanCodesSetupNode;
    protected TreeNode closingStackingTemplatesSetupNode;
    protected TreeNode complianceAuditSettingsSetupNode;
    protected TreeNode secondaryRootSetupNode;
    protected TreeNode productandPricingSetupNode;
    protected TreeNode secondaryLockFieldsSetupNode;
    protected TreeNode lockRequestAdditionalFieldsSetupNode;
    protected TreeNode lockComparisonToolFieldsSetupNode;
    protected TreeNode investorTemplatesSetupNode;
    protected TreeNode ePPSLoanProgramNode;
    protected TreeNode tradeManagementSetupSetupNode;
    protected TreeNode normalizedBidTapeTemplateNode;
    protected TreeNode adjustmentTemplatesSetupNode;
    protected TreeNode lockDeskSetupSetupNode;
    protected TreeNode sRPTemplatesSetupNode;
    protected TreeNode fundingTemplatesSetupNode;
    protected TreeNode servicingSetupNode;
    protected TreeNode purchaseAdviceFormSetupNode;
    protected TreeNode correspondentPurchaseAdviceManagementNode;
    protected TreeNode loanPricingDecimalPlacesSetupNode;
    protected TreeNode contactSetupNode;
    protected TreeNode borrowerCustomFieldsSetupNode;
    protected TreeNode borrowerContactStatusSetupNode;
    protected TreeNode borrwerContactUpdateSetupNode;
    protected TreeNode businessCustomFieldsSetupNode;
    protected TreeNode businessCategoriesSetupNode;
    protected TreeNode publicBusinessContactGroupsSetupNode;
    protected TreeNode emailServerSettingsSetupNode;
    protected TreeNode tablesFeesSetupNode;
    protected TreeNode escrowSetupNode;
    protected TreeNode titleSetupNode;
    protected TreeNode hELOCTableSetupNode;
    protected TreeNode mITablesSetupNode;
    protected TreeNode fHACountyLimitsSetupNode;
    protected TreeNode conventionalCountyLimitsSetupNode;
    protected TreeNode fedThresholdsSetupNode;
    protected TreeNode cityTaxSetupNode;
    protected TreeNode stateTaxSetupNode;
    protected TreeNode userDefinedFeeSetupNode;
    protected TreeNode itemizationFeeManagementSetupNode;
    protected TreeNode lOCompensationSetupNode;
    protected TreeNode tmpBuydownSetupNode;
    protected TreeNode specialFeatureCodesSetupNode;
    protected TreeNode businessRulesSetupNode;
    protected TreeNode loanFolderBusinessRuleSetupNode;
    protected TreeNode milestoneCompletionSetupNode;
    protected TreeNode loanActionCompletionSetupNode;
    protected TreeNode fieldDataEntrySetupNode;
    protected TreeNode fieldTriggersSetupNode;
    protected TreeNode automatedConditionsSetupNode;
    protected TreeNode automatedEnhancedConditionsSetupNode;
    protected TreeNode automatedPurchaseConditionsSetupNode;
    protected TreeNode personaAccesstoFieldsSetupNode;
    protected TreeNode personaAccesstoLoansSetupNode;
    protected TreeNode personaAccesstoLoanActionsSetupNode;
    protected TreeNode roleAccesstoDocumentsSetupNode;
    protected TreeNode inputFormListSetupNode;
    protected TreeNode loanFormPrintingSetupNode;
    protected TreeNode printAutoSelectionSetupNode;
    protected TreeNode appraisalOrderManagementSetupNode;
    protected TreeNode lOCompensationRuleSetupNode;
    protected TreeNode DocumentTrackingSetupNode;
    protected TreeNode dynamicDataMgmtSetupNode;
    protected TreeNode feeRulesSetupNode;
    protected TreeNode fieldRulesSetupNode;
    protected TreeNode dataTablesSetupNode;
    protected TreeNode feeGroupsSetupNode;
    protected TreeNode dataPopulationTimeingSetupNode;
    protected TreeNode automatedEConsentSetupNode;
    protected TreeNode sysAdminSetupNode;
    protected TreeNode analysisToolsSetupNode;
    protected TreeNode currentLoginsSetupNode;
    protected TreeNode logUsersOutSetupNode;
    protected TreeNode disableLoginsSetupNode;
    protected TreeNode allUserInformationSetupNode;
    protected TreeNode loanReassignmentSetupNode;
    protected TreeNode unlockLoanFileSetupNode;
    protected TreeNode unlockTradeSetupNode;
    protected TreeNode systemAuditTrailSetupNode;
    protected TreeNode loanErrorInfoSetupNode;
    protected TreeNode settingsReportSetupNode;
    protected TreeNode viewSettReportSetupNode;
    protected TreeNode viewOthSettReportSetupNode;
    protected TreeNode cancelSettReportSetupNode;
    protected TreeNode cancelOthSettReportSetupNode;
    protected TreeNode delSettReportSetupNode;
    protected TreeNode addServicesSetupNode;
    protected TreeNode eDocumentManagementSetupNode;
    protected TreeNode companyStatusOnlineSetupNode;
    protected TreeNode eDisclosureFulfillmentSetupNode;
    protected TreeNode ElliemaeAIQSetupNode;
    protected TreeNode eCloseSetupNode;
    protected TreeNode complianceReviewSetupSetupNode;
    protected TreeNode tservice4506SetupNode;
    protected TreeNode tQLServicesSetupNode;
    protected TreeNode appraisalServiceSetupNode;
    protected TreeNode titleServiceSetupNode;
    protected TreeNode fraudServiceSetupNode;
    protected TreeNode fannieServiceSetupNode;
    protected TreeNode freddieServiceSetupNode;
    protected TreeNode miServiceSetupNode;
    protected TreeNode freddieMacCACServiceSetupNode;
    protected TreeNode fannieMaeUCDTransferServiceSetupNode;
    protected TreeNode ginnieMaePddServiceSetup;
    protected TreeNode valuationServiceSetupNode;
    protected TreeNode floodServiceSetupNode;
    protected TreeNode dataTracConnectionSetupNode;
    protected TreeNode insightsSetupNode;
    protected TreeNode personalTemplatesSetupNode;
    protected TreeNode customPrintFormsSetupNode;
    protected TreeNode printFormGroupsSetupNode;
    protected TreeNode loanProgramsSetupNode;
    protected TreeNode closingCostsSetupNode;
    protected TreeNode inputFormSetsSetupNode;
    protected TreeNode settlementServiceProvidersSetupNode;
    protected TreeNode affiliatedBusinessArrangementTemplatesSetupNode;
    protected TreeNode documentSetsSetupNode;
    protected TreeNode taskSetsSetupNode;
    protected TreeNode dataTemplatesSetupNode;
    protected TreeNode loanTemplateSetsSetupNode;
    protected TreeNode irs4506TTemplatesSetupNode;
    protected TreeNode autoLockSetupNode;

    public SettingsCompanySecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.SettingsTabCompanyFeatures);
    }

    public SettingsCompanySecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.SettingsTabCompanyFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      bool flag = !Convert.ToBoolean(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "EnableBidTape"));
      this.companySettingNode = new TreeNode("Access to Company Settings");
      this.userGroupsNode = new TreeNode("User Groups");
      this.businessRulesNode = new TreeNode("Business Rules");
      this.companyUserSetupNode = new TreeNode("Company/User Setup");
      this.companyInformationSetupNode = new TreeNode("Company Information");
      this.ellieMaeNetworkCompanySetupNode = new TreeNode("ICE Mortgage Technology Network Company");
      this.servicesPasswordManagerSetupNode = new TreeNode("Services Password Manager");
      this.personasSetupNode = new TreeNode("Personas");
      this.personasEditSetupNode = new TreeNode("Edit");
      this.organizationsUserSetupNode = new TreeNode("Organizations/User");
      this.orgUserSetupAddLEINode = new TreeNode("Add LEI");
      this.orgUserSetupEditLEINode = new TreeNode("Edit LEI");
      this.rolesSetupNode = new TreeNode("Roles");
      this.milestonesSetupNode = new TreeNode("Milestones");
      this.userGroupsSetupNode = new TreeNode("User Groups");
      this.investorConnectSetupNode = new TreeNode("Investor Connect Setup");
      this.deliverLoansSetupNode = new TreeNode("Deliver Loans");
      this.partnerSetupSetupNode = new TreeNode("Partner Setup");
      this.loanSetupNode = new TreeNode("Loan Setup");
      this.autoLoanNumberingSetupNode = new TreeNode("Auto Loan Numbering");
      this.autoMERSMINNumberingSetupNode = new TreeNode("Auto MERS MIN Numbering");
      this.loanFoldersSetupNode = new TreeNode("Loan Folders");
      this.loanDuplicationSetupNode = new TreeNode("Loan Duplication");
      this.alertsSetupNode = new TreeNode("Alerts");
      this.logSetupNode = new TreeNode("Log");
      this.tasksSetupNode = new TreeNode("Tasks");
      this.defaultInputFormsSetupNode = new TreeNode("Default Input Forms");
      this.conditionFormsSetupNode = new TreeNode("Condition Forms");
      this.loanCustomFieldsSetupNode = new TreeNode("Loan Custom Fields");
      this.configurableKeyDatesSetupNode = new TreeNode("Configurable Key Dates");
      this.channelOptionsSetupNode = new TreeNode("Channel Options");
      this.rESPASetupNode = new TreeNode("RESPA");
      this.changedCircumstancesSetupSetupNode = new TreeNode("Changed Circumstances Setup");
      this.disclosureTrackingSettingsSetupNode = new TreeNode("Disclosure Tracking Settings");
      this.complianceCalendarSetupNode = new TreeNode("Compliance Calendar");
      this.gfe2009PrintSetupNode = new TreeNode("2009 GFE Print");
      this.trusteeListSetupNode = new TreeNode("Trustee List");
      this.piggybackLoanSynchronizationSetupNode = new TreeNode("Piggyback Loan Synchronization");
      this.syncTemplateSetupNode = new TreeNode("Sync Templates");
      this.privacyPolicySetupNode = new TreeNode("Privacy Policy");
      this.zipcodeSetupSetupNode = new TreeNode("Zipcode Setup");
      this.hmdaSetupNode = new TreeNode("HMDA Profiles");
      this.hmdaAddLEINode = new TreeNode("Add HMDA Profile");
      this.hmdaEditLEINode = new TreeNode("Edit HMDA Profile");
      this.hmdaRemoveLEINode = new TreeNode("Delete HMDA Profile");
      this.nMLSReportSetupSetupNode = new TreeNode("NMLS Report Setup");
      this.verificationContactSetupSetupNode = new TreeNode("Verification Contact Setup");
      this.eFolderSetupNode = new TreeNode("eFolder Setup");
      this.documentConversionSetupNode = new TreeNode("Document Conversion");
      this.documentsSetupNode = new TreeNode("Documents");
      this.documentExportTemplatesSetupNode = new TreeNode("Document Export Templates");
      this.documentGroupsSetupNode = new TreeNode("Document Groups");
      this.documentStackingTemplatesSetupNode = new TreeNode("Document Stacking Templates");
      this.documentIdentificationSetupNode = new TreeNode("Document Identification");
      this.documentTrainingSetupNode = new TreeNode("Document Training");
      this.enhancedConditionsSetupNode = new TreeNode("Enhanced Conditions");
      this.enhancedConditionSetsSetupNode = new TreeNode("Enhanced Condition Sets");
      this.conditionTypeSettingsSetupNode = new TreeNode("Condition Type Settings");
      this.addEditCopyConditionsSetupNode = new TreeNode("Add/Edit/Duplicate Condition");
      this.deleteConditionsSetupNode = new TreeNode("Delete Condition");
      this.activateDeativateConditionsSetupNode = new TreeNode("Activate/Deactivate Condition");
      this.conditionsSetupNode = new TreeNode("Conditions");
      this.conditionSetsSetupNode = new TreeNode("Condition Sets");
      this.postClosingConditionsSetupNode = new TreeNode("Post Closing Conditions");
      this.postClosingConditionSetsSetupNode = new TreeNode("Post-Closing Condition Sets");
      this.purchaseConditionOptionsSetupNode = new TreeNode("Purchase Condition Options");
      this.purchaseConditionsSetupNode = new TreeNode("Purchase Conditions");
      this.purchaseConditionSetsSetupNode = new TreeNode("Purchase Condition Sets");
      this.hTMLEmailTemplatesSetupNode = new TreeNode("HTML Email Templates");
      this.webCenterConfigurationSetupNode = new TreeNode("WebCenter Configuration");
      this.notificationTemplatesNode = new TreeNode("Notification Templates");
      this.docsSetupNode = new TreeNode("Docs Setup");
      this.eDisclosurePackagesSetupNode = new TreeNode("eDisclosure Packages");
      this.eDisclosurePlanCodesSetupNode = new TreeNode("eDisclosure Plan Codes");
      this.eDisclosureStackingTemplatesSetupNode = new TreeNode("eDisclosure Stacking Templates");
      this.closingPlanCodesSetupNode = new TreeNode("Closing Plan Codes");
      this.closingStackingTemplatesSetupNode = new TreeNode("Closing Stacking Templates");
      this.complianceAuditSettingsSetupNode = new TreeNode("Compliance Audit Settings");
      this.secondaryRootSetupNode = new TreeNode("Secondary Setup");
      this.productandPricingSetupNode = new TreeNode("Product and Pricing");
      this.secondaryLockFieldsSetupNode = new TreeNode("Secondary Lock Fields");
      this.lockRequestAdditionalFieldsSetupNode = new TreeNode("Lock Request Additional Fields");
      this.lockComparisonToolFieldsSetupNode = new TreeNode("Lock Comparison Tool Fields");
      this.investorTemplatesSetupNode = new TreeNode("Investor Templates");
      this.ePPSLoanProgramNode = new TreeNode("ICE PPE Loan Program Table");
      this.tradeManagementSetupSetupNode = new TreeNode("Trade Management Setup");
      if (!flag)
        this.normalizedBidTapeTemplateNode = new TreeNode("Normalized Bid Tape Template");
      this.adjustmentTemplatesSetupNode = new TreeNode("Adjustment Templates");
      this.lockDeskSetupSetupNode = new TreeNode("Lock Desk Setup");
      this.sRPTemplatesSetupNode = new TreeNode("SRP Templates");
      this.fundingTemplatesSetupNode = new TreeNode("Funding Templates");
      this.servicingSetupNode = new TreeNode("Servicing");
      this.purchaseAdviceFormSetupNode = new TreeNode("Purchase Advice Form");
      this.correspondentPurchaseAdviceManagementNode = new TreeNode("Correspondent Purchase Advice Management Setup");
      this.loanPricingDecimalPlacesSetupNode = new TreeNode("Loan Pricing Decimal Places");
      this.autoLockSetupNode = new TreeNode("Auto-Lock");
      this.contactSetupNode = new TreeNode("Contact Setup");
      this.borrowerCustomFieldsSetupNode = new TreeNode("Borrower Custom Fields");
      this.borrowerContactStatusSetupNode = new TreeNode("Borrower Contact Status");
      this.borrwerContactUpdateSetupNode = new TreeNode("Borrwer Contact Update");
      this.businessCustomFieldsSetupNode = new TreeNode("Business Custom Fields");
      this.businessCategoriesSetupNode = new TreeNode("Business Categories");
      this.publicBusinessContactGroupsSetupNode = new TreeNode("Public Business Contact Groups");
      this.emailServerSettingsSetupNode = new TreeNode("Email Server Settings");
      this.tablesFeesSetupNode = new TreeNode("Tables and Fees");
      this.escrowSetupNode = new TreeNode("Escrow");
      this.titleSetupNode = new TreeNode("Title");
      this.hELOCTableSetupNode = new TreeNode("HELOC Table");
      this.mITablesSetupNode = new TreeNode("MI Tables");
      this.fHACountyLimitsSetupNode = new TreeNode("FHA County Limits");
      this.conventionalCountyLimitsSetupNode = new TreeNode("Conventional County Limits");
      this.fedThresholdsSetupNode = new TreeNode("Federal Threshold Adjustments");
      this.cityTaxSetupNode = new TreeNode("City Tax");
      this.stateTaxSetupNode = new TreeNode("State Tax");
      this.userDefinedFeeSetupNode = new TreeNode("User Defined Fee");
      this.itemizationFeeManagementSetupNode = new TreeNode("Itemization Fee Management");
      this.lOCompensationSetupNode = new TreeNode("LO Compensation");
      this.tmpBuydownSetupNode = new TreeNode("Temporary Buydown");
      this.specialFeatureCodesSetupNode = new TreeNode("Special Feature Codes");
      this.businessRulesSetupNode = new TreeNode("Business Rules");
      this.loanFolderBusinessRuleSetupNode = new TreeNode("Loan Folder Business Rule");
      this.milestoneCompletionSetupNode = new TreeNode("Milestone Completion");
      this.loanActionCompletionSetupNode = new TreeNode("Loan Action Completion");
      this.fieldDataEntrySetupNode = new TreeNode("Field Data Entry");
      this.fieldTriggersSetupNode = new TreeNode("Field Triggers");
      this.automatedConditionsSetupNode = new TreeNode("Automated Conditions");
      this.automatedEnhancedConditionsSetupNode = new TreeNode("Automated Enhanced Conditions");
      this.automatedPurchaseConditionsSetupNode = new TreeNode("Automated Purchase Conditions");
      this.personaAccesstoFieldsSetupNode = new TreeNode("Persona Access to Fields");
      this.personaAccesstoLoansSetupNode = new TreeNode("Persona Access to Loans");
      this.personaAccesstoLoanActionsSetupNode = new TreeNode("Persona Access to Loan Actions");
      this.roleAccesstoDocumentsSetupNode = new TreeNode("Role Access to Documents");
      this.inputFormListSetupNode = new TreeNode("Input Form List");
      this.loanFormPrintingSetupNode = new TreeNode("Loan Form Printing");
      this.printAutoSelectionSetupNode = new TreeNode("Print Auto Selection");
      this.appraisalOrderManagementSetupNode = new TreeNode("Appraisal Order Management");
      this.lOCompensationRuleSetupNode = new TreeNode("LO Compensation Rule");
      this.DocumentTrackingSetupNode = new TreeNode("Collateral Tracking");
      this.dynamicDataMgmtSetupNode = new TreeNode("Dynamic Data Management");
      this.feeRulesSetupNode = new TreeNode("Fee Rules");
      this.fieldRulesSetupNode = new TreeNode("Field Rules");
      this.dataTablesSetupNode = new TreeNode("Data Tables");
      this.feeGroupsSetupNode = new TreeNode("Fee Groups");
      this.dataPopulationTimeingSetupNode = new TreeNode("Global DDM Settings");
      this.automatedEConsentSetupNode = new TreeNode("Automated Request for eConsent");
      this.loanErrorInfoSetupNode = new TreeNode("Loan Error Information");
      this.sysAdminSetupNode = new TreeNode("System Administration");
      this.analysisToolsSetupNode = new TreeNode("Analysis Tools");
      this.currentLoginsSetupNode = new TreeNode("Current Logins");
      this.logUsersOutSetupNode = new TreeNode("Log Users Out");
      this.disableLoginsSetupNode = new TreeNode("Disable Logins");
      this.allUserInformationSetupNode = new TreeNode("All User Information");
      this.settingsReportSetupNode = new TreeNode("Settings Reports");
      this.viewSettReportSetupNode = new TreeNode("Generate & View Settings Reports");
      this.viewOthSettReportSetupNode = new TreeNode("View Settings Reports submitted by others");
      this.cancelSettReportSetupNode = new TreeNode("Cancel Settings Reports");
      this.cancelOthSettReportSetupNode = new TreeNode("Cancel Settings Reports submitted by others");
      this.delSettReportSetupNode = new TreeNode("Delete Settings Reports");
      this.loanReassignmentSetupNode = new TreeNode("Loan Reassignment");
      this.unlockLoanFileSetupNode = new TreeNode("Unlock Loan File");
      this.unlockTradeSetupNode = new TreeNode("Unlock Trade");
      this.systemAuditTrailSetupNode = new TreeNode("System Audit Trail");
      this.addServicesSetupNode = new TreeNode("Additional Services");
      this.eDocumentManagementSetupNode = new TreeNode("E-Document Management");
      this.companyStatusOnlineSetupNode = new TreeNode("Company Status Online");
      this.eDisclosureFulfillmentSetupNode = new TreeNode("eDisclosure Fulfillment");
      this.ElliemaeAIQSetupNode = new TreeNode("Data & Document Automation and Mortgage Analyzers");
      this.eCloseSetupNode = new TreeNode("Encompass eClose Setup");
      this.complianceReviewSetupSetupNode = new TreeNode("Compliance Review Setup");
      this.tservice4506SetupNode = new TreeNode("4506T Service");
      this.tQLServicesSetupNode = new TreeNode("TQL Services");
      this.appraisalServiceSetupNode = new TreeNode("Appraisal Service");
      this.titleServiceSetupNode = new TreeNode("Title Service");
      this.fraudServiceSetupNode = new TreeNode("Fraud Service");
      this.fannieServiceSetupNode = new TreeNode("Fannie Mae Workflow");
      this.freddieServiceSetupNode = new TreeNode("Freddie Mac Order Alert");
      this.miServiceSetupNode = new TreeNode("Mortgage Insurance Service");
      this.freddieMacCACServiceSetupNode = new TreeNode("Freddie Mac Loan Assignment");
      this.fannieMaeUCDTransferServiceSetupNode = new TreeNode("Fannie Mae UCD Transfer");
      this.ginnieMaePddServiceSetup = new TreeNode("Ginnie Mae PDD Service");
      this.valuationServiceSetupNode = new TreeNode("Valuation Service");
      this.floodServiceSetupNode = new TreeNode("Flood Service");
      this.dataTracConnectionSetupNode = new TreeNode("DataTrac Connection");
      this.insightsSetupNode = new TreeNode("Insights Setup");
      this.personalTemplatesSetupNode = new TreeNode("Personal Templates");
      this.customPrintFormsSetupNode = new TreeNode("Custom Print Forms");
      this.printFormGroupsSetupNode = new TreeNode("Print Form Groups");
      this.loanProgramsSetupNode = new TreeNode("Loan Programs");
      this.closingCostsSetupNode = new TreeNode("Closing Costs");
      this.inputFormSetsSetupNode = new TreeNode("Input Form Sets");
      this.settlementServiceProvidersSetupNode = new TreeNode("Settlement Service Providers");
      this.affiliatedBusinessArrangementTemplatesSetupNode = new TreeNode("Affiliated Business Arrangement Templates");
      this.documentSetsSetupNode = new TreeNode("Document Sets");
      this.taskSetsSetupNode = new TreeNode("Task Sets");
      this.dataTemplatesSetupNode = new TreeNode("Data Templates");
      this.loanTemplateSetsSetupNode = new TreeNode("Loan Template Sets");
      this.irs4506TTemplatesSetupNode = new TreeNode("Transcript Request Templates");
      this.companyUserSetupNode.Nodes.AddRange(new TreeNode[8]
      {
        this.companyInformationSetupNode,
        this.ellieMaeNetworkCompanySetupNode,
        this.servicesPasswordManagerSetupNode,
        this.personasSetupNode,
        this.organizationsUserSetupNode,
        this.rolesSetupNode,
        this.milestonesSetupNode,
        this.userGroupsSetupNode
      });
      this.personasSetupNode.Nodes.AddRange(new TreeNode[1]
      {
        this.personasEditSetupNode
      });
      this.organizationsUserSetupNode.Nodes.AddRange(new TreeNode[2]
      {
        this.orgUserSetupAddLEINode,
        this.orgUserSetupEditLEINode
      });
      this.investorConnectSetupNode.Nodes.AddRange(new TreeNode[2]
      {
        this.deliverLoansSetupNode,
        this.partnerSetupSetupNode
      });
      this.hmdaSetupNode.Nodes.AddRange(new TreeNode[3]
      {
        this.hmdaAddLEINode,
        this.hmdaEditLEINode,
        this.hmdaRemoveLEINode
      });
      if ((bool) this.session.ServerManager.GetServerSetting("Policies.AllowConfigurableKeyDates"))
        this.loanSetupNode.Nodes.AddRange(new TreeNode[25]
        {
          this.autoLoanNumberingSetupNode,
          this.autoMERSMINNumberingSetupNode,
          this.loanFoldersSetupNode,
          this.loanDuplicationSetupNode,
          this.alertsSetupNode,
          this.logSetupNode,
          this.tasksSetupNode,
          this.defaultInputFormsSetupNode,
          this.conditionFormsSetupNode,
          this.loanCustomFieldsSetupNode,
          this.configurableKeyDatesSetupNode,
          this.channelOptionsSetupNode,
          this.rESPASetupNode,
          this.changedCircumstancesSetupSetupNode,
          this.disclosureTrackingSettingsSetupNode,
          this.complianceCalendarSetupNode,
          this.gfe2009PrintSetupNode,
          this.trusteeListSetupNode,
          this.piggybackLoanSynchronizationSetupNode,
          this.syncTemplateSetupNode,
          this.privacyPolicySetupNode,
          this.zipcodeSetupSetupNode,
          this.hmdaSetupNode,
          this.nMLSReportSetupSetupNode,
          this.verificationContactSetupSetupNode
        });
      else
        this.loanSetupNode.Nodes.AddRange(new TreeNode[24]
        {
          this.autoLoanNumberingSetupNode,
          this.autoMERSMINNumberingSetupNode,
          this.loanFoldersSetupNode,
          this.loanDuplicationSetupNode,
          this.alertsSetupNode,
          this.logSetupNode,
          this.tasksSetupNode,
          this.defaultInputFormsSetupNode,
          this.conditionFormsSetupNode,
          this.loanCustomFieldsSetupNode,
          this.channelOptionsSetupNode,
          this.rESPASetupNode,
          this.changedCircumstancesSetupSetupNode,
          this.disclosureTrackingSettingsSetupNode,
          this.complianceCalendarSetupNode,
          this.gfe2009PrintSetupNode,
          this.trusteeListSetupNode,
          this.piggybackLoanSynchronizationSetupNode,
          this.syncTemplateSetupNode,
          this.privacyPolicySetupNode,
          this.zipcodeSetupSetupNode,
          this.hmdaSetupNode,
          this.nMLSReportSetupSetupNode,
          this.verificationContactSetupSetupNode
        });
      if ((bool) this.session.ServerManager.GetServerSetting("CLIENT.ENHANCEDCONDITIONSETTINGS"))
        this.eFolderSetupNode.Nodes.AddRange(new TreeNode[13]
        {
          this.documentConversionSetupNode,
          this.documentsSetupNode,
          this.documentExportTemplatesSetupNode,
          this.documentGroupsSetupNode,
          this.documentStackingTemplatesSetupNode,
          this.documentIdentificationSetupNode,
          this.documentTrainingSetupNode,
          this.enhancedConditionsSetupNode,
          this.enhancedConditionSetsSetupNode,
          this.conditionsSetupNode,
          this.conditionSetsSetupNode,
          this.postClosingConditionsSetupNode,
          this.postClosingConditionSetsSetupNode
        });
      else
        this.eFolderSetupNode.Nodes.AddRange(new TreeNode[11]
        {
          this.documentConversionSetupNode,
          this.documentsSetupNode,
          this.documentExportTemplatesSetupNode,
          this.documentGroupsSetupNode,
          this.documentStackingTemplatesSetupNode,
          this.documentIdentificationSetupNode,
          this.documentTrainingSetupNode,
          this.conditionsSetupNode,
          this.conditionSetsSetupNode,
          this.postClosingConditionsSetupNode,
          this.postClosingConditionSetsSetupNode
        });
      if ((bool) this.session.ServerManager.GetServerSetting("Policies.AllowPurchaseCondition"))
        this.eFolderSetupNode.Nodes.AddRange(new TreeNode[5]
        {
          this.purchaseConditionOptionsSetupNode,
          this.purchaseConditionsSetupNode,
          this.purchaseConditionSetsSetupNode,
          this.hTMLEmailTemplatesSetupNode,
          this.webCenterConfigurationSetupNode
        });
      else
        this.eFolderSetupNode.Nodes.AddRange(new TreeNode[2]
        {
          this.hTMLEmailTemplatesSetupNode,
          this.webCenterConfigurationSetupNode
        });
      this.eFolderSetupNode.Nodes.AddRange(new TreeNode[1]
      {
        this.notificationTemplatesNode
      });
      this.docsSetupNode.Nodes.AddRange(new TreeNode[6]
      {
        this.eDisclosurePackagesSetupNode,
        this.eDisclosurePlanCodesSetupNode,
        this.eDisclosureStackingTemplatesSetupNode,
        this.closingPlanCodesSetupNode,
        this.closingStackingTemplatesSetupNode,
        this.complianceAuditSettingsSetupNode
      });
      this.secondaryRootSetupNode.Nodes.AddRange(new TreeNode[3]
      {
        this.productandPricingSetupNode,
        this.secondaryLockFieldsSetupNode,
        this.lockRequestAdditionalFieldsSetupNode
      });
      this.secondaryRootSetupNode.Nodes.Add(this.lockComparisonToolFieldsSetupNode);
      this.secondaryRootSetupNode.Nodes.AddRange(new TreeNode[4]
      {
        this.autoLockSetupNode,
        this.investorTemplatesSetupNode,
        this.ePPSLoanProgramNode,
        this.tradeManagementSetupSetupNode
      });
      if (!flag)
        this.secondaryRootSetupNode.Nodes.Add(this.normalizedBidTapeTemplateNode);
      this.secondaryRootSetupNode.Nodes.AddRange(new TreeNode[8]
      {
        this.adjustmentTemplatesSetupNode,
        this.lockDeskSetupSetupNode,
        this.sRPTemplatesSetupNode,
        this.fundingTemplatesSetupNode,
        this.servicingSetupNode,
        this.purchaseAdviceFormSetupNode,
        this.correspondentPurchaseAdviceManagementNode,
        this.loanPricingDecimalPlacesSetupNode
      });
      this.contactSetupNode.Nodes.AddRange(new TreeNode[7]
      {
        this.borrowerCustomFieldsSetupNode,
        this.borrowerContactStatusSetupNode,
        this.borrwerContactUpdateSetupNode,
        this.businessCustomFieldsSetupNode,
        this.businessCategoriesSetupNode,
        this.publicBusinessContactGroupsSetupNode,
        this.emailServerSettingsSetupNode
      });
      this.tablesFeesSetupNode.Nodes.AddRange(new TreeNode[14]
      {
        this.escrowSetupNode,
        this.titleSetupNode,
        this.hELOCTableSetupNode,
        this.mITablesSetupNode,
        this.fHACountyLimitsSetupNode,
        this.conventionalCountyLimitsSetupNode,
        this.fedThresholdsSetupNode,
        this.cityTaxSetupNode,
        this.stateTaxSetupNode,
        this.userDefinedFeeSetupNode,
        this.itemizationFeeManagementSetupNode,
        this.lOCompensationSetupNode,
        this.tmpBuydownSetupNode,
        this.specialFeatureCodesSetupNode
      });
      this.session.IsBankerEdition();
      this.businessRulesSetupNode.Nodes.AddRange(new TreeNode[16]
      {
        this.loanFolderBusinessRuleSetupNode,
        this.milestoneCompletionSetupNode,
        this.loanActionCompletionSetupNode,
        this.fieldDataEntrySetupNode,
        this.fieldTriggersSetupNode,
        this.automatedConditionsSetupNode,
        this.personaAccesstoFieldsSetupNode,
        this.personaAccesstoLoansSetupNode,
        this.personaAccesstoLoanActionsSetupNode,
        this.roleAccesstoDocumentsSetupNode,
        this.inputFormListSetupNode,
        this.loanFormPrintingSetupNode,
        this.printAutoSelectionSetupNode,
        this.appraisalOrderManagementSetupNode,
        this.lOCompensationRuleSetupNode,
        this.DocumentTrackingSetupNode
      });
      int index = this.businessRulesSetupNode.Nodes.IndexOf(this.automatedConditionsSetupNode);
      if ((bool) this.session.ServerManager.GetServerSetting("CLIENT.ENHANCEDCONDITIONSETTINGS"))
        this.businessRulesSetupNode.Nodes.Insert(++index, this.automatedEnhancedConditionsSetupNode);
      if ((bool) this.session.ServerManager.GetServerSetting("Policies.AllowPurchaseCondition"))
        this.businessRulesSetupNode.Nodes.Insert(index, this.automatedPurchaseConditionsSetupNode);
      this.cancelSettReportSetupNode.Nodes.AddRange(new TreeNode[1]
      {
        this.cancelOthSettReportSetupNode
      });
      this.dynamicDataMgmtSetupNode.Nodes.AddRange(new TreeNode[4]
      {
        this.feeRulesSetupNode,
        this.fieldRulesSetupNode,
        this.dataTablesSetupNode,
        this.dataPopulationTimeingSetupNode
      });
      this.sysAdminSetupNode.Nodes.AddRange(new TreeNode[9]
      {
        this.analysisToolsSetupNode,
        this.currentLoginsSetupNode,
        this.allUserInformationSetupNode,
        this.settingsReportSetupNode,
        this.loanReassignmentSetupNode,
        this.unlockLoanFileSetupNode,
        this.unlockTradeSetupNode,
        this.systemAuditTrailSetupNode,
        this.loanErrorInfoSetupNode
      });
      if (!this.session.StartupInfo.AllowLoanErrorInfo)
        treeView.Nodes.Remove(this.loanErrorInfoSetupNode);
      this.settingsReportSetupNode.Nodes.AddRange(new TreeNode[4]
      {
        this.viewSettReportSetupNode,
        this.viewOthSettReportSetupNode,
        this.cancelSettReportSetupNode,
        this.delSettReportSetupNode
      });
      this.currentLoginsSetupNode.Nodes.AddRange(new TreeNode[2]
      {
        this.logUsersOutSetupNode,
        this.disableLoginsSetupNode
      });
      this.enhancedConditionsSetupNode.Nodes.AddRange(new TreeNode[4]
      {
        this.conditionTypeSettingsSetupNode,
        this.addEditCopyConditionsSetupNode,
        this.deleteConditionsSetupNode,
        this.activateDeativateConditionsSetupNode
      });
      this.addServicesSetupNode.Nodes.AddRange(new TreeNode[21]
      {
        this.eDocumentManagementSetupNode,
        this.companyStatusOnlineSetupNode,
        this.eDisclosureFulfillmentSetupNode,
        this.eCloseSetupNode,
        this.ElliemaeAIQSetupNode,
        this.complianceReviewSetupSetupNode,
        this.tservice4506SetupNode,
        this.tQLServicesSetupNode,
        this.appraisalServiceSetupNode,
        this.titleServiceSetupNode,
        this.fraudServiceSetupNode,
        this.fannieServiceSetupNode,
        this.fannieMaeUCDTransferServiceSetupNode,
        this.ginnieMaePddServiceSetup,
        this.freddieServiceSetupNode,
        this.freddieMacCACServiceSetupNode,
        this.miServiceSetupNode,
        this.valuationServiceSetupNode,
        this.floodServiceSetupNode,
        this.dataTracConnectionSetupNode,
        this.insightsSetupNode
      });
      this.personalTemplatesSetupNode.Nodes.AddRange(new TreeNode[11]
      {
        this.customPrintFormsSetupNode,
        this.printFormGroupsSetupNode,
        this.loanProgramsSetupNode,
        this.closingCostsSetupNode,
        this.inputFormSetsSetupNode,
        this.settlementServiceProvidersSetupNode,
        this.affiliatedBusinessArrangementTemplatesSetupNode,
        this.documentSetsSetupNode,
        this.taskSetsSetupNode,
        this.dataTemplatesSetupNode,
        this.loanTemplateSetsSetupNode
      });
      if (this.session.EncompassEdition == EncompassEdition.Banker)
        treeView.Nodes.AddRange(new TreeNode[14]
        {
          this.companyUserSetupNode,
          this.investorConnectSetupNode,
          this.loanSetupNode,
          this.eFolderSetupNode,
          this.docsSetupNode,
          this.secondaryRootSetupNode,
          this.contactSetupNode,
          this.tablesFeesSetupNode,
          this.businessRulesSetupNode,
          this.dynamicDataMgmtSetupNode,
          this.automatedEConsentSetupNode,
          this.sysAdminSetupNode,
          this.addServicesSetupNode,
          this.personalTemplatesSetupNode
        });
      else
        treeView.Nodes.AddRange(new TreeNode[13]
        {
          this.companyUserSetupNode,
          this.loanSetupNode,
          this.eFolderSetupNode,
          this.docsSetupNode,
          this.secondaryRootSetupNode,
          this.contactSetupNode,
          this.tablesFeesSetupNode,
          this.businessRulesSetupNode,
          this.dynamicDataMgmtSetupNode,
          this.automatedEConsentSetupNode,
          this.sysAdminSetupNode,
          this.addServicesSetupNode,
          this.personalTemplatesSetupNode
        });
      if (!this.session.StartupInfo.AllowDDM)
        treeView.Nodes.Remove(this.dynamicDataMgmtSetupNode);
      if (!this.session.StartupInfo.AllowAutoEConsent)
        treeView.Nodes.Remove(this.automatedEConsentSetupNode);
      if (!this.session.StartupInfo.AllowInsightsSetup || this.session.IsBrokerEdition())
        treeView.Nodes.Remove(this.insightsSetupNode);
      treeView.ExpandAll();
      this.dependentNodes.Add((object) this.settingsReportSetupNode);
      this.nodeToFeature = new Hashtable(FeatureSets.SettingsTabCompanyFeatures.Length);
      this.nodeToFeature.Add((object) this.companyUserSetupNode, (object) AclFeature.SettingsTab_CompanyUser);
      this.nodeToFeature.Add((object) this.companyInformationSetupNode, (object) AclFeature.SettingsTab_CompanyInformation);
      this.nodeToFeature.Add((object) this.ellieMaeNetworkCompanySetupNode, (object) AclFeature.SettingsTab_EllieMaeNetworkCompany);
      this.nodeToFeature.Add((object) this.servicesPasswordManagerSetupNode, (object) AclFeature.SettingsTab_ServicesPasswordManager);
      this.nodeToFeature.Add((object) this.personasSetupNode, (object) AclFeature.SettingsTab_Personas);
      this.nodeToFeature.Add((object) this.personasEditSetupNode, (object) AclFeature.SettingsTab_PersonasEdit);
      this.nodeToFeature.Add((object) this.organizationsUserSetupNode, (object) AclFeature.SettingsTab_OrganizationsUser);
      this.nodeToFeature.Add((object) this.orgUserSetupAddLEINode, (object) AclFeature.SettingsTab_OrgUserAddLEI);
      this.nodeToFeature.Add((object) this.orgUserSetupEditLEINode, (object) AclFeature.SettingsTab_OrgUserEditLEI);
      this.nodeToFeature.Add((object) this.rolesSetupNode, (object) AclFeature.SettingsTab_Roles);
      this.nodeToFeature.Add((object) this.milestonesSetupNode, (object) AclFeature.SettingsTab_Milestones);
      this.nodeToFeature.Add((object) this.userGroupsSetupNode, (object) AclFeature.SettingsTab_UserGroups);
      this.nodeToFeature.Add((object) this.investorConnectSetupNode, (object) AclFeature.SettingsTab_InvestorConnectSetup);
      this.nodeToFeature.Add((object) this.deliverLoansSetupNode, (object) AclFeature.SettingsTab_DeliverLoans);
      this.nodeToFeature.Add((object) this.partnerSetupSetupNode, (object) AclFeature.SettingsTab_PartnerSetup);
      this.nodeToFeature.Add((object) this.loanSetupNode, (object) AclFeature.SettingsTab_Loan);
      this.nodeToFeature.Add((object) this.autoLoanNumberingSetupNode, (object) AclFeature.SettingsTab_AutoLoanNumbering);
      this.nodeToFeature.Add((object) this.autoMERSMINNumberingSetupNode, (object) AclFeature.SettingsTab_AutoMERSMINNumbering);
      this.nodeToFeature.Add((object) this.loanFoldersSetupNode, (object) AclFeature.SettingsTab_LoanFolders);
      this.nodeToFeature.Add((object) this.loanDuplicationSetupNode, (object) AclFeature.SettingsTab_LoanDuplication);
      this.nodeToFeature.Add((object) this.alertsSetupNode, (object) AclFeature.SettingsTab_Alerts);
      this.nodeToFeature.Add((object) this.logSetupNode, (object) AclFeature.SettingsTab_Log);
      this.nodeToFeature.Add((object) this.tasksSetupNode, (object) AclFeature.SettingsTab_Tasks);
      this.nodeToFeature.Add((object) this.defaultInputFormsSetupNode, (object) AclFeature.SettingsTab_DefaultInputForms);
      this.nodeToFeature.Add((object) this.conditionFormsSetupNode, (object) AclFeature.SettingsTab_Company_ConditionalApprovalLetter);
      this.nodeToFeature.Add((object) this.loanCustomFieldsSetupNode, (object) AclFeature.SettingsTab_Company_LoanCustomFields);
      this.nodeToFeature.Add((object) this.configurableKeyDatesSetupNode, (object) AclFeature.SettingsTab_Company_ConfigurableKeyDates);
      this.nodeToFeature.Add((object) this.channelOptionsSetupNode, (object) AclFeature.SettingsTab_ChannelOptions);
      this.nodeToFeature.Add((object) this.rESPASetupNode, (object) AclFeature.SettingsTab_RESPA);
      this.nodeToFeature.Add((object) this.changedCircumstancesSetupSetupNode, (object) AclFeature.SettingsTab_ChangedCircumstancesSetup);
      this.nodeToFeature.Add((object) this.disclosureTrackingSettingsSetupNode, (object) AclFeature.SettingsTab_DisclosureTrackingSettings);
      this.nodeToFeature.Add((object) this.complianceCalendarSetupNode, (object) AclFeature.SettingsTab_ComplianceCalendar);
      this.nodeToFeature.Add((object) this.gfe2009PrintSetupNode, (object) AclFeature.SettingsTab_2009GFEPrint);
      this.nodeToFeature.Add((object) this.trusteeListSetupNode, (object) AclFeature.SettingsTab_TrusteeList);
      this.nodeToFeature.Add((object) this.piggybackLoanSynchronizationSetupNode, (object) AclFeature.SettingsTab_PiggybackLoanSynchronization);
      this.nodeToFeature.Add((object) this.syncTemplateSetupNode, (object) AclFeature.SettingsTab_SyncTemplates);
      this.nodeToFeature.Add((object) this.privacyPolicySetupNode, (object) AclFeature.SettingsTab_PrivacyPolicy);
      this.nodeToFeature.Add((object) this.zipcodeSetupSetupNode, (object) AclFeature.SettingsTab_ZipcodeSetup);
      this.nodeToFeature.Add((object) this.nMLSReportSetupSetupNode, (object) AclFeature.SettingsTab_NMLSReportSetup);
      this.nodeToFeature.Add((object) this.hmdaSetupNode, (object) AclFeature.SettingsTab_HMDASetup);
      this.nodeToFeature.Add((object) this.hmdaAddLEINode, (object) AclFeature.SettingsTab_HMDAAddLEI);
      this.nodeToFeature.Add((object) this.hmdaEditLEINode, (object) AclFeature.SettingsTab_HMDAEditLEI);
      this.nodeToFeature.Add((object) this.hmdaRemoveLEINode, (object) AclFeature.SettingsTab_HMDARemoveLEI);
      this.nodeToFeature.Add((object) this.verificationContactSetupSetupNode, (object) AclFeature.SettingsTab_VerificationContactSetup);
      this.nodeToFeature.Add((object) this.eFolderSetupNode, (object) AclFeature.SettingsTab_EFolder);
      this.nodeToFeature.Add((object) this.documentConversionSetupNode, (object) AclFeature.SettingsTab_DocumentConversion);
      this.nodeToFeature.Add((object) this.documentsSetupNode, (object) AclFeature.SettingsTab_Documents);
      this.nodeToFeature.Add((object) this.documentExportTemplatesSetupNode, (object) AclFeature.SettingsTab_DocumentExportTemplates);
      this.nodeToFeature.Add((object) this.documentGroupsSetupNode, (object) AclFeature.SettingsTab_DocumentGroups);
      this.nodeToFeature.Add((object) this.documentStackingTemplatesSetupNode, (object) AclFeature.SettingsTab_DocumentStackingTemplates);
      this.nodeToFeature.Add((object) this.documentIdentificationSetupNode, (object) AclFeature.SettingsTab_DocumentIdentification);
      this.nodeToFeature.Add((object) this.documentTrainingSetupNode, (object) AclFeature.eFolder_UF_Approver);
      this.nodeToFeature.Add((object) this.enhancedConditionsSetupNode, (object) AclFeature.SettingsTab_EnhancedConditions);
      this.nodeToFeature.Add((object) this.conditionTypeSettingsSetupNode, (object) AclFeature.SettingsTab_ConditionTypeSettings);
      this.nodeToFeature.Add((object) this.addEditCopyConditionsSetupNode, (object) AclFeature.SettingsTab_AddEditCopyConditions);
      this.nodeToFeature.Add((object) this.deleteConditionsSetupNode, (object) AclFeature.SettingsTab_DeleteConditions);
      this.nodeToFeature.Add((object) this.activateDeativateConditionsSetupNode, (object) AclFeature.SettingsTab_ActivateDeactivateConditions);
      this.nodeToFeature.Add((object) this.enhancedConditionSetsSetupNode, (object) AclFeature.SettingsTab_EnhancedConditionSets);
      this.nodeToFeature.Add((object) this.conditionsSetupNode, (object) AclFeature.SettingsTab_Conditions);
      this.nodeToFeature.Add((object) this.conditionSetsSetupNode, (object) AclFeature.SettingsTab_ConditionSets);
      this.nodeToFeature.Add((object) this.postClosingConditionsSetupNode, (object) AclFeature.SettingsTab_PostClosingConditions);
      this.nodeToFeature.Add((object) this.postClosingConditionSetsSetupNode, (object) AclFeature.SettingsTab_PostClosingConditionSets);
      this.nodeToFeature.Add((object) this.purchaseConditionOptionsSetupNode, (object) AclFeature.SettingsTab_PurchaseConditionOptions);
      this.nodeToFeature.Add((object) this.purchaseConditionsSetupNode, (object) AclFeature.SettingsTab_PurchaseConditions);
      this.nodeToFeature.Add((object) this.purchaseConditionSetsSetupNode, (object) AclFeature.SettingsTab_PurchaseConditionSets);
      this.nodeToFeature.Add((object) this.hTMLEmailTemplatesSetupNode, (object) AclFeature.SettingsTab_HTMLEmailTemplates);
      this.nodeToFeature.Add((object) this.webCenterConfigurationSetupNode, (object) AclFeature.SettingsTab_WebCenterConfiguration);
      this.nodeToFeature.Add((object) this.notificationTemplatesNode, (object) AclFeature.SettingsTab_NotificationTemplates);
      this.nodeToFeature.Add((object) this.docsSetupNode, (object) AclFeature.SettingsTab_Docs);
      this.nodeToFeature.Add((object) this.eDisclosurePackagesSetupNode, (object) AclFeature.SettingsTab_EDisclosurePackages);
      this.nodeToFeature.Add((object) this.eDisclosurePlanCodesSetupNode, (object) AclFeature.eFolder_Other_eDisclosure_ManagePlanCodes);
      this.nodeToFeature.Add((object) this.eDisclosureStackingTemplatesSetupNode, (object) AclFeature.eFolder_Other_eDisclosure_ManageStackingTemplates);
      this.nodeToFeature.Add((object) this.closingPlanCodesSetupNode, (object) AclFeature.LoanTab_Other_PlanCode);
      this.nodeToFeature.Add((object) this.closingStackingTemplatesSetupNode, (object) AclFeature.LoanTab_EMClosingDocs_ManageStackingOrders);
      this.nodeToFeature.Add((object) this.complianceAuditSettingsSetupNode, (object) AclFeature.SettingsTab_ComplianceAuditSettings);
      this.nodeToFeature.Add((object) this.secondaryRootSetupNode, (object) AclFeature.SettingsTab_Company_SecondarySetup);
      this.nodeToFeature.Add((object) this.productandPricingSetupNode, (object) AclFeature.SettingsTab_ProductandPricing);
      this.nodeToFeature.Add((object) this.secondaryLockFieldsSetupNode, (object) AclFeature.SettingsTab_SecondaryLockFields);
      this.nodeToFeature.Add((object) this.lockRequestAdditionalFieldsSetupNode, (object) AclFeature.SettingsTab_LockRequestAdditionalFields);
      this.nodeToFeature.Add((object) this.lockComparisonToolFieldsSetupNode, (object) AclFeature.SettingsTab_LockComparisonToolFields);
      this.nodeToFeature.Add((object) this.autoLockSetupNode, (object) AclFeature.SettingsTab_AutoLock);
      this.nodeToFeature.Add((object) this.investorTemplatesSetupNode, (object) AclFeature.SettingsTab_InvestorTemplates);
      this.nodeToFeature.Add((object) this.ePPSLoanProgramNode, (object) AclFeature.SettingsTab_EPPSLoanProgram);
      this.nodeToFeature.Add((object) this.tradeManagementSetupSetupNode, (object) AclFeature.SettingsTab_TradeManagementSetup);
      if (!flag)
        this.nodeToFeature.Add((object) this.normalizedBidTapeTemplateNode, (object) AclFeature.SettingsTab_NormalizedBidTapeTemplate);
      this.nodeToFeature.Add((object) this.adjustmentTemplatesSetupNode, (object) AclFeature.SettingsTab_AdjustmentTemplates);
      this.nodeToFeature.Add((object) this.lockDeskSetupSetupNode, (object) AclFeature.SettingsTab_LockDeskSetup);
      this.nodeToFeature.Add((object) this.sRPTemplatesSetupNode, (object) AclFeature.SettingsTab_SRPTemplates);
      this.nodeToFeature.Add((object) this.fundingTemplatesSetupNode, (object) AclFeature.SettingsTab_FundingTemplates);
      this.nodeToFeature.Add((object) this.servicingSetupNode, (object) AclFeature.SettingsTab_Servicing);
      this.nodeToFeature.Add((object) this.purchaseAdviceFormSetupNode, (object) AclFeature.SettingsTab_PurchaseAdviceForm);
      this.nodeToFeature.Add((object) this.correspondentPurchaseAdviceManagementNode, (object) AclFeature.SettingsTab_CorrespondentPurchaseAdviceManagement);
      this.nodeToFeature.Add((object) this.loanPricingDecimalPlacesSetupNode, (object) AclFeature.SettingsTab_LoanPricingDecimalPlaces);
      this.nodeToFeature.Add((object) this.contactSetupNode, (object) AclFeature.SettingsTab_Contact);
      this.nodeToFeature.Add((object) this.borrowerCustomFieldsSetupNode, (object) AclFeature.SettingsTab_BorrowerCustomFields);
      this.nodeToFeature.Add((object) this.borrowerContactStatusSetupNode, (object) AclFeature.SettingsTab_BorrowerContactStatus);
      this.nodeToFeature.Add((object) this.borrwerContactUpdateSetupNode, (object) AclFeature.SettingsTab_BorrwerContactUpdate);
      this.nodeToFeature.Add((object) this.businessCustomFieldsSetupNode, (object) AclFeature.SettingsTab_BusinessCustomFields);
      this.nodeToFeature.Add((object) this.businessCategoriesSetupNode, (object) AclFeature.SettingsTab_BusinessCategories);
      this.nodeToFeature.Add((object) this.publicBusinessContactGroupsSetupNode, (object) AclFeature.SettingsTab_Company_PublicBizContactGroup);
      this.nodeToFeature.Add((object) this.emailServerSettingsSetupNode, (object) AclFeature.SettingsTab_EmailServerSettings);
      this.nodeToFeature.Add((object) this.tablesFeesSetupNode, (object) AclFeature.SettingsTab_Company_TableFee);
      this.nodeToFeature.Add((object) this.escrowSetupNode, (object) AclFeature.SettingsTab_Escrow);
      this.nodeToFeature.Add((object) this.titleSetupNode, (object) AclFeature.SettingsTab_Title);
      this.nodeToFeature.Add((object) this.hELOCTableSetupNode, (object) AclFeature.SettingsTab_HELOCTable);
      this.nodeToFeature.Add((object) this.mITablesSetupNode, (object) AclFeature.SettingsTab_MITables);
      this.nodeToFeature.Add((object) this.fHACountyLimitsSetupNode, (object) AclFeature.SettingsTab_FHACountyLimits);
      this.nodeToFeature.Add((object) this.conventionalCountyLimitsSetupNode, (object) AclFeature.SettingsTab_ConventionalCountyLimits);
      this.nodeToFeature.Add((object) this.fedThresholdsSetupNode, (object) AclFeature.SettingsTab_FedTresholdAdjustments);
      this.nodeToFeature.Add((object) this.cityTaxSetupNode, (object) AclFeature.SettingsTab_CityTax);
      this.nodeToFeature.Add((object) this.stateTaxSetupNode, (object) AclFeature.SettingsTab_StateTax);
      this.nodeToFeature.Add((object) this.userDefinedFeeSetupNode, (object) AclFeature.SettingsTab_UserDefinedFee);
      this.nodeToFeature.Add((object) this.itemizationFeeManagementSetupNode, (object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt);
      this.nodeToFeature.Add((object) this.lOCompensationSetupNode, (object) AclFeature.SettingsTab_LOCompensation);
      this.nodeToFeature.Add((object) this.tmpBuydownSetupNode, (object) AclFeature.SettingsTab_TemporaryBuydown);
      this.nodeToFeature.Add((object) this.specialFeatureCodesSetupNode, (object) AclFeature.SettingsTab_SpecialFeatureCodes);
      this.nodeToFeature.Add((object) this.businessRulesSetupNode, (object) AclFeature.SettingsTab_BusinessRules);
      this.nodeToFeature.Add((object) this.loanFolderBusinessRuleSetupNode, (object) AclFeature.SettingsTab_LoanFolderBusinessRule);
      this.nodeToFeature.Add((object) this.milestoneCompletionSetupNode, (object) AclFeature.SettingsTab_MilestoneCompletion);
      this.nodeToFeature.Add((object) this.loanActionCompletionSetupNode, (object) AclFeature.SettingsTab_LoanActionCompletion);
      this.nodeToFeature.Add((object) this.fieldDataEntrySetupNode, (object) AclFeature.SettingsTab_FieldDataEntry);
      this.nodeToFeature.Add((object) this.fieldTriggersSetupNode, (object) AclFeature.SettingsTab_FieldTriggers);
      this.nodeToFeature.Add((object) this.automatedConditionsSetupNode, (object) AclFeature.SettingsTab_AutomatedConditions);
      this.nodeToFeature.Add((object) this.automatedEnhancedConditionsSetupNode, (object) AclFeature.SettingsTab_AutomatedEnhancedConditions);
      this.nodeToFeature.Add((object) this.automatedPurchaseConditionsSetupNode, (object) AclFeature.SettingsTab_AutomatedPurchaseConditions);
      this.nodeToFeature.Add((object) this.personaAccesstoFieldsSetupNode, (object) AclFeature.SettingsTab_PersonaAccesstoFields);
      this.nodeToFeature.Add((object) this.personaAccesstoLoansSetupNode, (object) AclFeature.SettingsTab_PersonaAccesstoLoans);
      this.nodeToFeature.Add((object) this.personaAccesstoLoanActionsSetupNode, (object) AclFeature.SettingsTab_PersonaAccesstoLoanActions);
      this.nodeToFeature.Add((object) this.roleAccesstoDocumentsSetupNode, (object) AclFeature.SettingsTab_RoleAccesstoDocuments);
      this.nodeToFeature.Add((object) this.inputFormListSetupNode, (object) AclFeature.SettingsTab_InputFormList);
      this.nodeToFeature.Add((object) this.loanFormPrintingSetupNode, (object) AclFeature.SettingsTab_LoanFormPrinting);
      this.nodeToFeature.Add((object) this.printAutoSelectionSetupNode, (object) AclFeature.SettingsTab_PrintAutoSelection);
      this.nodeToFeature.Add((object) this.appraisalOrderManagementSetupNode, (object) AclFeature.SettingsTab_AppraisalOrderManagement);
      this.nodeToFeature.Add((object) this.lOCompensationRuleSetupNode, (object) AclFeature.SettingsTab_LOCompensationRule);
      this.nodeToFeature.Add((object) this.DocumentTrackingSetupNode, (object) AclFeature.SettingsTab_DocumentTracking);
      this.nodeToFeature.Add((object) this.sysAdminSetupNode, (object) AclFeature.SettingsTab_SysAdmin);
      this.nodeToFeature.Add((object) this.analysisToolsSetupNode, (object) AclFeature.ThinThick_AnalysisTool_Access);
      this.nodeToFeature.Add((object) this.currentLoginsSetupNode, (object) AclFeature.SettingsTab_Company_CurrentLogins);
      this.nodeToFeature.Add((object) this.logUsersOutSetupNode, (object) AclFeature.SettingsTab_Company_LogUsersOut);
      this.nodeToFeature.Add((object) this.disableLoginsSetupNode, (object) AclFeature.SettingsTab_Company_DisableLogins);
      this.nodeToFeature.Add((object) this.allUserInformationSetupNode, (object) AclFeature.SettingsTab_AllUserInformation);
      this.nodeToFeature.Add((object) this.viewSettReportSetupNode, (object) AclFeature.ViewNewSettings_Report);
      this.nodeToFeature.Add((object) this.viewOthSettReportSetupNode, (object) AclFeature.ViewOtherSettings_Report);
      this.nodeToFeature.Add((object) this.cancelSettReportSetupNode, (object) AclFeature.CancelSettings_Report);
      this.nodeToFeature.Add((object) this.cancelOthSettReportSetupNode, (object) AclFeature.CancelOtherSettings_Report);
      this.nodeToFeature.Add((object) this.delSettReportSetupNode, (object) AclFeature.DeleteSettings_Report);
      this.nodeToFeature.Add((object) this.loanReassignmentSetupNode, (object) AclFeature.SettingsTab_Company_LoanReassignment);
      this.nodeToFeature.Add((object) this.unlockLoanFileSetupNode, (object) AclFeature.SettingsTab_UnlockLoanFiles);
      this.nodeToFeature.Add((object) this.unlockTradeSetupNode, (object) AclFeature.SettingsTab_UnlockTrade);
      this.nodeToFeature.Add((object) this.systemAuditTrailSetupNode, (object) AclFeature.SettingsTab_Company_SystemAuditTrail);
      this.nodeToFeature.Add((object) this.loanErrorInfoSetupNode, (object) AclFeature.SettingsTab_LoanErrorInformation);
      this.nodeToFeature.Add((object) this.addServicesSetupNode, (object) AclFeature.SettingsTab_AddServices);
      this.nodeToFeature.Add((object) this.eDocumentManagementSetupNode, (object) AclFeature.SettingsTab_EDocumentManagement);
      this.nodeToFeature.Add((object) this.companyStatusOnlineSetupNode, (object) AclFeature.SettingsTab_CompanyStatusOnline);
      this.nodeToFeature.Add((object) this.eDisclosureFulfillmentSetupNode, (object) AclFeature.SettingsTab_EDisclosureFulfillment);
      this.nodeToFeature.Add((object) this.ElliemaeAIQSetupNode, (object) AclFeature.SettingsTab_ElliemaeAIQ);
      this.nodeToFeature.Add((object) this.eCloseSetupNode, (object) AclFeature.SettingsTab_EClose);
      this.nodeToFeature.Add((object) this.complianceReviewSetupSetupNode, (object) AclFeature.SettingsTab_Company_ComplianceReviewSetup);
      this.nodeToFeature.Add((object) this.tservice4506SetupNode, (object) AclFeature.SettingsTab_Tservice4506);
      this.nodeToFeature.Add((object) this.tQLServicesSetupNode, (object) AclFeature.SettingsTab_TQLServices);
      this.nodeToFeature.Add((object) this.appraisalServiceSetupNode, (object) AclFeature.SettingsTab_AppraisalService);
      this.nodeToFeature.Add((object) this.titleServiceSetupNode, (object) AclFeature.SettingsTab_TitleService);
      this.nodeToFeature.Add((object) this.fraudServiceSetupNode, (object) AclFeature.SettingsTab_FraudService);
      this.nodeToFeature.Add((object) this.fannieServiceSetupNode, (object) AclFeature.SettingsTab_FannieService);
      this.nodeToFeature.Add((object) this.freddieServiceSetupNode, (object) AclFeature.SettingsTab_FreddieService);
      this.nodeToFeature.Add((object) this.miServiceSetupNode, (object) AclFeature.SettingsTab_MIService);
      this.nodeToFeature.Add((object) this.freddieMacCACServiceSetupNode, (object) AclFeature.SettingsTab_FreddieMacCACService);
      this.nodeToFeature.Add((object) this.fannieMaeUCDTransferServiceSetupNode, (object) AclFeature.SettingsTab_FannieMaeUCDTransferService);
      this.nodeToFeature.Add((object) this.ginnieMaePddServiceSetup, (object) AclFeature.SettingsTab_GinnieMaePdd);
      this.nodeToFeature.Add((object) this.valuationServiceSetupNode, (object) AclFeature.SettingsTab_ValuationService);
      this.nodeToFeature.Add((object) this.floodServiceSetupNode, (object) AclFeature.SettingsTab_FloodService);
      this.nodeToFeature.Add((object) this.dataTracConnectionSetupNode, (object) AclFeature.SettingsTab_DataTracConnection);
      this.nodeToFeature.Add((object) this.insightsSetupNode, (object) AclFeature.SettingsTab_InsightsSetup);
      this.nodeToFeature.Add((object) this.personalTemplatesSetupNode, (object) AclFeature.SettingsTab_PersonalTemplates);
      this.nodeToFeature.Add((object) this.customPrintFormsSetupNode, (object) AclFeature.SettingsTab_Personal_CustomPrintForms);
      this.nodeToFeature.Add((object) this.printFormGroupsSetupNode, (object) AclFeature.SettingsTab_Personal_PrintGroups);
      this.nodeToFeature.Add((object) this.loanProgramsSetupNode, (object) AclFeature.SettingsTab_Personal_LoanPrograms);
      this.nodeToFeature.Add((object) this.closingCostsSetupNode, (object) AclFeature.SettingsTab_Personal_ClosingCosts);
      this.nodeToFeature.Add((object) this.inputFormSetsSetupNode, (object) AclFeature.SettingsTab_Personal_InputFormSets);
      this.nodeToFeature.Add((object) this.settlementServiceProvidersSetupNode, (object) AclFeature.SettingsTab_Personal_SettlementServiceProvider);
      this.nodeToFeature.Add((object) this.affiliatedBusinessArrangementTemplatesSetupNode, (object) AclFeature.SettingsTab_Personal_Affiliate);
      this.nodeToFeature.Add((object) this.documentSetsSetupNode, (object) AclFeature.SettingsTab_Personal_DocumentSets);
      this.nodeToFeature.Add((object) this.taskSetsSetupNode, (object) AclFeature.SettingsTab_Personal_TaskSets);
      this.nodeToFeature.Add((object) this.dataTemplatesSetupNode, (object) AclFeature.SettingsTab_Personal_MiscDataTemplates);
      this.nodeToFeature.Add((object) this.loanTemplateSetsSetupNode, (object) AclFeature.SettingsTab_Personal_LoanTemplateSets);
      this.nodeToFeature.Add((object) this.dynamicDataMgmtSetupNode, (object) AclFeature.SettingsTab_DynamicDataManagement);
      this.nodeToFeature.Add((object) this.feeRulesSetupNode, (object) AclFeature.SettingsTab_FeeRules);
      this.nodeToFeature.Add((object) this.fieldRulesSetupNode, (object) AclFeature.SettingsTab_FieldRules);
      this.nodeToFeature.Add((object) this.dataTablesSetupNode, (object) AclFeature.SettingsTab_DataTables);
      this.nodeToFeature.Add((object) this.feeGroupsSetupNode, (object) AclFeature.SettingsTab_FeeGroups);
      this.nodeToFeature.Add((object) this.dataPopulationTimeingSetupNode, (object) AclFeature.SettingsTab_DataPopulationTiming);
      this.nodeToFeature.Add((object) this.automatedEConsentSetupNode, (object) AclFeature.SettingsTab_AutomatedEConsent);
      this.featureToNodeTbl = new Hashtable(FeatureSets.SettingsTabCompanyFeatures.Length);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_ConditionalApprovalLetter, (object) this.conditionFormsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_CompanyUser, (object) this.companyUserSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_CompanyInformation, (object) this.companyInformationSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_EllieMaeNetworkCompany, (object) this.ellieMaeNetworkCompanySetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ServicesPasswordManager, (object) this.servicesPasswordManagerSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personas, (object) this.personasSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PersonasEdit, (object) this.personasEditSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_OrganizationsUser, (object) this.organizationsUserSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_OrgUserAddLEI, (object) this.orgUserSetupAddLEINode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_OrgUserEditLEI, (object) this.orgUserSetupEditLEINode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Roles, (object) this.rolesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Milestones, (object) this.milestonesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_UserGroups, (object) this.userGroupsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_InvestorConnectSetup, (object) this.investorConnectSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DeliverLoans, (object) this.deliverLoansSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PartnerSetup, (object) this.partnerSetupSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Loan, (object) this.loanSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AutoLoanNumbering, (object) this.autoLoanNumberingSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AutoMERSMINNumbering, (object) this.autoMERSMINNumberingSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LoanFolders, (object) this.loanFoldersSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LoanDuplication, (object) this.loanDuplicationSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Alerts, (object) this.alertsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Log, (object) this.logSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Tasks, (object) this.tasksSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DefaultInputForms, (object) this.defaultInputFormsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_LoanCustomFields, (object) this.loanCustomFieldsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_ConfigurableKeyDates, (object) this.configurableKeyDatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ChannelOptions, (object) this.channelOptionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_RESPA, (object) this.rESPASetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ChangedCircumstancesSetup, (object) this.changedCircumstancesSetupSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DisclosureTrackingSettings, (object) this.disclosureTrackingSettingsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ComplianceCalendar, (object) this.complianceCalendarSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_2009GFEPrint, (object) this.gfe2009PrintSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_TrusteeList, (object) this.trusteeListSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PiggybackLoanSynchronization, (object) this.piggybackLoanSynchronizationSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_SyncTemplates, (object) this.syncTemplateSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PrivacyPolicy, (object) this.privacyPolicySetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ZipcodeSetup, (object) this.zipcodeSetupSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_HMDASetup, (object) this.hmdaSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_HMDAAddLEI, (object) this.hmdaAddLEINode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_HMDAEditLEI, (object) this.hmdaEditLEINode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_HMDARemoveLEI, (object) this.hmdaRemoveLEINode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_NMLSReportSetup, (object) this.nMLSReportSetupSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_VerificationContactSetup, (object) this.verificationContactSetupSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_EFolder, (object) this.eFolderSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DocumentConversion, (object) this.documentConversionSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Documents, (object) this.documentsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DocumentExportTemplates, (object) this.documentExportTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DocumentGroups, (object) this.documentGroupsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DocumentStackingTemplates, (object) this.documentStackingTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DocumentIdentification, (object) this.documentIdentificationSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_UF_Approver, (object) this.documentTrainingSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_EnhancedConditions, (object) this.enhancedConditionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_EnhancedConditionSets, (object) this.enhancedConditionSetsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ConditionTypeSettings, (object) this.conditionTypeSettingsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AddEditCopyConditions, (object) this.addEditCopyConditionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DeleteConditions, (object) this.deleteConditionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ActivateDeactivateConditions, (object) this.activateDeativateConditionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Conditions, (object) this.conditionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ConditionSets, (object) this.conditionSetsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PostClosingConditions, (object) this.postClosingConditionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PostClosingConditionSets, (object) this.postClosingConditionSetsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PurchaseConditionOptions, (object) this.purchaseConditionOptionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PurchaseConditions, (object) this.purchaseConditionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PurchaseConditionSets, (object) this.purchaseConditionSetsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_HTMLEmailTemplates, (object) this.hTMLEmailTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_WebCenterConfiguration, (object) this.webCenterConfigurationSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_NotificationTemplates, (object) this.notificationTemplatesNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Docs, (object) this.docsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_EDisclosurePackages, (object) this.eDisclosurePackagesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_EDisclosurePlanCodes, (object) this.eDisclosurePlanCodesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_eDisclosure_ManagePlanCodes, (object) this.eDisclosurePlanCodesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.eFolder_Other_eDisclosure_ManageStackingTemplates, (object) this.eDisclosureStackingTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_Other_PlanCode, (object) this.closingPlanCodesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanTab_EMClosingDocs_ManageStackingOrders, (object) this.closingStackingTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ComplianceAuditSettings, (object) this.complianceAuditSettingsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_SecondarySetup, (object) this.secondaryRootSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ProductandPricing, (object) this.productandPricingSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_SecondaryLockFields, (object) this.secondaryLockFieldsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LockRequestAdditionalFields, (object) this.lockRequestAdditionalFieldsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LockComparisonToolFields, (object) this.lockComparisonToolFieldsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AutoLock, (object) this.autoLockSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_InvestorTemplates, (object) this.investorTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_EPPSLoanProgram, (object) this.ePPSLoanProgramNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_TradeManagementSetup, (object) this.tradeManagementSetupSetupNode);
      if (!flag)
        this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_NormalizedBidTapeTemplate, (object) this.normalizedBidTapeTemplateNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AdjustmentTemplates, (object) this.adjustmentTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LockDeskSetup, (object) this.lockDeskSetupSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_SRPTemplates, (object) this.sRPTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FundingTemplates, (object) this.fundingTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Servicing, (object) this.servicingSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PurchaseAdviceForm, (object) this.purchaseAdviceFormSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_CorrespondentPurchaseAdviceManagement, (object) this.correspondentPurchaseAdviceManagementNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LoanPricingDecimalPlaces, (object) this.loanPricingDecimalPlacesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Contact, (object) this.contactSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_BorrowerCustomFields, (object) this.borrowerCustomFieldsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_BorrowerContactStatus, (object) this.borrowerContactStatusSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_BorrwerContactUpdate, (object) this.borrwerContactUpdateSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_BusinessCustomFields, (object) this.businessCustomFieldsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_BusinessCategories, (object) this.businessCategoriesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_PublicBizContactGroup, (object) this.publicBusinessContactGroupsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_EmailServerSettings, (object) this.emailServerSettingsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_TableFee, (object) this.tablesFeesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Escrow, (object) this.escrowSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Title, (object) this.titleSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_HELOCTable, (object) this.hELOCTableSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_MITables, (object) this.mITablesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FHACountyLimits, (object) this.fHACountyLimitsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ConventionalCountyLimits, (object) this.conventionalCountyLimitsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FedTresholdAdjustments, (object) this.fedThresholdsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_CityTax, (object) this.cityTaxSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_StateTax, (object) this.stateTaxSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_UserDefinedFee, (object) this.userDefinedFeeSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt, (object) this.itemizationFeeManagementSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LOCompensation, (object) this.lOCompensationSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_TemporaryBuydown, (object) this.tmpBuydownSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_SpecialFeatureCodes, (object) this.specialFeatureCodesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_BusinessRules, (object) this.businessRulesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LoanFolderBusinessRule, (object) this.loanFolderBusinessRuleSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_MilestoneCompletion, (object) this.milestoneCompletionSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LoanActionCompletion, (object) this.loanActionCompletionSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FieldDataEntry, (object) this.fieldDataEntrySetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FieldTriggers, (object) this.fieldTriggersSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AutomatedEnhancedConditions, (object) this.automatedEnhancedConditionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AutomatedPurchaseConditions, (object) this.automatedPurchaseConditionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PersonaAccesstoFields, (object) this.personaAccesstoFieldsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PersonaAccesstoLoans, (object) this.personaAccesstoLoansSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PersonaAccesstoLoanActions, (object) this.personaAccesstoLoanActionsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_RoleAccesstoDocuments, (object) this.roleAccesstoDocumentsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_InputFormList, (object) this.inputFormListSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LoanFormPrinting, (object) this.loanFormPrintingSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PrintAutoSelection, (object) this.printAutoSelectionSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AppraisalOrderManagement, (object) this.appraisalOrderManagementSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LOCompensationRule, (object) this.lOCompensationRuleSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DocumentTracking, (object) this.DocumentTrackingSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_SysAdmin, (object) this.sysAdminSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.ThinThick_AnalysisTool_Access, (object) this.analysisToolsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_CurrentLogins, (object) this.currentLoginsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_LogUsersOut, (object) this.logUsersOutSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_DisableLogins, (object) this.disableLoginsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AllUserInformation, (object) this.allUserInformationSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.ViewNewSettings_Report, (object) this.viewSettReportSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.ViewOtherSettings_Report, (object) this.viewOthSettReportSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.CancelSettings_Report, (object) this.cancelSettReportSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.CancelOtherSettings_Report, (object) this.cancelOthSettReportSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.DeleteSettings_Report, (object) this.delSettReportSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_LoanReassignment, (object) this.loanReassignmentSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_UnlockLoanFiles, (object) this.unlockLoanFileSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_UnlockTrade, (object) this.unlockTradeSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_SystemAuditTrail, (object) this.systemAuditTrailSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_LoanErrorInformation, (object) this.loanErrorInfoSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AddServices, (object) this.addServicesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_EDocumentManagement, (object) this.eDocumentManagementSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_CompanyStatusOnline, (object) this.companyStatusOnlineSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_EDisclosureFulfillment, (object) this.eDisclosureFulfillmentSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ElliemaeAIQ, (object) this.ElliemaeAIQSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_EClose, (object) this.eCloseSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Company_ComplianceReviewSetup, (object) this.complianceReviewSetupSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Tservice4506, (object) this.tservice4506SetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_TQLServices, (object) this.tQLServicesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AppraisalService, (object) this.appraisalServiceSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_TitleService, (object) this.titleServiceSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FraudService, (object) this.fraudServiceSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FannieService, (object) this.fannieServiceSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FreddieService, (object) this.freddieServiceSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_MIService, (object) this.miServiceSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FreddieMacCACService, (object) this.freddieMacCACServiceSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FannieMaeUCDTransferService, (object) this.fannieMaeUCDTransferServiceSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_GinnieMaePdd, (object) this.ginnieMaePddServiceSetup);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_ValuationService, (object) this.valuationServiceSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FloodService, (object) this.floodServiceSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DataTracConnection, (object) this.dataTracConnectionSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_InsightsSetup, (object) this.insightsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_PersonalTemplates, (object) this.personalTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_CustomPrintForms, (object) this.customPrintFormsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_PrintGroups, (object) this.printFormGroupsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_LoanPrograms, (object) this.loanProgramsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_ClosingCosts, (object) this.closingCostsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_InputFormSets, (object) this.inputFormSetsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_SettlementServiceProvider, (object) this.settlementServiceProvidersSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_Affiliate, (object) this.affiliatedBusinessArrangementTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_DocumentSets, (object) this.documentSetsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_TaskSets, (object) this.taskSetsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_MiscDataTemplates, (object) this.dataTemplatesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_Personal_LoanTemplateSets, (object) this.loanTemplateSetsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DynamicDataManagement, (object) this.dynamicDataMgmtSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FeeRules, (object) this.feeRulesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FieldRules, (object) this.fieldRulesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DataTables, (object) this.dataTablesSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_FeeGroups, (object) this.feeGroupsSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_DataPopulationTiming, (object) this.dataPopulationTimeingSetupNode);
      this.featureToNodeTbl.Add((object) AclFeature.SettingsTab_AutomatedEConsent, (object) this.automatedEConsentSetupNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.companySettingNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.companyUserSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.companyInformationSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ellieMaeNetworkCompanySetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.servicesPasswordManagerSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.personasSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.personasEditSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.organizationsUserSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orgUserSetupAddLEINode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.orgUserSetupEditLEINode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.rolesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.milestonesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.userGroupsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.investorConnectSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deliverLoansSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.partnerSetupSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.autoLoanNumberingSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.autoMERSMINNumberingSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanFoldersSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanDuplicationSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.alertsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.logSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tasksSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.defaultInputFormsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.conditionFormsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanCustomFieldsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.configurableKeyDatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.channelOptionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.rESPASetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.changedCircumstancesSetupSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.disclosureTrackingSettingsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.complianceCalendarSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.gfe2009PrintSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.trusteeListSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.piggybackLoanSynchronizationSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.syncTemplateSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.privacyPolicySetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.zipcodeSetupSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.hmdaSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.hmdaAddLEINode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.hmdaEditLEINode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.hmdaRemoveLEINode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.nMLSReportSetupSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.verificationContactSetupSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.eFolderSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.documentConversionSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.documentsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.documentExportTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.documentGroupsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.documentStackingTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.documentIdentificationSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.documentTrainingSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.enhancedConditionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.enhancedConditionSetsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.conditionTypeSettingsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addEditCopyConditionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteConditionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.activateDeativateConditionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.conditionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.conditionSetsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.postClosingConditionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.postClosingConditionSetsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.purchaseConditionOptionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.purchaseConditionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.purchaseConditionSetsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.hTMLEmailTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.webCenterConfigurationSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.notificationTemplatesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.docsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.eDisclosurePackagesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.eDisclosurePlanCodesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.eDisclosureStackingTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.closingPlanCodesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.closingStackingTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.complianceAuditSettingsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.secondaryRootSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.productandPricingSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.secondaryLockFieldsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.lockRequestAdditionalFieldsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.lockComparisonToolFieldsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.autoLockSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.investorTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ePPSLoanProgramNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tradeManagementSetupSetupNode, (object) false);
      if (!flag)
        this.nodeToUpdateStatus.Add((object) this.normalizedBidTapeTemplateNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.adjustmentTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.lockDeskSetupSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.sRPTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.fundingTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.servicingSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.purchaseAdviceFormSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.correspondentPurchaseAdviceManagementNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanPricingDecimalPlacesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.contactSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.borrowerCustomFieldsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.borrowerContactStatusSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.borrwerContactUpdateSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.businessCustomFieldsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.businessCategoriesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.publicBusinessContactGroupsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.emailServerSettingsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tablesFeesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.escrowSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.titleSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.hELOCTableSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.mITablesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.fHACountyLimitsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.conventionalCountyLimitsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.fedThresholdsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.cityTaxSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.stateTaxSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.userDefinedFeeSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.itemizationFeeManagementSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.lOCompensationSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tmpBuydownSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.specialFeatureCodesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.businessRulesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanFolderBusinessRuleSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.milestoneCompletionSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanActionCompletionSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.fieldDataEntrySetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.fieldTriggersSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.automatedConditionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.automatedEnhancedConditionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.automatedPurchaseConditionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.personaAccesstoFieldsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.personaAccesstoLoansSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.personaAccesstoLoanActionsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.roleAccesstoDocumentsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.inputFormListSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanFormPrintingSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.printAutoSelectionSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.appraisalOrderManagementSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.lOCompensationRuleSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.DocumentTrackingSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.sysAdminSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.analysisToolsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.currentLoginsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.logUsersOutSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.disableLoginsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.allUserInformationSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.viewSettReportSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.viewOthSettReportSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.cancelSettReportSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.cancelOthSettReportSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.delSettReportSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanReassignmentSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.unlockLoanFileSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.unlockTradeSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.systemAuditTrailSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanErrorInfoSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.addServicesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.eDocumentManagementSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ElliemaeAIQSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.companyStatusOnlineSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.eDisclosureFulfillmentSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.eCloseSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.complianceReviewSetupSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tservice4506SetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tQLServicesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.appraisalServiceSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.titleServiceSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.fraudServiceSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.fannieServiceSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.freddieServiceSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.miServiceSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.freddieMacCACServiceSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.fannieMaeUCDTransferServiceSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ginnieMaePddServiceSetup, (object) false);
      this.nodeToUpdateStatus.Add((object) this.valuationServiceSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.floodServiceSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.dataTracConnectionSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.insightsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.personalTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.customPrintFormsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.printFormGroupsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanProgramsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.closingCostsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.inputFormSetsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.settlementServiceProvidersSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.affiliatedBusinessArrangementTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.documentSetsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.taskSetsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.dataTemplatesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanTemplateSetsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.dynamicDataMgmtSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.feeRulesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.fieldRulesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.dataTablesSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.feeGroupsSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.dataPopulationTimeingSetupNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.automatedEConsentSetupNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
    }
  }
}
