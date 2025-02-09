// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Role
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
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

    public int ID => ((RoleSummaryInfo) this.role).ID;

    public string Name => ((RoleSummaryInfo) this.role).Name;

    public string Abbreviation => ((RoleSummaryInfo) this.role).RoleAbbr;

    public bool Protected => ((RoleSummaryInfo) this.role).Protected;

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

    public override string ToString() => this.Name;

    public override int GetHashCode() => this.ID;

    public override bool Equals(object obj) => obj is Role role && role.ID == this.ID;
  }
}
