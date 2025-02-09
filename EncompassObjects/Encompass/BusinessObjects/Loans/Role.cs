// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Role
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// A Role represents the business function that a user can take within a Loan.
  /// </summary>
  public class Role : SessionBoundObject, IRole
  {
    private RoleInfo role;
    private RolePersonas personas;
    private RoleUserGroups groups;

    internal Role(Session session, RoleInfo role)
      : base(session)
    {
      this.role = role;
    }

    /// <summary>Returns the ID for the Role.</summary>
    public int ID => this.role.ID;

    /// <summary>Gets the display name for the Role.</summary>
    public string Name => this.role.Name;

    /// <summary>Gets the two-character abbreviation for the Role.</summary>
    public string Abbreviation => this.role.RoleAbbr;

    /// <summary>
    /// Gets a flag indicating if the role is "protected", meaning that certain rights cannot be revoked
    /// once granted.
    /// </summary>
    public bool Protected => this.role.Protected;

    /// <summary>
    /// Returns the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" />s which are eligible for this role.
    /// </summary>
    public RolePersonas EligiblePersonas
    {
      get
      {
        lock (this)
        {
          if (this.personas == null)
            this.personas = new RolePersonas(this.Session, this.role.PersonaIDs);
        }
        return this.personas;
      }
    }

    /// <summary>
    /// Returns the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" />s which are eligible for this role.
    /// </summary>
    public RoleUserGroups EligibleGroups
    {
      get
      {
        lock (this)
        {
          if (this.groups == null)
            this.groups = new RoleUserGroups(this.Session, this.role.UserGroupIDs);
        }
        return this.groups;
      }
    }

    /// <summary>Provides a string representation of a Role.</summary>
    /// <returns>Returns the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Role.Name" /> of the Role.</returns>
    public override string ToString() => this.Name;

    /// <summary>Provides a Hash code for the Role object.</summary>
    /// <returns>Returns the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Role.ID" /> of the Role.</returns>
    public override int GetHashCode() => this.ID;

    /// <summary>
    /// Compares two Roles for equality based on their Role IDs.
    /// </summary>
    /// <param name="obj">The Role to which to compare the current Role.</param>
    /// <returns>Returns <c>true</c> if the objects represent the same Role, <c>false</c> otherwise.</returns>
    public override bool Equals(object obj) => obj is Role role && role.ID == this.ID;
  }
}
