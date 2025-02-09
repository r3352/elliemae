// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.UserInGroupLoan
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class UserInGroupLoan
  {
    private string userId;
    private AclResourceAccess access;

    public UserInGroupLoan()
    {
    }

    public UserInGroupLoan(string userId, AclResourceAccess access)
    {
      this.userId = userId;
      this.access = access;
    }

    public override int GetHashCode() => this.userId.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj != null && !(this.GetType() != obj.GetType()) && object.Equals((object) this.userId, (object) ((UserInGroupLoan) obj).userId);
    }

    public static bool operator ==(UserInGroupLoan o1, UserInGroupLoan o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(UserInGroupLoan o1, UserInGroupLoan o2) => !(o1 == o2);

    public string UserID
    {
      get => this.userId;
      set => this.userId = value;
    }

    public AclResourceAccess Access
    {
      get => this.access;
      set => this.access = value;
    }
  }
}
