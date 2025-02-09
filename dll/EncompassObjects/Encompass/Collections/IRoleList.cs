// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IRoleList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("B21E883A-CD02-4b39-A4C5-CD535DCE4167")]
  public interface IRoleList
  {
    Role this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(Role value);

    bool Contains(Role value);

    int IndexOf(Role value);

    void Insert(int index, Role value);

    void Remove(Role value);

    Role[] ToArray();

    IEnumerator GetEnumerator();
  }
}
