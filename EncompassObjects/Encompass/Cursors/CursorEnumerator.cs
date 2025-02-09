// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Cursors.CursorEnumerator
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Cursors
{
  /// <summary>Used for fast enumeration over a Cursor object.</summary>
  internal class CursorEnumerator : IEnumerator
  {
    private Cursor cursor;
    private int chunkSize = -1;
    private ListBase chunkData;
    private int itemIndex = -1;
    private int itemCount = -1;
    private int chunkCount = -1;
    private int lastChunkSize = -1;

    public CursorEnumerator(Cursor cursor)
      : this(cursor, 100)
    {
    }

    public CursorEnumerator(Cursor cursor, int chunkSize)
    {
      this.cursor = cursor;
      this.chunkSize = chunkSize;
      this.itemCount = cursor.Count;
      this.lastChunkSize = this.itemCount % this.chunkSize;
      if (this.lastChunkSize == 0)
      {
        this.chunkCount = this.itemCount / this.chunkSize;
        this.lastChunkSize = this.chunkSize;
      }
      else
        this.chunkCount = this.itemCount / this.chunkSize + 1;
    }

    public void Reset()
    {
      this.itemIndex = -1;
      this.chunkData = (ListBase) null;
    }

    public object Current
    {
      get
      {
        return this.itemIndex < 0 || this.itemIndex >= this.itemCount ? (object) null : this.chunkData.GetItemAt(this.itemIndex % this.chunkSize);
      }
    }

    public bool MoveNext()
    {
      ++this.itemIndex;
      if (this.itemIndex >= this.itemCount)
        return false;
      if (this.itemIndex % this.chunkSize == 0)
        this.chunkData = this.itemIndex / this.chunkSize != this.chunkCount - 1 ? this.cursor.GetItems(this.itemIndex, this.chunkSize) : this.cursor.GetItems(this.itemIndex, this.lastChunkSize);
      return true;
    }
  }
}
