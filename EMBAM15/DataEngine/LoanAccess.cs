// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanAccess
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Text;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanAccess
  {
    public static string GetAccessRightMessage(LoanContentAccess loancontentAccess)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if ((loancontentAccess & LoanContentAccess.DocumentTracking) == LoanContentAccess.DocumentTracking)
        stringBuilder.AppendLine("eFolder Documents and Files");
      else if ((loancontentAccess & LoanContentAccess.DocTrackingPartial) == LoanContentAccess.DocTrackingPartial)
      {
        if ((loancontentAccess & LoanContentAccess.DocTrackingUnassignedFiles) == LoanContentAccess.DocTrackingUnassignedFiles)
          stringBuilder.AppendLine("Unassigned Files");
        if ((loancontentAccess & LoanContentAccess.DocTrackingCreateDocs) == LoanContentAccess.DocTrackingCreateDocs)
          stringBuilder.AppendLine("Create eFolder Documents");
        if ((loancontentAccess & LoanContentAccess.DocTrackingUnprotectedDocs) == LoanContentAccess.DocTrackingUnprotectedDocs)
          stringBuilder.AppendLine("Unprotected eFolder Documents");
        if ((loancontentAccess & LoanContentAccess.DocTrackingProtectedDocs) == LoanContentAccess.DocTrackingProtectedDocs)
          stringBuilder.AppendLine("Protected eFolder Documents");
        if ((loancontentAccess & LoanContentAccess.DocTrackingOrderDisclosures) == LoanContentAccess.DocTrackingOrderDisclosures)
          stringBuilder.AppendLine("Order eDisclosures");
        if ((loancontentAccess & LoanContentAccess.DocTrackingRequestRetrieveBorrower) == LoanContentAccess.DocTrackingRequestRetrieveBorrower)
          stringBuilder.AppendLine("Requesting and Retrieving Borrower Documents");
        if ((loancontentAccess & LoanContentAccess.DocTrackingRequestRetrieveService) == LoanContentAccess.DocTrackingRequestRetrieveService)
          stringBuilder.AppendLine("Requesting and Retrieving ICE Mortgage Technology Network Services");
      }
      if ((loancontentAccess & LoanContentAccess.ConditionTracking) == LoanContentAccess.ConditionTracking)
        stringBuilder.AppendLine("eFolder Underwriting Conditions");
      if ((loancontentAccess & LoanContentAccess.ConversationLog) == LoanContentAccess.ConversationLog)
        stringBuilder.AppendLine("Conversation Log");
      if ((loancontentAccess & LoanContentAccess.Task) == LoanContentAccess.Task)
        stringBuilder.AppendLine("Tasks");
      if ((loancontentAccess & LoanContentAccess.ProfitManagement) == LoanContentAccess.ProfitManagement)
        stringBuilder.AppendLine("Profit Management");
      if ((loancontentAccess & LoanContentAccess.LockRequest) == LoanContentAccess.LockRequest)
        stringBuilder.AppendLine("Lock Requests");
      if ((loancontentAccess & LoanContentAccess.FormFields) == LoanContentAccess.FormFields)
        stringBuilder.AppendLine("Some Form Fields");
      if ((loancontentAccess & LoanContentAccess.DisclosureTracking) == LoanContentAccess.DisclosureTracking)
        stringBuilder.AppendLine("Disclosure Tracking");
      return stringBuilder.ToString();
    }
  }
}
