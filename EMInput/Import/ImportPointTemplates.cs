// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.ImportPointTemplates
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
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
  public class ImportPointTemplates : Form
  {
    private const string className = "ImportPointTemplates";
    private static readonly string sw = Tracing.SwImportExport;
    private System.ComponentModel.Container components;
    private Button importBtn;
    private Button cancelBtn;
    private Button selectAllBtn;
    private GroupBox sourceGrpBox;
    private GroupBox destinationGrpBox;
    private DriveListBox driveListBox;
    private DirListBox dirListBox;
    private FileListBox fileListBox;
    public FSExplorer sourceIFS;
    private ImportProgress progressDialog;
    private string type = string.Empty;
    private string folder = string.Empty;

    public ImportPointTemplates(string type)
    {
      this.InitializeComponent();
      this.type = type;
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
      this.sourceGrpBox.SuspendLayout();
      this.destinationGrpBox.SuspendLayout();
      this.SuspendLayout();
      this.driveListBox.Location = new Point(8, 24);
      this.driveListBox.Name = "driveListBox";
      this.driveListBox.Size = new Size(168, 21);
      this.driveListBox.TabIndex = 7;
      this.driveListBox.SelectedIndexChanged += new EventHandler(this.driveListBox_SelectedIndexChanged);
      this.importBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.importBtn.DialogResult = DialogResult.OK;
      this.importBtn.Location = new Point(600, 456);
      this.importBtn.Name = "importBtn";
      this.importBtn.Size = new Size(75, 24);
      this.importBtn.TabIndex = 3;
      this.importBtn.Text = "&Import";
      this.importBtn.Click += new EventHandler(this.importBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(680, 456);
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
      this.fileListBox.Pattern = "*.*";
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
      this.sourceIFS.HasPublicRight = true;
      this.sourceIFS.Location = new Point(16, 24);
      this.sourceIFS.Name = "sourceIFS";
      this.sourceIFS.setContactType = ContactType.BizPartner;
      this.sourceIFS.Size = new Size(288, 368);
      this.sourceIFS.TabIndex = 28;
      this.sourceIFS.FolderChanged += new EventHandler(this.sourceIFS_FolderChanged);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(762, 487);
      this.Controls.Add((Control) this.destinationGrpBox);
      this.Controls.Add((Control) this.sourceGrpBox);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.importBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportPointTemplates);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import Point Template Files";
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
      else if (0 >= this.fileListBox.SelectedItems.Count)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to select a file in order to import.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.None;
      }
      else
      {
        string path1 = this.dirListBox.Path;
        string path2 = this.sourceIFS.CurrentFolder.Path;
        bool isRootPublic = this.sourceIFS.IsRootPublic;
        string[] fileNames = new string[this.fileListBox.SelectedItems.Count];
        int num3 = 0;
        foreach (object selectedItem in this.fileListBox.SelectedItems)
          fileNames[num3++] = selectedItem.ToString();
        this.progressDialog = new ImportProgress(fileNames.Length);
        this.progressDialog.progressLbl.Text = "Point Templates";
        ImportPointTemplates.PointTemplateImportParameters parameter = new ImportPointTemplates.PointTemplateImportParameters(this.type, path1, path2, isRootPublic, fileNames);
        Thread thread = new Thread(new ParameterizedThreadStart(this.ImportThread));
        thread.Start((object) parameter);
        if (DialogResult.Cancel != this.progressDialog.ShowDialog((IWin32Window) this))
          return;
        thread.Abort();
      }
    }

    private void ImportThread(object inputParameter)
    {
      ImportPointTemplates.PointTemplateImportParameters importParameters = inputParameter as ImportPointTemplates.PointTemplateImportParameters;
      long importedCount = 0;
      if (importParameters != null)
      {
        WordHandler wordHandler = (WordHandler) null;
        if ("CF" == importParameters.TemplateType)
        {
          wordHandler = new WordHandler();
          wordHandler.CreateNewDoc();
          wordHandler.SetWordAppVisibility(false);
        }
        ReportLog.ClearAllLogs();
        foreach (string fileName in importParameters.FileNames)
        {
          string str = importParameters.PointTemplateFolder + "\\" + fileName;
          this.updateProgress(str);
          try
          {
            if (this.FileExists(str))
            {
              int num = 1;
              switch (importParameters.TemplateType)
              {
                case "LP":
                  num = new PointLPImport().ImportFile((IWin32Window) this, str, importParameters.EmliteFolder, importParameters.IsDestinationPublic);
                  break;
                case "CC":
                  num = new PointCCImport().ImportFile((IWin32Window) this, str, importParameters.EmliteFolder, importParameters.IsDestinationPublic);
                  break;
                case "CF":
                  num = new PointCustomFormsImport().Import(str, importParameters.EmliteFolder, importParameters.IsDestinationPublic);
                  break;
              }
              if (num == 1)
                ++importedCount;
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(ImportPointTemplates.sw, TraceLevel.Error, nameof (ImportPointTemplates), "Problem importing Template file:  " + str + ", exception: " + ex.Message + "\r\n");
          }
        }
        wordHandler?.ShutDown(false);
      }
      this.closeProgress(importedCount);
    }

    private void updateProgress(string fileName)
    {
      if (this.progressDialog.InvokeRequired)
        this.Invoke((Delegate) new ImportPointTemplates.UpdateProgressCallback(this.updateProgress), (object) fileName);
      else
        this.progressDialog.CurrentFile = fileName;
    }

    private void closeProgress(long importedCount)
    {
      if (this.progressDialog.InvokeRequired)
      {
        this.Invoke((Delegate) new ImportPointTemplates.CloseProgressCallback(this.closeProgress), (object) importedCount);
      }
      else
      {
        string str = "CC" == this.type ? " closing costs " : ("CF" == this.type ? " custom forms " : " loan programs ");
        string text1 = importedCount.ToString() + " out of " + (object) this.fileListBox.SelectedItems.Count + str + "were imported.";
        if ("CF" == this.type && ReportLog.HasLog() || "CF" != this.type && importedCount < (long) this.fileListBox.SelectedItems.Count)
        {
          string text2;
          if ("CF" == this.type)
            text2 = text1 + Environment.NewLine + "Would you like to open the Log File to view details about the fields that were not imported?";
          else
            text2 = text1 + Environment.NewLine + "Would you like to open the Log File to view details about the" + str + "that were not imported?";
          if (DialogResult.Yes == Utils.Dialog((IWin32Window) this, text2, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
          {
            if ("CF" == this.type)
              this.showCustomFormImportReport();
            else
              SystemUtil.ShellExecute(Tracing.LogFile);
          }
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) this, text1, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        this.progressDialog.Close();
      }
    }

    private void showCustomFormImportReport()
    {
      string[] allLogs = ReportLog.GetAllLogs();
      CustomFormImportDialog formImportDialog = new CustomFormImportDialog();
      formImportDialog.SetViewContent(allLogs);
      int num = (int) formImportDialog.ShowDialog((IWin32Window) this);
    }

    private bool FileExists(string filename)
    {
      if (File.Exists(filename))
        return true;
      Tracing.Log(ImportPointTemplates.sw, TraceLevel.Error, nameof (ImportPointTemplates), "File '" + filename + "' does not exists.\r\n");
      return false;
    }

    private void SetSourceFolder()
    {
      string installationPath = new ImportPointSettings("").GetPointInstallationPath("Templates");
      if (installationPath == string.Empty)
        return;
      string str = installationPath.Substring(0, installationPath.IndexOf(":") + 1);
      this.driveListBox.Drive = !(str != "") ? installationPath : str;
      switch (this.type)
      {
        case "LP":
          installationPath += "LOANPROG";
          this.fileListBox.Pattern = "*.*";
          break;
        case "CC":
          installationPath += "CCS";
          this.fileListBox.Pattern = "*.*";
          break;
        case "CF":
          installationPath += "CUSTOMDOC";
          this.fileListBox.Pattern = "*.PCD";
          break;
      }
      try
      {
        this.dirListBox.Path = installationPath;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Encompass can't find Calyx Point path '" + installationPath + "'. Please use Browse button to find the path.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        Tracing.Log(ImportPointTemplates.sw, TraceLevel.Error, nameof (ImportPointTemplates), "Can't find path:  " + installationPath + ", exception: " + ex.Message + "\r\n");
      }
    }

    private void SetDestinationFolder()
    {
      string section = string.Empty;
      EllieMae.EMLite.ClientServer.TemplateSettingsType templateSettingsType = EllieMae.EMLite.ClientServer.TemplateSettingsType.CustomLetter;
      bool publicOnly = true;
      switch (this.type)
      {
        case "CC":
          section = "ClosingCostTemplate";
          templateSettingsType = EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost;
          publicOnly = !this.checkPersonalRight(templateSettingsType);
          this.sourceIFS.FileType = FSExplorer.FileTypes.ClosingCosts;
          break;
        case "LP":
          section = "LoanProgramTemplate";
          templateSettingsType = EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram;
          publicOnly = !this.checkPersonalRight(templateSettingsType);
          this.sourceIFS.FileType = FSExplorer.FileTypes.LoanPrograms;
          break;
        case "CF":
          section = "CustomLetterPanel";
          templateSettingsType = EllieMae.EMLite.ClientServer.TemplateSettingsType.CustomLetter;
          publicOnly = !UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas) && !((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_Personal_CustomPrintForms);
          this.sourceIFS.FileType = FSExplorer.FileTypes.CustomForms;
          break;
      }
      string privateProfileString = Session.GetPrivateProfileString(section, "LastFolderViewed");
      if (this.type == "CF")
        this.sourceIFS.SetProperties(true, false, false, false);
      else
        this.sourceIFS.SetProperties(true, false, false, (int) templateSettingsType, true);
      this.sourceIFS.HideDescription = true;
      this.sourceIFS.DisplayFolderOperationButtonsOnly = true;
      PointImportTemplatesIFSExplorer ifsExplorer = new PointImportTemplatesIFSExplorer(this.type);
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
      this.sourceIFS.Init((IFSExplorerBase) ifsExplorer, fileSystemEntry, publicOnly);
    }

    private bool checkPersonalRight(EllieMae.EMLite.ClientServer.TemplateSettingsType type)
    {
      if (UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas))
        return true;
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      AclFeature feature = AclFeature.SettingsTab_Personal_LoanPrograms;
      switch (type)
      {
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram:
          feature = AclFeature.SettingsTab_Personal_LoanPrograms;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost:
          feature = AclFeature.SettingsTab_Personal_ClosingCosts;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData:
          feature = AclFeature.SettingsTab_Personal_MiscDataTemplates;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList:
          feature = AclFeature.SettingsTab_Personal_InputFormSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet:
          feature = AclFeature.SettingsTab_Personal_DocumentSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate:
          feature = AclFeature.SettingsTab_Personal_LoanTemplateSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet:
          feature = AclFeature.SettingsTab_Personal_TaskSets;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders:
          feature = AclFeature.SettingsTab_Personal_SettlementServiceProvider;
          break;
        case EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements:
          feature = AclFeature.SettingsTab_Personal_Affiliate;
          break;
      }
      return aclManager.GetUserApplicationRight(feature);
    }

    private void sourceIFS_FolderChanged(object sender, EventArgs e)
    {
      if (this.sourceIFS.CurrentFolder != null)
      {
        if (this.sourceIFS.CurrentFolder.IsPublic && this.sourceIFS.CurrentFolder.Access == AclResourceAccess.ReadOnly)
          this.importBtn.Enabled = false;
        else
          this.importBtn.Enabled = true;
      }
      else
        this.importBtn.Enabled = false;
    }

    private delegate void UpdateProgressCallback(string text);

    private delegate void CloseProgressCallback(long importedCount);

    public class PointTemplateImportParameters
    {
      private string templateType = string.Empty;
      private string pointTemplateFolder = string.Empty;
      private string emliteFolder = string.Empty;
      private bool isDestinationPublic;
      private string[] fileNames;

      public string TemplateType => this.templateType;

      public string PointTemplateFolder => this.pointTemplateFolder;

      public string EmliteFolder => this.emliteFolder;

      public bool IsDestinationPublic => this.isDestinationPublic;

      public string[] FileNames => this.fileNames;

      public PointTemplateImportParameters(
        string templateType,
        string pointTemplateFolder,
        string emliteFolder,
        bool isDestinationPublic,
        string[] fileNames)
      {
        this.templateType = templateType;
        this.pointTemplateFolder = pointTemplateFolder;
        this.emliteFolder = emliteFolder;
        this.isDestinationPublic = isDestinationPublic;
        this.fileNames = fileNames;
      }
    }
  }
}
