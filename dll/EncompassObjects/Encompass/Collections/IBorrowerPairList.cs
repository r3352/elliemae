// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IBorrowerPairList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("670703E6-752E-4261-B0EB-3D0DA7FEE5BE")]
  public interface IBorrowerPairList
  {
    BorrowerPair this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(BorrowerPair value);

    bool Contains(BorrowerPair value);

    int IndexOf(BorrowerPair value);

    void Insert(int index, BorrowerPair value);

    void Remove(BorrowerPair value);

    BorrowerPair[] ToArray();

    IEnumerator GetEnumerator();
  }
}
