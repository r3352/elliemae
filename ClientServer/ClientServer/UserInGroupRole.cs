// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.UserInGroupRole
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class UserInGroupRole
  {
    private string userId;
    private int roleId = -1;

    public UserInGroupRole()
    {
    }

    public UserInGroupRole(string userId, int roleId)
    {
      this.userId = userId;
      this.roleId = roleId;
    }

    public override int GetHashCode() => this.userId.GetHashCode();

    public override bool Equals(object obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      UserInGroupRole userInGroupRole = (UserInGroupRole) obj;
      return object.Equals((object) this.userId, (object) userInGroupRole.userId) && object.Equals((object) this.roleId, (object) userInGroupRole.roleId);
    }

    public static bool operator ==(UserInGroupRole o1, UserInGroupRole o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(UserInGroupRole o1, UserInGroupRole o2) => !(o1 == o2);

    public string UserID
    {
      get => this.userId;
      set => this.userId = value;
    }

    public int RoleID
    {
      get => this.roleId;
      set => this.roleId = value;
    }
  }
}
