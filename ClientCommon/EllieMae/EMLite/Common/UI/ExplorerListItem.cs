// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ExplorerListItem
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ExplorerListItem
  {
    private GVItem listItem;
    private FileSystemEntry fsEntry;
    private string displayName = "";
    private Image displayIcon;

    internal ExplorerListItem(GVItem item, FileSystemEntry fsEntry)
    {
      this.listItem = item;
      this.fsEntry = fsEntry;
    }

    public string DisplayName
    {
      get => this.displayName;
      set
      {
        this.displayName = value;
        this.refreshItemDisplay();
      }
    }

    public Image DisplayIcon
    {
      get => this.displayIcon;
      set
      {
        this.displayIcon = value;
        this.refreshItemDisplay();
      }
    }

    public FileSystemEntry FileFolderEntry
    {
      get => this.fsEntry;
      internal set => this.fsEntry = value;
    }

    public string GetColumnText(int index)
    {
      this.validateColumnIndex(index);
      return this.listItem.SubItems[index].Text;
    }

    public void SetColumnText(int index, string text)
    {
      this.validateColumnIndex(index);
      this.listItem.SubItems[index].Text = text;
    }

    public object GetColumnSortValue(int index)
    {
      this.validateColumnIndex(index);
      return this.listItem.SubItems[index].SortValue;
    }

    public void SetColumnSortValue(int index, object sortValue)
    {
      this.validateColumnIndex(index);
      this.listItem.SubItems[index].SortValue = sortValue;
    }

    public void SetColumnContents(int index, Element e)
    {
      this.validateColumnIndex(index);
      this.listItem.SubItems[index].Value = (object) e;
    }

    public void SetColumnData(int index, string text, object sortValue)
    {
      this.validateColumnIndex(index);
      this.listItem.SubItems[index].Text = text;
      this.listItem.SubItems[index].SortValue = sortValue;
    }

    public void SetColumnData(int index, Element e, object sortValue)
    {
      this.validateColumnIndex(index);
      this.listItem.SubItems[index].Value = (object) e;
      this.listItem.SubItems[index].SortValue = sortValue;
    }

    private void refreshItemDisplay()
    {
      this.listItem.SubItems[0].Value = (object) new ObjectWithImage(this.displayName, this.displayIcon);
      this.listItem.SubItems[0].SortValue = (object) ((this.FileFolderEntry.Type == FileSystemEntry.Types.Folder ? "A" : "B") + this.displayName);
    }

    private void validateColumnIndex(int index)
    {
      if (index == 0)
        throw new ArgumentException("Use the DisplayName and DisplayIcon properties to modify the first column");
    }

    public static explicit operator FileSystemEntry(ExplorerListItem item) => item?.FileFolderEntry;
  }
}
