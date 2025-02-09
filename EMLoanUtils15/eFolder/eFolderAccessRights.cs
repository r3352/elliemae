// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eFolderAccessRights
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.eFolder
{
  public class eFolderAccessRights
  {
    private LoanDataMgr loanDataMgr;
    private LogRecordBase logEntry;
    private bool canEditLoan;
    private bool canEditUnassignedFiles;
    private bool canEditUnprotectedDocs;
    private bool canEditProtectedDocs;
    private bool canCreateDocs;
    private bool canOrderDisclosures;
    private bool canSetDocumentAccess;
    private bool canSendConsent;
    private bool canRequestRetrieveBorrower;
    private bool canRequestRetrieveServices;
    private RetrieveDocumentMethod retrieveBorrowerMethod;
    private RetrieveDocumentMethod retrieveServiceMethod;
    private bool canEditPreliminary;
    private bool canEditUnderwriting;
    private bool canEditPostClosing;
    private bool canAccessDocumentTab;
    private bool canAccessUnderwritingTab;
    private bool isProtected;
    private bool canEditSellCondition;
    private const string ENHANCEDCONDITIONINVESTORDELIVERYTYPE = "Investor Delivery�";

    public eFolderAccessRights(LoanDataMgr loanDataMgr)
    {
      this.loanDataMgr = loanDataMgr;
      LoanContentAccess contentAccess = loanDataMgr.LoanData.ContentAccess;
      if (contentAccess != LoanContentAccess.None)
        this.canEditLoan = loanDataMgr.Writable;
      if (this.canEditLoan && !this.loanDataMgr.LoanData.IsULDDExporting)
      {
        if ((contentAccess & LoanContentAccess.DocumentTracking) != LoanContentAccess.None)
        {
          this.canEditUnassignedFiles = true;
          this.canEditUnprotectedDocs = true;
          this.canEditProtectedDocs = true;
          this.canCreateDocs = true;
          this.canOrderDisclosures = true;
          this.canSetDocumentAccess = true;
          this.canSendConsent = true;
          this.canRequestRetrieveBorrower = true;
          this.canRequestRetrieveServices = true;
          this.retrieveBorrowerMethod = RetrieveDocumentMethod.AssignCurrent;
          this.retrieveServiceMethod = RetrieveDocumentMethod.AssignCurrent;
        }
        else if ((contentAccess & LoanContentAccess.DocTrackingPartial) != LoanContentAccess.None)
        {
          if ((contentAccess & LoanContentAccess.DocTrackingUnassignedFiles) != LoanContentAccess.None)
            this.canEditUnassignedFiles = true;
          if ((contentAccess & LoanContentAccess.DocTrackingUnprotectedDocs) != LoanContentAccess.None)
            this.canEditUnprotectedDocs = true;
          if ((contentAccess & LoanContentAccess.DocTrackingProtectedDocs) != LoanContentAccess.None)
            this.canEditProtectedDocs = true;
          if ((contentAccess & LoanContentAccess.DocTrackingCreateDocs) != LoanContentAccess.None)
            this.canCreateDocs = true;
          if ((contentAccess & LoanContentAccess.DocTrackingOrderDisclosures) != LoanContentAccess.None)
            this.canOrderDisclosures = true;
          if ((contentAccess & LoanContentAccess.DocTrackingRequestRetrieveBorrower) != LoanContentAccess.None)
            this.canSendConsent = true;
          if ((contentAccess & LoanContentAccess.DocTrackingRequestRetrieveBorrower) != LoanContentAccess.None)
          {
            this.canRequestRetrieveBorrower = true;
            this.retrieveBorrowerMethod = (contentAccess & LoanContentAccess.DocTrackingRetrieveBorrowerCurrent) == LoanContentAccess.None ? ((contentAccess & LoanContentAccess.DocTrackingRetrieveBorrowerNotCurrent) == LoanContentAccess.None ? RetrieveDocumentMethod.Unassigned : RetrieveDocumentMethod.AssignNotCurrent) : RetrieveDocumentMethod.AssignCurrent;
          }
          if ((contentAccess & LoanContentAccess.DocTrackingRequestRetrieveService) != LoanContentAccess.None)
          {
            this.canRequestRetrieveServices = true;
            this.retrieveServiceMethod = (contentAccess & LoanContentAccess.DocTrackingRetrieveServiceCurrent) == LoanContentAccess.None ? ((contentAccess & LoanContentAccess.DocTrackingRetrieveServiceNotCurrent) == LoanContentAccess.None ? RetrieveDocumentMethod.Unassigned : RetrieveDocumentMethod.AssignNotCurrent) : RetrieveDocumentMethod.AssignCurrent;
          }
        }
        if (contentAccess == LoanContentAccess.FullAccess)
          this.canEditPreliminary = true;
        if ((contentAccess & LoanContentAccess.ConditionTracking) != LoanContentAccess.None)
          this.canEditUnderwriting = true;
        if (contentAccess == LoanContentAccess.FullAccess)
        {
          this.canEditPostClosing = true;
          this.canEditSellCondition = true;
        }
      }
      if ((contentAccess & (LoanContentAccess.DocumentTracking | LoanContentAccess.DocTrackingViewOnly | LoanContentAccess.DocTrackingPartial)) != LoanContentAccess.None)
        this.canAccessDocumentTab = true;
      else if (contentAccess == LoanContentAccess.None)
        this.canAccessDocumentTab = true;
      if ((contentAccess & (LoanContentAccess.ConditionTracking | LoanContentAccess.ConditionTrackingViewOnly)) != LoanContentAccess.None)
      {
        this.canAccessUnderwritingTab = true;
      }
      else
      {
        if (contentAccess != LoanContentAccess.None)
          return;
        this.canAccessUnderwritingTab = true;
      }
    }

    public eFolderAccessRights(LoanDataMgr loanDataMgr, LogRecordBase logEntry)
      : this(loanDataMgr)
    {
      this.logEntry = logEntry;
      if (logEntry == null)
        return;
      this.isProtected = loanDataMgr.AccessRules.IsLogEntryProtected(logEntry);
    }

    private bool hasAccessRight(AclFeature feature)
    {
      this.loanDataMgr.SessionObjects.StartupInfo.UserInfo.IsSuperAdministrator();
      return (bool) this.loanDataMgr.SessionObjects.StartupInfo.UserAclFeatureRights[(object) feature];
    }

    public bool CanDeleteFiles
    {
      get
      {
        return this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_DeleteFilePermanently);
      }
    }

    public bool CanAutoAssignFiles
    {
      get => this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_AutoAssign);
    }

    public bool CanSuggestTrainings
    {
      get
      {
        if (this.CanApproveTrainings)
          return true;
        return this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_Suggestor);
      }
    }

    public bool CanApproveTrainings => this.hasAccessRight(AclFeature.eFolder_UF_Approver);

    public bool CanBrowseAttach
    {
      get
      {
        return this.logEntry == null ? this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_BrowseAndAttach) : (this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_BrowseAndAttach) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_BrowseAndAttach));
      }
    }

    public bool CanScanAttach
    {
      get
      {
        return this.logEntry == null ? this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_ScanAndAttach) : (this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_ScanAndAttach) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_ScanAndAttach));
      }
    }

    public bool CanAttachForms
    {
      get
      {
        return this.logEntry == null ? this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_AttachEncompassForms) : (this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_AttachEncompassForms) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_AttachEncompassForms));
      }
    }

    public bool CanAttachUnassignedFiles
    {
      get
      {
        return this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_AttachUnassignedFiles) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_AttachUnassignedFiles);
      }
    }

    public bool CanMoveFilesUpDown
    {
      get
      {
        return this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_MoveFileUpDown) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_MoveFileUpDown);
      }
    }

    public bool CanEditFile
    {
      get
      {
        return this.logEntry == null ? this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_EditFile) : (this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_EditFile) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_EditFile));
      }
    }

    public bool CanDeletePageFromFile
    {
      get
      {
        return this.logEntry == null ? this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_EF_DeletePagePermanently) : (this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_EF_DeletePagePermanently) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_EF_DeletePagePermanently));
      }
    }

    public bool CanMergeFiles
    {
      get
      {
        return this.logEntry == null ? this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_MergeFiles) : (this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_MergeFiles) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_MergeFiles));
      }
    }

    public bool CanSplitFiles
    {
      get
      {
        return this.logEntry == null ? this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_SplitFileUp) : (this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_SplitFileUp) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_SplitFileUp));
      }
    }

    public bool CanAnnotateFiles
    {
      get
      {
        return this.logEntry == null ? this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_AddNotesToFile) : (this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_AddNotesToFile) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_AddNotesToFile));
      }
    }

    public bool CanDeleteAnnotations
    {
      get
      {
        return this.logEntry == null ? this.canEditUnassignedFiles && this.hasAccessRight(AclFeature.eFolder_UF_AN_DeleteNotes) : (this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_AN_DeleteNotes) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_AN_DeleteNotes));
      }
    }

    public bool CanRemoveDocumentFiles
    {
      get
      {
        return this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_RemoveFileFromDoc) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_RemoveFileFromDoc);
      }
    }

    public bool CanMarkFileAsCurrent
    {
      get
      {
        return this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_MarkFileAsCurrentVersion) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_MarkFileAsCurrentVersion);
      }
    }

    public bool CanAccessDocumentTab
    {
      get
      {
        return this.canAccessDocumentTab && this.hasAccessRight(AclFeature.eFolder_AccessToDocumentTab);
      }
    }

    public bool CanAddDocuments
    {
      get => this.canCreateDocs && this.hasAccessRight(AclFeature.eFolder_Other_NewDuplicateDoc);
    }

    public bool CanEditDocument
    {
      get
      {
        return this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_EditDocDetails) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_EditDocDetails);
      }
    }

    public bool CanAddNewDocumentName
    {
      get
      {
        return this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_EDD_CreateNewDocName) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_EDD_CreateNewDocName);
      }
    }

    public bool CanAddDocumentComments
    {
      get
      {
        return this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_AddComment) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_AddComment);
      }
    }

    public bool CanDeleteDocumentComments
    {
      get
      {
        return this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_DeleteComment) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_DeleteComment);
      }
    }

    public bool CanDeleteDocuments
    {
      get
      {
        return this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_DeleteDoc) || this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_DeleteDoc);
      }
    }

    public bool CanSetDocumentAccess
    {
      get
      {
        return this.canSetDocumentAccess && this.hasAccessRight(AclFeature.eFolder_Other_ManageAccessToDocs);
      }
    }

    public bool CanRemoveAccessFromProtectedDocument
    {
      get
      {
        return this.canSetDocumentAccess && this.hasAccessRight(AclFeature.eFolder_Other_MAD_RemoveAccessFromProtectedRoles);
      }
    }

    public bool CanSendConsent
    {
      get => this.canSendConsent && this.hasAccessRight(AclFeature.eFolder_Other_SendConsent);
    }

    public bool CanRequestDocuments
    {
      get
      {
        return this.canRequestRetrieveBorrower && this.hasAccessRight(AclFeature.eFolder_Other_RequestBorrowerDocuments);
      }
    }

    public bool CanRequestServices
    {
      get
      {
        return this.canRequestRetrieveServices && this.hasAccessRight(AclFeature.eFolder_Other_RequestEllieMaeNetworkServices);
      }
    }

    public bool CanSendDisclosures
    {
      get => this.canOrderDisclosures && this.hasAccessRight(AclFeature.eFolder_Other_eDisclosure);
    }

    public bool CanSelectDisclosures
    {
      get
      {
        return this.canOrderDisclosures && this.hasAccessRight(AclFeature.eFolder_Other_eDisclosure_AddAdditionalDocs);
      }
    }

    public bool CanRetrieveDocuments
    {
      get
      {
        return this.canRequestRetrieveBorrower && this.hasAccessRight(AclFeature.eFolder_Other_RetrieveBorrowerDocuments);
      }
    }

    public RetrieveDocumentMethod RetrieveDocumentMethod => this.retrieveBorrowerMethod;

    public bool CanRetrieveServices
    {
      get
      {
        return this.canRequestRetrieveServices && this.hasAccessRight(AclFeature.eFolder_Other_RetrieveEllieMaeNetworkServices);
      }
    }

    public RetrieveDocumentMethod RetrieveServiceMethod => this.retrieveServiceMethod;

    public bool CanSendDocuments
    {
      get => this.canEditLoan && this.hasAccessRight(AclFeature.eFolder_Other_SendFiles);
    }

    public bool CanSubmitDocuments
    {
      get => this.canEditLoan && this.hasAccessRight(AclFeature.eFolder_Other_SendFilesToLender);
    }

    public bool CanArchiveDocuments
    {
      get => this.canEditLoan && this.hasAccessRight(AclFeature.eFolder_Other_ArchiveDocs);
    }

    public bool CanReviewDocuments
    {
      get
      {
        return this.isProtected ? this.canEditProtectedDocs && this.hasAccessRight(AclFeature.eFolder_PD_ED_MarkStatusASReviewed) : this.canEditUnprotectedDocs && this.hasAccessRight(AclFeature.eFolder_UD_ED_MarkStatusASReviewed);
      }
    }

    public bool CanUpdateDocumentStackingTemplate
    {
      get => this.hasAccessRight(AclFeature.SettingsTab_DocumentStackingTemplates);
    }

    public bool CanViewAllAnnotations
    {
      get => this.hasAccessRight(AclFeature.eFolder_Other_ViewAllAnnotations);
    }

    public bool CanAccessEnhancedCondition(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.AccessConditions);
    }

    public bool CanAddEnhancedCondition(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.AddConditions);
    }

    public bool CanAddBlankEnhancedCondition(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.CreateBlankCondition);
    }

    public bool CanAddAutomatedEnhancedCondition(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.AddAutomatedConditions);
    }

    public bool CanDeleteEnhancedCondition(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.DeleteConditions);
    }

    public bool CanEditEnhancedCondition(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.EditConditions);
    }

    public bool CanChangeEnhancedConditionPriorTo(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.ChangePriorTo);
    }

    public bool CanEditEnhancedConditionInternalDescription(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.InternalDescription);
    }

    public bool CanEditEnhancedConditionExternalDescription(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.ExternalDescription);
    }

    public bool CanEditEnhancedConditionPrintInternally(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.PrintInternally);
    }

    public bool CanEditEnhancedConditionPrintExternally(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.PrintExternally);
    }

    public bool CanAddEnhancedConditionComments(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.AddComments);
    }

    public bool CanDeleteEnhancedConditionComments(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.RemoveComments);
    }

    public bool CanMarkEnhancedConditionComments(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.MarkComments);
    }

    public bool CanAssignEnhancedConditionDocuments(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.AssignDocuments);
    }

    public bool CanUnassignEnhancedConditionDocuments(string condTypeTitle)
    {
      return this.hasEnhancedConditionRight(condTypeTitle, AclEnhancedConditionType.UnassignDocuments);
    }

    public bool CanReviewAndImportEnhancedConditions()
    {
      return this.hasEnhancedConditionRight("Investor Delivery", AclEnhancedConditionType.ReviewAndImportConditions);
    }

    public bool CanImportAllEnhancedConditions()
    {
      return this.hasEnhancedConditionRight("Investor Delivery", AclEnhancedConditionType.ImportAllConditions);
    }

    public bool CanDeliverConditionResponses()
    {
      return this.hasEnhancedConditionRight("Investor Delivery", AclEnhancedConditionType.DeliveryConditionResponses);
    }

    public bool CanViewConditionDeliveryStatus()
    {
      return this.hasEnhancedConditionRight("Investor Delivery", AclEnhancedConditionType.ViewConditionDeliveryStatus);
    }

    public bool CanUpdateEnhancedConditionTrackingStatus(StatusTrackingDefinition trackDef)
    {
      return this.loanDataMgr.AccessRules.CanUpdateEnhancedConditionTrackingStatus(trackDef);
    }

    private bool hasEnhancedConditionRight(
      string condTypeTitle,
      AclEnhancedConditionType aclEnhancedConditionType)
    {
      if (this.loanDataMgr.SessionObjects.StartupInfo.UserInfo.IsSuperAdministrator())
        return true;
      if (!this.loanDataMgr.SessionObjects.StartupInfo.UserAclEnhancedConditionRights.ContainsKey(condTypeTitle))
        return false;
      Hashtable enhancedConditionRight = this.loanDataMgr.SessionObjects.StartupInfo.UserAclEnhancedConditionRights[condTypeTitle];
      return enhancedConditionRight.ContainsKey((object) aclEnhancedConditionType) && (bool) enhancedConditionRight[(object) aclEnhancedConditionType];
    }

    public bool CanOrderEcloseDocsWithComplianceFailures
    {
      get
      {
        return this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_OrderDocsWithAuditFailuresDigitalClosing);
      }
    }

    public bool CanAddAdditionalEcloseDocs
    {
      get => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_AddClosingDocsDigitalClosing);
    }

    public bool CanMoveECloseDocsUpDown
    {
      get => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_RearrangeDocsDigitalClosing);
    }

    public bool CanDeselectECloseDocs
    {
      get
      {
        return this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_DeselectClosingDocsDigitalClosing);
      }
    }

    public bool CanApproveECloseForSigning
    {
      get => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_ApproveForSigningDigitalClosing);
    }

    public bool CanUpdateClosingStackingTemplates
    {
      get => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_ManageStackingOrders);
    }

    public bool CanConfigureClosingOptions
    {
      get
      {
        return this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_ConfigureClosingOptionsDigitalClosing);
      }
    }

    public bool HasPackageSigningDateRight
    {
      get => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_PackageSigningDateDigitalClosing);
    }

    public bool HasPackagePreviewRight
    {
      get => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_PackagePreviewDigitalClosing);
    }

    public bool HasPackageExpirationRight
    {
      get => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_PackageExpirationDigitalClosing);
    }

    public bool HasReverseRegistrationRight
    {
      get => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_ENoteTab_ReverseRegistration);
    }

    public bool CanAccessPackagesTab => this.hasAccessRight(AclFeature.eFolder_Other_PackagesTab);

    public bool CanAccessENoteTab => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_eNoteTab);

    public bool CanTransfer
    {
      get => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_ENoteTab_Transfer);
    }

    public bool CanDeactivateEnote
    {
      get => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_ENoteTab_Deactivate);
    }

    public bool CanReverseDeactivation
    {
      get => this.hasAccessRight(AclFeature.LoanTab_EMClosingDocs_ENoteTab_ReverseDeactivation);
    }

    public bool CanAccessPreliminaryTab
    {
      get => this.hasAccessRight(AclFeature.eFolder_Conditions_PreliminaryCondition);
    }

    public bool CanAddPreliminaryConditions
    {
      get
      {
        return this.canEditPreliminary && this.hasAccessRight(AclFeature.eFolder_Conditions_PreliminaryCondition);
      }
    }

    public bool CanEditPreliminaryCondition
    {
      get
      {
        return this.canEditPreliminary && this.hasAccessRight(AclFeature.eFolder_Conditions_PreliminaryCondition);
      }
    }

    public bool CanDeletePreliminaryConditions
    {
      get
      {
        return this.canEditPreliminary && this.hasAccessRight(AclFeature.eFolder_Conditions_PreliminaryCondition);
      }
    }

    public bool CanAddPreliminaryConditionDocuments => this.canEditPreliminary;

    public bool CanRemovePreliminaryConditionDocuments => this.canEditPreliminary;

    public bool CanUsePreliminaryAutomatedCondition
    {
      get
      {
        return this.canEditPreliminary && this.hasAccessRight(AclFeature.eFolder_Conditions_AddBusinessRulePreliminary);
      }
    }

    public bool CanUsePreliminaryConditionImportCondition
    {
      get
      {
        return this.IsSellConditionEnabled && this.canEditPreliminary && this.hasAccessRight(AclFeature.eFolder_Conditions_PreliminaryCondTab_ImportCond);
      }
    }

    public bool CanAccessUnderwritingTab
    {
      get
      {
        return this.canAccessUnderwritingTab && this.hasAccessRight(AclFeature.eFolder_Conditions_UnderWritingCondTab);
      }
    }

    public bool CanAddUnderwritingConditions
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_NewEditImpDel);
      }
    }

    public bool CanEditUnderwritingCondition
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_NewEditImpDel);
      }
    }

    public bool CanDeleteUnderwritingConditions
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_NewEditImpDel);
      }
    }

    public bool CanChangeSignoffName
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_EditUser);
      }
    }

    public bool CanChangeSignoffDate
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_EditDate);
      }
    }

    public bool CanChangePriorTo
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_PriorTo);
      }
    }

    public bool CanFulfillUnderwritingCondition
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_Status_Fulfilled);
      }
    }

    public bool CanReceiveUnderwritingCondition
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_Status_Received);
      }
    }

    public bool CanReviewUnderwritingCondition
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_Status_Reviewed);
      }
    }

    public bool CanRejectUnderwritingCondition
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_Status_Rejected);
      }
    }

    public bool CanClearUnderwritingCondition
    {
      get
      {
        return this.canEditUnderwriting && this.loanDataMgr.AccessRules.IsAllowedToClearCondition((ConditionLog) this.logEntry);
      }
    }

    public bool CanWaiveUnderwritingCondition
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_Status_Waived);
      }
    }

    public bool CanAddUnderwritingConditionComments
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_EditComment);
      }
    }

    public bool CanDeleteUnderwritingConditionComments
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_EditComment);
      }
    }

    public bool CanAddUnderwritingConditionDocuments
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_AddSupportDoc);
      }
    }

    public bool CanRemoveUnderwritingConditionDocuments
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UW_RemoveSupportDoc);
      }
    }

    public bool CanUseUnderwritingAutomatedCondition
    {
      get
      {
        return this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_AddBusinessRuleUnderwriting);
      }
    }

    public bool CanUseUnderwritingConditionImportCondition
    {
      get
      {
        return this.IsSellConditionEnabled && this.canEditUnderwriting && this.hasAccessRight(AclFeature.eFolder_Conditions_UnderwritingCond_ImportCond);
      }
    }

    public bool CanAccessPostClosingTab
    {
      get => this.hasAccessRight(AclFeature.eFolder_Conditions_PostClosingCondTab);
    }

    public bool CanAddPostClosingConditions
    {
      get
      {
        return this.canEditPostClosing && this.hasAccessRight(AclFeature.eFolder_Conditions_PCCT_NewEditImpDel);
      }
    }

    public bool CanEditPostClosingCondition
    {
      get
      {
        return this.canEditPostClosing && this.hasAccessRight(AclFeature.eFolder_Conditions_PCCT_NewEditImpDel);
      }
    }

    public bool CanDeletePostClosingConditions
    {
      get
      {
        return this.canEditPostClosing && this.hasAccessRight(AclFeature.eFolder_Conditions_PCCT_NewEditImpDel);
      }
    }

    public bool CanAddPostClosingConditionDocuments => this.canEditPostClosing;

    public bool CanRemovePostClosingConditionDocuments => this.canEditPostClosing;

    public bool CanUsePostClosingAutomatedCondition
    {
      get
      {
        return this.canEditPostClosing && this.hasAccessRight(AclFeature.eFolder_Conditions_AddBusinessRulePostClosing);
      }
    }

    public bool CanUsePostClosingImportCondition
    {
      get
      {
        return this.IsSellConditionEnabled && this.canEditPostClosing && this.hasAccessRight(AclFeature.eFolder_Conditions_PostClosingCondition_ImportCond);
      }
    }

    public bool CanAccessSellTab
    {
      get
      {
        return this.loanDataMgr.SessionObjects.GetPolicySetting("EnableSellCondition") && this.hasAccessRight(AclFeature.eFolder_Conditions_SellCondTab);
      }
    }

    public bool CanAddSellConditions
    {
      get
      {
        return this.canEditSellCondition && this.hasAccessRight(AclFeature.eFolder_Conditions_SellCond_AddEditDel);
      }
    }

    public bool CanEditSellCondition
    {
      get
      {
        return this.canEditSellCondition && this.hasAccessRight(AclFeature.eFolder_Conditions_SellCond_AddEditDel);
      }
    }

    public bool CanDeleteSellConditions
    {
      get
      {
        return this.canEditSellCondition && this.hasAccessRight(AclFeature.eFolder_Conditions_SellCond_AddEditDel);
      }
    }

    public bool CanAddSellConditionDocuments => this.canEditSellCondition;

    public bool CanRemoveSellConditionDocuments => this.canEditSellCondition;

    public bool CanUseSellConditionImportCondition
    {
      get
      {
        return this.IsSellConditionEnabled && this.canEditSellCondition && this.hasAccessRight(AclFeature.eFolder_Conditions_SellCond_ImportInvestorCond);
      }
    }

    public bool CanUseDeliveryConditionImportAllCondition
    {
      get
      {
        return this.IsSellConditionEnabled && this.canEditSellCondition && this.hasAccessRight(AclFeature.eFolder_Conditions_SellCond_ImportAllDeliveryCond);
      }
    }

    public bool CanUseDeliveryConditionResponse
    {
      get
      {
        return this.IsSellConditionEnabled && this.canEditSellCondition && this.hasAccessRight(AclFeature.eFolder_Conditions_SellCond_DeliverConditionResponse);
      }
    }

    public bool CanUseConditionDeliveryStatus
    {
      get
      {
        return this.IsSellConditionEnabled && this.canEditSellCondition && this.hasAccessRight(AclFeature.eFolder_Conditions_SellCond_ConditionDeliveryStatus);
      }
    }

    private bool IsSellConditionEnabled
    {
      get => this.loanDataMgr.SessionObjects.GetPolicySetting("EnableSellCondition");
    }

    public bool CanAccessHistoryTab
    {
      get => this.hasAccessRight(AclFeature.eFolder_Conditions_HistoryTab);
    }

    public bool CanAccessAIQFeatures
    {
      get => this.hasAIQAccessRight(AclFeature.SettingsTab_EncompassAIQAccess);
    }

    private bool hasAIQAccessRight(AclFeature aclType)
    {
      bool flag = false;
      if (this.loanDataMgr.SessionObjects.StartupInfo.HasAIQLicense)
      {
        if (this.loanDataMgr.SessionObjects.StartupInfo.UserInfo.IsSuperAdministrator())
          flag = true;
        else if (this.loanDataMgr.SessionObjects.StartupInfo.UserAclFeaturConfigRights.ContainsKey(aclType))
        {
          if (((IEnumerable<int>) new int[2]
          {
            1,
            int.MaxValue
          }).Contains<int>(this.loanDataMgr.SessionObjects.StartupInfo.UserAclFeaturConfigRights[aclType]))
            flag = true;
        }
      }
      return flag;
    }
  }
}
