// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanLock
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanLock : ILoanLock
  {
    private LockInfo lockInfo;

    internal LoanLock(LockInfo lockInfo) => this.lockInfo = lockInfo;

    public string LockedBy => this.lockInfo.LockedBy;

    public DateTime LockedSince => this.lockInfo.LockedSince;

    public LockType LockType => (LockType) this.lockInfo.LockedFor;

    public bool Exclusive => LoanLock.IsExclusiveLock(this.lockInfo.Exclusive);

    public string SessionID => this.lockInfo.LoginSessionID;

    internal static bool IsExclusiveLock(LockInfo.ExclusiveLock exclusiveLock)
    {
      return exclusiveLock == 1 || exclusiveLock == 2 || exclusiveLock == 3;
    }
  }
}
