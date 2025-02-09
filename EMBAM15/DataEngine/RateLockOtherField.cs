// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RateLockOtherField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class RateLockOtherField : BankerEditionVirtualField
  {
    private const string FieldPrefix = "LOCKRATE";
    private string virtualLockFieldID = string.Empty;

    public string VirtualLockFieldID => this.virtualLockFieldID;

    public RateLockOtherField(string fieldID, string fieldDescription, FieldFormat fieldFormat)
      : base("LOCKRATE." + fieldID, "Rate Lock - " + fieldDescription, fieldFormat)
    {
      this.virtualLockFieldID = fieldID;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.RateLockFields;

    protected override string Evaluate(LoanData loan)
    {
      try
      {
        LogList logList = loan.GetLogList();
        if (this.virtualLockFieldID == "CONFIRMATIONCOUNT")
          return string.Concat((object) this.GetConfirmationCount(logList));
        if (this.virtualLockFieldID == "DENIALCOUNT")
          return string.Concat((object) logList.GetAllRecordsOfType(typeof (LockDenialLog)).Length);
        LockRequestLog[] lockRequestLogArray = (LockRequestLog[]) null;
        if (logList != null)
          lockRequestLogArray = logList.GetAllLockRequests();
        int num = 0;
        string virtualLockFieldId = this.virtualLockFieldID;
        // ISSUE: reference to a compiler-generated method
        switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(virtualLockFieldId))
        {
          case 13120460:
            if (virtualLockFieldId == "DENIALCOMMENTS")
              goto label_30;
            else
              goto label_42;
          case 83776599:
            if (virtualLockFieldId == "CURRENTDATETIME")
              return lockRequestLogArray == null || lockRequestLogArray.Length == 0 ? "" : lockRequestLogArray[lockRequestLogArray.Length - 1].Date.ToString("MM/dd/yyyy") + " " + lockRequestLogArray[lockRequestLogArray.Length - 1].TimeRequested;
            goto label_42;
          case 942266183:
            if (virtualLockFieldId == "DENIALCOMMENTS1")
              goto label_30;
            else
              goto label_42;
          case 959043802:
            if (virtualLockFieldId == "DENIALCOMMENTS2")
              goto label_30;
            else
              goto label_42;
          case 1316542747:
            if (virtualLockFieldId == "BUYLOCKCOUNT")
              break;
            goto label_42;
          case 2817489097:
            if (virtualLockFieldId == "SELLLOCKCOUNT")
              break;
            goto label_42;
          case 3951759225:
            if (virtualLockFieldId == "REQUESTCOUNT")
              return string.Concat((object) lockRequestLogArray.Length);
            goto label_42;
          default:
            goto label_42;
        }
        string empty = string.Empty;
        for (int index = 0; index < lockRequestLogArray.Length; ++index)
        {
          if (lockRequestLogArray[index].RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked) || lockRequestLogArray[index].RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.OldLock))
          {
            Hashtable lockRequestSnapshot = lockRequestLogArray[index].GetLockRequestSnapshot();
            if (lockRequestSnapshot != null)
            {
              if (this.virtualLockFieldID == "BUYLOCKCOUNT" && lockRequestSnapshot.ContainsKey((object) "2151"))
              {
                string str = lockRequestSnapshot[(object) "2151"].ToString();
                if (str != string.Empty && str != "//")
                  ++num;
              }
              else if (this.virtualLockFieldID == "SELLLOCKCOUNT" && lockRequestSnapshot.ContainsKey((object) "2222"))
              {
                string str = lockRequestSnapshot[(object) "2222"].ToString();
                if (str != string.Empty && str != "//")
                  ++num;
              }
            }
          }
        }
        return string.Concat((object) num);
label_30:
        LockDenialLog[] allRecordsOfType = (LockDenialLog[]) logList.GetAllRecordsOfType(typeof (LockDenialLog));
        if (allRecordsOfType == null || allRecordsOfType.Length == 0)
          return "";
        if (this.virtualLockFieldID == "DENIALCOMMENTS1")
          return allRecordsOfType.Length > 1 ? allRecordsOfType[allRecordsOfType.Length - 2].Comments : string.Empty;
        if (!(this.virtualLockFieldID == "DENIALCOMMENTS2"))
          return allRecordsOfType[allRecordsOfType.Length - 1].Comments;
        return allRecordsOfType.Length > 2 ? allRecordsOfType[allRecordsOfType.Length - 3].Comments : string.Empty;
      }
      catch
      {
      }
label_42:
      return "";
    }

    private int GetConfirmationCount(LogList logList)
    {
      int confirmationCount = 0;
      foreach (LockConfirmLog lockConfirmLog in logList.GetAllRecordsOfType(typeof (LockConfirmLog)))
      {
        if (lockConfirmLog.IncludeConfirmCnt)
          ++confirmationCount;
      }
      return confirmationCount;
    }
  }
}
