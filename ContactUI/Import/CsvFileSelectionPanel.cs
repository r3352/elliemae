// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CsvFileSelectionPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.Reporting;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CsvFileSelectionPanel : ContactImportWizardItem
  {
    private CsvImportParameters csvParams;
    private OpenFileDialog ofdBrowse;
    private Panel panel2;
    private Label label1;
    private TextBox txtFile;
    private Button btnBrowse;
    private IContainer components;

    public CsvFileSelectionPanel(ContactImportWizardItem prevItem)
      : base(prevItem)
    {
      this.csvParams = (CsvImportParameters) this.ImportParameters.ImportOptions;
      this.InitializeComponent();
    }

    public CsvFileSelectionPanel(ContactImportParameters importParams)
      : base(importParams)
    {
      this.csvParams = (CsvImportParameters) this.ImportParameters.ImportOptions;
      this.InitializeComponent();
      if (this.csvParams.ContactType != ContactType.TPO && this.csvParams.ContactType != ContactType.TPOCompany)
        return;
      if (this.csvParams.ContactType == ContactType.TPOCompany)
      {
        this.label1.Text = "Import company and branch data from:";
        this.label1.AutoSize = true;
      }
      this.SubheaderAutoSize = true;
      this.SetSubheaderLink("or click here to open import template", new Point(this.SubheaderLocationX + this.SubheaderWidth, this.SubheaderLocationY));
      this.SubheaderLinkVisible = true;
      this.SubheaderLink_Clicked = new EventHandler(this.subheaderLink_Clicked);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.ofdBrowse = new OpenFileDialog();
      this.panel2 = new Panel();
      this.label1 = new Label();
      this.txtFile = new TextBox();
      this.btnBrowse = new Button();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.ofdBrowse.DefaultExt = "csv";
      this.ofdBrowse.Filter = "Comma-Separated Value Files|*.csv|All Files|*.*";
      this.ofdBrowse.Title = "Select File to Import";
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.txtFile);
      this.panel2.Controls.Add((Control) this.btnBrowse);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 10;
      this.label1.Location = new Point(38, 64);
      this.label1.Name = "label1";
      this.label1.Size = new Size(174, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "Import contact data from:";
      this.txtFile.Location = new Point(38, 80);
      this.txtFile.Name = "txtFile";
      this.txtFile.Size = new Size(316, 20);
      this.txtFile.TabIndex = 4;
      this.txtFile.Text = "";
      this.btnBrowse.BackColor = SystemColors.Control;
      this.btnBrowse.Location = new Point(356, 78);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.TabIndex = 5;
      this.btnBrowse.Text = "Browse...";
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      this.Controls.Add((Control) this.panel2);
      this.Header = "File Selection";
      this.Name = nameof (CsvFileSelectionPanel);
      this.Subheader = "Browse for the CSV file to import";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void subheaderLink_Clicked(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      ExcelHandler excelHandler = new ExcelHandler();
      foreach (CsvImportColumn allAvailableColumn in this.csvParams.GetAllAvailableColumns())
        excelHandler.AddHeaderColumn(allAvailableColumn.Description, "@");
      excelHandler.SetFileFormatToCSV();
      excelHandler.CreateExcel(true);
      Cursor.Current = Cursors.Default;
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      if (this.ofdBrowse.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.txtFile.Text = this.ofdBrowse.FileName;
    }

    public override WizardItem Next()
    {
      if (this.txtFile.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a valid file name in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (WizardItem) null;
      }
      if (!File.Exists(this.txtFile.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified file could not be found. Enter a valid file name and try again.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (WizardItem) null;
      }
      if (this.txtFile.Text != this.csvParams.ImportFile)
      {
        this.csvParams.Reset();
        if (new ProgressDialog("Please Wait...", new AsynchronousProcess(this.parseFileData), (object) this.txtFile.Text, false).ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
          return (WizardItem) null;
        if (this.csvParams.RowCount == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "The specified file contains no data and cannot be imported.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.csvParams.Reset();
          return (WizardItem) null;
        }
      }
      return (WizardItem) new CsvParsingPanel((ContactImportWizardItem) this);
    }

    private DialogResult parseFileData(object filePath, IProgressFeedback feedback)
    {
      feedback.Status = "Reading data from file...";
      feedback.ResetCounter(30);
      try
      {
        using (new System.Threading.Timer(new TimerCallback(this.timerCallback), (object) feedback, 1000, 1000))
          this.csvParams.Parse(filePath.ToString());
        feedback.Increment(feedback.MaxValue);
        return DialogResult.OK;
      }
      catch (IOException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified file could not be read. It may be inaccessible or in use by another process.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return DialogResult.Abort;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "An error occurred while parsing the data: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return DialogResult.Abort;
      }
    }

    private void timerCallback(object feedbackAsObject)
    {
      ((IServerProgressFeedback) feedbackAsObject).Increment(1);
    }
  }
}
