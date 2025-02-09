// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.RoundRobinList`1
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System.Collections.Generic;

#nullable disable
namespace Elli.MessageQueues
{
  internal class RoundRobinList<T> : IRoundRobinList<T>
  {
    private volatile IList<T> _list;
    private volatile int _index;

    public RoundRobinList(IEnumerable<T> list) => this._list = (IList<T>) new List<T>(list);

    public RoundRobinList() => this._list = (IList<T>) new List<T>();

    public IEnumerable<T> All => (IEnumerable<T>) this._list;

    public void ClearAll() => this._list.Clear();

    public void Add(T item)
    {
      lock (this._list)
        this._list.Add(item);
    }

    public T Current
    {
      get
      {
        lock (this._list)
          return this._list[this._index];
      }
    }

    public T GetNext()
    {
      if (this._list.Count == 0)
        return default (T);
      lock (this._list)
      {
        ++this._index;
        if (this._index >= this._list.Count)
          this._index = 0;
        return this._list[this._index];
      }
    }
  }
}
