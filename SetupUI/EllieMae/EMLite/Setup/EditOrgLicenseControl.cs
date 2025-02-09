// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditOrgLicenseControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditOrgLicenseControl : UserControl
  {
    private Sessions.Session session;
    private bool forCompanyInfo;
    private string[] statesAbbrevation;
    private Hashtable stateTable;
    private BranchExtLicensing branchExtLicensing;
    private BranchExtLicensing parentLicensing;
    private IOrganizationManager rOrg;
    private int parentID;
    private int currentOID;
    private const string ALLSTATES = "All States";
    private static string[] lenderTypeList = new string[9]
    {
      "",
      "Federal Credit Union (FCU)",
      "Federal Savings Bank (FSB)",
      "National Bank (FNB)",
      "State Bank (STB)",
      "State Credit Union (STCU)",
      "State Licensed Lender - Non Depository Institution (STLIC)",
      "State Savings Association (STSA)",
      "State Savings Bank (STSB)"
    };
    private DateTime minValue = DateTime.Parse("1/1/1900");
    private DateTime maxValue = DateTime.Parse("01/01/2079");
    public bool saveOnLoad;
    private string lastSelectedComboItemText = string.Empty;
    private SessionObjects obj;
    private bool validated;
    private IContainer components;
    private Label label3;
    private ComboBox cboHomeState;
    private Label label2;
    private GridView gridViewStates;
    private ComboBox cboLenderType;
    private Label label1;
    private GroupContainer grpKansas;
    private GroupContainer grpMaryland;
    private GroupContainer grpLicenseList;
    private ComboBox cboTypeState;
    private RadioButton rdoKansasYes;
    private RadioButton rdoKansasNo;
    private RadioButton rdoMarylandYes;
    private RadioButton rdoMarylandNo;
    private CheckBox chkUseParentInfo;
    private IconButton iconBtnHelpKansas;
    private IconButton iconBtnHelpMaryland;
    private Panel panelTop2;
    private GroupContainer groupContainer5;
    private ComboBox cboExemptCreditor;
    private Label label6;
    private ComboBox cboSmallCreditor;
    private Label label5;
    private GroupContainer grpAll;
    private Panel panelHeader;
    private Label label33;
    private StandardIconButton btnReset;
    private StandardIconButton btnSave;
    private GroupContainer grpLicense;
    private Panel panelLicenseList;
    private Panel panelMarylandKansas;
    private GroupContainer grpLicensingIssues;
    private TextBox txtMsgUploadNonApproved;
    private Label label4;
    private Label label7;
    private Panel panel1;
    private Panel panel2;
    private Label label8;
    private StandardIconButton btnMoveLicenseUp;
    private StandardIconButton btnMoveLicenseDown;
    private ComboBox cmbAllowLoansWithLicenseIssues;
    private Label label9;
    private Panel panel3;
    private CheckBox checkBoxOptOut;
    private RadioButton rdoMarylandYes1;
    private RadioButton rdoMarylandYes2;

    public event EventHandler SaveButton_Clicked;

    public EditOrgLicenseControl(
      bool forCompanyInfo,
      int parentID,
      int currentOID,
      SessionObjects obj)
    {
      this.forCompanyInfo = forCompanyInfo;
      this.obj = obj;
      this.rOrg = this.obj.OrganizationManager;
      this.parentID = parentID;
      this.currentOID = currentOID;
      this.InitializeComponent();
    }

    public EditOrgLicenseControl(
      bool forCompanyInfo,
      int parentID,
      int currentOID,
      Sessions.Session session)
    {
      this.forCompanyInfo = forCompanyInfo;
      this.session = session;
      this.rOrg = this.session.OrganizationManager;
      this.parentID = parentID;
      this.currentOID = currentOID;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.grpLicense.DockPadding.Right = 2;
      this.grpLicensingIssues.DockPadding.Left = 2;
      this.grpMaryland.DockPadding.Right = 2;
      this.grpKansas.DockPadding.Left = 2;
      if (this.parentID == 0)
        this.chkUseParentInfo.Enabled = this.chkUseParentInfo.Checked = false;
      this.statesAbbrevation = Utils.GetStates();
      List<string> stringList = new List<string>((IEnumerable<string>) this.statesAbbrevation);
      stringList.Remove("PR");
      stringList.Remove("GU");
      stringList.Remove("VI");
      stringList.Sort();
      this.statesAbbrevation = stringList.ToArray();
      this.cboTypeState.Items.Clear();
      this.cboTypeState.Sorted = true;
      this.cboTypeState.Items.AddRange((object[]) this.statesAbbrevation);
      this.cboTypeState.Sorted = false;
      this.cboTypeState.Items.Insert(0, (object) "All States");
      this.cboTypeState.Text = "All States";
      this.cboHomeState.Items.AddRange((object[]) this.statesAbbrevation);
      this.cboHomeState.Sorted = true;
      this.cboLenderType.Items.AddRange((object[]) EditOrgLicenseControl.lenderTypeList);
      this.Resize += new EventHandler(this.addEditOrgLicenseControl_Resize);
      this.cboTypeState.SelectedIndexChanged += new EventHandler(this.cboTypeState_SelectedIndexChanged);
      this.chkUseParentInfo.Click += new EventHandler(this.chkUseParentInfo_Click);
      this.SetButtonStatus(false);
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(currentOID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    private void addEditOrgLicenseControl_Resize(object sender, EventArgs e)
    {
      this.grpMaryland.Width = this.grpLicenseList.Width / 2;
      this.grpKansas.Width = this.grpMaryland.Width - 5;
      this.grpKansas.Left = this.grpMaryland.Left + this.grpMaryland.Width + 5;
      this.grpLicense.Width = this.grpLicenseList.Width / 2;
      this.grpLicensingIssues.Width = this.grpLicense.Width - 5;
      this.grpLicensingIssues.Left = this.grpLicense.Left + this.grpLicense.Width + 5;
    }

    public void RefreshData()
    {
      this.branchExtLicensing = this.session.ConfigurationManager.GetExtLicenseDetails(this.currentOID);
      if (this.branchExtLicensing != null)
        this.chkUseParentInfo.Checked = this.branchExtLicensing.UseParentInfo;
      if (this.parentID != 0 && this.branchExtLicensing.UseParentInfo)
      {
        this.parentLicensing = this.session.ConfigurationManager.GetExtLicenseDetails(this.parentID);
        this.branchExtLicensing = this.parentLicensing;
        this.chkUseParentInfo.Enabled = this.chkUseParentInfo.Checked = true;
        this.saveOnLoad = true;
      }
      this.PopulateLicenseInfo(this.branchExtLicensing);
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(this.currentOID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
      this.lastSelectedComboItemText = this.cmbAllowLoansWithLicenseIssues.SelectedItem != null ? this.cmbAllowLoansWithLicenseIssues.SelectedItem.ToString() : string.Empty;
    }

    public void PopulateLicenseInfo(BranchExtLicensing branchExtLicensing)
    {
      if (branchExtLicensing != null)
      {
        this.cboLenderType.Text = branchExtLicensing.LenderType;
        if (this.disableOptOut())
        {
          this.checkBoxOptOut.CheckState = CheckState.Unchecked;
          this.checkBoxOptOut.Enabled = false;
        }
        else
        {
          this.checkBoxOptOut.CheckState = branchExtLicensing.OptOut == "Y" ? CheckState.Checked : CheckState.Unchecked;
          this.checkBoxOptOut.Enabled = true;
        }
        if (this.cboLenderType.Text == "State Licensed Lender - Non Depository Institution (STLIC)" || this.cboLenderType.Text == string.Empty)
        {
          this.cboHomeState.SelectedIndex = -1;
          this.cboHomeState.Enabled = false;
        }
        else if (this.checkBoxOptOut.CheckState == CheckState.Checked)
        {
          this.cboHomeState.Text = branchExtLicensing.HomeState;
          this.cboHomeState.Enabled = false;
        }
        else
        {
          this.cboHomeState.Text = branchExtLicensing.HomeState;
          if (!this.chkUseParentInfo.Checked)
            this.cboHomeState.Enabled = true;
        }
      }
      this.stateTable = new Hashtable();
      if (branchExtLicensing != null)
      {
        for (int index = 0; index < branchExtLicensing.StateLicenseExtTypes.Count; ++index)
        {
          if (!(branchExtLicensing.StateLicenseExtTypes[index].LicenseType == string.Empty) && !(branchExtLicensing.StateLicenseExtTypes[index].StateAbbrevation == "GU") && !(branchExtLicensing.StateLicenseExtTypes[index].StateAbbrevation == "PR") && !(branchExtLicensing.StateLicenseExtTypes[index].StateAbbrevation == "VI") && !this.stateTable.ContainsKey((object) this.setStateKey(branchExtLicensing.StateLicenseExtTypes[index].StateAbbrevation, branchExtLicensing.StateLicenseExtTypes[index].LicenseType)))
            this.stateTable.Add((object) this.setStateKey(branchExtLicensing.StateLicenseExtTypes[index].StateAbbrevation, branchExtLicensing.StateLicenseExtTypes[index].LicenseType), (object) branchExtLicensing.StateLicenseExtTypes[index]);
        }
        this.rdoMarylandYes.Checked = branchExtLicensing.StatutoryElectionInMaryland;
        this.rdoMarylandYes1.Checked = branchExtLicensing.StatutoryElectionInMaryland2 == "10";
        this.rdoMarylandYes2.Checked = branchExtLicensing.StatutoryElectionInMaryland2 == "01";
        this.rdoMarylandNo.Checked = !this.rdoMarylandYes.Checked && !this.rdoMarylandYes1.Checked && !this.rdoMarylandYes2.Checked;
        this.rdoKansasNo.Checked = !(this.rdoKansasYes.Checked = branchExtLicensing.StatutoryElectionInKansas);
        this.cboSmallCreditor.SelectedIndexChanged -= new EventHandler(this.fieldChanged);
        this.cboSmallCreditor.Text = branchExtLicensing.ATRSmallCreditorToString();
        this.cboSmallCreditor.SelectedIndexChanged += new EventHandler(this.fieldChanged);
        this.cboExemptCreditor.SelectedIndexChanged -= new EventHandler(this.fieldChanged);
        this.cboExemptCreditor.Text = branchExtLicensing.ATRExemptCreditorToString();
        this.cboExemptCreditor.SelectedIndexChanged += new EventHandler(this.fieldChanged);
        this.cmbAllowLoansWithLicenseIssues.SelectedIndex = branchExtLicensing.AllowLoansWithIssues;
        this.txtMsgUploadNonApproved.Text = branchExtLicensing.MsgUploadNonApprovedLoans != "" ? branchExtLicensing.MsgUploadNonApprovedLoans : "Our records indicate there are licensing issues that exist for this loan. Please contact your Account Executive for assistance.";
      }
      else
      {
        this.txtMsgUploadNonApproved.Text = "Our records indicate there are licensing issues that exist for this loan. Please contact your Account Executive for assistance.";
        if (this.disableOptOut())
        {
          this.checkBoxOptOut.CheckState = CheckState.Unchecked;
          this.checkBoxOptOut.Enabled = false;
        }
        else
          this.checkBoxOptOut.Enabled = true;
        if (this.cboLenderType.Text == "State Licensed Lender - Non Depository Institution (STLIC)" || this.cboLenderType.Text == string.Empty)
        {
          this.cboHomeState.SelectedIndex = -1;
          this.cboHomeState.Enabled = false;
        }
        else if (this.checkBoxOptOut.CheckState == CheckState.Checked)
          this.cboHomeState.Enabled = false;
        else
          this.cboHomeState.Enabled = true;
      }
      this.SetButtonStatus(false);
      this.cboTypeState_SelectedIndexChanged((object) null, (EventArgs) null);
      this.SetReadOnly(this.chkUseParentInfo.Checked);
      this.SetButtonStatus(false);
    }

    public void SetUseParentInfo(bool useParentInfo)
    {
      this.chkUseParentInfo.Checked = useParentInfo;
      this.SetReadOnly(useParentInfo);
    }

    public bool IsUseParentInfoChecked() => this.chkUseParentInfo.Checked;

    public void SetReadOnly(bool readOnly)
    {
      this.rdoMarylandYes.Enabled = this.rdoMarylandNo.Enabled = this.rdoMarylandYes1.Enabled = this.rdoMarylandYes2.Enabled = this.rdoKansasYes.Enabled = this.rdoKansasNo.Enabled = this.cboSmallCreditor.Enabled = this.cboExemptCreditor.Enabled = !readOnly;
      this.cboLenderType.Enabled = this.cmbAllowLoansWithLicenseIssues.Enabled = this.txtMsgUploadNonApproved.Enabled = this.checkBoxOptOut.Enabled = !readOnly;
      if (!this.cboLenderType.Enabled)
      {
        this.cboHomeState.Enabled = this.cboLenderType.Enabled;
      }
      else
      {
        if (this.disableOptOut())
        {
          this.checkBoxOptOut.CheckState = CheckState.Unchecked;
          this.checkBoxOptOut.Enabled = false;
        }
        else
          this.checkBoxOptOut.Enabled = true;
        if (this.cboLenderType.Text == "State Licensed Lender - Non Depository Institution (STLIC)" || this.cboLenderType.Text == string.Empty)
        {
          this.cboHomeState.SelectedIndex = -1;
          this.cboHomeState.Enabled = false;
        }
        else if (this.checkBoxOptOut.CheckState == CheckState.Checked)
          this.cboHomeState.Enabled = false;
        else
          this.cboHomeState.Enabled = true;
      }
      for (int nItemIndex = 0; nItemIndex < this.gridViewStates.Items.Count; ++nItemIndex)
      {
        this.gridViewStates.Items[nItemIndex].SubItems[0].CheckBoxEnabled = !readOnly;
        this.gridViewStates.Items[nItemIndex].SubItems[1].CheckBoxEnabled = !readOnly;
      }
      this.btnMoveLicenseDown.Enabled = this.btnMoveLicenseUp.Enabled = !readOnly;
      if (!readOnly)
      {
        if (this.gridViewStates.SelectedItems.Count != 0)
        {
          this.btnMoveLicenseUp.Enabled = this.gridViewStates.SelectedItems[0].Index != 0;
          this.btnMoveLicenseDown.Enabled = this.gridViewStates.SelectedItems[0].Index != this.gridViewStates.Items.Count - 1;
        }
        else
          this.btnMoveLicenseUp.Enabled = this.btnMoveLicenseDown.Enabled = false;
      }
      if (!(this.cboTypeState.Text == "All States"))
        return;
      this.btnMoveLicenseDown.Enabled = this.btnMoveLicenseUp.Enabled = false;
    }

    private void cboTypeState_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.IsDirty)
      {
        DialogResult dialogResult = Utils.Dialog((IWin32Window) this, "Do you want to save the unsaved license information for this state?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
        if (dialogResult == DialogResult.Yes)
          this.btnSave_Click((object) null, (EventArgs) null);
        if (dialogResult == DialogResult.No)
        {
          this.RefreshData();
          return;
        }
        if (dialogResult == DialogResult.Cancel)
          return;
      }
      this.gridViewStates.BeginUpdate();
      this.gridViewStates.Items.Clear();
      this.DisableArrows(this.cboTypeState.Text == "All States");
      for (int index1 = 0; index1 < this.statesAbbrevation.Length; ++index1)
      {
        if (string.Compare(this.statesAbbrevation[index1], "GU", true) != 0 && string.Compare(this.statesAbbrevation[index1], "VI", true) != 0 && string.Compare(this.statesAbbrevation[index1], "PR", true) != 0 && (!(this.cboTypeState.Text != "All States") || string.Compare(this.cboTypeState.Text, this.statesAbbrevation[index1], true) == 0))
        {
          string[] licenseTypes = MaventLicenseTypesUtils.GetLicenseTypes(this.statesAbbrevation[index1]);
          Dictionary<GVItem, int> dictionary = new Dictionary<GVItem, int>();
          for (int index2 = 0; index2 < licenseTypes.Length; ++index2)
          {
            if (string.Compare(licenseTypes[index2], "No License Required", true) != 0)
            {
              GVItem key = new GVItem()
              {
                SubItems = {
                  [0] = {
                    Value = (object) null
                  },
                  [1] = {
                    Value = (object) null
                  }
                }
              };
              key.SubItems.Add((object) this.statesAbbrevation[index1]);
              key.SubItems.Add((object) licenseTypes[index2]);
              key.SubItems.Add((object) "");
              key.SubItems.Add((object) "");
              key.SubItems.Add((object) "");
              key.SubItems.Add((object) "");
              key.SubItems.Add((object) "");
              key.SubItems.Add((object) "");
              key.SubItems.Add((object) "");
              key.SubItems.Add((object) "");
              key.SubItems.Add((object) "");
              if (this.stateTable.ContainsKey((object) this.setStateKey(this.statesAbbrevation[index1], licenseTypes[index2])))
              {
                StateLicenseExtType stateLicenseExtType = (StateLicenseExtType) this.stateTable[(object) this.setStateKey(this.statesAbbrevation[index1], licenseTypes[index2])];
                key.SubItems[0].Checked = stateLicenseExtType.Approved;
                if (string.Compare(licenseTypes[index2], "No License Required", true) != 0)
                {
                  key.SubItems[1].Checked = stateLicenseExtType.Exempt;
                  key.SubItems[1].CheckBoxVisible = stateLicenseExtType.Approved;
                  key.SubItems[4].Text = stateLicenseExtType.LicenseNo;
                  if (stateLicenseExtType.IssueDate != DateTime.MinValue)
                    key.SubItems[5].Value = (object) stateLicenseExtType.IssueDate.ToString("d");
                  if (stateLicenseExtType.StartDate != DateTime.MinValue)
                    key.SubItems[6].Value = (object) stateLicenseExtType.StartDate.ToString("d");
                  if (stateLicenseExtType.EndDate != DateTime.MinValue)
                    key.SubItems[7].Value = (object) stateLicenseExtType.EndDate.ToString("d");
                  key.SubItems[8].Value = (object) stateLicenseExtType.LicenseStatus;
                  if (stateLicenseExtType.StatusDate != DateTime.MinValue)
                    key.SubItems[9].Value = (object) stateLicenseExtType.StatusDate.ToString("d");
                  if (stateLicenseExtType.LastChecked != DateTime.MinValue)
                    key.SubItems[10].Value = (object) stateLicenseExtType.LastChecked.ToString("d");
                  if (stateLicenseExtType.SortIndex > 0)
                    dictionary.Add(key, stateLicenseExtType.SortIndex);
                  key.Tag = (object) false;
                }
                else
                  key.SubItems[1].CheckBoxVisible = false;
              }
              else
                key.SubItems[1].CheckBoxVisible = false;
              key.SubItems[0].CheckBoxEnabled = key.SubItems[1].CheckBoxEnabled = !this.chkUseParentInfo.Checked;
              this.gridViewStates.Items.Add(key);
            }
          }
          if (dictionary.Count > 0)
          {
            SortedDictionary<int, GVItem> source = new SortedDictionary<int, GVItem>();
            int num = 0;
            foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewStates.Items)
            {
              if (gvItem.SubItems[2].Text == this.statesAbbrevation[index1])
              {
                num = gvItem.Index;
                break;
              }
            }
            foreach (KeyValuePair<GVItem, int> keyValuePair in dictionary)
            {
              int key = num + keyValuePair.Value - 1;
              source.Add(key, keyValuePair.Key);
            }
            foreach (KeyValuePair<int, GVItem> keyValuePair in source.Reverse<KeyValuePair<int, GVItem>>())
              this.gridViewStates.Items.RemoveAt(keyValuePair.Key);
            foreach (KeyValuePair<int, GVItem> keyValuePair in source)
              this.gridViewStates.Items.Insert(keyValuePair.Key, keyValuePair.Value);
          }
        }
      }
      if (this.gridViewStates.Items.Count <= 1)
        this.DisableArrows(true);
      else
        this.gridViewStates.Items[0].Selected = true;
      this.gridViewStates.EndUpdate();
    }

    public BranchExtLicensing CurrentBranchLicensing
    {
      get
      {
        BranchExtLicensing currentBranchLicensing = new BranchExtLicensing(this.chkUseParentInfo.Checked, this.cmbAllowLoansWithLicenseIssues.SelectedIndex, this.txtMsgUploadNonApproved.Text, this.chkUseParentInfo.Checked ? (string) null : this.cboLenderType.Text, this.chkUseParentInfo.Checked ? "" : this.cboHomeState.Text, this.checkBoxOptOut.Checked ? "Y" : "N", this.rdoMarylandYes.Checked && !this.chkUseParentInfo.Checked, this.chkUseParentInfo.Checked ? "00" : this.getMarylandStatutoryElection(), this.rdoKansasYes.Checked && !this.chkUseParentInfo.Checked, (List<StateLicenseExtType>) null, false, BranchLicensing.ATRSmallCreditorToEnum(this.cboSmallCreditor.SelectedIndex), BranchLicensing.ATRExemptCreditorToEnum(this.cboExemptCreditor.SelectedIndex));
        List<StateLicenseExtType> stateLicenseExtTypeList = new List<StateLicenseExtType>();
        if (!this.chkUseParentInfo.Checked)
        {
          foreach (DictionaryEntry dictionaryEntry in this.stateTable)
          {
            StateLicenseExtType stateLicenseExtType = (StateLicenseExtType) dictionaryEntry.Value;
            if (stateLicenseExtType.Selected || stateLicenseExtType.Exempt)
              currentBranchLicensing.AddStateLicenseType((StateLicenseType) dictionaryEntry.Value);
          }
        }
        return currentBranchLicensing;
      }
    }

    private string getMarylandStatutoryElection()
    {
      if (this.rdoMarylandYes1.Checked)
        return "10";
      return this.rdoMarylandYes2.Checked ? "01" : "00";
    }

    private void cboLenderType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.disableOptOut())
      {
        this.checkBoxOptOut.CheckState = CheckState.Unchecked;
        this.checkBoxOptOut.Enabled = false;
      }
      else
        this.checkBoxOptOut.Enabled = true;
      if (this.cboLenderType.Text == "State Licensed Lender - Non Depository Institution (STLIC)" || this.cboLenderType.Text == string.Empty)
      {
        this.cboHomeState.SelectedIndex = -1;
        this.cboHomeState.Enabled = false;
      }
      else if (this.checkBoxOptOut.CheckState == CheckState.Checked)
      {
        this.cboHomeState.Enabled = false;
      }
      else
      {
        if (this.cboHomeState.SelectedIndex == -1)
          this.cboHomeState.SelectedIndex = 0;
        if (!this.chkUseParentInfo.Checked)
          this.cboHomeState.Enabled = true;
      }
      this.SetButtonStatus(true);
    }

    private void chkUseParentInfo_Click(object sender, EventArgs e)
    {
      if (this.chkUseParentInfo.Checked)
      {
        this.parentLicensing = this.session.ConfigurationManager.GetExtLicenseDetails(this.parentID);
        this.PopulateLicenseInfo(this.parentLicensing);
      }
      this.SetReadOnly(this.chkUseParentInfo.Checked);
      this.SetButtonStatus(true);
    }

    public BranchExtLicensing GetModifiedData()
    {
      BranchExtLicensing modifiedData = this.branchExtLicensing ?? new BranchExtLicensing(false, 0, "", "", "", "", false, "00", false, (List<StateLicenseExtType>) null, false, BranchLicensing.ATRSmallCreditors.None, BranchLicensing.ATRExemptCreditors.None);
      modifiedData.AllowLoansWithIssues = this.cmbAllowLoansWithLicenseIssues.SelectedIndex;
      modifiedData.MsgUploadNonApprovedLoans = this.txtMsgUploadNonApproved.Text;
      modifiedData.LenderType = this.cboLenderType.SelectedIndex != -1 ? this.cboLenderType.SelectedItem.ToString() : "";
      modifiedData.HomeState = this.cboHomeState.SelectedIndex != -1 ? this.cboHomeState.SelectedItem.ToString() : "";
      modifiedData.OptOut = this.checkBoxOptOut.CheckState == CheckState.Checked ? "Y" : "N";
      modifiedData.StatutoryElectionInMaryland = !this.rdoMarylandNo.Checked;
      if (this.rdoMarylandYes1.Checked)
        modifiedData.StatutoryElectionInMaryland2 = "10";
      else if (this.rdoMarylandYes2.Checked)
        modifiedData.StatutoryElectionInMaryland2 = "01";
      else
        modifiedData.StatutoryElectionInMaryland2 = "00";
      modifiedData.StatutoryElectionInKansas = this.rdoKansasYes.Checked;
      modifiedData.ATRExemptCreditor = this.cboExemptCreditor.SelectedIndex == -1 ? BranchLicensing.ATRExemptCreditors.None : (BranchLicensing.ATRExemptCreditors) this.cboExemptCreditor.SelectedIndex;
      modifiedData.ATRSmallCreditor = this.cboSmallCreditor.SelectedIndex == -1 ? BranchLicensing.ATRSmallCreditors.None : (BranchLicensing.ATRSmallCreditors) this.cboSmallCreditor.SelectedIndex;
      modifiedData.UseParentInfo = this.chkUseParentInfo.Checked;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewStates.Items)
      {
        if (gvItem.Tag != null)
        {
          StateLicenseExtType stateLicenseExtType = new StateLicenseExtType(gvItem.SubItems[2].Text, gvItem.SubItems[3].Text, gvItem.SubItems[4].Text, gvItem.SubItems[5].Text != "" ? Convert.ToDateTime(gvItem.SubItems[5].Text) : DateTime.MinValue, gvItem.SubItems[6].Text != "" ? Convert.ToDateTime(gvItem.SubItems[6].Text) : DateTime.MinValue, gvItem.SubItems[7].Text != "" ? Convert.ToDateTime(gvItem.SubItems[7].Text) : DateTime.MinValue, gvItem.SubItems[8].Text, gvItem.SubItems[9].Text != "" ? Convert.ToDateTime(gvItem.SubItems[9].Text) : DateTime.MinValue, gvItem.SubItems[0].Checked, gvItem.SubItems[1].Checked, gvItem.SubItems[10].Text != "" ? Convert.ToDateTime(gvItem.SubItems[10].Text) : DateTime.MinValue, this.getSortIndex(gvItem.SubItems[2].Text, gvItem.SubItems[3].Text));
          if (modifiedData.IsExists(stateLicenseExtType))
            modifiedData.RemoveStateLicenseExtType(stateLicenseExtType);
          modifiedData.AddStateLicenseExtType(stateLicenseExtType);
        }
      }
      if (modifiedData.UseParentInfo)
      {
        List<StateLicenseExtType> stateLicenseExtTypeList = new List<StateLicenseExtType>();
        foreach (StateLicenseExtType stateLicenseExtType in modifiedData.StateLicenseExtTypes)
        {
          if (!this.parentLicensing.IsExists(stateLicenseExtType))
            stateLicenseExtTypeList.Add(stateLicenseExtType);
        }
        foreach (StateLicenseExtType stateLicenseExtType in stateLicenseExtTypeList)
          modifiedData.RemoveStateLicenseExtType(stateLicenseExtType);
      }
      return modifiedData;
    }

    private int getSortIndex(string state, string licenseType)
    {
      int sortIndex = 0;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewStates.Items)
      {
        if (gvItem.SubItems[2].Text == state)
        {
          ++sortIndex;
          if (gvItem.SubItems[3].Text == licenseType)
            return sortIndex;
        }
      }
      return 0;
    }

    public bool DataValidated()
    {
      this.validated = false;
      if (this.cboHomeState.Enabled && this.cboHomeState.SelectedIndex == -1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The home state cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.cboHomeState.Focus();
        return false;
      }
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewStates.Items)
      {
        if (gvItem.Tag != null && gvItem.Tag.ToString().ToLower().Equals("true"))
        {
          if (gvItem.SubItems[5].Text.Trim() != "")
          {
            DateTime dateTime = Convert.ToDateTime(gvItem.SubItems[5].Text.Trim());
            if (dateTime > this.maxValue || dateTime < this.minValue)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, "The value of Issue Date '" + dateTime.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.gridViewStates.EnsureVisible(gvItem.Index);
              gvItem.SubItems[5].BeginEdit();
              return false;
            }
          }
          if (gvItem.SubItems[6].Text.Trim() != "")
          {
            DateTime dateTime = Convert.ToDateTime(gvItem.SubItems[6].Text.Trim());
            if (dateTime > this.maxValue || dateTime < this.minValue)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, "The value of Start Date '" + dateTime.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.gridViewStates.EnsureVisible(gvItem.Index);
              gvItem.SubItems[6].BeginEdit();
              return false;
            }
          }
          if (gvItem.SubItems[7].Text.Trim() != "")
          {
            DateTime dateTime = Convert.ToDateTime(gvItem.SubItems[7].Text.Trim());
            if (dateTime > this.maxValue || dateTime < this.minValue)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, "The value of End Date '" + dateTime.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.gridViewStates.EnsureVisible(gvItem.Index);
              gvItem.SubItems[7].BeginEdit();
              return false;
            }
          }
          if (gvItem.SubItems[9].Text.Trim() != "")
          {
            DateTime dateTime = Convert.ToDateTime(gvItem.SubItems[9].Text.Trim());
            if (dateTime > this.maxValue || dateTime < this.minValue)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, "The value of Status Date '" + dateTime.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.gridViewStates.EnsureVisible(gvItem.Index);
              gvItem.SubItems[9].BeginEdit();
              return false;
            }
          }
          if (gvItem.SubItems[10].Text.Trim() != "")
          {
            DateTime dateTime = Convert.ToDateTime(gvItem.SubItems[10].Text.Trim());
            if (dateTime > this.maxValue || dateTime < this.minValue)
            {
              int num = (int) MessageBox.Show((IWin32Window) this, "The value of Last Checked Date '" + dateTime.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.gridViewStates.EnsureVisible(gvItem.Index);
              gvItem.SubItems[10].BeginEdit();
              return false;
            }
          }
        }
      }
      this.validated = true;
      return true;
    }

    private bool validateDates(DateTime startDate, DateTime endDate) => !(startDate >= endDate);

    private void gridViewStates_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index != 0 && e.SubItem.Index != 1)
        return;
      string stateAbbrevation = e.SubItem.Item.SubItems[2].Text.Trim();
      string licenseKey = MaventLicenseTypesUtils.GetLicenseKey(e.SubItem.Item.SubItems[2].Text.Trim(), e.SubItem.Item.SubItems[3].Text.Trim());
      if (!this.stateTable.ContainsKey((object) this.setStateKey(stateAbbrevation, licenseKey)))
        this.stateTable.Add((object) this.setStateKey(stateAbbrevation, licenseKey), (object) new StateLicenseExtType(stateAbbrevation, licenseKey, string.Empty, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, string.Empty, DateTime.MinValue, false, false, DateTime.MinValue, 0));
      StateLicenseExtType stateLicenseExtType = (StateLicenseExtType) this.stateTable[(object) this.setStateKey(stateAbbrevation, licenseKey)];
      if (e.SubItem.Index == 0)
      {
        stateLicenseExtType.Approved = e.SubItem.Checked;
        if (!stateLicenseExtType.Approved)
        {
          stateLicenseExtType.Exempt = false;
          e.SubItem.Item.SubItems[1].Checked = false;
          e.SubItem.Item.SubItems[1].CheckBoxVisible = false;
        }
        else
          e.SubItem.Item.SubItems[1].CheckBoxVisible = string.Compare(e.SubItem.Item.SubItems[1].Text, "No License Required") != 0;
      }
      else if (e.SubItem.Index == 1)
        stateLicenseExtType.Exempt = e.SubItem.Checked;
      e.SubItem.Item.Tag = (object) true;
      this.SetButtonStatus(true);
    }

    private void gridViewStates_EditorClosing(object sender, GVSubItemEditingEventArgs e)
    {
      switch (e.SubItem.Index)
      {
        case 4:
          if (e.EditorControl.Text.Length > 49)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The license number cannot be " + (object) e.EditorControl.Text.Length + " characters long.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            e.SubItem.Item.SubItems[4].Text = e.EditorControl.Text.Substring(0, 49);
            e.Handled = true;
            break;
          }
          break;
        case 5:
        case 6:
        case 7:
        case 9:
        case 10:
          DateTime result = new DateTime();
          if (DateTime.TryParse(e.EditorControl.Text, out result))
            e.SubItem.Value = (object) result.ToString("d");
          else
            e.SubItem.Value = (object) "";
          e.Handled = true;
          break;
      }
      e.SubItem.Item.Tag = (object) true;
      this.SetButtonStatus(true);
    }

    private void gridViewStates_EditorOpening(object sender, GVSubItemEditingEventArgs e)
    {
      if (this.chkUseParentInfo.Checked)
      {
        e.Cancel = true;
      }
      else
      {
        if (e.SubItem.Index != 8)
          return;
        ComboBox editorControl = (ComboBox) e.EditorControl;
        editorControl.Items.Clear();
        editorControl.DropDownStyle = ComboBoxStyle.DropDownList;
        editorControl.Items.AddRange((object[]) new string[28]
        {
          "",
          "Transition Requested",
          "Transition Cancelled",
          "Transition Rejected",
          "Pending Incomplete",
          "Pending Review",
          "Pending Deficient",
          "Pending - Withdraw Requested",
          "Withdrawn - Application Abandoned",
          "Withdrawn - Voluntary without Licensure",
          "Denied",
          "Denied - On Appeal",
          "Approved",
          "Approved - Conditional",
          "Approved - Deficient",
          "Approved - Failed to Renew",
          "Approved - Inactive",
          "Approved - On Appeal",
          "Approved - Surrender/Cancellation Requested",
          "Revoked",
          "Revoked - On Appeal",
          "Suspended",
          "Suspended - On Appeal",
          "Temporary Cease and Desist",
          "Temporary - Expired",
          "Temporary - Failed to Renew",
          "Terminated - Ordered to Surrender",
          "Terminated - Surrendered/Cancelled"
        });
        editorControl.Text = e.SubItem.Text;
      }
    }

    private string setStateKey(string stateAbbrevation, string licenseKey)
    {
      return stateAbbrevation + "_" + licenseKey;
    }

    private void iconBtnHelpMaryland_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Statutory Elections");
    }

    private void iconBtnHelpKansas_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Statutory Elections");
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.gridViewStates.StopEditing())
        return;
      if (this.SaveButton_Clicked != null)
        this.SaveButton_Clicked(sender, e);
      if (!this.validated)
        return;
      this.RefreshData();
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset? All changes will be lost.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No || !this.gridViewStates.StopEditing())
        return;
      this.RefreshData();
      this.SetButtonStatus(false);
    }

    private void panel2_Resize(object sender, EventArgs e)
    {
      this.grpLicensingIssues.Size = new Size(this.panel2.Width / 2, this.grpLicensingIssues.Height);
    }

    public void SetButtonStatus(bool enabled)
    {
      this.btnSave.Enabled = this.btnReset.Enabled = enabled;
    }

    private void fieldChanged(object sender, EventArgs e) => this.SetButtonStatus(true);

    public bool IsDirty => this.btnSave.Enabled;

    public void DisableArrows(bool value)
    {
      if (this.chkUseParentInfo.Checked)
      {
        this.btnMoveLicenseDown.Enabled = this.btnMoveLicenseUp.Enabled = false;
      }
      else
      {
        this.btnMoveLicenseDown.Enabled = this.btnMoveLicenseUp.Enabled = !value;
        if (value || this.gridViewStates.SelectedItems.Count <= 0)
          return;
        this.btnMoveLicenseUp.Enabled = this.gridViewStates.SelectedItems[0].Index != 0;
        this.btnMoveLicenseDown.Enabled = this.gridViewStates.SelectedItems[0].Index != this.gridViewStates.Items.Count - 1;
      }
    }

    private bool isMoveButtonsEnabled() => this.btnMoveLicenseDown.Enabled;

    private void btnMoveLicenseDown_Click(object sender, EventArgs e)
    {
      if (!this.gridViewStates.StopEditing())
        return;
      GridView gridViewStates = this.gridViewStates;
      int index = gridViewStates.SelectedItems[0].Index;
      GVItem gvItem1 = gridViewStates.Items[index];
      gridViewStates.Items.RemoveAt(index);
      gridViewStates.Items.Insert(index + 1, gvItem1);
      gvItem1.Selected = true;
      gridViewStates.EnsureVisible(index + 1);
      foreach (GVItem gvItem2 in (IEnumerable<GVItem>) this.gridViewStates.Items)
        gvItem2.Tag = (object) true;
      this.SetButtonStatus(true);
    }

    private void btnMoveLicenseUp_Click(object sender, EventArgs e)
    {
      if (!this.gridViewStates.StopEditing())
        return;
      GridView gridViewStates = this.gridViewStates;
      int index = gridViewStates.SelectedItems[0].Index;
      GVItem gvItem1 = gridViewStates.Items[index];
      gridViewStates.Items.RemoveAt(index);
      gridViewStates.Items.Insert(index - 1, gvItem1);
      gvItem1.Selected = true;
      gridViewStates.EnsureVisible(index - 1);
      foreach (GVItem gvItem2 in (IEnumerable<GVItem>) this.gridViewStates.Items)
        gvItem2.Tag = (object) true;
      this.SetButtonStatus(true);
    }

    private void gridViewStates_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboTypeState.Text == "All States")
        this.DisableArrows(true);
      else if (this.gridViewStates.SelectedItems.Count == 0)
        this.DisableArrows(true);
      else if (this.chkUseParentInfo.Checked)
      {
        this.btnMoveLicenseDown.Enabled = this.btnMoveLicenseUp.Enabled = false;
      }
      else
      {
        this.btnMoveLicenseUp.Enabled = this.gridViewStates.SelectedItems[0].Index != 0;
        this.btnMoveLicenseDown.Enabled = this.gridViewStates.SelectedItems[0].Index != this.gridViewStates.Items.Count - 1;
      }
    }

    public void DisableControls()
    {
      this.btnSave.Visible = this.btnReset.Visible = this.btnMoveLicenseDown.Visible = this.btnMoveLicenseUp.Visible = false;
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

    private void checkBoxOptOut_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBoxOptOut.CheckState == CheckState.Checked)
        this.cboHomeState.Enabled = false;
      else
        this.cboHomeState.Enabled = true;
      this.SetButtonStatus(true);
    }

    private bool disableOptOut()
    {
      return this.cboLenderType.Text == "National Bank (FNB)" || this.cboLenderType.Text == "State Licensed Lender - Non Depository Institution (STLIC)" || this.cboLenderType.Text == string.Empty;
    }

    private void cmbAllowLoansWithLicenseIssues_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.cmbAllowLoansWithLicenseIssues.SelectedItem != null)
      {
        string str = this.cmbAllowLoansWithLicenseIssues.SelectedItem.ToString();
        if (!string.IsNullOrEmpty(str) && str.ToLower() == "no restrictions" && this.lastSelectedComboItemText.ToLower() != "no restrictions")
        {
          int num = (int) MessageBox.Show("Please Note: Selecting ‘No Restrictions’ will allow this company to submit loans with no limits specific to this policy.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.lastSelectedComboItemText = str;
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
      this.grpAll = new GroupContainer();
      this.chkUseParentInfo = new CheckBox();
      this.panel1 = new Panel();
      this.panel3 = new Panel();
      this.groupContainer5 = new GroupContainer();
      this.cboExemptCreditor = new ComboBox();
      this.label6 = new Label();
      this.cboSmallCreditor = new ComboBox();
      this.label5 = new Label();
      this.panel2 = new Panel();
      this.grpLicense = new GroupContainer();
      this.panelTop2 = new Panel();
      this.checkBoxOptOut = new CheckBox();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label1 = new Label();
      this.cboLenderType = new ComboBox();
      this.label2 = new Label();
      this.cboHomeState = new ComboBox();
      this.label3 = new Label();
      this.grpLicensingIssues = new GroupContainer();
      this.cmbAllowLoansWithLicenseIssues = new ComboBox();
      this.txtMsgUploadNonApproved = new TextBox();
      this.label4 = new Label();
      this.panelMarylandKansas = new Panel();
      this.grpMaryland = new GroupContainer();
      this.rdoMarylandYes2 = new RadioButton();
      this.rdoMarylandYes1 = new RadioButton();
      this.iconBtnHelpMaryland = new IconButton();
      this.rdoMarylandYes = new RadioButton();
      this.rdoMarylandNo = new RadioButton();
      this.grpKansas = new GroupContainer();
      this.iconBtnHelpKansas = new IconButton();
      this.rdoKansasYes = new RadioButton();
      this.rdoKansasNo = new RadioButton();
      this.panelLicenseList = new Panel();
      this.grpLicenseList = new GroupContainer();
      this.label9 = new Label();
      this.btnMoveLicenseUp = new StandardIconButton();
      this.btnMoveLicenseDown = new StandardIconButton();
      this.cboTypeState = new ComboBox();
      this.gridViewStates = new GridView();
      this.btnReset = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.panelHeader = new Panel();
      this.label33 = new Label();
      this.grpAll.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.groupContainer5.SuspendLayout();
      this.panel2.SuspendLayout();
      this.grpLicense.SuspendLayout();
      this.panelTop2.SuspendLayout();
      this.grpLicensingIssues.SuspendLayout();
      this.panelMarylandKansas.SuspendLayout();
      this.grpMaryland.SuspendLayout();
      ((ISupportInitialize) this.iconBtnHelpMaryland).BeginInit();
      this.grpKansas.SuspendLayout();
      ((ISupportInitialize) this.iconBtnHelpKansas).BeginInit();
      this.panelLicenseList.SuspendLayout();
      this.grpLicenseList.SuspendLayout();
      ((ISupportInitialize) this.btnMoveLicenseUp).BeginInit();
      ((ISupportInitialize) this.btnMoveLicenseDown).BeginInit();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.grpAll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpAll.Controls.Add((Control) this.chkUseParentInfo);
      this.grpAll.Controls.Add((Control) this.panel1);
      this.grpAll.Controls.Add((Control) this.btnReset);
      this.grpAll.Controls.Add((Control) this.btnSave);
      this.grpAll.Controls.Add((Control) this.panelHeader);
      this.grpAll.HeaderForeColor = SystemColors.ControlText;
      this.grpAll.Location = new Point(5, 5);
      this.grpAll.Margin = new Padding(0);
      this.grpAll.Name = "grpAll";
      this.grpAll.Size = new Size(862, 610);
      this.grpAll.TabIndex = 1;
      this.grpAll.Text = "License";
      this.chkUseParentInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfo.AutoSize = true;
      this.chkUseParentInfo.BackColor = Color.Transparent;
      this.chkUseParentInfo.Location = new Point(693, 4);
      this.chkUseParentInfo.Name = "chkUseParentInfo";
      this.chkUseParentInfo.Size = new Size(100, 17);
      this.chkUseParentInfo.TabIndex = 0;
      this.chkUseParentInfo.Text = "Use Parent Info";
      this.chkUseParentInfo.UseVisualStyleBackColor = false;
      this.panel1.Controls.Add((Control) this.panel3);
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Controls.Add((Control) this.panelMarylandKansas);
      this.panel1.Controls.Add((Control) this.panelLicenseList);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(1, 52);
      this.panel1.Margin = new Padding(5);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(5);
      this.panel1.Size = new Size(860, 557);
      this.panel1.TabIndex = 32;
      this.panel3.Controls.Add((Control) this.groupContainer5);
      this.panel3.Dock = DockStyle.Bottom;
      this.panel3.Location = new Point(5, 463);
      this.panel3.Margin = new Padding(5);
      this.panel3.Name = "panel3";
      this.panel3.Padding = new Padding(3);
      this.panel3.Size = new Size(850, 89);
      this.panel3.TabIndex = 6;
      this.groupContainer5.Controls.Add((Control) this.cboExemptCreditor);
      this.groupContainer5.Controls.Add((Control) this.label6);
      this.groupContainer5.Controls.Add((Control) this.cboSmallCreditor);
      this.groupContainer5.Controls.Add((Control) this.label5);
      this.groupContainer5.Dock = DockStyle.Fill;
      this.groupContainer5.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer5.Location = new Point(3, 3);
      this.groupContainer5.Margin = new Padding(5);
      this.groupContainer5.Name = "groupContainer5";
      this.groupContainer5.Padding = new Padding(3);
      this.groupContainer5.Size = new Size(844, 83);
      this.groupContainer5.TabIndex = 6;
      this.groupContainer5.Text = "ATR/QM";
      this.cboExemptCreditor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboExemptCreditor.FormattingEnabled = true;
      this.cboExemptCreditor.Items.AddRange(new object[5]
      {
        (object) "",
        (object) "Community Development Financial Institution",
        (object) "Community Housing Development Organization",
        (object) "Downpayment Assistance Provider",
        (object) "Nonprofit Organization"
      });
      this.cboExemptCreditor.Location = new Point(103, 55);
      this.cboExemptCreditor.Name = "cboExemptCreditor";
      this.cboExemptCreditor.Size = new Size(296, 21);
      this.cboExemptCreditor.TabIndex = 11;
      this.cboExemptCreditor.SelectedIndexChanged += new EventHandler(this.fieldChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(20, 61);
      this.label6.Name = "label6";
      this.label6.Size = new Size(81, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "Exempt Creditor";
      this.cboSmallCreditor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSmallCreditor.FormattingEnabled = true;
      this.cboSmallCreditor.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Small Creditor",
        (object) "Rural Small Creditor"
      });
      this.cboSmallCreditor.Location = new Point(103, 31);
      this.cboSmallCreditor.Name = "cboSmallCreditor";
      this.cboSmallCreditor.Size = new Size(296, 21);
      this.cboSmallCreditor.TabIndex = 10;
      this.cboSmallCreditor.SelectedIndexChanged += new EventHandler(this.fieldChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(20, 37);
      this.label5.Name = "label5";
      this.label5.Size = new Size(71, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Small Creditor";
      this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panel2.Controls.Add((Control) this.grpLicense);
      this.panel2.Controls.Add((Control) this.grpLicensingIssues);
      this.panel2.Location = new Point(5, 6);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new Padding(3);
      this.panel2.Size = new Size(850, 125);
      this.panel2.TabIndex = 0;
      this.panel2.Resize += new EventHandler(this.panel2_Resize);
      this.grpLicense.Controls.Add((Control) this.panelTop2);
      this.grpLicense.HeaderForeColor = SystemColors.ControlText;
      this.grpLicense.Location = new Point(3, 3);
      this.grpLicense.Name = "grpLicense";
      this.grpLicense.Size = new Size(415, 116);
      this.grpLicense.TabIndex = 1;
      this.grpLicense.Text = "License";
      this.panelTop2.Controls.Add((Control) this.checkBoxOptOut);
      this.panelTop2.Controls.Add((Control) this.label8);
      this.panelTop2.Controls.Add((Control) this.label7);
      this.panelTop2.Controls.Add((Control) this.label1);
      this.panelTop2.Controls.Add((Control) this.cboLenderType);
      this.panelTop2.Controls.Add((Control) this.label2);
      this.panelTop2.Controls.Add((Control) this.cboHomeState);
      this.panelTop2.Controls.Add((Control) this.label3);
      this.panelTop2.Dock = DockStyle.Top;
      this.panelTop2.Location = new Point(1, 26);
      this.panelTop2.Name = "panelTop2";
      this.panelTop2.Size = new Size(413, 86);
      this.panelTop2.TabIndex = 2;
      this.checkBoxOptOut.AutoSize = true;
      this.checkBoxOptOut.Location = new Point(10, 65);
      this.checkBoxOptOut.Name = "checkBoxOptOut";
      this.checkBoxOptOut.Size = new Size(293, 17);
      this.checkBoxOptOut.TabIndex = 8;
      this.checkBoxOptOut.Text = "My Company wishes to not apply interest rate exportation";
      this.checkBoxOptOut.UseVisualStyleBackColor = true;
      this.checkBoxOptOut.CheckedChanged += new EventHandler(this.checkBoxOptOut_CheckedChanged);
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Transparent;
      this.label8.ForeColor = Color.Red;
      this.label8.Location = new Point(70, 30);
      this.label8.Name = "label8";
      this.label8.Size = new Size(11, 13);
      this.label8.TabIndex = 7;
      this.label8.Text = "*";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(146, 47);
      this.label7.Name = "label7";
      this.label7.Size = new Size(105, 13);
      this.label7.TabIndex = 6;
      this.label7.Text = "selected home state.";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(67, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Lender Type";
      this.cboLenderType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLenderType.FormattingEnabled = true;
      this.cboLenderType.Location = new Point(88, 5);
      this.cboLenderType.Name = "cboLenderType";
      this.cboLenderType.Size = new Size(310, 21);
      this.cboLenderType.TabIndex = 1;
      this.cboLenderType.SelectedIndexChanged += new EventHandler(this.cboLenderType_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 31);
      this.label2.Name = "label2";
      this.label2.Size = new Size(63, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Home State";
      this.cboHomeState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboHomeState.FormattingEnabled = true;
      this.cboHomeState.Location = new Point(88, 28);
      this.cboHomeState.Name = "cboHomeState";
      this.cboHomeState.Size = new Size(49, 21);
      this.cboHomeState.TabIndex = 2;
      this.cboHomeState.SelectedIndexChanged += new EventHandler(this.fieldChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(143, 31);
      this.label3.Name = "label3";
      this.label3.Size = new Size(239, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "The interest rate exportation will be based on the ";
      this.grpLicensingIssues.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.grpLicensingIssues.Controls.Add((Control) this.cmbAllowLoansWithLicenseIssues);
      this.grpLicensingIssues.Controls.Add((Control) this.txtMsgUploadNonApproved);
      this.grpLicensingIssues.Controls.Add((Control) this.label4);
      this.grpLicensingIssues.HeaderForeColor = SystemColors.ControlText;
      this.grpLicensingIssues.Location = new Point(417, 3);
      this.grpLicensingIssues.Name = "grpLicensingIssues";
      this.grpLicensingIssues.Size = new Size(440, 116);
      this.grpLicensingIssues.TabIndex = 2;
      this.grpLicensingIssues.Text = "Policy for Loans in Unlicensed States";
      this.cmbAllowLoansWithLicenseIssues.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cmbAllowLoansWithLicenseIssues.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbAllowLoansWithLicenseIssues.FormattingEnabled = true;
      this.cmbAllowLoansWithLicenseIssues.Items.AddRange(new object[3]
      {
        (object) "No Restrictions",
        (object) "Don't allow lock or submission",
        (object) "Don't allow loan creation"
      });
      this.cmbAllowLoansWithLicenseIssues.Location = new Point(264, 3);
      this.cmbAllowLoansWithLicenseIssues.Name = "cmbAllowLoansWithLicenseIssues";
      this.cmbAllowLoansWithLicenseIssues.Size = new Size(168, 21);
      this.cmbAllowLoansWithLicenseIssues.TabIndex = 13;
      this.cmbAllowLoansWithLicenseIssues.SelectionChangeCommitted += new EventHandler(this.cmbAllowLoansWithLicenseIssues_SelectionChangeCommitted);
      this.txtMsgUploadNonApproved.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtMsgUploadNonApproved.Location = new Point(104, 30);
      this.txtMsgUploadNonApproved.Multiline = true;
      this.txtMsgUploadNonApproved.Name = "txtMsgUploadNonApproved";
      this.txtMsgUploadNonApproved.ScrollBars = ScrollBars.Vertical;
      this.txtMsgUploadNonApproved.Size = new Size(316, 49);
      this.txtMsgUploadNonApproved.TabIndex = 4;
      this.txtMsgUploadNonApproved.TextChanged += new EventHandler(this.fieldChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 33);
      this.label4.Name = "label4";
      this.label4.Size = new Size(93, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Warning Message";
      this.panelMarylandKansas.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelMarylandKansas.Controls.Add((Control) this.grpMaryland);
      this.panelMarylandKansas.Controls.Add((Control) this.grpKansas);
      this.panelMarylandKansas.Location = new Point(5, 355);
      this.panelMarylandKansas.Margin = new Padding(5);
      this.panelMarylandKansas.Name = "panelMarylandKansas";
      this.panelMarylandKansas.Padding = new Padding(3);
      this.panelMarylandKansas.Size = new Size(851, 112);
      this.panelMarylandKansas.TabIndex = 5;
      this.grpMaryland.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.grpMaryland.Controls.Add((Control) this.rdoMarylandYes2);
      this.grpMaryland.Controls.Add((Control) this.rdoMarylandYes1);
      this.grpMaryland.Controls.Add((Control) this.iconBtnHelpMaryland);
      this.grpMaryland.Controls.Add((Control) this.rdoMarylandYes);
      this.grpMaryland.Controls.Add((Control) this.rdoMarylandNo);
      this.grpMaryland.HeaderForeColor = SystemColors.ControlText;
      this.grpMaryland.Location = new Point(1, 2);
      this.grpMaryland.Margin = new Padding(0, 3, 3, 3);
      this.grpMaryland.Name = "grpMaryland";
      this.grpMaryland.Size = new Size(417, 104);
      this.grpMaryland.TabIndex = 3;
      this.grpMaryland.Text = "Statutory Election in Maryland";
      this.rdoMarylandYes2.AutoSize = true;
      this.rdoMarylandYes2.Location = new Point(15, 84);
      this.rdoMarylandYes2.Name = "rdoMarylandYes2";
      this.rdoMarylandYes2.Size = new Size(235, 17);
      this.rdoMarylandYes2.TabIndex = 11;
      this.rdoMarylandYes2.Text = "Credit Grantor Law Election (for 1-4 unit only)";
      this.rdoMarylandYes2.UseVisualStyleBackColor = true;
      this.rdoMarylandYes2.Click += new EventHandler(this.fieldChanged);
      this.rdoMarylandYes1.AutoSize = true;
      this.rdoMarylandYes1.Location = new Point(15, 46);
      this.rdoMarylandYes1.Name = "rdoMarylandYes1";
      this.rdoMarylandYes1.Size = new Size(221, 17);
      this.rdoMarylandYes1.TabIndex = 10;
      this.rdoMarylandYes1.Text = "Credit Grantor Law Election (for All Loans)";
      this.rdoMarylandYes1.UseVisualStyleBackColor = true;
      this.rdoMarylandYes1.Click += new EventHandler(this.fieldChanged);
      this.iconBtnHelpMaryland.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnHelpMaryland.DisabledImage = (Image) null;
      this.iconBtnHelpMaryland.Image = (Image) Resources.help;
      this.iconBtnHelpMaryland.Location = new Point(397, 6);
      this.iconBtnHelpMaryland.MouseDownImage = (Image) null;
      this.iconBtnHelpMaryland.MouseOverImage = (Image) Resources.help_over;
      this.iconBtnHelpMaryland.Name = "iconBtnHelpMaryland";
      this.iconBtnHelpMaryland.Size = new Size(16, 16);
      this.iconBtnHelpMaryland.TabIndex = 2;
      this.iconBtnHelpMaryland.TabStop = false;
      this.iconBtnHelpMaryland.Click += new EventHandler(this.iconBtnHelpMaryland_Click);
      this.rdoMarylandYes.AutoSize = true;
      this.rdoMarylandYes.Location = new Point(15, 65);
      this.rdoMarylandYes.Name = "rdoMarylandYes";
      this.rdoMarylandYes.Size = new Size(275, 17);
      this.rdoMarylandYes.TabIndex = 7;
      this.rdoMarylandYes.TabStop = true;
      this.rdoMarylandYes.Text = "Credit Grantor Law Election (for 1-4 unit, Jr. Lien only)";
      this.rdoMarylandYes.UseVisualStyleBackColor = true;
      this.rdoMarylandYes.Click += new EventHandler(this.fieldChanged);
      this.rdoMarylandNo.AutoSize = true;
      this.rdoMarylandNo.Checked = true;
      this.rdoMarylandNo.Location = new Point(15, 29);
      this.rdoMarylandNo.Name = "rdoMarylandNo";
      this.rdoMarylandNo.Size = new Size(125, 17);
      this.rdoMarylandNo.TabIndex = 6;
      this.rdoMarylandNo.TabStop = true;
      this.rdoMarylandNo.Text = "No Statutory Election";
      this.rdoMarylandNo.UseVisualStyleBackColor = true;
      this.rdoMarylandNo.Click += new EventHandler(this.fieldChanged);
      this.grpKansas.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.grpKansas.Controls.Add((Control) this.iconBtnHelpKansas);
      this.grpKansas.Controls.Add((Control) this.rdoKansasYes);
      this.grpKansas.Controls.Add((Control) this.rdoKansasNo);
      this.grpKansas.HeaderForeColor = SystemColors.ControlText;
      this.grpKansas.Location = new Point(424, 2);
      this.grpKansas.Name = "grpKansas";
      this.grpKansas.Size = new Size(423, 71);
      this.grpKansas.TabIndex = 4;
      this.grpKansas.Text = "Statutory Election in Kansas";
      this.iconBtnHelpKansas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnHelpKansas.DisabledImage = (Image) null;
      this.iconBtnHelpKansas.Image = (Image) Resources.help;
      this.iconBtnHelpKansas.Location = new Point(403, 6);
      this.iconBtnHelpKansas.MouseDownImage = (Image) null;
      this.iconBtnHelpKansas.MouseOverImage = (Image) Resources.help_over;
      this.iconBtnHelpKansas.Name = "iconBtnHelpKansas";
      this.iconBtnHelpKansas.Size = new Size(16, 16);
      this.iconBtnHelpKansas.TabIndex = 4;
      this.iconBtnHelpKansas.TabStop = false;
      this.iconBtnHelpKansas.Click += new EventHandler(this.iconBtnHelpKansas_Click);
      this.rdoKansasYes.AutoSize = true;
      this.rdoKansasYes.Location = new Point(16, 47);
      this.rdoKansasYes.Name = "rdoKansasYes";
      this.rdoKansasYes.Size = new Size(197, 17);
      this.rdoKansasYes.TabIndex = 9;
      this.rdoKansasYes.TabStop = true;
      this.rdoKansasYes.Text = "Kansas UCCC Election For All Loans";
      this.rdoKansasYes.UseVisualStyleBackColor = true;
      this.rdoKansasYes.Click += new EventHandler(this.fieldChanged);
      this.rdoKansasNo.AutoSize = true;
      this.rdoKansasNo.Checked = true;
      this.rdoKansasNo.Location = new Point(16, 29);
      this.rdoKansasNo.Name = "rdoKansasNo";
      this.rdoKansasNo.Size = new Size(125, 17);
      this.rdoKansasNo.TabIndex = 8;
      this.rdoKansasNo.TabStop = true;
      this.rdoKansasNo.Text = "No Statutory Election";
      this.rdoKansasNo.UseVisualStyleBackColor = true;
      this.rdoKansasNo.Click += new EventHandler(this.fieldChanged);
      this.panelLicenseList.Controls.Add((Control) this.grpLicenseList);
      this.panelLicenseList.Location = new Point(5, 134);
      this.panelLicenseList.Margin = new Padding(5);
      this.panelLicenseList.Name = "panelLicenseList";
      this.panelLicenseList.Padding = new Padding(3);
      this.panelLicenseList.Size = new Size(851, 221);
      this.panelLicenseList.TabIndex = 2;
      this.grpLicenseList.Controls.Add((Control) this.label9);
      this.grpLicenseList.Controls.Add((Control) this.btnMoveLicenseUp);
      this.grpLicenseList.Controls.Add((Control) this.btnMoveLicenseDown);
      this.grpLicenseList.Controls.Add((Control) this.cboTypeState);
      this.grpLicenseList.Controls.Add((Control) this.gridViewStates);
      this.grpLicenseList.Dock = DockStyle.Fill;
      this.grpLicenseList.HeaderForeColor = SystemColors.ControlText;
      this.grpLicenseList.Location = new Point(3, 3);
      this.grpLicenseList.Name = "grpLicenseList";
      this.grpLicenseList.Size = new Size(845, 215);
      this.grpLicenseList.TabIndex = 2;
      this.grpLicenseList.Text = "License Type in ";
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.Location = new Point(228, 6);
      this.label9.Name = "label9";
      this.label9.Size = new Size(256, 13);
      this.label9.TabIndex = 38;
      this.label9.Text = "Enter license information directly into the table below.";
      this.btnMoveLicenseUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveLicenseUp.BackColor = Color.Transparent;
      this.btnMoveLicenseUp.Enabled = false;
      this.btnMoveLicenseUp.Location = new Point(820, 6);
      this.btnMoveLicenseUp.MouseDownImage = (Image) null;
      this.btnMoveLicenseUp.Name = "btnMoveLicenseUp";
      this.btnMoveLicenseUp.Size = new Size(16, 17);
      this.btnMoveLicenseUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveLicenseUp.TabIndex = 37;
      this.btnMoveLicenseUp.TabStop = false;
      this.btnMoveLicenseUp.Click += new EventHandler(this.btnMoveLicenseUp_Click);
      this.btnMoveLicenseDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveLicenseDown.BackColor = Color.Transparent;
      this.btnMoveLicenseDown.Enabled = false;
      this.btnMoveLicenseDown.Location = new Point(798, 6);
      this.btnMoveLicenseDown.MouseDownImage = (Image) null;
      this.btnMoveLicenseDown.Name = "btnMoveLicenseDown";
      this.btnMoveLicenseDown.Size = new Size(16, 17);
      this.btnMoveLicenseDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveLicenseDown.TabIndex = 36;
      this.btnMoveLicenseDown.TabStop = false;
      this.btnMoveLicenseDown.Click += new EventHandler(this.btnMoveLicenseDown_Click);
      this.cboTypeState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTypeState.FormattingEnabled = true;
      this.cboTypeState.Location = new Point(113, 2);
      this.cboTypeState.Name = "cboTypeState";
      this.cboTypeState.Size = new Size(109, 21);
      this.cboTypeState.TabIndex = 5;
      this.gridViewStates.AllowMultiselect = false;
      this.gridViewStates.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnSelect";
      gvColumn1.Text = "Select";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 60;
      gvColumn2.CheckBoxes = true;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnExempt";
      gvColumn2.Text = "Exempt";
      gvColumn2.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn2.Width = 50;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnState";
      gvColumn3.Text = "State";
      gvColumn3.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn3.Width = 50;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnType";
      gvColumn4.Text = "License Type";
      gvColumn4.Width = 200;
      gvColumn5.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnLicenseNo";
      gvColumn5.Text = "License #";
      gvColumn5.Width = 150;
      gvColumn6.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnIssueDate";
      gvColumn6.Text = "Issue Date";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 80;
      gvColumn7.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnStartDate";
      gvColumn7.Text = "Start Date";
      gvColumn7.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn7.Width = 80;
      gvColumn8.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnEndDate";
      gvColumn8.Text = "End Date";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 80;
      gvColumn9.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "ColumnStatus";
      gvColumn9.Text = "Status";
      gvColumn9.Width = 250;
      gvColumn10.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "ColumnStatusDate";
      gvColumn10.Text = "Status Date";
      gvColumn10.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn10.Width = 80;
      gvColumn11.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "ColumnLastCheck";
      gvColumn11.Text = "Last Checked";
      gvColumn11.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn11.Width = 80;
      this.gridViewStates.Columns.AddRange(new GVColumn[11]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11
      });
      this.gridViewStates.Dock = DockStyle.Fill;
      this.gridViewStates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewStates.Location = new Point(1, 26);
      this.gridViewStates.Name = "gridViewStates";
      this.gridViewStates.Size = new Size(843, 188);
      this.gridViewStates.SortIconVisible = false;
      this.gridViewStates.SortOption = GVSortOption.None;
      this.gridViewStates.TabIndex = 2;
      this.gridViewStates.SelectedIndexChanged += new EventHandler(this.gridViewStates_SelectedIndexChanged);
      this.gridViewStates.SubItemCheck += new GVSubItemEventHandler(this.gridViewStates_SubItemCheck);
      this.gridViewStates.EditorOpening += new GVSubItemEditingEventHandler(this.gridViewStates_EditorOpening);
      this.gridViewStates.EditorClosing += new GVSubItemEditingEventHandler(this.gridViewStates_EditorClosing);
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(837, 4);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 31;
      this.btnReset.TabStop = false;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(815, 4);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 30;
      this.btnSave.TabStop = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.panelHeader.Controls.Add((Control) this.label33);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(860, 26);
      this.panelHeader.TabIndex = 0;
      this.label33.AutoSize = true;
      this.label33.Location = new Point(6, 6);
      this.label33.Name = "label33";
      this.label33.Size = new Size(372, 13);
      this.label33.TabIndex = 37;
      this.label33.Text = "Enter the license information for the Third Party Originator company or branch.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpAll);
      this.Margin = new Padding(0);
      this.Name = nameof (EditOrgLicenseControl);
      this.Padding = new Padding(5);
      this.Size = new Size(872, 620);
      this.grpAll.ResumeLayout(false);
      this.grpAll.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.groupContainer5.ResumeLayout(false);
      this.groupContainer5.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.grpLicense.ResumeLayout(false);
      this.panelTop2.ResumeLayout(false);
      this.panelTop2.PerformLayout();
      this.grpLicensingIssues.ResumeLayout(false);
      this.grpLicensingIssues.PerformLayout();
      this.panelMarylandKansas.ResumeLayout(false);
      this.grpMaryland.ResumeLayout(false);
      this.grpMaryland.PerformLayout();
      ((ISupportInitialize) this.iconBtnHelpMaryland).EndInit();
      this.grpKansas.ResumeLayout(false);
      this.grpKansas.PerformLayout();
      ((ISupportInitialize) this.iconBtnHelpKansas).EndInit();
      this.panelLicenseList.ResumeLayout(false);
      this.grpLicenseList.ResumeLayout(false);
      this.grpLicenseList.PerformLayout();
      ((ISupportInitialize) this.btnMoveLicenseUp).EndInit();
      ((ISupportInitialize) this.btnMoveLicenseDown).EndInit();
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
