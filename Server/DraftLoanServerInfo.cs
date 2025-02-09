// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.DraftLoanServerInfo
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class DraftLoanServerInfo : ICloneable
  {
    private Hashtable userRightPairs;
    private string loanGUID;
    private Dictionary<string, LockInfo> currentLocks = new Dictionary<string, LockInfo>();
    private DateTime lastModified = DateTime.MinValue;
    private bool lockChanged = true;
    public readonly List<string> Changes = new List<string>();

    public LockInfo[] CurrentLocks
    {
      get
      {
        LockInfo[] currentLocks = new LockInfo[this.currentLocks.Count];
        int num = 0;
        foreach (LockInfo lockInfo in this.currentLocks.Values)
          currentLocks[num++] = lockInfo;
        return currentLocks;
      }
    }

    public DraftLoanServerInfo(string guid)
    {
      this.loanGUID = guid;
      this.userRightPairs = new Hashtable();
      this.currentLocks.Add("", new LockInfo(guid));
    }

    public DraftLoanServerInfo(DraftLoanServerInfo source)
    {
      this.loanGUID = source.LoanGUID;
      this.userRightPairs = (Hashtable) source.userRightPairs.Clone();
      foreach (string key in source.currentLocks.Keys)
        this.currentLocks.Add(key, (LockInfo) source.currentLocks[key].Clone());
      foreach (string change in source.Changes)
        this.Changes.Add(change);
      this.lastModified = source.lastModified;
      this.lockChanged = source.lockChanged;
    }

    public string LoanGUID => this.loanGUID;

    public Hashtable UserRightPairs
    {
      get => this.userRightPairs;
      set => this.userRightPairs = value;
    }

    public bool Locked => this.currentLocks.Count != 1 || !this.currentLocks.ContainsKey("");

    public bool IsLockedBySession(string sessionID)
    {
      foreach (string key in this.currentLocks.Keys)
      {
        if (key == sessionID)
          return true;
      }
      return false;
    }

    public string[] GetLockedBySessions()
    {
      List<string> stringList = new List<string>();
      foreach (string key in this.currentLocks.Keys)
      {
        if (!stringList.Contains(key))
          stringList.Add(key);
      }
      return stringList.ToArray();
    }

    public string[] GetLockedByUsers()
    {
      List<string> stringList = new List<string>();
      foreach (string key in this.currentLocks.Keys)
      {
        if (!stringList.Contains(this.currentLocks[key].LockedBy))
          stringList.Add(this.currentLocks[key].LockedBy);
      }
      return stringList.ToArray();
    }

    public LockInfo GetLockInfo(string sessionID)
    {
      if (!this.Locked)
        return new LockInfo(this.loanGUID);
      return this.currentLocks.ContainsKey(sessionID) ? this.currentLocks[sessionID] : new LockInfo(this.loanGUID, sessionID);
    }

    public void AddLockInfo(LockInfo lockInfo)
    {
      if (this.currentLocks.ContainsKey(lockInfo.LoginSessionID))
        this.currentLocks.Remove(lockInfo.LoginSessionID);
      if (lockInfo.LoginSessionID != "" && this.currentLocks.ContainsKey(""))
        this.currentLocks.Remove("");
      this.currentLocks.Add(lockInfo.LoginSessionID, lockInfo);
      if (!ClientContext.GetCurrent().AllowConcurrentEditing)
        this.Changes.Clear();
      else if (this.Changes.Contains(""))
        this.Changes.Remove("");
      this.Changes.Add(lockInfo.LoginSessionID);
      this.lockChanged = true;
    }

    public void RemoveLockInfo(string sessionID)
    {
      if (sessionID == null)
      {
        foreach (string key in this.currentLocks.Keys)
        {
          if (key != "")
            this.Changes.Add(key);
        }
        this.currentLocks.Clear();
      }
      else if (this.currentLocks.ContainsKey(sessionID))
      {
        this.Changes.Add(sessionID);
        this.currentLocks.Remove(sessionID);
      }
      if (this.currentLocks.Count == 0)
        this.currentLocks.Add("", new LockInfo(this.loanGUID));
      this.lockChanged = true;
    }

    public void Unlock()
    {
      foreach (string key in this.currentLocks.Keys)
      {
        if (key != "")
          this.Changes.Add(key);
      }
      this.currentLocks.Clear();
      this.currentLocks.Add("", new LockInfo(this.loanGUID));
      this.Changes.Clear();
      this.Changes.Add("");
      this.lockChanged = true;
    }

    public bool IsLockModified
    {
      get => this.lockChanged;
      set => this.lockChanged = value;
    }

    public DateTime LastModified
    {
      get => this.lastModified;
      set => this.lastModified = value;
    }

    public void ClearModifiedRecords()
    {
      this.Changes.Clear();
      this.lockChanged = false;
    }

    public LoanInfo.Right GetRight(string userid)
    {
      return !this.userRightPairs.ContainsKey((object) userid) ? LoanInfo.Right.NoRight : (LoanInfo.Right) this.userRightPairs[(object) userid];
    }

    public bool HasRight(string userid, LoanInfo.Right rights)
    {
      return (this.GetRight(userid) & rights) == rights;
    }

    public bool HasFullRight(string userid) => this.HasRight(userid, LoanInfo.Right.FullRight);

    public void AddRight(string userid, LoanInfo.Right right)
    {
      if (right == LoanInfo.Right.NoRight)
        return;
      if (!this.userRightPairs.ContainsKey((object) userid))
        this.userRightPairs[(object) userid] = (object) right;
      else
        this.userRightPairs[(object) userid] = (object) ((LoanInfo.Right) this.userRightPairs[(object) userid] | right);
    }

    public void SetRight(string userid, LoanInfo.Right right)
    {
      this.userRightPairs[(object) userid] = (object) right;
      if ((LoanInfo.Right) this.userRightPairs[(object) userid] != LoanInfo.Right.NoRight)
        return;
      this.userRightPairs.Remove((object) userid);
    }

    public void RemoveRight(string userid, LoanInfo.Right right)
    {
      if (right == LoanInfo.Right.NoRight || !this.userRightPairs.ContainsKey((object) userid))
        return;
      this.userRightPairs[(object) userid] = (object) (int) ((LoanInfo.Right) this.userRightPairs[(object) userid] & ((LoanInfo.Right) 255 ^ right));
      if ((LoanInfo.Right) this.userRightPairs[(object) userid] != LoanInfo.Right.NoRight)
        return;
      this.userRightPairs.Remove((object) userid);
    }

    public object Clone() => (object) new DraftLoanServerInfo(this);
  }
}
