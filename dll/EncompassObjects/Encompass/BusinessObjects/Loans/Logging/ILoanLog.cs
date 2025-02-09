// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILoanLog
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("6E8735EF-1C79-4413-85AB-7EBC589CC2B6")]
  public interface ILoanLog
  {
    LogMilestoneEvents MilestoneEvents { get; }

    LogTrackedDocuments TrackedDocuments { get; }

    LogConversations Conversations { get; }

    LogEntryList GetLogEntrySequence(LogEntryType entryTypes);

    LogReceivedDownloads ReceivedDownloads { get; }

    LogEDMTransactions EDMTransactions { get; }

    LogStatusOnlineUpdates StatusOnlineUpdates { get; }

    LogUnderwritingConditions UnderwritingConditions { get; }

    LogPostClosingConditions PostClosingConditions { get; }

    LogEntry GetEntryByID(string entryId);

    LogLockRequests LockRequests { get; }

    LogLockConfirmations LockConfirmations { get; }

    LogLockDenials LockDenials { get; }

    LogPrintEvents PrintEvents { get; }

    LogMilestoneTasks MilestoneTasks { get; }

    LogPreliminaryConditions PreliminaryConditions { get; }

    LogHtmlEmailMessages HtmlEmailMessages { get; }

    LogDisclosures Disclosures { get; }

    LogDisclosures2015 Disclosures2015 { get; }
  }
}
