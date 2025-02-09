// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.Core.BindableCollectionBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.Collections;
using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.BizLayer.Core
{
  [Serializable]
  public abstract class BindableCollectionBase : 
    CollectionBase,
    IBindingList,
    IList,
    ICollection,
    IEnumerable
  {
    protected bool AllowNew;
    protected bool AllowEdit;
    protected bool AllowRemove;
    protected bool AllowSort;
    protected bool AllowFind;
    [NonSerialized]
    private ListChangedEventHandler _nonSerializableHandlers;
    private ListChangedEventHandler _serializableHandlers;

    public event ListChangedEventHandler ListChanged
    {
      add
      {
        if (value.Method.IsPublic && (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
          this._serializableHandlers += value;
        else
          this._nonSerializableHandlers += value;
      }
      remove
      {
        if (value.Method.IsPublic && (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
          this._serializableHandlers -= value;
        else
          this._nonSerializableHandlers -= value;
      }
    }

    protected virtual void OnListChanged(ListChangedEventArgs e)
    {
      if (this._nonSerializableHandlers != null)
        this._nonSerializableHandlers((object) this, e);
      if (this._serializableHandlers == null)
        return;
      this._serializableHandlers((object) this, e);
    }

    protected override void OnInsertComplete(int index, object value)
    {
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
    }

    protected override void OnClearComplete()
    {
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
    }

    protected override void OnRemoveComplete(int index, object value)
    {
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
    }

    protected override void OnSetComplete(int index, object oldValue, object newValue)
    {
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
    }

    object IBindingList.AddNew()
    {
      if (this.AllowNew)
        return this.OnAddNew();
      throw new InvalidOperationException("Adding items not allowed");
    }

    bool IBindingList.AllowEdit => this.AllowEdit;

    bool IBindingList.AllowNew => this.AllowNew;

    bool IBindingList.AllowRemove => this.AllowRemove;

    bool IBindingList.SupportsSearching => this.AllowFind;

    bool IBindingList.SupportsSorting => this.AllowSort;

    bool IBindingList.SupportsChangeNotification => true;

    int IBindingList.Find(PropertyDescriptor property, object key)
    {
      return this.IBindingList_Find(property, key);
    }

    void IBindingList.AddIndex(PropertyDescriptor property)
    {
    }

    void IBindingList.RemoveIndex(PropertyDescriptor property)
    {
    }

    void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
    {
      this.IBindingList_ApplySort(property, direction);
    }

    void IBindingList.RemoveSort() => this.IBindingList_RemoveSort();

    bool IBindingList.IsSorted => this.IBindingList_IsSorted;

    ListSortDirection IBindingList.SortDirection => this.IBindingList_SortDirection;

    PropertyDescriptor IBindingList.SortProperty => this.IBindingList_SortProperty;

    protected virtual object OnAddNew() => (object) null;

    protected virtual int IBindingList_Find(PropertyDescriptor property, object key) => -1;

    protected virtual bool IBindingList_IsSorted => false;

    protected virtual PropertyDescriptor IBindingList_SortProperty => (PropertyDescriptor) null;

    protected virtual ListSortDirection IBindingList_SortDirection => ListSortDirection.Ascending;

    protected virtual void IBindingList_ApplySort(
      PropertyDescriptor property,
      ListSortDirection direction)
    {
    }

    protected virtual void IBindingList_RemoveSort()
    {
    }
  }
}
