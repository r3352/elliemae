// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.IFSExplorerBase
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class IFSExplorerBase : Form, IFSExplorer
  {
    public virtual FileSystemEntry[] GetFileSystemEntries(FileSystemEntry parentFolder)
    {
      return (FileSystemEntry[]) null;
    }

    public virtual bool EntryExists(FileSystemEntry entry) => true;

    public virtual bool EntryExistsOfAnyType(FileSystemEntry entry) => true;

    public virtual string NewDocBaseName => string.Empty;

    public virtual string NewDocExtension => string.Empty;

    public virtual bool CreateNew(FileSystemEntry entry) => false;

    public virtual void OpenFile(FileSystemEntry fileInfo)
    {
    }

    public virtual void OpenFile(FileSystemEntry fileInfo, GVItem lvItem)
    {
    }

    public virtual void DeleteEntry(FileSystemEntry entry)
    {
      throw new NotSupportedException("The specified operation is not supported.");
    }

    public virtual void CreateFolder(FileSystemEntry entry)
    {
      throw new NotSupportedException("The specified operation is not supported.");
    }

    public virtual void MoveEntry(FileSystemEntry sourceEntry, FileSystemEntry targetEntry)
    {
      throw new NotSupportedException("The specified operation is not supported.");
    }

    public virtual void CopyEntry(FileSystemEntry sourceEntry, FileSystemEntry targetEntry)
    {
      throw new NotSupportedException("The specified operation is not supported.");
    }

    public virtual string GetDescription(FileSystemEntry entry) => "";

    public virtual string GetRESPA(FileSystemEntry entry) => "";

    public virtual void Deploy(FileSystemEntry fileEntry)
    {
      throw new NotSupportedException("The specified operation is not supported.");
    }

    public virtual void Import(FileSystemEntry fileEntry)
    {
      throw new NotSupportedException("The specified operation is not supported.");
    }

    public virtual void Export(List<FileSystemEntry> fsEntryList)
    {
      throw new NotSupportedException("The specified operation is not supported.");
    }
  }
}
