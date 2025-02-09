// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ATRQMDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ATRQMDialog : Form
  {
    private DocumentLog doc;
    private FeaturesAclManager aclMgr;
    private bool canAddVerification;
    private bool canEditHowVerified;
    private IContainer components;
    private ToolTip tooltip;
    private Panel pnlClose;
    private TabControl tabVerification;
    private TabPage pageEmployment;
    private TabPage pageObligation;
    private Label lblChooseVerificationType;
    private Label lblWarning;
    private CheckBox chkIncome;
    private CheckBox chkObligation;
    private CheckBox chkEmployment;
    private Button btnSave;
    private Button btnCancel;
    private CheckBox chkAsset;
    private TabPage pageIncome;
    private TabPage pageAsset;
    private Panel pnlHistory;
    private Panel pnlMonthly;
    private GroupContainer gcCreditHistory;
    private GridView gvCreditHistory;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnAddCreditHistory;
    private Panel pnlNonEmploymentIncome;
    private GroupContainer gcNonEmploymentIncome;
    private GridView gvNonEmploymentIncome;
    private FlowLayoutPanel flowLayoutPanel3;
    private StandardIconButton btnAddNonEmploymentIncome;
    private Panel pnlEmploymentIncome;
    private GroupContainer gcEmploymentIncome;
    private GridView gvEmploymentIncome;
    private FlowLayoutPanel flowLayoutPanel2;
    private StandardIconButton btnAddEmploymentIncome;
    private GroupContainer gcMonthlyObligations;
    private GridView gvMonthlyObligations;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnAddMonthlyObligation;
    private GroupContainer gcAssets;
    private GridView gvAssets;
    private FlowLayoutPanel flowLayoutPanel4;
    private StandardIconButton btnAddAsset;
    private GroupContainer gcEmployment;
    private FlowLayoutPanel flowLayoutPanel5;
    private StandardIconButton btnAddEmployment;
    private GridView gvEmploymentStatus;

    public ATRQMDialog(DocumentLog doc, VerificationTimelineType verificationType)
    {
      this.InitializeComponent();
      this.doc = doc;
      this.Text = "ATR/QM (" + this.doc.Title + ")";
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.canAddVerification = this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_VerificationNew);
      this.canEditHowVerified = this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_VerificationEdit);
      this.loadVerificationTypeFields();
      if (verificationType == VerificationTimelineType.Employment)
        this.tabVerification.SelectedTab = this.pageEmployment;
      if (verificationType == VerificationTimelineType.Obligation)
        this.tabVerification.SelectedTab = this.pageObligation;
      if (verificationType == VerificationTimelineType.Income)
        this.tabVerification.SelectedTab = this.pageIncome;
      if (verificationType == VerificationTimelineType.Asset)
        this.tabVerification.SelectedTab = this.pageAsset;
      this.loadEmploymentVerifications();
      this.loadObligationsVerifications();
      this.loadIncomeVerifications();
      this.loadAssetVerifications();
    }

    private void loadVerificationTypeFields()
    {
      this.chkEmployment.Checked = this.doc.IsEmploymentVerification;
      this.chkObligation.Checked = this.doc.IsObligationVerification;
      this.chkIncome.Checked = this.doc.IsIncomeVerification;
      this.chkAsset.Checked = this.doc.IsAssetVerification;
      if (this.canAddVerification)
      {
        this.chkEmployment.Enabled = !this.chkEmployment.Checked;
        this.chkObligation.Enabled = !this.chkObligation.Checked;
        this.chkIncome.Enabled = !this.chkIncome.Checked;
        this.chkAsset.Enabled = !this.chkAsset.Checked;
      }
      this.showHideTabs();
    }

    private void saveVerifications()
    {
      if (this.chkEmployment.Checked)
      {
        if (!this.doc.IsEmploymentVerification)
          this.doc.IsEmploymentVerification = true;
        this.saveEmploymentVerifications();
      }
      if (this.chkObligation.Checked)
      {
        if (!this.doc.IsObligationVerification)
          this.doc.IsObligationVerification = true;
        this.saveObligationVerifications();
      }
      if (this.chkIncome.Checked)
      {
        if (!this.doc.IsIncomeVerification)
          this.doc.IsIncomeVerification = true;
        this.saveIncomeVerifications();
      }
      if (!this.chkAsset.Checked)
        return;
      if (!this.doc.IsAssetVerification)
        this.doc.IsAssetVerification = true;
      this.saveAssetVerifications();
    }

    private void showHideTabs()
    {
      TabPage selectedTab = this.tabVerification.SelectedTab;
      this.tabVerification.TabPages.Remove(this.pageEmployment);
      this.tabVerification.TabPages.Remove(this.pageObligation);
      this.tabVerification.TabPages.Remove(this.pageIncome);
      this.tabVerification.TabPages.Remove(this.pageAsset);
      if (this.chkEmployment.Checked)
      {
        this.tabVerification.TabPages.Add(this.pageEmployment);
        this.btnAddEmployment.Enabled = this.canAddVerification;
        if (selectedTab == this.pageEmployment)
          this.tabVerification.SelectedTab = this.pageEmployment;
      }
      if (this.chkObligation.Checked)
      {
        this.tabVerification.TabPages.Add(this.pageObligation);
        this.btnAddMonthlyObligation.Enabled = this.canAddVerification;
        this.btnAddCreditHistory.Enabled = this.canAddVerification;
        if (selectedTab == this.pageObligation)
          this.tabVerification.SelectedTab = this.pageObligation;
      }
      if (this.chkIncome.Checked)
      {
        this.tabVerification.TabPages.Add(this.pageIncome);
        this.btnAddEmploymentIncome.Enabled = this.canAddVerification;
        this.btnAddNonEmploymentIncome.Enabled = this.canAddVerification;
        if (selectedTab == this.pageIncome)
          this.tabVerification.SelectedTab = this.pageIncome;
      }
      if (!this.chkAsset.Checked)
        return;
      this.tabVerification.TabPages.Add(this.pageAsset);
      this.btnAddAsset.Enabled = this.canAddVerification;
      if (selectedTab != this.pageAsset)
        return;
      this.tabVerification.SelectedTab = this.pageAsset;
    }

    private void chkEmployment_CheckedChanged(object sender, EventArgs e)
    {
      this.showHideTabs();
      if (!this.chkEmployment.Checked)
        return;
      this.tabVerification.SelectedTab = this.pageEmployment;
    }

    private void chkObligation_CheckedChanged(object sender, EventArgs e)
    {
      this.showHideTabs();
      if (!this.chkObligation.Checked)
        return;
      this.tabVerification.SelectedTab = this.pageObligation;
    }

    private void chkIncome_CheckedChanged(object sender, EventArgs e)
    {
      this.showHideTabs();
      if (!this.chkIncome.Checked)
        return;
      this.tabVerification.SelectedTab = this.pageIncome;
    }

    private void chkAsset_CheckedChanged(object sender, EventArgs e)
    {
      this.showHideTabs();
      if (!this.chkAsset.Checked)
        return;
      this.tabVerification.SelectedTab = this.pageAsset;
    }

    private void loadEmploymentVerifications()
    {
      this.gvEmploymentStatus.Items.Clear();
      foreach (DocumentVerificationEmployment verification in this.doc.Verifications.Get(VerificationTimelineType.Employment))
        this.addEmploymentVerificationToList(verification);
    }

    private void addEmploymentVerificationToList(DocumentVerificationEmployment verification)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) verification;
      gvItem.SubItems[0].Text = EnumUtil.GetEnumDescription((Enum) verification.EmploymentType);
      gvItem.SubItems[1].Text = string.Empty;
      gvItem.SubItems[1].Tag = (object) verification.BorrowerType;
      if (verification.BorrowerType == LoanBorrowerType.Borrower)
        gvItem.SubItems[1].Text = "B";
      if (verification.BorrowerType == LoanBorrowerType.Coborrower)
        gvItem.SubItems[1].Text = "C";
      if (verification.BorrowerType == LoanBorrowerType.Both)
        gvItem.SubItems[1].Text = "J";
      TextBox textBox = new TextBox();
      textBox.Text = verification.HowVerified;
      textBox.Enabled = this.canEditHowVerified;
      gvItem.SubItems[2].Value = (object) textBox;
      this.gvEmploymentStatus.Items.Add(gvItem);
    }

    private void btnAddEmployment_Click(object sender, EventArgs e)
    {
      using (AddVerificationEmploymentDialog employmentDialog = new AddVerificationEmploymentDialog())
      {
        if (employmentDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (DocumentVerificationEmployment verification in employmentDialog.Verifications)
          this.addEmploymentVerificationToList(verification);
      }
    }

    private void saveEmploymentVerifications()
    {
      if (!this.chkEmployment.Checked || !this.doc.IsEmploymentVerification)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvEmploymentStatus.Items)
      {
        DocumentVerificationEmployment tag = (DocumentVerificationEmployment) gvItem.Tag;
        DocumentVerificationType verification = this.doc.Verifications[tag.Guid];
        TextBox textBox = (TextBox) gvItem.SubItems[2].Value;
        if (verification == null)
        {
          tag.HowVerified = textBox.Text;
          this.doc.Verifications.Add((DocumentVerificationType) tag);
        }
        else
        {
          verification.BorrowerType = (LoanBorrowerType) gvItem.SubItems[1].Tag;
          verification.HowVerified = textBox.Text;
        }
      }
    }

    private void loadObligationsVerifications()
    {
      this.gvCreditHistory.Items.Clear();
      this.gvMonthlyObligations.Items.Clear();
      foreach (DocumentVerificationObligation obligation in this.doc.Verifications.Get(VerificationTimelineType.Obligation))
        this.addObligationVerificationToList(obligation);
    }

    private void addObligationVerificationToList(DocumentVerificationObligation obligation)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) obligation;
      gvItem.SubItems[0].Text = EnumUtil.GetEnumDescription((Enum) obligation.ObligationType);
      if (obligation.ObligationType == ObligationType.MortgageLate)
        gvItem.SubItems[0].Text = "Mortgage Lates: How Many: " + obligation.MortageLateCount + " delinquencies";
      Decimal amount;
      if (obligation.ObligationType == ObligationType.SecondLien)
      {
        GVSubItem subItem = gvItem.SubItems[0];
        amount = obligation.Amount;
        string str = "2nd Lien: How Much: $" + amount.ToString() + ".00/month";
        subItem.Text = str;
      }
      if (obligation.ObligationType == ObligationType.HELOC)
      {
        GVSubItem subItem = gvItem.SubItems[0];
        amount = obligation.Amount;
        string str = "HELOC: How Much: $" + amount.ToString() + ".00/month";
        subItem.Text = str;
      }
      if (obligation.ObligationType == ObligationType.OtherMonthlyObligation || obligation.ObligationType == ObligationType.OtherCreditHistory)
        gvItem.SubItems[0].Text = "Other: " + obligation.OtherDescription;
      gvItem.SubItems[1].Text = string.Empty;
      gvItem.SubItems[1].Tag = (object) obligation.BorrowerType;
      if (obligation.BorrowerType == LoanBorrowerType.Borrower)
        gvItem.SubItems[1].Text = "B";
      if (obligation.BorrowerType == LoanBorrowerType.Coborrower)
        gvItem.SubItems[1].Text = "C";
      if (obligation.BorrowerType == LoanBorrowerType.Both)
        gvItem.SubItems[1].Text = "J";
      TextBox textBox = new TextBox();
      textBox.Text = obligation.HowVerified;
      textBox.Enabled = this.canEditHowVerified;
      gvItem.SubItems[2].Value = (object) textBox;
      if (this.isCreditHistoryObligationType(obligation.ObligationType))
        this.gvCreditHistory.Items.Add(gvItem);
      else
        this.gvMonthlyObligations.Items.Add(gvItem);
    }

    private bool isCreditHistoryObligationType(ObligationType type)
    {
      return type != ObligationType.None && type != ObligationType.InstallmentLoan && type != ObligationType.RealEstateLoan && type != ObligationType.AlimonyOrMaintenance && type != ObligationType.MonthlyHousingExpense && type != ObligationType.RevolvingChargeAccount && type != ObligationType.SimultaneousLoansOnProperty && type != ObligationType.ChildSupport && type != ObligationType.RequiredEscrow && type != ObligationType.OtherMonthlyObligation;
    }

    private void btnAddMonthlyObligation_Click(object sender, EventArgs e)
    {
      using (AddVerificationMonthlyObligationDialog obligationDialog = new AddVerificationMonthlyObligationDialog())
      {
        if (obligationDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (DocumentVerificationObligation verification in obligationDialog.Verifications)
          this.addObligationVerificationToList(verification);
      }
    }

    private void btnAddCreditHistory_Click(object sender, EventArgs e)
    {
      using (AddVerificationCreditHistoryDialog creditHistoryDialog = new AddVerificationCreditHistoryDialog())
      {
        if (creditHistoryDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (DocumentVerificationObligation verification in creditHistoryDialog.Verifications)
          this.addObligationVerificationToList(verification);
      }
    }

    private void saveObligationVerifications()
    {
      if (!this.chkObligation.Checked || !this.doc.IsObligationVerification)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMonthlyObligations.Items)
        this.saveObligationVerification(gvItem);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvCreditHistory.Items)
        this.saveObligationVerification(gvItem);
    }

    private void saveObligationVerification(GVItem item)
    {
      DocumentVerificationObligation tag = (DocumentVerificationObligation) item.Tag;
      DocumentVerificationObligation verification = (DocumentVerificationObligation) this.doc.Verifications[tag.Guid];
      TextBox textBox = (TextBox) item.SubItems[2].Value;
      if (verification == null)
      {
        tag.HowVerified = textBox.Text;
        this.doc.Verifications.Add((DocumentVerificationType) tag);
      }
      else
      {
        verification.BorrowerType = (LoanBorrowerType) item.SubItems[1].Tag;
        verification.HowVerified = textBox.Text;
        if (tag.ObligationType == ObligationType.MortgageLate)
          verification.MortageLateCount = tag.MortageLateCount;
        if (tag.ObligationType == ObligationType.SecondLien || tag.ObligationType == ObligationType.HELOC)
          verification.Amount = tag.Amount;
        if (tag.ObligationType != ObligationType.OtherMonthlyObligation && tag.ObligationType != ObligationType.OtherCreditHistory)
          return;
        verification.OtherDescription = tag.OtherDescription;
      }
    }

    private void loadIncomeVerifications()
    {
      this.gvEmploymentIncome.Items.Clear();
      this.gvNonEmploymentIncome.Items.Clear();
      foreach (DocumentVerificationIncome verification in this.doc.Verifications.Get(VerificationTimelineType.Income))
        this.addIncomeVerificationToList(verification);
    }

    private void addIncomeVerificationToList(DocumentVerificationIncome verification)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) verification;
      gvItem.SubItems[0].Text = EnumUtil.GetEnumDescription((Enum) verification.IncomeType);
      if (verification.IncomeType == IncomeType.TaxReturn)
        gvItem.SubItems[0].Text = gvItem.SubItems[0].Text + ": " + verification.TaxReturnYear.ToString();
      if (verification.IncomeType == IncomeType.OtherEmployment || verification.IncomeType == IncomeType.OtherNonEmployment)
        gvItem.SubItems[0].Text = gvItem.SubItems[0].Text + ": " + verification.OtherDescription;
      gvItem.SubItems[1].Text = string.Empty;
      gvItem.SubItems[1].Tag = (object) verification.BorrowerType;
      if (verification.BorrowerType == LoanBorrowerType.Borrower)
        gvItem.SubItems[1].Text = "B";
      if (verification.BorrowerType == LoanBorrowerType.Coborrower)
        gvItem.SubItems[1].Text = "C";
      if (verification.BorrowerType == LoanBorrowerType.Both)
        gvItem.SubItems[1].Text = "J";
      TextBox textBox = new TextBox();
      textBox.Text = verification.HowVerified;
      textBox.Enabled = this.canEditHowVerified;
      gvItem.SubItems[2].Value = (object) textBox;
      if (this.isEmploymentIncome(verification.IncomeType))
        this.gvEmploymentIncome.Items.Add(gvItem);
      else
        this.gvNonEmploymentIncome.Items.Add(gvItem);
    }

    private bool isEmploymentIncome(IncomeType type)
    {
      return type != IncomeType.None && type != IncomeType.AlimonyOrMaintenance && type != IncomeType.ChildSupport && type != IncomeType.RentalIncome && type != IncomeType.OtherNonEmployment;
    }

    private void btnAddEmploymentIncome_Click(object sender, EventArgs e)
    {
      using (AddVerificationEmploymentIncomeDialog employmentIncomeDialog = new AddVerificationEmploymentIncomeDialog())
      {
        if (employmentIncomeDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (DocumentVerificationIncome verification in employmentIncomeDialog.Verifications)
          this.addIncomeVerificationToList(verification);
      }
    }

    private void btnAddNonEmploymentIncome_Click(object sender, EventArgs e)
    {
      using (AddVerificationNonEmploymentIncomeDialog employmentIncomeDialog = new AddVerificationNonEmploymentIncomeDialog())
      {
        if (employmentIncomeDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (DocumentVerificationIncome verification in employmentIncomeDialog.Verifications)
          this.addIncomeVerificationToList(verification);
      }
    }

    private void saveIncomeVerifications()
    {
      if (!this.chkIncome.Checked || !this.doc.IsIncomeVerification)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvEmploymentIncome.Items)
        this.saveIncomeVerification(gvItem);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvNonEmploymentIncome.Items)
        this.saveIncomeVerification(gvItem);
    }

    private void saveIncomeVerification(GVItem item)
    {
      DocumentVerificationIncome tag = (DocumentVerificationIncome) item.Tag;
      DocumentVerificationIncome verification = (DocumentVerificationIncome) this.doc.Verifications[tag.Guid];
      TextBox textBox = (TextBox) item.SubItems[2].Value;
      if (verification == null)
      {
        tag.HowVerified = textBox.Text;
        this.doc.Verifications.Add((DocumentVerificationType) tag);
      }
      else
      {
        verification.BorrowerType = (LoanBorrowerType) item.SubItems[1].Tag;
        verification.HowVerified = textBox.Text;
        if (tag.IncomeType == IncomeType.TaxReturn)
          verification.TaxReturnYear = tag.TaxReturnYear;
        if (tag.IncomeType != IncomeType.OtherEmployment && tag.IncomeType != IncomeType.OtherNonEmployment)
          return;
        verification.OtherDescription = tag.OtherDescription;
      }
    }

    private void loadAssetVerifications()
    {
      this.gvAssets.Items.Clear();
      foreach (DocumentVerificationAsset verification in this.doc.Verifications.Get(VerificationTimelineType.Asset))
        this.addAssetVerificationToList(verification);
    }

    private void addAssetVerificationToList(DocumentVerificationAsset verification)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) verification;
      gvItem.SubItems[0].Text = EnumUtil.GetEnumDescription((Enum) verification.AssetType);
      if (verification.AssetType == AssetType.Other)
        gvItem.SubItems[0].Text = gvItem.SubItems[0].Text + ": " + verification.OtherDescription;
      gvItem.SubItems[1].Text = string.Empty;
      gvItem.SubItems[1].Tag = (object) verification.BorrowerType;
      if (verification.BorrowerType == LoanBorrowerType.Borrower)
        gvItem.SubItems[1].Text = "B";
      if (verification.BorrowerType == LoanBorrowerType.Coborrower)
        gvItem.SubItems[1].Text = "C";
      if (verification.BorrowerType == LoanBorrowerType.Both)
        gvItem.SubItems[1].Text = "J";
      TextBox textBox = new TextBox();
      textBox.Text = verification.HowVerified;
      textBox.Enabled = this.canEditHowVerified;
      gvItem.SubItems[2].Value = (object) textBox;
      this.gvAssets.Items.Add(gvItem);
    }

    private void btnAddAsset_Click(object sender, EventArgs e)
    {
      using (AddVerificationAssetDialog verificationAssetDialog = new AddVerificationAssetDialog())
      {
        if (verificationAssetDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (DocumentVerificationAsset verification in verificationAssetDialog.Verifications)
          this.addAssetVerificationToList(verification);
      }
    }

    private void saveAssetVerifications()
    {
      if (!this.chkAsset.Checked || !this.doc.IsAssetVerification)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAssets.Items)
      {
        DocumentVerificationAsset tag = (DocumentVerificationAsset) gvItem.Tag;
        DocumentVerificationAsset verification = (DocumentVerificationAsset) this.doc.Verifications[tag.Guid];
        TextBox textBox = (TextBox) gvItem.SubItems[2].Value;
        if (verification == null)
        {
          tag.HowVerified = textBox.Text;
          this.doc.Verifications.Add((DocumentVerificationType) tag);
        }
        else
        {
          verification.BorrowerType = (LoanBorrowerType) gvItem.SubItems[1].Tag;
          verification.HowVerified = textBox.Text;
          if (tag.AssetType == AssetType.Other)
            verification.OtherDescription = tag.OtherDescription;
        }
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.saveVerifications();
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      GVColumn gvColumn15 = new GVColumn();
      GVColumn gvColumn16 = new GVColumn();
      GVColumn gvColumn17 = new GVColumn();
      GVColumn gvColumn18 = new GVColumn();
      this.tooltip = new ToolTip(this.components);
      this.btnAddCreditHistory = new StandardIconButton();
      this.btnAddEmploymentIncome = new StandardIconButton();
      this.btnAddNonEmploymentIncome = new StandardIconButton();
      this.btnAddMonthlyObligation = new StandardIconButton();
      this.btnAddAsset = new StandardIconButton();
      this.btnAddEmployment = new StandardIconButton();
      this.pnlClose = new Panel();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.tabVerification = new TabControl();
      this.pageEmployment = new TabPage();
      this.gcEmployment = new GroupContainer();
      this.gvEmploymentStatus = new GridView();
      this.flowLayoutPanel5 = new FlowLayoutPanel();
      this.pageObligation = new TabPage();
      this.pnlHistory = new Panel();
      this.gcCreditHistory = new GroupContainer();
      this.gvCreditHistory = new GridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.pnlMonthly = new Panel();
      this.gcMonthlyObligations = new GroupContainer();
      this.gvMonthlyObligations = new GridView();
      this.pnlToolbar = new FlowLayoutPanel();
      this.pageIncome = new TabPage();
      this.pnlNonEmploymentIncome = new Panel();
      this.gcNonEmploymentIncome = new GroupContainer();
      this.gvNonEmploymentIncome = new GridView();
      this.flowLayoutPanel3 = new FlowLayoutPanel();
      this.pnlEmploymentIncome = new Panel();
      this.gcEmploymentIncome = new GroupContainer();
      this.gvEmploymentIncome = new GridView();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.pageAsset = new TabPage();
      this.gcAssets = new GroupContainer();
      this.gvAssets = new GridView();
      this.flowLayoutPanel4 = new FlowLayoutPanel();
      this.lblChooseVerificationType = new Label();
      this.lblWarning = new Label();
      this.chkIncome = new CheckBox();
      this.chkObligation = new CheckBox();
      this.chkEmployment = new CheckBox();
      this.chkAsset = new CheckBox();
      ((ISupportInitialize) this.btnAddCreditHistory).BeginInit();
      ((ISupportInitialize) this.btnAddEmploymentIncome).BeginInit();
      ((ISupportInitialize) this.btnAddNonEmploymentIncome).BeginInit();
      ((ISupportInitialize) this.btnAddMonthlyObligation).BeginInit();
      ((ISupportInitialize) this.btnAddAsset).BeginInit();
      ((ISupportInitialize) this.btnAddEmployment).BeginInit();
      this.pnlClose.SuspendLayout();
      this.tabVerification.SuspendLayout();
      this.pageEmployment.SuspendLayout();
      this.gcEmployment.SuspendLayout();
      this.flowLayoutPanel5.SuspendLayout();
      this.pageObligation.SuspendLayout();
      this.pnlHistory.SuspendLayout();
      this.gcCreditHistory.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.pnlMonthly.SuspendLayout();
      this.gcMonthlyObligations.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      this.pageIncome.SuspendLayout();
      this.pnlNonEmploymentIncome.SuspendLayout();
      this.gcNonEmploymentIncome.SuspendLayout();
      this.flowLayoutPanel3.SuspendLayout();
      this.pnlEmploymentIncome.SuspendLayout();
      this.gcEmploymentIncome.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.pageAsset.SuspendLayout();
      this.gcAssets.SuspendLayout();
      this.flowLayoutPanel4.SuspendLayout();
      this.SuspendLayout();
      this.btnAddCreditHistory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddCreditHistory.BackColor = Color.Transparent;
      this.btnAddCreditHistory.Location = new Point(84, 3);
      this.btnAddCreditHistory.Margin = new Padding(4, 3, 0, 3);
      this.btnAddCreditHistory.MouseDownImage = (Image) null;
      this.btnAddCreditHistory.Name = "btnAddCreditHistory";
      this.btnAddCreditHistory.Size = new Size(16, 16);
      this.btnAddCreditHistory.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddCreditHistory.TabIndex = 12;
      this.btnAddCreditHistory.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddCreditHistory, "New Credit History");
      this.btnAddCreditHistory.Click += new EventHandler(this.btnAddCreditHistory_Click);
      this.btnAddEmploymentIncome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddEmploymentIncome.BackColor = Color.Transparent;
      this.btnAddEmploymentIncome.Location = new Point(84, 3);
      this.btnAddEmploymentIncome.Margin = new Padding(4, 3, 0, 3);
      this.btnAddEmploymentIncome.MouseDownImage = (Image) null;
      this.btnAddEmploymentIncome.Name = "btnAddEmploymentIncome";
      this.btnAddEmploymentIncome.Size = new Size(16, 16);
      this.btnAddEmploymentIncome.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddEmploymentIncome.TabIndex = 12;
      this.btnAddEmploymentIncome.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddEmploymentIncome, "New Employment Income");
      this.btnAddEmploymentIncome.Click += new EventHandler(this.btnAddEmploymentIncome_Click);
      this.btnAddNonEmploymentIncome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddNonEmploymentIncome.BackColor = Color.Transparent;
      this.btnAddNonEmploymentIncome.Location = new Point(84, 3);
      this.btnAddNonEmploymentIncome.Margin = new Padding(4, 3, 0, 3);
      this.btnAddNonEmploymentIncome.MouseDownImage = (Image) null;
      this.btnAddNonEmploymentIncome.Name = "btnAddNonEmploymentIncome";
      this.btnAddNonEmploymentIncome.Size = new Size(16, 16);
      this.btnAddNonEmploymentIncome.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddNonEmploymentIncome.TabIndex = 12;
      this.btnAddNonEmploymentIncome.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddNonEmploymentIncome, "New Non-Employment Income");
      this.btnAddNonEmploymentIncome.Click += new EventHandler(this.btnAddNonEmploymentIncome_Click);
      this.btnAddMonthlyObligation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddMonthlyObligation.BackColor = Color.Transparent;
      this.btnAddMonthlyObligation.Location = new Point(84, 3);
      this.btnAddMonthlyObligation.Margin = new Padding(4, 3, 0, 3);
      this.btnAddMonthlyObligation.MouseDownImage = (Image) null;
      this.btnAddMonthlyObligation.Name = "btnAddMonthlyObligation";
      this.btnAddMonthlyObligation.Size = new Size(16, 16);
      this.btnAddMonthlyObligation.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddMonthlyObligation.TabIndex = 12;
      this.btnAddMonthlyObligation.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddMonthlyObligation, "New Monthly Obligation");
      this.btnAddMonthlyObligation.Click += new EventHandler(this.btnAddMonthlyObligation_Click);
      this.btnAddAsset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddAsset.BackColor = Color.Transparent;
      this.btnAddAsset.Location = new Point(84, 3);
      this.btnAddAsset.Margin = new Padding(4, 3, 0, 3);
      this.btnAddAsset.MouseDownImage = (Image) null;
      this.btnAddAsset.Name = "btnAddAsset";
      this.btnAddAsset.Size = new Size(16, 16);
      this.btnAddAsset.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddAsset.TabIndex = 12;
      this.btnAddAsset.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddAsset, "New Asset");
      this.btnAddAsset.Click += new EventHandler(this.btnAddAsset_Click);
      this.btnAddEmployment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddEmployment.BackColor = Color.Transparent;
      this.btnAddEmployment.Location = new Point(84, 3);
      this.btnAddEmployment.Margin = new Padding(4, 3, 0, 3);
      this.btnAddEmployment.MouseDownImage = (Image) null;
      this.btnAddEmployment.Name = "btnAddEmployment";
      this.btnAddEmployment.Size = new Size(16, 16);
      this.btnAddEmployment.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddEmployment.TabIndex = 12;
      this.btnAddEmployment.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddEmployment, "New Employment");
      this.btnAddEmployment.Click += new EventHandler(this.btnAddEmployment_Click);
      this.pnlClose.Controls.Add((Control) this.btnSave);
      this.pnlClose.Controls.Add((Control) this.btnCancel);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 544);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(415, 40);
      this.pnlClose.TabIndex = 4;
      this.btnSave.Location = new Point((int) byte.MaxValue, 10);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 5;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(331, 10);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.tabVerification.Controls.Add((Control) this.pageEmployment);
      this.tabVerification.Controls.Add((Control) this.pageObligation);
      this.tabVerification.Controls.Add((Control) this.pageIncome);
      this.tabVerification.Controls.Add((Control) this.pageAsset);
      this.tabVerification.Location = new Point(0, 155);
      this.tabVerification.Name = "tabVerification";
      this.tabVerification.SelectedIndex = 0;
      this.tabVerification.Size = new Size(416, 393);
      this.tabVerification.TabIndex = 5;
      this.pageEmployment.AutoScroll = true;
      this.pageEmployment.AutoScrollMargin = new Size(8, 8);
      this.pageEmployment.BackColor = Color.WhiteSmoke;
      this.pageEmployment.Controls.Add((Control) this.gcEmployment);
      this.pageEmployment.Location = new Point(4, 23);
      this.pageEmployment.Name = "pageEmployment";
      this.pageEmployment.Padding = new Padding(0, 2, 2, 2);
      this.pageEmployment.Size = new Size(408, 366);
      this.pageEmployment.TabIndex = 0;
      this.pageEmployment.Text = "Employment Status Verification";
      this.pageEmployment.UseVisualStyleBackColor = true;
      this.gcEmployment.Controls.Add((Control) this.gvEmploymentStatus);
      this.gcEmployment.Controls.Add((Control) this.flowLayoutPanel5);
      this.gcEmployment.Dock = DockStyle.Fill;
      this.gcEmployment.HeaderForeColor = SystemColors.ControlText;
      this.gcEmployment.Location = new Point(0, 2);
      this.gcEmployment.Name = "gcEmployment";
      this.gcEmployment.Size = new Size(406, 362);
      this.gcEmployment.TabIndex = 36;
      this.gcEmployment.Text = "Employment";
      this.gvEmploymentStatus.BorderStyle = BorderStyle.None;
      this.gvEmploymentStatus.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colEmploymentStatus";
      gvColumn1.Text = "Employment Status";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colBorrower";
      gvColumn2.Text = "For Borrower";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colHowVerified";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "How was it verified?";
      gvColumn3.Width = 154;
      this.gvEmploymentStatus.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvEmploymentStatus.Dock = DockStyle.Fill;
      this.gvEmploymentStatus.Location = new Point(1, 26);
      this.gvEmploymentStatus.Name = "gvEmploymentStatus";
      this.gvEmploymentStatus.Size = new Size(404, 335);
      this.gvEmploymentStatus.TabIndex = 17;
      this.gvEmploymentStatus.TextTrimming = StringTrimming.EllipsisCharacter;
      this.flowLayoutPanel5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel5.BackColor = Color.Transparent;
      this.flowLayoutPanel5.Controls.Add((Control) this.btnAddEmployment);
      this.flowLayoutPanel5.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel5.Location = new Point(302, 2);
      this.flowLayoutPanel5.Name = "flowLayoutPanel5";
      this.flowLayoutPanel5.Size = new Size(100, 22);
      this.flowLayoutPanel5.TabIndex = 7;
      this.pageObligation.BackColor = Color.WhiteSmoke;
      this.pageObligation.Controls.Add((Control) this.pnlHistory);
      this.pageObligation.Controls.Add((Control) this.pnlMonthly);
      this.pageObligation.Location = new Point(4, 23);
      this.pageObligation.Name = "pageObligation";
      this.pageObligation.Padding = new Padding(0, 2, 2, 2);
      this.pageObligation.Size = new Size(408, 366);
      this.pageObligation.TabIndex = 1;
      this.pageObligation.Text = "Monthly Obligation Verification";
      this.pageObligation.UseVisualStyleBackColor = true;
      this.pnlHistory.BackColor = Color.White;
      this.pnlHistory.Controls.Add((Control) this.gcCreditHistory);
      this.pnlHistory.Dock = DockStyle.Fill;
      this.pnlHistory.Location = new Point(0, 183);
      this.pnlHistory.Name = "pnlHistory";
      this.pnlHistory.Size = new Size(406, 181);
      this.pnlHistory.TabIndex = 16;
      this.gcCreditHistory.Controls.Add((Control) this.gvCreditHistory);
      this.gcCreditHistory.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcCreditHistory.Dock = DockStyle.Fill;
      this.gcCreditHistory.HeaderForeColor = SystemColors.ControlText;
      this.gcCreditHistory.Location = new Point(0, 0);
      this.gcCreditHistory.Name = "gcCreditHistory";
      this.gcCreditHistory.Size = new Size(406, 181);
      this.gcCreditHistory.TabIndex = 36;
      this.gcCreditHistory.Text = "Credit History";
      this.gvCreditHistory.AllowColumnReorder = true;
      this.gvCreditHistory.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colCreditHistory";
      gvColumn4.Text = "Credit History";
      gvColumn4.Width = 150;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colBorrower";
      gvColumn5.Text = "For Borrower";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colHowVerified";
      gvColumn6.SpringToFit = true;
      gvColumn6.Text = "How was it verified?";
      gvColumn6.Width = 154;
      this.gvCreditHistory.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvCreditHistory.Dock = DockStyle.Fill;
      this.gvCreditHistory.Location = new Point(1, 26);
      this.gvCreditHistory.Name = "gvCreditHistory";
      this.gvCreditHistory.Size = new Size(404, 154);
      this.gvCreditHistory.TabIndex = 16;
      this.gvCreditHistory.TextTrimming = StringTrimming.EllipsisCharacter;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddCreditHistory);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(302, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(100, 22);
      this.flowLayoutPanel1.TabIndex = 7;
      this.pnlMonthly.BackColor = Color.White;
      this.pnlMonthly.Controls.Add((Control) this.gcMonthlyObligations);
      this.pnlMonthly.Dock = DockStyle.Top;
      this.pnlMonthly.Location = new Point(0, 2);
      this.pnlMonthly.Name = "pnlMonthly";
      this.pnlMonthly.Size = new Size(406, 181);
      this.pnlMonthly.TabIndex = 15;
      this.gcMonthlyObligations.Controls.Add((Control) this.gvMonthlyObligations);
      this.gcMonthlyObligations.Controls.Add((Control) this.pnlToolbar);
      this.gcMonthlyObligations.Dock = DockStyle.Fill;
      this.gcMonthlyObligations.HeaderForeColor = SystemColors.ControlText;
      this.gcMonthlyObligations.Location = new Point(0, 0);
      this.gcMonthlyObligations.Name = "gcMonthlyObligations";
      this.gcMonthlyObligations.Size = new Size(406, 181);
      this.gcMonthlyObligations.TabIndex = 35;
      this.gcMonthlyObligations.Text = "Monthly Obligations";
      this.gvMonthlyObligations.AllowColumnReorder = true;
      this.gvMonthlyObligations.BorderStyle = BorderStyle.None;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "colObligations";
      gvColumn7.Text = "Monthly Obligations";
      gvColumn7.Width = 150;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "colBorrower";
      gvColumn8.Text = "For Borrower";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "colHowVerified";
      gvColumn9.SpringToFit = true;
      gvColumn9.Text = "How was it verified?";
      gvColumn9.Width = 154;
      this.gvMonthlyObligations.Columns.AddRange(new GVColumn[3]
      {
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gvMonthlyObligations.Dock = DockStyle.Fill;
      this.gvMonthlyObligations.Location = new Point(1, 26);
      this.gvMonthlyObligations.Name = "gvMonthlyObligations";
      this.gvMonthlyObligations.Size = new Size(404, 154);
      this.gvMonthlyObligations.TabIndex = 16;
      this.gvMonthlyObligations.TextTrimming = StringTrimming.EllipsisCharacter;
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnAddMonthlyObligation);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(302, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(100, 22);
      this.pnlToolbar.TabIndex = 7;
      this.pageIncome.Controls.Add((Control) this.pnlNonEmploymentIncome);
      this.pageIncome.Controls.Add((Control) this.pnlEmploymentIncome);
      this.pageIncome.Location = new Point(4, 23);
      this.pageIncome.Name = "pageIncome";
      this.pageIncome.Size = new Size(408, 366);
      this.pageIncome.TabIndex = 2;
      this.pageIncome.Text = "Income Verification";
      this.pageIncome.UseVisualStyleBackColor = true;
      this.pnlNonEmploymentIncome.BackColor = Color.White;
      this.pnlNonEmploymentIncome.Controls.Add((Control) this.gcNonEmploymentIncome);
      this.pnlNonEmploymentIncome.Dock = DockStyle.Fill;
      this.pnlNonEmploymentIncome.Location = new Point(0, 181);
      this.pnlNonEmploymentIncome.Name = "pnlNonEmploymentIncome";
      this.pnlNonEmploymentIncome.Size = new Size(408, 185);
      this.pnlNonEmploymentIncome.TabIndex = 17;
      this.gcNonEmploymentIncome.Controls.Add((Control) this.gvNonEmploymentIncome);
      this.gcNonEmploymentIncome.Controls.Add((Control) this.flowLayoutPanel3);
      this.gcNonEmploymentIncome.Dock = DockStyle.Fill;
      this.gcNonEmploymentIncome.HeaderForeColor = SystemColors.ControlText;
      this.gcNonEmploymentIncome.Location = new Point(0, 0);
      this.gcNonEmploymentIncome.Name = "gcNonEmploymentIncome";
      this.gcNonEmploymentIncome.Size = new Size(408, 185);
      this.gcNonEmploymentIncome.TabIndex = 36;
      this.gcNonEmploymentIncome.Text = "Non-Employment";
      this.gvNonEmploymentIncome.AllowColumnReorder = true;
      this.gvNonEmploymentIncome.BorderStyle = BorderStyle.None;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "colSupportingInfo";
      gvColumn10.Text = "Income Supporting Information";
      gvColumn10.Width = 175;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "colBorrower";
      gvColumn11.Text = "For Borrower";
      gvColumn11.Width = 100;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "colHowVerified";
      gvColumn12.SpringToFit = true;
      gvColumn12.Text = "How was it verified?";
      gvColumn12.Width = 131;
      this.gvNonEmploymentIncome.Columns.AddRange(new GVColumn[3]
      {
        gvColumn10,
        gvColumn11,
        gvColumn12
      });
      this.gvNonEmploymentIncome.Dock = DockStyle.Fill;
      this.gvNonEmploymentIncome.Location = new Point(1, 26);
      this.gvNonEmploymentIncome.Name = "gvNonEmploymentIncome";
      this.gvNonEmploymentIncome.Size = new Size(406, 158);
      this.gvNonEmploymentIncome.TabIndex = 16;
      this.gvNonEmploymentIncome.TextTrimming = StringTrimming.EllipsisCharacter;
      this.flowLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel3.BackColor = Color.Transparent;
      this.flowLayoutPanel3.Controls.Add((Control) this.btnAddNonEmploymentIncome);
      this.flowLayoutPanel3.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel3.Location = new Point(304, 2);
      this.flowLayoutPanel3.Name = "flowLayoutPanel3";
      this.flowLayoutPanel3.Size = new Size(100, 22);
      this.flowLayoutPanel3.TabIndex = 7;
      this.pnlEmploymentIncome.BackColor = Color.White;
      this.pnlEmploymentIncome.Controls.Add((Control) this.gcEmploymentIncome);
      this.pnlEmploymentIncome.Dock = DockStyle.Top;
      this.pnlEmploymentIncome.Location = new Point(0, 0);
      this.pnlEmploymentIncome.Name = "pnlEmploymentIncome";
      this.pnlEmploymentIncome.Size = new Size(408, 181);
      this.pnlEmploymentIncome.TabIndex = 16;
      this.gcEmploymentIncome.Controls.Add((Control) this.gvEmploymentIncome);
      this.gcEmploymentIncome.Controls.Add((Control) this.flowLayoutPanel2);
      this.gcEmploymentIncome.Dock = DockStyle.Fill;
      this.gcEmploymentIncome.HeaderForeColor = SystemColors.ControlText;
      this.gcEmploymentIncome.Location = new Point(0, 0);
      this.gcEmploymentIncome.Name = "gcEmploymentIncome";
      this.gcEmploymentIncome.Size = new Size(408, 181);
      this.gcEmploymentIncome.TabIndex = 35;
      this.gcEmploymentIncome.Text = "Employment";
      this.gvEmploymentIncome.AllowColumnReorder = true;
      this.gvEmploymentIncome.BorderStyle = BorderStyle.None;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "colSupportingInfo";
      gvColumn13.Text = "Income Supporting Information";
      gvColumn13.Width = 175;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "colBorrower";
      gvColumn14.Text = "For Borrower";
      gvColumn14.Width = 100;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "colHowVerified";
      gvColumn15.SpringToFit = true;
      gvColumn15.Text = "How was it verified?";
      gvColumn15.Width = 131;
      this.gvEmploymentIncome.Columns.AddRange(new GVColumn[3]
      {
        gvColumn13,
        gvColumn14,
        gvColumn15
      });
      this.gvEmploymentIncome.Dock = DockStyle.Fill;
      this.gvEmploymentIncome.Location = new Point(1, 26);
      this.gvEmploymentIncome.Name = "gvEmploymentIncome";
      this.gvEmploymentIncome.Size = new Size(406, 154);
      this.gvEmploymentIncome.TabIndex = 16;
      this.gvEmploymentIncome.TextTrimming = StringTrimming.EllipsisCharacter;
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnAddEmploymentIncome);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(304, 2);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(100, 22);
      this.flowLayoutPanel2.TabIndex = 7;
      this.pageAsset.Controls.Add((Control) this.gcAssets);
      this.pageAsset.Location = new Point(4, 23);
      this.pageAsset.Name = "pageAsset";
      this.pageAsset.Size = new Size(408, 366);
      this.pageAsset.TabIndex = 3;
      this.pageAsset.Text = "Asset Verification";
      this.pageAsset.UseVisualStyleBackColor = true;
      this.gcAssets.Controls.Add((Control) this.gvAssets);
      this.gcAssets.Controls.Add((Control) this.flowLayoutPanel4);
      this.gcAssets.Dock = DockStyle.Fill;
      this.gcAssets.HeaderForeColor = SystemColors.ControlText;
      this.gcAssets.Location = new Point(0, 0);
      this.gcAssets.Name = "gcAssets";
      this.gcAssets.Size = new Size(408, 366);
      this.gcAssets.TabIndex = 36;
      this.gcAssets.Text = "Assets";
      this.gvAssets.AllowColumnReorder = true;
      this.gvAssets.BorderStyle = BorderStyle.None;
      gvColumn16.ImageIndex = -1;
      gvColumn16.Name = "colAssets";
      gvColumn16.Text = "Assets";
      gvColumn16.Width = 150;
      gvColumn17.ImageIndex = -1;
      gvColumn17.Name = "colBorrower";
      gvColumn17.Text = "For Borrower";
      gvColumn17.Width = 100;
      gvColumn18.ImageIndex = -1;
      gvColumn18.Name = "colHowVerified";
      gvColumn18.SpringToFit = true;
      gvColumn18.Text = "How was it verified?";
      gvColumn18.Width = 156;
      this.gvAssets.Columns.AddRange(new GVColumn[3]
      {
        gvColumn16,
        gvColumn17,
        gvColumn18
      });
      this.gvAssets.Dock = DockStyle.Fill;
      this.gvAssets.Location = new Point(1, 26);
      this.gvAssets.Name = "gvAssets";
      this.gvAssets.Size = new Size(406, 339);
      this.gvAssets.TabIndex = 16;
      this.gvAssets.TextTrimming = StringTrimming.EllipsisCharacter;
      this.flowLayoutPanel4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel4.BackColor = Color.Transparent;
      this.flowLayoutPanel4.Controls.Add((Control) this.btnAddAsset);
      this.flowLayoutPanel4.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel4.Location = new Point(304, 2);
      this.flowLayoutPanel4.Name = "flowLayoutPanel4";
      this.flowLayoutPanel4.Size = new Size(100, 22);
      this.flowLayoutPanel4.TabIndex = 7;
      this.lblChooseVerificationType.Location = new Point(12, 64);
      this.lblChooseVerificationType.Name = "lblChooseVerificationType";
      this.lblChooseVerificationType.Size = new Size(395, 33);
      this.lblChooseVerificationType.TabIndex = 7;
      this.lblChooseVerificationType.Text = "Choose a verification type to select the applicable ATR/QM requirements. Click Save to save all selections and continue.";
      this.lblWarning.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblWarning.ForeColor = Color.Red;
      this.lblWarning.Location = new Point(12, 9);
      this.lblWarning.Name = "lblWarning";
      this.lblWarning.Size = new Size(400, 44);
      this.lblWarning.TabIndex = 6;
      this.lblWarning.Text = "WARNING: Review your selections carefully. The selections on this window cannot be changed after you click Save, and only a user with Super Administrator persona rights can delete the document.";
      this.chkIncome.AutoSize = true;
      this.chkIncome.Enabled = false;
      this.chkIncome.Location = new Point(240, 100);
      this.chkIncome.Name = "chkIncome";
      this.chkIncome.Size = new Size(117, 18);
      this.chkIncome.TabIndex = 32;
      this.chkIncome.Text = "Income Verification";
      this.chkIncome.CheckedChanged += new EventHandler(this.chkIncome_CheckedChanged);
      this.chkObligation.AutoSize = true;
      this.chkObligation.Enabled = false;
      this.chkObligation.Location = new Point(12, 124);
      this.chkObligation.Name = "chkObligation";
      this.chkObligation.Size = new Size(170, 18);
      this.chkObligation.TabIndex = 30;
      this.chkObligation.Text = "Monthly Obligation Verification";
      this.chkObligation.CheckedChanged += new EventHandler(this.chkObligation_CheckedChanged);
      this.chkEmployment.AutoSize = true;
      this.chkEmployment.Enabled = false;
      this.chkEmployment.Location = new Point(12, 100);
      this.chkEmployment.Name = "chkEmployment";
      this.chkEmployment.Size = new Size(174, 18);
      this.chkEmployment.TabIndex = 31;
      this.chkEmployment.Text = "Employment Status Verification";
      this.chkEmployment.CheckedChanged += new EventHandler(this.chkEmployment_CheckedChanged);
      this.chkAsset.AutoSize = true;
      this.chkAsset.Enabled = false;
      this.chkAsset.Location = new Point(240, 124);
      this.chkAsset.Name = "chkAsset";
      this.chkAsset.Size = new Size(112, 18);
      this.chkAsset.TabIndex = 33;
      this.chkAsset.Text = "Asset Verification";
      this.chkAsset.CheckedChanged += new EventHandler(this.chkAsset_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(415, 584);
      this.Controls.Add((Control) this.chkAsset);
      this.Controls.Add((Control) this.chkIncome);
      this.Controls.Add((Control) this.chkObligation);
      this.Controls.Add((Control) this.chkEmployment);
      this.Controls.Add((Control) this.lblChooseVerificationType);
      this.Controls.Add((Control) this.lblWarning);
      this.Controls.Add((Control) this.tabVerification);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ATRQMDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ATR/QM";
      ((ISupportInitialize) this.btnAddCreditHistory).EndInit();
      ((ISupportInitialize) this.btnAddEmploymentIncome).EndInit();
      ((ISupportInitialize) this.btnAddNonEmploymentIncome).EndInit();
      ((ISupportInitialize) this.btnAddMonthlyObligation).EndInit();
      ((ISupportInitialize) this.btnAddAsset).EndInit();
      ((ISupportInitialize) this.btnAddEmployment).EndInit();
      this.pnlClose.ResumeLayout(false);
      this.tabVerification.ResumeLayout(false);
      this.pageEmployment.ResumeLayout(false);
      this.gcEmployment.ResumeLayout(false);
      this.flowLayoutPanel5.ResumeLayout(false);
      this.pageObligation.ResumeLayout(false);
      this.pnlHistory.ResumeLayout(false);
      this.gcCreditHistory.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.pnlMonthly.ResumeLayout(false);
      this.gcMonthlyObligations.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      this.pageIncome.ResumeLayout(false);
      this.pnlNonEmploymentIncome.ResumeLayout(false);
      this.gcNonEmploymentIncome.ResumeLayout(false);
      this.flowLayoutPanel3.ResumeLayout(false);
      this.pnlEmploymentIncome.ResumeLayout(false);
      this.gcEmploymentIncome.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.pageAsset.ResumeLayout(false);
      this.gcAssets.ResumeLayout(false);
      this.flowLayoutPanel4.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
