// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IMilestoneEvent
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for MilestoneEvent class.</summary>
  /// <exclude />
  [Guid("F987695E-1D0B-4bcb-B1F6-52C26CA9FBFB")]
  public interface IMilestoneEvent
  {
    string ID { get; }

    object Date { get; set; }

    LogEntryType EntryType { get; }

    string Comments { get; set; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    void SetDate(object newDate);

    string MilestoneName { get; }

    bool Completed { get; set; }

    void AdjustDate(
      object newDate,
      bool allowAdjustPastMilestones,
      bool allowAdjustFutureMilestones);

    LoanAssociate LoanAssociate { get; }
  }
}
