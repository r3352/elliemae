// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DataTableImportParameters
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI.Import;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DataTableImportParameters : Form
  {
    private Sessions.Session _session;
    private string _csvFileName;
    private ProgressBarControl _progressControl;
    private GridView _gridControl;
    private BackgroundWorker _fileParserWorker;
    private bool _hasHeadersInformation;
    private bool _useQuotesToEscapeComments = true;
    public bool _hasSystemGeneratedColumn;
    private StandardFields _standardFields;
    private FieldSettings _fieldSettings;
    private bool overwriteExisting;
    private const int NUM_OF_OUTPUT_COL = 3;
    private IContainer components;
    private Label lblHorizontalSeparator1;
    private Label label1;
    private Label lblTableName;
    private Label label4;
    private Label lblTableDescription;
    private TextBox tbxTableName;
    private TextBox tbxTableDescription;
    private Label lblParsingOptions;
    private Label lblSettingsMessage;
    private Label label2;
    private Label lblDataToImport;
    private CheckBox cbxContainsHeaderInformation;
    private CheckBox cbxUseQuotesToEscape;
    private Button btnNext;
    private Button btnCancel;
    private FlowLayoutPanel pnlGridPlaceHolder;
    private ComboBox outputColComboBox;
    private Label label3;

    public DDMDataTable ImportedDataTable { get; private set; }

    public bool ForceRefreshLandingPage { get; private set; }

    public bool FileHasHeaderInformation => this._hasHeadersInformation;

    public bool UseQuotesToEscapeComments => this._useQuotesToEscapeComments;

    public DataTableImportParameters() => this.InitializeComponent();

    public DataTableImportParameters(
      Sessions.Session session,
      string fileName,
      StandardFields standardFields,
      FieldSettings fieldSettings)
    {
      this.InitializeComponent();
      this._session = session;
      this._csvFileName = fileName;
      this._standardFields = standardFields;
      this._fieldSettings = fieldSettings;
      this.ToggleTextBoxesReadonly(true);
      this.cbxUseQuotesToEscape.CheckedChanged -= new EventHandler(this.cbxUseQuotesToEscape_CheckedChanged);
      this.cbxUseQuotesToEscape.Checked = this._useQuotesToEscapeComments;
      this.cbxUseQuotesToEscape.CheckedChanged += new EventHandler(this.cbxUseQuotesToEscape_CheckedChanged);
      this.InitializeFlowLayoutPanelControls();
      this.InitializeAndStartBackgroundWorker();
      this.outputColComboBox.DisplayMember = "Name";
      this.outputColComboBox.ValueMember = "Id";
      this.outputColComboBox.Items.Add((object) new DataTableImportParameters.OutputColumnSelection("Please select...", 0));
      this.outputColComboBox.Items.Add((object) new DataTableImportParameters.OutputColumnSelection("1 Output Column", 1));
      this.outputColComboBox.Items.Add((object) new DataTableImportParameters.OutputColumnSelection("2 Output Columns", 2));
      this.outputColComboBox.Items.Add((object) new DataTableImportParameters.OutputColumnSelection("3 Output Columns", 3));
      this.outputColComboBox.Items.Add((object) new DataTableImportParameters.OutputColumnSelection("4 Output Columns", 4));
      this.outputColComboBox.Items.Add((object) new DataTableImportParameters.OutputColumnSelection("5 Output Columns", 5));
      this.outputColComboBox.Items.Add((object) new DataTableImportParameters.OutputColumnSelection("6 Output Columns", 6));
      this.outputColComboBox.Items.Add((object) new DataTableImportParameters.OutputColumnSelection("7 Output Columns", 7));
      this.outputColComboBox.Items.Add((object) new DataTableImportParameters.OutputColumnSelection("8 Output Columns", 8));
      this.outputColComboBox.SelectedIndex = 0;
    }

    private void ToggleTextBoxesReadonly(bool readOnly)
    {
      this.tbxTableDescription.ReadOnly = this.tbxTableName.ReadOnly = readOnly;
    }

    private void InitializeFlowLayoutPanelControls()
    {
      if (this.pnlGridPlaceHolder.Controls.Contains((Control) this._gridControl))
        this.pnlGridPlaceHolder.Controls.Remove((Control) this._gridControl);
      if (this.pnlGridPlaceHolder.Controls.Contains((Control) this._progressControl))
        this.pnlGridPlaceHolder.Controls.Remove((Control) this._progressControl);
      this._progressControl = new ProgressBarControl();
      this.pnlGridPlaceHolder.Controls.Add((Control) this._progressControl);
      this._progressControl.SetProgressText("Initializing....");
      this._progressControl.SetProgress(0);
    }

    private void InitializeAndStartBackgroundWorker()
    {
      if (this._fileParserWorker != null)
      {
        if (this._fileParserWorker.IsBusy)
          return;
        this._fileParserWorker.Dispose();
      }
      this._fileParserWorker = new BackgroundWorker();
      this._fileParserWorker.WorkerSupportsCancellation = true;
      this._fileParserWorker.WorkerReportsProgress = true;
      this._fileParserWorker.DoWork += new DoWorkEventHandler(this._fileParserWorker_DoWork);
      this._fileParserWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this._fileParserWorker_RunWorkerCompleted);
      this._fileParserWorker.ProgressChanged += new ProgressChangedEventHandler(this._fileParserWorker_ProgressChanged);
      this._fileParserWorker.RunWorkerAsync((object) this._csvFileName);
    }

    private void _fileParserWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      string path = (string) e.Argument;
      if (!File.Exists(path))
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format("Selected file '{0}' does not exist", (object) this._csvFileName), "File Does Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this._fileParserWorker.ReportProgress(100);
      }
      try
      {
        using (TextReader dataReader = (TextReader) File.OpenText(path))
        {
          this._fileParserWorker.ReportProgress(50);
          string[][] strArray = new CsvParserForDdm(dataReader, this._useQuotesToEscapeComments).RemainingRows();
          this._fileParserWorker.ReportProgress(75);
          e.Result = (object) strArray;
        }
      }
      finally
      {
        this._fileParserWorker.ReportProgress(100);
      }
    }

    private void _fileParserWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (this.pnlGridPlaceHolder.Contains((Control) this._progressControl))
        this.pnlGridPlaceHolder.Controls.Remove((Control) this._progressControl);
      if (e.Error != null)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format("Unexpcted error occurred.\n Details {0}", (object) e.Error.Message), "Error Importing Data Table", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.Close();
      }
      else
      {
        this.BuildGridView(e.Result as string[][]);
        this.pnlGridPlaceHolder.Controls.Add((Control) this._gridControl);
        this.ToggleTextBoxesReadonly(false);
      }
    }

    private void BuildGridView(string[][] result)
    {
      this.cbxContainsHeaderInformation.CheckedChanged -= new EventHandler(this.cbxContainsHeaderInformation_CheckedChanged);
      this.cbxUseQuotesToEscape.CheckedChanged -= new EventHandler(this.cbxUseQuotesToEscape_CheckedChanged);
      if (result == null || result.Length == 0)
        return;
      this._gridControl = new GridView();
      this._gridControl.Size = this.pnlGridPlaceHolder.Size;
      this._gridControl.Location = this.pnlGridPlaceHolder.Location;
      int length1 = result[0].Length;
      if (result[0][0] == "SystemLogID")
      {
        this._hasHeadersInformation = true;
        this._hasSystemGeneratedColumn = true;
        this.cbxContainsHeaderInformation.Checked = true;
        this.cbxContainsHeaderInformation.Enabled = false;
      }
      if (this._hasHeadersInformation)
      {
        string[] strArray = result[0];
        for (int index = this._hasSystemGeneratedColumn ? 1 : 0; index < length1; ++index)
        {
          try
          {
            this._gridControl.Columns.Add(new GVColumn(strArray[index].ToUpperInvariant()));
          }
          catch (ArgumentException ex)
          {
            int num = (int) MessageBox.Show((IWin32Window) this, "Input file contains duplicate columns. Please remove repeated columns and try again.", "Check CSV", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.Close();
            return;
          }
        }
      }
      else
      {
        for (int index = 0; index < length1; ++index)
          this._gridControl.Columns.Add(new GVColumn(string.Format("Field ID {0}", (object) (index + 1))));
      }
      if (this._hasSystemGeneratedColumn && this._hasHeadersInformation)
      {
        string dataTableExportLogID = result[1][0];
        DDMDataTableExportLog dataTableExportLog = ((DDMDataTableBpmManager) this._session.BPM.GetBpmManager(BpmCategory.DDMDataTables)).GetDataTableExportLog(dataTableExportLogID, true);
        if (dataTableExportLog != null)
          this.SetTableNameAndDescription(dataTableExportLog.DataTableName, dataTableExportLog.DataTableDescription);
      }
      int length2 = result.Length;
      for (int index1 = this._hasHeadersInformation ? 1 : 0; index1 < length2; ++index1)
      {
        try
        {
          if (result[index1].Length != length1)
            throw new IndexOutOfRangeException();
          GVItem gvItem = new GVItem();
          for (int index2 = this._hasSystemGeneratedColumn ? 1 : 0; index2 < length1; ++index2)
            gvItem.SubItems.Add((object) result[index1][index2]);
          this._gridControl.Items.Add(gvItem);
        }
        catch (IndexOutOfRangeException ex)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, "Input file contains one or more invalid rows. Please make sure the input file has same number of comma-separated values on each row and try again.", "Check CSV", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.Close();
          return;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, "Unexpected error parsing the file. Please contact your system administrator to resolve this issue.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.Close();
          return;
        }
      }
      this.cbxContainsHeaderInformation.CheckedChanged += new EventHandler(this.cbxContainsHeaderInformation_CheckedChanged);
      this.cbxUseQuotesToEscape.CheckedChanged += new EventHandler(this.cbxUseQuotesToEscape_CheckedChanged);
    }

    private void SetTableNameAndDescription(string tableName, string tableDescription)
    {
      if (string.IsNullOrEmpty(this.tbxTableName.Text))
        this.tbxTableName.Text = tableName;
      if (!string.IsNullOrEmpty(this.tbxTableDescription.Text))
        return;
      this.tbxTableDescription.Text = tableDescription;
    }

    private void _fileParserWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this._progressControl.SetProgressText(string.Format("{0}% of 100% completed", (object) e.ProgressPercentage));
      this._progressControl.SetProgress(e.ProgressPercentage);
    }

    private void cbxContainsHeaderInformation_CheckedChanged(object sender, EventArgs e)
    {
      this._hasHeadersInformation = this.cbxContainsHeaderInformation.Checked;
      if (this._gridControl == null || this._gridControl.Items.Count == 0)
        return;
      if (this._hasHeadersInformation)
      {
        GVItem gvItem = this._gridControl.Items[0];
        int nItemIndex = 0;
        foreach (GVColumn column in this._gridControl.Columns)
        {
          column.Name = column.Text = gvItem.SubItems[nItemIndex].Text;
          ++nItemIndex;
        }
        this._gridControl.Items.Remove(gvItem);
      }
      else
      {
        GVItem gvItem = new GVItem();
        int num = 0;
        foreach (GVColumn column in this._gridControl.Columns)
        {
          gvItem.SubItems.Add((object) column.Text);
          column.Name = column.Text = "Field ID " + (object) (num + 1);
          ++num;
        }
        this._gridControl.Items.Insert(0, gvItem);
      }
    }

    private void cbxUseQuotesToEscape_CheckedChanged(object sender, EventArgs e)
    {
      this._useQuotesToEscapeComments = this.cbxUseQuotesToEscape.Checked;
      this.InitializeFlowLayoutPanelControls();
      this.InitializeAndStartBackgroundWorker();
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void btnNext_Click(object sender, EventArgs e)
    {
      if (!this.ValidateParameters())
        return;
      string[] strArray = this.GetColumnsForImport();
      if (((DDMDataTableBpmManager) Session.BPM.GetBpmManager(BpmCategory.DDMDataTables)).DDMDataTableExists(this.tbxTableName.Text.Trim(), true))
      {
        if (MessageBox.Show((IWin32Window) this, "A data table with same name exists in the database. Do you want to overwrite the data table?", "Overwrite Existing Data Table?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
        {
          this.tbxTableName.Focus();
          return;
        }
        this.overwriteExisting = true;
      }
      DataTableImportParameters.OutputColumnSelection selectedItem = (DataTableImportParameters.OutputColumnSelection) this.outputColComboBox.SelectedItem;
      if (strArray.Length < selectedItem.Id)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have indicated more Output columns than the total number of columns in the import file. Please verify your selection.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (selectedItem.Id == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, string.Format("You must choose at least one output column."), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (strArray.Length == selectedItem.Id)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "Your selection indicates that the import file has only Output columns. Please include at least one column that maps to a Field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (strArray.Length - selectedItem.Id > 40)
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, string.Format("The data table can have a maximum of {0} fields (excluding the 'Output' columns).", (object) 40), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int count = strArray.Length - selectedItem.Id;
        Dictionary<string, List<string>> dataTableData = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> outputColDataTableData = new Dictionary<string, List<string>>();
        bool columnsChanged;
        if (!this.InitializeDataTableData(strArray, dataTableData, out columnsChanged))
          return;
        if (columnsChanged)
          strArray = dataTableData.Keys.ToArray<string>();
        string[] array1 = ((IEnumerable<string>) strArray).Take<string>(count).ToArray<string>();
        string[] array2 = ((IEnumerable<string>) strArray).Skip<string>(count).Take<string>(strArray.Length - count).ToArray<string>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this._gridControl.Items)
        {
          int index = 0;
          foreach (GVSubItem subItem in (IEnumerable<GVSubItem>) gvItem.SubItems)
          {
            string str = strArray[index];
            if (index < count)
            {
              dataTableData[str.ToUpper()].Add(subItem.Text);
            }
            else
            {
              dataTableData.Remove(str.ToUpper());
              if (outputColDataTableData.ContainsKey(str.ToUpper()))
                outputColDataTableData[str.ToUpper()].Add(subItem.Text);
              else
                outputColDataTableData.Add(str.ToUpper(), new List<string>()
                {
                  subItem.Text
                });
            }
            ++index;
          }
        }
        if (outputColDataTableData.Count < array2.Length)
        {
          int num5 = outputColDataTableData.Count + 1;
          for (int index1 = 0; index1 < this._gridControl.Items.Count; ++index1)
          {
            for (int index2 = num5; index2 <= 3; ++index2)
            {
              string str = "Output " + (object) index2;
              if (outputColDataTableData.ContainsKey(str.ToUpper()))
                outputColDataTableData[str.ToUpper()].Add(string.Empty);
              else
                outputColDataTableData.Add(str.ToUpper(), new List<string>()
                {
                  string.Empty
                });
            }
          }
        }
        this.Cursor = Cursors.WaitCursor;
        DataTableDlg dataTableDlg = new DataTableDlg(new DDMDataTable(0, this.tbxTableName.Text.Trim(), this.tbxTableDescription.Text.Trim(), DateTime.Now.ToString("MM/dd/yyyy h:m:s tt"), this._session.UserID, this._session.UserID + " (" + this._session.UserInfo.FullName + ")", string.Empty, string.Join("|", array1), string.Join("|", array2)), array1, array2, this._gridControl.Items.Count, dataTableData, outputColDataTableData, this._hasHeadersInformation, this.overwriteExisting, this._standardFields, this._fieldSettings);
        if (dataTableDlg.ShowDialog((IWin32Window) this) != DialogResult.Retry)
        {
          this.ForceRefreshLandingPage = this.overwriteExisting;
          this.ImportedDataTable = dataTableDlg.ImportedDataTable;
          this.Close();
        }
        this.Cursor = Cursors.Default;
      }
    }

    private bool ValidateParameters()
    {
      if (!string.IsNullOrEmpty(this.tbxTableName.Text.Trim()))
        return true;
      int num = (int) MessageBox.Show((IWin32Window) this, "Please enter a name for the data table.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private string[] GetColumnsForImport()
    {
      return this._gridControl.Columns.Select<GVColumn, string>((Func<GVColumn, string>) (gvColumn => gvColumn.Name)).ToList<string>().ToArray();
    }

    private bool InitializeDataTableData(
      string[] columns,
      Dictionary<string, List<string>> dataTableData,
      out bool columnsChanged)
    {
      columnsChanged = false;
      int num1 = 1;
      try
      {
        foreach (string column in columns)
        {
          if (string.IsNullOrEmpty(column))
          {
            dataTableData.Add(string.Format("Field ID {0}", (object) num1).ToUpper(), new List<string>());
            columnsChanged = true;
          }
          else
            dataTableData.Add(column.ToUpper(), new List<string>());
          ++num1;
        }
      }
      catch (ArgumentException ex)
      {
        int num2 = (int) MessageBox.Show((IWin32Window) this, "One or more duplicate Field IDs identified in the import file. Please verify.", "Check CSV", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      return true;
    }

    private void tbx_TextChanged(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      int selectionStart = textBox.SelectionStart;
      textBox.Text = Regex.Replace(textBox.Text, "[^0-9a-zA-Z ]", "");
      textBox.SelectionStart = selectionStart;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DataTableImportParameters));
      this.lblHorizontalSeparator1 = new Label();
      this.label1 = new Label();
      this.lblTableName = new Label();
      this.label4 = new Label();
      this.lblTableDescription = new Label();
      this.tbxTableName = new TextBox();
      this.tbxTableDescription = new TextBox();
      this.lblParsingOptions = new Label();
      this.lblSettingsMessage = new Label();
      this.label2 = new Label();
      this.lblDataToImport = new Label();
      this.cbxContainsHeaderInformation = new CheckBox();
      this.cbxUseQuotesToEscape = new CheckBox();
      this.btnNext = new Button();
      this.btnCancel = new Button();
      this.pnlGridPlaceHolder = new FlowLayoutPanel();
      this.outputColComboBox = new ComboBox();
      this.label3 = new Label();
      this.SuspendLayout();
      this.lblHorizontalSeparator1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.lblHorizontalSeparator1.BorderStyle = BorderStyle.Fixed3D;
      this.lblHorizontalSeparator1.Location = new Point(4, 106);
      this.lblHorizontalSeparator1.Name = "lblHorizontalSeparator1";
      this.lblHorizontalSeparator1.Size = new Size(488, 2);
      this.lblHorizontalSeparator1.TabIndex = 0;
      this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.label1.BorderStyle = BorderStyle.Fixed3D;
      this.label1.Location = new Point(4, 368);
      this.label1.Name = "label1";
      this.label1.Size = new Size(488, 2);
      this.label1.TabIndex = 1;
      this.lblTableName.AutoSize = true;
      this.lblTableName.Location = new Point(16, 14);
      this.lblTableName.Name = "lblTableName";
      this.lblTableName.Size = new Size(65, 13);
      this.lblTableName.TabIndex = 2;
      this.lblTableName.Text = "Table Name";
      this.label4.AutoSize = true;
      this.label4.ForeColor = Color.FromArgb(192, 0, 0);
      this.label4.Location = new Point(77, 13);
      this.label4.Margin = new Padding(2, 0, 2, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(11, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "*";
      this.lblTableDescription.AutoSize = true;
      this.lblTableDescription.Location = new Point(16, 40);
      this.lblTableDescription.Name = "lblTableDescription";
      this.lblTableDescription.Size = new Size(90, 13);
      this.lblTableDescription.TabIndex = 5;
      this.lblTableDescription.Text = "Table Description";
      this.tbxTableName.Location = new Point(119, 10);
      this.tbxTableName.MaxLength = 64;
      this.tbxTableName.Name = "tbxTableName";
      this.tbxTableName.Size = new Size(347, 20);
      this.tbxTableName.TabIndex = 6;
      this.tbxTableName.TextChanged += new EventHandler(this.tbx_TextChanged);
      this.tbxTableDescription.Location = new Point(119, 36);
      this.tbxTableDescription.MaxLength = 256;
      this.tbxTableDescription.Name = "tbxTableDescription";
      this.tbxTableDescription.Size = new Size(347, 20);
      this.tbxTableDescription.TabIndex = 7;
      this.tbxTableDescription.TextChanged += new EventHandler(this.tbx_TextChanged);
      this.lblParsingOptions.AutoSize = true;
      this.lblParsingOptions.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblParsingOptions.Location = new Point(16, 66);
      this.lblParsingOptions.Name = "lblParsingOptions";
      this.lblParsingOptions.Size = new Size(96, 13);
      this.lblParsingOptions.TabIndex = 8;
      this.lblParsingOptions.Text = "Parsing Options";
      this.lblSettingsMessage.AutoSize = true;
      this.lblSettingsMessage.Location = new Point(47, 83);
      this.lblSettingsMessage.Name = "lblSettingsMessage";
      this.lblSettingsMessage.Size = new Size(244, 13);
      this.lblSettingsMessage.TabIndex = 9;
      this.lblSettingsMessage.Text = "Specify the settings that are appropriate for this file";
      this.label2.Location = new Point(47, 124);
      this.label2.Name = "label2";
      this.label2.Size = new Size(418, 41);
      this.label2.TabIndex = 10;
      this.label2.Text = "Modify the options below so that the data in the CSV file is properly broken into columns. The following table shows how the rows will be parsed based on the current options selected.";
      this.lblDataToImport.AutoSize = true;
      this.lblDataToImport.Location = new Point(47, 175);
      this.lblDataToImport.Name = "lblDataToImport";
      this.lblDataToImport.Size = new Size(77, 13);
      this.lblDataToImport.TabIndex = 11;
      this.lblDataToImport.Text = "Data to Import:";
      this.cbxContainsHeaderInformation.AutoSize = true;
      this.cbxContainsHeaderInformation.Location = new Point(50, 299);
      this.cbxContainsHeaderInformation.Name = "cbxContainsHeaderInformation";
      this.cbxContainsHeaderInformation.Size = new Size(198, 17);
      this.cbxContainsHeaderInformation.TabIndex = 13;
      this.cbxContainsHeaderInformation.Text = "First row contains header information";
      this.cbxContainsHeaderInformation.UseVisualStyleBackColor = true;
      this.cbxContainsHeaderInformation.CheckedChanged += new EventHandler(this.cbxContainsHeaderInformation_CheckedChanged);
      this.cbxUseQuotesToEscape.AutoSize = true;
      this.cbxUseQuotesToEscape.Location = new Point(50, 322);
      this.cbxUseQuotesToEscape.Name = "cbxUseQuotesToEscape";
      this.cbxUseQuotesToEscape.Size = new Size(257, 17);
      this.cbxUseQuotesToEscape.TabIndex = 14;
      this.cbxUseQuotesToEscape.Text = "Use quotes as escape character to hide commas";
      this.cbxUseQuotesToEscape.UseVisualStyleBackColor = true;
      this.cbxUseQuotesToEscape.CheckedChanged += new EventHandler(this.cbxUseQuotesToEscape_CheckedChanged);
      this.btnNext.Location = new Point(325, 381);
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new Size(75, 23);
      this.btnNext.TabIndex = 15;
      this.btnNext.Text = "Next >";
      this.btnNext.UseVisualStyleBackColor = true;
      this.btnNext.Click += new EventHandler(this.btnNext_Click);
      this.btnCancel.Location = new Point(406, 381);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 16;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.pnlGridPlaceHolder.Location = new Point(50, 192);
      this.pnlGridPlaceHolder.Name = "pnlGridPlaceHolder";
      this.pnlGridPlaceHolder.Size = new Size(415, 100);
      this.pnlGridPlaceHolder.TabIndex = 18;
      this.outputColComboBox.FormattingEnabled = true;
      this.outputColComboBox.Location = new Point(245, 339);
      this.outputColComboBox.Name = "outputColComboBox";
      this.outputColComboBox.Size = new Size(121, 21);
      this.outputColComboBox.TabIndex = 0;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(47, 342);
      this.label3.Name = "label3";
      this.label3.Size = new Size(192, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Number of Output columns in Import file";
      this.AcceptButton = (IButtonControl) this.btnNext;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(494, 418);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.outputColComboBox);
      this.Controls.Add((Control) this.pnlGridPlaceHolder);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnNext);
      this.Controls.Add((Control) this.cbxUseQuotesToEscape);
      this.Controls.Add((Control) this.cbxContainsHeaderInformation);
      this.Controls.Add((Control) this.lblDataToImport);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.lblSettingsMessage);
      this.Controls.Add((Control) this.lblParsingOptions);
      this.Controls.Add((Control) this.tbxTableDescription);
      this.Controls.Add((Control) this.tbxTableName);
      this.Controls.Add((Control) this.lblTableDescription);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.lblTableName);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblHorizontalSeparator1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DataTableImportParameters);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Data Table Import Wizard";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum Screens
    {
      Screen1,
      Screen2,
    }

    private class OutputColumnSelection
    {
      private string _Name;
      private int _Id;

      public OutputColumnSelection(string name, int id)
      {
        this._Name = name;
        this._Id = id;
      }

      public string Name => this._Name;

      public int Id => this._Id;
    }
  }
}
