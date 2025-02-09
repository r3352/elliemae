// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanStatusMap
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanStatusMap
  {
    public static string[] LoanStatusStrings = new string[9]
    {
      "Active Loan",
      "Loan Originated",
      "Application approved but not accepted",
      "Application denied",
      "Application withdrawn",
      "File Closed for incompleteness",
      "Loan purchased by your institution",
      "Preapproval request denied by financial institution",
      "Preapproval request approved but not accepted"
    };

    private LoanStatusMap()
    {
    }

    public static LoanStatusMap.LoanStatus GetLoanStatusEnum(string str)
    {
      switch (str)
      {
        case "":
          return LoanStatusMap.LoanStatus.ActiveLoan;
        case "Active Loan":
          return LoanStatusMap.LoanStatus.ActiveLoan;
        case "Application approved but not accepted":
          return LoanStatusMap.LoanStatus.AppApprovedNotAccepted;
        case "Application denied":
          return LoanStatusMap.LoanStatus.AppDenied;
        case "Application withdrawn":
          return LoanStatusMap.LoanStatus.AppWithdrawn;
        case "File Closed for incompleteness":
          return LoanStatusMap.LoanStatus.FileClosed;
        case "Loan Originated":
          return LoanStatusMap.LoanStatus.LoanOriginated;
        case "Loan purchased by your institution":
          return LoanStatusMap.LoanStatus.LoanPurchased;
        case "Preapproval request approved but not accepted":
          return LoanStatusMap.LoanStatus.PreapprovalReqApprovedNotAccepted;
        case "Preapproval request denied by financial institution":
          return LoanStatusMap.LoanStatus.PreapprovalReqDenied;
        default:
          throw new Exception("Invalid loan status: " + str);
      }
    }

    public static LoanStatusMap.LoanStatus[] GetAdverseStatuses()
    {
      return new LoanStatusMap.LoanStatus[6]
      {
        LoanStatusMap.LoanStatus.AppApprovedNotAccepted,
        LoanStatusMap.LoanStatus.AppDenied,
        LoanStatusMap.LoanStatus.AppWithdrawn,
        LoanStatusMap.LoanStatus.FileClosed,
        LoanStatusMap.LoanStatus.PreapprovalReqDenied,
        LoanStatusMap.LoanStatus.PreapprovalReqApprovedNotAccepted
      };
    }

    public static LoanStatusMap.LoanStatus[] GetNonAdverseStatuses()
    {
      return new LoanStatusMap.LoanStatus[3]
      {
        LoanStatusMap.LoanStatus.ActiveLoan,
        LoanStatusMap.LoanStatus.LoanOriginated,
        LoanStatusMap.LoanStatus.LoanPurchased
      };
    }

    public static bool IsAdverseStatus(string status)
    {
      return LoanStatusMap.IsAdverseStatus(LoanStatusMap.GetLoanStatusEnum(status));
    }

    public static bool IsAdverseStatus(LoanStatusMap.LoanStatus status)
    {
      foreach (LoanStatusMap.LoanStatus nonAdverseStatuse in LoanStatusMap.GetNonAdverseStatuses())
      {
        if (status == nonAdverseStatuse)
          return false;
      }
      return true;
    }

    public enum LoanStatus
    {
      ActiveLoan,
      LoanOriginated,
      AppApprovedNotAccepted,
      AppDenied,
      AppWithdrawn,
      FileClosed,
      LoanPurchased,
      PreapprovalReqDenied,
      PreapprovalReqApprovedNotAccepted,
    }
  }
}
