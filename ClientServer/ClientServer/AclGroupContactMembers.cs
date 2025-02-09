// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AclGroupContactMembers
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class AclGroupContactMembers
  {
    private int groupId;
    private OrgInGroupContact[] orgs;
    private UserInGroupContact[] users;

    public int GroupID
    {
      get => this.groupId;
      set => this.groupId = value;
    }

    public AclGroupContactMembers()
    {
      this.groupId = -1;
      this.orgs = (OrgInGroupContact[]) null;
      this.users = (UserInGroupContact[]) null;
    }

    public AclGroupContactMembers(int groupId)
    {
      this.groupId = groupId;
      this.orgs = (OrgInGroupContact[]) null;
      this.users = (UserInGroupContact[]) null;
    }

    public OrgInGroupContact[] OrgMembers
    {
      get => this.orgs;
      set => this.orgs = value;
    }

    public UserInGroupContact[] UserMembers
    {
      get => this.users;
      set => this.users = value;
    }
  }
}
