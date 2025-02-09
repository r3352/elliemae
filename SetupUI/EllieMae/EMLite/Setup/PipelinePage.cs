// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PipelinePage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PipelinePage : PersonaTreePageBase
  {
    private Label label2;
    private IContainer components;
    private int personaID = -1;
    private string userID = "";
    private Persona[] personaList;
    private bool isPersonal;
    private MoveLoanFolderDlg mlfDlg;
    private ImportLoanDlg impDlg;
    private LoanDuplicationTemplatePage dupDlg;
    private ExportServices exportServicesDlg;
    private SelectInvestorsPage investorServicesDlg;
    private WarehouseLendersServicePage warehouseLenderServicesDlg;
    private DueDiligenceServicePage dueDiligenceServiceDlg;
    private HedgeAdvisoryServicePage hedgeAdvisoryServiceDlg;
    private SubservicingServicePage subservicingServicesDlg;
    private BidTapeServicePage bidTapeServicesDlg;
    private QCAuditServicesPage qcAuditServicesDlg;
    private WholesaleLenderServicePage wholesaleLenderServicesDlg;
    private ServicingServicePage servicingServicesDlg;
    private Hashtable cachedData;
    private int selectOption = 2;
    private PipelineConfiguration pipelineConfiguration;

    public PipelinePage(
      Sessions.Session session,
      int personaId,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.pipelineConfiguration = pipelineConfiguration;
      this.Title = "Pipeline Tasks";
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new PipelineSecurityHelper(this.session, personaId);
      this.personaID = personaId;
      this.cachedData = new Hashtable();
      this.bInit = true;
      this.InitialSpecialDepNodes();
      this.init();
    }

    public PipelinePage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.pipelineConfiguration = pipelineConfiguration;
      this.Title = "Pipeline Tasks";
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.bIsUserSetup = true;
      this.isPersonal = true;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.userID = userId;
      this.personaList = personas;
      this.cachedData = new Hashtable();
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new PipelineSecurityHelper(this.session, userId, personas);
      this.bInit = true;
      this.InitialSpecialDepNodes();
      this.init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label2 = new Label();
      this.SuspendLayout();
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(8, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(224, 16);
      this.label2.TabIndex = 2;
      this.label2.Text = "Loan Management";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(528, 361);
      this.Name = nameof (PipelinePage);
      this.ResumeLayout(false);
    }

    private void NodeMouseUp(TreeNode node)
    {
      if (!this.pipelineConfiguration.HasPipelineLoanTabAccess())
        return;
      if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Move)
      {
        if (!this.isPersonal)
        {
          if (this.mlfDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) (AclFeature.LoanMgmt_Move.ToString() + "From")))
            {
              this.mlfDlg = new MoveLoanFolderDlg(this.session, this.personaID, this.readOnly, 2);
              this.cachedData[(object) (AclFeature.LoanMgmt_Move.ToString() + "From")] = (object) this.mlfDlg.DataViewFrom;
              this.cachedData[(object) (AclFeature.LoanMgmt_Move.ToString() + "To")] = (object) this.mlfDlg.DataViewTo;
            }
            this.mlfDlg = new MoveLoanFolderDlg(this.session, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
        }
        else if (this.mlfDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) (AclFeature.LoanMgmt_Move.ToString() + "From")))
          {
            this.mlfDlg = new MoveLoanFolderDlg(this.session, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) (AclFeature.LoanMgmt_Move.ToString() + "From")] = (object) this.mlfDlg.DataViewFrom;
            this.cachedData[(object) (AclFeature.LoanMgmt_Move.ToString() + "To")] = (object) this.mlfDlg.DataViewTo;
          }
          this.mlfDlg = new MoveLoanFolderDlg(this.session, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        this.mlfDlg.isReadOnly = this.readOnly;
        if (this.cachedData[(object) (AclFeature.LoanMgmt_Move.ToString() + "From")] != null)
        {
          this.mlfDlg.DataViewFrom = (ArrayList) this.cachedData[(object) (AclFeature.LoanMgmt_Move.ToString() + "From")];
          this.mlfDlg.DataViewTo = (ArrayList) this.cachedData[(object) (AclFeature.LoanMgmt_Move.ToString() + "To")];
        }
        if (DialogResult.OK == this.mlfDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) (AclFeature.LoanMgmt_Move.ToString() + "From")] = (object) this.mlfDlg.DataViewFrom;
          this.cachedData[(object) (AclFeature.LoanMgmt_Move.ToString() + "To")] = (object) this.mlfDlg.DataViewTo;
          if (this.mlfDlg.HasBeenModified())
            this.setDirtyFlag(true);
        }
        if (this.mlfDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.mlfDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.isPersonal)
          return;
        node.ImageIndex = this.mlfDlg.GetImageIndex();
        node.SelectedImageIndex = this.mlfDlg.GetImageIndex();
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Import)
      {
        if (!this.isPersonal)
        {
          if (this.impDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Import))
            {
              this.impDlg = new ImportLoanDlg(this.session, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanMgmt_Import] = (object) this.impDlg.DataView;
            }
            this.impDlg = new ImportLoanDlg(this.session, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
        }
        else if (this.impDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Import))
          {
            this.impDlg = new ImportLoanDlg(this.session, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Import] = (object) this.impDlg.DataView;
          }
          this.impDlg = new ImportLoanDlg(this.session, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        if (this.cachedData[(object) AclFeature.LoanMgmt_Import] != null)
          this.impDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Import];
        this.impDlg.IsReadOnly = this.readOnly;
        if (DialogResult.OK == this.impDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_Import] = (object) this.impDlg.DataView;
          if (this.impDlg.HasBeenModified())
            this.setDirtyFlag(true);
        }
        if (this.impDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.impDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.isPersonal)
          return;
        node.ImageIndex = this.impDlg.GetImageIndex();
        node.SelectedImageIndex = this.impDlg.GetImageIndex();
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Duplicate)
      {
        if (!this.isPersonal)
        {
          if (this.dupDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Duplicate))
            {
              this.dupDlg = new LoanDuplicationTemplatePage(this.session, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanMgmt_Duplicate] = (object) this.dupDlg.DataView;
            }
            this.dupDlg = new LoanDuplicationTemplatePage(this.session, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
        }
        else if (this.dupDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Duplicate))
          {
            this.dupDlg = new LoanDuplicationTemplatePage(this.session, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Duplicate] = (object) this.dupDlg.DataView;
          }
          this.dupDlg = new LoanDuplicationTemplatePage(this.session, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        this.dupDlg.isReadOnly = this.readOnly;
        if (this.cachedData[(object) AclFeature.LoanMgmt_Duplicate] != null)
          this.dupDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Duplicate];
        if (DialogResult.OK == this.dupDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_Duplicate] = (object) this.dupDlg.DataView;
          if (this.dupDlg.HasBeenModified())
            this.setDirtyFlag(true);
        }
        else if (this.dupDlg != null)
          this.dupDlg.InitForm();
        if (this.dupDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.dupDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.isPersonal)
          return;
        node.ImageIndex = this.dupDlg.GetImageIndex();
        node.SelectedImageIndex = this.dupDlg.GetImageIndex();
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_MgmtPipelineServices)
      {
        if (!this.bIsUserSetup)
        {
          if (this.exportServicesDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_MgmtPipelineServices))
            {
              this.exportServicesDlg = new ExportServices(this.session, AclFeature.LoanMgmt_MgmtPipelineServices, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanTab_Other_ePASS] = (object) this.exportServicesDlg.DataView;
            }
            this.exportServicesDlg = new ExportServices(this.session, AclFeature.LoanMgmt_MgmtPipelineServices, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
          else
            this.exportServicesDlg.IsReadOnly = this.readOnly;
        }
        else if (this.exportServicesDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanTab_Other_ePASS))
          {
            this.exportServicesDlg = new ExportServices(this.session, AclFeature.LoanMgmt_MgmtPipelineServices, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanTab_Other_ePASS] = (object) this.exportServicesDlg.DataView;
          }
          this.exportServicesDlg = new ExportServices(this.session, AclFeature.LoanMgmt_MgmtPipelineServices, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.exportServicesDlg.IsReadOnly = this.readOnly;
        if (this.cachedData[(object) AclFeature.LoanMgmt_MgmtPipelineServices] != null)
          this.exportServicesDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_MgmtPipelineServices];
        if (DialogResult.OK == this.exportServicesDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_MgmtPipelineServices] = (object) this.exportServicesDlg.DataView;
          if (this.exportServicesDlg.HasBeenModified)
            this.setDirtyFlag(true);
        }
        if (this.exportServicesDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.exportServicesDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.bIsUserSetup)
          return;
        node.ImageIndex = this.exportServicesDlg.GetImageIndex();
        node.SelectedImageIndex = this.exportServicesDlg.GetImageIndex();
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Investor_Service)
      {
        if (!this.bIsUserSetup)
        {
          if (this.investorServicesDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Service))
            {
              this.investorServicesDlg = new SelectInvestorsPage(this.session, AclFeature.LoanMgmt_Investor_Service, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanMgmt_Investor_Service] = (object) this.investorServicesDlg.DataView;
            }
            this.investorServicesDlg = new SelectInvestorsPage(this.session, AclFeature.LoanMgmt_Investor_Service, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
          else
            this.investorServicesDlg.IsReadOnly = this.readOnly;
        }
        else if (this.investorServicesDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Service))
          {
            this.investorServicesDlg = new SelectInvestorsPage(this.session, AclFeature.LoanMgmt_Investor_Service, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Investor_Service] = (object) this.investorServicesDlg.DataView;
          }
          this.investorServicesDlg = new SelectInvestorsPage(this.session, AclFeature.LoanMgmt_Investor_Service, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.investorServicesDlg.IsReadOnly = this.readOnly;
        if (this.cachedData[(object) AclFeature.LoanMgmt_Investor_Service] != null)
          this.investorServicesDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Investor_Service];
        if (DialogResult.OK == this.investorServicesDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_Investor_Service] = (object) this.investorServicesDlg.DataView;
          if (this.investorServicesDlg.HasBeenModified)
            this.setDirtyFlag(true);
        }
        if (this.investorServicesDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.investorServicesDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.bIsUserSetup)
          return;
        node.ImageIndex = this.investorServicesDlg.GetImageIndex();
        node.SelectedImageIndex = this.investorServicesDlg.GetImageIndex();
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Investor_Warehouse_Lenders)
      {
        if (!this.bIsUserSetup)
        {
          if (this.warehouseLenderServicesDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Warehouse_Lenders))
            {
              this.warehouseLenderServicesDlg = new WarehouseLendersServicePage(this.session, AclFeature.LoanMgmt_Investor_Warehouse_Lenders, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanMgmt_Investor_Warehouse_Lenders] = (object) this.warehouseLenderServicesDlg.DataView;
            }
            this.warehouseLenderServicesDlg = new WarehouseLendersServicePage(this.session, AclFeature.LoanMgmt_Investor_Warehouse_Lenders, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
          else
            this.warehouseLenderServicesDlg.IsReadOnly = this.readOnly;
        }
        else if (this.warehouseLenderServicesDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Warehouse_Lenders))
          {
            this.warehouseLenderServicesDlg = new WarehouseLendersServicePage(this.session, AclFeature.LoanMgmt_Investor_Warehouse_Lenders, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Investor_Warehouse_Lenders] = (object) this.warehouseLenderServicesDlg.DataView;
          }
          this.warehouseLenderServicesDlg = new WarehouseLendersServicePage(this.session, AclFeature.LoanMgmt_Investor_Warehouse_Lenders, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.warehouseLenderServicesDlg.IsReadOnly = this.readOnly;
        if (this.cachedData[(object) AclFeature.LoanMgmt_Investor_Warehouse_Lenders] != null)
          this.warehouseLenderServicesDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Investor_Warehouse_Lenders];
        if (DialogResult.OK == this.warehouseLenderServicesDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_Investor_Warehouse_Lenders] = (object) this.warehouseLenderServicesDlg.DataView;
          if (this.warehouseLenderServicesDlg.HasBeenModified)
            this.setDirtyFlag(true);
        }
        if (this.warehouseLenderServicesDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.warehouseLenderServicesDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.bIsUserSetup)
          return;
        node.ImageIndex = this.warehouseLenderServicesDlg.GetImageIndex();
        node.SelectedImageIndex = this.warehouseLenderServicesDlg.GetImageIndex();
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Investor_Due_Diligence)
      {
        if (!this.bIsUserSetup)
        {
          if (this.dueDiligenceServiceDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Due_Diligence))
            {
              this.dueDiligenceServiceDlg = new DueDiligenceServicePage(this.session, AclFeature.LoanMgmt_Investor_Due_Diligence, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanMgmt_Investor_Due_Diligence] = (object) this.dueDiligenceServiceDlg.DataView;
            }
            this.dueDiligenceServiceDlg = new DueDiligenceServicePage(this.session, AclFeature.LoanMgmt_Investor_Due_Diligence, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
          else
            this.dueDiligenceServiceDlg.IsReadOnly = this.readOnly;
        }
        else if (this.dueDiligenceServiceDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Due_Diligence))
          {
            this.dueDiligenceServiceDlg = new DueDiligenceServicePage(this.session, AclFeature.LoanMgmt_Investor_Due_Diligence, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Investor_Due_Diligence] = (object) this.dueDiligenceServiceDlg.DataView;
          }
          this.dueDiligenceServiceDlg = new DueDiligenceServicePage(this.session, AclFeature.LoanMgmt_Investor_Due_Diligence, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.dueDiligenceServiceDlg.IsReadOnly = this.readOnly;
        if (this.cachedData[(object) AclFeature.LoanMgmt_Investor_Due_Diligence] != null)
          this.dueDiligenceServiceDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Investor_Due_Diligence];
        if (DialogResult.OK == this.dueDiligenceServiceDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_Investor_Due_Diligence] = (object) this.dueDiligenceServiceDlg.DataView;
          if (this.dueDiligenceServiceDlg.HasBeenModified)
            this.setDirtyFlag(true);
        }
        if (this.dueDiligenceServiceDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.dueDiligenceServiceDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.bIsUserSetup)
          return;
        node.ImageIndex = this.dueDiligenceServiceDlg.GetImageIndex();
        node.SelectedImageIndex = this.dueDiligenceServiceDlg.GetImageIndex();
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Investor_Hedge_Advisory)
      {
        if (!this.bIsUserSetup)
        {
          if (this.hedgeAdvisoryServiceDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Hedge_Advisory))
            {
              this.hedgeAdvisoryServiceDlg = new HedgeAdvisoryServicePage(this.session, AclFeature.LoanMgmt_Investor_Hedge_Advisory, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanMgmt_Investor_Hedge_Advisory] = (object) this.hedgeAdvisoryServiceDlg.DataView;
            }
            this.hedgeAdvisoryServiceDlg = new HedgeAdvisoryServicePage(this.session, AclFeature.LoanMgmt_Investor_Hedge_Advisory, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
          else
            this.hedgeAdvisoryServiceDlg.IsReadOnly = this.readOnly;
        }
        else if (this.hedgeAdvisoryServiceDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Hedge_Advisory))
          {
            this.hedgeAdvisoryServiceDlg = new HedgeAdvisoryServicePage(this.session, AclFeature.LoanMgmt_Investor_Hedge_Advisory, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Investor_Due_Diligence] = (object) this.hedgeAdvisoryServiceDlg.DataView;
          }
          this.hedgeAdvisoryServiceDlg = new HedgeAdvisoryServicePage(this.session, AclFeature.LoanMgmt_Investor_Hedge_Advisory, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.hedgeAdvisoryServiceDlg.IsReadOnly = this.readOnly;
        if (this.cachedData[(object) AclFeature.LoanMgmt_Investor_Hedge_Advisory] != null)
          this.hedgeAdvisoryServiceDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Investor_Hedge_Advisory];
        if (DialogResult.OK == this.hedgeAdvisoryServiceDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_Investor_Hedge_Advisory] = (object) this.hedgeAdvisoryServiceDlg.DataView;
          if (this.hedgeAdvisoryServiceDlg.HasBeenModified)
            this.setDirtyFlag(true);
        }
        if (this.hedgeAdvisoryServiceDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.hedgeAdvisoryServiceDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.bIsUserSetup)
          return;
        node.ImageIndex = this.hedgeAdvisoryServiceDlg.GetImageIndex();
        node.SelectedImageIndex = this.hedgeAdvisoryServiceDlg.GetImageIndex();
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Investor_Subservicing_Services)
      {
        if (!this.bIsUserSetup)
        {
          if (this.subservicingServicesDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Subservicing_Services))
            {
              this.subservicingServicesDlg = new SubservicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Subservicing_Services, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanMgmt_Investor_Subservicing_Services] = (object) this.subservicingServicesDlg.DataView;
            }
            this.subservicingServicesDlg = new SubservicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Subservicing_Services, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
          else
            this.subservicingServicesDlg.IsReadOnly = this.readOnly;
        }
        else if (this.subservicingServicesDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Subservicing_Services))
          {
            this.subservicingServicesDlg = new SubservicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Subservicing_Services, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Investor_Subservicing_Services] = (object) this.subservicingServicesDlg.DataView;
          }
          this.subservicingServicesDlg = new SubservicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Subservicing_Services, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.subservicingServicesDlg.IsReadOnly = this.readOnly;
        if (this.cachedData[(object) AclFeature.LoanMgmt_Investor_Subservicing_Services] != null)
          this.subservicingServicesDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Investor_Subservicing_Services];
        if (DialogResult.OK == this.subservicingServicesDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_Investor_Subservicing_Services] = (object) this.subservicingServicesDlg.DataView;
          if (this.subservicingServicesDlg.HasBeenModified)
            this.setDirtyFlag(true);
        }
        if (this.subservicingServicesDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.subservicingServicesDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.bIsUserSetup)
          return;
        node.ImageIndex = this.subservicingServicesDlg.GetImageIndex();
        node.SelectedImageIndex = this.subservicingServicesDlg.GetImageIndex();
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Investor_Bid_Tape_Services)
      {
        if (!this.bIsUserSetup)
        {
          if (this.bidTapeServicesDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Bid_Tape_Services))
            {
              this.bidTapeServicesDlg = new BidTapeServicePage(this.session, AclFeature.LoanMgmt_Investor_Bid_Tape_Services, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanMgmt_Investor_Bid_Tape_Services] = (object) this.bidTapeServicesDlg.DataView;
            }
            this.bidTapeServicesDlg = new BidTapeServicePage(this.session, AclFeature.LoanMgmt_Investor_Bid_Tape_Services, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
          else
            this.bidTapeServicesDlg.IsReadOnly = this.readOnly;
        }
        else if (this.bidTapeServicesDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Bid_Tape_Services))
          {
            this.bidTapeServicesDlg = new BidTapeServicePage(this.session, AclFeature.LoanMgmt_Investor_Bid_Tape_Services, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Investor_Bid_Tape_Services] = (object) this.bidTapeServicesDlg.DataView;
          }
          this.bidTapeServicesDlg = new BidTapeServicePage(this.session, AclFeature.LoanMgmt_Investor_Bid_Tape_Services, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.bidTapeServicesDlg.IsReadOnly = this.readOnly;
        if (this.cachedData[(object) AclFeature.LoanMgmt_Investor_Bid_Tape_Services] != null)
          this.bidTapeServicesDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Investor_Bid_Tape_Services];
        if (DialogResult.OK == this.bidTapeServicesDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_Investor_Bid_Tape_Services] = (object) this.bidTapeServicesDlg.DataView;
          if (this.bidTapeServicesDlg.HasBeenModified)
            this.setDirtyFlag(true);
        }
        if (this.bidTapeServicesDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.bidTapeServicesDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.bIsUserSetup)
          return;
        node.ImageIndex = this.bidTapeServicesDlg.GetImageIndex();
        node.SelectedImageIndex = this.bidTapeServicesDlg.GetImageIndex();
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Investor_QC_Audit_Services)
      {
        if (!this.bIsUserSetup)
        {
          if (this.qcAuditServicesDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_QC_Audit_Services))
            {
              this.qcAuditServicesDlg = new QCAuditServicesPage(this.session, AclFeature.LoanMgmt_Investor_QC_Audit_Services, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanMgmt_Investor_QC_Audit_Services] = (object) this.qcAuditServicesDlg.DataView;
            }
            this.qcAuditServicesDlg = new QCAuditServicesPage(this.session, AclFeature.LoanMgmt_Investor_QC_Audit_Services, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
          else
            this.qcAuditServicesDlg.IsReadOnly = this.readOnly;
        }
        else if (this.qcAuditServicesDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_QC_Audit_Services))
          {
            this.qcAuditServicesDlg = new QCAuditServicesPage(this.session, AclFeature.LoanMgmt_Investor_QC_Audit_Services, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Investor_QC_Audit_Services] = (object) this.qcAuditServicesDlg.DataView;
          }
          this.qcAuditServicesDlg = new QCAuditServicesPage(this.session, AclFeature.LoanMgmt_Investor_QC_Audit_Services, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.qcAuditServicesDlg.IsReadOnly = this.readOnly;
        if (this.cachedData[(object) AclFeature.LoanMgmt_Investor_QC_Audit_Services] != null)
          this.qcAuditServicesDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Investor_QC_Audit_Services];
        if (DialogResult.OK == this.qcAuditServicesDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_Investor_QC_Audit_Services] = (object) this.qcAuditServicesDlg.DataView;
          if (this.qcAuditServicesDlg.HasBeenModified)
            this.setDirtyFlag(true);
        }
        if (this.qcAuditServicesDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.qcAuditServicesDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.bIsUserSetup)
          return;
        node.ImageIndex = this.qcAuditServicesDlg.GetImageIndex();
        node.SelectedImageIndex = this.qcAuditServicesDlg.GetImageIndex();
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services)
      {
        if (!this.bIsUserSetup)
        {
          if (this.wholesaleLenderServicesDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services))
            {
              this.wholesaleLenderServicesDlg = new WholesaleLenderServicePage(this.session, AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services] = (object) this.wholesaleLenderServicesDlg.DataView;
            }
            this.wholesaleLenderServicesDlg = new WholesaleLenderServicePage(this.session, AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
          else
            this.wholesaleLenderServicesDlg.IsReadOnly = this.readOnly;
        }
        else if (this.wholesaleLenderServicesDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services))
          {
            this.wholesaleLenderServicesDlg = new WholesaleLenderServicePage(this.session, AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services] = (object) this.wholesaleLenderServicesDlg.DataView;
          }
          this.wholesaleLenderServicesDlg = new WholesaleLenderServicePage(this.session, AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.wholesaleLenderServicesDlg.IsReadOnly = this.readOnly;
        if (this.cachedData[(object) AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services] != null)
          this.wholesaleLenderServicesDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services];
        if (DialogResult.OK == this.wholesaleLenderServicesDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services] = (object) this.wholesaleLenderServicesDlg.DataView;
          if (this.wholesaleLenderServicesDlg.HasBeenModified)
            this.setDirtyFlag(true);
        }
        if (this.wholesaleLenderServicesDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.wholesaleLenderServicesDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.bIsUserSetup)
          return;
        node.ImageIndex = this.wholesaleLenderServicesDlg.GetImageIndex();
        node.SelectedImageIndex = this.wholesaleLenderServicesDlg.GetImageIndex();
      }
      else
      {
        if (node.Tag == null || (AclFeature) node.Tag != AclFeature.LoanMgmt_Investor_Servicing_Services)
          return;
        if (!this.bIsUserSetup)
        {
          if (this.servicingServicesDlg == null || this.selectOption < 2)
          {
            if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Servicing_Services))
            {
              this.servicingServicesDlg = new ServicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Servicing_Services, this.personaID, this.readOnly, 2);
              this.cachedData[(object) AclFeature.LoanMgmt_Investor_Servicing_Services] = (object) this.servicingServicesDlg.DataView;
            }
            this.servicingServicesDlg = new ServicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Servicing_Services, this.personaID, this.readOnly, this.selectOption);
            this.selectOption = 2;
          }
          else
            this.servicingServicesDlg.IsReadOnly = this.readOnly;
        }
        else if (this.servicingServicesDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanMgmt_Investor_Servicing_Services))
          {
            this.servicingServicesDlg = new ServicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Servicing_Services, this.userID, this.personaList, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanMgmt_Investor_Servicing_Services] = (object) this.servicingServicesDlg.DataView;
          }
          this.servicingServicesDlg = new ServicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Servicing_Services, this.userID, this.personaList, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.servicingServicesDlg.IsReadOnly = this.readOnly;
        if (this.cachedData[(object) AclFeature.LoanMgmt_Investor_Servicing_Services] != null)
          this.servicingServicesDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanMgmt_Investor_Servicing_Services];
        if (DialogResult.OK == this.servicingServicesDlg.ShowDialog((IWin32Window) this))
        {
          this.cachedData[(object) AclFeature.LoanMgmt_Investor_Servicing_Services] = (object) this.servicingServicesDlg.DataView;
          if (this.servicingServicesDlg.HasBeenModified)
            this.setDirtyFlag(true);
        }
        if (this.servicingServicesDlg.HasSomethingChecked() && !node.Checked)
        {
          this.bInit = true;
          node.Checked = true;
          this.bInit = false;
        }
        else if (!this.servicingServicesDlg.HasSomethingChecked() && node.Checked)
        {
          this.bInit = true;
          node.Checked = false;
          this.bInit = false;
        }
        if (!this.bIsUserSetup)
          return;
        node.ImageIndex = this.servicingServicesDlg.GetImageIndex();
        node.SelectedImageIndex = this.servicingServicesDlg.GetImageIndex();
      }
    }

    private void NodeChecked(TreeNode node)
    {
      if (this.bInit)
        return;
      if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Move)
      {
        this.selectOption = !node.Checked ? 0 : 1;
        this.NodeMouseUp(node);
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Import)
      {
        this.selectOption = !node.Checked ? 0 : 1;
        this.NodeMouseUp(node);
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_Duplicate)
      {
        this.selectOption = !node.Checked ? 0 : 1;
        this.NodeMouseUp(node);
      }
      else if (node.Tag != null && (AclFeature) node.Tag == AclFeature.LoanMgmt_MgmtPipelineServices)
      {
        this.selectOption = !node.Checked ? 0 : 1;
        this.NodeMouseUp(node);
      }
      else
      {
        if (node.Tag == null || (AclFeature) node.Tag != AclFeature.LoanMgmt_Investor_Service)
          return;
        this.selectOption = !node.Checked ? 0 : 1;
        this.NodeMouseUp(node);
      }
    }

    private void InitialSpecialDepNodes()
    {
      if (this.isPersonal)
      {
        this.mlfDlg = new MoveLoanFolderDlg(this.session, this.userID, this.personaList, this.readOnly, this.selectOption);
        this.impDlg = new ImportLoanDlg(this.session, this.userID, this.personaList, this.readOnly, this.selectOption);
        this.dupDlg = new LoanDuplicationTemplatePage(this.session, this.userID, this.personaList, this.readOnly, this.selectOption);
        this.exportServicesDlg = new ExportServices(this.session, AclFeature.LoanMgmt_MgmtPipelineServices, this.userID, this.personaList, this.readOnly, this.selectOption);
        this.investorServicesDlg = new SelectInvestorsPage(this.session, AclFeature.LoanMgmt_Investor_Service, this.userID, this.personaList, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(WarehouseLendersServicePage.InvestorCategory))
          this.warehouseLenderServicesDlg = new WarehouseLendersServicePage(this.session, AclFeature.LoanMgmt_Investor_Warehouse_Lenders, this.userID, this.personaList, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(DueDiligenceServicePage.InvestorCategory))
          this.dueDiligenceServiceDlg = new DueDiligenceServicePage(this.session, AclFeature.LoanMgmt_Investor_Due_Diligence, this.userID, this.personaList, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(HedgeAdvisoryServicePage.InvestorCategory))
          this.hedgeAdvisoryServiceDlg = new HedgeAdvisoryServicePage(this.session, AclFeature.LoanMgmt_Investor_Hedge_Advisory, this.userID, this.personaList, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(SubservicingServicePage.InvestorCategory))
          this.subservicingServicesDlg = new SubservicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Subservicing_Services, this.userID, this.personaList, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(BidTapeServicePage.InvestorCategory))
          this.bidTapeServicesDlg = new BidTapeServicePage(this.session, AclFeature.LoanMgmt_Investor_Bid_Tape_Services, this.userID, this.personaList, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(QCAuditServicesPage.InvestorCategory))
          this.qcAuditServicesDlg = new QCAuditServicesPage(this.session, AclFeature.LoanMgmt_Investor_QC_Audit_Services, this.userID, this.personaList, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(WholesaleLenderServicePage.InvestorCategory))
          this.wholesaleLenderServicesDlg = new WholesaleLenderServicePage(this.session, AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, this.userID, this.personaList, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(ServicingServicePage.InvestorCategory))
          this.servicingServicesDlg = new ServicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Servicing_Services, this.userID, this.personaList, this.readOnly, this.selectOption);
      }
      else
      {
        this.mlfDlg = new MoveLoanFolderDlg(this.session, this.personaID, this.readOnly, this.selectOption);
        this.impDlg = new ImportLoanDlg(this.session, this.personaID, this.readOnly, this.selectOption);
        this.dupDlg = new LoanDuplicationTemplatePage(this.session, this.personaID, this.readOnly, this.selectOption);
        this.exportServicesDlg = new ExportServices(this.session, AclFeature.LoanMgmt_MgmtPipelineServices, this.personaID, this.readOnly, this.selectOption);
        this.investorServicesDlg = new SelectInvestorsPage(this.session, AclFeature.LoanMgmt_Investor_Service, this.personaID, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(WarehouseLendersServicePage.InvestorCategory))
          this.warehouseLenderServicesDlg = new WarehouseLendersServicePage(this.session, AclFeature.LoanMgmt_Investor_Warehouse_Lenders, this.personaID, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(DueDiligenceServicePage.InvestorCategory))
          this.dueDiligenceServiceDlg = new DueDiligenceServicePage(this.session, AclFeature.LoanMgmt_Investor_Due_Diligence, this.personaID, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(HedgeAdvisoryServicePage.InvestorCategory))
          this.hedgeAdvisoryServiceDlg = new HedgeAdvisoryServicePage(this.session, AclFeature.LoanMgmt_Investor_Hedge_Advisory, this.personaID, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(SubservicingServicePage.InvestorCategory))
          this.subservicingServicesDlg = new SubservicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Subservicing_Services, this.personaID, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(BidTapeServicePage.InvestorCategory))
          this.bidTapeServicesDlg = new BidTapeServicePage(this.session, AclFeature.LoanMgmt_Investor_Bid_Tape_Services, this.personaID, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(QCAuditServicesPage.InvestorCategory))
          this.qcAuditServicesDlg = new QCAuditServicesPage(this.session, AclFeature.LoanMgmt_Investor_QC_Audit_Services, this.personaID, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(WholesaleLenderServicePage.InvestorCategory))
          this.wholesaleLenderServicesDlg = new WholesaleLenderServicePage(this.session, AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, this.personaID, this.readOnly, this.selectOption);
        if (new DataDocsServiceHelper(this.session).HasServiceAcccess(ServicingServicePage.InvestorCategory))
          this.servicingServicesDlg = new ServicingServicePage(this.session, AclFeature.LoanMgmt_Investor_Servicing_Services, this.personaID, this.readOnly, this.selectOption);
      }
      Hashtable specialDepTreeNodes1 = new Hashtable();
      Hashtable specialDepTreeNodes2 = new Hashtable();
      if (this.mlfDlg.HasSomethingChecked())
        specialDepTreeNodes1.Add((object) "Move Loans", (object) true);
      else
        specialDepTreeNodes1.Add((object) "Move Loans", (object) false);
      specialDepTreeNodes2.Add((object) "Move Loans", (object) this.mlfDlg.GetImageIndex());
      if (this.impDlg.HasSomethingChecked())
        specialDepTreeNodes1.Add((object) "Import Loans", (object) true);
      else
        specialDepTreeNodes1.Add((object) "Import Loans", (object) false);
      specialDepTreeNodes2.Add((object) "Import Loans", (object) this.impDlg.GetImageIndex());
      if (this.dupDlg.HasSomethingChecked())
        specialDepTreeNodes1.Add((object) "Duplicate Loans", (object) true);
      else
        specialDepTreeNodes1.Add((object) "Duplicate Loans", (object) false);
      specialDepTreeNodes2.Add((object) "Duplicate Loans", (object) this.dupDlg.GetImageIndex());
      if (this.exportServicesDlg.HasSomethingChecked())
        specialDepTreeNodes1.Add((object) "Manage Pipeline Services", (object) true);
      else
        specialDepTreeNodes1.Add((object) "Manage Pipeline Services", (object) false);
      specialDepTreeNodes2.Add((object) "Manage Pipeline Services", (object) this.exportServicesDlg.GetImageIndex());
      if (this.investorServicesDlg.HasSomethingChecked())
        specialDepTreeNodes1.Add((object) "Investor Services", (object) true);
      else
        specialDepTreeNodes1.Add((object) "Investor Services", (object) false);
      specialDepTreeNodes2.Add((object) "Investor Services", (object) this.investorServicesDlg.GetImageIndex());
      if (this.warehouseLenderServicesDlg != null)
      {
        if (this.warehouseLenderServicesDlg.HasSomethingChecked())
          specialDepTreeNodes1.Add((object) "Warehouse Lender Services", (object) true);
        else
          specialDepTreeNodes1.Add((object) "Warehouse Lender Services", (object) false);
        specialDepTreeNodes2.Add((object) "Warehouse Lender Services", (object) this.warehouseLenderServicesDlg.GetImageIndex());
      }
      if (this.wholesaleLenderServicesDlg != null)
      {
        if (this.wholesaleLenderServicesDlg.HasSomethingChecked())
          specialDepTreeNodes1.Add((object) "Wholesale Lender Services", (object) true);
        else
          specialDepTreeNodes1.Add((object) "Wholesale Lender Services", (object) false);
        specialDepTreeNodes2.Add((object) "Wholesale Lender Services", (object) this.wholesaleLenderServicesDlg.GetImageIndex());
      }
      if (this.dueDiligenceServiceDlg != null)
      {
        if (this.dueDiligenceServiceDlg.HasSomethingChecked())
          specialDepTreeNodes1.Add((object) "Due Diligence Services", (object) true);
        else
          specialDepTreeNodes1.Add((object) "Due Diligence Services", (object) false);
        specialDepTreeNodes2.Add((object) "Due Diligence Services", (object) this.dueDiligenceServiceDlg.GetImageIndex());
      }
      if (this.qcAuditServicesDlg != null)
      {
        if (this.qcAuditServicesDlg.HasSomethingChecked())
          specialDepTreeNodes1.Add((object) "QC Audit Services", (object) true);
        else
          specialDepTreeNodes1.Add((object) "QC Audit Services", (object) false);
        specialDepTreeNodes2.Add((object) "QC Audit Services", (object) this.qcAuditServicesDlg.GetImageIndex());
      }
      if (this.servicingServicesDlg != null)
      {
        if (this.servicingServicesDlg.HasSomethingChecked())
          specialDepTreeNodes1.Add((object) "Servicing Services", (object) true);
        else
          specialDepTreeNodes1.Add((object) "Servicing Services", (object) false);
        specialDepTreeNodes2.Add((object) "Servicing Services", (object) this.servicingServicesDlg.GetImageIndex());
      }
      if (this.subservicingServicesDlg != null)
      {
        if (this.subservicingServicesDlg.HasSomethingChecked())
          specialDepTreeNodes1.Add((object) "Subservicing Services", (object) true);
        else
          specialDepTreeNodes1.Add((object) "Subservicing Services", (object) false);
        specialDepTreeNodes2.Add((object) "Subservicing Services", (object) this.subservicingServicesDlg.GetImageIndex());
      }
      if (this.bidTapeServicesDlg != null)
      {
        if (this.bidTapeServicesDlg.HasSomethingChecked())
          specialDepTreeNodes1.Add((object) "Bid Tape Services", (object) true);
        else
          specialDepTreeNodes1.Add((object) "Bid Tape Services", (object) false);
        specialDepTreeNodes2.Add((object) "Bid Tape Services", (object) this.bidTapeServicesDlg.GetImageIndex());
      }
      if (this.hedgeAdvisoryServiceDlg != null)
      {
        if (this.hedgeAdvisoryServiceDlg.HasSomethingChecked())
          specialDepTreeNodes1.Add((object) "Hedge Advisory Services", (object) true);
        else
          specialDepTreeNodes1.Add((object) "Hedge Advisory Services", (object) false);
        specialDepTreeNodes2.Add((object) "Hedge Advisory Services", (object) this.hedgeAdvisoryServiceDlg.GetImageIndex());
      }
      this.securityHelper.setSpecialDepTreeNodes(specialDepTreeNodes1);
      this.securityHelper.setSpecialDepTreeNodesImg(specialDepTreeNodes2);
    }

    private bool makeCheck(TreeNode node, string nodeName)
    {
      bool flag = false;
      if (node.Text == nodeName)
      {
        node.Checked = true;
        return true;
      }
      foreach (TreeNode node1 in node.Nodes)
      {
        if (this.makeCheck(node1, nodeName))
        {
          node.Checked = true;
          flag = true;
          break;
        }
      }
      return flag;
    }

    public int PersonaID
    {
      get => this.personaID;
      set
      {
        if (this.personaID != value)
        {
          this.personaID = value;
          this.SetPersona(value);
          this.Reset();
        }
        else
          this.personaID = value;
      }
    }

    public void Save()
    {
      if (!this.NeedToSaveData())
        return;
      if (this.mlfDlg != null)
        this.mlfDlg.SaveData();
      if (this.impDlg != null)
        this.impDlg.SaveData();
      if (this.dupDlg != null)
        this.dupDlg.SaveData();
      if (this.exportServicesDlg != null)
        this.exportServicesDlg.SaveData();
      if (this.investorServicesDlg != null && this.investorServicesDlg.HasBeenModified)
        this.investorServicesDlg.SaveData();
      if (this.warehouseLenderServicesDlg != null && this.warehouseLenderServicesDlg.HasBeenModified)
        this.warehouseLenderServicesDlg.SaveData();
      if (this.dueDiligenceServiceDlg != null && this.dueDiligenceServiceDlg.HasBeenModified)
        this.dueDiligenceServiceDlg.SaveData();
      if (this.subservicingServicesDlg != null && this.subservicingServicesDlg.HasBeenModified)
        this.subservicingServicesDlg.SaveData();
      if (this.bidTapeServicesDlg != null && this.bidTapeServicesDlg.HasBeenModified)
        this.bidTapeServicesDlg.SaveData();
      if (this.hedgeAdvisoryServiceDlg != null && this.hedgeAdvisoryServiceDlg.HasBeenModified)
        this.hedgeAdvisoryServiceDlg.SaveData();
      if (this.qcAuditServicesDlg != null && this.qcAuditServicesDlg.HasBeenModified)
        this.qcAuditServicesDlg.SaveData();
      if (this.wholesaleLenderServicesDlg != null && this.wholesaleLenderServicesDlg.HasBeenModified)
        this.wholesaleLenderServicesDlg.SaveData();
      if (this.servicingServicesDlg != null && this.servicingServicesDlg.HasBeenModified)
        this.servicingServicesDlg.SaveData();
      this.UpdatePermissions();
      this.dupDlg = new LoanDuplicationTemplatePage(this.session, this.personaID, this.readOnly, this.selectOption);
      if (!this.dupDlg.HasSomethingChecked())
        this.securityHelper.SetPermission(AclFeature.LoanMgmt_Duplicate, AclTriState.False);
      if (this.warehouseLenderServicesDlg != null && !this.warehouseLenderServicesDlg.HasSomethingChecked())
        this.securityHelper.SetPermission(AclFeature.LoanMgmt_Investor_Warehouse_Lenders, AclTriState.False);
      if (this.dueDiligenceServiceDlg != null && !this.dueDiligenceServiceDlg.HasSomethingChecked())
        this.securityHelper.SetPermission(AclFeature.LoanMgmt_Investor_Due_Diligence, AclTriState.False);
      if (this.subservicingServicesDlg != null && !this.subservicingServicesDlg.HasSomethingChecked())
        this.securityHelper.SetPermission(AclFeature.LoanMgmt_Investor_Subservicing_Services, AclTriState.False);
      if (this.bidTapeServicesDlg != null && !this.bidTapeServicesDlg.HasSomethingChecked())
        this.securityHelper.SetPermission(AclFeature.LoanMgmt_Investor_Bid_Tape_Services, AclTriState.False);
      if (this.hedgeAdvisoryServiceDlg != null && !this.hedgeAdvisoryServiceDlg.HasSomethingChecked())
        this.securityHelper.SetPermission(AclFeature.LoanMgmt_Investor_Hedge_Advisory, AclTriState.False);
      if (this.qcAuditServicesDlg != null && !this.qcAuditServicesDlg.HasSomethingChecked())
        this.securityHelper.SetPermission(AclFeature.LoanMgmt_Investor_QC_Audit_Services, AclTriState.False);
      if (this.wholesaleLenderServicesDlg != null && !this.wholesaleLenderServicesDlg.HasSomethingChecked())
        this.securityHelper.SetPermission(AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services, AclTriState.False);
      if (this.servicingServicesDlg != null && !this.servicingServicesDlg.HasSomethingChecked())
        this.securityHelper.SetPermission(AclFeature.LoanMgmt_Investor_Servicing_Services, AclTriState.False);
      this.dupDlg = (LoanDuplicationTemplatePage) null;
    }

    public void Reset()
    {
      this.InitialSpecialDepNodes();
      this.ResetTree();
      this.mlfDlg = (MoveLoanFolderDlg) null;
      this.impDlg = (ImportLoanDlg) null;
      this.dupDlg = (LoanDuplicationTemplatePage) null;
      this.exportServicesDlg = (ExportServices) null;
      this.investorServicesDlg = (SelectInvestorsPage) null;
      this.warehouseLenderServicesDlg = (WarehouseLendersServicePage) null;
      this.dueDiligenceServiceDlg = (DueDiligenceServicePage) null;
      this.hedgeAdvisoryServiceDlg = (HedgeAdvisoryServicePage) null;
      this.subservicingServicesDlg = (SubservicingServicePage) null;
      this.bidTapeServicesDlg = (BidTapeServicePage) null;
      this.qcAuditServicesDlg = (QCAuditServicesPage) null;
      this.wholesaleLenderServicesDlg = (WholesaleLenderServicePage) null;
      this.servicingServicesDlg = (ServicingServicePage) null;
      this.cachedData = new Hashtable();
    }

    public bool NeedToSaveData()
    {
      return this.mlfDlg != null && this.mlfDlg.HasBeenModified() || this.impDlg != null && this.impDlg.HasBeenModified() || this.dupDlg != null && this.dupDlg.HasBeenModified() || this.exportServicesDlg != null && this.exportServicesDlg.HasBeenModified || this.investorServicesDlg != null && this.investorServicesDlg.HasBeenModified || this.warehouseLenderServicesDlg != null && this.warehouseLenderServicesDlg.HasBeenModified || this.dueDiligenceServiceDlg != null && this.dueDiligenceServiceDlg.HasBeenModified || this.hedgeAdvisoryServiceDlg != null && this.hedgeAdvisoryServiceDlg.HasBeenModified || this.subservicingServicesDlg != null && this.subservicingServicesDlg.HasBeenModified || this.bidTapeServicesDlg != null && this.bidTapeServicesDlg.HasBeenModified || this.qcAuditServicesDlg != null && this.qcAuditServicesDlg.HasBeenModified || this.wholesaleLenderServicesDlg != null && this.wholesaleLenderServicesDlg.HasBeenModified || this.servicingServicesDlg != null && this.servicingServicesDlg.HasBeenModified || this.hasBeenModified();
    }

    public void HideControl(bool hide)
    {
      this.treeViewTabs.Visible = !hide;
      this.setNoAccessLabel(hide, AclFeature.GlobalTab_Pipeline);
    }
  }
}
