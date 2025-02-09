// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Classes.UserGroupMemberUser
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Classes
{
  [Serializable]
  public class UserGroupMemberUser
  {
    public string UserId;
    public string FirstName;
    public string MiddleName;
    public string LastName;
    public string SuffixName;
    public UserInfo.UserStatusEnum Login;
    public UserInfo.UserStatusEnum Status;
    public string UserType;
  }
}
