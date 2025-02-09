// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.Core.SortableCollectionBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.BizLayer.Core
{
  [Serializable]
  public class SortableCollectionBase : BindableCollectionBase
  {
    [NotUndoable]
    private bool _isSorted;
    [NotUndoable]
    [NonSerialized]
    private PropertyDescriptor _sortProperty;
    [NotUndoable]
    private string _sortPropertyName = string.Empty;
    [NotUndoable]
    private ListSortDirection _listSortDirection;
    [NotUndoable]
    private ArrayList _unsortedList;
    [NotUndoable]
    private bool _activelySorting;

    protected bool ActivelySorting => this._activelySorting;

    protected override bool IBindingList_IsSorted => this._isSorted;

    protected override PropertyDescriptor IBindingList_SortProperty
    {
      get
      {
        if (this._sortProperty == null)
        {
          if (this._sortPropertyName.Length > 0)
          {
            try
            {
              Type componentType;
              if (this.List.Count > 0)
              {
                componentType = this.List[0].GetType();
              }
              else
              {
                try
                {
                  Type[] types = new Type[1]{ typeof (int) };
                  componentType = this.GetType().GetProperty("Item", types).PropertyType;
                }
                catch
                {
                  componentType = typeof (object);
                }
              }
              this._sortProperty = TypeDescriptor.GetProperties(componentType)[this._sortPropertyName];
            }
            catch
            {
              this._sortProperty = (PropertyDescriptor) null;
            }
          }
        }
        return this._sortProperty;
      }
    }

    protected override ListSortDirection IBindingList_SortDirection => this._listSortDirection;

    protected override void IBindingList_ApplySort(
      PropertyDescriptor property,
      ListSortDirection direction)
    {
      if (!this.AllowSort)
        throw new NotSupportedException("Sorting is not supported by this collection");
      this._sortProperty = property;
      this._sortPropertyName = this._sortProperty.Name;
      this._listSortDirection = direction;
      if (!this._isSorted && this.List.Count > 0)
      {
        this._unsortedList = new ArrayList();
        foreach (object obj in (IEnumerable) this.List)
          this._unsortedList.Add(obj);
      }
      if (this.List.Count > 1)
      {
        try
        {
          this._activelySorting = true;
          ArrayList arrayList = new ArrayList();
          for (int index = 0; index < this.List.Count; ++index)
            arrayList.Add((object) new SortableCollectionBase.SortData(SortableCollectionBase.CallByName(this.List[index], this._sortPropertyName, SortableCollectionBase.CallType.Get), this.List[index]));
          arrayList.Sort((IComparer) new SortableCollectionBase.SortDataCompare());
          this.List.Clear();
          if (direction == ListSortDirection.Ascending)
          {
            foreach (SortableCollectionBase.SortData sortData in arrayList)
              this.List.Add(sortData.Value);
          }
          else
          {
            for (int index = arrayList.Count - 1; index >= 0; --index)
              this.List.Add(((SortableCollectionBase.SortData) arrayList[index]).Value);
          }
          this._isSorted = true;
        }
        catch
        {
          this.IBindingList_RemoveSort();
        }
        finally
        {
          this._activelySorting = false;
        }
      }
      else
      {
        if (this.List.Count != 1)
          return;
        this._isSorted = true;
      }
    }

    private static bool IsNumeric(object value)
    {
      return double.TryParse(value.ToString(), NumberStyles.Any, (IFormatProvider) NumberFormatInfo.InvariantInfo, out double _);
    }

    private static object CallByName(
      object target,
      string methodName,
      SortableCollectionBase.CallType callType,
      params object[] args)
    {
      switch (callType)
      {
        case SortableCollectionBase.CallType.Get:
          return target.GetType().GetProperty(methodName).GetValue(target, args);
        case SortableCollectionBase.CallType.Let:
        case SortableCollectionBase.CallType.Set:
          PropertyInfo property = target.GetType().GetProperty(methodName);
          object[] index = (object[]) null;
          args.CopyTo((Array) index, 1);
          property.SetValue(target, args[0], index);
          return (object) null;
        case SortableCollectionBase.CallType.Method:
          return target.GetType().GetMethod(methodName).Invoke(target, args);
        default:
          return (object) null;
      }
    }

    protected override void IBindingList_RemoveSort()
    {
      if (!this.AllowSort)
        throw new NotSupportedException("Sorting is not supported by this collection");
      if (!this._isSorted)
        return;
      this._activelySorting = true;
      this.List.Clear();
      foreach (object unsorted in this._unsortedList)
        this.List.Add(unsorted);
      this._unsortedList = (ArrayList) null;
      this._isSorted = false;
      this._sortProperty = (PropertyDescriptor) null;
      this._sortPropertyName = string.Empty;
      this._listSortDirection = ListSortDirection.Ascending;
      this._activelySorting = false;
    }

    protected override void OnInsertComplete(int index, object value)
    {
      if (this._isSorted && !this.ActivelySorting)
        this._unsortedList.Add(value);
      base.OnInsertComplete(index, value);
    }

    protected override void OnClearComplete()
    {
      if (this._isSorted && !this.ActivelySorting)
        this._unsortedList.Clear();
      base.OnClearComplete();
    }

    protected override void OnRemoveComplete(int index, object value)
    {
      if (this._isSorted && !this.ActivelySorting)
        this._unsortedList.Remove(value);
      base.OnRemoveComplete(index, value);
    }

    protected override int IBindingList_Find(PropertyDescriptor property, object key)
    {
      if (!this.AllowFind)
        throw new NotSupportedException("Searching is not supported by this collection");
      string name = property.Name;
      for (int index = 0; index < this.List.Count; ++index)
      {
        if (SortableCollectionBase.CallByName(this.List[index], name, SortableCollectionBase.CallType.Get).Equals(key))
          return index;
      }
      return -1;
    }

    private struct SortData(object key, object value)
    {
      private object _key = key;
      private object _value = value;

      public object Value => this._value;

      public object Key
      {
        get
        {
          return SortableCollectionBase.IsNumeric(this._key) || this._key is string ? this._key : (object) this._key.ToString();
        }
      }
    }

    private class SortDataCompare : IComparer
    {
      public int Compare(object x, object y)
      {
        SortableCollectionBase.SortData sortData1 = (SortableCollectionBase.SortData) x;
        SortableCollectionBase.SortData sortData2 = (SortableCollectionBase.SortData) y;
        return Comparer.Default.Compare(sortData1.Key, sortData2.Key);
      }
    }

    private enum CallType
    {
      Get,
      Let,
      Method,
      Set,
    }
  }
}
