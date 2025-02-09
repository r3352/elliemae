// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TemplateFileSystem
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public abstract class TemplateFileSystem : FileSystemBase
  {
    private Sessions.Session session;
    private EllieMae.EMLite.ClientServer.TemplateSettingsType templateType;

    public TemplateFileSystem(Sessions.Session session, EllieMae.EMLite.ClientServer.TemplateSettingsType templateType)
    {
      this.session = session;
      this.templateType = templateType;
    }

    protected Sessions.Session Session => this.session;

    public BinaryObject GetTemplateObject(FileSystemEntry fileEntry)
    {
      return this.Session.ConfigurationManager.GetTemplateSettings(this.templateType, fileEntry);
    }

    public FileSystemEntry GetLastVisitedFolder()
    {
      string privateProfileString = this.session.GetPrivateProfileString(this.UserSettingsCategory, "LastFolderViewed");
      if ((privateProfileString ?? "") == "")
        return (FileSystemEntry) null;
      try
      {
        return FileSystemEntry.Parse(privateProfileString, this.session.UserID);
      }
      catch
      {
        return (FileSystemEntry) null;
      }
    }

    public void SetLastVisitedFolder(FileSystemEntry folder)
    {
      this.session.WritePrivateProfileString(this.UserSettingsCategory, "LastFolderViewed", folder.ToString());
    }

    public virtual string UserSettingsCategory => this.templateType.ToString();

    public abstract AclFileType FileType { get; }

    public override string FileEntryDisplayName => "Template";

    public override string RootObjectDisplayName => "Templates";

    public override bool IsActionSupported(FileFolderAction action)
    {
      switch (action)
      {
        case FileFolderAction.ImportFile:
        case FileFolderAction.DeployFile:
        case FileFolderAction.ExportFile:
        case FileFolderAction.SetAsDefault:
          return false;
        default:
          return true;
      }
    }

    public override void ConfigureExplorer(FileSystemExplorer explorer)
    {
      int width = explorer.ClientRectangle.Width;
      explorer.ResizeColumn(0, Convert.ToInt32((double) width * 0.3));
      explorer.AddColumn("Description", Convert.ToInt32(0.6 * (double) width));
    }

    public override bool CopyEntry(
      IWin32Window parentWindow,
      FileSystemEntry sourceEntry,
      FileSystemEntry targetEntry)
    {
      this.session.ConfigurationManager.CopyTemplateSettingsObject(this.templateType, sourceEntry, targetEntry);
      return true;
    }

    public override bool CreateFolder(IWin32Window parentWindow, FileSystemEntry entry)
    {
      this.session.ConfigurationManager.CreateTemplateSettingsFolder(this.templateType, entry);
      return true;
    }

    public override bool DeleteEntry(IWin32Window parentWindow, FileSystemEntry entry)
    {
      this.session.ConfigurationManager.DeleteTemplateSettingsObject(this.templateType, entry);
      return true;
    }

    public override bool EntryExists(FileSystemEntry entry)
    {
      return this.session.ConfigurationManager.TemplateSettingsObjectExists(this.templateType, entry);
    }

    public override bool EntryExistsOfAnyType(FileSystemEntry entry)
    {
      return this.session.ConfigurationManager.TemplateSettingsObjectExistsOfAnyType(this.templateType, entry);
    }

    public override bool MoveEntry(
      IWin32Window parentWindow,
      FileSystemEntry sourceEntry,
      FileSystemEntry targetEntry)
    {
      if (targetEntry.Type == FileSystemEntry.Types.Folder && this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram && targetEntry.Name.IndexOf("/") > -1)
      {
        int num = (int) Utils.Dialog(parentWindow, "A folder name cannot contain the \"/\" character.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      this.session.ConfigurationManager.MoveTemplateSettingsObject(this.templateType, sourceEntry, targetEntry);
      return true;
    }

    public override FileSystemEntry[] GetFileSystemEntries(FileSystemEntry parentFolder)
    {
      return this.session.ConfigurationManager.GetFilteredTemplateDirEntries(this.templateType, parentFolder);
    }

    public override FileSystemEntry GetPropertiesAndRights(FileSystemEntry fsEntry)
    {
      return this.session.ConfigurationManager.GetTemplatePropertiesAndRights(this.templateType, fsEntry, true, true);
    }

    public static TemplateFileSystem Create(
      EllieMae.EMLite.ClientServer.TemplateSettingsType templateType,
      Sessions.Session session)
    {
      if (templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram)
        return (TemplateFileSystem) new LoanProgramFileSystem(session);
      throw new ArgumentException("The specified template type is not supported");
    }
  }
}
