// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IOrganizationList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("A1B18715-CBF7-4928-A16E-9C0830CED725")]
  public interface IOrganizationList
  {
    Organization this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(Organization value);

    bool Contains(Organization value);

    int IndexOf(Organization value);

    void Insert(int index, Organization value);

    void Remove(Organization value);

    Organization[] ToArray();

    IEnumerator GetEnumerator();
  }
}
