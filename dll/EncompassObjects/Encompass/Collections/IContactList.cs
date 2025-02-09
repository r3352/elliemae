// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IContactList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("D20473F0-D772-4225-A1CD-83DEDC76D5CE")]
  public interface IContactList
  {
    Contact this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(Contact value);

    bool Contains(Contact value);

    int IndexOf(Contact value);

    void Insert(int index, Contact value);

    void Remove(Contact value);

    Contact[] ToArray();

    IEnumerator GetEnumerator();
  }
}
