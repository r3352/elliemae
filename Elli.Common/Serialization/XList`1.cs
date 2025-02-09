// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.XList`1
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using Elli.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Elli.Common.Serialization
{
  public class XList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IXSerializable where T : new()
  {
    private string serializedXml;
    private List<T> baseList;
    private string itemName;

    public XList()
    {
      Type type = typeof (T);
      this.baseList = new List<T>();
      this.itemName = type.Name;
    }

    public int IndexOf(T item)
    {
      this.parseList();
      return this.baseList.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
      this.parseList();
      this.baseList.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
      this.parseList();
      this.baseList.RemoveAt(index);
    }

    public T this[int index]
    {
      get
      {
        this.parseList();
        return this.baseList[index];
      }
      set
      {
        this.parseList();
        this.baseList[index] = value;
      }
    }

    public void Add(T item)
    {
      this.parseList();
      this.baseList.Add(item);
    }

    public void Clear()
    {
      this.serializedXml = (string) null;
      this.baseList = new List<T>();
    }

    public bool Contains(T item)
    {
      this.parseList();
      return this.baseList.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      this.parseList();
      this.baseList.CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get
      {
        this.parseList();
        return this.baseList.Count;
      }
    }

    public bool IsReadOnly => false;

    public bool Remove(T item)
    {
      this.parseList();
      return this.baseList.Remove(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
      this.parseList();
      return (IEnumerator<T>) this.baseList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void ToXml(XElement element, string context = null)
    {
      if (this.serializedXml != null)
      {
        foreach (XElement element1 in XElement.Parse(this.serializedXml).Elements())
          element.Add((object) new XElement(element1));
      }
      else
        ListSerializer<T>.Serialize((IList<T>) this, element, this.itemName);
    }

    public XElement ToXml(string groupName)
    {
      XElement element = new XElement((XName) groupName);
      this.ToXml(element, (string) null);
      return element;
    }

    public string ToXmlString(string groupName, bool nullIfEmpty = false)
    {
      if (this.serializedXml != null)
        return this.serializedXml;
      return this.baseList.Count == 0 & nullIfEmpty ? (string) null : this.ToXml(groupName).ToString(false);
    }

    public void FromXml(XElement element, string context = null)
    {
      this.serializedXml = element.ToString();
      this.baseList = (List<T>) null;
    }

    public void FromXmlString(string xmlValue)
    {
      this.serializedXml = xmlValue;
      if (this.serializedXml == null)
        this.baseList = new List<T>();
      else
        this.baseList = (List<T>) null;
    }

    private void parseList()
    {
      if (this.serializedXml == null)
        return;
      this.baseList = new List<T>();
      ListSerializer<T>.Deserialize((IList<T>) this.baseList, XElement.Parse(this.serializedXml));
    }
  }
}
