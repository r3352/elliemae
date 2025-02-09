// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.ImportCampaignDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class ImportCampaignDialog : Form
  {
    private CampaignIFSExplorer campaignIFSExplorer;
    private FileSystemEntry importToEntry;
    private string lastImportFolder = string.Empty;
    private DirectoryInfo zipDirectory;
    private XmlDocument manifest;
    private System.ComponentModel.Container components;
    private Label lblImportFrom;
    private Label lblFolder;
    private Label lblFiles;
    private GroupBox grpHorizontalSeparator;
    private Button btnCancel;
    private Button btnImport;
    private FileListBox vbFilesListBox;
    private DirListBox vbDirectoryListBox;
    private DriveListBox vbDriveListBox;

    private ImportCampaignDialog()
    {
    }

    public ImportCampaignDialog(
      CampaignIFSExplorer campaignIFSExplorer,
      FileSystemEntry importToEntry)
    {
      this.campaignIFSExplorer = campaignIFSExplorer;
      this.importToEntry = importToEntry;
      this.InitializeComponent();
      this.initialize();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void initialize()
    {
      this.lastImportFolder = Session.GetPrivateProfileString("CampaignTemplate", "LastImportFolder");
      if (string.Empty != this.lastImportFolder)
      {
        try
        {
          this.vbDriveListBox.Drive = Path.GetPathRoot(this.lastImportFolder);
          this.vbDirectoryListBox.Path = this.lastImportFolder;
          this.vbFilesListBox.Path = this.lastImportFolder;
        }
        catch
        {
          this.lastImportFolder = string.Empty;
        }
      }
      if (!(string.Empty == this.lastImportFolder))
        return;
      string pathRoot = Path.GetPathRoot(Environment.CurrentDirectory);
      this.vbDriveListBox.Drive = pathRoot;
      this.vbDirectoryListBox.Path = pathRoot;
      this.vbFilesListBox.Path = pathRoot;
    }

    private DialogResult importCampaignTemplates()
    {
      this.zipDirectory = this.createZipDirectory();
      if (this.zipDirectory == null)
        return DialogResult.Abort;
      try
      {
        foreach (object selectedItem in this.vbFilesListBox.SelectedItems)
        {
          FileCompressor.Instance.Unzip(Path.Combine(this.vbDirectoryListBox.Path, selectedItem.ToString()), this.zipDirectory.FullName);
          this.manifest = this.getManifestDocument();
          if (this.manifest == null)
            return DialogResult.Abort;
          XmlNodeList elementsByTagName = this.manifest.GetElementsByTagName("CampaignTemplate");
          if (elementsByTagName.Count != 0)
          {
            CampaignTemplate campaignTemplate = this.getCampaignTemplate(elementsByTagName[0].Attributes["Name"].Value);
            if (campaignTemplate != null && this.saveCustomLetters(campaignTemplate))
              this.saveCampaignTemplate(campaignTemplate);
          }
        }
        return DialogResult.OK;
      }
      finally
      {
        this.zipDirectory.Delete(true);
        if (this.lastImportFolder != this.vbDirectoryListBox.Path)
          Session.WritePrivateProfileString("CampaignTemplate", "LastImportFolder", this.vbDirectoryListBox.Path);
      }
    }

    private DirectoryInfo createZipDirectory()
    {
      try
      {
        string path = Path.GetTempPath() + Guid.NewGuid().ToString();
        if (Directory.Exists(path))
          Directory.Delete(path, true);
        return Directory.CreateDirectory(path);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Could not create temporary directory.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (DirectoryInfo) null;
      }
    }

    private XmlDocument getManifestDocument()
    {
      XmlDocument manifestDocument = (XmlDocument) null;
      XmlTextReader reader = (XmlTextReader) null;
      try
      {
        string url = Path.Combine(this.zipDirectory.FullName, "manifest.xml");
        manifestDocument = new XmlDocument();
        reader = new XmlTextReader(url);
        manifestDocument.Load((XmlReader) reader);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error reading manifest file 'manifest.xml'. The import package appears to be corrupt.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        manifestDocument = (XmlDocument) null;
      }
      finally
      {
        reader?.Close();
      }
      return manifestDocument;
    }

    private CampaignTemplate getCampaignTemplate(string campaignTemplateName)
    {
      try
      {
        using (BinaryObject campaignTemplate = new BinaryObject(Path.Combine(this.zipDirectory.FullName, campaignTemplateName + ".xml")))
          return (CampaignTemplate) campaignTemplate;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format("Error reading campaign template file '{0}'. The import package appears to be corrupt.", (object) (campaignTemplateName + ".xml")), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (CampaignTemplate) null;
      }
    }

    private bool saveCustomLetters(CampaignTemplate campaignTemplate)
    {
      XmlNodeList elementsByTagName = this.manifest.GetElementsByTagName("DocumentId");
      if (elementsByTagName.Count == 0)
        return true;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (XmlNode xmlNode in elementsByTagName)
      {
        string str = xmlNode.Attributes.Item(0).Value;
        string key = xmlNode.Attributes.Item(1).Value;
        dictionary.Add(key, str);
      }
      CustomLetterDialog customLetterDialog = new CustomLetterDialog((ContactType) new ContactTypeNameProvider().GetValue(campaignTemplate.GetField("ContactType")));
      if (DialogResult.Cancel == customLetterDialog.ShowDialog())
        return false;
      FileSystemEntry selectedFolder = customLetterDialog.SelectedFolder;
      Dictionary<string, string> pathMap = new Dictionary<string, string>();
      foreach (string key in dictionary.Keys)
      {
        string sourceFilePath = Path.Combine(this.zipDirectory.FullName, key);
        FileSystemEntry fsTargetFile = new FileSystemEntry(selectedFolder.Path, Path.GetFileName(dictionary[key]), FileSystemEntry.Types.File, selectedFolder.Owner);
        FileSystemEntry fileSystemEntry = customLetterDialog.Import(sourceFilePath, fsTargetFile);
        if (fileSystemEntry != null)
          pathMap.Add(dictionary[key], fileSystemEntry.ToString());
      }
      this.updateCampaignTemplate(campaignTemplate, pathMap);
      return true;
    }

    private bool updateCampaignTemplate(
      CampaignTemplate campaignTemplate,
      Dictionary<string, string> pathMap)
    {
      foreach (CampaignTemplate.CampaignStepTemplate campaignStepTemplate in (ArrayList) campaignTemplate.CampaignStepTemplates)
      {
        string field = campaignStepTemplate.GetField("DocumentId");
        if (string.Empty != field && pathMap.ContainsKey(field))
          campaignStepTemplate["DocumentId"] = (object) pathMap[field];
      }
      return true;
    }

    private bool saveCampaignTemplate(CampaignTemplate campaignTemplate)
    {
      try
      {
        return this.campaignIFSExplorer.SaveCampaignTemplate(new FileSystemEntry(this.importToEntry.Path, campaignTemplate.TemplateName, FileSystemEntry.Types.File, this.importToEntry.Owner), campaignTemplate);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format("Error writing campaign template file '{0}'. The import package appears to be corrupt.", (object) campaignTemplate.TemplateName), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    private void vbDirectoryListBox_Change(object sender, EventArgs e)
    {
      this.vbFilesListBox.Path = this.vbDirectoryListBox.Path;
    }

    private void vbDriveListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.vbDirectoryListBox.Path = this.vbDriveListBox.Drive;
      }
      catch (IOException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Device " + this.vbDriveListBox.Drive + " is unavailable.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        string path = this.vbDirectoryListBox.Path;
        this.vbDriveListBox.Drive = this.vbDirectoryListBox.Path.Substring(0, this.vbDirectoryListBox.Path.IndexOf("\\"));
        this.vbDirectoryListBox.Path = path;
      }
    }

    private void vbFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnImport.Enabled = 0 < this.vbFilesListBox.SelectedItems.Count;
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      this.DialogResult = this.importCampaignTemplates();
    }

    private void InitializeComponent()
    {
      this.vbFilesListBox = new FileListBox();
      this.vbDirectoryListBox = new DirListBox();
      this.vbDriveListBox = new DriveListBox();
      this.lblImportFrom = new Label();
      this.lblFolder = new Label();
      this.lblFiles = new Label();
      this.grpHorizontalSeparator = new GroupBox();
      this.btnCancel = new Button();
      this.btnImport = new Button();
      this.SuspendLayout();
      this.vbFilesListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.vbFilesListBox.FormattingEnabled = true;
      this.vbFilesListBox.Location = new Point(252, 51);
      this.vbFilesListBox.Name = "vbFilesListBox";
      this.vbFilesListBox.Pattern = "*.emcmpgn";
      this.vbFilesListBox.SelectionMode = SelectionMode.MultiExtended;
      this.vbFilesListBox.Size = new Size(234, 290);
      this.vbFilesListBox.TabIndex = 5;
      this.vbFilesListBox.SelectedIndexChanged += new EventHandler(this.vbFilesListBox_SelectedIndexChanged);
      this.vbDirectoryListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.vbDirectoryListBox.FormattingEnabled = true;
      this.vbDirectoryListBox.IntegralHeight = false;
      this.vbDirectoryListBox.Location = new Point(12, 51);
      this.vbDirectoryListBox.Name = "vbDirectoryListBox";
      this.vbDirectoryListBox.Size = new Size(234, 289);
      this.vbDirectoryListBox.TabIndex = 3;
      this.vbDirectoryListBox.Change += new EventHandler(this.vbDirectoryListBox_Change);
      this.vbDriveListBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.vbDriveListBox.FormattingEnabled = true;
      this.vbDriveListBox.Location = new Point(83, 5);
      this.vbDriveListBox.Name = "vbDriveListBox";
      this.vbDriveListBox.Size = new Size(163, 21);
      this.vbDriveListBox.TabIndex = 1;
      this.vbDriveListBox.SelectedIndexChanged += new EventHandler(this.vbDriveListBox_SelectedIndexChanged);
      this.lblImportFrom.AutoSize = true;
      this.lblImportFrom.Location = new Point(12, 9);
      this.lblImportFrom.Name = "lblImportFrom";
      this.lblImportFrom.Size = new Size(65, 13);
      this.lblImportFrom.TabIndex = 0;
      this.lblImportFrom.Text = "Import From:";
      this.lblImportFrom.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFolder.AutoSize = true;
      this.lblFolder.Location = new Point(12, 35);
      this.lblFolder.Name = "lblFolder";
      this.lblFolder.Size = new Size(39, 13);
      this.lblFolder.TabIndex = 2;
      this.lblFolder.Text = "Folder:";
      this.lblFolder.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFiles.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblFiles.AutoSize = true;
      this.lblFiles.Location = new Point(252, 35);
      this.lblFiles.Name = "lblFiles";
      this.lblFiles.Size = new Size(31, 13);
      this.lblFiles.TabIndex = 4;
      this.lblFiles.Text = "Files:";
      this.lblFiles.TextAlign = ContentAlignment.MiddleLeft;
      this.grpHorizontalSeparator.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpHorizontalSeparator.Location = new Point(12, 347);
      this.grpHorizontalSeparator.Name = "grpHorizontalSeparator";
      this.grpHorizontalSeparator.Size = new Size(474, 4);
      this.grpHorizontalSeparator.TabIndex = 6;
      this.grpHorizontalSeparator.TabStop = false;
      this.grpHorizontalSeparator.Text = "groupBox1";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(252, 357);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "&Cancel";
      this.btnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnImport.Enabled = false;
      this.btnImport.Location = new Point(171, 357);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(75, 24);
      this.btnImport.TabIndex = 7;
      this.btnImport.Text = "&Import";
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.AcceptButton = (IButtonControl) this.btnImport;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(498, 388);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnImport);
      this.Controls.Add((Control) this.grpHorizontalSeparator);
      this.Controls.Add((Control) this.lblFiles);
      this.Controls.Add((Control) this.lblFolder);
      this.Controls.Add((Control) this.lblImportFrom);
      this.Controls.Add((Control) this.vbFilesListBox);
      this.Controls.Add((Control) this.vbDirectoryListBox);
      this.Controls.Add((Control) this.vbDriveListBox);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportCampaignDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import Campaign Template(s)";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
