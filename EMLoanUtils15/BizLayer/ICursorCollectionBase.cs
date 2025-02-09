// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.ICursorCollectionBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.EMLite.BizLayer
{
  [Serializable]
  public abstract class ICursorCollectionBase : ReadOnlyCollectionBase, ICursor, IDisposable
  {
    [NotUndoable]
    [NonSerialized]
    protected ICursor iCursor;

    int ICursor.GetItemCount()
    {
      this.OpenCursor();
      return this.iCursor.GetItemCount();
    }

    int ICursor.GetItemCount(int sqlRead)
    {
      this.OpenCursor();
      return this.iCursor.GetItemCount(sqlRead);
    }

    object ICursor.GetItem(int index, bool isExternalOrganization)
    {
      this.OpenCursor();
      return this.iCursor.GetItem(index, isExternalOrganization);
    }

    object[] ICursor.GetItems(int startIndex, int count, bool isExternalOrganization)
    {
      this.OpenCursor();
      object[] items = this.iCursor.GetItems(startIndex, count, isExternalOrganization);
      this.SyncCursor(items, count);
      return items;
    }

    object[] ICursor.GetItems(int startIndex, int count, bool isExternalOrganization, int sqlRead)
    {
      this.OpenCursor();
      object[] items = this.iCursor.GetItems(startIndex, count, isExternalOrganization, sqlRead);
      this.SyncCursor(items, count);
      return items;
    }

    object ICursor.GetItem(int index)
    {
      this.OpenCursor();
      return this.iCursor.GetItem(index, false);
    }

    object[] ICursor.GetItems(int startIndex, int count)
    {
      this.OpenCursor();
      object[] items = this.iCursor.GetItems(startIndex, count, false);
      this.SyncCursor(items, count);
      return items;
    }

    protected abstract void OpenCursor();

    protected abstract void SyncCursor(object[] items, int count);

    public void Dispose()
    {
      if (this.iCursor != null)
        this.iCursor.Dispose();
      this.iCursor = (ICursor) null;
    }
  }
}
