// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IRoles
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("4E2103CB-0E1E-4970-8752-2A46506814F7")]
  public interface IRoles
  {
    int Count { get; }

    Role this[int index] { get; }

    Role FileStarter { get; }

    Role Others { get; }

    IEnumerator GetEnumerator();

    void Refresh();

    Role GetRoleByID(int roleId);

    Role GetRoleByAbbrev(string abbrev);

    Role GetRoleByName(string name);

    Role GetFixedRole(FixedRole roleType);
  }
}
