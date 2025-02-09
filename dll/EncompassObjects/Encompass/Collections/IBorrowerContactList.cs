// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IBorrowerContactList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("CD1E8B2D-6FD3-4082-AD4B-D136129D8B97")]
  public interface IBorrowerContactList
  {
    BorrowerContact this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(BorrowerContact value);

    bool Contains(BorrowerContact value);

    int IndexOf(BorrowerContact value);

    void Insert(int index, BorrowerContact value);

    void Remove(BorrowerContact value);

    BorrowerContact[] ToArray();

    IEnumerator GetEnumerator();
  }
}
