// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.GetAgenciesForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class GetAgenciesForm : Form
  {
    private const string className = "GetAgenciesForm";
    protected static string sw = Tracing.SwInputEngine;
    private Sessions.Session session;
    private IHtmlInput inputData;
    private GenericFormInputHandler genericFormInputHandler;
    private List<string> existingAgencyIDs;
    private List<KeyValuePair<string, string>> serviceList;
    private List<KeyValuePair<string, string>> languageList;
    private AgencyControl agencyForm;
    private List<string[]> importedAgencies;
    private IContainer components;
    private Button btnCancel;
    private Button btnFind;
    private GroupContainer groupContainer1;
    private GridView grdAgencies;
    private Button btnSave;
    private GroupContainer groupList;
    private ToolTip toolTipField;

    public GetAgenciesForm(
      Sessions.Session session,
      IHtmlInput inputData,
      List<KeyValuePair<string, string>> serviceList,
      List<KeyValuePair<string, string>> languageList,
      List<string> existingAgencyIDs)
    {
      this.session = session;
      this.inputData = inputData;
      this.serviceList = serviceList;
      this.languageList = languageList;
      this.existingAgencyIDs = existingAgencyIDs;
      this.InitializeComponent();
      this.agencyForm = new AgencyControl(session, inputData, serviceList, languageList);
      this.groupContainer1.Controls.Add((Control) this.agencyForm);
      this.agencyForm.enableFindBtn += new AgencyControlEventHandler(this.findBtn_Enable);
      if (!this.isFindButtonEnabled())
        this.btnFind.Enabled = false;
      this.genericFormInputHandler = new GenericFormInputHandler(this.inputData, this.Controls, this.session);
      this.initForm();
      if (this.btnFind.Enabled)
        this.AcceptButton = (IButtonControl) this.btnFind;
      this.btnSave.Enabled = this.grdAgencies.Items.Count > 0;
    }

    private void initForm()
    {
      this.genericFormInputHandler.SetFieldValuesToForm();
      this.genericFormInputHandler.SetBusinessRules(new ResourceManager(typeof (IncomeOtherForm)));
      this.genericFormInputHandler.SetFieldTip(this.toolTipField);
      for (int index = 0; index < this.genericFormInputHandler.FieldControls.Count; ++index)
        this.genericFormInputHandler.SetFieldEvents(this.genericFormInputHandler.FieldControls[index]);
    }

    private void refreshGeocode()
    {
      GeoCoordinate zipGeoCoordinate = ZipCodeUtils.GetZipGeoCoordinate(this.agencyForm.txtZipcode.Text.Trim(), this.agencyForm.cboState.Text.Trim(), this.agencyForm.txtCity.Text.Trim(), string.Empty);
      if (!(zipGeoCoordinate != (GeoCoordinate) null))
        return;
      this.agencyForm.txtLatitude.Text = string.Concat((object) zipGeoCoordinate.Latitude);
      this.agencyForm.txtLongitude.Text = string.Concat((object) zipGeoCoordinate.Longitude);
    }

    private void btnFind_Click(object sender, EventArgs e)
    {
      this.btnFind.Enabled = false;
      Cursor.Current = Cursors.WaitCursor;
      this.grdAgencies.BeginUpdate();
      this.grdAgencies.Items.Clear();
      this.grdAgencies.EndUpdate();
      bool validGeo = false;
      string homeCounselingUrl = this.session?.LoanData?.GetHomeCounselingUrl(this.agencyForm.txtZipcode.Text.Trim(), this.agencyForm.cboState.Text.Trim(), this.agencyForm.txtCity.Text.Trim(), out validGeo);
      if (!validGeo)
        return;
      List<List<string[]>> counselingResults = this.session.SessionObjects.ParseHomeCounselingResults(this.session.SessionObjects.GetHomeCounseling(homeCounselingUrl, (IWin32Window) this.session.MainForm));
      if (counselingResults != null && counselingResults.Count > 0)
      {
        double longitude1 = Utils.ParseDouble((object) this.agencyForm.txtLongitude.Text.Trim());
        double latitude1 = Utils.ParseDouble((object) this.agencyForm.txtLatitude.Text.Trim());
        double longitude2 = -1.0;
        double latitude2 = -1.0;
        GeoCoordinate geoCoordinate = (GeoCoordinate) null;
        try
        {
          geoCoordinate = new GeoCoordinate(latitude1, longitude1);
        }
        catch (Exception ex)
        {
          Tracing.Log(GetAgenciesForm.sw, nameof (GetAgenciesForm), TraceLevel.Error, ex.ToString());
        }
        this.grdAgencies.BeginUpdate();
        this.grdAgencies.Items.Clear();
        foreach (List<string[]> strArrayList in counselingResults)
        {
          GVItem gvItem = new GVItem();
          for (int index = 1; index < this.grdAgencies.Columns.Count; ++index)
            gvItem.SubItems.Add((object) "");
          foreach (string[] strArray in strArrayList)
          {
            if (string.Compare(strArray[0], "agc_ADDR_LATITUDE", true) == 0)
              latitude2 = Utils.ParseDouble((object) strArray[1]);
            else if (string.Compare(strArray[0], "agc_ADDR_LONGITUDE", true) == 0)
              longitude2 = Utils.ParseDouble((object) strArray[1]);
            this.addFieldValueToGrid(strArray[0], strArray[1], gvItem);
          }
          this.addFieldValueToGrid("distance", geoCoordinate != (GeoCoordinate) null ? string.Concat((object) Utils.ArithmeticRounding(geoCoordinate.GetDistanceTo(new GeoCoordinate(latitude2, longitude2)) / 1609.34, 2)) : "", gvItem);
          this.grdAgencies.Items.Add(gvItem);
        }
        this.grdAgencies.Sort(5, SortOrder.Ascending);
        for (int nItemIndex = 0; nItemIndex < this.grdAgencies.Items.Count; ++nItemIndex)
        {
          this.grdAgencies.Items[nItemIndex].SubItems[0].Checked = true;
          if (nItemIndex >= 9)
            break;
        }
        this.grdAgencies.EndUpdate();
      }
      else
      {
        this.grdAgencies.BeginUpdate();
        this.grdAgencies.Items.Clear();
        this.grdAgencies.EndUpdate();
      }
      this.groupList.Text = "Home Counselor list (" + (object) this.grdAgencies.Items.Count + ")";
      this.grdAgencies_SubItemCheck((object) null, (GVSubItemEventArgs) null);
      if (this.grdAgencies.Items.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Cannot find any Home Counselor in " + this.agencyForm.txtDistance.Text.Trim() + " miles. Please increase the distance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.grdAgencies.Items.Count < 10)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Less than 10 agencies have been located. Please expand the distance, services or languages selected to find more counselors.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.btnFind.Enabled = true;
      Cursor.Current = Cursors.Default;
    }

    private void addFieldValueToGrid(string id, string val, GVItem item)
    {
      foreach (GVColumn column in this.grdAgencies.Columns)
      {
        if (string.Compare(column.Name, id, true) == 0)
        {
          if (string.Compare(column.Name, "services", true) == 0 || string.Compare(column.Name, "languages", true) == 0)
            val = this != null ? this.session?.SessionObjects?.translateServiceLanguageCodes(val, string.Compare(column.Name, "services", true) == 0, this.serviceList, this.languageList) : (string) null;
          else if (string.Compare(column.Name, "email", true) == 0 && (string.IsNullOrEmpty(val) || string.Compare(val, "N/A", true) == 0) || string.Compare(column.Name, "weburl", true) == 0 && (string.IsNullOrEmpty(val) || string.Compare(val, "N/A", true) == 0))
            val = "Not Available";
          else if (string.Compare(column.Name, "city", true) == 0 && !string.IsNullOrEmpty(val))
            val = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(val.ToLower());
          item.SubItems[column.Index].Text = val;
          break;
        }
      }
    }

    private string findFieldValueFromGrid(string id, GVItem item)
    {
      foreach (GVColumn column in this.grdAgencies.Columns)
      {
        if (string.Compare(column.Name, id, true) == 0)
          return item.SubItems[column.Index].Text;
      }
      return string.Empty;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.existingAgencyIDs != null && this.existingAgencyIDs.Count > 0)
      {
        string str = string.Empty;
        for (int j = 0; j < this.grdAgencies.Items.Count; ++j)
        {
          if (!string.IsNullOrEmpty(this.existingAgencyIDs.FirstOrDefault<string>((Func<string, bool>) (x => x == this.findFieldValueFromGrid("agcid", this.grdAgencies.Items[j])))))
            str = str + (str != string.Empty ? ",\r\n" : "") + (this.findFieldValueFromGrid("nme", this.grdAgencies.Items[j]) != string.Empty ? this.findFieldValueFromGrid("nme", this.grdAgencies.Items[j]) : this.findFieldValueFromGrid("agcid", this.grdAgencies.Items[j]));
        }
        if (!string.IsNullOrEmpty(str) && Utils.Dialog((IWin32Window) this, "The current Home Counseling List already contains the following record(s):\r\n\r\n" + str + "\r\n\r\nThese record(s) will be updated as well. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
      }
      this.importedAgencies = new List<string[]>();
      List<string> stringList1 = new List<string>();
      for (int nColumnIndex = 0; nColumnIndex < this.grdAgencies.Columns.Count; ++nColumnIndex)
        stringList1.Add(this.grdAgencies.Columns[nColumnIndex].Name);
      this.importedAgencies.Add(stringList1.ToArray());
      for (int nItemIndex1 = 0; nItemIndex1 < this.grdAgencies.Items.Count; ++nItemIndex1)
      {
        if (this.grdAgencies.Items[nItemIndex1].SubItems[0].Checked)
        {
          List<string> stringList2 = new List<string>();
          for (int nItemIndex2 = 0; nItemIndex2 < this.grdAgencies.Columns.Count; ++nItemIndex2)
            stringList2.Add(this.grdAgencies.Items[nItemIndex1].SubItems[nItemIndex2].Text);
          this.importedAgencies.Add(stringList2.ToArray());
        }
      }
      this.genericFormInputHandler.SetFieldValuesToLoan();
      this.DialogResult = DialogResult.OK;
    }

    public List<string[]> ImportedAgencies => this.importedAgencies;

    private void grdAgencies_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.btnSave.Enabled = this.grdAgencies.GetCheckedItems(0).Length >= 10;
    }

    private void findBtn_Enable(object source, AgencyControlEventArgs e)
    {
      this.btnFind.Enabled = e.isFindBtnEnabled;
    }

    private bool isFindButtonEnabled()
    {
      return this.agencyForm.txtLongitude.Text.Trim() != string.Empty && this.agencyForm.txtLatitude.Text.Trim() != string.Empty && this.agencyForm.txtDistance.Text.Trim() != string.Empty && this.agencyForm.grdViewServices.GetCheckedItems(0).Length != 0 && this.agencyForm.grdViewLanguages.GetCheckedItems(0).Length != 0;
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
      this.btnCancel = new Button();
      this.btnFind = new Button();
      this.btnSave = new Button();
      this.toolTipField = new ToolTip(this.components);
      this.groupList = new GroupContainer();
      this.grdAgencies = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.groupList.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(579, 537);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnFind.Location = new Point(562, 271);
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = new Size(91, 23);
      this.btnFind.TabIndex = 1;
      this.btnFind.Text = "&Get Agencies";
      this.btnFind.UseVisualStyleBackColor = true;
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.btnSave.Location = new Point(498, 537);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.groupList.Controls.Add((Control) this.grdAgencies);
      this.groupList.HeaderForeColor = SystemColors.ControlText;
      this.groupList.Location = new Point(12, 300);
      this.groupList.Name = "groupList";
      this.groupList.Size = new Size(642, 231);
      this.groupList.TabIndex = 5;
      this.groupList.Text = "Home Counselor List";
      this.grdAgencies.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "agcid";
      gvColumn1.Text = "Agency ID";
      gvColumn1.Width = 90;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "adr1";
      gvColumn2.Text = "Address";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "city";
      gvColumn3.Text = "City";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "statecd";
      gvColumn4.Text = "State";
      gvColumn4.Width = 40;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "zipcd";
      gvColumn5.Text = "Zip";
      gvColumn5.Width = 80;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "distance";
      gvColumn6.SortMethod = GVSortMethod.Numeric;
      gvColumn6.Text = "Distance (Miles)";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 105;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "phone1";
      gvColumn7.Text = "Phone";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "weburl";
      gvColumn8.Text = "Fax";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "email";
      gvColumn9.Text = "Email";
      gvColumn9.Width = 100;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "nme";
      gvColumn10.Text = "Agency Name";
      gvColumn10.Width = 120;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "services";
      gvColumn11.Text = "Services Supported";
      gvColumn11.Width = 300;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "languages";
      gvColumn12.Text = "Languages Supported";
      gvColumn12.Width = 200;
      this.grdAgencies.Columns.AddRange(new GVColumn[12]
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
        gvColumn11,
        gvColumn12
      });
      this.grdAgencies.Dock = DockStyle.Fill;
      this.grdAgencies.Location = new Point(1, 26);
      this.grdAgencies.Name = "grdAgencies";
      this.grdAgencies.Size = new Size(640, 204);
      this.grdAgencies.TabIndex = 3;
      this.grdAgencies.SubItemCheck += new GVSubItemEventHandler(this.grdAgencies_SubItemCheck);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(642, 253);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Borrower's Address";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(666, 571);
      this.Controls.Add((Control) this.groupList);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnFind);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (GetAgenciesForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Get Home Counseling Agencies";
      this.groupList.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
