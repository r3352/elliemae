// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddEditOrgLicenseControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddEditOrgLicenseControl : UserControl
  {
    private bool forCompanyInfo;
    private string[] statesAbbrevation;
    private Hashtable stateTable;
    private BranchExtLicensing branchLicensing;
    private IOrganizationManager rOrg;
    private int parentID;
    private int currentOID;
    private const string ALLSTATES = "All States";
    private bool dirty;
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
    private IContainer components;
    private GroupContainer groupContainer1;
    private Label label3;
    private ComboBox cboHomeState;
    private Label label2;
    private ComboBox cboLenderType;
    private Label label1;
    private GroupContainer groupContainer4;
    private GroupContainer groupContainer3;
    private GroupContainer groupContainer2;
    private ComboBox cboTypeState;
    private RadioButton rdoKansasYes;
    private RadioButton rdoKansasNo;
    private RadioButton rdoMarylandYes;
    private RadioButton rdoMarylandNo;
    private CheckBox chkUseParentInfo;
    private IconButton iconBtnHelpKansas;
    private IconButton iconBtnHelpMaryland;
    private Label label4;
    private CheckBox chkUseCustomLender;
    private Panel panelTop1;
    private Panel panelTop2;
    private GroupContainer groupContainer5;
    private ComboBox cboExemptCreditor;
    private Label label6;
    private ComboBox cboSmallCreditor;
    private Label label5;
    private StandardIconButton btnMoveLicenseUp;
    private StandardIconButton btnMoveLicenseDown;
    private GridView gridViewStates;
    private RadioButton rdoMarylandYes1;
    private RadioButton rdoMarylandYes2;
    private CheckBox checkBoxOptOut;

    public event EventHandler DataChanged;

    public AddEditOrgLicenseControl(bool forCompanyInfo)
      : this(forCompanyInfo, (IOrganizationManager) null, 0, 0)
    {
    }

    public AddEditOrgLicenseControl(
      bool forCompanyInfo,
      IOrganizationManager rOrg,
      int parentID,
      int currentOID)
    {
      this.forCompanyInfo = forCompanyInfo;
      this.rOrg = rOrg;
      this.parentID = parentID;
      this.currentOID = currentOID;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.chkUseParentInfo.Visible = !this.forCompanyInfo;
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
      this.cboLenderType.Items.AddRange((object[]) AddEditOrgLicenseControl.lenderTypeList);
      this.Resize += new EventHandler(this.addEditOrgLicenseControl_Resize);
      this.cboTypeState.SelectedIndexChanged += new EventHandler(this.cboTypeState_SelectedIndexChanged);
      if (!this.forCompanyInfo)
        this.chkUseParentInfo.Click += new EventHandler(this.chkUseParentInfo_Click);
      this.dirty = false;
    }

    private void addEditOrgLicenseControl_Resize(object sender, EventArgs e)
    {
      this.groupContainer3.Width = this.groupContainer2.Width / 2 + 25;
      this.groupContainer4.Width = this.groupContainer3.Width - 55;
      this.groupContainer4.Left = this.groupContainer3.Left + this.groupContainer3.Width + 4;
    }

    public void RefreshData(BranchExtLicensing branchLicensing)
    {
      this.branchLicensing = branchLicensing;
      if (this.branchLicensing != null)
      {
        this.cboLenderType.Text = this.branchLicensing.LenderType;
        this.cboHomeState.Text = this.branchLicensing.HomeState;
        this.checkBoxOptOut.CheckState = this.branchLicensing.OptOut == "Y" ? CheckState.Checked : CheckState.Unchecked;
      }
      this.stateTable = new Hashtable();
      if (this.branchLicensing != null)
      {
        for (int index = 0; index < this.branchLicensing.StateLicenseExtTypes.Count; ++index)
        {
          if (!(this.branchLicensing.StateLicenseExtTypes[index].LicenseType == string.Empty) && !(this.branchLicensing.StateLicenseExtTypes[index].StateAbbrevation == "GU") && !(this.branchLicensing.StateLicenseExtTypes[index].StateAbbrevation == "PR") && !(this.branchLicensing.StateLicenseExtTypes[index].StateAbbrevation == "VI") && !this.stateTable.ContainsKey((object) this.setStateKey(this.branchLicensing.StateLicenseExtTypes[index].StateAbbrevation, this.branchLicensing.StateLicenseExtTypes[index].LicenseType)))
            this.stateTable.Add((object) this.setStateKey(this.branchLicensing.StateLicenseExtTypes[index].StateAbbrevation, this.branchLicensing.StateLicenseExtTypes[index].LicenseType), (object) this.branchLicensing.StateLicenseExtTypes[index]);
        }
        this.rdoMarylandYes.Checked = this.branchLicensing.StatutoryElectionInMaryland;
        this.rdoMarylandYes1.Checked = this.branchLicensing.StatutoryElectionInMaryland2 == "10";
        this.rdoMarylandYes2.Checked = this.branchLicensing.StatutoryElectionInMaryland2 == "01";
        this.rdoKansasYes.Checked = this.branchLicensing.StatutoryElectionInKansas;
        this.chkUseCustomLender.CheckedChanged -= new EventHandler(this.chkUseCustomLender_CheckedChanged);
        this.chkUseCustomLender.Checked = this.branchLicensing.UseCustomLenderProfile;
        this.chkUseCustomLender_CheckedChanged((object) null, (EventArgs) null);
        this.chkUseCustomLender.CheckedChanged += new EventHandler(this.chkUseCustomLender_CheckedChanged);
        this.cboSmallCreditor.SelectedIndexChanged -= new EventHandler(this.fieldChanged);
        this.cboSmallCreditor.Text = this.branchLicensing.ATRSmallCreditorToString();
        this.cboSmallCreditor.SelectedIndexChanged += new EventHandler(this.fieldChanged);
        this.cboExemptCreditor.SelectedIndexChanged -= new EventHandler(this.fieldChanged);
        this.cboExemptCreditor.Text = this.branchLicensing.ATRExemptCreditorToString();
        this.cboExemptCreditor.SelectedIndexChanged += new EventHandler(this.fieldChanged);
      }
      this.dirty = false;
    }

    public void ReloadStateView()
    {
      this.cboTypeState_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    public void SetUseParentInfo(bool useParentInfo)
    {
      this.chkUseParentInfo.Checked = useParentInfo;
      this.SetReadOnly(useParentInfo);
    }

    public bool IsDirty => this.dirty;

    public void SetDirtyFlag(bool val) => this.dirty = val;

    public void SetReadOnly(bool readOnly)
    {
      this.chkUseCustomLender.Enabled = !readOnly;
      this.cboLenderType.Enabled = this.cboHomeState.Enabled = this.checkBoxOptOut.Enabled = !readOnly;
      this.rdoMarylandYes.Enabled = this.rdoMarylandNo.Enabled = this.rdoMarylandYes1.Enabled = this.rdoMarylandYes2.Enabled = this.rdoKansasYes.Enabled = this.rdoKansasNo.Enabled = this.cboSmallCreditor.Enabled = this.cboExemptCreditor.Enabled = !readOnly;
      if (!this.cboLenderType.Enabled)
      {
        this.checkBoxOptOut.Enabled = this.cboHomeState.Enabled = this.cboLenderType.Enabled;
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
        this.branchLicensing = this.GetModifiedData();
        this.RefreshData(this.branchLicensing);
      }
      this.gridViewStates.BeginUpdate();
      this.gridViewStates.Items.Clear();
      this.disableArrows(this.cboTypeState.Text == "All States");
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
              if (this.stateTable.ContainsKey((object) this.setStateKey(this.statesAbbrevation[index1], MaventLicenseTypesUtils.GetLicenseKey(this.statesAbbrevation[index1], licenseTypes[index2]))))
              {
                StateLicenseExtType stateLicenseExtType = (StateLicenseExtType) this.stateTable[(object) this.setStateKey(this.statesAbbrevation[index1], MaventLicenseTypesUtils.GetLicenseKey(this.statesAbbrevation[index1], licenseTypes[index2]))];
                key.SubItems[0].Checked = stateLicenseExtType.Approved;
                if (string.Compare(licenseTypes[index2], "No License Required", true) != 0)
                {
                  key.SubItems[1].Checked = stateLicenseExtType.Exempt;
                  key.SubItems[1].CheckBoxVisible = stateLicenseExtType.Approved;
                  key.SubItems[4].Text = stateLicenseExtType.LicenseNo;
                  DateTime dateTime = stateLicenseExtType.IssueDate;
                  if (dateTime.Year > 1900)
                  {
                    GVSubItem subItem = key.SubItems[5];
                    dateTime = stateLicenseExtType.IssueDate;
                    string str = dateTime.ToString("d");
                    subItem.Value = (object) str;
                  }
                  dateTime = stateLicenseExtType.StartDate;
                  if (dateTime.Year > 1900)
                  {
                    GVSubItem subItem = key.SubItems[6];
                    dateTime = stateLicenseExtType.StartDate;
                    string str = dateTime.ToString("d");
                    subItem.Value = (object) str;
                  }
                  dateTime = stateLicenseExtType.EndDate;
                  if (dateTime.Year > 1900)
                  {
                    GVSubItem subItem = key.SubItems[7];
                    dateTime = stateLicenseExtType.EndDate;
                    string str = dateTime.ToString("d");
                    subItem.Value = (object) str;
                  }
                  key.SubItems[8].Value = (object) stateLicenseExtType.LicenseStatus;
                  dateTime = stateLicenseExtType.StatusDate;
                  if (dateTime.Year > 1900)
                  {
                    GVSubItem subItem = key.SubItems[9];
                    dateTime = stateLicenseExtType.StatusDate;
                    string str = dateTime.ToString("d");
                    subItem.Value = (object) str;
                  }
                  dateTime = stateLicenseExtType.LastChecked;
                  if (dateTime.Year > 1900)
                  {
                    GVSubItem subItem = key.SubItems[10];
                    dateTime = stateLicenseExtType.LastChecked;
                    string str = dateTime.ToString("d");
                    subItem.Value = (object) str;
                  }
                  if (stateLicenseExtType.SortIndex > 0)
                    dictionary.Add(key, stateLicenseExtType.SortIndex);
                  key.Tag = (object) false;
                }
                else
                  key.SubItems[1].CheckBoxVisible = false;
              }
              else
                key.SubItems[1].CheckBoxVisible = false;
              key.SubItems[0].CheckBoxEnabled = !this.chkUseParentInfo.Checked;
              key.SubItems[1].CheckBoxEnabled = !this.chkUseParentInfo.Checked;
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
        this.disableArrows(true);
      else
        this.gridViewStates.Items[0].Selected = true;
      this.gridViewStates.EndUpdate();
      this.gridViewStates.EndUpdate();
    }

    public BranchExtLicensing CurrentBranchLicensing
    {
      get
      {
        BranchExtLicensing branchExtLicensing = new BranchExtLicensing(this.chkUseParentInfo.Checked, this.chkUseParentInfo.Checked ? (string) null : this.cboLenderType.Text, this.chkUseParentInfo.Checked ? "" : this.cboHomeState.Text, this.checkBoxOptOut.Checked ? "Y" : "N", this.rdoMarylandYes.Checked && !this.chkUseParentInfo.Checked, this.chkUseParentInfo.Checked ? "00" : this.getMarylandElection2(), this.rdoKansasYes.Checked && !this.chkUseParentInfo.Checked, (List<StateLicenseType>) null, this.chkUseCustomLender.Checked, BranchLicensing.ATRSmallCreditorToEnum(this.cboSmallCreditor.SelectedIndex), BranchLicensing.ATRExemptCreditorToEnum(this.cboExemptCreditor.SelectedIndex));
        return !this.chkUseParentInfo.Checked ? this.GetModifiedData() : branchExtLicensing;
      }
    }

    public bool DataValidated()
    {
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
      return true;
    }

    private void cboLenderType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.disableOptOut())
      {
        this.checkBoxOptOut.CheckState = CheckState.Unchecked;
        this.checkBoxOptOut.Enabled = false;
      }
      else if (this.chkUseParentInfo.CheckState == CheckState.Unchecked)
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
      this.fieldChanged(sender, e);
    }

    private void chkUseParentInfo_Click(object sender, EventArgs e)
    {
      this.fieldChanged(sender, e);
      if (this.chkUseParentInfo.Checked)
        this.ReloadParentStateLicensingInfo((this.currentOID != -1 || this.parentID == -1 ? this.rOrg.GetOrganization(this.currentOID) : this.rOrg.GetOrganization(this.parentID)).Parent);
      this.SetReadOnly(this.chkUseParentInfo.Checked);
    }

    public void ReloadParentStateLicensingInfo(int uID)
    {
      OrgInfo withStateLicensing = this.rOrg.GetFirstOrganizationWithStateLicensing(uID);
      if (withStateLicensing != null)
      {
        if (withStateLicensing.Oid != this.currentOID)
          this.SetUseParentInfo(true);
        else
          this.SetUseParentInfo(false);
      }
      else if (uID != 0)
        this.SetUseParentInfo(true);
      if (this.currentOID == 0)
        this.SetUseParentInfo(false);
      this.chkUseParentInfo.Enabled = this.currentOID != 0;
      if (withStateLicensing == null)
        return;
      this.RefreshData(withStateLicensing.OrgBranchLicensing);
      this.ReloadStateView();
    }

    public BranchExtLicensing GetModifiedData()
    {
      BranchExtLicensing modifiedData = this.branchLicensing ?? new BranchExtLicensing(false, 0, "", "", "", "", false, "00", false, (List<StateLicenseExtType>) null, false, BranchLicensing.ATRSmallCreditors.None, BranchLicensing.ATRExemptCreditors.None);
      modifiedData.AllowLoansWithIssues = 0;
      modifiedData.MsgUploadNonApprovedLoans = "";
      modifiedData.LenderType = this.cboLenderType.SelectedIndex != -1 ? this.cboLenderType.SelectedItem.ToString() : "";
      modifiedData.HomeState = this.cboHomeState.SelectedIndex != -1 ? this.cboHomeState.SelectedItem.ToString() : "";
      modifiedData.OptOut = this.checkBoxOptOut.CheckState == CheckState.Checked ? "Y" : "N";
      modifiedData.StatutoryElectionInMaryland = !this.rdoMarylandNo.Checked;
      if (modifiedData.StatutoryElectionInMaryland)
      {
        if (this.rdoMarylandYes1.Checked)
          modifiedData.StatutoryElectionInMaryland2 = "10";
        else if (this.rdoMarylandYes2.Checked)
          modifiedData.StatutoryElectionInMaryland2 = "01";
        else
          modifiedData.StatutoryElectionInMaryland2 = "00";
      }
      else
        modifiedData.StatutoryElectionInMaryland2 = string.Empty;
      modifiedData.StatutoryElectionInKansas = this.rdoKansasYes.Checked;
      modifiedData.ATRExemptCreditor = this.cboExemptCreditor.SelectedIndex == -1 ? BranchLicensing.ATRExemptCreditors.None : (BranchLicensing.ATRExemptCreditors) this.cboExemptCreditor.SelectedIndex;
      modifiedData.ATRSmallCreditor = this.cboSmallCreditor.SelectedIndex == -1 ? BranchLicensing.ATRSmallCreditors.None : (BranchLicensing.ATRSmallCreditors) this.cboSmallCreditor.SelectedIndex;
      modifiedData.UseParentInfo = this.chkUseParentInfo.Checked;
      modifiedData.UseCustomLenderProfile = this.chkUseCustomLender.Checked;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewStates.Items)
      {
        if (gvItem.Tag != null)
        {
          bool flag = true;
          foreach (GVSubItem subItem in (IEnumerable<GVSubItem>) gvItem.SubItems)
          {
            if (subItem.Checked || subItem.Text.Trim() != "")
            {
              flag = false;
              break;
            }
          }
          if (!flag)
          {
            string licenseKey = MaventLicenseTypesUtils.GetLicenseKey(gvItem.SubItems[2].Text.Trim(), gvItem.SubItems[3].Text.Trim());
            foreach (StateLicenseExtType stateLicenseExtType in modifiedData.StateLicenseExtTypes)
            {
              if (stateLicenseExtType.LicenseType == licenseKey)
              {
                modifiedData.RemoveStateLicenseExtType(stateLicenseExtType);
                break;
              }
            }
            modifiedData.AddStateLicenseExtType(new StateLicenseExtType(gvItem.SubItems[2].Text, licenseKey, gvItem.SubItems[4].Text, gvItem.SubItems[5].Text != "" ? Convert.ToDateTime(gvItem.SubItems[5].Text) : DateTime.MinValue, gvItem.SubItems[6].Text != "" ? Convert.ToDateTime(gvItem.SubItems[6].Text) : DateTime.MinValue, gvItem.SubItems[7].Text != "" ? Convert.ToDateTime(gvItem.SubItems[7].Text) : DateTime.MinValue, gvItem.SubItems[8].Text, gvItem.SubItems[9].Text != "" ? Convert.ToDateTime(gvItem.SubItems[9].Text) : DateTime.MinValue, gvItem.SubItems[0].Checked, gvItem.SubItems[1].Checked, gvItem.SubItems[10].Text != "" ? Convert.ToDateTime(gvItem.SubItems[10].Text) : DateTime.MinValue, this.getSortIndex(gvItem.SubItems[2].Text, gvItem.SubItems[3].Text)));
          }
        }
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
      this.fieldChanged(sender, new EventArgs());
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

    private void gridViewStates_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index != 0 && e.SubItem.Index != 1)
        return;
      string stateAbbrevation = e.SubItem.Item.SubItems[2].Text.Trim();
      string licenseKey = MaventLicenseTypesUtils.GetLicenseKey(e.SubItem.Item.SubItems[2].Text.Trim(), e.SubItem.Item.SubItems[3].Text.Trim());
      if (!this.stateTable.ContainsKey((object) this.setStateKey(stateAbbrevation, licenseKey)))
        this.stateTable.Add((object) this.setStateKey(stateAbbrevation, licenseKey), (object) new StateLicenseExtType(stateAbbrevation, licenseKey, "", DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, "", DateTime.MinValue, false, false, DateTime.MinValue));
      StateLicenseExtType stateLicenseExtType = (StateLicenseExtType) this.stateTable[(object) this.setStateKey(stateAbbrevation, licenseKey)];
      if (e.SubItem.Index == 0)
      {
        stateLicenseExtType.Selected = e.SubItem.Checked;
        if (!stateLicenseExtType.Selected)
        {
          stateLicenseExtType.Exempt = false;
          e.SubItem.Item.SubItems[1].Checked = false;
          e.SubItem.Item.SubItems[1].CheckBoxVisible = false;
        }
        else
          e.SubItem.Item.SubItems[1].CheckBoxVisible = string.Compare(e.SubItem.Item.SubItems[3].Text, "No License Required") != 0;
      }
      else if (e.SubItem.Index == 1)
        stateLicenseExtType.Exempt = e.SubItem.Checked;
      e.SubItem.Item.Tag = (object) true;
      this.fieldChanged(source, new EventArgs());
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

    private void chkUseCustomLender_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkUseCustomLender.Checked && sender != null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "If you have not done so already, you must fill out a theory of lending questionnaire to use this feature. Please contact client care for further details.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      this.fieldChanged(sender, e);
    }

    private void gridViewStates_SizeChanged(object sender, EventArgs e)
    {
      this.gridViewStates.Columns[0].Width = (int) ((double) this.gridViewStates.Width * 0.135);
      this.gridViewStates.Columns[1].Width = (int) ((double) this.gridViewStates.Width * 0.595);
      this.gridViewStates.Columns[2].Width = (int) ((double) this.gridViewStates.Width * 0.135);
      this.gridViewStates.Columns[3].Width = (int) ((double) this.gridViewStates.Width * 0.135);
    }

    private void fieldChanged(object sender, EventArgs e)
    {
      if (this.DataChanged != null)
        this.DataChanged(sender, e);
      this.dirty = true;
    }

    private void disableArrows(bool value)
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

    private void gridViewStates_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboTypeState.Text == "All States")
        this.disableArrows(true);
      else if (this.gridViewStates.SelectedItems.Count == 0)
        this.disableArrows(true);
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
      this.fieldChanged(sender, e);
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
      this.fieldChanged(sender, e);
    }

    private void checkBoxOptOut_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBoxOptOut.CheckState == CheckState.Checked)
        this.cboHomeState.Enabled = false;
      else
        this.cboHomeState.Enabled = true;
      this.fieldChanged(sender, e);
    }

    private bool disableOptOut()
    {
      return this.cboLenderType.Text == "National Bank (FNB)" || this.cboLenderType.Text == "State Licensed Lender - Non Depository Institution (STLIC)" || this.cboLenderType.Text == string.Empty;
    }

    private string getMarylandElection2()
    {
      if (this.rdoMarylandYes1.Checked)
        return "10";
      return this.rdoMarylandYes2.Checked ? "01" : "00";
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
      this.groupContainer1 = new GroupContainer();
      this.groupContainer5 = new GroupContainer();
      this.cboExemptCreditor = new ComboBox();
      this.label6 = new Label();
      this.cboSmallCreditor = new ComboBox();
      this.label5 = new Label();
      this.panelTop2 = new Panel();
      this.checkBoxOptOut = new CheckBox();
      this.label1 = new Label();
      this.cboLenderType = new ComboBox();
      this.label2 = new Label();
      this.cboHomeState = new ComboBox();
      this.label3 = new Label();
      this.panelTop1 = new Panel();
      this.label4 = new Label();
      this.chkUseCustomLender = new CheckBox();
      this.chkUseParentInfo = new CheckBox();
      this.groupContainer4 = new GroupContainer();
      this.iconBtnHelpKansas = new IconButton();
      this.rdoKansasYes = new RadioButton();
      this.rdoKansasNo = new RadioButton();
      this.groupContainer3 = new GroupContainer();
      this.rdoMarylandYes2 = new RadioButton();
      this.rdoMarylandYes1 = new RadioButton();
      this.iconBtnHelpMaryland = new IconButton();
      this.rdoMarylandYes = new RadioButton();
      this.rdoMarylandNo = new RadioButton();
      this.groupContainer2 = new GroupContainer();
      this.gridViewStates = new GridView();
      this.btnMoveLicenseUp = new StandardIconButton();
      this.btnMoveLicenseDown = new StandardIconButton();
      this.cboTypeState = new ComboBox();
      this.groupContainer1.SuspendLayout();
      this.groupContainer5.SuspendLayout();
      this.panelTop2.SuspendLayout();
      this.panelTop1.SuspendLayout();
      this.groupContainer4.SuspendLayout();
      ((ISupportInitialize) this.iconBtnHelpKansas).BeginInit();
      this.groupContainer3.SuspendLayout();
      ((ISupportInitialize) this.iconBtnHelpMaryland).BeginInit();
      this.groupContainer2.SuspendLayout();
      ((ISupportInitialize) this.btnMoveLicenseUp).BeginInit();
      ((ISupportInitialize) this.btnMoveLicenseDown).BeginInit();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.groupContainer5);
      this.groupContainer1.Controls.Add((Control) this.panelTop2);
      this.groupContainer1.Controls.Add((Control) this.panelTop1);
      this.groupContainer1.Controls.Add((Control) this.chkUseParentInfo);
      this.groupContainer1.Controls.Add((Control) this.groupContainer4);
      this.groupContainer1.Controls.Add((Control) this.groupContainer3);
      this.groupContainer1.Controls.Add((Control) this.groupContainer2);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(588, 513);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "License";
      this.groupContainer5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer5.Controls.Add((Control) this.cboExemptCreditor);
      this.groupContainer5.Controls.Add((Control) this.label6);
      this.groupContainer5.Controls.Add((Control) this.cboSmallCreditor);
      this.groupContainer5.Controls.Add((Control) this.label5);
      this.groupContainer5.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer5.Location = new Point(5, 427);
      this.groupContainer5.Name = "groupContainer5";
      this.groupContainer5.Size = new Size(580, 82);
      this.groupContainer5.TabIndex = 14;
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
      this.cboExemptCreditor.Location = new Point(90, 55);
      this.cboExemptCreditor.Name = "cboExemptCreditor";
      this.cboExemptCreditor.Size = new Size(296, 21);
      this.cboExemptCreditor.TabIndex = 9;
      this.cboExemptCreditor.SelectedIndexChanged += new EventHandler(this.fieldChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(4, 58);
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
      this.cboSmallCreditor.Location = new Point(90, 31);
      this.cboSmallCreditor.Name = "cboSmallCreditor";
      this.cboSmallCreditor.Size = new Size(296, 21);
      this.cboSmallCreditor.TabIndex = 7;
      this.cboSmallCreditor.SelectedIndexChanged += new EventHandler(this.fieldChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(4, 34);
      this.label5.Name = "label5";
      this.label5.Size = new Size(71, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Small Creditor";
      this.panelTop2.Controls.Add((Control) this.checkBoxOptOut);
      this.panelTop2.Controls.Add((Control) this.label1);
      this.panelTop2.Controls.Add((Control) this.cboLenderType);
      this.panelTop2.Controls.Add((Control) this.label2);
      this.panelTop2.Controls.Add((Control) this.cboHomeState);
      this.panelTop2.Controls.Add((Control) this.label3);
      this.panelTop2.Dock = DockStyle.Top;
      this.panelTop2.Location = new Point(1, 46);
      this.panelTop2.Name = "panelTop2";
      this.panelTop2.Size = new Size(586, 80);
      this.panelTop2.TabIndex = 13;
      this.checkBoxOptOut.AutoSize = true;
      this.checkBoxOptOut.Location = new Point(9, 54);
      this.checkBoxOptOut.Name = "checkBoxOptOut";
      this.checkBoxOptOut.Size = new Size(296, 17);
      this.checkBoxOptOut.TabIndex = 6;
      this.checkBoxOptOut.Text = "My Company wishes to not apply interest rate exportation.";
      this.checkBoxOptOut.UseVisualStyleBackColor = true;
      this.checkBoxOptOut.CheckedChanged += new EventHandler(this.checkBoxOptOut_CheckedChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(67, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Lender Type";
      this.cboLenderType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLenderType.FormattingEnabled = true;
      this.cboLenderType.Location = new Point(81, 4);
      this.cboLenderType.Name = "cboLenderType";
      this.cboLenderType.Size = new Size(358, 21);
      this.cboLenderType.TabIndex = 1;
      this.cboLenderType.SelectedIndexChanged += new EventHandler(this.cboLenderType_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 30);
      this.label2.Name = "label2";
      this.label2.Size = new Size(63, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Home State";
      this.cboHomeState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboHomeState.FormattingEnabled = true;
      this.cboHomeState.Location = new Point(81, 27);
      this.cboHomeState.Name = "cboHomeState";
      this.cboHomeState.Size = new Size(109, 21);
      this.cboHomeState.TabIndex = 4;
      this.cboHomeState.SelectedIndexChanged += new EventHandler(this.fieldChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(196, 30);
      this.label3.Name = "label3";
      this.label3.Size = new Size(337, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "The interest rate exportation will be based on the selected home state.";
      this.panelTop1.Controls.Add((Control) this.label4);
      this.panelTop1.Controls.Add((Control) this.chkUseCustomLender);
      this.panelTop1.Dock = DockStyle.Top;
      this.panelTop1.Location = new Point(1, 26);
      this.panelTop1.Name = "panelTop1";
      this.panelTop1.Size = new Size(586, 20);
      this.panelTop1.TabIndex = 12;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 5);
      this.label4.Name = "label4";
      this.label4.Size = new Size(171, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Use Custom Lender Profile for ECS";
      this.chkUseCustomLender.AutoSize = true;
      this.chkUseCustomLender.Location = new Point(190, 5);
      this.chkUseCustomLender.Name = "chkUseCustomLender";
      this.chkUseCustomLender.Size = new Size(15, 14);
      this.chkUseCustomLender.TabIndex = 0;
      this.chkUseCustomLender.UseVisualStyleBackColor = true;
      this.chkUseCustomLender.CheckedChanged += new EventHandler(this.chkUseCustomLender_CheckedChanged);
      this.chkUseParentInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfo.AutoSize = true;
      this.chkUseParentInfo.BackColor = Color.Transparent;
      this.chkUseParentInfo.Location = new Point(482, 5);
      this.chkUseParentInfo.Name = "chkUseParentInfo";
      this.chkUseParentInfo.Size = new Size(100, 17);
      this.chkUseParentInfo.TabIndex = 9;
      this.chkUseParentInfo.Text = "Use Parent Info";
      this.chkUseParentInfo.UseVisualStyleBackColor = false;
      this.groupContainer4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.groupContainer4.Controls.Add((Control) this.iconBtnHelpKansas);
      this.groupContainer4.Controls.Add((Control) this.rdoKansasYes);
      this.groupContainer4.Controls.Add((Control) this.rdoKansasNo);
      this.groupContainer4.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer4.Location = new Point(325, 311);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(258, 71);
      this.groupContainer4.TabIndex = 8;
      this.groupContainer4.Text = "Statutory Election in Kansas";
      this.iconBtnHelpKansas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnHelpKansas.DisabledImage = (Image) null;
      this.iconBtnHelpKansas.Image = (Image) Resources.help;
      this.iconBtnHelpKansas.Location = new Point(238, 6);
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
      this.rdoKansasYes.TabIndex = 3;
      this.rdoKansasYes.TabStop = true;
      this.rdoKansasYes.Text = "Kansas UCCC Election For All Loans";
      this.rdoKansasYes.UseVisualStyleBackColor = true;
      this.rdoKansasYes.Click += new EventHandler(this.fieldChanged);
      this.rdoKansasNo.AutoSize = true;
      this.rdoKansasNo.Checked = true;
      this.rdoKansasNo.Location = new Point(16, 29);
      this.rdoKansasNo.Name = "rdoKansasNo";
      this.rdoKansasNo.Size = new Size(125, 17);
      this.rdoKansasNo.TabIndex = 2;
      this.rdoKansasNo.TabStop = true;
      this.rdoKansasNo.Text = "No Statutory Election";
      this.rdoKansasNo.UseVisualStyleBackColor = true;
      this.rdoKansasNo.Click += new EventHandler(this.fieldChanged);
      this.groupContainer3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupContainer3.Controls.Add((Control) this.rdoMarylandYes2);
      this.groupContainer3.Controls.Add((Control) this.rdoMarylandYes1);
      this.groupContainer3.Controls.Add((Control) this.iconBtnHelpMaryland);
      this.groupContainer3.Controls.Add((Control) this.rdoMarylandYes);
      this.groupContainer3.Controls.Add((Control) this.rdoMarylandNo);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(4, 311);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(315, 112);
      this.groupContainer3.TabIndex = 7;
      this.groupContainer3.Text = "Statutory Election in Maryland";
      this.rdoMarylandYes2.AutoSize = true;
      this.rdoMarylandYes2.Location = new Point(15, 87);
      this.rdoMarylandYes2.Name = "rdoMarylandYes2";
      this.rdoMarylandYes2.Size = new Size(235, 17);
      this.rdoMarylandYes2.TabIndex = 10;
      this.rdoMarylandYes2.Text = "Credit Grantor Law Election (for 1-4 unit only)";
      this.rdoMarylandYes2.UseVisualStyleBackColor = true;
      this.rdoMarylandYes2.Click += new EventHandler(this.fieldChanged);
      this.rdoMarylandYes1.AutoSize = true;
      this.rdoMarylandYes1.Location = new Point(15, 47);
      this.rdoMarylandYes1.Name = "rdoMarylandYes1";
      this.rdoMarylandYes1.Size = new Size(221, 17);
      this.rdoMarylandYes1.TabIndex = 9;
      this.rdoMarylandYes1.Text = "Credit Grantor Law Election (for All Loans)";
      this.rdoMarylandYes1.UseVisualStyleBackColor = true;
      this.rdoMarylandYes1.Click += new EventHandler(this.fieldChanged);
      this.iconBtnHelpMaryland.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnHelpMaryland.DisabledImage = (Image) null;
      this.iconBtnHelpMaryland.Image = (Image) Resources.help;
      this.iconBtnHelpMaryland.Location = new Point(295, 6);
      this.iconBtnHelpMaryland.MouseDownImage = (Image) null;
      this.iconBtnHelpMaryland.MouseOverImage = (Image) Resources.help_over;
      this.iconBtnHelpMaryland.Name = "iconBtnHelpMaryland";
      this.iconBtnHelpMaryland.Size = new Size(16, 16);
      this.iconBtnHelpMaryland.TabIndex = 2;
      this.iconBtnHelpMaryland.TabStop = false;
      this.iconBtnHelpMaryland.Click += new EventHandler(this.iconBtnHelpMaryland_Click);
      this.rdoMarylandYes.AutoSize = true;
      this.rdoMarylandYes.Location = new Point(15, 67);
      this.rdoMarylandYes.Name = "rdoMarylandYes";
      this.rdoMarylandYes.Size = new Size(277, 17);
      this.rdoMarylandYes.TabIndex = 1;
      this.rdoMarylandYes.TabStop = true;
      this.rdoMarylandYes.Text = "Credit Grantor Law Election (for 1-4 unit, Jr. Lien Only)";
      this.rdoMarylandYes.UseVisualStyleBackColor = true;
      this.rdoMarylandYes.Click += new EventHandler(this.fieldChanged);
      this.rdoMarylandNo.AutoSize = true;
      this.rdoMarylandNo.Checked = true;
      this.rdoMarylandNo.Location = new Point(15, 29);
      this.rdoMarylandNo.Name = "rdoMarylandNo";
      this.rdoMarylandNo.Size = new Size(125, 17);
      this.rdoMarylandNo.TabIndex = 0;
      this.rdoMarylandNo.TabStop = true;
      this.rdoMarylandNo.Text = "No Statutory Election";
      this.rdoMarylandNo.UseVisualStyleBackColor = true;
      this.rdoMarylandNo.Click += new EventHandler(this.fieldChanged);
      this.groupContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.gridViewStates);
      this.groupContainer2.Controls.Add((Control) this.btnMoveLicenseUp);
      this.groupContainer2.Controls.Add((Control) this.btnMoveLicenseDown);
      this.groupContainer2.Controls.Add((Control) this.cboTypeState);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(4, 130);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(580, 175);
      this.groupContainer2.TabIndex = 6;
      this.groupContainer2.Text = "License Type in ";
      this.gridViewStates.AllowMultiselect = false;
      this.gridViewStates.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnSelect";
      gvColumn1.Text = "Select";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 50;
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
      gvColumn4.Width = 100;
      gvColumn5.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnLicenseNo";
      gvColumn5.Text = "License #";
      gvColumn5.Width = 100;
      gvColumn6.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnIssueDate";
      gvColumn6.Text = "Issue Date";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 100;
      gvColumn7.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnStartDate";
      gvColumn7.Text = "Start Date";
      gvColumn7.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn7.Width = 100;
      gvColumn8.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnEndDate";
      gvColumn8.Text = "End Date";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 100;
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
      gvColumn10.Width = 100;
      gvColumn11.ActivatedEditorType = GVActivatedEditorType.DatePicker;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "ColumnLastCheck";
      gvColumn11.Text = "Last Checked";
      gvColumn11.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn11.Width = 100;
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
      this.gridViewStates.Size = new Size(578, 148);
      this.gridViewStates.SortIconVisible = false;
      this.gridViewStates.SortOption = GVSortOption.None;
      this.gridViewStates.TabIndex = 40;
      this.gridViewStates.SelectedIndexChanged += new EventHandler(this.gridViewStates_SelectedIndexChanged);
      this.gridViewStates.SubItemCheck += new GVSubItemEventHandler(this.gridViewStates_SubItemCheck);
      this.gridViewStates.EditorOpening += new GVSubItemEditingEventHandler(this.gridViewStates_EditorOpening);
      this.gridViewStates.EditorClosing += new GVSubItemEditingEventHandler(this.gridViewStates_EditorClosing);
      this.btnMoveLicenseUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveLicenseUp.BackColor = Color.Transparent;
      this.btnMoveLicenseUp.Enabled = false;
      this.btnMoveLicenseUp.Location = new Point(559, 4);
      this.btnMoveLicenseUp.MouseDownImage = (Image) null;
      this.btnMoveLicenseUp.Name = "btnMoveLicenseUp";
      this.btnMoveLicenseUp.Size = new Size(16, 17);
      this.btnMoveLicenseUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveLicenseUp.TabIndex = 39;
      this.btnMoveLicenseUp.TabStop = false;
      this.btnMoveLicenseUp.Click += new EventHandler(this.btnMoveLicenseUp_Click);
      this.btnMoveLicenseDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveLicenseDown.BackColor = Color.Transparent;
      this.btnMoveLicenseDown.Enabled = false;
      this.btnMoveLicenseDown.Location = new Point(537, 4);
      this.btnMoveLicenseDown.MouseDownImage = (Image) null;
      this.btnMoveLicenseDown.Name = "btnMoveLicenseDown";
      this.btnMoveLicenseDown.Size = new Size(16, 17);
      this.btnMoveLicenseDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveLicenseDown.TabIndex = 38;
      this.btnMoveLicenseDown.TabStop = false;
      this.btnMoveLicenseDown.Click += new EventHandler(this.btnMoveLicenseDown_Click);
      this.cboTypeState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTypeState.FormattingEnabled = true;
      this.cboTypeState.Location = new Point(113, 2);
      this.cboTypeState.Name = "cboTypeState";
      this.cboTypeState.Size = new Size(109, 21);
      this.cboTypeState.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (AddEditOrgLicenseControl);
      this.Size = new Size(588, 513);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.groupContainer5.ResumeLayout(false);
      this.groupContainer5.PerformLayout();
      this.panelTop2.ResumeLayout(false);
      this.panelTop2.PerformLayout();
      this.panelTop1.ResumeLayout(false);
      this.panelTop1.PerformLayout();
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      ((ISupportInitialize) this.iconBtnHelpKansas).EndInit();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      ((ISupportInitialize) this.iconBtnHelpMaryland).EndInit();
      this.groupContainer2.ResumeLayout(false);
      ((ISupportInitialize) this.btnMoveLicenseUp).EndInit();
      ((ISupportInitialize) this.btnMoveLicenseDown).EndInit();
      this.ResumeLayout(false);
    }
  }
}
