// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.DashboardIFSExplorer
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  internal class DashboardIFSExplorer : IFSExplorerBase
  {
    private bool screenNewEntries;
    private bool internalTrigger;
    private Sessions.Session session;

    public DashboardIFSExplorer()
      : this(Session.DefaultInstance)
    {
    }

    public DashboardIFSExplorer(Sessions.Session session) => this.session = session;

    public DashboardIFSExplorer(bool screenNewEntries, Sessions.Session session)
      : this(session)
    {
      this.screenNewEntries = screenNewEntries;
    }

    public void SaveDashboardTemplate(FileSystemEntry fsFile, DashboardTemplate dashboardTemplate)
    {
      this.session.ConfigurationManager.SaveTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, fsFile, (BinaryObject) (BinaryConvertibleObject) dashboardTemplate);
    }

    public DashboardTemplate LoadDashboardTemplate(FileSystemEntry fsFile)
    {
      try
      {
        return (DashboardTemplate) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, fsFile);
      }
      catch
      {
        return (DashboardTemplate) null;
      }
    }

    public Dictionary<FileSystemEntry, DashboardTemplate> LoadDashboardTemplate(
      FileSystemEntry[] fsFiles)
    {
      try
      {
        Dictionary<FileSystemEntry, BinaryObject> templateSettings = this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, fsFiles);
        Dictionary<FileSystemEntry, DashboardTemplate> dictionary = new Dictionary<FileSystemEntry, DashboardTemplate>();
        foreach (FileSystemEntry key in templateSettings.Keys)
        {
          if (templateSettings[key] == null)
            dictionary.Add(key, (DashboardTemplate) null);
          else
            dictionary.Add(key, (DashboardTemplate) templateSettings[key]);
        }
        return dictionary;
      }
      catch
      {
        return new Dictionary<FileSystemEntry, DashboardTemplate>();
      }
    }

    public override FileSystemEntry[] GetFileSystemEntries(FileSystemEntry parentEntry)
    {
      if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, parentEntry))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The folder cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileSystemEntry[]) null;
      }
      FileSystemEntry[] templateDirEntries = this.session.ConfigurationManager.GetFilteredTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, parentEntry);
      if (!this.screenNewEntries)
        return templateDirEntries;
      List<FileSystemEntry> fileSystemEntryList = new List<FileSystemEntry>();
      foreach (FileSystemEntry fileSystemEntry in templateDirEntries)
      {
        if (FileSystemEntry.Types.File != fileSystemEntry.Type || !("true" == fileSystemEntry.Properties[(object) "IsNewTemplate"].ToString()))
          fileSystemEntryList.Add(fileSystemEntry);
      }
      return fileSystemEntryList.ToArray();
    }

    public override void CreateFolder(FileSystemEntry fileSystemEntry)
    {
      this.session.ConfigurationManager.CreateTemplateSettingsFolder(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, fileSystemEntry);
    }

    public override bool CreateNew(FileSystemEntry fileSystemEntry)
    {
      DashboardTemplate data = new DashboardTemplate(fileSystemEntry.Name, string.Empty);
      data.DataSourceType = DashboardDataSourceType.LoanData;
      List<string> stringList = new List<string>();
      string[] allLoanFolderNames = this.session.LoanManager.GetAllLoanFolderNames(false);
      if (allLoanFolderNames != null)
      {
        foreach (string str in allLoanFolderNames)
        {
          if (!str.StartsWith("(") || !str.EndsWith(")"))
            stringList.Add(str);
        }
      }
      data.Folders = stringList;
      this.session.ConfigurationManager.SaveTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, fileSystemEntry, (BinaryObject) (BinaryConvertibleObject) data);
      return true;
    }

    public override void OpenFile(FileSystemEntry fileSystemEntry, GVItem lvItem)
    {
      this.OnOpenFileEvent(new SelectedFileEventArgs(fileSystemEntry));
    }

    public override void DeleteEntry(FileSystemEntry fileSystemEntry)
    {
      if (!this.internalTrigger)
      {
        DashboardViewInfo[] referencedDashboardViews = this.session.ReportManager.GetReferencedDashboardViews(fileSystemEntry.ToString(), false);
        if (referencedDashboardViews != null && referencedDashboardViews.Length != 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "This Snapshot is referenced by one or more Views and cannot be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (DialogResult.No == Utils.Dialog((IWin32Window) this, "Are you sure you want to delete this " + (FileSystemEntry.Types.File == fileSystemEntry.Type ? "snapshot?" : "folder?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
          return;
      }
      this.session.ConfigurationManager.DeleteTemplateSettingsObject(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, fileSystemEntry);
      this.OnDeleteFileEvent(new SelectedFileEventArgs(fileSystemEntry));
    }

    public override void MoveEntry(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.Type == FileSystemEntry.Types.Folder)
        this.moveFolder(source, target);
      else
        this.moveFile(source, target);
      this.OnMoveEntryEvent(new SelectedFileEventArgs(target));
    }

    private bool moveFolder(FileSystemEntry source, FileSystemEntry target)
    {
      FileSystemEntry[] fileSystemEntries = this.GetFileSystemEntries(source);
      this.CreateFolder(target);
      bool flag = true;
      for (int index = 0; index < fileSystemEntries.Length; ++index)
        flag = fileSystemEntries[index].Type != FileSystemEntry.Types.File ? this.moveFolder(fileSystemEntries[index], new FileSystemEntry(target.Path, fileSystemEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner)) & flag : this.moveFile(fileSystemEntries[index], target) & flag;
      if (flag)
      {
        this.internalTrigger = true;
        this.DeleteEntry(source);
        this.internalTrigger = false;
      }
      return flag;
    }

    private bool moveFile(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.IsPublic && !target.IsPublic)
      {
        DashboardViewInfo[] referencedDashboardViews = this.session.ReportManager.GetReferencedDashboardViews(source.ToString(), true);
        if (referencedDashboardViews != null && referencedDashboardViews.Length != 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Snapshot '" + source.Name + "' is referenced by one or more public dashboard views, so it can not be moved to a personal folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      this.session.ConfigurationManager.MoveTemplateSettingsObject(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, source, target);
      return true;
    }

    public override void CopyEntry(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.IsPublic || !target.IsPublic)
        this.session.ConfigurationManager.CopyTemplateSettingsObject(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, source, target);
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

    private bool copyFile(FileSystemEntry source, FileSystemEntry target)
    {
      this.session.ConfigurationManager.CopyTemplateSettingsObject(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, source, target);
      return true;
    }

    public override bool EntryExists(FileSystemEntry fileSystemEntry)
    {
      return this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, fileSystemEntry);
    }

    public override bool EntryExistsOfAnyType(FileSystemEntry fileSystemEntry)
    {
      return this.session.ConfigurationManager.TemplateSettingsObjectExistsOfAnyType(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardTemplate, fileSystemEntry);
    }

    public override string NewDocBaseName => "New Snapshot";

    public event DashboardIFSExplorer.OpenFileEventHandler OpenFileEvent;

    protected virtual void OnOpenFileEvent(SelectedFileEventArgs e)
    {
      if (this.OpenFileEvent == null)
        return;
      this.OpenFileEvent((object) this, e);
    }

    public event DashboardIFSExplorer.DeleteFileEventHandler DeleteFileEvent;

    protected virtual void OnDeleteFileEvent(SelectedFileEventArgs e)
    {
      if (this.DeleteFileEvent == null)
        return;
      this.DeleteFileEvent((object) this, e);
    }

    public event DashboardIFSExplorer.MoveEntryEventHandler MoveEntryEvent;

    protected virtual void OnMoveEntryEvent(SelectedFileEventArgs e)
    {
      if (this.MoveEntryEvent == null)
        return;
      this.MoveEntryEvent((object) this, e);
    }

    public delegate void OpenFileEventHandler(object sender, SelectedFileEventArgs e);

    public delegate void DeleteFileEventHandler(object sender, SelectedFileEventArgs e);

    public delegate void MoveEntryEventHandler(object sender, SelectedFileEventArgs e);
  }
}
