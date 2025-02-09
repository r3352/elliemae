// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Roles
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class Roles : SessionBoundObject, IRoles, IEnumerable
  {
    private ArrayList roles;
    private Role others;
    private Role fileStarter;
    private RolesMappingInfo[] roleMaps;

    internal Roles(Session session)
      : base(session)
    {
      this.fileStarter = new Role(session, RoleInfo.FileStarter);
      this.others = new Role(session, RoleInfo.Others);
      this.Refresh();
    }

    public int Count => this.roles.Count;

    public Role this[int index] => (Role) this.roles[index];

    public Role GetRoleByID(int roleId)
    {
      foreach (Role role in this.roles)
      {
        if (role.ID == roleId)
          return role;
      }
      return this.others.ID == roleId ? this.others : (Role) null;
    }

    public Role GetRoleByAbbrev(string abbrev)
    {
      foreach (Role role in this.roles)
      {
        if (string.Compare(role.Abbreviation, abbrev, true) == 0)
          return role;
      }
      return string.Compare(this.others.Abbreviation, abbrev, true) == 0 ? this.others : (Role) null;
    }

    public Role GetRoleByName(string name)
    {
      foreach (Role role in this.roles)
      {
        if (string.Compare(role.Name, name, true) == 0)
          return role;
      }
      return string.Compare(this.others.Name, name, true) == 0 ? this.others : (Role) null;
    }

    public Role GetFixedRole(FixedRole roleType)
    {
      foreach (RolesMappingInfo roleMap in this.roleMaps)
      {
        if ((FixedRole) roleMap.RealWorldRoleID == roleType && roleMap.RoleIDList != null && roleMap.RoleIDList.Length != 0)
          return this.GetRoleByID(roleMap.RoleIDList[0]);
      }
      return (Role) null;
    }

    public Role Others => this.others;

    public Role FileStarter => this.fileStarter;

    public void Refresh()
    {
      this.roles = new ArrayList();
      this.roles.Add((object) this.fileStarter);
      foreach (RoleInfo allRoleFunction in this.WorkflowManager.GetAllRoleFunctions())
        this.roles.Add((object) new Role(this.Session, allRoleFunction));
      this.roleMaps = this.Session.SessionObjects.BpmManager.GetAllRoleMappingInfos();
    }

    public IEnumerator GetEnumerator() => this.roles.GetEnumerator();

    private IBpmManager WorkflowManager => (IBpmManager) this.Session.GetObject("BpmManager");

    internal Role Find(RoleInfo roleInfo)
    {
      foreach (Role role in this.roles)
      {
        if (role.ID == ((RoleSummaryInfo) roleInfo).RoleID)
          return role;
      }
      return (Role) null;
    }
  }
}
