// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IPreliminaryCondition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for EDMTransaction class.</summary>
  /// <exclude />
  [Guid("FDBC36B4-00BF-4d30-9A47-6DE8572C11BF")]
  public interface IPreliminaryCondition
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

    bool Fulfilled { get; }

    object DateFulfilled { get; set; }

    string FulfilledBy { get; set; }

    BorrowerPair BorrowerPair { get; set; }

    ConditionStatus Status { get; }

    bool Received { get; }

    bool Requested { get; }

    bool Rerequested { get; }

    object DateExpected { get; }

    string ReceivedBy { get; set; }

    object DateReceived { get; set; }

    string RequestedBy { get; set; }

    object DateRequested { get; set; }

    string RequestedFrom { get; }

    string RerequestedBy { get; set; }

    object DateRerequested { get; set; }

    int DaysToReceive { get; set; }
  }
}
