// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IContactNoteList
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
  /// <summary>Interface for ContactNoteList class.</summary>
  /// <exclude />
  [Guid("DB8D8E20-D242-4928-BDD1-8849E6E94E80")]
  public interface IContactNoteList
  {
    ContactNote this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(ContactNote value);

    bool Contains(ContactNote value);

    int IndexOf(ContactNote value);

    void Insert(int index, ContactNote value);

    void Remove(ContactNote value);

    ContactNote[] ToArray();

    IEnumerator GetEnumerator();
  }
}
