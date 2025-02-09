// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockStatisticField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class RateLockStatisticField : BankerEditionVirtualField
  {
    private const string FieldPrefix = "LOCKRATE";
    private string virtualFieldID = string.Empty;
    private RateLockStatisticField.RateLockOrder lockOrder;

    public string VirtualFieldID => this.virtualFieldID;

    public RateLockStatisticField(
      string virtualFieldID,
      string description,
      FieldFormat fieldFormat)
      : base("LOCKRATE." + virtualFieldID, "Rate Lock - " + description, fieldFormat)
    {
      this.virtualFieldID = virtualFieldID;
      this.lockOrder = RateLockStatisticField.RateLockOrder.MostRecent;
    }

    public RateLockStatisticField(
      string virtualFieldID,
      string description,
      RateLockStatisticField.RateLockOrder lockOrder,
      FieldFormat fieldFormat)
      : base("LOCKRATE" + (object) (int) lockOrder + "." + virtualFieldID, "Rate Lock - " + description, fieldFormat)
    {
      this.virtualFieldID = virtualFieldID;
      this.lockOrder = lockOrder;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.RateLockFields;

    protected override string Evaluate(LoanData loan)
    {
      LockConfirmLog[] lockConfirmLogArray = (LockConfirmLog[]) null;
      LockConfirmLog lockConfirmLog = (LockConfirmLog) null;
      if (this.virtualFieldID != "REQUESTEDBY" && this.virtualFieldID != "CANCELLEDDATETIME")
      {
        lockConfirmLogArray = loan.GetLogList().GetAllConfirmLocks();
        if (lockConfirmLogArray == null || lockConfirmLogArray.Length == 0)
          return string.Empty;
      }
      int num1 = 0;
      string virtualFieldId = this.virtualFieldID;
      if (virtualFieldId == "CONFIRMEDREQUESTEDBY" || virtualFieldId == "DURATION" || virtualFieldId == "FULFILLEDDATETIME")
      {
        for (int index = lockConfirmLogArray.Length - 1; index >= 0; --index)
        {
          if (this.lockOrder == RateLockStatisticField.RateLockOrder.MostRecent && num1 == 0 || this.lockOrder == RateLockStatisticField.RateLockOrder.Previous && num1 == 1 || this.lockOrder == RateLockStatisticField.RateLockOrder.Previous2 && num1 == 2)
          {
            lockConfirmLog = lockConfirmLogArray[index];
            break;
          }
          ++num1;
        }
      }
      if ((this.virtualFieldID == "DURATION" || this.virtualFieldID == "FULFILLEDDATETIME") && lockConfirmLog == null)
        return "";
      if (this.virtualFieldID == "DURATION")
      {
        LockRequestLog lockRequest = loan.GetLogList().GetLockRequest(lockConfirmLog.RequestGUID);
        return lockRequest == null ? string.Empty : this.getDuration(lockRequest.DateTimeRequested, lockConfirmLog.GetSortDate()).ToString();
      }
      if (this.virtualFieldID == "FULFILLEDDATETIME")
        return lockConfirmLog.DateConfirmed;
      if (this.virtualFieldID == "AVERAGEDURATION")
      {
        int num2 = 0;
        int num3 = 0;
        for (int index = 0; index < lockConfirmLogArray.Length; ++index)
        {
          LockRequestLog lockRequest = loan.GetLogList().GetLockRequest(lockConfirmLogArray[index].RequestGUID);
          if (lockRequest != null)
          {
            ++num3;
            num2 += this.getDuration(lockRequest.DateTimeRequested, lockConfirmLogArray[index].GetSortDate());
          }
        }
        if (num3 == 0)
          return string.Empty;
        double num4 = (double) (num2 / num3);
        return num4 != 0.0 ? num4.ToString("N2") : "0";
      }
      if (this.virtualFieldID == "CONFIRMEDREQUESTEDBY")
      {
        if (lockConfirmLog != null && (lockConfirmLog.RequestGUID ?? "") != string.Empty)
        {
          LockRequestLog lockRequest = loan.GetLogList().GetLockRequest(lockConfirmLog.RequestGUID);
          if (lockRequest != null)
            return lockRequest.RequestedFullName;
        }
      }
      else if (this.virtualFieldID == "REQUESTEDBY")
      {
        LockRequestLog[] allLockRequests = loan.GetLogList().GetAllLockRequests();
        if (allLockRequests != null && allLockRequests.Length != 0)
        {
          for (int index = allLockRequests.Length - 1; index >= 0; --index)
          {
            if (this.lockOrder == RateLockStatisticField.RateLockOrder.MostRecent && num1 == 0 || this.lockOrder == RateLockStatisticField.RateLockOrder.Previous && num1 == 1 || this.lockOrder == RateLockStatisticField.RateLockOrder.Previous2 && num1 == 2)
              return allLockRequests[index].RequestedFullName;
            ++num1;
          }
        }
      }
      else if (this.virtualFieldID == "CANCELLEDDATETIME")
      {
        LockCancellationLog lockCancellation = loan.GetLogList().GetMostRecentLockCancellation();
        return lockCancellation == null || lockCancellation.Date == DateTime.MinValue ? string.Empty : lockCancellation.Date.ToString("MM/dd/yyyy") + " " + lockCancellation.TimeCancelled.ToString();
      }
      return "";
    }

    private int getDuration(DateTime request, DateTime confirmed)
    {
      TimeSpan timeSpan = confirmed - request;
      return timeSpan.Minutes <= 0 ? 0 : timeSpan.Minutes;
    }

    public RateLockStatisticField.RateLockOrder GetRateLockOrder => this.lockOrder;

    public enum RateLockOrder
    {
      MostRecent,
      Previous,
      Previous2,
    }
  }
}
