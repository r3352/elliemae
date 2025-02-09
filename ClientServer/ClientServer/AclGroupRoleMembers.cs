// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AclGroupRoleMembers
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class AclGroupRoleMembers
  {
    private int groupId;
    private int roleId;
    private OrgInGroupRole[] orgs;
    private UserInGroupRole[] users;

    public int GroupID
    {
      get => this.groupId;
      set => this.groupId = value;
    }

    public int RoleID
    {
      get => this.roleId;
      set => this.roleId = value;
    }

    public AclGroupRoleMembers()
    {
      this.groupId = -1;
      this.roleId = -1;
      this.orgs = (OrgInGroupRole[]) null;
      this.users = (UserInGroupRole[]) null;
    }

    public AclGroupRoleMembers(int groupId, int roleId)
    {
      this.groupId = groupId;
      this.roleId = roleId;
      this.orgs = (OrgInGroupRole[]) null;
      this.users = (UserInGroupRole[]) null;
    }

    public OrgInGroupRole[] OrgMembers
    {
      get => this.orgs;
      set => this.orgs = value;
    }

    public UserInGroupRole[] UserMembers
    {
      get => this.users;
      set => this.users = value;
    }
  }
}
