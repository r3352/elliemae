// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.GridViewFilterManager
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class GridViewFilterManager : IDisposable
  {
    private GridView gridView;
    private List<Control> filterControls = new List<Control>();
    private bool autoFilter;
    private bool suspendEvents;
    private bool allowTextMatchForOptions;
    private bool applyFilterToEmptyCells = true;
    private bool applyEmptyFilters = true;
    private Sessions.Session session;

    public event EventHandler FilterChanged;

    public GridViewFilterManager(Sessions.Session session, GridView gridView)
      : this(session, gridView, false)
    {
    }

    public GridViewFilterManager(Sessions.Session session, GridView gridView, bool autoFilter)
    {
      this.session = session;
      this.gridView = gridView;
      this.autoFilter = autoFilter;
    }

    public GridView GridView => this.gridView;

    public bool AllowTextMatchForListOptions
    {
      get => this.allowTextMatchForOptions;
      set => this.allowTextMatchForOptions = value;
    }

    public bool ApplyFilterToEmptyCells
    {
      get => this.applyFilterToEmptyCells;
      set => this.applyFilterToEmptyCells = value;
    }

    public bool ApplyEmptyFilters
    {
      get => this.applyEmptyFilters;
      set => this.applyEmptyFilters = value;
    }

    public virtual Control CreateColumnFilter(
      int columnIndex,
      GridViewFilterControlType controlType)
    {
      Control columnFilter;
      switch (controlType)
      {
        case GridViewFilterControlType.Text:
          columnFilter = this.createTextFilterControl(TextBoxContentRule.None);
          break;
        case GridViewFilterControlType.Dropdown:
          columnFilter = this.createDropdownFilterControl(true);
          break;
        case GridViewFilterControlType.DropdownList:
          columnFilter = this.createDropdownFilterControl(false);
          break;
        case GridViewFilterControlType.Integer:
          columnFilter = this.createNumericFilterControl(TextBoxContentRule.Integer);
          break;
        case GridViewFilterControlType.Decimal:
          columnFilter = this.createNumericFilterControl(TextBoxContentRule.Decimal);
          break;
        case GridViewFilterControlType.Date:
        case GridViewFilterControlType.DateTime:
          columnFilter = this.createDateFilterControl(DateFilterBox.DateStyle.FullDate);
          break;
        case GridViewFilterControlType.MonthDay:
          columnFilter = this.createDateFilterControl(DateFilterBox.DateStyle.MonthDay);
          break;
        case GridViewFilterControlType.Phone:
          columnFilter = this.createTextFilterControl(TextBoxContentRule.PhoneNumber);
          break;
        case GridViewFilterControlType.ZipCode:
          columnFilter = this.createTextFilterControl(TextBoxContentRule.ZipCode);
          break;
        case GridViewFilterControlType.Milestone:
          columnFilter = this.createMilestoneFilterControl(false, true);
          break;
        case GridViewFilterControlType.NextMilestone:
          columnFilter = this.createMilestoneFilterControl(false, false);
          break;
        case GridViewFilterControlType.CoreMilestone:
          columnFilter = this.createMilestoneFilterControl(true, true);
          break;
        case GridViewFilterControlType.RateLock:
          columnFilter = this.createRateLockFilterControl(false);
          break;
        case GridViewFilterControlType.RateLockAndRequest:
          columnFilter = this.createRateLockFilterControl(true);
          break;
        case GridViewFilterControlType.ContactGroup:
          columnFilter = this.createContactGroupFilterControl(EllieMae.EMLite.ContactUI.ContactType.Borrower);
          break;
        case GridViewFilterControlType.BizContactGroup:
          columnFilter = this.createContactGroupFilterControl(EllieMae.EMLite.ContactUI.ContactType.BizPartner);
          break;
        case GridViewFilterControlType.PublicContactGroup:
          columnFilter = this.createContactGroupFilterControl(EllieMae.EMLite.ContactUI.ContactType.PublicBiz);
          break;
        case GridViewFilterControlType.LockValidationStatus:
          columnFilter = this.createLockValidationStatusFilterControl();
          break;
        case GridViewFilterControlType.ObjectType:
          columnFilter = this.createObjectTypeFilterControl();
          break;
        default:
          throw new ArgumentException("Unknown Filter control type specified");
      }
      this.filterControls.Add(columnFilter);
      this.gridView.Columns[columnIndex].FilterControl = columnFilter;
      return columnFilter;
    }

    public Control CreateColumnFilter(int columnIndex, FieldFormat format)
    {
      switch (format)
      {
        case FieldFormat.YN:
          ComboBox columnFilter1 = (ComboBox) this.CreateColumnFilter(columnIndex, GridViewFilterControlType.DropdownList);
          columnFilter1.Items.AddRange((object[]) new string[3]
          {
            "",
            "Y",
            "N"
          });
          return (Control) columnFilter1;
        case FieldFormat.X:
          ComboBox columnFilter2 = (ComboBox) this.CreateColumnFilter(columnIndex, GridViewFilterControlType.DropdownList);
          columnFilter2.Items.AddRange((object[]) new string[2]
          {
            "",
            "X"
          });
          return (Control) columnFilter2;
        case FieldFormat.ZIPCODE:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.ZipCode);
        case FieldFormat.PHONE:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.Phone);
        case FieldFormat.INTEGER:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.Integer);
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.Decimal);
        case FieldFormat.DATE:
        case FieldFormat.DATETIME:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.Date);
        case FieldFormat.MONTHDAY:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.MonthDay);
        case FieldFormat.DROPDOWNLIST:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.DropdownList);
        case FieldFormat.DROPDOWN:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.Dropdown);
        default:
          return this.CreateColumnFilter(columnIndex, GridViewFilterControlType.Text);
      }
    }

    public Control CreateColumnFilter(int columnIndex, FieldDefinition fieldDef)
    {
      FieldFormat format = fieldDef.Format;
      if (fieldDef.Options.RequireValueFromList)
        format = FieldFormat.DROPDOWNLIST;
      else if (fieldDef.Options.Count > 0)
        format = FieldFormat.DROPDOWN;
      Control columnFilter = this.CreateColumnFilter(columnIndex, format);
      if (columnFilter.GetType() == typeof (ComboBox))
      {
        ComboBox comboBox = (ComboBox) columnFilter;
        comboBox.Items.Clear();
        comboBox.Items.Add((object) "");
        comboBox.Items.AddRange((object[]) fieldDef.Options.ToArray());
      }
      return columnFilter;
    }

    public virtual void ReleaseFilterControls()
    {
      foreach (GVColumn column in this.gridView.Columns)
        column.FilterControl = (Control) null;
      foreach (Control filterControl in this.filterControls)
        this.ReleaseControl(filterControl);
      this.filterControls.Clear();
    }

    public void ClearColumnFilters()
    {
      this.suspendEvents = true;
      foreach (GVColumn column in this.gridView.Columns)
      {
        if (column.FilterControl != null)
          this.ClearControl(column.FilterControl);
      }
      if (this.autoFilter)
        this.ApplyFilter();
      this.suspendEvents = false;
    }

    protected List<Control> FilterControls => this.filterControls;

    protected virtual void ReleaseControl(Control c)
    {
      switch (c)
      {
        case DateFilterBox _:
          ((DateFilterBox) c).FilterChanged -= new EventHandler(this.onFilterValueChanged);
          break;
        case NumericFilterBox _:
          ((NumericFilterBox) c).FilterChanged -= new EventHandler(this.onFilterValueChanged);
          break;
        case ComboBox _:
          ((ComboBox) c).SelectedIndexChanged -= new EventHandler(this.onFilterValueChanged);
          c.TextChanged -= new EventHandler(this.onFilterValueChanged);
          break;
        case SizableTextBox _:
          ((SizableTextBox) c).ChangeCommitted -= new EventHandler(this.onFilterValueChanged);
          ((SizableTextBox) c).TextBox.KeyDown += new KeyEventHandler(this.onTextBoxKeyDown);
          break;
        case MilestoneDropdownBox _:
          ((MilestoneDropdownBox) c).SelectedIndexChanged -= new EventHandler(this.onFilterValueChanged);
          break;
        case RateLockDropdownBox _:
          ((RateLockDropdownBox) c).SelectedIndexChanged -= new EventHandler(this.onFilterValueChanged);
          break;
        case LockValidationStatusDropdownBox _:
          ((LockValidationStatusDropdownBox) c).SelectedIndexChanged -= new EventHandler(this.onFilterValueChanged);
          break;
      }
      if (c.Parent == null)
        return;
      c.Parent.Controls.Remove(c);
    }

    protected virtual void ClearControl(Control c)
    {
      switch (c)
      {
        case DateFilterBox _:
          ((DateFilterBox) c).Value = DateTime.MinValue;
          break;
        case NumericFilterBox _:
          ((NumericFilterBox) c).Value = Decimal.MinValue;
          break;
        case ComboBox _:
          c.Text = "";
          break;
        case SizableTextBox _:
          ((SizableTextBox) c).TextBox.Text = "";
          break;
        case MilestoneDropdownBox _:
          ((MilestoneDropdownBox) c).MilestoneName = (string) null;
          break;
        case RateLockDropdownBox _:
          ((RateLockDropdownBox) c).Clear();
          break;
        case LockValidationStatusDropdownBox _:
          ((LockValidationStatusDropdownBox) c).Clear();
          break;
      }
      if (c.Parent == null)
        return;
      c.Parent.Controls.Remove(c);
    }

    public void ClearFilterValue(Control c)
    {
      this.suspendEvents = true;
      switch (c)
      {
        case DateFilterBox _:
          ((DateFilterBox) c).Value = DateTime.MinValue;
          break;
        case NumericFilterBox _:
          ((NumericFilterBox) c).Value = Decimal.MinValue;
          break;
        case ComboBox _:
          c.Text = "";
          break;
        case SizableTextBox _:
          ((SizableTextBox) c).TextBox.Text = "";
          break;
        case MilestoneDropdownBox _:
          ((MilestoneDropdownBox) c).MilestoneName = (string) null;
          break;
        case ObjectTypeDropdownBox _:
          ((ObjectTypeDropdownBox) c).ObjectType = (string) null;
          break;
      }
      this.suspendEvents = false;
    }

    public void SetFilterValue(Control c, string value)
    {
      this.suspendEvents = true;
      switch (c)
      {
        case DateFilterBox _:
          ((DateFilterBox) c).Value = Convert.ToDateTime(value);
          break;
        case NumericFilterBox _:
          ((NumericFilterBox) c).Value = Convert.ToDecimal(value);
          break;
        case ComboBox _:
          c.Text = value;
          break;
        case SizableTextBox _:
          ((SizableTextBox) c).TextBox.Text = value;
          break;
        case MilestoneDropdownBox _:
          ((MilestoneDropdownBox) c).MilestoneName = value;
          break;
        case ObjectTypeDropdownBox _:
          ((ObjectTypeDropdownBox) c).ObjectType = value;
          break;
      }
      this.suspendEvents = false;
    }

    public QueryCriterion ToQueryCriterion()
    {
      return this.ToFieldFilterList().CreateEvaluator().ToQueryCriterion();
    }

    public FieldFilterList ToFieldFilterList()
    {
      FieldFilterList fieldFilterList = new FieldFilterList();
      foreach (GVColumn column in this.gridView.Columns)
      {
        if (column.FilterControl != null)
        {
          FieldFilter filterForColumn = this.GetFilterForColumn(column);
          if (filterForColumn != null)
          {
            filterForColumn.JointToken = JointTokens.And;
            fieldFilterList.Add(filterForColumn);
          }
        }
      }
      return fieldFilterList;
    }

    public FieldFilter GetFilterForColumn(int index)
    {
      GVColumn column = this.gridView.Columns[index];
      return column.FilterControl == null ? (FieldFilter) null : this.GetFilterForColumn(column);
    }

    protected virtual FieldFilter GetFilterForColumn(GVColumn col)
    {
      string fieldName = !(col.Tag is TableLayout.Column tag) ? string.Concat(col.Tag) : tag.ColumnID;
      if (fieldName == null)
        return (FieldFilter) null;
      if (col.FilterControl is SizableTextBox)
        return this.getTextFilter(col.Text, fieldName, col.FilterControl as SizableTextBox);
      if (col.FilterControl is ContactGroupDropdownBox)
        return this.getContactGroupFilter(col.Text, fieldName, col.FilterControl as ContactGroupDropdownBox);
      if (col.FilterControl is ComboBox)
        return this.getListFilter(col.Text, fieldName, col.FilterControl as ComboBox);
      if (col.FilterControl is DateFilterBox)
        return this.getDateFilter(col.Text, fieldName, col.FilterControl as DateFilterBox);
      if (col.FilterControl is NumericFilterBox)
        return this.getNumericFilter(col.Text, fieldName, col.FilterControl as NumericFilterBox);
      if (col.FilterControl is MilestoneDropdownBox)
        return this.getMilestoneFilter(col.Text, fieldName, col.FilterControl as MilestoneDropdownBox);
      if (col.FilterControl is RateLockDropdownBox)
        return this.getRateLockFilter(col.Text, fieldName, col.FilterControl as RateLockDropdownBox);
      if (col.FilterControl is LockValidationStatusDropdownBox)
        return this.getLockValidationStatusFilter(col.Text, fieldName, col.FilterControl as LockValidationStatusDropdownBox);
      return col.FilterControl is ObjectTypeDropdownBox ? this.getObjectTypeFilter(col.Text, fieldName, col.FilterControl as ObjectTypeDropdownBox) : (FieldFilter) null;
    }

    private FieldFilter getTextFilter(string fieldDesc, string fieldName, SizableTextBox textBox)
    {
      return textBox.Text == "" ? (FieldFilter) null : new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, fieldName, fieldDesc, OperatorTypes.Contains, textBox.Text);
    }

    private FieldFilter getListFilter(string fieldDesc, string fieldName, ComboBox cboBox)
    {
      return cboBox.SelectedItem is FieldOption selectedItem ? (selectedItem.Text.Trim() == BorrowerPair.All.ToString() ? new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList, fieldName, fieldName, fieldDesc, OperatorTypes.Contains, this.getOptionValueList(selectedItem), selectedItem.Text) : new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList, fieldName, fieldName, fieldDesc, OperatorTypes.IsAnyOf, this.getOptionValueList(selectedItem), selectedItem.Text)) : (cboBox.Text == "" ? (FieldFilter) null : new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, fieldName, fieldName, fieldDesc, OperatorTypes.Contains, cboBox.Text));
    }

    private string[] getOptionValueList(FieldOption option)
    {
      List<string> stringList = new List<string>();
      stringList.Add(option.Value);
      if (!stringList.Contains(option.ReportingDatabaseValue))
        stringList.Add(option.ReportingDatabaseValue);
      if (this.AllowTextMatchForListOptions && !stringList.Contains(option.Text))
        stringList.Add(option.Text);
      return stringList.ToArray();
    }

    private FieldFilter getDateFilter(string fieldDesc, string fieldName, DateFilterBox dtFilter)
    {
      return dtFilter.Value == DateTime.MinValue ? (FieldFilter) null : new FieldFilter(dtFilter.Style == DateFilterBox.DateStyle.FullDate ? EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate : EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay, fieldName, fieldDesc, GridViewFilterManager.getOperatorTypeForComparison(dtFilter.ComparisonOperator), dtFilter.FormattedValue);
    }

    private FieldFilter getNumericFilter(
      string fieldDesc,
      string fieldName,
      NumericFilterBox numFilter)
    {
      return numFilter.Value == Decimal.MinValue ? (FieldFilter) null : new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric, fieldName, fieldDesc, GridViewFilterManager.getOperatorTypeForComparison(numFilter.ComparisonOperator), numFilter.Value.ToString());
    }

    private FieldFilter getMilestoneFilter(
      string fieldDesc,
      string fieldName,
      MilestoneDropdownBox msFilter)
    {
      if (msFilter.MilestoneName == null)
        return (FieldFilter) null;
      return new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList, fieldName, fieldName, fieldDesc, OperatorTypes.IsAnyOf, new string[1]
      {
        msFilter.MilestoneName
      }, msFilter.MilestoneDisplayName);
    }

    private FieldFilter getRateLockFilter(
      string fieldDesc,
      string fieldName,
      RateLockDropdownBox rlFilter)
    {
      string[] selectedLockStatuses = rlFilter.GetSelectedLockStatuses();
      if (selectedLockStatuses == null || selectedLockStatuses.Length == 0)
        return (FieldFilter) null;
      FieldDefinition fieldDefinition = rlFilter.IncludesRequestStatus ? (FieldDefinition) new RateLockAndRequestStatusField() : (FieldDefinition) new RateLockStatusField();
      string valueDescription = "";
      for (int index = 0; index < selectedLockStatuses.Length; ++index)
      {
        if (index > 0)
          valueDescription += ", ";
        valueDescription += fieldDefinition.Options.ValueToText(selectedLockStatuses[index]);
      }
      return new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList, fieldDefinition.FieldID, fieldName, fieldDefinition.Description, OperatorTypes.IsAnyOf, selectedLockStatuses, valueDescription);
    }

    private FieldFilter getContactGroupFilter(
      string fieldDesc,
      string fieldName,
      ContactGroupDropdownBox cgFilter)
    {
      if (string.Concat(cgFilter.SelectedItem) == "")
        return (FieldFilter) null;
      string fieldID = "";
      string fieldDescription = "";
      string criterionName = "ContactGroup.GroupName";
      switch (cgFilter.ContactType)
      {
        case EllieMae.EMLite.ContactUI.ContactType.Borrower:
          fieldID = "ContactGroup";
          fieldDescription = "Contact Group Name";
          break;
        case EllieMae.EMLite.ContactUI.ContactType.BizPartner:
          fieldID = "BizGroupName";
          fieldDescription = "Contact Group Name";
          break;
        case EllieMae.EMLite.ContactUI.ContactType.PublicBiz:
          fieldID = "PublicContactGroupName";
          fieldDescription = "Public Contact Group Name";
          criterionName = "PublicContactGroup.GroupName";
          break;
      }
      return new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList, fieldID, criterionName, fieldDescription, OperatorTypes.IsAnyOf, new string[1]
      {
        string.Concat(cgFilter.SelectedItem)
      }, string.Concat(cgFilter.SelectedItem));
    }

    private FieldFilter getLockValidationStatusFilter(
      string fieldDesc,
      string fieldName,
      LockValidationStatusDropdownBox rlFilter)
    {
      string[] validationStatuses = rlFilter.GetSelectedValidationStatuses();
      if (validationStatuses == null || validationStatuses.Length == 0)
        return (FieldFilter) null;
      FieldDefinition field = EncompassFields.GetField("4788");
      string valueDescription = "";
      for (int index = 0; index < validationStatuses.Length; ++index)
      {
        if (index > 0)
          valueDescription += ", ";
        valueDescription += field.Options.ValueToText(validationStatuses[index]);
      }
      return new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList, field.FieldID, fieldName, fieldDesc, OperatorTypes.IsAnyOf, validationStatuses, valueDescription);
    }

    private FieldFilter getObjectTypeFilter(
      string fieldDesc,
      string fieldName,
      ObjectTypeDropdownBox otFilter)
    {
      if (otFilter.ObjectType == null)
        return (FieldFilter) null;
      return new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList, fieldName, fieldName, fieldDesc, OperatorTypes.IsAnyOf, new string[1]
      {
        otFilter.ObjectType
      }, otFilter.ObjectType);
    }

    private static OperatorTypes getOperatorTypeForComparison(ComparisonOperator op)
    {
      switch (op)
      {
        case ComparisonOperator.Equals:
          return OperatorTypes.Equals;
        case ComparisonOperator.NotEquals:
          return OperatorTypes.NotEqual;
        case ComparisonOperator.GreaterThan:
          return OperatorTypes.GreaterThan;
        case ComparisonOperator.GreaterThanOrEquals:
          return OperatorTypes.NotLessThan;
        case ComparisonOperator.LessThan:
          return OperatorTypes.LessThan;
        case ComparisonOperator.LessThanOrEquals:
          return OperatorTypes.NotGreaterThan;
        default:
          throw new ArgumentException("Invalid comparison operator specified");
      }
    }

    public virtual void Dispose()
    {
      this.ReleaseFilterControls();
      this.gridView = (GridView) null;
    }

    private Control createDateFilterControl(DateFilterBox.DateStyle dateStyle)
    {
      DateFilterBox dateFilterControl = new DateFilterBox();
      dateFilterControl.AutoHeight = false;
      dateFilterControl.Style = dateStyle;
      dateFilterControl.FilterChanged += new EventHandler(this.onFilterValueChanged);
      return (Control) dateFilterControl;
    }

    private Control createDropdownFilterControl(bool allowEdit)
    {
      ComboBox dropdownFilterControl = new ComboBox();
      dropdownFilterControl.DropDownStyle = allowEdit ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
      dropdownFilterControl.SelectedIndexChanged += new EventHandler(this.onFilterValueChanged);
      if (allowEdit)
        dropdownFilterControl.Validated += new EventHandler(this.onFilterValueChanged);
      return (Control) dropdownFilterControl;
    }

    private Control createTextFilterControl(TextBoxContentRule contentRule)
    {
      SizableTextBox textFilterControl = new SizableTextBox();
      textFilterControl.AutoHeight = false;
      if (contentRule != TextBoxContentRule.None)
        TextBoxFormatter.Attach(textFilterControl.TextBox, contentRule);
      textFilterControl.TextBox.KeyDown += new KeyEventHandler(this.onTextBoxKeyDown);
      textFilterControl.ChangeCommitted += new EventHandler(this.onFilterValueChanged);
      textFilterControl.Tag = (object) contentRule;
      return (Control) textFilterControl;
    }

    private void onTextBoxKeyDown(object sender, KeyEventArgs e)
    {
      try
      {
        if (e.KeyCode != Keys.Return)
          return;
        ((Control) sender).FindForm().SelectNextControl((Control) sender, true, true, true, true);
      }
      catch
      {
      }
    }

    private Control createNumericFilterControl(TextBoxContentRule contentRule)
    {
      NumericFilterBox numericFilterControl = new NumericFilterBox(contentRule);
      numericFilterControl.AutoHeight = false;
      numericFilterControl.FilterChanged += new EventHandler(this.onFilterValueChanged);
      return (Control) numericFilterControl;
    }

    private Control createMilestoneFilterControl(bool coreMilestonesOnly, bool includeFileStarted)
    {
      MilestoneDropdownBox milestoneFilterControl = new MilestoneDropdownBox();
      milestoneFilterControl.PopulateAllMilestones((IEnumerable<EllieMae.EMLite.Workflow.Milestone>) this.session.StartupInfo.Milestones, includeFileStarted, true);
      milestoneFilterControl.SelectedIndexChanged += new EventHandler(this.onFilterValueChanged);
      return (Control) milestoneFilterControl;
    }

    private Control createRateLockFilterControl(bool displayRequestStatus)
    {
      RateLockDropdownBox lockFilterControl = new RateLockDropdownBox(displayRequestStatus);
      lockFilterControl.SelectedIndexChanged += new EventHandler(this.onFilterValueChanged);
      return (Control) lockFilterControl;
    }

    private Control createContactGroupFilterControl(EllieMae.EMLite.ContactUI.ContactType contactType)
    {
      ContactGroupDropdownBox groupFilterControl = new ContactGroupDropdownBox(contactType);
      groupFilterControl.SelectedIndexChanged += new EventHandler(this.onFilterValueChanged);
      return (Control) groupFilterControl;
    }

    private Control createLockValidationStatusFilterControl()
    {
      LockValidationStatusDropdownBox statusFilterControl = new LockValidationStatusDropdownBox();
      statusFilterControl.SelectedIndexChanged += new EventHandler(this.onFilterValueChanged);
      return (Control) statusFilterControl;
    }

    private Control createObjectTypeFilterControl()
    {
      ObjectTypeDropdownBox typeFilterControl = new ObjectTypeDropdownBox();
      typeFilterControl.SelectedIndexChanged += new EventHandler(this.onFilterValueChanged);
      return (Control) typeFilterControl;
    }

    private void onFilterValueChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.OnFilterChanged(EventArgs.Empty);
    }

    protected virtual void OnFilterChanged(EventArgs e)
    {
      if (this.autoFilter)
        this.ApplyFilter();
      if (this.FilterChanged == null)
        return;
      this.FilterChanged((object) this, e);
    }

    public void ApplyFilter()
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 788, nameof (ApplyFilter), "D:\\ws\\24.3.0.0\\EmLite\\ClientCommon\\UI\\GridViewFilterManager.cs");
      FieldFilter[] columnFilters = this.getColumnFilters();
      PerformanceMeter.Current.AddCheckpoint("getColumnFilters", 792, nameof (ApplyFilter), "D:\\ws\\24.3.0.0\\EmLite\\ClientCommon\\UI\\GridViewFilterManager.cs");
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridView.Items)
        gvItem.State = !this.compareToFilter(gvItem, columnFilters) ? GVItemState.Hidden : GVItemState.Normal;
      PerformanceMeter.Current.AddCheckpoint("after Filter loop", 804, nameof (ApplyFilter), "D:\\ws\\24.3.0.0\\EmLite\\ClientCommon\\UI\\GridViewFilterManager.cs");
      this.gridView.ReSort();
      PerformanceMeter.Current.AddCheckpoint("END", 809, nameof (ApplyFilter), "D:\\ws\\24.3.0.0\\EmLite\\ClientCommon\\UI\\GridViewFilterManager.cs");
    }

    private FieldFilter[] getColumnFilters()
    {
      FieldFilter[] columnFilters = new FieldFilter[this.gridView.Columns.Count];
      for (int index = 0; index < this.gridView.Columns.Count; ++index)
        columnFilters[index] = this.GetFilterForColumn(index);
      return columnFilters;
    }

    private bool compareToFilter(GVItem item, FieldFilter[] filters)
    {
      for (int nItemIndex = 0; nItemIndex < filters.Length; ++nItemIndex)
      {
        FieldFilter filter = filters[nItemIndex];
        if (filter != null)
        {
          object obj = item.SubItems[nItemIndex].Value;
          if (obj is MilestoneLabel)
            obj = (object) ((MilestoneLabel) obj).MilestoneName;
          if (obj is ImageElement && ((Element) obj).Tag != null)
            obj = ((Element) obj).Tag;
          if ((this.applyFilterToEmptyCells || obj != null && obj.ToString() != "") && (this.applyEmptyFilters || !this.applyEmptyFilters && !string.IsNullOrEmpty(filter.ValueFrom)) && !filter.Evaluate(obj))
            return false;
        }
      }
      return true;
    }

    public void RefreshFilterContent()
    {
      foreach (Control filterControl in this.filterControls)
      {
        if (filterControl is ContactGroupDropdownBox)
          ((ContactGroupDropdownBox) filterControl).RefreshFilter();
      }
    }
  }
}
