// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.ImportPoint
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
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
  public class ImportPoint : Form
  {
    private const string className = "ImportPoint";
    private static readonly string sw = Tracing.SwImportExport;
    private System.ComponentModel.Container components;
    private Label datafolderLbl;
    private GroupBox typeGroupBox;
    private RadioButton prospectRadioBtn;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ComboBox folderCboBox;
    private RadioButton borrowerRadioBtn;
    private GroupBox groupBox2;
    private ComboBox folderCombo;
    private Label label2;
    private Button cancelBtn;
    private Button importBtn;
    private ListView fileListView;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private ColumnHeader columnHeader6;
    private ColumnHeader columnHeader7;
    private Button selectAllBtn;
    private Button dataFolderBrowse;
    private FolderBrowserDialog folderBrowserDialog;
    private ListViewColumnSorter lvwColumnSorter;
    private ArrayList manualIndex = new ArrayList();
    private bool isManualBrowse;
    private Hashtable filesDict = new Hashtable();
    private ImportProgress progressDialog;
    private string loanOfficerId = string.Empty;

    [DllImport("kernel32", EntryPoint = "GetPrivateProfileStringA", CharSet = CharSet.Ansi)]
    private static extern int GetPrivateProfileString(
      string sectionName,
      string keyName,
      string defaultValue,
      StringBuilder returnbuffer,
      int buffersize,
      string filename);

    public ImportPoint(string loanOfficerId)
    {
      this.loanOfficerId = loanOfficerId;
      this.InitializeComponent();
      this.ReadINIfile();
      this.InitializeColumnSorter();
      string[] foldersForAction = ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetLoanFoldersForAction(LoanFolderAction.Import);
      int selectedIndex = -1;
      this.folderCombo.Items.AddRange((object[]) LoanFolderUtil.LoanFolderNames2LoanFolderInfos(foldersForAction, Session.UserInfo.WorkingFolder, out selectedIndex));
      if (selectedIndex >= 0)
      {
        this.folderCombo.SelectedIndex = selectedIndex;
      }
      else
      {
        if (this.folderCombo.Items.Count <= 0)
          return;
        this.folderCombo.SelectedIndex = 0;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.datafolderLbl = new Label();
      this.folderCboBox = new ComboBox();
      this.typeGroupBox = new GroupBox();
      this.borrowerRadioBtn = new RadioButton();
      this.prospectRadioBtn = new RadioButton();
      this.fileListView = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.columnHeader6 = new ColumnHeader();
      this.columnHeader7 = new ColumnHeader();
      this.groupBox2 = new GroupBox();
      this.folderCombo = new ComboBox();
      this.label2 = new Label();
      this.cancelBtn = new Button();
      this.importBtn = new Button();
      this.selectAllBtn = new Button();
      this.dataFolderBrowse = new Button();
      this.folderBrowserDialog = new FolderBrowserDialog();
      this.typeGroupBox.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.datafolderLbl.Location = new Point(8, 8);
      this.datafolderLbl.Name = "datafolderLbl";
      this.datafolderLbl.Size = new Size(96, 16);
      this.datafolderLbl.TabIndex = 0;
      this.datafolderLbl.Text = "Data Folder:";
      this.folderCboBox.Location = new Point(8, 24);
      this.folderCboBox.Name = "folderCboBox";
      this.folderCboBox.Size = new Size(248, 21);
      this.folderCboBox.TabIndex = 1;
      this.folderCboBox.SelectedIndexChanged += new EventHandler(this.folderCboBox_SelectedIndexChanged);
      this.typeGroupBox.Controls.Add((Control) this.borrowerRadioBtn);
      this.typeGroupBox.Controls.Add((Control) this.prospectRadioBtn);
      this.typeGroupBox.Location = new Point(336, 11);
      this.typeGroupBox.Name = "typeGroupBox";
      this.typeGroupBox.Size = new Size(176, 40);
      this.typeGroupBox.TabIndex = 2;
      this.typeGroupBox.TabStop = false;
      this.typeGroupBox.Text = "Client Type";
      this.borrowerRadioBtn.Checked = true;
      this.borrowerRadioBtn.Location = new Point(8, 16);
      this.borrowerRadioBtn.Name = "borrowerRadioBtn";
      this.borrowerRadioBtn.Size = new Size(72, 16);
      this.borrowerRadioBtn.TabIndex = 1;
      this.borrowerRadioBtn.TabStop = true;
      this.borrowerRadioBtn.Text = "Borrower";
      this.prospectRadioBtn.Location = new Point(96, 16);
      this.prospectRadioBtn.Name = "prospectRadioBtn";
      this.prospectRadioBtn.Size = new Size(72, 16);
      this.prospectRadioBtn.TabIndex = 0;
      this.prospectRadioBtn.Text = "Prospect";
      this.prospectRadioBtn.CheckedChanged += new EventHandler(this.prospectRadioBtn_CheckedChanged);
      this.fileListView.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4,
        this.columnHeader5
      });
      this.fileListView.FullRowSelect = true;
      this.fileListView.HideSelection = false;
      this.fileListView.Location = new Point(8, 56);
      this.fileListView.Name = "fileListView";
      this.fileListView.Size = new Size(504, 312);
      this.fileListView.Sorting = SortOrder.Ascending;
      this.fileListView.TabIndex = 5;
      this.fileListView.View = View.Details;
      this.fileListView.ColumnClick += new ColumnClickEventHandler(this.fileListView_ColumnClick);
      this.columnHeader1.Text = "LastName";
      this.columnHeader1.Width = 90;
      this.columnHeader2.Text = "FirstName";
      this.columnHeader2.Width = 90;
      this.columnHeader3.Text = "FileName";
      this.columnHeader3.Width = 117;
      this.columnHeader4.Text = "RepName";
      this.columnHeader4.Width = 90;
      this.columnHeader5.Text = "ContactDate";
      this.columnHeader5.Width = 90;
      this.groupBox2.Controls.Add((Control) this.folderCombo);
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupBox2.Location = new Point(13, 400);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(500, 63);
      this.groupBox2.TabIndex = 6;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Import To:";
      this.folderCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.folderCombo.Location = new Point(100, 28);
      this.folderCombo.Name = "folderCombo";
      this.folderCombo.Size = new Size(307, 21);
      this.folderCombo.TabIndex = 3;
      this.folderCombo.SelectedIndexChanged += new EventHandler(this.folderCombo_SelectedIndexChanged);
      this.label2.Location = new Point(20, 28);
      this.label2.Name = "label2";
      this.label2.Size = new Size(73, 20);
      this.label2.TabIndex = 2;
      this.label2.Text = "Loan Folder:";
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(436, 472);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 8;
      this.cancelBtn.Text = "&Close";
      this.importBtn.Location = new Point(352, 472);
      this.importBtn.Name = "importBtn";
      this.importBtn.Size = new Size(75, 24);
      this.importBtn.TabIndex = 7;
      this.importBtn.Text = "&Import";
      this.importBtn.Click += new EventHandler(this.importBtn_Click);
      this.selectAllBtn.Location = new Point(436, 378);
      this.selectAllBtn.Name = "selectAllBtn";
      this.selectAllBtn.Size = new Size(75, 24);
      this.selectAllBtn.TabIndex = 9;
      this.selectAllBtn.Text = "&Select All";
      this.selectAllBtn.Click += new EventHandler(this.selectAllBtn_Click);
      this.dataFolderBrowse.Location = new Point(257, 24);
      this.dataFolderBrowse.Name = "dataFolderBrowse";
      this.dataFolderBrowse.Size = new Size(72, 23);
      this.dataFolderBrowse.TabIndex = 10;
      this.dataFolderBrowse.Text = "Browse...";
      this.dataFolderBrowse.Click += new EventHandler(this.dataFolderBrowse_Click);
      this.folderBrowserDialog.Description = "Select Point data folder.";
      this.folderBrowserDialog.ShowNewFolderButton = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(522, 503);
      this.Controls.Add((Control) this.dataFolderBrowse);
      this.Controls.Add((Control) this.selectAllBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.importBtn);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.fileListView);
      this.Controls.Add((Control) this.typeGroupBox);
      this.Controls.Add((Control) this.folderCboBox);
      this.Controls.Add((Control) this.datafolderLbl);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportPoint);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import From Calyx Point";
      this.typeGroupBox.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void ReadINIfile()
    {
      string environmentVariable = Environment.GetEnvironmentVariable("winDir");
      string str = environmentVariable + "\\winpoint.ini";
      StringBuilder returnbuffer = new StringBuilder(256);
      int num1 = 0;
      if (File.Exists(str))
      {
        while (true)
        {
          ImportPoint.GetPrivateProfileString("Directories", "Folder" + num1.ToString(), "SENTINEL", returnbuffer, 256, str);
          if (!(returnbuffer.ToString() == "SENTINEL"))
          {
            this.folderCboBox.Items.Add((object) returnbuffer.ToString());
            ++num1;
          }
          else
            break;
        }
        if (this.folderCboBox.Items.Count > 0)
        {
          this.folderCboBox.SelectedIndex = 0;
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Encompass can't get necessary information from the Point configuration file 'winpoint.ini'. Please make sure you have Point installed or use 'Browse' button to locate Point loan files.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          Tracing.Log(ImportPoint.sw, TraceLevel.Error, nameof (ImportPoint), "There is no any folder information in '" + str + "' file.");
        }
      }
      else
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "The Point configuration file 'winpoint.ini' was not found in '" + environmentVariable + "' directory. Please make sure you have Point installed or use 'Browse' button to locate Point loan files.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        Tracing.Log(ImportPoint.sw, TraceLevel.Error, nameof (ImportPoint), "The '" + str + "' file is not found.");
      }
    }

    private void importBtn_Click(object sender, EventArgs e)
    {
      ContactUtil.InitContactUtil();
      if (this.folderCombo.SelectedItem == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to choose a destination folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.fileListView.SelectedItems.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to select at least one file in order to import.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string str = this.prospectRadioBtn.Checked ? "PROSPECT" : "BORROWER";
        string pointFolder = this.isManualBrowse ? this.folderCboBox.SelectedItem.ToString() + "\\" : this.folderCboBox.SelectedItem.ToString() + "\\" + str + "\\";
        string name = ((LoanFolderInfo) this.folderCombo.SelectedItem).Name;
        string[] fileNames = new string[this.fileListView.SelectedItems.Count];
        int num3 = 0;
        foreach (ListViewItem selectedItem in this.fileListView.SelectedItems)
          fileNames[num3++] = selectedItem.SubItems[2].Text.ToString();
        this.progressDialog = new ImportProgress(this.fileListView.SelectedItems.Count);
        this.progressDialog.progressLbl.Text = "Calyx Point";
        ImportPoint.PointImportParameters parameter = new ImportPoint.PointImportParameters(pointFolder, name, fileNames);
        Thread thread = new Thread(new ParameterizedThreadStart(this.ImportThread));
        thread.Start((object) parameter);
        Session.ISession.LoanImportInProgress = true;
        if (DialogResult.Cancel == this.progressDialog.ShowDialog((IWin32Window) this))
          thread.Abort();
        Session.ISession.LoanImportInProgress = false;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void ImportThread(object inputParameter)
    {
      ImportPoint.PointImportParameters importParameters = inputParameter as ImportPoint.PointImportParameters;
      long importedCount = 0;
      if (importParameters != null)
      {
        PointImport pointImport = new PointImport(Session.SessionObjects, this.loanOfficerId);
        if (EnableDisableSetting.Enabled == (EnableDisableSetting) Session.ServerManager.GetServerSetting("Import.LoanNumbering"))
          pointImport.UseEMLoanNumbering = true;
        foreach (string fileName in importParameters.FileNames)
        {
          this.updateProgress(fileName);
          try
          {
            LoanDataMgr loanDataMgr = pointImport.ImportFile(importParameters.PointFolder + fileName);
            loanDataMgr.FromLoanImport = true;
            loanDataMgr.Create(importParameters.EmliteFolder, "");
            loanDataMgr.Close();
            ++importedCount;
          }
          catch (Exception ex)
          {
            Tracing.Log(ImportPoint.sw, TraceLevel.Error, nameof (ImportPoint), "Problem importing Point file:  " + fileName + ", exception: " + (object) ex + "\r\n");
          }
        }
      }
      this.closeProgress(importedCount);
    }

    private void updateProgress(string fileName)
    {
      if (this.progressDialog.InvokeRequired)
        this.Invoke((Delegate) new ImportPoint.UpdateProgressCallback(this.updateProgress), (object) fileName);
      else
        this.progressDialog.CurrentFile = fileName;
    }

    private void closeProgress(long importedCount)
    {
      if (this.progressDialog.InvokeRequired)
      {
        this.Invoke((Delegate) new ImportPoint.CloseProgressCallback(this.closeProgress), (object) importedCount);
      }
      else
      {
        string text = importedCount.ToString() + " out of " + (object) this.fileListView.SelectedItems.Count + " file(s) were imported.";
        if (importedCount < (long) this.fileListView.SelectedItems.Count)
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

    private void InitializeColumnSorter()
    {
      this.lvwColumnSorter = new ListViewColumnSorter();
      this.fileListView.ListViewItemSorter = (IComparer) this.lvwColumnSorter;
      this.lvwColumnSorter.SortColumn = 0;
      this.lvwColumnSorter.SortType = 0;
      this.lvwColumnSorter.Order = SortOrder.Ascending;
    }

    private void prospectRadioBtn_CheckedChanged(object sender, EventArgs e)
    {
      if (this.isManualBrowse)
        this.BuildManualPointFileList();
      else
        this.BuildPointFileList();
    }

    private void folderCboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      int selectedIndex = this.folderCboBox.SelectedIndex;
      if (selectedIndex < 0)
        return;
      if (this.manualIndex.Contains((object) selectedIndex))
      {
        this.isManualBrowse = true;
        this.BuildManualPointFileList();
      }
      else
      {
        this.isManualBrowse = false;
        this.BuildPointFileList();
      }
    }

    private void folderCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private bool BuildPointFileList()
    {
      this.fileListView.Items.Clear();
      string str1 = "BORROWER";
      if (this.prospectRadioBtn.Checked)
        str1 = "PROSPECT";
      if (this.folderCboBox.SelectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please setup data folder first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      string path = (string) this.folderCboBox.SelectedItem + "\\" + str1 + ".IDX";
      if (File.Exists(path))
      {
        this.Cursor = Cursors.WaitCursor;
        FileStream fileStream = (FileStream) null;
        try
        {
          fileStream = File.OpenRead(path);
        }
        catch (Exception ex)
        {
          Tracing.Log(ImportPoint.sw, TraceLevel.Error, nameof (ImportPoint), "File open failed: " + path + ", exception: " + ex.Message);
        }
        int length = (int) fileStream.Length;
        byte[] buffer = new byte[length];
        fileStream.Read(buffer, 0, length);
        fileStream.Close();
        BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(buffer));
        Encoding encoding = Encoding.GetEncoding(1252);
        ArrayList arrayList = new ArrayList();
        string str2;
        do
        {
          try
          {
            str2 = encoding.GetString(binaryReader.ReadBytes(87));
            arrayList.Clear();
            if (str2 != string.Empty && str2 != null)
            {
              arrayList.Add((object) str2.Substring(0, 20));
              arrayList.Add((object) str2.Substring(20, 20));
              arrayList.Add((object) str2.Substring(40, 13));
              arrayList.Add((object) str2.Substring(53, 25));
              arrayList.Add((object) str2.Substring(78, 9));
            }
            if (arrayList.Count > 0)
            {
              ListViewItem listViewItem = new ListViewItem(arrayList[0].ToString().Split(new char[1])[0].ToString());
              for (int index = 1; index < arrayList.Count; ++index)
              {
                string str3 = arrayList[index].ToString().Split(new char[1])[0].ToString();
                if (str3.IndexOf(".BRW") > 0)
                {
                  str3 = this.folderCboBox.Text + "\\" + str1 + "\\" + str3;
                  string[] files = Directory.GetFiles(Path.GetDirectoryName(str3), Path.GetFileName(str3));
                  if (files.Length != 0)
                  {
                    string str4 = files[0];
                    str3 = str4.Substring(str4.LastIndexOf("\\") + 1);
                  }
                }
                listViewItem.SubItems.Add(str3);
              }
              this.fileListView.Items.Add(listViewItem);
            }
          }
          catch (EndOfStreamException ex)
          {
            binaryReader.Close();
            break;
          }
        }
        while (str2 != string.Empty);
        this.Cursor = Cursors.Default;
        return true;
      }
      return this.buildPointIndexList();
    }

    private void fileListView_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      this.lvwColumnSorter.SortColumn = e.Column;
      this.lvwColumnSorter.SortType = 0;
      this.lvwColumnSorter.Order = this.lvwColumnSorter.Order != SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending;
      this.fileListView.Sort();
    }

    private void selectAllBtn_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.fileListView.Items)
        listViewItem.Selected = true;
    }

    private void dataFolderBrowse_Click(object sender, EventArgs e)
    {
      if (this.folderBrowserDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      string selectedPath = this.folderBrowserDialog.SelectedPath;
      if (this.folderCboBox.Items.Contains((object) selectedPath))
      {
        this.folderCboBox.SelectedItem = (object) selectedPath;
      }
      else
      {
        int num = this.folderCboBox.Items.Add((object) selectedPath);
        this.manualIndex.Add((object) num);
        this.folderCboBox.SelectedIndexChanged -= new EventHandler(this.folderCboBox_SelectedIndexChanged);
        this.folderCboBox.SelectedIndex = num;
        if (!this.BuildPointFileList())
          this.BuildManualPointFileList();
        this.folderCboBox.SelectedIndexChanged += new EventHandler(this.folderCboBox_SelectedIndexChanged);
      }
    }

    private void BuildManualPointFileList()
    {
      if (this.BuildPointFileList())
      {
        this.isManualBrowse = false;
      }
      else
      {
        if (this.folderCboBox.Text == "")
          return;
        string path = this.folderCboBox.SelectedItem.ToString();
        this.Cursor = Cursors.WaitCursor;
        this.fileListView.Items.Clear();
        for (int index = 1; index <= 2; ++index)
        {
          string searchPattern = index != 1 ? "*.PRS" : "*.BRW";
          foreach (object file in Directory.GetFiles(path, searchPattern))
          {
            string str1 = file.ToString();
            string str2 = str1.Substring(str1.LastIndexOf("\\") + 1);
            this.fileListView.Items.Add(new ListViewItem("")
            {
              SubItems = {
                "",
                str2
              }
            });
          }
        }
        this.Cursor = Cursors.Default;
        this.isManualBrowse = true;
      }
    }

    private bool buildPointIndexList()
    {
      string str1 = this.folderCboBox.SelectedItem.ToString() + "\\folder.ini";
      if (!File.Exists(str1))
        return false;
      try
      {
        StringBuilder returnbuffer = new StringBuilder(256);
        ImportPoint.GetPrivateProfileString("Folder", "IndexVersion", "", returnbuffer, 256, str1);
        if (returnbuffer.ToString() != "2")
          return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(ImportPoint.sw, TraceLevel.Error, nameof (ImportPoint), "Can't access Folder.ini file in " + this.folderCboBox.SelectedItem.ToString() + ", exception: " + ex.Message + "\r\n");
        return false;
      }
      string str2 = this.folderCboBox.SelectedItem.ToString();
      if (!str2.EndsWith("\\"))
        str2 += "\\";
      string searchPattern = "*.BRW";
      string path;
      if (this.prospectRadioBtn.Checked)
      {
        path = str2 + "PROSPECT";
        searchPattern = "*.PRS";
      }
      else
        path = str2 + "BORROWER";
      this.fileListView.Items.Clear();
      this.fileListView.BeginUpdate();
      new ProgressDialog("Build Point File List", new AsynchronousProcess(this.buildListViewFromPoint), (object) Directory.GetFiles(path, searchPattern), true).RunWait((IWin32Window) this);
      this.fileListView.EndUpdate();
      return true;
    }

    private DialogResult buildListViewFromPoint(object state, IProgressFeedback feedback)
    {
      string[] strArray1 = (string[]) state;
      feedback.Status = "Retrieving Loan Information...";
      feedback.ResetCounter(strArray1.Length);
      ListViewItem listViewItem = (ListViewItem) null;
      for (int index1 = 0; index1 < strArray1.Length; ++index1)
      {
        Hashtable hashtable = this.loadPointFile(strArray1[index1]);
        string str = strArray1[index1];
        int num = str.LastIndexOf("\\");
        if (num > -1)
          str = str.Substring(num + 1);
        feedback.Details = "Retrieving '" + str + "'...";
        feedback.Increment(1);
        if (hashtable != null)
        {
          string[] strArray2 = new string[5]
          {
            "2",
            "100",
            "FileName",
            "4",
            "6200"
          };
          for (int index2 = 0; index2 < strArray2.Length; ++index2)
          {
            string text = "";
            if (hashtable.ContainsKey((object) strArray2[index2]))
              text = (string) hashtable[(object) strArray2[index2]];
            if (index2 == 2)
              text = str;
            if (index2 == 0)
              listViewItem = new ListViewItem(text);
            else
              listViewItem.SubItems.Add(text);
          }
          this.fileListView.Items.Add(listViewItem);
        }
      }
      return DialogResult.OK;
    }

    private Hashtable loadPointFile(string pointFile)
    {
      Hashtable hashtable = new Hashtable();
      FileStream fileStream;
      try
      {
        fileStream = File.Open(pointFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      }
      catch (Exception ex)
      {
        Tracing.Log(ImportPoint.sw, TraceLevel.Error, nameof (ImportPoint), "File open failed: " + pointFile + ", exception: " + ex.Message);
        return (Hashtable) null;
      }
      Encoding encoding = Encoding.GetEncoding(1252);
      int length = (int) fileStream.Length;
      byte[] buffer = new byte[length];
      fileStream.Read(buffer, 0, length);
      fileStream.Close();
      BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(buffer));
      hashtable.Clear();
      string key = (string) null;
      string str = (string) null;
      while (true)
      {
        try
        {
          key = ((ushort) binaryReader.ReadInt16()).ToString();
          int count = (int) binaryReader.ReadByte();
          if (count == (int) byte.MaxValue)
            count = (int) binaryReader.ReadInt16();
          str = encoding.GetString(binaryReader.ReadBytes(count));
          hashtable[(object) key] = (object) str.Trim();
        }
        catch (EndOfStreamException ex)
        {
          binaryReader.Close();
          break;
        }
        catch (ArgumentException ex)
        {
          Tracing.Log(ImportPoint.sw, TraceLevel.Error, nameof (ImportPoint), "duplicated id/val: Point id, value (" + key + ", " + str + "); exception: " + ex.Message);
        }
        catch (Exception ex)
        {
          Tracing.Log(ImportPoint.sw, TraceLevel.Error, nameof (ImportPoint), "mapping exception: Point id, value (" + key + ", " + str + "); exception: " + ex.Message);
        }
      }
      return hashtable;
    }

    private delegate void UpdateProgressCallback(string text);

    private delegate void CloseProgressCallback(long importedCount);

    public class PointImportParameters
    {
      private string pointFolder = string.Empty;
      private string emliteFolder = string.Empty;
      private string[] fileNames;

      public string PointFolder => this.pointFolder;

      public string EmliteFolder => this.emliteFolder;

      public string[] FileNames => this.fileNames;

      public PointImportParameters(string pointFolder, string emliteFolder, string[] fileNames)
      {
        this.pointFolder = pointFolder;
        this.emliteFolder = emliteFolder;
        this.fileNames = fileNames;
      }
    }
  }
}
