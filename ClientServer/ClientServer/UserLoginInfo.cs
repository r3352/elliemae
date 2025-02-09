// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.UserLoginInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class UserLoginInfo : UserInfoSummary
  {
    private string email;
    private string phone;
    private string encVersion;
    private string userType;

    public UserLoginInfo(
      string userId,
      string lastName,
      string firstName,
      string email,
      string phone,
      string encVersion,
      string userType)
      : base(userId, lastName, firstName)
    {
      this.email = email;
      this.phone = phone;
      this.encVersion = encVersion;
      this.userType = userType;
    }

    public UserLoginInfo(UserInfo userInfo)
      : base(userInfo)
    {
      this.email = userInfo.Email;
      this.phone = userInfo.Phone;
      this.encVersion = userInfo.EncompassVersion;
      this.userType = userInfo.UserType;
    }

    public string Email => this.email;

    public string Phone => this.phone;

    public string EncompassVersion => this.encVersion;

    public string UserType => this.userType;
  }
}
