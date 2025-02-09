// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.ImportOtherCustomForms
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.CustomLetters;
using EllieMae.EMLite.RemotingServices;
using Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class ImportOtherCustomForms : Form
  {
    private const string className = "ImportOtherCustomForms";
    private static readonly string sw = Tracing.SwImportExport;
    private System.ComponentModel.Container components;
    private Button importBtn;
    private Button cancelBtn;
    private GroupBox sourceGrpBox;
    private GroupBox destinationGrpBox;
    private Button selectAllBtn;
    private CheckBox keepBox;
    private DriveListBox driveListBox;
    private DirListBox dirListBox;
    private FileListBox fileListBox;
    public FSExplorer sourceIFS;
    private string folder = string.Empty;
    private ImportProgress progressForm;

    public ImportOtherCustomForms()
    {
      this.InitializeComponent();
      this.SetSourceFolder();
      this.SetDestinationFolder();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.driveListBox = new DriveListBox();
      this.importBtn = new Button();
      this.cancelBtn = new Button();
      this.dirListBox = new DirListBox();
      this.fileListBox = new FileListBox();
      this.selectAllBtn = new Button();
      this.sourceGrpBox = new GroupBox();
      this.destinationGrpBox = new GroupBox();
      this.sourceIFS = new FSExplorer();
      this.keepBox = new CheckBox();
      this.sourceGrpBox.SuspendLayout();
      this.destinationGrpBox.SuspendLayout();
      this.SuspendLayout();
      this.driveListBox.Location = new Point(8, 24);
      this.driveListBox.Name = "driveListBox";
      this.driveListBox.Size = new Size(168, 21);
      this.driveListBox.TabIndex = 7;
      this.driveListBox.SelectedIndexChanged += new EventHandler(this.driveListBox_SelectedIndexChanged);
      this.importBtn.DialogResult = DialogResult.OK;
      this.importBtn.Location = new Point(592, 456);
      this.importBtn.Name = "importBtn";
      this.importBtn.Size = new Size(75, 24);
      this.importBtn.TabIndex = 3;
      this.importBtn.Text = "&Import";
      this.importBtn.Click += new EventHandler(this.importBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(672, 456);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 4;
      this.cancelBtn.Text = "&Close";
      this.dirListBox.IntegralHeight = false;
      this.dirListBox.Location = new Point(8, 56);
      this.dirListBox.Name = "dirListBox";
      this.dirListBox.Size = new Size(168, 336);
      this.dirListBox.TabIndex = 6;
      this.dirListBox.Change += new EventHandler(this.dirListBox_Change);
      this.fileListBox.Location = new Point(184, 24);
      this.fileListBox.Name = "fileListBox";
      this.fileListBox.Pattern = "*.DOC;*.RTF";
      this.fileListBox.SelectionMode = SelectionMode.MultiExtended;
      this.fileListBox.Size = new Size(232, 368);
      this.fileListBox.TabIndex = 8;
      this.selectAllBtn.Location = new Point(336, 400);
      this.selectAllBtn.Name = "selectAllBtn";
      this.selectAllBtn.Size = new Size(75, 24);
      this.selectAllBtn.TabIndex = 11;
      this.selectAllBtn.Text = "&Select All";
      this.selectAllBtn.Click += new EventHandler(this.selectAllBtn_Click);
      this.sourceGrpBox.Controls.Add((Control) this.fileListBox);
      this.sourceGrpBox.Controls.Add((Control) this.dirListBox);
      this.sourceGrpBox.Controls.Add((Control) this.selectAllBtn);
      this.sourceGrpBox.Controls.Add((Control) this.driveListBox);
      this.sourceGrpBox.Location = new Point(8, 16);
      this.sourceGrpBox.Name = "sourceGrpBox";
      this.sourceGrpBox.Size = new Size(424, 432);
      this.sourceGrpBox.TabIndex = 12;
      this.sourceGrpBox.TabStop = false;
      this.sourceGrpBox.Text = "Source:";
      this.destinationGrpBox.Controls.Add((Control) this.sourceIFS);
      this.destinationGrpBox.Location = new Point(440, 16);
      this.destinationGrpBox.Name = "destinationGrpBox";
      this.destinationGrpBox.Size = new Size(312, 432);
      this.destinationGrpBox.TabIndex = 13;
      this.destinationGrpBox.TabStop = false;
      this.destinationGrpBox.Text = "Destination:";
      this.sourceIFS.Location = new Point(16, 24);
      this.sourceIFS.Name = "sourceIFS";
      this.sourceIFS.Size = new Size(288, 368);
      this.sourceIFS.TabIndex = 28;
      this.keepBox.Checked = true;
      this.keepBox.CheckState = CheckState.Checked;
      this.keepBox.Location = new Point(16, 448);
      this.keepBox.Name = "keepBox";
      this.keepBox.Size = new Size(416, 32);
      this.keepBox.TabIndex = 15;
      this.keepBox.Text = "Keep data fields that exist in the custom forms you are importing. Please note that data fields from other applications are not compatible with Encompass.";
      this.keepBox.TextAlign = ContentAlignment.BottomLeft;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(762, 487);
      this.Controls.Add((Control) this.destinationGrpBox);
      this.Controls.Add((Control) this.sourceGrpBox);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.importBtn);
      this.Controls.Add((Control) this.keepBox);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportOtherCustomForms);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import Other Custom Forms";
      this.sourceGrpBox.ResumeLayout(false);
      this.destinationGrpBox.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void driveListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.dirListBox.Path = this.driveListBox.Drive;
      }
      catch (IOException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Device " + this.driveListBox.Drive + " unavailable.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        string path = this.dirListBox.Path;
        this.driveListBox.Drive = this.dirListBox.Path.Substring(0, this.dirListBox.Path.IndexOf("\\"));
        this.dirListBox.Path = path;
      }
    }

    private void dirListBox_Change(object sender, EventArgs e)
    {
      this.fileListBox.Path = this.dirListBox.Path;
    }

    private void selectAllBtn_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.fileListBox.Items.Count; ++index)
        this.fileListBox.SetSelected(index, true);
    }

    private void importBtn_Click(object sender, EventArgs e)
    {
      if (this.sourceIFS.CurrentFolder == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to choose a destination folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.fileListBox.SelectedItems.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to select at least one file in order to import.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string path = this.dirListBox.Path;
        FileSystemEntry currentFolder = this.sourceIFS.CurrentFolder;
        bool keepDataFields = this.keepBox.Checked;
        string[] fileNames = new string[this.fileListBox.SelectedItems.Count];
        int num3 = 0;
        foreach (object selectedItem in this.fileListBox.SelectedItems)
          fileNames[num3++] = selectedItem.ToString();
        this.progressForm = new ImportProgress(this.fileListBox.SelectedItems.Count);
        this.progressForm.progressLbl.Text = "Other Custom Forms";
        ImportOtherCustomForms.CustomImportParameters parameter = new ImportOtherCustomForms.CustomImportParameters(path, currentFolder, keepDataFields, fileNames);
        Thread thread = new Thread(new ParameterizedThreadStart(this.ImportThread));
        thread.Start((object) parameter);
        if (DialogResult.Cancel != this.progressForm.ShowDialog((IWin32Window) this))
          return;
        thread.Abort();
      }
    }

    private void ImportThread(object inputParameter)
    {
      ImportOtherCustomForms.CustomImportParameters importParameters = inputParameter as ImportOtherCustomForms.CustomImportParameters;
      long importedCount = 0;
      if (importParameters != null)
      {
        foreach (string fileName in importParameters.FileNames)
        {
          if (this.FileExists(importParameters.CustomFormsFolder + "\\" + fileName))
          {
            this.updateProgress(importParameters.CustomFormsFolder + "\\" + fileName);
            try
            {
              if (new CustomLetterIFSExplorer(Session.DefaultInstance, Session.MainScreen).ImportForm(importParameters.EmliteFolderEntry, importParameters.CustomFormsFolder, fileName, importParameters.KeepDataFields, true))
                ++importedCount;
            }
            catch (Exception ex)
            {
              Tracing.Log(ImportOtherCustomForms.sw, TraceLevel.Error, nameof (ImportOtherCustomForms), "Problem importing file '" + importParameters.CustomFormsFolder + "\\" + fileName + "'. Exception: " + ex.Message + "\r\n");
            }
          }
        }
      }
      this.closeProgress(importedCount);
    }

    private void updateProgress(string fileName)
    {
      if (this.progressForm.InvokeRequired)
        this.Invoke((Delegate) new ImportOtherCustomForms.UpdateProgressCallback(this.updateProgress), (object) fileName);
      else
        this.progressForm.CurrentFile = fileName;
    }

    private void closeProgress(long importedCount)
    {
      if (this.progressForm.InvokeRequired)
      {
        this.Invoke((Delegate) new ImportOtherCustomForms.CloseProgressCallback(this.closeProgress), (object) importedCount);
      }
      else
      {
        string text = importedCount.ToString() + " out of " + (object) this.fileListBox.SelectedItems.Count + " file(s) were imported.";
        if (importedCount < (long) this.fileListBox.SelectedItems.Count)
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

    private bool FileExists(string filename)
    {
      if (File.Exists(filename))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "File '" + filename + "' does not exists. We will continue importing next selected file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void SetSourceFolder()
    {
      this.driveListBox.Drive = SystemSettings.LocalAppDir;
      this.dirListBox.Path = SystemSettings.LocalAppDir;
      this.fileListBox.Path = SystemSettings.LocalAppDir;
    }

    private bool HasPersonalSetting(AclGroup[] groupList, AclFileType fileType)
    {
      bool flag = false;
      foreach (AclGroup group in groupList)
      {
        FileInGroup[] aclGroupFileRefs = Session.AclGroupManager.GetAclGroupFileRefs(group.ID, fileType);
        if (aclGroupFileRefs != null && aclGroupFileRefs.Length != 0)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private void SetDestinationFolder()
    {
      string privateProfileString = Session.GetPrivateProfileString("CustomLetterPanel", "LastFolderViewed");
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      bool publicOnly = true;
      if (aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Personal_CustomPrintForms))
        publicOnly = false;
      this.sourceIFS.HasPublicRight = this.HasPersonalSetting(Session.AclGroupManager.GetGroupsOfUser(Session.UserID), AclFileType.CustomPrintForms);
      this.sourceIFS.SetProperties(true, false, false, false);
      this.sourceIFS.FileType = FSExplorer.FileTypes.CustomForms;
      this.sourceIFS.DisplayFolderOperationButtonsOnly = true;
      this.sourceIFS.HideDescription = true;
      PointImportTemplatesIFSExplorer ifsExplorer = new PointImportTemplatesIFSExplorer("CF");
      FileSystemEntry fileSystemEntry = (FileSystemEntry) null;
      try
      {
        fileSystemEntry = FileSystemEntry.Parse(privateProfileString);
        if (!ifsExplorer.EntryExists(fileSystemEntry))
          fileSystemEntry = (FileSystemEntry) null;
      }
      catch
      {
      }
      if (fileSystemEntry == null)
        fileSystemEntry = !this.sourceIFS.HasPublicRight ? FileSystemEntry.PrivateRoot(Session.UserID) : FileSystemEntry.PublicRoot;
      this.sourceIFS.FolderChanged += new EventHandler(this.formifsExplorer_FolderChanged);
      this.sourceIFS.Init((IFSExplorerBase) ifsExplorer, fileSystemEntry, publicOnly);
    }

    private void formifsExplorer_FolderChanged(object sender, EventArgs e)
    {
      FSExplorer fsExplorer = (FSExplorer) sender;
      if (fsExplorer.CurrentFolder != null)
      {
        if (!fsExplorer.CurrentFolder.IsPublic)
          this.importBtn.Enabled = true;
        else if (fsExplorer.CurrentFolder.Access == AclResourceAccess.ReadWrite)
          this.importBtn.Enabled = true;
        else
          this.importBtn.Enabled = false;
      }
      else
        this.importBtn.Enabled = false;
    }

    private delegate void UpdateProgressCallback(string text);

    private delegate void CloseProgressCallback(long importedCount);

    public class CustomImportParameters
    {
      private string customFormsFolder = string.Empty;
      private FileSystemEntry emliteFolderEntry;
      private bool keepDataFields;
      private string[] fileNames;

      public string CustomFormsFolder => this.customFormsFolder;

      public FileSystemEntry EmliteFolderEntry => this.emliteFolderEntry;

      public bool KeepDataFields => this.keepDataFields;

      public string[] FileNames => this.fileNames;

      public CustomImportParameters(
        string customFormsFolder,
        FileSystemEntry emliteFolderEntry,
        bool keepDataFields,
        string[] fileNames)
      {
        this.customFormsFolder = customFormsFolder;
        this.emliteFolderEntry = emliteFolderEntry;
        this.keepDataFields = keepDataFields;
        this.fileNames = fileNames;
      }
    }
  }
}
