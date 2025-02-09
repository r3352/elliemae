// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.CursorGVDataProvider
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.UI;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class CursorGVDataProvider : IGVDataProvider, IDisposable
  {
    private ICursor cursor;
    private int chunkSize;
    private int cursorSize;
    private BitArray stateFlags;
    private ArrayList data;

    public event PopulateGVItemEventHandler PopulateGVItem;

    public CursorGVDataProvider(ICursor cursor)
      : this(cursor, (PopulateGVItemEventHandler) null)
    {
    }

    public CursorGVDataProvider(ICursor cursor, PopulateGVItemEventHandler populateItemHandler)
      : this(cursor, 100, populateItemHandler)
    {
    }

    public CursorGVDataProvider(ICursor cursor, int chunkSize)
      : this(cursor, chunkSize, (PopulateGVItemEventHandler) null)
    {
    }

    public CursorGVDataProvider(
      ICursor cursor,
      int chunkSize,
      PopulateGVItemEventHandler populateItemHandler)
    {
      this.cursor = cursor;
      this.chunkSize = chunkSize;
      this.cursorSize = cursor.GetItemCount();
      if (populateItemHandler != null)
        this.PopulateGVItem += populateItemHandler;
      this.stateFlags = new BitArray(this.getChunkCount(), false);
      this.data = new ArrayList((ICollection) new object[this.cursorSize]);
    }

    public void Dispose()
    {
      if (this.cursor != null)
      {
        this.cursor.Dispose();
        this.cursor = (ICursor) null;
      }
      this.data = (ArrayList) null;
      this.stateFlags = (BitArray) null;
    }

    public int ItemCount => this.cursorSize;

    public void PopulateItem(GVItem item)
    {
      if (this.cursor == null)
        throw new ObjectDisposedException("GVCursorDataProvider");
      int chunkIndex = this.getChunkIndex(item.Index);
      if (!this.stateFlags[chunkIndex])
        this.loadChunkIntoCache(chunkIndex);
      this.OnPopulateGVItem(new PopulateGVItemEventArgs(item, this.data[item.Index]));
      this.data[item.Index] = (object) null;
    }

    protected void OnPopulateGVItem(PopulateGVItemEventArgs e)
    {
      if (this.PopulateGVItem == null)
        return;
      this.PopulateGVItem((object) this, e);
    }

    private void loadChunkIntoCache(int chunkIndex)
    {
      Range<int> chunkRange = this.getChunkRange(chunkIndex);
      object[] items = this.cursor.GetItems(chunkRange.Minimum, chunkRange.Maximum - chunkRange.Minimum + 1, false);
      this.data.SetRange(chunkRange.Minimum, (ICollection) new ArrayList((ICollection) items));
      this.stateFlags[chunkIndex] = true;
    }

    private int getChunkIndex(int itemIndex) => itemIndex / this.chunkSize;

    private Range<int> getChunkRange(int chunkIndex)
    {
      return new Range<int>(chunkIndex * this.chunkSize, Math.Min((chunkIndex + 1) * this.chunkSize - 1, this.cursorSize - 1));
    }

    private int getChunkCount()
    {
      int chunkCount = this.cursorSize / this.chunkSize;
      if (this.cursorSize % this.chunkSize != 0)
        ++chunkCount;
      return chunkCount;
    }
  }
}
