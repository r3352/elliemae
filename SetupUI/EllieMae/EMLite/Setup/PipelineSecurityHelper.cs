// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PipelineSecurityHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PipelineSecurityHelper : FeatureSecurityHelperBase
  {
    protected TreeNode createPipelineViewNode;
    protected TreeNode createNewLoanNode;
    protected TreeNode newBlankLoanNode;
    protected TreeNode newFromTemplateNode;
    protected TreeNode duplicateLoanNode;
    protected TreeNode duplicateLoanForSecondNode;
    protected TreeNode moveLoanNode;
    protected TreeNode deleteLoanNode;
    protected TreeNode importLoanNode;
    protected TreeNode exportLoanNode;
    protected TreeNode hmdaServicesNode;
    protected TreeNode hmdaGenerateHMDALAR;
    protected TreeNode hmdaOrderHMDAServices;
    protected TreeNode nmlsReportNode;
    protected TreeNode transferLoanNode;
    protected TreeNode manageAlertsNode;
    protected TreeNode trashFolderNode;
    protected TreeNode tfRestoreNode;
    protected TreeNode tfDeleteNode;
    protected TreeNode autoRefreshNode;
    protected TreeNode manageServicesNode;
    protected TreeNode searchArchiveFoldersNode;
    protected TreeNode accessToArchiveLoansNode;
    protected TreeNode accessToArchiveFoldersNode;
    protected TreeNode GSEServicesNode;
    protected TreeNode exportULADforDuNode;
    protected TreeNode exportULDDtoFannieMaeNode;
    protected TreeNode exportULDDtoFreddieMacNode;
    protected TreeNode exportPDDtoGinnieMaeNode;
    protected TreeNode exportFannieMaeFormattedFileNode;
    protected TreeNode exportUCDFileNode;
    protected TreeNode freddieMACCAC;
    protected TreeNode fannieMaeUCDTransfer;
    protected TreeNode freddieMacLPABatch;
    protected TreeNode exportILADNode;
    protected TreeNode investorServiceNode;
    protected TreeNode warehouseLendersNode;
    protected TreeNode dueDiligenceServiceNode;
    protected TreeNode hedgeAdvisoryServiceNode;
    protected TreeNode subservicingServiceNode;
    protected TreeNode bidTapeServiceNode;
    protected TreeNode qcAuditServiceNode;
    protected TreeNode wholesaleLenderServiceNode;
    protected TreeNode servicingServiceNode;
    protected TreeNode deliveryServicesNode;
    protected TreeNode viewReceivedLoansNode;
    protected TreeNode manageImportExceptionsNode;

    public PipelineSecurityHelper(Sessions.Session session, string userId, Persona[] personas)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new UserSecurityHelper(this.session, userId, personas, AclCategory.Features, FeatureSets.LoanMgmtFeatures);
    }

    public PipelineSecurityHelper(Sessions.Session session, int personaId)
      : base(session)
    {
      this.securityHelper = (ILevelSecurityHelper) new PersonaSecurityHelper(this.session, personaId, AclCategory.Features, FeatureSets.LoanMgmtFeatures);
    }

    public Hashtable SpecialDepTreeNodes
    {
      get => this.securityHelper.getSpecialDepTreeNodes();
      set => this.securityHelper.setSpecialDepTreeNodes(value);
    }

    public override void BuildNodes(TreeView treeView)
    {
      this.createPipelineViewNode = new TreeNode("Create Pipeline Views");
      this.createNewLoanNode = new TreeNode("New Loans");
      this.newBlankLoanNode = new TreeNode("New Blank Loan");
      this.newFromTemplateNode = new TreeNode("New from Template");
      this.duplicateLoanNode = new TreeNode("Duplicate Loans");
      this.duplicateLoanNode.Tag = (object) AclFeature.LoanMgmt_Duplicate;
      this.duplicateLoanNode.ForeColor = Color.Blue;
      this.duplicateLoanForSecondNode = new TreeNode("Duplicate Loans for Second");
      this.moveLoanNode = new TreeNode("Move Loans");
      this.moveLoanNode.Tag = (object) AclFeature.LoanMgmt_Move;
      this.moveLoanNode.ForeColor = Color.Blue;
      this.deleteLoanNode = new TreeNode("Delete Loans");
      this.importLoanNode = new TreeNode("Import Loans");
      this.importLoanNode.Tag = (object) AclFeature.LoanMgmt_Import;
      this.importLoanNode.ForeColor = Color.Blue;
      this.transferLoanNode = new TreeNode("Transfer Loans");
      this.exportLoanNode = new TreeNode("Export Data to Excel");
      this.hmdaServicesNode = new TreeNode("HMDA Services");
      this.hmdaGenerateHMDALAR = new TreeNode("Generate HMDA LAR");
      this.hmdaOrderHMDAServices = new TreeNode("Order HMDA Services");
      this.hmdaServicesNode.Nodes.AddRange(new TreeNode[2]
      {
        this.hmdaGenerateHMDALAR,
        this.hmdaOrderHMDAServices
      });
      this.nmlsReportNode = new TreeNode("Generate NMLS Report");
      this.manageServicesNode = new TreeNode("Manage Pipeline Services");
      this.manageServicesNode.Tag = (object) AclFeature.LoanMgmt_MgmtPipelineServices;
      this.manageServicesNode.ForeColor = Color.Blue;
      this.deliveryServicesNode = new TreeNode("Delivery Services");
      this.viewReceivedLoansNode = new TreeNode("View Received Loans");
      this.manageImportExceptionsNode = new TreeNode("Manage Import Exceptions");
      this.createNewLoanNode.Nodes.AddRange(new TreeNode[2]
      {
        this.newBlankLoanNode,
        this.newFromTemplateNode
      });
      this.manageAlertsNode = new TreeNode("Manage Alerts");
      this.autoRefreshNode = new TreeNode("Automatic Refresh Configuration");
      bool flag1 = string.Equals(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "EnableLoanSoftArchival"), "true", StringComparison.OrdinalIgnoreCase);
      if (flag1)
      {
        this.accessToArchiveFoldersNode = new TreeNode("Access to Archive Folders");
        this.accessToArchiveLoansNode = new TreeNode("Access to Archive Loans");
      }
      else
        this.searchArchiveFoldersNode = new TreeNode("Exclude Archive Folders");
      this.GSEServicesNode = new TreeNode("GSE Services ");
      this.exportULADforDuNode = new TreeNode("Export ULAD");
      this.exportULDDtoFannieMaeNode = new TreeNode("Export ULDD to Fannie Mae");
      this.exportULDDtoFreddieMacNode = new TreeNode("Export ULDD to Freddie Mac");
      this.exportPDDtoGinnieMaeNode = new TreeNode("Export PDD to Ginnie Mae");
      this.exportFannieMaeFormattedFileNode = new TreeNode("Export Fannie Mae Formatted File (3.2)");
      this.exportUCDFileNode = new TreeNode("Export UCD file");
      this.freddieMACCAC = new TreeNode("Freddie Mac Loan Assignment and Release");
      this.fannieMaeUCDTransfer = new TreeNode("Fannie Mae UCD Transfer");
      this.freddieMacLPABatch = new TreeNode("Freddie Mac LPA Batch");
      this.exportILADNode = new TreeNode("Export iLAD");
      this.GSEServicesNode.Nodes.AddRange(new TreeNode[10]
      {
        this.exportULADforDuNode,
        this.exportULDDtoFannieMaeNode,
        this.exportULDDtoFreddieMacNode,
        this.exportPDDtoGinnieMaeNode,
        this.exportFannieMaeFormattedFileNode,
        this.exportUCDFileNode,
        this.freddieMACCAC,
        this.fannieMaeUCDTransfer,
        this.freddieMacLPABatch,
        this.exportILADNode
      });
      this.investorServiceNode = new TreeNode("Investor Services");
      this.investorServiceNode.Tag = (object) AclFeature.LoanMgmt_Investor_Service;
      this.investorServiceNode.ForeColor = Color.Blue;
      this.warehouseLendersNode = new TreeNode("Warehouse Lender Services");
      this.warehouseLendersNode.Tag = (object) AclFeature.LoanMgmt_Investor_Warehouse_Lenders;
      this.warehouseLendersNode.ForeColor = Color.Blue;
      this.dueDiligenceServiceNode = new TreeNode("Due Diligence Services");
      this.dueDiligenceServiceNode.Tag = (object) AclFeature.LoanMgmt_Investor_Due_Diligence;
      this.dueDiligenceServiceNode.ForeColor = Color.Blue;
      this.hedgeAdvisoryServiceNode = new TreeNode("Hedge Advisory Services");
      this.hedgeAdvisoryServiceNode.Tag = (object) AclFeature.LoanMgmt_Investor_Hedge_Advisory;
      this.hedgeAdvisoryServiceNode.ForeColor = Color.Blue;
      this.subservicingServiceNode = new TreeNode("Subservicing Services");
      this.subservicingServiceNode.Tag = (object) AclFeature.LoanMgmt_Investor_Subservicing_Services;
      this.subservicingServiceNode.ForeColor = Color.Blue;
      this.bidTapeServiceNode = new TreeNode("Bid Tape");
      this.bidTapeServiceNode.Tag = (object) AclFeature.LoanMgmt_Investor_Bid_Tape_Services;
      this.bidTapeServiceNode.ForeColor = Color.Blue;
      this.qcAuditServiceNode = new TreeNode("QC Audit Services");
      this.qcAuditServiceNode.Tag = (object) AclFeature.LoanMgmt_Investor_QC_Audit_Services;
      this.qcAuditServiceNode.ForeColor = Color.Blue;
      this.wholesaleLenderServiceNode = new TreeNode("Wholesale Lender Services");
      this.wholesaleLenderServiceNode.Tag = (object) AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services;
      this.wholesaleLenderServiceNode.ForeColor = Color.Blue;
      this.servicingServiceNode = new TreeNode("Servicing Services");
      this.servicingServiceNode.Tag = (object) AclFeature.LoanMgmt_Investor_Servicing_Services;
      this.servicingServiceNode.ForeColor = Color.Blue;
      this.trashFolderNode = new TreeNode("Trash Folder Tasks");
      this.tfRestoreNode = new TreeNode("Restore Loans to Folder");
      this.tfDeleteNode = new TreeNode("Delete Loans Permanently");
      this.trashFolderNode.Nodes.AddRange(new TreeNode[2]
      {
        this.tfRestoreNode,
        this.tfDeleteNode
      });
      DataDocsServiceHelper docsServiceHelper = new DataDocsServiceHelper(this.session);
      bool flag2 = docsServiceHelper.HasServiceAcccess(WarehouseLendersServicePage.InvestorCategory);
      bool flag3 = docsServiceHelper.HasServiceAcccess(DueDiligenceServicePage.InvestorCategory);
      bool flag4 = docsServiceHelper.HasServiceAcccess(HedgeAdvisoryServicePage.InvestorCategory);
      bool flag5 = docsServiceHelper.HasServiceAcccess(SubservicingServicePage.InvestorCategory);
      bool flag6 = docsServiceHelper.HasServiceAcccess(BidTapeServicePage.InvestorCategory);
      bool flag7 = docsServiceHelper.HasServiceAcccess(QCAuditServicesPage.InvestorCategory);
      bool flag8 = docsServiceHelper.HasServiceAcccess(WholesaleLenderServicePage.InvestorCategory);
      bool flag9 = docsServiceHelper.HasServiceAcccess(ServicingServicePage.InvestorCategory);
      treeView.Nodes.AddRange(new TreeNode[4]
      {
        this.createPipelineViewNode,
        this.createNewLoanNode,
        this.GSEServicesNode,
        this.investorServiceNode
      });
      if (flag2)
        treeView.Nodes.Add(this.warehouseLendersNode);
      if (flag8)
        treeView.Nodes.Add(this.wholesaleLenderServiceNode);
      if (flag3)
        treeView.Nodes.Add(this.dueDiligenceServiceNode);
      if (flag7)
        treeView.Nodes.Add(this.qcAuditServiceNode);
      if (flag9)
        treeView.Nodes.Add(this.servicingServiceNode);
      if (flag5)
        treeView.Nodes.Add(this.subservicingServiceNode);
      if (flag6)
        treeView.Nodes.Add(this.bidTapeServiceNode);
      if (flag4)
        treeView.Nodes.Add(this.hedgeAdvisoryServiceNode);
      treeView.Nodes.AddRange(new TreeNode[13]
      {
        this.duplicateLoanNode,
        this.duplicateLoanForSecondNode,
        this.moveLoanNode,
        this.deleteLoanNode,
        this.importLoanNode,
        this.exportLoanNode,
        this.manageServicesNode,
        this.hmdaServicesNode,
        this.nmlsReportNode,
        this.transferLoanNode,
        this.trashFolderNode,
        this.manageAlertsNode,
        this.autoRefreshNode
      });
      if (flag1)
        treeView.Nodes.AddRange(new TreeNode[2]
        {
          this.accessToArchiveFoldersNode,
          this.accessToArchiveLoansNode
        });
      else
        treeView.Nodes.AddRange(new TreeNode[1]
        {
          this.searchArchiveFoldersNode
        });
      treeView.ExpandAll();
      this.dependentNodes.AddRange((ICollection) new TreeNode[8]
      {
        this.createNewLoanNode,
        this.moveLoanNode,
        this.importLoanNode,
        this.trashFolderNode,
        this.duplicateLoanNode,
        this.manageServicesNode,
        this.hmdaServicesNode,
        this.investorServiceNode
      });
      if (flag2)
        this.dependentNodes.Add((object) this.warehouseLendersNode);
      if (flag8)
        this.dependentNodes.Add((object) this.wholesaleLenderServiceNode);
      if (flag3)
        this.dependentNodes.Add((object) this.dueDiligenceServiceNode);
      if (flag7)
        this.dependentNodes.Add((object) this.qcAuditServiceNode);
      if (flag9)
        this.dependentNodes.Add((object) this.servicingServiceNode);
      if (flag5)
        this.dependentNodes.Add((object) this.subservicingServiceNode);
      if (flag6)
        this.dependentNodes.Add((object) this.bidTapeServiceNode);
      if (flag4)
        this.dependentNodes.Add((object) this.hedgeAdvisoryServiceNode);
      this.nodeToFeature = new Hashtable();
      this.nodeToFeature.Add((object) this.createPipelineViewNode, (object) AclFeature.LoanMgmt_CreatePipelineViews);
      this.nodeToFeature.Add((object) this.newBlankLoanNode, (object) AclFeature.LoanMgmt_CreateBlank);
      this.nodeToFeature.Add((object) this.newFromTemplateNode, (object) AclFeature.LoanMgmt_CreateFromTmpl);
      this.nodeToFeature.Add((object) this.duplicateLoanForSecondNode, (object) AclFeature.LoanMgmt_Duplicate_For_Second);
      this.nodeToFeature.Add((object) this.deleteLoanNode, (object) AclFeature.LoanMgmt_Delete);
      this.nodeToFeature.Add((object) this.transferLoanNode, (object) AclFeature.LoanMgmt_Transfer);
      this.nodeToFeature.Add((object) this.manageAlertsNode, (object) AclFeature.LoanMgmt_Pipeline_Alert);
      this.nodeToFeature.Add((object) this.exportLoanNode, (object) AclFeature.LoanMgmt_ExportToExcel);
      this.nodeToFeature.Add((object) this.hmdaGenerateHMDALAR, (object) AclFeature.LoanMgmt_HMDAGenerateHMDALAR);
      this.nodeToFeature.Add((object) this.hmdaOrderHMDAServices, (object) AclFeature.LoanMgmt_HMDAOrderHMDAServices);
      this.nodeToFeature.Add((object) this.nmlsReportNode, (object) AclFeature.LoanMgmt_GenerateNMLSReport);
      this.nodeToFeature.Add((object) this.tfRestoreNode, (object) AclFeature.LoanMgmt_TF_Restore);
      this.nodeToFeature.Add((object) this.tfDeleteNode, (object) AclFeature.LoanMgmt_TF_Delete);
      this.nodeToFeature.Add((object) this.autoRefreshNode, (object) AclFeature.LoanMgmt_PipelineAutoRefresh);
      if (flag1)
      {
        this.nodeToFeature.Add((object) this.accessToArchiveFoldersNode, (object) AclFeature.LoanMgmt_AccessToArchiveFolders);
        this.nodeToFeature.Add((object) this.accessToArchiveLoansNode, (object) AclFeature.LoanMgmt_AccessToArchiveLoans);
      }
      else
        this.nodeToFeature.Add((object) this.searchArchiveFoldersNode, (object) AclFeature.LoanMgmt_SearchArchiveFolders);
      this.nodeToFeature.Add((object) this.GSEServicesNode, (object) AclFeature.LoanMgmt_GSE_Services);
      this.nodeToFeature.Add((object) this.exportULADforDuNode, (object) AclFeature.LoanMgmt_Export_ULAD_ForDu);
      this.nodeToFeature.Add((object) this.exportULDDtoFannieMaeNode, (object) AclFeature.LoanMgmt_Export_ULDD_FannieMae);
      this.nodeToFeature.Add((object) this.exportULDDtoFreddieMacNode, (object) AclFeature.LoanMgmt_Export_ULDD_FreddieMac);
      this.nodeToFeature.Add((object) this.exportPDDtoGinnieMaeNode, (object) AclFeature.LoanMgmt_Export_PDD_GinnieMae);
      this.nodeToFeature.Add((object) this.exportFannieMaeFormattedFileNode, (object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile);
      this.nodeToFeature.Add((object) this.exportUCDFileNode, (object) AclFeature.LoanMgmt_Export_FannieMae_UcdFile);
      this.nodeToFeature.Add((object) this.freddieMACCAC, (object) AclFeature.Freddie_Mac_CAC);
      this.nodeToFeature.Add((object) this.fannieMaeUCDTransfer, (object) AclFeature.Fannie_Mae_UCD_Transfer);
      this.nodeToFeature.Add((object) this.freddieMacLPABatch, (object) AclFeature.Freddie_Mac_LPA_Batch);
      this.nodeToFeature.Add((object) this.exportILADNode, (object) AclFeature.LoanMgmt_Export_ILAD);
      this.nodeToFeature.Add((object) this.viewReceivedLoansNode, (object) AclFeature.LoanMgmt_ViewReceivedLoans);
      this.nodeToFeature.Add((object) this.manageImportExceptionsNode, (object) AclFeature.LoanMgmt_ManageImportExceptions);
      this.nodeToFeature.Add((object) this.investorServiceNode, (object) AclFeature.LoanMgmt_Investor_Service);
      this.nodeToFeature.Add((object) this.warehouseLendersNode, (object) AclFeature.LoanMgmt_Investor_Warehouse_Lenders);
      this.featureToNodeTbl = new Hashtable();
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_CreatePipelineViews, (object) this.createPipelineViewNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_CreateBlank, (object) this.newBlankLoanNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_CreateFromTmpl, (object) this.newFromTemplateNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Duplicate_For_Second, (object) this.duplicateLoanForSecondNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Delete, (object) this.deleteLoanNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Transfer, (object) this.transferLoanNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Pipeline_Alert, (object) this.manageAlertsNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_ExportToExcel, (object) this.exportLoanNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_HMDAGenerateHMDALAR, (object) this.hmdaGenerateHMDALAR);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_HMDAOrderHMDAServices, (object) this.hmdaOrderHMDAServices);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_GenerateNMLSReport, (object) this.nmlsReportNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_TF_Restore, (object) this.tfRestoreNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_TF_Delete, (object) this.tfDeleteNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_PipelineAutoRefresh, (object) this.autoRefreshNode);
      if (flag1)
      {
        this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_AccessToArchiveFolders, (object) this.accessToArchiveFoldersNode);
        this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_AccessToArchiveLoans, (object) this.accessToArchiveLoansNode);
      }
      else
        this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_SearchArchiveFolders, (object) this.searchArchiveFoldersNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_GSE_Services, (object) this.GSEServicesNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Export_ULAD_ForDu, (object) this.exportULADforDuNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Export_ULDD_FannieMae, (object) this.exportULDDtoFannieMaeNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Export_ULDD_FreddieMac, (object) this.exportULDDtoFreddieMacNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Export_PDD_GinnieMae, (object) this.exportPDDtoGinnieMaeNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Export_FannieMae_FormattedFile, (object) this.exportFannieMaeFormattedFileNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Export_FannieMae_UcdFile, (object) this.exportUCDFileNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Investor_Service, (object) this.investorServiceNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Export_ILAD, (object) this.exportILADNode);
      if (flag2)
        this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Investor_Warehouse_Lenders, (object) this.warehouseLendersNode);
      if (flag8)
        this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, (object) this.wholesaleLenderServiceNode);
      if (flag3)
        this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Investor_Due_Diligence, (object) this.dueDiligenceServiceNode);
      if (flag4)
        this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Investor_Hedge_Advisory, (object) this.hedgeAdvisoryServiceNode);
      if (flag7)
        this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Investor_QC_Audit_Services, (object) this.qcAuditServiceNode);
      if (flag9)
        this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Investor_Servicing_Services, (object) this.servicingServiceNode);
      if (flag5)
        this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Investor_Subservicing_Services, (object) this.subservicingServiceNode);
      if (flag6)
        this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_Investor_Bid_Tape_Services, (object) this.bidTapeServiceNode);
      this.featureToNodeTbl.Add((object) AclFeature.Freddie_Mac_CAC, (object) this.freddieMACCAC);
      this.featureToNodeTbl.Add((object) AclFeature.Fannie_Mae_UCD_Transfer, (object) this.fannieMaeUCDTransfer);
      this.featureToNodeTbl.Add((object) AclFeature.Freddie_Mac_LPA_Batch, (object) this.freddieMacLPABatch);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_ViewReceivedLoans, (object) this.viewReceivedLoansNode);
      this.featureToNodeTbl.Add((object) AclFeature.LoanMgmt_ManageImportExceptions, (object) this.manageImportExceptionsNode);
      this.nodeToUpdateStatus = new Hashtable();
      this.nodeToUpdateStatus.Add((object) this.createPipelineViewNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.newBlankLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.newFromTemplateNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.duplicateLoanForSecondNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.deleteLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.transferLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.manageAlertsNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportLoanNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.hmdaGenerateHMDALAR, (object) false);
      this.nodeToUpdateStatus.Add((object) this.hmdaOrderHMDAServices, (object) false);
      this.nodeToUpdateStatus.Add((object) this.nmlsReportNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tfRestoreNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.tfDeleteNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.autoRefreshNode, (object) false);
      if (flag1)
      {
        this.nodeToUpdateStatus.Add((object) this.accessToArchiveFoldersNode, (object) false);
        this.nodeToUpdateStatus.Add((object) this.accessToArchiveLoansNode, (object) false);
      }
      else
        this.nodeToUpdateStatus.Add((object) this.searchArchiveFoldersNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.GSEServicesNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportULADforDuNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportULDDtoFannieMaeNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportULDDtoFreddieMacNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportPDDtoGinnieMaeNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportFannieMaeFormattedFileNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportUCDFileNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.investorServiceNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.exportILADNode, (object) false);
      if (flag2)
        this.nodeToUpdateStatus.Add((object) this.warehouseLendersNode, (object) false);
      if (flag8)
        this.nodeToUpdateStatus.Add((object) this.wholesaleLenderServiceNode, (object) false);
      if (flag3)
        this.nodeToUpdateStatus.Add((object) this.dueDiligenceServiceNode, (object) false);
      if (flag4)
        this.nodeToUpdateStatus.Add((object) this.hedgeAdvisoryServiceNode, (object) false);
      if (flag7)
        this.nodeToUpdateStatus.Add((object) this.qcAuditServiceNode, (object) false);
      if (flag9)
        this.nodeToUpdateStatus.Add((object) this.servicingServiceNode, (object) false);
      if (flag5)
        this.nodeToUpdateStatus.Add((object) this.subservicingServiceNode, (object) false);
      if (flag6)
        this.nodeToUpdateStatus.Add((object) this.bidTapeServiceNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.fannieMaeUCDTransfer, (object) false);
      this.nodeToUpdateStatus.Add((object) this.freddieMACCAC, (object) false);
      this.nodeToUpdateStatus.Add((object) this.freddieMacLPABatch, (object) false);
      this.nodeToUpdateStatus.Add((object) this.viewReceivedLoansNode, (object) false);
      this.nodeToUpdateStatus.Add((object) this.manageImportExceptionsNode, (object) false);
      this.securityHelper.SetTables(this.featureToNodeTbl, this.nodeToFeature, this.dependentNodes, this.nodeToUpdateStatus);
    }
  }
}
