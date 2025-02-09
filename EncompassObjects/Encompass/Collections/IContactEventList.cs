// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IContactEventList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Contacts;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for ContactEventList class.</summary>
  /// <exclude />
  [Guid("82F6E488-D23C-4e2a-8E92-8F6F042CBF25")]
  public interface IContactEventList
  {
    ContactEvent this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(ContactEvent value);

    bool Contains(ContactEvent value);

    int IndexOf(ContactEvent value);

    void Insert(int index, ContactEvent value);

    void Remove(ContactEvent value);

    ContactEvent[] ToArray();

    IEnumerator GetEnumerator();
  }
}
