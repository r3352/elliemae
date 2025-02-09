// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LockInfo
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class LockInfo : ICloneable
  {
    private string guid;
    private string lockedBy;
    private string lockedByFirstName;
    private string lockedByLastName;
    private string loginSessionID = "";
    private string logonServer;
    private LoanInfo.LockReason lockedFor;
    private DateTime lockTime;
    private LockInfo.LockOwnerLoggedOn isCurrentlyLoggedOn = LockInfo.LockOwnerLoggedOn.Unknown;
    private LockInfo.ExclusiveLock exclusive;
    private bool isSessionLess;
    private string[] lockedByList;

    public LockInfo(
      string guid,
      string lockedBy,
      string lockedByFirstName,
      string lockedByLastName,
      string loginSessionID,
      string logonServer,
      LoanInfo.LockReason lockedFor,
      DateTime lockTime,
      LockInfo.LockOwnerLoggedOn isCurrentlyLoggedOn,
      LockInfo.ExclusiveLock exclusive,
      bool isSessionLess = false,
      string[] lockedByList = null)
    {
      this.guid = guid;
      this.lockedBy = lockedBy;
      this.lockedByFirstName = lockedByFirstName;
      this.lockedByLastName = lockedByLastName;
      this.loginSessionID = loginSessionID;
      this.logonServer = logonServer;
      this.lockedFor = lockedFor;
      this.lockTime = lockTime;
      this.isCurrentlyLoggedOn = isCurrentlyLoggedOn;
      this.exclusive = exclusive;
      this.isSessionLess = isSessionLess;
      this.lockedByList = lockedByList;
    }

    public LockInfo(
      string guid,
      string lockedBy,
      string lockedByFirstName,
      string lockedByLastName,
      string loginSessionID,
      string logonServer,
      LoanInfo.LockReason lockedFor,
      DateTime lockTime,
      LockInfo.ExclusiveLock exclusive,
      bool isSessionLess = false,
      string[] lockedByList = null)
      : this(guid, lockedBy, lockedByFirstName, lockedByLastName, loginSessionID, logonServer, lockedFor, lockTime, LockInfo.LockOwnerLoggedOn.Unknown, exclusive, isSessionLess, lockedByList)
    {
    }

    public LockInfo(string guid)
      : this(guid, "", (string) null, (string) null, "", "", LoanInfo.LockReason.NotLocked, DateTime.MinValue, LockInfo.LockOwnerLoggedOn.False, LockInfo.ExclusiveLock.Nonexclusive)
    {
    }

    public LockInfo(string guid, string sessionID)
      : this(guid, "", (string) null, (string) null, sessionID, "", LoanInfo.LockReason.NotLocked, DateTime.MinValue, LockInfo.LockOwnerLoggedOn.False, LockInfo.ExclusiveLock.Nonexclusive)
    {
    }

    public string GUID => this.guid;

    public string LockedBy => this.lockedBy;

    public string LockedByFirstName => this.lockedByFirstName;

    public string LockedByLastName => this.lockedByLastName;

    public string LoginSessionID => this.loginSessionID;

    public string LogonServer => this.logonServer;

    public LockInfo.LockOwnerLoggedOn CurrentlyLoggedOn => this.isCurrentlyLoggedOn;

    public LoanInfo.LockReason LockedFor => this.lockedFor;

    public DateTime LockedSince => this.lockTime;

    public bool IsLocked => this.LockedFor != 0;

    public LockInfo.ExclusiveLock Exclusive => this.exclusive;

    public bool IsSessionLess => this.isSessionLess;

    public string[] LockedByList => this.lockedByList;

    public object Clone()
    {
      return (object) new LockInfo(this.guid, this.lockedBy, this.lockedByFirstName, this.lockedByLastName, this.loginSessionID, this.logonServer, this.lockedFor, this.lockTime, this.isCurrentlyLoggedOn, this.exclusive, lockedByList: this.lockedByList);
    }

    public object CloneForSessionLess()
    {
      Guid guid = Guid.NewGuid();
      this.exclusive = LockInfo.ExclusiveLock.NGSharedLock;
      return (object) new LockInfo(this.guid, this.lockedBy, this.lockedByFirstName, this.lockedByLastName, guid.ToString(), this.logonServer, this.lockedFor, this.lockTime, this.isCurrentlyLoggedOn, this.exclusive, true, this.lockedByList);
    }

    public enum ExclusiveLock
    {
      ReleaseExclusiveA = -2, // 0xFFFFFFFE
      ReleaseExclusive = -1, // 0xFFFFFFFF
      Nonexclusive = 0,
      Exclusive = 1,
      ExclusiveA = 2,
      Both = 3,
      NGSharedLock = 4,
    }

    public enum LockOwnerLoggedOn
    {
      Unknown = -1, // 0xFFFFFFFF
      False = 0,
      True = 1,
    }
  }
}
