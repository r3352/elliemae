// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AclGroupRoleAccessLevel
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class AclGroupRoleAccessLevel
  {
    private int groupId = -1;
    private int roleId = -1;
    private AclGroupRoleAccessEnum access;
    private bool hideDisabledAccount;

    public AclGroupRoleAccessLevel()
    {
    }

    public AclGroupRoleAccessLevel(
      int groupId,
      int roleId,
      AclGroupRoleAccessEnum access,
      bool hideDisabledAccount)
    {
      this.groupId = groupId;
      this.roleId = roleId;
      this.access = access;
      this.hideDisabledAccount = hideDisabledAccount;
    }

    public override int GetHashCode() => this.groupId.GetHashCode() ^ this.roleId.GetHashCode();

    public override bool Equals(object obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      AclGroupRoleAccessLevel groupRoleAccessLevel = (AclGroupRoleAccessLevel) obj;
      return object.Equals((object) this.groupId, (object) groupRoleAccessLevel.groupId) && object.Equals((object) this.roleId, (object) groupRoleAccessLevel.roleId);
    }

    public static bool operator ==(AclGroupRoleAccessLevel o1, AclGroupRoleAccessLevel o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(AclGroupRoleAccessLevel o1, AclGroupRoleAccessLevel o2)
    {
      return !(o1 == o2);
    }

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

    public AclGroupRoleAccessEnum Access
    {
      get => this.access;
      set => this.access = value;
    }

    public bool HideDisabledAccount
    {
      get => this.hideDisabledAccount;
      set => this.hideDisabledAccount = value;
    }
  }
}
