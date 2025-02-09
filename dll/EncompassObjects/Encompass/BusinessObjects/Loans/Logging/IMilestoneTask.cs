// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IMilestoneTask
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("19704180-E81A-4295-B3AA-4322E2D5A4B4")]
  public interface IMilestoneTask
  {
    string ID { get; }

    object Date { get; }

    LogEntryType EntryType { get; }

    string Comments { get; set; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    string Name { get; set; }

    string Description { get; set; }

    string AddedBy { get; }

    DateTime DateAdded { get; }

    TaskPriority Priority { get; set; }

    bool Completed { get; }

    object DateCompleted { get; set; }

    string CompletedBy { get; set; }

    MilestoneEvent MilestoneEvent { get; set; }

    MilestoneTaskContacts Contacts { get; }
  }
}
