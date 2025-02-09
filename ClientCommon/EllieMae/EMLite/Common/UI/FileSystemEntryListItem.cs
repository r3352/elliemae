// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.FileSystemEntryListItem
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class FileSystemEntryListItem
  {
    private FileSystemEntry fsEntry;

    public FileSystemEntryListItem(FileSystemEntry e) => this.fsEntry = e;

    public FileSystemEntry Entry => this.fsEntry;

    public override string ToString() => this.fsEntry.Name;

    public override bool Equals(object obj)
    {
      return obj is FileSystemEntryListItem systemEntryListItem && systemEntryListItem.Entry.Equals((object) this.fsEntry);
    }

    public override int GetHashCode() => this.fsEntry.GetHashCode();
  }
}
