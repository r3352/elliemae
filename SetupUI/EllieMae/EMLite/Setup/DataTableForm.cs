// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DataTableForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.DynamicDataManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DataTableForm : UserControl, IOnlineHelpTarget
  {
    private StandardFields standardFields;
    private FieldSettings fieldSettings;
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer gcBaseRate;
    private GridView listViewOptions;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnDelete;
    private StandardIconButton stdIconBtnCopy;
    private ToolTip toolTip;
    private StandardIconButton stdIconBtnImport;
    private StandardIconButton stdIconButtonExport;

    public DataTableForm()
    {
      this.InitializeComponent();
      this.updateTableCount();
      this.listViewOptions.DoubleClick += new EventHandler(this.listViewOptions_DoubleClick);
      this.HandleStandardButtons();
    }

    public DataTableForm(SetUpContainer setupContainer, Sessions.Session session)
      : this()
    {
      this.session = session;
      this.refreshGridWithData(true);
      this.HandleStandardButtons();
    }

    private void updateTableCount()
    {
      this.gcBaseRate.Text = "Data Tables List (" + (object) this.listViewOptions.Items.Count + ")";
    }

    private void loadStandardFieldsandFieldSettings()
    {
      if (this.standardFields == null)
        this.standardFields = Session.LoanManager.GetStandardFields();
      if (this.fieldSettings != null)
        return;
      this.fieldSettings = Session.LoanManager.GetFieldSettings();
    }

    private void stdIconBtnNew_Click(object sender, EventArgs e)
    {
      this.loadStandardFieldsandFieldSettings();
      using (DataTableDlg dataTableDlg = new DataTableDlg(this.standardFields, this.fieldSettings))
      {
        int num = (int) dataTableDlg.ShowDialog();
      }
      this.refreshGridWithData();
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e)
    {
      GVItem selectedItem = this.getSelectedItem();
      if ((string) selectedItem.SubItems[2].Value == "System Table")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please use the Settings under ‘Tables and Fees’ to manage this System table.");
      }
      else
        this.openDataTableSetupDlg(selectedItem);
    }

    private void listViewOptions_DoubleClick(object sender, EventArgs e)
    {
      GVItem selectedItem = this.getSelectedItem();
      if (selectedItem == null)
        return;
      if ((string) selectedItem.SubItems[2].Value == "System Table")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please use the Settings under ‘Tables and Fees’ to manage this System table.");
      }
      else
      {
        this.openDataTableSetupDlg(selectedItem);
        this.HandleStandardButtons();
      }
    }

    private void openDataTableSetupDlg(GVItem selItem)
    {
      if (selItem == null)
        return;
      int id = (selItem.Tag as DDMDataTable).Id;
      using (DataTableSetupDlg dataTableSetupDlg = new DataTableSetupDlg(id, this.session))
      {
        int num = (int) dataTableSetupDlg.ShowDialog((IWin32Window) this);
        this.refreshGridWithData();
      }
      this.selectDataTable(id);
    }

    private void selectDataTable(int selectedDataTableId)
    {
      GVItem gvItem = this.listViewOptions.Items.Where<GVItem>((Func<GVItem, bool>) (item => ((DDMDataTable) item.Tag).Id == selectedDataTableId)).FirstOrDefault<GVItem>();
      if (gvItem == null)
        return;
      gvItem.Selected = true;
    }

    private void refreshGridWithData(bool defaultSort = false)
    {
      this.listViewOptions.Items.Clear();
      foreach (DDMDataTable ddmDataTable in ((DDMDataTableBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMDataTables)).GetAllDDMDataTable(true))
        this.BindDataTableToGrid(ddmDataTable);
      using (IEnumerator<GVColumn> enumerator = this.listViewOptions.Columns.Where<GVColumn>((Func<GVColumn, bool>) (col => col.SortPriority == 0)).GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          GVColumn current = enumerator.Current;
          this.listViewOptions.Sort(current.Index, current.SortOrder);
        }
      }
      if (defaultSort)
        this.listViewOptions.Sort(4, SortOrder.Descending);
      this.BindSystemTablesToGrid();
      this.updateTableCount();
    }

    private void BindSystemTablesToGrid()
    {
      string str1 = "Escrow Fees for purchase or refinance loans";
      this.listViewOptions.Items.Add(new GVItem("Escrow")
      {
        SubItems = {
          [0] = {
            Value = (object) "Escrow"
          },
          [1] = {
            Value = (object) str1
          },
          [2] = {
            Value = (object) "System Table"
          },
          [3] = {
            Value = (object) ""
          },
          [4] = {
            Value = (object) ""
          }
        }
      });
      string str2 = "Title Fees for purchase or refinance loans";
      this.listViewOptions.Items.Add(new GVItem("Title")
      {
        SubItems = {
          [0] = {
            Value = (object) "Title"
          },
          [1] = {
            Value = (object) str2
          },
          [2] = {
            Value = (object) "System Table"
          },
          [3] = {
            Value = (object) ""
          },
          [4] = {
            Value = (object) ""
          }
        }
      });
      string str3 = "Data maintained under the HELOC Table";
      this.listViewOptions.Items.Add(new GVItem("HELOC Table")
      {
        SubItems = {
          [0] = {
            Value = (object) "HELOC Table"
          },
          [1] = {
            Value = (object) str3
          },
          [2] = {
            Value = (object) "System Table"
          },
          [3] = {
            Value = (object) ""
          },
          [4] = {
            Value = (object) ""
          }
        }
      });
      this.listViewOptions.Items.Add(new GVItem("MI Tables")
      {
        SubItems = {
          [0] = {
            Value = (object) "MI Tables"
          },
          [1] = {
            Value = (object) "Get MI - Mortgage Insurance Tables"
          },
          [2] = {
            Value = (object) "System Table"
          },
          [3] = {
            Value = (object) ""
          },
          [4] = {
            Value = (object) ""
          }
        }
      });
      string str4 = "Calculate City tax on a loan";
      this.listViewOptions.Items.Add(new GVItem("City Tax")
      {
        SubItems = {
          [0] = {
            Value = (object) "City Tax"
          },
          [1] = {
            Value = (object) str4
          },
          [2] = {
            Value = (object) "System Table"
          },
          [3] = {
            Value = (object) ""
          },
          [4] = {
            Value = (object) ""
          }
        }
      });
      string str5 = "Calculate State tax on a loan";
      this.listViewOptions.Items.Add(new GVItem("State Tax")
      {
        SubItems = {
          [0] = {
            Value = (object) "State Tax"
          },
          [1] = {
            Value = (object) str5
          },
          [2] = {
            Value = (object) "System Table"
          },
          [3] = {
            Value = (object) ""
          },
          [4] = {
            Value = (object) ""
          }
        }
      });
      string str6 = "User defined tables to calculate fees on a loan";
      this.listViewOptions.Items.Add(new GVItem("User Defined Fee")
      {
        SubItems = {
          [0] = {
            Value = (object) "User Defined Fee"
          },
          [1] = {
            Value = (object) str6
          },
          [2] = {
            Value = (object) "System Table"
          },
          [3] = {
            Value = (object) ""
          },
          [4] = {
            Value = (object) ""
          }
        }
      });
    }

    private void BindDataTableToGrid(DDMDataTable ddmDataTable)
    {
      this.listViewOptions.Items.Add(new GVItem()
      {
        Tag = (object) ddmDataTable,
        SubItems = {
          [0] = {
            Value = (object) ddmDataTable.Name
          },
          [1] = {
            Value = (object) ddmDataTable.Description
          },
          [2] = {
            Value = (object) "User defined table"
          },
          [3] = {
            Value = (object) (ddmDataTable.LastModByUserID + " (" + ddmDataTable.LastModByFullName + ")")
          },
          [4] = {
            Value = (object) ddmDataTable.LastModDt
          }
        }
      });
    }

    private GVItem getSelectedItem()
    {
      return this.listViewOptions.SelectedItems.Count > 0 ? this.listViewOptions.SelectedItems[0] : (GVItem) null;
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      DDMDataTableBpmManager bpmManager = (DDMDataTableBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMDataTables);
      GVItem selectedItem = this.getSelectedItem();
      if ((string) selectedItem.SubItems[2].Value == "System Table")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please use the Settings under ‘Tables and Fees’ to manage this System table.");
      }
      else
      {
        DDMDataTable tag = (DDMDataTable) selectedItem.Tag;
        if (bpmManager.IsTableUsedByFeeOrFieldRules(tag, true))
        {
          if (MessageBox.Show("You have at least one active scenario using this data table. Are you sure you want to delete this data table?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
          bpmManager.DeleteDDMDataTable(tag);
          bpmManager.ResetDataTableFeeRuleFieldRuleValue(tag);
          this.listViewOptions.Items.Remove(this.listViewOptions.SelectedItems[0]);
        }
        else
        {
          if (MessageBox.Show("Are you sure you want to delete this data table?", "Encompass", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
          bpmManager.DeleteDDMDataTable(tag);
          bpmManager.ResetDataTableFeeRuleFieldRuleValue(tag);
          this.listViewOptions.Items.Remove(this.listViewOptions.SelectedItems[0]);
        }
      }
    }

    private void stdIconBtnImport_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Comma Separated Value Files (*.csv)|*.csv";
      openFileDialog.Multiselect = false;
      if (openFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      string fileName = openFileDialog.FileName;
      this.loadStandardFieldsandFieldSettings();
      DataTableImportParameters importParameters = new DataTableImportParameters(this.session, fileName, this.standardFields, this.fieldSettings);
      importParameters.ShowDialog((IWin32Window) this);
      if (importParameters.ImportedDataTable == null)
        return;
      this.refreshGridWithData(true);
      this.selectDataTable(importParameters.ImportedDataTable.Id);
    }

    private void listViewOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.HandleStandardButtons();
    }

    private void HandleStandardButtons()
    {
      this.stdIconButtonExport.Enabled = this.stdIconBtnCopy.Enabled = this.stdIconBtnDelete.Enabled = this.stdIconBtnEdit.Enabled = this.listViewOptions.SelectedItems.Count > 0;
    }

    private void stdIconButtonExport_Click(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems.Count == 0)
        return;
      if ((string) this.getSelectedItem().SubItems[2].Value == "System Table")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please use the Settings under ‘Tables and Fees’ to manage this System table.");
      }
      else
      {
        DDMDataTable tag = (DDMDataTable) this.listViewOptions.SelectedItems[0].Tag;
        DDMDataTableBpmManager bpmManager = (DDMDataTableBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMDataTables);
        DDMDataTable tableAndFieldValues = bpmManager.GetDDMDataTableAndFieldValues(tag.Id, true);
        if (tableAndFieldValues.FieldValues == null || tableAndFieldValues.FieldValues.Count == 0)
        {
          int num2 = (int) MessageBox.Show((IWin32Window) this, "Cannot export selected data table as there is no data in the table.", "Cannot export", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          List<string> rowInfo = new List<string>();
          List<DDMDataTableFieldValue> fieldValue1 = tableAndFieldValues.FieldValues[0];
          rowInfo.Add("SystemLogID");
          foreach (DDMDataTableFieldValue dataTableFieldValue in fieldValue1)
          {
            string fieldId = dataTableFieldValue.FieldId;
            rowInfo.Add(fieldId);
          }
          List<List<string>> tableInfo = new List<List<string>>();
          string dataTableExportLogID = Guid.NewGuid().ToString();
          foreach (int key in tableAndFieldValues.FieldValues.Keys)
          {
            List<string> stringList = new List<string>();
            List<DDMDataTableFieldValue> fieldValue2 = tableAndFieldValues.FieldValues[key];
            stringList.Add(dataTableExportLogID);
            foreach (DDMDataTableFieldValue ddmDataTableFieldValue in fieldValue2)
            {
              string str = CsvUtility.EscapeCommasInFieldValues(CsvUtility.FormatFieldValuesForCsv(ddmDataTableFieldValue));
              stringList.Add(str);
            }
            tableInfo.Add(stringList);
          }
          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
          saveFileDialog.FileName = tableAndFieldValues.Name + ".csv";
          if (saveFileDialog.ShowDialog() == DialogResult.OK)
          {
            if (File.Exists(saveFileDialog.FileName))
              File.Delete(saveFileDialog.FileName);
            using (TextWriter writer = (TextWriter) new StreamWriter(saveFileDialog.FileName, false))
            {
              CsvUtility.WriteRow(writer, rowInfo);
              CsvUtility.WriteRows(writer, tableInfo);
            }
          }
          DDMDataTableExportLog dataTableExportLog = new DDMDataTableExportLog(dataTableExportLogID, tableAndFieldValues.Name, tableAndFieldValues.Description, tableAndFieldValues.DataTableType, DateTime.Now, this.session.UserID, this.session.UserInfo.FullName);
          bpmManager.SaveDataTableExportLog(dataTableExportLog);
        }
      }
    }

    private void listViewOptions_ColumnClick(object source, GVColumnClickEventArgs e)
    {
      while (this.listViewOptions.Items.Any<GVItem>((Func<GVItem, bool>) (x => (string) x.SubItems[2].Value == "System Table")))
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewOptions.Items)
        {
          if ((string) gvItem.SubItems[2].Value == "System Table")
            this.listViewOptions.Items.Remove(gvItem);
        }
      }
      this.BindSystemTablesToGrid();
    }

    private void stdIconBtnCopy_Click(object sender, EventArgs e)
    {
      GVItem selectedItem = this.getSelectedItem();
      if (selectedItem == null)
        return;
      if ((string) selectedItem.SubItems[2].Value == "System Table")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please use the Settings under ‘Tables and Fees’ to manage this System table.");
      }
      else
      {
        DDMDataTableBpmManager bpmManager = (DDMDataTableBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMDataTables);
        DDMDataTable tag = (DDMDataTable) selectedItem.Tag;
        DDMDataTable ddmDataTable = (DDMDataTable) bpmManager.GetDDMDataTableAndFieldValues(tag.Id, true).Clone();
        string tableName = ddmDataTable.Name;
        int num2 = 1;
        while (bpmManager.DDMDataTableExists(tableName, true))
        {
          tableName = ddmDataTable.Name + string.Format("({0})", (object) num2);
          ++num2;
        }
        ddmDataTable.Name = tableName;
        bpmManager.UpsertDDMDataTable(ddmDataTable, forceToPrimaryDb: true);
        this.refreshGridWithData();
      }
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Data Tables";

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
      this.gcBaseRate = new GroupContainer();
      this.stdIconButtonExport = new StandardIconButton();
      this.stdIconBtnImport = new StandardIconButton();
      this.stdIconBtnCopy = new StandardIconButton();
      this.listViewOptions = new GridView();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.toolTip = new ToolTip(this.components);
      this.gcBaseRate.SuspendLayout();
      ((ISupportInitialize) this.stdIconButtonExport).BeginInit();
      ((ISupportInitialize) this.stdIconBtnImport).BeginInit();
      ((ISupportInitialize) this.stdIconBtnCopy).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.gcBaseRate.Borders = AnchorStyles.Top;
      this.gcBaseRate.Controls.Add((Control) this.stdIconButtonExport);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnImport);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnCopy);
      this.gcBaseRate.Controls.Add((Control) this.listViewOptions);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnNew);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcBaseRate.Dock = DockStyle.Fill;
      this.gcBaseRate.HeaderForeColor = SystemColors.ControlText;
      this.gcBaseRate.Location = new Point(0, 0);
      this.gcBaseRate.Name = "gcBaseRate";
      this.gcBaseRate.Size = new Size(800, 445);
      this.gcBaseRate.TabIndex = 4;
      this.gcBaseRate.Text = "Data Tables List";
      this.stdIconButtonExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconButtonExport.BackColor = Color.Transparent;
      this.stdIconButtonExport.Location = new Point(757, 5);
      this.stdIconButtonExport.MouseDownImage = (Image) null;
      this.stdIconButtonExport.Name = "stdIconButtonExport";
      this.stdIconButtonExport.Size = new Size(16, 16);
      this.stdIconButtonExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.stdIconButtonExport.TabIndex = 83;
      this.stdIconButtonExport.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconButtonExport, "Export...");
      this.stdIconButtonExport.Click += new EventHandler(this.stdIconButtonExport_Click);
      this.stdIconBtnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnImport.BackColor = Color.Transparent;
      this.stdIconBtnImport.Location = new Point(779, 5);
      this.stdIconBtnImport.MouseDownImage = (Image) null;
      this.stdIconBtnImport.Name = "stdIconBtnImport";
      this.stdIconBtnImport.Size = new Size(16, 16);
      this.stdIconBtnImport.StandardButtonType = StandardIconButton.ButtonType.ImportLoanButton;
      this.stdIconBtnImport.TabIndex = 82;
      this.stdIconBtnImport.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnImport, "Import");
      this.stdIconBtnImport.Click += new EventHandler(this.stdIconBtnImport_Click);
      this.stdIconBtnCopy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnCopy.BackColor = Color.Transparent;
      this.stdIconBtnCopy.Location = new Point(691, 5);
      this.stdIconBtnCopy.MouseDownImage = (Image) null;
      this.stdIconBtnCopy.Name = "stdIconBtnCopy";
      this.stdIconBtnCopy.Size = new Size(16, 16);
      this.stdIconBtnCopy.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdIconBtnCopy.TabIndex = 81;
      this.stdIconBtnCopy.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnCopy, "Duplicate");
      this.stdIconBtnCopy.Click += new EventHandler(this.stdIconBtnCopy_Click);
      this.listViewOptions.AutoHeight = true;
      this.listViewOptions.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 220;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Type";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Last Modified By";
      gvColumn4.Width = 150;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Last Modified Date & Time";
      gvColumn5.Width = 150;
      this.listViewOptions.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.listViewOptions.Dock = DockStyle.Fill;
      this.listViewOptions.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.listViewOptions.HeaderHeight = 22;
      this.listViewOptions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewOptions.Location = new Point(0, 26);
      this.listViewOptions.Name = "listViewOptions";
      this.listViewOptions.Size = new Size(800, 419);
      this.listViewOptions.SortingType = SortingType.AlphaNumeric;
      this.listViewOptions.TabIndex = 0;
      this.listViewOptions.SelectedIndexChanged += new EventHandler(this.listViewOptions_SelectedIndexChanged);
      this.listViewOptions.ColumnClick += new GVColumnClickEventHandler(this.listViewOptions_ColumnClick);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(669, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 79;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(713, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 77;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(735, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 74;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcBaseRate);
      this.Name = nameof (DataTableForm);
      this.Size = new Size(800, 445);
      this.gcBaseRate.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconButtonExport).EndInit();
      ((ISupportInitialize) this.stdIconBtnImport).EndInit();
      ((ISupportInitialize) this.stdIconBtnCopy).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
