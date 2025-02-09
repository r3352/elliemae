// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LockException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [ComVisible(false)]
  public class LockException : ApplicationException
  {
    private LockException innerException;
    private LoanLock lockInfo;

    internal LockException(LockException ex)
      : base(((Exception) ex).Message)
    {
      this.innerException = ex;
      this.lockInfo = new LoanLock(ex.LockInfo);
      this.HResult = -2147212784;
    }

    public LoanLock CurrentLock => this.lockInfo;
  }
}
