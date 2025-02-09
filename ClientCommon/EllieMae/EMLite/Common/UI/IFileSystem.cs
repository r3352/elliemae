// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.IFileSystem
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public interface IFileSystem
  {
    string FileEntryDisplayName { get; }

    string RootObjectDisplayName { get; }

    int MaxFileNameLength { get; }

    string DefaultExtension { get; }

    bool DisplayExtensions { get; }

    bool AllowPublicAccess { get; }

    bool AllowPrivateAccess { get; }

    string GetCustomDisplayName(FileSystemEntry entry);

    Image GetCustomDisplayIcon(FileSystemEntry entry);

    void ConfigureExplorer(FileSystemExplorer explorer);

    void CustomizeListItem(ExplorerListItem listItem);

    bool IsActionSupported(FileFolderAction action);

    FileSystemEntry[] GetFileSystemEntries(FileSystemEntry parentFolder);

    FileSystemEntry GetPropertiesAndRights(FileSystemEntry entry);

    bool EntryExists(FileSystemEntry entry);

    bool EntryExistsOfAnyType(FileSystemEntry entry);

    bool CreateFile(IWin32Window parentWindow, FileSystemEntry entry);

    bool CreateFolder(IWin32Window parentWindow, FileSystemEntry entry);

    bool OpenFile(IWin32Window parentWindow, FileSystemEntry fileEntry);

    bool DeleteEntry(IWin32Window parentWindow, FileSystemEntry entry);

    bool MoveEntry(
      IWin32Window parentWindow,
      FileSystemEntry sourceEntry,
      FileSystemEntry targetEntry);

    bool CopyEntry(
      IWin32Window parentWindow,
      FileSystemEntry sourceEntry,
      FileSystemEntry targetEntry);

    bool Import(IWin32Window parentWindow, FileSystemEntry fileEntry);

    bool Export(IWin32Window parentWindow, FileSystemEntry[] fsEntryList);

    bool Deploy(IWin32Window parentWindow, FileSystemEntry fileEntry);
  }
}
