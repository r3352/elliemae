// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.ListSerializer`1
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Elli.Common.Serialization
{
  public static class ListSerializer<T> where T : new()
  {
    public static void Serialize(IList<T> list, XElement groupElement, string itemName)
    {
      if (list == null)
        return;
      XmlSerializer xmlSerializer = XmlSerializer.Create(typeof (T));
      foreach (T sourceObj in (IEnumerable<T>) list)
      {
        XElement xelement = new XElement((XName) itemName);
        xmlSerializer.Serialize((object) sourceObj, xelement);
        groupElement.Add((object) xelement);
      }
    }

    public static XElement Serialize(IList<T> list, string groupName, string itemName)
    {
      if (list == null)
        return (XElement) null;
      XElement groupElement = new XElement((XName) groupName);
      ListSerializer<T>.Serialize(list, groupElement, itemName);
      return groupElement;
    }

    public static void Serialize(
      IList<T> list,
      XElement parentElement,
      string groupName,
      string itemName)
    {
      XElement content = ListSerializer<T>.Serialize(list, groupName, itemName);
      if (content == null)
        return;
      parentElement.Add((object) content);
    }

    public static IList<T> Deserialize(XElement groupElement)
    {
      if (groupElement == null)
        return (IList<T>) null;
      List<T> list = new List<T>();
      ListSerializer<T>.Deserialize((IList<T>) list, groupElement);
      return (IList<T>) list;
    }

    public static void Deserialize(IList<T> list, XElement groupElement)
    {
      if (groupElement == null)
        return;
      XmlSerializer xmlSerializer = XmlSerializer.Create(typeof (T));
      foreach (XElement element in groupElement.Elements())
      {
        T targetObj = new T();
        xmlSerializer.Deserialize((object) targetObj, element);
        list.Add(targetObj);
      }
    }
  }
}
