// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.CursorEnumerator
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class CursorEnumerator : IEnumerator
  {
    private ICursor cursor;
    private int chunkSize = -1;
    private object[] chunkData;
    private int itemIndex = -1;
    private int itemCount = -1;
    private int chunkCount = -1;
    private int lastChunkSize = -1;
    private bool isExternalOrganization;

    public CursorEnumerator(ICursor cursor, bool isExternalOrganization)
      : this(cursor, 100, isExternalOrganization)
    {
    }

    public CursorEnumerator(ICursor cursor, int chunkSize, bool isExternalOrganization)
    {
      this.isExternalOrganization = isExternalOrganization;
      this.cursor = cursor;
      this.chunkSize = chunkSize;
      this.itemCount = cursor.GetItemCount();
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
      this.chunkData = (object[]) null;
    }

    public object Current
    {
      get
      {
        return this.itemIndex < 0 || this.itemIndex >= this.itemCount ? (object) null : this.chunkData[this.itemIndex % this.chunkSize];
      }
    }

    public bool MoveNext()
    {
      ++this.itemIndex;
      if (this.itemIndex >= this.itemCount)
        return false;
      if (this.itemIndex % this.chunkSize == 0)
        this.chunkData = this.itemIndex / this.chunkSize != this.chunkCount - 1 ? this.cursor.GetItems(this.itemIndex, this.chunkSize, this.isExternalOrganization) : this.cursor.GetItems(this.itemIndex, this.lastChunkSize, this.isExternalOrganization);
      return true;
    }
  }
}
