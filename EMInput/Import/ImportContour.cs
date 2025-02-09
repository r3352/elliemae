// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.ImportContour
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using csiClickloanLib;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using emiDatabaseLib5;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class ImportContour : Form
  {
    private const string className = "ImportContour";
    private static readonly string sw = Tracing.SwImportExport;
    private System.ComponentModel.Container components;
    private Label sourceLbl;
    private ComboBox sourceCbo;
    private GroupBox groupBox2;
    private ComboBox destinationCbo;
    private Button importBtn;
    private ListView borsListView;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private Label destinationLbl;
    private Button selectAllBtn;
    private Button cancelBtn;
    private Button dataFolderBrowse;
    private FolderBrowserDialog folderBrowserDialog;
    private ListViewColumnSorter lvwColumnSorter;
    private string BorPath = string.Empty;
    private DatabaseLibraryClass dbcBorrowers = new DatabaseLibraryClass();
    private bool isManualBrowse;
    private ArrayList manualIndex = new ArrayList();
    private ImportProgress progressForm;
    private string loanOfficerId = string.Empty;

    public ImportContour(string loanOfficerId)
    {
      this.loanOfficerId = loanOfficerId;
      this.Cursor = Cursors.WaitCursor;
      this.InitializeComponent();
      this.ReadINIfile();
      this.InitializeColumnSorter();
      string[] foldersForAction = ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetLoanFoldersForAction(LoanFolderAction.Import);
      int selectedIndex = -1;
      this.destinationCbo.Items.AddRange((object[]) LoanFolderUtil.LoanFolderNames2LoanFolderInfos(foldersForAction, Session.UserInfo.WorkingFolder, out selectedIndex));
      if (selectedIndex >= 0)
        this.destinationCbo.SelectedIndex = selectedIndex;
      else if (this.destinationCbo.Items.Count > 0)
        this.destinationCbo.SelectedIndex = 0;
      this.Cursor = Cursors.Default;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.sourceLbl = new Label();
      this.sourceCbo = new ComboBox();
      this.borsListView = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.groupBox2 = new GroupBox();
      this.destinationCbo = new ComboBox();
      this.destinationLbl = new Label();
      this.importBtn = new Button();
      this.selectAllBtn = new Button();
      this.cancelBtn = new Button();
      this.dataFolderBrowse = new Button();
      this.folderBrowserDialog = new FolderBrowserDialog();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.folderBrowserDialog.Description = "Select Contour data folder.";
      this.folderBrowserDialog.ShowNewFolderButton = false;
      this.sourceLbl.Location = new Point(8, 8);
      this.sourceLbl.Name = "sourceLbl";
      this.sourceLbl.Size = new Size(96, 16);
      this.sourceLbl.TabIndex = 0;
      this.sourceLbl.Text = "Borrower Folder:";
      this.sourceCbo.Location = new Point(8, 24);
      this.sourceCbo.Name = "sourceCbo";
      this.sourceCbo.Size = new Size(384, 21);
      this.sourceCbo.TabIndex = 9;
      this.sourceCbo.SelectedIndexChanged += new EventHandler(this.sourceCbo_SelectedIndexChanged);
      this.borsListView.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4,
        this.columnHeader5
      });
      this.borsListView.FullRowSelect = true;
      this.borsListView.HideSelection = false;
      this.borsListView.Location = new Point(8, 56);
      this.borsListView.Name = "borsListView";
      this.borsListView.Size = new Size(504, 312);
      this.borsListView.Sorting = SortOrder.Ascending;
      this.borsListView.TabIndex = 5;
      this.borsListView.View = View.Details;
      this.borsListView.ColumnClick += new ColumnClickEventHandler(this.borsListView_ColumnClick);
      this.columnHeader1.Text = "Item";
      this.columnHeader1.Width = 0;
      this.columnHeader2.Text = "LastName";
      this.columnHeader2.Width = 100;
      this.columnHeader3.Text = "FirstName";
      this.columnHeader3.Width = 100;
      this.columnHeader4.Text = "FileName";
      this.columnHeader4.Width = 200;
      this.columnHeader5.Text = "RepName";
      this.columnHeader5.Width = 117;
      this.groupBox2.Controls.Add((Control) this.destinationCbo);
      this.groupBox2.Controls.Add((Control) this.destinationLbl);
      this.groupBox2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupBox2.Location = new Point(13, 400);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(500, 63);
      this.groupBox2.TabIndex = 6;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Import To:";
      this.destinationCbo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.destinationCbo.Location = new Point(100, 28);
      this.destinationCbo.Name = "destinationCbo";
      this.destinationCbo.Size = new Size(307, 21);
      this.destinationCbo.TabIndex = 3;
      this.destinationLbl.Location = new Point(20, 28);
      this.destinationLbl.Name = "destinationLbl";
      this.destinationLbl.Size = new Size(73, 20);
      this.destinationLbl.TabIndex = 2;
      this.destinationLbl.Text = "Loan Folder:";
      this.importBtn.DialogResult = DialogResult.OK;
      this.importBtn.Location = new Point(352, 472);
      this.importBtn.Name = "importBtn";
      this.importBtn.Size = new Size(75, 24);
      this.importBtn.TabIndex = 7;
      this.importBtn.Text = "&Import";
      this.importBtn.Click += new EventHandler(this.importBtn_Click);
      this.selectAllBtn.Location = new Point(436, 376);
      this.selectAllBtn.Name = "selectAllBtn";
      this.selectAllBtn.Size = new Size(75, 24);
      this.selectAllBtn.TabIndex = 10;
      this.selectAllBtn.Text = "&Select All";
      this.selectAllBtn.Click += new EventHandler(this.selectAllBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(436, 472);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 12;
      this.cancelBtn.Text = "&Close";
      this.dataFolderBrowse.Location = new Point(416, 24);
      this.dataFolderBrowse.Name = "dataFolderBrowse";
      this.dataFolderBrowse.Size = new Size(96, 23);
      this.dataFolderBrowse.TabIndex = 13;
      this.dataFolderBrowse.Text = "Browse...";
      this.dataFolderBrowse.Click += new EventHandler(this.dataFolderBrowse_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(522, 503);
      this.Controls.Add((Control) this.dataFolderBrowse);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.selectAllBtn);
      this.Controls.Add((Control) this.importBtn);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.borsListView);
      this.Controls.Add((Control) this.sourceCbo);
      this.Controls.Add((Control) this.sourceLbl);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportContour);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import From Contour - TLH 5.3";
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void InitializeColumnSorter()
    {
      this.lvwColumnSorter = new ListViewColumnSorter();
      this.borsListView.ListViewItemSorter = (IComparer) this.lvwColumnSorter;
      this.lvwColumnSorter.SortColumn = 0;
      this.lvwColumnSorter.SortType = 0;
      this.lvwColumnSorter.Order = SortOrder.Ascending;
    }

    [DllImport("kernel32", EntryPoint = "GetPrivateProfileStringA", CharSet = CharSet.Ansi)]
    private static extern int GetPrivateProfileString(
      string sectionName,
      string keyName,
      string defaultValue,
      StringBuilder returnbuffer,
      int buffersize,
      string filename);

    private void ReadINIfile()
    {
      string str1 = Environment.GetEnvironmentVariable("winDir") + "\\CSI.INI";
      if (!File.Exists(str1))
        return;
      StringBuilder returnbuffer = new StringBuilder(256);
      int privateProfileString = ImportContour.GetPrivateProfileString("Clickloan", "ApplicationDirectory", "", returnbuffer, 256, str1);
      string str2 = returnbuffer.ToString();
      if (str2 == string.Empty || str2 == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Unable to find TLH application path '" + str2 + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string filename = str2 + "\\TLH.INI";
        privateProfileString = ImportContour.GetPrivateProfileString("Setup", "BorrowerDirectory", "", returnbuffer, 256, filename);
        this.BorPath = returnbuffer.ToString();
        if (this.BorPath == string.Empty || this.BorPath == null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Unable to find Borrowers path from file: '" + filename + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string[] directories = Directory.GetDirectories(this.BorPath);
          if (directories.Length == 0)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "Unable to find Borrower folders from path '" + this.BorPath + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            foreach (object obj in directories)
              this.sourceCbo.Items.Add(obj);
            this.InitializeDatabase();
            this.sourceCbo.SelectedIndex = 0;
          }
        }
      }
    }

    private void InitializeDatabase()
    {
      this.dbcBorrowers.UDLFile = this.BorPath + "\\BORROWER.UDL";
      this.dbcBorrowers.SQLTable = "BORS";
      this.dbcBorrowers.SQLFields = "TLH_1_37,TLH_1_36,FILENAME,TLH_1_317";
      this.dbcBorrowers.SQLOrderBy = "FILENAME";
    }

    private void BuildBorList()
    {
      this.Cursor = Cursors.WaitCursor;
      this.borsListView.Items.Clear();
      string str = this.sourceCbo.SelectedItem.ToString();
      if (str != string.Empty)
        str = str.Substring(str.LastIndexOf("\\") + 1);
      string empty = string.Empty;
      this.dbcBorrowers.SQLCriteria = "FILENAME LIKE '" + str + "\\%'";
      if (!this.dbcBorrowers.OpenRecords(ref empty))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Unable to open Contour database BORS.mdb. Please contact tech support.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.dbcBorrowers.MoveFirst();
        for (int index = 0; index < this.dbcBorrowers.RecordCount; ++index)
        {
          ListViewItem listViewItem = new ListViewItem(index.ToString());
          for (int Field = 0; Field < 4; ++Field)
            listViewItem.SubItems.Add(this.dbcBorrowers.get_FieldValue((object) Field).ToString());
          this.borsListView.Items.Add(listViewItem);
          this.dbcBorrowers.MoveNext();
        }
        this.borsListView.Columns[0].Width = 0;
        this.dbcBorrowers.CloseRecords();
        this.Cursor = Cursors.Default;
      }
    }

    private bool LoadContourFile(
      string fileName,
      ArrayList coMortgagers,
      CFileImportExportClass bamImport)
    {
      if (!File.Exists(fileName))
      {
        Tracing.Log(ImportContour.sw, TraceLevel.Error, nameof (ImportContour), "The Contour borrower file '" + fileName + "' does not exists.\r\n");
        return false;
      }
      if (!bamImport.LoadBorrower(fileName))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There was a problem loading the Contour borrower file '" + fileName + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      for (short iBorrower = 1; (int) iBorrower <= (int) bamImport.NumberOfBorrower; ++iBorrower)
      {
        if (iBorrower > (short) 1)
          coMortgagers.Add((object) bamImport.GetBAMFileName(iBorrower));
      }
      return true;
    }

    private void importBtn_Click(object sender, EventArgs e)
    {
      ContactUtil.InitContactUtil();
      if (this.destinationCbo.SelectedItem == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to choose a destination folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.borsListView.SelectedItems.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to select at least one file in order to import.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string name = ((LoanFolderInfo) this.destinationCbo.SelectedItem).Name;
        if (this.isManualBrowse)
          this.BorPath = this.sourceCbo.SelectedItem.ToString();
        string[] fileNames = new string[this.borsListView.SelectedItems.Count];
        int num3 = 0;
        foreach (ListViewItem selectedItem in this.borsListView.SelectedItems)
          fileNames[num3++] = selectedItem.SubItems[3].Text.ToString();
        this.progressForm = new ImportProgress(this.borsListView.SelectedItems.Count);
        this.progressForm.progressLbl.Text = "Contour Software";
        ImportContour.ContourImportParameters parameter = new ImportContour.ContourImportParameters(name, this.BorPath, fileNames);
        Thread thread = new Thread(new ParameterizedThreadStart(this.ImportThread));
        thread.Start((object) parameter);
        Session.ISession.LoanImportInProgress = true;
        if (DialogResult.Cancel == this.progressForm.ShowDialog((IWin32Window) this))
          thread.Abort();
        Session.ISession.LoanImportInProgress = false;
      }
    }

    private void ImportThread(object inputParameter)
    {
      ImportContour.ContourImportParameters importParameters = inputParameter as ImportContour.ContourImportParameters;
      long importedCount = 0;
      if (importParameters != null)
      {
        ArrayList coMortgagers = new ArrayList();
        ContourImport contourImport = new ContourImport(Session.SessionObjects, this.loanOfficerId);
        CFileImportExportClass importExportClass = new CFileImportExportClass();
        if ((EnableDisableSetting) Session.ServerManager.GetServerSetting("Import.LoanNumbering") == EnableDisableSetting.Enabled)
          contourImport.UseEMLoanNumbering = true;
        foreach (string fileName in importParameters.FileNames)
        {
          this.updateProgress(fileName);
          if (coMortgagers.Contains((object) (importParameters.BorrowerPath + "\\" + fileName)))
          {
            ++importedCount;
          }
          else
          {
            try
            {
              if (this.LoadContourFile(importParameters.BorrowerPath + "\\" + fileName, coMortgagers, importExportClass))
              {
                if (!importExportClass.GetBAMFieldValue("1000", (short) 0).StartsWith("5."))
                {
                  Tracing.Log(ImportContour.sw, TraceLevel.Error, nameof (ImportContour), "The file '" + fileName + "'. Does not have the right Contour format.\r\n");
                }
                else
                {
                  LoanDataMgr loanDataMgr = contourImport.ImportFile(importExportClass);
                  importExportClass.CloseBorrower();
                  loanDataMgr.FromLoanImport = true;
                  loanDataMgr.Create(importParameters.EmliteFolder, "");
                  loanDataMgr.Close();
                  ++importedCount;
                }
              }
            }
            catch (Exception ex)
            {
              Tracing.Log(ImportContour.sw, TraceLevel.Error, nameof (ImportContour), "Problem importing Contour file:  " + fileName + ", exception: " + ex.Message + "\r\n");
            }
          }
        }
      }
      this.closeProgress(importedCount);
    }

    private void updateProgress(string fileName)
    {
      if (this.progressForm.InvokeRequired)
        this.Invoke((Delegate) new ImportContour.UpdateProgressCallback(this.updateProgress), (object) fileName);
      else
        this.progressForm.CurrentFile = fileName;
    }

    private void closeProgress(long importedCount)
    {
      if (this.progressForm.InvokeRequired)
      {
        this.Invoke((Delegate) new ImportContour.CloseProgressCallback(this.closeProgress), (object) importedCount);
      }
      else
      {
        string text = importedCount.ToString() + " out of " + (object) this.borsListView.SelectedItems.Count + " file(s) were imported.";
        if (importedCount < (long) this.borsListView.SelectedItems.Count)
        {
          if (DialogResult.Yes == Utils.Dialog((IWin32Window) this.progressForm, text + Environment.NewLine + "Would you like to open the Log File to view the log of files that were not imported?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            SystemUtil.ShellExecute(Tracing.LogFile);
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) this.progressForm, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        this.progressForm.Close();
      }
    }

    private void sourceCbo_SelectedIndexChanged(object sender, EventArgs e)
    {
      int selectedIndex = this.sourceCbo.SelectedIndex;
      if (selectedIndex < 0)
        return;
      if (this.manualIndex.Contains((object) selectedIndex))
      {
        this.isManualBrowse = true;
        this.BuildManualBorList();
      }
      else
      {
        this.isManualBrowse = false;
        this.BuildBorList();
      }
    }

    private void BuildManualBorList()
    {
      string path = this.sourceCbo.SelectedItem.ToString();
      this.Cursor = Cursors.WaitCursor;
      this.borsListView.Items.Clear();
      foreach (object file in Directory.GetFiles(path, "*.?"))
      {
        string str1 = file.ToString();
        string str2 = str1.Substring(str1.LastIndexOf("\\") + 1);
        this.borsListView.Items.Add(new ListViewItem("")
        {
          SubItems = {
            "",
            "",
            str2,
            ""
          }
        });
      }
      this.Cursor = Cursors.Default;
    }

    private void borsListView_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      this.lvwColumnSorter.SortColumn = e.Column;
      this.lvwColumnSorter.SortType = 0;
      this.lvwColumnSorter.Order = this.lvwColumnSorter.Order != SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending;
      this.borsListView.Sort();
    }

    private void selectAllBtn_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.borsListView.Items)
        listViewItem.Selected = true;
    }

    private void dataFolderBrowse_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      string selectedPath = this.folderBrowserDialog.SelectedPath;
      if (this.sourceCbo.Items.Contains((object) selectedPath))
      {
        this.sourceCbo.SelectedItem = (object) selectedPath;
      }
      else
      {
        int num = this.sourceCbo.Items.Add((object) selectedPath);
        this.manualIndex.Add((object) num);
        this.sourceCbo.SelectedIndex = num;
        this.BuildManualBorList();
      }
    }

    private delegate void UpdateProgressCallback(string text);

    private delegate void CloseProgressCallback(long importedCount);

    public class ContourImportParameters
    {
      private string emliteFolder = string.Empty;
      private string borrowerPath = string.Empty;
      private string[] fileNames;

      public string EmliteFolder => this.emliteFolder;

      public string BorrowerPath => this.borrowerPath;

      public string[] FileNames => this.fileNames;

      public ContourImportParameters(string emliteFolder, string borrowerPath, string[] fileNames)
      {
        this.emliteFolder = emliteFolder;
        this.borrowerPath = borrowerPath;
        this.fileNames = fileNames;
      }
    }
  }
}
