// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockLastActionTimeField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class RateLockLastActionTimeField : BankerEditionVirtualField
  {
    public RateLockLastActionTimeField()
      : base("LOCKRATE.LASTACTIONTIME", "Last Lock Request Action Time", FieldFormat.DATETIME)
    {
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.RateLockFields;

    protected override string Evaluate(LoanData loan)
    {
      DateTime dateTime = DateTime.MinValue;
      foreach (LockRequestLog lockRequestLog in loan.GetLogList().GetAllRecordsOfType(typeof (LockRequestLog)))
      {
        if (lockRequestLog.DateTimeRequested > dateTime)
          dateTime = lockRequestLog.DateTimeRequested;
      }
      foreach (LockConfirmLog lockConfirmLog in loan.GetLogList().GetAllRecordsOfType(typeof (LockConfirmLog)))
      {
        if (lockConfirmLog.DateTimeConfirmed > dateTime)
          dateTime = lockConfirmLog.DateTimeConfirmed;
      }
      foreach (LockDenialLog lockDenialLog in loan.GetLogList().GetAllRecordsOfType(typeof (LockDenialLog)))
      {
        if (lockDenialLog.DateTimeDenied > dateTime)
          dateTime = lockDenialLog.DateTimeDenied;
      }
      foreach (LockCancellationLog lockCancellationLog in loan.GetLogList().GetAllRecordsOfType(typeof (LockCancellationLog)))
      {
        if (lockCancellationLog.DateTimeCancelled > dateTime)
          dateTime = lockCancellationLog.DateTimeCancelled;
      }
      return dateTime == DateTime.MinValue ? "" : dateTime.ToString("M/d/yyyy h:mm:ss tt");
    }
  }
}
