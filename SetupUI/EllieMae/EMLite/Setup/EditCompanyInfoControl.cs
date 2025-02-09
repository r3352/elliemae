// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditCompanyInfoControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditCompanyInfoControl : UserControl
  {
    private Sessions.Session session;
    private SessionObjects sessionObjects;
    public bool edit;
    private int compType;
    private ExternalOriginatorManagementData externalContact;
    private bool forLender;
    private string originalZipCode = string.Empty;
    private InputEventHelper inputEventHelper;
    private int oid = -1;
    private int parent;
    private ExternalOriginatorManagementData parentContact;
    private List<long> allTPOId = new List<long>();
    private Dictionary<string, List<ExternalSettingValue>> settingsList;
    public string PrimarySalesRepUserId;
    public string DefaultPrimarySalesRepUserId;
    public string PrimarySalesRepName;
    public string DefaultPrimarySalesRepName;
    public DateTime DefaultPrimarySalesRepAssignedDate = DateTime.MinValue;
    public bool IsPrimarySalesRepChanged;
    public bool IsPrimaryAeAssignedDateChanged;
    private IOrganizationManager rOrg;
    private UserInfo[] allInternalUsers;
    private Hashtable orgLookup;
    private List<ExternalUserInfo> managers;
    private bool hasAccess = true;
    private bool tpoMVPSiteExists;
    private bool useAutoOrgID;
    private int cmbOrgIndex = -1;
    private IContainer components;
    private Panel pnlBody;
    private Panel pnlLeft;
    private GroupContainer gcOranization;
    private Label label52;
    private TextBox txtOrgName;
    private Label lblOrganizationName;
    private GroupContainer gcProductPricingInfo;
    private ComboBox cboEPPSCompModel;
    private CheckBox chkUseParentInfoForEPPS;
    private TextBox txtEPPSUserName;
    private Label lblEPPSUserName;
    private Label lblEPPSCompModel;
    private Panel pnlPML;
    private TextBox txtPMLCustomerCode;
    private Label lblPMLCustomerCode;
    private TextBox txtPMLPassword;
    private Label lblPMLPassword;
    private TextBox txtPMLUserName;
    private Label lblPMLUserName;
    private GroupContainer gcCompanyDetails;
    private Label label50;
    private Label label49;
    private Label label48;
    private Label label47;
    private Label label46;
    private Label label45;
    private Label label44;
    private CheckBox chkDisable;
    private ComboBox cboStateAddr;
    private ComboBox cmbOrgType;
    private Label label39;
    private TextBox txtLastLoanSubmit;
    private Label label9;
    private ComboBox cboManager;
    private Label label8;
    private TextBox txtWebsite;
    private TextBox txtEmail;
    private Label label6;
    private Label label7;
    private TextBox txtFax;
    private TextBox txtPhone;
    private Label label4;
    private Label label5;
    private TextBox txtOwnerName;
    private Label label3;
    private TextBox txtOrgID;
    private Label label2;
    private CheckBox chkUseParentInfoForCompany;
    private TextBox txtZip;
    private CheckBox chkBroker;
    private TextBox txtID;
    private Label lblAddress;
    private TextBox txtCity;
    private TextBox txtName;
    private Label lblCity;
    private Label lblOriginatorName;
    private TextBox txtAddress;
    private Label lblState;
    private Label lblName;
    private Label lblZip;
    private Label lblID;
    private CheckBox chkCorrespondent;
    private GroupContainer gcRateSheetLockInfo;
    private CheckBox chkUseParentInfoForRate;
    private TextBox txtEmailRateSheet;
    private Label label10;
    private Label label11;
    private TextBox txtEmailLockInfo;
    private TextBox txtFaxRateSheet;
    private TextBox txtFaxLockInfo;
    private Label label12;
    private Label label13;
    private Panel pnlRight;
    private Panel pnlRightTop;
    private GroupContainer gcSalesRepInfo;
    private Label label51;
    private StandardIconButton btnAddRep;
    private TextBox txtNameSalesRep;
    private Label label38;
    private Label label41;
    private TextBox txtPhoneSalesRep;
    private TextBox txtPersonaSalesRep;
    private TextBox txtEmailSalesRep;
    private Label label42;
    private Label label43;
    private GroupContainer gcApprovalStatus;
    private ComboBox cboCompanyRating;
    private Label label40;
    private CheckBox chkAddWatchList;
    private DatePicker dpApplicationDate;
    private DatePicker dpApprovedDate;
    private DatePicker dpActClosingDate;
    private ComboBox cboCurrentStatus;
    private CheckBox chkUseParentInfoForApproval;
    private Label label14;
    private Label label15;
    private Label label16;
    private Label label17;
    private Panel pnlRightBottom;
    private GroupContainer gcBusinessInfo;
    private ComboBox cboTypeofEntity;
    private TextBox txtEOCompany;
    private Label label30;
    private DatePicker dpEOExpirationDate;
    private Label label29;
    private ComboBox cboCloseInOwnName;
    private ComboBox cboFundInOwnName;
    private ComboBox cboDUSponsored;
    private Label label35;
    private Label label36;
    private Label label37;
    private TextBox txtMERSOrgID;
    private TextBox txtEOPolicy;
    private Label label31;
    private Label label32;
    private TextBox txtCompanyNetWorth;
    private Label label34;
    private DatePicker dpFinancialLastUpdate;
    private Label label28;
    private Label label27;
    private TextBox txtFinancialPeriod;
    private Label label26;
    private TextBox txtNMLSID;
    private Label label53;
    private TextBox txtLEI;
    private CheckBox chkUseSSN;
    private Label label25;
    private TextBox txtTaxID;
    private DatePicker dpDateincorporation;
    private ComboBox cboStateIncorp;
    private CheckBox chkIncorporated;
    private CheckBox chkUseParentInfoForBusiness;
    private Label label18;
    private Label label19;
    private TextBox txtEntityDescription;
    private Label label20;
    private Label label21;
    private StandardIconButton btnSave;
    private Panel panelHeader;
    private Label label33;
    private StandardIconButton btnReset;
    private GroupContainer grpAll;
    private ComboBox cboRateSheet;
    private Label lblRateSheet;
    private ComboBox cboPriceGroupBroker;
    private Label lblPriceGroupBroker;
    private CheckBox chkVisibleOnTPOWCsite;
    private CheckBox chkDelegated;
    private CheckBox chkNonDelegated;
    private TextBox txtLPAPassword;
    private TextBox txtTPONumber;
    private ComboBox cboLPASponsored;
    private Label label24;
    private Label label23;
    private Label label1;
    private ComboBox cboGenerateDisclosures;
    private CheckBox chkMultiFactorAuth;
    private ComboBox cboPriceGroupCorNonDel;
    private Label lblPriceGroupCorNonDel;
    private ComboBox cboPriceGroupCorDel;
    private Label lblPriceGroupCorDel;
    private CheckBox chkNoAfterHourWires;
    private Label label22;
    private Label lblAcceptFirstPayments;
    private CheckBox chkAcceptFirstPayments;
    private Label label54;
    private DatePicker dpAeAssignedDate;
    private Label label55;
    private ComboBox ccbTimezone;
    private TextBox txtYrsInBusiness;
    private Label label56;
    private ComboBox cboBillState;
    private TextBox txtBillCity;
    private TextBox txtBillAddress;
    private Label label62;
    private Label label61;
    private Label label59;
    private Label label57;
    private TextBox txtBillZip;
    private CheckBox chkCopyCompanyAddress;

    public event EventHandler SaveButton_Clicked;

    public event EventHandler Broker_Checked;

    public EditCompanyInfoControl(
      Sessions.Session session,
      int oid,
      bool forLender,
      int parent,
      bool edit)
    {
      this.session = session;
      this.forLender = forLender;
      this.edit = edit;
      this.oid = oid;
      this.parent = parent;
      this.tpoMVPSiteExists = session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      this.parentContact = this.session.ConfigurationManager.GetExternalOrganization(this.forLender, parent);
      this.allTPOId = this.session.ConfigurationManager.GetAllTpoID();
      this.settingsList = this.session.ConfigurationManager.GetExternalOrgSettings();
      this.managers = !edit ? this.session.ConfigurationManager.GetAllCompanyManagers(parent) : this.session.ConfigurationManager.GetAllCompanyManagers(oid);
      this.InitializeComponent();
      this.RefreshPrimarySalesInfo(this.oid, this.parent);
      this.Dock = DockStyle.Fill;
      this.cboStateIncorp.Items.AddRange((object[]) Utils.GetStates());
      this.cboStateAddr.Items.AddRange((object[]) Utils.GetStates());
      this.cboBillState.Items.AddRange((object[]) Utils.GetStatesWithBlankEntry());
      if (!this.settingsList.ContainsKey("Company Rating") || this.settingsList["Company Rating"] == null)
        this.settingsList["Company Rating"] = new List<ExternalSettingValue>();
      this.settingsList["Company Rating"].Insert(0, new ExternalSettingValue(-1, -1, "", "", 0));
      if (!this.settingsList.ContainsKey("Current Company Status") || this.settingsList["Current Company Status"] == null)
        this.settingsList["Current Company Status"] = new List<ExternalSettingValue>();
      this.settingsList["Current Company Status"].Insert(0, new ExternalSettingValue(-1, -1, "", "", 0));
      if (!this.settingsList.ContainsKey("Price Group") || this.settingsList["Price Group"] == null)
        this.settingsList["Price Group"] = new List<ExternalSettingValue>();
      this.settingsList["Price Group"].Insert(0, new ExternalSettingValue(-1, -1, "", "", 0));
      if (!this.settingsList.ContainsKey("ICE PPE Rate Sheet") || this.settingsList["ICE PPE Rate Sheet"] == null)
        this.settingsList["ICE PPE Rate Sheet"] = new List<ExternalSettingValue>();
      this.settingsList["ICE PPE Rate Sheet"].Insert(0, new ExternalSettingValue(-1, -1, "", "", 0));
      this.cboCompanyRating.DataSource = (object) this.settingsList["Company Rating"];
      this.cboCompanyRating.DisplayMember = "settingValue";
      this.cboCompanyRating.ValueMember = "settingId";
      this.cboCurrentStatus.DataSource = (object) this.settingsList["Current Company Status"];
      this.cboCurrentStatus.DisplayMember = "settingValue";
      this.cboCurrentStatus.ValueMember = "settingId";
      this.cboCurrentStatus.SelectedIndex = -1;
      this.cboPriceGroupBroker.DataSource = (object) new List<ExternalSettingValue>((IEnumerable<ExternalSettingValue>) this.settingsList["Price Group"]);
      this.cboPriceGroupBroker.DisplayMember = "CodeValueConcat";
      this.cboPriceGroupBroker.ValueMember = "settingId";
      this.cboPriceGroupCorDel.DataSource = (object) new List<ExternalSettingValue>((IEnumerable<ExternalSettingValue>) this.settingsList["Price Group"]);
      this.cboPriceGroupCorDel.DisplayMember = "CodeValueConcat";
      this.cboPriceGroupCorDel.ValueMember = "settingId";
      this.cboPriceGroupCorNonDel.DataSource = (object) new List<ExternalSettingValue>((IEnumerable<ExternalSettingValue>) this.settingsList["Price Group"]);
      this.cboPriceGroupCorNonDel.DisplayMember = "CodeValueConcat";
      this.cboPriceGroupCorNonDel.ValueMember = "settingId";
      this.cboRateSheet.DataSource = (object) this.settingsList["ICE PPE Rate Sheet"];
      this.cboRateSheet.DisplayMember = "settingValue";
      this.cboRateSheet.ValueMember = "settingId";
      this.externalContact = this.session.ConfigurationManager.GetByoid(this.forLender, oid);
      this.inputEventHelper = new InputEventHelper();
      this.cboManager.Items.Add((object) "");
      this.managers.ForEach((Action<ExternalUserInfo>) (item => this.cboManager.Items.Add((object) (item.FirstName + " " + item.LastName))));
      this.populateOrganizationType();
      this.cboLPASponsored.SelectedIndex = 0;
      if (this.parent == 0)
      {
        this.useAutoOrgID = string.Equals(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "EnableAutoOrgIdNumbering"), "true", StringComparison.CurrentCultureIgnoreCase);
        if (this.useAutoOrgID)
        {
          this.txtOrgID.Enabled = false;
          this.txtOrgID.Text = "";
        }
      }
      if (this.edit)
        this.populateFields();
      else if (this.parentContact != null)
      {
        this.updateUseParentInfo("checked", true);
        this.inheritParentInfo();
      }
      else
      {
        this.updateUseParentInfo("checked", false);
        this.updateUseParentInfo("enabled", false);
      }
      this.initFieldEvents();
      if (this.externalContact == null)
      {
        this.btnReset.Enabled = false;
        this.btnSave.Enabled = true;
      }
      if (this.parent == 0 && !this.edit)
      {
        UserInfo salesRep = Session.UserInfo;
        this.loadAllInternalUsers();
        if (this.allInternalUsers != null && this.allInternalUsers.Length != 0 && new List<UserInfo>((IEnumerable<UserInfo>) this.allInternalUsers).FirstOrDefault<UserInfo>((Func<UserInfo, bool>) (e1 => e1.Userid.Equals(salesRep.Userid))) != (UserInfo) null)
        {
          this.PrimarySalesRepUserId = salesRep.Userid;
          this.PrimarySalesRepName = salesRep.FullName;
          this.populateSalesRepInfo(salesRep);
          this.IsPrimarySalesRepChanged = true;
          this.DefaultPrimarySalesRepUserId = salesRep.Userid;
          this.SetButtonStatus(true);
        }
      }
      this.updateCopyCompanyAddressCheckbox();
      this.initProductPricingSection();
    }

    public bool HasAccess
    {
      get => this.hasAccess;
      set => this.hasAccess = value;
    }

    private void initProductPricingSection()
    {
      List<ProductPricingSetting> productPricingSettings = this.session.ConfigurationManager.GetProductPricingSettings();
      ProductPricingSetting productPricingSetting1 = (ProductPricingSetting) null;
      foreach (ProductPricingSetting productPricingSetting2 in productPricingSettings)
      {
        if (productPricingSetting2.Active)
          productPricingSetting1 = productPricingSetting2;
      }
      if (productPricingSetting1 != null && productPricingSetting1.PartnerID == "PRICEMYLOAN")
      {
        this.pnlPML.Visible = true;
        this.txtEPPSUserName.Visible = false;
        this.cboEPPSCompModel.Visible = false;
        this.cboRateSheet.Visible = false;
        this.lblEPPSUserName.Visible = false;
        this.lblEPPSCompModel.Visible = false;
        this.lblRateSheet.Visible = false;
      }
      else if (productPricingSetting1 != null && (productPricingSetting1.PartnerID == "OPTIMALBLUE" || productPricingSetting1.PartnerID == "OPTIMALBLUE_NEW"))
      {
        this.pnlPML.Visible = false;
        this.txtEPPSUserName.Visible = false;
        this.cboEPPSCompModel.Visible = false;
        this.lblEPPSUserName.Visible = false;
        this.lblEPPSCompModel.Visible = false;
        this.lblRateSheet.Visible = false;
        this.cboRateSheet.Visible = false;
        this.lblPriceGroupBroker.Visible = true;
        this.cboPriceGroupBroker.Visible = true;
        this.lblPriceGroupCorDel.Visible = true;
        this.cboPriceGroupCorDel.Visible = true;
        this.lblPriceGroupCorNonDel.Visible = true;
        this.cboPriceGroupCorNonDel.Visible = true;
      }
      else if (productPricingSetting1 != null && productPricingSetting1.IsEPPS)
      {
        this.pnlPML.Visible = false;
        this.txtEPPSUserName.Visible = true;
        this.cboEPPSCompModel.Visible = true;
        this.lblEPPSUserName.Visible = true;
        this.lblEPPSCompModel.Visible = true;
      }
      else
      {
        this.pnlPML.Visible = false;
        this.txtEPPSUserName.Visible = false;
        this.cboEPPSCompModel.Visible = false;
        this.lblEPPSUserName.Visible = false;
        this.lblEPPSCompModel.Visible = false;
        this.cboRateSheet.Visible = false;
        this.lblRateSheet.Visible = false;
      }
    }

    public void RefreshPrimarySalesInfo(int oid, int parent)
    {
      ExternalOriginatorManagementData externalOrganization;
      if (this.edit)
      {
        externalOrganization = this.session.ConfigurationManager.GetExternalOrganization(false, oid);
      }
      else
      {
        externalOrganization = this.session.ConfigurationManager.GetExternalOrganization(false, parent);
        this.IsPrimarySalesRepChanged = true;
      }
      if (externalOrganization == null)
        return;
      this.PrimarySalesRepUserId = this.DefaultPrimarySalesRepUserId = externalOrganization.PrimarySalesRepUserId;
      DateTime salesRepAssignedDate = externalOrganization.PrimarySalesRepAssignedDate;
      this.DefaultPrimarySalesRepAssignedDate = this.dpAeAssignedDate.Value = externalOrganization.PrimarySalesRepAssignedDate;
      if (string.IsNullOrWhiteSpace(externalOrganization.PrimarySalesRepUserId))
        return;
      UserInfo user = this.session.OrganizationManager.GetUser(externalOrganization.PrimarySalesRepUserId);
      this.populateSalesRepInfo(user);
      if (!(user != (UserInfo) null))
        return;
      this.PrimarySalesRepName = user.FullName;
      this.DefaultPrimarySalesRepName = user.FullName;
    }

    public EditCompanyInfoControl(
      SessionObjects sessionObjects,
      int oid,
      bool forLender,
      int parent,
      bool edit)
    {
      this.sessionObjects = sessionObjects;
      this.forLender = forLender;
      this.edit = edit;
      this.oid = oid;
      this.parent = parent;
      this.parentContact = this.sessionObjects.ConfigurationManager.GetExternalOrganization(this.forLender, parent);
      this.allTPOId = this.sessionObjects.ConfigurationManager.GetAllTpoID();
      this.externalContact = this.sessionObjects.ConfigurationManager.GetByoid(this.forLender, oid);
      if (this.externalContact == null)
        this.externalContact = new ExternalOriginatorManagementData();
      this.managers = !edit ? this.sessionObjects.ConfigurationManager.GetAllCompanyManagers(parent) : this.sessionObjects.ConfigurationManager.GetAllCompanyManagers(oid);
      this.InitializeComponent();
    }

    private void populateSalesRepInfo(UserInfo salesRepInfo)
    {
      if (salesRepInfo != (UserInfo) null)
      {
        this.txtNameSalesRep.Text = salesRepInfo.FullName;
        this.txtPersonaSalesRep.Text = Persona.ToString(salesRepInfo.UserPersonas);
        this.txtEmailSalesRep.Text = salesRepInfo.Email;
        this.txtPhoneSalesRep.Text = salesRepInfo.Phone;
      }
      else
      {
        this.txtNameSalesRep.Text = string.Empty;
        this.txtPersonaSalesRep.Text = string.Empty;
        this.txtEmailSalesRep.Text = string.Empty;
        this.txtPhoneSalesRep.Text = string.Empty;
      }
    }

    private void initFieldEvents()
    {
      this.inputEventHelper.AddZipcodeFieldHelper(this.txtZip, this.txtCity, this.cboStateAddr);
      this.inputEventHelper.AddZipcodeFieldHelper(this.txtBillZip, this.txtBillCity, this.cboBillState);
      this.inputEventHelper.AddPhoneFieldHelper(this.txtPhone);
      this.inputEventHelper.AddPhoneFieldHelper(this.txtFax);
      this.inputEventHelper.AddPhoneFieldHelper(this.txtFaxRateSheet);
      this.inputEventHelper.AddPhoneFieldHelper(this.txtFaxLockInfo);
      this.inputEventHelper.AddNumericFieldHelper(this.txtCompanyNetWorth);
      if (this.chkUseSSN.Checked)
        this.inputEventHelper.AddSSNFieldHelper(this.txtTaxID);
      this.chkUseSSN_CheckedChanged((object) null, (EventArgs) null);
      this.cboTypeofEntity_SelectedIndexChanged((object) null, (EventArgs) null);
      this.SetButtonStatus(false);
    }

    private void populateFields()
    {
      this.compType = (int) this.externalContact.entityType;
      if (this.externalContact.entityType == ExternalOriginatorEntityType.Both)
      {
        this.chkBroker.Checked = true;
        this.chkCorrespondent.Checked = true;
      }
      else if (this.externalContact.entityType == ExternalOriginatorEntityType.Broker)
      {
        this.chkBroker.Checked = true;
        this.chkCorrespondent.Checked = false;
      }
      else if (this.externalContact.entityType == ExternalOriginatorEntityType.Correspondent)
      {
        this.chkBroker.Checked = false;
        this.chkCorrespondent.Checked = true;
      }
      else if (this.externalContact.entityType == ExternalOriginatorEntityType.None)
      {
        this.chkBroker.Checked = false;
        this.chkCorrespondent.Checked = false;
      }
      this.chkDelegated.Enabled = this.chkNonDelegated.Enabled = this.chkCorrespondent.Checked;
      if (this.externalContact.UnderwritingType == ExternalOriginatorUnderwritingType.Both)
      {
        this.chkDelegated.Checked = true;
        this.chkNonDelegated.Checked = true;
      }
      else if (this.externalContact.UnderwritingType == ExternalOriginatorUnderwritingType.Delegated)
      {
        this.chkDelegated.Checked = true;
        this.chkNonDelegated.Checked = false;
      }
      else if (this.externalContact.UnderwritingType == ExternalOriginatorUnderwritingType.NonDelegated)
      {
        this.chkDelegated.Checked = false;
        this.chkNonDelegated.Checked = true;
      }
      else if (this.externalContact.UnderwritingType == ExternalOriginatorUnderwritingType.None)
      {
        this.chkDelegated.Checked = false;
        this.chkNonDelegated.Checked = false;
      }
      this.chkNoAfterHourWires.Checked = this.externalContact.NoAfterHourWires;
      this.ccbTimezone.SelectedItem = !(this.externalContact.Timezone == "") ? (object) this.externalContact.Timezone : (object) null;
      this.cboGenerateDisclosures.Text = Helper.GetDescription((Enum) this.externalContact.GenerateDisclosures);
      this.txtName.Text = this.externalContact.CompanyLegalName;
      this.txtOrgName.Text = this.externalContact.OrganizationName;
      CheckBox visibleOnTpowCsite = this.chkVisibleOnTPOWCsite;
      bool? visibleOnTpowcSite = this.externalContact.VisibleOnTPOWCSite;
      bool flag = true;
      int num = visibleOnTpowcSite.GetValueOrDefault() == flag & visibleOnTpowcSite.HasValue ? 1 : 0;
      visibleOnTpowCsite.Checked = num != 0;
      this.chkMultiFactorAuth.Checked = this.externalContact.MultiFactorAuthentication;
      this.txtAddress.Text = this.externalContact.Address;
      this.txtCity.Text = this.externalContact.City;
      if (this.externalContact.State == "")
        this.cboStateAddr.SelectedIndex = -1;
      else
        this.cboStateAddr.Text = this.externalContact.State;
      this.txtZip.Text = this.externalContact.Zip;
      this.txtOrgID.Text = this.externalContact.OrgID;
      this.txtBillAddress.Text = this.externalContact.BillingAddress;
      this.txtBillCity.Text = this.externalContact.BillingCity;
      if (this.externalContact.BillingState == "")
        this.cboBillState.SelectedIndex = -1;
      else
        this.cboBillState.Text = this.externalContact.BillingState;
      this.txtBillZip.Text = this.externalContact.BillingZip;
      this.txtOwnerName.Text = this.externalContact.OwnerName;
      this.txtPhone.Text = this.externalContact.PhoneNumber;
      this.txtFax.Text = this.externalContact.FaxNumber;
      this.txtEmail.Text = this.externalContact.Email;
      this.txtWebsite.Text = this.externalContact.Website;
      if (this.externalContact.LastLoanSubmitted != DateTime.MinValue)
        this.txtLastLoanSubmit.Text = this.externalContact.LastLoanSubmitted.ToString();
      else
        this.txtLastLoanSubmit.Text = "";
      if (this.externalContact.Manager != "")
        this.cboManager.SelectedItem = (object) this.getManagerName(this.externalContact.Manager);
      else
        this.cboManager.SelectedIndex = -1;
      this.txtEmailRateSheet.Text = this.externalContact.EmailForRateSheet;
      this.txtFaxRateSheet.Text = this.externalContact.FaxForRateSheet;
      this.txtEmailLockInfo.Text = this.externalContact.EmailForLockInfo;
      this.txtFaxLockInfo.Text = this.externalContact.FaxForLockInfo;
      this.txtEPPSUserName.Text = this.externalContact.EPPSUserName;
      if (this.externalContact.EPPSCompModel != null && this.externalContact.EPPSCompModel != "")
        this.cboEPPSCompModel.SelectedIndex = Convert.ToInt32(this.externalContact.EPPSCompModel);
      else
        this.cboEPPSCompModel.SelectedIndex = -1;
      this.txtPMLUserName.Text = this.externalContact.PMLUserName;
      this.txtPMLPassword.Text = this.externalContact.PMLPassword;
      this.txtPMLCustomerCode.Text = this.externalContact.PMLCustomerCode;
      this.cboCurrentStatus.SelectedValue = (object) this.externalContact.CurrentStatus;
      this.chkAddWatchList.Checked = this.externalContact.AddToWatchlist;
      this.dpActClosingDate.Value = !(this.externalContact.CurrentStatusDate != DateTime.MinValue) ? new DateTime() : this.externalContact.CurrentStatusDate;
      this.dpApprovedDate.Value = !(this.externalContact.ApprovedDate != DateTime.MinValue) ? new DateTime() : this.externalContact.ApprovedDate;
      this.dpApplicationDate.Value = !(this.externalContact.ApplicationDate != DateTime.MinValue) ? new DateTime() : this.externalContact.ApplicationDate;
      this.cboCompanyRating.SelectedValue = (object) this.externalContact.CompanyRating;
      this.chkIncorporated.Checked = this.externalContact.Incorporated;
      if (this.externalContact.StateIncorp != "")
        this.cboStateIncorp.SelectedItem = (object) this.externalContact.StateIncorp;
      else
        this.cboStateIncorp.SelectedIndex = -1;
      this.dpDateincorporation.Value = !(this.externalContact.DateOfIncorporation != DateTime.MinValue) ? new DateTime() : this.externalContact.DateOfIncorporation;
      this.txtYrsInBusiness.Text = this.externalContact.YrsInBusiness;
      this.cboTypeofEntity.SelectedIndex = this.externalContact.TypeOfEntity;
      this.txtEntityDescription.Enabled = this.cboTypeofEntity.SelectedIndex == 6;
      this.txtEntityDescription.ReadOnly = this.cboTypeofEntity.SelectedIndex != 6;
      this.txtEntityDescription.Text = this.externalContact.OtherEntityDescription;
      this.txtTaxID.Text = this.externalContact.TaxID;
      this.chkUseSSN.Checked = this.externalContact.UseSSNFormat;
      this.txtNMLSID.Text = this.externalContact.NmlsId;
      this.txtFinancialPeriod.Text = this.externalContact.FinancialsPeriod;
      this.dpFinancialLastUpdate.Value = !(this.externalContact.FinancialsLastUpdate != DateTime.MinValue) ? new DateTime() : this.externalContact.FinancialsLastUpdate;
      this.txtCompanyNetWorth.Text = Convert.ToDecimal((object) this.externalContact.CompanyNetWorth).ToString("#,#", (IFormatProvider) CultureInfo.InvariantCulture);
      this.dpEOExpirationDate.Value = !(this.externalContact.EOExpirationDate != DateTime.MinValue) ? new DateTime() : this.externalContact.EOExpirationDate;
      this.cboRateSheet.SelectedItem = (object) this.settingsList["ICE PPE Rate Sheet"].FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId.ToString() == this.externalContact.EPPSRateSheet));
      this.txtEOCompany.Text = this.externalContact.EOCompany;
      this.txtEOPolicy.Text = this.externalContact.EOPolicyNumber;
      this.txtMERSOrgID.Text = this.externalContact.MERSOriginatingORGID;
      this.cboDUSponsored.SelectedIndex = this.externalContact.DUSponsored;
      this.cboFundInOwnName.SelectedIndex = this.externalContact.CanFundInOwnName;
      this.cboCloseInOwnName.SelectedIndex = this.externalContact.CanCloseInOwnName;
      this.chkDisable.Checked = this.externalContact.DisabledLogin;
      this.chkUseParentInfoForCompany.Checked = this.externalContact.UseParentInfoForCompanyDetails;
      this.chkUseParentInfoForRate.Checked = this.externalContact.UseParentInfoForRateLock;
      this.chkUseParentInfoForEPPS.Checked = this.externalContact.UseParentInfoForEPPS;
      this.chkUseParentInfoForApproval.Checked = this.externalContact.UseParentInfoForApprovalStatus;
      this.chkUseParentInfoForBusiness.Checked = this.externalContact.UseParentInfoForBusinessInfo;
      this.cboPriceGroupBroker.SelectedItem = (object) this.settingsList["Price Group"].FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId.ToString() == this.externalContact.EPPSPriceGroupBroker));
      this.cboPriceGroupCorDel.SelectedItem = (object) this.settingsList["Price Group"].FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId.ToString() == this.externalContact.EPPSPriceGroupDelegated));
      this.cboPriceGroupCorNonDel.SelectedItem = (object) this.settingsList["Price Group"].FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId.ToString() == this.externalContact.EPPSPriceGroupNonDelegated));
      this.lblPriceGroupBroker.Enabled = !this.externalContact.UseParentInfoForEPPS && this.chkBroker.Checked;
      this.cboPriceGroupBroker.Enabled = !this.externalContact.UseParentInfoForEPPS && this.chkBroker.Checked;
      this.lblPriceGroupCorDel.Enabled = !this.externalContact.UseParentInfoForEPPS && this.chkDelegated.Checked;
      this.cboPriceGroupCorDel.Enabled = !this.externalContact.UseParentInfoForEPPS && this.chkDelegated.Checked;
      this.lblPriceGroupCorNonDel.Enabled = !this.externalContact.UseParentInfoForEPPS && this.chkNonDelegated.Checked;
      this.cboPriceGroupCorNonDel.Enabled = !this.externalContact.UseParentInfoForEPPS && this.chkNonDelegated.Checked;
      this.chkAcceptFirstPayments.Checked = this.externalContact.CanAcceptFirstPayments;
      this.cboTypeofEntity_SelectedIndexChanged((object) null, (EventArgs) null);
      this.cboLPASponsored.SelectedIndex = this.externalContact.LPASponsored;
      this.cboLPASponsored.Enabled = !this.externalContact.UseParentInfoForBusinessInfo;
      this.txtTPONumber.Enabled = this.externalContact.LPASponsored == 1 && !this.externalContact.UseParentInfoForBusinessInfo;
      this.txtLPAPassword.Enabled = this.externalContact.LPASponsored == 1 && !this.externalContact.UseParentInfoForBusinessInfo;
      if (this.externalContact.LPASponsored == 1)
      {
        this.txtTPONumber.Text = this.externalContact.LPASponsorTPONumber;
        this.txtLPAPassword.Text = XT.TryDSB64x2(this.externalContact.LPASponsorLPAPassword, KB.SC64);
      }
      this.txtLEI.Text = this.externalContact.LEINumber;
      if (this.edit)
      {
        bool hierarchyAccess;
        Session.TpoHierarchyAccessCache.TryGetValue(this.oid, out hierarchyAccess);
        TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
      }
      this.SetButtonStatus(false);
    }

    public void DisableControls()
    {
      this.btnAddRep.Visible = false;
      this.btnSave.Visible = false;
      this.btnReset.Visible = false;
      this.disableControl(this.Controls);
    }

    public void EnableChannel(ExternalOriginatorEntityType channel)
    {
      if (channel != ExternalOriginatorEntityType.Broker)
      {
        if (channel != ExternalOriginatorEntityType.Correspondent)
          return;
        this.chkCorrespondent.Checked = true;
      }
      else
        this.chkBroker.Checked = true;
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

    private void inheritCompanyInfo()
    {
      this.compType = (int) this.parentContact.entityType;
      if (this.parentContact.entityType == ExternalOriginatorEntityType.Both)
      {
        this.chkBroker.Checked = true;
        this.chkCorrespondent.Checked = true;
      }
      else if (this.parentContact.entityType == ExternalOriginatorEntityType.Broker)
      {
        this.chkBroker.Checked = true;
        this.chkCorrespondent.Checked = false;
      }
      else if (this.parentContact.entityType == ExternalOriginatorEntityType.Correspondent)
      {
        this.chkBroker.Checked = false;
        this.chkCorrespondent.Checked = true;
      }
      else if (this.parentContact.entityType == ExternalOriginatorEntityType.None)
      {
        this.chkBroker.Checked = false;
        this.chkCorrespondent.Checked = false;
      }
      if (this.parentContact.UnderwritingType == ExternalOriginatorUnderwritingType.Both)
      {
        this.chkDelegated.Checked = true;
        this.chkNonDelegated.Checked = true;
      }
      else if (this.parentContact.UnderwritingType == ExternalOriginatorUnderwritingType.Delegated)
      {
        this.chkDelegated.Checked = true;
        this.chkNonDelegated.Checked = false;
      }
      else if (this.parentContact.UnderwritingType == ExternalOriginatorUnderwritingType.NonDelegated)
      {
        this.chkDelegated.Checked = false;
        this.chkNonDelegated.Checked = true;
      }
      else if (this.parentContact.UnderwritingType == ExternalOriginatorUnderwritingType.None)
      {
        this.chkDelegated.Checked = false;
        this.chkNonDelegated.Checked = false;
      }
      this.cboGenerateDisclosures.Text = Helper.GetDescription((Enum) this.parentContact.GenerateDisclosures);
      this.txtName.Text = this.parentContact.CompanyLegalName;
      this.txtAddress.Text = this.parentContact.Address;
      this.txtCity.Text = this.parentContact.City;
      if (this.parentContact.State == "")
        this.cboStateAddr.SelectedIndex = -1;
      else
        this.cboStateAddr.Text = this.parentContact.State;
      this.txtZip.Text = this.parentContact.Zip;
      this.txtOrgID.Text = this.parentContact.OrgID;
      this.txtBillAddress.Text = this.parentContact.BillingAddress;
      this.txtBillCity.Text = this.parentContact.BillingCity;
      if (this.parentContact.BillingState == "")
        this.cboBillState.SelectedIndex = -1;
      else
        this.cboBillState.Text = this.parentContact.BillingState;
      this.txtBillZip.Text = this.parentContact.BillingZip;
      this.txtOwnerName.Text = this.parentContact.OwnerName;
      this.txtPhone.Text = this.parentContact.PhoneNumber;
      this.txtFax.Text = this.parentContact.FaxNumber;
      this.txtEmail.Text = this.parentContact.Email;
      this.txtWebsite.Text = this.parentContact.Website;
      this.chkAcceptFirstPayments.Checked = this.parentContact.CanAcceptFirstPayments;
      if (this.parentContact.LastLoanSubmitted != DateTime.MinValue)
        this.txtLastLoanSubmit.Text = this.parentContact.LastLoanSubmitted.ToString();
      else
        this.txtLastLoanSubmit.Text = "";
      if (this.parentContact.Manager != "")
        this.cboManager.SelectedItem = (object) this.getManagerName(this.parentContact.Manager);
      else
        this.cboManager.SelectedIndex = -1;
      this.chkNoAfterHourWires.Checked = this.parentContact.NoAfterHourWires;
      this.ccbTimezone.SelectedItem = (object) this.parentContact.Timezone;
    }

    private void inheritRateLockInfo()
    {
      this.txtEmailRateSheet.Text = this.parentContact.EmailForRateSheet;
      this.txtFaxRateSheet.Text = this.parentContact.FaxForRateSheet;
      this.txtEmailLockInfo.Text = this.parentContact.EmailForLockInfo;
      this.txtFaxLockInfo.Text = this.parentContact.FaxForLockInfo;
    }

    private void inheritEPPSInfo()
    {
      this.txtEPPSUserName.Text = this.parentContact.EPPSUserName;
      if (this.parentContact.EPPSCompModel != null && this.parentContact.EPPSCompModel != "")
        this.cboEPPSCompModel.SelectedIndex = Convert.ToInt32(this.parentContact.EPPSCompModel);
      else
        this.cboEPPSCompModel.SelectedIndex = -1;
      if (this.parentContact.EPPSPriceGroupBroker != "")
        this.cboPriceGroupBroker.SelectedItem = (object) this.settingsList["Price Group"].SingleOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId.ToString() == this.parentContact.EPPSPriceGroupBroker));
      else
        this.cboPriceGroupBroker.SelectedIndex = -1;
      if (this.parentContact.EPPSPriceGroupDelegated != "")
        this.cboPriceGroupCorDel.SelectedItem = (object) this.settingsList["Price Group"].SingleOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId.ToString() == this.parentContact.EPPSPriceGroupDelegated));
      else
        this.cboPriceGroupCorDel.SelectedIndex = -1;
      if (this.parentContact.EPPSPriceGroupNonDelegated != "")
        this.cboPriceGroupCorNonDel.SelectedItem = (object) this.settingsList["Price Group"].SingleOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId.ToString() == this.parentContact.EPPSPriceGroupNonDelegated));
      else
        this.cboPriceGroupCorNonDel.SelectedIndex = -1;
      if (this.parentContact.EPPSRateSheet != "")
        this.cboRateSheet.SelectedItem = (object) this.settingsList["ICE PPE Rate Sheet"].SingleOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId.ToString().Equals(this.parentContact.EPPSRateSheet)));
      else
        this.cboRateSheet.SelectedIndex = -1;
      this.txtPMLUserName.Text = this.parentContact.PMLUserName;
      this.txtPMLPassword.Text = this.parentContact.PMLPassword;
      this.txtPMLCustomerCode.Text = this.parentContact.PMLCustomerCode;
    }

    private void inheritApprovalStatusInfo()
    {
      this.cboCurrentStatus.SelectedValue = (object) this.parentContact.CurrentStatus;
      this.chkAddWatchList.Checked = this.parentContact.AddToWatchlist;
      this.dpActClosingDate.Value = !(this.parentContact.CurrentStatusDate != DateTime.MinValue) ? new DateTime() : this.parentContact.CurrentStatusDate;
      this.dpApprovedDate.Value = !(this.parentContact.ApprovedDate != DateTime.MinValue) ? new DateTime() : this.parentContact.ApprovedDate;
      this.dpApplicationDate.Value = !(this.parentContact.ApplicationDate != DateTime.MinValue) ? new DateTime() : this.parentContact.ApplicationDate;
      this.cboCompanyRating.SelectedValue = (object) this.parentContact.CompanyRating;
    }

    private void inheritBusinessInfo()
    {
      this.chkIncorporated.Checked = this.parentContact.Incorporated;
      if (this.parentContact.StateIncorp != "")
        this.cboStateIncorp.SelectedItem = (object) this.parentContact.StateIncorp;
      else
        this.cboStateIncorp.SelectedIndex = -1;
      this.dpDateincorporation.Value = !(this.parentContact.DateOfIncorporation != DateTime.MinValue) ? new DateTime() : this.parentContact.DateOfIncorporation;
      this.cboTypeofEntity.SelectedIndex = this.parentContact.TypeOfEntity;
      this.txtYrsInBusiness.Text = this.parentContact.YrsInBusiness;
      this.txtEntityDescription.Text = this.parentContact.OtherEntityDescription;
      this.txtTaxID.Text = this.parentContact.TaxID;
      this.chkUseSSN.Checked = this.parentContact.UseSSNFormat;
      this.txtNMLSID.Text = this.parentContact.NmlsId;
      this.txtFinancialPeriod.Text = this.parentContact.FinancialsPeriod;
      this.dpFinancialLastUpdate.Value = !(this.parentContact.FinancialsLastUpdate != DateTime.MinValue) ? new DateTime() : this.parentContact.FinancialsLastUpdate;
      this.txtCompanyNetWorth.Text = Convert.ToDecimal((object) this.parentContact.CompanyNetWorth).ToString("#,#", (IFormatProvider) CultureInfo.InvariantCulture);
      this.dpEOExpirationDate.Value = !(this.parentContact.EOExpirationDate != DateTime.MinValue) ? new DateTime() : this.parentContact.EOExpirationDate;
      this.txtEOCompany.Text = this.parentContact.EOCompany;
      this.txtEOPolicy.Text = this.parentContact.EOPolicyNumber;
      this.txtMERSOrgID.Text = this.parentContact.MERSOriginatingORGID;
      this.cboDUSponsored.SelectedIndex = this.parentContact.DUSponsored;
      this.cboFundInOwnName.SelectedIndex = this.parentContact.CanFundInOwnName;
      this.cboCloseInOwnName.SelectedIndex = this.parentContact.CanCloseInOwnName;
      this.cboLPASponsored.SelectedIndex = this.parentContact.LPASponsored;
      if (this.parentContact.LPASponsored == 1)
      {
        this.txtTPONumber.Text = this.parentContact.LPASponsorTPONumber;
        this.txtLPAPassword.Text = XT.TryDSB64x2(this.parentContact.LPASponsorLPAPassword, KB.SC64);
      }
      this.txtLEI.Text = this.parentContact.LEINumber;
      this.cboTypeofEntity_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void inheritParentInfo()
    {
      this.inheritCompanyInfo();
      this.inheritRateLockInfo();
      this.inheritEPPSInfo();
      this.inheritApprovalStatusInfo();
      this.inheritBusinessInfo();
      this.populateOrganizationType();
    }

    private void populateOrganizationType()
    {
      this.cmbOrgType.Items.Clear();
      this.chkVisibleOnTPOWCsite.Visible = this.chkVisibleOnTPOWCsite.Checked = false;
      if (!this.chkUseParentInfoForCompany.Checked)
      {
        this.cboGenerateDisclosures.SelectedIndexChanged -= new EventHandler(this.textField_Changed);
        this.cboGenerateDisclosures.SelectedIndex = 0;
        this.cboGenerateDisclosures.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      }
      this.cmbOrgType.SelectedIndexChanged -= new EventHandler(this.cmbOrgType_SelectedIndexChanged);
      if (this.parentContact == null)
      {
        this.cmbOrgType.Items.Add((object) "Company");
        this.cmbOrgType.SelectedIndex = 0;
        if (this.externalContact == null || this.externalContact.ExternalID == "")
          this.txtID.Text = Utils.NewTpoID(this.allTPOId).ToString();
        else
          this.txtID.Text = this.externalContact.ExternalID;
        this.updateUseParentInfo("checked", false);
        this.updateUseParentInfo("enabled", false);
        this.chkAddWatchList.Enabled = true;
      }
      else if (this.parentContact.OrganizationType == ExternalOriginatorOrgType.Company)
      {
        this.cmbOrgType.Items.Add((object) "Company Extension");
        this.cmbOrgType.Items.Add((object) "Branch");
        if (this.externalContact == null)
        {
          this.cmbOrgType.SelectedIndex = 0;
          this.txtID.Text = this.parentContact.ExternalID;
          if (this.tpoMVPSiteExists)
            this.chkVisibleOnTPOWCsite.Visible = true;
        }
        else if (this.externalContact.OrganizationType.ToString() == "Branch")
        {
          this.cmbOrgType.SelectedIndex = 1;
          this.txtID.Text = this.externalContact.ExternalID;
          if (!this.externalContact.UseParentInfo)
            this.chkAddWatchList.Enabled = true;
        }
        else
        {
          this.cmbOrgType.SelectedIndex = 0;
          this.txtID.Text = this.parentContact.ExternalID;
          this.chkAddWatchList.Enabled = false;
          if (this.tpoMVPSiteExists)
            this.chkVisibleOnTPOWCsite.Visible = true;
        }
      }
      else if (this.parentContact.OrganizationType == ExternalOriginatorOrgType.CompanyExtension)
      {
        this.cmbOrgType.Items.Add((object) "Company Extension");
        this.cmbOrgType.SelectedIndex = 0;
        this.txtID.Text = this.parentContact.ExternalID;
        this.chkAddWatchList.Enabled = false;
        if (this.tpoMVPSiteExists)
          this.chkVisibleOnTPOWCsite.Visible = true;
      }
      else if (this.parentContact.OrganizationType == ExternalOriginatorOrgType.Branch)
      {
        this.cmbOrgType.Items.Add((object) "Branch Extension");
        this.cmbOrgType.SelectedIndex = 0;
        this.txtID.Text = this.parentContact.ExternalID;
        this.chkAddWatchList.Enabled = false;
        if (this.tpoMVPSiteExists)
          this.chkVisibleOnTPOWCsite.Visible = true;
      }
      else if (this.parentContact.OrganizationType == ExternalOriginatorOrgType.BranchExtension)
      {
        this.cmbOrgType.Items.Add((object) "Branch Extension");
        this.cmbOrgType.SelectedIndex = 0;
        this.txtID.Text = this.parentContact.ExternalID;
        this.chkAddWatchList.Enabled = false;
        if (this.tpoMVPSiteExists)
          this.chkVisibleOnTPOWCsite.Visible = true;
      }
      this.cmbOrgType.SelectedIndexChanged += new EventHandler(this.cmbOrgType_SelectedIndexChanged);
      this.cmbOrgIndex = this.cmbOrgType.SelectedIndex;
      this.SetButtonStatus(false);
    }

    private void CompType_CheckedChanged(object sender, EventArgs e)
    {
      this.compType = !this.chkBroker.Checked || !this.chkCorrespondent.Checked ? (!this.chkBroker.Checked ? (!this.chkCorrespondent.Checked ? 0 : 1) : 2) : 3;
      if (this.Broker_Checked != null)
        this.Broker_Checked(sender, e);
      this.chkDelegated.Enabled = this.chkNonDelegated.Enabled = this.chkCorrespondent.Checked && this.chkCorrespondent.Enabled;
      if (this.chkCorrespondent.Checked)
        this.chkNonDelegated.Checked = true;
      if (!this.chkCorrespondent.Checked)
        this.chkDelegated.Checked = this.chkNonDelegated.Checked = false;
      this.SetButtonStatus(true);
      this.lblPriceGroupBroker.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkBroker.Checked;
      this.cboPriceGroupBroker.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkBroker.Checked;
      this.lblPriceGroupCorDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkDelegated.Checked;
      this.cboPriceGroupCorDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkDelegated.Checked;
      this.lblPriceGroupCorNonDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkNonDelegated.Checked;
      this.cboPriceGroupCorNonDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkNonDelegated.Checked;
    }

    private void textField_Changed(object sender, EventArgs e)
    {
      if (!this.chkDelegated.Checked && !this.chkNonDelegated.Checked)
        this.chkCorrespondent.Checked = false;
      if (this.chkDelegated.Checked || this.chkNonDelegated.Checked)
      {
        this.chkCorrespondent.CheckedChanged -= new EventHandler(this.CompType_CheckedChanged);
        this.chkCorrespondent.Checked = true;
        this.chkCorrespondent.CheckedChanged += new EventHandler(this.CompType_CheckedChanged);
      }
      this.SetButtonStatus(true);
      if (sender != null && sender is DatePicker)
      {
        if ((sender as DatePicker).Name.Trim().Equals("dpAeAssignedDate"))
          this.IsPrimaryAeAssignedDateChanged = true;
        if ((sender as DatePicker).Name.Trim().Equals("dpDateincorporation"))
        {
          string str1 = string.Empty;
          if (!string.IsNullOrWhiteSpace(this.dpDateincorporation.Value.ToString()) && this.dpDateincorporation.Value != DateTime.MinValue)
          {
            if (this.dpDateincorporation.Value > DateTime.Now)
            {
              str1 = "0 Year 0 Months";
            }
            else
            {
              DateTime date = DateTime.Now.Date;
              DateTime dateTime = this.dpDateincorporation.Value;
              int num1 = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
              int num2 = date.Day + (num1 - dateTime.Day);
              int num3;
              int num4;
              if (date.Month > dateTime.Month)
              {
                num3 = date.Year - dateTime.Year;
                num4 = date.Month - (dateTime.Month + 1) + Math.Abs(num2 / num1);
              }
              else if (date.Month == dateTime.Month)
              {
                if (date.Day >= dateTime.Day)
                {
                  num3 = date.Year - dateTime.Year;
                  num4 = 0;
                }
                else
                {
                  num3 = date.Year - 1 - dateTime.Year;
                  num4 = 11;
                }
              }
              else
              {
                num3 = date.Year - 1 - dateTime.Year;
                num4 = date.Month + (11 - dateTime.Month) + Math.Abs(num2 / num1);
              }
              string str2 = num3 > 1 ? " Years " : " Year ";
              str1 = num3.ToString() + str2 + (object) num4 + " Months";
            }
          }
          this.txtYrsInBusiness.Text = str1;
        }
      }
      this.lblPriceGroupBroker.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkBroker.Checked;
      this.cboPriceGroupBroker.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkBroker.Checked;
      this.lblPriceGroupCorDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkDelegated.Checked;
      this.cboPriceGroupCorDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkDelegated.Checked;
      this.lblPriceGroupCorNonDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkNonDelegated.Checked;
      this.cboPriceGroupCorNonDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkNonDelegated.Checked;
    }

    public void SetButtonStatus(bool enabled)
    {
      this.btnSave.Enabled = this.btnReset.Enabled = enabled;
    }

    public void ResetExternalObject()
    {
      this.externalContact = this.session.ConfigurationManager.GetByoid(this.forLender, this.oid);
    }

    private void companyName_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (e.KeyChar.Equals('\\'))
        e.Handled = true;
      else
        e.Handled = false;
    }

    private void txtOrgID_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsNumber(e.KeyChar))
        e.Handled = true;
      else
        e.Handled = false;
    }

    public bool DataValidated()
    {
      if (this.txtOrgName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The organization name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtOrgName.Focus();
        return false;
      }
      if (this.txtName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The company legal name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtName.Focus();
        return false;
      }
      if (this.edit)
      {
        if (this.externalContact != null && string.Compare(this.externalContact.OrganizationName, this.txtOrgName.Text.Trim(), true) != 0 && this.session.ConfigurationManager.CheckIfOrgNameExists(this.forLender, this.txtOrgName.Text.Trim(), this.parent))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You already have organization named '" + this.txtOrgName.Text.Trim() + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtOrgName.Focus();
          return false;
        }
      }
      else if (this.session.ConfigurationManager.CheckIfOrgNameExists(this.forLender, this.txtOrgName.Text.Trim(), this.parent))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You already have organization named '" + this.txtOrgName.Text.Trim() + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtOrgName.Focus();
        return false;
      }
      if (this.txtEmail.Text.Trim() != "" && !Utils.ValidateEmail(this.txtEmail.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The email address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtEmail.Focus();
        return false;
      }
      if (this.txtEmailLockInfo.Text.Trim() != "" && !Utils.ValidateEmail(this.txtEmailLockInfo.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The email address format for Lock Info is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtEmailLockInfo.Focus();
        return false;
      }
      if (this.txtEmailRateSheet.Text.Trim() != "" && !Utils.ValidateEmail(this.txtEmailRateSheet.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The email address format for Rate Sheet is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtEmailRateSheet.Focus();
        return false;
      }
      if (this.PrimarySalesRepUserId == null || this.PrimarySalesRepUserId == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Primary Sales Rep Info is required for the company.");
        return false;
      }
      if (!this.chkBroker.Checked && !this.chkCorrespondent.Checked)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Channel Type Info is required for the company.");
        return false;
      }
      if (this.txtAddress.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Address Info is required for the company.");
        this.txtAddress.Focus();
        return false;
      }
      if (this.txtCity.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "City Info is required for the company.");
        this.txtCity.Focus();
        return false;
      }
      if (this.cboStateAddr.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "State Info is required for the company.");
        this.cboStateAddr.Focus();
        return false;
      }
      if (this.txtZip.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Zip Info is required for the company.");
        this.txtZip.Focus();
        return false;
      }
      if (this.txtPhone.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Phone Info is required for the company.");
        this.txtPhone.Focus();
        return false;
      }
      List<ExternalOrgURL> selectedOrgUrls = this.session.ConfigurationManager.GetSelectedOrgUrls(this.oid);
      if (selectedOrgUrls == null || !selectedOrgUrls.Any<ExternalOrgURL>())
        selectedOrgUrls = this.session.ConfigurationManager.GetSelectedOrgUrls(this.parent);
      ExternalOriginatorEntityType originatorEntityType = !this.chkBroker.Checked || !this.chkCorrespondent.Checked ? (this.chkBroker.Checked ? ExternalOriginatorEntityType.Broker : (this.chkCorrespondent.Checked ? ExternalOriginatorEntityType.Correspondent : ExternalOriginatorEntityType.None)) : ExternalOriginatorEntityType.Both;
      foreach (ExternalOrgURL externalOrgUrl in selectedOrgUrls)
      {
        if (originatorEntityType != ExternalOriginatorEntityType.Both && externalOrgUrl.EntityType != Convert.ToInt32((object) originatorEntityType))
        {
          int num = (int) MessageBox.Show("When updating the Channel Type for your company, you must always update the Channel Type for the affected websites to the desired channel first. If you have already updated the Channel Type here, please do the following:\n• Undo your Channel Type changes here.\n• Go to the TPO WebCenter Setup tab and update the Channel Type for the websites to the desired channel.\n• Return here and update the Channel Type", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      if (this.cboLPASponsored.SelectedIndex != 1 || !string.IsNullOrWhiteSpace(this.txtTPONumber.Text) && !string.IsNullOrWhiteSpace(this.txtLPAPassword.Text))
        return true;
      int num1 = (int) MessageBox.Show("TPO Number and LPA Password are required if LPA Sponsor is 'yes'", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    public void RefreshHierarchyPath(string hierarchyPath)
    {
      this.externalContact.HierarchyPath = hierarchyPath;
    }

    public ExternalOriginatorManagementData GetModifiedData()
    {
      ExternalOriginatorManagementData modifiedData = new ExternalOriginatorManagementData();
      string str1 = Regex.Replace(this.cmbOrgType.SelectedItem.ToString(), "\\s+", "");
      modifiedData.OrganizationType = (ExternalOriginatorOrgType) Enum.Parse(typeof (ExternalOriginatorOrgType), str1, true);
      modifiedData.entityType = !this.chkBroker.Checked || !this.chkCorrespondent.Checked ? (this.chkBroker.Checked ? ExternalOriginatorEntityType.Broker : (this.chkCorrespondent.Checked ? ExternalOriginatorEntityType.Correspondent : ExternalOriginatorEntityType.None)) : ExternalOriginatorEntityType.Both;
      modifiedData.UnderwritingType = !this.chkDelegated.Checked || !this.chkNonDelegated.Checked ? (this.chkDelegated.Checked ? ExternalOriginatorUnderwritingType.Delegated : (this.chkNonDelegated.Checked ? ExternalOriginatorUnderwritingType.NonDelegated : ExternalOriginatorUnderwritingType.None)) : ExternalOriginatorUnderwritingType.Both;
      modifiedData.GenerateDisclosures = Utils.GetEnumValueFromDescription<ManageFeeLEDisclosures>(this.cboGenerateDisclosures.Text);
      modifiedData.ExternalID = this.txtID.Text.Trim();
      modifiedData.OldExternalID = this.externalContact != null ? this.externalContact.OldExternalID : "";
      modifiedData.OrgID = this.txtOrgID.Text.Trim();
      modifiedData.AutoAssignOrgID = this.useAutoOrgID;
      modifiedData.OwnerName = this.txtOwnerName.Text.Trim();
      if (this.externalContact != null && this.externalContact.OrganizationName != this.txtOrgName.Text.Trim())
      {
        string str2 = this.externalContact.HierarchyPath.Substring(0, this.externalContact.HierarchyPath.LastIndexOf('\\') + 1);
        modifiedData.HierarchyPath = str2 + this.txtOrgName.Text.Trim();
      }
      else if (this.externalContact != null)
        modifiedData.HierarchyPath = this.externalContact.HierarchyPath;
      modifiedData.CompanyLegalName = this.txtName.Text.Trim();
      modifiedData.OrganizationName = this.txtOrgName.Text.Trim();
      modifiedData.Address = this.txtAddress.Text.Trim();
      modifiedData.City = this.txtCity.Text.Trim();
      modifiedData.State = this.cboStateAddr.Text.Trim();
      modifiedData.Zip = this.txtZip.Text.Trim();
      modifiedData.PhoneNumber = this.txtPhone.Text.Trim();
      modifiedData.FaxNumber = this.txtFax.Text.Trim();
      modifiedData.Email = this.txtEmail.Text.Trim();
      modifiedData.Website = this.txtWebsite.Text.Trim();
      modifiedData.CanAcceptFirstPayments = this.chkAcceptFirstPayments.Checked;
      modifiedData.BillingAddress = this.txtBillAddress.Text.Trim();
      modifiedData.BillingCity = this.txtBillCity.Text.Trim();
      modifiedData.BillingState = this.cboBillState.Text.Trim();
      modifiedData.BillingZip = this.txtBillZip.Text.Trim();
      if (this.cboManager.SelectedItem != null)
        modifiedData.Manager = this.getManagerID(this.cboManager.SelectedItem.ToString());
      if (this.txtLastLoanSubmit.Text.Trim() != "")
        modifiedData.LastLoanSubmitted = Convert.ToDateTime(this.txtLastLoanSubmit.Text.Trim());
      modifiedData.EmailForRateSheet = this.txtEmailRateSheet.Text.Trim();
      modifiedData.FaxForRateSheet = this.txtFaxRateSheet.Text.Trim();
      modifiedData.EmailForLockInfo = this.txtEmailLockInfo.Text.Trim();
      modifiedData.FaxForLockInfo = this.txtFaxLockInfo.Text.Trim();
      modifiedData.EPPSUserName = this.txtEPPSUserName.Text.Trim();
      int num;
      if (this.cboEPPSCompModel.SelectedItem != null)
      {
        ExternalOriginatorManagementData originatorManagementData = modifiedData;
        num = this.cboEPPSCompModel.SelectedIndex;
        string str3 = num.ToString();
        originatorManagementData.EPPSCompModel = str3;
      }
      if (this.cboPriceGroupBroker.SelectedItem != null)
      {
        ExternalOriginatorManagementData originatorManagementData = modifiedData;
        num = ((ExternalSettingValue) this.cboPriceGroupBroker.SelectedItem).settingId;
        string str4 = num.ToString();
        originatorManagementData.EPPSPriceGroupBroker = str4;
      }
      if (this.cboPriceGroupCorDel.SelectedItem != null)
      {
        ExternalOriginatorManagementData originatorManagementData = modifiedData;
        num = ((ExternalSettingValue) this.cboPriceGroupCorDel.SelectedItem).settingId;
        string str5 = num.ToString();
        originatorManagementData.EPPSPriceGroupDelegated = str5;
      }
      if (this.cboPriceGroupCorNonDel.SelectedItem != null)
      {
        ExternalOriginatorManagementData originatorManagementData = modifiedData;
        num = ((ExternalSettingValue) this.cboPriceGroupCorNonDel.SelectedItem).settingId;
        string str6 = num.ToString();
        originatorManagementData.EPPSPriceGroupNonDelegated = str6;
      }
      if (this.cboRateSheet.SelectedItem != null)
      {
        ExternalOriginatorManagementData originatorManagementData = modifiedData;
        num = ((ExternalSettingValue) this.cboRateSheet.SelectedItem).settingId;
        string str7 = num.ToString();
        originatorManagementData.EPPSRateSheet = str7;
      }
      modifiedData.PMLUserName = this.txtPMLUserName.Text.Trim();
      modifiedData.PMLPassword = this.txtPMLPassword.Text.Trim();
      modifiedData.PMLCustomerCode = this.txtPMLCustomerCode.Text.Trim();
      modifiedData.CurrentStatus = this.cboCurrentStatus.SelectedIndex <= 0 ? -1 : ((ExternalSettingValue) this.cboCurrentStatus.SelectedItem).settingId;
      modifiedData.AddToWatchlist = this.chkAddWatchList.Checked;
      modifiedData.CurrentStatusDate = Convert.ToDateTime(this.dpActClosingDate.Value);
      modifiedData.ApprovedDate = Convert.ToDateTime(this.dpApprovedDate.Value);
      modifiedData.ApplicationDate = Convert.ToDateTime(this.dpApplicationDate.Value);
      modifiedData.CompanyRating = this.cboCompanyRating.SelectedIndex <= 0 ? -1 : ((ExternalSettingValue) this.cboCompanyRating.SelectedItem).settingId;
      modifiedData.Incorporated = this.chkIncorporated.Checked;
      if (this.cboStateIncorp.SelectedItem != null)
        modifiedData.StateIncorp = this.cboStateIncorp.SelectedItem.ToString();
      modifiedData.DateOfIncorporation = Convert.ToDateTime(this.dpDateincorporation.Value);
      modifiedData.TypeOfEntity = this.cboTypeofEntity.SelectedIndex;
      modifiedData.OtherEntityDescription = this.txtEntityDescription.Text.Trim();
      modifiedData.TaxID = this.txtTaxID.Text.Trim();
      modifiedData.UseSSNFormat = this.chkUseSSN.Checked;
      modifiedData.NmlsId = this.txtNMLSID.Text.Trim();
      modifiedData.FinancialsPeriod = this.txtFinancialPeriod.Text.Trim();
      modifiedData.FinancialsLastUpdate = Convert.ToDateTime(this.dpFinancialLastUpdate.Value);
      if (this.txtCompanyNetWorth.Text.Trim() != "")
        modifiedData.CompanyNetWorth = new Decimal?(Convert.ToDecimal(this.txtCompanyNetWorth.Text.Trim()));
      modifiedData.EOExpirationDate = Convert.ToDateTime(this.dpEOExpirationDate.Value);
      modifiedData.EOCompany = this.txtEOCompany.Text.Trim();
      modifiedData.EOPolicyNumber = this.txtEOPolicy.Text.Trim();
      modifiedData.MERSOriginatingORGID = this.txtMERSOrgID.Text.Trim();
      modifiedData.DUSponsored = this.cboDUSponsored.SelectedIndex;
      modifiedData.CanFundInOwnName = this.cboFundInOwnName.SelectedIndex;
      modifiedData.CanCloseInOwnName = this.cboCloseInOwnName.SelectedIndex;
      modifiedData.UseParentInfoForCompanyDetails = this.chkUseParentInfoForCompany.Checked;
      modifiedData.UseParentInfoForRateLock = this.chkUseParentInfoForRate.Checked;
      modifiedData.UseParentInfoForEPPS = this.chkUseParentInfoForEPPS.Checked;
      modifiedData.UseParentInfoForApprovalStatus = this.chkUseParentInfoForApproval.Checked;
      modifiedData.UseParentInfoForBusinessInfo = this.chkUseParentInfoForBusiness.Checked;
      modifiedData.DisabledLogin = this.chkDisable.Checked;
      modifiedData.VisibleOnTPOWCSite = new bool?(this.chkVisibleOnTPOWCsite.Checked);
      modifiedData.MultiFactorAuthentication = this.chkMultiFactorAuth.Checked;
      modifiedData.NoAfterHourWires = this.chkNoAfterHourWires.Checked;
      modifiedData.Timezone = this.ccbTimezone.SelectedItem == null ? (string) null : this.ccbTimezone.SelectedItem.ToString().Trim();
      modifiedData.LPASponsored = this.cboLPASponsored.SelectedIndex;
      modifiedData.LPASponsorTPONumber = this.txtTPONumber.Text;
      modifiedData.LPASponsorLPAPassword = XT.ESB64x2(this.txtLPAPassword.Text, KB.SC64);
      modifiedData.LEINumber = this.txtLEI.Text;
      if (this.externalContact != null)
      {
        modifiedData.CommitmentUseBestEffort = this.externalContact.CommitmentUseBestEffort;
        modifiedData.CommitmentUseBestEffortLimited = this.externalContact.CommitmentUseBestEffortLimited;
        modifiedData.MaxCommitmentAuthority = this.externalContact.MaxCommitmentAuthority;
        modifiedData.CommitmentMandatory = this.externalContact.CommitmentMandatory;
        modifiedData.MaxCommitmentAmount = this.externalContact.MaxCommitmentAmount;
        modifiedData.IsCommitmentDeliveryIndividual = this.externalContact.IsCommitmentDeliveryIndividual;
        modifiedData.IsCommitmentDeliveryBulk = this.externalContact.IsCommitmentDeliveryBulk;
        modifiedData.IsCommitmentDeliveryAOT = this.externalContact.IsCommitmentDeliveryAOT;
        modifiedData.IsCommitmentDeliveryLiveTrade = this.externalContact.IsCommitmentDeliveryLiveTrade;
        modifiedData.IsCommitmentDeliveryForward = this.externalContact.IsCommitmentDeliveryForward;
        modifiedData.IsCommitmentDeliveryCoIssue = this.externalContact.IsCommitmentDeliveryCoIssue;
        modifiedData.IsCommitmentDeliveryBulkAOT = this.externalContact.IsCommitmentDeliveryBulkAOT;
        modifiedData.CommitmentPolicy = this.externalContact.CommitmentPolicy;
        modifiedData.CommitmentTradePolicy = this.externalContact.CommitmentTradePolicy;
        modifiedData.CommitmentMessage = this.externalContact.CommitmentMessage;
        modifiedData.BestEffortToleranceAmt = this.externalContact.BestEffortToleranceAmt;
        modifiedData.BestEffortTolerancePct = this.externalContact.BestEffortTolerancePct;
        modifiedData.BestEffortTolerencePolicy = this.externalContact.BestEffortTolerencePolicy;
        modifiedData.MandatoryToleranceAmt = this.externalContact.MandatoryToleranceAmt;
        modifiedData.MandatoryTolerancePct = this.externalContact.MandatoryTolerancePct;
        modifiedData.MandatoryTolerencePolicy = this.externalContact.MandatoryTolerencePolicy;
        modifiedData.BestEffortDailyVolumeLimit = this.externalContact.BestEffortDailyVolumeLimit;
        modifiedData.BestEfforDailyLimitPolicy = this.externalContact.BestEfforDailyLimitPolicy;
        modifiedData.DailyLimitWarningMsg = this.externalContact.DailyLimitWarningMsg;
        modifiedData.TradeMgmtEnableTPOTradeManagementForTPOClient = this.externalContact.TradeMgmtEnableTPOTradeManagementForTPOClient;
        modifiedData.TradeMgmtUseCompanyTPOTradeManagementSettings = this.externalContact.TradeMgmtUseCompanyTPOTradeManagementSettings;
        modifiedData.TradeMgmtViewCorrespondentTrade = this.externalContact.TradeMgmtViewCorrespondentTrade;
        modifiedData.TradeMgmtViewCorrespondentMasterCommitment = this.externalContact.TradeMgmtViewCorrespondentMasterCommitment;
        modifiedData.TradeMgmtLoanEligibilityToCorrespondentTrade = this.externalContact.TradeMgmtLoanEligibilityToCorrespondentTrade;
        modifiedData.TradeMgmtEPPSLoanProgramEligibilityPricing = this.externalContact.TradeMgmtEPPSLoanProgramEligibilityPricing;
        modifiedData.TradeMgmtLoanAssignmentToCorrespondentTrade = this.externalContact.TradeMgmtLoanAssignmentToCorrespondentTrade;
        modifiedData.TradeMgmtLoanDeletionFromCorrespondentTrade = this.externalContact.TradeMgmtLoanDeletionFromCorrespondentTrade;
        modifiedData.TradeMgmtRequestPairOff = this.externalContact.TradeMgmtRequestPairOff;
        modifiedData.TradeMgmtReceiveCommitmentConfirmation = this.externalContact.TradeMgmtReceiveCommitmentConfirmation;
      }
      modifiedData.PrimarySalesRepUserId = this.PrimarySalesRepUserId;
      modifiedData.PrimarySalesRepAssignedDate = this.dpAeAssignedDate.Value;
      this.externalContact = modifiedData;
      return modifiedData;
    }

    public void AssignOid(int oid)
    {
      if (this.externalContact != null)
        this.externalContact.oid = oid;
      this.oid = oid;
      if (!this.useAutoOrgID)
        return;
      this.externalContact = this.session.ConfigurationManager.GetByoid(this.forLender, oid);
      this.txtOrgID.Text = this.externalContact.OrgID;
    }

    public int Oid => this.externalContact != null ? this.externalContact.oid : -1;

    public bool IsBroker => this.chkBroker.Checked;

    public bool IsCorrespondent => this.chkCorrespondent.Checked;

    public bool IsCompanyInfoBlank => this.txtName.Text.Trim() == "";

    public bool IsDirty => this.btnSave.Enabled;

    private void chkUseSSN_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkUseSSN.Checked)
        this.inputEventHelper.AddSSNFieldHelper(this.txtTaxID);
      else
        this.inputEventHelper.RemoveSSNFieldHelper(this.txtTaxID);
      this.txtTaxID.Text = this.txtTaxID.Text.Replace("-", "");
      if (!this.chkUseSSN.Checked)
        this.txtTaxID.Text = Regex.Replace(this.txtTaxID.Text, "(\\d{2})(\\d{7})", "$1-$2");
      else
        this.inputEventHelper.ReformatField(FieldFormat.SSN, this.txtTaxID);
      this.SetButtonStatus(true);
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset? All changes will be lost.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      if (this.edit)
      {
        this.cmbOrgIndex = -1;
        this.populateOrganizationType();
        this.populateFields();
      }
      else
      {
        this.clearControl((Control) this);
        if (this.parentContact != null)
          this.inheritParentInfo();
      }
      if (this.DefaultPrimarySalesRepUserId != null)
      {
        UserInfo user = this.session.OrganizationManager.GetUser(this.DefaultPrimarySalesRepUserId);
        if (user != (UserInfo) null)
        {
          this.PrimarySalesRepUserId = user.Userid;
          this.PrimarySalesRepName = user.FullName;
        }
        if (this.edit)
          this.IsPrimarySalesRepChanged = false;
        else if (!this.edit && user != (UserInfo) null)
          this.IsPrimarySalesRepChanged = true;
        this.dpAeAssignedDate.Value = this.DefaultPrimarySalesRepAssignedDate;
        this.populateSalesRepInfo(user);
        if (user != (UserInfo) null)
          this.DefaultPrimarySalesRepName = user.FullName;
      }
      else
      {
        this.txtNameSalesRep.Text = string.Empty;
        this.txtPersonaSalesRep.Text = string.Empty;
        this.txtEmailSalesRep.Text = string.Empty;
        this.txtPhoneSalesRep.Text = string.Empty;
        this.PrimarySalesRepUserId = (string) null;
        this.dpAeAssignedDate.Value = new DateTime(0L);
      }
      if (this.edit)
        this.SetButtonStatus(false);
      else
        this.SetButtonStatus(true);
      this.updateCopyCompanyAddressCheckbox();
    }

    private void clearControl(Control control)
    {
      if (control.Name == "txtID" || control.Name == "cmbOrgType")
        return;
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

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.txtLPAPassword.PasswordChar = '*';
      if (this.chkCopyCompanyAddress.Checked)
      {
        this.txtBillAddress.Text = this.txtAddress.Text;
        this.txtBillCity.Text = this.txtCity.Text;
        this.cboBillState.Text = this.cboStateAddr.Text;
        this.txtBillZip.Text = this.txtZip.Text;
      }
      if (this.SaveButton_Clicked != null)
        this.SaveButton_Clicked(sender, e);
      if (this.btnSave.Enabled)
        return;
      this.edit = true;
      this.IsPrimarySalesRepChanged = false;
    }

    private void cmbOrgType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.chkVisibleOnTPOWCsite.Checked = false;
      if (this.cmbOrgType.SelectedIndex == this.cmbOrgIndex)
        return;
      this.SetButtonStatus(true);
      if (this.cmbOrgType.SelectedItem.ToString() == "Company Extension")
      {
        if (this.txtID.Text != this.parentContact.ExternalID)
        {
          if (this.externalContact != null)
            this.externalContact.OldExternalID = this.txtID.Text;
          this.txtID.Text = this.parentContact.ExternalID;
        }
        this.chkAddWatchList.Enabled = false;
        this.chkAddWatchList.Checked = this.parentContact.AddToWatchlist;
        if (this.tpoMVPSiteExists)
          this.chkVisibleOnTPOWCsite.Visible = true;
      }
      else if (this.cmbOrgType.SelectedItem.ToString() == "Branch")
      {
        if (this.externalContact == null || this.externalContact.ExternalID == "")
        {
          this.txtID.Text = Utils.NewTpoID(this.allTPOId).ToString();
        }
        else
        {
          if (this.externalContact.ExternalID != this.txtID.Text)
            this.txtID.Text = this.externalContact.ExternalID;
          else if (!string.IsNullOrEmpty(this.externalContact.OldExternalID))
            this.txtID.Text = this.externalContact.OldExternalID;
          else
            this.txtID.Text = Utils.NewTpoID(this.allTPOId).ToString();
          this.externalContact.ExternalID = this.txtID.Text;
        }
        if (!this.chkUseParentInfoForApproval.Checked)
          this.chkAddWatchList.Enabled = true;
        this.chkVisibleOnTPOWCsite.Visible = false;
      }
      else if (this.cmbOrgType.SelectedItem.ToString() == "Branch Extension")
      {
        this.chkAddWatchList.Enabled = false;
        this.chkAddWatchList.Checked = this.parentContact.AddToWatchlist;
        if (this.tpoMVPSiteExists)
          this.chkVisibleOnTPOWCsite.Visible = true;
      }
      this.cmbOrgIndex = this.cmbOrgType.SelectedIndex;
    }

    private void cboTypeofEntity_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
      if (this.chkUseParentInfoForBusiness.Checked)
      {
        this.txtEntityDescription.Text = this.cboTypeofEntity.SelectedIndex != 6 ? "" : (this.parentContact != null ? this.parentContact.OtherEntityDescription : "");
      }
      else
      {
        this.txtEntityDescription.Enabled = this.cboTypeofEntity.SelectedIndex == 6;
        this.txtEntityDescription.ReadOnly = this.cboTypeofEntity.SelectedIndex != 6;
        this.txtEntityDescription.Text = this.cboTypeofEntity.SelectedIndex != 6 ? "" : (this.externalContact != null ? this.externalContact.OtherEntityDescription : "");
      }
    }

    private void txtNMLSID_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsNumber(e.KeyChar))
        e.Handled = true;
      else
        e.Handled = false;
    }

    private void chkUseParentInfoForCompany_CheckedChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
      this.txtOrgID.Enabled = this.cmbOrgType.Enabled = this.chkCorrespondent.Enabled = this.chkBroker.Enabled = this.txtOwnerName.Enabled = this.txtName.Enabled = this.txtAddress.Enabled = this.txtCity.Enabled = this.cboStateAddr.Enabled = this.txtZip.Enabled = this.txtPhone.Enabled = this.txtFax.Enabled = this.txtEmail.Enabled = this.txtWebsite.Enabled = this.cboManager.Enabled = this.txtLastLoanSubmit.Enabled = this.chkNoAfterHourWires.Enabled = this.ccbTimezone.Enabled = this.chkAcceptFirstPayments.Enabled = this.txtBillAddress.Enabled = this.txtBillCity.Enabled = this.cboBillState.Enabled = this.txtBillZip.Enabled = this.chkCopyCompanyAddress.Enabled = !this.chkUseParentInfoForCompany.Checked;
      this.chkDelegated.Enabled = this.chkNonDelegated.Enabled = this.chkCorrespondent.Checked && this.chkCorrespondent.Enabled;
      if (this.chkUseParentInfoForCompany.Checked)
        this.inheritCompanyInfo();
      this.updateCopyCompanyAddressCheckbox();
    }

    private void chkUseParentInfoForRate_CheckedChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
      this.txtEmailRateSheet.Enabled = this.txtFaxRateSheet.Enabled = this.txtEmailLockInfo.Enabled = this.txtFaxLockInfo.Enabled = !this.chkUseParentInfoForRate.Checked;
      if (!this.chkUseParentInfoForRate.Checked)
        return;
      this.inheritRateLockInfo();
    }

    private void chkUseParentInfoForEPPS_CheckedChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
      this.txtEPPSUserName.Enabled = this.cboEPPSCompModel.Enabled = this.cboPriceGroupBroker.Enabled = this.cboPriceGroupCorDel.Enabled = this.cboPriceGroupCorNonDel.Enabled = this.cboRateSheet.Enabled = this.txtPMLCustomerCode.Enabled = this.txtPMLPassword.Enabled = this.txtPMLUserName.Enabled = !this.chkUseParentInfoForEPPS.Checked;
      this.lblPriceGroupBroker.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkBroker.Checked;
      this.cboPriceGroupBroker.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkBroker.Checked;
      this.lblPriceGroupCorDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkDelegated.Checked;
      this.cboPriceGroupCorDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkDelegated.Checked;
      this.lblPriceGroupCorNonDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkNonDelegated.Checked;
      this.cboPriceGroupCorNonDel.Enabled = !this.chkUseParentInfoForEPPS.Checked && this.chkNonDelegated.Checked;
      if (!this.chkUseParentInfoForEPPS.Checked)
        return;
      this.inheritEPPSInfo();
    }

    private void chkUseParentInfoForApproval_CheckedChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
      this.cboCurrentStatus.Enabled = this.chkAddWatchList.Enabled = this.dpActClosingDate.Enabled = this.dpApplicationDate.Enabled = this.dpApprovedDate.Enabled = this.cboCompanyRating.Enabled = !this.chkUseParentInfoForApproval.Checked;
      if (this.chkUseParentInfoForApproval.Checked)
        this.inheritApprovalStatusInfo();
      if (this.chkUseParentInfoForApproval.Checked)
        return;
      if (this.cmbOrgType.SelectedItem.ToString() == "Company Extension")
      {
        this.chkAddWatchList.Enabled = false;
        this.chkAddWatchList.Checked = this.parentContact.AddToWatchlist;
      }
      else if (this.cmbOrgType.SelectedItem.ToString() == "Branch")
      {
        this.chkAddWatchList.Enabled = true;
      }
      else
      {
        if (!(this.cmbOrgType.SelectedItem.ToString() == "Branch Extension"))
          return;
        this.chkAddWatchList.Enabled = false;
        this.chkAddWatchList.Checked = this.parentContact.AddToWatchlist;
      }
    }

    private void ChkUseParentInfoForBusinessOnCheckedChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
      this.chkIncorporated.Enabled = this.cboStateIncorp.Enabled = this.dpDateincorporation.Enabled = this.cboTypeofEntity.Enabled = this.txtEntityDescription.Enabled = this.txtTaxID.Enabled = this.chkUseSSN.Enabled = this.txtNMLSID.Enabled = this.txtFinancialPeriod.Enabled = this.dpFinancialLastUpdate.Enabled = this.txtCompanyNetWorth.Enabled = this.dpEOExpirationDate.Enabled = this.txtEOCompany.Enabled = this.txtEOPolicy.Enabled = this.txtMERSOrgID.Enabled = this.cboDUSponsored.Enabled = this.cboCloseInOwnName.Enabled = this.cboFundInOwnName.Enabled = this.cboLPASponsored.Enabled = this.txtTPONumber.Enabled = this.txtLPAPassword.Enabled = this.txtLEI.Enabled = !this.chkUseParentInfoForBusiness.Checked;
      if (this.cboTypeofEntity.Enabled)
      {
        this.txtEntityDescription.Enabled = this.cboTypeofEntity.SelectedIndex == 6;
        this.txtEntityDescription.ReadOnly = this.cboTypeofEntity.SelectedIndex != 6;
      }
      if (!this.chkUseParentInfoForBusiness.Checked)
        return;
      this.inheritBusinessInfo();
    }

    private void updateUseParentInfo(string property, bool value)
    {
      if (property == "checked")
        this.chkUseParentInfoForCompany.Checked = this.chkUseParentInfoForRate.Checked = this.chkUseParentInfoForEPPS.Checked = this.chkUseParentInfoForApproval.Checked = this.chkUseParentInfoForBusiness.Checked = value;
      if (!(property == "enabled"))
        return;
      this.chkUseParentInfoForCompany.Enabled = this.chkUseParentInfoForRate.Enabled = this.chkUseParentInfoForEPPS.Enabled = this.chkUseParentInfoForApproval.Enabled = this.chkUseParentInfoForBusiness.Enabled = value;
    }

    private void txtTaxID_Leave(object sender, EventArgs e)
    {
      if (this.externalContact != null && this.externalContact.TaxID != null)
      {
        if (this.externalContact.TaxID.Trim() == this.txtTaxID.Text.Trim())
          return;
      }
      else if (this.txtTaxID.Text.Trim() == "")
        return;
      this.SetButtonStatus(true);
      this.chkUseSSN_CheckedChanged((object) null, (EventArgs) null);
    }

    private void txtTaxID_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
      {
        this.SetButtonStatus(true);
      }
      else
      {
        if (!char.IsNumber(e.KeyChar))
          e.Handled = true;
        if (this.txtTaxID.Text.Length == 9 && this.txtTaxID.Text.IndexOf('-') == 2 || this.txtTaxID.Text.Length >= 9 && this.txtTaxID.Text.IndexOf('-') == 3)
          return;
        if (this.txtTaxID.Text.Length >= 9)
          e.Handled = true;
        this.SetButtonStatus(true);
      }
    }

    private void grpAll_Resize(object sender, EventArgs e)
    {
    }

    private void loadAllInternalUsers()
    {
      this.loadOrgLookUp();
      if (this.allInternalUsers != null)
        return;
      this.allInternalUsers = this.rOrg.GetAllAccessibleSalesRepUsers();
    }

    private void loadOrgLookUp()
    {
      if (this.rOrg == null)
        this.rOrg = this.session.OrganizationManager;
      if (this.orgLookup != null)
        return;
      OrgInfo[] allOrganizations = this.rOrg.GetAllOrganizations();
      this.orgLookup = new Hashtable(allOrganizations.Length);
      for (int index = 0; index < allOrganizations.Length; ++index)
        this.orgLookup.Add((object) allOrganizations[index].Oid, (object) allOrganizations[index].OrgName);
    }

    private void btnAddRep_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.loadAllInternalUsers();
      List<string> existingUserIDs = new List<string>();
      if (this.PrimarySalesRepUserId != null)
        existingUserIDs.Add(this.PrimarySalesRepUserId);
      Cursor.Current = Cursors.Default;
      using (AddSalesRepForm addSalesRepForm = new AddSalesRepForm(this.allInternalUsers, existingUserIDs))
      {
        if (addSalesRepForm.ShowDialog((IWin32Window) this) != DialogResult.OK || addSalesRepForm.SelectedUsers.Length == 0)
          return;
        this.PrimarySalesRepUserId = addSalesRepForm.SelectedUsers[0].Userid;
        this.PrimarySalesRepName = addSalesRepForm.SelectedUsers[0].FullName;
        this.dpAeAssignedDate.Value = DateTime.Now.Date;
        this.populateSalesRepInfo(this.session.OrganizationManager.GetUser(this.PrimarySalesRepUserId));
        this.IsPrimarySalesRepChanged = true;
        this.IsPrimaryAeAssignedDateChanged = true;
        this.SetButtonStatus(true);
      }
    }

    private string getManagerName(string userid)
    {
      ExternalUserInfo externalUserInfo = this.managers.FirstOrDefault<ExternalUserInfo>((Func<ExternalUserInfo, bool>) (item => item.ExternalUserID == userid));
      return (UserInfo) externalUserInfo != (UserInfo) null ? externalUserInfo.FirstName + " " + externalUserInfo.LastName : "";
    }

    private string getManagerID(string name)
    {
      ExternalUserInfo externalUserInfo = this.managers.FirstOrDefault<ExternalUserInfo>((Func<ExternalUserInfo, bool>) (item => name == item.FirstName + " " + item.LastName));
      return (UserInfo) externalUserInfo != (UserInfo) null ? externalUserInfo.ExternalUserID : "";
    }

    public List<ExternalUserInfo> Manager
    {
      set => this.managers = value;
    }

    private void txtCompanyNetWorth_Leave(object sender, EventArgs e)
    {
      try
      {
        if (!(this.txtCompanyNetWorth.Text.Trim() != ""))
          return;
        this.txtCompanyNetWorth.Text = Convert.ToDecimal(this.txtCompanyNetWorth.Text).ToString("#,#", (IFormatProvider) CultureInfo.InvariantCulture);
        this.txtCompanyNetWorth.SelectionLength = this.txtCompanyNetWorth.Text.Trim().Length;
      }
      catch (Exception ex)
      {
        this.txtCompanyNetWorth.Text = string.Empty;
      }
    }

    private void txtCompanyNetWorth_Enter(object sender, EventArgs e)
    {
      if (!(this.txtCompanyNetWorth.Text.Trim() == "0"))
        return;
      this.txtCompanyNetWorth.Text = "";
    }

    private void cboCurrentStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.dpActClosingDate.Value = this.externalContact == null || Convert.ToInt32(this.cboCurrentStatus.SelectedValue) != this.externalContact.CurrentStatus ? (this.externalContact != null || this.cboCurrentStatus.SelectedIndex != 0 && this.cboCurrentStatus.SelectedIndex != -1 ? DateTime.Now : new DateTime()) : (!(this.externalContact.CurrentStatusDate != DateTime.MinValue) ? new DateTime() : this.externalContact.CurrentStatusDate);
      this.SetButtonStatus(true);
    }

    private void chkVisibleOnTPOWCsite_CheckedChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
    }

    private void ccbTimezone_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
    }

    private void txtTPONumber_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (this.txtTPONumber.Text.Length >= 15)
        e.Handled = true;
      this.SetButtonStatus(true);
    }

    private void txtTPONumber_Leave(object sender, EventArgs e)
    {
      if (this.externalContact == null || this.externalContact.LPASponsorTPONumber == null || !(this.externalContact.LPASponsorTPONumber.Trim() != this.txtTPONumber.Text.Trim()))
        return;
      this.SetButtonStatus(true);
    }

    private void txtLPAPassword_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.txtLPAPassword.PasswordChar == '*' && !this.chkUseParentInfoForBusiness.Checked)
      {
        this.txtLPAPassword.PasswordChar = char.MinValue;
        this.txtLPAPassword.Text = string.Empty;
      }
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (this.txtLPAPassword.Text.Length >= 8)
        e.Handled = true;
      this.SetButtonStatus(true);
    }

    private void txtLPAPassword_Leave(object sender, EventArgs e)
    {
      if (this.externalContact != null && this.externalContact.LPASponsorLPAPassword != null && XT.TryDSB64x2(this.externalContact.LPASponsorLPAPassword, KB.SC64).Trim() != this.txtLPAPassword.Text.Trim())
        this.SetButtonStatus(true);
      this.txtLPAPassword.PasswordChar = '*';
    }

    private void cboLPASponsored_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.txtTPONumber.Enabled = this.cboLPASponsored.SelectedIndex == 1 && !this.chkUseParentInfoForBusiness.Checked;
      this.txtLPAPassword.Enabled = this.cboLPASponsored.SelectedIndex == 1 && !this.chkUseParentInfoForBusiness.Checked;
      if (this.cboLPASponsored.SelectedIndex != 1)
      {
        this.txtTPONumber.Text = (string) null;
        this.txtLPAPassword.Text = (string) null;
      }
      else if (this.cboLPASponsored.SelectedIndex == 1 && this.externalContact != null)
      {
        this.txtTPONumber.Text = this.externalContact.LPASponsorTPONumber;
        this.txtLPAPassword.Text = XT.TryDSB64x2(this.externalContact.LPASponsorLPAPassword, KB.SC64);
      }
      this.SetButtonStatus(true);
    }

    private void txtLEI_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || e.KeyChar.Equals((object) Keys.Space))
        return;
      if (this.txtLEI.Text.Length >= 20)
        e.Handled = true;
      e.Handled = !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
      this.SetButtonStatus(true);
    }

    private void txtLEI_Leave(object sender, EventArgs e)
    {
      if (this.externalContact == null || this.externalContact.LEINumber == null || !(this.externalContact.LEINumber.Trim() != this.txtLEI.Text.Trim()))
        return;
      this.SetButtonStatus(true);
    }

    private void chkAcceptFirstPayments_CheckedChanged(object sender, EventArgs e)
    {
      this.SetButtonStatus(true);
    }

    private void chkCopyAdd_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkCopyCompanyAddress.Checked)
      {
        this.txtBillAddress.Text = this.txtAddress.Text;
        this.txtBillCity.Text = this.txtCity.Text;
        this.cboBillState.Text = this.cboStateAddr.Text;
        this.txtBillZip.Text = this.txtZip.Text;
        this.txtBillAddress.Enabled = false;
        this.txtBillCity.Enabled = false;
        this.cboBillState.Enabled = false;
        this.txtBillZip.Enabled = false;
      }
      else if (this.chkUseParentInfoForCompany.Checked)
      {
        this.txtBillAddress.Text = this.parentContact.BillingAddress;
        this.txtBillCity.Text = this.parentContact.BillingCity;
        this.txtBillZip.Text = this.parentContact.BillingZip;
        if (this.parentContact.BillingState == "")
          this.cboBillState.SelectedIndex = -1;
        else
          this.cboBillState.Text = this.parentContact.BillingState;
      }
      else
      {
        this.txtBillAddress.Text = this.edit ? this.externalContact.BillingAddress : string.Empty;
        this.txtBillCity.Text = this.edit ? this.externalContact.BillingCity : string.Empty;
        this.txtBillZip.Text = this.edit ? this.externalContact.BillingZip : string.Empty;
        if (this.edit)
        {
          if (this.externalContact.BillingState == "")
            this.cboBillState.SelectedIndex = -1;
          else
            this.cboBillState.Text = this.externalContact.BillingState;
        }
        else
          this.cboBillState.SelectedIndex = -1;
        this.txtBillAddress.Enabled = true;
        this.txtBillCity.Enabled = true;
        this.cboBillState.Enabled = true;
        this.txtBillZip.Enabled = true;
      }
    }

    private void updateCopyCompanyAddressCheckbox()
    {
      if (this.edit && this.txtBillAddress.Text == this.txtAddress.Text && this.txtBillCity.Text == this.txtCity.Text && this.cboBillState.Text == this.cboStateAddr.Text && this.txtBillZip.Text == this.txtZip.Text)
      {
        this.chkCopyCompanyAddress.Checked = true;
        this.txtBillAddress.Enabled = false;
        this.txtBillCity.Enabled = false;
        this.cboBillState.Enabled = false;
        this.txtBillZip.Enabled = false;
      }
      else
        this.chkCopyCompanyAddress.Checked = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grpAll = new GroupContainer();
      this.btnReset = new StandardIconButton();
      this.panelHeader = new Panel();
      this.label33 = new Label();
      this.btnSave = new StandardIconButton();
      this.pnlBody = new Panel();
      this.pnlLeft = new Panel();
      this.gcOranization = new GroupContainer();
      this.cboGenerateDisclosures = new ComboBox();
      this.chkMultiFactorAuth = new CheckBox();
      this.chkVisibleOnTPOWCsite = new CheckBox();
      this.label52 = new Label();
      this.txtOrgName = new TextBox();
      this.lblOrganizationName = new Label();
      this.chkDisable = new CheckBox();
      this.gcProductPricingInfo = new GroupContainer();
      this.cboRateSheet = new ComboBox();
      this.cboPriceGroupCorNonDel = new ComboBox();
      this.lblPriceGroupCorNonDel = new Label();
      this.cboPriceGroupCorDel = new ComboBox();
      this.lblPriceGroupCorDel = new Label();
      this.cboPriceGroupBroker = new ComboBox();
      this.lblPriceGroupBroker = new Label();
      this.cboEPPSCompModel = new ComboBox();
      this.lblRateSheet = new Label();
      this.pnlPML = new Panel();
      this.txtPMLCustomerCode = new TextBox();
      this.lblPMLCustomerCode = new Label();
      this.txtPMLPassword = new TextBox();
      this.lblPMLPassword = new Label();
      this.txtPMLUserName = new TextBox();
      this.lblPMLUserName = new Label();
      this.chkUseParentInfoForEPPS = new CheckBox();
      this.lblEPPSUserName = new Label();
      this.txtEPPSUserName = new TextBox();
      this.lblEPPSCompModel = new Label();
      this.gcCompanyDetails = new GroupContainer();
      this.chkCopyCompanyAddress = new CheckBox();
      this.ccbTimezone = new ComboBox();
      this.label55 = new Label();
      this.chkAcceptFirstPayments = new CheckBox();
      this.lblAcceptFirstPayments = new Label();
      this.chkNoAfterHourWires = new CheckBox();
      this.chkDelegated = new CheckBox();
      this.chkNonDelegated = new CheckBox();
      this.label50 = new Label();
      this.label49 = new Label();
      this.label48 = new Label();
      this.label47 = new Label();
      this.label46 = new Label();
      this.label45 = new Label();
      this.label44 = new Label();
      this.cboStateAddr = new ComboBox();
      this.cmbOrgType = new ComboBox();
      this.label39 = new Label();
      this.txtLastLoanSubmit = new TextBox();
      this.label9 = new Label();
      this.cboManager = new ComboBox();
      this.label8 = new Label();
      this.txtWebsite = new TextBox();
      this.txtEmail = new TextBox();
      this.label6 = new Label();
      this.label7 = new Label();
      this.txtFax = new TextBox();
      this.txtPhone = new TextBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.txtOwnerName = new TextBox();
      this.label3 = new Label();
      this.txtOrgID = new TextBox();
      this.label2 = new Label();
      this.chkUseParentInfoForCompany = new CheckBox();
      this.txtZip = new TextBox();
      this.chkBroker = new CheckBox();
      this.txtID = new TextBox();
      this.lblAddress = new Label();
      this.txtCity = new TextBox();
      this.txtName = new TextBox();
      this.lblCity = new Label();
      this.lblOriginatorName = new Label();
      this.txtAddress = new TextBox();
      this.lblState = new Label();
      this.lblName = new Label();
      this.lblZip = new Label();
      this.label22 = new Label();
      this.lblID = new Label();
      this.chkCorrespondent = new CheckBox();
      this.gcRateSheetLockInfo = new GroupContainer();
      this.chkUseParentInfoForRate = new CheckBox();
      this.txtEmailRateSheet = new TextBox();
      this.label10 = new Label();
      this.label11 = new Label();
      this.txtEmailLockInfo = new TextBox();
      this.txtFaxRateSheet = new TextBox();
      this.txtFaxLockInfo = new TextBox();
      this.label12 = new Label();
      this.label13 = new Label();
      this.pnlRight = new Panel();
      this.pnlRightTop = new Panel();
      this.gcSalesRepInfo = new GroupContainer();
      this.dpAeAssignedDate = new DatePicker();
      this.label54 = new Label();
      this.label51 = new Label();
      this.btnAddRep = new StandardIconButton();
      this.txtNameSalesRep = new TextBox();
      this.label38 = new Label();
      this.label41 = new Label();
      this.txtPhoneSalesRep = new TextBox();
      this.txtPersonaSalesRep = new TextBox();
      this.txtEmailSalesRep = new TextBox();
      this.label42 = new Label();
      this.label43 = new Label();
      this.gcApprovalStatus = new GroupContainer();
      this.cboCompanyRating = new ComboBox();
      this.label40 = new Label();
      this.chkAddWatchList = new CheckBox();
      this.dpApplicationDate = new DatePicker();
      this.dpApprovedDate = new DatePicker();
      this.dpActClosingDate = new DatePicker();
      this.cboCurrentStatus = new ComboBox();
      this.chkUseParentInfoForApproval = new CheckBox();
      this.label14 = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.label17 = new Label();
      this.pnlRightBottom = new Panel();
      this.gcBusinessInfo = new GroupContainer();
      this.txtLPAPassword = new TextBox();
      this.txtTPONumber = new TextBox();
      this.cboTypeofEntity = new ComboBox();
      this.txtEOCompany = new TextBox();
      this.label30 = new Label();
      this.dpEOExpirationDate = new DatePicker();
      this.label29 = new Label();
      this.cboCloseInOwnName = new ComboBox();
      this.cboFundInOwnName = new ComboBox();
      this.cboLPASponsored = new ComboBox();
      this.cboDUSponsored = new ComboBox();
      this.label35 = new Label();
      this.label24 = new Label();
      this.label23 = new Label();
      this.label1 = new Label();
      this.label36 = new Label();
      this.label37 = new Label();
      this.txtMERSOrgID = new TextBox();
      this.txtEOPolicy = new TextBox();
      this.label31 = new Label();
      this.label32 = new Label();
      this.txtCompanyNetWorth = new TextBox();
      this.label34 = new Label();
      this.dpFinancialLastUpdate = new DatePicker();
      this.label28 = new Label();
      this.label27 = new Label();
      this.txtFinancialPeriod = new TextBox();
      this.label53 = new Label();
      this.txtLEI = new TextBox();
      this.label26 = new Label();
      this.txtNMLSID = new TextBox();
      this.chkUseSSN = new CheckBox();
      this.label25 = new Label();
      this.txtTaxID = new TextBox();
      this.dpDateincorporation = new DatePicker();
      this.cboStateIncorp = new ComboBox();
      this.chkIncorporated = new CheckBox();
      this.chkUseParentInfoForBusiness = new CheckBox();
      this.label18 = new Label();
      this.label19 = new Label();
      this.txtEntityDescription = new TextBox();
      this.label20 = new Label();
      this.label21 = new Label();
      this.txtYrsInBusiness = new TextBox();
      this.label56 = new Label();
      this.txtBillAddress = new TextBox();
      this.txtBillCity = new TextBox();
      this.cboBillState = new ComboBox();
      this.txtBillZip = new TextBox();
      this.label57 = new Label();
      this.label59 = new Label();
      this.label61 = new Label();
      this.label62 = new Label();
      this.grpAll.SuspendLayout();
      ((ISupportInitialize) this.btnReset).BeginInit();
      this.panelHeader.SuspendLayout();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.pnlBody.SuspendLayout();
      this.pnlLeft.SuspendLayout();
      this.gcOranization.SuspendLayout();
      this.gcProductPricingInfo.SuspendLayout();
      this.pnlPML.SuspendLayout();
      this.gcCompanyDetails.SuspendLayout();
      this.gcRateSheetLockInfo.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.pnlRightTop.SuspendLayout();
      this.gcSalesRepInfo.SuspendLayout();
      ((ISupportInitialize) this.btnAddRep).BeginInit();
      this.gcApprovalStatus.SuspendLayout();
      this.pnlRightBottom.SuspendLayout();
      this.gcBusinessInfo.SuspendLayout();
      this.SuspendLayout();
      this.grpAll.Controls.Add((Control) this.btnReset);
      this.grpAll.Controls.Add((Control) this.panelHeader);
      this.grpAll.Controls.Add((Control) this.btnSave);
      this.grpAll.Controls.Add((Control) this.pnlBody);
      this.grpAll.Dock = DockStyle.Fill;
      this.grpAll.HeaderForeColor = SystemColors.ControlText;
      this.grpAll.Location = new Point(5, 5);
      this.grpAll.Margin = new Padding(0);
      this.grpAll.Name = "grpAll";
      this.grpAll.Size = new Size(862, 1101);
      this.grpAll.TabIndex = 29;
      this.grpAll.Text = "Company Information";
      this.grpAll.Resize += new EventHandler(this.grpAll_Resize);
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(836, 4);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 29;
      this.btnReset.TabStop = false;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.panelHeader.Controls.Add((Control) this.label33);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(860, 26);
      this.panelHeader.TabIndex = 0;
      this.label33.AutoSize = true;
      this.label33.Location = new Point(6, 6);
      this.label33.Name = "label33";
      this.label33.Size = new Size(370, 13);
      this.label33.TabIndex = 35;
      this.label33.Text = "Set up the basic information for the Third Party Originator company or branch.";
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(814, 4);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 28;
      this.btnSave.TabStop = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.pnlBody.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBody.AutoScroll = true;
      this.pnlBody.Controls.Add((Control) this.pnlLeft);
      this.pnlBody.Controls.Add((Control) this.pnlRight);
      this.pnlBody.Location = new Point(1, 58);
      this.pnlBody.Name = "pnlBody";
      this.pnlBody.Size = new Size(857, 1039);
      this.pnlBody.TabIndex = 1;
      this.pnlLeft.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlLeft.Controls.Add((Control) this.gcOranization);
      this.pnlLeft.Controls.Add((Control) this.gcProductPricingInfo);
      this.pnlLeft.Controls.Add((Control) this.gcCompanyDetails);
      this.pnlLeft.Controls.Add((Control) this.gcRateSheetLockInfo);
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Margin = new Padding(0);
      this.pnlLeft.MinimumSize = new Size(335, 981);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(365, 1051);
      this.pnlLeft.TabIndex = 36;
      this.gcOranization.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcOranization.Controls.Add((Control) this.cboGenerateDisclosures);
      this.gcOranization.Controls.Add((Control) this.chkMultiFactorAuth);
      this.gcOranization.Controls.Add((Control) this.chkVisibleOnTPOWCsite);
      this.gcOranization.Controls.Add((Control) this.label52);
      this.gcOranization.Controls.Add((Control) this.txtOrgName);
      this.gcOranization.Controls.Add((Control) this.lblOrganizationName);
      this.gcOranization.Controls.Add((Control) this.chkDisable);
      this.gcOranization.HeaderForeColor = SystemColors.ControlText;
      this.gcOranization.Location = new Point(5, 0);
      this.gcOranization.Margin = new Padding(0);
      this.gcOranization.Name = "gcOranization";
      this.gcOranization.Size = new Size(360, 196);
      this.gcOranization.TabIndex = 26;
      this.gcOranization.Text = "Organization";
      this.cboGenerateDisclosures.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboGenerateDisclosures.FormattingEnabled = true;
      this.cboGenerateDisclosures.Items.AddRange(new object[3]
      {
        (object) "Disable Fee Management",
        (object) "Request LE & Disclosures",
        (object) "Generate LE & Disclosures"
      });
      this.cboGenerateDisclosures.Location = new Point(145, 101);
      this.cboGenerateDisclosures.Name = "cboGenerateDisclosures";
      this.cboGenerateDisclosures.Size = new Size(162, 21);
      this.cboGenerateDisclosures.TabIndex = 45;
      this.cboGenerateDisclosures.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cboGenerateDisclosures.TextChanged += new EventHandler(this.textField_Changed);
      this.chkMultiFactorAuth.AutoSize = true;
      this.chkMultiFactorAuth.BackColor = Color.Transparent;
      this.chkMultiFactorAuth.Location = new Point(145, 80);
      this.chkMultiFactorAuth.Name = "chkMultiFactorAuth";
      this.chkMultiFactorAuth.Size = new Size(152, 17);
      this.chkMultiFactorAuth.TabIndex = 44;
      this.chkMultiFactorAuth.Text = "Multi-Factor Authentication";
      this.chkMultiFactorAuth.UseVisualStyleBackColor = false;
      this.chkMultiFactorAuth.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkVisibleOnTPOWCsite.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.chkVisibleOnTPOWCsite.AutoSize = true;
      this.chkVisibleOnTPOWCsite.BackColor = Color.Transparent;
      this.chkVisibleOnTPOWCsite.Location = new Point(145, (int) sbyte.MaxValue);
      this.chkVisibleOnTPOWCsite.Name = "chkVisibleOnTPOWCsite";
      this.chkVisibleOnTPOWCsite.Size = new Size(133, 17);
      this.chkVisibleOnTPOWCsite.TabIndex = 43;
      this.chkVisibleOnTPOWCsite.Text = "Visible on TPOWC site";
      this.chkVisibleOnTPOWCsite.UseVisualStyleBackColor = false;
      this.chkVisibleOnTPOWCsite.CheckedChanged += new EventHandler(this.chkVisibleOnTPOWCsite_CheckedChanged);
      this.label52.AutoSize = true;
      this.label52.BackColor = Color.Transparent;
      this.label52.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label52.ForeColor = Color.Red;
      this.label52.Location = new Point(101, 34);
      this.label52.Name = "label52";
      this.label52.Size = new Size(13, 16);
      this.label52.TabIndex = 42;
      this.label52.Text = "*";
      this.txtOrgName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOrgName.Location = new Point(145, 31);
      this.txtOrgName.MaxLength = 128;
      this.txtOrgName.Name = "txtOrgName";
      this.txtOrgName.Size = new Size(207, 20);
      this.txtOrgName.TabIndex = 26;
      this.txtOrgName.TextChanged += new EventHandler(this.textField_Changed);
      this.lblOrganizationName.AutoSize = true;
      this.lblOrganizationName.Location = new Point(6, 34);
      this.lblOrganizationName.Name = "lblOrganizationName";
      this.lblOrganizationName.Size = new Size(97, 13);
      this.lblOrganizationName.TabIndex = 28;
      this.lblOrganizationName.Text = "Organization Name";
      this.chkDisable.AutoSize = true;
      this.chkDisable.BackColor = Color.Transparent;
      this.chkDisable.Location = new Point(145, 58);
      this.chkDisable.Name = "chkDisable";
      this.chkDisable.Size = new Size(90, 17);
      this.chkDisable.TabIndex = 0;
      this.chkDisable.Text = "Disable Login";
      this.chkDisable.UseVisualStyleBackColor = false;
      this.chkDisable.CheckedChanged += new EventHandler(this.textField_Changed);
      this.gcProductPricingInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcProductPricingInfo.Controls.Add((Control) this.cboRateSheet);
      this.gcProductPricingInfo.Controls.Add((Control) this.cboPriceGroupCorNonDel);
      this.gcProductPricingInfo.Controls.Add((Control) this.lblPriceGroupCorNonDel);
      this.gcProductPricingInfo.Controls.Add((Control) this.cboPriceGroupCorDel);
      this.gcProductPricingInfo.Controls.Add((Control) this.lblPriceGroupCorDel);
      this.gcProductPricingInfo.Controls.Add((Control) this.cboPriceGroupBroker);
      this.gcProductPricingInfo.Controls.Add((Control) this.lblPriceGroupBroker);
      this.gcProductPricingInfo.Controls.Add((Control) this.cboEPPSCompModel);
      this.gcProductPricingInfo.Controls.Add((Control) this.lblRateSheet);
      this.gcProductPricingInfo.Controls.Add((Control) this.pnlPML);
      this.gcProductPricingInfo.Controls.Add((Control) this.chkUseParentInfoForEPPS);
      this.gcProductPricingInfo.Controls.Add((Control) this.lblEPPSUserName);
      this.gcProductPricingInfo.Controls.Add((Control) this.txtEPPSUserName);
      this.gcProductPricingInfo.Controls.Add((Control) this.lblEPPSCompModel);
      this.gcProductPricingInfo.HeaderForeColor = SystemColors.ControlText;
      this.gcProductPricingInfo.Location = new Point(5, 903);
      this.gcProductPricingInfo.Margin = new Padding(0);
      this.gcProductPricingInfo.Name = "gcProductPricingInfo";
      this.gcProductPricingInfo.Size = new Size(360, 178);
      this.gcProductPricingInfo.TabIndex = 25;
      this.gcProductPricingInfo.Text = "Product and Pricing";
      this.cboRateSheet.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboRateSheet.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRateSheet.FormattingEnabled = true;
      this.cboRateSheet.Location = new Point(143, 147);
      this.cboRateSheet.Name = "cboRateSheet";
      this.cboRateSheet.Size = new Size(208, 21);
      this.cboRateSheet.TabIndex = 36;
      this.cboRateSheet.Visible = false;
      this.cboRateSheet.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cboRateSheet.TextChanged += new EventHandler(this.textField_Changed);
      this.cboPriceGroupCorNonDel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboPriceGroupCorNonDel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPriceGroupCorNonDel.FormattingEnabled = true;
      this.cboPriceGroupCorNonDel.Location = new Point(143, 79);
      this.cboPriceGroupCorNonDel.Name = "cboPriceGroupCorNonDel";
      this.cboPriceGroupCorNonDel.Size = new Size(208, 21);
      this.cboPriceGroupCorNonDel.TabIndex = 28;
      this.cboPriceGroupCorNonDel.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cboPriceGroupCorNonDel.TextChanged += new EventHandler(this.textField_Changed);
      this.lblPriceGroupCorNonDel.AutoSize = true;
      this.lblPriceGroupCorNonDel.Location = new Point(6, 83);
      this.lblPriceGroupCorNonDel.Name = "lblPriceGroupCorNonDel";
      this.lblPriceGroupCorNonDel.Size = new Size(130, 13);
      this.lblPriceGroupCorNonDel.TabIndex = 34;
      this.lblPriceGroupCorNonDel.Text = "Price Group (Cor-Non-Del)";
      this.cboPriceGroupCorDel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboPriceGroupCorDel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPriceGroupCorDel.FormattingEnabled = true;
      this.cboPriceGroupCorDel.Location = new Point(143, 54);
      this.cboPriceGroupCorDel.Name = "cboPriceGroupCorDel";
      this.cboPriceGroupCorDel.Size = new Size(208, 21);
      this.cboPriceGroupCorDel.TabIndex = 28;
      this.cboPriceGroupCorDel.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cboPriceGroupCorDel.TextChanged += new EventHandler(this.textField_Changed);
      this.lblPriceGroupCorDel.AutoSize = true;
      this.lblPriceGroupCorDel.Location = new Point(6, 58);
      this.lblPriceGroupCorDel.Name = "lblPriceGroupCorDel";
      this.lblPriceGroupCorDel.Size = new Size(107, 13);
      this.lblPriceGroupCorDel.TabIndex = 34;
      this.lblPriceGroupCorDel.Text = "Price Group (Cor-Del)";
      this.cboPriceGroupBroker.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboPriceGroupBroker.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPriceGroupBroker.FormattingEnabled = true;
      this.cboPriceGroupBroker.Location = new Point(143, 29);
      this.cboPriceGroupBroker.Name = "cboPriceGroupBroker";
      this.cboPriceGroupBroker.Size = new Size(208, 21);
      this.cboPriceGroupBroker.TabIndex = 28;
      this.cboPriceGroupBroker.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cboPriceGroupBroker.TextChanged += new EventHandler(this.textField_Changed);
      this.lblPriceGroupBroker.AutoSize = true;
      this.lblPriceGroupBroker.Location = new Point(6, 33);
      this.lblPriceGroupBroker.Name = "lblPriceGroupBroker";
      this.lblPriceGroupBroker.Size = new Size(103, 13);
      this.lblPriceGroupBroker.TabIndex = 34;
      this.lblPriceGroupBroker.Text = "Price Group (Broker)";
      this.cboEPPSCompModel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboEPPSCompModel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboEPPSCompModel.FormattingEnabled = true;
      this.cboEPPSCompModel.Items.AddRange(new object[3]
      {
        (object) "Borrower Only",
        (object) "Creditor Only",
        (object) "Borrower or Creditor"
      });
      this.cboEPPSCompModel.Location = new Point(143, 124);
      this.cboEPPSCompModel.Name = "cboEPPSCompModel";
      this.cboEPPSCompModel.Size = new Size(208, 21);
      this.cboEPPSCompModel.TabIndex = 27;
      this.cboEPPSCompModel.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cboEPPSCompModel.TextChanged += new EventHandler(this.textField_Changed);
      this.lblRateSheet.AutoSize = true;
      this.lblRateSheet.Location = new Point(6, 146);
      this.lblRateSheet.Name = "lblRateSheet";
      this.lblRateSheet.Size = new Size(61, 13);
      this.lblRateSheet.TabIndex = 35;
      this.lblRateSheet.Text = "Rate Sheet";
      this.lblRateSheet.Visible = false;
      this.pnlPML.Controls.Add((Control) this.txtPMLCustomerCode);
      this.pnlPML.Controls.Add((Control) this.lblPMLCustomerCode);
      this.pnlPML.Controls.Add((Control) this.txtPMLPassword);
      this.pnlPML.Controls.Add((Control) this.lblPMLPassword);
      this.pnlPML.Controls.Add((Control) this.txtPMLUserName);
      this.pnlPML.Controls.Add((Control) this.lblPMLUserName);
      this.pnlPML.Dock = DockStyle.Bottom;
      this.pnlPML.Location = new Point(1, 101);
      this.pnlPML.Name = "pnlPML";
      this.pnlPML.Size = new Size(358, 76);
      this.pnlPML.TabIndex = 35;
      this.pnlPML.Visible = false;
      this.txtPMLCustomerCode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPMLCustomerCode.Location = new Point(140, 49);
      this.txtPMLCustomerCode.MaxLength = 64;
      this.txtPMLCustomerCode.Name = "txtPMLCustomerCode";
      this.txtPMLCustomerCode.Size = new Size(209, 20);
      this.txtPMLCustomerCode.TabIndex = 34;
      this.txtPMLCustomerCode.TextChanged += new EventHandler(this.textField_Changed);
      this.lblPMLCustomerCode.AutoSize = true;
      this.lblPMLCustomerCode.Location = new Point(3, 51);
      this.lblPMLCustomerCode.Name = "lblPMLCustomerCode";
      this.lblPMLCustomerCode.Size = new Size(79, 13);
      this.lblPMLCustomerCode.TabIndex = 33;
      this.lblPMLCustomerCode.Text = "Customer Code";
      this.txtPMLPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPMLPassword.Location = new Point(140, 26);
      this.txtPMLPassword.MaxLength = 32;
      this.txtPMLPassword.Name = "txtPMLPassword";
      this.txtPMLPassword.PasswordChar = '*';
      this.txtPMLPassword.Size = new Size(209, 20);
      this.txtPMLPassword.TabIndex = 32;
      this.txtPMLPassword.TextChanged += new EventHandler(this.textField_Changed);
      this.lblPMLPassword.AutoSize = true;
      this.lblPMLPassword.Location = new Point(3, 29);
      this.lblPMLPassword.Name = "lblPMLPassword";
      this.lblPMLPassword.Size = new Size(53, 13);
      this.lblPMLPassword.TabIndex = 31;
      this.lblPMLPassword.Text = "Password";
      this.txtPMLUserName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPMLUserName.Location = new Point(140, 3);
      this.txtPMLUserName.MaxLength = 32;
      this.txtPMLUserName.Name = "txtPMLUserName";
      this.txtPMLUserName.Size = new Size(209, 20);
      this.txtPMLUserName.TabIndex = 30;
      this.txtPMLUserName.TextChanged += new EventHandler(this.textField_Changed);
      this.lblPMLUserName.AutoSize = true;
      this.lblPMLUserName.Location = new Point(3, 6);
      this.lblPMLUserName.Name = "lblPMLUserName";
      this.lblPMLUserName.Size = new Size(60, 13);
      this.lblPMLUserName.TabIndex = 29;
      this.lblPMLUserName.Text = "User Name";
      this.chkUseParentInfoForEPPS.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfoForEPPS.AutoSize = true;
      this.chkUseParentInfoForEPPS.BackColor = Color.Transparent;
      this.chkUseParentInfoForEPPS.Location = new Point((int) byte.MaxValue, 5);
      this.chkUseParentInfoForEPPS.Name = "chkUseParentInfoForEPPS";
      this.chkUseParentInfoForEPPS.Size = new Size(100, 17);
      this.chkUseParentInfoForEPPS.TabIndex = 25;
      this.chkUseParentInfoForEPPS.Text = "Use Parent Info";
      this.chkUseParentInfoForEPPS.UseVisualStyleBackColor = false;
      this.chkUseParentInfoForEPPS.CheckedChanged += new EventHandler(this.chkUseParentInfoForEPPS_CheckedChanged);
      this.lblEPPSUserName.AutoSize = true;
      this.lblEPPSUserName.Location = new Point(6, 104);
      this.lblEPPSUserName.Name = "lblEPPSUserName";
      this.lblEPPSUserName.Size = new Size(91, 13);
      this.lblEPPSUserName.TabIndex = 28;
      this.lblEPPSUserName.Text = "ICE PPE User Name";
      this.txtEPPSUserName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEPPSUserName.Location = new Point(143, 102);
      this.txtEPPSUserName.MaxLength = 64;
      this.txtEPPSUserName.Name = "txtEPPSUserName";
      this.txtEPPSUserName.Size = new Size(208, 20);
      this.txtEPPSUserName.TabIndex = 26;
      this.txtEPPSUserName.TextChanged += new EventHandler(this.textField_Changed);
      this.lblEPPSCompModel.AutoSize = true;
      this.lblEPPSCompModel.Location = new Point(6, 125);
      this.lblEPPSCompModel.Name = "lblEPPSCompModel";
      this.lblEPPSCompModel.Size = new Size(100, 13);
      this.lblEPPSCompModel.TabIndex = 22;
      this.lblEPPSCompModel.Text = "ICE PPE Comp. Model";
      this.gcCompanyDetails.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcCompanyDetails.Controls.Add((Control) this.label62);
      this.gcCompanyDetails.Controls.Add((Control) this.chkCopyCompanyAddress);
      this.gcCompanyDetails.Controls.Add((Control) this.label61);
      this.gcCompanyDetails.Controls.Add((Control) this.label59);
      this.gcCompanyDetails.Controls.Add((Control) this.label57);
      this.gcCompanyDetails.Controls.Add((Control) this.txtBillZip);
      this.gcCompanyDetails.Controls.Add((Control) this.cboBillState);
      this.gcCompanyDetails.Controls.Add((Control) this.txtBillCity);
      this.gcCompanyDetails.Controls.Add((Control) this.txtBillAddress);
      this.gcCompanyDetails.Controls.Add((Control) this.ccbTimezone);
      this.gcCompanyDetails.Controls.Add((Control) this.label55);
      this.gcCompanyDetails.Controls.Add((Control) this.chkAcceptFirstPayments);
      this.gcCompanyDetails.Controls.Add((Control) this.lblAcceptFirstPayments);
      this.gcCompanyDetails.Controls.Add((Control) this.chkNoAfterHourWires);
      this.gcCompanyDetails.Controls.Add((Control) this.chkDelegated);
      this.gcCompanyDetails.Controls.Add((Control) this.chkNonDelegated);
      this.gcCompanyDetails.Controls.Add((Control) this.label50);
      this.gcCompanyDetails.Controls.Add((Control) this.label49);
      this.gcCompanyDetails.Controls.Add((Control) this.label48);
      this.gcCompanyDetails.Controls.Add((Control) this.label47);
      this.gcCompanyDetails.Controls.Add((Control) this.label46);
      this.gcCompanyDetails.Controls.Add((Control) this.label45);
      this.gcCompanyDetails.Controls.Add((Control) this.label44);
      this.gcCompanyDetails.Controls.Add((Control) this.cboStateAddr);
      this.gcCompanyDetails.Controls.Add((Control) this.cmbOrgType);
      this.gcCompanyDetails.Controls.Add((Control) this.label39);
      this.gcCompanyDetails.Controls.Add((Control) this.txtLastLoanSubmit);
      this.gcCompanyDetails.Controls.Add((Control) this.label9);
      this.gcCompanyDetails.Controls.Add((Control) this.cboManager);
      this.gcCompanyDetails.Controls.Add((Control) this.label8);
      this.gcCompanyDetails.Controls.Add((Control) this.txtWebsite);
      this.gcCompanyDetails.Controls.Add((Control) this.txtEmail);
      this.gcCompanyDetails.Controls.Add((Control) this.label6);
      this.gcCompanyDetails.Controls.Add((Control) this.label7);
      this.gcCompanyDetails.Controls.Add((Control) this.txtFax);
      this.gcCompanyDetails.Controls.Add((Control) this.txtPhone);
      this.gcCompanyDetails.Controls.Add((Control) this.label4);
      this.gcCompanyDetails.Controls.Add((Control) this.label5);
      this.gcCompanyDetails.Controls.Add((Control) this.txtOwnerName);
      this.gcCompanyDetails.Controls.Add((Control) this.label3);
      this.gcCompanyDetails.Controls.Add((Control) this.txtOrgID);
      this.gcCompanyDetails.Controls.Add((Control) this.label2);
      this.gcCompanyDetails.Controls.Add((Control) this.chkUseParentInfoForCompany);
      this.gcCompanyDetails.Controls.Add((Control) this.txtZip);
      this.gcCompanyDetails.Controls.Add((Control) this.chkBroker);
      this.gcCompanyDetails.Controls.Add((Control) this.txtID);
      this.gcCompanyDetails.Controls.Add((Control) this.lblAddress);
      this.gcCompanyDetails.Controls.Add((Control) this.txtCity);
      this.gcCompanyDetails.Controls.Add((Control) this.txtName);
      this.gcCompanyDetails.Controls.Add((Control) this.lblCity);
      this.gcCompanyDetails.Controls.Add((Control) this.lblOriginatorName);
      this.gcCompanyDetails.Controls.Add((Control) this.txtAddress);
      this.gcCompanyDetails.Controls.Add((Control) this.lblState);
      this.gcCompanyDetails.Controls.Add((Control) this.lblName);
      this.gcCompanyDetails.Controls.Add((Control) this.lblZip);
      this.gcCompanyDetails.Controls.Add((Control) this.label22);
      this.gcCompanyDetails.Controls.Add((Control) this.lblID);
      this.gcCompanyDetails.Controls.Add((Control) this.chkCorrespondent);
      this.gcCompanyDetails.HeaderForeColor = SystemColors.ControlText;
      this.gcCompanyDetails.Location = new Point(5, 198);
      this.gcCompanyDetails.Margin = new Padding(0);
      this.gcCompanyDetails.Name = "gcCompanyDetails";
      this.gcCompanyDetails.Size = new Size(360, 550);
      this.gcCompanyDetails.TabIndex = 23;
      this.gcCompanyDetails.Text = "Company Details";
      this.ccbTimezone.FormattingEnabled = true;
      this.ccbTimezone.Items.AddRange(new object[7]
      {
        (object) "(UTC -10:00) Hawaii Time",
        (object) "(UTC -09:00) Alaska Time",
        (object) "(UTC -08:00) Pacific Time",
        (object) "(UTC -07:00) Arizona Time",
        (object) "(UTC -07:00) Mountain Time",
        (object) "(UTC -06:00) Central Time",
        (object) "(UTC -05:00) Eastern Time"
      });
      this.ccbTimezone.Location = new Point(144, 114);
      this.ccbTimezone.Name = "ccbTimezone";
      this.ccbTimezone.Size = new Size(163, 21);
      this.ccbTimezone.TabIndex = 48;
      this.ccbTimezone.SelectedIndexChanged += new EventHandler(this.ccbTimezone_SelectedIndexChanged);
      this.label55.AutoSize = true;
      this.label55.Location = new Point(4, 114);
      this.label55.Name = "label55";
      this.label55.Size = new Size(53, 13);
      this.label55.TabIndex = 47;
      this.label55.Text = "Timezone";
      this.chkAcceptFirstPayments.AutoSize = true;
      this.chkAcceptFirstPayments.Location = new Point(145, 519);
      this.chkAcceptFirstPayments.Name = "chkAcceptFirstPayments";
      this.chkAcceptFirstPayments.Size = new Size(15, 14);
      this.chkAcceptFirstPayments.TabIndex = 25;
      this.chkAcceptFirstPayments.UseVisualStyleBackColor = true;
      this.chkAcceptFirstPayments.CheckedChanged += new EventHandler(this.chkAcceptFirstPayments_CheckedChanged);
      this.lblAcceptFirstPayments.Location = new Point(6, 517);
      this.lblAcceptFirstPayments.Name = "lblAcceptFirstPayments";
      this.lblAcceptFirstPayments.Size = new Size(130, 30);
      this.lblAcceptFirstPayments.TabIndex = 45;
      this.lblAcceptFirstPayments.Text = "Company can accept first payments";
      this.chkNoAfterHourWires.AutoSize = true;
      this.chkNoAfterHourWires.Location = new Point(145, 139);
      this.chkNoAfterHourWires.Name = "chkNoAfterHourWires";
      this.chkNoAfterHourWires.Size = new Size(15, 14);
      this.chkNoAfterHourWires.TabIndex = 43;
      this.chkNoAfterHourWires.UseVisualStyleBackColor = true;
      this.chkNoAfterHourWires.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkDelegated.AutoSize = true;
      this.chkDelegated.Enabled = false;
      this.chkDelegated.Location = new Point(144, 91);
      this.chkDelegated.Name = "chkDelegated";
      this.chkDelegated.Size = new Size(75, 17);
      this.chkDelegated.TabIndex = 43;
      this.chkDelegated.Text = "Delegated";
      this.chkDelegated.UseVisualStyleBackColor = true;
      this.chkDelegated.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkNonDelegated.AutoSize = true;
      this.chkNonDelegated.Enabled = false;
      this.chkNonDelegated.Location = new Point(233, 91);
      this.chkNonDelegated.Name = "chkNonDelegated";
      this.chkNonDelegated.Size = new Size(98, 17);
      this.chkNonDelegated.TabIndex = 44;
      this.chkNonDelegated.Text = "Non-Delegated";
      this.chkNonDelegated.UseVisualStyleBackColor = true;
      this.chkNonDelegated.CheckedChanged += new EventHandler(this.textField_Changed);
      this.label50.AutoSize = true;
      this.label50.BackColor = Color.Transparent;
      this.label50.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label50.ForeColor = Color.Red;
      this.label50.Location = new Point(78, 57);
      this.label50.Name = "label50";
      this.label50.Size = new Size(13, 16);
      this.label50.TabIndex = 41;
      this.label50.Text = "*";
      this.label49.AutoSize = true;
      this.label49.BackColor = Color.Transparent;
      this.label49.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label49.ForeColor = Color.Red;
      this.label49.Location = new Point(243, 298);
      this.label49.Name = "label49";
      this.label49.Size = new Size(13, 16);
      this.label49.TabIndex = 40;
      this.label49.Text = "*";
      this.label48.AutoSize = true;
      this.label48.BackColor = Color.Transparent;
      this.label48.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label48.ForeColor = Color.Red;
      this.label48.Location = new Point(84, 385);
      this.label48.Name = "label48";
      this.label48.Size = new Size(13, 16);
      this.label48.TabIndex = 39;
      this.label48.Text = "*";
      this.label47.AutoSize = true;
      this.label47.BackColor = Color.Transparent;
      this.label47.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label47.ForeColor = Color.Red;
      this.label47.Location = new Point(39, 294);
      this.label47.Name = "label47";
      this.label47.Size = new Size(13, 16);
      this.label47.TabIndex = 38;
      this.label47.Text = "*";
      this.label46.AutoSize = true;
      this.label46.BackColor = Color.Transparent;
      this.label46.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label46.ForeColor = Color.Red;
      this.label46.Location = new Point(32, 272);
      this.label46.Name = "label46";
      this.label46.Size = new Size(13, 16);
      this.label46.TabIndex = 37;
      this.label46.Text = "*";
      this.label45.AutoSize = true;
      this.label45.BackColor = Color.Transparent;
      this.label45.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label45.ForeColor = Color.Red;
      this.label45.Location = new Point(55, 251);
      this.label45.Name = "label45";
      this.label45.Size = new Size(13, 16);
      this.label45.TabIndex = 36;
      this.label45.Text = "*";
      this.label44.AutoSize = true;
      this.label44.BackColor = Color.Transparent;
      this.label44.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label44.ForeColor = Color.Red;
      this.label44.Location = new Point(116, 231);
      this.label44.Name = "label44";
      this.label44.Size = new Size(13, 16);
      this.label44.TabIndex = 35;
      this.label44.Text = "*";
      this.cboStateAddr.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStateAddr.FormattingEnabled = true;
      this.cboStateAddr.Location = new Point(144, 294);
      this.cboStateAddr.Name = "cboStateAddr";
      this.cboStateAddr.Size = new Size(56, 21);
      this.cboStateAddr.TabIndex = 12;
      this.cboStateAddr.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cboStateAddr.TextChanged += new EventHandler(this.textField_Changed);
      this.cmbOrgType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbOrgType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbOrgType.FormattingEnabled = true;
      this.cmbOrgType.Location = new Point(144, 30);
      this.cmbOrgType.Name = "cmbOrgType";
      this.cmbOrgType.Size = new Size(211, 21);
      this.cmbOrgType.TabIndex = 2;
      this.cmbOrgType.SelectedIndexChanged += new EventHandler(this.cmbOrgType_SelectedIndexChanged);
      this.label39.AutoSize = true;
      this.label39.Location = new Point(6, 33);
      this.label39.Name = "label39";
      this.label39.Size = new Size(93, 13);
      this.label39.TabIndex = 34;
      this.label39.Text = "Organization Type";
      this.txtLastLoanSubmit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtLastLoanSubmit.Enabled = false;
      this.txtLastLoanSubmit.Location = new Point(144, 495);
      this.txtLastLoanSubmit.MaxLength = 64;
      this.txtLastLoanSubmit.Name = "txtLastLoanSubmit";
      this.txtLastLoanSubmit.ReadOnly = true;
      this.txtLastLoanSubmit.Size = new Size(209, 20);
      this.txtLastLoanSubmit.TabIndex = 24;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(6, 498);
      this.label9.Name = "label9";
      this.label9.Size = new Size(130, 13);
      this.label9.TabIndex = 32;
      this.label9.Text = "Last Loan Submitted Date";
      this.cboManager.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboManager.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboManager.FormattingEnabled = true;
      this.cboManager.Location = new Point(144, 472);
      this.cboManager.Name = "cboManager";
      this.cboManager.Size = new Size(209, 21);
      this.cboManager.TabIndex = 23;
      this.cboManager.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(6, 475);
      this.label8.Name = "label8";
      this.label8.Size = new Size(74, 13);
      this.label8.TabIndex = 30;
      this.label8.Text = "TPO Manager";
      this.txtWebsite.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtWebsite.Location = new Point(144, 450);
      this.txtWebsite.MaxLength = 50;
      this.txtWebsite.Name = "txtWebsite";
      this.txtWebsite.Size = new Size(209, 20);
      this.txtWebsite.TabIndex = 22;
      this.txtWebsite.TextChanged += new EventHandler(this.textField_Changed);
      this.txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEmail.Location = new Point(144, 428);
      this.txtEmail.MaxLength = 64;
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(209, 20);
      this.txtEmail.TabIndex = 21;
      this.txtEmail.TextChanged += new EventHandler(this.textField_Changed);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 431);
      this.label6.Name = "label6";
      this.label6.Size = new Size(79, 13);
      this.label6.TabIndex = 26;
      this.label6.Text = "Company Email";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 453);
      this.label7.Name = "label7";
      this.label7.Size = new Size(46, 13);
      this.label7.TabIndex = 28;
      this.label7.Text = "Website";
      this.txtFax.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFax.Location = new Point(144, 406);
      this.txtFax.MaxLength = 30;
      this.txtFax.Name = "txtFax";
      this.txtFax.Size = new Size(209, 20);
      this.txtFax.TabIndex = 20;
      this.txtFax.TextChanged += new EventHandler(this.textField_Changed);
      this.txtPhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPhone.Location = new Point(144, 384);
      this.txtPhone.MaxLength = 30;
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(209, 20);
      this.txtPhone.TabIndex = 19;
      this.txtPhone.TextChanged += new EventHandler(this.textField_Changed);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 387);
      this.label4.Name = "label4";
      this.label4.Size = new Size(78, 13);
      this.label4.TabIndex = 22;
      this.label4.Text = "Phone Number";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 409);
      this.label5.Name = "label5";
      this.label5.Size = new Size(64, 13);
      this.label5.TabIndex = 24;
      this.label5.Text = "Fax Number";
      this.txtOwnerName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOwnerName.Location = new Point(144, 206);
      this.txtOwnerName.MaxLength = 64;
      this.txtOwnerName.Name = "txtOwnerName";
      this.txtOwnerName.Size = new Size(209, 20);
      this.txtOwnerName.TabIndex = 7;
      this.txtOwnerName.TextChanged += new EventHandler(this.textField_Changed);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 209);
      this.label3.Name = "label3";
      this.label3.Size = new Size(123, 13);
      this.label3.TabIndex = 20;
      this.label3.Text = "Company Owner's Name";
      this.txtOrgID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOrgID.Location = new Point(144, 184);
      this.txtOrgID.MaxLength = 25;
      this.txtOrgID.Name = "txtOrgID";
      this.txtOrgID.Size = new Size(209, 20);
      this.txtOrgID.TabIndex = 6;
      this.txtOrgID.TextChanged += new EventHandler(this.textField_Changed);
      this.txtOrgID.KeyPress += new KeyPressEventHandler(this.txtOrgID_KeyPress);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 187);
      this.label2.Name = "label2";
      this.label2.Size = new Size(80, 13);
      this.label2.TabIndex = 18;
      this.label2.Text = "Organization ID";
      this.chkUseParentInfoForCompany.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfoForCompany.AutoSize = true;
      this.chkUseParentInfoForCompany.BackColor = Color.Transparent;
      this.chkUseParentInfoForCompany.Location = new Point((int) byte.MaxValue, 5);
      this.chkUseParentInfoForCompany.Name = "chkUseParentInfoForCompany";
      this.chkUseParentInfoForCompany.Size = new Size(100, 17);
      this.chkUseParentInfoForCompany.TabIndex = 1;
      this.chkUseParentInfoForCompany.Text = "Use Parent Info";
      this.chkUseParentInfoForCompany.UseVisualStyleBackColor = false;
      this.chkUseParentInfoForCompany.CheckedChanged += new EventHandler(this.chkUseParentInfoForCompany_CheckedChanged);
      this.txtZip.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtZip.Location = new Point(269, 294);
      this.txtZip.MaxLength = 15;
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new Size(84, 20);
      this.txtZip.TabIndex = 13;
      this.txtZip.Tag = (object) "2284";
      this.txtZip.TextChanged += new EventHandler(this.textField_Changed);
      this.chkBroker.AutoSize = true;
      this.chkBroker.Location = new Point(144, 55);
      this.chkBroker.Name = "chkBroker";
      this.chkBroker.Size = new Size(57, 17);
      this.chkBroker.TabIndex = 3;
      this.chkBroker.Text = "Broker";
      this.chkBroker.UseVisualStyleBackColor = true;
      this.chkBroker.CheckedChanged += new EventHandler(this.CompType_CheckedChanged);
      this.txtID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtID.Enabled = false;
      this.txtID.Location = new Point(144, 162);
      this.txtID.Name = "txtID";
      this.txtID.Size = new Size(209, 20);
      this.txtID.TabIndex = 5;
      this.txtID.TextChanged += new EventHandler(this.textField_Changed);
      this.lblAddress.AutoSize = true;
      this.lblAddress.Location = new Point(6, 253);
      this.lblAddress.Name = "lblAddress";
      this.lblAddress.Size = new Size(45, 13);
      this.lblAddress.TabIndex = 11;
      this.lblAddress.Text = "Address";
      this.txtCity.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCity.Location = new Point(144, 272);
      this.txtCity.MaxLength = 50;
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size(209, 20);
      this.txtCity.TabIndex = 11;
      this.txtCity.TextChanged += new EventHandler(this.textField_Changed);
      this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtName.Location = new Point(144, 228);
      this.txtName.MaxLength = 128;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(209, 20);
      this.txtName.TabIndex = 8;
      this.txtName.TextChanged += new EventHandler(this.textField_Changed);
      this.lblCity.AutoSize = true;
      this.lblCity.Location = new Point(6, 274);
      this.lblCity.Name = "lblCity";
      this.lblCity.Size = new Size(24, 13);
      this.lblCity.TabIndex = 13;
      this.lblCity.Text = "City";
      this.lblOriginatorName.AutoSize = true;
      this.lblOriginatorName.Location = new Point(6, 55);
      this.lblOriginatorName.Name = "lblOriginatorName";
      this.lblOriginatorName.Size = new Size(73, 13);
      this.lblOriginatorName.TabIndex = 0;
      this.lblOriginatorName.Text = "Channel Type";
      this.txtAddress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAddress.Location = new Point(144, 250);
      this.txtAddress.MaxLength = 100;
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = new Size(209, 20);
      this.txtAddress.TabIndex = 10;
      this.txtAddress.TextChanged += new EventHandler(this.textField_Changed);
      this.lblState.AutoSize = true;
      this.lblState.Location = new Point(6, 296);
      this.lblState.Name = "lblState";
      this.lblState.Size = new Size(32, 13);
      this.lblState.TabIndex = 14;
      this.lblState.Text = "State";
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(6, 231);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(111, 13);
      this.lblName.TabIndex = 7;
      this.lblName.Text = "Company Legal Name";
      this.lblZip.AutoSize = true;
      this.lblZip.Location = new Point(223, 297);
      this.lblZip.Name = "lblZip";
      this.lblZip.Size = new Size(22, 13);
      this.lblZip.TabIndex = 16;
      this.lblZip.Text = "Zip";
      this.label22.AutoSize = true;
      this.label22.Location = new Point(6, 139);
      this.label22.Name = "label22";
      this.label22.Size = new Size(102, 13);
      this.label22.TabIndex = 5;
      this.label22.Text = "No After Hour Wires";
      this.lblID.AutoSize = true;
      this.lblID.Location = new Point(6, 165);
      this.lblID.Name = "lblID";
      this.lblID.Size = new Size(43, 13);
      this.lblID.TabIndex = 5;
      this.lblID.Text = "TPO ID";
      this.chkCorrespondent.AutoSize = true;
      this.chkCorrespondent.Location = new Point(144, 72);
      this.chkCorrespondent.Name = "chkCorrespondent";
      this.chkCorrespondent.Size = new Size(95, 17);
      this.chkCorrespondent.TabIndex = 4;
      this.chkCorrespondent.Text = "Correspondent";
      this.chkCorrespondent.UseVisualStyleBackColor = true;
      this.chkCorrespondent.CheckedChanged += new EventHandler(this.CompType_CheckedChanged);
      this.gcRateSheetLockInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcRateSheetLockInfo.Controls.Add((Control) this.chkUseParentInfoForRate);
      this.gcRateSheetLockInfo.Controls.Add((Control) this.txtEmailRateSheet);
      this.gcRateSheetLockInfo.Controls.Add((Control) this.label10);
      this.gcRateSheetLockInfo.Controls.Add((Control) this.label11);
      this.gcRateSheetLockInfo.Controls.Add((Control) this.txtEmailLockInfo);
      this.gcRateSheetLockInfo.Controls.Add((Control) this.txtFaxRateSheet);
      this.gcRateSheetLockInfo.Controls.Add((Control) this.txtFaxLockInfo);
      this.gcRateSheetLockInfo.Controls.Add((Control) this.label12);
      this.gcRateSheetLockInfo.Controls.Add((Control) this.label13);
      this.gcRateSheetLockInfo.HeaderForeColor = SystemColors.ControlText;
      this.gcRateSheetLockInfo.Location = new Point(5, 751);
      this.gcRateSheetLockInfo.Margin = new Padding(0);
      this.gcRateSheetLockInfo.Name = "gcRateSheetLockInfo";
      this.gcRateSheetLockInfo.Size = new Size(360, 149);
      this.gcRateSheetLockInfo.TabIndex = 24;
      this.gcRateSheetLockInfo.Text = "Rate Sheet and Lock Information";
      this.chkUseParentInfoForRate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfoForRate.AutoSize = true;
      this.chkUseParentInfoForRate.BackColor = Color.Transparent;
      this.chkUseParentInfoForRate.Location = new Point((int) byte.MaxValue, 5);
      this.chkUseParentInfoForRate.Name = "chkUseParentInfoForRate";
      this.chkUseParentInfoForRate.Size = new Size(100, 17);
      this.chkUseParentInfoForRate.TabIndex = 20;
      this.chkUseParentInfoForRate.Text = "Use Parent Info";
      this.chkUseParentInfoForRate.UseVisualStyleBackColor = false;
      this.chkUseParentInfoForRate.CheckedChanged += new EventHandler(this.chkUseParentInfoForRate_CheckedChanged);
      this.txtEmailRateSheet.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEmailRateSheet.Location = new Point(144, 33);
      this.txtEmailRateSheet.MaxLength = 64;
      this.txtEmailRateSheet.Name = "txtEmailRateSheet";
      this.txtEmailRateSheet.Size = new Size(208, 20);
      this.txtEmailRateSheet.TabIndex = 21;
      this.txtEmailRateSheet.TextChanged += new EventHandler(this.textField_Changed);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(6, 36);
      this.label10.Name = "label10";
      this.label10.Size = new Size(89, 13);
      this.label10.TabIndex = 28;
      this.label10.Text = "Rate Sheet Email";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(6, 102);
      this.label11.Name = "label11";
      this.label11.Size = new Size(112, 13);
      this.label11.TabIndex = 26;
      this.label11.Text = "Lock Info Fax Number";
      this.txtEmailLockInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEmailLockInfo.Location = new Point(144, 77);
      this.txtEmailLockInfo.MaxLength = 64;
      this.txtEmailLockInfo.Name = "txtEmailLockInfo";
      this.txtEmailLockInfo.Size = new Size(208, 20);
      this.txtEmailLockInfo.TabIndex = 23;
      this.txtEmailLockInfo.TextChanged += new EventHandler(this.textField_Changed);
      this.txtFaxRateSheet.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFaxRateSheet.Location = new Point(144, 55);
      this.txtFaxRateSheet.MaxLength = 25;
      this.txtFaxRateSheet.Name = "txtFaxRateSheet";
      this.txtFaxRateSheet.Size = new Size(208, 20);
      this.txtFaxRateSheet.TabIndex = 22;
      this.txtFaxRateSheet.TextChanged += new EventHandler(this.textField_Changed);
      this.txtFaxLockInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFaxLockInfo.Location = new Point(144, 99);
      this.txtFaxLockInfo.MaxLength = 25;
      this.txtFaxLockInfo.Name = "txtFaxLockInfo";
      this.txtFaxLockInfo.Size = new Size(208, 20);
      this.txtFaxLockInfo.TabIndex = 24;
      this.txtFaxLockInfo.TextChanged += new EventHandler(this.textField_Changed);
      this.label12.AutoSize = true;
      this.label12.Location = new Point(6, 58);
      this.label12.Name = "label12";
      this.label12.Size = new Size(121, 13);
      this.label12.TabIndex = 22;
      this.label12.Text = "Rate Sheet Fax Number";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(6, 80);
      this.label13.Name = "label13";
      this.label13.Size = new Size(80, 13);
      this.label13.TabIndex = 24;
      this.label13.Text = "Lock Info Email";
      this.pnlRight.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlRight.Controls.Add((Control) this.pnlRightTop);
      this.pnlRight.Controls.Add((Control) this.pnlRightBottom);
      this.pnlRight.Location = new Point(367, 0);
      this.pnlRight.Margin = new Padding(0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(490, 774);
      this.pnlRight.TabIndex = 0;
      this.pnlRightTop.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlRightTop.Controls.Add((Control) this.gcSalesRepInfo);
      this.pnlRightTop.Controls.Add((Control) this.gcApprovalStatus);
      this.pnlRightTop.Location = new Point(0, 0);
      this.pnlRightTop.Margin = new Padding(0);
      this.pnlRightTop.Name = "pnlRightTop";
      this.pnlRightTop.Size = new Size(490, 168);
      this.pnlRightTop.TabIndex = 26;
      this.gcSalesRepInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcSalesRepInfo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.gcSalesRepInfo.Controls.Add((Control) this.dpAeAssignedDate);
      this.gcSalesRepInfo.Controls.Add((Control) this.label54);
      this.gcSalesRepInfo.Controls.Add((Control) this.label51);
      this.gcSalesRepInfo.Controls.Add((Control) this.btnAddRep);
      this.gcSalesRepInfo.Controls.Add((Control) this.txtNameSalesRep);
      this.gcSalesRepInfo.Controls.Add((Control) this.label38);
      this.gcSalesRepInfo.Controls.Add((Control) this.label41);
      this.gcSalesRepInfo.Controls.Add((Control) this.txtPhoneSalesRep);
      this.gcSalesRepInfo.Controls.Add((Control) this.txtPersonaSalesRep);
      this.gcSalesRepInfo.Controls.Add((Control) this.txtEmailSalesRep);
      this.gcSalesRepInfo.Controls.Add((Control) this.label42);
      this.gcSalesRepInfo.Controls.Add((Control) this.label43);
      this.gcSalesRepInfo.HeaderForeColor = SystemColors.ControlText;
      this.gcSalesRepInfo.Location = new Point(263, 0);
      this.gcSalesRepInfo.Margin = new Padding(0);
      this.gcSalesRepInfo.Name = "gcSalesRepInfo";
      this.gcSalesRepInfo.Size = new Size(227, 166);
      this.gcSalesRepInfo.TabIndex = 32;
      this.gcSalesRepInfo.Text = "Primary Sales Rep / AE";
      this.dpAeAssignedDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dpAeAssignedDate.BackColor = SystemColors.Window;
      this.dpAeAssignedDate.Location = new Point(91, 121);
      this.dpAeAssignedDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpAeAssignedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpAeAssignedDate.Name = "dpAeAssignedDate";
      this.dpAeAssignedDate.Size = new Size(111, 21);
      this.dpAeAssignedDate.TabIndex = 47;
      this.dpAeAssignedDate.Tag = (object) "763";
      this.dpAeAssignedDate.ToolTip = "";
      this.dpAeAssignedDate.Value = new DateTime(0L);
      this.dpAeAssignedDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.label54.AutoSize = true;
      this.label54.Location = new Point(6, 124);
      this.label54.Name = "label54";
      this.label54.Size = new Size(76, 13);
      this.label54.TabIndex = 43;
      this.label54.Text = "Assigned Date";
      this.label51.AutoSize = true;
      this.label51.BackColor = Color.Transparent;
      this.label51.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.label51.ForeColor = Color.Red;
      this.label51.Location = new Point(39, 37);
      this.label51.Name = "label51";
      this.label51.Size = new Size(13, 16);
      this.label51.TabIndex = 42;
      this.label51.Text = "*";
      this.btnAddRep.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddRep.BackColor = Color.Transparent;
      this.btnAddRep.Location = new Point(197, 5);
      this.btnAddRep.MouseDownImage = (Image) null;
      this.btnAddRep.Name = "btnAddRep";
      this.btnAddRep.Size = new Size(16, 16);
      this.btnAddRep.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddRep.TabIndex = 29;
      this.btnAddRep.TabStop = false;
      this.btnAddRep.Click += new EventHandler(this.btnAddRep_Click);
      this.txtNameSalesRep.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtNameSalesRep.Location = new Point(91, 33);
      this.txtNameSalesRep.Name = "txtNameSalesRep";
      this.txtNameSalesRep.ReadOnly = true;
      this.txtNameSalesRep.Size = new Size(130, 20);
      this.txtNameSalesRep.TabIndex = 19;
      this.label38.AutoSize = true;
      this.label38.Location = new Point(6, 36);
      this.label38.Name = "label38";
      this.label38.Size = new Size(35, 13);
      this.label38.TabIndex = 28;
      this.label38.Text = "Name";
      this.label41.AutoSize = true;
      this.label41.Location = new Point(6, 102);
      this.label41.Name = "label41";
      this.label41.Size = new Size(32, 13);
      this.label41.TabIndex = 26;
      this.label41.Text = "Email";
      this.txtPhoneSalesRep.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPhoneSalesRep.Location = new Point(91, 77);
      this.txtPhoneSalesRep.MaxLength = 64;
      this.txtPhoneSalesRep.Name = "txtPhoneSalesRep";
      this.txtPhoneSalesRep.ReadOnly = true;
      this.txtPhoneSalesRep.Size = new Size(130, 20);
      this.txtPhoneSalesRep.TabIndex = 21;
      this.txtPersonaSalesRep.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPersonaSalesRep.Location = new Point(91, 55);
      this.txtPersonaSalesRep.MaxLength = 64;
      this.txtPersonaSalesRep.Name = "txtPersonaSalesRep";
      this.txtPersonaSalesRep.ReadOnly = true;
      this.txtPersonaSalesRep.Size = new Size(130, 20);
      this.txtPersonaSalesRep.TabIndex = 20;
      this.txtEmailSalesRep.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEmailSalesRep.Location = new Point(91, 99);
      this.txtEmailSalesRep.MaxLength = 100;
      this.txtEmailSalesRep.Name = "txtEmailSalesRep";
      this.txtEmailSalesRep.ReadOnly = true;
      this.txtEmailSalesRep.Size = new Size(130, 20);
      this.txtEmailSalesRep.TabIndex = 22;
      this.label42.AutoSize = true;
      this.label42.Location = new Point(6, 58);
      this.label42.Name = "label42";
      this.label42.Size = new Size(46, 13);
      this.label42.TabIndex = 22;
      this.label42.Text = "Persona";
      this.label43.AutoSize = true;
      this.label43.Location = new Point(6, 80);
      this.label43.Name = "label43";
      this.label43.Size = new Size(78, 13);
      this.label43.TabIndex = 24;
      this.label43.Text = "Phone Number";
      this.gcApprovalStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.gcApprovalStatus.Controls.Add((Control) this.cboCompanyRating);
      this.gcApprovalStatus.Controls.Add((Control) this.label40);
      this.gcApprovalStatus.Controls.Add((Control) this.chkAddWatchList);
      this.gcApprovalStatus.Controls.Add((Control) this.dpApplicationDate);
      this.gcApprovalStatus.Controls.Add((Control) this.dpApprovedDate);
      this.gcApprovalStatus.Controls.Add((Control) this.dpActClosingDate);
      this.gcApprovalStatus.Controls.Add((Control) this.cboCurrentStatus);
      this.gcApprovalStatus.Controls.Add((Control) this.chkUseParentInfoForApproval);
      this.gcApprovalStatus.Controls.Add((Control) this.label14);
      this.gcApprovalStatus.Controls.Add((Control) this.label15);
      this.gcApprovalStatus.Controls.Add((Control) this.label16);
      this.gcApprovalStatus.Controls.Add((Control) this.label17);
      this.gcApprovalStatus.HeaderForeColor = SystemColors.ControlText;
      this.gcApprovalStatus.Location = new Point(0, 0);
      this.gcApprovalStatus.Margin = new Padding(0);
      this.gcApprovalStatus.Name = "gcApprovalStatus";
      this.gcApprovalStatus.Size = new Size(259, 168);
      this.gcApprovalStatus.TabIndex = 26;
      this.gcApprovalStatus.Text = "Approval Status";
      this.cboCompanyRating.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboCompanyRating.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCompanyRating.FormattingEnabled = true;
      this.cboCompanyRating.Items.AddRange(new object[5]
      {
        (object) "1",
        (object) "2",
        (object) "3",
        (object) "4",
        (object) "5"
      });
      this.cboCompanyRating.Location = new Point(142, 143);
      this.cboCompanyRating.Name = "cboCompanyRating";
      this.cboCompanyRating.Size = new Size(110, 21);
      this.cboCompanyRating.TabIndex = 35;
      this.cboCompanyRating.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.label40.AutoSize = true;
      this.label40.Location = new Point(6, 146);
      this.label40.Name = "label40";
      this.label40.Size = new Size(85, 13);
      this.label40.TabIndex = 46;
      this.label40.Text = "Company Rating";
      this.chkAddWatchList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.chkAddWatchList.AutoSize = true;
      this.chkAddWatchList.Location = new Point(143, 58);
      this.chkAddWatchList.Name = "chkAddWatchList";
      this.chkAddWatchList.Size = new Size(104, 17);
      this.chkAddWatchList.TabIndex = 31;
      this.chkAddWatchList.Text = "Add to Watchlist";
      this.chkAddWatchList.UseVisualStyleBackColor = true;
      this.chkAddWatchList.CheckedChanged += new EventHandler(this.textField_Changed);
      this.dpApplicationDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dpApplicationDate.BackColor = SystemColors.Window;
      this.dpApplicationDate.Location = new Point(142, 121);
      this.dpApplicationDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpApplicationDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpApplicationDate.Name = "dpApplicationDate";
      this.dpApplicationDate.Size = new Size(111, 21);
      this.dpApplicationDate.TabIndex = 34;
      this.dpApplicationDate.Tag = (object) "763";
      this.dpApplicationDate.ToolTip = "";
      this.dpApplicationDate.Value = new DateTime(0L);
      this.dpApplicationDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.dpApprovedDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dpApprovedDate.BackColor = SystemColors.Window;
      this.dpApprovedDate.Location = new Point(142, 99);
      this.dpApprovedDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpApprovedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpApprovedDate.Name = "dpApprovedDate";
      this.dpApprovedDate.Size = new Size(111, 21);
      this.dpApprovedDate.TabIndex = 33;
      this.dpApprovedDate.Tag = (object) "763";
      this.dpApprovedDate.ToolTip = "";
      this.dpApprovedDate.Value = new DateTime(0L);
      this.dpApprovedDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.dpActClosingDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dpActClosingDate.BackColor = SystemColors.Window;
      this.dpActClosingDate.Location = new Point(142, 77);
      this.dpActClosingDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpActClosingDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpActClosingDate.Name = "dpActClosingDate";
      this.dpActClosingDate.Size = new Size(111, 21);
      this.dpActClosingDate.TabIndex = 32;
      this.dpActClosingDate.Tag = (object) "763";
      this.dpActClosingDate.ToolTip = "";
      this.dpActClosingDate.Value = new DateTime(0L);
      this.dpActClosingDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.cboCurrentStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboCurrentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCurrentStatus.FormattingEnabled = true;
      this.cboCurrentStatus.Location = new Point(142, 33);
      this.cboCurrentStatus.Name = "cboCurrentStatus";
      this.cboCurrentStatus.Size = new Size(111, 21);
      this.cboCurrentStatus.TabIndex = 30;
      this.cboCurrentStatus.SelectedIndexChanged += new EventHandler(this.cboCurrentStatus_SelectedIndexChanged);
      this.cboCurrentStatus.TextChanged += new EventHandler(this.textField_Changed);
      this.chkUseParentInfoForApproval.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfoForApproval.AutoSize = true;
      this.chkUseParentInfoForApproval.BackColor = Color.Transparent;
      this.chkUseParentInfoForApproval.Location = new Point(155, 4);
      this.chkUseParentInfoForApproval.Name = "chkUseParentInfoForApproval";
      this.chkUseParentInfoForApproval.Size = new Size(100, 17);
      this.chkUseParentInfoForApproval.TabIndex = 29;
      this.chkUseParentInfoForApproval.Text = "Use Parent Info";
      this.chkUseParentInfoForApproval.UseVisualStyleBackColor = false;
      this.chkUseParentInfoForApproval.CheckedChanged += new EventHandler(this.chkUseParentInfoForApproval_CheckedChanged);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(6, 36);
      this.label14.Name = "label14";
      this.label14.Size = new Size(74, 13);
      this.label14.TabIndex = 28;
      this.label14.Text = "Current Status";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(6, 124);
      this.label15.Name = "label15";
      this.label15.Size = new Size(85, 13);
      this.label15.TabIndex = 26;
      this.label15.Text = "Application Date";
      this.label16.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label16.AutoSize = true;
      this.label16.Location = new Point(6, 80);
      this.label16.Name = "label16";
      this.label16.Size = new Size(100, 13);
      this.label16.TabIndex = 22;
      this.label16.Text = "Current Status Date";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(6, 102);
      this.label17.Name = "label17";
      this.label17.Size = new Size(79, 13);
      this.label17.TabIndex = 24;
      this.label17.Text = "Approved Date";
      this.pnlRightBottom.Controls.Add((Control) this.gcBusinessInfo);
      this.pnlRightBottom.Dock = DockStyle.Bottom;
      this.pnlRightBottom.Location = new Point(0, 170);
      this.pnlRightBottom.Name = "pnlRightBottom";
      this.pnlRightBottom.Size = new Size(490, 604);
      this.pnlRightBottom.TabIndex = 0;
      this.gcBusinessInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcBusinessInfo.Controls.Add((Control) this.txtYrsInBusiness);
      this.gcBusinessInfo.Controls.Add((Control) this.txtLPAPassword);
      this.gcBusinessInfo.Controls.Add((Control) this.label56);
      this.gcBusinessInfo.Controls.Add((Control) this.txtTPONumber);
      this.gcBusinessInfo.Controls.Add((Control) this.cboTypeofEntity);
      this.gcBusinessInfo.Controls.Add((Control) this.txtEOCompany);
      this.gcBusinessInfo.Controls.Add((Control) this.label30);
      this.gcBusinessInfo.Controls.Add((Control) this.dpEOExpirationDate);
      this.gcBusinessInfo.Controls.Add((Control) this.label29);
      this.gcBusinessInfo.Controls.Add((Control) this.cboCloseInOwnName);
      this.gcBusinessInfo.Controls.Add((Control) this.cboFundInOwnName);
      this.gcBusinessInfo.Controls.Add((Control) this.cboLPASponsored);
      this.gcBusinessInfo.Controls.Add((Control) this.cboDUSponsored);
      this.gcBusinessInfo.Controls.Add((Control) this.label35);
      this.gcBusinessInfo.Controls.Add((Control) this.label24);
      this.gcBusinessInfo.Controls.Add((Control) this.label23);
      this.gcBusinessInfo.Controls.Add((Control) this.label1);
      this.gcBusinessInfo.Controls.Add((Control) this.label36);
      this.gcBusinessInfo.Controls.Add((Control) this.label37);
      this.gcBusinessInfo.Controls.Add((Control) this.txtMERSOrgID);
      this.gcBusinessInfo.Controls.Add((Control) this.txtEOPolicy);
      this.gcBusinessInfo.Controls.Add((Control) this.label31);
      this.gcBusinessInfo.Controls.Add((Control) this.label32);
      this.gcBusinessInfo.Controls.Add((Control) this.txtCompanyNetWorth);
      this.gcBusinessInfo.Controls.Add((Control) this.label34);
      this.gcBusinessInfo.Controls.Add((Control) this.dpFinancialLastUpdate);
      this.gcBusinessInfo.Controls.Add((Control) this.label28);
      this.gcBusinessInfo.Controls.Add((Control) this.label27);
      this.gcBusinessInfo.Controls.Add((Control) this.txtFinancialPeriod);
      this.gcBusinessInfo.Controls.Add((Control) this.label53);
      this.gcBusinessInfo.Controls.Add((Control) this.txtLEI);
      this.gcBusinessInfo.Controls.Add((Control) this.label26);
      this.gcBusinessInfo.Controls.Add((Control) this.txtNMLSID);
      this.gcBusinessInfo.Controls.Add((Control) this.chkUseSSN);
      this.gcBusinessInfo.Controls.Add((Control) this.label25);
      this.gcBusinessInfo.Controls.Add((Control) this.txtTaxID);
      this.gcBusinessInfo.Controls.Add((Control) this.dpDateincorporation);
      this.gcBusinessInfo.Controls.Add((Control) this.cboStateIncorp);
      this.gcBusinessInfo.Controls.Add((Control) this.chkIncorporated);
      this.gcBusinessInfo.Controls.Add((Control) this.chkUseParentInfoForBusiness);
      this.gcBusinessInfo.Controls.Add((Control) this.label18);
      this.gcBusinessInfo.Controls.Add((Control) this.label19);
      this.gcBusinessInfo.Controls.Add((Control) this.txtEntityDescription);
      this.gcBusinessInfo.Controls.Add((Control) this.label20);
      this.gcBusinessInfo.Controls.Add((Control) this.label21);
      this.gcBusinessInfo.HeaderForeColor = SystemColors.ControlText;
      this.gcBusinessInfo.Location = new Point(0, 0);
      this.gcBusinessInfo.Margin = new Padding(0);
      this.gcBusinessInfo.Name = "gcBusinessInfo";
      this.gcBusinessInfo.Size = new Size(490, 604);
      this.gcBusinessInfo.TabIndex = 27;
      this.gcBusinessInfo.Text = "Business Information";
      this.txtLPAPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtLPAPassword.Enabled = false;
      this.txtLPAPassword.Location = new Point(144, 474);
      this.txtLPAPassword.MaxLength = 100;
      this.txtLPAPassword.Name = "txtLPAPassword";
      this.txtLPAPassword.PasswordChar = '*';
      this.txtLPAPassword.Size = new Size(123, 20);
      this.txtLPAPassword.TabIndex = 84;
      this.txtLPAPassword.KeyPress += new KeyPressEventHandler(this.txtLPAPassword_KeyPress);
      this.txtLPAPassword.Leave += new EventHandler(this.txtLPAPassword_Leave);
      this.txtTPONumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTPONumber.Enabled = false;
      this.txtTPONumber.Location = new Point(144, 451);
      this.txtTPONumber.MaxLength = 100;
      this.txtTPONumber.Name = "txtTPONumber";
      this.txtTPONumber.Size = new Size(123, 20);
      this.txtTPONumber.TabIndex = 84;
      this.txtTPONumber.KeyPress += new KeyPressEventHandler(this.txtTPONumber_KeyPress);
      this.txtTPONumber.Leave += new EventHandler(this.txtTPONumber_Leave);
      this.cboTypeofEntity.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTypeofEntity.FormattingEnabled = true;
      this.cboTypeofEntity.Items.AddRange(new object[7]
      {
        (object) " ",
        (object) "Individual",
        (object) "Sole Proprietorship",
        (object) "Partnership",
        (object) "Corporation",
        (object) "Limited Liability Company",
        (object) "Other (please specify)"
      });
      this.cboTypeofEntity.Location = new Point(144, 102);
      this.cboTypeofEntity.Name = "cboTypeofEntity";
      this.cboTypeofEntity.Size = new Size(143, 21);
      this.cboTypeofEntity.TabIndex = 40;
      this.cboTypeofEntity.SelectedIndexChanged += new EventHandler(this.cboTypeofEntity_SelectedIndexChanged);
      this.cboTypeofEntity.TextChanged += new EventHandler(this.textField_Changed);
      this.txtEOCompany.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEOCompany.Location = new Point(144, 334);
      this.txtEOCompany.MaxLength = 50;
      this.txtEOCompany.Name = "txtEOCompany";
      this.txtEOCompany.Size = new Size(340, 20);
      this.txtEOCompany.TabIndex = 49;
      this.txtEOCompany.TextChanged += new EventHandler(this.textField_Changed);
      this.label30.AutoSize = true;
      this.label30.Location = new Point(6, 339);
      this.label30.Name = "label30";
      this.label30.Size = new Size(75, 13);
      this.label30.TabIndex = 83;
      this.label30.Text = "E&&O Company";
      this.dpEOExpirationDate.BackColor = SystemColors.Window;
      this.dpEOExpirationDate.Location = new Point(144, 310);
      this.dpEOExpirationDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpEOExpirationDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpEOExpirationDate.Name = "dpEOExpirationDate";
      this.dpEOExpirationDate.Size = new Size(143, 21);
      this.dpEOExpirationDate.TabIndex = 48;
      this.dpEOExpirationDate.Tag = (object) "763";
      this.dpEOExpirationDate.ToolTip = "";
      this.dpEOExpirationDate.Value = new DateTime(0L);
      this.dpEOExpirationDate.ValueChanged += new EventHandler(this.textField_Changed);
      this.label29.AutoSize = true;
      this.label29.Location = new Point(6, 315);
      this.label29.Name = "label29";
      this.label29.Size = new Size(103, 13);
      this.label29.TabIndex = 81;
      this.label29.Text = "E&&O Expiration Date";
      this.cboCloseInOwnName.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCloseInOwnName.FormattingEnabled = true;
      this.cboCloseInOwnName.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Yes",
        (object) "No"
      });
      this.cboCloseInOwnName.Location = new Point(144, 544);
      this.cboCloseInOwnName.Name = "cboCloseInOwnName";
      this.cboCloseInOwnName.Size = new Size(143, 21);
      this.cboCloseInOwnName.TabIndex = 54;
      this.cboCloseInOwnName.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cboCloseInOwnName.TextChanged += new EventHandler(this.textField_Changed);
      this.cboFundInOwnName.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFundInOwnName.FormattingEnabled = true;
      this.cboFundInOwnName.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Yes",
        (object) "No"
      });
      this.cboFundInOwnName.Location = new Point(144, 522);
      this.cboFundInOwnName.Name = "cboFundInOwnName";
      this.cboFundInOwnName.Size = new Size(143, 21);
      this.cboFundInOwnName.TabIndex = 53;
      this.cboFundInOwnName.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cboFundInOwnName.TextChanged += new EventHandler(this.textField_Changed);
      this.cboLPASponsored.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLPASponsored.FormattingEnabled = true;
      this.cboLPASponsored.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Yes",
        (object) "No"
      });
      this.cboLPASponsored.Location = new Point(144, 427);
      this.cboLPASponsored.Name = "cboLPASponsored";
      this.cboLPASponsored.Size = new Size(143, 21);
      this.cboLPASponsored.TabIndex = 52;
      this.cboLPASponsored.SelectedIndexChanged += new EventHandler(this.cboLPASponsored_SelectedIndexChanged);
      this.cboDUSponsored.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDUSponsored.FormattingEnabled = true;
      this.cboDUSponsored.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Yes",
        (object) "No"
      });
      this.cboDUSponsored.Location = new Point(144, 403);
      this.cboDUSponsored.Name = "cboDUSponsored";
      this.cboDUSponsored.Size = new Size(143, 21);
      this.cboDUSponsored.TabIndex = 52;
      this.cboDUSponsored.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cboDUSponsored.TextChanged += new EventHandler(this.textField_Changed);
      this.label35.AutoSize = true;
      this.label35.Location = new Point(7, 527);
      this.label35.Name = "label35";
      this.label35.Size = new Size(120, 13);
      this.label35.TabIndex = 74;
      this.label35.Text = "Can Fund in Own Name";
      this.label24.AutoSize = true;
      this.label24.Location = new Point(20, 479);
      this.label24.Name = "label24";
      this.label24.Size = new Size(101, 13);
      this.label24.TabIndex = 73;
      this.label24.Text = "TPO LPA Password";
      this.label23.AutoSize = true;
      this.label23.Location = new Point(20, 456);
      this.label23.Name = "label23";
      this.label23.Size = new Size(69, 13);
      this.label23.TabIndex = 73;
      this.label23.Text = "TPO Number";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 433);
      this.label1.Name = "label1";
      this.label1.Size = new Size(81, 13);
      this.label1.TabIndex = 73;
      this.label1.Text = "LPA Sponsored";
      this.label36.AutoSize = true;
      this.label36.Location = new Point(7, 549);
      this.label36.Name = "label36";
      this.label36.Size = new Size(122, 13);
      this.label36.TabIndex = 75;
      this.label36.Text = "Can Close in Own Name";
      this.label37.AutoSize = true;
      this.label37.Location = new Point(7, 410);
      this.label37.Name = "label37";
      this.label37.Size = new Size(77, 13);
      this.label37.TabIndex = 73;
      this.label37.Text = "DU Sponsored";
      this.txtMERSOrgID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtMERSOrgID.Location = new Point(144, 380);
      this.txtMERSOrgID.MaxLength = 50;
      this.txtMERSOrgID.Name = "txtMERSOrgID";
      this.txtMERSOrgID.Size = new Size(340, 20);
      this.txtMERSOrgID.TabIndex = 51;
      this.txtMERSOrgID.TextChanged += new EventHandler(this.textField_Changed);
      this.txtEOPolicy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEOPolicy.Location = new Point(144, 357);
      this.txtEOPolicy.MaxLength = 50;
      this.txtEOPolicy.Name = "txtEOPolicy";
      this.txtEOPolicy.Size = new Size(340, 20);
      this.txtEOPolicy.TabIndex = 50;
      this.txtEOPolicy.TextChanged += new EventHandler(this.textField_Changed);
      this.label31.AutoSize = true;
      this.label31.Location = new Point(7, 363);
      this.label31.Name = "label31";
      this.label31.Size = new Size(69, 13);
      this.label31.TabIndex = 68;
      this.label31.Text = "E&&O Policy #";
      this.label32.AutoSize = true;
      this.label32.Location = new Point(7, 386);
      this.label32.Name = "label32";
      this.label32.Size = new Size(125, 13);
      this.label32.TabIndex = 70;
      this.label32.Text = "MERS Originating Org ID";
      this.txtCompanyNetWorth.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCompanyNetWorth.Location = new Point(144, 287);
      this.txtCompanyNetWorth.MaxLength = 17;
      this.txtCompanyNetWorth.Name = "txtCompanyNetWorth";
      this.txtCompanyNetWorth.Size = new Size(340, 20);
      this.txtCompanyNetWorth.TabIndex = 47;
      this.txtCompanyNetWorth.TextChanged += new EventHandler(this.textField_Changed);
      this.txtCompanyNetWorth.Enter += new EventHandler(this.txtCompanyNetWorth_Enter);
      this.txtCompanyNetWorth.Leave += new EventHandler(this.txtCompanyNetWorth_Leave);
      this.label34.AutoSize = true;
      this.label34.Location = new Point(6, 291);
      this.label34.Name = "label34";
      this.label34.Size = new Size(103, 13);
      this.label34.TabIndex = 66;
      this.label34.Text = "Company Net Worth";
      this.dpFinancialLastUpdate.BackColor = SystemColors.Window;
      this.dpFinancialLastUpdate.Location = new Point(144, 263);
      this.dpFinancialLastUpdate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpFinancialLastUpdate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpFinancialLastUpdate.Name = "dpFinancialLastUpdate";
      this.dpFinancialLastUpdate.Size = new Size(143, 21);
      this.dpFinancialLastUpdate.TabIndex = 46;
      this.dpFinancialLastUpdate.Tag = (object) "763";
      this.dpFinancialLastUpdate.ToolTip = "";
      this.dpFinancialLastUpdate.Value = new DateTime(0L);
      this.dpFinancialLastUpdate.ValueChanged += new EventHandler(this.textField_Changed);
      this.label28.AutoSize = true;
      this.label28.Location = new Point(6, 268);
      this.label28.Name = "label28";
      this.label28.Size = new Size(121, 13);
      this.label28.TabIndex = 58;
      this.label28.Text = "Financials Last Updated";
      this.label27.AutoSize = true;
      this.label27.Location = new Point(6, 245);
      this.label27.Name = "label27";
      this.label27.Size = new Size(87, 13);
      this.label27.TabIndex = 56;
      this.label27.Text = "Financials Period";
      this.txtFinancialPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFinancialPeriod.Location = new Point(144, 240);
      this.txtFinancialPeriod.MaxLength = 50;
      this.txtFinancialPeriod.Name = "txtFinancialPeriod";
      this.txtFinancialPeriod.Size = new Size(340, 20);
      this.txtFinancialPeriod.TabIndex = 45;
      this.txtFinancialPeriod.TextChanged += new EventHandler(this.textField_Changed);
      this.label53.AutoSize = true;
      this.label53.Location = new Point(6, 200);
      this.label53.Name = "label53";
      this.label53.Size = new Size(23, 13);
      this.label53.TabIndex = 65;
      this.label53.Text = "LEI";
      this.txtLEI.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtLEI.Location = new Point(144, 194);
      this.txtLEI.MaxLength = 20;
      this.txtLEI.Name = "txtLEI";
      this.txtLEI.Size = new Size(340, 20);
      this.txtLEI.TabIndex = 75;
      this.txtLEI.KeyPress += new KeyPressEventHandler(this.txtLEI_KeyPress);
      this.txtLEI.Leave += new EventHandler(this.txtLEI_Leave);
      this.label26.AutoSize = true;
      this.label26.Location = new Point(6, 224);
      this.label26.Name = "label26";
      this.label26.Size = new Size(51, 13);
      this.label26.TabIndex = 53;
      this.label26.Text = "NMLS ID";
      this.txtNMLSID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtNMLSID.Location = new Point(144, 217);
      this.txtNMLSID.MaxLength = 12;
      this.txtNMLSID.Name = "txtNMLSID";
      this.txtNMLSID.Size = new Size(340, 20);
      this.txtNMLSID.TabIndex = 44;
      this.txtNMLSID.TextChanged += new EventHandler(this.textField_Changed);
      this.txtNMLSID.KeyPress += new KeyPressEventHandler(this.txtNMLSID_KeyPress);
      this.chkUseSSN.AutoSize = true;
      this.chkUseSSN.Location = new Point(144, 172);
      this.chkUseSSN.Name = "chkUseSSN";
      this.chkUseSSN.Size = new Size(105, 17);
      this.chkUseSSN.TabIndex = 43;
      this.chkUseSSN.Text = "Use SSN Format";
      this.chkUseSSN.UseVisualStyleBackColor = true;
      this.chkUseSSN.CheckedChanged += new EventHandler(this.chkUseSSN_CheckedChanged);
      this.label25.AutoSize = true;
      this.label25.Location = new Point(6, 149);
      this.label25.Name = "label25";
      this.label25.Size = new Size(39, 13);
      this.label25.TabIndex = 50;
      this.label25.Text = "Tax ID";
      this.txtTaxID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTaxID.Location = new Point(144, 148);
      this.txtTaxID.MaxLength = 11;
      this.txtTaxID.Name = "txtTaxID";
      this.txtTaxID.Size = new Size(340, 20);
      this.txtTaxID.TabIndex = 42;
      this.txtTaxID.KeyPress += new KeyPressEventHandler(this.txtTaxID_KeyPress);
      this.txtTaxID.Leave += new EventHandler(this.txtTaxID_Leave);
      this.dpDateincorporation.BackColor = SystemColors.Window;
      this.dpDateincorporation.Location = new Point(144, 80);
      this.dpDateincorporation.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpDateincorporation.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpDateincorporation.Name = "dpDateincorporation";
      this.dpDateincorporation.Size = new Size(143, 21);
      this.dpDateincorporation.TabIndex = 39;
      this.dpDateincorporation.Tag = (object) "763";
      this.dpDateincorporation.ToolTip = "";
      this.dpDateincorporation.Value = new DateTime(0L);
      this.dpDateincorporation.ValueChanged += new EventHandler(this.textField_Changed);
      this.cboStateIncorp.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStateIncorp.FormattingEnabled = true;
      this.cboStateIncorp.Location = new Point(144, 58);
      this.cboStateIncorp.Name = "cboStateIncorp";
      this.cboStateIncorp.Size = new Size(143, 21);
      this.cboStateIncorp.TabIndex = 38;
      this.cboStateIncorp.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.cboStateIncorp.TextChanged += new EventHandler(this.textField_Changed);
      this.chkIncorporated.AutoSize = true;
      this.chkIncorporated.Location = new Point(144, 35);
      this.chkIncorporated.Name = "chkIncorporated";
      this.chkIncorporated.Size = new Size(86, 17);
      this.chkIncorporated.TabIndex = 37;
      this.chkIncorporated.Text = "Incorporated";
      this.chkIncorporated.UseVisualStyleBackColor = true;
      this.chkIncorporated.CheckedChanged += new EventHandler(this.textField_Changed);
      this.chkUseParentInfoForBusiness.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfoForBusiness.AutoSize = true;
      this.chkUseParentInfoForBusiness.BackColor = Color.Transparent;
      this.chkUseParentInfoForBusiness.Location = new Point(385, 5);
      this.chkUseParentInfoForBusiness.Name = "chkUseParentInfoForBusiness";
      this.chkUseParentInfoForBusiness.Size = new Size(100, 17);
      this.chkUseParentInfoForBusiness.TabIndex = 36;
      this.chkUseParentInfoForBusiness.Text = "Use Parent Info";
      this.chkUseParentInfoForBusiness.UseVisualStyleBackColor = false;
      this.chkUseParentInfoForBusiness.CheckedChanged += new EventHandler(this.ChkUseParentInfoForBusinessOnCheckedChanged);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(6, 61);
      this.label18.Name = "label18";
      this.label18.Size = new Size(109, 13);
      this.label18.TabIndex = 28;
      this.label18.Text = "State of Incorporation";
      this.label19.AutoSize = true;
      this.label19.Location = new Point(6, (int) sbyte.MaxValue);
      this.label19.Name = "label19";
      this.label19.Size = new Size(118, 13);
      this.label19.TabIndex = 26;
      this.label19.Text = "Other Entity Description";
      this.txtEntityDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEntityDescription.Location = new Point(144, 125);
      this.txtEntityDescription.MaxLength = 100;
      this.txtEntityDescription.Name = "txtEntityDescription";
      this.txtEntityDescription.Size = new Size(340, 20);
      this.txtEntityDescription.TabIndex = 41;
      this.txtEntityDescription.TextChanged += new EventHandler(this.textField_Changed);
      this.label20.AutoSize = true;
      this.label20.Location = new Point(6, 83);
      this.label20.Name = "label20";
      this.label20.Size = new Size(107, 13);
      this.label20.TabIndex = 22;
      this.label20.Text = "Date of Incorporation";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(6, 105);
      this.label21.Name = "label21";
      this.label21.Size = new Size(72, 13);
      this.label21.TabIndex = 24;
      this.label21.Text = "Type of Entity";
      this.txtYrsInBusiness.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtYrsInBusiness.Enabled = false;
      this.txtYrsInBusiness.Location = new Point(386, 79);
      this.txtYrsInBusiness.MaxLength = 64;
      this.txtYrsInBusiness.Name = "txtYrsInBusiness";
      this.txtYrsInBusiness.ReadOnly = true;
      this.txtYrsInBusiness.Size = new Size(98, 20);
      this.txtYrsInBusiness.TabIndex = 48;
      this.label56.AutoSize = true;
      this.label56.Location = new Point(302, 83);
      this.label56.Name = "label56";
      this.label56.Size = new Size(78, 13);
      this.label56.TabIndex = 49;
      this.label56.Text = "Yrs in Business";
      this.label56.TextAlign = ContentAlignment.MiddleRight;
      this.txtBillAddress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBillAddress.Location = new Point(144, 317);
      this.txtBillAddress.MaxLength = 100;
      this.txtBillAddress.Name = "txtBillAddress";
      this.txtBillAddress.Size = new Size(73, 20);
      this.txtBillAddress.TabIndex = 14;
      this.txtBillAddress.TextChanged += new EventHandler(this.textField_Changed);
      this.chkCopyCompanyAddress.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkCopyCompanyAddress.AutoSize = true;
      this.chkCopyCompanyAddress.BackColor = Color.Transparent;
      this.chkCopyCompanyAddress.Location = new Point(224, 319);
      this.chkCopyCompanyAddress.Name = "chkCopyCompanyAddress";
      this.chkCopyCompanyAddress.Size = new Size(134, 17);
      this.chkCopyCompanyAddress.TabIndex = 15;
      this.chkCopyCompanyAddress.Text = "Same as Main Address";
      this.chkCopyCompanyAddress.TextAlign = ContentAlignment.TopLeft;
      this.chkCopyCompanyAddress.TextImageRelation = TextImageRelation.ImageAboveText;
      this.chkCopyCompanyAddress.UseVisualStyleBackColor = false;
      this.chkCopyCompanyAddress.CheckedChanged += new EventHandler(this.chkCopyAdd_CheckedChanged);
      this.txtBillCity.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBillCity.Location = new Point(144, 339);
      this.txtBillCity.MaxLength = 100;
      this.txtBillCity.Name = "txtBillCity";
      this.txtBillCity.Size = new Size(209, 20);
      this.txtBillCity.TabIndex = 16;
      this.txtBillCity.TextChanged += new EventHandler(this.textField_Changed);
      this.cboBillState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBillState.FormattingEnabled = true;
      this.cboBillState.Location = new Point(144, 361);
      this.cboBillState.Name = "cboBillState";
      this.cboBillState.Size = new Size(56, 21);
      this.cboBillState.TabIndex = 17;
      this.cboBillState.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.txtBillZip.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBillZip.Location = new Point(269, 361);
      this.txtBillZip.MaxLength = 15;
      this.txtBillZip.Name = "txtBillZip";
      this.txtBillZip.Size = new Size(84, 20);
      this.txtBillZip.TabIndex = 18;
      this.txtBillZip.Tag = (object) "2284";
      this.txtBillZip.TextChanged += new EventHandler(this.textField_Changed);
      this.label57.AutoSize = true;
      this.label57.Location = new Point(6, 320);
      this.label57.Name = "label57";
      this.label57.Size = new Size(75, 13);
      this.label57.TabIndex = 53;
      this.label57.Text = "Billing Address";
      this.label59.AutoSize = true;
      this.label59.Location = new Point(6, 343);
      this.label59.Name = "label59";
      this.label59.Size = new Size(54, 13);
      this.label59.TabIndex = 55;
      this.label59.Text = "Billing City";
      this.label61.AutoSize = true;
      this.label61.Location = new Point(6, 366);
      this.label61.Name = "label61";
      this.label61.Size = new Size(62, 13);
      this.label61.TabIndex = 57;
      this.label61.Text = "Billing State";
      this.label62.Location = new Point(203, 365);
      this.label62.Name = "label62";
      this.label62.Size = new Size(53, 13);
      this.label62.TabIndex = 59;
      this.label62.Text = "Billing Zip";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpAll);
      this.Margin = new Padding(0);
      this.Name = nameof (EditCompanyInfoControl);
      this.Padding = new Padding(5);
      this.Size = new Size(872, 1111);
      this.grpAll.ResumeLayout(false);
      ((ISupportInitialize) this.btnReset).EndInit();
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.pnlBody.ResumeLayout(false);
      this.pnlLeft.ResumeLayout(false);
      this.gcOranization.ResumeLayout(false);
      this.gcOranization.PerformLayout();
      this.gcProductPricingInfo.ResumeLayout(false);
      this.gcProductPricingInfo.PerformLayout();
      this.pnlPML.ResumeLayout(false);
      this.pnlPML.PerformLayout();
      this.gcCompanyDetails.ResumeLayout(false);
      this.gcCompanyDetails.PerformLayout();
      this.gcRateSheetLockInfo.ResumeLayout(false);
      this.gcRateSheetLockInfo.PerformLayout();
      this.pnlRight.ResumeLayout(false);
      this.pnlRightTop.ResumeLayout(false);
      this.gcSalesRepInfo.ResumeLayout(false);
      this.gcSalesRepInfo.PerformLayout();
      ((ISupportInitialize) this.btnAddRep).EndInit();
      this.gcApprovalStatus.ResumeLayout(false);
      this.gcApprovalStatus.PerformLayout();
      this.pnlRightBottom.ResumeLayout(false);
      this.gcBusinessInfo.ResumeLayout(false);
      this.gcBusinessInfo.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
