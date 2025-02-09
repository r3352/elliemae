// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.RoleAccessList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class RoleAccessList : IRoleAccessList, IEnumerable
  {
    private Loan loan;
    private DocumentLog docItem;

    internal RoleAccessList(Loan loan, DocumentLog docItem)
    {
      this.loan = loan;
      this.docItem = docItem;
    }

    public int Count => this.docItem.AllowedRoles.Length;

    public Role this[int index]
    {
      get => this.loan.Session.Loans.Roles.GetRoleByID(this.docItem.AllowedRoles[index]);
    }

    public void Add(Role roleToAdd) => this.docItem.GrantAccess(roleToAdd.ID);

    public void Remove(Role roleToRemove) => this.docItem.RemoveAccess(roleToRemove.ID);

    public void Clear()
    {
      foreach (Role roleToRemove in this)
        this.Remove(roleToRemove);
    }

    public IEnumerator GetEnumerator()
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < this.Count; ++index)
        arrayList.Add((object) this[index]);
      return arrayList.GetEnumerator();
    }
  }
}
