// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.UserInfoSummary
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class UserInfoSummary : IComparable
  {
    private string userid;
    private string lastName;
    private string firstName;

    public UserInfoSummary(string userid, string lastName, string firstName)
    {
      this.userid = userid;
      this.lastName = lastName;
      this.firstName = firstName;
    }

    public UserInfoSummary(UserInfo userInfo)
    {
      this.userid = userInfo.Userid;
      this.lastName = userInfo.LastName;
      this.firstName = userInfo.FirstName;
    }

    public string UserID => this.userid;

    public string LastName => this.lastName;

    public string FirstName => this.firstName;

    public override int GetHashCode() => this.userid.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj != null && !(this.GetType() != obj.GetType()) && object.Equals((object) this.userid, (object) ((UserInfoSummary) obj).userid);
    }

    public static bool operator ==(UserInfoSummary o1, UserInfoSummary o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(UserInfoSummary o1, UserInfoSummary o2) => !(o1 == o2);

    public string FullName => this.firstName + " " + this.lastName;

    public override string ToString()
    {
      string str = string.Empty;
      if (this.firstName != null && this.firstName != string.Empty)
        str = this.firstName + " ";
      if (this.lastName != null)
        str += this.lastName;
      return str;
    }

    public int CompareTo(object obj) => this.FullName.CompareTo(((UserInfoSummary) obj).FullName);
  }
}
