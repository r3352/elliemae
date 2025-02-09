// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.RateLockEnum
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class RateLockEnum
  {
    public static string GetRateLockStatusString(RateLockRequestStatus status)
    {
      string lockStatusString = "";
      switch (status)
      {
        case RateLockRequestStatus.Requested:
          lockStatusString = "Requested";
          break;
        case RateLockRequestStatus.RateExpired:
          lockStatusString = "Expired";
          break;
        case RateLockRequestStatus.OldLock:
          lockStatusString = "Old Lock";
          break;
        case RateLockRequestStatus.RateLocked:
          lockStatusString = "Locked";
          break;
        case RateLockRequestStatus.OldRequest:
          lockStatusString = "Old Request";
          break;
        case RateLockRequestStatus.RequestDenied:
          lockStatusString = "Denied";
          break;
        case RateLockRequestStatus.Registered:
          lockStatusString = "Registered";
          break;
        case RateLockRequestStatus.OldRegistered:
          lockStatusString = "Old Registration";
          break;
        case RateLockRequestStatus.ExtensionRequested:
          lockStatusString = "Extension Requested";
          break;
        case RateLockRequestStatus.Cancelled:
          lockStatusString = "Cancelled";
          break;
        case RateLockRequestStatus.NotLocked:
          lockStatusString = "Not Locked";
          break;
        case RateLockRequestStatus.Voided:
          lockStatusString = "Voided";
          break;
        case RateLockRequestStatus.OldVoid:
          lockStatusString = "Old Void";
          break;
      }
      return lockStatusString;
    }

    public static string GetRateLockStatusNum(RateLockRequestStatus status)
    {
      string rateLockStatusNum = "";
      switch (status)
      {
        case RateLockRequestStatus.Requested:
          rateLockStatusNum = "1";
          break;
        case RateLockRequestStatus.RateExpired:
          rateLockStatusNum = "2";
          break;
        case RateLockRequestStatus.OldLock:
          rateLockStatusNum = "3";
          break;
        case RateLockRequestStatus.RateLocked:
          rateLockStatusNum = "4";
          break;
        case RateLockRequestStatus.OldRequest:
          rateLockStatusNum = "5";
          break;
        case RateLockRequestStatus.RequestDenied:
          rateLockStatusNum = "6";
          break;
        case RateLockRequestStatus.Registered:
          rateLockStatusNum = "7";
          break;
        case RateLockRequestStatus.OldRegistered:
          rateLockStatusNum = "8";
          break;
        case RateLockRequestStatus.ExtensionRequested:
          rateLockStatusNum = "9";
          break;
        case RateLockRequestStatus.Cancelled:
          rateLockStatusNum = "10";
          break;
        case RateLockRequestStatus.NotLocked:
          rateLockStatusNum = "11";
          break;
        case RateLockRequestStatus.Voided:
          rateLockStatusNum = "12";
          break;
        case RateLockRequestStatus.OldVoid:
          rateLockStatusNum = "13";
          break;
      }
      return rateLockStatusNum;
    }

    public static RateLockRequestStatus GetRateLockEnum(string status)
    {
      switch (status)
      {
        case "Cancelled":
          return RateLockRequestStatus.Cancelled;
        case "Denied":
          return RateLockRequestStatus.RequestDenied;
        case "Expired":
          return RateLockRequestStatus.RateExpired;
        case "Extension Requested":
          return RateLockRequestStatus.ExtensionRequested;
        case "Locked":
          return RateLockRequestStatus.RateLocked;
        case "NotLocked":
          return RateLockRequestStatus.NotLocked;
        case "Old Lock":
          return RateLockRequestStatus.OldLock;
        case "Old Registration":
          return RateLockRequestStatus.OldRegistered;
        case "Old Request":
          return RateLockRequestStatus.OldRequest;
        case "Old Void":
          return RateLockRequestStatus.OldVoid;
        case "Registered":
          return RateLockRequestStatus.Registered;
        case "Requested":
          return RateLockRequestStatus.Requested;
        case "Voided":
          return RateLockRequestStatus.Voided;
        default:
          return RateLockRequestStatus.NoRequest;
      }
    }
  }
}
