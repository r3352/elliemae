// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.UserServerInfo
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  [Serializable]
  public class UserServerInfo : ICloneable
  {
    private int failedLoginAttempts;
    private DateTime lastPasswordChangedDate;
    private DateTime? lastLockedOutDateTime;

    public UserServerInfo()
    {
      this.failedLoginAttempts = 0;
      this.lastPasswordChangedDate = DateTime.Now;
    }

    public UserServerInfo(
      DateTime lastPasswordChangedDate,
      int failedLoginAttempts,
      DateTime? lastLockedOutDateTime)
    {
      this.failedLoginAttempts = failedLoginAttempts;
      this.lastPasswordChangedDate = lastPasswordChangedDate;
      this.lastLockedOutDateTime = lastLockedOutDateTime;
    }

    public UserServerInfo(UserServerInfo source)
    {
      this.failedLoginAttempts = source.failedLoginAttempts;
      this.lastPasswordChangedDate = source.lastPasswordChangedDate;
      this.lastLockedOutDateTime = source.lastLockedOutDateTime;
    }

    public int FailedLoginAttempts => this.failedLoginAttempts;

    public void setFailedLoginAttemps(string userId, int value) => this.failedLoginAttempts = value;

    public DateTime LastPasswordChangedDate
    {
      get => this.lastPasswordChangedDate;
      set => this.lastPasswordChangedDate = value;
    }

    public DateTime? LastLockedOutDateTime
    {
      get => this.lastLockedOutDateTime;
      set => this.lastLockedOutDateTime = value;
    }

    public object Clone() => (object) new UserServerInfo(this);
  }
}
