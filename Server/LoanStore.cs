// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class LoanStore
  {
    private const string className = "LoanStore�";

    private LoanStore()
    {
    }

    public static Loan CheckOut(string guid)
    {
      if ((guid ?? "") == "")
        throw new ArgumentException("Guid cannot be blank or null", nameof (guid));
      ICacheLock<bool?> innerLock = HzcLoanLockFactory.UseHzcLock ? HzcLoanLockFactory.Instance.CheckOutLoan(nameof (LoanStore), guid) : ClientContext.GetCurrent().Cache.CheckOutWithNull<bool?>(nameof (LoanStore), guid, (object) guid);
      try
      {
        return new Loan(innerLock, false);
      }
      catch (Exception ex)
      {
        innerLock.UndoCheckout();
        Err.Reraise(nameof (LoanStore), ex);
        return (Loan) null;
      }
    }

    public static Loan CheckOutDraft(string guid)
    {
      ICacheLock<bool?> innerLock = !((guid ?? "") == "") ? ClientContext.GetCurrent().Cache.CheckOutWithNull<bool?>(nameof (LoanStore), guid, (object) guid) : throw new ArgumentException("Guid cannot be blank or null", nameof (guid));
      try
      {
        return new Loan(innerLock, true);
      }
      catch (Exception ex)
      {
        innerLock.UndoCheckout();
        Err.Reraise(nameof (LoanStore), ex);
        return (Loan) null;
      }
    }

    public static bool AuthCheck(UserInfo userInfo, string draftLoanGuid)
    {
      return Loan.AuthCheckDraftLoan(userInfo, draftLoanGuid);
    }

    public static Loan GetLatestVersion(string guid)
    {
      return !((guid ?? "") == "") ? new Loan(guid) : throw new ArgumentException("Guid cannot be blank or null", nameof (guid));
    }

    public static Loan GetLatestVersion(LoanIdentity identity) => new Loan(identity);

    public static Loan GetLatestVersion(LoanIdentity identity, bool exists)
    {
      return new Loan(identity, exists);
    }

    public static Loan GetVersion(string guid, int version)
    {
      if ((guid ?? "") == "")
        throw new ArgumentException("Guid cannot be blank or null", nameof (guid));
      return version >= 1 ? new Loan(guid, version) : throw new ArgumentException("Invalid loan version", nameof (version));
    }

    public static Loan GetVersion(LoanIdentity identity, int version, bool exists)
    {
      if (version < 1)
        throw new ArgumentException("Invalid loan version", nameof (version));
      return new Loan(identity, version, exists);
    }
  }
}
