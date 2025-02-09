// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.FieldFilterEditor
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class FieldFilterEditor : UserControl
  {
    private FieldFilter currentFilter = new FieldFilter();
    private bool allowDynamicOperators = true;
    private bool allowVirtualFields = true;
    private ReportFieldDefs fieldDefs;
    private ReportFieldDef currentField;
    private OperatorTypesEnumNameProvider opTypesNameProvider = new OperatorTypesEnumNameProvider();
    private EllieMae.EMLite.ClientServer.Reporting.FieldTypes currentFilterFieldTypes = EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNothing;
    private IButtonControl defaultAcceptButton;
    private IContainer components;
    private TextBox txtStringValue;
    private ComboBox cboOperator;
    private DatePicker dtMaxDate;
    private DatePicker dtMinDate;
    private Label lblMinNumber;
    private Label lblAndDate;
    private TextBox txtDescription;
    private TextBox txtField;
    private Label label10;
    private Label label2;
    private Label lblMaxDate;
    private Label lblMinDate;
    private Label label3;
    private Label lblMaxNumber;
    private Label lblAndNumber;
    private CheckedListBox lstOptions;
    private TextBox txtMaxNumber;
    private Label label1;
    private Panel pnlOptions;
    private Label lblOptions;
    private Panel pnlText;
    private Panel pnlDate;
    private TextBox txtMinNumber;
    private Panel pnlNumber;
    private StandardIconButton btnFieldList;
    private ToolTip toolTip1;
    private Panel pnlOptionBase;
    private Panel pnlDateOperatorOption;
    private Panel pnlOperator;
    private CheckBox chkRecurring;
    private Panel panel2;

    public bool DDMSetting { get; set; }

    public FieldFilterEditor()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtMinNumber, TextBoxContentRule.Decimal);
      TextBoxFormatter.Attach(this.txtMaxNumber, TextBoxContentRule.Decimal);
      this.setControlState(false);
    }

    [Browsable(false)]
    public ReportFieldDefs FieldDefinitions
    {
      get => this.fieldDefs;
      set => this.fieldDefs = value;
    }

    [Browsable(false)]
    public FieldFilter CurrentFilter
    {
      get => this.currentFilter;
      set
      {
        if (this.DesignMode)
          return;
        if (value == null)
          value = new FieldFilter();
        this.currentFilter = value;
        this.loadFilterData();
      }
    }

    public bool AllowDatabaseFieldsOnly
    {
      get => this.txtField.ReadOnly;
      set => this.txtField.ReadOnly = value;
    }

    public bool AllowDynamicOperators
    {
      get => this.allowDynamicOperators;
      set => this.allowDynamicOperators = value;
    }

    public bool AllowVirtualFields
    {
      get => this.allowVirtualFields;
      set => this.allowVirtualFields = value;
    }

    private EllieMae.EMLite.ClientServer.Reporting.FieldTypes displayFieldType
    {
      get
      {
        if (!this.pnlDateOperatorOption.Visible)
          return this.currentFilterFieldTypes;
        if (this.chkRecurring.Checked)
          return EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay;
        return this.currentField != null ? this.currentField.FieldType : EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate;
      }
      set => this.currentFilterFieldTypes = value;
    }

    private void loadFilterData()
    {
      if (this.fieldDefs == null)
        return;
      this.displayFieldType = this.currentFilter.FieldType;
      ReportFieldDef selectedDef = this.fieldDefs.GetFieldByID(this.currentFilter.FieldID);
      if (selectedDef == null && (this.currentFilter.CriterionName ?? "") != "")
        selectedDef = this.fieldDefs.GetFieldByCriterionName(this.currentFilter.CriterionName);
      if (selectedDef == null || this.DDMSetting)
      {
        ILoanManager loanManager = (ILoanManager) null;
        FieldDefinition fieldDef = this.DDMSetting ? DDM_FieldAccess_Utils.GetFieldDefinition(this.currentFilter.FieldID, loanManager) : EncompassFields.GetField(this.currentFilter.FieldID);
        if (fieldDef == null)
        {
          if (selectedDef == null)
            return;
        }
        else
          selectedDef = this.fieldDefs.CreateReportFieldDef(Session.DefaultInstance, fieldDef);
      }
      this.loadField(selectedDef, true);
      this.txtDescription.Text = this.currentFilter.FieldDescription;
      ClientCommonUtils.PopulateDropdown(this.cboOperator, (object) this.currentFilter.OperatorTypeAsString, false);
      if (this.currentFilter.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate || this.currentFilter.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay || this.currentFilter.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime)
        this.loadDateValueData();
      else if (this.currentFilter.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric)
        this.loadNumericValueData();
      else if (this.currentFilter.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList)
        this.loadOptionListData();
      else
        this.loadTextValueData();
      if (this.cboOperator.SelectedIndex != -1)
        return;
      this.cboOperator.SelectedIndex = 0;
    }

    private void loadDateValueData()
    {
      if (Utils.IsDate((object) this.currentFilter.ValueFrom))
        this.dtMinDate.Value = this.normalizeDate(Utils.ParseDate((object) this.currentFilter.ValueFrom));
      if (!Utils.IsDate((object) this.currentFilter.ValueTo))
        return;
      this.dtMaxDate.Value = this.normalizeDate(Utils.ParseDate((object) this.currentFilter.ValueTo));
    }

    private void loadNumericValueData()
    {
      this.txtMinNumber.Text = this.currentFilter.ValueFrom;
      this.txtMaxNumber.Text = this.currentFilter.ValueTo;
    }

    private void loadTextValueData() => this.txtStringValue.Text = this.currentFilter.ValueFrom;

    private void loadOptionListData()
    {
      foreach (string options in this.currentFilter.GetOptionsList())
      {
        for (int index = 0; index < this.lstOptions.Items.Count; ++index)
        {
          if (((FieldOption) this.lstOptions.Items[index]).ReportingDatabaseValue == options)
            this.lstOptions.SetItemChecked(index, true);
        }
      }
    }

    private void btnFieldList_Click(object sender, EventArgs e)
    {
      using (ReportFieldSelector reportFieldSelector = new ReportFieldSelector(this.fieldDefs, this.AllowDatabaseFieldsOnly, this.AllowVirtualFields))
      {
        if (reportFieldSelector.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (this.currentFilter == null)
          this.currentFilterFieldTypes = EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNothing;
        this.loadField(reportFieldSelector.SelectedField, false);
      }
    }

    private void loadField(ReportFieldDef selectedDef, bool fromFilter)
    {
      EllieMae.EMLite.ClientServer.Reporting.FieldTypes fieldType = fromFilter ? this.displayFieldType : selectedDef.FieldType;
      bool flag = fromFilter || this.currentField == null || selectedDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList;
      if (!fromFilter)
      {
        if (this.displayFieldType != selectedDef.FieldType)
          flag = true;
        this.displayFieldType = selectedDef.FieldType;
      }
      this.currentField = selectedDef;
      this.txtField.Text = selectedDef.FieldID;
      this.txtDescription.Text = selectedDef.Description;
      if (flag)
      {
        this.loadOperatorsForFieldType(fieldType);
        switch (fieldType)
        {
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate:
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime:
            this.initDateFields();
            break;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay:
            this.initMonthDayFields();
            break;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList:
            this.loadOptions(selectedDef.FieldDefinition);
            break;
        }
        this.pnlOptions.Visible = fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList;
        this.pnlNumber.Visible = fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric;
        this.pnlText.Visible = fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString || fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsPhone;
        Panel pnlDate = this.pnlDate;
        int num;
        switch (fieldType)
        {
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate:
            num = 1;
            break;
          case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay:
            num = 1;
            break;
          default:
            num = fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime ? 1 : 0;
            break;
        }
        pnlDate.Visible = num != 0;
        this.pnlDateOperatorOption.Visible = fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate || fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay && selectedDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate || fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime || fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay && selectedDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime;
        if (fieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay && (selectedDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate || selectedDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime))
          this.chkRecurring.Checked = true;
        this.cboOperator.SelectedIndex = 0;
      }
      this.setControlState(true);
    }

    private void initMonthDayFields()
    {
      this.dtMinDate.CustomFormat = this.dtMaxDate.CustomFormat = "MM/dd";
      this.dtMinDate.Value = new DateTime(2000, DateTime.Today.Month, 1);
      DatePicker dtMaxDate = this.dtMaxDate;
      DateTime today = DateTime.Today;
      int month = today.Month;
      today = DateTime.Today;
      int day = DateTime.DaysInMonth(2000, today.Month);
      DateTime dateTime = new DateTime(2000, month, day);
      dtMaxDate.Value = dateTime;
    }

    private void initDateFields()
    {
      this.chkRecurring.CheckedChanged -= new EventHandler(this.chkRecurring_CheckedChanged);
      this.chkRecurring.Checked = false;
      this.chkRecurring.CheckedChanged += new EventHandler(this.chkRecurring_CheckedChanged);
      this.dtMinDate.CustomFormat = this.dtMaxDate.CustomFormat = "MM/dd/yyyy";
      this.dtMinDate.Value = DateTime.Today;
      this.dtMaxDate.Value = DateTime.Today.AddMonths(1);
    }

    private void loadOperatorsForFieldType(EllieMae.EMLite.ClientServer.Reporting.FieldTypes fieldType)
    {
      OperatorTypesEnumNameProvider enumNameProvider = new OperatorTypesEnumNameProvider();
      this.cboOperator.Items.Clear();
      foreach (OperatorTypes op in FieldTypeUtilities.GetOperatorTypesForFieldType(fieldType))
      {
        if (this.allowDynamicOperators || !enumNameProvider.IsDynamic(op))
          this.cboOperator.Items.Add((object) enumNameProvider.GetName((object) op));
      }
    }

    private void loadOptions(FieldDefinition fieldDef)
    {
      this.lstOptions.Items.Clear();
      switch (fieldDef.Format)
      {
        case FieldFormat.YN:
          this.lstOptions.Visible = this.lblOptions.Visible = true;
          this.lstOptions.Items.Add((object) new FieldOption("Yes", "Y"));
          this.lstOptions.Items.Add((object) new FieldOption("No", ""));
          break;
        case FieldFormat.X:
          this.lstOptions.Visible = this.lblOptions.Visible = false;
          break;
        default:
          this.lstOptions.Visible = this.lblOptions.Visible = true;
          List<string> stringList = new List<string>();
          IEnumerator enumerator = fieldDef.Options.GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              FieldOption current = (FieldOption) enumerator.Current;
              if (!stringList.Contains(current.Text))
              {
                stringList.Add(current.Text);
                this.lstOptions.Items.Add((object) current);
              }
            }
            break;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
      }
    }

    private OperatorTypes getSelectedOperator()
    {
      return (OperatorTypes) new OperatorTypesEnumNameProvider().GetValue(this.cboOperator.SelectedItem.ToString());
    }

    private int getParameterCount()
    {
      OperatorTypesEnumNameProvider enumNameProvider = new OperatorTypesEnumNameProvider();
      OperatorTypes op = (OperatorTypes) enumNameProvider.GetValue(this.cboOperator.SelectedItem.ToString());
      return enumNameProvider.GetParameterCount(op);
    }

    private void cboOperator_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboOperator.SelectedIndex == -1)
        return;
      switch (this.getSelectedOperator())
      {
        case OperatorTypes.IsNot:
        case OperatorTypes.Equals:
          if (this.currentField != null && this.currentField.FieldDefinition.IsNumeric())
          {
            this.lblMinNumber.Text = this.lblMinDate.Text = "Value";
            this.lblAndNumber.Visible = this.lblAndDate.Visible = false;
            this.txtMaxNumber.Visible = this.dtMaxDate.Visible = false;
            this.lblMaxNumber.Visible = this.lblMaxDate.Visible = false;
            break;
          }
          this.lblMinDate.Text = "Value";
          this.lblMinDate.Visible = this.dtMinDate.Visible = true;
          this.lblAndDate.Visible = this.dtMaxDate.Visible = this.lblMaxDate.Visible = false;
          break;
        case OperatorTypes.NotEqual:
        case OperatorTypes.DateOnOrAfter:
        case OperatorTypes.DateOnOrBefore:
        case OperatorTypes.DateAfter:
        case OperatorTypes.DateBefore:
          this.lblMinDate.Text = "Value";
          this.lblMinDate.Visible = this.dtMinDate.Visible = true;
          this.lblAndDate.Visible = this.dtMaxDate.Visible = this.lblMaxDate.Visible = false;
          break;
        case OperatorTypes.Between:
        case OperatorTypes.NotBetween:
          this.lblMinNumber.Text = "Minimum";
          this.lblAndNumber.Visible = this.lblMaxNumber.Visible = this.txtMaxNumber.Visible = true;
          break;
        case OperatorTypes.CurrentWeek:
        case OperatorTypes.CurrentMonth:
        case OperatorTypes.YearToDate:
        case OperatorTypes.PreviousWeek:
        case OperatorTypes.PreviousMonth:
        case OperatorTypes.PreviousYear:
        case OperatorTypes.Last7Days:
        case OperatorTypes.Last30Days:
        case OperatorTypes.Last90Days:
        case OperatorTypes.Last365Days:
        case OperatorTypes.EmptyDate:
        case OperatorTypes.NotEmptyDate:
        case OperatorTypes.Today:
        case OperatorTypes.NextWeek:
        case OperatorTypes.NextMonth:
        case OperatorTypes.NextYear:
        case OperatorTypes.Last15Days:
        case OperatorTypes.Last60Days:
        case OperatorTypes.Last180Days:
        case OperatorTypes.Next7Days:
        case OperatorTypes.Next15Days:
        case OperatorTypes.Next30Days:
        case OperatorTypes.Next60Days:
        case OperatorTypes.Next90Days:
        case OperatorTypes.Next180Days:
        case OperatorTypes.Next365Days:
          this.lblMinDate.Visible = this.dtMinDate.Visible = false;
          this.lblAndDate.Visible = this.dtMaxDate.Visible = this.lblMaxDate.Visible = false;
          break;
        case OperatorTypes.DateBetween:
        case OperatorTypes.DateNotBetween:
          this.lblMinDate.Text = "Minimum";
          if (this.dtMaxDate.Value <= this.dtMinDate.Value)
            this.dtMaxDate.Value = this.dtMinDate.Value.AddMonths(1);
          this.lblMinDate.Visible = this.dtMinDate.Visible = true;
          this.lblAndDate.Visible = this.dtMaxDate.Visible = this.lblMaxDate.Visible = true;
          break;
        default:
          this.lblMinNumber.Text = this.lblMinDate.Text = "Value";
          this.lblAndNumber.Visible = this.lblAndDate.Visible = false;
          this.txtMaxNumber.Visible = this.dtMaxDate.Visible = false;
          this.lblMaxNumber.Visible = this.lblMaxDate.Visible = false;
          break;
      }
    }

    public bool ValidateContents()
    {
      if (this.txtField.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a field for this filter.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.txtDescription.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Enter a description for this field in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.cboOperator.SelectedIndex < 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select an operator from the list provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.pnlOptions.Visible && this.lstOptions.CheckedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Select one or more of the available options from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.pnlDate.Visible)
      {
        if (this.dtMinDate.Visible && this.dtMinDate.Value == DateTime.MinValue)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The specified date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        if (this.dtMaxDate.Visible && this.dtMaxDate.Value == DateTime.MinValue)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The specified maximum date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        if (this.getParameterCount() > 1 && this.normalizeDate(this.dtMaxDate.Value) <= this.normalizeDate(this.dtMinDate.Value))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The maximum date must be later than the minimum date.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
      }
      if (this.pnlNumber.Visible)
      {
        if (!Utils.IsDecimal((object) this.txtMinNumber.Text.Trim()))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "One or more values specified is not a valid number.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        if (this.getParameterCount() > 1)
        {
          if (!Utils.IsDecimal((object) this.txtMaxNumber.Text.Trim()))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "One or more values specified is not a valid number.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
          if (Utils.ParseDecimal((object) this.txtMaxNumber.Text.Trim()) <= Utils.ParseDecimal((object) this.txtMinNumber.Text.Trim()))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The maximum value must be greater than the minimum value.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
        }
      }
      if (this.pnlText.Visible)
      {
        OperatorTypes selectedOperator = this.getSelectedOperator();
        if (string.Empty == this.txtStringValue.Text.Trim() && OperatorTypes.IsExact != selectedOperator && OperatorTypes.IsNot != selectedOperator)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Enter the text to be found in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
      }
      return true;
    }

    private DateTime normalizeDate(DateTime date)
    {
      return date == DateTime.MinValue || this.currentField.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate || this.currentField.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime ? date : new DateTime(2000, date.Month, date.Day, date.Hour, date.Minute, date.Second);
    }

    public void CommitChanges()
    {
      this.commitCommonFields();
      if (this.pnlText.Visible)
        this.commitTextFilter();
      else if (this.pnlNumber.Visible)
        this.commitNumericFilter();
      else if (this.pnlDate.Visible)
      {
        this.commitDateFilter();
      }
      else
      {
        if (!this.pnlOptions.Visible)
          return;
        this.commitOptionsFilter();
      }
    }

    private void commitOptionsFilter()
    {
      string[] values = new string[this.lstOptions.CheckedItems.Count];
      string[] names = new string[this.lstOptions.CheckedItems.Count];
      for (int index = 0; index < this.lstOptions.CheckedItems.Count; ++index)
      {
        FieldOption checkedItem = (FieldOption) this.lstOptions.CheckedItems[index];
        names[index] = checkedItem.Text;
        values[index] = checkedItem.ReportingDatabaseValue;
      }
      this.currentFilter.SetOptionsList(values, names);
    }

    private void commitDateFilter()
    {
      string str1 = "";
      string str2 = "";
      if (this.getParameterCount() > 0)
        str1 = this.displayFieldType != EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate ? (this.displayFieldType != EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime ? this.normalizeDate(this.dtMinDate.Value).ToString("MM/dd") : this.normalizeDate(this.dtMinDate.Value).ToString("MM/dd/yyyy HH:mm:ss")) : this.normalizeDate(this.dtMinDate.Value).ToString("MM/dd/yyyy");
      if (this.getParameterCount() > 1)
        str2 = this.displayFieldType != EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate ? (this.displayFieldType != EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime ? this.normalizeDate(this.dtMaxDate.Value).ToString("MM/dd") : this.normalizeDate(this.dtMaxDate.Value).ToString("MM/dd/yyyy HH:mm:ss")) : this.normalizeDate(this.dtMaxDate.Value).ToString("MM/dd/yyyy");
      this.currentFilter.ValueFrom = str1;
      this.currentFilter.ValueTo = str2;
    }

    private void commitNumericFilter()
    {
      this.currentFilter.ValueFrom = this.txtMinNumber.Visible ? this.txtMinNumber.Text.Trim() : string.Empty;
      this.currentFilter.ValueTo = this.txtMaxNumber.Visible ? this.txtMaxNumber.Text.Trim() : string.Empty;
    }

    private void commitTextFilter()
    {
      this.currentFilter.ValueFrom = this.txtStringValue.Text.Trim();
    }

    private void commitCommonFields()
    {
      this.currentFilter.FieldID = this.txtField.Text;
      this.currentFilter.FieldDescription = this.txtDescription.Text;
      this.currentFilter.OperatorType = this.getSelectedOperator();
      this.currentFilter.FieldType = this.displayFieldType;
      this.currentFilter.IsVolatile = this.currentField.IsVolatile;
      this.currentFilter.ForceDataConversion = this.currentFilter.ForceDataConversion;
      this.currentFilter.DataSource = this.currentField.DataSource;
      this.currentFilter.CriterionName = this.currentField.CriterionFieldName;
      this.currentFilter.ValueDescription = (string) null;
    }

    private void txtField_Validating(object sender, CancelEventArgs e)
    {
      if (this.txtField.ReadOnly)
        return;
      string fieldId = this.txtField.Text.Trim();
      if (fieldId == "")
      {
        this.currentField = (ReportFieldDef) null;
        this.setControlState(false);
      }
      else
      {
        if (this.currentField != null && !(fieldId != this.currentField.FieldID))
          return;
        ReportFieldDef selectedDef = this.fieldDefs.GetFieldByID(fieldId);
        if (selectedDef == null || this.DDMSetting)
        {
          ILoanManager loanManager = (ILoanManager) null;
          FieldDefinition fieldDef = this.DDMSetting ? DDM_FieldAccess_Utils.GetFieldDefinition(fieldId, loanManager) : EncompassFields.GetField(fieldId);
          if (fieldDef is VirtualField && !this.allowVirtualFields)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The specified field is not valid in this context.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
            this.setControlState(false);
            return;
          }
          if (fieldDef != null)
            selectedDef = this.fieldDefs.CreateReportFieldDef(Session.DefaultInstance, fieldDef);
        }
        if (selectedDef == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + fieldId + "' is not a valid Field ID.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          e.Cancel = true;
          this.setControlState(false);
        }
        else
        {
          this.loadField(selectedDef, false);
          this.txtDescription.Focus();
        }
      }
    }

    private void setControlState(bool enabled)
    {
      if (!enabled)
      {
        this.txtDescription.Text = "";
        this.pnlOptions.Visible = false;
        this.pnlNumber.Visible = false;
        this.pnlText.Visible = false;
        this.pnlDate.Visible = false;
        this.pnlDateOperatorOption.Visible = false;
      }
      this.txtDescription.ReadOnly = !enabled;
      this.cboOperator.SelectedIndex = -1;
      this.cboOperator.Enabled = enabled;
    }

    private void chkRecurring_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkRecurring.Checked)
        this.initMonthDayFields();
      else
        this.initDateFields();
      this.loadOperatorsForFieldType(this.displayFieldType);
    }

    private void txtField_Enter(object sender, EventArgs e)
    {
      if (this.txtField.ReadOnly || this.ParentForm.AcceptButton == null)
        return;
      this.defaultAcceptButton = this.ParentForm.AcceptButton;
      this.ParentForm.AcceptButton = (IButtonControl) null;
    }

    private void txtField_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.txtField.ReadOnly || e.KeyCode != Keys.Return)
        return;
      this.txtField_Validating(sender, new CancelEventArgs());
    }

    private void txtField_Leave(object sender, EventArgs e)
    {
      if (this.defaultAcceptButton == null)
        return;
      this.ParentForm.AcceptButton = this.defaultAcceptButton;
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
      this.txtStringValue = new TextBox();
      this.cboOperator = new ComboBox();
      this.dtMaxDate = new DatePicker();
      this.dtMinDate = new DatePicker();
      this.lblMinNumber = new Label();
      this.lblAndDate = new Label();
      this.txtDescription = new TextBox();
      this.txtField = new TextBox();
      this.label10 = new Label();
      this.label2 = new Label();
      this.lblMaxDate = new Label();
      this.lblMinDate = new Label();
      this.label3 = new Label();
      this.lblMaxNumber = new Label();
      this.lblAndNumber = new Label();
      this.lstOptions = new CheckedListBox();
      this.txtMaxNumber = new TextBox();
      this.label1 = new Label();
      this.pnlOptions = new Panel();
      this.lblOptions = new Label();
      this.pnlText = new Panel();
      this.pnlDate = new Panel();
      this.txtMinNumber = new TextBox();
      this.pnlNumber = new Panel();
      this.btnFieldList = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.pnlOptionBase = new Panel();
      this.panel2 = new Panel();
      this.pnlOperator = new Panel();
      this.pnlDateOperatorOption = new Panel();
      this.chkRecurring = new CheckBox();
      this.pnlOptions.SuspendLayout();
      this.pnlText.SuspendLayout();
      this.pnlDate.SuspendLayout();
      this.pnlNumber.SuspendLayout();
      ((ISupportInitialize) this.btnFieldList).BeginInit();
      this.pnlOptionBase.SuspendLayout();
      this.panel2.SuspendLayout();
      this.pnlOperator.SuspendLayout();
      this.pnlDateOperatorOption.SuspendLayout();
      this.SuspendLayout();
      this.txtStringValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtStringValue.Location = new Point(68, 0);
      this.txtStringValue.Name = "txtStringValue";
      this.txtStringValue.Size = new Size(217, 20);
      this.txtStringValue.TabIndex = 6;
      this.cboOperator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboOperator.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboOperator.FormattingEnabled = true;
      this.cboOperator.Location = new Point(68, 0);
      this.cboOperator.Name = "cboOperator";
      this.cboOperator.Size = new Size(217, 22);
      this.cboOperator.TabIndex = 23;
      this.cboOperator.SelectedIndexChanged += new EventHandler(this.cboOperator_SelectedIndexChanged);
      this.dtMaxDate.BackColor = SystemColors.Window;
      this.dtMaxDate.Location = new Point(68, 39);
      this.dtMaxDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtMaxDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtMaxDate.Name = "dtMaxDate";
      this.dtMaxDate.Size = new Size(100, 22);
      this.dtMaxDate.TabIndex = 11;
      this.dtMaxDate.Value = new DateTime(0L);
      this.dtMinDate.BackColor = SystemColors.Window;
      this.dtMinDate.Location = new Point(68, 0);
      this.dtMinDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtMinDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtMinDate.Name = "dtMinDate";
      this.dtMinDate.Size = new Size(100, 22);
      this.dtMinDate.TabIndex = 10;
      this.dtMinDate.Value = new DateTime(0L);
      this.lblMinNumber.AutoSize = true;
      this.lblMinNumber.Location = new Point(0, 4);
      this.lblMinNumber.Name = "lblMinNumber";
      this.lblMinNumber.Size = new Size(35, 14);
      this.lblMinNumber.TabIndex = 5;
      this.lblMinNumber.Text = "Value";
      this.lblAndDate.AutoSize = true;
      this.lblAndDate.Location = new Point(67, 23);
      this.lblAndDate.Name = "lblAndDate";
      this.lblAndDate.Size = new Size(25, 14);
      this.lblAndDate.TabIndex = 9;
      this.lblAndDate.Text = "and";
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Location = new Point(68, 24);
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(217, 20);
      this.txtDescription.TabIndex = 17;
      this.txtField.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtField.Location = new Point(68, 2);
      this.txtField.Name = "txtField";
      this.txtField.ReadOnly = true;
      this.txtField.Size = new Size(195, 20);
      this.txtField.TabIndex = 15;
      this.txtField.KeyDown += new KeyEventHandler(this.txtField_KeyDown);
      this.txtField.Leave += new EventHandler(this.txtField_Leave);
      this.txtField.Enter += new EventHandler(this.txtField_Enter);
      this.txtField.Validating += new CancelEventHandler(this.txtField_Validating);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(0, 3);
      this.label10.Name = "label10";
      this.label10.Size = new Size(35, 14);
      this.label10.TabIndex = 5;
      this.label10.Text = "Value";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(0, 28);
      this.label2.Name = "label2";
      this.label2.Size = new Size(61, 14);
      this.label2.TabIndex = 16;
      this.label2.Text = "Description";
      this.lblMaxDate.AutoSize = true;
      this.lblMaxDate.Location = new Point(0, 42);
      this.lblMaxDate.Name = "lblMaxDate";
      this.lblMaxDate.Size = new Size(51, 14);
      this.lblMaxDate.TabIndex = 6;
      this.lblMaxDate.Text = "Maximum";
      this.lblMinDate.AutoSize = true;
      this.lblMinDate.Location = new Point(0, 3);
      this.lblMinDate.Name = "lblMinDate";
      this.lblMinDate.Size = new Size(35, 14);
      this.lblMinDate.TabIndex = 5;
      this.lblMinDate.Text = "Value";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(0, 3);
      this.label3.Name = "label3";
      this.label3.Size = new Size(50, 14);
      this.label3.TabIndex = 18;
      this.label3.Text = "Operator";
      this.lblMaxNumber.AutoSize = true;
      this.lblMaxNumber.Location = new Point(0, 42);
      this.lblMaxNumber.Name = "lblMaxNumber";
      this.lblMaxNumber.Size = new Size(51, 14);
      this.lblMaxNumber.TabIndex = 6;
      this.lblMaxNumber.Text = "Maximum";
      this.lblAndNumber.AutoSize = true;
      this.lblAndNumber.Location = new Point(67, 22);
      this.lblAndNumber.Name = "lblAndNumber";
      this.lblAndNumber.Size = new Size(25, 14);
      this.lblAndNumber.TabIndex = 9;
      this.lblAndNumber.Text = "and";
      this.lstOptions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lstOptions.CheckOnClick = true;
      this.lstOptions.FormattingEnabled = true;
      this.lstOptions.HorizontalScrollbar = true;
      this.lstOptions.Location = new Point(68, 0);
      this.lstOptions.Name = "lstOptions";
      this.lstOptions.Size = new Size(217, 154);
      this.lstOptions.TabIndex = 6;
      this.txtMaxNumber.Location = new Point(68, 38);
      this.txtMaxNumber.Name = "txtMaxNumber";
      this.txtMaxNumber.Size = new Size(123, 20);
      this.txtMaxNumber.TabIndex = 8;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(0, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(29, 14);
      this.label1.TabIndex = 14;
      this.label1.Text = "Field";
      this.pnlOptions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlOptions.Controls.Add((Control) this.lstOptions);
      this.pnlOptions.Controls.Add((Control) this.lblOptions);
      this.pnlOptions.Location = new Point(0, 0);
      this.pnlOptions.Name = "pnlOptions";
      this.pnlOptions.Size = new Size(288, 156);
      this.pnlOptions.TabIndex = 19;
      this.pnlOptions.Visible = false;
      this.lblOptions.AutoSize = true;
      this.lblOptions.Location = new Point(0, 3);
      this.lblOptions.Name = "lblOptions";
      this.lblOptions.Size = new Size(44, 14);
      this.lblOptions.TabIndex = 5;
      this.lblOptions.Text = "Options";
      this.pnlText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlText.Controls.Add((Control) this.txtStringValue);
      this.pnlText.Controls.Add((Control) this.label10);
      this.pnlText.Location = new Point(0, 0);
      this.pnlText.Name = "pnlText";
      this.pnlText.Size = new Size(288, 75);
      this.pnlText.TabIndex = 26;
      this.pnlText.Visible = false;
      this.pnlDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlDate.Controls.Add((Control) this.dtMaxDate);
      this.pnlDate.Controls.Add((Control) this.dtMinDate);
      this.pnlDate.Controls.Add((Control) this.lblAndDate);
      this.pnlDate.Controls.Add((Control) this.lblMaxDate);
      this.pnlDate.Controls.Add((Control) this.lblMinDate);
      this.pnlDate.Location = new Point(0, 0);
      this.pnlDate.Name = "pnlDate";
      this.pnlDate.Size = new Size(288, 82);
      this.pnlDate.TabIndex = 25;
      this.pnlDate.Visible = false;
      this.txtMinNumber.Location = new Point(68, 0);
      this.txtMinNumber.Name = "txtMinNumber";
      this.txtMinNumber.Size = new Size(123, 20);
      this.txtMinNumber.TabIndex = 7;
      this.pnlNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlNumber.Controls.Add((Control) this.lblAndNumber);
      this.pnlNumber.Controls.Add((Control) this.txtMaxNumber);
      this.pnlNumber.Controls.Add((Control) this.txtMinNumber);
      this.pnlNumber.Controls.Add((Control) this.lblMaxNumber);
      this.pnlNumber.Controls.Add((Control) this.lblMinNumber);
      this.pnlNumber.Location = new Point(0, 0);
      this.pnlNumber.Name = "pnlNumber";
      this.pnlNumber.Size = new Size(288, (int) sbyte.MaxValue);
      this.pnlNumber.TabIndex = 24;
      this.pnlNumber.Visible = false;
      this.btnFieldList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFieldList.BackColor = Color.Transparent;
      this.btnFieldList.Location = new Point(267, 3);
      this.btnFieldList.Name = "btnFieldList";
      this.btnFieldList.Size = new Size(16, 16);
      this.btnFieldList.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFieldList.TabIndex = 27;
      this.btnFieldList.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnFieldList, "Find Field");
      this.btnFieldList.Click += new EventHandler(this.btnFieldList_Click);
      this.pnlOptionBase.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlOptionBase.Controls.Add((Control) this.panel2);
      this.pnlOptionBase.Controls.Add((Control) this.pnlOperator);
      this.pnlOptionBase.Controls.Add((Control) this.pnlDateOperatorOption);
      this.pnlOptionBase.Location = new Point(0, 46);
      this.pnlOptionBase.Name = "pnlOptionBase";
      this.pnlOptionBase.Size = new Size(292, 203);
      this.pnlOptionBase.TabIndex = 28;
      this.panel2.Controls.Add((Control) this.pnlText);
      this.panel2.Controls.Add((Control) this.pnlOptions);
      this.panel2.Controls.Add((Control) this.pnlNumber);
      this.panel2.Controls.Add((Control) this.pnlDate);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 46);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(292, 157);
      this.panel2.TabIndex = 1;
      this.pnlOperator.Controls.Add((Control) this.cboOperator);
      this.pnlOperator.Controls.Add((Control) this.label3);
      this.pnlOperator.Dock = DockStyle.Top;
      this.pnlOperator.Location = new Point(0, 22);
      this.pnlOperator.Name = "pnlOperator";
      this.pnlOperator.Size = new Size(292, 24);
      this.pnlOperator.TabIndex = 0;
      this.pnlDateOperatorOption.Controls.Add((Control) this.chkRecurring);
      this.pnlDateOperatorOption.Dock = DockStyle.Top;
      this.pnlDateOperatorOption.Location = new Point(0, 0);
      this.pnlDateOperatorOption.Name = "pnlDateOperatorOption";
      this.pnlDateOperatorOption.Size = new Size(292, 22);
      this.pnlDateOperatorOption.TabIndex = 0;
      this.pnlDateOperatorOption.Visible = false;
      this.chkRecurring.Location = new Point(68, 1);
      this.chkRecurring.Name = "chkRecurring";
      this.chkRecurring.Size = new Size(214, 18);
      this.chkRecurring.TabIndex = 0;
      this.chkRecurring.Text = "Date is recurring (disregard the year).";
      this.chkRecurring.UseVisualStyleBackColor = true;
      this.chkRecurring.CheckedChanged += new EventHandler(this.chkRecurring_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlOptionBase);
      this.Controls.Add((Control) this.btnFieldList);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.txtField);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (FieldFilterEditor);
      this.Size = new Size(301, 260);
      this.pnlOptions.ResumeLayout(false);
      this.pnlOptions.PerformLayout();
      this.pnlText.ResumeLayout(false);
      this.pnlText.PerformLayout();
      this.pnlDate.ResumeLayout(false);
      this.pnlDate.PerformLayout();
      this.pnlNumber.ResumeLayout(false);
      this.pnlNumber.PerformLayout();
      ((ISupportInitialize) this.btnFieldList).EndInit();
      this.pnlOptionBase.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.pnlOperator.ResumeLayout(false);
      this.pnlOperator.PerformLayout();
      this.pnlDateOperatorOption.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
