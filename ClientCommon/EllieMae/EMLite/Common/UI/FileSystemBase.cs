// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.FileSystemBase
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public abstract class FileSystemBase : IFileSystem
  {
    public abstract string FileEntryDisplayName { get; }

    public abstract string RootObjectDisplayName { get; }

    public virtual string DefaultExtension => "";

    public virtual bool DisplayExtensions => true;

    public virtual int MaxFileNameLength => 260;

    public virtual bool AllowPublicAccess => true;

    public virtual bool AllowPrivateAccess => true;

    public virtual bool IsActionSupported(FileFolderAction action) => true;

    public virtual void ConfigureExplorer(FileSystemExplorer explorer)
    {
    }

    public virtual void CustomizeListItem(ExplorerListItem listItem)
    {
    }

    public virtual string GetCustomDisplayName(FileSystemEntry entry) => (string) null;

    public virtual Image GetCustomDisplayIcon(FileSystemEntry entry) => (Image) null;

    public abstract FileSystemEntry[] GetFileSystemEntries(FileSystemEntry parentFolder);

    public abstract FileSystemEntry GetPropertiesAndRights(FileSystemEntry fsEntry);

    public abstract bool EntryExists(FileSystemEntry entry);

    public abstract bool EntryExistsOfAnyType(FileSystemEntry entry);

    public virtual bool CreateFile(IWin32Window parentWindow, FileSystemEntry fileEntry)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public virtual bool CreateFolder(IWin32Window parentWindow, FileSystemEntry entry)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public virtual bool OpenFile(IWin32Window parentWindow, FileSystemEntry fileEntry)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public virtual bool DeleteEntry(IWin32Window parentWindow, FileSystemEntry entry)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public virtual bool MoveEntry(
      IWin32Window parentWindow,
      FileSystemEntry sourceEntry,
      FileSystemEntry targetEntry)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public virtual bool CopyEntry(
      IWin32Window parentWindow,
      FileSystemEntry sourceEntry,
      FileSystemEntry targetEntry)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public virtual bool Import(IWin32Window parentWindow, FileSystemEntry fileEntry)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public virtual bool Export(IWin32Window parentWindow, FileSystemEntry[] fsEntryList)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public virtual bool Deploy(IWin32Window parentWindow, FileSystemEntry fileEntry)
    {
      throw new Exception("The method or operation is not implemented.");
    }
  }
}
