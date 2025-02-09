// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LockException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Exception class that represents errors when attempting to lock an object that is
  /// already locked by another user.
  /// </summary>
  [ComVisible(false)]
  public class LockException : ApplicationException
  {
    private EllieMae.EMLite.ClientServer.Exceptions.LockException innerException;
    private LoanLock lockInfo;

    internal LockException(EllieMae.EMLite.ClientServer.Exceptions.LockException ex)
      : base(ex.Message)
    {
      this.innerException = ex;
      this.lockInfo = new LoanLock(ex.LockInfo);
      this.HResult = -2147212784;
    }

    /// <summary>
    /// Gets the User ID of the user which currently holds a lock on this object.
    /// </summary>
    public LoanLock CurrentLock => this.lockInfo;
  }
}
