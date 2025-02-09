// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.RoleAccessList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Summary description for RoleAccessList.</summary>
  public class RoleAccessList : IRoleAccessList, IEnumerable
  {
    private Loan loan;
    private DocumentLog docItem;

    internal RoleAccessList(Loan loan, DocumentLog docItem)
    {
      this.loan = loan;
      this.docItem = docItem;
    }

    /// <summary>Returns the number of roles in the collection.</summary>
    public int Count => this.docItem.AllowedRoles.Length;

    /// <summary>
    /// Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> from the access list by index.
    /// </summary>
    public Role this[int index]
    {
      get => this.loan.Session.Loans.Roles.GetRoleByID(this.docItem.AllowedRoles[index]);
    }

    /// <summary>
    /// Adds a specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> to the access list.
    /// </summary>
    /// <param name="roleToAdd">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> to be added.</param>
    /// <remarks>If the specified role is already in the access list, this method will
    /// return without error.</remarks>
    public void Add(Role roleToAdd) => this.docItem.GrantAccess(roleToAdd.ID);

    /// <summary>
    /// Removes a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> from the access list.
    /// </summary>
    /// <param name="roleToRemove">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> to be removed.</param>
    /// <remarks>If the specified Role is not in the access list, this method will
    /// return without error.</remarks>
    public void Remove(Role roleToRemove) => this.docItem.RemoveAccess(roleToRemove.ID);

    /// <summary>Clears the access list.</summary>
    /// <remarks>When an access list is cleared, all roles are presumed to have access to
    /// the resource which is protects.</remarks>
    public void Clear()
    {
      foreach (Role roleToRemove in this)
        this.Remove(roleToRemove);
    }

    /// <summary>
    /// Creates an enumerator for the collection of roles in the access list.
    /// </summary>
    /// <returns>Returns an enumerator for the collection of roles.</returns>
    public IEnumerator GetEnumerator()
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < this.Count; ++index)
        arrayList.Add((object) this[index]);
      return arrayList.GetEnumerator();
    }
  }
}
