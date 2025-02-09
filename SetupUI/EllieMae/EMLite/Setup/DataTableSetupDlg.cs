// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DataTableSetupDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.DynamicDataManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DataTableSetupDlg : Form, IHelp
  {
    private const string COL_OUTPUT_TXT = "Output";
    private static readonly string _sw = Tracing.SwOutsideLoan;
    private Sessions.Session _session;
    private DataTableSetupDlg.FiniteStateMachine _fsm;
    private bool isDirty;
    private bool showUserInputPopup;
    private int changedRowIndex = -1;
    private int lastSelectIndex = -1;
    private int tableID = -1;
    private DataTableSetupDlg.FormDataSource _formDataSource;
    private StandardFields cachedStandardFields;
    private FieldSettings cachedFieldSettings;
    private IContainer components;
    private GroupContainer gcDataTableSetup;
    private StandardIconButton stdButtonNew;
    private StandardIconButton stdButtonUp;
    private StandardIconButton stdButtonDown;
    private StandardIconButton stdButtonDelete;
    private GridView gvDataTableSetup;
    private TabPageEx tpValues;
    private ButtonEx btnClearValue;
    private ButtonEx btnSetValue;
    private GridView gvScenarioFeeValues;
    private ToolTip ttipMessageAltText;
    private ButtonEx btnClose;
    private GradientPanel gradientPanel1;
    private StandardIconButton stdButtonSave;
    private StandardIconButton stdButtonReset;
    private ButtonEx btnRulesUsingThisTable;
    private ButtonEx btnModifyDataTable;
    private FormattedLabel lblHeaderText;
    private EMHelpLink emHelpLink1;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem addRowToolStripMenuItem;
    private ToolStripMenuItem insertRowToolStripMenuItem;
    private ToolStripMenuItem duplicateRowToolStripMenuItem;
    private ToolStripMenuItem deleteRowToolStripMenuItem;
    private ToolStripMenuItem moveUpToolStripMenuItem;
    private ToolStripMenuItem moveDownToolStripMenuItem;
    private StandardIconButton stdButtonDuplicate;
    private VerticalSeparator verticalSeparator1;

    public DataTableSetupDlg()
    {
      this.InitializeComponent();
      this.MinimizeBox = true;
      this.MaximizeBox = true;
      this.ShowInTaskbar = false;
      this.FormBorderStyle = FormBorderStyle.Sizable;
      this.FormClosing += new FormClosingEventHandler(this.DataTableSetupDlg_FormClosing);
      this.gvDataTableSetup.SortOption = GVSortOption.None;
      this.ContextMenuStrip = this.contextMenuStrip1;
      this._fsm = new DataTableSetupDlg.FiniteStateMachine(this);
    }

    public DataTableSetupDlg(int existingDataTableId, Sessions.Session session)
      : this()
    {
      this.tableID = existingDataTableId;
      this._session = session;
      this._formDataSource = new DataTableSetupDlg.FormDataSource(this.fetchDataTable(existingDataTableId), session);
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.OpenWithExisting);
    }

    public DataTableSetupDlg(DDMDataTableInfo dataTableInfo, Sessions.Session session)
      : this()
    {
      this._session = session;
      this._formDataSource = new DataTableSetupDlg.FormDataSource(dataTableInfo, session);
      this._formDataSource.SetUserInfo(this._session);
      this.showUserInputPopup = true;
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.OpenWithNew);
    }

    private void DataTableListDlg_Activated(object sender, EventArgs e)
    {
      int num = this.showUserInputPopup ? 1 : 0;
      this.showUserInputPopup = false;
    }

    private void DataTableSetupDlg_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this._fsm.CanCloseForm)
        return;
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.FormClosing);
      e.Cancel = !this._fsm.CanCloseForm;
    }

    private void refreshGrid()
    {
      if (this.tableID == -1)
        return;
      this.gvDataTableSetup.Items.Clear();
      this._formDataSource = new DataTableSetupDlg.FormDataSource(this.fetchDataTable(this.tableID), this._session);
      this.initGridViewColumns();
      this.initGridViewData();
      this.changedRowIndex = -1;
    }

    private void emHelpLink1_Help(object sender, EventArgs e)
    {
    }

    private void gvDataTableSetup_SelectedIndexChanged(object sender, EventArgs e)
    {
      GridView gridView = sender as GridView;
      if (gridView.SelectedItems != null && gridView.SelectedItems.Count > 0)
      {
        int index = gridView.SelectedItems[0].Index;
        if (this.lastSelectIndex >= 0 && this.lastSelectIndex < this.gvDataTableSetup.Items.Count)
          this.hideBuddyButtonForRow(this.lastSelectIndex);
        this.showBuddyButtonForRow(index);
        this.lastSelectIndex = index;
      }
      else
      {
        if (this.lastSelectIndex >= 0 && this.lastSelectIndex < this.gvDataTableSetup.Items.Count)
          this.hideBuddyButtonForRow(this.lastSelectIndex);
        this.lastSelectIndex = -1;
      }
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction);
    }

    private void stdButtonUp_Click(object sender, EventArgs e)
    {
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction, new Action(this.moveSelectedUp));
    }

    private void stdButtonDown_Click(object sender, EventArgs e)
    {
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction, new Action(this.moveSelectedDown));
    }

    private void stdButtonNew_Click(object sender, EventArgs e)
    {
      DataTableFieldValues dtFieldValues = this.openFieldValueDlg(-1, 0);
      if (dtFieldValues == null)
        return;
      this.addNewRowAfterCheckStatus(DataTableSetupDlg.NewRowPosition.Bottom, -1);
      GVItem selectedItem = this.getSelectedItem();
      this.updateFieldValue(selectedItem.Index, selectedItem.Index, dtFieldValues);
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction);
    }

    private void stdButtonDuplicate_Click(object sender, EventArgs e)
    {
      this.duplicateSelectedRow();
    }

    private void stdButtonDelete_Click(object sender, EventArgs e)
    {
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction, new Action(this.deleteTableSetupItem));
    }

    private void handleSubItemClick(int itemIndex, int rowIdx, GVSubItem subItem)
    {
      DataTableFieldValues dtFieldValues = this.openFieldValueDlg(rowIdx, subItem.Index);
      if (dtFieldValues == null)
        return;
      this.updateFieldValue(itemIndex, rowIdx, dtFieldValues);
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction);
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.LeaveForm);
    }

    private void stdButtonSave_Click(object sender, EventArgs e)
    {
      this.saveAllChangeAfterCheckStatus();
    }

    private void stdButtonReset_Click(object sender, EventArgs e)
    {
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction, new Action(this.resetTableSetup));
    }

    private void btnModifyDataTable_Click(object sender, EventArgs e)
    {
      if (!this.openDataTableFieldsDlg())
        return;
      this.saveAllChanges();
      this._formDataSource = new DataTableSetupDlg.FormDataSource(this.fetchDataTable(this.tableID), this._session);
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.FieldsChange);
    }

    private DDMDataTable fetchDataTable(int dataTableId)
    {
      return ((DDMDataTableBpmManager) this._session.BPM.GetBpmManager(BpmCategory.DDMDataTables)).GetDDMDataTableAndFieldValues(dataTableId, true);
    }

    private void initHeaderTexts(string dataTableName = null)
    {
      this.gcDataTableSetup.Text = "Data Table : " + this._formDataSource.DDMDataTable.Name;
    }

    private void initGridViewColumns()
    {
      this.gvDataTableSetup.Items.Clear();
      this.gvDataTableSetup.Columns.Clear();
      if (this._formDataSource.DDMDataTableInfo == null || this._formDataSource.DDMDataTableInfo.Fields == null)
        return;
      foreach (DDMDataTableFieldInfo dataTableFieldInfo in ((IEnumerable<DDMDataTableFieldInfo>) this._formDataSource.DDMDataTableInfo.Fields).Where<DDMDataTableFieldInfo>((System.Func<DDMDataTableFieldInfo, bool>) (f => !f.IsOutput)).Select<DDMDataTableFieldInfo, DDMDataTableFieldInfo>((System.Func<DDMDataTableFieldInfo, DDMDataTableFieldInfo>) (f => f)))
      {
        FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(dataTableFieldInfo.FieldId);
        string strColumnName;
        if (fieldPairInfo.PairIndex > 0)
        {
          DDMField ddmField = new DDMField()
          {
            ComortgagorPair = fieldPairInfo.PairIndex,
            FieldId = fieldPairInfo.FieldID
          };
          strColumnName = string.Format("{0} ({1} Pair) - {2}", (object) ddmField.FieldId, (object) ddmField.PairText, (object) dataTableFieldInfo.Description);
        }
        else
          strColumnName = dataTableFieldInfo.FieldId + " - " + dataTableFieldInfo.Description;
        this.gvDataTableSetup.Columns.Add(strColumnName, 150, ContentAlignment.MiddleLeft);
      }
      IEnumerable<DDMDataTableFieldInfo> dataTableFieldInfos = ((IEnumerable<DDMDataTableFieldInfo>) this._formDataSource.DDMDataTableInfo.Fields).Where<DDMDataTableFieldInfo>((System.Func<DDMDataTableFieldInfo, bool>) (f => f.IsOutput)).Select<DDMDataTableFieldInfo, DDMDataTableFieldInfo>((System.Func<DDMDataTableFieldInfo, DDMDataTableFieldInfo>) (f => f));
      int num = 0;
      foreach (DDMDataTableFieldInfo dataTableFieldInfo in dataTableFieldInfos)
        this.gvDataTableSetup.Columns.Add(string.IsNullOrEmpty(dataTableFieldInfo.FieldId) ? string.Format("{0} {1}", (object) "Output", (object) ++num) : dataTableFieldInfo.FieldId, 150, ContentAlignment.MiddleLeft);
    }

    private void initGridViewData(bool addNewIfNone = false)
    {
      DataTableSetupDlg.RowItemStatus[] tableFieldValues = this._formDataSource.GetDDMDataTableFieldValues();
      if (tableFieldValues != null && tableFieldValues.Length != 0)
      {
        int length1 = tableFieldValues.Length;
        int length2 = this._formDataSource.DDMDataTableInfo.Fields.Length;
        for (int index = 0; index < length1; ++index)
          this.addExistingRow(tableFieldValues[index]);
      }
      else
      {
        if (!addNewIfNone)
          return;
        this.addNewRow(DataTableSetupDlg.NewRowPosition.Bottom, -1)();
      }
    }

    private void handleBuddyButtonClickEvent(object sender, EventArgs e)
    {
      DataTableSetupDlg.ControlPosition tag = (DataTableSetupDlg.ControlPosition) ((Control) sender).Tag;
      int row = tag.Row;
      int column = tag.Column;
      int num = row;
      this.highlightRow(num);
      this.handleSubItemClick(num, row, this.gvDataTableSetup.Items[row].SubItems[column]);
    }

    private void panelClickEvent(object sender, EventArgs e)
    {
      if (!(sender is Panel panel))
        return;
      this.highlightRow(((DataTableSetupDlg.ControlPosition) panel.Tag).Row);
    }

    public void saveAllChangeAfterCheckStatus()
    {
      List<int> emptyOutputRows;
      switch (this.dialogUserOnHandlingEmptyOutput(out emptyOutputRows))
      {
        case DialogResult.OK:
          List<GVItem> gvItemList = new List<GVItem>();
          foreach (int nItemIndex in emptyOutputRows)
            gvItemList.Add(this.gvDataTableSetup.Items[nItemIndex]);
          using (List<GVItem>.Enumerator enumerator = gvItemList.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.deleteTableSetupItem(enumerator.Current);
            break;
          }
        case DialogResult.Cancel:
          if (emptyOutputRows == null || emptyOutputRows.Count <= 0)
            return;
          this.highlightRow(emptyOutputRows[0]);
          return;
      }
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction, new Action(this.saveAllChanges));
      this.changedRowIndex = -1;
    }

    private void duplicateSelectedRow()
    {
      GVItem selectedItem = this.getSelectedItem();
      if (selectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a row to duplicate first.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.addNewRowAfterCheckStatus(DataTableSetupDlg.NewRowPosition.Below, selectedItem.Index, selectedItem);
    }

    private void addNewRowAfterCheckStatus(
      DataTableSetupDlg.NewRowPosition newRowPos,
      int selectedItemIndex,
      GVItem originalItem = null)
    {
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction, this.addNewRow(newRowPos, selectedItemIndex, originalItem));
    }

    private Action addNewRow(
      DataTableSetupDlg.NewRowPosition newRowPos,
      int selectedRowIndex,
      GVItem original = null)
    {
      return (Action) (() =>
      {
        if (this._formDataSource.DDMDataTableInfo.Fields == null || this._formDataSource.DDMDataTableInfo.Fields.Length == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please modify the field definition first", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int count = this.gvDataTableSetup.Items.Count;
          int newRowId = 0;
          switch (newRowPos)
          {
            case DataTableSetupDlg.NewRowPosition.Top:
              newRowId = 0;
              break;
            case DataTableSetupDlg.NewRowPosition.Bottom:
              newRowId = count;
              break;
            case DataTableSetupDlg.NewRowPosition.Above:
              newRowId = selectedRowIndex;
              break;
            case DataTableSetupDlg.NewRowPosition.Below:
              newRowId = selectedRowIndex + 1;
              break;
            default:
              newRowId = count;
              break;
          }
          DataTableSetupDlg.RowItemStatus rowItemStatus = new DataTableSetupDlg.RowItemStatus();
          rowItemStatus.RowIndex = newRowId;
          rowItemStatus.DataTableFieldValues = this._formDataSource.GetNewDataTableFieldValues(newRowId);
          GVItem target = new GVItem();
          target.Tag = (object) rowItemStatus;
          for (int index = 0; index < this.gvDataTableSetup.Columns.Count; ++index)
          {
            Panel panel = new Panel();
            panel.Tag = (object) new DataTableSetupDlg.ControlPosition()
            {
              Row = rowItemStatus.RowIndex,
              Column = index
            };
            Label child1 = new Label();
            child1.Text = rowItemStatus.DataTableFieldValues[index].Values;
            child1.Tag = (object) new DataTableSetupDlg.ControlPosition()
            {
              Row = rowItemStatus.RowIndex,
              Column = index
            };
            child1.Width = this.gvDataTableSetup.Columns[index].Width / 2;
            child1.AutoSize = true;
            child1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            DataTableSetupDlg.BuddyButton child2 = new DataTableSetupDlg.BuddyButton();
            child2.UseVisualStyleBackColor = true;
            child2.ForeColor = SystemColors.ControlText;
            child2.Width = 20;
            child2.Tag = (object) new DataTableSetupDlg.ControlPosition()
            {
              Row = rowItemStatus.RowIndex,
              Column = index
            };
            child2.Dock = DockStyle.Right;
            child2.Click += new EventHandler(this.handleBuddyButtonClickEvent);
            child2.Refresh();
            panel.SuspendLayout();
            panel.Controls.Add((Control) child1);
            panel.Controls.Add((Control) child2);
            panel.Controls.SetChildIndex((Control) child2, 0);
            panel.Controls.SetChildIndex((Control) child1, 1);
            panel.ResumeLayout();
            target.SubItems[index].Value = (object) panel;
          }
          if (original != null)
            this.copyValueOfRow(original, target);
          if (count > newRowId)
          {
            foreach (GVItem gvItem in this.gvDataTableSetup.Items.Where<GVItem>((System.Func<GVItem, bool>) (i => ((DataTableSetupDlg.RowItemStatus) i.Tag).RowIndex >= newRowId)).Select<GVItem, GVItem>((System.Func<GVItem, GVItem>) (gi => gi)).ToArray<GVItem>())
            {
              ++((DataTableSetupDlg.RowItemStatus) gvItem.Tag).RowIndex;
              foreach (GVSubItem subItem in (IEnumerable<GVSubItem>) gvItem.SubItems)
              {
                object obj = subItem.Value;
                if (obj is Panel)
                {
                  foreach (Control control in (ArrangedElementCollection) ((Control) obj).Controls)
                  {
                    if (control.Tag is DataTableSetupDlg.ControlPosition tag2)
                      ++tag2.Row;
                  }
                }
              }
            }
          }
          switch (newRowPos)
          {
            case DataTableSetupDlg.NewRowPosition.Top:
              this.gvDataTableSetup.Items.Insert(0, target);
              break;
            case DataTableSetupDlg.NewRowPosition.Bottom:
              this.gvDataTableSetup.Items.Add(target);
              break;
            case DataTableSetupDlg.NewRowPosition.Above:
              this.gvDataTableSetup.Items.Insert(selectedRowIndex, target);
              break;
            case DataTableSetupDlg.NewRowPosition.Below:
              if (selectedRowIndex == count)
              {
                this.gvDataTableSetup.Items.Add(target);
                break;
              }
              this.gvDataTableSetup.Items.Insert(selectedRowIndex + 1, target);
              break;
            default:
              newRowId = count;
              break;
          }
          for (int index = 0; index < this.gvDataTableSetup.Items.Count; ++index)
          {
            if (this.gvDataTableSetup.Items[index].Selected)
            {
              this.gvDataTableSetup.Items[index].Selected = false;
              this.hideBuddyButtonForRow(index);
              this.lastSelectIndex = -1;
              break;
            }
          }
          this._formDataSource.AddNewRow(newRowId, original);
          this.changedRowIndex = newRowId;
          this.lastSelectIndex = newRowId;
          this.highlightRow(newRowId);
        }
      });
    }

    private void copyValueOfRow(GVItem source, GVItem target)
    {
      for (int nItemIndex = 0; nItemIndex < source.SubItems.Count; ++nItemIndex)
      {
        GVSubItem subItem1 = source.SubItems[nItemIndex];
        string str = !(subItem1.Value is Panel) ? Convert.ToString(subItem1.Value) : ((subItem1.Value as Panel).Controls[1] as Label).Text;
        GVSubItem subItem2 = target.SubItems[nItemIndex];
        if (subItem2.Value is Panel)
          ((subItem2.Value as Panel).Controls[1] as Label).Text = str;
        else
          subItem2.Value = (object) str;
      }
    }

    private string[] getValuesOfRow(GVItem source)
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < source.SubItems.Count; ++index)
      {
        GVSubItem subItem = source.SubItems[0];
        string str = !(subItem.Value is Panel) ? Convert.ToString(subItem.Value) : ((subItem.Value as Panel).Controls[1] as Label).Text;
        stringList.Add(str);
      }
      return stringList.ToArray();
    }

    private void addExistingRow(DataTableSetupDlg.RowItemStatus rowItem)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) rowItem;
      for (int nItemIndex = 0; nItemIndex < this.gvDataTableSetup.Columns.Count; ++nItemIndex)
      {
        DDMCriteria criteria = (DDMCriteria) rowItem.DataTableFieldValues[nItemIndex].Criteria;
        string str = this.ConvertCriteriaTextToSymbol(criteria);
        if (criteria == DDMCriteria.CheckBox && rowItem.DataTableFieldValues[nItemIndex].Values.Contains<char>('|'))
          str = string.Empty;
        gvItem.SubItems[nItemIndex].Value = (object) string.Format("{0}{1}", string.IsNullOrEmpty(str) ? (object) string.Empty : (object) (str + " "), (object) rowItem.DataTableFieldValues[nItemIndex].Values);
      }
      this.gvDataTableSetup.Items.Add(gvItem);
    }

    private void updateItemPosition(int deletedRowIndex)
    {
      for (int nItemIndex = 0; nItemIndex < this.gvDataTableSetup.Columns.Count; ++nItemIndex)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDataTableSetup.Items)
        {
          if (gvItem.SubItems[nItemIndex].Value is Panel panel)
          {
            DataTableSetupDlg.ControlPosition tag = panel.Tag as DataTableSetupDlg.ControlPosition;
            if (tag.Row > deletedRowIndex)
            {
              --tag.Row;
              foreach (Control control in (ArrangedElementCollection) panel.Controls)
                --(control.Tag as DataTableSetupDlg.ControlPosition).Row;
            }
          }
        }
      }
    }

    private void moveSelectedUp()
    {
      GVItem selectedItem = this.getSelectedItem();
      if (selectedItem == null || selectedItem.Index == 0)
        return;
      int upper = selectedItem.Index - 1;
      int index = selectedItem.Index;
      this.changedRowIndex = upper;
      this.swapRow(selectedItem.Index - 1, selectedItem.Index, false);
      this.highlightRow(selectedItem.Index);
      this._formDataSource.SwapRow(upper, index);
      this.lastSelectIndex = upper;
    }

    private void moveSelectedDown()
    {
      GVItem selectedItem = this.getSelectedItem();
      if (selectedItem == null || selectedItem.Index == this.gvDataTableSetup.Items.Count)
        return;
      int index = selectedItem.Index;
      int lower = selectedItem.Index + 1;
      this.changedRowIndex = lower;
      this.swapRow(selectedItem.Index, selectedItem.Index + 1, true);
      this.highlightRow(selectedItem.Index);
      this._formDataSource.SwapRow(index, lower);
      this.lastSelectIndex = lower;
    }

    private void swapRow(int upper, int lower, bool isMovingDown)
    {
      GVItem gvItem1 = this.gvDataTableSetup.Items[upper];
      GVItem gvItem2 = this.gvDataTableSetup.Items[lower];
      int nItemIndex = lower - 1;
      this.gvDataTableSetup.Items.RemoveAt(upper);
      this.gvDataTableSetup.Items.RemoveAt(nItemIndex);
      if (isMovingDown)
        this.updateControlPosition(gvItem1);
      else
        this.updateControlPosition(gvItem2, false);
      this.gvDataTableSetup.Items.Insert(upper, gvItem2);
      this.gvDataTableSetup.Items.Insert(lower, gvItem1);
    }

    private void updateControlPosition(GVItem item, bool isMovingDown = true)
    {
      for (int nItemIndex = 0; nItemIndex < this.gvDataTableSetup.Columns.Count; ++nItemIndex)
      {
        Panel panel = item.SubItems[nItemIndex].Value as Panel;
        DataTableSetupDlg.ControlPosition tag1 = panel.Tag as DataTableSetupDlg.ControlPosition;
        if (isMovingDown)
          ++tag1.Row;
        else
          --tag1.Row;
        foreach (Control control in (ArrangedElementCollection) panel.Controls)
        {
          DataTableSetupDlg.ControlPosition tag2 = control.Tag as DataTableSetupDlg.ControlPosition;
          if (isMovingDown)
            ++tag2.Row;
          else
            --tag2.Row;
        }
      }
    }

    private void highlightRow(int idx)
    {
      int count = this.gvDataTableSetup.Items.Count;
      if (idx < 0 || count - 1 < idx)
        return;
      this.gvDataTableSetup.Items[idx].Selected = true;
      this.gvDataTableSetup.EnsureVisible(idx);
      this.gvDataTableSetup.Focus();
    }

    private GVItem getSelectedItem()
    {
      return this.gvDataTableSetup.SelectedItems.Count > 0 ? this.gvDataTableSetup.SelectedItems[0] : (GVItem) null;
    }

    private bool openDataTableFieldsDlg()
    {
      bool flag = false;
      if (this.cachedStandardFields == null)
        this.cachedStandardFields = Session.LoanManager.GetStandardFields();
      if (this.cachedFieldSettings == null)
        this.cachedFieldSettings = Session.LoanManager.GetFieldSettings();
      using (DataTableDlg dataTableDlg = new DataTableDlg(this._formDataSource.DDMDataTableInfo, this.cachedStandardFields, this.cachedFieldSettings))
      {
        DDMDataTableInfo ddmDataTableInfo1 = this.copyDdmTableFieldInfo(this._formDataSource.DDMDataTableInfo);
        int num = (int) dataTableDlg.ShowDialog((IWin32Window) this);
        DDMDataTableInfo ddmDataTableInfo2 = dataTableDlg.DdmDataTableInfo;
        if (ddmDataTableInfo2 != null)
        {
          if (this.checkIfAnyHeaderChanged(ddmDataTableInfo1, ddmDataTableInfo2))
          {
            flag = true;
            this._formDataSource.ChangeHeader(ddmDataTableInfo2);
          }
          if (this.checkIfAnyFieldChanged(ddmDataTableInfo1, ddmDataTableInfo2))
          {
            flag = true;
            this._formDataSource.ChangeFields(ddmDataTableInfo2, ddmDataTableInfo1);
            this._formDataSource.CleanUpEmptyRows();
          }
        }
        else
          this._formDataSource.DDMDataTableInfo = ddmDataTableInfo1;
      }
      return flag;
    }

    private DataTableFieldValues openFieldValueDlg(int rowIdx, int selFieldIdx)
    {
      DataTableFieldValues dtFieldValues = new DataTableFieldValues();
      List<DataTableField> dataTableFieldList = new List<DataTableField>();
      DDMDataTableInfo ddmDataTableInfo = this._formDataSource.DDMDataTableInfo;
      dtFieldValues.CurrentItemIndex = selFieldIdx;
      DDMDataTableFieldValue[] tableFieldValues = this._formDataSource.GetDDMDataTableFieldValues(rowIdx);
      for (int index = 0; index < ((IEnumerable<DDMDataTableFieldInfo>) ddmDataTableInfo.Fields).Count<DDMDataTableFieldInfo>(); ++index)
      {
        DDMDataTableFieldValue dataTableFieldValue = rowIdx < 0 ? (DDMDataTableFieldValue) null : tableFieldValues[index];
        bool isOutput = ddmDataTableInfo.Fields[index].IsOutput;
        FieldValueBase fieldValueBase = !isOutput ? (rowIdx < 0 ? FeeValueDlg.GenerateFieldValueClass(ddmDataTableInfo.Fields[index].FieldId, DDMCriteria.IgnoreValueInLoanFile, ddmDataTableInfo.Fields[index].Format, string.Empty) : FeeValueDlg.GenerateFieldValueClass(dataTableFieldValue.FieldId, (DDMCriteria) dataTableFieldValue.Criteria, ddmDataTableInfo.Fields[index].Format, dataTableFieldValue.Values)) : (rowIdx < 0 ? FeeValueDlg.GenerateFieldValueClass(string.Empty, DDMCriteria.none, FieldFormat.NONE, string.Empty) : FeeValueDlg.GenerateFieldValueClass(dataTableFieldValue.FieldId, (DDMCriteria) dataTableFieldValue.Criteria, FieldFormat.NONE, dataTableFieldValue.Values));
        dataTableFieldList.Add(new DataTableField()
        {
          Description = ddmDataTableInfo.Fields[index].Description,
          FieldID = ddmDataTableInfo.Fields[index].FieldId,
          rCriteria = isOutput ? ReportingDatabaseColumnType.Text : ddmDataTableInfo.Fields[index].Type,
          FieldType = isOutput ? FieldFormat.NONE : ddmDataTableInfo.Fields[index].Format,
          IsOutput = isOutput,
          Value = fieldValueBase
        });
      }
      dtFieldValues.DataTableFields = dataTableFieldList.ToArray();
      using (FeeValueDlg feeValueDlg = new FeeValueDlg(dtFieldValues, this._session))
      {
        feeValueDlg.ShowInTaskbar = false;
        int num = (int) feeValueDlg.ShowDialog((IWin32Window) this);
        if (feeValueDlg.DialogResult == DialogResult.OK)
        {
          if (rowIdx == -1 && feeValueDlg.deleteEmptyRowInDataTable)
            return (DataTableFieldValues) null;
          if (feeValueDlg.deleteEmptyRowInDataTable)
          {
            this._formDataSource.DeleteRow(rowIdx);
            this.gvDataTableSetup.Items.Remove(this.gvDataTableSetup.Items[rowIdx]);
            this.lastSelectIndex = -1;
            return (DataTableFieldValues) null;
          }
          if (feeValueDlg.IsDirty)
            this.isDirty = true;
          return feeValueDlg.DtFieldValues;
        }
      }
      return (DataTableFieldValues) null;
    }

    private string ConvertCriteriaTextToSymbol(DDMCriteria criteria)
    {
      string empty = string.Empty;
      string symbol;
      switch (criteria)
      {
        case DDMCriteria.none:
        case DDMCriteria.OP_SpecificValue:
        case DDMCriteria.OP_AdvancedCoding:
        case DDMCriteria.IgnoreValueInLoanFile:
          symbol = "";
          break;
        case DDMCriteria.Equals:
          symbol = "=";
          break;
        case DDMCriteria.LessThan:
          symbol = "<";
          break;
        case DDMCriteria.LessThanOrEqual:
          symbol = "<=";
          break;
        case DDMCriteria.GreaterThan:
          symbol = ">";
          break;
        case DDMCriteria.GreaterThanOrEqual:
          symbol = ">=";
          break;
        case DDMCriteria.NotEqual:
          symbol = "<>";
          break;
        case DDMCriteria.CheckBox:
        case DDMCriteria.SSN_SpecificValue:
        case DDMCriteria.strEquals:
        case DDMCriteria.zip_SpecificValue:
        case DDMCriteria.zip_FindByZip:
        case DDMCriteria.county_SpecificValue:
        case DDMCriteria.county_FindByCounty:
          symbol = "=";
          break;
        case DDMCriteria.SSN_ListofValues:
        case DDMCriteria.ListOfValues:
        case DDMCriteria.st_ListOfValues:
          symbol = "";
          break;
        case DDMCriteria.strNotEqual:
          symbol = "<>";
          break;
        case DDMCriteria.strContains:
          symbol = "Contains";
          break;
        case DDMCriteria.strNotContains:
          symbol = "DoesNotContain";
          break;
        case DDMCriteria.strBegins:
          symbol = "BeginsWith";
          break;
        case DDMCriteria.strEnds:
          symbol = "EndsWith";
          break;
        case DDMCriteria.NoValueInLoanFile:
          symbol = "No value in loan file";
          break;
        case DDMCriteria.OP_ClearValueInLoanFile:
          symbol = "Clear value in Loan File";
          break;
        default:
          symbol = criteria.ToString();
          break;
      }
      return symbol;
    }

    private void updateFieldValue(int itemIndex, int rowIdx, DataTableFieldValues dtFieldValues)
    {
      GVItem gvItem = this.gvDataTableSetup.Items[itemIndex];
      this.changedRowIndex = itemIndex;
      for (int nItemIndex = 0; nItemIndex < dtFieldValues.DataTableFields.Length; ++nItemIndex)
      {
        gvItem.SubItems[nItemIndex].Tag = (object) dtFieldValues.DataTableFields[nItemIndex].Value;
        string str1;
        if (dtFieldValues.DataTableFields[nItemIndex].Value == null)
        {
          str1 = (string) null;
        }
        else
        {
          DDMCriteria criteria = dtFieldValues.DataTableFields[nItemIndex].Value.Criteria;
          string str2 = this.ConvertCriteriaTextToSymbol(criteria);
          if (criteria == DDMCriteria.CheckBox && dtFieldValues.DataTableFields[nItemIndex].Value.ToString().Contains<char>('|'))
            str2 = string.Empty;
          str1 = string.Format("{0}{1}", string.IsNullOrEmpty(str2) ? (object) string.Empty : (object) (str2 + " "), (object) dtFieldValues.DataTableFields[nItemIndex].Value.ToString());
        }
        foreach (Control control in (ArrangedElementCollection) ((Control) gvItem.SubItems[nItemIndex].Value).Controls)
        {
          if (control.GetType() == typeof (Label))
            control.Text = str1;
        }
      }
      this._formDataSource.UpdateFieldValue(rowIdx, dtFieldValues);
    }

    private bool checkIfAnyHeaderChanged(
      DDMDataTableInfo originalInfo,
      DDMDataTableInfo updatedInfo)
    {
      return !updatedInfo.Name.Equals(originalInfo.Name) || !updatedInfo.Description.Equals(originalInfo.Description);
    }

    private bool checkIfAnyFieldChanged(DDMDataTableInfo originalInfo, DDMDataTableInfo updatedInfo)
    {
      bool flag = false;
      if (updatedInfo.Fields.Length != originalInfo.Fields.Length)
      {
        flag = true;
      }
      else
      {
        for (int index = 0; index < updatedInfo.Fields.Length; ++index)
        {
          if (!updatedInfo.Fields[index].FieldId.Equals(originalInfo.Fields[index].FieldId))
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    public bool checkIfAnyUserChange()
    {
      return this._formDataSource.ChangesOnForm.IsAnyChange() || this.isDirty;
    }

    private void handleUpDownButtons()
    {
      GVItem selectedItem = this.getSelectedItem();
      if (this.gvDataTableSetup.SelectedItems.Count == 0 || this.gvDataTableSetup.Items.Count == 1)
        this.stdButtonDown.Enabled = this.stdButtonUp.Enabled = false;
      else if (selectedItem != null && selectedItem.Index == 0)
      {
        this.stdButtonDown.Enabled = true;
        this.stdButtonUp.Enabled = false;
      }
      else if (selectedItem != null && selectedItem.Index == this.gvDataTableSetup.Items.Count - 1)
      {
        this.stdButtonUp.Enabled = true;
        this.stdButtonDown.Enabled = false;
      }
      else
        this.stdButtonDown.Enabled = this.stdButtonUp.Enabled = true;
    }

    private void handleSelectedItemDependents()
    {
      this.stdButtonDuplicate.Enabled = this.gvDataTableSetup.SelectedItems.Count > 0;
      this.stdButtonDelete.Enabled = this.gvDataTableSetup.SelectedItems.Count > 0;
    }

    private void handleSaveResetButtons()
    {
      if (this.checkIfAnyUserChange())
        this.stdButtonSave.Enabled = this.stdButtonReset.Enabled = true;
      else
        this.stdButtonSave.Enabled = this.stdButtonReset.Enabled = false;
      this.isDirty = false;
    }

    private void saveAllChanges()
    {
      this._formDataSource.CommitChanges(this._session);
      this.tableID = this._formDataSource.DDMDataTable.Id;
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction);
    }

    private DDMDataTable convertToDataTableObject(DDMDataTableInfo dataTableInfo, int existingId = -1)
    {
      DDMDataTable dataTableObject = new DDMDataTable(existingId, dataTableInfo.Name, dataTableInfo.Description, DateTime.Now.ToString(), this._session.UserID, this._session.UserInfo.firstLastName, "", dataTableInfo.GetFieldIdListString(), dataTableInfo.GetOutputIdListString());
      dataTableObject.FieldValues = new Dictionary<int, List<DDMDataTableFieldValue>>();
      int key = 0;
      dataTableObject.FieldValues.Add(key, new List<DDMDataTableFieldValue>());
      foreach (DDMDataTableFieldInfo field in dataTableInfo.Fields)
        dataTableObject.FieldValues[key].Add(new DDMDataTableFieldValue()
        {
          Id = -1,
          FieldId = field.FieldId,
          Criteria = -1,
          RowId = key
        });
      return dataTableObject;
    }

    public void resetTableSetup() => this.refreshGrid();

    public void deleteTableSetupItem()
    {
      GVItem selectedItem = this.getSelectedItem();
      if (selectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a row to delete.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "The selected data row will be deleted.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
          return;
        this._formDataSource.DeleteRow(selectedItem.Index);
        this.gvDataTableSetup.Items.Remove(selectedItem);
        this.lastSelectIndex = -1;
      }
    }

    public void deleteTableSetupItem(GVItem item)
    {
      GVItem gvItem = item;
      if (gvItem == null)
        return;
      this._formDataSource.DeleteRow(gvItem.Index);
      this.gvDataTableSetup.Items.Remove(gvItem);
    }

    public DDMDataTableInfo copyDdmTableFieldInfo(DDMDataTableInfo tableInfo)
    {
      DDMDataTableInfo ddmDataTableInfo = new DDMDataTableInfo();
      ddmDataTableInfo.Name = tableInfo.Name;
      ddmDataTableInfo.Description = tableInfo.Description;
      ddmDataTableInfo.Fields = new DDMDataTableFieldInfo[tableInfo.Fields == null ? 0 : tableInfo.Fields.Length];
      int num1 = 0;
      foreach (DDMDataTableFieldInfo dataTableFieldInfo in ((IEnumerable<DDMDataTableFieldInfo>) tableInfo.Fields).Where<DDMDataTableFieldInfo>((System.Func<DDMDataTableFieldInfo, bool>) (f => !f.IsOutput)).Select<DDMDataTableFieldInfo, DDMDataTableFieldInfo>((System.Func<DDMDataTableFieldInfo, DDMDataTableFieldInfo>) (f => f)))
        ddmDataTableInfo.Fields[num1++] = new DDMDataTableFieldInfo(dataTableFieldInfo.FieldId, dataTableFieldInfo.Description, dataTableFieldInfo.Type);
      int num2 = 0;
      foreach (DDMDataTableFieldInfo dataTableFieldInfo in ((IEnumerable<DDMDataTableFieldInfo>) tableInfo.Fields).Where<DDMDataTableFieldInfo>((System.Func<DDMDataTableFieldInfo, bool>) (f => f.IsOutput)).Select<DDMDataTableFieldInfo, DDMDataTableFieldInfo>((System.Func<DDMDataTableFieldInfo, DDMDataTableFieldInfo>) (f => f)))
        ddmDataTableInfo.Fields[num1++] = new DDMDataTableFieldInfo(dataTableFieldInfo.FieldId, outputIdx: num2++);
      return ddmDataTableInfo;
    }

    private bool? promptUserToSaveOrDiscard()
    {
      switch (MessageBox.Show((IWin32Window) this, "There are unsaved changes. Do you want to save them?", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return new bool?();
        case DialogResult.Yes:
          return new bool?(true);
        default:
          return new bool?(false);
      }
    }

    private DialogResult dialogUserOnHandlingEmptyOutput(out List<int> emptyOutputRows)
    {
      emptyOutputRows = this._formDataSource.IsEmptyOutputColumns();
      return emptyOutputRows != null && emptyOutputRows.Count > 0 ? MessageBox.Show((IWin32Window) this, "There are table(s) with empty Output column(s) in the following rows. They will be deleted. Do you want to proceed?" + Environment.NewLine + string.Join<int>(",", (IEnumerable<int>) emptyOutputRows.ToArray()), "Save Changes?", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand) : DialogResult.None;
    }

    private void hideBuddyButtonForRow(int rowIndex)
    {
      if (rowIndex < 0)
        return;
      GVItem gvItem = this.gvDataTableSetup.Items[rowIndex];
      DataTableSetupDlg.RowItemStatus tag = (DataTableSetupDlg.RowItemStatus) gvItem.Tag;
      for (int nItemIndex = 0; nItemIndex < this.gvDataTableSetup.Columns.Count; ++nItemIndex)
      {
        string str = string.Empty;
        if (gvItem.SubItems[nItemIndex].Value is Panel)
        {
          Panel panel = gvItem.SubItems[nItemIndex].Value as Panel;
          foreach (Control control in (ArrangedElementCollection) panel.Controls)
          {
            if (control is DataTableSetupDlg.BuddyButton)
              control.Click -= new EventHandler(this.handleBuddyButtonClickEvent);
            if (control is Label)
              str = control.Text;
          }
          panel.Controls.Clear();
          panel.Click -= new EventHandler(this.panelClickEvent);
          gvItem.SubItems[nItemIndex].Value = (object) str;
        }
      }
    }

    private void showBuddyButtonForRow(int rowIndex)
    {
      int count = this.gvDataTableSetup.Items.Count;
      if (rowIndex < 0 || count - 1 < rowIndex)
        return;
      GVItem gvItem = this.gvDataTableSetup.Items[rowIndex];
      DataTableSetupDlg.RowItemStatus tag = (DataTableSetupDlg.RowItemStatus) gvItem.Tag;
      if (gvItem.SubItems[0].Item.Value.GetType() == typeof (Panel))
        return;
      for (int index = 0; index < this.gvDataTableSetup.Columns.Count; ++index)
      {
        Panel panel = new Panel();
        panel.Click += new EventHandler(this.panelClickEvent);
        panel.Tag = (object) new DataTableSetupDlg.ControlPosition()
        {
          Row = rowIndex,
          Column = index
        };
        Label child1 = new Label();
        child1.Text = gvItem.SubItems[index].Text;
        child1.Tag = (object) new DataTableSetupDlg.ControlPosition()
        {
          Row = rowIndex,
          Column = index
        };
        child1.Width = this.gvDataTableSetup.Columns[index].Width / 2;
        child1.AutoSize = true;
        child1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        DataTableSetupDlg.BuddyButton child2 = new DataTableSetupDlg.BuddyButton();
        child2.UseVisualStyleBackColor = true;
        child2.ForeColor = SystemColors.ControlText;
        child2.Width = 20;
        child2.Tag = (object) new DataTableSetupDlg.ControlPosition()
        {
          Row = rowIndex,
          Column = index
        };
        child2.Dock = DockStyle.Right;
        child2.Click += new EventHandler(this.handleBuddyButtonClickEvent);
        child2.Refresh();
        panel.SuspendLayout();
        panel.Controls.Add((Control) child1);
        panel.Controls.Add((Control) child2);
        panel.Controls.SetChildIndex((Control) child2, 0);
        panel.Controls.SetChildIndex((Control) child1, 1);
        panel.ResumeLayout();
        gvItem.SubItems[index].Value = (object) panel;
        new ToolTip().SetToolTip((Control) child1, child1.Text);
      }
    }

    private void btnRulesUsingThisTable_Click(object sender, EventArgs e)
    {
      using (DataTableRules dataTableRules = new DataTableRules(this._formDataSource.DDMDataTableInfo))
      {
        int num = (int) dataTableRules.ShowDialog((IWin32Window) this);
      }
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      GVItem selectedItem = this.getSelectedItem();
      foreach (ToolStripDropDownItem stripDropDownItem in (ArrangedElementCollection) this.contextMenuStrip1.Items)
      {
        if (stripDropDownItem.Text.ToLower().Equals("add row"))
          stripDropDownItem.Enabled = true;
        else if (selectedItem == null)
          stripDropDownItem.Enabled = false;
        else if (stripDropDownItem.Text.ToLower().Equals("move down"))
          stripDropDownItem.Enabled = selectedItem.Index < this.gvDataTableSetup.Items.Count - 1;
        else if (stripDropDownItem.Text.ToLower().Equals("move up"))
          stripDropDownItem.Enabled = selectedItem.Index > 0;
        else if (stripDropDownItem.Text.ToLower().Equals("duplicate row"))
          stripDropDownItem.Enabled = true;
        else
          stripDropDownItem.Enabled = true;
      }
    }

    private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DataTableFieldValues dtFieldValues = this.openFieldValueDlg(-1, 0);
      if (dtFieldValues == null)
        return;
      this.addNewRowAfterCheckStatus(DataTableSetupDlg.NewRowPosition.Bottom, -1);
      GVItem selectedItem = this.getSelectedItem();
      this.updateFieldValue(selectedItem.Index, selectedItem.Index, dtFieldValues);
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction);
    }

    private void insertRowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      GVItem selectedItem1 = this.getSelectedItem();
      if (selectedItem1 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a row to add the new one above", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        DataTableFieldValues dtFieldValues = this.openFieldValueDlg(-1, 0);
        if (dtFieldValues == null)
          return;
        this.addNewRowAfterCheckStatus(DataTableSetupDlg.NewRowPosition.Above, selectedItem1.Index);
        GVItem selectedItem2 = this.getSelectedItem();
        this.updateFieldValue(selectedItem2.Index, selectedItem2.Index, dtFieldValues);
        this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction);
      }
    }

    private void duplicateRowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.duplicateSelectedRow();
    }

    private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction, new Action(this.deleteTableSetupItem));
    }

    private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction, new Action(this.moveSelectedUp));
    }

    private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this._fsm.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.UserAction, new Action(this.moveSelectedDown));
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Data Table Setup");
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
      this.gcDataTableSetup = new GroupContainer();
      this.stdButtonDuplicate = new StandardIconButton();
      this.btnModifyDataTable = new ButtonEx();
      this.stdButtonSave = new StandardIconButton();
      this.btnRulesUsingThisTable = new ButtonEx();
      this.stdButtonReset = new StandardIconButton();
      this.gvDataTableSetup = new GridView();
      this.stdButtonDelete = new StandardIconButton();
      this.stdButtonUp = new StandardIconButton();
      this.stdButtonDown = new StandardIconButton();
      this.stdButtonNew = new StandardIconButton();
      this.tpValues = new TabPageEx();
      this.btnClearValue = new ButtonEx();
      this.btnSetValue = new ButtonEx();
      this.gvScenarioFeeValues = new GridView();
      this.ttipMessageAltText = new ToolTip(this.components);
      this.btnClose = new ButtonEx();
      this.gradientPanel1 = new GradientPanel();
      this.emHelpLink1 = new EMHelpLink();
      this.lblHeaderText = new FormattedLabel();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.addRowToolStripMenuItem = new ToolStripMenuItem();
      this.insertRowToolStripMenuItem = new ToolStripMenuItem();
      this.duplicateRowToolStripMenuItem = new ToolStripMenuItem();
      this.deleteRowToolStripMenuItem = new ToolStripMenuItem();
      this.moveUpToolStripMenuItem = new ToolStripMenuItem();
      this.moveDownToolStripMenuItem = new ToolStripMenuItem();
      this.verticalSeparator1 = new VerticalSeparator();
      this.gcDataTableSetup.SuspendLayout();
      ((ISupportInitialize) this.stdButtonDuplicate).BeginInit();
      ((ISupportInitialize) this.stdButtonSave).BeginInit();
      ((ISupportInitialize) this.stdButtonReset).BeginInit();
      ((ISupportInitialize) this.stdButtonDelete).BeginInit();
      ((ISupportInitialize) this.stdButtonUp).BeginInit();
      ((ISupportInitialize) this.stdButtonDown).BeginInit();
      ((ISupportInitialize) this.stdButtonNew).BeginInit();
      this.tpValues.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      this.gcDataTableSetup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcDataTableSetup.Controls.Add((Control) this.verticalSeparator1);
      this.gcDataTableSetup.Controls.Add((Control) this.stdButtonDuplicate);
      this.gcDataTableSetup.Controls.Add((Control) this.btnModifyDataTable);
      this.gcDataTableSetup.Controls.Add((Control) this.stdButtonSave);
      this.gcDataTableSetup.Controls.Add((Control) this.btnRulesUsingThisTable);
      this.gcDataTableSetup.Controls.Add((Control) this.stdButtonReset);
      this.gcDataTableSetup.Controls.Add((Control) this.gvDataTableSetup);
      this.gcDataTableSetup.Controls.Add((Control) this.stdButtonDelete);
      this.gcDataTableSetup.Controls.Add((Control) this.stdButtonUp);
      this.gcDataTableSetup.Controls.Add((Control) this.stdButtonDown);
      this.gcDataTableSetup.Controls.Add((Control) this.stdButtonNew);
      this.gcDataTableSetup.HeaderForeColor = SystemColors.ControlText;
      this.gcDataTableSetup.Location = new Point(0, 24);
      this.gcDataTableSetup.Name = "gcDataTableSetup";
      this.gcDataTableSetup.Size = new Size(970, 447);
      this.gcDataTableSetup.TabIndex = 0;
      this.gcDataTableSetup.Text = "Data Table";
      this.stdButtonDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonDuplicate.BackColor = Color.Transparent;
      this.stdButtonDuplicate.Location = new Point(613, 4);
      this.stdButtonDuplicate.MouseDownImage = (Image) null;
      this.stdButtonDuplicate.Name = "stdButtonDuplicate";
      this.stdButtonDuplicate.Size = new Size(16, 16);
      this.stdButtonDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdButtonDuplicate.TabIndex = 15;
      this.stdButtonDuplicate.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonDuplicate, "Duplicate");
      this.stdButtonDuplicate.Click += new EventHandler(this.stdButtonDuplicate_Click);
      this.btnModifyDataTable.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnModifyDataTable.Location = new Point(736, 1);
      this.btnModifyDataTable.Name = "btnModifyDataTable";
      this.btnModifyDataTable.Size = new Size(98, 23);
      this.btnModifyDataTable.TabIndex = 2;
      this.btnModifyDataTable.Text = "Modify Data Table";
      this.btnModifyDataTable.UseVisualStyleBackColor = true;
      this.btnModifyDataTable.Click += new EventHandler(this.btnModifyDataTable_Click);
      this.stdButtonSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonSave.BackColor = Color.Transparent;
      this.stdButtonSave.Enabled = false;
      this.stdButtonSave.Location = new Point(690, 4);
      this.stdButtonSave.MouseDownImage = (Image) null;
      this.stdButtonSave.Name = "stdButtonSave";
      this.stdButtonSave.Size = new Size(16, 16);
      this.stdButtonSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdButtonSave.TabIndex = 14;
      this.stdButtonSave.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonSave, "Save");
      this.stdButtonSave.Click += new EventHandler(this.stdButtonSave_Click);
      this.btnRulesUsingThisTable.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRulesUsingThisTable.Location = new Point(838, 1);
      this.btnRulesUsingThisTable.Name = "btnRulesUsingThisTable";
      this.btnRulesUsingThisTable.Size = new Size(130, 23);
      this.btnRulesUsingThisTable.TabIndex = 3;
      this.btnRulesUsingThisTable.Text = "Scenarios using this table";
      this.btnRulesUsingThisTable.UseVisualStyleBackColor = true;
      this.btnRulesUsingThisTable.Click += new EventHandler(this.btnRulesUsingThisTable_Click);
      this.stdButtonReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonReset.BackColor = Color.Transparent;
      this.stdButtonReset.Enabled = false;
      this.stdButtonReset.Location = new Point(709, 4);
      this.stdButtonReset.MouseDownImage = (Image) null;
      this.stdButtonReset.Name = "stdButtonReset";
      this.stdButtonReset.Size = new Size(16, 16);
      this.stdButtonReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdButtonReset.TabIndex = 13;
      this.stdButtonReset.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonReset, "Reset");
      this.stdButtonReset.Click += new EventHandler(this.stdButtonReset_Click);
      this.gvDataTableSetup.AllowMultiselect = false;
      this.gvDataTableSetup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvDataTableSetup.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Output";
      gvColumn1.Text = "Output";
      gvColumn1.Width = 100;
      this.gvDataTableSetup.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvDataTableSetup.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDataTableSetup.Location = new Point(3, 26);
      this.gvDataTableSetup.Name = "gvDataTableSetup";
      this.gvDataTableSetup.Size = new Size(965, 421);
      this.gvDataTableSetup.TabIndex = 8;
      this.gvDataTableSetup.SelectedIndexChanged += new EventHandler(this.gvDataTableSetup_SelectedIndexChanged);
      this.stdButtonDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonDelete.BackColor = Color.Transparent;
      this.stdButtonDelete.Location = new Point(670, 4);
      this.stdButtonDelete.MouseDownImage = (Image) null;
      this.stdButtonDelete.Name = "stdButtonDelete";
      this.stdButtonDelete.Size = new Size(16, 16);
      this.stdButtonDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdButtonDelete.TabIndex = 5;
      this.stdButtonDelete.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonDelete, "Delete");
      this.stdButtonDelete.Click += new EventHandler(this.stdButtonDelete_Click);
      this.stdButtonUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonUp.BackColor = Color.Transparent;
      this.stdButtonUp.Location = new Point(651, 4);
      this.stdButtonUp.MouseDownImage = (Image) null;
      this.stdButtonUp.Name = "stdButtonUp";
      this.stdButtonUp.Size = new Size(16, 16);
      this.stdButtonUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdButtonUp.TabIndex = 4;
      this.stdButtonUp.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonUp, "Move Up");
      this.stdButtonUp.Click += new EventHandler(this.stdButtonUp_Click);
      this.stdButtonDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonDown.BackColor = Color.Transparent;
      this.stdButtonDown.Location = new Point(632, 4);
      this.stdButtonDown.MouseDownImage = (Image) null;
      this.stdButtonDown.Name = "stdButtonDown";
      this.stdButtonDown.Size = new Size(16, 16);
      this.stdButtonDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdButtonDown.TabIndex = 3;
      this.stdButtonDown.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonDown, "Move Down");
      this.stdButtonDown.Click += new EventHandler(this.stdButtonDown_Click);
      this.stdButtonNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdButtonNew.BackColor = Color.Transparent;
      this.stdButtonNew.Location = new Point(594, 4);
      this.stdButtonNew.MouseDownImage = (Image) null;
      this.stdButtonNew.Name = "stdButtonNew";
      this.stdButtonNew.Size = new Size(16, 16);
      this.stdButtonNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdButtonNew.TabIndex = 0;
      this.stdButtonNew.TabStop = false;
      this.ttipMessageAltText.SetToolTip((Control) this.stdButtonNew, "New");
      this.stdButtonNew.Click += new EventHandler(this.stdButtonNew_Click);
      this.tpValues.BackColor = Color.Transparent;
      this.tpValues.Controls.Add((Control) this.btnClearValue);
      this.tpValues.Controls.Add((Control) this.btnSetValue);
      this.tpValues.Controls.Add((Control) this.gvScenarioFeeValues);
      this.tpValues.Location = new Point(1, 23);
      this.tpValues.Name = "tpValues";
      this.tpValues.TabIndex = 0;
      this.tpValues.TabWidth = 100;
      this.tpValues.Text = "Value";
      this.tpValues.Value = (object) "Value";
      this.btnClearValue.Location = new Point(767, 145);
      this.btnClearValue.Name = "btnClearValue";
      this.btnClearValue.Size = new Size(75, 23);
      this.btnClearValue.TabIndex = 2;
      this.btnClearValue.Text = "Clear Value";
      this.btnClearValue.UseVisualStyleBackColor = true;
      this.btnSetValue.Location = new Point(767, 94);
      this.btnSetValue.Name = "btnSetValue";
      this.btnSetValue.Size = new Size(75, 23);
      this.btnSetValue.TabIndex = 1;
      this.btnSetValue.Text = "Set Value";
      this.btnSetValue.UseVisualStyleBackColor = true;
      this.gvScenarioFeeValues.AllowMultiselect = false;
      this.gvScenarioFeeValues.BorderStyle = BorderStyle.FixedSingle;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Field";
      gvColumn2.Text = "Field";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "fieldID";
      gvColumn3.Text = "Field ID";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "valType";
      gvColumn4.Text = "Value Type";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Value";
      gvColumn5.SpringToFit = true;
      gvColumn5.Text = "Value";
      gvColumn5.Width = 299;
      this.gvScenarioFeeValues.Columns.AddRange(new GVColumn[4]
      {
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvScenarioFeeValues.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvScenarioFeeValues.Location = new Point(7, 0);
      this.gvScenarioFeeValues.Name = "gvScenarioFeeValues";
      this.gvScenarioFeeValues.Size = new Size(751, 342);
      this.gvScenarioFeeValues.TabIndex = 0;
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(889, 10);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 20;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.gradientPanel1.Controls.Add((Control) this.emHelpLink1);
      this.gradientPanel1.Controls.Add((Control) this.btnClose);
      this.gradientPanel1.Dock = DockStyle.Bottom;
      this.gradientPanel1.GradientColor1 = Color.White;
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 472);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(970, 45);
      this.gradientPanel1.TabIndex = 3;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Data Table Setup";
      this.emHelpLink1.Location = new Point(12, 13);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 21;
      this.lblHeaderText.BackColor = Color.Transparent;
      this.lblHeaderText.Location = new Point(3, 5);
      this.lblHeaderText.Name = "lblHeaderText";
      this.lblHeaderText.Size = new Size(887, 16);
      this.lblHeaderText.TabIndex = 4;
      this.lblHeaderText.Text = "Order of data table rows is important. The 'Output' value of the row that first satisfies the loan condition will be assigned to the field using this data table in a fee or field scenario.";
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.addRowToolStripMenuItem,
        (ToolStripItem) this.insertRowToolStripMenuItem,
        (ToolStripItem) this.duplicateRowToolStripMenuItem,
        (ToolStripItem) this.deleteRowToolStripMenuItem,
        (ToolStripItem) this.moveUpToolStripMenuItem,
        (ToolStripItem) this.moveDownToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(148, 136);
      this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
      this.addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
      this.addRowToolStripMenuItem.Size = new Size(147, 22);
      this.addRowToolStripMenuItem.Text = "Add row";
      this.addRowToolStripMenuItem.Click += new EventHandler(this.addRowToolStripMenuItem_Click);
      this.insertRowToolStripMenuItem.Name = "insertRowToolStripMenuItem";
      this.insertRowToolStripMenuItem.Size = new Size(147, 22);
      this.insertRowToolStripMenuItem.Text = "Insert row";
      this.insertRowToolStripMenuItem.Click += new EventHandler(this.insertRowToolStripMenuItem_Click);
      this.duplicateRowToolStripMenuItem.Name = "duplicateRowToolStripMenuItem";
      this.duplicateRowToolStripMenuItem.Size = new Size(147, 22);
      this.duplicateRowToolStripMenuItem.Text = "Duplicate row";
      this.duplicateRowToolStripMenuItem.Click += new EventHandler(this.duplicateRowToolStripMenuItem_Click);
      this.deleteRowToolStripMenuItem.Name = "deleteRowToolStripMenuItem";
      this.deleteRowToolStripMenuItem.Size = new Size(147, 22);
      this.deleteRowToolStripMenuItem.Text = "Delete row";
      this.deleteRowToolStripMenuItem.Click += new EventHandler(this.deleteRowToolStripMenuItem_Click);
      this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
      this.moveUpToolStripMenuItem.Size = new Size(147, 22);
      this.moveUpToolStripMenuItem.Text = "Move up";
      this.moveUpToolStripMenuItem.Click += new EventHandler(this.moveUpToolStripMenuItem_Click);
      this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
      this.moveDownToolStripMenuItem.Size = new Size(147, 22);
      this.moveDownToolStripMenuItem.Text = "Move down";
      this.moveDownToolStripMenuItem.Click += new EventHandler(this.moveDownToolStripMenuItem_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(731, 4);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 16;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(970, 517);
      this.Controls.Add((Control) this.lblHeaderText);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Controls.Add((Control) this.gcDataTableSetup);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (DataTableSetupDlg);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Data Table Setup";
      this.KeyPreview = true;
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.Activated += new EventHandler(this.DataTableListDlg_Activated);
      this.gcDataTableSetup.ResumeLayout(false);
      ((ISupportInitialize) this.stdButtonDuplicate).EndInit();
      ((ISupportInitialize) this.stdButtonSave).EndInit();
      ((ISupportInitialize) this.stdButtonReset).EndInit();
      ((ISupportInitialize) this.stdButtonDelete).EndInit();
      ((ISupportInitialize) this.stdButtonUp).EndInit();
      ((ISupportInitialize) this.stdButtonDown).EndInit();
      ((ISupportInitialize) this.stdButtonNew).EndInit();
      this.tpValues.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum NewRowPosition
    {
      Top,
      Bottom,
      Above,
      Below,
    }

    private class FiniteStateMachine
    {
      private Action[,] fsm;
      private DataTableSetupDlg.FiniteStateMachine.Events currentEvent;
      private DataTableSetupDlg.FiniteStateMachine.States previousState;
      private Action additionalAction;
      private DataTableSetupDlg boundedForm;

      public DataTableSetupDlg.FiniteStateMachine.States State { get; set; }

      public bool CanCloseForm { get; set; }

      public FiniteStateMachine(DataTableSetupDlg form)
      {
        this.boundedForm = form;
        this.State = DataTableSetupDlg.FiniteStateMachine.States.Inactive;
        Action[,] actionArray = new Action[5, 6];
        actionArray[0, 0] = new Action(this.FormOn);
        actionArray[0, 1] = new Action(this.FormOn);
        actionArray[1, 2] = new Action(this.OnUserAction);
        actionArray[1, 3] = new Action(this.FieldChange);
        actionArray[1, 4] = new Action(this.OnLeave);
        actionArray[1, 5] = new Action(this.OnClosing);
        actionArray[2, 2] = new Action(this.OnUserAction);
        actionArray[2, 3] = new Action(this.FieldChange);
        actionArray[2, 4] = new Action(this.OnLeave);
        actionArray[2, 5] = new Action(this.OnClosing);
        actionArray[3, 2] = new Action(this.OnUserAction);
        actionArray[3, 3] = new Action(this.FieldChange);
        actionArray[3, 4] = new Action(this.ConfirmExit);
        actionArray[3, 5] = new Action(this.ConfirmClosing);
        this.fsm = actionArray;
      }

      public void ProcessEvent(
        DataTableSetupDlg.FiniteStateMachine.Events theEvent,
        Action formAction = null)
      {
        this.currentEvent = theEvent;
        this.additionalAction = formAction;
        Action action = this.fsm[(int) this.State, (int) theEvent];
        if (action == null)
          return;
        action();
      }

      private void FormOn()
      {
        if (this.currentEvent == DataTableSetupDlg.FiniteStateMachine.Events.OpenWithExisting)
          this.changeState(DataTableSetupDlg.FiniteStateMachine.States.ActiveByExisting);
        else if (this.currentEvent == DataTableSetupDlg.FiniteStateMachine.Events.OpenWithNew)
          this.changeState(DataTableSetupDlg.FiniteStateMachine.States.ActiveByNew);
        string name = this.boundedForm._formDataSource.DDMDataTableInfo.Name;
        if (this.State == DataTableSetupDlg.FiniteStateMachine.States.ActiveByNew)
        {
          this.boundedForm.initHeaderTexts(name);
          this.boundedForm.initGridViewColumns();
          this.boundedForm.showUserInputPopup = true;
        }
        else if (this.State == DataTableSetupDlg.FiniteStateMachine.States.ActiveByExisting)
        {
          this.boundedForm.initHeaderTexts(name);
          this.boundedForm.initGridViewColumns();
          this.boundedForm.initGridViewData();
          this.boundedForm.showUserInputPopup = false;
        }
        this.boundedForm.handleUpDownButtons();
        this.boundedForm.handleSaveResetButtons();
        this.boundedForm.handleSelectedItemDependents();
      }

      private void OnUserAction()
      {
        if (this.State == DataTableSetupDlg.FiniteStateMachine.States.StagedToExit)
          this.changeState(this.previousState);
        if (this.additionalAction != null)
          this.additionalAction();
        this.boundedForm.handleUpDownButtons();
        this.boundedForm.handleSaveResetButtons();
        this.boundedForm.handleSelectedItemDependents();
      }

      private void FieldChange()
      {
        if (this.State == DataTableSetupDlg.FiniteStateMachine.States.StagedToExit)
          this.changeState(this.previousState);
        this.boundedForm.initHeaderTexts(this.boundedForm._formDataSource.DDMDataTableInfo.Name);
        this.boundedForm.initGridViewColumns();
        this.boundedForm.initGridViewData();
        this.boundedForm.handleUpDownButtons();
        this.boundedForm.handleSaveResetButtons();
        this.boundedForm.handleSelectedItemDependents();
      }

      private void OnLeave()
      {
        this.CanCloseForm = false;
        if (this.boundedForm.checkIfAnyUserChange())
        {
          bool? saveOrDiscard = this.boundedForm.promptUserToSaveOrDiscard();
          if (!saveOrDiscard.HasValue)
          {
            if (this.boundedForm.lastSelectIndex <= -1)
              return;
            this.boundedForm.highlightRow(this.boundedForm.lastSelectIndex);
          }
          else
          {
            this.changeState(DataTableSetupDlg.FiniteStateMachine.States.StagedToExit);
            if (saveOrDiscard.Value)
              this.boundedForm.saveAllChanges();
            this.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.LeaveForm);
          }
        }
        else
        {
          this.changeState(DataTableSetupDlg.FiniteStateMachine.States.StagedToExit);
          this.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.LeaveForm);
        }
      }

      private void OnClosing()
      {
        this.CanCloseForm = false;
        if (this.boundedForm.checkIfAnyUserChange())
        {
          bool? saveOrDiscard = this.boundedForm.promptUserToSaveOrDiscard();
          if (!saveOrDiscard.HasValue)
          {
            if (this.boundedForm.lastSelectIndex <= -1)
              return;
            this.boundedForm.highlightRow(this.boundedForm.lastSelectIndex);
          }
          else
          {
            this.changeState(DataTableSetupDlg.FiniteStateMachine.States.StagedToExit);
            if (saveOrDiscard.Value)
              this.boundedForm.saveAllChanges();
            this.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.FormClosing);
          }
        }
        else
        {
          this.changeState(DataTableSetupDlg.FiniteStateMachine.States.StagedToExit);
          this.ProcessEvent(DataTableSetupDlg.FiniteStateMachine.Events.FormClosing);
        }
      }

      private void ConfirmExit()
      {
        this.changeState(DataTableSetupDlg.FiniteStateMachine.States.Exited);
        this.CanCloseForm = true;
        this.boundedForm.Close();
        this.boundedForm = (DataTableSetupDlg) null;
      }

      private void ConfirmClosing()
      {
        this.changeState(DataTableSetupDlg.FiniteStateMachine.States.Exited);
        this.CanCloseForm = true;
        this.boundedForm = (DataTableSetupDlg) null;
      }

      private void changeState(
        DataTableSetupDlg.FiniteStateMachine.States newState)
      {
        this.previousState = this.State;
        this.State = newState;
      }

      public enum States
      {
        Inactive,
        ActiveByNew,
        ActiveByExisting,
        StagedToExit,
        Exited,
      }

      public enum Events
      {
        OpenWithNew,
        OpenWithExisting,
        UserAction,
        FieldsChange,
        LeaveForm,
        FormClosing,
      }
    }

    private class ControlPosition
    {
      public int Row;
      public int Column;
    }

    private enum RowLook
    {
      Default,
      TextBox,
    }

    private class RowItemStatus
    {
      public int RowIndex;
      public DDMDataTableFieldValue[] DataTableFieldValues;
    }

    private class ChangesOnForm
    {
      private DataTableSetupDlg.UserChangesType UserChanges { get; set; }

      public ChangesOnForm() => this.UserChanges = DataTableSetupDlg.UserChangesType.None;

      public void ResetChange(DataTableSetupDlg.UserChangesType change)
      {
        this.UserChanges &= ~change;
      }

      public void ResetAllChanges() => this.UserChanges = DataTableSetupDlg.UserChangesType.None;

      public void SetChange(DataTableSetupDlg.UserChangesType change) => this.UserChanges |= change;

      public void SetAllChanges()
      {
        this.UserChanges = DataTableSetupDlg.UserChangesType.Header | DataTableSetupDlg.UserChangesType.Fields;
      }

      public bool CheckChange(DataTableSetupDlg.UserChangesType change)
      {
        return (this.UserChanges & change) > DataTableSetupDlg.UserChangesType.None;
      }

      public bool IsAnyChange() => this.UserChanges > DataTableSetupDlg.UserChangesType.None;
    }

    private class FormDataSource
    {
      private DataTable _dtDdmDataTable;
      private DataView _dvDdmDataTable;
      private Sessions.Session _session;
      public const string FLD_ROW_IDX = "RowIdx";
      public const string FLD_FIELD_IDX = "FieldIdx";
      public const string FLD_CRUD = "CRUD";
      public const string FLD_FIELDID = "FieldId";
      public const string FLD_IS_OUTPUT = "IsOutput";
      public const string FLD_DT_OBJ = "DDMDataTableFieldValue";

      public DDMDataTable DDMDataTable { get; set; }

      public DDMDataTableInfo DDMDataTableInfo { get; set; }

      public DataTableSetupDlg.ChangesOnForm ChangesOnForm { get; set; }

      public FormDataSource()
      {
        this.ChangesOnForm = new DataTableSetupDlg.ChangesOnForm();
        this._dtDdmDataTable = this.createInMemTable();
      }

      public FormDataSource(DDMDataTable dataTable, Sessions.Session session)
        : this()
      {
        this.DDMDataTable = dataTable;
        this._session = session;
        for (int index1 = 0; index1 < this.DDMDataTable.FieldValues.Keys.Count; ++index1)
        {
          List<DDMDataTableFieldValue> fieldValue = this.DDMDataTable.FieldValues[index1];
          for (int index2 = 0; index2 < fieldValue.Count; ++index2)
          {
            DDMDataTableFieldValue dtFieldValue = fieldValue[index2];
            this._dtDdmDataTable.Rows.Add(this.getDefaultNewRow(index1, index2, dtFieldValue));
          }
        }
        this._dvDdmDataTable = new DataView(this._dtDdmDataTable);
        this._dvDdmDataTable.Sort = "RowIdx";
        this.DDMDataTableInfo = this.ConvertToDataTableInfo();
      }

      public FormDataSource(DDMDataTableInfo dataTableInfo, Sessions.Session session)
        : this()
      {
        this.DDMDataTable = new DDMDataTable(-1, dataTableInfo.Name, dataTableInfo.Description, DateTime.Now.ToString(), (string) null, (string) null, "", dataTableInfo.GetFieldIdListString(), dataTableInfo.GetOutputIdListString());
        this.DDMDataTableInfo = dataTableInfo;
        this._session = session;
        this._dvDdmDataTable = new DataView(this._dtDdmDataTable);
        this._dvDdmDataTable.Sort = "RowIdx";
        this.ChangesOnForm.SetChange(DataTableSetupDlg.UserChangesType.Header | DataTableSetupDlg.UserChangesType.Fields);
      }

      public DataTableSetupDlg.RowItemStatus[] GetDDMDataTableFieldValues()
      {
        int rowNum = this.getRowNum();
        int columnNum = this.getColumnNum();
        DataTableSetupDlg.RowItemStatus[] tableFieldValues = new DataTableSetupDlg.RowItemStatus[rowNum];
        for (int index1 = rowNum - 1; index1 >= 0; --index1)
        {
          DataRow[] dataRowArray = this._dtDdmDataTable.Select("RowIdx = " + (object) index1 + " AND CRUD <> 'D'", "FieldIdx ASC");
          DataTableSetupDlg.RowItemStatus rowItemStatus = new DataTableSetupDlg.RowItemStatus();
          rowItemStatus.RowIndex = index1;
          rowItemStatus.DataTableFieldValues = new DDMDataTableFieldValue[columnNum];
          tableFieldValues[index1] = rowItemStatus;
          for (int index2 = 0; index2 < columnNum; ++index2)
          {
            DataRow dataRow = dataRowArray[index2];
            rowItemStatus.DataTableFieldValues[index2] = (DDMDataTableFieldValue) dataRow["DDMDataTableFieldValue"];
          }
        }
        return tableFieldValues;
      }

      public void SetUserInfo(Sessions.Session session)
      {
        this.DDMDataTable.LastModByUserID = session.UserID;
        this.DDMDataTable.LastModByFullName = session.UserInfo.FullName;
      }

      public DDMDataTableFieldValue[] GetDDMDataTableFieldValues(int rowIdx)
      {
        int columnNum = this.getColumnNum();
        DDMDataTableFieldValue[] tableFieldValues = new DDMDataTableFieldValue[columnNum];
        DataRow[] dataRowArray = this._dtDdmDataTable.Select("RowIdx = " + (object) rowIdx + " AND CRUD <> 'D'", "FieldIdx ASC");
        for (int index = 0; dataRowArray.Length != 0 && index < columnNum; ++index)
        {
          DataRow dataRow = dataRowArray[index];
          tableFieldValues[index] = (DDMDataTableFieldValue) dataRow["DDMDataTableFieldValue"];
        }
        return tableFieldValues;
      }

      public DataRow[] GetDDMDataTableFieldValuesAllStatus(int rowIdx)
      {
        return this._dtDdmDataTable.Select("RowIdx = " + (object) rowIdx);
      }

      public bool IsAnyFieldsAssigned(int rowIdx)
      {
        return this._dtDdmDataTable.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (dr =>
        {
          DDMDataTableFieldValue dataTableFieldValue = dr.Field<DDMDataTableFieldValue>("DDMDataTableFieldValue");
          return dr.Field<int>("RowIdx") == rowIdx && !dr.Field<string>("CRUD").Equals("D") && !dr.Field<bool>("IsOutput") && dataTableFieldValue.Criteria != -1 && dataTableFieldValue.Criteria != 24;
        })).Select<DataRow, DataRow>((System.Func<DataRow, DataRow>) (dr => dr)).Count<DataRow>() > 0;
      }

      public List<int> IsEmptyOutputColumns()
      {
        IEnumerable<int> second = this._dtDdmDataTable.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (dr =>
        {
          DDMDataTableFieldValue dataTableFieldValue = dr.Field<DDMDataTableFieldValue>("DDMDataTableFieldValue");
          return !dr.Field<string>("CRUD").Equals("D") && dr.Field<bool>("IsOutput") && dataTableFieldValue.Criteria != -1 && dataTableFieldValue.Criteria != 24;
        })).GroupBy<DataRow, int>((System.Func<DataRow, int>) (dr => dr.Field<int>("RowIdx"))).Select<IGrouping<int, DataRow>, int>((System.Func<IGrouping<int, DataRow>, int>) (grp => grp.First<DataRow>().Field<int>("RowIdx")));
        return this._dtDdmDataTable.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (dr => !dr.Field<string>("CRUD").Equals("D"))).GroupBy<DataRow, int>((System.Func<DataRow, int>) (dr => dr.Field<int>("RowIdx"))).Select<IGrouping<int, DataRow>, int>((System.Func<IGrouping<int, DataRow>, int>) (grp => grp.First<DataRow>().Field<int>("RowIdx"))).Except<int>(second).ToList<int>();
      }

      public void ChangeHeader(DDMDataTableInfo dataTableInfo)
      {
        string empty = string.Empty;
        if (!(!string.IsNullOrEmpty(this.DDMDataTable.OriginalName) ? this.DDMDataTable.OriginalName : this.DDMDataTable.Name).Equals(dataTableInfo.Name))
          this.DDMDataTable.OriginalName = this.DDMDataTable.Name;
        this.DDMDataTable.Name = dataTableInfo.Name;
        this.DDMDataTable.Description = dataTableInfo.Description;
        this.ChangesOnForm.SetChange(DataTableSetupDlg.UserChangesType.Header);
      }

      public void SetDataTableId(int dataTableId) => this.DDMDataTable.Id = dataTableId;

      public void ChangeFields(DDMDataTableInfo updDataTableInfo, DDMDataTableInfo orgDataTableInfo)
      {
        this.DDMDataTableInfo = updDataTableInfo;
        this.DDMDataTable.FieldIdList = updDataTableInfo.GetFieldIdListString();
        this.DDMDataTable.OutputIdList = updDataTableInfo.GetOutputIdListString();
        Dictionary<string, DataTableSetupDlg.FormDataSource.FieldCheckStatus> dictionary1 = new Dictionary<string, DataTableSetupDlg.FormDataSource.FieldCheckStatus>();
        Dictionary<int, DataTableSetupDlg.FormDataSource.FieldCheckStatus> dictionary2 = new Dictionary<int, DataTableSetupDlg.FormDataSource.FieldCheckStatus>();
        for (int index = 0; index < orgDataTableInfo.Fields.Length; ++index)
        {
          if (!orgDataTableInfo.Fields[index].IsOutput)
            dictionary1.Add(orgDataTableInfo.Fields[index].FieldId, new DataTableSetupDlg.FormDataSource.FieldCheckStatus()
            {
              OriginalIdx = index,
              NewIdx = -1,
              Status = ""
            });
        }
        int num1 = 0;
        for (int index = 0; index < updDataTableInfo.Fields.Length; ++index)
        {
          DDMDataTableFieldInfo field = updDataTableInfo.Fields[index];
          if (field.IsOutput)
            dictionary2.Add(num1++, new DataTableSetupDlg.FormDataSource.FieldCheckStatus()
            {
              OriginalIdx = -1,
              NewIdx = index,
              Status = "",
              FieldInfo = field
            });
          else if (dictionary1.ContainsKey(field.FieldId))
          {
            DataTableSetupDlg.FormDataSource.FieldCheckStatus fieldCheckStatus = dictionary1[field.FieldId];
            if (fieldCheckStatus.OriginalIdx != index)
            {
              fieldCheckStatus.NewIdx = index;
              fieldCheckStatus.Status = "OC";
            }
            else
              fieldCheckStatus.Status = "NA";
          }
          else
            dictionary1.Add(field.FieldId, new DataTableSetupDlg.FormDataSource.FieldCheckStatus()
            {
              OriginalIdx = -1,
              NewIdx = index,
              Status = "NF",
              FieldInfo = field
            });
        }
        foreach (string key in dictionary1.Keys)
        {
          DataTableSetupDlg.FormDataSource.FieldCheckStatus fieldCheckStatus = dictionary1[key];
          if (fieldCheckStatus.Status == "")
          {
            foreach (DataRow dataRow in this._dtDdmDataTable.Select("FieldId = '" + key + "'"))
            {
              if (dataRow["CRUD"].ToString().Equals("C"))
                dataRow.Delete();
              else
                dataRow["CRUD"] = (object) "D";
            }
            this.ChangesOnForm.SetChange(DataTableSetupDlg.UserChangesType.Deleted);
          }
          else if (fieldCheckStatus.Status == "OC")
          {
            foreach (DataRow dataRow in this._dtDdmDataTable.Select("FieldId = '" + key + "'"))
            {
              dataRow["FieldIdx"] = (object) fieldCheckStatus.NewIdx;
              (dataRow["DDMDataTableFieldValue"] as DDMDataTableFieldValue).ColumnId = fieldCheckStatus.NewIdx;
              string str = (string) dataRow["CRUD"];
              if (str != "C" && str != "D")
                dataRow["CRUD"] = (object) "U";
            }
            this.ChangesOnForm.SetChange(DataTableSetupDlg.UserChangesType.OrderChange);
          }
          else if (fieldCheckStatus.Status == "NF")
          {
            int rowNum = this.getRowNum();
            for (int rowIdx = 0; rowIdx < rowNum; ++rowIdx)
              this._dtDdmDataTable.Rows.Add(this.getDefaultNewRow(rowIdx, fieldCheckStatus.NewIdx, fieldCheckStatus.FieldInfo, "C"));
            this.ChangesOnForm.SetChange(DataTableSetupDlg.UserChangesType.NewField);
          }
        }
        DataRow[] dataRowArray = this._dtDdmDataTable.Select("IsOutput", "RowIdx ASC, FieldIdx ASC");
        int key1 = 0;
        int num2 = -1;
        foreach (DataRow dataRow in dataRowArray)
        {
          int int32 = Convert.ToInt32(dataRow["RowIdx"]);
          if (num2 != int32)
          {
            num2 = int32;
            key1 = 0;
          }
          if (!dictionary2.ContainsKey(key1))
          {
            dataRow["CRUD"] = (object) "D";
          }
          else
          {
            int newIdx = dictionary2[key1].NewIdx;
            if (Convert.ToInt32(dataRow["FieldIdx"]) != newIdx)
            {
              dataRow["FieldIdx"] = (object) newIdx;
              (dataRow["DDMDataTableFieldValue"] as DDMDataTableFieldValue).ColumnId = newIdx;
              string str = (string) dataRow["CRUD"];
              if (str != "C" && str != "D")
                dataRow["CRUD"] = (object) "U";
            }
            if (!object.Equals(dataRow["FieldId"], (object) dictionary2[key1].FieldInfo.FieldId))
            {
              dataRow["FieldId"] = (object) dictionary2[key1].FieldInfo.FieldId;
              (dataRow["DDMDataTableFieldValue"] as DDMDataTableFieldValue).FieldId = dictionary2[key1].FieldInfo.FieldId;
              string str = (string) dataRow["CRUD"];
              if (str != "C" && str != "D")
                dataRow["CRUD"] = (object) "U";
            }
          }
          ++key1;
        }
        int num3 = ((IEnumerable<DDMDataTableFieldInfo>) orgDataTableInfo.Fields).Count<DDMDataTableFieldInfo>((System.Func<DDMDataTableFieldInfo, bool>) (x => x.IsOutput));
        int num4 = ((IEnumerable<DDMDataTableFieldInfo>) updDataTableInfo.Fields).Count<DDMDataTableFieldInfo>((System.Func<DDMDataTableFieldInfo, bool>) (x => x.IsOutput));
        if (num4 > num3)
        {
          List<string> stringList = new List<string>((IEnumerable<string>) updDataTableInfo.GetOutputIdListString().Split('|'));
          int rowNum = this.getRowNum();
          for (int rowIdx = 0; rowIdx < rowNum; ++rowIdx)
          {
            for (int index = num3; index < num4; ++index)
            {
              int fieldIdx = ((IEnumerable<DDMDataTableFieldInfo>) updDataTableInfo.Fields).Count<DDMDataTableFieldInfo>() - (num4 - index);
              string fieldId = stringList[index];
              this._dtDdmDataTable.Rows.Add(this.getDefaultNewRow(rowIdx, fieldIdx, new DDMDataTableFieldInfo(fieldId, outputIdx: index), "C", true));
            }
          }
        }
        this.ChangesOnForm.SetChange(DataTableSetupDlg.UserChangesType.Fields);
      }

      public int CleanUpEmptyRows()
      {
        List<\u003C\u003Ef__AnonymousType8<int, int>> list = this._dtDdmDataTable.AsEnumerable().Where<DataRow>((System.Func<DataRow, bool>) (dr => !dr.Field<DDMDataTableFieldValue>("DDMDataTableFieldValue").IsOutput && dr.Field<string>("CRUD") != "D")).Select(dr =>
        {
          DDMDataTableFieldValue dataTableFieldValue = dr.Field<DDMDataTableFieldValue>("DDMDataTableFieldValue");
          return new
          {
            RowId = dr.Field<int>("RowIdx"),
            Count = dataTableFieldValue.Criteria != -1 ? 1 : 0
          };
        }).GroupBy(f => f.RowId).Select(g => new
        {
          RowId = g.Key,
          Count = g.Sum(x => x.Count)
        }).OrderBy(r => r.RowId).ToList();
        int num1 = 0;
        int num2 = 0;
        foreach (var data in list)
        {
          if (data.Count == 0)
          {
            this.DeleteRow(data.RowId - num2);
            ++num2;
            ++num1;
          }
        }
        return num1;
      }

      public void UpdateFieldValue(int rowIdx, DataTableFieldValues dtFieldValues)
      {
        DataRow[] dataRowArray = this._dtDdmDataTable.Select("RowIdx = " + (object) rowIdx + " AND CRUD <> 'D'", "FieldIdx ASC");
        for (int index = 0; index < dataRowArray.Length; ++index)
        {
          DataRow row = dataRowArray[index];
          DDMDataTableFieldValue dataTableFieldValue = (DDMDataTableFieldValue) row["DDMDataTableFieldValue"];
          FieldValueBase fieldValueBase = dtFieldValues.DataTableFields[index].Value;
          if (dataTableFieldValue.Values != (fieldValueBase != null ? fieldValueBase.ToString() : "") || fieldValueBase == null || (DDMCriteria) dataTableFieldValue.Criteria != fieldValueBase.Criteria)
          {
            if (row.Field<string>("CRUD") != "C")
              row["CRUD"] = (object) "U";
            if (fieldValueBase != null)
            {
              dataTableFieldValue.Values = dtFieldValues.DataTableFields[index].Value.ToString();
              dataTableFieldValue.Criteria = (int) dtFieldValues.DataTableFields[index].Value.Criteria;
            }
            else
            {
              dataTableFieldValue.Values = "";
              dataTableFieldValue.Criteria = -1;
            }
            this.ChangesOnForm.SetChange(DataTableSetupDlg.UserChangesType.Value);
          }
        }
      }

      public void AddNewRow(int requiredRowPos, GVItem original)
      {
        if (this.DDMDataTableInfo.Fields == null)
        {
          int num1 = (int) MessageBox.Show("You must modify the field because the field definition is empty.");
        }
        else
        {
          if (this.getRowNum() > requiredRowPos)
          {
            foreach (DataRow dataRow in this._dtDdmDataTable.Select("RowIdx >= " + (object) requiredRowPos))
            {
              int num2 = (int) dataRow["RowIdx"];
              dataRow["RowIdx"] = (object) (num2 + 1);
              (dataRow["DDMDataTableFieldValue"] as DDMDataTableFieldValue).RowId = num2 + 1;
              string str = (string) dataRow["CRUD"];
              if (str != "C" && str != "D")
                dataRow["CRUD"] = (object) "U";
            }
          }
          DDMDataTableInfo ddmDataTableInfo = this.DDMDataTableInfo;
          for (int fieldIdx = 0; fieldIdx < ddmDataTableInfo.Fields.Length; ++fieldIdx)
          {
            DDMDataTableFieldInfo field = ddmDataTableInfo.Fields[fieldIdx];
            this._dtDdmDataTable.Rows.Add(original == null ? this.getDefaultNewRow(requiredRowPos, fieldIdx, field, "C", field.IsOutput) : this.getCopiedNewRow(requiredRowPos, fieldIdx, original.Index, field, isOutput: field.IsOutput));
          }
          this.ChangesOnForm.SetChange(DataTableSetupDlg.UserChangesType.NewRow);
        }
      }

      public DDMDataTableInfo ConvertToDataTableInfo()
      {
        DDMDataTable ddmDataTable = this.DDMDataTable;
        string fieldIdList = ddmDataTable.FieldIdList;
        string outputIdList = ddmDataTable.OutputIdList;
        DDMDataTableInfo dataTableInfo = new DDMDataTableInfo();
        dataTableInfo.Name = ddmDataTable.Name;
        dataTableInfo.Description = ddmDataTable.Description;
        string[] strArray = fieldIdList.Split('|');
        List<string> stringList = new List<string>((IEnumerable<string>) outputIdList.Split('|'));
        int count = stringList.Count;
        if (strArray != null && strArray.Length != 0)
        {
          dataTableInfo.Fields = new DDMDataTableFieldInfo[strArray.Length + count];
          for (int index = 0; index < strArray.Length; ++index)
          {
            string str = strArray[index];
            FieldDefinition field1 = EncompassFields.GetField(str);
            if (str.ToLower().StartsWith("cx."))
            {
              FieldDefinition field2 = EncompassFields.GetField(str, this._session.LoanManager.GetFieldSettings());
              dataTableInfo.Fields[index] = new DDMDataTableFieldInfo(field2);
            }
            else if (field1.Category != FieldCategory.Common)
            {
              FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(str);
              LoanXDBField xdbField = new LoanXDBField(fieldPairInfo.FieldID, field1.Description, LoanXDBTableList.TableTypes.IsString, "", true, false, fieldPairInfo.PairIndex, fieldPairInfo.PairIndex, LoanXDBField.FieldStatus.None);
              dataTableInfo.Fields[index] = new DDMDataTableFieldInfo(xdbField, field1);
            }
            else
              dataTableInfo.Fields[index] = new DDMDataTableFieldInfo(field1);
          }
          for (int index = 0; index < count; ++index)
          {
            DDMDataTableFieldInfo dataTableFieldInfo = new DDMDataTableFieldInfo(stringList[index], outputIdx: index);
            dataTableInfo.Fields[strArray.Length + index] = dataTableFieldInfo;
          }
        }
        else
          dataTableInfo.Fields = new DDMDataTableFieldInfo[0];
        return dataTableInfo;
      }

      public DDMDataTableFieldValue GetNewDataTableFieldValue(
        DDMDataTableFieldInfo dataTableFieldInfo,
        int rowIdx,
        int fieldIdx,
        bool isOutput = false)
      {
        return new DDMDataTableFieldValue()
        {
          Id = -1,
          FieldId = dataTableFieldInfo == null ? string.Empty : dataTableFieldInfo.FieldId,
          DataTableId = this.DDMDataTable.Id,
          RowId = rowIdx,
          ColumnId = fieldIdx,
          Values = "",
          IsOutput = isOutput,
          Criteria = -1
        };
      }

      public DDMDataTableFieldValue GetNewDataTableFieldValue(
        DDMDataTableFieldInfo dataTableFieldInfo,
        int rowIdx,
        int fieldIdx,
        string value,
        int criteria,
        bool isOutput = false)
      {
        DDMDataTableFieldValue dataTableFieldValue = this.GetNewDataTableFieldValue(dataTableFieldInfo, rowIdx, fieldIdx, isOutput);
        dataTableFieldValue.Values = value;
        dataTableFieldValue.Criteria = criteria;
        return dataTableFieldValue;
      }

      public DDMDataTableFieldValue[] GetNewDataTableFieldValues(int rowIdx)
      {
        if (this.DDMDataTableInfo.Fields == null)
          return (DDMDataTableFieldValue[]) null;
        int length = this.DDMDataTableInfo.Fields.Length;
        DDMDataTableFieldValue[] tableFieldValues = new DDMDataTableFieldValue[length];
        for (int fieldIdx = 0; fieldIdx < length; ++fieldIdx)
        {
          DDMDataTableFieldInfo field = this.DDMDataTableInfo.Fields[fieldIdx];
          tableFieldValues[fieldIdx] = this.GetNewDataTableFieldValue(field, rowIdx, fieldIdx);
        }
        return tableFieldValues;
      }

      public void DeleteRow(int rowIdx)
      {
        foreach (DataRow dataRow in this._dtDdmDataTable.Select("RowIdx = " + (object) rowIdx))
        {
          DDMDataTableFieldValue dataTableFieldValue = (DDMDataTableFieldValue) dataRow["DDMDataTableFieldValue"];
          if ((string) dataRow["CRUD"] != "C")
            dataRow["CRUD"] = (object) "D";
          else
            dataRow.Delete();
        }
        DataRow[] dataRowArray = this._dtDdmDataTable.Select("RowIdx > " + (object) rowIdx);
        foreach (DataRow dataRow in dataRowArray)
        {
          int num = (int) dataRow["RowIdx"];
          dataRow["RowIdx"] = (object) (num - 1);
          (dataRow["DDMDataTableFieldValue"] as DDMDataTableFieldValue).RowId = num - 1;
          string str = (string) dataRow["CRUD"];
          if (str != "C" && str != "D")
            dataRow["CRUD"] = (object) "U";
        }
        if (dataRowArray.Length != 0)
          this.ChangesOnForm.SetChange(DataTableSetupDlg.UserChangesType.OrderChange);
        this.ChangesOnForm.SetChange(DataTableSetupDlg.UserChangesType.Deleted);
      }

      public void SwapRow(int upper, int lower)
      {
        DataRow[] dataRowArray1 = this._dtDdmDataTable.Select("RowIdx = " + (object) upper);
        DataRow[] dataRowArray2 = this._dtDdmDataTable.Select("RowIdx = " + (object) lower);
        for (int index = 0; index < dataRowArray1.Length; ++index)
        {
          DataRow dataRow = dataRowArray1[index];
          ((DDMDataTableFieldValue) dataRow["DDMDataTableFieldValue"]).RowId = lower;
          dataRow["RowIdx"] = (object) lower;
          string str = (string) dataRow["CRUD"];
          if (str != "C" && str != "D")
            dataRow["CRUD"] = (object) "U";
        }
        for (int index = 0; index < dataRowArray2.Length; ++index)
        {
          DataRow dataRow = dataRowArray2[index];
          ((DDMDataTableFieldValue) dataRow["DDMDataTableFieldValue"]).RowId = upper;
          dataRow["RowIdx"] = (object) upper;
          string str = (string) dataRow["CRUD"];
          if (str != "C" && str != "D")
            dataRow["CRUD"] = (object) "U";
        }
        this.ChangesOnForm.SetChange(DataTableSetupDlg.UserChangesType.OrderChange);
      }

      public void CommitChanges(Sessions.Session session)
      {
        DDMDataTableBpmManager bpmManager1 = (DDMDataTableBpmManager) session.BPM.GetBpmManager(BpmCategory.DDMDataTables);
        DDMDataTableFieldBpmManager bpmManager2 = (DDMDataTableFieldBpmManager) session.BPM.GetBpmManager(BpmCategory.DDMDataTableFields);
        bool flag = false;
        int dataTableId;
        if (this.ChangesOnForm.CheckChange(DataTableSetupDlg.UserChangesType.Header | DataTableSetupDlg.UserChangesType.Fields))
        {
          this.DDMDataTable.LastModDt = session.ServerTime.ToString();
          this.DDMDataTable.LastModByUserID = session.UserID;
          this.DDMDataTable.LastModByFullName = session.UserInfo.FullName;
          dataTableId = bpmManager1.UpsertDDMDataTable(new DDMDataTable(this.DDMDataTable.Id, this.DDMDataTable.Name, this.DDMDataTable.Description, this.DDMDataTable.LastModDt, this.DDMDataTable.LastModByUserID, this.DDMDataTable.LastModByFullName, this.DDMDataTable.DataTableType, this.DDMDataTable.FieldIdList, this.DDMDataTable.OutputIdList)
          {
            OriginalName = this.DDMDataTable.OriginalName
          }, forceToPrimaryDb: true);
          BpmManager bpmManager3 = session.BPM.GetBpmManager(BizRuleType.DDMFeeScenarios);
          bpmManager3.BPMManager.InvalidateCache(BizRuleType.DDMFeeScenarios);
          bpmManager3.BPMManager.InvalidateCache(BizRuleType.DDMFieldScenarios);
          this.SetDataTableId(dataTableId);
          flag = true;
        }
        else
          dataTableId = this.DDMDataTable.Id;
        if (this.ChangesOnForm.CheckChange(DataTableSetupDlg.UserChangesType.Fields | DataTableSetupDlg.UserChangesType.NewRow | DataTableSetupDlg.UserChangesType.NewField | DataTableSetupDlg.UserChangesType.Deleted | DataTableSetupDlg.UserChangesType.OrderChange | DataTableSetupDlg.UserChangesType.Value))
        {
          List<DataRow> dataRowList = new List<DataRow>();
          List<DDMDataTableFieldValue> batchDeletionList = new List<DDMDataTableFieldValue>();
          List<DDMDataTableFieldValue> batchCreationList = new List<DDMDataTableFieldValue>();
          List<DDMDataTableFieldValue> batchUpdateList = new List<DDMDataTableFieldValue>();
          for (int index = 0; index < this._dtDdmDataTable.Rows.Count; ++index)
          {
            DataRow row = this._dtDdmDataTable.Rows[index];
            switch ((string) row["CRUD"])
            {
              case "U":
                DDMDataTableFieldValue dataTableFieldValue1 = (DDMDataTableFieldValue) row["DDMDataTableFieldValue"];
                dataTableFieldValue1.DataTableId = dataTableId;
                batchUpdateList.Add(dataTableFieldValue1);
                row["CRUD"] = (object) "R";
                break;
              case "C":
                DDMDataTableFieldValue dataTableFieldValue2 = (DDMDataTableFieldValue) row["DDMDataTableFieldValue"];
                dataTableFieldValue2.DataTableId = dataTableId;
                batchCreationList.Add(dataTableFieldValue2);
                row["CRUD"] = (object) "R";
                break;
              case "D":
                DDMDataTableFieldValue dataTableFieldValue3 = (DDMDataTableFieldValue) row["DDMDataTableFieldValue"];
                batchDeletionList.Add(dataTableFieldValue3);
                dataRowList.Add(row);
                break;
            }
          }
          int[] numArray = bpmManager2.AtomicDataTableChange(batchCreationList, batchUpdateList, batchDeletionList);
          for (int index = 0; index < batchCreationList.Count; ++index)
            batchCreationList[index].Id = numArray[index];
          foreach (DataRow dataRow in dataRowList)
            dataRow.Delete();
          if (!flag)
          {
            this.DDMDataTable.LastModByUserID = session.UserID;
            this.DDMDataTable.LastModByFullName = session.UserInfo.FullName;
            this.DDMDataTable.LastModDt = DateTime.Now.ToString();
            DDMDataTable ddmDataTable = new DDMDataTable(this.DDMDataTable.Id, this.DDMDataTable.Name, this.DDMDataTable.Description, this.DDMDataTable.LastModDt, this.DDMDataTable.LastModByUserID, this.DDMDataTable.LastModByFullName, this.DDMDataTable.DataTableType, this.DDMDataTable.FieldIdList, this.DDMDataTable.OutputIdList);
            bpmManager1.UpsertDDMDataTable(ddmDataTable, forceToPrimaryDb: true);
            this.DDMDataTable.OriginalName = (string) null;
          }
        }
        this.ChangesOnForm.ResetAllChanges();
      }

      private DataTable createInMemTable()
      {
        return new DataTable()
        {
          Columns = {
            {
              "RowIdx",
              typeof (int)
            },
            {
              "FieldIdx",
              typeof (int)
            },
            {
              "CRUD",
              typeof (string)
            },
            {
              "FieldId",
              typeof (string)
            },
            {
              "IsOutput",
              typeof (bool)
            },
            {
              "DDMDataTableFieldValue",
              typeof (DDMDataTableFieldValue)
            }
          }
        };
      }

      private DataRow getDefaultNewRow(
        int rowIdx,
        int fieldIdx,
        DDMDataTableFieldValue dtFieldValue,
        string crud = "R")
      {
        DataRow defaultNewRow = this._dtDdmDataTable.NewRow();
        defaultNewRow["RowIdx"] = (object) rowIdx;
        defaultNewRow["FieldIdx"] = (object) fieldIdx;
        defaultNewRow["CRUD"] = (object) crud;
        defaultNewRow["FieldId"] = (object) dtFieldValue.FieldId;
        defaultNewRow["IsOutput"] = (object) dtFieldValue.IsOutput;
        defaultNewRow["DDMDataTableFieldValue"] = (object) dtFieldValue;
        return defaultNewRow;
      }

      private DataRow getDefaultNewRow(
        int rowIdx,
        int fieldIdx,
        DDMDataTableFieldInfo dtFieldInfo,
        string crud = "R",
        bool isOutput = false)
      {
        DataRow defaultNewRow = this._dtDdmDataTable.NewRow();
        defaultNewRow["RowIdx"] = (object) rowIdx;
        defaultNewRow["FieldIdx"] = (object) fieldIdx;
        defaultNewRow["CRUD"] = (object) crud;
        defaultNewRow["FieldId"] = (object) dtFieldInfo.FieldId;
        defaultNewRow["IsOutput"] = (object) dtFieldInfo.IsOutput;
        defaultNewRow["DDMDataTableFieldValue"] = (object) this.GetNewDataTableFieldValue(dtFieldInfo, rowIdx, fieldIdx, isOutput);
        return defaultNewRow;
      }

      private DataRow getCopiedNewRow(
        int rowIdx,
        int fieldIdx,
        int sourceRowId,
        DDMDataTableFieldInfo dtFieldInfo,
        string crud = "C",
        bool isOutput = false)
      {
        DataRow copiedNewRow = this._dtDdmDataTable.NewRow();
        copiedNewRow["RowIdx"] = (object) rowIdx;
        copiedNewRow["FieldIdx"] = (object) fieldIdx;
        copiedNewRow["CRUD"] = (object) crud;
        copiedNewRow["FieldId"] = (object) dtFieldInfo.FieldId;
        copiedNewRow["IsOutput"] = (object) dtFieldInfo.IsOutput;
        DataRow[] dataRowArray = this._dtDdmDataTable.Select("RowIdx = " + (object) sourceRowId + " AND FieldIdx = " + (object) fieldIdx + " AND CRUD <> 'D'", "FieldIdx ASC");
        DDMDataTableFieldValue dataTableFieldValue = dataRowArray != null && dataRowArray.Length == 1 ? (DDMDataTableFieldValue) dataRowArray[0]["DDMDataTableFieldValue"] : throw new Exception("Expected single row from in-mem DataTable data source, but received multiple rows.");
        copiedNewRow["DDMDataTableFieldValue"] = (object) this.GetNewDataTableFieldValue(dtFieldInfo, rowIdx, fieldIdx, dataTableFieldValue.Values, dataTableFieldValue.Criteria, isOutput);
        return copiedNewRow;
      }

      private int getRowNum()
      {
        return this._dtDdmDataTable.Rows.Cast<DataRow>().Where<DataRow>((System.Func<DataRow, bool>) (r => (string) r["CRUD"] != "D")).Select(r => new
        {
          RowId = r["RowIdx"]
        }).Distinct().Count();
      }

      private int getColumnNum()
      {
        return this.DDMDataTableInfo.Fields != null ? this.DDMDataTableInfo.Fields.Length : 0;
      }

      private class FieldCheckStatus
      {
        public int OriginalIdx;
        public int NewIdx;
        public string Status;
        public DDMDataTableFieldInfo FieldInfo;
      }
    }

    [Flags]
    private enum UserChangesType
    {
      None = 0,
      Header = 1,
      Fields = 2,
      NewRow = 4,
      NewField = 8,
      Deleted = 16, // 0x00000010
      OrderChange = 32, // 0x00000020
      Value = 64, // 0x00000040
    }

    private class BuddyButton : Button
    {
      protected override void OnPaint(PaintEventArgs pevent)
      {
        base.OnPaint(pevent);
        pevent.Graphics.DrawString("...", new Font(this.Font.Name, 10f, FontStyle.Regular), (Brush) new SolidBrush(this.ForeColor), (PointF) new Point(1, -2));
      }
    }
  }
}
