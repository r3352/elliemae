// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IRoleAccessList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("3681E525-F5C1-4046-8ACA-9465A5F99CB5")]
  public interface IRoleAccessList
  {
    int Count { get; }

    Role this[int index] { get; }

    void Add(Role roleToAdd);

    void Remove(Role roleToRemove);

    void Clear();

    IEnumerator GetEnumerator();
  }
}
