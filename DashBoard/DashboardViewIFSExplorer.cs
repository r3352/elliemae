// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.DashboardViewIFSExplorer
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientSession.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  internal class DashboardViewIFSExplorer : IFSExplorerBase
  {
    private bool screenNewEntries;
    private FileSystemEntry currentlySelectedFile;
    private Sessions.Session session;

    public DashboardViewIFSExplorer()
    {
    }

    public DashboardViewIFSExplorer(
      bool screenNewEntries,
      DashboardViewList viewList,
      Sessions.Session session)
    {
      this.session = session;
      this.screenNewEntries = screenNewEntries;
    }

    public void SaveDashboardViewTemplate(
      FileSystemEntry fsFile,
      DashboardViewTemplate dashboardViewTemplate)
    {
      this.session.ConfigurationManager.SaveTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, fsFile, (BinaryObject) (BinaryConvertibleObject) dashboardViewTemplate);
    }

    public DashboardViewTemplate LoadDashboardViewTemplate(FileSystemEntry fsFile)
    {
      return (DashboardViewTemplate) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, fsFile);
    }

    public override FileSystemEntry[] GetFileSystemEntries(FileSystemEntry parentEntry)
    {
      if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, parentEntry))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The folder cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileSystemEntry[]) null;
      }
      FileSystemEntry[] templateDirEntries = this.session.ConfigurationManager.GetFilteredTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, parentEntry);
      if (!this.screenNewEntries)
        return templateDirEntries;
      List<FileSystemEntry> fileSystemEntryList = new List<FileSystemEntry>();
      foreach (FileSystemEntry fileSystemEntry in templateDirEntries)
        fileSystemEntryList.Add(fileSystemEntry);
      return fileSystemEntryList.ToArray();
    }

    public override void CreateFolder(FileSystemEntry fileSystemEntry)
    {
      this.session.ConfigurationManager.CreateTemplateSettingsFolder(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, fileSystemEntry);
    }

    public override bool CreateNew(FileSystemEntry fileSystemEntry)
    {
      DashboardViewTemplate data = new DashboardViewTemplate(Guid.NewGuid().ToString(), fileSystemEntry.Name);
      this.session.ConfigurationManager.SaveTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, fileSystemEntry, (BinaryObject) (BinaryConvertibleObject) data);
      return true;
    }

    public override void OpenFile(FileSystemEntry fileSystemEntry, GVItem lvItem)
    {
      this.currentlySelectedFile = fileSystemEntry;
      this.OnOpenFileEvent(new SelectedFileEventArgs(fileSystemEntry));
    }

    public override void DeleteEntry(FileSystemEntry fileSystemEntry)
    {
      if (fileSystemEntry.Type == FileSystemEntry.Types.File && this.session.ConfigurationManager.IsReferencedAsDefaultViewTemplatePath(fileSystemEntry))
      {
        if (Utils.Dialog((IWin32Window) this, "Dashboard view '" + fileSystemEntry.Name + "' is referenced by one or more users as the default dashboard view.  Are you sure you want to delete this template?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        this.session.ConfigurationManager.RemoveAllDefaultViewReference(fileSystemEntry);
      }
      this.session.ConfigurationManager.DeleteTemplateSettingsObject(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, fileSystemEntry);
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
        this.DeleteEntry(source);
      return flag;
    }

    private bool moveFile(FileSystemEntry source, FileSystemEntry target)
    {
      if (!source.IsPublic && target.IsPublic)
      {
        DashboardViewInfo dashboardView = this.session.ReportManager.GetDashboardView(((DashboardViewTemplate) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, source)).ViewGuid);
        if (dashboardView != null && dashboardView.DashboardReportInfos != null && dashboardView.DashboardReportInfos.Length != 0)
        {
          bool flag = false;
          foreach (DashboardReportInfo dashboardReportInfo in dashboardView.DashboardReportInfos)
          {
            if (dashboardReportInfo.DashboardTemplatePath.StartsWith("Personal:"))
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "This Dashboard view contains one ore more personal snapshots, so it can not be moved to a public folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
      }
      this.session.ConfigurationManager.MoveTemplateSettingsObject(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, source, target);
      return true;
    }

    public override void CopyEntry(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.Type == FileSystemEntry.Types.Folder)
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
      if (!source.IsPublic && target.IsPublic)
      {
        DashboardViewInfo dashboardView = this.session.ReportManager.GetDashboardView(((DashboardViewTemplate) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, source)).ViewGuid);
        if (dashboardView != null && dashboardView.DashboardReportInfos != null && dashboardView.DashboardReportInfos.Length != 0)
        {
          bool flag = false;
          foreach (DashboardReportInfo dashboardReportInfo in dashboardView.DashboardReportInfos)
          {
            if (dashboardReportInfo.DashboardTemplatePath.StartsWith("Personal:"))
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Dashboard view '" + dashboardView.ViewName + "'  contains one ore more personal snapshots, so it can not be moved to a public folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
      }
      this.session.ConfigurationManager.CopyTemplateSettingsObject(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, source, target);
      this.OnDuplicateFileEvent(new SelectedFileEventArgs(source), new SelectedFileEventArgs(target));
      return true;
    }

    public override bool EntryExists(FileSystemEntry fileSystemEntry)
    {
      return this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, fileSystemEntry);
    }

    public override bool EntryExistsOfAnyType(FileSystemEntry fileSystemEntry)
    {
      return this.session.ConfigurationManager.TemplateSettingsObjectExistsOfAnyType(EllieMae.EMLite.ClientServer.TemplateSettingsType.DashboardViewTemplate, fileSystemEntry);
    }

    public override string NewDocBaseName => "New Dashboard View";

    public event DashboardViewIFSExplorer.OpenFileEventHandler OpenFileEvent;

    protected virtual void OnOpenFileEvent(SelectedFileEventArgs e)
    {
      if (this.OpenFileEvent == null)
        return;
      this.OpenFileEvent((object) this, e);
    }

    public event DashboardViewIFSExplorer.DeleteFileEventHandler DeleteFileEvent;

    protected virtual void OnDeleteFileEvent(SelectedFileEventArgs e)
    {
      if (this.DeleteFileEvent == null)
        return;
      this.DeleteFileEvent((object) this, e);
    }

    public event DashboardViewIFSExplorer.MoveEntryEventHandler MoveEntryEvent;

    protected virtual void OnMoveEntryEvent(SelectedFileEventArgs e)
    {
      if (this.MoveEntryEvent == null)
        return;
      this.MoveEntryEvent((object) this, e);
    }

    public event DashboardViewIFSExplorer.DuplicateFileEventHandler DuplicateFileEvent;

    protected virtual void OnDuplicateFileEvent(
      SelectedFileEventArgs source,
      SelectedFileEventArgs target)
    {
      if (this.DuplicateFileEvent == null)
        return;
      this.DuplicateFileEvent((object) this, source, target);
    }

    public delegate void OpenFileEventHandler(object sender, SelectedFileEventArgs e);

    public delegate void DeleteFileEventHandler(object sender, SelectedFileEventArgs e);

    public delegate void MoveEntryEventHandler(object sender, SelectedFileEventArgs e);

    public delegate void DuplicateFileEventHandler(
      object sender,
      SelectedFileEventArgs source,
      SelectedFileEventArgs target);
  }
}
