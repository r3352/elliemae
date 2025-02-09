// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanLock
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Provides information about the current lock on a loan file.
  /// </summary>
  public class LoanLock : ILoanLock
  {
    private LockInfo lockInfo;

    internal LoanLock(LockInfo lockInfo) => this.lockInfo = lockInfo;

    /// <summary>
    /// Gets the ID of the user holding the current lock on the loan.
    /// </summary>
    public string LockedBy => this.lockInfo.LockedBy;

    /// <summary>Gets the date and time the lock was established.</summary>
    public DateTime LockedSince => this.lockInfo.LockedSince;

    /// <summary>Gets the type of lock being held.</summary>
    public LockType LockType => (LockType) this.lockInfo.LockedFor;

    /// <summary>Indicates if the lock is an exclusive lock.</summary>
    public bool Exclusive => LoanLock.IsExclusiveLock(this.lockInfo.Exclusive);

    /// <summary>Gets the SessionID of the user who owns the lock.</summary>
    public string SessionID => this.lockInfo.LoginSessionID;

    internal static bool IsExclusiveLock(LockInfo.ExclusiveLock exclusiveLock)
    {
      return exclusiveLock == LockInfo.ExclusiveLock.Exclusive || exclusiveLock == LockInfo.ExclusiveLock.ExclusiveA || exclusiveLock == LockInfo.ExclusiveLock.Both;
    }
  }
}
