// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalSettingsSecurityHelper
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
namespace EllieMae.EMLite.Setup
{
  internal class ExternalSettingsSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode companyDetails;
    protected TreeNode createOrganizations;
    protected TreeNode deleteOrganizations;
    protected TreeNode organizationSettings;
    protected TreeNode companyInformation;
    protected TreeNode editCompanyInformation;
    protected TreeNode license;
    protected TreeNode editLicenseInformation;
    protected TreeNode loanType;
    protected TreeNode editLoanTypes;
    protected TreeNode TPOContacts;
    protected TreeNode editTPOContacts;
    protected TreeNode salesReps;
    protected TreeNode editSalesReps;
    protected TreeNode lenderContacts;
    protected TreeNode editLenderContacts;
    protected TreeNode commitment;
    protected TreeNode editCommitment;
    protected TreeNode LOComp;
    protected TreeNode editLOComp;
    protected TreeNode TradeMgmt;
    protected TreeNode editTradeMgmt;
    protected TreeNode ONRP;
    protected TreeNode editONRP;
    protected TreeNode notes;
    protected TreeNode editNotes;
    protected TreeNode deleteNotes;
    protected TreeNode webCenterSetup;
    protected TreeNode editWebCenterSetup;
    protected TreeNode attachments;
    protected TreeNode editAttachments;
    protected TreeNode deleteAttachments;
    protected TreeNode customFields;
    protected TreeNode customFieldsTab1;
    protected TreeNode editCustomFieldsTab1;
    protected TreeNode customFieldsTab2;
    protected TreeNode editCustomFieldsTab2;
    protected TreeNode customFieldsTab3;
    protected TreeNode editCustomFieldsTab3;
    protected TreeNode customFieldsTab4;
    protected TreeNode editCustomFieldsTab4;
    protected TreeNode customFieldsTab5;
    protected TreeNode editCustomFieldsTab5;
    protected TreeNode contacts;
    protected TreeNode contactSalesRep;
    protected TreeNode editContacts;
    protected TreeNode exportContacts;
    protected TreeNode deleteContacts;
    protected TreeNode sendWelcomeEmail;
    protected TreeNode resetPassword;
    protected TreeNode authorizedTrader;
    protected TreeNode TPOSettings;
    protected TreeNode TPOFees;
    protected TreeNode editTPOFees;
    protected TreeNode deleteTPOFees;
    protected TreeNode TPOFeesTab;
    protected TreeNode editTPOFeesTab;
    protected TreeNode deleteTPOFeesTab;
    protected TreeNode TPOAssignment;
    protected TreeNode TPOCustomFields;
    protected TreeNode AllTPOInformation;
    protected TreeNode TPOGlobalLenderContact;
    protected TreeNode DBATab;
    protected TreeNode editDBATab;
    protected TreeNode WarehouseTab;
    protected TreeNode editWarehouseTab;
    protected TreeNode TPODocSetting;
    protected TreeNode EditTPODocSetting;
    protected TreeNode DeleteTPODocSetting;
    protected TreeNode TPOConnectSiteManagement;
    protected TreeNode TPODisclosureSettings;
    protected TreeNode GlobalAccess;
    protected TreeNode AccountManagement;
    protected TreeNode TPODocumentTab;
    protected TreeNode DisableGlobalDocsForTPO;
    protected TreeNode EditTPODocument;
    protected TreeNode DeleteTPODocument;
    protected TreeNode TPOWCSiteManagement;
    protected TreeNode editBanks;
    protected TreeNode deleteBanks;
    protected TreeNode exportOrganizations;

    public ExternalSettingsSecurityHelper(
      Sessions.Session session,
      string userId,
      Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.ExternalSettingTabFeatures);
    }

    public ExternalSettingsSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.ExternalSettingTabFeatures);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.companyDetails = new TreeNode("Company Details");
      this.createOrganizations = new TreeNode("Create Organizations");
      this.deleteOrganizations = new TreeNode("Delete Organizations");
      this.editBanks = new TreeNode("Create/Edit Banks");
      this.deleteBanks = new TreeNode("Delete Banks");
      this.organizationSettings = new TreeNode("TPO Organization Settings");
      this.companyInformation = new TreeNode("Basic Info");
      this.editCompanyInformation = new TreeNode("Edit Basic Information");
      this.license = new TreeNode("License");
      this.editLicenseInformation = new TreeNode("Edit License Information");
      this.loanType = new TreeNode("Loan Criteria");
      this.editLoanTypes = new TreeNode("Edit Loan Criteria");
      this.TPOContacts = new TreeNode("TPO Contacts");
      this.editTPOContacts = new TreeNode("Edit TPO Contacts");
      this.salesReps = new TreeNode("Sales Reps / AE");
      this.editSalesReps = new TreeNode("Edit Sales Reps / AE");
      this.lenderContacts = new TreeNode("Lender Contacts");
      this.editLenderContacts = new TreeNode("Edit Lender Contacts");
      this.commitment = new TreeNode("Commitments");
      this.editCommitment = new TreeNode("Edit Commitments");
      this.LOComp = new TreeNode("LO Comp");
      this.editLOComp = new TreeNode("Edit LO Comp");
      this.TradeMgmt = new TreeNode("Trade Mgmt");
      this.editTradeMgmt = new TreeNode("Edit Trade Mgmt");
      this.ONRP = new TreeNode("ONRP");
      this.editONRP = new TreeNode("Edit ONRP");
      this.notes = new TreeNode("Notes");
      this.editNotes = new TreeNode("Edit Notes");
      this.deleteNotes = new TreeNode("Delete Notes");
      this.webCenterSetup = new TreeNode("TPO WebCenter Setup");
      this.editWebCenterSetup = new TreeNode("Edit TPO WebCenter Setup");
      this.attachments = new TreeNode("Attachments");
      this.editAttachments = new TreeNode("Edit Attachments");
      this.deleteAttachments = new TreeNode("Delete Attachments");
      this.customFields = new TreeNode("Custom Fields");
      this.customFieldsTab1 = new TreeNode("Custom Fields Tab1");
      this.editCustomFieldsTab1 = new TreeNode("Edit Tab 1 Custom Fields");
      this.customFieldsTab2 = new TreeNode("Custom Fields Tab2");
      this.editCustomFieldsTab2 = new TreeNode("Edit Tab 2 Custom Fields");
      this.customFieldsTab3 = new TreeNode("Custom Fields Tab3");
      this.editCustomFieldsTab3 = new TreeNode("Edit Tab 3 Custom Fields");
      this.customFieldsTab4 = new TreeNode("Custom Fields Tab4");
      this.editCustomFieldsTab4 = new TreeNode("Edit Tab 4 Custom Fields");
      this.customFieldsTab5 = new TreeNode("Custom Fields Tab5");
      this.editCustomFieldsTab5 = new TreeNode("Edit Tab 5 Custom Fields");
      this.contacts = new TreeNode("TPO Contacts");
      this.contactSalesRep = new TreeNode("Sales Rep");
      this.authorizedTrader = new TreeNode("Authorized Trader");
      this.editContacts = new TreeNode("Create/Edit Contacts");
      this.exportContacts = new TreeNode("Export Contacts");
      this.deleteContacts = new TreeNode("Delete Contacts");
      this.sendWelcomeEmail = new TreeNode("Send Welcome Email");
      this.resetPassword = new TreeNode("Reset Password");
      this.TPOSettings = new TreeNode("TPO Settings");
      this.TPOFees = new TreeNode("TPO Fees");
      this.editTPOFees = new TreeNode("Add/Edit TPO Fees");
      this.deleteTPOFees = new TreeNode("Delete TPO Fees");
      this.TPOFeesTab = new TreeNode("Fees");
      this.editTPOFeesTab = new TreeNode("Add/Edit TPO Fees");
      this.deleteTPOFeesTab = new TreeNode("Delete TPO Fees");
      this.TPOAssignment = new TreeNode("TPO Reassignment");
      this.TPOCustomFields = new TreeNode("TPO Custom Fields");
      this.AllTPOInformation = new TreeNode("All TPO Contact Information");
      this.TPOGlobalLenderContact = new TreeNode("TPO Global Lender Contacts");
      this.DBATab = new TreeNode("DBA");
      this.editDBATab = new TreeNode("Edit DBA");
      this.WarehouseTab = new TreeNode("Warehouse Tab");
      this.editWarehouseTab = new TreeNode("Edit Warehouse Banks");
      this.TPODocSetting = new TreeNode("TPO WebCenter Document List Settings");
      this.EditTPODocSetting = new TreeNode("Edit Document");
      this.DeleteTPODocSetting = new TreeNode("Delete Document");
      this.TPOConnectSiteManagement = new TreeNode("TPO Connect Site Management");
      this.TPODisclosureSettings = new TreeNode("TPO Disclosure Settings");
      this.GlobalAccess = new TreeNode("Global Company/User Delegation Access");
      this.AccountManagement = new TreeNode("Company/User Account Management");
      this.TPODocumentTab = new TreeNode("TPO WebCenter Documents tab");
      this.DisableGlobalDocsForTPO = new TreeNode("Disable TPO WebCenter Global Documents");
      this.EditTPODocument = new TreeNode("Add/Edit TPO WebCenter Additional Documents");
      this.DeleteTPODocument = new TreeNode("Delete TPO WebCenter Documents");
      this.TPOWCSiteManagement = new TreeNode("TPO WebCenter Site Management");
      this.exportOrganizations = new TreeNode("Export Organizations");
      this.companyInformation.Nodes.AddRange(new TreeNode[1]
      {
        this.editCompanyInformation
      });
      this.license.Nodes.AddRange(new TreeNode[1]
      {
        this.editLicenseInformation
      });
      this.loanType.Nodes.AddRange(new TreeNode[1]
      {
        this.editLoanTypes
      });
      this.TPOContacts.Nodes.AddRange(new TreeNode[1]
      {
        this.editTPOContacts
      });
      this.salesReps.Nodes.AddRange(new TreeNode[1]
      {
        this.editSalesReps
      });
      this.lenderContacts.Nodes.AddRange(new TreeNode[1]
      {
        this.editLenderContacts
      });
      this.commitment.Nodes.AddRange(new TreeNode[1]
      {
        this.editCommitment
      });
      this.LOComp.Nodes.AddRange(new TreeNode[1]
      {
        this.editLOComp
      });
      this.TradeMgmt.Nodes.AddRange(new TreeNode[1]
      {
        this.editTradeMgmt
      });
      this.ONRP.Nodes.AddRange(new TreeNode[1]
      {
        this.editONRP
      });
      this.editNotes.Nodes.AddRange(new TreeNode[1]
      {
        this.deleteNotes
      });
      this.notes.Nodes.AddRange(new TreeNode[1]
      {
        this.editNotes
      });
      this.webCenterSetup.Nodes.AddRange(new TreeNode[1]
      {
        this.editWebCenterSetup
      });
      this.editAttachments.Nodes.AddRange(new TreeNode[1]
      {
        this.deleteAttachments
      });
      this.attachments.Nodes.AddRange(new TreeNode[1]
      {
        this.editAttachments
      });
      this.customFieldsTab1.Nodes.AddRange(new TreeNode[1]
      {
        this.editCustomFieldsTab1
      });
      this.customFieldsTab2.Nodes.AddRange(new TreeNode[1]
      {
        this.editCustomFieldsTab2
      });
      this.customFieldsTab3.Nodes.AddRange(new TreeNode[1]
      {
        this.editCustomFieldsTab3
      });
      this.customFieldsTab4.Nodes.AddRange(new TreeNode[1]
      {
        this.editCustomFieldsTab4
      });
      this.customFieldsTab5.Nodes.AddRange(new TreeNode[1]
      {
        this.editCustomFieldsTab5
      });
      this.customFields.Nodes.AddRange(new TreeNode[5]
      {
        this.customFieldsTab1,
        this.customFieldsTab2,
        this.customFieldsTab3,
        this.customFieldsTab4,
        this.customFieldsTab5
      });
      this.EditTPODocument.Nodes.AddRange(new TreeNode[1]
      {
        this.DeleteTPODocument
      });
      this.TPODocumentTab.Nodes.AddRange(new TreeNode[2]
      {
        this.DisableGlobalDocsForTPO,
        this.EditTPODocument
      });
      this.organizationSettings.Nodes.AddRange(new TreeNode[19]
      {
        this.companyInformation,
        this.DBATab,
        this.license,
        this.loanType,
        this.TPOContacts,
        this.salesReps,
        this.lenderContacts,
        this.WarehouseTab,
        this.TPOFeesTab,
        this.LOComp,
        this.commitment,
        this.TradeMgmt,
        this.ONRP,
        this.notes,
        this.webCenterSetup,
        this.TPODocumentTab,
        this.TPOWCSiteManagement,
        this.attachments,
        this.customFields
      });
      this.contacts.Nodes.AddRange(new TreeNode[7]
      {
        this.contactSalesRep,
        this.authorizedTrader,
        this.editContacts,
        this.exportContacts,
        this.deleteContacts,
        this.sendWelcomeEmail,
        this.resetPassword
      });
      this.companyDetails.Nodes.AddRange(new TreeNode[6]
      {
        this.createOrganizations,
        this.deleteOrganizations,
        this.exportOrganizations,
        this.editBanks,
        this.organizationSettings,
        this.contacts
      });
      this.editBanks.Nodes.AddRange(new TreeNode[1]
      {
        this.deleteBanks
      });
      this.EditTPODocSetting.Nodes.AddRange(new TreeNode[1]
      {
        this.DeleteTPODocSetting
      });
      this.TPODocSetting.Nodes.AddRange(new TreeNode[1]
      {
        this.EditTPODocSetting
      });
      this.editTPOFees.Nodes.AddRange(new TreeNode[1]
      {
        this.deleteTPOFees
      });
      this.TPOFees.Nodes.AddRange(new TreeNode[1]
      {
        this.editTPOFees
      });
      this.editTPOFeesTab.Nodes.AddRange(new TreeNode[1]
      {
        this.deleteTPOFeesTab
      });
      this.TPOFeesTab.Nodes.AddRange(new TreeNode[1]
      {
        this.editTPOFeesTab
      });
      this.DBATab.Nodes.AddRange(new TreeNode[1]
      {
        this.editDBATab
      });
      this.WarehouseTab.Nodes.AddRange(new TreeNode[1]
      {
        this.editWarehouseTab
      });
      if (this.session.EncompassEdition == EncompassEdition.Banker)
        treeView.Nodes.AddRange(new TreeNode[12]
        {
          this.companyDetails,
          this.TPOSettings,
          this.TPOFees,
          this.TPOAssignment,
          this.TPOCustomFields,
          this.AllTPOInformation,
          this.TPOGlobalLenderContact,
          this.TPODocSetting,
          this.TPOConnectSiteManagement,
          this.TPODisclosureSettings,
          this.GlobalAccess,
          this.AccountManagement
        });
      treeView.ExpandAll();
      this.nodeToFeature = new Hashtable(FeatureSets.ExternalSettingTabFeatures.Length);
      this.nodeToFeature.Add((object) this.companyDetails, (object) AclFeature.ExternalSettings_CompanyDetails);
      this.nodeToFeature.Add((object) this.createOrganizations, (object) AclFeature.ExternalSettings_CreateOrganizations);
      this.nodeToFeature.Add((object) this.deleteOrganizations, (object) AclFeature.ExternalSettings_DeleteOrganizations);
      this.nodeToFeature.Add((object) this.editBanks, (object) AclFeature.ExternalSettings_EditBanks);
      this.nodeToFeature.Add((object) this.deleteBanks, (object) AclFeature.ExternalSettings_DeleteBanks);
      this.nodeToFeature.Add((object) this.organizationSettings, (object) AclFeature.ExternalSettings_OrganizationSettings);
      this.nodeToFeature.Add((object) this.companyInformation, (object) AclFeature.ExternalSettings_CompanyInformation);
      this.nodeToFeature.Add((object) this.editCompanyInformation, (object) AclFeature.ExternalSettings_EditCompanyInformation);
      this.nodeToFeature.Add((object) this.license, (object) AclFeature.ExternalSettings_License);
      this.nodeToFeature.Add((object) this.editLicenseInformation, (object) AclFeature.ExternalSettings_EditLicenseInformation);
      this.nodeToFeature.Add((object) this.loanType, (object) AclFeature.ExternalSettings_LoanType);
      this.nodeToFeature.Add((object) this.editLoanTypes, (object) AclFeature.ExternalSettings_EditLoanTypes);
      this.nodeToFeature.Add((object) this.TPOContacts, (object) AclFeature.ExternalSettings_TPOContacts);
      this.nodeToFeature.Add((object) this.editTPOContacts, (object) AclFeature.ExternalSettings_EditTPOContacts);
      this.nodeToFeature.Add((object) this.commitment, (object) AclFeature.ExternalSettings_Commitment);
      this.nodeToFeature.Add((object) this.editCommitment, (object) AclFeature.ExternalSettings_EditCommitment);
      this.nodeToFeature.Add((object) this.LOComp, (object) AclFeature.ExternalSettings_LOComp);
      this.nodeToFeature.Add((object) this.editLOComp, (object) AclFeature.ExternalSettings_EditLOComp);
      this.nodeToFeature.Add((object) this.TradeMgmt, (object) AclFeature.ExternalSettings_TradeMgmt);
      this.nodeToFeature.Add((object) this.editTradeMgmt, (object) AclFeature.ExternalSettings_EditTradeMgmt);
      this.nodeToFeature.Add((object) this.ONRP, (object) AclFeature.ExternalSettings_ONRP);
      this.nodeToFeature.Add((object) this.editONRP, (object) AclFeature.ExternalSettings_EditONRP);
      this.nodeToFeature.Add((object) this.notes, (object) AclFeature.ExternalSettings_Notes);
      this.nodeToFeature.Add((object) this.editNotes, (object) AclFeature.ExternalSettings_EditNotes);
      this.nodeToFeature.Add((object) this.deleteNotes, (object) AclFeature.ExternalSettings_DeleteNotes);
      this.nodeToFeature.Add((object) this.webCenterSetup, (object) AclFeature.ExternalSettings_WebCenterSetup);
      this.nodeToFeature.Add((object) this.editWebCenterSetup, (object) AclFeature.ExternalSettings_EditWebCenterSetup);
      this.nodeToFeature.Add((object) this.TPODocumentTab, (object) AclFeature.ExternalSettings_TPOWCDocumentsTab);
      this.nodeToFeature.Add((object) this.DisableGlobalDocsForTPO, (object) AclFeature.ExternalSettings_TPOWCDisableDocumentsTab);
      this.nodeToFeature.Add((object) this.EditTPODocument, (object) AclFeature.ExternalSettings_TPOWCEditDocumentsTab);
      this.nodeToFeature.Add((object) this.DeleteTPODocument, (object) AclFeature.ExternalSettings_TPOWCDeleteDocumentsTab);
      this.nodeToFeature.Add((object) this.TPOWCSiteManagement, (object) AclFeature.ExternalSettings_TPOWCSiteManagementTab);
      this.nodeToFeature.Add((object) this.attachments, (object) AclFeature.ExternalSettings_Attachments);
      this.nodeToFeature.Add((object) this.editAttachments, (object) AclFeature.ExternalSettings_EditAttachments);
      this.nodeToFeature.Add((object) this.deleteAttachments, (object) AclFeature.ExternalSettings_DeleteAttachments);
      this.nodeToFeature.Add((object) this.salesReps, (object) AclFeature.ExternalSettings_SalesReps);
      this.nodeToFeature.Add((object) this.editSalesReps, (object) AclFeature.ExternalSettings_EditSalesReps);
      this.nodeToFeature.Add((object) this.lenderContacts, (object) AclFeature.ExternalSettings_LenderContacts);
      this.nodeToFeature.Add((object) this.editLenderContacts, (object) AclFeature.ExternalSettings_EditLenderContacts);
      this.nodeToFeature.Add((object) this.customFields, (object) AclFeature.ExternalSettings_CustomFields);
      this.nodeToFeature.Add((object) this.customFieldsTab1, (object) AclFeature.ExternalSettings_CustomFieldsTab1);
      this.nodeToFeature.Add((object) this.editCustomFieldsTab1, (object) AclFeature.ExternalSettings_EditCustomFieldsTab1);
      this.nodeToFeature.Add((object) this.customFieldsTab2, (object) AclFeature.ExternalSettings_CustomFieldsTab2);
      this.nodeToFeature.Add((object) this.editCustomFieldsTab2, (object) AclFeature.ExternalSettings_EditCustomFieldsTab2);
      this.nodeToFeature.Add((object) this.customFieldsTab3, (object) AclFeature.ExternalSettings_CustomFieldsTab3);
      this.nodeToFeature.Add((object) this.editCustomFieldsTab3, (object) AclFeature.ExternalSettings_EditCustomFieldsTab3);
      this.nodeToFeature.Add((object) this.customFieldsTab4, (object) AclFeature.ExternalSettings_CustomFieldsTab4);
      this.nodeToFeature.Add((object) this.editCustomFieldsTab4, (object) AclFeature.ExternalSettings_EditCustomFieldsTab4);
      this.nodeToFeature.Add((object) this.customFieldsTab5, (object) AclFeature.ExternalSettings_CustomFieldsTab5);
      this.nodeToFeature.Add((object) this.editCustomFieldsTab5, (object) AclFeature.ExternalSettings_EditCustomFieldsTab5);
      this.nodeToFeature.Add((object) this.contacts, (object) AclFeature.ExternalSettings_Contacts);
      this.nodeToFeature.Add((object) this.contactSalesRep, (object) AclFeature.ExternalSettings_ContactSalesRep);
      this.nodeToFeature.Add((object) this.authorizedTrader, (object) AclFeature.ExternalSettings_AuthorizedTrader);
      this.nodeToFeature.Add((object) this.editContacts, (object) AclFeature.ExternalSettings_EditContacts);
      this.nodeToFeature.Add((object) this.exportContacts, (object) AclFeature.ExternalSettings_ExportContacts);
      this.nodeToFeature.Add((object) this.deleteContacts, (object) AclFeature.ExternalSettings_DeleteContacts);
      this.nodeToFeature.Add((object) this.sendWelcomeEmail, (object) AclFeature.ExternalSettings_SendWelcomeEmail);
      this.nodeToFeature.Add((object) this.resetPassword, (object) AclFeature.ExternalSettings_ResetPassword);
      this.nodeToFeature.Add((object) this.TPOSettings, (object) AclFeature.ExternalSettings_TPOSettings);
      this.nodeToFeature.Add((object) this.TPOFees, (object) AclFeature.ExternalSettings_TPOFees);
      this.nodeToFeature.Add((object) this.editTPOFees, (object) AclFeature.ExternalSettings_EditTPOFees);
      this.nodeToFeature.Add((object) this.deleteTPOFees, (object) AclFeature.ExternalSettings_DeleteTPOFees);
      this.nodeToFeature.Add((object) this.TPOFeesTab, (object) AclFeature.ExternalSettings_TPOFeesTab);
      this.nodeToFeature.Add((object) this.editTPOFeesTab, (object) AclFeature.ExternalSettings_EditTPOFeesTab);
      this.nodeToFeature.Add((object) this.deleteTPOFeesTab, (object) AclFeature.ExternalSettings_DeleteTPOFeesTab);
      this.nodeToFeature.Add((object) this.DBATab, (object) AclFeature.ExternalSettings_DBATab);
      this.nodeToFeature.Add((object) this.editDBATab, (object) AclFeature.ExternalSettings_EditDBATab);
      this.nodeToFeature.Add((object) this.WarehouseTab, (object) AclFeature.ExternalSettings_WarehouseTab);
      this.nodeToFeature.Add((object) this.editWarehouseTab, (object) AclFeature.ExternalSettings_EditWarehouseTab);
      this.nodeToFeature.Add((object) this.TPOAssignment, (object) AclFeature.ExternalSettings_TPOAssignment);
      this.nodeToFeature.Add((object) this.TPOCustomFields, (object) AclFeature.ExternalSettings_TPOCustomFields);
      this.nodeToFeature.Add((object) this.AllTPOInformation, (object) AclFeature.ExternalSettings_AllTPOInformation);
      this.nodeToFeature.Add((object) this.TPOGlobalLenderContact, (object) AclFeature.ExternalSettings_TPOGlobalLenderContact);
      this.nodeToFeature.Add((object) this.TPODocSetting, (object) AclFeature.ExternalSettings_TPOWCDocuments);
      this.nodeToFeature.Add((object) this.EditTPODocSetting, (object) AclFeature.ExternalSettings_TPOWCEditDocuments);
      this.nodeToFeature.Add((object) this.DeleteTPODocSetting, (object) AclFeature.ExternalSettings_TPOWCDeleteDocuments);
      this.nodeToFeature.Add((object) this.TPOConnectSiteManagement, (object) AclFeature.ExternalSettings_TPOConnectSiteManagement);
      this.nodeToFeature.Add((object) this.TPODisclosureSettings, (object) AclFeature.ExternalSettings_TPODisclosureSettings);
      this.nodeToFeature.Add((object) this.GlobalAccess, (object) AclFeature.ExternalSettings_GlobalAccess);
      this.nodeToFeature.Add((object) this.AccountManagement, (object) AclFeature.ExternalSettings_AccountManagement);
      this.nodeToFeature.Add((object) this.exportOrganizations, (object) AclFeature.ExternalSettings_ExportOrganizations);
      this.featureToNodeTbl = new Hashtable(FeatureSets.SettingsTabCompanyFeatures.Length);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_CompanyDetails, (object) this.companyDetails);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_CreateOrganizations, (object) this.createOrganizations);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_DeleteOrganizations, (object) this.deleteOrganizations);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditBanks, (object) this.editBanks);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_DeleteBanks, (object) this.deleteBanks);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_OrganizationSettings, (object) this.organizationSettings);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_CompanyInformation, (object) this.companyInformation);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditCompanyInformation, (object) this.editCompanyInformation);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_License, (object) this.license);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditLicenseInformation, (object) this.editLicenseInformation);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_LoanType, (object) this.loanType);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditLoanTypes, (object) this.editLoanTypes);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOContacts, (object) this.TPOContacts);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditTPOContacts, (object) this.editTPOContacts);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_SalesReps, (object) this.salesReps);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditSalesReps, (object) this.editSalesReps);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_LenderContacts, (object) this.lenderContacts);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditLenderContacts, (object) this.editLenderContacts);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_Commitment, (object) this.commitment);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditCommitment, (object) this.editCommitment);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_LOComp, (object) this.LOComp);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditLOComp, (object) this.editLOComp);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TradeMgmt, (object) this.TradeMgmt);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditTradeMgmt, (object) this.editTradeMgmt);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_ONRP, (object) this.ONRP);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditONRP, (object) this.editONRP);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_Notes, (object) this.notes);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditNotes, (object) this.editNotes);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_DeleteNotes, (object) this.deleteNotes);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_WebCenterSetup, (object) this.webCenterSetup);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditWebCenterSetup, (object) this.editWebCenterSetup);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOWCDocumentsTab, (object) this.TPODocumentTab);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOWCDisableDocumentsTab, (object) this.DisableGlobalDocsForTPO);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOWCEditDocumentsTab, (object) this.EditTPODocument);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOWCDeleteDocumentsTab, (object) this.DeleteTPODocument);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOWCSiteManagementTab, (object) this.TPOWCSiteManagement);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_Attachments, (object) this.attachments);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditAttachments, (object) this.editAttachments);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_DeleteAttachments, (object) this.deleteAttachments);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_CustomFields, (object) this.customFields);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_CustomFieldsTab1, (object) this.customFieldsTab1);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditCustomFieldsTab1, (object) this.editCustomFieldsTab1);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_CustomFieldsTab2, (object) this.customFieldsTab2);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditCustomFieldsTab2, (object) this.editCustomFieldsTab2);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_CustomFieldsTab3, (object) this.customFieldsTab3);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditCustomFieldsTab3, (object) this.editCustomFieldsTab3);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_CustomFieldsTab4, (object) this.customFieldsTab4);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditCustomFieldsTab4, (object) this.editCustomFieldsTab4);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_CustomFieldsTab5, (object) this.customFieldsTab5);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditCustomFieldsTab5, (object) this.editCustomFieldsTab5);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_Contacts, (object) this.contacts);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_ContactSalesRep, (object) this.contactSalesRep);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_AuthorizedTrader, (object) this.authorizedTrader);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditContacts, (object) this.editContacts);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_ExportContacts, (object) this.exportContacts);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_DeleteContacts, (object) this.deleteContacts);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_SendWelcomeEmail, (object) this.sendWelcomeEmail);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_ResetPassword, (object) this.resetPassword);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOSettings, (object) this.TPOSettings);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOFees, (object) this.TPOFees);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditTPOFees, (object) this.editTPOFees);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_DeleteTPOFees, (object) this.deleteTPOFees);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOFeesTab, (object) this.TPOFeesTab);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditTPOFeesTab, (object) this.editTPOFeesTab);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_DeleteTPOFeesTab, (object) this.deleteTPOFeesTab);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_DBATab, (object) this.DBATab);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditDBATab, (object) this.editDBATab);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_WarehouseTab, (object) this.WarehouseTab);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_EditWarehouseTab, (object) this.editWarehouseTab);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOAssignment, (object) this.TPOAssignment);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOCustomFields, (object) this.TPOCustomFields);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_AllTPOInformation, (object) this.AllTPOInformation);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOGlobalLenderContact, (object) this.TPOGlobalLenderContact);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOWCDocuments, (object) this.TPODocSetting);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOWCEditDocuments, (object) this.EditTPODocSetting);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOWCDeleteDocuments, (object) this.DeleteTPODocSetting);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPOConnectSiteManagement, (object) this.TPOConnectSiteManagement);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_TPODisclosureSettings, (object) this.TPODisclosureSettings);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_GlobalAccess, (object) this.GlobalAccess);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_AccountManagement, (object) this.AccountManagement);
      this.featureToNodeTbl.Add((object) AclFeature.ExternalSettings_ExportOrganizations, (object) this.exportOrganizations);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.companyDetails, (object) false);
      this.nodeToUpdateStatus.Add((object) this.createOrganizations, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteOrganizations, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportOrganizations, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editBanks, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteBanks, (object) false);
      this.nodeToUpdateStatus.Add((object) this.organizationSettings, (object) false);
      this.nodeToUpdateStatus.Add((object) this.companyInformation, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editCompanyInformation, (object) false);
      this.nodeToUpdateStatus.Add((object) this.license, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editLicenseInformation, (object) false);
      this.nodeToUpdateStatus.Add((object) this.loanType, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editLoanTypes, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPOContacts, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editTPOContacts, (object) false);
      this.nodeToUpdateStatus.Add((object) this.salesReps, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editSalesReps, (object) false);
      this.nodeToUpdateStatus.Add((object) this.lenderContacts, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editLenderContacts, (object) false);
      this.nodeToUpdateStatus.Add((object) this.commitment, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editCommitment, (object) false);
      this.nodeToUpdateStatus.Add((object) this.LOComp, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editLOComp, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TradeMgmt, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editTradeMgmt, (object) false);
      this.nodeToUpdateStatus.Add((object) this.ONRP, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editONRP, (object) false);
      this.nodeToUpdateStatus.Add((object) this.notes, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editNotes, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteNotes, (object) false);
      this.nodeToUpdateStatus.Add((object) this.webCenterSetup, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editWebCenterSetup, (object) false);
      this.nodeToUpdateStatus.Add((object) this.attachments, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editAttachments, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteAttachments, (object) false);
      this.nodeToUpdateStatus.Add((object) this.customFields, (object) false);
      this.nodeToUpdateStatus.Add((object) this.customFieldsTab1, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editCustomFieldsTab1, (object) false);
      this.nodeToUpdateStatus.Add((object) this.customFieldsTab2, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editCustomFieldsTab2, (object) false);
      this.nodeToUpdateStatus.Add((object) this.customFieldsTab3, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editCustomFieldsTab3, (object) false);
      this.nodeToUpdateStatus.Add((object) this.customFieldsTab4, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editCustomFieldsTab4, (object) false);
      this.nodeToUpdateStatus.Add((object) this.customFieldsTab5, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editCustomFieldsTab5, (object) false);
      this.nodeToUpdateStatus.Add((object) this.contacts, (object) false);
      this.nodeToUpdateStatus.Add((object) this.contactSalesRep, (object) false);
      this.nodeToUpdateStatus.Add((object) this.authorizedTrader, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editContacts, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportContacts, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteContacts, (object) false);
      this.nodeToUpdateStatus.Add((object) this.sendWelcomeEmail, (object) false);
      this.nodeToUpdateStatus.Add((object) this.resetPassword, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPOSettings, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPOFees, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editTPOFees, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteTPOFees, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPOFeesTab, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editTPOFeesTab, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteTPOFeesTab, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPOAssignment, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPOCustomFields, (object) false);
      this.nodeToUpdateStatus.Add((object) this.AllTPOInformation, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPOGlobalLenderContact, (object) false);
      this.nodeToUpdateStatus.Add((object) this.DBATab, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editDBATab, (object) false);
      this.nodeToUpdateStatus.Add((object) this.WarehouseTab, (object) false);
      this.nodeToUpdateStatus.Add((object) this.editWarehouseTab, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPODocSetting, (object) false);
      this.nodeToUpdateStatus.Add((object) this.EditTPODocSetting, (object) false);
      this.nodeToUpdateStatus.Add((object) this.DeleteTPODocSetting, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPOConnectSiteManagement, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPODisclosureSettings, (object) false);
      this.nodeToUpdateStatus.Add((object) this.GlobalAccess, (object) false);
      this.nodeToUpdateStatus.Add((object) this.AccountManagement, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPODocumentTab, (object) false);
      this.nodeToUpdateStatus.Add((object) this.DisableGlobalDocsForTPO, (object) false);
      this.nodeToUpdateStatus.Add((object) this.EditTPODocument, (object) false);
      this.nodeToUpdateStatus.Add((object) this.DeleteTPODocument, (object) false);
      this.nodeToUpdateStatus.Add((object) this.TPOWCSiteManagement, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
    }
  }
}
