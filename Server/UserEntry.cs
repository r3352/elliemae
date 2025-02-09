// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.UserEntry
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  [Serializable]
  public class UserEntry : ICloneable
  {
    public UserInfo UserInfo;
    public UserServerInfo ServerInfo;

    public UserEntry(UserInfo userInfo, UserServerInfo ServerInfo)
    {
      this.UserInfo = userInfo;
      this.ServerInfo = ServerInfo;
    }

    public UserEntry(UserEntry source)
    {
      this.UserInfo = (UserInfo) source.UserInfo.Clone();
      this.ServerInfo = (UserServerInfo) source.ServerInfo.Clone();
    }

    public object Clone() => (object) new UserEntry(this);
  }
}
