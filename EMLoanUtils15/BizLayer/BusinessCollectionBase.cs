// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.BusinessCollectionBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer.Core;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace EllieMae.EMLite.BizLayer
{
  [Serializable]
  public abstract class BusinessCollectionBase : SortableCollectionBase, ICloneable
  {
    public DeletedCollection deletedList = new DeletedCollection();
    private int _editLevel;
    [NotUndoable]
    private bool _IsChild;

    public int AddedCount
    {
      get
      {
        int addedCount = 0;
        foreach (BusinessBase businessBase in (IEnumerable) this.List)
        {
          if (businessBase.IsNew)
            ++addedCount;
        }
        return addedCount;
      }
    }

    public int UpdatedCount
    {
      get
      {
        int updatedCount = 0;
        foreach (BusinessBase businessBase in (IEnumerable) this.List)
        {
          if (!businessBase.IsNew && businessBase.IsDirty)
            ++updatedCount;
        }
        return updatedCount;
      }
    }

    public int DeletedCount => this.deletedList.Count;

    internal DeletedCollection DeletedCollection => this.deletedList;

    public bool Contains(BusinessBase item) => this.List.Contains((object) item);

    public bool ContainsDeleted(BusinessBase item)
    {
      foreach (BusinessBase deleted in (CollectionBase) this.deletedList)
      {
        if (deleted.Equals((object) item))
          return true;
      }
      return false;
    }

    public bool IsDirty
    {
      get
      {
        if (this.deletedList.Count > 0)
          return true;
        foreach (BusinessBase businessBase in (IEnumerable) this.List)
        {
          if (businessBase.IsDirty)
            return true;
        }
        return false;
      }
    }

    public bool IsValid
    {
      get
      {
        foreach (BusinessBase businessBase in (IEnumerable) this.List)
        {
          if (!businessBase.IsValid)
            return false;
        }
        return true;
      }
    }

    public void BeginEdit()
    {
      if (this.IsChild)
        throw new NotSupportedException("BeginEdit is not valid on a child object");
      this.CopyState();
    }

    public void CancelEdit()
    {
      if (this.IsChild)
        throw new NotSupportedException("CancelEdit is not valid on a child object");
      this.UndoChanges();
      foreach (BusinessBase businessBase in (IEnumerable) this.List)
        businessBase.AddBusinessRules();
      foreach (BusinessBase deleted in (CollectionBase) this.deletedList)
        deleted.AddBusinessRules();
    }

    public void ApplyEdit()
    {
      if (this.IsChild)
        throw new NotSupportedException("ApplyEdit is not valid on a child object");
      this.AcceptChanges();
    }

    internal void CopyState()
    {
      ++this._editLevel;
      foreach (UndoableBase undoableBase in (IEnumerable) this.List)
        undoableBase.CopyState();
      foreach (UndoableBase deleted in (CollectionBase) this.deletedList)
        deleted.CopyState();
    }

    internal void UndoChanges()
    {
      this._editLevel = 0 < this._editLevel ? this._editLevel - 1 : 0;
      for (int index = this.List.Count - 1; index >= 0; --index)
      {
        BusinessBase businessBase = (BusinessBase) this.List[index];
        businessBase.UndoChanges();
        if (businessBase.EditLevelAdded > this._editLevel)
          this.List.RemoveAt(index);
      }
      for (int index = this.deletedList.Count - 1; index >= 0; --index)
      {
        BusinessBase deleted = this.deletedList[index];
        deleted.UndoChanges();
        if (deleted.EditLevelAdded > this._editLevel)
          this.deletedList.RemoveAt(index);
        else if (!deleted.IsDeleted)
          this.UnDeleteChild(deleted);
      }
    }

    internal void AcceptChanges()
    {
      this._editLevel = 0 < this._editLevel ? this._editLevel - 1 : 0;
      foreach (BusinessBase businessBase in (IEnumerable) this.List)
      {
        businessBase.AcceptChanges();
        if (businessBase.EditLevelAdded > this._editLevel)
          businessBase.EditLevelAdded = this._editLevel;
      }
      for (int index = this.deletedList.Count - 1; index >= 0; --index)
      {
        BusinessBase deleted = this.deletedList[index];
        deleted.AcceptChanges();
        if (deleted.EditLevelAdded > this._editLevel)
          this.deletedList.RemoveAt(index);
      }
    }

    private void DeleteChild(BusinessBase child)
    {
      child.DeleteChild();
      this.deletedList.Add(child);
    }

    private void UnDeleteChild(BusinessBase child)
    {
      int editLevelAdded = child.EditLevelAdded;
      this.List.Add((object) child);
      child.EditLevelAdded = editLevelAdded;
      this.deletedList.Remove(child);
    }

    internal void RemoveChild(BusinessBase child) => this.List.Remove((object) child);

    protected override void OnInsert(int index, object val)
    {
      if (this.ActivelySorting)
        return;
      ((BusinessBase) val).EditLevelAdded = this._editLevel;
      ((BusinessBase) val).SetParent(this);
      base.OnInsert(index, val);
    }

    protected override void OnRemove(int index, object val)
    {
      if (this.ActivelySorting)
        return;
      this.DeleteChild((BusinessBase) val);
      base.OnRemove(index, val);
    }

    protected override void OnClear()
    {
      if (this.ActivelySorting)
        return;
      while (this.List.Count > 0)
        this.List.RemoveAt(this.List.Count - 1);
      base.OnClear();
    }

    protected bool IsChild => this._IsChild;

    protected void MarkAsChild() => this._IsChild = true;

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
