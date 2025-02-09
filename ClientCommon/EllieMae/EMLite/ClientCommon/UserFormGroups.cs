// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.UserFormGroups
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientCommon
{
  public class UserFormGroups : IFSExplorerBase
  {
    private bool noAccessCheck;
    private Sessions.Session session;

    public UserFormGroups(Sessions.Session session) => this.session = session;

    public UserFormGroups(Sessions.Session session, bool noAccessCheck)
    {
      this.session = session;
      this.noAccessCheck = noAccessCheck;
    }

    public override FileSystemEntry[] GetFileSystemEntries(FileSystemEntry parentFolder)
    {
      try
      {
        return !this.noAccessCheck ? this.session.ConfigurationManager.GetFilteredFormGroupDirEntries(parentFolder) : this.session.ConfigurationManager.GetFormGroupDirEntries(parentFolder);
      }
      catch (ObjectNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The folder cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileSystemEntry[]) null;
      }
    }

    public override string NewDocBaseName => "New Form Group";

    public override void DeleteEntry(FileSystemEntry entry)
    {
      this.session.ConfigurationManager.DeleteFormGroupObject(entry);
    }

    public override void MoveEntry(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.IsPublic || !target.IsPublic)
        this.session.ConfigurationManager.MoveFormGroupObject(source, target);
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
      if (!this.checkForPersonalContents(source))
        return false;
      this.session.ConfigurationManager.MoveFormGroupObject(source, target);
      return true;
    }

    public override void CopyEntry(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.IsPublic || !target.IsPublic)
        this.session.ConfigurationManager.CopyFormGroupObject(source, target);
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
      if (!this.checkForPersonalContents(source))
        return false;
      this.session.ConfigurationManager.CopyFormGroupObject(source, target);
      return true;
    }

    public override void CreateFolder(FileSystemEntry entry)
    {
      this.session.ConfigurationManager.CreateFormGroupFolder(entry);
    }

    public override bool EntryExists(FileSystemEntry entry)
    {
      return this.session.ConfigurationManager.FormGroupObjectExists(entry);
    }

    public override bool EntryExistsOfAnyType(FileSystemEntry entry)
    {
      return this.session.ConfigurationManager.FormGroupObjectExists(entry);
    }

    private bool checkForPersonalContents(FileSystemEntry source)
    {
      try
      {
        foreach (FileSystemEntry formGroupXref in this.session.ConfigurationManager.GetFormGroupXRefs(source))
        {
          if (!formGroupXref.IsPublic)
            return Utils.Dialog((IWin32Window) Form.ActiveForm, "The form group '" + source.Name + "' contains one or more personal custom letters. Would you like to copy this group and have the personal custom letters removed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
      }
      catch
      {
      }
      return true;
    }
  }
}
