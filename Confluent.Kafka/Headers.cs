// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Headers
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>A collection of Kafka message headers.</summary>
  /// <remarks>
  ///     Message headers are supported by v0.11 brokers and above.
  /// </remarks>
  public class Headers : IEnumerable<IHeader>, IEnumerable
  {
    private readonly List<IHeader> headers = new List<IHeader>();

    /// <summary>Append a new header to the collection.</summary>
    /// <param name="key">The header key.</param>
    /// <param name="val">
    ///     The header value (possibly null). Note: A null
    ///     header value is distinct from an empty header
    ///     value (array of length 0).
    /// </param>
    public void Add(string key, byte[] val)
    {
      if (key == null)
        throw new ArgumentNullException("Kafka message header key cannot be null.");
      this.headers.Add((IHeader) new Header(key, val));
    }

    /// <summary>Append a new header to the collection.</summary>
    /// <param name="header">The header to add to the collection.</param>
    public void Add(Header header) => this.headers.Add((IHeader) header);

    /// <summary>
    ///     Get the value of the latest header with the specified key.
    /// </summary>
    /// <param name="key">The key to get the associated value of.</param>
    /// <returns>
    ///     The value of the latest element in the collection with the specified key.
    /// </returns>
    /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
    ///     The key <paramref name="key" /> was not present in the collection.
    /// </exception>
    public byte[] GetLastBytes(string key)
    {
      byte[] lastHeader;
      if (this.TryGetLastBytes(key, out lastHeader))
        return lastHeader;
      throw new KeyNotFoundException("The key " + key + " was not present in the headers collection.");
    }

    /// <summary>
    ///     Try to get the value of the latest header with the specified key.
    /// </summary>
    /// <param name="key">The key to get the associated value of.</param>
    /// <param name="lastHeader">
    ///     The value of the latest element in the collection with the
    ///     specified key, if a header with that key was present in the
    ///     collection.
    /// </param>
    /// <returns>
    ///     true if the a value with the specified key was present in
    ///     the collection, false otherwise.
    /// </returns>
    public bool TryGetLastBytes(string key, out byte[] lastHeader)
    {
      for (int index = this.headers.Count - 1; index >= 0; --index)
      {
        if (this.headers[index].Key == key)
        {
          lastHeader = this.headers[index].GetValueBytes();
          return true;
        }
      }
      lastHeader = (byte[]) null;
      return false;
    }

    /// <summary>Removes all headers for the given key.</summary>
    /// <param name="key">The key to remove all headers for</param>
    public void Remove(string key)
    {
      this.headers.RemoveAll((Predicate<IHeader>) (a => a.Key == key));
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the headers collection.
    /// </summary>
    /// <returns>
    ///     An enumerator object that can be used to iterate through the headers collection.
    /// </returns>
    public IEnumerator<IHeader> GetEnumerator()
    {
      return (IEnumerator<IHeader>) new Headers.HeadersEnumerator(this);
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the headers collection.
    /// </summary>
    /// <returns>
    ///     An enumerator object that can be used to iterate through the headers collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new Headers.HeadersEnumerator(this);

    /// <summary>Gets the header at the specified index</summary>
    /// <param key="index">
    ///     The zero-based index of the element to get.
    /// </param>
    public IHeader this[int index] => this.headers[index];

    /// <summary>The number of headers in the collection.</summary>
    public int Count => this.headers.Count;

    internal class HeadersEnumerator : IEnumerator<IHeader>, IDisposable, IEnumerator
    {
      private Headers headers;
      private int location = -1;

      public HeadersEnumerator(Headers headers) => this.headers = headers;

      public object Current => (object) ((IEnumerator<IHeader>) this).Current;

      IHeader IEnumerator<IHeader>.Current => this.headers.headers[this.location];

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
        ++this.location;
        return this.location < this.headers.headers.Count;
      }

      public void Reset() => this.location = -1;
    }
  }
}
