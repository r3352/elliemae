// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignIFSExplorer
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignIFSExplorer : IFSExplorerBase
  {
    private EllieMae.EMLite.ClientServer.TemplateSettingsType tpType = EllieMae.EMLite.ClientServer.TemplateSettingsType.Campaign;
    private FSExplorer fsExplorer;
    private bool noAccessCheck;

    public CampaignIFSExplorer(FSExplorer fsExplorer) => this.fsExplorer = fsExplorer;

    public CampaignIFSExplorer(FSExplorer fsExplorer, bool noAccessCheck)
    {
      this.fsExplorer = fsExplorer;
      this.noAccessCheck = noAccessCheck;
    }

    public bool SaveCampaignTemplate(FileSystemEntry fsFile, CampaignTemplate campaignTemplate)
    {
      while (this.EntryExists(fsFile))
      {
        DuplicateFileNameDialog duplicateFileNameDialog = new DuplicateFileNameDialog(Path.GetFileNameWithoutExtension(fsFile.Name));
        DialogResult dialogResult = duplicateFileNameDialog.ShowDialog();
        if (DialogResult.Cancel == dialogResult)
          return false;
        if (DialogResult.Yes != dialogResult)
        {
          fsFile = new FileSystemEntry(Path.Combine(Path.GetDirectoryName(fsFile.Path), duplicateFileNameDialog.FileName + Path.GetExtension(fsFile.Path)), fsFile.Type, fsFile.Owner);
          campaignTemplate.TemplateName = duplicateFileNameDialog.FileName;
        }
        else
          break;
      }
      Session.ConfigurationManager.SaveTemplateSettings(this.tpType, fsFile, (BinaryObject) (BinaryConvertibleObject) campaignTemplate);
      return true;
    }

    public CampaignTemplate LoadCampaignTemplate(FileSystemEntry fsFile)
    {
      return (CampaignTemplate) Session.ConfigurationManager.GetTemplateSettings(this.tpType, fsFile);
    }

    public override FileSystemEntry[] GetFileSystemEntries(FileSystemEntry parentEntry)
    {
      if (!Session.ConfigurationManager.TemplateSettingsObjectExists(this.tpType, parentEntry))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The folder cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileSystemEntry[]) null;
      }
      return !this.noAccessCheck ? Session.ConfigurationManager.GetFilteredTemplateDirEntries(this.tpType, parentEntry) : Session.ConfigurationManager.GetTemplateDirEntries(this.tpType, parentEntry);
    }

    public override void CreateFolder(FileSystemEntry entry)
    {
      Session.ConfigurationManager.CreateTemplateSettingsFolder(this.tpType, entry);
    }

    public override bool CreateNew(FileSystemEntry entry)
    {
      return this.OnCreateNewEvent(new SelectedFileEventArgs(entry));
    }

    public override void OpenFile(FileSystemEntry fileInfo, GVItem lvItem)
    {
      this.OnOpenFileEvent(new SelectedFileEventArgs(fileInfo));
    }

    public override void DeleteEntry(FileSystemEntry entry)
    {
      Session.ConfigurationManager.DeleteTemplateSettingsObject(this.tpType, entry);
    }

    public override void MoveEntry(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.IsPublic || !target.IsPublic)
        Session.ConfigurationManager.MoveTemplateSettingsObject(this.tpType, source, target);
      else if (source.Type == FileSystemEntry.Types.Folder)
        this.moveFolder(source, target);
      else
        this.moveFile(source, target);
    }

    private bool moveFolder(FileSystemEntry source, FileSystemEntry target)
    {
      FileSystemEntry[] fileSystemEntries = this.GetFileSystemEntries(source);
      this.CreateFolder(target);
      bool flag = true;
      for (int index = 0; index < fileSystemEntries.Length; ++index)
        flag = fileSystemEntries[index].Type != FileSystemEntry.Types.File ? this.moveFolder(fileSystemEntries[index], new FileSystemEntry(target.Path, fileSystemEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner)) & flag : this.moveFile(fileSystemEntries[index], target) & flag;
      if (flag)
        this.DeleteEntry(source);
      return flag;
    }

    private bool moveFile(FileSystemEntry source, FileSystemEntry target)
    {
      Session.ConfigurationManager.MoveTemplateSettingsObject(this.tpType, source, target);
      return true;
    }

    public override void CopyEntry(FileSystemEntry source, FileSystemEntry target)
    {
      Session.ConfigurationManager.CopyTemplateSettingsObject(this.tpType, source, target);
      if (source.IsPublic || !target.IsPublic)
        Session.ConfigurationManager.CopyTemplateSettingsObject(this.tpType, source, target);
      else if (source.Type == FileSystemEntry.Types.Folder)
        this.copyFolder(source, target);
      else
        this.copyFile(source, target);
    }

    private void copyFolder(FileSystemEntry source, FileSystemEntry target)
    {
      FileSystemEntry[] fileSystemEntries = this.GetFileSystemEntries(source);
      this.CreateFolder(target);
      for (int index = 0; index < fileSystemEntries.Length; ++index)
      {
        if (fileSystemEntries[index].Type == FileSystemEntry.Types.File)
          this.copyFile(fileSystemEntries[index], target);
        else
          this.copyFolder(fileSystemEntries[index], new FileSystemEntry(target.Path, fileSystemEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner));
      }
    }

    public bool copyFile(FileSystemEntry source, FileSystemEntry target)
    {
      Session.ConfigurationManager.CopyTemplateSettingsObject(this.tpType, source, target);
      return true;
    }

    public override bool EntryExists(FileSystemEntry entry)
    {
      return Session.ConfigurationManager.TemplateSettingsObjectExists(this.tpType, entry);
    }

    public override bool EntryExistsOfAnyType(FileSystemEntry entry)
    {
      return Session.ConfigurationManager.TemplateSettingsObjectExistsOfAnyType(this.tpType, entry);
    }

    public override string NewDocBaseName => "New Campaign Template";

    public override string GetDescription(FileSystemEntry entry)
    {
      string str = entry.Properties[(object) "Description"].ToString();
      if (str != null && string.Empty != str)
        str = str.Replace("\r\n", "  ");
      return str ?? string.Empty;
    }

    public override void Deploy(FileSystemEntry fsEntry)
    {
      this.OnDeployEvent(new SelectedFileEventArgs(fsEntry));
    }

    public override void Import(FileSystemEntry fsEntry)
    {
      this.OnImportEvent(new SelectedFileEventArgs(fsEntry));
    }

    public override void Export(List<FileSystemEntry> fsEntryList)
    {
      this.OnExportEvent(new SelectedFileListEventArgs(fsEntryList));
    }

    public event CampaignIFSExplorer.CreateNewEventHandler CreateNewEvent;

    protected virtual bool OnCreateNewEvent(SelectedFileEventArgs e)
    {
      if (this.CreateNewEvent != null)
        this.CreateNewEvent((object) this, e);
      return true;
    }

    public event CampaignIFSExplorer.OpenFileEventHandler OpenFileEvent;

    protected virtual void OnOpenFileEvent(SelectedFileEventArgs e)
    {
      if (this.OpenFileEvent == null)
        return;
      this.OpenFileEvent((object) this, e);
    }

    public event CampaignIFSExplorer.DeployEventHandler DeployEvent;

    protected virtual void OnDeployEvent(SelectedFileEventArgs e)
    {
      if (this.DeployEvent == null)
        return;
      this.DeployEvent((object) this, e);
    }

    public event CampaignIFSExplorer.ImportEventHandler ImportEvent;

    protected virtual void OnImportEvent(SelectedFileEventArgs e)
    {
      if (this.ImportEvent == null)
        return;
      this.ImportEvent((object) this, e);
    }

    public event CampaignIFSExplorer.ExportEventHandler ExportEvent;

    protected virtual void OnExportEvent(SelectedFileListEventArgs e)
    {
      if (this.ExportEvent == null)
        return;
      this.ExportEvent((object) this, e);
    }

    public delegate void CreateNewEventHandler(object sender, SelectedFileEventArgs e);

    public delegate void OpenFileEventHandler(object sender, SelectedFileEventArgs e);

    public delegate void DeployEventHandler(object sender, SelectedFileEventArgs e);

    public delegate void ImportEventHandler(object sender, SelectedFileEventArgs e);

    public delegate void ExportEventHandler(object sender, SelectedFileListEventArgs e);
  }
}
