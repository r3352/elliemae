// Decompiled with JetBrains decompiler
// Type: ImportGenesisData.GenesisImportDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using ExpLoansFromGenesisLib;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace ImportGenesisData
{
  public class GenesisImportDialog : Form
  {
    private System.ComponentModel.Container components;
    private ListView LoanList;
    private ColumnHeader ColumnHeader1;
    private ColumnHeader FileNM;
    private ColumnHeader FirstName;
    private ColumnHeader LastName;
    private ColumnHeader Status;
    private ColumnHeader LoanProcessor;
    private ColumnHeader LoanOfficer;
    private Label label1;
    private ComboBox Paths;
    private GroupBox groupBox1;
    private RadioButton Prequal;
    private RadioButton Proccessing;
    private GroupBox groupBox2;
    private Label label2;
    private ComboBox folderCombo;
    private Button cancelBtn;
    private Button importBtn;
    private Label label3;
    private Label label4;
    private TextBox LoginName;
    private TextBox Password;
    private Button GetList;
    private Button SelectAll;
    private ColumnHeader LoanNo;
    private ColumnHeader Property;
    private GroupBox groupBox3;
    private RadioButton Active;
    private RadioButton Archived;
    private ColumnHeader LoanType;
    private Label TotalFiles;
    private Button dataFolderBrowse;
    private FolderBrowserDialog folderBrowserDialog;
    private BackgroundWorker loadLoanListWorker;
    private const string className = "ImportGenesis";
    private static readonly string sw = Tracing.SwImportExport;
    public ImportProgress progressDialog;
    private LoanListClass database;
    private bool firstTime = true;
    private string mapfile;
    private string loanOfficerId = string.Empty;

    public GenesisImportDialog(string loanOfficerId)
    {
      this.loanOfficerId = loanOfficerId;
      this.InitializeComponent();
      this.initializeBackgoundWorker();
      string[] foldersForAction = ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetLoanFoldersForAction(LoanFolderAction.Import);
      int selectedIndex = -1;
      this.folderCombo.Items.AddRange((object[]) LoanFolderUtil.LoanFolderNames2LoanFolderInfos(foldersForAction, Session.UserInfo.WorkingFolder, out selectedIndex));
      if (selectedIndex >= 0)
        this.folderCombo.SelectedIndex = selectedIndex;
      else if (this.folderCombo.Items.Count > 0)
        this.folderCombo.SelectedIndex = 0;
      this.database = new LoanListClass();
      if (this.GetGenesisInstalledPaths())
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Genesis installed paths could not be retrieved", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void initializeBackgoundWorker()
    {
      this.loadLoanListWorker = new BackgroundWorker();
      this.loadLoanListWorker.WorkerReportsProgress = true;
      this.loadLoanListWorker.WorkerSupportsCancellation = true;
      this.loadLoanListWorker.DoWork += new DoWorkEventHandler(this.loadLoanListWorker_DoWork);
      this.loadLoanListWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.loadLoanListWorker_RunWorkerCompleted);
      this.loadLoanListWorker.ProgressChanged += new ProgressChangedEventHandler(this.loadLoanListWorker_ProgressChanged);
    }

    private void loadLoanListWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;
      e.Result = (object) this.LoadLoanList(worker, e);
    }

    private void loadLoanListWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.progressDialog.Close();
      this.LoanList.Items.Clear();
      if (e.Error != null)
      {
        int num = (int) MessageBox.Show(e.Error.Message);
      }
      else if (!e.Cancelled)
      {
        this.LoanList.BeginUpdate();
        this.LoanList.Items.AddRange((ListViewItem[]) e.Result);
        this.LoanList.ListViewItemSorter = (IComparer) new ListViewItemComparer(1);
        this.LoanList.Sort();
        this.LoanList.EndUpdate();
      }
      this.TotalFiles.Text = "Total Loan Files: " + this.LoanList.Items.Count.ToString();
    }

    private void loadLoanListWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.progressDialog.CurrentFile = e.ProgressPercentage.ToString();
    }

    private bool GetGenesisInstalledPaths()
    {
      RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("Software\\\\Genesis 2000\\\\Genesis 2000");
      if (registryKey1 == null)
        return false;
      foreach (string subKeyName in registryKey1.GetSubKeyNames())
      {
        RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("Software\\\\Genesis 2000\\\\Genesis 2000\\\\" + subKeyName);
        if (registryKey2 == null)
          return false;
        string str1 = (string) registryKey2.GetValue("Path");
        string str2 = (string) registryKey2.GetValue("Version");
        if (str2 != null && str1 != null && str2.IndexOf("17") >= 0)
          this.Paths.Items.Add((object) str1);
      }
      if (this.Paths.Items.Count <= 0)
        return false;
      this.Paths.SelectedIndex = 0;
      return true;
    }

    private bool GetGenesisLoanList(ClickFrom clk)
    {
      string text = this.Paths.Text;
      if (!File.Exists(text + "\\DEFDIR.DBF"))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, text + " is not a valid Genesis installation path.  Please enter a valid installation path.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.database.IsInstallationAbove17(text) == 0)
        return false;
      if (text != string.Empty)
        this.database.SetDBFPATH(text);
      if (!this.ValidateInput(clk))
        return false;
      if (text != string.Empty)
      {
        this.database.CloseGenesisLoanData();
        if ((!this.Proccessing.Checked ? (!this.Active.Checked ? this.database.OpenGenesisLoanData("V", "W") : this.database.OpenGenesisLoanData("Q", "R")) : (!this.Active.Checked ? this.database.OpenGenesisLoanData("A", "B") : this.database.OpenGenesisLoanData(" ", "A"))) != null)
        {
          int totalRecords = this.database.TotalRecords;
          if (totalRecords > 0)
          {
            this.progressDialog = new ImportProgress(totalRecords);
            this.progressDialog.label1.Text = "Loading " + this.database.TotalRecords.ToString() + " Files..";
            this.progressDialog.fileLbl.Text = "Loaded Files: ";
            this.loadLoanListWorker.RunWorkerAsync();
            if (DialogResult.Cancel == this.progressDialog.ShowDialog((IWin32Window) this))
            {
              this.loadLoanListWorker.CancelAsync();
              return false;
            }
          }
          return true;
        }
        int num = (int) Utils.Dialog((IWin32Window) this, "Error accessing genesis loan data.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a Genesis Installation.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private ListViewItem[] LoadLoanList(BackgroundWorker worker, DoWorkEventArgs e)
    {
      List<ListViewItem> listViewItemList = new List<ListViewItem>();
      int num = 0;
      for (int index = this.database.MoveToFirstLoan(); index != 0; index = this.database.MoveToNextLoan())
      {
        if (worker.CancellationPending)
        {
          e.Cancel = true;
          break;
        }
        worker.ReportProgress(++num);
        listViewItemList.Add(new ListViewItem()
        {
          SubItems = {
            this.database.LastName,
            this.database.FirstName,
            this.database.Property,
            this.database.LoanNo,
            this.database.LoanType,
            this.database.LoanOfficer,
            this.database.LoanProcessor,
            this.database.Status,
            this.database.FileNM
          }
        });
      }
      return listViewItemList.ToArray();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void LoanList_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      this.LoanList.ListViewItemSorter = (IComparer) new ListViewItemComparer(e.Column);
      this.LoanList.Sort();
    }

    private void Processing_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.Proccessing.Checked)
        return;
      this.LoanList.Items.Clear();
      if (this.LoginName.Text == null || !(this.LoginName.Text != string.Empty))
        return;
      this.GetGenesisLoanList(ClickFrom.Other);
    }

    private void Prequal_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.Prequal.Checked)
        return;
      this.LoanList.Items.Clear();
      if (this.LoginName.Text == null || !(this.LoginName.Text != string.Empty))
        return;
      this.GetGenesisLoanList(ClickFrom.Other);
    }

    private void InitializeComponent()
    {
      this.LoanList = new ListView();
      this.ColumnHeader1 = new ColumnHeader();
      this.LastName = new ColumnHeader();
      this.FirstName = new ColumnHeader();
      this.Property = new ColumnHeader();
      this.LoanNo = new ColumnHeader();
      this.LoanType = new ColumnHeader();
      this.LoanOfficer = new ColumnHeader();
      this.LoanProcessor = new ColumnHeader();
      this.Status = new ColumnHeader();
      this.FileNM = new ColumnHeader();
      this.label1 = new Label();
      this.Paths = new ComboBox();
      this.groupBox1 = new GroupBox();
      this.Proccessing = new RadioButton();
      this.Prequal = new RadioButton();
      this.groupBox2 = new GroupBox();
      this.folderCombo = new ComboBox();
      this.label2 = new Label();
      this.cancelBtn = new Button();
      this.importBtn = new Button();
      this.label3 = new Label();
      this.label4 = new Label();
      this.LoginName = new TextBox();
      this.Password = new TextBox();
      this.GetList = new Button();
      this.SelectAll = new Button();
      this.groupBox3 = new GroupBox();
      this.Archived = new RadioButton();
      this.Active = new RadioButton();
      this.TotalFiles = new Label();
      this.dataFolderBrowse = new Button();
      this.folderBrowserDialog = new FolderBrowserDialog();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      this.folderBrowserDialog.Description = "Select Genesis installation path.";
      this.folderBrowserDialog.ShowNewFolderButton = false;
      this.LoanList.Columns.AddRange(new ColumnHeader[10]
      {
        this.ColumnHeader1,
        this.LastName,
        this.FirstName,
        this.Property,
        this.LoanNo,
        this.LoanType,
        this.LoanOfficer,
        this.LoanProcessor,
        this.Status,
        this.FileNM
      });
      this.LoanList.FullRowSelect = true;
      this.LoanList.HideSelection = false;
      this.LoanList.Location = new Point(16, 128);
      this.LoanList.Name = "LoanList";
      this.LoanList.Size = new Size(488, 248);
      this.LoanList.Sorting = SortOrder.Ascending;
      this.LoanList.TabIndex = 8;
      this.LoanList.View = View.Details;
      this.LoanList.DoubleClick += new EventHandler(this.LoanList_DoubleClick);
      this.LoanList.ColumnClick += new ColumnClickEventHandler(this.LoanList_ColumnClick);
      this.ColumnHeader1.Text = "Item";
      this.ColumnHeader1.Width = 0;
      this.LastName.Text = "Last Name";
      this.LastName.Width = 90;
      this.FirstName.Text = "First Name";
      this.FirstName.Width = 90;
      this.Property.Text = "Property";
      this.Property.Width = 90;
      this.LoanNo.Text = "LoanNo";
      this.LoanNo.Width = 90;
      this.LoanType.Text = "Loan Type";
      this.LoanType.Width = 90;
      this.LoanOfficer.Text = "Loan Officer";
      this.LoanOfficer.Width = 70;
      this.LoanProcessor.Text = "Loan Processor";
      this.LoanProcessor.Width = 94;
      this.Status.Text = "Status";
      this.Status.Width = 55;
      this.FileNM.Text = "FileNM";
      this.FileNM.Width = 64;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(264, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Genesis Installation Paths of version 17 and above:";
      this.Paths.Location = new Point(8, 24);
      this.Paths.Name = "Paths";
      this.Paths.Size = new Size(216, 21);
      this.Paths.TabIndex = 0;
      this.Paths.SelectedIndexChanged += new EventHandler(this.Paths_SelectedIndexChanged);
      this.groupBox1.Controls.Add((Control) this.Proccessing);
      this.groupBox1.Controls.Add((Control) this.Prequal);
      this.groupBox1.Location = new Point(320, 8);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(184, 40);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "File Mode";
      this.Proccessing.Checked = true;
      this.Proccessing.Location = new Point(16, 16);
      this.Proccessing.Name = "Proccessing";
      this.Proccessing.Size = new Size(80, 16);
      this.Proccessing.TabIndex = 0;
      this.Proccessing.TabStop = true;
      this.Proccessing.Text = "Processing";
      this.Proccessing.CheckedChanged += new EventHandler(this.Processing_CheckedChanged);
      this.Prequal.Location = new Point(96, 16);
      this.Prequal.Name = "Prequal";
      this.Prequal.Size = new Size(72, 16);
      this.Prequal.TabIndex = 1;
      this.Prequal.TabStop = true;
      this.Prequal.Text = "Prequal";
      this.Prequal.CheckedChanged += new EventHandler(this.Prequal_CheckedChanged);
      this.groupBox2.Controls.Add((Control) this.folderCombo);
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.groupBox2.Location = new Point(24, 408);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(464, 56);
      this.groupBox2.TabIndex = 10;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Import To:";
      this.folderCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.folderCombo.Location = new Point(104, 24);
      this.folderCombo.Name = "folderCombo";
      this.folderCombo.Size = new Size(168, 21);
      this.folderCombo.TabIndex = 1;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label2.Location = new Point(16, 24);
      this.label2.Name = "label2";
      this.label2.Size = new Size(72, 16);
      this.label2.TabIndex = 0;
      this.label2.Text = "Loan Folder:";
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(416, 472);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 12;
      this.cancelBtn.Text = "&Close";
      this.cancelBtn.Click += new EventHandler(this.CloseBtn_Click);
      this.importBtn.Location = new Point(336, 472);
      this.importBtn.Name = "importBtn";
      this.importBtn.Size = new Size(75, 24);
      this.importBtn.TabIndex = 11;
      this.importBtn.Text = "&Import";
      this.importBtn.Click += new EventHandler(this.importBtn_Click);
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label3.Location = new Point(8, 72);
      this.label3.Name = "label3";
      this.label3.Size = new Size(120, 16);
      this.label3.TabIndex = 3;
      this.label3.Text = "Genesis Login Name :";
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label4.Location = new Point(8, 96);
      this.label4.Name = "label4";
      this.label4.Size = new Size(112, 16);
      this.label4.TabIndex = 5;
      this.label4.Text = "Genesis Password :";
      this.LoginName.CharacterCasing = CharacterCasing.Upper;
      this.LoginName.Location = new Point(128, 72);
      this.LoginName.Name = "LoginName";
      this.LoginName.Size = new Size(160, 20);
      this.LoginName.TabIndex = 4;
      this.LoginName.Text = "";
      this.Password.AcceptsReturn = true;
      this.Password.CharacterCasing = CharacterCasing.Upper;
      this.Password.Location = new Point(128, 96);
      this.Password.Name = "Password";
      this.Password.PasswordChar = '*';
      this.Password.Size = new Size(160, 20);
      this.Password.TabIndex = 6;
      this.Password.Text = "";
      this.Password.KeyPress += new KeyPressEventHandler(this.PasswordEnter);
      this.GetList.Location = new Point(296, 96);
      this.GetList.Name = "GetList";
      this.GetList.Size = new Size(112, 24);
      this.GetList.TabIndex = 7;
      this.GetList.Text = "&Refresh Loan List";
      this.GetList.Click += new EventHandler(this.GetList_Click);
      this.SelectAll.Location = new Point(408, 384);
      this.SelectAll.Name = "SelectAll";
      this.SelectAll.Size = new Size(75, 24);
      this.SelectAll.TabIndex = 9;
      this.SelectAll.Text = "&Select All";
      this.SelectAll.Click += new EventHandler(this.SelectAll_Click);
      this.groupBox3.Controls.Add((Control) this.Archived);
      this.groupBox3.Controls.Add((Control) this.Active);
      this.groupBox3.Location = new Point(320, 48);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(184, 40);
      this.groupBox3.TabIndex = 3;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Status";
      this.Archived.Location = new Point(96, 16);
      this.Archived.Name = "Archived";
      this.Archived.Size = new Size(80, 16);
      this.Archived.TabIndex = 1;
      this.Archived.Text = "Archived";
      this.Archived.CheckedChanged += new EventHandler(this.Archived_CheckedChanged);
      this.Active.Checked = true;
      this.Active.Location = new Point(16, 16);
      this.Active.Name = "Active";
      this.Active.Size = new Size(72, 16);
      this.Active.TabIndex = 0;
      this.Active.TabStop = true;
      this.Active.Text = "Active";
      this.Active.CheckedChanged += new EventHandler(this.Active_CheckedChanged);
      this.TotalFiles.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.TotalFiles.Location = new Point(16, 384);
      this.TotalFiles.Name = "TotalFiles";
      this.TotalFiles.Size = new Size(160, 16);
      this.TotalFiles.TabIndex = 14;
      this.TotalFiles.Text = "Total Loan Files :";
      this.dataFolderBrowse.Location = new Point(232, 24);
      this.dataFolderBrowse.Name = "dataFolderBrowse";
      this.dataFolderBrowse.Size = new Size(72, 23);
      this.dataFolderBrowse.TabIndex = 1;
      this.dataFolderBrowse.Text = "Browse...";
      this.dataFolderBrowse.Click += new EventHandler(this.dataFolderBrowse_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(522, 503);
      this.Controls.Add((Control) this.dataFolderBrowse);
      this.Controls.Add((Control) this.TotalFiles);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.SelectAll);
      this.Controls.Add((Control) this.GetList);
      this.Controls.Add((Control) this.Password);
      this.Controls.Add((Control) this.LoginName);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.importBtn);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.Paths);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.LoanList);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (GenesisImportDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import From Genesis";
      this.Load += new EventHandler(this.GenesisImportDialog_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void GenesisImportDialog_Load(object sender, EventArgs e)
    {
    }

    private void Paths_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.LoanList.Items.Clear();
      if (!this.firstTime && this.LoginName.Text != null && this.LoginName.Text != string.Empty)
        this.GetGenesisLoanList(ClickFrom.PathChange);
      else
        this.firstTime = false;
    }

    private void CloseBtn_Click(object sender, EventArgs e) => this.database.CloseGenesisLoanData();

    private void LoanList_DoubleClick(object sender, EventArgs e)
    {
      this.importBtn_Click(sender, e);
    }

    private void importBtn_Click(object sender, EventArgs e)
    {
      ContactUtil.InitContactUtil();
      if (!this.ValidateInput(ClickFrom.ImportClk))
        return;
      if (this.folderCombo.SelectedItem == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to choose a destination folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.LoanList.SelectedItems.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to select at least one file in order to import.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.mapfile = AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "GEN2ENC.DBF", SystemSettings.LocalAppDir);
        if (!File.Exists(this.mapfile))
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Map file " + this.mapfile + " does not exits.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string name = ((LoanFolderInfo) this.folderCombo.SelectedItem).Name;
          string path = Path.GetTempPath() + "ExpGenFiles";
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
          foreach (FileSystemInfo file in new DirectoryInfo(path).GetFiles())
            file.Delete();
          GenesisImportParameters.FileParameter[] fileParameters = new GenesisImportParameters.FileParameter[this.LoanList.SelectedItems.Count];
          int num4 = 0;
          foreach (ListViewItem selectedItem in this.LoanList.SelectedItems)
            fileParameters[num4++] = new GenesisImportParameters.FileParameter(selectedItem.SubItems[9].Text.ToString(), selectedItem.SubItems[2].Text.ToString(), selectedItem.SubItems[1].Text.ToString());
          this.progressDialog = new ImportProgress(this.LoanList.SelectedItems.Count);
          this.progressDialog.progressLbl.Text = "Genesis Loans";
          GenesisImportParameters parameter = new GenesisImportParameters(this.mapfile, name, fileParameters);
          Thread thread = new Thread(new ParameterizedThreadStart(this.ImportThread));
          thread.Start((object) parameter);
          Session.ISession.LoanImportInProgress = true;
          if (DialogResult.Cancel == this.progressDialog.ShowDialog((IWin32Window) this))
            thread.Abort();
          Session.ISession.LoanImportInProgress = false;
        }
      }
    }

    private void ImportThread(object inputParameter)
    {
      GenesisImportParameters importParameters = inputParameter as GenesisImportParameters;
      long importedCount = 0;
      if (importParameters != null)
      {
        GenesisImport genesisImport = new GenesisImport(Session.SessionObjects, this.loanOfficerId);
        if ((EnableDisableSetting) Session.ServerManager.GetServerSetting("Import.LoanNumbering") == EnableDisableSetting.Enabled)
          genesisImport.UseEMLoanNumbering = true;
        foreach (GenesisImportParameters.FileParameter fileParameter in importParameters.FileParameters)
        {
          this.updateProgress(fileParameter.FirstName + " " + fileParameter.LastName);
          string str1 = Path.GetTempPath() + "ExpGenFiles\\" + fileParameter.FileName + ".XML";
          new FileStream(str1, FileMode.Create, FileAccess.Write).Close();
          try
          {
            string str2 = this.database.ExportFromGenesis(fileParameter.FileName, str1, importParameters.MapFile);
            if (str2 == null || string.Empty == str2)
            {
              LoanDataMgr loanDataMgr = genesisImport.ImportFile(str1);
              if (!genesisImport.LoanExist)
              {
                loanDataMgr.FromLoanImport = true;
                loanDataMgr.Create(importParameters.EmliteFolder, "");
              }
              else
              {
                loanDataMgr.Interactive = false;
                loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
                loanDataMgr.Save();
                loanDataMgr.Unlock();
              }
              loanDataMgr.Close();
              ++importedCount;
            }
            else
              Tracing.Log(GenesisImportDialog.sw, TraceLevel.Error, "ImportGenesis", "Problem exporting loan file " + fileParameter.FileName + "from Genesis.\r\n");
          }
          catch (Exception ex)
          {
            Tracing.Log(GenesisImportDialog.sw, TraceLevel.Error, "ImportGenesis", "Problem importing Genesis file:  " + fileParameter.FileName + ", exception: " + ex.Message + "\r\n");
          }
        }
      }
      this.closeProgress(importedCount);
    }

    private void updateProgress(string fileName)
    {
      if (this.progressDialog.InvokeRequired)
        this.Invoke((Delegate) new GenesisImportDialog.UpdateProgressCallback(this.updateProgress), (object) fileName);
      else
        this.progressDialog.CurrentFile = fileName;
    }

    private void closeProgress(long importedCount)
    {
      if (this.progressDialog.InvokeRequired)
      {
        this.Invoke((Delegate) new GenesisImportDialog.CloseProgressCallback(this.closeProgress), (object) importedCount);
      }
      else
      {
        string text = importedCount.ToString() + " out of " + (object) this.LoanList.SelectedItems.Count + " file(s) were imported.";
        if (importedCount < (long) this.LoanList.SelectedItems.Count)
        {
          if (DialogResult.Yes == Utils.Dialog((IWin32Window) this.progressDialog, text + Environment.NewLine + "Would you like to open the Log File to view the log of files that were not imported?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            SystemUtil.ShellExecute(Tracing.LogFile);
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) this.progressDialog, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        this.progressDialog.Close();
      }
    }

    private bool ValidateInput(ClickFrom Clk)
    {
      string text1 = this.Paths.Text;
      string text2 = string.Empty;
      bool flag = true;
      if (text1 == string.Empty)
      {
        text2 = "Please enter a valid installation path.";
        flag = false;
      }
      else if (!File.Exists(text1 + "\\DEFDIR.DBF"))
      {
        text2 = "Genesis database file " + text1 + "\\DEFDIR.DBF does not exists. Please enter a valid installation path.";
        flag = false;
      }
      else
      {
        string dbfpath = this.database.GetDBFPATH(this.Paths.Text);
        if (!File.Exists(dbfpath + "MASTER.DBF"))
        {
          text2 = "Genesis database file " + dbfpath + "MASTER.DBF does not exists. Please enter a valid installation path.";
          flag = false;
        }
        else if (!File.Exists(dbfpath + "TRAKLOAN.DBF"))
        {
          text2 = "Genesis database file " + dbfpath + "TRAKLOAN.DBF does not exists. Please enter a valid installation path.";
          flag = false;
        }
        else if (!File.Exists(dbfpath + "USERS.DBF"))
        {
          text2 = "Genesis database file " + dbfpath + "USERS.DBF does not exists. Please enter a valid installation path.";
          flag = false;
        }
        else if (this.LoginName.Text == string.Empty)
        {
          text2 = "Please enter a valid Genesis Login Name and Password and click ‘Refresh Loan List'.";
          flag = false;
        }
        else
        {
          if (this.LoginName.Text != string.Empty)
          {
            string str = this.ValidatePassword();
            if (str != null)
            {
              if (Clk != ClickFrom.PathChange)
                text2 = str;
              flag = false;
              goto label_19;
            }
          }
          if (Clk == ClickFrom.ImportClk && this.LoanList.SelectedItems.Count <= 0)
          {
            text2 = "You have to select at least one file in order to import.";
            flag = false;
          }
        }
      }
label_19:
      if (text2 != string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, text2, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return flag;
    }

    private string ValidatePassword()
    {
      string text = this.Paths.Text;
      if (!File.Exists(text + "\\DEFDIR.DBF"))
        return "Genesis database file " + text + "\\DEFDIR.DBF does not exists. Please enter a valid installation path.";
      string dbfpath = this.database.GetDBFPATH(this.Paths.Text);
      return !File.Exists(dbfpath + "USERS.DBF") ? "Genesis database file " + dbfpath + "USERS.DBF does not exists. Please enter a valid installation path." : this.database.ValidateGenesisLogin(this.LoginName.Text, this.Password.Text);
    }

    private void PasswordEnter(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      e.Handled = true;
      this.LoanList.Items.Clear();
      this.GetGenesisLoanList(ClickFrom.Other);
    }

    private void GetList_Click(object sender, EventArgs e)
    {
      this.LoanList.Items.Clear();
      this.GetGenesisLoanList(ClickFrom.Other);
    }

    private void SelectAll_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.LoanList.Items)
        listViewItem.Selected = true;
    }

    private void Active_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.Active.Checked)
        return;
      this.LoanList.Items.Clear();
      if (this.LoginName.Text == null || !(this.LoginName.Text != string.Empty))
        return;
      this.GetGenesisLoanList(ClickFrom.Other);
    }

    private void Archived_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.Archived.Checked)
        return;
      this.LoanList.Items.Clear();
      if (this.LoginName.Text == null || !(this.LoginName.Text != string.Empty))
        return;
      this.GetGenesisLoanList(ClickFrom.Other);
    }

    private void dataFolderBrowse_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.Paths.Text = this.folderBrowserDialog.SelectedPath;
    }

    private delegate void UpdateProgressCallback(string text);

    private delegate void CloseProgressCallback(long importedCount);
  }
}
