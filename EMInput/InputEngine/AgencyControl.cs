// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AgencyControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class AgencyControl : UserControl
  {
    private Sessions.Session session;
    private IHtmlInput inputData;
    private GenericFormInputHandler genericFormInputHandler;
    private List<KeyValuePair<string, string>> serviceList;
    private List<KeyValuePair<string, string>> languageList;
    private IContainer components;
    private GroupContainer groupContainer1;
    public TextBox txtDistance;
    private Label label7;
    public TextBox txtLatitude;
    private Label label5;
    public TextBox txtLongitude;
    private Label label6;
    public TextBox txtZipcode;
    private Label label4;
    private Label label3;
    public TextBox txtCity;
    private Label label2;
    public TextBox txtAddress;
    private Label label1;
    private Label label9;
    private Label label8;
    private PanelEx panelExLanguage;
    public GridView grdViewLanguages;
    private PanelEx panelExServuce;
    public GridView grdViewServices;
    private ToolTip toolTipField;
    public ComboBox cboState;

    public event AgencyControlEventHandler enableFindBtn;

    public AgencyControl(
      Sessions.Session session,
      IHtmlInput inputData,
      List<KeyValuePair<string, string>> serviceList,
      List<KeyValuePair<string, string>> languageList)
    {
      this.session = session;
      this.inputData = inputData;
      this.serviceList = serviceList;
      this.languageList = languageList;
      this.InitializeComponent();
      this.genericFormInputHandler = new GenericFormInputHandler(this.inputData, this.Controls, this.session);
      this.initForm();
    }

    private void initForm()
    {
      LoanData inputData = (LoanData) this.inputData;
      if (inputData != null && inputData.IsTemplate)
      {
        this.txtAddress.Enabled = false;
        this.txtCity.Enabled = false;
        this.txtZipcode.Enabled = false;
        this.cboState.Enabled = false;
      }
      this.cboState.Items.Clear();
      this.cboState.Items.Add((object) "");
      this.cboState.Items.AddRange((object[]) Utils.GetStates());
      if (this.getField("1825") == "2020" && this.getField("FR0129") == "Y")
      {
        this.groupContainer1.Text = "Subject Property Address";
        this.label1.Text = "   Subject Property Address";
        this.txtAddress.Tag = (object) "URLA.X73";
        this.txtCity.Tag = (object) "12";
        this.txtZipcode.Tag = (object) "15";
        this.cboState.Tag = (object) "14";
      }
      this.genericFormInputHandler.SetFieldValuesToForm();
      this.genericFormInputHandler.SetBusinessRules(new ResourceManager(typeof (IncomeOtherForm)));
      this.genericFormInputHandler.SetFieldTip(this.toolTipField);
      for (int index = 0; index < this.genericFormInputHandler.FieldControls.Count; ++index)
        this.genericFormInputHandler.SetFieldEvents(this.genericFormInputHandler.FieldControls[index]);
      this.genericFormInputHandler.FieldFormatChanged += new EventHandler(this.setFieldFormat);
      this.genericFormInputHandler.OnKeyUp += new EventHandler(this.calculateFields);
      this.refreshGeocode();
      this.txtDistance.Text = this.getField("HCSETTING.DISTANCE") == "" ? "500" : this.getField("HCSETTING.DISTANCE");
      this.refreshCodeList(this.grdViewServices, this.serviceList);
      this.refreshCodeList(this.grdViewLanguages, this.languageList);
    }

    private void calculateFields(object sender, EventArgs e)
    {
      if (!(sender is TextBox))
        return;
      TextBox textBox = (TextBox) sender;
      if (!(textBox.Name == "txtZipcode") || textBox.Text.Trim().Length < 5)
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(textBox.Text.Trim().Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(textBox.Text.Trim().Substring(0, 5)));
      if (zipCodeInfo != null)
      {
        this.txtCity.Text = Utils.CapsConvert(zipCodeInfo.City, false);
        this.cboState.Text = zipCodeInfo.State;
        this.refreshGeocode();
      }
      else
      {
        this.txtLatitude.Text = string.Empty;
        this.txtLongitude.Text = string.Empty;
      }
      this.textField_Changed(sender, e);
    }

    private void refreshGeocode()
    {
      GeoCoordinate zipGeoCoordinate = ZipCodeUtils.GetZipGeoCoordinate(this.txtZipcode.Text.Trim(), this.cboState.Text.Trim(), this.txtCity.Text.Trim(), string.Empty);
      if (!(zipGeoCoordinate != (GeoCoordinate) null))
        return;
      this.txtLatitude.Text = string.Concat((object) zipGeoCoordinate.Latitude);
      this.txtLongitude.Text = string.Concat((object) zipGeoCoordinate.Longitude);
    }

    private void textField_Changed(object sender, EventArgs e)
    {
      if (sender is TextBox && (TextBox) sender == this.txtDistance)
        this.setField("HCSETTING.DISTANCE", this.txtDistance.Text);
      this.enableFindBtnMainFormEV(sender, new AgencyControlEventArgs(false));
    }

    private void setFieldFormat(object sender, EventArgs e)
    {
      if (!(sender is TextBox))
        return;
      TextBox textBox = (TextBox) sender;
      if (textBox == this.txtDistance)
      {
        this.genericFormInputHandler.CurrentFieldFormat = FieldFormat.INTEGER;
      }
      else
      {
        if (textBox != this.txtLatitude && textBox != this.txtLongitude)
          return;
        this.genericFormInputHandler.CurrentFieldFormat = FieldFormat.DECIMAL_6;
      }
    }

    private void refreshCodeList(GridView view, List<KeyValuePair<string, string>> codeList)
    {
      bool flag = false;
      string[] settingFieldList = this.getParsedHCSettingFieldList(view);
      codeList.Sort((Comparison<KeyValuePair<string, string>>) ((x, y) => x.Value.CompareTo(y.Value)));
      view.BeginUpdate();
      for (int index = 0; index < codeList.Count; ++index)
      {
        GVItem gvItem = new GVItem(codeList[index].Value);
        gvItem.SubItems.Add((object) codeList[index].Key);
        if (settingFieldList.Length != 0)
        {
          if (((IEnumerable<string>) settingFieldList).Contains<string>(codeList[index].Key))
            gvItem.SubItems[0].Checked = true;
        }
        else if (view == this.grdViewLanguages && codeList[index].Key == "ENG")
          flag = true;
        gvItem.Tag = (object) codeList[index].Key;
        if (view == this.grdViewLanguages && codeList[index].Key == "ENG")
          view.Items.Insert(0, gvItem);
        else
          view.Items.Add(gvItem);
      }
      view.EndUpdate();
      if (!flag)
        return;
      view.Items[0].SubItems[0].Checked = true;
    }

    private string[] getParsedHCSettingFieldList(GridView view)
    {
      string str = "";
      if (view == this.grdViewServices)
        str = this.getField("HCSETTING.SERVICES");
      else if (view == this.grdViewLanguages)
        str = this.getField("HCSETTING.LANGUAGES");
      if (string.IsNullOrEmpty(str))
        return new string[0];
      return str.Split(',');
    }

    private void grdViewServices_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.enableFindBtnMainFormEV(source, new AgencyControlEventArgs(false));
      this.updateItemsFieldList(this.grdViewServices);
    }

    private void grdViewLanguages_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.enableFindBtnMainFormEV(source, new AgencyControlEventArgs(false));
      this.updateItemsFieldList(this.grdViewLanguages);
    }

    private void updateItemsFieldList(GridView view)
    {
      GVItemCollection items = view.Items;
      List<string> values = new List<string>();
      for (int nItemIndex = 0; nItemIndex < items.Count; ++nItemIndex)
      {
        GVItem gvItem = items[nItemIndex];
        if (gvItem.SubItems[0].Checked)
          values.Add(gvItem.SubItems[1].Text);
      }
      if (view == this.grdViewServices)
      {
        this.setField("HCSETTING.SERVICES", string.Join(",", (IEnumerable<string>) values));
      }
      else
      {
        if (view != this.grdViewLanguages)
          return;
        this.setField("HCSETTING.LANGUAGES", string.Join(",", (IEnumerable<string>) values));
      }
    }

    private void enableFindBtnMainFormEV(object source, AgencyControlEventArgs e)
    {
      e.isFindBtnEnabled = this.txtLongitude.Text.Trim() != string.Empty && this.txtLatitude.Text.Trim() != string.Empty && this.txtDistance.Text.Trim() != string.Empty && this.grdViewServices.GetCheckedItems(0).Length != 0 && this.grdViewLanguages.GetCheckedItems(0).Length != 0;
      if (this.enableFindBtn == null)
        return;
      this.enableFindBtn(source, e);
    }

    private string getField(string fieldId)
    {
      return this.inputData != null ? this.inputData.GetField(fieldId) : "";
    }

    private void setField(string fieldId, string value)
    {
      if (this.inputData == null)
        return;
      this.inputData.SetField(fieldId, value);
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
      this.toolTipField = new ToolTip(this.components);
      this.groupContainer1 = new GroupContainer();
      this.cboState = new ComboBox();
      this.panelExLanguage = new PanelEx();
      this.grdViewLanguages = new GridView();
      this.panelExServuce = new PanelEx();
      this.grdViewServices = new GridView();
      this.label9 = new Label();
      this.label8 = new Label();
      this.txtDistance = new TextBox();
      this.label7 = new Label();
      this.txtLatitude = new TextBox();
      this.label5 = new Label();
      this.txtLongitude = new TextBox();
      this.label6 = new Label();
      this.txtZipcode = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.txtCity = new TextBox();
      this.label2 = new Label();
      this.txtAddress = new TextBox();
      this.label1 = new Label();
      this.groupContainer1.SuspendLayout();
      this.panelExLanguage.SuspendLayout();
      this.panelExServuce.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.cboState);
      this.groupContainer1.Controls.Add((Control) this.panelExLanguage);
      this.groupContainer1.Controls.Add((Control) this.panelExServuce);
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.label8);
      this.groupContainer1.Controls.Add((Control) this.txtDistance);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.txtLatitude);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.txtLongitude);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.txtZipcode);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.txtCity);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.txtAddress);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(642, 253);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Borrower's Address";
      this.cboState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboState.FormattingEnabled = true;
      this.cboState.Location = new Point(148, 85);
      this.cboState.Name = "cboState";
      this.cboState.Size = new Size(55, 21);
      this.cboState.TabIndex = 21;
      this.cboState.Tag = (object) "FR0107";
      this.panelExLanguage.BorderStyle = BorderStyle.FixedSingle;
      this.panelExLanguage.Controls.Add((Control) this.grdViewLanguages);
      this.panelExLanguage.Location = new Point(329, 126);
      this.panelExLanguage.Name = "panelExLanguage";
      this.panelExLanguage.Size = new Size(304, 123);
      this.panelExLanguage.TabIndex = 20;
      this.grdViewLanguages.AllowMultiselect = false;
      this.grdViewLanguages.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnName";
      gvColumn1.Text = "Language";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnKey";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Language Key";
      gvColumn2.Width = 102;
      this.grdViewLanguages.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.grdViewLanguages.Dock = DockStyle.Fill;
      this.grdViewLanguages.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdViewLanguages.Location = new Point(0, 0);
      this.grdViewLanguages.Name = "grdViewLanguages";
      this.grdViewLanguages.Size = new Size(302, 121);
      this.grdViewLanguages.SortOption = GVSortOption.None;
      this.grdViewLanguages.TabIndex = 1;
      this.grdViewLanguages.SubItemCheck += new GVSubItemEventHandler(this.grdViewLanguages_SubItemCheck);
      this.panelExServuce.BorderStyle = BorderStyle.FixedSingle;
      this.panelExServuce.Controls.Add((Control) this.grdViewServices);
      this.panelExServuce.Location = new Point(9, 126);
      this.panelExServuce.Name = "panelExServuce";
      this.panelExServuce.Size = new Size(304, 123);
      this.panelExServuce.TabIndex = 19;
      this.grdViewServices.AllowMultiselect = false;
      this.grdViewServices.BorderStyle = BorderStyle.None;
      gvColumn3.CheckBoxes = true;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnName";
      gvColumn3.Text = "Service Name";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnKey";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Service Key";
      gvColumn4.Width = 102;
      this.grdViewServices.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.grdViewServices.Dock = DockStyle.Fill;
      this.grdViewServices.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdViewServices.Location = new Point(0, 0);
      this.grdViewServices.Name = "grdViewServices";
      this.grdViewServices.Size = new Size(302, 121);
      this.grdViewServices.SortOption = GVSortOption.None;
      this.grdViewServices.TabIndex = 1;
      this.grdViewServices.SubItemCheck += new GVSubItemEventHandler(this.grdViewServices_SubItemCheck);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(326, 110);
      this.label9.Name = "label9";
      this.label9.Size = new Size(99, 13);
      this.label9.TabIndex = 16;
      this.label9.Text = "Multiple Languages";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(6, 110);
      this.label8.Name = "label8";
      this.label8.Size = new Size(87, 13);
      this.label8.TabIndex = 14;
      this.label8.Text = "Multiple Services";
      this.txtDistance.Location = new Point(509, 85);
      this.txtDistance.Name = "txtDistance";
      this.txtDistance.Size = new Size(105, 20);
      this.txtDistance.TabIndex = 7;
      this.txtDistance.TextAlign = HorizontalAlignment.Right;
      this.txtDistance.TextChanged += new EventHandler(this.textField_Changed);
      this.txtDistance.Tag = (object) "HCSETTING.DISTANCE";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(454, 88);
      this.label7.Name = "label7";
      this.label7.Size = new Size(49, 13);
      this.label7.TabIndex = 12;
      this.label7.Text = "Distance";
      this.txtLatitude.Location = new Point(509, 63);
      this.txtLatitude.Name = "txtLatitude";
      this.txtLatitude.ReadOnly = true;
      this.txtLatitude.Size = new Size(105, 20);
      this.txtLatitude.TabIndex = 6;
      this.txtLatitude.TabStop = false;
      this.txtLatitude.TextAlign = HorizontalAlignment.Right;
      this.txtLatitude.TextChanged += new EventHandler(this.textField_Changed);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(458, 66);
      this.label5.Name = "label5";
      this.label5.Size = new Size(45, 13);
      this.label5.TabIndex = 10;
      this.label5.Text = "Latitude";
      this.txtLongitude.Location = new Point(509, 41);
      this.txtLongitude.Name = "txtLongitude";
      this.txtLongitude.ReadOnly = true;
      this.txtLongitude.Size = new Size(105, 20);
      this.txtLongitude.TabIndex = 5;
      this.txtLongitude.TabStop = false;
      this.txtLongitude.TextAlign = HorizontalAlignment.Right;
      this.txtLongitude.TextChanged += new EventHandler(this.textField_Changed);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(449, 44);
      this.label6.Name = "label6";
      this.label6.Size = new Size(54, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "Longitude";
      this.txtZipcode.Location = new Point(273, 85);
      this.txtZipcode.Name = "txtZipcode";
      this.txtZipcode.Size = new Size(125, 20);
      this.txtZipcode.TabIndex = 4;
      this.txtZipcode.Tag = (object) "FR0108";
      this.txtZipcode.TextChanged += new EventHandler(this.textField_Changed);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(221, 88);
      this.label4.Name = "label4";
      this.label4.Size = new Size(46, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Zipcode";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(110, 88);
      this.label3.Name = "label3";
      this.label3.Size = new Size(32, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "State";
      this.txtCity.Location = new Point(148, 63);
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size(250, 20);
      this.txtCity.TabIndex = 2;
      this.txtCity.Tag = (object) "FR0106";
      this.txtCity.TabIndexChanged += new EventHandler(this.textField_Changed);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(118, 66);
      this.label2.Name = "label2";
      this.label2.Size = new Size(24, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "City";
      this.txtAddress.Location = new Point(148, 41);
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = new Size(250, 20);
      this.txtAddress.TabIndex = 1;
      this.txtAddress.Tag = (object) "FR0104";
      this.txtAddress.TabIndexChanged += new EventHandler(this.textField_Changed);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 44);
      this.label1.Name = "label1";
      this.label1.Size = new Size(136, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Borrower's Present Address";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (AgencyControl);
      this.Size = new Size(642, 253);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.panelExLanguage.ResumeLayout(false);
      this.panelExServuce.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
