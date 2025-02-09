// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PrintFormGroupIFSExplorer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PrintFormGroupIFSExplorer : IFSExplorerBase
  {
    private Sessions.Session session;
    private bool noAccessCheck;

    public PrintFormGroupIFSExplorer(Sessions.Session session) => this.session = session;

    public PrintFormGroupIFSExplorer(Sessions.Session session, bool noAccessCheck)
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

    public override string GetDescription(FileSystemEntry entry)
    {
      string description = string.Concat(entry.Properties[(object) "Description"]);
      if (description != string.Empty)
        description = description.Replace("\r\n", "  ");
      return description;
    }

    public override bool CreateNew(FileSystemEntry entry)
    {
      Cursor.Current = Cursors.WaitCursor;
      FormInfo[] forms = new FormInfo[1]
      {
        new FormInfo("", OutputFormType.Description)
      };
      entry.Properties[(object) "Description"] = (object) "";
      this.session.ConfigurationManager.SaveForms(entry, forms);
      Cursor.Current = Cursors.Default;
      return true;
    }

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

    public override void OpenFile(FileSystemEntry entry, GVItem lvItem)
    {
      if (!this.EntryExists(entry))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "The template has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (PrintFormGroupDetailDialog groupDetailDialog = new PrintFormGroupDetailDialog(this.session, entry))
        {
          if (groupDetailDialog.ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
            return;
          string groupDescription = groupDetailDialog.FormGroupDescription;
          entry.Properties[(object) "Description"] = (object) groupDescription;
          try
          {
            this.session.ConfigurationManager.SaveForms(entry, groupDetailDialog.GetSelectedForms());
            if (groupDescription == null)
              return;
            string str = groupDescription.Replace("\r\n", "  ");
            lvItem.SubItems[1].Text = str;
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "Error saving form group: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
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
