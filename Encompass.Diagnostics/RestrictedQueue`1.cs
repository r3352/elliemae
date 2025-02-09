// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.RestrictedQueue`1
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Encompass.Diagnostics
{
  public class RestrictedQueue<T> : IEnumerable<T>, IEnumerable
  {
    private Queue<T> _queue;
    private int _size;

    public RestrictedQueue(int size)
    {
      this._queue = size > 0 ? new Queue<T>(size) : new Queue<T>();
      this._size = size;
    }

    public void Enqueue(T item)
    {
      if (this._size > 0 && this._queue.Count >= this._size)
      {
        this._queue.Dequeue();
        this._queue.Enqueue(item);
      }
      else
        this._queue.Enqueue(item);
    }

    public T Dequeue() => this._queue.Dequeue();

    public T Peek() => this._queue.Peek();

    public void Clear() => this._queue.Clear();

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._queue.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._queue.GetEnumerator();

    public int Count => this._queue.Count;
  }
}
