// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IPostClosingCondition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("882E0810-3B5C-4a00-8AF2-A7FB8D466C7D")]
  public interface IPostClosingCondition
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

    string Recipient { get; set; }

    bool Expected { get; }

    object DateExpected { get; }

    bool Received { get; }

    object DateReceived { get; set; }

    string ReceivedBy { get; set; }

    bool Requested { get; }

    object DateRequested { get; set; }

    string RequestedBy { get; set; }

    bool Rerequested { get; }

    object DateRerequested { get; set; }

    string RerequestedBy { get; set; }

    bool Sent { get; }

    object DateSent { get; set; }

    string SentBy { get; set; }

    bool PrintInternally { get; set; }

    bool PrintExternally { get; set; }

    int DaysToReceive { get; set; }

    string RequestedFrom { get; set; }
  }
}
