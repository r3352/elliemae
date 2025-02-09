// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditCompanyLoanTypeControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditCompanyLoanTypeControl : UserControl
  {
    private Sessions.Session session;
    private SessionObjects obj;
    private int orgID = -1;
    private int parentID = -1;
    private ExternalOrgLoanTypes loanTypes;
    private ExternalOrgLoanTypes parentLoanTypes;
    private ExternalOriginatorManagementData parentContact;
    private IConfigurationManager config;
    private DateTime minValue = DateTime.Parse("1/1/1900");
    private DateTime maxValue = DateTime.Parse("01/01/2079");
    public bool saveOnLoad;
    private string advancedCodeXml;
    private string lastSelectedComboItemText = string.Empty;
    private string lastSelectedComboItemTextCorNonDel = string.Empty;
    private string lastSelectedComboItemTextCorDel = string.Empty;
    private IContainer components;
    private GroupContainer grpBLoanTypes;
    private CheckBox chkBUSDA;
    private CheckBox chkBFirstLien;
    private CheckBox chkBHELOC;
    private CheckBox chkBVA;
    private CheckBox chkBFHA;
    private CheckBox chkBConventional;
    private GroupContainer grpBPurpose;
    private CheckBox chkBConstructionPerm;
    private CheckBox chkBConstruction;
    private CheckBox chkBRefin;
    private CheckBox chkBNoRefin;
    private CheckBox chkBPurchase;
    private GroupContainer grpFHAVA;
    private ComboBox cboFHAStatus;
    private Label label40;
    private DatePicker dpFHAExpDate;
    private DatePicker dpFHAApprovedDate;
    private CheckBox chkUseParentInfo;
    private Label label14;
    private Label label15;
    private Label label16;
    private Label label17;
    private TextBox txtFHACompRatio;
    private TextBox txtFHASponId;
    private Label label1;
    private TextBox txtFHAId;
    private TextBox txtVAId;
    private ComboBox cboVAStatus;
    private Label label3;
    private DatePicker dpVAExpirationDate;
    private DatePicker dpVAApprovedDate;
    private Label label4;
    private Label label5;
    private Label label6;
    private CheckBox chkBSecondLien;
    private GroupContainer grpAll;
    private Panel panelHeader;
    private StandardIconButton btnReset;
    private StandardIconButton btnSave;
    private TabControl tabLoanTypes;
    private TabPage tabBroker;
    private GroupContainer grpBLicensingIssues;
    private TextBox txtBMsgUploadNonApproved;
    private Label label7;
    private Label lblPercent;
    private CheckBox chkBOther;
    private CheckBox chkBPOther;
    private ComboBox cmbBAllowLoansWithLicenseIssues;
    private Label label33;
    private TabPage tabCorres;
    private ComboBox cmbUnderwriting;
    private Label label2;
    private TabControl tabControl1;
    private TabPage tabCorrDel;
    private GroupContainer grpCLicensingIssues;
    private ComboBox cmbCAllowLoansWithLicenseIssues;
    private TextBox txtCMsgUploadNonApproved;
    private Label label8;
    private GroupContainer grpCLoanTypes;
    private CheckBox chkCOther;
    private CheckBox chkCSecondLien;
    private CheckBox chkCUSDA;
    private CheckBox chkCFirstLien;
    private CheckBox chkCHELOC;
    private CheckBox chkCVA;
    private CheckBox chkCFHA;
    private CheckBox chkCConventional;
    private GroupContainer grpCPurpose;
    private CheckBox chkCPOther;
    private CheckBox chkCConstructionPerm;
    private CheckBox chkCConstruction;
    private CheckBox chkCRefin;
    private CheckBox chkCNoRefin;
    private CheckBox chkCPurchase;
    private TabPage tabCorrNonDel;
    private GroupContainer grpNDLicensingIssues;
    private ComboBox cmbNDAllowLoansWithLicenseIssues;
    private TextBox txtNDMsgUploadNonApproved;
    private Label label10;
    private GroupContainer grpNDLoanTypes;
    private CheckBox chkNDOther;
    private CheckBox chkNDSecondLien;
    private CheckBox chkNDUSDA;
    private CheckBox chkNDFirstLien;
    private CheckBox chkNDHELOC;
    private CheckBox chkNDVA;
    private CheckBox chkNDFHA;
    private CheckBox chkNDConventional;
    private GroupContainer grpNDPurpose;
    private CheckBox chkNDPOther;
    private CheckBox chkNDConstructionPerm;
    private CheckBox chkNDConstruction;
    private CheckBox chkNDRefin;
    private CheckBox chkNDNoRefin;
    private CheckBox chkNDPurchase;
    private Panel panel1;
    private StandardIconButton btnSelect;
    private TextBox textConditionCode;
    private Label label9;
    private ComboBox cboFHADirectEndorsement;
    private TextBox txtVASponsorID;
    private Label label11;
    private Label label12;
    private CheckBox chkFNMAApproved;
    private TextBox txtFannieMaeID;
    private Label label13;
    private CheckBox chkFHMLCApproved;
    private Label label18;
    private TextBox txtFreddieMacID;
    private Label label19;
    private ComboBox cboAUSMethod;
    private Label label20;
    private Panel pnlFHAVA;
    private Label lblPadding;

    public event EventHandler SaveButton_Clicked;

    public EditCompanyLoanTypeControl(SessionObjects obj, int orgID, int parentID)
    {
      this.obj = obj;
      this.orgID = orgID;
      this.parentID = parentID;
      this.session = new Sessions.Session(obj.SessionID);
      this.config = this.obj.ConfigurationManager;
      this.parentContact = this.obj.ConfigurationManager.GetExternalOrganization(false, parentID);
      if (this.parentContact != null)
        this.parentLoanTypes = this.config.GetExternalOrganizationLoanTypes(this.parentID);
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.btnReset_Click((object) null, (EventArgs) null);
    }

    public EditCompanyLoanTypeControl(Sessions.Session session, int orgID, int parentID)
    {
      this.session = session;
      this.orgID = orgID;
      this.parentID = parentID;
      this.config = this.session.ConfigurationManager;
      this.parentContact = this.session.ConfigurationManager.GetExternalOrganization(false, parentID);
      if (this.parentContact != null)
        this.parentLoanTypes = this.config.GetExternalOrganizationLoanTypes(this.parentID);
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.btnReset_Click((object) null, (EventArgs) null);
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
      this.IncludeDockPadding();
    }

    private void IncludeDockPadding()
    {
      this.grpBLoanTypes.DockPadding.Right = 5;
      this.grpBLicensingIssues.DockPadding.Right = 5;
      this.grpBPurpose.DockPadding.Left = 5;
      this.grpCLoanTypes.DockPadding.Right = 5;
      this.grpCLicensingIssues.DockPadding.Right = 5;
      this.grpCPurpose.DockPadding.Left = 5;
      this.grpNDLoanTypes.DockPadding.Right = 5;
      this.grpNDLicensingIssues.DockPadding.Right = 5;
      this.grpNDPurpose.DockPadding.Left = 5;
    }

    private void initForm()
    {
      this.loanTypes = this.config.GetExternalOrganizationLoanTypes(this.orgID);
      if (this.loanTypes != null && this.loanTypes.ExternalOrgID != 0)
      {
        this.chkUseParentInfo.Checked = this.loanTypes.UseParentInfoFhaVa;
        this.chkUseParentInfo.Enabled = true;
        this.populateLoanTypes(this.loanTypes);
      }
      else
      {
        this.clearControl((Control) this);
        this.populateLoanTypes((ExternalOrgLoanTypes) null);
        this.txtCMsgUploadNonApproved.Text = this.txtNDMsgUploadNonApproved.Text = this.txtBMsgUploadNonApproved.Text = "Your organization is not approved to submit loans of this type. Please contact your Account Executive for assistance.";
        this.cmbBAllowLoansWithLicenseIssues.SelectedIndex = this.cmbCAllowLoansWithLicenseIssues.SelectedIndex = this.cmbNDAllowLoansWithLicenseIssues.SelectedIndex = 0;
      }
      if (this.parentContact != null && (this.loanTypes == null || this.loanTypes.ExternalOrgID == 0))
      {
        this.chkUseParentInfo.Checked = true;
        this.chkUseParentInfo.Enabled = true;
        if (this.parentLoanTypes.ExternalOrgID != 0)
          this.populateLoanTypes(this.parentLoanTypes);
        this.saveOnLoad = true;
      }
      if (this.parentContact == null)
      {
        this.chkUseParentInfo.Checked = false;
        this.chkUseParentInfo.Enabled = false;
      }
      this.lastSelectedComboItemText = this.cmbBAllowLoansWithLicenseIssues.SelectedItem != null ? this.cmbBAllowLoansWithLicenseIssues.SelectedItem.ToString() : string.Empty;
      this.lastSelectedComboItemTextCorDel = this.cmbCAllowLoansWithLicenseIssues.SelectedItem != null ? this.cmbCAllowLoansWithLicenseIssues.SelectedItem.ToString() : string.Empty;
      this.lastSelectedComboItemTextCorNonDel = this.cmbNDAllowLoansWithLicenseIssues.SelectedItem != null ? this.cmbNDAllowLoansWithLicenseIssues.SelectedItem.ToString() : string.Empty;
    }

    private void clearControl(Control control)
    {
      switch (control)
      {
        case TextBox _:
          control.Text = string.Empty;
          break;
        case CheckBox _:
          CheckBox checkBox = (CheckBox) control;
          checkBox.Checked = checkBox.Text == "Use Parent Info" && this.parentContact != null;
          break;
        case DateTimePicker _:
          ((DateTimePicker) control).Value = DateTime.Now;
          break;
        case ComboBox _:
          ((ListControl) control).SelectedIndex = -1;
          break;
      }
      if (!control.HasChildren)
        return;
      foreach (Control control1 in (ArrangedElementCollection) control.Controls)
        this.clearControl(control1);
    }

    private void populateLoanTypes(ExternalOrgLoanTypes loanTypes)
    {
      if (loanTypes == null || loanTypes.ExternalOrgID == 0)
      {
        this.checkChannelControls((Control) this.tabBroker);
        this.checkChannelControls((Control) this.tabCorrDel);
        this.checkChannelControls((Control) this.tabCorrNonDel);
      }
      else
      {
        if (loanTypes.Broker != null)
        {
          this.populateBrokerTab(loanTypes.Broker);
        }
        else
        {
          this.checkChannelControls((Control) this.tabBroker);
          this.txtBMsgUploadNonApproved.Text = "Your organization is not approved to submit loans of this type. Please contact your Account Executive for assistance.";
        }
        if (loanTypes.CorrespondentDelegated != null)
        {
          this.populateCorrespondentTab(loanTypes.CorrespondentDelegated);
        }
        else
        {
          this.checkChannelControls((Control) this.tabCorrDel);
          this.txtCMsgUploadNonApproved.Text = "Your organization is not approved to submit loans of this type. Please contact your Account Executive for assistance.";
        }
        if (loanTypes.CorrespondentNonDelegated != null)
        {
          this.populateCorrespondentNonDelTab(loanTypes.CorrespondentNonDelegated);
        }
        else
        {
          this.checkChannelControls((Control) this.tabCorrNonDel);
          this.txtNDMsgUploadNonApproved.Text = "Your organization is not approved to submit loans of this type. Please contact your Account Executive for assistance.";
        }
        this.populateFHAVA(loanTypes);
        this.populateUnderwriting(loanTypes);
      }
    }

    private void populateUnderwriting(ExternalOrgLoanTypes loanTypes)
    {
      this.cmbUnderwriting.SelectedIndex = loanTypes.Underwriting;
      if (loanTypes.Underwriting != 2)
        return;
      this.textConditionCode.Text = loanTypes.AdvancedCode;
      this.advancedCodeXml = loanTypes.AdvancedCodeXml;
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (sender != null && Utils.Dialog((IWin32Window) this, "Are you sure you want to reset? All changes will be lost.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      this.initForm();
      this.SetButtonStatus(false);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.SaveButton_Clicked == null)
        return;
      this.SaveButton_Clicked(sender, e);
    }

    public void Reset() => this.initForm();

    private void checkChannelControls(Control control)
    {
      if (control is CheckBox)
      {
        CheckBox checkBox = (CheckBox) control;
        checkBox.Checked = (!(checkBox.Text == "Use Parent Info") || this.parentContact != null) && !(checkBox.Text == "Disable Upload");
      }
      if (!control.HasChildren)
        return;
      foreach (Control control1 in (ArrangedElementCollection) control.Controls)
        this.checkChannelControls(control1);
    }

    private void populateBrokerTab(
      ExternalOrgLoanTypes.ExternalOrgChannelLoanType channelType)
    {
      this.chkBConventional.Checked = (channelType.LoanTypes & 1) == 1;
      this.chkBFHA.Checked = (channelType.LoanTypes & 2) == 2;
      this.chkBVA.Checked = (channelType.LoanTypes & 4) == 4;
      this.chkBUSDA.Checked = (channelType.LoanTypes & 8) == 8;
      this.chkBHELOC.Checked = (channelType.LoanTypes & 16) == 16;
      this.chkBOther.Checked = (channelType.LoanTypes & 32) == 32;
      this.chkBFirstLien.Checked = (channelType.LoanTypes & 64) == 64;
      this.chkBSecondLien.Checked = (channelType.LoanTypes & 128) == 128;
      this.chkBPurchase.Checked = (channelType.LoanPurpose & 1) == 1;
      this.chkBNoRefin.Checked = (channelType.LoanPurpose & 2) == 2;
      this.chkBRefin.Checked = (channelType.LoanPurpose & 4) == 4;
      this.chkBConstruction.Checked = (channelType.LoanPurpose & 8) == 8;
      this.chkBConstructionPerm.Checked = (channelType.LoanPurpose & 16) == 16;
      this.chkBPOther.Checked = (channelType.LoanPurpose & 32) == 32;
      this.cmbBAllowLoansWithLicenseIssues.SelectedIndex = channelType.AllowLoansWithIssues;
      if (channelType.MsgUploadNonApprovedLoans != null && channelType.MsgUploadNonApprovedLoans.Trim() != "")
        this.txtBMsgUploadNonApproved.Text = channelType.MsgUploadNonApprovedLoans;
      else
        this.txtBMsgUploadNonApproved.Text = "Your organization is not approved to submit loans of this type. Please contact your Account Executive for assistance.";
    }

    private void populateCorrespondentTab(
      ExternalOrgLoanTypes.ExternalOrgChannelLoanType channelType)
    {
      this.chkCConventional.Checked = (channelType.LoanTypes & 1) == 1;
      this.chkCFHA.Checked = (channelType.LoanTypes & 2) == 2;
      this.chkCVA.Checked = (channelType.LoanTypes & 4) == 4;
      this.chkCUSDA.Checked = (channelType.LoanTypes & 8) == 8;
      this.chkCHELOC.Checked = (channelType.LoanTypes & 16) == 16;
      this.chkCOther.Checked = (channelType.LoanTypes & 32) == 32;
      this.chkCFirstLien.Checked = (channelType.LoanTypes & 64) == 64;
      this.chkCSecondLien.Checked = (channelType.LoanTypes & 128) == 128;
      this.chkCPurchase.Checked = (channelType.LoanPurpose & 1) == 1;
      this.chkCNoRefin.Checked = (channelType.LoanPurpose & 2) == 2;
      this.chkCRefin.Checked = (channelType.LoanPurpose & 4) == 4;
      this.chkCConstruction.Checked = (channelType.LoanPurpose & 8) == 8;
      this.chkCConstructionPerm.Checked = (channelType.LoanPurpose & 16) == 16;
      this.chkCPOther.Checked = (channelType.LoanPurpose & 32) == 32;
      this.cmbCAllowLoansWithLicenseIssues.SelectedIndex = channelType.AllowLoansWithIssues;
      if (channelType.MsgUploadNonApprovedLoans != null && channelType.MsgUploadNonApprovedLoans.Trim() != "")
        this.txtCMsgUploadNonApproved.Text = channelType.MsgUploadNonApprovedLoans;
      else
        this.txtCMsgUploadNonApproved.Text = "Your organization is not approved to submit loans of this type. Please contact your Account Executive for assistance.";
    }

    private void populateCorrespondentNonDelTab(
      ExternalOrgLoanTypes.ExternalOrgChannelLoanType channelType)
    {
      this.chkNDConventional.Checked = (channelType.LoanTypes & 1) == 1;
      this.chkNDFHA.Checked = (channelType.LoanTypes & 2) == 2;
      this.chkNDVA.Checked = (channelType.LoanTypes & 4) == 4;
      this.chkNDUSDA.Checked = (channelType.LoanTypes & 8) == 8;
      this.chkNDHELOC.Checked = (channelType.LoanTypes & 16) == 16;
      this.chkNDOther.Checked = (channelType.LoanTypes & 32) == 32;
      this.chkNDFirstLien.Checked = (channelType.LoanTypes & 64) == 64;
      this.chkNDSecondLien.Checked = (channelType.LoanTypes & 128) == 128;
      this.chkNDPurchase.Checked = (channelType.LoanPurpose & 1) == 1;
      this.chkNDNoRefin.Checked = (channelType.LoanPurpose & 2) == 2;
      this.chkNDRefin.Checked = (channelType.LoanPurpose & 4) == 4;
      this.chkNDConstruction.Checked = (channelType.LoanPurpose & 8) == 8;
      this.chkNDConstructionPerm.Checked = (channelType.LoanPurpose & 16) == 16;
      this.chkNDPOther.Checked = (channelType.LoanPurpose & 32) == 32;
      this.cmbNDAllowLoansWithLicenseIssues.SelectedIndex = channelType.AllowLoansWithIssues;
      if (channelType.MsgUploadNonApprovedLoans != null && channelType.MsgUploadNonApprovedLoans.Trim() != "")
        this.txtNDMsgUploadNonApproved.Text = channelType.MsgUploadNonApprovedLoans;
      else
        this.txtNDMsgUploadNonApproved.Text = "Your organization is not approved to submit loans of this type. Please contact your Account Executive for assistance.";
    }

    private void populateFHAVA(ExternalOrgLoanTypes loanTypes)
    {
      this.txtFHAId.Text = loanTypes.FHAId;
      this.txtFHASponId.Text = loanTypes.FHASonsorId;
      this.cboFHAStatus.Text = loanTypes.FHAStatus;
      this.txtFHACompRatio.Text = loanTypes.FHACompareRatio == 0M ? "" : loanTypes.FHACompareRatio.ToString("0.#####");
      this.dpFHAApprovedDate.Text = loanTypes.FHAApprovedDate != DateTime.MinValue ? loanTypes.FHAApprovedDate.ToString("MM/dd/yyyy") : "";
      this.dpFHAExpDate.Text = loanTypes.FHAExpirationDate != DateTime.MinValue ? loanTypes.FHAExpirationDate.ToString("MM/dd/yyyy") : "";
      this.cboFHADirectEndorsement.Text = loanTypes.FHADirectEndorsement;
      this.txtVAId.Text = loanTypes.VAId;
      this.cboVAStatus.Text = loanTypes.VAStatus;
      this.dpVAApprovedDate.Text = loanTypes.VAApprovedDate != DateTime.MinValue ? loanTypes.VAApprovedDate.ToString("MM/dd/yyyy") : "";
      this.dpVAExpirationDate.Text = loanTypes.VAExpirationDate != DateTime.MinValue ? loanTypes.VAExpirationDate.ToString("MM/dd/yyyy") : "";
      this.txtVASponsorID.Text = loanTypes.VASponsorID;
      this.chkFNMAApproved.Checked = loanTypes.FNMAApproved;
      this.txtFannieMaeID.Text = loanTypes.FannieMaeID;
      this.chkFHMLCApproved.Checked = loanTypes.FHMLCApproved;
      this.txtFreddieMacID.Text = loanTypes.FreddieMacID;
      this.cboAUSMethod.Text = loanTypes.AUSMethod;
    }

    public ExternalOrgLoanTypes GetModifiedData()
    {
      ExternalOrgLoanTypes loanTypes = new ExternalOrgLoanTypes();
      loanTypes.Broker = this.getModifiedBroker(loanTypes.Broker);
      loanTypes.CorrespondentDelegated = this.getModifiedCorrDel(loanTypes.CorrespondentDelegated);
      loanTypes.CorrespondentNonDelegated = this.getModifiedCorrNonDel(loanTypes.CorrespondentNonDelegated);
      ExternalOrgLoanTypes modifiedFhaVa = this.getModifiedFhaVa(loanTypes);
      modifiedFhaVa.Underwriting = this.cmbUnderwriting.SelectedIndex;
      if (this.cmbUnderwriting.SelectedIndex == 2)
      {
        modifiedFhaVa.AdvancedCode = this.textConditionCode.Text;
        modifiedFhaVa.AdvancedCodeXml = this.advancedCodeXml;
      }
      else
        modifiedFhaVa.AdvancedCode = modifiedFhaVa.AdvancedCodeXml = "";
      return modifiedFhaVa;
    }

    private ExternalOrgLoanTypes.ExternalOrgChannelLoanType getModifiedCorrDel(
      ExternalOrgLoanTypes.ExternalOrgChannelLoanType loanTypes)
    {
      if (loanTypes == null)
      {
        loanTypes = new ExternalOrgLoanTypes.ExternalOrgChannelLoanType();
        loanTypes.ChannelType = 1;
      }
      loanTypes.LoanTypes = 0;
      if (this.chkCConventional.Checked)
        loanTypes.LoanTypes |= 1;
      if (this.chkCFHA.Checked)
        loanTypes.LoanTypes |= 2;
      if (this.chkCVA.Checked)
        loanTypes.LoanTypes |= 4;
      if (this.chkCUSDA.Checked)
        loanTypes.LoanTypes |= 8;
      if (this.chkCHELOC.Checked)
        loanTypes.LoanTypes |= 16;
      if (this.chkCOther.Checked)
        loanTypes.LoanTypes |= 32;
      if (this.chkCFirstLien.Checked)
        loanTypes.LoanTypes |= 64;
      if (this.chkCSecondLien.Checked)
        loanTypes.LoanTypes |= 128;
      loanTypes.LoanPurpose = 0;
      if (this.chkCPurchase.Checked)
        loanTypes.LoanPurpose |= 1;
      if (this.chkCNoRefin.Checked)
        loanTypes.LoanPurpose |= 2;
      if (this.chkCRefin.Checked)
        loanTypes.LoanPurpose |= 4;
      if (this.chkCConstruction.Checked)
        loanTypes.LoanPurpose |= 8;
      if (this.chkCConstructionPerm.Checked)
        loanTypes.LoanPurpose |= 16;
      if (this.chkCPOther.Checked)
        loanTypes.LoanPurpose |= 32;
      loanTypes.AllowLoansWithIssues = this.cmbCAllowLoansWithLicenseIssues.SelectedIndex;
      loanTypes.MsgUploadNonApprovedLoans = this.txtCMsgUploadNonApproved.Text.Trim();
      return loanTypes;
    }

    private ExternalOrgLoanTypes.ExternalOrgChannelLoanType getModifiedCorrNonDel(
      ExternalOrgLoanTypes.ExternalOrgChannelLoanType loanTypes)
    {
      if (loanTypes == null)
      {
        loanTypes = new ExternalOrgLoanTypes.ExternalOrgChannelLoanType();
        loanTypes.ChannelType = 2;
      }
      loanTypes.LoanTypes = 0;
      if (this.chkNDConventional.Checked)
        loanTypes.LoanTypes |= 1;
      if (this.chkNDFHA.Checked)
        loanTypes.LoanTypes |= 2;
      if (this.chkNDVA.Checked)
        loanTypes.LoanTypes |= 4;
      if (this.chkNDUSDA.Checked)
        loanTypes.LoanTypes |= 8;
      if (this.chkNDHELOC.Checked)
        loanTypes.LoanTypes |= 16;
      if (this.chkNDOther.Checked)
        loanTypes.LoanTypes |= 32;
      if (this.chkNDFirstLien.Checked)
        loanTypes.LoanTypes |= 64;
      if (this.chkNDSecondLien.Checked)
        loanTypes.LoanTypes |= 128;
      loanTypes.LoanPurpose = 0;
      if (this.chkNDPurchase.Checked)
        loanTypes.LoanPurpose |= 1;
      if (this.chkNDNoRefin.Checked)
        loanTypes.LoanPurpose |= 2;
      if (this.chkNDRefin.Checked)
        loanTypes.LoanPurpose |= 4;
      if (this.chkNDConstruction.Checked)
        loanTypes.LoanPurpose |= 8;
      if (this.chkNDConstructionPerm.Checked)
        loanTypes.LoanPurpose |= 16;
      if (this.chkNDPOther.Checked)
        loanTypes.LoanPurpose |= 32;
      loanTypes.AllowLoansWithIssues = this.cmbNDAllowLoansWithLicenseIssues.SelectedIndex;
      loanTypes.MsgUploadNonApprovedLoans = this.txtNDMsgUploadNonApproved.Text.Trim();
      return loanTypes;
    }

    private ExternalOrgLoanTypes.ExternalOrgChannelLoanType getModifiedBroker(
      ExternalOrgLoanTypes.ExternalOrgChannelLoanType loanTypes)
    {
      if (loanTypes == null)
      {
        loanTypes = new ExternalOrgLoanTypes.ExternalOrgChannelLoanType();
        loanTypes.ChannelType = 0;
      }
      loanTypes.LoanTypes = 0;
      if (this.chkBConventional.Checked)
        loanTypes.LoanTypes |= 1;
      if (this.chkBFHA.Checked)
        loanTypes.LoanTypes |= 2;
      if (this.chkBVA.Checked)
        loanTypes.LoanTypes |= 4;
      if (this.chkBUSDA.Checked)
        loanTypes.LoanTypes |= 8;
      if (this.chkBHELOC.Checked)
        loanTypes.LoanTypes |= 16;
      if (this.chkBOther.Checked)
        loanTypes.LoanTypes |= 32;
      if (this.chkBFirstLien.Checked)
        loanTypes.LoanTypes |= 64;
      if (this.chkBSecondLien.Checked)
        loanTypes.LoanTypes |= 128;
      loanTypes.LoanPurpose = 0;
      if (this.chkBPurchase.Checked)
        loanTypes.LoanPurpose |= 1;
      if (this.chkBNoRefin.Checked)
        loanTypes.LoanPurpose |= 2;
      if (this.chkBRefin.Checked)
        loanTypes.LoanPurpose |= 4;
      if (this.chkBConstruction.Checked)
        loanTypes.LoanPurpose |= 8;
      if (this.chkBConstructionPerm.Checked)
        loanTypes.LoanPurpose |= 16;
      if (this.chkBPOther.Checked)
        loanTypes.LoanPurpose |= 32;
      loanTypes.AllowLoansWithIssues = this.cmbBAllowLoansWithLicenseIssues.SelectedIndex;
      loanTypes.MsgUploadNonApprovedLoans = this.txtBMsgUploadNonApproved.Text.Trim();
      return loanTypes;
    }

    private ExternalOrgLoanTypes getModifiedFhaVa(ExternalOrgLoanTypes loanTypes)
    {
      loanTypes.FHAId = this.txtFHAId.Text.Trim();
      loanTypes.FHASonsorId = this.txtFHASponId.Text.Trim();
      loanTypes.FHAStatus = this.cboFHAStatus.Text;
      loanTypes.FHACompareRatio = this.txtFHACompRatio.Text.Trim() != string.Empty ? Utils.ParseDecimal((object) this.txtFHACompRatio.Text.Trim()) : 0M;
      loanTypes.FHAApprovedDate = this.dpFHAApprovedDate.Text != string.Empty ? this.dpFHAApprovedDate.Value : DateTime.MinValue;
      loanTypes.FHADirectEndorsement = this.cboFHADirectEndorsement.Text;
      loanTypes.FHAExpirationDate = this.dpFHAExpDate.Text != string.Empty ? this.dpFHAExpDate.Value : DateTime.MinValue;
      loanTypes.VAId = this.txtVAId.Text.Trim();
      loanTypes.VAStatus = this.cboVAStatus.Text;
      loanTypes.VAApprovedDate = this.dpVAApprovedDate.Text != string.Empty ? this.dpVAApprovedDate.Value : DateTime.MinValue;
      loanTypes.VAExpirationDate = this.dpVAExpirationDate.Text != string.Empty ? this.dpVAExpirationDate.Value : DateTime.MinValue;
      loanTypes.UseParentInfoFhaVa = this.chkUseParentInfo.Checked;
      loanTypes.VASponsorID = this.txtVASponsorID.Text.Trim();
      loanTypes.FHMLCApproved = this.chkFHMLCApproved.Checked;
      loanTypes.FNMAApproved = this.chkFNMAApproved.Checked;
      loanTypes.FannieMaeID = this.txtFannieMaeID.Text.Trim();
      loanTypes.FreddieMacID = this.txtFreddieMacID.Text.Trim();
      loanTypes.AUSMethod = this.cboAUSMethod.Text;
      return loanTypes;
    }

    public bool DataValidated()
    {
      DateTime dateTime1 = this.dpFHAApprovedDate.Value;
      if (dateTime1 != DateTime.MinValue && (dateTime1 > this.maxValue || dateTime1 < this.minValue))
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "The value of FHA Approved Date '" + dateTime1.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.dpFHAApprovedDate.Focus();
        return false;
      }
      DateTime dateTime2 = this.dpFHAExpDate.Value;
      if (dateTime2 != DateTime.MinValue && (dateTime2 > this.maxValue || dateTime2 < this.minValue))
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "The value of FHA Expiration Date '" + dateTime2.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.dpFHAExpDate.Focus();
        return false;
      }
      DateTime dateTime3 = this.dpVAApprovedDate.Value;
      if (dateTime3 != DateTime.MinValue && (dateTime3 > this.maxValue || dateTime3 < this.minValue))
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "The value of VA Approved Date '" + dateTime3.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.dpVAApprovedDate.Focus();
        return false;
      }
      DateTime dateTime4 = this.dpVAExpirationDate.Value;
      if (dateTime4 != DateTime.MinValue && (dateTime4 > this.maxValue || dateTime4 < this.minValue))
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "The value of VA Expiration Date '" + dateTime4.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.dpVAExpirationDate.Focus();
        return false;
      }
      return this.cmbUnderwriting.SelectedIndex != 2 || this.validateAdvancedCode();
    }

    private void textField_Changed(object sender, EventArgs e) => this.SetButtonStatus(true);

    public void SetButtonStatus(bool enabled)
    {
      this.btnSave.Enabled = this.btnReset.Enabled = enabled;
    }

    public bool IsDirty => this.btnSave.Enabled;

    private void tabLoanTypes_Resize(object sender, EventArgs e)
    {
      this.grpBPurpose.Size = new Size(this.tabBroker.Width / 2 - 15, this.grpBPurpose.Height);
      GroupContainer grpBpurpose = this.grpBPurpose;
      Point location = this.grpBLoanTypes.Location;
      int x = location.X + this.grpBLoanTypes.Width + 5;
      location = this.grpBPurpose.Location;
      int y = location.Y;
      Point point = new Point(x, y);
      grpBpurpose.Location = point;
    }

    private void tabCorrespondent_Resize(object sender, EventArgs e)
    {
      this.grpCLoanTypes.Size = new Size(this.tabCorrDel.Width / 2, this.tabCorrDel.Height / 2 - 2);
      this.grpCLicensingIssues.Size = new Size(this.tabCorrDel.Width / 2, this.tabCorrDel.Height / 2 - 12);
    }

    private void tabCorrNonDel_Resize(object sender, EventArgs e)
    {
      this.grpNDLoanTypes.Size = new Size(this.tabCorrNonDel.Width / 2, this.tabCorrNonDel.Height / 2 - 2);
      this.grpNDLicensingIssues.Size = new Size(this.tabCorrNonDel.Width / 2, this.tabCorrNonDel.Height / 2 - 12);
    }

    private void tabBroker_Resize(object sender, EventArgs e)
    {
      this.grpBLoanTypes.Size = new Size(this.tabBroker.Width / 2, this.tabBroker.Height / 2 - 2);
      this.grpBLicensingIssues.Size = new Size(this.tabBroker.Width / 2, this.tabBroker.Height / 2 - 12);
    }

    private void tabCorres_Resize(object sender, EventArgs e)
    {
      this.tabControl1.Size = new Size(this.tabCorres.Width - 2, this.tabCorres.Height - 52);
      this.panel1.Size = new Size(this.tabCorres.Width - 2, this.panel1.Height);
      this.grpCPurpose.Size = new Size(this.tabCorrDel.Width / 2 - 15, this.grpCPurpose.Height);
      GroupContainer grpCpurpose = this.grpCPurpose;
      Point location1 = this.grpCLoanTypes.Location;
      int x1 = location1.X + this.grpCLoanTypes.Width + 5;
      location1 = this.grpCPurpose.Location;
      int y1 = location1.Y;
      Point point1 = new Point(x1, y1);
      grpCpurpose.Location = point1;
      this.grpNDPurpose.Size = new Size(this.tabCorrNonDel.Width / 2 - 15, this.grpNDPurpose.Height);
      GroupContainer grpNdPurpose = this.grpNDPurpose;
      Point location2 = this.grpNDLoanTypes.Location;
      int x2 = location2.X + this.grpNDLoanTypes.Width + 5;
      location2 = this.grpNDPurpose.Location;
      int y2 = location2.Y;
      Point point2 = new Point(x2, y2);
      grpNdPurpose.Location = point2;
    }

    private void tabLoanTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
      Point location;
      if (this.tabLoanTypes.SelectedTab == this.tabBroker)
      {
        this.tabLoanTypes.Height -= 100;
        GroupContainer grpFhava = this.grpFHAVA;
        location = this.grpFHAVA.Location;
        int x = location.X;
        location = this.grpFHAVA.Location;
        int y = location.Y - 100;
        Point point = new Point(x, y);
        grpFhava.Location = point;
        this.tabBroker_Resize((object) null, (EventArgs) null);
      }
      if (this.tabLoanTypes.SelectedTab == this.tabCorres)
      {
        this.tabLoanTypes.Height += 100;
        GroupContainer grpFhava = this.grpFHAVA;
        location = this.grpFHAVA.Location;
        int x = location.X;
        location = this.grpFHAVA.Location;
        int y = location.Y + 100;
        Point point = new Point(x, y);
        grpFhava.Location = point;
        this.tabCorres_Resize((object) null, (EventArgs) null);
      }
      this.tabLoanTypes_Resize((object) null, (EventArgs) null);
    }

    private void chkUseParentInfo_CheckedChanged(object sender, EventArgs e)
    {
      this.chkCConventional.Enabled = this.chkCFHA.Enabled = this.chkCVA.Enabled = this.chkCHELOC.Enabled = this.chkCOther.Enabled = this.chkCUSDA.Enabled = this.chkCFirstLien.Enabled = this.chkCSecondLien.Enabled = !this.chkUseParentInfo.Checked;
      this.chkCPurchase.Enabled = this.chkCNoRefin.Enabled = this.chkCRefin.Enabled = this.chkCConstruction.Enabled = this.chkCConstructionPerm.Enabled = this.chkCPOther.Enabled = !this.chkUseParentInfo.Checked;
      this.cmbCAllowLoansWithLicenseIssues.Enabled = this.txtCMsgUploadNonApproved.Enabled = !this.chkUseParentInfo.Checked;
      this.chkNDConventional.Enabled = this.chkNDFHA.Enabled = this.chkNDVA.Enabled = this.chkNDHELOC.Enabled = this.chkNDOther.Enabled = this.chkNDUSDA.Enabled = this.chkNDFirstLien.Enabled = this.chkNDSecondLien.Enabled = !this.chkUseParentInfo.Checked;
      this.chkNDPurchase.Enabled = this.chkNDNoRefin.Enabled = this.chkNDRefin.Enabled = this.chkNDConstruction.Enabled = this.chkNDConstructionPerm.Enabled = this.chkNDPOther.Enabled = !this.chkUseParentInfo.Checked;
      this.cmbNDAllowLoansWithLicenseIssues.Enabled = this.txtNDMsgUploadNonApproved.Enabled = !this.chkUseParentInfo.Checked;
      this.chkBConventional.Enabled = this.chkBFHA.Enabled = this.chkBVA.Enabled = this.chkBHELOC.Enabled = this.chkBOther.Enabled = this.chkBUSDA.Enabled = this.chkBFirstLien.Enabled = this.chkBSecondLien.Enabled = !this.chkUseParentInfo.Checked;
      this.chkBPurchase.Enabled = this.chkBNoRefin.Enabled = this.chkBRefin.Enabled = this.chkBConstruction.Enabled = this.chkBConstructionPerm.Enabled = this.chkBPOther.Enabled = !this.chkUseParentInfo.Checked;
      this.cmbBAllowLoansWithLicenseIssues.Enabled = this.txtBMsgUploadNonApproved.Enabled = !this.chkUseParentInfo.Checked;
      this.txtFHAId.Enabled = this.txtFHASponId.Enabled = this.cboFHAStatus.Enabled = this.dpFHAApprovedDate.Enabled = this.dpFHAExpDate.Enabled = this.txtFHACompRatio.Enabled = this.txtVAId.Enabled = this.cboVAStatus.Enabled = this.dpVAApprovedDate.Enabled = this.dpVAExpirationDate.Enabled = this.cboFHADirectEndorsement.Enabled = this.txtVASponsorID.Enabled = this.chkFHMLCApproved.Enabled = this.chkFNMAApproved.Enabled = this.txtFannieMaeID.Enabled = this.txtFreddieMacID.Enabled = this.cboAUSMethod.Enabled = !this.chkUseParentInfo.Checked;
      this.panel1.Enabled = !this.chkUseParentInfo.Checked;
      this.SetButtonStatus(true);
      if (!this.chkUseParentInfo.Checked)
        return;
      if (this.parentLoanTypes.ExternalOrgID != 0)
        this.populateLoanTypes(this.parentLoanTypes);
      else
        this.populateLoanTypes((ExternalOrgLoanTypes) null);
      this.lastSelectedComboItemText = this.cmbBAllowLoansWithLicenseIssues.SelectedItem != null ? this.cmbBAllowLoansWithLicenseIssues.SelectedItem.ToString() : string.Empty;
      this.lastSelectedComboItemTextCorDel = this.cmbCAllowLoansWithLicenseIssues.SelectedItem != null ? this.cmbCAllowLoansWithLicenseIssues.SelectedItem.ToString() : string.Empty;
      this.lastSelectedComboItemTextCorNonDel = this.cmbNDAllowLoansWithLicenseIssues.SelectedItem != null ? this.cmbNDAllowLoansWithLicenseIssues.SelectedItem.ToString() : string.Empty;
    }

    public void DisableControls()
    {
      this.btnSave.Visible = false;
      this.btnReset.Visible = false;
      this.disableControl(this.Controls);
    }

    private void disableControl(Control.ControlCollection controls)
    {
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        switch (control)
        {
          case TextBox _:
          case CheckBox _:
          case ComboBox _:
          case DatePicker _:
            control.Enabled = false;
            break;
        }
        if (control.Controls != null && control.Controls.Count > 0)
          this.disableControl(control.Controls);
      }
    }

    private void grpAll_Resize(object sender, EventArgs e)
    {
      if (this.tabLoanTypes.SelectedTab == this.tabBroker)
        this.tabBroker_Resize((object) null, (EventArgs) null);
      if (this.tabLoanTypes.SelectedTab == this.tabCorres)
        this.tabCorres_Resize((object) null, (EventArgs) null);
      this.tabLoanTypes_Resize((object) null, (EventArgs) null);
    }

    private void cmbUnderwriting_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.textConditionCode.Visible = this.btnSelect.Visible = this.cmbUnderwriting.SelectedIndex == 2;
      this.textField_Changed((object) null, (EventArgs) null);
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      using (AdvConditionEditor advConditionEditor = new AdvConditionEditor(this.session, this.advancedCodeXml))
      {
        if (advConditionEditor.GetConditionScript() != this.textConditionCode.Text)
          advConditionEditor.ClearFilters();
        if (advConditionEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.textConditionCode.Text = advConditionEditor.GetConditionScript();
        this.advancedCodeXml = advConditionEditor.GetConditionXml();
      }
    }

    private bool validateAdvancedCode()
    {
      if (this.textConditionCode.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "You must provide code to determine the conditions under which the loan criteria is conditionally triggered.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      try
      {
        using (RuntimeContext context = RuntimeContext.Create())
          new AdvancedCodeCondition(this.textConditionCode.Text).CreateImplementation(context);
        return true;
      }
      catch (CompileException ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display((Exception) ex);
          return false;
        }
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Validation failed: the condition contains errors or is not a valid expression.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      catch (Exception ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display(ex);
          return false;
        }
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Error validating expression: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private void textboxNumericOnlyValidation(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsNumber(e.KeyChar))
        e.Handled = true;
      else
        e.Handled = false;
    }

    private void cmb_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (!(sender is ComboBox comboBox))
        return;
      if (comboBox.SelectedItem != null)
      {
        string empty = comboBox.SelectedItem.ToString();
        if (!string.IsNullOrEmpty(empty) && !string.IsNullOrWhiteSpace(empty))
        {
          string str = string.Empty;
          switch (comboBox.Name)
          {
            case "cmbBAllowLoansWithLicenseIssues":
              str = this.lastSelectedComboItemText.ToLower();
              this.lastSelectedComboItemText = empty;
              break;
            case "cmbCAllowLoansWithLicenseIssues":
              str = this.lastSelectedComboItemTextCorDel.ToLower();
              this.lastSelectedComboItemTextCorDel = empty;
              break;
            case "cmbNDAllowLoansWithLicenseIssues":
              str = this.lastSelectedComboItemTextCorNonDel.ToLower();
              this.lastSelectedComboItemTextCorNonDel = empty;
              break;
            default:
              empty = string.Empty;
              break;
          }
          if (empty.ToLower() == "no restrictions" && str != "no restrictions")
          {
            int num = (int) MessageBox.Show("Please Note: Selecting ‘No Restrictions’ will allow this company to submit loans with no limits specific to this policy.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      }
      this.SetButtonStatus(true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditCompanyLoanTypeControl));
      this.grpAll = new GroupContainer();
      this.tabLoanTypes = new TabControl();
      this.tabBroker = new TabPage();
      this.grpBLicensingIssues = new GroupContainer();
      this.cmbBAllowLoansWithLicenseIssues = new ComboBox();
      this.txtBMsgUploadNonApproved = new TextBox();
      this.label7 = new Label();
      this.grpBLoanTypes = new GroupContainer();
      this.chkBOther = new CheckBox();
      this.chkBSecondLien = new CheckBox();
      this.chkBUSDA = new CheckBox();
      this.chkBFirstLien = new CheckBox();
      this.chkBHELOC = new CheckBox();
      this.chkBVA = new CheckBox();
      this.chkBFHA = new CheckBox();
      this.chkBConventional = new CheckBox();
      this.grpBPurpose = new GroupContainer();
      this.chkBPOther = new CheckBox();
      this.chkBConstructionPerm = new CheckBox();
      this.chkBConstruction = new CheckBox();
      this.chkBRefin = new CheckBox();
      this.chkBNoRefin = new CheckBox();
      this.chkBPurchase = new CheckBox();
      this.tabCorres = new TabPage();
      this.tabControl1 = new TabControl();
      this.tabCorrDel = new TabPage();
      this.grpCLicensingIssues = new GroupContainer();
      this.cmbCAllowLoansWithLicenseIssues = new ComboBox();
      this.txtCMsgUploadNonApproved = new TextBox();
      this.label8 = new Label();
      this.grpCLoanTypes = new GroupContainer();
      this.chkCOther = new CheckBox();
      this.chkCSecondLien = new CheckBox();
      this.chkCUSDA = new CheckBox();
      this.chkCFirstLien = new CheckBox();
      this.chkCHELOC = new CheckBox();
      this.chkCVA = new CheckBox();
      this.chkCFHA = new CheckBox();
      this.chkCConventional = new CheckBox();
      this.grpCPurpose = new GroupContainer();
      this.chkCPOther = new CheckBox();
      this.chkCConstructionPerm = new CheckBox();
      this.chkCConstruction = new CheckBox();
      this.chkCRefin = new CheckBox();
      this.chkCNoRefin = new CheckBox();
      this.chkCPurchase = new CheckBox();
      this.tabCorrNonDel = new TabPage();
      this.grpNDLicensingIssues = new GroupContainer();
      this.cmbNDAllowLoansWithLicenseIssues = new ComboBox();
      this.txtNDMsgUploadNonApproved = new TextBox();
      this.label10 = new Label();
      this.grpNDLoanTypes = new GroupContainer();
      this.chkNDOther = new CheckBox();
      this.chkNDSecondLien = new CheckBox();
      this.chkNDUSDA = new CheckBox();
      this.chkNDFirstLien = new CheckBox();
      this.chkNDHELOC = new CheckBox();
      this.chkNDVA = new CheckBox();
      this.chkNDFHA = new CheckBox();
      this.chkNDConventional = new CheckBox();
      this.grpNDPurpose = new GroupContainer();
      this.chkNDPOther = new CheckBox();
      this.chkNDConstructionPerm = new CheckBox();
      this.chkNDConstruction = new CheckBox();
      this.chkNDRefin = new CheckBox();
      this.chkNDNoRefin = new CheckBox();
      this.chkNDPurchase = new CheckBox();
      this.panel1 = new Panel();
      this.btnSelect = new StandardIconButton();
      this.textConditionCode = new TextBox();
      this.cmbUnderwriting = new ComboBox();
      this.label2 = new Label();
      this.btnReset = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.panelHeader = new Panel();
      this.label33 = new Label();
      this.grpFHAVA = new GroupContainer();
      this.cboAUSMethod = new ComboBox();
      this.label20 = new Label();
      this.txtFreddieMacID = new TextBox();
      this.label19 = new Label();
      this.chkFHMLCApproved = new CheckBox();
      this.label18 = new Label();
      this.txtFannieMaeID = new TextBox();
      this.label13 = new Label();
      this.chkFNMAApproved = new CheckBox();
      this.label12 = new Label();
      this.txtVASponsorID = new TextBox();
      this.label11 = new Label();
      this.cboFHADirectEndorsement = new ComboBox();
      this.label9 = new Label();
      this.lblPercent = new Label();
      this.txtVAId = new TextBox();
      this.cboVAStatus = new ComboBox();
      this.label3 = new Label();
      this.dpVAExpirationDate = new DatePicker();
      this.dpVAApprovedDate = new DatePicker();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.txtFHACompRatio = new TextBox();
      this.txtFHASponId = new TextBox();
      this.label1 = new Label();
      this.txtFHAId = new TextBox();
      this.cboFHAStatus = new ComboBox();
      this.label40 = new Label();
      this.dpFHAExpDate = new DatePicker();
      this.dpFHAApprovedDate = new DatePicker();
      this.label14 = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.label17 = new Label();
      this.chkUseParentInfo = new CheckBox();
      this.pnlFHAVA = new Panel();
      this.lblPadding = new Label();
      this.grpAll.SuspendLayout();
      this.tabLoanTypes.SuspendLayout();
      this.tabBroker.SuspendLayout();
      this.grpBLicensingIssues.SuspendLayout();
      this.grpBLoanTypes.SuspendLayout();
      this.grpBPurpose.SuspendLayout();
      this.tabCorres.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabCorrDel.SuspendLayout();
      this.grpCLicensingIssues.SuspendLayout();
      this.grpCLoanTypes.SuspendLayout();
      this.grpCPurpose.SuspendLayout();
      this.tabCorrNonDel.SuspendLayout();
      this.grpNDLicensingIssues.SuspendLayout();
      this.grpNDLoanTypes.SuspendLayout();
      this.grpNDPurpose.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.btnSelect).BeginInit();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.panelHeader.SuspendLayout();
      this.grpFHAVA.SuspendLayout();
      this.pnlFHAVA.SuspendLayout();
      this.SuspendLayout();
      this.grpAll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpAll.AutoScrollMargin = new Size(20, 0);
      this.grpAll.Controls.Add((Control) this.tabLoanTypes);
      this.grpAll.Controls.Add((Control) this.btnReset);
      this.grpAll.Controls.Add((Control) this.btnSave);
      this.grpAll.Controls.Add((Control) this.panelHeader);
      this.grpAll.Controls.Add((Control) this.grpFHAVA);
      this.grpAll.Controls.Add((Control) this.chkUseParentInfo);
      this.grpAll.HeaderForeColor = SystemColors.ControlText;
      this.grpAll.Location = new Point(5, 5);
      this.grpAll.Margin = new Padding(0);
      this.grpAll.Name = "grpAll";
      this.grpAll.Padding = new Padding(5);
      this.grpAll.Size = new Size(862, 610);
      this.grpAll.TabIndex = 31;
      this.grpAll.Text = "Loan Criteria";
      this.grpAll.Resize += new EventHandler(this.grpAll_Resize);
      this.tabLoanTypes.Controls.Add((Control) this.tabBroker);
      this.tabLoanTypes.Controls.Add((Control) this.tabCorres);
      this.tabLoanTypes.Dock = DockStyle.Top;
      this.tabLoanTypes.Location = new Point(6, 67);
      this.tabLoanTypes.Margin = new Padding(5);
      this.tabLoanTypes.Name = "tabLoanTypes";
      this.tabLoanTypes.SelectedIndex = 0;
      this.tabLoanTypes.Size = new Size(850, 241);
      this.tabLoanTypes.TabIndex = 1;
      this.tabLoanTypes.SelectedIndexChanged += new EventHandler(this.tabLoanTypes_SelectedIndexChanged);
      this.tabLoanTypes.Resize += new EventHandler(this.tabLoanTypes_Resize);
      this.tabBroker.Controls.Add((Control) this.grpBLicensingIssues);
      this.tabBroker.Controls.Add((Control) this.grpBLoanTypes);
      this.tabBroker.Controls.Add((Control) this.grpBPurpose);
      this.tabBroker.Location = new Point(4, 22);
      this.tabBroker.Name = "tabBroker";
      this.tabBroker.Size = new Size(842, 215);
      this.tabBroker.TabIndex = 0;
      this.tabBroker.Text = "Broker";
      this.tabBroker.UseVisualStyleBackColor = true;
      this.tabBroker.Resize += new EventHandler(this.tabBroker_Resize);
      this.grpBLicensingIssues.Controls.Add((Control) this.cmbBAllowLoansWithLicenseIssues);
      this.grpBLicensingIssues.Controls.Add((Control) this.txtBMsgUploadNonApproved);
      this.grpBLicensingIssues.Controls.Add((Control) this.label7);
      this.grpBLicensingIssues.HeaderForeColor = SystemColors.ControlText;
      this.grpBLicensingIssues.Location = new Point(5, 115);
      this.grpBLicensingIssues.Name = "grpBLicensingIssues";
      this.grpBLicensingIssues.Size = new Size(390, 96);
      this.grpBLicensingIssues.TabIndex = 1;
      this.grpBLicensingIssues.Text = "Policy for Unapproved Loans";
      this.cmbBAllowLoansWithLicenseIssues.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cmbBAllowLoansWithLicenseIssues.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBAllowLoansWithLicenseIssues.FormattingEnabled = true;
      this.cmbBAllowLoansWithLicenseIssues.Items.AddRange(new object[4]
      {
        (object) "No Restrictions",
        (object) "Don't allow lock or submission",
        (object) "Don't allow loan creation",
        (object) "Don't allow all - loan creation/lock/submission"
      });
      this.cmbBAllowLoansWithLicenseIssues.Location = new Point(136, 3);
      this.cmbBAllowLoansWithLicenseIssues.Name = "cmbBAllowLoansWithLicenseIssues";
      this.cmbBAllowLoansWithLicenseIssues.Size = new Size(248, 21);
      this.cmbBAllowLoansWithLicenseIssues.TabIndex = 10;
      this.cmbBAllowLoansWithLicenseIssues.SelectionChangeCommitted += new EventHandler(this.cmb_SelectionChangeCommitted);
      this.txtBMsgUploadNonApproved.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBMsgUploadNonApproved.Location = new Point(104, 30);
      this.txtBMsgUploadNonApproved.Multiline = true;
      this.txtBMsgUploadNonApproved.Name = "txtBMsgUploadNonApproved";
      this.txtBMsgUploadNonApproved.ScrollBars = ScrollBars.Vertical;
      this.txtBMsgUploadNonApproved.Size = new Size(278, 55);
      this.txtBMsgUploadNonApproved.TabIndex = 9;
      this.txtBMsgUploadNonApproved.TextChanged += new EventHandler(this.textField_Changed);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(10, 33);
      this.label7.Name = "label7";
      this.label7.Size = new Size(93, 13);
      this.label7.TabIndex = 4;
      this.label7.Text = "Warning Message";
      this.grpBLoanTypes.Controls.Add((Control) this.chkBOther);
      this.grpBLoanTypes.Controls.Add((Control) this.chkBSecondLien);
      this.grpBLoanTypes.Controls.Add((Control) this.chkBUSDA);
      this.grpBLoanTypes.Controls.Add((Control) this.chkBFirstLien);
      this.grpBLoanTypes.Controls.Add((Control) this.chkBHELOC);
      this.grpBLoanTypes.Controls.Add((Control) this.chkBVA);
      this.grpBLoanTypes.Controls.Add((Control) this.chkBFHA);
      this.grpBLoanTypes.Controls.Add((Control) this.chkBConventional);
      this.grpBLoanTypes.HeaderForeColor = SystemColors.ControlText;
      this.grpBLoanTypes.Location = new Point(5, 5);
      this.grpBLoanTypes.Margin = new Padding(5);
      this.grpBLoanTypes.Name = "grpBLoanTypes";
      this.grpBLoanTypes.Size = new Size(390, 106);
      this.grpBLoanTypes.TabIndex = 0;
      this.grpBLoanTypes.Text = "Approved Loan Types";
      this.chkBOther.AutoSize = true;
      this.chkBOther.Location = new Point(140, 78);
      this.chkBOther.Name = "chkBOther";
      this.chkBOther.Size = new Size(52, 17);
      this.chkBOther.TabIndex = 8;
      this.chkBOther.Text = "Other";
      this.chkBOther.UseVisualStyleBackColor = true;
      this.chkBOther.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBSecondLien.AutoSize = true;
      this.chkBSecondLien.Location = new Point(270, 58);
      this.chkBSecondLien.Name = "chkBSecondLien";
      this.chkBSecondLien.Size = new Size(86, 17);
      this.chkBSecondLien.TabIndex = 7;
      this.chkBSecondLien.Text = "Second Lien";
      this.chkBSecondLien.UseVisualStyleBackColor = true;
      this.chkBSecondLien.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBUSDA.AutoSize = true;
      this.chkBUSDA.Location = new Point(140, 38);
      this.chkBUSDA.Name = "chkBUSDA";
      this.chkBUSDA.Size = new Size(88, 17);
      this.chkBUSDA.TabIndex = 4;
      this.chkBUSDA.Text = "USDA - RHS";
      this.chkBUSDA.UseVisualStyleBackColor = true;
      this.chkBUSDA.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBFirstLien.AutoSize = true;
      this.chkBFirstLien.Location = new Point(270, 38);
      this.chkBFirstLien.Name = "chkBFirstLien";
      this.chkBFirstLien.Size = new Size(68, 17);
      this.chkBFirstLien.TabIndex = 6;
      this.chkBFirstLien.Text = "First Lien";
      this.chkBFirstLien.UseVisualStyleBackColor = true;
      this.chkBFirstLien.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBHELOC.AutoSize = true;
      this.chkBHELOC.Location = new Point(140, 58);
      this.chkBHELOC.Name = "chkBHELOC";
      this.chkBHELOC.Size = new Size(62, 17);
      this.chkBHELOC.TabIndex = 5;
      this.chkBHELOC.Text = "HELOC";
      this.chkBHELOC.UseVisualStyleBackColor = true;
      this.chkBHELOC.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBVA.AutoSize = true;
      this.chkBVA.Location = new Point(10, 78);
      this.chkBVA.Name = "chkBVA";
      this.chkBVA.Size = new Size(40, 17);
      this.chkBVA.TabIndex = 3;
      this.chkBVA.Text = "VA";
      this.chkBVA.UseVisualStyleBackColor = true;
      this.chkBVA.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBFHA.AutoSize = true;
      this.chkBFHA.Location = new Point(10, 58);
      this.chkBFHA.Name = "chkBFHA";
      this.chkBFHA.Size = new Size(47, 17);
      this.chkBFHA.TabIndex = 2;
      this.chkBFHA.Text = "FHA";
      this.chkBFHA.UseVisualStyleBackColor = true;
      this.chkBFHA.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBConventional.AutoSize = true;
      this.chkBConventional.Location = new Point(10, 38);
      this.chkBConventional.Name = "chkBConventional";
      this.chkBConventional.Size = new Size(88, 17);
      this.chkBConventional.TabIndex = 1;
      this.chkBConventional.Text = "Conventional";
      this.chkBConventional.UseVisualStyleBackColor = true;
      this.chkBConventional.CheckedChanged += new EventHandler(this.textField_Changed);
      this.grpBPurpose.Controls.Add((Control) this.chkBPOther);
      this.grpBPurpose.Controls.Add((Control) this.chkBConstructionPerm);
      this.grpBPurpose.Controls.Add((Control) this.chkBConstruction);
      this.grpBPurpose.Controls.Add((Control) this.chkBRefin);
      this.grpBPurpose.Controls.Add((Control) this.chkBNoRefin);
      this.grpBPurpose.Controls.Add((Control) this.chkBPurchase);
      this.grpBPurpose.HeaderForeColor = SystemColors.ControlText;
      this.grpBPurpose.Location = new Point(400, 5);
      this.grpBPurpose.Name = "grpBPurpose";
      this.grpBPurpose.Padding = new Padding(0, 0, 5, 0);
      this.grpBPurpose.Size = new Size(425, 205);
      this.grpBPurpose.TabIndex = 2;
      this.grpBPurpose.Text = "Approved Loan Purposes";
      this.chkBPOther.AutoSize = true;
      this.chkBPOther.Location = new Point(10, 138);
      this.chkBPOther.Name = "chkBPOther";
      this.chkBPOther.Size = new Size(52, 17);
      this.chkBPOther.TabIndex = 22;
      this.chkBPOther.Text = "Other";
      this.chkBPOther.UseVisualStyleBackColor = true;
      this.chkBPOther.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBConstructionPerm.AutoSize = true;
      this.chkBConstructionPerm.Location = new Point(10, 118);
      this.chkBConstructionPerm.Name = "chkBConstructionPerm";
      this.chkBConstructionPerm.Size = new Size(121, 17);
      this.chkBConstructionPerm.TabIndex = 17;
      this.chkBConstructionPerm.Text = "Construction - Perm ";
      this.chkBConstructionPerm.UseVisualStyleBackColor = true;
      this.chkBConstructionPerm.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBConstruction.AutoSize = true;
      this.chkBConstruction.Location = new Point(10, 98);
      this.chkBConstruction.Name = "chkBConstruction";
      this.chkBConstruction.Size = new Size(88, 17);
      this.chkBConstruction.TabIndex = 16;
      this.chkBConstruction.Text = "Construction ";
      this.chkBConstruction.UseVisualStyleBackColor = true;
      this.chkBConstruction.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBRefin.AutoSize = true;
      this.chkBRefin.Location = new Point(10, 78);
      this.chkBRefin.Name = "chkBRefin";
      this.chkBRefin.Size = new Size(122, 17);
      this.chkBRefin.TabIndex = 13;
      this.chkBRefin.Text = "Cash-Out Refinance";
      this.chkBRefin.UseVisualStyleBackColor = true;
      this.chkBRefin.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBNoRefin.AutoSize = true;
      this.chkBNoRefin.Location = new Point(10, 58);
      this.chkBNoRefin.Name = "chkBNoRefin";
      this.chkBNoRefin.Size = new Size(139, 17);
      this.chkBNoRefin.TabIndex = 12;
      this.chkBNoRefin.Text = "No Cash-Out Refinance";
      this.chkBNoRefin.UseVisualStyleBackColor = true;
      this.chkBNoRefin.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkBPurchase.AutoSize = true;
      this.chkBPurchase.Location = new Point(10, 38);
      this.chkBPurchase.Name = "chkBPurchase";
      this.chkBPurchase.Size = new Size(71, 17);
      this.chkBPurchase.TabIndex = 11;
      this.chkBPurchase.Text = "Purchase";
      this.chkBPurchase.UseVisualStyleBackColor = true;
      this.chkBPurchase.CheckedChanged += new EventHandler(this.textField_Changed);
      this.tabCorres.Controls.Add((Control) this.tabControl1);
      this.tabCorres.Controls.Add((Control) this.panel1);
      this.tabCorres.Location = new Point(4, 22);
      this.tabCorres.Name = "tabCorres";
      this.tabCorres.Size = new Size(842, 215);
      this.tabCorres.TabIndex = 3;
      this.tabCorres.Text = "Correspondent";
      this.tabCorres.UseVisualStyleBackColor = true;
      this.tabCorres.Resize += new EventHandler(this.tabCorres_Resize);
      this.tabControl1.Controls.Add((Control) this.tabCorrDel);
      this.tabControl1.Controls.Add((Control) this.tabCorrNonDel);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(0, 71);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(842, 144);
      this.tabControl1.TabIndex = 2;
      this.tabCorrDel.Controls.Add((Control) this.grpCLicensingIssues);
      this.tabCorrDel.Controls.Add((Control) this.grpCLoanTypes);
      this.tabCorrDel.Controls.Add((Control) this.grpCPurpose);
      this.tabCorrDel.Location = new Point(4, 22);
      this.tabCorrDel.Name = "tabCorrDel";
      this.tabCorrDel.Size = new Size(834, 118);
      this.tabCorrDel.TabIndex = 1;
      this.tabCorrDel.Text = "Correspondent - Delegated";
      this.tabCorrDel.UseVisualStyleBackColor = true;
      this.tabCorrDel.Resize += new EventHandler(this.tabCorrespondent_Resize);
      this.grpCLicensingIssues.Controls.Add((Control) this.cmbCAllowLoansWithLicenseIssues);
      this.grpCLicensingIssues.Controls.Add((Control) this.txtCMsgUploadNonApproved);
      this.grpCLicensingIssues.Controls.Add((Control) this.label8);
      this.grpCLicensingIssues.HeaderForeColor = SystemColors.ControlText;
      this.grpCLicensingIssues.Location = new Point(5, 115);
      this.grpCLicensingIssues.Name = "grpCLicensingIssues";
      this.grpCLicensingIssues.Size = new Size(390, 96);
      this.grpCLicensingIssues.TabIndex = 2;
      this.grpCLicensingIssues.Text = "Policy for Unapproved Loans";
      this.cmbCAllowLoansWithLicenseIssues.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cmbCAllowLoansWithLicenseIssues.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCAllowLoansWithLicenseIssues.FormattingEnabled = true;
      this.cmbCAllowLoansWithLicenseIssues.Items.AddRange(new object[4]
      {
        (object) "No Restrictions",
        (object) "Don't allow lock or submission",
        (object) "Don't allow loan creation",
        (object) "Don't allow all - loan creation/lock/submission"
      });
      this.cmbCAllowLoansWithLicenseIssues.Location = new Point(136, 3);
      this.cmbCAllowLoansWithLicenseIssues.Name = "cmbCAllowLoansWithLicenseIssues";
      this.cmbCAllowLoansWithLicenseIssues.Size = new Size(248, 21);
      this.cmbCAllowLoansWithLicenseIssues.TabIndex = 11;
      this.cmbCAllowLoansWithLicenseIssues.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cmbCAllowLoansWithLicenseIssues.SelectionChangeCommitted += new EventHandler(this.cmb_SelectionChangeCommitted);
      this.txtCMsgUploadNonApproved.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCMsgUploadNonApproved.Location = new Point(104, 30);
      this.txtCMsgUploadNonApproved.Multiline = true;
      this.txtCMsgUploadNonApproved.Name = "txtCMsgUploadNonApproved";
      this.txtCMsgUploadNonApproved.ScrollBars = ScrollBars.Vertical;
      this.txtCMsgUploadNonApproved.Size = new Size(278, 55);
      this.txtCMsgUploadNonApproved.TabIndex = 9;
      this.txtCMsgUploadNonApproved.TextChanged += new EventHandler(this.textField_Changed);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(10, 33);
      this.label8.Name = "label8";
      this.label8.Size = new Size(93, 13);
      this.label8.TabIndex = 4;
      this.label8.Text = "Warning Message";
      this.grpCLoanTypes.Controls.Add((Control) this.chkCOther);
      this.grpCLoanTypes.Controls.Add((Control) this.chkCSecondLien);
      this.grpCLoanTypes.Controls.Add((Control) this.chkCUSDA);
      this.grpCLoanTypes.Controls.Add((Control) this.chkCFirstLien);
      this.grpCLoanTypes.Controls.Add((Control) this.chkCHELOC);
      this.grpCLoanTypes.Controls.Add((Control) this.chkCVA);
      this.grpCLoanTypes.Controls.Add((Control) this.chkCFHA);
      this.grpCLoanTypes.Controls.Add((Control) this.chkCConventional);
      this.grpCLoanTypes.HeaderForeColor = SystemColors.ControlText;
      this.grpCLoanTypes.Location = new Point(5, 5);
      this.grpCLoanTypes.Margin = new Padding(5);
      this.grpCLoanTypes.Name = "grpCLoanTypes";
      this.grpCLoanTypes.Size = new Size(390, 106);
      this.grpCLoanTypes.TabIndex = 0;
      this.grpCLoanTypes.Text = "Approved Loan Types";
      this.chkCOther.AutoSize = true;
      this.chkCOther.Location = new Point(140, 78);
      this.chkCOther.Name = "chkCOther";
      this.chkCOther.Size = new Size(52, 17);
      this.chkCOther.TabIndex = 9;
      this.chkCOther.Text = "Other";
      this.chkCOther.UseVisualStyleBackColor = true;
      this.chkCOther.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCSecondLien.AutoSize = true;
      this.chkCSecondLien.Location = new Point(270, 58);
      this.chkCSecondLien.Name = "chkCSecondLien";
      this.chkCSecondLien.Size = new Size(86, 17);
      this.chkCSecondLien.TabIndex = 7;
      this.chkCSecondLien.Text = "Second Lien";
      this.chkCSecondLien.UseVisualStyleBackColor = true;
      this.chkCSecondLien.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCUSDA.AutoSize = true;
      this.chkCUSDA.Location = new Point(140, 38);
      this.chkCUSDA.Name = "chkCUSDA";
      this.chkCUSDA.Size = new Size(88, 17);
      this.chkCUSDA.TabIndex = 4;
      this.chkCUSDA.Text = "USDA - RHS";
      this.chkCUSDA.UseVisualStyleBackColor = true;
      this.chkCUSDA.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCFirstLien.AutoSize = true;
      this.chkCFirstLien.Location = new Point(270, 38);
      this.chkCFirstLien.Name = "chkCFirstLien";
      this.chkCFirstLien.Size = new Size(68, 17);
      this.chkCFirstLien.TabIndex = 6;
      this.chkCFirstLien.Text = "First Lien";
      this.chkCFirstLien.UseVisualStyleBackColor = true;
      this.chkCFirstLien.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCHELOC.AutoSize = true;
      this.chkCHELOC.Location = new Point(140, 58);
      this.chkCHELOC.Name = "chkCHELOC";
      this.chkCHELOC.Size = new Size(62, 17);
      this.chkCHELOC.TabIndex = 5;
      this.chkCHELOC.Text = "HELOC";
      this.chkCHELOC.UseVisualStyleBackColor = true;
      this.chkCHELOC.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCVA.AutoSize = true;
      this.chkCVA.Location = new Point(10, 78);
      this.chkCVA.Name = "chkCVA";
      this.chkCVA.Size = new Size(40, 17);
      this.chkCVA.TabIndex = 3;
      this.chkCVA.Text = "VA";
      this.chkCVA.UseVisualStyleBackColor = true;
      this.chkCVA.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCFHA.AutoSize = true;
      this.chkCFHA.Location = new Point(10, 58);
      this.chkCFHA.Name = "chkCFHA";
      this.chkCFHA.Size = new Size(47, 17);
      this.chkCFHA.TabIndex = 2;
      this.chkCFHA.Text = "FHA";
      this.chkCFHA.UseVisualStyleBackColor = true;
      this.chkCFHA.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCConventional.AutoSize = true;
      this.chkCConventional.Location = new Point(10, 38);
      this.chkCConventional.Name = "chkCConventional";
      this.chkCConventional.Size = new Size(88, 17);
      this.chkCConventional.TabIndex = 1;
      this.chkCConventional.Text = "Conventional";
      this.chkCConventional.UseVisualStyleBackColor = true;
      this.chkCConventional.CheckedChanged += new EventHandler(this.textField_Changed);
      this.grpCPurpose.Controls.Add((Control) this.chkCPOther);
      this.grpCPurpose.Controls.Add((Control) this.chkCConstructionPerm);
      this.grpCPurpose.Controls.Add((Control) this.chkCConstruction);
      this.grpCPurpose.Controls.Add((Control) this.chkCRefin);
      this.grpCPurpose.Controls.Add((Control) this.chkCNoRefin);
      this.grpCPurpose.Controls.Add((Control) this.chkCPurchase);
      this.grpCPurpose.HeaderForeColor = SystemColors.ControlText;
      this.grpCPurpose.Location = new Point(400, 5);
      this.grpCPurpose.Name = "grpCPurpose";
      this.grpCPurpose.Padding = new Padding(0, 0, 5, 0);
      this.grpCPurpose.Size = new Size(425, 205);
      this.grpCPurpose.TabIndex = 3;
      this.grpCPurpose.Text = "Approved Loan Purposes";
      this.chkCPOther.AutoSize = true;
      this.chkCPOther.Location = new Point(10, 138);
      this.chkCPOther.Name = "chkCPOther";
      this.chkCPOther.Size = new Size(52, 17);
      this.chkCPOther.TabIndex = 18;
      this.chkCPOther.Text = "Other";
      this.chkCPOther.UseVisualStyleBackColor = true;
      this.chkCPOther.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCConstructionPerm.AutoSize = true;
      this.chkCConstructionPerm.Location = new Point(10, 118);
      this.chkCConstructionPerm.Name = "chkCConstructionPerm";
      this.chkCConstructionPerm.Size = new Size(121, 17);
      this.chkCConstructionPerm.TabIndex = 17;
      this.chkCConstructionPerm.Text = "Construction - Perm ";
      this.chkCConstructionPerm.UseVisualStyleBackColor = true;
      this.chkCConstructionPerm.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCConstruction.AutoSize = true;
      this.chkCConstruction.Location = new Point(10, 98);
      this.chkCConstruction.Name = "chkCConstruction";
      this.chkCConstruction.Size = new Size(88, 17);
      this.chkCConstruction.TabIndex = 16;
      this.chkCConstruction.Text = "Construction ";
      this.chkCConstruction.UseVisualStyleBackColor = true;
      this.chkCConstruction.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCRefin.AutoSize = true;
      this.chkCRefin.Location = new Point(10, 78);
      this.chkCRefin.Name = "chkCRefin";
      this.chkCRefin.Size = new Size(122, 17);
      this.chkCRefin.TabIndex = 13;
      this.chkCRefin.Text = "Cash-Out Refinance";
      this.chkCRefin.UseVisualStyleBackColor = true;
      this.chkCRefin.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCNoRefin.AutoSize = true;
      this.chkCNoRefin.Location = new Point(10, 58);
      this.chkCNoRefin.Name = "chkCNoRefin";
      this.chkCNoRefin.Size = new Size(139, 17);
      this.chkCNoRefin.TabIndex = 12;
      this.chkCNoRefin.Text = "No Cash-Out Refinance";
      this.chkCNoRefin.UseVisualStyleBackColor = true;
      this.chkCNoRefin.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkCPurchase.AutoSize = true;
      this.chkCPurchase.Location = new Point(10, 38);
      this.chkCPurchase.Name = "chkCPurchase";
      this.chkCPurchase.Size = new Size(71, 17);
      this.chkCPurchase.TabIndex = 11;
      this.chkCPurchase.Text = "Purchase";
      this.chkCPurchase.UseVisualStyleBackColor = true;
      this.chkCPurchase.CheckedChanged += new EventHandler(this.textField_Changed);
      this.tabCorrNonDel.Controls.Add((Control) this.grpNDLicensingIssues);
      this.tabCorrNonDel.Controls.Add((Control) this.grpNDLoanTypes);
      this.tabCorrNonDel.Controls.Add((Control) this.grpNDPurpose);
      this.tabCorrNonDel.Location = new Point(4, 22);
      this.tabCorrNonDel.Name = "tabCorrNonDel";
      this.tabCorrNonDel.Size = new Size(834, 118);
      this.tabCorrNonDel.TabIndex = 2;
      this.tabCorrNonDel.Text = "Correspondent - Non-Delegated";
      this.tabCorrNonDel.UseVisualStyleBackColor = true;
      this.tabCorrNonDel.Resize += new EventHandler(this.tabCorrNonDel_Resize);
      this.grpNDLicensingIssues.Controls.Add((Control) this.cmbNDAllowLoansWithLicenseIssues);
      this.grpNDLicensingIssues.Controls.Add((Control) this.txtNDMsgUploadNonApproved);
      this.grpNDLicensingIssues.Controls.Add((Control) this.label10);
      this.grpNDLicensingIssues.HeaderForeColor = SystemColors.ControlText;
      this.grpNDLicensingIssues.Location = new Point(5, 115);
      this.grpNDLicensingIssues.Name = "grpNDLicensingIssues";
      this.grpNDLicensingIssues.Size = new Size(390, 96);
      this.grpNDLicensingIssues.TabIndex = 1;
      this.grpNDLicensingIssues.Text = "Policy for Unapproved Loans";
      this.cmbNDAllowLoansWithLicenseIssues.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cmbNDAllowLoansWithLicenseIssues.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbNDAllowLoansWithLicenseIssues.FormattingEnabled = true;
      this.cmbNDAllowLoansWithLicenseIssues.Items.AddRange(new object[4]
      {
        (object) "No Restrictions",
        (object) "Don't allow lock or submission",
        (object) "Don't allow loan creation",
        (object) "Don't allow all - loan creation/lock/submission"
      });
      this.cmbNDAllowLoansWithLicenseIssues.Location = new Point(136, 3);
      this.cmbNDAllowLoansWithLicenseIssues.Name = "cmbNDAllowLoansWithLicenseIssues";
      this.cmbNDAllowLoansWithLicenseIssues.Size = new Size(248, 21);
      this.cmbNDAllowLoansWithLicenseIssues.TabIndex = 12;
      this.cmbNDAllowLoansWithLicenseIssues.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cmbNDAllowLoansWithLicenseIssues.SelectionChangeCommitted += new EventHandler(this.cmb_SelectionChangeCommitted);
      this.txtNDMsgUploadNonApproved.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtNDMsgUploadNonApproved.Location = new Point(104, 30);
      this.txtNDMsgUploadNonApproved.Multiline = true;
      this.txtNDMsgUploadNonApproved.Name = "txtNDMsgUploadNonApproved";
      this.txtNDMsgUploadNonApproved.ScrollBars = ScrollBars.Vertical;
      this.txtNDMsgUploadNonApproved.Size = new Size(278, 55);
      this.txtNDMsgUploadNonApproved.TabIndex = 9;
      this.txtNDMsgUploadNonApproved.TextChanged += new EventHandler(this.textField_Changed);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(10, 33);
      this.label10.Name = "label10";
      this.label10.Size = new Size(93, 13);
      this.label10.TabIndex = 4;
      this.label10.Text = "Warning Message";
      this.grpNDLoanTypes.Controls.Add((Control) this.chkNDOther);
      this.grpNDLoanTypes.Controls.Add((Control) this.chkNDSecondLien);
      this.grpNDLoanTypes.Controls.Add((Control) this.chkNDUSDA);
      this.grpNDLoanTypes.Controls.Add((Control) this.chkNDFirstLien);
      this.grpNDLoanTypes.Controls.Add((Control) this.chkNDHELOC);
      this.grpNDLoanTypes.Controls.Add((Control) this.chkNDVA);
      this.grpNDLoanTypes.Controls.Add((Control) this.chkNDFHA);
      this.grpNDLoanTypes.Controls.Add((Control) this.chkNDConventional);
      this.grpNDLoanTypes.HeaderForeColor = SystemColors.ControlText;
      this.grpNDLoanTypes.Location = new Point(5, 5);
      this.grpNDLoanTypes.Name = "grpNDLoanTypes";
      this.grpNDLoanTypes.Size = new Size(390, 106);
      this.grpNDLoanTypes.TabIndex = 0;
      this.grpNDLoanTypes.Text = "Approved Loan Types";
      this.chkNDOther.AutoSize = true;
      this.chkNDOther.Location = new Point(140, 78);
      this.chkNDOther.Name = "chkNDOther";
      this.chkNDOther.Size = new Size(52, 17);
      this.chkNDOther.TabIndex = 10;
      this.chkNDOther.Text = "Other";
      this.chkNDOther.UseVisualStyleBackColor = true;
      this.chkNDOther.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDSecondLien.AutoSize = true;
      this.chkNDSecondLien.Location = new Point(270, 58);
      this.chkNDSecondLien.Name = "chkNDSecondLien";
      this.chkNDSecondLien.Size = new Size(86, 17);
      this.chkNDSecondLien.TabIndex = 7;
      this.chkNDSecondLien.Text = "Second Lien";
      this.chkNDSecondLien.UseVisualStyleBackColor = true;
      this.chkNDSecondLien.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDUSDA.AutoSize = true;
      this.chkNDUSDA.Location = new Point(140, 38);
      this.chkNDUSDA.Name = "chkNDUSDA";
      this.chkNDUSDA.Size = new Size(88, 17);
      this.chkNDUSDA.TabIndex = 4;
      this.chkNDUSDA.Text = "USDA - RHS";
      this.chkNDUSDA.UseVisualStyleBackColor = true;
      this.chkNDUSDA.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDFirstLien.AutoSize = true;
      this.chkNDFirstLien.Location = new Point(270, 38);
      this.chkNDFirstLien.Name = "chkNDFirstLien";
      this.chkNDFirstLien.Size = new Size(68, 17);
      this.chkNDFirstLien.TabIndex = 6;
      this.chkNDFirstLien.Text = "First Lien";
      this.chkNDFirstLien.UseVisualStyleBackColor = true;
      this.chkNDFirstLien.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDHELOC.AutoSize = true;
      this.chkNDHELOC.Location = new Point(140, 58);
      this.chkNDHELOC.Name = "chkNDHELOC";
      this.chkNDHELOC.Size = new Size(62, 17);
      this.chkNDHELOC.TabIndex = 5;
      this.chkNDHELOC.Text = "HELOC";
      this.chkNDHELOC.UseVisualStyleBackColor = true;
      this.chkNDHELOC.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDVA.AutoSize = true;
      this.chkNDVA.Location = new Point(10, 78);
      this.chkNDVA.Name = "chkNDVA";
      this.chkNDVA.Size = new Size(40, 17);
      this.chkNDVA.TabIndex = 3;
      this.chkNDVA.Text = "VA";
      this.chkNDVA.UseVisualStyleBackColor = true;
      this.chkNDVA.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDFHA.AutoSize = true;
      this.chkNDFHA.Location = new Point(10, 58);
      this.chkNDFHA.Name = "chkNDFHA";
      this.chkNDFHA.Size = new Size(47, 17);
      this.chkNDFHA.TabIndex = 2;
      this.chkNDFHA.Text = "FHA";
      this.chkNDFHA.UseVisualStyleBackColor = true;
      this.chkNDFHA.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDConventional.AutoSize = true;
      this.chkNDConventional.Location = new Point(10, 38);
      this.chkNDConventional.Name = "chkNDConventional";
      this.chkNDConventional.Size = new Size(88, 17);
      this.chkNDConventional.TabIndex = 1;
      this.chkNDConventional.Text = "Conventional";
      this.chkNDConventional.UseVisualStyleBackColor = true;
      this.chkNDConventional.CheckedChanged += new EventHandler(this.textField_Changed);
      this.grpNDPurpose.Controls.Add((Control) this.chkNDPOther);
      this.grpNDPurpose.Controls.Add((Control) this.chkNDConstructionPerm);
      this.grpNDPurpose.Controls.Add((Control) this.chkNDConstruction);
      this.grpNDPurpose.Controls.Add((Control) this.chkNDRefin);
      this.grpNDPurpose.Controls.Add((Control) this.chkNDNoRefin);
      this.grpNDPurpose.Controls.Add((Control) this.chkNDPurchase);
      this.grpNDPurpose.HeaderForeColor = SystemColors.ControlText;
      this.grpNDPurpose.Location = new Point(400, 5);
      this.grpNDPurpose.Name = "grpNDPurpose";
      this.grpNDPurpose.Padding = new Padding(0, 0, 5, 0);
      this.grpNDPurpose.Size = new Size(425, 205);
      this.grpNDPurpose.TabIndex = 2;
      this.grpNDPurpose.Text = "Approved Loan Purposes";
      this.chkNDPOther.AutoSize = true;
      this.chkNDPOther.Location = new Point(10, 138);
      this.chkNDPOther.Name = "chkNDPOther";
      this.chkNDPOther.Size = new Size(52, 17);
      this.chkNDPOther.TabIndex = 18;
      this.chkNDPOther.Text = "Other";
      this.chkNDPOther.UseVisualStyleBackColor = true;
      this.chkNDPOther.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDConstructionPerm.AutoSize = true;
      this.chkNDConstructionPerm.Location = new Point(10, 118);
      this.chkNDConstructionPerm.Name = "chkNDConstructionPerm";
      this.chkNDConstructionPerm.Size = new Size(121, 17);
      this.chkNDConstructionPerm.TabIndex = 17;
      this.chkNDConstructionPerm.Text = "Construction - Perm ";
      this.chkNDConstructionPerm.UseVisualStyleBackColor = true;
      this.chkNDConstructionPerm.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDConstruction.AutoSize = true;
      this.chkNDConstruction.Location = new Point(10, 98);
      this.chkNDConstruction.Name = "chkNDConstruction";
      this.chkNDConstruction.Size = new Size(88, 17);
      this.chkNDConstruction.TabIndex = 16;
      this.chkNDConstruction.Text = "Construction ";
      this.chkNDConstruction.UseVisualStyleBackColor = true;
      this.chkNDConstruction.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDRefin.AutoSize = true;
      this.chkNDRefin.Location = new Point(10, 78);
      this.chkNDRefin.Name = "chkNDRefin";
      this.chkNDRefin.Size = new Size(122, 17);
      this.chkNDRefin.TabIndex = 13;
      this.chkNDRefin.Text = "Cash-Out Refinance";
      this.chkNDRefin.UseVisualStyleBackColor = true;
      this.chkNDRefin.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDNoRefin.AutoSize = true;
      this.chkNDNoRefin.Location = new Point(10, 58);
      this.chkNDNoRefin.Name = "chkNDNoRefin";
      this.chkNDNoRefin.Size = new Size(139, 17);
      this.chkNDNoRefin.TabIndex = 12;
      this.chkNDNoRefin.Text = "No Cash-Out Refinance";
      this.chkNDNoRefin.UseVisualStyleBackColor = true;
      this.chkNDNoRefin.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNDPurchase.AutoSize = true;
      this.chkNDPurchase.Location = new Point(10, 38);
      this.chkNDPurchase.Name = "chkNDPurchase";
      this.chkNDPurchase.Size = new Size(71, 17);
      this.chkNDPurchase.TabIndex = 11;
      this.chkNDPurchase.Text = "Purchase";
      this.chkNDPurchase.UseVisualStyleBackColor = true;
      this.chkNDPurchase.CheckedChanged += new EventHandler(this.textField_Changed);
      this.panel1.Controls.Add((Control) this.btnSelect);
      this.panel1.Controls.Add((Control) this.textConditionCode);
      this.panel1.Controls.Add((Control) this.cmbUnderwriting);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(842, 71);
      this.panel1.TabIndex = 3;
      this.btnSelect.BackColor = Color.Transparent;
      this.btnSelect.Location = new Point(539, 7);
      this.btnSelect.MouseDownImage = (Image) null;
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(16, 16);
      this.btnSelect.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelect.TabIndex = 36;
      this.btnSelect.TabStop = false;
      this.btnSelect.Visible = false;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.textConditionCode.Location = new Point(288, 7);
      this.textConditionCode.Multiline = true;
      this.textConditionCode.Name = "textConditionCode";
      this.textConditionCode.Size = new Size(245, 56);
      this.textConditionCode.TabIndex = 35;
      this.textConditionCode.Visible = false;
      this.textConditionCode.TextChanged += new EventHandler(this.textField_Changed);
      this.cmbUnderwriting.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbUnderwriting.FormattingEnabled = true;
      this.cmbUnderwriting.Items.AddRange(new object[3]
      {
        (object) "Delegated",
        (object) "Non-Delegated",
        (object) "Delegated Conditionally"
      });
      this.cmbUnderwriting.Location = new Point(100, 22);
      this.cmbUnderwriting.Name = "cmbUnderwriting";
      this.cmbUnderwriting.Size = new Size(169, 21);
      this.cmbUnderwriting.TabIndex = 1;
      this.cmbUnderwriting.SelectedIndexChanged += new EventHandler(this.cmbUnderwriting_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(19, 25);
      this.label2.Name = "label2";
      this.label2.Size = new Size(66, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Underwriting";
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(837, 4);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 32;
      this.btnReset.TabStop = false;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(815, 4);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 31;
      this.btnSave.TabStop = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.panelHeader.Controls.Add((Control) this.label33);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(6, 31);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(850, 36);
      this.panelHeader.TabIndex = 0;
      this.label33.Location = new Point(7, 4);
      this.label33.Name = "label33";
      this.label33.Padding = new Padding(4, 0, 0, 0);
      this.label33.Size = new Size(822, 29);
      this.label33.TabIndex = 36;
      this.label33.Text = componentResourceManager.GetString("label33.Text");
      this.grpFHAVA.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpFHAVA.AutoScrollMargin = new Size(10, 0);
      this.grpFHAVA.Controls.Add((Control) this.pnlFHAVA);
      this.grpFHAVA.HeaderForeColor = SystemColors.ControlText;
      this.grpFHAVA.Location = new Point(6, 313);
      this.grpFHAVA.Name = "grpFHAVA";
      this.grpFHAVA.Padding = new Padding(5, 0, 0, 0);
      this.grpFHAVA.Size = new Size(847, 212);
      this.grpFHAVA.TabIndex = 2;
      this.grpFHAVA.Text = "GSE Information";
      this.cboAUSMethod.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboAUSMethod.FormattingEnabled = true;
      this.cboAUSMethod.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "DU",
        (object) "LP",
        (object) "DU and LP"
      });
      this.cboAUSMethod.Location = new Point(823, 98);
      this.cboAUSMethod.MinimumSize = new Size(200, 0);
      this.cboAUSMethod.Name = "cboAUSMethod";
      this.cboAUSMethod.Size = new Size(200, 21);
      this.cboAUSMethod.TabIndex = 39;
      this.cboAUSMethod.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.label20.AutoSize = true;
      this.label20.Location = new Point(718, 101);
      this.label20.Name = "label20";
      this.label20.Size = new Size(68, 13);
      this.label20.TabIndex = 74;
      this.label20.Text = "AUS Method";
      this.txtFreddieMacID.Location = new Point(823, 74);
      this.txtFreddieMacID.MaxLength = 9;
      this.txtFreddieMacID.Name = "txtFreddieMacID";
      this.txtFreddieMacID.ShortcutsEnabled = false;
      this.txtFreddieMacID.Size = new Size(200, 20);
      this.txtFreddieMacID.TabIndex = 38;
      this.txtFreddieMacID.TextChanged += new EventHandler(this.textField_Changed);
      this.txtFreddieMacID.KeyPress += new KeyPressEventHandler(this.textboxNumericOnlyValidation);
      this.label19.AutoSize = true;
      this.label19.Location = new Point(718, 77);
      this.label19.Name = "label19";
      this.label19.Size = new Size(77, 13);
      this.label19.TabIndex = 72;
      this.label19.Text = "FreddieMac ID";
      this.chkFHMLCApproved.AutoSize = true;
      this.chkFHMLCApproved.Location = new Point(823, 55);
      this.chkFHMLCApproved.Name = "chkFHMLCApproved";
      this.chkFHMLCApproved.Size = new Size(15, 14);
      this.chkFHMLCApproved.TabIndex = 37;
      this.chkFHMLCApproved.UseVisualStyleBackColor = true;
      this.chkFHMLCApproved.CheckedChanged += new EventHandler(this.textField_Changed);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(718, 55);
      this.label18.Name = "label18";
      this.label18.Size = new Size(92, 13);
      this.label18.TabIndex = 70;
      this.label18.Text = "FHMLC Approved";
      this.txtFannieMaeID.Location = new Point(823, 30);
      this.txtFannieMaeID.MaxLength = 9;
      this.txtFannieMaeID.Name = "txtFannieMaeID";
      this.txtFannieMaeID.ShortcutsEnabled = false;
      this.txtFannieMaeID.Size = new Size(200, 20);
      this.txtFannieMaeID.TabIndex = 36;
      this.txtFannieMaeID.TextChanged += new EventHandler(this.textField_Changed);
      this.txtFannieMaeID.KeyPress += new KeyPressEventHandler(this.textboxNumericOnlyValidation);
      this.label13.AutoSize = true;
      this.label13.Location = new Point(718, 33);
      this.label13.Name = "label13";
      this.label13.Size = new Size(77, 13);
      this.label13.TabIndex = 68;
      this.label13.Text = "Fannie Mae ID";
      this.chkFNMAApproved.AutoSize = true;
      this.chkFNMAApproved.Location = new Point(823, 11);
      this.chkFNMAApproved.Name = "chkFNMAApproved";
      this.chkFNMAApproved.Size = new Size(15, 14);
      this.chkFNMAApproved.TabIndex = 35;
      this.chkFNMAApproved.UseVisualStyleBackColor = true;
      this.chkFNMAApproved.CheckedChanged += new EventHandler(this.textField_Changed);
      this.label12.AutoSize = true;
      this.label12.Location = new Point(718, 11);
      this.label12.Name = "label12";
      this.label12.Size = new Size(86, 13);
      this.label12.TabIndex = 66;
      this.label12.Text = "FNMA Approved";
      this.txtVASponsorID.Location = new Point(489, 30);
      this.txtVASponsorID.MaxLength = 10;
      this.txtVASponsorID.Name = "txtVASponsorID";
      this.txtVASponsorID.ShortcutsEnabled = false;
      this.txtVASponsorID.Size = new Size(200, 20);
      this.txtVASponsorID.TabIndex = 31;
      this.txtVASponsorID.TextChanged += new EventHandler(this.textField_Changed);
      this.txtVASponsorID.KeyPress += new KeyPressEventHandler(this.textboxNumericOnlyValidation);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(373, 33);
      this.label11.Name = "label11";
      this.label11.Size = new Size(77, 13);
      this.label11.TabIndex = 64;
      this.label11.Text = "VA Sponsor ID";
      this.cboFHADirectEndorsement.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFHADirectEndorsement.FormattingEnabled = true;
      this.cboFHADirectEndorsement.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Principal/Agent",
        (object) "Sponsor"
      });
      this.cboFHADirectEndorsement.Location = new Point(134, 140);
      this.cboFHADirectEndorsement.Name = "cboFHADirectEndorsement";
      this.cboFHADirectEndorsement.Size = new Size(200, 21);
      this.cboFHADirectEndorsement.TabIndex = 29;
      this.cboFHADirectEndorsement.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(6, 143);
      this.label9.Name = "label9";
      this.label9.Size = new Size(124, 13);
      this.label9.TabIndex = 61;
      this.label9.Text = "FHA Direct Endorsement";
      this.lblPercent.AutoSize = true;
      this.lblPercent.Location = new Point(346, 77);
      this.lblPercent.Name = "lblPercent";
      this.lblPercent.Size = new Size(15, 13);
      this.lblPercent.TabIndex = 59;
      this.lblPercent.Text = "%";
      this.txtVAId.Location = new Point(489, 8);
      this.txtVAId.MaxLength = 50;
      this.txtVAId.Name = "txtVAId";
      this.txtVAId.Size = new Size(200, 20);
      this.txtVAId.TabIndex = 30;
      this.txtVAId.TextChanged += new EventHandler(this.textField_Changed);
      this.cboVAStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboVAStatus.FormattingEnabled = true;
      this.cboVAStatus.Items.AddRange(new object[8]
      {
        (object) "",
        (object) "Approved",
        (object) "Not Approved",
        (object) "Supervised Lender",
        (object) "Non-Supervised Lender",
        (object) "Non-Supervised Automatic Lender",
        (object) "Sponsoring Lender",
        (object) "Agent"
      });
      this.cboVAStatus.Location = new Point(489, 52);
      this.cboVAStatus.Name = "cboVAStatus";
      this.cboVAStatus.Size = new Size(200, 21);
      this.cboVAStatus.TabIndex = 32;
      this.cboVAStatus.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(373, 99);
      this.label3.Name = "label3";
      this.label3.Size = new Size(96, 13);
      this.label3.TabIndex = 58;
      this.label3.Text = "VA Expiration Date";
      this.dpVAExpirationDate.BackColor = SystemColors.Window;
      this.dpVAExpirationDate.Location = new Point(489, 98);
      this.dpVAExpirationDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpVAExpirationDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpVAExpirationDate.Name = "dpVAExpirationDate";
      this.dpVAExpirationDate.Size = new Size(200, 21);
      this.dpVAExpirationDate.TabIndex = 34;
      this.dpVAExpirationDate.Tag = (object) "763";
      this.dpVAExpirationDate.ToolTip = "";
      this.dpVAExpirationDate.Value = new DateTime(0L);
      this.dpVAExpirationDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.dpVAApprovedDate.BackColor = SystemColors.Window;
      this.dpVAApprovedDate.Location = new Point(489, 75);
      this.dpVAApprovedDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpVAApprovedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpVAApprovedDate.Name = "dpVAApprovedDate";
      this.dpVAApprovedDate.Size = new Size(200, 21);
      this.dpVAApprovedDate.TabIndex = 33;
      this.dpVAApprovedDate.Tag = (object) "763";
      this.dpVAApprovedDate.ToolTip = "";
      this.dpVAApprovedDate.Value = new DateTime(0L);
      this.dpVAApprovedDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(373, 11);
      this.label4.Name = "label4";
      this.label4.Size = new Size(35, 13);
      this.label4.TabIndex = 54;
      this.label4.Text = "VA ID";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(373, 77);
      this.label5.Name = "label5";
      this.label5.Size = new Size(96, 13);
      this.label5.TabIndex = 53;
      this.label5.Text = "VA Approved Date";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(373, 55);
      this.label6.Name = "label6";
      this.label6.Size = new Size(54, 13);
      this.label6.TabIndex = 51;
      this.label6.Text = "VA Status";
      this.txtFHACompRatio.Location = new Point(134, 74);
      this.txtFHACompRatio.MaxLength = 10;
      this.txtFHACompRatio.Name = "txtFHACompRatio";
      this.txtFHACompRatio.RightToLeft = RightToLeft.Yes;
      this.txtFHACompRatio.Size = new Size(200, 20);
      this.txtFHACompRatio.TabIndex = 26;
      this.txtFHACompRatio.TextChanged += new EventHandler(this.textField_Changed);
      this.txtFHACompRatio.KeyPress += new KeyPressEventHandler(this.textboxNumericOnlyValidation);
      this.txtFHASponId.Location = new Point(134, 30);
      this.txtFHASponId.MaxLength = 50;
      this.txtFHASponId.Name = "txtFHASponId";
      this.txtFHASponId.Size = new Size(199, 20);
      this.txtFHASponId.TabIndex = 24;
      this.txtFHASponId.TextChanged += new EventHandler(this.textField_Changed);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 33);
      this.label1.Name = "label1";
      this.label1.Size = new Size(84, 13);
      this.label1.TabIndex = 48;
      this.label1.Text = "FHA Sponsor ID";
      this.txtFHAId.Location = new Point(134, 8);
      this.txtFHAId.MaxLength = 50;
      this.txtFHAId.Name = "txtFHAId";
      this.txtFHAId.Size = new Size(199, 20);
      this.txtFHAId.TabIndex = 23;
      this.txtFHAId.TextChanged += new EventHandler(this.textField_Changed);
      this.cboFHAStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFHAStatus.FormattingEnabled = true;
      this.cboFHAStatus.Items.AddRange(new object[9]
      {
        (object) "",
        (object) "Approved",
        (object) "Not Approved",
        (object) "Supervised Mortgagee",
        (object) "Non-Supervised Mortgagee",
        (object) "Supervised Loan Correspondent",
        (object) "Non-Supervised Loan Correspondent",
        (object) "Investing Mortgagee",
        (object) "Governmental Institution"
      });
      this.cboFHAStatus.Location = new Point(134, 52);
      this.cboFHAStatus.Name = "cboFHAStatus";
      this.cboFHAStatus.Size = new Size(200, 21);
      this.cboFHAStatus.TabIndex = 25;
      this.cboFHAStatus.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.label40.AutoSize = true;
      this.label40.Location = new Point(6, 121);
      this.label40.Name = "label40";
      this.label40.Size = new Size(103, 13);
      this.label40.TabIndex = 46;
      this.label40.Text = "FHA Expiration Date";
      this.dpFHAExpDate.BackColor = SystemColors.Window;
      this.dpFHAExpDate.Location = new Point(134, 117);
      this.dpFHAExpDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpFHAExpDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpFHAExpDate.Name = "dpFHAExpDate";
      this.dpFHAExpDate.Size = new Size(199, 21);
      this.dpFHAExpDate.TabIndex = 28;
      this.dpFHAExpDate.Tag = (object) "763";
      this.dpFHAExpDate.ToolTip = "";
      this.dpFHAExpDate.Value = new DateTime(0L);
      this.dpFHAExpDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.dpFHAApprovedDate.BackColor = SystemColors.Window;
      this.dpFHAApprovedDate.Location = new Point(134, 95);
      this.dpFHAApprovedDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpFHAApprovedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpFHAApprovedDate.Name = "dpFHAApprovedDate";
      this.dpFHAApprovedDate.Size = new Size(199, 21);
      this.dpFHAApprovedDate.TabIndex = 27;
      this.dpFHAApprovedDate.Tag = (object) "763";
      this.dpFHAApprovedDate.ToolTip = "";
      this.dpFHAApprovedDate.Value = new DateTime(0L);
      this.dpFHAApprovedDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(6, 11);
      this.label14.Name = "label14";
      this.label14.Size = new Size(42, 13);
      this.label14.TabIndex = 28;
      this.label14.Text = "FHA ID";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(6, 99);
      this.label15.Name = "label15";
      this.label15.Size = new Size(103, 13);
      this.label15.TabIndex = 26;
      this.label15.Text = "FHA Approved Date";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(6, 55);
      this.label16.Name = "label16";
      this.label16.Size = new Size(61, 13);
      this.label16.TabIndex = 22;
      this.label16.Text = "FHA Status";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(6, 77);
      this.label17.Name = "label17";
      this.label17.Size = new Size(101, 13);
      this.label17.TabIndex = 24;
      this.label17.Text = "FHA Compare Ratio";
      this.chkUseParentInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfo.AutoSize = true;
      this.chkUseParentInfo.BackColor = Color.Transparent;
      this.chkUseParentInfo.Location = new Point(693, 4);
      this.chkUseParentInfo.Name = "chkUseParentInfo";
      this.chkUseParentInfo.Size = new Size(100, 17);
      this.chkUseParentInfo.TabIndex = 0;
      this.chkUseParentInfo.Text = "Use Parent Info";
      this.chkUseParentInfo.UseVisualStyleBackColor = false;
      this.chkUseParentInfo.CheckedChanged += new EventHandler(this.chkUseParentInfo_CheckedChanged);
      this.pnlFHAVA.AutoScroll = true;
      this.pnlFHAVA.Controls.Add((Control) this.lblPadding);
      this.pnlFHAVA.Controls.Add((Control) this.label14);
      this.pnlFHAVA.Controls.Add((Control) this.cboAUSMethod);
      this.pnlFHAVA.Controls.Add((Control) this.label17);
      this.pnlFHAVA.Controls.Add((Control) this.label20);
      this.pnlFHAVA.Controls.Add((Control) this.label16);
      this.pnlFHAVA.Controls.Add((Control) this.txtFreddieMacID);
      this.pnlFHAVA.Controls.Add((Control) this.label15);
      this.pnlFHAVA.Controls.Add((Control) this.label19);
      this.pnlFHAVA.Controls.Add((Control) this.dpFHAApprovedDate);
      this.pnlFHAVA.Controls.Add((Control) this.chkFHMLCApproved);
      this.pnlFHAVA.Controls.Add((Control) this.dpFHAExpDate);
      this.pnlFHAVA.Controls.Add((Control) this.label18);
      this.pnlFHAVA.Controls.Add((Control) this.label40);
      this.pnlFHAVA.Controls.Add((Control) this.txtFannieMaeID);
      this.pnlFHAVA.Controls.Add((Control) this.cboFHAStatus);
      this.pnlFHAVA.Controls.Add((Control) this.label13);
      this.pnlFHAVA.Controls.Add((Control) this.txtFHAId);
      this.pnlFHAVA.Controls.Add((Control) this.chkFNMAApproved);
      this.pnlFHAVA.Controls.Add((Control) this.label1);
      this.pnlFHAVA.Controls.Add((Control) this.label12);
      this.pnlFHAVA.Controls.Add((Control) this.txtFHASponId);
      this.pnlFHAVA.Controls.Add((Control) this.txtVASponsorID);
      this.pnlFHAVA.Controls.Add((Control) this.txtFHACompRatio);
      this.pnlFHAVA.Controls.Add((Control) this.label11);
      this.pnlFHAVA.Controls.Add((Control) this.label6);
      this.pnlFHAVA.Controls.Add((Control) this.cboFHADirectEndorsement);
      this.pnlFHAVA.Controls.Add((Control) this.label5);
      this.pnlFHAVA.Controls.Add((Control) this.label9);
      this.pnlFHAVA.Controls.Add((Control) this.label4);
      this.pnlFHAVA.Controls.Add((Control) this.lblPercent);
      this.pnlFHAVA.Controls.Add((Control) this.dpVAApprovedDate);
      this.pnlFHAVA.Controls.Add((Control) this.txtVAId);
      this.pnlFHAVA.Controls.Add((Control) this.dpVAExpirationDate);
      this.pnlFHAVA.Controls.Add((Control) this.cboVAStatus);
      this.pnlFHAVA.Controls.Add((Control) this.label3);
      this.pnlFHAVA.Dock = DockStyle.Fill;
      this.pnlFHAVA.Location = new Point(6, 26);
      this.pnlFHAVA.Name = "pnlFHAVA";
      this.pnlFHAVA.Size = new Size(840, 185);
      this.pnlFHAVA.TabIndex = 33;
      this.lblPadding.BackColor = Color.Transparent;
      this.lblPadding.Location = new Point(1018, 12);
      this.lblPadding.Name = "lblPadding";
      this.lblPadding.Size = new Size(15, 13);
      this.lblPadding.TabIndex = 75;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpAll);
      this.Margin = new Padding(0);
      this.Name = nameof (EditCompanyLoanTypeControl);
      this.Padding = new Padding(5);
      this.Size = new Size(872, 620);
      this.grpAll.ResumeLayout(false);
      this.grpAll.PerformLayout();
      this.tabLoanTypes.ResumeLayout(false);
      this.tabBroker.ResumeLayout(false);
      this.grpBLicensingIssues.ResumeLayout(false);
      this.grpBLicensingIssues.PerformLayout();
      this.grpBLoanTypes.ResumeLayout(false);
      this.grpBLoanTypes.PerformLayout();
      this.grpBPurpose.ResumeLayout(false);
      this.grpBPurpose.PerformLayout();
      this.tabCorres.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabCorrDel.ResumeLayout(false);
      this.grpCLicensingIssues.ResumeLayout(false);
      this.grpCLicensingIssues.PerformLayout();
      this.grpCLoanTypes.ResumeLayout(false);
      this.grpCLoanTypes.PerformLayout();
      this.grpCPurpose.ResumeLayout(false);
      this.grpCPurpose.PerformLayout();
      this.tabCorrNonDel.ResumeLayout(false);
      this.grpNDLicensingIssues.ResumeLayout(false);
      this.grpNDLicensingIssues.PerformLayout();
      this.grpNDLoanTypes.ResumeLayout(false);
      this.grpNDLoanTypes.PerformLayout();
      this.grpNDPurpose.ResumeLayout(false);
      this.grpNDPurpose.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.btnSelect).EndInit();
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.panelHeader.ResumeLayout(false);
      this.grpFHAVA.ResumeLayout(false);
      this.pnlFHAVA.ResumeLayout(false);
      this.pnlFHAVA.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
