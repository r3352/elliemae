// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Collections.ChunkedList`1
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Collections
{
  public class ChunkedList<T>
  {
    private T[] items;
    private int chunkSize;
    private int nextIndex;

    public ChunkedList(T[] items, int chunkSize)
    {
      this.items = items;
      this.chunkSize = chunkSize;
    }

    public void Reset() => this.nextIndex = 0;

    public T[] Next()
    {
      if (this.nextIndex >= this.items.Length)
        return (T[]) null;
      int length = Math.Min(this.chunkSize, this.items.Length - this.nextIndex);
      T[] destinationArray = new T[length];
      Array.Copy((Array) this.items, this.nextIndex, (Array) destinationArray, 0, length);
      this.nextIndex += length;
      return destinationArray;
    }
  }
}
