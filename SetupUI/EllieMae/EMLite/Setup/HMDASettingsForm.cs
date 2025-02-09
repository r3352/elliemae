// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.HMDASettingsForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class HMDASettingsForm : SettingsUserControl
  {
    private Sessions.Session session;
    private FieldDropdownItem currentAppDateSelection;
    private FieldDropdownItem currentReliedUponFactorsDTISelection;
    private FieldDropdownItem currentReliedUponFactorsCLTVSelection;
    private FieldDropdownItem currentReliedUponFactorsIncomeSelection;
    private FeaturesAclManager aclManager;
    private bool hasAddLEIPermission;
    private bool hasEditLEIPermission;
    private bool hasRemoveLEIPermission;
    private HMDAInformation _hmdaInformationCached;
    private List<HMDAProfile> hmdaProfiles;
    private HMDAProfile profile;
    private int profileId;
    private Dictionary<string, string> agencies;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Label label1;
    private GradientPanel gradientPanel1;
    private Label label18;
    private TextBox txtPhone;
    private TextBox txtFax;
    private TextBox txtEmail;
    private Label label11;
    private Label label15;
    private Label label16;
    private TextBox txtCompanyName;
    private TextBox txtAddress;
    private TextBox txtCity;
    private TextBox txtZip;
    private Label label14;
    private Label label13;
    private Label label12;
    private Label label10;
    private Label label6;
    private ComboBox cmbAgency;
    private Label label5;
    private TextBox txtTaxId;
    private Label lblTaxId;
    private TextBox txtRespondentId;
    private Label lblRespondentId;
    private ComboBox cmbState;
    private ComboBox cmbParentMailingState;
    private Label label19;
    private TextBox txtParentMailingName;
    private TextBox txtParentMailingAddress;
    private TextBox txtParentMailingCity;
    private TextBox txtParentMailingZip;
    private Label label20;
    private Label label21;
    private Label label22;
    private Label label23;
    private Label label24;
    private ComboBox cmbApplicationDate;
    private Label label8;
    private GradientPanel gradientPanel2;
    private Label label7;
    private Label label9;
    private GradientPanel gradientPanel3;
    private Label label25;
    private Label label4;
    private Label label3;
    private ComboBox cmbLoansCorrespondent;
    private Label label2;
    private ComboBox cmbLoansNoChannel;
    private Label label27;
    private GradientPanel gradientPanel4;
    private Label label26;
    private Panel panel1;
    private Panel panel3;
    private Panel panel2;
    private RadioButton rdBtnPurchaseLoansNo;
    private RadioButton rdBtnPurchaseLoansYes;
    private RadioButton rdBtnReportInfoNo;
    private RadioButton rdBtnReportInfoYes;
    private Label label29;
    private TextBox txtLEI;
    private CheckBox txtNuli;
    private Label label28;
    private RadioButton rdBtnRprtIncmNo;
    private Label label30;
    private RadioButton rdBtnRprtIncmYes;
    private GradientPanel gradientPanel5;
    private Label label31;
    private RadioButton radioButton3;
    private Label label32;
    private RadioButton radioButton4;
    private Panel panel4;
    private Panel panel5;
    private RadioButton radioButton7;
    private Label label34;
    private RadioButton radioButton8;
    private RadioButton rdBtnRateSpreadNo;
    private Label label35;
    private RadioButton rdBtnRateSpreadYes;
    private RadioButton radioButton5;
    private Label label33;
    private RadioButton radioButton6;
    private TextBox txtName;
    private Label label36;
    private Label verticalSeparator2;
    private Label verticalSeparator1;
    private Label horizontalSeparator1;
    private GradientPanel gradientPanel6;
    private Label label17;
    private Label verticalSeparator3;
    private Label label37;
    private Label label40;
    private Label label39;
    private Label label38;
    private ComboBox cboDTI;
    private ComboBox cboIncome;
    private ComboBox cboCLTV;
    private GroupContainer groupContainer2;
    private GridView gvHMDAProfile;
    private StandardIconButton stdButtonNew;
    private StandardIconButton stdButtonEdit;
    private StandardIconButton stdButtonDelete;
    private GroupContainer groupContainer3;
    private Label label42;
    private TextBox txtProfileName;
    private Label label41;
    private StandardIconButton stdButtonReset;
    private StandardIconButton stdButtonSave;
    private Panel panel8;
    private Label label46;
    private RadioButton rdButtonCoBorrowerAgeYes;
    private RadioButton rdButtonCoBorrowerAgeNo;
    private Panel panel7;
    private Label label44;
    private RadioButton rdButtonBorrowerAgeYes;
    private RadioButton rdButtonBorrowerAgeNo;

    public HMDAProfile Profile => this.profile;

    public HMDASettingsForm(Sessions.Session session, SetUpContainer setupContainer, int profileId = 0)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.profileId = profileId;
      this.aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.hasAddLEIPermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_HMDAAddLEI);
      this.hasEditLEIPermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_HMDAEditLEI);
      this.hasRemoveLEIPermission = this.aclManager.GetUserApplicationRight(AclFeature.SettingsTab_HMDARemoveLEI);
      this.InitAgencyList();
      this.cmbState.Items.AddRange((object[]) Utils.GetStates());
      this.cmbParentMailingState.Items.AddRange((object[]) Utils.GetStates());
      this.hmdaProfiles = this.session.ConfigurationManager.GetHMDAProfile();
      if (this.hmdaProfiles.Count > 0)
      {
        this.LoadHMDAProfiles(this.hmdaProfiles);
        int num = 0;
        if (this.profileId > 0)
          num = this.hmdaProfiles.FindIndex((Predicate<HMDAProfile>) (p => p.HMDAProfileID == this.profileId));
        this.gvHMDAProfile.Items[num].Selected = true;
        this.LoadHMDASetting(this.hmdaProfiles[num]);
      }
      else
        this.LoadHMDASetting((HMDAProfile) null);
      this.stdButtonSave.Enabled = false;
      this.stdButtonNew.Enabled = this.hasAddLEIPermission;
      this.stdButtonEdit.Enabled = this.hasEditLEIPermission && this.hmdaProfiles.Count > 0;
      this.stdButtonDelete.Enabled = this.hasRemoveLEIPermission && this.hmdaProfiles.Count > 0;
      this.stdButtonReset.Enabled = false;
      this.rdBtnRprtIncmYes.CheckedChanged += new EventHandler(this.CheckChange);
      this.rdBtnRprtIncmNo.CheckedChanged += new EventHandler(this.CheckChange);
      this.rdButtonBorrowerAgeYes.CheckedChanged += new EventHandler(this.CheckChange);
      this.rdButtonBorrowerAgeNo.CheckedChanged += new EventHandler(this.CheckChange);
      this.rdButtonCoBorrowerAgeYes.CheckedChanged += new EventHandler(this.CheckChange);
      this.rdButtonCoBorrowerAgeNo.CheckedChanged += new EventHandler(this.CheckChange);
    }

    private void loadAgencies(string agency)
    {
      this.cmbAgency.DataSource = (object) null;
      this.cmbAgency.Items.Clear();
      this.cmbAgency.DataSource = (object) new BindingSource((object) this.agencies, (string) null);
      this.cmbAgency.DisplayMember = "Value";
      this.cmbAgency.ValueMember = "Key";
      if (string.IsNullOrEmpty(agency))
        return;
      foreach (object obj in this.cmbAgency.Items)
      {
        if (string.Compare(((KeyValuePair<string, string>) obj).Key, agency, true) == 0)
        {
          this.cmbAgency.SelectedItem = obj;
          break;
        }
      }
    }

    private void loadApplicationSetting(string fieldId)
    {
      this.cmbApplicationDate.Items.Clear();
      this.cmbApplicationDate.Items.Add((object) new FieldDropdownItem("745", "Application Date"));
      if (string.IsNullOrEmpty(fieldId))
        fieldId = "745";
      if (fieldId != "745")
      {
        FieldSettings fieldSettings = this.session.LoanManager.GetFieldSettings();
        FieldDefinition field = EncompassFields.GetField(fieldId, fieldSettings, true);
        if (field != null)
          this.cmbApplicationDate.Items.Add((object) new FieldDropdownItem(field.FieldID, field.Description));
        else
          this.cmbApplicationDate.Items.Add((object) new FieldDropdownItem(fieldId, "Unknown field"));
      }
      this.cmbApplicationDate.Items.Add((object) "Other...");
      foreach (object obj in this.cmbApplicationDate.Items)
      {
        if (obj is FieldDropdownItem && string.Compare(((FieldDropdownItem) obj).FieldID, fieldId, true) == 0)
        {
          this.cmbApplicationDate.SelectedItem = obj;
          break;
        }
      }
    }

    private void loadFieldDropdownItemSetting(string fieldID, ComboBox cb)
    {
      if (((IEnumerable<string>) this.fillDropdown(ref fieldID, cb)).All<string>((Func<string, bool>) (x => x != fieldID)))
      {
        FieldSettings fieldSettings = this.session.LoanManager.GetFieldSettings();
        FieldDefinition field = EncompassFields.GetField(fieldID, fieldSettings, true);
        if (field != null)
          cb.Items.Add((object) new FieldDropdownItem(field.FieldID, field.Description));
        else
          cb.Items.Add((object) new FieldDropdownItem(fieldID, "Unknown field"));
      }
      cb.Items.Add((object) "Other...");
      foreach (object obj in cb.Items)
      {
        if (obj is FieldDropdownItem && string.Compare(((FieldDropdownItem) obj).FieldID, fieldID, true) == 0)
        {
          cb.SelectedItem = obj;
          break;
        }
      }
    }

    private string[] fillDropdown(ref string fieldID, ComboBox cb)
    {
      string[] strArray = (string[]) null;
      cb.Items.Clear();
      switch (cb.Tag.ToString())
      {
        case "DTI":
          cb.Items.Add((object) new FieldDropdownItem("742", "DTI"));
          cb.Items.Add((object) new FieldDropdownItem("AUSF.X17", "AUS Tracking - Latest Submission - Housing Ratio"));
          cb.Items.Add((object) new FieldDropdownItem("QM.X375", "ATR QM - Qualification - Initial Rate Housing Ratio"));
          cb.Items.Add((object) new FieldDropdownItem("QM.X115", "ATR QM - Qualification - Fully Index Rate Housing Ratio"));
          cb.Items.Add((object) new FieldDropdownItem("QM.X118", "ATR QM - Qualification - Max Rate During First 5 Years - Housing Ratio"));
          if (string.IsNullOrEmpty(fieldID))
            fieldID = "742";
          strArray = new string[5]
          {
            "742",
            "AUSF.X17",
            "QM.X375",
            "QM.X115",
            "QM.X118"
          };
          break;
        case "CLTV":
          cb.Items.Add((object) new FieldDropdownItem("976", "CLTV"));
          cb.Items.Add((object) new FieldDropdownItem("AUSF.X18", "AUS Tracking - Latest Submission - Total Expense Ratio"));
          cb.Items.Add((object) new FieldDropdownItem("QM.X376", "ATR QM - Qualification - Initial Rate Total Debt Ratio"));
          cb.Items.Add((object) new FieldDropdownItem("QM.X116", "ATR QM - Qualification - Fully Index Rate Total Debt Ratio"));
          cb.Items.Add((object) new FieldDropdownItem("QM.X119", "ATR QM - Qualification - Max Rate During First 5 Years - Total Debt Ratio"));
          if (string.IsNullOrEmpty(fieldID))
            fieldID = "976";
          strArray = new string[5]
          {
            "976",
            "AUSF.X18",
            "QM.X376",
            ".X116",
            "QM.X119"
          };
          break;
        case "Income":
          cb.Items.Add((object) new FieldDropdownItem("1389", "Income"));
          cb.Items.Add((object) new FieldDropdownItem("736", "Income Total Mo. Income"));
          if (string.IsNullOrEmpty(fieldID))
            fieldID = "1389";
          strArray = new string[2]{ "1389", "736" };
          break;
      }
      return strArray;
    }

    private void cmbApplicationDate_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbApplicationDate.SelectedItem == null)
        return;
      if (!(this.cmbApplicationDate.SelectedItem is FieldDropdownItem fieldDropdownItem))
        fieldDropdownItem = this.selectField();
      if (fieldDropdownItem == null)
        fieldDropdownItem = this.currentAppDateSelection;
      if (!this.cmbApplicationDate.Items.Contains((object) fieldDropdownItem))
        this.cmbApplicationDate.Items.Insert(this.cmbApplicationDate.Items.Count - 1, (object) fieldDropdownItem);
      if (!this.cmbApplicationDate.SelectedItem.Equals((object) fieldDropdownItem))
        this.cmbApplicationDate.SelectedItem = (object) fieldDropdownItem;
      if (fieldDropdownItem.Equals((object) this.currentAppDateSelection))
        return;
      this.currentAppDateSelection = fieldDropdownItem;
    }

    private void FieldDropdownItem_SelectedIndexChanged(object sender, EventArgs e)
    {
      ComboBox cb = sender as ComboBox;
      if (cb.SelectedItem == null)
        return;
      FieldDropdownItem selectedItem = cb.SelectedItem as FieldDropdownItem;
      FieldDropdownItem dropdownItemSelection = this.getCurrentFieldDropdownItemSelection(cb);
      if (selectedItem == null)
        selectedItem = this.selectField();
      if (selectedItem == null)
        selectedItem = dropdownItemSelection;
      if (!cb.Items.Contains((object) selectedItem))
        cb.Items.Insert(cb.Items.Count - 1, (object) selectedItem);
      if (!cb.SelectedItem.Equals((object) selectedItem))
        cb.SelectedItem = (object) selectedItem;
      if (selectedItem.Equals((object) dropdownItemSelection))
        return;
      this.setCurrentFieldDropdownItemSelection(cb, selectedItem);
    }

    private FieldDropdownItem getCurrentFieldDropdownItemSelection(ComboBox cb)
    {
      switch (cb.Tag.ToString())
      {
        case "DTI":
          return this.currentReliedUponFactorsDTISelection;
        case "CLTV":
          return this.currentReliedUponFactorsCLTVSelection;
        case "Income":
          return this.currentReliedUponFactorsIncomeSelection;
        default:
          return (FieldDropdownItem) null;
      }
    }

    private void setCurrentFieldDropdownItemSelection(ComboBox cb, FieldDropdownItem selectedItem)
    {
      switch (cb.Tag.ToString())
      {
        case "DTI":
          this.currentReliedUponFactorsDTISelection = selectedItem;
          break;
        case "CLTV":
          this.currentReliedUponFactorsCLTVSelection = selectedItem;
          break;
        case "Income":
          this.currentReliedUponFactorsIncomeSelection = selectedItem;
          break;
      }
    }

    private FieldDropdownItem selectField()
    {
      using (ReportFieldSelector reportFieldSelector = new ReportFieldSelector((ReportFieldDefs) LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.LoanDataFieldsInDatabase), (EllieMae.EMLite.ClientServer.Reporting.FieldTypes[]) null, true, true, this.session))
        return reportFieldSelector.ShowDialog((IWin32Window) this) != DialogResult.OK ? (FieldDropdownItem) null : new FieldDropdownItem(reportFieldSelector.SelectedField.FieldID, reportFieldSelector.SelectedField.FieldDefinition.Description);
    }

    private ZipCodeInfo getZipCodeRelatedInfo(object sender)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox == null)
        return (ZipCodeInfo) null;
      string str = textBox.Text.Trim();
      return str.Length < 5 ? (ZipCodeInfo) null : ZipcodeSelector.GetZipCodeInfo(str.Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(str.Substring(0, 5)));
    }

    private void txtZip_Leave(object sender, EventArgs e)
    {
      ZipCodeInfo zipCodeRelatedInfo = this.getZipCodeRelatedInfo(sender);
      if (zipCodeRelatedInfo == null)
        return;
      this.txtCity.Text = zipCodeRelatedInfo.City;
      this.cmbState.SelectedItem = (object) zipCodeRelatedInfo.State.ToUpper();
    }

    private void txtParentMailingZip_Leave(object sender, EventArgs e)
    {
      ZipCodeInfo zipCodeRelatedInfo = this.getZipCodeRelatedInfo(sender);
      if (zipCodeRelatedInfo == null)
        return;
      this.txtParentMailingCity.Text = zipCodeRelatedInfo.City;
      this.cmbParentMailingState.SelectedItem = (object) zipCodeRelatedInfo.State.ToUpper();
    }

    private void TextChange(object sender, EventArgs e)
    {
      if (!this.IsDirty)
        return;
      this.stdButtonReset.Enabled = true;
    }

    private void CheckChange(object sender, EventArgs e)
    {
      if (!this.IsDirty)
        return;
      this.stdButtonReset.Enabled = true;
    }

    private void SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.IsDirty)
        return;
      this.stdButtonReset.Enabled = true;
    }

    private void txtZip_KeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.ZIPCODE;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void Phone_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
        return;
      FieldFormat dataFormat = FieldFormat.PHONE;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void LoadHMDAProfiles(List<HMDAProfile> HMDAProfiles)
    {
      this.gvHMDAProfile.Items.Clear();
      foreach (HMDAProfile hmdaProfile in HMDAProfiles)
      {
        GVItem gvItem = new GVItem(hmdaProfile.HMDAProfileName);
        gvItem.Tag = (object) hmdaProfile;
        gvItem.SubItems.Add((object) hmdaProfile.HMDAProfileRespondentID);
        gvItem.SubItems.Add((object) hmdaProfile.HMDAProfileLEI);
        gvItem.SubItems.Add((object) hmdaProfile.HMDAProfileCompanyName);
        if (hmdaProfile.HMDAProfileAgency.Equals(""))
          hmdaProfile.HMDAProfileAgency = "0";
        gvItem.SubItems.Add((object) this.agencies[hmdaProfile.HMDAProfileAgency]);
        gvItem.SubItems[gvItem.SubItems.Count - 1].SortValue = (object) Utils.ParseInt((object) hmdaProfile.HMDAProfileAgency);
        gvItem.SubItems.Add((object) hmdaProfile.HMDAProfileLastModifiedBy);
        gvItem.SubItems.Add((object) hmdaProfile.HMDAProfileLastModifiedDate);
        this.gvHMDAProfile.Items.Add(gvItem);
      }
    }

    private void EnableDisableAllFields(bool isEnabled)
    {
      foreach (Control control in (ArrangedElementCollection) this.groupContainer3.Controls)
        this.EnableDisableControl(control, isEnabled);
      foreach (Control control in (ArrangedElementCollection) this.panel1.Controls)
        this.EnableDisableControl(control, isEnabled);
      foreach (Control control in (ArrangedElementCollection) this.panel2.Controls)
        this.EnableDisableControl(control, isEnabled);
      foreach (Control control in (ArrangedElementCollection) this.panel3.Controls)
        this.EnableDisableControl(control, isEnabled);
      foreach (Control control in (ArrangedElementCollection) this.panel4.Controls)
        this.EnableDisableControl(control, isEnabled);
      foreach (Control control in (ArrangedElementCollection) this.panel5.Controls)
        this.EnableDisableControl(control, isEnabled);
      foreach (Control control in (ArrangedElementCollection) this.panel7.Controls)
        this.EnableDisableControl(control, isEnabled);
      foreach (Control control in (ArrangedElementCollection) this.panel8.Controls)
        this.EnableDisableControl(control, isEnabled);
    }

    private void EnableDisableControl(Control ctrl, bool isEnabled)
    {
      if (ctrl.GetType() == typeof (TextBox))
        ((TextBoxBase) ctrl).ReadOnly = !isEnabled;
      if (ctrl.GetType() == typeof (ComboBox))
        ctrl.Enabled = isEnabled;
      if (ctrl.GetType() == typeof (CheckBox))
        ctrl.Enabled = isEnabled;
      if (ctrl.GetType() == typeof (DateTimePicker))
        ctrl.Enabled = isEnabled;
      if (!(ctrl.GetType() == typeof (RadioButton)))
        return;
      ctrl.Enabled = isEnabled;
    }

    private void InitAgencyList()
    {
      this.agencies = new Dictionary<string, string>();
      this.agencies.Add("0", string.Empty);
      this.agencies.Add("1", "1. The Office of Comptroller of the Currency (OCC)");
      this.agencies.Add("2", "2. Federal Reserve System (FRS)");
      this.agencies.Add("3", "3. Federal Deposit Insurance Corporation (FDIC)");
      this.agencies.Add("5", "5. National Credit Union Administration (NCUA)");
      this.agencies.Add("7", "7. United States Department of Housing and Urban Development (HUD)");
      this.agencies.Add("9", "9. Consumer Financial Protection Bureau (CFPB)");
    }

    private void LoadHMDASetting(HMDAProfile hmdaProfile)
    {
      this.EnableDisableAllFields(false);
      if (hmdaProfile == null)
      {
        this.loadAgencies("");
        this.loadApplicationSetting("");
        this.loadFieldDropdownItemSetting("", this.cboDTI);
        this.loadFieldDropdownItemSetting("", this.cboCLTV);
        this.loadFieldDropdownItemSetting("", this.cboIncome);
      }
      else
      {
        this._hmdaInformationCached = new HMDAInformation(hmdaProfile.HMDAProfileSetting);
        this.txtProfileName.Text = hmdaProfile.HMDAProfileName;
        this.txtRespondentId.Text = this._hmdaInformationCached.HMDARespondentID;
        this.txtTaxId.Text = this._hmdaInformationCached.HMDARespondentTaxID;
        this.txtLEI.Text = this._hmdaInformationCached.HMDALEI;
        this.loadAgencies(this._hmdaInformationCached.HMDARespondentAgency);
        this.txtCompanyName.Text = this._hmdaInformationCached.HMDACompanyName;
        this.txtName.Text = this._hmdaInformationCached.HMDAContactName;
        this.txtAddress.Text = this._hmdaInformationCached.HMDAContactAddressLine1;
        this.txtCity.Text = this._hmdaInformationCached.HMDAContactCity;
        this.cmbState.SelectedItem = (object) this._hmdaInformationCached.HMDAContactState;
        this.txtZip.Text = this._hmdaInformationCached.HMDAContactZipCode;
        this.txtPhone.Text = this._hmdaInformationCached.HMDAContactPhone;
        this.txtFax.Text = this._hmdaInformationCached.HMDAContactFax;
        this.txtEmail.Text = this._hmdaInformationCached.HMDAContactEmail;
        this.txtParentMailingName.Text = this._hmdaInformationCached.HMDAParentName;
        this.txtParentMailingAddress.Text = this._hmdaInformationCached.HMDAParentAddressLine1;
        this.txtParentMailingCity.Text = this._hmdaInformationCached.HMDAParentCity;
        this.cmbParentMailingState.SelectedItem = (object) this._hmdaInformationCached.HMDAParentState;
        this.txtParentMailingZip.Text = this._hmdaInformationCached.HMDAParentZipCode;
        this.txtNuli.Checked = this._hmdaInformationCached.HMDANuli;
        this.loadApplicationSetting(this._hmdaInformationCached.HMDAApplicationDate);
        this.loadFieldDropdownItemSetting(this._hmdaInformationCached.HMDADTI, this.cboDTI);
        this.loadFieldDropdownItemSetting(this._hmdaInformationCached.HMDACLTV, this.cboCLTV);
        this.loadFieldDropdownItemSetting(this._hmdaInformationCached.HMDAIncome, this.cboIncome);
        if (this._hmdaInformationCached.HMDAInstitutionPurchaseLoans)
          this.rdBtnPurchaseLoansYes.Checked = true;
        else
          this.rdBtnPurchaseLoansNo.Checked = true;
        this.cmbLoansNoChannel.SelectedItem = (object) this._hmdaInformationCached.HMDAChannelInfoNoChannel;
        this.cmbLoansCorrespondent.SelectedItem = (object) this._hmdaInformationCached.HMDAChannelInfoCorrespondent;
        if (this._hmdaInformationCached.HMDAShowDemographicInfo)
          this.rdBtnReportInfoYes.Checked = true;
        else
          this.rdBtnReportInfoNo.Checked = true;
        if (this._hmdaInformationCached.HMDAReportIncome)
          this.rdBtnRprtIncmYes.Checked = true;
        else
          this.rdBtnRprtIncmNo.Checked = true;
        if (this._hmdaInformationCached.HMDAReportAgeOfBorrower)
          this.rdButtonBorrowerAgeYes.Checked = true;
        else
          this.rdButtonBorrowerAgeNo.Checked = true;
        if (this._hmdaInformationCached.HMDAReportAgeOfCoBorrower)
          this.rdButtonCoBorrowerAgeYes.Checked = true;
        else
          this.rdButtonCoBorrowerAgeNo.Checked = true;
        if (this._hmdaInformationCached.HMDADisplayRateSpreadTo3Decimals)
          this.rdBtnRateSpreadYes.Checked = true;
        else
          this.rdBtnRateSpreadNo.Checked = true;
      }
    }

    private void LoadHMDAProfileForm(HMDASettingsForm.ActionPerformed action)
    {
      this.hmdaProfiles = Session.ConfigurationManager.GetHMDAProfile();
      this.LoadHMDAProfiles(this.hmdaProfiles);
      int nItemIndex = 0;
      if (this.gvHMDAProfile.Items.Count > 0)
      {
        switch (action)
        {
          case HMDASettingsForm.ActionPerformed.Create:
            int maxProfileID = this.hmdaProfiles.Max<HMDAProfile>((Func<HMDAProfile, int>) (p => p.HMDAProfileID));
            nItemIndex = this.hmdaProfiles.FindIndex((Predicate<HMDAProfile>) (p => p.HMDAProfileID == maxProfileID));
            break;
          case HMDASettingsForm.ActionPerformed.Edit:
            nItemIndex = this.hmdaProfiles.FindIndex((Predicate<HMDAProfile>) (p => p.HMDAProfileID == this.profile.HMDAProfileID));
            break;
        }
        this.gvHMDAProfile.Items[nItemIndex].Selected = true;
      }
      else
      {
        this.ClearControlFields();
        this.LoadHMDASetting((HMDAProfile) null);
      }
    }

    private void performSave()
    {
      this.setDirtyFlag(false);
      this.EnableDisableAllFields(false);
      this.SaveHMDAProfile(this.profile);
      if (this.profile.HMDAProfileID == 0)
        this.LoadHMDAProfileForm(HMDASettingsForm.ActionPerformed.Create);
      else
        this.LoadHMDAProfileForm(HMDASettingsForm.ActionPerformed.Edit);
      this.txtProfileName.Focus();
      this.stdButtonSave.Enabled = false;
      this.stdButtonEdit.Enabled = this.hasEditLEIPermission && this.gvHMDAProfile.Items.Count > 0;
      this.stdButtonDelete.Enabled = this.hasRemoveLEIPermission && this.gvHMDAProfile.Items.Count > 0;
      this.stdButtonSave.Enabled = false;
      this.stdButtonReset.Enabled = false;
    }

    private void stdButtonSave_Click(object sender, EventArgs e)
    {
      if (!this.VerifyProfileName(this.txtProfileName.Text))
        return;
      this.performSave();
    }

    public override bool SaveRtnBool()
    {
      if (!this.VerifyProfileName(this.txtProfileName.Text))
        return false;
      this.performSave();
      return true;
    }

    private void SaveHMDAProfile(HMDAProfile profile)
    {
      HMDAInformation hmdaInformation = new HMDAInformation();
      hmdaInformation.HMDARespondentID = this.txtRespondentId.Text.Trim();
      hmdaInformation.HMDARespondentTaxID = this.txtTaxId.Text.Trim();
      hmdaInformation.HMDARespondentAgency = this.cmbAgency.SelectedValue.ToString() == "0" ? "" : this.cmbAgency.SelectedValue.ToString();
      hmdaInformation.HMDALEI = this.txtLEI.Text.Trim();
      hmdaInformation.HMDACompanyName = this.txtCompanyName.Text.Trim();
      hmdaInformation.HMDAContactName = this.txtName.Text.Trim();
      hmdaInformation.HMDAContactAddressLine1 = this.txtAddress.Text.Trim();
      hmdaInformation.HMDAContactCity = this.txtCity.Text.Trim();
      if (this.cmbState.SelectedItem != null)
        hmdaInformation.HMDAContactState = this.cmbState.SelectedItem.ToString();
      hmdaInformation.HMDAContactZipCode = this.txtZip.Text.Trim();
      hmdaInformation.HMDAContactPhone = this.txtPhone.Text.Trim();
      hmdaInformation.HMDAContactFax = this.txtFax.Text.Trim();
      hmdaInformation.HMDAContactEmail = this.txtEmail.Text.Trim();
      hmdaInformation.HMDAParentName = this.txtParentMailingName.Text.Trim();
      hmdaInformation.HMDAParentAddressLine1 = this.txtParentMailingAddress.Text;
      hmdaInformation.HMDAParentCity = this.txtParentMailingCity.Text;
      if (this.cmbParentMailingState.SelectedItem != null)
        hmdaInformation.HMDAParentState = this.cmbParentMailingState.SelectedItem.ToString();
      hmdaInformation.HMDAParentZipCode = this.txtParentMailingZip.Text;
      hmdaInformation.HMDAApplicationDate = ((FieldDropdownItem) this.cmbApplicationDate.SelectedItem).FieldID;
      hmdaInformation.HMDADTI = ((FieldDropdownItem) this.cboDTI.SelectedItem).FieldID;
      hmdaInformation.HMDACLTV = ((FieldDropdownItem) this.cboCLTV.SelectedItem).FieldID;
      hmdaInformation.HMDAIncome = ((FieldDropdownItem) this.cboIncome.SelectedItem).FieldID;
      hmdaInformation.HMDAInstitutionPurchaseLoans = this.rdBtnPurchaseLoansYes.Checked;
      if (this.cmbLoansNoChannel.SelectedItem != null)
        hmdaInformation.HMDAChannelInfoNoChannel = this.cmbLoansNoChannel.SelectedItem.ToString();
      if (this.cmbLoansCorrespondent.SelectedItem != null)
        hmdaInformation.HMDAChannelInfoCorrespondent = this.cmbLoansCorrespondent.SelectedItem.ToString();
      hmdaInformation.HMDAShowDemographicInfo = this.rdBtnReportInfoYes.Checked;
      hmdaInformation.HMDAReportIncome = this.rdBtnRprtIncmYes.Checked;
      hmdaInformation.HMDAReportAgeOfBorrower = this.rdButtonBorrowerAgeYes.Checked;
      hmdaInformation.HMDAReportAgeOfCoBorrower = this.rdButtonCoBorrowerAgeYes.Checked;
      hmdaInformation.HMDADisplayRateSpreadTo3Decimals = this.rdBtnRateSpreadYes.Checked;
      hmdaInformation.HMDANuli = this.txtNuli.Checked;
      profile.HMDAProfileName = this.txtProfileName.Text;
      profile.HMDAProfileRespondentID = hmdaInformation.HMDARespondentID;
      profile.HMDAProfileLEI = hmdaInformation.HMDALEI;
      profile.HMDAProfileCompanyName = this.txtCompanyName.Text;
      profile.HMDAProfileAgency = hmdaInformation.HMDARespondentAgency;
      profile.HMDAProfileLastModifiedBy = this.session.UserID;
      profile.HMDAProfileLastModifiedDate = DateTime.Now;
      profile.HMDAProfileSetting = hmdaInformation.ToXmlString();
      this.session.ConfigurationManager.UpdateHMDAProfile(profile);
    }

    private void stdButtonNew_Click(object sender, EventArgs e)
    {
      if (!this.hasAddLEIPermission)
        return;
      this.EnableDisableAllFields(true);
      if (this.gvHMDAProfile.SelectedItems.Count > 0)
      {
        this.gvHMDAProfile.SelectedIndexChanged -= new EventHandler(this.HMDAProfileList_SelectedIndexChanged);
        this.gvHMDAProfile.SelectedItems[0].Selected = false;
        this.gvHMDAProfile.SelectedIndexChanged += new EventHandler(this.HMDAProfileList_SelectedIndexChanged);
      }
      this.ClearControlFields();
      this.txtProfileName.Focus();
      this.profile = new HMDAProfile();
      this.stdButtonSave.Enabled = true;
      this.setDirtyFlag(true);
    }

    private void stdButtonEdit_Click(object sender, EventArgs e)
    {
      if (this.gvHMDAProfile.SelectedItems.Count == 0 || !this.hasEditLEIPermission)
        return;
      this.EnableDisableAllFields(true);
      this.txtProfileName.Focus();
      string text = this.gvHMDAProfile.SelectedItems[0].SubItems[0].Text;
      foreach (HMDAProfile hmdaProfile in this.hmdaProfiles)
      {
        if (string.Compare(text, hmdaProfile.HMDAProfileName, StringComparison.CurrentCultureIgnoreCase) == 0)
        {
          this.profile = hmdaProfile;
          break;
        }
      }
      this.stdButtonSave.Enabled = this.hasEditLEIPermission;
      this.setDirtyFlag(true);
    }

    private bool VerifyProfileName(string profileName)
    {
      if (string.IsNullOrEmpty(this.txtProfileName.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide details for mandatory fields.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.DoesProfileNameExist(this.txtProfileName.Text.Trim()) && (this.gvHMDAProfile.SelectedItems.Count == 0 || string.Compare(this.txtProfileName.Text.Trim(), this.profile.HMDAProfileName, StringComparison.CurrentCultureIgnoreCase) != 0))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The profile name already exists", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.txtLEI.Text.Trim() != string.Empty && !this.txtLEI.Text.Trim().IsAlphaNumeric())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter alphanumeric characters for LEI.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (!(this.txtEmail.Text.Trim() != string.Empty) || Utils.ValidateEmail(this.txtEmail.Text.Trim()))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The email address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.txtEmail.Focus();
      return false;
    }

    private bool DoesProfileNameExist(string profileName)
    {
      return Session.ConfigurationManager.DoesProfileNameExist(profileName);
    }

    public void HMDAProfileList_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.IsDirty && this.hasEditLEIPermission)
      {
        if (Utils.Dialog((IWin32Window) this, "Do you want to save the change? Otherwise the change would be discarded", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        {
          if (this.VerifyProfileName(this.txtProfileName.Text))
          {
            this.setDirtyFlag(false);
            this.SaveHMDAProfile(this.profile);
            this.LoadHMDAProfileForm(HMDASettingsForm.ActionPerformed.Edit);
            this.txtProfileName.Focus();
            this.stdButtonSave.Enabled = false;
            this.stdButtonEdit.Enabled = this.hasEditLEIPermission && this.gvHMDAProfile.Items.Count > 0;
            this.stdButtonDelete.Enabled = this.hasRemoveLEIPermission && this.gvHMDAProfile.Items.Count > 0;
            return;
          }
          this.gvHMDAProfile.SelectedIndexChanged -= new EventHandler(this.HMDAProfileList_SelectedIndexChanged);
          this.gvHMDAProfile.SelectedItems.Clear();
          if (this.profile.HMDAProfileID != 0)
          {
            foreach (HMDAProfile hmdaProfile in this.hmdaProfiles)
            {
              if (hmdaProfile.HMDAProfileID == this.profile.HMDAProfileID)
              {
                this.profile.HMDAProfileName = hmdaProfile.HMDAProfileName;
                break;
              }
            }
            foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvHMDAProfile.Items)
            {
              if (gvItem.SubItems[0].Text == this.profile.HMDAProfileName)
              {
                gvItem.Selected = true;
                break;
              }
            }
          }
          this.gvHMDAProfile.SelectedIndexChanged += new EventHandler(this.HMDAProfileList_SelectedIndexChanged);
          return;
        }
        this.setDirtyFlag(false);
        this.stdButtonReset.Enabled = false;
        this.stdButtonSave.Enabled = false;
      }
      if (this.gvHMDAProfile.SelectedItems.Count <= 0)
        return;
      string text = this.gvHMDAProfile.SelectedItems[0].SubItems[0].Text;
      foreach (HMDAProfile hmdaProfile in this.hmdaProfiles)
      {
        if (string.Compare(text, hmdaProfile.HMDAProfileName, StringComparison.CurrentCultureIgnoreCase) == 0)
        {
          this.profile = hmdaProfile;
          break;
        }
      }
      this.LoadHMDASetting(this.profile);
    }

    private void stdButtonDelete_Click(object sender, EventArgs e)
    {
      if (this.gvHMDAProfile.SelectedItems.Count == 0 || !this.hasRemoveLEIPermission || Utils.Dialog((IWin32Window) this, "The selected HMDA Profile will be deleted. Do you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        return;
      if (this.IsAssociateToOrg(this.gvHMDAProfile.SelectedItems[0].Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected HMDA Profile is associated to " + this.GetOrgNameByHMDAProfile(this.gvHMDAProfile.SelectedItems[0].Text) + " and cannot be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        Session.ConfigurationManager.DeleteProfileName(this.gvHMDAProfile.SelectedItems[0].Text);
        this.LoadHMDAProfileForm(HMDASettingsForm.ActionPerformed.Delete);
        this.stdButtonDelete.Enabled = this.gvHMDAProfile.Items.Count > 0 && this.hasRemoveLEIPermission;
        this.stdButtonEdit.Enabled = this.gvHMDAProfile.Items.Count > 0 && this.hasEditLEIPermission;
      }
    }

    private bool IsAssociateToOrg(string profileName)
    {
      int profileID = -1;
      foreach (HMDAProfile hmdaProfile in this.hmdaProfiles)
      {
        if (hmdaProfile.HMDAProfileName == profileName)
        {
          profileID = hmdaProfile.HMDAProfileID;
          break;
        }
      }
      return Session.ConfigurationManager.IsAssociateToOrg(profileID);
    }

    private string GetOrgNameByHMDAProfile(string profileName)
    {
      int profileID = -1;
      foreach (HMDAProfile hmdaProfile in this.hmdaProfiles)
      {
        if (hmdaProfile.HMDAProfileName == profileName)
        {
          profileID = hmdaProfile.HMDAProfileID;
          break;
        }
      }
      return Session.ConfigurationManager.GetOrgNameByHMDAProfile(profileID);
    }

    private void ClearControlFields()
    {
      foreach (object control in (ArrangedElementCollection) this.groupContainer3.Controls)
      {
        if (control is TextBox)
          ((Control) control).Text = string.Empty;
      }
      foreach (object control in (ArrangedElementCollection) this.panel1.Controls)
      {
        if (control is TextBox)
          ((Control) control).Text = string.Empty;
      }
      foreach (object control in (ArrangedElementCollection) this.panel2.Controls)
      {
        if (control is TextBox)
          ((Control) control).Text = string.Empty;
      }
      foreach (object control in (ArrangedElementCollection) this.panel3.Controls)
      {
        if (control is TextBox)
          ((Control) control).Text = string.Empty;
      }
      foreach (object control in (ArrangedElementCollection) this.panel4.Controls)
      {
        if (control is TextBox)
          ((Control) control).Text = string.Empty;
      }
      foreach (object control in (ArrangedElementCollection) this.panel5.Controls)
      {
        if (control is TextBox)
          ((Control) control).Text = string.Empty;
      }
      this.loadAgencies("");
      this.loadApplicationSetting("");
      this.loadFieldDropdownItemSetting("", this.cboDTI);
      this.loadFieldDropdownItemSetting("", this.cboCLTV);
      this.loadFieldDropdownItemSetting("", this.cboIncome);
      this.rdBtnReportInfoNo.Checked = this.rdBtnRprtIncmNo.Checked = this.rdBtnRateSpreadNo.Checked = true;
      this.rdButtonBorrowerAgeNo.Checked = this.rdButtonCoBorrowerAgeNo.Checked = true;
      this.txtNuli.Checked = false;
    }

    private void stdButtonReset_Click(object sender, EventArgs e)
    {
      if (this.gvHMDAProfile.SelectedItems.Count == 0)
      {
        this.ClearControlFields();
      }
      else
      {
        string text = this.gvHMDAProfile.SelectedItems[0].Text;
        foreach (HMDAProfile hmdaProfile in this.hmdaProfiles)
        {
          if (text == hmdaProfile.HMDAProfileName)
            this.LoadHMDASetting(hmdaProfile);
        }
        this.stdButtonSave.Enabled = false;
        this.stdButtonReset.Enabled = false;
        this.setDirtyFlag(false);
      }
    }

    private void gvHMDAProfile_DoubleClick(object sender, EventArgs e)
    {
      this.stdButtonEdit_Click(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      this.groupContainer1 = new GroupContainer();
      this.panel1 = new Panel();
      this.panel8 = new Panel();
      this.label46 = new Label();
      this.rdButtonCoBorrowerAgeYes = new RadioButton();
      this.rdButtonCoBorrowerAgeNo = new RadioButton();
      this.panel7 = new Panel();
      this.label44 = new Label();
      this.rdButtonBorrowerAgeYes = new RadioButton();
      this.rdButtonBorrowerAgeNo = new RadioButton();
      this.cboIncome = new ComboBox();
      this.cboCLTV = new ComboBox();
      this.cboDTI = new ComboBox();
      this.label40 = new Label();
      this.label39 = new Label();
      this.label38 = new Label();
      this.label37 = new Label();
      this.verticalSeparator3 = new Label();
      this.gradientPanel6 = new GradientPanel();
      this.label17 = new Label();
      this.verticalSeparator2 = new Label();
      this.verticalSeparator1 = new Label();
      this.horizontalSeparator1 = new Label();
      this.txtName = new TextBox();
      this.label36 = new Label();
      this.panel4 = new Panel();
      this.radioButton5 = new RadioButton();
      this.label33 = new Label();
      this.label30 = new Label();
      this.radioButton6 = new RadioButton();
      this.rdBtnRprtIncmYes = new RadioButton();
      this.rdBtnRprtIncmNo = new RadioButton();
      this.gradientPanel5 = new GradientPanel();
      this.label31 = new Label();
      this.label29 = new Label();
      this.txtLEI = new TextBox();
      this.txtNuli = new CheckBox();
      this.label28 = new Label();
      this.panel3 = new Panel();
      this.radioButton3 = new RadioButton();
      this.label32 = new Label();
      this.radioButton4 = new RadioButton();
      this.rdBtnReportInfoNo = new RadioButton();
      this.label27 = new Label();
      this.rdBtnReportInfoYes = new RadioButton();
      this.panel2 = new Panel();
      this.rdBtnPurchaseLoansNo = new RadioButton();
      this.rdBtnPurchaseLoansYes = new RadioButton();
      this.label9 = new Label();
      this.gradientPanel4 = new GradientPanel();
      this.label26 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.cmbLoansCorrespondent = new ComboBox();
      this.label2 = new Label();
      this.cmbLoansNoChannel = new ComboBox();
      this.gradientPanel3 = new GradientPanel();
      this.label25 = new Label();
      this.cmbApplicationDate = new ComboBox();
      this.label8 = new Label();
      this.gradientPanel2 = new GradientPanel();
      this.label7 = new Label();
      this.cmbParentMailingState = new ComboBox();
      this.label19 = new Label();
      this.txtParentMailingName = new TextBox();
      this.txtParentMailingAddress = new TextBox();
      this.txtParentMailingCity = new TextBox();
      this.txtParentMailingZip = new TextBox();
      this.label20 = new Label();
      this.label21 = new Label();
      this.label22 = new Label();
      this.label23 = new Label();
      this.label24 = new Label();
      this.cmbState = new ComboBox();
      this.label18 = new Label();
      this.txtPhone = new TextBox();
      this.txtFax = new TextBox();
      this.txtEmail = new TextBox();
      this.label11 = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.txtCompanyName = new TextBox();
      this.txtAddress = new TextBox();
      this.txtCity = new TextBox();
      this.txtZip = new TextBox();
      this.label14 = new Label();
      this.label13 = new Label();
      this.label12 = new Label();
      this.label10 = new Label();
      this.label6 = new Label();
      this.cmbAgency = new ComboBox();
      this.label5 = new Label();
      this.txtTaxId = new TextBox();
      this.lblTaxId = new Label();
      this.txtRespondentId = new TextBox();
      this.lblRespondentId = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.panel5 = new Panel();
      this.radioButton7 = new RadioButton();
      this.label34 = new Label();
      this.radioButton8 = new RadioButton();
      this.rdBtnRateSpreadNo = new RadioButton();
      this.label35 = new Label();
      this.rdBtnRateSpreadYes = new RadioButton();
      this.groupContainer2 = new GroupContainer();
      this.gvHMDAProfile = new GridView();
      this.groupContainer3 = new GroupContainer();
      this.label42 = new Label();
      this.txtProfileName = new TextBox();
      this.label41 = new Label();
      this.stdButtonReset = new StandardIconButton();
      this.stdButtonSave = new StandardIconButton();
      this.stdButtonNew = new StandardIconButton();
      this.stdButtonDelete = new StandardIconButton();
      this.stdButtonEdit = new StandardIconButton();
      this.groupContainer1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel8.SuspendLayout();
      this.panel7.SuspendLayout();
      this.gradientPanel6.SuspendLayout();
      this.panel4.SuspendLayout();
      this.gradientPanel5.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel2.SuspendLayout();
      this.gradientPanel4.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.panel5.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      ((ISupportInitialize) this.stdButtonReset).BeginInit();
      ((ISupportInitialize) this.stdButtonSave).BeginInit();
      ((ISupportInitialize) this.stdButtonNew).BeginInit();
      ((ISupportInitialize) this.stdButtonDelete).BeginInit();
      ((ISupportInitialize) this.stdButtonEdit).BeginInit();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.panel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 222);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1230, 578);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Respondent Information";
      this.panel1.AutoScroll = true;
      this.panel1.Controls.Add((Control) this.panel8);
      this.panel1.Controls.Add((Control) this.panel7);
      this.panel1.Controls.Add((Control) this.cboIncome);
      this.panel1.Controls.Add((Control) this.cboCLTV);
      this.panel1.Controls.Add((Control) this.cboDTI);
      this.panel1.Controls.Add((Control) this.label40);
      this.panel1.Controls.Add((Control) this.label39);
      this.panel1.Controls.Add((Control) this.label38);
      this.panel1.Controls.Add((Control) this.label37);
      this.panel1.Controls.Add((Control) this.verticalSeparator3);
      this.panel1.Controls.Add((Control) this.gradientPanel6);
      this.panel1.Controls.Add((Control) this.verticalSeparator2);
      this.panel1.Controls.Add((Control) this.verticalSeparator1);
      this.panel1.Controls.Add((Control) this.horizontalSeparator1);
      this.panel1.Controls.Add((Control) this.txtName);
      this.panel1.Controls.Add((Control) this.label36);
      this.panel1.Controls.Add((Control) this.panel4);
      this.panel1.Controls.Add((Control) this.gradientPanel5);
      this.panel1.Controls.Add((Control) this.label29);
      this.panel1.Controls.Add((Control) this.txtLEI);
      this.panel1.Controls.Add((Control) this.txtNuli);
      this.panel1.Controls.Add((Control) this.label28);
      this.panel1.Controls.Add((Control) this.panel3);
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Controls.Add((Control) this.gradientPanel4);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.cmbLoansCorrespondent);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.cmbLoansNoChannel);
      this.panel1.Controls.Add((Control) this.gradientPanel3);
      this.panel1.Controls.Add((Control) this.cmbApplicationDate);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.gradientPanel2);
      this.panel1.Controls.Add((Control) this.cmbParentMailingState);
      this.panel1.Controls.Add((Control) this.label19);
      this.panel1.Controls.Add((Control) this.txtParentMailingName);
      this.panel1.Controls.Add((Control) this.txtParentMailingAddress);
      this.panel1.Controls.Add((Control) this.txtParentMailingCity);
      this.panel1.Controls.Add((Control) this.txtParentMailingZip);
      this.panel1.Controls.Add((Control) this.label20);
      this.panel1.Controls.Add((Control) this.label21);
      this.panel1.Controls.Add((Control) this.label22);
      this.panel1.Controls.Add((Control) this.label23);
      this.panel1.Controls.Add((Control) this.label24);
      this.panel1.Controls.Add((Control) this.cmbState);
      this.panel1.Controls.Add((Control) this.label18);
      this.panel1.Controls.Add((Control) this.txtPhone);
      this.panel1.Controls.Add((Control) this.txtFax);
      this.panel1.Controls.Add((Control) this.txtEmail);
      this.panel1.Controls.Add((Control) this.label11);
      this.panel1.Controls.Add((Control) this.label15);
      this.panel1.Controls.Add((Control) this.label16);
      this.panel1.Controls.Add((Control) this.txtCompanyName);
      this.panel1.Controls.Add((Control) this.txtAddress);
      this.panel1.Controls.Add((Control) this.txtCity);
      this.panel1.Controls.Add((Control) this.txtZip);
      this.panel1.Controls.Add((Control) this.label14);
      this.panel1.Controls.Add((Control) this.label13);
      this.panel1.Controls.Add((Control) this.label12);
      this.panel1.Controls.Add((Control) this.label10);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.cmbAgency);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.txtTaxId);
      this.panel1.Controls.Add((Control) this.lblTaxId);
      this.panel1.Controls.Add((Control) this.txtRespondentId);
      this.panel1.Controls.Add((Control) this.lblRespondentId);
      this.panel1.Controls.Add((Control) this.gradientPanel1);
      this.panel1.Controls.Add((Control) this.panel5);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1228, 551);
      this.panel1.TabIndex = 0;
      this.panel8.Controls.Add((Control) this.label46);
      this.panel8.Controls.Add((Control) this.rdButtonCoBorrowerAgeYes);
      this.panel8.Controls.Add((Control) this.rdButtonCoBorrowerAgeNo);
      this.panel8.Location = new Point(1, 691);
      this.panel8.Name = "panel8";
      this.panel8.Size = new Size(329, 24);
      this.panel8.TabIndex = 96;
      this.label46.AutoSize = true;
      this.label46.Location = new Point(11, 4);
      this.label46.Name = "label46";
      this.label46.Size = new Size(147, 14);
      this.label46.TabIndex = 66;
      this.label46.Text = "Report Age of Co-Borrower?";
      this.rdButtonCoBorrowerAgeYes.AutoSize = true;
      this.rdButtonCoBorrowerAgeYes.Location = new Point(194, 2);
      this.rdButtonCoBorrowerAgeYes.Name = "rdButtonCoBorrowerAgeYes";
      this.rdButtonCoBorrowerAgeYes.Size = new Size(44, 18);
      this.rdButtonCoBorrowerAgeYes.TabIndex = 65;
      this.rdButtonCoBorrowerAgeYes.Text = "Yes";
      this.rdButtonCoBorrowerAgeYes.UseVisualStyleBackColor = true;
      this.rdButtonCoBorrowerAgeYes.CheckedChanged += new EventHandler(this.CheckChange);
      this.rdButtonCoBorrowerAgeNo.AutoSize = true;
      this.rdButtonCoBorrowerAgeNo.Location = new Point(244, 3);
      this.rdButtonCoBorrowerAgeNo.Name = "rdButtonCoBorrowerAgeNo";
      this.rdButtonCoBorrowerAgeNo.Size = new Size(38, 18);
      this.rdButtonCoBorrowerAgeNo.TabIndex = 64;
      this.rdButtonCoBorrowerAgeNo.TabStop = true;
      this.rdButtonCoBorrowerAgeNo.Text = "No";
      this.rdButtonCoBorrowerAgeNo.UseVisualStyleBackColor = true;
      this.rdButtonCoBorrowerAgeNo.CheckedChanged += new EventHandler(this.CheckChange);
      this.panel7.Controls.Add((Control) this.label44);
      this.panel7.Controls.Add((Control) this.rdButtonBorrowerAgeYes);
      this.panel7.Controls.Add((Control) this.rdButtonBorrowerAgeNo);
      this.panel7.Location = new Point(0, 664);
      this.panel7.Name = "panel7";
      this.panel7.Size = new Size(330, 24);
      this.panel7.TabIndex = 96;
      this.label44.AutoSize = true;
      this.label44.Location = new Point(11, 4);
      this.label44.Name = "label44";
      this.label44.Size = new Size(124, 14);
      this.label44.TabIndex = 66;
      this.label44.Text = "Report Age of Borrower";
      this.rdButtonBorrowerAgeYes.AutoSize = true;
      this.rdButtonBorrowerAgeYes.Location = new Point(194, 2);
      this.rdButtonBorrowerAgeYes.Name = "rdButtonBorrowerAgeYes";
      this.rdButtonBorrowerAgeYes.Size = new Size(44, 18);
      this.rdButtonBorrowerAgeYes.TabIndex = 65;
      this.rdButtonBorrowerAgeYes.Text = "Yes";
      this.rdButtonBorrowerAgeYes.UseVisualStyleBackColor = true;
      this.rdButtonBorrowerAgeYes.CheckedChanged += new EventHandler(this.CheckChange);
      this.rdButtonBorrowerAgeNo.AutoSize = true;
      this.rdButtonBorrowerAgeNo.Location = new Point(244, 3);
      this.rdButtonBorrowerAgeNo.Name = "rdButtonBorrowerAgeNo";
      this.rdButtonBorrowerAgeNo.Size = new Size(38, 18);
      this.rdButtonBorrowerAgeNo.TabIndex = 64;
      this.rdButtonBorrowerAgeNo.TabStop = true;
      this.rdButtonBorrowerAgeNo.Text = "No";
      this.rdButtonBorrowerAgeNo.UseVisualStyleBackColor = true;
      this.rdButtonBorrowerAgeNo.CheckedChanged += new EventHandler(this.CheckChange);
      this.cboIncome.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboIncome.FormattingEnabled = true;
      this.cboIncome.Location = new Point(647, 545);
      this.cboIncome.Name = "cboIncome";
      this.cboIncome.Size = new Size(432, 22);
      this.cboIncome.TabIndex = 109;
      this.cboIncome.Tag = (object) "Income";
      this.cboIncome.SelectedIndexChanged += new EventHandler(this.FieldDropdownItem_SelectedIndexChanged);
      this.cboCLTV.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCLTV.FormattingEnabled = true;
      this.cboCLTV.Location = new Point(647, 521);
      this.cboCLTV.Name = "cboCLTV";
      this.cboCLTV.Size = new Size(432, 22);
      this.cboCLTV.TabIndex = 108;
      this.cboCLTV.Tag = (object) "CLTV";
      this.cboCLTV.SelectedIndexChanged += new EventHandler(this.FieldDropdownItem_SelectedIndexChanged);
      this.cboDTI.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDTI.FormattingEnabled = true;
      this.cboDTI.Location = new Point(647, 497);
      this.cboDTI.Name = "cboDTI";
      this.cboDTI.Size = new Size(432, 22);
      this.cboDTI.TabIndex = 107;
      this.cboDTI.Tag = (object) "DTI";
      this.cboDTI.SelectedIndexChanged += new EventHandler(this.FieldDropdownItem_SelectedIndexChanged);
      this.label40.AutoSize = true;
      this.label40.Location = new Point(600, 524);
      this.label40.Name = "label40";
      this.label40.Size = new Size(33, 14);
      this.label40.TabIndex = 106;
      this.label40.Text = "CLTV";
      this.label39.AutoSize = true;
      this.label39.Location = new Point(600, 548);
      this.label39.Name = "label39";
      this.label39.Size = new Size(41, 14);
      this.label39.TabIndex = 105;
      this.label39.Text = "Income";
      this.label38.AutoSize = true;
      this.label38.Location = new Point(600, 500);
      this.label38.Name = "label38";
      this.label38.Size = new Size(22, 14);
      this.label38.TabIndex = 104;
      this.label38.Text = "DTI";
      this.label37.AutoEllipsis = true;
      this.label37.AutoSize = true;
      this.label37.Location = new Point(600, 476);
      this.label37.Name = "label37";
      this.label37.Size = new Size(397, 14);
      this.label37.TabIndex = 103;
      this.label37.Text = "Configure the default relied upon factors to be used in making the credit decision.";
      this.verticalSeparator3.BorderStyle = BorderStyle.Fixed3D;
      this.verticalSeparator3.Location = new Point(589, 438);
      this.verticalSeparator3.Name = "verticalSeparator3";
      this.verticalSeparator3.Size = new Size(2, 139);
      this.verticalSeparator3.TabIndex = 102;
      this.gradientPanel6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel6.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel6.Controls.Add((Control) this.label17);
      this.gradientPanel6.Location = new Point(589, 435);
      this.gradientPanel6.Name = "gradientPanel6";
      this.gradientPanel6.Size = new Size(1888, 28);
      this.gradientPanel6.TabIndex = 10;
      this.label17.AutoSize = true;
      this.label17.BackColor = Color.Transparent;
      this.label17.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label17.Location = new Point(11, 7);
      this.label17.Name = "label17";
      this.label17.Size = new Size(116, 14);
      this.label17.TabIndex = 4;
      this.label17.Text = "Relied Upon Factors";
      this.verticalSeparator2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.verticalSeparator2.BorderStyle = BorderStyle.Fixed3D;
      this.verticalSeparator2.Location = new Point(336, 575);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 4642);
      this.verticalSeparator2.TabIndex = 101;
      this.verticalSeparator1.BorderStyle = BorderStyle.Fixed3D;
      this.verticalSeparator1.Location = new Point(335, 106);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 331);
      this.verticalSeparator1.TabIndex = 100;
      this.horizontalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.horizontalSeparator1.BorderStyle = BorderStyle.Fixed3D;
      this.horizontalSeparator1.Location = new Point(3, 105);
      this.horizontalSeparator1.Name = "horizontalSeparator1";
      this.horizontalSeparator1.Size = new Size(2483, 2);
      this.horizontalSeparator1.TabIndex = 99;
      this.txtName.Location = new Point(105, 180);
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(224, 20);
      this.txtName.TabIndex = 31;
      this.label36.AutoSize = true;
      this.label36.Location = new Point(11, 183);
      this.label36.Name = "label36";
      this.label36.Size = new Size(37, 14);
      this.label36.TabIndex = 98;
      this.label36.Text = "Name:";
      this.panel4.Controls.Add((Control) this.radioButton5);
      this.panel4.Controls.Add((Control) this.label33);
      this.panel4.Controls.Add((Control) this.label30);
      this.panel4.Controls.Add((Control) this.radioButton6);
      this.panel4.Controls.Add((Control) this.rdBtnRprtIncmYes);
      this.panel4.Controls.Add((Control) this.rdBtnRprtIncmNo);
      this.panel4.Location = new Point(0, 637);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(337, 24);
      this.panel4.TabIndex = 95;
      this.radioButton5.AutoSize = true;
      this.radioButton5.Location = new Point(579, 11);
      this.radioButton5.Name = "radioButton5";
      this.radioButton5.Size = new Size(38, 18);
      this.radioButton5.TabIndex = 67;
      this.radioButton5.TabStop = true;
      this.radioButton5.Text = "No";
      this.radioButton5.UseVisualStyleBackColor = true;
      this.label33.AutoSize = true;
      this.label33.Location = new Point(346, 12);
      this.label33.Name = "label33";
      this.label33.Size = new Size(177, 14);
      this.label33.TabIndex = 69;
      this.label33.Text = "To report demographic information?";
      this.label30.AutoSize = true;
      this.label30.Location = new Point(11, 4);
      this.label30.Name = "label30";
      this.label30.Size = new Size(82, 14);
      this.label30.TabIndex = 66;
      this.label30.Text = "Report Income?";
      this.radioButton6.AutoSize = true;
      this.radioButton6.Location = new Point(529, 10);
      this.radioButton6.Name = "radioButton6";
      this.radioButton6.Size = new Size(44, 18);
      this.radioButton6.TabIndex = 68;
      this.radioButton6.TabStop = true;
      this.radioButton6.Text = "Yes";
      this.radioButton6.UseVisualStyleBackColor = true;
      this.rdBtnRprtIncmYes.AutoSize = true;
      this.rdBtnRprtIncmYes.Location = new Point(194, 2);
      this.rdBtnRprtIncmYes.Name = "rdBtnRprtIncmYes";
      this.rdBtnRprtIncmYes.Size = new Size(44, 18);
      this.rdBtnRprtIncmYes.TabIndex = 65;
      this.rdBtnRprtIncmYes.Text = "Yes";
      this.rdBtnRprtIncmYes.UseVisualStyleBackColor = true;
      this.rdBtnRprtIncmNo.AutoSize = true;
      this.rdBtnRprtIncmNo.Location = new Point(244, 3);
      this.rdBtnRprtIncmNo.Name = "rdBtnRprtIncmNo";
      this.rdBtnRprtIncmNo.Size = new Size(38, 18);
      this.rdBtnRprtIncmNo.TabIndex = 64;
      this.rdBtnRprtIncmNo.TabStop = true;
      this.rdBtnRprtIncmNo.Text = "No";
      this.rdBtnRprtIncmNo.UseVisualStyleBackColor = true;
      this.gradientPanel5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel5.AutoSize = true;
      this.gradientPanel5.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel5.Controls.Add((Control) this.label31);
      this.gradientPanel5.Location = new Point(338, 575);
      this.gradientPanel5.Name = "gradientPanel5";
      this.gradientPanel5.Size = new Size(2176, 28);
      this.gradientPanel5.TabIndex = 11;
      this.label31.AutoSize = true;
      this.label31.BackColor = Color.Transparent;
      this.label31.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label31.Location = new Point(8, 7);
      this.label31.Name = "label31";
      this.label31.Size = new Size(73, 14);
      this.label31.TabIndex = 4;
      this.label31.Text = "Rate Spread";
      this.label29.AutoSize = true;
      this.label29.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label29.Location = new Point(11, 158);
      this.label29.Name = "label29";
      this.label29.Size = new Size(92, 14);
      this.label29.TabIndex = 97;
      this.label29.Text = "Contact Person";
      this.txtLEI.Location = new Point(603, 56);
      this.txtLEI.MaxLength = 20;
      this.txtLEI.Name = "txtLEI";
      this.txtLEI.Size = new Size(370, 20);
      this.txtLEI.TabIndex = 14;
      this.txtLEI.TextChanged += new EventHandler(this.TextChange);
      this.txtNuli.Location = new Point(603, 84);
      this.txtNuli.Name = "txtNuli";
      this.txtNuli.Size = new Size(450, 18);
      this.txtNuli.TabIndex = 15;
      this.txtNuli.Text = "Use Non Universal Loan Identifier (NULI) for Loans Reported as Partially Exempt";
      this.txtNuli.CheckStateChanged += new EventHandler(this.CheckChange);
      this.label28.AutoSize = true;
      this.label28.Location = new Point(543, 58);
      this.label28.Name = "label28";
      this.label28.Size = new Size(24, 14);
      this.label28.TabIndex = 95;
      this.label28.Text = "LEI:";
      this.panel3.Controls.Add((Control) this.radioButton3);
      this.panel3.Controls.Add((Control) this.label32);
      this.panel3.Controls.Add((Control) this.radioButton4);
      this.panel3.Controls.Add((Control) this.rdBtnReportInfoNo);
      this.panel3.Controls.Add((Control) this.label27);
      this.panel3.Controls.Add((Control) this.rdBtnReportInfoYes);
      this.panel3.Location = new Point(0, 603);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(337, 34);
      this.panel3.TabIndex = 94;
      this.radioButton3.AutoSize = true;
      this.radioButton3.Location = new Point(579, 11);
      this.radioButton3.Name = "radioButton3";
      this.radioButton3.Size = new Size(38, 18);
      this.radioButton3.TabIndex = 67;
      this.radioButton3.TabStop = true;
      this.radioButton3.Text = "No";
      this.radioButton3.UseVisualStyleBackColor = true;
      this.label32.AutoSize = true;
      this.label32.Location = new Point(346, 12);
      this.label32.Name = "label32";
      this.label32.Size = new Size(177, 14);
      this.label32.TabIndex = 69;
      this.label32.Text = "To report demographic information?";
      this.radioButton4.AutoSize = true;
      this.radioButton4.Location = new Point(529, 10);
      this.radioButton4.Name = "radioButton4";
      this.radioButton4.Size = new Size(44, 18);
      this.radioButton4.TabIndex = 68;
      this.radioButton4.TabStop = true;
      this.radioButton4.Text = "Yes";
      this.radioButton4.UseVisualStyleBackColor = true;
      this.rdBtnReportInfoNo.AutoSize = true;
      this.rdBtnReportInfoNo.Location = new Point(245, 13);
      this.rdBtnReportInfoNo.Name = "rdBtnReportInfoNo";
      this.rdBtnReportInfoNo.Size = new Size(38, 18);
      this.rdBtnReportInfoNo.TabIndex = 58;
      this.rdBtnReportInfoNo.TabStop = true;
      this.rdBtnReportInfoNo.Text = "No";
      this.rdBtnReportInfoNo.UseVisualStyleBackColor = true;
      this.rdBtnReportInfoNo.CheckedChanged += new EventHandler(this.CheckChange);
      this.label27.AutoSize = true;
      this.label27.Location = new Point(12, 14);
      this.label27.Name = "label27";
      this.label27.Size = new Size(177, 14);
      this.label27.TabIndex = 63;
      this.label27.Text = "To report demographic information?";
      this.rdBtnReportInfoYes.AutoSize = true;
      this.rdBtnReportInfoYes.Location = new Point(195, 12);
      this.rdBtnReportInfoYes.Name = "rdBtnReportInfoYes";
      this.rdBtnReportInfoYes.Size = new Size(44, 18);
      this.rdBtnReportInfoYes.TabIndex = 58;
      this.rdBtnReportInfoYes.TabStop = true;
      this.rdBtnReportInfoYes.Text = "Yes";
      this.rdBtnReportInfoYes.UseVisualStyleBackColor = true;
      this.rdBtnReportInfoYes.CheckedChanged += new EventHandler(this.CheckChange);
      this.panel2.Controls.Add((Control) this.rdBtnPurchaseLoansNo);
      this.panel2.Controls.Add((Control) this.rdBtnPurchaseLoansYes);
      this.panel2.Controls.Add((Control) this.label9);
      this.panel2.Location = new Point(338, 382);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(335, 53);
      this.panel2.TabIndex = 93;
      this.rdBtnPurchaseLoansNo.AutoSize = true;
      this.rdBtnPurchaseLoansNo.Location = new Point(81, 30);
      this.rdBtnPurchaseLoansNo.Name = "rdBtnPurchaseLoansNo";
      this.rdBtnPurchaseLoansNo.Size = new Size(38, 18);
      this.rdBtnPurchaseLoansNo.TabIndex = 57;
      this.rdBtnPurchaseLoansNo.TabStop = true;
      this.rdBtnPurchaseLoansNo.Text = "No";
      this.rdBtnPurchaseLoansNo.UseVisualStyleBackColor = true;
      this.rdBtnPurchaseLoansNo.CheckedChanged += new EventHandler(this.CheckChange);
      this.rdBtnPurchaseLoansYes.AutoSize = true;
      this.rdBtnPurchaseLoansYes.Location = new Point(11, 30);
      this.rdBtnPurchaseLoansYes.Name = "rdBtnPurchaseLoansYes";
      this.rdBtnPurchaseLoansYes.Size = new Size(44, 18);
      this.rdBtnPurchaseLoansYes.TabIndex = 56;
      this.rdBtnPurchaseLoansYes.TabStop = true;
      this.rdBtnPurchaseLoansYes.Text = "Yes";
      this.rdBtnPurchaseLoansYes.UseVisualStyleBackColor = true;
      this.rdBtnPurchaseLoansYes.CheckedChanged += new EventHandler(this.CheckChange);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(8, 9);
      this.label9.Name = "label9";
      this.label9.Size = new Size(189, 14);
      this.label9.TabIndex = 55;
      this.label9.Text = "Does your institution purchase loans?";
      this.gradientPanel4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel4.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel4.Controls.Add((Control) this.label26);
      this.gradientPanel4.Location = new Point(1, 575);
      this.gradientPanel4.Name = "gradientPanel4";
      this.gradientPanel4.Size = new Size(2175, 28);
      this.gradientPanel4.TabIndex = 11;
      this.label26.AutoSize = true;
      this.label26.BackColor = Color.Transparent;
      this.label26.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label26.Location = new Point(8, 7);
      this.label26.Name = "label26";
      this.label26.Size = new Size(103, 14);
      this.label26.TabIndex = 4;
      this.label26.Text = "Purchased Loans";
      this.label4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label4.AutoEllipsis = true;
      this.label4.Location = new Point(11, 476);
      this.label4.Name = "label4";
      this.label4.Size = new Size(2389, 14);
      this.label4.TabIndex = 62;
      this.label4.Text = "Configure how loans with no channel selection and Correspondent loans are treated when generating HMDA reports.";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(11, 524);
      this.label3.Name = "label3";
      this.label3.Size = new Size(254, 14);
      this.label3.TabIndex = 61;
      this.label3.Text = "Loans marked as Correspondent will be treated as:";
      this.cmbLoansCorrespondent.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLoansCorrespondent.FormattingEnabled = true;
      this.cmbLoansCorrespondent.Items.AddRange(new object[2]
      {
        (object) "Retail",
        (object) "Purchased"
      });
      this.cmbLoansCorrespondent.Location = new Point(271, 521);
      this.cmbLoansCorrespondent.Name = "cmbLoansCorrespondent";
      this.cmbLoansCorrespondent.Size = new Size(121, 22);
      this.cmbLoansCorrespondent.TabIndex = 82;
      this.cmbLoansCorrespondent.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(11, 500);
      this.label2.Name = "label2";
      this.label2.Size = new Size(250, 14);
      this.label2.TabIndex = 59;
      this.label2.Text = "Loans with no channel selected will be treated as:";
      this.cmbLoansNoChannel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLoansNoChannel.FormattingEnabled = true;
      this.cmbLoansNoChannel.Items.AddRange(new object[4]
      {
        (object) "Banked - Retail",
        (object) "Banked - Wholesale",
        (object) "Brokered",
        (object) "Correspondent"
      });
      this.cmbLoansNoChannel.Location = new Point(271, 497);
      this.cmbLoansNoChannel.Name = "cmbLoansNoChannel";
      this.cmbLoansNoChannel.Size = new Size(121, 22);
      this.cmbLoansNoChannel.TabIndex = 80;
      this.cmbLoansNoChannel.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
      this.gradientPanel3.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel3.Controls.Add((Control) this.label25);
      this.gradientPanel3.Location = new Point(1, 435);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(685, 28);
      this.gradientPanel3.TabIndex = 10;
      this.label25.AutoSize = true;
      this.label25.BackColor = Color.Transparent;
      this.label25.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label25.Location = new Point(8, 7);
      this.label25.Name = "label25";
      this.label25.Size = new Size(91, 14);
      this.label25.TabIndex = 4;
      this.label25.Text = "Channel Option";
      this.cmbApplicationDate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbApplicationDate.FormattingEnabled = true;
      this.cmbApplicationDate.Location = new Point(105, 388);
      this.cmbApplicationDate.Name = "cmbApplicationDate";
      this.cmbApplicationDate.Size = new Size(220, 22);
      this.cmbApplicationDate.TabIndex = 70;
      this.cmbApplicationDate.SelectedIndexChanged += new EventHandler(this.cmbApplicationDate_SelectedIndexChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(11, 391);
      this.label8.Name = "label8";
      this.label8.Size = new Size(88, 14);
      this.label8.TabIndex = 53;
      this.label8.Text = "Application Date:";
      this.gradientPanel2.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel2.Controls.Add((Control) this.label7);
      this.gradientPanel2.Location = new Point(335, 354);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(905, 25);
      this.gradientPanel2.TabIndex = 10;
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(8, 7);
      this.label7.Name = "label7";
      this.label7.Size = new Size(92, 14);
      this.label7.TabIndex = 4;
      this.label7.Text = "Institution Type";
      this.cmbParentMailingState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbParentMailingState.FormattingEnabled = true;
      this.cmbParentMailingState.Location = new Point(593, 180);
      this.cmbParentMailingState.Name = "cmbParentMailingState";
      this.cmbParentMailingState.Size = new Size(80, 22);
      this.cmbParentMailingState.TabIndex = 56;
      this.cmbParentMailingState.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
      this.label19.AutoSize = true;
      this.label19.Location = new Point(543, 183);
      this.label19.Name = "label19";
      this.label19.Size = new Size(35, 14);
      this.label19.TabIndex = 51;
      this.label19.Text = "State:";
      this.txtParentMailingName.Location = new Point(419, 133);
      this.txtParentMailingName.Name = "txtParentMailingName";
      this.txtParentMailingName.Size = new Size(254, 20);
      this.txtParentMailingName.TabIndex = 50;
      this.txtParentMailingName.TextChanged += new EventHandler(this.TextChange);
      this.txtParentMailingAddress.Location = new Point(419, 157);
      this.txtParentMailingAddress.Name = "txtParentMailingAddress";
      this.txtParentMailingAddress.Size = new Size(254, 20);
      this.txtParentMailingAddress.TabIndex = 52;
      this.txtParentMailingAddress.TextChanged += new EventHandler(this.TextChange);
      this.txtParentMailingCity.Location = new Point(419, 178);
      this.txtParentMailingCity.Name = "txtParentMailingCity";
      this.txtParentMailingCity.Size = new Size(118, 20);
      this.txtParentMailingCity.TabIndex = 54;
      this.txtParentMailingCity.TextChanged += new EventHandler(this.TextChange);
      this.txtParentMailingZip.Location = new Point(419, 208);
      this.txtParentMailingZip.Name = "txtParentMailingZip";
      this.txtParentMailingZip.Size = new Size(254, 20);
      this.txtParentMailingZip.TabIndex = 58;
      this.txtParentMailingZip.TextChanged += new EventHandler(this.TextChange);
      this.txtParentMailingZip.KeyUp += new KeyEventHandler(this.txtZip_KeyUp);
      this.txtParentMailingZip.Leave += new EventHandler(this.txtParentMailingZip_Leave);
      this.label20.AutoSize = true;
      this.label20.Location = new Point(346, 160);
      this.label20.Name = "label20";
      this.label20.Size = new Size(52, 14);
      this.label20.TabIndex = 46;
      this.label20.Text = "Address:";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(346, 184);
      this.label21.Name = "label21";
      this.label21.Size = new Size(28, 14);
      this.label21.TabIndex = 45;
      this.label21.Text = "City:";
      this.label22.AutoSize = true;
      this.label22.Location = new Point(346, 208);
      this.label22.Name = "label22";
      this.label22.Size = new Size(53, 14);
      this.label22.TabIndex = 44;
      this.label22.Text = "Zip Code:";
      this.label23.AutoSize = true;
      this.label23.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label23.Location = new Point(346, 114);
      this.label23.Name = "label23";
      this.label23.Size = new Size(139, 14);
      this.label23.TabIndex = 43;
      this.label23.Text = "Parent Mailing Address:";
      this.label24.AutoSize = true;
      this.label24.Location = new Point(346, 136);
      this.label24.Name = "label24";
      this.label24.Size = new Size(37, 14);
      this.label24.TabIndex = 42;
      this.label24.Text = "Name:";
      this.cmbState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbState.FormattingEnabled = true;
      this.cmbState.Location = new Point(250, 227);
      this.cmbState.Name = "cmbState";
      this.cmbState.Size = new Size(80, 22);
      this.cmbState.TabIndex = 36;
      this.cmbState.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(203, 232);
      this.label18.Name = "label18";
      this.label18.Size = new Size(35, 14);
      this.label18.TabIndex = 40;
      this.label18.Text = "State:";
      this.txtPhone.Location = new Point(104, 277);
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(226, 20);
      this.txtPhone.TabIndex = 42;
      this.txtPhone.TextChanged += new EventHandler(this.TextChange);
      this.txtPhone.KeyUp += new KeyEventHandler(this.Phone_KeyUp);
      this.txtFax.Location = new Point(104, 301);
      this.txtFax.Name = "txtFax";
      this.txtFax.Size = new Size(225, 20);
      this.txtFax.TabIndex = 44;
      this.txtFax.TextChanged += new EventHandler(this.TextChange);
      this.txtFax.KeyUp += new KeyEventHandler(this.Phone_KeyUp);
      this.txtEmail.Location = new Point(105, 324);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(224, 20);
      this.txtEmail.TabIndex = 46;
      this.txtEmail.TextChanged += new EventHandler(this.TextChange);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(11, 282);
      this.label11.Name = "label11";
      this.label11.Size = new Size(40, 14);
      this.label11.TabIndex = 35;
      this.label11.Text = "Phone:";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(11, 306);
      this.label15.Name = "label15";
      this.label15.Size = new Size(28, 14);
      this.label15.TabIndex = 34;
      this.label15.Text = "Fax:";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(11, 330);
      this.label16.Name = "label16";
      this.label16.Size = new Size(34, 14);
      this.label16.TabIndex = 33;
      this.label16.Text = "Email:";
      this.txtCompanyName.Location = new Point(109, 133);
      this.txtCompanyName.Name = "txtCompanyName";
      this.txtCompanyName.Size = new Size(221, 20);
      this.txtCompanyName.TabIndex = 30;
      this.txtCompanyName.TextChanged += new EventHandler(this.TextChange);
      this.txtAddress.Location = new Point(105, 203);
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = new Size(225, 20);
      this.txtAddress.TabIndex = 32;
      this.txtAddress.TextChanged += new EventHandler(this.TextChange);
      this.txtCity.Location = new Point(104, 229);
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size(93, 20);
      this.txtCity.TabIndex = 34;
      this.txtCity.TextChanged += new EventHandler(this.TextChange);
      this.txtZip.Location = new Point(105, 253);
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new Size(224, 20);
      this.txtZip.TabIndex = 38;
      this.txtZip.TextChanged += new EventHandler(this.TextChange);
      this.txtZip.KeyUp += new KeyEventHandler(this.txtZip_KeyUp);
      this.txtZip.Leave += new EventHandler(this.txtZip_Leave);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(12, 208);
      this.label14.Name = "label14";
      this.label14.Size = new Size(52, 14);
      this.label14.TabIndex = 26;
      this.label14.Text = "Address:";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(12, 232);
      this.label13.Name = "label13";
      this.label13.Size = new Size(28, 14);
      this.label13.TabIndex = 25;
      this.label13.Text = "City:";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(12, 256);
      this.label12.Name = "label12";
      this.label12.Size = new Size(53, 14);
      this.label12.TabIndex = 24;
      this.label12.Text = "Zip Code:";
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(12, 114);
      this.label10.Name = "label10";
      this.label10.Size = new Size(189, 14);
      this.label10.TabIndex = 22;
      this.label10.Text = "Respondent Contact Information:";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(11, 136);
      this.label6.Name = "label6";
      this.label6.Size = new Size(85, 14);
      this.label6.TabIndex = 18;
      this.label6.Text = "Company Name:";
      this.cmbAgency.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbAgency.FormattingEnabled = true;
      this.cmbAgency.Location = new Point(603, 28);
      this.cmbAgency.Name = "cmbAgency";
      this.cmbAgency.Size = new Size(370, 22);
      this.cmbAgency.TabIndex = 13;
      this.cmbAgency.SelectedIndexChanged += new EventHandler(this.SelectedIndexChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(543, 28);
      this.label5.Name = "label5";
      this.label5.Size = new Size(48, 14);
      this.label5.TabIndex = 10;
      this.label5.Text = "Agency:";
      this.txtTaxId.Location = new Point(136, 55);
      this.txtTaxId.Name = "txtTaxId";
      this.txtTaxId.Size = new Size(370, 20);
      this.txtTaxId.TabIndex = 12;
      this.txtTaxId.TextChanged += new EventHandler(this.TextChange);
      this.lblTaxId.AutoSize = true;
      this.lblTaxId.Location = new Point(50, 55);
      this.lblTaxId.Name = "lblTaxId";
      this.lblTaxId.Size = new Size(39, 14);
      this.lblTaxId.TabIndex = 13;
      this.lblTaxId.Text = "Tax ID:";
      this.txtRespondentId.Location = new Point(136, 28);
      this.txtRespondentId.Name = "txtRespondentId";
      this.txtRespondentId.Size = new Size(370, 20);
      this.txtRespondentId.TabIndex = 11;
      this.txtRespondentId.TextChanged += new EventHandler(this.TextChange);
      this.lblRespondentId.AutoSize = true;
      this.lblRespondentId.Location = new Point(50, 28);
      this.lblRespondentId.Name = "lblRespondentId";
      this.lblRespondentId.Size = new Size(80, 14);
      this.lblRespondentId.TabIndex = 53;
      this.lblRespondentId.Text = "Respondent ID:";
      this.gradientPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Location = new Point(1, 354);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(2153, 28);
      this.gradientPanel1.TabIndex = 9;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(11, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(130, 14);
      this.label1.TabIndex = 4;
      this.label1.Text = "HMDA Application Date";
      this.panel5.Controls.Add((Control) this.radioButton7);
      this.panel5.Controls.Add((Control) this.label34);
      this.panel5.Controls.Add((Control) this.radioButton8);
      this.panel5.Controls.Add((Control) this.rdBtnRateSpreadNo);
      this.panel5.Controls.Add((Control) this.label35);
      this.panel5.Controls.Add((Control) this.rdBtnRateSpreadYes);
      this.panel5.Location = new Point(338, 603);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(335, 34);
      this.panel5.TabIndex = 95;
      this.radioButton7.AutoSize = true;
      this.radioButton7.Location = new Point(579, 11);
      this.radioButton7.Name = "radioButton7";
      this.radioButton7.Size = new Size(38, 18);
      this.radioButton7.TabIndex = 67;
      this.radioButton7.TabStop = true;
      this.radioButton7.Text = "No";
      this.radioButton7.UseVisualStyleBackColor = true;
      this.label34.AutoSize = true;
      this.label34.Location = new Point(346, 12);
      this.label34.Name = "label34";
      this.label34.Size = new Size(177, 14);
      this.label34.TabIndex = 69;
      this.label34.Text = "To report demographic information?";
      this.radioButton8.AutoSize = true;
      this.radioButton8.Location = new Point(529, 10);
      this.radioButton8.Name = "radioButton8";
      this.radioButton8.Size = new Size(44, 18);
      this.radioButton8.TabIndex = 68;
      this.radioButton8.TabStop = true;
      this.radioButton8.Text = "Yes";
      this.radioButton8.UseVisualStyleBackColor = true;
      this.rdBtnRateSpreadNo.AutoSize = true;
      this.rdBtnRateSpreadNo.Location = new Point(271, 14);
      this.rdBtnRateSpreadNo.Name = "rdBtnRateSpreadNo";
      this.rdBtnRateSpreadNo.Size = new Size(38, 18);
      this.rdBtnRateSpreadNo.TabIndex = 58;
      this.rdBtnRateSpreadNo.TabStop = true;
      this.rdBtnRateSpreadNo.Text = "No";
      this.rdBtnRateSpreadNo.UseVisualStyleBackColor = true;
      this.rdBtnRateSpreadNo.CheckedChanged += new EventHandler(this.CheckChange);
      this.label35.AutoSize = true;
      this.label35.Location = new Point(12, 14);
      this.label35.Name = "label35";
      this.label35.Size = new Size(203, 14);
      this.label35.TabIndex = 63;
      this.label35.Text = "Display Rate Spread to 3 decimal places ";
      this.rdBtnRateSpreadYes.AutoSize = true;
      this.rdBtnRateSpreadYes.Location = new Point(221, 13);
      this.rdBtnRateSpreadYes.Name = "rdBtnRateSpreadYes";
      this.rdBtnRateSpreadYes.Size = new Size(44, 18);
      this.rdBtnRateSpreadYes.TabIndex = 58;
      this.rdBtnRateSpreadYes.TabStop = true;
      this.rdBtnRateSpreadYes.Text = "Yes";
      this.rdBtnRateSpreadYes.UseVisualStyleBackColor = true;
      this.rdBtnRateSpreadYes.CheckedChanged += new EventHandler(this.CheckChange);
      this.groupContainer2.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.gvHMDAProfile);
      this.groupContainer2.Controls.Add((Control) this.groupContainer3);
      this.groupContainer2.Controls.Add((Control) this.stdButtonNew);
      this.groupContainer2.Controls.Add((Control) this.stdButtonDelete);
      this.groupContainer2.Controls.Add((Control) this.stdButtonEdit);
      this.groupContainer2.Dock = DockStyle.Top;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(1230, 222);
      this.groupContainer2.TabIndex = 0;
      this.groupContainer2.Text = "List of HMDA Profiles";
      this.gvHMDAProfile.AllowMultiselect = false;
      this.gvHMDAProfile.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Tag = (object) "Name";
      gvColumn1.Text = "Profile Name";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Tag = (object) "Status";
      gvColumn2.Text = "Respondent ID";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Tag = (object) "LineNumGroup";
      gvColumn3.Text = "LEI";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Tag = (object) "LastModBy";
      gvColumn4.Text = "Company Name";
      gvColumn4.Width = 250;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SortMethod = GVSortMethod.DateTime;
      gvColumn5.Tag = (object) "LastModDateTime";
      gvColumn5.Text = "Agency";
      gvColumn5.Width = 250;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Last Modified By";
      gvColumn6.Width = 150;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.SortMethod = GVSortMethod.DateTime;
      gvColumn7.Text = "Last Modified Date & Time";
      gvColumn7.Width = 150;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column77";
      gvColumn8.Tag = (object) "LineNumGroup";
      gvColumn8.Text = "Non Universal Loan Identifier";
      gvColumn8.Width = 150;
      this.gvHMDAProfile.Columns.AddRange(new GVColumn[8]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.gvHMDAProfile.Dock = DockStyle.Fill;
      this.gvHMDAProfile.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvHMDAProfile.Location = new Point(1, 26);
      this.gvHMDAProfile.Name = "gvHMDAProfile";
      this.gvHMDAProfile.Size = new Size(1228, 132);
      this.gvHMDAProfile.TabIndex = 0;
      this.gvHMDAProfile.SelectedIndexChanged += new EventHandler(this.HMDAProfileList_SelectedIndexChanged);
      this.gvHMDAProfile.DoubleClick += new EventHandler(this.gvHMDAProfile_DoubleClick);
      this.groupContainer3.Controls.Add((Control) this.label42);
      this.groupContainer3.Controls.Add((Control) this.txtProfileName);
      this.groupContainer3.Controls.Add((Control) this.label41);
      this.groupContainer3.Controls.Add((Control) this.stdButtonReset);
      this.groupContainer3.Controls.Add((Control) this.stdButtonSave);
      this.groupContainer3.Dock = DockStyle.Bottom;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(1, 158);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(1228, 64);
      this.groupContainer3.TabIndex = 1;
      this.groupContainer3.Text = "HMDA Profile Details";
      this.label42.AutoSize = true;
      this.label42.ForeColor = Color.Red;
      this.label42.Location = new Point(115, 29);
      this.label42.Name = "label42";
      this.label42.Size = new Size(11, 14);
      this.label42.TabIndex = 2;
      this.label42.Text = "*";
      this.txtProfileName.Location = new Point(136, 33);
      this.txtProfileName.Name = "txtProfileName";
      this.txtProfileName.Size = new Size(365, 20);
      this.txtProfileName.TabIndex = 1;
      this.txtProfileName.TextChanged += new EventHandler(this.TextChange);
      this.label41.AutoSize = true;
      this.label41.Location = new Point(16, 36);
      this.label41.Name = "label41";
      this.label41.Size = new Size(102, 14);
      this.label41.TabIndex = 1;
      this.label41.Text = "HMDA Profile Name:";
      this.stdButtonReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonReset.BackColor = Color.Transparent;
      this.stdButtonReset.Location = new Point(1200, 4);
      this.stdButtonReset.MouseDownImage = (Image) null;
      this.stdButtonReset.Name = "stdButtonReset";
      this.stdButtonReset.Size = new Size(16, 16);
      this.stdButtonReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdButtonReset.TabIndex = 111;
      this.stdButtonReset.TabStop = false;
      this.stdButtonReset.Click += new EventHandler(this.stdButtonReset_Click);
      this.stdButtonSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonSave.BackColor = Color.Transparent;
      this.stdButtonSave.Location = new Point(1178, 3);
      this.stdButtonSave.MouseDownImage = (Image) null;
      this.stdButtonSave.Name = "stdButtonSave";
      this.stdButtonSave.Size = new Size(16, 16);
      this.stdButtonSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdButtonSave.TabIndex = 110;
      this.stdButtonSave.TabStop = false;
      this.stdButtonSave.Click += new EventHandler(this.stdButtonSave_Click);
      this.stdButtonNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonNew.BackColor = Color.Transparent;
      this.stdButtonNew.Location = new Point(1158, 4);
      this.stdButtonNew.MouseDownImage = (Image) null;
      this.stdButtonNew.Name = "stdButtonNew";
      this.stdButtonNew.Size = new Size(16, 16);
      this.stdButtonNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdButtonNew.TabIndex = 67;
      this.stdButtonNew.TabStop = false;
      this.stdButtonNew.Click += new EventHandler(this.stdButtonNew_Click);
      this.stdButtonDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonDelete.BackColor = Color.Transparent;
      this.stdButtonDelete.Location = new Point(1202, 4);
      this.stdButtonDelete.MouseDownImage = (Image) null;
      this.stdButtonDelete.Name = "stdButtonDelete";
      this.stdButtonDelete.Size = new Size(16, 16);
      this.stdButtonDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdButtonDelete.TabIndex = 111;
      this.stdButtonDelete.TabStop = false;
      this.stdButtonDelete.Click += new EventHandler(this.stdButtonDelete_Click);
      this.stdButtonEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonEdit.BackColor = Color.Transparent;
      this.stdButtonEdit.Location = new Point(1180, 3);
      this.stdButtonEdit.MouseDownImage = (Image) null;
      this.stdButtonEdit.Name = "stdButtonEdit";
      this.stdButtonEdit.Size = new Size(16, 16);
      this.stdButtonEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdButtonEdit.TabIndex = 110;
      this.stdButtonEdit.TabStop = false;
      this.stdButtonEdit.Click += new EventHandler(this.stdButtonEdit_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.groupContainer2);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (HMDASettingsForm);
      this.Size = new Size(1230, 800);
      this.groupContainer1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel8.ResumeLayout(false);
      this.panel8.PerformLayout();
      this.panel7.ResumeLayout(false);
      this.panel7.PerformLayout();
      this.gradientPanel6.ResumeLayout(false);
      this.gradientPanel6.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.gradientPanel5.ResumeLayout(false);
      this.gradientPanel5.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.gradientPanel4.ResumeLayout(false);
      this.gradientPanel4.PerformLayout();
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      ((ISupportInitialize) this.stdButtonReset).EndInit();
      ((ISupportInitialize) this.stdButtonSave).EndInit();
      ((ISupportInitialize) this.stdButtonNew).EndInit();
      ((ISupportInitialize) this.stdButtonDelete).EndInit();
      ((ISupportInitialize) this.stdButtonEdit).EndInit();
      this.ResumeLayout(false);
    }

    public enum ActionPerformed
    {
      Create,
      Edit,
      Delete,
    }
  }
}
