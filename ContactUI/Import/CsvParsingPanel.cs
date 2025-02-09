// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CsvParsingPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Wizard;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CsvParsingPanel : ContactImportWizardItem
  {
    private int maxRowsToDisplay = 100;
    private CsvImportParameters csvParams;
    private Panel panel2;
    private CheckBox chkUseQuotes;
    private CheckBox chkHeaderRow;
    private Label lblDataHeader;
    private ListView lvwData;
    private Label lblListHeader;
    private Label lblShowAll;
    private CheckBox chkShowAll;
    private IContainer components;

    public CsvParsingPanel(ContactImportWizardItem prevItem)
      : base(prevItem)
    {
      this.InitializeComponent();
      this.csvParams = (CsvImportParameters) this.ImportParameters.ImportOptions;
      this.displayParsedData();
      this.chkHeaderRow.Checked = this.csvParams.ExcludeFirstRow;
      this.chkUseQuotes.Checked = this.csvParams.UseQuotedStrings;
      if (this.csvParams.RowCount == 1 && !this.csvParams.ExcludeFirstRow)
        this.chkHeaderRow.Enabled = false;
      this.chkHeaderRow.Click += new EventHandler(this.chkHeaderRow_CheckedChanged);
      this.chkUseQuotes.Click += new EventHandler(this.chkUseQuotes_CheckedChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel2 = new Panel();
      this.lblListHeader = new Label();
      this.chkUseQuotes = new CheckBox();
      this.chkHeaderRow = new CheckBox();
      this.lblDataHeader = new Label();
      this.lvwData = new ListView();
      this.lblShowAll = new Label();
      this.chkShowAll = new CheckBox();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.lblShowAll);
      this.panel2.Controls.Add((Control) this.chkShowAll);
      this.panel2.Controls.Add((Control) this.lblListHeader);
      this.panel2.Controls.Add((Control) this.chkUseQuotes);
      this.panel2.Controls.Add((Control) this.chkHeaderRow);
      this.panel2.Controls.Add((Control) this.lblDataHeader);
      this.panel2.Controls.Add((Control) this.lvwData);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 10;
      this.lblListHeader.Location = new Point(38, 54);
      this.lblListHeader.Name = "lblListHeader";
      this.lblListHeader.Size = new Size(288, 16);
      this.lblListHeader.TabIndex = 8;
      this.lblListHeader.Text = "Data to Import:";
      this.chkUseQuotes.Location = new Point(38, 214);
      this.chkUseQuotes.Name = "chkUseQuotes";
      this.chkUseQuotes.Size = new Size(416, 18);
      this.chkUseQuotes.TabIndex = 7;
      this.chkUseQuotes.Text = "Use quotes as escape character to hide commas";
      this.chkHeaderRow.Location = new Point(38, 192);
      this.chkHeaderRow.Name = "chkHeaderRow";
      this.chkHeaderRow.Size = new Size(416, 18);
      this.chkHeaderRow.TabIndex = 6;
      this.chkHeaderRow.Text = "First row contains header information";
      this.lblDataHeader.Location = new Point(38, 9);
      this.lblDataHeader.Name = "lblDataHeader";
      this.lblDataHeader.Size = new Size(416, 42);
      this.lblDataHeader.TabIndex = 5;
      this.lblDataHeader.Text = "Modify the options below so that the data in the CSV file is properly broken into columns. The following table shows how the rows will be parsed based on the current options selected.";
      this.lvwData.GridLines = true;
      this.lvwData.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.lvwData.Location = new Point(38, 70);
      this.lvwData.MultiSelect = false;
      this.lvwData.Name = "lvwData";
      this.lvwData.Size = new Size(416, 118);
      this.lvwData.TabIndex = 4;
      this.lvwData.View = View.Details;
      this.lblShowAll.Location = new Point(344, 54);
      this.lblShowAll.Name = "lblShowAll";
      this.lblShowAll.Size = new Size(94, 16);
      this.lblShowAll.TabIndex = 13;
      this.lblShowAll.Text = "Show All:";
      this.lblShowAll.TextAlign = ContentAlignment.MiddleRight;
      this.lblShowAll.Visible = false;
      this.chkShowAll.Location = new Point(440, 56);
      this.chkShowAll.Name = "chkShowAll";
      this.chkShowAll.Size = new Size(14, 13);
      this.chkShowAll.TabIndex = 12;
      this.chkShowAll.Visible = false;
      this.chkShowAll.Click += new EventHandler(this.chkShowAll_Click);
      this.Controls.Add((Control) this.panel2);
      this.Header = "Parsing Options";
      this.Name = nameof (CsvParsingPanel);
      this.Subheader = "Specify the settings that are appropriate for this file";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void displayParsedData()
    {
      Cursor.Current = Cursors.WaitCursor;
      this.lvwData.BeginUpdate();
      try
      {
        this.lvwData.Clear();
        for (int index = 0; index < this.csvParams.ColumnCount; ++index)
        {
          ColumnHeader columnHeader = new ColumnHeader();
          columnHeader.Text = "Column " + (object) (index + 1);
          if (this.csvParams.Columns != null && this.csvParams.Columns.Length > index && this.csvParams.Columns[index] != CsvImportColumn.Unassigned)
            columnHeader.Text = this.csvParams.Columns[index].Description;
          columnHeader.Width = this.lvwData.ClientSize.Width / 2;
          this.lvwData.Columns.Add(columnHeader);
        }
        for (int index = 0; index < this.maxRowsToDisplay && index < this.csvParams.RowCount; ++index)
          this.lvwData.Items.Add(new ListViewItem(this.csvParams.GetRow(index)));
        this.lblListHeader.Text = "Data to Import:";
        if (this.maxRowsToDisplay >= this.csvParams.RowCount)
          return;
        Label lblListHeader = this.lblListHeader;
        lblListHeader.Text = lblListHeader.Text + " (first " + (object) this.maxRowsToDisplay + " records only)";
      }
      finally
      {
        this.lvwData.EndUpdate();
        Cursor.Current = Cursors.Default;
      }
    }

    private void chkHeaderRow_CheckedChanged(object sender, EventArgs e)
    {
      this.csvParams.ExcludeFirstRow = this.chkHeaderRow.Checked;
      if (this.csvParams.AutoAssignColumns)
      {
        if (this.chkHeaderRow.Checked)
          this.autoAssignColumns();
        else
          this.csvParams.Columns = (CsvImportColumn[]) null;
      }
      else if ((this.csvParams.ContactType == ContactType.TPO || this.csvParams.ContactType == ContactType.TPOCompany) && this.chkHeaderRow.Checked)
      {
        for (int index = 0; index < this.csvParams.Columns.Length; ++index)
        {
          if (string.Compare(this.csvParams.Columns[index].Description, "(Unassigned)", true) == 0 && Utils.Dialog((IWin32Window) this, "You have some unassigned columns in Import Wizard. Do you need Import Wizard to reassign those columns automatically for you?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            this.autoAssignColumns();
        }
      }
      this.displayParsedData();
    }

    private void chkUseQuotes_CheckedChanged(object sender, EventArgs e)
    {
      this.csvParams.UseQuotedStrings = this.chkUseQuotes.Checked;
      this.csvParams.ReParse();
      if (this.chkHeaderRow.Checked && this.csvParams.AutoAssignColumns)
        this.autoAssignColumns();
      this.displayParsedData();
    }

    private void autoAssignColumns()
    {
      string[] headerRow = this.csvParams.GetHeaderRow();
      CsvImportColumn[] csvImportColumnArray = new CsvImportColumn[this.csvParams.ColumnCount];
      for (int index = 0; index < csvImportColumnArray.Length; ++index)
        csvImportColumnArray[index] = CsvImportColumn.Unassigned;
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (CsvImportColumn allAvailableColumn in this.csvParams.GetAllAvailableColumns())
      {
        if (!insensitiveHashtable.ContainsKey((object) allAvailableColumn.Description))
          insensitiveHashtable.Add((object) allAvailableColumn.Description, (object) allAvailableColumn);
      }
      for (int index = 0; index < headerRow.Length; ++index)
      {
        if (insensitiveHashtable.ContainsKey((object) headerRow[index]))
          csvImportColumnArray[index] = (CsvImportColumn) insensitiveHashtable[(object) headerRow[index]];
        else if (insensitiveHashtable.ContainsKey((object) ("CustomField." + headerRow[index])))
          csvImportColumnArray[index] = (CsvImportColumn) insensitiveHashtable[(object) ("CustomField." + headerRow[index])];
      }
      this.csvParams.Columns = csvImportColumnArray;
    }

    public override WizardItem Next()
    {
      return (WizardItem) new CsvColumnSelectionPanel((ContactImportWizardItem) this);
    }

    private void chkShowAll_Click(object sender, EventArgs e)
    {
      this.maxRowsToDisplay = !this.chkShowAll.Checked ? 100 : int.MaxValue;
      this.displayParsedData();
    }
  }
}
