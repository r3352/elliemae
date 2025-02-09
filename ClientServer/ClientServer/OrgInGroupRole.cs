// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.OrgInGroupRole
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class OrgInGroupRole : OrgInGroup
  {
    private int roleId = -1;

    public OrgInGroupRole()
    {
    }

    public OrgInGroupRole(int roleId, int orgId, bool isInclusive, string orgName)
      : base(orgId, isInclusive, orgName)
    {
      this.roleId = roleId;
    }

    public override int GetHashCode() => this.OrgID.GetHashCode() ^ this.roleId;

    public override bool Equals(object obj)
    {
      return obj != null && object.Equals((object) this.OrgID, (object) ((OrgInGroup) obj).OrgID);
    }

    public static bool operator ==(OrgInGroupRole o1, OrgInGroupRole o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(OrgInGroupRole o1, OrgInGroupRole o2) => !(o1 == o2);

    public int RoleID
    {
      get => this.roleId;
      set => this.roleId = value;
    }
  }
}
