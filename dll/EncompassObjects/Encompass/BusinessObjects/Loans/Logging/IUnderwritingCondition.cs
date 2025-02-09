// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IUnderwritingCondition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("39AA5E49-06D8-46fa-B22D-7B9B6792E376")]
  public interface IUnderwritingCondition
  {
    string ID { get; }

    LogEntryType EntryType { get; }

    Comments Comments { get; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    string Title { get; set; }

    ConditionType ConditionType { get; }

    string Description { get; set; }

    string Source { get; set; }

    string AddedBy { get; }

    DateTime DateAdded { get; }

    bool Cleared { get; }

    object DateCleared { get; set; }

    string ClearedBy { get; set; }

    BorrowerPair BorrowerPair { get; set; }

    ConditionStatus Status { get; }

    string Category { get; set; }

    string PriorTo { get; set; }

    bool Received { get; }

    object DateReceived { get; set; }

    string ReceivedBy { get; set; }

    bool Reviewed { get; }

    object DateReviewed { get; set; }

    string ReviewedBy { get; set; }

    bool Waived { get; }

    object DateWaived { get; set; }

    string WaivedBy { get; set; }

    Role ForRole { get; set; }

    bool AllowToClear { get; set; }

    bool Fulfilled { get; }

    object DateFulfilled { get; set; }

    string FulfilledBy { get; set; }

    bool Rejected { get; }

    object DateRejected { get; set; }

    string RejectedBy { get; set; }

    bool ForInternalUse { get; set; }

    bool ForExternalUse { get; set; }

    bool Requested { get; }

    object DateRequested { get; set; }

    string RequestedBy { get; set; }

    bool Rerequested { get; }

    object DateRerequested { get; set; }

    string RerequestedBy { get; set; }
  }
}
