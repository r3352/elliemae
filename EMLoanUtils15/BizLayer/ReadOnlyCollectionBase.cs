// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.ReadOnlyCollectionBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer.Core;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace EllieMae.EMLite.BizLayer
{
  [Serializable]
  public abstract class ReadOnlyCollectionBase : SortableCollectionBase, ICloneable
  {
    protected bool locked = true;

    public ReadOnlyCollectionBase()
    {
      this.AllowEdit = false;
      this.AllowNew = false;
      this.AllowRemove = false;
    }

    protected override void OnInsert(int index, object val)
    {
      if (!this.ActivelySorting && this.locked)
        throw new NotSupportedException("NoInsertReadOnlyException");
    }

    protected override void OnRemove(int index, object val)
    {
      if (!this.ActivelySorting && this.locked)
        throw new NotSupportedException("NoRemoveReadOnlyException");
    }

    protected override void OnClear()
    {
      if (!this.ActivelySorting && this.locked)
        throw new NotSupportedException("NoClearReadOnlyException");
    }

    protected override void OnSet(int index, object oldValue, object newValue)
    {
      if (!this.ActivelySorting && this.locked)
        throw new NotSupportedException("NoChangeReadOnlyException");
    }

    public object Clone()
    {
      MemoryStream serializationStream = new MemoryStream();
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      binaryFormatter.Serialize((Stream) serializationStream, (object) this);
      serializationStream.Position = 0L;
      return binaryFormatter.Deserialize((Stream) serializationStream);
    }
  }
}
