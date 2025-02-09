// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockRequestStatusField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class RateLockRequestStatusField : BankerEditionVirtualField
  {
    public const string NoRequest = "NoRequest";
    public const string Requested = "Requested";
    public const string Locked = "Locked";
    public const string Confirmed = "Confirmed";
    public const string Denied = "Denied";
    private FieldOptionCollection options;

    public RateLockRequestStatusField()
      : base("LOCKRATE.REQUESTSTATUS", "Rate Lock Request Status", FieldFormat.STRING)
    {
      this.options = new FieldOptionCollection(new List<FieldOption>()
      {
        new FieldOption("Not Requested", nameof (NoRequest)),
        new FieldOption(nameof (Requested), nameof (Requested)),
        new FieldOption(nameof (Locked), nameof (Locked)),
        new FieldOption(nameof (Confirmed), nameof (Confirmed)),
        new FieldOption(nameof (Denied), nameof (Denied))
      }.ToArray(), true);
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.RateLockFields;

    public override FieldOptionCollection Options => this.options;

    protected override string Evaluate(LoanData loan)
    {
      LogList logList = loan.GetLogList();
      LockRequestLog recentLockRequest = logList.GetMostRecentLockRequest();
      if (recentLockRequest == null)
        return "NoRequest";
      if (recentLockRequest.LockRequestStatus == RateLockRequestStatus.Requested)
        return "Requested";
      if (recentLockRequest.LockRequestStatus == RateLockRequestStatus.RequestDenied)
        return "Denied";
      if (recentLockRequest.LockRequestStatus != RateLockRequestStatus.RateLocked)
        return "NoRequest";
      LockConfirmLog confirmLockLog = logList.GetConfirmLockLog();
      return confirmLockLog != null && confirmLockLog.RequestGUID == recentLockRequest.Guid ? "Confirmed" : "Locked";
    }
  }
}
