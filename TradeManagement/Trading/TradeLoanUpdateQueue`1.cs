// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeLoanUpdateQueue`1
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  internal class TradeLoanUpdateQueue<T>
  {
    private LinkedList<T> list = new LinkedList<T>();

    public void Enqueue(T t) => this.list.AddLast(t);

    public T Dequeue()
    {
      T obj = this.list.First.Value;
      this.list.RemoveFirst();
      return obj;
    }

    public T Peek() => this.list.First.Value;

    public bool Remove(T t) => this.list.Remove(t);

    public bool Contains(T t) => this.list.Contains(t);

    public LinkedListNode<T> find(T t) => this.list.Find(t);

    public int Count => this.list.Count;

    public LinkedList<T> QueueList => this.list;
  }
}
