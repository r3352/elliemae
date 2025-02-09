// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.IFSExplorer
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public interface IFSExplorer
  {
    FileSystemEntry[] GetFileSystemEntries(FileSystemEntry parentFolder);

    bool EntryExists(FileSystemEntry entry);

    bool EntryExistsOfAnyType(FileSystemEntry entry);

    string NewDocBaseName { get; }

    string NewDocExtension { get; }

    bool CreateNew(FileSystemEntry entry);

    void OpenFile(FileSystemEntry fileInfo);

    void OpenFile(FileSystemEntry fileInfo, GVItem gvItem);

    void DeleteEntry(FileSystemEntry entry);

    void CreateFolder(FileSystemEntry entry);

    void MoveEntry(FileSystemEntry sourceEntry, FileSystemEntry targetEntry);

    void CopyEntry(FileSystemEntry sourceEntry, FileSystemEntry targetEntry);

    string GetDescription(FileSystemEntry entry);

    void Import(FileSystemEntry fileEntry);
  }
}
