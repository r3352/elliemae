// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.ExportCampaignDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class ExportCampaignDialog : Form
  {
    private CampaignIFSExplorer ifsExplorer;
    private List<FileSystemEntry> campaignTemplateEntries;
    private string lastExportFolder = string.Empty;
    private DirectoryInfo zipDirectory;
    private XmlDocument manifest;
    private IContainer components;
    private Button btnCancel;
    private Button btnExport;
    private GroupBox grpHorizontalSeparator;
    private Label lblFolder;
    private Label lblExportTo;
    private DirListBox vbDirectoryListBox;
    private DriveListBox vbDriveListBox;

    private ExportCampaignDialog()
    {
    }

    public ExportCampaignDialog(
      CampaignIFSExplorer ifsExplorer,
      List<FileSystemEntry> campaignTemplateEntries)
    {
      this.ifsExplorer = ifsExplorer;
      this.campaignTemplateEntries = campaignTemplateEntries;
      this.InitializeComponent();
      this.initialize();
    }

    private void initialize()
    {
      this.lastExportFolder = Session.GetPrivateProfileString("CampaignTemplate", "LastExportFolder");
      if (string.Empty != this.lastExportFolder)
      {
        try
        {
          this.vbDriveListBox.Drive = Path.GetPathRoot(this.lastExportFolder);
          this.vbDirectoryListBox.Path = this.lastExportFolder;
        }
        catch
        {
          this.lastExportFolder = string.Empty;
        }
      }
      if (!(string.Empty == this.lastExportFolder))
        return;
      string pathRoot = Path.GetPathRoot(Environment.CurrentDirectory);
      this.vbDriveListBox.Drive = pathRoot;
      this.vbDirectoryListBox.Path = pathRoot;
    }

    private DialogResult exportCampaignTemplates()
    {
      this.zipDirectory = this.createZipDirectory();
      if (this.zipDirectory == null)
        return DialogResult.Abort;
      try
      {
        foreach (FileSystemEntry campaignTemplateEntry in this.campaignTemplateEntries)
        {
          if (FileSystemEntry.Types.File == campaignTemplateEntry.Type)
          {
            CampaignTemplate campaignTemplate = this.loadCampaignTemplate(campaignTemplateEntry);
            if (campaignTemplate != null)
            {
              DialogResult dialogResult = DialogResult.Yes;
              string str;
              DuplicateFileNameDialog duplicateFileNameDialog;
              for (str = Path.Combine(this.vbDirectoryListBox.Path, campaignTemplate.TemplateName + ".emcmpgn"); File.Exists(str); str = Path.Combine(Path.GetDirectoryName(str), duplicateFileNameDialog.FileName + Path.GetExtension(str)))
              {
                duplicateFileNameDialog = new DuplicateFileNameDialog(Path.GetFileNameWithoutExtension(str));
                dialogResult = duplicateFileNameDialog.ShowDialog();
                if (DialogResult.Yes == dialogResult || DialogResult.Cancel == dialogResult)
                  break;
              }
              if (DialogResult.Cancel != dialogResult)
              {
                this.manifest = new XmlDocument();
                this.manifest.LoadXml("<ManifestData Version=\"1.0\" />");
                XmlElement element = this.manifest.CreateElement("CampaignTemplate");
                element.SetAttribute("Name", campaignTemplate.TemplateName);
                this.manifest.DocumentElement.AppendChild((XmlNode) element);
                this.copyCustomLetters(campaignTemplate, element);
                string path = Path.Combine(this.zipDirectory.FullName, campaignTemplate.TemplateName + ".xml");
                ((BinaryObject) (BinaryConvertibleObject) campaignTemplate).Write(path);
                XmlTextWriter w = new XmlTextWriter(Path.Combine(this.zipDirectory.FullName, "manifest.xml"), (Encoding) null);
                w.Formatting = Formatting.Indented;
                this.manifest.Save((XmlWriter) w);
                w.Close();
                if (!this.createExportPackage(str))
                  return DialogResult.Abort;
                foreach (FileSystemInfo file in this.zipDirectory.GetFiles())
                  file.Delete();
              }
            }
          }
        }
        return DialogResult.OK;
      }
      finally
      {
        this.zipDirectory.Delete(true);
        if (this.lastExportFolder != this.vbDirectoryListBox.Path)
          Session.WritePrivateProfileString("CampaignTemplate", "LastExportFolder", this.vbDirectoryListBox.Path);
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

    private CampaignTemplate loadCampaignTemplate(FileSystemEntry campaignTemplateEntry)
    {
      CampaignTemplate campaignTemplate;
      try
      {
        campaignTemplate = (CampaignTemplate) Session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.Campaign, campaignTemplateEntry);
      }
      catch (Exception ex)
      {
        campaignTemplate = (CampaignTemplate) null;
      }
      if (campaignTemplate == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format("Campaign Template '{0}'could not be processed.", (object) campaignTemplateEntry.ToString()), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return campaignTemplate;
    }

    private void copyCustomLetters(CampaignTemplate campaignTemplate, XmlElement templateElement)
    {
      List<string> documentIds = this.getDocumentIds(campaignTemplate);
      if (documentIds.Count == 0)
        return;
      CustomLetterType type = ContactType.BizPartner == (ContactType) new ContactTypeNameProvider().GetValue(campaignTemplate.GetField("ContactType")) ? CustomLetterType.BizPartner : CustomLetterType.Borrower;
      XmlNode element1 = (XmlNode) this.manifest.CreateElement("DocumentIds");
      templateElement.AppendChild(element1);
      foreach (string fsEntryString in documentIds)
      {
        FileSystemEntry fsEntry = this.createFSEntry(fsEntryString);
        if (fsEntry != null)
        {
          BinaryObject customLetter = Session.ConfigurationManager.GetCustomLetter(type, fsEntry);
          if (customLetter != null)
          {
            string path2 = Guid.NewGuid().ToString();
            string path = Path.Combine(this.zipDirectory.FullName, path2);
            customLetter.Write(path);
            XmlElement element2 = this.manifest.CreateElement("DocumentId");
            element2.SetAttribute("Path", fsEntry.ToString());
            element2.SetAttribute("Guid", path2);
            element1.AppendChild((XmlNode) element2);
          }
        }
      }
    }

    private List<string> getDocumentIds(CampaignTemplate campaignTemplate)
    {
      List<string> documentIds = new List<string>();
      foreach (CampaignTemplate.CampaignStepTemplate campaignStepTemplate in (ArrayList) campaignTemplate.CampaignStepTemplates)
      {
        string field = campaignStepTemplate.GetField("DocumentId");
        if (string.Empty != field && !documentIds.Contains(field))
          documentIds.Add(field);
      }
      return documentIds;
    }

    private FileSystemEntry createFSEntry(string fsEntryString)
    {
      FileSystemEntry fsEntry = (FileSystemEntry) null;
      try
      {
        fsEntry = FileSystemEntry.Parse(fsEntryString);
      }
      catch (ArgumentException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format("Custom Letter '{0}' could not be processed.", (object) fsEntryString), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return fsEntry;
    }

    private bool createExportPackage(string packagePath)
    {
      bool exportPackage = false;
      try
      {
        FileCompressor.Instance.ZipDirectory(this.zipDirectory.FullName, packagePath);
        exportPackage = true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Could not write to the selected directory.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return exportPackage;
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

    private void btnExport_Click(object sender, EventArgs e)
    {
      this.DialogResult = this.exportCampaignTemplates();
      if (DialogResult.OK != this.DialogResult)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Export is complete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnExport = new Button();
      this.grpHorizontalSeparator = new GroupBox();
      this.lblFolder = new Label();
      this.lblExportTo = new Label();
      this.vbDirectoryListBox = new DirListBox();
      this.vbDriveListBox = new DriveListBox();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(132, 357);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "&Cancel";
      this.btnExport.Location = new Point(51, 357);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(75, 24);
      this.btnExport.TabIndex = 5;
      this.btnExport.Text = "&Export";
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.grpHorizontalSeparator.Location = new Point(12, 347);
      this.grpHorizontalSeparator.Name = "grpHorizontalSeparator";
      this.grpHorizontalSeparator.Size = new Size(234, 4);
      this.grpHorizontalSeparator.TabIndex = 4;
      this.grpHorizontalSeparator.TabStop = false;
      this.grpHorizontalSeparator.Text = "groupBox1";
      this.lblFolder.AutoSize = true;
      this.lblFolder.Location = new Point(12, 35);
      this.lblFolder.Name = "lblFolder";
      this.lblFolder.Size = new Size(39, 13);
      this.lblFolder.TabIndex = 2;
      this.lblFolder.Text = "Folder:";
      this.lblFolder.TextAlign = ContentAlignment.MiddleLeft;
      this.lblExportTo.AutoSize = true;
      this.lblExportTo.Location = new Point(12, 9);
      this.lblExportTo.Name = "lblExportTo";
      this.lblExportTo.Size = new Size(56, 13);
      this.lblExportTo.TabIndex = 0;
      this.lblExportTo.Text = "Export To:";
      this.lblExportTo.TextAlign = ContentAlignment.MiddleLeft;
      this.vbDirectoryListBox.FormattingEnabled = true;
      this.vbDirectoryListBox.IntegralHeight = false;
      this.vbDirectoryListBox.Location = new Point(12, 51);
      this.vbDirectoryListBox.Name = "vbDirectoryListBox";
      this.vbDirectoryListBox.Size = new Size(234, 289);
      this.vbDirectoryListBox.TabIndex = 3;
      this.vbDriveListBox.FormattingEnabled = true;
      this.vbDriveListBox.Location = new Point(83, 5);
      this.vbDriveListBox.Name = "vbDriveListBox";
      this.vbDriveListBox.Size = new Size(163, 21);
      this.vbDriveListBox.TabIndex = 1;
      this.vbDriveListBox.SelectedIndexChanged += new EventHandler(this.vbDriveListBox_SelectedIndexChanged);
      this.AcceptButton = (IButtonControl) this.btnExport;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(258, 388);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnExport);
      this.Controls.Add((Control) this.grpHorizontalSeparator);
      this.Controls.Add((Control) this.lblFolder);
      this.Controls.Add((Control) this.lblExportTo);
      this.Controls.Add((Control) this.vbDirectoryListBox);
      this.Controls.Add((Control) this.vbDriveListBox);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ExportCampaignDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Export Campaign Template(s)";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
