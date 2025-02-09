// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class RateLockField : BankerEditionVirtualField
  {
    public const string FieldPrefix = "LOCKRATE";
    private string fieldID = string.Empty;
    private FieldDefinition baseField;
    private RateLockField.RateLockOrder lockOrder;

    public string BaseFieldID => this.fieldID;

    public RateLockField.RateLockOrder LockOrder => this.lockOrder;

    public RateLockField(FieldDefinition baseField)
      : base(RateLockField.GenerateRateLockFieldID(baseField.FieldID, RateLockField.RateLockOrder.MostRecent), "Rate Lock - " + baseField.Description + " (Most Recent)", baseField.Format)
    {
      this.fieldID = baseField.FieldID;
      this.baseField = baseField;
      this.lockOrder = RateLockField.RateLockOrder.MostRecent;
    }

    public RateLockField(FieldDefinition baseField, RateLockField.RateLockOrder lockOrder)
      : base(RateLockField.GenerateRateLockFieldID(baseField.FieldID, lockOrder), "Rate Lock - " + baseField.Description + " (Previous " + (object) (int) lockOrder + ")", baseField.Format)
    {
      this.fieldID = baseField.FieldID;
      this.baseField = baseField;
      this.lockOrder = lockOrder;
    }

    public RateLockField(
      FieldDefinition baseField,
      string description,
      RateLockField.RateLockOrder lockOrder)
      : base(RateLockField.GenerateRateLockFieldID(baseField.FieldID, lockOrder), description, baseField.Format)
    {
      this.fieldID = baseField.FieldID;
      this.baseField = baseField;
      this.lockOrder = lockOrder;
    }

    public RateLockField(
      FieldDefinition baseField,
      string readFieldID,
      string description,
      RateLockField.RateLockOrder lockOrder)
      : base(RateLockField.GenerateRateLockFieldID(readFieldID, lockOrder), description, baseField.Format)
    {
      this.fieldID = readFieldID;
      this.baseField = baseField;
      this.lockOrder = lockOrder;
    }

    public RateLockField(string fieldId, FieldDefinition baseField)
      : base(fieldId, "Rate Lock - " + baseField.Description, baseField.Format)
    {
      this.fieldID = baseField.FieldID;
      this.baseField = baseField;
      this.lockOrder = RateLockField.GetLockOrderForFieldID(fieldId);
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.RateLockFields;

    protected override string Evaluate(LoanData loan)
    {
      LockRequestLog lockRequestLog = this.getLockRequestLog(loan);
      if (lockRequestLog == null)
        return "";
      Hashtable lockRequestSnapshot = lockRequestLog.GetLockRequestSnapshot();
      return lockRequestSnapshot != null && lockRequestSnapshot.ContainsKey((object) this.fieldID) ? string.Concat(lockRequestSnapshot[(object) this.fieldID]) : "";
    }

    public override void SetValue(LoanData loan, string value)
    {
      LockRequestLog lockRequestLog = this.getLockRequestLog(loan);
      Hashtable lockRequestSnapshot = lockRequestLog != null ? lockRequestLog.GetLockRequestSnapshot() : throw new Exception("The lock request log for this virtual field value could not be found");
      if (lockRequestSnapshot == null)
        return;
      lockRequestSnapshot[(object) this.fieldID] = (object) value;
      lockRequestLog.AddLockRequestSnapshot(lockRequestSnapshot);
    }

    public override bool AllowEdit => true;

    private LockRequestLog getLockRequestLog(LoanData loan)
    {
      LockRequestLog[] allLockRequests = loan.GetLogList().GetAllLockRequests();
      Dictionary<string, LockConfirmLog> dictionary1 = new Dictionary<string, LockConfirmLog>();
      Dictionary<string, LockDenialLog> dictionary2 = new Dictionary<string, LockDenialLog>();
      int num = 0;
      switch (this.lockOrder)
      {
        case RateLockField.RateLockOrder.MostRecentlyConfirmed:
          foreach (LockConfirmLog allConfirmLock in loan.GetLogList().GetAllConfirmLocks())
            dictionary1[allConfirmLock.RequestGUID] = allConfirmLock;
          break;
        case RateLockField.RateLockOrder.MostRecentDenied:
        case RateLockField.RateLockOrder.PreviousDenied:
        case RateLockField.RateLockOrder.Previous2Denied:
          foreach (LockDenialLog lockDenialLog in loan.GetLogList().GetAllDenialLockLog())
            dictionary2[lockDenialLog.RequestGUID] = lockDenialLog;
          break;
      }
      for (int index = allLockRequests.Length - 1; index >= 0; --index)
      {
        switch (this.lockOrder)
        {
          case RateLockField.RateLockOrder.MostRecentRequest:
            if (this.lockOrder == RateLockField.RateLockOrder.MostRecentRequest && num == 0)
              return allLockRequests[index];
            break;
          case RateLockField.RateLockOrder.MostRecentlyConfirmed:
            if (dictionary1.ContainsKey(allLockRequests[index].Guid))
              return allLockRequests[index];
            break;
          case RateLockField.RateLockOrder.MostRecentDenied:
          case RateLockField.RateLockOrder.PreviousDenied:
          case RateLockField.RateLockOrder.Previous2Denied:
            if (allLockRequests[index].RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RequestDenied) && dictionary2.ContainsKey(allLockRequests[index].Guid))
            {
              if (this.lockOrder == RateLockField.RateLockOrder.MostRecentDenied && num == 0 || this.lockOrder == RateLockField.RateLockOrder.PreviousDenied && num == 1 || this.lockOrder == RateLockField.RateLockOrder.Previous2Denied && num == 2)
                return allLockRequests[index];
              ++num;
              break;
            }
            break;
          case RateLockField.RateLockOrder.MostRecentLockRequest:
          case RateLockField.RateLockOrder.PreviousLockRequest:
          case RateLockField.RateLockOrder.Previous2LockRequest:
            if (allLockRequests[index].RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested) || allLockRequests[index].RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.OldRequest))
            {
              if (this.lockOrder == RateLockField.RateLockOrder.MostRecentLockRequest && num == 0 || this.lockOrder == RateLockField.RateLockOrder.PreviousLockRequest && num == 1 || this.lockOrder == RateLockField.RateLockOrder.Previous2LockRequest && num == 2)
                return allLockRequests[index];
              ++num;
              break;
            }
            break;
          default:
            if (allLockRequests[index].RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked) || allLockRequests[index].RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.OldLock))
            {
              if (this.lockOrder == RateLockField.RateLockOrder.MostRecent && num == 0 || this.lockOrder == RateLockField.RateLockOrder.Previous && num == 1 || this.lockOrder == RateLockField.RateLockOrder.Previous2 && num == 2)
                return allLockRequests[index];
              ++num;
              break;
            }
            break;
        }
      }
      return (LockRequestLog) null;
    }

    public override FieldOptionCollection Options => this.baseField.Options;

    public override int ReportingDatabaseColumnSize => base.ReportingDatabaseColumnSize;

    public override ReportingDatabaseColumnType ReportingDatabaseColumnType
    {
      get => this.baseField.ReportingDatabaseColumnType;
    }

    private MilestoneLog getMilestoneLog(LoanData loan)
    {
      string.Concat(this.InstanceSpecifier);
      return loan.GetLogList().GetMilestoneByName(string.Concat(this.InstanceSpecifier));
    }

    public RateLockField.RateLockOrder GetRateLockOrder => this.lockOrder;

    public static RateLockField.RateLockOrder GetLockOrderForFieldID(string fieldId)
    {
      string str1 = fieldId.Substring("LOCKRATE".Length);
      string str2 = "";
      string str3;
      if (!str1.StartsWith("."))
      {
        str2 = str1.Substring(0, 1);
        str3 = str1.Substring(2);
      }
      else
        str3 = str1.Substring(1);
      string str4 = "";
      if (str3.IndexOf(".") > -1)
        str4 = str3.Substring(0, str3.IndexOf("."));
      switch (str4.ToUpper())
      {
        case "CONFIRMED":
          return RateLockField.RateLockOrder.MostRecentlyConfirmed;
        case "REQUEST":
          switch (str2)
          {
            case "1":
              return RateLockField.RateLockOrder.PreviousLockRequest;
            case "2":
              return RateLockField.RateLockOrder.Previous2LockRequest;
            default:
              return RateLockField.RateLockOrder.MostRecentLockRequest;
          }
        case "DENIED":
          switch (str2)
          {
            case "1":
              return RateLockField.RateLockOrder.PreviousDenied;
            case "2":
              return RateLockField.RateLockOrder.Previous2Denied;
            default:
              return RateLockField.RateLockOrder.MostRecentDenied;
          }
        default:
          switch (str2)
          {
            case "1":
              return RateLockField.RateLockOrder.Previous;
            case "2":
              return RateLockField.RateLockOrder.Previous2;
            case "3":
              return RateLockField.RateLockOrder.MostRecentRequest;
            default:
              return RateLockField.RateLockOrder.MostRecent;
          }
      }
    }

    public static bool IsRateLockField(string fieldId)
    {
      return fieldId.StartsWith("LOCKRATE", StringComparison.CurrentCultureIgnoreCase);
    }

    public static string GetBaseFieldIDForRateLockField(string rlFieldId)
    {
      string[] strArray = rlFieldId.StartsWith("LOCKRATE", StringComparison.CurrentCultureIgnoreCase) ? rlFieldId.Split('.') : throw new Exception("Field ID '" + rlFieldId + "' does not have the correct format for a rate lock field");
      if (strArray.Length < 2)
        throw new Exception("Field ID '" + rlFieldId + "' does not have the correct format for a rate lock field");
      return strArray.Length > 2 && (string.Compare(strArray[1], "CONFIRMED", true) == 0 || string.Compare(strArray[1], "DENIED", true) == 0 || string.Compare(strArray[1], "REQUEST", true) == 0) ? string.Join(".", strArray, 2, strArray.Length - 2) : string.Join(".", strArray, 1, strArray.Length - 1);
    }

    public static string GenerateRateLockFieldID(
      string baseFieldID,
      RateLockField.RateLockOrder lockOrder)
    {
      switch (lockOrder)
      {
        case RateLockField.RateLockOrder.MostRecent:
          return "LOCKRATE." + baseFieldID;
        case RateLockField.RateLockOrder.MostRecentlyConfirmed:
          return "LOCKRATE.CONFIRMED." + baseFieldID;
        case RateLockField.RateLockOrder.MostRecentDenied:
          return "LOCKRATE.DENIED." + baseFieldID;
        case RateLockField.RateLockOrder.PreviousDenied:
          return "LOCKRATE1.DENIED." + baseFieldID;
        case RateLockField.RateLockOrder.Previous2Denied:
          return "LOCKRATE2.DENIED." + baseFieldID;
        case RateLockField.RateLockOrder.MostRecentLockRequest:
          return "LOCKRATE.REQUEST." + baseFieldID;
        case RateLockField.RateLockOrder.PreviousLockRequest:
          return "LOCKRATE1.REQUEST." + baseFieldID;
        case RateLockField.RateLockOrder.Previous2LockRequest:
          return "LOCKRATE2.REQUEST." + baseFieldID;
        default:
          return "LOCKRATE" + (object) (int) lockOrder + "." + baseFieldID;
      }
    }

    public enum RateLockOrder
    {
      MostRecent,
      Previous,
      Previous2,
      MostRecentRequest,
      MostRecentlyConfirmed,
      MostRecentDenied,
      PreviousDenied,
      Previous2Denied,
      MostRecentLockRequest,
      PreviousLockRequest,
      Previous2LockRequest,
    }
  }
}
